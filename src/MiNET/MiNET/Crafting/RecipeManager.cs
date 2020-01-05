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

using System.Collections.Generic;
using log4net;
using MiNET.Items;
using MiNET.Net;
using MiNET.Worlds;

namespace MiNET.Crafting
{
	public class RecipeManager
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(RecipeManager));

		public static Recipes Recipes { get; private set; }

		private static McpeWrapper _craftingData;

		public static McpeWrapper GetCraftingData()
		{
			if (_craftingData == null)
			{
				McpeCraftingData craftingData = McpeCraftingData.CreateObject();
				craftingData.recipes = Recipes;
				//craftingData.someArraySize = 0;
				//craftingData.someArraySize2 = 0;
				craftingData.isClean = true;
				var packet = Level.CreateMcpeBatch(craftingData.Encode());
				packet.MarkPermanent(true);
				_craftingData = packet;
			}

			return _craftingData;
		}

		// GENERATED CODE. DON'T EDIT BY HAND
		static RecipeManager()
		{
			Recipes = new Recipes
			{
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(47, 0),
					},
					new Item[]
					{
						new Item(5, 32767),
						new Item(340, 32767),
						new Item(5, 32767),
						new Item(5, 32767),
						new Item(340, 32767),
						new Item(5, 32767),
						new Item(5, 32767),
						new Item(340, 32767),
						new Item(5, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(281, 0),
					},
					new Item[]
					{
						new Item(5, 32767),
						new Item(5, 32767),
						new Item(5, 32767),
						new Item(0, 0),
						new Item(0, 0),
						new Item(0, 0),
					}, "crafting_table"),
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(-140, 0),
					},
					new Item[]
					{
						new Item(5, 4),
					}, "crafting_table"),
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(-141, 0),
					},
					new Item[]
					{
						new Item(5, 2),
					}, "crafting_table"),
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(-142, 0),
					},
					new Item[]
					{
						new Item(5, 5),
					}, "crafting_table"),
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(-143, 0),
					},
					new Item[]
					{
						new Item(5, 3),
					}, "crafting_table"),
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(-144, 0),
					},
					new Item[]
					{
						new Item(5, 1),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(54, 0),
					},
					new Item[]
					{
						new Item(5, 32767),
						new Item(5, 32767),
						new Item(5, 32767),
						new Item(5, 32767),
						new Item(0, 0),
						new Item(5, 32767),
						new Item(5, 32767),
						new Item(5, 32767),
						new Item(5, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(151, 0),
					},
					new Item[]
					{
						new Item(20, 0),
						new Item(406, 32767),
						new Item(158, 32767),
						new Item(20, 0),
						new Item(406, 32767),
						new Item(158, 32767),
						new Item(20, 0),
						new Item(406, 32767),
						new Item(158, 32767),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(385, 0, 3),
					},
					new List<Item>
					{
						new Item(377, 32767, 1),
						new Item(263, 1, 1),
						new Item(289, 32767, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(385, 0, 3),
					},
					new List<Item>
					{
						new Item(377, 32767, 1),
						new Item(263, 32767, 1),
						new Item(289, 32767, 1),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(84, 0),
					},
					new Item[]
					{
						new Item(5, 32767),
						new Item(5, 32767),
						new Item(5, 32767),
						new Item(5, 32767),
						new Item(264, 32767),
						new Item(5, 32767),
						new Item(5, 32767),
						new Item(5, 32767),
						new Item(5, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(25, 0),
					},
					new Item[]
					{
						new Item(5, 32767),
						new Item(5, 32767),
						new Item(5, 32767),
						new Item(5, 32767),
						new Item(331, 32767),
						new Item(5, 32767),
						new Item(5, 32767),
						new Item(5, 32767),
						new Item(5, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(44, 3),
					},
					new Item[]
					{
						new Item(4, 0),
						new Item(4, 0),
						new Item(4, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(44, 7),
					},
					new Item[]
					{
						new Item(112, 0),
						new Item(112, 0),
						new Item(112, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(44, 1),
					},
					new Item[]
					{
						new Item(24, 0),
						new Item(24, 0),
						new Item(24, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(321, 0),
					},
					new Item[]
					{
						new Item(280, 32767),
						new Item(280, 32767),
						new Item(280, 32767),
						new Item(280, 32767),
						new Item(35, 32767),
						new Item(280, 32767),
						new Item(280, 32767),
						new Item(280, 32767),
						new Item(280, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(33, 1),
					},
					new Item[]
					{
						new Item(5, 32767),
						new Item(4, 0),
						new Item(4, 0),
						new Item(5, 32767),
						new Item(265, 32767),
						new Item(331, 32767),
						new Item(5, 32767),
						new Item(4, 0),
						new Item(4, 0),
					}, "crafting_table"),
				new ShapedRecipe(2, 1,
					new List<Item>
					{
						new Item(-150, 0),
					},
					new Item[]
					{
						new Item(5, 4),
						new Item(5, 4),
					}, "crafting_table"),
				new ShapedRecipe(2, 1,
					new List<Item>
					{
						new Item(-151, 0),
					},
					new Item[]
					{
						new Item(5, 2),
						new Item(5, 2),
					}, "crafting_table"),
				new ShapedRecipe(2, 1,
					new List<Item>
					{
						new Item(-152, 0),
					},
					new Item[]
					{
						new Item(5, 5),
						new Item(5, 5),
					}, "crafting_table"),
				new ShapedRecipe(2, 1,
					new List<Item>
					{
						new Item(-153, 0),
					},
					new Item[]
					{
						new Item(5, 3),
						new Item(5, 3),
					}, "crafting_table"),
				new ShapedRecipe(2, 1,
					new List<Item>
					{
						new Item(-154, 0),
					},
					new Item[]
					{
						new Item(5, 1),
						new Item(5, 1),
					}, "crafting_table"),
				new ShapedRecipe(1, 2,
					new List<Item>
					{
						new Item(280, 0),
					},
					new Item[]
					{
						new Item(-163, 0),
						new Item(-163, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(-166, 2),
					},
					new Item[]
					{
						new Item(1, 0),
						new Item(1, 0),
						new Item(1, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(-166, 0),
					},
					new Item[]
					{
						new Item(98, 1),
						new Item(98, 1),
						new Item(98, 1),
					}, "crafting_table"),
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(44, 4),
					},
					new Item[]
					{
						new Item(45, 0),
						new Item(45, 0),
						new Item(45, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(44, 5),
					},
					new Item[]
					{
						new Item(98, 0),
						new Item(98, 0),
						new Item(98, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(44, 0),
					},
					new Item[]
					{
						new Item(-183, 0),
						new Item(-183, 0),
						new Item(-183, 0),
					}, "crafting_table"),
				new ShapedRecipe(1, 2,
					new List<Item>
					{
						new Item(50, 0),
					},
					new Item[]
					{
						new Item(263, 32767),
						new Item(280, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(-145, 0),
					},
					new Item[]
					{
						new Item(5, 4),
						new Item(5, 4),
						new Item(5, 4),
						new Item(5, 4),
						new Item(5, 4),
						new Item(5, 4),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(-146, 0),
					},
					new Item[]
					{
						new Item(5, 2),
						new Item(5, 2),
						new Item(5, 2),
						new Item(5, 2),
						new Item(5, 2),
						new Item(5, 2),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(-147, 0),
					},
					new Item[]
					{
						new Item(5, 5),
						new Item(5, 5),
						new Item(5, 5),
						new Item(5, 5),
						new Item(5, 5),
						new Item(5, 5),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(-148, 0),
					},
					new Item[]
					{
						new Item(5, 3),
						new Item(5, 3),
						new Item(5, 3),
						new Item(5, 3),
						new Item(5, 3),
						new Item(5, 3),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(-149, 0),
					},
					new Item[]
					{
						new Item(5, 1),
						new Item(5, 1),
						new Item(5, 1),
						new Item(5, 1),
						new Item(5, 1),
						new Item(5, 1),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(96, 0),
					},
					new Item[]
					{
						new Item(5, 0),
						new Item(5, 0),
						new Item(5, 0),
						new Item(5, 0),
						new Item(5, 0),
						new Item(5, 0),
					}, "crafting_table"),
				new ShapedRecipe(1, 3,
					new List<Item>
					{
						new Item(131, 0),
					},
					new Item[]
					{
						new Item(265, 32767),
						new Item(280, 32767),
						new Item(5, 32767),
					}, "crafting_table"),
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(143, 0),
					},
					new Item[]
					{
						new Item(5, 0),
					}, "crafting_table"),
				new ShapedRecipe(2, 1,
					new List<Item>
					{
						new Item(72, 0),
					},
					new Item[]
					{
						new Item(5, 0),
						new Item(5, 0),
					}, "crafting_table"),
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(58, 0),
					},
					new Item[]
					{
						new Item(5, 32767),
						new Item(5, 32767),
						new Item(5, 32767),
						new Item(5, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(163, 0),
					},
					new Item[]
					{
						new Item(5, 4),
						new Item(5, 4),
						new Item(5, 4),
						new Item(0, 0),
						new Item(5, 4),
						new Item(5, 4),
						new Item(0, 0),
						new Item(0, 0),
						new Item(5, 4),
					}, "crafting_table"),
				new MultiRecipe() { Id = new UUID("d81aaeaf-e172-4440-9225-868df030d27b") }, // d81aaeaf-e172-4440-9225-868df030d27b
				new MultiRecipe() { Id = new UUID("b5c5d105-75a2-4076-af2b-923ea2bf4bf0") }, // b5c5d105-75a2-4076-af2b-923ea2bf4bf0
				new MultiRecipe() { Id = new UUID("00000000-0000-0000-0000-000000000002") }, // 00000000-0000-0000-0000-000000000002
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(355, 0),
					},
					new Item[]
					{
						new Item(35, 0),
						new Item(35, 0),
						new Item(5, 32767),
						new Item(35, 0),
						new Item(5, 32767),
						new Item(5, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(355, 1),
					},
					new Item[]
					{
						new Item(35, 1),
						new Item(35, 1),
						new Item(5, 32767),
						new Item(35, 1),
						new Item(5, 32767),
						new Item(5, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(355, 10),
					},
					new Item[]
					{
						new Item(35, 10),
						new Item(35, 10),
						new Item(5, 32767),
						new Item(35, 10),
						new Item(5, 32767),
						new Item(5, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(355, 11),
					},
					new Item[]
					{
						new Item(35, 11),
						new Item(35, 11),
						new Item(5, 32767),
						new Item(35, 11),
						new Item(5, 32767),
						new Item(5, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(355, 12),
					},
					new Item[]
					{
						new Item(35, 12),
						new Item(35, 12),
						new Item(5, 32767),
						new Item(35, 12),
						new Item(5, 32767),
						new Item(5, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(355, 13),
					},
					new Item[]
					{
						new Item(35, 13),
						new Item(35, 13),
						new Item(5, 32767),
						new Item(35, 13),
						new Item(5, 32767),
						new Item(5, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(355, 14),
					},
					new Item[]
					{
						new Item(35, 14),
						new Item(35, 14),
						new Item(5, 32767),
						new Item(35, 14),
						new Item(5, 32767),
						new Item(5, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(355, 15),
					},
					new Item[]
					{
						new Item(35, 15),
						new Item(35, 15),
						new Item(5, 32767),
						new Item(35, 15),
						new Item(5, 32767),
						new Item(5, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(355, 2),
					},
					new Item[]
					{
						new Item(35, 2),
						new Item(35, 2),
						new Item(5, 32767),
						new Item(35, 2),
						new Item(5, 32767),
						new Item(5, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(355, 3),
					},
					new Item[]
					{
						new Item(35, 3),
						new Item(35, 3),
						new Item(5, 32767),
						new Item(35, 3),
						new Item(5, 32767),
						new Item(5, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(355, 4),
					},
					new Item[]
					{
						new Item(35, 4),
						new Item(35, 4),
						new Item(5, 32767),
						new Item(35, 4),
						new Item(5, 32767),
						new Item(5, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(355, 5),
					},
					new Item[]
					{
						new Item(35, 5),
						new Item(35, 5),
						new Item(5, 32767),
						new Item(35, 5),
						new Item(5, 32767),
						new Item(5, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(355, 6),
					},
					new Item[]
					{
						new Item(35, 6),
						new Item(35, 6),
						new Item(5, 32767),
						new Item(35, 6),
						new Item(5, 32767),
						new Item(5, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(355, 7),
					},
					new Item[]
					{
						new Item(35, 7),
						new Item(35, 7),
						new Item(5, 32767),
						new Item(35, 7),
						new Item(5, 32767),
						new Item(5, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(355, 8),
					},
					new Item[]
					{
						new Item(35, 8),
						new Item(35, 8),
						new Item(5, 32767),
						new Item(35, 8),
						new Item(5, 32767),
						new Item(5, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(355, 9),
					},
					new Item[]
					{
						new Item(35, 9),
						new Item(35, 9),
						new Item(5, 32767),
						new Item(35, 9),
						new Item(5, 32767),
						new Item(5, 32767),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 15, 1),
					},
					new List<Item>
					{
						new Item(355, 14, 1),
						new Item(351, 0, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 15, 1),
					},
					new List<Item>
					{
						new Item(355, 5, 1),
						new Item(351, 0, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 15, 1),
					},
					new List<Item>
					{
						new Item(355, 4, 1),
						new Item(351, 0, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 15, 1),
					},
					new List<Item>
					{
						new Item(355, 3, 1),
						new Item(351, 0, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 15, 1),
					},
					new List<Item>
					{
						new Item(355, 2, 1),
						new Item(351, 0, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 15, 1),
					},
					new List<Item>
					{
						new Item(355, 1, 1),
						new Item(351, 0, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 15, 1),
					},
					new List<Item>
					{
						new Item(355, 0, 1),
						new Item(351, 0, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 15, 1),
					},
					new List<Item>
					{
						new Item(355, 13, 1),
						new Item(351, 0, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 15, 1),
					},
					new List<Item>
					{
						new Item(355, 12, 1),
						new Item(351, 0, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 15, 1),
					},
					new List<Item>
					{
						new Item(355, 11, 1),
						new Item(351, 0, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 15, 1),
					},
					new List<Item>
					{
						new Item(355, 10, 1),
						new Item(351, 0, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 15, 1),
					},
					new List<Item>
					{
						new Item(355, 9, 1),
						new Item(351, 0, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 15, 1),
					},
					new List<Item>
					{
						new Item(355, 8, 1),
						new Item(351, 0, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 15, 1),
					},
					new List<Item>
					{
						new Item(355, 7, 1),
						new Item(351, 0, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 15, 1),
					},
					new List<Item>
					{
						new Item(355, 6, 1),
						new Item(351, 0, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 5, 1),
					},
					new List<Item>
					{
						new Item(355, 15, 1),
						new Item(351, 10, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 5, 1),
					},
					new List<Item>
					{
						new Item(355, 14, 1),
						new Item(351, 10, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 5, 1),
					},
					new List<Item>
					{
						new Item(355, 4, 1),
						new Item(351, 10, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 5, 1),
					},
					new List<Item>
					{
						new Item(355, 3, 1),
						new Item(351, 10, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 5, 1),
					},
					new List<Item>
					{
						new Item(355, 2, 1),
						new Item(351, 10, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 5, 1),
					},
					new List<Item>
					{
						new Item(355, 1, 1),
						new Item(351, 10, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 5, 1),
					},
					new List<Item>
					{
						new Item(355, 0, 1),
						new Item(351, 10, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 5, 1),
					},
					new List<Item>
					{
						new Item(355, 13, 1),
						new Item(351, 10, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 5, 1),
					},
					new List<Item>
					{
						new Item(355, 12, 1),
						new Item(351, 10, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 5, 1),
					},
					new List<Item>
					{
						new Item(355, 11, 1),
						new Item(351, 10, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 5, 1),
					},
					new List<Item>
					{
						new Item(355, 10, 1),
						new Item(351, 10, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 5, 1),
					},
					new List<Item>
					{
						new Item(355, 9, 1),
						new Item(351, 10, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 5, 1),
					},
					new List<Item>
					{
						new Item(355, 8, 1),
						new Item(351, 10, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 5, 1),
					},
					new List<Item>
					{
						new Item(355, 7, 1),
						new Item(351, 10, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 5, 1),
					},
					new List<Item>
					{
						new Item(355, 6, 1),
						new Item(351, 10, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 4, 1),
					},
					new List<Item>
					{
						new Item(355, 15, 1),
						new Item(351, 11, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 4, 1),
					},
					new List<Item>
					{
						new Item(355, 14, 1),
						new Item(351, 11, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 4, 1),
					},
					new List<Item>
					{
						new Item(355, 5, 1),
						new Item(351, 11, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 4, 1),
					},
					new List<Item>
					{
						new Item(355, 3, 1),
						new Item(351, 11, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 4, 1),
					},
					new List<Item>
					{
						new Item(355, 2, 1),
						new Item(351, 11, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 4, 1),
					},
					new List<Item>
					{
						new Item(355, 1, 1),
						new Item(351, 11, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 4, 1),
					},
					new List<Item>
					{
						new Item(355, 0, 1),
						new Item(351, 11, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 4, 1),
					},
					new List<Item>
					{
						new Item(355, 13, 1),
						new Item(351, 11, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 4, 1),
					},
					new List<Item>
					{
						new Item(355, 12, 1),
						new Item(351, 11, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 4, 1),
					},
					new List<Item>
					{
						new Item(355, 11, 1),
						new Item(351, 11, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 4, 1),
					},
					new List<Item>
					{
						new Item(355, 10, 1),
						new Item(351, 11, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 4, 1),
					},
					new List<Item>
					{
						new Item(355, 9, 1),
						new Item(351, 11, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 4, 1),
					},
					new List<Item>
					{
						new Item(355, 8, 1),
						new Item(351, 11, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 4, 1),
					},
					new List<Item>
					{
						new Item(355, 7, 1),
						new Item(351, 11, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 4, 1),
					},
					new List<Item>
					{
						new Item(355, 6, 1),
						new Item(351, 11, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 3, 1),
					},
					new List<Item>
					{
						new Item(355, 15, 1),
						new Item(351, 12, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 3, 1),
					},
					new List<Item>
					{
						new Item(355, 14, 1),
						new Item(351, 12, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 3, 1),
					},
					new List<Item>
					{
						new Item(355, 5, 1),
						new Item(351, 12, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 3, 1),
					},
					new List<Item>
					{
						new Item(355, 4, 1),
						new Item(351, 12, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 3, 1),
					},
					new List<Item>
					{
						new Item(355, 2, 1),
						new Item(351, 12, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 3, 1),
					},
					new List<Item>
					{
						new Item(355, 1, 1),
						new Item(351, 12, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 3, 1),
					},
					new List<Item>
					{
						new Item(355, 0, 1),
						new Item(351, 12, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 3, 1),
					},
					new List<Item>
					{
						new Item(355, 13, 1),
						new Item(351, 12, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 3, 1),
					},
					new List<Item>
					{
						new Item(355, 12, 1),
						new Item(351, 12, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 3, 1),
					},
					new List<Item>
					{
						new Item(355, 11, 1),
						new Item(351, 12, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 3, 1),
					},
					new List<Item>
					{
						new Item(355, 10, 1),
						new Item(351, 12, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 3, 1),
					},
					new List<Item>
					{
						new Item(355, 9, 1),
						new Item(351, 12, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 3, 1),
					},
					new List<Item>
					{
						new Item(355, 8, 1),
						new Item(351, 12, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 3, 1),
					},
					new List<Item>
					{
						new Item(355, 7, 1),
						new Item(351, 12, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 3, 1),
					},
					new List<Item>
					{
						new Item(355, 6, 1),
						new Item(351, 12, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 2, 1),
					},
					new List<Item>
					{
						new Item(355, 15, 1),
						new Item(351, 13, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 2, 1),
					},
					new List<Item>
					{
						new Item(355, 14, 1),
						new Item(351, 13, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 2, 1),
					},
					new List<Item>
					{
						new Item(355, 5, 1),
						new Item(351, 13, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 2, 1),
					},
					new List<Item>
					{
						new Item(355, 4, 1),
						new Item(351, 13, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 2, 1),
					},
					new List<Item>
					{
						new Item(355, 3, 1),
						new Item(351, 13, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 2, 1),
					},
					new List<Item>
					{
						new Item(355, 1, 1),
						new Item(351, 13, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 2, 1),
					},
					new List<Item>
					{
						new Item(355, 0, 1),
						new Item(351, 13, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 2, 1),
					},
					new List<Item>
					{
						new Item(355, 13, 1),
						new Item(351, 13, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 2, 1),
					},
					new List<Item>
					{
						new Item(355, 12, 1),
						new Item(351, 13, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 2, 1),
					},
					new List<Item>
					{
						new Item(355, 11, 1),
						new Item(351, 13, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 2, 1),
					},
					new List<Item>
					{
						new Item(355, 10, 1),
						new Item(351, 13, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 2, 1),
					},
					new List<Item>
					{
						new Item(355, 9, 1),
						new Item(351, 13, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 2, 1),
					},
					new List<Item>
					{
						new Item(355, 8, 1),
						new Item(351, 13, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 2, 1),
					},
					new List<Item>
					{
						new Item(355, 7, 1),
						new Item(351, 13, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 2, 1),
					},
					new List<Item>
					{
						new Item(355, 6, 1),
						new Item(351, 13, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 1, 1),
					},
					new List<Item>
					{
						new Item(355, 15, 1),
						new Item(351, 14, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 1, 1),
					},
					new List<Item>
					{
						new Item(355, 14, 1),
						new Item(351, 14, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 1, 1),
					},
					new List<Item>
					{
						new Item(355, 5, 1),
						new Item(351, 14, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 1, 1),
					},
					new List<Item>
					{
						new Item(355, 4, 1),
						new Item(351, 14, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 1, 1),
					},
					new List<Item>
					{
						new Item(355, 3, 1),
						new Item(351, 14, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 1, 1),
					},
					new List<Item>
					{
						new Item(355, 2, 1),
						new Item(351, 14, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 1, 1),
					},
					new List<Item>
					{
						new Item(355, 0, 1),
						new Item(351, 14, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 1, 1),
					},
					new List<Item>
					{
						new Item(355, 13, 1),
						new Item(351, 14, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 1, 1),
					},
					new List<Item>
					{
						new Item(355, 12, 1),
						new Item(351, 14, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 1, 1),
					},
					new List<Item>
					{
						new Item(355, 11, 1),
						new Item(351, 14, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 1, 1),
					},
					new List<Item>
					{
						new Item(355, 10, 1),
						new Item(351, 14, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 1, 1),
					},
					new List<Item>
					{
						new Item(355, 9, 1),
						new Item(351, 14, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 1, 1),
					},
					new List<Item>
					{
						new Item(355, 8, 1),
						new Item(351, 14, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 1, 1),
					},
					new List<Item>
					{
						new Item(355, 7, 1),
						new Item(351, 14, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 1, 1),
					},
					new List<Item>
					{
						new Item(355, 6, 1),
						new Item(351, 14, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 0, 1),
					},
					new List<Item>
					{
						new Item(355, 15, 1),
						new Item(351, 15, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 0, 1),
					},
					new List<Item>
					{
						new Item(355, 14, 1),
						new Item(351, 15, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 0, 1),
					},
					new List<Item>
					{
						new Item(355, 5, 1),
						new Item(351, 15, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 0, 1),
					},
					new List<Item>
					{
						new Item(355, 4, 1),
						new Item(351, 15, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 0, 1),
					},
					new List<Item>
					{
						new Item(355, 3, 1),
						new Item(351, 15, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 0, 1),
					},
					new List<Item>
					{
						new Item(355, 2, 1),
						new Item(351, 15, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 0, 1),
					},
					new List<Item>
					{
						new Item(355, 1, 1),
						new Item(351, 15, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 0, 1),
					},
					new List<Item>
					{
						new Item(355, 13, 1),
						new Item(351, 15, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 0, 1),
					},
					new List<Item>
					{
						new Item(355, 12, 1),
						new Item(351, 15, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 0, 1),
					},
					new List<Item>
					{
						new Item(355, 11, 1),
						new Item(351, 15, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 0, 1),
					},
					new List<Item>
					{
						new Item(355, 10, 1),
						new Item(351, 15, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 0, 1),
					},
					new List<Item>
					{
						new Item(355, 9, 1),
						new Item(351, 15, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 0, 1),
					},
					new List<Item>
					{
						new Item(355, 8, 1),
						new Item(351, 15, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 0, 1),
					},
					new List<Item>
					{
						new Item(355, 7, 1),
						new Item(351, 15, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 0, 1),
					},
					new List<Item>
					{
						new Item(355, 6, 1),
						new Item(351, 15, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 15, 1),
					},
					new List<Item>
					{
						new Item(355, 14, 1),
						new Item(351, 16, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 15, 1),
					},
					new List<Item>
					{
						new Item(355, 5, 1),
						new Item(351, 16, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 15, 1),
					},
					new List<Item>
					{
						new Item(355, 4, 1),
						new Item(351, 16, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 15, 1),
					},
					new List<Item>
					{
						new Item(355, 3, 1),
						new Item(351, 16, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 15, 1),
					},
					new List<Item>
					{
						new Item(355, 2, 1),
						new Item(351, 16, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 15, 1),
					},
					new List<Item>
					{
						new Item(355, 1, 1),
						new Item(351, 16, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 15, 1),
					},
					new List<Item>
					{
						new Item(355, 0, 1),
						new Item(351, 16, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 15, 1),
					},
					new List<Item>
					{
						new Item(355, 13, 1),
						new Item(351, 16, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 15, 1),
					},
					new List<Item>
					{
						new Item(355, 12, 1),
						new Item(351, 16, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 15, 1),
					},
					new List<Item>
					{
						new Item(355, 11, 1),
						new Item(351, 16, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 15, 1),
					},
					new List<Item>
					{
						new Item(355, 10, 1),
						new Item(351, 16, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 15, 1),
					},
					new List<Item>
					{
						new Item(355, 9, 1),
						new Item(351, 16, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 15, 1),
					},
					new List<Item>
					{
						new Item(355, 8, 1),
						new Item(351, 16, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 15, 1),
					},
					new List<Item>
					{
						new Item(355, 7, 1),
						new Item(351, 16, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 15, 1),
					},
					new List<Item>
					{
						new Item(355, 6, 1),
						new Item(351, 16, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 12, 1),
					},
					new List<Item>
					{
						new Item(355, 15, 1),
						new Item(351, 17, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 12, 1),
					},
					new List<Item>
					{
						new Item(355, 14, 1),
						new Item(351, 17, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 12, 1),
					},
					new List<Item>
					{
						new Item(355, 5, 1),
						new Item(351, 17, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 12, 1),
					},
					new List<Item>
					{
						new Item(355, 4, 1),
						new Item(351, 17, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 12, 1),
					},
					new List<Item>
					{
						new Item(355, 3, 1),
						new Item(351, 17, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 12, 1),
					},
					new List<Item>
					{
						new Item(355, 2, 1),
						new Item(351, 17, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 12, 1),
					},
					new List<Item>
					{
						new Item(355, 1, 1),
						new Item(351, 17, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 12, 1),
					},
					new List<Item>
					{
						new Item(355, 0, 1),
						new Item(351, 17, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 12, 1),
					},
					new List<Item>
					{
						new Item(355, 13, 1),
						new Item(351, 17, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 12, 1),
					},
					new List<Item>
					{
						new Item(355, 11, 1),
						new Item(351, 17, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 12, 1),
					},
					new List<Item>
					{
						new Item(355, 10, 1),
						new Item(351, 17, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 12, 1),
					},
					new List<Item>
					{
						new Item(355, 9, 1),
						new Item(351, 17, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 12, 1),
					},
					new List<Item>
					{
						new Item(355, 8, 1),
						new Item(351, 17, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 12, 1),
					},
					new List<Item>
					{
						new Item(355, 7, 1),
						new Item(351, 17, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 12, 1),
					},
					new List<Item>
					{
						new Item(355, 6, 1),
						new Item(351, 17, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 11, 1),
					},
					new List<Item>
					{
						new Item(355, 15, 1),
						new Item(351, 18, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 11, 1),
					},
					new List<Item>
					{
						new Item(355, 14, 1),
						new Item(351, 18, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 11, 1),
					},
					new List<Item>
					{
						new Item(355, 5, 1),
						new Item(351, 18, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 11, 1),
					},
					new List<Item>
					{
						new Item(355, 4, 1),
						new Item(351, 18, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 11, 1),
					},
					new List<Item>
					{
						new Item(355, 3, 1),
						new Item(351, 18, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 11, 1),
					},
					new List<Item>
					{
						new Item(355, 2, 1),
						new Item(351, 18, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 11, 1),
					},
					new List<Item>
					{
						new Item(355, 1, 1),
						new Item(351, 18, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 11, 1),
					},
					new List<Item>
					{
						new Item(355, 0, 1),
						new Item(351, 18, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 11, 1),
					},
					new List<Item>
					{
						new Item(355, 13, 1),
						new Item(351, 18, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 11, 1),
					},
					new List<Item>
					{
						new Item(355, 12, 1),
						new Item(351, 18, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 11, 1),
					},
					new List<Item>
					{
						new Item(355, 10, 1),
						new Item(351, 18, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 11, 1),
					},
					new List<Item>
					{
						new Item(355, 9, 1),
						new Item(351, 18, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 11, 1),
					},
					new List<Item>
					{
						new Item(355, 8, 1),
						new Item(351, 18, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 11, 1),
					},
					new List<Item>
					{
						new Item(355, 7, 1),
						new Item(351, 18, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 11, 1),
					},
					new List<Item>
					{
						new Item(355, 6, 1),
						new Item(351, 18, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 0, 1),
					},
					new List<Item>
					{
						new Item(355, 15, 1),
						new Item(351, 19, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 0, 1),
					},
					new List<Item>
					{
						new Item(355, 14, 1),
						new Item(351, 19, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 0, 1),
					},
					new List<Item>
					{
						new Item(355, 5, 1),
						new Item(351, 19, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 0, 1),
					},
					new List<Item>
					{
						new Item(355, 4, 1),
						new Item(351, 19, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 0, 1),
					},
					new List<Item>
					{
						new Item(355, 3, 1),
						new Item(351, 19, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 0, 1),
					},
					new List<Item>
					{
						new Item(355, 2, 1),
						new Item(351, 19, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 0, 1),
					},
					new List<Item>
					{
						new Item(355, 1, 1),
						new Item(351, 19, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 0, 1),
					},
					new List<Item>
					{
						new Item(355, 13, 1),
						new Item(351, 19, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 0, 1),
					},
					new List<Item>
					{
						new Item(355, 12, 1),
						new Item(351, 19, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 0, 1),
					},
					new List<Item>
					{
						new Item(355, 11, 1),
						new Item(351, 19, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 0, 1),
					},
					new List<Item>
					{
						new Item(355, 10, 1),
						new Item(351, 19, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 0, 1),
					},
					new List<Item>
					{
						new Item(355, 9, 1),
						new Item(351, 19, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 0, 1),
					},
					new List<Item>
					{
						new Item(355, 8, 1),
						new Item(351, 19, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 0, 1),
					},
					new List<Item>
					{
						new Item(355, 7, 1),
						new Item(351, 19, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 0, 1),
					},
					new List<Item>
					{
						new Item(355, 6, 1),
						new Item(351, 19, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 14, 1),
					},
					new List<Item>
					{
						new Item(355, 15, 1),
						new Item(351, 1, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 14, 1),
					},
					new List<Item>
					{
						new Item(355, 5, 1),
						new Item(351, 1, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 14, 1),
					},
					new List<Item>
					{
						new Item(355, 4, 1),
						new Item(351, 1, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 14, 1),
					},
					new List<Item>
					{
						new Item(355, 3, 1),
						new Item(351, 1, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 14, 1),
					},
					new List<Item>
					{
						new Item(355, 2, 1),
						new Item(351, 1, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 14, 1),
					},
					new List<Item>
					{
						new Item(355, 1, 1),
						new Item(351, 1, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 14, 1),
					},
					new List<Item>
					{
						new Item(355, 0, 1),
						new Item(351, 1, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 14, 1),
					},
					new List<Item>
					{
						new Item(355, 13, 1),
						new Item(351, 1, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 14, 1),
					},
					new List<Item>
					{
						new Item(355, 12, 1),
						new Item(351, 1, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 14, 1),
					},
					new List<Item>
					{
						new Item(355, 11, 1),
						new Item(351, 1, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 14, 1),
					},
					new List<Item>
					{
						new Item(355, 10, 1),
						new Item(351, 1, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 14, 1),
					},
					new List<Item>
					{
						new Item(355, 9, 1),
						new Item(351, 1, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 14, 1),
					},
					new List<Item>
					{
						new Item(355, 8, 1),
						new Item(351, 1, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 14, 1),
					},
					new List<Item>
					{
						new Item(355, 7, 1),
						new Item(351, 1, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 14, 1),
					},
					new List<Item>
					{
						new Item(355, 6, 1),
						new Item(351, 1, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 13, 1),
					},
					new List<Item>
					{
						new Item(355, 15, 1),
						new Item(351, 2, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 13, 1),
					},
					new List<Item>
					{
						new Item(355, 14, 1),
						new Item(351, 2, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 13, 1),
					},
					new List<Item>
					{
						new Item(355, 5, 1),
						new Item(351, 2, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 13, 1),
					},
					new List<Item>
					{
						new Item(355, 4, 1),
						new Item(351, 2, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 13, 1),
					},
					new List<Item>
					{
						new Item(355, 3, 1),
						new Item(351, 2, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 13, 1),
					},
					new List<Item>
					{
						new Item(355, 2, 1),
						new Item(351, 2, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 13, 1),
					},
					new List<Item>
					{
						new Item(355, 1, 1),
						new Item(351, 2, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 13, 1),
					},
					new List<Item>
					{
						new Item(355, 0, 1),
						new Item(351, 2, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 13, 1),
					},
					new List<Item>
					{
						new Item(355, 12, 1),
						new Item(351, 2, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 13, 1),
					},
					new List<Item>
					{
						new Item(355, 11, 1),
						new Item(351, 2, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 13, 1),
					},
					new List<Item>
					{
						new Item(355, 10, 1),
						new Item(351, 2, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 13, 1),
					},
					new List<Item>
					{
						new Item(355, 9, 1),
						new Item(351, 2, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 13, 1),
					},
					new List<Item>
					{
						new Item(355, 8, 1),
						new Item(351, 2, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 13, 1),
					},
					new List<Item>
					{
						new Item(355, 7, 1),
						new Item(351, 2, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 13, 1),
					},
					new List<Item>
					{
						new Item(355, 6, 1),
						new Item(351, 2, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 12, 1),
					},
					new List<Item>
					{
						new Item(355, 15, 1),
						new Item(351, 3, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 12, 1),
					},
					new List<Item>
					{
						new Item(355, 14, 1),
						new Item(351, 3, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 12, 1),
					},
					new List<Item>
					{
						new Item(355, 5, 1),
						new Item(351, 3, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 12, 1),
					},
					new List<Item>
					{
						new Item(355, 4, 1),
						new Item(351, 3, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 12, 1),
					},
					new List<Item>
					{
						new Item(355, 3, 1),
						new Item(351, 3, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 12, 1),
					},
					new List<Item>
					{
						new Item(355, 2, 1),
						new Item(351, 3, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 12, 1),
					},
					new List<Item>
					{
						new Item(355, 1, 1),
						new Item(351, 3, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 12, 1),
					},
					new List<Item>
					{
						new Item(355, 0, 1),
						new Item(351, 3, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 12, 1),
					},
					new List<Item>
					{
						new Item(355, 13, 1),
						new Item(351, 3, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 12, 1),
					},
					new List<Item>
					{
						new Item(355, 11, 1),
						new Item(351, 3, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 12, 1),
					},
					new List<Item>
					{
						new Item(355, 10, 1),
						new Item(351, 3, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 12, 1),
					},
					new List<Item>
					{
						new Item(355, 9, 1),
						new Item(351, 3, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 12, 1),
					},
					new List<Item>
					{
						new Item(355, 8, 1),
						new Item(351, 3, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 12, 1),
					},
					new List<Item>
					{
						new Item(355, 7, 1),
						new Item(351, 3, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 12, 1),
					},
					new List<Item>
					{
						new Item(355, 6, 1),
						new Item(351, 3, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 11, 1),
					},
					new List<Item>
					{
						new Item(355, 15, 1),
						new Item(351, 4, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 11, 1),
					},
					new List<Item>
					{
						new Item(355, 14, 1),
						new Item(351, 4, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 11, 1),
					},
					new List<Item>
					{
						new Item(355, 5, 1),
						new Item(351, 4, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 11, 1),
					},
					new List<Item>
					{
						new Item(355, 4, 1),
						new Item(351, 4, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 11, 1),
					},
					new List<Item>
					{
						new Item(355, 3, 1),
						new Item(351, 4, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 11, 1),
					},
					new List<Item>
					{
						new Item(355, 2, 1),
						new Item(351, 4, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 11, 1),
					},
					new List<Item>
					{
						new Item(355, 1, 1),
						new Item(351, 4, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 11, 1),
					},
					new List<Item>
					{
						new Item(355, 0, 1),
						new Item(351, 4, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 11, 1),
					},
					new List<Item>
					{
						new Item(355, 13, 1),
						new Item(351, 4, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 11, 1),
					},
					new List<Item>
					{
						new Item(355, 12, 1),
						new Item(351, 4, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 11, 1),
					},
					new List<Item>
					{
						new Item(355, 10, 1),
						new Item(351, 4, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 11, 1),
					},
					new List<Item>
					{
						new Item(355, 9, 1),
						new Item(351, 4, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 11, 1),
					},
					new List<Item>
					{
						new Item(355, 8, 1),
						new Item(351, 4, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 11, 1),
					},
					new List<Item>
					{
						new Item(355, 7, 1),
						new Item(351, 4, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 11, 1),
					},
					new List<Item>
					{
						new Item(355, 6, 1),
						new Item(351, 4, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 10, 1),
					},
					new List<Item>
					{
						new Item(355, 15, 1),
						new Item(351, 5, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 10, 1),
					},
					new List<Item>
					{
						new Item(355, 14, 1),
						new Item(351, 5, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 10, 1),
					},
					new List<Item>
					{
						new Item(355, 5, 1),
						new Item(351, 5, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 10, 1),
					},
					new List<Item>
					{
						new Item(355, 4, 1),
						new Item(351, 5, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 10, 1),
					},
					new List<Item>
					{
						new Item(355, 3, 1),
						new Item(351, 5, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 10, 1),
					},
					new List<Item>
					{
						new Item(355, 2, 1),
						new Item(351, 5, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 10, 1),
					},
					new List<Item>
					{
						new Item(355, 1, 1),
						new Item(351, 5, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 10, 1),
					},
					new List<Item>
					{
						new Item(355, 0, 1),
						new Item(351, 5, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 10, 1),
					},
					new List<Item>
					{
						new Item(355, 13, 1),
						new Item(351, 5, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 10, 1),
					},
					new List<Item>
					{
						new Item(355, 12, 1),
						new Item(351, 5, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 10, 1),
					},
					new List<Item>
					{
						new Item(355, 11, 1),
						new Item(351, 5, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 10, 1),
					},
					new List<Item>
					{
						new Item(355, 9, 1),
						new Item(351, 5, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 10, 1),
					},
					new List<Item>
					{
						new Item(355, 8, 1),
						new Item(351, 5, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 10, 1),
					},
					new List<Item>
					{
						new Item(355, 7, 1),
						new Item(351, 5, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 10, 1),
					},
					new List<Item>
					{
						new Item(355, 6, 1),
						new Item(351, 5, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 9, 1),
					},
					new List<Item>
					{
						new Item(355, 15, 1),
						new Item(351, 6, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 9, 1),
					},
					new List<Item>
					{
						new Item(355, 14, 1),
						new Item(351, 6, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 9, 1),
					},
					new List<Item>
					{
						new Item(355, 5, 1),
						new Item(351, 6, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 9, 1),
					},
					new List<Item>
					{
						new Item(355, 4, 1),
						new Item(351, 6, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 9, 1),
					},
					new List<Item>
					{
						new Item(355, 3, 1),
						new Item(351, 6, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 9, 1),
					},
					new List<Item>
					{
						new Item(355, 2, 1),
						new Item(351, 6, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 9, 1),
					},
					new List<Item>
					{
						new Item(355, 1, 1),
						new Item(351, 6, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 9, 1),
					},
					new List<Item>
					{
						new Item(355, 0, 1),
						new Item(351, 6, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 9, 1),
					},
					new List<Item>
					{
						new Item(355, 13, 1),
						new Item(351, 6, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 9, 1),
					},
					new List<Item>
					{
						new Item(355, 12, 1),
						new Item(351, 6, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 9, 1),
					},
					new List<Item>
					{
						new Item(355, 11, 1),
						new Item(351, 6, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 9, 1),
					},
					new List<Item>
					{
						new Item(355, 10, 1),
						new Item(351, 6, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 9, 1),
					},
					new List<Item>
					{
						new Item(355, 8, 1),
						new Item(351, 6, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 9, 1),
					},
					new List<Item>
					{
						new Item(355, 7, 1),
						new Item(351, 6, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 9, 1),
					},
					new List<Item>
					{
						new Item(355, 6, 1),
						new Item(351, 6, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 8, 1),
					},
					new List<Item>
					{
						new Item(355, 15, 1),
						new Item(351, 7, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 8, 1),
					},
					new List<Item>
					{
						new Item(355, 14, 1),
						new Item(351, 7, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 8, 1),
					},
					new List<Item>
					{
						new Item(355, 5, 1),
						new Item(351, 7, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 8, 1),
					},
					new List<Item>
					{
						new Item(355, 4, 1),
						new Item(351, 7, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 8, 1),
					},
					new List<Item>
					{
						new Item(355, 3, 1),
						new Item(351, 7, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 8, 1),
					},
					new List<Item>
					{
						new Item(355, 2, 1),
						new Item(351, 7, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 8, 1),
					},
					new List<Item>
					{
						new Item(355, 1, 1),
						new Item(351, 7, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 8, 1),
					},
					new List<Item>
					{
						new Item(355, 0, 1),
						new Item(351, 7, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 8, 1),
					},
					new List<Item>
					{
						new Item(355, 13, 1),
						new Item(351, 7, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 8, 1),
					},
					new List<Item>
					{
						new Item(355, 12, 1),
						new Item(351, 7, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 8, 1),
					},
					new List<Item>
					{
						new Item(355, 11, 1),
						new Item(351, 7, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 8, 1),
					},
					new List<Item>
					{
						new Item(355, 10, 1),
						new Item(351, 7, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 8, 1),
					},
					new List<Item>
					{
						new Item(355, 9, 1),
						new Item(351, 7, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 8, 1),
					},
					new List<Item>
					{
						new Item(355, 7, 1),
						new Item(351, 7, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 8, 1),
					},
					new List<Item>
					{
						new Item(355, 6, 1),
						new Item(351, 7, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 7, 1),
					},
					new List<Item>
					{
						new Item(355, 15, 1),
						new Item(351, 8, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 7, 1),
					},
					new List<Item>
					{
						new Item(355, 14, 1),
						new Item(351, 8, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 7, 1),
					},
					new List<Item>
					{
						new Item(355, 5, 1),
						new Item(351, 8, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 7, 1),
					},
					new List<Item>
					{
						new Item(355, 4, 1),
						new Item(351, 8, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 7, 1),
					},
					new List<Item>
					{
						new Item(355, 3, 1),
						new Item(351, 8, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 7, 1),
					},
					new List<Item>
					{
						new Item(355, 2, 1),
						new Item(351, 8, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 7, 1),
					},
					new List<Item>
					{
						new Item(355, 1, 1),
						new Item(351, 8, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 7, 1),
					},
					new List<Item>
					{
						new Item(355, 0, 1),
						new Item(351, 8, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 7, 1),
					},
					new List<Item>
					{
						new Item(355, 13, 1),
						new Item(351, 8, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 7, 1),
					},
					new List<Item>
					{
						new Item(355, 12, 1),
						new Item(351, 8, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 7, 1),
					},
					new List<Item>
					{
						new Item(355, 11, 1),
						new Item(351, 8, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 7, 1),
					},
					new List<Item>
					{
						new Item(355, 10, 1),
						new Item(351, 8, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 7, 1),
					},
					new List<Item>
					{
						new Item(355, 9, 1),
						new Item(351, 8, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 7, 1),
					},
					new List<Item>
					{
						new Item(355, 8, 1),
						new Item(351, 8, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 7, 1),
					},
					new List<Item>
					{
						new Item(355, 6, 1),
						new Item(351, 8, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 6, 1),
					},
					new List<Item>
					{
						new Item(355, 15, 1),
						new Item(351, 9, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 6, 1),
					},
					new List<Item>
					{
						new Item(355, 14, 1),
						new Item(351, 9, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 6, 1),
					},
					new List<Item>
					{
						new Item(355, 5, 1),
						new Item(351, 9, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 6, 1),
					},
					new List<Item>
					{
						new Item(355, 4, 1),
						new Item(351, 9, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 6, 1),
					},
					new List<Item>
					{
						new Item(355, 3, 1),
						new Item(351, 9, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 6, 1),
					},
					new List<Item>
					{
						new Item(355, 2, 1),
						new Item(351, 9, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 6, 1),
					},
					new List<Item>
					{
						new Item(355, 1, 1),
						new Item(351, 9, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 6, 1),
					},
					new List<Item>
					{
						new Item(355, 0, 1),
						new Item(351, 9, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 6, 1),
					},
					new List<Item>
					{
						new Item(355, 13, 1),
						new Item(351, 9, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 6, 1),
					},
					new List<Item>
					{
						new Item(355, 12, 1),
						new Item(351, 9, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 6, 1),
					},
					new List<Item>
					{
						new Item(355, 11, 1),
						new Item(351, 9, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 6, 1),
					},
					new List<Item>
					{
						new Item(355, 10, 1),
						new Item(351, 9, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 6, 1),
					},
					new List<Item>
					{
						new Item(355, 9, 1),
						new Item(351, 9, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 6, 1),
					},
					new List<Item>
					{
						new Item(355, 8, 1),
						new Item(351, 9, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 6, 1),
					},
					new List<Item>
					{
						new Item(355, 7, 1),
						new Item(351, 9, 1),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(135, 0),
					},
					new Item[]
					{
						new Item(5, 2),
						new Item(5, 2),
						new Item(5, 2),
						new Item(0, 0),
						new Item(5, 2),
						new Item(5, 2),
						new Item(0, 0),
						new Item(0, 0),
						new Item(5, 2),
					}, "crafting_table"),
				new MultiRecipe() { Id = new UUID("d1ca6b84-338e-4f2f-9c6b-76cc8b4bd98d") }, // d1ca6b84-338e-4f2f-9c6b-76cc8b4bd98d
				new ShapedRecipe(1, 2,
					new List<Item>
					{
						new Item(155, 1),
					},
					new Item[]
					{
						new Item(44, 6),
						new Item(44, 6),
					}, "crafting_table"),
				new ShapedRecipe(1, 2,
					new List<Item>
					{
						new Item(98, 3),
					},
					new Item[]
					{
						new Item(44, 5),
						new Item(44, 5),
					}, "crafting_table"),
				new MultiRecipe() { Id = new UUID("85939755-ba10-4d9d-a4cc-efb7a8e943c4") }, // 85939755-ba10-4d9d-a4cc-efb7a8e943c4
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(164, 0),
					},
					new Item[]
					{
						new Item(5, 5),
						new Item(5, 5),
						new Item(5, 5),
						new Item(0, 0),
						new Item(5, 5),
						new Item(5, 5),
						new Item(0, 0),
						new Item(0, 0),
						new Item(5, 5),
					}, "crafting_table"),
				new MultiRecipe() { Id = new UUID("d392b075-4ba1-40ae-8789-af868d56f6ce") }, // d392b075-4ba1-40ae-8789-af868d56f6ce
				new ShapedRecipe(1, 2,
					new List<Item>
					{
						new Item(179, 1),
					},
					new Item[]
					{
						new Item(182, 0),
						new Item(182, 0),
					}, "crafting_table"),
				new ShapedRecipe(1, 2,
					new List<Item>
					{
						new Item(24, 1),
					},
					new Item[]
					{
						new Item(44, 1),
						new Item(44, 1),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(136, 0),
					},
					new Item[]
					{
						new Item(5, 3),
						new Item(5, 3),
						new Item(5, 3),
						new Item(0, 0),
						new Item(5, 3),
						new Item(5, 3),
						new Item(0, 0),
						new Item(0, 0),
						new Item(5, 3),
					}, "crafting_table"),
				new ShapedRecipe(1, 2,
					new List<Item>
					{
						new Item(201, 2),
					},
					new Item[]
					{
						new Item(182, 1),
						new Item(182, 1),
					}, "crafting_table"),
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(-204, 0),
					},
					new Item[]
					{
						new Item(287, 32767),
						new Item(5, 32767),
						new Item(287, 32767),
						new Item(5, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(333, 4),
					},
					new Item[]
					{
						new Item(5, 4),
						new Item(5, 4),
						new Item(5, 4),
						new Item(269, 0),
						new Item(5, 4),
						new Item(5, 4),
					}, "crafting_table"),
				new ShapedRecipe(2, 3,
					new List<Item>
					{
						new Item(430, 0),
					},
					new Item[]
					{
						new Item(5, 4),
						new Item(5, 4),
						new Item(5, 4),
						new Item(5, 4),
						new Item(5, 4),
						new Item(5, 4),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(85, 4),
					},
					new Item[]
					{
						new Item(5, 4),
						new Item(5, 4),
						new Item(280, 32767),
						new Item(280, 32767),
						new Item(5, 4),
						new Item(5, 4),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(187, 0),
					},
					new Item[]
					{
						new Item(280, 32767),
						new Item(280, 32767),
						new Item(5, 4),
						new Item(5, 4),
						new Item(280, 32767),
						new Item(280, 32767),
					}, "crafting_table"),
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(5, 4),
					},
					new Item[]
					{
						new Item(162, 0),
					}, "crafting_table"),
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(5, 4),
					},
					new Item[]
					{
						new Item(-8, 32767),
					}, "crafting_table"),
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(5, 4),
					},
					new Item[]
					{
						new Item(-212, 12),
					}, "crafting_table"),
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(5, 4),
					},
					new Item[]
					{
						new Item(-212, 4),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(163, 0),
					},
					new Item[]
					{
						new Item(5, 4),
						new Item(5, 4),
						new Item(5, 4),
						new Item(0, 0),
						new Item(5, 4),
						new Item(5, 4),
						new Item(0, 0),
						new Item(0, 0),
						new Item(5, 4),
					}, "crafting_table"),
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(-212, 4),
					},
					new Item[]
					{
						new Item(162, 0),
						new Item(162, 0),
						new Item(162, 0),
						new Item(162, 0),
					}, "crafting_table"),
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(-212, 12),
					},
					new Item[]
					{
						new Item(-8, 32767),
						new Item(-8, 32767),
						new Item(-8, 32767),
						new Item(-8, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(158, 4),
					},
					new Item[]
					{
						new Item(5, 4),
						new Item(5, 4),
						new Item(5, 4),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(126, 0),
					},
					new Item[]
					{
						new Item(265, 32767),
						new Item(265, 32767),
						new Item(265, 32767),
						new Item(280, 32767),
						new Item(76, 32767),
						new Item(280, 32767),
						new Item(265, 32767),
						new Item(265, 32767),
						new Item(265, 32767),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(1, 5, 2),
					},
					new List<Item>
					{
						new Item(1, 3, 1),
						new Item(4, 32767, 1),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-171, 0),
					},
					new Item[]
					{
						new Item(1, 5),
						new Item(1, 5),
						new Item(1, 5),
						new Item(0, 0),
						new Item(1, 5),
						new Item(1, 5),
						new Item(0, 0),
						new Item(0, 0),
						new Item(1, 5),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(139, 4),
					},
					new Item[]
					{
						new Item(1, 5),
						new Item(1, 5),
						new Item(1, 5),
						new Item(1, 5),
						new Item(1, 5),
						new Item(1, 5),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(145, 0),
					},
					new Item[]
					{
						new Item(42, 32767),
						new Item(0, 0),
						new Item(265, 32767),
						new Item(42, 32767),
						new Item(265, 32767),
						new Item(265, 32767),
						new Item(42, 32767),
						new Item(0, 0),
						new Item(265, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(425, 0),
					},
					new Item[]
					{
						new Item(280, 32767),
						new Item(0, 0),
						new Item(280, 32767),
						new Item(280, 32767),
						new Item(280, 32767),
						new Item(44, 0),
						new Item(280, 32767),
						new Item(0, 0),
						new Item(280, 32767),
					}, "crafting_table"),
				new ShapedRecipe(1, 3,
					new List<Item>
					{
						new Item(262, 0),
					},
					new Item[]
					{
						new Item(318, 32767),
						new Item(280, 32767),
						new Item(288, 32767),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(434, 4, 1),
					},
					new List<Item>
					{
						new Item(339, 32767, 1),
						new Item(45, 32767, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(434, 0, 1),
					},
					new List<Item>
					{
						new Item(339, 32767, 1),
						new Item(397, 4, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(434, 2, 1),
					},
					new List<Item>
					{
						new Item(339, 32767, 1),
						new Item(38, 8, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(434, 1, 1),
					},
					new List<Item>
					{
						new Item(339, 32767, 1),
						new Item(397, 1, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(434, 3, 1),
					},
					new List<Item>
					{
						new Item(339, 32767, 1),
						new Item(466, 32767, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(434, 5, 1),
					},
					new List<Item>
					{
						new Item(339, 32767, 1),
						new Item(106, 32767, 1),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-203, 0),
					},
					new Item[]
					{
						new Item(280, 32767),
						new Item(280, 32767),
						new Item(280, 32767),
						new Item(158, 32767),
						new Item(0, 0),
						new Item(158, 32767),
						new Item(280, 32767),
						new Item(280, 32767),
						new Item(280, 32767),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(395, 2, 1),
					},
					new List<Item>
					{
						new Item(395, 1, 1),
						new Item(345, 32767, 1),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(138, 0),
					},
					new Item[]
					{
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(49, 32767),
						new Item(20, 32767),
						new Item(399, 32767),
						new Item(49, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(49, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-219, 0),
					},
					new Item[]
					{
						new Item(5, 32767),
						new Item(736, 32767),
						new Item(5, 32767),
						new Item(5, 32767),
						new Item(736, 32767),
						new Item(5, 32767),
						new Item(5, 32767),
						new Item(736, 32767),
						new Item(5, 32767),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(459, 0, 1),
					},
					new List<Item>
					{
						new Item(281, 32767, 1),
						new Item(457, 32767, 1),
						new Item(457, 32767, 1),
						new Item(457, 32767, 1),
						new Item(457, 32767, 1),
						new Item(457, 32767, 1),
						new Item(457, 32767, 1),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(333, 2),
					},
					new Item[]
					{
						new Item(5, 2),
						new Item(5, 2),
						new Item(5, 2),
						new Item(269, 0),
						new Item(5, 2),
						new Item(5, 2),
					}, "crafting_table"),
				new ShapedRecipe(2, 3,
					new List<Item>
					{
						new Item(428, 0),
					},
					new Item[]
					{
						new Item(5, 2),
						new Item(5, 2),
						new Item(5, 2),
						new Item(5, 2),
						new Item(5, 2),
						new Item(5, 2),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(85, 2),
					},
					new Item[]
					{
						new Item(5, 2),
						new Item(5, 2),
						new Item(280, 32767),
						new Item(280, 32767),
						new Item(5, 2),
						new Item(5, 2),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(184, 0),
					},
					new Item[]
					{
						new Item(280, 32767),
						new Item(280, 32767),
						new Item(5, 2),
						new Item(5, 2),
						new Item(280, 32767),
						new Item(280, 32767),
					}, "crafting_table"),
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(5, 2),
					},
					new Item[]
					{
						new Item(17, 2),
					}, "crafting_table"),
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(5, 2),
					},
					new Item[]
					{
						new Item(-6, 32767),
					}, "crafting_table"),
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(5, 2),
					},
					new Item[]
					{
						new Item(-212, 10),
					}, "crafting_table"),
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(5, 2),
					},
					new Item[]
					{
						new Item(-212, 2),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(135, 0),
					},
					new Item[]
					{
						new Item(5, 2),
						new Item(5, 2),
						new Item(5, 2),
						new Item(0, 0),
						new Item(5, 2),
						new Item(5, 2),
						new Item(0, 0),
						new Item(0, 0),
						new Item(5, 2),
					}, "crafting_table"),
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(-212, 2),
					},
					new Item[]
					{
						new Item(17, 2),
						new Item(17, 2),
						new Item(17, 2),
						new Item(17, 2),
					}, "crafting_table"),
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(-212, 10),
					},
					new Item[]
					{
						new Item(-6, 32767),
						new Item(-6, 32767),
						new Item(-6, 32767),
						new Item(-6, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(158, 2),
					},
					new Item[]
					{
						new Item(5, 2),
						new Item(5, 2),
						new Item(5, 2),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(446, 0),
					},
					new Item[]
					{
						new Item(35, 15),
						new Item(35, 15),
						new Item(0, 0),
						new Item(35, 15),
						new Item(35, 15),
						new Item(280, 32767),
						new Item(35, 15),
						new Item(35, 15),
						new Item(0, 0),
					}, "crafting_table"),
				new ShapedRecipe(2, 1,
					new List<Item>
					{
						new Item(171, 15),
					},
					new Item[]
					{
						new Item(35, 15),
						new Item(35, 15),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(171, 15),
					},
					new Item[]
					{
						new Item(171, 0),
						new Item(171, 0),
						new Item(171, 0),
						new Item(171, 0),
						new Item(351, 16),
						new Item(171, 0),
						new Item(171, 0),
						new Item(171, 0),
						new Item(171, 0),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(237, 15, 8),
					},
					new List<Item>
					{
						new Item(351, 16, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(13, 32767, 1),
						new Item(13, 32767, 1),
						new Item(13, 32767, 1),
						new Item(13, 32767, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(237, 15, 8),
					},
					new List<Item>
					{
						new Item(351, 0, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(13, 32767, 1),
						new Item(13, 32767, 1),
						new Item(13, 32767, 1),
						new Item(13, 32767, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 16, 1),
					},
					new List<Item>
					{
						new Item(351, 0, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 16, 1),
					},
					new List<Item>
					{
						new Item(-216, 32767, 1),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(241, 15),
					},
					new Item[]
					{
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(351, 16),
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(241, 15),
					},
					new Item[]
					{
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(351, 0),
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(160, 15),
					},
					new Item[]
					{
						new Item(241, 15),
						new Item(241, 15),
						new Item(241, 15),
						new Item(241, 15),
						new Item(241, 15),
						new Item(241, 15),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(160, 15),
					},
					new Item[]
					{
						new Item(102, 32767),
						new Item(102, 32767),
						new Item(102, 32767),
						new Item(102, 32767),
						new Item(351, 16),
						new Item(102, 32767),
						new Item(102, 32767),
						new Item(102, 32767),
						new Item(102, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(159, 15),
					},
					new Item[]
					{
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(351, 16),
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(172, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(159, 15),
					},
					new Item[]
					{
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(351, 0),
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(172, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-196, 0),
					},
					new Item[]
					{
						new Item(265, 32767),
						new Item(265, 32767),
						new Item(-183, 32767),
						new Item(265, 32767),
						new Item(61, 32767),
						new Item(-183, 32767),
						new Item(265, 32767),
						new Item(265, 32767),
						new Item(-183, 32767),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(377, 0, 2),
					},
					new List<Item>
					{
						new Item(369, 32767, 1),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(446, 4),
					},
					new Item[]
					{
						new Item(35, 11),
						new Item(35, 11),
						new Item(0, 0),
						new Item(35, 11),
						new Item(35, 11),
						new Item(280, 32767),
						new Item(35, 11),
						new Item(35, 11),
						new Item(0, 0),
					}, "crafting_table"),
				new ShapedRecipe(2, 1,
					new List<Item>
					{
						new Item(171, 11),
					},
					new Item[]
					{
						new Item(35, 11),
						new Item(35, 11),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(171, 11),
					},
					new Item[]
					{
						new Item(171, 0),
						new Item(171, 0),
						new Item(171, 0),
						new Item(171, 0),
						new Item(351, 18),
						new Item(171, 0),
						new Item(171, 0),
						new Item(171, 0),
						new Item(171, 0),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(237, 11, 8),
					},
					new List<Item>
					{
						new Item(351, 18, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(13, 32767, 1),
						new Item(13, 32767, 1),
						new Item(13, 32767, 1),
						new Item(13, 32767, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(237, 11, 8),
					},
					new List<Item>
					{
						new Item(351, 4, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(13, 32767, 1),
						new Item(13, 32767, 1),
						new Item(13, 32767, 1),
						new Item(13, 32767, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 18, 1),
					},
					new List<Item>
					{
						new Item(38, 9, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 18, 1),
					},
					new List<Item>
					{
						new Item(351, 4, 1),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-11, 0),
					},
					new Item[]
					{
						new Item(174, 32767),
						new Item(174, 32767),
						new Item(174, 32767),
						new Item(174, 32767),
						new Item(174, 32767),
						new Item(174, 32767),
						new Item(174, 32767),
						new Item(174, 32767),
						new Item(174, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(241, 11),
					},
					new Item[]
					{
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(351, 18),
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(241, 11),
					},
					new Item[]
					{
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(351, 4),
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(160, 11),
					},
					new Item[]
					{
						new Item(241, 11),
						new Item(241, 11),
						new Item(241, 11),
						new Item(241, 11),
						new Item(241, 11),
						new Item(241, 11),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(160, 11),
					},
					new Item[]
					{
						new Item(102, 32767),
						new Item(102, 32767),
						new Item(102, 32767),
						new Item(102, 32767),
						new Item(351, 18),
						new Item(102, 32767),
						new Item(102, 32767),
						new Item(102, 32767),
						new Item(102, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(159, 11),
					},
					new Item[]
					{
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(351, 18),
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(172, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(159, 11),
					},
					new Item[]
					{
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(351, 4),
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(172, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(333, 0),
					},
					new Item[]
					{
						new Item(5, 0),
						new Item(5, 0),
						new Item(5, 0),
						new Item(269, 0),
						new Item(5, 0),
						new Item(5, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(216, 0),
					},
					new Item[]
					{
						new Item(351, 15),
						new Item(351, 15),
						new Item(351, 15),
						new Item(351, 15),
						new Item(351, 15),
						new Item(351, 15),
						new Item(351, 15),
						new Item(351, 15),
						new Item(351, 15),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 15, 9),
					},
					new List<Item>
					{
						new Item(216, 32767, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 15, 3),
					},
					new List<Item>
					{
						new Item(352, 32767, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(340, 0, 1),
					},
					new List<Item>
					{
						new Item(339, 0, 1),
						new Item(339, 0, 1),
						new Item(339, 0, 1),
						new Item(334, 0, 1),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(261, 0),
					},
					new Item[]
					{
						new Item(0, 0),
						new Item(280, 32767),
						new Item(0, 0),
						new Item(280, 32767),
						new Item(0, 0),
						new Item(280, 32767),
						new Item(287, 32767),
						new Item(287, 32767),
						new Item(287, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(297, 0),
					},
					new Item[]
					{
						new Item(296, 32767),
						new Item(296, 32767),
						new Item(296, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(379, 0),
					},
					new Item[]
					{
						new Item(0, 0),
						new Item(0, 0),
						new Item(4, 32767),
						new Item(369, 32767),
						new Item(4, 32767),
						new Item(4, 32767),
					}, "crafting_table"),
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(45, 0),
					},
					new Item[]
					{
						new Item(336, 32767),
						new Item(336, 32767),
						new Item(336, 32767),
						new Item(336, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(108, 0),
					},
					new Item[]
					{
						new Item(45, 32767),
						new Item(45, 32767),
						new Item(45, 32767),
						new Item(0, 0),
						new Item(45, 32767),
						new Item(45, 32767),
						new Item(0, 0),
						new Item(0, 0),
						new Item(45, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(139, 6),
					},
					new Item[]
					{
						new Item(45, 32767),
						new Item(45, 32767),
						new Item(45, 32767),
						new Item(45, 32767),
						new Item(45, 32767),
						new Item(45, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(446, 3),
					},
					new Item[]
					{
						new Item(35, 12),
						new Item(35, 12),
						new Item(0, 0),
						new Item(35, 12),
						new Item(35, 12),
						new Item(280, 32767),
						new Item(35, 12),
						new Item(35, 12),
						new Item(0, 0),
					}, "crafting_table"),
				new ShapedRecipe(2, 1,
					new List<Item>
					{
						new Item(171, 12),
					},
					new Item[]
					{
						new Item(35, 12),
						new Item(35, 12),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(171, 12),
					},
					new Item[]
					{
						new Item(171, 0),
						new Item(171, 0),
						new Item(171, 0),
						new Item(171, 0),
						new Item(351, 17),
						new Item(171, 0),
						new Item(171, 0),
						new Item(171, 0),
						new Item(171, 0),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(237, 12, 8),
					},
					new List<Item>
					{
						new Item(351, 17, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(13, 32767, 1),
						new Item(13, 32767, 1),
						new Item(13, 32767, 1),
						new Item(13, 32767, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(237, 12, 8),
					},
					new List<Item>
					{
						new Item(351, 3, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(13, 32767, 1),
						new Item(13, 32767, 1),
						new Item(13, 32767, 1),
						new Item(13, 32767, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 17, 1),
					},
					new List<Item>
					{
						new Item(351, 3, 1),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(241, 12),
					},
					new Item[]
					{
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(351, 17),
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(241, 12),
					},
					new Item[]
					{
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(351, 3),
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(160, 12),
					},
					new Item[]
					{
						new Item(241, 12),
						new Item(241, 12),
						new Item(241, 12),
						new Item(241, 12),
						new Item(241, 12),
						new Item(241, 12),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(160, 12),
					},
					new Item[]
					{
						new Item(102, 32767),
						new Item(102, 32767),
						new Item(102, 32767),
						new Item(102, 32767),
						new Item(351, 17),
						new Item(102, 32767),
						new Item(102, 32767),
						new Item(102, 32767),
						new Item(102, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(159, 12),
					},
					new Item[]
					{
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(351, 17),
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(172, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(159, 12),
					},
					new Item[]
					{
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(351, 3),
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(172, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(325, 0),
					},
					new Item[]
					{
						new Item(265, 32767),
						new Item(265, 32767),
						new Item(265, 32767),
						new Item(0, 0),
						new Item(0, 0),
						new Item(0, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(354, 0),
						new Item(325, 0),
					},
					new Item[]
					{
						new Item(325, 1),
						new Item(353, 32767),
						new Item(296, 32767),
						new Item(325, 1),
						new Item(344, 32767),
						new Item(296, 32767),
						new Item(325, 1),
						new Item(353, 32767),
						new Item(296, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(720, 0),
					},
					new Item[]
					{
						new Item(0, 0),
						new Item(280, 32767),
						new Item(17, 32767),
						new Item(280, 32767),
						new Item(263, 32767),
						new Item(17, 32767),
						new Item(0, 0),
						new Item(280, 32767),
						new Item(17, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(720, 0),
					},
					new Item[]
					{
						new Item(0, 0),
						new Item(280, 32767),
						new Item(162, 32767),
						new Item(280, 32767),
						new Item(263, 32767),
						new Item(162, 32767),
						new Item(0, 0),
						new Item(280, 32767),
						new Item(162, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(720, 0),
					},
					new Item[]
					{
						new Item(0, 0),
						new Item(280, 32767),
						new Item(-8, 32767),
						new Item(280, 32767),
						new Item(263, 32767),
						new Item(-8, 32767),
						new Item(0, 0),
						new Item(280, 32767),
						new Item(-8, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(720, 0),
					},
					new Item[]
					{
						new Item(0, 0),
						new Item(280, 32767),
						new Item(-6, 32767),
						new Item(280, 32767),
						new Item(263, 32767),
						new Item(-6, 32767),
						new Item(0, 0),
						new Item(280, 32767),
						new Item(-6, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(720, 0),
					},
					new Item[]
					{
						new Item(0, 0),
						new Item(280, 32767),
						new Item(-9, 32767),
						new Item(280, 32767),
						new Item(263, 32767),
						new Item(-9, 32767),
						new Item(0, 0),
						new Item(280, 32767),
						new Item(-9, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(720, 0),
					},
					new Item[]
					{
						new Item(0, 0),
						new Item(280, 32767),
						new Item(-7, 32767),
						new Item(280, 32767),
						new Item(263, 32767),
						new Item(-7, 32767),
						new Item(0, 0),
						new Item(280, 32767),
						new Item(-7, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(720, 0),
					},
					new Item[]
					{
						new Item(0, 0),
						new Item(280, 32767),
						new Item(-10, 32767),
						new Item(280, 32767),
						new Item(263, 32767),
						new Item(-10, 32767),
						new Item(0, 0),
						new Item(280, 32767),
						new Item(-10, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(720, 0),
					},
					new Item[]
					{
						new Item(0, 0),
						new Item(280, 32767),
						new Item(-5, 32767),
						new Item(280, 32767),
						new Item(263, 32767),
						new Item(-5, 32767),
						new Item(0, 0),
						new Item(280, 32767),
						new Item(-5, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(720, 0),
					},
					new Item[]
					{
						new Item(0, 0),
						new Item(280, 32767),
						new Item(-212, 32767),
						new Item(280, 32767),
						new Item(263, 32767),
						new Item(-212, 32767),
						new Item(0, 0),
						new Item(280, 32767),
						new Item(-212, 32767),
					}, "crafting_table"),
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(398, 0),
					},
					new Item[]
					{
						new Item(346, 0),
						new Item(0, 0),
						new Item(0, 0),
						new Item(391, 32767),
					}, "crafting_table"),
				new ShapedRecipe(2, 3,
					new List<Item>
					{
						new Item(-200, 0),
					},
					new Item[]
					{
						new Item(339, 32767),
						new Item(5, 32767),
						new Item(339, 32767),
						new Item(5, 32767),
						new Item(5, 32767),
						new Item(5, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(380, 0),
					},
					new Item[]
					{
						new Item(265, 32767),
						new Item(265, 32767),
						new Item(265, 32767),
						new Item(0, 0),
						new Item(0, 0),
						new Item(265, 32767),
						new Item(265, 32767),
						new Item(265, 32767),
						new Item(265, 32767),
					}, "crafting_table"),
				new ShapedRecipe(1, 2,
					new List<Item>
					{
						new Item(342, 0),
					},
					new Item[]
					{
						new Item(54, 32767),
						new Item(328, 32767),
					}, "crafting_table"),
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(82, 0),
					},
					new Item[]
					{
						new Item(337, 32767),
						new Item(337, 32767),
						new Item(337, 32767),
						new Item(337, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(347, 0),
					},
					new Item[]
					{
						new Item(0, 0),
						new Item(266, 32767),
						new Item(0, 0),
						new Item(266, 32767),
						new Item(331, 32767),
						new Item(266, 32767),
						new Item(0, 0),
						new Item(266, 32767),
						new Item(0, 0),
					}, "crafting_table"),
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(263, 0),
					},
					new Item[]
					{
						new Item(173, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(173, 0),
					},
					new Item[]
					{
						new Item(263, 0),
						new Item(263, 0),
						new Item(263, 0),
						new Item(263, 0),
						new Item(263, 0),
						new Item(263, 0),
						new Item(263, 0),
						new Item(263, 0),
						new Item(263, 0),
					}, "crafting_table"),
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(3, 1),
					},
					new Item[]
					{
						new Item(3, 0),
						new Item(13, 32767),
						new Item(13, 32767),
						new Item(3, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(67, 0),
					},
					new Item[]
					{
						new Item(4, 32767),
						new Item(4, 32767),
						new Item(4, 32767),
						new Item(0, 0),
						new Item(4, 32767),
						new Item(4, 32767),
						new Item(0, 0),
						new Item(0, 0),
						new Item(4, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(139, 0),
					},
					new Item[]
					{
						new Item(4, 32767),
						new Item(4, 32767),
						new Item(4, 32767),
						new Item(4, 32767),
						new Item(4, 32767),
						new Item(4, 32767),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(287, 0, 9),
					},
					new List<Item>
					{
						new Item(30, 32767, 1),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(404, 0),
					},
					new Item[]
					{
						new Item(0, 0),
						new Item(76, 32767),
						new Item(1, 0),
						new Item(76, 32767),
						new Item(406, 32767),
						new Item(1, 0),
						new Item(0, 0),
						new Item(76, 32767),
						new Item(1, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(345, 0),
					},
					new Item[]
					{
						new Item(0, 0),
						new Item(265, 32767),
						new Item(0, 0),
						new Item(265, 32767),
						new Item(331, 32767),
						new Item(265, 32767),
						new Item(0, 0),
						new Item(265, 32767),
						new Item(0, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-213, 0),
					},
					new Item[]
					{
						new Item(158, 32767),
						new Item(158, 32767),
						new Item(158, 32767),
						new Item(0, 0),
						new Item(0, 0),
						new Item(158, 32767),
						new Item(158, 32767),
						new Item(158, 32767),
						new Item(158, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-157, 0),
					},
					new Item[]
					{
						new Item(465, 32767),
						new Item(465, 32767),
						new Item(465, 32767),
						new Item(465, 32767),
						new Item(467, 32767),
						new Item(465, 32767),
						new Item(465, 32767),
						new Item(465, 32767),
						new Item(465, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(357, 0),
					},
					new Item[]
					{
						new Item(296, 32767),
						new Item(351, 3),
						new Item(296, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(471, 0),
					},
					new Item[]
					{
						new Item(280, 32767),
						new Item(287, 32767),
						new Item(0, 0),
						new Item(265, 32767),
						new Item(131, 32767),
						new Item(280, 32767),
						new Item(280, 32767),
						new Item(287, 32767),
						new Item(0, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(446, 6),
					},
					new Item[]
					{
						new Item(35, 9),
						new Item(35, 9),
						new Item(0, 0),
						new Item(35, 9),
						new Item(35, 9),
						new Item(280, 32767),
						new Item(35, 9),
						new Item(35, 9),
						new Item(0, 0),
					}, "crafting_table"),
				new ShapedRecipe(2, 1,
					new List<Item>
					{
						new Item(171, 9),
					},
					new Item[]
					{
						new Item(35, 9),
						new Item(35, 9),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(171, 9),
					},
					new Item[]
					{
						new Item(171, 0),
						new Item(171, 0),
						new Item(171, 0),
						new Item(171, 0),
						new Item(351, 6),
						new Item(171, 0),
						new Item(171, 0),
						new Item(171, 0),
						new Item(171, 0),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(237, 9, 8),
					},
					new List<Item>
					{
						new Item(351, 6, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(13, 32767, 1),
						new Item(13, 32767, 1),
						new Item(13, 32767, 1),
						new Item(13, 32767, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 6, 2),
					},
					new List<Item>
					{
						new Item(351, 18, 1),
						new Item(351, 2, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 6, 2),
					},
					new List<Item>
					{
						new Item(351, 4, 1),
						new Item(351, 2, 1),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(241, 9),
					},
					new Item[]
					{
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(351, 6),
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(160, 9),
					},
					new Item[]
					{
						new Item(241, 9),
						new Item(241, 9),
						new Item(241, 9),
						new Item(241, 9),
						new Item(241, 9),
						new Item(241, 9),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(160, 9),
					},
					new Item[]
					{
						new Item(102, 32767),
						new Item(102, 32767),
						new Item(102, 32767),
						new Item(102, 32767),
						new Item(351, 6),
						new Item(102, 32767),
						new Item(102, 32767),
						new Item(102, 32767),
						new Item(102, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(159, 9),
					},
					new Item[]
					{
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(351, 6),
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(172, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(333, 5),
					},
					new Item[]
					{
						new Item(5, 5),
						new Item(5, 5),
						new Item(5, 5),
						new Item(269, 0),
						new Item(5, 5),
						new Item(5, 5),
					}, "crafting_table"),
				new ShapedRecipe(2, 3,
					new List<Item>
					{
						new Item(431, 0),
					},
					new Item[]
					{
						new Item(5, 5),
						new Item(5, 5),
						new Item(5, 5),
						new Item(5, 5),
						new Item(5, 5),
						new Item(5, 5),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(85, 5),
					},
					new Item[]
					{
						new Item(5, 5),
						new Item(5, 5),
						new Item(280, 32767),
						new Item(280, 32767),
						new Item(5, 5),
						new Item(5, 5),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(186, 0),
					},
					new Item[]
					{
						new Item(280, 32767),
						new Item(280, 32767),
						new Item(5, 5),
						new Item(5, 5),
						new Item(280, 32767),
						new Item(280, 32767),
					}, "crafting_table"),
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(5, 5),
					},
					new Item[]
					{
						new Item(162, 1),
					}, "crafting_table"),
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(5, 5),
					},
					new Item[]
					{
						new Item(-9, 32767),
					}, "crafting_table"),
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(5, 5),
					},
					new Item[]
					{
						new Item(-212, 13),
					}, "crafting_table"),
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(5, 5),
					},
					new Item[]
					{
						new Item(-212, 5),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(164, 0),
					},
					new Item[]
					{
						new Item(5, 5),
						new Item(5, 5),
						new Item(5, 5),
						new Item(0, 0),
						new Item(5, 5),
						new Item(5, 5),
						new Item(0, 0),
						new Item(0, 0),
						new Item(5, 5),
					}, "crafting_table"),
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(-212, 5),
					},
					new Item[]
					{
						new Item(162, 1),
						new Item(162, 1),
						new Item(162, 1),
						new Item(162, 1),
					}, "crafting_table"),
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(-212, 13),
					},
					new Item[]
					{
						new Item(-9, 32767),
						new Item(-9, 32767),
						new Item(-9, 32767),
						new Item(-9, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(158, 5),
					},
					new Item[]
					{
						new Item(5, 5),
						new Item(5, 5),
						new Item(5, 5),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(168, 1),
					},
					new Item[]
					{
						new Item(409, 32767),
						new Item(409, 32767),
						new Item(409, 32767),
						new Item(409, 32767),
						new Item(351, 16),
						new Item(409, 32767),
						new Item(409, 32767),
						new Item(409, 32767),
						new Item(409, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(168, 1),
					},
					new Item[]
					{
						new Item(409, 32767),
						new Item(409, 32767),
						new Item(409, 32767),
						new Item(409, 32767),
						new Item(351, 0),
						new Item(409, 32767),
						new Item(409, 32767),
						new Item(409, 32767),
						new Item(409, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(28, 0),
					},
					new Item[]
					{
						new Item(265, 32767),
						new Item(265, 32767),
						new Item(265, 32767),
						new Item(0, 0),
						new Item(70, 32767),
						new Item(331, 32767),
						new Item(265, 32767),
						new Item(265, 32767),
						new Item(265, 32767),
					}, "crafting_table"),
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(264, 0),
					},
					new Item[]
					{
						new Item(57, 32767),
					}, "crafting_table"),
				new ShapedRecipe(2, 3,
					new List<Item>
					{
						new Item(279, 0),
					},
					new Item[]
					{
						new Item(264, 32767),
						new Item(280, 32767),
						new Item(264, 32767),
						new Item(0, 0),
						new Item(264, 32767),
						new Item(280, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(57, 0),
					},
					new Item[]
					{
						new Item(264, 32767),
						new Item(264, 32767),
						new Item(264, 32767),
						new Item(264, 32767),
						new Item(264, 32767),
						new Item(264, 32767),
						new Item(264, 32767),
						new Item(264, 32767),
						new Item(264, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(313, 0),
					},
					new Item[]
					{
						new Item(264, 32767),
						new Item(264, 32767),
						new Item(0, 0),
						new Item(0, 0),
						new Item(264, 32767),
						new Item(264, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(311, 0),
					},
					new Item[]
					{
						new Item(264, 32767),
						new Item(264, 32767),
						new Item(264, 32767),
						new Item(0, 0),
						new Item(264, 32767),
						new Item(264, 32767),
						new Item(264, 32767),
						new Item(264, 32767),
						new Item(264, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(310, 0),
					},
					new Item[]
					{
						new Item(264, 32767),
						new Item(264, 32767),
						new Item(0, 0),
						new Item(264, 32767),
						new Item(264, 32767),
						new Item(264, 32767),
					}, "crafting_table"),
				new ShapedRecipe(2, 3,
					new List<Item>
					{
						new Item(293, 0),
					},
					new Item[]
					{
						new Item(264, 32767),
						new Item(280, 32767),
						new Item(264, 32767),
						new Item(0, 0),
						new Item(0, 0),
						new Item(280, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(312, 0),
					},
					new Item[]
					{
						new Item(264, 32767),
						new Item(264, 32767),
						new Item(264, 32767),
						new Item(264, 32767),
						new Item(0, 0),
						new Item(0, 0),
						new Item(264, 32767),
						new Item(264, 32767),
						new Item(264, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(278, 0),
					},
					new Item[]
					{
						new Item(264, 32767),
						new Item(0, 0),
						new Item(0, 0),
						new Item(264, 32767),
						new Item(280, 32767),
						new Item(280, 32767),
						new Item(264, 32767),
						new Item(0, 0),
						new Item(0, 0),
					}, "crafting_table"),
				new ShapedRecipe(1, 3,
					new List<Item>
					{
						new Item(277, 0),
					},
					new Item[]
					{
						new Item(264, 32767),
						new Item(280, 32767),
						new Item(280, 32767),
					}, "crafting_table"),
				new ShapedRecipe(1, 3,
					new List<Item>
					{
						new Item(276, 0),
					},
					new Item[]
					{
						new Item(264, 32767),
						new Item(264, 32767),
						new Item(280, 32767),
					}, "crafting_table"),
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(1, 3),
					},
					new Item[]
					{
						new Item(4, 32767),
						new Item(406, 32767),
						new Item(406, 32767),
						new Item(4, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-170, 0),
					},
					new Item[]
					{
						new Item(1, 3),
						new Item(1, 3),
						new Item(1, 3),
						new Item(0, 0),
						new Item(1, 3),
						new Item(1, 3),
						new Item(0, 0),
						new Item(0, 0),
						new Item(1, 3),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(139, 3),
					},
					new Item[]
					{
						new Item(1, 3),
						new Item(1, 3),
						new Item(1, 3),
						new Item(1, 3),
						new Item(1, 3),
						new Item(1, 3),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(23, 3),
					},
					new Item[]
					{
						new Item(4, 32767),
						new Item(4, 32767),
						new Item(4, 32767),
						new Item(4, 32767),
						new Item(261, 0),
						new Item(331, 32767),
						new Item(4, 32767),
						new Item(4, 32767),
						new Item(4, 32767),
					}, "crafting_table"),
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(464, 0),
					},
					new Item[]
					{
						new Item(-139, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-139, 0),
					},
					new Item[]
					{
						new Item(464, 32767),
						new Item(464, 32767),
						new Item(464, 32767),
						new Item(464, 32767),
						new Item(464, 32767),
						new Item(464, 32767),
						new Item(464, 32767),
						new Item(464, 32767),
						new Item(464, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(125, 3),
					},
					new Item[]
					{
						new Item(4, 32767),
						new Item(4, 32767),
						new Item(4, 32767),
						new Item(4, 32767),
						new Item(0, 0),
						new Item(331, 32767),
						new Item(4, 32767),
						new Item(4, 32767),
						new Item(4, 32767),
					}, "crafting_table"),
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(388, 0),
					},
					new Item[]
					{
						new Item(133, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(133, 0),
					},
					new Item[]
					{
						new Item(388, 32767),
						new Item(388, 32767),
						new Item(388, 32767),
						new Item(388, 32767),
						new Item(388, 32767),
						new Item(388, 32767),
						new Item(388, 32767),
						new Item(388, 32767),
						new Item(388, 32767),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(395, 2, 1),
					},
					new List<Item>
					{
						new Item(395, 0, 1),
						new Item(345, 32767, 1),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(116, 0),
					},
					new Item[]
					{
						new Item(0, 0),
						new Item(264, 32767),
						new Item(49, 32767),
						new Item(340, 32767),
						new Item(49, 32767),
						new Item(49, 32767),
						new Item(0, 0),
						new Item(264, 32767),
						new Item(49, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-178, 0),
					},
					new Item[]
					{
						new Item(206, 32767),
						new Item(206, 32767),
						new Item(206, 32767),
						new Item(0, 0),
						new Item(206, 32767),
						new Item(206, 32767),
						new Item(0, 0),
						new Item(0, 0),
						new Item(206, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(139, 10),
					},
					new Item[]
					{
						new Item(206, 32767),
						new Item(206, 32767),
						new Item(206, 32767),
						new Item(206, 32767),
						new Item(206, 32767),
						new Item(206, 32767),
					}, "crafting_table"),
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(206, 0),
					},
					new Item[]
					{
						new Item(121, 32767),
						new Item(121, 32767),
						new Item(121, 32767),
						new Item(121, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(426, 0),
					},
					new Item[]
					{
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(381, 32767),
						new Item(370, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
					}, "crafting_table"),
				new ShapedRecipe(1, 2,
					new List<Item>
					{
						new Item(208, 0),
					},
					new Item[]
					{
						new Item(369, 32767),
						new Item(433, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(130, 0),
					},
					new Item[]
					{
						new Item(49, 32767),
						new Item(49, 32767),
						new Item(49, 32767),
						new Item(49, 32767),
						new Item(381, 32767),
						new Item(49, 32767),
						new Item(49, 32767),
						new Item(49, 32767),
						new Item(49, 32767),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(381, 0, 1),
					},
					new List<Item>
					{
						new Item(368, 32767, 1),
						new Item(377, 32767, 1),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(85, 0),
					},
					new Item[]
					{
						new Item(5, 0),
						new Item(5, 0),
						new Item(280, 32767),
						new Item(280, 32767),
						new Item(5, 0),
						new Item(5, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(107, 0),
					},
					new Item[]
					{
						new Item(280, 32767),
						new Item(280, 32767),
						new Item(5, 0),
						new Item(5, 0),
						new Item(280, 32767),
						new Item(280, 32767),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(376, 0, 1),
					},
					new List<Item>
					{
						new Item(375, 32767, 1),
						new Item(39, 32767, 1),
						new Item(353, 32767, 1),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(346, 0),
					},
					new Item[]
					{
						new Item(0, 0),
						new Item(0, 0),
						new Item(280, 32767),
						new Item(0, 0),
						new Item(280, 32767),
						new Item(0, 0),
						new Item(280, 32767),
						new Item(287, 32767),
						new Item(287, 32767),
					}, "crafting_table"),
				new ShapedRecipe(2, 3,
					new List<Item>
					{
						new Item(-201, 0),
					},
					new Item[]
					{
						new Item(318, 32767),
						new Item(5, 32767),
						new Item(318, 32767),
						new Item(5, 32767),
						new Item(5, 32767),
						new Item(5, 32767),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(259, 0, 1),
					},
					new List<Item>
					{
						new Item(265, 32767, 1),
						new Item(318, 32767, 1),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(390, 0),
					},
					new Item[]
					{
						new Item(336, 32767),
						new Item(336, 32767),
						new Item(336, 32767),
						new Item(0, 0),
						new Item(0, 0),
						new Item(0, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(61, 0),
					},
					new Item[]
					{
						new Item(4, 32767),
						new Item(4, 32767),
						new Item(4, 32767),
						new Item(4, 32767),
						new Item(0, 0),
						new Item(4, 32767),
						new Item(4, 32767),
						new Item(4, 32767),
						new Item(4, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(374, 0),
					},
					new Item[]
					{
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(0, 0),
						new Item(0, 0),
						new Item(0, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(102, 0),
					},
					new Item[]
					{
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
					}, "crafting_table"),
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(89, 0),
					},
					new Item[]
					{
						new Item(348, 32767),
						new Item(348, 32767),
						new Item(348, 32767),
						new Item(348, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(41, 0),
					},
					new Item[]
					{
						new Item(266, 32767),
						new Item(266, 32767),
						new Item(266, 32767),
						new Item(266, 32767),
						new Item(266, 32767),
						new Item(266, 32767),
						new Item(266, 32767),
						new Item(266, 32767),
						new Item(266, 32767),
					}, "crafting_table"),
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(266, 0),
					},
					new Item[]
					{
						new Item(41, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(266, 0),
					},
					new Item[]
					{
						new Item(371, 32767),
						new Item(371, 32767),
						new Item(371, 32767),
						new Item(371, 32767),
						new Item(371, 32767),
						new Item(371, 32767),
						new Item(371, 32767),
						new Item(371, 32767),
						new Item(371, 32767),
					}, "crafting_table"),
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(371, 0),
					},
					new Item[]
					{
						new Item(266, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(322, 0),
					},
					new Item[]
					{
						new Item(266, 32767),
						new Item(266, 32767),
						new Item(266, 32767),
						new Item(266, 32767),
						new Item(260, 32767),
						new Item(266, 32767),
						new Item(266, 32767),
						new Item(266, 32767),
						new Item(266, 32767),
					}, "crafting_table"),
				new ShapedRecipe(2, 3,
					new List<Item>
					{
						new Item(286, 0),
					},
					new Item[]
					{
						new Item(266, 32767),
						new Item(280, 32767),
						new Item(266, 32767),
						new Item(0, 0),
						new Item(266, 32767),
						new Item(280, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(317, 0),
					},
					new Item[]
					{
						new Item(266, 32767),
						new Item(266, 32767),
						new Item(0, 0),
						new Item(0, 0),
						new Item(266, 32767),
						new Item(266, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(396, 0),
					},
					new Item[]
					{
						new Item(371, 32767),
						new Item(371, 32767),
						new Item(371, 32767),
						new Item(371, 32767),
						new Item(391, 32767),
						new Item(371, 32767),
						new Item(371, 32767),
						new Item(371, 32767),
						new Item(371, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(315, 0),
					},
					new Item[]
					{
						new Item(266, 32767),
						new Item(266, 32767),
						new Item(266, 32767),
						new Item(0, 0),
						new Item(266, 32767),
						new Item(266, 32767),
						new Item(266, 32767),
						new Item(266, 32767),
						new Item(266, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(314, 0),
					},
					new Item[]
					{
						new Item(266, 32767),
						new Item(266, 32767),
						new Item(0, 0),
						new Item(266, 32767),
						new Item(266, 32767),
						new Item(266, 32767),
					}, "crafting_table"),
				new ShapedRecipe(2, 3,
					new List<Item>
					{
						new Item(294, 0),
					},
					new Item[]
					{
						new Item(266, 32767),
						new Item(280, 32767),
						new Item(266, 32767),
						new Item(0, 0),
						new Item(0, 0),
						new Item(280, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(316, 0),
					},
					new Item[]
					{
						new Item(266, 32767),
						new Item(266, 32767),
						new Item(266, 32767),
						new Item(266, 32767),
						new Item(0, 0),
						new Item(0, 0),
						new Item(266, 32767),
						new Item(266, 32767),
						new Item(266, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(285, 0),
					},
					new Item[]
					{
						new Item(266, 32767),
						new Item(0, 0),
						new Item(0, 0),
						new Item(266, 32767),
						new Item(280, 32767),
						new Item(280, 32767),
						new Item(266, 32767),
						new Item(0, 0),
						new Item(0, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(27, 0),
					},
					new Item[]
					{
						new Item(266, 32767),
						new Item(266, 32767),
						new Item(266, 32767),
						new Item(0, 0),
						new Item(280, 32767),
						new Item(331, 32767),
						new Item(266, 32767),
						new Item(266, 32767),
						new Item(266, 32767),
					}, "crafting_table"),
				new ShapedRecipe(1, 3,
					new List<Item>
					{
						new Item(284, 0),
					},
					new Item[]
					{
						new Item(266, 32767),
						new Item(280, 32767),
						new Item(280, 32767),
					}, "crafting_table"),
				new ShapedRecipe(1, 3,
					new List<Item>
					{
						new Item(283, 0),
					},
					new Item[]
					{
						new Item(266, 32767),
						new Item(266, 32767),
						new Item(280, 32767),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(1, 1, 1),
					},
					new List<Item>
					{
						new Item(1, 3, 1),
						new Item(406, 32767, 1),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-169, 0),
					},
					new Item[]
					{
						new Item(1, 1),
						new Item(1, 1),
						new Item(1, 1),
						new Item(0, 0),
						new Item(1, 1),
						new Item(1, 1),
						new Item(0, 0),
						new Item(0, 0),
						new Item(1, 1),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(139, 2),
					},
					new Item[]
					{
						new Item(1, 1),
						new Item(1, 1),
						new Item(1, 1),
						new Item(1, 1),
						new Item(1, 1),
						new Item(1, 1),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(446, 8),
					},
					new Item[]
					{
						new Item(35, 7),
						new Item(35, 7),
						new Item(0, 0),
						new Item(35, 7),
						new Item(35, 7),
						new Item(280, 32767),
						new Item(35, 7),
						new Item(35, 7),
						new Item(0, 0),
					}, "crafting_table"),
				new ShapedRecipe(2, 1,
					new List<Item>
					{
						new Item(171, 7),
					},
					new Item[]
					{
						new Item(35, 7),
						new Item(35, 7),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(171, 7),
					},
					new Item[]
					{
						new Item(171, 0),
						new Item(171, 0),
						new Item(171, 0),
						new Item(171, 0),
						new Item(351, 8),
						new Item(171, 0),
						new Item(171, 0),
						new Item(171, 0),
						new Item(171, 0),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(237, 7, 8),
					},
					new List<Item>
					{
						new Item(351, 8, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(13, 32767, 1),
						new Item(13, 32767, 1),
						new Item(13, 32767, 1),
						new Item(13, 32767, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 8, 2),
					},
					new List<Item>
					{
						new Item(351, 16, 1),
						new Item(351, 19, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 8, 2),
					},
					new List<Item>
					{
						new Item(351, 16, 1),
						new Item(351, 15, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 8, 2),
					},
					new List<Item>
					{
						new Item(351, 0, 1),
						new Item(351, 15, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 8, 2),
					},
					new List<Item>
					{
						new Item(351, 0, 1),
						new Item(351, 19, 1),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(241, 7),
					},
					new Item[]
					{
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(351, 8),
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(160, 7),
					},
					new Item[]
					{
						new Item(241, 7),
						new Item(241, 7),
						new Item(241, 7),
						new Item(241, 7),
						new Item(241, 7),
						new Item(241, 7),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(160, 7),
					},
					new Item[]
					{
						new Item(102, 32767),
						new Item(102, 32767),
						new Item(102, 32767),
						new Item(102, 32767),
						new Item(351, 8),
						new Item(102, 32767),
						new Item(102, 32767),
						new Item(102, 32767),
						new Item(102, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(159, 7),
					},
					new Item[]
					{
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(351, 8),
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(172, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(446, 2),
					},
					new Item[]
					{
						new Item(35, 13),
						new Item(35, 13),
						new Item(0, 0),
						new Item(35, 13),
						new Item(35, 13),
						new Item(280, 32767),
						new Item(35, 13),
						new Item(35, 13),
						new Item(0, 0),
					}, "crafting_table"),
				new ShapedRecipe(2, 1,
					new List<Item>
					{
						new Item(171, 13),
					},
					new Item[]
					{
						new Item(35, 13),
						new Item(35, 13),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(171, 13),
					},
					new Item[]
					{
						new Item(171, 0),
						new Item(171, 0),
						new Item(171, 0),
						new Item(171, 0),
						new Item(351, 2),
						new Item(171, 0),
						new Item(171, 0),
						new Item(171, 0),
						new Item(171, 0),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(237, 13, 8),
					},
					new List<Item>
					{
						new Item(351, 2, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(13, 32767, 1),
						new Item(13, 32767, 1),
						new Item(13, 32767, 1),
						new Item(13, 32767, 1),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(241, 13),
					},
					new Item[]
					{
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(351, 2),
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(160, 13),
					},
					new Item[]
					{
						new Item(241, 13),
						new Item(241, 13),
						new Item(241, 13),
						new Item(241, 13),
						new Item(241, 13),
						new Item(241, 13),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(160, 13),
					},
					new Item[]
					{
						new Item(102, 32767),
						new Item(102, 32767),
						new Item(102, 32767),
						new Item(102, 32767),
						new Item(351, 2),
						new Item(102, 32767),
						new Item(102, 32767),
						new Item(102, 32767),
						new Item(102, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(159, 13),
					},
					new Item[]
					{
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(351, 2),
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(172, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(-195, 0),
					},
					new Item[]
					{
						new Item(280, 32767),
						new Item(280, 32767),
						new Item(0, 0),
						new Item(-166, 2),
						new Item(5, 32767),
						new Item(5, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(170, 0),
					},
					new Item[]
					{
						new Item(296, 32767),
						new Item(296, 32767),
						new Item(296, 32767),
						new Item(296, 32767),
						new Item(296, 32767),
						new Item(296, 32767),
						new Item(296, 32767),
						new Item(296, 32767),
						new Item(296, 32767),
					}, "crafting_table"),
				new ShapedRecipe(2, 1,
					new List<Item>
					{
						new Item(148, 0),
					},
					new Item[]
					{
						new Item(265, 32767),
						new Item(265, 32767),
					}, "crafting_table"),
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(-220, 0),
						new Item(374, 0),
					},
					new Item[]
					{
						new Item(737, 32767),
						new Item(737, 32767),
						new Item(737, 32767),
						new Item(737, 32767),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(737, 0, 4),
					},
					new List<Item>
					{
						new Item(-220, 32767, 1),
						new Item(374, 32767, 1),
						new Item(374, 32767, 1),
						new Item(374, 32767, 1),
						new Item(374, 32767, 1),
					}, "crafting_table"),
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(353, 0),
						new Item(374, 0),
					},
					new Item[]
					{
						new Item(737, 32767),
					}, "crafting_table"),
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(-221, 0),
					},
					new Item[]
					{
						new Item(736, 32767),
						new Item(736, 32767),
						new Item(736, 32767),
						new Item(736, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(410, 0),
					},
					new Item[]
					{
						new Item(265, 32767),
						new Item(265, 32767),
						new Item(0, 0),
						new Item(0, 0),
						new Item(54, 32767),
						new Item(265, 32767),
						new Item(265, 32767),
						new Item(265, 32767),
						new Item(0, 0),
					}, "crafting_table"),
				new ShapedRecipe(1, 2,
					new List<Item>
					{
						new Item(408, 0),
					},
					new Item[]
					{
						new Item(410, 32767),
						new Item(328, 32767),
					}, "crafting_table"),
				new ShapedRecipe(2, 3,
					new List<Item>
					{
						new Item(258, 0),
					},
					new Item[]
					{
						new Item(265, 32767),
						new Item(280, 32767),
						new Item(265, 32767),
						new Item(0, 0),
						new Item(265, 32767),
						new Item(280, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(101, 0),
					},
					new Item[]
					{
						new Item(265, 32767),
						new Item(265, 32767),
						new Item(265, 32767),
						new Item(265, 32767),
						new Item(265, 32767),
						new Item(265, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(42, 0),
					},
					new Item[]
					{
						new Item(265, 32767),
						new Item(265, 32767),
						new Item(265, 32767),
						new Item(265, 32767),
						new Item(265, 32767),
						new Item(265, 32767),
						new Item(265, 32767),
						new Item(265, 32767),
						new Item(265, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(309, 0),
					},
					new Item[]
					{
						new Item(265, 32767),
						new Item(265, 32767),
						new Item(0, 0),
						new Item(0, 0),
						new Item(265, 32767),
						new Item(265, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(307, 0),
					},
					new Item[]
					{
						new Item(265, 32767),
						new Item(265, 32767),
						new Item(265, 32767),
						new Item(0, 0),
						new Item(265, 32767),
						new Item(265, 32767),
						new Item(265, 32767),
						new Item(265, 32767),
						new Item(265, 32767),
					}, "crafting_table"),
				new ShapedRecipe(2, 3,
					new List<Item>
					{
						new Item(330, 0),
					},
					new Item[]
					{
						new Item(265, 32767),
						new Item(265, 32767),
						new Item(265, 32767),
						new Item(265, 32767),
						new Item(265, 32767),
						new Item(265, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(306, 0),
					},
					new Item[]
					{
						new Item(265, 32767),
						new Item(265, 32767),
						new Item(0, 0),
						new Item(265, 32767),
						new Item(265, 32767),
						new Item(265, 32767),
					}, "crafting_table"),
				new ShapedRecipe(2, 3,
					new List<Item>
					{
						new Item(292, 0),
					},
					new Item[]
					{
						new Item(265, 32767),
						new Item(280, 32767),
						new Item(265, 32767),
						new Item(0, 0),
						new Item(0, 0),
						new Item(280, 32767),
					}, "crafting_table"),
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(265, 0),
					},
					new Item[]
					{
						new Item(42, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(265, 0),
					},
					new Item[]
					{
						new Item(452, 32767),
						new Item(452, 32767),
						new Item(452, 32767),
						new Item(452, 32767),
						new Item(452, 32767),
						new Item(452, 32767),
						new Item(452, 32767),
						new Item(452, 32767),
						new Item(452, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(308, 0),
					},
					new Item[]
					{
						new Item(265, 32767),
						new Item(265, 32767),
						new Item(265, 32767),
						new Item(265, 32767),
						new Item(0, 0),
						new Item(0, 0),
						new Item(265, 32767),
						new Item(265, 32767),
						new Item(265, 32767),
					}, "crafting_table"),
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(452, 0),
					},
					new Item[]
					{
						new Item(265, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(257, 0),
					},
					new Item[]
					{
						new Item(265, 32767),
						new Item(0, 0),
						new Item(0, 0),
						new Item(265, 32767),
						new Item(280, 32767),
						new Item(280, 32767),
						new Item(265, 32767),
						new Item(0, 0),
						new Item(0, 0),
					}, "crafting_table"),
				new ShapedRecipe(1, 3,
					new List<Item>
					{
						new Item(256, 0),
					},
					new Item[]
					{
						new Item(265, 32767),
						new Item(280, 32767),
						new Item(280, 32767),
					}, "crafting_table"),
				new ShapedRecipe(1, 3,
					new List<Item>
					{
						new Item(267, 0),
					},
					new Item[]
					{
						new Item(265, 32767),
						new Item(265, 32767),
						new Item(280, 32767),
					}, "crafting_table"),
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(167, 0),
					},
					new Item[]
					{
						new Item(265, 32767),
						new Item(265, 32767),
						new Item(265, 32767),
						new Item(265, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(389, 0),
					},
					new Item[]
					{
						new Item(280, 32767),
						new Item(280, 32767),
						new Item(280, 32767),
						new Item(280, 32767),
						new Item(334, 32767),
						new Item(280, 32767),
						new Item(280, 32767),
						new Item(280, 32767),
						new Item(280, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(333, 3),
					},
					new Item[]
					{
						new Item(5, 3),
						new Item(5, 3),
						new Item(5, 3),
						new Item(269, 0),
						new Item(5, 3),
						new Item(5, 3),
					}, "crafting_table"),
				new ShapedRecipe(2, 3,
					new List<Item>
					{
						new Item(429, 0),
					},
					new Item[]
					{
						new Item(5, 3),
						new Item(5, 3),
						new Item(5, 3),
						new Item(5, 3),
						new Item(5, 3),
						new Item(5, 3),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(85, 3),
					},
					new Item[]
					{
						new Item(5, 3),
						new Item(5, 3),
						new Item(280, 32767),
						new Item(280, 32767),
						new Item(5, 3),
						new Item(5, 3),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(185, 0),
					},
					new Item[]
					{
						new Item(280, 32767),
						new Item(280, 32767),
						new Item(5, 3),
						new Item(5, 3),
						new Item(280, 32767),
						new Item(280, 32767),
					}, "crafting_table"),
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(5, 3),
					},
					new Item[]
					{
						new Item(17, 3),
					}, "crafting_table"),
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(5, 3),
					},
					new Item[]
					{
						new Item(-7, 32767),
					}, "crafting_table"),
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(5, 3),
					},
					new Item[]
					{
						new Item(-212, 11),
					}, "crafting_table"),
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(5, 3),
					},
					new Item[]
					{
						new Item(-212, 3),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(136, 0),
					},
					new Item[]
					{
						new Item(5, 3),
						new Item(5, 3),
						new Item(5, 3),
						new Item(0, 0),
						new Item(5, 3),
						new Item(5, 3),
						new Item(0, 0),
						new Item(0, 0),
						new Item(5, 3),
					}, "crafting_table"),
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(-212, 3),
					},
					new Item[]
					{
						new Item(17, 3),
						new Item(17, 3),
						new Item(17, 3),
						new Item(17, 3),
					}, "crafting_table"),
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(-212, 11),
					},
					new Item[]
					{
						new Item(-7, 32767),
						new Item(-7, 32767),
						new Item(-7, 32767),
						new Item(-7, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(158, 3),
					},
					new Item[]
					{
						new Item(5, 3),
						new Item(5, 3),
						new Item(5, 3),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(65, 0),
					},
					new Item[]
					{
						new Item(280, 32767),
						new Item(280, 32767),
						new Item(280, 32767),
						new Item(0, 0),
						new Item(280, 32767),
						new Item(0, 0),
						new Item(280, 32767),
						new Item(280, 32767),
						new Item(280, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-208, 0),
					},
					new Item[]
					{
						new Item(452, 32767),
						new Item(452, 32767),
						new Item(452, 32767),
						new Item(452, 32767),
						new Item(50, 32767),
						new Item(452, 32767),
						new Item(452, 32767),
						new Item(452, 32767),
						new Item(452, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(22, 0),
					},
					new Item[]
					{
						new Item(351, 4),
						new Item(351, 4),
						new Item(351, 4),
						new Item(351, 4),
						new Item(351, 4),
						new Item(351, 4),
						new Item(351, 4),
						new Item(351, 4),
						new Item(351, 4),
					}, "crafting_table"),
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(351, 4),
					},
					new Item[]
					{
						new Item(22, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(420, 0),
					},
					new Item[]
					{
						new Item(287, 32767),
						new Item(287, 32767),
						new Item(0, 0),
						new Item(287, 32767),
						new Item(341, 32767),
						new Item(0, 0),
						new Item(0, 0),
						new Item(0, 0),
						new Item(287, 32767),
					}, "crafting_table"),
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(334, 0),
					},
					new Item[]
					{
						new Item(415, 32767),
						new Item(415, 32767),
						new Item(415, 32767),
						new Item(415, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(301, 0),
					},
					new Item[]
					{
						new Item(334, 32767),
						new Item(334, 32767),
						new Item(0, 0),
						new Item(0, 0),
						new Item(334, 32767),
						new Item(334, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(299, 0),
					},
					new Item[]
					{
						new Item(334, 32767),
						new Item(334, 32767),
						new Item(334, 32767),
						new Item(0, 0),
						new Item(334, 32767),
						new Item(334, 32767),
						new Item(334, 32767),
						new Item(334, 32767),
						new Item(334, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(298, 0),
					},
					new Item[]
					{
						new Item(334, 32767),
						new Item(334, 32767),
						new Item(0, 0),
						new Item(334, 32767),
						new Item(334, 32767),
						new Item(334, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(416, 0),
					},
					new Item[]
					{
						new Item(334, 32767),
						new Item(334, 32767),
						new Item(334, 32767),
						new Item(0, 0),
						new Item(334, 32767),
						new Item(0, 0),
						new Item(334, 32767),
						new Item(334, 32767),
						new Item(334, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(300, 0),
					},
					new Item[]
					{
						new Item(334, 32767),
						new Item(334, 32767),
						new Item(334, 32767),
						new Item(334, 32767),
						new Item(0, 0),
						new Item(0, 0),
						new Item(334, 32767),
						new Item(334, 32767),
						new Item(334, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-194, 0),
					},
					new Item[]
					{
						new Item(158, 32767),
						new Item(0, 0),
						new Item(0, 0),
						new Item(158, 32767),
						new Item(47, 32767),
						new Item(158, 32767),
						new Item(158, 32767),
						new Item(0, 0),
						new Item(0, 0),
					}, "crafting_table"),
				new ShapedRecipe(1, 2,
					new List<Item>
					{
						new Item(69, 0),
					},
					new Item[]
					{
						new Item(280, 32767),
						new Item(4, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(446, 12),
					},
					new Item[]
					{
						new Item(35, 3),
						new Item(35, 3),
						new Item(0, 0),
						new Item(35, 3),
						new Item(35, 3),
						new Item(280, 32767),
						new Item(35, 3),
						new Item(35, 3),
						new Item(0, 0),
					}, "crafting_table"),
				new ShapedRecipe(2, 1,
					new List<Item>
					{
						new Item(171, 3),
					},
					new Item[]
					{
						new Item(35, 3),
						new Item(35, 3),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(171, 3),
					},
					new Item[]
					{
						new Item(171, 0),
						new Item(171, 0),
						new Item(171, 0),
						new Item(171, 0),
						new Item(351, 12),
						new Item(171, 0),
						new Item(171, 0),
						new Item(171, 0),
						new Item(171, 0),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(237, 3, 8),
					},
					new List<Item>
					{
						new Item(351, 12, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(13, 32767, 1),
						new Item(13, 32767, 1),
						new Item(13, 32767, 1),
						new Item(13, 32767, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 12, 2),
					},
					new List<Item>
					{
						new Item(351, 18, 1),
						new Item(351, 19, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 12, 2),
					},
					new List<Item>
					{
						new Item(351, 18, 1),
						new Item(351, 15, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 12, 1),
					},
					new List<Item>
					{
						new Item(38, 1, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 12, 2),
					},
					new List<Item>
					{
						new Item(351, 4, 1),
						new Item(351, 15, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 12, 2),
					},
					new List<Item>
					{
						new Item(351, 4, 1),
						new Item(351, 19, 1),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(241, 3),
					},
					new Item[]
					{
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(351, 12),
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(160, 3),
					},
					new Item[]
					{
						new Item(241, 3),
						new Item(241, 3),
						new Item(241, 3),
						new Item(241, 3),
						new Item(241, 3),
						new Item(241, 3),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(160, 3),
					},
					new Item[]
					{
						new Item(102, 32767),
						new Item(102, 32767),
						new Item(102, 32767),
						new Item(102, 32767),
						new Item(351, 12),
						new Item(102, 32767),
						new Item(102, 32767),
						new Item(102, 32767),
						new Item(102, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(159, 3),
					},
					new Item[]
					{
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(351, 12),
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(172, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(171, 8),
					},
					new Item[]
					{
						new Item(171, 0),
						new Item(171, 0),
						new Item(171, 0),
						new Item(171, 0),
						new Item(351, 7),
						new Item(171, 0),
						new Item(171, 0),
						new Item(171, 0),
						new Item(171, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(446, 7),
					},
					new Item[]
					{
						new Item(35, 8),
						new Item(35, 8),
						new Item(0, 0),
						new Item(35, 8),
						new Item(35, 8),
						new Item(280, 32767),
						new Item(35, 8),
						new Item(35, 8),
						new Item(0, 0),
					}, "crafting_table"),
				new ShapedRecipe(2, 1,
					new List<Item>
					{
						new Item(171, 8),
					},
					new Item[]
					{
						new Item(35, 8),
						new Item(35, 8),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(237, 8, 8),
					},
					new List<Item>
					{
						new Item(351, 7, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(13, 32767, 1),
						new Item(13, 32767, 1),
						new Item(13, 32767, 1),
						new Item(13, 32767, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 7, 3),
					},
					new List<Item>
					{
						new Item(351, 16, 1),
						new Item(351, 19, 1),
						new Item(351, 19, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 7, 1),
					},
					new List<Item>
					{
						new Item(38, 3, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 7, 3),
					},
					new List<Item>
					{
						new Item(351, 16, 1),
						new Item(351, 15, 1),
						new Item(351, 15, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 7, 2),
					},
					new List<Item>
					{
						new Item(351, 8, 1),
						new Item(351, 15, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 7, 2),
					},
					new List<Item>
					{
						new Item(351, 8, 1),
						new Item(351, 19, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 7, 3),
					},
					new List<Item>
					{
						new Item(351, 0, 1),
						new Item(351, 15, 1),
						new Item(351, 15, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 7, 3),
					},
					new List<Item>
					{
						new Item(351, 0, 1),
						new Item(351, 19, 1),
						new Item(351, 19, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 7, 1),
					},
					new List<Item>
					{
						new Item(38, 8, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 7, 1),
					},
					new List<Item>
					{
						new Item(38, 6, 1),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(241, 8),
					},
					new Item[]
					{
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(351, 7),
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(160, 8),
					},
					new Item[]
					{
						new Item(241, 8),
						new Item(241, 8),
						new Item(241, 8),
						new Item(241, 8),
						new Item(241, 8),
						new Item(241, 8),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(160, 8),
					},
					new Item[]
					{
						new Item(102, 32767),
						new Item(102, 32767),
						new Item(102, 32767),
						new Item(102, 32767),
						new Item(351, 7),
						new Item(102, 32767),
						new Item(102, 32767),
						new Item(102, 32767),
						new Item(102, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(159, 8),
					},
					new Item[]
					{
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(351, 7),
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(172, 32767),
					}, "crafting_table"),
				new ShapedRecipe(2, 1,
					new List<Item>
					{
						new Item(147, 0),
					},
					new Item[]
					{
						new Item(266, 32767),
						new Item(266, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(171, 5),
					},
					new Item[]
					{
						new Item(171, 0),
						new Item(171, 0),
						new Item(171, 0),
						new Item(171, 0),
						new Item(351, 10),
						new Item(171, 0),
						new Item(171, 0),
						new Item(171, 0),
						new Item(171, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(446, 10),
					},
					new Item[]
					{
						new Item(35, 5),
						new Item(35, 5),
						new Item(0, 0),
						new Item(35, 5),
						new Item(35, 5),
						new Item(280, 32767),
						new Item(35, 5),
						new Item(35, 5),
						new Item(0, 0),
					}, "crafting_table"),
				new ShapedRecipe(2, 1,
					new List<Item>
					{
						new Item(171, 5),
					},
					new Item[]
					{
						new Item(35, 5),
						new Item(35, 5),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(237, 5, 8),
					},
					new List<Item>
					{
						new Item(351, 10, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(13, 32767, 1),
						new Item(13, 32767, 1),
						new Item(13, 32767, 1),
						new Item(13, 32767, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 10, 2),
					},
					new List<Item>
					{
						new Item(351, 2, 1),
						new Item(351, 19, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 10, 2),
					},
					new List<Item>
					{
						new Item(351, 2, 1),
						new Item(351, 15, 1),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(241, 5),
					},
					new Item[]
					{
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(351, 10),
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(160, 5),
					},
					new Item[]
					{
						new Item(241, 5),
						new Item(241, 5),
						new Item(241, 5),
						new Item(241, 5),
						new Item(241, 5),
						new Item(241, 5),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(160, 5),
					},
					new Item[]
					{
						new Item(102, 32767),
						new Item(102, 32767),
						new Item(102, 32767),
						new Item(102, 32767),
						new Item(351, 10),
						new Item(102, 32767),
						new Item(102, 32767),
						new Item(102, 32767),
						new Item(102, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(159, 5),
					},
					new Item[]
					{
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(351, 10),
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(172, 32767),
					}, "crafting_table"),
				new ShapedRecipe(1, 2,
					new List<Item>
					{
						new Item(91, 0),
					},
					new Item[]
					{
						new Item(-155, 32767),
						new Item(50, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(395, 2),
					},
					new Item[]
					{
						new Item(339, 32767),
						new Item(339, 32767),
						new Item(339, 32767),
						new Item(339, 32767),
						new Item(345, 32767),
						new Item(339, 32767),
						new Item(339, 32767),
						new Item(339, 32767),
						new Item(339, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(446, 13),
					},
					new Item[]
					{
						new Item(35, 2),
						new Item(35, 2),
						new Item(0, 0),
						new Item(35, 2),
						new Item(35, 2),
						new Item(280, 32767),
						new Item(35, 2),
						new Item(35, 2),
						new Item(0, 0),
					}, "crafting_table"),
				new ShapedRecipe(2, 1,
					new List<Item>
					{
						new Item(171, 2),
					},
					new Item[]
					{
						new Item(35, 2),
						new Item(35, 2),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(171, 2),
					},
					new Item[]
					{
						new Item(171, 0),
						new Item(171, 0),
						new Item(171, 0),
						new Item(171, 0),
						new Item(351, 13),
						new Item(171, 0),
						new Item(171, 0),
						new Item(171, 0),
						new Item(171, 0),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(237, 2, 8),
					},
					new List<Item>
					{
						new Item(351, 13, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(13, 32767, 1),
						new Item(13, 32767, 1),
						new Item(13, 32767, 1),
						new Item(13, 32767, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 13, 3),
					},
					new List<Item>
					{
						new Item(351, 18, 1),
						new Item(351, 1, 1),
						new Item(351, 9, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 13, 1),
					},
					new List<Item>
					{
						new Item(38, 2, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 13, 4),
					},
					new List<Item>
					{
						new Item(351, 18, 1),
						new Item(351, 1, 1),
						new Item(351, 1, 1),
						new Item(351, 15, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 13, 4),
					},
					new List<Item>
					{
						new Item(351, 18, 1),
						new Item(351, 1, 1),
						new Item(351, 1, 1),
						new Item(351, 19, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 13, 4),
					},
					new List<Item>
					{
						new Item(351, 4, 1),
						new Item(351, 1, 1),
						new Item(351, 1, 1),
						new Item(351, 15, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 13, 4),
					},
					new List<Item>
					{
						new Item(351, 4, 1),
						new Item(351, 1, 1),
						new Item(351, 1, 1),
						new Item(351, 19, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 13, 3),
					},
					new List<Item>
					{
						new Item(351, 4, 1),
						new Item(351, 1, 1),
						new Item(351, 9, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 13, 2),
					},
					new List<Item>
					{
						new Item(175, 1, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 13, 2),
					},
					new List<Item>
					{
						new Item(351, 5, 1),
						new Item(351, 9, 1),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(241, 2),
					},
					new Item[]
					{
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(351, 13),
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(160, 2),
					},
					new Item[]
					{
						new Item(241, 2),
						new Item(241, 2),
						new Item(241, 2),
						new Item(241, 2),
						new Item(241, 2),
						new Item(241, 2),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(160, 2),
					},
					new Item[]
					{
						new Item(102, 32767),
						new Item(102, 32767),
						new Item(102, 32767),
						new Item(102, 32767),
						new Item(351, 13),
						new Item(102, 32767),
						new Item(102, 32767),
						new Item(102, 32767),
						new Item(102, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(159, 2),
					},
					new Item[]
					{
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(351, 13),
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(172, 32767),
					}, "crafting_table"),
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(213, 0),
					},
					new Item[]
					{
						new Item(378, 32767),
						new Item(378, 32767),
						new Item(378, 32767),
						new Item(378, 32767),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(378, 0, 1),
					},
					new List<Item>
					{
						new Item(377, 32767, 1),
						new Item(341, 32767, 1),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(395, 0),
					},
					new Item[]
					{
						new Item(339, 32767),
						new Item(339, 32767),
						new Item(339, 32767),
						new Item(339, 32767),
						new Item(339, 32767),
						new Item(339, 32767),
						new Item(339, 32767),
						new Item(339, 32767),
						new Item(339, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(103, 0),
					},
					new Item[]
					{
						new Item(360, 32767),
						new Item(360, 32767),
						new Item(360, 32767),
						new Item(360, 32767),
						new Item(360, 32767),
						new Item(360, 32767),
						new Item(360, 32767),
						new Item(360, 32767),
						new Item(360, 32767),
					}, "crafting_table"),
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(362, 0),
					},
					new Item[]
					{
						new Item(360, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(328, 0),
					},
					new Item[]
					{
						new Item(265, 32767),
						new Item(265, 32767),
						new Item(265, 32767),
						new Item(0, 0),
						new Item(265, 32767),
						new Item(265, 32767),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(48, 0, 1),
					},
					new List<Item>
					{
						new Item(4, 32767, 1),
						new Item(106, 32767, 1),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-179, 0),
					},
					new Item[]
					{
						new Item(48, 32767),
						new Item(48, 32767),
						new Item(48, 32767),
						new Item(0, 0),
						new Item(48, 32767),
						new Item(48, 32767),
						new Item(0, 0),
						new Item(0, 0),
						new Item(48, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(139, 1),
					},
					new Item[]
					{
						new Item(48, 32767),
						new Item(48, 32767),
						new Item(48, 32767),
						new Item(48, 32767),
						new Item(48, 32767),
						new Item(48, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-175, 0),
					},
					new Item[]
					{
						new Item(98, 1),
						new Item(98, 1),
						new Item(98, 1),
						new Item(0, 0),
						new Item(98, 1),
						new Item(98, 1),
						new Item(0, 0),
						new Item(0, 0),
						new Item(98, 1),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(139, 8),
					},
					new Item[]
					{
						new Item(98, 1),
						new Item(98, 1),
						new Item(98, 1),
						new Item(98, 1),
						new Item(98, 1),
						new Item(98, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(98, 1, 1),
					},
					new List<Item>
					{
						new Item(98, 0, 1),
						new Item(106, 32767, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(282, 0, 1),
					},
					new List<Item>
					{
						new Item(39, 32767, 1),
						new Item(40, 32767, 1),
						new Item(281, 32767, 1),
					}, "crafting_table"),
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(112, 0),
					},
					new Item[]
					{
						new Item(405, 32767),
						new Item(405, 32767),
						new Item(405, 32767),
						new Item(405, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(113, 0),
					},
					new Item[]
					{
						new Item(112, 32767),
						new Item(112, 32767),
						new Item(405, 32767),
						new Item(405, 32767),
						new Item(112, 32767),
						new Item(112, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(114, 0),
					},
					new Item[]
					{
						new Item(112, 32767),
						new Item(112, 32767),
						new Item(112, 32767),
						new Item(0, 0),
						new Item(112, 32767),
						new Item(112, 32767),
						new Item(0, 0),
						new Item(0, 0),
						new Item(112, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(139, 9),
					},
					new Item[]
					{
						new Item(112, 32767),
						new Item(112, 32767),
						new Item(112, 32767),
						new Item(112, 32767),
						new Item(112, 32767),
						new Item(112, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(214, 0),
					},
					new Item[]
					{
						new Item(372, 32767),
						new Item(372, 32767),
						new Item(372, 32767),
						new Item(372, 32767),
						new Item(372, 32767),
						new Item(372, 32767),
						new Item(372, 32767),
						new Item(372, 32767),
						new Item(372, 32767),
					}, "crafting_table"),
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(5, 0),
					},
					new Item[]
					{
						new Item(17, 0),
					}, "crafting_table"),
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(5, 0),
					},
					new Item[]
					{
						new Item(-10, 32767),
					}, "crafting_table"),
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(5, 0),
					},
					new Item[]
					{
						new Item(-212, 8),
					}, "crafting_table"),
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(5, 0),
					},
					new Item[]
					{
						new Item(-212, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(53, 0),
					},
					new Item[]
					{
						new Item(5, 0),
						new Item(5, 0),
						new Item(5, 0),
						new Item(0, 0),
						new Item(5, 0),
						new Item(5, 0),
						new Item(0, 0),
						new Item(0, 0),
						new Item(5, 0),
					}, "crafting_table"),
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(-212, 0),
					},
					new Item[]
					{
						new Item(17, 0),
						new Item(17, 0),
						new Item(17, 0),
						new Item(17, 0),
					}, "crafting_table"),
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(-212, 8),
					},
					new Item[]
					{
						new Item(-10, 32767),
						new Item(-10, 32767),
						new Item(-10, 32767),
						new Item(-10, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(158, 0),
					},
					new Item[]
					{
						new Item(5, 0),
						new Item(5, 0),
						new Item(5, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(251, 0),
					},
					new Item[]
					{
						new Item(4, 32767),
						new Item(331, 32767),
						new Item(4, 32767),
						new Item(4, 32767),
						new Item(331, 32767),
						new Item(4, 32767),
						new Item(4, 32767),
						new Item(406, 32767),
						new Item(4, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(446, 14),
					},
					new Item[]
					{
						new Item(35, 1),
						new Item(35, 1),
						new Item(0, 0),
						new Item(35, 1),
						new Item(35, 1),
						new Item(280, 32767),
						new Item(35, 1),
						new Item(35, 1),
						new Item(0, 0),
					}, "crafting_table"),
				new ShapedRecipe(2, 1,
					new List<Item>
					{
						new Item(171, 1),
					},
					new Item[]
					{
						new Item(35, 1),
						new Item(35, 1),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(171, 1),
					},
					new Item[]
					{
						new Item(171, 0),
						new Item(171, 0),
						new Item(171, 0),
						new Item(171, 0),
						new Item(351, 14),
						new Item(171, 0),
						new Item(171, 0),
						new Item(171, 0),
						new Item(171, 0),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(237, 1, 8),
					},
					new List<Item>
					{
						new Item(351, 14, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(13, 32767, 1),
						new Item(13, 32767, 1),
						new Item(13, 32767, 1),
						new Item(13, 32767, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 14, 1),
					},
					new List<Item>
					{
						new Item(38, 5, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 14, 2),
					},
					new List<Item>
					{
						new Item(351, 1, 1),
						new Item(351, 11, 1),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(241, 1),
					},
					new Item[]
					{
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(351, 14),
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(160, 1),
					},
					new Item[]
					{
						new Item(241, 1),
						new Item(241, 1),
						new Item(241, 1),
						new Item(241, 1),
						new Item(241, 1),
						new Item(241, 1),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(160, 1),
					},
					new Item[]
					{
						new Item(102, 32767),
						new Item(102, 32767),
						new Item(102, 32767),
						new Item(102, 32767),
						new Item(351, 14),
						new Item(102, 32767),
						new Item(102, 32767),
						new Item(102, 32767),
						new Item(102, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(159, 1),
					},
					new Item[]
					{
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(351, 14),
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(172, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(174, 0),
					},
					new Item[]
					{
						new Item(79, 32767),
						new Item(79, 32767),
						new Item(79, 32767),
						new Item(79, 32767),
						new Item(79, 32767),
						new Item(79, 32767),
						new Item(79, 32767),
						new Item(79, 32767),
						new Item(79, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(339, 0),
					},
					new Item[]
					{
						new Item(338, 32767),
						new Item(338, 32767),
						new Item(338, 32767),
					}, "crafting_table"),
				new ShapedRecipe(1, 2,
					new List<Item>
					{
						new Item(155, 2),
					},
					new Item[]
					{
						new Item(155, 0),
						new Item(155, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(446, 9),
					},
					new Item[]
					{
						new Item(35, 6),
						new Item(35, 6),
						new Item(0, 0),
						new Item(35, 6),
						new Item(35, 6),
						new Item(280, 32767),
						new Item(35, 6),
						new Item(35, 6),
						new Item(0, 0),
					}, "crafting_table"),
				new ShapedRecipe(2, 1,
					new List<Item>
					{
						new Item(171, 6),
					},
					new Item[]
					{
						new Item(35, 6),
						new Item(35, 6),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(171, 6),
					},
					new Item[]
					{
						new Item(171, 0),
						new Item(171, 0),
						new Item(171, 0),
						new Item(171, 0),
						new Item(351, 9),
						new Item(171, 0),
						new Item(171, 0),
						new Item(171, 0),
						new Item(171, 0),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(237, 6, 8),
					},
					new List<Item>
					{
						new Item(351, 9, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(13, 32767, 1),
						new Item(13, 32767, 1),
						new Item(13, 32767, 1),
						new Item(13, 32767, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 9, 2),
					},
					new List<Item>
					{
						new Item(351, 1, 1),
						new Item(351, 19, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 9, 2),
					},
					new List<Item>
					{
						new Item(175, 5, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 9, 1),
					},
					new List<Item>
					{
						new Item(38, 7, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 9, 2),
					},
					new List<Item>
					{
						new Item(351, 1, 1),
						new Item(351, 15, 1),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(241, 6),
					},
					new Item[]
					{
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(351, 9),
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(160, 6),
					},
					new Item[]
					{
						new Item(241, 6),
						new Item(241, 6),
						new Item(241, 6),
						new Item(241, 6),
						new Item(241, 6),
						new Item(241, 6),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(160, 6),
					},
					new Item[]
					{
						new Item(102, 32767),
						new Item(102, 32767),
						new Item(102, 32767),
						new Item(102, 32767),
						new Item(351, 9),
						new Item(102, 32767),
						new Item(102, 32767),
						new Item(102, 32767),
						new Item(102, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(159, 6),
					},
					new Item[]
					{
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(351, 9),
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(172, 32767),
					}, "crafting_table"),
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(1, 6),
					},
					new Item[]
					{
						new Item(1, 5),
						new Item(1, 5),
						new Item(1, 5),
						new Item(1, 5),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-174, 0),
					},
					new Item[]
					{
						new Item(1, 6),
						new Item(1, 6),
						new Item(1, 6),
						new Item(0, 0),
						new Item(1, 6),
						new Item(1, 6),
						new Item(0, 0),
						new Item(0, 0),
						new Item(1, 6),
					}, "crafting_table"),
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(1, 4),
					},
					new Item[]
					{
						new Item(1, 3),
						new Item(1, 3),
						new Item(1, 3),
						new Item(1, 3),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-173, 0),
					},
					new Item[]
					{
						new Item(1, 4),
						new Item(1, 4),
						new Item(1, 4),
						new Item(0, 0),
						new Item(1, 4),
						new Item(1, 4),
						new Item(0, 0),
						new Item(0, 0),
						new Item(1, 4),
					}, "crafting_table"),
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(1, 2),
					},
					new Item[]
					{
						new Item(1, 1),
						new Item(1, 1),
						new Item(1, 1),
						new Item(1, 1),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-172, 0),
					},
					new Item[]
					{
						new Item(1, 2),
						new Item(1, 2),
						new Item(1, 2),
						new Item(0, 0),
						new Item(1, 2),
						new Item(1, 2),
						new Item(0, 0),
						new Item(0, 0),
						new Item(1, 2),
					}, "crafting_table"),
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(168, 0),
					},
					new Item[]
					{
						new Item(409, 32767),
						new Item(409, 32767),
						new Item(409, 32767),
						new Item(409, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(168, 2),
					},
					new Item[]
					{
						new Item(409, 32767),
						new Item(409, 32767),
						new Item(409, 32767),
						new Item(409, 32767),
						new Item(409, 32767),
						new Item(409, 32767),
						new Item(409, 32767),
						new Item(409, 32767),
						new Item(409, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-2, 0),
					},
					new Item[]
					{
						new Item(168, 0),
						new Item(168, 0),
						new Item(168, 0),
						new Item(0, 0),
						new Item(168, 0),
						new Item(168, 0),
						new Item(0, 0),
						new Item(0, 0),
						new Item(168, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-4, 0),
					},
					new Item[]
					{
						new Item(168, 2),
						new Item(168, 2),
						new Item(168, 2),
						new Item(0, 0),
						new Item(168, 2),
						new Item(168, 2),
						new Item(0, 0),
						new Item(0, 0),
						new Item(168, 2),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-3, 0),
					},
					new Item[]
					{
						new Item(168, 1),
						new Item(168, 1),
						new Item(168, 1),
						new Item(0, 0),
						new Item(168, 1),
						new Item(168, 1),
						new Item(0, 0),
						new Item(0, 0),
						new Item(168, 1),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(139, 11),
					},
					new Item[]
					{
						new Item(168, 0),
						new Item(168, 0),
						new Item(168, 0),
						new Item(168, 0),
						new Item(168, 0),
						new Item(168, 0),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(400, 0, 1),
					},
					new List<Item>
					{
						new Item(86, 32767, 1),
						new Item(353, 32767, 1),
						new Item(344, 32767, 1),
					}, "crafting_table"),
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(361, 0),
					},
					new Item[]
					{
						new Item(86, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(446, 5),
					},
					new Item[]
					{
						new Item(35, 10),
						new Item(35, 10),
						new Item(0, 0),
						new Item(35, 10),
						new Item(35, 10),
						new Item(280, 32767),
						new Item(35, 10),
						new Item(35, 10),
						new Item(0, 0),
					}, "crafting_table"),
				new ShapedRecipe(2, 1,
					new List<Item>
					{
						new Item(171, 10),
					},
					new Item[]
					{
						new Item(35, 10),
						new Item(35, 10),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(171, 10),
					},
					new Item[]
					{
						new Item(171, 0),
						new Item(171, 0),
						new Item(171, 0),
						new Item(171, 0),
						new Item(351, 5),
						new Item(171, 0),
						new Item(171, 0),
						new Item(171, 0),
						new Item(171, 0),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(237, 10, 8),
					},
					new List<Item>
					{
						new Item(351, 5, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(13, 32767, 1),
						new Item(13, 32767, 1),
						new Item(13, 32767, 1),
						new Item(13, 32767, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 5, 2),
					},
					new List<Item>
					{
						new Item(351, 18, 1),
						new Item(351, 1, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 5, 2),
					},
					new List<Item>
					{
						new Item(351, 4, 1),
						new Item(351, 1, 1),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(241, 10),
					},
					new Item[]
					{
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(351, 5),
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(160, 10),
					},
					new Item[]
					{
						new Item(241, 10),
						new Item(241, 10),
						new Item(241, 10),
						new Item(241, 10),
						new Item(241, 10),
						new Item(241, 10),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(160, 10),
					},
					new Item[]
					{
						new Item(102, 32767),
						new Item(102, 32767),
						new Item(102, 32767),
						new Item(102, 32767),
						new Item(351, 5),
						new Item(102, 32767),
						new Item(102, 32767),
						new Item(102, 32767),
						new Item(102, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(159, 10),
					},
					new Item[]
					{
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(351, 5),
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(172, 32767),
					}, "crafting_table"),
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(201, 0),
					},
					new Item[]
					{
						new Item(433, 32767),
						new Item(433, 32767),
						new Item(433, 32767),
						new Item(433, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(203, 0),
					},
					new Item[]
					{
						new Item(201, 32767),
						new Item(201, 32767),
						new Item(201, 32767),
						new Item(0, 0),
						new Item(201, 32767),
						new Item(201, 32767),
						new Item(0, 0),
						new Item(0, 0),
						new Item(201, 32767),
					}, "crafting_table"),
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(155, 0),
					},
					new Item[]
					{
						new Item(406, 32767),
						new Item(406, 32767),
						new Item(406, 32767),
						new Item(406, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(156, 0),
					},
					new Item[]
					{
						new Item(155, 0),
						new Item(155, 0),
						new Item(155, 0),
						new Item(0, 0),
						new Item(155, 0),
						new Item(155, 0),
						new Item(0, 0),
						new Item(0, 0),
						new Item(155, 0),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(413, 0, 1),
					},
					new List<Item>
					{
						new Item(281, 32767, 1),
						new Item(393, 32767, 1),
						new Item(391, 32767, 1),
						new Item(39, 32767, 1),
						new Item(412, 32767, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(413, 0, 1),
					},
					new List<Item>
					{
						new Item(281, 32767, 1),
						new Item(393, 32767, 1),
						new Item(391, 32767, 1),
						new Item(40, 32767, 1),
						new Item(412, 32767, 1),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(66, 0),
					},
					new Item[]
					{
						new Item(265, 32767),
						new Item(265, 32767),
						new Item(265, 32767),
						new Item(0, 0),
						new Item(280, 32767),
						new Item(0, 0),
						new Item(265, 32767),
						new Item(265, 32767),
						new Item(265, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(446, 1),
					},
					new Item[]
					{
						new Item(35, 14),
						new Item(35, 14),
						new Item(0, 0),
						new Item(35, 14),
						new Item(35, 14),
						new Item(280, 32767),
						new Item(35, 14),
						new Item(35, 14),
						new Item(0, 0),
					}, "crafting_table"),
				new ShapedRecipe(2, 1,
					new List<Item>
					{
						new Item(171, 14),
					},
					new Item[]
					{
						new Item(35, 14),
						new Item(35, 14),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(171, 14),
					},
					new Item[]
					{
						new Item(171, 0),
						new Item(171, 0),
						new Item(171, 0),
						new Item(171, 0),
						new Item(351, 1),
						new Item(171, 0),
						new Item(171, 0),
						new Item(171, 0),
						new Item(171, 0),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(237, 14, 8),
					},
					new List<Item>
					{
						new Item(351, 1, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(13, 32767, 1),
						new Item(13, 32767, 1),
						new Item(13, 32767, 1),
						new Item(13, 32767, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 1, 1),
					},
					new List<Item>
					{
						new Item(457, 32767, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 1, 1),
					},
					new List<Item>
					{
						new Item(38, 0, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 1, 2),
					},
					new List<Item>
					{
						new Item(175, 4, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 1, 1),
					},
					new List<Item>
					{
						new Item(38, 4, 1),
					}, "crafting_table"),
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(215, 0),
					},
					new Item[]
					{
						new Item(405, 32767),
						new Item(372, 32767),
						new Item(372, 32767),
						new Item(405, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-184, 0),
					},
					new Item[]
					{
						new Item(215, 32767),
						new Item(215, 32767),
						new Item(215, 32767),
						new Item(0, 0),
						new Item(215, 32767),
						new Item(215, 32767),
						new Item(0, 0),
						new Item(0, 0),
						new Item(215, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(139, 13),
					},
					new Item[]
					{
						new Item(215, 32767),
						new Item(215, 32767),
						new Item(215, 32767),
						new Item(215, 32767),
						new Item(215, 32767),
						new Item(215, 32767),
					}, "crafting_table"),
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(179, 0),
					},
					new Item[]
					{
						new Item(12, 1),
						new Item(12, 1),
						new Item(12, 1),
						new Item(12, 1),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(180, 0),
					},
					new Item[]
					{
						new Item(179, 0),
						new Item(179, 0),
						new Item(179, 0),
						new Item(0, 0),
						new Item(179, 0),
						new Item(179, 0),
						new Item(0, 0),
						new Item(0, 0),
						new Item(179, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(139, 12),
					},
					new Item[]
					{
						new Item(179, 0),
						new Item(179, 0),
						new Item(179, 0),
						new Item(179, 0),
						new Item(179, 0),
						new Item(179, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(241, 14),
					},
					new Item[]
					{
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(351, 1),
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(160, 14),
					},
					new Item[]
					{
						new Item(241, 14),
						new Item(241, 14),
						new Item(241, 14),
						new Item(241, 14),
						new Item(241, 14),
						new Item(241, 14),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(160, 14),
					},
					new Item[]
					{
						new Item(102, 32767),
						new Item(102, 32767),
						new Item(102, 32767),
						new Item(102, 32767),
						new Item(351, 1),
						new Item(102, 32767),
						new Item(102, 32767),
						new Item(102, 32767),
						new Item(102, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(159, 14),
					},
					new Item[]
					{
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(351, 1),
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(172, 32767),
					}, "crafting_table"),
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(331, 0),
					},
					new Item[]
					{
						new Item(152, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(152, 0),
					},
					new Item[]
					{
						new Item(331, 32767),
						new Item(331, 32767),
						new Item(331, 32767),
						new Item(331, 32767),
						new Item(331, 32767),
						new Item(331, 32767),
						new Item(331, 32767),
						new Item(331, 32767),
						new Item(331, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(123, 0),
					},
					new Item[]
					{
						new Item(0, 0),
						new Item(331, 32767),
						new Item(0, 0),
						new Item(331, 32767),
						new Item(89, 32767),
						new Item(331, 32767),
						new Item(0, 0),
						new Item(331, 32767),
						new Item(0, 0),
					}, "crafting_table"),
				new ShapedRecipe(1, 2,
					new List<Item>
					{
						new Item(76, 0),
					},
					new Item[]
					{
						new Item(331, 32767),
						new Item(280, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(356, 0),
					},
					new Item[]
					{
						new Item(76, 32767),
						new Item(76, 32767),
						new Item(1, 0),
						new Item(331, 32767),
						new Item(1, 0),
						new Item(1, 0),
					}, "crafting_table"),
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(24, 0),
					},
					new Item[]
					{
						new Item(12, 0),
						new Item(12, 0),
						new Item(12, 0),
						new Item(12, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(128, 0),
					},
					new Item[]
					{
						new Item(24, 0),
						new Item(24, 0),
						new Item(24, 0),
						new Item(0, 0),
						new Item(24, 0),
						new Item(24, 0),
						new Item(0, 0),
						new Item(0, 0),
						new Item(24, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(139, 5),
					},
					new Item[]
					{
						new Item(24, 0),
						new Item(24, 0),
						new Item(24, 0),
						new Item(24, 0),
						new Item(24, 0),
						new Item(24, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-165, 0),
					},
					new Item[]
					{
						new Item(-163, 32767),
						new Item(-163, 32767),
						new Item(-163, 32767),
						new Item(287, 32767),
						new Item(0, 0),
						new Item(0, 0),
						new Item(-163, 32767),
						new Item(-163, 32767),
						new Item(-163, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(169, 0),
					},
					new Item[]
					{
						new Item(409, 32767),
						new Item(422, 32767),
						new Item(409, 32767),
						new Item(422, 32767),
						new Item(422, 32767),
						new Item(422, 32767),
						new Item(409, 32767),
						new Item(422, 32767),
						new Item(409, 32767),
					}, "crafting_table"),
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(359, 0),
					},
					new Item[]
					{
						new Item(0, 0),
						new Item(265, 32767),
						new Item(265, 32767),
						new Item(0, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(513, 0),
					},
					new Item[]
					{
						new Item(5, 32767),
						new Item(5, 32767),
						new Item(0, 0),
						new Item(265, 32767),
						new Item(5, 32767),
						new Item(5, 32767),
						new Item(5, 32767),
						new Item(5, 32767),
						new Item(0, 0),
					}, "crafting_table"),
				new ShapedRecipe(1, 3,
					new List<Item>
					{
						new Item(205, 0),
					},
					new Item[]
					{
						new Item(445, 32767),
						new Item(54, 32767),
						new Item(445, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(475, 0),
					},
					new Item[]
					{
						new Item(5, 4),
						new Item(5, 4),
						new Item(0, 0),
						new Item(5, 4),
						new Item(5, 4),
						new Item(280, 32767),
						new Item(5, 4),
						new Item(5, 4),
						new Item(0, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(473, 0),
					},
					new Item[]
					{
						new Item(5, 2),
						new Item(5, 2),
						new Item(0, 0),
						new Item(5, 2),
						new Item(5, 2),
						new Item(280, 32767),
						new Item(5, 2),
						new Item(5, 2),
						new Item(0, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(476, 0),
					},
					new Item[]
					{
						new Item(5, 5),
						new Item(5, 5),
						new Item(0, 0),
						new Item(5, 5),
						new Item(5, 5),
						new Item(280, 32767),
						new Item(5, 5),
						new Item(5, 5),
						new Item(0, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(474, 0),
					},
					new Item[]
					{
						new Item(5, 3),
						new Item(5, 3),
						new Item(0, 0),
						new Item(5, 3),
						new Item(5, 3),
						new Item(280, 32767),
						new Item(5, 3),
						new Item(5, 3),
						new Item(0, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(323, 0),
					},
					new Item[]
					{
						new Item(5, 0),
						new Item(5, 0),
						new Item(0, 0),
						new Item(5, 0),
						new Item(5, 0),
						new Item(280, 32767),
						new Item(5, 0),
						new Item(5, 0),
						new Item(0, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(472, 0),
					},
					new Item[]
					{
						new Item(5, 1),
						new Item(5, 1),
						new Item(0, 0),
						new Item(5, 1),
						new Item(5, 1),
						new Item(280, 32767),
						new Item(5, 1),
						new Item(5, 1),
						new Item(0, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(165, 0),
					},
					new Item[]
					{
						new Item(341, 32767),
						new Item(341, 32767),
						new Item(341, 32767),
						new Item(341, 32767),
						new Item(341, 32767),
						new Item(341, 32767),
						new Item(341, 32767),
						new Item(341, 32767),
						new Item(341, 32767),
					}, "crafting_table"),
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(341, 0),
					},
					new Item[]
					{
						new Item(165, 32767),
					}, "crafting_table"),
				new ShapedRecipe(2, 3,
					new List<Item>
					{
						new Item(-202, 0),
					},
					new Item[]
					{
						new Item(265, 32767),
						new Item(5, 32767),
						new Item(265, 32767),
						new Item(5, 32767),
						new Item(5, 32767),
						new Item(5, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-198, 0),
					},
					new Item[]
					{
						new Item(0, 0),
						new Item(17, 32767),
						new Item(0, 0),
						new Item(17, 32767),
						new Item(61, 32767),
						new Item(17, 32767),
						new Item(0, 0),
						new Item(17, 32767),
						new Item(0, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-198, 0),
					},
					new Item[]
					{
						new Item(0, 0),
						new Item(162, 32767),
						new Item(0, 0),
						new Item(162, 32767),
						new Item(61, 32767),
						new Item(162, 32767),
						new Item(0, 0),
						new Item(162, 32767),
						new Item(0, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-198, 0),
					},
					new Item[]
					{
						new Item(0, 0),
						new Item(-8, 32767),
						new Item(0, 0),
						new Item(-8, 32767),
						new Item(61, 32767),
						new Item(-8, 32767),
						new Item(0, 0),
						new Item(-8, 32767),
						new Item(0, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-198, 0),
					},
					new Item[]
					{
						new Item(0, 0),
						new Item(-6, 32767),
						new Item(0, 0),
						new Item(-6, 32767),
						new Item(61, 32767),
						new Item(-6, 32767),
						new Item(0, 0),
						new Item(-6, 32767),
						new Item(0, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-198, 0),
					},
					new Item[]
					{
						new Item(0, 0),
						new Item(-9, 32767),
						new Item(0, 0),
						new Item(-9, 32767),
						new Item(61, 32767),
						new Item(-9, 32767),
						new Item(0, 0),
						new Item(-9, 32767),
						new Item(0, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-198, 0),
					},
					new Item[]
					{
						new Item(0, 0),
						new Item(-7, 32767),
						new Item(0, 0),
						new Item(-7, 32767),
						new Item(61, 32767),
						new Item(-7, 32767),
						new Item(0, 0),
						new Item(-7, 32767),
						new Item(0, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-198, 0),
					},
					new Item[]
					{
						new Item(0, 0),
						new Item(-10, 32767),
						new Item(0, 0),
						new Item(-10, 32767),
						new Item(61, 32767),
						new Item(-10, 32767),
						new Item(0, 0),
						new Item(-10, 32767),
						new Item(0, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-198, 0),
					},
					new Item[]
					{
						new Item(0, 0),
						new Item(-5, 32767),
						new Item(0, 0),
						new Item(-5, 32767),
						new Item(61, 32767),
						new Item(-5, 32767),
						new Item(0, 0),
						new Item(-5, 32767),
						new Item(0, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-185, 0),
					},
					new Item[]
					{
						new Item(155, 3),
						new Item(155, 3),
						new Item(155, 3),
						new Item(0, 0),
						new Item(155, 3),
						new Item(155, 3),
						new Item(0, 0),
						new Item(0, 0),
						new Item(155, 3),
					}, "crafting_table"),
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(179, 2),
					},
					new Item[]
					{
						new Item(179, 0),
						new Item(179, 0),
						new Item(179, 0),
						new Item(179, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-176, 0),
					},
					new Item[]
					{
						new Item(179, 3),
						new Item(179, 3),
						new Item(179, 3),
						new Item(0, 0),
						new Item(179, 3),
						new Item(179, 3),
						new Item(0, 0),
						new Item(0, 0),
						new Item(179, 3),
					}, "crafting_table"),
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(24, 2),
					},
					new Item[]
					{
						new Item(24, 0),
						new Item(24, 0),
						new Item(24, 0),
						new Item(24, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-177, 0),
					},
					new Item[]
					{
						new Item(24, 3),
						new Item(24, 3),
						new Item(24, 3),
						new Item(0, 0),
						new Item(24, 3),
						new Item(24, 3),
						new Item(0, 0),
						new Item(0, 0),
						new Item(24, 3),
					}, "crafting_table"),
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(80, 0),
					},
					new Item[]
					{
						new Item(332, 32767),
						new Item(332, 32767),
						new Item(332, 32767),
						new Item(332, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(78, 0),
					},
					new Item[]
					{
						new Item(80, 32767),
						new Item(80, 32767),
						new Item(80, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(382, 0),
					},
					new Item[]
					{
						new Item(371, 32767),
						new Item(371, 32767),
						new Item(371, 32767),
						new Item(371, 32767),
						new Item(360, 32767),
						new Item(371, 32767),
						new Item(371, 32767),
						new Item(371, 32767),
						new Item(371, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(333, 1),
					},
					new Item[]
					{
						new Item(5, 1),
						new Item(5, 1),
						new Item(5, 1),
						new Item(269, 0),
						new Item(5, 1),
						new Item(5, 1),
					}, "crafting_table"),
				new ShapedRecipe(2, 3,
					new List<Item>
					{
						new Item(427, 0),
					},
					new Item[]
					{
						new Item(5, 1),
						new Item(5, 1),
						new Item(5, 1),
						new Item(5, 1),
						new Item(5, 1),
						new Item(5, 1),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(85, 1),
					},
					new Item[]
					{
						new Item(5, 1),
						new Item(5, 1),
						new Item(280, 32767),
						new Item(280, 32767),
						new Item(5, 1),
						new Item(5, 1),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(183, 0),
					},
					new Item[]
					{
						new Item(280, 32767),
						new Item(280, 32767),
						new Item(5, 1),
						new Item(5, 1),
						new Item(280, 32767),
						new Item(280, 32767),
					}, "crafting_table"),
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(5, 1),
					},
					new Item[]
					{
						new Item(17, 1),
					}, "crafting_table"),
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(5, 1),
					},
					new Item[]
					{
						new Item(-5, 32767),
					}, "crafting_table"),
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(5, 1),
					},
					new Item[]
					{
						new Item(-212, 9),
					}, "crafting_table"),
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(5, 1),
					},
					new Item[]
					{
						new Item(-212, 1),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(134, 0),
					},
					new Item[]
					{
						new Item(5, 1),
						new Item(5, 1),
						new Item(5, 1),
						new Item(0, 0),
						new Item(5, 1),
						new Item(5, 1),
						new Item(0, 0),
						new Item(0, 0),
						new Item(5, 1),
					}, "crafting_table"),
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(-212, 1),
					},
					new Item[]
					{
						new Item(17, 1),
						new Item(17, 1),
						new Item(17, 1),
						new Item(17, 1),
					}, "crafting_table"),
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(-212, 9),
					},
					new Item[]
					{
						new Item(-5, 32767),
						new Item(-5, 32767),
						new Item(-5, 32767),
						new Item(-5, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(158, 1),
					},
					new Item[]
					{
						new Item(5, 1),
						new Item(5, 1),
						new Item(5, 1),
					}, "crafting_table"),
				new ShapedRecipe(1, 2,
					new List<Item>
					{
						new Item(29, 1),
					},
					new Item[]
					{
						new Item(341, 32767),
						new Item(33, 32767),
					}, "crafting_table"),
				new ShapedRecipe(2, 3,
					new List<Item>
					{
						new Item(275, 0),
					},
					new Item[]
					{
						new Item(4, 32767),
						new Item(280, 32767),
						new Item(4, 32767),
						new Item(0, 0),
						new Item(4, 32767),
						new Item(280, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(109, 0),
					},
					new Item[]
					{
						new Item(98, 0),
						new Item(98, 0),
						new Item(98, 0),
						new Item(0, 0),
						new Item(98, 0),
						new Item(98, 0),
						new Item(0, 0),
						new Item(0, 0),
						new Item(98, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(139, 7),
					},
					new Item[]
					{
						new Item(98, 0),
						new Item(98, 0),
						new Item(98, 0),
						new Item(98, 0),
						new Item(98, 0),
						new Item(98, 0),
					}, "crafting_table"),
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(77, 0),
					},
					new Item[]
					{
						new Item(1, 0),
					}, "crafting_table"),
				new ShapedRecipe(2, 3,
					new List<Item>
					{
						new Item(291, 0),
					},
					new Item[]
					{
						new Item(4, 32767),
						new Item(280, 32767),
						new Item(4, 32767),
						new Item(0, 0),
						new Item(0, 0),
						new Item(280, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(274, 0),
					},
					new Item[]
					{
						new Item(4, 32767),
						new Item(0, 0),
						new Item(0, 0),
						new Item(4, 32767),
						new Item(280, 32767),
						new Item(280, 32767),
						new Item(4, 32767),
						new Item(0, 0),
						new Item(0, 0),
					}, "crafting_table"),
				new ShapedRecipe(2, 1,
					new List<Item>
					{
						new Item(70, 0),
					},
					new Item[]
					{
						new Item(1, 0),
						new Item(1, 0),
					}, "crafting_table"),
				new ShapedRecipe(1, 3,
					new List<Item>
					{
						new Item(273, 0),
					},
					new Item[]
					{
						new Item(4, 32767),
						new Item(280, 32767),
						new Item(280, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-180, 0),
					},
					new Item[]
					{
						new Item(1, 0),
						new Item(1, 0),
						new Item(1, 0),
						new Item(0, 0),
						new Item(1, 0),
						new Item(1, 0),
						new Item(0, 0),
						new Item(0, 0),
						new Item(1, 0),
					}, "crafting_table"),
				new ShapedRecipe(1, 3,
					new List<Item>
					{
						new Item(272, 0),
					},
					new Item[]
					{
						new Item(4, 32767),
						new Item(4, 32767),
						new Item(280, 32767),
					}, "crafting_table"),
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(98, 0),
					},
					new Item[]
					{
						new Item(1, 0),
						new Item(1, 0),
						new Item(1, 0),
						new Item(1, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(-197, 0),
					},
					new Item[]
					{
						new Item(0, 0),
						new Item(0, 0),
						new Item(1, 32767),
						new Item(265, 32767),
						new Item(1, 32767),
						new Item(1, 32767),
					}, "crafting_table"),
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(35, 0),
					},
					new Item[]
					{
						new Item(287, 32767),
						new Item(287, 32767),
						new Item(287, 32767),
						new Item(287, 32767),
					}, "crafting_table"),
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(353, 0),
					},
					new Item[]
					{
						new Item(338, 32767),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(734, 7, 1),
					},
					new List<Item>
					{
						new Item(39, 32767, 1),
						new Item(40, 32767, 1),
						new Item(281, 32767, 1),
						new Item(38, 2, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(734, 3, 1),
					},
					new List<Item>
					{
						new Item(39, 32767, 1),
						new Item(40, 32767, 1),
						new Item(281, 32767, 1),
						new Item(38, 3, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(734, 6, 1),
					},
					new List<Item>
					{
						new Item(39, 32767, 1),
						new Item(40, 32767, 1),
						new Item(281, 32767, 1),
						new Item(38, 1, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(734, 1, 1),
					},
					new List<Item>
					{
						new Item(39, 32767, 1),
						new Item(40, 32767, 1),
						new Item(281, 32767, 1),
						new Item(38, 9, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(734, 5, 1),
					},
					new List<Item>
					{
						new Item(39, 32767, 1),
						new Item(40, 32767, 1),
						new Item(281, 32767, 1),
						new Item(37, 32767, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(734, 4, 1),
					},
					new List<Item>
					{
						new Item(39, 32767, 1),
						new Item(40, 32767, 1),
						new Item(281, 32767, 1),
						new Item(38, 10, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(734, 8, 1),
					},
					new List<Item>
					{
						new Item(39, 32767, 1),
						new Item(40, 32767, 1),
						new Item(281, 32767, 1),
						new Item(38, 8, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(734, 0, 1),
					},
					new List<Item>
					{
						new Item(39, 32767, 1),
						new Item(40, 32767, 1),
						new Item(281, 32767, 1),
						new Item(38, 0, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(734, 2, 1),
					},
					new List<Item>
					{
						new Item(39, 32767, 1),
						new Item(40, 32767, 1),
						new Item(281, 32767, 1),
						new Item(38, 5, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(734, 2, 1),
					},
					new List<Item>
					{
						new Item(39, 32767, 1),
						new Item(40, 32767, 1),
						new Item(281, 32767, 1),
						new Item(38, 7, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(734, 2, 1),
					},
					new List<Item>
					{
						new Item(39, 32767, 1),
						new Item(40, 32767, 1),
						new Item(281, 32767, 1),
						new Item(38, 4, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(734, 2, 1),
					},
					new List<Item>
					{
						new Item(39, 32767, 1),
						new Item(40, 32767, 1),
						new Item(281, 32767, 1),
						new Item(38, 6, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(734, 9, 1),
					},
					new List<Item>
					{
						new Item(39, 32767, 1),
						new Item(40, 32767, 1),
						new Item(281, 32767, 1),
						new Item(-216, 32767, 1),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(46, 0),
					},
					new Item[]
					{
						new Item(289, 32767),
						new Item(12, 32767),
						new Item(289, 32767),
						new Item(12, 32767),
						new Item(289, 32767),
						new Item(12, 32767),
						new Item(289, 32767),
						new Item(12, 32767),
						new Item(289, 32767),
					}, "crafting_table"),
				new ShapedRecipe(1, 2,
					new List<Item>
					{
						new Item(407, 0),
					},
					new Item[]
					{
						new Item(46, 0),
						new Item(328, 32767),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(146, 0, 1),
					},
					new List<Item>
					{
						new Item(54, 32767, 1),
						new Item(131, 32767, 1),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(469, 0),
					},
					new Item[]
					{
						new Item(468, 32767),
						new Item(468, 32767),
						new Item(0, 0),
						new Item(468, 32767),
						new Item(468, 32767),
						new Item(468, 32767),
					}, "crafting_table"),
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(296, 0),
					},
					new Item[]
					{
						new Item(170, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(446, 15),
					},
					new Item[]
					{
						new Item(35, 0),
						new Item(35, 0),
						new Item(0, 0),
						new Item(35, 0),
						new Item(35, 0),
						new Item(280, 32767),
						new Item(35, 0),
						new Item(35, 0),
						new Item(0, 0),
					}, "crafting_table"),
				new ShapedRecipe(2, 1,
					new List<Item>
					{
						new Item(171, 0),
					},
					new Item[]
					{
						new Item(35, 0),
						new Item(35, 0),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(237, 0, 8),
					},
					new List<Item>
					{
						new Item(351, 19, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(13, 32767, 1),
						new Item(13, 32767, 1),
						new Item(13, 32767, 1),
						new Item(13, 32767, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(237, 0, 8),
					},
					new List<Item>
					{
						new Item(351, 15, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(13, 32767, 1),
						new Item(13, 32767, 1),
						new Item(13, 32767, 1),
						new Item(13, 32767, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 19, 1),
					},
					new List<Item>
					{
						new Item(351, 15, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 19, 1),
					},
					new List<Item>
					{
						new Item(38, 10, 1),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(241, 0),
					},
					new Item[]
					{
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(351, 19),
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(241, 0),
					},
					new Item[]
					{
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(351, 15),
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(160, 0),
					},
					new Item[]
					{
						new Item(241, 0),
						new Item(241, 0),
						new Item(241, 0),
						new Item(241, 0),
						new Item(241, 0),
						new Item(241, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(160, 0),
					},
					new Item[]
					{
						new Item(102, 32767),
						new Item(102, 32767),
						new Item(102, 32767),
						new Item(102, 32767),
						new Item(351, 19),
						new Item(102, 32767),
						new Item(102, 32767),
						new Item(102, 32767),
						new Item(102, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(159, 0),
					},
					new Item[]
					{
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(351, 19),
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(172, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(159, 0),
					},
					new Item[]
					{
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(351, 15),
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(172, 32767),
					}, "crafting_table"),
				new ShapedRecipe(2, 3,
					new List<Item>
					{
						new Item(324, 0),
					},
					new Item[]
					{
						new Item(5, 0),
						new Item(5, 0),
						new Item(5, 0),
						new Item(5, 0),
						new Item(5, 0),
						new Item(5, 0),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(386, 0, 1),
					},
					new List<Item>
					{
						new Item(340, 32767, 1),
						new Item(351, 0, 1),
						new Item(288, 32767, 1),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(446, 11),
					},
					new Item[]
					{
						new Item(35, 4),
						new Item(35, 4),
						new Item(0, 0),
						new Item(35, 4),
						new Item(35, 4),
						new Item(280, 32767),
						new Item(35, 4),
						new Item(35, 4),
						new Item(0, 0),
					}, "crafting_table"),
				new ShapedRecipe(2, 1,
					new List<Item>
					{
						new Item(171, 4),
					},
					new Item[]
					{
						new Item(35, 4),
						new Item(35, 4),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(171, 4),
					},
					new Item[]
					{
						new Item(171, 0),
						new Item(171, 0),
						new Item(171, 0),
						new Item(171, 0),
						new Item(351, 11),
						new Item(171, 0),
						new Item(171, 0),
						new Item(171, 0),
						new Item(171, 0),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(237, 4, 8),
					},
					new List<Item>
					{
						new Item(351, 11, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(13, 32767, 1),
						new Item(13, 32767, 1),
						new Item(13, 32767, 1),
						new Item(13, 32767, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 11, 1),
					},
					new List<Item>
					{
						new Item(37, 0, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 11, 2),
					},
					new List<Item>
					{
						new Item(175, 0, 1),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(241, 4),
					},
					new Item[]
					{
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(351, 11),
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
						new Item(20, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(160, 4),
					},
					new Item[]
					{
						new Item(241, 4),
						new Item(241, 4),
						new Item(241, 4),
						new Item(241, 4),
						new Item(241, 4),
						new Item(241, 4),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(160, 4),
					},
					new Item[]
					{
						new Item(102, 32767),
						new Item(102, 32767),
						new Item(102, 32767),
						new Item(102, 32767),
						new Item(351, 11),
						new Item(102, 32767),
						new Item(102, 32767),
						new Item(102, 32767),
						new Item(102, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(159, 4),
					},
					new Item[]
					{
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(351, 11),
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(172, 32767),
						new Item(172, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(53, 0),
					},
					new Item[]
					{
						new Item(5, 0),
						new Item(5, 0),
						new Item(5, 0),
						new Item(0, 0),
						new Item(5, 0),
						new Item(5, 0),
						new Item(0, 0),
						new Item(0, 0),
						new Item(5, 0),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(401, 0, 3),
					},
					new List<Item>
					{
						new Item(339, 32767, 1),
						new Item(289, 32767, 1),
						new Item(402, 32767, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(401, 0, 3),
					},
					new List<Item>
					{
						new Item(339, 32767, 1),
						new Item(289, 32767, 1),
						new Item(402, 32767, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(401, 0, 3),
					},
					new List<Item>
					{
						new Item(339, 32767, 1),
						new Item(289, 32767, 1),
						new Item(402, 32767, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(401, 0, 3),
					},
					new List<Item>
					{
						new Item(339, 32767, 1),
						new Item(289, 32767, 1),
						new Item(402, 32767, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(401, 0, 3),
					},
					new List<Item>
					{
						new Item(339, 32767, 1),
						new Item(289, 32767, 1),
						new Item(402, 32767, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(401, 0, 3),
					},
					new List<Item>
					{
						new Item(339, 32767, 1),
						new Item(289, 32767, 1),
						new Item(402, 32767, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(401, 0, 3),
					},
					new List<Item>
					{
						new Item(339, 32767, 1),
						new Item(289, 32767, 1),
						new Item(402, 32767, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(401, 0, 3),
					},
					new List<Item>
					{
						new Item(339, 32767, 1),
						new Item(289, 32767, 1),
						new Item(402, 32767, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(401, 0, 3),
					},
					new List<Item>
					{
						new Item(339, 32767, 1),
						new Item(289, 32767, 1),
						new Item(402, 32767, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(401, 0, 3),
					},
					new List<Item>
					{
						new Item(339, 32767, 1),
						new Item(289, 32767, 1),
						new Item(402, 32767, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(401, 0, 3),
					},
					new List<Item>
					{
						new Item(339, 32767, 1),
						new Item(289, 32767, 1),
						new Item(402, 32767, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(401, 0, 3),
					},
					new List<Item>
					{
						new Item(339, 32767, 1),
						new Item(289, 32767, 1),
						new Item(402, 32767, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(401, 0, 3),
					},
					new List<Item>
					{
						new Item(339, 32767, 1),
						new Item(289, 32767, 1),
						new Item(402, 32767, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(401, 0, 3),
					},
					new List<Item>
					{
						new Item(339, 32767, 1),
						new Item(289, 32767, 1),
						new Item(402, 32767, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(401, 0, 3),
					},
					new List<Item>
					{
						new Item(339, 32767, 1),
						new Item(289, 32767, 1),
						new Item(402, 32767, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(401, 0, 3),
					},
					new List<Item>
					{
						new Item(339, 32767, 1),
						new Item(289, 32767, 1),
						new Item(402, 32767, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(401, 0, 3),
					},
					new List<Item>
					{
						new Item(339, 32767, 1),
						new Item(289, 32767, 1),
						new Item(402, 32767, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(401, 0, 3),
					},
					new List<Item>
					{
						new Item(339, 32767, 1),
						new Item(289, 32767, 1),
						new Item(402, 32767, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(401, 0, 3),
					},
					new List<Item>
					{
						new Item(339, 32767, 1),
						new Item(289, 32767, 1),
						new Item(402, 32767, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(401, 0, 3),
					},
					new List<Item>
					{
						new Item(339, 32767, 1),
						new Item(289, 32767, 1),
						new Item(402, 32767, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(402, 0, 1),
					},
					new List<Item>
					{
						new Item(289, 32767, 1),
						new Item(351, 0, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(402, 10, 1),
					},
					new List<Item>
					{
						new Item(289, 32767, 1),
						new Item(351, 10, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(402, 11, 1),
					},
					new List<Item>
					{
						new Item(289, 32767, 1),
						new Item(351, 11, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(402, 12, 1),
					},
					new List<Item>
					{
						new Item(289, 32767, 1),
						new Item(351, 12, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(402, 13, 1),
					},
					new List<Item>
					{
						new Item(289, 32767, 1),
						new Item(351, 13, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(402, 14, 1),
					},
					new List<Item>
					{
						new Item(289, 32767, 1),
						new Item(351, 14, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(402, 15, 1),
					},
					new List<Item>
					{
						new Item(289, 32767, 1),
						new Item(351, 15, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(402, 0, 1),
					},
					new List<Item>
					{
						new Item(289, 32767, 1),
						new Item(351, 16, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(402, 3, 1),
					},
					new List<Item>
					{
						new Item(289, 32767, 1),
						new Item(351, 17, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(402, 4, 1),
					},
					new List<Item>
					{
						new Item(289, 32767, 1),
						new Item(351, 18, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(402, 15, 1),
					},
					new List<Item>
					{
						new Item(289, 32767, 1),
						new Item(351, 19, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(402, 1, 1),
					},
					new List<Item>
					{
						new Item(289, 32767, 1),
						new Item(351, 1, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(402, 2, 1),
					},
					new List<Item>
					{
						new Item(289, 32767, 1),
						new Item(351, 2, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(402, 3, 1),
					},
					new List<Item>
					{
						new Item(289, 32767, 1),
						new Item(351, 3, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(402, 4, 1),
					},
					new List<Item>
					{
						new Item(289, 32767, 1),
						new Item(351, 4, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(402, 5, 1),
					},
					new List<Item>
					{
						new Item(289, 32767, 1),
						new Item(351, 5, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(402, 6, 1),
					},
					new List<Item>
					{
						new Item(289, 32767, 1),
						new Item(351, 6, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(402, 7, 1),
					},
					new List<Item>
					{
						new Item(289, 32767, 1),
						new Item(351, 7, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(402, 8, 1),
					},
					new List<Item>
					{
						new Item(289, 32767, 1),
						new Item(351, 8, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(402, 9, 1),
					},
					new List<Item>
					{
						new Item(289, 32767, 1),
						new Item(351, 9, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(401, 0, 3),
					},
					new List<Item>
					{
						new Item(339, 32767, 1),
						new Item(289, 32767, 1),
					}, "crafting_table"),
				new MultiRecipe() { Id = new UUID("00000000-0000-0000-0000-000000000001") }, // 00000000-0000-0000-0000-000000000001
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 15, 1),
					},
					new List<Item>
					{
						new Item(218, 5, 1),
						new Item(351, 0, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 15, 1),
					},
					new List<Item>
					{
						new Item(218, 4, 1),
						new Item(351, 0, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 15, 1),
					},
					new List<Item>
					{
						new Item(218, 3, 1),
						new Item(351, 0, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 15, 1),
					},
					new List<Item>
					{
						new Item(218, 2, 1),
						new Item(351, 0, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 15, 1),
					},
					new List<Item>
					{
						new Item(218, 1, 1),
						new Item(351, 0, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 15, 1),
					},
					new List<Item>
					{
						new Item(218, 0, 1),
						new Item(351, 0, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 15, 1),
					},
					new List<Item>
					{
						new Item(218, 14, 1),
						new Item(351, 0, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 15, 1),
					},
					new List<Item>
					{
						new Item(218, 13, 1),
						new Item(351, 0, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 15, 1),
					},
					new List<Item>
					{
						new Item(218, 12, 1),
						new Item(351, 0, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 15, 1),
					},
					new List<Item>
					{
						new Item(218, 11, 1),
						new Item(351, 0, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 15, 1),
					},
					new List<Item>
					{
						new Item(218, 10, 1),
						new Item(351, 0, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 15, 1),
					},
					new List<Item>
					{
						new Item(218, 9, 1),
						new Item(351, 0, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 15, 1),
					},
					new List<Item>
					{
						new Item(218, 8, 1),
						new Item(351, 0, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 15, 1),
					},
					new List<Item>
					{
						new Item(218, 7, 1),
						new Item(351, 0, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 15, 1),
					},
					new List<Item>
					{
						new Item(218, 6, 1),
						new Item(351, 0, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 5, 1),
					},
					new List<Item>
					{
						new Item(218, 15, 1),
						new Item(351, 10, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 5, 1),
					},
					new List<Item>
					{
						new Item(218, 4, 1),
						new Item(351, 10, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 5, 1),
					},
					new List<Item>
					{
						new Item(218, 3, 1),
						new Item(351, 10, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 5, 1),
					},
					new List<Item>
					{
						new Item(218, 2, 1),
						new Item(351, 10, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 5, 1),
					},
					new List<Item>
					{
						new Item(218, 1, 1),
						new Item(351, 10, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 5, 1),
					},
					new List<Item>
					{
						new Item(218, 0, 1),
						new Item(351, 10, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 5, 1),
					},
					new List<Item>
					{
						new Item(218, 14, 1),
						new Item(351, 10, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 5, 1),
					},
					new List<Item>
					{
						new Item(218, 13, 1),
						new Item(351, 10, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 5, 1),
					},
					new List<Item>
					{
						new Item(218, 12, 1),
						new Item(351, 10, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 5, 1),
					},
					new List<Item>
					{
						new Item(218, 11, 1),
						new Item(351, 10, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 5, 1),
					},
					new List<Item>
					{
						new Item(218, 10, 1),
						new Item(351, 10, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 5, 1),
					},
					new List<Item>
					{
						new Item(218, 9, 1),
						new Item(351, 10, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 5, 1),
					},
					new List<Item>
					{
						new Item(218, 8, 1),
						new Item(351, 10, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 5, 1),
					},
					new List<Item>
					{
						new Item(218, 7, 1),
						new Item(351, 10, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 5, 1),
					},
					new List<Item>
					{
						new Item(218, 6, 1),
						new Item(351, 10, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 4, 1),
					},
					new List<Item>
					{
						new Item(218, 15, 1),
						new Item(351, 11, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 4, 1),
					},
					new List<Item>
					{
						new Item(218, 5, 1),
						new Item(351, 11, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 4, 1),
					},
					new List<Item>
					{
						new Item(218, 3, 1),
						new Item(351, 11, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 4, 1),
					},
					new List<Item>
					{
						new Item(218, 2, 1),
						new Item(351, 11, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 4, 1),
					},
					new List<Item>
					{
						new Item(218, 1, 1),
						new Item(351, 11, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 4, 1),
					},
					new List<Item>
					{
						new Item(218, 0, 1),
						new Item(351, 11, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 4, 1),
					},
					new List<Item>
					{
						new Item(218, 14, 1),
						new Item(351, 11, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 4, 1),
					},
					new List<Item>
					{
						new Item(218, 13, 1),
						new Item(351, 11, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 4, 1),
					},
					new List<Item>
					{
						new Item(218, 12, 1),
						new Item(351, 11, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 4, 1),
					},
					new List<Item>
					{
						new Item(218, 11, 1),
						new Item(351, 11, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 4, 1),
					},
					new List<Item>
					{
						new Item(218, 10, 1),
						new Item(351, 11, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 4, 1),
					},
					new List<Item>
					{
						new Item(218, 9, 1),
						new Item(351, 11, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 4, 1),
					},
					new List<Item>
					{
						new Item(218, 8, 1),
						new Item(351, 11, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 4, 1),
					},
					new List<Item>
					{
						new Item(218, 7, 1),
						new Item(351, 11, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 4, 1),
					},
					new List<Item>
					{
						new Item(218, 6, 1),
						new Item(351, 11, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 3, 1),
					},
					new List<Item>
					{
						new Item(218, 15, 1),
						new Item(351, 12, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 3, 1),
					},
					new List<Item>
					{
						new Item(218, 5, 1),
						new Item(351, 12, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 3, 1),
					},
					new List<Item>
					{
						new Item(218, 4, 1),
						new Item(351, 12, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 3, 1),
					},
					new List<Item>
					{
						new Item(218, 2, 1),
						new Item(351, 12, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 3, 1),
					},
					new List<Item>
					{
						new Item(218, 1, 1),
						new Item(351, 12, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 3, 1),
					},
					new List<Item>
					{
						new Item(218, 0, 1),
						new Item(351, 12, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 3, 1),
					},
					new List<Item>
					{
						new Item(218, 14, 1),
						new Item(351, 12, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 3, 1),
					},
					new List<Item>
					{
						new Item(218, 13, 1),
						new Item(351, 12, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 3, 1),
					},
					new List<Item>
					{
						new Item(218, 12, 1),
						new Item(351, 12, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 3, 1),
					},
					new List<Item>
					{
						new Item(218, 11, 1),
						new Item(351, 12, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 3, 1),
					},
					new List<Item>
					{
						new Item(218, 10, 1),
						new Item(351, 12, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 3, 1),
					},
					new List<Item>
					{
						new Item(218, 9, 1),
						new Item(351, 12, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 3, 1),
					},
					new List<Item>
					{
						new Item(218, 8, 1),
						new Item(351, 12, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 3, 1),
					},
					new List<Item>
					{
						new Item(218, 7, 1),
						new Item(351, 12, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 3, 1),
					},
					new List<Item>
					{
						new Item(218, 6, 1),
						new Item(351, 12, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 2, 1),
					},
					new List<Item>
					{
						new Item(218, 15, 1),
						new Item(351, 13, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 2, 1),
					},
					new List<Item>
					{
						new Item(218, 5, 1),
						new Item(351, 13, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 2, 1),
					},
					new List<Item>
					{
						new Item(218, 4, 1),
						new Item(351, 13, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 2, 1),
					},
					new List<Item>
					{
						new Item(218, 3, 1),
						new Item(351, 13, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 2, 1),
					},
					new List<Item>
					{
						new Item(218, 1, 1),
						new Item(351, 13, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 2, 1),
					},
					new List<Item>
					{
						new Item(218, 0, 1),
						new Item(351, 13, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 2, 1),
					},
					new List<Item>
					{
						new Item(218, 14, 1),
						new Item(351, 13, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 2, 1),
					},
					new List<Item>
					{
						new Item(218, 13, 1),
						new Item(351, 13, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 2, 1),
					},
					new List<Item>
					{
						new Item(218, 12, 1),
						new Item(351, 13, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 2, 1),
					},
					new List<Item>
					{
						new Item(218, 11, 1),
						new Item(351, 13, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 2, 1),
					},
					new List<Item>
					{
						new Item(218, 10, 1),
						new Item(351, 13, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 2, 1),
					},
					new List<Item>
					{
						new Item(218, 9, 1),
						new Item(351, 13, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 2, 1),
					},
					new List<Item>
					{
						new Item(218, 8, 1),
						new Item(351, 13, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 2, 1),
					},
					new List<Item>
					{
						new Item(218, 7, 1),
						new Item(351, 13, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 2, 1),
					},
					new List<Item>
					{
						new Item(218, 6, 1),
						new Item(351, 13, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 1, 1),
					},
					new List<Item>
					{
						new Item(218, 15, 1),
						new Item(351, 14, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 1, 1),
					},
					new List<Item>
					{
						new Item(218, 5, 1),
						new Item(351, 14, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 1, 1),
					},
					new List<Item>
					{
						new Item(218, 4, 1),
						new Item(351, 14, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 1, 1),
					},
					new List<Item>
					{
						new Item(218, 3, 1),
						new Item(351, 14, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 1, 1),
					},
					new List<Item>
					{
						new Item(218, 2, 1),
						new Item(351, 14, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 1, 1),
					},
					new List<Item>
					{
						new Item(218, 0, 1),
						new Item(351, 14, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 1, 1),
					},
					new List<Item>
					{
						new Item(218, 14, 1),
						new Item(351, 14, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 1, 1),
					},
					new List<Item>
					{
						new Item(218, 13, 1),
						new Item(351, 14, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 1, 1),
					},
					new List<Item>
					{
						new Item(218, 12, 1),
						new Item(351, 14, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 1, 1),
					},
					new List<Item>
					{
						new Item(218, 11, 1),
						new Item(351, 14, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 1, 1),
					},
					new List<Item>
					{
						new Item(218, 10, 1),
						new Item(351, 14, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 1, 1),
					},
					new List<Item>
					{
						new Item(218, 9, 1),
						new Item(351, 14, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 1, 1),
					},
					new List<Item>
					{
						new Item(218, 8, 1),
						new Item(351, 14, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 1, 1),
					},
					new List<Item>
					{
						new Item(218, 7, 1),
						new Item(351, 14, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 1, 1),
					},
					new List<Item>
					{
						new Item(218, 6, 1),
						new Item(351, 14, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1),
					},
					new List<Item>
					{
						new Item(218, 15, 1),
						new Item(351, 15, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1),
					},
					new List<Item>
					{
						new Item(218, 5, 1),
						new Item(351, 15, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1),
					},
					new List<Item>
					{
						new Item(218, 4, 1),
						new Item(351, 15, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1),
					},
					new List<Item>
					{
						new Item(218, 3, 1),
						new Item(351, 15, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1),
					},
					new List<Item>
					{
						new Item(218, 2, 1),
						new Item(351, 15, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1),
					},
					new List<Item>
					{
						new Item(218, 1, 1),
						new Item(351, 15, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1),
					},
					new List<Item>
					{
						new Item(218, 14, 1),
						new Item(351, 15, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1),
					},
					new List<Item>
					{
						new Item(218, 13, 1),
						new Item(351, 15, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1),
					},
					new List<Item>
					{
						new Item(218, 12, 1),
						new Item(351, 15, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1),
					},
					new List<Item>
					{
						new Item(218, 11, 1),
						new Item(351, 15, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1),
					},
					new List<Item>
					{
						new Item(218, 10, 1),
						new Item(351, 15, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1),
					},
					new List<Item>
					{
						new Item(218, 9, 1),
						new Item(351, 15, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1),
					},
					new List<Item>
					{
						new Item(218, 8, 1),
						new Item(351, 15, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1),
					},
					new List<Item>
					{
						new Item(218, 7, 1),
						new Item(351, 15, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1),
					},
					new List<Item>
					{
						new Item(218, 6, 1),
						new Item(351, 15, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 15, 1),
					},
					new List<Item>
					{
						new Item(218, 5, 1),
						new Item(351, 16, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 15, 1),
					},
					new List<Item>
					{
						new Item(218, 4, 1),
						new Item(351, 16, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 15, 1),
					},
					new List<Item>
					{
						new Item(218, 3, 1),
						new Item(351, 16, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 15, 1),
					},
					new List<Item>
					{
						new Item(218, 2, 1),
						new Item(351, 16, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 15, 1),
					},
					new List<Item>
					{
						new Item(218, 1, 1),
						new Item(351, 16, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 15, 1),
					},
					new List<Item>
					{
						new Item(218, 0, 1),
						new Item(351, 16, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 15, 1),
					},
					new List<Item>
					{
						new Item(218, 14, 1),
						new Item(351, 16, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 15, 1),
					},
					new List<Item>
					{
						new Item(218, 13, 1),
						new Item(351, 16, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 15, 1),
					},
					new List<Item>
					{
						new Item(218, 12, 1),
						new Item(351, 16, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 15, 1),
					},
					new List<Item>
					{
						new Item(218, 11, 1),
						new Item(351, 16, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 15, 1),
					},
					new List<Item>
					{
						new Item(218, 10, 1),
						new Item(351, 16, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 15, 1),
					},
					new List<Item>
					{
						new Item(218, 9, 1),
						new Item(351, 16, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 15, 1),
					},
					new List<Item>
					{
						new Item(218, 8, 1),
						new Item(351, 16, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 15, 1),
					},
					new List<Item>
					{
						new Item(218, 7, 1),
						new Item(351, 16, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 15, 1),
					},
					new List<Item>
					{
						new Item(218, 6, 1),
						new Item(351, 16, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 12, 1),
					},
					new List<Item>
					{
						new Item(218, 15, 1),
						new Item(351, 17, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 12, 1),
					},
					new List<Item>
					{
						new Item(218, 5, 1),
						new Item(351, 17, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 12, 1),
					},
					new List<Item>
					{
						new Item(218, 4, 1),
						new Item(351, 17, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 12, 1),
					},
					new List<Item>
					{
						new Item(218, 3, 1),
						new Item(351, 17, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 12, 1),
					},
					new List<Item>
					{
						new Item(218, 2, 1),
						new Item(351, 17, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 12, 1),
					},
					new List<Item>
					{
						new Item(218, 1, 1),
						new Item(351, 17, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 12, 1),
					},
					new List<Item>
					{
						new Item(218, 0, 1),
						new Item(351, 17, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 12, 1),
					},
					new List<Item>
					{
						new Item(218, 14, 1),
						new Item(351, 17, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 12, 1),
					},
					new List<Item>
					{
						new Item(218, 13, 1),
						new Item(351, 17, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 12, 1),
					},
					new List<Item>
					{
						new Item(218, 11, 1),
						new Item(351, 17, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 12, 1),
					},
					new List<Item>
					{
						new Item(218, 10, 1),
						new Item(351, 17, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 12, 1),
					},
					new List<Item>
					{
						new Item(218, 9, 1),
						new Item(351, 17, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 12, 1),
					},
					new List<Item>
					{
						new Item(218, 8, 1),
						new Item(351, 17, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 12, 1),
					},
					new List<Item>
					{
						new Item(218, 7, 1),
						new Item(351, 17, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 12, 1),
					},
					new List<Item>
					{
						new Item(218, 6, 1),
						new Item(351, 17, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 11, 1),
					},
					new List<Item>
					{
						new Item(218, 15, 1),
						new Item(351, 18, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 11, 1),
					},
					new List<Item>
					{
						new Item(218, 5, 1),
						new Item(351, 18, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 11, 1),
					},
					new List<Item>
					{
						new Item(218, 4, 1),
						new Item(351, 18, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 11, 1),
					},
					new List<Item>
					{
						new Item(218, 3, 1),
						new Item(351, 18, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 11, 1),
					},
					new List<Item>
					{
						new Item(218, 2, 1),
						new Item(351, 18, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 11, 1),
					},
					new List<Item>
					{
						new Item(218, 1, 1),
						new Item(351, 18, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 11, 1),
					},
					new List<Item>
					{
						new Item(218, 0, 1),
						new Item(351, 18, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 11, 1),
					},
					new List<Item>
					{
						new Item(218, 14, 1),
						new Item(351, 18, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 11, 1),
					},
					new List<Item>
					{
						new Item(218, 13, 1),
						new Item(351, 18, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 11, 1),
					},
					new List<Item>
					{
						new Item(218, 12, 1),
						new Item(351, 18, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 11, 1),
					},
					new List<Item>
					{
						new Item(218, 10, 1),
						new Item(351, 18, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 11, 1),
					},
					new List<Item>
					{
						new Item(218, 9, 1),
						new Item(351, 18, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 11, 1),
					},
					new List<Item>
					{
						new Item(218, 8, 1),
						new Item(351, 18, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 11, 1),
					},
					new List<Item>
					{
						new Item(218, 7, 1),
						new Item(351, 18, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 11, 1),
					},
					new List<Item>
					{
						new Item(218, 6, 1),
						new Item(351, 18, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1),
					},
					new List<Item>
					{
						new Item(218, 15, 1),
						new Item(351, 19, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1),
					},
					new List<Item>
					{
						new Item(218, 5, 1),
						new Item(351, 19, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1),
					},
					new List<Item>
					{
						new Item(218, 4, 1),
						new Item(351, 19, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1),
					},
					new List<Item>
					{
						new Item(218, 3, 1),
						new Item(351, 19, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1),
					},
					new List<Item>
					{
						new Item(218, 2, 1),
						new Item(351, 19, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1),
					},
					new List<Item>
					{
						new Item(218, 1, 1),
						new Item(351, 19, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1),
					},
					new List<Item>
					{
						new Item(218, 14, 1),
						new Item(351, 19, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1),
					},
					new List<Item>
					{
						new Item(218, 13, 1),
						new Item(351, 19, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1),
					},
					new List<Item>
					{
						new Item(218, 12, 1),
						new Item(351, 19, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1),
					},
					new List<Item>
					{
						new Item(218, 11, 1),
						new Item(351, 19, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1),
					},
					new List<Item>
					{
						new Item(218, 10, 1),
						new Item(351, 19, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1),
					},
					new List<Item>
					{
						new Item(218, 9, 1),
						new Item(351, 19, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1),
					},
					new List<Item>
					{
						new Item(218, 8, 1),
						new Item(351, 19, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1),
					},
					new List<Item>
					{
						new Item(218, 7, 1),
						new Item(351, 19, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1),
					},
					new List<Item>
					{
						new Item(218, 6, 1),
						new Item(351, 19, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 14, 1),
					},
					new List<Item>
					{
						new Item(218, 15, 1),
						new Item(351, 1, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 14, 1),
					},
					new List<Item>
					{
						new Item(218, 5, 1),
						new Item(351, 1, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 14, 1),
					},
					new List<Item>
					{
						new Item(218, 4, 1),
						new Item(351, 1, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 14, 1),
					},
					new List<Item>
					{
						new Item(218, 3, 1),
						new Item(351, 1, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 14, 1),
					},
					new List<Item>
					{
						new Item(218, 2, 1),
						new Item(351, 1, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 14, 1),
					},
					new List<Item>
					{
						new Item(218, 1, 1),
						new Item(351, 1, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 14, 1),
					},
					new List<Item>
					{
						new Item(218, 0, 1),
						new Item(351, 1, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 14, 1),
					},
					new List<Item>
					{
						new Item(218, 13, 1),
						new Item(351, 1, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 14, 1),
					},
					new List<Item>
					{
						new Item(218, 12, 1),
						new Item(351, 1, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 14, 1),
					},
					new List<Item>
					{
						new Item(218, 11, 1),
						new Item(351, 1, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 14, 1),
					},
					new List<Item>
					{
						new Item(218, 10, 1),
						new Item(351, 1, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 14, 1),
					},
					new List<Item>
					{
						new Item(218, 9, 1),
						new Item(351, 1, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 14, 1),
					},
					new List<Item>
					{
						new Item(218, 8, 1),
						new Item(351, 1, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 14, 1),
					},
					new List<Item>
					{
						new Item(218, 7, 1),
						new Item(351, 1, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 14, 1),
					},
					new List<Item>
					{
						new Item(218, 6, 1),
						new Item(351, 1, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 13, 1),
					},
					new List<Item>
					{
						new Item(218, 15, 1),
						new Item(351, 2, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 13, 1),
					},
					new List<Item>
					{
						new Item(218, 5, 1),
						new Item(351, 2, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 13, 1),
					},
					new List<Item>
					{
						new Item(218, 4, 1),
						new Item(351, 2, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 13, 1),
					},
					new List<Item>
					{
						new Item(218, 3, 1),
						new Item(351, 2, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 13, 1),
					},
					new List<Item>
					{
						new Item(218, 2, 1),
						new Item(351, 2, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 13, 1),
					},
					new List<Item>
					{
						new Item(218, 1, 1),
						new Item(351, 2, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 13, 1),
					},
					new List<Item>
					{
						new Item(218, 0, 1),
						new Item(351, 2, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 13, 1),
					},
					new List<Item>
					{
						new Item(218, 14, 1),
						new Item(351, 2, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 13, 1),
					},
					new List<Item>
					{
						new Item(218, 12, 1),
						new Item(351, 2, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 13, 1),
					},
					new List<Item>
					{
						new Item(218, 11, 1),
						new Item(351, 2, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 13, 1),
					},
					new List<Item>
					{
						new Item(218, 10, 1),
						new Item(351, 2, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 13, 1),
					},
					new List<Item>
					{
						new Item(218, 9, 1),
						new Item(351, 2, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 13, 1),
					},
					new List<Item>
					{
						new Item(218, 8, 1),
						new Item(351, 2, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 13, 1),
					},
					new List<Item>
					{
						new Item(218, 7, 1),
						new Item(351, 2, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 13, 1),
					},
					new List<Item>
					{
						new Item(218, 6, 1),
						new Item(351, 2, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 12, 1),
					},
					new List<Item>
					{
						new Item(218, 15, 1),
						new Item(351, 3, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 12, 1),
					},
					new List<Item>
					{
						new Item(218, 5, 1),
						new Item(351, 3, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 12, 1),
					},
					new List<Item>
					{
						new Item(218, 4, 1),
						new Item(351, 3, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 12, 1),
					},
					new List<Item>
					{
						new Item(218, 3, 1),
						new Item(351, 3, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 12, 1),
					},
					new List<Item>
					{
						new Item(218, 2, 1),
						new Item(351, 3, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 12, 1),
					},
					new List<Item>
					{
						new Item(218, 1, 1),
						new Item(351, 3, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 12, 1),
					},
					new List<Item>
					{
						new Item(218, 0, 1),
						new Item(351, 3, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 12, 1),
					},
					new List<Item>
					{
						new Item(218, 14, 1),
						new Item(351, 3, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 12, 1),
					},
					new List<Item>
					{
						new Item(218, 13, 1),
						new Item(351, 3, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 12, 1),
					},
					new List<Item>
					{
						new Item(218, 11, 1),
						new Item(351, 3, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 12, 1),
					},
					new List<Item>
					{
						new Item(218, 10, 1),
						new Item(351, 3, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 12, 1),
					},
					new List<Item>
					{
						new Item(218, 9, 1),
						new Item(351, 3, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 12, 1),
					},
					new List<Item>
					{
						new Item(218, 8, 1),
						new Item(351, 3, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 12, 1),
					},
					new List<Item>
					{
						new Item(218, 7, 1),
						new Item(351, 3, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 12, 1),
					},
					new List<Item>
					{
						new Item(218, 6, 1),
						new Item(351, 3, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 11, 1),
					},
					new List<Item>
					{
						new Item(218, 15, 1),
						new Item(351, 4, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 11, 1),
					},
					new List<Item>
					{
						new Item(218, 5, 1),
						new Item(351, 4, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 11, 1),
					},
					new List<Item>
					{
						new Item(218, 4, 1),
						new Item(351, 4, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 11, 1),
					},
					new List<Item>
					{
						new Item(218, 3, 1),
						new Item(351, 4, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 11, 1),
					},
					new List<Item>
					{
						new Item(218, 2, 1),
						new Item(351, 4, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 11, 1),
					},
					new List<Item>
					{
						new Item(218, 1, 1),
						new Item(351, 4, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 11, 1),
					},
					new List<Item>
					{
						new Item(218, 0, 1),
						new Item(351, 4, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 11, 1),
					},
					new List<Item>
					{
						new Item(218, 14, 1),
						new Item(351, 4, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 11, 1),
					},
					new List<Item>
					{
						new Item(218, 13, 1),
						new Item(351, 4, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 11, 1),
					},
					new List<Item>
					{
						new Item(218, 12, 1),
						new Item(351, 4, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 11, 1),
					},
					new List<Item>
					{
						new Item(218, 10, 1),
						new Item(351, 4, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 11, 1),
					},
					new List<Item>
					{
						new Item(218, 9, 1),
						new Item(351, 4, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 11, 1),
					},
					new List<Item>
					{
						new Item(218, 8, 1),
						new Item(351, 4, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 11, 1),
					},
					new List<Item>
					{
						new Item(218, 7, 1),
						new Item(351, 4, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 11, 1),
					},
					new List<Item>
					{
						new Item(218, 6, 1),
						new Item(351, 4, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 10, 1),
					},
					new List<Item>
					{
						new Item(218, 15, 1),
						new Item(351, 5, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 10, 1),
					},
					new List<Item>
					{
						new Item(218, 5, 1),
						new Item(351, 5, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 10, 1),
					},
					new List<Item>
					{
						new Item(218, 4, 1),
						new Item(351, 5, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 10, 1),
					},
					new List<Item>
					{
						new Item(218, 3, 1),
						new Item(351, 5, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 10, 1),
					},
					new List<Item>
					{
						new Item(218, 2, 1),
						new Item(351, 5, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 10, 1),
					},
					new List<Item>
					{
						new Item(218, 1, 1),
						new Item(351, 5, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 10, 1),
					},
					new List<Item>
					{
						new Item(218, 0, 1),
						new Item(351, 5, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 10, 1),
					},
					new List<Item>
					{
						new Item(218, 14, 1),
						new Item(351, 5, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 10, 1),
					},
					new List<Item>
					{
						new Item(218, 13, 1),
						new Item(351, 5, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 10, 1),
					},
					new List<Item>
					{
						new Item(218, 12, 1),
						new Item(351, 5, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 10, 1),
					},
					new List<Item>
					{
						new Item(218, 11, 1),
						new Item(351, 5, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 10, 1),
					},
					new List<Item>
					{
						new Item(218, 9, 1),
						new Item(351, 5, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 10, 1),
					},
					new List<Item>
					{
						new Item(218, 8, 1),
						new Item(351, 5, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 10, 1),
					},
					new List<Item>
					{
						new Item(218, 7, 1),
						new Item(351, 5, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 10, 1),
					},
					new List<Item>
					{
						new Item(218, 6, 1),
						new Item(351, 5, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 9, 1),
					},
					new List<Item>
					{
						new Item(218, 15, 1),
						new Item(351, 6, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 9, 1),
					},
					new List<Item>
					{
						new Item(218, 5, 1),
						new Item(351, 6, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 9, 1),
					},
					new List<Item>
					{
						new Item(218, 4, 1),
						new Item(351, 6, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 9, 1),
					},
					new List<Item>
					{
						new Item(218, 3, 1),
						new Item(351, 6, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 9, 1),
					},
					new List<Item>
					{
						new Item(218, 2, 1),
						new Item(351, 6, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 9, 1),
					},
					new List<Item>
					{
						new Item(218, 1, 1),
						new Item(351, 6, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 9, 1),
					},
					new List<Item>
					{
						new Item(218, 0, 1),
						new Item(351, 6, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 9, 1),
					},
					new List<Item>
					{
						new Item(218, 14, 1),
						new Item(351, 6, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 9, 1),
					},
					new List<Item>
					{
						new Item(218, 13, 1),
						new Item(351, 6, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 9, 1),
					},
					new List<Item>
					{
						new Item(218, 12, 1),
						new Item(351, 6, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 9, 1),
					},
					new List<Item>
					{
						new Item(218, 11, 1),
						new Item(351, 6, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 9, 1),
					},
					new List<Item>
					{
						new Item(218, 10, 1),
						new Item(351, 6, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 9, 1),
					},
					new List<Item>
					{
						new Item(218, 8, 1),
						new Item(351, 6, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 9, 1),
					},
					new List<Item>
					{
						new Item(218, 7, 1),
						new Item(351, 6, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 9, 1),
					},
					new List<Item>
					{
						new Item(218, 6, 1),
						new Item(351, 6, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 8, 1),
					},
					new List<Item>
					{
						new Item(218, 15, 1),
						new Item(351, 7, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 8, 1),
					},
					new List<Item>
					{
						new Item(218, 5, 1),
						new Item(351, 7, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 8, 1),
					},
					new List<Item>
					{
						new Item(218, 4, 1),
						new Item(351, 7, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 8, 1),
					},
					new List<Item>
					{
						new Item(218, 3, 1),
						new Item(351, 7, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 8, 1),
					},
					new List<Item>
					{
						new Item(218, 2, 1),
						new Item(351, 7, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 8, 1),
					},
					new List<Item>
					{
						new Item(218, 1, 1),
						new Item(351, 7, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 8, 1),
					},
					new List<Item>
					{
						new Item(218, 0, 1),
						new Item(351, 7, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 8, 1),
					},
					new List<Item>
					{
						new Item(218, 14, 1),
						new Item(351, 7, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 8, 1),
					},
					new List<Item>
					{
						new Item(218, 13, 1),
						new Item(351, 7, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 8, 1),
					},
					new List<Item>
					{
						new Item(218, 12, 1),
						new Item(351, 7, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 8, 1),
					},
					new List<Item>
					{
						new Item(218, 11, 1),
						new Item(351, 7, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 8, 1),
					},
					new List<Item>
					{
						new Item(218, 10, 1),
						new Item(351, 7, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 8, 1),
					},
					new List<Item>
					{
						new Item(218, 9, 1),
						new Item(351, 7, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 8, 1),
					},
					new List<Item>
					{
						new Item(218, 7, 1),
						new Item(351, 7, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 8, 1),
					},
					new List<Item>
					{
						new Item(218, 6, 1),
						new Item(351, 7, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 7, 1),
					},
					new List<Item>
					{
						new Item(218, 15, 1),
						new Item(351, 8, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 7, 1),
					},
					new List<Item>
					{
						new Item(218, 5, 1),
						new Item(351, 8, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 7, 1),
					},
					new List<Item>
					{
						new Item(218, 4, 1),
						new Item(351, 8, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 7, 1),
					},
					new List<Item>
					{
						new Item(218, 3, 1),
						new Item(351, 8, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 7, 1),
					},
					new List<Item>
					{
						new Item(218, 2, 1),
						new Item(351, 8, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 7, 1),
					},
					new List<Item>
					{
						new Item(218, 1, 1),
						new Item(351, 8, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 7, 1),
					},
					new List<Item>
					{
						new Item(218, 0, 1),
						new Item(351, 8, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 7, 1),
					},
					new List<Item>
					{
						new Item(218, 14, 1),
						new Item(351, 8, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 7, 1),
					},
					new List<Item>
					{
						new Item(218, 13, 1),
						new Item(351, 8, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 7, 1),
					},
					new List<Item>
					{
						new Item(218, 12, 1),
						new Item(351, 8, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 7, 1),
					},
					new List<Item>
					{
						new Item(218, 11, 1),
						new Item(351, 8, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 7, 1),
					},
					new List<Item>
					{
						new Item(218, 10, 1),
						new Item(351, 8, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 7, 1),
					},
					new List<Item>
					{
						new Item(218, 9, 1),
						new Item(351, 8, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 7, 1),
					},
					new List<Item>
					{
						new Item(218, 8, 1),
						new Item(351, 8, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 7, 1),
					},
					new List<Item>
					{
						new Item(218, 6, 1),
						new Item(351, 8, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 6, 1),
					},
					new List<Item>
					{
						new Item(218, 15, 1),
						new Item(351, 9, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 6, 1),
					},
					new List<Item>
					{
						new Item(218, 5, 1),
						new Item(351, 9, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 6, 1),
					},
					new List<Item>
					{
						new Item(218, 4, 1),
						new Item(351, 9, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 6, 1),
					},
					new List<Item>
					{
						new Item(218, 3, 1),
						new Item(351, 9, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 6, 1),
					},
					new List<Item>
					{
						new Item(218, 2, 1),
						new Item(351, 9, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 6, 1),
					},
					new List<Item>
					{
						new Item(218, 1, 1),
						new Item(351, 9, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 6, 1),
					},
					new List<Item>
					{
						new Item(218, 0, 1),
						new Item(351, 9, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 6, 1),
					},
					new List<Item>
					{
						new Item(218, 14, 1),
						new Item(351, 9, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 6, 1),
					},
					new List<Item>
					{
						new Item(218, 13, 1),
						new Item(351, 9, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 6, 1),
					},
					new List<Item>
					{
						new Item(218, 12, 1),
						new Item(351, 9, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 6, 1),
					},
					new List<Item>
					{
						new Item(218, 11, 1),
						new Item(351, 9, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 6, 1),
					},
					new List<Item>
					{
						new Item(218, 10, 1),
						new Item(351, 9, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 6, 1),
					},
					new List<Item>
					{
						new Item(218, 9, 1),
						new Item(351, 9, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 6, 1),
					},
					new List<Item>
					{
						new Item(218, 8, 1),
						new Item(351, 9, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 6, 1),
					},
					new List<Item>
					{
						new Item(218, 7, 1),
						new Item(351, 9, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 15, 1),
					},
					new List<Item>
					{
						new Item(205, 0, 1),
						new Item(351, 0, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 5, 1),
					},
					new List<Item>
					{
						new Item(205, 0, 1),
						new Item(351, 10, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 4, 1),
					},
					new List<Item>
					{
						new Item(205, 0, 1),
						new Item(351, 11, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 3, 1),
					},
					new List<Item>
					{
						new Item(205, 0, 1),
						new Item(351, 12, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 2, 1),
					},
					new List<Item>
					{
						new Item(205, 0, 1),
						new Item(351, 13, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 1, 1),
					},
					new List<Item>
					{
						new Item(205, 0, 1),
						new Item(351, 14, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1),
					},
					new List<Item>
					{
						new Item(205, 0, 1),
						new Item(351, 15, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 15, 1),
					},
					new List<Item>
					{
						new Item(205, 0, 1),
						new Item(351, 16, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 12, 1),
					},
					new List<Item>
					{
						new Item(205, 0, 1),
						new Item(351, 17, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 11, 1),
					},
					new List<Item>
					{
						new Item(205, 0, 1),
						new Item(351, 18, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1),
					},
					new List<Item>
					{
						new Item(205, 0, 1),
						new Item(351, 19, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 14, 1),
					},
					new List<Item>
					{
						new Item(205, 0, 1),
						new Item(351, 1, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 13, 1),
					},
					new List<Item>
					{
						new Item(205, 0, 1),
						new Item(351, 2, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 12, 1),
					},
					new List<Item>
					{
						new Item(205, 0, 1),
						new Item(351, 3, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 11, 1),
					},
					new List<Item>
					{
						new Item(205, 0, 1),
						new Item(351, 4, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 10, 1),
					},
					new List<Item>
					{
						new Item(205, 0, 1),
						new Item(351, 5, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 9, 1),
					},
					new List<Item>
					{
						new Item(205, 0, 1),
						new Item(351, 6, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 8, 1),
					},
					new List<Item>
					{
						new Item(205, 0, 1),
						new Item(351, 7, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 7, 1),
					},
					new List<Item>
					{
						new Item(205, 0, 1),
						new Item(351, 8, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 6, 1),
					},
					new List<Item>
					{
						new Item(205, 0, 1),
						new Item(351, 9, 1),
					}, "crafting_table"),
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(-162, 0),
					},
					new Item[]
					{
						new Item(206, 0),
						new Item(206, 0),
						new Item(206, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(134, 0),
					},
					new Item[]
					{
						new Item(5, 1),
						new Item(5, 1),
						new Item(5, 1),
						new Item(0, 0),
						new Item(5, 1),
						new Item(5, 1),
						new Item(0, 0),
						new Item(0, 0),
						new Item(5, 1),
					}, "crafting_table"),
				new ShapedRecipe(1, 2,
					new List<Item>
					{
						new Item(280, 0),
					},
					new Item[]
					{
						new Item(5, 32767),
						new Item(5, 32767),
					}, "crafting_table"),
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(182, 0),
					},
					new Item[]
					{
						new Item(179, 0),
						new Item(179, 0),
						new Item(179, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(182, 4),
					},
					new Item[]
					{
						new Item(168, 2),
						new Item(168, 2),
						new Item(168, 2),
					}, "crafting_table"),
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(182, 3),
					},
					new Item[]
					{
						new Item(168, 1),
						new Item(168, 1),
						new Item(168, 1),
					}, "crafting_table"),
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(182, 1),
					},
					new Item[]
					{
						new Item(201, 0),
						new Item(201, 0),
						new Item(201, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(182, 5),
					},
					new Item[]
					{
						new Item(48, 0),
						new Item(48, 0),
						new Item(48, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(182, 7),
					},
					new Item[]
					{
						new Item(215, 0),
						new Item(215, 0),
						new Item(215, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(182, 0),
					},
					new Item[]
					{
						new Item(179, 1),
						new Item(179, 1),
						new Item(179, 1),
					}, "crafting_table"),
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(182, 6),
					},
					new Item[]
					{
						new Item(24, 3),
						new Item(24, 3),
						new Item(24, 3),
					}, "crafting_table"),
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(-162, 3),
					},
					new Item[]
					{
						new Item(1, 5),
						new Item(1, 5),
						new Item(1, 5),
					}, "crafting_table"),
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(-162, 4),
					},
					new Item[]
					{
						new Item(1, 3),
						new Item(1, 3),
						new Item(1, 3),
					}, "crafting_table"),
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(-162, 6),
					},
					new Item[]
					{
						new Item(1, 1),
						new Item(1, 1),
						new Item(1, 1),
					}, "crafting_table"),
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(-162, 7),
					},
					new Item[]
					{
						new Item(1, 2),
						new Item(1, 2),
						new Item(1, 2),
					}, "crafting_table"),
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(-162, 2),
					},
					new Item[]
					{
						new Item(1, 6),
						new Item(1, 6),
						new Item(1, 6),
					}, "crafting_table"),
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(-162, 5),
					},
					new Item[]
					{
						new Item(1, 4),
						new Item(1, 4),
						new Item(1, 4),
					}, "crafting_table"),
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(-162, 1),
					},
					new Item[]
					{
						new Item(179, 3),
						new Item(179, 3),
						new Item(179, 3),
					}, "crafting_table"),
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(-166, 4),
					},
					new Item[]
					{
						new Item(179, 2),
						new Item(179, 2),
						new Item(179, 2),
					}, "crafting_table"),
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(-166, 3),
					},
					new Item[]
					{
						new Item(24, 2),
						new Item(24, 2),
						new Item(24, 2),
					}, "crafting_table"),
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(-166, 1),
					},
					new Item[]
					{
						new Item(155, 3),
						new Item(155, 3),
						new Item(155, 3),
					}, "crafting_table"),
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(44, 6),
					},
					new Item[]
					{
						new Item(155, 0),
						new Item(155, 0),
						new Item(155, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(182, 2),
					},
					new Item[]
					{
						new Item(168, 0),
						new Item(168, 0),
						new Item(168, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(44, 1),
					},
					new Item[]
					{
						new Item(24, 1),
						new Item(24, 1),
						new Item(24, 1),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(270, 0),
					},
					new Item[]
					{
						new Item(5, 32767),
						new Item(0, 0),
						new Item(0, 0),
						new Item(5, 32767),
						new Item(280, 32767),
						new Item(280, 32767),
						new Item(5, 32767),
						new Item(0, 0),
						new Item(0, 0),
					}, "crafting_table"),
				new ShapedRecipe(1, 3,
					new List<Item>
					{
						new Item(269, 0),
					},
					new Item[]
					{
						new Item(5, 32767),
						new Item(280, 32767),
						new Item(280, 32767),
					}, "crafting_table"),
				new ShapedRecipe(2, 3,
					new List<Item>
					{
						new Item(271, 0),
					},
					new Item[]
					{
						new Item(5, 32767),
						new Item(280, 32767),
						new Item(5, 32767),
						new Item(0, 0),
						new Item(5, 32767),
						new Item(280, 32767),
					}, "crafting_table"),
				new ShapedRecipe(2, 3,
					new List<Item>
					{
						new Item(290, 0),
					},
					new Item[]
					{
						new Item(5, 32767),
						new Item(280, 32767),
						new Item(5, 32767),
						new Item(0, 0),
						new Item(0, 0),
						new Item(280, 32767),
					}, "crafting_table"),
				new MultiRecipe() { Id = new UUID("aecd2294-4b94-434b-8667-4499bb2c9327") }, // aecd2294-4b94-434b-8667-4499bb2c9327
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(262, 11),
					},
					new Item[]
					{
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(441, 10),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(262, 12),
					},
					new Item[]
					{
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(441, 11),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(262, 13),
					},
					new Item[]
					{
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(441, 12),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(262, 14),
					},
					new Item[]
					{
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(441, 13),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(262, 15),
					},
					new Item[]
					{
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(441, 14),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(262, 16),
					},
					new Item[]
					{
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(441, 15),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(262, 17),
					},
					new Item[]
					{
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(441, 16),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(262, 18),
					},
					new Item[]
					{
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(441, 17),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(262, 19),
					},
					new Item[]
					{
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(441, 18),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(262, 20),
					},
					new Item[]
					{
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(441, 19),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(262, 21),
					},
					new Item[]
					{
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(441, 20),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(262, 22),
					},
					new Item[]
					{
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(441, 21),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(262, 23),
					},
					new Item[]
					{
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(441, 22),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(262, 24),
					},
					new Item[]
					{
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(441, 23),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(262, 25),
					},
					new Item[]
					{
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(441, 24),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(262, 26),
					},
					new Item[]
					{
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(441, 25),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(262, 27),
					},
					new Item[]
					{
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(441, 26),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(262, 28),
					},
					new Item[]
					{
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(441, 27),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(262, 29),
					},
					new Item[]
					{
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(441, 28),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(262, 30),
					},
					new Item[]
					{
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(441, 29),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(262, 31),
					},
					new Item[]
					{
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(441, 30),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(262, 32),
					},
					new Item[]
					{
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(441, 31),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(262, 33),
					},
					new Item[]
					{
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(441, 32),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(262, 34),
					},
					new Item[]
					{
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(441, 33),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(262, 35),
					},
					new Item[]
					{
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(441, 34),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(262, 36),
					},
					new Item[]
					{
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(441, 35),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(262, 37),
					},
					new Item[]
					{
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(441, 36),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(262, 38),
					},
					new Item[]
					{
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(441, 37),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(262, 39),
					},
					new Item[]
					{
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(441, 38),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(262, 40),
					},
					new Item[]
					{
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(441, 39),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(262, 41),
					},
					new Item[]
					{
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(441, 40),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(262, 42),
					},
					new Item[]
					{
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(441, 41),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(262, 6),
					},
					new Item[]
					{
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(441, 5),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(262, 7),
					},
					new Item[]
					{
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(441, 6),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(262, 8),
					},
					new Item[]
					{
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(441, 7),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(262, 9),
					},
					new Item[]
					{
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(441, 8),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
					}, "crafting_table"),
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(262, 10),
					},
					new Item[]
					{
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(441, 9),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
					}, "crafting_table"),
				new ShapedRecipe(1, 3,
					new List<Item>
					{
						new Item(268, 0),
					},
					new Item[]
					{
						new Item(5, 32767),
						new Item(5, 32767),
						new Item(280, 32767),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 15, 1),
					},
					new List<Item>
					{
						new Item(351, 0, 1),
						new Item(35, 14, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 15, 1),
					},
					new List<Item>
					{
						new Item(351, 0, 1),
						new Item(35, 5, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 15, 1),
					},
					new List<Item>
					{
						new Item(351, 0, 1),
						new Item(35, 4, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 15, 1),
					},
					new List<Item>
					{
						new Item(351, 0, 1),
						new Item(35, 3, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 15, 1),
					},
					new List<Item>
					{
						new Item(351, 0, 1),
						new Item(35, 2, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 15, 1),
					},
					new List<Item>
					{
						new Item(351, 0, 1),
						new Item(35, 1, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 15, 1),
					},
					new List<Item>
					{
						new Item(351, 0, 1),
						new Item(35, 0, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 15, 1),
					},
					new List<Item>
					{
						new Item(351, 0, 1),
						new Item(35, 13, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 15, 1),
					},
					new List<Item>
					{
						new Item(351, 0, 1),
						new Item(35, 12, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 15, 1),
					},
					new List<Item>
					{
						new Item(351, 0, 1),
						new Item(35, 11, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 15, 1),
					},
					new List<Item>
					{
						new Item(351, 0, 1),
						new Item(35, 10, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 15, 1),
					},
					new List<Item>
					{
						new Item(351, 0, 1),
						new Item(35, 9, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 15, 1),
					},
					new List<Item>
					{
						new Item(351, 0, 1),
						new Item(35, 8, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 15, 1),
					},
					new List<Item>
					{
						new Item(351, 0, 1),
						new Item(35, 7, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 15, 1),
					},
					new List<Item>
					{
						new Item(351, 0, 1),
						new Item(35, 6, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 5, 1),
					},
					new List<Item>
					{
						new Item(351, 10, 1),
						new Item(35, 15, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 5, 1),
					},
					new List<Item>
					{
						new Item(351, 10, 1),
						new Item(35, 14, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 5, 1),
					},
					new List<Item>
					{
						new Item(351, 10, 1),
						new Item(35, 4, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 5, 1),
					},
					new List<Item>
					{
						new Item(351, 10, 1),
						new Item(35, 3, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 5, 1),
					},
					new List<Item>
					{
						new Item(351, 10, 1),
						new Item(35, 2, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 5, 1),
					},
					new List<Item>
					{
						new Item(351, 10, 1),
						new Item(35, 1, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 5, 1),
					},
					new List<Item>
					{
						new Item(351, 10, 1),
						new Item(35, 0, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 5, 1),
					},
					new List<Item>
					{
						new Item(351, 10, 1),
						new Item(35, 13, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 5, 1),
					},
					new List<Item>
					{
						new Item(351, 10, 1),
						new Item(35, 12, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 5, 1),
					},
					new List<Item>
					{
						new Item(351, 10, 1),
						new Item(35, 11, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 5, 1),
					},
					new List<Item>
					{
						new Item(351, 10, 1),
						new Item(35, 10, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 5, 1),
					},
					new List<Item>
					{
						new Item(351, 10, 1),
						new Item(35, 9, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 5, 1),
					},
					new List<Item>
					{
						new Item(351, 10, 1),
						new Item(35, 8, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 5, 1),
					},
					new List<Item>
					{
						new Item(351, 10, 1),
						new Item(35, 7, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 5, 1),
					},
					new List<Item>
					{
						new Item(351, 10, 1),
						new Item(35, 6, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 4, 1),
					},
					new List<Item>
					{
						new Item(351, 11, 1),
						new Item(35, 15, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 4, 1),
					},
					new List<Item>
					{
						new Item(351, 11, 1),
						new Item(35, 14, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 4, 1),
					},
					new List<Item>
					{
						new Item(351, 11, 1),
						new Item(35, 5, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 4, 1),
					},
					new List<Item>
					{
						new Item(351, 11, 1),
						new Item(35, 3, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 4, 1),
					},
					new List<Item>
					{
						new Item(351, 11, 1),
						new Item(35, 2, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 4, 1),
					},
					new List<Item>
					{
						new Item(351, 11, 1),
						new Item(35, 1, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 4, 1),
					},
					new List<Item>
					{
						new Item(351, 11, 1),
						new Item(35, 0, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 4, 1),
					},
					new List<Item>
					{
						new Item(351, 11, 1),
						new Item(35, 13, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 4, 1),
					},
					new List<Item>
					{
						new Item(351, 11, 1),
						new Item(35, 12, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 4, 1),
					},
					new List<Item>
					{
						new Item(351, 11, 1),
						new Item(35, 11, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 4, 1),
					},
					new List<Item>
					{
						new Item(351, 11, 1),
						new Item(35, 10, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 4, 1),
					},
					new List<Item>
					{
						new Item(351, 11, 1),
						new Item(35, 9, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 4, 1),
					},
					new List<Item>
					{
						new Item(351, 11, 1),
						new Item(35, 8, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 4, 1),
					},
					new List<Item>
					{
						new Item(351, 11, 1),
						new Item(35, 7, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 4, 1),
					},
					new List<Item>
					{
						new Item(351, 11, 1),
						new Item(35, 6, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 3, 1),
					},
					new List<Item>
					{
						new Item(351, 12, 1),
						new Item(35, 15, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 3, 1),
					},
					new List<Item>
					{
						new Item(351, 12, 1),
						new Item(35, 14, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 3, 1),
					},
					new List<Item>
					{
						new Item(351, 12, 1),
						new Item(35, 5, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 3, 1),
					},
					new List<Item>
					{
						new Item(351, 12, 1),
						new Item(35, 4, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 3, 1),
					},
					new List<Item>
					{
						new Item(351, 12, 1),
						new Item(35, 2, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 3, 1),
					},
					new List<Item>
					{
						new Item(351, 12, 1),
						new Item(35, 1, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 3, 1),
					},
					new List<Item>
					{
						new Item(351, 12, 1),
						new Item(35, 0, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 3, 1),
					},
					new List<Item>
					{
						new Item(351, 12, 1),
						new Item(35, 13, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 3, 1),
					},
					new List<Item>
					{
						new Item(351, 12, 1),
						new Item(35, 12, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 3, 1),
					},
					new List<Item>
					{
						new Item(351, 12, 1),
						new Item(35, 11, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 3, 1),
					},
					new List<Item>
					{
						new Item(351, 12, 1),
						new Item(35, 10, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 3, 1),
					},
					new List<Item>
					{
						new Item(351, 12, 1),
						new Item(35, 9, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 3, 1),
					},
					new List<Item>
					{
						new Item(351, 12, 1),
						new Item(35, 8, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 3, 1),
					},
					new List<Item>
					{
						new Item(351, 12, 1),
						new Item(35, 7, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 3, 1),
					},
					new List<Item>
					{
						new Item(351, 12, 1),
						new Item(35, 6, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 2, 1),
					},
					new List<Item>
					{
						new Item(351, 13, 1),
						new Item(35, 15, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 2, 1),
					},
					new List<Item>
					{
						new Item(351, 13, 1),
						new Item(35, 14, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 2, 1),
					},
					new List<Item>
					{
						new Item(351, 13, 1),
						new Item(35, 5, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 2, 1),
					},
					new List<Item>
					{
						new Item(351, 13, 1),
						new Item(35, 4, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 2, 1),
					},
					new List<Item>
					{
						new Item(351, 13, 1),
						new Item(35, 3, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 2, 1),
					},
					new List<Item>
					{
						new Item(351, 13, 1),
						new Item(35, 1, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 2, 1),
					},
					new List<Item>
					{
						new Item(351, 13, 1),
						new Item(35, 0, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 2, 1),
					},
					new List<Item>
					{
						new Item(351, 13, 1),
						new Item(35, 13, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 2, 1),
					},
					new List<Item>
					{
						new Item(351, 13, 1),
						new Item(35, 12, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 2, 1),
					},
					new List<Item>
					{
						new Item(351, 13, 1),
						new Item(35, 11, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 2, 1),
					},
					new List<Item>
					{
						new Item(351, 13, 1),
						new Item(35, 10, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 2, 1),
					},
					new List<Item>
					{
						new Item(351, 13, 1),
						new Item(35, 9, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 2, 1),
					},
					new List<Item>
					{
						new Item(351, 13, 1),
						new Item(35, 8, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 2, 1),
					},
					new List<Item>
					{
						new Item(351, 13, 1),
						new Item(35, 7, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 2, 1),
					},
					new List<Item>
					{
						new Item(351, 13, 1),
						new Item(35, 6, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 1, 1),
					},
					new List<Item>
					{
						new Item(351, 14, 1),
						new Item(35, 15, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 1, 1),
					},
					new List<Item>
					{
						new Item(351, 14, 1),
						new Item(35, 14, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 1, 1),
					},
					new List<Item>
					{
						new Item(351, 14, 1),
						new Item(35, 5, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 1, 1),
					},
					new List<Item>
					{
						new Item(351, 14, 1),
						new Item(35, 4, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 1, 1),
					},
					new List<Item>
					{
						new Item(351, 14, 1),
						new Item(35, 3, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 1, 1),
					},
					new List<Item>
					{
						new Item(351, 14, 1),
						new Item(35, 2, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 1, 1),
					},
					new List<Item>
					{
						new Item(351, 14, 1),
						new Item(35, 0, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 1, 1),
					},
					new List<Item>
					{
						new Item(351, 14, 1),
						new Item(35, 13, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 1, 1),
					},
					new List<Item>
					{
						new Item(351, 14, 1),
						new Item(35, 12, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 1, 1),
					},
					new List<Item>
					{
						new Item(351, 14, 1),
						new Item(35, 11, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 1, 1),
					},
					new List<Item>
					{
						new Item(351, 14, 1),
						new Item(35, 10, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 1, 1),
					},
					new List<Item>
					{
						new Item(351, 14, 1),
						new Item(35, 9, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 1, 1),
					},
					new List<Item>
					{
						new Item(351, 14, 1),
						new Item(35, 8, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 1, 1),
					},
					new List<Item>
					{
						new Item(351, 14, 1),
						new Item(35, 7, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 1, 1),
					},
					new List<Item>
					{
						new Item(351, 14, 1),
						new Item(35, 6, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1),
					},
					new List<Item>
					{
						new Item(351, 15, 1),
						new Item(35, 15, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1),
					},
					new List<Item>
					{
						new Item(351, 15, 1),
						new Item(35, 14, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1),
					},
					new List<Item>
					{
						new Item(351, 15, 1),
						new Item(35, 5, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1),
					},
					new List<Item>
					{
						new Item(351, 15, 1),
						new Item(35, 4, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1),
					},
					new List<Item>
					{
						new Item(351, 15, 1),
						new Item(35, 3, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1),
					},
					new List<Item>
					{
						new Item(351, 15, 1),
						new Item(35, 2, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1),
					},
					new List<Item>
					{
						new Item(351, 15, 1),
						new Item(35, 1, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1),
					},
					new List<Item>
					{
						new Item(351, 15, 1),
						new Item(35, 13, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1),
					},
					new List<Item>
					{
						new Item(351, 15, 1),
						new Item(35, 12, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1),
					},
					new List<Item>
					{
						new Item(351, 15, 1),
						new Item(35, 11, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1),
					},
					new List<Item>
					{
						new Item(351, 15, 1),
						new Item(35, 10, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1),
					},
					new List<Item>
					{
						new Item(351, 15, 1),
						new Item(35, 9, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1),
					},
					new List<Item>
					{
						new Item(351, 15, 1),
						new Item(35, 8, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1),
					},
					new List<Item>
					{
						new Item(351, 15, 1),
						new Item(35, 7, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1),
					},
					new List<Item>
					{
						new Item(351, 15, 1),
						new Item(35, 6, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 15, 1),
					},
					new List<Item>
					{
						new Item(351, 16, 1),
						new Item(35, 14, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 15, 1),
					},
					new List<Item>
					{
						new Item(351, 16, 1),
						new Item(35, 5, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 15, 1),
					},
					new List<Item>
					{
						new Item(351, 16, 1),
						new Item(35, 4, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 15, 1),
					},
					new List<Item>
					{
						new Item(351, 16, 1),
						new Item(35, 3, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 15, 1),
					},
					new List<Item>
					{
						new Item(351, 16, 1),
						new Item(35, 2, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 15, 1),
					},
					new List<Item>
					{
						new Item(351, 16, 1),
						new Item(35, 1, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 15, 1),
					},
					new List<Item>
					{
						new Item(351, 16, 1),
						new Item(35, 0, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 15, 1),
					},
					new List<Item>
					{
						new Item(351, 16, 1),
						new Item(35, 13, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 15, 1),
					},
					new List<Item>
					{
						new Item(351, 16, 1),
						new Item(35, 12, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 15, 1),
					},
					new List<Item>
					{
						new Item(351, 16, 1),
						new Item(35, 11, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 15, 1),
					},
					new List<Item>
					{
						new Item(351, 16, 1),
						new Item(35, 10, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 15, 1),
					},
					new List<Item>
					{
						new Item(351, 16, 1),
						new Item(35, 9, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 15, 1),
					},
					new List<Item>
					{
						new Item(351, 16, 1),
						new Item(35, 8, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 15, 1),
					},
					new List<Item>
					{
						new Item(351, 16, 1),
						new Item(35, 7, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 15, 1),
					},
					new List<Item>
					{
						new Item(351, 16, 1),
						new Item(35, 6, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 12, 1),
					},
					new List<Item>
					{
						new Item(351, 17, 1),
						new Item(35, 15, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 12, 1),
					},
					new List<Item>
					{
						new Item(351, 17, 1),
						new Item(35, 14, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 12, 1),
					},
					new List<Item>
					{
						new Item(351, 17, 1),
						new Item(35, 5, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 12, 1),
					},
					new List<Item>
					{
						new Item(351, 17, 1),
						new Item(35, 4, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 12, 1),
					},
					new List<Item>
					{
						new Item(351, 17, 1),
						new Item(35, 3, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 12, 1),
					},
					new List<Item>
					{
						new Item(351, 17, 1),
						new Item(35, 2, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 12, 1),
					},
					new List<Item>
					{
						new Item(351, 17, 1),
						new Item(35, 1, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 12, 1),
					},
					new List<Item>
					{
						new Item(351, 17, 1),
						new Item(35, 0, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 12, 1),
					},
					new List<Item>
					{
						new Item(351, 17, 1),
						new Item(35, 13, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 12, 1),
					},
					new List<Item>
					{
						new Item(351, 17, 1),
						new Item(35, 11, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 12, 1),
					},
					new List<Item>
					{
						new Item(351, 17, 1),
						new Item(35, 10, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 12, 1),
					},
					new List<Item>
					{
						new Item(351, 17, 1),
						new Item(35, 9, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 12, 1),
					},
					new List<Item>
					{
						new Item(351, 17, 1),
						new Item(35, 8, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 12, 1),
					},
					new List<Item>
					{
						new Item(351, 17, 1),
						new Item(35, 7, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 12, 1),
					},
					new List<Item>
					{
						new Item(351, 17, 1),
						new Item(35, 6, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 11, 1),
					},
					new List<Item>
					{
						new Item(351, 18, 1),
						new Item(35, 15, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 11, 1),
					},
					new List<Item>
					{
						new Item(351, 18, 1),
						new Item(35, 14, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 11, 1),
					},
					new List<Item>
					{
						new Item(351, 18, 1),
						new Item(35, 5, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 11, 1),
					},
					new List<Item>
					{
						new Item(351, 18, 1),
						new Item(35, 4, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 11, 1),
					},
					new List<Item>
					{
						new Item(351, 18, 1),
						new Item(35, 3, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 11, 1),
					},
					new List<Item>
					{
						new Item(351, 18, 1),
						new Item(35, 2, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 11, 1),
					},
					new List<Item>
					{
						new Item(351, 18, 1),
						new Item(35, 1, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 11, 1),
					},
					new List<Item>
					{
						new Item(351, 18, 1),
						new Item(35, 0, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 11, 1),
					},
					new List<Item>
					{
						new Item(351, 18, 1),
						new Item(35, 13, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 11, 1),
					},
					new List<Item>
					{
						new Item(351, 18, 1),
						new Item(35, 12, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 11, 1),
					},
					new List<Item>
					{
						new Item(351, 18, 1),
						new Item(35, 10, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 11, 1),
					},
					new List<Item>
					{
						new Item(351, 18, 1),
						new Item(35, 9, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 11, 1),
					},
					new List<Item>
					{
						new Item(351, 18, 1),
						new Item(35, 8, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 11, 1),
					},
					new List<Item>
					{
						new Item(351, 18, 1),
						new Item(35, 7, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 11, 1),
					},
					new List<Item>
					{
						new Item(351, 18, 1),
						new Item(35, 6, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1),
					},
					new List<Item>
					{
						new Item(351, 19, 1),
						new Item(35, 15, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1),
					},
					new List<Item>
					{
						new Item(351, 19, 1),
						new Item(35, 14, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1),
					},
					new List<Item>
					{
						new Item(351, 19, 1),
						new Item(35, 5, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1),
					},
					new List<Item>
					{
						new Item(351, 19, 1),
						new Item(35, 4, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1),
					},
					new List<Item>
					{
						new Item(351, 19, 1),
						new Item(35, 3, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1),
					},
					new List<Item>
					{
						new Item(351, 19, 1),
						new Item(35, 2, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1),
					},
					new List<Item>
					{
						new Item(351, 19, 1),
						new Item(35, 1, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1),
					},
					new List<Item>
					{
						new Item(351, 19, 1),
						new Item(35, 13, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1),
					},
					new List<Item>
					{
						new Item(351, 19, 1),
						new Item(35, 12, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1),
					},
					new List<Item>
					{
						new Item(351, 19, 1),
						new Item(35, 11, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1),
					},
					new List<Item>
					{
						new Item(351, 19, 1),
						new Item(35, 10, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1),
					},
					new List<Item>
					{
						new Item(351, 19, 1),
						new Item(35, 9, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1),
					},
					new List<Item>
					{
						new Item(351, 19, 1),
						new Item(35, 8, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1),
					},
					new List<Item>
					{
						new Item(351, 19, 1),
						new Item(35, 7, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1),
					},
					new List<Item>
					{
						new Item(351, 19, 1),
						new Item(35, 6, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 14, 1),
					},
					new List<Item>
					{
						new Item(351, 1, 1),
						new Item(35, 15, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 14, 1),
					},
					new List<Item>
					{
						new Item(351, 1, 1),
						new Item(35, 5, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 14, 1),
					},
					new List<Item>
					{
						new Item(351, 1, 1),
						new Item(35, 4, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 14, 1),
					},
					new List<Item>
					{
						new Item(351, 1, 1),
						new Item(35, 3, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 14, 1),
					},
					new List<Item>
					{
						new Item(351, 1, 1),
						new Item(35, 2, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 14, 1),
					},
					new List<Item>
					{
						new Item(351, 1, 1),
						new Item(35, 1, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 14, 1),
					},
					new List<Item>
					{
						new Item(351, 1, 1),
						new Item(35, 0, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 14, 1),
					},
					new List<Item>
					{
						new Item(351, 1, 1),
						new Item(35, 13, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 14, 1),
					},
					new List<Item>
					{
						new Item(351, 1, 1),
						new Item(35, 12, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 14, 1),
					},
					new List<Item>
					{
						new Item(351, 1, 1),
						new Item(35, 11, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 14, 1),
					},
					new List<Item>
					{
						new Item(351, 1, 1),
						new Item(35, 10, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 14, 1),
					},
					new List<Item>
					{
						new Item(351, 1, 1),
						new Item(35, 9, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 14, 1),
					},
					new List<Item>
					{
						new Item(351, 1, 1),
						new Item(35, 8, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 14, 1),
					},
					new List<Item>
					{
						new Item(351, 1, 1),
						new Item(35, 7, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 14, 1),
					},
					new List<Item>
					{
						new Item(351, 1, 1),
						new Item(35, 6, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 13, 1),
					},
					new List<Item>
					{
						new Item(351, 2, 1),
						new Item(35, 15, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 13, 1),
					},
					new List<Item>
					{
						new Item(351, 2, 1),
						new Item(35, 14, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 13, 1),
					},
					new List<Item>
					{
						new Item(351, 2, 1),
						new Item(35, 5, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 13, 1),
					},
					new List<Item>
					{
						new Item(351, 2, 1),
						new Item(35, 4, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 13, 1),
					},
					new List<Item>
					{
						new Item(351, 2, 1),
						new Item(35, 3, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 13, 1),
					},
					new List<Item>
					{
						new Item(351, 2, 1),
						new Item(35, 2, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 13, 1),
					},
					new List<Item>
					{
						new Item(351, 2, 1),
						new Item(35, 1, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 13, 1),
					},
					new List<Item>
					{
						new Item(351, 2, 1),
						new Item(35, 0, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 13, 1),
					},
					new List<Item>
					{
						new Item(351, 2, 1),
						new Item(35, 12, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 13, 1),
					},
					new List<Item>
					{
						new Item(351, 2, 1),
						new Item(35, 11, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 13, 1),
					},
					new List<Item>
					{
						new Item(351, 2, 1),
						new Item(35, 10, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 13, 1),
					},
					new List<Item>
					{
						new Item(351, 2, 1),
						new Item(35, 9, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 13, 1),
					},
					new List<Item>
					{
						new Item(351, 2, 1),
						new Item(35, 8, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 13, 1),
					},
					new List<Item>
					{
						new Item(351, 2, 1),
						new Item(35, 7, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 13, 1),
					},
					new List<Item>
					{
						new Item(351, 2, 1),
						new Item(35, 6, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 12, 1),
					},
					new List<Item>
					{
						new Item(351, 3, 1),
						new Item(35, 15, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 12, 1),
					},
					new List<Item>
					{
						new Item(351, 3, 1),
						new Item(35, 14, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 12, 1),
					},
					new List<Item>
					{
						new Item(351, 3, 1),
						new Item(35, 5, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 12, 1),
					},
					new List<Item>
					{
						new Item(351, 3, 1),
						new Item(35, 4, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 12, 1),
					},
					new List<Item>
					{
						new Item(351, 3, 1),
						new Item(35, 3, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 12, 1),
					},
					new List<Item>
					{
						new Item(351, 3, 1),
						new Item(35, 2, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 12, 1),
					},
					new List<Item>
					{
						new Item(351, 3, 1),
						new Item(35, 1, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 12, 1),
					},
					new List<Item>
					{
						new Item(351, 3, 1),
						new Item(35, 0, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 12, 1),
					},
					new List<Item>
					{
						new Item(351, 3, 1),
						new Item(35, 13, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 12, 1),
					},
					new List<Item>
					{
						new Item(351, 3, 1),
						new Item(35, 11, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 12, 1),
					},
					new List<Item>
					{
						new Item(351, 3, 1),
						new Item(35, 10, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 12, 1),
					},
					new List<Item>
					{
						new Item(351, 3, 1),
						new Item(35, 9, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 12, 1),
					},
					new List<Item>
					{
						new Item(351, 3, 1),
						new Item(35, 8, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 12, 1),
					},
					new List<Item>
					{
						new Item(351, 3, 1),
						new Item(35, 7, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 12, 1),
					},
					new List<Item>
					{
						new Item(351, 3, 1),
						new Item(35, 6, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 11, 1),
					},
					new List<Item>
					{
						new Item(351, 4, 1),
						new Item(35, 15, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 11, 1),
					},
					new List<Item>
					{
						new Item(351, 4, 1),
						new Item(35, 14, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 11, 1),
					},
					new List<Item>
					{
						new Item(351, 4, 1),
						new Item(35, 5, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 11, 1),
					},
					new List<Item>
					{
						new Item(351, 4, 1),
						new Item(35, 4, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 11, 1),
					},
					new List<Item>
					{
						new Item(351, 4, 1),
						new Item(35, 3, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 11, 1),
					},
					new List<Item>
					{
						new Item(351, 4, 1),
						new Item(35, 2, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 11, 1),
					},
					new List<Item>
					{
						new Item(351, 4, 1),
						new Item(35, 1, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 11, 1),
					},
					new List<Item>
					{
						new Item(351, 4, 1),
						new Item(35, 0, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 11, 1),
					},
					new List<Item>
					{
						new Item(351, 4, 1),
						new Item(35, 13, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 11, 1),
					},
					new List<Item>
					{
						new Item(351, 4, 1),
						new Item(35, 12, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 11, 1),
					},
					new List<Item>
					{
						new Item(351, 4, 1),
						new Item(35, 10, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 11, 1),
					},
					new List<Item>
					{
						new Item(351, 4, 1),
						new Item(35, 9, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 11, 1),
					},
					new List<Item>
					{
						new Item(351, 4, 1),
						new Item(35, 8, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 11, 1),
					},
					new List<Item>
					{
						new Item(351, 4, 1),
						new Item(35, 7, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 11, 1),
					},
					new List<Item>
					{
						new Item(351, 4, 1),
						new Item(35, 6, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 10, 1),
					},
					new List<Item>
					{
						new Item(351, 5, 1),
						new Item(35, 15, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 10, 1),
					},
					new List<Item>
					{
						new Item(351, 5, 1),
						new Item(35, 14, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 10, 1),
					},
					new List<Item>
					{
						new Item(351, 5, 1),
						new Item(35, 5, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 10, 1),
					},
					new List<Item>
					{
						new Item(351, 5, 1),
						new Item(35, 4, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 10, 1),
					},
					new List<Item>
					{
						new Item(351, 5, 1),
						new Item(35, 3, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 10, 1),
					},
					new List<Item>
					{
						new Item(351, 5, 1),
						new Item(35, 2, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 10, 1),
					},
					new List<Item>
					{
						new Item(351, 5, 1),
						new Item(35, 1, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 10, 1),
					},
					new List<Item>
					{
						new Item(351, 5, 1),
						new Item(35, 0, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 10, 1),
					},
					new List<Item>
					{
						new Item(351, 5, 1),
						new Item(35, 13, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 10, 1),
					},
					new List<Item>
					{
						new Item(351, 5, 1),
						new Item(35, 12, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 10, 1),
					},
					new List<Item>
					{
						new Item(351, 5, 1),
						new Item(35, 11, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 10, 1),
					},
					new List<Item>
					{
						new Item(351, 5, 1),
						new Item(35, 9, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 10, 1),
					},
					new List<Item>
					{
						new Item(351, 5, 1),
						new Item(35, 8, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 10, 1),
					},
					new List<Item>
					{
						new Item(351, 5, 1),
						new Item(35, 7, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 10, 1),
					},
					new List<Item>
					{
						new Item(351, 5, 1),
						new Item(35, 6, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 9, 1),
					},
					new List<Item>
					{
						new Item(351, 6, 1),
						new Item(35, 15, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 9, 1),
					},
					new List<Item>
					{
						new Item(351, 6, 1),
						new Item(35, 14, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 9, 1),
					},
					new List<Item>
					{
						new Item(351, 6, 1),
						new Item(35, 5, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 9, 1),
					},
					new List<Item>
					{
						new Item(351, 6, 1),
						new Item(35, 4, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 9, 1),
					},
					new List<Item>
					{
						new Item(351, 6, 1),
						new Item(35, 3, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 9, 1),
					},
					new List<Item>
					{
						new Item(351, 6, 1),
						new Item(35, 2, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 9, 1),
					},
					new List<Item>
					{
						new Item(351, 6, 1),
						new Item(35, 1, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 9, 1),
					},
					new List<Item>
					{
						new Item(351, 6, 1),
						new Item(35, 0, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 9, 1),
					},
					new List<Item>
					{
						new Item(351, 6, 1),
						new Item(35, 13, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 9, 1),
					},
					new List<Item>
					{
						new Item(351, 6, 1),
						new Item(35, 12, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 9, 1),
					},
					new List<Item>
					{
						new Item(351, 6, 1),
						new Item(35, 11, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 9, 1),
					},
					new List<Item>
					{
						new Item(351, 6, 1),
						new Item(35, 10, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 9, 1),
					},
					new List<Item>
					{
						new Item(351, 6, 1),
						new Item(35, 8, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 9, 1),
					},
					new List<Item>
					{
						new Item(351, 6, 1),
						new Item(35, 7, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 9, 1),
					},
					new List<Item>
					{
						new Item(351, 6, 1),
						new Item(35, 6, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 8, 1),
					},
					new List<Item>
					{
						new Item(351, 7, 1),
						new Item(35, 15, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 8, 1),
					},
					new List<Item>
					{
						new Item(351, 7, 1),
						new Item(35, 14, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 8, 1),
					},
					new List<Item>
					{
						new Item(351, 7, 1),
						new Item(35, 5, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 8, 1),
					},
					new List<Item>
					{
						new Item(351, 7, 1),
						new Item(35, 4, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 8, 1),
					},
					new List<Item>
					{
						new Item(351, 7, 1),
						new Item(35, 3, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 8, 1),
					},
					new List<Item>
					{
						new Item(351, 7, 1),
						new Item(35, 2, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 8, 1),
					},
					new List<Item>
					{
						new Item(351, 7, 1),
						new Item(35, 1, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 8, 1),
					},
					new List<Item>
					{
						new Item(351, 7, 1),
						new Item(35, 0, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 8, 1),
					},
					new List<Item>
					{
						new Item(351, 7, 1),
						new Item(35, 13, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 8, 1),
					},
					new List<Item>
					{
						new Item(351, 7, 1),
						new Item(35, 12, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 8, 1),
					},
					new List<Item>
					{
						new Item(351, 7, 1),
						new Item(35, 11, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 8, 1),
					},
					new List<Item>
					{
						new Item(351, 7, 1),
						new Item(35, 10, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 8, 1),
					},
					new List<Item>
					{
						new Item(351, 7, 1),
						new Item(35, 9, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 8, 1),
					},
					new List<Item>
					{
						new Item(351, 7, 1),
						new Item(35, 7, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 8, 1),
					},
					new List<Item>
					{
						new Item(351, 7, 1),
						new Item(35, 6, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 7, 1),
					},
					new List<Item>
					{
						new Item(351, 8, 1),
						new Item(35, 15, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 7, 1),
					},
					new List<Item>
					{
						new Item(351, 8, 1),
						new Item(35, 14, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 7, 1),
					},
					new List<Item>
					{
						new Item(351, 8, 1),
						new Item(35, 5, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 7, 1),
					},
					new List<Item>
					{
						new Item(351, 8, 1),
						new Item(35, 4, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 7, 1),
					},
					new List<Item>
					{
						new Item(351, 8, 1),
						new Item(35, 3, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 7, 1),
					},
					new List<Item>
					{
						new Item(351, 8, 1),
						new Item(35, 2, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 7, 1),
					},
					new List<Item>
					{
						new Item(351, 8, 1),
						new Item(35, 1, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 7, 1),
					},
					new List<Item>
					{
						new Item(351, 8, 1),
						new Item(35, 0, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 7, 1),
					},
					new List<Item>
					{
						new Item(351, 8, 1),
						new Item(35, 13, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 7, 1),
					},
					new List<Item>
					{
						new Item(351, 8, 1),
						new Item(35, 12, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 7, 1),
					},
					new List<Item>
					{
						new Item(351, 8, 1),
						new Item(35, 11, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 7, 1),
					},
					new List<Item>
					{
						new Item(351, 8, 1),
						new Item(35, 10, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 7, 1),
					},
					new List<Item>
					{
						new Item(351, 8, 1),
						new Item(35, 9, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 7, 1),
					},
					new List<Item>
					{
						new Item(351, 8, 1),
						new Item(35, 8, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 7, 1),
					},
					new List<Item>
					{
						new Item(351, 8, 1),
						new Item(35, 6, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 6, 1),
					},
					new List<Item>
					{
						new Item(351, 9, 1),
						new Item(35, 15, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 6, 1),
					},
					new List<Item>
					{
						new Item(351, 9, 1),
						new Item(35, 14, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 6, 1),
					},
					new List<Item>
					{
						new Item(351, 9, 1),
						new Item(35, 5, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 6, 1),
					},
					new List<Item>
					{
						new Item(351, 9, 1),
						new Item(35, 4, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 6, 1),
					},
					new List<Item>
					{
						new Item(351, 9, 1),
						new Item(35, 3, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 6, 1),
					},
					new List<Item>
					{
						new Item(351, 9, 1),
						new Item(35, 2, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 6, 1),
					},
					new List<Item>
					{
						new Item(351, 9, 1),
						new Item(35, 1, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 6, 1),
					},
					new List<Item>
					{
						new Item(351, 9, 1),
						new Item(35, 0, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 6, 1),
					},
					new List<Item>
					{
						new Item(351, 9, 1),
						new Item(35, 13, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 6, 1),
					},
					new List<Item>
					{
						new Item(351, 9, 1),
						new Item(35, 12, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 6, 1),
					},
					new List<Item>
					{
						new Item(351, 9, 1),
						new Item(35, 11, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 6, 1),
					},
					new List<Item>
					{
						new Item(351, 9, 1),
						new Item(35, 10, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 6, 1),
					},
					new List<Item>
					{
						new Item(351, 9, 1),
						new Item(35, 9, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 6, 1),
					},
					new List<Item>
					{
						new Item(351, 9, 1),
						new Item(35, 8, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 6, 1),
					},
					new List<Item>
					{
						new Item(351, 9, 1),
						new Item(35, 7, 1),
					}, "crafting_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-162, 3, 2),
					},
					new List<Item>
					{
						new Item(1, 5, 1),
					}, "stonecutter"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-171, 0, 1),
					},
					new List<Item>
					{
						new Item(1, 5, 1),
					}, "stonecutter"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(139, 4, 1),
					},
					new List<Item>
					{
						new Item(1, 5, 1),
					}, "stonecutter"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(44, 4, 2),
					},
					new List<Item>
					{
						new Item(45, 0, 1),
					}, "stonecutter"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(108, 0, 1),
					},
					new List<Item>
					{
						new Item(45, 0, 1),
					}, "stonecutter"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(139, 6, 1),
					},
					new List<Item>
					{
						new Item(45, 0, 1),
					}, "stonecutter"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(44, 3, 2),
					},
					new List<Item>
					{
						new Item(4, 0, 1),
					}, "stonecutter"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(67, 0, 1),
					},
					new List<Item>
					{
						new Item(4, 0, 1),
					}, "stonecutter"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(139, 0, 1),
					},
					new List<Item>
					{
						new Item(4, 0, 1),
					}, "stonecutter"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(182, 3, 2),
					},
					new List<Item>
					{
						new Item(168, 1, 1),
					}, "stonecutter"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-3, 0, 1),
					},
					new List<Item>
					{
						new Item(168, 1, 1),
					}, "stonecutter"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-162, 4, 2),
					},
					new List<Item>
					{
						new Item(1, 3, 1),
					}, "stonecutter"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-170, 0, 1),
					},
					new List<Item>
					{
						new Item(1, 3, 1),
					}, "stonecutter"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(139, 3, 1),
					},
					new List<Item>
					{
						new Item(1, 3, 1),
					}, "stonecutter"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-166, 2, 2),
					},
					new List<Item>
					{
						new Item(1, 0, 1),
					}, "stonecutter"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-162, 0, 2),
					},
					new List<Item>
					{
						new Item(121, 0, 1),
					}, "stonecutter"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-162, 0, 2),
					},
					new List<Item>
					{
						new Item(206, 0, 1),
					}, "stonecutter"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-178, 0, 1),
					},
					new List<Item>
					{
						new Item(121, 0, 1),
					}, "stonecutter"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-178, 0, 1),
					},
					new List<Item>
					{
						new Item(206, 0, 1),
					}, "stonecutter"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(139, 10, 1),
					},
					new List<Item>
					{
						new Item(121, 0, 1),
					}, "stonecutter"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(139, 10, 1),
					},
					new List<Item>
					{
						new Item(206, 0, 1),
					}, "stonecutter"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(206, 0, 1),
					},
					new List<Item>
					{
						new Item(121, 0, 1),
					}, "stonecutter"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-162, 6, 2),
					},
					new List<Item>
					{
						new Item(1, 1, 1),
					}, "stonecutter"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-169, 0, 1),
					},
					new List<Item>
					{
						new Item(1, 1, 1),
					}, "stonecutter"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(139, 2, 1),
					},
					new List<Item>
					{
						new Item(1, 1, 1),
					}, "stonecutter"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(182, 5, 2),
					},
					new List<Item>
					{
						new Item(48, 0, 1),
					}, "stonecutter"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-179, 0, 1),
					},
					new List<Item>
					{
						new Item(48, 0, 1),
					}, "stonecutter"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(139, 1, 1),
					},
					new List<Item>
					{
						new Item(48, 0, 1),
					}, "stonecutter"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-166, 0, 2),
					},
					new List<Item>
					{
						new Item(98, 1, 1),
					}, "stonecutter"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-175, 0, 1),
					},
					new List<Item>
					{
						new Item(98, 1, 1),
					}, "stonecutter"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(139, 8, 1),
					},
					new List<Item>
					{
						new Item(98, 1, 1),
					}, "stonecutter"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(44, 7, 2),
					},
					new List<Item>
					{
						new Item(112, 0, 1),
					}, "stonecutter"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(114, 0, 1),
					},
					new List<Item>
					{
						new Item(112, 0, 1),
					}, "stonecutter"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(139, 9, 1),
					},
					new List<Item>
					{
						new Item(112, 0, 1),
					}, "stonecutter"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(1, 6, 1),
					},
					new List<Item>
					{
						new Item(1, 5, 1),
					}, "stonecutter"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-162, 2, 2),
					},
					new List<Item>
					{
						new Item(1, 5, 1),
					}, "stonecutter"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-162, 2, 2),
					},
					new List<Item>
					{
						new Item(1, 6, 1),
					}, "stonecutter"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-174, 0, 1),
					},
					new List<Item>
					{
						new Item(1, 5, 1),
					}, "stonecutter"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-174, 0, 1),
					},
					new List<Item>
					{
						new Item(1, 6, 1),
					}, "stonecutter"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(1, 4, 1),
					},
					new List<Item>
					{
						new Item(1, 3, 1),
					}, "stonecutter"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-162, 5, 2),
					},
					new List<Item>
					{
						new Item(1, 3, 1),
					}, "stonecutter"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-162, 5, 2),
					},
					new List<Item>
					{
						new Item(1, 4, 1),
					}, "stonecutter"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-173, 0, 1),
					},
					new List<Item>
					{
						new Item(1, 3, 1),
					}, "stonecutter"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-173, 0, 1),
					},
					new List<Item>
					{
						new Item(1, 4, 1),
					}, "stonecutter"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(1, 2, 1),
					},
					new List<Item>
					{
						new Item(1, 1, 1),
					}, "stonecutter"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-162, 7, 2),
					},
					new List<Item>
					{
						new Item(1, 1, 1),
					}, "stonecutter"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-162, 7, 2),
					},
					new List<Item>
					{
						new Item(1, 2, 1),
					}, "stonecutter"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-172, 0, 1),
					},
					new List<Item>
					{
						new Item(1, 1, 1),
					}, "stonecutter"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-172, 0, 1),
					},
					new List<Item>
					{
						new Item(1, 2, 1),
					}, "stonecutter"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(182, 4, 2),
					},
					new List<Item>
					{
						new Item(168, 2, 1),
					}, "stonecutter"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-4, 0, 1),
					},
					new List<Item>
					{
						new Item(168, 2, 1),
					}, "stonecutter"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(182, 2, 2),
					},
					new List<Item>
					{
						new Item(168, 0, 1),
					}, "stonecutter"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-2, 0, 1),
					},
					new List<Item>
					{
						new Item(168, 0, 1),
					}, "stonecutter"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(139, 11, 1),
					},
					new List<Item>
					{
						new Item(168, 0, 1),
					}, "stonecutter"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(201, 2, 1),
					},
					new List<Item>
					{
						new Item(201, 0, 1),
					}, "stonecutter"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(182, 1, 2),
					},
					new List<Item>
					{
						new Item(201, 0, 1),
					}, "stonecutter"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(203, 0, 1),
					},
					new List<Item>
					{
						new Item(201, 0, 1),
					}, "stonecutter"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(155, 1, 1),
					},
					new List<Item>
					{
						new Item(155, 0, 1),
					}, "stonecutter"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(155, 2, 1),
					},
					new List<Item>
					{
						new Item(155, 0, 1),
					}, "stonecutter"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-166, 1, 2),
					},
					new List<Item>
					{
						new Item(155, 0, 1),
					}, "stonecutter"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(156, 0, 1),
					},
					new List<Item>
					{
						new Item(155, 0, 1),
					}, "stonecutter"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(182, 7, 2),
					},
					new List<Item>
					{
						new Item(215, 0, 1),
					}, "stonecutter"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-184, 0, 1),
					},
					new List<Item>
					{
						new Item(215, 0, 1),
					}, "stonecutter"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(139, 13, 1),
					},
					new List<Item>
					{
						new Item(215, 0, 1),
					}, "stonecutter"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(182, 0, 2),
					},
					new List<Item>
					{
						new Item(179, 0, 1),
					}, "stonecutter"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(179, 2, 1),
					},
					new List<Item>
					{
						new Item(179, 0, 1),
					}, "stonecutter"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(179, 1, 1),
					},
					new List<Item>
					{
						new Item(179, 0, 1),
					}, "stonecutter"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(180, 0, 1),
					},
					new List<Item>
					{
						new Item(179, 0, 1),
					}, "stonecutter"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(139, 12, 1),
					},
					new List<Item>
					{
						new Item(179, 0, 1),
					}, "stonecutter"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(44, 1, 2),
					},
					new List<Item>
					{
						new Item(24, 0, 1),
					}, "stonecutter"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(24, 2, 1),
					},
					new List<Item>
					{
						new Item(24, 0, 1),
					}, "stonecutter"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(24, 1, 1),
					},
					new List<Item>
					{
						new Item(24, 0, 1),
					}, "stonecutter"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(128, 0, 1),
					},
					new List<Item>
					{
						new Item(24, 0, 1),
					}, "stonecutter"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(139, 5, 1),
					},
					new List<Item>
					{
						new Item(24, 0, 1),
					}, "stonecutter"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(44, 0, 2),
					},
					new List<Item>
					{
						new Item(-183, 0, 1),
					}, "stonecutter"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-166, 1, 2),
					},
					new List<Item>
					{
						new Item(155, 3, 1),
					}, "stonecutter"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-185, 0, 1),
					},
					new List<Item>
					{
						new Item(155, 3, 1),
					}, "stonecutter"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-162, 1, 2),
					},
					new List<Item>
					{
						new Item(179, 3, 1),
					}, "stonecutter"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-176, 0, 1),
					},
					new List<Item>
					{
						new Item(179, 3, 1),
					}, "stonecutter"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(182, 6, 2),
					},
					new List<Item>
					{
						new Item(24, 3, 1),
					}, "stonecutter"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-177, 0, 1),
					},
					new List<Item>
					{
						new Item(24, 3, 1),
					}, "stonecutter"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-180, 0, 1),
					},
					new List<Item>
					{
						new Item(1, 0, 1),
					}, "stonecutter"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(98, 0, 1),
					},
					new List<Item>
					{
						new Item(1, 0, 1),
					}, "stonecutter"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(98, 3, 1),
					},
					new List<Item>
					{
						new Item(1, 0, 1),
					}, "stonecutter"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(44, 5, 2),
					},
					new List<Item>
					{
						new Item(1, 0, 1),
					}, "stonecutter"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(44, 5, 2),
					},
					new List<Item>
					{
						new Item(98, 0, 1),
					}, "stonecutter"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(109, 0, 1),
					},
					new List<Item>
					{
						new Item(1, 0, 1),
					}, "stonecutter"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(109, 0, 1),
					},
					new List<Item>
					{
						new Item(98, 0, 1),
					}, "stonecutter"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(139, 7, 1),
					},
					new List<Item>
					{
						new Item(1, 0, 1),
					}, "stonecutter"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(139, 7, 1),
					},
					new List<Item>
					{
						new Item(98, 0, 1),
					}, "stonecutter"),
				new MultiRecipe() { Id = new UUID("442d85ed-8272-4543-a6f1-418f90ded05d") }, // 442d85ed-8272-4543-a6f1-418f90ded05d
				new MultiRecipe() { Id = new UUID("8b36268c-1829-483c-a0f1-993b7156a8f2") }, // 8b36268c-1829-483c-a0f1-993b7156a8f2
				new MultiRecipe() { Id = new UUID("602234e4-cac1-4353-8bb7-b1ebff70024b") }, // 602234e4-cac1-4353-8bb7-b1ebff70024b
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(395, 2, 1),
					},
					new List<Item>
					{
						new Item(339, 32767, 1),
						new Item(345, 32767, 1),
					}, "cartography_table"),
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(395, 0, 1),
					},
					new List<Item>
					{
						new Item(339, 32767, 1),
					}, "cartography_table"),
				new MultiRecipe() { Id = new UUID("98c84b38-1085-46bd-b1ce-dd38c159e6cc") }, // 98c84b38-1085-46bd-b1ce-dd38c159e6cc
				new SmeltingRecipe(new Item(263, 1, 1), new Item(-212, 0), "furnace"),
				new SmeltingRecipe(new Item(263, 1, 1), new Item(-212, 1), "furnace"),
				new SmeltingRecipe(new Item(263, 1, 1), new Item(-212, 2), "furnace"),
				new SmeltingRecipe(new Item(263, 1, 1), new Item(-212, 3), "furnace"),
				new SmeltingRecipe(new Item(263, 1, 1), new Item(-212, 4), "furnace"),
				new SmeltingRecipe(new Item(263, 1, 1), new Item(-212, 5), "furnace"),
				new SmeltingRecipe(new Item(263, 1, 1), new Item(-212, 8), "furnace"),
				new SmeltingRecipe(new Item(263, 1, 1), new Item(-212, 9), "furnace"),
				new SmeltingRecipe(new Item(263, 1, 1), new Item(-212, 10), "furnace"),
				new SmeltingRecipe(new Item(263, 1, 1), new Item(-212, 11), "furnace"),
				new SmeltingRecipe(new Item(263, 1, 1), new Item(-212, 12), "furnace"),
				new SmeltingRecipe(new Item(263, 1, 1), new Item(-212, 13), "furnace"),
				new SmeltingRecipe(new Item(351, 10, 1), new Item(-156, 32767), "furnace"),
				new SmeltingRecipe(new Item(263, 1, 1), new Item(-10, 32767), "furnace"),
				new SmeltingRecipe(new Item(263, 1, 1), new Item(-9, 32767), "furnace"),
				new SmeltingRecipe(new Item(263, 1, 1), new Item(-8, 32767), "furnace"),
				new SmeltingRecipe(new Item(263, 1, 1), new Item(-7, 32767), "furnace"),
				new SmeltingRecipe(new Item(263, 1, 1), new Item(-6, 32767), "furnace"),
				new SmeltingRecipe(new Item(263, 1, 1), new Item(-5, 32767), "furnace"),
				new SmeltingRecipe(new Item(-183, -1, 1), new Item(1, 0), "furnace"),
				new SmeltingRecipe(new Item(1, -1, 1), new Item(4, 32767), "furnace"),
				new SmeltingRecipe(new Item(20, -1, 1), new Item(12, 32767), "furnace"),
				new SmeltingRecipe(new Item(266, -1, 1), new Item(14, 32767), "furnace"),
				new SmeltingRecipe(new Item(266, -1, 1), new Item(14, 32767), "blast_furnace"),
				new SmeltingRecipe(new Item(265, -1, 1), new Item(15, 32767), "furnace"),
				new SmeltingRecipe(new Item(265, -1, 1), new Item(15, 32767), "blast_furnace"),
				new SmeltingRecipe(new Item(263, -1, 1), new Item(16, 32767), "furnace"),
				new SmeltingRecipe(new Item(263, -1, 1), new Item(16, 32767), "blast_furnace"),
				new SmeltingRecipe(new Item(263, 1, 1), new Item(17, 0), "furnace"),
				new SmeltingRecipe(new Item(263, 1, 1), new Item(17, 1), "furnace"),
				new SmeltingRecipe(new Item(263, 1, 1), new Item(17, 2), "furnace"),
				new SmeltingRecipe(new Item(263, 1, 1), new Item(17, 3), "furnace"),
				new SmeltingRecipe(new Item(19, 0, 1), new Item(19, 1), "furnace"),
				new SmeltingRecipe(new Item(351, 4, 1), new Item(21, 32767), "furnace"),
				new SmeltingRecipe(new Item(351, 4, 1), new Item(21, 32767), "blast_furnace"),
				new SmeltingRecipe(new Item(24, 3, 1), new Item(24, 32767), "furnace"),
				new SmeltingRecipe(new Item(264, -1, 1), new Item(56, 32767), "furnace"),
				new SmeltingRecipe(new Item(264, -1, 1), new Item(56, 32767), "blast_furnace"),
				new SmeltingRecipe(new Item(331, -1, 1), new Item(73, 32767), "furnace"),
				new SmeltingRecipe(new Item(331, -1, 1), new Item(73, 32767), "blast_furnace"),
				new SmeltingRecipe(new Item(351, 2, 1), new Item(81, 32767), "furnace"),
				new SmeltingRecipe(new Item(172, -1, 1), new Item(82, 32767), "furnace"),
				new SmeltingRecipe(new Item(405, -1, 1), new Item(87, 32767), "furnace"),
				new SmeltingRecipe(new Item(98, 2, 1), new Item(98, 0), "furnace"),
				new SmeltingRecipe(new Item(388, -1, 1), new Item(129, 32767), "furnace"),
				new SmeltingRecipe(new Item(388, -1, 1), new Item(129, 32767), "blast_furnace"),
				new SmeltingRecipe(new Item(406, -1, 1), new Item(153, 32767), "furnace"),
				new SmeltingRecipe(new Item(406, -1, 1), new Item(153, 32767), "blast_furnace"),
				new SmeltingRecipe(new Item(155, 3, 1), new Item(155, 32767), "furnace"),
				new SmeltingRecipe(new Item(220, -1, 1), new Item(159, 0), "furnace"),
				new SmeltingRecipe(new Item(221, -1, 1), new Item(159, 1), "furnace"),
				new SmeltingRecipe(new Item(222, -1, 1), new Item(159, 2), "furnace"),
				new SmeltingRecipe(new Item(223, -1, 1), new Item(159, 3), "furnace"),
				new SmeltingRecipe(new Item(224, -1, 1), new Item(159, 4), "furnace"),
				new SmeltingRecipe(new Item(225, -1, 1), new Item(159, 5), "furnace"),
				new SmeltingRecipe(new Item(226, -1, 1), new Item(159, 6), "furnace"),
				new SmeltingRecipe(new Item(227, -1, 1), new Item(159, 7), "furnace"),
				new SmeltingRecipe(new Item(228, -1, 1), new Item(159, 8), "furnace"),
				new SmeltingRecipe(new Item(229, -1, 1), new Item(159, 9), "furnace"),
				new SmeltingRecipe(new Item(219, -1, 1), new Item(159, 10), "furnace"),
				new SmeltingRecipe(new Item(231, -1, 1), new Item(159, 11), "furnace"),
				new SmeltingRecipe(new Item(232, -1, 1), new Item(159, 12), "furnace"),
				new SmeltingRecipe(new Item(233, -1, 1), new Item(159, 13), "furnace"),
				new SmeltingRecipe(new Item(234, -1, 1), new Item(159, 14), "furnace"),
				new SmeltingRecipe(new Item(235, -1, 1), new Item(159, 15), "furnace"),
				new SmeltingRecipe(new Item(263, 1, 1), new Item(162, 0), "furnace"),
				new SmeltingRecipe(new Item(263, 1, 1), new Item(162, 1), "furnace"),
				new SmeltingRecipe(new Item(179, 3, 1), new Item(179, 32767), "furnace"),
				new SmeltingRecipe(new Item(452, -1, 1), new Item(256, 0), "furnace"),
				new SmeltingRecipe(new Item(452, -1, 1), new Item(256, 0), "blast_furnace"),
				new SmeltingRecipe(new Item(452, -1, 1), new Item(257, 0), "furnace"),
				new SmeltingRecipe(new Item(452, -1, 1), new Item(257, 0), "blast_furnace"),
				new SmeltingRecipe(new Item(452, -1, 1), new Item(258, 0), "furnace"),
				new SmeltingRecipe(new Item(452, -1, 1), new Item(258, 0), "blast_furnace"),
				new SmeltingRecipe(new Item(452, -1, 1), new Item(267, 0), "furnace"),
				new SmeltingRecipe(new Item(452, -1, 1), new Item(267, 0), "blast_furnace"),
				new SmeltingRecipe(new Item(371, -1, 1), new Item(283, 0), "furnace"),
				new SmeltingRecipe(new Item(371, -1, 1), new Item(283, 0), "blast_furnace"),
				new SmeltingRecipe(new Item(371, -1, 1), new Item(284, 0), "furnace"),
				new SmeltingRecipe(new Item(371, -1, 1), new Item(284, 0), "blast_furnace"),
				new SmeltingRecipe(new Item(371, -1, 1), new Item(285, 0), "furnace"),
				new SmeltingRecipe(new Item(371, -1, 1), new Item(285, 0), "blast_furnace"),
				new SmeltingRecipe(new Item(371, -1, 1), new Item(286, 0), "furnace"),
				new SmeltingRecipe(new Item(371, -1, 1), new Item(286, 0), "blast_furnace"),
				new SmeltingRecipe(new Item(452, -1, 1), new Item(292, 0), "furnace"),
				new SmeltingRecipe(new Item(452, -1, 1), new Item(292, 0), "blast_furnace"),
				new SmeltingRecipe(new Item(371, -1, 1), new Item(294, 0), "furnace"),
				new SmeltingRecipe(new Item(371, -1, 1), new Item(294, 0), "blast_furnace"),
				new SmeltingRecipe(new Item(452, -1, 1), new Item(302, 0), "furnace"),
				new SmeltingRecipe(new Item(452, -1, 1), new Item(302, 0), "blast_furnace"),
				new SmeltingRecipe(new Item(452, -1, 1), new Item(303, 0), "furnace"),
				new SmeltingRecipe(new Item(452, -1, 1), new Item(303, 0), "blast_furnace"),
				new SmeltingRecipe(new Item(452, -1, 1), new Item(304, 0), "furnace"),
				new SmeltingRecipe(new Item(452, -1, 1), new Item(304, 0), "blast_furnace"),
				new SmeltingRecipe(new Item(452, -1, 1), new Item(305, 0), "furnace"),
				new SmeltingRecipe(new Item(452, -1, 1), new Item(305, 0), "blast_furnace"),
				new SmeltingRecipe(new Item(452, -1, 1), new Item(306, 0), "furnace"),
				new SmeltingRecipe(new Item(452, -1, 1), new Item(306, 0), "blast_furnace"),
				new SmeltingRecipe(new Item(452, -1, 1), new Item(307, 0), "furnace"),
				new SmeltingRecipe(new Item(452, -1, 1), new Item(307, 0), "blast_furnace"),
				new SmeltingRecipe(new Item(452, -1, 1), new Item(308, 0), "furnace"),
				new SmeltingRecipe(new Item(452, -1, 1), new Item(308, 0), "blast_furnace"),
				new SmeltingRecipe(new Item(452, -1, 1), new Item(309, 0), "furnace"),
				new SmeltingRecipe(new Item(452, -1, 1), new Item(309, 0), "blast_furnace"),
				new SmeltingRecipe(new Item(371, -1, 1), new Item(314, 0), "furnace"),
				new SmeltingRecipe(new Item(371, -1, 1), new Item(314, 0), "blast_furnace"),
				new SmeltingRecipe(new Item(371, -1, 1), new Item(315, 0), "furnace"),
				new SmeltingRecipe(new Item(371, -1, 1), new Item(315, 0), "blast_furnace"),
				new SmeltingRecipe(new Item(371, -1, 1), new Item(316, 0), "furnace"),
				new SmeltingRecipe(new Item(371, -1, 1), new Item(316, 0), "blast_furnace"),
				new SmeltingRecipe(new Item(371, -1, 1), new Item(317, 0), "furnace"),
				new SmeltingRecipe(new Item(371, -1, 1), new Item(317, 0), "blast_furnace"),
				new SmeltingRecipe(new Item(320, -1, 1), new Item(319, 32767), "smoker"),
				new SmeltingRecipe(new Item(320, -1, 1), new Item(319, 32767), "furnace"),
				new SmeltingRecipe(new Item(320, -1, 1), new Item(319, 32767), "campfire"),
				new SmeltingRecipe(new Item(464, -1, 1), new Item(335, 32767), "smoker"),
				new SmeltingRecipe(new Item(464, -1, 1), new Item(335, 32767), "furnace"),
				new SmeltingRecipe(new Item(464, -1, 1), new Item(335, 32767), "campfire"),
				new SmeltingRecipe(new Item(336, -1, 1), new Item(337, 32767), "furnace"),
				new SmeltingRecipe(new Item(350, -1, 1), new Item(349, 32767), "smoker"),
				new SmeltingRecipe(new Item(350, -1, 1), new Item(349, 32767), "furnace"),
				new SmeltingRecipe(new Item(350, -1, 1), new Item(349, 32767), "campfire"),
				new SmeltingRecipe(new Item(364, -1, 1), new Item(363, 32767), "smoker"),
				new SmeltingRecipe(new Item(364, -1, 1), new Item(363, 32767), "furnace"),
				new SmeltingRecipe(new Item(364, -1, 1), new Item(363, 32767), "campfire"),
				new SmeltingRecipe(new Item(366, -1, 1), new Item(365, 32767), "smoker"),
				new SmeltingRecipe(new Item(366, -1, 1), new Item(365, 32767), "furnace"),
				new SmeltingRecipe(new Item(366, -1, 1), new Item(365, 32767), "campfire"),
				new SmeltingRecipe(new Item(393, -1, 1), new Item(392, 32767), "smoker"),
				new SmeltingRecipe(new Item(393, -1, 1), new Item(392, 32767), "furnace"),
				new SmeltingRecipe(new Item(393, -1, 1), new Item(392, 32767), "campfire"),
				new SmeltingRecipe(new Item(412, -1, 1), new Item(411, 32767), "smoker"),
				new SmeltingRecipe(new Item(412, -1, 1), new Item(411, 32767), "furnace"),
				new SmeltingRecipe(new Item(412, -1, 1), new Item(411, 32767), "campfire"),
				new SmeltingRecipe(new Item(452, -1, 1), new Item(417, 32767), "furnace"),
				new SmeltingRecipe(new Item(452, -1, 1), new Item(417, 32767), "blast_furnace"),
				new SmeltingRecipe(new Item(371, -1, 1), new Item(418, 32767), "furnace"),
				new SmeltingRecipe(new Item(371, -1, 1), new Item(418, 32767), "blast_furnace"),
				new SmeltingRecipe(new Item(424, -1, 1), new Item(423, 32767), "smoker"),
				new SmeltingRecipe(new Item(424, -1, 1), new Item(423, 32767), "furnace"),
				new SmeltingRecipe(new Item(424, -1, 1), new Item(423, 32767), "campfire"),
				new SmeltingRecipe(new Item(433, -1, 1), new Item(432, 32767), "furnace"),
				new SmeltingRecipe(new Item(463, -1, 1), new Item(460, 32767), "smoker"),
				new SmeltingRecipe(new Item(463, -1, 1), new Item(460, 32767), "furnace"),
				new SmeltingRecipe(new Item(463, -1, 1), new Item(460, 32767), "campfire"),
			};
		}

		public static void Add(Recipe recipe)
		{
			Log.InfoFormat("{0}", recipe.Id);
		}
	}
}