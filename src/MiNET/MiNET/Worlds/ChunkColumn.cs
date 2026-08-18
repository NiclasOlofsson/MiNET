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
using System.Collections.Concurrent;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.CompilerServices;
using fNbt;
using log4net;
using MiNET.Blocks;
using MiNET.Net;
using MiNET.Utils.Diagnostics;
using MiNET.Utils.IO;
using MiNET.Worlds.BlobCache;
using MiNET.Utils.Vectors;

namespace MiNET.Worlds
{
	public class ChunkColumn : ICloneable, IEnumerable<SubChunk>, IDisposable
	{
		public const int WorldHeight = 384;
		public const int WorldMaxY = WorldHeight + WorldMinY;
		public const int WorldMinY = -64;

		/// <summary>
		///     Whether a world Y addresses a block at all. <see cref="GetSubChunk" /> clamps, so a Y
		///     outside the range resolves to a real block somewhere else in the column rather than to
		///     nothing: callers ask this first and answer "no block here" themselves.
		/// </summary>
		public static bool IsInsideWorld(int y)
		{
			return y >= WorldMinY && y < WorldMaxY;
		}


		private static readonly ILog Log = LogManager.GetLogger(typeof(ChunkColumn));

		public int X { get; set; }
		public int Z { get; set; }

		private Dimension _dimension = Dimension.Overworld;

		/// <summary>
		///     Which dimension this column belongs to. It goes on the wire in every LevelChunk, and a
		///     client discards a column stamped with a dimension it is not in, so a level outside the
		///     overworld has to set this or none of its chunks are ever drawn. Changing it dirties the
		///     column: the cached batches already carry the old value in their bytes.
		/// </summary>
		public Dimension Dimension
		{
			get => _dimension;
			set
			{
				if (_dimension == value) return;

				_dimension = value;
				IsDirty = true;
			}
		}

		public bool IsAllAir { get; set; }

		public byte[] biomeId;
		public short[] height;

		//TODO: This dictionary need to be concurrent. Investigate performance before changing.
		public IDictionary<BlockCoordinates, NbtCompound> BlockEntities { get; private set; } = new Dictionary<BlockCoordinates, NbtCompound>();

		private SubChunk[] _subChunks = new SubChunk[WorldHeight / 16];

		// Cache related. Should actually all be private, but well
		public bool IsDirty { get; set; }
		public bool NeedSave { get; set; }

		public bool DisableCache { get; set; }
		private object _cacheSync = new object();

		/// <summary>
		///     Sub-chunk responses, by section and by whether the caller wants the blob form. Same
		///     column, same section, same answer for every player, so this is built once and handed
		///     out with only the request's own offset stamped on.
		///     <para>
		///         Cleared in <see cref="SetDirty" />, which is the single funnel every block write
		///         goes through, rather than keyed on <see cref="IsDirty" />: the batch getters clear
		///         that flag when they rebuild, so the first of them to run would otherwise make a
		///         stale sub-chunk look current.
		///     </para>
		/// </summary>
		// Not readonly: Clone() must hand the copy its own dictionary. Shared, the first column
		// to serialize a section answers for every clone of the same template.
		private ConcurrentDictionary<int, SubChunkPacketData> _cachedSubChunkData = new ConcurrentDictionary<int, SubChunkPacketData>();

		public ChunkColumn(bool clearBuffers = true)
		{
			biomeId = ArrayPool<byte>.Shared.Rent(256);
			height = ArrayPool<short>.Shared.Rent(256);

			if (clearBuffers) ClearBuffers();

			IsDirty = false;
		}

		private void ClearBuffers()
		{
			Array.Clear(biomeId, 0, 256);
			Fill<byte>(biomeId, 1);

			// Rented from the shared pool, so it arrives holding the previous tenant's heights. A
			// column RecalcHeight never resolves keeps them, and they go to disk and to the client.
			Array.Clear(height, 0, 256);
		}

		/// <summary>
		///     Bumped by every block write, so a column that has changed is a different version of
		///     itself. This is what a player's sent-set is keyed on: a skeleton already pushed is never
		///     pushed again, and the only thing that makes it worth pushing a second time is the
		///     content actually being different. It plays the part the blob cache key plays, and works
		///     the same whether or not the client caches.
		/// </summary>
		public long Version { get; private set; }

		private void SetDirty()
		{
			IsDirty = true;
			NeedSave = true;
			Version++;
			_cachedSubChunkData.Clear();
		}


