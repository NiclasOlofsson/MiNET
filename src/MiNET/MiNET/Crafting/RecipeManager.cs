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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2018 Niclas Olofsson. 
// All Rights Reserved.

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using MiNET.Blocks;
using MiNET.Inventory;
using MiNET.Items;
using MiNET.Net;
using MiNET.Net.Crafting;
using MiNET.Utils;
using MiNET.Worlds;
using Newtonsoft.Json;

namespace MiNET.Crafting
{
	public class RecipeManager
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(RecipeManager));

		private static int _recipeUniqueIdCounter = 1;

		public static Dictionary<int, Recipe> NetworkIdRecipeMap { get; } = new Dictionary<int, Recipe>();
		public static Dictionary<UUID, Recipe> IdRecipeMap { get; } = new Dictionary<UUID, Recipe>();
		public static Dictionary<int, SmeltingRecipeBase> SmeltingRecipes { get; } = new Dictionary<int, SmeltingRecipeBase>();
		public static Recipes Recipes { get; private set; }

		private static McpeWrapper _craftingData;

		public static McpeWrapper GetCraftingData()
		{
			if (_craftingData == null)
			{
				var craftingData = McpeCraftingData.CreateObject();
				craftingData.recipes = Recipes;
				//craftingData.isClean = true;
				var packet = Level.CreateMcpeBatch(craftingData.Encode());
				craftingData.PutPool();
				packet.MarkPermanent(true);
				_craftingData = packet;
			}

			return _craftingData;
		}

		static RecipeManager()
		{
			Recipes = new Recipes();

			LoadShapedRecipes();
			//LoadShapedChemistryRecipes(); // Edu only
			LoadShapelessRecipes();
			LoadShapelessShulkerBoxRecipes();
			//LoadShapelessChemistryRecipes(); // Edu only
			LoadSmeltingRecipes();

			Recipes.Add(new MultiRecipe() { Id = new UUID("442d85ed-8272-4543-a6f1-418f90ded05d"), UniqueId = _recipeUniqueIdCounter++ }); // 442d85ed-8272-4543-a6f1-418f90ded05d
			Recipes.Add(new MultiRecipe() { Id = new UUID("8b36268c-1829-483c-a0f1-993b7156a8f2"), UniqueId = _recipeUniqueIdCounter++ }); // 8b36268c-1829-483c-a0f1-993b7156a8f2
			Recipes.Add(new MultiRecipe() { Id = new UUID("602234e4-cac1-4353-8bb7-b1ebff70024b"), UniqueId = _recipeUniqueIdCounter++ }); // 602234e4-cac1-4353-8bb7-b1ebff70024b
			Recipes.Add(new MultiRecipe() { Id = new UUID("98c84b38-1085-46bd-b1ce-dd38c159e6cc"), UniqueId = _recipeUniqueIdCounter++ }); // 98c84b38-1085-46bd-b1ce-dd38c159e6cc
			Recipes.Add(new MultiRecipe() { Id = new UUID("d81aaeaf-e172-4440-9225-868df030d27b"), UniqueId = _recipeUniqueIdCounter++ }); // d81aaeaf-e172-4440-9225-868df030d27b
			Recipes.Add(new MultiRecipe() { Id = new UUID("b5c5d105-75a2-4076-af2b-923ea2bf4bf0"), UniqueId = _recipeUniqueIdCounter++ }); // b5c5d105-75a2-4076-af2b-923ea2bf4bf0
			Recipes.Add(new MultiRecipe() { Id = new UUID("00000000-0000-0000-0000-000000000002"), UniqueId = _recipeUniqueIdCounter++ }); // 00000000-0000-0000-0000-000000000002
			Recipes.Add(new MultiRecipe() { Id = new UUID("d1ca6b84-338e-4f2f-9c6b-76cc8b4bd98d"), UniqueId = _recipeUniqueIdCounter++ }); // d1ca6b84-338e-4f2f-9c6b-76cc8b4bd98d
			Recipes.Add(new MultiRecipe() { Id = new UUID("85939755-ba10-4d9d-a4cc-efb7a8e943c4"), UniqueId = _recipeUniqueIdCounter++ }); // 85939755-ba10-4d9d-a4cc-efb7a8e943c4
			Recipes.Add(new MultiRecipe() { Id = new UUID("d392b075-4ba1-40ae-8789-af868d56f6ce"), UniqueId = _recipeUniqueIdCounter++ }); // d392b075-4ba1-40ae-8789-af868d56f6ce
			Recipes.Add(new MultiRecipe() { Id = new UUID("00000000-0000-0000-0000-000000000001"), UniqueId = _recipeUniqueIdCounter++ }); // 00000000-0000-0000-0000-000000000001
			Recipes.Add(new MultiRecipe() { Id = new UUID("aecd2294-4b94-434b-8667-4499bb2c9327"), UniqueId = _recipeUniqueIdCounter++ }); // aecd2294-4b94-434b-8667-4499bb2c9327
		}

		public static bool ValidateRecipe(Recipe recipe, List<Item> input, int times, out List<Item> resultItems, out Item[] consumeItems)
		{
			resultItems = null;
			consumeItems = null;

			return recipe switch
			{
				ShapedRecipe shapedRecipe => ValidateRecipe(shapedRecipe, input, times, out resultItems, out consumeItems),
				ShapelessRecipe shapedRecipe => ValidateRecipe(shapedRecipe, input, times, out resultItems, out consumeItems),

				_ => false
			};
		}

		public static bool TryGetSmeltingResult(Item input, string block, out Item output)
		{
			if (input is ItemAir)
			{
				output = null;
				return false;
			}

			var hash1 = HashCode.Combine(input, block);
			var hash2 = HashCode.Combine(input.RuntimeId, block);

			output = (SmeltingRecipes.GetValueOrDefault(hash1) ?? SmeltingRecipes.GetValueOrDefault(hash2))?.Output;

			return output != null;
		}

		private static bool ValidateRecipe(ShapedRecipe recipe, List<Item> input, int times, out List<Item> resultItems, out Item[] consumeItems)
		{
			consumeItems = new Item[input.Count];
			resultItems = new List<Item>();

			var inputClone = input.ToList();
			for (var i = 0; i < recipe.Input.Length; i++)
			{
				var ingredient = recipe.Input[i];

				if (ingredient is RecipeAirIngredient) continue;

				var count = ingredient.Count * times;

				Item item = null;
				int index = 0;
				for (; index < inputClone.Count; index++)
				{
					item = inputClone[index];
					if (item == null || item is ItemAir) continue;
					if (ingredient.ValidateItem(item)) break;
				}

				if (index >= inputClone.Count) return false;
				if (item.Count < count) return false;

				item = item.Clone() as Item;
				item.Count = (byte) count;
				consumeItems[index] = item;
				inputClone[index] = null;
			}

			foreach (var item in recipe.Output)
			{
				var resultItem = item.Clone() as Item;
				resultItem.Count = (byte) (resultItem.Count * times);

				resultItems.Add(resultItem);
			}

			return true;
		}

		private static bool ValidateRecipe(ShapelessRecipe recipe, List<Item> input, int times, out List<Item> resultItems, out Item[] consumeItems)
		{
			consumeItems = new Item[input.Count];
			resultItems = new List<Item>();

			var inputClone = input.ToList();

			for (var i = 0; i < recipe.Input.Count; i++)
			{
				var ingredient = recipe.Input[i];
				var count = ingredient.Count * times;

				var item = inputClone.FirstOrDefault(ingredient.ValidateItem);
				if (item == null) return false;
				if (item.Count < count) return false;

				item = item.Clone() as Item;
				item.Count = (byte) count;
				consumeItems[inputClone.IndexOf(item)] = item;

				if (!inputClone.Remove(item)) return false;
			}

			foreach (var item in recipe.Output)
			{
				var resultItem = item.Clone() as Item;
				resultItem.Count = (byte) (resultItem.Count * times);

				resultItems.Add(resultItem);
			}

			return true;
		}

		private static void LoadShapelessChemistryRecipes()
		{
			LoadShapelessRecipesBase("shapeless_chemistry.json", recipe =>
				new ShapelessChemistryRecipe(recipe.Output, recipe.Input, recipe.Block)
				{
					Priority = recipe.Priority,
					UniqueId = recipe.UniqueId
				});
		}

		private static void LoadShapelessShulkerBoxRecipes()
		{
			LoadShapelessRecipesBase("shapeless_shulker_box.json", recipe => 
				new ShapelessShulkerBoxRecipe(recipe.Output, recipe.Input, recipe.Block)
				{
					Priority = recipe.Priority,
					UniqueId = recipe.UniqueId
				});
		}

		private static void LoadShapelessRecipes()
		{
			LoadShapelessRecipesBase("shapeless_crafting.json", recipe => recipe);
		}

		private static void LoadShapelessRecipesBase(string source, Func<ShapelessRecipeBase, ShapelessRecipeBase> getRecipe)
		{
			var shapelessCrafting = ResourceUtil.ReadResource<List<ShapelessRecipeData>>(source, typeof(RecipeManager), "Data");

			foreach (var recipeData in shapelessCrafting)
			{
				var input = recipeData.Input.Select(data =>
				{
					TryGetRecipeIngredientFromExternalData(data, out var ingredient);

					return ingredient;
				}).ToList();

				if (input.Any(val => val == null))
				{
					Log.Warn($"Missing shapeless recipe Inputs: {JsonConvert.SerializeObject(recipeData)}");

					continue;
				}

				var output = recipeData.Output.Select(data =>
				{
					InventoryUtils.TryGetItemFromExternalData(data, out var item);

					return item;
				}).ToList();

				if (output.Any(val => val == null))
				{
					Log.Warn($"Missing shapeless recipe Outputs: {JsonConvert.SerializeObject(recipeData)}");

					continue;
				}

				ShapelessRecipeBase recipe = new ShapelessRecipe(output, input, recipeData.Block) { Priority = recipeData.Priority, UniqueId = _recipeUniqueIdCounter++ };
				recipe = getRecipe(recipe);
				NetworkIdRecipeMap.Add(recipe.UniqueId, recipe);
				IdRecipeMap.Add(recipe.Id, recipe);
				Recipes.Add(recipe);
			}
		}

		private static void LoadShapedChemistryRecipes()
		{
			LoadShapedRecipesBase("shaped_chemistry.json", recipe =>
			new ShapedChemistryRecipe(recipe.Width, recipe.Height, recipe.Output, recipe.Input, recipe.Block) 
			{ 
				Priority = recipe.Priority, 
				UniqueId = recipe.UniqueId 
			});
		}


		private static void LoadShapedRecipes()
		{
			LoadShapedRecipesBase("shaped_crafting.json", recipe => recipe);
		}

		private static void LoadShapedRecipesBase(string source, Func<ShapedRecipeBase, ShapedRecipeBase> getRecipe)
		{
			var shapedCrafting = ResourceUtil.ReadResource<List<ShapedRecipeData>>(source, typeof(RecipeManager), "Data");

			foreach (var recipeData in shapedCrafting)
			{
				var input = recipeData.Input.ToDictionary(pair => pair.Key, pair =>
				{
					TryGetRecipeIngredientFromExternalData(pair.Value, out var ingredient);

					return ingredient;
				});

				if (input.Values.Any(val => val == null))
				{
					Log.Warn($"Missing shaped recipe Inputs: {JsonConvert.SerializeObject(recipeData)}");

					continue;
				}

				var output = recipeData.Output.Select(data =>
				{
					InventoryUtils.TryGetItemFromExternalData(data, out var item);

					return item;
				}).ToList();

				if (output.Any(val => val == null))
				{
					Log.Warn($"Missing shaped recipe Outputs: {JsonConvert.SerializeObject(recipeData)}");

					continue;
				}

				var height = recipeData.Shape.Length;
				var width = recipeData.Shape.First().Length;


				var inputShape = new List<RecipeIngredient>();
				for (var i = 0; i < height; i++)
				{
					for (var j = 0; j < width; j++)
					{
						if (input.TryGetValue(recipeData.Shape[i][j].ToString(), out var ingredient))
						{
							inputShape.Add(ingredient);
						}
						else
						{
							inputShape.Add(new RecipeAirIngredient());
						}
					}
				}

				ShapedRecipeBase recipe = new ShapedRecipe(width, height, output, inputShape.ToArray(), recipeData.Block) { Priority = recipeData.Priority, UniqueId = _recipeUniqueIdCounter++ };
				recipe = getRecipe(recipe);
				NetworkIdRecipeMap.Add(recipe.UniqueId, recipe);
				IdRecipeMap.Add(recipe.Id, recipe);
				Recipes.Add(recipe);
			}
		}

		private static void LoadSmeltingRecipes()
		{
			var shapedCrafting = ResourceUtil.ReadResource<List<SmeltingRecipeData>>("smelting.json", typeof(RecipeManager), "Data");

			foreach (var recipeData in shapedCrafting)
			{
				if (!InventoryUtils.TryGetItemFromExternalData(recipeData.Input, out var input))
				{
					Log.Warn($"Missing smelting recipe Input: {JsonConvert.SerializeObject(recipeData)}");

					continue;
				}

				if (!InventoryUtils.TryGetItemFromExternalData(recipeData.Output, out var output))
				{
					Log.Warn($"Missing smelting recipe Output: {JsonConvert.SerializeObject(recipeData)}");

					continue;
				}

				SmeltingRecipeBase recipe = recipeData.Input.Metadata == short.MaxValue ? new SmeltingRecipe() : new SmeltingDataRecipe();

				recipe.Block = recipeData.Block;
				recipe.Input = input;
				recipe.Output = output;

				if (SmeltingRecipes.TryAdd(recipe.GetHashCode(), recipe))
				{
					IdRecipeMap.Add(recipe.Id, recipe);
					Recipes.Add(recipe);
				}
			}
		}

		private static bool TryGetRecipeIngredientFromExternalData(ExternalDataItem itemData, out RecipeIngredient recipeIngredient)
		{
			recipeIngredient = null;

			if (!string.IsNullOrEmpty(itemData.Tag))
			{
				if (!ItemFactory.ItemTags.ContainsKey(itemData.Tag)) return false;

				recipeIngredient = new RecipeTagIngredient(itemData.Tag);
				return true;
			}

			if (!string.IsNullOrEmpty(itemData.Id))
			{
				if (!InventoryUtils.TryGetItemFromExternalData(itemData, out var item)) return false;

				recipeIngredient = new RecipeItemIngredient(item.Id, item.Metadata, item.Count);

				return true;
			}

			return false;
		}
	}
}