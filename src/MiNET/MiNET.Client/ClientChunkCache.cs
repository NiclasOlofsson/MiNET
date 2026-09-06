#region LICENSE

// The contents of this file are subject to the Common Public Attribution
// License Version 1.0. (the "License"); you may not use this file except in
// compliance with the License. You may obtain a copy of the License at
// https://github.com/NiclasOlofsson/MiNET/blob/master/LICENSE.
// The License is based on the Mozilla Public License Version 1.1, but Sections 14
// and 15 have been added to cover use of software over a computer network and
// provide for limited attribution for the Original Developer. In addition, Exhibit A has
// been modified to be consistent with Exhibit B.
//
// Software distributed under the License is distributed on an "AS IS" basis,
// WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License for
// the specific language governing rights and limitations under the License.
//
// The Original Code is MiNET.
//
// The Original Developer is the Initial Developer.  The Initial Developer of
// the Original Code is Niclas Olofsson.
//
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2026 Niclas Olofsson.
// All Rights Reserved.

#endregion

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using MiNET.Net;
using MiNET.Utils.Vectors;

namespace MiNET.Client
{
	/// <summary>
	///     The blobs this client holds, content addressed exactly as the server stores them: the
	///     hash is the payload's whole identity, so a blob kept from an earlier column, an earlier
	///     join or a different server answers a later announcement with nothing on the wire. In
	///     memory only; a real client keeps this on disk and nothing here needs that yet.
	///     <para>
	///         KeepPayloads off is the load-test mode: hashes are remembered and the bytes dropped,
	///         so a fleet runs the whole protocol dance without holding a world per bot.
	///     </para>
	///     <para>
	///         Settable on the cache so a fleet can point every client at ONE shared store: hashes
	///         are content addressed and client agnostic, so sharing makes the fleet look like a
	///         warm returning population instead of a thousand cold first joins.
	///     </para>
	/// </summary>
	public class BlobCache
	{
		private readonly ConcurrentDictionary<ulong, byte[]> _blobs = new ConcurrentDictionary<ulong, byte[]>();

		public bool KeepPayloads { get; set; } = true;

		/// <summary>Hashes known, whether or not their bytes have arrived yet.</summary>
		public int Count => _blobs.Count;

		/// <summary>Hashes whose bytes are actually held. Below <see cref="Count" /> means payloads are still in flight, or dropped.</summary>
		public int PayloadCount
		{
			get
			{
				int held = 0;
				foreach (KeyValuePair<ulong, byte[]> blob in _blobs)
				{
					if (blob.Value != null) held++;
				}

				return held;
			}
		}

		/// <summary>
		///     Whether this hash can be answered as a hit. True from the moment a miss has been
		///     reported for it: the payload is already on its way, and reporting the same hash as a
		///     miss twice makes the server send those bytes twice.
		/// </summary>
		public bool Contains(ulong hash) => _blobs.ContainsKey(hash);

		/// <summary>The payload, when we hold the bytes and not just the hash.</summary>
		public bool TryGetPayload(ulong hash, out byte[] payload)
		{
			return _blobs.TryGetValue(hash, out payload) && payload != null;
		}

		/// <summary>Records the hash as ours before its bytes arrive. See <see cref="Contains" />.</summary>
		public void MarkRequested(ulong hash) => _blobs.TryAdd(hash, null);

		/// <summary>A payload as it lands in a ClientCacheMissResponse.</summary>
		public void Store(ulong hash, byte[] payload) => _blobs[hash] = KeepPayloads ? payload : null;

		public void Clear() => _blobs.Clear();
	}

	/// <summary>
	///     One column as the wire delivers it: payloads, not blocks. Turning these into a world is
	///     the consumer's job (<see cref="ClientUtils.DecodeChunkColumn" /> reads one section's
	///     version-9 payload), which is what lets a bot run the same flow and keep nothing.
	/// </summary>
	/// <summary>Which flow delivered a column. A server can use both in one session, and does.</summary>
	public enum ChunkDelivery
	{
		/// <summary>Skeleton plus sub-chunk requests: the join burst, where the client's own selectivity tames a cold radius.</summary>
		Pull,

