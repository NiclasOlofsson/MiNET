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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using fNbt;
using log4net;
using MiNET.BlockEntities;
using MiNET.Blocks;
using MiNET.Items;
using MiNET.Net;
using MiNET.Utils;
using MiNET.Utils.Vectors;

namespace MiNET.Worlds
{
	public class Mapper : Tuple<int, Func<int, byte, byte>>
	{
		public Mapper(int blockId, Func<int, byte, byte> dataMapper)
			: base(blockId, dataMapper)
		{
		}
	}

	public class NoDataMapper : Mapper
	{
		public NoDataMapper(int blockId) : base(blockId, (bi, i1) => i1)
		{
		}
	}

	public class AnvilWorldProvider : IWorldProvider, ICachingWorldProvider, ICloneable
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(AnvilWorldProvider));

		public static readonly Dictionary<int, Tuple<int, Func<int, byte, byte>>> Convert;

		public IWorldGenerator MissingChunkProvider { get; set; }

		public LevelInfo LevelInfo { get; private set; }

		public ConcurrentDictionary<ChunkCoordinates, ChunkColumn> _chunkCache = new ConcurrentDictionary<ChunkCoordinates, ChunkColumn>();

		public string BasePath { get; private set; }

		public Dimension Dimension { get; set; }

		public bool IsCaching { get; private set; } = true;

		public bool IsDimensionWithSkyLight { get; set; } = true;

		public bool ReadSkyLight { get; set; } = true;

		public bool ReadBlockLight { get; set; } = true;

		public bool Locked { get; set; } = false;

		static AnvilWorldProvider()
		{
			var air = new Mapper(0, (i, b) => 0);
			Convert = new Dictionary<int, Tuple<int, Func<int, byte, byte>>>
			{
				{36, new NoDataMapper(250)}, // minecraft:piston_extension		=> MovingBlock
				{43, new Mapper(43, (i, b) => (byte) (b == 6 ? 7 : b == 7 ? 6 : b))}, // Fence		=> Fence
				{44, new Mapper(44, (i, b) => (byte) (b == 6 ? 7 : b == 7 ? 6 : b == 14 ? 15 : b == 15 ? 14 : b))}, // Fence		=> Fence
				{
					77, new Mapper(77, delegate(int i, byte b) // stone_button
					{
						switch (b & 0x7f)
						{
							case 0:
								return (byte) BlockFace.Down;
							case 1:
								return (byte) BlockFace.East;
							case 2:
								return (byte) BlockFace.West;
							case 3:
								return (byte) BlockFace.South;
							case 4:
								return (byte) BlockFace.North;
							case 5:
								return (byte) BlockFace.Up;
						}

						return 0;
					})
				},
				{84, new NoDataMapper(25)}, // minecraft:jukebox		=> noteblock
				{85, new Mapper(85, (i, b) => 0)}, // Fence		=> Fence
				{95, new NoDataMapper(241)}, // minecraft:stained_glass	=> Stained Glass
				{96, new Mapper(96, (i, b) => (byte) (((b & 0x04) << 1) | ((b & 0x08) >> 1) | (3 - (b & 0x03))))}, // Trapdoor Fix
				{125, new NoDataMapper(157)}, // minecraft:double_wooden_slab	=> minecraft:double_wooden_slab
				{126, new NoDataMapper(158)}, // minecraft:wooden_slab		=> minecraft:wooden_slab
				{
					143, new Mapper(143, delegate(int i, byte b) // wooden_button
					{
						switch (b & 0x7f)
						{
							case 0:
								return (byte) BlockFace.Down; // 0
							case 1:
								return (byte) BlockFace.East; // 5
							case 2:
								return (byte) BlockFace.West; // 4
							case 3:
								return (byte) BlockFace.South; // 3
							case 4:
								return (byte) BlockFace.North; // 2
							case 5:
								return (byte) BlockFace.Up; // 1
						}

						return 0;
					})
				},
				{157, new NoDataMapper(126)}, // minecraft:activator_rail
				{158, new NoDataMapper(125)}, // minecraft:dropper
				{166, new NoDataMapper(95)}, // minecraft:barrier		=> (Invisible Bedrock)
				{167, new Mapper(167, (i, b) => (byte) (((b & 0x04) << 1) | ((b & 0x08) >> 1) | (3 - (b & 0x03))))}, //Fix iron_trapdoor
				{188, new Mapper(85, (i, b) => 1)}, // Spruce Fence		=> Fence
				{189, new Mapper(85, (i, b) => 2)}, // Birch Fence		=> Fence
				{190, new Mapper(85, (i, b) => 3)}, // Jungle Fence		=> Fence
				{191, new Mapper(85, (i, b) => 5)}, // Dark Oak Fence	=> Fence
				{192, new Mapper(85, (i, b) => 4)}, // Acacia Fence		=> Fence
				{198, new NoDataMapper(208)}, // minecraft:end_rod	=> EndRod
				{199, new NoDataMapper(240)}, // minecraft:chorus_plant
				{202, new Mapper(201, (i, b) => 2)}, // minecraft:purpur_pillar => PurpurBlock:2 (idk why)
				{204, new Mapper(181, (i, b) => 1)}, // minecraft:purpur_double_slab
				{205, new Mapper(182, (i, b) => 1)}, // minecraft:purpur_slab
				{207, new NoDataMapper(244)}, // minecraft:beetroot_block
				{208, new NoDataMapper(198)}, // minecraft:grass_path
				{210, new NoDataMapper(188)}, // repeating_command_block
				{211, new NoDataMapper(189)}, // minecraft:chain_command_block
				{212, new NoDataMapper(297)}, // Frosted Ice
				{218, new NoDataMapper(251)}, // minecraft:observer => Observer
				{219, new Mapper(218, (i, b) => (byte) (0 + (b << 4)))}, // => minecraft:white_shulker_box
				{220, new Mapper(218, (i, b) => (byte) (1 + (b << 4)))}, // => minecraft:orange_shulker_box
				{221, new Mapper(218, (i, b) => (byte) (2 + (b << 4)))}, // => minecraft:magenta_shulker_box
				{222, new Mapper(218, (i, b) => (byte) (3 + (b << 4)))}, // => minecraft:light_blue_shulker_box 
				{223, new Mapper(218, (i, b) => (byte) (4 + (b << 4)))}, // => minecraft:yellow_shulker_box 
				{224, new Mapper(218, (i, b) => (byte) (5 + (b << 4)))}, // => minecraft:lime_shulker_box 
				{225, new Mapper(218, (i, b) => (byte) (6 + (b << 4)))}, // => minecraft:pink_shulker_box 
				{226, new Mapper(218, (i, b) => (byte) (7 + (b << 4)))}, // => minecraft:gray_shulker_box 
				{227, new Mapper(218, (i, b) => (byte) (8 + (b << 4)))}, // => minecraft:light_gray_shulker_box 
				{228, new Mapper(218, (i, b) => (byte) (9 + (b << 4)))}, // => minecraft:cyan_shulker_box 
				{229, new Mapper(218, (i, b) => (byte) (10 + (b << 4)))}, // => minecraft:purple_shulker_box 
				{230, new Mapper(218, (i, b) => (byte) (11 + (b << 4)))}, // => minecraft:blue_shulker_box 
				{231, new Mapper(218, (i, b) => (byte) (12 + (b << 4)))}, // => minecraft:brown_shulker_box 
				{232, new Mapper(218, (i, b) => (byte) (13 + (b << 4)))}, // => minecraft:green_shulker_box 
				{233, new Mapper(218, (i, b) => (byte) (14 + (b << 4)))}, // => minecraft:red_shulker_box 
				{234, new Mapper(218, (i, b) => (byte) (15 + (b << 4)))}, // => minecraft:black_shulker_box 

				{235, new NoDataMapper(220)}, // => minecraft:white_glazed_terracotta
				{236, new NoDataMapper(221)}, // => minecraft:orange_glazed_terracotta
				{237, new NoDataMapper(222)}, // => minecraft:magenta_glazed_terracotta
				{238, new NoDataMapper(223)}, // => minecraft:light_blue_glazed_terracotta
				{239, new NoDataMapper(224)}, // => minecraft:yellow_glazed_terracotta
				{240, new NoDataMapper(225)}, // => minecraft:lime_glazed_terracotta
				{241, new NoDataMapper(226)}, // => minecraft:pink_glazed_terracotta
				{242, new NoDataMapper(227)}, // => minecraft:gray_glazed_terracotta
				{243, new NoDataMapper(228)}, // => minecraft:light_gray_glazed_terracotta
				{244, new NoDataMapper(229)}, // => minecraft:cyan_glazed_terracotta
				{245, new NoDataMapper(219)}, // => minecraft:purple_glazed_terracotta
				{246, new NoDataMapper(231)}, // => minecraft:blue_glazed_terracotta
				{247, new NoDataMapper(232)}, // => minecraft:brown_glazed_terracotta
				{248, new NoDataMapper(233)}, // => minecraft:green_glazed_terracotta
				{249, new NoDataMapper(234)}, // => minecraft:red_glazed_terracotta
				{250, new NoDataMapper(235)}, // => minecraft:black_glazed_terracotta

				{251, new NoDataMapper(236)}, // => minecraft:concrete
				{252, new NoDataMapper(237)}, // => minecraft:concrete_powder
			};
		}

		public AnvilWorldProvider()
		{
		}

		public AnvilWorldProvider(string basePath) : this()
		{
			BasePath = basePath;
		}

		protected AnvilWorldProvider(string basePath, LevelInfo levelInfo, ConcurrentDictionary<ChunkCoordinates, ChunkColumn> chunkCache)
		{
			BasePath = basePath;
			LevelInfo = levelInfo;
			_chunkCache = chunkCache;
			_isInitialized = true;
		}

		private bool _isInitialized = false;
		private object _initializeSync = new object();

		public void Initialize()
		{
			if (_isInitialized) return; // Quick exit

			lock (_initializeSync)
			{
				if (_isInitialized) return;

				BasePath ??= Config.GetProperty("PCWorldFolder", "World").Trim();

				NbtFile file = new NbtFile();
				var levelFileName = Path.Combine(BasePath, "level.dat");
				if (File.Exists(levelFileName))
				{
					file.LoadFromFile(levelFileName);
					NbtTag dataTag = file.RootTag["Data"];
					LevelInfo = new LevelInfo(dataTag);
				}
				else
				{
					Log.Warn($"No level.dat found at {levelFileName}. Creating empty.");
					LevelInfo = new LevelInfo();
				}

				switch (Dimension)
				{
					case Dimension.Overworld:
						break;
					case Dimension.Nether:
						BasePath = Path.Combine(BasePath, @"DIM-1");
						break;
					case Dimension.TheEnd:
						BasePath = Path.Combine(BasePath, @"DIM1");
						break;
				}

				MissingChunkProvider?.Initialize(this);

				_isInitialized = true;
			}
		}

		private int Noop(int blockId, int data)
		{
			return 0;
		}

		public bool CachedChunksContains(ChunkCoordinates chunkCoord)
		{
			return _chunkCache.ContainsKey(chunkCoord);
		}

		public int UnloadChunks(Player[] players, ChunkCoordinates spawn, double maxViewDistance)
		{
			int removed = 0;

			lock (_chunkCache)
			{
				List<ChunkCoordinates> coords = new List<ChunkCoordinates> {spawn};

				foreach (var player in players)
				{
					var chunkCoordinates = new ChunkCoordinates(player.KnownPosition);
					if (!coords.Contains(chunkCoordinates)) coords.Add(chunkCoordinates);
				}

				bool save = Config.GetProperty("Save.Enabled", false);

				Parallel.ForEach(_chunkCache, (chunkColumn) =>
				{
					bool keep = coords.Exists(c => c.DistanceTo(chunkColumn.Key) < maxViewDistance);
					if (!keep)
					{
						_chunkCache.TryRemove(chunkColumn.Key, out ChunkColumn waste);
						if (save && waste.NeedSave)
						{
							SaveChunk(waste, BasePath);
						}

						if (waste != null)
						{
							foreach (var chunk in waste)
							{
								chunk.PutPool();
							}
						}

						Interlocked.Increment(ref removed);
					}
				});
			}

			return removed;
		}

		public ChunkColumn[] GetCachedChunks()
		{
			return _chunkCache.Values.Where(column => column != null).ToArray();
		}

		public void ClearCachedChunks()
		{
			_chunkCache.Clear();
		}

		public ChunkColumn GenerateChunkColumn(ChunkCoordinates chunkCoordinates, bool cacheOnly = false)
		{
			if (Locked || cacheOnly)
			{
				_chunkCache.TryGetValue(chunkCoordinates, out ChunkColumn chunk);
				return chunk;
			}

			if (_chunkCache.TryGetValue(chunkCoordinates, out ChunkColumn value))
			{
				if (value == null) _chunkCache.TryRemove(chunkCoordinates, out value);
				if (value != null) return value;
			}

			// Warning: The following code MAY execute the GetChunk 2 times for the same coordinate
			// if called in rapid succession. However, for the scenario of the provider, this is highly unlikely.
			return _chunkCache.GetOrAdd(chunkCoordinates, coordinates => GetChunk(coordinates, BasePath, MissingChunkProvider));
		}

		public Queue<Block> LightSources { get; set; } = new Queue<Block>();

		public ChunkColumn GetChunk(ChunkCoordinates coordinates, string basePath, IWorldGenerator generator)
		{
			try
			{
				int width = 32;
				int depth = 32;

				int rx = coordinates.X >> 5;
				int rz = coordinates.Z >> 5;

				string filePath = Path.Combine(basePath, string.Format(@"region{2}r.{0}.{1}.mca", rx, rz, Path.DirectorySeparatorChar));

				if (!File.Exists(filePath))
				{
					var chunkColumn = generator?.GenerateChunkColumn(coordinates);
					if (chunkColumn != null)
					{
						if (Dimension == Dimension.Overworld && Config.GetProperty("CalculateLights", false))
						{
							SkyLightBlockAccess blockAccess = new SkyLightBlockAccess(this, chunkColumn);
							new SkyLightCalculations().RecalcSkyLight(chunkColumn, blockAccess);
						}

						chunkColumn.IsDirty = false;
						chunkColumn.NeedSave = false;
					}

					return chunkColumn;
				}

				using (var regionFile = File.OpenRead(filePath))
				{
					byte[] buffer = new byte[8192];

					regionFile.Read(buffer, 0, 8192);

					int xi = (coordinates.X % width);
					if (xi < 0) xi += 32;
					int zi = (coordinates.Z % depth);
					if (zi < 0) zi += 32;
					int tableOffset = (xi + zi * width) * 4;

					regionFile.Seek(tableOffset, SeekOrigin.Begin);

					byte[] offsetBuffer = new byte[4];
					regionFile.Read(offsetBuffer, 0, 3);
					Array.Reverse(offsetBuffer);
					int offset = BitConverter.ToInt32(offsetBuffer, 0) << 4;

					byte[] bytes = BitConverter.GetBytes(offset >> 4);
					Array.Reverse(bytes);
					if (offset != 0 && offsetBuffer[0] != bytes[0] && offsetBuffer[1] != bytes[1] && offsetBuffer[2] != bytes[2])
					{
						throw new Exception($"Not the same buffer\n{Packet.HexDump(offsetBuffer)}\n{Packet.HexDump(bytes)}");
					}

					int length = regionFile.ReadByte();

					if (offset == 0 || length == 0)
					{
						var chunkColumn = generator?.GenerateChunkColumn(coordinates);
						if (chunkColumn != null)
						{
							if (Dimension == Dimension.Overworld && Config.GetProperty("CalculateLights", false))
							{
								SkyLightBlockAccess blockAccess = new SkyLightBlockAccess(this, chunkColumn);
								new SkyLightCalculations().RecalcSkyLight(chunkColumn, blockAccess);
							}

							chunkColumn.IsDirty = false;
							chunkColumn.NeedSave = false;
						}

						return chunkColumn;
					}

					regionFile.Seek(offset, SeekOrigin.Begin);
					byte[] waste = new byte[4];
					regionFile.Read(waste, 0, 4);
					int compressionMode = regionFile.ReadByte();

					if (compressionMode != 0x02)
						throw new Exception($"CX={coordinates.X}, CZ={coordinates.Z}, NBT wrong compression. Expected 0x02, got 0x{compressionMode:X2}. " +
											$"Offset={offset}, length={length}\n{Packet.HexDump(waste)}");

					var nbt = new NbtFile();
					nbt.LoadFromStream(regionFile, NbtCompression.ZLib);

					NbtCompound dataTag = (NbtCompound) nbt.RootTag["Level"];

					bool isPocketEdition = false;
					if (dataTag.Contains("MCPE BID"))
					{
						isPocketEdition = dataTag["MCPE BID"].ByteValue == 1;
					}

					NbtList sections = dataTag["Sections"] as NbtList;

					ChunkColumn chunk = new ChunkColumn
					{
						X = coordinates.X,
						Z = coordinates.Z,
						biomeId = dataTag["Biomes"].ByteArrayValue,
						IsAllAir = true
					};

					if (chunk.biomeId.Length > 256) throw new Exception();

					NbtTag heights = dataTag["HeightMap"] as NbtIntArray;
					if (heights != null)
					{
						int[] intHeights = heights.IntArrayValue;
						for (int i = 0; i < 256; i++)
						{
							chunk.height[i] = (short) intHeights[i];
						}
					}

					// This will turn into a full chunk column
					foreach (NbtTag sectionTag in sections)
					{
						ReadSection(sectionTag, chunk, !isPocketEdition);
					}

					NbtList entities = dataTag["Entities"] as NbtList;

					NbtList blockEntities = dataTag["TileEntities"] as NbtList;
					if (blockEntities != null)
					{
						foreach (var nbtTag in blockEntities)
						{
							var blockEntityTag = (NbtCompound) nbtTag.Clone();
							string entityId = blockEntityTag["id"].StringValue;
							int x = blockEntityTag["x"].IntValue;
							int y = blockEntityTag["y"].IntValue;
							int z = blockEntityTag["z"].IntValue;

							if (entityId.StartsWith("minecraft:"))
							{
								var id = entityId.Split(':')[1];

								entityId = id.First().ToString().ToUpper() + id.Substring(1);
								if (entityId == "Flower_pot") entityId = "FlowerPot";
								else if (entityId == "Shulker_box") entityId = "ShulkerBox";
								else if (entityId == "Mob_spawner") entityId = "MobSpawner";

								blockEntityTag["id"] = new NbtString("id", entityId);
							}

							BlockEntity blockEntity = BlockEntityFactory.GetBlockEntityById(entityId);

							if (blockEntity != null)
							{
								blockEntityTag.Name = string.Empty;
								blockEntity.Coordinates = new BlockCoordinates(x, y, z);

								if (blockEntity is SignBlockEntity)
								{
									if (Log.IsDebugEnabled) Log.Debug($"Loaded sign block entity\n{blockEntityTag}");
									// Remove the JSON stuff and get the text out of extra data.
									// TAG_String("Text2"): "{"extra":["10c a loaf!"],"text":""}"
									CleanSignText(blockEntityTag, "Text1");
									CleanSignText(blockEntityTag, "Text2");
									CleanSignText(blockEntityTag, "Text3");
									CleanSignText(blockEntityTag, "Text4");
								}
								else if (blockEntity is ChestBlockEntity || blockEntity is ShulkerBoxBlockEntity)
								{
									if (blockEntity is ShulkerBoxBlockEntity)
									{
										//var meta = chunk.GetMetadata(x & 0x0f, y, z & 0x0f);

										//blockEntityTag["facing"] = new NbtByte("facing", (byte) (meta >> 4));

										//chunk.SetBlock(x & 0x0f, y, z & 0x0f, 218,(byte) (meta - ((byte) (meta >> 4) << 4)));
									}

									NbtList items = (NbtList) blockEntityTag["Items"];

									if (items != null)
									{
										for (byte i = 0; i < items.Count; i++)
										{
											NbtCompound item = (NbtCompound) items[i];

											string itemName = item["id"].StringValue;
											if (itemName.StartsWith("minecraft:"))
											{
												var id = itemName.Split(':')[1];

												itemName = id.First().ToString().ToUpper() + id.Substring(1);
											}

											short itemId = ItemFactory.GetItemIdByName(itemName);
											item.Remove("id");
											item.Add(new NbtShort("id", itemId));
										}
									}
								}
								else if (blockEntity is BedBlockEntity)
								{
									var color = blockEntityTag["color"];
									blockEntityTag.Remove("color");
									blockEntityTag.Add(color is NbtByte ? color : new NbtByte("color", (byte) color.IntValue));
								}
								else if (blockEntity is FlowerPotBlockEntity)
								{
									string itemName = blockEntityTag["Item"].StringValue;
									if (itemName.StartsWith("minecraft:"))
									{
										var id = itemName.Split(':')[1];

										itemName = id.First().ToString().ToUpper() + id.Substring(1);
									}

									short itemId = ItemFactory.GetItemIdByName(itemName);
									blockEntityTag.Remove("Item");
									blockEntityTag.Add(new NbtShort("item", itemId));

									var data = blockEntityTag["Data"].IntValue;
									blockEntityTag.Remove("Data");
									blockEntityTag.Add(new NbtInt("mData", data));
								}
								else
								{
									if (Log.IsDebugEnabled) Log.Debug($"Loaded block entity\n{blockEntityTag}");
									blockEntity.SetCompound(blockEntityTag);
									blockEntityTag = blockEntity.GetCompound();
								}

								chunk.SetBlockEntity(new BlockCoordinates(x, y, z), blockEntityTag);
							}
							else
							{
								if (Log.IsDebugEnabled) Log.Debug($"Loaded unknown block entity\n{blockEntityTag}");
							}
						}
					}

					//NbtList tileTicks = dataTag["TileTicks"] as NbtList;

					if (Dimension == Dimension.Overworld && Config.GetProperty("CalculateLights", false))
					{
						chunk.RecalcHeight();

						SkyLightBlockAccess blockAccess = new SkyLightBlockAccess(this, chunk);
						new SkyLightCalculations().RecalcSkyLight(chunk, blockAccess);
						//TODO: Block lights.
					}

					chunk.IsDirty = false;
					chunk.NeedSave = false;

					return chunk;
				}
			}
			catch (Exception e)
			{
				Log.Error($"Loading chunk {coordinates}", e);
				var chunkColumn = generator?.GenerateChunkColumn(coordinates);
				if (chunkColumn != null)
				{
					//chunkColumn.NeedSave = true;
				}

				return chunkColumn;
			}
		}

		private void ReadSection(NbtTag sectionTag, ChunkColumn chunkColumn, bool convertBid = true)
		{
			int sectionIndex = sectionTag["Y"].ByteValue;
			byte[] blocks = sectionTag["Blocks"].ByteArrayValue;
			byte[] data = sectionTag["Data"].ByteArrayValue;
			NbtTag addTag = sectionTag["Add"];
			byte[] adddata = new byte[2048];
			if (addTag != null) adddata = addTag.ByteArrayValue;
			byte[] blockLight = sectionTag["BlockLight"].ByteArrayValue;
			byte[] skyLight = sectionTag["SkyLight"].ByteArrayValue;

			var subChunk = chunkColumn[4 + sectionIndex]; //Offset by 4 because of 1.18 world update.

			for (int x = 0; x < 16; x++)
			{
				for (int z = 0; z < 16; z++)
				{
					for (int y = 0; y < 16; y++)
					{
						int yi = (sectionIndex << 4) + y;

						int anvilIndex = (y << 8) + (z << 4) + x;
						int blockId = blocks[anvilIndex] + (Nibble4(adddata, anvilIndex) << 8);
						
						if (blockId == 0) continue;
						
						// Anvil to PE friendly converstion

						Func<int, byte, byte> dataConverter = (i, b) => b; // Default no-op converter
						if (convertBid && Convert.ContainsKey(blockId))
						{
							dataConverter = Convert[blockId].Item2;
							blockId = Convert[blockId].Item1;
						}
						//else
						//{
						//	if (BlockFactory.GetBlockById((byte)blockId).GetType() == typeof(Block))
						//	{
						//		Log.Warn($"No block implemented for block ID={blockId}, Meta={data}");
						//		//blockId = 57;
						//	}
						//}

						chunkColumn.IsAllAir &= blockId == 0;
						if (blockId > 255)
						{
							Log.Warn($"Failed mapping for block ID={blockId}, Meta={data}");
							blockId = 41;
						}

						if (yi == 0 && (blockId == 8 || blockId == 9)) blockId = 7; // Bedrock under water

						byte metadata = Nibble4(data, anvilIndex);
						metadata = dataConverter(blockId, metadata);

						int runtimeId = (int) BlockFactory.GetRuntimeId(blockId, metadata);
						subChunk.SetBlockByRuntimeId(x, y, z, runtimeId);
						if (ReadBlockLight)
						{
							subChunk.SetBlocklight(x, y, z, Nibble4(blockLight, anvilIndex));
						}

						if (ReadSkyLight)
						{
							subChunk.SetSkylight(x, y, z, Nibble4(skyLight, anvilIndex));
						}
						else
						{
							subChunk.SetSkylight(x, y, z, 0);
						}

						if (blockId == 0) continue;

						if (convertBid && blockId == 3 && metadata == 2)
						{
							// Dirt Podzol => (Podzol)
							subChunk.SetBlock(x, y, z, new Podzol());
							blockId = 243;
						}

						if (BlockFactory.LuminousBlocks[blockId] != 0)
						{
							var block = BlockFactory.GetBlockById(subChunk.GetBlockId(x, y, z));
							block.Coordinates = new BlockCoordinates(x + (chunkColumn.X << 4), yi, z + (chunkColumn.Z << 4));
							subChunk.SetBlocklight(x, y, z, (byte) block.LightLevel);
							lock (LightSources) LightSources.Enqueue(block);
						}
					}
				}
			}
		}

		private static Regex _regex = new Regex(@"^((\{""extra"":\[)?)(\{""text"":"".*?""})(],)?(""text"":"".*?""})?$");

		private static void CleanSignText(NbtCompound blockEntityTag, string tagName)
		{
			var text = blockEntityTag[tagName].StringValue;
			var replace = /*Regex.Unescape*/(_regex.Replace(text, "$3"));
			blockEntityTag[tagName] = new NbtString(tagName, replace);
		}

		private static byte Nibble4(byte[] arr, int index)
		{
			return (byte) (arr[index >> 1] >> ((index & 1) * 4) & 0xF);
		}

		private static void SetNibble4(byte[] arr, int index, byte value)
		{
			value &= 0xF;
			var idx = index >> 1;
			arr[idx] &= (byte) (0xF << (((index + 1) & 1) * 4));
			arr[idx] |= (byte) (value << ((index & 1) * 4));
		}

		public Vector3 GetSpawnPoint()
		{
			var spawnPoint = new Vector3(LevelInfo.SpawnX, LevelInfo.SpawnY + 2 /* + WaterOffsetY*/, LevelInfo.SpawnZ);
			if (Dimension == Dimension.TheEnd)
			{
				spawnPoint = new Vector3(100, 49, 0);
			}
			else if (Dimension == Dimension.Nether)
			{
				spawnPoint = new Vector3(0, 80, 0);
			}

			if (spawnPoint.Y > 256) spawnPoint.Y = 255;

			return spawnPoint;
		}

		public long GetTime()
		{
			return LevelInfo.Time;
		}

		public long GetDayTime()
		{
			return LevelInfo.DayTime;
		}

		public string GetName()
		{
			return LevelInfo.LevelName;
		}

		public void SaveLevelInfo(LevelInfo level)
		{
			if (Dimension != Dimension.Overworld) return;
			
			level.LastPlayed = DateTimeOffset.Now.ToUnixTimeMilliseconds();
			var leveldat = Path.Combine(BasePath, "level.dat");

			if (!Directory.Exists(BasePath))
				Directory.CreateDirectory(BasePath);
			else if (File.Exists(leveldat))
				return; // What if this is changed? Need a dirty flag on this

			if (LevelInfo.SpawnY <= 0) LevelInfo.SpawnY = 256;
			
			NbtFile file = new NbtFile();
			NbtTag dataTag = new NbtCompound("Data");
			NbtCompound rootTag = (NbtCompound) file.RootTag;
			rootTag.Add(dataTag);
			level.SaveToNbt(dataTag);
			file.SaveToFile(leveldat, NbtCompression.GZip);
		}

		public int SaveChunks()
		{
			if (!Config.GetProperty("Save.Enabled", false)) return 0;

			int count = 0;
			try
			{
				lock (_chunkCache)
				{
					if (Dimension == Dimension.Overworld) SaveLevelInfo(LevelInfo);

					var regions = new Dictionary<Tuple<int, int>, List<ChunkColumn>>();
					foreach (var chunkColumn in _chunkCache.OrderBy(pair => pair.Key.X >> 5).ThenBy(pair => pair.Key.Z >> 5))
					{
						var regionKey = new Tuple<int, int>(chunkColumn.Key.X >> 5, chunkColumn.Key.Z >> 5);
						if (!regions.ContainsKey(regionKey))
						{
							regions.Add(regionKey, new List<ChunkColumn>());
						}

						regions[regionKey].Add(chunkColumn.Value);
					}

					var tasks = new List<Task>();
					foreach (var region in regions.OrderBy(pair => pair.Key.Item1).ThenBy(pair => pair.Key.Item2))
					{
						Task task = new Task(delegate
						{
							List<ChunkColumn> chunks = region.Value;
							foreach (var chunkColumn in chunks)
							{
								if (chunkColumn != null && chunkColumn.NeedSave)
								{
									SaveChunk(chunkColumn, BasePath);
									count++;
								}
							}
						});
						task.Start();
						tasks.Add(task);
					}

					Task.WaitAll(tasks.ToArray());

					//foreach (var chunkColumn in _chunkCache.OrderBy(pair => pair.Key.X >> 5).ThenBy(pair => pair.Key.Z >> 5))
					//{
					//	if (chunkColumn.Value != null && chunkColumn.Value.NeedSave)
					//	{
					//		SaveChunk(chunkColumn.Value, BasePath);
					//		count++;
					//	}
					//}
				}
			}
			catch (Exception e)
			{
				Log.Error("saving chunks", e);
			}

			return count;
		}

		public bool HaveNether()
		{
			//return !(MissingChunkProvider is SuperflatGenerator);
			return Directory.Exists(Path.Combine(BasePath, @"DIM-1"));
		}

		public bool HaveTheEnd()
		{
			//return !(MissingChunkProvider is SuperflatGenerator);
			return Directory.Exists(Path.Combine(BasePath, @"DIM1"));
		}

		public static void SaveChunk(ChunkColumn chunk, string basePath)
		{
			// WARNING: This method does not consider growing size of the chunks. Needs refactoring to find
			// free sectors and clear up old ones. It works fine as long as no dynamic data is written
			// like block entity data (signs etc).

			var time = Stopwatch.StartNew();

			chunk.NeedSave = false;

			var coordinates = new ChunkCoordinates(chunk.X, chunk.Z);

			int width = 32;
			int depth = 32;

			int rx = coordinates.X >> 5;
			int rz = coordinates.Z >> 5;

			string filePath = Path.Combine(basePath, string.Format(@"region{2}r.{0}.{1}.mca", rx, rz, Path.DirectorySeparatorChar));

			Log.Debug($"Save chunk X={chunk.X}, Z={chunk.Z} to {filePath}");

			if (!File.Exists(filePath))
			{
				// Make sure directory exist
				Directory.CreateDirectory(Path.Combine(basePath, "region"));

				// Create empty region file
				using (var regionFile = File.Open(filePath, FileMode.CreateNew))
				{
					byte[] buffer = new byte[8192];
					regionFile.Write(buffer, 0, buffer.Length);
				}
			}

			var testTime = new Stopwatch();

			using (var regionFile = File.Open(filePath, FileMode.Open))
			{
				// Region files begin with an 8kiB header containing information about which chunks are present in the region file, 
				// when they were last updated, and where they can be found.
				byte[] buffer = new byte[8192];
				regionFile.Read(buffer, 0, buffer.Length);

				int xi = (coordinates.X % width);
				if (xi < 0) xi += 32;
				int zi = (coordinates.Z % depth);
				if (zi < 0) zi += 32;
				int tableOffset = (xi + zi * width) * 4;

				regionFile.Seek(tableOffset, SeekOrigin.Begin);

				// Location information for a chunk consists of four bytes split into two fields: the first three bytes are a(big - endian) offset in 4KiB sectors 
				// from the start of the file, and a remaining byte which gives the length of the chunk(also in 4KiB sectors, rounded up).
				byte[] offsetBuffer = new byte[4];
				regionFile.Read(offsetBuffer, 0, 3);
				Array.Reverse(offsetBuffer);
				int offset = BitConverter.ToInt32(offsetBuffer, 0) << 4;
				byte sectorCount = (byte) regionFile.ReadByte();

				testTime.Restart(); // RESTART

				// Seriaize NBT to get lenght
				NbtFile nbt = CreateNbtFromChunkColumn(chunk);

				testTime.Stop();

				byte[] nbtBuf = nbt.SaveToBuffer(NbtCompression.ZLib);
				int nbtLength = nbtBuf.Length;
				byte nbtSectorCount = (byte) Math.Ceiling(nbtLength / 4096d);

				// Don't write yet, just use the lenght

				if (offset == 0 || sectorCount == 0 || nbtSectorCount > sectorCount)
				{
					if (Log.IsDebugEnabled)
						if (sectorCount != 0)
							Log.Warn($"Creating new sectors for this chunk even tho it existed. Old sector count={sectorCount}, new sector count={nbtSectorCount} (lenght={nbtLength})");

					regionFile.Seek(0, SeekOrigin.End);
					offset = (int) ((int) regionFile.Position & 0xfffffff0);

					regionFile.Seek(tableOffset, SeekOrigin.Begin);

					byte[] bytes = BitConverter.GetBytes(offset >> 4);
					Array.Reverse(bytes);
					regionFile.Write(bytes, 0, 3);
					regionFile.WriteByte(nbtSectorCount);
				}

				byte[] lenghtBytes = BitConverter.GetBytes(nbtLength + 1);
				Array.Reverse(lenghtBytes);

				regionFile.Seek(offset, SeekOrigin.Begin);
				regionFile.Write(lenghtBytes, 0, 4); // Lenght
				regionFile.WriteByte(0x02); // Compression mode zlib

				regionFile.Write(nbtBuf, 0, nbtBuf.Length);

				int reminder;
				Math.DivRem(nbtLength + 4, 4096, out reminder);

				byte[] padding = new byte[4096 - reminder];
				if (padding.Length > 0) regionFile.Write(padding, 0, padding.Length);

				testTime.Stop(); // STOP

				Log.Warn($"Took {time.ElapsedMilliseconds}ms to save. And {testTime.ElapsedMilliseconds}ms to generate bytes from NBT");
			}
		}

		public static NbtFile CreateNbtFromChunkColumn(ChunkColumn chunk)
		{
			var nbt = new NbtFile();

			var levelTag = new NbtCompound("Level");
			var rootTag = (NbtCompound) nbt.RootTag;
			rootTag.Add(levelTag);

			levelTag.Add(new NbtByte("MCPE BID", 1)); // Indicate that the chunks contain PE block ID's.

			levelTag.Add(new NbtInt("xPos", chunk.X));
			levelTag.Add(new NbtInt("zPos", chunk.Z));
			levelTag.Add(new NbtByteArray("Biomes", chunk.biomeId));

			NbtList sectionsTag = new NbtList("Sections", NbtTagType.Compound);
			levelTag.Add(sectionsTag);

			for (int i = 0; i < 16; i++)
			{
				SubChunk subChunk = chunk[i];
				if (subChunk.IsAllAir())
				{
					if(i == 0) Log.Debug($"All air bottom chunk? {subChunk.GetBlockId(0,0,0)}");
					continue;
				}

				var sectionTag = new NbtCompound();
				sectionsTag.Add(sectionTag);
				sectionTag.Add(new NbtByte("Y", (byte) i));

				var blocks = new byte[4096];
				var data = new byte[2048];
				var blockLight = new byte[2048];
				var skyLight = new byte[2048];

				{
					for (int x = 0; x < 16; x++)
					{
						for (int z = 0; z < 16; z++)
						{
							for (int y = 0; y < 16; y++)
							{
								int anvilIndex = y * 16 * 16 + z * 16 + x;
								byte blockId = (byte) subChunk.GetBlockId(x, y, z);
								blocks[anvilIndex] = blockId;
								//SetNibble4(data, anvilIndex, section.GetMetadata(x, y, z));
								SetNibble4(blockLight, anvilIndex, subChunk.GetBlocklight(x, y, z));
								SetNibble4(skyLight, anvilIndex, subChunk.GetSkylight(x, y, z));
							}
						}
					}
				}
				sectionTag.Add(new NbtByteArray("Blocks", blocks));
				sectionTag.Add(new NbtByteArray("Data", data));
				sectionTag.Add(new NbtByteArray("BlockLight", blockLight));
				sectionTag.Add(new NbtByteArray("SkyLight", skyLight));
			}

			var heights = new int[256];
			for (int h = 0; h < heights.Length; h++)
			{
				heights[h] = chunk.height[h];
			}
			levelTag.Add(new NbtIntArray("HeightMap", heights));

			// TODO: Save entities
			var entitiesTag = new NbtList("Entities", NbtTagType.Compound);
			levelTag.Add(entitiesTag);

			var blockEntitiesTag = new NbtList("TileEntities", NbtTagType.Compound);
			foreach (NbtCompound blockEntityNbt in chunk.BlockEntities.Values)
			{
				var nbtClone = (NbtCompound) blockEntityNbt.Clone();
				nbtClone.Name = null;
				blockEntitiesTag.Add(nbtClone);
			}

			levelTag.Add(blockEntitiesTag);

			levelTag.Add(new NbtList("TileTicks", NbtTagType.Compound));

			return nbt;
		}

		public int NumberOfCachedChunks()
		{
			return _chunkCache.Count;
		}

		public object Clone()
		{
			ConcurrentDictionary<ChunkCoordinates, ChunkColumn> chunkCache = new ConcurrentDictionary<ChunkCoordinates, ChunkColumn>();
			foreach (KeyValuePair<ChunkCoordinates, ChunkColumn> valuePair in _chunkCache)
			{
				chunkCache.TryAdd(valuePair.Key, (ChunkColumn) valuePair.Value?.Clone());
			}

			AnvilWorldProvider provider = new AnvilWorldProvider(BasePath, (LevelInfo) LevelInfo.Clone(), chunkCache);
			return provider;
		}

		public int PruneAir()
		{
			int prunedChunks = 0;
			Stopwatch sw = new Stopwatch();
			sw.Start();

			foreach (KeyValuePair<ChunkCoordinates, ChunkColumn> valuePair in _chunkCache)
			{
				ChunkCoordinates chunkCoordinates = valuePair.Key;
				ChunkColumn chunkColumn = valuePair.Value;

				if (chunkColumn != null && chunkColumn.IsAllAir)
				{
					bool surroundingIsAir = true;

					for (int startX = chunkCoordinates.X - 1; startX <= chunkCoordinates.X + 1; startX++)
					{
						for (int startZ = chunkCoordinates.Z - 1; startZ <= chunkCoordinates.Z + 1; startZ++)
						{
							ChunkCoordinates surroundingChunkCoordinates = new ChunkCoordinates(startX, startZ);

							if (!surroundingChunkCoordinates.Equals(chunkCoordinates))
							{
								_chunkCache.TryGetValue(surroundingChunkCoordinates, out var surroundingChunkColumn);

								if (surroundingChunkColumn != null && !surroundingChunkColumn.IsAllAir)
								{
									surroundingIsAir = false;
									break;
								}
							}
						}
					}

					if (surroundingIsAir)
					{
						_chunkCache.TryGetValue(chunkCoordinates, out var chunk);
						_chunkCache[chunkCoordinates] = null;
						if (chunk != null)
						{
							foreach (var c in chunk)
							{
								c.PutPool();
							}
						}
						prunedChunks++;
					}
				}
			}

			sw.Stop();
			Log.Info("Pruned " + prunedChunks + " in " + sw.ElapsedMilliseconds + "ms");
			return prunedChunks;
		}

		public int MakeAirChunksAroundWorldToCompensateForBadRendering()
		{
			int createdChunks = 0;
			Stopwatch sw = new Stopwatch();
			sw.Start();

			foreach (KeyValuePair<ChunkCoordinates, ChunkColumn> valuePair in _chunkCache)
			{
				ChunkCoordinates chunkCoordinates = valuePair.Key;
				ChunkColumn chunkColumn = valuePair.Value;

				if (chunkColumn != null && !chunkColumn.IsAllAir)
				{
					for (int startX = chunkCoordinates.X - 1; startX <= chunkCoordinates.X + 1; startX++)
					{
						for (int startZ = chunkCoordinates.Z - 1; startZ <= chunkCoordinates.Z + 1; startZ++)
						{
							ChunkCoordinates surroundingChunkCoordinates = new ChunkCoordinates(startX, startZ);

							if (surroundingChunkCoordinates.Equals(chunkCoordinates)) continue;

							ChunkColumn surroundingChunkColumn;

							_chunkCache.TryGetValue(surroundingChunkCoordinates, out surroundingChunkColumn);

							if (surroundingChunkColumn == null)
							{
								ChunkColumn airColumn = new ChunkColumn
								{
									X = startX,
									Z = startZ,
									IsAllAir = true
								};

								airColumn.GetBatch();

								_chunkCache[surroundingChunkCoordinates] = airColumn;
								createdChunks++;
							}
						}
					}
				}
			}

			sw.Stop();
			Log.Info("Created " + createdChunks + " air chunks in " + sw.ElapsedMilliseconds + "ms");
			return createdChunks;
		}
	}
}
