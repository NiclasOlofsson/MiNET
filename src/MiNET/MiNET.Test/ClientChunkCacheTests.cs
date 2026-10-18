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

using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiNET.Client;
using MiNET.Net;
using MiNET.Utils.Vectors;

namespace MiNET.Test
{
	/// <summary>
	///     The client's half of chunk delivery. What matters here is not that the packets parse, it
	///     is that a client ends up HOLDING the terrain: the announcement flow only ever names blobs
	///     by hash, so a client that answers the verdicts without keeping the payloads reports hits
	///     for bytes it does not have and the server, correctly, never sends them again. That is the
	///     failure this exists to prevent, in both delivery flows.
	/// </summary>
	[TestClass]
	public class ClientChunkCacheTests
	{
		private const ulong BiomeHash = 0x1111;
		private const ulong SectionHash = 0x2222;

		/// <summary>The join flow: skeleton, request, sub-chunk answers, and the bytes behind the hashes.</summary>
		[TestMethod]
		public void PullFlowAssemblesColumnFromMissResponses()
		{
			var cache = new ClientChunkCache();

			var hits = new List<ulong>();
			var misses = new List<ulong>();

			bool needsRequest = cache.OnLevelChunk(Skeleton(0, 0, BiomeHash, limit: 5), hits, misses);

			Assert.IsTrue(needsRequest, "a skeleton carries no block data, so the client owes a sub-chunk request");
			CollectionAssert.AreEqual(new List<ulong> {BiomeHash}, misses, "a cold client holds nothing, so the biome blob is a miss");
			Assert.AreEqual(0, hits.Count);

			cache.SectionsRequested(new ChunkCoordinates(0, 0), 1);

			hits.Clear();
			misses.Clear();
			cache.OnSubChunkResponse(SubChunkResponse(0, 0, sectionY: 4, SectionHash, blockEntities: new byte[] {7}), hits, misses);

			CollectionAssert.AreEqual(new List<ulong> {SectionHash}, misses, "the section's blob is announced by the sub-chunk answer, not by the skeleton");

			Assert.IsTrue(cache.TryGetColumn(new ChunkCoordinates(0, 0), out CachedChunkColumn column));
			Assert.IsFalse(column.IsComplete, "both payloads are still in flight");

			cache.OnBlobPayloads(new Dictionary<ulong, byte[]> {{BiomeHash, new byte[] {1}}, {SectionHash, new byte[] {2}}});

			CollectionAssert.AreEqual(new byte[] {1}, column.Biomes);
			CollectionAssert.AreEqual(new byte[] {2}, column.Sections[4]);
			CollectionAssert.AreEqual(new byte[] {7}, column.SectionTails[4], "block entities ride beside the blob id, never inside the blob");
			Assert.IsTrue(column.IsComplete);
		}

		/// <summary>
		///     The whole point of a content-addressed cache: the second column to name a blob is
		///     answered from memory. Reporting the hit is only half of it - the payload has to land in
		///     the new column too, or the client has announced terrain it can never draw.
		/// </summary>
		[TestMethod]
		public void HeldBlobFillsTheNextColumnWithoutAskingAgain()
		{
			var cache = new ClientChunkCache();

			var hits = new List<ulong>();
			var misses = new List<ulong>();
			cache.OnLevelChunk(Skeleton(0, 0, BiomeHash, limit: 5), hits, misses);
			cache.OnBlobPayloads(new Dictionary<ulong, byte[]> {{BiomeHash, new byte[] {1}}});

			hits.Clear();
			misses.Clear();
			cache.OnLevelChunk(Skeleton(1, 0, BiomeHash, limit: 5), hits, misses);

			CollectionAssert.AreEqual(new List<ulong> {BiomeHash}, hits, "the same biome payload in a neighbouring column is a hit");
			Assert.AreEqual(0, misses.Count, "asking again for bytes we hold is the round trip the cache exists to remove");

			Assert.IsTrue(cache.TryGetColumn(new ChunkCoordinates(1, 0), out CachedChunkColumn column));
			CollectionAssert.AreEqual(new byte[] {1}, column.Biomes, "a hit has to fill the column from the cache; the server will not send those bytes");
		}

		/// <summary>Cached push: every section announced up front, nothing for the client to request.</summary>
		[TestMethod]
		public void PushFlowAssemblesColumnWithoutRequesting()
		{
			var cache = new ClientChunkCache();

			var hits = new List<ulong>();
			var misses = new List<ulong>();

			bool needsRequest = cache.OnLevelChunk(CachedPush(0, 0, new List<ulong> {0xA1, 0xA2}, BiomeHash), hits, misses);

			Assert.IsFalse(needsRequest, "a pushed column is complete on the wire; the client asks for nothing");
			CollectionAssert.AreEqual(new List<ulong> {0xA1, 0xA2, BiomeHash}, misses);

			cache.OnBlobPayloads(new Dictionary<ulong, byte[]> {{0xA1, new byte[] {1}}, {0xA2, new byte[] {2}}, {BiomeHash, new byte[] {3}}});

			Assert.IsTrue(cache.TryGetColumn(new ChunkCoordinates(0, 0), out CachedChunkColumn column));
			CollectionAssert.AreEqual(new byte[] {1}, column.Sections[CachedChunkColumn.LowestSectionY], "hashes are ordered bottom-up from the world's lowest section");
			CollectionAssert.AreEqual(new byte[] {2}, column.Sections[CachedChunkColumn.LowestSectionY + 1]);
			CollectionAssert.AreEqual(new byte[] {3}, column.Biomes, "the biome blob is the last hash, after the sections subChunkCount counts");
			Assert.IsTrue(column.IsComplete);
		}