		/// <summary>One LevelChunk announcing every section by hash: the rim delta once a player is walking, where they take the whole column anyway.</summary>
		Push,

		/// <summary>Everything inline in one payload, no hashes. Only from a server serving a client that declined the cache.</summary>
		Legacy
	}

	public class CachedChunkColumn
	{
		/// <summary>
		///     The lowest sub-chunk index, so a section's absolute index maps to a storage slot.
		///     Fixed: ChunkColumn.WorldMinY is -64 for every dimension in this codebase.
		/// </summary>
		public const int LowestSectionY = -4;

		public CachedChunkColumn(ChunkCoordinates coordinates, int dimension)
		{
			Coordinates = coordinates;
			Dimension = dimension;
		}

		public ChunkCoordinates Coordinates { get; }
		public int Dimension { get; }

		/// <summary>How this one arrived. Consumers do not need it to draw the column, but a session
		/// that mixes the flows is invisible without it.</summary>
		public ChunkDelivery Delivery { get; set; }

		/// <summary>The biome payload: its own blob in both cached forms, inline in the legacy one.</summary>
		public byte[] Biomes { get; set; }

		/// <summary>Border blocks and the column's block entities. Always inline, never blobbed.</summary>
		public byte[] Tail { get; set; }

		/// <summary>
		///     The whole column in one payload, legacy push only: <see cref="SubChunkCount" />
		///     sections, biomes and the tail, exactly as DecodeChunkColumn reads them.
		/// </summary>
		public byte[] LegacyPayload { get; set; }

		public int SubChunkCount { get; set; }

		/// <summary>Version-9 section payloads by absolute sub-chunk index (<see cref="LowestSectionY" /> is the lowest).</summary>
		public ConcurrentDictionary<int, byte[]> Sections { get; } = new ConcurrentDictionary<int, byte[]>();

		/// <summary>
		///     Per-section block entities. In the cached flow they travel inline beside the blob id
		///     rather than inside the blob, so a chest arrives as a block AND as the entity that
		///     draws it.
		/// </summary>
		public ConcurrentDictionary<int, byte[]> SectionTails { get; } = new ConcurrentDictionary<int, byte[]>();

		/// <summary>Sections asked for and not yet answered, plus blob payloads announced and not yet delivered.</summary>
		public int Outstanding;

		/// <summary>A skeleton has landed and its sub-chunk request has not gone out yet.</summary>
		public bool AwaitingRequest;

		/// <summary>Nothing is on its way any more: every piece this column was promised is here.</summary>
		public bool IsComplete => !AwaitingRequest && Outstanding == 0;
	}

	/// <summary>
	///     The client's chunk state: the blob cache plus the columns being assembled out of it.
	///     Both delivery flows land here, which is the point of it.
	///     <list type="bullet">
	///         <item>
	///             Pull (the join burst): a skeleton LevelChunk announces the biome blob, the client
	///             asks for the sections it wants, and each SubChunkPacket entry announces that
	///             section's blob.
	///         </item>
	///         <item>
	///             Push: one LevelChunk announces every section's blob plus the biome blob, and the
	///             client asks for nothing.
	///         </item>
	///     </list>
	///     Either way an announcement is answered hit or miss, and a miss comes back as bytes in a
	///     ClientCacheMissResponse, which is where the terrain actually arrives.
	///     <para>
	///         The columns are a sliding window, kept identical to the server's: <see cref="Forget" />
	///         drops what falls outside the disc around the player, so a re-entered column arrives as
	///         a fresh skeleton and dances again while its blobs, which are never forgotten, all hit.
	///     </para>
	/// </summary>
	public class ClientChunkCache
	{
		private readonly object _sync = new object();
		private readonly HashSet<ChunkCoordinates> _known = new HashSet<ChunkCoordinates>();
		private readonly ConcurrentDictionary<ChunkCoordinates, CachedChunkColumn> _columns = new ConcurrentDictionary<ChunkCoordinates, CachedChunkColumn>();

		/// <summary>Columns waiting on a blob, by hash. One blob is shared by many columns: every empty section in the world is the same payload.</summary>
		private readonly ConcurrentDictionary<ulong, List<(CachedChunkColumn Column, int SectionY)>> _waiting = new ConcurrentDictionary<ulong, List<(CachedChunkColumn, int)>>();

