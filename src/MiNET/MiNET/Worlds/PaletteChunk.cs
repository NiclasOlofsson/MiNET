using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using log4net;
using MiNET.Blocks;
using MiNET.Utils;

namespace MiNET.Worlds
{
	public class PaletteChunk : ChunkBase, ICloneable
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(PaletteChunk));

		private bool _isAllAir = true;

		public short[] blocks = new short[16 * 16 * 16];
		public NibbleArray metadata = new NibbleArray(16 * 16 * 16);

		private byte[] _cache;
		private bool _isDirty;
		private object _cacheSync = new object();

		public PaletteChunk()
		{
			ChunkColumn.Fill<byte>(skylight.Data, 0xff);
		}

		public override bool IsDirty => _isDirty;

		public override bool IsAllAir()
		{
			if (_isDirty)
			{
				_isAllAir = blocks.All(b => b == 0);
				_isDirty = false;
			}
			return _isAllAir;
		}

		private static int GetIndex(int bx, int by, int bz)
		{
			return (bx * 256) + (bz * 16) + by;
		}

		public override int GetBlock(int bx, int by, int bz)
		{
			return (byte)blocks[GetIndex(bx, @by, bz)];
		}

		public override void SetBlock(int bx, int by, int bz, int bid)
		{
			blocks[GetIndex(bx, by, bz)] = (short) bid;
			_cache = null;
			_isDirty = true;
		}

		public override byte GetBlocklight(int bx, int by, int bz)
		{
			return blocklight[GetIndex(bx, by, bz)];
		}

		public override void SetBlocklight(int bx, int by, int bz, byte data)
		{
			blocklight[GetIndex(bx, by, bz)] = data;
			//_cache = null;
			//_isDirty = true;
		}

		public override byte GetMetadata(int bx, int by, int bz)
		{

			return metadata[GetIndex(bx, by, bz)];
		}

		public override void SetMetadata(int bx, int by, int bz, byte data)
		{
			metadata[GetIndex(bx, by, bz)] = data;
			_cache = null;
			_isDirty = true;
		}

		public override byte GetSkylight(int bx, int by, int bz)
		{
			return skylight[GetIndex(bx, by, bz)];
		}

		public override void SetSkylight(int bx, int by, int bz, byte data)
		{
			skylight[GetIndex(bx, by, bz)] = data;
			//_cache = null;
			//_isDirty = true;
		}

		public override byte[] GetBytes(Stream stream)
		{
			if (_cache != null) return _cache;
			stream.WriteByte(8); // version

			int numberOfStores = 1;
			stream.WriteByte((byte) numberOfStores); // storage size

			List<uint> palette = new List<uint>(10);
			byte[] indexes = new byte[blocks.Length];
			for (int i = 0; i < numberOfStores; i++)
			{
				int cacheHits = 0;
				int lookups = 0;

				palette.Clear();

				stream.WriteByte((byte) ((8 << 1) | 1)); // version

				int index = 0;
				uint prevHash = uint.MaxValue;
				for (int b = 0; b < blocks.Length; b++)
				{
					byte bid = (byte) blocks[b];
					byte data = metadata[b];
					uint hash = BlockFactory.GetRuntimeId(bid, data);
					if (hash != prevHash)
					{
						index = palette.IndexOf(hash);
						if (index == -1)
						{
							palette.Add(hash);
						}
						index = palette.IndexOf(hash);

						lookups++;
					}
					else
					{
						cacheHits++;
					}

					indexes[b] = (byte) index;
					prevHash = hash;
				}

				stream.Write(indexes, 0, indexes.Length);

				VarInt.WriteSInt32(stream, palette.Count); // count
				//Log.Debug($"Palette size = {palette.Count}, {IsAllAir()}, {lookups}, {cacheHits}");
				foreach (var val in palette)
				{
					VarInt.WriteSInt32(stream, (int)val); // air
				}
			}
			//_cache = stream.ToArray();

			return _cache;
		}

		//private bool _isAllAir = true;
		//private bool _isDirty;

		public override object Clone()
		{
			PaletteChunk cc = CreateObject();
			cc._isAllAir = _isAllAir;
			cc._isDirty = _isDirty;

			blocks.CopyTo(cc.blocks, 0);
			//metadata.Data.CopyTo(cc.metadata.Data, 0);
			blocklight.Data.CopyTo(cc.blocklight.Data, 0);
			skylight.Data.CopyTo(cc.skylight.Data, 0);

			if (_cache != null)
			{
				cc._cache = (byte[])_cache.Clone();
			}

			cc._cacheSync = new object();

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
			Array.Clear(blocks, 0, blocks.Length);
			//Array.Clear(metadata.Data, 0, metadata.Data.Length);
			Array.Clear(blocklight.Data, 0, blocklight.Data.Length);
			//Array.Clear(skylight.Data, 0, skylight.Data.Length);
			ChunkColumn.Fill<byte>(skylight.Data, 0xff);

			_cache = null;
			_isDirty = false;
		}

		~PaletteChunk()
		{
			Log.Error($"Unexpected dispose chunk");
		}
	}
}