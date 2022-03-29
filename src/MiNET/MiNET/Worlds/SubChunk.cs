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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
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
		internal List<int> RuntimeIds => _runtimeIds;

		private short[] _blocks;
		internal short[] Blocks => _blocks;

		private List<int> _loggedRuntimeIds = new List<int>();
		internal List<int> LoggedRuntimeIds => _loggedRuntimeIds;

		private byte[] _loggedBlocks; // We use only byte size on this palette index table, because can basically only be water and snow-levels
		internal byte[] LoggedBlocks => _loggedBlocks;

		// Consider disabling these if we don't calculate lights
		public NibbleArray _blocklight;
		public NibbleArray _skylight;

		public bool IsDirty { get; private set; }

		public ulong Hash { get; set; }
		public bool DisableCache { get; set; } = true;
		private byte[] _cache;

		public SubChunk(bool clearBuffers = true)
		{
			_runtimeIds = new List<int> {(int) BlockFactory.GetBlockByName("minecraft:air").GetRuntimeId()};
				
			_blocks = ArrayPool<short>.Shared.Rent(4096);
			_loggedBlocks = ArrayPool<byte>.Shared.Rent(4096);
			_blocklight = new NibbleArray(ArrayPool<byte>.Shared.Rent(2048));
			_skylight = new NibbleArray(ArrayPool<byte>.Shared.Rent(2048));

			if (clearBuffers) ClearBuffers();
		}

		public void ClearBuffers()
		{
			Array.Clear(_blocks, 0, 4096);
			Array.Clear(_loggedBlocks, 0, 4096);
			Array.Clear(_blocklight.Data, 0, 2048);
			ChunkColumn.Fill<byte>(_skylight.Data, 0xff);
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

		private static int GetIndex(int bx, int by, int bz)
		{
			return (bx << 8) | (bz << 4) | by;
		}

		public int GetBlockId(int bx, int by, int bz)
		{
			if (_runtimeIds.Count == 0) return 0;

			int paletteIndex = _blocks[GetIndex(bx, by, bz)];
			if (paletteIndex >= _runtimeIds.Count || paletteIndex < 0) Log.Warn($"Unexpected paletteIndex of {paletteIndex} with size of palette is {_runtimeIds.Count}");
			int runtimeId = _runtimeIds[paletteIndex];
			if (runtimeId < 0 || runtimeId >= BlockFactory.BlockPalette.Count) Log.Warn($"Couldn't locate runtime id {runtimeId} for block");
			int bid = BlockFactory.BlockPalette[runtimeId].Id;
			return bid == -1 ? 0 : bid;
		}

		public Block GetBlockObject(int bx, int @by, int bz)
		{
			if (_runtimeIds.Count == 0) return new Air();

			int index = _blocks[GetIndex(bx, by, bz)];
			int runtimeId = _runtimeIds[index];
			BlockStateContainer blockState = BlockFactory.BlockPalette[runtimeId];
			Block block = BlockFactory.GetBlockById(blockState.Id);
			block.SetState(blockState.States);
			block.Metadata = (byte) blockState.Data; //TODO: REMOVE metadata. Not needed.

			return block;
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
			return _blocklight[GetIndex(bx, by, bz)];
		}

		public void SetBlocklight(int bx, int by, int bz, byte data)
		{
			_blocklight[GetIndex(bx, by, bz)] = data;
		}

		public byte GetSkylight(int bx, int by, int bz)
		{
			return _skylight[GetIndex(bx, by, bz)];
		}

		public void SetSkylight(int bx, int by, int bz, byte data)
		{
			_skylight[GetIndex(bx, by, bz)] = data;
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

		public object Clone()
		{
			SubChunk cc = CreateObject();
			cc._isAllAir = _isAllAir;
			cc.IsDirty = IsDirty;

			cc._runtimeIds = new List<int>(_runtimeIds);
			_blocks.CopyTo(cc._blocks, 0);
			cc._loggedRuntimeIds = new List<int>(_loggedRuntimeIds);
			_loggedBlocks.CopyTo(cc._loggedBlocks, 0);
			_blocklight.Data.CopyTo(cc._blocklight.Data, 0);
			_skylight.Data.CopyTo(cc._skylight.Data, 0);

			if (_cache != null)
			{
				cc._cache = (byte[]) _cache.Clone();
			}

			return cc;
		}

		private static readonly ChunkPool<SubChunk> Pool = new ChunkPool<SubChunk>(() => new SubChunk());

		public static SubChunk CreateObject()
		{
			return new SubChunk();
			//return Pool.GetObject();
		}

		public void PutPool()
		{
			Dispose();
			//Reset();
			//Pool.PutObject(this);
		}

		public void REMOVEReset()
		{
			_isAllAir = true;
			_runtimeIds.Clear();
			Array.Clear(_blocks, 0, _blocks.Length);
			_loggedRuntimeIds.Clear();
			Array.Clear(_loggedBlocks, 0, _blocks.Length);
			Array.Clear(_blocklight.Data, 0, _blocklight.Data.Length);
			Array.Fill<byte>(_skylight.Data, 0xff);
			_cache = null;
			IsDirty = false;
		}

		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (_blocks != null) ArrayPool<short>.Shared.Return(_blocks);
				if (_loggedBlocks != null) ArrayPool<byte>.Shared.Return(_loggedBlocks);
				if (_blocklight != null) ArrayPool<byte>.Shared.Return(_blocklight.Data);
				if (_skylight != null) ArrayPool<byte>.Shared.Return(_skylight.Data);
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		~SubChunk()
		{
			Dispose(false);
		}
	}

	public class ChunkPool<T>
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(ChunkPool<T>));

		private ConcurrentQueue<T> _objects;

		private Func<T> _objectGenerator;

		public ChunkPool(Func<T> objectGenerator)
		{
			if (objectGenerator == null)
				throw new ArgumentNullException("objectGenerator");
			_objects = new ConcurrentQueue<T>();
			_objectGenerator = objectGenerator;
		}

		public T GetObject()
		{
			if (_objects.IsEmpty)
				return _objectGenerator();

			T item;
			if (_objects.TryDequeue(out item))
				return item;
			return _objectGenerator();
		}

		const long MaxPoolSize = 10000000;

		public void PutObject(T item)
		{
			//_objects.Enqueue(item);
		}
	}
}