		private const int BiomeSlot = int.MinValue;

		public BlobCache Blobs { get; set; } = new BlobCache();

		/// <summary>
		///     Off (the bot fleet): columns are remembered as coordinates only, so the dance runs
		///     without the payloads or the per-column bookkeeping a rendering client needs. On (the
		///     default): every column is assembled and handed to <see cref="ColumnUpdated" />.
		/// </summary>
		public bool TrackColumns { get; set; } = true;

		/// <summary>Raised whenever a piece of a column lands. Check <see cref="CachedChunkColumn.IsComplete" /> for the last one.</summary>
		public event Action<CachedChunkColumn> ColumnUpdated;

		public ICollection<CachedChunkColumn> Columns => _columns.Values;

		public bool TryGetColumn(ChunkCoordinates coordinates, out CachedChunkColumn column) => _columns.TryGetValue(coordinates, out column);

		/// <summary>
		///     A LevelChunk in any of its three forms. Fills <paramref name="hits" /> and
		///     <paramref name="misses" /> with the verdicts owed for the hashes it announces, and
		///     returns true when this column still needs its sub-chunk request sent.
		/// </summary>
		public bool OnLevelChunk(McpeLevelChunk message, List<ulong> hits, List<ulong> misses)
		{
			var coordinates = new ChunkCoordinates(message.chunkPosition.x, message.chunkPosition.z);

			// Verdicts are owed for every announcement, including a re-push of a column we hold:
			// the server is waiting on an answer for those hashes either way.
			bool firstTime;
			lock (_sync) firstTime = _known.Add(coordinates);

			CachedChunkColumn column = null;
			if (TrackColumns && firstTime)
			{
				column = new CachedChunkColumn(coordinates, message.dimension);
				_columns[coordinates] = column;
			}
			else if (TrackColumns)
			{
				_columns.TryGetValue(coordinates, out column);
			}

			if (column != null)
			{
				column.Delivery = message.clientRequestSubchunkLimit != null ? ChunkDelivery.Pull
					: message.cacheEnabled ? ChunkDelivery.Push
					: ChunkDelivery.Legacy;
			}

			if (message.cacheEnabled)
			{
				// Section blobs bottom-up, the biome blob last, with subChunkCount counting only the
				// sections: the skeleton counts none and announces the biome blob alone.
				int sections = (int) message.subChunkCount;
				for (int i = 0; i < message.cacheMetadata.Count; i++)
				{
					ulong hash = message.cacheMetadata[i];
					int slot = i < sections ? CachedChunkColumn.LowestSectionY + i : BiomeSlot;
					Announce(hash, column, slot, hits, misses);
				}

				if (column != null) column.Tail = message.chunkData;
			}
			else if (column != null && message.clientRequestSubchunkLimit == null)
			{
				// Legacy push: sections, biomes and tail in one payload, nothing addressed by hash.
				column.LegacyPayload = message.chunkData;
				column.SubChunkCount = (int) message.subChunkCount;
			}

			// A skeleton is only half a column, so it does not count as complete until the request
			// it demands has gone out and the answers are outstanding.
			bool needsRequest = message.clientRequestSubchunkLimit != null && firstTime;
			if (column != null && needsRequest) column.AwaitingRequest = true;

			Updated(column);
			return needsRequest;
		}

		/// <summary>
		///     The answers to a sub-chunk request. Cached, an entry announces the section's blob and
		///     carries its block entities inline; uncached, the entry carries the section payload
		///     itself. Both forms land in the same slot.
		/// </summary>
		public void OnSubChunkResponse(McpeSubChunkPacket message, List<ulong> hits, List<ulong> misses)
		{
			if (message.subchunkData == null) return;

			foreach (SubChunkPacketData entry in message.subchunkData)
			{
				var coordinates = new ChunkCoordinates(
					message.centerPos.subchunkPositionX + entry.subchunkPosOffset.subchunkOffsetX,
					message.centerPos.subchunkPositionZ + entry.subchunkPosOffset.subchunkOffsetZ);
				int sectionY = message.centerPos.subchunkPositionY + entry.subchunkPosOffset.subchunkOffsetY;

				CachedChunkColumn column = null;
				if (TrackColumns) _columns.TryGetValue(coordinates, out column);

				// Answered either way: a rejection, an all-air section and a real one all close one
				// of the requests this column is waiting on.
				Delivered(column);

				if (message.cacheEnabled)
				{
					if (entry.serializedSubChunk != null && column != null) column.SectionTails[sectionY] = entry.serializedSubChunk;

					// All-air and rejected entries carry no blob.
					if (entry.blobId is not ulong blobId) continue;

					Announce(blobId, column, sectionY, hits, misses);
				}
				else if (column != null && entry.serializedSubChunk != null)
				{
					column.Sections[sectionY] = entry.serializedSubChunk;
				}

				Updated(column);
			}
		}

