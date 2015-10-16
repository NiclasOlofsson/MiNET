using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using fNbt;
using MiNET.Net;
using MiNET.Utils;

namespace MiNET.Worlds
{
	public class ChunkColumn : ICloneable
	{
		public int x;
		public int z;
		public byte[] biomeId = ArrayOf<byte>.Create(256, 2);
		public int[] biomeColor = ArrayOf<int>.Create(256, 1);
		public byte[] height = ArrayOf<byte>.Create(256, 0);

		public byte[] blocks = new byte[16*16*128];
		public NibbleArray metadata = new NibbleArray(16*16*128);
		public NibbleArray blocklight = new NibbleArray(16*16*128);
		public NibbleArray skylight = new NibbleArray(16*16*128);
		public IDictionary<BlockCoordinates, NbtCompound> BlockEntities = new Dictionary<BlockCoordinates, NbtCompound>();

		private byte[] _cache;
		public bool isDirty;

		public ChunkColumn()
		{
			isDirty = false;
			//Parallel.For(0, skylight.Data.Length, i => skylight.Data[i] = 0xff);

			//for (int i = 0; i < skylight.Data.Length; i++)
			//	skylight.Data[i] = 0xff;

			for (int i = 0; i < biomeColor.Length; i++)
				biomeColor[i] = 8761930; // Grass color?
		}

		public byte GetBlock(int bx, int by, int bz)
		{
			return blocks[(bx*2048) + (bz*128) + by];
		}

		public void SetBlock(int bx, int by, int bz, byte bid)
		{
			_cache = null;
			isDirty = true;
			blocks[(bx*2048) + (bz*128) + by] = bid;
		}

		public void SetHeight(int bx, int bz, byte h)
		{
			height[(bx << 4) + (bz)] = h;
		}

		public byte GetBlocklight(int bx, int by, int bz)
		{
			return blocklight[(bx*2048) + (bz*128) + by];
		}

		public void SetBlocklight(int bx, int by, int bz, byte data)
		{
			_cache = null;
			isDirty = true;
			blocklight[(bx*2048) + (bz*128) + by] = data;
		}

		public byte GetMetadata(int bx, int by, int bz)
		{
			return metadata[(bx*2048) + (bz*128) + by];
		}

		public void SetMetadata(int bx, int by, int bz, byte data)
		{
			_cache = null;
			isDirty = true;
			metadata[(bx*2048) + (bz*128) + by] = data;
		}

		public byte GetSkylight(int bx, int by, int bz)
		{
			return skylight[(bx*2048) + (bz*128) + by];
		}

		public void SetSkylight(int bx, int by, int bz, byte data)
		{
			_cache = null;
			isDirty = true;
			skylight[(bx*2048) + (bz*128) + by] = data;
		}

		public NbtCompound GetBlockEntity(BlockCoordinates coordinates)
		{
			NbtCompound nbt;
			BlockEntities.TryGetValue(coordinates, out nbt);
			return nbt;
		}

		public void SetBlockEntity(BlockCoordinates coordinates, NbtCompound nbt)
		{
			_cache = null;
			isDirty = true;
			BlockEntities[coordinates] = nbt;
		}

		public void RemoveBlockEntity(BlockCoordinates coordinates)
		{
			_cache = null;
			isDirty = true;
			BlockEntities.Remove(coordinates);
		}

		public void RecalcHeight()
		{
			for (int x = 0; x < 16; x++)
			{
				for (int z = 0; z < 16; z++)
				{
					for (byte y = 127; y > 0; y--)
					{
						if (GetBlock(x, y, z) != 0)
						{
							SetHeight(x, z, y);
							break;
						}
					}
				}
			}
		}

		private McpeBatch _cachedBatch = null;

		private object _cacheSync = new object();

