﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using fNbt;
using log4net;
using MiNET.BlockEntities;
using MiNET.Blocks;
using MiNET.Net;
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

	public class AnvilWorldProvider : IWorldProvider
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (AnvilWorldProvider));

		private static List<int> _gaps;
		private static List<int> _ignore;
		private static Dictionary<int, Tuple<int, Func<int, byte, byte>>> _convert;

		private FlatlandWorldProvider _flatland;
		private LevelInfo _level;
		public readonly ConcurrentDictionary<ChunkCoordinates, ChunkColumn> _chunkCache = new ConcurrentDictionary<ChunkCoordinates, ChunkColumn>();
		public readonly ConcurrentDictionary<ChunkCoordinates, McpeBatch> _batchCache = new ConcurrentDictionary<ChunkCoordinates, McpeBatch>();
		private string _basePath;

		public bool IsCaching { get; private set; }

		public byte WaterOffsetY { get; set; }


		public AnvilWorldProvider()
		{
			IsCaching = true;
			_flatland = new FlatlandWorldProvider();
		}

		public AnvilWorldProvider(string basePath) : this()
		{
			_basePath = basePath;
		}

		public void Initialize()
		{
			_basePath = _basePath ?? Config.GetProperty("PCWorldFolder", "World").Trim();

			NbtFile file = new NbtFile();
			file.LoadFromFile(Path.Combine(_basePath, "level.dat"));
			NbtTag dataTag = file.RootTag["Data"];
			_level = new LevelInfo(dataTag);

			WaterOffsetY = WaterOffsetY == 0 ? (byte) Config.GetProperty("PCWaterOffset", 0) : WaterOffsetY;

			_ignore = new List<int>();
			_ignore.Add(23);
			_ignore.Add(25);
			_ignore.Add(28);
			_ignore.Add(29);
			_ignore.Add(33);
			_ignore.Add(34);
			_ignore.Add(36);
			_ignore.Add(55);
			_ignore.Add(69);
			_ignore.Add(70);
			_ignore.Add(71);
			_ignore.Add(72);
//			_ignore.Add(75);
//			_ignore.Add(76);
			_ignore.Add(77);
			_ignore.Add(84);
			_ignore.Add(88);
			_ignore.Add(93);
			_ignore.Add(94);
			_ignore.Add(97);
			_ignore.Add(113);
			_ignore.Add(115);
			_ignore.Add(117);
			_ignore.Add(118);
//			_ignore.Add(123);
			_ignore.Add(131);
			_ignore.Add(132);
			_ignore.Add(138);
			_ignore.Add(140);
			_ignore.Add(143);
			_ignore.Add(144);
			_ignore.Add(145);
			_ignore.Sort();

			_gaps = new List<int>();
			_gaps.Add(23);
			_gaps.Add(25);
//			_gaps.Add(27);
			_gaps.Add(28);
			_gaps.Add(29);
			_gaps.Add(33);
			_gaps.Add(34);
			_gaps.Add(36);
			_gaps.Add(55);
//			_gaps.Add(66);
			_gaps.Add(69);
			_gaps.Add(70);
			_gaps.Add(72);
			_gaps.Add(75);
			_gaps.Add(76);
			_gaps.Add(77);
			_gaps.Add(84);
//			_gaps.Add(87);
			_gaps.Add(88);
			_gaps.Add(90);
			_gaps.Add(93);
			_gaps.Add(94);
			_gaps.Add(95);
			_gaps.Add(97);
//			_gaps.Add(99);
//			_gaps.Add(100);
//			_gaps.Add(106);
//			_gaps.Add(111);
			_gaps.Add(115);
			_gaps.Add(116);
			_gaps.Add(117);
			_gaps.Add(118);
			_gaps.Add(119);
//			_gaps.Add(120);
//			_gaps.Add(121);
			_gaps.Add(122);
			_gaps.Add(123);
			_gaps.Add(124);
			_gaps.Add(125);
			_gaps.Add(126);
//			_gaps.Add(127);
			_gaps.Add(130);
			_gaps.Add(131);
			_gaps.Add(132);
			_gaps.Add(137);
			_gaps.Add(138);
			_gaps.Add(140);
			_gaps.Add(143);
			_gaps.Add(144);
			_gaps.Add(145);
			_gaps.Add(146);
			_gaps.Add(147);
			_gaps.Add(148);
			_gaps.Add(149);
			_gaps.Add(150);
			_gaps.Add(151);
			_gaps.Add(152);
			_gaps.Add(153);
			_gaps.Add(154);
			_gaps.Add(160);
			_gaps.Add(165);
			_gaps.Add(166);
			_gaps.Add(167);
			_gaps.Add(168);
			_gaps.Add(169);
			_gaps.Sort();

			var air = new Mapper(0, (i, b) => 0);

			_convert = new Dictionary<int, Tuple<int, Func<int, byte, byte>>>
			{
				{25, new NoDataMapper(3)}, // Note Block		=> Dirt
				{27, new NoDataMapper(66)}, // Powered Rail		=> Rail
				{28, new NoDataMapper(66)}, // Detector Rail 	=> Rail
				{29, air}, // Sticky Piston	=> Air
				{33, air}, // Piston		=> Air
				{34, air}, // Piston Head		=> Air
				{55, air}, // Redstone Wire	=> Air
				{69, air}, // Lever		=> Air
				{70, air}, // Stone Pressure	=> Air
				{72, air}, // Wooden Pressure	=> Air
				{75, new NoDataMapper(50)}, // Redstone Torch O	=> Torch
				{76, new NoDataMapper(50)}, // Redstone Torch I	=> Torch
				{77, air}, // Stone Button		=> Air
				{84, new NoDataMapper(3)}, // Jukebox		=> Dirt
				{85, new Mapper(85, (i, b) => 0)}, // Fence		=> Fence
				{90, air}, // Nether Portal	=> Air
				{93, air}, // Red Repeater	O	=> Air
				{94, air}, // Red Repeater I	=> Air
				{95, new NoDataMapper(20)}, // Invisible bedrock	=> Air
				{97, new NoDataMapper(1)}, // Stone Monster Eg	=> Stone
				{113, new NoDataMapper(85)}, // Nether Fence		=> Fence
				{115, air}, // Nether Wart		=> Air
				{116, air}, // Enchant Table	=> Air
				{117, air}, // Brewing Stand	=> Air
				{118, air}, // Cauldron		=> Air
				{119, air}, // End Portal		=> Air
				{122, air}, // Dragon Egg		=> Air
				{123, new NoDataMapper(89)}, // Redstone Lamp O	=> Glowstone
				{124, new NoDataMapper(89)}, // Redstone Lamp I	=> Glowstone
				{125, new NoDataMapper(157)}, // 2x Wooden Slabs	=> (2x Wooden Slabs)
				{126, new NoDataMapper(158)}, // Wooden Slabs		=> (Wooden Slabs)
				{130, new NoDataMapper(54)}, // Ender Chest		=> Chest
				{131, air}, // Tripwire Hook	=> Air
				{132, air}, // Tripwire		=> Air
				{137, air}, // Command Block	=> Air
				{138, air}, // Beacon		=> Air
				{143, air}, // Wooden Button	=> Air
				{144, air}, // Mob Head		=> Air
				{145, air}, // Anvil		=> Air
				{146, new NoDataMapper(54)}, // Trapped Chest	=> Chest
				{147, air}, // Gold Pressure	=> Air
				{148, air}, // Iron Pressure	=> Air
				{149, air}, // Comparator O		=> Air
				{150, air}, // Comparator I		=> Air
				{151, air}, // Daylight Sensor	=> Air
				{152, new NoDataMapper(152)}, // Block of Redstone	=> Block of Redstone
				{153, new NoDataMapper(87)}, // Nether Quarts Ore 	=> Netherrack
				{154, air}, // Hopper		=> Air
				{157, new NoDataMapper(66)}, // Activator Rail	=> Rail
				{158, air}, // Dropper		=> Air
				{160, new NoDataMapper(102)}, // Stained Glass Pa	=> Glass Pane
				{161, new NoDataMapper(18)}, // Acacia Leaves	=> Leaves
				{162, new NoDataMapper(17)}, // Acacia Wood		=> Wood
				{165, air}, // Slime Block		=> Air
				{166, new NoDataMapper(97)}, // Barrier		=> (Invisible Bedrock)
				{167, new NoDataMapper(96)}, // Iron Trapdoor	=> Trapdoor
				{168, air}, // Prismarine		=> Air
				{169, new NoDataMapper(89)}, // Sea Lantern		=> Glowstone
				{183, new NoDataMapper(183)}, // Spruce Gate		=> Gate
				{184, new NoDataMapper(184)}, // Birch Gate		=> Gate
				{185, new NoDataMapper(185)}, // Jungle Gate		=> Gate
				{186, new NoDataMapper(186)}, // Dark Oak Gate	=> Gate
				{187, new NoDataMapper(187)}, // Acacia Gate		=> Gate
				{188, new Mapper(85, (i, b) => 1)}, // Spruce Fence		=> Fence
				{189, new Mapper(85, (i, b) => 2)}, // Birch Fence		=> Fence
				{190, new Mapper(85, (i, b) => 3)}, // Jungle Fence		=> Fence
				{191, new Mapper(85, (i, b) => 4)}, // Dark Oak Fence	=> Fence
				{192, new Mapper(85, (i, b) => 5)}, // Acacia Fence		=> Fence
			};
		}

		private int Noop(int blockId, int data)
		{
			return 0;
		}

		public ChunkColumn[] GetCachedChunks()
		{
			lock (_chunkCache)
			{
				return _chunkCache.Values.ToArray();
			}
		}

		public ChunkColumn GenerateChunkColumn(ChunkCoordinates chunkCoordinates)
		{
			lock (_chunkCache)
			{
				ChunkColumn cachedChunk;
				if (_chunkCache.TryGetValue(chunkCoordinates, out cachedChunk)) return cachedChunk;

				ChunkColumn chunk = GetChunk(chunkCoordinates, _basePath, _flatland, WaterOffsetY);

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

			string filePath = Path.Combine(basePath, string.Format(@"region\r.{0}.{1}.mca", rx, rz));

			if (!File.Exists(filePath)) return generator.GenerateChunkColumn(coordinates);

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
					return generator.GenerateChunkColumn(coordinates);
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
					biomeId = dataTag["Biomes"].ByteArrayValue
				};

				for (int i = 0; i < chunk.biomeId.Length; i++)
				{
					if (chunk.biomeId[i] > 22) chunk.biomeId[i] = 0;
				}
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

								Func<int, byte, byte> dataConverter = (i, b) => b;
								// Anvil to PE friendly converstion
								if (_convert.ContainsKey(blockId))
								{
									dataConverter = _convert[blockId].Item2;
									blockId = _convert[blockId].Item1;
								}
								else if (_ignore.BinarySearch(blockId) >= 0) blockId = 0;
								else if (_gaps.BinarySearch(blockId) >= 0)
								{
									Log.WarnFormat("Missing material on convert: {0}", blockId);
									blockId = 133;
								}

								if (blockId > 255) blockId = 41;

								if (yi == 127 && blockId != 0) blockId = 30;
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
									chunk.SetBlock(x, yi, z, 198);
									chunk.SetMetadata(x, yi, z, 0);
								}
								else if (blockId == 3 && chunk.GetMetadata(x, yi, z) == 2)
								{
									chunk.SetBlock(x, yi, z, 143); //Coarse Dirt => Pat
									chunk.SetMetadata(x, yi, z, 0); // Podzol => (Podzol)
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
						var blockEntityTag = (NbtCompound) nbtTag;
						string entityId = blockEntityTag["id"].StringValue;
						int x = blockEntityTag["x"].IntValue;
						int y = blockEntityTag["y"].IntValue - yoffset;
						int z = blockEntityTag["z"].IntValue;
						blockEntityTag["y"] = new NbtInt("y", y);

						BlockEntity blockEntity = BlockEntityFactory.GetBlockEntityById(entityId);
						if (blockEntity != null)
						{
							blockEntityTag.Name = string.Empty;
							chunk.SetBlockEntity(new BlockCoordinates(x, y, z), blockEntityTag);
						}
					}
				}

				NbtList tileTicks = dataTag["TileTicks"] as NbtList;

				chunk.isDirty = false;
				return chunk;
			}
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
			var spawnPoint = new Vector3(_level.SpawnX, _level.SpawnY + 2 /* + WaterOffsetY*/, _level.SpawnZ);

			if (spawnPoint.Y > 127) spawnPoint.Y = 127;

			return spawnPoint;
		}

		public long GetTime()
		{
			return _level.Time;
		}

		public void SaveChunks()
		{
			lock (_chunkCache)
			{
				foreach (var chunkColumn in _chunkCache)
				{
					if (chunkColumn.Value.isDirty) SaveChunk(chunkColumn.Value, _basePath, WaterOffsetY);
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

			string filePath = Path.Combine(basePath, string.Format(@"region\r.{0}.{1}.mca", rx, rz));

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
	}
}