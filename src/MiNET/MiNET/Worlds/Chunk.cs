using System;
using System.IO;
using System.Linq;
using fNbt;
using log4net;
using MiNET.Utils;

namespace MiNET.Worlds
{
	public class Chunk : ICloneable
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (ChunkColumn));

		private bool _isAllAir = true;

		public byte[] blocks = new byte[16*16*16];
		public NibbleArray metadata = new NibbleArray(16*16*16);
		public NibbleArray blocklight = new NibbleArray(16*16*16);
		public NibbleArray skylight = new NibbleArray(16*16*16);

		private byte[] _cache;
		private bool _isDirty;
		private object _cacheSync = new object();

		public Chunk()
		{
			ChunkColumn.Fill<byte>(skylight.Data, 0xff);
			//BiomeUtils utils = new BiomeUtils();
			//utils.PrecomputeBiomeColors();
		}

		public bool IsDirty => _isDirty;

		public bool IsAllAir()
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
			return (bx*256) + (bz*16) + by;
		}

		public byte GetBlock(int bx, int by, int bz)
		{
			return blocks[GetIndex(bx, by, bz)];
		}

		public void SetBlock(int bx, int by, int bz, byte bid)
		{
			blocks[GetIndex(bx, by, bz)] = bid;
			_cache = null;
			_isDirty = true;
		}

		public byte GetBlocklight(int bx, int by, int bz)
		{
			return blocklight[GetIndex(bx, by, bz)];
		}

		public void SetBlocklight(int bx, int by, int bz, byte data)
		{
			blocklight[GetIndex(bx, by, bz)] = data;
			_cache = null;
			_isDirty = true;
		}

		public byte GetMetadata(int bx, int by, int bz)
		{
			return metadata[GetIndex(bx, by, bz)];
		}

		public void SetMetadata(int bx, int by, int bz, byte data)
		{
			metadata[GetIndex(bx, by, bz)] = data;
			_cache = null;
			_isDirty = true;
		}

		public byte GetSkylight(int bx, int by, int bz)
		{
			return skylight[GetIndex(bx, by, bz)];
		}

		public void SetSkylight(int bx, int by, int bz, byte data)
		{
			skylight[GetIndex(bx, by, bz)] = data;
			_cache = null;
			_isDirty = true;
		}

		public byte[] GetBytes()
		{
			if (_cache != null) return _cache;

			using (MemoryStream stream = MiNetServer.MemoryStreamManager.GetStream())
			{
				NbtBinaryWriter writer = new NbtBinaryWriter(stream, true);

				writer.Write(blocks);
				writer.Write(metadata.Data);
				writer.Write(skylight.Data);
				writer.Write(blocklight.Data);
				_cache = stream.ToArray();
			}

			return _cache;
		}

		public object Clone()
		{
			Chunk cc = (Chunk) MemberwiseClone();

			cc.blocks = (byte[]) blocks.Clone();
			cc.metadata = (NibbleArray) metadata.Clone();
			cc.blocklight = (NibbleArray) blocklight.Clone();
			cc.skylight = (NibbleArray) skylight.Clone();

			if (_cache != null)
			{
				cc._cache = (byte[]) _cache.Clone();
			}

			cc._cacheSync = new object();

			return cc;
		}
	}
}