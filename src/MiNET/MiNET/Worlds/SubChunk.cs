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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2020 Niclas Olofsson.
// All Rights Reserved.

#endregion

using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using log4net;
using MiNET.Blocks;
using MiNET.Utils;

namespace MiNET.Worlds
{
	public class SubChunk : IDisposable, ICloneable
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(SubChunk));

		private bool _isAllAir = true;

		private List<int> _runtimeIds; // Add air, always as first (performance)
		private List<int> _loggedRuntimeIds;
		private List<int> _biomeIds;

		private short[] _blocks;
		private byte[] _loggedBlocks; // We use only byte size on this palette index table, because can basically only be water and snow-levels
		private byte[] _biomes;

		// Consider disabling these if we don't calculate lights
		private NibbleArray _blockLight;
		private NibbleArray _skyLight;

		private byte[] _cache;

		public int X { get; set; }
		public int Z { get; set; }
		public int Index { get; set; }

		internal List<int> RuntimeIds => _runtimeIds;
		internal List<int> LoggedRuntimeIds => _loggedRuntimeIds;
		internal List<int> BiomeIds => _biomeIds;

		internal short[] Blocks => _blocks;
		internal byte[] LoggedBlocks => _loggedBlocks;
		internal virtual byte[] Biomes => _biomes;

		public NibbleArray BlockLight => _blockLight;
		public NibbleArray SkyLight => _skyLight;

		public bool IsDirty { get; private set; }

		public ulong Hash { get; set; }
		public bool DisableCache { get; set; } = true;

		public SubChunk()
		{
			_runtimeIds = new List<int> { new Air().GetRuntimeId() };
			_loggedRuntimeIds = new List<int> { new Air().GetRuntimeId() };
			_biomeIds = new List<int> { 1 };

			_blocks = ArrayPool<short>.Shared.Rent(4096);
			_biomes = ArrayPool<byte>.Shared.Rent(4096);
			_loggedBlocks = ArrayPool<byte>.Shared.Rent(4096);
			_blockLight = new NibbleArray(ArrayPool<byte>.Shared.Rent(2048));
			_skyLight = new NibbleArray(ArrayPool<byte>.Shared.Rent(2048));
		}

		public SubChunk(int x, int z, int index, bool clearBuffers = true)
			: this()
		{
			X = x;
			Z = z;
			Index = index;

			if (clearBuffers) ClearBuffers();
		}

		public virtual void ClearBuffers()
		{
			Array.Clear(_blocks, 0, 4096);
			Array.Clear(_biomes, 0, 4096);
			Array.Clear(_biomes, 0, 4096);
			Array.Clear(_loggedBlocks, 0, 4096);
			Array.Clear(_blockLight.Data, 0, 2048);
			ChunkColumn.Fill<byte>(_skyLight.Data, 0xff);
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsAllAir()
		{
			//if (IsDirty)
			{
				_isAllAir = AllZeroFast(_blocks);
			}
			return _isAllAir;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static unsafe bool AllZeroFast<T>(T[] data) where T : unmanaged
		{
			fixed (T* shorts = data)
			{
				byte* bytes = (byte*) shorts;
				int len = data.Length * sizeof(T);
				int rem = len % (sizeof(long) * 16);
				long* b = (long*) bytes;
				long* e = (long*) (shorts + len - rem);

				while (b < e)
				{
					if ((*(b)
						| *(b + 1)
						| *(b + 2)
						| *(b + 3)
						| *(b + 4)
						| *(b + 5)
						| *(b + 6)
						| *(b + 7)
						| *(b + 8)
						| *(b + 9)
						| *(b + 10)
						| *(b + 11)
						| *(b + 12)
						| *(b + 13)
						| *(b + 14)
						| *(b + 15)) != 0)
						return false;
					b += 16;
				}

				for (int i = 0; i < rem; i++)
				{
					if (data[len - 1 - i].Equals(default(T)))
						return false;
				}

				return true;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected static int GetIndex(int bx, int by, int bz)
		{
			return (bx << 8) | (bz << 4) | by;
		}

		public int GetBlockRuntimeId(int bx, int by, int bz)
		{
			if (_runtimeIds.Count == 0) return 0;

			int paletteIndex = _blocks[GetIndex(bx, by, bz)];

			if (paletteIndex >= _runtimeIds.Count || paletteIndex < 0)
			{
				Log.Error($"Can't read block index [{paletteIndex}] from [{(X << 4) | bx}, {(Index << 4) + ChunkColumn.WorldMinY + by}, {(Z << 4) | bz}] " +
					$"in ids [{string.Join(", ", _runtimeIds)}] of chunk [{X}, {Index + (ChunkColumn.WorldMinY >> 4)}, {Z}]");
				return 0;
			}

			int runtimeId = _runtimeIds[paletteIndex];
			if (runtimeId < 0 || runtimeId >= BlockFactory.BlockPalette.Count) Log.Warn($"Couldn't locate runtime id {runtimeId} for block");

			return runtimeId;
		}

		public Block GetBlockObject(int bx, int @by, int bz)
		{
			return BlockFactory.GetBlockByRuntimeId(GetBlockRuntimeId(bx, by, bz));
		}

		public void SetBlock(int bx, int by, int bz, Block block)
		{
			int runtimeId = block.GetRuntimeId();
			if (runtimeId < 0) return;

			SetBlockByRuntimeId(bx, by, bz, runtimeId);
		}

		public void SetBlockByRuntimeId(int bx, int by, int bz, int runtimeId)
		{
			var paletteIndex = _runtimeIds.IndexOf(runtimeId);
			if (paletteIndex == -1)
			{
				_runtimeIds.Add(runtimeId);
				paletteIndex = _runtimeIds.IndexOf(runtimeId);
			}

			_blocks[GetIndex(bx, by, bz)] = (short) paletteIndex;
			_cache = null;
			IsDirty = true;
		}

		public void SetBlockIndex(int bx, int by, int bz, short paletteIndex)
		{
			_blocks[GetIndex(bx, by, bz)] = paletteIndex;
			_cache = null;
			IsDirty = true;
		}

		public byte GetBiome(int bx, int by, int bz)
		{
			if (_biomeIds.Count == 0) return 0;

			int paletteIndex = Biomes[GetIndex(bx, by, bz)];
			if (paletteIndex >= _biomeIds.Count || paletteIndex < 0)
			{
				Log.Error($"Can't read biome index [{paletteIndex}] from [{(X << 4) | bx}, {(Index << 4) + ChunkColumn.WorldMinY + by}, {(Z << 4) | bz}] " +
					$"in ids [{string.Join(", ", _biomeIds)}] of chunk [{X}, {Index + (ChunkColumn.WorldMinY >> 4)}, {Z}]");
				return 0;
			}

			return (byte) _biomeIds[paletteIndex];
		}

		internal byte GetBiomeIndex(int bx, int by, int bz)
		{
			return Biomes[GetIndex(bx, by, bz)];
		}

		public void SetBiome(int bx, int by, int bz, byte biome)
		{
			var paletteIndex = _biomeIds.IndexOf(biome);
			if (paletteIndex == -1)
			{
				_biomeIds.Add(biome);
				paletteIndex = _biomeIds.IndexOf(biome);
			}

			Biomes[GetIndex(bx, by, bz)] = (byte) paletteIndex;
		}

		public void SetLoggedBlock(int bx, int by, int bz, Block block)
		{
			int runtimeId = block.GetRuntimeId();
			if (runtimeId < 0) return;

			SetLoggedBlockByRuntimeId(bx, by, bz, runtimeId);
		}

		public void SetLoggedBlockByRuntimeId(int bx, int by, int bz, int runtimeId)
		{
			var paletteIndex = _loggedRuntimeIds.IndexOf(runtimeId);
			if (paletteIndex == -1)
			{
				_loggedRuntimeIds.Add(runtimeId);
				paletteIndex = (byte) _loggedRuntimeIds.IndexOf(runtimeId);
			}

			_loggedBlocks[GetIndex(bx, by, bz)] = (byte) paletteIndex;
			_cache = null;
			IsDirty = true;
		}

		public void SetLoggedBlockIndex(int bx, int by, int bz, byte paletteIndex)
		{
			_loggedBlocks[GetIndex(bx, by, bz)] = paletteIndex;
			_cache = null;
			IsDirty = true;
		}

		public byte GetBlocklight(int bx, int by, int bz)
		{
			return _blockLight[GetIndex(bx, by, bz)];
		}

		public void SetBlocklight(int bx, int by, int bz, byte data)
		{
			_blockLight[GetIndex(bx, by, bz)] = data;
		}

		public byte GetSkylight(int bx, int by, int bz)
		{
			return _skyLight[GetIndex(bx, by, bz)];
		}

		public void SetSkylight(int bx, int by, int bz, byte data)
		{
			_skyLight[GetIndex(bx, by, bz)] = data;
		}

		public void Write(MemoryStream stream)
		{
			if (!DisableCache && !IsDirty && _cache != null)
			{
				stream.Write(_cache);
				return;
			}

			var startPos = stream.Position;

			stream.WriteByte(8); // version
			
			int numberOfStores = 0;

			var runtimeIds = _runtimeIds;
			var blocks = _blocks;
			
			if (runtimeIds != null && runtimeIds.Count > 0)
				numberOfStores++;
			
			var loggedRuntimeIds = _loggedRuntimeIds;
			var loggedBlocks = _loggedBlocks;

			if (loggedRuntimeIds != null && loggedRuntimeIds.Count > 0)
				numberOfStores++;
			
			stream.WriteByte((byte) numberOfStores); // storage size
			
			if (WriteStore(stream, blocks, null, false, runtimeIds))
			{
				//numberOfStores++;
				if (WriteStore(stream, null, loggedBlocks, false, loggedRuntimeIds))
				{
					//numberOfStores++;
				}
			}

			int length = (int) (stream.Position - startPos);

			//stream.Position = storePosition;
			//stream.WriteByte((byte) numberOfStores); // storage size

			//if (DisableCache)
			{
				var bytes = new byte[length];
				stream.Position = startPos;
				int read = stream.Read(bytes, 0, length);
				if (read != length)
					throw new InvalidDataException($"Read wrong amount of data. Expected {length} but read {read}");
				if (startPos + length != stream.Position)
					throw new InvalidDataException($"Expected {startPos + length} but was {stream.Position}");

				_cache = bytes;
			}

			IsDirty = false;
		}

		public static bool WriteStore(MemoryStream stream, short[] blocks, byte[] loggedBlocks, bool forceWrite, List<int> palette)
		{
			if (palette.Count == 0) return false;

			// log2(number of entries) => bits needed to store them
			int bitsPerBlock = (int) Math.Ceiling(Math.Log(palette.Count, 2));

			switch (bitsPerBlock)
			{
				case 0:
					if (!forceWrite && palette.Contains(0)) return false;
					bitsPerBlock = 1;
					break;
				case 1:
				case 2:
				case 3:
				case 4:
				case 5:
				case 6:
					//Paletted1 = 1,   // 32 blocks per word
					//Paletted2 = 2,   // 16 blocks per word
					//Paletted3 = 3,   // 10 blocks and 2 bits of padding per word
					//Paletted4 = 4,   // 8 blocks per word
					//Paletted5 = 5,   // 6 blocks and 2 bits of padding per word
					//Paletted6 = 6,   // 5 blocks and 2 bits of padding per word
					break;
				case 7:
				case 8:
					//Paletted8 = 8,  // 4 blocks per word
					bitsPerBlock = 8;
					break;
				case int i when i > 8:
					//Paletted16 = 16, // 2 blocks per word
					bitsPerBlock = 16;
					break;
				default:
					break;
			}

			stream.WriteByte((byte) ((bitsPerBlock << 1) | 1)); // flags

			int blocksPerWord = (int) Math.Floor(32f / bitsPerBlock); // Floor to remove padding bits
			int wordsPerChunk = (int) Math.Ceiling(4096f / blocksPerWord);

			uint[] indexes = new uint[wordsPerChunk];

			int position = 0;
			for (int w = 0; w < wordsPerChunk; w++)
			{
				uint word = 0;
				for (int block = 0; block < blocksPerWord; block++)
				{
					if (position >= 4096)
						continue;

					uint state;
					if (blocks != null)
					{
						state = (uint) blocks[position];
					}
					else
					{
						state = (uint) loggedBlocks[position];
					}
					word |= state << (bitsPerBlock * block);

					position++;
				}
				indexes[w] = word;
			}

			byte[] ba = new byte[indexes.Length * 4];
			Buffer.BlockCopy(indexes, 0, ba, 0, indexes.Length * 4);

			stream.Write(ba, 0, ba.Length);

			VarInt.WriteSInt32(stream, palette.Count); // count
			foreach (var val in palette)
			{
				VarInt.WriteSInt32(stream, val);
			}

			return true;
		}

		public virtual object Clone()
		{
			var cc = (SubChunk) Activator.CreateInstance(GetType());
			cc.X = X;
			cc.Z = Z;
			cc.Index = Index;

			cc._isAllAir = _isAllAir;
			cc.IsDirty = IsDirty;

			cc._runtimeIds = new List<int>(_runtimeIds);
			cc._loggedRuntimeIds = new List<int>(_loggedRuntimeIds);
			cc._biomeIds = new List<int>(_biomeIds);
			_blocks.CopyTo(cc._blocks, 0);
			_loggedBlocks.CopyTo(cc._loggedBlocks, 0);
			_biomes.CopyTo(cc._biomes, 0);
			_blockLight.Data.CopyTo(cc._blockLight.Data, 0);
			_skyLight.Data.CopyTo(cc._skyLight.Data, 0);

			if (_cache != null)
			{
				cc._cache = (byte[]) _cache.Clone();
			}

			return cc;
		}

		public virtual void Dispose()
		{
			if (_blocks != null) ArrayPool<short>.Shared.Return(_blocks);
			if (_loggedBlocks != null) ArrayPool<byte>.Shared.Return(_loggedBlocks);
			if (_biomes != null) ArrayPool<byte>.Shared.Return(_biomes);
			if (_blockLight != null) ArrayPool<byte>.Shared.Return(_blockLight.Data);
			if (_skyLight != null) ArrayPool<byte>.Shared.Return(_skyLight.Data);

			GC.SuppressFinalize(this);
		}
	}
}