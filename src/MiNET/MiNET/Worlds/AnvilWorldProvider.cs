using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using fNbt;
using log4net;
using MiNET.Utils;

namespace MiNET.Worlds
{
	public class AnvilWorldProvider : IWorldProvider
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (AnvilWorldProvider));

		private List<int> _gaps;
		private List<int> _ignore;
		private byte _waterOffsetY;
		private FlatlandWorldProvider _flatland;
		private LevelInfo _level;
		private readonly ConcurrentDictionary<ChunkCoordinates, ChunkColumn> _chunkCache = new ConcurrentDictionary<ChunkCoordinates, ChunkColumn>();
		private string _basePath;

		public bool IsCaching { get; private set; }


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

			_waterOffsetY = (byte) Config.GetProperty("PCWaterOffset", 0);

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
			_ignore.Add(87);
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
		}

		public ChunkColumn GenerateChunkColumn(ChunkCoordinates chunkCoordinates)
		{
			lock (_chunkCache)
			{
				ChunkColumn cachedChunk;
				if (_chunkCache.TryGetValue(chunkCoordinates, out cachedChunk)) return cachedChunk;

				ChunkColumn chunk = GetChunk(chunkCoordinates);

				_chunkCache[chunkCoordinates] = chunk;

				return chunk;
			}
		}

		public ChunkColumn GetChunk(ChunkCoordinates coordinates)
		{
			int width = 32;
			int depth = 32;

			int rx = coordinates.X >> 5;
			int rz = coordinates.Z >> 5;

			string filePath = Path.Combine(_basePath, string.Format(@"region\r.{0}.{1}.mca", rx, rz));

			if (!File.Exists(filePath)) return _flatland.GenerateChunkColumn(coordinates);

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
					return _flatland.GenerateChunkColumn(coordinates);
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
								int yi = sy + y - _waterOffsetY;
								if (yi < 0 || yi >= 128) continue;

								int anvilIndex = y*16*16 + z*16 + x;
								int blockId = blocks[anvilIndex] + (Nibble4(adddata, anvilIndex) << 8);

								// Anvil to PE friendly converstion
								if (blockId == 125) blockId = 5;
								else if (blockId == 126) blockId = 158;
								else if (blockId == 75) blockId = 50;
								else if (blockId == 76) blockId = 50;
								else if (blockId == 123) blockId = 89;
								else if (blockId == 124) blockId = 89;
								else if (blockId == 152) blockId = 73;
								else if (_ignore.BinarySearch(blockId) >= 0) blockId = 0;
								else if (_gaps.BinarySearch(blockId) >= 0)
								{
									Debug.WriteLine("Missing material: " + blockId);
									blockId = 133;
								}

								if (blockId > 255) blockId = 41;

								if (yi == 127 && blockId != 0) blockId = 30;
								if (yi == 0 && (blockId == 8 || blockId == 9 || blockId == 0)) blockId = 7;

								//if (blockId != 0) blockId = 41;

								chunk.SetBlock(x, yi, z, (byte) blockId);
								chunk.SetMetadata(x, yi, z, Nibble4(data, anvilIndex));
								chunk.SetBlocklight(x, yi, z, Nibble4(blockLight, anvilIndex));
								chunk.SetSkylight(x, yi, z, Nibble4(skyLight, anvilIndex));
							}
						}
					}
				}

				NbtList entities = dataTag["Entities"] as NbtList;
				NbtList blockEntities = dataTag["TileEntities"] as NbtList;
				NbtList tileTicks = dataTag["TileTicks"] as NbtList;

				chunk.isDirty = false;
				return chunk;
			}
		}

		private byte Nibble4(byte[] arr, int index)
		{
			return (byte) (index%2 == 0 ? arr[index/2] & 0x0F : (arr[index/2] >> 4) & 0x0F);
		}

		private void SetNibble4(byte[] arr, int index, byte value)
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
			var spawnPoint = new Vector3(_level.SpawnX, _level.SpawnY, _level.SpawnZ);
			spawnPoint.Y += 2; // Compensate for point being at head
			spawnPoint.Y += _waterOffsetY; // Compensate for offset
			if (spawnPoint.Y > 127) spawnPoint.Y = 127;
			return spawnPoint;
		}

		public void SaveChunks()
		{
			lock (_chunkCache)
			{
				foreach (var chunkColumn in _chunkCache)
				{
					if (chunkColumn.Value.isDirty) SaveChunk(chunkColumn.Value);
				}
			}
		}

		private void SaveChunk(ChunkColumn chunk)
		{
			var coordinates = new ChunkCoordinates(chunk.x, chunk.z);

			int width = 32;
			int depth = 32;

			int rx = coordinates.X >> 5;
			int rz = coordinates.Z >> 5;

			string filePath = Path.Combine(_basePath, string.Format(@"region\r.{0}.{1}.mca", rx, rz));

			if (!File.Exists(filePath))
			{
				// Make sure directory exist
				Directory.CreateDirectory(Path.Combine(_basePath, "region"));

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
					offset = (int)regionFile.Position;

					regionFile.Seek(tableOffset, SeekOrigin.Begin);

					byte[] bytes = BitConverter.GetBytes(offset >> 4);
					Array.Reverse(bytes);
					regionFile.Write(bytes, 0, 3);
					regionFile.WriteByte(1);
				}

				regionFile.Seek(offset, SeekOrigin.Begin);
				byte[] waste = new byte[4];
				regionFile.Read(waste, 0, 4);
				int compressionMode = regionFile.ReadByte();

				// Write NBT
				NbtFile nbt = CreateNbtFromChunkColumn(chunk);
				nbt.SaveToStream(regionFile, NbtCompression.ZLib);
			}
		}

		private NbtFile CreateNbtFromChunkColumn(ChunkColumn chunk)
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

							int anvilIndex = (y + _waterOffsetY)*16*16 + z*16 + x;
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

			levelTag.Add(new NbtList("Entities", NbtTagType.Compound));
			levelTag.Add(new NbtList("TileEntities", NbtTagType.Compound));
			levelTag.Add(new NbtList("TileTicks", NbtTagType.Compound));

			return nbt;
		}

		public int NumberOfCachedChunks()
		{
			return _chunkCache.Count;
		}
	}

	public class LevelInfo
	{
		public LevelInfo()
		{
		}

		public LevelInfo(NbtTag dataTag)
		{
			LoadFromNbt(dataTag);
		}

		public int Version { get; set; }
		public bool Initialized { get; set; }
		public string LevelName { get; set; }
		public string GeneratorName { get; set; }
		public int GeneratorVersion { get; set; }
		public string GeneratorOptions { get; set; }
		public long RandomSeed { get; set; }
		public bool MapFeatures { get; set; }
		public long LastPlayed { get; set; }
		public bool AllowCommands { get; set; }
		public bool Hardcore { get; set; }
		public int GameType { get; set; }
		public long Time { get; set; }
		public long DayTime { get; set; }
		public int SpawnX { get; set; }
		public int SpawnY { get; set; }
		public int SpawnZ { get; set; }
		public bool Raining { get; set; }
		public int RainTime { get; set; }
		public bool Thundering { get; set; }
		public int ThunderTime { get; set; }

		public T GetPropertyValue<T>(NbtTag tag, Expression<Func<T>> property)
		{
			var propertyInfo = ((MemberExpression) property.Body).Member as PropertyInfo;
			if (propertyInfo == null)
			{
				throw new ArgumentException("The lambda expression 'property' should point to a valid Property");
			}

			NbtTag nbtTag = tag[propertyInfo.Name];
			if (nbtTag == null)
			{
				nbtTag = tag[LowercaseFirst(propertyInfo.Name)];
			}

			if (nbtTag == null) return default(T);

			var mex = property.Body as MemberExpression;
			var target = Expression.Lambda(mex.Expression).Compile().DynamicInvoke();

			switch (nbtTag.TagType)
			{
				case NbtTagType.Unknown:
					break;
				case NbtTagType.End:
					break;
				case NbtTagType.Byte:
					if (propertyInfo.PropertyType == typeof (bool)) propertyInfo.SetValue(target, nbtTag.ByteValue == 1);
					else propertyInfo.SetValue(target, nbtTag.ByteValue);
					break;
				case NbtTagType.Short:
					propertyInfo.SetValue(target, nbtTag.ShortValue);
					break;
				case NbtTagType.Int:
					if (propertyInfo.PropertyType == typeof (bool)) propertyInfo.SetValue(target, nbtTag.IntValue == 1);
					else propertyInfo.SetValue(target, nbtTag.IntValue);
					break;
				case NbtTagType.Long:
					propertyInfo.SetValue(target, nbtTag.LongValue);
					break;
				case NbtTagType.Float:
					propertyInfo.SetValue(target, nbtTag.FloatValue);
					break;
				case NbtTagType.Double:
					propertyInfo.SetValue(target, nbtTag.DoubleValue);
					break;
				case NbtTagType.ByteArray:
					propertyInfo.SetValue(target, nbtTag.ByteArrayValue);
					break;
				case NbtTagType.String:
					propertyInfo.SetValue(target, nbtTag.StringValue);
					break;
				case NbtTagType.List:
					break;
				case NbtTagType.Compound:
					break;
				case NbtTagType.IntArray:
					propertyInfo.SetValue(target, nbtTag.IntArrayValue);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			return (T) propertyInfo.GetValue(target);
		}

		public T SetPropertyValue<T>(NbtTag tag, Expression<Func<T>> property, bool upperFirst = true)
		{
			var propertyInfo = ((MemberExpression) property.Body).Member as PropertyInfo;
			if (propertyInfo == null)
			{
				throw new ArgumentException("The lambda expression 'property' should point to a valid Property");
			}

			NbtTag nbtTag = tag[propertyInfo.Name];
			if (nbtTag == null)
			{
				nbtTag = tag[LowercaseFirst(propertyInfo.Name)];
			}

			if (nbtTag == null) return default(T);

			var mex = property.Body as MemberExpression;
			var target = Expression.Lambda(mex.Expression).Compile().DynamicInvoke();

			switch (nbtTag.TagType)
			{
				case NbtTagType.Unknown:
					break;
				case NbtTagType.End:
					break;
				case NbtTagType.Byte:
					if (propertyInfo.PropertyType == typeof (bool))
						tag[nbtTag.Name] = new NbtByte((byte) ((bool) propertyInfo.GetValue(target) ? 1 : 0));
					else
						tag[nbtTag.Name] = new NbtByte((byte) propertyInfo.GetValue(target));
					break;
				case NbtTagType.Short:
					tag[nbtTag.Name] = new NbtShort((short) propertyInfo.GetValue(target));
					break;
				case NbtTagType.Int:
					if (propertyInfo.PropertyType == typeof (bool))
						tag[nbtTag.Name] = new NbtInt((bool) propertyInfo.GetValue(target) ? 1 : 0);
					else
						tag[nbtTag.Name] = new NbtInt((int) propertyInfo.GetValue(target));
					break;
				case NbtTagType.Long:
					tag[nbtTag.Name] = new NbtLong((long) propertyInfo.GetValue(target));
					break;
				case NbtTagType.Float:
					tag[nbtTag.Name] = new NbtFloat((float) propertyInfo.GetValue(target));
					break;
				case NbtTagType.Double:
					tag[nbtTag.Name] = new NbtDouble((double) propertyInfo.GetValue(target));
					break;
				case NbtTagType.ByteArray:
					tag[nbtTag.Name] = new NbtByteArray((byte[]) propertyInfo.GetValue(target));
					break;
				case NbtTagType.String:
					tag[nbtTag.Name] = new NbtString((string) propertyInfo.GetValue(target));
					break;
				case NbtTagType.List:
					break;
				case NbtTagType.Compound:
					break;
				case NbtTagType.IntArray:
					tag[nbtTag.Name] = new NbtIntArray((int[]) propertyInfo.GetValue(target));
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			return (T) propertyInfo.GetValue(target);
		}


		private static string LowercaseFirst(string s)
		{
			// Check for empty string.
			if (string.IsNullOrEmpty(s))
			{
				return string.Empty;
			}
			// Return char and concat substring.
			return char.ToLower(s[0]) + s.Substring(1);
		}

		public void LoadFromNbt(NbtTag dataTag)
		{
			GetPropertyValue(dataTag, () => Version);
			GetPropertyValue(dataTag, () => Initialized);
			GetPropertyValue(dataTag, () => LevelName);
			GetPropertyValue(dataTag, () => GeneratorName);
			GetPropertyValue(dataTag, () => GeneratorVersion);
			GetPropertyValue(dataTag, () => GeneratorOptions);
			GetPropertyValue(dataTag, () => RandomSeed);
			GetPropertyValue(dataTag, () => MapFeatures);
			GetPropertyValue(dataTag, () => LastPlayed);
			GetPropertyValue(dataTag, () => AllowCommands);
			GetPropertyValue(dataTag, () => Hardcore);
			GetPropertyValue(dataTag, () => GameType);
			GetPropertyValue(dataTag, () => Time);
			GetPropertyValue(dataTag, () => DayTime);
			GetPropertyValue(dataTag, () => SpawnX);
			GetPropertyValue(dataTag, () => SpawnY);
			GetPropertyValue(dataTag, () => SpawnZ);
			GetPropertyValue(dataTag, () => Raining);
			GetPropertyValue(dataTag, () => RainTime);
			GetPropertyValue(dataTag, () => Thundering);
			GetPropertyValue(dataTag, () => ThunderTime);
		}

		public void SaveToNbt(NbtTag dataTag)
		{
			SetPropertyValue(dataTag, () => Version);
			SetPropertyValue(dataTag, () => Initialized);
			SetPropertyValue(dataTag, () => LevelName);
			SetPropertyValue(dataTag, () => GeneratorName);
			SetPropertyValue(dataTag, () => GeneratorVersion);
			SetPropertyValue(dataTag, () => GeneratorOptions);
			SetPropertyValue(dataTag, () => RandomSeed);
			SetPropertyValue(dataTag, () => MapFeatures);
			SetPropertyValue(dataTag, () => LastPlayed);
			SetPropertyValue(dataTag, () => AllowCommands);
			SetPropertyValue(dataTag, () => Hardcore);
			SetPropertyValue(dataTag, () => GameType);
			SetPropertyValue(dataTag, () => Time);
			SetPropertyValue(dataTag, () => DayTime);
			SetPropertyValue(dataTag, () => SpawnX);
			SetPropertyValue(dataTag, () => SpawnY);
			SetPropertyValue(dataTag, () => SpawnZ);
			SetPropertyValue(dataTag, () => Raining);
			SetPropertyValue(dataTag, () => RainTime);
			SetPropertyValue(dataTag, () => Thundering);
			SetPropertyValue(dataTag, () => ThunderTime);
		}
	}
}