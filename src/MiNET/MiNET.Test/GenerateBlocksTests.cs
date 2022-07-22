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
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiNET.Blocks;
using MiNET.Items;
using MiNET.Utils;

namespace MiNET.Test
{
	[TestClass
	, Ignore("Manual code generation")
	]
	public class GenerateBlocksTests
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(GenerateBlocksTests));


		[TestMethod]
		public void GetItemByName()
		{
			
			foreach (KeyValuePair<string, short> keyValuePair in ItemFactory.NameToId)
			{
				if(keyValuePair.Key.Equals("sapling")) Console.WriteLine(keyValuePair.Key);
			}
			var itemId = ItemFactory.GetItemIdByName("minecraft:sapling");
			Assert.AreEqual(6, itemId);

			Item item = ItemFactory.GetItem("minecraft:sapling");
			Assert.AreEqual("minecraft:sapling", item.Name);
			Assert.IsNotNull(item as ItemBlock);
		}

		[TestMethod]
		public void GenerateItemConstructorsWithNames()
		{
			string fileName = Path.GetTempPath() + "Items_constructors_" + Guid.NewGuid() + ".txt";
			using FileStream file = File.OpenWrite(fileName);
			var writer = new IndentedTextWriter(new StreamWriter(file));

			writer.Indent += 2;
			writer.WriteLine();

			var itemStates = ItemFactory.Itemstates;
			foreach (Itemstate state in itemStates)
			{
				Item item = ItemFactory.GetItem(state.Id);
				if(!string.IsNullOrEmpty(item.Name)) continue;

				string clazzName = CodeName(state.Name.Replace("minecraft:", ""), true);
				string minecraftName = state.Name;


				writer.WriteLine($"public Item{clazzName}() : base(\"{minecraftName}\", {state.Id})");
			}

			writer.Flush();
		}

		[TestMethod]
		public void GenerateMissingItemsFromItemsStates()
		{
			string fileName = Path.GetTempPath() + "MissingItems_" + Guid.NewGuid() + ".txt";
			using FileStream file = File.OpenWrite(fileName);
			var writer = new IndentedTextWriter(new StreamWriter(file));

			var itemStates = ItemFactory.Itemstates;
			List<Itemstate> newItems = new List<Itemstate>();
			foreach (Itemstate state in itemStates)
			{
				var item = ItemFactory.GetItem(state.Id);
				if (item.GetType() == typeof(Item))
				{
					newItems.Add(state);
					Console.WriteLine($"New item: {state.Id}, {state.Name}");
					string clazzName = CodeName(state.Name.Replace("minecraft:", ""), true);

					string baseClazz = "Item";
					baseClazz = clazzName.EndsWith("Axe") ? "ItemAxe" : baseClazz;
					baseClazz = clazzName.EndsWith("Shovel") ? "ItemShovel" : baseClazz;
					baseClazz = clazzName.EndsWith("Pickaxe") ? "ItemPickaxe" : baseClazz;
					baseClazz = clazzName.EndsWith("Hoe") ? "ItemHoe" : baseClazz;
					baseClazz = clazzName.EndsWith("Sword") ? "ItemSword" : baseClazz;
					baseClazz = clazzName.EndsWith("Helmet") ? "ArmorHelmetBase" : baseClazz;
					baseClazz = clazzName.EndsWith("Chestplate") ? "ArmorChestplateBase" : baseClazz;
					baseClazz = clazzName.EndsWith("Leggings") ? "ArmorLeggingsBase" : baseClazz;
					baseClazz = clazzName.EndsWith("Boots") ? "ArmorBootsBase" : baseClazz;

					baseClazz = clazzName.EndsWith("Door") ? "ItemWoodenDoor" : baseClazz;

					writer.WriteLine($"public class Item{clazzName} : {baseClazz} {{ public Item{clazzName}() : base({state.Id}) {{}} }}");
				}
			}
			writer.Flush();

			foreach (Itemstate state in newItems.OrderBy(s => s.Id))
			{
				string clazzName = CodeName(state.Name.Replace("minecraft:", ""), true);
				writer.WriteLine($"else if (id == {state.Id}) item = new Item{clazzName}();");
			}

			writer.Flush();
		}


		[TestMethod]
		public void BlcoksWithBlockstates()
		{
			List<string> blocksWithStates = new List<string>();
			BlockPalette blockPalette = BlockFactory.BlockPalette;
			foreach (BlockStateContainer stateContainer in blockPalette)
			{
				if (stateContainer.States.Count > 0)
				{
					if (stateContainer.States.Count(s => s.Name.Contains("direction")) > 0) blocksWithStates.Add(stateContainer.Name);
					if (stateContainer.States.Count(s => s.Name.Contains("face")) > 0) blocksWithStates.Add(stateContainer.Name);
				}
			}

			foreach (string name in blocksWithStates.OrderBy(n => n).Distinct())
			{
				Console.WriteLine($"{name}");
				foreach (var state in BlockFactory.GetBlockByName(name).GetState().States)
				{
					if (state.Name.Contains("direction")) Console.WriteLine($"\t{state.Name}");
					if (state.Name.Contains("face")) Console.WriteLine($"\t{state.Name}");
				}
			}
		}

		[TestMethod]
		public void GenerateMissingBlocks()
		{
			foreach (var block in BlockFactory.BlockStates)
			{
				var b = BlockFactory.GetBlockByName(block.Name);
				if (b == null)
				{
					Console.WriteLine($"Missing {block.Name}");
					continue;
				}


				b.SetState(block.States);
				//block.RuntimeId
			}
		}

		[TestMethod]
		public void GeneratePartialBlocksFromBlockstates()
		{
			var blockPalette = BlockFactory.BlockPalette;

			string fileName = Path.GetTempPath() + "MissingBlocks_" + Guid.NewGuid() + ".txt";
			using (FileStream file = File.OpenWrite(fileName))
			{
				var blocks = new List<(int, string)>();

				var writer = new IndentedTextWriter(new StreamWriter(file));

				Console.WriteLine($"Directory:\n{Path.GetTempPath()}");
				Console.WriteLine($"Filename:\n{fileName}");
				Log.Warn($"Writing blocks to filename:\n{fileName}");

				writer.WriteLine($"namespace MiNET.Blocks");
				writer.WriteLine($"{{");
				writer.Indent++;

				foreach (IGrouping<string, BlockStateContainer> blockstateGrouping in blockPalette.OrderBy(record => record.Name).ThenBy(record => record.Data).GroupBy(record => record.Name))
				{
					var currentBlockState = blockstateGrouping.First();
					var defaultBlockState = blockstateGrouping.FirstOrDefault(bs => bs.Data == 0);

					Log.Debug($"{currentBlockState.RuntimeId}, {currentBlockState.Name}, {currentBlockState.Data}");
					Block blockById = BlockFactory.GetBlockById(currentBlockState.Id);
					bool existingBlock = blockById.GetType() != typeof(Block) && !blockById.IsGenerated;
					int id = existingBlock ? currentBlockState.Id : -1;

					string blockClassName = CodeName(currentBlockState.Name.Replace("minecraft:", ""), true);

					blocks.Add((blockById.Id, blockClassName));

					writer.WriteLineNoTabs($"");

					writer.WriteLine($"public partial class {blockClassName} {(existingBlock ? "" : ": Block")} // {blockById.Id} typeof={blockById.GetType().Name}");
					writer.WriteLine($"{{");
					writer.Indent++;

					var bits = new List<BlockStateByte>();
					foreach (var state in blockstateGrouping.First().States)
					{
						var q = blockstateGrouping.SelectMany(c => c.States);

						// If this is on base, skip this property. We need this to implement common functionality.
						Type baseType = blockById.GetType().BaseType;
						bool propOverride = baseType != null
											&& ("Block" != baseType.Name
												&& baseType.GetProperty(CodeName(state.Name, true)) != null);

						switch (state)
						{
							case BlockStateByte blockStateByte:
							{
								var values = q.Where(s => s.Name == state.Name).Select(d => ((BlockStateByte) d).Value).Distinct().OrderBy(s => s).ToList();
								byte defaultVal = ((BlockStateByte) defaultBlockState?.States.First(s => s.Name == state.Name))?.Value ?? 0;
								if (values.Min() == 0 && values.Max() == 1)
								{
									bits.Add(blockStateByte);
									writer.Write($"[StateBit] ");
									writer.WriteLine($"public {(propOverride ? "override" : "")} bool {CodeName(state.Name, true)} {{ get; set; }} = {(defaultVal == 1 ? "true" : "false")};");
								}
								else
								{
									writer.Write($"[StateRange({values.Min()}, {values.Max()})] ");
									writer.WriteLine($"public {(propOverride ? "override" : "")} byte {CodeName(state.Name, true)} {{ get; set; }} = {defaultVal};");
								}
								break;
							}
							case BlockStateInt blockStateInt:
							{
								var values = q.Where(s => s.Name == state.Name).Select(d => ((BlockStateInt) d).Value).Distinct().OrderBy(s => s).ToList();
								int defaultVal = ((BlockStateInt) defaultBlockState?.States.First(s => s.Name == state.Name))?.Value ?? 0;
								writer.Write($"[StateRange({values.Min()}, {values.Max()})] ");
								writer.WriteLine($"public {(propOverride ? "override" : "")} int {CodeName(state.Name, true)} {{ get; set; }} = {defaultVal};");
								break;
							}
							case BlockStateString blockStateString:
							{
								var values = q.Where(s => s.Name == state.Name).Select(d => ((BlockStateString) d).Value).Distinct().ToList();
								string defaultVal = ((BlockStateString) defaultBlockState?.States.First(s => s.Name == state.Name))?.Value ?? "";
								if (values.Count > 1)
								{
									writer.WriteLine($"[StateEnum({string.Join(',', values.Select(v => $"\"{v}\""))})]");
								}
								writer.WriteLine($"public {(propOverride ? "override" : "")} string {CodeName(state.Name, true)} {{ get; set; }} = \"{defaultVal}\";");
								break;
							}
							default:
								throw new ArgumentOutOfRangeException(nameof(state));
						}
					}

					if (id == -1 || blockById.IsGenerated)
					{
						writer.WriteLine($"");

						writer.WriteLine($"public {blockClassName}() : base({currentBlockState.Id})");
						writer.WriteLine($"{{");
						writer.Indent++;
						writer.WriteLine($"IsGenerated = true;");
						writer.Indent--;
						writer.WriteLine($"}}");
					}

					writer.WriteLineNoTabs($"");
					writer.WriteLine($"public override void SetState(List<IBlockState> states)");
					writer.WriteLine($"{{");
					writer.Indent++;
					writer.WriteLine($"foreach (var state in states)");
					writer.WriteLine($"{{");
					writer.Indent++;
					writer.WriteLine($"switch(state)");
					writer.WriteLine($"{{");
					writer.Indent++;

					foreach (var state in blockstateGrouping.First().States)
					{
						writer.WriteLine($"case {state.GetType().Name} s when s.Name == \"{state.Name}\":");
						writer.Indent++;
						writer.WriteLine($"{CodeName(state.Name, true)} = {(bits.Contains(state) ? "Convert.ToBoolean(s.Value)" : "s.Value")};");
						writer.WriteLine($"break;");
						writer.Indent--;
					}

					writer.Indent--;
					writer.WriteLine($"}} // switch");
					writer.Indent--;
					writer.WriteLine($"}} // foreach");
					writer.Indent--;
					writer.WriteLine($"}} // method");

					writer.WriteLineNoTabs($"");
					writer.WriteLine($"public override BlockStateContainer GetState()");
					writer.WriteLine($"{{");
					writer.Indent++;
					writer.WriteLine($"var record = new BlockStateContainer();");
					writer.WriteLine($"record.Name = \"{blockstateGrouping.First().Name}\";");
					writer.WriteLine($"record.Id = {blockstateGrouping.First().Id};");
					foreach (var state in blockstateGrouping.First().States)
					{
						string propName = CodeName(state.Name, true);
						writer.WriteLine($"record.States.Add(new {state.GetType().Name} {{Name = \"{state.Name}\", Value = {(bits.Contains(state) ? $"Convert.ToByte({propName})" : propName)}}});");
					}
					writer.WriteLine($"return record;");
					writer.Indent--;
					writer.WriteLine($"}} // method");

					//writer.WriteLine($"");

					//writer.WriteLine($"public byte GetMetadataFromState()");
					//writer.WriteLine($"{{");
					//writer.Indent++;

					//writer.WriteLine($"switch(this)");
					//writer.WriteLine($"{{");
					//writer.Indent++;


					//i = 0;
					//foreach (var record in message.BlockPalette.Where(b => b.Id == enumerator.Current.Id).OrderBy(b => b.Data))
					//{
					//	//case { } b when b.ButtonPressedBit == 0 && b.FacingDirection == 0:
					//	//	return 0;

					//	writer.Write($"case {{ }} b when true");
					//	string retVal = "";
					//	foreach (var state in record.States.OrderBy(s => s.Name).ThenBy(s => s.Value))
					//	{
					//		if (state.Type == (byte) NbtTagType.Byte)
					//		{
					//			writer.Write($" && b.{Client.CodeName(state.Name, true)} == {state.Value}");
					//		}
					//		else if (state.Type == (byte) NbtTagType.Int)
					//		{
					//			writer.Write($" && b.{Client.CodeName(state.Name, true)} == {state.Value}");
					//		}
					//		else if (state.Type == (byte) NbtTagType.String)
					//		{
					//			writer.Write($" && b.{Client.CodeName(state.Name, true)} == \"{state.Value}\"");
					//		}
					//	}
					//	writer.WriteLine($":");

					//	writer.Indent++;
					//	writer.WriteLine($"return { i++ };");
					//	writer.Indent--;
					//}

					//writer.Indent--;
					//writer.WriteLine($"}} // switch");

					//writer.WriteLine($"throw new ArithmeticException(\"Invalid state. Unable to convert state to valid metadata\");");

					//writer.Indent--;
					//writer.WriteLine($"}} // method");

					writer.Indent--;
					writer.WriteLine($"}} // class");
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

		public string CodeName(string name, bool firstUpper = false)
		{
			//name = name.ToLowerInvariant();

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