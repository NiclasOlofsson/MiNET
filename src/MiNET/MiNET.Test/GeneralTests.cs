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
using MiNET.Net;
using MiNET.Utils;
using MiNET.Worlds;
using Newtonsoft.Json.Linq;

namespace MiNET.Test
{
	[TestClass]
	public class GeneralTests
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(GeneralTests));

		[TestMethod]
		public void DeltaEncodeTest()
		{
			float curr = 0.34453f;
			float prev = -3.7989f;

			int delta = McpeMoveEntityDelta.ToIntDelta(curr, prev);

			float result = BitConverter.Int32BitsToSingle(BitConverter.SingleToInt32Bits((float) Math.Round(prev, 2)) + delta);

			Assert.AreEqual(Math.Round(curr, 2), Math.Round(result, 2));
		}

		[TestMethod]
		public void EncodePalettedChunk()
		{
			//PaletteChunk chunk = new PaletteChunk();
			//chunk.GetBytes()

			uint waste = BlockFactory.GetRuntimeId(0, 0);
			int[] legacyToRuntimeId = BlockFactory.LegacyToRuntimeId;

			short[] blocks = new short[4096];
			Random random = new Random();
			for (int i = 0; i < blocks.Length; i++)
			{
				blocks[i] = (short) random.Next(8);
			}
			//blocks[0] = 0b000;
			//blocks[1] = 0b111;
			//blocks[2] = 0b111;
			//blocks[3] = 0b111;
			//blocks[4] = 0b111;
			//blocks[5] = 0b111;
			//blocks[6] = 0b111;
			//blocks[7] = 0b001;
			//blocks[8] = 0b010;
			//blocks[9] = 0b011;
			//blocks[10] = 0b100;
			//blocks[11] = 0b101;
			byte[] metas = new byte[4096];

			int count = 10_000;
			var sw = Stopwatch.StartNew();
			for (int c = 0; c < count; c++)
			{
				for (int sc = 0; sc < 8; sc++)
				{
					var palette = new Dictionary<uint, byte>();
					uint prevHash = uint.MaxValue;
					for (int i = 0; i < 4096; i++)
					{
						uint hash = (uint) blocks[i] << 4 | metas[i];
						if (hash == prevHash) continue;

						prevHash = hash;
						palette[hash] = 0;
					}

					// log2(number of entries) => bits needed to store them
					//Assert.AreEqual(0, Math.Ceiling(Math.Log(1, 2)));
					//Assert.AreEqual(1, Math.Ceiling(Math.Log(2, 2)));
					//Assert.AreEqual(2, Math.Ceiling(Math.Log(3, 2)));
					//Assert.AreEqual(2, Math.Ceiling(Math.Log(4, 2)));
					//Assert.AreEqual(3, Math.Ceiling(Math.Log(5, 2)));
					//Assert.AreEqual(3, Math.Ceiling(Math.Log(palette.Count, 2)));
					//Assert.AreEqual(3, Math.Ceiling(Math.Log(8, 2)));
					//Assert.AreEqual(4, Math.Ceiling(Math.Log(9, 2)));
					//Assert.AreEqual(4, Math.Ceiling(Math.Log(16, 2)));
					//Assert.AreEqual(5, Math.Ceiling(Math.Log(17, 2)));
					//Assert.AreEqual(5, Math.Ceiling(Math.Log(32, 2)));
					//Assert.AreEqual(6, Math.Ceiling(Math.Log(33, 2)));
					//Assert.AreEqual(6, Math.Ceiling(Math.Log(64, 2)));
					//Assert.AreEqual(7, Math.Ceiling(Math.Log(65, 2)));
					//Assert.AreEqual(7, Math.Ceiling(Math.Log(128, 2)));
					//Assert.AreEqual(8, Math.Ceiling(Math.Log(129, 2)));
					//Assert.AreEqual(8, Math.Ceiling(Math.Log(256, 2)));
					//Assert.AreEqual(16, Math.Ceiling(Math.Log(ushort.MaxValue, 2)));

					int bitsPerBlock = (int) Math.Ceiling(Math.Log(palette.Count, 2));

					switch (bitsPerBlock)
					{
						case 1:
						case 2:
						case 3:
						case 4:
						case 5:
						case 6:
							//Paletted1 = 1,   // 32 blocks per word
							//Paletted2 = 2,   // 16 blocks per word
							//Paletted3 = 3,   // 10 blocks and 2 bits of padding per word
							//Paletted4 = 4,   // 8 blocks per word
							//Paletted5 = 5,   // 6 blocks and 2 bits of padding per word
							//Paletted6 = 6,   // 5 blocks and 2 bits of padding per word
							break;
						case 7:
						case 8:
							//Paletted8 = 8,  // 4 blocks per word
							bitsPerBlock = 8;
							break;
						case int i when i > 8:
							//Paletted16 = 16, // 2 blocks per word
							bitsPerBlock = 16;
							break;
						default:
							break;
					}

					int blocksPerWord = (int) Math.Floor(32f / bitsPerBlock); // Floor to remove padding bits
					int wordsPerChunk = (int) Math.Ceiling(4096f / blocksPerWord);

					Assert.AreEqual(10, blocksPerWord);

					byte t = 0;
					foreach (var b in palette.ToArray())
					{
						palette[b.Key] = t++;
					}

					uint[] indexes = new uint[wordsPerChunk];

					int position = 0;
					for (int w = 0; w < wordsPerChunk; w++)
					{
						uint word = 0;
						for (int block = 0; block < blocksPerWord; block++)
						{
							if (position >= 4096) continue;

							uint state = palette[(uint) blocks[position] << 4 | metas[position]];
							word |= state << (bitsPerBlock * block);

							//string bin = Convert.ToString(word, 2);
							//bin = new string('0', 32 - bin.Length) + bin;
							//Console.WriteLine($"{bin}");

							position++;
						}
						indexes[w] = word;
					}
				}
			}
			Console.WriteLine($"time={sw.ElapsedMilliseconds}");
		}

		[TestMethod]
		public void StateShiftTest()
		{

			int position = 0;
			int blocksPerWord = 8;
			int bitsPerBlock = 4;

			uint word = 0;
			uint idx = 0;
			for (int block = 0; block < blocksPerWord; block++)
			{
				if (position >= 4096)
					continue;

				uint state = idx++;
				word |= state << (bitsPerBlock * block);

				position++;
			}

			position = 0;
			idx = 0;
			for (int block = 0; block < blocksPerWord; block++)
			{
				if (position >= 4096)
					continue;
				
				uint state = (uint) ((word >> ((position % blocksPerWord) * bitsPerBlock)) & ((1 << bitsPerBlock) - 1));
				Assert.AreEqual(idx++, state);
				position++;
			}

		}


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
			BlockPallet pallet = null;

			var assembly = Assembly.GetAssembly(typeof(Block));
			using (var stream = assembly.GetManifestResourceStream(typeof(Block).Namespace + ".blockstates.json"))
			using (var reader = new StreamReader(stream))
			{
				pallet = BlockPallet.FromJson(reader.ReadToEnd());
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


				foreach (IGrouping<string, BlockRecord> blockstate in pallet.OrderBy(r => r.Name).ThenBy(r => r.Data).GroupBy(r => r.Name))
				{
					var enumerator = blockstate.GetEnumerator();
					enumerator.MoveNext();
					var value = enumerator.Current;
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
							writer.WriteLine($"// runtime id: {enumerator.Current.RuntimeId} 0x{enumerator.Current.RuntimeId:X}, data: {enumerator.Current.Data}");
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