using System;
using System.Diagnostics;
using fNbt;
using MiNET.Utils;
using MiNET.Worlds;
using NUnit.Framework;

namespace MiNET
{
	[TestFixture]
	public class MinetAnvilTest
	{
		[Test, Ignore]
		public void OffsetIntTest()
		{
			//int original = (4096 * 2) - 10;

			//int reminder;
			//Math.DivRem(original, 4096, out reminder);
			////Assert.AreEqual(10, reminder);

			//Assert.AreEqual((4096 * 2), original + (4096 - reminder));

			byte[] expected = {0x00, 0x00, 0x01, 0x00};

			byte[] offsetBuffer = {0x00, 0x00, 0x01, 0x00};
			Array.Reverse(offsetBuffer);
			int offset = BitConverter.ToInt32(offsetBuffer, 0) << 4;
			Assert.AreEqual(4096, offset);

			Assert.AreEqual(4096, BitConverter.ToInt32(new byte[] {0x01, 0x00, 0x00, 0x00}, 0)*4096);


			//byte[] bytes = BitConverter.GetBytes(offset >> 4);
			//Array.Reverse(bytes);
			//Assert.AreEqual(expected, bytes);

			//Assert.AreEqual(offset, BitConverter.ToInt32(bytes, 0) << 4);
		}

		[Test, Ignore]
		public void SaveAnvilChunkTest()
		{
			int width = 32;
			int depth = 32;

			int regionX = 5;
			int regionZ = 24;

			AnvilWorldProvider anvil = new AnvilWorldProvider(@"D:\Development\Worlds\KingsLanding\");
			anvil.Initialize();
			Stopwatch sw = new Stopwatch();
			sw.Start();
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
			Console.WriteLine("Read {0} chunks in {1}ms", anvil.NumberOfCachedChunks(), sw.ElapsedMilliseconds);

			sw.Restart();

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
			int width = 32;
			int depth = 32;

			int cx = (width*4) + 3;
			int cz = (depth*25) + 0;

			ChunkCoordinates coordinates = new ChunkCoordinates(cx, cz);

			int rx = coordinates.X >> 5;
			int rz = coordinates.Z >> 5;

			Assert.AreEqual(4, rx);
			Assert.AreEqual(25, rz);

			string basePath = @"D:\Downloads\KingsLanding1";
			var generator = new FlatlandWorldProvider();

			Stopwatch sw = new Stopwatch();
			sw.Start();

			int iterations = 1024;
			for (int i = 0; i < iterations; i++)
			{
				AnvilWorldProvider.GetChunk(coordinates, basePath, generator, 30);
			}

			long ticks = sw.ElapsedTicks;
			long ms = sw.ElapsedMilliseconds;

			//Assert.Less(ticks/iterations, 100);

			Console.WriteLine("Read {0} chunk-columns in {1}ns ({3}ms) at a rate of {2}ns/col", iterations, ticks, ticks / iterations, ms);
		}


		[Test, Ignore]
		public void LoadFullAnvilRegionLoadTest()
		{
			int width = 32;
			int depth = 32;

			int regionX = 5;
			int regionZ = 24;

			string basePath = @"D:\Downloads\KingsLanding1";
			var generator = new FlatlandWorldProvider();

			Stopwatch sw = new Stopwatch();
			sw.Start();
			int noChunksRead = 0;
			for (int x = 0; x < 32; x++)
			{
				for (int z = 0; z < 32; z++)
				{
					noChunksRead++;
					int cx = (width*regionX) + x;
					int cz = (depth*regionZ) + z;

					ChunkCoordinates coordinates = new ChunkCoordinates(cx, cz);
					ChunkColumn chunk = AnvilWorldProvider.GetChunk(coordinates, basePath, generator, 30);
					Assert.NotNull(chunk);
				}
			}
			Console.WriteLine("Read {0} chunks in {1}ms", noChunksRead, sw.ElapsedMilliseconds);
		}
	}
}