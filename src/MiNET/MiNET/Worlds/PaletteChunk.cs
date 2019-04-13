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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2018 Niclas Olofsson. 
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

		private bool _isAllAir = true;

		private short[] _blocks = new short[4096];
		private byte[] _metadata = new byte[4096];

		private byte[] _cache;
		private bool _isDirty;

		public PaletteChunk()
		{
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
			return (bx * 256) + (bz * 16) + by;
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

				int numberOfStores = 1;
				stream.WriteByte((byte) numberOfStores); // storage size

				List<uint> palette = new List<uint>(10);
				byte[] indexes = new byte[_blocks.Length];
				for (int i = 0; i < numberOfStores; i++)
				{
					palette.Clear();

					stream.WriteByte((8 << 1) | 1); // version

					int index = 0;
					uint prevHash = uint.MaxValue;
					for (int b = 0; b < _blocks.Length; b++)
					{
						byte bid = (byte) _blocks[b];
						byte data = _metadata[b];
						uint hash = BlockFactory.GetRuntimeId(bid, data);
						if (hash != prevHash)
						{
							index = palette.IndexOf(hash);
							if (index == -1)
							{
								palette.Add(hash);
							}
							index = palette.IndexOf(hash);
						}

						indexes[b] = (byte) index;
						prevHash = hash;
					}

					stream.Write(indexes, 0, indexes.Length);

					VarInt.WriteSInt32(stream, palette.Count); // count
					foreach (var val in palette)
					{
						VarInt.WriteSInt32(stream, (int) val);
					}
				}

				var bytes = stream.ToArray();
				instream.Write(bytes);
				_cache = bytes;
			}

			return _cache;
		}

		public override object Clone()
		{
			PaletteChunk cc = CreateObject();
			cc._isAllAir = _isAllAir;
			cc._isDirty = _isDirty;

			_blocks.CopyTo(cc._blocks, 0);
			_metadata.CopyTo(cc._metadata, 0);
			blocklight.Data.CopyTo(cc.blocklight.Data, 0);
			skylight.Data.CopyTo(cc.skylight.Data, 0);

			if (_cache != null)
			{
				cc._cache = (byte[]) _cache.Clone();
			}

			return cc;
		}

		private static readonly ChunkPool<PaletteChunk> Pool = new ChunkPool<PaletteChunk>(() => new PaletteChunk());

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
			Array.Clear(_metadata, 0, _metadata.Length);
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