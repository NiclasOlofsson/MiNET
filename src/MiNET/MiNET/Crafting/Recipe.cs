using System.Collections.Generic;
using log4net;
using MiNET.Items;
using MiNET.Net;
using MiNET.Utils;

namespace MiNET.Crafting
{
	public class RecipeManager
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (RecipeManager));

		public static Recipes Recipes { get; private set; }

		static RecipeManager()
		{
			Recipes = new Recipes
			{
				new ShapelessRecipe(new ItemStack(ItemFactory.GetItem(282, 0), 1),
					new List<ItemStack>
					{
						new ItemStack(ItemFactory.GetItem(39, -1), 1),
						new ItemStack(ItemFactory.GetItem(40, -1), 1),
						new ItemStack(ItemFactory.GetItem(281, 0), 1),
					}),
				new ShapedRecipe(3, 3, new ItemStack(ItemFactory.GetItem(282, 0), 1),
					new Item[]
					{
						ItemFactory.GetItem(39, -1),
						ItemFactory.GetItem(40, -1),
						ItemFactory.GetItem(281, 0),
					}),
			};
		}

		public static void Add(Recipe recipe)
		{
			Log.InfoFormat("{0}", recipe.Id);
		}
	}

	public class Recipes : List<Recipe>
	{
	}

	public abstract class Recipe
	{
		public UUID Id { get; set; }
	}

	public class ShapelessRecipe : Recipe
	{
		public List<ItemStack> Input { get; private set; }
		public ItemStack Result { get; set; }

		public ShapelessRecipe()
		{
			Input = new List<ItemStack>();
		}

		public ShapelessRecipe(ItemStack result, List<ItemStack> input) : this()
		{
			Result = result;
			Input = input;
		}
	}

	public class ShapedRecipe : Recipe
	{
		public int Width { get; set; }
		public int Height { get; set; }
		public Item[] Input { get; set; }
		public ItemStack Result { get; set; }

		public ShapedRecipe(int width, int height)
		{
			Width = width;
			Height = height;
			Input = new Item[Width*height];
		}

		public ShapedRecipe(int width, int height, ItemStack result, Item[] input) : this(width, height)
		{
			Result = result;
			Input = input;
		}
	}

	public class SmeltingRecipe : Recipe
	{
		public Item Input { get; set; }
		public ItemStack Result { get; set; }
	}
}