		public McpeBatch GetBatch()
		{
			lock (_cacheSync)
			{
				if (_cache != null && _cachedBatch != null) return _cachedBatch;

				McpeFullChunkData fullChunkData = McpeFullChunkData.CreateObject();
				fullChunkData.chunkX = x;
				fullChunkData.chunkZ = z;
				fullChunkData.order = 0;
				fullChunkData.chunkData = GetBytes();
				fullChunkData.chunkDataLength = fullChunkData.chunkData.Length;
				byte[] bytes = fullChunkData.Encode();
				fullChunkData.PutPool();

				MemoryStream memStream = new MemoryStream();
				memStream.Write(BitConverter.GetBytes(Endian.SwapInt32(bytes.Length)), 0, 4);
				memStream.Write(bytes, 0, bytes.Length);

				McpeBatch batch = McpeBatch.CreateObject();
				byte[] buffer = Player.CompressBytes(memStream.ToArray(), CompressionLevel.Optimal);
				batch.payloadSize = buffer.Length;
				batch.payload = buffer;
				batch.Encode();
				batch.MarkPermanent();

				_cachedBatch = batch;

				return batch;
			}
		}


		private byte[] GetBytes()
		{
			if (_cache != null) return _cache;

			MemoryStream stream = new MemoryStream();
			{
				NbtBinaryWriter writer = new NbtBinaryWriter(stream, true);

				writer.Write(blocks);
				writer.Write(metadata.Data);
				writer.Write(skylight.Data);
				writer.Write(blocklight.Data);

				//RecalcHeight();
				writer.Write(height);

				for (int i = 0; i < biomeColor.Length; i++)
				{
					writer.Write(biomeColor[i]);
				}

				int extraSize = 0;
				writer.Write(extraSize); // No extra data

				if (BlockEntities.Count == 0)
				{
					NbtFile file = new NbtFile(new NbtCompound(string.Empty)) {BigEndian = false};
					writer.Write(file.SaveToBuffer(NbtCompression.None));
				}
				else
				{
					//TODO: Not working
					foreach (NbtCompound blockEntity in BlockEntities.Values)
					{
						NbtFile file = new NbtFile(blockEntity) {BigEndian = false};
						writer.Write(file.SaveToBuffer(NbtCompression.None));
					}
				}


				writer.Flush();
				writer.Close();
			}

			var bytes = stream.ToArray();
			stream.Close();

			_cache = bytes;

			return bytes;
		}

		public byte[] GetBytes(bool compress)
		{
			if (compress) return GetBytes();

			MemoryStream stream = new MemoryStream();
			NbtBinaryWriter writer = new NbtBinaryWriter(stream, true);

			writer.Write(IPAddress.HostToNetworkOrder(x));
			writer.Write(IPAddress.HostToNetworkOrder(z));

			writer.Write(blocks);
			writer.Write(metadata.Data);
			writer.Write(skylight.Data);
			writer.Write(blocklight.Data);

			writer.Write(biomeId);

			for (int i = 0; i < biomeColor.Length; i++)
			{
				writer.Write(biomeColor[i]);
			}


			writer.Flush();

			writer.Close();
			return stream.ToArray();
		}

		public object Clone()
		{
			ChunkColumn cc = (ChunkColumn) MemberwiseClone();
			cc._cache = _cache;
			cc._cachedBatch = (McpeBatch) _cachedBatch.Clone();
			cc.metadata = (NibbleArray) metadata.Clone();
			cc.blocklight = (NibbleArray) blocklight.Clone();
			cc.skylight = (NibbleArray) skylight.Clone();
			cc.BlockEntities = new ConcurrentDictionary<BlockCoordinates, NbtCompound>();
			foreach (KeyValuePair<BlockCoordinates, NbtCompound> blockEntityPair in BlockEntities)
			{
				cc.BlockEntities.Add(blockEntityPair.Key, (NbtCompound) blockEntityPair.Value.Clone());
			}

			return cc;
		}
	}


	public static class ArrayOf<T> where T : new()
	{
		public static T[] Create(int size, T initialValue)
		{
			T[] array = (T[]) Array.CreateInstance(typeof (T), size);
			for (int i = 0; i < array.Length; i++)
				array[i] = initialValue;
			return array;
		}

		public static T[] Create(int size)
		{
			T[] array = (T[]) Array.CreateInstance(typeof (T), size);
			for (int i = 0; i < array.Length; i++)
				array[i] = new T();
			return array;
		}
	}
}