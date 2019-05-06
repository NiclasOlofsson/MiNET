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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2019 Niclas Olofsson.
// All Rights Reserved.

#endregion

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiNET.Blocks;
using MiNET.Items;
using MiNET.Worlds;
using Newtonsoft.Json.Linq;

namespace MiNET.Test
{
	[TestClass]
	public class GeneralTests
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(GeneralTests));

		[TestMethod]
		public void IndexShiftTest()
		{
			byte bx = 15;
			byte bz = 15;
			byte by = 15;

			int a = (bx * 256) + (bz * 16) + by;
			int b = (bx << 8) | (bz << 4) | by;

			Assert.AreEqual(a, b);

			int c = by - 16 * (by >> 4);
			int d = by & 0xf;

			int y6 = 0b0000_0000_0000_0011; // 144
			int y7 = 0b0000_0000_1001_0000; // 144
			int y1 = 0b0000_0000_1001_0011; // 147
			int y3 = 0b0000_0000_1111_1111; // 255
			int v6 = 0b0000_0001_0000_1111; // 15
			int y2 = 0b0000_0001_0000_0000; // 256
			int v5 = 0b0000_0001_0001_0000; // 16

			Assert.AreEqual(c, d);
		}

		//[TestMethod]
		//public void TimerDisposeTest()
		//{
		//	int count = 0;
		//	HighPrecisionTimer timer = null;
		//	timer = new HighPrecisionTimer(2, o =>
		//	{
		//		Console.WriteLine(".. tick ..");
		//		if (count++ == 10) new Task(() => timer?.Dispose()).Start();
		//		//if (count++ == 10) timer?.Dispose();
		//	}, false, false);

		//	Thread.Sleep(1000);
		//}
		//[TestMethod]
		//public void CheckStateDecodingForPalette()
		//{
		//	//uint word = 0b0011_1000_1110_0011_1000_1110_0011_1000;
		//	uint word = 0b11111111111111001001001001001001;
		//	int position = 1; // 111 111 111 111 11
		//	int bitsPerBlock = 3;
		//	int blocksPerWord = 10;

		//	int wordCount = (int) Math.Ceiling(4096d / blocksPerWord);
		//	Assert.AreEqual(410, wordCount);

		//	int mask = (1 << bitsPerBlock) - 1;
		//	Assert.AreEqual(0b111, mask, "wrong mask");
		//	//long state = (word >> ((position % blocksPerWord) * bitsPerBlock)) & ((1 << bitsPerBlock) - 1);
		//	int state = (int) ((word >> ((position % blocksPerWord) * bitsPerBlock)) & ((1 << bitsPerBlock) - 1));
		//	Assert.AreEqual(0b111, state, "Wrong index");

		//	//state = (word >> ((position++ % blocksPerWord) * bitsPerBlock)) & ((1 << bitsPerBlock) - 1);
		//	//Assert.AreEqual(0b111, state);

		//	// 2019-05-05 00:56:15,666 [DedicatedThreadPool-095cc122-3d6d-4f0b-8eab-4b4797f555a2_1] ERROR MiNET.Worlds.LevelDbProvider -
		//	// Got wrong state=7 from word. bitsPerBlock=3, blocksPerWord=10, Word=4294742601

		//	//2019-05-05 01:09:55,892 [DedicatedThreadPool-095cc122-3d6d-4f0b-8eab-4b4797f555a2_4] ERROR MiNET.Worlds.LevelDbProvider -
		//	// Got wrong state=7 from word. bitsPerBlock=3, blocksPerWord=10, Word=4294742601
		//}


		[TestMethod]
		public void NbtCheckPerformanceTests()
		{
			var firework = new ItemFireworks();

			firework.ExtraData = ItemFireworks.ToNbt(new ItemFireworks.FireworksData()
			{
				Explosions = new List<ItemFireworks.FireworksExplosion>()
				{
					new ItemFireworks.FireworksExplosion()
					{
						FireworkColor = new[] {(byte) 0},
						FireworkFade = new[] {(byte) 1},
						FireworkFlicker = true,
						FireworkTrail = false,
						FireworkType = 0,
					},
					new ItemFireworks.FireworksExplosion()
					{
						FireworkColor = new[] {(byte) 1},
						FireworkFade = new[] {(byte) 2},
						FireworkFlicker = true,
						FireworkTrail = false,
						FireworkType = 1,
					},
					new ItemFireworks.FireworksExplosion()
					{
						FireworkColor = new[] {(byte) 2},
						FireworkFade = new[] {(byte) 3},
						FireworkFlicker = true,
						FireworkTrail = false,
						FireworkType = 2,
					},
					new ItemFireworks.FireworksExplosion()
					{
						FireworkColor = new[] {(byte) 3},
						FireworkFade = new[] {(byte) 4},
						FireworkFlicker = true,
						FireworkTrail = false,
						FireworkType = 3,
					},
					new ItemFireworks.FireworksExplosion()
					{
						FireworkColor = new[] {(byte) 4},
						FireworkFade = new[] {(byte) 5},
						FireworkFlicker = true,
						FireworkTrail = false,
						FireworkType = 4,
					}
				},
				Flight = 2
			});

			for (var i = 0; i < 10000; i++)
				Assert.AreEqual(firework.Equals(firework), true);
		}

		[TestMethod]
		public void ArrayFillPerformanceTests()
		{
			byte[] array = new byte[1000000000];
			byte b = array[array.Length - 1];

			var sw = Stopwatch.StartNew();
			ChunkColumn.Fill<byte>(array, 0xff);
			Console.WriteLine($"My fill {sw.ElapsedMilliseconds}ms");

			sw.Restart();
			Array.Fill<byte>(array, 0xff);
			Console.WriteLine($"Core fill {sw.ElapsedMilliseconds}ms");
		}

		[TestMethod]
		public void GenerateClassesForBlocks()
		{
			Dictionary<int, Blockstate> blockstates = new Dictionary<int, Blockstate>();

			var assembly = Assembly.GetAssembly(typeof(Block));
			var legacyIdMap = new Dictionary<string, int>();
			using (Stream stream = assembly.GetManifestResourceStream(typeof(Block).Namespace + ".legacy_id_map.json"))
			using (StreamReader reader = new StreamReader(stream))
			{
				var result = JObject.Parse(reader.ReadToEnd());

				foreach (var obj in result)
				{
					legacyIdMap.Add(obj.Key, (int) obj.Value);
				}
			}

			using (Stream stream = assembly.GetManifestResourceStream(typeof(Block).Namespace + ".blockstates.json"))
			using (StreamReader reader = new StreamReader(stream))
			{
				dynamic jsonBlockstates = JArray.Parse(reader.ReadToEnd());

				int runtimeId = 0;
				foreach (var obj in jsonBlockstates)
				{
					try
					{
						var name = (string) obj.name;
						if (legacyIdMap.TryGetValue(name, out var id))
						{
							blockstates.Add(runtimeId, new Blockstate() {Id = id, Data = (short) obj.data, Name = (string) obj.name, RuntimeId = runtimeId});
							runtimeId++;
						}
					}
					catch (Exception e)
					{
						Console.WriteLine($"{obj}");
						throw;
					}
				}
			}


			List<(int, string)> blocks = new List<(int, string)>();

			string fileName = Path.GetTempPath() + "MissingBlocks_" + Guid.NewGuid() + ".txt";
			using (FileStream file = File.OpenWrite(fileName))
			{
				Log.Warn($"Writing new blocks to filename:\n{fileName}");

				IndentedTextWriter writer = new IndentedTextWriter(new StreamWriter(file));

				writer.WriteLine($"namespace MiNET.Blocks");
				writer.WriteLine($"{{");
				writer.Indent++;


				foreach (IGrouping<string, KeyValuePair<int, Blockstate>> blockstate in blockstates.OrderBy(kvp => kvp.Value.Name).ThenBy(kvp => kvp.Value.Data).GroupBy(kvp => kvp.Value.Name))
				{
					var enumerator = blockstate.GetEnumerator();
					enumerator.MoveNext();
					var value = enumerator.Current.Value;
					if (value == null) continue;
					Log.Debug($"{value.RuntimeId}, {value.Name}, {value.Data}");
					int id = BlockFactory.GetBlockIdByName(value.Name.Replace("minecraft:", ""));

					if (id == 0 && !value.Name.Contains("air"))
					{
						string blockName = CodeName(value.Name.Replace("minecraft:", ""), true);

						blocks.Add((value.Id, blockName));

						writer.WriteLine($"public class {blockName}: Block");
						writer.WriteLine($"{{");
						writer.Indent++;

						writer.WriteLine($"public {blockName}() : base({value.Id})");
						writer.WriteLine($"{{");
						writer.Indent++;

						writer.WriteLine($"Name = \"{value.Name}\";");

						do
						{
							writer.WriteLine($"// runtime id: {enumerator.Current.Value.RuntimeId} 0x{enumerator.Current.Value.RuntimeId:X}, data: {enumerator.Current.Value.Data}");
						} while (enumerator.MoveNext());

						writer.Indent--;
						writer.WriteLine($"}}");

						writer.Indent--;
						writer.WriteLine($"}}");
					}
				}
				writer.Indent--;
				writer.WriteLine($"}}");

				foreach (var block in blocks.OrderBy(tuple => tuple.Item1))
				{
					writer.WriteLine($"else if (blockId == {block.Item1}) block = new {block.Item2}();");
				}

				writer.Flush();
			}
		}


		private string CodeName(string name, bool firstUpper = false)
		{
			name = name.ToLowerInvariant();

			bool upperCase = firstUpper;

			var result = string.Empty;
			for (int i = 0; i < name.Length; i++)
			{
				if (name[i] == ' ' || name[i] == '_')
				{
					upperCase = true;
				}
				else
				{
					if ((i == 0 && firstUpper) || upperCase)
					{
						result += name[i].ToString().ToUpperInvariant();
						upperCase = false;
					}
					else
					{
						result += name[i];
					}
				}
			}

			result = result.Replace(@"[]", "s");
			return result;
		}
	}
}