using System;
using System.Collections.Generic;
using System.Drawing;
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

		public bool isAllAir = false;

		public byte[] biomeId = ArrayOf<byte>.Create(256, 1);
		public int[] biomeColor = ArrayOf<int>.Create(256, 0);
		public byte[] height = ArrayOf<byte>.Create(256*2, 0);

		public byte[] blocks = new byte[16*16*16];
		public NibbleArray metadata = new NibbleArray(16*16*16);
		public NibbleArray blocklight = new NibbleArray(16*16*16);
		public NibbleArray skylight = new NibbleArray(16*16*16);

		//TODO: This dictionary need to be concurent. Investigate performance before changing.
		public IDictionary<BlockCoordinates, NbtCompound> BlockEntities = new Dictionary<BlockCoordinates, NbtCompound>();

		private byte[] _cache;
		public bool isDirty;
		private object _cacheSync = new object();

		public Chunk()
		{
			isDirty = false;

			for (int i = 0; i < biomeColor.Length; i++)
				biomeColor[i] = 8761930; // Grass color?

			//BiomeUtils utils = new BiomeUtils();
			//utils.PrecomputeBiomeColors();
		}

		public bool IsAllAir()
		{
			return blocks.All(b => b == 0);
		}

		private static int GetIndex(int bx, int by, int bz)
		{
			return (bx * 256) + (bz * 16) + by;
		}

		public byte GetBlock(int bx, int by, int bz)
		{
			return blocks[GetIndex(bx, by, bz)];
		}

		public void SetBlock(int bx, int by, int bz, byte bid)
		{
			blocks[GetIndex(bx, by, bz)] = bid;
			_cache = null;
			isDirty = true;
		}

		public void SetHeight(int bx, int bz, byte h)
		{
			height[(bz << 4) + (bx)] = h;
		}

		public byte GetHeight(int bx, int bz)
		{
			return height[(bz << 4) + (bx)];
		}

		public void SetBiome(int bx, int bz, byte biome)
		{
			biomeId[(bz << 4) + (bx)] = biome;
		}

		public byte GetBiome(int bx, int bz)
		{
			return biomeId[(bz << 4) + (bx)];
		}

		public void SetBiomeColor(int bx, int bz, int color)
		{
			biomeColor[(bz << 4) + (bx)] = (color & 0x00ffffff);
		}

		public byte GetBlocklight(int bx, int by, int bz)
		{
			return blocklight[GetIndex(bx, by, bz)];
		}

		public void SetBlocklight(int bx, int by, int bz, byte data)
		{
			blocklight[GetIndex(bx, by, bz)] = data;
			_cache = null;
			isDirty = true;
		}

		public byte GetMetadata(int bx, int by, int bz)
		{
			return metadata[GetIndex(bx, by, bz)];
		}

		public void SetMetadata(int bx, int by, int bz, byte data)
		{
			metadata[GetIndex(bx, by, bz)] = data;
			_cache = null;
			isDirty = true;
		}

		public byte GetSkylight(int bx, int by, int bz)
		{
			return skylight[GetIndex(bx, by, bz)];
		}

		public void SetSkylight(int bx, int by, int bz, byte data)
		{
			skylight[GetIndex(bx, by, bz)] = data;
			_cache = null;
			isDirty = true;
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
			_cache = null;
			isDirty = true;
		}

		public void RemoveBlockEntity(BlockCoordinates coordinates)
		{
			BlockEntities.Remove(coordinates);
			_cache = null;
			isDirty = true;
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
			cc.blocklight = (NibbleArray) blocklight.Clone();

			//public NibbleArray skylight = new NibbleArray(16 * 16 * 128);
			cc.skylight = (NibbleArray) skylight.Clone();

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

			_cacheSync = new object();

			return cc;
		}
	}
}