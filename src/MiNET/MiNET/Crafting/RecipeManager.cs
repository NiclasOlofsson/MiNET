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
using MiNET.Net.RakNet;
using MiNET.Utils;
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
				var craftingData = McpeCraftingData.CreateObject();
				craftingData.recipes = Recipes;
				craftingData.isClean = true;
				var packet = Level.CreateMcpeBatch(craftingData.Encode());
				craftingData.PutPool();
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
				new MultiRecipe() { Id = new UUID("442d85ed-8272-4543-a6f1-418f90ded05d"), UniqueId = 2141 }, // 442d85ed-8272-4543-a6f1-418f90ded05d
				new MultiRecipe() { Id = new UUID("8b36268c-1829-483c-a0f1-993b7156a8f2"), UniqueId = 2143 }, // 8b36268c-1829-483c-a0f1-993b7156a8f2
				new MultiRecipe() { Id = new UUID("602234e4-cac1-4353-8bb7-b1ebff70024b"), UniqueId = 2144 }, // 602234e4-cac1-4353-8bb7-b1ebff70024b
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(515, 2, 1){ UniqueId = 41293861, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(772, -2, 2){ UniqueId = 41293857, RuntimeId=0 },
						new Item(782, -2, 2){ UniqueId = 41293857, RuntimeId=0 },
					}, "cartography_table"){ UniqueId = 500 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(515, 0, 1){ UniqueId = 41293861, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(772, -2, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "cartography_table"){ UniqueId = 626 },
				new MultiRecipe() { Id = new UUID("98c84b38-1085-46bd-b1ce-dd38c159e6cc"), UniqueId = 2146 }, // 98c84b38-1085-46bd-b1ce-dd38c159e6cc
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-395, 0, 1){ UniqueId = 41293861, RuntimeId=1129 },
					},
					new List<Item>
					{
						new Item(757, 0, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 116 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-380, 0, 2){ UniqueId = 41293861, RuntimeId=1145 },
					},
					new List<Item>
					{
						new Item(757, 0, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 68 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-381, 0, 1){ UniqueId = 41293861, RuntimeId=1147 },
					},
					new List<Item>
					{
						new Item(757, 0, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 73 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-382, 0, 1){ UniqueId = 41293861, RuntimeId=1155 },
					},
					new List<Item>
					{
						new Item(757, 0, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 164 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-392, 0, 2){ UniqueId = 41293861, RuntimeId=4104 },
					},
					new List<Item>
					{
						new Item(757, 0, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 110 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-392, 0, 2){ UniqueId = 41293861, RuntimeId=4104 },
					},
					new List<Item>
					{
						new Item(781, 0, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 43 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-392, 0, 2){ UniqueId = 41293861, RuntimeId=4104 },
					},
					new List<Item>
					{
						new Item(765, 0, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 175 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-393, 0, 1){ UniqueId = 41293861, RuntimeId=4106 },
					},
					new List<Item>
					{
						new Item(757, 0, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 62 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-393, 0, 1){ UniqueId = 41293861, RuntimeId=4106 },
					},
					new List<Item>
					{
						new Item(781, 0, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 174 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-393, 0, 1){ UniqueId = 41293861, RuntimeId=4106 },
					},
					new List<Item>
					{
						new Item(765, 0, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 166 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-394, 0, 1){ UniqueId = 41293861, RuntimeId=4114 },
					},
					new List<Item>
					{
						new Item(757, 0, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 59 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-394, 0, 1){ UniqueId = 41293861, RuntimeId=4114 },
					},
					new List<Item>
					{
						new Item(781, 0, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 46 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-394, 0, 1){ UniqueId = 41293861, RuntimeId=4114 },
					},
					new List<Item>
					{
						new Item(765, 0, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 146 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-391, 0, 1){ UniqueId = 41293861, RuntimeId=4276 },
					},
					new List<Item>
					{
						new Item(757, 0, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 177 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-391, 0, 1){ UniqueId = 41293861, RuntimeId=4276 },
					},
					new List<Item>
					{
						new Item(765, 0, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 22 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-388, 0, 2){ UniqueId = 41293861, RuntimeId=4287 },
					},
					new List<Item>
					{
						new Item(757, 0, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 76 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-388, 0, 2){ UniqueId = 41293861, RuntimeId=4287 },
					},
					new List<Item>
					{
						new Item(781, 0, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 45 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-388, 0, 2){ UniqueId = 41293861, RuntimeId=4287 },
					},
					new List<Item>
					{
						new Item(773, 0, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 178 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-388, 0, 2){ UniqueId = 41293861, RuntimeId=4287 },
					},
					new List<Item>
					{
						new Item(765, 0, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 23 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-389, 0, 1){ UniqueId = 41293861, RuntimeId=4289 },
					},
					new List<Item>
					{
						new Item(757, 0, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 50 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-389, 0, 1){ UniqueId = 41293861, RuntimeId=4289 },
					},
					new List<Item>
					{
						new Item(781, 0, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 91 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-389, 0, 1){ UniqueId = 41293861, RuntimeId=4289 },
					},
					new List<Item>
					{
						new Item(773, 0, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 113 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-389, 0, 1){ UniqueId = 41293861, RuntimeId=4289 },
					},
					new List<Item>
					{
						new Item(765, 0, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 49 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-390, 0, 1){ UniqueId = 41293861, RuntimeId=4297 },
					},
					new List<Item>
					{
						new Item(757, 0, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 132 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-390, 0, 1){ UniqueId = 41293861, RuntimeId=4297 },
					},
					new List<Item>
					{
						new Item(781, 0, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 154 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-390, 0, 1){ UniqueId = 41293861, RuntimeId=4297 },
					},
					new List<Item>
					{
						new Item(773, 0, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 55 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-390, 0, 1){ UniqueId = 41293861, RuntimeId=4297 },
					},
					new List<Item>
					{
						new Item(765, 0, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 35 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-387, 0, 1){ UniqueId = 41293861, RuntimeId=4459 },
					},
					new List<Item>
					{
						new Item(757, 0, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 145 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-387, 0, 1){ UniqueId = 41293861, RuntimeId=4459 },
					},
					new List<Item>
					{
						new Item(781, 0, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 144 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-387, 0, 1){ UniqueId = 41293861, RuntimeId=4459 },
					},
					new List<Item>
					{
						new Item(765, 0, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 60 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-383, 0, 1){ UniqueId = 41293861, RuntimeId=6176 },
					},
					new List<Item>
					{
						new Item(757, 0, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 67 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-384, 0, 2){ UniqueId = 41293861, RuntimeId=6179 },
					},
					new List<Item>
					{
						new Item(757, 0, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 81 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-384, 0, 2){ UniqueId = 41293861, RuntimeId=6179 },
					},
					new List<Item>
					{
						new Item(765, 0, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 160 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-385, 0, 1){ UniqueId = 41293861, RuntimeId=6181 },
					},
					new List<Item>
					{
						new Item(757, 0, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 53 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-385, 0, 1){ UniqueId = 41293861, RuntimeId=6181 },
					},
					new List<Item>
					{
						new Item(765, 0, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 126 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-386, 0, 1){ UniqueId = 41293861, RuntimeId=6189 },
					},
					new List<Item>
					{
						new Item(757, 0, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 75 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-386, 0, 1){ UniqueId = 41293861, RuntimeId=6189 },
					},
					new List<Item>
					{
						new Item(765, 0, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 70 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-162, 0, 2){ UniqueId = 41293861, RuntimeId=7162 },
					},
					new List<Item>
					{
						new Item(2, 10, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 855 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-171, 0, 1){ UniqueId = 41293861, RuntimeId=144 },
					},
					new List<Item>
					{
						new Item(2, 10, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 853 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(139, 0, 1){ UniqueId = 41293861, RuntimeId=1322 },
					},
					new List<Item>
					{
						new Item(2, 10, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 704 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-282, 0, 2){ UniqueId = 41293861, RuntimeId=497 },
					},
					new List<Item>
					{
						new Item(545, -2, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 359 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-276, 0, 1){ UniqueId = 41293861, RuntimeId=499 },
					},
					new List<Item>
					{
						new Item(545, -2, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 297 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-277, 0, 1){ UniqueId = 41293861, RuntimeId=507 },
					},
					new List<Item>
					{
						new Item(545, -2, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 304 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(44, 0, 2){ UniqueId = 41293861, RuntimeId=7131 },
					},
					new List<Item>
					{
						new Item(90, 0, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 786 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-284, 0, 2){ UniqueId = 41293861, RuntimeId=5801 },
					},
					new List<Item>
					{
						new Item(581, -2, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 342 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(108, 0, 1){ UniqueId = 41293861, RuntimeId=876 },
					},
					new List<Item>
					{
						new Item(90, 0, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 875 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-275, 0, 1){ UniqueId = 41293861, RuntimeId=5803 },
					},
					new List<Item>
					{
						new Item(581, -2, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 334 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(139, 0, 1){ UniqueId = 41293861, RuntimeId=1324 },
					},
					new List<Item>
					{
						new Item(90, 0, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 901 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-278, 0, 1){ UniqueId = 41293861, RuntimeId=5811 },
					},
					new List<Item>
					{
						new Item(581, -2, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 371 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-274, 0, 1){ UniqueId = 41293861, RuntimeId=5973 },
					},
					new List<Item>
					{
						new Item(581, -2, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 220 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-279, 0, 1){ UniqueId = 41293861, RuntimeId=1131 },
					},
					new List<Item>
					{
						new Item(581, -2, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 323 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-302, 0, 1){ UniqueId = 41293861, RuntimeId=1130 },
					},
					new List<Item>
					{
						new Item(224, -2, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 303 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-279, 0, 1){ UniqueId = 41293861, RuntimeId=1131 },
					},
					new List<Item>
					{
						new Item(545, -2, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 318 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(44, 0, 2){ UniqueId = 41293861, RuntimeId=7130 },
					},
					new List<Item>
					{
						new Item(8, 0, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 599 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(67, 0, 1){ UniqueId = 41293861, RuntimeId=7185 },
					},
					new List<Item>
					{
						new Item(8, 0, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 983 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(139, 0, 1){ UniqueId = 41293861, RuntimeId=1318 },
					},
					new List<Item>
					{
						new Item(8, 0, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 561 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-347, 0, 1){ UniqueId = 41293861, RuntimeId=3909 },
					},
					new List<Item>
					{
						new Item(679, -2, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 27 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-361, 0, 2){ UniqueId = 41293861, RuntimeId=3910 },
					},
					new List<Item>
					{
						new Item(679, -2, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 123 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-354, 0, 1){ UniqueId = 41293861, RuntimeId=3912 },
					},
					new List<Item>
					{
						new Item(679, -2, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 98 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-361, 0, 2){ UniqueId = 41293861, RuntimeId=3910 },
					},
					new List<Item>
					{
						new Item(693, -2, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 165 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-354, 0, 1){ UniqueId = 41293861, RuntimeId=3912 },
					},
					new List<Item>
					{
						new Item(693, -2, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 150 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(182, 0, 2){ UniqueId = 41293861, RuntimeId=7146 },
					},
					new List<Item>
					{
						new Item(336, 2, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 915 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-3, 0, 1){ UniqueId = 41293861, RuntimeId=4036 },
					},
					new List<Item>
					{
						new Item(336, 2, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 510 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-162, 0, 2){ UniqueId = 41293861, RuntimeId=7163 },
					},
					new List<Item>
					{
						new Item(2, 6, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 627 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-170, 0, 1){ UniqueId = 41293861, RuntimeId=4475 },
					},
					new List<Item>
					{
						new Item(2, 6, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 881 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(139, 0, 1){ UniqueId = 41293861, RuntimeId=1321 },
					},
					new List<Item>
					{
						new Item(2, 6, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 880 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-166, 0, 2){ UniqueId = 41293861, RuntimeId=7177 },
					},
					new List<Item>
					{
						new Item(2, 0, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 789 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-162, 0, 2){ UniqueId = 41293861, RuntimeId=7159 },
					},
					new List<Item>
					{
						new Item(242, 0, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 947 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-162, 0, 2){ UniqueId = 41293861, RuntimeId=7159 },
					},
					new List<Item>
					{
						new Item(412, 0, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 882 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-178, 0, 1){ UniqueId = 41293861, RuntimeId=4719 },
					},
					new List<Item>
					{
						new Item(242, 0, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 942 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-178, 0, 1){ UniqueId = 41293861, RuntimeId=4719 },
					},
					new List<Item>
					{
						new Item(412, 0, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 969 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(139, 0, 1){ UniqueId = 41293861, RuntimeId=1328 },
					},
					new List<Item>
					{
						new Item(242, 0, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 785 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(139, 0, 1){ UniqueId = 41293861, RuntimeId=1328 },
					},
					new List<Item>
					{
						new Item(412, 0, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 748 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(206, 0, 1){ UniqueId = 41293861, RuntimeId=4727 },
					},
					new List<Item>
					{
						new Item(242, 0, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 749 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-348, 0, 1){ UniqueId = 41293861, RuntimeId=4752 },
					},
					new List<Item>
					{
						new Item(681, -2, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 94 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-355, 0, 1){ UniqueId = 41293861, RuntimeId=4755 },
					},
					new List<Item>
					{
						new Item(681, -2, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 48 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-362, 0, 2){ UniqueId = 41293861, RuntimeId=4753 },
					},
					new List<Item>
					{
						new Item(681, -2, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 173 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-362, 0, 2){ UniqueId = 41293861, RuntimeId=4753 },
					},
					new List<Item>
					{
						new Item(695, -2, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 115 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-355, 0, 1){ UniqueId = 41293861, RuntimeId=4755 },
					},
					new List<Item>
					{
						new Item(695, -2, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 151 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-162, 0, 2){ UniqueId = 41293861, RuntimeId=7165 },
					},
					new List<Item>
					{
						new Item(2, 2, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 696 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-169, 0, 1){ UniqueId = 41293861, RuntimeId=4964 },
					},
					new List<Item>
					{
						new Item(2, 2, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 819 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(139, 0, 1){ UniqueId = 41293861, RuntimeId=1320 },
					},
					new List<Item>
					{
						new Item(2, 2, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 731 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(182, 0, 2){ UniqueId = 41293861, RuntimeId=7148 },
					},
					new List<Item>
					{
						new Item(96, 0, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 656 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-179, 0, 1){ UniqueId = 41293861, RuntimeId=5643 },
					},
					new List<Item>
					{
						new Item(96, 0, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 831 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(139, 0, 1){ UniqueId = 41293861, RuntimeId=1319 },
					},
					new List<Item>
					{
						new Item(96, 0, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 492 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-166, 0, 2){ UniqueId = 41293861, RuntimeId=7175 },
					},
					new List<Item>
					{
						new Item(196, 2, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 619 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-175, 0, 1){ UniqueId = 41293861, RuntimeId=5651 },
					},
					new List<Item>
					{
						new Item(196, 2, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 396 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(139, 0, 1){ UniqueId = 41293861, RuntimeId=1326 },
					},
					new List<Item>
					{
						new Item(196, 2, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 589 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(44, 0, 2){ UniqueId = 41293861, RuntimeId=7134 },
					},
					new List<Item>
					{
						new Item(224, 0, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 431 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(114, 0, 1){ UniqueId = 41293861, RuntimeId=5663 },
					},
					new List<Item>
					{
						new Item(224, 0, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 837 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(139, 0, 1){ UniqueId = 41293861, RuntimeId=1327 },
					},
					new List<Item>
					{
						new Item(224, 0, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 801 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-350, 0, 1){ UniqueId = 41293861, RuntimeId=5728 },
					},
					new List<Item>
					{
						new Item(685, -2, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 30 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-364, 0, 2){ UniqueId = 41293861, RuntimeId=5729 },
					},
					new List<Item>
					{
						new Item(685, -2, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 171 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-357, 0, 1){ UniqueId = 41293861, RuntimeId=5731 },
					},
					new List<Item>
					{
						new Item(685, -2, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 83 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-364, 0, 2){ UniqueId = 41293861, RuntimeId=5729 },
					},
					new List<Item>
					{
						new Item(699, -2, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 100 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-357, 0, 1){ UniqueId = 41293861, RuntimeId=5731 },
					},
					new List<Item>
					{
						new Item(699, -2, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 39 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(1, 0, 1){ UniqueId = 41293861, RuntimeId=7090 },
					},
					new List<Item>
					{
						new Item(2, 10, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 700 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-162, 0, 2){ UniqueId = 41293861, RuntimeId=7161 },
					},
					new List<Item>
					{
						new Item(2, 10, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 414 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-162, 0, 2){ UniqueId = 41293861, RuntimeId=7161 },
					},
					new List<Item>
					{
						new Item(2, 12, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 894 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-174, 0, 1){ UniqueId = 41293861, RuntimeId=5787 },
					},
					new List<Item>
					{
						new Item(2, 10, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 730 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-174, 0, 1){ UniqueId = 41293861, RuntimeId=5787 },
					},
					new List<Item>
					{
						new Item(2, 12, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 386 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-235, 0, 1){ UniqueId = 41293861, RuntimeId=5795 },
					},
					new List<Item>
					{
						new Item(467, -2, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 337 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-284, 0, 2){ UniqueId = 41293861, RuntimeId=5801 },
					},
					new List<Item>
					{
						new Item(545, -2, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 261 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-275, 0, 1){ UniqueId = 41293861, RuntimeId=5803 },
					},
					new List<Item>
					{
						new Item(545, -2, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 356 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-278, 0, 1){ UniqueId = 41293861, RuntimeId=5811 },
					},
					new List<Item>
					{
						new Item(545, -2, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 351 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-274, 0, 1){ UniqueId = 41293861, RuntimeId=5973 },
					},
					new List<Item>
					{
						new Item(545, -2, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 319 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(1, 0, 1){ UniqueId = 41293861, RuntimeId=7088 },
					},
					new List<Item>
					{
						new Item(2, 6, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 845 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-162, 0, 2){ UniqueId = 41293861, RuntimeId=7164 },
					},
					new List<Item>
					{
						new Item(2, 6, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 824 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-162, 0, 2){ UniqueId = 41293861, RuntimeId=7164 },
					},
					new List<Item>
					{
						new Item(2, 8, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 746 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-173, 0, 1){ UniqueId = 41293861, RuntimeId=6351 },
					},
					new List<Item>
					{
						new Item(2, 6, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 767 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-173, 0, 1){ UniqueId = 41293861, RuntimeId=6351 },
					},
					new List<Item>
					{
						new Item(2, 8, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 998 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-291, 0, 1){ UniqueId = 41293861, RuntimeId=5798 },
					},
					new List<Item>
					{
						new Item(545, -2, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 233 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(1, 0, 1){ UniqueId = 41293861, RuntimeId=7086 },
					},
					new List<Item>
					{
						new Item(2, 2, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 590 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-162, 0, 2){ UniqueId = 41293861, RuntimeId=7166 },
					},
					new List<Item>
					{
						new Item(2, 2, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 973 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-162, 0, 2){ UniqueId = 41293861, RuntimeId=7166 },
					},
					new List<Item>
					{
						new Item(2, 4, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 557 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-172, 0, 1){ UniqueId = 41293861, RuntimeId=6359 },
					},
					new List<Item>
					{
						new Item(2, 2, 2){ UniqueId = 41293861, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 532 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-172, 0, 1){ UniqueId = 41293865, RuntimeId=6359 },
					},
					new List<Item>
					{
						new Item(2, 4, 2){ UniqueId = 41293865, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 816 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-293, 0, 2){ UniqueId = 41293865, RuntimeId=6004 },
					},
					new List<Item>
					{
						new Item(545, -2, 2){ UniqueId = 41293865, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 258 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-292, 0, 1){ UniqueId = 41293865, RuntimeId=6006 },
					},
					new List<Item>
					{
						new Item(545, -2, 2){ UniqueId = 41293865, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 283 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-297, 0, 1){ UniqueId = 41293865, RuntimeId=6014 },
					},
					new List<Item>
					{
						new Item(545, -2, 2){ UniqueId = 41293865, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 345 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(182, 0, 2){ UniqueId = 41293865, RuntimeId=7147 },
					},
					new List<Item>
					{
						new Item(336, 4, 2){ UniqueId = 41293865, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 529 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-4, 0, 1){ UniqueId = 41293865, RuntimeId=6414 },
					},
					new List<Item>
					{
						new Item(336, 4, 2){ UniqueId = 41293865, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 860 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(182, 0, 2){ UniqueId = 41293865, RuntimeId=7145 },
					},
					new List<Item>
					{
						new Item(336, 0, 2){ UniqueId = 41293865, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 399 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-2, 0, 1){ UniqueId = 41293865, RuntimeId=6422 },
					},
					new List<Item>
					{
						new Item(336, 0, 2){ UniqueId = 41293865, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 579 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(139, 0, 1){ UniqueId = 41293865, RuntimeId=1329 },
					},
					new List<Item>
					{
						new Item(336, 0, 2){ UniqueId = 41293865, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 779 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(201, 0, 1){ UniqueId = 41293865, RuntimeId=6500 },
					},
					new List<Item>
					{
						new Item(402, 0, 2){ UniqueId = 41293865, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 570 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(182, 0, 2){ UniqueId = 41293865, RuntimeId=7144 },
					},
					new List<Item>
					{
						new Item(402, 0, 2){ UniqueId = 41293865, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 600 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(203, 0, 1){ UniqueId = 41293865, RuntimeId=6510 },
					},
					new List<Item>
					{
						new Item(402, 0, 2){ UniqueId = 41293865, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 645 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-304, 0, 1){ UniqueId = 41293865, RuntimeId=6530 },
					},
					new List<Item>
					{
						new Item(310, 0, 2){ UniqueId = 41293865, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 367 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(155, 0, 1){ UniqueId = 41293865, RuntimeId=6519 },
					},
					new List<Item>
					{
						new Item(310, 0, 2){ UniqueId = 41293865, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 439 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(155, 0, 1){ UniqueId = 41293865, RuntimeId=6520 },
					},
					new List<Item>
					{
						new Item(310, 0, 2){ UniqueId = 41293865, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 814 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(44, 0, 2){ UniqueId = 41293865, RuntimeId=7133 },
					},
					new List<Item>
					{
						new Item(310, 0, 2){ UniqueId = 41293865, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 363 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(156, 0, 1){ UniqueId = 41293865, RuntimeId=6532 },
					},
					new List<Item>
					{
						new Item(310, 0, 2){ UniqueId = 41293865, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 904 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(182, 0, 2){ UniqueId = 41293865, RuntimeId=7150 },
					},
					new List<Item>
					{
						new Item(430, 0, 2){ UniqueId = 41293865, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 549 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-184, 0, 1){ UniqueId = 41293865, RuntimeId=6598 },
					},
					new List<Item>
					{
						new Item(430, 0, 2){ UniqueId = 41293865, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 463 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(139, 0, 1){ UniqueId = 41293865, RuntimeId=1331 },
					},
					new List<Item>
					{
						new Item(430, 0, 2){ UniqueId = 41293865, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 601 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(182, 0, 2){ UniqueId = 41293865, RuntimeId=7143 },
					},
					new List<Item>
					{
						new Item(358, 0, 2){ UniqueId = 41293865, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 438 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(179, 0, 1){ UniqueId = 41293865, RuntimeId=6608 },
					},
					new List<Item>
					{
						new Item(358, 0, 2){ UniqueId = 41293865, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 898 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(179, 0, 1){ UniqueId = 41293865, RuntimeId=6607 },
					},
					new List<Item>
					{
						new Item(358, 0, 2){ UniqueId = 41293865, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 962 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(180, 0, 1){ UniqueId = 41293865, RuntimeId=6610 },
					},
					new List<Item>
					{
						new Item(358, 0, 2){ UniqueId = 41293865, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 977 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(139, 0, 1){ UniqueId = 41293865, RuntimeId=1330 },
					},
					new List<Item>
					{
						new Item(358, 0, 2){ UniqueId = 41293865, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 525 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(44, 0, 2){ UniqueId = 41293865, RuntimeId=7128 },
					},
					new List<Item>
					{
						new Item(48, 0, 2){ UniqueId = 41293865, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 960 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(24, 0, 1){ UniqueId = 41293865, RuntimeId=6681 },
					},
					new List<Item>
					{
						new Item(48, 0, 2){ UniqueId = 41293865, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 635 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(24, 0, 1){ UniqueId = 41293865, RuntimeId=6680 },
					},
					new List<Item>
					{
						new Item(48, 0, 2){ UniqueId = 41293865, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 841 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(128, 0, 1){ UniqueId = 41293865, RuntimeId=6683 },
					},
					new List<Item>
					{
						new Item(48, 0, 2){ UniqueId = 41293865, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 678 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(139, 0, 1){ UniqueId = 41293865, RuntimeId=1323 },
					},
					new List<Item>
					{
						new Item(48, 0, 2){ UniqueId = 41293865, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 984 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-293, 0, 2){ UniqueId = 41293865, RuntimeId=6004 },
					},
					new List<Item>
					{
						new Item(581, -2, 2){ UniqueId = 41293865, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 228 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-284, 0, 2){ UniqueId = 41293865, RuntimeId=5801 },
					},
					new List<Item>
					{
						new Item(547, -2, 2){ UniqueId = 41293865, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 194 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(44, 0, 2){ UniqueId = 41293865, RuntimeId=7127 },
					},
					new List<Item>
					{
						new Item(365, 0, 2){ UniqueId = 41293865, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 817 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-166, 0, 2){ UniqueId = 41293865, RuntimeId=7176 },
					},
					new List<Item>
					{
						new Item(310, 6, 2){ UniqueId = 41293865, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 623 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-185, 0, 1){ UniqueId = 41293865, RuntimeId=6791 },
					},
					new List<Item>
					{
						new Item(310, 6, 2){ UniqueId = 41293865, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 916 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-162, 0, 2){ UniqueId = 41293865, RuntimeId=7160 },
					},
					new List<Item>
					{
						new Item(358, 6, 2){ UniqueId = 41293865, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 695 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-176, 0, 1){ UniqueId = 41293865, RuntimeId=6799 },
					},
					new List<Item>
					{
						new Item(358, 6, 2){ UniqueId = 41293865, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 803 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(182, 0, 2){ UniqueId = 41293865, RuntimeId=7149 },
					},
					new List<Item>
					{
						new Item(48, 6, 2){ UniqueId = 41293865, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 578 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-177, 0, 1){ UniqueId = 41293865, RuntimeId=6807 },
					},
					new List<Item>
					{
						new Item(48, 6, 2){ UniqueId = 41293865, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 499 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-292, 0, 1){ UniqueId = 41293865, RuntimeId=6006 },
					},
					new List<Item>
					{
						new Item(581, -2, 2){ UniqueId = 41293865, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 201 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-180, 0, 1){ UniqueId = 41293865, RuntimeId=5681 },
					},
					new List<Item>
					{
						new Item(2, 0, 2){ UniqueId = 41293865, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 888 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(98, 0, 1){ UniqueId = 41293865, RuntimeId=7193 },
					},
					new List<Item>
					{
						new Item(2, 0, 2){ UniqueId = 41293865, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 506 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(98, 0, 1){ UniqueId = 41293865, RuntimeId=7196 },
					},
					new List<Item>
					{
						new Item(2, 0, 2){ UniqueId = 41293865, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 628 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(44, 0, 2){ UniqueId = 41293865, RuntimeId=7132 },
					},
					new List<Item>
					{
						new Item(2, 0, 2){ UniqueId = 41293865, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 592 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(44, 0, 2){ UniqueId = 41293865, RuntimeId=7132 },
					},
					new List<Item>
					{
						new Item(196, 0, 2){ UniqueId = 41293865, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 681 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(109, 0, 1){ UniqueId = 41293865, RuntimeId=7091 },
					},
					new List<Item>
					{
						new Item(2, 0, 2){ UniqueId = 41293865, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 689 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(109, 0, 1){ UniqueId = 41293865, RuntimeId=7091 },
					},
					new List<Item>
					{
						new Item(196, 0, 2){ UniqueId = 41293865, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 483 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(139, 0, 1){ UniqueId = 41293865, RuntimeId=1325 },
					},
					new List<Item>
					{
						new Item(2, 0, 2){ UniqueId = 41293865, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 621 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(139, 0, 1){ UniqueId = 41293865, RuntimeId=1325 },
					},
					new List<Item>
					{
						new Item(196, 0, 2){ UniqueId = 41293865, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 563 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-297, 0, 1){ UniqueId = 41293865, RuntimeId=6014 },
					},
					new List<Item>
					{
						new Item(581, -2, 2){ UniqueId = 41293865, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 289 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-278, 0, 1){ UniqueId = 41293865, RuntimeId=5811 },
					},
					new List<Item>
					{
						new Item(547, -2, 2){ UniqueId = 41293865, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 200 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-351, 0, 1){ UniqueId = 41293865, RuntimeId=7590 },
					},
					new List<Item>
					{
						new Item(687, -2, 2){ UniqueId = 41293865, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 169 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-365, 0, 2){ UniqueId = 41293865, RuntimeId=7591 },
					},
					new List<Item>
					{
						new Item(687, -2, 2){ UniqueId = 41293865, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 128 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-358, 0, 1){ UniqueId = 41293865, RuntimeId=7593 },
					},
					new List<Item>
					{
						new Item(687, -2, 2){ UniqueId = 41293865, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 133 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-366, 0, 2){ UniqueId = 41293865, RuntimeId=7605 },
					},
					new List<Item>
					{
						new Item(689, -2, 2){ UniqueId = 41293865, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 44 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-365, 0, 2){ UniqueId = 41293865, RuntimeId=7591 },
					},
					new List<Item>
					{
						new Item(701, -2, 2){ UniqueId = 41293865, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 180 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-358, 0, 1){ UniqueId = 41293865, RuntimeId=7593 },
					},
					new List<Item>
					{
						new Item(701, -2, 2){ UniqueId = 41293865, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 104 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-352, 0, 1){ UniqueId = 41293865, RuntimeId=7604 },
					},
					new List<Item>
					{
						new Item(689, -2, 2){ UniqueId = 41293865, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 112 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-359, 0, 1){ UniqueId = 41293865, RuntimeId=7607 },
					},
					new List<Item>
					{
						new Item(689, -2, 2){ UniqueId = 41293865, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 29 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-366, 0, 2){ UniqueId = 41293865, RuntimeId=7605 },
					},
					new List<Item>
					{
						new Item(703, -2, 2){ UniqueId = 41293865, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 114 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-359, 0, 1){ UniqueId = 41293865, RuntimeId=7607 },
					},
					new List<Item>
					{
						new Item(703, -2, 2){ UniqueId = 41293865, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 86 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-447, 0, 1){ UniqueId = 41293865, RuntimeId=7618 },
					},
					new List<Item>
					{
						new Item(891, -2, 2){ UniqueId = 41293865, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 131 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-449, 0, 2){ UniqueId = 41293865, RuntimeId=7619 },
					},
					new List<Item>
					{
						new Item(891, -2, 2){ UniqueId = 41293865, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 101 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-448, 0, 1){ UniqueId = 41293865, RuntimeId=7621 },
					},
					new List<Item>
					{
						new Item(891, -2, 2){ UniqueId = 41293865, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 138 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-449, 0, 1){ UniqueId = 41293865, RuntimeId=7619 },
					},
					new List<Item>
					{
						new Item(893, -2, 2){ UniqueId = 41293865, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 89 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-448, 0, 1){ UniqueId = 41293865, RuntimeId=7621 },
					},
					new List<Item>
					{
						new Item(893, -2, 2){ UniqueId = 41293865, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 51 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-353, 0, 1){ UniqueId = 41293865, RuntimeId=7632 },
					},
					new List<Item>
					{
						new Item(691, -2, 2){ UniqueId = 41293865, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 157 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-367, 0, 2){ UniqueId = 41293865, RuntimeId=7633 },
					},
					new List<Item>
					{
						new Item(691, -2, 2){ UniqueId = 41293865, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 168 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-360, 0, 1){ UniqueId = 41293865, RuntimeId=7635 },
					},
					new List<Item>
					{
						new Item(691, -2, 2){ UniqueId = 41293865, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 179 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-367, 0, 2){ UniqueId = 41293865, RuntimeId=7633 },
					},
					new List<Item>
					{
						new Item(705, -2, 2){ UniqueId = 41293865, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 153 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-360, 0, 1){ UniqueId = 41293865, RuntimeId=7635 },
					},
					new List<Item>
					{
						new Item(705, -2, 2){ UniqueId = 41293865, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 95 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-349, 0, 1){ UniqueId = 41293865, RuntimeId=7646 },
					},
					new List<Item>
					{
						new Item(683, -2, 2){ UniqueId = 41293865, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 102 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-363, 0, 2){ UniqueId = 41293865, RuntimeId=7647 },
					},
					new List<Item>
					{
						new Item(683, -2, 2){ UniqueId = 41293865, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 127 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-356, 0, 1){ UniqueId = 41293865, RuntimeId=7649 },
					},
					new List<Item>
					{
						new Item(683, -2, 2){ UniqueId = 41293865, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 108 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-363, 0, 2){ UniqueId = 41293865, RuntimeId=7647 },
					},
					new List<Item>
					{
						new Item(697, -2, 2){ UniqueId = 41293865, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 122 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-356, 0, 1){ UniqueId = 41293865, RuntimeId=7649 },
					},
					new List<Item>
					{
						new Item(697, -2, 2){ UniqueId = 41293865, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 40 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-275, 0, 1){ UniqueId = 41293865, RuntimeId=5803 },
					},
					new List<Item>
					{
						new Item(547, -2, 2){ UniqueId = 41293865, RuntimeId=0 },
					}, "stonecutter"){ UniqueId = 259 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(47, 0, 1){ UniqueId = 41293865, RuntimeId=704 },
					},
					new Item[]
					{
						new Item(10, -2, 2){ UniqueId = 41293865, RuntimeId=0 },
						new Item(774, -2, 2){ UniqueId = 41293865, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293865, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293865, RuntimeId=0 },
						new Item(774, -2, 2){ UniqueId = 41293865, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293865, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293865, RuntimeId=0 },
						new Item(774, -2, 2){ UniqueId = 41293865, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293865, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1987 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(321, 0, 4){ UniqueId = 41293865, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(10, -2, 2){ UniqueId = 41293865, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293865, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293865, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1979 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(-140, 0, 1){ UniqueId = 41293865, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(10, 8, 2){ UniqueId = 41293865, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1990 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(-141, 0, 1){ UniqueId = 41293865, RuntimeId=356 },
					},
					new Item[]
					{
						new Item(10, 4, 2){ UniqueId = 41293865, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1992 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(-142, 0, 1){ UniqueId = 41293865, RuntimeId=3936 },
					},
					new Item[]
					{
						new Item(10, 10, 2){ UniqueId = 41293865, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1994 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(-143, 0, 1){ UniqueId = 41293865, RuntimeId=5184 },
					},
					new Item[]
					{
						new Item(10, 6, 2){ UniqueId = 41293865, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1996 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(-144, 0, 1){ UniqueId = 41293865, RuntimeId=6870 },
					},
					new Item[]
					{
						new Item(10, 2, 2){ UniqueId = 41293865, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1998 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(54, 0, 1){ UniqueId = 41293865, RuntimeId=1123 },
					},
					new Item[]
					{
						new Item(10, -2, 2){ UniqueId = 41293865, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293865, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293865, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293865, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293865, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293865, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293865, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293865, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1977 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(151, 0, 1){ UniqueId = 41293865, RuntimeId=4066 },
					},
					new Item[]
					{
						new Item(40, 0, 2){ UniqueId = 41293865, RuntimeId=0 },
						new Item(1048, -2, 2){ UniqueId = 41293865, RuntimeId=0 },
						new Item(316, -2, 2){ UniqueId = 41293865, RuntimeId=0 },
						new Item(40, 0, 2){ UniqueId = 41293865, RuntimeId=0 },
						new Item(1048, -2, 2){ UniqueId = 41293865, RuntimeId=0 },
						new Item(316, -2, 2){ UniqueId = 41293865, RuntimeId=0 },
						new Item(40, 0, 2){ UniqueId = 41293865, RuntimeId=0 },
						new Item(1048, -2, 2){ UniqueId = 41293865, RuntimeId=0 },
						new Item(316, -2, 2){ UniqueId = 41293865, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1980 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(509, 0, 3){ UniqueId = 41293869, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(858, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(606, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(656, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2010 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(509, 0, 3){ UniqueId = 41293869, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(858, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(604, 0, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(656, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2009 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(84, 0, 1){ UniqueId = 41293869, RuntimeId=5183 },
					},
					new Item[]
					{
						new Item(10, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(608, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1981 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(25, 0, 1){ UniqueId = 41293869, RuntimeId=5689 },
					},
					new Item[]
					{
						new Item(10, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(746, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1982 },
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(44, 0, 6){ UniqueId = 41293869, RuntimeId=7130 },
					},
					new Item[]
					{
						new Item(8, 0, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(8, 0, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(8, 0, 2){ UniqueId = 41293869, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2016 },
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(44, 0, 6){ UniqueId = 41293869, RuntimeId=7134 },
					},
					new Item[]
					{
						new Item(224, 0, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(224, 0, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(224, 0, 2){ UniqueId = 41293869, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2017 },
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(44, 0, 6){ UniqueId = 41293869, RuntimeId=7128 },
					},
					new Item[]
					{
						new Item(48, 0, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(48, 0, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(48, 0, 2){ UniqueId = 41293869, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2018 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(357, 0, 1){ UniqueId = 41293869, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(640, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1983 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(33, 0, 1){ UniqueId = 41293869, RuntimeId=5759 },
					},
					new Item[]
					{
						new Item(10, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(8, 0, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(8, 0, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(746, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(8, 0, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(8, 0, 2){ UniqueId = 41293869, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1984 },
				new ShapedRecipe(2, 1,
					new List<Item>
					{
						new Item(-150, 0, 1){ UniqueId = 41293869, RuntimeId=60 },
					},
					new Item[]
					{
						new Item(10, 8, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, 8, 2){ UniqueId = 41293869, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1991 },
				new ShapedRecipe(2, 1,
					new List<Item>
					{
						new Item(-151, 0, 1){ UniqueId = 41293869, RuntimeId=416 },
					},
					new Item[]
					{
						new Item(10, 4, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, 4, 2){ UniqueId = 41293869, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1993 },
				new ShapedRecipe(2, 1,
					new List<Item>
					{
						new Item(-152, 0, 1){ UniqueId = 41293869, RuntimeId=3996 },
					},
					new Item[]
					{
						new Item(10, 10, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, 10, 2){ UniqueId = 41293869, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1995 },
				new ShapedRecipe(2, 1,
					new List<Item>
					{
						new Item(-153, 0, 1){ UniqueId = 41293869, RuntimeId=5244 },
					},
					new Item[]
					{
						new Item(10, 6, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, 6, 2){ UniqueId = 41293869, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1997 },
				new ShapedRecipe(2, 1,
					new List<Item>
					{
						new Item(-154, 0, 1){ UniqueId = 41293869, RuntimeId=6930 },
					},
					new Item[]
					{
						new Item(10, 2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, 2, 2){ UniqueId = 41293869, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1999 },
				new ShapedRecipe(1, 2,
					new List<Item>
					{
						new Item(320, 0, 1){ UniqueId = 41293869, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(325, 0, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(325, 0, 2){ UniqueId = 41293869, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1985 },
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(-166, 0, 6){ UniqueId = 41293869, RuntimeId=7177 },
					},
					new Item[]
					{
						new Item(2, 0, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(2, 0, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(2, 0, 2){ UniqueId = 41293869, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2011 },
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(-166, 0, 6){ UniqueId = 41293869, RuntimeId=7175 },
					},
					new Item[]
					{
						new Item(196, 2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(196, 2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(196, 2, 2){ UniqueId = 41293869, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2013 },
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(44, 0, 6){ UniqueId = 41293869, RuntimeId=7131 },
					},
					new Item[]
					{
						new Item(90, 0, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(90, 0, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(90, 0, 2){ UniqueId = 41293869, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2015 },
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(44, 0, 6){ UniqueId = 41293869, RuntimeId=7132 },
					},
					new Item[]
					{
						new Item(196, 0, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(196, 0, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(196, 0, 2){ UniqueId = 41293869, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2014 },
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(44, 0, 6){ UniqueId = 41293869, RuntimeId=7127 },
					},
					new Item[]
					{
						new Item(365, 0, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(365, 0, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(365, 0, 2){ UniqueId = 41293869, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2012 },
				new ShapedRecipe(1, 2,
					new List<Item>
					{
						new Item(50, 0, 4){ UniqueId = 41293869, RuntimeId=7261 },
					},
					new Item[]
					{
						new Item(606, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2008 },
				new ShapedRecipe(1, 2,
					new List<Item>
					{
						new Item(50, 0, 4){ UniqueId = 41293869, RuntimeId=7261 },
					},
					new Item[]
					{
						new Item(604, 0, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2007 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(-145, 0, 2){ UniqueId = 41293869, RuntimeId=100 },
					},
					new Item[]
					{
						new Item(10, 8, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, 8, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, 8, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, 8, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, 8, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, 8, 2){ UniqueId = 41293869, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2002 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(-146, 0, 2){ UniqueId = 41293869, RuntimeId=456 },
					},
					new Item[]
					{
						new Item(10, 4, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, 4, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, 4, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, 4, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, 4, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, 4, 2){ UniqueId = 41293869, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2003 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(-147, 0, 2){ UniqueId = 41293869, RuntimeId=4020 },
					},
					new Item[]
					{
						new Item(10, 10, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, 10, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, 10, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, 10, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, 10, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, 10, 2){ UniqueId = 41293869, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2004 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(-148, 0, 2){ UniqueId = 41293869, RuntimeId=5284 },
					},
					new Item[]
					{
						new Item(10, 6, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, 6, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, 6, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, 6, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, 6, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, 6, 2){ UniqueId = 41293869, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2005 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(-149, 0, 2){ UniqueId = 41293869, RuntimeId=6970 },
					},
					new Item[]
					{
						new Item(10, 2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, 2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, 2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, 2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, 2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, 2, 2){ UniqueId = 41293869, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2006 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(96, 0, 2){ UniqueId = 41293869, RuntimeId=7267 },
					},
					new Item[]
					{
						new Item(10, 0, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, 0, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, 0, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, 0, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, 0, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, 0, 2){ UniqueId = 41293869, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2001 },
				new ShapedRecipe(1, 3,
					new List<Item>
					{
						new Item(131, 0, 2){ UniqueId = 41293869, RuntimeId=7305 },
					},
					new Item[]
					{
						new Item(610, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2000 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(143, 0, 1){ UniqueId = 41293869, RuntimeId=7747 },
					},
					new Item[]
					{
						new Item(10, 0, 2){ UniqueId = 41293869, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1988 },
				new ShapedRecipe(2, 1,
					new List<Item>
					{
						new Item(72, 0, 1){ UniqueId = 41293869, RuntimeId=7791 },
					},
					new Item[]
					{
						new Item(10, 0, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, 0, 2){ UniqueId = 41293869, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1989 },
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(58, 0, 1){ UniqueId = 41293869, RuntimeId=3770 },
					},
					new Item[]
					{
						new Item(10, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1978 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(163, 0, 4){ UniqueId = 41293869, RuntimeId=76 },
					},
					new Item[]
					{
						new Item(10, 8, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, 8, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, 8, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(10, 8, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, 8, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(10, 8, 2){ UniqueId = 41293869, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2045 },
				new MultiRecipe() { Id = new UUID("d81aaeaf-e172-4440-9225-868df030d27b"), UniqueId = 2138 }, // d81aaeaf-e172-4440-9225-868df030d27b
				new MultiRecipe() { Id = new UUID("b5c5d105-75a2-4076-af2b-923ea2bf4bf0"), UniqueId = 2137 }, // b5c5d105-75a2-4076-af2b-923ea2bf4bf0
				new MultiRecipe() { Id = new UUID("00000000-0000-0000-0000-000000000002"), UniqueId = 2139 }, // 00000000-0000-0000-0000-000000000002
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(417, 0, 1){ UniqueId = 41293869, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(70, 0, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 0, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 0, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1629 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(417, 1, 1){ UniqueId = 41293869, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(70, 2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1630 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(417, 10, 1){ UniqueId = 41293869, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(70, 20, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 20, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 20, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1639 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(417, 11, 1){ UniqueId = 41293869, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(70, 22, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 22, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 22, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1640 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(417, 12, 1){ UniqueId = 41293869, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(70, 24, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 24, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 24, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1641 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(417, 13, 1){ UniqueId = 41293869, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(70, 26, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 26, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 26, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1642 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(417, 14, 1){ UniqueId = 41293869, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(70, 28, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 28, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 28, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1643 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(417, 15, 1){ UniqueId = 41293869, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(70, 30, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 30, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 30, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1644 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(417, 2, 1){ UniqueId = 41293869, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(70, 4, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 4, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 4, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1631 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(417, 3, 1){ UniqueId = 41293869, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(70, 6, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 6, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 6, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1632 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(417, 4, 1){ UniqueId = 41293869, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(70, 8, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 8, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 8, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1633 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(417, 5, 1){ UniqueId = 41293869, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(70, 10, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 10, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 10, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1634 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(417, 6, 1){ UniqueId = 41293869, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(70, 12, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 12, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 12, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1635 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(417, 7, 1){ UniqueId = 41293869, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(70, 14, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 14, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 14, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1636 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(417, 8, 1){ UniqueId = 41293869, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(70, 16, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 16, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 16, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1637 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(417, 9, 1){ UniqueId = 41293869, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(70, 18, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 18, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 18, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1638 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(417, 0, 1){ UniqueId = 41293869, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(70, 0, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 0, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 0, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1661 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(417, 1, 1){ UniqueId = 41293869, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(70, 2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1662 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(417, 10, 1){ UniqueId = 41293869, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(70, 20, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 20, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 20, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1671 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(417, 11, 1){ UniqueId = 41293869, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(70, 22, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 22, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 22, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1672 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(417, 12, 1){ UniqueId = 41293869, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(70, 24, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 24, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 24, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1673 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(417, 13, 1){ UniqueId = 41293869, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(70, 26, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 26, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 26, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1674 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(417, 14, 1){ UniqueId = 41293869, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(70, 28, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 28, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 28, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1675 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(417, 15, 1){ UniqueId = 41293869, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(70, 30, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 30, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 30, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1676 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(417, 2, 1){ UniqueId = 41293869, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(70, 4, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 4, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 4, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1663 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(417, 3, 1){ UniqueId = 41293869, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(70, 6, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 6, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 6, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1664 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(417, 4, 1){ UniqueId = 41293869, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(70, 8, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 8, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 8, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1665 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(417, 5, 1){ UniqueId = 41293869, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(70, 10, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 10, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 10, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1666 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(417, 6, 1){ UniqueId = 41293869, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(70, 12, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 12, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 12, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1667 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(417, 7, 1){ UniqueId = 41293869, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(70, 14, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 14, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 14, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1668 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(417, 8, 1){ UniqueId = 41293869, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(70, 16, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 16, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 16, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1669 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(417, 9, 1){ UniqueId = 41293869, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(70, 18, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 18, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 18, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1670 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(417, 0, 1){ UniqueId = 41293869, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(70, 0, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 0, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 0, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1645 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(417, 1, 1){ UniqueId = 41293869, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(70, 2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1646 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(417, 10, 1){ UniqueId = 41293869, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(70, 20, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 20, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 20, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1655 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(417, 11, 1){ UniqueId = 41293869, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(70, 22, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 22, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 22, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1656 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(417, 12, 1){ UniqueId = 41293869, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(70, 24, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 24, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 24, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1657 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(417, 13, 1){ UniqueId = 41293869, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(70, 26, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 26, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 26, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1658 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(417, 14, 1){ UniqueId = 41293869, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(70, 28, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 28, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 28, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1659 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(417, 15, 1){ UniqueId = 41293869, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(70, 30, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 30, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 30, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1660 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(417, 2, 1){ UniqueId = 41293869, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(70, 4, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 4, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 4, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1647 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(417, 3, 1){ UniqueId = 41293869, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(70, 6, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 6, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 6, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1648 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(417, 4, 1){ UniqueId = 41293869, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(70, 8, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 8, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 8, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1649 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(417, 5, 1){ UniqueId = 41293869, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(70, 10, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 10, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 10, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1650 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(417, 6, 1){ UniqueId = 41293869, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(70, 12, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 12, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(70, 12, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293869, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1651 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(417, 7, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(70, 14, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(70, 14, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(70, 14, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1652 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(417, 8, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(70, 16, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(70, 16, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(70, 16, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1653 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(417, 9, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(70, 18, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(70, 18, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(70, 18, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1654 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 15, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 28, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(826, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1962 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 15, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 10, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(826, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1971 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 15, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 8, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(826, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1972 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 15, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 6, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(826, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1973 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 15, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 4, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(826, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1974 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 15, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 2, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(826, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1975 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 15, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 0, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(826, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1976 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 15, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 26, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(826, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1963 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 15, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 24, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(826, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1964 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 15, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 22, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(826, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1965 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 15, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 20, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(826, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1966 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 15, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 18, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(826, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1967 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 15, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 16, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(826, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1968 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 15, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 14, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(826, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1969 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 15, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 12, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(826, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1970 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 5, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 30, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(810, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1812 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 5, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 28, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(810, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1813 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 5, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 8, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(810, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1822 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 5, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 6, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(810, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1823 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 5, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 4, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(810, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1824 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 5, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 2, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(810, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1825 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 5, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 0, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(810, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1826 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 5, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 26, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(810, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1814 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 5, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 24, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(810, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1815 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 5, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 22, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(810, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1816 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 5, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 20, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(810, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1817 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 5, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 18, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(810, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1818 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 5, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 16, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(810, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1819 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 5, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 14, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(810, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1820 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 5, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 12, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(810, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1821 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 4, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 30, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(812, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1797 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 4, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 28, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(812, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1798 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 4, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 10, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(812, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1807 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 4, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 6, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(812, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1808 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 4, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 4, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(812, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1809 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 4, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 2, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(812, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1810 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 4, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 0, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(812, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1811 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 4, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 26, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(812, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1799 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 4, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 24, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(812, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1800 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 4, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 22, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(812, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1801 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 4, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 20, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(812, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1802 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 4, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 18, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(812, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1803 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 4, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 16, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(812, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1804 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 4, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 14, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(812, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1805 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 4, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 12, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(812, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1806 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 3, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 30, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(814, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1782 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 3, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 28, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(814, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1783 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 3, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 10, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(814, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1792 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 3, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 8, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(814, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1793 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 3, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 4, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(814, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1794 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 3, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 2, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(814, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1795 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 3, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 0, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(814, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1796 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 3, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 26, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(814, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1784 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 3, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 24, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(814, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1785 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 3, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 22, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(814, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1786 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 3, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 20, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(814, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1787 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 3, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 18, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(814, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1788 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 3, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 16, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(814, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1789 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 3, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 14, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(814, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1790 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 3, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 12, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(814, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1791 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 2, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 30, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(816, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1767 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 2, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 28, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(816, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1768 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 2, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 10, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(816, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1777 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 2, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 8, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(816, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1778 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 2, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 6, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(816, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1779 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 2, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 2, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(816, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1780 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 2, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 0, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(816, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1781 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 2, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 26, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(816, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1769 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 2, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 24, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(816, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1770 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 2, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 22, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(816, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1771 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 2, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 20, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(816, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1772 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 2, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 18, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(816, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1773 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 2, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 16, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(816, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1774 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 2, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 14, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(816, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1775 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 2, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 12, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(816, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1776 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 1, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 30, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(818, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1752 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 1, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 28, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(818, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1753 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 1, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 10, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(818, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1762 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 1, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 8, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(818, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1763 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 1, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 6, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(818, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1764 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 1, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 4, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(818, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1765 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 1, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 0, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(818, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1766 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 1, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 26, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(818, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1754 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 1, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 24, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(818, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1755 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 1, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 22, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(818, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1756 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 1, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 20, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(818, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1757 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 1, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 18, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(818, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1758 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 1, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 16, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(818, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1759 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 1, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 14, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(818, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1760 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 1, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 12, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(818, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1761 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 0, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 30, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(822, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1737 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 0, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 28, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(822, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1738 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 0, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 10, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(822, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1747 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 0, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 8, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(822, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1748 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 0, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 6, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(822, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1749 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 0, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 4, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(822, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1750 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 0, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 2, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(822, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1751 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 0, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 26, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(822, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1739 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 0, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 24, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(822, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1740 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 0, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 22, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(822, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1741 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 0, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 20, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(822, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1742 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 0, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 18, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(822, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1743 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 0, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 16, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(822, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1744 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 0, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 14, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(822, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1745 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 0, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 12, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(822, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1746 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 15, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 28, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(790, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1722 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 15, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 10, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(790, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1731 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 15, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 8, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(790, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1732 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 15, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 6, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(790, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1733 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 15, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 4, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(790, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1734 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 15, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 2, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(790, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1735 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 15, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 0, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(790, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1736 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 15, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 26, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(790, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1723 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 15, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 24, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(790, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1724 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 15, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 22, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(790, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1725 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 15, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 20, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(790, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1726 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 15, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 18, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(790, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1727 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 15, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 16, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(790, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1728 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 15, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 14, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(790, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1729 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 15, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 12, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(790, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1730 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 12, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 30, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(796, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1707 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 12, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 28, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(796, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1708 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 12, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 10, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(796, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1716 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 12, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 8, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(796, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1717 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 12, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 6, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(796, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1718 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 12, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 4, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(796, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1719 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 12, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 2, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(796, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1720 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 12, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 0, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(796, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1721 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 12, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 26, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(796, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1709 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 12, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 22, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(796, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1710 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 12, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 20, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(796, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1711 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 12, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 18, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(796, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1712 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 12, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 16, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(796, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1713 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 12, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 14, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(796, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1714 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 12, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 12, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(796, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1715 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 11, 1){ UniqueId = 41293873, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 30, 2){ UniqueId = 41293873, RuntimeId=0 },
						new Item(798, -2, 2){ UniqueId = 41293873, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1692 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 11, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 28, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(798, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1693 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 11, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 10, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(798, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1701 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 11, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 8, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(798, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1702 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 11, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 6, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(798, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1703 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 11, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 4, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(798, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1704 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 11, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 2, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(798, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1705 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 11, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 0, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(798, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1706 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 11, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 26, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(798, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1694 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 11, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 24, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(798, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1695 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 11, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 20, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(798, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1696 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 11, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 18, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(798, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1697 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 11, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 16, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(798, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1698 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 11, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 14, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(798, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1699 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 11, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 12, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(798, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1700 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 0, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 30, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(820, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1677 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 0, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 28, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(820, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1678 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 0, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 10, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(820, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1687 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 0, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 8, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(820, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1688 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 0, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 6, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(820, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1689 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 0, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 4, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(820, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1690 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 0, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 2, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(820, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1691 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 0, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 26, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(820, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1679 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 0, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 24, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(820, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1680 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 0, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 22, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(820, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1681 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 0, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 20, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(820, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1682 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 0, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 18, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(820, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1683 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 0, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 16, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(820, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1684 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 0, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 14, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(820, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1685 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 0, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 12, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(820, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1686 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 14, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 30, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(792, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1947 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 14, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 10, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(792, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1956 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 14, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 8, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(792, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1957 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 14, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 6, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(792, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1958 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 14, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 4, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(792, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1959 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 14, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 2, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(792, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1960 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 14, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 0, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(792, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1961 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 14, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 26, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(792, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1948 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 14, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 24, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(792, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1949 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 14, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 22, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(792, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1950 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 14, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 20, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(792, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1951 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 14, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 18, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(792, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1952 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 14, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 16, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(792, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1953 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 14, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 14, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(792, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1954 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 14, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 12, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(792, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1955 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 13, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 30, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(794, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1932 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 13, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 28, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(794, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1933 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 13, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 10, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(794, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1941 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 13, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 8, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(794, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1942 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 13, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 6, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(794, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1943 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 13, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 4, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(794, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1944 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 13, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 2, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(794, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1945 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 13, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 0, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(794, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1946 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 13, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 24, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(794, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1934 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 13, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 22, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(794, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1935 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 13, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 20, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(794, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1936 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 13, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 18, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(794, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1937 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 13, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 16, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(794, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1938 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 13, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 14, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(794, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1939 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 13, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 12, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(794, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1940 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 12, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 30, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(824, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1917 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 12, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 28, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(824, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1918 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 12, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 10, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(824, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1926 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 12, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 8, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(824, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1927 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 12, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 6, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(824, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1928 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 12, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 4, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(824, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1929 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 12, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 2, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(824, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1930 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 12, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 0, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(824, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1931 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 12, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 26, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(824, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1919 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 12, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 22, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(824, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1920 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 12, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 20, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(824, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1921 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 12, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 18, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(824, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1922 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 12, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 16, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(824, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1923 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 12, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 14, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(824, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1924 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 12, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 12, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(824, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1925 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 11, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 30, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(828, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1902 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 11, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 28, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(828, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1903 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 11, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 10, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(828, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1911 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 11, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 8, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(828, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1912 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 11, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 6, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(828, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1913 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 11, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 4, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(828, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1914 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 11, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 2, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(828, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1915 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 11, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 0, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(828, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1916 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 11, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 26, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(828, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1904 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 11, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 24, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(828, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1905 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 11, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 20, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(828, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1906 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 11, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 18, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(828, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1907 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 11, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 16, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(828, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1908 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 11, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 14, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(828, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1909 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 11, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 12, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(828, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1910 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 10, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 30, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(800, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1887 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 10, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 28, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(800, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1888 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 10, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 10, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(800, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1896 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 10, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 8, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(800, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1897 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 10, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 6, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(800, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1898 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 10, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 4, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(800, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1899 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 10, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 2, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(800, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1900 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 10, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 0, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(800, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1901 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 10, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 26, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(800, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1889 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 10, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 24, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(800, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1890 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 10, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 22, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(800, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1891 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 10, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 18, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(800, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1892 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 10, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 16, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(800, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1893 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 10, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 14, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(800, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1894 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 10, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 12, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(800, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1895 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 9, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 30, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(802, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1872 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 9, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 28, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(802, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1873 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 9, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 10, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(802, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1881 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 9, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 8, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(802, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1882 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 9, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 6, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(802, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1883 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 9, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 4, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(802, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1884 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 9, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 2, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(802, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1885 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 9, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 0, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(802, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1886 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 9, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 26, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(802, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1874 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 9, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 24, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(802, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1875 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 9, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 22, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(802, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1876 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 9, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 20, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(802, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1877 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 9, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 16, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(802, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1878 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 9, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 14, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(802, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1879 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 9, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 12, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(802, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1880 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 8, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 30, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(804, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1857 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 8, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 28, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(804, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1858 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 8, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 10, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(804, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1866 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 8, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 8, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(804, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1867 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 8, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 6, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(804, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1868 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 8, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 4, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(804, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1869 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 8, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 2, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(804, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1870 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 8, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 0, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(804, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1871 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 8, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 26, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(804, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1859 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 8, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 24, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(804, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1860 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 8, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 22, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(804, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1861 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 8, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 20, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(804, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1862 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 8, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 18, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(804, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1863 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 8, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 14, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(804, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1864 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 8, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 12, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(804, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1865 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 7, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 30, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(806, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1842 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 7, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 28, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(806, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1843 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 7, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 10, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(806, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1851 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 7, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 8, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(806, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1852 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 7, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 6, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(806, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1853 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 7, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 4, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(806, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1854 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 7, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 2, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(806, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1855 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 7, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 0, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(806, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1856 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 7, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 26, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(806, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1844 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 7, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 24, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(806, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1845 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 7, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 22, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(806, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1846 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 7, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 20, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(806, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1847 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 7, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 18, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(806, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1848 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 7, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 16, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(806, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1849 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 7, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 12, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(806, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1850 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 6, 1){ UniqueId = 41293877, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 30, 2){ UniqueId = 41293877, RuntimeId=0 },
						new Item(808, -2, 2){ UniqueId = 41293877, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1827 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 6, 1){ UniqueId = 41293881, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 28, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(808, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1828 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 6, 1){ UniqueId = 41293881, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 10, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(808, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1836 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 6, 1){ UniqueId = 41293881, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 8, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(808, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1837 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 6, 1){ UniqueId = 41293881, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 6, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(808, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1838 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 6, 1){ UniqueId = 41293881, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 4, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(808, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1839 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 6, 1){ UniqueId = 41293881, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(808, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1840 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 6, 1){ UniqueId = 41293881, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 0, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(808, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1841 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 6, 1){ UniqueId = 41293881, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 26, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(808, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1829 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 6, 1){ UniqueId = 41293881, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 24, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(808, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1830 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 6, 1){ UniqueId = 41293881, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 22, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(808, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1831 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 6, 1){ UniqueId = 41293881, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 20, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(808, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1832 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 6, 1){ UniqueId = 41293881, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 18, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(808, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1833 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 6, 1){ UniqueId = 41293881, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 16, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(808, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1834 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(417, 6, 1){ UniqueId = 41293881, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(836, 14, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(808, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1835 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(135, 0, 4){ UniqueId = 41293881, RuntimeId=432 },
					},
					new Item[]
					{
						new Item(10, 4, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(10, 4, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(10, 4, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(10, 4, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(10, 4, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(10, 4, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2043 },
				new MultiRecipe() { Id = new UUID("d1ca6b84-338e-4f2f-9c6b-76cc8b4bd98d"), UniqueId = 2148 }, // d1ca6b84-338e-4f2f-9c6b-76cc8b4bd98d
				new ShapedRecipe(1, 2,
					new List<Item>
					{
						new Item(155, 0, 1){ UniqueId = 41293881, RuntimeId=6519 },
					},
					new Item[]
					{
						new Item(88, 12, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(88, 12, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2047 },
				new ShapedRecipe(1, 2,
					new List<Item>
					{
						new Item(98, 0, 1){ UniqueId = 41293881, RuntimeId=7196 },
					},
					new Item[]
					{
						new Item(88, 10, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(88, 10, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2050 },
				new MultiRecipe() { Id = new UUID("85939755-ba10-4d9d-a4cc-efb7a8e943c4"), UniqueId = 2140 }, // 85939755-ba10-4d9d-a4cc-efb7a8e943c4
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(164, 0, 4){ UniqueId = 41293881, RuntimeId=4012 },
					},
					new Item[]
					{
						new Item(10, 10, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(10, 10, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(10, 10, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(10, 10, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(10, 10, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(10, 10, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2046 },
				new MultiRecipe() { Id = new UUID("d392b075-4ba1-40ae-8789-af868d56f6ce"), UniqueId = 2142 }, // d392b075-4ba1-40ae-8789-af868d56f6ce
				new ShapedRecipe(1, 2,
					new List<Item>
					{
						new Item(179, 0, 1){ UniqueId = 41293881, RuntimeId=6607 },
					},
					new Item[]
					{
						new Item(364, 0, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(364, 0, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2048 },
				new ShapedRecipe(1, 2,
					new List<Item>
					{
						new Item(24, 0, 1){ UniqueId = 41293881, RuntimeId=6680 },
					},
					new Item[]
					{
						new Item(88, 2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(88, 2, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2049 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(136, 0, 4){ UniqueId = 41293881, RuntimeId=5260 },
					},
					new Item[]
					{
						new Item(10, 6, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(10, 6, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(10, 6, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(10, 6, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(10, 6, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(10, 6, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2044 },
				new ShapedRecipe(1, 2,
					new List<Item>
					{
						new Item(201, 0, 1){ UniqueId = 41293881, RuntimeId=6500 },
					},
					new Item[]
					{
						new Item(364, 2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(364, 2, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2051 },
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(-204, 0, 1){ UniqueId = 41293881, RuntimeId=5557 },
					},
					new Item[]
					{
						new Item(652, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(652, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2093 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(379, 0, 1){ UniqueId = 41293881, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(10, 8, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(10, 8, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(10, 8, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(618, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(10, 8, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(10, 8, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 986 },
				new ShapedRecipe(2, 3,
					new List<Item>
					{
						new Item(556, 0, 3){ UniqueId = 41293881, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(10, 8, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(10, 8, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(10, 8, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(10, 8, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(10, 8, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(10, 8, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 671 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(85, 0, 3){ UniqueId = 41293881, RuntimeId=4777 },
					},
					new Item[]
					{
						new Item(10, 8, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(10, 8, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(10, 8, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(10, 8, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 581 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(187, 0, 1){ UniqueId = 41293881, RuntimeId=44 },
					},
					new Item[]
					{
						new Item(640, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(10, 8, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(10, 8, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 593 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(5, 0, 4){ UniqueId = 41293881, RuntimeId=5774 },
					},
					new Item[]
					{
						new Item(324, 0, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 825 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(5, 0, 4){ UniqueId = 41293881, RuntimeId=5774 },
					},
					new Item[]
					{
						new Item(15, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 672 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(5, 0, 4){ UniqueId = 41293881, RuntimeId=5774 },
					},
					new Item[]
					{
						new Item(423, 24, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 487 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(5, 0, 4){ UniqueId = 41293881, RuntimeId=5774 },
					},
					new Item[]
					{
						new Item(423, 8, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 693 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(163, 0, 4){ UniqueId = 41293881, RuntimeId=76 },
					},
					new Item[]
					{
						new Item(10, 8, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(10, 8, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(10, 8, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(10, 8, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(10, 8, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(10, 8, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 851 },
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(-212, 0, 3){ UniqueId = 41293881, RuntimeId=7715 },
					},
					new Item[]
					{
						new Item(324, 0, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(324, 0, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(324, 0, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(324, 0, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1003 },
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(-212, 0, 3){ UniqueId = 41293881, RuntimeId=7721 },
					},
					new Item[]
					{
						new Item(15, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(15, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(15, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(15, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 736 },
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(158, 0, 6){ UniqueId = 41293881, RuntimeId=7811 },
					},
					new Item[]
					{
						new Item(10, 8, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(10, 8, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(10, 8, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 796 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(126, 0, 6){ UniqueId = 41293881, RuntimeId=122 },
					},
					new Item[]
					{
						new Item(610, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(152, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 905 },
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(-327, 0, 1){ UniqueId = 41293881, RuntimeId=136 },
					},
					new Item[]
					{
						new Item(1246, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(1246, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(1246, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(1246, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 158 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(1, 0, 2){ UniqueId = 41293881, RuntimeId=7089 },
					},
					new List<Item>
					{
						new Item(2, 6, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(8, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1001 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-171, 0, 4){ UniqueId = 41293881, RuntimeId=144 },
					},
					new Item[]
					{
						new Item(2, 10, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(2, 10, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(2, 10, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(2, 10, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(2, 10, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(2, 10, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 815 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(139, 0, 6){ UniqueId = 41293881, RuntimeId=1322 },
					},
					new Item[]
					{
						new Item(2, 10, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(2, 10, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(2, 10, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(2, 10, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(2, 10, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(2, 10, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 421 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(145, 0, 1){ UniqueId = 41293881, RuntimeId=152 },
					},
					new Item[]
					{
						new Item(84, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(84, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(84, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1000 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(552, 0, 1){ UniqueId = 41293881, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(640, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(88, 0, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 932 },
				new ShapedRecipe(1, 3,
					new List<Item>
					{
						new Item(301, 0, 4){ UniqueId = 41293881, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(712, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(654, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 655 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(585, 0, 1){ UniqueId = 41293881, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(772, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(90, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 897 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(582, 0, 1){ UniqueId = 41293881, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(772, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(1032, 8, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 963 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(581, 0, 1){ UniqueId = 41293881, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(772, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(76, 16, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 919 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(583, 0, 1){ UniqueId = 41293881, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(772, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(1032, 2, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 952 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(584, 0, 1){ UniqueId = 41293881, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(772, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(518, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 665 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(586, 0, 1){ UniqueId = 41293881, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(772, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(212, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 495 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-203, 0, 1){ UniqueId = 41293881, RuntimeId=201 },
					},
					new Item[]
					{
						new Item(640, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(316, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(316, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 406 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-203, 0, 1){ UniqueId = 41293881, RuntimeId=201 },
					},
					new Item[]
					{
						new Item(640, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(527, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(527, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 243 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-203, 0, 1){ UniqueId = 41293881, RuntimeId=201 },
					},
					new Item[]
					{
						new Item(640, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(529, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(529, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 355 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(515, 2, 1){ UniqueId = 41293881, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(1030, 2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(782, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 663 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(138, 0, 1){ UniqueId = 41293881, RuntimeId=217 },
					},
					new Item[]
					{
						new Item(40, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(98, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(1036, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(98, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(98, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 522 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-219, 0, 1){ UniqueId = 41293881, RuntimeId=260 },
					},
					new Item[]
					{
						new Item(10, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(1180, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(1180, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(1180, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 372 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-219, 0, 1){ UniqueId = 41293881, RuntimeId=260 },
					},
					new Item[]
					{
						new Item(483, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(1180, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(1180, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(1180, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 262 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-219, 0, 1){ UniqueId = 41293881, RuntimeId=260 },
					},
					new Item[]
					{
						new Item(485, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(1180, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(1180, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(1180, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 260 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(286, 0, 1){ UniqueId = 41293881, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(642, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(570, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(570, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(570, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(570, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(570, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(570, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 449 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(376, 0, 1){ UniqueId = 41293881, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(10, 4, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(10, 4, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(10, 4, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(618, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(10, 4, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(10, 4, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 920 },
				new ShapedRecipe(2, 3,
					new List<Item>
					{
						new Item(554, 0, 3){ UniqueId = 41293881, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(10, 4, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(10, 4, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(10, 4, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(10, 4, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(10, 4, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(10, 4, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 934 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(85, 0, 3){ UniqueId = 41293881, RuntimeId=4775 },
					},
					new Item[]
					{
						new Item(10, 4, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(10, 4, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(10, 4, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(10, 4, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 667 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(184, 0, 1){ UniqueId = 41293881, RuntimeId=400 },
					},
					new Item[]
					{
						new Item(640, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(10, 4, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(10, 4, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 792 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(5, 0, 4){ UniqueId = 41293881, RuntimeId=5772 },
					},
					new Item[]
					{
						new Item(34, 4, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 879 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(5, 0, 4){ UniqueId = 41293881, RuntimeId=5772 },
					},
					new Item[]
					{
						new Item(11, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 866 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(5, 0, 4){ UniqueId = 41293881, RuntimeId=5772 },
					},
					new Item[]
					{
						new Item(423, 20, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 869 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(5, 0, 4){ UniqueId = 41293881, RuntimeId=5772 },
					},
					new Item[]
					{
						new Item(423, 4, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 470 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(135, 0, 4){ UniqueId = 41293881, RuntimeId=432 },
					},
					new Item[]
					{
						new Item(10, 4, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(10, 4, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(10, 4, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(10, 4, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(10, 4, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(10, 4, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 922 },
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(-212, 0, 3){ UniqueId = 41293881, RuntimeId=7713 },
					},
					new Item[]
					{
						new Item(34, 4, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(34, 4, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(34, 4, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(34, 4, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 452 },
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(-212, 0, 3){ UniqueId = 41293881, RuntimeId=7719 },
					},
					new Item[]
					{
						new Item(11, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(11, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(11, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(11, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 895 },
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(158, 0, 6){ UniqueId = 41293881, RuntimeId=7809 },
					},
					new Item[]
					{
						new Item(10, 4, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(10, 4, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(10, 4, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 607 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(567, 0, 1){ UniqueId = 41293881, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(70, 30, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(70, 30, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(70, 30, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(70, 30, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(70, 30, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(70, 30, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 388 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-428, 0, 1){ UniqueId = 41293881, RuntimeId=478 },
					},
					new List<Item>
					{
						new Item(823, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(790, 0, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 16 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-428, 0, 1){ UniqueId = 41293881, RuntimeId=478 },
					},
					new List<Item>
					{
						new Item(823, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(826, 0, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 19 },
				new ShapedRecipe(2, 1,
					new List<Item>
					{
						new Item(171, 0, 3){ UniqueId = 41293881, RuntimeId=978 },
					},
					new Item[]
					{
						new Item(70, 30, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(70, 30, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 614 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(171, 0, 8){ UniqueId = 41293881, RuntimeId=978 },
					},
					new Item[]
					{
						new Item(342, 0, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(790, 0, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 465 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(237, 0, 8){ UniqueId = 41293881, RuntimeId=3674 },
					},
					new List<Item>
					{
						new Item(790, 0, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(24, 0, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(24, 0, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(24, 0, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(24, 0, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(26, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(26, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(26, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(26, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 480 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(237, 0, 8){ UniqueId = 41293881, RuntimeId=3674 },
					},
					new List<Item>
					{
						new Item(826, 0, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(24, 0, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(24, 0, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(24, 0, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(24, 0, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(26, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(26, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(26, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(26, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 828 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(395, 0, 1){ UniqueId = 41293881, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(826, 0, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 965 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(395, 0, 1){ UniqueId = 41293881, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(431, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 940 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(241, 0, 8){ UniqueId = 41293881, RuntimeId=7007 },
					},
					new Item[]
					{
						new Item(40, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(790, 0, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 618 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(241, 0, 8){ UniqueId = 41293881, RuntimeId=7007 },
					},
					new Item[]
					{
						new Item(40, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(826, 0, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 526 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(160, 0, 16){ UniqueId = 41293881, RuntimeId=7023 },
					},
					new Item[]
					{
						new Item(482, 30, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(482, 30, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(482, 30, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(482, 30, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(482, 30, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(482, 30, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 485 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(160, 0, 8){ UniqueId = 41293881, RuntimeId=7023 },
					},
					new Item[]
					{
						new Item(204, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(790, 0, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 929 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(159, 0, 8){ UniqueId = 41293881, RuntimeId=7039 },
					},
					new Item[]
					{
						new Item(344, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(790, 0, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 976 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(159, 0, 8){ UniqueId = 41293881, RuntimeId=7039 },
					},
					new Item[]
					{
						new Item(344, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(826, 0, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 862 },
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(-282, 0, 6){ UniqueId = 41293881, RuntimeId=497 },
					},
					new Item[]
					{
						new Item(545, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(545, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(545, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 350 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-276, 0, 4){ UniqueId = 41293881, RuntimeId=499 },
					},
					new Item[]
					{
						new Item(545, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(545, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(545, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(545, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(545, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(545, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 362 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(-277, 0, 6){ UniqueId = 41293881, RuntimeId=507 },
					},
					new Item[]
					{
						new Item(545, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(545, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(545, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(545, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(545, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(545, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 353 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-196, 0, 1){ UniqueId = 41293881, RuntimeId=669 },
					},
					new Item[]
					{
						new Item(610, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(365, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(122, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(365, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(365, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 534 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(429, 0, 2){ UniqueId = 41293881, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(846, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 594 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(567, 4, 1){ UniqueId = 41293881, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(70, 22, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(70, 22, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(70, 22, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(70, 22, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(70, 22, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(70, 22, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 751 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-424, 0, 1){ UniqueId = 41293881, RuntimeId=675 },
					},
					new List<Item>
					{
						new Item(823, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(798, 0, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 10 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-424, 0, 1){ UniqueId = 41293881, RuntimeId=675 },
					},
					new List<Item>
					{
						new Item(823, -2, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(828, 0, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 3 },
				new ShapedRecipe(2, 1,
					new List<Item>
					{
						new Item(171, 0, 3){ UniqueId = 41293881, RuntimeId=974 },
					},
					new Item[]
					{
						new Item(70, 22, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(70, 22, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 490 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(171, 0, 8){ UniqueId = 41293881, RuntimeId=974 },
					},
					new Item[]
					{
						new Item(342, 0, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(798, 0, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293881, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 762 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(237, 0, 8){ UniqueId = 41293885, RuntimeId=3670 },
					},
					new List<Item>
					{
						new Item(798, 0, 2){ UniqueId = 41293881, RuntimeId=0 },
						new Item(24, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(24, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(24, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(24, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(26, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(26, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(26, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(26, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 382 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(237, 0, 8){ UniqueId = 41293885, RuntimeId=3670 },
					},
					new List<Item>
					{
						new Item(828, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(24, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(24, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(24, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(24, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(26, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(26, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(26, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(26, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 988 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(399, 0, 1){ UniqueId = 41293885, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(76, 18, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 660 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(399, 0, 1){ UniqueId = 41293885, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(828, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 747 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-11, 0, 1){ UniqueId = 41293885, RuntimeId=691 },
					},
					new Item[]
					{
						new Item(348, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(348, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(348, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(348, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(348, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(348, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(348, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(348, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(348, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 769 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(241, 0, 8){ UniqueId = 41293885, RuntimeId=7003 },
					},
					new Item[]
					{
						new Item(40, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(798, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 925 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(241, 0, 8){ UniqueId = 41293885, RuntimeId=7003 },
					},
					new Item[]
					{
						new Item(40, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(828, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 566 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(160, 0, 16){ UniqueId = 41293885, RuntimeId=7019 },
					},
					new Item[]
					{
						new Item(482, 22, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(482, 22, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(482, 22, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(482, 22, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(482, 22, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(482, 22, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 548 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(160, 0, 8){ UniqueId = 41293885, RuntimeId=7019 },
					},
					new Item[]
					{
						new Item(204, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(798, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 538 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(159, 0, 8){ UniqueId = 41293885, RuntimeId=7035 },
					},
					new Item[]
					{
						new Item(344, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(798, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 668 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(159, 0, 8){ UniqueId = 41293885, RuntimeId=7035 },
					},
					new Item[]
					{
						new Item(344, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(828, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 553 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(375, 0, 1){ UniqueId = 41293885, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(10, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(10, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(10, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(618, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(10, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(10, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 743 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(216, 0, 1){ UniqueId = 41293885, RuntimeId=692 },
					},
					new Item[]
					{
						new Item(822, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(822, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(822, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(822, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(822, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(822, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(822, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(822, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(822, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 518 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(411, 0, 9){ UniqueId = 41293885, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(432, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 454 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(411, 0, 3){ UniqueId = 41293885, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(830, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 424 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(387, 0, 1){ UniqueId = 41293885, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(772, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(772, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(772, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(762, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 834 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(47, 0, 1){ UniqueId = 41293885, RuntimeId=704 },
					},
					new Item[]
					{
						new Item(483, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(774, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(774, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(774, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 322 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(47, 0, 1){ UniqueId = 41293885, RuntimeId=704 },
					},
					new Item[]
					{
						new Item(485, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(774, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(774, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(774, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 252 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(300, 0, 1){ UniqueId = 41293885, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(652, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(652, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(652, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 838 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(321, 0, 4){ UniqueId = 41293885, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(483, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 313 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(321, 0, 4){ UniqueId = 41293885, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(485, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 366 },
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(261, 0, 1){ UniqueId = 41293885, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(668, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(668, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(668, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 394 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(431, 0, 1){ UniqueId = 41293885, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(8, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(846, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(8, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(8, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 603 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(431, 0, 1){ UniqueId = 41293885, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(545, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(846, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(545, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(545, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 182 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(431, 0, 1){ UniqueId = 41293885, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(757, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(846, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(757, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(757, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 66 },
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(45, 0, 1){ UniqueId = 41293885, RuntimeId=875 },
					},
					new Item[]
					{
						new Item(766, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(766, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(766, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(766, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 745 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(108, 0, 4){ UniqueId = 41293885, RuntimeId=876 },
					},
					new Item[]
					{
						new Item(90, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(90, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(90, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(90, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(90, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(90, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 434 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(139, 0, 6){ UniqueId = 41293885, RuntimeId=1324 },
					},
					new Item[]
					{
						new Item(90, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(90, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(90, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(90, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(90, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(90, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 908 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(567, 3, 1){ UniqueId = 41293885, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(70, 24, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(70, 24, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(70, 24, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(70, 24, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(70, 24, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(70, 24, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 657 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-425, 0, 1){ UniqueId = 41293885, RuntimeId=884 },
					},
					new List<Item>
					{
						new Item(823, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(796, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 7 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-425, 0, 1){ UniqueId = 41293885, RuntimeId=884 },
					},
					new List<Item>
					{
						new Item(823, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(824, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 4 },
				new ShapedRecipe(2, 1,
					new List<Item>
					{
						new Item(171, 0, 3){ UniqueId = 41293885, RuntimeId=975 },
					},
					new Item[]
					{
						new Item(70, 24, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(70, 24, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 721 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(171, 0, 8){ UniqueId = 41293885, RuntimeId=975 },
					},
					new Item[]
					{
						new Item(342, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(796, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 840 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(237, 0, 8){ UniqueId = 41293885, RuntimeId=3671 },
					},
					new List<Item>
					{
						new Item(796, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(24, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(24, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(24, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(24, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(26, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(26, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(26, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(26, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 950 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(237, 0, 8){ UniqueId = 41293885, RuntimeId=3671 },
					},
					new List<Item>
					{
						new Item(824, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(24, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(24, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(24, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(24, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(26, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(26, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(26, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(26, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 836 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(398, 0, 1){ UniqueId = 41293885, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(824, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 686 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(241, 0, 8){ UniqueId = 41293885, RuntimeId=7004 },
					},
					new Item[]
					{
						new Item(40, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(796, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 666 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(241, 0, 8){ UniqueId = 41293885, RuntimeId=7004 },
					},
					new Item[]
					{
						new Item(40, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(824, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 535 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(160, 0, 16){ UniqueId = 41293885, RuntimeId=7020 },
					},
					new Item[]
					{
						new Item(482, 24, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(482, 24, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(482, 24, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(482, 24, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(482, 24, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(482, 24, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 460 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(160, 0, 8){ UniqueId = 41293885, RuntimeId=7020 },
					},
					new Item[]
					{
						new Item(204, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(796, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 638 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(159, 0, 8){ UniqueId = 41293885, RuntimeId=7036 },
					},
					new Item[]
					{
						new Item(344, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(796, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 674 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(159, 0, 8){ UniqueId = 41293885, RuntimeId=7036 },
					},
					new Item[]
					{
						new Item(344, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(824, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 661 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(360, 0, 1){ UniqueId = 41293885, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(610, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 685 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(417, 0, 1){ UniqueId = 41293885, RuntimeId=0 },
						new Item(360, 0, 3){ UniqueId = 41293885, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(722, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(832, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(668, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(722, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(780, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(668, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(722, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(832, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(668, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 709 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(588, 0, 1){ UniqueId = 41293885, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(34, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(604, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(34, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(34, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 276 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(588, 0, 1){ UniqueId = 41293885, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(34, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(606, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(34, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(34, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 728 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(588, 0, 1){ UniqueId = 41293885, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(324, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(606, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(324, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(324, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 558 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(588, 0, 1){ UniqueId = 41293885, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(15, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(606, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(15, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(15, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 404 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(588, 0, 1){ UniqueId = 41293885, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(11, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(606, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(11, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(11, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 756 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(588, 0, 1){ UniqueId = 41293885, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(17, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(606, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(17, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(17, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 861 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(588, 0, 1){ UniqueId = 41293885, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(13, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(606, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(13, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(13, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 718 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(588, 0, 1){ UniqueId = 41293885, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(19, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(606, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(19, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(19, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 605 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(588, 0, 1){ UniqueId = 41293885, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(9, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(606, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(9, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(9, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 630 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(588, 0, 1){ UniqueId = 41293885, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(423, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(606, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(423, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(423, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 768 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(588, 0, 1){ UniqueId = 41293885, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(449, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(604, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(449, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(449, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 277 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(588, 0, 1){ UniqueId = 41293885, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(324, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(604, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(324, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(324, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 205 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(588, 0, 1){ UniqueId = 41293885, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(15, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(604, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(15, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(15, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 321 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(588, 0, 1){ UniqueId = 41293885, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(11, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(604, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(11, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(11, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 282 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(588, 0, 1){ UniqueId = 41293885, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(479, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(604, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(479, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(479, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 314 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(588, 0, 1){ UniqueId = 41293885, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(17, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(604, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(17, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(17, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 290 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(588, 0, 1){ UniqueId = 41293885, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(13, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(604, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(13, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(13, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 212 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(588, 0, 1){ UniqueId = 41293885, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(19, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(604, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(19, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(19, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 263 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(588, 0, 1){ UniqueId = 41293885, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(9, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(604, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(9, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(9, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 361 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(588, 0, 1){ UniqueId = 41293885, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(481, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(604, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(481, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(481, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 300 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(588, 0, 1){ UniqueId = 41293885, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(451, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(604, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(451, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(451, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 240 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(588, 0, 1){ UniqueId = 41293885, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(423, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(604, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(423, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(423, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 218 },
				new ShapedRecipe(1, 2,
					new List<Item>
					{
						new Item(-412, 0, 1){ UniqueId = 41293885, RuntimeId=953 },
					},
					new Item[]
					{
						new Item(652, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(1180, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 15 },
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(517, 0, 1){ UniqueId = 41293885, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(784, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(558, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 675 },
				new ShapedRecipe(2, 3,
					new List<Item>
					{
						new Item(-200, 0, 1){ UniqueId = 41293885, RuntimeId=987 },
					},
					new Item[]
					{
						new Item(772, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(772, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 890 },
				new ShapedRecipe(2, 3,
					new List<Item>
					{
						new Item(-200, 0, 1){ UniqueId = 41293885, RuntimeId=987 },
					},
					new Item[]
					{
						new Item(772, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(772, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 234 },
				new ShapedRecipe(2, 3,
					new List<Item>
					{
						new Item(-200, 0, 1){ UniqueId = 41293885, RuntimeId=987 },
					},
					new Item[]
					{
						new Item(772, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(772, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 255 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(432, 0, 1){ UniqueId = 41293885, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(610, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 641 },
				new ShapedRecipe(1, 3,
					new List<Item>
					{
						new Item(617, 0, 1){ UniqueId = 41293885, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(1138, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(1138, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 352 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(54, 0, 1){ UniqueId = 41293885, RuntimeId=1123 },
					},
					new Item[]
					{
						new Item(483, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 211 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(54, 0, 1){ UniqueId = 41293885, RuntimeId=1123 },
					},
					new Item[]
					{
						new Item(485, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 368 },
				new ShapedRecipe(1, 2,
					new List<Item>
					{
						new Item(389, 0, 1){ UniqueId = 41293885, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(108, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(740, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 459 },
				new ShapedRecipe(1, 2,
					new List<Item>
					{
						new Item(-395, 0, 1){ UniqueId = 41293885, RuntimeId=1129 },
					},
					new Item[]
					{
						new Item(759, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(759, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 118 },
				new ShapedRecipe(1, 2,
					new List<Item>
					{
						new Item(-302, 0, 1){ UniqueId = 41293885, RuntimeId=1130 },
					},
					new Item[]
					{
						new Item(88, 14, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(88, 14, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 291 },
				new ShapedRecipe(1, 2,
					new List<Item>
					{
						new Item(-279, 0, 1){ UniqueId = 41293885, RuntimeId=1131 },
					},
					new Item[]
					{
						new Item(585, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(585, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 238 },
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(82, 0, 1){ UniqueId = 41293885, RuntimeId=1139 },
					},
					new Item[]
					{
						new Item(768, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(768, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(768, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(768, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 832 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(393, 0, 1){ UniqueId = 41293885, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(612, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(612, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(746, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(612, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(612, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 688 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(302, 0, 9){ UniqueId = 41293885, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(346, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 624 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(173, 0, 1){ UniqueId = 41293885, RuntimeId=1140 },
					},
					new Item[]
					{
						new Item(604, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(604, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(604, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(604, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(604, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(604, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(604, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(604, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(604, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 872 },
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(3, 0, 4){ UniqueId = 41293885, RuntimeId=4484 },
					},
					new Item[]
					{
						new Item(6, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(26, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(26, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(6, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 481 },
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(-380, 0, 6){ UniqueId = 41293885, RuntimeId=1145 },
					},
					new Item[]
					{
						new Item(757, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(757, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(757, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 152 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-381, 0, 4){ UniqueId = 41293885, RuntimeId=1147 },
					},
					new Item[]
					{
						new Item(757, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(757, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(757, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(757, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(757, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(757, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 147 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(-382, 0, 6){ UniqueId = 41293885, RuntimeId=1155 },
					},
					new Item[]
					{
						new Item(757, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(757, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(757, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(757, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(757, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(757, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 172 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(67, 0, 4){ UniqueId = 41293885, RuntimeId=7185 },
					},
					new Item[]
					{
						new Item(8, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(8, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(8, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(8, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(8, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(8, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 923 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(139, 0, 6){ UniqueId = 41293885, RuntimeId=1318 },
					},
					new Item[]
					{
						new Item(8, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(8, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(8, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(8, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(8, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(8, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 772 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(326, 0, 9){ UniqueId = 41293885, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(60, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 710 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(522, 0, 1){ UniqueId = 41293885, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(152, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(2, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(152, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(1048, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(2, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(152, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(2, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 694 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(391, 0, 1){ UniqueId = 41293885, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(746, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1006 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-213, 0, 1){ UniqueId = 41293885, RuntimeId=3634 },
					},
					new Item[]
					{
						new Item(316, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(316, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(316, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(316, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(316, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(316, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(316, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 653 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-213, 0, 1){ UniqueId = 41293885, RuntimeId=3634 },
					},
					new Item[]
					{
						new Item(527, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(527, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(527, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(527, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(527, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(527, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(527, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 341 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-213, 0, 1){ UniqueId = 41293885, RuntimeId=3634 },
					},
					new Item[]
					{
						new Item(529, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(529, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(529, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(529, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(529, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(529, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(529, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 360 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-157, 0, 1){ UniqueId = 41293885, RuntimeId=3675 },
					},
					new Item[]
					{
						new Item(1140, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(1140, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(1140, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(1140, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(1142, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(1140, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(1140, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(1140, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(1140, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 974 },
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(271, 0, 8){ UniqueId = 41293885, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(668, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(824, 0, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(668, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 503 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-340, 0, 1){ UniqueId = 41293885, RuntimeId=3676 },
					},
					new Item[]
					{
						new Item(1008, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(1008, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(1008, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(1008, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(1008, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(1008, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(1008, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(1008, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(1008, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 24 },
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(-347, 0, 4){ UniqueId = 41293885, RuntimeId=3909 },
					},
					new Item[]
					{
						new Item(679, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(679, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(679, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(679, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 42 },
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(-361, 0, 6){ UniqueId = 41293885, RuntimeId=3910 },
					},
					new Item[]
					{
						new Item(693, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(693, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(693, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 161 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-354, 0, 4){ UniqueId = 41293885, RuntimeId=3912 },
					},
					new Item[]
					{
						new Item(693, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(693, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(693, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(693, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(693, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(693, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 79 },
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(-348, 0, 4){ UniqueId = 41293885, RuntimeId=4752 },
					},
					new Item[]
					{
						new Item(681, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(681, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(681, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(681, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 162 },
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(-362, 0, 6){ UniqueId = 41293885, RuntimeId=4753 },
					},
					new Item[]
					{
						new Item(695, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(695, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(695, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 121 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-355, 0, 4){ UniqueId = 41293885, RuntimeId=4755 },
					},
					new Item[]
					{
						new Item(695, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(695, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(695, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(695, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(695, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(695, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 170 },
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(58, 0, 1){ UniqueId = 41293885, RuntimeId=3770 },
					},
					new Item[]
					{
						new Item(483, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293885, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 203 },
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(58, 0, 1){ UniqueId = 41293889, RuntimeId=3770 },
					},
					new Item[]
					{
						new Item(485, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 302 },
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(-350, 0, 4){ UniqueId = 41293889, RuntimeId=5728 },
					},
					new Item[]
					{
						new Item(685, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(685, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(685, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(685, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 31 },
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(-364, 0, 6){ UniqueId = 41293889, RuntimeId=5729 },
					},
					new Item[]
					{
						new Item(699, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(699, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(699, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 163 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-357, 0, 4){ UniqueId = 41293889, RuntimeId=5731 },
					},
					new Item[]
					{
						new Item(699, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(699, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(699, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(699, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(699, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(699, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 32 },
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(-351, 0, 4){ UniqueId = 41293889, RuntimeId=7590 },
					},
					new Item[]
					{
						new Item(687, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(687, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(687, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(687, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 92 },
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(-365, 0, 6){ UniqueId = 41293889, RuntimeId=7591 },
					},
					new Item[]
					{
						new Item(701, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(701, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(701, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 167 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-358, 0, 4){ UniqueId = 41293889, RuntimeId=7593 },
					},
					new Item[]
					{
						new Item(701, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(701, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(701, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(701, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(701, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(701, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 41 },
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(-352, 0, 4){ UniqueId = 41293889, RuntimeId=7604 },
					},
					new Item[]
					{
						new Item(689, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(689, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(689, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(689, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 58 },
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(-366, 0, 6){ UniqueId = 41293889, RuntimeId=7605 },
					},
					new Item[]
					{
						new Item(703, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(703, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(703, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 25 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-359, 0, 4){ UniqueId = 41293889, RuntimeId=7607 },
					},
					new Item[]
					{
						new Item(703, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(703, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(703, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(703, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(703, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(703, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 141 },
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(-447, 0, 4){ UniqueId = 41293889, RuntimeId=7618 },
					},
					new Item[]
					{
						new Item(891, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(891, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(891, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(891, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 71 },
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(-449, 0, 6){ UniqueId = 41293889, RuntimeId=7619 },
					},
					new Item[]
					{
						new Item(893, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(893, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(893, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 74 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-448, 0, 4){ UniqueId = 41293889, RuntimeId=7621 },
					},
					new Item[]
					{
						new Item(893, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(893, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(893, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(893, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(893, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(893, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 124 },
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(-353, 0, 4){ UniqueId = 41293889, RuntimeId=7632 },
					},
					new Item[]
					{
						new Item(691, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(691, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(691, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(691, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 117 },
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(-367, 0, 6){ UniqueId = 41293889, RuntimeId=7633 },
					},
					new Item[]
					{
						new Item(705, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(705, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(705, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 82 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-360, 0, 4){ UniqueId = 41293889, RuntimeId=7635 },
					},
					new Item[]
					{
						new Item(705, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(705, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(705, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(705, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(705, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(705, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 156 },
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(-349, 0, 4){ UniqueId = 41293889, RuntimeId=7646 },
					},
					new Item[]
					{
						new Item(683, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(683, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(683, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(683, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 149 },
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(-363, 0, 6){ UniqueId = 41293889, RuntimeId=7647 },
					},
					new Item[]
					{
						new Item(697, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(697, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(697, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 119 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-356, 0, 4){ UniqueId = 41293889, RuntimeId=7649 },
					},
					new Item[]
					{
						new Item(697, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(697, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(697, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(697, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(697, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(697, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 105 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(-260, 0, 1){ UniqueId = 41293889, RuntimeId=3771 },
					},
					new Item[]
					{
						new Item(483, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 244 },
				new ShapedRecipe(2, 3,
					new List<Item>
					{
						new Item(614, 0, 3){ UniqueId = 41293889, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(483, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 294 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(-256, 0, 3){ UniqueId = 41293889, RuntimeId=3817 },
					},
					new Item[]
					{
						new Item(483, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 222 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(-258, 0, 1){ UniqueId = 41293889, RuntimeId=3818 },
					},
					new Item[]
					{
						new Item(640, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 331 },
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(-299, 0, 3){ UniqueId = 41293889, RuntimeId=3835 },
					},
					new Item[]
					{
						new Item(449, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(449, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(449, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(449, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 330 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(-242, 0, 4){ UniqueId = 41293889, RuntimeId=3839 },
					},
					new Item[]
					{
						new Item(449, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 257 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(-242, 0, 4){ UniqueId = 41293889, RuntimeId=3839 },
					},
					new Item[]
					{
						new Item(597, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 340 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(-242, 0, 4){ UniqueId = 41293889, RuntimeId=3839 },
					},
					new Item[]
					{
						new Item(599, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 305 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(-242, 0, 4){ UniqueId = 41293889, RuntimeId=3839 },
					},
					new Item[]
					{
						new Item(479, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 333 },
				new ShapedRecipe(2, 1,
					new List<Item>
					{
						new Item(-262, 0, 1){ UniqueId = 41293889, RuntimeId=3840 },
					},
					new Item[]
					{
						new Item(483, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 230 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(612, 0, 3){ UniqueId = 41293889, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(483, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 227 },
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(-264, 0, 6){ UniqueId = 41293889, RuntimeId=3857 },
					},
					new Item[]
					{
						new Item(483, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 316 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-254, 0, 4){ UniqueId = 41293889, RuntimeId=3859 },
					},
					new Item[]
					{
						new Item(483, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 206 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(-246, 0, 2){ UniqueId = 41293889, RuntimeId=3886 },
					},
					new Item[]
					{
						new Item(483, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 278 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(575, 0, 1){ UniqueId = 41293889, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(640, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(652, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(262, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(652, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 697 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(567, 6, 1){ UniqueId = 41293889, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(70, 18, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(70, 18, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(70, 18, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(70, 18, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(70, 18, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(70, 18, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 631 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-422, 0, 1){ UniqueId = 41293889, RuntimeId=3920 },
					},
					new List<Item>
					{
						new Item(823, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(802, 0, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 5 },
				new ShapedRecipe(2, 1,
					new List<Item>
					{
						new Item(171, 0, 3){ UniqueId = 41293889, RuntimeId=972 },
					},
					new Item[]
					{
						new Item(70, 18, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(70, 18, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 395 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(171, 0, 8){ UniqueId = 41293889, RuntimeId=972 },
					},
					new Item[]
					{
						new Item(342, 0, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(802, 0, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 588 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(237, 0, 8){ UniqueId = 41293889, RuntimeId=3668 },
					},
					new List<Item>
					{
						new Item(802, 0, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(24, 0, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(24, 0, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(24, 0, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(24, 0, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(26, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(26, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(26, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(26, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 405 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(401, 0, 2){ UniqueId = 41293889, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(798, 0, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(794, 0, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 996 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(401, 0, 2){ UniqueId = 41293889, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(828, 0, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(794, 0, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 426 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(241, 0, 8){ UniqueId = 41293889, RuntimeId=7001 },
					},
					new Item[]
					{
						new Item(40, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(802, 0, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 531 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(160, 0, 16){ UniqueId = 41293889, RuntimeId=7017 },
					},
					new Item[]
					{
						new Item(482, 18, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(482, 18, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(482, 18, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(482, 18, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(482, 18, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(482, 18, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 765 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(160, 0, 8){ UniqueId = 41293889, RuntimeId=7017 },
					},
					new Item[]
					{
						new Item(204, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(802, 0, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 616 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(159, 0, 8){ UniqueId = 41293889, RuntimeId=7033 },
					},
					new Item[]
					{
						new Item(344, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(802, 0, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 788 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(380, 0, 1){ UniqueId = 41293889, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(10, 10, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(10, 10, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(10, 10, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(618, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(10, 10, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(10, 10, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 402 },
				new ShapedRecipe(2, 3,
					new List<Item>
					{
						new Item(557, 0, 3){ UniqueId = 41293889, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(10, 10, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(10, 10, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(10, 10, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(10, 10, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(10, 10, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(10, 10, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 412 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(85, 0, 3){ UniqueId = 41293889, RuntimeId=4778 },
					},
					new Item[]
					{
						new Item(10, 10, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(10, 10, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(10, 10, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(10, 10, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 651 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(186, 0, 1){ UniqueId = 41293889, RuntimeId=3980 },
					},
					new Item[]
					{
						new Item(640, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(10, 10, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(10, 10, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 536 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(5, 0, 4){ UniqueId = 41293889, RuntimeId=5775 },
					},
					new Item[]
					{
						new Item(324, 2, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 980 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(5, 0, 4){ UniqueId = 41293889, RuntimeId=5775 },
					},
					new Item[]
					{
						new Item(17, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 909 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(5, 0, 4){ UniqueId = 41293889, RuntimeId=5775 },
					},
					new Item[]
					{
						new Item(423, 26, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 729 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(5, 0, 4){ UniqueId = 41293889, RuntimeId=5775 },
					},
					new Item[]
					{
						new Item(423, 10, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 805 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(164, 0, 4){ UniqueId = 41293889, RuntimeId=4012 },
					},
					new Item[]
					{
						new Item(10, 10, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(10, 10, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(10, 10, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(10, 10, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(10, 10, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(10, 10, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 456 },
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(-212, 0, 3){ UniqueId = 41293889, RuntimeId=7716 },
					},
					new Item[]
					{
						new Item(324, 2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(324, 2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(324, 2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(324, 2, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 567 },
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(-212, 0, 3){ UniqueId = 41293889, RuntimeId=7722 },
					},
					new Item[]
					{
						new Item(17, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(17, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(17, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(17, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 793 },
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(158, 0, 6){ UniqueId = 41293889, RuntimeId=7812 },
					},
					new Item[]
					{
						new Item(10, 10, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(10, 10, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(10, 10, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 389 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(168, 0, 1){ UniqueId = 41293889, RuntimeId=6412 },
					},
					new Item[]
					{
						new Item(1130, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(1130, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(1130, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(1130, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(790, 0, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(1130, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(1130, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(1130, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(1130, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 968 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(168, 0, 1){ UniqueId = 41293889, RuntimeId=6412 },
					},
					new Item[]
					{
						new Item(1130, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(1130, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(1130, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(1130, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(826, 0, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(1130, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(1130, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(1130, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(1130, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 761 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(151, 0, 1){ UniqueId = 41293889, RuntimeId=4066 },
					},
					new Item[]
					{
						new Item(40, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(1048, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(527, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(1048, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(527, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(1048, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(527, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 329 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(151, 0, 1){ UniqueId = 41293889, RuntimeId=4066 },
					},
					new Item[]
					{
						new Item(40, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(1048, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(529, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(1048, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(529, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(1048, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(529, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 245 },
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(-392, 0, 6){ UniqueId = 41293889, RuntimeId=4104 },
					},
					new Item[]
					{
						new Item(781, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(781, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(781, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 61 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-393, 0, 4){ UniqueId = 41293889, RuntimeId=4106 },
					},
					new Item[]
					{
						new Item(781, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(781, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(781, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(781, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(781, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(781, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 109 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(-394, 0, 6){ UniqueId = 41293889, RuntimeId=4114 },
					},
					new Item[]
					{
						new Item(781, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(781, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(781, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(781, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(781, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(781, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 80 },
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(-391, 0, 4){ UniqueId = 41293889, RuntimeId=4276 },
					},
					new Item[]
					{
						new Item(765, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(765, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(765, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(765, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 65 },
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(-388, 0, 6){ UniqueId = 41293889, RuntimeId=4287 },
					},
					new Item[]
					{
						new Item(773, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(773, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(773, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 176 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-389, 0, 4){ UniqueId = 41293889, RuntimeId=4289 },
					},
					new Item[]
					{
						new Item(773, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(773, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(773, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(773, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(773, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(773, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 139 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(-390, 0, 6){ UniqueId = 41293889, RuntimeId=4297 },
					},
					new Item[]
					{
						new Item(773, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(773, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(773, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(773, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(773, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(773, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 99 },
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(-387, 0, 4){ UniqueId = 41293889, RuntimeId=4459 },
					},
					new Item[]
					{
						new Item(781, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(781, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(781, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(781, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 155 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(28, 0, 6){ UniqueId = 41293889, RuntimeId=4461 },
					},
					new Item[]
					{
						new Item(610, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(140, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(746, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 418 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(304, 0, 9){ UniqueId = 41293889, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(114, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 482 },
				new ShapedRecipe(2, 3,
					new List<Item>
					{
						new Item(319, 0, 1){ UniqueId = 41293889, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(608, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(608, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(608, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 397 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(57, 0, 1){ UniqueId = 41293889, RuntimeId=4473 },
					},
					new Item[]
					{
						new Item(608, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(608, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(608, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(608, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(608, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(608, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(608, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(608, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(608, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 644 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(350, 0, 1){ UniqueId = 41293889, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(608, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(608, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(608, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(608, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 508 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(348, 0, 1){ UniqueId = 41293889, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(608, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(608, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(608, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(608, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(608, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(608, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(608, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(608, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 924 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(347, 0, 1){ UniqueId = 41293889, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(608, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(608, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(608, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(608, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(608, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 946 },
				new ShapedRecipe(2, 3,
					new List<Item>
					{
						new Item(332, 0, 1){ UniqueId = 41293889, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(608, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(608, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 477 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(349, 0, 1){ UniqueId = 41293889, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(608, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(608, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(608, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(608, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(608, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(608, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(608, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 703 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(318, 0, 1){ UniqueId = 41293889, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(608, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(608, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(608, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 702 },
				new ShapedRecipe(1, 3,
					new List<Item>
					{
						new Item(317, 0, 1){ UniqueId = 41293889, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(608, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 379 },
				new ShapedRecipe(1, 3,
					new List<Item>
					{
						new Item(316, 0, 1){ UniqueId = 41293889, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(608, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(608, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 450 },
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(1, 0, 2){ UniqueId = 41293889, RuntimeId=7087 },
					},
					new Item[]
					{
						new Item(8, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(1048, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(1048, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(8, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 776 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-170, 0, 4){ UniqueId = 41293889, RuntimeId=4475 },
					},
					new Item[]
					{
						new Item(2, 6, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(2, 6, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(2, 6, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(2, 6, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(2, 6, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(2, 6, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 568 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(139, 0, 6){ UniqueId = 41293889, RuntimeId=1321 },
					},
					new Item[]
					{
						new Item(2, 6, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(2, 6, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(2, 6, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(2, 6, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(2, 6, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(2, 6, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 494 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(23, 0, 1){ UniqueId = 41293889, RuntimeId=4489 },
					},
					new Item[]
					{
						new Item(8, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(8, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(8, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(8, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(600, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(746, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(8, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(8, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(8, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 544 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(270, 0, 9){ UniqueId = 41293889, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(277, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 560 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-139, 0, 1){ UniqueId = 41293889, RuntimeId=4583 },
					},
					new Item[]
					{
						new Item(540, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(540, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(540, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(540, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(540, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(540, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(540, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(540, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(540, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 849 },
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(-317, 0, 1){ UniqueId = 41293889, RuntimeId=4584 },
					},
					new Item[]
					{
						new Item(615, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(615, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(615, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(615, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 159 },
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(-317, 0, 1){ UniqueId = 41293889, RuntimeId=4584 },
					},
					new Item[]
					{
						new Item(615, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(615, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(615, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(615, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 63 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(125, 0, 1){ UniqueId = 41293889, RuntimeId=4588 },
					},
					new Item[]
					{
						new Item(8, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(8, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(8, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(8, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(746, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(8, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(8, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(8, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 659 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(512, 0, 9){ UniqueId = 41293889, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(266, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 820 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(133, 0, 1){ UniqueId = 41293889, RuntimeId=4716 },
					},
					new Item[]
					{
						new Item(1024, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(1024, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(1024, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(1024, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(1024, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(1024, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(1024, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(1024, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(1024, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 699 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(515, 2, 1){ UniqueId = 41293889, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(1030, 0, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(782, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 715 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(116, 0, 1){ UniqueId = 41293889, RuntimeId=4718 },
					},
					new Item[]
					{
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(608, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(98, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(774, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(98, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(98, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(608, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(98, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 497 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-178, 0, 4){ UniqueId = 41293889, RuntimeId=4719 },
					},
					new Item[]
					{
						new Item(412, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(412, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(412, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(412, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(412, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(412, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 722 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(139, 0, 6){ UniqueId = 41293889, RuntimeId=1328 },
					},
					new Item[]
					{
						new Item(412, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(412, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(412, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(412, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(412, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(412, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 650 },
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(206, 0, 4){ UniqueId = 41293889, RuntimeId=4727 },
					},
					new Item[]
					{
						new Item(242, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(242, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(242, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(242, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 780 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(629, 0, 1){ UniqueId = 41293889, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(40, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(866, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(848, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 691 },
				new ShapedRecipe(1, 2,
					new List<Item>
					{
						new Item(208, 0, 4){ UniqueId = 41293889, RuntimeId=4738 },
					},
					new Item[]
					{
						new Item(846, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(1118, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 808 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(130, 0, 1){ UniqueId = 41293889, RuntimeId=4745 },
					},
					new Item[]
					{
						new Item(98, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(98, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(98, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(98, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(866, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(98, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(98, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(98, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(98, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 981 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(433, 0, 1){ UniqueId = 41293889, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(844, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(858, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 513 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(85, 0, 3){ UniqueId = 41293889, RuntimeId=4773 },
					},
					new Item[]
					{
						new Item(10, 0, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(10, 0, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(10, 0, 2){ UniqueId = 41293889, RuntimeId=0 },
						new Item(10, 0, 2){ UniqueId = 41293889, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 596 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(107, 0, 1){ UniqueId = 41293893, RuntimeId=4779 },
					},
					new Item[]
					{
						new Item(640, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(10, 0, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(10, 0, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 488 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(428, 0, 1){ UniqueId = 41293893, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(556, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(78, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(832, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 591 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(392, 0, 1){ UniqueId = 41293893, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(652, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(652, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 436 },
				new ShapedRecipe(2, 3,
					new List<Item>
					{
						new Item(-201, 0, 1){ UniqueId = 41293893, RuntimeId=4811 },
					},
					new Item[]
					{
						new Item(712, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(712, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 982 },
				new ShapedRecipe(2, 3,
					new List<Item>
					{
						new Item(-201, 0, 1){ UniqueId = 41293893, RuntimeId=4811 },
					},
					new Item[]
					{
						new Item(712, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(712, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 214 },
				new ShapedRecipe(2, 3,
					new List<Item>
					{
						new Item(-201, 0, 1){ UniqueId = 41293893, RuntimeId=4811 },
					},
					new Item[]
					{
						new Item(712, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(712, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 193 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(299, 0, 1){ UniqueId = 41293893, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(610, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(712, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 573 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(514, 0, 1){ UniqueId = 41293893, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(766, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(766, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(766, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 775 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(61, 0, 1){ UniqueId = 41293893, RuntimeId=4863 },
					},
					new Item[]
					{
						new Item(8, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(8, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(8, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(8, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(8, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(8, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(8, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(8, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 422 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(61, 0, 1){ UniqueId = 41293893, RuntimeId=4863 },
					},
					new Item[]
					{
						new Item(545, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(545, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(545, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(545, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(545, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(545, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(545, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(545, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 187 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(61, 0, 1){ UniqueId = 41293893, RuntimeId=4863 },
					},
					new Item[]
					{
						new Item(757, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(757, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(757, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(757, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(757, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(757, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(757, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(757, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 36 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(427, 0, 3){ UniqueId = 41293893, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(40, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 724 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(102, 0, 16){ UniqueId = 41293893, RuntimeId=4871 },
					},
					new Item[]
					{
						new Item(40, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 443 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(621, 0, 1){ UniqueId = 41293893, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(1026, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(1006, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 135 },
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(89, 0, 1){ UniqueId = 41293893, RuntimeId=4949 },
					},
					new Item[]
					{
						new Item(788, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(788, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(788, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(788, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 899 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(41, 0, 1){ UniqueId = 41293893, RuntimeId=4950 },
					},
					new Item[]
					{
						new Item(612, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(612, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(612, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(612, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(612, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(612, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(612, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(612, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(612, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 530 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(306, 0, 9){ UniqueId = 41293893, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(82, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 577 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(306, 0, 1){ UniqueId = 41293893, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(850, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(850, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(850, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(850, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(850, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(850, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(850, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(850, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(850, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 687 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(425, 0, 9){ UniqueId = 41293893, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(612, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 732 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(258, 0, 1){ UniqueId = 41293893, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(612, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(612, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(612, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(612, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(514, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(612, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(612, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(612, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(612, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 891 },
				new ShapedRecipe(2, 3,
					new List<Item>
					{
						new Item(325, 0, 1){ UniqueId = 41293893, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(612, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(612, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(612, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 846 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(354, 0, 1){ UniqueId = 41293893, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(612, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(612, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(612, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(612, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 877 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(283, 0, 1){ UniqueId = 41293893, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(850, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(850, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(850, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(850, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(558, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(850, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(850, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(850, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(850, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 493 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(352, 0, 1){ UniqueId = 41293893, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(612, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(612, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(612, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(612, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(612, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(612, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(612, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(612, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 432 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(351, 0, 1){ UniqueId = 41293893, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(612, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(612, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(612, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(612, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(612, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 763 },
				new ShapedRecipe(2, 3,
					new List<Item>
					{
						new Item(333, 0, 1){ UniqueId = 41293893, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(612, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(612, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 928 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(353, 0, 1){ UniqueId = 41293893, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(612, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(612, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(612, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(612, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(612, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(612, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(612, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 966 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(324, 0, 1){ UniqueId = 41293893, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(612, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(612, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(612, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 740 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(27, 0, 6){ UniqueId = 41293893, RuntimeId=4952 },
					},
					new Item[]
					{
						new Item(612, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(612, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(612, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(746, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(612, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(612, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(612, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 926 },
				new ShapedRecipe(1, 3,
					new List<Item>
					{
						new Item(323, 0, 1){ UniqueId = 41293893, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(612, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 415 },
				new ShapedRecipe(1, 3,
					new List<Item>
					{
						new Item(322, 0, 1){ UniqueId = 41293893, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(612, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(612, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 682 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(1, 0, 1){ UniqueId = 41293893, RuntimeId=7085 },
					},
					new List<Item>
					{
						new Item(2, 6, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(1048, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 528 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-169, 0, 4){ UniqueId = 41293893, RuntimeId=4964 },
					},
					new Item[]
					{
						new Item(2, 2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(2, 2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(2, 2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(2, 2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(2, 2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(2, 2, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 784 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(139, 0, 6){ UniqueId = 41293893, RuntimeId=1320 },
					},
					new Item[]
					{
						new Item(2, 2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(2, 2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(2, 2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(2, 2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(2, 2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(2, 2, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 911 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(567, 8, 1){ UniqueId = 41293893, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(70, 14, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(70, 14, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(70, 14, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(70, 14, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(70, 14, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(70, 14, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 827 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-420, 0, 1){ UniqueId = 41293893, RuntimeId=4975 },
					},
					new List<Item>
					{
						new Item(823, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(806, 0, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1 },
				new ShapedRecipe(2, 1,
					new List<Item>
					{
						new Item(171, 0, 3){ UniqueId = 41293893, RuntimeId=970 },
					},
					new Item[]
					{
						new Item(70, 14, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(70, 14, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 523 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(171, 0, 8){ UniqueId = 41293893, RuntimeId=970 },
					},
					new Item[]
					{
						new Item(342, 0, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(806, 0, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 799 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(237, 0, 8){ UniqueId = 41293893, RuntimeId=3666 },
					},
					new List<Item>
					{
						new Item(806, 0, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(24, 0, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(24, 0, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(24, 0, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(24, 0, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(26, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(26, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(26, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(26, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 953 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(403, 0, 2){ UniqueId = 41293893, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(790, 0, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(820, 0, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 896 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(403, 0, 2){ UniqueId = 41293893, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(790, 0, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(822, 0, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 797 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(403, 0, 2){ UniqueId = 41293893, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(826, 0, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(822, 0, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 550 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(403, 0, 2){ UniqueId = 41293893, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(826, 0, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(820, 0, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 930 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(241, 0, 8){ UniqueId = 41293893, RuntimeId=6999 },
					},
					new Item[]
					{
						new Item(40, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(806, 0, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 587 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(160, 0, 16){ UniqueId = 41293893, RuntimeId=7015 },
					},
					new Item[]
					{
						new Item(482, 14, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(482, 14, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(482, 14, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(482, 14, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(482, 14, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(482, 14, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 437 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(160, 0, 8){ UniqueId = 41293893, RuntimeId=7015 },
					},
					new Item[]
					{
						new Item(204, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(806, 0, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 649 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(159, 0, 8){ UniqueId = 41293893, RuntimeId=7031 },
					},
					new Item[]
					{
						new Item(344, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(806, 0, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 491 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(567, 2, 1){ UniqueId = 41293893, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(70, 26, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(70, 26, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(70, 26, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(70, 26, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(70, 26, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(70, 26, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 806 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-426, 0, 1){ UniqueId = 41293893, RuntimeId=4991 },
					},
					new List<Item>
					{
						new Item(823, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(794, 0, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 20 },
				new ShapedRecipe(2, 1,
					new List<Item>
					{
						new Item(171, 0, 3){ UniqueId = 41293893, RuntimeId=976 },
					},
					new Item[]
					{
						new Item(70, 26, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(70, 26, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 625 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(171, 0, 8){ UniqueId = 41293893, RuntimeId=976 },
					},
					new Item[]
					{
						new Item(342, 0, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(794, 0, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 556 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(237, 0, 8){ UniqueId = 41293893, RuntimeId=3672 },
					},
					new List<Item>
					{
						new Item(794, 0, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(24, 0, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(24, 0, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(24, 0, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(24, 0, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(26, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(26, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(26, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(26, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1005 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(241, 0, 8){ UniqueId = 41293893, RuntimeId=7005 },
					},
					new Item[]
					{
						new Item(40, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(794, 0, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 705 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(160, 0, 16){ UniqueId = 41293893, RuntimeId=7021 },
					},
					new Item[]
					{
						new Item(482, 26, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(482, 26, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(482, 26, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(482, 26, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(482, 26, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(482, 26, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 486 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(160, 0, 8){ UniqueId = 41293893, RuntimeId=7021 },
					},
					new Item[]
					{
						new Item(204, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(794, 0, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 913 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(159, 0, 8){ UniqueId = 41293893, RuntimeId=7037 },
					},
					new Item[]
					{
						new Item(344, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(794, 0, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 484 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(-195, 0, 1){ UniqueId = 41293893, RuntimeId=5007 },
					},
					new Item[]
					{
						new Item(640, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(331, 4, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 683 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(-195, 0, 1){ UniqueId = 41293893, RuntimeId=5007 },
					},
					new Item[]
					{
						new Item(640, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(88, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 215 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(-195, 0, 1){ UniqueId = 41293893, RuntimeId=5007 },
					},
					new Item[]
					{
						new Item(640, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(364, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 217 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(-195, 0, 1){ UniqueId = 41293893, RuntimeId=5007 },
					},
					new Item[]
					{
						new Item(640, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(323, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 324 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(-195, 0, 1){ UniqueId = 41293893, RuntimeId=5007 },
					},
					new Item[]
					{
						new Item(640, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(331, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 250 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(-195, 0, 1){ UniqueId = 41293893, RuntimeId=5007 },
					},
					new Item[]
					{
						new Item(640, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(88, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 270 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(-195, 0, 1){ UniqueId = 41293893, RuntimeId=5007 },
					},
					new Item[]
					{
						new Item(640, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(364, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 370 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(-195, 0, 1){ UniqueId = 41293893, RuntimeId=5007 },
					},
					new Item[]
					{
						new Item(640, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(323, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 232 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(-195, 0, 1){ UniqueId = 41293893, RuntimeId=5007 },
					},
					new Item[]
					{
						new Item(640, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(331, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 346 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(170, 0, 1){ UniqueId = 41293893, RuntimeId=5059 },
					},
					new Item[]
					{
						new Item(668, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(668, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(668, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(668, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(668, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(668, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(668, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(668, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(668, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 428 },
				new ShapedRecipe(2, 1,
					new List<Item>
					{
						new Item(148, 0, 1){ UniqueId = 41293893, RuntimeId=5071 },
					},
					new Item[]
					{
						new Item(610, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 958 },
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(-220, 0, 1){ UniqueId = 41293893, RuntimeId=5087 },
						new Item(427, 0, 4){ UniqueId = 41293893, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(1182, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(1182, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(1182, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(1182, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 375 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(591, 0, 4){ UniqueId = 41293893, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(439, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(854, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(854, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(854, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(854, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 373 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(416, 0, 3){ UniqueId = 41293893, RuntimeId=0 },
						new Item(427, 0, 1){ UniqueId = 41293893, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(1182, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 374 },
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(-221, 0, 1){ UniqueId = 41293893, RuntimeId=5088 },
					},
					new Item[]
					{
						new Item(1180, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(1180, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(1180, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(1180, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 376 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(527, 0, 1){ UniqueId = 41293893, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(610, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(108, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 455 },
				new ShapedRecipe(1, 2,
					new List<Item>
					{
						new Item(526, 0, 1){ UniqueId = 41293893, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(1054, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(740, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 842 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(504, 0, 9){ UniqueId = 41293893, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(679, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 148 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(504, 0, 9){ UniqueId = 41293893, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(687, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 64 },
				new ShapedRecipe(2, 3,
					new List<Item>
					{
						new Item(298, 0, 1){ UniqueId = 41293893, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(610, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 565 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(101, 0, 16){ UniqueId = 41293893, RuntimeId=5108 },
					},
					new Item[]
					{
						new Item(610, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 741 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(42, 0, 1){ UniqueId = 41293893, RuntimeId=5109 },
					},
					new Item[]
					{
						new Item(610, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 610 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(346, 0, 1){ UniqueId = 41293893, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(610, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 956 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(344, 0, 1){ UniqueId = 41293893, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(610, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 462 },
				new ShapedRecipe(2, 3,
					new List<Item>
					{
						new Item(372, 0, 3){ UniqueId = 41293893, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(610, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1008 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(343, 0, 1){ UniqueId = 41293893, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(610, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 993 },
				new ShapedRecipe(2, 3,
					new List<Item>
					{
						new Item(331, 0, 1){ UniqueId = 41293893, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(610, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 906 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(305, 0, 9){ UniqueId = 41293893, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(84, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 606 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(305, 0, 1){ UniqueId = 41293893, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(1138, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(1138, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(1138, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(1138, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(1138, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(1138, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(1138, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(1138, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(1138, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 791 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(345, 0, 1){ UniqueId = 41293893, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(610, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 744 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(569, 0, 9){ UniqueId = 41293893, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(610, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 398 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(297, 0, 1){ UniqueId = 41293893, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(610, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 391 },
				new ShapedRecipe(1, 3,
					new List<Item>
					{
						new Item(296, 0, 1){ UniqueId = 41293893, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(610, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 448 },
				new ShapedRecipe(1, 3,
					new List<Item>
					{
						new Item(307, 0, 1){ UniqueId = 41293893, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(610, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 870 },
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(167, 0, 1){ UniqueId = 41293893, RuntimeId=5143 },
					},
					new Item[]
					{
						new Item(610, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 737 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(513, 0, 1){ UniqueId = 41293893, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(640, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(762, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 856 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(84, 0, 1){ UniqueId = 41293893, RuntimeId=5183 },
					},
					new Item[]
					{
						new Item(483, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(608, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 328 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(84, 0, 1){ UniqueId = 41293893, RuntimeId=5183 },
					},
					new Item[]
					{
						new Item(485, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(608, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 275 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(377, 0, 1){ UniqueId = 41293893, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(10, 6, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(10, 6, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(10, 6, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(618, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(10, 6, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(10, 6, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 742 },
				new ShapedRecipe(2, 3,
					new List<Item>
					{
						new Item(555, 0, 3){ UniqueId = 41293893, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(10, 6, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(10, 6, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(10, 6, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(10, 6, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(10, 6, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(10, 6, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 410 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(85, 0, 3){ UniqueId = 41293893, RuntimeId=4776 },
					},
					new Item[]
					{
						new Item(10, 6, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(10, 6, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(10, 6, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(10, 6, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 690 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(185, 0, 1){ UniqueId = 41293893, RuntimeId=5228 },
					},
					new Item[]
					{
						new Item(640, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(10, 6, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(10, 6, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 527 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(5, 0, 4){ UniqueId = 41293893, RuntimeId=5773 },
					},
					new Item[]
					{
						new Item(34, 6, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 847 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(5, 0, 4){ UniqueId = 41293893, RuntimeId=5773 },
					},
					new Item[]
					{
						new Item(13, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 472 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(5, 0, 4){ UniqueId = 41293893, RuntimeId=5773 },
					},
					new Item[]
					{
						new Item(423, 22, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 692 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(5, 0, 4){ UniqueId = 41293893, RuntimeId=5773 },
					},
					new Item[]
					{
						new Item(423, 6, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 723 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(136, 0, 4){ UniqueId = 41293893, RuntimeId=5260 },
					},
					new Item[]
					{
						new Item(10, 6, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(10, 6, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(10, 6, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(10, 6, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(10, 6, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(10, 6, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 658 },
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(-212, 0, 3){ UniqueId = 41293893, RuntimeId=7714 },
					},
					new Item[]
					{
						new Item(34, 6, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(34, 6, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(34, 6, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(34, 6, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 759 },
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(-212, 0, 3){ UniqueId = 41293893, RuntimeId=7720 },
					},
					new Item[]
					{
						new Item(13, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(13, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(13, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(13, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 804 },
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(158, 0, 6){ UniqueId = 41293893, RuntimeId=7810 },
					},
					new Item[]
					{
						new Item(10, 6, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(10, 6, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(10, 6, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 975 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(65, 0, 3){ UniqueId = 41293893, RuntimeId=5332 },
					},
					new Item[]
					{
						new Item(640, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 546 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-208, 0, 1){ UniqueId = 41293893, RuntimeId=5338 },
					},
					new Item[]
					{
						new Item(1138, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(1138, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(1138, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(1138, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(100, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(1138, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(1138, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(1138, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(1138, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 643 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(22, 0, 1){ UniqueId = 41293893, RuntimeId=5340 },
					},
					new Item[]
					{
						new Item(828, 0, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(828, 0, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(828, 0, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(828, 0, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(828, 0, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(828, 0, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(828, 0, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(828, 0, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(828, 0, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 475 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(414, 0, 9){ UniqueId = 41293893, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(44, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 669 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(547, 0, 2){ UniqueId = 41293893, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(652, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(652, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(652, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(776, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(652, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 423 },
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(381, 0, 1){ UniqueId = 41293897, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(1058, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(1058, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(1058, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
						new Item(1058, -2, 2){ UniqueId = 41293893, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 664 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(338, 0, 1){ UniqueId = 41293897, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(762, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(762, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(762, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(762, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 381 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(336, 0, 1){ UniqueId = 41293897, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(762, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(762, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(762, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(762, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(762, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(762, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(762, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(762, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 783 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(335, 0, 1){ UniqueId = 41293897, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(762, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(762, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(762, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(762, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(762, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 586 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(530, 0, 1){ UniqueId = 41293897, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(762, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(762, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(762, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(762, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(762, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(762, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(762, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 496 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(337, 0, 1){ UniqueId = 41293897, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(762, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(762, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(762, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(762, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(762, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(762, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(762, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 447 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-194, 0, 1){ UniqueId = 41293897, RuntimeId=5409 },
					},
					new Item[]
					{
						new Item(316, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(316, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(94, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(316, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(316, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 507 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-194, 0, 1){ UniqueId = 41293897, RuntimeId=5409 },
					},
					new Item[]
					{
						new Item(527, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(527, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(94, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(527, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(527, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 365 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-194, 0, 1){ UniqueId = 41293897, RuntimeId=5409 },
					},
					new Item[]
					{
						new Item(529, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(529, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(94, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(529, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(529, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 191 },
				new ShapedRecipe(1, 2,
					new List<Item>
					{
						new Item(69, 0, 1){ UniqueId = 41293897, RuntimeId=5417 },
					},
					new Item[]
					{
						new Item(640, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(8, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 868 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(567, 12, 1){ UniqueId = 41293897, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(70, 6, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(70, 6, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(70, 6, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(70, 6, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(70, 6, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(70, 6, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 520 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-416, 0, 1){ UniqueId = 41293897, RuntimeId=5449 },
					},
					new List<Item>
					{
						new Item(823, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(814, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2 },
				new ShapedRecipe(2, 1,
					new List<Item>
					{
						new Item(171, 0, 3){ UniqueId = 41293897, RuntimeId=966 },
					},
					new Item[]
					{
						new Item(70, 6, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(70, 6, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 716 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(171, 0, 8){ UniqueId = 41293897, RuntimeId=966 },
					},
					new Item[]
					{
						new Item(342, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(814, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 407 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(237, 0, 8){ UniqueId = 41293897, RuntimeId=3662 },
					},
					new List<Item>
					{
						new Item(814, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(24, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(24, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(24, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(24, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(26, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(26, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(26, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(26, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 411 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(407, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(798, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(820, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 753 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(407, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(798, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(822, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 680 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(407, 0, 1){ UniqueId = 41293897, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(76, 2, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 545 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(407, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(828, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(822, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 812 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(407, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(828, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(820, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 478 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(241, 0, 8){ UniqueId = 41293897, RuntimeId=6995 },
					},
					new Item[]
					{
						new Item(40, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(814, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 989 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(160, 0, 16){ UniqueId = 41293897, RuntimeId=7011 },
					},
					new Item[]
					{
						new Item(482, 6, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(482, 6, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(482, 6, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(482, 6, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(482, 6, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(482, 6, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 766 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(160, 0, 8){ UniqueId = 41293897, RuntimeId=7011 },
					},
					new Item[]
					{
						new Item(204, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(814, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 777 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(159, 0, 8){ UniqueId = 41293897, RuntimeId=7027 },
					},
					new Item[]
					{
						new Item(344, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(814, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 971 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(171, 0, 8){ UniqueId = 41293897, RuntimeId=971 },
					},
					new Item[]
					{
						new Item(342, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(804, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 647 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(567, 7, 1){ UniqueId = 41293897, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(70, 16, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(70, 16, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(70, 16, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(70, 16, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(70, 16, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(70, 16, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 918 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-421, 0, 1){ UniqueId = 41293897, RuntimeId=5465 },
					},
					new List<Item>
					{
						new Item(823, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(804, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 18 },
				new ShapedRecipe(2, 1,
					new List<Item>
					{
						new Item(171, 0, 3){ UniqueId = 41293897, RuntimeId=971 },
					},
					new Item[]
					{
						new Item(70, 16, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(70, 16, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 821 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(237, 0, 8){ UniqueId = 41293897, RuntimeId=3667 },
					},
					new List<Item>
					{
						new Item(804, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(24, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(24, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(24, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(24, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(26, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(26, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(26, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(26, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 390 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(402, 0, 3){ UniqueId = 41293897, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(790, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(820, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(820, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 739 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(402, 0, 1){ UniqueId = 41293897, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(76, 6, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 850 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(402, 0, 3){ UniqueId = 41293897, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(790, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(822, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(822, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 509 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(402, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(806, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(822, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 873 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(402, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(806, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(820, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 798 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(402, 0, 3){ UniqueId = 41293897, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(826, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(822, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(822, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 921 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(402, 0, 3){ UniqueId = 41293897, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(826, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(820, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(820, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 466 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(402, 0, 1){ UniqueId = 41293897, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(76, 16, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 990 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(402, 0, 1){ UniqueId = 41293897, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(76, 12, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 608 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(241, 0, 8){ UniqueId = 41293897, RuntimeId=7000 },
					},
					new Item[]
					{
						new Item(40, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(804, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 795 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(160, 0, 16){ UniqueId = 41293897, RuntimeId=7016 },
					},
					new Item[]
					{
						new Item(482, 16, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(482, 16, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(482, 16, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(482, 16, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(482, 16, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(482, 16, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1004 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(160, 0, 8){ UniqueId = 41293897, RuntimeId=7016 },
					},
					new Item[]
					{
						new Item(204, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(804, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 479 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(159, 0, 8){ UniqueId = 41293897, RuntimeId=7032 },
					},
					new Item[]
					{
						new Item(344, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(804, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 429 },
				new ShapedRecipe(2, 1,
					new List<Item>
					{
						new Item(147, 0, 1){ UniqueId = 41293897, RuntimeId=5475 },
					},
					new Item[]
					{
						new Item(612, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(612, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 629 },
				new ShapedRecipe(1, 3,
					new List<Item>
					{
						new Item(-312, 0, 1){ UniqueId = 41293897, RuntimeId=5491 },
					},
					new Item[]
					{
						new Item(1008, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(1008, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(1008, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 120 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(171, 0, 8){ UniqueId = 41293897, RuntimeId=968 },
					},
					new Item[]
					{
						new Item(342, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(810, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 514 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(567, 10, 1){ UniqueId = 41293897, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(70, 10, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(70, 10, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(70, 10, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(70, 10, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(70, 10, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(70, 10, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 886 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-418, 0, 1){ UniqueId = 41293897, RuntimeId=5497 },
					},
					new List<Item>
					{
						new Item(823, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(810, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 17 },
				new ShapedRecipe(2, 1,
					new List<Item>
					{
						new Item(171, 0, 3){ UniqueId = 41293897, RuntimeId=968 },
					},
					new Item[]
					{
						new Item(70, 10, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(70, 10, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 564 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(237, 0, 8){ UniqueId = 41293897, RuntimeId=3664 },
					},
					new List<Item>
					{
						new Item(810, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(24, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(24, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(24, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(24, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(26, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(26, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(26, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(26, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 453 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(405, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(794, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(820, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 999 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(405, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(794, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(822, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 512 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(241, 0, 8){ UniqueId = 41293897, RuntimeId=6997 },
					},
					new Item[]
					{
						new Item(40, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(810, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 733 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(160, 0, 16){ UniqueId = 41293897, RuntimeId=7013 },
					},
					new Item[]
					{
						new Item(482, 10, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(482, 10, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(482, 10, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(482, 10, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(482, 10, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(482, 10, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 972 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(160, 0, 8){ UniqueId = 41293897, RuntimeId=7013 },
					},
					new Item[]
					{
						new Item(204, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(810, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 813 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(159, 0, 8){ UniqueId = 41293897, RuntimeId=7029 },
					},
					new Item[]
					{
						new Item(344, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(810, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 727 },
				new ShapedRecipe(1, 2,
					new List<Item>
					{
						new Item(91, 0, 1){ UniqueId = 41293897, RuntimeId=5526 },
					},
					new Item[]
					{
						new Item(309, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(100, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 547 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(515, 2, 1){ UniqueId = 41293897, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(772, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(772, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(772, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(772, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(782, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(772, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(772, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(772, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(772, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 859 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-222, 0, 1){ UniqueId = 41293897, RuntimeId=5538 },
					},
					new Item[]
					{
						new Item(196, 6, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(196, 6, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(196, 6, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(196, 6, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(1202, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(196, 6, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(196, 6, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(196, 6, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(196, 6, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 256 },
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(-204, 0, 1){ UniqueId = 41293897, RuntimeId=5557 },
					},
					new Item[]
					{
						new Item(652, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(652, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 286 },
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(-204, 0, 1){ UniqueId = 41293897, RuntimeId=5557 },
					},
					new Item[]
					{
						new Item(652, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(652, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 274 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(567, 13, 1){ UniqueId = 41293897, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(70, 4, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(70, 4, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(70, 4, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(70, 4, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(70, 4, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(70, 4, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 620 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-415, 0, 1){ UniqueId = 41293897, RuntimeId=5561 },
					},
					new List<Item>
					{
						new Item(823, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(816, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 21 },
				new ShapedRecipe(2, 1,
					new List<Item>
					{
						new Item(171, 0, 3){ UniqueId = 41293897, RuntimeId=965 },
					},
					new Item[]
					{
						new Item(70, 4, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(70, 4, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 551 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(171, 0, 8){ UniqueId = 41293897, RuntimeId=965 },
					},
					new Item[]
					{
						new Item(342, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(816, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 951 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(237, 0, 8){ UniqueId = 41293897, RuntimeId=3661 },
					},
					new List<Item>
					{
						new Item(816, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(24, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(24, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(24, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(24, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(26, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(26, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(26, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(26, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 945 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(408, 0, 3){ UniqueId = 41293897, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(798, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(792, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(808, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 639 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(408, 0, 1){ UniqueId = 41293897, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(76, 4, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 917 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(408, 0, 4){ UniqueId = 41293897, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(798, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(792, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(792, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(822, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 384 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(408, 0, 4){ UniqueId = 41293897, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(798, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(792, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(792, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(820, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 760 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(408, 0, 4){ UniqueId = 41293897, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(828, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(792, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(792, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(822, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 706 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(408, 0, 4){ UniqueId = 41293897, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(828, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(792, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(792, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(820, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 858 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(408, 0, 3){ UniqueId = 41293897, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(828, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(792, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(808, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 458 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(408, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(350, 2, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 632 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(408, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(800, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(808, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 755 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(241, 0, 8){ UniqueId = 41293897, RuntimeId=6994 },
					},
					new Item[]
					{
						new Item(40, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(816, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 562 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(160, 0, 16){ UniqueId = 41293897, RuntimeId=7010 },
					},
					new Item[]
					{
						new Item(482, 4, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(482, 4, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(482, 4, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(482, 4, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(482, 4, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(482, 4, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 684 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(160, 0, 8){ UniqueId = 41293897, RuntimeId=7010 },
					},
					new Item[]
					{
						new Item(204, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(816, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 673 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(159, 0, 8){ UniqueId = 41293897, RuntimeId=7026 },
					},
					new Item[]
					{
						new Item(344, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(816, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 707 },
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(213, 0, 1){ UniqueId = 41293897, RuntimeId=5577 },
					},
					new Item[]
					{
						new Item(860, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(860, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(860, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(860, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 427 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(430, 0, 1){ UniqueId = 41293897, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(858, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(776, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 964 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(515, 0, 1){ UniqueId = 41293897, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(772, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(772, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(772, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(772, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(772, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(772, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(772, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(772, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(772, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 807 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(103, 0, 1){ UniqueId = 41293897, RuntimeId=5584 },
					},
					new Item[]
					{
						new Item(544, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(544, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(544, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(544, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(544, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(544, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(544, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(544, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(544, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 602 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(293, 0, 1){ UniqueId = 41293897, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(544, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 552 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(370, 0, 1){ UniqueId = 41293897, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(610, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 433 },
				new ShapedRecipe(2, 1,
					new List<Item>
					{
						new Item(-335, 0, 3){ UniqueId = 41293897, RuntimeId=5641 },
					},
					new Item[]
					{
						new Item(639, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(639, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 125 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(48, 0, 1){ UniqueId = 41293897, RuntimeId=5642 },
					},
					new List<Item>
					{
						new Item(8, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(212, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1007 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(48, 0, 1){ UniqueId = 41293897, RuntimeId=5642 },
					},
					new List<Item>
					{
						new Item(8, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(639, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 77 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-179, 0, 4){ UniqueId = 41293897, RuntimeId=5643 },
					},
					new Item[]
					{
						new Item(96, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(96, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(96, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(96, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(96, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(96, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 642 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(139, 0, 6){ UniqueId = 41293897, RuntimeId=1319 },
					},
					new Item[]
					{
						new Item(96, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(96, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(96, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(96, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(96, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(96, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 720 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-175, 0, 4){ UniqueId = 41293897, RuntimeId=5651 },
					},
					new Item[]
					{
						new Item(196, 2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(196, 2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(196, 2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(196, 2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(196, 2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(196, 2, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 725 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(139, 0, 6){ UniqueId = 41293897, RuntimeId=1326 },
					},
					new Item[]
					{
						new Item(196, 2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(196, 2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(196, 2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(196, 2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(196, 2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(196, 2, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 515 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(98, 0, 1){ UniqueId = 41293897, RuntimeId=7194 },
					},
					new List<Item>
					{
						new Item(196, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(212, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 517 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(98, 0, 1){ UniqueId = 41293897, RuntimeId=7194 },
					},
					new List<Item>
					{
						new Item(196, 0, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(639, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 85 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(260, 0, 1){ UniqueId = 41293897, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(78, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(80, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(642, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 377 },
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(112, 0, 1){ UniqueId = 41293897, RuntimeId=5661 },
					},
					new Item[]
					{
						new Item(1046, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(1046, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(1046, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(1046, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 867 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(113, 0, 6){ UniqueId = 41293897, RuntimeId=5662 },
					},
					new Item[]
					{
						new Item(224, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(224, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(1046, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(1046, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(224, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(224, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 854 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(114, 0, 4){ UniqueId = 41293897, RuntimeId=5663 },
					},
					new Item[]
					{
						new Item(224, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(224, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(224, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(224, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(224, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(224, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 885 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(139, 0, 6){ UniqueId = 41293897, RuntimeId=1327 },
					},
					new Item[]
					{
						new Item(224, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(224, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(224, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(224, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(224, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(224, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 800 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(214, 0, 1){ UniqueId = 41293897, RuntimeId=5677 },
					},
					new Item[]
					{
						new Item(588, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(588, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(588, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(588, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(588, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(588, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(588, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(588, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(588, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 863 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-270, 0, 1){ UniqueId = 41293897, RuntimeId=5678 },
					},
					new Item[]
					{
						new Item(1202, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(1202, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(1202, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(1202, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(1202, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(1202, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(1202, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(1202, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(1202, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 229 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(601, 0, 1){ UniqueId = 41293897, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(1222, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(1222, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(1222, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(1222, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(612, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(612, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(612, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
						new Item(612, -2, 2){ UniqueId = 41293897, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 204 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(601, 0, 9){ UniqueId = 41293901, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(539, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 197 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(25, 0, 1){ UniqueId = 41293901, RuntimeId=5689 },
					},
					new Item[]
					{
						new Item(483, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(746, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 239 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(25, 0, 1){ UniqueId = 41293901, RuntimeId=5689 },
					},
					new Item[]
					{
						new Item(485, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(746, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 251 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(5, 0, 4){ UniqueId = 41293901, RuntimeId=5770 },
					},
					new Item[]
					{
						new Item(34, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 440 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(5, 0, 4){ UniqueId = 41293901, RuntimeId=5770 },
					},
					new Item[]
					{
						new Item(19, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 646 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(5, 0, 4){ UniqueId = 41293901, RuntimeId=5770 },
					},
					new Item[]
					{
						new Item(423, 16, 2){ UniqueId = 41293901, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 521 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(5, 0, 4){ UniqueId = 41293901, RuntimeId=5770 },
					},
					new Item[]
					{
						new Item(423, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 585 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(53, 0, 4){ UniqueId = 41293901, RuntimeId=5690 },
					},
					new Item[]
					{
						new Item(10, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(10, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(10, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(10, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(10, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(10, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 774 },
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(-212, 0, 3){ UniqueId = 41293901, RuntimeId=7711 },
					},
					new Item[]
					{
						new Item(34, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(34, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(34, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(34, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 787 },
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(-212, 0, 3){ UniqueId = 41293901, RuntimeId=7717 },
					},
					new Item[]
					{
						new Item(19, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(19, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(19, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(19, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 887 },
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(158, 0, 6){ UniqueId = 41293901, RuntimeId=7807 },
					},
					new Item[]
					{
						new Item(10, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(10, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(10, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 719 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(251, 0, 1){ UniqueId = 41293901, RuntimeId=5698 },
					},
					new Item[]
					{
						new Item(8, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(746, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(8, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(8, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(746, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(8, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(8, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(1048, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(8, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 979 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(567, 14, 1){ UniqueId = 41293901, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(70, 2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(70, 2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(70, 2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(70, 2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(70, 2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(70, 2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 519 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-414, 0, 1){ UniqueId = 41293901, RuntimeId=5711 },
					},
					new List<Item>
					{
						new Item(823, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(818, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 14 },
				new ShapedRecipe(2, 1,
					new List<Item>
					{
						new Item(171, 0, 3){ UniqueId = 41293901, RuntimeId=964 },
					},
					new Item[]
					{
						new Item(70, 2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(70, 2, 2){ UniqueId = 41293901, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 781 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(171, 0, 8){ UniqueId = 41293901, RuntimeId=964 },
					},
					new Item[]
					{
						new Item(342, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(818, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 609 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(237, 0, 8){ UniqueId = 41293901, RuntimeId=3660 },
					},
					new List<Item>
					{
						new Item(818, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(24, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(24, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(24, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(24, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(26, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(26, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(26, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(26, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 385 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(409, 0, 1){ UniqueId = 41293901, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(76, 10, 2){ UniqueId = 41293901, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 539 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(409, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(792, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(812, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 790 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(241, 0, 8){ UniqueId = 41293901, RuntimeId=6993 },
					},
					new Item[]
					{
						new Item(40, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(818, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 574 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(160, 0, 16){ UniqueId = 41293901, RuntimeId=7009 },
					},
					new Item[]
					{
						new Item(482, 2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(482, 2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(482, 2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(482, 2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(482, 2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(482, 2, 2){ UniqueId = 41293901, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 933 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(160, 0, 8){ UniqueId = 41293901, RuntimeId=7009 },
					},
					new Item[]
					{
						new Item(204, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(818, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 927 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(159, 0, 8){ UniqueId = 41293901, RuntimeId=7025 },
					},
					new Item[]
					{
						new Item(344, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(818, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 830 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(174, 0, 1){ UniqueId = 41293901, RuntimeId=5741 },
					},
					new Item[]
					{
						new Item(158, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(158, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(158, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(158, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(158, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(158, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(158, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(158, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(158, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 393 },
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(386, 0, 3){ UniqueId = 41293901, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(770, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(770, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(770, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 970 },
				new ShapedRecipe(1, 2,
					new List<Item>
					{
						new Item(155, 0, 2){ UniqueId = 41293901, RuntimeId=6520 },
					},
					new Item[]
					{
						new Item(310, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(310, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 717 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(567, 9, 1){ UniqueId = 41293901, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(70, 12, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(70, 12, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(70, 12, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(70, 12, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(70, 12, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(70, 12, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 893 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-419, 0, 1){ UniqueId = 41293901, RuntimeId=5742 },
					},
					new List<Item>
					{
						new Item(823, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(808, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 11 },
				new ShapedRecipe(2, 1,
					new List<Item>
					{
						new Item(171, 0, 3){ UniqueId = 41293901, RuntimeId=969 },
					},
					new Item[]
					{
						new Item(70, 12, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(70, 12, 2){ UniqueId = 41293901, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 957 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(171, 0, 8){ UniqueId = 41293901, RuntimeId=969 },
					},
					new Item[]
					{
						new Item(342, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(808, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 543 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(237, 0, 8){ UniqueId = 41293901, RuntimeId=3665 },
					},
					new List<Item>
					{
						new Item(808, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(24, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(24, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(24, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(24, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(26, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(26, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(26, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(26, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 843 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(404, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(792, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(820, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 504 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(404, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(350, 10, 2){ UniqueId = 41293901, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 604 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(404, 0, 1){ UniqueId = 41293901, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(76, 14, 2){ UniqueId = 41293901, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 468 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(404, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(792, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(822, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 457 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(241, 0, 8){ UniqueId = 41293901, RuntimeId=6998 },
					},
					new Item[]
					{
						new Item(40, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(808, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 446 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(160, 0, 16){ UniqueId = 41293901, RuntimeId=7014 },
					},
					new Item[]
					{
						new Item(482, 12, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(482, 12, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(482, 12, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(482, 12, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(482, 12, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(482, 12, 2){ UniqueId = 41293901, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 451 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(160, 0, 8){ UniqueId = 41293901, RuntimeId=7014 },
					},
					new Item[]
					{
						new Item(204, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(808, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 473 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(159, 0, 8){ UniqueId = 41293901, RuntimeId=7030 },
					},
					new Item[]
					{
						new Item(344, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(808, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 654 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(33, 0, 1){ UniqueId = 41293901, RuntimeId=5759 },
					},
					new Item[]
					{
						new Item(483, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(8, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(8, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(746, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(8, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(8, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 269 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(33, 0, 1){ UniqueId = 41293901, RuntimeId=5759 },
					},
					new Item[]
					{
						new Item(485, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(8, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(8, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(746, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(8, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(8, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 236 },
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(1, 0, 4){ UniqueId = 41293901, RuntimeId=7090 },
					},
					new Item[]
					{
						new Item(2, 10, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(2, 10, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(2, 10, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(2, 10, 2){ UniqueId = 41293901, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 883 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-174, 0, 4){ UniqueId = 41293901, RuntimeId=5787 },
					},
					new Item[]
					{
						new Item(2, 12, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(2, 12, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(2, 12, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(2, 12, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(2, 12, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(2, 12, 2){ UniqueId = 41293901, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 997 },
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(-235, 0, 4){ UniqueId = 41293901, RuntimeId=5795 },
					},
					new Item[]
					{
						new Item(467, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(467, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(467, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(467, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 298 },
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(-291, 0, 4){ UniqueId = 41293901, RuntimeId=5798 },
					},
					new Item[]
					{
						new Item(545, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(545, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(545, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(545, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 254 },
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(-284, 0, 6){ UniqueId = 41293901, RuntimeId=5801 },
					},
					new Item[]
					{
						new Item(547, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(547, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(547, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 216 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-275, 0, 4){ UniqueId = 41293901, RuntimeId=5803 },
					},
					new Item[]
					{
						new Item(547, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(547, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(547, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(547, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(547, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(547, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 189 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(-278, 0, 6){ UniqueId = 41293901, RuntimeId=5811 },
					},
					new Item[]
					{
						new Item(547, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(547, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(547, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(547, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(547, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(547, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 272 },
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(-274, 0, 4){ UniqueId = 41293901, RuntimeId=5973 },
					},
					new Item[]
					{
						new Item(581, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(581, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(581, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(581, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 310 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(-296, 0, 1){ UniqueId = 41293901, RuntimeId=5974 },
					},
					new Item[]
					{
						new Item(581, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 317 },
				new ShapedRecipe(2, 1,
					new List<Item>
					{
						new Item(-295, 0, 1){ UniqueId = 41293901, RuntimeId=5988 },
					},
					new Item[]
					{
						new Item(581, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(581, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 268 },
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(-293, 0, 6){ UniqueId = 41293901, RuntimeId=6004 },
					},
					new Item[]
					{
						new Item(581, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(581, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(581, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 190 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-292, 0, 4){ UniqueId = 41293901, RuntimeId=6006 },
					},
					new Item[]
					{
						new Item(581, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(581, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(581, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(581, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(581, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(581, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 339 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(-297, 0, 6){ UniqueId = 41293901, RuntimeId=6014 },
					},
					new Item[]
					{
						new Item(581, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(581, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(581, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(581, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(581, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(581, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 223 },
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(-383, 0, 4){ UniqueId = 41293901, RuntimeId=6176 },
					},
					new Item[]
					{
						new Item(757, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(757, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(757, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(757, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 136 },
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(-384, 0, 6){ UniqueId = 41293901, RuntimeId=6179 },
					},
					new Item[]
					{
						new Item(765, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(765, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(765, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 103 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-385, 0, 4){ UniqueId = 41293901, RuntimeId=6181 },
					},
					new Item[]
					{
						new Item(765, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(765, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(765, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(765, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(765, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(765, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 142 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(-386, 0, 6){ UniqueId = 41293901, RuntimeId=6189 },
					},
					new Item[]
					{
						new Item(765, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(765, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(765, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(765, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(765, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(765, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 69 },
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(1, 0, 4){ UniqueId = 41293901, RuntimeId=7088 },
					},
					new Item[]
					{
						new Item(2, 6, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(2, 6, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(2, 6, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(2, 6, 2){ UniqueId = 41293901, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 392 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-173, 0, 4){ UniqueId = 41293901, RuntimeId=6351 },
					},
					new Item[]
					{
						new Item(2, 8, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(2, 8, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(2, 8, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(2, 8, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(2, 8, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(2, 8, 2){ UniqueId = 41293901, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 967 },
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(1, 0, 4){ UniqueId = 41293901, RuntimeId=7086 },
					},
					new Item[]
					{
						new Item(2, 2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(2, 2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(2, 2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(2, 2, 2){ UniqueId = 41293901, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 533 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-172, 0, 4){ UniqueId = 41293901, RuntimeId=6359 },
					},
					new Item[]
					{
						new Item(2, 4, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(2, 4, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(2, 4, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(2, 4, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(2, 4, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(2, 4, 2){ UniqueId = 41293901, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 378 },
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(168, 0, 1){ UniqueId = 41293901, RuntimeId=6411 },
					},
					new Item[]
					{
						new Item(1130, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(1130, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(1130, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(1130, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 713 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(168, 0, 1){ UniqueId = 41293901, RuntimeId=6413 },
					},
					new Item[]
					{
						new Item(1130, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(1130, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(1130, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(1130, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(1130, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(1130, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(1130, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(1130, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(1130, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 403 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-2, 0, 4){ UniqueId = 41293901, RuntimeId=6422 },
					},
					new Item[]
					{
						new Item(336, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(336, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(336, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(336, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(336, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(336, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 648 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-4, 0, 4){ UniqueId = 41293901, RuntimeId=6414 },
					},
					new Item[]
					{
						new Item(336, 4, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(336, 4, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(336, 4, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(336, 4, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(336, 4, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(336, 4, 2){ UniqueId = 41293901, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 833 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-3, 0, 4){ UniqueId = 41293901, RuntimeId=4036 },
					},
					new Item[]
					{
						new Item(336, 2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(336, 2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(336, 2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(336, 2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(336, 2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(336, 2, 2){ UniqueId = 41293901, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 985 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(139, 0, 6){ UniqueId = 41293901, RuntimeId=1329 },
					},
					new Item[]
					{
						new Item(336, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(336, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(336, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(336, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(336, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(336, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 572 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(284, 0, 1){ UniqueId = 41293901, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(172, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(832, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(780, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 943 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(292, 0, 4){ UniqueId = 41293901, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(172, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 884 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(567, 5, 1){ UniqueId = 41293901, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(70, 20, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(70, 20, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(70, 20, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(70, 20, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(70, 20, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(70, 20, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 758 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-423, 0, 1){ UniqueId = 41293901, RuntimeId=6482 },
					},
					new List<Item>
					{
						new Item(823, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(800, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 9 },
				new ShapedRecipe(2, 1,
					new List<Item>
					{
						new Item(171, 0, 3){ UniqueId = 41293901, RuntimeId=973 },
					},
					new Item[]
					{
						new Item(70, 20, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(70, 20, 2){ UniqueId = 41293901, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 914 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(171, 0, 8){ UniqueId = 41293901, RuntimeId=973 },
					},
					new Item[]
					{
						new Item(342, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(800, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 467 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(237, 0, 8){ UniqueId = 41293901, RuntimeId=3669 },
					},
					new List<Item>
					{
						new Item(800, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(24, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(24, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(24, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(24, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(26, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(26, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(26, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(26, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 734 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(400, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(798, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(792, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 445 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(400, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(828, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(792, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 936 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(241, 0, 8){ UniqueId = 41293901, RuntimeId=7002 },
					},
					new Item[]
					{
						new Item(40, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(800, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 889 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(160, 0, 16){ UniqueId = 41293901, RuntimeId=7018 },
					},
					new Item[]
					{
						new Item(482, 20, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(482, 20, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(482, 20, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(482, 20, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(482, 20, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(482, 20, 2){ UniqueId = 41293901, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 941 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(160, 0, 8){ UniqueId = 41293901, RuntimeId=7018 },
					},
					new Item[]
					{
						new Item(204, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(800, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 752 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(159, 0, 8){ UniqueId = 41293901, RuntimeId=7034 },
					},
					new Item[]
					{
						new Item(344, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(800, 0, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 576 },
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(201, 0, 4){ UniqueId = 41293901, RuntimeId=6498 },
					},
					new Item[]
					{
						new Item(1118, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(1118, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(1118, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(1118, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 874 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(203, 0, 4){ UniqueId = 41293901, RuntimeId=6510 },
					},
					new Item[]
					{
						new Item(402, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(402, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(402, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(402, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(402, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(402, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 380 },
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(155, 0, 1){ UniqueId = 41293901, RuntimeId=6518 },
					},
					new Item[]
					{
						new Item(1048, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(1048, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(1048, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
						new Item(1048, -2, 2){ UniqueId = 41293901, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 476 },
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(-304, 0, 1){ UniqueId = 41293905, RuntimeId=6530 },
					},
					new Item[]
					{
						new Item(310, 0, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(310, 0, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(310, 0, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(310, 0, 2){ UniqueId = 41293905, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 249 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(156, 0, 4){ UniqueId = 41293905, RuntimeId=6532 },
					},
					new Item[]
					{
						new Item(310, 0, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(310, 0, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(310, 0, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(310, 0, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(310, 0, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(310, 0, 2){ UniqueId = 41293905, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 442 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(290, 0, 1){ UniqueId = 41293905, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(642, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(562, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(558, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(78, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(578, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 419 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(290, 0, 1){ UniqueId = 41293905, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(642, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(562, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(558, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(80, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(578, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 400 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(66, 0, 16){ UniqueId = 41293905, RuntimeId=6540 },
					},
					new Item[]
					{
						new Item(610, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 892 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(507, 0, 9){ UniqueId = 41293905, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(903, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 38 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-452, 0, 1){ UniqueId = 41293905, RuntimeId=6550 },
					},
					new Item[]
					{
						new Item(1014, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(1014, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(1014, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(1014, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(1014, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(1014, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(1014, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(1014, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(1014, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 140 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(506, 0, 9){ UniqueId = 41293905, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(905, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 93 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-453, 0, 1){ UniqueId = 41293905, RuntimeId=6551 },
					},
					new Item[]
					{
						new Item(1012, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(1012, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(1012, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(1012, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(1012, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(1012, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(1012, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(1012, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(1012, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 72 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(505, 0, 9){ UniqueId = 41293905, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(901, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 88 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-451, 0, 1){ UniqueId = 41293905, RuntimeId=6552 },
					},
					new Item[]
					{
						new Item(1010, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(1010, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(1010, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(1010, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(1010, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(1010, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(1010, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(1010, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(1010, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 107 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(567, 1, 1){ UniqueId = 41293905, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(70, 28, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(70, 28, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(70, 28, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(70, 28, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(70, 28, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(70, 28, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 498 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-427, 0, 1){ UniqueId = 41293905, RuntimeId=6553 },
					},
					new List<Item>
					{
						new Item(823, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(792, 0, 2){ UniqueId = 41293905, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 12 },
				new ShapedRecipe(2, 1,
					new List<Item>
					{
						new Item(171, 0, 3){ UniqueId = 41293905, RuntimeId=977 },
					},
					new Item[]
					{
						new Item(70, 28, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(70, 28, 2){ UniqueId = 41293905, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 991 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(171, 0, 8){ UniqueId = 41293905, RuntimeId=977 },
					},
					new Item[]
					{
						new Item(342, 0, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(792, 0, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293905, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 978 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(237, 0, 8){ UniqueId = 41293905, RuntimeId=3673 },
					},
					new List<Item>
					{
						new Item(792, 0, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(24, 0, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(24, 0, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(24, 0, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(24, 0, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(26, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(26, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(26, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(26, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 954 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(396, 0, 1){ UniqueId = 41293905, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(570, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 559 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(396, 0, 1){ UniqueId = 41293905, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(76, 0, 2){ UniqueId = 41293905, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 907 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(396, 0, 2){ UniqueId = 41293905, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(350, 8, 2){ UniqueId = 41293905, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 735 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(396, 0, 1){ UniqueId = 41293905, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(76, 8, 2){ UniqueId = 41293905, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 944 },
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(215, 0, 1){ UniqueId = 41293905, RuntimeId=6597 },
					},
					new Item[]
					{
						new Item(1046, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(588, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(588, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(1046, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 949 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-184, 0, 4){ UniqueId = 41293905, RuntimeId=6598 },
					},
					new Item[]
					{
						new Item(430, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(430, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(430, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(430, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(430, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(430, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 622 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(139, 0, 6){ UniqueId = 41293905, RuntimeId=1331 },
					},
					new Item[]
					{
						new Item(430, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(430, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(430, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(430, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(430, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(430, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 878 },
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(179, 0, 1){ UniqueId = 41293905, RuntimeId=6606 },
					},
					new Item[]
					{
						new Item(24, 2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(24, 2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(24, 2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(24, 2, 2){ UniqueId = 41293905, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 844 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(180, 0, 4){ UniqueId = 41293905, RuntimeId=6610 },
					},
					new Item[]
					{
						new Item(358, 0, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(358, 0, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(358, 0, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(358, 0, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(358, 0, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(358, 0, 2){ UniqueId = 41293905, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 712 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(139, 0, 6){ UniqueId = 41293905, RuntimeId=1330 },
					},
					new Item[]
					{
						new Item(358, 0, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(358, 0, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(358, 0, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(358, 0, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(358, 0, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(358, 0, 2){ UniqueId = 41293905, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 876 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(241, 0, 8){ UniqueId = 41293905, RuntimeId=7006 },
					},
					new Item[]
					{
						new Item(40, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(792, 0, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 569 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(160, 0, 16){ UniqueId = 41293905, RuntimeId=7022 },
					},
					new Item[]
					{
						new Item(482, 28, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(482, 28, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(482, 28, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(482, 28, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(482, 28, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(482, 28, 2){ UniqueId = 41293905, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 441 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(160, 0, 8){ UniqueId = 41293905, RuntimeId=7022 },
					},
					new Item[]
					{
						new Item(204, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(792, 0, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 794 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(159, 0, 8){ UniqueId = 41293905, RuntimeId=7038 },
					},
					new Item[]
					{
						new Item(344, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(792, 0, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 754 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(373, 0, 9){ UniqueId = 41293905, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(304, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 597 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(152, 0, 1){ UniqueId = 41293905, RuntimeId=6618 },
					},
					new Item[]
					{
						new Item(746, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(746, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(746, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(746, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(746, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(746, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(746, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(746, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(746, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 435 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(123, 0, 1){ UniqueId = 41293905, RuntimeId=6619 },
					},
					new Item[]
					{
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(746, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(746, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(178, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(746, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(746, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 770 },
				new ShapedRecipe(1, 2,
					new List<Item>
					{
						new Item(76, 0, 1){ UniqueId = 41293905, RuntimeId=6621 },
					},
					new Item[]
					{
						new Item(746, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 871 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(419, 0, 1){ UniqueId = 41293905, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(152, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(152, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(2, 0, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(746, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(2, 0, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(2, 0, 2){ UniqueId = 41293905, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1002 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-272, 0, 1){ UniqueId = 41293905, RuntimeId=6672 },
					},
					new Item[]
					{
						new Item(577, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(178, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(577, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(577, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(178, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(577, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(577, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(178, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(577, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 266 },
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(24, 0, 1){ UniqueId = 41293905, RuntimeId=6679 },
					},
					new Item[]
					{
						new Item(24, 0, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(24, 0, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(24, 0, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(24, 0, 2){ UniqueId = 41293905, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 679 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(128, 0, 4){ UniqueId = 41293905, RuntimeId=6683 },
					},
					new Item[]
					{
						new Item(48, 0, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(48, 0, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(48, 0, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(48, 0, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(48, 0, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(48, 0, 2){ UniqueId = 41293905, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 554 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(139, 0, 6){ UniqueId = 41293905, RuntimeId=1323 },
					},
					new Item[]
					{
						new Item(48, 0, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(48, 0, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(48, 0, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(48, 0, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(48, 0, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(48, 0, 2){ UniqueId = 41293905, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 852 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-165, 0, 6){ UniqueId = 41293905, RuntimeId=6703 },
					},
					new Item[]
					{
						new Item(325, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(325, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(325, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(652, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(325, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(325, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(325, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 383 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(169, 0, 1){ UniqueId = 41293905, RuntimeId=6732 },
					},
					new Item[]
					{
						new Item(1130, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(1098, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(1130, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(1098, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(1098, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(1098, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(1130, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(1098, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(1130, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 580 },
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(421, 0, 1){ UniqueId = 41293905, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 413 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(355, 0, 1){ UniqueId = 41293905, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(10, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 471 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(355, 0, 1){ UniqueId = 41293905, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(483, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 311 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(355, 0, 1){ UniqueId = 41293905, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(485, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 235 },
				new ShapedRecipe(1, 3,
					new List<Item>
					{
						new Item(205, 0, 1){ UniqueId = 41293905, RuntimeId=7366 },
					},
					new Item[]
					{
						new Item(1132, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(108, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(1132, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 714 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(579, 0, 3){ UniqueId = 41293905, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(10, 8, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(10, 8, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(10, 8, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(10, 8, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(10, 8, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(10, 8, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 555 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(577, 0, 3){ UniqueId = 41293905, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(10, 4, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(10, 4, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(10, 4, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(10, 4, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(10, 4, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(10, 4, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 994 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(580, 0, 3){ UniqueId = 41293905, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(10, 10, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(10, 10, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(10, 10, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(10, 10, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(10, 10, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(10, 10, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 582 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(578, 0, 3){ UniqueId = 41293905, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(10, 6, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(10, 6, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(10, 6, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(10, 6, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(10, 6, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(10, 6, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 900 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(358, 0, 3){ UniqueId = 41293905, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(10, 0, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(10, 0, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(10, 0, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(10, 0, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(10, 0, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(10, 0, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 417 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(576, 0, 3){ UniqueId = 41293905, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(10, 2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(10, 2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(10, 2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(10, 2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(10, 2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(10, 2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 961 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(165, 0, 1){ UniqueId = 41293905, RuntimeId=6768 },
					},
					new Item[]
					{
						new Item(776, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(776, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(776, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(776, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(776, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(776, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(776, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(776, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(776, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 408 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(388, 0, 9){ UniqueId = 41293905, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(330, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 583 },
				new ShapedRecipe(2, 3,
					new List<Item>
					{
						new Item(-202, 0, 1){ UniqueId = 41293905, RuntimeId=6783 },
					},
					new Item[]
					{
						new Item(610, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 409 },
				new ShapedRecipe(2, 3,
					new List<Item>
					{
						new Item(-202, 0, 1){ UniqueId = 41293905, RuntimeId=6783 },
					},
					new Item[]
					{
						new Item(610, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 348 },
				new ShapedRecipe(2, 3,
					new List<Item>
					{
						new Item(-202, 0, 1){ UniqueId = 41293905, RuntimeId=6783 },
					},
					new Item[]
					{
						new Item(610, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 202 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-198, 0, 1){ UniqueId = 41293905, RuntimeId=6784 },
					},
					new Item[]
					{
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(34, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(34, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(122, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(34, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(34, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 782 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-198, 0, 1){ UniqueId = 41293905, RuntimeId=6784 },
					},
					new Item[]
					{
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(449, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(449, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(122, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(449, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(449, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 219 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-198, 0, 1){ UniqueId = 41293905, RuntimeId=6784 },
					},
					new Item[]
					{
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(324, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(324, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(122, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(324, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(324, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 992 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-198, 0, 1){ UniqueId = 41293905, RuntimeId=6784 },
					},
					new Item[]
					{
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(15, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(15, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(122, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(15, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(15, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 698 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-198, 0, 1){ UniqueId = 41293905, RuntimeId=6784 },
					},
					new Item[]
					{
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(11, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(11, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(122, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(11, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(11, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 540 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-198, 0, 1){ UniqueId = 41293905, RuntimeId=6784 },
					},
					new Item[]
					{
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(479, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(479, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(122, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(479, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(479, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 349 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-198, 0, 1){ UniqueId = 41293905, RuntimeId=6784 },
					},
					new Item[]
					{
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(17, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(17, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(122, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(17, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(17, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 595 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-198, 0, 1){ UniqueId = 41293905, RuntimeId=6784 },
					},
					new Item[]
					{
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(13, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(13, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(122, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(13, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(13, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 987 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-198, 0, 1){ UniqueId = 41293905, RuntimeId=6784 },
					},
					new Item[]
					{
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(19, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(19, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(122, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(19, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(19, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 416 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-198, 0, 1){ UniqueId = 41293905, RuntimeId=6784 },
					},
					new Item[]
					{
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(9, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(9, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(122, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(9, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(9, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 613 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-198, 0, 1){ UniqueId = 41293905, RuntimeId=6784 },
					},
					new Item[]
					{
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(481, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(481, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(122, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(481, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(481, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 196 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-198, 0, 1){ UniqueId = 41293905, RuntimeId=6784 },
					},
					new Item[]
					{
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(451, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(451, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(122, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(451, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(451, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 307 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-185, 0, 4){ UniqueId = 41293905, RuntimeId=6791 },
					},
					new Item[]
					{
						new Item(310, 6, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(310, 6, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(310, 6, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(310, 6, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(310, 6, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(310, 6, 2){ UniqueId = 41293905, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 955 },
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(179, 0, 4){ UniqueId = 41293905, RuntimeId=6608 },
					},
					new Item[]
					{
						new Item(358, 0, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(358, 0, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(358, 0, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(358, 0, 2){ UniqueId = 41293905, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 802 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-176, 0, 4){ UniqueId = 41293905, RuntimeId=6799 },
					},
					new Item[]
					{
						new Item(358, 6, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(358, 6, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(358, 6, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(358, 6, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(358, 6, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(358, 6, 2){ UniqueId = 41293905, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 575 },
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(24, 0, 4){ UniqueId = 41293905, RuntimeId=6681 },
					},
					new Item[]
					{
						new Item(48, 0, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(48, 0, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(48, 0, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(48, 0, 2){ UniqueId = 41293905, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 537 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-177, 0, 4){ UniqueId = 41293905, RuntimeId=6807 },
					},
					new Item[]
					{
						new Item(48, 6, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(48, 6, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(48, 6, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(48, 6, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(48, 6, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(48, 6, 2){ UniqueId = 41293905, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 401 },
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(80, 0, 1){ UniqueId = 41293905, RuntimeId=6816 },
					},
					new Item[]
					{
						new Item(748, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(748, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(748, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(748, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 848 },
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(78, 0, 6){ UniqueId = 41293905, RuntimeId=6817 },
					},
					new Item[]
					{
						new Item(160, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(160, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(160, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 489 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(620, 0, 1){ UniqueId = 41293905, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(449, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(471, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(449, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(449, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 287 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(620, 0, 1){ UniqueId = 41293905, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(449, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(176, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(449, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(449, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 369 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(620, 0, 1){ UniqueId = 41293909, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(34, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(176, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(34, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293905, RuntimeId=0 },
						new Item(34, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 326 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(620, 0, 1){ UniqueId = 41293909, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(324, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(176, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(324, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(324, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 312 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(620, 0, 1){ UniqueId = 41293909, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(15, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(176, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(15, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(15, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 335 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(620, 0, 1){ UniqueId = 41293909, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(11, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(176, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(11, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(11, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 295 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(620, 0, 1){ UniqueId = 41293909, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(17, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(176, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(17, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(17, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 221 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(620, 0, 1){ UniqueId = 41293909, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(13, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(176, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(13, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(13, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 248 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(620, 0, 1){ UniqueId = 41293909, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(19, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(176, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(19, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(19, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 327 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(620, 0, 1){ UniqueId = 41293909, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(9, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(176, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(9, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(9, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 343 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(620, 0, 1){ UniqueId = 41293909, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(423, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(176, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(423, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(423, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 285 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(620, 0, 1){ UniqueId = 41293909, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(34, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(471, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(34, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(34, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 253 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(620, 0, 1){ UniqueId = 41293909, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(324, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(471, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(324, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(324, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 344 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(620, 0, 1){ UniqueId = 41293909, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(15, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(471, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(15, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(15, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 242 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(620, 0, 1){ UniqueId = 41293909, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(11, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(471, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(11, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(11, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 338 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(620, 0, 1){ UniqueId = 41293909, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(17, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(471, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(17, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(17, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 364 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(620, 0, 1){ UniqueId = 41293909, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(13, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(471, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(13, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(13, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 210 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(620, 0, 1){ UniqueId = 41293909, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(19, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(471, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(19, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(19, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 246 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(620, 0, 1){ UniqueId = 41293909, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(9, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(471, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(9, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(9, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 271 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(620, 0, 1){ UniqueId = 41293909, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(423, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(471, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(423, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(423, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 292 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(620, 0, 1){ UniqueId = 41293909, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(479, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(471, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(479, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(479, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 267 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(620, 0, 1){ UniqueId = 41293909, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(479, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(176, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(479, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(479, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 336 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(620, 0, 1){ UniqueId = 41293909, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(481, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(471, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(481, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(481, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 315 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(620, 0, 1){ UniqueId = 41293909, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(481, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(176, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(481, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(481, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 199 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(620, 0, 1){ UniqueId = 41293909, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(451, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(471, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(451, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(451, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 226 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(620, 0, 1){ UniqueId = 41293909, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(451, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(176, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(451, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(451, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 279 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-269, 0, 1){ UniqueId = 41293909, RuntimeId=6857 },
					},
					new Item[]
					{
						new Item(1138, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(1138, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(1138, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(1138, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(535, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(1138, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(1138, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(1138, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(1138, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 198 },
				new ShapedRecipe(1, 3,
					new List<Item>
					{
						new Item(-268, 0, 4){ UniqueId = 41293909, RuntimeId=6861 },
					},
					new Item[]
					{
						new Item(604, 0, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(176, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 293 },
				new ShapedRecipe(1, 3,
					new List<Item>
					{
						new Item(-268, 0, 4){ UniqueId = 41293909, RuntimeId=6861 },
					},
					new Item[]
					{
						new Item(604, 0, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(471, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 320 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(434, 0, 1){ UniqueId = 41293909, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(850, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(850, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(850, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(850, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(544, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(850, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(850, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(850, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(850, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 829 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(378, 0, 1){ UniqueId = 41293909, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(10, 2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(10, 2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(10, 2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(618, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(10, 2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(10, 2, 2){ UniqueId = 41293909, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 652 },
				new ShapedRecipe(2, 3,
					new List<Item>
					{
						new Item(553, 0, 3){ UniqueId = 41293909, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(10, 2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(10, 2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(10, 2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(10, 2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(10, 2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(10, 2, 2){ UniqueId = 41293909, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 773 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(85, 0, 3){ UniqueId = 41293909, RuntimeId=4774 },
					},
					new Item[]
					{
						new Item(10, 2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(10, 2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(10, 2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(10, 2, 2){ UniqueId = 41293909, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 636 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(183, 0, 1){ UniqueId = 41293909, RuntimeId=6914 },
					},
					new Item[]
					{
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(10, 2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(10, 2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 708 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(5, 0, 4){ UniqueId = 41293909, RuntimeId=5771 },
					},
					new Item[]
					{
						new Item(34, 2, 2){ UniqueId = 41293909, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 516 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(5, 0, 4){ UniqueId = 41293909, RuntimeId=5771 },
					},
					new Item[]
					{
						new Item(9, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 839 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(5, 0, 4){ UniqueId = 41293909, RuntimeId=5771 },
					},
					new Item[]
					{
						new Item(423, 18, 2){ UniqueId = 41293909, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 959 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(5, 0, 4){ UniqueId = 41293909, RuntimeId=5771 },
					},
					new Item[]
					{
						new Item(423, 2, 2){ UniqueId = 41293909, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 865 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(134, 0, 4){ UniqueId = 41293909, RuntimeId=6946 },
					},
					new Item[]
					{
						new Item(10, 2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(10, 2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(10, 2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(10, 2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(10, 2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(10, 2, 2){ UniqueId = 41293909, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 771 },
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(-212, 0, 3){ UniqueId = 41293909, RuntimeId=7712 },
					},
					new Item[]
					{
						new Item(34, 2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(34, 2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(34, 2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(34, 2, 2){ UniqueId = 41293909, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 822 },
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(-212, 0, 3){ UniqueId = 41293909, RuntimeId=7718 },
					},
					new Item[]
					{
						new Item(9, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(9, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(9, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(9, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 810 },
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(158, 0, 6){ UniqueId = 41293909, RuntimeId=7808 },
					},
					new Item[]
					{
						new Item(10, 2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(10, 2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(10, 2, 2){ UniqueId = 41293909, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 903 },
				new ShapedRecipe(1, 3,
					new List<Item>
					{
						new Item(624, 0, 1){ UniqueId = 41293909, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(1246, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(1008, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(1008, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 111 },
				new ShapedRecipe(1, 2,
					new List<Item>
					{
						new Item(320, 0, 4){ UniqueId = 41293909, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(483, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 357 },
				new ShapedRecipe(1, 2,
					new List<Item>
					{
						new Item(320, 0, 4){ UniqueId = 41293909, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(485, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 306 },
				new ShapedRecipe(1, 2,
					new List<Item>
					{
						new Item(29, 0, 1){ UniqueId = 41293909, RuntimeId=7073 },
					},
					new Item[]
					{
						new Item(776, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(66, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 615 },
				new ShapedRecipe(2, 3,
					new List<Item>
					{
						new Item(315, 0, 1){ UniqueId = 41293909, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(8, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(8, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(8, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 912 },
				new ShapedRecipe(2, 3,
					new List<Item>
					{
						new Item(315, 0, 1){ UniqueId = 41293909, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(545, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(545, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(545, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 181 },
				new ShapedRecipe(2, 3,
					new List<Item>
					{
						new Item(315, 0, 1){ UniqueId = 41293909, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(757, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(757, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(757, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 47 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(109, 0, 4){ UniqueId = 41293909, RuntimeId=7091 },
					},
					new Item[]
					{
						new Item(196, 0, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(196, 0, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(196, 0, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(196, 0, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(196, 0, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(196, 0, 2){ UniqueId = 41293909, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 584 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(139, 0, 6){ UniqueId = 41293909, RuntimeId=1325 },
					},
					new Item[]
					{
						new Item(196, 0, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(196, 0, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(196, 0, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(196, 0, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(196, 0, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(196, 0, 2){ UniqueId = 41293909, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 778 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(77, 0, 1){ UniqueId = 41293909, RuntimeId=7099 },
					},
					new Item[]
					{
						new Item(2, 0, 2){ UniqueId = 41293909, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 464 },
				new ShapedRecipe(2, 3,
					new List<Item>
					{
						new Item(330, 0, 1){ UniqueId = 41293909, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(8, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(8, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 524 },
				new ShapedRecipe(2, 3,
					new List<Item>
					{
						new Item(330, 0, 1){ UniqueId = 41293909, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(545, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(545, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 186 },
				new ShapedRecipe(2, 3,
					new List<Item>
					{
						new Item(330, 0, 1){ UniqueId = 41293909, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(757, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(757, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 52 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(314, 0, 1){ UniqueId = 41293909, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(8, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(8, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(8, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 937 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(314, 0, 1){ UniqueId = 41293909, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(545, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(545, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(545, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 183 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(314, 0, 1){ UniqueId = 41293909, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(757, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(757, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(757, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 33 },
				new ShapedRecipe(2, 1,
					new List<Item>
					{
						new Item(70, 0, 1){ UniqueId = 41293909, RuntimeId=7111 },
					},
					new Item[]
					{
						new Item(2, 0, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(2, 0, 2){ UniqueId = 41293909, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 637 },
				new ShapedRecipe(1, 3,
					new List<Item>
					{
						new Item(313, 0, 1){ UniqueId = 41293909, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(8, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 598 },
				new ShapedRecipe(1, 3,
					new List<Item>
					{
						new Item(313, 0, 1){ UniqueId = 41293909, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(545, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 184 },
				new ShapedRecipe(1, 3,
					new List<Item>
					{
						new Item(313, 0, 1){ UniqueId = 41293909, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(757, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 130 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-180, 0, 4){ UniqueId = 41293909, RuntimeId=5681 },
					},
					new Item[]
					{
						new Item(2, 0, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(2, 0, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(2, 0, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(2, 0, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(2, 0, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(2, 0, 2){ UniqueId = 41293909, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 612 },
				new ShapedRecipe(1, 3,
					new List<Item>
					{
						new Item(312, 0, 1){ UniqueId = 41293909, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(8, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(8, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 701 },
				new ShapedRecipe(1, 3,
					new List<Item>
					{
						new Item(312, 0, 1){ UniqueId = 41293909, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(545, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(545, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 185 },
				new ShapedRecipe(1, 3,
					new List<Item>
					{
						new Item(312, 0, 1){ UniqueId = 41293909, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(757, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(757, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 137 },
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(98, 0, 4){ UniqueId = 41293909, RuntimeId=7193 },
					},
					new Item[]
					{
						new Item(2, 0, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(2, 0, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(2, 0, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(2, 0, 2){ UniqueId = 41293909, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 826 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(-197, 0, 1){ UniqueId = 41293909, RuntimeId=7199 },
					},
					new Item[]
					{
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(2, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(610, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(2, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(2, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 935 },
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293909, RuntimeId=7819 },
					},
					new Item[]
					{
						new Item(652, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(652, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(652, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(652, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 910 },
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(-300, 0, 3){ UniqueId = 41293909, RuntimeId=7211 },
					},
					new Item[]
					{
						new Item(479, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(479, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(479, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(479, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 264 },
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(-301, 0, 3){ UniqueId = 41293909, RuntimeId=7229 },
					},
					new Item[]
					{
						new Item(481, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(481, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(481, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(481, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 237 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(416, 0, 1){ UniqueId = 41293909, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(770, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 634 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(589, 7, 1){ UniqueId = 41293909, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(78, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(80, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(642, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(76, 4, 2){ UniqueId = 41293909, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 670 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(589, 3, 1){ UniqueId = 41293909, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(78, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(80, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(642, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(76, 6, 2){ UniqueId = 41293909, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 811 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(589, 6, 1){ UniqueId = 41293909, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(78, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(80, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(642, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(76, 2, 2){ UniqueId = 41293909, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 738 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(589, 1, 1){ UniqueId = 41293909, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(78, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(80, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(642, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(76, 18, 2){ UniqueId = 41293909, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 726 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(589, 5, 1){ UniqueId = 41293909, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(78, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(80, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(642, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(74, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 420 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(589, 4, 1){ UniqueId = 41293909, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(78, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(80, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(642, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(76, 20, 2){ UniqueId = 41293909, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 662 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(589, 8, 1){ UniqueId = 41293909, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(78, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(80, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(642, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(76, 16, 2){ UniqueId = 41293909, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 461 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(589, 0, 1){ UniqueId = 41293909, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(78, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(80, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(642, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(76, 0, 2){ UniqueId = 41293909, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 511 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(589, 2, 1){ UniqueId = 41293909, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(78, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(80, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(642, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(76, 10, 2){ UniqueId = 41293909, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 542 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(589, 2, 1){ UniqueId = 41293909, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(78, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(80, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(642, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(76, 14, 2){ UniqueId = 41293909, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 764 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(589, 2, 1){ UniqueId = 41293909, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(78, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(80, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(642, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(76, 8, 2){ UniqueId = 41293909, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 818 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(589, 2, 1){ UniqueId = 41293909, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(78, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(80, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(642, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(76, 12, 2){ UniqueId = 41293909, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 387 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(589, 9, 1){ UniqueId = 41293909, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(78, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(80, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(642, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(431, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 640 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-239, 0, 1){ UniqueId = 41293909, RuntimeId=7255 },
					},
					new Item[]
					{
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(746, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(746, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(340, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(746, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(746, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 225 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-334, 0, 2){ UniqueId = 41293909, RuntimeId=7256 },
					},
					new Item[]
					{
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(1246, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(1246, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(1246, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(1246, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 134 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(46, 0, 1){ UniqueId = 41293913, RuntimeId=7257 },
					},
					new Item[]
					{
						new Item(656, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(24, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(656, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(24, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(656, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(24, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(656, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(24, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
						new Item(656, -2, 2){ UniqueId = 41293909, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 611 },
				new ShapedRecipe(1, 2,
					new List<Item>
					{
						new Item(525, 0, 1){ UniqueId = 41293913, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(92, 0, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(740, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 505 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(146, 0, 1){ UniqueId = 41293913, RuntimeId=7283 },
					},
					new List<Item>
					{
						new Item(108, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(262, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 809 },
				new ShapedRecipe(1, 3,
					new List<Item>
					{
						new Item(131, 0, 1){ UniqueId = 41293913, RuntimeId=7305 },
					},
					new Item[]
					{
						new Item(610, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 265 },
				new ShapedRecipe(1, 3,
					new List<Item>
					{
						new Item(131, 0, 1){ UniqueId = 41293913, RuntimeId=7305 },
					},
					new Item[]
					{
						new Item(610, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 247 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(573, 0, 1){ UniqueId = 41293913, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(1144, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(1144, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(1144, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(1144, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(1144, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 750 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(-261, 0, 1){ UniqueId = 41293913, RuntimeId=7434 },
					},
					new Item[]
					{
						new Item(485, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 288 },
				new ShapedRecipe(2, 3,
					new List<Item>
					{
						new Item(615, 0, 3){ UniqueId = 41293913, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(485, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 224 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(-257, 0, 3){ UniqueId = 41293913, RuntimeId=7480 },
					},
					new Item[]
					{
						new Item(485, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 308 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(-259, 0, 1){ UniqueId = 41293913, RuntimeId=7481 },
					},
					new Item[]
					{
						new Item(640, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 281 },
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(616, 0, 1){ UniqueId = 41293913, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(784, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(457, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 209 },
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(-298, 0, 3){ UniqueId = 41293913, RuntimeId=7498 },
					},
					new Item[]
					{
						new Item(451, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(451, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(451, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(451, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 188 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(-243, 0, 4){ UniqueId = 41293913, RuntimeId=7502 },
					},
					new Item[]
					{
						new Item(451, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 347 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(-243, 0, 4){ UniqueId = 41293913, RuntimeId=7502 },
					},
					new Item[]
					{
						new Item(481, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 207 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(-243, 0, 4){ UniqueId = 41293913, RuntimeId=7502 },
					},
					new Item[]
					{
						new Item(601, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 354 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(-243, 0, 4){ UniqueId = 41293913, RuntimeId=7502 },
					},
					new Item[]
					{
						new Item(595, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 284 },
				new ShapedRecipe(2, 1,
					new List<Item>
					{
						new Item(-263, 0, 1){ UniqueId = 41293913, RuntimeId=7503 },
					},
					new Item[]
					{
						new Item(485, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 299 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(613, 0, 3){ UniqueId = 41293913, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(485, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 213 },
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(-265, 0, 6){ UniqueId = 41293913, RuntimeId=7520 },
					},
					new Item[]
					{
						new Item(485, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 358 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-255, 0, 4){ UniqueId = 41293913, RuntimeId=7522 },
					},
					new Item[]
					{
						new Item(485, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 195 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(-247, 0, 2){ UniqueId = 41293913, RuntimeId=7549 },
					},
					new Item[]
					{
						new Item(485, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 296 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-344, 0, 1){ UniqueId = 41293913, RuntimeId=7589 },
					},
					new List<Item>
					{
						new Item(679, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(1180, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 34 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-351, 0, 1){ UniqueId = 41293913, RuntimeId=7590 },
					},
					new List<Item>
					{
						new Item(693, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(1180, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 78 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-365, 0, 1){ UniqueId = 41293913, RuntimeId=7591 },
					},
					new List<Item>
					{
						new Item(721, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(1180, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 54 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-358, 0, 1){ UniqueId = 41293913, RuntimeId=7593 },
					},
					new List<Item>
					{
						new Item(707, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(1180, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 129 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-345, 0, 1){ UniqueId = 41293913, RuntimeId=7603 },
					},
					new List<Item>
					{
						new Item(681, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(1180, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 96 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-352, 0, 1){ UniqueId = 41293913, RuntimeId=7604 },
					},
					new List<Item>
					{
						new Item(695, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(1180, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 106 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-366, 0, 1){ UniqueId = 41293913, RuntimeId=7605 },
					},
					new List<Item>
					{
						new Item(723, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(1180, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 57 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-359, 0, 1){ UniqueId = 41293913, RuntimeId=7607 },
					},
					new List<Item>
					{
						new Item(709, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(1180, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 143 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-446, 0, 1){ UniqueId = 41293913, RuntimeId=7617 },
					},
					new List<Item>
					{
						new Item(685, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(1180, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 87 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-447, 0, 1){ UniqueId = 41293913, RuntimeId=7618 },
					},
					new List<Item>
					{
						new Item(699, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(1180, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 26 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-449, 0, 1){ UniqueId = 41293913, RuntimeId=7619 },
					},
					new List<Item>
					{
						new Item(727, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(1180, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 90 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-448, 0, 1){ UniqueId = 41293913, RuntimeId=7621 },
					},
					new List<Item>
					{
						new Item(713, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(1180, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 56 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-346, 0, 1){ UniqueId = 41293913, RuntimeId=7631 },
					},
					new List<Item>
					{
						new Item(683, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(1180, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 84 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-353, 0, 1){ UniqueId = 41293913, RuntimeId=7632 },
					},
					new List<Item>
					{
						new Item(697, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(1180, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 97 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-367, 0, 1){ UniqueId = 41293913, RuntimeId=7633 },
					},
					new List<Item>
					{
						new Item(725, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(1180, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 37 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-360, 0, 1){ UniqueId = 41293913, RuntimeId=7635 },
					},
					new List<Item>
					{
						new Item(711, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(1180, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 28 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(334, 0, 9){ UniqueId = 41293913, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(340, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 444 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(567, 15, 1){ UniqueId = 41293913, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(70, 0, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(70, 0, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(70, 0, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(70, 0, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(70, 0, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(70, 0, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 757 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-413, 0, 1){ UniqueId = 41293913, RuntimeId=7694 },
					},
					new List<Item>
					{
						new Item(823, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(820, 0, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 13 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-413, 0, 1){ UniqueId = 41293913, RuntimeId=7694 },
					},
					new List<Item>
					{
						new Item(823, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(822, 0, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 8 },
				new ShapedRecipe(2, 1,
					new List<Item>
					{
						new Item(171, 0, 3){ UniqueId = 41293913, RuntimeId=963 },
					},
					new Item[]
					{
						new Item(70, 0, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(70, 0, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 823 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(237, 0, 8){ UniqueId = 41293913, RuntimeId=3659 },
					},
					new List<Item>
					{
						new Item(820, 0, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(24, 0, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(24, 0, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(24, 0, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(24, 0, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(26, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(26, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(26, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(26, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 430 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(237, 0, 8){ UniqueId = 41293913, RuntimeId=3659 },
					},
					new List<Item>
					{
						new Item(822, 0, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(24, 0, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(24, 0, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(24, 0, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(24, 0, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(26, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(26, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(26, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(26, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 948 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(410, 0, 1){ UniqueId = 41293913, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(822, 0, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 502 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(410, 0, 1){ UniqueId = 41293913, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(76, 20, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 711 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(241, 0, 8){ UniqueId = 41293913, RuntimeId=6992 },
					},
					new Item[]
					{
						new Item(40, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(820, 0, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 835 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(241, 0, 8){ UniqueId = 41293913, RuntimeId=6992 },
					},
					new Item[]
					{
						new Item(40, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(822, 0, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 931 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(160, 0, 16){ UniqueId = 41293913, RuntimeId=7008 },
					},
					new Item[]
					{
						new Item(482, 0, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(482, 0, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(482, 0, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(482, 0, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(482, 0, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(482, 0, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 469 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(160, 0, 8){ UniqueId = 41293913, RuntimeId=7008 },
					},
					new Item[]
					{
						new Item(204, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(820, 0, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 541 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(159, 0, 8){ UniqueId = 41293913, RuntimeId=7024 },
					},
					new Item[]
					{
						new Item(344, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(820, 0, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 474 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(159, 0, 8){ UniqueId = 41293913, RuntimeId=7024 },
					},
					new Item[]
					{
						new Item(344, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(822, 0, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 676 },
				new ShapedRecipe(2, 3,
					new List<Item>
					{
						new Item(311, 0, 1){ UniqueId = 41293913, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(483, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 208 },
				new ShapedRecipe(2, 3,
					new List<Item>
					{
						new Item(311, 0, 1){ UniqueId = 41293913, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(485, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 241 },
				new ShapedRecipe(2, 3,
					new List<Item>
					{
						new Item(359, 0, 3){ UniqueId = 41293913, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(10, 0, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(10, 0, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(10, 0, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(10, 0, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(10, 0, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(10, 0, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 677 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(329, 0, 1){ UniqueId = 41293913, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(483, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 273 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(329, 0, 1){ UniqueId = 41293913, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(485, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 332 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(310, 0, 1){ UniqueId = 41293913, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(483, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 231 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(310, 0, 1){ UniqueId = 41293913, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(485, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 325 },
				new ShapedRecipe(1, 3,
					new List<Item>
					{
						new Item(309, 0, 1){ UniqueId = 41293913, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(483, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 309 },
				new ShapedRecipe(1, 3,
					new List<Item>
					{
						new Item(309, 0, 1){ UniqueId = 41293913, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(485, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 192 },
				new ShapedRecipe(1, 3,
					new List<Item>
					{
						new Item(308, 0, 1){ UniqueId = 41293913, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(483, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(483, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 280 },
				new ShapedRecipe(1, 3,
					new List<Item>
					{
						new Item(308, 0, 1){ UniqueId = 41293913, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(485, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(485, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 301 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(510, 0, 1){ UniqueId = 41293913, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(774, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(826, 0, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(654, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 571 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(567, 11, 1){ UniqueId = 41293913, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(70, 8, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(70, 8, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(70, 8, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(70, 8, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(70, 8, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(70, 8, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 995 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-417, 0, 1){ UniqueId = 41293913, RuntimeId=7835 },
					},
					new List<Item>
					{
						new Item(823, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(812, 0, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 6 },
				new ShapedRecipe(2, 1,
					new List<Item>
					{
						new Item(171, 0, 3){ UniqueId = 41293913, RuntimeId=967 },
					},
					new Item[]
					{
						new Item(70, 8, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(70, 8, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 939 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(171, 0, 8){ UniqueId = 41293913, RuntimeId=967 },
					},
					new Item[]
					{
						new Item(342, 0, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(812, 0, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(342, 0, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 425 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(237, 0, 8){ UniqueId = 41293913, RuntimeId=3663 },
					},
					new List<Item>
					{
						new Item(812, 0, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(24, 0, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(24, 0, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(24, 0, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(24, 0, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(26, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(26, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(26, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(26, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 857 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(406, 0, 1){ UniqueId = 41293913, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(74, 0, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 617 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(406, 0, 2){ UniqueId = 41293913, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(350, 0, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 501 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(241, 0, 8){ UniqueId = 41293913, RuntimeId=6996 },
					},
					new Item[]
					{
						new Item(40, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(812, 0, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(40, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 938 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(160, 0, 16){ UniqueId = 41293913, RuntimeId=7012 },
					},
					new Item[]
					{
						new Item(482, 8, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(482, 8, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(482, 8, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(482, 8, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(482, 8, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(482, 8, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 633 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(160, 0, 8){ UniqueId = 41293913, RuntimeId=7012 },
					},
					new Item[]
					{
						new Item(204, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(812, 0, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(204, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 902 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(159, 0, 8){ UniqueId = 41293913, RuntimeId=7028 },
					},
					new Item[]
					{
						new Item(344, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(812, 0, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(344, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 864 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(53, 0, 4){ UniqueId = 41293913, RuntimeId=5690 },
					},
					new Item[]
					{
						new Item(10, 0, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(10, 0, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(10, 0, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(10, 0, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(10, 0, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(10, 0, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2041 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(519, 0, 3){ UniqueId = 41293913, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(772, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(656, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(1040, 0, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2092 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(519, 0, 3){ UniqueId = 41293913, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(772, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(656, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(1040, 20, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2072 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(519, 0, 3){ UniqueId = 41293913, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(772, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(656, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(1040, 22, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2070 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(519, 0, 3){ UniqueId = 41293913, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(772, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(656, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(1040, 24, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2068 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(519, 0, 3){ UniqueId = 41293913, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(772, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(656, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(1040, 26, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2066 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(519, 0, 3){ UniqueId = 41293913, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(772, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(656, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(1040, 28, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2064 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(519, 0, 3){ UniqueId = 41293913, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(772, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(656, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(1040, 30, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2062 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(519, 0, 3){ UniqueId = 41293913, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(772, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(656, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(1040, 0, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2060 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(519, 0, 3){ UniqueId = 41293913, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(772, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(656, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(1040, 6, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2058 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(519, 0, 3){ UniqueId = 41293913, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(772, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(656, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(1040, 8, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2056 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(519, 0, 3){ UniqueId = 41293913, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(772, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(656, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(1040, 30, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2054 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(519, 0, 3){ UniqueId = 41293913, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(772, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(656, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(1040, 2, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2090 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(519, 0, 3){ UniqueId = 41293913, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(772, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(656, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(1040, 4, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2088 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(519, 0, 3){ UniqueId = 41293913, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(772, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(656, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(1040, 6, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2086 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(519, 0, 3){ UniqueId = 41293913, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(772, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(656, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(1040, 8, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2084 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(519, 0, 3){ UniqueId = 41293913, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(772, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(656, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(1040, 10, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2082 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(519, 0, 3){ UniqueId = 41293913, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(772, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(656, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(1040, 12, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2080 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(519, 0, 3){ UniqueId = 41293913, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(772, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(656, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(1040, 14, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2078 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(519, 0, 3){ UniqueId = 41293913, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(772, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(656, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(1040, 16, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2076 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(519, 0, 3){ UniqueId = 41293913, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(772, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(656, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(1040, 18, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2074 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(520, 0, 1){ UniqueId = 41293913, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(656, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(826, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2091 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(520, 10, 1){ UniqueId = 41293913, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(656, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(810, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2071 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(520, 11, 1){ UniqueId = 41293913, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(656, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(812, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2069 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(520, 12, 1){ UniqueId = 41293913, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(656, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(814, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2067 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(520, 13, 1){ UniqueId = 41293913, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(656, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(816, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2065 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(520, 14, 1){ UniqueId = 41293913, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(656, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(818, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2063 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(520, 15, 1){ UniqueId = 41293913, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(656, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(822, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2061 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(520, 0, 1){ UniqueId = 41293913, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(656, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(790, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2059 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(520, 3, 1){ UniqueId = 41293913, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(656, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(796, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2057 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(520, 4, 1){ UniqueId = 41293913, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(656, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(798, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2055 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(520, 15, 1){ UniqueId = 41293913, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(656, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(820, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2053 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(520, 1, 1){ UniqueId = 41293913, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(656, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(792, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2089 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(520, 2, 1){ UniqueId = 41293913, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(656, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(794, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2087 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(520, 3, 1){ UniqueId = 41293913, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(656, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(824, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2085 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(520, 4, 1){ UniqueId = 41293913, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(656, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
						new Item(828, -2, 2){ UniqueId = 41293913, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2083 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(520, 5, 1){ UniqueId = 41293917, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(656, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(800, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2081 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(520, 6, 1){ UniqueId = 41293917, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(656, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(802, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2079 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(520, 7, 1){ UniqueId = 41293917, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(656, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(804, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2077 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(520, 8, 1){ UniqueId = 41293917, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(656, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(806, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2075 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(520, 9, 1){ UniqueId = 41293917, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(656, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(808, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2073 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(519, 0, 3){ UniqueId = 41293917, RuntimeId=0 },
					},
					new List<Item>
					{
						new Item(772, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(656, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2052 },
				new MultiRecipe() { Id = new UUID("00000000-0000-0000-0000-000000000001"), UniqueId = 2147 }, // 00000000-0000-0000-0000-000000000001
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6749 },
					},
					new List<Item>
					{
						new Item(436, 10, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(826, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1623 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6749 },
					},
					new List<Item>
					{
						new Item(436, 8, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(826, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1624 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6749 },
					},
					new List<Item>
					{
						new Item(436, 6, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(826, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1625 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6749 },
					},
					new List<Item>
					{
						new Item(436, 4, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(826, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1626 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6749 },
					},
					new List<Item>
					{
						new Item(436, 2, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(826, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1627 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6749 },
					},
					new List<Item>
					{
						new Item(436, 0, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(826, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1628 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6749 },
					},
					new List<Item>
					{
						new Item(436, 28, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(826, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1614 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6749 },
					},
					new List<Item>
					{
						new Item(436, 26, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(826, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1615 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6749 },
					},
					new List<Item>
					{
						new Item(436, 24, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(826, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1616 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6749 },
					},
					new List<Item>
					{
						new Item(436, 22, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(826, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1617 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6749 },
					},
					new List<Item>
					{
						new Item(436, 20, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(826, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1618 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6749 },
					},
					new List<Item>
					{
						new Item(436, 18, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(826, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1619 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6749 },
					},
					new List<Item>
					{
						new Item(436, 16, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(826, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1620 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6749 },
					},
					new List<Item>
					{
						new Item(436, 14, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(826, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1621 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6749 },
					},
					new List<Item>
					{
						new Item(436, 12, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(826, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1622 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6739 },
					},
					new List<Item>
					{
						new Item(436, 30, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(810, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1454 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6739 },
					},
					new List<Item>
					{
						new Item(436, 8, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(810, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1464 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6739 },
					},
					new List<Item>
					{
						new Item(436, 6, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(810, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1465 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6739 },
					},
					new List<Item>
					{
						new Item(436, 4, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(810, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1466 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6739 },
					},
					new List<Item>
					{
						new Item(436, 2, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(810, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1467 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6739 },
					},
					new List<Item>
					{
						new Item(436, 0, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(810, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1468 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6739 },
					},
					new List<Item>
					{
						new Item(436, 28, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(810, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1455 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6739 },
					},
					new List<Item>
					{
						new Item(436, 26, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(810, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1456 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6739 },
					},
					new List<Item>
					{
						new Item(436, 24, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(810, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1457 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6739 },
					},
					new List<Item>
					{
						new Item(436, 22, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(810, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1458 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6739 },
					},
					new List<Item>
					{
						new Item(436, 20, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(810, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1459 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6739 },
					},
					new List<Item>
					{
						new Item(436, 18, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(810, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1460 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6739 },
					},
					new List<Item>
					{
						new Item(436, 16, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(810, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1461 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6739 },
					},
					new List<Item>
					{
						new Item(436, 14, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(810, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1462 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6739 },
					},
					new List<Item>
					{
						new Item(436, 12, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(810, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1463 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6738 },
					},
					new List<Item>
					{
						new Item(436, 30, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(812, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1438 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6738 },
					},
					new List<Item>
					{
						new Item(436, 10, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(812, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1448 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6738 },
					},
					new List<Item>
					{
						new Item(436, 6, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(812, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1449 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6738 },
					},
					new List<Item>
					{
						new Item(436, 4, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(812, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1450 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6738 },
					},
					new List<Item>
					{
						new Item(436, 2, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(812, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1451 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6738 },
					},
					new List<Item>
					{
						new Item(436, 0, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(812, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1452 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6738 },
					},
					new List<Item>
					{
						new Item(436, 28, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(812, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1439 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6738 },
					},
					new List<Item>
					{
						new Item(436, 26, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(812, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1440 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6738 },
					},
					new List<Item>
					{
						new Item(436, 24, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(812, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1441 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6738 },
					},
					new List<Item>
					{
						new Item(436, 22, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(812, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1442 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6738 },
					},
					new List<Item>
					{
						new Item(436, 20, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(812, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1443 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6738 },
					},
					new List<Item>
					{
						new Item(436, 18, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(812, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1444 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6738 },
					},
					new List<Item>
					{
						new Item(436, 16, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(812, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1445 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6738 },
					},
					new List<Item>
					{
						new Item(436, 14, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(812, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1446 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6738 },
					},
					new List<Item>
					{
						new Item(436, 12, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(812, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1447 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6737 },
					},
					new List<Item>
					{
						new Item(436, 30, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(814, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1422 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6737 },
					},
					new List<Item>
					{
						new Item(436, 10, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(814, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1432 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6737 },
					},
					new List<Item>
					{
						new Item(436, 8, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(814, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1433 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6737 },
					},
					new List<Item>
					{
						new Item(436, 4, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(814, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1434 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6737 },
					},
					new List<Item>
					{
						new Item(436, 2, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(814, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1435 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6737 },
					},
					new List<Item>
					{
						new Item(436, 0, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(814, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1436 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6737 },
					},
					new List<Item>
					{
						new Item(436, 28, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(814, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1423 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6737 },
					},
					new List<Item>
					{
						new Item(436, 26, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(814, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1424 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6737 },
					},
					new List<Item>
					{
						new Item(436, 24, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(814, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1425 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6737 },
					},
					new List<Item>
					{
						new Item(436, 22, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(814, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1426 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6737 },
					},
					new List<Item>
					{
						new Item(436, 20, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(814, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1427 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6737 },
					},
					new List<Item>
					{
						new Item(436, 18, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(814, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1428 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6737 },
					},
					new List<Item>
					{
						new Item(436, 16, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(814, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1429 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6737 },
					},
					new List<Item>
					{
						new Item(436, 14, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(814, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1430 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6737 },
					},
					new List<Item>
					{
						new Item(436, 12, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(814, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1431 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6736 },
					},
					new List<Item>
					{
						new Item(436, 30, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(816, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1406 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6736 },
					},
					new List<Item>
					{
						new Item(436, 10, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(816, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1416 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6736 },
					},
					new List<Item>
					{
						new Item(436, 8, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(816, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1417 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6736 },
					},
					new List<Item>
					{
						new Item(436, 6, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(816, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1418 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6736 },
					},
					new List<Item>
					{
						new Item(436, 2, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(816, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1419 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6736 },
					},
					new List<Item>
					{
						new Item(436, 0, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(816, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1420 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6736 },
					},
					new List<Item>
					{
						new Item(436, 28, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(816, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1407 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6736 },
					},
					new List<Item>
					{
						new Item(436, 26, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(816, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1408 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6736 },
					},
					new List<Item>
					{
						new Item(436, 24, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(816, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1409 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6736 },
					},
					new List<Item>
					{
						new Item(436, 22, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(816, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1410 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6736 },
					},
					new List<Item>
					{
						new Item(436, 20, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(816, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1411 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6736 },
					},
					new List<Item>
					{
						new Item(436, 18, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(816, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1412 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6736 },
					},
					new List<Item>
					{
						new Item(436, 16, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(816, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1413 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6736 },
					},
					new List<Item>
					{
						new Item(436, 14, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(816, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1414 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6736 },
					},
					new List<Item>
					{
						new Item(436, 12, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(816, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1415 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6735 },
					},
					new List<Item>
					{
						new Item(436, 30, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(818, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1390 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6735 },
					},
					new List<Item>
					{
						new Item(436, 10, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(818, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1400 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6735 },
					},
					new List<Item>
					{
						new Item(436, 8, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(818, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1401 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6735 },
					},
					new List<Item>
					{
						new Item(436, 6, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(818, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1402 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6735 },
					},
					new List<Item>
					{
						new Item(436, 4, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(818, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1403 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6735 },
					},
					new List<Item>
					{
						new Item(436, 0, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(818, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1404 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6735 },
					},
					new List<Item>
					{
						new Item(436, 28, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(818, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1391 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6735 },
					},
					new List<Item>
					{
						new Item(436, 26, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(818, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1392 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6735 },
					},
					new List<Item>
					{
						new Item(436, 24, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(818, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1393 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6735 },
					},
					new List<Item>
					{
						new Item(436, 22, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(818, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1394 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6735 },
					},
					new List<Item>
					{
						new Item(436, 20, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(818, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1395 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6735 },
					},
					new List<Item>
					{
						new Item(436, 18, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(818, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1396 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6735 },
					},
					new List<Item>
					{
						new Item(436, 16, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(818, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1397 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6735 },
					},
					new List<Item>
					{
						new Item(436, 14, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(818, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1398 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6735 },
					},
					new List<Item>
					{
						new Item(436, 12, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(818, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1399 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6734 },
					},
					new List<Item>
					{
						new Item(436, 30, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(822, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1374 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6734 },
					},
					new List<Item>
					{
						new Item(436, 10, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(822, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1384 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6734 },
					},
					new List<Item>
					{
						new Item(436, 8, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(822, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1385 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6734 },
					},
					new List<Item>
					{
						new Item(436, 6, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(822, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1386 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6734 },
					},
					new List<Item>
					{
						new Item(436, 4, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(822, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1387 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6734 },
					},
					new List<Item>
					{
						new Item(436, 2, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(822, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1388 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6734 },
					},
					new List<Item>
					{
						new Item(436, 28, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(822, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1375 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6734 },
					},
					new List<Item>
					{
						new Item(436, 26, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(822, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1376 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6734 },
					},
					new List<Item>
					{
						new Item(436, 24, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(822, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1377 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6734 },
					},
					new List<Item>
					{
						new Item(436, 22, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(822, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1378 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6734 },
					},
					new List<Item>
					{
						new Item(436, 20, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(822, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1379 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6734 },
					},
					new List<Item>
					{
						new Item(436, 18, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(822, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1380 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6734 },
					},
					new List<Item>
					{
						new Item(436, 16, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(822, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1381 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293917, RuntimeId=6734 },
					},
					new List<Item>
					{
						new Item(436, 14, 2){ UniqueId = 41293917, RuntimeId=0 },
						new Item(822, -2, 2){ UniqueId = 41293917, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1382 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6734 },
					},
					new List<Item>
					{
						new Item(436, 12, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(822, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1383 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6749 },
					},
					new List<Item>
					{
						new Item(436, 10, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(790, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1367 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6749 },
					},
					new List<Item>
					{
						new Item(436, 8, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(790, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1368 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6749 },
					},
					new List<Item>
					{
						new Item(436, 6, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(790, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1369 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6749 },
					},
					new List<Item>
					{
						new Item(436, 4, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(790, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1370 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6749 },
					},
					new List<Item>
					{
						new Item(436, 2, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(790, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1371 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6749 },
					},
					new List<Item>
					{
						new Item(436, 0, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(790, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1372 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6749 },
					},
					new List<Item>
					{
						new Item(436, 28, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(790, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1358 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6749 },
					},
					new List<Item>
					{
						new Item(436, 26, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(790, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1359 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6749 },
					},
					new List<Item>
					{
						new Item(436, 24, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(790, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1360 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6749 },
					},
					new List<Item>
					{
						new Item(436, 22, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(790, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1361 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6749 },
					},
					new List<Item>
					{
						new Item(436, 20, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(790, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1362 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6749 },
					},
					new List<Item>
					{
						new Item(436, 18, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(790, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1363 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6749 },
					},
					new List<Item>
					{
						new Item(436, 16, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(790, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1364 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6749 },
					},
					new List<Item>
					{
						new Item(436, 14, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(790, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1365 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6749 },
					},
					new List<Item>
					{
						new Item(436, 12, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(790, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1366 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6746 },
					},
					new List<Item>
					{
						new Item(436, 30, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(796, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1342 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6746 },
					},
					new List<Item>
					{
						new Item(436, 10, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(796, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1351 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6746 },
					},
					new List<Item>
					{
						new Item(436, 8, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(796, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1352 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6746 },
					},
					new List<Item>
					{
						new Item(436, 6, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(796, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1353 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6746 },
					},
					new List<Item>
					{
						new Item(436, 4, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(796, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1354 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6746 },
					},
					new List<Item>
					{
						new Item(436, 2, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(796, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1355 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6746 },
					},
					new List<Item>
					{
						new Item(436, 0, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(796, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1356 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6746 },
					},
					new List<Item>
					{
						new Item(436, 28, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(796, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1343 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6746 },
					},
					new List<Item>
					{
						new Item(436, 26, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(796, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1344 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6746 },
					},
					new List<Item>
					{
						new Item(436, 22, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(796, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1345 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6746 },
					},
					new List<Item>
					{
						new Item(436, 20, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(796, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1346 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6746 },
					},
					new List<Item>
					{
						new Item(436, 18, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(796, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1347 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6746 },
					},
					new List<Item>
					{
						new Item(436, 16, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(796, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1348 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6746 },
					},
					new List<Item>
					{
						new Item(436, 14, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(796, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1349 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6746 },
					},
					new List<Item>
					{
						new Item(436, 12, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(796, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1350 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6745 },
					},
					new List<Item>
					{
						new Item(436, 30, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(798, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1326 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6745 },
					},
					new List<Item>
					{
						new Item(436, 10, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(798, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1335 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6745 },
					},
					new List<Item>
					{
						new Item(436, 8, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(798, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1336 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6745 },
					},
					new List<Item>
					{
						new Item(436, 6, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(798, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1337 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6745 },
					},
					new List<Item>
					{
						new Item(436, 4, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(798, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1338 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6745 },
					},
					new List<Item>
					{
						new Item(436, 2, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(798, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1339 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6745 },
					},
					new List<Item>
					{
						new Item(436, 0, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(798, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1340 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6745 },
					},
					new List<Item>
					{
						new Item(436, 28, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(798, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1327 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6745 },
					},
					new List<Item>
					{
						new Item(436, 26, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(798, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1328 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6745 },
					},
					new List<Item>
					{
						new Item(436, 24, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(798, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1329 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6745 },
					},
					new List<Item>
					{
						new Item(436, 20, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(798, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1330 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6745 },
					},
					new List<Item>
					{
						new Item(436, 18, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(798, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1331 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6745 },
					},
					new List<Item>
					{
						new Item(436, 16, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(798, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1332 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6745 },
					},
					new List<Item>
					{
						new Item(436, 14, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(798, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1333 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6745 },
					},
					new List<Item>
					{
						new Item(436, 12, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(798, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1334 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6734 },
					},
					new List<Item>
					{
						new Item(436, 30, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(820, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1310 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6734 },
					},
					new List<Item>
					{
						new Item(436, 10, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(820, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1320 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6734 },
					},
					new List<Item>
					{
						new Item(436, 8, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(820, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1321 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6734 },
					},
					new List<Item>
					{
						new Item(436, 6, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(820, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1322 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6734 },
					},
					new List<Item>
					{
						new Item(436, 4, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(820, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1323 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6734 },
					},
					new List<Item>
					{
						new Item(436, 2, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(820, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1324 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6734 },
					},
					new List<Item>
					{
						new Item(436, 28, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(820, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1311 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6734 },
					},
					new List<Item>
					{
						new Item(436, 26, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(820, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1312 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6734 },
					},
					new List<Item>
					{
						new Item(436, 24, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(820, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1313 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6734 },
					},
					new List<Item>
					{
						new Item(436, 22, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(820, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1314 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6734 },
					},
					new List<Item>
					{
						new Item(436, 20, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(820, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1315 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6734 },
					},
					new List<Item>
					{
						new Item(436, 18, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(820, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1316 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6734 },
					},
					new List<Item>
					{
						new Item(436, 16, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(820, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1317 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6734 },
					},
					new List<Item>
					{
						new Item(436, 14, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(820, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1318 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6734 },
					},
					new List<Item>
					{
						new Item(436, 12, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(820, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1319 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6748 },
					},
					new List<Item>
					{
						new Item(436, 30, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(792, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1598 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6748 },
					},
					new List<Item>
					{
						new Item(436, 10, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(792, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1607 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6748 },
					},
					new List<Item>
					{
						new Item(436, 8, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(792, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1608 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6748 },
					},
					new List<Item>
					{
						new Item(436, 6, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(792, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1609 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6748 },
					},
					new List<Item>
					{
						new Item(436, 4, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(792, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1610 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6748 },
					},
					new List<Item>
					{
						new Item(436, 2, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(792, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1611 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6748 },
					},
					new List<Item>
					{
						new Item(436, 0, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(792, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1612 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6748 },
					},
					new List<Item>
					{
						new Item(436, 26, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(792, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1599 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6748 },
					},
					new List<Item>
					{
						new Item(436, 24, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(792, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1600 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6748 },
					},
					new List<Item>
					{
						new Item(436, 22, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(792, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1601 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6748 },
					},
					new List<Item>
					{
						new Item(436, 20, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(792, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1602 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6748 },
					},
					new List<Item>
					{
						new Item(436, 18, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(792, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1603 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6748 },
					},
					new List<Item>
					{
						new Item(436, 16, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(792, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1604 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6748 },
					},
					new List<Item>
					{
						new Item(436, 14, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(792, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1605 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6748 },
					},
					new List<Item>
					{
						new Item(436, 12, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(792, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1606 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6747 },
					},
					new List<Item>
					{
						new Item(436, 30, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(794, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1582 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6747 },
					},
					new List<Item>
					{
						new Item(436, 10, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(794, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1591 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6747 },
					},
					new List<Item>
					{
						new Item(436, 8, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(794, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1592 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6747 },
					},
					new List<Item>
					{
						new Item(436, 6, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(794, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1593 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6747 },
					},
					new List<Item>
					{
						new Item(436, 4, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(794, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1594 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6747 },
					},
					new List<Item>
					{
						new Item(436, 2, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(794, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1595 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6747 },
					},
					new List<Item>
					{
						new Item(436, 0, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(794, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1596 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6747 },
					},
					new List<Item>
					{
						new Item(436, 28, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(794, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1583 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6747 },
					},
					new List<Item>
					{
						new Item(436, 24, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(794, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1584 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6747 },
					},
					new List<Item>
					{
						new Item(436, 22, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(794, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1585 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6747 },
					},
					new List<Item>
					{
						new Item(436, 20, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(794, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1586 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6747 },
					},
					new List<Item>
					{
						new Item(436, 18, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(794, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1587 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6747 },
					},
					new List<Item>
					{
						new Item(436, 16, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(794, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1588 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6747 },
					},
					new List<Item>
					{
						new Item(436, 14, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(794, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1589 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6747 },
					},
					new List<Item>
					{
						new Item(436, 12, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(794, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1590 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6746 },
					},
					new List<Item>
					{
						new Item(436, 30, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(824, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1566 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6746 },
					},
					new List<Item>
					{
						new Item(436, 10, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(824, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1575 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6746 },
					},
					new List<Item>
					{
						new Item(436, 8, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(824, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1576 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6746 },
					},
					new List<Item>
					{
						new Item(436, 6, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(824, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1577 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6746 },
					},
					new List<Item>
					{
						new Item(436, 4, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(824, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1578 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6746 },
					},
					new List<Item>
					{
						new Item(436, 2, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(824, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1579 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6746 },
					},
					new List<Item>
					{
						new Item(436, 0, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(824, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1580 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6746 },
					},
					new List<Item>
					{
						new Item(436, 28, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(824, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1567 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6746 },
					},
					new List<Item>
					{
						new Item(436, 26, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(824, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1568 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6746 },
					},
					new List<Item>
					{
						new Item(436, 22, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(824, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1569 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6746 },
					},
					new List<Item>
					{
						new Item(436, 20, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(824, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1570 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6746 },
					},
					new List<Item>
					{
						new Item(436, 18, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(824, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1571 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6746 },
					},
					new List<Item>
					{
						new Item(436, 16, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(824, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1572 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6746 },
					},
					new List<Item>
					{
						new Item(436, 14, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(824, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1573 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6746 },
					},
					new List<Item>
					{
						new Item(436, 12, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(824, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1574 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6745 },
					},
					new List<Item>
					{
						new Item(436, 30, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(828, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1550 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6745 },
					},
					new List<Item>
					{
						new Item(436, 10, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(828, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1559 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6745 },
					},
					new List<Item>
					{
						new Item(436, 8, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(828, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1560 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6745 },
					},
					new List<Item>
					{
						new Item(436, 6, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(828, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1561 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6745 },
					},
					new List<Item>
					{
						new Item(436, 4, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(828, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1562 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6745 },
					},
					new List<Item>
					{
						new Item(436, 2, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(828, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1563 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6745 },
					},
					new List<Item>
					{
						new Item(436, 0, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(828, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1564 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6745 },
					},
					new List<Item>
					{
						new Item(436, 28, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(828, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1551 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6745 },
					},
					new List<Item>
					{
						new Item(436, 26, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(828, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1552 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6745 },
					},
					new List<Item>
					{
						new Item(436, 24, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(828, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1553 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6745 },
					},
					new List<Item>
					{
						new Item(436, 20, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(828, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1554 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6745 },
					},
					new List<Item>
					{
						new Item(436, 18, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(828, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1555 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6745 },
					},
					new List<Item>
					{
						new Item(436, 16, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(828, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1556 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293921, RuntimeId=6745 },
					},
					new List<Item>
					{
						new Item(436, 14, 2){ UniqueId = 41293921, RuntimeId=0 },
						new Item(828, -2, 2){ UniqueId = 41293921, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1557 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6745 },
					},
					new List<Item>
					{
						new Item(436, 12, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(828, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1558 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6744 },
					},
					new List<Item>
					{
						new Item(436, 30, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(800, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1534 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6744 },
					},
					new List<Item>
					{
						new Item(436, 10, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(800, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1543 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6744 },
					},
					new List<Item>
					{
						new Item(436, 8, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(800, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1544 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6744 },
					},
					new List<Item>
					{
						new Item(436, 6, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(800, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1545 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6744 },
					},
					new List<Item>
					{
						new Item(436, 4, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(800, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1546 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6744 },
					},
					new List<Item>
					{
						new Item(436, 2, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(800, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1547 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6744 },
					},
					new List<Item>
					{
						new Item(436, 0, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(800, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1548 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6744 },
					},
					new List<Item>
					{
						new Item(436, 28, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(800, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1535 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6744 },
					},
					new List<Item>
					{
						new Item(436, 26, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(800, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1536 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6744 },
					},
					new List<Item>
					{
						new Item(436, 24, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(800, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1537 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6744 },
					},
					new List<Item>
					{
						new Item(436, 22, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(800, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1538 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6744 },
					},
					new List<Item>
					{
						new Item(436, 18, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(800, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1539 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6744 },
					},
					new List<Item>
					{
						new Item(436, 16, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(800, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1540 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6744 },
					},
					new List<Item>
					{
						new Item(436, 14, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(800, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1541 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6744 },
					},
					new List<Item>
					{
						new Item(436, 12, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(800, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1542 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6743 },
					},
					new List<Item>
					{
						new Item(436, 30, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(802, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1518 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6743 },
					},
					new List<Item>
					{
						new Item(436, 10, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(802, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1527 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6743 },
					},
					new List<Item>
					{
						new Item(436, 8, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(802, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1528 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6743 },
					},
					new List<Item>
					{
						new Item(436, 6, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(802, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1529 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6743 },
					},
					new List<Item>
					{
						new Item(436, 4, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(802, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1530 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6743 },
					},
					new List<Item>
					{
						new Item(436, 2, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(802, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1531 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6743 },
					},
					new List<Item>
					{
						new Item(436, 0, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(802, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1532 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6743 },
					},
					new List<Item>
					{
						new Item(436, 28, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(802, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1519 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6743 },
					},
					new List<Item>
					{
						new Item(436, 26, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(802, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1520 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6743 },
					},
					new List<Item>
					{
						new Item(436, 24, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(802, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1521 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6743 },
					},
					new List<Item>
					{
						new Item(436, 22, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(802, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1522 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6743 },
					},
					new List<Item>
					{
						new Item(436, 20, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(802, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1523 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6743 },
					},
					new List<Item>
					{
						new Item(436, 16, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(802, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1524 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6743 },
					},
					new List<Item>
					{
						new Item(436, 14, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(802, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1525 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6743 },
					},
					new List<Item>
					{
						new Item(436, 12, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(802, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1526 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6742 },
					},
					new List<Item>
					{
						new Item(436, 30, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(804, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1502 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6742 },
					},
					new List<Item>
					{
						new Item(436, 10, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(804, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1511 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6742 },
					},
					new List<Item>
					{
						new Item(436, 8, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(804, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1512 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6742 },
					},
					new List<Item>
					{
						new Item(436, 6, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(804, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1513 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6742 },
					},
					new List<Item>
					{
						new Item(436, 4, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(804, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1514 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6742 },
					},
					new List<Item>
					{
						new Item(436, 2, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(804, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1515 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6742 },
					},
					new List<Item>
					{
						new Item(436, 0, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(804, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1516 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6742 },
					},
					new List<Item>
					{
						new Item(436, 28, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(804, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1503 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6742 },
					},
					new List<Item>
					{
						new Item(436, 26, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(804, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1504 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6742 },
					},
					new List<Item>
					{
						new Item(436, 24, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(804, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1505 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6742 },
					},
					new List<Item>
					{
						new Item(436, 22, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(804, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1506 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6742 },
					},
					new List<Item>
					{
						new Item(436, 20, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(804, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1507 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6742 },
					},
					new List<Item>
					{
						new Item(436, 18, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(804, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1508 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6742 },
					},
					new List<Item>
					{
						new Item(436, 14, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(804, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1509 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6742 },
					},
					new List<Item>
					{
						new Item(436, 12, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(804, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1510 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6741 },
					},
					new List<Item>
					{
						new Item(436, 30, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(806, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1486 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6741 },
					},
					new List<Item>
					{
						new Item(436, 10, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(806, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1495 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6741 },
					},
					new List<Item>
					{
						new Item(436, 8, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(806, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1496 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6741 },
					},
					new List<Item>
					{
						new Item(436, 6, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(806, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1497 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6741 },
					},
					new List<Item>
					{
						new Item(436, 4, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(806, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1498 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6741 },
					},
					new List<Item>
					{
						new Item(436, 2, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(806, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1499 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6741 },
					},
					new List<Item>
					{
						new Item(436, 0, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(806, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1500 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6741 },
					},
					new List<Item>
					{
						new Item(436, 28, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(806, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1487 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6741 },
					},
					new List<Item>
					{
						new Item(436, 26, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(806, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1488 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6741 },
					},
					new List<Item>
					{
						new Item(436, 24, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(806, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1489 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6741 },
					},
					new List<Item>
					{
						new Item(436, 22, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(806, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1490 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6741 },
					},
					new List<Item>
					{
						new Item(436, 20, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(806, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1491 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6741 },
					},
					new List<Item>
					{
						new Item(436, 18, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(806, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1492 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6741 },
					},
					new List<Item>
					{
						new Item(436, 16, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(806, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1493 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6741 },
					},
					new List<Item>
					{
						new Item(436, 12, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(806, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1494 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6740 },
					},
					new List<Item>
					{
						new Item(436, 30, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(808, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1470 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6740 },
					},
					new List<Item>
					{
						new Item(436, 10, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(808, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1479 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6740 },
					},
					new List<Item>
					{
						new Item(436, 8, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(808, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1480 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6740 },
					},
					new List<Item>
					{
						new Item(436, 6, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(808, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1481 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6740 },
					},
					new List<Item>
					{
						new Item(436, 4, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(808, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1482 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6740 },
					},
					new List<Item>
					{
						new Item(436, 2, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(808, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1483 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6740 },
					},
					new List<Item>
					{
						new Item(436, 0, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(808, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1484 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6740 },
					},
					new List<Item>
					{
						new Item(436, 28, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(808, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1471 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6740 },
					},
					new List<Item>
					{
						new Item(436, 26, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(808, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1472 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6740 },
					},
					new List<Item>
					{
						new Item(436, 24, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(808, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1473 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6740 },
					},
					new List<Item>
					{
						new Item(436, 22, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(808, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1474 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6740 },
					},
					new List<Item>
					{
						new Item(436, 20, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(808, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1475 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6740 },
					},
					new List<Item>
					{
						new Item(436, 18, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(808, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1476 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6740 },
					},
					new List<Item>
					{
						new Item(436, 16, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(808, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1477 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6740 },
					},
					new List<Item>
					{
						new Item(436, 14, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(808, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1478 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6749 },
					},
					new List<Item>
					{
						new Item(410, 0, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(826, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1613 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6739 },
					},
					new List<Item>
					{
						new Item(410, 0, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(810, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1453 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6738 },
					},
					new List<Item>
					{
						new Item(410, 0, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(812, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1437 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6737 },
					},
					new List<Item>
					{
						new Item(410, 0, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(814, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1421 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6736 },
					},
					new List<Item>
					{
						new Item(410, 0, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(816, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1405 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6735 },
					},
					new List<Item>
					{
						new Item(410, 0, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(818, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1389 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6734 },
					},
					new List<Item>
					{
						new Item(410, 0, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(822, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1373 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6749 },
					},
					new List<Item>
					{
						new Item(410, 0, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(790, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1357 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6746 },
					},
					new List<Item>
					{
						new Item(410, 0, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(796, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1341 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6745 },
					},
					new List<Item>
					{
						new Item(410, 0, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(798, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1325 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6734 },
					},
					new List<Item>
					{
						new Item(410, 0, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(820, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1309 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6748 },
					},
					new List<Item>
					{
						new Item(410, 0, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(792, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1597 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6747 },
					},
					new List<Item>
					{
						new Item(410, 0, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(794, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1581 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6746 },
					},
					new List<Item>
					{
						new Item(410, 0, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(824, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1565 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6745 },
					},
					new List<Item>
					{
						new Item(410, 0, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(828, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1549 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6744 },
					},
					new List<Item>
					{
						new Item(410, 0, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(800, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1533 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6743 },
					},
					new List<Item>
					{
						new Item(410, 0, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(802, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1517 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6742 },
					},
					new List<Item>
					{
						new Item(410, 0, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(804, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1501 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6741 },
					},
					new List<Item>
					{
						new Item(410, 0, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(806, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1485 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1){ UniqueId = 41293925, RuntimeId=6740 },
					},
					new List<Item>
					{
						new Item(410, 0, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(808, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1469 },
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(-162, 0, 6){ UniqueId = 41293925, RuntimeId=7159 },
					},
					new Item[]
					{
						new Item(412, 0, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(412, 0, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(412, 0, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2031 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(134, 0, 4){ UniqueId = 41293925, RuntimeId=6946 },
					},
					new Item[]
					{
						new Item(10, 2, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(10, 2, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(10, 2, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(10, 2, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(10, 2, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(10, 2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2042 },
				new ShapedRecipe(1, 2,
					new List<Item>
					{
						new Item(320, 0, 4){ UniqueId = 41293925, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(10, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1986 },
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(182, 0, 6){ UniqueId = 41293925, RuntimeId=7143 },
					},
					new Item[]
					{
						new Item(358, 0, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(358, 0, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(358, 0, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2019 },
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(182, 0, 6){ UniqueId = 41293925, RuntimeId=7147 },
					},
					new Item[]
					{
						new Item(336, 4, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(336, 4, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(336, 4, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2027 },
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(182, 0, 6){ UniqueId = 41293925, RuntimeId=7146 },
					},
					new Item[]
					{
						new Item(336, 2, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(336, 2, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(336, 2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2026 },
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(182, 0, 6){ UniqueId = 41293925, RuntimeId=7144 },
					},
					new Item[]
					{
						new Item(402, 0, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(402, 0, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(402, 0, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2022 },
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(182, 0, 6){ UniqueId = 41293925, RuntimeId=7148 },
					},
					new Item[]
					{
						new Item(96, 0, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(96, 0, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(96, 0, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2028 },
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(182, 0, 6){ UniqueId = 41293925, RuntimeId=7150 },
					},
					new Item[]
					{
						new Item(430, 0, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(430, 0, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(430, 0, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2030 },
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(182, 0, 6){ UniqueId = 41293925, RuntimeId=7143 },
					},
					new Item[]
					{
						new Item(358, 2, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(358, 2, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(358, 2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2021 },
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(182, 0, 6){ UniqueId = 41293925, RuntimeId=7149 },
					},
					new Item[]
					{
						new Item(48, 6, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(48, 6, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(48, 6, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2029 },
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(-162, 0, 6){ UniqueId = 41293925, RuntimeId=7162 },
					},
					new Item[]
					{
						new Item(2, 10, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(2, 10, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(2, 10, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2034 },
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(-162, 0, 6){ UniqueId = 41293925, RuntimeId=7163 },
					},
					new Item[]
					{
						new Item(2, 6, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(2, 6, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(2, 6, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2035 },
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(-162, 0, 6){ UniqueId = 41293925, RuntimeId=7165 },
					},
					new Item[]
					{
						new Item(2, 2, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(2, 2, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(2, 2, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2037 },
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(-162, 0, 6){ UniqueId = 41293925, RuntimeId=7166 },
					},
					new Item[]
					{
						new Item(2, 4, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(2, 4, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(2, 4, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2038 },
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(-162, 0, 6){ UniqueId = 41293925, RuntimeId=7161 },
					},
					new Item[]
					{
						new Item(2, 12, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(2, 12, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(2, 12, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2033 },
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(-162, 0, 6){ UniqueId = 41293925, RuntimeId=7164 },
					},
					new Item[]
					{
						new Item(2, 8, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(2, 8, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(2, 8, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2036 },
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(-162, 0, 6){ UniqueId = 41293925, RuntimeId=7160 },
					},
					new Item[]
					{
						new Item(358, 6, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(358, 6, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(358, 6, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2032 },
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(-166, 0, 6){ UniqueId = 41293925, RuntimeId=7179 },
					},
					new Item[]
					{
						new Item(358, 4, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(358, 4, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(358, 4, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2040 },
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(-166, 0, 6){ UniqueId = 41293925, RuntimeId=7178 },
					},
					new Item[]
					{
						new Item(48, 4, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(48, 4, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(48, 4, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2039 },
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(-166, 0, 6){ UniqueId = 41293925, RuntimeId=7176 },
					},
					new Item[]
					{
						new Item(310, 6, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(310, 6, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(310, 6, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2023 },
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(44, 0, 6){ UniqueId = 41293925, RuntimeId=7133 },
					},
					new Item[]
					{
						new Item(310, 0, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(310, 0, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(310, 0, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2024 },
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(182, 0, 6){ UniqueId = 41293925, RuntimeId=7145 },
					},
					new Item[]
					{
						new Item(336, 0, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(336, 0, 2){ UniqueId = 41293925, RuntimeId=0 },
						new Item(336, 0, 2){ UniqueId = 41293925, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2025 },
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(44, 0, 6){ UniqueId = 41293929, RuntimeId=7128 },
					},
					new Item[]
					{
						new Item(48, 2, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(48, 2, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(48, 2, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2020 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(310, 0, 1){ UniqueId = 41293929, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(10, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2094 },
				new ShapedRecipe(1, 3,
					new List<Item>
					{
						new Item(309, 0, 1){ UniqueId = 41293929, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(10, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2095 },
				new ShapedRecipe(2, 3,
					new List<Item>
					{
						new Item(311, 0, 1){ UniqueId = 41293929, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(10, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2096 },
				new ShapedRecipe(2, 3,
					new List<Item>
					{
						new Item(329, 0, 1){ UniqueId = 41293929, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(10, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(0, 0, 0){ UniqueId = 0, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2097 },
				new MultiRecipe() { Id = new UUID("aecd2294-4b94-434b-8667-4499bb2c9327"), UniqueId = 2145 }, // aecd2294-4b94-434b-8667-4499bb2c9327
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(301, 11, 8){ UniqueId = 41293929, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(1124, 20, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2104 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(301, 12, 8){ UniqueId = 41293929, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(1124, 22, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2105 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(301, 13, 8){ UniqueId = 41293929, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(1124, 24, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2106 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(301, 14, 8){ UniqueId = 41293929, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(1124, 26, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2107 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(301, 15, 8){ UniqueId = 41293929, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(1124, 28, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2108 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(301, 16, 8){ UniqueId = 41293929, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(1124, 30, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2109 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(301, 17, 8){ UniqueId = 41293929, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(1124, 32, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2110 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(301, 18, 8){ UniqueId = 41293929, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(1124, 34, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2111 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(301, 19, 8){ UniqueId = 41293929, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(1124, 36, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2112 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(301, 20, 8){ UniqueId = 41293929, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(1124, 38, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2113 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(301, 21, 8){ UniqueId = 41293929, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(1124, 40, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2114 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(301, 22, 8){ UniqueId = 41293929, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(1124, 42, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2115 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(301, 23, 8){ UniqueId = 41293929, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(1124, 44, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2116 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(301, 24, 8){ UniqueId = 41293929, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(1124, 46, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2117 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(301, 25, 8){ UniqueId = 41293929, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(1124, 48, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2118 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(301, 26, 8){ UniqueId = 41293929, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(1124, 50, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2119 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(301, 27, 8){ UniqueId = 41293929, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(1124, 52, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2120 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(301, 28, 8){ UniqueId = 41293929, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(1124, 54, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2121 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(301, 29, 8){ UniqueId = 41293929, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(1124, 56, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2122 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(301, 30, 8){ UniqueId = 41293929, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(1124, 58, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2123 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(301, 31, 8){ UniqueId = 41293929, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(1124, 60, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2124 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(301, 32, 8){ UniqueId = 41293929, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(1124, 62, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2125 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(301, 33, 8){ UniqueId = 41293929, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(1124, 64, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2126 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(301, 34, 8){ UniqueId = 41293929, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(1124, 66, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2127 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(301, 35, 8){ UniqueId = 41293929, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(1124, 68, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2128 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(301, 36, 8){ UniqueId = 41293929, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(1124, 70, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2129 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(301, 37, 8){ UniqueId = 41293929, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(1124, 72, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2130 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(301, 38, 8){ UniqueId = 41293929, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(1124, 74, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2131 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(301, 39, 8){ UniqueId = 41293929, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(1124, 76, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2132 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(301, 40, 8){ UniqueId = 41293929, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(1124, 78, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2133 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(301, 41, 8){ UniqueId = 41293929, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(1124, 80, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2134 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(301, 42, 8){ UniqueId = 41293929, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(1124, 82, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2135 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(301, 43, 8){ UniqueId = 41293929, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(1124, 84, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2136 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(301, 6, 8){ UniqueId = 41293929, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(1124, 10, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2099 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(301, 7, 8){ UniqueId = 41293929, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(1124, 12, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2100 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(301, 8, 8){ UniqueId = 41293929, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(1124, 14, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2101 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(301, 9, 8){ UniqueId = 41293929, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(1124, 16, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2102 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(301, 10, 8){ UniqueId = 41293929, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(1124, 18, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(602, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2103 },
				new ShapedRecipe(1, 3,
					new List<Item>
					{
						new Item(308, 0, 1){ UniqueId = 41293929, RuntimeId=0 },
					},
					new Item[]
					{
						new Item(10, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(10, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(640, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 2098 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293929, RuntimeId=7834 },
					},
					new List<Item>
					{
						new Item(826, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(70, 28, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1308 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293929, RuntimeId=7834 },
					},
					new List<Item>
					{
						new Item(826, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(70, 10, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1299 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293929, RuntimeId=7834 },
					},
					new List<Item>
					{
						new Item(826, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(70, 8, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1298 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293929, RuntimeId=7834 },
					},
					new List<Item>
					{
						new Item(826, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(70, 6, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1297 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293929, RuntimeId=7834 },
					},
					new List<Item>
					{
						new Item(826, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(70, 4, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1296 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293929, RuntimeId=7834 },
					},
					new List<Item>
					{
						new Item(826, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(70, 2, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1295 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293929, RuntimeId=7834 },
					},
					new List<Item>
					{
						new Item(826, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(70, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1294 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293929, RuntimeId=7834 },
					},
					new List<Item>
					{
						new Item(826, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(70, 26, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1307 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293929, RuntimeId=7834 },
					},
					new List<Item>
					{
						new Item(826, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(70, 24, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1306 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293929, RuntimeId=7834 },
					},
					new List<Item>
					{
						new Item(826, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(70, 22, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1305 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293929, RuntimeId=7834 },
					},
					new List<Item>
					{
						new Item(826, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(70, 20, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1304 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293929, RuntimeId=7834 },
					},
					new List<Item>
					{
						new Item(826, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(70, 18, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1303 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293929, RuntimeId=7834 },
					},
					new List<Item>
					{
						new Item(826, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(70, 16, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1302 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293929, RuntimeId=7834 },
					},
					new List<Item>
					{
						new Item(826, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(70, 14, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1301 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293929, RuntimeId=7834 },
					},
					new List<Item>
					{
						new Item(826, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(70, 12, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1300 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293929, RuntimeId=7824 },
					},
					new List<Item>
					{
						new Item(810, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(70, 30, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1158 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293929, RuntimeId=7824 },
					},
					new List<Item>
					{
						new Item(810, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(70, 28, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1157 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293929, RuntimeId=7824 },
					},
					new List<Item>
					{
						new Item(810, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(70, 8, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1148 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293929, RuntimeId=7824 },
					},
					new List<Item>
					{
						new Item(810, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(70, 6, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1147 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293929, RuntimeId=7824 },
					},
					new List<Item>
					{
						new Item(810, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(70, 4, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1146 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293929, RuntimeId=7824 },
					},
					new List<Item>
					{
						new Item(810, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(70, 2, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1145 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293929, RuntimeId=7824 },
					},
					new List<Item>
					{
						new Item(810, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(70, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1144 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293929, RuntimeId=7824 },
					},
					new List<Item>
					{
						new Item(810, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(70, 26, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1156 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293929, RuntimeId=7824 },
					},
					new List<Item>
					{
						new Item(810, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(70, 24, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1155 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293929, RuntimeId=7824 },
					},
					new List<Item>
					{
						new Item(810, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(70, 22, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1154 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293929, RuntimeId=7824 },
					},
					new List<Item>
					{
						new Item(810, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(70, 20, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1153 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293929, RuntimeId=7824 },
					},
					new List<Item>
					{
						new Item(810, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(70, 18, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1152 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293929, RuntimeId=7824 },
					},
					new List<Item>
					{
						new Item(810, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(70, 16, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1151 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293929, RuntimeId=7824 },
					},
					new List<Item>
					{
						new Item(810, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(70, 14, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1150 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293929, RuntimeId=7824 },
					},
					new List<Item>
					{
						new Item(810, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(70, 12, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1149 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293929, RuntimeId=7823 },
					},
					new List<Item>
					{
						new Item(812, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(70, 30, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1143 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293929, RuntimeId=7823 },
					},
					new List<Item>
					{
						new Item(812, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(70, 28, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1142 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293929, RuntimeId=7823 },
					},
					new List<Item>
					{
						new Item(812, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(70, 10, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1133 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293929, RuntimeId=7823 },
					},
					new List<Item>
					{
						new Item(812, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(70, 6, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1132 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293929, RuntimeId=7823 },
					},
					new List<Item>
					{
						new Item(812, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(70, 4, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1131 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293929, RuntimeId=7823 },
					},
					new List<Item>
					{
						new Item(812, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(70, 2, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1130 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293929, RuntimeId=7823 },
					},
					new List<Item>
					{
						new Item(812, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(70, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1129 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293929, RuntimeId=7823 },
					},
					new List<Item>
					{
						new Item(812, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(70, 26, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1141 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293929, RuntimeId=7823 },
					},
					new List<Item>
					{
						new Item(812, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(70, 24, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1140 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293929, RuntimeId=7823 },
					},
					new List<Item>
					{
						new Item(812, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(70, 22, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1139 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293929, RuntimeId=7823 },
					},
					new List<Item>
					{
						new Item(812, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(70, 20, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1138 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293929, RuntimeId=7823 },
					},
					new List<Item>
					{
						new Item(812, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(70, 18, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1137 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293929, RuntimeId=7823 },
					},
					new List<Item>
					{
						new Item(812, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(70, 16, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1136 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293929, RuntimeId=7823 },
					},
					new List<Item>
					{
						new Item(812, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(70, 14, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1135 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293929, RuntimeId=7823 },
					},
					new List<Item>
					{
						new Item(812, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(70, 12, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1134 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293929, RuntimeId=7822 },
					},
					new List<Item>
					{
						new Item(814, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(70, 30, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1128 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293929, RuntimeId=7822 },
					},
					new List<Item>
					{
						new Item(814, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(70, 28, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1127 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293929, RuntimeId=7822 },
					},
					new List<Item>
					{
						new Item(814, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(70, 10, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1118 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293929, RuntimeId=7822 },
					},
					new List<Item>
					{
						new Item(814, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(70, 8, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1117 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293929, RuntimeId=7822 },
					},
					new List<Item>
					{
						new Item(814, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(70, 4, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1116 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293929, RuntimeId=7822 },
					},
					new List<Item>
					{
						new Item(814, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(70, 2, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1115 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293929, RuntimeId=7822 },
					},
					new List<Item>
					{
						new Item(814, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(70, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1114 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293929, RuntimeId=7822 },
					},
					new List<Item>
					{
						new Item(814, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(70, 26, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1126 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293929, RuntimeId=7822 },
					},
					new List<Item>
					{
						new Item(814, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(70, 24, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1125 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293929, RuntimeId=7822 },
					},
					new List<Item>
					{
						new Item(814, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(70, 22, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1124 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293929, RuntimeId=7822 },
					},
					new List<Item>
					{
						new Item(814, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(70, 20, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1123 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293929, RuntimeId=7822 },
					},
					new List<Item>
					{
						new Item(814, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(70, 18, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1122 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293929, RuntimeId=7822 },
					},
					new List<Item>
					{
						new Item(814, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(70, 16, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1121 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293929, RuntimeId=7822 },
					},
					new List<Item>
					{
						new Item(814, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(70, 14, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1120 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293929, RuntimeId=7822 },
					},
					new List<Item>
					{
						new Item(814, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(70, 12, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1119 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293929, RuntimeId=7821 },
					},
					new List<Item>
					{
						new Item(816, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(70, 30, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1113 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293929, RuntimeId=7821 },
					},
					new List<Item>
					{
						new Item(816, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(70, 28, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1112 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293929, RuntimeId=7821 },
					},
					new List<Item>
					{
						new Item(816, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(70, 10, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1103 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293929, RuntimeId=7821 },
					},
					new List<Item>
					{
						new Item(816, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(70, 8, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1102 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293929, RuntimeId=7821 },
					},
					new List<Item>
					{
						new Item(816, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(70, 6, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1101 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293929, RuntimeId=7821 },
					},
					new List<Item>
					{
						new Item(816, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(70, 2, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1100 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293929, RuntimeId=7821 },
					},
					new List<Item>
					{
						new Item(816, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(70, 0, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1099 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293929, RuntimeId=7821 },
					},
					new List<Item>
					{
						new Item(816, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(70, 26, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1111 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293929, RuntimeId=7821 },
					},
					new List<Item>
					{
						new Item(816, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(70, 24, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1110 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293929, RuntimeId=7821 },
					},
					new List<Item>
					{
						new Item(816, -2, 2){ UniqueId = 41293929, RuntimeId=0 },
						new Item(70, 22, 2){ UniqueId = 41293929, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1109 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7821 },
					},
					new List<Item>
					{
						new Item(816, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 20, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1108 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7821 },
					},
					new List<Item>
					{
						new Item(816, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 18, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1107 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7821 },
					},
					new List<Item>
					{
						new Item(816, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 16, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1106 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7821 },
					},
					new List<Item>
					{
						new Item(816, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 14, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1105 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7821 },
					},
					new List<Item>
					{
						new Item(816, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 12, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1104 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7820 },
					},
					new List<Item>
					{
						new Item(818, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 30, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1098 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7820 },
					},
					new List<Item>
					{
						new Item(818, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 28, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1097 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7820 },
					},
					new List<Item>
					{
						new Item(818, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 10, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1088 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7820 },
					},
					new List<Item>
					{
						new Item(818, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 8, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1087 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7820 },
					},
					new List<Item>
					{
						new Item(818, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 6, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1086 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7820 },
					},
					new List<Item>
					{
						new Item(818, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 4, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1085 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7820 },
					},
					new List<Item>
					{
						new Item(818, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 0, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1084 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7820 },
					},
					new List<Item>
					{
						new Item(818, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 26, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1096 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7820 },
					},
					new List<Item>
					{
						new Item(818, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 24, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1095 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7820 },
					},
					new List<Item>
					{
						new Item(818, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 22, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1094 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7820 },
					},
					new List<Item>
					{
						new Item(818, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 20, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1093 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7820 },
					},
					new List<Item>
					{
						new Item(818, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 18, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1092 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7820 },
					},
					new List<Item>
					{
						new Item(818, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 16, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1091 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7820 },
					},
					new List<Item>
					{
						new Item(818, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 14, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1090 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7820 },
					},
					new List<Item>
					{
						new Item(818, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 12, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1089 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7819 },
					},
					new List<Item>
					{
						new Item(822, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 30, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1083 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7819 },
					},
					new List<Item>
					{
						new Item(822, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 28, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1082 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7819 },
					},
					new List<Item>
					{
						new Item(822, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 10, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1073 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7819 },
					},
					new List<Item>
					{
						new Item(822, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 8, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1072 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7819 },
					},
					new List<Item>
					{
						new Item(822, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 6, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1071 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7819 },
					},
					new List<Item>
					{
						new Item(822, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 4, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1070 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7819 },
					},
					new List<Item>
					{
						new Item(822, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 2, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1069 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7819 },
					},
					new List<Item>
					{
						new Item(822, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 26, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1081 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7819 },
					},
					new List<Item>
					{
						new Item(822, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 24, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1080 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7819 },
					},
					new List<Item>
					{
						new Item(822, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 22, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1079 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7819 },
					},
					new List<Item>
					{
						new Item(822, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 20, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1078 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7819 },
					},
					new List<Item>
					{
						new Item(822, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 18, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1077 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7819 },
					},
					new List<Item>
					{
						new Item(822, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 16, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1076 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7819 },
					},
					new List<Item>
					{
						new Item(822, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 14, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1075 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7819 },
					},
					new List<Item>
					{
						new Item(822, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 12, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1074 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7834 },
					},
					new List<Item>
					{
						new Item(790, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 28, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1068 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7834 },
					},
					new List<Item>
					{
						new Item(790, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 10, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1059 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7834 },
					},
					new List<Item>
					{
						new Item(790, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 8, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1058 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7834 },
					},
					new List<Item>
					{
						new Item(790, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 6, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1057 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7834 },
					},
					new List<Item>
					{
						new Item(790, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 4, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1056 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7834 },
					},
					new List<Item>
					{
						new Item(790, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 2, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1055 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7834 },
					},
					new List<Item>
					{
						new Item(790, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 0, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1054 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7834 },
					},
					new List<Item>
					{
						new Item(790, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 26, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1067 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7834 },
					},
					new List<Item>
					{
						new Item(790, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 24, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1066 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7834 },
					},
					new List<Item>
					{
						new Item(790, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 22, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1065 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7834 },
					},
					new List<Item>
					{
						new Item(790, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 20, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1064 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7834 },
					},
					new List<Item>
					{
						new Item(790, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 18, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1063 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7834 },
					},
					new List<Item>
					{
						new Item(790, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 16, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1062 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7834 },
					},
					new List<Item>
					{
						new Item(790, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 14, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1061 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7834 },
					},
					new List<Item>
					{
						new Item(790, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 12, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1060 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7831 },
					},
					new List<Item>
					{
						new Item(796, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 30, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1053 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7831 },
					},
					new List<Item>
					{
						new Item(796, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 28, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1052 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7831 },
					},
					new List<Item>
					{
						new Item(796, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 10, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1044 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7831 },
					},
					new List<Item>
					{
						new Item(796, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 8, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1043 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7831 },
					},
					new List<Item>
					{
						new Item(796, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 6, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1042 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7831 },
					},
					new List<Item>
					{
						new Item(796, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 4, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1041 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7831 },
					},
					new List<Item>
					{
						new Item(796, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 2, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1040 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7831 },
					},
					new List<Item>
					{
						new Item(796, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 0, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1039 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7831 },
					},
					new List<Item>
					{
						new Item(796, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 26, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1051 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7831 },
					},
					new List<Item>
					{
						new Item(796, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 22, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1050 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7831 },
					},
					new List<Item>
					{
						new Item(796, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 20, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1049 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7831 },
					},
					new List<Item>
					{
						new Item(796, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 18, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1048 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7831 },
					},
					new List<Item>
					{
						new Item(796, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 16, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1047 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7831 },
					},
					new List<Item>
					{
						new Item(796, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 14, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1046 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7831 },
					},
					new List<Item>
					{
						new Item(796, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 12, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1045 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7830 },
					},
					new List<Item>
					{
						new Item(798, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 30, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1038 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7830 },
					},
					new List<Item>
					{
						new Item(798, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 28, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1037 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7830 },
					},
					new List<Item>
					{
						new Item(798, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 10, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1029 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7830 },
					},
					new List<Item>
					{
						new Item(798, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 8, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1028 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7830 },
					},
					new List<Item>
					{
						new Item(798, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 6, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1027 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7830 },
					},
					new List<Item>
					{
						new Item(798, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 4, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1026 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7830 },
					},
					new List<Item>
					{
						new Item(798, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 2, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1025 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7830 },
					},
					new List<Item>
					{
						new Item(798, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 0, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1024 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7830 },
					},
					new List<Item>
					{
						new Item(798, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 26, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1036 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7830 },
					},
					new List<Item>
					{
						new Item(798, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 24, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1035 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7830 },
					},
					new List<Item>
					{
						new Item(798, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 20, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1034 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7830 },
					},
					new List<Item>
					{
						new Item(798, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 18, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1033 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7830 },
					},
					new List<Item>
					{
						new Item(798, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 16, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1032 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7830 },
					},
					new List<Item>
					{
						new Item(798, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 14, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1031 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7830 },
					},
					new List<Item>
					{
						new Item(798, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 12, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1030 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7819 },
					},
					new List<Item>
					{
						new Item(820, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 30, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1023 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7819 },
					},
					new List<Item>
					{
						new Item(820, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 28, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1022 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7819 },
					},
					new List<Item>
					{
						new Item(820, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 10, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1013 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7819 },
					},
					new List<Item>
					{
						new Item(820, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 8, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1012 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7819 },
					},
					new List<Item>
					{
						new Item(820, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 6, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1011 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7819 },
					},
					new List<Item>
					{
						new Item(820, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 4, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1010 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7819 },
					},
					new List<Item>
					{
						new Item(820, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 2, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1009 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7819 },
					},
					new List<Item>
					{
						new Item(820, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 26, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1021 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7819 },
					},
					new List<Item>
					{
						new Item(820, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 24, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1020 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7819 },
					},
					new List<Item>
					{
						new Item(820, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 22, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1019 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7819 },
					},
					new List<Item>
					{
						new Item(820, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 20, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1018 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7819 },
					},
					new List<Item>
					{
						new Item(820, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 18, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1017 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7819 },
					},
					new List<Item>
					{
						new Item(820, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 16, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1016 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7819 },
					},
					new List<Item>
					{
						new Item(820, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 14, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1015 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7819 },
					},
					new List<Item>
					{
						new Item(820, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 12, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1014 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7833 },
					},
					new List<Item>
					{
						new Item(792, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 30, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1293 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7833 },
					},
					new List<Item>
					{
						new Item(792, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 10, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1284 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7833 },
					},
					new List<Item>
					{
						new Item(792, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 8, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1283 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7833 },
					},
					new List<Item>
					{
						new Item(792, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 6, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1282 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7833 },
					},
					new List<Item>
					{
						new Item(792, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 4, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1281 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293933, RuntimeId=7833 },
					},
					new List<Item>
					{
						new Item(792, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 2, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1280 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7833 },
					},
					new List<Item>
					{
						new Item(792, -2, 2){ UniqueId = 41293933, RuntimeId=0 },
						new Item(70, 0, 2){ UniqueId = 41293933, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1279 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7833 },
					},
					new List<Item>
					{
						new Item(792, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 26, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1292 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7833 },
					},
					new List<Item>
					{
						new Item(792, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 24, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1291 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7833 },
					},
					new List<Item>
					{
						new Item(792, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 22, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1290 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7833 },
					},
					new List<Item>
					{
						new Item(792, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 20, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1289 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7833 },
					},
					new List<Item>
					{
						new Item(792, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 18, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1288 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7833 },
					},
					new List<Item>
					{
						new Item(792, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 16, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1287 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7833 },
					},
					new List<Item>
					{
						new Item(792, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 14, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1286 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7833 },
					},
					new List<Item>
					{
						new Item(792, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 12, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1285 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7832 },
					},
					new List<Item>
					{
						new Item(794, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 30, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1278 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7832 },
					},
					new List<Item>
					{
						new Item(794, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 28, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1277 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7832 },
					},
					new List<Item>
					{
						new Item(794, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 10, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1269 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7832 },
					},
					new List<Item>
					{
						new Item(794, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 8, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1268 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7832 },
					},
					new List<Item>
					{
						new Item(794, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 6, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1267 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7832 },
					},
					new List<Item>
					{
						new Item(794, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 4, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1266 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7832 },
					},
					new List<Item>
					{
						new Item(794, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 2, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1265 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7832 },
					},
					new List<Item>
					{
						new Item(794, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 0, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1264 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7832 },
					},
					new List<Item>
					{
						new Item(794, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 24, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1276 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7832 },
					},
					new List<Item>
					{
						new Item(794, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 22, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1275 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7832 },
					},
					new List<Item>
					{
						new Item(794, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 20, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1274 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7832 },
					},
					new List<Item>
					{
						new Item(794, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 18, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1273 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7832 },
					},
					new List<Item>
					{
						new Item(794, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 16, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1272 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7832 },
					},
					new List<Item>
					{
						new Item(794, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 14, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1271 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7832 },
					},
					new List<Item>
					{
						new Item(794, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 12, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1270 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7831 },
					},
					new List<Item>
					{
						new Item(824, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 30, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1263 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7831 },
					},
					new List<Item>
					{
						new Item(824, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 28, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1262 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7831 },
					},
					new List<Item>
					{
						new Item(824, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 10, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1254 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7831 },
					},
					new List<Item>
					{
						new Item(824, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 8, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1253 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7831 },
					},
					new List<Item>
					{
						new Item(824, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 6, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1252 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7831 },
					},
					new List<Item>
					{
						new Item(824, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 4, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1251 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7831 },
					},
					new List<Item>
					{
						new Item(824, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 2, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1250 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7831 },
					},
					new List<Item>
					{
						new Item(824, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 0, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1249 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7831 },
					},
					new List<Item>
					{
						new Item(824, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 26, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1261 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7831 },
					},
					new List<Item>
					{
						new Item(824, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 22, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1260 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7831 },
					},
					new List<Item>
					{
						new Item(824, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 20, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1259 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7831 },
					},
					new List<Item>
					{
						new Item(824, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 18, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1258 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7831 },
					},
					new List<Item>
					{
						new Item(824, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 16, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1257 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7831 },
					},
					new List<Item>
					{
						new Item(824, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 14, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1256 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7831 },
					},
					new List<Item>
					{
						new Item(824, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 12, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1255 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7830 },
					},
					new List<Item>
					{
						new Item(828, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 30, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1248 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7830 },
					},
					new List<Item>
					{
						new Item(828, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 28, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1247 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7830 },
					},
					new List<Item>
					{
						new Item(828, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 10, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1239 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7830 },
					},
					new List<Item>
					{
						new Item(828, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 8, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1238 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7830 },
					},
					new List<Item>
					{
						new Item(828, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 6, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1237 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7830 },
					},
					new List<Item>
					{
						new Item(828, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 4, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1236 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7830 },
					},
					new List<Item>
					{
						new Item(828, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 2, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1235 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7830 },
					},
					new List<Item>
					{
						new Item(828, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 0, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1234 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7830 },
					},
					new List<Item>
					{
						new Item(828, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 26, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1246 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7830 },
					},
					new List<Item>
					{
						new Item(828, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 24, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1245 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7830 },
					},
					new List<Item>
					{
						new Item(828, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 20, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1244 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7830 },
					},
					new List<Item>
					{
						new Item(828, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 18, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1243 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7830 },
					},
					new List<Item>
					{
						new Item(828, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 16, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1242 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7830 },
					},
					new List<Item>
					{
						new Item(828, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 14, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1241 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7830 },
					},
					new List<Item>
					{
						new Item(828, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 12, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1240 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7829 },
					},
					new List<Item>
					{
						new Item(800, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 30, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1233 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7829 },
					},
					new List<Item>
					{
						new Item(800, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 28, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1232 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7829 },
					},
					new List<Item>
					{
						new Item(800, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 10, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1224 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7829 },
					},
					new List<Item>
					{
						new Item(800, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 8, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1223 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7829 },
					},
					new List<Item>
					{
						new Item(800, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 6, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1222 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7829 },
					},
					new List<Item>
					{
						new Item(800, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 4, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1221 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7829 },
					},
					new List<Item>
					{
						new Item(800, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 2, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1220 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7829 },
					},
					new List<Item>
					{
						new Item(800, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 0, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1219 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7829 },
					},
					new List<Item>
					{
						new Item(800, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 26, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1231 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7829 },
					},
					new List<Item>
					{
						new Item(800, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 24, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1230 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7829 },
					},
					new List<Item>
					{
						new Item(800, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 22, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1229 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7829 },
					},
					new List<Item>
					{
						new Item(800, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 18, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1228 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7829 },
					},
					new List<Item>
					{
						new Item(800, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 16, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1227 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7829 },
					},
					new List<Item>
					{
						new Item(800, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 14, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1226 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7829 },
					},
					new List<Item>
					{
						new Item(800, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 12, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1225 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7828 },
					},
					new List<Item>
					{
						new Item(802, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 30, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1218 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7828 },
					},
					new List<Item>
					{
						new Item(802, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 28, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1217 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7828 },
					},
					new List<Item>
					{
						new Item(802, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 10, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1209 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7828 },
					},
					new List<Item>
					{
						new Item(802, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 8, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1208 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7828 },
					},
					new List<Item>
					{
						new Item(802, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 6, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1207 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7828 },
					},
					new List<Item>
					{
						new Item(802, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 4, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1206 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7828 },
					},
					new List<Item>
					{
						new Item(802, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 2, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1205 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7828 },
					},
					new List<Item>
					{
						new Item(802, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 0, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1204 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7828 },
					},
					new List<Item>
					{
						new Item(802, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 26, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1216 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7828 },
					},
					new List<Item>
					{
						new Item(802, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 24, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1215 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7828 },
					},
					new List<Item>
					{
						new Item(802, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 22, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1214 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7828 },
					},
					new List<Item>
					{
						new Item(802, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 20, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1213 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7828 },
					},
					new List<Item>
					{
						new Item(802, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 16, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1212 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7828 },
					},
					new List<Item>
					{
						new Item(802, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 14, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1211 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7828 },
					},
					new List<Item>
					{
						new Item(802, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 12, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1210 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7827 },
					},
					new List<Item>
					{
						new Item(804, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 30, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1203 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7827 },
					},
					new List<Item>
					{
						new Item(804, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 28, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1202 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7827 },
					},
					new List<Item>
					{
						new Item(804, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 10, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1194 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7827 },
					},
					new List<Item>
					{
						new Item(804, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 8, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1193 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7827 },
					},
					new List<Item>
					{
						new Item(804, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 6, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1192 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7827 },
					},
					new List<Item>
					{
						new Item(804, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 4, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1191 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7827 },
					},
					new List<Item>
					{
						new Item(804, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 2, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1190 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7827 },
					},
					new List<Item>
					{
						new Item(804, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 0, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1189 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7827 },
					},
					new List<Item>
					{
						new Item(804, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 26, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1201 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7827 },
					},
					new List<Item>
					{
						new Item(804, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 24, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1200 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7827 },
					},
					new List<Item>
					{
						new Item(804, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 22, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1199 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7827 },
					},
					new List<Item>
					{
						new Item(804, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 20, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1198 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7827 },
					},
					new List<Item>
					{
						new Item(804, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 18, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1197 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7827 },
					},
					new List<Item>
					{
						new Item(804, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 14, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1196 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7827 },
					},
					new List<Item>
					{
						new Item(804, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 12, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1195 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293937, RuntimeId=7826 },
					},
					new List<Item>
					{
						new Item(806, -2, 2){ UniqueId = 41293937, RuntimeId=0 },
						new Item(70, 30, 2){ UniqueId = 41293937, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1188 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293941, RuntimeId=7826 },
					},
					new List<Item>
					{
						new Item(806, -2, 2){ UniqueId = 41293941, RuntimeId=0 },
						new Item(70, 28, 2){ UniqueId = 41293941, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1187 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293941, RuntimeId=7826 },
					},
					new List<Item>
					{
						new Item(806, -2, 2){ UniqueId = 41293941, RuntimeId=0 },
						new Item(70, 10, 2){ UniqueId = 41293941, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1179 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293941, RuntimeId=7826 },
					},
					new List<Item>
					{
						new Item(806, -2, 2){ UniqueId = 41293941, RuntimeId=0 },
						new Item(70, 8, 2){ UniqueId = 41293941, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1178 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293941, RuntimeId=7826 },
					},
					new List<Item>
					{
						new Item(806, -2, 2){ UniqueId = 41293941, RuntimeId=0 },
						new Item(70, 6, 2){ UniqueId = 41293941, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1177 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293941, RuntimeId=7826 },
					},
					new List<Item>
					{
						new Item(806, -2, 2){ UniqueId = 41293941, RuntimeId=0 },
						new Item(70, 4, 2){ UniqueId = 41293941, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1176 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293941, RuntimeId=7826 },
					},
					new List<Item>
					{
						new Item(806, -2, 2){ UniqueId = 41293941, RuntimeId=0 },
						new Item(70, 2, 2){ UniqueId = 41293941, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1175 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293941, RuntimeId=7826 },
					},
					new List<Item>
					{
						new Item(806, -2, 2){ UniqueId = 41293941, RuntimeId=0 },
						new Item(70, 0, 2){ UniqueId = 41293941, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1174 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293941, RuntimeId=7826 },
					},
					new List<Item>
					{
						new Item(806, -2, 2){ UniqueId = 41293941, RuntimeId=0 },
						new Item(70, 26, 2){ UniqueId = 41293941, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1186 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293941, RuntimeId=7826 },
					},
					new List<Item>
					{
						new Item(806, -2, 2){ UniqueId = 41293941, RuntimeId=0 },
						new Item(70, 24, 2){ UniqueId = 41293941, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1185 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293941, RuntimeId=7826 },
					},
					new List<Item>
					{
						new Item(806, -2, 2){ UniqueId = 41293941, RuntimeId=0 },
						new Item(70, 22, 2){ UniqueId = 41293941, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1184 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293941, RuntimeId=7826 },
					},
					new List<Item>
					{
						new Item(806, -2, 2){ UniqueId = 41293941, RuntimeId=0 },
						new Item(70, 20, 2){ UniqueId = 41293941, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1183 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293941, RuntimeId=7826 },
					},
					new List<Item>
					{
						new Item(806, -2, 2){ UniqueId = 41293941, RuntimeId=0 },
						new Item(70, 18, 2){ UniqueId = 41293941, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1182 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293941, RuntimeId=7826 },
					},
					new List<Item>
					{
						new Item(806, -2, 2){ UniqueId = 41293941, RuntimeId=0 },
						new Item(70, 16, 2){ UniqueId = 41293941, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1181 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293941, RuntimeId=7826 },
					},
					new List<Item>
					{
						new Item(806, -2, 2){ UniqueId = 41293941, RuntimeId=0 },
						new Item(70, 12, 2){ UniqueId = 41293941, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1180 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293941, RuntimeId=7825 },
					},
					new List<Item>
					{
						new Item(808, -2, 2){ UniqueId = 41293941, RuntimeId=0 },
						new Item(70, 30, 2){ UniqueId = 41293941, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1173 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293941, RuntimeId=7825 },
					},
					new List<Item>
					{
						new Item(808, -2, 2){ UniqueId = 41293941, RuntimeId=0 },
						new Item(70, 28, 2){ UniqueId = 41293941, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1172 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293941, RuntimeId=7825 },
					},
					new List<Item>
					{
						new Item(808, -2, 2){ UniqueId = 41293941, RuntimeId=0 },
						new Item(70, 10, 2){ UniqueId = 41293941, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1164 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293941, RuntimeId=7825 },
					},
					new List<Item>
					{
						new Item(808, -2, 2){ UniqueId = 41293941, RuntimeId=0 },
						new Item(70, 8, 2){ UniqueId = 41293941, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1163 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293941, RuntimeId=7825 },
					},
					new List<Item>
					{
						new Item(808, -2, 2){ UniqueId = 41293941, RuntimeId=0 },
						new Item(70, 6, 2){ UniqueId = 41293941, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1162 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293941, RuntimeId=7825 },
					},
					new List<Item>
					{
						new Item(808, -2, 2){ UniqueId = 41293941, RuntimeId=0 },
						new Item(70, 4, 2){ UniqueId = 41293941, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1161 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293941, RuntimeId=7825 },
					},
					new List<Item>
					{
						new Item(808, -2, 2){ UniqueId = 41293941, RuntimeId=0 },
						new Item(70, 2, 2){ UniqueId = 41293941, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1160 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293941, RuntimeId=7825 },
					},
					new List<Item>
					{
						new Item(808, -2, 2){ UniqueId = 41293941, RuntimeId=0 },
						new Item(70, 0, 2){ UniqueId = 41293941, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1159 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293941, RuntimeId=7825 },
					},
					new List<Item>
					{
						new Item(808, -2, 2){ UniqueId = 41293941, RuntimeId=0 },
						new Item(70, 26, 2){ UniqueId = 41293941, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1171 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293941, RuntimeId=7825 },
					},
					new List<Item>
					{
						new Item(808, -2, 2){ UniqueId = 41293941, RuntimeId=0 },
						new Item(70, 24, 2){ UniqueId = 41293941, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1170 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293941, RuntimeId=7825 },
					},
					new List<Item>
					{
						new Item(808, -2, 2){ UniqueId = 41293941, RuntimeId=0 },
						new Item(70, 22, 2){ UniqueId = 41293941, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1169 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293941, RuntimeId=7825 },
					},
					new List<Item>
					{
						new Item(808, -2, 2){ UniqueId = 41293941, RuntimeId=0 },
						new Item(70, 20, 2){ UniqueId = 41293941, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1168 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293941, RuntimeId=7825 },
					},
					new List<Item>
					{
						new Item(808, -2, 2){ UniqueId = 41293941, RuntimeId=0 },
						new Item(70, 18, 2){ UniqueId = 41293941, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1167 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293941, RuntimeId=7825 },
					},
					new List<Item>
					{
						new Item(808, -2, 2){ UniqueId = 41293941, RuntimeId=0 },
						new Item(70, 16, 2){ UniqueId = 41293941, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1166 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1){ UniqueId = 41293941, RuntimeId=7825 },
					},
					new List<Item>
					{
						new Item(808, -2, 2){ UniqueId = 41293941, RuntimeId=0 },
						new Item(70, 14, 2){ UniqueId = 41293941, RuntimeId=0 },
					}, "crafting_table"){ UniqueId = 1165 },
				new SmeltingRecipe(new Item(504, 32767, 1){ UniqueId = 41293941, RuntimeId=0 }, new Item(815, -2){ UniqueId = 41293941, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(504, 32767, 1){ UniqueId = 41293941, RuntimeId=0 }, new Item(815, -2){ UniqueId = 41293941, RuntimeId=0 }, "blast_furnace"),
				new SmeltingRecipe(new Item(512, 32767, 1){ UniqueId = 41293941, RuntimeId=0 }, new Item(813, -2){ UniqueId = 41293941, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(512, 32767, 1){ UniqueId = 41293941, RuntimeId=0 }, new Item(813, -2){ UniqueId = 41293941, RuntimeId=0 }, "blast_furnace"),
				new SmeltingRecipe(new Item(302, 0, 1){ UniqueId = 41293941, RuntimeId=0 }, new Item(811, -2){ UniqueId = 41293941, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(302, 0, 1){ UniqueId = 41293941, RuntimeId=0 }, new Item(811, -2){ UniqueId = 41293941, RuntimeId=0 }, "blast_furnace"),
				new SmeltingRecipe(new Item(304, 32767, 1){ UniqueId = 41293941, RuntimeId=0 }, new Item(809, -2){ UniqueId = 41293941, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(304, 32767, 1){ UniqueId = 41293941, RuntimeId=0 }, new Item(809, -2){ UniqueId = 41293941, RuntimeId=0 }, "blast_furnace"),
				new SmeltingRecipe(new Item(373, 32767, 1){ UniqueId = 41293941, RuntimeId=0 }, new Item(805, -2){ UniqueId = 41293941, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(373, 32767, 1){ UniqueId = 41293941, RuntimeId=0 }, new Item(805, -2){ UniqueId = 41293941, RuntimeId=0 }, "blast_furnace"),
				new SmeltingRecipe(new Item(306, 32767, 1){ UniqueId = 41293941, RuntimeId=0 }, new Item(803, -2){ UniqueId = 41293941, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(306, 32767, 1){ UniqueId = 41293941, RuntimeId=0 }, new Item(803, -2){ UniqueId = 41293941, RuntimeId=0 }, "blast_furnace"),
				new SmeltingRecipe(new Item(305, 32767, 1){ UniqueId = 41293941, RuntimeId=0 }, new Item(801, -2){ UniqueId = 41293941, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(305, 32767, 1){ UniqueId = 41293941, RuntimeId=0 }, new Item(801, -2){ UniqueId = 41293941, RuntimeId=0 }, "blast_furnace"),
				new SmeltingRecipe(new Item(414, 0, 1){ UniqueId = 41293941, RuntimeId=0 }, new Item(799, -2){ UniqueId = 41293941, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(414, 0, 1){ UniqueId = 41293941, RuntimeId=0 }, new Item(799, -2){ UniqueId = 41293941, RuntimeId=0 }, "blast_furnace"),
				new SmeltingRecipe(new Item(-410, 0, 1){ UniqueId = 41293941, RuntimeId=3766 }, new Item(781, -2){ UniqueId = 41293941, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(-409, 0, 1){ UniqueId = 41293941, RuntimeId=3767 }, new Item(773, -2){ UniqueId = 41293941, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(-378, 0, 1){ UniqueId = 41293941, RuntimeId=4099 }, new Item(757, -2){ UniqueId = 41293941, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(504, 32767, 1){ UniqueId = 41293941, RuntimeId=0 }, new Item(621, -2){ UniqueId = 41293941, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(504, 32767, 1){ UniqueId = 41293941, RuntimeId=0 }, new Item(621, -2){ UniqueId = 41293941, RuntimeId=0 }, "blast_furnace"),
				new SmeltingRecipe(new Item(306, 32767, 1){ UniqueId = 41293941, RuntimeId=0 }, new Item(575, -2){ UniqueId = 41293941, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(306, 32767, 1){ UniqueId = 41293941, RuntimeId=0 }, new Item(575, -2){ UniqueId = 41293941, RuntimeId=0 }, "blast_furnace"),
				new SmeltingRecipe(new Item(-280, 0, 1){ UniqueId = 41293941, RuntimeId=3769 }, new Item(547, -2){ UniqueId = 41293941, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(611, 32767, 1){ UniqueId = 41293941, RuntimeId=0 }, new Item(541, -2){ UniqueId = 41293941, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(611, 32767, 1){ UniqueId = 41293941, RuntimeId=0 }, new Item(541, -2){ UniqueId = 41293941, RuntimeId=0 }, "blast_furnace"),
				new SmeltingRecipe(new Item(-377, 0, 1){ UniqueId = 41293941, RuntimeId=6790 }, new Item(467, -2){ UniqueId = 41293941, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(303, 0, 1){ UniqueId = 41293941, RuntimeId=0 }, new Item(423, 0){ UniqueId = 41293941, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(303, 0, 1){ UniqueId = 41293941, RuntimeId=0 }, new Item(423, 2){ UniqueId = 41293941, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(303, 0, 1){ UniqueId = 41293941, RuntimeId=0 }, new Item(423, 4){ UniqueId = 41293941, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(303, 0, 1){ UniqueId = 41293941, RuntimeId=0 }, new Item(423, 6){ UniqueId = 41293941, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(303, 0, 1){ UniqueId = 41293941, RuntimeId=0 }, new Item(423, 8){ UniqueId = 41293941, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(303, 0, 1){ UniqueId = 41293941, RuntimeId=0 }, new Item(423, 10){ UniqueId = 41293941, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(303, 0, 1){ UniqueId = 41293941, RuntimeId=0 }, new Item(423, 16){ UniqueId = 41293941, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(303, 0, 1){ UniqueId = 41293941, RuntimeId=0 }, new Item(423, 18){ UniqueId = 41293941, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(303, 0, 1){ UniqueId = 41293941, RuntimeId=0 }, new Item(423, 20){ UniqueId = 41293941, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(303, 0, 1){ UniqueId = 41293941, RuntimeId=0 }, new Item(423, 22){ UniqueId = 41293941, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(303, 0, 1){ UniqueId = 41293941, RuntimeId=0 }, new Item(423, 24){ UniqueId = 41293941, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(303, 0, 1){ UniqueId = 41293941, RuntimeId=0 }, new Item(423, 26){ UniqueId = 41293941, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(405, 0, 1){ UniqueId = 41293941, RuntimeId=0 }, new Item(311, -2){ UniqueId = 41293941, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(303, 0, 1){ UniqueId = 41293941, RuntimeId=0 }, new Item(19, -2){ UniqueId = 41293941, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(303, 0, 1){ UniqueId = 41293941, RuntimeId=0 }, new Item(17, -2){ UniqueId = 41293941, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(303, 0, 1){ UniqueId = 41293941, RuntimeId=0 }, new Item(15, -2){ UniqueId = 41293941, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(303, 0, 1){ UniqueId = 41293941, RuntimeId=0 }, new Item(13, -2){ UniqueId = 41293941, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(303, 0, 1){ UniqueId = 41293941, RuntimeId=0 }, new Item(11, -2){ UniqueId = 41293941, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(303, 0, 1){ UniqueId = 41293941, RuntimeId=0 }, new Item(9, -2){ UniqueId = 41293941, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(-183, 0, 1){ UniqueId = 41293941, RuntimeId=6815 }, new Item(2, 0){ UniqueId = 41293941, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(1, 0, 1){ UniqueId = 41293941, RuntimeId=7084 }, new Item(8, -2){ UniqueId = 41293941, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(20, 0, 1){ UniqueId = 41293941, RuntimeId=4870 }, new Item(24, -2){ UniqueId = 41293941, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(306, 32767, 1){ UniqueId = 41293941, RuntimeId=0 }, new Item(28, -2){ UniqueId = 41293941, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(306, 32767, 1){ UniqueId = 41293941, RuntimeId=0 }, new Item(28, -2){ UniqueId = 41293941, RuntimeId=0 }, "blast_furnace"),
				new SmeltingRecipe(new Item(305, 32767, 1){ UniqueId = 41293941, RuntimeId=0 }, new Item(30, -2){ UniqueId = 41293941, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(305, 32767, 1){ UniqueId = 41293941, RuntimeId=0 }, new Item(30, -2){ UniqueId = 41293941, RuntimeId=0 }, "blast_furnace"),
				new SmeltingRecipe(new Item(302, 0, 1){ UniqueId = 41293941, RuntimeId=0 }, new Item(32, -2){ UniqueId = 41293941, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(302, 0, 1){ UniqueId = 41293941, RuntimeId=0 }, new Item(32, -2){ UniqueId = 41293941, RuntimeId=0 }, "blast_furnace"),
				new SmeltingRecipe(new Item(303, 0, 1){ UniqueId = 41293941, RuntimeId=0 }, new Item(34, 0){ UniqueId = 41293941, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(303, 0, 1){ UniqueId = 41293941, RuntimeId=0 }, new Item(34, 2){ UniqueId = 41293941, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(303, 0, 1){ UniqueId = 41293941, RuntimeId=0 }, new Item(34, 4){ UniqueId = 41293941, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(303, 0, 1){ UniqueId = 41293941, RuntimeId=0 }, new Item(34, 6){ UniqueId = 41293941, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(19, 0, 1){ UniqueId = 41293941, RuntimeId=6867 }, new Item(38, 2){ UniqueId = 41293941, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(414, 0, 1){ UniqueId = 41293941, RuntimeId=0 }, new Item(42, -2){ UniqueId = 41293941, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(414, 0, 1){ UniqueId = 41293941, RuntimeId=0 }, new Item(42, -2){ UniqueId = 41293941, RuntimeId=0 }, "blast_furnace"),
				new SmeltingRecipe(new Item(24, 0, 1){ UniqueId = 41293941, RuntimeId=6682 }, new Item(48, -2){ UniqueId = 41293941, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(304, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(112, -2){ UniqueId = 41293945, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(304, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(112, -2){ UniqueId = 41293945, RuntimeId=0 }, "blast_furnace"),
				new SmeltingRecipe(new Item(373, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(146, -2){ UniqueId = 41293945, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(373, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(146, -2){ UniqueId = 41293945, RuntimeId=0 }, "blast_furnace"),
				new SmeltingRecipe(new Item(397, 0, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(162, -2){ UniqueId = 41293945, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(172, 0, 1){ UniqueId = 41293945, RuntimeId=5058 }, new Item(164, -2){ UniqueId = 41293945, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(523, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(174, -2){ UniqueId = 41293945, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(98, 0, 1){ UniqueId = 41293945, RuntimeId=7195 }, new Item(196, 0){ UniqueId = 41293945, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(-303, 0, 1){ UniqueId = 41293945, RuntimeId=3768 }, new Item(224, -2){ UniqueId = 41293945, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(512, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(258, -2){ UniqueId = 41293945, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(512, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(258, -2){ UniqueId = 41293945, RuntimeId=0 }, "blast_furnace"),
				new SmeltingRecipe(new Item(524, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(306, -2){ UniqueId = 41293945, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(524, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(306, -2){ UniqueId = 41293945, RuntimeId=0 }, "blast_furnace"),
				new SmeltingRecipe(new Item(155, 0, 1){ UniqueId = 41293945, RuntimeId=6521 }, new Item(310, 0){ UniqueId = 41293945, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(220, 0, 1){ UniqueId = 41293945, RuntimeId=7704 }, new Item(318, 0){ UniqueId = 41293945, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(221, 0, 1){ UniqueId = 41293945, RuntimeId=5721 }, new Item(318, 2){ UniqueId = 41293945, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(222, 0, 1){ UniqueId = 41293945, RuntimeId=5571 }, new Item(318, 4){ UniqueId = 41293945, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(223, 0, 1){ UniqueId = 41293945, RuntimeId=5459 }, new Item(318, 6){ UniqueId = 41293945, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(224, 0, 1){ UniqueId = 41293945, RuntimeId=7846 }, new Item(318, 8){ UniqueId = 41293945, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(225, 0, 1){ UniqueId = 41293945, RuntimeId=5507 }, new Item(318, 10){ UniqueId = 41293945, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(226, 0, 1){ UniqueId = 41293945, RuntimeId=5752 }, new Item(318, 12){ UniqueId = 41293945, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(227, 0, 1){ UniqueId = 41293945, RuntimeId=4985 }, new Item(318, 14){ UniqueId = 41293945, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(228, 0, 1){ UniqueId = 41293945, RuntimeId=6750 }, new Item(318, 16){ UniqueId = 41293945, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(229, 0, 1){ UniqueId = 41293945, RuntimeId=3930 }, new Item(318, 18){ UniqueId = 41293945, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(219, 0, 1){ UniqueId = 41293945, RuntimeId=6492 }, new Item(318, 20){ UniqueId = 41293945, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(231, 0, 1){ UniqueId = 41293945, RuntimeId=685 }, new Item(318, 22){ UniqueId = 41293945, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(232, 0, 1){ UniqueId = 41293945, RuntimeId=894 }, new Item(318, 24){ UniqueId = 41293945, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(233, 0, 1){ UniqueId = 41293945, RuntimeId=5001 }, new Item(318, 26){ UniqueId = 41293945, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(234, 0, 1){ UniqueId = 41293945, RuntimeId=6574 }, new Item(318, 28){ UniqueId = 41293945, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(235, 0, 1){ UniqueId = 41293945, RuntimeId=488 }, new Item(318, 30){ UniqueId = 41293945, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(303, 0, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(324, 0){ UniqueId = 41293945, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(303, 0, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(324, 2){ UniqueId = 41293945, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(179, 0, 1){ UniqueId = 41293945, RuntimeId=6609 }, new Item(358, -2){ UniqueId = 41293945, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(263, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(524, -2){ UniqueId = 41293945, RuntimeId=0 }, "smoker"),
				new SmeltingRecipe(new Item(263, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(524, -2){ UniqueId = 41293945, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(263, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(524, -2){ UniqueId = 41293945, RuntimeId=0 }, "soul_campfire"),
				new SmeltingRecipe(new Item(263, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(524, -2){ UniqueId = 41293945, RuntimeId=0 }, "campfire"),
				new SmeltingRecipe(new Item(268, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(528, -2){ UniqueId = 41293945, RuntimeId=0 }, "smoker"),
				new SmeltingRecipe(new Item(268, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(528, -2){ UniqueId = 41293945, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(268, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(528, -2){ UniqueId = 41293945, RuntimeId=0 }, "soul_campfire"),
				new SmeltingRecipe(new Item(268, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(528, -2){ UniqueId = 41293945, RuntimeId=0 }, "campfire"),
				new SmeltingRecipe(new Item(269, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(530, -2){ UniqueId = 41293945, RuntimeId=0 }, "smoker"),
				new SmeltingRecipe(new Item(269, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(530, -2){ UniqueId = 41293945, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(269, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(530, -2){ UniqueId = 41293945, RuntimeId=0 }, "soul_campfire"),
				new SmeltingRecipe(new Item(269, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(530, -2){ UniqueId = 41293945, RuntimeId=0 }, "campfire"),
				new SmeltingRecipe(new Item(274, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(546, -2){ UniqueId = 41293945, RuntimeId=0 }, "smoker"),
				new SmeltingRecipe(new Item(274, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(546, -2){ UniqueId = 41293945, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(274, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(546, -2){ UniqueId = 41293945, RuntimeId=0 }, "soul_campfire"),
				new SmeltingRecipe(new Item(274, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(546, -2){ UniqueId = 41293945, RuntimeId=0 }, "campfire"),
				new SmeltingRecipe(new Item(276, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(550, -2){ UniqueId = 41293945, RuntimeId=0 }, "smoker"),
				new SmeltingRecipe(new Item(276, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(550, -2){ UniqueId = 41293945, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(276, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(550, -2){ UniqueId = 41293945, RuntimeId=0 }, "soul_campfire"),
				new SmeltingRecipe(new Item(276, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(550, -2){ UniqueId = 41293945, RuntimeId=0 }, "campfire"),
				new SmeltingRecipe(new Item(281, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(560, -2){ UniqueId = 41293945, RuntimeId=0 }, "smoker"),
				new SmeltingRecipe(new Item(281, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(560, -2){ UniqueId = 41293945, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(281, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(560, -2){ UniqueId = 41293945, RuntimeId=0 }, "soul_campfire"),
				new SmeltingRecipe(new Item(281, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(560, -2){ UniqueId = 41293945, RuntimeId=0 }, "campfire"),
				new SmeltingRecipe(new Item(289, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(576, -2){ UniqueId = 41293945, RuntimeId=0 }, "smoker"),
				new SmeltingRecipe(new Item(289, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(576, -2){ UniqueId = 41293945, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(289, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(576, -2){ UniqueId = 41293945, RuntimeId=0 }, "soul_campfire"),
				new SmeltingRecipe(new Item(289, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(576, -2){ UniqueId = 41293945, RuntimeId=0 }, "campfire"),
				new SmeltingRecipe(new Item(569, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(592, -2){ UniqueId = 41293945, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(569, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(592, -2){ UniqueId = 41293945, RuntimeId=0 }, "blast_furnace"),
				new SmeltingRecipe(new Item(569, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(594, -2){ UniqueId = 41293945, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(569, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(594, -2){ UniqueId = 41293945, RuntimeId=0 }, "blast_furnace"),
				new SmeltingRecipe(new Item(569, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(596, -2){ UniqueId = 41293945, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(569, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(596, -2){ UniqueId = 41293945, RuntimeId=0 }, "blast_furnace"),
				new SmeltingRecipe(new Item(569, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(614, -2){ UniqueId = 41293945, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(569, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(614, -2){ UniqueId = 41293945, RuntimeId=0 }, "blast_furnace"),
				new SmeltingRecipe(new Item(425, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(644, -2){ UniqueId = 41293945, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(425, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(644, -2){ UniqueId = 41293945, RuntimeId=0 }, "blast_furnace"),
				new SmeltingRecipe(new Item(425, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(646, -2){ UniqueId = 41293945, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(425, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(646, -2){ UniqueId = 41293945, RuntimeId=0 }, "blast_furnace"),
				new SmeltingRecipe(new Item(425, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(648, -2){ UniqueId = 41293945, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(425, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(648, -2){ UniqueId = 41293945, RuntimeId=0 }, "blast_furnace"),
				new SmeltingRecipe(new Item(425, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(650, -2){ UniqueId = 41293945, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(425, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(650, -2){ UniqueId = 41293945, RuntimeId=0 }, "blast_furnace"),
				new SmeltingRecipe(new Item(569, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(662, -2){ UniqueId = 41293945, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(569, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(662, -2){ UniqueId = 41293945, RuntimeId=0 }, "blast_furnace"),
				new SmeltingRecipe(new Item(425, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(666, -2){ UniqueId = 41293945, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(425, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(666, -2){ UniqueId = 41293945, RuntimeId=0 }, "blast_furnace"),
				new SmeltingRecipe(new Item(569, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(678, 0){ UniqueId = 41293945, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(569, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(678, 0){ UniqueId = 41293945, RuntimeId=0 }, "blast_furnace"),
				new SmeltingRecipe(new Item(569, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(680, 0){ UniqueId = 41293945, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(569, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(680, 0){ UniqueId = 41293945, RuntimeId=0 }, "blast_furnace"),
				new SmeltingRecipe(new Item(569, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(682, 0){ UniqueId = 41293945, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(569, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(682, 0){ UniqueId = 41293945, RuntimeId=0 }, "blast_furnace"),
				new SmeltingRecipe(new Item(569, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(684, 0){ UniqueId = 41293945, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(569, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(684, 0){ UniqueId = 41293945, RuntimeId=0 }, "blast_furnace"),
				new SmeltingRecipe(new Item(569, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(686, 0){ UniqueId = 41293945, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(569, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(686, 0){ UniqueId = 41293945, RuntimeId=0 }, "blast_furnace"),
				new SmeltingRecipe(new Item(569, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(688, 0){ UniqueId = 41293945, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(569, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(688, 0){ UniqueId = 41293945, RuntimeId=0 }, "blast_furnace"),
				new SmeltingRecipe(new Item(569, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(690, 0){ UniqueId = 41293945, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(569, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(690, 0){ UniqueId = 41293945, RuntimeId=0 }, "blast_furnace"),
				new SmeltingRecipe(new Item(569, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(692, 0){ UniqueId = 41293945, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(569, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(692, 0){ UniqueId = 41293945, RuntimeId=0 }, "blast_furnace"),
				new SmeltingRecipe(new Item(425, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(702, 0){ UniqueId = 41293945, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(425, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(702, 0){ UniqueId = 41293945, RuntimeId=0 }, "blast_furnace"),
				new SmeltingRecipe(new Item(425, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(704, 0){ UniqueId = 41293945, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(425, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(704, 0){ UniqueId = 41293945, RuntimeId=0 }, "blast_furnace"),
				new SmeltingRecipe(new Item(425, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(706, 0){ UniqueId = 41293945, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(425, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(706, 0){ UniqueId = 41293945, RuntimeId=0 }, "blast_furnace"),
				new SmeltingRecipe(new Item(425, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(708, 0){ UniqueId = 41293945, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(425, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(708, 0){ UniqueId = 41293945, RuntimeId=0 }, "blast_furnace"),
				new SmeltingRecipe(new Item(270, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(764, -2){ UniqueId = 41293945, RuntimeId=0 }, "smoker"),
				new SmeltingRecipe(new Item(270, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(764, -2){ UniqueId = 41293945, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(270, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(764, -2){ UniqueId = 41293945, RuntimeId=0 }, "soul_campfire"),
				new SmeltingRecipe(new Item(270, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(764, -2){ UniqueId = 41293945, RuntimeId=0 }, "campfire"),
				new SmeltingRecipe(new Item(383, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(768, -2){ UniqueId = 41293945, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(305, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(1010, -2){ UniqueId = 41293945, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(305, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(1010, -2){ UniqueId = 41293945, RuntimeId=0 }, "blast_furnace"),
				new SmeltingRecipe(new Item(306, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(1012, -2){ UniqueId = 41293945, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(306, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(1012, -2){ UniqueId = 41293945, RuntimeId=0 }, "blast_furnace"),
				new SmeltingRecipe(new Item(504, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(1014, -2){ UniqueId = 41293945, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(504, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(1014, -2){ UniqueId = 41293945, RuntimeId=0 }, "blast_furnace"),
				new SmeltingRecipe(new Item(569, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(1062, -2){ UniqueId = 41293945, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(569, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(1062, -2){ UniqueId = 41293945, RuntimeId=0 }, "blast_furnace"),
				new SmeltingRecipe(new Item(425, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(1064, -2){ UniqueId = 41293945, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(425, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(1064, -2){ UniqueId = 41293945, RuntimeId=0 }, "blast_furnace"),
				new SmeltingRecipe(new Item(551, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(1100, -2){ UniqueId = 41293945, RuntimeId=0 }, "smoker"),
				new SmeltingRecipe(new Item(551, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(1100, -2){ UniqueId = 41293945, RuntimeId=0 }, "furnace"),
				new SmeltingRecipe(new Item(551, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(1100, -2){ UniqueId = 41293945, RuntimeId=0 }, "soul_campfire"),
				new SmeltingRecipe(new Item(551, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(1100, -2){ UniqueId = 41293945, RuntimeId=0 }, "campfire"),
				new SmeltingRecipe(new Item(559, 32767, 1){ UniqueId = 41293945, RuntimeId=0 }, new Item(1116, -2){ UniqueId = 41293945, RuntimeId=0 }, "furnace"),
			};
		}

		public static void Add(Recipe recipe)
		{
			Log.InfoFormat("{0}", recipe.Id);
		}
	}
}