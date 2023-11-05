﻿using System;
using System.Collections;
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

namespace MiNET.Worlds.Anvil
{

	public class AnvilWorldProvider : IWorldProvider, ICachingWorldProvider, ICloneable
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(AnvilWorldProvider));

		private static readonly int WaterBlockRuntimeId = new Water().GetRuntimeId();
		private static readonly int SnowLayerBlockRuntimeId = new SnowLayer().GetRuntimeId();

		public IWorldGenerator MissingChunkProvider { get; set; }

		public AnvilBiomeManager BiomeManager { get; private set; }

		public LevelInfo LevelInfo { get; private set; }

		public ConcurrentDictionary<ChunkCoordinates, ChunkColumn> _chunkCache = new ConcurrentDictionary<ChunkCoordinates, ChunkColumn>();

		public string BasePath { get; private set; }

		public Dimension Dimension { get; set; }

		public bool IsCaching { get; private set; } = true;

		public bool IsDimensionWithSkyLight { get; set; } = true;

		public bool ReadSkyLight { get; set; } = true;

		public bool ReadBlockLight { get; set; } = true;

		public bool Locked { get; set; } = false;

		public AnvilWorldProvider()
		{
			BiomeManager = new AnvilBiomeManager(this);
		}

		public AnvilWorldProvider(string basePath) : this()
		{
			BasePath = basePath;
		}

		protected AnvilWorldProvider(string basePath, LevelInfo levelInfo, ConcurrentDictionary<ChunkCoordinates, ChunkColumn> chunkCache) : this()
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
			if (_isInitialized)
				return; // Quick exit

			lock (_initializeSync)
			{
				if (_isInitialized)
					return;

				BasePath ??= Config.GetProperty("PCWorldFolder", "World").Trim();

				var file = new NbtFile();
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
				var coords = new List<ChunkCoordinates> { spawn };

				foreach (var player in players)
				{
					var chunkCoordinates = new ChunkCoordinates(player.KnownPosition);
					if (!coords.Contains(chunkCoordinates))
						coords.Add(chunkCoordinates);
				}

				bool save = Config.GetProperty("Save.Enabled", false);

				Parallel.ForEach(_chunkCache, (Action<KeyValuePair<ChunkCoordinates, ChunkColumn>>) ((chunkColumn) =>
				{
					bool keep = coords.Exists(c => c.DistanceTo(chunkColumn.Key) < maxViewDistance);
					if (!keep)
					{
						_chunkCache.TryRemove(chunkColumn.Key, out ChunkColumn waste);
						if (save && waste.NeedSave)
							SaveChunk(waste, BasePath);

						if (waste != null)
							foreach (var chunk in waste)
								chunk.Dispose();

						Interlocked.Increment(ref removed);
					}
				}));
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
				if (value != null)
				{
					return value;
				}

				_chunkCache.TryRemove(chunkCoordinates, out value);
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
					return GenerateEmptyChunkColumn(coordinates, generator);

				using (var regionFile = File.OpenRead(filePath))
				{
					var buffer = new byte[8192];

					regionFile.Read(buffer, 0, 8192);

					var xi = coordinates.X % width;
					if (xi < 0)
						xi += 32;
					var zi = coordinates.Z % depth;
					if (zi < 0)
						zi += 32;
					var tableOffset = (xi + zi * width) * 4;

					regionFile.Seek(tableOffset, SeekOrigin.Begin);

					var offsetBuffer = new byte[4];
					regionFile.Read(offsetBuffer, 0, 3);
					Array.Reverse(offsetBuffer);
					var offset = BitConverter.ToInt32(offsetBuffer, 0) << 4;

					var bytes = BitConverter.GetBytes(offset >> 4);
					Array.Reverse(bytes);
					if (offset != 0 && offsetBuffer[0] != bytes[0] && offsetBuffer[1] != bytes[1] && offsetBuffer[2] != bytes[2])
						throw new Exception($"Not the same buffer\n{Packet.HexDump(offsetBuffer)}\n{Packet.HexDump(bytes)}");

					var length = regionFile.ReadByte();

					if (offset == 0 || length == 0)
						return GenerateEmptyChunkColumn(coordinates, generator);

					regionFile.Seek(offset, SeekOrigin.Begin);
					var waste = new byte[4];
					regionFile.Read(waste, 0, 4);
					var compressionMode = regionFile.ReadByte();

					if (compressionMode != 0x02)
						throw new Exception($"CX={coordinates.X}, CZ={coordinates.Z}, NBT wrong compression. Expected 0x02, got 0x{compressionMode:X2}. " +
											$"Offset={offset}, length={length}\n{Packet.HexDump(waste)}");

					var nbt = new NbtFile();
					nbt.LoadFromStream(regionFile, NbtCompression.ZLib);

					var dataTag = (NbtCompound) nbt.RootTag;

					var isPocketEdition = false; //obsolete
					if (dataTag.Contains("MCPE BID"))
						isPocketEdition = dataTag["MCPE BID"].ByteValue == 1;

					var chunk = new ChunkColumn((x, z, i) => new AnvilSubChunk(BiomeManager, x, z, i))
					{
						X = coordinates.X,
						Z = coordinates.Z,
						IsAllAir = true,
						IsDirty = false,
						NeedSave = false
					};

					ReadHeights(dataTag, chunk);
					ReadSections(dataTag, chunk);
					ReadEntities(dataTag, chunk);
					ReadBlockEntites(dataTag, chunk);

					//NbtList tileTicks = dataTag["TileTicks"] as NbtList;

					//if (Dimension == Dimension.Overworld && Config.GetProperty("CalculateLights", false))
					//{
					//	chunk.RecalcHeight();

					//	SkyLightBlockAccess blockAccess = new SkyLightBlockAccess(this, chunk);
					//	new SkyLightCalculations().RecalcSkyLight(chunk, blockAccess);
					//	//TODO: Block lights.
					//}

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

		private ChunkColumn GenerateEmptyChunkColumn(ChunkCoordinates coordinates, IWorldGenerator generator)
		{
			var chunkColumn = generator?.GenerateChunkColumn(coordinates);
			if (chunkColumn != null)
			{
				if (Dimension == Dimension.Overworld && Config.GetProperty("CalculateLights", false))
				{
					var blockAccess = new SkyLightBlockAccess(this, chunkColumn);
					new SkyLightCalculations().RecalcSkyLight(chunkColumn, blockAccess);
				}

				chunkColumn.IsDirty = false;
				chunkColumn.NeedSave = false;
			}

			return chunkColumn;
		}

		private void ReadHeights(NbtTag dataTag, ChunkColumn chunk)
		{
			var heights = dataTag["Heightmaps"] as NbtCompound;
			if (heights != null)
			{
				var worldSurface = heights["WORLD_SURFACE"];
				if (worldSurface != null)
				{
					var longHeights = worldSurface.LongArrayValue;
					ReadAnyBitLengthShortFromLongs(longHeights, chunk.height, 9);
				}
			}
		}

		private void ReadEntities(NbtTag dataTag, ChunkColumn chunk)
		{
			var entities = dataTag["Entities"] as NbtList;
		}

		private void ReadSections(NbtTag dataTag, ChunkColumn chunk)
		{
			var sections = dataTag["sections"] as NbtList;

			foreach (NbtTag sectionTag in sections)
			{
				ReadSection(sectionTag, chunk);
			}
		}

		private void ReadSection(NbtTag sectionTag, ChunkColumn chunkColumn)
		{
			var sectionIndex = (sbyte) sectionTag["Y"].ByteValue;

			// Y can be up to -4, but the array index starts from 0
			var subChunkId = 4 + sectionIndex;

			if (subChunkId < 0 || subChunkId >= ChunkColumn.WorldHeight << 4)
			{
				return;
			}

			var subChunk = (AnvilSubChunk) chunkColumn[subChunkId];

			ReadBlockStates(sectionTag, chunkColumn, subChunk);
			ReadBiomes(sectionTag, subChunk);
			ReadBlockLigths(sectionTag, subChunk);
			ReadSkyLigths(sectionTag, subChunk);
		}

		private void ReadBlockStates(NbtTag sectionTag, ChunkColumn chunkColumn, AnvilSubChunk subChunk)
		{
			var blockStatesTag = sectionTag["block_states"] as NbtCompound;
			var palette = blockStatesTag["palette"] as NbtList;

			var runtimeIds = new List<int>(palette.Count);
			var blockEntities = new List<BlockEntity>(palette.Count);
			var waterloggedIds = new List<int>(palette.Count);
			var snowyIds = new List<int>(palette.Count);

			foreach (NbtCompound p in palette)
			{
				var id = AnvilPaletteConverter.GetRuntimeIdByPalette(p, out var blockEntity);

				waterloggedIds.Add(
					p["Properties"]?["waterlogged"]?.StringValue == "true"
					|| AnvilPaletteConverter.IsSeaBlock(BlockFactory.GetIdByRuntimeId(id))
						? id : -1);

				//snowyIds.Add(
				//	p["Properties"]?["snowy"]?.StringValue == "true" 
				//		? id : -1);

				blockEntities.Add(blockEntity);
				runtimeIds.Add(id);
			}

			var waterRuntimeId = runtimeIds.IndexOf(WaterBlockRuntimeId);
			var snowRuntimeId = runtimeIds.IndexOf(SnowLayerBlockRuntimeId);
			byte waterChunkId = 0;
			byte snowChunkId = 0;

			subChunk.RuntimeIds.Clear();
			subChunk.RuntimeIds.AddRange(runtimeIds);

			var namedRuntimeIds = runtimeIds.Select(id => BlockFactory.GetIdByRuntimeId((short) id)).ToList();

			if (waterloggedIds.Any(id => id >= 0))
			{
				waterChunkId = (byte) subChunk.LoggedRuntimeIds.Count;
				subChunk.LoggedRuntimeIds.Add(WaterBlockRuntimeId);
			}
			if (snowyIds.Any(id => id >= 0))
			{
				snowChunkId = (byte) subChunk.LoggedRuntimeIds.Count;
				subChunk.LoggedRuntimeIds.Add(SnowLayerBlockRuntimeId);
			}

			if (runtimeIds.Count == 1)
			{
				var block = BlockFactory.GetBlockByRuntimeId(runtimeIds.Single());
				chunkColumn.IsAllAir &= block is Air;

				return;
			}

			var data = blockStatesTag["data"].LongArrayValue;

			var bitsPerBlock = (byte) Math.Max(4, Math.Ceiling(Math.Log(runtimeIds.Count, 2)));

			var blocks = subChunk.Blocks;

			ReadAnyBitLengthShortFromLongs(data, blocks, bitsPerBlock);

			for (var i = 0; i < blocks.Length; i++)
			{
				var y = i >> 8;
				var x = i & 0xf;
				var z = i >> 4 & 0xf;
				var j = x << 8 | z << 4 | y;

				if (y <= x)
				{
					var iBlock = blocks[i];
					var jBlock = blocks[i] = blocks[j];
					blocks[j] = iBlock;

					if (waterloggedIds[iBlock] >= 0)
					{
						subChunk.LoggedBlocks[j] = waterChunkId;
					}
					if (waterloggedIds[jBlock] >= 0)
					{
						subChunk.LoggedBlocks[i] = waterChunkId;
					}
					//if (snowyIds[iBlock] >= 0)
					//{
					//	subChunk.LoggedBlocks[j] = snowChunkId;
					//}
					//if (snowyIds[jBlock] >= 0)
					//{
					//	subChunk.LoggedBlocks[i] = snowChunkId;
					//}
				}

				if (blockEntities[blocks[j]] != null)
				{
					var template = blockEntities[blocks[j]];
					template.Coordinates = new BlockCoordinates(
						(subChunk.X << 4) | x,
						((subChunk.Index << 4) + ChunkColumn.WorldMinY) | y,
						(subChunk.Z << 4) | z);

					chunkColumn.SetBlockEntity(template.Coordinates, template.GetCompound());
				}
			}

			chunkColumn.IsAllAir = false;
		}

		private void ReadBlockLigths(NbtTag sectionTag, AnvilSubChunk subChunk)
		{
			if (!ReadBlockLight) return;

			var blockLight = sectionTag["BlockLight"]?.ByteArrayValue;

			if (blockLight == null) return;

			Array.Copy(blockLight, subChunk.BlockLight.Data, 0);
		}

		private void ReadSkyLigths(NbtTag sectionTag, AnvilSubChunk subChunk)
		{
			if (!ReadSkyLight) return;

			var skyLight = sectionTag["SkyLight"]?.ByteArrayValue;

			if (skyLight == null) return;

			Array.Copy(skyLight, subChunk.SkyLight.Data, 0);
		}

		private void ReadBiomes(NbtTag sectionTag, AnvilSubChunk subChunk)
		{
			var biomesTag = sectionTag["biomes"];
			var palette = biomesTag["palette"] as NbtList;

			var usingBiomes = palette.Select(p => AnvilPaletteConverter.GetBiomeByName(p.StringValue)).ToArray();

			subChunk.BiomeIds.Clear();
			subChunk.BiomeIds.AddRange(usingBiomes.Select(biome => biome.Id));

			if (usingBiomes.Length == 1)
			{
				return;
			}

			var biomesNoise = new byte[64];

			var data = biomesTag["data"].LongArrayValue;

			var bitsPerBlock = (byte) Math.Ceiling(Math.Log(usingBiomes.Length, 2));

			var sectionBiomesMap = new byte[64];
			ReadAnyBitLengthShortFromLongs(data, sectionBiomesMap, bitsPerBlock);

			subChunk.SetBiomesNoise(sectionBiomesMap);
		}

		private void ReadBlockEntites(NbtCompound dataTag, ChunkColumn chunk)
		{
			var blockEntities = dataTag["block_entities"] as NbtList;
			if (blockEntities != null)
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
						if (entityId == "Ender_chest") entityId = "EnderChest";
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
							if (Log.IsDebugEnabled)
								Log.Debug($"Loaded sign block entity\n{blockEntityTag}");
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

							var items = (NbtList) blockEntityTag["Items"];

							if (items != null)
								for (byte i = 0; i < items.Count; i++)
								{
									var sourceTag = (NbtCompound) items[i];

									string itemName = sourceTag["id"].StringValue;

									var item = ItemFactory.GetItem(itemName);
									var itemTag = item.ToNbt();
									itemTag.Remove("Count");

									sourceTag.Remove("id");
									sourceTag.AddRange(itemTag);
								}
						}
						else
						{
							if (Log.IsDebugEnabled) Log.Debug($"Loaded block entity\n{blockEntityTag}");
							// TODO - 1.20 - update
							//blockEntity.SetCompound(blockEntityTag);
							//blockEntityTag = blockEntity.GetCompound();
						}

						var coordinates = new BlockCoordinates(x, y, z);
						var existingBlockEntity = chunk.GetBlockEntity(coordinates);
						if (chunk.GetBlockEntity(coordinates) == null)
						{
							chunk.SetBlockEntity(coordinates, blockEntityTag);
						}
						else
						{
							existingBlockEntity.AddRange(blockEntityTag.ExceptBy(existingBlockEntity.Select(tag => tag.Name), tag => tag.Name).Select(tag => (NbtTag) tag.Clone()));
						}
					}
					else
						if (Log.IsDebugEnabled)
							Log.Debug($"Loaded unknown block entity\n{blockEntityTag}");
				}
		}

		private static Regex _regex = new Regex(@"^((\{""extra"":\[)?)(\{""text"":"".*?""})(],)?(""text"":"".*?""})?$");

		private static void CleanSignText(NbtCompound blockEntityTag, string tagName)
		{
			// TODO - ?
			//var text = blockEntityTag[tagName].StringValue;
			//var replace = /*Regex.Unescape*/(_regex.Replace(text, "$3"));
			//blockEntityTag[tagName] = new NbtString(tagName, replace);
		}

		private static byte Nibble4(byte[] arr, int index)
		{
			return (byte) (arr[index >> 1] >> (index & 1) * 4 & 0xF);
		}

		private static void SetNibble4(byte[] arr, int index, byte value)
		{
			value &= 0xF;
			var idx = index >> 1;
			arr[idx] &= (byte) (0xF << (index + 1 & 1) * 4);
			arr[idx] |= (byte) (value << (index & 1) * 4);
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

			if (spawnPoint.Y > ChunkColumn.WorldMaxY)
			{
				spawnPoint.Y = ChunkColumn.WorldMaxY - 1;
			}

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
			if (Dimension != Dimension.Overworld)
				return;

			level.LastPlayed = DateTimeOffset.Now.ToUnixTimeMilliseconds();
			var leveldat = Path.Combine(BasePath, "level.dat");

			if (!Directory.Exists(BasePath))
				Directory.CreateDirectory(BasePath);
			else if (File.Exists(leveldat))
				return; // What if this is changed? Need a dirty flag on this

			if (LevelInfo.SpawnY <= 0)
				LevelInfo.SpawnY = 256;

			var file = new NbtFile();
			NbtTag dataTag = new NbtCompound("Data");
			var rootTag = (NbtCompound) file.RootTag;
			rootTag.Add(dataTag);
			level.SaveToNbt(dataTag);
			file.SaveToFile(leveldat, NbtCompression.GZip);
		}

		public int SaveChunks()
		{
			if (!Config.GetProperty("Save.Enabled", false))
				return 0;

			int count = 0;
			try
			{
				lock (_chunkCache)
				{
					if (Dimension == Dimension.Overworld)
						SaveLevelInfo(LevelInfo);

					var regions = new Dictionary<Tuple<int, int>, List<ChunkColumn>>();
					foreach (var chunkColumn in _chunkCache.OrderBy(pair => pair.Key.X >> 5).ThenBy(pair => pair.Key.Z >> 5))
					{
						var regionKey = new Tuple<int, int>(chunkColumn.Key.X >> 5, chunkColumn.Key.Z >> 5);
						if (!regions.ContainsKey(regionKey))
							regions.Add(regionKey, new List<ChunkColumn>());

						regions[regionKey].Add(chunkColumn.Value);
					}

					var tasks = new List<Task>();
					foreach (var region in regions.OrderBy(pair => pair.Key.Item1).ThenBy(pair => pair.Key.Item2))
					{
						var task = new Task(delegate
						{
							List<ChunkColumn> chunks = region.Value;
							foreach (var chunkColumn in chunks)
								if (chunkColumn != null && chunkColumn.NeedSave)
								{
									SaveChunk(chunkColumn, BasePath);
									count++;
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

				int xi = coordinates.X % width;
				if (xi < 0)
					xi += 32;
				int zi = coordinates.Z % depth;
				if (zi < 0)
					zi += 32;
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
				if (padding.Length > 0)
					regionFile.Write(padding, 0, padding.Length);

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

			// TODO - 1.20 - update
			//levelTag.Add(new NbtByteArray("Biomes", chunk.biomeId));

			var sectionsTag = new NbtList("Sections", NbtTagType.Compound);
			levelTag.Add(sectionsTag);

			for (int i = 0; i < 16; i++)
			{
				SubChunk subChunk = chunk[i];
				if (subChunk.IsAllAir())
					// OLD GENERATION FORMAT IS INVALID
					//if (i == 0) Log.Debug($"All air bottom chunk? {subChunk.GetBlockId(0,0,0)}");
					continue;

				var sectionTag = new NbtCompound();
				sectionsTag.Add(sectionTag);
				sectionTag.Add(new NbtByte("Y", (byte) i));

				var blocks = new byte[4096];
				var data = new byte[2048];
				var blockLight = new byte[2048];
				var skyLight = new byte[2048];

				{
					for (int x = 0; x < 16; x++)
						for (int z = 0; z < 16; z++)
							for (int y = 0; y < 16; y++)
							{
								// OLD GENERATION FORMAT IS INVALID
								int anvilIndex = y * 16 * 16 + z * 16 + x;
								byte blockId =  0; // subChunk.GetBlockId(x, y, z);
								blocks[anvilIndex] = blockId;
								//SetNibble4(data, anvilIndex, section.GetMetadata(x, y, z));
								SetNibble4(blockLight, anvilIndex, subChunk.GetBlocklight(x, y, z));
								SetNibble4(skyLight, anvilIndex, subChunk.GetSkylight(x, y, z));
							}
				}
				sectionTag.Add(new NbtByteArray("Blocks", blocks));
				sectionTag.Add(new NbtByteArray("Data", data));
				sectionTag.Add(new NbtByteArray("BlockLight", blockLight));
				sectionTag.Add(new NbtByteArray("SkyLight", skyLight));
			}

			var heights = new int[256];
			for (int h = 0; h < heights.Length; h++)
				heights[h] = chunk.height[h];
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
			var chunkCache = new ConcurrentDictionary<ChunkCoordinates, ChunkColumn>();
			foreach (KeyValuePair<ChunkCoordinates, ChunkColumn> valuePair in _chunkCache)
				chunkCache.TryAdd(valuePair.Key, (ChunkColumn) valuePair.Value?.Clone());

			var provider = new AnvilWorldProvider(BasePath, (LevelInfo) LevelInfo.Clone(), chunkCache);
			return provider;
		}

		public int PruneAir()
		{
			int prunedChunks = 0;
			var sw = new Stopwatch();
			sw.Start();

			foreach (KeyValuePair<ChunkCoordinates, ChunkColumn> valuePair in _chunkCache)
			{
				ChunkCoordinates chunkCoordinates = valuePair.Key;
				ChunkColumn chunkColumn = valuePair.Value;

				if (chunkColumn != null && chunkColumn.IsAllAir)
				{
					bool surroundingIsAir = true;

					for (int startX = chunkCoordinates.X - 1; startX <= chunkCoordinates.X + 1; startX++)
						for (int startZ = chunkCoordinates.Z - 1; startZ <= chunkCoordinates.Z + 1; startZ++)
						{
							var surroundingChunkCoordinates = new ChunkCoordinates(startX, startZ);

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

					if (surroundingIsAir)
					{
						_chunkCache.TryGetValue(chunkCoordinates, out var chunk);
						_chunkCache[chunkCoordinates] = null;
						if (chunk != null)
							foreach (var c in chunk)
								c.Dispose();
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
			var sw = new Stopwatch();
			sw.Start();

			foreach (KeyValuePair<ChunkCoordinates, ChunkColumn> valuePair in _chunkCache)
			{
				ChunkCoordinates chunkCoordinates = valuePair.Key;
				ChunkColumn chunkColumn = valuePair.Value;

				if (chunkColumn != null && !chunkColumn.IsAllAir)
					for (int startX = chunkCoordinates.X - 1; startX <= chunkCoordinates.X + 1; startX++)
						for (int startZ = chunkCoordinates.Z - 1; startZ <= chunkCoordinates.Z + 1; startZ++)
						{
							var surroundingChunkCoordinates = new ChunkCoordinates(startX, startZ);

							if (surroundingChunkCoordinates.Equals(chunkCoordinates))
								continue;

							ChunkColumn surroundingChunkColumn;

							_chunkCache.TryGetValue(surroundingChunkCoordinates, out surroundingChunkColumn);

							if (surroundingChunkColumn == null)
							{
								var airColumn = new ChunkColumn
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

			sw.Stop();
			Log.Info("Created " + createdChunks + " air chunks in " + sw.ElapsedMilliseconds + "ms");
			return createdChunks;
		}

		private void ReadAnyBitLengthShortFromLongs(long[] longs, short[] shorts, byte shortSize)
		{
			var longBitSize = sizeof(long) * 8;
			var valueBits = (1 << shortSize) - 1;

			var shortsInLongCount = longBitSize / shortSize;

			for (var i = 0; i < shorts.Length; i++)
			{
				var offset = i % shortsInLongCount * shortSize;
				var longsOffset = i / shortsInLongCount;

				shorts[i] = (short) (longs[longsOffset] >> offset & valueBits);
			}
		}

		private void ReadAnyBitLengthShortFromLongs(long[] longs, byte[] shorts, byte shortSize)
		{
			var longBitSize = sizeof(long) * 8;
			var valueBits = (1 << shortSize) - 1;

			var shortsInLongCount = longBitSize / shortSize;

			for (var i = 0; i < shorts.Length; i++)
			{
				var offset = i % shortsInLongCount * shortSize;
				var longsOffset = i / shortsInLongCount;

				shorts[i] = (byte) (longs[longsOffset] >> offset & valueBits);
			}
		}
	}
}
