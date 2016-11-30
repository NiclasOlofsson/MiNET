using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using fNbt;
using log4net;
using MiNET.Blocks;
using MiNET.Net;
using MiNET.Utils;

namespace MiNET.Worlds
{
	public class ChunkColumn : ICloneable
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (ChunkColumn));

		public bool isAllAir = false;

		public int x;
		public int z;
		public byte[] biomeId = ArrayOf<byte>.Create(256, 1);
		public int[] biomeColor = ArrayOf<int>.Create(256, 0);
		public byte[] height = ArrayOf<byte>.Create(256, 0);

		public byte[] blocks = new byte[16*16*128];
		public NibbleArray metadata = new NibbleArray(16*16*128);
		public NibbleArray blockLight = new NibbleArray(16*16*128);
		public NibbleArray skyLight = new NibbleArray(16*16*128);

		//TODO: This dictionary need to be concurent. Investigate performance before changing.
		public IDictionary<BlockCoordinates, NbtCompound> BlockEntities = new Dictionary<BlockCoordinates, NbtCompound>();

		private byte[] _cache;
		public bool isDirty;
		public bool IsLoaded = false;
		public bool NeedSave = true;
		private McpeBatch _cachedBatch = null;
		private object _cacheSync = new object();

		//~ChunkColumn()
		//{
		//	Log.Warn($"Unexpected dispose chunk. X={x}, Z={z}");
		//}

		public ChunkColumn()
		{
			isDirty = false;
			//Parallel.For(0, skylight.Data.Length, i => skylight.Data[i] = 0xff);

			//for (int i = 0; i < skylight.Data.Length; i++)
			//	skylight.Data[i] = 0xff;

			for (int i = 0; i < biomeColor.Length; i++)
				biomeColor[i] = 8761930; // Grass color?

			//for (int i = 0; i < biomeColor.Length; i++)
			//	biomeColor[i] = Color.FromArgb(0, 255, 204, 51).ToArgb();

			BiomeUtils utils = new BiomeUtils();
			utils.PrecomputeBiomeColors();
		}

		private void SetDirty()
		{
			_cache = null;
			isDirty = true;
			NeedSave = true;
		}

		public byte GetBlock(int bx, int by, int bz)
		{
			return blocks[(bx*2048) + (bz*128) + by];
		}

		public void SetBlock(int bx, int by, int bz, byte bid)
		{
			blocks[(bx*2048) + (bz*128) + by] = bid;
			SetDirty();
		}

		public byte GetMetadata(int bx, int by, int bz)
		{
			return metadata[(bx * 2048) + (bz * 128) + by];
		}

		public void SetMetadata(int bx, int by, int bz, byte data)
		{
			metadata[(bx * 2048) + (bz * 128) + by] = data;
			SetDirty();
		}

		public byte GetHeight(int bx, int bz)
		{
			return height[(bz << 4) + (bx)];
		}

		public void SetHeight(int bx, int bz, byte h)
		{
			height[(bz << 4) + (bx)] = h;
			SetDirty();
		}

		public byte GetBiome(int bx, int bz)
		{
			return biomeId[(bz << 4) + (bx)];
		}

		public void SetBiome(int bx, int bz, byte biome)
		{
			biomeId[(bz << 4) + (bx)] = biome;
			SetDirty();
		}

		public void SetBiomeColor(int bx, int bz, int color)
		{
			biomeColor[(bz << 4) + (bx)] = (color & 0x00ffffff);
			//SetDirty();
		}

		public byte GetBlockLight(int bx, int by, int bz)
		{
			return blockLight[(bx * 2048) + (bz * 128) + by];
		}

		public void SetBlockLight(int bx, int by, int bz, byte data)
		{
			blockLight[(bx * 2048) + (bz * 128) + by] = data;
			SetDirty();
		}

		public byte GetSkyLight(int bx, int by, int bz)
		{
			return skyLight[(bx * 2048) + (bz * 128) + by];
		}

		public void SetSkyLight(int bx, int by, int bz, byte data)
		{
			skyLight[(bx*2048) + (bz*128) + by] = data;
			SetDirty();
		}

		public NbtCompound GetBlockEntity(BlockCoordinates coordinates)
		{
			NbtCompound nbt;
			BlockEntities.TryGetValue(coordinates, out nbt);

			// High cost clone. Consider alternative options on this.
			return (NbtCompound) nbt?.Clone();
		}

		public void SetBlockEntity(BlockCoordinates coordinates, NbtCompound nbt)
		{
			NbtCompound blockEntity = (NbtCompound) nbt.Clone();
			BlockEntities[coordinates] = blockEntity;
			SetDirty();
		}

		public void RemoveBlockEntity(BlockCoordinates coordinates)
		{
			BlockEntities.Remove(coordinates);
			SetDirty();
		}


		/// <summary>Blends the specified colors together.</summary>
		/// <param name="color">Color to blend onto the background color.</param>
		/// <param name="backColor">Color to blend the other color onto.</param>
		/// <param name="amount">How much of <paramref name="color"/> to keep,
		/// “on top of” <paramref name="backColor"/>.</param>
		/// <returns>The blended colors.</returns>
		public static Color Blend(Color color, Color backColor, double amount)
		{
			byte r = (byte) ((color.R*amount) + backColor.R*(1 - amount));
			byte g = (byte) ((color.G*amount) + backColor.G*(1 - amount));
			byte b = (byte) ((color.B*amount) + backColor.B*(1 - amount));
			return Color.FromArgb(r, g, b);
		}

		public Color CombineColors(params Color[] aColors)
		{
			int r = 0;
			int g = 0;
			int b = 0;
			foreach (Color c in aColors)
			{
				r += c.R;
				g += c.G;
				b += c.B;
			}

			r /= aColors.Length;
			g /= aColors.Length;
			b /= aColors.Length;

			return Color.FromArgb(r, g, b);
		}

		private void InterpolateBiomes()
		{
			for (int bx = 0; bx < 16; bx++)
			{
				for (int bz = 0; bz < 16; bz++)
				{
					Color c = CombineColors(
						GetBiomeColor(bx, bz),
						GetBiomeColor(bx - 1, bz - 1),
						GetBiomeColor(bx - 1, bz),
						GetBiomeColor(bx, bz - 1),
						GetBiomeColor(bx + 1, bz + 1),
						GetBiomeColor(bx + 1, bz),
						GetBiomeColor(bx, bz + 1),
						GetBiomeColor(bx - 1, bz + 1),
						GetBiomeColor(bx + 1, bz - 1)
						);
					SetBiomeColor(bx, bz, c.ToArgb());
				}
			}

			//SetBiomeColor(0, 0, Color.GreenYellow.ToArgb());
			//SetBiomeColor(0, 15, Color.Blue.ToArgb());
			//SetBiomeColor(15, 0, Color.Red.ToArgb());
			//SetBiomeColor(15, 15, Color.Yellow.ToArgb());
		}

		private Random random = new Random();

		private Color GetBiomeColor(int bx, int bz)
		{
			if (bx < 0) bx = 0;
			if (bz < 0) bz = 0;
			if (bx > 15) bx = 15;
			if (bz > 15) bz = 15;

			BiomeUtils utils = new BiomeUtils();
			var biome = GetBiome(bx, bz);
			int color = utils.ComputeBiomeColor(biome, 0, true);

			if (random.Next(30) == 0)
			{
				Color col = Color.FromArgb(color);
				color = Color.FromArgb(0, Math.Max(0, col.R - 160), Math.Max(0, col.G - 160), Math.Max(0, col.B - 160)).ToArgb();
			}

			return Color.FromArgb(color);
		}

		public void RecalcHeight()
		{
			for (int x = 0; x < 16; x++)
			{
				for (int z = 0; z < 16; z++)
				{
					bool isInLight = true;

					for (int y = 127; y >= 0; y--)
					{
						if (isInLight)
						{
							if (GetBlock(x, y, z) != 0)
							{
								SetHeight(x, z, (byte)(y + 1));
								SetSkyLight(x, y, z, 0);
								isInLight = false;
							}
							else
							{
								SetSkyLight(x, y, z, 15);
							}
						}
						else
						{
							SetSkyLight(x, y, z, 0);
						}
					}
				}
			}
		}

		public McpeBatch GetBatch()
		{
			lock (_cacheSync)
			{
				if (!isDirty && _cachedBatch != null) return _cachedBatch;

				McpeFullChunkData fullChunkData = McpeFullChunkData.CreateObject();
				fullChunkData.chunkX = x;
				fullChunkData.chunkZ = z;
				fullChunkData.order = 0;
				fullChunkData.chunkData = GetBytes();
				byte[] bytes = fullChunkData.Encode();
				fullChunkData.PutPool();

				var batch = BatchUtils.CreateBatchPacket(bytes, 0, bytes.Length, CompressionLevel.Optimal, true);
				batch.MarkPermanent();
				batch.Encode();

				_cachedBatch = batch;
				_cache = null;
				isDirty = false;

				return batch;
			}
		}


		public byte[] GetBytes()
		{
			if (_cache != null) return _cache;

			using (MemoryStream stream = MiNetServer.MemoryStreamManager.GetStream())
			{
				NbtBinaryWriter writer = new NbtBinaryWriter(stream, true);

				writer.Write(blocks);
				writer.Write(metadata.Data);
				writer.Write(skyLight.Data);
				writer.Write(blockLight.Data);
				writer.Write(height);

				BiomeUtils utils = new BiomeUtils();
				utils.PrecomputeBiomeColors();

				InterpolateBiomes();

				for (int i = 0; i < biomeId.Length; i++)
				{
					var biome = biomeId[i];
					int color = biomeColor[i];
					writer.Write((color & 0x00ffffff) | biome << 24);
				}

				short extraSize = 0;
				writer.Write(extraSize); // No extra data

				if (BlockEntities.Count == 0)
				{
					//NbtFile file = new NbtFile(new NbtCompound(string.Empty)) {BigEndian = false, UseVarInt = true};
					//file.SaveToStream(writer.BaseStream, NbtCompression.None);
				}
				else
				{
					foreach (NbtCompound blockEntity in BlockEntities.Values.ToArray())
					{
						NbtFile file = new NbtFile(blockEntity) {BigEndian = false, UseVarInt = true};
						file.SaveToStream(writer.BaseStream, NbtCompression.None);
					}
				}

				_cache = stream.ToArray();
			}

			return _cache;
		}

		public object Clone()
		{
			ChunkColumn cc = (ChunkColumn) MemberwiseClone();

			//public int x;
			//public int z;
			//public bool isDirty;

			//public byte[] biomeId = ArrayOf<byte>.Create(256, 2);
			cc.biomeId = (byte[]) biomeId.Clone();

			//public int[] biomeColor = ArrayOf<int>.Create(256, 1);
			cc.biomeColor = (int[]) biomeColor.Clone();

			//public byte[] height = ArrayOf<byte>.Create(256, 0);
			cc.height = (byte[]) height.Clone();

			//public byte[] blocks = new byte[16 * 16 * 128];
			cc.blocks = (byte[]) blocks.Clone();

			//public NibbleArray metadata = new NibbleArray(16 * 16 * 128);
			cc.metadata = (NibbleArray) metadata.Clone();

			//public NibbleArray blocklight = new NibbleArray(16 * 16 * 128);
			cc.blockLight = (NibbleArray) blockLight.Clone();

			//public NibbleArray skylight = new NibbleArray(16 * 16 * 128);
			cc.skyLight = (NibbleArray) skyLight.Clone();

			//public IDictionary<BlockCoordinates, NbtCompound> BlockEntities = new Dictionary<BlockCoordinates, NbtCompound>();
			cc.BlockEntities = new Dictionary<BlockCoordinates, NbtCompound>();
			foreach (KeyValuePair<BlockCoordinates, NbtCompound> blockEntityPair in BlockEntities)
			{
				cc.BlockEntities.Add(blockEntityPair.Key, (NbtCompound) blockEntityPair.Value.Clone());
			}

			//private byte[] _cache;
			if (_cache != null)
			{
				cc._cache = (byte[]) _cache.Clone();
			}

			//private McpeBatch _cachedBatch = null;
			McpeBatch batch = McpeBatch.CreateObject();
			batch.payload = _cachedBatch.payload;
			batch.Encode();
			batch.MarkPermanent();

			cc._cachedBatch = batch;

			//private object _cacheSync = new object();
			_cacheSync = new object();

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