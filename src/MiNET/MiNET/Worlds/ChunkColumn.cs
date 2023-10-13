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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2020 Niclas Olofsson.
// All Rights Reserved.

#endregion

using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.CompilerServices;
using fNbt;
using log4net;
using MiNET.Blocks;
using MiNET.Net;
using MiNET.Utils.IO;
using MiNET.Utils.Vectors;

namespace MiNET.Worlds
{
	public class ChunkColumn : ICloneable, IEnumerable<SubChunk>, IDisposable
	{
		public const int WorldHeight = 384;
		public const int WorldMaxY = WorldHeight + WorldMinY;
		public const int WorldMinY = -64;
		
		private static readonly ILog Log = LogManager.GetLogger(typeof(ChunkColumn));

		public int X { get; set; }
		public int Z { get; set; }

		public bool IsAllAir { get; set; }

		public short[] height;

		//TODO: This dictionary need to be concurrent. Investigate performance before changing.
		public IDictionary<BlockCoordinates, NbtCompound> BlockEntities { get; private set; } = new Dictionary<BlockCoordinates, NbtCompound>();

		private SubChunk[] _subChunks = new SubChunk[WorldHeight >> 4];

		private SubChunkFactory _subChunkFactory;

		public int Length => _subChunks.Length;

		// Cache related. Should actually all be private, but well
		public bool IsDirty { get; set; }
		public bool NeedSave { get; set; }

		public bool DisableCache { get; set; }
		private McpeWrapper _cachedBatch;
		private object _cacheSync = new object();

		public ChunkColumn() : this((x, z, i) => new SubChunk(x, z, i))
		{

		}

		public ChunkColumn(SubChunkFactory subChunkFactory)
		{
			_subChunkFactory = subChunkFactory;

			height = ArrayPool<short>.Shared.Rent(256);

			IsDirty = false;
		}

		private void SetDirty()
		{
			IsDirty = true;
			NeedSave = true;
		}


		public SubChunk this[int chunkIndex, bool generateIfMissing = true]
		{
			get
			{
				SubChunk subChunk = _subChunks[chunkIndex];
				if (generateIfMissing && subChunk == null)
				{
					subChunk = _subChunkFactory(X, Z, chunkIndex);
					_subChunks[chunkIndex] = subChunk;
				}
				return subChunk;
			}
			set => _subChunks[chunkIndex] = value;
		}

		public int Count()
		{
			return _subChunks.Count(s => s != null);
		}

		public SubChunk GetSubChunk(int by)
		{
			by >>= 4;
			by += WorldMinY < 0 ? Math.Abs(WorldMinY >> 4) : 0;
			
			return this[Math.Clamp(by, 0, _subChunks.Length - 1)];
		}

		public int GetBlockRuntimeId(int bx, int by, int bz)
		{
			var subChunk = GetSubChunk(by);
			return subChunk.GetBlockRuntimeId(bx, by & 0xf, bz);
		}

		public Block GetBlockObject(int bx, int @by, int bz)
		{
			var subChunk = GetSubChunk(by);
			return subChunk.GetBlockObject(bx, by & 0xf, bz);
		}

		public void SetBlock(int bx, int by, int bz, Block block)
		{
			var subChunk = GetSubChunk(by);
			subChunk.SetBlock(bx, by & 0xf, bz, block);
			SetDirty();
		}

		public void SetBlockByRuntimeId(int bx, int by, int bz, int runtimeId)
		{
			var subChunk = GetSubChunk(by);
			subChunk.SetBlockByRuntimeId(bx, by & 0xf, bz, runtimeId);
			SetDirty();
		}

		public void SetHeight(int bx, int bz, short h)
		{
			height[((bz << 4) + (bx))] = (short) (h - WorldMinY);
			SetDirty();
		}

		public short GetHeight(int bx, int bz)
		{
			return (short) (height[((bz << 4) + (bx))] + WorldMinY);
		}

		public void SetBiome(int bx, int by, int bz, byte biome)
		{
			var subChunk = GetSubChunk(by);
			subChunk.SetBiome(bx, by & 0xf, bz, biome);
			SetDirty();
		}

		public byte GetBiome(int bx, int by, int bz)
		{
			var subChunk = GetSubChunk(by);
			return subChunk.GetBiome(bx, by & 0xf, bz);
		}

		public byte GetBlocklight(int bx, int by, int bz)
		{
			var subChunk = GetSubChunk(by);
			return subChunk.GetBlocklight(bx, by & 0xf, bz);
		}

		public void SetBlocklight(int bx, int by, int bz, byte data)
		{
			var subChunk = GetSubChunk(by);
			subChunk.SetBlocklight(bx, by & 0xf, bz, data);
		}

		public byte GetSkylight(int bx, int by, int bz)
		{
			var subChunk = GetSubChunk(by);
			return subChunk.GetSkylight(bx, by & 0xf, bz);
		}

		public void SetSkyLight(int bx, int by, int bz, byte data)
		{
			var subChunk = GetSubChunk(by);
			subChunk.SetSkylight(bx, by & 0xf, bz, data);
		}

		public NbtCompound GetBlockEntity(BlockCoordinates coordinates)
		{
			BlockEntities.TryGetValue(coordinates, out NbtCompound nbt);

			// High cost clone. Consider alternative options on this.
			return (NbtCompound) nbt?.Clone();
		}

