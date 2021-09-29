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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2020 Niclas Olofsson.
// All Rights Reserved.

#endregion

using System;
using System.Buffers.Text;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.InteropServices;
using fNbt;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiNET.Blocks;
using MiNET.Items;
using MiNET.Net;
using MiNET.Utils;
using MiNET.Utils.Cryptography;
using MiNET.Utils.Nbt;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Test
{
	[TestClass]
	public class GeneralTests
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(GeneralTests));
#if DEBUG
		private int _iterations = 100_000;
#else
		private int _iterations = 10_000_000;
#endif
		[TestMethod]
		public void Check_all_air_base_test()
		{
			var buffer = new byte[10_000].Concat(new byte[] {42}).ToArray();

			for (int k = 0; k < _iterations; k++)
			{
				bool foundNonZero = false;
				for (int i = 0; i < buffer.Length; i++)
				{
					if (buffer[i] != 0)
					{
						foundNonZero = true;
						break;
					}
				}
				Assert.IsTrue(foundNonZero);
			}
		}

		[TestMethod]
		public void Check_all_air_fast_test()
		{
			var buffer = new short[10_000].Concat(new short[] {42}).ToArray();
			for (int i = 0; i < _iterations; i++)
			{
				Assert.IsFalse(SubChunk.AllZeroFast(buffer));
			}
		}

		[TestMethod]
		public void Check_all_air_vector_test()
		{
			Assert.IsTrue(Vector.IsHardwareAccelerated);

			var buffer = new byte[10_000].Concat(new byte[] {42}).ToArray();

			for (int i = 0; i < _iterations; i++)
			{
				bool foundNonZero = false;
				int remainingStart = 0;
				while (remainingStart <= buffer.Length - Vector<byte>.Count)
				{
					var vector = new Vector<byte>(buffer, remainingStart);
					if (!Vector.EqualsAll(vector, default))
					{
						break;
					}
					remainingStart += Vector<byte>.Count;
				}

				for (int j = remainingStart; j < buffer.Length; j++)
				{
					if (buffer[j] != 0)
					{
						foundNonZero = true;
						break;
					}
				}
				Assert.IsTrue(foundNonZero);
			}
		}

		[TestMethod]
		public void Check_all_air_long_test()
		{
			var buffer = new byte[10_000].Concat(new byte[] {42}).ToArray();

			for (int k = 0; k < _iterations; k++)
			{
				int remainingStart = 0;
				bool foundNonZero = false;
				if (IntPtr.Size == sizeof(long))
				{
					Span<long> longBuffer = MemoryMarshal.Cast<byte, long>(buffer);
					remainingStart = longBuffer.Length * sizeof(long);

					for (int i = 0; i < longBuffer.Length; i++)
					{
						if (longBuffer[i] != 0)
						{
							remainingStart = i * sizeof(long);
							break;
						}
					}
				}

				for (int i = remainingStart; i < buffer.Length; i++)
				{
					if (buffer[i] != 0)
					{
						foundNonZero = true;
						break;
					}
				}
				Assert.IsTrue(foundNonZero);
			}
		}

		[TestMethod]
		public void ZeroValueDetectSse()
		{
			var buffer = new byte[10_000].Concat(new byte[] { 42 }).ToArray();
			for (int k = 0; k < _iterations; k++)
			{
				bool foundNonZero = false;
				int concurrentAmount = 4;
				int startIndex = 0;
				int endIndex = buffer.Length -1;
				int sseIndexEnd = startIndex + ((endIndex - startIndex + 1) / (Vector<byte>.Count * concurrentAmount)) * (Vector<byte>.Count * concurrentAmount);
				int i;
				int offset1 = Vector<byte>.Count;
				int offset2 = Vector<byte>.Count * 2;
				int offset3 = Vector<byte>.Count * 3;
				int increment = Vector<byte>.Count * concurrentAmount;
				for (i = startIndex; i < sseIndexEnd; i += increment)
				{
					var inVector = new Vector<byte>(buffer, i);
					inVector |= new Vector<byte>(buffer, i + offset1);
					inVector |= new Vector<byte>(buffer, i + offset2);
					inVector |= new Vector<byte>(buffer, i + offset3);
					if (!Vector.EqualsAll(inVector, default))
					{
						foundNonZero = true;
						break;
					}
				}

				if(foundNonZero) continue;

				byte overallOr = 0;
				for (; i <= endIndex; i++)
					overallOr |= buffer[i];
				foundNonZero = overallOr != 0;
				//for (; i <= endIndex; i++)
				//{
				//	if (buffer[i] != 0)
				//	{
				//		foundNonZero = true;
				//		break;
				//	}
				//}

				Assert.IsTrue(foundNonZero);
			}
		}

		/*[TestMethod]
		public void DeltaEncodeTest()
		{
			float curr = 0.34453f;
			float prev = -3.7989f;

			int delta = McpeMoveEntityDelta.ToIntDelta(curr, prev);

			float result = BitConverter.Int32BitsToSingle(BitConverter.SingleToInt32Bits((float) Math.Round(prev, 2)) + delta);

			Assert.AreEqual(Math.Round(curr, 2), Math.Round(result, 2));
		}*/

		[TestMethod]
		public void EncodePaletteChunk()
		{
			var blocks = new short[4096];
			var random = new Random();
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
			var metas = new byte[4096];

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
			var firework = new ItemFireworkRocket();

			firework.ExtraData = ItemFireworkRocket.ToNbt(new ItemFireworkRocket.FireworksData()
			{
				Explosions = new List<ItemFireworkRocket.FireworksExplosion>()
				{
					new ItemFireworkRocket.FireworksExplosion()
					{
						FireworkColor = new[] {(byte) 0},
						FireworkFade = new[] {(byte) 1},
						FireworkFlicker = true,
						FireworkTrail = false,
						FireworkType = 0,
					},
					new ItemFireworkRocket.FireworksExplosion()
					{
						FireworkColor = new[] {(byte) 1},
						FireworkFade = new[] {(byte) 2},
						FireworkFlicker = true,
						FireworkTrail = false,
						FireworkType = 1,
					},
					new ItemFireworkRocket.FireworksExplosion()
					{
						FireworkColor = new[] {(byte) 2},
						FireworkFade = new[] {(byte) 3},
						FireworkFlicker = true,
						FireworkTrail = false,
						FireworkType = 2,
					},
					new ItemFireworkRocket.FireworksExplosion()
					{
						FireworkColor = new[] {(byte) 3},
						FireworkFade = new[] {(byte) 4},
						FireworkFlicker = true,
						FireworkTrail = false,
						FireworkType = 3,
					},
					new ItemFireworkRocket.FireworksExplosion()
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
			byte[] array = new byte[4096];

			ChunkColumn.Fill<byte>(array, 0xff);
			var sw = Stopwatch.StartNew();
			int iterations = _iterations;
			for (int i = 0; i < iterations; i++)
			{
				ChunkColumn.Fill<byte>(array, 0xff);
			}
			Console.WriteLine($"My fill {sw.ElapsedMilliseconds}ms");

			ChunkColumn.FastFill<byte>(ref array, 0xff, ulong.MaxValue);
			sw.Restart();
			for (int i = 0; i < iterations; i++)
			{
				ChunkColumn.FastFill<byte>(ref array, 0xff, ulong.MaxValue);
			}
			Console.WriteLine($"My fast fill {sw.ElapsedMilliseconds}ms");
			foreach (byte t in array)
			{
				Assert.AreEqual(0xff, t);
			}

			Array.Fill<byte>(array, 0xff);
			sw.Restart();
			for (int i = 0; i < iterations; i++)
			{
				Array.Fill<byte>(array, 0xff);
			}
			Console.WriteLine($"Core fill {sw.ElapsedMilliseconds}ms");

			Array.Clear(array, 0, array.Length);
			sw.Restart();
			for (int i = 0; i < iterations; i++)
			{
				Array.Clear(array, 0, array.Length);
			}
			Console.WriteLine($"Core clear {sw.ElapsedMilliseconds}ms");
		}

		[TestMethod]
		public void GenerateClassesForBlocks()
		{
			BlockPalette palette = null;

			var assembly = Assembly.GetAssembly(typeof(Block));
			using (var stream = assembly.GetManifestResourceStream(typeof(Block).Namespace + ".blockstates.json"))
			using (var reader = new StreamReader(stream))
			{
				palette = BlockPalette.FromJson(reader.ReadToEnd());
			}
			//Assert.AreEqual(3045, palette.Count);
			BlockStateContainer stateContainer = palette.First(b => b.Id == 6 && b.Data == 10);
			Assert.AreEqual(2, stateContainer.States.Count);

			foreach (IBlockState recordState in stateContainer.States)
			{
				switch (recordState)
				{
					case BlockStateByte blockStateByte:
						Assert.AreEqual("age_bit", blockStateByte.Name);
						Assert.AreEqual(1, blockStateByte.Value);
						break;
					case BlockStateInt blockStateInt:
						Assert.AreEqual(0, blockStateInt.Value);
						break;
					case BlockStateString blockStateString:
						Assert.AreEqual("sapling_type", blockStateString.Name);
						Assert.AreEqual("birch", blockStateString.Value);
						break;
					default:
						throw new ArgumentOutOfRangeException(nameof(recordState));
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


				foreach (IGrouping<string, BlockStateContainer> blockstate in palette.OrderBy(r => r.Name).ThenBy(r => r.Data).GroupBy(r => r.Name))
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

		[TestMethod]
		public void McpeMoveEntityDelta_encode_decode_tests()
		{
			var packet = new McpeMoveEntityDelta();
			packet.runtimeEntityId = 0x0102030405;
			packet.flags = 0;
			var prev = new PlayerLocation(new Vector3(0, 0, 0), 0, 0, 0);
			packet.prevSentPosition = prev;
			var current = new PlayerLocation(new Vector3(1, 2, 3), 41, 49, 60);
			packet.currentPosition = current;
			packet.isOnGround = true;

			var bytes = packet.Encode();

			packet = new McpeMoveEntityDelta();
			packet.prevSentPosition = prev;
			packet.Decode(bytes.AsMemory());

			Assert.AreEqual(packet.runtimeEntityId, 0x0102030405);

			Assert.AreEqual(packet.flags & McpeMoveEntityDelta.HasX, McpeMoveEntityDelta.HasX);
			Assert.AreEqual(packet.flags & McpeMoveEntityDelta.HasY, McpeMoveEntityDelta.HasY);
			Assert.AreEqual(packet.flags & McpeMoveEntityDelta.HasZ, McpeMoveEntityDelta.HasZ);

			Assert.AreEqual(packet.flags & McpeMoveEntityDelta.HasRotX, McpeMoveEntityDelta.HasRotX);
			Assert.AreEqual(packet.flags & McpeMoveEntityDelta.HasRotY, McpeMoveEntityDelta.HasRotY);
			Assert.AreEqual(packet.flags & McpeMoveEntityDelta.HasRotZ, McpeMoveEntityDelta.HasRotZ);

			Assert.AreEqual(packet.flags & McpeMoveEntityDelta.OnGround, McpeMoveEntityDelta.OnGround);

			Assert.AreEqual(new Vector3(1, 2, 3), packet.GetCurrentPosition(prev).ToVector3());
		}

		[TestMethod]
		public void DetlaConversionTest()
		{
			//int intValue = BitConverter.ToInt32(BitConverter.GetBytes(3f), 0);
			int intValue = BitConverter.SingleToInt32Bits(3f);
			Assert.AreEqual(0x40400000, intValue);
			float singleValue = BitConverter.Int32BitsToSingle(intValue);
			Assert.AreEqual(3f, singleValue);


			float prev = 10.0f;
			float currentPos = 13.0f;

			int intResult = BitConverter.SingleToInt32Bits(currentPos) - BitConverter.SingleToInt32Bits(prev);

			Assert.AreEqual(0x300000, intResult);

			float converted = BitConverter.Int32BitsToSingle(BitConverter.SingleToInt32Bits(prev) + intResult);

			Assert.AreEqual(13f, converted);
		}

		[TestMethod]
		public void NbtBiomeParseTest()
		{
			string base64 = "CgAKDWJhbWJvb19qdW5nbGUFCGRvd25mYWxsZmZmPwULdGVtcGVyYXR1cmUzM3M/AAoTYmFtYm9vX2p1bmdsZV9oaWxscwUIZG93bmZhbGxmZmY/BQt0ZW1wZXJhdHVyZTMzcz8ACgViZWFjaAUIZG93bmZhbGzNzMw+BQt0ZW1wZXJhdHVyZc3MTD8ACgxiaXJjaF9mb3Jlc3QFCGRvd25mYWxsmpkZPwULdGVtcGVyYXR1cmWamRk/AAoSYmlyY2hfZm9yZXN0X2hpbGxzBQhkb3duZmFsbJqZGT8FC3RlbXBlcmF0dXJlmpkZPwAKGmJpcmNoX2ZvcmVzdF9oaWxsc19tdXRhdGVkBQhkb3duZmFsbM3MTD8FC3RlbXBlcmF0dXJlMzMzPwAKFGJpcmNoX2ZvcmVzdF9tdXRhdGVkBQhkb3duZmFsbM3MTD8FC3RlbXBlcmF0dXJlMzMzPwAKCmNvbGRfYmVhY2gFCGRvd25mYWxsmpmZPgULdGVtcGVyYXR1cmXNzEw9AAoKY29sZF9vY2VhbgUIZG93bmZhbGwAAAA/BQt0ZW1wZXJhdHVyZQAAAD8ACgpjb2xkX3RhaWdhBQhkb3duZmFsbM3MzD4FC3RlbXBlcmF0dXJlAAAAvwAKEGNvbGRfdGFpZ2FfaGlsbHMFCGRvd25mYWxszczMPgULdGVtcGVyYXR1cmUAAAC/AAoSY29sZF90YWlnYV9tdXRhdGVkBQhkb3duZmFsbM3MzD4FC3RlbXBlcmF0dXJlAAAAvwAKD2RlZXBfY29sZF9vY2VhbgUIZG93bmZhbGwAAAA/BQt0ZW1wZXJhdHVyZQAAAD8AChFkZWVwX2Zyb3plbl9vY2VhbgUIZG93bmZhbGwAAAA/BQt0ZW1wZXJhdHVyZQAAAAAAChNkZWVwX2x1a2V3YXJtX29jZWFuBQhkb3duZmFsbAAAAD8FC3RlbXBlcmF0dXJlAAAAPwAKCmRlZXBfb2NlYW4FCGRvd25mYWxsAAAAPwULdGVtcGVyYXR1cmUAAAA/AAoPZGVlcF93YXJtX29jZWFuBQhkb3duZmFsbAAAAD8FC3RlbXBlcmF0dXJlAAAAPwAKBmRlc2VydAUIZG93bmZhbGwAAAAABQt0ZW1wZXJhdHVyZQAAAEAACgxkZXNlcnRfaGlsbHMFCGRvd25mYWxsAAAAAAULdGVtcGVyYXR1cmUAAABAAAoOZGVzZXJ0X211dGF0ZWQFCGRvd25mYWxsAAAAAAULdGVtcGVyYXR1cmUAAABAAAoNZXh0cmVtZV9oaWxscwUIZG93bmZhbGyamZk+BQt0ZW1wZXJhdHVyZc3MTD4AChJleHRyZW1lX2hpbGxzX2VkZ2UFCGRvd25mYWxsmpmZPgULdGVtcGVyYXR1cmXNzEw+AAoVZXh0cmVtZV9oaWxsc19tdXRhdGVkBQhkb3duZmFsbJqZmT4FC3RlbXBlcmF0dXJlzcxMPgAKGGV4dHJlbWVfaGlsbHNfcGx1c190cmVlcwUIZG93bmZhbGyamZk+BQt0ZW1wZXJhdHVyZc3MTD4ACiBleHRyZW1lX2hpbGxzX3BsdXNfdHJlZXNfbXV0YXRlZAUIZG93bmZhbGyamZk+BQt0ZW1wZXJhdHVyZc3MTD4ACg1mbG93ZXJfZm9yZXN0BQhkb3duZmFsbM3MTD8FC3RlbXBlcmF0dXJlMzMzPwAKBmZvcmVzdAUIZG93bmZhbGzNzEw/BQt0ZW1wZXJhdHVyZTMzMz8ACgxmb3Jlc3RfaGlsbHMFCGRvd25mYWxszcxMPwULdGVtcGVyYXR1cmUzMzM/AAoMZnJvemVuX29jZWFuBQhkb3duZmFsbAAAAD8FC3RlbXBlcmF0dXJlAAAAAAAKDGZyb3plbl9yaXZlcgUIZG93bmZhbGwAAAA/BQt0ZW1wZXJhdHVyZQAAAAAACgRoZWxsBQhkb3duZmFsbAAAAAAFC3RlbXBlcmF0dXJlAAAAQAAKDWljZV9tb3VudGFpbnMFCGRvd25mYWxsAAAAPwULdGVtcGVyYXR1cmUAAAAAAAoKaWNlX3BsYWlucwUIZG93bmZhbGwAAAA/BQt0ZW1wZXJhdHVyZQAAAAAAChFpY2VfcGxhaW5zX3NwaWtlcwUIZG93bmZhbGwAAIA/BQt0ZW1wZXJhdHVyZQAAAAAACgZqdW5nbGUFCGRvd25mYWxsZmZmPwULdGVtcGVyYXR1cmUzM3M/AAoLanVuZ2xlX2VkZ2UFCGRvd25mYWxszcxMPwULdGVtcGVyYXR1cmUzM3M/AAoTanVuZ2xlX2VkZ2VfbXV0YXRlZAUIZG93bmZhbGzNzEw/BQt0ZW1wZXJhdHVyZTMzcz8ACgxqdW5nbGVfaGlsbHMFCGRvd25mYWxsZmZmPwULdGVtcGVyYXR1cmUzM3M/AAoOanVuZ2xlX211dGF0ZWQFCGRvd25mYWxsZmZmPwULdGVtcGVyYXR1cmUzM3M/AAoTbGVnYWN5X2Zyb3plbl9vY2VhbgUIZG93bmZhbGwAAAA/BQt0ZW1wZXJhdHVyZQAAAAAACg5sdWtld2FybV9vY2VhbgUIZG93bmZhbGwAAAA/BQt0ZW1wZXJhdHVyZQAAAD8ACgptZWdhX3RhaWdhBQhkb3duZmFsbM3MTD8FC3RlbXBlcmF0dXJlmpmZPgAKEG1lZ2FfdGFpZ2FfaGlsbHMFCGRvd25mYWxszcxMPwULdGVtcGVyYXR1cmWamZk+AAoEbWVzYQUIZG93bmZhbGwAAAAABQt0ZW1wZXJhdHVyZQAAAEAACgptZXNhX2JyeWNlBQhkb3duZmFsbAAAAAAFC3RlbXBlcmF0dXJlAAAAQAAKDG1lc2FfcGxhdGVhdQUIZG93bmZhbGwAAAAABQt0ZW1wZXJhdHVyZQAAAEAAChRtZXNhX3BsYXRlYXVfbXV0YXRlZAUIZG93bmZhbGwAAAAABQt0ZW1wZXJhdHVyZQAAAEAAChJtZXNhX3BsYXRlYXVfc3RvbmUFCGRvd25mYWxsAAAAAAULdGVtcGVyYXR1cmUAAABAAAoabWVzYV9wbGF0ZWF1X3N0b25lX211dGF0ZWQFCGRvd25mYWxsAAAAAAULdGVtcGVyYXR1cmUAAABAAAoPbXVzaHJvb21faXNsYW5kBQhkb3duZmFsbAAAgD8FC3RlbXBlcmF0dXJlZmZmPwAKFW11c2hyb29tX2lzbGFuZF9zaG9yZQUIZG93bmZhbGwAAIA/BQt0ZW1wZXJhdHVyZWZmZj8ACgVvY2VhbgUIZG93bmZhbGwAAAA/BQt0ZW1wZXJhdHVyZQAAAD8ACgZwbGFpbnMFCGRvd25mYWxszczMPgULdGVtcGVyYXR1cmXNzEw/AAobcmVkd29vZF90YWlnYV9oaWxsc19tdXRhdGVkBQhkb3duZmFsbM3MTD8FC3RlbXBlcmF0dXJlmpmZPgAKFXJlZHdvb2RfdGFpZ2FfbXV0YXRlZAUIZG93bmZhbGzNzEw/BQt0ZW1wZXJhdHVyZQAAgD4ACgVyaXZlcgUIZG93bmZhbGwAAAA/BQt0ZW1wZXJhdHVyZQAAAD8ACg1yb29mZWRfZm9yZXN0BQhkb3duZmFsbM3MTD8FC3RlbXBlcmF0dXJlMzMzPwAKFXJvb2ZlZF9mb3Jlc3RfbXV0YXRlZAUIZG93bmZhbGzNzEw/BQt0ZW1wZXJhdHVyZTMzMz8ACgdzYXZhbm5hBQhkb3duZmFsbAAAAAAFC3RlbXBlcmF0dXJlmpmZPwAKD3NhdmFubmFfbXV0YXRlZAUIZG93bmZhbGwAAAA/BQt0ZW1wZXJhdHVyZc3MjD8ACg9zYXZhbm5hX3BsYXRlYXUFCGRvd25mYWxsAAAAAAULdGVtcGVyYXR1cmUAAIA/AAoXc2F2YW5uYV9wbGF0ZWF1X211dGF0ZWQFCGRvd25mYWxsAAAAPwULdGVtcGVyYXR1cmUAAIA/AAoLc3RvbmVfYmVhY2gFCGRvd25mYWxsmpmZPgULdGVtcGVyYXR1cmXNzEw+AAoQc3VuZmxvd2VyX3BsYWlucwUIZG93bmZhbGzNzMw+BQt0ZW1wZXJhdHVyZc3MTD8ACglzd2FtcGxhbmQFCGRvd25mYWxsAAAAPwULdGVtcGVyYXR1cmXNzEw/AAoRc3dhbXBsYW5kX211dGF0ZWQFCGRvd25mYWxsAAAAPwULdGVtcGVyYXR1cmXNzEw/AAoFdGFpZ2EFCGRvd25mYWxszcxMPwULdGVtcGVyYXR1cmUAAIA+AAoLdGFpZ2FfaGlsbHMFCGRvd25mYWxszcxMPwULdGVtcGVyYXR1cmUAAIA+AAoNdGFpZ2FfbXV0YXRlZAUIZG93bmZhbGzNzEw/BQt0ZW1wZXJhdHVyZQAAgD4ACgd0aGVfZW5kBQhkb3duZmFsbAAAAD8FC3RlbXBlcmF0dXJlAAAAPwAKCndhcm1fb2NlYW4FCGRvd25mYWxsAAAAPwULdGVtcGVyYXR1cmUAAAA/AAA=";
			var bytes = base64.DecodeBase64();

			Assert.AreEqual(0x0a, bytes[0]);
			Assert.AreEqual(0x3f, bytes[^3]);

			var stream = new MemoryStream(bytes);
			Nbt nbt = new Nbt();
			NbtFile file = new NbtFile();
			file.BigEndian = false;
			file.UseVarInt = true;
			nbt.NbtFile = file;
			file.LoadFromStream(stream, NbtCompression.None);
		}

				[TestMethod]
		public void NbtBlockPropertiesParseTest()
		{
			var bytes = new byte[] {0x0a, 0x00, 0x00};

			var stream = new MemoryStream(bytes);
			Nbt nbt = new Nbt();
			NbtFile file = new NbtFile();
			file.BigEndian = false;
			file.UseVarInt = true;
			nbt.NbtFile = file;
			file.LoadFromStream(stream, NbtCompression.None);
		}


	}
}