		/// <summary>
		///     The sliding window: columns are forgotten with the server's own disc so a re-entered
		///     one arrives as a fresh skeleton and dances again. The blob cache is deliberately NOT
		///     windowed, which is what makes walking back over old ground cost round trips instead of
		///     terrain.
		/// </summary>
		[TestMethod]
		public void ForgottenColumnRedancesButItsBlobsStillHit()
		{
			var cache = new ClientChunkCache();

			var hits = new List<ulong>();
			var misses = new List<ulong>();
			cache.OnLevelChunk(Skeleton(20, 0, BiomeHash, limit: 5), hits, misses);
			cache.OnBlobPayloads(new Dictionary<ulong, byte[]> {{BiomeHash, new byte[] {1}}});

			// Walked away: the column is outside the disc around the player now.
			cache.Forget(new ChunkCoordinates(0, 0), radiusChunks: 8);
			Assert.IsFalse(cache.TryGetColumn(new ChunkCoordinates(20, 0), out _));

			hits.Clear();
			misses.Clear();
			bool needsRequest = cache.OnLevelChunk(Skeleton(20, 0, BiomeHash, limit: 5), hits, misses);

			Assert.IsTrue(needsRequest, "a re-entered column is new to the window and runs the full dance again");
			CollectionAssert.AreEqual(new List<ulong> {BiomeHash}, hits, "the blob cache survives the window, so the re-dance costs no terrain");
		}

		/// <summary>
		///     The load-test mode, which is the only client that is allowed to lie about holding
		///     anything: verdicts still go out, so the server's cache path is exercised, but no bytes
		///     and no columns are kept.
		/// </summary>
		[TestMethod]
		public void UntrackedModeAnswersVerdictsAndKeepsNothing()
		{
			var cache = new ClientChunkCache {TrackColumns = false};
			cache.Blobs.KeepPayloads = false;

			var hits = new List<ulong>();
			var misses = new List<ulong>();
			cache.OnLevelChunk(Skeleton(0, 0, BiomeHash, limit: 5), hits, misses);
			cache.OnBlobPayloads(new Dictionary<ulong, byte[]> {{BiomeHash, new byte[] {1}}});

			CollectionAssert.AreEqual(new List<ulong> {BiomeHash}, misses);
			Assert.IsFalse(cache.TryGetColumn(new ChunkCoordinates(0, 0), out _), "a bot holds no columns");
			Assert.IsFalse(cache.Blobs.TryGetPayload(BiomeHash, out _), "and no payloads");

			hits.Clear();
			misses.Clear();
			cache.OnLevelChunk(Skeleton(1, 0, BiomeHash, limit: 5), hits, misses);

			CollectionAssert.AreEqual(new List<ulong> {BiomeHash}, hits, "the hash is still remembered, so the server is not asked twice");
		}

		private static McpeLevelChunk Skeleton(int x, int z, ulong biomeHash, int limit)
		{
			var packet = McpeLevelChunk.CreateObject();
			packet.chunkPosition = new ChunkPos {x = x, z = z};
			packet.dimension = 0;
			packet.subChunkCount = 0;
			packet.cacheEnabled = true;
			packet.cacheMetadata = new List<ulong> {biomeHash};
			packet.chunkData = new byte[] {0};
			packet.clientRequestSubChunkLimit = limit;
			return packet;
		}

		private static McpeLevelChunk CachedPush(int x, int z, List<ulong> sectionHashes, ulong biomeHash)
		{
			var packet = McpeLevelChunk.CreateObject();
			packet.chunkPosition = new ChunkPos {x = x, z = z};
			packet.dimension = 0;
			packet.subChunkCount = (uint) sectionHashes.Count;
			packet.cacheEnabled = true;
			packet.cacheMetadata = new List<ulong>(sectionHashes) {biomeHash};
			packet.chunkData = new byte[] {0};
			return packet;
		}

		private static McpeSubChunkPacket SubChunkResponse(int x, int z, int sectionY, ulong blobId, byte[] blockEntities)
		{
			var packet = McpeSubChunkPacket.CreateObject();
			packet.cacheEnabled = true;
			packet.dimensionType = 0;
			packet.centerPos = new SubChunkPos {subchunkPositionX = x, subchunkPositionY = 0, subchunkPositionZ = z};
			packet.subChunkData = new List<SubChunkPacketData>
			{
				new SubChunkPacketData
				{
					subChunkPosOffset = new SubChunkPosOffset {subchunkOffsetX = 0, subchunkOffsetY = (sbyte) sectionY, subchunkOffsetZ = 0},
					subChunkRequestResult = SubChunkPacketData.SubChunkRequestResult.Success,
					blobId = blobId,
					serializedSubChunk = blockEntities
				}
			};
			return packet;
		}
	}
}