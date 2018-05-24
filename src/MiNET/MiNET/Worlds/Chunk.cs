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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using log4net;
using MiNET.Blocks;
using MiNET.Utils;
using Newtonsoft.Json.Linq;

namespace MiNET.Worlds
{

	public abstract class ChunkBase : ICloneable
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(ChunkBase));

		public ChunkBase()
		{
		}


		public abstract bool IsAllAir();

		public abstract byte GetBlock(int bx, int by, int bz);

		public abstract void SetBlock(int bx, int by, int bz, byte bid);

		public abstract byte GetBlocklight(int bx, int by, int bz);

		public abstract void SetBlocklight(int bx, int by, int bz, byte data);

		public abstract byte GetMetadata(int bx, int by, int bz);

		public abstract void SetMetadata(int bx, int by, int bz, byte data);

		public abstract byte GetSkylight(int bx, int by, int bz);

		public abstract void SetSkylight(int bx, int by, int bz, byte data);

		public abstract byte[] GetBytes(Stream stream);

		public abstract object Clone();

		public abstract void Reset();
	}

	public class Chunk : ChunkBase
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(Chunk));

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
			//ChunkColumn.Fill<byte>(blocklight.Data, 0x88);
		}

		public bool IsDirty => _isDirty;

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
			return (bx*256) + (bz*16) + by;
		}

		public override byte GetBlock(int bx, int by, int bz)
		{
			return blocks[GetIndex(bx, by, bz)];
		}

		public override void SetBlock(int bx, int by, int bz, byte bid)
		{
			blocks[GetIndex(bx, by, bz)] = bid;
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

			//using (MemoryStream stream = MiNetServer.MemoryStreamManager.GetStream())
			{
				stream.WriteByte((byte) 0); // version
				stream.Write(blocks, 0, blocks.Length);
				stream.Write(metadata.Data, 0, metadata.Data.Length);
				//writer.Write(skylight.Data);
				//writer.Write(blocklight.Data);
				//_cache = stream.ToArray();
			}

			return _cache;
		}

		public override object Clone()
		{
			Chunk cc = CreateObject();
			cc._isAllAir = _isAllAir;
			cc._isDirty = _isDirty;

			blocks.CopyTo(cc.blocks, 0);
			metadata.Data.CopyTo(cc.metadata.Data, 0);
			blocklight.Data.CopyTo(cc.blocklight.Data, 0);
			skylight.Data.CopyTo(cc.skylight.Data, 0);

			if (_cache != null)
			{
				cc._cache = (byte[]) _cache.Clone();
			}

			cc._cacheSync = new object();

			return cc;
		}

		private static readonly ChunkPool<Chunk> Pool = new ChunkPool<Chunk>(() => new Chunk());

		public static Chunk CreateObject()
		{
			return Pool.GetObject();
		}

		public void PutPool()
		{
			Reset();
			Pool.PutObject(this);
		}

		public override void Reset()
		{
			_isAllAir = true;
			Array.Clear(blocks, 0, blocks.Length);
			Array.Clear(metadata.Data, 0, metadata.Data.Length);
			Array.Clear(blocklight.Data, 0, blocklight.Data.Length);
			Array.Clear(skylight.Data, 0, skylight.Data.Length);
			ChunkColumn.Fill<byte>(skylight.Data, 0xff);
			_cache = null;
			_isDirty = false;
		}

		~Chunk()
		{
			Log.Error($"Unexpected dispose chunk");
		}
	}

	public class ChunkPool<T>
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(ChunkPool<T>));

		private ConcurrentQueue<T> _objects;

		private Func<T> _objectGenerator;

		public void FillPool(int count)
		{
			for (int i = 0; i < count; i++)
			{
				var item = _objectGenerator();
				_objects.Enqueue(item);
			}
		}

		public ChunkPool(Func<T> objectGenerator)
		{
			if (objectGenerator == null) throw new ArgumentNullException("objectGenerator");
			_objects = new ConcurrentQueue<T>();
			_objectGenerator = objectGenerator;
		}

		public T GetObject()
		{
			if (_objects.IsEmpty) return _objectGenerator();

			T item;
			if (_objects.TryDequeue(out item)) return item;
			return _objectGenerator();
		}

		const long MaxPoolSize = 10000000;

		public void PutObject(T item)
		{
			_objects.Enqueue(item);
		}
	}

	//public class PaletteChunk : ChunkBase, ICloneable
	//{
	//	private static readonly ILog Log = LogManager.GetLogger(typeof(PaletteChunk));

	//	private bool _isAllAir = true;

	//	public short[] blocks = new short[16*16*16];
	//	public NibbleArray blocklight = new NibbleArray(16*16*16);
	//	public NibbleArray skylight = new NibbleArray(16*16*16);

	//	private byte[] _cache;
	//	private bool _isDirty;
	//	private object _cacheSync = new object();

	//	public PaletteChunk()
	//	{
	//		ChunkColumn.Fill<byte>(skylight.Data, 0xff);
	//	}

	//	public bool IsDirty => _isDirty;

	//	public override bool IsAllAir()
	//	{
	//		if (_isDirty)
	//		{
	//			_isAllAir = blocks.All(b => b == 0);
	//			_isDirty = false;
	//		}
	//		return _isAllAir;
	//	}

	//	private static int GetIndex(int bx, int by, int bz)
	//	{
	//		return (bx*256) + (bz*16) + by;
	//	}

	//	public override byte GetBlock(int bx, int by, int bz)
	//	{
	//		return (byte) blocks[GetIndex(bx, @by, bz)];
	//	}

	//	public override void SetBlock(int bx, int by, int bz, byte bid)
	//	{
	//		blocks[GetIndex(bx, by, bz)] = bid;
	//		_cache = null;
	//		_isDirty = true;
	//	}

	//	public override byte GetBlocklight(int bx, int by, int bz)
	//	{
	//		return blocklight[GetIndex(bx, by, bz)];
	//	}

	//	public override void SetBlocklight(int bx, int by, int bz, byte data)
	//	{
	//		blocklight[GetIndex(bx, by, bz)] = data;
	//		//_cache = null;
	//		//_isDirty = true;
	//	}

	//	public override byte GetMetadata(int bx, int by, int bz)
	//	{

	//		//return metadata[GetIndex(bx, by, bz)];
	//	}

	//	public override void SetMetadata(int bx, int by, int bz, byte data)
	//	{
	//		//metadata[GetIndex(bx, by, bz)] = data;
	//		_cache = null;
	//		_isDirty = true;
	//	}

	//	public override byte GetSkylight(int bx, int by, int bz)
	//	{
	//		return skylight[GetIndex(bx, by, bz)];
	//	}

	//	public override void SetSkylight(int bx, int by, int bz, byte data)
	//	{
	//		skylight[GetIndex(bx, by, bz)] = data;
	//		//_cache = null;
	//		//_isDirty = true;
	//	}

	//	public override byte[] GetBytes(Stream stream)
	//	{
	//		if (_cache != null) return _cache;

	//		//using (MemoryStream stream = MiNetServer.MemoryStreamManager.GetStream())
	//		{
	//			stream.WriteByte(8); // version

	//			int numberOfStores = 2;
	//			stream.WriteByte((byte) numberOfStores); // storage size

	//			List<int> palette = new List<int>(10);
	//			List<short> paletteHash = new List<short>(10);
	//			byte[] indexes = new byte[blocks.Length];
	//			for (int i = 0; i < numberOfStores; i++)
	//			{
	//				int cacheHits = 0;
	//				int lookups = 0;

	//				palette.Clear();
	//				paletteHash.Clear();

	//				stream.WriteByte((byte) ((8 << 1) | 1)); // version

	//				int index = 0;
	//				short prevHash = 0;
	//				for (int b = 0; b < blocks.Length; b++)
	//				{
	//					byte bid = (byte) blocks[b];
	//					byte data = metadata[b];
	//					short hash = (short) (bid << 4 | (data & 0x0f));
	//					if (hash != prevHash)
	//					{
	//						//index = paletteHash.IndexOf(hash);
	//						index = paletteHash.IndexOf(hash);
	//						if (index == -1)
	//						{
	//							int runtimeId = BlockStateUtils.BlockStates[(bid, data)];
	//							palette.Add(runtimeId);
	//							paletteHash.Add(hash);
	//						}

	//						lookups++;
	//					}
	//					else
	//					{
	//						cacheHits++;
	//					}

	//					indexes[b] = (byte) index;
	//					prevHash = hash;
	//				}

	//				stream.Write(indexes, 0, indexes.Length);

	//				VarInt.WriteSInt32(stream, palette.Count); // count
	//				//Log.Debug($"Palette size = {palette.Count}, {IsAllAir()}, {lookups}, {cacheHits}");
	//				foreach (var val in palette)
	//				{
	//					VarInt.WriteSInt32(stream, val); // air
	//				}
	//			}
	//			//_cache = stream.ToArray();
	//		}

	//		return _cache;
	//	}

	//	//private bool _isAllAir = true;
	//	//private bool _isDirty;

	//	public override object Clone()
	//	{
	//		PaletteChunk cc = CreateObject();
	//		cc._isAllAir = _isAllAir;
	//		cc._isDirty = _isDirty;

	//		blocks.CopyTo(cc.blocks, 0);
	//		//metadata.Data.CopyTo(cc.metadata.Data, 0);
	//		blocklight.Data.CopyTo(cc.blocklight.Data, 0);
	//		skylight.Data.CopyTo(cc.skylight.Data, 0);

	//		if (_cache != null)
	//		{
	//			cc._cache = (byte[]) _cache.Clone();
	//		}

	//		cc._cacheSync = new object();

	//		return cc;
	//	}

	//	private static readonly ChunkPool<PaletteChunk> Pool = new ChunkPool<PaletteChunk>(() => new PaletteChunk());

	//	public static PaletteChunk CreateObject()
	//	{
	//		return Pool.GetObject();
	//	}

	//	public void PutPool()
	//	{
	//		Reset();
	//		Pool.PutObject(this);
	//	}

	//	public override void Reset()
	//	{
	//		_isAllAir = true;
	//		Array.Clear(blocks, 0, blocks.Length);
	//		//Array.Clear(metadata.Data, 0, metadata.Data.Length);
	//		Array.Clear(blocklight.Data, 0, blocklight.Data.Length);
	//		//Array.Clear(skylight.Data, 0, skylight.Data.Length);
	//		ChunkColumn.Fill<byte>(skylight.Data, 0xff);

	//		_cache = null;
	//		_isDirty = false;
	//	}

	//	~PaletteChunk()
	//	{
	//		Log.Error($"Unexpected dispose chunk");
	//	}
	//}
}