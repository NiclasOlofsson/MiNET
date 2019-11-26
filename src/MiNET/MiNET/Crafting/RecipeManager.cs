﻿#region LICENSE

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
				craftingData.someArraySize = 0;
				craftingData.someArraySize2 = 0;
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
				new ShapedRecipe(3, 2, new Item(333, 4, 1),
					new Item[]
					{
						new Item(5, 4),
						new Item(5, 4),
						new Item(5, 4),
						new Item(269, -1),
						new Item(5, 4),
						new Item(5, 4),
					}),
				new ShapedRecipe(2, 3, new Item(430, 0, 3),
					new Item[]
					{
						new Item(5, 4),
						new Item(5, 4),
						new Item(5, 4),
						new Item(5, 4),
						new Item(5, 4),
						new Item(5, 4),
					}),
				new ShapedRecipe(3, 2, new Item(85, 4, 3),
					new Item[]
					{
						new Item(5, 4),
						new Item(5, 4),
						new Item(280, -1),
						new Item(280, -1),
						new Item(5, 4),
						new Item(5, 4),
					}),
				new ShapedRecipe(3, 2, new Item(187, 0, 1),
					new Item[]
					{
						new Item(280, -1),
						new Item(280, -1),
						new Item(5, 4),
						new Item(5, 4),
						new Item(280, -1),
						new Item(280, -1),
					}),
				new ShapedRecipe(1, 1, new Item(5, 4, 4),
					new Item[]
					{
						new Item(162, 0),
					}),
				new ShapedRecipe(1, 1, new Item(5, 4, 4),
					new Item[]
					{
						new Item(-8, -1),
					}),
				new ShapedRecipe(1, 1, new Item(5, 4, 4),
					new Item[]
					{
						new Item(-212, 12),
					}),
				new ShapedRecipe(1, 1, new Item(5, 4, 4),
					new Item[]
					{
						new Item(-212, 10),
					}),
				new ShapedRecipe(3, 3, new Item(163, 0, 4),
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
					}),
				new ShapedRecipe(2, 2, new Item(-212, 4, 3),
					new Item[]
					{
						new Item(162, 0),
						new Item(162, 0),
						new Item(162, 0),
						new Item(162, 0),
					}),
				new ShapedRecipe(3, 1, new Item(158, 4, 6),
					new Item[]
					{
						new Item(5, 4),
						new Item(5, 4),
						new Item(5, 4),
					}),
				new ShapedRecipe(2, 2, new Item(-212, 12, 3),
					new Item[]
					{
						new Item(-8, -1),
						new Item(-8, -1),
						new Item(-8, -1),
						new Item(-8, -1),
					}),
				new ShapedRecipe(3, 3, new Item(126, 0, 6),
					new Item[]
					{
						new Item(265, -1),
						new Item(265, -1),
						new Item(265, -1),
						new Item(280, -1),
						new Item(76, -1),
						new Item(280, -1),
						new Item(265, -1),
						new Item(265, -1),
						new Item(265, -1),
					}),
				new ShapelessRecipe(new Item(1, 5, 2),
					new List<Item>
					{
						new Item(1, 3, 1),
						new Item(4, -1, 1),
					}),
				new ShapedRecipe(3, 3, new Item(-171, 0, 4),
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
					}),
				new ShapedRecipe(3, 2, new Item(139, 4, 6),
					new Item[]
					{
						new Item(1, 5),
						new Item(1, 5),
						new Item(1, 5),
						new Item(1, 5),
						new Item(1, 5),
						new Item(1, 5),
					}),
				new ShapedRecipe(3, 3, new Item(145, 0, 1),
					new Item[]
					{
						new Item(42, -1),
						new Item(0, 0),
						new Item(265, -1),
						new Item(42, -1),
						new Item(265, -1),
						new Item(265, -1),
						new Item(42, -1),
						new Item(0, 0),
						new Item(265, -1),
					}),
				new ShapedRecipe(3, 3, new Item(425, 0, 1),
					new Item[]
					{
						new Item(280, -1),
						new Item(0, 0),
						new Item(280, -1),
						new Item(280, -1),
						new Item(280, -1),
						new Item(44, 0),
						new Item(280, -1),
						new Item(0, 0),
						new Item(280, -1),
					}),
				new ShapedRecipe(1, 3, new Item(262, 0, 4),
					new Item[]
					{
						new Item(318, -1),
						new Item(280, -1),
						new Item(288, -1),
					}),
				new ShapelessRecipe(new Item(434, 4, 1),
					new List<Item>
					{
						new Item(339, -1, 1),
						new Item(45, -1, 1),
					}),
				new ShapelessRecipe(new Item(434, 0, 1),
					new List<Item>
					{
						new Item(339, -1, 1),
						new Item(397, 4, 1),
					}),
				new ShapelessRecipe(new Item(434, 2, 1),
					new List<Item>
					{
						new Item(339, -1, 1),
						new Item(38, 8, 1),
					}),
				new ShapelessRecipe(new Item(434, 1, 1),
					new List<Item>
					{
						new Item(339, -1, 1),
						new Item(397, 1, 1),
					}),
				new ShapelessRecipe(new Item(434, 3, 1),
					new List<Item>
					{
						new Item(339, -1, 1),
						new Item(466, -1, 1),
					}),
				new ShapelessRecipe(new Item(434, 5, 1),
					new List<Item>
					{
						new Item(339, -1, 1),
						new Item(106, -1, 1),
					}),
				new ShapedRecipe(3, 3, new Item(-203, 0, 1),
					new Item[]
					{
						new Item(280, -1),
						new Item(280, -1),
						new Item(280, -1),
						new Item(158, -1),
						new Item(0, 0),
						new Item(158, -1),
						new Item(280, -1),
						new Item(280, -1),
						new Item(280, -1),
					}),
				new ShapelessRecipe(new Item(395, 2, 1),
					new List<Item>
					{
						new Item(395, 1, 1),
						new Item(345, -1, 1),
					}),
				new ShapedRecipe(3, 3, new Item(138, 0, 1),
					new Item[]
					{
						new Item(20, -1),
						new Item(20, -1),
						new Item(49, -1),
						new Item(20, -1),
						new Item(399, -1),
						new Item(49, -1),
						new Item(20, -1),
						new Item(20, -1),
						new Item(49, -1),
					}),
				new ShapelessRecipe(new Item(459, 0, 1),
					new List<Item>
					{
						new Item(281, -1, 1),
						new Item(457, -1, 1),
						new Item(457, -1, 1),
						new Item(457, -1, 1),
						new Item(457, -1, 1),
						new Item(457, -1, 1),
						new Item(457, -1, 1),
					}),
				new ShapedRecipe(3, 2, new Item(333, 2, 1),
					new Item[]
					{
						new Item(5, 2),
						new Item(5, 2),
						new Item(5, 2),
						new Item(269, -1),
						new Item(5, 2),
						new Item(5, 2),
					}),
				new ShapedRecipe(2, 3, new Item(428, 0, 3),
					new Item[]
					{
						new Item(5, 2),
						new Item(5, 2),
						new Item(5, 2),
						new Item(5, 2),
						new Item(5, 2),
						new Item(5, 2),
					}),
				new ShapedRecipe(3, 2, new Item(85, 2, 3),
					new Item[]
					{
						new Item(5, 2),
						new Item(5, 2),
						new Item(280, -1),
						new Item(280, -1),
						new Item(5, 2),
						new Item(5, 2),
					}),
				new ShapedRecipe(3, 2, new Item(184, 0, 1),
					new Item[]
					{
						new Item(280, -1),
						new Item(280, -1),
						new Item(5, 2),
						new Item(5, 2),
						new Item(280, -1),
						new Item(280, -1),
					}),
				new ShapedRecipe(1, 1, new Item(5, 2, 4),
					new Item[]
					{
						new Item(17, 2),
					}),
				new ShapedRecipe(1, 1, new Item(5, 2, 4),
					new Item[]
					{
						new Item(-6, -1),
					}),
				new ShapedRecipe(1, 1, new Item(5, 2, 4),
					new Item[]
					{
						new Item(-212, 10),
					}),
				new ShapedRecipe(1, 1, new Item(5, 2, 4),
					new Item[]
					{
						new Item(-212, 2),
					}),
				new ShapedRecipe(3, 3, new Item(135, 0, 4),
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
					}),
				new ShapedRecipe(2, 2, new Item(-212, 2, 3),
					new Item[]
					{
						new Item(17, 2),
						new Item(17, 2),
						new Item(17, 2),
						new Item(17, 2),
					}),
				new ShapedRecipe(3, 1, new Item(158, 2, 6),
					new Item[]
					{
						new Item(5, 2),
						new Item(5, 2),
						new Item(5, 2),
					}),
				new ShapedRecipe(2, 2, new Item(-212, 10, 3),
					new Item[]
					{
						new Item(-6, -1),
						new Item(-6, -1),
						new Item(-6, -1),
						new Item(-6, -1),
					}),
				new ShapedRecipe(3, 3, new Item(446, 0, 1),
					new Item[]
					{
						new Item(35, 15),
						new Item(35, 15),
						new Item(0, 0),
						new Item(35, 15),
						new Item(35, 15),
						new Item(280, -1),
						new Item(35, 15),
						new Item(35, 15),
						new Item(0, 0),
					}),
				new ShapedRecipe(2, 1, new Item(171, 15, 3),
					new Item[]
					{
						new Item(35, 15),
						new Item(35, 15),
					}),
				new ShapedRecipe(3, 2, new Item(160, 0, 16),
					new Item[]
					{
						new Item(241, 0),
						new Item(241, 0),
						new Item(241, 0),
						new Item(241, 0),
						new Item(241, 0),
						new Item(241, 0),
					}),
				new ShapelessRecipe(new Item(237, 15, 8),
					new List<Item>
					{
						new Item(351, 16, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(13, -1, 1),
						new Item(13, -1, 1),
						new Item(13, -1, 1),
						new Item(13, -1, 1),
					}),
				new ShapedRecipe(3, 3, new Item(160, 0, 8),
					new Item[]
					{
						new Item(102, -1),
						new Item(102, -1),
						new Item(102, -1),
						new Item(102, -1),
						new Item(351, 19),
						new Item(102, -1),
						new Item(102, -1),
						new Item(102, -1),
						new Item(102, -1),
					}),
				new ShapelessRecipe(new Item(351, 16, 1),
					new List<Item>
					{
						new Item(351, 0, 1),
					}),
				new ShapedRecipe(3, 3, new Item(241, 15, 8),
					new Item[]
					{
						new Item(20, -1),
						new Item(20, -1),
						new Item(20, -1),
						new Item(20, -1),
						new Item(351, 16),
						new Item(20, -1),
						new Item(20, -1),
						new Item(20, -1),
						new Item(20, -1),
					}),
				new ShapedRecipe(3, 3, new Item(159, 0, 8),
					new Item[]
					{
						new Item(172, -1),
						new Item(172, -1),
						new Item(172, -1),
						new Item(172, -1),
						new Item(351, 19),
						new Item(172, -1),
						new Item(172, -1),
						new Item(172, -1),
						new Item(172, -1),
					}),
				new ShapedRecipe(3, 2, new Item(160, 15, 16),
					new Item[]
					{
						new Item(241, 15),
						new Item(241, 15),
						new Item(241, 15),
						new Item(241, 15),
						new Item(241, 15),
						new Item(241, 15),
					}),
				new ShapedRecipe(3, 3, new Item(160, 15, 8),
					new Item[]
					{
						new Item(102, -1),
						new Item(102, -1),
						new Item(102, -1),
						new Item(102, -1),
						new Item(351, 16),
						new Item(102, -1),
						new Item(102, -1),
						new Item(102, -1),
						new Item(102, -1),
					}),
				new ShapedRecipe(3, 3, new Item(159, 15, 8),
					new Item[]
					{
						new Item(172, -1),
						new Item(172, -1),
						new Item(172, -1),
						new Item(172, -1),
						new Item(351, 16),
						new Item(172, -1),
						new Item(172, -1),
						new Item(172, -1),
						new Item(172, -1),
					}),
				new ShapedRecipe(2, 3, new Item(324, 0, 3),
					new Item[]
					{
						new Item(5, 0),
						new Item(5, 0),
						new Item(5, 0),
						new Item(5, 0),
						new Item(5, 0),
						new Item(5, 0),
					}),
				new ShapedRecipe(3, 3, new Item(-196, 0, 1),
					new Item[]
					{
						new Item(265, -1),
						new Item(265, -1),
						new Item(-183, -1),
						new Item(265, -1),
						new Item(61, -1),
						new Item(-183, -1),
						new Item(265, -1),
						new Item(265, -1),
						new Item(-183, -1),
					}),
				new ShapelessRecipe(new Item(377, 0, 2),
					new List<Item>
					{
						new Item(369, -1, 1),
					}),
				new ShapedRecipe(3, 3, new Item(446, 4, 1),
					new Item[]
					{
						new Item(35, 11),
						new Item(35, 11),
						new Item(0, 0),
						new Item(35, 11),
						new Item(35, 11),
						new Item(280, -1),
						new Item(35, 11),
						new Item(35, 11),
						new Item(0, 0),
					}),
				new ShapedRecipe(2, 1, new Item(171, 11, 3),
					new Item[]
					{
						new Item(35, 11),
						new Item(35, 11),
					}),
				new ShapedRecipe(3, 3, new Item(171, 11, 8),
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
					}),
				new ShapelessRecipe(new Item(237, 11, 8),
					new List<Item>
					{
						new Item(351, 18, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(13, -1, 1),
						new Item(13, -1, 1),
						new Item(13, -1, 1),
						new Item(13, -1, 1),
					}),
				new ShapelessRecipe(new Item(386, 0, 1),
					new List<Item>
					{
						new Item(340, -1, 1),
						new Item(351, 0, 1),
						new Item(288, -1, 1),
					}),
				new ShapelessRecipe(new Item(351, 18, 1),
					new List<Item>
					{
						new Item(38, 9, 1),
					}),
				new ShapelessRecipe(new Item(351, 18, 1),
					new List<Item>
					{
						new Item(351, 4, 1),
					}),
				new ShapedRecipe(3, 3, new Item(-11, 0, 1),
					new Item[]
					{
						new Item(174, -1),
						new Item(174, -1),
						new Item(174, -1),
						new Item(174, -1),
						new Item(174, -1),
						new Item(174, -1),
						new Item(174, -1),
						new Item(174, -1),
						new Item(174, -1),
					}),
				new ShapedRecipe(3, 3, new Item(241, 11, 8),
					new Item[]
					{
						new Item(20, -1),
						new Item(20, -1),
						new Item(20, -1),
						new Item(20, -1),
						new Item(351, 18),
						new Item(20, -1),
						new Item(20, -1),
						new Item(20, -1),
						new Item(20, -1),
					}),
				new ShapedRecipe(1, 1, new Item(5, 5, 4),
					new Item[]
					{
						new Item(162, 1),
					}),
				new ShapedRecipe(3, 2, new Item(160, 11, 16),
					new Item[]
					{
						new Item(241, 11),
						new Item(241, 11),
						new Item(241, 11),
						new Item(241, 11),
						new Item(241, 11),
						new Item(241, 11),
					}),
				new ShapedRecipe(3, 3, new Item(160, 11, 8),
					new Item[]
					{
						new Item(102, -1),
						new Item(102, -1),
						new Item(102, -1),
						new Item(102, -1),
						new Item(351, 18),
						new Item(102, -1),
						new Item(102, -1),
						new Item(102, -1),
						new Item(102, -1),
					}),
				new ShapedRecipe(3, 3, new Item(159, 11, 8),
					new Item[]
					{
						new Item(172, -1),
						new Item(172, -1),
						new Item(172, -1),
						new Item(172, -1),
						new Item(351, 18),
						new Item(172, -1),
						new Item(172, -1),
						new Item(172, -1),
						new Item(172, -1),
					}),
				new ShapedRecipe(3, 3, new Item(446, 11, 1),
					new Item[]
					{
						new Item(35, 4),
						new Item(35, 4),
						new Item(0, 0),
						new Item(35, 4),
						new Item(35, 4),
						new Item(280, -1),
						new Item(35, 4),
						new Item(35, 4),
						new Item(0, 0),
					}),
				new ShapedRecipe(3, 2, new Item(333, 0, 1),
					new Item[]
					{
						new Item(5, 0),
						new Item(5, 0),
						new Item(5, 0),
						new Item(269, -1),
						new Item(5, 0),
						new Item(5, 0),
					}),
				new ShapedRecipe(3, 3, new Item(216, 0, 1),
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
					}),
				new ShapelessRecipe(new Item(351, 15, 9),
					new List<Item>
					{
						new Item(216, -1, 1),
					}),
				new ShapelessRecipe(new Item(351, 15, 3),
					new List<Item>
					{
						new Item(352, -1, 1),
					}),
				new ShapelessRecipe(new Item(340, 0, 1),
					new List<Item>
					{
						new Item(339, 0, 1),
						new Item(339, 0, 1),
						new Item(339, 0, 1),
						new Item(334, 0, 1),
					}),
				new ShapedRecipe(3, 3, new Item(261, 0, 1),
					new Item[]
					{
						new Item(0, 0),
						new Item(280, -1),
						new Item(0, 0),
						new Item(280, -1),
						new Item(0, 0),
						new Item(280, -1),
						new Item(287, -1),
						new Item(287, -1),
						new Item(287, -1),
					}),
				new ShapedRecipe(3, 1, new Item(297, 0, 1),
					new Item[]
					{
						new Item(296, -1),
						new Item(296, -1),
						new Item(296, -1),
					}),
				new ShapedRecipe(3, 2, new Item(379, 0, 1),
					new Item[]
					{
						new Item(0, 0),
						new Item(0, 0),
						new Item(4, -1),
						new Item(369, -1),
						new Item(4, -1),
						new Item(4, -1),
					}),
				new ShapedRecipe(2, 2, new Item(45, 0, 1),
					new Item[]
					{
						new Item(336, -1),
						new Item(336, -1),
						new Item(336, -1),
						new Item(336, -1),
					}),
				new ShapedRecipe(3, 3, new Item(108, 0, 4),
					new Item[]
					{
						new Item(45, -1),
						new Item(45, -1),
						new Item(45, -1),
						new Item(0, 0),
						new Item(45, -1),
						new Item(45, -1),
						new Item(0, 0),
						new Item(0, 0),
						new Item(45, -1),
					}),
				new ShapedRecipe(3, 2, new Item(139, 6, 6),
					new Item[]
					{
						new Item(45, -1),
						new Item(45, -1),
						new Item(45, -1),
						new Item(45, -1),
						new Item(45, -1),
						new Item(45, -1),
					}),
				new ShapedRecipe(3, 3, new Item(446, 3, 1),
					new Item[]
					{
						new Item(35, 12),
						new Item(35, 12),
						new Item(0, 0),
						new Item(35, 12),
						new Item(35, 12),
						new Item(280, -1),
						new Item(35, 12),
						new Item(35, 12),
						new Item(0, 0),
					}),
				new ShapedRecipe(2, 1, new Item(171, 12, 3),
					new Item[]
					{
						new Item(35, 12),
						new Item(35, 12),
					}),
				new ShapedRecipe(3, 3, new Item(171, 12, 8),
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
					}),
				new ShapelessRecipe(new Item(237, 12, 8),
					new List<Item>
					{
						new Item(351, 17, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(13, -1, 1),
						new Item(13, -1, 1),
						new Item(13, -1, 1),
						new Item(13, -1, 1),
					}),
				new ShapedRecipe(2, 1, new Item(171, 4, 3),
					new Item[]
					{
						new Item(35, 4),
						new Item(35, 4),
					}),
				new ShapelessRecipe(new Item(351, 17, 1),
					new List<Item>
					{
						new Item(351, 3, 1),
					}),
				new ShapedRecipe(3, 3, new Item(241, 12, 8),
					new Item[]
					{
						new Item(20, -1),
						new Item(20, -1),
						new Item(20, -1),
						new Item(20, -1),
						new Item(351, 17),
						new Item(20, -1),
						new Item(20, -1),
						new Item(20, -1),
						new Item(20, -1),
					}),
				new ShapedRecipe(3, 3, new Item(171, 4, 8),
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
					}),
				new ShapedRecipe(3, 2, new Item(160, 12, 16),
					new Item[]
					{
						new Item(241, 12),
						new Item(241, 12),
						new Item(241, 12),
						new Item(241, 12),
						new Item(241, 12),
						new Item(241, 12),
					}),
				new ShapedRecipe(3, 3, new Item(160, 12, 8),
					new Item[]
					{
						new Item(102, -1),
						new Item(102, -1),
						new Item(102, -1),
						new Item(102, -1),
						new Item(351, 17),
						new Item(102, -1),
						new Item(102, -1),
						new Item(102, -1),
						new Item(102, -1),
					}),
				new ShapedRecipe(3, 3, new Item(159, 12, 8),
					new Item[]
					{
						new Item(172, -1),
						new Item(172, -1),
						new Item(172, -1),
						new Item(172, -1),
						new Item(351, 17),
						new Item(172, -1),
						new Item(172, -1),
						new Item(172, -1),
						new Item(172, -1),
					}),
				new ShapelessRecipe(new Item(237, 4, 8),
					new List<Item>
					{
						new Item(351, 11, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(13, -1, 1),
						new Item(13, -1, 1),
						new Item(13, -1, 1),
						new Item(13, -1, 1),
					}),
				new ShapedRecipe(3, 2, new Item(325, 0, 1),
					new Item[]
					{
						new Item(265, -1),
						new Item(265, -1),
						new Item(265, -1),
						new Item(0, 0),
						new Item(0, 0),
						new Item(0, 0),
					}),
				new ShapedRecipe(3, 3, new Item(325, 0, 3),
					new Item[]
					{
						new Item(325, 1),
						new Item(353, -1),
						new Item(296, -1),
						new Item(325, 1),
						new Item(344, -1),
						new Item(296, -1),
						new Item(325, 1),
						new Item(353, -1),
						new Item(296, -1),
					}),
				new ShapedRecipe(3, 3, new Item(-209, 0, 1),
					new Item[]
					{
						new Item(0, 0),
						new Item(280, -1),
						new Item(17, -1),
						new Item(280, -1),
						new Item(263, -1),
						new Item(17, -1),
						new Item(0, 0),
						new Item(280, -1),
						new Item(17, -1),
					}),
				new ShapedRecipe(3, 3, new Item(-209, 0, 1),
					new Item[]
					{
						new Item(0, 0),
						new Item(280, -1),
						new Item(162, -1),
						new Item(280, -1),
						new Item(263, -1),
						new Item(162, -1),
						new Item(0, 0),
						new Item(280, -1),
						new Item(162, -1),
					}),
				new ShapedRecipe(2, 2, new Item(398, 0, 1),
					new Item[]
					{
						new Item(346, -1),
						new Item(0, 0),
						new Item(0, 0),
						new Item(391, -1),
					}),
				new ShapelessRecipe(new Item(-200, 0, 1),
					new List<Item>
					{
						new Item(58, -1, 1),
						new Item(339, -1, 1),
					}),
				new ShapedRecipe(3, 3, new Item(380, 0, 1),
					new Item[]
					{
						new Item(265, -1),
						new Item(265, -1),
						new Item(265, -1),
						new Item(0, 0),
						new Item(0, 0),
						new Item(265, -1),
						new Item(265, -1),
						new Item(265, -1),
						new Item(265, -1),
					}),
				new ShapedRecipe(1, 2, new Item(342, 0, 1),
					new Item[]
					{
						new Item(54, -1),
						new Item(328, -1),
					}),
				new ShapedRecipe(2, 2, new Item(82, 0, 1),
					new Item[]
					{
						new Item(337, -1),
						new Item(337, -1),
						new Item(337, -1),
						new Item(337, -1),
					}),
				new ShapedRecipe(3, 3, new Item(347, 0, 1),
					new Item[]
					{
						new Item(0, 0),
						new Item(266, -1),
						new Item(0, 0),
						new Item(266, -1),
						new Item(331, -1),
						new Item(266, -1),
						new Item(0, 0),
						new Item(266, -1),
						new Item(0, 0),
					}),
				new ShapedRecipe(1, 1, new Item(263, 0, 9),
					new Item[]
					{
						new Item(173, -1),
					}),
				new ShapedRecipe(3, 3, new Item(173, 0, 1),
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
					}),
				new ShapedRecipe(2, 2, new Item(3, 1, 4),
					new Item[]
					{
						new Item(3, 0),
						new Item(13, -1),
						new Item(13, -1),
						new Item(3, 0),
					}),
				new ShapedRecipe(3, 3, new Item(67, 0, 4),
					new Item[]
					{
						new Item(4, -1),
						new Item(4, -1),
						new Item(4, -1),
						new Item(0, 0),
						new Item(4, -1),
						new Item(4, -1),
						new Item(0, 0),
						new Item(0, 0),
						new Item(4, -1),
					}),
				new ShapedRecipe(3, 2, new Item(139, 0, 6),
					new Item[]
					{
						new Item(4, -1),
						new Item(4, -1),
						new Item(4, -1),
						new Item(4, -1),
						new Item(4, -1),
						new Item(4, -1),
					}),
				new ShapelessRecipe(new Item(287, 0, 9),
					new List<Item>
					{
						new Item(30, -1, 1),
					}),
				new ShapedRecipe(3, 3, new Item(404, 0, 1),
					new Item[]
					{
						new Item(0, 0),
						new Item(76, -1),
						new Item(1, 0),
						new Item(76, -1),
						new Item(406, -1),
						new Item(1, 0),
						new Item(0, 0),
						new Item(76, -1),
						new Item(1, 0),
					}),
				new ShapedRecipe(3, 3, new Item(345, 0, 1),
					new Item[]
					{
						new Item(0, 0),
						new Item(265, -1),
						new Item(0, 0),
						new Item(265, -1),
						new Item(331, -1),
						new Item(265, -1),
						new Item(0, 0),
						new Item(265, -1),
						new Item(0, 0),
					}),
				new ShapedRecipe(3, 3, new Item(-213, 0, 1),
					new Item[]
					{
						new Item(158, -1),
						new Item(158, -1),
						new Item(158, -1),
						new Item(0, 0),
						new Item(0, 0),
						new Item(158, -1),
						new Item(158, -1),
						new Item(158, -1),
						new Item(158, -1),
					}),
				new ShapedRecipe(3, 3, new Item(-157, 0, 1),
					new Item[]
					{
						new Item(465, -1),
						new Item(465, -1),
						new Item(465, -1),
						new Item(465, -1),
						new Item(467, -1),
						new Item(465, -1),
						new Item(465, -1),
						new Item(465, -1),
						new Item(465, -1),
					}),
				new ShapedRecipe(3, 1, new Item(357, 0, 8),
					new Item[]
					{
						new Item(296, -1),
						new Item(351, 3),
						new Item(296, -1),
					}),
				new ShapedRecipe(3, 3, new Item(471, 0, 1),
					new Item[]
					{
						new Item(280, -1),
						new Item(287, -1),
						new Item(0, 0),
						new Item(265, -1),
						new Item(131, -1),
						new Item(280, -1),
						new Item(280, -1),
						new Item(287, -1),
						new Item(0, 0),
					}),
				new ShapedRecipe(3, 3, new Item(446, 6, 1),
					new Item[]
					{
						new Item(35, 9),
						new Item(35, 9),
						new Item(0, 0),
						new Item(35, 9),
						new Item(35, 9),
						new Item(280, -1),
						new Item(35, 9),
						new Item(35, 9),
						new Item(0, 0),
					}),
				new ShapedRecipe(2, 1, new Item(171, 9, 3),
					new Item[]
					{
						new Item(35, 9),
						new Item(35, 9),
					}),
				new ShapedRecipe(3, 3, new Item(171, 9, 8),
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
					}),
				new ShapelessRecipe(new Item(237, 9, 8),
					new List<Item>
					{
						new Item(351, 6, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(13, -1, 1),
						new Item(13, -1, 1),
						new Item(13, -1, 1),
						new Item(13, -1, 1),
					}),
				new ShapelessRecipe(new Item(351, 6, 2),
					new List<Item>
					{
						new Item(351, 18, 1),
						new Item(351, 2, 1),
					}),
				new ShapelessRecipe(new Item(351, 11, 1),
					new List<Item>
					{
						new Item(37, 0, 1),
					}),
				new ShapedRecipe(3, 3, new Item(241, 9, 8),
					new Item[]
					{
						new Item(20, -1),
						new Item(20, -1),
						new Item(20, -1),
						new Item(20, -1),
						new Item(351, 6),
						new Item(20, -1),
						new Item(20, -1),
						new Item(20, -1),
						new Item(20, -1),
					}),
				new ShapedRecipe(3, 2, new Item(160, 9, 16),
					new Item[]
					{
						new Item(241, 9),
						new Item(241, 9),
						new Item(241, 9),
						new Item(241, 9),
						new Item(241, 9),
						new Item(241, 9),
					}),
				new ShapedRecipe(3, 3, new Item(160, 9, 8),
					new Item[]
					{
						new Item(102, -1),
						new Item(102, -1),
						new Item(102, -1),
						new Item(102, -1),
						new Item(351, 6),
						new Item(102, -1),
						new Item(102, -1),
						new Item(102, -1),
						new Item(102, -1),
					}),
				new ShapedRecipe(3, 3, new Item(159, 9, 8),
					new Item[]
					{
						new Item(172, -1),
						new Item(172, -1),
						new Item(172, -1),
						new Item(172, -1),
						new Item(351, 6),
						new Item(172, -1),
						new Item(172, -1),
						new Item(172, -1),
						new Item(172, -1),
					}),
				new ShapedRecipe(3, 2, new Item(333, 5, 1),
					new Item[]
					{
						new Item(5, 5),
						new Item(5, 5),
						new Item(5, 5),
						new Item(269, -1),
						new Item(5, 5),
						new Item(5, 5),
					}),
				new ShapedRecipe(2, 3, new Item(431, 0, 3),
					new Item[]
					{
						new Item(5, 5),
						new Item(5, 5),
						new Item(5, 5),
						new Item(5, 5),
						new Item(5, 5),
						new Item(5, 5),
					}),
				new ShapedRecipe(3, 2, new Item(85, 5, 3),
					new Item[]
					{
						new Item(5, 5),
						new Item(5, 5),
						new Item(280, -1),
						new Item(280, -1),
						new Item(5, 5),
						new Item(5, 5),
					}),
				new ShapedRecipe(3, 2, new Item(186, 0, 1),
					new Item[]
					{
						new Item(280, -1),
						new Item(280, -1),
						new Item(5, 5),
						new Item(5, 5),
						new Item(280, -1),
						new Item(280, -1),
					}),
				new ShapelessRecipe(new Item(351, 11, 2),
					new List<Item>
					{
						new Item(175, 0, 1),
					}),
				new ShapedRecipe(1, 1, new Item(5, 5, 4),
					new Item[]
					{
						new Item(-9, -1),
					}),
				new ShapedRecipe(1, 1, new Item(5, 5, 4),
					new Item[]
					{
						new Item(-212, 13),
					}),
				new ShapedRecipe(1, 1, new Item(5, 5, 4),
					new Item[]
					{
						new Item(-212, 5),
					}),
				new ShapedRecipe(3, 3, new Item(164, 0, 4),
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
					}),
				new ShapedRecipe(2, 2, new Item(-212, 5, 3),
					new Item[]
					{
						new Item(162, 1),
						new Item(162, 1),
						new Item(162, 1),
						new Item(162, 1),
					}),
				new ShapedRecipe(3, 1, new Item(158, 5, 6),
					new Item[]
					{
						new Item(5, 5),
						new Item(5, 5),
						new Item(5, 5),
					}),
				new ShapedRecipe(2, 2, new Item(-212, 13, 3),
					new Item[]
					{
						new Item(-9, -1),
						new Item(-9, -1),
						new Item(-9, -1),
						new Item(-9, -1),
					}),
				new ShapedRecipe(3, 3, new Item(168, 1, 1),
					new Item[]
					{
						new Item(409, -1),
						new Item(409, -1),
						new Item(409, -1),
						new Item(409, -1),
						new Item(351, 16),
						new Item(409, -1),
						new Item(409, -1),
						new Item(409, -1),
						new Item(409, -1),
					}),
				new ShapedRecipe(3, 3, new Item(241, 4, 8),
					new Item[]
					{
						new Item(20, -1),
						new Item(20, -1),
						new Item(20, -1),
						new Item(20, -1),
						new Item(351, 11),
						new Item(20, -1),
						new Item(20, -1),
						new Item(20, -1),
						new Item(20, -1),
					}),
				new ShapedRecipe(3, 3, new Item(28, 0, 6),
					new Item[]
					{
						new Item(265, -1),
						new Item(265, -1),
						new Item(265, -1),
						new Item(0, 0),
						new Item(70, -1),
						new Item(331, -1),
						new Item(265, -1),
						new Item(265, -1),
						new Item(265, -1),
					}),
				new ShapedRecipe(1, 1, new Item(264, 0, 9),
					new Item[]
					{
						new Item(57, -1),
					}),
				new ShapedRecipe(2, 3, new Item(279, 0, 1),
					new Item[]
					{
						new Item(264, -1),
						new Item(280, -1),
						new Item(264, -1),
						new Item(0, 0),
						new Item(264, -1),
						new Item(280, -1),
					}),
				new ShapedRecipe(3, 3, new Item(57, 0, 1),
					new Item[]
					{
						new Item(264, -1),
						new Item(264, -1),
						new Item(264, -1),
						new Item(264, -1),
						new Item(264, -1),
						new Item(264, -1),
						new Item(264, -1),
						new Item(264, -1),
						new Item(264, -1),
					}),
				new ShapedRecipe(3, 2, new Item(313, 0, 1),
					new Item[]
					{
						new Item(264, -1),
						new Item(264, -1),
						new Item(0, 0),
						new Item(0, 0),
						new Item(264, -1),
						new Item(264, -1),
					}),
				new ShapedRecipe(3, 3, new Item(311, 0, 1),
					new Item[]
					{
						new Item(264, -1),
						new Item(264, -1),
						new Item(264, -1),
						new Item(0, 0),
						new Item(264, -1),
						new Item(264, -1),
						new Item(264, -1),
						new Item(264, -1),
						new Item(264, -1),
					}),
				new ShapedRecipe(3, 2, new Item(310, 0, 1),
					new Item[]
					{
						new Item(264, -1),
						new Item(264, -1),
						new Item(0, 0),
						new Item(264, -1),
						new Item(264, -1),
						new Item(264, -1),
					}),
				new ShapedRecipe(2, 3, new Item(293, 0, 1),
					new Item[]
					{
						new Item(264, -1),
						new Item(280, -1),
						new Item(264, -1),
						new Item(0, 0),
						new Item(0, 0),
						new Item(280, -1),
					}),
				new ShapedRecipe(3, 3, new Item(312, 0, 1),
					new Item[]
					{
						new Item(264, -1),
						new Item(264, -1),
						new Item(264, -1),
						new Item(264, -1),
						new Item(0, 0),
						new Item(0, 0),
						new Item(264, -1),
						new Item(264, -1),
						new Item(264, -1),
					}),
				new ShapedRecipe(3, 3, new Item(278, 0, 1),
					new Item[]
					{
						new Item(264, -1),
						new Item(0, 0),
						new Item(0, 0),
						new Item(264, -1),
						new Item(280, -1),
						new Item(280, -1),
						new Item(264, -1),
						new Item(0, 0),
						new Item(0, 0),
					}),
				new ShapedRecipe(1, 3, new Item(277, 0, 1),
					new Item[]
					{
						new Item(264, -1),
						new Item(280, -1),
						new Item(280, -1),
					}),
				new ShapedRecipe(1, 3, new Item(276, 0, 1),
					new Item[]
					{
						new Item(264, -1),
						new Item(264, -1),
						new Item(280, -1),
					}),
				new ShapedRecipe(2, 2, new Item(1, 3, 2),
					new Item[]
					{
						new Item(4, -1),
						new Item(406, -1),
						new Item(406, -1),
						new Item(4, -1),
					}),
				new ShapedRecipe(3, 3, new Item(-170, 0, 4),
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
					}),
				new ShapedRecipe(3, 2, new Item(139, 3, 6),
					new Item[]
					{
						new Item(1, 3),
						new Item(1, 3),
						new Item(1, 3),
						new Item(1, 3),
						new Item(1, 3),
						new Item(1, 3),
					}),
				new ShapedRecipe(3, 3, new Item(23, 3, 1),
					new Item[]
					{
						new Item(4, -1),
						new Item(4, -1),
						new Item(4, -1),
						new Item(4, -1),
						new Item(261, -1),
						new Item(331, -1),
						new Item(4, -1),
						new Item(4, -1),
						new Item(4, -1),
					}),
				new ShapedRecipe(1, 1, new Item(464, 0, 9),
					new Item[]
					{
						new Item(-139, -1),
					}),
				new ShapedRecipe(3, 3, new Item(-139, 0, 1),
					new Item[]
					{
						new Item(464, -1),
						new Item(464, -1),
						new Item(464, -1),
						new Item(464, -1),
						new Item(464, -1),
						new Item(464, -1),
						new Item(464, -1),
						new Item(464, -1),
						new Item(464, -1),
					}),
				new ShapedRecipe(3, 3, new Item(125, 3, 1),
					new Item[]
					{
						new Item(4, -1),
						new Item(4, -1),
						new Item(4, -1),
						new Item(4, -1),
						new Item(0, 0),
						new Item(331, -1),
						new Item(4, -1),
						new Item(4, -1),
						new Item(4, -1),
					}),
				new ShapedRecipe(1, 1, new Item(388, 0, 9),
					new Item[]
					{
						new Item(133, -1),
					}),
				new ShapedRecipe(3, 3, new Item(133, 0, 1),
					new Item[]
					{
						new Item(388, -1),
						new Item(388, -1),
						new Item(388, -1),
						new Item(388, -1),
						new Item(388, -1),
						new Item(388, -1),
						new Item(388, -1),
						new Item(388, -1),
						new Item(388, -1),
					}),
				new ShapelessRecipe(new Item(395, 2, 1),
					new List<Item>
					{
						new Item(395, 0, 1),
						new Item(345, -1, 1),
					}),
				new ShapedRecipe(3, 3, new Item(116, 0, 1),
					new Item[]
					{
						new Item(0, 0),
						new Item(264, -1),
						new Item(49, -1),
						new Item(340, -1),
						new Item(49, -1),
						new Item(49, -1),
						new Item(0, 0),
						new Item(264, -1),
						new Item(49, -1),
					}),
				new ShapedRecipe(3, 3, new Item(130, 0, 1),
					new Item[]
					{
						new Item(49, -1),
						new Item(49, -1),
						new Item(49, -1),
						new Item(49, -1),
						new Item(381, -1),
						new Item(49, -1),
						new Item(49, -1),
						new Item(49, -1),
						new Item(49, -1),
					}),
				new ShapelessRecipe(new Item(381, 0, 1),
					new List<Item>
					{
						new Item(368, -1, 1),
						new Item(377, -1, 1),
					}),
				new ShapedRecipe(2, 2, new Item(206, 0, 4),
					new Item[]
					{
						new Item(121, -1),
						new Item(121, -1),
						new Item(121, -1),
						new Item(121, -1),
					}),
				new ShapedRecipe(3, 3, new Item(-178, 0, 4),
					new Item[]
					{
						new Item(206, -1),
						new Item(206, -1),
						new Item(206, -1),
						new Item(0, 0),
						new Item(206, -1),
						new Item(206, -1),
						new Item(0, 0),
						new Item(0, 0),
						new Item(206, -1),
					}),
				new ShapedRecipe(3, 2, new Item(139, 10, 6),
					new Item[]
					{
						new Item(206, -1),
						new Item(206, -1),
						new Item(206, -1),
						new Item(206, -1),
						new Item(206, -1),
						new Item(206, -1),
					}),
				new ShapedRecipe(3, 3, new Item(426, 0, 1),
					new Item[]
					{
						new Item(20, -1),
						new Item(20, -1),
						new Item(20, -1),
						new Item(20, -1),
						new Item(381, -1),
						new Item(370, -1),
						new Item(20, -1),
						new Item(20, -1),
						new Item(20, -1),
					}),
				new ShapedRecipe(1, 2, new Item(208, 0, 4),
					new Item[]
					{
						new Item(369, -1),
						new Item(433, -1),
					}),
				new ShapedRecipe(3, 2, new Item(85, 0, 3),
					new Item[]
					{
						new Item(5, 0),
						new Item(5, 0),
						new Item(280, -1),
						new Item(280, -1),
						new Item(5, 0),
						new Item(5, 0),
					}),
				new ShapedRecipe(3, 2, new Item(107, 0, 1),
					new Item[]
					{
						new Item(280, -1),
						new Item(280, -1),
						new Item(5, 0),
						new Item(5, 0),
						new Item(280, -1),
						new Item(280, -1),
					}),
				new ShapelessRecipe(new Item(376, 0, 1),
					new List<Item>
					{
						new Item(375, -1, 1),
						new Item(39, -1, 1),
						new Item(353, -1, 1),
					}),
				new ShapedRecipe(3, 3, new Item(346, 0, 1),
					new Item[]
					{
						new Item(0, 0),
						new Item(0, 0),
						new Item(280, -1),
						new Item(0, 0),
						new Item(280, -1),
						new Item(0, 0),
						new Item(280, -1),
						new Item(287, -1),
						new Item(287, -1),
					}),
				new ShapelessRecipe(new Item(-201, 0, 1),
					new List<Item>
					{
						new Item(58, -1, 1),
						new Item(262, 0, 1),
					}),
				new ShapelessRecipe(new Item(259, 0, 1),
					new List<Item>
					{
						new Item(265, -1, 1),
						new Item(318, -1, 1),
					}),
				new ShapedRecipe(3, 2, new Item(390, 0, 1),
					new Item[]
					{
						new Item(336, -1),
						new Item(336, -1),
						new Item(336, -1),
						new Item(0, 0),
						new Item(0, 0),
						new Item(0, 0),
					}),
				new ShapedRecipe(3, 3, new Item(61, 0, 1),
					new Item[]
					{
						new Item(4, -1),
						new Item(4, -1),
						new Item(4, -1),
						new Item(4, -1),
						new Item(0, 0),
						new Item(4, -1),
						new Item(4, -1),
						new Item(4, -1),
						new Item(4, -1),
					}),
				new ShapedRecipe(3, 2, new Item(374, 0, 3),
					new Item[]
					{
						new Item(20, -1),
						new Item(20, -1),
						new Item(20, -1),
						new Item(0, 0),
						new Item(0, 0),
						new Item(0, 0),
					}),
				new ShapedRecipe(3, 2, new Item(102, 0, 16),
					new Item[]
					{
						new Item(20, -1),
						new Item(20, -1),
						new Item(20, -1),
						new Item(20, -1),
						new Item(20, -1),
						new Item(20, -1),
					}),
				new ShapedRecipe(2, 2, new Item(89, 0, 1),
					new Item[]
					{
						new Item(348, -1),
						new Item(348, -1),
						new Item(348, -1),
						new Item(348, -1),
					}),
				new ShapedRecipe(3, 3, new Item(322, 0, 1),
					new Item[]
					{
						new Item(266, -1),
						new Item(266, -1),
						new Item(266, -1),
						new Item(266, -1),
						new Item(260, -1),
						new Item(266, -1),
						new Item(266, -1),
						new Item(266, -1),
						new Item(266, -1),
					}),
				new ShapedRecipe(2, 3, new Item(286, 0, 1),
					new Item[]
					{
						new Item(266, -1),
						new Item(280, -1),
						new Item(266, -1),
						new Item(0, 0),
						new Item(266, -1),
						new Item(280, -1),
					}),
				new ShapedRecipe(3, 2, new Item(317, 0, 1),
					new Item[]
					{
						new Item(266, -1),
						new Item(266, -1),
						new Item(0, 0),
						new Item(0, 0),
						new Item(266, -1),
						new Item(266, -1),
					}),
				new ShapedRecipe(3, 3, new Item(396, 0, 1),
					new Item[]
					{
						new Item(371, -1),
						new Item(371, -1),
						new Item(371, -1),
						new Item(371, -1),
						new Item(391, -1),
						new Item(371, -1),
						new Item(371, -1),
						new Item(371, -1),
						new Item(371, -1),
					}),
				new ShapedRecipe(3, 3, new Item(315, 0, 1),
					new Item[]
					{
						new Item(266, -1),
						new Item(266, -1),
						new Item(266, -1),
						new Item(0, 0),
						new Item(266, -1),
						new Item(266, -1),
						new Item(266, -1),
						new Item(266, -1),
						new Item(266, -1),
					}),
				new ShapedRecipe(3, 2, new Item(314, 0, 1),
					new Item[]
					{
						new Item(266, -1),
						new Item(266, -1),
						new Item(0, 0),
						new Item(266, -1),
						new Item(266, -1),
						new Item(266, -1),
					}),
				new ShapedRecipe(2, 3, new Item(294, 0, 1),
					new Item[]
					{
						new Item(266, -1),
						new Item(280, -1),
						new Item(266, -1),
						new Item(0, 0),
						new Item(0, 0),
						new Item(280, -1),
					}),
				new ShapedRecipe(3, 3, new Item(316, 0, 1),
					new Item[]
					{
						new Item(266, -1),
						new Item(266, -1),
						new Item(266, -1),
						new Item(266, -1),
						new Item(0, 0),
						new Item(0, 0),
						new Item(266, -1),
						new Item(266, -1),
						new Item(266, -1),
					}),
				new ShapedRecipe(3, 3, new Item(285, 0, 1),
					new Item[]
					{
						new Item(266, -1),
						new Item(0, 0),
						new Item(0, 0),
						new Item(266, -1),
						new Item(280, -1),
						new Item(280, -1),
						new Item(266, -1),
						new Item(0, 0),
						new Item(0, 0),
					}),
				new ShapedRecipe(3, 3, new Item(27, 0, 6),
					new Item[]
					{
						new Item(266, -1),
						new Item(266, -1),
						new Item(266, -1),
						new Item(0, 0),
						new Item(280, -1),
						new Item(331, -1),
						new Item(266, -1),
						new Item(266, -1),
						new Item(266, -1),
					}),
				new ShapedRecipe(1, 3, new Item(284, 0, 1),
					new Item[]
					{
						new Item(266, -1),
						new Item(280, -1),
						new Item(280, -1),
					}),
				new ShapedRecipe(1, 3, new Item(283, 0, 1),
					new Item[]
					{
						new Item(266, -1),
						new Item(266, -1),
						new Item(280, -1),
					}),
				new ShapedRecipe(3, 3, new Item(41, 0, 1),
					new Item[]
					{
						new Item(266, -1),
						new Item(266, -1),
						new Item(266, -1),
						new Item(266, -1),
						new Item(266, -1),
						new Item(266, -1),
						new Item(266, -1),
						new Item(266, -1),
						new Item(266, -1),
					}),
				new ShapedRecipe(1, 1, new Item(266, 0, 9),
					new Item[]
					{
						new Item(41, -1),
					}),
				new ShapedRecipe(3, 3, new Item(266, 0, 1),
					new Item[]
					{
						new Item(371, -1),
						new Item(371, -1),
						new Item(371, -1),
						new Item(371, -1),
						new Item(371, -1),
						new Item(371, -1),
						new Item(371, -1),
						new Item(371, -1),
						new Item(371, -1),
					}),
				new ShapedRecipe(1, 1, new Item(371, 0, 9),
					new Item[]
					{
						new Item(266, -1),
					}),
				new ShapelessRecipe(new Item(1, 1, 1),
					new List<Item>
					{
						new Item(1, 3, 1),
						new Item(406, -1, 1),
					}),
				new ShapedRecipe(3, 3, new Item(-169, 0, 4),
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
					}),
				new ShapedRecipe(3, 2, new Item(139, 2, 6),
					new Item[]
					{
						new Item(1, 1),
						new Item(1, 1),
						new Item(1, 1),
						new Item(1, 1),
						new Item(1, 1),
						new Item(1, 1),
					}),
				new ShapedRecipe(3, 3, new Item(446, 8, 1),
					new Item[]
					{
						new Item(35, 7),
						new Item(35, 7),
						new Item(0, 0),
						new Item(35, 7),
						new Item(35, 7),
						new Item(280, -1),
						new Item(35, 7),
						new Item(35, 7),
						new Item(0, 0),
					}),
				new ShapedRecipe(2, 1, new Item(171, 7, 3),
					new Item[]
					{
						new Item(35, 7),
						new Item(35, 7),
					}),
				new ShapedRecipe(3, 3, new Item(171, 7, 8),
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
					}),
				new ShapelessRecipe(new Item(237, 7, 8),
					new List<Item>
					{
						new Item(351, 8, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(13, -1, 1),
						new Item(13, -1, 1),
						new Item(13, -1, 1),
						new Item(13, -1, 1),
					}),
				new ShapelessRecipe(new Item(351, 8, 2),
					new List<Item>
					{
						new Item(351, 16, 1),
						new Item(351, 19, 1),
					}),
				new ShapedRecipe(3, 2, new Item(160, 4, 16),
					new Item[]
					{
						new Item(241, 4),
						new Item(241, 4),
						new Item(241, 4),
						new Item(241, 4),
						new Item(241, 4),
						new Item(241, 4),
					}),
				new ShapedRecipe(3, 3, new Item(160, 4, 8),
					new Item[]
					{
						new Item(102, -1),
						new Item(102, -1),
						new Item(102, -1),
						new Item(102, -1),
						new Item(351, 11),
						new Item(102, -1),
						new Item(102, -1),
						new Item(102, -1),
						new Item(102, -1),
					}),
				new ShapedRecipe(3, 3, new Item(159, 4, 8),
					new Item[]
					{
						new Item(172, -1),
						new Item(172, -1),
						new Item(172, -1),
						new Item(172, -1),
						new Item(351, 11),
						new Item(172, -1),
						new Item(172, -1),
						new Item(172, -1),
						new Item(172, -1),
					}),
				new ShapedRecipe(3, 3, new Item(241, 7, 8),
					new Item[]
					{
						new Item(20, -1),
						new Item(20, -1),
						new Item(20, -1),
						new Item(20, -1),
						new Item(351, 8),
						new Item(20, -1),
						new Item(20, -1),
						new Item(20, -1),
						new Item(20, -1),
					}),
				new ShapedRecipe(3, 2, new Item(160, 7, 16),
					new Item[]
					{
						new Item(241, 7),
						new Item(241, 7),
						new Item(241, 7),
						new Item(241, 7),
						new Item(241, 7),
						new Item(241, 7),
					}),
				new ShapedRecipe(3, 3, new Item(160, 7, 8),
					new Item[]
					{
						new Item(102, -1),
						new Item(102, -1),
						new Item(102, -1),
						new Item(102, -1),
						new Item(351, 8),
						new Item(102, -1),
						new Item(102, -1),
						new Item(102, -1),
						new Item(102, -1),
					}),
				new ShapedRecipe(3, 3, new Item(159, 7, 8),
					new Item[]
					{
						new Item(172, -1),
						new Item(172, -1),
						new Item(172, -1),
						new Item(172, -1),
						new Item(351, 8),
						new Item(172, -1),
						new Item(172, -1),
						new Item(172, -1),
						new Item(172, -1),
					}),
				new ShapedRecipe(3, 3, new Item(446, 2, 1),
					new Item[]
					{
						new Item(35, 13),
						new Item(35, 13),
						new Item(0, 0),
						new Item(35, 13),
						new Item(35, 13),
						new Item(280, -1),
						new Item(35, 13),
						new Item(35, 13),
						new Item(0, 0),
					}),
				new ShapedRecipe(2, 1, new Item(171, 13, 3),
					new Item[]
					{
						new Item(35, 13),
						new Item(35, 13),
					}),
				new ShapedRecipe(3, 3, new Item(171, 13, 8),
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
					}),
				new ShapelessRecipe(new Item(237, 13, 8),
					new List<Item>
					{
						new Item(351, 2, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(13, -1, 1),
						new Item(13, -1, 1),
						new Item(13, -1, 1),
						new Item(13, -1, 1),
					}),
				new ShapedRecipe(3, 3, new Item(241, 13, 8),
					new Item[]
					{
						new Item(20, -1),
						new Item(20, -1),
						new Item(20, -1),
						new Item(20, -1),
						new Item(351, 2),
						new Item(20, -1),
						new Item(20, -1),
						new Item(20, -1),
						new Item(20, -1),
					}),
				new ShapedRecipe(3, 2, new Item(160, 13, 16),
					new Item[]
					{
						new Item(241, 13),
						new Item(241, 13),
						new Item(241, 13),
						new Item(241, 13),
						new Item(241, 13),
						new Item(241, 13),
					}),
				new ShapedRecipe(3, 3, new Item(160, 13, 8),
					new Item[]
					{
						new Item(102, -1),
						new Item(102, -1),
						new Item(102, -1),
						new Item(102, -1),
						new Item(351, 2),
						new Item(102, -1),
						new Item(102, -1),
						new Item(102, -1),
						new Item(102, -1),
					}),
				new ShapedRecipe(3, 3, new Item(159, 13, 8),
					new Item[]
					{
						new Item(172, -1),
						new Item(172, -1),
						new Item(172, -1),
						new Item(172, -1),
						new Item(351, 2),
						new Item(172, -1),
						new Item(172, -1),
						new Item(172, -1),
						new Item(172, -1),
					}),
				new ShapedRecipe(3, 2, new Item(-195, 0, 1),
					new Item[]
					{
						new Item(280, -1),
						new Item(280, -1),
						new Item(0, 0),
						new Item(-166, 2),
						new Item(5, 0),
						new Item(5, 0),
					}),
				new ShapedRecipe(3, 3, new Item(170, 0, 1),
					new Item[]
					{
						new Item(296, -1),
						new Item(296, -1),
						new Item(296, -1),
						new Item(296, -1),
						new Item(296, -1),
						new Item(296, -1),
						new Item(296, -1),
						new Item(296, -1),
						new Item(296, -1),
					}),
				new ShapedRecipe(2, 1, new Item(148, 0, 1),
					new Item[]
					{
						new Item(265, -1),
						new Item(265, -1),
					}),
				new ShapedRecipe(3, 3, new Item(410, 0, 1),
					new Item[]
					{
						new Item(265, -1),
						new Item(265, -1),
						new Item(0, 0),
						new Item(0, 0),
						new Item(54, -1),
						new Item(265, -1),
						new Item(265, -1),
						new Item(265, -1),
						new Item(0, 0),
					}),
				new ShapedRecipe(1, 2, new Item(408, 0, 1),
					new Item[]
					{
						new Item(410, -1),
						new Item(328, -1),
					}),
				new ShapedRecipe(2, 3, new Item(258, 0, 1),
					new Item[]
					{
						new Item(265, -1),
						new Item(280, -1),
						new Item(265, -1),
						new Item(0, 0),
						new Item(265, -1),
						new Item(280, -1),
					}),
				new ShapedRecipe(3, 2, new Item(101, 0, 16),
					new Item[]
					{
						new Item(265, -1),
						new Item(265, -1),
						new Item(265, -1),
						new Item(265, -1),
						new Item(265, -1),
						new Item(265, -1),
					}),
				new ShapedRecipe(3, 3, new Item(42, 0, 1),
					new Item[]
					{
						new Item(265, -1),
						new Item(265, -1),
						new Item(265, -1),
						new Item(265, -1),
						new Item(265, -1),
						new Item(265, -1),
						new Item(265, -1),
						new Item(265, -1),
						new Item(265, -1),
					}),
				new ShapedRecipe(3, 2, new Item(309, 0, 1),
					new Item[]
					{
						new Item(265, -1),
						new Item(265, -1),
						new Item(0, 0),
						new Item(0, 0),
						new Item(265, -1),
						new Item(265, -1),
					}),
				new ShapedRecipe(3, 3, new Item(307, 0, 1),
					new Item[]
					{
						new Item(265, -1),
						new Item(265, -1),
						new Item(265, -1),
						new Item(0, 0),
						new Item(265, -1),
						new Item(265, -1),
						new Item(265, -1),
						new Item(265, -1),
						new Item(265, -1),
					}),
				new ShapedRecipe(2, 3, new Item(330, 0, 3),
					new Item[]
					{
						new Item(265, -1),
						new Item(265, -1),
						new Item(265, -1),
						new Item(265, -1),
						new Item(265, -1),
						new Item(265, -1),
					}),
				new ShapedRecipe(3, 2, new Item(306, 0, 1),
					new Item[]
					{
						new Item(265, -1),
						new Item(265, -1),
						new Item(0, 0),
						new Item(265, -1),
						new Item(265, -1),
						new Item(265, -1),
					}),
				new ShapedRecipe(2, 3, new Item(292, 0, 1),
					new Item[]
					{
						new Item(265, -1),
						new Item(280, -1),
						new Item(265, -1),
						new Item(0, 0),
						new Item(0, 0),
						new Item(280, -1),
					}),
				new ShapedRecipe(1, 1, new Item(265, 0, 9),
					new Item[]
					{
						new Item(42, -1),
					}),
				new ShapedRecipe(3, 3, new Item(265, 0, 1),
					new Item[]
					{
						new Item(452, -1),
						new Item(452, -1),
						new Item(452, -1),
						new Item(452, -1),
						new Item(452, -1),
						new Item(452, -1),
						new Item(452, -1),
						new Item(452, -1),
						new Item(452, -1),
					}),
				new ShapedRecipe(3, 3, new Item(308, 0, 1),
					new Item[]
					{
						new Item(265, -1),
						new Item(265, -1),
						new Item(265, -1),
						new Item(265, -1),
						new Item(0, 0),
						new Item(0, 0),
						new Item(265, -1),
						new Item(265, -1),
						new Item(265, -1),
					}),
				new ShapedRecipe(1, 1, new Item(452, 0, 9),
					new Item[]
					{
						new Item(265, -1),
					}),
				new ShapedRecipe(3, 3, new Item(257, 0, 1),
					new Item[]
					{
						new Item(265, -1),
						new Item(0, 0),
						new Item(0, 0),
						new Item(265, -1),
						new Item(280, -1),
						new Item(280, -1),
						new Item(265, -1),
						new Item(0, 0),
						new Item(0, 0),
					}),
				new ShapedRecipe(1, 3, new Item(256, 0, 1),
					new Item[]
					{
						new Item(265, -1),
						new Item(280, -1),
						new Item(280, -1),
					}),
				new ShapedRecipe(1, 3, new Item(267, 0, 1),
					new Item[]
					{
						new Item(265, -1),
						new Item(265, -1),
						new Item(280, -1),
					}),
				new ShapedRecipe(2, 2, new Item(167, 0, 1),
					new Item[]
					{
						new Item(265, -1),
						new Item(265, -1),
						new Item(265, -1),
						new Item(265, -1),
					}),
				new ShapedRecipe(3, 3, new Item(389, 0, 1),
					new Item[]
					{
						new Item(280, -1),
						new Item(280, -1),
						new Item(280, -1),
						new Item(280, -1),
						new Item(334, -1),
						new Item(280, -1),
						new Item(280, -1),
						new Item(280, -1),
						new Item(280, -1),
					}),
				new ShapedRecipe(3, 2, new Item(333, 3, 1),
					new Item[]
					{
						new Item(5, 3),
						new Item(5, 3),
						new Item(5, 3),
						new Item(269, -1),
						new Item(5, 3),
						new Item(5, 3),
					}),
				new ShapedRecipe(2, 3, new Item(429, 0, 3),
					new Item[]
					{
						new Item(5, 3),
						new Item(5, 3),
						new Item(5, 3),
						new Item(5, 3),
						new Item(5, 3),
						new Item(5, 3),
					}),
				new ShapedRecipe(3, 2, new Item(85, 3, 3),
					new Item[]
					{
						new Item(5, 3),
						new Item(5, 3),
						new Item(280, -1),
						new Item(280, -1),
						new Item(5, 3),
						new Item(5, 3),
					}),
				new ShapedRecipe(3, 2, new Item(185, 0, 1),
					new Item[]
					{
						new Item(280, -1),
						new Item(280, -1),
						new Item(5, 3),
						new Item(5, 3),
						new Item(280, -1),
						new Item(280, -1),
					}),
				new ShapedRecipe(1, 1, new Item(5, 3, 4),
					new Item[]
					{
						new Item(17, 3),
					}),
				new ShapedRecipe(1, 1, new Item(5, 3, 4),
					new Item[]
					{
						new Item(-7, -1),
					}),
				new ShapedRecipe(1, 1, new Item(5, 3, 4),
					new Item[]
					{
						new Item(-212, 11),
					}),
				new ShapedRecipe(1, 1, new Item(5, 3, 4),
					new Item[]
					{
						new Item(-212, 3),
					}),
				new ShapedRecipe(3, 3, new Item(136, 0, 4),
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
					}),
				new ShapedRecipe(2, 2, new Item(-212, 3, 3),
					new Item[]
					{
						new Item(17, 3),
						new Item(17, 3),
						new Item(17, 3),
						new Item(17, 3),
					}),
				new ShapedRecipe(3, 1, new Item(158, 3, 6),
					new Item[]
					{
						new Item(5, 3),
						new Item(5, 3),
						new Item(5, 3),
					}),
				new ShapedRecipe(2, 2, new Item(-212, 11, 3),
					new Item[]
					{
						new Item(-7, -1),
						new Item(-7, -1),
						new Item(-7, -1),
						new Item(-7, -1),
					}),
				new ShapedRecipe(3, 3, new Item(65, 0, 3),
					new Item[]
					{
						new Item(280, -1),
						new Item(280, -1),
						new Item(280, -1),
						new Item(0, 0),
						new Item(280, -1),
						new Item(0, 0),
						new Item(280, -1),
						new Item(280, -1),
						new Item(280, -1),
					}),
				new ShapedRecipe(3, 3, new Item(-208, 0, 1),
					new Item[]
					{
						new Item(452, -1),
						new Item(452, -1),
						new Item(452, -1),
						new Item(452, -1),
						new Item(50, -1),
						new Item(452, -1),
						new Item(452, -1),
						new Item(452, -1),
						new Item(452, -1),
					}),
				new ShapedRecipe(3, 3, new Item(22, 0, 1),
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
					}),
				new ShapedRecipe(1, 1, new Item(351, 4, 9),
					new Item[]
					{
						new Item(22, -1),
					}),
				new ShapedRecipe(3, 3, new Item(420, 0, 2),
					new Item[]
					{
						new Item(287, -1),
						new Item(287, -1),
						new Item(0, 0),
						new Item(287, -1),
						new Item(341, -1),
						new Item(0, 0),
						new Item(0, 0),
						new Item(0, 0),
						new Item(287, -1),
					}),
				new ShapedRecipe(2, 2, new Item(334, 0, 1),
					new Item[]
					{
						new Item(415, -1),
						new Item(415, -1),
						new Item(415, -1),
						new Item(415, -1),
					}),
				new ShapedRecipe(3, 2, new Item(301, 0, 1),
					new Item[]
					{
						new Item(334, -1),
						new Item(334, -1),
						new Item(0, 0),
						new Item(0, 0),
						new Item(334, -1),
						new Item(334, -1),
					}),
				new ShapedRecipe(3, 3, new Item(299, 0, 1),
					new Item[]
					{
						new Item(334, -1),
						new Item(334, -1),
						new Item(334, -1),
						new Item(0, 0),
						new Item(334, -1),
						new Item(334, -1),
						new Item(334, -1),
						new Item(334, -1),
						new Item(334, -1),
					}),
				new ShapedRecipe(3, 2, new Item(298, 0, 1),
					new Item[]
					{
						new Item(334, -1),
						new Item(334, -1),
						new Item(0, 0),
						new Item(334, -1),
						new Item(334, -1),
						new Item(334, -1),
					}),
				new ShapedRecipe(3, 3, new Item(416, 0, 1),
					new Item[]
					{
						new Item(334, -1),
						new Item(334, -1),
						new Item(334, -1),
						new Item(0, 0),
						new Item(334, -1),
						new Item(0, 0),
						new Item(334, -1),
						new Item(334, -1),
						new Item(334, -1),
					}),
				new ShapedRecipe(3, 3, new Item(300, 0, 1),
					new Item[]
					{
						new Item(334, -1),
						new Item(334, -1),
						new Item(334, -1),
						new Item(334, -1),
						new Item(0, 0),
						new Item(0, 0),
						new Item(334, -1),
						new Item(334, -1),
						new Item(334, -1),
					}),
				new ShapedRecipe(3, 3, new Item(-194, 0, 1),
					new Item[]
					{
						new Item(158, -1),
						new Item(0, 0),
						new Item(0, 0),
						new Item(158, -1),
						new Item(47, -1),
						new Item(158, -1),
						new Item(158, -1),
						new Item(0, 0),
						new Item(0, 0),
					}),
				new ShapedRecipe(1, 2, new Item(69, 0, 1),
					new Item[]
					{
						new Item(280, -1),
						new Item(4, -1),
					}),
				new ShapedRecipe(3, 3, new Item(446, 12, 1),
					new Item[]
					{
						new Item(35, 3),
						new Item(35, 3),
						new Item(0, 0),
						new Item(35, 3),
						new Item(35, 3),
						new Item(280, -1),
						new Item(35, 3),
						new Item(35, 3),
						new Item(0, 0),
					}),
				new ShapedRecipe(2, 1, new Item(171, 3, 3),
					new Item[]
					{
						new Item(35, 3),
						new Item(35, 3),
					}),
				new ShapedRecipe(3, 3, new Item(171, 3, 8),
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
					}),
				new ShapelessRecipe(new Item(237, 3, 8),
					new List<Item>
					{
						new Item(351, 12, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(13, -1, 1),
						new Item(13, -1, 1),
						new Item(13, -1, 1),
						new Item(13, -1, 1),
					}),
				new ShapelessRecipe(new Item(351, 12, 1),
					new List<Item>
					{
						new Item(38, 1, 1),
					}),
				new ShapedRecipe(3, 3, new Item(241, 3, 8),
					new Item[]
					{
						new Item(20, -1),
						new Item(20, -1),
						new Item(20, -1),
						new Item(20, -1),
						new Item(351, 12),
						new Item(20, -1),
						new Item(20, -1),
						new Item(20, -1),
						new Item(20, -1),
					}),
				new ShapedRecipe(3, 2, new Item(160, 3, 16),
					new Item[]
					{
						new Item(241, 3),
						new Item(241, 3),
						new Item(241, 3),
						new Item(241, 3),
						new Item(241, 3),
						new Item(241, 3),
					}),
				new ShapedRecipe(3, 3, new Item(160, 3, 8),
					new Item[]
					{
						new Item(102, -1),
						new Item(102, -1),
						new Item(102, -1),
						new Item(102, -1),
						new Item(351, 12),
						new Item(102, -1),
						new Item(102, -1),
						new Item(102, -1),
						new Item(102, -1),
					}),
				new ShapedRecipe(3, 3, new Item(159, 3, 8),
					new Item[]
					{
						new Item(172, -1),
						new Item(172, -1),
						new Item(172, -1),
						new Item(172, -1),
						new Item(351, 12),
						new Item(172, -1),
						new Item(172, -1),
						new Item(172, -1),
						new Item(172, -1),
					}),
				new ShapedRecipe(3, 3, new Item(446, 7, 1),
					new Item[]
					{
						new Item(35, 8),
						new Item(35, 8),
						new Item(0, 0),
						new Item(35, 8),
						new Item(35, 8),
						new Item(280, -1),
						new Item(35, 8),
						new Item(35, 8),
						new Item(0, 0),
					}),
				new ShapedRecipe(2, 1, new Item(171, 8, 3),
					new Item[]
					{
						new Item(35, 8),
						new Item(35, 8),
					}),
				new ShapelessRecipe(new Item(237, 8, 8),
					new List<Item>
					{
						new Item(351, 7, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(13, -1, 1),
						new Item(13, -1, 1),
						new Item(13, -1, 1),
						new Item(13, -1, 1),
					}),
				new ShapelessRecipe(new Item(351, 7, 1),
					new List<Item>
					{
						new Item(38, 6, 1),
					}),
				new ShapedRecipe(3, 3, new Item(241, 8, 8),
					new Item[]
					{
						new Item(20, -1),
						new Item(20, -1),
						new Item(20, -1),
						new Item(20, -1),
						new Item(351, 7),
						new Item(20, -1),
						new Item(20, -1),
						new Item(20, -1),
						new Item(20, -1),
					}),
				new ShapedRecipe(3, 2, new Item(160, 8, 16),
					new Item[]
					{
						new Item(241, 8),
						new Item(241, 8),
						new Item(241, 8),
						new Item(241, 8),
						new Item(241, 8),
						new Item(241, 8),
					}),
				new ShapedRecipe(3, 3, new Item(160, 8, 8),
					new Item[]
					{
						new Item(102, -1),
						new Item(102, -1),
						new Item(102, -1),
						new Item(102, -1),
						new Item(351, 7),
						new Item(102, -1),
						new Item(102, -1),
						new Item(102, -1),
						new Item(102, -1),
					}),
				new ShapedRecipe(3, 3, new Item(159, 8, 8),
					new Item[]
					{
						new Item(172, -1),
						new Item(172, -1),
						new Item(172, -1),
						new Item(172, -1),
						new Item(351, 7),
						new Item(172, -1),
						new Item(172, -1),
						new Item(172, -1),
						new Item(172, -1),
					}),
				new ShapedRecipe(3, 3, new Item(171, 8, 8),
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
					}),
				new ShapedRecipe(2, 1, new Item(147, 0, 1),
					new Item[]
					{
						new Item(266, -1),
						new Item(266, -1),
					}),
				new ShapedRecipe(3, 3, new Item(446, 10, 1),
					new Item[]
					{
						new Item(35, 5),
						new Item(35, 5),
						new Item(0, 0),
						new Item(35, 5),
						new Item(35, 5),
						new Item(280, -1),
						new Item(35, 5),
						new Item(35, 5),
						new Item(0, 0),
					}),
				new ShapedRecipe(2, 1, new Item(171, 5, 3),
					new Item[]
					{
						new Item(35, 5),
						new Item(35, 5),
					}),
				new ShapelessRecipe(new Item(237, 5, 8),
					new List<Item>
					{
						new Item(351, 10, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(13, -1, 1),
						new Item(13, -1, 1),
						new Item(13, -1, 1),
						new Item(13, -1, 1),
					}),
				new ShapelessRecipe(new Item(351, 10, 2),
					new List<Item>
					{
						new Item(351, 2, 1),
						new Item(351, 19, 1),
					}),
				new ShapedRecipe(3, 3, new Item(241, 5, 8),
					new Item[]
					{
						new Item(20, -1),
						new Item(20, -1),
						new Item(20, -1),
						new Item(20, -1),
						new Item(351, 10),
						new Item(20, -1),
						new Item(20, -1),
						new Item(20, -1),
						new Item(20, -1),
					}),
				new ShapedRecipe(3, 2, new Item(160, 5, 16),
					new Item[]
					{
						new Item(241, 5),
						new Item(241, 5),
						new Item(241, 5),
						new Item(241, 5),
						new Item(241, 5),
						new Item(241, 5),
					}),
				new ShapedRecipe(3, 3, new Item(160, 5, 8),
					new Item[]
					{
						new Item(102, -1),
						new Item(102, -1),
						new Item(102, -1),
						new Item(102, -1),
						new Item(351, 10),
						new Item(102, -1),
						new Item(102, -1),
						new Item(102, -1),
						new Item(102, -1),
					}),
				new ShapedRecipe(3, 3, new Item(159, 5, 8),
					new Item[]
					{
						new Item(172, -1),
						new Item(172, -1),
						new Item(172, -1),
						new Item(172, -1),
						new Item(351, 10),
						new Item(172, -1),
						new Item(172, -1),
						new Item(172, -1),
						new Item(172, -1),
					}),
				new ShapedRecipe(3, 3, new Item(171, 5, 8),
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
					}),
				new ShapedRecipe(1, 2, new Item(91, 0, 1),
					new Item[]
					{
						new Item(-155, -1),
						new Item(50, -1),
					}),
				new ShapedRecipe(3, 3, new Item(395, 2, 1),
					new Item[]
					{
						new Item(339, -1),
						new Item(339, -1),
						new Item(339, -1),
						new Item(339, -1),
						new Item(345, -1),
						new Item(339, -1),
						new Item(339, -1),
						new Item(339, -1),
						new Item(339, -1),
					}),
				new ShapedRecipe(3, 3, new Item(446, 13, 1),
					new Item[]
					{
						new Item(35, 2),
						new Item(35, 2),
						new Item(0, 0),
						new Item(35, 2),
						new Item(35, 2),
						new Item(280, -1),
						new Item(35, 2),
						new Item(35, 2),
						new Item(0, 0),
					}),
				new ShapedRecipe(2, 1, new Item(171, 2, 3),
					new Item[]
					{
						new Item(35, 2),
						new Item(35, 2),
					}),
				new ShapedRecipe(3, 3, new Item(171, 2, 8),
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
					}),
				new ShapelessRecipe(new Item(237, 2, 8),
					new List<Item>
					{
						new Item(351, 13, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(13, -1, 1),
						new Item(13, -1, 1),
						new Item(13, -1, 1),
						new Item(13, -1, 1),
					}),
				new ShapelessRecipe(new Item(351, 13, 2),
					new List<Item>
					{
						new Item(175, 1, 1),
					}),
				new ShapedRecipe(3, 3, new Item(241, 2, 8),
					new Item[]
					{
						new Item(20, -1),
						new Item(20, -1),
						new Item(20, -1),
						new Item(20, -1),
						new Item(351, 13),
						new Item(20, -1),
						new Item(20, -1),
						new Item(20, -1),
						new Item(20, -1),
					}),
				new ShapedRecipe(3, 2, new Item(160, 2, 16),
					new Item[]
					{
						new Item(241, 2),
						new Item(241, 2),
						new Item(241, 2),
						new Item(241, 2),
						new Item(241, 2),
						new Item(241, 2),
					}),
				new ShapedRecipe(3, 3, new Item(160, 2, 8),
					new Item[]
					{
						new Item(102, -1),
						new Item(102, -1),
						new Item(102, -1),
						new Item(102, -1),
						new Item(351, 13),
						new Item(102, -1),
						new Item(102, -1),
						new Item(102, -1),
						new Item(102, -1),
					}),
				new ShapedRecipe(3, 3, new Item(159, 2, 8),
					new Item[]
					{
						new Item(172, -1),
						new Item(172, -1),
						new Item(172, -1),
						new Item(172, -1),
						new Item(351, 13),
						new Item(172, -1),
						new Item(172, -1),
						new Item(172, -1),
						new Item(172, -1),
					}),
				new ShapedRecipe(2, 2, new Item(213, 0, 1),
					new Item[]
					{
						new Item(378, -1),
						new Item(378, -1),
						new Item(378, -1),
						new Item(378, -1),
					}),
				new ShapelessRecipe(new Item(378, 0, 1),
					new List<Item>
					{
						new Item(377, -1, 1),
						new Item(341, -1, 1),
					}),
				new ShapedRecipe(3, 3, new Item(395, 0, 1),
					new Item[]
					{
						new Item(339, -1),
						new Item(339, -1),
						new Item(339, -1),
						new Item(339, -1),
						new Item(339, -1),
						new Item(339, -1),
						new Item(339, -1),
						new Item(339, -1),
						new Item(339, -1),
					}),
				new ShapedRecipe(3, 3, new Item(103, 0, 1),
					new Item[]
					{
						new Item(360, -1),
						new Item(360, -1),
						new Item(360, -1),
						new Item(360, -1),
						new Item(360, -1),
						new Item(360, -1),
						new Item(360, -1),
						new Item(360, -1),
						new Item(360, -1),
					}),
				new ShapedRecipe(1, 1, new Item(362, 0, 1),
					new Item[]
					{
						new Item(360, -1),
					}),
				new ShapedRecipe(3, 2, new Item(328, 0, 1),
					new Item[]
					{
						new Item(265, -1),
						new Item(265, -1),
						new Item(265, -1),
						new Item(0, 0),
						new Item(265, -1),
						new Item(265, -1),
					}),
				new ShapelessRecipe(new Item(48, 0, 1),
					new List<Item>
					{
						new Item(4, -1, 1),
						new Item(106, -1, 1),
					}),
				new ShapedRecipe(3, 3, new Item(-179, 0, 4),
					new Item[]
					{
						new Item(48, -1),
						new Item(48, -1),
						new Item(48, -1),
						new Item(0, 0),
						new Item(48, -1),
						new Item(48, -1),
						new Item(0, 0),
						new Item(0, 0),
						new Item(48, -1),
					}),
				new ShapedRecipe(3, 2, new Item(139, 1, 6),
					new Item[]
					{
						new Item(48, -1),
						new Item(48, -1),
						new Item(48, -1),
						new Item(48, -1),
						new Item(48, -1),
						new Item(48, -1),
					}),
				new ShapelessRecipe(new Item(98, 1, 1),
					new List<Item>
					{
						new Item(98, 0, 1),
						new Item(106, -1, 1),
					}),
				new ShapedRecipe(3, 3, new Item(-175, 0, 4),
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
					}),
				new ShapedRecipe(3, 2, new Item(139, 8, 6),
					new Item[]
					{
						new Item(98, 1),
						new Item(98, 1),
						new Item(98, 1),
						new Item(98, 1),
						new Item(98, 1),
						new Item(98, 1),
					}),
				new ShapelessRecipe(new Item(282, 0, 1),
					new List<Item>
					{
						new Item(39, -1, 1),
						new Item(40, -1, 1),
						new Item(281, -1, 1),
					}),
				new ShapedRecipe(2, 2, new Item(112, 0, 1),
					new Item[]
					{
						new Item(405, -1),
						new Item(405, -1),
						new Item(405, -1),
						new Item(405, -1),
					}),
				new ShapedRecipe(3, 2, new Item(113, 0, 6),
					new Item[]
					{
						new Item(112, -1),
						new Item(112, -1),
						new Item(405, -1),
						new Item(405, -1),
						new Item(112, -1),
						new Item(112, -1),
					}),
				new ShapedRecipe(3, 3, new Item(114, 0, 4),
					new Item[]
					{
						new Item(112, -1),
						new Item(112, -1),
						new Item(112, -1),
						new Item(0, 0),
						new Item(112, -1),
						new Item(112, -1),
						new Item(0, 0),
						new Item(0, 0),
						new Item(112, -1),
					}),
				new ShapedRecipe(3, 2, new Item(139, 9, 6),
					new Item[]
					{
						new Item(112, -1),
						new Item(112, -1),
						new Item(112, -1),
						new Item(112, -1),
						new Item(112, -1),
						new Item(112, -1),
					}),
				new ShapedRecipe(3, 3, new Item(214, 0, 1),
					new Item[]
					{
						new Item(372, -1),
						new Item(372, -1),
						new Item(372, -1),
						new Item(372, -1),
						new Item(372, -1),
						new Item(372, -1),
						new Item(372, -1),
						new Item(372, -1),
						new Item(372, -1),
					}),
				new ShapedRecipe(1, 1, new Item(5, 0, 4),
					new Item[]
					{
						new Item(17, 0),
					}),
				new ShapedRecipe(1, 1, new Item(5, 0, 4),
					new Item[]
					{
						new Item(-10, -1),
					}),
				new ShapedRecipe(1, 1, new Item(5, 0, 4),
					new Item[]
					{
						new Item(-212, 8),
					}),
				new ShapedRecipe(1, 1, new Item(5, 0, 4),
					new Item[]
					{
						new Item(-212, 0),
					}),
				new ShapedRecipe(3, 3, new Item(53, 0, 4),
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
					}),
				new ShapedRecipe(2, 2, new Item(-212, 0, 3),
					new Item[]
					{
						new Item(17, 0),
						new Item(17, 0),
						new Item(17, 0),
						new Item(17, 0),
					}),
				new ShapedRecipe(3, 1, new Item(158, 0, 6),
					new Item[]
					{
						new Item(5, 0),
						new Item(5, 0),
						new Item(5, 0),
					}),
				new ShapedRecipe(2, 2, new Item(-212, 8, 3),
					new Item[]
					{
						new Item(-10, -1),
						new Item(-10, -1),
						new Item(-10, -1),
						new Item(-10, -1),
					}),
				new ShapedRecipe(3, 3, new Item(251, 0, 1),
					new Item[]
					{
						new Item(4, -1),
						new Item(331, -1),
						new Item(4, -1),
						new Item(4, -1),
						new Item(331, -1),
						new Item(4, -1),
						new Item(4, -1),
						new Item(406, -1),
						new Item(4, -1),
					}),
				new ShapedRecipe(3, 3, new Item(446, 14, 1),
					new Item[]
					{
						new Item(35, 1),
						new Item(35, 1),
						new Item(0, 0),
						new Item(35, 1),
						new Item(35, 1),
						new Item(280, -1),
						new Item(35, 1),
						new Item(35, 1),
						new Item(0, 0),
					}),
				new ShapedRecipe(2, 1, new Item(171, 1, 3),
					new Item[]
					{
						new Item(35, 1),
						new Item(35, 1),
					}),
				new ShapedRecipe(3, 3, new Item(171, 1, 8),
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
					}),
				new ShapelessRecipe(new Item(237, 1, 8),
					new List<Item>
					{
						new Item(351, 14, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(13, -1, 1),
						new Item(13, -1, 1),
						new Item(13, -1, 1),
						new Item(13, -1, 1),
					}),
				new ShapelessRecipe(new Item(351, 14, 1),
					new List<Item>
					{
						new Item(38, 5, 1),
					}),
				new ShapelessRecipe(new Item(351, 14, 2),
					new List<Item>
					{
						new Item(351, 1, 1),
						new Item(351, 11, 1),
					}),
				new ShapedRecipe(3, 3, new Item(241, 1, 8),
					new Item[]
					{
						new Item(20, -1),
						new Item(20, -1),
						new Item(20, -1),
						new Item(20, -1),
						new Item(351, 14),
						new Item(20, -1),
						new Item(20, -1),
						new Item(20, -1),
						new Item(20, -1),
					}),
				new ShapedRecipe(3, 2, new Item(160, 1, 16),
					new Item[]
					{
						new Item(241, 1),
						new Item(241, 1),
						new Item(241, 1),
						new Item(241, 1),
						new Item(241, 1),
						new Item(241, 1),
					}),
				new ShapedRecipe(3, 3, new Item(160, 1, 8),
					new Item[]
					{
						new Item(102, -1),
						new Item(102, -1),
						new Item(102, -1),
						new Item(102, -1),
						new Item(351, 14),
						new Item(102, -1),
						new Item(102, -1),
						new Item(102, -1),
						new Item(102, -1),
					}),
				new ShapedRecipe(3, 3, new Item(159, 1, 8),
					new Item[]
					{
						new Item(172, -1),
						new Item(172, -1),
						new Item(172, -1),
						new Item(172, -1),
						new Item(351, 14),
						new Item(172, -1),
						new Item(172, -1),
						new Item(172, -1),
						new Item(172, -1),
					}),
				new ShapedRecipe(3, 3, new Item(174, 0, 1),
					new Item[]
					{
						new Item(79, -1),
						new Item(79, -1),
						new Item(79, -1),
						new Item(79, -1),
						new Item(79, -1),
						new Item(79, -1),
						new Item(79, -1),
						new Item(79, -1),
						new Item(79, -1),
					}),
				new ShapedRecipe(3, 1, new Item(339, 0, 3),
					new Item[]
					{
						new Item(338, -1),
						new Item(338, -1),
						new Item(338, -1),
					}),
				new ShapedRecipe(1, 2, new Item(155, 2, 2),
					new Item[]
					{
						new Item(155, 0),
						new Item(155, 0),
					}),
				new ShapedRecipe(3, 3, new Item(446, 9, 1),
					new Item[]
					{
						new Item(35, 6),
						new Item(35, 6),
						new Item(0, 0),
						new Item(35, 6),
						new Item(35, 6),
						new Item(280, -1),
						new Item(35, 6),
						new Item(35, 6),
						new Item(0, 0),
					}),
				new ShapedRecipe(2, 1, new Item(171, 6, 3),
					new Item[]
					{
						new Item(35, 6),
						new Item(35, 6),
					}),
				new ShapedRecipe(3, 3, new Item(171, 6, 8),
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
					}),
				new ShapelessRecipe(new Item(237, 6, 8),
					new List<Item>
					{
						new Item(351, 9, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(13, -1, 1),
						new Item(13, -1, 1),
						new Item(13, -1, 1),
						new Item(13, -1, 1),
					}),
				new ShapelessRecipe(new Item(351, 9, 2),
					new List<Item>
					{
						new Item(351, 1, 1),
						new Item(351, 19, 1),
					}),
				new ShapelessRecipe(new Item(351, 9, 2),
					new List<Item>
					{
						new Item(175, 5, 1),
					}),
				new ShapelessRecipe(new Item(351, 9, 1),
					new List<Item>
					{
						new Item(38, 7, 1),
					}),
				new ShapelessRecipe(new Item(351, 9, 2),
					new List<Item>
					{
						new Item(351, 1, 1),
						new Item(351, 15, 1),
					}),
				new ShapedRecipe(3, 3, new Item(241, 6, 8),
					new Item[]
					{
						new Item(20, -1),
						new Item(20, -1),
						new Item(20, -1),
						new Item(20, -1),
						new Item(351, 9),
						new Item(20, -1),
						new Item(20, -1),
						new Item(20, -1),
						new Item(20, -1),
					}),
				new ShapedRecipe(3, 2, new Item(160, 6, 16),
					new Item[]
					{
						new Item(241, 6),
						new Item(241, 6),
						new Item(241, 6),
						new Item(241, 6),
						new Item(241, 6),
						new Item(241, 6),
					}),
				new ShapedRecipe(3, 3, new Item(160, 6, 8),
					new Item[]
					{
						new Item(102, -1),
						new Item(102, -1),
						new Item(102, -1),
						new Item(102, -1),
						new Item(351, 9),
						new Item(102, -1),
						new Item(102, -1),
						new Item(102, -1),
						new Item(102, -1),
					}),
				new ShapedRecipe(3, 3, new Item(159, 6, 8),
					new Item[]
					{
						new Item(172, -1),
						new Item(172, -1),
						new Item(172, -1),
						new Item(172, -1),
						new Item(351, 9),
						new Item(172, -1),
						new Item(172, -1),
						new Item(172, -1),
						new Item(172, -1),
					}),
				new ShapedRecipe(2, 2, new Item(1, 6, 4),
					new Item[]
					{
						new Item(1, 5),
						new Item(1, 5),
						new Item(1, 5),
						new Item(1, 5),
					}),
				new ShapedRecipe(3, 3, new Item(-174, 0, 4),
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
					}),
				new ShapedRecipe(2, 2, new Item(1, 4, 4),
					new Item[]
					{
						new Item(1, 3),
						new Item(1, 3),
						new Item(1, 3),
						new Item(1, 3),
					}),
				new ShapedRecipe(3, 3, new Item(-173, 0, 4),
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
					}),
				new ShapedRecipe(2, 2, new Item(1, 2, 4),
					new Item[]
					{
						new Item(1, 1),
						new Item(1, 1),
						new Item(1, 1),
						new Item(1, 1),
					}),
				new ShapedRecipe(3, 3, new Item(-172, 0, 4),
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
					}),
				new ShapedRecipe(2, 2, new Item(168, 0, 1),
					new Item[]
					{
						new Item(409, -1),
						new Item(409, -1),
						new Item(409, -1),
						new Item(409, -1),
					}),
				new ShapedRecipe(3, 3, new Item(168, 2, 1),
					new Item[]
					{
						new Item(409, -1),
						new Item(409, -1),
						new Item(409, -1),
						new Item(409, -1),
						new Item(409, -1),
						new Item(409, -1),
						new Item(409, -1),
						new Item(409, -1),
						new Item(409, -1),
					}),
				new ShapedRecipe(3, 3, new Item(-2, 0, 4),
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
					}),
				new ShapedRecipe(3, 3, new Item(-4, 0, 4),
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
					}),
				new ShapedRecipe(3, 3, new Item(-3, 0, 4),
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
					}),
				new ShapedRecipe(3, 2, new Item(139, 11, 6),
					new Item[]
					{
						new Item(168, 0),
						new Item(168, 0),
						new Item(168, 0),
						new Item(168, 0),
						new Item(168, 0),
						new Item(168, 0),
					}),
				new ShapelessRecipe(new Item(400, 0, 1),
					new List<Item>
					{
						new Item(86, -1, 1),
						new Item(353, -1, 1),
						new Item(344, -1, 1),
					}),
				new ShapedRecipe(1, 1, new Item(361, 0, 4),
					new Item[]
					{
						new Item(86, -1),
					}),
				new ShapedRecipe(3, 3, new Item(446, 5, 1),
					new Item[]
					{
						new Item(35, 10),
						new Item(35, 10),
						new Item(0, 0),
						new Item(35, 10),
						new Item(35, 10),
						new Item(280, -1),
						new Item(35, 10),
						new Item(35, 10),
						new Item(0, 0),
					}),
				new ShapedRecipe(2, 1, new Item(171, 10, 3),
					new Item[]
					{
						new Item(35, 10),
						new Item(35, 10),
					}),
				new ShapedRecipe(3, 3, new Item(171, 10, 8),
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
					}),
				new ShapelessRecipe(new Item(237, 10, 8),
					new List<Item>
					{
						new Item(351, 5, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(13, -1, 1),
						new Item(13, -1, 1),
						new Item(13, -1, 1),
						new Item(13, -1, 1),
					}),
				new ShapelessRecipe(new Item(351, 5, 2),
					new List<Item>
					{
						new Item(351, 18, 1),
						new Item(351, 1, 1),
					}),
				new ShapedRecipe(3, 3, new Item(241, 10, 8),
					new Item[]
					{
						new Item(20, -1),
						new Item(20, -1),
						new Item(20, -1),
						new Item(20, -1),
						new Item(351, 5),
						new Item(20, -1),
						new Item(20, -1),
						new Item(20, -1),
						new Item(20, -1),
					}),
				new ShapedRecipe(3, 2, new Item(160, 10, 16),
					new Item[]
					{
						new Item(241, 10),
						new Item(241, 10),
						new Item(241, 10),
						new Item(241, 10),
						new Item(241, 10),
						new Item(241, 10),
					}),
				new ShapedRecipe(3, 3, new Item(160, 10, 8),
					new Item[]
					{
						new Item(102, -1),
						new Item(102, -1),
						new Item(102, -1),
						new Item(102, -1),
						new Item(351, 5),
						new Item(102, -1),
						new Item(102, -1),
						new Item(102, -1),
						new Item(102, -1),
					}),
				new ShapedRecipe(3, 3, new Item(159, 10, 8),
					new Item[]
					{
						new Item(172, -1),
						new Item(172, -1),
						new Item(172, -1),
						new Item(172, -1),
						new Item(351, 5),
						new Item(172, -1),
						new Item(172, -1),
						new Item(172, -1),
						new Item(172, -1),
					}),
				new ShapedRecipe(2, 2, new Item(201, 0, 4),
					new Item[]
					{
						new Item(433, -1),
						new Item(433, -1),
						new Item(433, -1),
						new Item(433, -1),
					}),
				new ShapedRecipe(3, 3, new Item(203, 0, 4),
					new Item[]
					{
						new Item(201, -1),
						new Item(201, -1),
						new Item(201, -1),
						new Item(0, 0),
						new Item(201, -1),
						new Item(201, -1),
						new Item(0, 0),
						new Item(0, 0),
						new Item(201, -1),
					}),
				new ShapedRecipe(2, 2, new Item(155, 0, 1),
					new Item[]
					{
						new Item(406, -1),
						new Item(406, -1),
						new Item(406, -1),
						new Item(406, -1),
					}),
				new ShapedRecipe(3, 3, new Item(156, 0, 4),
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
					}),
				new ShapelessRecipe(new Item(413, 0, 1),
					new List<Item>
					{
						new Item(281, -1, 1),
						new Item(393, -1, 1),
						new Item(391, -1, 1),
						new Item(39, -1, 1),
						new Item(412, -1, 1),
					}),
				new ShapelessRecipe(new Item(413, 0, 1),
					new List<Item>
					{
						new Item(281, -1, 1),
						new Item(393, -1, 1),
						new Item(391, -1, 1),
						new Item(40, -1, 1),
						new Item(412, -1, 1),
					}),
				new ShapedRecipe(3, 3, new Item(66, 0, 16),
					new Item[]
					{
						new Item(265, -1),
						new Item(265, -1),
						new Item(265, -1),
						new Item(0, 0),
						new Item(280, -1),
						new Item(0, 0),
						new Item(265, -1),
						new Item(265, -1),
						new Item(265, -1),
					}),
				new ShapedRecipe(1, 1, new Item(331, 0, 9),
					new Item[]
					{
						new Item(152, -1),
					}),
				new ShapedRecipe(3, 3, new Item(152, 0, 1),
					new Item[]
					{
						new Item(331, -1),
						new Item(331, -1),
						new Item(331, -1),
						new Item(331, -1),
						new Item(331, -1),
						new Item(331, -1),
						new Item(331, -1),
						new Item(331, -1),
						new Item(331, -1),
					}),
				new ShapedRecipe(3, 3, new Item(123, 0, 1),
					new Item[]
					{
						new Item(0, 0),
						new Item(331, -1),
						new Item(0, 0),
						new Item(331, -1),
						new Item(89, -1),
						new Item(331, -1),
						new Item(0, 0),
						new Item(331, -1),
						new Item(0, 0),
					}),
				new ShapedRecipe(1, 2, new Item(76, 0, 1),
					new Item[]
					{
						new Item(331, -1),
						new Item(280, -1),
					}),
				new ShapedRecipe(3, 3, new Item(446, 1, 1),
					new Item[]
					{
						new Item(35, 14),
						new Item(35, 14),
						new Item(0, 0),
						new Item(35, 14),
						new Item(35, 14),
						new Item(280, -1),
						new Item(35, 14),
						new Item(35, 14),
						new Item(0, 0),
					}),
				new ShapedRecipe(2, 1, new Item(171, 14, 3),
					new Item[]
					{
						new Item(35, 14),
						new Item(35, 14),
					}),
				new ShapedRecipe(3, 3, new Item(171, 14, 8),
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
					}),
				new ShapelessRecipe(new Item(237, 14, 8),
					new List<Item>
					{
						new Item(351, 1, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(13, -1, 1),
						new Item(13, -1, 1),
						new Item(13, -1, 1),
						new Item(13, -1, 1),
					}),
				new ShapelessRecipe(new Item(351, 1, 1),
					new List<Item>
					{
						new Item(457, -1, 1),
					}),
				new ShapelessRecipe(new Item(351, 1, 1),
					new List<Item>
					{
						new Item(38, 0, 1),
					}),
				new ShapelessRecipe(new Item(351, 1, 2),
					new List<Item>
					{
						new Item(175, 4, 1),
					}),
				new ShapelessRecipe(new Item(351, 1, 1),
					new List<Item>
					{
						new Item(38, 4, 1),
					}),
				new ShapedRecipe(2, 2, new Item(215, 0, 1),
					new Item[]
					{
						new Item(405, -1),
						new Item(372, -1),
						new Item(372, -1),
						new Item(405, -1),
					}),
				new ShapedRecipe(3, 3, new Item(-184, 0, 4),
					new Item[]
					{
						new Item(215, -1),
						new Item(215, -1),
						new Item(215, -1),
						new Item(0, 0),
						new Item(215, -1),
						new Item(215, -1),
						new Item(0, 0),
						new Item(0, 0),
						new Item(215, -1),
					}),
				new ShapedRecipe(3, 2, new Item(139, 13, 6),
					new Item[]
					{
						new Item(215, -1),
						new Item(215, -1),
						new Item(215, -1),
						new Item(215, -1),
						new Item(215, -1),
						new Item(215, -1),
					}),
				new ShapedRecipe(2, 2, new Item(179, 0, 1),
					new Item[]
					{
						new Item(12, 1),
						new Item(12, 1),
						new Item(12, 1),
						new Item(12, 1),
					}),
				new ShapedRecipe(3, 3, new Item(180, 0, 4),
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
					}),
				new ShapedRecipe(3, 2, new Item(139, 12, 6),
					new Item[]
					{
						new Item(179, 0),
						new Item(179, 0),
						new Item(179, 0),
						new Item(179, 0),
						new Item(179, 0),
						new Item(179, 0),
					}),
				new ShapedRecipe(3, 3, new Item(241, 14, 8),
					new Item[]
					{
						new Item(20, -1),
						new Item(20, -1),
						new Item(20, -1),
						new Item(20, -1),
						new Item(351, 1),
						new Item(20, -1),
						new Item(20, -1),
						new Item(20, -1),
						new Item(20, -1),
					}),
				new ShapedRecipe(3, 2, new Item(160, 14, 16),
					new Item[]
					{
						new Item(241, 14),
						new Item(241, 14),
						new Item(241, 14),
						new Item(241, 14),
						new Item(241, 14),
						new Item(241, 14),
					}),
				new ShapedRecipe(3, 3, new Item(160, 14, 8),
					new Item[]
					{
						new Item(102, -1),
						new Item(102, -1),
						new Item(102, -1),
						new Item(102, -1),
						new Item(351, 1),
						new Item(102, -1),
						new Item(102, -1),
						new Item(102, -1),
						new Item(102, -1),
					}),
				new ShapedRecipe(3, 3, new Item(159, 14, 8),
					new Item[]
					{
						new Item(172, -1),
						new Item(172, -1),
						new Item(172, -1),
						new Item(172, -1),
						new Item(351, 1),
						new Item(172, -1),
						new Item(172, -1),
						new Item(172, -1),
						new Item(172, -1),
					}),
				new ShapedRecipe(3, 2, new Item(356, 0, 1),
					new Item[]
					{
						new Item(76, -1),
						new Item(76, -1),
						new Item(1, 0),
						new Item(331, -1),
						new Item(1, 0),
						new Item(1, 0),
					}),
				new ShapedRecipe(2, 2, new Item(24, 0, 1),
					new Item[]
					{
						new Item(12, 0),
						new Item(12, 0),
						new Item(12, 0),
						new Item(12, 0),
					}),
				new ShapedRecipe(3, 3, new Item(128, 0, 4),
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
					}),
				new ShapedRecipe(3, 2, new Item(139, 5, 6),
					new Item[]
					{
						new Item(24, 0),
						new Item(24, 0),
						new Item(24, 0),
						new Item(24, 0),
						new Item(24, 0),
						new Item(24, 0),
					}),
				new ShapedRecipe(3, 3, new Item(-165, 0, 6),
					new Item[]
					{
						new Item(-163, -1),
						new Item(-163, -1),
						new Item(-163, -1),
						new Item(287, -1),
						new Item(0, 0),
						new Item(0, 0),
						new Item(-163, -1),
						new Item(-163, -1),
						new Item(-163, -1),
					}),
				new ShapedRecipe(3, 3, new Item(169, 0, 1),
					new Item[]
					{
						new Item(409, -1),
						new Item(422, -1),
						new Item(409, -1),
						new Item(422, -1),
						new Item(422, -1),
						new Item(422, -1),
						new Item(409, -1),
						new Item(422, -1),
						new Item(409, -1),
					}),
				new ShapedRecipe(2, 2, new Item(359, 0, 1),
					new Item[]
					{
						new Item(0, 0),
						new Item(265, -1),
						new Item(265, -1),
						new Item(0, 0),
					}),
				new ShapedRecipe(3, 3, new Item(513, 0, 1),
					new Item[]
					{
						new Item(5, -1),
						new Item(5, -1),
						new Item(0, 0),
						new Item(265, -1),
						new Item(5, -1),
						new Item(5, -1),
						new Item(5, -1),
						new Item(5, -1),
						new Item(0, 0),
					}),
				new ShapedRecipe(1, 3, new Item(205, 0, 1),
					new Item[]
					{
						new Item(445, -1),
						new Item(54, -1),
						new Item(445, -1),
					}),
				new ShapedRecipe(3, 3, new Item(475, 0, 3),
					new Item[]
					{
						new Item(5, 4),
						new Item(5, 4),
						new Item(0, 0),
						new Item(5, 4),
						new Item(5, 4),
						new Item(280, -1),
						new Item(5, 4),
						new Item(5, 4),
						new Item(0, 0),
					}),
				new ShapedRecipe(3, 3, new Item(473, 0, 3),
					new Item[]
					{
						new Item(5, 2),
						new Item(5, 2),
						new Item(0, 0),
						new Item(5, 2),
						new Item(5, 2),
						new Item(280, -1),
						new Item(5, 2),
						new Item(5, 2),
						new Item(0, 0),
					}),
				new ShapedRecipe(3, 3, new Item(476, 0, 3),
					new Item[]
					{
						new Item(5, 5),
						new Item(5, 5),
						new Item(0, 0),
						new Item(5, 5),
						new Item(5, 5),
						new Item(280, -1),
						new Item(5, 5),
						new Item(5, 5),
						new Item(0, 0),
					}),
				new ShapedRecipe(3, 3, new Item(474, 0, 3),
					new Item[]
					{
						new Item(5, 3),
						new Item(5, 3),
						new Item(0, 0),
						new Item(5, 3),
						new Item(5, 3),
						new Item(280, -1),
						new Item(5, 3),
						new Item(5, 3),
						new Item(0, 0),
					}),
				new ShapedRecipe(3, 3, new Item(323, 0, 3),
					new Item[]
					{
						new Item(5, 0),
						new Item(5, 0),
						new Item(0, 0),
						new Item(5, 0),
						new Item(5, 0),
						new Item(280, -1),
						new Item(5, 0),
						new Item(5, 0),
						new Item(0, 0),
					}),
				new ShapedRecipe(3, 3, new Item(472, 0, 3),
					new Item[]
					{
						new Item(5, 1),
						new Item(5, 1),
						new Item(0, 0),
						new Item(5, 1),
						new Item(5, 1),
						new Item(280, -1),
						new Item(5, 1),
						new Item(5, 1),
						new Item(0, 0),
					}),
				new ShapedRecipe(3, 3, new Item(165, 0, 1),
					new Item[]
					{
						new Item(341, -1),
						new Item(341, -1),
						new Item(341, -1),
						new Item(341, -1),
						new Item(341, -1),
						new Item(341, -1),
						new Item(341, -1),
						new Item(341, -1),
						new Item(341, -1),
					}),
				new ShapedRecipe(1, 1, new Item(341, 0, 9),
					new Item[]
					{
						new Item(165, -1),
					}),
				new ShapelessRecipe(new Item(-202, 0, 1),
					new List<Item>
					{
						new Item(58, -1, 1),
						new Item(4, -1, 1),
					}),
				new ShapedRecipe(3, 3, new Item(-198, 0, 1),
					new Item[]
					{
						new Item(0, 0),
						new Item(17, -1),
						new Item(0, 0),
						new Item(17, -1),
						new Item(61, -1),
						new Item(17, -1),
						new Item(0, 0),
						new Item(17, -1),
						new Item(0, 0),
					}),
				new ShapedRecipe(3, 3, new Item(-198, 0, 1),
					new Item[]
					{
						new Item(0, 0),
						new Item(162, -1),
						new Item(0, 0),
						new Item(162, -1),
						new Item(61, -1),
						new Item(162, -1),
						new Item(0, 0),
						new Item(162, -1),
						new Item(0, 0),
					}),
				new ShapedRecipe(3, 3, new Item(-198, 0, 1),
					new Item[]
					{
						new Item(0, 0),
						new Item(-8, -1),
						new Item(0, 0),
						new Item(-8, -1),
						new Item(61, -1),
						new Item(-8, -1),
						new Item(0, 0),
						new Item(-8, -1),
						new Item(0, 0),
					}),
				new ShapedRecipe(3, 3, new Item(-198, 0, 1),
					new Item[]
					{
						new Item(0, 0),
						new Item(-6, -1),
						new Item(0, 0),
						new Item(-6, -1),
						new Item(61, -1),
						new Item(-6, -1),
						new Item(0, 0),
						new Item(-6, -1),
						new Item(0, 0),
					}),
				new ShapedRecipe(3, 3, new Item(-198, 0, 1),
					new Item[]
					{
						new Item(0, 0),
						new Item(-9, -1),
						new Item(0, 0),
						new Item(-9, -1),
						new Item(61, -1),
						new Item(-9, -1),
						new Item(0, 0),
						new Item(-9, -1),
						new Item(0, 0),
					}),
				new ShapedRecipe(3, 3, new Item(-198, 0, 1),
					new Item[]
					{
						new Item(0, 0),
						new Item(-7, -1),
						new Item(0, 0),
						new Item(-7, -1),
						new Item(61, -1),
						new Item(-7, -1),
						new Item(0, 0),
						new Item(-7, -1),
						new Item(0, 0),
					}),
				new ShapedRecipe(3, 3, new Item(-198, 0, 1),
					new Item[]
					{
						new Item(0, 0),
						new Item(-10, -1),
						new Item(0, 0),
						new Item(-10, -1),
						new Item(61, -1),
						new Item(-10, -1),
						new Item(0, 0),
						new Item(-10, -1),
						new Item(0, 0),
					}),
				new ShapedRecipe(3, 3, new Item(-198, 0, 1),
					new Item[]
					{
						new Item(0, 0),
						new Item(-5, -1),
						new Item(0, 0),
						new Item(-5, -1),
						new Item(61, -1),
						new Item(-5, -1),
						new Item(0, 0),
						new Item(-5, -1),
						new Item(0, 0),
					}),
				new ShapedRecipe(3, 3, new Item(-185, 0, 4),
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
					}),
				new ShapedRecipe(2, 2, new Item(179, 2, 4),
					new Item[]
					{
						new Item(179, 0),
						new Item(179, 0),
						new Item(179, 0),
						new Item(179, 0),
					}),
				new ShapedRecipe(3, 3, new Item(-176, 0, 4),
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
					}),
				new ShapedRecipe(2, 2, new Item(24, 2, 4),
					new Item[]
					{
						new Item(24, 0),
						new Item(24, 0),
						new Item(24, 0),
						new Item(24, 0),
					}),
				new ShapedRecipe(3, 3, new Item(-177, 0, 4),
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
					}),
				new ShapedRecipe(2, 2, new Item(80, 0, 1),
					new Item[]
					{
						new Item(332, -1),
						new Item(332, -1),
						new Item(332, -1),
						new Item(332, -1),
					}),
				new ShapedRecipe(3, 1, new Item(78, 0, 6),
					new Item[]
					{
						new Item(80, -1),
						new Item(80, -1),
						new Item(80, -1),
					}),
				new ShapedRecipe(3, 3, new Item(382, 0, 1),
					new Item[]
					{
						new Item(371, -1),
						new Item(371, -1),
						new Item(371, -1),
						new Item(371, -1),
						new Item(360, -1),
						new Item(371, -1),
						new Item(371, -1),
						new Item(371, -1),
						new Item(371, -1),
					}),
				new ShapedRecipe(3, 2, new Item(333, 1, 1),
					new Item[]
					{
						new Item(5, 1),
						new Item(5, 1),
						new Item(5, 1),
						new Item(269, -1),
						new Item(5, 1),
						new Item(5, 1),
					}),
				new ShapedRecipe(2, 3, new Item(427, 0, 3),
					new Item[]
					{
						new Item(5, 1),
						new Item(5, 1),
						new Item(5, 1),
						new Item(5, 1),
						new Item(5, 1),
						new Item(5, 1),
					}),
				new ShapedRecipe(3, 2, new Item(85, 1, 3),
					new Item[]
					{
						new Item(5, 1),
						new Item(5, 1),
						new Item(280, -1),
						new Item(280, -1),
						new Item(5, 1),
						new Item(5, 1),
					}),
				new ShapedRecipe(3, 2, new Item(183, 0, 1),
					new Item[]
					{
						new Item(280, -1),
						new Item(280, -1),
						new Item(5, 1),
						new Item(5, 1),
						new Item(280, -1),
						new Item(280, -1),
					}),
				new ShapedRecipe(1, 1, new Item(5, 1, 4),
					new Item[]
					{
						new Item(17, 1),
					}),
				new ShapedRecipe(1, 1, new Item(5, 1, 4),
					new Item[]
					{
						new Item(-5, -1),
					}),
				new ShapedRecipe(1, 1, new Item(5, 1, 4),
					new Item[]
					{
						new Item(-212, 9),
					}),
				new ShapedRecipe(1, 1, new Item(5, 1, 4),
					new Item[]
					{
						new Item(-212, 1),
					}),
				new ShapedRecipe(3, 3, new Item(134, 0, 4),
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
					}),
				new ShapedRecipe(2, 2, new Item(-212, 1, 3),
					new Item[]
					{
						new Item(17, 1),
						new Item(17, 1),
						new Item(17, 1),
						new Item(17, 1),
					}),
				new ShapedRecipe(3, 1, new Item(158, 1, 6),
					new Item[]
					{
						new Item(5, 1),
						new Item(5, 1),
						new Item(5, 1),
					}),
				new ShapedRecipe(2, 2, new Item(-212, 9, 3),
					new Item[]
					{
						new Item(-5, -1),
						new Item(-5, -1),
						new Item(-5, -1),
						new Item(-5, -1),
					}),
				new ShapedRecipe(1, 2, new Item(29, 1, 1),
					new Item[]
					{
						new Item(341, -1),
						new Item(33, -1),
					}),
				new ShapedRecipe(2, 2, new Item(98, 0, 4),
					new Item[]
					{
						new Item(1, 0),
						new Item(1, 0),
						new Item(1, 0),
						new Item(1, 0),
					}),
				new ShapedRecipe(3, 2, new Item(-197, 0, 1),
					new Item[]
					{
						new Item(0, 0),
						new Item(0, 0),
						new Item(1, -1),
						new Item(265, -1),
						new Item(1, -1),
						new Item(1, -1),
					}),
				new ShapedRecipe(2, 3, new Item(275, 0, 1),
					new Item[]
					{
						new Item(4, -1),
						new Item(280, -1),
						new Item(4, -1),
						new Item(0, 0),
						new Item(4, -1),
						new Item(280, -1),
					}),
				new ShapedRecipe(3, 3, new Item(109, 0, 4),
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
					}),
				new ShapedRecipe(3, 2, new Item(139, 7, 6),
					new Item[]
					{
						new Item(98, 0),
						new Item(98, 0),
						new Item(98, 0),
						new Item(98, 0),
						new Item(98, 0),
						new Item(98, 0),
					}),
				new ShapedRecipe(1, 1, new Item(77, 5, 1),
					new Item[]
					{
						new Item(1, 0),
					}),
				new ShapedRecipe(2, 3, new Item(291, 0, 1),
					new Item[]
					{
						new Item(4, -1),
						new Item(280, -1),
						new Item(4, -1),
						new Item(0, 0),
						new Item(0, 0),
						new Item(280, -1),
					}),
				new ShapedRecipe(3, 3, new Item(274, 0, 1),
					new Item[]
					{
						new Item(4, -1),
						new Item(0, 0),
						new Item(0, 0),
						new Item(4, -1),
						new Item(280, -1),
						new Item(280, -1),
						new Item(4, -1),
						new Item(0, 0),
						new Item(0, 0),
					}),
				new ShapedRecipe(2, 1, new Item(70, 0, 1),
					new Item[]
					{
						new Item(1, 0),
						new Item(1, 0),
					}),
				new ShapedRecipe(1, 3, new Item(273, 0, 1),
					new Item[]
					{
						new Item(4, -1),
						new Item(280, -1),
						new Item(280, -1),
					}),
				new ShapedRecipe(3, 3, new Item(-180, 0, 4),
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
					}),
				new ShapedRecipe(1, 3, new Item(272, 0, 1),
					new Item[]
					{
						new Item(4, -1),
						new Item(4, -1),
						new Item(280, -1),
					}),
				new ShapedRecipe(2, 2, new Item(35, 0, 1),
					new Item[]
					{
						new Item(287, -1),
						new Item(287, -1),
						new Item(287, -1),
						new Item(287, -1),
					}),
				new ShapedRecipe(1, 1, new Item(353, 0, 1),
					new Item[]
					{
						new Item(338, -1),
					}),
				new ShapedRecipe(1, 2, new Item(407, 0, 1),
					new Item[]
					{
						new Item(46, 0),
						new Item(328, -1),
					}),
				new ShapelessRecipe(new Item(146, 0, 1),
					new List<Item>
					{
						new Item(54, -1, 1),
						new Item(131, -1, 1),
					}),
				new ShapedRecipe(3, 2, new Item(469, 0, 1),
					new Item[]
					{
						new Item(468, -1),
						new Item(468, -1),
						new Item(0, 0),
						new Item(468, -1),
						new Item(468, -1),
						new Item(468, -1),
					}),
				new ShapedRecipe(1, 1, new Item(296, 0, 9),
					new Item[]
					{
						new Item(170, -1),
					}),
				new ShapedRecipe(3, 3, new Item(446, 15, 1),
					new Item[]
					{
						new Item(35, 0),
						new Item(35, 0),
						new Item(0, 0),
						new Item(35, 0),
						new Item(35, 0),
						new Item(280, -1),
						new Item(35, 0),
						new Item(35, 0),
						new Item(0, 0),
					}),
				new ShapedRecipe(2, 1, new Item(171, 0, 3),
					new Item[]
					{
						new Item(35, 0),
						new Item(35, 0),
					}),
				new ShapelessRecipe(new Item(237, 0, 8),
					new List<Item>
					{
						new Item(351, 19, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(13, -1, 1),
						new Item(13, -1, 1),
						new Item(13, -1, 1),
						new Item(13, -1, 1),
					}),
				new ShapelessRecipe(new Item(351, 19, 1),
					new List<Item>
					{
						new Item(351, 15, 1),
					}),
				new ShapelessRecipe(new Item(351, 19, 1),
					new List<Item>
					{
						new Item(38, 10, 1),
					}),
				new ShapedRecipe(3, 3, new Item(241, 0, 8),
					new Item[]
					{
						new Item(20, -1),
						new Item(20, -1),
						new Item(20, -1),
						new Item(20, -1),
						new Item(351, 19),
						new Item(20, -1),
						new Item(20, -1),
						new Item(20, -1),
						new Item(20, -1),
					}),
				new ShapedRecipe(3, 3, new Item(171, 15, 8),
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
					}),
				new ShapelessRecipe(new Item(237, 15, 8),
					new List<Item>
					{
						new Item(351, 0, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(13, -1, 1),
						new Item(13, -1, 1),
						new Item(13, -1, 1),
						new Item(13, -1, 1),
					}),
				new ShapedRecipe(3, 3, new Item(241, 15, 8),
					new Item[]
					{
						new Item(20, -1),
						new Item(20, -1),
						new Item(20, -1),
						new Item(20, -1),
						new Item(351, 0),
						new Item(20, -1),
						new Item(20, -1),
						new Item(20, -1),
						new Item(20, -1),
					}),
				new ShapedRecipe(3, 3, new Item(159, 15, 8),
					new Item[]
					{
						new Item(172, -1),
						new Item(172, -1),
						new Item(172, -1),
						new Item(172, -1),
						new Item(351, 0),
						new Item(172, -1),
						new Item(172, -1),
						new Item(172, -1),
						new Item(172, -1),
					}),
				new ShapelessRecipe(new Item(237, 11, 8),
					new List<Item>
					{
						new Item(351, 4, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(13, -1, 1),
						new Item(13, -1, 1),
						new Item(13, -1, 1),
						new Item(13, -1, 1),
					}),
				new ShapedRecipe(3, 3, new Item(159, 11, 8),
					new Item[]
					{
						new Item(172, -1),
						new Item(172, -1),
						new Item(172, -1),
						new Item(172, -1),
						new Item(351, 4),
						new Item(172, -1),
						new Item(172, -1),
						new Item(172, -1),
						new Item(172, -1),
					}),
				new ShapelessRecipe(new Item(237, 12, 8),
					new List<Item>
					{
						new Item(351, 3, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(13, -1, 1),
						new Item(13, -1, 1),
						new Item(13, -1, 1),
						new Item(13, -1, 1),
					}),
				new ShapedRecipe(3, 3, new Item(241, 12, 8),
					new Item[]
					{
						new Item(20, -1),
						new Item(20, -1),
						new Item(20, -1),
						new Item(20, -1),
						new Item(351, 3),
						new Item(20, -1),
						new Item(20, -1),
						new Item(20, -1),
						new Item(20, -1),
					}),
				new ShapedRecipe(3, 3, new Item(159, 12, 8),
					new Item[]
					{
						new Item(172, -1),
						new Item(172, -1),
						new Item(172, -1),
						new Item(172, -1),
						new Item(351, 3),
						new Item(172, -1),
						new Item(172, -1),
						new Item(172, -1),
						new Item(172, -1),
					}),
				new ShapelessRecipe(new Item(351, 6, 2),
					new List<Item>
					{
						new Item(351, 4, 1),
						new Item(351, 2, 1),
					}),
				new ShapedRecipe(3, 3, new Item(241, 11, 8),
					new Item[]
					{
						new Item(20, -1),
						new Item(20, -1),
						new Item(20, -1),
						new Item(20, -1),
						new Item(351, 4),
						new Item(20, -1),
						new Item(20, -1),
						new Item(20, -1),
						new Item(20, -1),
					}),
				new ShapedRecipe(3, 3, new Item(168, 1, 1),
					new Item[]
					{
						new Item(409, -1),
						new Item(409, -1),
						new Item(409, -1),
						new Item(409, -1),
						new Item(351, 0),
						new Item(409, -1),
						new Item(409, -1),
						new Item(409, -1),
						new Item(409, -1),
					}),
				new ShapelessRecipe(new Item(351, 8, 2),
					new List<Item>
					{
						new Item(351, 16, 1),
						new Item(351, 15, 1),
					}),
				new ShapedRecipe(3, 3, new Item(241, 0, 8),
					new Item[]
					{
						new Item(20, -1),
						new Item(20, -1),
						new Item(20, -1),
						new Item(20, -1),
						new Item(351, 15),
						new Item(20, -1),
						new Item(20, -1),
						new Item(20, -1),
						new Item(20, -1),
					}),
				new ShapedRecipe(3, 3, new Item(159, 0, 8),
					new Item[]
					{
						new Item(172, -1),
						new Item(172, -1),
						new Item(172, -1),
						new Item(172, -1),
						new Item(351, 15),
						new Item(172, -1),
						new Item(172, -1),
						new Item(172, -1),
						new Item(172, -1),
					}),
				new ShapelessRecipe(new Item(351, 10, 2),
					new List<Item>
					{
						new Item(351, 2, 1),
						new Item(351, 15, 1),
					}),
				new ShapelessRecipe(new Item(351, 7, 1),
					new List<Item>
					{
						new Item(38, 8, 1),
					}),
				new ShapelessRecipe(new Item(351, 5, 2),
					new List<Item>
					{
						new Item(351, 4, 1),
						new Item(351, 1, 1),
					}),
				new ShapelessRecipe(new Item(351, 13, 1),
					new List<Item>
					{
						new Item(38, 2, 1),
					}),
				new ShapelessRecipe(new Item(237, 0, 8),
					new List<Item>
					{
						new Item(351, 15, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(12, 0, 1),
						new Item(13, -1, 1),
						new Item(13, -1, 1),
						new Item(13, -1, 1),
						new Item(13, -1, 1),
					}),
				new ShapelessRecipe(new Item(351, 12, 2),
					new List<Item>
					{
						new Item(351, 18, 1),
						new Item(351, 19, 1),
					}),
				new ShapelessRecipe(new Item(351, 8, 2),
					new List<Item>
					{
						new Item(351, 0, 1),
						new Item(351, 19, 1),
					}),
				new ShapelessRecipe(new Item(351, 12, 2),
					new List<Item>
					{
						new Item(351, 18, 1),
						new Item(351, 15, 1),
					}),
				new ShapelessRecipe(new Item(351, 13, 3),
					new List<Item>
					{
						new Item(351, 18, 1),
						new Item(351, 1, 1),
						new Item(351, 9, 1),
					}),
				new ShapelessRecipe(new Item(351, 7, 1),
					new List<Item>
					{
						new Item(38, 3, 1),
					}),
				new ShapelessRecipe(new Item(351, 8, 2),
					new List<Item>
					{
						new Item(351, 0, 1),
						new Item(351, 15, 1),
					}),
				new ShapelessRecipe(new Item(351, 12, 2),
					new List<Item>
					{
						new Item(351, 4, 1),
						new Item(351, 19, 1),
					}),
				new ShapelessRecipe(new Item(351, 7, 3),
					new List<Item>
					{
						new Item(351, 16, 1),
						new Item(351, 19, 1),
						new Item(351, 19, 1),
					}),
				new ShapelessRecipe(new Item(351, 13, 2),
					new List<Item>
					{
						new Item(351, 5, 1),
						new Item(351, 9, 1),
					}),
				new ShapelessRecipe(new Item(351, 7, 2),
					new List<Item>
					{
						new Item(351, 8, 1),
						new Item(351, 19, 1),
					}),
				new ShapelessRecipe(new Item(351, 12, 2),
					new List<Item>
					{
						new Item(351, 4, 1),
						new Item(351, 15, 1),
					}),
				new ShapelessRecipe(new Item(351, 13, 4),
					new List<Item>
					{
						new Item(351, 18, 1),
						new Item(351, 1, 1),
						new Item(351, 1, 1),
						new Item(351, 19, 1),
					}),
				new ShapelessRecipe(new Item(351, 13, 3),
					new List<Item>
					{
						new Item(351, 4, 1),
						new Item(351, 1, 1),
						new Item(351, 9, 1),
					}),
				new ShapelessRecipe(new Item(351, 7, 3),
					new List<Item>
					{
						new Item(351, 0, 1),
						new Item(351, 19, 1),
						new Item(351, 19, 1),
					}),
				new ShapelessRecipe(new Item(351, 7, 2),
					new List<Item>
					{
						new Item(351, 8, 1),
						new Item(351, 15, 1),
					}),
				new ShapelessRecipe(new Item(351, 13, 4),
					new List<Item>
					{
						new Item(351, 18, 1),
						new Item(351, 1, 1),
						new Item(351, 1, 1),
						new Item(351, 15, 1),
					}),
				new ShapelessRecipe(new Item(351, 7, 3),
					new List<Item>
					{
						new Item(351, 16, 1),
						new Item(351, 15, 1),
						new Item(351, 15, 1),
					}),
				new ShapelessRecipe(new Item(351, 13, 4),
					new List<Item>
					{
						new Item(351, 4, 1),
						new Item(351, 1, 1),
						new Item(351, 1, 1),
						new Item(351, 19, 1),
					}),
				new ShapelessRecipe(new Item(351, 7, 3),
					new List<Item>
					{
						new Item(351, 0, 1),
						new Item(351, 15, 1),
						new Item(351, 15, 1),
					}),
				new ShapelessRecipe(new Item(351, 13, 4),
					new List<Item>
					{
						new Item(351, 4, 1),
						new Item(351, 1, 1),
						new Item(351, 1, 1),
						new Item(351, 15, 1),
					}),
				new ShapelessRecipe(new Item(35, 0, 1),
					new List<Item>
					{
						new Item(351, 19, 1),
						new Item(35, 1, 1),
					}),
				new ShapelessRecipe(new Item(35, 0, 1),
					new List<Item>
					{
						new Item(351, 19, 1),
						new Item(35, 2, 1),
					}),
				new ShapelessRecipe(new Item(35, 0, 1),
					new List<Item>
					{
						new Item(351, 19, 1),
						new Item(35, 3, 1),
					}),
				new ShapelessRecipe(new Item(35, 0, 1),
					new List<Item>
					{
						new Item(351, 19, 1),
						new Item(35, 4, 1),
					}),
				new ShapelessRecipe(new Item(35, 0, 1),
					new List<Item>
					{
						new Item(351, 19, 1),
						new Item(35, 5, 1),
					}),
				new ShapelessRecipe(new Item(35, 0, 1),
					new List<Item>
					{
						new Item(351, 19, 1),
						new Item(35, 6, 1),
					}),
				new ShapelessRecipe(new Item(35, 0, 1),
					new List<Item>
					{
						new Item(351, 19, 1),
						new Item(35, 7, 1),
					}),
				new ShapelessRecipe(new Item(35, 0, 1),
					new List<Item>
					{
						new Item(351, 19, 1),
						new Item(35, 8, 1),
					}),
				new ShapelessRecipe(new Item(35, 0, 1),
					new List<Item>
					{
						new Item(351, 19, 1),
						new Item(35, 9, 1),
					}),
				new ShapelessRecipe(new Item(35, 0, 1),
					new List<Item>
					{
						new Item(351, 19, 1),
						new Item(35, 10, 1),
					}),
				new ShapelessRecipe(new Item(35, 0, 1),
					new List<Item>
					{
						new Item(351, 19, 1),
						new Item(35, 11, 1),
					}),
				new ShapelessRecipe(new Item(35, 0, 1),
					new List<Item>
					{
						new Item(351, 19, 1),
						new Item(35, 12, 1),
					}),
				new ShapelessRecipe(new Item(35, 0, 1),
					new List<Item>
					{
						new Item(351, 19, 1),
						new Item(35, 13, 1),
					}),
				new ShapelessRecipe(new Item(35, 0, 1),
					new List<Item>
					{
						new Item(351, 19, 1),
						new Item(35, 14, 1),
					}),
				new ShapelessRecipe(new Item(35, 0, 1),
					new List<Item>
					{
						new Item(351, 19, 1),
						new Item(35, 15, 1),
					}),
				new ShapelessRecipe(new Item(35, 11, 1),
					new List<Item>
					{
						new Item(351, 18, 1),
						new Item(35, 0, 1),
					}),
				new ShapelessRecipe(new Item(35, 11, 1),
					new List<Item>
					{
						new Item(351, 18, 1),
						new Item(35, 1, 1),
					}),
				new ShapelessRecipe(new Item(35, 11, 1),
					new List<Item>
					{
						new Item(351, 18, 1),
						new Item(35, 2, 1),
					}),
				new ShapelessRecipe(new Item(35, 11, 1),
					new List<Item>
					{
						new Item(351, 18, 1),
						new Item(35, 3, 1),
					}),
				new ShapelessRecipe(new Item(35, 11, 1),
					new List<Item>
					{
						new Item(351, 18, 1),
						new Item(35, 4, 1),
					}),
				new ShapelessRecipe(new Item(35, 11, 1),
					new List<Item>
					{
						new Item(351, 18, 1),
						new Item(35, 5, 1),
					}),
				new ShapelessRecipe(new Item(35, 11, 1),
					new List<Item>
					{
						new Item(351, 18, 1),
						new Item(35, 6, 1),
					}),
				new ShapelessRecipe(new Item(35, 11, 1),
					new List<Item>
					{
						new Item(351, 18, 1),
						new Item(35, 7, 1),
					}),
				new ShapelessRecipe(new Item(35, 11, 1),
					new List<Item>
					{
						new Item(351, 18, 1),
						new Item(35, 8, 1),
					}),
				new ShapelessRecipe(new Item(35, 11, 1),
					new List<Item>
					{
						new Item(351, 18, 1),
						new Item(35, 9, 1),
					}),
				new ShapelessRecipe(new Item(35, 11, 1),
					new List<Item>
					{
						new Item(351, 18, 1),
						new Item(35, 10, 1),
					}),
				new ShapelessRecipe(new Item(35, 11, 1),
					new List<Item>
					{
						new Item(351, 18, 1),
						new Item(35, 12, 1),
					}),
				new ShapelessRecipe(new Item(35, 11, 1),
					new List<Item>
					{
						new Item(351, 18, 1),
						new Item(35, 13, 1),
					}),
				new ShapelessRecipe(new Item(35, 11, 1),
					new List<Item>
					{
						new Item(351, 18, 1),
						new Item(35, 14, 1),
					}),
				new ShapelessRecipe(new Item(35, 11, 1),
					new List<Item>
					{
						new Item(351, 18, 1),
						new Item(35, 15, 1),
					}),
				new ShapelessRecipe(new Item(35, 12, 1),
					new List<Item>
					{
						new Item(351, 17, 1),
						new Item(35, 0, 1),
					}),
				new ShapelessRecipe(new Item(35, 12, 1),
					new List<Item>
					{
						new Item(351, 17, 1),
						new Item(35, 1, 1),
					}),
				new ShapelessRecipe(new Item(35, 12, 1),
					new List<Item>
					{
						new Item(351, 17, 1),
						new Item(35, 2, 1),
					}),
				new ShapelessRecipe(new Item(35, 12, 1),
					new List<Item>
					{
						new Item(351, 17, 1),
						new Item(35, 3, 1),
					}),
				new ShapelessRecipe(new Item(35, 12, 1),
					new List<Item>
					{
						new Item(351, 17, 1),
						new Item(35, 4, 1),
					}),
				new ShapelessRecipe(new Item(35, 12, 1),
					new List<Item>
					{
						new Item(351, 17, 1),
						new Item(35, 5, 1),
					}),
				new ShapelessRecipe(new Item(35, 12, 1),
					new List<Item>
					{
						new Item(351, 17, 1),
						new Item(35, 6, 1),
					}),
				new ShapelessRecipe(new Item(35, 12, 1),
					new List<Item>
					{
						new Item(351, 17, 1),
						new Item(35, 7, 1),
					}),
				new ShapelessRecipe(new Item(35, 12, 1),
					new List<Item>
					{
						new Item(351, 17, 1),
						new Item(35, 8, 1),
					}),
				new ShapelessRecipe(new Item(35, 12, 1),
					new List<Item>
					{
						new Item(351, 17, 1),
						new Item(35, 9, 1),
					}),
				new ShapelessRecipe(new Item(35, 12, 1),
					new List<Item>
					{
						new Item(351, 17, 1),
						new Item(35, 10, 1),
					}),
				new ShapelessRecipe(new Item(35, 12, 1),
					new List<Item>
					{
						new Item(351, 17, 1),
						new Item(35, 11, 1),
					}),
				new ShapelessRecipe(new Item(35, 12, 1),
					new List<Item>
					{
						new Item(351, 17, 1),
						new Item(35, 13, 1),
					}),
				new ShapelessRecipe(new Item(35, 12, 1),
					new List<Item>
					{
						new Item(351, 17, 1),
						new Item(35, 14, 1),
					}),
				new ShapelessRecipe(new Item(35, 12, 1),
					new List<Item>
					{
						new Item(351, 17, 1),
						new Item(35, 15, 1),
					}),
				new ShapelessRecipe(new Item(35, 15, 1),
					new List<Item>
					{
						new Item(351, 16, 1),
						new Item(35, 0, 1),
					}),
				new ShapelessRecipe(new Item(35, 15, 1),
					new List<Item>
					{
						new Item(351, 16, 1),
						new Item(35, 1, 1),
					}),
				new ShapelessRecipe(new Item(35, 15, 1),
					new List<Item>
					{
						new Item(351, 16, 1),
						new Item(35, 2, 1),
					}),
				new ShapelessRecipe(new Item(35, 15, 1),
					new List<Item>
					{
						new Item(351, 16, 1),
						new Item(35, 3, 1),
					}),
				new ShapelessRecipe(new Item(35, 15, 1),
					new List<Item>
					{
						new Item(351, 16, 1),
						new Item(35, 4, 1),
					}),
				new ShapelessRecipe(new Item(35, 15, 1),
					new List<Item>
					{
						new Item(351, 16, 1),
						new Item(35, 5, 1),
					}),
				new ShapelessRecipe(new Item(35, 15, 1),
					new List<Item>
					{
						new Item(351, 16, 1),
						new Item(35, 6, 1),
					}),
				new ShapelessRecipe(new Item(35, 15, 1),
					new List<Item>
					{
						new Item(351, 16, 1),
						new Item(35, 7, 1),
					}),
				new ShapelessRecipe(new Item(35, 15, 1),
					new List<Item>
					{
						new Item(351, 16, 1),
						new Item(35, 8, 1),
					}),
				new ShapelessRecipe(new Item(35, 15, 1),
					new List<Item>
					{
						new Item(351, 16, 1),
						new Item(35, 9, 1),
					}),
				new ShapelessRecipe(new Item(35, 15, 1),
					new List<Item>
					{
						new Item(351, 16, 1),
						new Item(35, 10, 1),
					}),
				new ShapelessRecipe(new Item(35, 15, 1),
					new List<Item>
					{
						new Item(351, 16, 1),
						new Item(35, 11, 1),
					}),
				new ShapelessRecipe(new Item(35, 15, 1),
					new List<Item>
					{
						new Item(351, 16, 1),
						new Item(35, 12, 1),
					}),
				new ShapelessRecipe(new Item(35, 15, 1),
					new List<Item>
					{
						new Item(351, 16, 1),
						new Item(35, 13, 1),
					}),
				new ShapelessRecipe(new Item(35, 15, 1),
					new List<Item>
					{
						new Item(351, 16, 1),
						new Item(35, 14, 1),
					}),
				new ShapelessRecipe(new Item(35, 0, 1),
					new List<Item>
					{
						new Item(351, 15, 1),
						new Item(35, 1, 1),
					}),
				new ShapelessRecipe(new Item(35, 0, 1),
					new List<Item>
					{
						new Item(351, 15, 1),
						new Item(35, 2, 1),
					}),
				new ShapelessRecipe(new Item(35, 0, 1),
					new List<Item>
					{
						new Item(351, 15, 1),
						new Item(35, 3, 1),
					}),
				new ShapelessRecipe(new Item(35, 0, 1),
					new List<Item>
					{
						new Item(351, 15, 1),
						new Item(35, 4, 1),
					}),
				new ShapelessRecipe(new Item(35, 0, 1),
					new List<Item>
					{
						new Item(351, 15, 1),
						new Item(35, 5, 1),
					}),
				new ShapelessRecipe(new Item(35, 0, 1),
					new List<Item>
					{
						new Item(351, 15, 1),
						new Item(35, 6, 1),
					}),
				new ShapelessRecipe(new Item(35, 0, 1),
					new List<Item>
					{
						new Item(351, 15, 1),
						new Item(35, 7, 1),
					}),
				new ShapelessRecipe(new Item(35, 0, 1),
					new List<Item>
					{
						new Item(351, 15, 1),
						new Item(35, 8, 1),
					}),
				new ShapelessRecipe(new Item(35, 0, 1),
					new List<Item>
					{
						new Item(351, 15, 1),
						new Item(35, 9, 1),
					}),
				new ShapelessRecipe(new Item(35, 0, 1),
					new List<Item>
					{
						new Item(351, 15, 1),
						new Item(35, 10, 1),
					}),
				new ShapelessRecipe(new Item(35, 0, 1),
					new List<Item>
					{
						new Item(351, 15, 1),
						new Item(35, 11, 1),
					}),
				new ShapelessRecipe(new Item(35, 0, 1),
					new List<Item>
					{
						new Item(351, 15, 1),
						new Item(35, 12, 1),
					}),
				new ShapelessRecipe(new Item(35, 0, 1),
					new List<Item>
					{
						new Item(351, 15, 1),
						new Item(35, 13, 1),
					}),
				new ShapelessRecipe(new Item(35, 0, 1),
					new List<Item>
					{
						new Item(351, 15, 1),
						new Item(35, 14, 1),
					}),
				new ShapelessRecipe(new Item(35, 0, 1),
					new List<Item>
					{
						new Item(351, 15, 1),
						new Item(35, 15, 1),
					}),
				new ShapelessRecipe(new Item(35, 1, 1),
					new List<Item>
					{
						new Item(351, 14, 1),
						new Item(35, 0, 1),
					}),
				new ShapelessRecipe(new Item(35, 1, 1),
					new List<Item>
					{
						new Item(351, 14, 1),
						new Item(35, 2, 1),
					}),
				new ShapelessRecipe(new Item(35, 1, 1),
					new List<Item>
					{
						new Item(351, 14, 1),
						new Item(35, 3, 1),
					}),
				new ShapelessRecipe(new Item(35, 1, 1),
					new List<Item>
					{
						new Item(351, 14, 1),
						new Item(35, 4, 1),
					}),
				new ShapelessRecipe(new Item(35, 1, 1),
					new List<Item>
					{
						new Item(351, 14, 1),
						new Item(35, 5, 1),
					}),
				new ShapelessRecipe(new Item(35, 1, 1),
					new List<Item>
					{
						new Item(351, 14, 1),
						new Item(35, 6, 1),
					}),
				new ShapelessRecipe(new Item(35, 1, 1),
					new List<Item>
					{
						new Item(351, 14, 1),
						new Item(35, 7, 1),
					}),
				new ShapelessRecipe(new Item(35, 1, 1),
					new List<Item>
					{
						new Item(351, 14, 1),
						new Item(35, 8, 1),
					}),
				new ShapelessRecipe(new Item(35, 1, 1),
					new List<Item>
					{
						new Item(351, 14, 1),
						new Item(35, 9, 1),
					}),
				new ShapelessRecipe(new Item(35, 1, 1),
					new List<Item>
					{
						new Item(351, 14, 1),
						new Item(35, 10, 1),
					}),
				new ShapelessRecipe(new Item(35, 1, 1),
					new List<Item>
					{
						new Item(351, 14, 1),
						new Item(35, 11, 1),
					}),
				new ShapelessRecipe(new Item(35, 1, 1),
					new List<Item>
					{
						new Item(351, 14, 1),
						new Item(35, 12, 1),
					}),
				new ShapelessRecipe(new Item(35, 1, 1),
					new List<Item>
					{
						new Item(351, 14, 1),
						new Item(35, 13, 1),
					}),
				new ShapelessRecipe(new Item(35, 1, 1),
					new List<Item>
					{
						new Item(351, 14, 1),
						new Item(35, 14, 1),
					}),
				new ShapelessRecipe(new Item(35, 1, 1),
					new List<Item>
					{
						new Item(351, 14, 1),
						new Item(35, 15, 1),
					}),
				new ShapelessRecipe(new Item(35, 2, 1),
					new List<Item>
					{
						new Item(351, 13, 1),
						new Item(35, 0, 1),
					}),
				new ShapelessRecipe(new Item(35, 2, 1),
					new List<Item>
					{
						new Item(351, 13, 1),
						new Item(35, 1, 1),
					}),
				new ShapelessRecipe(new Item(35, 2, 1),
					new List<Item>
					{
						new Item(351, 13, 1),
						new Item(35, 3, 1),
					}),
				new ShapelessRecipe(new Item(35, 2, 1),
					new List<Item>
					{
						new Item(351, 13, 1),
						new Item(35, 4, 1),
					}),
				new ShapelessRecipe(new Item(35, 2, 1),
					new List<Item>
					{
						new Item(351, 13, 1),
						new Item(35, 5, 1),
					}),
				new ShapelessRecipe(new Item(35, 2, 1),
					new List<Item>
					{
						new Item(351, 13, 1),
						new Item(35, 6, 1),
					}),
				new ShapelessRecipe(new Item(35, 2, 1),
					new List<Item>
					{
						new Item(351, 13, 1),
						new Item(35, 7, 1),
					}),
				new ShapelessRecipe(new Item(35, 2, 1),
					new List<Item>
					{
						new Item(351, 13, 1),
						new Item(35, 8, 1),
					}),
				new ShapelessRecipe(new Item(35, 2, 1),
					new List<Item>
					{
						new Item(351, 13, 1),
						new Item(35, 9, 1),
					}),
				new ShapelessRecipe(new Item(35, 2, 1),
					new List<Item>
					{
						new Item(351, 13, 1),
						new Item(35, 10, 1),
					}),
				new ShapelessRecipe(new Item(35, 2, 1),
					new List<Item>
					{
						new Item(351, 13, 1),
						new Item(35, 11, 1),
					}),
				new ShapelessRecipe(new Item(35, 2, 1),
					new List<Item>
					{
						new Item(351, 13, 1),
						new Item(35, 12, 1),
					}),
				new ShapelessRecipe(new Item(35, 2, 1),
					new List<Item>
					{
						new Item(351, 13, 1),
						new Item(35, 13, 1),
					}),
				new ShapelessRecipe(new Item(35, 2, 1),
					new List<Item>
					{
						new Item(351, 13, 1),
						new Item(35, 14, 1),
					}),
				new ShapelessRecipe(new Item(35, 2, 1),
					new List<Item>
					{
						new Item(351, 13, 1),
						new Item(35, 15, 1),
					}),
				new ShapelessRecipe(new Item(35, 3, 1),
					new List<Item>
					{
						new Item(351, 12, 1),
						new Item(35, 0, 1),
					}),
				new ShapelessRecipe(new Item(35, 3, 1),
					new List<Item>
					{
						new Item(351, 12, 1),
						new Item(35, 1, 1),
					}),
				new ShapelessRecipe(new Item(35, 3, 1),
					new List<Item>
					{
						new Item(351, 12, 1),
						new Item(35, 2, 1),
					}),
				new ShapelessRecipe(new Item(35, 3, 1),
					new List<Item>
					{
						new Item(351, 12, 1),
						new Item(35, 4, 1),
					}),
				new ShapelessRecipe(new Item(35, 3, 1),
					new List<Item>
					{
						new Item(351, 12, 1),
						new Item(35, 5, 1),
					}),
				new ShapelessRecipe(new Item(35, 3, 1),
					new List<Item>
					{
						new Item(351, 12, 1),
						new Item(35, 6, 1),
					}),
				new ShapelessRecipe(new Item(35, 3, 1),
					new List<Item>
					{
						new Item(351, 12, 1),
						new Item(35, 7, 1),
					}),
				new ShapelessRecipe(new Item(35, 3, 1),
					new List<Item>
					{
						new Item(351, 12, 1),
						new Item(35, 8, 1),
					}),
				new ShapelessRecipe(new Item(35, 3, 1),
					new List<Item>
					{
						new Item(351, 12, 1),
						new Item(35, 9, 1),
					}),
				new ShapelessRecipe(new Item(35, 3, 1),
					new List<Item>
					{
						new Item(351, 12, 1),
						new Item(35, 10, 1),
					}),
				new ShapelessRecipe(new Item(35, 3, 1),
					new List<Item>
					{
						new Item(351, 12, 1),
						new Item(35, 11, 1),
					}),
				new ShapelessRecipe(new Item(35, 3, 1),
					new List<Item>
					{
						new Item(351, 12, 1),
						new Item(35, 12, 1),
					}),
				new ShapelessRecipe(new Item(35, 3, 1),
					new List<Item>
					{
						new Item(351, 12, 1),
						new Item(35, 13, 1),
					}),
				new ShapelessRecipe(new Item(35, 3, 1),
					new List<Item>
					{
						new Item(351, 12, 1),
						new Item(35, 14, 1),
					}),
				new ShapelessRecipe(new Item(35, 3, 1),
					new List<Item>
					{
						new Item(351, 12, 1),
						new Item(35, 15, 1),
					}),
				new ShapelessRecipe(new Item(35, 4, 1),
					new List<Item>
					{
						new Item(351, 11, 1),
						new Item(35, 0, 1),
					}),
				new ShapelessRecipe(new Item(35, 4, 1),
					new List<Item>
					{
						new Item(351, 11, 1),
						new Item(35, 1, 1),
					}),
				new ShapelessRecipe(new Item(35, 4, 1),
					new List<Item>
					{
						new Item(351, 11, 1),
						new Item(35, 2, 1),
					}),
				new ShapelessRecipe(new Item(35, 4, 1),
					new List<Item>
					{
						new Item(351, 11, 1),
						new Item(35, 3, 1),
					}),
				new ShapelessRecipe(new Item(35, 4, 1),
					new List<Item>
					{
						new Item(351, 11, 1),
						new Item(35, 5, 1),
					}),
				new ShapelessRecipe(new Item(35, 4, 1),
					new List<Item>
					{
						new Item(351, 11, 1),
						new Item(35, 6, 1),
					}),
				new ShapelessRecipe(new Item(35, 4, 1),
					new List<Item>
					{
						new Item(351, 11, 1),
						new Item(35, 7, 1),
					}),
				new ShapelessRecipe(new Item(35, 4, 1),
					new List<Item>
					{
						new Item(351, 11, 1),
						new Item(35, 8, 1),
					}),
				new ShapelessRecipe(new Item(35, 4, 1),
					new List<Item>
					{
						new Item(351, 11, 1),
						new Item(35, 9, 1),
					}),
				new ShapelessRecipe(new Item(35, 4, 1),
					new List<Item>
					{
						new Item(351, 11, 1),
						new Item(35, 10, 1),
					}),
				new ShapelessRecipe(new Item(35, 4, 1),
					new List<Item>
					{
						new Item(351, 11, 1),
						new Item(35, 11, 1),
					}),
				new ShapelessRecipe(new Item(35, 4, 1),
					new List<Item>
					{
						new Item(351, 11, 1),
						new Item(35, 12, 1),
					}),
				new ShapelessRecipe(new Item(35, 4, 1),
					new List<Item>
					{
						new Item(351, 11, 1),
						new Item(35, 13, 1),
					}),
				new ShapelessRecipe(new Item(35, 4, 1),
					new List<Item>
					{
						new Item(351, 11, 1),
						new Item(35, 14, 1),
					}),
				new ShapelessRecipe(new Item(35, 4, 1),
					new List<Item>
					{
						new Item(351, 11, 1),
						new Item(35, 15, 1),
					}),
				new ShapelessRecipe(new Item(35, 5, 1),
					new List<Item>
					{
						new Item(351, 10, 1),
						new Item(35, 0, 1),
					}),
				new ShapelessRecipe(new Item(35, 5, 1),
					new List<Item>
					{
						new Item(351, 10, 1),
						new Item(35, 1, 1),
					}),
				new ShapelessRecipe(new Item(35, 5, 1),
					new List<Item>
					{
						new Item(351, 10, 1),
						new Item(35, 2, 1),
					}),
				new ShapelessRecipe(new Item(35, 5, 1),
					new List<Item>
					{
						new Item(351, 10, 1),
						new Item(35, 3, 1),
					}),
				new ShapelessRecipe(new Item(35, 5, 1),
					new List<Item>
					{
						new Item(351, 10, 1),
						new Item(35, 4, 1),
					}),
				new ShapelessRecipe(new Item(35, 5, 1),
					new List<Item>
					{
						new Item(351, 10, 1),
						new Item(35, 6, 1),
					}),
				new ShapelessRecipe(new Item(35, 5, 1),
					new List<Item>
					{
						new Item(351, 10, 1),
						new Item(35, 7, 1),
					}),
				new ShapelessRecipe(new Item(35, 5, 1),
					new List<Item>
					{
						new Item(351, 10, 1),
						new Item(35, 8, 1),
					}),
				new ShapelessRecipe(new Item(35, 5, 1),
					new List<Item>
					{
						new Item(351, 10, 1),
						new Item(35, 9, 1),
					}),
				new ShapelessRecipe(new Item(35, 5, 1),
					new List<Item>
					{
						new Item(351, 10, 1),
						new Item(35, 10, 1),
					}),
				new ShapelessRecipe(new Item(35, 5, 1),
					new List<Item>
					{
						new Item(351, 10, 1),
						new Item(35, 11, 1),
					}),
				new ShapelessRecipe(new Item(35, 5, 1),
					new List<Item>
					{
						new Item(351, 10, 1),
						new Item(35, 12, 1),
					}),
				new ShapelessRecipe(new Item(35, 5, 1),
					new List<Item>
					{
						new Item(351, 10, 1),
						new Item(35, 13, 1),
					}),
				new ShapelessRecipe(new Item(35, 5, 1),
					new List<Item>
					{
						new Item(351, 10, 1),
						new Item(35, 14, 1),
					}),
				new ShapelessRecipe(new Item(35, 5, 1),
					new List<Item>
					{
						new Item(351, 10, 1),
						new Item(35, 15, 1),
					}),
				new ShapelessRecipe(new Item(35, 6, 1),
					new List<Item>
					{
						new Item(351, 9, 1),
						new Item(35, 0, 1),
					}),
				new ShapelessRecipe(new Item(35, 6, 1),
					new List<Item>
					{
						new Item(351, 9, 1),
						new Item(35, 1, 1),
					}),
				new ShapelessRecipe(new Item(35, 6, 1),
					new List<Item>
					{
						new Item(351, 9, 1),
						new Item(35, 2, 1),
					}),
				new ShapelessRecipe(new Item(35, 6, 1),
					new List<Item>
					{
						new Item(351, 9, 1),
						new Item(35, 3, 1),
					}),
				new ShapelessRecipe(new Item(35, 6, 1),
					new List<Item>
					{
						new Item(351, 9, 1),
						new Item(35, 4, 1),
					}),
				new ShapelessRecipe(new Item(35, 6, 1),
					new List<Item>
					{
						new Item(351, 9, 1),
						new Item(35, 5, 1),
					}),
				new ShapelessRecipe(new Item(35, 6, 1),
					new List<Item>
					{
						new Item(351, 9, 1),
						new Item(35, 7, 1),
					}),
				new ShapelessRecipe(new Item(35, 6, 1),
					new List<Item>
					{
						new Item(351, 9, 1),
						new Item(35, 8, 1),
					}),
				new ShapelessRecipe(new Item(35, 6, 1),
					new List<Item>
					{
						new Item(351, 9, 1),
						new Item(35, 9, 1),
					}),
				new ShapelessRecipe(new Item(35, 6, 1),
					new List<Item>
					{
						new Item(351, 9, 1),
						new Item(35, 10, 1),
					}),
				new ShapelessRecipe(new Item(35, 6, 1),
					new List<Item>
					{
						new Item(351, 9, 1),
						new Item(35, 11, 1),
					}),
				new ShapelessRecipe(new Item(35, 6, 1),
					new List<Item>
					{
						new Item(351, 9, 1),
						new Item(35, 12, 1),
					}),
				new ShapelessRecipe(new Item(35, 6, 1),
					new List<Item>
					{
						new Item(351, 9, 1),
						new Item(35, 13, 1),
					}),
				new ShapelessRecipe(new Item(35, 6, 1),
					new List<Item>
					{
						new Item(351, 9, 1),
						new Item(35, 14, 1),
					}),
				new ShapelessRecipe(new Item(35, 6, 1),
					new List<Item>
					{
						new Item(351, 9, 1),
						new Item(35, 15, 1),
					}),
				new ShapelessRecipe(new Item(35, 7, 1),
					new List<Item>
					{
						new Item(351, 8, 1),
						new Item(35, 0, 1),
					}),
				new ShapelessRecipe(new Item(35, 7, 1),
					new List<Item>
					{
						new Item(351, 8, 1),
						new Item(35, 1, 1),
					}),
				new ShapelessRecipe(new Item(35, 7, 1),
					new List<Item>
					{
						new Item(351, 8, 1),
						new Item(35, 2, 1),
					}),
				new ShapelessRecipe(new Item(35, 7, 1),
					new List<Item>
					{
						new Item(351, 8, 1),
						new Item(35, 3, 1),
					}),
				new ShapelessRecipe(new Item(35, 7, 1),
					new List<Item>
					{
						new Item(351, 8, 1),
						new Item(35, 4, 1),
					}),
				new ShapelessRecipe(new Item(35, 7, 1),
					new List<Item>
					{
						new Item(351, 8, 1),
						new Item(35, 5, 1),
					}),
				new ShapelessRecipe(new Item(35, 7, 1),
					new List<Item>
					{
						new Item(351, 8, 1),
						new Item(35, 6, 1),
					}),
				new ShapelessRecipe(new Item(35, 7, 1),
					new List<Item>
					{
						new Item(351, 8, 1),
						new Item(35, 8, 1),
					}),
				new ShapelessRecipe(new Item(35, 7, 1),
					new List<Item>
					{
						new Item(351, 8, 1),
						new Item(35, 9, 1),
					}),
				new ShapelessRecipe(new Item(35, 7, 1),
					new List<Item>
					{
						new Item(351, 8, 1),
						new Item(35, 10, 1),
					}),
				new ShapelessRecipe(new Item(35, 7, 1),
					new List<Item>
					{
						new Item(351, 8, 1),
						new Item(35, 11, 1),
					}),
				new ShapelessRecipe(new Item(35, 7, 1),
					new List<Item>
					{
						new Item(351, 8, 1),
						new Item(35, 12, 1),
					}),
				new ShapelessRecipe(new Item(35, 7, 1),
					new List<Item>
					{
						new Item(351, 8, 1),
						new Item(35, 13, 1),
					}),
				new ShapelessRecipe(new Item(35, 7, 1),
					new List<Item>
					{
						new Item(351, 8, 1),
						new Item(35, 14, 1),
					}),
				new ShapelessRecipe(new Item(35, 7, 1),
					new List<Item>
					{
						new Item(351, 8, 1),
						new Item(35, 15, 1),
					}),
				new ShapelessRecipe(new Item(35, 8, 1),
					new List<Item>
					{
						new Item(351, 7, 1),
						new Item(35, 0, 1),
					}),
				new ShapelessRecipe(new Item(35, 8, 1),
					new List<Item>
					{
						new Item(351, 7, 1),
						new Item(35, 1, 1),
					}),
				new ShapelessRecipe(new Item(35, 8, 1),
					new List<Item>
					{
						new Item(351, 7, 1),
						new Item(35, 2, 1),
					}),
				new ShapelessRecipe(new Item(35, 8, 1),
					new List<Item>
					{
						new Item(351, 7, 1),
						new Item(35, 3, 1),
					}),
				new ShapelessRecipe(new Item(35, 8, 1),
					new List<Item>
					{
						new Item(351, 7, 1),
						new Item(35, 4, 1),
					}),
				new ShapelessRecipe(new Item(35, 8, 1),
					new List<Item>
					{
						new Item(351, 7, 1),
						new Item(35, 5, 1),
					}),
				new ShapelessRecipe(new Item(35, 8, 1),
					new List<Item>
					{
						new Item(351, 7, 1),
						new Item(35, 6, 1),
					}),
				new ShapelessRecipe(new Item(35, 8, 1),
					new List<Item>
					{
						new Item(351, 7, 1),
						new Item(35, 7, 1),
					}),
				new ShapelessRecipe(new Item(35, 8, 1),
					new List<Item>
					{
						new Item(351, 7, 1),
						new Item(35, 9, 1),
					}),
				new ShapelessRecipe(new Item(35, 8, 1),
					new List<Item>
					{
						new Item(351, 7, 1),
						new Item(35, 10, 1),
					}),
				new ShapelessRecipe(new Item(35, 8, 1),
					new List<Item>
					{
						new Item(351, 7, 1),
						new Item(35, 11, 1),
					}),
				new ShapelessRecipe(new Item(35, 8, 1),
					new List<Item>
					{
						new Item(351, 7, 1),
						new Item(35, 12, 1),
					}),
				new ShapelessRecipe(new Item(35, 8, 1),
					new List<Item>
					{
						new Item(351, 7, 1),
						new Item(35, 13, 1),
					}),
				new ShapelessRecipe(new Item(35, 8, 1),
					new List<Item>
					{
						new Item(351, 7, 1),
						new Item(35, 14, 1),
					}),
				new ShapelessRecipe(new Item(35, 8, 1),
					new List<Item>
					{
						new Item(351, 7, 1),
						new Item(35, 15, 1),
					}),
				new ShapelessRecipe(new Item(35, 9, 1),
					new List<Item>
					{
						new Item(351, 6, 1),
						new Item(35, 0, 1),
					}),
				new ShapelessRecipe(new Item(35, 9, 1),
					new List<Item>
					{
						new Item(351, 6, 1),
						new Item(35, 1, 1),
					}),
				new ShapelessRecipe(new Item(35, 9, 1),
					new List<Item>
					{
						new Item(351, 6, 1),
						new Item(35, 2, 1),
					}),
				new ShapelessRecipe(new Item(35, 9, 1),
					new List<Item>
					{
						new Item(351, 6, 1),
						new Item(35, 3, 1),
					}),
				new ShapelessRecipe(new Item(35, 9, 1),
					new List<Item>
					{
						new Item(351, 6, 1),
						new Item(35, 4, 1),
					}),
				new ShapelessRecipe(new Item(35, 9, 1),
					new List<Item>
					{
						new Item(351, 6, 1),
						new Item(35, 5, 1),
					}),
				new ShapelessRecipe(new Item(35, 9, 1),
					new List<Item>
					{
						new Item(351, 6, 1),
						new Item(35, 6, 1),
					}),
				new ShapelessRecipe(new Item(35, 9, 1),
					new List<Item>
					{
						new Item(351, 6, 1),
						new Item(35, 7, 1),
					}),
				new ShapelessRecipe(new Item(35, 9, 1),
					new List<Item>
					{
						new Item(351, 6, 1),
						new Item(35, 8, 1),
					}),
				new ShapelessRecipe(new Item(35, 9, 1),
					new List<Item>
					{
						new Item(351, 6, 1),
						new Item(35, 10, 1),
					}),
				new ShapelessRecipe(new Item(35, 9, 1),
					new List<Item>
					{
						new Item(351, 6, 1),
						new Item(35, 11, 1),
					}),
				new ShapelessRecipe(new Item(35, 9, 1),
					new List<Item>
					{
						new Item(351, 6, 1),
						new Item(35, 12, 1),
					}),
				new ShapelessRecipe(new Item(35, 9, 1),
					new List<Item>
					{
						new Item(351, 6, 1),
						new Item(35, 13, 1),
					}),
				new ShapelessRecipe(new Item(35, 9, 1),
					new List<Item>
					{
						new Item(351, 6, 1),
						new Item(35, 14, 1),
					}),
				new ShapelessRecipe(new Item(35, 9, 1),
					new List<Item>
					{
						new Item(351, 6, 1),
						new Item(35, 15, 1),
					}),
				new ShapelessRecipe(new Item(35, 10, 1),
					new List<Item>
					{
						new Item(351, 5, 1),
						new Item(35, 0, 1),
					}),
				new ShapelessRecipe(new Item(35, 10, 1),
					new List<Item>
					{
						new Item(351, 5, 1),
						new Item(35, 1, 1),
					}),
				new ShapelessRecipe(new Item(35, 10, 1),
					new List<Item>
					{
						new Item(351, 5, 1),
						new Item(35, 2, 1),
					}),
				new ShapelessRecipe(new Item(35, 10, 1),
					new List<Item>
					{
						new Item(351, 5, 1),
						new Item(35, 3, 1),
					}),
				new ShapelessRecipe(new Item(35, 10, 1),
					new List<Item>
					{
						new Item(351, 5, 1),
						new Item(35, 4, 1),
					}),
				new ShapelessRecipe(new Item(35, 10, 1),
					new List<Item>
					{
						new Item(351, 5, 1),
						new Item(35, 5, 1),
					}),
				new ShapelessRecipe(new Item(35, 10, 1),
					new List<Item>
					{
						new Item(351, 5, 1),
						new Item(35, 6, 1),
					}),
				new ShapelessRecipe(new Item(35, 10, 1),
					new List<Item>
					{
						new Item(351, 5, 1),
						new Item(35, 7, 1),
					}),
				new ShapelessRecipe(new Item(35, 10, 1),
					new List<Item>
					{
						new Item(351, 5, 1),
						new Item(35, 8, 1),
					}),
				new ShapelessRecipe(new Item(35, 10, 1),
					new List<Item>
					{
						new Item(351, 5, 1),
						new Item(35, 9, 1),
					}),
				new ShapelessRecipe(new Item(35, 10, 1),
					new List<Item>
					{
						new Item(351, 5, 1),
						new Item(35, 11, 1),
					}),
				new ShapelessRecipe(new Item(35, 10, 1),
					new List<Item>
					{
						new Item(351, 5, 1),
						new Item(35, 12, 1),
					}),
				new ShapelessRecipe(new Item(35, 10, 1),
					new List<Item>
					{
						new Item(351, 5, 1),
						new Item(35, 13, 1),
					}),
				new ShapelessRecipe(new Item(35, 10, 1),
					new List<Item>
					{
						new Item(351, 5, 1),
						new Item(35, 14, 1),
					}),
				new ShapelessRecipe(new Item(35, 10, 1),
					new List<Item>
					{
						new Item(351, 5, 1),
						new Item(35, 15, 1),
					}),
				new ShapelessRecipe(new Item(35, 11, 1),
					new List<Item>
					{
						new Item(351, 4, 1),
						new Item(35, 0, 1),
					}),
				new ShapelessRecipe(new Item(35, 11, 1),
					new List<Item>
					{
						new Item(351, 4, 1),
						new Item(35, 1, 1),
					}),
				new ShapelessRecipe(new Item(35, 11, 1),
					new List<Item>
					{
						new Item(351, 4, 1),
						new Item(35, 2, 1),
					}),
				new ShapelessRecipe(new Item(35, 11, 1),
					new List<Item>
					{
						new Item(351, 4, 1),
						new Item(35, 3, 1),
					}),
				new ShapelessRecipe(new Item(35, 11, 1),
					new List<Item>
					{
						new Item(351, 4, 1),
						new Item(35, 4, 1),
					}),
				new ShapelessRecipe(new Item(35, 11, 1),
					new List<Item>
					{
						new Item(351, 4, 1),
						new Item(35, 5, 1),
					}),
				new ShapelessRecipe(new Item(35, 11, 1),
					new List<Item>
					{
						new Item(351, 4, 1),
						new Item(35, 6, 1),
					}),
				new ShapelessRecipe(new Item(35, 11, 1),
					new List<Item>
					{
						new Item(351, 4, 1),
						new Item(35, 7, 1),
					}),
				new ShapelessRecipe(new Item(35, 11, 1),
					new List<Item>
					{
						new Item(351, 4, 1),
						new Item(35, 8, 1),
					}),
				new ShapelessRecipe(new Item(35, 11, 1),
					new List<Item>
					{
						new Item(351, 4, 1),
						new Item(35, 9, 1),
					}),
				new ShapelessRecipe(new Item(35, 11, 1),
					new List<Item>
					{
						new Item(351, 4, 1),
						new Item(35, 10, 1),
					}),
				new ShapelessRecipe(new Item(35, 11, 1),
					new List<Item>
					{
						new Item(351, 4, 1),
						new Item(35, 12, 1),
					}),
				new ShapelessRecipe(new Item(35, 11, 1),
					new List<Item>
					{
						new Item(351, 4, 1),
						new Item(35, 13, 1),
					}),
				new ShapelessRecipe(new Item(35, 11, 1),
					new List<Item>
					{
						new Item(351, 4, 1),
						new Item(35, 14, 1),
					}),
				new ShapelessRecipe(new Item(35, 11, 1),
					new List<Item>
					{
						new Item(351, 4, 1),
						new Item(35, 15, 1),
					}),
				new ShapelessRecipe(new Item(35, 12, 1),
					new List<Item>
					{
						new Item(351, 3, 1),
						new Item(35, 0, 1),
					}),
				new ShapelessRecipe(new Item(35, 12, 1),
					new List<Item>
					{
						new Item(351, 3, 1),
						new Item(35, 1, 1),
					}),
				new ShapelessRecipe(new Item(35, 12, 1),
					new List<Item>
					{
						new Item(351, 3, 1),
						new Item(35, 2, 1),
					}),
				new ShapelessRecipe(new Item(35, 12, 1),
					new List<Item>
					{
						new Item(351, 3, 1),
						new Item(35, 3, 1),
					}),
				new ShapelessRecipe(new Item(35, 12, 1),
					new List<Item>
					{
						new Item(351, 3, 1),
						new Item(35, 4, 1),
					}),
				new ShapelessRecipe(new Item(35, 12, 1),
					new List<Item>
					{
						new Item(351, 3, 1),
						new Item(35, 5, 1),
					}),
				new ShapelessRecipe(new Item(35, 12, 1),
					new List<Item>
					{
						new Item(351, 3, 1),
						new Item(35, 6, 1),
					}),
				new ShapelessRecipe(new Item(35, 12, 1),
					new List<Item>
					{
						new Item(351, 3, 1),
						new Item(35, 7, 1),
					}),
				new ShapelessRecipe(new Item(35, 12, 1),
					new List<Item>
					{
						new Item(351, 3, 1),
						new Item(35, 8, 1),
					}),
				new ShapelessRecipe(new Item(35, 12, 1),
					new List<Item>
					{
						new Item(351, 3, 1),
						new Item(35, 9, 1),
					}),
				new ShapelessRecipe(new Item(35, 12, 1),
					new List<Item>
					{
						new Item(351, 3, 1),
						new Item(35, 10, 1),
					}),
				new ShapelessRecipe(new Item(35, 12, 1),
					new List<Item>
					{
						new Item(351, 3, 1),
						new Item(35, 11, 1),
					}),
				new ShapelessRecipe(new Item(35, 12, 1),
					new List<Item>
					{
						new Item(351, 3, 1),
						new Item(35, 13, 1),
					}),
				new ShapelessRecipe(new Item(35, 12, 1),
					new List<Item>
					{
						new Item(351, 3, 1),
						new Item(35, 14, 1),
					}),
				new ShapelessRecipe(new Item(35, 12, 1),
					new List<Item>
					{
						new Item(351, 3, 1),
						new Item(35, 15, 1),
					}),
				new ShapelessRecipe(new Item(35, 13, 1),
					new List<Item>
					{
						new Item(351, 2, 1),
						new Item(35, 0, 1),
					}),
				new ShapelessRecipe(new Item(35, 13, 1),
					new List<Item>
					{
						new Item(351, 2, 1),
						new Item(35, 1, 1),
					}),
				new ShapelessRecipe(new Item(35, 13, 1),
					new List<Item>
					{
						new Item(351, 2, 1),
						new Item(35, 2, 1),
					}),
				new ShapelessRecipe(new Item(35, 13, 1),
					new List<Item>
					{
						new Item(351, 2, 1),
						new Item(35, 3, 1),
					}),
				new ShapelessRecipe(new Item(35, 13, 1),
					new List<Item>
					{
						new Item(351, 2, 1),
						new Item(35, 4, 1),
					}),
				new ShapelessRecipe(new Item(35, 13, 1),
					new List<Item>
					{
						new Item(351, 2, 1),
						new Item(35, 5, 1),
					}),
				new ShapelessRecipe(new Item(35, 13, 1),
					new List<Item>
					{
						new Item(351, 2, 1),
						new Item(35, 6, 1),
					}),
				new ShapelessRecipe(new Item(35, 13, 1),
					new List<Item>
					{
						new Item(351, 2, 1),
						new Item(35, 7, 1),
					}),
				new ShapelessRecipe(new Item(35, 13, 1),
					new List<Item>
					{
						new Item(351, 2, 1),
						new Item(35, 8, 1),
					}),
				new ShapelessRecipe(new Item(35, 13, 1),
					new List<Item>
					{
						new Item(351, 2, 1),
						new Item(35, 9, 1),
					}),
				new ShapelessRecipe(new Item(35, 13, 1),
					new List<Item>
					{
						new Item(351, 2, 1),
						new Item(35, 10, 1),
					}),
				new ShapelessRecipe(new Item(35, 13, 1),
					new List<Item>
					{
						new Item(351, 2, 1),
						new Item(35, 11, 1),
					}),
				new ShapelessRecipe(new Item(35, 13, 1),
					new List<Item>
					{
						new Item(351, 2, 1),
						new Item(35, 12, 1),
					}),
				new ShapelessRecipe(new Item(35, 13, 1),
					new List<Item>
					{
						new Item(351, 2, 1),
						new Item(35, 14, 1),
					}),
				new ShapelessRecipe(new Item(35, 13, 1),
					new List<Item>
					{
						new Item(351, 2, 1),
						new Item(35, 15, 1),
					}),
				new ShapelessRecipe(new Item(35, 14, 1),
					new List<Item>
					{
						new Item(351, 1, 1),
						new Item(35, 0, 1),
					}),
				new ShapelessRecipe(new Item(35, 14, 1),
					new List<Item>
					{
						new Item(351, 1, 1),
						new Item(35, 1, 1),
					}),
				new ShapelessRecipe(new Item(35, 14, 1),
					new List<Item>
					{
						new Item(351, 1, 1),
						new Item(35, 2, 1),
					}),
				new ShapelessRecipe(new Item(35, 14, 1),
					new List<Item>
					{
						new Item(351, 1, 1),
						new Item(35, 3, 1),
					}),
				new ShapelessRecipe(new Item(35, 14, 1),
					new List<Item>
					{
						new Item(351, 1, 1),
						new Item(35, 4, 1),
					}),
				new ShapelessRecipe(new Item(35, 14, 1),
					new List<Item>
					{
						new Item(351, 1, 1),
						new Item(35, 5, 1),
					}),
				new ShapelessRecipe(new Item(35, 14, 1),
					new List<Item>
					{
						new Item(351, 1, 1),
						new Item(35, 6, 1),
					}),
				new ShapelessRecipe(new Item(35, 14, 1),
					new List<Item>
					{
						new Item(351, 1, 1),
						new Item(35, 7, 1),
					}),
				new ShapelessRecipe(new Item(35, 14, 1),
					new List<Item>
					{
						new Item(351, 1, 1),
						new Item(35, 8, 1),
					}),
				new ShapelessRecipe(new Item(35, 14, 1),
					new List<Item>
					{
						new Item(351, 1, 1),
						new Item(35, 9, 1),
					}),
				new ShapelessRecipe(new Item(35, 14, 1),
					new List<Item>
					{
						new Item(351, 1, 1),
						new Item(35, 10, 1),
					}),
				new ShapelessRecipe(new Item(35, 14, 1),
					new List<Item>
					{
						new Item(351, 1, 1),
						new Item(35, 11, 1),
					}),
				new ShapelessRecipe(new Item(35, 14, 1),
					new List<Item>
					{
						new Item(351, 1, 1),
						new Item(35, 12, 1),
					}),
				new ShapelessRecipe(new Item(35, 14, 1),
					new List<Item>
					{
						new Item(351, 1, 1),
						new Item(35, 13, 1),
					}),
				new ShapelessRecipe(new Item(35, 14, 1),
					new List<Item>
					{
						new Item(351, 1, 1),
						new Item(35, 15, 1),
					}),
				new ShapelessRecipe(new Item(35, 15, 1),
					new List<Item>
					{
						new Item(351, 0, 1),
						new Item(35, 0, 1),
					}),
				new ShapelessRecipe(new Item(35, 15, 1),
					new List<Item>
					{
						new Item(351, 0, 1),
						new Item(35, 1, 1),
					}),
				new ShapelessRecipe(new Item(35, 15, 1),
					new List<Item>
					{
						new Item(351, 0, 1),
						new Item(35, 2, 1),
					}),
				new ShapelessRecipe(new Item(35, 15, 1),
					new List<Item>
					{
						new Item(351, 0, 1),
						new Item(35, 3, 1),
					}),
				new ShapelessRecipe(new Item(35, 15, 1),
					new List<Item>
					{
						new Item(351, 0, 1),
						new Item(35, 4, 1),
					}),
				new ShapelessRecipe(new Item(35, 15, 1),
					new List<Item>
					{
						new Item(351, 0, 1),
						new Item(35, 5, 1),
					}),
				new ShapelessRecipe(new Item(35, 15, 1),
					new List<Item>
					{
						new Item(351, 0, 1),
						new Item(35, 6, 1),
					}),
				new ShapelessRecipe(new Item(35, 15, 1),
					new List<Item>
					{
						new Item(351, 0, 1),
						new Item(35, 7, 1),
					}),
				new ShapelessRecipe(new Item(35, 15, 1),
					new List<Item>
					{
						new Item(351, 0, 1),
						new Item(35, 8, 1),
					}),
				new ShapelessRecipe(new Item(35, 15, 1),
					new List<Item>
					{
						new Item(351, 0, 1),
						new Item(35, 9, 1),
					}),
				new ShapelessRecipe(new Item(35, 15, 1),
					new List<Item>
					{
						new Item(351, 0, 1),
						new Item(35, 10, 1),
					}),
				new ShapelessRecipe(new Item(35, 15, 1),
					new List<Item>
					{
						new Item(351, 0, 1),
						new Item(35, 11, 1),
					}),
				new ShapelessRecipe(new Item(35, 15, 1),
					new List<Item>
					{
						new Item(351, 0, 1),
						new Item(35, 12, 1),
					}),
				new ShapelessRecipe(new Item(35, 15, 1),
					new List<Item>
					{
						new Item(351, 0, 1),
						new Item(35, 13, 1),
					}),
				new ShapelessRecipe(new Item(35, 15, 1),
					new List<Item>
					{
						new Item(351, 0, 1),
						new Item(35, 14, 1),
					}),
				new ShapelessRecipe(new Item(218, 0, 1),
					new List<Item>
					{
						new Item(205, -1, 1),
						new Item(351, 19, 1),
					}),
				new ShapelessRecipe(new Item(218, 0, 1),
					new List<Item>
					{
						new Item(218, 15, 1),
						new Item(351, 19, 1),
					}),
				new ShapelessRecipe(new Item(218, 0, 1),
					new List<Item>
					{
						new Item(218, 14, 1),
						new Item(351, 19, 1),
					}),
				new ShapelessRecipe(new Item(218, 0, 1),
					new List<Item>
					{
						new Item(218, 13, 1),
						new Item(351, 19, 1),
					}),
				new ShapelessRecipe(new Item(218, 0, 1),
					new List<Item>
					{
						new Item(218, 12, 1),
						new Item(351, 19, 1),
					}),
				new ShapelessRecipe(new Item(218, 0, 1),
					new List<Item>
					{
						new Item(218, 11, 1),
						new Item(351, 19, 1),
					}),
				new ShapelessRecipe(new Item(218, 0, 1),
					new List<Item>
					{
						new Item(218, 10, 1),
						new Item(351, 19, 1),
					}),
				new ShapelessRecipe(new Item(218, 0, 1),
					new List<Item>
					{
						new Item(218, 9, 1),
						new Item(351, 19, 1),
					}),
				new ShapelessRecipe(new Item(218, 0, 1),
					new List<Item>
					{
						new Item(218, 8, 1),
						new Item(351, 19, 1),
					}),
				new ShapelessRecipe(new Item(218, 0, 1),
					new List<Item>
					{
						new Item(218, 7, 1),
						new Item(351, 19, 1),
					}),
				new ShapelessRecipe(new Item(218, 0, 1),
					new List<Item>
					{
						new Item(218, 6, 1),
						new Item(351, 19, 1),
					}),
				new ShapelessRecipe(new Item(218, 0, 1),
					new List<Item>
					{
						new Item(218, 5, 1),
						new Item(351, 19, 1),
					}),
				new ShapelessRecipe(new Item(218, 0, 1),
					new List<Item>
					{
						new Item(218, 4, 1),
						new Item(351, 19, 1),
					}),
				new ShapelessRecipe(new Item(218, 0, 1),
					new List<Item>
					{
						new Item(218, 3, 1),
						new Item(351, 19, 1),
					}),
				new ShapelessRecipe(new Item(218, 0, 1),
					new List<Item>
					{
						new Item(218, 2, 1),
						new Item(351, 19, 1),
					}),
				new ShapelessRecipe(new Item(218, 0, 1),
					new List<Item>
					{
						new Item(218, 1, 1),
						new Item(351, 19, 1),
					}),
				new ShapelessRecipe(new Item(218, 11, 1),
					new List<Item>
					{
						new Item(205, -1, 1),
						new Item(351, 18, 1),
					}),
				new ShapelessRecipe(new Item(218, 11, 1),
					new List<Item>
					{
						new Item(218, 15, 1),
						new Item(351, 18, 1),
					}),
				new ShapelessRecipe(new Item(218, 11, 1),
					new List<Item>
					{
						new Item(218, 14, 1),
						new Item(351, 18, 1),
					}),
				new ShapelessRecipe(new Item(218, 11, 1),
					new List<Item>
					{
						new Item(218, 13, 1),
						new Item(351, 18, 1),
					}),
				new ShapelessRecipe(new Item(218, 11, 1),
					new List<Item>
					{
						new Item(218, 12, 1),
						new Item(351, 18, 1),
					}),
				new ShapelessRecipe(new Item(218, 11, 1),
					new List<Item>
					{
						new Item(218, 10, 1),
						new Item(351, 18, 1),
					}),
				new ShapelessRecipe(new Item(218, 11, 1),
					new List<Item>
					{
						new Item(218, 9, 1),
						new Item(351, 18, 1),
					}),
				new ShapelessRecipe(new Item(218, 11, 1),
					new List<Item>
					{
						new Item(218, 8, 1),
						new Item(351, 18, 1),
					}),
				new ShapelessRecipe(new Item(218, 11, 1),
					new List<Item>
					{
						new Item(218, 7, 1),
						new Item(351, 18, 1),
					}),
				new ShapelessRecipe(new Item(218, 11, 1),
					new List<Item>
					{
						new Item(218, 6, 1),
						new Item(351, 18, 1),
					}),
				new ShapelessRecipe(new Item(218, 11, 1),
					new List<Item>
					{
						new Item(218, 5, 1),
						new Item(351, 18, 1),
					}),
				new ShapelessRecipe(new Item(218, 11, 1),
					new List<Item>
					{
						new Item(218, 4, 1),
						new Item(351, 18, 1),
					}),
				new ShapelessRecipe(new Item(218, 11, 1),
					new List<Item>
					{
						new Item(218, 3, 1),
						new Item(351, 18, 1),
					}),
				new ShapelessRecipe(new Item(218, 11, 1),
					new List<Item>
					{
						new Item(218, 2, 1),
						new Item(351, 18, 1),
					}),
				new ShapelessRecipe(new Item(218, 11, 1),
					new List<Item>
					{
						new Item(218, 1, 1),
						new Item(351, 18, 1),
					}),
				new ShapelessRecipe(new Item(218, 11, 1),
					new List<Item>
					{
						new Item(218, 0, 1),
						new Item(351, 18, 1),
					}),
				new ShapelessRecipe(new Item(218, 12, 1),
					new List<Item>
					{
						new Item(205, -1, 1),
						new Item(351, 17, 1),
					}),
				new ShapelessRecipe(new Item(218, 12, 1),
					new List<Item>
					{
						new Item(218, 15, 1),
						new Item(351, 17, 1),
					}),
				new ShapelessRecipe(new Item(218, 12, 1),
					new List<Item>
					{
						new Item(218, 14, 1),
						new Item(351, 17, 1),
					}),
				new ShapelessRecipe(new Item(218, 12, 1),
					new List<Item>
					{
						new Item(218, 13, 1),
						new Item(351, 17, 1),
					}),
				new ShapelessRecipe(new Item(218, 12, 1),
					new List<Item>
					{
						new Item(218, 11, 1),
						new Item(351, 17, 1),
					}),
				new ShapelessRecipe(new Item(218, 12, 1),
					new List<Item>
					{
						new Item(218, 10, 1),
						new Item(351, 17, 1),
					}),
				new ShapelessRecipe(new Item(218, 12, 1),
					new List<Item>
					{
						new Item(218, 9, 1),
						new Item(351, 17, 1),
					}),
				new ShapelessRecipe(new Item(218, 12, 1),
					new List<Item>
					{
						new Item(218, 8, 1),
						new Item(351, 17, 1),
					}),
				new ShapelessRecipe(new Item(218, 12, 1),
					new List<Item>
					{
						new Item(218, 7, 1),
						new Item(351, 17, 1),
					}),
				new ShapelessRecipe(new Item(218, 12, 1),
					new List<Item>
					{
						new Item(218, 6, 1),
						new Item(351, 17, 1),
					}),
				new ShapelessRecipe(new Item(218, 12, 1),
					new List<Item>
					{
						new Item(218, 5, 1),
						new Item(351, 17, 1),
					}),
				new ShapelessRecipe(new Item(218, 12, 1),
					new List<Item>
					{
						new Item(218, 4, 1),
						new Item(351, 17, 1),
					}),
				new ShapelessRecipe(new Item(218, 12, 1),
					new List<Item>
					{
						new Item(218, 3, 1),
						new Item(351, 17, 1),
					}),
				new ShapelessRecipe(new Item(218, 12, 1),
					new List<Item>
					{
						new Item(218, 2, 1),
						new Item(351, 17, 1),
					}),
				new ShapelessRecipe(new Item(218, 12, 1),
					new List<Item>
					{
						new Item(218, 1, 1),
						new Item(351, 17, 1),
					}),
				new ShapelessRecipe(new Item(218, 12, 1),
					new List<Item>
					{
						new Item(218, 0, 1),
						new Item(351, 17, 1),
					}),
				new ShapelessRecipe(new Item(218, 15, 1),
					new List<Item>
					{
						new Item(205, -1, 1),
						new Item(351, 16, 1),
					}),
				new ShapelessRecipe(new Item(218, 15, 1),
					new List<Item>
					{
						new Item(218, 14, 1),
						new Item(351, 16, 1),
					}),
				new ShapelessRecipe(new Item(218, 15, 1),
					new List<Item>
					{
						new Item(218, 13, 1),
						new Item(351, 16, 1),
					}),
				new ShapelessRecipe(new Item(218, 15, 1),
					new List<Item>
					{
						new Item(218, 12, 1),
						new Item(351, 16, 1),
					}),
				new ShapelessRecipe(new Item(218, 15, 1),
					new List<Item>
					{
						new Item(218, 11, 1),
						new Item(351, 16, 1),
					}),
				new ShapelessRecipe(new Item(218, 15, 1),
					new List<Item>
					{
						new Item(218, 10, 1),
						new Item(351, 16, 1),
					}),
				new ShapelessRecipe(new Item(218, 15, 1),
					new List<Item>
					{
						new Item(218, 9, 1),
						new Item(351, 16, 1),
					}),
				new ShapelessRecipe(new Item(218, 15, 1),
					new List<Item>
					{
						new Item(218, 8, 1),
						new Item(351, 16, 1),
					}),
				new ShapelessRecipe(new Item(218, 15, 1),
					new List<Item>
					{
						new Item(218, 7, 1),
						new Item(351, 16, 1),
					}),
				new ShapelessRecipe(new Item(218, 15, 1),
					new List<Item>
					{
						new Item(218, 6, 1),
						new Item(351, 16, 1),
					}),
				new ShapelessRecipe(new Item(218, 15, 1),
					new List<Item>
					{
						new Item(218, 5, 1),
						new Item(351, 16, 1),
					}),
				new ShapelessRecipe(new Item(218, 15, 1),
					new List<Item>
					{
						new Item(218, 4, 1),
						new Item(351, 16, 1),
					}),
				new ShapelessRecipe(new Item(218, 15, 1),
					new List<Item>
					{
						new Item(218, 3, 1),
						new Item(351, 16, 1),
					}),
				new ShapelessRecipe(new Item(218, 15, 1),
					new List<Item>
					{
						new Item(218, 2, 1),
						new Item(351, 16, 1),
					}),
				new ShapelessRecipe(new Item(218, 15, 1),
					new List<Item>
					{
						new Item(218, 1, 1),
						new Item(351, 16, 1),
					}),
				new ShapelessRecipe(new Item(218, 15, 1),
					new List<Item>
					{
						new Item(218, 0, 1),
						new Item(351, 16, 1),
					}),
				new ShapelessRecipe(new Item(218, 0, 1),
					new List<Item>
					{
						new Item(205, -1, 1),
						new Item(351, 15, 1),
					}),
				new ShapelessRecipe(new Item(218, 0, 1),
					new List<Item>
					{
						new Item(218, 15, 1),
						new Item(351, 15, 1),
					}),
				new ShapelessRecipe(new Item(218, 0, 1),
					new List<Item>
					{
						new Item(218, 14, 1),
						new Item(351, 15, 1),
					}),
				new ShapelessRecipe(new Item(218, 0, 1),
					new List<Item>
					{
						new Item(218, 13, 1),
						new Item(351, 15, 1),
					}),
				new ShapelessRecipe(new Item(218, 0, 1),
					new List<Item>
					{
						new Item(218, 12, 1),
						new Item(351, 15, 1),
					}),
				new ShapelessRecipe(new Item(218, 0, 1),
					new List<Item>
					{
						new Item(218, 11, 1),
						new Item(351, 15, 1),
					}),
				new ShapelessRecipe(new Item(218, 0, 1),
					new List<Item>
					{
						new Item(218, 10, 1),
						new Item(351, 15, 1),
					}),
				new ShapelessRecipe(new Item(218, 0, 1),
					new List<Item>
					{
						new Item(218, 9, 1),
						new Item(351, 15, 1),
					}),
				new ShapelessRecipe(new Item(218, 0, 1),
					new List<Item>
					{
						new Item(218, 8, 1),
						new Item(351, 15, 1),
					}),
				new ShapelessRecipe(new Item(218, 0, 1),
					new List<Item>
					{
						new Item(218, 7, 1),
						new Item(351, 15, 1),
					}),
				new ShapelessRecipe(new Item(218, 0, 1),
					new List<Item>
					{
						new Item(218, 6, 1),
						new Item(351, 15, 1),
					}),
				new ShapelessRecipe(new Item(218, 0, 1),
					new List<Item>
					{
						new Item(218, 5, 1),
						new Item(351, 15, 1),
					}),
				new ShapelessRecipe(new Item(218, 0, 1),
					new List<Item>
					{
						new Item(218, 4, 1),
						new Item(351, 15, 1),
					}),
				new ShapelessRecipe(new Item(218, 0, 1),
					new List<Item>
					{
						new Item(218, 3, 1),
						new Item(351, 15, 1),
					}),
				new ShapelessRecipe(new Item(218, 0, 1),
					new List<Item>
					{
						new Item(218, 2, 1),
						new Item(351, 15, 1),
					}),
				new ShapelessRecipe(new Item(218, 0, 1),
					new List<Item>
					{
						new Item(218, 1, 1),
						new Item(351, 15, 1),
					}),
				new ShapelessRecipe(new Item(218, 1, 1),
					new List<Item>
					{
						new Item(205, -1, 1),
						new Item(351, 14, 1),
					}),
				new ShapelessRecipe(new Item(218, 1, 1),
					new List<Item>
					{
						new Item(218, 15, 1),
						new Item(351, 14, 1),
					}),
				new ShapelessRecipe(new Item(218, 1, 1),
					new List<Item>
					{
						new Item(218, 14, 1),
						new Item(351, 14, 1),
					}),
				new ShapelessRecipe(new Item(218, 1, 1),
					new List<Item>
					{
						new Item(218, 13, 1),
						new Item(351, 14, 1),
					}),
				new ShapelessRecipe(new Item(218, 1, 1),
					new List<Item>
					{
						new Item(218, 12, 1),
						new Item(351, 14, 1),
					}),
				new ShapelessRecipe(new Item(218, 1, 1),
					new List<Item>
					{
						new Item(218, 11, 1),
						new Item(351, 14, 1),
					}),
				new ShapelessRecipe(new Item(218, 1, 1),
					new List<Item>
					{
						new Item(218, 10, 1),
						new Item(351, 14, 1),
					}),
				new ShapelessRecipe(new Item(218, 1, 1),
					new List<Item>
					{
						new Item(218, 9, 1),
						new Item(351, 14, 1),
					}),
				new ShapelessRecipe(new Item(218, 1, 1),
					new List<Item>
					{
						new Item(218, 8, 1),
						new Item(351, 14, 1),
					}),
				new ShapelessRecipe(new Item(218, 1, 1),
					new List<Item>
					{
						new Item(218, 7, 1),
						new Item(351, 14, 1),
					}),
				new ShapelessRecipe(new Item(218, 1, 1),
					new List<Item>
					{
						new Item(218, 6, 1),
						new Item(351, 14, 1),
					}),
				new ShapelessRecipe(new Item(218, 1, 1),
					new List<Item>
					{
						new Item(218, 5, 1),
						new Item(351, 14, 1),
					}),
				new ShapelessRecipe(new Item(218, 1, 1),
					new List<Item>
					{
						new Item(218, 4, 1),
						new Item(351, 14, 1),
					}),
				new ShapelessRecipe(new Item(218, 1, 1),
					new List<Item>
					{
						new Item(218, 3, 1),
						new Item(351, 14, 1),
					}),
				new ShapelessRecipe(new Item(218, 1, 1),
					new List<Item>
					{
						new Item(218, 2, 1),
						new Item(351, 14, 1),
					}),
				new ShapelessRecipe(new Item(218, 1, 1),
					new List<Item>
					{
						new Item(218, 0, 1),
						new Item(351, 14, 1),
					}),
				new ShapelessRecipe(new Item(218, 2, 1),
					new List<Item>
					{
						new Item(205, -1, 1),
						new Item(351, 13, 1),
					}),
				new ShapelessRecipe(new Item(218, 2, 1),
					new List<Item>
					{
						new Item(218, 15, 1),
						new Item(351, 13, 1),
					}),
				new ShapelessRecipe(new Item(218, 2, 1),
					new List<Item>
					{
						new Item(218, 14, 1),
						new Item(351, 13, 1),
					}),
				new ShapelessRecipe(new Item(218, 2, 1),
					new List<Item>
					{
						new Item(218, 13, 1),
						new Item(351, 13, 1),
					}),
				new ShapelessRecipe(new Item(218, 2, 1),
					new List<Item>
					{
						new Item(218, 12, 1),
						new Item(351, 13, 1),
					}),
				new ShapelessRecipe(new Item(218, 2, 1),
					new List<Item>
					{
						new Item(218, 11, 1),
						new Item(351, 13, 1),
					}),
				new ShapelessRecipe(new Item(218, 2, 1),
					new List<Item>
					{
						new Item(218, 10, 1),
						new Item(351, 13, 1),
					}),
				new ShapelessRecipe(new Item(218, 2, 1),
					new List<Item>
					{
						new Item(218, 9, 1),
						new Item(351, 13, 1),
					}),
				new ShapelessRecipe(new Item(218, 2, 1),
					new List<Item>
					{
						new Item(218, 8, 1),
						new Item(351, 13, 1),
					}),
				new ShapelessRecipe(new Item(218, 2, 1),
					new List<Item>
					{
						new Item(218, 7, 1),
						new Item(351, 13, 1),
					}),
				new ShapelessRecipe(new Item(218, 2, 1),
					new List<Item>
					{
						new Item(218, 6, 1),
						new Item(351, 13, 1),
					}),
				new ShapelessRecipe(new Item(218, 2, 1),
					new List<Item>
					{
						new Item(218, 5, 1),
						new Item(351, 13, 1),
					}),
				new ShapelessRecipe(new Item(218, 2, 1),
					new List<Item>
					{
						new Item(218, 4, 1),
						new Item(351, 13, 1),
					}),
				new ShapelessRecipe(new Item(218, 2, 1),
					new List<Item>
					{
						new Item(218, 3, 1),
						new Item(351, 13, 1),
					}),
				new ShapelessRecipe(new Item(218, 2, 1),
					new List<Item>
					{
						new Item(218, 1, 1),
						new Item(351, 13, 1),
					}),
				new ShapelessRecipe(new Item(218, 2, 1),
					new List<Item>
					{
						new Item(218, 0, 1),
						new Item(351, 13, 1),
					}),
				new ShapelessRecipe(new Item(218, 3, 1),
					new List<Item>
					{
						new Item(205, -1, 1),
						new Item(351, 12, 1),
					}),
				new ShapelessRecipe(new Item(218, 3, 1),
					new List<Item>
					{
						new Item(218, 15, 1),
						new Item(351, 12, 1),
					}),
				new ShapelessRecipe(new Item(218, 3, 1),
					new List<Item>
					{
						new Item(218, 14, 1),
						new Item(351, 12, 1),
					}),
				new ShapelessRecipe(new Item(218, 3, 1),
					new List<Item>
					{
						new Item(218, 13, 1),
						new Item(351, 12, 1),
					}),
				new ShapelessRecipe(new Item(218, 3, 1),
					new List<Item>
					{
						new Item(218, 12, 1),
						new Item(351, 12, 1),
					}),
				new ShapelessRecipe(new Item(218, 3, 1),
					new List<Item>
					{
						new Item(218, 11, 1),
						new Item(351, 12, 1),
					}),
				new ShapelessRecipe(new Item(218, 3, 1),
					new List<Item>
					{
						new Item(218, 10, 1),
						new Item(351, 12, 1),
					}),
				new ShapelessRecipe(new Item(218, 3, 1),
					new List<Item>
					{
						new Item(218, 9, 1),
						new Item(351, 12, 1),
					}),
				new ShapelessRecipe(new Item(218, 3, 1),
					new List<Item>
					{
						new Item(218, 8, 1),
						new Item(351, 12, 1),
					}),
				new ShapelessRecipe(new Item(218, 3, 1),
					new List<Item>
					{
						new Item(218, 7, 1),
						new Item(351, 12, 1),
					}),
				new ShapelessRecipe(new Item(218, 3, 1),
					new List<Item>
					{
						new Item(218, 6, 1),
						new Item(351, 12, 1),
					}),
				new ShapelessRecipe(new Item(218, 3, 1),
					new List<Item>
					{
						new Item(218, 5, 1),
						new Item(351, 12, 1),
					}),
				new ShapelessRecipe(new Item(218, 3, 1),
					new List<Item>
					{
						new Item(218, 4, 1),
						new Item(351, 12, 1),
					}),
				new ShapelessRecipe(new Item(218, 3, 1),
					new List<Item>
					{
						new Item(218, 2, 1),
						new Item(351, 12, 1),
					}),
				new ShapelessRecipe(new Item(218, 3, 1),
					new List<Item>
					{
						new Item(218, 1, 1),
						new Item(351, 12, 1),
					}),
				new ShapelessRecipe(new Item(218, 3, 1),
					new List<Item>
					{
						new Item(218, 0, 1),
						new Item(351, 12, 1),
					}),
				new ShapelessRecipe(new Item(218, 4, 1),
					new List<Item>
					{
						new Item(205, -1, 1),
						new Item(351, 11, 1),
					}),
				new ShapelessRecipe(new Item(218, 4, 1),
					new List<Item>
					{
						new Item(218, 15, 1),
						new Item(351, 11, 1),
					}),
				new ShapelessRecipe(new Item(218, 4, 1),
					new List<Item>
					{
						new Item(218, 14, 1),
						new Item(351, 11, 1),
					}),
				new ShapelessRecipe(new Item(218, 4, 1),
					new List<Item>
					{
						new Item(218, 13, 1),
						new Item(351, 11, 1),
					}),
				new ShapelessRecipe(new Item(218, 4, 1),
					new List<Item>
					{
						new Item(218, 12, 1),
						new Item(351, 11, 1),
					}),
				new ShapelessRecipe(new Item(218, 4, 1),
					new List<Item>
					{
						new Item(218, 11, 1),
						new Item(351, 11, 1),
					}),
				new ShapelessRecipe(new Item(218, 4, 1),
					new List<Item>
					{
						new Item(218, 10, 1),
						new Item(351, 11, 1),
					}),
				new ShapelessRecipe(new Item(218, 4, 1),
					new List<Item>
					{
						new Item(218, 9, 1),
						new Item(351, 11, 1),
					}),
				new ShapelessRecipe(new Item(218, 4, 1),
					new List<Item>
					{
						new Item(218, 8, 1),
						new Item(351, 11, 1),
					}),
				new ShapelessRecipe(new Item(218, 4, 1),
					new List<Item>
					{
						new Item(218, 7, 1),
						new Item(351, 11, 1),
					}),
				new ShapelessRecipe(new Item(218, 4, 1),
					new List<Item>
					{
						new Item(218, 6, 1),
						new Item(351, 11, 1),
					}),
				new ShapelessRecipe(new Item(218, 4, 1),
					new List<Item>
					{
						new Item(218, 5, 1),
						new Item(351, 11, 1),
					}),
				new ShapelessRecipe(new Item(218, 4, 1),
					new List<Item>
					{
						new Item(218, 3, 1),
						new Item(351, 11, 1),
					}),
				new ShapelessRecipe(new Item(218, 4, 1),
					new List<Item>
					{
						new Item(218, 2, 1),
						new Item(351, 11, 1),
					}),
				new ShapelessRecipe(new Item(218, 4, 1),
					new List<Item>
					{
						new Item(218, 1, 1),
						new Item(351, 11, 1),
					}),
				new ShapelessRecipe(new Item(218, 4, 1),
					new List<Item>
					{
						new Item(218, 0, 1),
						new Item(351, 11, 1),
					}),
				new ShapelessRecipe(new Item(218, 5, 1),
					new List<Item>
					{
						new Item(205, -1, 1),
						new Item(351, 10, 1),
					}),
				new ShapelessRecipe(new Item(218, 5, 1),
					new List<Item>
					{
						new Item(218, 15, 1),
						new Item(351, 10, 1),
					}),
				new ShapelessRecipe(new Item(218, 5, 1),
					new List<Item>
					{
						new Item(218, 14, 1),
						new Item(351, 10, 1),
					}),
				new ShapelessRecipe(new Item(218, 5, 1),
					new List<Item>
					{
						new Item(218, 13, 1),
						new Item(351, 10, 1),
					}),
				new ShapelessRecipe(new Item(218, 5, 1),
					new List<Item>
					{
						new Item(218, 12, 1),
						new Item(351, 10, 1),
					}),
				new ShapelessRecipe(new Item(218, 5, 1),
					new List<Item>
					{
						new Item(218, 11, 1),
						new Item(351, 10, 1),
					}),
				new ShapelessRecipe(new Item(218, 5, 1),
					new List<Item>
					{
						new Item(218, 10, 1),
						new Item(351, 10, 1),
					}),
				new ShapelessRecipe(new Item(218, 5, 1),
					new List<Item>
					{
						new Item(218, 9, 1),
						new Item(351, 10, 1),
					}),
				new ShapelessRecipe(new Item(218, 5, 1),
					new List<Item>
					{
						new Item(218, 8, 1),
						new Item(351, 10, 1),
					}),
				new ShapelessRecipe(new Item(218, 5, 1),
					new List<Item>
					{
						new Item(218, 7, 1),
						new Item(351, 10, 1),
					}),
				new ShapelessRecipe(new Item(218, 5, 1),
					new List<Item>
					{
						new Item(218, 6, 1),
						new Item(351, 10, 1),
					}),
				new ShapelessRecipe(new Item(218, 5, 1),
					new List<Item>
					{
						new Item(218, 4, 1),
						new Item(351, 10, 1),
					}),
				new ShapelessRecipe(new Item(218, 5, 1),
					new List<Item>
					{
						new Item(218, 3, 1),
						new Item(351, 10, 1),
					}),
				new ShapelessRecipe(new Item(218, 5, 1),
					new List<Item>
					{
						new Item(218, 2, 1),
						new Item(351, 10, 1),
					}),
				new ShapelessRecipe(new Item(218, 5, 1),
					new List<Item>
					{
						new Item(218, 1, 1),
						new Item(351, 10, 1),
					}),
				new ShapelessRecipe(new Item(218, 5, 1),
					new List<Item>
					{
						new Item(218, 0, 1),
						new Item(351, 10, 1),
					}),
				new ShapelessRecipe(new Item(218, 6, 1),
					new List<Item>
					{
						new Item(205, -1, 1),
						new Item(351, 9, 1),
					}),
				new ShapelessRecipe(new Item(218, 6, 1),
					new List<Item>
					{
						new Item(218, 15, 1),
						new Item(351, 9, 1),
					}),
				new ShapelessRecipe(new Item(218, 6, 1),
					new List<Item>
					{
						new Item(218, 14, 1),
						new Item(351, 9, 1),
					}),
				new ShapelessRecipe(new Item(218, 6, 1),
					new List<Item>
					{
						new Item(218, 13, 1),
						new Item(351, 9, 1),
					}),
				new ShapelessRecipe(new Item(218, 6, 1),
					new List<Item>
					{
						new Item(218, 12, 1),
						new Item(351, 9, 1),
					}),
				new ShapelessRecipe(new Item(218, 6, 1),
					new List<Item>
					{
						new Item(218, 11, 1),
						new Item(351, 9, 1),
					}),
				new ShapelessRecipe(new Item(218, 6, 1),
					new List<Item>
					{
						new Item(218, 10, 1),
						new Item(351, 9, 1),
					}),
				new ShapelessRecipe(new Item(218, 6, 1),
					new List<Item>
					{
						new Item(218, 9, 1),
						new Item(351, 9, 1),
					}),
				new ShapelessRecipe(new Item(218, 6, 1),
					new List<Item>
					{
						new Item(218, 8, 1),
						new Item(351, 9, 1),
					}),
				new ShapelessRecipe(new Item(218, 6, 1),
					new List<Item>
					{
						new Item(218, 7, 1),
						new Item(351, 9, 1),
					}),
				new ShapelessRecipe(new Item(218, 6, 1),
					new List<Item>
					{
						new Item(218, 5, 1),
						new Item(351, 9, 1),
					}),
				new ShapelessRecipe(new Item(218, 6, 1),
					new List<Item>
					{
						new Item(218, 4, 1),
						new Item(351, 9, 1),
					}),
				new ShapelessRecipe(new Item(218, 6, 1),
					new List<Item>
					{
						new Item(218, 3, 1),
						new Item(351, 9, 1),
					}),
				new ShapelessRecipe(new Item(218, 6, 1),
					new List<Item>
					{
						new Item(218, 2, 1),
						new Item(351, 9, 1),
					}),
				new ShapelessRecipe(new Item(218, 6, 1),
					new List<Item>
					{
						new Item(218, 1, 1),
						new Item(351, 9, 1),
					}),
				new ShapelessRecipe(new Item(218, 6, 1),
					new List<Item>
					{
						new Item(218, 0, 1),
						new Item(351, 9, 1),
					}),
				new ShapelessRecipe(new Item(218, 7, 1),
					new List<Item>
					{
						new Item(205, -1, 1),
						new Item(351, 8, 1),
					}),
				new ShapelessRecipe(new Item(218, 7, 1),
					new List<Item>
					{
						new Item(218, 15, 1),
						new Item(351, 8, 1),
					}),
				new ShapelessRecipe(new Item(218, 7, 1),
					new List<Item>
					{
						new Item(218, 14, 1),
						new Item(351, 8, 1),
					}),
				new ShapelessRecipe(new Item(218, 7, 1),
					new List<Item>
					{
						new Item(218, 13, 1),
						new Item(351, 8, 1),
					}),
				new ShapelessRecipe(new Item(218, 7, 1),
					new List<Item>
					{
						new Item(218, 12, 1),
						new Item(351, 8, 1),
					}),
				new ShapelessRecipe(new Item(218, 7, 1),
					new List<Item>
					{
						new Item(218, 11, 1),
						new Item(351, 8, 1),
					}),
				new ShapelessRecipe(new Item(218, 7, 1),
					new List<Item>
					{
						new Item(218, 10, 1),
						new Item(351, 8, 1),
					}),
				new ShapelessRecipe(new Item(218, 7, 1),
					new List<Item>
					{
						new Item(218, 9, 1),
						new Item(351, 8, 1),
					}),
				new ShapelessRecipe(new Item(218, 7, 1),
					new List<Item>
					{
						new Item(218, 8, 1),
						new Item(351, 8, 1),
					}),
				new ShapelessRecipe(new Item(218, 7, 1),
					new List<Item>
					{
						new Item(218, 6, 1),
						new Item(351, 8, 1),
					}),
				new ShapelessRecipe(new Item(218, 7, 1),
					new List<Item>
					{
						new Item(218, 5, 1),
						new Item(351, 8, 1),
					}),
				new ShapelessRecipe(new Item(218, 7, 1),
					new List<Item>
					{
						new Item(218, 4, 1),
						new Item(351, 8, 1),
					}),
				new ShapelessRecipe(new Item(218, 7, 1),
					new List<Item>
					{
						new Item(218, 3, 1),
						new Item(351, 8, 1),
					}),
				new ShapelessRecipe(new Item(218, 7, 1),
					new List<Item>
					{
						new Item(218, 2, 1),
						new Item(351, 8, 1),
					}),
				new ShapelessRecipe(new Item(218, 7, 1),
					new List<Item>
					{
						new Item(218, 1, 1),
						new Item(351, 8, 1),
					}),
				new ShapelessRecipe(new Item(218, 7, 1),
					new List<Item>
					{
						new Item(218, 0, 1),
						new Item(351, 8, 1),
					}),
				new ShapelessRecipe(new Item(218, 8, 1),
					new List<Item>
					{
						new Item(205, -1, 1),
						new Item(351, 7, 1),
					}),
				new ShapelessRecipe(new Item(218, 8, 1),
					new List<Item>
					{
						new Item(218, 15, 1),
						new Item(351, 7, 1),
					}),
				new ShapelessRecipe(new Item(218, 8, 1),
					new List<Item>
					{
						new Item(218, 14, 1),
						new Item(351, 7, 1),
					}),
				new ShapelessRecipe(new Item(218, 8, 1),
					new List<Item>
					{
						new Item(218, 13, 1),
						new Item(351, 7, 1),
					}),
				new ShapelessRecipe(new Item(218, 8, 1),
					new List<Item>
					{
						new Item(218, 12, 1),
						new Item(351, 7, 1),
					}),
				new ShapelessRecipe(new Item(218, 8, 1),
					new List<Item>
					{
						new Item(218, 11, 1),
						new Item(351, 7, 1),
					}),
				new ShapelessRecipe(new Item(218, 8, 1),
					new List<Item>
					{
						new Item(218, 10, 1),
						new Item(351, 7, 1),
					}),
				new ShapelessRecipe(new Item(218, 8, 1),
					new List<Item>
					{
						new Item(218, 9, 1),
						new Item(351, 7, 1),
					}),
				new ShapelessRecipe(new Item(218, 8, 1),
					new List<Item>
					{
						new Item(218, 7, 1),
						new Item(351, 7, 1),
					}),
				new ShapelessRecipe(new Item(218, 8, 1),
					new List<Item>
					{
						new Item(218, 6, 1),
						new Item(351, 7, 1),
					}),
				new ShapelessRecipe(new Item(218, 8, 1),
					new List<Item>
					{
						new Item(218, 5, 1),
						new Item(351, 7, 1),
					}),
				new ShapelessRecipe(new Item(218, 8, 1),
					new List<Item>
					{
						new Item(218, 4, 1),
						new Item(351, 7, 1),
					}),
				new ShapelessRecipe(new Item(218, 8, 1),
					new List<Item>
					{
						new Item(218, 3, 1),
						new Item(351, 7, 1),
					}),
				new ShapelessRecipe(new Item(218, 8, 1),
					new List<Item>
					{
						new Item(218, 2, 1),
						new Item(351, 7, 1),
					}),
				new ShapelessRecipe(new Item(218, 8, 1),
					new List<Item>
					{
						new Item(218, 1, 1),
						new Item(351, 7, 1),
					}),
				new ShapelessRecipe(new Item(218, 8, 1),
					new List<Item>
					{
						new Item(218, 0, 1),
						new Item(351, 7, 1),
					}),
				new ShapelessRecipe(new Item(218, 9, 1),
					new List<Item>
					{
						new Item(205, -1, 1),
						new Item(351, 6, 1),
					}),
				new ShapelessRecipe(new Item(218, 9, 1),
					new List<Item>
					{
						new Item(218, 15, 1),
						new Item(351, 6, 1),
					}),
				new ShapelessRecipe(new Item(218, 9, 1),
					new List<Item>
					{
						new Item(218, 14, 1),
						new Item(351, 6, 1),
					}),
				new ShapelessRecipe(new Item(218, 9, 1),
					new List<Item>
					{
						new Item(218, 13, 1),
						new Item(351, 6, 1),
					}),
				new ShapelessRecipe(new Item(218, 9, 1),
					new List<Item>
					{
						new Item(218, 12, 1),
						new Item(351, 6, 1),
					}),
				new ShapelessRecipe(new Item(218, 9, 1),
					new List<Item>
					{
						new Item(218, 11, 1),
						new Item(351, 6, 1),
					}),
				new ShapelessRecipe(new Item(218, 9, 1),
					new List<Item>
					{
						new Item(218, 10, 1),
						new Item(351, 6, 1),
					}),
				new ShapelessRecipe(new Item(218, 9, 1),
					new List<Item>
					{
						new Item(218, 8, 1),
						new Item(351, 6, 1),
					}),
				new ShapelessRecipe(new Item(218, 9, 1),
					new List<Item>
					{
						new Item(218, 7, 1),
						new Item(351, 6, 1),
					}),
				new ShapelessRecipe(new Item(218, 9, 1),
					new List<Item>
					{
						new Item(218, 6, 1),
						new Item(351, 6, 1),
					}),
				new ShapelessRecipe(new Item(218, 9, 1),
					new List<Item>
					{
						new Item(218, 5, 1),
						new Item(351, 6, 1),
					}),
				new ShapelessRecipe(new Item(218, 9, 1),
					new List<Item>
					{
						new Item(218, 4, 1),
						new Item(351, 6, 1),
					}),
				new ShapelessRecipe(new Item(218, 9, 1),
					new List<Item>
					{
						new Item(218, 3, 1),
						new Item(351, 6, 1),
					}),
				new ShapelessRecipe(new Item(218, 9, 1),
					new List<Item>
					{
						new Item(218, 2, 1),
						new Item(351, 6, 1),
					}),
				new ShapelessRecipe(new Item(218, 9, 1),
					new List<Item>
					{
						new Item(218, 1, 1),
						new Item(351, 6, 1),
					}),
				new ShapelessRecipe(new Item(218, 9, 1),
					new List<Item>
					{
						new Item(218, 0, 1),
						new Item(351, 6, 1),
					}),
				new ShapelessRecipe(new Item(218, 10, 1),
					new List<Item>
					{
						new Item(205, -1, 1),
						new Item(351, 5, 1),
					}),
				new ShapelessRecipe(new Item(218, 10, 1),
					new List<Item>
					{
						new Item(218, 15, 1),
						new Item(351, 5, 1),
					}),
				new ShapelessRecipe(new Item(218, 10, 1),
					new List<Item>
					{
						new Item(218, 14, 1),
						new Item(351, 5, 1),
					}),
				new ShapelessRecipe(new Item(218, 10, 1),
					new List<Item>
					{
						new Item(218, 13, 1),
						new Item(351, 5, 1),
					}),
				new ShapelessRecipe(new Item(218, 10, 1),
					new List<Item>
					{
						new Item(218, 12, 1),
						new Item(351, 5, 1),
					}),
				new ShapelessRecipe(new Item(218, 10, 1),
					new List<Item>
					{
						new Item(218, 11, 1),
						new Item(351, 5, 1),
					}),
				new ShapelessRecipe(new Item(218, 10, 1),
					new List<Item>
					{
						new Item(218, 9, 1),
						new Item(351, 5, 1),
					}),
				new ShapelessRecipe(new Item(218, 10, 1),
					new List<Item>
					{
						new Item(218, 8, 1),
						new Item(351, 5, 1),
					}),
				new ShapelessRecipe(new Item(218, 10, 1),
					new List<Item>
					{
						new Item(218, 7, 1),
						new Item(351, 5, 1),
					}),
				new ShapelessRecipe(new Item(218, 10, 1),
					new List<Item>
					{
						new Item(218, 6, 1),
						new Item(351, 5, 1),
					}),
				new ShapelessRecipe(new Item(218, 10, 1),
					new List<Item>
					{
						new Item(218, 5, 1),
						new Item(351, 5, 1),
					}),
				new ShapelessRecipe(new Item(218, 10, 1),
					new List<Item>
					{
						new Item(218, 4, 1),
						new Item(351, 5, 1),
					}),
				new ShapelessRecipe(new Item(218, 10, 1),
					new List<Item>
					{
						new Item(218, 3, 1),
						new Item(351, 5, 1),
					}),
				new ShapelessRecipe(new Item(218, 10, 1),
					new List<Item>
					{
						new Item(218, 2, 1),
						new Item(351, 5, 1),
					}),
				new ShapelessRecipe(new Item(218, 10, 1),
					new List<Item>
					{
						new Item(218, 1, 1),
						new Item(351, 5, 1),
					}),
				new ShapelessRecipe(new Item(218, 10, 1),
					new List<Item>
					{
						new Item(218, 0, 1),
						new Item(351, 5, 1),
					}),
				new ShapelessRecipe(new Item(218, 11, 1),
					new List<Item>
					{
						new Item(205, -1, 1),
						new Item(351, 4, 1),
					}),
				new ShapelessRecipe(new Item(218, 11, 1),
					new List<Item>
					{
						new Item(218, 15, 1),
						new Item(351, 4, 1),
					}),
				new ShapelessRecipe(new Item(218, 11, 1),
					new List<Item>
					{
						new Item(218, 14, 1),
						new Item(351, 4, 1),
					}),
				new ShapelessRecipe(new Item(218, 11, 1),
					new List<Item>
					{
						new Item(218, 13, 1),
						new Item(351, 4, 1),
					}),
				new ShapelessRecipe(new Item(218, 11, 1),
					new List<Item>
					{
						new Item(218, 12, 1),
						new Item(351, 4, 1),
					}),
				new ShapelessRecipe(new Item(218, 11, 1),
					new List<Item>
					{
						new Item(218, 10, 1),
						new Item(351, 4, 1),
					}),
				new ShapelessRecipe(new Item(218, 11, 1),
					new List<Item>
					{
						new Item(218, 9, 1),
						new Item(351, 4, 1),
					}),
				new ShapelessRecipe(new Item(218, 11, 1),
					new List<Item>
					{
						new Item(218, 8, 1),
						new Item(351, 4, 1),
					}),
				new ShapelessRecipe(new Item(218, 11, 1),
					new List<Item>
					{
						new Item(218, 7, 1),
						new Item(351, 4, 1),
					}),
				new ShapelessRecipe(new Item(218, 11, 1),
					new List<Item>
					{
						new Item(218, 6, 1),
						new Item(351, 4, 1),
					}),
				new ShapelessRecipe(new Item(218, 11, 1),
					new List<Item>
					{
						new Item(218, 5, 1),
						new Item(351, 4, 1),
					}),
				new ShapelessRecipe(new Item(218, 11, 1),
					new List<Item>
					{
						new Item(218, 4, 1),
						new Item(351, 4, 1),
					}),
				new ShapelessRecipe(new Item(218, 11, 1),
					new List<Item>
					{
						new Item(218, 3, 1),
						new Item(351, 4, 1),
					}),
				new ShapelessRecipe(new Item(218, 11, 1),
					new List<Item>
					{
						new Item(218, 2, 1),
						new Item(351, 4, 1),
					}),
				new ShapelessRecipe(new Item(218, 11, 1),
					new List<Item>
					{
						new Item(218, 1, 1),
						new Item(351, 4, 1),
					}),
				new ShapelessRecipe(new Item(218, 11, 1),
					new List<Item>
					{
						new Item(218, 0, 1),
						new Item(351, 4, 1),
					}),
				new ShapelessRecipe(new Item(218, 12, 1),
					new List<Item>
					{
						new Item(205, -1, 1),
						new Item(351, 3, 1),
					}),
				new ShapelessRecipe(new Item(218, 12, 1),
					new List<Item>
					{
						new Item(218, 15, 1),
						new Item(351, 3, 1),
					}),
				new ShapelessRecipe(new Item(218, 12, 1),
					new List<Item>
					{
						new Item(218, 14, 1),
						new Item(351, 3, 1),
					}),
				new ShapelessRecipe(new Item(218, 12, 1),
					new List<Item>
					{
						new Item(218, 13, 1),
						new Item(351, 3, 1),
					}),
				new ShapelessRecipe(new Item(218, 12, 1),
					new List<Item>
					{
						new Item(218, 11, 1),
						new Item(351, 3, 1),
					}),
				new ShapelessRecipe(new Item(218, 12, 1),
					new List<Item>
					{
						new Item(218, 10, 1),
						new Item(351, 3, 1),
					}),
				new ShapelessRecipe(new Item(218, 12, 1),
					new List<Item>
					{
						new Item(218, 9, 1),
						new Item(351, 3, 1),
					}),
				new ShapelessRecipe(new Item(218, 12, 1),
					new List<Item>
					{
						new Item(218, 8, 1),
						new Item(351, 3, 1),
					}),
				new ShapelessRecipe(new Item(218, 12, 1),
					new List<Item>
					{
						new Item(218, 7, 1),
						new Item(351, 3, 1),
					}),
				new ShapelessRecipe(new Item(218, 12, 1),
					new List<Item>
					{
						new Item(218, 6, 1),
						new Item(351, 3, 1),
					}),
				new ShapelessRecipe(new Item(218, 12, 1),
					new List<Item>
					{
						new Item(218, 5, 1),
						new Item(351, 3, 1),
					}),
				new ShapelessRecipe(new Item(218, 12, 1),
					new List<Item>
					{
						new Item(218, 4, 1),
						new Item(351, 3, 1),
					}),
				new ShapelessRecipe(new Item(218, 12, 1),
					new List<Item>
					{
						new Item(218, 3, 1),
						new Item(351, 3, 1),
					}),
				new ShapelessRecipe(new Item(218, 12, 1),
					new List<Item>
					{
						new Item(218, 2, 1),
						new Item(351, 3, 1),
					}),
				new ShapelessRecipe(new Item(218, 12, 1),
					new List<Item>
					{
						new Item(218, 1, 1),
						new Item(351, 3, 1),
					}),
				new ShapelessRecipe(new Item(218, 12, 1),
					new List<Item>
					{
						new Item(218, 0, 1),
						new Item(351, 3, 1),
					}),
				new ShapelessRecipe(new Item(218, 13, 1),
					new List<Item>
					{
						new Item(205, -1, 1),
						new Item(351, 2, 1),
					}),
				new ShapelessRecipe(new Item(218, 13, 1),
					new List<Item>
					{
						new Item(218, 15, 1),
						new Item(351, 2, 1),
					}),
				new ShapelessRecipe(new Item(218, 13, 1),
					new List<Item>
					{
						new Item(218, 14, 1),
						new Item(351, 2, 1),
					}),
				new ShapelessRecipe(new Item(218, 13, 1),
					new List<Item>
					{
						new Item(218, 12, 1),
						new Item(351, 2, 1),
					}),
				new ShapelessRecipe(new Item(218, 13, 1),
					new List<Item>
					{
						new Item(218, 11, 1),
						new Item(351, 2, 1),
					}),
				new ShapelessRecipe(new Item(218, 13, 1),
					new List<Item>
					{
						new Item(218, 10, 1),
						new Item(351, 2, 1),
					}),
				new ShapelessRecipe(new Item(218, 13, 1),
					new List<Item>
					{
						new Item(218, 9, 1),
						new Item(351, 2, 1),
					}),
				new ShapelessRecipe(new Item(218, 13, 1),
					new List<Item>
					{
						new Item(218, 8, 1),
						new Item(351, 2, 1),
					}),
				new ShapelessRecipe(new Item(218, 13, 1),
					new List<Item>
					{
						new Item(218, 7, 1),
						new Item(351, 2, 1),
					}),
				new ShapelessRecipe(new Item(218, 13, 1),
					new List<Item>
					{
						new Item(218, 6, 1),
						new Item(351, 2, 1),
					}),
				new ShapelessRecipe(new Item(218, 13, 1),
					new List<Item>
					{
						new Item(218, 5, 1),
						new Item(351, 2, 1),
					}),
				new ShapelessRecipe(new Item(218, 13, 1),
					new List<Item>
					{
						new Item(218, 4, 1),
						new Item(351, 2, 1),
					}),
				new ShapelessRecipe(new Item(218, 13, 1),
					new List<Item>
					{
						new Item(218, 3, 1),
						new Item(351, 2, 1),
					}),
				new ShapelessRecipe(new Item(218, 13, 1),
					new List<Item>
					{
						new Item(218, 2, 1),
						new Item(351, 2, 1),
					}),
				new ShapelessRecipe(new Item(218, 13, 1),
					new List<Item>
					{
						new Item(218, 1, 1),
						new Item(351, 2, 1),
					}),
				new ShapelessRecipe(new Item(218, 13, 1),
					new List<Item>
					{
						new Item(218, 0, 1),
						new Item(351, 2, 1),
					}),
				new ShapelessRecipe(new Item(218, 14, 1),
					new List<Item>
					{
						new Item(205, -1, 1),
						new Item(351, 1, 1),
					}),
				new ShapelessRecipe(new Item(218, 14, 1),
					new List<Item>
					{
						new Item(218, 15, 1),
						new Item(351, 1, 1),
					}),
				new ShapelessRecipe(new Item(218, 14, 1),
					new List<Item>
					{
						new Item(218, 13, 1),
						new Item(351, 1, 1),
					}),
				new ShapelessRecipe(new Item(218, 14, 1),
					new List<Item>
					{
						new Item(218, 12, 1),
						new Item(351, 1, 1),
					}),
				new ShapelessRecipe(new Item(218, 14, 1),
					new List<Item>
					{
						new Item(218, 11, 1),
						new Item(351, 1, 1),
					}),
				new ShapelessRecipe(new Item(218, 14, 1),
					new List<Item>
					{
						new Item(218, 10, 1),
						new Item(351, 1, 1),
					}),
				new ShapelessRecipe(new Item(218, 14, 1),
					new List<Item>
					{
						new Item(218, 9, 1),
						new Item(351, 1, 1),
					}),
				new ShapelessRecipe(new Item(218, 14, 1),
					new List<Item>
					{
						new Item(218, 8, 1),
						new Item(351, 1, 1),
					}),
				new ShapelessRecipe(new Item(218, 14, 1),
					new List<Item>
					{
						new Item(218, 7, 1),
						new Item(351, 1, 1),
					}),
				new ShapelessRecipe(new Item(218, 14, 1),
					new List<Item>
					{
						new Item(218, 6, 1),
						new Item(351, 1, 1),
					}),
				new ShapelessRecipe(new Item(218, 14, 1),
					new List<Item>
					{
						new Item(218, 5, 1),
						new Item(351, 1, 1),
					}),
				new ShapelessRecipe(new Item(218, 14, 1),
					new List<Item>
					{
						new Item(218, 4, 1),
						new Item(351, 1, 1),
					}),
				new ShapelessRecipe(new Item(218, 14, 1),
					new List<Item>
					{
						new Item(218, 3, 1),
						new Item(351, 1, 1),
					}),
				new ShapelessRecipe(new Item(218, 14, 1),
					new List<Item>
					{
						new Item(218, 2, 1),
						new Item(351, 1, 1),
					}),
				new ShapelessRecipe(new Item(218, 14, 1),
					new List<Item>
					{
						new Item(218, 1, 1),
						new Item(351, 1, 1),
					}),
				new ShapelessRecipe(new Item(218, 14, 1),
					new List<Item>
					{
						new Item(218, 0, 1),
						new Item(351, 1, 1),
					}),
				new ShapelessRecipe(new Item(218, 15, 1),
					new List<Item>
					{
						new Item(205, -1, 1),
						new Item(351, 0, 1),
					}),
				new ShapelessRecipe(new Item(218, 15, 1),
					new List<Item>
					{
						new Item(218, 14, 1),
						new Item(351, 0, 1),
					}),
				new ShapelessRecipe(new Item(218, 15, 1),
					new List<Item>
					{
						new Item(218, 13, 1),
						new Item(351, 0, 1),
					}),
				new ShapelessRecipe(new Item(218, 15, 1),
					new List<Item>
					{
						new Item(218, 12, 1),
						new Item(351, 0, 1),
					}),
				new ShapelessRecipe(new Item(218, 15, 1),
					new List<Item>
					{
						new Item(218, 11, 1),
						new Item(351, 0, 1),
					}),
				new ShapelessRecipe(new Item(218, 15, 1),
					new List<Item>
					{
						new Item(218, 10, 1),
						new Item(351, 0, 1),
					}),
				new ShapelessRecipe(new Item(218, 15, 1),
					new List<Item>
					{
						new Item(218, 9, 1),
						new Item(351, 0, 1),
					}),
				new ShapelessRecipe(new Item(218, 15, 1),
					new List<Item>
					{
						new Item(218, 8, 1),
						new Item(351, 0, 1),
					}),
				new ShapelessRecipe(new Item(218, 15, 1),
					new List<Item>
					{
						new Item(218, 7, 1),
						new Item(351, 0, 1),
					}),
				new ShapelessRecipe(new Item(218, 15, 1),
					new List<Item>
					{
						new Item(218, 6, 1),
						new Item(351, 0, 1),
					}),
				new ShapelessRecipe(new Item(218, 15, 1),
					new List<Item>
					{
						new Item(218, 5, 1),
						new Item(351, 0, 1),
					}),
				new ShapelessRecipe(new Item(218, 15, 1),
					new List<Item>
					{
						new Item(218, 4, 1),
						new Item(351, 0, 1),
					}),
				new ShapelessRecipe(new Item(218, 15, 1),
					new List<Item>
					{
						new Item(218, 3, 1),
						new Item(351, 0, 1),
					}),
				new ShapelessRecipe(new Item(218, 15, 1),
					new List<Item>
					{
						new Item(218, 2, 1),
						new Item(351, 0, 1),
					}),
				new ShapelessRecipe(new Item(218, 15, 1),
					new List<Item>
					{
						new Item(218, 1, 1),
						new Item(351, 0, 1),
					}),
				new ShapelessRecipe(new Item(218, 15, 1),
					new List<Item>
					{
						new Item(218, 0, 1),
						new Item(351, 0, 1),
					}),
				new ShapedRecipe(3, 2, new Item(355, 0, 1),
					new Item[]
					{
						new Item(35, 0),
						new Item(35, 0),
						new Item(5, -1),
						new Item(35, 0),
						new Item(5, -1),
						new Item(5, -1),
					}),
				new ShapedRecipe(3, 2, new Item(355, 1, 1),
					new Item[]
					{
						new Item(35, 1),
						new Item(35, 1),
						new Item(5, -1),
						new Item(35, 1),
						new Item(5, -1),
						new Item(5, -1),
					}),
				new ShapedRecipe(3, 2, new Item(355, 2, 1),
					new Item[]
					{
						new Item(35, 2),
						new Item(35, 2),
						new Item(5, -1),
						new Item(35, 2),
						new Item(5, -1),
						new Item(5, -1),
					}),
				new ShapedRecipe(3, 2, new Item(355, 3, 1),
					new Item[]
					{
						new Item(35, 3),
						new Item(35, 3),
						new Item(5, -1),
						new Item(35, 3),
						new Item(5, -1),
						new Item(5, -1),
					}),
				new ShapedRecipe(3, 2, new Item(355, 4, 1),
					new Item[]
					{
						new Item(35, 4),
						new Item(35, 4),
						new Item(5, -1),
						new Item(35, 4),
						new Item(5, -1),
						new Item(5, -1),
					}),
				new ShapedRecipe(3, 2, new Item(355, 5, 1),
					new Item[]
					{
						new Item(35, 5),
						new Item(35, 5),
						new Item(5, -1),
						new Item(35, 5),
						new Item(5, -1),
						new Item(5, -1),
					}),
				new ShapedRecipe(3, 2, new Item(355, 6, 1),
					new Item[]
					{
						new Item(35, 6),
						new Item(35, 6),
						new Item(5, -1),
						new Item(35, 6),
						new Item(5, -1),
						new Item(5, -1),
					}),
				new ShapedRecipe(3, 2, new Item(355, 7, 1),
					new Item[]
					{
						new Item(35, 7),
						new Item(35, 7),
						new Item(5, -1),
						new Item(35, 7),
						new Item(5, -1),
						new Item(5, -1),
					}),
				new ShapedRecipe(3, 2, new Item(355, 8, 1),
					new Item[]
					{
						new Item(35, 8),
						new Item(35, 8),
						new Item(5, -1),
						new Item(35, 8),
						new Item(5, -1),
						new Item(5, -1),
					}),
				new ShapedRecipe(3, 2, new Item(355, 9, 1),
					new Item[]
					{
						new Item(35, 9),
						new Item(35, 9),
						new Item(5, -1),
						new Item(35, 9),
						new Item(5, -1),
						new Item(5, -1),
					}),
				new ShapedRecipe(3, 2, new Item(355, 10, 1),
					new Item[]
					{
						new Item(35, 10),
						new Item(35, 10),
						new Item(5, -1),
						new Item(35, 10),
						new Item(5, -1),
						new Item(5, -1),
					}),
				new ShapedRecipe(3, 2, new Item(355, 11, 1),
					new Item[]
					{
						new Item(35, 11),
						new Item(35, 11),
						new Item(5, -1),
						new Item(35, 11),
						new Item(5, -1),
						new Item(5, -1),
					}),
				new ShapedRecipe(3, 2, new Item(355, 12, 1),
					new Item[]
					{
						new Item(35, 12),
						new Item(35, 12),
						new Item(5, -1),
						new Item(35, 12),
						new Item(5, -1),
						new Item(5, -1),
					}),
				new ShapedRecipe(3, 2, new Item(355, 13, 1),
					new Item[]
					{
						new Item(35, 13),
						new Item(35, 13),
						new Item(5, -1),
						new Item(35, 13),
						new Item(5, -1),
						new Item(5, -1),
					}),
				new ShapedRecipe(3, 2, new Item(355, 14, 1),
					new Item[]
					{
						new Item(35, 14),
						new Item(35, 14),
						new Item(5, -1),
						new Item(35, 14),
						new Item(5, -1),
						new Item(5, -1),
					}),
				new ShapedRecipe(3, 2, new Item(355, 15, 1),
					new Item[]
					{
						new Item(35, 15),
						new Item(35, 15),
						new Item(5, -1),
						new Item(35, 15),
						new Item(5, -1),
						new Item(5, -1),
					}),
				new ShapelessRecipe(new Item(355, 0, 1),
					new List<Item>
					{
						new Item(355, 15, 1),
						new Item(351, 19, 1),
					}),
				new ShapelessRecipe(new Item(355, 0, 1),
					new List<Item>
					{
						new Item(355, 14, 1),
						new Item(351, 19, 1),
					}),
				new ShapelessRecipe(new Item(355, 0, 1),
					new List<Item>
					{
						new Item(355, 13, 1),
						new Item(351, 19, 1),
					}),
				new ShapelessRecipe(new Item(355, 0, 1),
					new List<Item>
					{
						new Item(355, 12, 1),
						new Item(351, 19, 1),
					}),
				new ShapelessRecipe(new Item(355, 0, 1),
					new List<Item>
					{
						new Item(355, 11, 1),
						new Item(351, 19, 1),
					}),
				new ShapelessRecipe(new Item(355, 0, 1),
					new List<Item>
					{
						new Item(355, 10, 1),
						new Item(351, 19, 1),
					}),
				new ShapelessRecipe(new Item(355, 0, 1),
					new List<Item>
					{
						new Item(355, 9, 1),
						new Item(351, 19, 1),
					}),
				new ShapelessRecipe(new Item(355, 0, 1),
					new List<Item>
					{
						new Item(355, 8, 1),
						new Item(351, 19, 1),
					}),
				new ShapelessRecipe(new Item(355, 0, 1),
					new List<Item>
					{
						new Item(355, 7, 1),
						new Item(351, 19, 1),
					}),
				new ShapelessRecipe(new Item(355, 0, 1),
					new List<Item>
					{
						new Item(355, 6, 1),
						new Item(351, 19, 1),
					}),
				new ShapelessRecipe(new Item(355, 0, 1),
					new List<Item>
					{
						new Item(355, 5, 1),
						new Item(351, 19, 1),
					}),
				new ShapelessRecipe(new Item(355, 0, 1),
					new List<Item>
					{
						new Item(355, 4, 1),
						new Item(351, 19, 1),
					}),
				new ShapelessRecipe(new Item(355, 0, 1),
					new List<Item>
					{
						new Item(355, 3, 1),
						new Item(351, 19, 1),
					}),
				new ShapelessRecipe(new Item(355, 0, 1),
					new List<Item>
					{
						new Item(355, 2, 1),
						new Item(351, 19, 1),
					}),
				new ShapelessRecipe(new Item(355, 0, 1),
					new List<Item>
					{
						new Item(355, 1, 1),
						new Item(351, 19, 1),
					}),
				new ShapelessRecipe(new Item(355, 11, 1),
					new List<Item>
					{
						new Item(355, 15, 1),
						new Item(351, 18, 1),
					}),
				new ShapelessRecipe(new Item(355, 11, 1),
					new List<Item>
					{
						new Item(355, 14, 1),
						new Item(351, 18, 1),
					}),
				new ShapelessRecipe(new Item(355, 11, 1),
					new List<Item>
					{
						new Item(355, 13, 1),
						new Item(351, 18, 1),
					}),
				new ShapelessRecipe(new Item(355, 11, 1),
					new List<Item>
					{
						new Item(355, 12, 1),
						new Item(351, 18, 1),
					}),
				new ShapelessRecipe(new Item(355, 11, 1),
					new List<Item>
					{
						new Item(355, 10, 1),
						new Item(351, 18, 1),
					}),
				new ShapelessRecipe(new Item(355, 11, 1),
					new List<Item>
					{
						new Item(355, 9, 1),
						new Item(351, 18, 1),
					}),
				new ShapelessRecipe(new Item(355, 11, 1),
					new List<Item>
					{
						new Item(355, 8, 1),
						new Item(351, 18, 1),
					}),
				new ShapelessRecipe(new Item(355, 11, 1),
					new List<Item>
					{
						new Item(355, 7, 1),
						new Item(351, 18, 1),
					}),
				new ShapelessRecipe(new Item(355, 11, 1),
					new List<Item>
					{
						new Item(355, 6, 1),
						new Item(351, 18, 1),
					}),
				new ShapelessRecipe(new Item(355, 11, 1),
					new List<Item>
					{
						new Item(355, 5, 1),
						new Item(351, 18, 1),
					}),
				new ShapelessRecipe(new Item(355, 11, 1),
					new List<Item>
					{
						new Item(355, 4, 1),
						new Item(351, 18, 1),
					}),
				new ShapelessRecipe(new Item(355, 11, 1),
					new List<Item>
					{
						new Item(355, 3, 1),
						new Item(351, 18, 1),
					}),
				new ShapelessRecipe(new Item(355, 11, 1),
					new List<Item>
					{
						new Item(355, 2, 1),
						new Item(351, 18, 1),
					}),
				new ShapelessRecipe(new Item(355, 11, 1),
					new List<Item>
					{
						new Item(355, 1, 1),
						new Item(351, 18, 1),
					}),
				new ShapelessRecipe(new Item(355, 11, 1),
					new List<Item>
					{
						new Item(355, 0, 1),
						new Item(351, 18, 1),
					}),
				new ShapelessRecipe(new Item(355, 12, 1),
					new List<Item>
					{
						new Item(355, 15, 1),
						new Item(351, 17, 1),
					}),
				new ShapelessRecipe(new Item(355, 12, 1),
					new List<Item>
					{
						new Item(355, 14, 1),
						new Item(351, 17, 1),
					}),
				new ShapelessRecipe(new Item(355, 12, 1),
					new List<Item>
					{
						new Item(355, 13, 1),
						new Item(351, 17, 1),
					}),
				new ShapelessRecipe(new Item(355, 12, 1),
					new List<Item>
					{
						new Item(355, 11, 1),
						new Item(351, 17, 1),
					}),
				new ShapelessRecipe(new Item(355, 12, 1),
					new List<Item>
					{
						new Item(355, 10, 1),
						new Item(351, 17, 1),
					}),
				new ShapelessRecipe(new Item(355, 12, 1),
					new List<Item>
					{
						new Item(355, 9, 1),
						new Item(351, 17, 1),
					}),
				new ShapelessRecipe(new Item(355, 12, 1),
					new List<Item>
					{
						new Item(355, 8, 1),
						new Item(351, 17, 1),
					}),
				new ShapelessRecipe(new Item(355, 12, 1),
					new List<Item>
					{
						new Item(355, 7, 1),
						new Item(351, 17, 1),
					}),
				new ShapelessRecipe(new Item(355, 12, 1),
					new List<Item>
					{
						new Item(355, 6, 1),
						new Item(351, 17, 1),
					}),
				new ShapelessRecipe(new Item(355, 12, 1),
					new List<Item>
					{
						new Item(355, 5, 1),
						new Item(351, 17, 1),
					}),
				new ShapelessRecipe(new Item(355, 12, 1),
					new List<Item>
					{
						new Item(355, 4, 1),
						new Item(351, 17, 1),
					}),
				new ShapelessRecipe(new Item(355, 12, 1),
					new List<Item>
					{
						new Item(355, 3, 1),
						new Item(351, 17, 1),
					}),
				new ShapelessRecipe(new Item(355, 12, 1),
					new List<Item>
					{
						new Item(355, 2, 1),
						new Item(351, 17, 1),
					}),
				new ShapelessRecipe(new Item(355, 12, 1),
					new List<Item>
					{
						new Item(355, 1, 1),
						new Item(351, 17, 1),
					}),
				new ShapelessRecipe(new Item(355, 12, 1),
					new List<Item>
					{
						new Item(355, 0, 1),
						new Item(351, 17, 1),
					}),
				new ShapelessRecipe(new Item(355, 15, 1),
					new List<Item>
					{
						new Item(355, 14, 1),
						new Item(351, 16, 1),
					}),
				new ShapelessRecipe(new Item(355, 15, 1),
					new List<Item>
					{
						new Item(355, 13, 1),
						new Item(351, 16, 1),
					}),
				new ShapelessRecipe(new Item(355, 15, 1),
					new List<Item>
					{
						new Item(355, 12, 1),
						new Item(351, 16, 1),
					}),
				new ShapelessRecipe(new Item(355, 15, 1),
					new List<Item>
					{
						new Item(355, 11, 1),
						new Item(351, 16, 1),
					}),
				new ShapelessRecipe(new Item(355, 15, 1),
					new List<Item>
					{
						new Item(355, 10, 1),
						new Item(351, 16, 1),
					}),
				new ShapelessRecipe(new Item(355, 15, 1),
					new List<Item>
					{
						new Item(355, 9, 1),
						new Item(351, 16, 1),
					}),
				new ShapelessRecipe(new Item(355, 15, 1),
					new List<Item>
					{
						new Item(355, 8, 1),
						new Item(351, 16, 1),
					}),
				new ShapelessRecipe(new Item(355, 15, 1),
					new List<Item>
					{
						new Item(355, 7, 1),
						new Item(351, 16, 1),
					}),
				new ShapelessRecipe(new Item(355, 15, 1),
					new List<Item>
					{
						new Item(355, 6, 1),
						new Item(351, 16, 1),
					}),
				new ShapelessRecipe(new Item(355, 15, 1),
					new List<Item>
					{
						new Item(355, 5, 1),
						new Item(351, 16, 1),
					}),
				new ShapelessRecipe(new Item(355, 15, 1),
					new List<Item>
					{
						new Item(355, 4, 1),
						new Item(351, 16, 1),
					}),
				new ShapelessRecipe(new Item(355, 15, 1),
					new List<Item>
					{
						new Item(355, 3, 1),
						new Item(351, 16, 1),
					}),
				new ShapelessRecipe(new Item(355, 15, 1),
					new List<Item>
					{
						new Item(355, 2, 1),
						new Item(351, 16, 1),
					}),
				new ShapelessRecipe(new Item(355, 15, 1),
					new List<Item>
					{
						new Item(355, 1, 1),
						new Item(351, 16, 1),
					}),
				new ShapelessRecipe(new Item(355, 15, 1),
					new List<Item>
					{
						new Item(355, 0, 1),
						new Item(351, 16, 1),
					}),
				new ShapelessRecipe(new Item(355, 0, 1),
					new List<Item>
					{
						new Item(355, 15, 1),
						new Item(351, 15, 1),
					}),
				new ShapelessRecipe(new Item(355, 0, 1),
					new List<Item>
					{
						new Item(355, 14, 1),
						new Item(351, 15, 1),
					}),
				new ShapelessRecipe(new Item(355, 0, 1),
					new List<Item>
					{
						new Item(355, 13, 1),
						new Item(351, 15, 1),
					}),
				new ShapelessRecipe(new Item(355, 0, 1),
					new List<Item>
					{
						new Item(355, 12, 1),
						new Item(351, 15, 1),
					}),
				new ShapelessRecipe(new Item(355, 0, 1),
					new List<Item>
					{
						new Item(355, 11, 1),
						new Item(351, 15, 1),
					}),
				new ShapelessRecipe(new Item(355, 0, 1),
					new List<Item>
					{
						new Item(355, 10, 1),
						new Item(351, 15, 1),
					}),
				new ShapelessRecipe(new Item(355, 0, 1),
					new List<Item>
					{
						new Item(355, 9, 1),
						new Item(351, 15, 1),
					}),
				new ShapelessRecipe(new Item(355, 0, 1),
					new List<Item>
					{
						new Item(355, 8, 1),
						new Item(351, 15, 1),
					}),
				new ShapelessRecipe(new Item(355, 0, 1),
					new List<Item>
					{
						new Item(355, 7, 1),
						new Item(351, 15, 1),
					}),
				new ShapelessRecipe(new Item(355, 0, 1),
					new List<Item>
					{
						new Item(355, 6, 1),
						new Item(351, 15, 1),
					}),
				new ShapelessRecipe(new Item(355, 0, 1),
					new List<Item>
					{
						new Item(355, 5, 1),
						new Item(351, 15, 1),
					}),
				new ShapelessRecipe(new Item(355, 0, 1),
					new List<Item>
					{
						new Item(355, 4, 1),
						new Item(351, 15, 1),
					}),
				new ShapelessRecipe(new Item(355, 0, 1),
					new List<Item>
					{
						new Item(355, 3, 1),
						new Item(351, 15, 1),
					}),
				new ShapelessRecipe(new Item(355, 0, 1),
					new List<Item>
					{
						new Item(355, 2, 1),
						new Item(351, 15, 1),
					}),
				new ShapelessRecipe(new Item(355, 0, 1),
					new List<Item>
					{
						new Item(355, 1, 1),
						new Item(351, 15, 1),
					}),
				new ShapelessRecipe(new Item(355, 1, 1),
					new List<Item>
					{
						new Item(355, 15, 1),
						new Item(351, 14, 1),
					}),
				new ShapelessRecipe(new Item(355, 1, 1),
					new List<Item>
					{
						new Item(355, 14, 1),
						new Item(351, 14, 1),
					}),
				new ShapelessRecipe(new Item(355, 1, 1),
					new List<Item>
					{
						new Item(355, 13, 1),
						new Item(351, 14, 1),
					}),
				new ShapelessRecipe(new Item(355, 1, 1),
					new List<Item>
					{
						new Item(355, 12, 1),
						new Item(351, 14, 1),
					}),
				new ShapelessRecipe(new Item(355, 1, 1),
					new List<Item>
					{
						new Item(355, 11, 1),
						new Item(351, 14, 1),
					}),
				new ShapelessRecipe(new Item(355, 1, 1),
					new List<Item>
					{
						new Item(355, 10, 1),
						new Item(351, 14, 1),
					}),
				new ShapelessRecipe(new Item(355, 1, 1),
					new List<Item>
					{
						new Item(355, 9, 1),
						new Item(351, 14, 1),
					}),
				new ShapelessRecipe(new Item(355, 1, 1),
					new List<Item>
					{
						new Item(355, 8, 1),
						new Item(351, 14, 1),
					}),
				new ShapelessRecipe(new Item(355, 1, 1),
					new List<Item>
					{
						new Item(355, 7, 1),
						new Item(351, 14, 1),
					}),
				new ShapelessRecipe(new Item(355, 1, 1),
					new List<Item>
					{
						new Item(355, 6, 1),
						new Item(351, 14, 1),
					}),
				new ShapelessRecipe(new Item(355, 1, 1),
					new List<Item>
					{
						new Item(355, 5, 1),
						new Item(351, 14, 1),
					}),
				new ShapelessRecipe(new Item(355, 1, 1),
					new List<Item>
					{
						new Item(355, 4, 1),
						new Item(351, 14, 1),
					}),
				new ShapelessRecipe(new Item(355, 1, 1),
					new List<Item>
					{
						new Item(355, 3, 1),
						new Item(351, 14, 1),
					}),
				new ShapelessRecipe(new Item(355, 1, 1),
					new List<Item>
					{
						new Item(355, 2, 1),
						new Item(351, 14, 1),
					}),
				new ShapelessRecipe(new Item(355, 1, 1),
					new List<Item>
					{
						new Item(355, 0, 1),
						new Item(351, 14, 1),
					}),
				new ShapelessRecipe(new Item(355, 2, 1),
					new List<Item>
					{
						new Item(355, 15, 1),
						new Item(351, 13, 1),
					}),
				new ShapelessRecipe(new Item(355, 2, 1),
					new List<Item>
					{
						new Item(355, 14, 1),
						new Item(351, 13, 1),
					}),
				new ShapelessRecipe(new Item(355, 2, 1),
					new List<Item>
					{
						new Item(355, 13, 1),
						new Item(351, 13, 1),
					}),
				new ShapelessRecipe(new Item(355, 2, 1),
					new List<Item>
					{
						new Item(355, 12, 1),
						new Item(351, 13, 1),
					}),
				new ShapelessRecipe(new Item(355, 2, 1),
					new List<Item>
					{
						new Item(355, 11, 1),
						new Item(351, 13, 1),
					}),
				new ShapelessRecipe(new Item(355, 2, 1),
					new List<Item>
					{
						new Item(355, 10, 1),
						new Item(351, 13, 1),
					}),
				new ShapelessRecipe(new Item(355, 2, 1),
					new List<Item>
					{
						new Item(355, 9, 1),
						new Item(351, 13, 1),
					}),
				new ShapelessRecipe(new Item(355, 2, 1),
					new List<Item>
					{
						new Item(355, 8, 1),
						new Item(351, 13, 1),
					}),
				new ShapelessRecipe(new Item(355, 2, 1),
					new List<Item>
					{
						new Item(355, 7, 1),
						new Item(351, 13, 1),
					}),
				new ShapelessRecipe(new Item(355, 2, 1),
					new List<Item>
					{
						new Item(355, 6, 1),
						new Item(351, 13, 1),
					}),
				new ShapelessRecipe(new Item(355, 2, 1),
					new List<Item>
					{
						new Item(355, 5, 1),
						new Item(351, 13, 1),
					}),
				new ShapelessRecipe(new Item(355, 2, 1),
					new List<Item>
					{
						new Item(355, 4, 1),
						new Item(351, 13, 1),
					}),
				new ShapelessRecipe(new Item(355, 2, 1),
					new List<Item>
					{
						new Item(355, 3, 1),
						new Item(351, 13, 1),
					}),
				new ShapelessRecipe(new Item(355, 2, 1),
					new List<Item>
					{
						new Item(355, 1, 1),
						new Item(351, 13, 1),
					}),
				new ShapelessRecipe(new Item(355, 2, 1),
					new List<Item>
					{
						new Item(355, 0, 1),
						new Item(351, 13, 1),
					}),
				new ShapelessRecipe(new Item(355, 3, 1),
					new List<Item>
					{
						new Item(355, 15, 1),
						new Item(351, 12, 1),
					}),
				new ShapelessRecipe(new Item(355, 3, 1),
					new List<Item>
					{
						new Item(355, 14, 1),
						new Item(351, 12, 1),
					}),
				new ShapelessRecipe(new Item(355, 3, 1),
					new List<Item>
					{
						new Item(355, 13, 1),
						new Item(351, 12, 1),
					}),
				new ShapelessRecipe(new Item(355, 3, 1),
					new List<Item>
					{
						new Item(355, 12, 1),
						new Item(351, 12, 1),
					}),
				new ShapelessRecipe(new Item(355, 3, 1),
					new List<Item>
					{
						new Item(355, 11, 1),
						new Item(351, 12, 1),
					}),
				new ShapelessRecipe(new Item(355, 3, 1),
					new List<Item>
					{
						new Item(355, 10, 1),
						new Item(351, 12, 1),
					}),
				new ShapelessRecipe(new Item(355, 3, 1),
					new List<Item>
					{
						new Item(355, 9, 1),
						new Item(351, 12, 1),
					}),
				new ShapelessRecipe(new Item(355, 3, 1),
					new List<Item>
					{
						new Item(355, 8, 1),
						new Item(351, 12, 1),
					}),
				new ShapelessRecipe(new Item(355, 3, 1),
					new List<Item>
					{
						new Item(355, 7, 1),
						new Item(351, 12, 1),
					}),
				new ShapelessRecipe(new Item(355, 3, 1),
					new List<Item>
					{
						new Item(355, 6, 1),
						new Item(351, 12, 1),
					}),
				new ShapelessRecipe(new Item(355, 3, 1),
					new List<Item>
					{
						new Item(355, 5, 1),
						new Item(351, 12, 1),
					}),
				new ShapelessRecipe(new Item(355, 3, 1),
					new List<Item>
					{
						new Item(355, 4, 1),
						new Item(351, 12, 1),
					}),
				new ShapelessRecipe(new Item(355, 3, 1),
					new List<Item>
					{
						new Item(355, 2, 1),
						new Item(351, 12, 1),
					}),
				new ShapelessRecipe(new Item(355, 3, 1),
					new List<Item>
					{
						new Item(355, 1, 1),
						new Item(351, 12, 1),
					}),
				new ShapelessRecipe(new Item(355, 3, 1),
					new List<Item>
					{
						new Item(355, 0, 1),
						new Item(351, 12, 1),
					}),
				new ShapelessRecipe(new Item(355, 4, 1),
					new List<Item>
					{
						new Item(355, 15, 1),
						new Item(351, 11, 1),
					}),
				new ShapelessRecipe(new Item(355, 4, 1),
					new List<Item>
					{
						new Item(355, 14, 1),
						new Item(351, 11, 1),
					}),
				new ShapelessRecipe(new Item(355, 4, 1),
					new List<Item>
					{
						new Item(355, 13, 1),
						new Item(351, 11, 1),
					}),
				new ShapelessRecipe(new Item(355, 4, 1),
					new List<Item>
					{
						new Item(355, 12, 1),
						new Item(351, 11, 1),
					}),
				new ShapelessRecipe(new Item(355, 4, 1),
					new List<Item>
					{
						new Item(355, 11, 1),
						new Item(351, 11, 1),
					}),
				new ShapelessRecipe(new Item(355, 4, 1),
					new List<Item>
					{
						new Item(355, 10, 1),
						new Item(351, 11, 1),
					}),
				new ShapelessRecipe(new Item(355, 4, 1),
					new List<Item>
					{
						new Item(355, 9, 1),
						new Item(351, 11, 1),
					}),
				new ShapelessRecipe(new Item(355, 4, 1),
					new List<Item>
					{
						new Item(355, 8, 1),
						new Item(351, 11, 1),
					}),
				new ShapelessRecipe(new Item(355, 4, 1),
					new List<Item>
					{
						new Item(355, 7, 1),
						new Item(351, 11, 1),
					}),
				new ShapelessRecipe(new Item(355, 4, 1),
					new List<Item>
					{
						new Item(355, 6, 1),
						new Item(351, 11, 1),
					}),
				new ShapelessRecipe(new Item(355, 4, 1),
					new List<Item>
					{
						new Item(355, 5, 1),
						new Item(351, 11, 1),
					}),
				new ShapelessRecipe(new Item(355, 4, 1),
					new List<Item>
					{
						new Item(355, 3, 1),
						new Item(351, 11, 1),
					}),
				new ShapelessRecipe(new Item(355, 4, 1),
					new List<Item>
					{
						new Item(355, 2, 1),
						new Item(351, 11, 1),
					}),
				new ShapelessRecipe(new Item(355, 4, 1),
					new List<Item>
					{
						new Item(355, 1, 1),
						new Item(351, 11, 1),
					}),
				new ShapelessRecipe(new Item(355, 4, 1),
					new List<Item>
					{
						new Item(355, 0, 1),
						new Item(351, 11, 1),
					}),
				new ShapelessRecipe(new Item(355, 5, 1),
					new List<Item>
					{
						new Item(355, 15, 1),
						new Item(351, 10, 1),
					}),
				new ShapelessRecipe(new Item(355, 5, 1),
					new List<Item>
					{
						new Item(355, 14, 1),
						new Item(351, 10, 1),
					}),
				new ShapelessRecipe(new Item(355, 5, 1),
					new List<Item>
					{
						new Item(355, 13, 1),
						new Item(351, 10, 1),
					}),
				new ShapelessRecipe(new Item(355, 5, 1),
					new List<Item>
					{
						new Item(355, 12, 1),
						new Item(351, 10, 1),
					}),
				new ShapelessRecipe(new Item(355, 5, 1),
					new List<Item>
					{
						new Item(355, 11, 1),
						new Item(351, 10, 1),
					}),
				new ShapelessRecipe(new Item(355, 5, 1),
					new List<Item>
					{
						new Item(355, 10, 1),
						new Item(351, 10, 1),
					}),
				new ShapelessRecipe(new Item(355, 5, 1),
					new List<Item>
					{
						new Item(355, 9, 1),
						new Item(351, 10, 1),
					}),
				new ShapelessRecipe(new Item(355, 5, 1),
					new List<Item>
					{
						new Item(355, 8, 1),
						new Item(351, 10, 1),
					}),
				new ShapelessRecipe(new Item(355, 5, 1),
					new List<Item>
					{
						new Item(355, 7, 1),
						new Item(351, 10, 1),
					}),
				new ShapelessRecipe(new Item(355, 5, 1),
					new List<Item>
					{
						new Item(355, 6, 1),
						new Item(351, 10, 1),
					}),
				new ShapelessRecipe(new Item(355, 5, 1),
					new List<Item>
					{
						new Item(355, 4, 1),
						new Item(351, 10, 1),
					}),
				new ShapelessRecipe(new Item(355, 5, 1),
					new List<Item>
					{
						new Item(355, 3, 1),
						new Item(351, 10, 1),
					}),
				new ShapelessRecipe(new Item(355, 5, 1),
					new List<Item>
					{
						new Item(355, 2, 1),
						new Item(351, 10, 1),
					}),
				new ShapelessRecipe(new Item(355, 5, 1),
					new List<Item>
					{
						new Item(355, 1, 1),
						new Item(351, 10, 1),
					}),
				new ShapelessRecipe(new Item(355, 5, 1),
					new List<Item>
					{
						new Item(355, 0, 1),
						new Item(351, 10, 1),
					}),
				new ShapelessRecipe(new Item(355, 6, 1),
					new List<Item>
					{
						new Item(355, 15, 1),
						new Item(351, 9, 1),
					}),
				new ShapelessRecipe(new Item(355, 6, 1),
					new List<Item>
					{
						new Item(355, 14, 1),
						new Item(351, 9, 1),
					}),
				new ShapelessRecipe(new Item(355, 6, 1),
					new List<Item>
					{
						new Item(355, 13, 1),
						new Item(351, 9, 1),
					}),
				new ShapelessRecipe(new Item(355, 6, 1),
					new List<Item>
					{
						new Item(355, 12, 1),
						new Item(351, 9, 1),
					}),
				new ShapelessRecipe(new Item(355, 6, 1),
					new List<Item>
					{
						new Item(355, 11, 1),
						new Item(351, 9, 1),
					}),
				new ShapelessRecipe(new Item(355, 6, 1),
					new List<Item>
					{
						new Item(355, 10, 1),
						new Item(351, 9, 1),
					}),
				new ShapelessRecipe(new Item(355, 6, 1),
					new List<Item>
					{
						new Item(355, 9, 1),
						new Item(351, 9, 1),
					}),
				new ShapelessRecipe(new Item(355, 6, 1),
					new List<Item>
					{
						new Item(355, 8, 1),
						new Item(351, 9, 1),
					}),
				new ShapelessRecipe(new Item(355, 6, 1),
					new List<Item>
					{
						new Item(355, 7, 1),
						new Item(351, 9, 1),
					}),
				new ShapelessRecipe(new Item(355, 6, 1),
					new List<Item>
					{
						new Item(355, 5, 1),
						new Item(351, 9, 1),
					}),
				new ShapelessRecipe(new Item(355, 6, 1),
					new List<Item>
					{
						new Item(355, 4, 1),
						new Item(351, 9, 1),
					}),
				new ShapelessRecipe(new Item(355, 6, 1),
					new List<Item>
					{
						new Item(355, 3, 1),
						new Item(351, 9, 1),
					}),
				new ShapelessRecipe(new Item(355, 6, 1),
					new List<Item>
					{
						new Item(355, 2, 1),
						new Item(351, 9, 1),
					}),
				new ShapelessRecipe(new Item(355, 6, 1),
					new List<Item>
					{
						new Item(355, 1, 1),
						new Item(351, 9, 1),
					}),
				new ShapelessRecipe(new Item(355, 6, 1),
					new List<Item>
					{
						new Item(355, 0, 1),
						new Item(351, 9, 1),
					}),
				new ShapelessRecipe(new Item(355, 7, 1),
					new List<Item>
					{
						new Item(355, 15, 1),
						new Item(351, 8, 1),
					}),
				new ShapelessRecipe(new Item(355, 7, 1),
					new List<Item>
					{
						new Item(355, 14, 1),
						new Item(351, 8, 1),
					}),
				new ShapelessRecipe(new Item(355, 7, 1),
					new List<Item>
					{
						new Item(355, 13, 1),
						new Item(351, 8, 1),
					}),
				new ShapelessRecipe(new Item(355, 7, 1),
					new List<Item>
					{
						new Item(355, 12, 1),
						new Item(351, 8, 1),
					}),
				new ShapelessRecipe(new Item(355, 7, 1),
					new List<Item>
					{
						new Item(355, 11, 1),
						new Item(351, 8, 1),
					}),
				new ShapelessRecipe(new Item(355, 7, 1),
					new List<Item>
					{
						new Item(355, 10, 1),
						new Item(351, 8, 1),
					}),
				new ShapelessRecipe(new Item(355, 7, 1),
					new List<Item>
					{
						new Item(355, 9, 1),
						new Item(351, 8, 1),
					}),
				new ShapelessRecipe(new Item(355, 7, 1),
					new List<Item>
					{
						new Item(355, 8, 1),
						new Item(351, 8, 1),
					}),
				new ShapelessRecipe(new Item(355, 7, 1),
					new List<Item>
					{
						new Item(355, 6, 1),
						new Item(351, 8, 1),
					}),
				new ShapelessRecipe(new Item(355, 7, 1),
					new List<Item>
					{
						new Item(355, 5, 1),
						new Item(351, 8, 1),
					}),
				new ShapelessRecipe(new Item(355, 7, 1),
					new List<Item>
					{
						new Item(355, 4, 1),
						new Item(351, 8, 1),
					}),
				new ShapelessRecipe(new Item(355, 7, 1),
					new List<Item>
					{
						new Item(355, 3, 1),
						new Item(351, 8, 1),
					}),
				new ShapelessRecipe(new Item(355, 7, 1),
					new List<Item>
					{
						new Item(355, 2, 1),
						new Item(351, 8, 1),
					}),
				new ShapelessRecipe(new Item(355, 7, 1),
					new List<Item>
					{
						new Item(355, 1, 1),
						new Item(351, 8, 1),
					}),
				new ShapelessRecipe(new Item(355, 7, 1),
					new List<Item>
					{
						new Item(355, 0, 1),
						new Item(351, 8, 1),
					}),
				new ShapelessRecipe(new Item(355, 8, 1),
					new List<Item>
					{
						new Item(355, 15, 1),
						new Item(351, 7, 1),
					}),
				new ShapelessRecipe(new Item(355, 8, 1),
					new List<Item>
					{
						new Item(355, 14, 1),
						new Item(351, 7, 1),
					}),
				new ShapelessRecipe(new Item(355, 8, 1),
					new List<Item>
					{
						new Item(355, 13, 1),
						new Item(351, 7, 1),
					}),
				new ShapelessRecipe(new Item(355, 8, 1),
					new List<Item>
					{
						new Item(355, 12, 1),
						new Item(351, 7, 1),
					}),
				new ShapelessRecipe(new Item(355, 8, 1),
					new List<Item>
					{
						new Item(355, 11, 1),
						new Item(351, 7, 1),
					}),
				new ShapelessRecipe(new Item(355, 8, 1),
					new List<Item>
					{
						new Item(355, 10, 1),
						new Item(351, 7, 1),
					}),
				new ShapelessRecipe(new Item(355, 8, 1),
					new List<Item>
					{
						new Item(355, 9, 1),
						new Item(351, 7, 1),
					}),
				new ShapelessRecipe(new Item(355, 8, 1),
					new List<Item>
					{
						new Item(355, 7, 1),
						new Item(351, 7, 1),
					}),
				new ShapelessRecipe(new Item(355, 8, 1),
					new List<Item>
					{
						new Item(355, 6, 1),
						new Item(351, 7, 1),
					}),
				new ShapelessRecipe(new Item(355, 8, 1),
					new List<Item>
					{
						new Item(355, 5, 1),
						new Item(351, 7, 1),
					}),
				new ShapelessRecipe(new Item(355, 8, 1),
					new List<Item>
					{
						new Item(355, 4, 1),
						new Item(351, 7, 1),
					}),
				new ShapelessRecipe(new Item(355, 8, 1),
					new List<Item>
					{
						new Item(355, 3, 1),
						new Item(351, 7, 1),
					}),
				new ShapelessRecipe(new Item(355, 8, 1),
					new List<Item>
					{
						new Item(355, 2, 1),
						new Item(351, 7, 1),
					}),
				new ShapelessRecipe(new Item(355, 8, 1),
					new List<Item>
					{
						new Item(355, 1, 1),
						new Item(351, 7, 1),
					}),
				new ShapelessRecipe(new Item(355, 8, 1),
					new List<Item>
					{
						new Item(355, 0, 1),
						new Item(351, 7, 1),
					}),
				new ShapelessRecipe(new Item(355, 9, 1),
					new List<Item>
					{
						new Item(355, 15, 1),
						new Item(351, 6, 1),
					}),
				new ShapelessRecipe(new Item(355, 9, 1),
					new List<Item>
					{
						new Item(355, 14, 1),
						new Item(351, 6, 1),
					}),
				new ShapelessRecipe(new Item(355, 9, 1),
					new List<Item>
					{
						new Item(355, 13, 1),
						new Item(351, 6, 1),
					}),
				new ShapelessRecipe(new Item(355, 9, 1),
					new List<Item>
					{
						new Item(355, 12, 1),
						new Item(351, 6, 1),
					}),
				new ShapelessRecipe(new Item(355, 9, 1),
					new List<Item>
					{
						new Item(355, 11, 1),
						new Item(351, 6, 1),
					}),
				new ShapelessRecipe(new Item(355, 9, 1),
					new List<Item>
					{
						new Item(355, 10, 1),
						new Item(351, 6, 1),
					}),
				new ShapelessRecipe(new Item(355, 9, 1),
					new List<Item>
					{
						new Item(355, 8, 1),
						new Item(351, 6, 1),
					}),
				new ShapelessRecipe(new Item(355, 9, 1),
					new List<Item>
					{
						new Item(355, 7, 1),
						new Item(351, 6, 1),
					}),
				new ShapelessRecipe(new Item(355, 9, 1),
					new List<Item>
					{
						new Item(355, 6, 1),
						new Item(351, 6, 1),
					}),
				new ShapelessRecipe(new Item(355, 9, 1),
					new List<Item>
					{
						new Item(355, 5, 1),
						new Item(351, 6, 1),
					}),
				new ShapelessRecipe(new Item(355, 9, 1),
					new List<Item>
					{
						new Item(355, 4, 1),
						new Item(351, 6, 1),
					}),
				new ShapelessRecipe(new Item(355, 9, 1),
					new List<Item>
					{
						new Item(355, 3, 1),
						new Item(351, 6, 1),
					}),
				new ShapelessRecipe(new Item(355, 9, 1),
					new List<Item>
					{
						new Item(355, 2, 1),
						new Item(351, 6, 1),
					}),
				new ShapelessRecipe(new Item(355, 9, 1),
					new List<Item>
					{
						new Item(355, 1, 1),
						new Item(351, 6, 1),
					}),
				new ShapelessRecipe(new Item(355, 9, 1),
					new List<Item>
					{
						new Item(355, 0, 1),
						new Item(351, 6, 1),
					}),
				new ShapelessRecipe(new Item(355, 10, 1),
					new List<Item>
					{
						new Item(355, 15, 1),
						new Item(351, 5, 1),
					}),
				new ShapelessRecipe(new Item(355, 10, 1),
					new List<Item>
					{
						new Item(355, 14, 1),
						new Item(351, 5, 1),
					}),
				new ShapelessRecipe(new Item(355, 10, 1),
					new List<Item>
					{
						new Item(355, 13, 1),
						new Item(351, 5, 1),
					}),
				new ShapelessRecipe(new Item(355, 10, 1),
					new List<Item>
					{
						new Item(355, 12, 1),
						new Item(351, 5, 1),
					}),
				new ShapelessRecipe(new Item(355, 10, 1),
					new List<Item>
					{
						new Item(355, 11, 1),
						new Item(351, 5, 1),
					}),
				new ShapelessRecipe(new Item(355, 10, 1),
					new List<Item>
					{
						new Item(355, 9, 1),
						new Item(351, 5, 1),
					}),
				new ShapelessRecipe(new Item(355, 10, 1),
					new List<Item>
					{
						new Item(355, 8, 1),
						new Item(351, 5, 1),
					}),
				new ShapelessRecipe(new Item(355, 10, 1),
					new List<Item>
					{
						new Item(355, 7, 1),
						new Item(351, 5, 1),
					}),
				new ShapelessRecipe(new Item(355, 10, 1),
					new List<Item>
					{
						new Item(355, 6, 1),
						new Item(351, 5, 1),
					}),
				new ShapelessRecipe(new Item(355, 10, 1),
					new List<Item>
					{
						new Item(355, 5, 1),
						new Item(351, 5, 1),
					}),
				new ShapelessRecipe(new Item(355, 10, 1),
					new List<Item>
					{
						new Item(355, 4, 1),
						new Item(351, 5, 1),
					}),
				new ShapelessRecipe(new Item(355, 10, 1),
					new List<Item>
					{
						new Item(355, 3, 1),
						new Item(351, 5, 1),
					}),
				new ShapelessRecipe(new Item(355, 10, 1),
					new List<Item>
					{
						new Item(355, 2, 1),
						new Item(351, 5, 1),
					}),
				new ShapelessRecipe(new Item(355, 10, 1),
					new List<Item>
					{
						new Item(355, 1, 1),
						new Item(351, 5, 1),
					}),
				new ShapelessRecipe(new Item(355, 10, 1),
					new List<Item>
					{
						new Item(355, 0, 1),
						new Item(351, 5, 1),
					}),
				new ShapelessRecipe(new Item(355, 11, 1),
					new List<Item>
					{
						new Item(355, 15, 1),
						new Item(351, 4, 1),
					}),
				new ShapelessRecipe(new Item(355, 11, 1),
					new List<Item>
					{
						new Item(355, 14, 1),
						new Item(351, 4, 1),
					}),
				new ShapelessRecipe(new Item(355, 11, 1),
					new List<Item>
					{
						new Item(355, 13, 1),
						new Item(351, 4, 1),
					}),
				new ShapelessRecipe(new Item(355, 11, 1),
					new List<Item>
					{
						new Item(355, 12, 1),
						new Item(351, 4, 1),
					}),
				new ShapelessRecipe(new Item(355, 11, 1),
					new List<Item>
					{
						new Item(355, 10, 1),
						new Item(351, 4, 1),
					}),
				new ShapelessRecipe(new Item(355, 11, 1),
					new List<Item>
					{
						new Item(355, 9, 1),
						new Item(351, 4, 1),
					}),
				new ShapelessRecipe(new Item(355, 11, 1),
					new List<Item>
					{
						new Item(355, 8, 1),
						new Item(351, 4, 1),
					}),
				new ShapelessRecipe(new Item(355, 11, 1),
					new List<Item>
					{
						new Item(355, 7, 1),
						new Item(351, 4, 1),
					}),
				new ShapelessRecipe(new Item(355, 11, 1),
					new List<Item>
					{
						new Item(355, 6, 1),
						new Item(351, 4, 1),
					}),
				new ShapelessRecipe(new Item(355, 11, 1),
					new List<Item>
					{
						new Item(355, 5, 1),
						new Item(351, 4, 1),
					}),
				new ShapelessRecipe(new Item(355, 11, 1),
					new List<Item>
					{
						new Item(355, 4, 1),
						new Item(351, 4, 1),
					}),
				new ShapelessRecipe(new Item(355, 11, 1),
					new List<Item>
					{
						new Item(355, 3, 1),
						new Item(351, 4, 1),
					}),
				new ShapelessRecipe(new Item(355, 11, 1),
					new List<Item>
					{
						new Item(355, 2, 1),
						new Item(351, 4, 1),
					}),
				new ShapelessRecipe(new Item(355, 11, 1),
					new List<Item>
					{
						new Item(355, 1, 1),
						new Item(351, 4, 1),
					}),
				new ShapelessRecipe(new Item(355, 11, 1),
					new List<Item>
					{
						new Item(355, 0, 1),
						new Item(351, 4, 1),
					}),
				new ShapelessRecipe(new Item(355, 12, 1),
					new List<Item>
					{
						new Item(355, 15, 1),
						new Item(351, 3, 1),
					}),
				new ShapelessRecipe(new Item(355, 12, 1),
					new List<Item>
					{
						new Item(355, 14, 1),
						new Item(351, 3, 1),
					}),
				new ShapelessRecipe(new Item(355, 12, 1),
					new List<Item>
					{
						new Item(355, 13, 1),
						new Item(351, 3, 1),
					}),
				new ShapelessRecipe(new Item(355, 12, 1),
					new List<Item>
					{
						new Item(355, 11, 1),
						new Item(351, 3, 1),
					}),
				new ShapelessRecipe(new Item(355, 12, 1),
					new List<Item>
					{
						new Item(355, 10, 1),
						new Item(351, 3, 1),
					}),
				new ShapelessRecipe(new Item(355, 12, 1),
					new List<Item>
					{
						new Item(355, 9, 1),
						new Item(351, 3, 1),
					}),
				new ShapelessRecipe(new Item(355, 12, 1),
					new List<Item>
					{
						new Item(355, 8, 1),
						new Item(351, 3, 1),
					}),
				new ShapelessRecipe(new Item(355, 12, 1),
					new List<Item>
					{
						new Item(355, 7, 1),
						new Item(351, 3, 1),
					}),
				new ShapelessRecipe(new Item(355, 12, 1),
					new List<Item>
					{
						new Item(355, 6, 1),
						new Item(351, 3, 1),
					}),
				new ShapelessRecipe(new Item(355, 12, 1),
					new List<Item>
					{
						new Item(355, 5, 1),
						new Item(351, 3, 1),
					}),
				new ShapelessRecipe(new Item(355, 12, 1),
					new List<Item>
					{
						new Item(355, 4, 1),
						new Item(351, 3, 1),
					}),
				new ShapelessRecipe(new Item(355, 12, 1),
					new List<Item>
					{
						new Item(355, 3, 1),
						new Item(351, 3, 1),
					}),
				new ShapelessRecipe(new Item(355, 12, 1),
					new List<Item>
					{
						new Item(355, 2, 1),
						new Item(351, 3, 1),
					}),
				new ShapelessRecipe(new Item(355, 12, 1),
					new List<Item>
					{
						new Item(355, 1, 1),
						new Item(351, 3, 1),
					}),
				new ShapelessRecipe(new Item(355, 12, 1),
					new List<Item>
					{
						new Item(355, 0, 1),
						new Item(351, 3, 1),
					}),
				new ShapelessRecipe(new Item(355, 13, 1),
					new List<Item>
					{
						new Item(355, 15, 1),
						new Item(351, 2, 1),
					}),
				new ShapelessRecipe(new Item(355, 13, 1),
					new List<Item>
					{
						new Item(355, 14, 1),
						new Item(351, 2, 1),
					}),
				new ShapelessRecipe(new Item(355, 13, 1),
					new List<Item>
					{
						new Item(355, 12, 1),
						new Item(351, 2, 1),
					}),
				new ShapelessRecipe(new Item(355, 13, 1),
					new List<Item>
					{
						new Item(355, 11, 1),
						new Item(351, 2, 1),
					}),
				new ShapelessRecipe(new Item(355, 13, 1),
					new List<Item>
					{
						new Item(355, 10, 1),
						new Item(351, 2, 1),
					}),
				new ShapelessRecipe(new Item(355, 13, 1),
					new List<Item>
					{
						new Item(355, 9, 1),
						new Item(351, 2, 1),
					}),
				new ShapelessRecipe(new Item(355, 13, 1),
					new List<Item>
					{
						new Item(355, 8, 1),
						new Item(351, 2, 1),
					}),
				new ShapelessRecipe(new Item(355, 13, 1),
					new List<Item>
					{
						new Item(355, 7, 1),
						new Item(351, 2, 1),
					}),
				new ShapelessRecipe(new Item(355, 13, 1),
					new List<Item>
					{
						new Item(355, 6, 1),
						new Item(351, 2, 1),
					}),
				new ShapelessRecipe(new Item(355, 13, 1),
					new List<Item>
					{
						new Item(355, 5, 1),
						new Item(351, 2, 1),
					}),
				new ShapelessRecipe(new Item(355, 13, 1),
					new List<Item>
					{
						new Item(355, 4, 1),
						new Item(351, 2, 1),
					}),
				new ShapelessRecipe(new Item(355, 13, 1),
					new List<Item>
					{
						new Item(355, 3, 1),
						new Item(351, 2, 1),
					}),
				new ShapelessRecipe(new Item(355, 13, 1),
					new List<Item>
					{
						new Item(355, 2, 1),
						new Item(351, 2, 1),
					}),
				new ShapelessRecipe(new Item(355, 13, 1),
					new List<Item>
					{
						new Item(355, 1, 1),
						new Item(351, 2, 1),
					}),
				new ShapelessRecipe(new Item(355, 13, 1),
					new List<Item>
					{
						new Item(355, 0, 1),
						new Item(351, 2, 1),
					}),
				new ShapelessRecipe(new Item(355, 14, 1),
					new List<Item>
					{
						new Item(355, 15, 1),
						new Item(351, 1, 1),
					}),
				new ShapelessRecipe(new Item(355, 14, 1),
					new List<Item>
					{
						new Item(355, 13, 1),
						new Item(351, 1, 1),
					}),
				new ShapelessRecipe(new Item(355, 14, 1),
					new List<Item>
					{
						new Item(355, 12, 1),
						new Item(351, 1, 1),
					}),
				new ShapelessRecipe(new Item(355, 14, 1),
					new List<Item>
					{
						new Item(355, 11, 1),
						new Item(351, 1, 1),
					}),
				new ShapelessRecipe(new Item(355, 14, 1),
					new List<Item>
					{
						new Item(355, 10, 1),
						new Item(351, 1, 1),
					}),
				new ShapelessRecipe(new Item(355, 14, 1),
					new List<Item>
					{
						new Item(355, 9, 1),
						new Item(351, 1, 1),
					}),
				new ShapelessRecipe(new Item(355, 14, 1),
					new List<Item>
					{
						new Item(355, 8, 1),
						new Item(351, 1, 1),
					}),
				new ShapelessRecipe(new Item(355, 14, 1),
					new List<Item>
					{
						new Item(355, 7, 1),
						new Item(351, 1, 1),
					}),
				new ShapelessRecipe(new Item(355, 14, 1),
					new List<Item>
					{
						new Item(355, 6, 1),
						new Item(351, 1, 1),
					}),
				new ShapelessRecipe(new Item(355, 14, 1),
					new List<Item>
					{
						new Item(355, 5, 1),
						new Item(351, 1, 1),
					}),
				new ShapelessRecipe(new Item(355, 14, 1),
					new List<Item>
					{
						new Item(355, 4, 1),
						new Item(351, 1, 1),
					}),
				new ShapelessRecipe(new Item(355, 14, 1),
					new List<Item>
					{
						new Item(355, 3, 1),
						new Item(351, 1, 1),
					}),
				new ShapelessRecipe(new Item(355, 14, 1),
					new List<Item>
					{
						new Item(355, 2, 1),
						new Item(351, 1, 1),
					}),
				new ShapelessRecipe(new Item(355, 14, 1),
					new List<Item>
					{
						new Item(355, 1, 1),
						new Item(351, 1, 1),
					}),
				new ShapelessRecipe(new Item(355, 14, 1),
					new List<Item>
					{
						new Item(355, 0, 1),
						new Item(351, 1, 1),
					}),
				new ShapelessRecipe(new Item(355, 15, 1),
					new List<Item>
					{
						new Item(355, 14, 1),
						new Item(351, 0, 1),
					}),
				new ShapelessRecipe(new Item(355, 15, 1),
					new List<Item>
					{
						new Item(355, 13, 1),
						new Item(351, 0, 1),
					}),
				new ShapelessRecipe(new Item(355, 15, 1),
					new List<Item>
					{
						new Item(355, 12, 1),
						new Item(351, 0, 1),
					}),
				new ShapelessRecipe(new Item(355, 15, 1),
					new List<Item>
					{
						new Item(355, 11, 1),
						new Item(351, 0, 1),
					}),
				new ShapelessRecipe(new Item(355, 15, 1),
					new List<Item>
					{
						new Item(355, 10, 1),
						new Item(351, 0, 1),
					}),
				new ShapelessRecipe(new Item(355, 15, 1),
					new List<Item>
					{
						new Item(355, 9, 1),
						new Item(351, 0, 1),
					}),
				new ShapelessRecipe(new Item(355, 15, 1),
					new List<Item>
					{
						new Item(355, 8, 1),
						new Item(351, 0, 1),
					}),
				new ShapelessRecipe(new Item(355, 15, 1),
					new List<Item>
					{
						new Item(355, 7, 1),
						new Item(351, 0, 1),
					}),
				new ShapelessRecipe(new Item(355, 15, 1),
					new List<Item>
					{
						new Item(355, 6, 1),
						new Item(351, 0, 1),
					}),
				new ShapelessRecipe(new Item(355, 15, 1),
					new List<Item>
					{
						new Item(355, 5, 1),
						new Item(351, 0, 1),
					}),
				new ShapelessRecipe(new Item(355, 15, 1),
					new List<Item>
					{
						new Item(355, 4, 1),
						new Item(351, 0, 1),
					}),
				new ShapelessRecipe(new Item(355, 15, 1),
					new List<Item>
					{
						new Item(355, 3, 1),
						new Item(351, 0, 1),
					}),
				new ShapelessRecipe(new Item(355, 15, 1),
					new List<Item>
					{
						new Item(355, 2, 1),
						new Item(351, 0, 1),
					}),
				new ShapelessRecipe(new Item(355, 15, 1),
					new List<Item>
					{
						new Item(355, 1, 1),
						new Item(351, 0, 1),
					}),
				new ShapelessRecipe(new Item(355, 15, 1),
					new List<Item>
					{
						new Item(355, 0, 1),
						new Item(351, 0, 1),
					}),
				new ShapedRecipe(3, 3, new Item(54, 0, 1),
					new Item[]
					{
						new Item(5, -1),
						new Item(5, -1),
						new Item(5, -1),
						new Item(5, -1),
						new Item(0, 0),
						new Item(5, -1),
						new Item(5, -1),
						new Item(5, -1),
						new Item(5, -1),
					}),
				new ShapedRecipe(2, 2, new Item(58, 0, 1),
					new Item[]
					{
						new Item(5, -1),
						new Item(5, -1),
						new Item(5, -1),
						new Item(5, -1),
					}),
				new ShapedRecipe(3, 2, new Item(281, 0, 4),
					new Item[]
					{
						new Item(5, -1),
						new Item(5, -1),
						new Item(5, -1),
						new Item(0, 0),
						new Item(0, 0),
						new Item(0, 0),
					}),
				new ShapedRecipe(3, 3, new Item(151, 0, 1),
					new Item[]
					{
						new Item(20, -1),
						new Item(406, 0),
						new Item(158, -1),
						new Item(20, -1),
						new Item(406, 0),
						new Item(158, -1),
						new Item(20, -1),
						new Item(406, 0),
						new Item(158, -1),
					}),
				new ShapedRecipe(3, 3, new Item(84, 0, 1),
					new Item[]
					{
						new Item(5, -1),
						new Item(5, -1),
						new Item(5, -1),
						new Item(5, -1),
						new Item(264, 0),
						new Item(5, -1),
						new Item(5, -1),
						new Item(5, -1),
						new Item(5, -1),
					}),
				new ShapedRecipe(3, 3, new Item(25, 0, 1),
					new Item[]
					{
						new Item(5, -1),
						new Item(5, -1),
						new Item(5, -1),
						new Item(5, -1),
						new Item(331, 0),
						new Item(5, -1),
						new Item(5, -1),
						new Item(5, -1),
						new Item(5, -1),
					}),
				new ShapedRecipe(3, 3, new Item(321, 0, 1),
					new Item[]
					{
						new Item(280, 0),
						new Item(280, 0),
						new Item(280, 0),
						new Item(280, 0),
						new Item(35, -1),
						new Item(280, 0),
						new Item(280, 0),
						new Item(280, 0),
						new Item(280, 0),
					}),
				new ShapedRecipe(3, 3, new Item(33, 1, 1),
					new Item[]
					{
						new Item(5, -1),
						new Item(4, -1),
						new Item(4, -1),
						new Item(5, -1),
						new Item(265, 0),
						new Item(331, 0),
						new Item(5, -1),
						new Item(4, -1),
						new Item(4, -1),
					}),
				new ShapedRecipe(1, 2, new Item(280, 0, 1),
					new Item[]
					{
						new Item(-163, -1),
						new Item(-163, -1),
					}),
				new ShapedRecipe(1, 2, new Item(280, 0, 4),
					new Item[]
					{
						new Item(5, -1),
						new Item(5, -1),
					}),
				new ShapedRecipe(3, 3, new Item(47, 0, 1),
					new Item[]
					{
						new Item(5, -1),
						new Item(340, 0),
						new Item(5, -1),
						new Item(5, -1),
						new Item(340, 0),
						new Item(5, -1),
						new Item(5, -1),
						new Item(340, 0),
						new Item(5, -1),
					}),
				new ShapedRecipe(1, 1, new Item(143, 5, 1),
					new Item[]
					{
						new Item(5, 0),
					}),
				new ShapedRecipe(2, 1, new Item(72, 0, 1),
					new Item[]
					{
						new Item(5, 0),
						new Item(5, 0),
					}),
				new ShapedRecipe(1, 1, new Item(-140, 5, 1),
					new Item[]
					{
						new Item(5, 4),
					}),
				new ShapedRecipe(2, 1, new Item(-150, 0, 1),
					new Item[]
					{
						new Item(5, 4),
						new Item(5, 4),
					}),
				new ShapedRecipe(1, 1, new Item(-141, 5, 1),
					new Item[]
					{
						new Item(5, 2),
					}),
				new ShapedRecipe(2, 1, new Item(-151, 0, 1),
					new Item[]
					{
						new Item(5, 2),
						new Item(5, 2),
					}),
				new ShapedRecipe(1, 1, new Item(-142, 5, 1),
					new Item[]
					{
						new Item(5, 5),
					}),
				new ShapedRecipe(2, 1, new Item(-152, 0, 1),
					new Item[]
					{
						new Item(5, 5),
						new Item(5, 5),
					}),
				new ShapedRecipe(1, 1, new Item(-143, 5, 1),
					new Item[]
					{
						new Item(5, 3),
					}),
				new ShapedRecipe(2, 1, new Item(-153, 0, 1),
					new Item[]
					{
						new Item(5, 3),
						new Item(5, 3),
					}),
				new ShapedRecipe(1, 1, new Item(-144, 5, 1),
					new Item[]
					{
						new Item(5, 1),
					}),
				new ShapedRecipe(2, 1, new Item(-154, 0, 1),
					new Item[]
					{
						new Item(5, 1),
						new Item(5, 1),
					}),
				new ShapedRecipe(1, 3, new Item(131, 0, 2),
					new Item[]
					{
						new Item(265, 0),
						new Item(280, 0),
						new Item(5, -1),
					}),
				new ShapedRecipe(3, 2, new Item(96, 0, 2),
					new Item[]
					{
						new Item(5, 0),
						new Item(5, 0),
						new Item(5, 0),
						new Item(5, 0),
						new Item(5, 0),
						new Item(5, 0),
					}),
				new ShapedRecipe(3, 2, new Item(-145, 0, 2),
					new Item[]
					{
						new Item(5, 4),
						new Item(5, 4),
						new Item(5, 4),
						new Item(5, 4),
						new Item(5, 4),
						new Item(5, 4),
					}),
				new ShapedRecipe(3, 2, new Item(-146, 0, 2),
					new Item[]
					{
						new Item(5, 2),
						new Item(5, 2),
						new Item(5, 2),
						new Item(5, 2),
						new Item(5, 2),
						new Item(5, 2),
					}),
				new ShapedRecipe(3, 2, new Item(-147, 0, 2),
					new Item[]
					{
						new Item(5, 5),
						new Item(5, 5),
						new Item(5, 5),
						new Item(5, 5),
						new Item(5, 5),
						new Item(5, 5),
					}),
				new ShapedRecipe(3, 2, new Item(-148, 0, 2),
					new Item[]
					{
						new Item(5, 3),
						new Item(5, 3),
						new Item(5, 3),
						new Item(5, 3),
						new Item(5, 3),
						new Item(5, 3),
					}),
				new ShapedRecipe(3, 2, new Item(-149, 0, 2),
					new Item[]
					{
						new Item(5, 1),
						new Item(5, 1),
						new Item(5, 1),
						new Item(5, 1),
						new Item(5, 1),
						new Item(5, 1),
					}),
				new ShapedRecipe(1, 2, new Item(50, 0, 4),
					new Item[]
					{
						new Item(263, -1),
						new Item(280, 0),
					}),
				new ShapelessRecipe(new Item(385, 0, 3),
					new List<Item>
					{
						new Item(377, 0, 1),
						new Item(263, 0, 1),
						new Item(289, 0, 1),
					}),
				new ShapelessRecipe(new Item(385, 0, 3),
					new List<Item>
					{
						new Item(377, 0, 1),
						new Item(263, 1, 1),
						new Item(289, 0, 1),
					}),
				new ShapedRecipe(3, 3, new Item(46, 0, 1),
					new Item[]
					{
						new Item(289, 0),
						new Item(12, -1),
						new Item(289, 0),
						new Item(12, -1),
						new Item(289, 0),
						new Item(12, -1),
						new Item(289, 0),
						new Item(12, -1),
						new Item(289, 0),
					}),
				new ShapedRecipe(3, 1, new Item(-166, 2, 6),
					new Item[]
					{
						new Item(1, 0),
						new Item(1, 0),
						new Item(1, 0),
					}),
				new ShapedRecipe(3, 1, new Item(44, 0, 6),
					new Item[]
					{
						new Item(-183, -1),
						new Item(-183, -1),
						new Item(-183, -1),
					}),
				new ShapedRecipe(3, 1, new Item(-166, 0, 6),
					new Item[]
					{
						new Item(98, 1),
						new Item(98, 1),
						new Item(98, 1),
					}),
				new ShapedRecipe(3, 1, new Item(44, 5, 6),
					new Item[]
					{
						new Item(98, -1),
						new Item(98, -1),
						new Item(98, -1),
					}),
				new ShapedRecipe(3, 1, new Item(44, 4, 6),
					new Item[]
					{
						new Item(45, -1),
						new Item(45, -1),
						new Item(45, -1),
					}),
				new ShapedRecipe(3, 1, new Item(44, 3, 6),
					new Item[]
					{
						new Item(4, -1),
						new Item(4, -1),
						new Item(4, -1),
					}),
				new ShapedRecipe(3, 1, new Item(44, 7, 6),
					new Item[]
					{
						new Item(112, -1),
						new Item(112, -1),
						new Item(112, -1),
					}),
				new ShapedRecipe(3, 1, new Item(44, 1, 6),
					new Item[]
					{
						new Item(24, 0),
						new Item(24, 0),
						new Item(24, 0),
					}),
				new ShapedRecipe(3, 1, new Item(182, 0, 6),
					new Item[]
					{
						new Item(179, 0),
						new Item(179, 0),
						new Item(179, 0),
					}),
				new ShapedRecipe(3, 1, new Item(44, 1, 6),
					new Item[]
					{
						new Item(24, 1),
						new Item(24, 1),
						new Item(24, 1),
					}),
				new ShapedRecipe(3, 1, new Item(182, 0, 6),
					new Item[]
					{
						new Item(179, 1),
						new Item(179, 1),
						new Item(179, 1),
					}),
				new ShapedRecipe(3, 1, new Item(182, 1, 6),
					new Item[]
					{
						new Item(201, -1),
						new Item(201, -1),
						new Item(201, -1),
					}),
				new ShapedRecipe(3, 1, new Item(-166, 1, 6),
					new Item[]
					{
						new Item(155, 3),
						new Item(155, 3),
						new Item(155, 3),
					}),
				new ShapedRecipe(3, 1, new Item(44, 6, 6),
					new Item[]
					{
						new Item(155, -1),
						new Item(155, -1),
						new Item(155, -1),
					}),
				new ShapedRecipe(3, 1, new Item(182, 2, 6),
					new Item[]
					{
						new Item(168, 0),
						new Item(168, 0),
						new Item(168, 0),
					}),
				new ShapedRecipe(3, 1, new Item(182, 3, 6),
					new Item[]
					{
						new Item(168, 1),
						new Item(168, 1),
						new Item(168, 1),
					}),
				new ShapedRecipe(3, 1, new Item(182, 4, 6),
					new Item[]
					{
						new Item(168, 2),
						new Item(168, 2),
						new Item(168, 2),
					}),
				new ShapedRecipe(3, 1, new Item(182, 5, 6),
					new Item[]
					{
						new Item(48, -1),
						new Item(48, -1),
						new Item(48, -1),
					}),
				new ShapedRecipe(3, 1, new Item(182, 6, 6),
					new Item[]
					{
						new Item(24, 3),
						new Item(24, 3),
						new Item(24, 3),
					}),
				new ShapedRecipe(3, 1, new Item(182, 7, 6),
					new Item[]
					{
						new Item(215, -1),
						new Item(215, -1),
						new Item(215, -1),
					}),
				new ShapedRecipe(3, 1, new Item(-162, 0, 6),
					new Item[]
					{
						new Item(206, -1),
						new Item(206, -1),
						new Item(206, -1),
					}),
				new ShapedRecipe(3, 1, new Item(-162, 1, 6),
					new Item[]
					{
						new Item(179, 3),
						new Item(179, 3),
						new Item(179, 3),
					}),
				new ShapedRecipe(3, 1, new Item(-162, 2, 6),
					new Item[]
					{
						new Item(1, 6),
						new Item(1, 6),
						new Item(1, 6),
					}),
				new ShapedRecipe(3, 1, new Item(-162, 3, 6),
					new Item[]
					{
						new Item(1, 5),
						new Item(1, 5),
						new Item(1, 5),
					}),
				new ShapedRecipe(3, 1, new Item(-162, 4, 6),
					new Item[]
					{
						new Item(1, 3),
						new Item(1, 3),
						new Item(1, 3),
					}),
				new ShapedRecipe(3, 1, new Item(-162, 5, 6),
					new Item[]
					{
						new Item(1, 4),
						new Item(1, 4),
						new Item(1, 4),
					}),
				new ShapedRecipe(3, 1, new Item(-162, 6, 6),
					new Item[]
					{
						new Item(1, 1),
						new Item(1, 1),
						new Item(1, 1),
					}),
				new ShapedRecipe(3, 1, new Item(-162, 7, 6),
					new Item[]
					{
						new Item(1, 2),
						new Item(1, 2),
						new Item(1, 2),
					}),
				new ShapedRecipe(3, 1, new Item(-166, 3, 6),
					new Item[]
					{
						new Item(24, 2),
						new Item(24, 2),
						new Item(24, 2),
					}),
				new ShapedRecipe(3, 1, new Item(-166, 4, 6),
					new Item[]
					{
						new Item(179, 2),
						new Item(179, 2),
						new Item(179, 2),
					}),
				new ShapedRecipe(3, 3, new Item(53, 0, 4),
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
					}),
				new ShapedRecipe(3, 3, new Item(134, 0, 4),
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
					}),
				new ShapedRecipe(3, 3, new Item(135, 0, 4),
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
					}),
				new ShapedRecipe(3, 3, new Item(136, 0, 4),
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
					}),
				new ShapedRecipe(3, 3, new Item(163, 0, 4),
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
					}),
				new ShapedRecipe(3, 3, new Item(164, 0, 4),
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
					}),
				new ShapedRecipe(1, 2, new Item(155, 1, 1),
					new Item[]
					{
						new Item(44, 6),
						new Item(44, 6),
					}),
				new ShapedRecipe(1, 2, new Item(179, 1, 1),
					new Item[]
					{
						new Item(182, 0),
						new Item(182, 0),
					}),
				new ShapedRecipe(1, 2, new Item(24, 1, 1),
					new Item[]
					{
						new Item(44, 1),
						new Item(44, 1),
					}),
				new ShapedRecipe(1, 2, new Item(98, 3, 1),
					new Item[]
					{
						new Item(44, 5),
						new Item(44, 5),
					}),
				new ShapedRecipe(1, 2, new Item(201, 2, 1),
					new Item[]
					{
						new Item(182, 1),
						new Item(182, 1),
					}),
				new ShapelessRecipe(new Item(401, 0, 3),
					new List<Item>
					{
						new Item(339, 0, 1),
						new Item(289, 0, 1),
					}),
				new ShapelessRecipe(new Item(402, 15, 1),
					new List<Item>
					{
						new Item(289, 0, 1),
						new Item(351, 19, 1),
					}),
				new ShapelessRecipe(new Item(401, 0, 3),
					new List<Item>
					{
						new Item(339, 0, 1),
						new Item(289, 0, 1),
						new Item(402, 15, 1),
					}),
				new ShapelessRecipe(new Item(402, 4, 1),
					new List<Item>
					{
						new Item(289, 0, 1),
						new Item(351, 18, 1),
					}),
				new ShapelessRecipe(new Item(401, 0, 3),
					new List<Item>
					{
						new Item(339, 0, 1),
						new Item(289, 0, 1),
						new Item(402, 4, 1),
					}),
				new ShapelessRecipe(new Item(402, 3, 1),
					new List<Item>
					{
						new Item(289, 0, 1),
						new Item(351, 17, 1),
					}),
				new ShapelessRecipe(new Item(401, 0, 3),
					new List<Item>
					{
						new Item(339, 0, 1),
						new Item(289, 0, 1),
						new Item(402, 3, 1),
					}),
				new ShapelessRecipe(new Item(402, 0, 1),
					new List<Item>
					{
						new Item(289, 0, 1),
						new Item(351, 16, 1),
					}),
				new ShapelessRecipe(new Item(401, 0, 3),
					new List<Item>
					{
						new Item(339, 0, 1),
						new Item(289, 0, 1),
						new Item(402, 0, 1),
					}),
				new ShapelessRecipe(new Item(402, 15, 1),
					new List<Item>
					{
						new Item(289, 0, 1),
						new Item(351, 15, 1),
					}),
				new ShapelessRecipe(new Item(401, 0, 3),
					new List<Item>
					{
						new Item(339, 0, 1),
						new Item(289, 0, 1),
						new Item(402, 15, 1),
					}),
				new ShapelessRecipe(new Item(402, 14, 1),
					new List<Item>
					{
						new Item(289, 0, 1),
						new Item(351, 14, 1),
					}),
				new ShapelessRecipe(new Item(401, 0, 3),
					new List<Item>
					{
						new Item(339, 0, 1),
						new Item(289, 0, 1),
						new Item(402, 14, 1),
					}),
				new ShapelessRecipe(new Item(402, 13, 1),
					new List<Item>
					{
						new Item(289, 0, 1),
						new Item(351, 13, 1),
					}),
				new ShapelessRecipe(new Item(401, 0, 3),
					new List<Item>
					{
						new Item(339, 0, 1),
						new Item(289, 0, 1),
						new Item(402, 13, 1),
					}),
				new ShapelessRecipe(new Item(402, 12, 1),
					new List<Item>
					{
						new Item(289, 0, 1),
						new Item(351, 12, 1),
					}),
				new ShapelessRecipe(new Item(401, 0, 3),
					new List<Item>
					{
						new Item(339, 0, 1),
						new Item(289, 0, 1),
						new Item(402, 12, 1),
					}),
				new ShapelessRecipe(new Item(402, 11, 1),
					new List<Item>
					{
						new Item(289, 0, 1),
						new Item(351, 11, 1),
					}),
				new ShapelessRecipe(new Item(401, 0, 3),
					new List<Item>
					{
						new Item(339, 0, 1),
						new Item(289, 0, 1),
						new Item(402, 11, 1),
					}),
				new ShapelessRecipe(new Item(402, 10, 1),
					new List<Item>
					{
						new Item(289, 0, 1),
						new Item(351, 10, 1),
					}),
				new ShapelessRecipe(new Item(401, 0, 3),
					new List<Item>
					{
						new Item(339, 0, 1),
						new Item(289, 0, 1),
						new Item(402, 10, 1),
					}),
				new ShapelessRecipe(new Item(402, 9, 1),
					new List<Item>
					{
						new Item(289, 0, 1),
						new Item(351, 9, 1),
					}),
				new ShapelessRecipe(new Item(401, 0, 3),
					new List<Item>
					{
						new Item(339, 0, 1),
						new Item(289, 0, 1),
						new Item(402, 9, 1),
					}),
				new ShapelessRecipe(new Item(402, 8, 1),
					new List<Item>
					{
						new Item(289, 0, 1),
						new Item(351, 8, 1),
					}),
				new ShapelessRecipe(new Item(401, 0, 3),
					new List<Item>
					{
						new Item(339, 0, 1),
						new Item(289, 0, 1),
						new Item(402, 8, 1),
					}),
				new ShapelessRecipe(new Item(402, 7, 1),
					new List<Item>
					{
						new Item(289, 0, 1),
						new Item(351, 7, 1),
					}),
				new ShapelessRecipe(new Item(401, 0, 3),
					new List<Item>
					{
						new Item(339, 0, 1),
						new Item(289, 0, 1),
						new Item(402, 7, 1),
					}),
				new ShapelessRecipe(new Item(402, 6, 1),
					new List<Item>
					{
						new Item(289, 0, 1),
						new Item(351, 6, 1),
					}),
				new ShapelessRecipe(new Item(401, 0, 3),
					new List<Item>
					{
						new Item(339, 0, 1),
						new Item(289, 0, 1),
						new Item(402, 6, 1),
					}),
				new ShapelessRecipe(new Item(402, 5, 1),
					new List<Item>
					{
						new Item(289, 0, 1),
						new Item(351, 5, 1),
					}),
				new ShapelessRecipe(new Item(401, 0, 3),
					new List<Item>
					{
						new Item(339, 0, 1),
						new Item(289, 0, 1),
						new Item(402, 5, 1),
					}),
				new ShapelessRecipe(new Item(402, 4, 1),
					new List<Item>
					{
						new Item(289, 0, 1),
						new Item(351, 4, 1),
					}),
				new ShapelessRecipe(new Item(401, 0, 3),
					new List<Item>
					{
						new Item(339, 0, 1),
						new Item(289, 0, 1),
						new Item(402, 4, 1),
					}),
				new ShapelessRecipe(new Item(402, 3, 1),
					new List<Item>
					{
						new Item(289, 0, 1),
						new Item(351, 3, 1),
					}),
				new ShapelessRecipe(new Item(401, 0, 3),
					new List<Item>
					{
						new Item(339, 0, 1),
						new Item(289, 0, 1),
						new Item(402, 3, 1),
					}),
				new ShapelessRecipe(new Item(402, 2, 1),
					new List<Item>
					{
						new Item(289, 0, 1),
						new Item(351, 2, 1),
					}),
				new ShapelessRecipe(new Item(401, 0, 3),
					new List<Item>
					{
						new Item(339, 0, 1),
						new Item(289, 0, 1),
						new Item(402, 2, 1),
					}),
				new ShapelessRecipe(new Item(402, 1, 1),
					new List<Item>
					{
						new Item(289, 0, 1),
						new Item(351, 1, 1),
					}),
				new ShapelessRecipe(new Item(401, 0, 3),
					new List<Item>
					{
						new Item(339, 0, 1),
						new Item(289, 0, 1),
						new Item(402, 1, 1),
					}),
				new ShapelessRecipe(new Item(402, 0, 1),
					new List<Item>
					{
						new Item(289, 0, 1),
						new Item(351, 0, 1),
					}),
				new ShapelessRecipe(new Item(401, 0, 3),
					new List<Item>
					{
						new Item(339, 0, 1),
						new Item(289, 0, 1),
						new Item(402, 0, 1),
					}),
				new ShapedRecipe(2, 2, new Item(-204, 0, 1),
					new Item[]
					{
						new Item(287, 0),
						new Item(5, -1),
						new Item(287, 0),
						new Item(5, -1),
					}),
				new ShapedRecipe(3, 3, new Item(270, 0, 1),
					new Item[]
					{
						new Item(5, -1),
						new Item(0, 0),
						new Item(0, 0),
						new Item(5, -1),
						new Item(280, 0),
						new Item(280, 0),
						new Item(5, -1),
						new Item(0, 0),
						new Item(0, 0),
					}),
				new ShapedRecipe(1, 3, new Item(269, 0, 1),
					new Item[]
					{
						new Item(5, -1),
						new Item(280, 0),
						new Item(280, 0),
					}),
				new ShapedRecipe(2, 3, new Item(271, 0, 1),
					new Item[]
					{
						new Item(5, -1),
						new Item(280, 0),
						new Item(5, -1),
						new Item(0, 0),
						new Item(5, -1),
						new Item(280, 0),
					}),
				new ShapedRecipe(2, 3, new Item(290, 0, 1),
					new Item[]
					{
						new Item(5, -1),
						new Item(280, 0),
						new Item(5, -1),
						new Item(0, 0),
						new Item(0, 0),
						new Item(280, 0),
					}),
				new ShapedRecipe(1, 3, new Item(268, 0, 1),
					new Item[]
					{
						new Item(5, -1),
						new Item(5, -1),
						new Item(280, 0),
					}),
				new ShapedRecipe(3, 3, new Item(262, 6, 8),
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
					}),
				new ShapedRecipe(3, 3, new Item(262, 7, 8),
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
					}),
				new ShapedRecipe(3, 3, new Item(262, 8, 8),
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
					}),
				new ShapedRecipe(3, 3, new Item(262, 9, 8),
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
					}),
				new ShapedRecipe(3, 3, new Item(262, 10, 8),
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
					}),
				new ShapedRecipe(3, 3, new Item(262, 11, 8),
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
					}),
				new ShapedRecipe(3, 3, new Item(262, 12, 8),
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
					}),
				new ShapedRecipe(3, 3, new Item(262, 13, 8),
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
					}),
				new ShapedRecipe(3, 3, new Item(262, 14, 8),
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
					}),
				new ShapedRecipe(3, 3, new Item(262, 15, 8),
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
					}),
				new ShapedRecipe(3, 3, new Item(262, 16, 8),
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
					}),
				new ShapedRecipe(3, 3, new Item(262, 17, 8),
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
					}),
				new ShapedRecipe(3, 3, new Item(262, 18, 8),
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
					}),
				new ShapedRecipe(3, 3, new Item(262, 19, 8),
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
					}),
				new ShapedRecipe(3, 3, new Item(262, 20, 8),
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
					}),
				new ShapedRecipe(3, 3, new Item(262, 21, 8),
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
					}),
				new ShapedRecipe(3, 3, new Item(262, 22, 8),
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
					}),
				new ShapedRecipe(3, 3, new Item(262, 23, 8),
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
					}),
				new ShapedRecipe(3, 3, new Item(262, 24, 8),
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
					}),
				new ShapedRecipe(3, 3, new Item(262, 25, 8),
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
					}),
				new ShapedRecipe(3, 3, new Item(262, 26, 8),
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
					}),
				new ShapedRecipe(3, 3, new Item(262, 27, 8),
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
					}),
				new ShapedRecipe(3, 3, new Item(262, 28, 8),
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
					}),
				new ShapedRecipe(3, 3, new Item(262, 29, 8),
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
					}),
				new ShapedRecipe(3, 3, new Item(262, 30, 8),
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
					}),
				new ShapedRecipe(3, 3, new Item(262, 31, 8),
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
					}),
				new ShapedRecipe(3, 3, new Item(262, 32, 8),
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
					}),
				new ShapedRecipe(3, 3, new Item(262, 33, 8),
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
					}),
				new ShapedRecipe(3, 3, new Item(262, 34, 8),
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
					}),
				new ShapedRecipe(3, 3, new Item(262, 35, 8),
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
					}),
				new ShapedRecipe(3, 3, new Item(262, 36, 8),
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
					}),
				new ShapedRecipe(3, 3, new Item(262, 37, 8),
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
					}),
				new ShapedRecipe(3, 3, new Item(262, 38, 8),
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
					}),
				new ShapedRecipe(3, 3, new Item(262, 39, 8),
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
					}),
				new ShapedRecipe(3, 3, new Item(262, 40, 8),
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
					}),
				new ShapedRecipe(3, 3, new Item(262, 41, 8),
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
					}),
				new ShapedRecipe(3, 3, new Item(262, 42, 8),
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
					}),
				new MultiRecipe() { Id = new UUID("b5c5d105-75a2-4076-af2b-923ea2bf4bf0") }, // b5c5d105-75a2-4076-af2b-923ea2bf4bf0
				new MultiRecipe() { Id = new UUID("d81aaeaf-e172-4440-9225-868df030d27b") }, // d81aaeaf-e172-4440-9225-868df030d27b
				new MultiRecipe() { Id = new UUID("00000000-0000-0000-0000-000000000002") }, // 00000000-0000-0000-0000-000000000002
				new MultiRecipe() { Id = new UUID("85939755-ba10-4d9d-a4cc-efb7a8e943c4") }, // 85939755-ba10-4d9d-a4cc-efb7a8e943c4
				new MultiRecipe() { Id = new UUID("d392b075-4ba1-40ae-8789-af868d56f6ce") }, // d392b075-4ba1-40ae-8789-af868d56f6ce
				new MultiRecipe() { Id = new UUID("aecd2294-4b94-434b-8667-4499bb2c9327") }, // aecd2294-4b94-434b-8667-4499bb2c9327
				new MultiRecipe() { Id = new UUID("00000000-0000-0000-0000-000000000001") }, // 00000000-0000-0000-0000-000000000001
				new MultiRecipe() { Id = new UUID("d1ca6b84-338e-4f2f-9c6b-76cc8b4bd98d") }, // d1ca6b84-338e-4f2f-9c6b-76cc8b4bd98d
				new SmeltingRecipe(new Item(263, 1, 1), new Item(-212, 0)),
				new SmeltingRecipe(new Item(351, 10, 1), new Item(-156, 0)),
				new SmeltingRecipe(new Item(263, 1, 1), new Item(-10, 0)),
				new SmeltingRecipe(new Item(263, 1, 1), new Item(-9, 0)),
				new SmeltingRecipe(new Item(263, 1, 1), new Item(-8, 0)),
				new SmeltingRecipe(new Item(263, 1, 1), new Item(-7, 0)),
				new SmeltingRecipe(new Item(263, 1, 1), new Item(-6, 0)),
				new SmeltingRecipe(new Item(263, 1, 1), new Item(-5, 0)),
				new SmeltingRecipe(new Item(1, 0, 1), new Item(4, 0)),
				new SmeltingRecipe(new Item(20, 0, 1), new Item(12, 0)),
				new SmeltingRecipe(new Item(266, 0, 1), new Item(14, 0)),
				new SmeltingRecipe(new Item(265, 0, 1), new Item(15, 0)),
				new SmeltingRecipe(new Item(263, 0, 1), new Item(16, 0)),
				new SmeltingRecipe(new Item(263, 1, 1), new Item(17, 0)),
				new SmeltingRecipe(new Item(351, 4, 1), new Item(21, 0)),
				new SmeltingRecipe(new Item(264, 0, 1), new Item(56, 0)),
				new SmeltingRecipe(new Item(331, 0, 1), new Item(73, 0)),
				new SmeltingRecipe(new Item(351, 2, 1), new Item(81, 0)),
				new SmeltingRecipe(new Item(172, 0, 1), new Item(82, 0)),
				new SmeltingRecipe(new Item(405, 0, 1), new Item(87, 0)),
				new SmeltingRecipe(new Item(388, 0, 1), new Item(129, 0)),
				new SmeltingRecipe(new Item(406, 0, 1), new Item(153, 0)),
				new SmeltingRecipe(new Item(263, 1, 1), new Item(162, 0)),
				new SmeltingRecipe(new Item(452, 0, 1), new Item(256, 0)),
				new SmeltingRecipe(new Item(452, 0, 1), new Item(257, 0)),
				new SmeltingRecipe(new Item(452, 0, 1), new Item(258, 0)),
				new SmeltingRecipe(new Item(452, 0, 1), new Item(267, 0)),
				new SmeltingRecipe(new Item(371, 0, 1), new Item(283, 0)),
				new SmeltingRecipe(new Item(371, 0, 1), new Item(284, 0)),
				new SmeltingRecipe(new Item(371, 0, 1), new Item(285, 0)),
				new SmeltingRecipe(new Item(371, 0, 1), new Item(286, 0)),
				new SmeltingRecipe(new Item(452, 0, 1), new Item(292, 0)),
				new SmeltingRecipe(new Item(371, 0, 1), new Item(294, 0)),
				new SmeltingRecipe(new Item(452, 0, 1), new Item(302, 0)),
				new SmeltingRecipe(new Item(452, 0, 1), new Item(303, 0)),
				new SmeltingRecipe(new Item(452, 0, 1), new Item(304, 0)),
				new SmeltingRecipe(new Item(452, 0, 1), new Item(305, 0)),
				new SmeltingRecipe(new Item(452, 0, 1), new Item(306, 0)),
				new SmeltingRecipe(new Item(452, 0, 1), new Item(307, 0)),
				new SmeltingRecipe(new Item(452, 0, 1), new Item(308, 0)),
				new SmeltingRecipe(new Item(452, 0, 1), new Item(309, 0)),
				new SmeltingRecipe(new Item(371, 0, 1), new Item(314, 0)),
				new SmeltingRecipe(new Item(371, 0, 1), new Item(315, 0)),
				new SmeltingRecipe(new Item(371, 0, 1), new Item(316, 0)),
				new SmeltingRecipe(new Item(371, 0, 1), new Item(317, 0)),
				new SmeltingRecipe(new Item(320, 0, 1), new Item(319, 0)),
				new SmeltingRecipe(new Item(464, 0, 1), new Item(335, 0)),
				new SmeltingRecipe(new Item(336, 0, 1), new Item(337, 0)),
				new SmeltingRecipe(new Item(350, 0, 1), new Item(349, 0)),
				new SmeltingRecipe(new Item(364, 0, 1), new Item(363, 0)),
				new SmeltingRecipe(new Item(366, 0, 1), new Item(365, 0)),
				new SmeltingRecipe(new Item(393, 0, 1), new Item(392, 0)),
				new SmeltingRecipe(new Item(412, 0, 1), new Item(411, 0)),
				new SmeltingRecipe(new Item(452, 0, 1), new Item(417, 0)),
				new SmeltingRecipe(new Item(371, 0, 1), new Item(418, 0)),
				new SmeltingRecipe(new Item(424, 0, 1), new Item(423, 0)),
				new SmeltingRecipe(new Item(433, 0, 1), new Item(432, 0)),
				new SmeltingRecipe(new Item(463, 0, 1), new Item(460, 0)),
				new SmeltingRecipe(new Item(-183, 0, 1), new Item(1, 0)),
				new SmeltingRecipe(new Item(19, 0, 1), new Item(19, 1)),
				new SmeltingRecipe(new Item(24, 3, 1), new Item(24, 0)),
				new SmeltingRecipe(new Item(98, 2, 1), new Item(98, 0)),
				new SmeltingRecipe(new Item(155, 3, 1), new Item(155, 0)),
				new SmeltingRecipe(new Item(220, 0, 1), new Item(159, 0)),
				new SmeltingRecipe(new Item(221, 0, 1), new Item(159, 1)),
				new SmeltingRecipe(new Item(222, 0, 1), new Item(159, 2)),
				new SmeltingRecipe(new Item(223, 0, 1), new Item(159, 3)),
				new SmeltingRecipe(new Item(224, 0, 1), new Item(159, 4)),
				new SmeltingRecipe(new Item(225, 0, 1), new Item(159, 5)),
				new SmeltingRecipe(new Item(226, 0, 1), new Item(159, 6)),
				new SmeltingRecipe(new Item(227, 0, 1), new Item(159, 7)),
				new SmeltingRecipe(new Item(228, 0, 1), new Item(159, 8)),
				new SmeltingRecipe(new Item(229, 0, 1), new Item(159, 9)),
				new SmeltingRecipe(new Item(219, 0, 1), new Item(159, 10)),
				new SmeltingRecipe(new Item(231, 0, 1), new Item(159, 11)),
				new SmeltingRecipe(new Item(232, 0, 1), new Item(159, 12)),
				new SmeltingRecipe(new Item(233, 0, 1), new Item(159, 13)),
				new SmeltingRecipe(new Item(234, 0, 1), new Item(159, 14)),
				new SmeltingRecipe(new Item(235, 0, 1), new Item(159, 15)),
				new SmeltingRecipe(new Item(179, 3, 1), new Item(179, 0)),
			};
		}

		public static void Add(Recipe recipe)
		{
			Log.InfoFormat("{0}", recipe.Id);
		}
	}
}