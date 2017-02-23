using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using fNbt;
using log4net;
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

		public Chunk[] chunks = ArrayOf<Chunk>.Create(16);

		public byte[] biomeId = ArrayOf<byte>.Create(256, 1);
		public byte[] height = ArrayOf<byte>.Create(256*2, 0);

		//TODO: This dictionary need to be concurent. Investigate performance before changing.
		public IDictionary<BlockCoordinates, NbtCompound> BlockEntities = new Dictionary<BlockCoordinates, NbtCompound>();

		private byte[] _cache;
		public bool isDirty;
		public bool IsLoaded = false;
		public bool NeedSave = true;
		private McpeBatch _cachedBatch = null;
		private object _cacheSync = new object();

		public ChunkColumn()
		{
			isDirty = false;
			//BiomeUtils utils = new BiomeUtils();
			//utils.PrecomputeBiomeColors();
		}

		private void SetDirty()
		{
			_cache = null;
			isDirty = true;
			NeedSave = true;
		}

		public byte GetBlock(int bx, int by, int bz)
		{
			Chunk chunk = chunks[by >> 4];
			return chunk.GetBlock(bx, by - 16*(by >> 4), bz);
		}

		public void SetBlock(int bx, int by, int bz, byte bid)
		{
			//int i = 30 - (16*(30 >> 4));

			Chunk chunk = chunks[by >> 4];
			chunk.SetBlock(bx, by - 16*(by >> 4), bz, bid);
			SetDirty();
		}

		public void SetHeight(int bx, int bz, byte h)
		{
			height[(bz << 4) + (bx)] = h;
			SetDirty();
		}

		public byte GetHeight(int bx, int bz)
		{
			return height[(bz << 4) + (bx)];
		}

		public void SetBiome(int bx, int bz, byte biome)
		{
			biomeId[(bz << 4) + (bx)] = biome;
			SetDirty();
		}

		public byte GetBiome(int bx, int bz)
		{
			return biomeId[(bz << 4) + (bx)];
		}

		public byte GetBlocklight(int bx, int by, int bz)
		{
			Chunk chunk = chunks[by >> 4];
			return chunk.GetBlocklight(bx, by - 16*(by >> 4), bz);
		}

		public void SetBlocklight(int bx, int by, int bz, byte data)
		{
			Chunk chunk = chunks[by >> 4];
			chunk.SetBlocklight(bx, by - 16*(by >> 4), bz, data);
			SetDirty();
		}

		public byte GetMetadata(int bx, int by, int bz)
		{
			Chunk chunk = chunks[by >> 4];
			return chunk.GetMetadata(bx, by - 16*(by >> 4), bz);
		}

		public void SetMetadata(int bx, int by, int bz, byte data)
		{
			Chunk chunk = chunks[by >> 4];
			chunk.SetMetadata(bx, by - 16*(by >> 4), bz, data);
			SetDirty();
		}

		public byte GetSkylight(int bx, int by, int bz)
		{
			Chunk chunk = chunks[by >> 4];
			return chunk.GetSkylight(bx, by - 16*(by >> 4), bz);
		}

		public void SetSkyLight(int bx, int by, int bz, byte data)
		{
			Chunk chunk = chunks[by >> 4];
			chunk.SetSkylight(bx, by - 16*(by >> 4), bz, data);
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
					//SetBiomeColor(bx, bz, c.ToArgb());
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

					for (int y = 255; y >= 0; y--)
					{
						if (isInLight)
						{
							if (GetBlock(x, y, z) != 0)
							{
								SetHeight(x, z, (byte) (y + 1));
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

		internal void ClearCache()
		{
			lock (_cacheSync)
			{
				if (_cachedBatch != null)
				{
					_cachedBatch.MarkPermanent(false);
					_cachedBatch.PutPool();

					_cachedBatch = null;
				}
			}
		}

		public McpeBatch GetBatch()
		{
			lock (_cacheSync)
			{
				if (!isDirty && _cachedBatch != null) return _cachedBatch;

				ClearCache();

				McpeFullChunkData fullChunkData = McpeFullChunkData.CreateObject();
				fullChunkData.chunkX = x;
				fullChunkData.chunkZ = z;
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

				int topEmpty = 16;
				for (int ci = 15; ci >= 0; ci--)
				{
					if (chunks[ci].IsAllAir()) topEmpty = ci;
					else break;
				}

				writer.Write((byte) topEmpty);

				int sent = 0;
				for (int ci = 0; ci < topEmpty; ci++)
				{
					writer.Write((byte) 0);
					writer.Write(chunks[ci].GetBytes());
					sent++;
				}

				Log.Debug($"Saved sending {16 - sent} chunks");

				//RecalcHeight();
				writer.Write(height);

				//BiomeUtils utils = new BiomeUtils();
				//utils.PrecomputeBiomeColors();

				//InterpolateBiomes();

				writer.Write(biomeId);

				//for (int i = 0; i < biomeId.Length; i++)
				//{
				//	//var biome = biomeId[i];
				//	int color = biomeColor[i];
				//	writer.Write((int) (color & 0x00ffffff) /*| biome << 24*/);
				//}

				//short extraSize = 0;
				//writer.Write(extraSize); // No extra data

				// Count = SignedVarInt (zigzag)
				// Each entry
				// - Hash SignedVarint x << 12, z << 8, y
				// - Block data short

				writer.Write((byte) 0); // Border blocks - nope

				VarInt.WriteSInt32(stream, 0); // Block extradata count
				//VarInt.WriteSInt32(stream, 2);
				//VarInt.WriteSInt32(stream, 1 << 12 | 1 << 8 | 4);
				//writer.Write((byte)31);
				//writer.Write((byte)0);

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

			cc.chunks = new Chunk[16];
			for (int i = 0; i < chunks.Length; i++)
			{
				cc.chunks[i] = (Chunk) chunks[i].Clone();
			}

			cc.biomeId = (byte[]) biomeId.Clone();
			cc.height = (byte[]) height.Clone();

			cc.BlockEntities = new Dictionary<BlockCoordinates, NbtCompound>();
			foreach (KeyValuePair<BlockCoordinates, NbtCompound> blockEntityPair in BlockEntities)
			{
				cc.BlockEntities.Add(blockEntityPair.Key, (NbtCompound) blockEntityPair.Value.Clone());
			}

			if (_cache != null)
			{
				cc._cache = (byte[]) _cache.Clone();
			}

			McpeBatch batch = McpeBatch.CreateObject();
			batch.payload = _cachedBatch.payload;
			batch.Encode();
			batch.MarkPermanent();

			cc._cachedBatch = batch;

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