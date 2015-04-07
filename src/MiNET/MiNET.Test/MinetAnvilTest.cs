using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using fNbt;
using MiNET.Utils;
using MiNET.Worlds;
using NUnit.Framework;

namespace MiNET
{
	[TestFixture]
	public class MinetAnvilTest
	{
		[Test]
		public void OffsetIntTest()
		{
			int original = (4096*2) - 10;

			int reminder;
			Math.DivRem(original, 4096, out reminder);
			//Assert.AreEqual(10, reminder);

			Assert.AreEqual((4096*2), original + (4096 - reminder));

			//byte[] expected = {0x00, 0x00, 0x01, 0x00};

			//byte[] offsetBuffer = {0x00, 0x00, 0x01, 0x00};
			//Array.Reverse(offsetBuffer);
			//int offset = BitConverter.ToInt32(offsetBuffer, 0) << 4;
			//Assert.AreEqual(4096, offset);

			//byte[] bytes = BitConverter.GetBytes(offset >> 4);
			//Array.Reverse(bytes);
			//Assert.AreEqual(expected, bytes);

//			Assert.AreEqual(offset, BitConverter.ToInt32(bytes, 0) << 4);
		}

		[Test]
		public void SaveAnvilChunkTest()
		{
			int width = 32;
			int depth = 32;

			int regionX = 5;
			int regionZ = 24;

			AnvilWorldProvider anvil = new AnvilWorldProvider(@"D:\Development\Worlds\KingsLanding\");
			anvil.Initialize();
			for (int x = 0; x < 32; x++)
			{
				for (int z = 0; z < 32; z++)
				{
					int cx = (width*regionX) + x;
					int cz = (depth*regionZ) + z;

					ChunkCoordinates coordinates = new ChunkCoordinates(cx, cz);
					ChunkColumn chunk = anvil.GenerateChunkColumn(coordinates);
					Assert.NotNull(chunk);
				}
			}

			Stopwatch sw = new Stopwatch();
			sw.Start();

			anvil.SaveChunks();

			Console.WriteLine("Saved {0} chunks in {1}ms", anvil.NumberOfCachedChunks(), sw.ElapsedMilliseconds);


			for (int x = 0; x < 32; x++)
			{
				for (int z = 0; z < 32; z++)
				{
					int cx = (width*regionX) + x;
					int cz = (depth*regionZ) + z;

					ChunkCoordinates coordinates = new ChunkCoordinates(cx, cz);
					anvil.GenerateChunkColumn(coordinates);
				}
			}
		}

		[Test, Ignore]
		public void SaveOneAnvilChunkTest()
		{
			int width = 32;
			int depth = 32;

			int cx = (width*4) + 3;
			int cz = (depth*25) + 0;

			AnvilWorldProvider anvil = new AnvilWorldProvider(@"D:\Development\Worlds\KingsLanding\");
			anvil.Initialize();

			ChunkCoordinates coordinates = new ChunkCoordinates(cx, cz);
			ChunkColumn chunk = anvil.GenerateChunkColumn(coordinates);
			Assert.NotNull(chunk);

			Stopwatch sw = new Stopwatch();
			sw.Start();

			anvil.SaveChunks();

			Assert.Less(sw.ElapsedMilliseconds, 1);
		}

		[Test, Ignore]
		public void LoadAnvilLevelLoadTest()
		{
			NbtFile file = new NbtFile();
			file.LoadFromFile(@"D:\Downloads\KingsLanding1\KingsLanding1\level.dat");
			NbtTag dataTag = file.RootTag["Data"];
			Assert.NotNull(dataTag);

			Assert.NotNull(dataTag["version"]);
			Assert.AreEqual(19133, dataTag["version"].IntValue);
			Assert.NotNull(dataTag["initialized"]);

			var level = new LevelInfo();

			level.GetPropertyValue(dataTag, () => level.Version);
			Assert.AreEqual(19133, level.Version);
			Assert.AreEqual(19133, level.GetPropertyValue(dataTag, () => level.Version));

			Assert.AreEqual(true, level.GetPropertyValue(dataTag, () => level.Initialized));
			Assert.AreEqual("WesterosCraft", level.GetPropertyValue(dataTag, () => level.LevelName));

			var levelFromNbt = new LevelInfo(dataTag);
			Assert.AreEqual(19133, levelFromNbt.Version);
			Assert.AreEqual(true, levelFromNbt.Initialized);
			Assert.AreEqual("WesterosCraft", levelFromNbt.LevelName);
		}

		[Test, Ignore]
		public void LoadAnvilChunkLoadTest()
		{
			Stopwatch sw = new Stopwatch();
			sw.Start();

			int width = 32;
			int depth = 32;

			int cx = (width*4) + 3;
			int cz = (depth*25) + 0;

			ChunkCoordinates coordinates = new ChunkCoordinates(cx, cz);

			int rx = coordinates.X >> 5;
			int rz = coordinates.Z >> 5;

			Assert.AreEqual(4, rx);
			Assert.AreEqual(25, rz);

			var regionFile = File.OpenRead(string.Format(@"D:\Downloads\KingsLanding1\KingsLanding1\region\r.{0}.{1}.mca", rx, rz));

			byte[] buffer = new byte[8192];
			regionFile.Read(buffer, 0, 8192);


			//for (int x = 0; x < 32; x++)
			//{
			//	for (int z = 0; z < 32; z++)
			//	{
			//		Print(regionFile, x, z);
			//	}
			//}

			int tableOffset = ((coordinates.X%width) + (coordinates.Z%depth)*width)*4;

			regionFile.Seek(tableOffset, SeekOrigin.Begin);
			byte[] offsetBuffer = new byte[4];
			regionFile.Read(offsetBuffer, 0, 3);
			Array.Reverse(offsetBuffer);
			int offset = BitConverter.ToInt32(offsetBuffer, 0) << 4;

			int length = regionFile.ReadByte();

			if (offset == 0 || length == 0)
				throw new Exception();

			Assert.AreEqual(229376, offset);
			Assert.AreEqual(1, length);
			Assert.AreEqual(4096, length*4096);

			regionFile.Seek(offset, SeekOrigin.Begin);
			byte[] waste = new byte[4];
			regionFile.Read(waste, 0, 4);
			int compressionMode = regionFile.ReadByte();
			Assert.AreEqual(2, compressionMode);

			var nbt = new NbtFile();
			nbt.LoadFromStream(regionFile, NbtCompression.ZLib);

			NbtTag dataTag = nbt.RootTag["Level"];
			Assert.NotNull(dataTag);

			Assert.NotNull(dataTag["xPos"]);
			Assert.AreEqual(131, dataTag["xPos"].IntValue);
			Assert.AreEqual(800, dataTag["zPos"].IntValue);
			Assert.AreEqual(700448540, dataTag["LastUpdate"].LongValue);
			//Assert.AreEqual(800, dataTag["LightPopulated"].ByteValue);
			Assert.AreEqual(1, dataTag["TerrainPopulated"].ByteValue);
			//Assert.AreEqual(1, dataTag["V"].ByteValue);
			Assert.AreEqual(0, dataTag["InhabitedTime"].LongValue);
			Assert.AreEqual(256, dataTag["Biomes"].ByteArrayValue.Length);
			Assert.AreEqual(256, dataTag["HeightMap"].IntArrayValue.Length);

			NbtList sections = dataTag["Sections"] as NbtList;
			Assert.IsNotNull(sections);
			int i = 0;
			foreach (NbtTag sectionTag in sections)
			{
				// This will turn into a full chunk column

				Assert.AreEqual(i++, sectionTag["Y"].ByteValue);
				Assert.AreEqual(4096, sectionTag["Blocks"].ByteArrayValue.Length);
				//Assert.AreEqual(2048, sectionTag["Add"].ByteArrayValue.Length);
				Assert.AreEqual(2048, sectionTag["Data"].ByteArrayValue.Length);
				Assert.AreEqual(2048, sectionTag["BlockLight"].ByteArrayValue.Length);
				Assert.AreEqual(2048, sectionTag["SkyLight"].ByteArrayValue.Length);
			}
			Assert.AreEqual(4, i);

			NbtList entities = dataTag["Entities"] as NbtList;
			Assert.IsNotNull(entities);
			Assert.AreEqual(0, entities.Count);

			NbtList blockEntities = dataTag["TileEntities"] as NbtList;
			Assert.IsNotNull(blockEntities);
			Assert.AreEqual(0, blockEntities.Count);

			NbtList tileTicks = dataTag["TileTicks"] as NbtList;
			Assert.IsNull(tileTicks);

			//Assert.AreEqual(1, dataTag[""].ByteValue);

			Assert.Less(sw.ElapsedMilliseconds, 100);
		}

		[Test, Ignore]
		public void NibbleTest()
		{
			byte[] array = new byte[16*16*128*32*32];

			NibbleArray buffer = new NibbleArray(16*16*128*32*32);

			Stopwatch sw = new Stopwatch();
			sw.Start();
			for (int i = 0; i < array.Length; i++)
				array[i] = 0xff;

			Assert.Less(sw.ElapsedMilliseconds, 200);
			sw.Restart();

			Parallel.For(0, array.Length, i => array[i] = 0xff);
			Assert.Less(sw.ElapsedMilliseconds, 100);

			sw.Restart();

			for (int i = 0; i < buffer.Length; i++)
				buffer[i] = 0xff;

			Assert.Less(sw.ElapsedMilliseconds, 1);
		}

		[Test, Ignore]
		public void LoadAnvilChunkLoadPerformanceTest()
		{
			var provider = new AnvilWorldProvider();
			provider.Initialize();

			Stopwatch sw = new Stopwatch();
			sw.Start();

			int width = 32;
			int depth = 32;

			for (int x = 0; x < 32; x++)
			{
				for (int z = 0; z < 32; z++)
				{
					int cx = (width*4) + x;
					int cz = (depth*25) + z;

					ChunkCoordinates coordinates = new ChunkCoordinates(cx, cz);
					provider.GetChunk(coordinates);
					//Print(regionFile, x, z);
				}
			}

			float noChunks = (32*32);

			Assert.Less(sw.ElapsedTicks/noChunks, 1500); // Avg less than 1.5ms
			Assert.Less(sw.ElapsedTicks, 1500*noChunks); // Total less than 1.5ms * number of chunks
		}

		private void Print(FileStream regionFile, int x, int z)
		{
			ChunkCoordinates coordinates = new ChunkCoordinates(x, z);

			//int tableOffset = ((coordinates.X%width) + (coordinates.Z%depth)*width)*4;
			int tableOffset = 4*((coordinates.X) + (coordinates.Z)*32);

			regionFile.Seek(tableOffset, SeekOrigin.Begin);
			byte[] offsetBuffer = new byte[4];
			regionFile.Read(offsetBuffer, 0, 3);
			Array.Reverse(offsetBuffer);
			int offset = BitConverter.ToInt32(offsetBuffer, 0) << 4;

			int length = regionFile.ReadByte();

			if (offset == 0 || length == 0)
				return;

			Console.WriteLine("X:{0}, Z:{1}", x, z);
		}
	}
}