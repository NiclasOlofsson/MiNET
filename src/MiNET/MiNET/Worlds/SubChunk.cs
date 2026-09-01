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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using log4net;
using MiNET.Blocks;
using MiNET.Utils;

namespace MiNET.Worlds
{
	public class SubChunk : IDisposable, ICloneable
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(SubChunk));

		/// <summary>
		///     Server-wide on purpose, not per level or per player: WriteStore caches the encoded
		///     bytes on the subchunk and everyone who gets that chunk gets those same bytes, so
		///     there can only be one answer. Changing it after chunks have been encoded serves
		///     stale bytes in the wrong encoding. Owned by BlockFactory, which is the only thing
		///     that translates between a block and the id the protocol carries.
		/// </summary>
		public static bool BlockNetworkIdsAreHashes => BlockFactory.BlockNetworkIdsAreHashes;

		private bool _isAllAir = true;

		private List<int> _runtimeIds; // Add air, always as first (performance)
		internal List<int> RuntimeIds => _runtimeIds;

		private List<int> _loggedRuntimeIds = new List<int>();
		internal List<int> LoggedRuntimeIds => _loggedRuntimeIds;

		// All buffers live in one block: the cell indices as shorts, the logged indices
		// (byte-sized, basically only water and snow-levels), then, only when light calculation
		// is on, the two light nibble arrays. One allocation instead of four, uninitialized on the
		// parse path because a storage record overwrites every cell it covers, and sized without
		// the light tail when nothing will ever read it.
		internal const int DataSize = 16384;
		private const int DataSizeWithoutLights = 12288;
		private const int LoggedOffset = 8192; // 4096 shorts
		private const int BlockLightOffset = 12288;
		private const int SkyLightOffset = 14336;

		/// <summary>
		///     Light nibbles are written at creation only when something will read them: light is
		///     never serialized to the client, so with CalculateLights off the arrays are dead
		///     weight and stay uninitialized. Read once at startup; flipping the config needs a
		///     restart, like the config itself.
		/// </summary>
		internal static bool InitializeLightBuffers = Config.GetProperty("CalculateLights", false);

		private byte[] _data;

		internal Span<short> Blocks => MemoryMarshal.Cast<byte, short>(_data.AsSpan(0, LoggedOffset));
		internal Span<byte> LoggedBlocks => _data.AsSpan(LoggedOffset, 4096);
		internal Span<byte> BlockLightData => _data.AsSpan(BlockLightOffset, 2048);
		internal Span<byte> SkyLightData => _data.AsSpan(SkyLightOffset, 2048);

		public bool IsDirty { get; private set; }

		public ulong Hash { get; set; }

		/// <summary>
		///     Off means <see cref="Write" /> re-serializes this section on every call. The cache is
		///     built either way, at the end of Write, so disabling only throws away work already
		///     paid for. Every mutation nulls it and sets <see cref="IsDirty" /> through the four
		///     block setters, which are the only writers.
		///     <para>Set true to keep serialization honest while debugging, not otherwise.</para>
		/// </summary>
		public bool DisableCache { get; set; }

		private byte[] _cache;

		public SubChunk(bool clearBuffers = true)
		{
			// Empty, not seeded with air: the readers all answer air for an empty palette, and
			// the setters seed air into slot 0 the moment a first real entry needs one above it.
			// Parse fills it straight from the record, which never wants a seed in the way.
			_runtimeIds = new List<int>();

			// Uninitialized on purpose: zeroing the block costs more than everything else here.
			// clearBuffers: false is strictly for the parse path, whose storages overwrite every
			// cell they cover and whose uniform branch clears explicitly. The lights are defined
			// either way, because nothing ever parses them.
			_data = GC.AllocateUninitializedArray<byte>(InitializeLightBuffers ? DataSize : DataSizeWithoutLights);

			if (clearBuffers)
			{
				ClearBuffers();
			}
			else if (InitializeLightBuffers)
			{
				BlockLightData.Clear();
				SkyLightData.Fill(0xff);
			}
		}

		public void ClearBuffers()
		{
			Blocks.Clear();
			LoggedBlocks.Clear();

			if (InitializeLightBuffers)
			{
				BlockLightData.Clear();
				SkyLightData.Fill(0xff);
			}
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool IsAllAir()
		{
			//if (IsDirty)
			{
				// Every block pointing at palette slot 0 only means air when slot 0 is air. A
				// subchunk that is uniformly one block (solid stone underground, a bedrock layer)
				// has a single-entry palette and all-zero indices, and calling that air deletes the
				// whole section from the chunk while the heightmap still says terrain is there.
				_isAllAir = AllZeroFast(_data.AsSpan(0, LoggedOffset)) && (_runtimeIds.Count == 0 || _runtimeIds[0] == BlockFactory.AirRuntimeId);
			}
			return _isAllAir;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static unsafe bool AllZeroFast(ReadOnlySpan<byte> data)
		{
			fixed (byte* start = data)
			{
				byte* bytes = start;
				int len = data.Length;
				int rem = len % (sizeof(long) * 16);
				long* b = (long*) bytes;
				long* e = (long*) (bytes + len - rem);

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
					if (bytes[len - 1 - i] != 0) return false;
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

			int paletteIndex = Blocks[GetIndex(bx, by, bz)];
			if (paletteIndex >= _runtimeIds.Count || paletteIndex < 0) Log.Warn($"Unexpected paletteIndex of {paletteIndex} with size of palette is {_runtimeIds.Count}");
			int runtimeId = _runtimeIds[paletteIndex];
			if (runtimeId < 0 || runtimeId >= BlockFactory.BlockPalette.Count) Log.Warn($"Couldn't locate runtime id {runtimeId} for block");
			int bid = BlockFactory.BlockPalette[runtimeId].Id;
			return bid == -1 ? 0 : bid;
		}

		/// <summary>
		///     The block's palette index, which is its identity here. Anything asking a question about
		///     the block itself wants this: GetBlockId projects onto a legacy id and answers air for
		///     every block that has none.
		/// </summary>
		public int GetBlockRuntimeId(int bx, int by, int bz)
		{
			if (_runtimeIds.Count == 0) return BlockFactory.AirRuntimeId;

			int paletteIndex = Blocks[GetIndex(bx, by, bz)];
			if (paletteIndex < 0 || paletteIndex >= _runtimeIds.Count) return BlockFactory.AirRuntimeId;

			return _runtimeIds[paletteIndex];
		}

		public Block GetBlockObject(int bx, int @by, int bz)
		{
			if (_runtimeIds.Count == 0) return new Air();

			int index = Blocks[GetIndex(bx, by, bz)];
			int runtimeId = _runtimeIds[index];

			// By name, not by legacy id: the id predates flattening and hands back a class whose
			// state no longer exists in the palette, so the block could not be written back.
			Block block = BlockFactory.GetBlockByRuntimeId(runtimeId);
			if (block == null) return new Air();

			block.Metadata = (byte) BlockFactory.BlockPalette[runtimeId].Data; //TODO: REMOVE metadata. Not needed.

			return block;
		}

		public void SetBlock(int bx, int by, int bz, Block block)
		{
			int runtimeId = block.GetRuntimeId();
			if (runtimeId < 0)
			{
				// Dropping the write silently leaves the world holding air where the client has
				// already drawn a block, and the next placement against it overwrites the block
				// that was aimed at. Loud, because there is no correct way to continue.
				Log.Error($"Refusing to write {block.Name} at {bx},{by},{bz}: its state has no runtime id. State={block.GetState()}");
				return;
			}

			SetBlockByRuntimeId(bx, by, bz, runtimeId);
		}

		public void SetBlockByRuntimeId(int bx, int by, int bz, int runtimeId)
		{
			var paletteIndex = _runtimeIds.IndexOf(runtimeId);
			if (paletteIndex == -1)
			{
				// Slot 0 must be air, because every untouched cell holds index 0. The first real
				// entry therefore lands above an air seeded in just in time.
				if (_runtimeIds.Count == 0 && runtimeId != BlockFactory.AirRuntimeId) _runtimeIds.Add(BlockFactory.AirRuntimeId);

				_runtimeIds.Add(runtimeId);
				paletteIndex = _runtimeIds.IndexOf(runtimeId);
			}

			Blocks[GetIndex(bx, by, bz)] = (short) paletteIndex;
			_cache = null;
			IsDirty = true;
		}

		public void SetBlockIndex(int bx, int by, int bz, short paletteIndex)
		{
			Blocks[GetIndex(bx, by, bz)] = paletteIndex;
			_cache = null;
			IsDirty = true;
		}

		/// <summary>
		///     The one dirty mark for a bulk loader that has written the backing arrays directly
		///     (<see cref="Blocks" />, the light arrays) instead of going through the per-cell
		///     setters. Direct writes without this serve a stale encode cache.
		/// </summary>
		internal void MarkBulkLoaded()
		{
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
				// The empty logged palette is what keeps a never-written logged buffer unreadable;
				// this append opens that gate, so every cell must be defined before it does. And
				// slot 0 must be air here too: without it, one waterlogged block puts water at
				// index 0 and all 4095 untouched cells read as waterlogged.
				if (_loggedRuntimeIds.Count == 0)
				{
					LoggedBlocks.Clear();
					if (runtimeId != BlockFactory.AirRuntimeId) _loggedRuntimeIds.Add(BlockFactory.AirRuntimeId);
				}

				_loggedRuntimeIds.Add(runtimeId);
				paletteIndex = (byte) _loggedRuntimeIds.IndexOf(runtimeId);
			}

			LoggedBlocks[GetIndex(bx, by, bz)] = (byte) paletteIndex;
			_cache = null;
			IsDirty = true;
		}

		public void SetLoggedBlockIndex(int bx, int by, int bz, byte paletteIndex)
		{
			LoggedBlocks[GetIndex(bx, by, bz)] = paletteIndex;
			_cache = null;
			IsDirty = true;
		}

		// With light calculation off the block carries no light regions at all, so the accessors
		// answer the defaults (no blocklight, full skylight) and writes drop, instead of indexing
		// past the short block.
		public byte GetBlocklight(int bx, int by, int bz)
		{
			if (!InitializeLightBuffers) return 0;

			return GetNibble(BlockLightOffset, GetIndex(bx, by, bz));
		}

		public void SetBlocklight(int bx, int by, int bz, byte data)
		{
			if (!InitializeLightBuffers) return;

			SetNibble(BlockLightOffset, GetIndex(bx, by, bz), data);
		}

		public byte GetSkylight(int bx, int by, int bz)
		{
			if (!InitializeLightBuffers) return 15;

			return GetNibble(SkyLightOffset, GetIndex(bx, by, bz));
		}

		public void SetSkylight(int bx, int by, int bz, byte data)
		{
			if (!InitializeLightBuffers) return;

			SetNibble(SkyLightOffset, GetIndex(bx, by, bz), data);
		}

		// Same nibble order as NibbleArray: even cell in the low half, odd cell in the high half.
		private byte GetNibble(int offset, int index)
		{
			return (byte) ((_data[offset + (index >> 1)] >> ((index & 1) * 4)) & 0xF);
		}

		private void SetNibble(int offset, int index, byte value)
		{
			value &= 0xF;
			int idx = offset + (index >> 1);
			_data[idx] &= (byte) (0xF << (((index + 1) & 1) * 4));
			_data[idx] |= (byte) (value << ((index & 1) * 4));
		}

		/// <summary>
		///     The sub-chunk request form (McpeSubChunk entries): version 9 with the absolute
		///     section index after the storage count. The inline LevelChunk form (<see cref="Write" />)
		///     stays version 8, where the section index is implied by stream order.
		/// </summary>
		public void WriteVersion9(MemoryStream stream, sbyte yIndex)
		{
			stream.WriteByte(9); // version

			int numberOfStores = 0;
			if (_runtimeIds != null && _runtimeIds.Count > 0) numberOfStores++;
			if (_loggedRuntimeIds != null && _loggedRuntimeIds.Count > 0) numberOfStores++;

			stream.WriteByte((byte) numberOfStores);
			stream.WriteByte((byte) yIndex);

			if (WriteStore(stream, Blocks, default, _runtimeIds))
			{
				WriteStore(stream, default, LoggedBlocks, _loggedRuntimeIds);
			}
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

			if (runtimeIds != null && runtimeIds.Count > 0)
				numberOfStores++;

			var loggedRuntimeIds = _loggedRuntimeIds;

			if (loggedRuntimeIds != null && loggedRuntimeIds.Count > 0)
				numberOfStores++;

			stream.WriteByte((byte) numberOfStores); // storage size

			if (WriteStore(stream, Blocks, default, runtimeIds))
			{
				WriteStore(stream, default, LoggedBlocks, loggedRuntimeIds);
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

		public static bool WriteStore(MemoryStream stream, ReadOnlySpan<short> blocks, ReadOnlySpan<byte> loggedBlocks, List<int> palette, bool isBlockPalette = true)
		{
			if (palette.Count == 0) return false;

			// log2(number of entries) => bits needed to store them
			int bitsPerBlock = (int) Math.Ceiling(Math.Log(palette.Count, 2));

			switch (bitsPerBlock)
			{
				case 0:
					// A single-entry palette always writes: its one value can be any runtime id or
					// biome id, and the caller has already counted this storage into the stream.
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
					if (!blocks.IsEmpty)
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
				// Only BLOCK palettes go through the wire id conversion (hash mode); biome palettes
				// carry plain biome ids, and hashing them poisons every chunk the moment
				// BlockNetworkIdsAreHashes is on.
				VarInt.WriteSInt32(stream, isBlockPalette ? (int) BlockFactory.GetNetworkId(val) : val);
			}

			return true;
		}

		public object Clone()
		{
			// clearBuffers: false because the copy below overwrites the whole block.
			SubChunk cc = new SubChunk(clearBuffers: false);
			cc._isAllAir = _isAllAir;
			cc.IsDirty = IsDirty;

			cc._runtimeIds = new List<int>(_runtimeIds);
			cc._loggedRuntimeIds = new List<int>(_loggedRuntimeIds);
			_data.CopyTo(cc._data, 0);

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
		}

		// The buffers are plain managed memory in one block; there is nothing to release.
		public void Dispose()
		{
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