		public void SetBlockEntity(BlockCoordinates coordinates, NbtCompound nbt)
		{
			var blockEntity = (NbtCompound) nbt.Clone();
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

		public static unsafe void FastFill<T>(ref T[] data, T value2, ulong value) where T : unmanaged
		{
			fixed (T* shorts = data)
			{
				byte* bytes = (byte*) shorts;
				int len = data.Length * sizeof(T);
				int rem = len % (sizeof(long) * 16);
				ulong* b = (ulong*) bytes;
				ulong* e = (ulong*) (shorts + len - rem);

				while (b < e)
				{
					*(b) = value;
					*(b + 1) = value;
					*(b + 2) = value;
					*(b + 3) = value;
					*(b + 4) = value;
					*(b + 5) = value;
					*(b + 6) = value;
					*(b + 7) = value;
					*(b + 8) = value;
					*(b + 9) = value;
					*(b + 10) = value;
					*(b + 11) = value;
					*(b + 12) = value;
					*(b + 13) = value;
					*(b + 14) = value;
					*(b + 15) = value;
					b += 16;
				}

				for (int i = 0; i < rem; i++)
				{
					data[len - 1 - i] = value2;
				}
			}
		}


		public static void Fill<T>(T[] destinationArray, params T[] value)
		{
			if (destinationArray == null)
			{
				throw new ArgumentNullException(nameof(destinationArray));
			}

			if (destinationArray.Length == 1 && value.Length == 1)
			{
				destinationArray[0] = value[0];
				return;
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

		public void RecalcHeight(int x, int z, int startY = WorldMaxY)
		{
			bool isInLight = true;
			bool isInAir = true;

			for (int y = startY; y >= 0; y--)
			{
				if (isInLight)
				{
					SubChunk chunk = GetSubChunk(y);
					if (isInAir && chunk.IsAllAir())
					{
						if (chunk.IsDirty) Array.Fill<byte>(chunk.SkyLight.Data, 0xff);
						y -= 15;
						continue;
					}

					isInAir = false;

					int bid = GetBlockRuntimeId(x, y, z);
					if (bid < 0 || bid >= BlockFactory.TransparentBlocks.Length) Log.Warn($"{bid}");
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

			for (int y = WorldHeight; y >= WorldMinY; y--)
			{
				{
					SubChunk chunk = GetSubChunk(y);
					if (isInAir && chunk.IsAllAir())
					{
						if (chunk.IsDirty) Array.Fill<byte>(chunk.SkyLight.Data, 0xff);
						y -= 15;
						continue;
					}

					isInAir = false;

					int bid = GetBlockRuntimeId(x, y, z);
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

		public McpeWrapper GetBatch()
		{
			lock (_cacheSync)
			{
				if (!DisableCache && !IsDirty && _cachedBatch != null) return _cachedBatch;

				ClearCache();

				int topEmpty = GetTopEmpty();
				byte[] chunkData = GetBytes(topEmpty);

				var fullChunkPacket = McpeLevelChunk.CreateObject();
				fullChunkPacket.cacheEnabled = false;
				fullChunkPacket.subChunkRequestMode = SubChunkRequestMode.SubChunkRequestModeLegacy;
				fullChunkPacket.chunkX = X;
				fullChunkPacket.chunkZ = Z;
				fullChunkPacket.subChunkCount = (uint) topEmpty;
				fullChunkPacket.chunkData = chunkData;
				byte[] bytes = fullChunkPacket.Encode();
				fullChunkPacket.PutPool();

				McpeWrapper batch = BatchUtils.CreateBatchPacket(new Memory<byte>(bytes, 0, bytes.Length), CompressionLevel.Fastest, true);
				batch.MarkPermanent();

				_cachedBatch = batch;
				IsDirty = false;

				return _cachedBatch;
			}
		}


		public byte[] GetBytes(int topEmpty)
		{
			using var stream = new MemoryStream();

			for (int ci = 0; ci < topEmpty; ci++)
			{
				this[ci].Write(stream);
			}

			WriteBiomePalette(stream);

			stream.WriteByte(0); // Border blocks - nope (EDU)

			if (BlockEntities.Count != 0)
			{
				foreach (NbtCompound blockEntity in BlockEntities.Values.ToArray())
				{
					var file = new NbtFile(blockEntity)
					{
						BigEndian = false,
						UseVarInt = true
					};
					file.SaveToStream(stream, NbtCompression.None);
				}
			}

			return stream.ToArray();
		}

		private void WriteBiomePalette(MemoryStream stream)
		{
			var emptySubChunkBiomes = new byte[16 * 16 * 16];
			var emptySubChunkUniqueBiomes = new List<int>() { 1 };
			Array.Fill(emptySubChunkBiomes, (byte) emptySubChunkUniqueBiomes.Single());

			for (int i = 0; i < 24; i++)
			{
				var subChunk = this[i];

				if (subChunk == null)
				{
					SubChunk.WriteStore(stream, null, emptySubChunkBiomes, false, emptySubChunkUniqueBiomes);
				}

				SubChunk.WriteStore(stream, null, this[i].Biomes, false, subChunk.BiomeIds);
			}
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal int GetTopEmpty()
		{
			int topEmpty = WorldHeight / 16;
			for (int ci = (WorldHeight / 16) - 1; ci >= 0; ci--)
			{
				// Maybe reconsider if this is what we really want to do. Pooling buffers may remove the need for it. It's just an object.
				if (_subChunks[ci] == null || _subChunks[ci].IsAllAir())
				{
					topEmpty = ci;
					_subChunks[ci]?.Dispose();
					_subChunks[ci] = null;
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

			cc._subChunks = new SubChunk[_subChunks.Length];
			for (int i = 0; i < _subChunks.Length; i++)
			{
				cc._subChunks[i] = (SubChunk) _subChunks[i]?.Clone();
			}

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

		public IEnumerator<SubChunk> GetEnumerator()
		{
			return _subChunks.Where(c => c != null).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (height != null) ArrayPool<short>.Shared.Return(height);
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		~ChunkColumn()
		{
			Dispose(false);
		}
	}


	public delegate SubChunk SubChunkFactory(int x, int z, int index);
}