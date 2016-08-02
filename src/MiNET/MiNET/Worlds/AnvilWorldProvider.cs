using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;
using fNbt;
using log4net;
using MiNET.BlockEntities;
using MiNET.Blocks;
using MiNET.Utils;

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

	public class AnvilWorldProvider : IWorldProvider, ICloneable
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (AnvilWorldProvider));

		private static readonly Dictionary<int, Tuple<int, Func<int, byte, byte>>> Convert;

	    public IWorldProvider MissingChunkProvider { get; set; }

	    public LevelInfo LevelInfo { get; private set; }

	    public ConcurrentDictionary<ChunkCoordinates, ChunkColumn> _chunkCache = new ConcurrentDictionary<ChunkCoordinates, ChunkColumn>();

	    public string BasePath { get; private set; }

	    public bool IsCaching { get; private set; }

		public byte WaterOffsetY { get; set; }

		static AnvilWorldProvider()
		{
			var air = new Mapper(0, (i, b) => 0);

			Convert = new Dictionary<int, Tuple<int, Func<int, byte, byte>>>
			{
				{23, air}, // minecraft:dispenser	=> Air
				{29, air}, // minecraft:sticky_piston	=> Air
				{33, air}, // minecraft:piston		=> Air
				{34, air}, // minecraft:piston_head		=> Air
				{36, air}, // minecraft:piston_extension		=> Air
				{84, air}, // minecraft:jukebox		=> Air
				{85, new Mapper(85, (i, b) => 0)}, // Fence		=> Fence
				//{90, air}, // Nether Portal	=> Air
				{93, air}, // minecraft:unpowered_repeater	=> Air
				{94, air}, // minecraft:powered_repeater	=> Air
				{95, new NoDataMapper(20)}, // minecraft:stained_glass	=> Glass
				{96, new Mapper(96, (i, b) => (byte) (((b & 0x04) << 1) | ((b & 0x08) >> 1) | (3 - (b & 0x03))))}, // Trapdoor Fix
				//{113, new NoDataMapper(85)}, // Nether Fence		=> Fence
				//{118, air}, // minecraft:cauldron		=> Air
				{119, air}, // minecraft:end_portal		=> Air
				{122, air}, // Dragon Egg		=> Air
				//{123, new NoDataMapper(122)}, // Redstone Lamp O	=> Glowstone
				//{124, new NoDataMapper(123)}, // Redstone Lamp O	=> Glowstone
				{125, new NoDataMapper(157)}, // minecraft:double_wooden_slab	=> minecraft:double_wooden_slab
				{126, new NoDataMapper(158)}, // minecraft:wooden_slab		=> minecraft:wooden_slab
				{130, new NoDataMapper(54)}, // Ender Chest		=> Chest
				{137, air}, // Command Block	=> Air
				{138, air}, // Beacon		=> Air
				{
					143, new Mapper(143, delegate(int i, byte b)
					{
						switch (b & 0x7f)
						{
							case 0:
								return (byte) BlockFace.Down; // 0
							case 1:
								return (byte) BlockFace.South; // 5
							case 2:
								return (byte) BlockFace.North; // 4
							case 3:
								return (byte) BlockFace.West; // 3
							case 4:
								return (byte) BlockFace.East; // 2
							case 5:
								return (byte) BlockFace.Up; // 1
						}

						return 0;
					})
				}, // Trapdoor Fix
				{
					77, new Mapper(77, delegate(int i, byte b)
					{
						switch (b & 0x7f)
						{
							case 0:
								return (byte) BlockFace.Down;
							case 1:
								return (byte) BlockFace.South;
							case 2:
								return (byte) BlockFace.North;
							case 3:
								return (byte) BlockFace.West;
							case 4:
								return (byte) BlockFace.East;
							case 5:
								return (byte) BlockFace.Up;
						}

						return 0;
					})
				}, // Trapdoor Fix
				{149, air}, // minecraft:unpowered_comparator		=> Air
				{150, air}, // minecraft:powered_comparator		=> Air
				//{154, air}, // minecraft:hopper		=> Air
				{157, new NoDataMapper(126)}, // minecraft:activator_rail	=> minecraft:activator_rail
				{158, new NoDataMapper(125)}, // minecraft:dropper		=> Air
				{160, new NoDataMapper(102)}, // minecraft:stained_glass_pane	=> Glass Pane
				//{165, air}, // Slime Block		=> Air
				{166, new NoDataMapper(95)}, // minecraft:barrier		=> (Invisible Bedrock)
				{168, air}, // minecraft:prismarine		=> Air
				{169, new NoDataMapper(89)}, // minecraft:sea_lantern		=> Glowstone
				{176, air}, // minecraft:standing_banner		=> Air
				{177, air}, // minecraft:wall_banner		=> Air
				// 179-182 Need mapping (Red Sandstone)
				{183, new NoDataMapper(183)}, // Spruce Gate		=> Gate
				{184, new NoDataMapper(184)}, // Birch Gate		=> Gate
				{185, new NoDataMapper(185)}, // Jungle Gate		=> Gate
				{186, new NoDataMapper(186)}, // Dark Oak Gate	=> Gate
				{187, new NoDataMapper(187)}, // Acacia Gate		=> Gate
				{188, new Mapper(85, (i, b) => 1)}, // Spruce Fence		=> Fence
				{189, new Mapper(85, (i, b) => 2)}, // Birch Fence		=> Fence
				{190, new Mapper(85, (i, b) => 3)}, // Jungle Fence		=> Fence
				{191, new Mapper(85, (i, b) => 5)}, // Dark Oak Fence	=> Fence
				{192, new Mapper(85, (i, b) => 4)}, // Acacia Fence		=> Fence
				{198, air}, // minecraft:end_rod		=> Air
				{212, new NoDataMapper(174)}, // Frosted Ice => Packed Ice
			};
		}

		public AnvilWorldProvider()
		{
			IsCaching = true;
			//_flatland = new FlatlandWorldProvider();
		}

		public AnvilWorldProvider(string basePath) : this()
		{
			BasePath = basePath;
		}

		protected AnvilWorldProvider(string basePath, LevelInfo levelInfo, byte waterOffsetY, ConcurrentDictionary<ChunkCoordinates, ChunkColumn> chunkCache)
		{
			IsCaching = true;
			BasePath = basePath;
			LevelInfo = levelInfo;
			WaterOffsetY = waterOffsetY;
			_chunkCache = chunkCache;
			_isInitialized = true;
			//_flatland = new FlatlandWorldProvider();
		}

		private bool _isInitialized = false;
		private object _initializeSync = new object();

		public void Initialize()
		{
			if (_isInitialized) return; // Quick exit

			lock (_initializeSync)
			{
				if (_isInitialized) return;

				BasePath = BasePath ?? Config.GetProperty("PCWorldFolder", "World").Trim();

				NbtFile file = new NbtFile();
				file.LoadFromFile(Path.Combine(BasePath, "level.dat"));
				NbtTag dataTag = file.RootTag["Data"];
				LevelInfo = new LevelInfo(dataTag);

				WaterOffsetY = WaterOffsetY == 0 ? (byte) Config.GetProperty("PCWaterOffset", 0) : WaterOffsetY;

				_isInitialized = true;
			}
		}

		private int Noop(int blockId, int data)
		{
			return 0;
		}

		public ChunkColumn[] GetCachedChunks()
		{
			lock (_chunkCache)
			{
				return _chunkCache.Values.Where(column => column != null).ToArray();
			}
		}

		public ChunkColumn GenerateChunkColumn(ChunkCoordinates chunkCoordinates)
		{
			lock (_chunkCache)
			{
				ChunkColumn cachedChunk;
				if (_chunkCache.TryGetValue(chunkCoordinates, out cachedChunk)) return cachedChunk;

				ChunkColumn chunk = GetChunk(chunkCoordinates, BasePath, MissingChunkProvider, WaterOffsetY);

				_chunkCache[chunkCoordinates] = chunk;

				return chunk;
			}
		}

		public static ChunkColumn GetChunk(ChunkCoordinates coordinates, string basePath, IWorldProvider generator, int yoffset)
		{
			int width = 32;
			int depth = 32;

			int rx = coordinates.X >> 5;
			int rz = coordinates.Z >> 5;

			string filePath = Path.Combine(basePath, string.Format(@"region{2}r.{0}.{1}.mca", rx, rz, Path.DirectorySeparatorChar));

			if (!File.Exists(filePath))
			{
				return generator?.GenerateChunkColumn(coordinates);
				//return new ChunkColumn
				//{
				//	x = coordinates.X,
				//	z = coordinates.Z,
				//};
			}

			using (var regionFile = File.OpenRead(filePath))
			{
				byte[] buffer = new byte[8192];

				regionFile.Read(buffer, 0, 8192);

				int xi = (coordinates.X%width);
				if (xi < 0) xi += 32;
				int zi = (coordinates.Z%depth);
				if (zi < 0) zi += 32;
				int tableOffset = (xi + zi*width)*4;

				regionFile.Seek(tableOffset, SeekOrigin.Begin);

				byte[] offsetBuffer = new byte[4];
				regionFile.Read(offsetBuffer, 0, 3);
				Array.Reverse(offsetBuffer);
				int offset = BitConverter.ToInt32(offsetBuffer, 0) << 4;

				int length = regionFile.ReadByte();

				if (offset == 0 || length == 0)
				{
					return generator?.GenerateChunkColumn(coordinates);
					//return new ChunkColumn
					//{
					//	x = coordinates.X,
					//	z = coordinates.Z,
					//};
				}

				regionFile.Seek(offset, SeekOrigin.Begin);
				byte[] waste = new byte[4];
				regionFile.Read(waste, 0, 4);
				int compressionMode = regionFile.ReadByte();

				var nbt = new NbtFile();
				nbt.LoadFromStream(regionFile, NbtCompression.ZLib);

				NbtTag dataTag = nbt.RootTag["Level"];

				NbtList sections = dataTag["Sections"] as NbtList;

				ChunkColumn chunk = new ChunkColumn
				{
					x = coordinates.X,
					z = coordinates.Z,
					biomeId = dataTag["Biomes"].ByteArrayValue,
					isAllAir = true
				};

				if (chunk.biomeId.Length > 256) throw new Exception();

				// This will turn into a full chunk column
				foreach (NbtTag sectionTag in sections)
				{
					int sy = sectionTag["Y"].ByteValue*16;
					byte[] blocks = sectionTag["Blocks"].ByteArrayValue;
					byte[] data = sectionTag["Data"].ByteArrayValue;
					NbtTag addTag = sectionTag["Add"];
					byte[] adddata = new byte[2048];
					if (addTag != null) adddata = addTag.ByteArrayValue;
					byte[] blockLight = sectionTag["BlockLight"].ByteArrayValue;
					byte[] skyLight = sectionTag["SkyLight"].ByteArrayValue;

					for (int x = 0; x < 16; x++)
					{
						for (int z = 0; z < 16; z++)
						{
							for (int y = 0; y < 16; y++)
							{
								int yi = sy + y - yoffset;
								if (yi < 0 || yi >= 128) continue;

								int anvilIndex = y*16*16 + z*16 + x;
								int blockId = blocks[anvilIndex] + (Nibble4(adddata, anvilIndex) << 8);

								// Anvil to PE friendly converstion

								Func<int, byte, byte> dataConverter = (i, b) => b; // Default no-op converter
								if (Convert.ContainsKey(blockId))
								{
									dataConverter = Convert[blockId].Item2;
									blockId = Convert[blockId].Item1;
								}
								else
								{
									if (BlockFactory.GetBlockById((byte) blockId).GetType() == typeof (Block))
									{
										Log.Warn($"No block implemented for block ID={blockId}, Meta={data}");
										//blockId = 57;
									}
								}

								chunk.isAllAir = chunk.isAllAir && blockId == 0;
								if (blockId > 255)
								{
									Log.Warn($"Failed mapping for block ID={blockId}, Meta={data}");
									blockId = 41;
								}

								//if (yi == 127 && blockId != 0) blockId = 30;
								if (yi == 0 && (blockId == 8 || blockId == 9)) blockId = 7;

								chunk.SetBlock(x, yi, z, (byte) blockId);
								byte metadata = Nibble4(data, anvilIndex);
								metadata = dataConverter(blockId, metadata);

								chunk.SetMetadata(x, yi, z, metadata);
								chunk.SetBlocklight(x, yi, z, Nibble4(blockLight, anvilIndex));
								chunk.SetSkylight(x, yi, z, Nibble4(skyLight, anvilIndex));

								var block = BlockFactory.GetBlockById(chunk.GetBlock(x, yi, z));
								if (block is BlockStairs || block is StoneSlab || block is WoodSlab)
								{
									chunk.SetSkylight(x, yi, z, 0xff);
								}

								if (blockId == 43 && chunk.GetMetadata(x, yi, z) == 7) chunk.SetMetadata(x, yi, z, 6);
								else if (blockId == 44 && chunk.GetMetadata(x, yi, z) == 7) chunk.SetMetadata(x, yi, z, 6);
								else if (blockId == 44 && chunk.GetMetadata(x, yi, z) == 15) chunk.SetMetadata(x, yi, z, 14);
								else if (blockId == 3 && chunk.GetMetadata(x, yi, z) == 1)
								{
									// Dirt Course => (Grass Path)
									chunk.SetBlock(x, yi, z, 198);
									chunk.SetMetadata(x, yi, z, 0);
								}
								else if (blockId == 3 && chunk.GetMetadata(x, yi, z) == 2)
								{
									// Dirt Podzol => (Podzol)
									chunk.SetBlock(x, yi, z, 243);
									chunk.SetMetadata(x, yi, z, 0);
								}
							}
						}
					}
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
						int y = blockEntityTag["y"].IntValue - yoffset;
						int z = blockEntityTag["z"].IntValue;
						blockEntityTag["y"] = new NbtInt("y", y);

						BlockEntity blockEntity = BlockEntityFactory.GetBlockEntityById(entityId);
						if (blockEntity != null)
						{
							blockEntityTag.Name = string.Empty;

							if (blockEntity is Sign)
							{
								// Remove the JSON stuff and get the text out of extra data.
								// TAG_String("Text2"): "{"extra":["10c a loaf!"],"text":""}"
								CleanSignText(blockEntityTag, "Text1");
								CleanSignText(blockEntityTag, "Text2");
								CleanSignText(blockEntityTag, "Text3");
								CleanSignText(blockEntityTag, "Text4");
							}
							else if (blockEntity is ChestBlockEntity)
							{
								NbtList items = (NbtList) blockEntityTag["Items"];

								if(items != null)
								{
									for (byte i = 0; i < items.Count; i++)
									{
										NbtCompound item = (NbtCompound) items[i];

										item.Add(new NbtShort("OriginalDamage", item["Damage"].ShortValue));

										byte metadata = (byte) (item["Damage"].ShortValue & 0xff);
										item.Remove("Damage");
										item.Add(new NbtByte("Damage", metadata));
									}
								}
							}

							chunk.SetBlockEntity(new BlockCoordinates(x, y, z), blockEntityTag);
						}
					}
				}

				//NbtList tileTicks = dataTag["TileTicks"] as NbtList;

				chunk.isDirty = false;
				return chunk;
			}
		}

		private static Regex _regex = new Regex(@"^((\{""extra"":\[)?)""(.*?)""(],""text"":""""})?$");

		private static void CleanSignText(NbtCompound blockEntityTag, string tagName)
		{
			var text = blockEntityTag[tagName].StringValue;
			var replace = Regex.Unescape(_regex.Replace(text, "$3"));
			blockEntityTag[tagName] = new NbtString(tagName, replace);
		}

		private static byte Nibble4(byte[] arr, int index)
		{
			return (byte) (index%2 == 0 ? arr[index/2] & 0x0F : (arr[index/2] >> 4) & 0x0F);
		}

		private static void SetNibble4(byte[] arr, int index, byte value)
		{
			if (index%2 == 0)
			{
				arr[index/2] = (byte) ((value & 0x0F) | arr[index/2]);
			}
			else
			{
				arr[index/2] = (byte) (((value << 4) & 0xF0) | arr[index/2]);
			}
		}

		public Vector3 GetSpawnPoint()
		{
			var spawnPoint = new Vector3(LevelInfo.SpawnX, LevelInfo.SpawnY + 2 /* + WaterOffsetY*/, LevelInfo.SpawnZ);

			if (spawnPoint.Y > 127) spawnPoint.Y = 127;

			return spawnPoint;
		}

		public long GetTime()
		{
			return LevelInfo.Time;
		}

		public void SaveChunks()
		{
			lock (_chunkCache)
			{
				foreach (var chunkColumn in _chunkCache)
				{
					if (chunkColumn.Value.isDirty) SaveChunk(chunkColumn.Value, BasePath, WaterOffsetY);
				}
			}
		}

		public static void SaveChunk(ChunkColumn chunk, string basePath, int yoffset)
		{
			var coordinates = new ChunkCoordinates(chunk.x, chunk.z);

			int width = 32;
			int depth = 32;

			int rx = coordinates.X >> 5;
			int rz = coordinates.Z >> 5;

			string filePath = Path.Combine(basePath, string.Format(@"region{2}r.{0}.{1}.mca", rx, rz, Path.DirectorySeparatorChar));

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

				return;
			}

			using (var regionFile = File.Open(filePath, FileMode.Open))
			{
				byte[] buffer = new byte[8192];
				regionFile.Read(buffer, 0, buffer.Length);

				int xi = (coordinates.X%width);
				if (xi < 0) xi += 32;
				int zi = (coordinates.Z%depth);
				if (zi < 0) zi += 32;
				int tableOffset = (xi + zi*width)*4;

				regionFile.Seek(tableOffset, SeekOrigin.Begin);

				byte[] offsetBuffer = new byte[4];
				regionFile.Read(offsetBuffer, 0, 3);
				Array.Reverse(offsetBuffer);
				int offset = BitConverter.ToInt32(offsetBuffer, 0) << 4;

				int length = regionFile.ReadByte();

				if (offset == 0 || length == 0)
				{
					regionFile.Seek(0, SeekOrigin.End);
					offset = (int) regionFile.Position;

					regionFile.Seek(tableOffset, SeekOrigin.Begin);

					byte[] bytes = BitConverter.GetBytes(offset >> 4);
					Array.Reverse(bytes);
					regionFile.Write(bytes, 0, 3);
					regionFile.WriteByte(1);
				}

				// Write NBT
				NbtFile nbt = CreateNbtFromChunkColumn(chunk, yoffset);
				byte[] nbtBuf = nbt.SaveToBuffer(NbtCompression.ZLib);

				int lenght = nbtBuf.Length;
				byte[] lenghtBytes = BitConverter.GetBytes(lenght + 1);
				Array.Reverse(lenghtBytes);

				regionFile.Seek(offset, SeekOrigin.Begin);
				regionFile.Write(lenghtBytes, 0, 4); // Lenght
				regionFile.WriteByte(0x02); // Compression mode

				regionFile.Write(nbtBuf, 0, nbtBuf.Length);

				int reminder;
				Math.DivRem(lenght + 4, 4096, out reminder);

				byte[] padding = new byte[4096 - reminder];
				if (padding.Length > 0) regionFile.Write(padding, 0, padding.Length);
			}
		}

		private static NbtFile CreateNbtFromChunkColumn(ChunkColumn chunk, int yoffset)
		{
			var nbt = new NbtFile();

			NbtCompound levelTag = new NbtCompound("Level");
			nbt.RootTag.Add(levelTag);

			levelTag.Add(new NbtInt("xPos", chunk.x));
			levelTag.Add(new NbtInt("zPos", chunk.z));
			levelTag.Add(new NbtByteArray("Biomes", chunk.biomeId));

			NbtList sectionsTag = new NbtList("Sections");
			levelTag.Add(sectionsTag);

			for (int i = 0; i < 8; i++)
			{
				NbtCompound sectionTag = new NbtCompound();
				sectionsTag.Add(sectionTag);
				sectionTag.Add(new NbtByte("Y", (byte) i));
				int sy = i*16;

				byte[] blocks = new byte[4096];
				byte[] data = new byte[2048];
				byte[] blockLight = new byte[2048];
				byte[] skyLight = new byte[2048];

				for (int x = 0; x < 16; x++)
				{
					for (int z = 0; z < 16; z++)
					{
						for (int y = 0; y < 16; y++)
						{
							int yi = sy + y;
							if (yi < 0 || yi >= 256) continue; // ?

							int anvilIndex = (y + yoffset)*16*16 + z*16 + x;
							byte blockId = chunk.GetBlock(x, yi, z);

							// PE to Anvil friendly converstion
							if (blockId == 5) blockId = 125;
							else if (blockId == 158) blockId = 126;
							else if (blockId == 50) blockId = 75;
							else if (blockId == 50) blockId = 76;
							else if (blockId == 89) blockId = 123;
							else if (blockId == 89) blockId = 124;
							else if (blockId == 73) blockId = 152;

							blocks[anvilIndex] = blockId;
							SetNibble4(data, anvilIndex, chunk.GetMetadata(x, yi, z));
							SetNibble4(blockLight, anvilIndex, chunk.GetBlocklight(x, yi, z));
							SetNibble4(skyLight, anvilIndex, chunk.GetSkylight(x, yi, z));
						}
					}
				}

				sectionTag.Add(new NbtByteArray("Blocks", blocks));
				sectionTag.Add(new NbtByteArray("Data", data));
				sectionTag.Add(new NbtByteArray("BlockLight", blockLight));
				sectionTag.Add(new NbtByteArray("SkyLight", skyLight));
			}

			// TODO: Save entities
			NbtList entitiesTag = new NbtList("Entities", NbtTagType.Compound);
			levelTag.Add(entitiesTag);

			NbtList blockEntitiesTag = new NbtList("TileEntities", NbtTagType.Compound);
			levelTag.Add(blockEntitiesTag);
			foreach (NbtCompound blockEntityNbt in chunk.BlockEntities.Values)
			{
				NbtCompound nbtClone = (NbtCompound) blockEntityNbt.Clone();
				nbtClone.Name = null;
				blockEntitiesTag.Add(nbtClone);
			}

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

			AnvilWorldProvider provider = new AnvilWorldProvider(BasePath, (LevelInfo) LevelInfo.Clone(), WaterOffsetY, chunkCache);
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

				if (chunkColumn != null && chunkColumn.isAllAir)
				{
					bool surroundingIsAir = true;

					for (int startX = chunkCoordinates.X - 1; startX <= chunkCoordinates.X + 1; startX++)
					{
						for (int startZ = chunkCoordinates.Z - 1; startZ <= chunkCoordinates.Z + 1; startZ++)
						{
							ChunkCoordinates surroundingChunkCoordinates = new ChunkCoordinates(startX, startZ);

							if (!surroundingChunkCoordinates.Equals(chunkCoordinates))
							{
								ChunkColumn surroundingChunkColumn;

								_chunkCache.TryGetValue(surroundingChunkCoordinates, out surroundingChunkColumn);

								if (surroundingChunkColumn != null && !surroundingChunkColumn.isAllAir)
								{
									surroundingIsAir = false;
									break;
								}
							}
						}
					}

					if (surroundingIsAir)
					{
						_chunkCache[chunkCoordinates] = null;
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

				if (chunkColumn != null && !chunkColumn.isAllAir)
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
									x = startX,
									z = startZ,
									isAllAir = true
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