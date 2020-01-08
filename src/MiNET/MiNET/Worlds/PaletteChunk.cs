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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2019 Niclas Olofsson.
// All Rights Reserved.

#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using log4net;
using MiNET.Blocks;
using MiNET.Utils;

namespace MiNET.Worlds
{
	public class PaletteChunk : ChunkBase
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(PaletteChunk));

		private bool _useAlexChunks = true;

		private bool _isAllAir = true;

		private short[] _blocks = new short[4096];
		private NibbleArray _metadata = new NibbleArray(16 * 16 * 16);
		private short[] _loggedBlocks = new short[4096];
		private NibbleArray _loggedMetadata = new NibbleArray(16 * 16 * 16);

		private byte[] _cache;
		private bool _isDirty;

		public PaletteChunk(bool useAlexChunks = false)
		{
			_useAlexChunks = useAlexChunks;
		}

		public override bool IsDirty => _isDirty;

		public override bool IsAllAir()
		{
			if (_isDirty)
			{
				_isAllAir = _blocks.All(b => b == 0);
				//_isDirty = false;
			}
			return _isAllAir;
		}

		private static int GetIndex(int bx, int by, int bz)
		{
			return (bx << 8) | (bz << 4) | by;
		}

		public override int GetBlock(int bx, int by, int bz)
		{
			return _blocks[GetIndex(bx, @by, bz)];
		}

		public override void SetBlock(int bx, int by, int bz, int bid)
		{
			_blocks[GetIndex(bx, by, bz)] = (short) bid;
			_cache = null;
			_isDirty = true;
		}

		public int GetLoggedBlock(int bx, int by, int bz)
		{
			return _loggedBlocks[GetIndex(bx, @by, bz)];
		}

		public void SetLoggedBlock(int bx, int by, int bz, int bid)
		{
			_loggedBlocks[GetIndex(bx, by, bz)] = (short) bid;
			_cache = null;
			_isDirty = true;
		}

		public override byte GetMetadata(int bx, int by, int bz)
		{
			return _metadata[GetIndex(bx, by, bz)];
		}

		public override void SetMetadata(int bx, int by, int bz, byte data)
		{
			_metadata[GetIndex(bx, by, bz)] = data;
			_cache = null;
			_isDirty = true;
		}

		public byte GetLoggedMetadata(int bx, int by, int bz)
		{
			return _loggedMetadata[GetIndex(bx, by, bz)];
		}

		public void SetLoggedMetadata(int bx, int by, int bz, byte data)
		{
			_loggedMetadata[GetIndex(bx, by, bz)] = data;
			_cache = null;
			_isDirty = true;
		}

		public override byte[] GetBytes(Stream instream)
		{
			if (_cache != null)
			{
				instream.Write(_cache);
				return _cache;
			}

			using (var stream = new MemoryStream())
			{
				stream.WriteByte(8); // version

				int numberOfStores = 0;
				stream.WriteByte((byte) numberOfStores); // storage size

				if (WriteStore(stream, _blocks, _metadata, false))
				{
					numberOfStores++;
					if (WriteStore(stream, _loggedBlocks, _loggedMetadata, false))
					{
						numberOfStores++;
					}
				}

				// Special implementation for the Alex client by Kennyvv. Will send
				// block and skylight to the client so that we can test our implementations
				if (_useAlexChunks)
				{
					stream.Write(skylight.Data);
					stream.Write(blocklight.Data);
				}

				stream.Position = 1;
				stream.WriteByte((byte) numberOfStores); // storage size

				var bytes = stream.ToArray();
				instream.Write(bytes);
				_cache = bytes;
			}

			return _cache;
		}

		private static bool WriteStore(MemoryStream stream, short[] blocks, NibbleArray metadata, bool forceWrite)
		{
			var palette = new Dictionary<uint, byte>();
			uint prevHash = uint.MaxValue;
			for (int i = 0; i < 4096; i++)
			{
				uint hash = (uint) blocks[i] << 4 | metadata[i]; // 1.7
				if (hash == prevHash)
					continue;

				prevHash = hash;
				palette[hash] = 0;
			}

			// log2(number of entries) => bits needed to store them
			int bitsPerBlock = (int) Math.Ceiling(Math.Log(palette.Count, 2));

			switch (bitsPerBlock)
			{
				case 0:
					if (!forceWrite && palette.ContainsKey(0)) return false;
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

			stream.WriteByte((byte) ((bitsPerBlock << 1) | 1)); // version

			int blocksPerWord = (int) Math.Floor(32f / bitsPerBlock); // Floor to remove padding bits
			int wordsPerChunk = (int) Math.Ceiling(4096f / blocksPerWord);

			byte t = 0;
			foreach (var b in palette.ToArray())
			{
				palette[b.Key] = t++;
			}

			uint[] indexes = new uint[wordsPerChunk];

			int position = 0;
			for (int w = 0; w < wordsPerChunk; w++)
			{
				uint word = 0;
				for (int block = 0; block < blocksPerWord; block++)
				{
					if (position >= 4096)
						continue;

					uint state = palette[(uint) blocks[position] << 4 | metadata[position]];
					word |= state << (bitsPerBlock * block);

					position++;
				}
				indexes[w] = word;
			}

			byte[] ba = new byte[indexes.Length * 4];
			Buffer.BlockCopy(indexes, 0, ba, 0, indexes.Length * 4);
			stream.Write(ba, 0, ba.Length);

			int[] legacyToRuntimeId = BlockFactory.LegacyToRuntimeId;

			VarInt.WriteSInt32(stream, palette.Count); // count
			foreach (var val in palette)
			{
				VarInt.WriteSInt32(stream, legacyToRuntimeId[val.Key]);
			}

			return true;
		}

		public override object Clone()
		{
			PaletteChunk cc = CreateObject();
			cc._isAllAir = _isAllAir;
			cc._isDirty = _isDirty;

			_blocks.CopyTo(cc._blocks, 0);
			_metadata.Data.CopyTo(cc._metadata.Data, 0);
			_loggedBlocks.CopyTo(cc._loggedBlocks, 0);
			_loggedMetadata.Data.CopyTo(cc._loggedMetadata.Data, 0);
			blocklight.Data.CopyTo(cc.blocklight.Data, 0);
			skylight.Data.CopyTo(cc.skylight.Data, 0);

			if (_cache != null)
			{
				cc._cache = (byte[]) _cache.Clone();
			}

			return cc;
		}

		private static readonly ChunkPool<PaletteChunk> Pool = new ChunkPool<PaletteChunk>(() => new PaletteChunk(Config.GetProperty("UseAlexChunks", false)));

		public static PaletteChunk CreateObject()
		{
			return Pool.GetObject();
		}

		public override void PutPool()
		{
			Reset();
			Pool.PutObject(this);
		}

		public override void Reset()
		{
			_isAllAir = true;
			Array.Clear(_blocks, 0, _blocks.Length);
			Array.Clear(_metadata.Data, 0, _metadata.Data.Length);
			Array.Clear(_loggedBlocks, 0, _blocks.Length);
			Array.Clear(_loggedMetadata.Data, 0, _metadata.Data.Length);
			Array.Clear(blocklight.Data, 0, blocklight.Data.Length);
			Array.Fill<byte>(skylight.Data, 0xff);
			_cache = null;
			_isDirty = false;
		}

		//~PaletteChunk()
		//{
		//	Log.Error($"Unexpected dispose chunk");
		//}
	}
}