		/// <summary>The bytes we reported as missing, which is where terrain actually arrives.</summary>
		public void OnBlobPayloads(IDictionary<ulong, byte[]> blobs)
		{
			if (blobs == null) return;

			foreach (KeyValuePair<ulong, byte[]> blob in blobs)
			{
				Blobs.Store(blob.Key, blob.Value);

				if (!_waiting.TryRemove(blob.Key, out List<(CachedChunkColumn Column, int SectionY)> waiters)) continue;

				lock (waiters)
				{
					foreach ((CachedChunkColumn column, int sectionY) in waiters)
					{
						Fill(column, sectionY, blob.Value);
						Delivered(column);
						Updated(column);
					}
				}
			}
		}

		/// <summary>The sections a request just asked this column for, which it now owes answers to.</summary>
		public void SectionsRequested(ChunkCoordinates coordinates, int count)
		{
			if (!TrackColumns || !_columns.TryGetValue(coordinates, out CachedChunkColumn column)) return;

			Interlocked.Add(ref column.Outstanding, count);
			column.AwaitingRequest = false;
		}

		/// <summary>
		///     The sliding window, pruned to the same disc the server prunes: everything outside is
		///     forgotten completely, so re-entering it means a fresh skeleton and a full dance. The
		///     blobs are untouched, which is what makes walking back over old ground free.
		/// </summary>
		public void Forget(ChunkCoordinates center, int radiusChunks)
		{
			lock (_sync)
			{
				_known.RemoveWhere(c =>
				{
					if (c.IsWithinView(center, radiusChunks)) return false;

					_columns.TryRemove(c, out _);
					return true;
				});
			}
		}

		public void Clear()
		{
			lock (_sync) _known.Clear();
			_columns.Clear();
			_waiting.Clear();
		}

		/// <summary>
		///     One announced hash: hit when we hold it or have already asked for it, miss otherwise,
		///     and either way the column's slot is filled now or registered to be filled when the
		///     payload lands.
		/// </summary>
		private void Announce(ulong hash, CachedChunkColumn column, int sectionY, List<ulong> hits, List<ulong> misses)
		{
			if (Blobs.TryGetPayload(hash, out byte[] payload))
			{
				hits.Add(hash);
				Fill(column, sectionY, payload);
				return;
			}

			if (Blobs.Contains(hash)) hits.Add(hash);
			else
			{
				misses.Add(hash);
				Blobs.MarkRequested(hash);
			}

			if (column == null) return;

			// Reported as a hit while the bytes are still in flight for another column, or reported
			// as a miss just now: both mean this slot is waiting on the same payload.
			Interlocked.Increment(ref column.Outstanding);
			List<(CachedChunkColumn, int)> waiters = _waiting.GetOrAdd(hash, _ => new List<(CachedChunkColumn, int)>());
			lock (waiters) waiters.Add((column, sectionY));
		}

		private static void Fill(CachedChunkColumn column, int sectionY, byte[] payload)
		{
			if (column == null || payload == null) return;

			if (sectionY == BiomeSlot) column.Biomes = payload;
			else column.Sections[sectionY] = payload;
		}

		private static void Delivered(CachedChunkColumn column)
		{
			if (column != null && column.Outstanding > 0) Interlocked.Decrement(ref column.Outstanding);
		}

		private void Updated(CachedChunkColumn column)
		{
			if (column != null) ColumnUpdated?.Invoke(column);
		}
	}
}