		public SubChunk this[int chunkIndex, bool generateIfMissing = true]
		{
			get
			{
				SubChunk subChunk = _subChunks[chunkIndex];
				if (generateIfMissing && subChunk == null)
				{
					subChunk = SubChunk.CreateObject();
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

		public SubChunk GetSubChunk(int by, bool generateIfMissing = true)
		{
			by >>= 4;
			by += WorldMinY < 0 ? Math.Abs(WorldMinY >> 4) : 0;

			return this[Math.Clamp(by, 0, _subChunks.Length - 1), generateIfMissing];
		}

		public int GetBlockId(int bx, int by, int bz)
		{
			var subChunk = GetSubChunk(by);
			return subChunk.GetBlockId(bx, by & 0xf, bz);
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
			height[((bz << 4) + (bx))] = h;
			SetDirty();
		}

		public short GetHeight(int bx, int bz)
		{
			return height[((bz << 4) + (bx))];
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
			// A column with no sections has nothing to measure: every height is zero, which is what
			// the buffers already hold. Walking it anyway is 256 positions times 24 sections of
			// stepping over emptiness, and a view distance worth of empty columns is most of what a
			// map world consists of.
			if (Count() == 0) return;

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

			// Down to the world floor, not to zero. Zero was the floor before 1.18 and a column whose
			// only solid blocks are below it would otherwise never resolve a height at all.
			for (int y = startY; y >= WorldMinY; y--)
			{
				// A section that was never stored is air, and asking for it must not bring it into
				// being: an empty column has 24 of those, and creating each one to discover it is
				// empty is the difference between a column costing microseconds and milliseconds.
				SubChunk missing = GetSubChunk(y, false);
				if (missing == null)
				{
					y = (y >> 4) << 4;
					continue;
				}

				if (isInLight)
				{
					SubChunk chunk = missing;
					if (isInAir && chunk.IsAllAir())
					{
						if (chunk.IsDirty) Array.Fill<byte>(chunk._skylight.Data, 0xff);

						// Drop to this subchunk's floor and let the loop step below it. y is not
						// aligned to a subchunk boundary, so it has to be floored rather than
						// decremented by a fixed 16.
						y = (y >> 4) << 4;
						continue;
					}

					isInAir = false;

					// By runtime id: a block's identity here is its palette entry, and GetBlockId
					// answers air for anything without a legacy id. Air, glass, leaves, water and
					// cobweb differ only in how much light they dampen, which the block data holds.
					if (BlockFactory.SkyLightPasses(GetBlockRuntimeId(x, y, z)))
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
					// Same as RecalcHeight: a missing section is air nobody stored, and creating it
					// to read it is the expensive way to learn nothing.
					SubChunk chunk = GetSubChunk(y, false);
					if (chunk == null)
					{
						y = (y >> 4) << 4;
						continue;
					}

					if (isInAir && chunk.IsAllAir())
					{
						if (chunk.IsDirty) Array.Fill<byte>(chunk._skylight.Data, 0xff);

						// Drop to this subchunk's floor and let the loop step below it. y is not
						// aligned to a subchunk boundary, so it has to be floored rather than
						// decremented by a fixed 16.
						y = (y >> 4) << 4;
						continue;
					}

					isInAir = false;

					if (BlockFactory.SkyLightPasses(GetBlockRuntimeId(x, y, z)))
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
			_cachedSubChunkData.Clear();
		}

		/// <summary>Is this an addressable sub-chunk index in the dimension?</summary>
		public static bool IsSectionInBounds(int sectionY)
		{
			int storageIndex = sectionY - (WorldMinY / 16);
			return storageIndex >= 0 && storageIndex < WorldHeight / 16;
		}

		/// <summary>
		///     The block half of the sub-chunk request flow, answering one requested section: a
		///     version-9 store carrying the absolute section index, plus the per-column heightmap
		///     expressed relative to that section. Sections wholly above or below the surface say
		///     so through the heightmap type and carry no heights. An all-air section carries no
		///     store at all. Blob mode moves the store into the content-addressed store the way
		///     vanilla serves a cache-enabled client; the heightmap stays inline either way.
		/// </summary>
		public SubChunkPacketData GetSubChunkData(SubChunkPosOffset offset, int sectionY)
		{
			// One key per section per form. Section indices are small and signed, so they are shifted
			// rather than multiplied, which keeps negative sections distinct from positive ones.
			int key = sectionY;

			if (!DisableCache && _cachedSubChunkData.TryGetValue(key, out SubChunkPacketData hit)) return WithOffset(hit, offset);

			SubChunkPacketData built = BuildSubChunkData(offset, sectionY);

			// Only a real section is worth keeping. An out-of-bounds or all-air answer is cheap to
			// produce and caching it would hold a dictionary entry per section of empty sky.
			if (!DisableCache && built.subchunkRequestResult == SubChunkPacketData.SubchunkRequestResult.Success) _cachedSubChunkData[key] = built;

			return built;
		}

		/// <summary>
		///     The same response aimed at a different request. Everything but the offset is identical
		///     for every player, and the arrays are only ever read by the encoder, so they are shared
		///     rather than copied.
		/// </summary>
		private static SubChunkPacketData WithOffset(SubChunkPacketData cached, SubChunkPosOffset offset)
		{
			return new SubChunkPacketData
			{
				subchunkPosOffset = offset,
				subchunkRequestResult = cached.subchunkRequestResult,
				serializedSubChunk = cached.serializedSubChunk,
				heightMapData = cached.heightMapData,
				blobId = cached.blobId
			};
		}

		private SubChunkPacketData BuildSubChunkData(SubChunkPosOffset offset, int sectionY)
		{
			var entry = new SubChunkPacketData
			{
				subchunkPosOffset = offset,
				heightMapData = new SubChunkHeightmapData
				{
					heightMapType = SubChunkHeightmapData.HeightMapType.Nodata,
					renderHeightMapType = SubChunkHeightmapData.RenderHeightMapType.Nodata
				}
			};

			if (!IsSectionInBounds(sectionY))
			{
				entry.subchunkRequestResult = SubChunkPacketData.SubchunkRequestResult.Indexoutofbounds;
				return entry;
			}

			int sectionBaseY = sectionY * 16;
			var heights = new byte[256];
			bool allBelow = true, allAbove = true;
			for (int i = 0; i < 256; i++)
			{
				int rel = height[i] - sectionBaseY;
				if (rel >= 0) allBelow = false;
				if (rel < 16) allAbove = false;

				// Signed, per Mojang's SubChunk Request System doc: each entry is an int8 where -1
				// means the column's surface is BELOW this section, 16 means it is at or above the
				// ceiling, and 0..15 is a position inside. Clamping the low end to 0 instead of -1
				// told the client the ground was at this section's floor for every column that
				// actually lies beneath it, which is exactly the sections straddling a terrain edge.
				// The field is byte[] only as storage: it is written as raw bytes, so -1 goes out as
				// 0xFF and is read back as -1.
				heights[i] = unchecked((byte) (sbyte) Math.Clamp(rel, -1, 16));
			}

			if (allBelow)
			{
				entry.heightMapData.heightMapType = SubChunkHeightmapData.HeightMapType.Alltoolow;
				entry.heightMapData.renderHeightMapType = SubChunkHeightmapData.RenderHeightMapType.Alltoolow;
			}
			else if (allAbove)
			{
				entry.heightMapData.heightMapType = SubChunkHeightmapData.HeightMapType.Alltoohigh;
				entry.heightMapData.renderHeightMapType = SubChunkHeightmapData.RenderHeightMapType.Alltoohigh;
			}
			else
			{
				entry.heightMapData.heightMapType = SubChunkHeightmapData.HeightMapType.Hasdata;
				entry.heightMapData.heights = heights;
				entry.heightMapData.renderHeightMapType = SubChunkHeightmapData.RenderHeightMapType.Allcopied;
			}

			SubChunk subChunk = this[sectionY - (WorldMinY / 16), generateIfMissing: false];
			if (subChunk == null || subChunk.IsAllAir())
			{
				entry.subchunkRequestResult = SubChunkPacketData.SubchunkRequestResult.Successallair;
				return entry;
			}

			// Pooled: this runs once per requested section per player, which is the busiest allocation
			// site in the join burst.
			using (MemoryStream stream = MiNetServer.MemoryStreamManager.GetStream())
			{
				subChunk.WriteVersion9(stream, (sbyte) sectionY);

				// The section's block entities ride at the end of its payload, which is where the
				// client reads them from (MiNET.Client's own decoder, written against BDS, calls them
				// "trailing block entities, varint nbt until end"). Leaving them out does not lose a
				// chest, it makes it invisible: a chest, ender chest, shulker box, sign or bed is
				// drawn by its block entity, so the block arrives, keeps its outline and its
				// particles, and has nothing to render.
				//
				// Cached, the two travel separately, confirmed against a real 1.26.40 client: the blob
				// is content-addressed and holds the terrain, which is what the hash is for, and the
				// block entities go inline in the same packet, in the field beside the blob id. Putting
				// them in the blob instead leaves the chests invisible, which is how that was settled.
				entry.blobId = BlobStore.Add(stream.ToArray());

				using MemoryStream entityStream = MiNetServer.MemoryStreamManager.GetStream();
				WriteBlockEntities(entityStream, sectionY);
				if (entityStream.Length > 0) entry.serializedSubChunk = entityStream.ToArray();
			}

			entry.subchunkRequestResult = SubChunkPacketData.SubchunkRequestResult.Success;
			return entry;
		}

		/// <summary>
		///     The computed parts of the skeleton, made once per column version and reused for
		///     every player the skeleton goes to: the biome payload is serialized and hashed when
		///     the column changes, not once per send. The reference swap is atomic and racing
		///     builders compute identical seeds, so a lost race wastes work without ever being
		///     wrong. The list and position are read-only riders on the packets built from them:
		///     encode reads them, Reset only drops the reference.
		/// </summary>
		private sealed record SkeletonSeed(long Version, int Limit, List<ulong> CacheMetadata, ChunkPos Position);

		private SkeletonSeed _skeletonSeed;

		/// <summary>
		///     The computed parts of the cached push form, seed-cached per column version exactly
		///     like <see cref="SkeletonSeed" />: the hash list, the section count it counts, and
		///     the inline tail. The tail carries the block entities, and SetBlockEntity funnels
		///     through SetDirty, so a container change re-seeds this like any block write.
		/// </summary>
		private sealed record PushSeed(long Version, uint SubChunkCount, List<ulong> CacheMetadata, byte[] Tail, ChunkPos Position);

		private PushSeed _pushSeed;

		private static readonly byte[] SkeletonChunkData = {0}; // Border blocks - nope (EDU)

		/// <summary>
		///     The skeleton for a cache-enabled client, matching vanilla 1.26.40: the biome payload
		///     moves into a content-addressed blob (one cacheMetadata hash), leaving only the
		///     border-blocks byte inline. The client reports hit or miss via ClientCacheBlobStatus
		///     and misses come back through ClientCacheMissResponse.
		/// </summary>
		public McpeLevelChunk CreateSkeletonChunk()
		{
			SkeletonSeed seed = _skeletonSeed;
			if (seed == null || seed.Version != Version)
			{
				// Content addressed, so identical biome payloads across columns collapse to one
				// hash and a returning client fetches none of them.
				ulong biomeBlob = BlobStore.Add(BuildSkeletonBiomes());

				// How far up the client should ask. Subtracting one to make it the index of the
				// highest non-air section, which is how Mojang's SubChunk Request System doc words
				// it, loses the top of the world in a real column, so the value the client wants
				// is the one GetTopEmpty already returns.
				seed = new SkeletonSeed(Version, GetTopEmpty(), new List<ulong> {biomeBlob}, new ChunkPos {x = X, z = Z});
				_skeletonSeed = seed;
			}

			var packet = McpeLevelChunk.CreateObject();
			packet.chunkPosition = seed.Position;
			packet.dimension = (int) Dimension;
			packet.subChunkCount = 0;
			packet.clientRequestSubchunkLimit = seed.Limit;
			packet.cacheEnabled = true;
			packet.cacheMetadata = seed.CacheMetadata;
			packet.chunkData = SkeletonChunkData;

			return packet;
		}

		/// <summary>
		///     There is no delivery-mode setting. Push and pull are not two configurations of the
		///     same thing, they are two answers to different situations, and the caller is the only
		///     one who knows which situation it is in: a join or a teleport wants every column
		///     complete in one packet, the steady-state rim of a walking player wants the same, and
		///     a caller that genuinely wants the client to choose what it needs asks for a skeleton.
		///     So each call site names the form it wants, and there is nothing to misconfigure.
		/// </summary>
		/// <summary>
		///     The same chunk with its bulk moved into content-addressed blobs: one per section,
		///     one for the biomes, leaving only border blocks and block entities inline. A client
		///     that already holds a blob never receives those bytes again, and blobs shared between
		///     chunks (all-air sections, repeated terrain) are stored and sent once for everyone.
		///     Server-driven, no request-mode marker: the client asks for nothing and reports hit
		///     or miss per hash instead.
		///     <para>
		///         Section blobs are the same version-9 bytes the sub-chunk request path serves,
		///         so both modes hash to the same blobs and a client cache filled by one mode hits
		///         in the other. Hash order is the client's rebuild order: sections bottom-up,
		///         biome blob last, with subChunkCount counting only the sections.
		///     </para>
		/// </summary>
		public McpeLevelChunk CreateCachedPushChunk()
		{
			PushSeed seed = _pushSeed;
			if (seed == null || seed.Version != Version)
			{
				int topEmpty = GetTopEmpty();

				var hashes = new List<ulong>(topEmpty + 1);
				for (int ci = 0; ci < topEmpty; ci++)
				{
					hashes.Add(GetSectionBlobId(ci));
				}
				hashes.Add(BlobStore.Add(BuildSkeletonBiomes()));

				seed = new PushSeed(Version, (uint) topEmpty, hashes, GetTailBytes(), new ChunkPos {x = X, z = Z});
				_pushSeed = seed;
			}

			var packet = McpeLevelChunk.CreateObject();
			packet.chunkPosition = seed.Position;
			packet.dimension = (int) Dimension;
			packet.subChunkCount = seed.SubChunkCount;
			packet.cacheEnabled = true;
			packet.cacheMetadata = seed.CacheMetadata;
			packet.chunkData = seed.Tail;

			return packet;
		}

		/// <summary>
		///     One section's blob id for the cached push form. A live section serializes exactly
		///     as the sub-chunk request path does; a missing or all-air one becomes the canonical
		///     empty version-9 section (no storages), so every empty section at the same height
		///     collapses to one blob for the whole world.
		/// </summary>
		private ulong GetSectionBlobId(int storageIndex)
		{
			sbyte sectionY = (sbyte) (storageIndex + WorldMinY / 16);

			SubChunk section = this[storageIndex, generateIfMissing: false];
			if (section == null || section.IsAllAir()) return BlobStore.Add(new byte[] {9, 0, unchecked((byte) sectionY)});

			using MemoryStream stream = MiNetServer.MemoryStreamManager.GetStream();
			section.WriteVersion9(stream, sectionY);
			return BlobStore.Add(stream.ToArray());
		}

		/// <summary>
		///     Everything in the chunk payload that is not a blob: the border block count and the
		///     block entities. Sections and biomes are addressed by hash in the cached form, so
		///     this is all that still travels inline.
		/// </summary>
		private byte[] GetTailBytes()
		{
			using var stream = new MemoryStream();

			stream.WriteByte(0); // Border blocks - nope (EDU)

			WriteBlockEntities(stream);

			return stream.ToArray();
		}


		// REFCT: a plain MemoryStream plus ToArray, so a full column's payload is a fresh array every
		// call and goes to the LOH (gen2, uncompacted) the moment it clears 85,000 bytes. Everything
		// else on the send path builds into MiNetServer.MemoryStreamManager's pooled streams; this
		// path predates that and never moved. Applies to GetTailBytes and the biome writer beside it.
		private void WriteBlockEntities(Stream stream)
		{
			if (BlockEntities.Count == 0) return;

			foreach (NbtCompound blockEntity in BlockEntities.Values.ToArray())
			{
				WriteBlockEntity(stream, blockEntity);
			}
		}

		/// <summary>The block entities standing in one section, for the sub-chunk form where a section
		/// travels on its own and carries its own.</summary>
		private void WriteBlockEntities(Stream stream, int sectionY)
		{
			if (BlockEntities.Count == 0) return;

			int floor = sectionY * 16;
			int ceiling = floor + 16;

			foreach (KeyValuePair<BlockCoordinates, NbtCompound> entry in BlockEntities.ToArray())
			{
				if (entry.Key.Y < floor || entry.Key.Y >= ceiling) continue;

				WriteBlockEntity(stream, entry.Value);
			}
		}

		private static void WriteBlockEntity(Stream stream, NbtCompound blockEntity)
		{
			var file = new NbtFile(blockEntity)
			{
				BigEndian = false,
				UseVarInt = true
			};
			file.SaveToStream(stream, NbtCompression.None);
		}

		/// <summary>
		///     The skeleton form of the biome payload, byte-matching what BDS 1.26.40 sends: one
		///     storage for the bottom section (bits-0 single-value when the chunk has one biome),
		///     then a 0xFF copy-previous marker per remaining section. Falls back to full storages
		///     for multi-biome chunks.
		/// </summary>
		private byte[] BuildSkeletonBiomes()
		{
			byte first = biomeId[0];
			bool uniform = true;
			for (int i = 1; i < 256; i++)
			{
				if (biomeId[i] != first && biomeId[i] != 255)
				{
					uniform = false;
					break;
				}
			}

			if (!uniform) return GetBiomePalette(biomeId);

			// Header, one zigzag varint, then one copy-previous marker per remaining section. The
			// scratch is a pooled lease sized to the worst case, the ordinary stream writers fill
			// it, and the written slice is copied out once, exact, for the blob store to own
			// permanently - which is why the result is deliberately NOT pooled memory.
			int sectionCount = WorldHeight / 16;
			byte[] scratch = ArrayPool<byte>.Shared.Rent(1 + 5 + (sectionCount - 1));
			try
			{
				using var stream = new MemoryStream(scratch);
				stream.WriteByte(0x01); // bits-0 storage: header only, one palette value
				MiNET.Utils.VarInt.WriteSInt32(stream, first == 255 ? 0 : first);
				for (int i = 1; i < sectionCount; i++)
				{
					stream.WriteByte(0xFF); // copy the previous section's storage
				}

				return scratch.AsSpan(0, (int) stream.Position).ToArray();
			}
			finally
			{
				ArrayPool<byte>.Shared.Return(scratch);
			}
		}

		private byte[] GetBiomePalette(byte[] biomes)
		{
			for (int b = 0; b < biomes.Length; b++)
			{
				if (biomes[b] == 255)
					biomes[b] = 0;
			}
			using var stream = new MemoryStream();
			
			var uniqueBiomes = biomes.Distinct().Select(x => (int)x).ToList();

			short[] newBiomes = new short[16 * 16 * 16];
			for (int x = 0; x < 16; x++)
			{
				for (int z = 0; z < 16; z++)
				{
					var currentBiome = (int)biomes[(z << 4) + (x)];

					for (int y = 0; y < 16; y++)
					{
						//var index = ((y >> 2) << 4) | ((z >> 2) << 2) | (x >> 2);
						newBiomes[(x << 8 | z << 4 | y)] = (short) uniqueBiomes.IndexOf(currentBiome);
					}
				}
			}
			
			for (int i = 0; i < 24; i++)
			{
				SubChunk.WriteStore(stream, newBiomes, null, false, uniqueBiomes, isBlockPalette: false);
			}

			return stream.ToArray();
		}


		/// <summary>
		///     How many sections from the bottom are worth sending or asking for: the index of the
		///     lowest section above which everything is empty.
		///     <para>
		///         A query, and only a query. It used to free the empty sub-chunks it walked past and
		///         null their slots, which is memory this column created and owns, freed from whatever
		///         thread happened to be answering one player. Two players streaming the same column
		///         then returned the same four pooled arrays twice, and the pool handed one of them to
		///         a second owner: the corruption surfaced far away, as unreadable batches in the
		///         transport, which is simply the heaviest user of that pool. Sub-chunks are released
		///         when the column is, in <see cref="Dispose" />, and nowhere else.
		///     </para>
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal int GetTopEmpty()
		{
			int topEmpty = WorldHeight / 16;
			for (int ci = (WorldHeight / 16) - 1; ci >= 0; ci--)
			{
				if (_subChunks[ci] == null || _subChunks[ci].IsAllAir())
				{
					topEmpty = ci;
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

			cc.biomeId = (byte[]) biomeId.Clone();
			cc.height = (short[]) height.Clone();

			cc.BlockEntities = new Dictionary<BlockCoordinates, NbtCompound>();
			foreach (KeyValuePair<BlockCoordinates, NbtCompound> blockEntityPair in BlockEntities)
			{
				cc.BlockEntities.Add(blockEntityPair.Key, (NbtCompound) blockEntityPair.Value.Clone());
			}

			cc._cacheSync = new object();
			cc._cachedSubChunkData = new ConcurrentDictionary<int, SubChunkPacketData>();

			// Never shared with the original: a clone is typically relocated (new X/Z) and then
			// mutated, and the seed's position and biome hash describe the column it was built
			// from. The clone builds its own on first send.
			cc._skeletonSeed = null;

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
				if (biomeId != null) ArrayPool<byte>.Shared.Return(biomeId);
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