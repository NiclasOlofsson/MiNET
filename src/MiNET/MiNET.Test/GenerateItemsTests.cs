using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Linq;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiNET.Blocks;
using MiNET.Items;

namespace MiNET.Test
{
	[TestClass]
	//[Ignore("Manual code generation")]
	public class GenerateItemsTests
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(GenerateItemsTests));

		[TestMethod]
		public void GeneratePartialItemsFromItemStates()
		{
			var assembly = typeof(Item).Assembly;

			var itemStates = ItemFactory.Itemstates;

			var idToTag = ItemFactory.ItemTags
				.SelectMany(tag => tag.Value.Select(itemId => (itemId, tag: tag.Key)))
				.GroupBy(tag => tag.itemId)
				.ToDictionary(pairs => pairs.Key, pairs => pairs.Select(pair => pair.tag).ToArray());

			string fileName = Path.GetTempPath() + "MissingItems_" + Guid.NewGuid() + ".txt";
			using (FileStream file = File.OpenWrite(fileName))
			{
				var writer = new IndentedTextWriter(new StreamWriter(file));

				Console.WriteLine($"Directory:\n{Path.GetTempPath()}");
				Console.WriteLine($"Filename:\n{fileName}");
				Log.Warn($"Writing items to filename:\n{fileName}");

				writer.WriteLine($"namespace MiNET.Items");
				writer.WriteLine($"{{");
				writer.Indent++;

				foreach (var state in itemStates)
				{
					var id = state.Key;

					if (BlockFactory.GetBlockById(id) != null) continue;

					var associatedBlockId = BlockFactory.GetBlockIdFromItemId(id);
					if (associatedBlockId != null && itemStates.ContainsKey(associatedBlockId)) id = associatedBlockId;

					var name = id.Replace("minecraft:", "").Replace("item.", "");
					var className = $"Item{GenerationUtils.CodeName(name, true)}";
					var baseName = associatedBlockId == null ? "Item" : "ItemBlock";
					ItemType? type = null;
					ItemMaterial? material = null;
					string blockClassName = associatedBlockId == null ? null : GenerationUtils.CodeName(associatedBlockId.Replace("minecraft:", ""), true);
					var maxStackSize = 64;

					if (idToTag.TryGetValue(id, out var tags))
					{
						foreach (var tag in tags)
						{
							switch (tag.Replace("minecraft:", ""))
							{
								case "boat":
								case "boats":
									baseName = typeof(ItemBoatBase).Name;
									break;
								case "spawn_egg":
									baseName = typeof(ItemSpawnEggBase).Name;
									break;
								case "bookshelf_books":
									type = ItemType.Book;
									break;
								case "door":
									baseName = typeof(ItemDoorBase).Name;
									break;
								case "is_axe":
									baseName = typeof(ItemAxeBase).Name;
									type = ItemType.Axe;
									break;
								case "is_hoe":
									baseName = typeof(ItemHoeBase).Name;
									type = ItemType.Hoe;
									break;
								case "is_pickaxe":
									baseName = typeof(ItemPickaxeBase).Name;
									type = ItemType.PickAxe;
									break;
								case "is_shovel":
									baseName = typeof(ItemShovelBase).Name;
									type = ItemType.Shovel;
									break;
								case "is_sword":
									baseName = typeof(ItemSwordBase).Name;
									type = ItemType.Sword;
									break;
								case "is_armor":
									maxStackSize = 1;
									baseName = Enum.Parse<ItemType>(name.Split('_').Last(), true) switch
									{
										ItemType.Helmet => typeof(ItemArmorHelmetBase).Name,
										ItemType.Chestplate => typeof(ItemArmorChestplateBase).Name,
										ItemType.Leggings => typeof(ItemArmorLeggingsBase).Name,
										ItemType.Boots => typeof(ItemArmorBootsBase).Name,
										_ => baseName
									};
									break;
								case "horse_armor":
									baseName = typeof(ItemHorseArmorBase).Name;
									maxStackSize = 1;
									material = id switch
									{
										"minecraft:diamond_horse_armor" => ItemMaterial.Diamond,
										"minecraft:golden_horse_armor" => ItemMaterial.Gold,
										"minecraft:iron_horse_armor" => ItemMaterial.Iron,
										"minecraft:leather_horse_armor" => ItemMaterial.Leather,
									};
									break;

								case "diamond_tier":
									material = ItemMaterial.Diamond;
									break;
								case "golden_tier":
									material = ItemMaterial.Gold;
									break;
								case "leather_tier":
									material = ItemMaterial.Leather;
									break;
								case "stone_tier":
									material = ItemMaterial.Stone;
									break;
								case "chainmail_tier":
									material = ItemMaterial.Chain;
									break;
								case "iron_tier":
									material = ItemMaterial.Iron;
									break;
								case "netherite_tier":
									material = ItemMaterial.Netherite;
									break;
								case "wooden_tier":
									material = ItemMaterial.Wood;
									break;
								case "is_tool":
									maxStackSize = 1;
									break;
								case "sign":
									baseName = typeof(ItemSignBase).Name;
									break;
								case "is_food":
									baseName = typeof(FoodItemBase).Name;
									break;
							}

							if (type == null)
							{
								if (Enum.TryParse<ItemType>(name.Split('_').Last(), true, out var itemType))
								{
									type = itemType;
								}
							}
						}
					}

					var baseClassPart = string.Empty;
					var existingType = assembly.GetType($"MiNET.Items.{className}");
					var baseType = assembly.GetType($"MiNET.Items.{baseName}");
					if (existingType == null 
						|| existingType.BaseType == baseType
						|| existingType.BaseType == typeof(object)
						|| existingType.BaseType == typeof(Item)
						|| (existingType.BaseType == typeof(ItemBlock) && (baseType?.IsAssignableTo(typeof(ItemBlock)) ?? false)))
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
					
					writer.WriteLine($"public override string {nameof(Item.Id)} {{ get; protected set; }} = \"{id}\";");

					if (!string.IsNullOrEmpty(blockClassName) && !string.IsNullOrEmpty(baseClassPart) && (baseType?.IsAssignableTo(typeof(ItemBlock)) ?? false))
					{
						writer.WriteLineNoTabs($"");
						writer.WriteLine($"public override {nameof(Block)} {nameof(ItemBlock.Block)} {{ get; protected set; }} = new {blockClassName}();");
					}

					if (type != null)
					{
						writer.WriteLineNoTabs($"");
						writer.WriteLine($"public override {nameof(ItemType)} {nameof(Item.ItemType)} {{ get; set; }} = {nameof(ItemType)}.{type};");
					}

					if (material != null)
					{
						writer.WriteLineNoTabs($"");
						writer.WriteLine($"public override {nameof(ItemMaterial)} {nameof(Item.ItemMaterial)} {{ get; set; }} = {nameof(ItemMaterial)}.{material};");
					}

					if (maxStackSize != 64)
					{
						writer.WriteLineNoTabs($"");
						writer.WriteLine($"public override int {nameof(Item.MaxStackSize)} {{ get; set; }} = {maxStackSize};");
					}

					writer.Indent--;
					writer.WriteLine($"}}");
				}

				writer.Indent--;
				writer.WriteLine($"}}");

				writer.Flush();
			}
		}
	}
}
