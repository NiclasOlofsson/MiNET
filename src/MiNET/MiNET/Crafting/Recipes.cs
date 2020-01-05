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
using MiNET.Items;
using MiNET.Net;

namespace MiNET.Crafting
{
	public class Recipes : List<Recipe>
	{
	}

	public abstract class Recipe
	{
		public UUID Id { get; set; } = new UUID(Guid.NewGuid().ToString());
		public string Block { get; set; }
	}

	public class MultiRecipe : Recipe
	{
	}

	public class ShapelessRecipe : Recipe
	{
		public List<Item> Input { get; private set; }
		public List<Item> Result { get; private set; }

		public ShapelessRecipe()
		{
			Input = new List<Item>();
			Result = new List<Item>();
		}

		public ShapelessRecipe(List<Item> result, List<Item> input, string block = null) : this()
		{
			Result = result;
			Input = input;
			Block = block;
		}

		public ShapelessRecipe(Item result, List<Item> input, string block = null) : this()
		{
			Result.Add(result);
			Input = input;
			Block = block;
		}

	}

	public class ShapedRecipe : Recipe
	{
		public int Width { get; set; }
		public int Height { get; set; }
		public Item[] Input { get; set; }
		public List<Item> Result { get; set; }

		public ShapedRecipe(int width, int height)
		{
			Width = width;
			Height = height;
			Input = new Item[Width * height];
			Result = new List<Item>();
		}

		public ShapedRecipe(int width, int height, Item result, Item[] input, string block = null) : this(width, height)
		{
			Result.Add(result);
			Input = input;
			Block = block;
		}

		public ShapedRecipe(int width, int height, List<Item> result, Item[] input, string block = null) : this(width, height)
		{
			Result = result;
			Input = input;
			Block = block;
		}

	}

	public class SmeltingRecipe : Recipe
	{
		public Item Input { get; set; }
		public Item Result { get; set; }

		public SmeltingRecipe()
		{
		}

		public SmeltingRecipe(Item result, Item input, string block = null) : this()
		{
			Result = result;
			Input = input;
			Block = block;
		}
	}

	public class PotionContainerChangeRecipe
	{
		public int InputItemId { get; set; }
		public int IngredientItemId { get; set; }
		public int OutputItemId { get; set; }
	}

	public class PotionTypeRecipe
	{
		public int InputPotionType { get; set; }
		public int IngredientItemId { get; set; }
		public int OutputPotionType { get; set; }
	}

}