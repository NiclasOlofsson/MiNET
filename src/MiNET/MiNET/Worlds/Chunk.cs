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
using System.IO;
using log4net;
using MiNET.Utils;

namespace MiNET.Worlds
{
	public abstract class ChunkBase : ICloneable
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(ChunkBase));

		public ChunkBase()
		{
			Array.Fill<byte>(skylight.Data, 0xff);
		}

		public abstract bool IsDirty { get; }

		public NibbleArray blocklight = new NibbleArray(16 * 16 * 16);
		public NibbleArray skylight = new NibbleArray(16 * 16 * 16);

		public abstract bool IsAllAir();

		public abstract int GetBlock(int bx, int by, int bz);

		public abstract void SetBlock(int bx, int by, int bz, int bid);

		public abstract void SetBlock(int bx, int @by, int bz, int bid, byte data);

		public abstract byte GetMetadata(int bx, int by, int bz);

		public abstract void SetMetadata(int bx, int by, int bz, byte data);

		public byte GetBlocklight(int bx, int by, int bz)
		{
			return blocklight[GetIndex(bx, by, bz)];
		}

		public void SetBlocklight(int bx, int by, int bz, byte data)
		{
			blocklight[GetIndex(bx, by, bz)] = data;
		}

		public virtual byte GetSkylight(int bx, int by, int bz)
		{
			return skylight[GetIndex(bx, by, bz)];
		}

		public virtual void SetSkylight(int bx, int by, int bz, byte data)
		{
			skylight[GetIndex(bx, by, bz)] = data;
		}

		public abstract byte[] GetBytes(Stream stream);

		public abstract object Clone();

		public abstract void PutPool();

		public abstract void Reset();

		private static int GetIndex(int bx, int by, int bz)
		{
			return (bx * 256) + (bz * 16) + by;
		}

	}

	//public class Chunk : ChunkBase
	//{
	//	private static readonly ILog Log = LogManager.GetLogger(typeof(Chunk));

	//	private bool _isAllAir = true;

	//	private byte[] _blocks = new byte[16*16*16];
	//	private NibbleArray _metadata = new NibbleArray(16*16*16);

	//	private byte[] _cache;
	//	private bool _isDirty;

	//	public Chunk()
	//	{
	//		Array.Fill<byte>(skylight.Data, 0xff);
	//	}

	//	public override bool IsDirty => _isDirty;

	//	public override bool IsAllAir()
	//	{
	//		if (_isDirty)
	//		{
	//			_isAllAir = _blocks.All(b => b == 0);
	//			_isDirty = false;
	//		}
	//		return _isAllAir;
	//	}

	//	private static int GetIndex(int bx, int by, int bz)
	//	{
	//		return (bx*256) + (bz*16) + by;
	//	}

	//	public override int GetBlock(int bx, int by, int bz)
	//	{
	//		return _blocks[GetIndex(bx, by, bz)];
	//	}

	//	public override void SetBlock(int bx, int by, int bz, int bid)
	//	{
	//		_blocks[GetIndex(bx, by, bz)] = (byte) bid;
	//		_cache = null;
	//		_isDirty = true;
	//	}

	//	public override byte GetMetadata(int bx, int by, int bz)
	//	{
	//		return _metadata[GetIndex(bx, by, bz)];
	//	}

	//	public override void SetMetadata(int bx, int by, int bz, byte data)
	//	{
	//		_metadata[GetIndex(bx, by, bz)] = data;
	//		_cache = null;
	//		_isDirty = true;
	//	}

	//	public override byte[] GetBytes(Stream stream)
	//	{
	//		if (_cache != null) return _cache;

	//		//using (MemoryStream stream = MiNetServer.MemoryStreamManager.GetStream())
	//		{
	//			stream.WriteByte((byte) 0); // version
	//			stream.Write(_blocks, 0, _blocks.Length);
	//			stream.Write(_metadata.Data, 0, _metadata.Data.Length);
	//			//writer.Write(skylight.Data);
	//			//writer.Write(blocklight.Data);
	//			//_cache = stream.ToArray();
	//		}

	//		return _cache;
	//	}

	//	public override object Clone()
	//	{
	//		Chunk cc = CreateObject();
	//		cc._isAllAir = _isAllAir;
	//		cc._isDirty = _isDirty;

	//		_blocks.CopyTo(cc._blocks, 0);
	//		_metadata.Data.CopyTo(cc._metadata.Data, 0);
	//		blocklight.Data.CopyTo(cc.blocklight.Data, 0);
	//		skylight.Data.CopyTo(cc.skylight.Data, 0);

	//		if (_cache != null)
	//		{
	//			cc._cache = (byte[]) _cache.Clone();
	//		}

	//		return cc;
	//	}

	//	private static readonly ChunkPool<Chunk> Pool = new ChunkPool<Chunk>(() => new Chunk());

	//	public static Chunk CreateObject()
	//	{
	//		return Pool.GetObject();
	//	}

	//	public override void PutPool()
	//	{
	//		Reset();
	//		Pool.PutObject(this);
	//	}

	//	public override void Reset()
	//	{
	//		_isAllAir = true;
	//		Array.Clear(_blocks, 0, _blocks.Length);
	//		Array.Clear(_metadata.Data, 0, _metadata.Data.Length);
	//		Array.Clear(blocklight.Data, 0, blocklight.Data.Length);
	//		Array.Clear(skylight.Data, 0, skylight.Data.Length);
	//		Array.Fill<byte>(skylight.Data, 0xff);
	//		_cache = null;
	//		_isDirty = false;
	//	}

	//	~Chunk()
	//	{
	//		Log.Error($"Unexpected dispose chunk");
	//	}
	//}

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
}