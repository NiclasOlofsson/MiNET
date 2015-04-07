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

			int iterations = 1000000;
			for (int i = 0; i < iterations; i++)
			{
				AnvilWorldProvider.GetChunk(coordinates, basePath, generator, 30);
			}

			long ticks = sw.ElapsedTicks;

			Assert.Less(ticks/iterations, 100);

			Console.WriteLine("Read {0} chunk-columns in {1}ns at a rate of {2}ns/chunk", iterations, ticks, ticks/iterations);
		}
	}
}