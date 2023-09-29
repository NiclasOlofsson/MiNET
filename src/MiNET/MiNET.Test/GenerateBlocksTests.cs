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
	//, Ignore("Manual code generation")
	]
	public class GenerateBlocksTests
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(GenerateBlocksTests));


		[TestMethod]
		public void GetItemByName()
		{
			// TODO - 1.19-update

			//foreach (KeyValuePair<string, int> keyValuePair in ItemFactory.IdToRuntimeId)
			//{
			//	if(keyValuePair.Key.Equals("sapling")) Console.WriteLine(keyValuePair.Key);
			//}
			//var itemId = ItemFactory.GetItemIdByName("minecraft:sapling");
			//Assert.AreEqual(6, itemId);

			//Item item = ItemFactory.GetItem("minecraft:sapling");
			//Assert.AreEqual("minecraft:sapling", item.Name);
			//Assert.IsNotNull(item as ItemBlock);
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
			foreach (var state in itemStates)
			{
				Item item = ItemFactory.GetItem(state.Value.RuntimeId);
				if(!string.IsNullOrEmpty(item.Id)) continue;

				string clazzName = GenerationUtils.CodeName(state.Key.Replace("minecraft:", ""), true);
				string minecraftName = state.Key;


				writer.WriteLine($"public Item{clazzName}() : base(\"{minecraftName}\", {state.Value.RuntimeId})");
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
			var newItems = new Dictionary<string, Itemstate>();
			foreach (var state in itemStates)
			{
				var item = ItemFactory.GetItem(state.Value.RuntimeId);
				if (item.GetType() == typeof(Item))
				{
					newItems.Add(state.Key, state.Value);
					Console.WriteLine($"New item: {state.Value.RuntimeId}, {state.Key}");
					string clazzName = GenerationUtils.CodeName(state.Key.Replace("minecraft:", ""), true);

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

					writer.WriteLine($"public class Item{clazzName} : {baseClazz} {{ public Item{clazzName}() : base({state.Value.RuntimeId}) {{}} }}");
				}
			}
			writer.Flush();

			foreach (var state in newItems.OrderBy(s => s.Value.RuntimeId))
			{
				string clazzName = GenerationUtils.CodeName(state.Key.Replace("minecraft:", ""), true);
				writer.WriteLine($"else if (id == {state.Value.RuntimeId}) item = new Item{clazzName}();");
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
					if (stateContainer.States.Count(s => s.Name.Contains("direction")) > 0) blocksWithStates.Add(stateContainer.Id);
					if (stateContainer.States.Count(s => s.Name.Contains("face")) > 0) blocksWithStates.Add(stateContainer.Id);
				}
			}

			foreach (string name in blocksWithStates.OrderBy(n => n).Distinct())
			{
				Console.WriteLine($"{name}");
				foreach (var state in BlockFactory.GetBlockById(name).GetState().States)
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
				var b = BlockFactory.GetBlockById(block.Id);
				if (b == null)
				{
					Console.WriteLine($"Missing {block.Id}");
					continue;
				}


				b.SetState(block.States);
				//block.RuntimeId
			}
		}

		[TestMethod]
		public void GeneratePartialBlocksFromBlockstates()
		{
			var assembly = typeof(Block).Assembly;

			var blockPalette = BlockFactory.BlockPalette;
			var blockRecordsGrouping = blockPalette
					.OrderBy(record => record.Id)
					.ThenBy(record => record.Data)
					.GroupBy(record => record.Id);

			var idToTag = ItemFactory.ItemTags
				.SelectMany(tag => tag.Value.Select(itemId => (itemId, tag: tag.Key)))
				.GroupBy(tag => tag.itemId)
				.ToDictionary(pairs => pairs.Key, pairs => pairs.Select(pair => pair.tag).ToArray());

			var fileName = Path.GetTempPath() + "MissingBlocks_" + Guid.NewGuid() + ".txt";
			using (var file = File.OpenWrite(fileName))
			{
				var writer = new IndentedTextWriter(new StreamWriter(file));

				Console.WriteLine($"Directory:\n{Path.GetTempPath()}");
				Console.WriteLine($"Filename:\n{fileName}");
				Log.Warn($"Writing blocks to filename:\n{fileName}");

				writer.WriteLine("using System;");
				writer.WriteLine("using System.Collections.Generic;");
				writer.WriteLine("using MiNET.Utils;");
				writer.WriteLineNoTabs($"");

				writer.WriteLine($"namespace MiNET.Blocks");
				writer.WriteLine($"{{");
				writer.Indent++;

				foreach (var blockstateGrouping in blockRecordsGrouping)
				{
					var currentBlockState = blockstateGrouping.First();
					var defaultBlockState = blockstateGrouping.FirstOrDefault(bs => bs.Data == 0);

					var id = currentBlockState.Id;
					var name = id.Replace("minecraft:", "");
					var className = GenerationUtils.CodeName(name, true);

					var baseName = GetBaseTypeByKnownBlockIds(id, idToTag);

					var baseClassPart = string.Empty;
					var existingType = assembly.GetType($"MiNET.Blocks.{className}");
					var baseType = assembly.GetType($"MiNET.Blocks.{baseName}");
					if (existingType == null
						|| existingType.BaseType == baseType
						|| existingType.BaseType == typeof(object)
						|| existingType.BaseType == typeof(Block))
					{
						baseClassPart = $" : {baseName}";
					}
					else
					{
						baseType = existingType.BaseType;
					}


					writer.WriteLineNoTabs($"");
					writer.WriteLine($"public partial class {className}{baseClassPart}");
					writer.WriteLine($"{{");
					writer.Indent++;

					writer.WriteLine($"public override string Id {{ get; protected set; }} = \"{currentBlockState.Id}\";");

					var bits = new List<BlockStateByte>();
					foreach (var state in currentBlockState.States)
					{
						var q = blockstateGrouping.SelectMany(c => c.States);

						var propertyName = GenerationUtils.CodeName(state.Name, true);

						// If this is on base, skip this property. We need this to implement common functionality.
						var propertyOverride = baseType != null
											&& baseType != typeof(Block)
											&& baseType.GetProperty(propertyName) != null;
						var propertyOverrideModifierPart = propertyOverride ? $" override" : string.Empty;

						switch (state)
						{
							case BlockStateByte blockStateByte:
							{
								writer.WriteLineNoTabs($"");

								var values = q.Where(s => s.Name == state.Name).Select(d => ((BlockStateByte) d).Value).Distinct().OrderBy(s => s).ToList();
								var defaultVal = ((BlockStateByte) defaultBlockState?.States.First(s => s.Name == state.Name))?.Value ?? 0;
								if (values.Min() == 0 && values.Max() == 1)
								{
									bits.Add(blockStateByte);
									writer.WriteLine($"[StateBit]");
									writer.WriteLine($"public{propertyOverrideModifierPart} bool {propertyName} {{ get; set; }} = {(defaultVal == 1 ? "true" : "false")};");
								}
								else
								{
									writer.WriteLine($"[StateRange({values.Min()}, {values.Max()})]");
									writer.WriteLine($"public{propertyOverrideModifierPart} byte {propertyName} {{ get; set; }} = {defaultVal};");
								}
								break;
							}
							case BlockStateInt blockStateInt:
							{
								writer.WriteLineNoTabs($"");

								var values = q.Where(s => s.Name == state.Name).Select(d => ((BlockStateInt) d).Value).Distinct().OrderBy(s => s).ToList();
								int defaultVal = ((BlockStateInt) defaultBlockState?.States.First(s => s.Name == state.Name))?.Value ?? 0;
								writer.WriteLine($"[StateRange({values.Min()}, {values.Max()})]");
								writer.WriteLine($"public{propertyOverrideModifierPart} int {propertyName} {{ get; set; }} = {defaultVal};");
								break;
							}
							case BlockStateString blockStateString:
							{
								writer.WriteLineNoTabs($"");

								var values = q.Where(s => s.Name == state.Name).Select(d => ((BlockStateString) d).Value).Distinct().ToList();
								string defaultVal = ((BlockStateString) defaultBlockState?.States.First(s => s.Name == state.Name))?.Value ?? string.Empty;
								if (values.Count > 1)
								{
									writer.WriteLine($"[StateEnum({string.Join(',', values.Select(v => $"\"{v}\""))})]");
								}
								writer.WriteLine($"public{propertyOverrideModifierPart} string {propertyName} {{ get; set; }} = \"{defaultVal}\";");
								break;
							}
							default:
								throw new ArgumentOutOfRangeException(nameof(state));
						}
					}

					if (currentBlockState.States.Any())
					{
						writer.WriteLineNoTabs($"");
						writer.WriteLine($"public override void {nameof(Block.SetState)}(List<{nameof(IBlockState)}> states)");
						writer.WriteLine($"{{");
						writer.Indent++;
						writer.WriteLine($"foreach (var state in states)");
						writer.WriteLine($"{{");
						writer.Indent++;
						writer.WriteLine($"switch(state)");
						writer.WriteLine($"{{");
						writer.Indent++;

						foreach (var state in currentBlockState.States)
						{
							writer.WriteLine($"case {state.GetType().Name} s when s.Name == \"{state.Name}\":");
							writer.Indent++;
							writer.WriteLine($"{GenerationUtils.CodeName(state.Name, true)} = {(bits.Contains(state) ? "Convert.ToBoolean(s.Value)" : "s.Value")};");
							writer.WriteLine($"break;");
							writer.Indent--;
						}

						writer.Indent--;
						writer.WriteLine($"}} // switch");
						writer.Indent--;
						writer.WriteLine($"}} // foreach");
						writer.Indent--;
						writer.WriteLine($"}} // method");
					}

					writer.WriteLineNoTabs($"");
					writer.WriteLine($"public override {nameof(BlockStateContainer)} {nameof(Block.GetState)}()");
					writer.WriteLine($"{{");
					writer.Indent++;
					writer.WriteLine($"var record = new {nameof(BlockStateContainer)}();");
					writer.WriteLine($"record.Id = {nameof(Block.Id)};");
					foreach (var state in currentBlockState.States)
					{
						string propName = GenerationUtils.CodeName(state.Name, true);
						writer.WriteLine($"record.States.Add(new {state.GetType().Name} {{Name = \"{state.Name}\", Value = {(bits.Contains(state) ? $"Convert.ToByte({propName})" : propName)}}});");
					}
					writer.WriteLine($"return record;");
					writer.Indent--;
					writer.WriteLine($"}} // method");

					writer.Indent--;
					writer.WriteLine($"}} // class");
				}

				writer.Indent--;
				writer.WriteLine($"}}");

				writer.Flush();
			}
		}

		private string GetBaseTypeByKnownBlockIds(string id, Dictionary<string, string[]> idToTag)
		{
			var name = id.Replace("minecraft:", "");

			if (idToTag.TryGetValue(id, out var tags))
			{
				foreach (var tag in tags)
				{
					switch (tag.Replace("minecraft:", ""))
					{
						case "logs":
							return typeof(LogBase).Name;
					}
				}
			}

			if (id.Contains("_double_slab") || (name.StartsWith("double_") && name.EndsWith("_slab")))
			{
				//return typeof(DoubleSlabBase).Name;
			}
			else if (id.Contains("_slab"))
			{
				return typeof(SlabBase).Name;
			}

			return "Block";
		}
	}
}