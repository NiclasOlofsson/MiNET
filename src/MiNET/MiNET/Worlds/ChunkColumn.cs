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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
	public class ChunkColumn : ICloneable, IEnumerable<ChunkBase>
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(ChunkColumn));

		public bool isAllAir = false;
		public bool isNew = true;

		public int x;
		public int z;

		private ChunkBase[] _chunks = new ChunkBase[16];

		public byte[] biomeId = ArrayOf<byte>.Create(256, 1);
		public short[] height = new short[256];

		//TODO: This dictionary need to be concurent. Investigate performance before changing.
		public IDictionary<BlockCoordinates, NbtCompound> BlockEntities = new Dictionary<BlockCoordinates, NbtCompound>();

		public bool isDirty;
		public bool isGenerated;
		public bool IsLoaded = false;
		public bool NeedSave = false;
		private McpeWrapper _cachedBatch = null;
		private object _cacheSync = new object();

		public ChunkColumn()
		{
			for (int i = 0; i < 16; i++)
			{
				_chunks[i] = PaletteChunk.CreateObject();
			}

			isDirty = false;
		}

		private void SetDirty()
		{
			isDirty = true;
			NeedSave = true;
		}

		public int GetBlock(int bx, int by, int bz)
		{
			var chunk = GetChunk(by);
			return chunk.GetBlock(bx, by & 0xf, bz);
		}

		public ChunkBase this[int chunkIndex]
		{
			get
			{
				ChunkBase chunk = _chunks[chunkIndex];
				if (chunk == null)
				{
					chunk = PaletteChunk.CreateObject();
					_chunks[chunkIndex] = chunk;
				}
				return chunk;
			}
			set => _chunks[chunkIndex] = value;
		}

		public ChunkBase GetChunk(int by)
		{
			ChunkBase chunk = _chunks[by >> 4];
			if (chunk == null)
			{
				chunk = PaletteChunk.CreateObject();
				_chunks[by >> 4] = chunk;
			}
			return chunk;
		}

		public void SetBlock(int bx, int by, int bz, int bid)
		{
			var chunk = GetChunk(by);
			chunk.SetBlock(bx, by - 16 * (by >> 4), bz, bid);
			SetDirty();
		}

		public void SetHeight(int bx, int bz, short h)
		{
			height[((bz << 4) + (bx))] = h;
			SetDirty();
		}

		public byte GetHeight(int bx, int bz)
		{
			return (byte) height[((bz << 4) + (bx))];
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
			var chunk = GetChunk(by);
			return chunk.GetBlocklight(bx, by - 16 * (by >> 4), bz);
		}

		public void SetBlocklight(int bx, int by, int bz, byte data)
		{
			var chunk = GetChunk(by);
			chunk.SetBlocklight(bx, by - 16 * (by >> 4), bz, data);
			//SetDirty();
		}

		public byte GetMetadata(int bx, int by, int bz)
		{
			var chunk = GetChunk(by);
			return chunk.GetMetadata(bx, by - 16 * (by >> 4), bz);
		}

		public void SetMetadata(int bx, int by, int bz, byte data)
		{
			var chunk = GetChunk(by);
			chunk.SetMetadata(bx, by - 16 * (by >> 4), bz, data);
			SetDirty();
		}

		public byte GetSkylight(int bx, int by, int bz)
		{
			var chunk = GetChunk(by);
			return chunk.GetSkylight(bx, by - 16 * (by >> 4), bz);
		}

		public void SetSkyLight(int bx, int by, int bz, byte data)
		{
			var chunk = GetChunk(by);
			chunk.SetSkylight(bx, by - 16 * (by >> 4), bz, data);
			//SetDirty();
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
		/// <param name="amount">
		///     How much of <paramref name="color" /> to keep,
		///     “on top of” <paramref name="backColor" />.
		/// </param>
		/// <returns>The blended colors.</returns>
		public static Color Blend(Color color, Color backColor, double amount)
		{
			byte r = (byte) ((color.R * amount) + backColor.R * (1 - amount));
			byte g = (byte) ((color.G * amount) + backColor.G * (1 - amount));
			byte b = (byte) ((color.B * amount) + backColor.B * (1 - amount));
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

		public static void Fill<T>(T[] destinationArray, params T[] value)
		{
			if (destinationArray == null)
			{
				throw new ArgumentNullException(nameof(destinationArray));
			}

			if (value.Length >= destinationArray.Length)
			{
				throw new ArgumentException("Length of value array must be less than length of destination");
			}

			// set the initial array value
			Array.Copy(value, destinationArray, value.Length);

			int arrayToFillHalfLength = destinationArray.Length / 2;
			int copyLength;

			for (copyLength = value.Length; copyLength < arrayToFillHalfLength; copyLength <<= 1)
			{
				Array.Copy(destinationArray, 0, destinationArray, copyLength, copyLength);
			}

			Array.Copy(destinationArray, 0, destinationArray, copyLength, destinationArray.Length - copyLength);
		}

		public void RecalcHeight()
		{
			for (int x = 0; x < 16; x++)
			{
				for (int z = 0; z < 16; z++)
				{
					RecalcHeight(x, z);
				}
			}
		}

		public void RecalcHeight(int x, int z, int startY = 255)
		{
			bool isInLight = true;
			bool isInAir = true;

			for (int y = startY; y >= 0; y--)
			{
				if (isInLight)
				{
					ChunkBase chunk = GetChunk(y);
					if (isInAir && chunk.IsAllAir())
					{
						if (chunk.IsDirty) Array.Fill<byte>(chunk.skylight.Data, 0xff);
						y -= 15;
						continue;
					}

					isInAir = false;

					int bid = GetBlock(x, y, z);
					if (bid == 0 || (BlockFactory.TransparentBlocks[bid] == 1 && bid != 18 && bid != 30 && bid != 8 && bid != 9))
					{
						SetSkyLight(x, y, z, 15);
					}
					else
					{
						SetHeight(x, z, (short) (y + 1));
						SetSkyLight(x, y, z, 0);
						isInLight = false;
					}
				}
				else
				{
					SetSkyLight(x, y, z, 0);
				}
			}
		}

		public int GetRecalatedHeight(int x, int z)
		{
			bool isInAir = true;

			for (int y = 255; y >= 0; y--)
			{
				{
					ChunkBase chunk = GetChunk(y);
					if (isInAir && chunk.IsAllAir())
					{
						if (chunk.IsDirty) Array.Fill<byte>(chunk.skylight.Data, 0xff);
						y -= 15;
						continue;
					}

					isInAir = false;

					int bid = GetBlock(x, y, z);
					if (bid == 0 || (BlockFactory.TransparentBlocks[bid] == 1 && bid != 18 && bid != 30))
					{
						continue;
					}

					return y + 1;
				}
			}

			return 0;
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

		private static long max = 0;
		private static long count = 0;
		private static double average = 0;
		private static double averageSize = 0;
		private static double averageCompressedSize = 0;

		public McpeWrapper GetBatch()
		{
			lock (_cacheSync)
			{
				if (!isDirty && _cachedBatch != null) return _cachedBatch;

				var sw = Stopwatch.StartNew();

				ClearCache();

				var fullChunkData = McpeLevelChunk.CreateObject();
				fullChunkData.chunkX = x;
				fullChunkData.chunkZ = z;

				int topEmpty = GetTopEmpty();
				fullChunkData.subChunkCount = (uint) topEmpty;

				fullChunkData.cacheEnabled = false;

				var chunkData = GetBytes();

				fullChunkData.chunkData = chunkData;
				byte[] bytes = fullChunkData.Encode();

				fullChunkData.PutPool();

				var fullChunkSize = bytes.Length;
				averageSize = ((averageSize * count) + fullChunkSize) / (count + 1);

				McpeWrapper batch = BatchUtils.CreateBatchPacket(new Memory<byte>(bytes, 0, bytes.Length), CompressionLevel.Fastest, true);
				batch.MarkPermanent();
				var wrapperData = batch.Encode();

				long elapsted = sw.ElapsedTicks;
				average = ((average * count) + elapsted) / (count + 1);

				var wrapperSize = wrapperData.Length;
				averageCompressedSize = ((averageCompressedSize * count) + wrapperSize) / (count + 1);


				//Log.Debug($"Serialized in {elapsted / (float)TimeSpan.TicksPerMillisecond:F4} ms, Average={average / (float)TimeSpan.TicksPerMillisecond:F4}, fcsize={averageSize:F0}, wsize={averageCompressedSize:F0}");

				_cachedBatch = batch;
				isDirty = false;

				count++;

				return _cachedBatch;
			}
		}


		public byte[] GetBytes()
		{
			using (var stream = new MemoryStream())
			{

				//TODO: Expensive since already done outside this method.
				int topEmpty = GetTopEmpty();

				int sent = 0;
				for (int ci = 0; ci < topEmpty; ci++)
				{
					_chunks[ci].GetBytes(stream);
					sent++;
				}

				//byte[] ba = new byte[512];
				//Buffer.BlockCopy(height, 0, ba, 0, 512);
				//stream.Write(ba, 0, ba.Length);

				stream.Write(biomeId, 0, biomeId.Length);

				stream.WriteByte(0); // Border blocks - nope

				if (BlockEntities.Count != 0)
				{
					foreach (NbtCompound blockEntity in BlockEntities.Values.ToArray())
					{
						var file = new NbtFile(blockEntity) {BigEndian = false, UseVarInt = true};
						file.SaveToStream(stream, NbtCompression.None);
					}
				}

				return stream.ToArray();
			}
		}

		private int GetTopEmpty()
		{
			int topEmpty = 16;
			for (int ci = 15; ci >= 0; ci--)
			{
				if (_chunks[ci] == null || _chunks[ci].IsAllAir())
				{
					topEmpty = ci;
					_chunks[ci]?.PutPool();
					_chunks[ci] = null;
				}
				else
				{
					break;
				}
			}
			return topEmpty;
		}

		public object Clone()
		{
			ChunkColumn cc = (ChunkColumn) MemberwiseClone();

			cc._chunks = new ChunkBase[16];
			for (int i = 0; i < _chunks.Length; i++)
			{
				cc._chunks[i] = (PaletteChunk) _chunks[i]?.Clone();
			}

			cc.biomeId = (byte[]) biomeId.Clone();
			cc.height = (short[]) height.Clone();

			cc.BlockEntities = new Dictionary<BlockCoordinates, NbtCompound>();
			foreach (KeyValuePair<BlockCoordinates, NbtCompound> blockEntityPair in BlockEntities)
			{
				cc.BlockEntities.Add(blockEntityPair.Key, (NbtCompound) blockEntityPair.Value.Clone());
			}

			McpeWrapper batch = McpeWrapper.CreateObject();
			batch.payload = _cachedBatch.payload;
			batch.Encode();
			batch.MarkPermanent();

			cc._cachedBatch = batch;

			cc._cacheSync = new object();

			return cc;
		}

		public IEnumerator<ChunkBase> GetEnumerator()
		{
			return _chunks.Where(c => c != null).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}


	public static class ArrayOf<T> where T : new()
	{
		public static T[] Create(int size, T initialValue)
		{
			T[] array = (T[]) Array.CreateInstance(typeof(T), size);
			for (int i = 0; i < array.Length; i++)
				array[i] = initialValue;
			return array;
		}

		public static T[] Create(int size)
		{
			T[] array = (T[]) Array.CreateInstance(typeof(T), size);
			for (int i = 0; i < array.Length; i++)
				array[i] = new T();
			return array;
		}
	}
}