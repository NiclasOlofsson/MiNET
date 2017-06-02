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
// The Original Code is Niclas Olofsson.
// 
// The Original Developer is the Initial Developer.  The Initial Developer of
// the Original Code is Niclas Olofsson.
// 
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2017 Niclas Olofsson. 
// All Rights Reserved.

#endregion

using System;
using System.Diagnostics;
using System.IO;
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
		public void OffsetFileExistPerformance()
		{
			var basePath = @"D:\Development\Repos\MapsPE\hub";
			var provider = new AnvilWorldProvider(basePath);
			provider.Initialize();

			var coordinates = new ChunkCoordinates(new PlayerLocation(-1021, 18, 2385));

			for (int i = 0; i < 10000; i++)
			{
				var chunk = provider.GenerateChunkColumn(coordinates);

				int rx = coordinates.X >> 5;
				int rz = coordinates.Z >> 5;

				string filePath = Path.Combine(basePath, string.Format(@"region{2}r.{0}.{1}.mca", rx, rz, Path.DirectorySeparatorChar));
				Assert.True(File.Exists(filePath));

				Assert.AreEqual(-2, rx, "X");
				Assert.AreEqual(4, rz, "Z");
				Assert.IsNull(chunk, $"Region Coord: {rx}, {rz}");
				//Assert.IsNotNull(chunk);
			}
		}

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

		//[Test, Ignore]
		//public void LoadAnvilChunkLoadTest()
		//{
		//	int width = 32;
		//	int depth = 32;

		//	int cx = (width*4) + 3;
		//	int cz = (depth*25) + 0;

		//	ChunkCoordinates coordinates = new ChunkCoordinates(cx, cz);

		//	int rx = coordinates.X >> 5;
		//	int rz = coordinates.Z >> 5;

		//	Assert.AreEqual(4, rx);
		//	Assert.AreEqual(25, rz);

		//	string basePath = @"D:\Downloads\KingsLanding1";
		//	var generator = new FlatlandWorldProvider();

		//	Stopwatch sw = new Stopwatch();
		//	sw.Start();

		//	int iterations = 1024;
		//	for (int i = 0; i < iterations; i++)
		//	{
		//		AnvilWorldProvider.GetChunk(coordinates, basePath, generator, 30);
		//	}

		//	long ticks = sw.ElapsedTicks;
		//	long ms = sw.ElapsedMilliseconds;

		//	//Assert.Less(ticks/iterations, 100);

		//	Console.WriteLine("Read {0} chunk-columns in {1}ns ({3}ms) at a rate of {2}ns/col", iterations, ticks, ticks/iterations, ms);
		//}


		[Test, Ignore]
		public void LoadFullAnvilRegionLoadTest()
		{
			int width = 32;
			int depth = 32;

			int regionX = 5;
			int regionZ = 25;

			string basePath = @"D:\Downloads\KingsLanding1\KingsLanding1";
			var generator = new FlatlandWorldProvider();

			Stopwatch sw = new Stopwatch();
			sw.Start();
			int noChunksRead = 0;
			for (int x = 1; x < 32; x++)
			{
				for (int z = 1; z < 32; z++)
				{
					noChunksRead++;
					int cx = (width*regionX) + x;
					int cz = (depth*regionZ) + z;

					ChunkCoordinates coordinates = new ChunkCoordinates(cx, cz);
					ChunkColumn chunk = new AnvilWorldProvider().GetChunk(coordinates, basePath, null, 0);
					Assert.NotNull(chunk, $"Expected chunk at {x}, {z}");
				}
			}
			Console.WriteLine("Read {0} chunks in {1}ms", noChunksRead, sw.ElapsedMilliseconds);
		}

		[Test, Ignore]
		public void CompressionTests()
		{
			string basePath = @"D:\Downloads\KingsLanding1\KingsLanding1";

			int cx = (32*5) + 1;
			int cz = (32*25) + 1;

			ChunkCoordinates coordinates = new ChunkCoordinates(cx, cz);
			ChunkColumn chunk = new AnvilWorldProvider().GetChunk(coordinates, basePath, null, 0);
			var bytes = chunk.GetBytes();

			Stopwatch sw = new Stopwatch();
			sw.Start();
			var noChunksCompressed = 10000;
			for (int i = 0; i < noChunksCompressed; i++)
			{
				Compression.Compress(bytes, 0, bytes.Length, true);
			}
			Console.WriteLine("Compressed {2} bytes {0} times in {1}ms", noChunksCompressed, sw.ElapsedMilliseconds, bytes.Length);
		}
	}
}