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
using System.IO;
using System.Runtime.CompilerServices;
using MiNET.Blocks;
using MiNET.Client;
using MiNET.Utils;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.AgentClient
{
	/// <summary>
	///     Block reads over what the wire delivered: the client's chunk cache holds version-9
	///     section payloads, and this decodes each one to a 4096-cell runtime id grid on first
	///     read. The decoded grid is keyed on the payload array itself, so a section that arrives
	///     again as new bytes is decoded again and nothing needs invalidating.
	///     <para>
	///         Block updates after delivery (UpdateBlock) land in an overlay of edited sections
	///         that shadows the delivered ones, so a build in progress renders as it stands
	///         without a rejoin. A section the server never delivered (all air above a cleared
	///         plot) is created in the overlay the moment a block is set in it.
	///     </para>
	/// </summary>
	public class ClientWorld
	{
		private readonly MiNetClient _client;
		private readonly ConditionalWeakTable<byte[], int[]> _decoded = new ConditionalWeakTable<byte[], int[]>();
		private readonly ConcurrentDictionary<(int X, int Y, int Z), int[]> _edited = new ConcurrentDictionary<(int, int, int), int[]>();

		/// <summary>The runtime id of air, the fill of a section created by an edit.</summary>
		private readonly int _air = BlockFactory.GetBlockByName("minecraft:air")?.GetRuntimeId() ?? 0;

		public ClientWorld(MiNetClient client)
		{
			_client = client;
		}

		public int EditedSectionCount => _edited.Count;

		/// <summary>A section's runtime ids, or null when the client holds nothing for it. Cell index is (x &lt;&lt; 8) | (z &lt;&lt; 4) | y.</summary>
		public int[] GetSection(int chunkX, int sectionY, int chunkZ)
		{
			if (_edited.TryGetValue((chunkX, sectionY, chunkZ), out int[] edited)) return edited;
			return GetDeliveredSection(chunkX, sectionY, chunkZ);
		}

		private int[] GetDeliveredSection(int chunkX, int sectionY, int chunkZ)
		{
			var coordinates = new ChunkCoordinates(chunkX, chunkZ);

			if (_client.ChunkCache.TryGetColumn(coordinates, out CachedChunkColumn column) && column.Sections.TryGetValue(sectionY, out byte[] payload))
			{
				return _decoded.GetValue(payload, Decode);
			}

			// The legacy flow decodes straight into a ChunkColumn; no client of ours runs it against
			// this server, but a picture of a legacy column should still be a picture.
			if (_client.Chunks.TryGetValue(coordinates, out ChunkColumn legacy) && legacy != null)
			{
				int storage = sectionY - ChunkColumn.WorldMinY / 16;
				if (storage < 0 || storage >= ChunkColumn.WorldHeight / 16) return null;
				SubChunk sub = legacy[storage];
				if (sub == null) return null;

				var grid = new int[4096];
				for (int x = 0; x < 16; x++)
				for (int z = 0; z < 16; z++)
				for (int y = 0; y < 16; y++)
					grid[(x << 8) | (z << 4) | y] = sub.GetBlockRuntimeId(x, y, z);
				return grid;
			}

			return null;
		}

		public int GetRuntimeId(int x, int y, int z)
		{
			int[] grid = GetSection(x >> 4, y >> 4, z >> 4);
			if (grid == null) return -1;
			return grid[((x & 15) << 8) | ((z & 15) << 4) | (y & 15)];
		}

		/// <summary>An UpdateBlock landed: shadow the delivered section with an edited copy.</summary>
		public void SetRuntimeId(int x, int y, int z, int runtimeId)
		{
			var key = (x >> 4, y >> 4, z >> 4);
			int[] grid = _edited.GetOrAdd(key, k =>
			{
				int[] delivered = GetDeliveredSection(k.Item1, k.Item2, k.Item3);
				var copy = new int[4096];
				if (delivered != null) Array.Copy(delivered, copy, 4096);
				else Array.Fill(copy, _air);
				return copy;
			});
			grid[((x & 15) << 8) | ((z & 15) << 4) | (y & 15)] = runtimeId;
		}

		/// <summary>The column was delivered afresh: the wire's copy is the truth again.</summary>
		public void ForgetEdits(int chunkX, int chunkZ)
		{
			foreach (var key in _edited.Keys)
			{
				if (key.X == chunkX && key.Z == chunkZ) _edited.TryRemove(key, out _);
			}
		}

		public int ColumnCount => _client.ChunkCache.Columns.Count;

		/// <summary>
		///     Storage 0 of a version-9 section payload: the block layer. Storage 1 is the liquid
		///     layer and is not drawn. Palette entries are runtime ids when the server speaks palette
		///     indices, network hashes otherwise, and either way they resolve to our runtime ids.
		/// </summary>
		private int[] Decode(byte[] data)
		{
			var grid = new int[4096];
			if (data == null || data.Length == 0) return grid;

			var stream = new MemoryStream(data);
			int version = stream.ReadByte();
			int storageCount = stream.ReadByte();
			if (version >= 9) stream.ReadByte(); // y index
			if (storageCount < 1) return grid;

			int flags = stream.ReadByte();
			bool isRuntime = (flags & 1) != 0;
			int bitsPerBlock = flags >> 1;
			if (!isRuntime) throw new InvalidDataException("Persistent (NBT) palette in a network section");

			int[] indices = null;
			if (bitsPerBlock > 0)
			{
				indices = new int[4096];
				int blocksPerWord = 32 / bitsPerBlock;
				int wordCount = (4096 + blocksPerWord - 1) / blocksPerWord;
				int mask = (1 << bitsPerBlock) - 1;
				Span<byte> word = stackalloc byte[4];
				int cell = 0;
				for (int w = 0; w < wordCount; w++)
				{
					if (stream.Read(word) != 4) throw new InvalidDataException("Section words truncated");
					uint value = BitConverter.ToUInt32(word);
					for (int j = 0; j < blocksPerWord && cell < 4096; j++, cell++)
					{
						indices[cell] = (int) ((value >> (j * bitsPerBlock)) & mask);
					}
				}
			}

			int paletteCount = VarInt.ReadSInt32(stream);
			var palette = new int[paletteCount];
			for (int j = 0; j < paletteCount; j++)
			{
				int entry = VarInt.ReadSInt32(stream);
				palette[j] = _client.BlockNetworkIdsAreHashes ? BlockFactory.GetRuntimeIdFromNetworkId(unchecked((uint) entry)) : entry;
			}

			if (indices == null)
			{
				Array.Fill(grid, paletteCount > 0 ? palette[0] : 0);
				return grid;
			}

			for (int i = 0; i < 4096; i++)
			{
				int index = indices[i];
				if (index >= paletteCount) throw new InvalidDataException($"Palette index {index} outside a palette of {paletteCount}");
				grid[i] = palette[index];
			}

			return grid;
		}
	}
}