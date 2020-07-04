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
				new MultiRecipe() { Id = new UUID("442d85ed-8272-4543-a6f1-418f90ded05d"), UniqueId = 1950 }, // 442d85ed-8272-4543-a6f1-418f90ded05d
				new MultiRecipe() { Id = new UUID("8b36268c-1829-483c-a0f1-993b7156a8f2"), UniqueId = 1952 }, // 8b36268c-1829-483c-a0f1-993b7156a8f2
				new MultiRecipe() { Id = new UUID("602234e4-cac1-4353-8bb7-b1ebff70024b"), UniqueId = 1953 }, // 602234e4-cac1-4353-8bb7-b1ebff70024b
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(395, 2, 1),
					},
					new List<Item>
					{
						new Item(339, 32767, 1),
						new Item(345, 32767, 1),
					}, "cartography_table"){ UniqueId = 293 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(395, 0, 1),
					},
					new List<Item>
					{
						new Item(339, 32767, 1),
					}, "cartography_table"){ UniqueId = 294 },
				new MultiRecipe() { Id = new UUID("98c84b38-1085-46bd-b1ce-dd38c159e6cc"), UniqueId = 1955 }, // 98c84b38-1085-46bd-b1ce-dd38c159e6cc
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-162, 3, 2),
					},
					new List<Item>
					{
						new Item(1, 5, 1),
					}, "stonecutter"){ UniqueId = 676 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-171, 0, 1),
					},
					new List<Item>
					{
						new Item(1, 5, 1),
					}, "stonecutter"){ UniqueId = 677 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(139, 4, 1),
					},
					new List<Item>
					{
						new Item(1, 5, 1),
					}, "stonecutter"){ UniqueId = 678 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-282, 0, 2),
					},
					new List<Item>
					{
						new Item(-273, 32767, 1),
					}, "stonecutter"){ UniqueId = 130 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-276, 0, 1),
					},
					new List<Item>
					{
						new Item(-273, 32767, 1),
					}, "stonecutter"){ UniqueId = 131 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-277, 0, 1),
					},
					new List<Item>
					{
						new Item(-273, 32767, 1),
					}, "stonecutter"){ UniqueId = 132 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(44, 4, 2),
					},
					new List<Item>
					{
						new Item(45, 0, 1),
					}, "stonecutter"){ UniqueId = 679 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-284, 0, 2),
					},
					new List<Item>
					{
						new Item(-291, 32767, 1),
					}, "stonecutter"){ UniqueId = 134 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(108, 0, 1),
					},
					new List<Item>
					{
						new Item(45, 0, 1),
					}, "stonecutter"){ UniqueId = 680 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-275, 0, 1),
					},
					new List<Item>
					{
						new Item(-291, 32767, 1),
					}, "stonecutter"){ UniqueId = 135 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(139, 6, 1),
					},
					new List<Item>
					{
						new Item(45, 0, 1),
					}, "stonecutter"){ UniqueId = 681 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-278, 0, 1),
					},
					new List<Item>
					{
						new Item(-291, 32767, 1),
					}, "stonecutter"){ UniqueId = 136 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-274, 0, 1),
					},
					new List<Item>
					{
						new Item(-291, 32767, 1),
					}, "stonecutter"){ UniqueId = 133 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-279, 0, 1),
					},
					new List<Item>
					{
						new Item(-291, 32767, 1),
					}, "stonecutter"){ UniqueId = 137 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-302, 0, 1),
					},
					new List<Item>
					{
						new Item(112, 32767, 1),
					}, "stonecutter"){ UniqueId = 138 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-279, 0, 1),
					},
					new List<Item>
					{
						new Item(-273, 32767, 1),
					}, "stonecutter"){ UniqueId = 139 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(44, 3, 2),
					},
					new List<Item>
					{
						new Item(4, 0, 1),
					}, "stonecutter"){ UniqueId = 682 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(67, 0, 1),
					},
					new List<Item>
					{
						new Item(4, 0, 1),
					}, "stonecutter"){ UniqueId = 683 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(139, 0, 1),
					},
					new List<Item>
					{
						new Item(4, 0, 1),
					}, "stonecutter"){ UniqueId = 684 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(182, 3, 2),
					},
					new List<Item>
					{
						new Item(168, 1, 1),
					}, "stonecutter"){ UniqueId = 685 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-3, 0, 1),
					},
					new List<Item>
					{
						new Item(168, 1, 1),
					}, "stonecutter"){ UniqueId = 686 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-162, 4, 2),
					},
					new List<Item>
					{
						new Item(1, 3, 1),
					}, "stonecutter"){ UniqueId = 687 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-170, 0, 1),
					},
					new List<Item>
					{
						new Item(1, 3, 1),
					}, "stonecutter"){ UniqueId = 688 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(139, 3, 1),
					},
					new List<Item>
					{
						new Item(1, 3, 1),
					}, "stonecutter"){ UniqueId = 689 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-166, 2, 2),
					},
					new List<Item>
					{
						new Item(1, 0, 1),
					}, "stonecutter"){ UniqueId = 763 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-162, 0, 2),
					},
					new List<Item>
					{
						new Item(121, 0, 1),
					}, "stonecutter"){ UniqueId = 691 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-162, 0, 2),
					},
					new List<Item>
					{
						new Item(206, 0, 1),
					}, "stonecutter"){ UniqueId = 692 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-178, 0, 1),
					},
					new List<Item>
					{
						new Item(121, 0, 1),
					}, "stonecutter"){ UniqueId = 693 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-178, 0, 1),
					},
					new List<Item>
					{
						new Item(206, 0, 1),
					}, "stonecutter"){ UniqueId = 694 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(139, 10, 1),
					},
					new List<Item>
					{
						new Item(121, 0, 1),
					}, "stonecutter"){ UniqueId = 695 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(139, 10, 1),
					},
					new List<Item>
					{
						new Item(206, 0, 1),
					}, "stonecutter"){ UniqueId = 696 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(206, 0, 1),
					},
					new List<Item>
					{
						new Item(121, 0, 1),
					}, "stonecutter"){ UniqueId = 690 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-162, 6, 2),
					},
					new List<Item>
					{
						new Item(1, 1, 1),
					}, "stonecutter"){ UniqueId = 697 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-169, 0, 1),
					},
					new List<Item>
					{
						new Item(1, 1, 1),
					}, "stonecutter"){ UniqueId = 698 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(139, 2, 1),
					},
					new List<Item>
					{
						new Item(1, 1, 1),
					}, "stonecutter"){ UniqueId = 699 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(182, 5, 2),
					},
					new List<Item>
					{
						new Item(48, 0, 1),
					}, "stonecutter"){ UniqueId = 700 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-179, 0, 1),
					},
					new List<Item>
					{
						new Item(48, 0, 1),
					}, "stonecutter"){ UniqueId = 701 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(139, 1, 1),
					},
					new List<Item>
					{
						new Item(48, 0, 1),
					}, "stonecutter"){ UniqueId = 702 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-166, 0, 2),
					},
					new List<Item>
					{
						new Item(98, 1, 1),
					}, "stonecutter"){ UniqueId = 703 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-175, 0, 1),
					},
					new List<Item>
					{
						new Item(98, 1, 1),
					}, "stonecutter"){ UniqueId = 704 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(139, 8, 1),
					},
					new List<Item>
					{
						new Item(98, 1, 1),
					}, "stonecutter"){ UniqueId = 705 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(44, 7, 2),
					},
					new List<Item>
					{
						new Item(112, 0, 1),
					}, "stonecutter"){ UniqueId = 706 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(114, 0, 1),
					},
					new List<Item>
					{
						new Item(112, 0, 1),
					}, "stonecutter"){ UniqueId = 707 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(139, 9, 1),
					},
					new List<Item>
					{
						new Item(112, 0, 1),
					}, "stonecutter"){ UniqueId = 708 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(1, 6, 1),
					},
					new List<Item>
					{
						new Item(1, 5, 1),
					}, "stonecutter"){ UniqueId = 709 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-162, 2, 2),
					},
					new List<Item>
					{
						new Item(1, 5, 1),
					}, "stonecutter"){ UniqueId = 710 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-162, 2, 2),
					},
					new List<Item>
					{
						new Item(1, 6, 1),
					}, "stonecutter"){ UniqueId = 711 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-174, 0, 1),
					},
					new List<Item>
					{
						new Item(1, 5, 1),
					}, "stonecutter"){ UniqueId = 712 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-174, 0, 1),
					},
					new List<Item>
					{
						new Item(1, 6, 1),
					}, "stonecutter"){ UniqueId = 713 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-235, 0, 1),
					},
					new List<Item>
					{
						new Item(-234, 32767, 1),
					}, "stonecutter"){ UniqueId = 140 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-284, 0, 2),
					},
					new List<Item>
					{
						new Item(-273, 32767, 1),
					}, "stonecutter"){ UniqueId = 142 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-275, 0, 1),
					},
					new List<Item>
					{
						new Item(-273, 32767, 1),
					}, "stonecutter"){ UniqueId = 143 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-278, 0, 1),
					},
					new List<Item>
					{
						new Item(-273, 32767, 1),
					}, "stonecutter"){ UniqueId = 144 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-274, 0, 1),
					},
					new List<Item>
					{
						new Item(-273, 32767, 1),
					}, "stonecutter"){ UniqueId = 141 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(1, 4, 1),
					},
					new List<Item>
					{
						new Item(1, 3, 1),
					}, "stonecutter"){ UniqueId = 714 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-162, 5, 2),
					},
					new List<Item>
					{
						new Item(1, 3, 1),
					}, "stonecutter"){ UniqueId = 715 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-162, 5, 2),
					},
					new List<Item>
					{
						new Item(1, 4, 1),
					}, "stonecutter"){ UniqueId = 716 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-173, 0, 1),
					},
					new List<Item>
					{
						new Item(1, 3, 1),
					}, "stonecutter"){ UniqueId = 717 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-173, 0, 1),
					},
					new List<Item>
					{
						new Item(1, 4, 1),
					}, "stonecutter"){ UniqueId = 718 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-291, 0, 1),
					},
					new List<Item>
					{
						new Item(-273, 32767, 1),
					}, "stonecutter"){ UniqueId = 145 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(1, 2, 1),
					},
					new List<Item>
					{
						new Item(1, 1, 1),
					}, "stonecutter"){ UniqueId = 719 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-162, 7, 2),
					},
					new List<Item>
					{
						new Item(1, 1, 1),
					}, "stonecutter"){ UniqueId = 720 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-162, 7, 2),
					},
					new List<Item>
					{
						new Item(1, 2, 1),
					}, "stonecutter"){ UniqueId = 721 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-172, 0, 1),
					},
					new List<Item>
					{
						new Item(1, 1, 1),
					}, "stonecutter"){ UniqueId = 722 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-172, 0, 1),
					},
					new List<Item>
					{
						new Item(1, 2, 1),
					}, "stonecutter"){ UniqueId = 723 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-293, 0, 2),
					},
					new List<Item>
					{
						new Item(-273, 32767, 1),
					}, "stonecutter"){ UniqueId = 146 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-292, 0, 1),
					},
					new List<Item>
					{
						new Item(-273, 32767, 1),
					}, "stonecutter"){ UniqueId = 147 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-297, 0, 1),
					},
					new List<Item>
					{
						new Item(-273, 32767, 1),
					}, "stonecutter"){ UniqueId = 148 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(182, 4, 2),
					},
					new List<Item>
					{
						new Item(168, 2, 1),
					}, "stonecutter"){ UniqueId = 724 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-4, 0, 1),
					},
					new List<Item>
					{
						new Item(168, 2, 1),
					}, "stonecutter"){ UniqueId = 725 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(182, 2, 2),
					},
					new List<Item>
					{
						new Item(168, 0, 1),
					}, "stonecutter"){ UniqueId = 726 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-2, 0, 1),
					},
					new List<Item>
					{
						new Item(168, 0, 1),
					}, "stonecutter"){ UniqueId = 727 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(139, 11, 1),
					},
					new List<Item>
					{
						new Item(168, 0, 1),
					}, "stonecutter"){ UniqueId = 728 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(201, 2, 1),
					},
					new List<Item>
					{
						new Item(201, 0, 1),
					}, "stonecutter"){ UniqueId = 729 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(182, 1, 2),
					},
					new List<Item>
					{
						new Item(201, 0, 1),
					}, "stonecutter"){ UniqueId = 730 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(203, 0, 1),
					},
					new List<Item>
					{
						new Item(201, 0, 1),
					}, "stonecutter"){ UniqueId = 731 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-304, 0, 1),
					},
					new List<Item>
					{
						new Item(155, 32767, 1),
					}, "stonecutter"){ UniqueId = 149 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(155, 1, 1),
					},
					new List<Item>
					{
						new Item(155, 0, 1),
					}, "stonecutter"){ UniqueId = 732 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(155, 2, 1),
					},
					new List<Item>
					{
						new Item(155, 0, 1),
					}, "stonecutter"){ UniqueId = 733 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(44, 6, 2),
					},
					new List<Item>
					{
						new Item(155, 0, 1),
					}, "stonecutter"){ UniqueId = 150 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(156, 0, 1),
					},
					new List<Item>
					{
						new Item(155, 0, 1),
					}, "stonecutter"){ UniqueId = 734 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(182, 7, 2),
					},
					new List<Item>
					{
						new Item(215, 0, 1),
					}, "stonecutter"){ UniqueId = 735 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-184, 0, 1),
					},
					new List<Item>
					{
						new Item(215, 0, 1),
					}, "stonecutter"){ UniqueId = 736 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(139, 13, 1),
					},
					new List<Item>
					{
						new Item(215, 0, 1),
					}, "stonecutter"){ UniqueId = 737 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(182, 0, 2),
					},
					new List<Item>
					{
						new Item(179, 0, 1),
					}, "stonecutter"){ UniqueId = 740 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(179, 2, 1),
					},
					new List<Item>
					{
						new Item(179, 0, 1),
					}, "stonecutter"){ UniqueId = 738 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(179, 1, 1),
					},
					new List<Item>
					{
						new Item(179, 0, 1),
					}, "stonecutter"){ UniqueId = 739 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(180, 0, 1),
					},
					new List<Item>
					{
						new Item(179, 0, 1),
					}, "stonecutter"){ UniqueId = 741 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(139, 12, 1),
					},
					new List<Item>
					{
						new Item(179, 0, 1),
					}, "stonecutter"){ UniqueId = 742 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(44, 1, 2),
					},
					new List<Item>
					{
						new Item(24, 0, 1),
					}, "stonecutter"){ UniqueId = 745 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(24, 2, 1),
					},
					new List<Item>
					{
						new Item(24, 0, 1),
					}, "stonecutter"){ UniqueId = 743 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(24, 1, 1),
					},
					new List<Item>
					{
						new Item(24, 0, 1),
					}, "stonecutter"){ UniqueId = 744 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(128, 0, 1),
					},
					new List<Item>
					{
						new Item(24, 0, 1),
					}, "stonecutter"){ UniqueId = 746 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(139, 5, 1),
					},
					new List<Item>
					{
						new Item(24, 0, 1),
					}, "stonecutter"){ UniqueId = 747 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-293, 0, 2),
					},
					new List<Item>
					{
						new Item(-291, 32767, 1),
					}, "stonecutter"){ UniqueId = 151 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-284, 0, 2),
					},
					new List<Item>
					{
						new Item(-274, 32767, 1),
					}, "stonecutter"){ UniqueId = 152 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(44, 0, 2),
					},
					new List<Item>
					{
						new Item(-183, 0, 1),
					}, "stonecutter"){ UniqueId = 754 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-166, 1, 2),
					},
					new List<Item>
					{
						new Item(155, 3, 1),
					}, "stonecutter"){ UniqueId = 748 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-185, 0, 1),
					},
					new List<Item>
					{
						new Item(155, 3, 1),
					}, "stonecutter"){ UniqueId = 749 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-162, 1, 2),
					},
					new List<Item>
					{
						new Item(179, 3, 1),
					}, "stonecutter"){ UniqueId = 750 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-176, 0, 1),
					},
					new List<Item>
					{
						new Item(179, 3, 1),
					}, "stonecutter"){ UniqueId = 751 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(182, 6, 2),
					},
					new List<Item>
					{
						new Item(24, 3, 1),
					}, "stonecutter"){ UniqueId = 752 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-177, 0, 1),
					},
					new List<Item>
					{
						new Item(24, 3, 1),
					}, "stonecutter"){ UniqueId = 753 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-292, 0, 1),
					},
					new List<Item>
					{
						new Item(-291, 32767, 1),
					}, "stonecutter"){ UniqueId = 153 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-180, 0, 1),
					},
					new List<Item>
					{
						new Item(1, 0, 1),
					}, "stonecutter"){ UniqueId = 764 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(98, 0, 1),
					},
					new List<Item>
					{
						new Item(1, 0, 1),
					}, "stonecutter"){ UniqueId = 755 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(98, 3, 1),
					},
					new List<Item>
					{
						new Item(1, 0, 1),
					}, "stonecutter"){ UniqueId = 756 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(44, 5, 2),
					},
					new List<Item>
					{
						new Item(1, 0, 1),
					}, "stonecutter"){ UniqueId = 757 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(44, 5, 2),
					},
					new List<Item>
					{
						new Item(98, 0, 1),
					}, "stonecutter"){ UniqueId = 758 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(109, 0, 1),
					},
					new List<Item>
					{
						new Item(1, 0, 1),
					}, "stonecutter"){ UniqueId = 759 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(109, 0, 1),
					},
					new List<Item>
					{
						new Item(98, 0, 1),
					}, "stonecutter"){ UniqueId = 760 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(139, 7, 1),
					},
					new List<Item>
					{
						new Item(1, 0, 1),
					}, "stonecutter"){ UniqueId = 761 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(139, 7, 1),
					},
					new List<Item>
					{
						new Item(98, 0, 1),
					}, "stonecutter"){ UniqueId = 762 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-297, 0, 1),
					},
					new List<Item>
					{
						new Item(-291, 32767, 1),
					}, "stonecutter"){ UniqueId = 155 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-278, 0, 1),
					},
					new List<Item>
					{
						new Item(-274, 32767, 1),
					}, "stonecutter"){ UniqueId = 156 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(-275, 0, 1),
					},
					new List<Item>
					{
						new Item(-274, 32767, 1),
					}, "stonecutter"){ UniqueId = 154 },
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
					}, "crafting_table"){ UniqueId = 1797 },
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
					}, "crafting_table"){ UniqueId = 1789 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(-140, 0),
					},
					new Item[]
					{
						new Item(5, 4),
					}, "crafting_table"){ UniqueId = 1800 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(-141, 0),
					},
					new Item[]
					{
						new Item(5, 2),
					}, "crafting_table"){ UniqueId = 1802 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(-142, 0),
					},
					new Item[]
					{
						new Item(5, 5),
					}, "crafting_table"){ UniqueId = 1804 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(-143, 0),
					},
					new Item[]
					{
						new Item(5, 3),
					}, "crafting_table"){ UniqueId = 1806 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(-144, 0),
					},
					new Item[]
					{
						new Item(5, 1),
					}, "crafting_table"){ UniqueId = 1808 },
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
					}, "crafting_table"){ UniqueId = 1787 },
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
					}, "crafting_table"){ UniqueId = 1790 },
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
					}, "crafting_table"){ UniqueId = 1819 },
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
					}, "crafting_table"){ UniqueId = 1818 },
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
					}, "crafting_table"){ UniqueId = 1791 },
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
					}, "crafting_table"){ UniqueId = 1792 },
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
					}, "crafting_table"){ UniqueId = 1825 },
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
					}, "crafting_table"){ UniqueId = 1826 },
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
					}, "crafting_table"){ UniqueId = 1827 },
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
					}, "crafting_table"){ UniqueId = 1793 },
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
					}, "crafting_table"){ UniqueId = 1794 },
				new ShapedRecipe(2, 1,
					new List<Item>
					{
						new Item(-150, 0),
					},
					new Item[]
					{
						new Item(5, 4),
						new Item(5, 4),
					}, "crafting_table"){ UniqueId = 1801 },
				new ShapedRecipe(2, 1,
					new List<Item>
					{
						new Item(-151, 0),
					},
					new Item[]
					{
						new Item(5, 2),
						new Item(5, 2),
					}, "crafting_table"){ UniqueId = 1803 },
				new ShapedRecipe(2, 1,
					new List<Item>
					{
						new Item(-152, 0),
					},
					new Item[]
					{
						new Item(5, 5),
						new Item(5, 5),
					}, "crafting_table"){ UniqueId = 1805 },
				new ShapedRecipe(2, 1,
					new List<Item>
					{
						new Item(-153, 0),
					},
					new Item[]
					{
						new Item(5, 3),
						new Item(5, 3),
					}, "crafting_table"){ UniqueId = 1807 },
				new ShapedRecipe(2, 1,
					new List<Item>
					{
						new Item(-154, 0),
					},
					new Item[]
					{
						new Item(5, 1),
						new Item(5, 1),
					}, "crafting_table"){ UniqueId = 1809 },
				new ShapedRecipe(1, 2,
					new List<Item>
					{
						new Item(280, 0),
					},
					new Item[]
					{
						new Item(-163, 0),
						new Item(-163, 0),
					}, "crafting_table"){ UniqueId = 1795 },
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
					}, "crafting_table"){ UniqueId = 1820 },
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
					}, "crafting_table"){ UniqueId = 1822 },
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
					}, "crafting_table"){ UniqueId = 1824 },
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
					}, "crafting_table"){ UniqueId = 1823 },
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
					}, "crafting_table"){ UniqueId = 1821 },
				new ShapedRecipe(1, 2,
					new List<Item>
					{
						new Item(50, 0),
					},
					new Item[]
					{
						new Item(263, 32767),
						new Item(280, 32767),
					}, "crafting_table"){ UniqueId = 1817 },
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
					}, "crafting_table"){ UniqueId = 1812 },
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
					}, "crafting_table"){ UniqueId = 1813 },
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
					}, "crafting_table"){ UniqueId = 1814 },
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
					}, "crafting_table"){ UniqueId = 1815 },
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
					}, "crafting_table"){ UniqueId = 1816 },
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
					}, "crafting_table"){ UniqueId = 1811 },
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
					}, "crafting_table"){ UniqueId = 1810 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(143, 0),
					},
					new Item[]
					{
						new Item(5, 0),
					}, "crafting_table"){ UniqueId = 1798 },
				new ShapedRecipe(2, 1,
					new List<Item>
					{
						new Item(72, 0),
					},
					new Item[]
					{
						new Item(5, 0),
						new Item(5, 0),
					}, "crafting_table"){ UniqueId = 1799 },
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
					}, "crafting_table"){ UniqueId = 1788 },
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
					}, "crafting_table"){ UniqueId = 1854 },
				new MultiRecipe() { Id = new UUID("d81aaeaf-e172-4440-9225-868df030d27b"), UniqueId = 1947 }, // d81aaeaf-e172-4440-9225-868df030d27b
				new MultiRecipe() { Id = new UUID("b5c5d105-75a2-4076-af2b-923ea2bf4bf0"), UniqueId = 1946 }, // b5c5d105-75a2-4076-af2b-923ea2bf4bf0
				new MultiRecipe() { Id = new UUID("00000000-0000-0000-0000-000000000002"), UniqueId = 1948 }, // 00000000-0000-0000-0000-000000000002
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
					}, "crafting_table"){ UniqueId = 1439 },
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
					}, "crafting_table"){ UniqueId = 1440 },
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
					}, "crafting_table"){ UniqueId = 1449 },
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
					}, "crafting_table"){ UniqueId = 1450 },
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
					}, "crafting_table"){ UniqueId = 1451 },
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
					}, "crafting_table"){ UniqueId = 1452 },
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
					}, "crafting_table"){ UniqueId = 1453 },
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
					}, "crafting_table"){ UniqueId = 1454 },
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
					}, "crafting_table"){ UniqueId = 1441 },
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
					}, "crafting_table"){ UniqueId = 1442 },
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
					}, "crafting_table"){ UniqueId = 1443 },
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
					}, "crafting_table"){ UniqueId = 1444 },
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
					}, "crafting_table"){ UniqueId = 1445 },
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
					}, "crafting_table"){ UniqueId = 1446 },
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
					}, "crafting_table"){ UniqueId = 1447 },
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
					}, "crafting_table"){ UniqueId = 1448 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(355, 0),
					},
					new Item[]
					{
						new Item(35, 0),
						new Item(35, 0),
						new Item(-242, 32767),
						new Item(35, 0),
						new Item(-242, 32767),
						new Item(-242, 32767),
					}, "crafting_table"){ UniqueId = 1471 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(355, 1),
					},
					new Item[]
					{
						new Item(35, 1),
						new Item(35, 1),
						new Item(-242, 32767),
						new Item(35, 1),
						new Item(-242, 32767),
						new Item(-242, 32767),
					}, "crafting_table"){ UniqueId = 1472 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(355, 10),
					},
					new Item[]
					{
						new Item(35, 10),
						new Item(35, 10),
						new Item(-242, 32767),
						new Item(35, 10),
						new Item(-242, 32767),
						new Item(-242, 32767),
					}, "crafting_table"){ UniqueId = 1481 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(355, 11),
					},
					new Item[]
					{
						new Item(35, 11),
						new Item(35, 11),
						new Item(-242, 32767),
						new Item(35, 11),
						new Item(-242, 32767),
						new Item(-242, 32767),
					}, "crafting_table"){ UniqueId = 1482 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(355, 12),
					},
					new Item[]
					{
						new Item(35, 12),
						new Item(35, 12),
						new Item(-242, 32767),
						new Item(35, 12),
						new Item(-242, 32767),
						new Item(-242, 32767),
					}, "crafting_table"){ UniqueId = 1483 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(355, 13),
					},
					new Item[]
					{
						new Item(35, 13),
						new Item(35, 13),
						new Item(-242, 32767),
						new Item(35, 13),
						new Item(-242, 32767),
						new Item(-242, 32767),
					}, "crafting_table"){ UniqueId = 1484 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(355, 14),
					},
					new Item[]
					{
						new Item(35, 14),
						new Item(35, 14),
						new Item(-242, 32767),
						new Item(35, 14),
						new Item(-242, 32767),
						new Item(-242, 32767),
					}, "crafting_table"){ UniqueId = 1485 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(355, 15),
					},
					new Item[]
					{
						new Item(35, 15),
						new Item(35, 15),
						new Item(-242, 32767),
						new Item(35, 15),
						new Item(-242, 32767),
						new Item(-242, 32767),
					}, "crafting_table"){ UniqueId = 1486 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(355, 2),
					},
					new Item[]
					{
						new Item(35, 2),
						new Item(35, 2),
						new Item(-242, 32767),
						new Item(35, 2),
						new Item(-242, 32767),
						new Item(-242, 32767),
					}, "crafting_table"){ UniqueId = 1473 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(355, 3),
					},
					new Item[]
					{
						new Item(35, 3),
						new Item(35, 3),
						new Item(-242, 32767),
						new Item(35, 3),
						new Item(-242, 32767),
						new Item(-242, 32767),
					}, "crafting_table"){ UniqueId = 1474 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(355, 4),
					},
					new Item[]
					{
						new Item(35, 4),
						new Item(35, 4),
						new Item(-242, 32767),
						new Item(35, 4),
						new Item(-242, 32767),
						new Item(-242, 32767),
					}, "crafting_table"){ UniqueId = 1475 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(355, 5),
					},
					new Item[]
					{
						new Item(35, 5),
						new Item(35, 5),
						new Item(-242, 32767),
						new Item(35, 5),
						new Item(-242, 32767),
						new Item(-242, 32767),
					}, "crafting_table"){ UniqueId = 1476 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(355, 6),
					},
					new Item[]
					{
						new Item(35, 6),
						new Item(35, 6),
						new Item(-242, 32767),
						new Item(35, 6),
						new Item(-242, 32767),
						new Item(-242, 32767),
					}, "crafting_table"){ UniqueId = 1477 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(355, 7),
					},
					new Item[]
					{
						new Item(35, 7),
						new Item(35, 7),
						new Item(-242, 32767),
						new Item(35, 7),
						new Item(-242, 32767),
						new Item(-242, 32767),
					}, "crafting_table"){ UniqueId = 1478 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(355, 8),
					},
					new Item[]
					{
						new Item(35, 8),
						new Item(35, 8),
						new Item(-242, 32767),
						new Item(35, 8),
						new Item(-242, 32767),
						new Item(-242, 32767),
					}, "crafting_table"){ UniqueId = 1479 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(355, 9),
					},
					new Item[]
					{
						new Item(35, 9),
						new Item(35, 9),
						new Item(-242, 32767),
						new Item(35, 9),
						new Item(-242, 32767),
						new Item(-242, 32767),
					}, "crafting_table"){ UniqueId = 1480 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(355, 0),
					},
					new Item[]
					{
						new Item(35, 0),
						new Item(35, 0),
						new Item(-243, 32767),
						new Item(35, 0),
						new Item(-243, 32767),
						new Item(-243, 32767),
					}, "crafting_table"){ UniqueId = 1455 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(355, 1),
					},
					new Item[]
					{
						new Item(35, 1),
						new Item(35, 1),
						new Item(-243, 32767),
						new Item(35, 1),
						new Item(-243, 32767),
						new Item(-243, 32767),
					}, "crafting_table"){ UniqueId = 1456 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(355, 10),
					},
					new Item[]
					{
						new Item(35, 10),
						new Item(35, 10),
						new Item(-243, 32767),
						new Item(35, 10),
						new Item(-243, 32767),
						new Item(-243, 32767),
					}, "crafting_table"){ UniqueId = 1465 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(355, 11),
					},
					new Item[]
					{
						new Item(35, 11),
						new Item(35, 11),
						new Item(-243, 32767),
						new Item(35, 11),
						new Item(-243, 32767),
						new Item(-243, 32767),
					}, "crafting_table"){ UniqueId = 1466 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(355, 12),
					},
					new Item[]
					{
						new Item(35, 12),
						new Item(35, 12),
						new Item(-243, 32767),
						new Item(35, 12),
						new Item(-243, 32767),
						new Item(-243, 32767),
					}, "crafting_table"){ UniqueId = 1467 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(355, 13),
					},
					new Item[]
					{
						new Item(35, 13),
						new Item(35, 13),
						new Item(-243, 32767),
						new Item(35, 13),
						new Item(-243, 32767),
						new Item(-243, 32767),
					}, "crafting_table"){ UniqueId = 1468 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(355, 14),
					},
					new Item[]
					{
						new Item(35, 14),
						new Item(35, 14),
						new Item(-243, 32767),
						new Item(35, 14),
						new Item(-243, 32767),
						new Item(-243, 32767),
					}, "crafting_table"){ UniqueId = 1469 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(355, 15),
					},
					new Item[]
					{
						new Item(35, 15),
						new Item(35, 15),
						new Item(-243, 32767),
						new Item(35, 15),
						new Item(-243, 32767),
						new Item(-243, 32767),
					}, "crafting_table"){ UniqueId = 1470 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(355, 2),
					},
					new Item[]
					{
						new Item(35, 2),
						new Item(35, 2),
						new Item(-243, 32767),
						new Item(35, 2),
						new Item(-243, 32767),
						new Item(-243, 32767),
					}, "crafting_table"){ UniqueId = 1457 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(355, 3),
					},
					new Item[]
					{
						new Item(35, 3),
						new Item(35, 3),
						new Item(-243, 32767),
						new Item(35, 3),
						new Item(-243, 32767),
						new Item(-243, 32767),
					}, "crafting_table"){ UniqueId = 1458 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(355, 4),
					},
					new Item[]
					{
						new Item(35, 4),
						new Item(35, 4),
						new Item(-243, 32767),
						new Item(35, 4),
						new Item(-243, 32767),
						new Item(-243, 32767),
					}, "crafting_table"){ UniqueId = 1459 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(355, 5),
					},
					new Item[]
					{
						new Item(35, 5),
						new Item(35, 5),
						new Item(-243, 32767),
						new Item(35, 5),
						new Item(-243, 32767),
						new Item(-243, 32767),
					}, "crafting_table"){ UniqueId = 1460 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(355, 6),
					},
					new Item[]
					{
						new Item(35, 6),
						new Item(35, 6),
						new Item(-243, 32767),
						new Item(35, 6),
						new Item(-243, 32767),
						new Item(-243, 32767),
					}, "crafting_table"){ UniqueId = 1461 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(355, 7),
					},
					new Item[]
					{
						new Item(35, 7),
						new Item(35, 7),
						new Item(-243, 32767),
						new Item(35, 7),
						new Item(-243, 32767),
						new Item(-243, 32767),
					}, "crafting_table"){ UniqueId = 1462 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(355, 8),
					},
					new Item[]
					{
						new Item(35, 8),
						new Item(35, 8),
						new Item(-243, 32767),
						new Item(35, 8),
						new Item(-243, 32767),
						new Item(-243, 32767),
					}, "crafting_table"){ UniqueId = 1463 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(355, 9),
					},
					new Item[]
					{
						new Item(35, 9),
						new Item(35, 9),
						new Item(-243, 32767),
						new Item(35, 9),
						new Item(-243, 32767),
						new Item(-243, 32767),
					}, "crafting_table"){ UniqueId = 1464 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 15, 1),
					},
					new List<Item>
					{
						new Item(355, 14, 1),
						new Item(351, 0, 1),
					}, "crafting_table"){ UniqueId = 1772 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 15, 1),
					},
					new List<Item>
					{
						new Item(355, 5, 1),
						new Item(351, 0, 1),
					}, "crafting_table"){ UniqueId = 1781 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 15, 1),
					},
					new List<Item>
					{
						new Item(355, 4, 1),
						new Item(351, 0, 1),
					}, "crafting_table"){ UniqueId = 1782 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 15, 1),
					},
					new List<Item>
					{
						new Item(355, 3, 1),
						new Item(351, 0, 1),
					}, "crafting_table"){ UniqueId = 1783 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 15, 1),
					},
					new List<Item>
					{
						new Item(355, 2, 1),
						new Item(351, 0, 1),
					}, "crafting_table"){ UniqueId = 1784 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 15, 1),
					},
					new List<Item>
					{
						new Item(355, 1, 1),
						new Item(351, 0, 1),
					}, "crafting_table"){ UniqueId = 1785 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 15, 1),
					},
					new List<Item>
					{
						new Item(355, 0, 1),
						new Item(351, 0, 1),
					}, "crafting_table"){ UniqueId = 1786 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 15, 1),
					},
					new List<Item>
					{
						new Item(355, 13, 1),
						new Item(351, 0, 1),
					}, "crafting_table"){ UniqueId = 1773 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 15, 1),
					},
					new List<Item>
					{
						new Item(355, 12, 1),
						new Item(351, 0, 1),
					}, "crafting_table"){ UniqueId = 1774 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 15, 1),
					},
					new List<Item>
					{
						new Item(355, 11, 1),
						new Item(351, 0, 1),
					}, "crafting_table"){ UniqueId = 1775 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 15, 1),
					},
					new List<Item>
					{
						new Item(355, 10, 1),
						new Item(351, 0, 1),
					}, "crafting_table"){ UniqueId = 1776 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 15, 1),
					},
					new List<Item>
					{
						new Item(355, 9, 1),
						new Item(351, 0, 1),
					}, "crafting_table"){ UniqueId = 1777 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 15, 1),
					},
					new List<Item>
					{
						new Item(355, 8, 1),
						new Item(351, 0, 1),
					}, "crafting_table"){ UniqueId = 1778 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 15, 1),
					},
					new List<Item>
					{
						new Item(355, 7, 1),
						new Item(351, 0, 1),
					}, "crafting_table"){ UniqueId = 1779 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 15, 1),
					},
					new List<Item>
					{
						new Item(355, 6, 1),
						new Item(351, 0, 1),
					}, "crafting_table"){ UniqueId = 1780 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 5, 1),
					},
					new List<Item>
					{
						new Item(355, 15, 1),
						new Item(351, 10, 1),
					}, "crafting_table"){ UniqueId = 1622 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 5, 1),
					},
					new List<Item>
					{
						new Item(355, 14, 1),
						new Item(351, 10, 1),
					}, "crafting_table"){ UniqueId = 1623 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 5, 1),
					},
					new List<Item>
					{
						new Item(355, 4, 1),
						new Item(351, 10, 1),
					}, "crafting_table"){ UniqueId = 1632 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 5, 1),
					},
					new List<Item>
					{
						new Item(355, 3, 1),
						new Item(351, 10, 1),
					}, "crafting_table"){ UniqueId = 1633 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 5, 1),
					},
					new List<Item>
					{
						new Item(355, 2, 1),
						new Item(351, 10, 1),
					}, "crafting_table"){ UniqueId = 1634 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 5, 1),
					},
					new List<Item>
					{
						new Item(355, 1, 1),
						new Item(351, 10, 1),
					}, "crafting_table"){ UniqueId = 1635 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 5, 1),
					},
					new List<Item>
					{
						new Item(355, 0, 1),
						new Item(351, 10, 1),
					}, "crafting_table"){ UniqueId = 1636 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 5, 1),
					},
					new List<Item>
					{
						new Item(355, 13, 1),
						new Item(351, 10, 1),
					}, "crafting_table"){ UniqueId = 1624 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 5, 1),
					},
					new List<Item>
					{
						new Item(355, 12, 1),
						new Item(351, 10, 1),
					}, "crafting_table"){ UniqueId = 1625 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 5, 1),
					},
					new List<Item>
					{
						new Item(355, 11, 1),
						new Item(351, 10, 1),
					}, "crafting_table"){ UniqueId = 1626 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 5, 1),
					},
					new List<Item>
					{
						new Item(355, 10, 1),
						new Item(351, 10, 1),
					}, "crafting_table"){ UniqueId = 1627 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 5, 1),
					},
					new List<Item>
					{
						new Item(355, 9, 1),
						new Item(351, 10, 1),
					}, "crafting_table"){ UniqueId = 1628 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 5, 1),
					},
					new List<Item>
					{
						new Item(355, 8, 1),
						new Item(351, 10, 1),
					}, "crafting_table"){ UniqueId = 1629 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 5, 1),
					},
					new List<Item>
					{
						new Item(355, 7, 1),
						new Item(351, 10, 1),
					}, "crafting_table"){ UniqueId = 1630 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 5, 1),
					},
					new List<Item>
					{
						new Item(355, 6, 1),
						new Item(351, 10, 1),
					}, "crafting_table"){ UniqueId = 1631 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 4, 1),
					},
					new List<Item>
					{
						new Item(355, 15, 1),
						new Item(351, 11, 1),
					}, "crafting_table"){ UniqueId = 1607 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 4, 1),
					},
					new List<Item>
					{
						new Item(355, 14, 1),
						new Item(351, 11, 1),
					}, "crafting_table"){ UniqueId = 1608 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 4, 1),
					},
					new List<Item>
					{
						new Item(355, 5, 1),
						new Item(351, 11, 1),
					}, "crafting_table"){ UniqueId = 1617 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 4, 1),
					},
					new List<Item>
					{
						new Item(355, 3, 1),
						new Item(351, 11, 1),
					}, "crafting_table"){ UniqueId = 1618 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 4, 1),
					},
					new List<Item>
					{
						new Item(355, 2, 1),
						new Item(351, 11, 1),
					}, "crafting_table"){ UniqueId = 1619 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 4, 1),
					},
					new List<Item>
					{
						new Item(355, 1, 1),
						new Item(351, 11, 1),
					}, "crafting_table"){ UniqueId = 1620 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 4, 1),
					},
					new List<Item>
					{
						new Item(355, 0, 1),
						new Item(351, 11, 1),
					}, "crafting_table"){ UniqueId = 1621 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 4, 1),
					},
					new List<Item>
					{
						new Item(355, 13, 1),
						new Item(351, 11, 1),
					}, "crafting_table"){ UniqueId = 1609 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 4, 1),
					},
					new List<Item>
					{
						new Item(355, 12, 1),
						new Item(351, 11, 1),
					}, "crafting_table"){ UniqueId = 1610 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 4, 1),
					},
					new List<Item>
					{
						new Item(355, 11, 1),
						new Item(351, 11, 1),
					}, "crafting_table"){ UniqueId = 1611 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 4, 1),
					},
					new List<Item>
					{
						new Item(355, 10, 1),
						new Item(351, 11, 1),
					}, "crafting_table"){ UniqueId = 1612 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 4, 1),
					},
					new List<Item>
					{
						new Item(355, 9, 1),
						new Item(351, 11, 1),
					}, "crafting_table"){ UniqueId = 1613 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 4, 1),
					},
					new List<Item>
					{
						new Item(355, 8, 1),
						new Item(351, 11, 1),
					}, "crafting_table"){ UniqueId = 1614 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 4, 1),
					},
					new List<Item>
					{
						new Item(355, 7, 1),
						new Item(351, 11, 1),
					}, "crafting_table"){ UniqueId = 1615 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 4, 1),
					},
					new List<Item>
					{
						new Item(355, 6, 1),
						new Item(351, 11, 1),
					}, "crafting_table"){ UniqueId = 1616 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 3, 1),
					},
					new List<Item>
					{
						new Item(355, 15, 1),
						new Item(351, 12, 1),
					}, "crafting_table"){ UniqueId = 1592 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 3, 1),
					},
					new List<Item>
					{
						new Item(355, 14, 1),
						new Item(351, 12, 1),
					}, "crafting_table"){ UniqueId = 1593 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 3, 1),
					},
					new List<Item>
					{
						new Item(355, 5, 1),
						new Item(351, 12, 1),
					}, "crafting_table"){ UniqueId = 1602 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 3, 1),
					},
					new List<Item>
					{
						new Item(355, 4, 1),
						new Item(351, 12, 1),
					}, "crafting_table"){ UniqueId = 1603 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 3, 1),
					},
					new List<Item>
					{
						new Item(355, 2, 1),
						new Item(351, 12, 1),
					}, "crafting_table"){ UniqueId = 1604 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 3, 1),
					},
					new List<Item>
					{
						new Item(355, 1, 1),
						new Item(351, 12, 1),
					}, "crafting_table"){ UniqueId = 1605 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 3, 1),
					},
					new List<Item>
					{
						new Item(355, 0, 1),
						new Item(351, 12, 1),
					}, "crafting_table"){ UniqueId = 1606 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 3, 1),
					},
					new List<Item>
					{
						new Item(355, 13, 1),
						new Item(351, 12, 1),
					}, "crafting_table"){ UniqueId = 1594 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 3, 1),
					},
					new List<Item>
					{
						new Item(355, 12, 1),
						new Item(351, 12, 1),
					}, "crafting_table"){ UniqueId = 1595 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 3, 1),
					},
					new List<Item>
					{
						new Item(355, 11, 1),
						new Item(351, 12, 1),
					}, "crafting_table"){ UniqueId = 1596 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 3, 1),
					},
					new List<Item>
					{
						new Item(355, 10, 1),
						new Item(351, 12, 1),
					}, "crafting_table"){ UniqueId = 1597 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 3, 1),
					},
					new List<Item>
					{
						new Item(355, 9, 1),
						new Item(351, 12, 1),
					}, "crafting_table"){ UniqueId = 1598 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 3, 1),
					},
					new List<Item>
					{
						new Item(355, 8, 1),
						new Item(351, 12, 1),
					}, "crafting_table"){ UniqueId = 1599 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 3, 1),
					},
					new List<Item>
					{
						new Item(355, 7, 1),
						new Item(351, 12, 1),
					}, "crafting_table"){ UniqueId = 1600 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 3, 1),
					},
					new List<Item>
					{
						new Item(355, 6, 1),
						new Item(351, 12, 1),
					}, "crafting_table"){ UniqueId = 1601 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 2, 1),
					},
					new List<Item>
					{
						new Item(355, 15, 1),
						new Item(351, 13, 1),
					}, "crafting_table"){ UniqueId = 1577 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 2, 1),
					},
					new List<Item>
					{
						new Item(355, 14, 1),
						new Item(351, 13, 1),
					}, "crafting_table"){ UniqueId = 1578 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 2, 1),
					},
					new List<Item>
					{
						new Item(355, 5, 1),
						new Item(351, 13, 1),
					}, "crafting_table"){ UniqueId = 1587 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 2, 1),
					},
					new List<Item>
					{
						new Item(355, 4, 1),
						new Item(351, 13, 1),
					}, "crafting_table"){ UniqueId = 1588 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 2, 1),
					},
					new List<Item>
					{
						new Item(355, 3, 1),
						new Item(351, 13, 1),
					}, "crafting_table"){ UniqueId = 1589 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 2, 1),
					},
					new List<Item>
					{
						new Item(355, 1, 1),
						new Item(351, 13, 1),
					}, "crafting_table"){ UniqueId = 1590 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 2, 1),
					},
					new List<Item>
					{
						new Item(355, 0, 1),
						new Item(351, 13, 1),
					}, "crafting_table"){ UniqueId = 1591 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 2, 1),
					},
					new List<Item>
					{
						new Item(355, 13, 1),
						new Item(351, 13, 1),
					}, "crafting_table"){ UniqueId = 1579 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 2, 1),
					},
					new List<Item>
					{
						new Item(355, 12, 1),
						new Item(351, 13, 1),
					}, "crafting_table"){ UniqueId = 1580 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 2, 1),
					},
					new List<Item>
					{
						new Item(355, 11, 1),
						new Item(351, 13, 1),
					}, "crafting_table"){ UniqueId = 1581 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 2, 1),
					},
					new List<Item>
					{
						new Item(355, 10, 1),
						new Item(351, 13, 1),
					}, "crafting_table"){ UniqueId = 1582 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 2, 1),
					},
					new List<Item>
					{
						new Item(355, 9, 1),
						new Item(351, 13, 1),
					}, "crafting_table"){ UniqueId = 1583 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 2, 1),
					},
					new List<Item>
					{
						new Item(355, 8, 1),
						new Item(351, 13, 1),
					}, "crafting_table"){ UniqueId = 1584 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 2, 1),
					},
					new List<Item>
					{
						new Item(355, 7, 1),
						new Item(351, 13, 1),
					}, "crafting_table"){ UniqueId = 1585 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 2, 1),
					},
					new List<Item>
					{
						new Item(355, 6, 1),
						new Item(351, 13, 1),
					}, "crafting_table"){ UniqueId = 1586 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 1, 1),
					},
					new List<Item>
					{
						new Item(355, 15, 1),
						new Item(351, 14, 1),
					}, "crafting_table"){ UniqueId = 1562 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 1, 1),
					},
					new List<Item>
					{
						new Item(355, 14, 1),
						new Item(351, 14, 1),
					}, "crafting_table"){ UniqueId = 1563 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 1, 1),
					},
					new List<Item>
					{
						new Item(355, 5, 1),
						new Item(351, 14, 1),
					}, "crafting_table"){ UniqueId = 1572 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 1, 1),
					},
					new List<Item>
					{
						new Item(355, 4, 1),
						new Item(351, 14, 1),
					}, "crafting_table"){ UniqueId = 1573 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 1, 1),
					},
					new List<Item>
					{
						new Item(355, 3, 1),
						new Item(351, 14, 1),
					}, "crafting_table"){ UniqueId = 1574 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 1, 1),
					},
					new List<Item>
					{
						new Item(355, 2, 1),
						new Item(351, 14, 1),
					}, "crafting_table"){ UniqueId = 1575 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 1, 1),
					},
					new List<Item>
					{
						new Item(355, 0, 1),
						new Item(351, 14, 1),
					}, "crafting_table"){ UniqueId = 1576 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 1, 1),
					},
					new List<Item>
					{
						new Item(355, 13, 1),
						new Item(351, 14, 1),
					}, "crafting_table"){ UniqueId = 1564 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 1, 1),
					},
					new List<Item>
					{
						new Item(355, 12, 1),
						new Item(351, 14, 1),
					}, "crafting_table"){ UniqueId = 1565 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 1, 1),
					},
					new List<Item>
					{
						new Item(355, 11, 1),
						new Item(351, 14, 1),
					}, "crafting_table"){ UniqueId = 1566 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 1, 1),
					},
					new List<Item>
					{
						new Item(355, 10, 1),
						new Item(351, 14, 1),
					}, "crafting_table"){ UniqueId = 1567 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 1, 1),
					},
					new List<Item>
					{
						new Item(355, 9, 1),
						new Item(351, 14, 1),
					}, "crafting_table"){ UniqueId = 1568 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 1, 1),
					},
					new List<Item>
					{
						new Item(355, 8, 1),
						new Item(351, 14, 1),
					}, "crafting_table"){ UniqueId = 1569 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 1, 1),
					},
					new List<Item>
					{
						new Item(355, 7, 1),
						new Item(351, 14, 1),
					}, "crafting_table"){ UniqueId = 1570 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 1, 1),
					},
					new List<Item>
					{
						new Item(355, 6, 1),
						new Item(351, 14, 1),
					}, "crafting_table"){ UniqueId = 1571 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 0, 1),
					},
					new List<Item>
					{
						new Item(355, 15, 1),
						new Item(351, 15, 1),
					}, "crafting_table"){ UniqueId = 1547 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 0, 1),
					},
					new List<Item>
					{
						new Item(355, 14, 1),
						new Item(351, 15, 1),
					}, "crafting_table"){ UniqueId = 1548 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 0, 1),
					},
					new List<Item>
					{
						new Item(355, 5, 1),
						new Item(351, 15, 1),
					}, "crafting_table"){ UniqueId = 1557 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 0, 1),
					},
					new List<Item>
					{
						new Item(355, 4, 1),
						new Item(351, 15, 1),
					}, "crafting_table"){ UniqueId = 1558 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 0, 1),
					},
					new List<Item>
					{
						new Item(355, 3, 1),
						new Item(351, 15, 1),
					}, "crafting_table"){ UniqueId = 1559 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 0, 1),
					},
					new List<Item>
					{
						new Item(355, 2, 1),
						new Item(351, 15, 1),
					}, "crafting_table"){ UniqueId = 1560 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 0, 1),
					},
					new List<Item>
					{
						new Item(355, 1, 1),
						new Item(351, 15, 1),
					}, "crafting_table"){ UniqueId = 1561 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 0, 1),
					},
					new List<Item>
					{
						new Item(355, 13, 1),
						new Item(351, 15, 1),
					}, "crafting_table"){ UniqueId = 1549 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 0, 1),
					},
					new List<Item>
					{
						new Item(355, 12, 1),
						new Item(351, 15, 1),
					}, "crafting_table"){ UniqueId = 1550 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 0, 1),
					},
					new List<Item>
					{
						new Item(355, 11, 1),
						new Item(351, 15, 1),
					}, "crafting_table"){ UniqueId = 1551 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 0, 1),
					},
					new List<Item>
					{
						new Item(355, 10, 1),
						new Item(351, 15, 1),
					}, "crafting_table"){ UniqueId = 1552 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 0, 1),
					},
					new List<Item>
					{
						new Item(355, 9, 1),
						new Item(351, 15, 1),
					}, "crafting_table"){ UniqueId = 1553 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 0, 1),
					},
					new List<Item>
					{
						new Item(355, 8, 1),
						new Item(351, 15, 1),
					}, "crafting_table"){ UniqueId = 1554 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 0, 1),
					},
					new List<Item>
					{
						new Item(355, 7, 1),
						new Item(351, 15, 1),
					}, "crafting_table"){ UniqueId = 1555 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 0, 1),
					},
					new List<Item>
					{
						new Item(355, 6, 1),
						new Item(351, 15, 1),
					}, "crafting_table"){ UniqueId = 1556 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 15, 1),
					},
					new List<Item>
					{
						new Item(355, 14, 1),
						new Item(351, 16, 1),
					}, "crafting_table"){ UniqueId = 1532 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 15, 1),
					},
					new List<Item>
					{
						new Item(355, 5, 1),
						new Item(351, 16, 1),
					}, "crafting_table"){ UniqueId = 1541 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 15, 1),
					},
					new List<Item>
					{
						new Item(355, 4, 1),
						new Item(351, 16, 1),
					}, "crafting_table"){ UniqueId = 1542 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 15, 1),
					},
					new List<Item>
					{
						new Item(355, 3, 1),
						new Item(351, 16, 1),
					}, "crafting_table"){ UniqueId = 1543 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 15, 1),
					},
					new List<Item>
					{
						new Item(355, 2, 1),
						new Item(351, 16, 1),
					}, "crafting_table"){ UniqueId = 1544 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 15, 1),
					},
					new List<Item>
					{
						new Item(355, 1, 1),
						new Item(351, 16, 1),
					}, "crafting_table"){ UniqueId = 1545 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 15, 1),
					},
					new List<Item>
					{
						new Item(355, 0, 1),
						new Item(351, 16, 1),
					}, "crafting_table"){ UniqueId = 1546 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 15, 1),
					},
					new List<Item>
					{
						new Item(355, 13, 1),
						new Item(351, 16, 1),
					}, "crafting_table"){ UniqueId = 1533 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 15, 1),
					},
					new List<Item>
					{
						new Item(355, 12, 1),
						new Item(351, 16, 1),
					}, "crafting_table"){ UniqueId = 1534 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 15, 1),
					},
					new List<Item>
					{
						new Item(355, 11, 1),
						new Item(351, 16, 1),
					}, "crafting_table"){ UniqueId = 1535 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 15, 1),
					},
					new List<Item>
					{
						new Item(355, 10, 1),
						new Item(351, 16, 1),
					}, "crafting_table"){ UniqueId = 1536 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 15, 1),
					},
					new List<Item>
					{
						new Item(355, 9, 1),
						new Item(351, 16, 1),
					}, "crafting_table"){ UniqueId = 1537 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 15, 1),
					},
					new List<Item>
					{
						new Item(355, 8, 1),
						new Item(351, 16, 1),
					}, "crafting_table"){ UniqueId = 1538 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 15, 1),
					},
					new List<Item>
					{
						new Item(355, 7, 1),
						new Item(351, 16, 1),
					}, "crafting_table"){ UniqueId = 1539 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 15, 1),
					},
					new List<Item>
					{
						new Item(355, 6, 1),
						new Item(351, 16, 1),
					}, "crafting_table"){ UniqueId = 1540 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 12, 1),
					},
					new List<Item>
					{
						new Item(355, 15, 1),
						new Item(351, 17, 1),
					}, "crafting_table"){ UniqueId = 1517 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 12, 1),
					},
					new List<Item>
					{
						new Item(355, 14, 1),
						new Item(351, 17, 1),
					}, "crafting_table"){ UniqueId = 1518 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 12, 1),
					},
					new List<Item>
					{
						new Item(355, 5, 1),
						new Item(351, 17, 1),
					}, "crafting_table"){ UniqueId = 1526 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 12, 1),
					},
					new List<Item>
					{
						new Item(355, 4, 1),
						new Item(351, 17, 1),
					}, "crafting_table"){ UniqueId = 1527 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 12, 1),
					},
					new List<Item>
					{
						new Item(355, 3, 1),
						new Item(351, 17, 1),
					}, "crafting_table"){ UniqueId = 1528 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 12, 1),
					},
					new List<Item>
					{
						new Item(355, 2, 1),
						new Item(351, 17, 1),
					}, "crafting_table"){ UniqueId = 1529 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 12, 1),
					},
					new List<Item>
					{
						new Item(355, 1, 1),
						new Item(351, 17, 1),
					}, "crafting_table"){ UniqueId = 1530 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 12, 1),
					},
					new List<Item>
					{
						new Item(355, 0, 1),
						new Item(351, 17, 1),
					}, "crafting_table"){ UniqueId = 1531 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 12, 1),
					},
					new List<Item>
					{
						new Item(355, 13, 1),
						new Item(351, 17, 1),
					}, "crafting_table"){ UniqueId = 1519 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 12, 1),
					},
					new List<Item>
					{
						new Item(355, 11, 1),
						new Item(351, 17, 1),
					}, "crafting_table"){ UniqueId = 1520 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 12, 1),
					},
					new List<Item>
					{
						new Item(355, 10, 1),
						new Item(351, 17, 1),
					}, "crafting_table"){ UniqueId = 1521 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 12, 1),
					},
					new List<Item>
					{
						new Item(355, 9, 1),
						new Item(351, 17, 1),
					}, "crafting_table"){ UniqueId = 1522 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 12, 1),
					},
					new List<Item>
					{
						new Item(355, 8, 1),
						new Item(351, 17, 1),
					}, "crafting_table"){ UniqueId = 1523 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 12, 1),
					},
					new List<Item>
					{
						new Item(355, 7, 1),
						new Item(351, 17, 1),
					}, "crafting_table"){ UniqueId = 1524 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 12, 1),
					},
					new List<Item>
					{
						new Item(355, 6, 1),
						new Item(351, 17, 1),
					}, "crafting_table"){ UniqueId = 1525 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 11, 1),
					},
					new List<Item>
					{
						new Item(355, 15, 1),
						new Item(351, 18, 1),
					}, "crafting_table"){ UniqueId = 1502 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 11, 1),
					},
					new List<Item>
					{
						new Item(355, 14, 1),
						new Item(351, 18, 1),
					}, "crafting_table"){ UniqueId = 1503 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 11, 1),
					},
					new List<Item>
					{
						new Item(355, 5, 1),
						new Item(351, 18, 1),
					}, "crafting_table"){ UniqueId = 1511 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 11, 1),
					},
					new List<Item>
					{
						new Item(355, 4, 1),
						new Item(351, 18, 1),
					}, "crafting_table"){ UniqueId = 1512 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 11, 1),
					},
					new List<Item>
					{
						new Item(355, 3, 1),
						new Item(351, 18, 1),
					}, "crafting_table"){ UniqueId = 1513 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 11, 1),
					},
					new List<Item>
					{
						new Item(355, 2, 1),
						new Item(351, 18, 1),
					}, "crafting_table"){ UniqueId = 1514 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 11, 1),
					},
					new List<Item>
					{
						new Item(355, 1, 1),
						new Item(351, 18, 1),
					}, "crafting_table"){ UniqueId = 1515 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 11, 1),
					},
					new List<Item>
					{
						new Item(355, 0, 1),
						new Item(351, 18, 1),
					}, "crafting_table"){ UniqueId = 1516 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 11, 1),
					},
					new List<Item>
					{
						new Item(355, 13, 1),
						new Item(351, 18, 1),
					}, "crafting_table"){ UniqueId = 1504 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 11, 1),
					},
					new List<Item>
					{
						new Item(355, 12, 1),
						new Item(351, 18, 1),
					}, "crafting_table"){ UniqueId = 1505 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 11, 1),
					},
					new List<Item>
					{
						new Item(355, 10, 1),
						new Item(351, 18, 1),
					}, "crafting_table"){ UniqueId = 1506 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 11, 1),
					},
					new List<Item>
					{
						new Item(355, 9, 1),
						new Item(351, 18, 1),
					}, "crafting_table"){ UniqueId = 1507 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 11, 1),
					},
					new List<Item>
					{
						new Item(355, 8, 1),
						new Item(351, 18, 1),
					}, "crafting_table"){ UniqueId = 1508 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 11, 1),
					},
					new List<Item>
					{
						new Item(355, 7, 1),
						new Item(351, 18, 1),
					}, "crafting_table"){ UniqueId = 1509 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 11, 1),
					},
					new List<Item>
					{
						new Item(355, 6, 1),
						new Item(351, 18, 1),
					}, "crafting_table"){ UniqueId = 1510 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 0, 1),
					},
					new List<Item>
					{
						new Item(355, 15, 1),
						new Item(351, 19, 1),
					}, "crafting_table"){ UniqueId = 1487 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 0, 1),
					},
					new List<Item>
					{
						new Item(355, 14, 1),
						new Item(351, 19, 1),
					}, "crafting_table"){ UniqueId = 1488 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 0, 1),
					},
					new List<Item>
					{
						new Item(355, 5, 1),
						new Item(351, 19, 1),
					}, "crafting_table"){ UniqueId = 1497 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 0, 1),
					},
					new List<Item>
					{
						new Item(355, 4, 1),
						new Item(351, 19, 1),
					}, "crafting_table"){ UniqueId = 1498 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 0, 1),
					},
					new List<Item>
					{
						new Item(355, 3, 1),
						new Item(351, 19, 1),
					}, "crafting_table"){ UniqueId = 1499 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 0, 1),
					},
					new List<Item>
					{
						new Item(355, 2, 1),
						new Item(351, 19, 1),
					}, "crafting_table"){ UniqueId = 1500 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 0, 1),
					},
					new List<Item>
					{
						new Item(355, 1, 1),
						new Item(351, 19, 1),
					}, "crafting_table"){ UniqueId = 1501 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 0, 1),
					},
					new List<Item>
					{
						new Item(355, 13, 1),
						new Item(351, 19, 1),
					}, "crafting_table"){ UniqueId = 1489 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 0, 1),
					},
					new List<Item>
					{
						new Item(355, 12, 1),
						new Item(351, 19, 1),
					}, "crafting_table"){ UniqueId = 1490 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 0, 1),
					},
					new List<Item>
					{
						new Item(355, 11, 1),
						new Item(351, 19, 1),
					}, "crafting_table"){ UniqueId = 1491 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 0, 1),
					},
					new List<Item>
					{
						new Item(355, 10, 1),
						new Item(351, 19, 1),
					}, "crafting_table"){ UniqueId = 1492 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 0, 1),
					},
					new List<Item>
					{
						new Item(355, 9, 1),
						new Item(351, 19, 1),
					}, "crafting_table"){ UniqueId = 1493 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 0, 1),
					},
					new List<Item>
					{
						new Item(355, 8, 1),
						new Item(351, 19, 1),
					}, "crafting_table"){ UniqueId = 1494 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 0, 1),
					},
					new List<Item>
					{
						new Item(355, 7, 1),
						new Item(351, 19, 1),
					}, "crafting_table"){ UniqueId = 1495 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 0, 1),
					},
					new List<Item>
					{
						new Item(355, 6, 1),
						new Item(351, 19, 1),
					}, "crafting_table"){ UniqueId = 1496 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 14, 1),
					},
					new List<Item>
					{
						new Item(355, 15, 1),
						new Item(351, 1, 1),
					}, "crafting_table"){ UniqueId = 1757 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 14, 1),
					},
					new List<Item>
					{
						new Item(355, 5, 1),
						new Item(351, 1, 1),
					}, "crafting_table"){ UniqueId = 1766 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 14, 1),
					},
					new List<Item>
					{
						new Item(355, 4, 1),
						new Item(351, 1, 1),
					}, "crafting_table"){ UniqueId = 1767 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 14, 1),
					},
					new List<Item>
					{
						new Item(355, 3, 1),
						new Item(351, 1, 1),
					}, "crafting_table"){ UniqueId = 1768 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 14, 1),
					},
					new List<Item>
					{
						new Item(355, 2, 1),
						new Item(351, 1, 1),
					}, "crafting_table"){ UniqueId = 1769 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 14, 1),
					},
					new List<Item>
					{
						new Item(355, 1, 1),
						new Item(351, 1, 1),
					}, "crafting_table"){ UniqueId = 1770 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 14, 1),
					},
					new List<Item>
					{
						new Item(355, 0, 1),
						new Item(351, 1, 1),
					}, "crafting_table"){ UniqueId = 1771 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 14, 1),
					},
					new List<Item>
					{
						new Item(355, 13, 1),
						new Item(351, 1, 1),
					}, "crafting_table"){ UniqueId = 1758 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 14, 1),
					},
					new List<Item>
					{
						new Item(355, 12, 1),
						new Item(351, 1, 1),
					}, "crafting_table"){ UniqueId = 1759 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 14, 1),
					},
					new List<Item>
					{
						new Item(355, 11, 1),
						new Item(351, 1, 1),
					}, "crafting_table"){ UniqueId = 1760 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 14, 1),
					},
					new List<Item>
					{
						new Item(355, 10, 1),
						new Item(351, 1, 1),
					}, "crafting_table"){ UniqueId = 1761 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 14, 1),
					},
					new List<Item>
					{
						new Item(355, 9, 1),
						new Item(351, 1, 1),
					}, "crafting_table"){ UniqueId = 1762 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 14, 1),
					},
					new List<Item>
					{
						new Item(355, 8, 1),
						new Item(351, 1, 1),
					}, "crafting_table"){ UniqueId = 1763 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 14, 1),
					},
					new List<Item>
					{
						new Item(355, 7, 1),
						new Item(351, 1, 1),
					}, "crafting_table"){ UniqueId = 1764 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 14, 1),
					},
					new List<Item>
					{
						new Item(355, 6, 1),
						new Item(351, 1, 1),
					}, "crafting_table"){ UniqueId = 1765 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 13, 1),
					},
					new List<Item>
					{
						new Item(355, 15, 1),
						new Item(351, 2, 1),
					}, "crafting_table"){ UniqueId = 1742 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 13, 1),
					},
					new List<Item>
					{
						new Item(355, 14, 1),
						new Item(351, 2, 1),
					}, "crafting_table"){ UniqueId = 1743 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 13, 1),
					},
					new List<Item>
					{
						new Item(355, 5, 1),
						new Item(351, 2, 1),
					}, "crafting_table"){ UniqueId = 1751 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 13, 1),
					},
					new List<Item>
					{
						new Item(355, 4, 1),
						new Item(351, 2, 1),
					}, "crafting_table"){ UniqueId = 1752 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 13, 1),
					},
					new List<Item>
					{
						new Item(355, 3, 1),
						new Item(351, 2, 1),
					}, "crafting_table"){ UniqueId = 1753 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 13, 1),
					},
					new List<Item>
					{
						new Item(355, 2, 1),
						new Item(351, 2, 1),
					}, "crafting_table"){ UniqueId = 1754 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 13, 1),
					},
					new List<Item>
					{
						new Item(355, 1, 1),
						new Item(351, 2, 1),
					}, "crafting_table"){ UniqueId = 1755 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 13, 1),
					},
					new List<Item>
					{
						new Item(355, 0, 1),
						new Item(351, 2, 1),
					}, "crafting_table"){ UniqueId = 1756 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 13, 1),
					},
					new List<Item>
					{
						new Item(355, 12, 1),
						new Item(351, 2, 1),
					}, "crafting_table"){ UniqueId = 1744 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 13, 1),
					},
					new List<Item>
					{
						new Item(355, 11, 1),
						new Item(351, 2, 1),
					}, "crafting_table"){ UniqueId = 1745 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 13, 1),
					},
					new List<Item>
					{
						new Item(355, 10, 1),
						new Item(351, 2, 1),
					}, "crafting_table"){ UniqueId = 1746 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 13, 1),
					},
					new List<Item>
					{
						new Item(355, 9, 1),
						new Item(351, 2, 1),
					}, "crafting_table"){ UniqueId = 1747 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 13, 1),
					},
					new List<Item>
					{
						new Item(355, 8, 1),
						new Item(351, 2, 1),
					}, "crafting_table"){ UniqueId = 1748 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 13, 1),
					},
					new List<Item>
					{
						new Item(355, 7, 1),
						new Item(351, 2, 1),
					}, "crafting_table"){ UniqueId = 1749 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 13, 1),
					},
					new List<Item>
					{
						new Item(355, 6, 1),
						new Item(351, 2, 1),
					}, "crafting_table"){ UniqueId = 1750 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 12, 1),
					},
					new List<Item>
					{
						new Item(355, 15, 1),
						new Item(351, 3, 1),
					}, "crafting_table"){ UniqueId = 1727 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 12, 1),
					},
					new List<Item>
					{
						new Item(355, 14, 1),
						new Item(351, 3, 1),
					}, "crafting_table"){ UniqueId = 1728 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 12, 1),
					},
					new List<Item>
					{
						new Item(355, 5, 1),
						new Item(351, 3, 1),
					}, "crafting_table"){ UniqueId = 1736 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 12, 1),
					},
					new List<Item>
					{
						new Item(355, 4, 1),
						new Item(351, 3, 1),
					}, "crafting_table"){ UniqueId = 1737 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 12, 1),
					},
					new List<Item>
					{
						new Item(355, 3, 1),
						new Item(351, 3, 1),
					}, "crafting_table"){ UniqueId = 1738 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 12, 1),
					},
					new List<Item>
					{
						new Item(355, 2, 1),
						new Item(351, 3, 1),
					}, "crafting_table"){ UniqueId = 1739 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 12, 1),
					},
					new List<Item>
					{
						new Item(355, 1, 1),
						new Item(351, 3, 1),
					}, "crafting_table"){ UniqueId = 1740 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 12, 1),
					},
					new List<Item>
					{
						new Item(355, 0, 1),
						new Item(351, 3, 1),
					}, "crafting_table"){ UniqueId = 1741 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 12, 1),
					},
					new List<Item>
					{
						new Item(355, 13, 1),
						new Item(351, 3, 1),
					}, "crafting_table"){ UniqueId = 1729 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 12, 1),
					},
					new List<Item>
					{
						new Item(355, 11, 1),
						new Item(351, 3, 1),
					}, "crafting_table"){ UniqueId = 1730 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 12, 1),
					},
					new List<Item>
					{
						new Item(355, 10, 1),
						new Item(351, 3, 1),
					}, "crafting_table"){ UniqueId = 1731 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 12, 1),
					},
					new List<Item>
					{
						new Item(355, 9, 1),
						new Item(351, 3, 1),
					}, "crafting_table"){ UniqueId = 1732 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 12, 1),
					},
					new List<Item>
					{
						new Item(355, 8, 1),
						new Item(351, 3, 1),
					}, "crafting_table"){ UniqueId = 1733 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 12, 1),
					},
					new List<Item>
					{
						new Item(355, 7, 1),
						new Item(351, 3, 1),
					}, "crafting_table"){ UniqueId = 1734 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 12, 1),
					},
					new List<Item>
					{
						new Item(355, 6, 1),
						new Item(351, 3, 1),
					}, "crafting_table"){ UniqueId = 1735 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 11, 1),
					},
					new List<Item>
					{
						new Item(355, 15, 1),
						new Item(351, 4, 1),
					}, "crafting_table"){ UniqueId = 1712 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 11, 1),
					},
					new List<Item>
					{
						new Item(355, 14, 1),
						new Item(351, 4, 1),
					}, "crafting_table"){ UniqueId = 1713 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 11, 1),
					},
					new List<Item>
					{
						new Item(355, 5, 1),
						new Item(351, 4, 1),
					}, "crafting_table"){ UniqueId = 1721 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 11, 1),
					},
					new List<Item>
					{
						new Item(355, 4, 1),
						new Item(351, 4, 1),
					}, "crafting_table"){ UniqueId = 1722 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 11, 1),
					},
					new List<Item>
					{
						new Item(355, 3, 1),
						new Item(351, 4, 1),
					}, "crafting_table"){ UniqueId = 1723 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 11, 1),
					},
					new List<Item>
					{
						new Item(355, 2, 1),
						new Item(351, 4, 1),
					}, "crafting_table"){ UniqueId = 1724 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 11, 1),
					},
					new List<Item>
					{
						new Item(355, 1, 1),
						new Item(351, 4, 1),
					}, "crafting_table"){ UniqueId = 1725 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 11, 1),
					},
					new List<Item>
					{
						new Item(355, 0, 1),
						new Item(351, 4, 1),
					}, "crafting_table"){ UniqueId = 1726 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 11, 1),
					},
					new List<Item>
					{
						new Item(355, 13, 1),
						new Item(351, 4, 1),
					}, "crafting_table"){ UniqueId = 1714 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 11, 1),
					},
					new List<Item>
					{
						new Item(355, 12, 1),
						new Item(351, 4, 1),
					}, "crafting_table"){ UniqueId = 1715 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 11, 1),
					},
					new List<Item>
					{
						new Item(355, 10, 1),
						new Item(351, 4, 1),
					}, "crafting_table"){ UniqueId = 1716 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 11, 1),
					},
					new List<Item>
					{
						new Item(355, 9, 1),
						new Item(351, 4, 1),
					}, "crafting_table"){ UniqueId = 1717 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 11, 1),
					},
					new List<Item>
					{
						new Item(355, 8, 1),
						new Item(351, 4, 1),
					}, "crafting_table"){ UniqueId = 1718 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 11, 1),
					},
					new List<Item>
					{
						new Item(355, 7, 1),
						new Item(351, 4, 1),
					}, "crafting_table"){ UniqueId = 1719 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 11, 1),
					},
					new List<Item>
					{
						new Item(355, 6, 1),
						new Item(351, 4, 1),
					}, "crafting_table"){ UniqueId = 1720 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 10, 1),
					},
					new List<Item>
					{
						new Item(355, 15, 1),
						new Item(351, 5, 1),
					}, "crafting_table"){ UniqueId = 1697 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 10, 1),
					},
					new List<Item>
					{
						new Item(355, 14, 1),
						new Item(351, 5, 1),
					}, "crafting_table"){ UniqueId = 1698 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 10, 1),
					},
					new List<Item>
					{
						new Item(355, 5, 1),
						new Item(351, 5, 1),
					}, "crafting_table"){ UniqueId = 1706 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 10, 1),
					},
					new List<Item>
					{
						new Item(355, 4, 1),
						new Item(351, 5, 1),
					}, "crafting_table"){ UniqueId = 1707 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 10, 1),
					},
					new List<Item>
					{
						new Item(355, 3, 1),
						new Item(351, 5, 1),
					}, "crafting_table"){ UniqueId = 1708 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 10, 1),
					},
					new List<Item>
					{
						new Item(355, 2, 1),
						new Item(351, 5, 1),
					}, "crafting_table"){ UniqueId = 1709 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 10, 1),
					},
					new List<Item>
					{
						new Item(355, 1, 1),
						new Item(351, 5, 1),
					}, "crafting_table"){ UniqueId = 1710 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 10, 1),
					},
					new List<Item>
					{
						new Item(355, 0, 1),
						new Item(351, 5, 1),
					}, "crafting_table"){ UniqueId = 1711 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 10, 1),
					},
					new List<Item>
					{
						new Item(355, 13, 1),
						new Item(351, 5, 1),
					}, "crafting_table"){ UniqueId = 1699 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 10, 1),
					},
					new List<Item>
					{
						new Item(355, 12, 1),
						new Item(351, 5, 1),
					}, "crafting_table"){ UniqueId = 1700 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 10, 1),
					},
					new List<Item>
					{
						new Item(355, 11, 1),
						new Item(351, 5, 1),
					}, "crafting_table"){ UniqueId = 1701 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 10, 1),
					},
					new List<Item>
					{
						new Item(355, 9, 1),
						new Item(351, 5, 1),
					}, "crafting_table"){ UniqueId = 1702 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 10, 1),
					},
					new List<Item>
					{
						new Item(355, 8, 1),
						new Item(351, 5, 1),
					}, "crafting_table"){ UniqueId = 1703 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 10, 1),
					},
					new List<Item>
					{
						new Item(355, 7, 1),
						new Item(351, 5, 1),
					}, "crafting_table"){ UniqueId = 1704 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 10, 1),
					},
					new List<Item>
					{
						new Item(355, 6, 1),
						new Item(351, 5, 1),
					}, "crafting_table"){ UniqueId = 1705 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 9, 1),
					},
					new List<Item>
					{
						new Item(355, 15, 1),
						new Item(351, 6, 1),
					}, "crafting_table"){ UniqueId = 1682 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 9, 1),
					},
					new List<Item>
					{
						new Item(355, 14, 1),
						new Item(351, 6, 1),
					}, "crafting_table"){ UniqueId = 1683 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 9, 1),
					},
					new List<Item>
					{
						new Item(355, 5, 1),
						new Item(351, 6, 1),
					}, "crafting_table"){ UniqueId = 1691 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 9, 1),
					},
					new List<Item>
					{
						new Item(355, 4, 1),
						new Item(351, 6, 1),
					}, "crafting_table"){ UniqueId = 1692 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 9, 1),
					},
					new List<Item>
					{
						new Item(355, 3, 1),
						new Item(351, 6, 1),
					}, "crafting_table"){ UniqueId = 1693 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 9, 1),
					},
					new List<Item>
					{
						new Item(355, 2, 1),
						new Item(351, 6, 1),
					}, "crafting_table"){ UniqueId = 1694 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 9, 1),
					},
					new List<Item>
					{
						new Item(355, 1, 1),
						new Item(351, 6, 1),
					}, "crafting_table"){ UniqueId = 1695 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 9, 1),
					},
					new List<Item>
					{
						new Item(355, 0, 1),
						new Item(351, 6, 1),
					}, "crafting_table"){ UniqueId = 1696 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 9, 1),
					},
					new List<Item>
					{
						new Item(355, 13, 1),
						new Item(351, 6, 1),
					}, "crafting_table"){ UniqueId = 1684 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 9, 1),
					},
					new List<Item>
					{
						new Item(355, 12, 1),
						new Item(351, 6, 1),
					}, "crafting_table"){ UniqueId = 1685 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 9, 1),
					},
					new List<Item>
					{
						new Item(355, 11, 1),
						new Item(351, 6, 1),
					}, "crafting_table"){ UniqueId = 1686 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 9, 1),
					},
					new List<Item>
					{
						new Item(355, 10, 1),
						new Item(351, 6, 1),
					}, "crafting_table"){ UniqueId = 1687 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 9, 1),
					},
					new List<Item>
					{
						new Item(355, 8, 1),
						new Item(351, 6, 1),
					}, "crafting_table"){ UniqueId = 1688 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 9, 1),
					},
					new List<Item>
					{
						new Item(355, 7, 1),
						new Item(351, 6, 1),
					}, "crafting_table"){ UniqueId = 1689 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 9, 1),
					},
					new List<Item>
					{
						new Item(355, 6, 1),
						new Item(351, 6, 1),
					}, "crafting_table"){ UniqueId = 1690 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 8, 1),
					},
					new List<Item>
					{
						new Item(355, 15, 1),
						new Item(351, 7, 1),
					}, "crafting_table"){ UniqueId = 1667 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 8, 1),
					},
					new List<Item>
					{
						new Item(355, 14, 1),
						new Item(351, 7, 1),
					}, "crafting_table"){ UniqueId = 1668 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 8, 1),
					},
					new List<Item>
					{
						new Item(355, 5, 1),
						new Item(351, 7, 1),
					}, "crafting_table"){ UniqueId = 1676 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 8, 1),
					},
					new List<Item>
					{
						new Item(355, 4, 1),
						new Item(351, 7, 1),
					}, "crafting_table"){ UniqueId = 1677 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 8, 1),
					},
					new List<Item>
					{
						new Item(355, 3, 1),
						new Item(351, 7, 1),
					}, "crafting_table"){ UniqueId = 1678 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 8, 1),
					},
					new List<Item>
					{
						new Item(355, 2, 1),
						new Item(351, 7, 1),
					}, "crafting_table"){ UniqueId = 1679 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 8, 1),
					},
					new List<Item>
					{
						new Item(355, 1, 1),
						new Item(351, 7, 1),
					}, "crafting_table"){ UniqueId = 1680 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 8, 1),
					},
					new List<Item>
					{
						new Item(355, 0, 1),
						new Item(351, 7, 1),
					}, "crafting_table"){ UniqueId = 1681 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 8, 1),
					},
					new List<Item>
					{
						new Item(355, 13, 1),
						new Item(351, 7, 1),
					}, "crafting_table"){ UniqueId = 1669 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 8, 1),
					},
					new List<Item>
					{
						new Item(355, 12, 1),
						new Item(351, 7, 1),
					}, "crafting_table"){ UniqueId = 1670 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 8, 1),
					},
					new List<Item>
					{
						new Item(355, 11, 1),
						new Item(351, 7, 1),
					}, "crafting_table"){ UniqueId = 1671 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 8, 1),
					},
					new List<Item>
					{
						new Item(355, 10, 1),
						new Item(351, 7, 1),
					}, "crafting_table"){ UniqueId = 1672 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 8, 1),
					},
					new List<Item>
					{
						new Item(355, 9, 1),
						new Item(351, 7, 1),
					}, "crafting_table"){ UniqueId = 1673 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 8, 1),
					},
					new List<Item>
					{
						new Item(355, 7, 1),
						new Item(351, 7, 1),
					}, "crafting_table"){ UniqueId = 1674 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 8, 1),
					},
					new List<Item>
					{
						new Item(355, 6, 1),
						new Item(351, 7, 1),
					}, "crafting_table"){ UniqueId = 1675 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 7, 1),
					},
					new List<Item>
					{
						new Item(355, 15, 1),
						new Item(351, 8, 1),
					}, "crafting_table"){ UniqueId = 1652 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 7, 1),
					},
					new List<Item>
					{
						new Item(355, 14, 1),
						new Item(351, 8, 1),
					}, "crafting_table"){ UniqueId = 1653 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 7, 1),
					},
					new List<Item>
					{
						new Item(355, 5, 1),
						new Item(351, 8, 1),
					}, "crafting_table"){ UniqueId = 1661 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 7, 1),
					},
					new List<Item>
					{
						new Item(355, 4, 1),
						new Item(351, 8, 1),
					}, "crafting_table"){ UniqueId = 1662 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 7, 1),
					},
					new List<Item>
					{
						new Item(355, 3, 1),
						new Item(351, 8, 1),
					}, "crafting_table"){ UniqueId = 1663 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 7, 1),
					},
					new List<Item>
					{
						new Item(355, 2, 1),
						new Item(351, 8, 1),
					}, "crafting_table"){ UniqueId = 1664 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 7, 1),
					},
					new List<Item>
					{
						new Item(355, 1, 1),
						new Item(351, 8, 1),
					}, "crafting_table"){ UniqueId = 1665 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 7, 1),
					},
					new List<Item>
					{
						new Item(355, 0, 1),
						new Item(351, 8, 1),
					}, "crafting_table"){ UniqueId = 1666 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 7, 1),
					},
					new List<Item>
					{
						new Item(355, 13, 1),
						new Item(351, 8, 1),
					}, "crafting_table"){ UniqueId = 1654 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 7, 1),
					},
					new List<Item>
					{
						new Item(355, 12, 1),
						new Item(351, 8, 1),
					}, "crafting_table"){ UniqueId = 1655 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 7, 1),
					},
					new List<Item>
					{
						new Item(355, 11, 1),
						new Item(351, 8, 1),
					}, "crafting_table"){ UniqueId = 1656 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 7, 1),
					},
					new List<Item>
					{
						new Item(355, 10, 1),
						new Item(351, 8, 1),
					}, "crafting_table"){ UniqueId = 1657 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 7, 1),
					},
					new List<Item>
					{
						new Item(355, 9, 1),
						new Item(351, 8, 1),
					}, "crafting_table"){ UniqueId = 1658 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 7, 1),
					},
					new List<Item>
					{
						new Item(355, 8, 1),
						new Item(351, 8, 1),
					}, "crafting_table"){ UniqueId = 1659 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 7, 1),
					},
					new List<Item>
					{
						new Item(355, 6, 1),
						new Item(351, 8, 1),
					}, "crafting_table"){ UniqueId = 1660 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 6, 1),
					},
					new List<Item>
					{
						new Item(355, 15, 1),
						new Item(351, 9, 1),
					}, "crafting_table"){ UniqueId = 1637 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 6, 1),
					},
					new List<Item>
					{
						new Item(355, 14, 1),
						new Item(351, 9, 1),
					}, "crafting_table"){ UniqueId = 1638 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 6, 1),
					},
					new List<Item>
					{
						new Item(355, 5, 1),
						new Item(351, 9, 1),
					}, "crafting_table"){ UniqueId = 1646 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 6, 1),
					},
					new List<Item>
					{
						new Item(355, 4, 1),
						new Item(351, 9, 1),
					}, "crafting_table"){ UniqueId = 1647 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 6, 1),
					},
					new List<Item>
					{
						new Item(355, 3, 1),
						new Item(351, 9, 1),
					}, "crafting_table"){ UniqueId = 1648 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 6, 1),
					},
					new List<Item>
					{
						new Item(355, 2, 1),
						new Item(351, 9, 1),
					}, "crafting_table"){ UniqueId = 1649 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 6, 1),
					},
					new List<Item>
					{
						new Item(355, 1, 1),
						new Item(351, 9, 1),
					}, "crafting_table"){ UniqueId = 1650 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 6, 1),
					},
					new List<Item>
					{
						new Item(355, 0, 1),
						new Item(351, 9, 1),
					}, "crafting_table"){ UniqueId = 1651 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 6, 1),
					},
					new List<Item>
					{
						new Item(355, 13, 1),
						new Item(351, 9, 1),
					}, "crafting_table"){ UniqueId = 1639 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 6, 1),
					},
					new List<Item>
					{
						new Item(355, 12, 1),
						new Item(351, 9, 1),
					}, "crafting_table"){ UniqueId = 1640 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 6, 1),
					},
					new List<Item>
					{
						new Item(355, 11, 1),
						new Item(351, 9, 1),
					}, "crafting_table"){ UniqueId = 1641 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 6, 1),
					},
					new List<Item>
					{
						new Item(355, 10, 1),
						new Item(351, 9, 1),
					}, "crafting_table"){ UniqueId = 1642 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 6, 1),
					},
					new List<Item>
					{
						new Item(355, 9, 1),
						new Item(351, 9, 1),
					}, "crafting_table"){ UniqueId = 1643 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 6, 1),
					},
					new List<Item>
					{
						new Item(355, 8, 1),
						new Item(351, 9, 1),
					}, "crafting_table"){ UniqueId = 1644 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(355, 6, 1),
					},
					new List<Item>
					{
						new Item(355, 7, 1),
						new Item(351, 9, 1),
					}, "crafting_table"){ UniqueId = 1645 },
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
					}, "crafting_table"){ UniqueId = 1852 },
				new MultiRecipe() { Id = new UUID("d1ca6b84-338e-4f2f-9c6b-76cc8b4bd98d"), UniqueId = 1957 }, // d1ca6b84-338e-4f2f-9c6b-76cc8b4bd98d
				new ShapedRecipe(1, 2,
					new List<Item>
					{
						new Item(155, 1),
					},
					new Item[]
					{
						new Item(44, 6),
						new Item(44, 6),
					}, "crafting_table"){ UniqueId = 1856 },
				new ShapedRecipe(1, 2,
					new List<Item>
					{
						new Item(98, 3),
					},
					new Item[]
					{
						new Item(44, 5),
						new Item(44, 5),
					}, "crafting_table"){ UniqueId = 1859 },
				new MultiRecipe() { Id = new UUID("85939755-ba10-4d9d-a4cc-efb7a8e943c4"), UniqueId = 1949 }, // 85939755-ba10-4d9d-a4cc-efb7a8e943c4
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
					}, "crafting_table"){ UniqueId = 1855 },
				new MultiRecipe() { Id = new UUID("d392b075-4ba1-40ae-8789-af868d56f6ce"), UniqueId = 1951 }, // d392b075-4ba1-40ae-8789-af868d56f6ce
				new ShapedRecipe(1, 2,
					new List<Item>
					{
						new Item(179, 1),
					},
					new Item[]
					{
						new Item(182, 0),
						new Item(182, 0),
					}, "crafting_table"){ UniqueId = 1857 },
				new ShapedRecipe(1, 2,
					new List<Item>
					{
						new Item(24, 1),
					},
					new Item[]
					{
						new Item(44, 1),
						new Item(44, 1),
					}, "crafting_table"){ UniqueId = 1858 },
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
					}, "crafting_table"){ UniqueId = 1853 },
				new ShapedRecipe(1, 2,
					new List<Item>
					{
						new Item(201, 2),
					},
					new Item[]
					{
						new Item(182, 1),
						new Item(182, 1),
					}, "crafting_table"){ UniqueId = 1860 },
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
					}, "crafting_table"){ UniqueId = 1902 },
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
						new Item(269, 32767),
						new Item(5, 4),
						new Item(5, 4),
					}, "crafting_table"){ UniqueId = 196 },
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
					}, "crafting_table"){ UniqueId = 197 },
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
					}, "crafting_table"){ UniqueId = 198 },
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
					}, "crafting_table"){ UniqueId = 199 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(5, 4),
					},
					new Item[]
					{
						new Item(162, 0),
					}, "crafting_table"){ UniqueId = 200 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(5, 4),
					},
					new Item[]
					{
						new Item(-8, 32767),
					}, "crafting_table"){ UniqueId = 201 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(5, 4),
					},
					new Item[]
					{
						new Item(-212, 12),
					}, "crafting_table"){ UniqueId = 202 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(5, 4),
					},
					new Item[]
					{
						new Item(-212, 4),
					}, "crafting_table"){ UniqueId = 203 },
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
					}, "crafting_table"){ UniqueId = 204 },
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
					}, "crafting_table"){ UniqueId = 205 },
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
					}, "crafting_table"){ UniqueId = 207 },
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
					}, "crafting_table"){ UniqueId = 206 },
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
					}, "crafting_table"){ UniqueId = 208 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(1, 5, 2),
					},
					new List<Item>
					{
						new Item(1, 3, 1),
						new Item(4, 32767, 1),
					}, "crafting_table"){ UniqueId = 209 },
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
					}, "crafting_table"){ UniqueId = 210 },
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
					}, "crafting_table"){ UniqueId = 211 },
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
					}, "crafting_table"){ UniqueId = 212 },
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
					}, "crafting_table"){ UniqueId = 213 },
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
					}, "crafting_table"){ UniqueId = 214 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(434, 4, 1),
					},
					new List<Item>
					{
						new Item(339, 32767, 1),
						new Item(45, 32767, 1),
					}, "crafting_table"){ UniqueId = 215 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(434, 0, 1),
					},
					new List<Item>
					{
						new Item(339, 32767, 1),
						new Item(397, 4, 1),
					}, "crafting_table"){ UniqueId = 216 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(434, 2, 1),
					},
					new List<Item>
					{
						new Item(339, 32767, 1),
						new Item(38, 8, 1),
					}, "crafting_table"){ UniqueId = 217 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(434, 1, 1),
					},
					new List<Item>
					{
						new Item(339, 32767, 1),
						new Item(397, 1, 1),
					}, "crafting_table"){ UniqueId = 218 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(434, 3, 1),
					},
					new List<Item>
					{
						new Item(339, 32767, 1),
						new Item(466, 32767, 1),
					}, "crafting_table"){ UniqueId = 219 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(434, 5, 1),
					},
					new List<Item>
					{
						new Item(339, 32767, 1),
						new Item(106, 32767, 1),
					}, "crafting_table"){ UniqueId = 220 },
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
					}, "crafting_table"){ UniqueId = 221 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-203, 0),
					},
					new Item[]
					{
						new Item(-264, 32767),
						new Item(-264, 32767),
						new Item(-264, 32767),
						new Item(280, 32767),
						new Item(0, 0),
						new Item(280, 32767),
						new Item(-264, 32767),
						new Item(-264, 32767),
						new Item(-264, 32767),
					}, "crafting_table"){ UniqueId = 1 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-203, 0),
					},
					new Item[]
					{
						new Item(-265, 32767),
						new Item(-265, 32767),
						new Item(-265, 32767),
						new Item(280, 32767),
						new Item(0, 0),
						new Item(280, 32767),
						new Item(-265, 32767),
						new Item(-265, 32767),
						new Item(-265, 32767),
					}, "crafting_table"){ UniqueId = 2 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(395, 2, 1),
					},
					new List<Item>
					{
						new Item(395, 1, 1),
						new Item(345, 32767, 1),
					}, "crafting_table"){ UniqueId = 222 },
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
					}, "crafting_table"){ UniqueId = 223 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-219, 3),
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
					}, "crafting_table"){ UniqueId = 191 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-219, 3),
					},
					new Item[]
					{
						new Item(-242, 32767),
						new Item(736, 32767),
						new Item(-242, 32767),
						new Item(-242, 32767),
						new Item(736, 32767),
						new Item(-242, 32767),
						new Item(-242, 32767),
						new Item(736, 32767),
						new Item(-242, 32767),
					}, "crafting_table"){ UniqueId = 3 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-219, 3),
					},
					new Item[]
					{
						new Item(-243, 32767),
						new Item(736, 32767),
						new Item(-243, 32767),
						new Item(-243, 32767),
						new Item(736, 32767),
						new Item(-243, 32767),
						new Item(-243, 32767),
						new Item(736, 32767),
						new Item(-243, 32767),
					}, "crafting_table"){ UniqueId = 4 },
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
					}, "crafting_table"){ UniqueId = 224 },
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
						new Item(269, 32767),
						new Item(5, 2),
						new Item(5, 2),
					}, "crafting_table"){ UniqueId = 225 },
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
					}, "crafting_table"){ UniqueId = 226 },
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
					}, "crafting_table"){ UniqueId = 227 },
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
					}, "crafting_table"){ UniqueId = 228 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(5, 2),
					},
					new Item[]
					{
						new Item(17, 2),
					}, "crafting_table"){ UniqueId = 229 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(5, 2),
					},
					new Item[]
					{
						new Item(-6, 32767),
					}, "crafting_table"){ UniqueId = 230 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(5, 2),
					},
					new Item[]
					{
						new Item(-212, 10),
					}, "crafting_table"){ UniqueId = 231 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(5, 2),
					},
					new Item[]
					{
						new Item(-212, 2),
					}, "crafting_table"){ UniqueId = 232 },
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
					}, "crafting_table"){ UniqueId = 233 },
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
					}, "crafting_table"){ UniqueId = 234 },
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
					}, "crafting_table"){ UniqueId = 236 },
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
					}, "crafting_table"){ UniqueId = 235 },
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
					}, "crafting_table"){ UniqueId = 237 },
				new ShapedRecipe(2, 1,
					new List<Item>
					{
						new Item(171, 15),
					},
					new Item[]
					{
						new Item(35, 15),
						new Item(35, 15),
					}, "crafting_table"){ UniqueId = 238 },
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
					}, "crafting_table"){ UniqueId = 239 },
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
					}, "crafting_table"){ UniqueId = 240 },
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
					}, "crafting_table"){ UniqueId = 241 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 16, 1),
					},
					new List<Item>
					{
						new Item(351, 0, 1),
					}, "crafting_table"){ UniqueId = 242 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 16, 1),
					},
					new List<Item>
					{
						new Item(-216, 32767, 1),
					}, "crafting_table"){ UniqueId = 243 },
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
					}, "crafting_table"){ UniqueId = 244 },
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
					}, "crafting_table"){ UniqueId = 245 },
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
					}, "crafting_table"){ UniqueId = 246 },
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
					}, "crafting_table"){ UniqueId = 247 },
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
					}, "crafting_table"){ UniqueId = 248 },
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
					}, "crafting_table"){ UniqueId = 249 },
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(-282, 0),
					},
					new Item[]
					{
						new Item(-273, 32767),
						new Item(-273, 32767),
						new Item(-273, 32767),
					}, "crafting_table"){ UniqueId = 5 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-276, 0),
					},
					new Item[]
					{
						new Item(-273, 32767),
						new Item(-273, 32767),
						new Item(-273, 32767),
						new Item(0, 0),
						new Item(-273, 32767),
						new Item(-273, 32767),
						new Item(0, 0),
						new Item(0, 0),
						new Item(-273, 32767),
					}, "crafting_table"){ UniqueId = 6 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(-277, 0),
					},
					new Item[]
					{
						new Item(-273, 32767),
						new Item(-273, 32767),
						new Item(-273, 32767),
						new Item(-273, 32767),
						new Item(-273, 32767),
						new Item(-273, 32767),
					}, "crafting_table"){ UniqueId = 7 },
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
					}, "crafting_table"){ UniqueId = 250 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(377, 0, 2),
					},
					new List<Item>
					{
						new Item(369, 32767, 1),
					}, "crafting_table"){ UniqueId = 251 },
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
					}, "crafting_table"){ UniqueId = 252 },
				new ShapedRecipe(2, 1,
					new List<Item>
					{
						new Item(171, 11),
					},
					new Item[]
					{
						new Item(35, 11),
						new Item(35, 11),
					}, "crafting_table"){ UniqueId = 253 },
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
					}, "crafting_table"){ UniqueId = 254 },
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
					}, "crafting_table"){ UniqueId = 255 },
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
					}, "crafting_table"){ UniqueId = 256 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 18, 1),
					},
					new List<Item>
					{
						new Item(38, 9, 1),
					}, "crafting_table"){ UniqueId = 257 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 18, 1),
					},
					new List<Item>
					{
						new Item(351, 4, 1),
					}, "crafting_table"){ UniqueId = 258 },
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
					}, "crafting_table"){ UniqueId = 259 },
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
					}, "crafting_table"){ UniqueId = 260 },
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
					}, "crafting_table"){ UniqueId = 261 },
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
					}, "crafting_table"){ UniqueId = 262 },
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
					}, "crafting_table"){ UniqueId = 263 },
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
					}, "crafting_table"){ UniqueId = 264 },
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
					}, "crafting_table"){ UniqueId = 265 },
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
						new Item(269, 32767),
						new Item(5, 0),
						new Item(5, 0),
					}, "crafting_table"){ UniqueId = 266 },
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
					}, "crafting_table"){ UniqueId = 267 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 15, 9),
					},
					new List<Item>
					{
						new Item(216, 32767, 1),
					}, "crafting_table"){ UniqueId = 268 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 15, 3),
					},
					new List<Item>
					{
						new Item(352, 32767, 1),
					}, "crafting_table"){ UniqueId = 269 },
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
					}, "crafting_table"){ UniqueId = 270 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(47, 0),
					},
					new Item[]
					{
						new Item(-242, 32767),
						new Item(340, 32767),
						new Item(-242, 32767),
						new Item(-242, 32767),
						new Item(340, 32767),
						new Item(-242, 32767),
						new Item(-242, 32767),
						new Item(340, 32767),
						new Item(-242, 32767),
					}, "crafting_table"){ UniqueId = 8 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(47, 0),
					},
					new Item[]
					{
						new Item(-243, 32767),
						new Item(340, 32767),
						new Item(-243, 32767),
						new Item(-243, 32767),
						new Item(340, 32767),
						new Item(-243, 32767),
						new Item(-243, 32767),
						new Item(340, 32767),
						new Item(-243, 32767),
					}, "crafting_table"){ UniqueId = 9 },
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
					}, "crafting_table"){ UniqueId = 271 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(281, 0),
					},
					new Item[]
					{
						new Item(-242, 32767),
						new Item(-242, 32767),
						new Item(-242, 32767),
						new Item(0, 0),
						new Item(0, 0),
						new Item(0, 0),
					}, "crafting_table"){ UniqueId = 10 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(281, 0),
					},
					new Item[]
					{
						new Item(-243, 32767),
						new Item(-243, 32767),
						new Item(-243, 32767),
						new Item(0, 0),
						new Item(0, 0),
						new Item(0, 0),
					}, "crafting_table"){ UniqueId = 11 },
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
					}, "crafting_table"){ UniqueId = 272 },
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
					}, "crafting_table"){ UniqueId = 273 },
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
					}, "crafting_table"){ UniqueId = 274 },
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
					}, "crafting_table"){ UniqueId = 275 },
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
					}, "crafting_table"){ UniqueId = 276 },
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
					}, "crafting_table"){ UniqueId = 277 },
				new ShapedRecipe(2, 1,
					new List<Item>
					{
						new Item(171, 12),
					},
					new Item[]
					{
						new Item(35, 12),
						new Item(35, 12),
					}, "crafting_table"){ UniqueId = 278 },
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
					}, "crafting_table"){ UniqueId = 279 },
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
					}, "crafting_table"){ UniqueId = 280 },
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
					}, "crafting_table"){ UniqueId = 281 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 17, 1),
					},
					new List<Item>
					{
						new Item(351, 3, 1),
					}, "crafting_table"){ UniqueId = 282 },
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
					}, "crafting_table"){ UniqueId = 283 },
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
					}, "crafting_table"){ UniqueId = 284 },
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
					}, "crafting_table"){ UniqueId = 285 },
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
					}, "crafting_table"){ UniqueId = 286 },
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
					}, "crafting_table"){ UniqueId = 287 },
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
					}, "crafting_table"){ UniqueId = 288 },
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
					}, "crafting_table"){ UniqueId = 289 },
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
					}, "crafting_table"){ UniqueId = 290 },
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
					}, "crafting_table"){ UniqueId = 12 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(720, 0),
					},
					new Item[]
					{
						new Item(0, 0),
						new Item(280, 32767),
						new Item(-225, 32767),
						new Item(280, 32767),
						new Item(263, 32767),
						new Item(-225, 32767),
						new Item(0, 0),
						new Item(280, 32767),
						new Item(-225, 32767),
					}, "crafting_table"){ UniqueId = 13 },
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
					}, "crafting_table"){ UniqueId = 14 },
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
					}, "crafting_table"){ UniqueId = 15 },
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
					}, "crafting_table"){ UniqueId = 16 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(720, 0),
					},
					new Item[]
					{
						new Item(0, 0),
						new Item(280, 32767),
						new Item(-240, 32767),
						new Item(280, 32767),
						new Item(263, 32767),
						new Item(-240, 32767),
						new Item(0, 0),
						new Item(280, 32767),
						new Item(-240, 32767),
					}, "crafting_table"){ UniqueId = 17 },
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
					}, "crafting_table"){ UniqueId = 18 },
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
					}, "crafting_table"){ UniqueId = 19 },
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
					}, "crafting_table"){ UniqueId = 20 },
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
					}, "crafting_table"){ UniqueId = 21 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(720, 0),
					},
					new Item[]
					{
						new Item(0, 0),
						new Item(280, 32767),
						new Item(-241, 32767),
						new Item(280, 32767),
						new Item(263, 32767),
						new Item(-241, 32767),
						new Item(0, 0),
						new Item(280, 32767),
						new Item(-241, 32767),
					}, "crafting_table"){ UniqueId = 22 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(720, 0),
					},
					new Item[]
					{
						new Item(0, 0),
						new Item(280, 32767),
						new Item(-226, 32767),
						new Item(280, 32767),
						new Item(263, 32767),
						new Item(-226, 32767),
						new Item(0, 0),
						new Item(280, 32767),
						new Item(-226, 32767),
					}, "crafting_table"){ UniqueId = 23 },
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
					}, "crafting_table"){ UniqueId = 24 },
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(398, 0),
					},
					new Item[]
					{
						new Item(346, 32767),
						new Item(0, 0),
						new Item(0, 0),
						new Item(391, 32767),
					}, "crafting_table"){ UniqueId = 291 },
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
					}, "crafting_table"){ UniqueId = 292 },
				new ShapedRecipe(2, 3,
					new List<Item>
					{
						new Item(-200, 0),
					},
					new Item[]
					{
						new Item(339, 32767),
						new Item(-242, 32767),
						new Item(339, 32767),
						new Item(-242, 32767),
						new Item(-242, 32767),
						new Item(-242, 32767),
					}, "crafting_table"){ UniqueId = 25 },
				new ShapedRecipe(2, 3,
					new List<Item>
					{
						new Item(-200, 0),
					},
					new Item[]
					{
						new Item(339, 32767),
						new Item(-243, 32767),
						new Item(339, 32767),
						new Item(-243, 32767),
						new Item(-243, 32767),
						new Item(-243, 32767),
					}, "crafting_table"){ UniqueId = 26 },
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
					}, "crafting_table"){ UniqueId = 295 },
				new ShapedRecipe(1, 3,
					new List<Item>
					{
						new Item(758, 0),
					},
					new Item[]
					{
						new Item(452, 32767),
						new Item(265, 32767),
						new Item(452, 32767),
					}, "crafting_table"){ UniqueId = 27 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(54, 0),
					},
					new Item[]
					{
						new Item(-242, 32767),
						new Item(-242, 32767),
						new Item(-242, 32767),
						new Item(-242, 32767),
						new Item(0, 0),
						new Item(-242, 32767),
						new Item(-242, 32767),
						new Item(-242, 32767),
						new Item(-242, 32767),
					}, "crafting_table"){ UniqueId = 28 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(54, 0),
					},
					new Item[]
					{
						new Item(-243, 32767),
						new Item(-243, 32767),
						new Item(-243, 32767),
						new Item(-243, 32767),
						new Item(0, 0),
						new Item(-243, 32767),
						new Item(-243, 32767),
						new Item(-243, 32767),
						new Item(-243, 32767),
					}, "crafting_table"){ UniqueId = 29 },
				new ShapedRecipe(1, 2,
					new List<Item>
					{
						new Item(342, 0),
					},
					new Item[]
					{
						new Item(54, 32767),
						new Item(328, 32767),
					}, "crafting_table"){ UniqueId = 296 },
				new ShapedRecipe(1, 2,
					new List<Item>
					{
						new Item(-302, 0),
					},
					new Item[]
					{
						new Item(44, 7),
						new Item(44, 7),
					}, "crafting_table"){ UniqueId = 30 },
				new ShapedRecipe(1, 2,
					new List<Item>
					{
						new Item(-279, 0),
					},
					new Item[]
					{
						new Item(-293, 32767),
						new Item(-293, 32767),
					}, "crafting_table"){ UniqueId = 31 },
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
					}, "crafting_table"){ UniqueId = 297 },
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
					}, "crafting_table"){ UniqueId = 298 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(263, 0),
					},
					new Item[]
					{
						new Item(173, 32767),
					}, "crafting_table"){ UniqueId = 299 },
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
					}, "crafting_table"){ UniqueId = 300 },
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
					}, "crafting_table"){ UniqueId = 301 },
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
					}, "crafting_table"){ UniqueId = 302 },
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
					}, "crafting_table"){ UniqueId = 303 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(287, 0, 9),
					},
					new List<Item>
					{
						new Item(30, 32767, 1),
					}, "crafting_table"){ UniqueId = 304 },
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
					}, "crafting_table"){ UniqueId = 305 },
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
					}, "crafting_table"){ UniqueId = 306 },
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
					}, "crafting_table"){ UniqueId = 307 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-213, 0),
					},
					new Item[]
					{
						new Item(-264, 32767),
						new Item(-264, 32767),
						new Item(-264, 32767),
						new Item(0, 0),
						new Item(0, 0),
						new Item(-264, 32767),
						new Item(-264, 32767),
						new Item(-264, 32767),
						new Item(-264, 32767),
					}, "crafting_table"){ UniqueId = 32 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-213, 0),
					},
					new Item[]
					{
						new Item(-265, 32767),
						new Item(-265, 32767),
						new Item(-265, 32767),
						new Item(0, 0),
						new Item(0, 0),
						new Item(-265, 32767),
						new Item(-265, 32767),
						new Item(-265, 32767),
						new Item(-265, 32767),
					}, "crafting_table"){ UniqueId = 33 },
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
					}, "crafting_table"){ UniqueId = 308 },
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
					}, "crafting_table"){ UniqueId = 309 },
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(58, 0),
					},
					new Item[]
					{
						new Item(-242, 32767),
						new Item(-242, 32767),
						new Item(-242, 32767),
						new Item(-242, 32767),
					}, "crafting_table"){ UniqueId = 34 },
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(58, 0),
					},
					new Item[]
					{
						new Item(-243, 32767),
						new Item(-243, 32767),
						new Item(-243, 32767),
						new Item(-243, 32767),
					}, "crafting_table"){ UniqueId = 35 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(-260, 0),
					},
					new Item[]
					{
						new Item(-242, 32767),
					}, "crafting_table"){ UniqueId = 36 },
				new ShapedRecipe(2, 3,
					new List<Item>
					{
						new Item(755, 0),
					},
					new Item[]
					{
						new Item(-242, 32767),
						new Item(-242, 32767),
						new Item(-242, 32767),
						new Item(-242, 32767),
						new Item(-242, 32767),
						new Item(-242, 32767),
					}, "crafting_table"){ UniqueId = 37 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(-256, 0),
					},
					new Item[]
					{
						new Item(-242, 32767),
						new Item(-242, 32767),
						new Item(280, 32767),
						new Item(280, 32767),
						new Item(-242, 32767),
						new Item(-242, 32767),
					}, "crafting_table"){ UniqueId = 38 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(-258, 0),
					},
					new Item[]
					{
						new Item(280, 32767),
						new Item(280, 32767),
						new Item(-242, 32767),
						new Item(-242, 32767),
						new Item(280, 32767),
						new Item(280, 32767),
					}, "crafting_table"){ UniqueId = 39 },
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(-299, 0),
					},
					new Item[]
					{
						new Item(-225, 32767),
						new Item(-225, 32767),
						new Item(-225, 32767),
						new Item(-225, 32767),
					}, "crafting_table"){ UniqueId = 40 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(-242, 0),
					},
					new Item[]
					{
						new Item(-225, 32767),
					}, "crafting_table"){ UniqueId = 42 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(-242, 0),
					},
					new Item[]
					{
						new Item(-299, 32767),
					}, "crafting_table"){ UniqueId = 43 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(-242, 0),
					},
					new Item[]
					{
						new Item(-300, 32767),
					}, "crafting_table"){ UniqueId = 44 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(-242, 0),
					},
					new Item[]
					{
						new Item(-240, 32767),
					}, "crafting_table"){ UniqueId = 45 },
				new ShapedRecipe(2, 1,
					new List<Item>
					{
						new Item(-262, 0),
					},
					new Item[]
					{
						new Item(-242, 32767),
						new Item(-242, 32767),
					}, "crafting_table"){ UniqueId = 46 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(753, 0),
					},
					new Item[]
					{
						new Item(-242, 32767),
						new Item(-242, 32767),
						new Item(0, 0),
						new Item(-242, 32767),
						new Item(-242, 32767),
						new Item(280, 32767),
						new Item(-242, 32767),
						new Item(-242, 32767),
						new Item(0, 0),
					}, "crafting_table"){ UniqueId = 47 },
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(-264, 0),
					},
					new Item[]
					{
						new Item(-242, 32767),
						new Item(-242, 32767),
						new Item(-242, 32767),
					}, "crafting_table"){ UniqueId = 48 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-254, 0),
					},
					new Item[]
					{
						new Item(-242, 32767),
						new Item(-242, 32767),
						new Item(-242, 32767),
						new Item(0, 0),
						new Item(-242, 32767),
						new Item(-242, 32767),
						new Item(0, 0),
						new Item(0, 0),
						new Item(-242, 32767),
					}, "crafting_table"){ UniqueId = 49 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(-246, 0),
					},
					new Item[]
					{
						new Item(-242, 32767),
						new Item(-242, 32767),
						new Item(-242, 32767),
						new Item(-242, 32767),
						new Item(-242, 32767),
						new Item(-242, 32767),
					}, "crafting_table"){ UniqueId = 50 },
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
					}, "crafting_table"){ UniqueId = 310 },
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
					}, "crafting_table"){ UniqueId = 311 },
				new ShapedRecipe(2, 1,
					new List<Item>
					{
						new Item(171, 9),
					},
					new Item[]
					{
						new Item(35, 9),
						new Item(35, 9),
					}, "crafting_table"){ UniqueId = 312 },
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
					}, "crafting_table"){ UniqueId = 313 },
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
					}, "crafting_table"){ UniqueId = 314 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 6, 2),
					},
					new List<Item>
					{
						new Item(351, 18, 1),
						new Item(351, 2, 1),
					}, "crafting_table"){ UniqueId = 315 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 6, 2),
					},
					new List<Item>
					{
						new Item(351, 4, 1),
						new Item(351, 2, 1),
					}, "crafting_table"){ UniqueId = 316 },
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
					}, "crafting_table"){ UniqueId = 317 },
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
					}, "crafting_table"){ UniqueId = 318 },
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
					}, "crafting_table"){ UniqueId = 319 },
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
					}, "crafting_table"){ UniqueId = 320 },
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
						new Item(269, 32767),
						new Item(5, 5),
						new Item(5, 5),
					}, "crafting_table"){ UniqueId = 321 },
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
					}, "crafting_table"){ UniqueId = 322 },
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
					}, "crafting_table"){ UniqueId = 323 },
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
					}, "crafting_table"){ UniqueId = 324 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(5, 5),
					},
					new Item[]
					{
						new Item(162, 1),
					}, "crafting_table"){ UniqueId = 325 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(5, 5),
					},
					new Item[]
					{
						new Item(-9, 32767),
					}, "crafting_table"){ UniqueId = 326 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(5, 5),
					},
					new Item[]
					{
						new Item(-212, 13),
					}, "crafting_table"){ UniqueId = 327 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(5, 5),
					},
					new Item[]
					{
						new Item(-212, 5),
					}, "crafting_table"){ UniqueId = 328 },
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
					}, "crafting_table"){ UniqueId = 329 },
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
					}, "crafting_table"){ UniqueId = 330 },
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
					}, "crafting_table"){ UniqueId = 332 },
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
					}, "crafting_table"){ UniqueId = 331 },
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
					}, "crafting_table"){ UniqueId = 333 },
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
					}, "crafting_table"){ UniqueId = 334 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(151, 0),
					},
					new Item[]
					{
						new Item(20, 32767),
						new Item(406, 32767),
						new Item(-264, 32767),
						new Item(20, 32767),
						new Item(406, 32767),
						new Item(-264, 32767),
						new Item(20, 32767),
						new Item(406, 32767),
						new Item(-264, 32767),
					}, "crafting_table"){ UniqueId = 51 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(151, 0),
					},
					new Item[]
					{
						new Item(20, 32767),
						new Item(406, 32767),
						new Item(-265, 32767),
						new Item(20, 32767),
						new Item(406, 32767),
						new Item(-265, 32767),
						new Item(20, 32767),
						new Item(406, 32767),
						new Item(-265, 32767),
					}, "crafting_table"){ UniqueId = 52 },
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
					}, "crafting_table"){ UniqueId = 335 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(264, 0),
					},
					new Item[]
					{
						new Item(57, 32767),
					}, "crafting_table"){ UniqueId = 336 },
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
					}, "crafting_table"){ UniqueId = 337 },
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
					}, "crafting_table"){ UniqueId = 338 },
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
					}, "crafting_table"){ UniqueId = 339 },
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
					}, "crafting_table"){ UniqueId = 340 },
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
					}, "crafting_table"){ UniqueId = 341 },
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
					}, "crafting_table"){ UniqueId = 342 },
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
					}, "crafting_table"){ UniqueId = 343 },
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
					}, "crafting_table"){ UniqueId = 344 },
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
					}, "crafting_table"){ UniqueId = 345 },
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
					}, "crafting_table"){ UniqueId = 346 },
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
					}, "crafting_table"){ UniqueId = 347 },
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
					}, "crafting_table"){ UniqueId = 348 },
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
					}, "crafting_table"){ UniqueId = 349 },
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
						new Item(261, 32767),
						new Item(331, 32767),
						new Item(4, 32767),
						new Item(4, 32767),
						new Item(4, 32767),
					}, "crafting_table"){ UniqueId = 350 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(464, 0),
					},
					new Item[]
					{
						new Item(-139, 32767),
					}, "crafting_table"){ UniqueId = 351 },
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
					}, "crafting_table"){ UniqueId = 352 },
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
					}, "crafting_table"){ UniqueId = 353 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(388, 0),
					},
					new Item[]
					{
						new Item(133, 32767),
					}, "crafting_table"){ UniqueId = 354 },
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
					}, "crafting_table"){ UniqueId = 355 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(395, 2, 1),
					},
					new List<Item>
					{
						new Item(395, 0, 1),
						new Item(345, 32767, 1),
					}, "crafting_table"){ UniqueId = 356 },
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
					}, "crafting_table"){ UniqueId = 357 },
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
					}, "crafting_table"){ UniqueId = 361 },
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
					}, "crafting_table"){ UniqueId = 362 },
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
					}, "crafting_table"){ UniqueId = 360 },
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
					}, "crafting_table"){ UniqueId = 363 },
				new ShapedRecipe(1, 2,
					new List<Item>
					{
						new Item(208, 0),
					},
					new Item[]
					{
						new Item(369, 32767),
						new Item(433, 32767),
					}, "crafting_table"){ UniqueId = 364 },
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
					}, "crafting_table"){ UniqueId = 358 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(381, 0, 1),
					},
					new List<Item>
					{
						new Item(368, 32767, 1),
						new Item(377, 32767, 1),
					}, "crafting_table"){ UniqueId = 359 },
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
					}, "crafting_table"){ UniqueId = 365 },
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
					}, "crafting_table"){ UniqueId = 366 },
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
					}, "crafting_table"){ UniqueId = 367 },
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
					}, "crafting_table"){ UniqueId = 368 },
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
					}, "crafting_table"){ UniqueId = 369 },
				new ShapedRecipe(2, 3,
					new List<Item>
					{
						new Item(-201, 0),
					},
					new Item[]
					{
						new Item(318, 32767),
						new Item(-242, 32767),
						new Item(318, 32767),
						new Item(-242, 32767),
						new Item(-242, 32767),
						new Item(-242, 32767),
					}, "crafting_table"){ UniqueId = 53 },
				new ShapedRecipe(2, 3,
					new List<Item>
					{
						new Item(-201, 0),
					},
					new Item[]
					{
						new Item(318, 32767),
						new Item(-243, 32767),
						new Item(318, 32767),
						new Item(-243, 32767),
						new Item(-243, 32767),
						new Item(-243, 32767),
					}, "crafting_table"){ UniqueId = 54 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(259, 0, 1),
					},
					new List<Item>
					{
						new Item(265, 32767, 1),
						new Item(318, 32767, 1),
					}, "crafting_table"){ UniqueId = 370 },
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
					}, "crafting_table"){ UniqueId = 371 },
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
					}, "crafting_table"){ UniqueId = 372 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(61, 0),
					},
					new Item[]
					{
						new Item(-273, 32767),
						new Item(-273, 32767),
						new Item(-273, 32767),
						new Item(-273, 32767),
						new Item(0, 0),
						new Item(-273, 32767),
						new Item(-273, 32767),
						new Item(-273, 32767),
						new Item(-273, 32767),
					}, "crafting_table"){ UniqueId = 55 },
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
					}, "crafting_table"){ UniqueId = 373 },
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
					}, "crafting_table"){ UniqueId = 374 },
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
					}, "crafting_table"){ UniqueId = 375 },
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
					}, "crafting_table"){ UniqueId = 388 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(266, 0),
					},
					new Item[]
					{
						new Item(41, 32767),
					}, "crafting_table"){ UniqueId = 389 },
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
					}, "crafting_table"){ UniqueId = 390 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(371, 0),
					},
					new Item[]
					{
						new Item(266, 32767),
					}, "crafting_table"){ UniqueId = 391 },
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
					}, "crafting_table"){ UniqueId = 376 },
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
					}, "crafting_table"){ UniqueId = 377 },
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
					}, "crafting_table"){ UniqueId = 378 },
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
					}, "crafting_table"){ UniqueId = 379 },
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
					}, "crafting_table"){ UniqueId = 380 },
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
					}, "crafting_table"){ UniqueId = 381 },
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
					}, "crafting_table"){ UniqueId = 382 },
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
					}, "crafting_table"){ UniqueId = 383 },
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
					}, "crafting_table"){ UniqueId = 384 },
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
					}, "crafting_table"){ UniqueId = 385 },
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
					}, "crafting_table"){ UniqueId = 386 },
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
					}, "crafting_table"){ UniqueId = 387 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(1, 1, 1),
					},
					new List<Item>
					{
						new Item(1, 3, 1),
						new Item(406, 32767, 1),
					}, "crafting_table"){ UniqueId = 392 },
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
					}, "crafting_table"){ UniqueId = 393 },
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
					}, "crafting_table"){ UniqueId = 394 },
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
					}, "crafting_table"){ UniqueId = 395 },
				new ShapedRecipe(2, 1,
					new List<Item>
					{
						new Item(171, 7),
					},
					new Item[]
					{
						new Item(35, 7),
						new Item(35, 7),
					}, "crafting_table"){ UniqueId = 396 },
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
					}, "crafting_table"){ UniqueId = 397 },
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
					}, "crafting_table"){ UniqueId = 398 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 8, 2),
					},
					new List<Item>
					{
						new Item(351, 16, 1),
						new Item(351, 19, 1),
					}, "crafting_table"){ UniqueId = 399 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 8, 2),
					},
					new List<Item>
					{
						new Item(351, 16, 1),
						new Item(351, 15, 1),
					}, "crafting_table"){ UniqueId = 400 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 8, 2),
					},
					new List<Item>
					{
						new Item(351, 0, 1),
						new Item(351, 15, 1),
					}, "crafting_table"){ UniqueId = 401 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 8, 2),
					},
					new List<Item>
					{
						new Item(351, 0, 1),
						new Item(351, 19, 1),
					}, "crafting_table"){ UniqueId = 402 },
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
					}, "crafting_table"){ UniqueId = 403 },
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
					}, "crafting_table"){ UniqueId = 404 },
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
					}, "crafting_table"){ UniqueId = 405 },
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
					}, "crafting_table"){ UniqueId = 406 },
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
					}, "crafting_table"){ UniqueId = 407 },
				new ShapedRecipe(2, 1,
					new List<Item>
					{
						new Item(171, 13),
					},
					new Item[]
					{
						new Item(35, 13),
						new Item(35, 13),
					}, "crafting_table"){ UniqueId = 408 },
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
					}, "crafting_table"){ UniqueId = 409 },
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
					}, "crafting_table"){ UniqueId = 410 },
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
					}, "crafting_table"){ UniqueId = 411 },
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
					}, "crafting_table"){ UniqueId = 412 },
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
					}, "crafting_table"){ UniqueId = 413 },
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
					}, "crafting_table"){ UniqueId = 414 },
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
					}, "crafting_table"){ UniqueId = 415 },
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
						new Item(44, 32767),
						new Item(-242, 32767),
						new Item(-242, 32767),
					}, "crafting_table"){ UniqueId = 56 },
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
						new Item(182, 32767),
						new Item(-242, 32767),
						new Item(-242, 32767),
					}, "crafting_table"){ UniqueId = 57 },
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
						new Item(-162, 32767),
						new Item(-242, 32767),
						new Item(-242, 32767),
					}, "crafting_table"){ UniqueId = 58 },
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
						new Item(-166, 32767),
						new Item(-242, 32767),
						new Item(-242, 32767),
					}, "crafting_table"){ UniqueId = 59 },
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
						new Item(44, 32767),
						new Item(-243, 32767),
						new Item(-243, 32767),
					}, "crafting_table"){ UniqueId = 60 },
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
						new Item(182, 32767),
						new Item(-243, 32767),
						new Item(-243, 32767),
					}, "crafting_table"){ UniqueId = 61 },
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
						new Item(-162, 32767),
						new Item(-243, 32767),
						new Item(-243, 32767),
					}, "crafting_table"){ UniqueId = 62 },
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
						new Item(-166, 32767),
						new Item(-243, 32767),
						new Item(-243, 32767),
					}, "crafting_table"){ UniqueId = 63 },
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
					}, "crafting_table"){ UniqueId = 416 },
				new ShapedRecipe(2, 1,
					new List<Item>
					{
						new Item(148, 0),
					},
					new Item[]
					{
						new Item(265, 32767),
						new Item(265, 32767),
					}, "crafting_table"){ UniqueId = 417 },
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
					}, "crafting_table"){ UniqueId = 193 },
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
					}, "crafting_table"){ UniqueId = 194 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(353, 0),
						new Item(374, 0),
					},
					new Item[]
					{
						new Item(737, 32767),
					}, "crafting_table"){ UniqueId = 195 },
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
					}, "crafting_table"){ UniqueId = 192 },
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
					}, "crafting_table"){ UniqueId = 418 },
				new ShapedRecipe(1, 2,
					new List<Item>
					{
						new Item(408, 0),
					},
					new Item[]
					{
						new Item(410, 32767),
						new Item(328, 32767),
					}, "crafting_table"){ UniqueId = 419 },
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
					}, "crafting_table"){ UniqueId = 420 },
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
					}, "crafting_table"){ UniqueId = 421 },
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
					}, "crafting_table"){ UniqueId = 422 },
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
					}, "crafting_table"){ UniqueId = 423 },
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
					}, "crafting_table"){ UniqueId = 424 },
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
					}, "crafting_table"){ UniqueId = 425 },
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
					}, "crafting_table"){ UniqueId = 426 },
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
					}, "crafting_table"){ UniqueId = 427 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(265, 0),
					},
					new Item[]
					{
						new Item(42, 32767),
					}, "crafting_table"){ UniqueId = 428 },
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
					}, "crafting_table"){ UniqueId = 429 },
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
					}, "crafting_table"){ UniqueId = 430 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(452, 0),
					},
					new Item[]
					{
						new Item(265, 32767),
					}, "crafting_table"){ UniqueId = 431 },
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
					}, "crafting_table"){ UniqueId = 432 },
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
					}, "crafting_table"){ UniqueId = 433 },
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
					}, "crafting_table"){ UniqueId = 434 },
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
					}, "crafting_table"){ UniqueId = 435 },
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
					}, "crafting_table"){ UniqueId = 436 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(84, 0),
					},
					new Item[]
					{
						new Item(-242, 32767),
						new Item(-242, 32767),
						new Item(-242, 32767),
						new Item(-242, 32767),
						new Item(264, 32767),
						new Item(-242, 32767),
						new Item(-242, 32767),
						new Item(-242, 32767),
						new Item(-242, 32767),
					}, "crafting_table"){ UniqueId = 64 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(84, 0),
					},
					new Item[]
					{
						new Item(-243, 32767),
						new Item(-243, 32767),
						new Item(-243, 32767),
						new Item(-243, 32767),
						new Item(264, 32767),
						new Item(-243, 32767),
						new Item(-243, 32767),
						new Item(-243, 32767),
						new Item(-243, 32767),
					}, "crafting_table"){ UniqueId = 65 },
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
						new Item(269, 32767),
						new Item(5, 3),
						new Item(5, 3),
					}, "crafting_table"){ UniqueId = 437 },
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
					}, "crafting_table"){ UniqueId = 438 },
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
					}, "crafting_table"){ UniqueId = 439 },
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
					}, "crafting_table"){ UniqueId = 440 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(5, 3),
					},
					new Item[]
					{
						new Item(17, 3),
					}, "crafting_table"){ UniqueId = 441 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(5, 3),
					},
					new Item[]
					{
						new Item(-7, 32767),
					}, "crafting_table"){ UniqueId = 442 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(5, 3),
					},
					new Item[]
					{
						new Item(-212, 11),
					}, "crafting_table"){ UniqueId = 443 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(5, 3),
					},
					new Item[]
					{
						new Item(-212, 3),
					}, "crafting_table"){ UniqueId = 444 },
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
					}, "crafting_table"){ UniqueId = 445 },
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
					}, "crafting_table"){ UniqueId = 446 },
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
					}, "crafting_table"){ UniqueId = 448 },
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
					}, "crafting_table"){ UniqueId = 447 },
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
					}, "crafting_table"){ UniqueId = 449 },
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
					}, "crafting_table"){ UniqueId = 450 },
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
					}, "crafting_table"){ UniqueId = 451 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(351, 4),
					},
					new Item[]
					{
						new Item(22, 32767),
					}, "crafting_table"){ UniqueId = 452 },
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
					}, "crafting_table"){ UniqueId = 453 },
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
					}, "crafting_table"){ UniqueId = 454 },
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
					}, "crafting_table"){ UniqueId = 455 },
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
					}, "crafting_table"){ UniqueId = 456 },
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
					}, "crafting_table"){ UniqueId = 457 },
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
					}, "crafting_table"){ UniqueId = 458 },
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
					}, "crafting_table"){ UniqueId = 459 },
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
					}, "crafting_table"){ UniqueId = 460 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-194, 0),
					},
					new Item[]
					{
						new Item(-264, 32767),
						new Item(0, 0),
						new Item(0, 0),
						new Item(-264, 32767),
						new Item(47, 32767),
						new Item(-264, 32767),
						new Item(-264, 32767),
						new Item(0, 0),
						new Item(0, 0),
					}, "crafting_table"){ UniqueId = 66 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-194, 0),
					},
					new Item[]
					{
						new Item(-265, 32767),
						new Item(0, 0),
						new Item(0, 0),
						new Item(-265, 32767),
						new Item(47, 32767),
						new Item(-265, 32767),
						new Item(-265, 32767),
						new Item(0, 0),
						new Item(0, 0),
					}, "crafting_table"){ UniqueId = 67 },
				new ShapedRecipe(1, 2,
					new List<Item>
					{
						new Item(69, 0),
					},
					new Item[]
					{
						new Item(280, 32767),
						new Item(4, 32767),
					}, "crafting_table"){ UniqueId = 461 },
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
					}, "crafting_table"){ UniqueId = 462 },
				new ShapedRecipe(2, 1,
					new List<Item>
					{
						new Item(171, 3),
					},
					new Item[]
					{
						new Item(35, 3),
						new Item(35, 3),
					}, "crafting_table"){ UniqueId = 463 },
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
					}, "crafting_table"){ UniqueId = 464 },
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
					}, "crafting_table"){ UniqueId = 465 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 12, 2),
					},
					new List<Item>
					{
						new Item(351, 18, 1),
						new Item(351, 19, 1),
					}, "crafting_table"){ UniqueId = 466 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 12, 2),
					},
					new List<Item>
					{
						new Item(351, 18, 1),
						new Item(351, 15, 1),
					}, "crafting_table"){ UniqueId = 467 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 12, 1),
					},
					new List<Item>
					{
						new Item(38, 1, 1),
					}, "crafting_table"){ UniqueId = 468 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 12, 2),
					},
					new List<Item>
					{
						new Item(351, 4, 1),
						new Item(351, 15, 1),
					}, "crafting_table"){ UniqueId = 469 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 12, 2),
					},
					new List<Item>
					{
						new Item(351, 4, 1),
						new Item(351, 19, 1),
					}, "crafting_table"){ UniqueId = 470 },
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
					}, "crafting_table"){ UniqueId = 471 },
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
					}, "crafting_table"){ UniqueId = 472 },
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
					}, "crafting_table"){ UniqueId = 473 },
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
					}, "crafting_table"){ UniqueId = 474 },
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
					}, "crafting_table"){ UniqueId = 491 },
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
					}, "crafting_table"){ UniqueId = 475 },
				new ShapedRecipe(2, 1,
					new List<Item>
					{
						new Item(171, 8),
					},
					new Item[]
					{
						new Item(35, 8),
						new Item(35, 8),
					}, "crafting_table"){ UniqueId = 476 },
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
					}, "crafting_table"){ UniqueId = 477 },
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
					}, "crafting_table"){ UniqueId = 478 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 7, 1),
					},
					new List<Item>
					{
						new Item(38, 3, 1),
					}, "crafting_table"){ UniqueId = 479 },
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
					}, "crafting_table"){ UniqueId = 480 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 7, 2),
					},
					new List<Item>
					{
						new Item(351, 8, 1),
						new Item(351, 15, 1),
					}, "crafting_table"){ UniqueId = 481 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 7, 2),
					},
					new List<Item>
					{
						new Item(351, 8, 1),
						new Item(351, 19, 1),
					}, "crafting_table"){ UniqueId = 482 },
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
					}, "crafting_table"){ UniqueId = 483 },
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
					}, "crafting_table"){ UniqueId = 484 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 7, 1),
					},
					new List<Item>
					{
						new Item(38, 8, 1),
					}, "crafting_table"){ UniqueId = 485 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 7, 1),
					},
					new List<Item>
					{
						new Item(38, 6, 1),
					}, "crafting_table"){ UniqueId = 486 },
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
					}, "crafting_table"){ UniqueId = 487 },
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
					}, "crafting_table"){ UniqueId = 488 },
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
					}, "crafting_table"){ UniqueId = 489 },
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
					}, "crafting_table"){ UniqueId = 490 },
				new ShapedRecipe(2, 1,
					new List<Item>
					{
						new Item(147, 0),
					},
					new Item[]
					{
						new Item(266, 32767),
						new Item(266, 32767),
					}, "crafting_table"){ UniqueId = 492 },
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
					}, "crafting_table"){ UniqueId = 502 },
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
					}, "crafting_table"){ UniqueId = 493 },
				new ShapedRecipe(2, 1,
					new List<Item>
					{
						new Item(171, 5),
					},
					new Item[]
					{
						new Item(35, 5),
						new Item(35, 5),
					}, "crafting_table"){ UniqueId = 494 },
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
					}, "crafting_table"){ UniqueId = 495 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 10, 2),
					},
					new List<Item>
					{
						new Item(351, 2, 1),
						new Item(351, 19, 1),
					}, "crafting_table"){ UniqueId = 496 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 10, 2),
					},
					new List<Item>
					{
						new Item(351, 2, 1),
						new Item(351, 15, 1),
					}, "crafting_table"){ UniqueId = 497 },
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
					}, "crafting_table"){ UniqueId = 498 },
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
					}, "crafting_table"){ UniqueId = 499 },
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
					}, "crafting_table"){ UniqueId = 500 },
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
					}, "crafting_table"){ UniqueId = 501 },
				new ShapedRecipe(1, 2,
					new List<Item>
					{
						new Item(91, 0),
					},
					new Item[]
					{
						new Item(-155, 32767),
						new Item(50, 32767),
					}, "crafting_table"){ UniqueId = 503 },
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
					}, "crafting_table"){ UniqueId = 504 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-222, 0),
					},
					new Item[]
					{
						new Item(98, 3),
						new Item(98, 3),
						new Item(98, 3),
						new Item(98, 3),
						new Item(742, 32767),
						new Item(98, 3),
						new Item(98, 3),
						new Item(98, 3),
						new Item(98, 3),
					}, "crafting_table"){ UniqueId = 68 },
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(-204, 0),
					},
					new Item[]
					{
						new Item(287, 32767),
						new Item(-242, 32767),
						new Item(287, 32767),
						new Item(-242, 32767),
					}, "crafting_table"){ UniqueId = 69 },
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(-204, 0),
					},
					new Item[]
					{
						new Item(287, 32767),
						new Item(-243, 32767),
						new Item(287, 32767),
						new Item(-243, 32767),
					}, "crafting_table"){ UniqueId = 70 },
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
					}, "crafting_table"){ UniqueId = 505 },
				new ShapedRecipe(2, 1,
					new List<Item>
					{
						new Item(171, 2),
					},
					new Item[]
					{
						new Item(35, 2),
						new Item(35, 2),
					}, "crafting_table"){ UniqueId = 506 },
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
					}, "crafting_table"){ UniqueId = 507 },
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
					}, "crafting_table"){ UniqueId = 508 },
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
					}, "crafting_table"){ UniqueId = 509 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 13, 1),
					},
					new List<Item>
					{
						new Item(38, 2, 1),
					}, "crafting_table"){ UniqueId = 510 },
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
					}, "crafting_table"){ UniqueId = 511 },
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
					}, "crafting_table"){ UniqueId = 512 },
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
					}, "crafting_table"){ UniqueId = 513 },
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
					}, "crafting_table"){ UniqueId = 514 },
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
					}, "crafting_table"){ UniqueId = 515 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 13, 2),
					},
					new List<Item>
					{
						new Item(175, 1, 1),
					}, "crafting_table"){ UniqueId = 516 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 13, 2),
					},
					new List<Item>
					{
						new Item(351, 5, 1),
						new Item(351, 9, 1),
					}, "crafting_table"){ UniqueId = 517 },
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
					}, "crafting_table"){ UniqueId = 518 },
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
					}, "crafting_table"){ UniqueId = 519 },
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
					}, "crafting_table"){ UniqueId = 520 },
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
					}, "crafting_table"){ UniqueId = 521 },
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
					}, "crafting_table"){ UniqueId = 522 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(378, 0, 1),
					},
					new List<Item>
					{
						new Item(377, 32767, 1),
						new Item(341, 32767, 1),
					}, "crafting_table"){ UniqueId = 523 },
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
					}, "crafting_table"){ UniqueId = 524 },
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
					}, "crafting_table"){ UniqueId = 525 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(362, 0),
					},
					new Item[]
					{
						new Item(360, 32767),
					}, "crafting_table"){ UniqueId = 526 },
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
					}, "crafting_table"){ UniqueId = 527 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(48, 0, 1),
					},
					new List<Item>
					{
						new Item(4, 32767, 1),
						new Item(106, 32767, 1),
					}, "crafting_table"){ UniqueId = 528 },
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
					}, "crafting_table"){ UniqueId = 529 },
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
					}, "crafting_table"){ UniqueId = 530 },
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
					}, "crafting_table"){ UniqueId = 532 },
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
					}, "crafting_table"){ UniqueId = 533 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(98, 1, 1),
					},
					new List<Item>
					{
						new Item(98, 0, 1),
						new Item(106, 32767, 1),
					}, "crafting_table"){ UniqueId = 531 },
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
					}, "crafting_table"){ UniqueId = 534 },
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
					}, "crafting_table"){ UniqueId = 535 },
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
					}, "crafting_table"){ UniqueId = 536 },
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
					}, "crafting_table"){ UniqueId = 537 },
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
					}, "crafting_table"){ UniqueId = 538 },
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
					}, "crafting_table"){ UniqueId = 539 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-270, 0),
					},
					new Item[]
					{
						new Item(742, 32767),
						new Item(742, 32767),
						new Item(742, 32767),
						new Item(742, 32767),
						new Item(742, 32767),
						new Item(742, 32767),
						new Item(742, 32767),
						new Item(742, 32767),
						new Item(742, 32767),
					}, "crafting_table"){ UniqueId = 71 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(742, 0, 1),
					},
					new List<Item>
					{
						new Item(752, 32767, 1),
						new Item(752, 32767, 1),
						new Item(752, 32767, 1),
						new Item(752, 32767, 1),
						new Item(266, 32767, 1),
						new Item(266, 32767, 1),
						new Item(266, 32767, 1),
						new Item(266, 32767, 1),
					}, "crafting_table"){ UniqueId = 72 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(742, 0),
					},
					new Item[]
					{
						new Item(-270, 32767),
					}, "crafting_table"){ UniqueId = 73 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(25, 0),
					},
					new Item[]
					{
						new Item(-242, 32767),
						new Item(-242, 32767),
						new Item(-242, 32767),
						new Item(-242, 32767),
						new Item(331, 32767),
						new Item(-242, 32767),
						new Item(-242, 32767),
						new Item(-242, 32767),
						new Item(-242, 32767),
					}, "crafting_table"){ UniqueId = 74 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(25, 0),
					},
					new Item[]
					{
						new Item(-243, 32767),
						new Item(-243, 32767),
						new Item(-243, 32767),
						new Item(-243, 32767),
						new Item(331, 32767),
						new Item(-243, 32767),
						new Item(-243, 32767),
						new Item(-243, 32767),
						new Item(-243, 32767),
					}, "crafting_table"){ UniqueId = 75 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(5, 0),
					},
					new Item[]
					{
						new Item(17, 0),
					}, "crafting_table"){ UniqueId = 540 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(5, 0),
					},
					new Item[]
					{
						new Item(-10, 32767),
					}, "crafting_table"){ UniqueId = 541 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(5, 0),
					},
					new Item[]
					{
						new Item(-212, 8),
					}, "crafting_table"){ UniqueId = 542 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(5, 0),
					},
					new Item[]
					{
						new Item(-212, 0),
					}, "crafting_table"){ UniqueId = 543 },
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
					}, "crafting_table"){ UniqueId = 544 },
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
					}, "crafting_table"){ UniqueId = 545 },
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
					}, "crafting_table"){ UniqueId = 547 },
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
					}, "crafting_table"){ UniqueId = 546 },
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
					}, "crafting_table"){ UniqueId = 548 },
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
					}, "crafting_table"){ UniqueId = 549 },
				new ShapedRecipe(2, 1,
					new List<Item>
					{
						new Item(171, 1),
					},
					new Item[]
					{
						new Item(35, 1),
						new Item(35, 1),
					}, "crafting_table"){ UniqueId = 550 },
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
					}, "crafting_table"){ UniqueId = 551 },
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
					}, "crafting_table"){ UniqueId = 552 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 14, 1),
					},
					new List<Item>
					{
						new Item(38, 5, 1),
					}, "crafting_table"){ UniqueId = 553 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 14, 2),
					},
					new List<Item>
					{
						new Item(351, 1, 1),
						new Item(351, 11, 1),
					}, "crafting_table"){ UniqueId = 554 },
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
					}, "crafting_table"){ UniqueId = 555 },
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
					}, "crafting_table"){ UniqueId = 556 },
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
					}, "crafting_table"){ UniqueId = 557 },
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
					}, "crafting_table"){ UniqueId = 558 },
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
					}, "crafting_table"){ UniqueId = 559 },
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
					}, "crafting_table"){ UniqueId = 560 },
				new ShapedRecipe(1, 2,
					new List<Item>
					{
						new Item(155, 2),
					},
					new Item[]
					{
						new Item(155, 0),
						new Item(155, 0),
					}, "crafting_table"){ UniqueId = 561 },
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
					}, "crafting_table"){ UniqueId = 562 },
				new ShapedRecipe(2, 1,
					new List<Item>
					{
						new Item(171, 6),
					},
					new Item[]
					{
						new Item(35, 6),
						new Item(35, 6),
					}, "crafting_table"){ UniqueId = 563 },
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
					}, "crafting_table"){ UniqueId = 564 },
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
					}, "crafting_table"){ UniqueId = 565 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 9, 2),
					},
					new List<Item>
					{
						new Item(351, 1, 1),
						new Item(351, 19, 1),
					}, "crafting_table"){ UniqueId = 566 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 9, 2),
					},
					new List<Item>
					{
						new Item(175, 5, 1),
					}, "crafting_table"){ UniqueId = 567 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 9, 1),
					},
					new List<Item>
					{
						new Item(38, 7, 1),
					}, "crafting_table"){ UniqueId = 568 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 9, 2),
					},
					new List<Item>
					{
						new Item(351, 1, 1),
						new Item(351, 15, 1),
					}, "crafting_table"){ UniqueId = 569 },
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
					}, "crafting_table"){ UniqueId = 570 },
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
					}, "crafting_table"){ UniqueId = 571 },
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
					}, "crafting_table"){ UniqueId = 572 },
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
					}, "crafting_table"){ UniqueId = 573 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(33, 0),
					},
					new Item[]
					{
						new Item(-242, 32767),
						new Item(4, 32767),
						new Item(4, 32767),
						new Item(-242, 32767),
						new Item(265, 32767),
						new Item(331, 32767),
						new Item(-242, 32767),
						new Item(4, 32767),
						new Item(4, 32767),
					}, "crafting_table"){ UniqueId = 76 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(33, 0),
					},
					new Item[]
					{
						new Item(-243, 32767),
						new Item(4, 32767),
						new Item(4, 32767),
						new Item(-243, 32767),
						new Item(265, 32767),
						new Item(331, 32767),
						new Item(-243, 32767),
						new Item(4, 32767),
						new Item(4, 32767),
					}, "crafting_table"){ UniqueId = 77 },
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
					}, "crafting_table"){ UniqueId = 574 },
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
					}, "crafting_table"){ UniqueId = 575 },
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(-235, 0),
					},
					new Item[]
					{
						new Item(-234, 32767),
						new Item(-234, 32767),
						new Item(-234, 32767),
						new Item(-234, 32767),
					}, "crafting_table"){ UniqueId = 78 },
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(-291, 0),
					},
					new Item[]
					{
						new Item(-273, 32767),
						new Item(-273, 32767),
						new Item(-273, 32767),
						new Item(-273, 32767),
					}, "crafting_table"){ UniqueId = 79 },
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(-284, 0),
					},
					new Item[]
					{
						new Item(-274, 32767),
						new Item(-274, 32767),
						new Item(-274, 32767),
					}, "crafting_table"){ UniqueId = 81 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-275, 0),
					},
					new Item[]
					{
						new Item(-274, 32767),
						new Item(-274, 32767),
						new Item(-274, 32767),
						new Item(0, 0),
						new Item(-274, 32767),
						new Item(-274, 32767),
						new Item(0, 0),
						new Item(0, 0),
						new Item(-274, 32767),
					}, "crafting_table"){ UniqueId = 82 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(-278, 0),
					},
					new Item[]
					{
						new Item(-274, 32767),
						new Item(-274, 32767),
						new Item(-274, 32767),
						new Item(-274, 32767),
						new Item(-274, 32767),
						new Item(-274, 32767),
					}, "crafting_table"){ UniqueId = 83 },
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(-274, 0),
					},
					new Item[]
					{
						new Item(-291, 32767),
						new Item(-291, 32767),
						new Item(-291, 32767),
						new Item(-291, 32767),
					}, "crafting_table"){ UniqueId = 80 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(-296, 0),
					},
					new Item[]
					{
						new Item(-291, 32767),
					}, "crafting_table"){ UniqueId = 84 },
				new ShapedRecipe(2, 1,
					new List<Item>
					{
						new Item(-295, 0),
					},
					new Item[]
					{
						new Item(-291, 32767),
						new Item(-291, 32767),
					}, "crafting_table"){ UniqueId = 85 },
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(-293, 0),
					},
					new Item[]
					{
						new Item(-291, 32767),
						new Item(-291, 32767),
						new Item(-291, 32767),
					}, "crafting_table"){ UniqueId = 86 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-292, 0),
					},
					new Item[]
					{
						new Item(-291, 32767),
						new Item(-291, 32767),
						new Item(-291, 32767),
						new Item(0, 0),
						new Item(-291, 32767),
						new Item(-291, 32767),
						new Item(0, 0),
						new Item(0, 0),
						new Item(-291, 32767),
					}, "crafting_table"){ UniqueId = 87 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(-297, 0),
					},
					new Item[]
					{
						new Item(-291, 32767),
						new Item(-291, 32767),
						new Item(-291, 32767),
						new Item(-291, 32767),
						new Item(-291, 32767),
						new Item(-291, 32767),
					}, "crafting_table"){ UniqueId = 88 },
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
					}, "crafting_table"){ UniqueId = 576 },
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
					}, "crafting_table"){ UniqueId = 577 },
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
					}, "crafting_table"){ UniqueId = 578 },
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
					}, "crafting_table"){ UniqueId = 579 },
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
					}, "crafting_table"){ UniqueId = 580 },
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
					}, "crafting_table"){ UniqueId = 581 },
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
					}, "crafting_table"){ UniqueId = 582 },
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
					}, "crafting_table"){ UniqueId = 583 },
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
					}, "crafting_table"){ UniqueId = 584 },
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
					}, "crafting_table"){ UniqueId = 585 },
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
					}, "crafting_table"){ UniqueId = 586 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(361, 0),
					},
					new Item[]
					{
						new Item(86, 32767),
					}, "crafting_table"){ UniqueId = 587 },
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
					}, "crafting_table"){ UniqueId = 588 },
				new ShapedRecipe(2, 1,
					new List<Item>
					{
						new Item(171, 10),
					},
					new Item[]
					{
						new Item(35, 10),
						new Item(35, 10),
					}, "crafting_table"){ UniqueId = 589 },
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
					}, "crafting_table"){ UniqueId = 590 },
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
					}, "crafting_table"){ UniqueId = 591 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 5, 2),
					},
					new List<Item>
					{
						new Item(351, 18, 1),
						new Item(351, 1, 1),
					}, "crafting_table"){ UniqueId = 592 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 5, 2),
					},
					new List<Item>
					{
						new Item(351, 4, 1),
						new Item(351, 1, 1),
					}, "crafting_table"){ UniqueId = 593 },
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
					}, "crafting_table"){ UniqueId = 594 },
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
					}, "crafting_table"){ UniqueId = 595 },
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
					}, "crafting_table"){ UniqueId = 596 },
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
					}, "crafting_table"){ UniqueId = 597 },
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
					}, "crafting_table"){ UniqueId = 598 },
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
					}, "crafting_table"){ UniqueId = 599 },
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
					}, "crafting_table"){ UniqueId = 600 },
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(-304, 0),
					},
					new Item[]
					{
						new Item(155, 0),
						new Item(155, 0),
						new Item(155, 0),
						new Item(155, 0),
					}, "crafting_table"){ UniqueId = 89 },
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
					}, "crafting_table"){ UniqueId = 601 },
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
					}, "crafting_table"){ UniqueId = 602 },
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
					}, "crafting_table"){ UniqueId = 603 },
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
					}, "crafting_table"){ UniqueId = 604 },
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
					}, "crafting_table"){ UniqueId = 609 },
				new ShapedRecipe(2, 1,
					new List<Item>
					{
						new Item(171, 14),
					},
					new Item[]
					{
						new Item(35, 14),
						new Item(35, 14),
					}, "crafting_table"){ UniqueId = 610 },
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
					}, "crafting_table"){ UniqueId = 611 },
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
					}, "crafting_table"){ UniqueId = 612 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 1, 1),
					},
					new List<Item>
					{
						new Item(457, 32767, 1),
					}, "crafting_table"){ UniqueId = 613 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 1, 1),
					},
					new List<Item>
					{
						new Item(38, 0, 1),
					}, "crafting_table"){ UniqueId = 614 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 1, 2),
					},
					new List<Item>
					{
						new Item(175, 4, 1),
					}, "crafting_table"){ UniqueId = 615 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 1, 1),
					},
					new List<Item>
					{
						new Item(38, 4, 1),
					}, "crafting_table"){ UniqueId = 616 },
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
					}, "crafting_table"){ UniqueId = 617 },
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
					}, "crafting_table"){ UniqueId = 618 },
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
					}, "crafting_table"){ UniqueId = 619 },
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
					}, "crafting_table"){ UniqueId = 620 },
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
					}, "crafting_table"){ UniqueId = 621 },
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
					}, "crafting_table"){ UniqueId = 622 },
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
					}, "crafting_table"){ UniqueId = 623 },
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
					}, "crafting_table"){ UniqueId = 624 },
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
					}, "crafting_table"){ UniqueId = 625 },
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
					}, "crafting_table"){ UniqueId = 626 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(331, 0),
					},
					new Item[]
					{
						new Item(152, 32767),
					}, "crafting_table"){ UniqueId = 605 },
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
					}, "crafting_table"){ UniqueId = 606 },
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
					}, "crafting_table"){ UniqueId = 607 },
				new ShapedRecipe(1, 2,
					new List<Item>
					{
						new Item(76, 0),
					},
					new Item[]
					{
						new Item(331, 32767),
						new Item(280, 32767),
					}, "crafting_table"){ UniqueId = 608 },
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
					}, "crafting_table"){ UniqueId = 627 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-272, 0),
					},
					new Item[]
					{
						new Item(-289, 32767),
						new Item(89, 32767),
						new Item(-289, 32767),
						new Item(-289, 32767),
						new Item(89, 32767),
						new Item(-289, 32767),
						new Item(-289, 32767),
						new Item(89, 32767),
						new Item(-289, 32767),
					}, "crafting_table"){ UniqueId = 90 },
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
					}, "crafting_table"){ UniqueId = 628 },
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
					}, "crafting_table"){ UniqueId = 629 },
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
					}, "crafting_table"){ UniqueId = 630 },
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
					}, "crafting_table"){ UniqueId = 631 },
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
					}, "crafting_table"){ UniqueId = 632 },
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
					}, "crafting_table"){ UniqueId = 633 },
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
					}, "crafting_table"){ UniqueId = 634 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(513, 0),
					},
					new Item[]
					{
						new Item(-242, 32767),
						new Item(-242, 32767),
						new Item(0, 0),
						new Item(265, 32767),
						new Item(-242, 32767),
						new Item(-242, 32767),
						new Item(-242, 32767),
						new Item(-242, 32767),
						new Item(0, 0),
					}, "crafting_table"){ UniqueId = 91 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(513, 0),
					},
					new Item[]
					{
						new Item(-243, 32767),
						new Item(-243, 32767),
						new Item(0, 0),
						new Item(265, 32767),
						new Item(-243, 32767),
						new Item(-243, 32767),
						new Item(-243, 32767),
						new Item(-243, 32767),
						new Item(0, 0),
					}, "crafting_table"){ UniqueId = 92 },
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
					}, "crafting_table"){ UniqueId = 635 },
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
					}, "crafting_table"){ UniqueId = 636 },
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
					}, "crafting_table"){ UniqueId = 637 },
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
					}, "crafting_table"){ UniqueId = 638 },
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
					}, "crafting_table"){ UniqueId = 639 },
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
					}, "crafting_table"){ UniqueId = 640 },
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
					}, "crafting_table"){ UniqueId = 641 },
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
					}, "crafting_table"){ UniqueId = 642 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(341, 0),
					},
					new Item[]
					{
						new Item(165, 32767),
					}, "crafting_table"){ UniqueId = 643 },
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
					}, "crafting_table"){ UniqueId = 644 },
				new ShapedRecipe(2, 3,
					new List<Item>
					{
						new Item(-202, 0),
					},
					new Item[]
					{
						new Item(265, 32767),
						new Item(-242, 32767),
						new Item(265, 32767),
						new Item(-242, 32767),
						new Item(-242, 32767),
						new Item(-242, 32767),
					}, "crafting_table"){ UniqueId = 93 },
				new ShapedRecipe(2, 3,
					new List<Item>
					{
						new Item(-202, 0),
					},
					new Item[]
					{
						new Item(265, 32767),
						new Item(-243, 32767),
						new Item(265, 32767),
						new Item(-243, 32767),
						new Item(-243, 32767),
						new Item(-243, 32767),
					}, "crafting_table"){ UniqueId = 94 },
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
					}, "crafting_table"){ UniqueId = 645 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-198, 0),
					},
					new Item[]
					{
						new Item(0, 0),
						new Item(-225, 32767),
						new Item(0, 0),
						new Item(-225, 32767),
						new Item(61, 32767),
						new Item(-225, 32767),
						new Item(0, 0),
						new Item(-225, 32767),
						new Item(0, 0),
					}, "crafting_table"){ UniqueId = 95 },
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
					}, "crafting_table"){ UniqueId = 646 },
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
					}, "crafting_table"){ UniqueId = 647 },
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
					}, "crafting_table"){ UniqueId = 648 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-198, 0),
					},
					new Item[]
					{
						new Item(0, 0),
						new Item(-240, 32767),
						new Item(0, 0),
						new Item(-240, 32767),
						new Item(61, 32767),
						new Item(-240, 32767),
						new Item(0, 0),
						new Item(-240, 32767),
						new Item(0, 0),
					}, "crafting_table"){ UniqueId = 96 },
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
					}, "crafting_table"){ UniqueId = 649 },
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
					}, "crafting_table"){ UniqueId = 650 },
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
					}, "crafting_table"){ UniqueId = 651 },
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
					}, "crafting_table"){ UniqueId = 652 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-198, 0),
					},
					new Item[]
					{
						new Item(0, 0),
						new Item(-241, 32767),
						new Item(0, 0),
						new Item(-241, 32767),
						new Item(61, 32767),
						new Item(-241, 32767),
						new Item(0, 0),
						new Item(-241, 32767),
						new Item(0, 0),
					}, "crafting_table"){ UniqueId = 97 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-198, 0),
					},
					new Item[]
					{
						new Item(0, 0),
						new Item(-226, 32767),
						new Item(0, 0),
						new Item(-226, 32767),
						new Item(61, 32767),
						new Item(-226, 32767),
						new Item(0, 0),
						new Item(-226, 32767),
						new Item(0, 0),
					}, "crafting_table"){ UniqueId = 98 },
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
					}, "crafting_table"){ UniqueId = 653 },
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
					}, "crafting_table"){ UniqueId = 654 },
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
					}, "crafting_table"){ UniqueId = 655 },
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
					}, "crafting_table"){ UniqueId = 656 },
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
					}, "crafting_table"){ UniqueId = 657 },
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
					}, "crafting_table"){ UniqueId = 658 },
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
					}, "crafting_table"){ UniqueId = 659 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(801, 0),
					},
					new Item[]
					{
						new Item(0, 0),
						new Item(280, 32767),
						new Item(-225, 32767),
						new Item(280, 32767),
						new Item(-236, 32767),
						new Item(-225, 32767),
						new Item(0, 0),
						new Item(280, 32767),
						new Item(-225, 32767),
					}, "crafting_table"){ UniqueId = 99 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(801, 0),
					},
					new Item[]
					{
						new Item(0, 0),
						new Item(280, 32767),
						new Item(-225, 32767),
						new Item(280, 32767),
						new Item(88, 32767),
						new Item(-225, 32767),
						new Item(0, 0),
						new Item(280, 32767),
						new Item(-225, 32767),
					}, "crafting_table"){ UniqueId = 100 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(801, 0),
					},
					new Item[]
					{
						new Item(0, 0),
						new Item(280, 32767),
						new Item(17, 32767),
						new Item(280, 32767),
						new Item(88, 32767),
						new Item(17, 32767),
						new Item(0, 0),
						new Item(280, 32767),
						new Item(17, 32767),
					}, "crafting_table"){ UniqueId = 101 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(801, 0),
					},
					new Item[]
					{
						new Item(0, 0),
						new Item(280, 32767),
						new Item(162, 32767),
						new Item(280, 32767),
						new Item(88, 32767),
						new Item(162, 32767),
						new Item(0, 0),
						new Item(280, 32767),
						new Item(162, 32767),
					}, "crafting_table"){ UniqueId = 102 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(801, 0),
					},
					new Item[]
					{
						new Item(0, 0),
						new Item(280, 32767),
						new Item(-8, 32767),
						new Item(280, 32767),
						new Item(88, 32767),
						new Item(-8, 32767),
						new Item(0, 0),
						new Item(280, 32767),
						new Item(-8, 32767),
					}, "crafting_table"){ UniqueId = 103 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(801, 0),
					},
					new Item[]
					{
						new Item(0, 0),
						new Item(280, 32767),
						new Item(-6, 32767),
						new Item(280, 32767),
						new Item(88, 32767),
						new Item(-6, 32767),
						new Item(0, 0),
						new Item(280, 32767),
						new Item(-6, 32767),
					}, "crafting_table"){ UniqueId = 104 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(801, 0),
					},
					new Item[]
					{
						new Item(0, 0),
						new Item(280, 32767),
						new Item(-9, 32767),
						new Item(280, 32767),
						new Item(88, 32767),
						new Item(-9, 32767),
						new Item(0, 0),
						new Item(280, 32767),
						new Item(-9, 32767),
					}, "crafting_table"){ UniqueId = 105 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(801, 0),
					},
					new Item[]
					{
						new Item(0, 0),
						new Item(280, 32767),
						new Item(-7, 32767),
						new Item(280, 32767),
						new Item(88, 32767),
						new Item(-7, 32767),
						new Item(0, 0),
						new Item(280, 32767),
						new Item(-7, 32767),
					}, "crafting_table"){ UniqueId = 106 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(801, 0),
					},
					new Item[]
					{
						new Item(0, 0),
						new Item(280, 32767),
						new Item(-10, 32767),
						new Item(280, 32767),
						new Item(88, 32767),
						new Item(-10, 32767),
						new Item(0, 0),
						new Item(280, 32767),
						new Item(-10, 32767),
					}, "crafting_table"){ UniqueId = 107 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(801, 0),
					},
					new Item[]
					{
						new Item(0, 0),
						new Item(280, 32767),
						new Item(-5, 32767),
						new Item(280, 32767),
						new Item(88, 32767),
						new Item(-5, 32767),
						new Item(0, 0),
						new Item(280, 32767),
						new Item(-5, 32767),
					}, "crafting_table"){ UniqueId = 108 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(801, 0),
					},
					new Item[]
					{
						new Item(0, 0),
						new Item(280, 32767),
						new Item(-212, 32767),
						new Item(280, 32767),
						new Item(88, 32767),
						new Item(-212, 32767),
						new Item(0, 0),
						new Item(280, 32767),
						new Item(-212, 32767),
					}, "crafting_table"){ UniqueId = 109 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(801, 0),
					},
					new Item[]
					{
						new Item(0, 0),
						new Item(280, 32767),
						new Item(17, 32767),
						new Item(280, 32767),
						new Item(-236, 32767),
						new Item(17, 32767),
						new Item(0, 0),
						new Item(280, 32767),
						new Item(17, 32767),
					}, "crafting_table"){ UniqueId = 110 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(801, 0),
					},
					new Item[]
					{
						new Item(0, 0),
						new Item(280, 32767),
						new Item(162, 32767),
						new Item(280, 32767),
						new Item(-236, 32767),
						new Item(162, 32767),
						new Item(0, 0),
						new Item(280, 32767),
						new Item(162, 32767),
					}, "crafting_table"){ UniqueId = 111 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(801, 0),
					},
					new Item[]
					{
						new Item(0, 0),
						new Item(280, 32767),
						new Item(-8, 32767),
						new Item(280, 32767),
						new Item(-236, 32767),
						new Item(-8, 32767),
						new Item(0, 0),
						new Item(280, 32767),
						new Item(-8, 32767),
					}, "crafting_table"){ UniqueId = 112 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(801, 0),
					},
					new Item[]
					{
						new Item(0, 0),
						new Item(280, 32767),
						new Item(-6, 32767),
						new Item(280, 32767),
						new Item(-236, 32767),
						new Item(-6, 32767),
						new Item(0, 0),
						new Item(280, 32767),
						new Item(-6, 32767),
					}, "crafting_table"){ UniqueId = 113 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(801, 0),
					},
					new Item[]
					{
						new Item(0, 0),
						new Item(280, 32767),
						new Item(-9, 32767),
						new Item(280, 32767),
						new Item(-236, 32767),
						new Item(-9, 32767),
						new Item(0, 0),
						new Item(280, 32767),
						new Item(-9, 32767),
					}, "crafting_table"){ UniqueId = 114 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(801, 0),
					},
					new Item[]
					{
						new Item(0, 0),
						new Item(280, 32767),
						new Item(-7, 32767),
						new Item(280, 32767),
						new Item(-236, 32767),
						new Item(-7, 32767),
						new Item(0, 0),
						new Item(280, 32767),
						new Item(-7, 32767),
					}, "crafting_table"){ UniqueId = 115 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(801, 0),
					},
					new Item[]
					{
						new Item(0, 0),
						new Item(280, 32767),
						new Item(-10, 32767),
						new Item(280, 32767),
						new Item(-236, 32767),
						new Item(-10, 32767),
						new Item(0, 0),
						new Item(280, 32767),
						new Item(-10, 32767),
					}, "crafting_table"){ UniqueId = 116 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(801, 0),
					},
					new Item[]
					{
						new Item(0, 0),
						new Item(280, 32767),
						new Item(-5, 32767),
						new Item(280, 32767),
						new Item(-236, 32767),
						new Item(-5, 32767),
						new Item(0, 0),
						new Item(280, 32767),
						new Item(-5, 32767),
					}, "crafting_table"){ UniqueId = 117 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(801, 0),
					},
					new Item[]
					{
						new Item(0, 0),
						new Item(280, 32767),
						new Item(-212, 32767),
						new Item(280, 32767),
						new Item(-236, 32767),
						new Item(-212, 32767),
						new Item(0, 0),
						new Item(280, 32767),
						new Item(-212, 32767),
					}, "crafting_table"){ UniqueId = 118 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(801, 0),
					},
					new Item[]
					{
						new Item(0, 0),
						new Item(280, 32767),
						new Item(-240, 32767),
						new Item(280, 32767),
						new Item(-236, 32767),
						new Item(-240, 32767),
						new Item(0, 0),
						new Item(280, 32767),
						new Item(-240, 32767),
					}, "crafting_table"){ UniqueId = 119 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(801, 0),
					},
					new Item[]
					{
						new Item(0, 0),
						new Item(280, 32767),
						new Item(-240, 32767),
						new Item(280, 32767),
						new Item(88, 32767),
						new Item(-240, 32767),
						new Item(0, 0),
						new Item(280, 32767),
						new Item(-240, 32767),
					}, "crafting_table"){ UniqueId = 120 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(801, 0),
					},
					new Item[]
					{
						new Item(0, 0),
						new Item(280, 32767),
						new Item(-241, 32767),
						new Item(280, 32767),
						new Item(-236, 32767),
						new Item(-241, 32767),
						new Item(0, 0),
						new Item(280, 32767),
						new Item(-241, 32767),
					}, "crafting_table"){ UniqueId = 121 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(801, 0),
					},
					new Item[]
					{
						new Item(0, 0),
						new Item(280, 32767),
						new Item(-241, 32767),
						new Item(280, 32767),
						new Item(88, 32767),
						new Item(-241, 32767),
						new Item(0, 0),
						new Item(280, 32767),
						new Item(-241, 32767),
					}, "crafting_table"){ UniqueId = 122 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(801, 0),
					},
					new Item[]
					{
						new Item(0, 0),
						new Item(280, 32767),
						new Item(-226, 32767),
						new Item(280, 32767),
						new Item(-236, 32767),
						new Item(-226, 32767),
						new Item(0, 0),
						new Item(280, 32767),
						new Item(-226, 32767),
					}, "crafting_table"){ UniqueId = 123 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(801, 0),
					},
					new Item[]
					{
						new Item(0, 0),
						new Item(280, 32767),
						new Item(-226, 32767),
						new Item(280, 32767),
						new Item(88, 32767),
						new Item(-226, 32767),
						new Item(0, 0),
						new Item(280, 32767),
						new Item(-226, 32767),
					}, "crafting_table"){ UniqueId = 124 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-269, 0),
					},
					new Item[]
					{
						new Item(452, 32767),
						new Item(452, 32767),
						new Item(452, 32767),
						new Item(452, 32767),
						new Item(-268, 32767),
						new Item(452, 32767),
						new Item(452, 32767),
						new Item(452, 32767),
						new Item(452, 32767),
					}, "crafting_table"){ UniqueId = 125 },
				new ShapedRecipe(1, 3,
					new List<Item>
					{
						new Item(-268, 0),
					},
					new Item[]
					{
						new Item(263, 32767),
						new Item(280, 32767),
						new Item(88, 32767),
					}, "crafting_table"){ UniqueId = 126 },
				new ShapedRecipe(1, 3,
					new List<Item>
					{
						new Item(-268, 0),
					},
					new Item[]
					{
						new Item(263, 32767),
						new Item(280, 32767),
						new Item(-236, 32767),
					}, "crafting_table"){ UniqueId = 127 },
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
					}, "crafting_table"){ UniqueId = 660 },
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
						new Item(269, 32767),
						new Item(5, 1),
						new Item(5, 1),
					}, "crafting_table"){ UniqueId = 661 },
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
					}, "crafting_table"){ UniqueId = 662 },
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
					}, "crafting_table"){ UniqueId = 663 },
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
					}, "crafting_table"){ UniqueId = 664 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(5, 1),
					},
					new Item[]
					{
						new Item(17, 1),
					}, "crafting_table"){ UniqueId = 665 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(5, 1),
					},
					new Item[]
					{
						new Item(-5, 32767),
					}, "crafting_table"){ UniqueId = 666 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(5, 1),
					},
					new Item[]
					{
						new Item(-212, 9),
					}, "crafting_table"){ UniqueId = 667 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(5, 1),
					},
					new Item[]
					{
						new Item(-212, 1),
					}, "crafting_table"){ UniqueId = 668 },
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
					}, "crafting_table"){ UniqueId = 669 },
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
					}, "crafting_table"){ UniqueId = 670 },
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
					}, "crafting_table"){ UniqueId = 672 },
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
					}, "crafting_table"){ UniqueId = 671 },
				new ShapedRecipe(1, 2,
					new List<Item>
					{
						new Item(280, 0),
					},
					new Item[]
					{
						new Item(-242, 32767),
						new Item(-242, 32767),
					}, "crafting_table"){ UniqueId = 128 },
				new ShapedRecipe(1, 2,
					new List<Item>
					{
						new Item(280, 0),
					},
					new Item[]
					{
						new Item(-243, 32767),
						new Item(-243, 32767),
					}, "crafting_table"){ UniqueId = 129 },
				new ShapedRecipe(1, 2,
					new List<Item>
					{
						new Item(29, 1),
					},
					new Item[]
					{
						new Item(341, 32767),
						new Item(33, 32767),
					}, "crafting_table"){ UniqueId = 673 },
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
					}, "crafting_table"){ UniqueId = 765 },
				new ShapedRecipe(2, 3,
					new List<Item>
					{
						new Item(275, 0),
					},
					new Item[]
					{
						new Item(-273, 32767),
						new Item(280, 32767),
						new Item(-273, 32767),
						new Item(0, 0),
						new Item(-273, 32767),
						new Item(280, 32767),
					}, "crafting_table"){ UniqueId = 157 },
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
					}, "crafting_table"){ UniqueId = 766 },
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
					}, "crafting_table"){ UniqueId = 767 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(77, 0),
					},
					new Item[]
					{
						new Item(1, 0),
					}, "crafting_table"){ UniqueId = 768 },
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
					}, "crafting_table"){ UniqueId = 769 },
				new ShapedRecipe(2, 3,
					new List<Item>
					{
						new Item(291, 0),
					},
					new Item[]
					{
						new Item(-273, 32767),
						new Item(280, 32767),
						new Item(-273, 32767),
						new Item(0, 0),
						new Item(0, 0),
						new Item(280, 32767),
					}, "crafting_table"){ UniqueId = 158 },
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
					}, "crafting_table"){ UniqueId = 770 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(274, 0),
					},
					new Item[]
					{
						new Item(-273, 32767),
						new Item(0, 0),
						new Item(0, 0),
						new Item(-273, 32767),
						new Item(280, 32767),
						new Item(280, 32767),
						new Item(-273, 32767),
						new Item(0, 0),
						new Item(0, 0),
					}, "crafting_table"){ UniqueId = 159 },
				new ShapedRecipe(2, 1,
					new List<Item>
					{
						new Item(70, 0),
					},
					new Item[]
					{
						new Item(1, 0),
						new Item(1, 0),
					}, "crafting_table"){ UniqueId = 771 },
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
					}, "crafting_table"){ UniqueId = 772 },
				new ShapedRecipe(1, 3,
					new List<Item>
					{
						new Item(273, 0),
					},
					new Item[]
					{
						new Item(-273, 32767),
						new Item(280, 32767),
						new Item(280, 32767),
					}, "crafting_table"){ UniqueId = 160 },
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
					}, "crafting_table"){ UniqueId = 773 },
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
					}, "crafting_table"){ UniqueId = 774 },
				new ShapedRecipe(1, 3,
					new List<Item>
					{
						new Item(272, 0),
					},
					new Item[]
					{
						new Item(-273, 32767),
						new Item(-273, 32767),
						new Item(280, 32767),
					}, "crafting_table"){ UniqueId = 161 },
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
					}, "crafting_table"){ UniqueId = 674 },
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
					}, "crafting_table"){ UniqueId = 675 },
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
					}, "crafting_table"){ UniqueId = 775 },
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(-300, 0),
					},
					new Item[]
					{
						new Item(-240, 32767),
						new Item(-240, 32767),
						new Item(-240, 32767),
						new Item(-240, 32767),
					}, "crafting_table"){ UniqueId = 41 },
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(-301, 0),
					},
					new Item[]
					{
						new Item(-241, 32767),
						new Item(-241, 32767),
						new Item(-241, 32767),
						new Item(-241, 32767),
					}, "crafting_table"){ UniqueId = 171 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(353, 0),
					},
					new Item[]
					{
						new Item(338, 32767),
					}, "crafting_table"){ UniqueId = 776 },
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
					}, "crafting_table"){ UniqueId = 777 },
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
					}, "crafting_table"){ UniqueId = 778 },
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
					}, "crafting_table"){ UniqueId = 779 },
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
					}, "crafting_table"){ UniqueId = 780 },
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
					}, "crafting_table"){ UniqueId = 781 },
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
					}, "crafting_table"){ UniqueId = 782 },
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
					}, "crafting_table"){ UniqueId = 783 },
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
					}, "crafting_table"){ UniqueId = 784 },
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
					}, "crafting_table"){ UniqueId = 785 },
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
					}, "crafting_table"){ UniqueId = 786 },
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
					}, "crafting_table"){ UniqueId = 787 },
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
					}, "crafting_table"){ UniqueId = 788 },
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
					}, "crafting_table"){ UniqueId = 789 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-239, 0),
					},
					new Item[]
					{
						new Item(0, 0),
						new Item(331, 32767),
						new Item(0, 0),
						new Item(331, 32767),
						new Item(170, 32767),
						new Item(331, 32767),
						new Item(0, 0),
						new Item(331, 32767),
						new Item(0, 0),
					}, "crafting_table"){ UniqueId = 162 },
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
					}, "crafting_table"){ UniqueId = 790 },
				new ShapedRecipe(1, 2,
					new List<Item>
					{
						new Item(407, 0),
					},
					new Item[]
					{
						new Item(46, 0),
						new Item(328, 32767),
					}, "crafting_table"){ UniqueId = 791 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(146, 0, 1),
					},
					new List<Item>
					{
						new Item(54, 32767, 1),
						new Item(131, 32767, 1),
					}, "crafting_table"){ UniqueId = 792 },
				new ShapedRecipe(1, 3,
					new List<Item>
					{
						new Item(131, 0),
					},
					new Item[]
					{
						new Item(265, 32767),
						new Item(280, 32767),
						new Item(-242, 32767),
					}, "crafting_table"){ UniqueId = 163 },
				new ShapedRecipe(1, 3,
					new List<Item>
					{
						new Item(131, 0),
					},
					new Item[]
					{
						new Item(265, 32767),
						new Item(280, 32767),
						new Item(-243, 32767),
					}, "crafting_table"){ UniqueId = 164 },
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
					}, "crafting_table"){ UniqueId = 793 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(-261, 0),
					},
					new Item[]
					{
						new Item(-243, 32767),
					}, "crafting_table"){ UniqueId = 165 },
				new ShapedRecipe(2, 3,
					new List<Item>
					{
						new Item(756, 0),
					},
					new Item[]
					{
						new Item(-243, 32767),
						new Item(-243, 32767),
						new Item(-243, 32767),
						new Item(-243, 32767),
						new Item(-243, 32767),
						new Item(-243, 32767),
					}, "crafting_table"){ UniqueId = 166 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(-257, 0),
					},
					new Item[]
					{
						new Item(-243, 32767),
						new Item(-243, 32767),
						new Item(280, 32767),
						new Item(280, 32767),
						new Item(-243, 32767),
						new Item(-243, 32767),
					}, "crafting_table"){ UniqueId = 167 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(-259, 0),
					},
					new Item[]
					{
						new Item(280, 32767),
						new Item(280, 32767),
						new Item(-243, 32767),
						new Item(-243, 32767),
						new Item(280, 32767),
						new Item(280, 32767),
					}, "crafting_table"){ UniqueId = 168 },
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(757, 0),
					},
					new Item[]
					{
						new Item(346, 32767),
						new Item(0, 0),
						new Item(0, 0),
						new Item(-229, 32767),
					}, "crafting_table"){ UniqueId = 169 },
				new ShapedRecipe(2, 2,
					new List<Item>
					{
						new Item(-298, 0),
					},
					new Item[]
					{
						new Item(-226, 32767),
						new Item(-226, 32767),
						new Item(-226, 32767),
						new Item(-226, 32767),
					}, "crafting_table"){ UniqueId = 170 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(-243, 0),
					},
					new Item[]
					{
						new Item(-226, 32767),
					}, "crafting_table"){ UniqueId = 172 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(-243, 0),
					},
					new Item[]
					{
						new Item(-241, 32767),
					}, "crafting_table"){ UniqueId = 173 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(-243, 0),
					},
					new Item[]
					{
						new Item(-301, 32767),
					}, "crafting_table"){ UniqueId = 174 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(-243, 0),
					},
					new Item[]
					{
						new Item(-298, 32767),
					}, "crafting_table"){ UniqueId = 175 },
				new ShapedRecipe(2, 1,
					new List<Item>
					{
						new Item(-263, 0),
					},
					new Item[]
					{
						new Item(-243, 32767),
						new Item(-243, 32767),
					}, "crafting_table"){ UniqueId = 176 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(754, 0),
					},
					new Item[]
					{
						new Item(-243, 32767),
						new Item(-243, 32767),
						new Item(0, 0),
						new Item(-243, 32767),
						new Item(-243, 32767),
						new Item(280, 32767),
						new Item(-243, 32767),
						new Item(-243, 32767),
						new Item(0, 0),
					}, "crafting_table"){ UniqueId = 177 },
				new ShapedRecipe(3, 1,
					new List<Item>
					{
						new Item(-265, 0),
					},
					new Item[]
					{
						new Item(-243, 32767),
						new Item(-243, 32767),
						new Item(-243, 32767),
					}, "crafting_table"){ UniqueId = 178 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(-255, 0),
					},
					new Item[]
					{
						new Item(-243, 32767),
						new Item(-243, 32767),
						new Item(-243, 32767),
						new Item(0, 0),
						new Item(-243, 32767),
						new Item(-243, 32767),
						new Item(0, 0),
						new Item(0, 0),
						new Item(-243, 32767),
					}, "crafting_table"){ UniqueId = 179 },
				new ShapedRecipe(3, 2,
					new List<Item>
					{
						new Item(-247, 0),
					},
					new Item[]
					{
						new Item(-243, 32767),
						new Item(-243, 32767),
						new Item(-243, 32767),
						new Item(-243, 32767),
						new Item(-243, 32767),
						new Item(-243, 32767),
					}, "crafting_table"){ UniqueId = 180 },
				new ShapedRecipe(1, 1,
					new List<Item>
					{
						new Item(296, 0),
					},
					new Item[]
					{
						new Item(170, 32767),
					}, "crafting_table"){ UniqueId = 794 },
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
					}, "crafting_table"){ UniqueId = 795 },
				new ShapedRecipe(2, 1,
					new List<Item>
					{
						new Item(171, 0),
					},
					new Item[]
					{
						new Item(35, 0),
						new Item(35, 0),
					}, "crafting_table"){ UniqueId = 796 },
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
					}, "crafting_table"){ UniqueId = 797 },
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
					}, "crafting_table"){ UniqueId = 798 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 19, 1),
					},
					new List<Item>
					{
						new Item(351, 15, 1),
					}, "crafting_table"){ UniqueId = 799 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 19, 1),
					},
					new List<Item>
					{
						new Item(38, 10, 1),
					}, "crafting_table"){ UniqueId = 800 },
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
					}, "crafting_table"){ UniqueId = 801 },
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
					}, "crafting_table"){ UniqueId = 802 },
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
					}, "crafting_table"){ UniqueId = 803 },
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
					}, "crafting_table"){ UniqueId = 804 },
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
					}, "crafting_table"){ UniqueId = 805 },
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
					}, "crafting_table"){ UniqueId = 806 },
				new ShapedRecipe(2, 3,
					new List<Item>
					{
						new Item(271, 0),
					},
					new Item[]
					{
						new Item(-242, 32767),
						new Item(280, 32767),
						new Item(-242, 32767),
						new Item(0, 0),
						new Item(-242, 32767),
						new Item(280, 32767),
					}, "crafting_table"){ UniqueId = 181 },
				new ShapedRecipe(2, 3,
					new List<Item>
					{
						new Item(271, 0),
					},
					new Item[]
					{
						new Item(-243, 32767),
						new Item(280, 32767),
						new Item(-243, 32767),
						new Item(0, 0),
						new Item(-243, 32767),
						new Item(280, 32767),
					}, "crafting_table"){ UniqueId = 182 },
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
					}, "crafting_table"){ UniqueId = 807 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(290, 0),
					},
					new Item[]
					{
						new Item(-242, 32767),
						new Item(0, 0),
						new Item(0, 0),
						new Item(-242, 32767),
						new Item(280, 32767),
						new Item(280, 32767),
						new Item(0, 0),
						new Item(0, 0),
						new Item(0, 0),
					}, "crafting_table"){ UniqueId = 183 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(290, 0),
					},
					new Item[]
					{
						new Item(-243, 32767),
						new Item(0, 0),
						new Item(0, 0),
						new Item(-243, 32767),
						new Item(280, 32767),
						new Item(280, 32767),
						new Item(0, 0),
						new Item(0, 0),
						new Item(0, 0),
					}, "crafting_table"){ UniqueId = 184 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(270, 0),
					},
					new Item[]
					{
						new Item(-242, 32767),
						new Item(0, 0),
						new Item(0, 0),
						new Item(-242, 32767),
						new Item(280, 32767),
						new Item(280, 32767),
						new Item(-242, 32767),
						new Item(0, 0),
						new Item(0, 0),
					}, "crafting_table"){ UniqueId = 185 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(270, 0),
					},
					new Item[]
					{
						new Item(-243, 32767),
						new Item(0, 0),
						new Item(0, 0),
						new Item(-243, 32767),
						new Item(280, 32767),
						new Item(280, 32767),
						new Item(-243, 32767),
						new Item(0, 0),
						new Item(0, 0),
					}, "crafting_table"){ UniqueId = 186 },
				new ShapedRecipe(1, 3,
					new List<Item>
					{
						new Item(269, 0),
					},
					new Item[]
					{
						new Item(-242, 32767),
						new Item(280, 32767),
						new Item(280, 32767),
					}, "crafting_table"){ UniqueId = 187 },
				new ShapedRecipe(1, 3,
					new List<Item>
					{
						new Item(269, 0),
					},
					new Item[]
					{
						new Item(-243, 32767),
						new Item(280, 32767),
						new Item(280, 32767),
					}, "crafting_table"){ UniqueId = 188 },
				new ShapedRecipe(1, 3,
					new List<Item>
					{
						new Item(268, 0),
					},
					new Item[]
					{
						new Item(-242, 32767),
						new Item(-242, 32767),
						new Item(280, 32767),
					}, "crafting_table"){ UniqueId = 189 },
				new ShapedRecipe(1, 3,
					new List<Item>
					{
						new Item(268, 0),
					},
					new Item[]
					{
						new Item(-243, 32767),
						new Item(-243, 32767),
						new Item(280, 32767),
					}, "crafting_table"){ UniqueId = 190 },
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
					}, "crafting_table"){ UniqueId = 808 },
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
					}, "crafting_table"){ UniqueId = 809 },
				new ShapedRecipe(2, 1,
					new List<Item>
					{
						new Item(171, 4),
					},
					new Item[]
					{
						new Item(35, 4),
						new Item(35, 4),
					}, "crafting_table"){ UniqueId = 810 },
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
					}, "crafting_table"){ UniqueId = 811 },
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
					}, "crafting_table"){ UniqueId = 812 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 11, 1),
					},
					new List<Item>
					{
						new Item(37, 0, 1),
					}, "crafting_table"){ UniqueId = 813 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(351, 11, 2),
					},
					new List<Item>
					{
						new Item(175, 0, 1),
					}, "crafting_table"){ UniqueId = 814 },
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
					}, "crafting_table"){ UniqueId = 815 },
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
					}, "crafting_table"){ UniqueId = 816 },
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
					}, "crafting_table"){ UniqueId = 817 },
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
					}, "crafting_table"){ UniqueId = 818 },
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
					}, "crafting_table"){ UniqueId = 1850 },
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
					}, "crafting_table"){ UniqueId = 1901 },
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
					}, "crafting_table"){ UniqueId = 1881 },
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
					}, "crafting_table"){ UniqueId = 1879 },
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
					}, "crafting_table"){ UniqueId = 1877 },
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
					}, "crafting_table"){ UniqueId = 1875 },
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
					}, "crafting_table"){ UniqueId = 1873 },
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
					}, "crafting_table"){ UniqueId = 1871 },
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
					}, "crafting_table"){ UniqueId = 1869 },
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
					}, "crafting_table"){ UniqueId = 1867 },
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
					}, "crafting_table"){ UniqueId = 1865 },
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
					}, "crafting_table"){ UniqueId = 1863 },
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
					}, "crafting_table"){ UniqueId = 1899 },
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
					}, "crafting_table"){ UniqueId = 1897 },
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
					}, "crafting_table"){ UniqueId = 1895 },
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
					}, "crafting_table"){ UniqueId = 1893 },
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
					}, "crafting_table"){ UniqueId = 1891 },
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
					}, "crafting_table"){ UniqueId = 1889 },
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
					}, "crafting_table"){ UniqueId = 1887 },
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
					}, "crafting_table"){ UniqueId = 1885 },
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
					}, "crafting_table"){ UniqueId = 1883 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(402, 0, 1),
					},
					new List<Item>
					{
						new Item(289, 32767, 1),
						new Item(351, 0, 1),
					}, "crafting_table"){ UniqueId = 1900 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(402, 10, 1),
					},
					new List<Item>
					{
						new Item(289, 32767, 1),
						new Item(351, 10, 1),
					}, "crafting_table"){ UniqueId = 1880 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(402, 11, 1),
					},
					new List<Item>
					{
						new Item(289, 32767, 1),
						new Item(351, 11, 1),
					}, "crafting_table"){ UniqueId = 1878 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(402, 12, 1),
					},
					new List<Item>
					{
						new Item(289, 32767, 1),
						new Item(351, 12, 1),
					}, "crafting_table"){ UniqueId = 1876 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(402, 13, 1),
					},
					new List<Item>
					{
						new Item(289, 32767, 1),
						new Item(351, 13, 1),
					}, "crafting_table"){ UniqueId = 1874 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(402, 14, 1),
					},
					new List<Item>
					{
						new Item(289, 32767, 1),
						new Item(351, 14, 1),
					}, "crafting_table"){ UniqueId = 1872 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(402, 15, 1),
					},
					new List<Item>
					{
						new Item(289, 32767, 1),
						new Item(351, 15, 1),
					}, "crafting_table"){ UniqueId = 1870 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(402, 0, 1),
					},
					new List<Item>
					{
						new Item(289, 32767, 1),
						new Item(351, 16, 1),
					}, "crafting_table"){ UniqueId = 1868 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(402, 3, 1),
					},
					new List<Item>
					{
						new Item(289, 32767, 1),
						new Item(351, 17, 1),
					}, "crafting_table"){ UniqueId = 1866 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(402, 4, 1),
					},
					new List<Item>
					{
						new Item(289, 32767, 1),
						new Item(351, 18, 1),
					}, "crafting_table"){ UniqueId = 1864 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(402, 15, 1),
					},
					new List<Item>
					{
						new Item(289, 32767, 1),
						new Item(351, 19, 1),
					}, "crafting_table"){ UniqueId = 1862 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(402, 1, 1),
					},
					new List<Item>
					{
						new Item(289, 32767, 1),
						new Item(351, 1, 1),
					}, "crafting_table"){ UniqueId = 1898 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(402, 2, 1),
					},
					new List<Item>
					{
						new Item(289, 32767, 1),
						new Item(351, 2, 1),
					}, "crafting_table"){ UniqueId = 1896 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(402, 3, 1),
					},
					new List<Item>
					{
						new Item(289, 32767, 1),
						new Item(351, 3, 1),
					}, "crafting_table"){ UniqueId = 1894 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(402, 4, 1),
					},
					new List<Item>
					{
						new Item(289, 32767, 1),
						new Item(351, 4, 1),
					}, "crafting_table"){ UniqueId = 1892 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(402, 5, 1),
					},
					new List<Item>
					{
						new Item(289, 32767, 1),
						new Item(351, 5, 1),
					}, "crafting_table"){ UniqueId = 1890 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(402, 6, 1),
					},
					new List<Item>
					{
						new Item(289, 32767, 1),
						new Item(351, 6, 1),
					}, "crafting_table"){ UniqueId = 1888 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(402, 7, 1),
					},
					new List<Item>
					{
						new Item(289, 32767, 1),
						new Item(351, 7, 1),
					}, "crafting_table"){ UniqueId = 1886 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(402, 8, 1),
					},
					new List<Item>
					{
						new Item(289, 32767, 1),
						new Item(351, 8, 1),
					}, "crafting_table"){ UniqueId = 1884 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(402, 9, 1),
					},
					new List<Item>
					{
						new Item(289, 32767, 1),
						new Item(351, 9, 1),
					}, "crafting_table"){ UniqueId = 1882 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(401, 0, 3),
					},
					new List<Item>
					{
						new Item(339, 32767, 1),
						new Item(289, 32767, 1),
					}, "crafting_table"){ UniqueId = 1861 },
				new MultiRecipe() { Id = new UUID("00000000-0000-0000-0000-000000000001"), UniqueId = 1956 }, // 00000000-0000-0000-0000-000000000001
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 15, 1),
					},
					new List<Item>
					{
						new Item(218, 5, 1),
						new Item(351, 0, 1),
					}, "crafting_table"){ UniqueId = 1433 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 15, 1),
					},
					new List<Item>
					{
						new Item(218, 4, 1),
						new Item(351, 0, 1),
					}, "crafting_table"){ UniqueId = 1434 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 15, 1),
					},
					new List<Item>
					{
						new Item(218, 3, 1),
						new Item(351, 0, 1),
					}, "crafting_table"){ UniqueId = 1435 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 15, 1),
					},
					new List<Item>
					{
						new Item(218, 2, 1),
						new Item(351, 0, 1),
					}, "crafting_table"){ UniqueId = 1436 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 15, 1),
					},
					new List<Item>
					{
						new Item(218, 1, 1),
						new Item(351, 0, 1),
					}, "crafting_table"){ UniqueId = 1437 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 15, 1),
					},
					new List<Item>
					{
						new Item(218, 0, 1),
						new Item(351, 0, 1),
					}, "crafting_table"){ UniqueId = 1438 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 15, 1),
					},
					new List<Item>
					{
						new Item(218, 14, 1),
						new Item(351, 0, 1),
					}, "crafting_table"){ UniqueId = 1424 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 15, 1),
					},
					new List<Item>
					{
						new Item(218, 13, 1),
						new Item(351, 0, 1),
					}, "crafting_table"){ UniqueId = 1425 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 15, 1),
					},
					new List<Item>
					{
						new Item(218, 12, 1),
						new Item(351, 0, 1),
					}, "crafting_table"){ UniqueId = 1426 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 15, 1),
					},
					new List<Item>
					{
						new Item(218, 11, 1),
						new Item(351, 0, 1),
					}, "crafting_table"){ UniqueId = 1427 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 15, 1),
					},
					new List<Item>
					{
						new Item(218, 10, 1),
						new Item(351, 0, 1),
					}, "crafting_table"){ UniqueId = 1428 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 15, 1),
					},
					new List<Item>
					{
						new Item(218, 9, 1),
						new Item(351, 0, 1),
					}, "crafting_table"){ UniqueId = 1429 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 15, 1),
					},
					new List<Item>
					{
						new Item(218, 8, 1),
						new Item(351, 0, 1),
					}, "crafting_table"){ UniqueId = 1430 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 15, 1),
					},
					new List<Item>
					{
						new Item(218, 7, 1),
						new Item(351, 0, 1),
					}, "crafting_table"){ UniqueId = 1431 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 15, 1),
					},
					new List<Item>
					{
						new Item(218, 6, 1),
						new Item(351, 0, 1),
					}, "crafting_table"){ UniqueId = 1432 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 5, 1),
					},
					new List<Item>
					{
						new Item(218, 15, 1),
						new Item(351, 10, 1),
					}, "crafting_table"){ UniqueId = 1264 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 5, 1),
					},
					new List<Item>
					{
						new Item(218, 4, 1),
						new Item(351, 10, 1),
					}, "crafting_table"){ UniqueId = 1274 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 5, 1),
					},
					new List<Item>
					{
						new Item(218, 3, 1),
						new Item(351, 10, 1),
					}, "crafting_table"){ UniqueId = 1275 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 5, 1),
					},
					new List<Item>
					{
						new Item(218, 2, 1),
						new Item(351, 10, 1),
					}, "crafting_table"){ UniqueId = 1276 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 5, 1),
					},
					new List<Item>
					{
						new Item(218, 1, 1),
						new Item(351, 10, 1),
					}, "crafting_table"){ UniqueId = 1277 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 5, 1),
					},
					new List<Item>
					{
						new Item(218, 0, 1),
						new Item(351, 10, 1),
					}, "crafting_table"){ UniqueId = 1278 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 5, 1),
					},
					new List<Item>
					{
						new Item(218, 14, 1),
						new Item(351, 10, 1),
					}, "crafting_table"){ UniqueId = 1265 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 5, 1),
					},
					new List<Item>
					{
						new Item(218, 13, 1),
						new Item(351, 10, 1),
					}, "crafting_table"){ UniqueId = 1266 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 5, 1),
					},
					new List<Item>
					{
						new Item(218, 12, 1),
						new Item(351, 10, 1),
					}, "crafting_table"){ UniqueId = 1267 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 5, 1),
					},
					new List<Item>
					{
						new Item(218, 11, 1),
						new Item(351, 10, 1),
					}, "crafting_table"){ UniqueId = 1268 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 5, 1),
					},
					new List<Item>
					{
						new Item(218, 10, 1),
						new Item(351, 10, 1),
					}, "crafting_table"){ UniqueId = 1269 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 5, 1),
					},
					new List<Item>
					{
						new Item(218, 9, 1),
						new Item(351, 10, 1),
					}, "crafting_table"){ UniqueId = 1270 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 5, 1),
					},
					new List<Item>
					{
						new Item(218, 8, 1),
						new Item(351, 10, 1),
					}, "crafting_table"){ UniqueId = 1271 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 5, 1),
					},
					new List<Item>
					{
						new Item(218, 7, 1),
						new Item(351, 10, 1),
					}, "crafting_table"){ UniqueId = 1272 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 5, 1),
					},
					new List<Item>
					{
						new Item(218, 6, 1),
						new Item(351, 10, 1),
					}, "crafting_table"){ UniqueId = 1273 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 4, 1),
					},
					new List<Item>
					{
						new Item(218, 15, 1),
						new Item(351, 11, 1),
					}, "crafting_table"){ UniqueId = 1248 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 4, 1),
					},
					new List<Item>
					{
						new Item(218, 5, 1),
						new Item(351, 11, 1),
					}, "crafting_table"){ UniqueId = 1258 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 4, 1),
					},
					new List<Item>
					{
						new Item(218, 3, 1),
						new Item(351, 11, 1),
					}, "crafting_table"){ UniqueId = 1259 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 4, 1),
					},
					new List<Item>
					{
						new Item(218, 2, 1),
						new Item(351, 11, 1),
					}, "crafting_table"){ UniqueId = 1260 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 4, 1),
					},
					new List<Item>
					{
						new Item(218, 1, 1),
						new Item(351, 11, 1),
					}, "crafting_table"){ UniqueId = 1261 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 4, 1),
					},
					new List<Item>
					{
						new Item(218, 0, 1),
						new Item(351, 11, 1),
					}, "crafting_table"){ UniqueId = 1262 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 4, 1),
					},
					new List<Item>
					{
						new Item(218, 14, 1),
						new Item(351, 11, 1),
					}, "crafting_table"){ UniqueId = 1249 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 4, 1),
					},
					new List<Item>
					{
						new Item(218, 13, 1),
						new Item(351, 11, 1),
					}, "crafting_table"){ UniqueId = 1250 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 4, 1),
					},
					new List<Item>
					{
						new Item(218, 12, 1),
						new Item(351, 11, 1),
					}, "crafting_table"){ UniqueId = 1251 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 4, 1),
					},
					new List<Item>
					{
						new Item(218, 11, 1),
						new Item(351, 11, 1),
					}, "crafting_table"){ UniqueId = 1252 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 4, 1),
					},
					new List<Item>
					{
						new Item(218, 10, 1),
						new Item(351, 11, 1),
					}, "crafting_table"){ UniqueId = 1253 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 4, 1),
					},
					new List<Item>
					{
						new Item(218, 9, 1),
						new Item(351, 11, 1),
					}, "crafting_table"){ UniqueId = 1254 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 4, 1),
					},
					new List<Item>
					{
						new Item(218, 8, 1),
						new Item(351, 11, 1),
					}, "crafting_table"){ UniqueId = 1255 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 4, 1),
					},
					new List<Item>
					{
						new Item(218, 7, 1),
						new Item(351, 11, 1),
					}, "crafting_table"){ UniqueId = 1256 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 4, 1),
					},
					new List<Item>
					{
						new Item(218, 6, 1),
						new Item(351, 11, 1),
					}, "crafting_table"){ UniqueId = 1257 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 3, 1),
					},
					new List<Item>
					{
						new Item(218, 15, 1),
						new Item(351, 12, 1),
					}, "crafting_table"){ UniqueId = 1232 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 3, 1),
					},
					new List<Item>
					{
						new Item(218, 5, 1),
						new Item(351, 12, 1),
					}, "crafting_table"){ UniqueId = 1242 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 3, 1),
					},
					new List<Item>
					{
						new Item(218, 4, 1),
						new Item(351, 12, 1),
					}, "crafting_table"){ UniqueId = 1243 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 3, 1),
					},
					new List<Item>
					{
						new Item(218, 2, 1),
						new Item(351, 12, 1),
					}, "crafting_table"){ UniqueId = 1244 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 3, 1),
					},
					new List<Item>
					{
						new Item(218, 1, 1),
						new Item(351, 12, 1),
					}, "crafting_table"){ UniqueId = 1245 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 3, 1),
					},
					new List<Item>
					{
						new Item(218, 0, 1),
						new Item(351, 12, 1),
					}, "crafting_table"){ UniqueId = 1246 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 3, 1),
					},
					new List<Item>
					{
						new Item(218, 14, 1),
						new Item(351, 12, 1),
					}, "crafting_table"){ UniqueId = 1233 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 3, 1),
					},
					new List<Item>
					{
						new Item(218, 13, 1),
						new Item(351, 12, 1),
					}, "crafting_table"){ UniqueId = 1234 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 3, 1),
					},
					new List<Item>
					{
						new Item(218, 12, 1),
						new Item(351, 12, 1),
					}, "crafting_table"){ UniqueId = 1235 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 3, 1),
					},
					new List<Item>
					{
						new Item(218, 11, 1),
						new Item(351, 12, 1),
					}, "crafting_table"){ UniqueId = 1236 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 3, 1),
					},
					new List<Item>
					{
						new Item(218, 10, 1),
						new Item(351, 12, 1),
					}, "crafting_table"){ UniqueId = 1237 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 3, 1),
					},
					new List<Item>
					{
						new Item(218, 9, 1),
						new Item(351, 12, 1),
					}, "crafting_table"){ UniqueId = 1238 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 3, 1),
					},
					new List<Item>
					{
						new Item(218, 8, 1),
						new Item(351, 12, 1),
					}, "crafting_table"){ UniqueId = 1239 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 3, 1),
					},
					new List<Item>
					{
						new Item(218, 7, 1),
						new Item(351, 12, 1),
					}, "crafting_table"){ UniqueId = 1240 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 3, 1),
					},
					new List<Item>
					{
						new Item(218, 6, 1),
						new Item(351, 12, 1),
					}, "crafting_table"){ UniqueId = 1241 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 2, 1),
					},
					new List<Item>
					{
						new Item(218, 15, 1),
						new Item(351, 13, 1),
					}, "crafting_table"){ UniqueId = 1216 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 2, 1),
					},
					new List<Item>
					{
						new Item(218, 5, 1),
						new Item(351, 13, 1),
					}, "crafting_table"){ UniqueId = 1226 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 2, 1),
					},
					new List<Item>
					{
						new Item(218, 4, 1),
						new Item(351, 13, 1),
					}, "crafting_table"){ UniqueId = 1227 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 2, 1),
					},
					new List<Item>
					{
						new Item(218, 3, 1),
						new Item(351, 13, 1),
					}, "crafting_table"){ UniqueId = 1228 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 2, 1),
					},
					new List<Item>
					{
						new Item(218, 1, 1),
						new Item(351, 13, 1),
					}, "crafting_table"){ UniqueId = 1229 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 2, 1),
					},
					new List<Item>
					{
						new Item(218, 0, 1),
						new Item(351, 13, 1),
					}, "crafting_table"){ UniqueId = 1230 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 2, 1),
					},
					new List<Item>
					{
						new Item(218, 14, 1),
						new Item(351, 13, 1),
					}, "crafting_table"){ UniqueId = 1217 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 2, 1),
					},
					new List<Item>
					{
						new Item(218, 13, 1),
						new Item(351, 13, 1),
					}, "crafting_table"){ UniqueId = 1218 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 2, 1),
					},
					new List<Item>
					{
						new Item(218, 12, 1),
						new Item(351, 13, 1),
					}, "crafting_table"){ UniqueId = 1219 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 2, 1),
					},
					new List<Item>
					{
						new Item(218, 11, 1),
						new Item(351, 13, 1),
					}, "crafting_table"){ UniqueId = 1220 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 2, 1),
					},
					new List<Item>
					{
						new Item(218, 10, 1),
						new Item(351, 13, 1),
					}, "crafting_table"){ UniqueId = 1221 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 2, 1),
					},
					new List<Item>
					{
						new Item(218, 9, 1),
						new Item(351, 13, 1),
					}, "crafting_table"){ UniqueId = 1222 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 2, 1),
					},
					new List<Item>
					{
						new Item(218, 8, 1),
						new Item(351, 13, 1),
					}, "crafting_table"){ UniqueId = 1223 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 2, 1),
					},
					new List<Item>
					{
						new Item(218, 7, 1),
						new Item(351, 13, 1),
					}, "crafting_table"){ UniqueId = 1224 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 2, 1),
					},
					new List<Item>
					{
						new Item(218, 6, 1),
						new Item(351, 13, 1),
					}, "crafting_table"){ UniqueId = 1225 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 1, 1),
					},
					new List<Item>
					{
						new Item(218, 15, 1),
						new Item(351, 14, 1),
					}, "crafting_table"){ UniqueId = 1200 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 1, 1),
					},
					new List<Item>
					{
						new Item(218, 5, 1),
						new Item(351, 14, 1),
					}, "crafting_table"){ UniqueId = 1210 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 1, 1),
					},
					new List<Item>
					{
						new Item(218, 4, 1),
						new Item(351, 14, 1),
					}, "crafting_table"){ UniqueId = 1211 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 1, 1),
					},
					new List<Item>
					{
						new Item(218, 3, 1),
						new Item(351, 14, 1),
					}, "crafting_table"){ UniqueId = 1212 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 1, 1),
					},
					new List<Item>
					{
						new Item(218, 2, 1),
						new Item(351, 14, 1),
					}, "crafting_table"){ UniqueId = 1213 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 1, 1),
					},
					new List<Item>
					{
						new Item(218, 0, 1),
						new Item(351, 14, 1),
					}, "crafting_table"){ UniqueId = 1214 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 1, 1),
					},
					new List<Item>
					{
						new Item(218, 14, 1),
						new Item(351, 14, 1),
					}, "crafting_table"){ UniqueId = 1201 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 1, 1),
					},
					new List<Item>
					{
						new Item(218, 13, 1),
						new Item(351, 14, 1),
					}, "crafting_table"){ UniqueId = 1202 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 1, 1),
					},
					new List<Item>
					{
						new Item(218, 12, 1),
						new Item(351, 14, 1),
					}, "crafting_table"){ UniqueId = 1203 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 1, 1),
					},
					new List<Item>
					{
						new Item(218, 11, 1),
						new Item(351, 14, 1),
					}, "crafting_table"){ UniqueId = 1204 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 1, 1),
					},
					new List<Item>
					{
						new Item(218, 10, 1),
						new Item(351, 14, 1),
					}, "crafting_table"){ UniqueId = 1205 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 1, 1),
					},
					new List<Item>
					{
						new Item(218, 9, 1),
						new Item(351, 14, 1),
					}, "crafting_table"){ UniqueId = 1206 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 1, 1),
					},
					new List<Item>
					{
						new Item(218, 8, 1),
						new Item(351, 14, 1),
					}, "crafting_table"){ UniqueId = 1207 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 1, 1),
					},
					new List<Item>
					{
						new Item(218, 7, 1),
						new Item(351, 14, 1),
					}, "crafting_table"){ UniqueId = 1208 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 1, 1),
					},
					new List<Item>
					{
						new Item(218, 6, 1),
						new Item(351, 14, 1),
					}, "crafting_table"){ UniqueId = 1209 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1),
					},
					new List<Item>
					{
						new Item(218, 15, 1),
						new Item(351, 15, 1),
					}, "crafting_table"){ UniqueId = 1184 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1),
					},
					new List<Item>
					{
						new Item(218, 5, 1),
						new Item(351, 15, 1),
					}, "crafting_table"){ UniqueId = 1194 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1),
					},
					new List<Item>
					{
						new Item(218, 4, 1),
						new Item(351, 15, 1),
					}, "crafting_table"){ UniqueId = 1195 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1),
					},
					new List<Item>
					{
						new Item(218, 3, 1),
						new Item(351, 15, 1),
					}, "crafting_table"){ UniqueId = 1196 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1),
					},
					new List<Item>
					{
						new Item(218, 2, 1),
						new Item(351, 15, 1),
					}, "crafting_table"){ UniqueId = 1197 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1),
					},
					new List<Item>
					{
						new Item(218, 1, 1),
						new Item(351, 15, 1),
					}, "crafting_table"){ UniqueId = 1198 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1),
					},
					new List<Item>
					{
						new Item(218, 14, 1),
						new Item(351, 15, 1),
					}, "crafting_table"){ UniqueId = 1185 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1),
					},
					new List<Item>
					{
						new Item(218, 13, 1),
						new Item(351, 15, 1),
					}, "crafting_table"){ UniqueId = 1186 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1),
					},
					new List<Item>
					{
						new Item(218, 12, 1),
						new Item(351, 15, 1),
					}, "crafting_table"){ UniqueId = 1187 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1),
					},
					new List<Item>
					{
						new Item(218, 11, 1),
						new Item(351, 15, 1),
					}, "crafting_table"){ UniqueId = 1188 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1),
					},
					new List<Item>
					{
						new Item(218, 10, 1),
						new Item(351, 15, 1),
					}, "crafting_table"){ UniqueId = 1189 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1),
					},
					new List<Item>
					{
						new Item(218, 9, 1),
						new Item(351, 15, 1),
					}, "crafting_table"){ UniqueId = 1190 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1),
					},
					new List<Item>
					{
						new Item(218, 8, 1),
						new Item(351, 15, 1),
					}, "crafting_table"){ UniqueId = 1191 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1),
					},
					new List<Item>
					{
						new Item(218, 7, 1),
						new Item(351, 15, 1),
					}, "crafting_table"){ UniqueId = 1192 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1),
					},
					new List<Item>
					{
						new Item(218, 6, 1),
						new Item(351, 15, 1),
					}, "crafting_table"){ UniqueId = 1193 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 15, 1),
					},
					new List<Item>
					{
						new Item(218, 5, 1),
						new Item(351, 16, 1),
					}, "crafting_table"){ UniqueId = 1177 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 15, 1),
					},
					new List<Item>
					{
						new Item(218, 4, 1),
						new Item(351, 16, 1),
					}, "crafting_table"){ UniqueId = 1178 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 15, 1),
					},
					new List<Item>
					{
						new Item(218, 3, 1),
						new Item(351, 16, 1),
					}, "crafting_table"){ UniqueId = 1179 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 15, 1),
					},
					new List<Item>
					{
						new Item(218, 2, 1),
						new Item(351, 16, 1),
					}, "crafting_table"){ UniqueId = 1180 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 15, 1),
					},
					new List<Item>
					{
						new Item(218, 1, 1),
						new Item(351, 16, 1),
					}, "crafting_table"){ UniqueId = 1181 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 15, 1),
					},
					new List<Item>
					{
						new Item(218, 0, 1),
						new Item(351, 16, 1),
					}, "crafting_table"){ UniqueId = 1182 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 15, 1),
					},
					new List<Item>
					{
						new Item(218, 14, 1),
						new Item(351, 16, 1),
					}, "crafting_table"){ UniqueId = 1168 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 15, 1),
					},
					new List<Item>
					{
						new Item(218, 13, 1),
						new Item(351, 16, 1),
					}, "crafting_table"){ UniqueId = 1169 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 15, 1),
					},
					new List<Item>
					{
						new Item(218, 12, 1),
						new Item(351, 16, 1),
					}, "crafting_table"){ UniqueId = 1170 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 15, 1),
					},
					new List<Item>
					{
						new Item(218, 11, 1),
						new Item(351, 16, 1),
					}, "crafting_table"){ UniqueId = 1171 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 15, 1),
					},
					new List<Item>
					{
						new Item(218, 10, 1),
						new Item(351, 16, 1),
					}, "crafting_table"){ UniqueId = 1172 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 15, 1),
					},
					new List<Item>
					{
						new Item(218, 9, 1),
						new Item(351, 16, 1),
					}, "crafting_table"){ UniqueId = 1173 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 15, 1),
					},
					new List<Item>
					{
						new Item(218, 8, 1),
						new Item(351, 16, 1),
					}, "crafting_table"){ UniqueId = 1174 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 15, 1),
					},
					new List<Item>
					{
						new Item(218, 7, 1),
						new Item(351, 16, 1),
					}, "crafting_table"){ UniqueId = 1175 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 15, 1),
					},
					new List<Item>
					{
						new Item(218, 6, 1),
						new Item(351, 16, 1),
					}, "crafting_table"){ UniqueId = 1176 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 12, 1),
					},
					new List<Item>
					{
						new Item(218, 15, 1),
						new Item(351, 17, 1),
					}, "crafting_table"){ UniqueId = 1152 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 12, 1),
					},
					new List<Item>
					{
						new Item(218, 5, 1),
						new Item(351, 17, 1),
					}, "crafting_table"){ UniqueId = 1161 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 12, 1),
					},
					new List<Item>
					{
						new Item(218, 4, 1),
						new Item(351, 17, 1),
					}, "crafting_table"){ UniqueId = 1162 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 12, 1),
					},
					new List<Item>
					{
						new Item(218, 3, 1),
						new Item(351, 17, 1),
					}, "crafting_table"){ UniqueId = 1163 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 12, 1),
					},
					new List<Item>
					{
						new Item(218, 2, 1),
						new Item(351, 17, 1),
					}, "crafting_table"){ UniqueId = 1164 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 12, 1),
					},
					new List<Item>
					{
						new Item(218, 1, 1),
						new Item(351, 17, 1),
					}, "crafting_table"){ UniqueId = 1165 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 12, 1),
					},
					new List<Item>
					{
						new Item(218, 0, 1),
						new Item(351, 17, 1),
					}, "crafting_table"){ UniqueId = 1166 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 12, 1),
					},
					new List<Item>
					{
						new Item(218, 14, 1),
						new Item(351, 17, 1),
					}, "crafting_table"){ UniqueId = 1153 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 12, 1),
					},
					new List<Item>
					{
						new Item(218, 13, 1),
						new Item(351, 17, 1),
					}, "crafting_table"){ UniqueId = 1154 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 12, 1),
					},
					new List<Item>
					{
						new Item(218, 11, 1),
						new Item(351, 17, 1),
					}, "crafting_table"){ UniqueId = 1155 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 12, 1),
					},
					new List<Item>
					{
						new Item(218, 10, 1),
						new Item(351, 17, 1),
					}, "crafting_table"){ UniqueId = 1156 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 12, 1),
					},
					new List<Item>
					{
						new Item(218, 9, 1),
						new Item(351, 17, 1),
					}, "crafting_table"){ UniqueId = 1157 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 12, 1),
					},
					new List<Item>
					{
						new Item(218, 8, 1),
						new Item(351, 17, 1),
					}, "crafting_table"){ UniqueId = 1158 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 12, 1),
					},
					new List<Item>
					{
						new Item(218, 7, 1),
						new Item(351, 17, 1),
					}, "crafting_table"){ UniqueId = 1159 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 12, 1),
					},
					new List<Item>
					{
						new Item(218, 6, 1),
						new Item(351, 17, 1),
					}, "crafting_table"){ UniqueId = 1160 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 11, 1),
					},
					new List<Item>
					{
						new Item(218, 15, 1),
						new Item(351, 18, 1),
					}, "crafting_table"){ UniqueId = 1136 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 11, 1),
					},
					new List<Item>
					{
						new Item(218, 5, 1),
						new Item(351, 18, 1),
					}, "crafting_table"){ UniqueId = 1145 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 11, 1),
					},
					new List<Item>
					{
						new Item(218, 4, 1),
						new Item(351, 18, 1),
					}, "crafting_table"){ UniqueId = 1146 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 11, 1),
					},
					new List<Item>
					{
						new Item(218, 3, 1),
						new Item(351, 18, 1),
					}, "crafting_table"){ UniqueId = 1147 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 11, 1),
					},
					new List<Item>
					{
						new Item(218, 2, 1),
						new Item(351, 18, 1),
					}, "crafting_table"){ UniqueId = 1148 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 11, 1),
					},
					new List<Item>
					{
						new Item(218, 1, 1),
						new Item(351, 18, 1),
					}, "crafting_table"){ UniqueId = 1149 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 11, 1),
					},
					new List<Item>
					{
						new Item(218, 0, 1),
						new Item(351, 18, 1),
					}, "crafting_table"){ UniqueId = 1150 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 11, 1),
					},
					new List<Item>
					{
						new Item(218, 14, 1),
						new Item(351, 18, 1),
					}, "crafting_table"){ UniqueId = 1137 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 11, 1),
					},
					new List<Item>
					{
						new Item(218, 13, 1),
						new Item(351, 18, 1),
					}, "crafting_table"){ UniqueId = 1138 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 11, 1),
					},
					new List<Item>
					{
						new Item(218, 12, 1),
						new Item(351, 18, 1),
					}, "crafting_table"){ UniqueId = 1139 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 11, 1),
					},
					new List<Item>
					{
						new Item(218, 10, 1),
						new Item(351, 18, 1),
					}, "crafting_table"){ UniqueId = 1140 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 11, 1),
					},
					new List<Item>
					{
						new Item(218, 9, 1),
						new Item(351, 18, 1),
					}, "crafting_table"){ UniqueId = 1141 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 11, 1),
					},
					new List<Item>
					{
						new Item(218, 8, 1),
						new Item(351, 18, 1),
					}, "crafting_table"){ UniqueId = 1142 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 11, 1),
					},
					new List<Item>
					{
						new Item(218, 7, 1),
						new Item(351, 18, 1),
					}, "crafting_table"){ UniqueId = 1143 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 11, 1),
					},
					new List<Item>
					{
						new Item(218, 6, 1),
						new Item(351, 18, 1),
					}, "crafting_table"){ UniqueId = 1144 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1),
					},
					new List<Item>
					{
						new Item(218, 15, 1),
						new Item(351, 19, 1),
					}, "crafting_table"){ UniqueId = 1120 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1),
					},
					new List<Item>
					{
						new Item(218, 5, 1),
						new Item(351, 19, 1),
					}, "crafting_table"){ UniqueId = 1130 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1),
					},
					new List<Item>
					{
						new Item(218, 4, 1),
						new Item(351, 19, 1),
					}, "crafting_table"){ UniqueId = 1131 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1),
					},
					new List<Item>
					{
						new Item(218, 3, 1),
						new Item(351, 19, 1),
					}, "crafting_table"){ UniqueId = 1132 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1),
					},
					new List<Item>
					{
						new Item(218, 2, 1),
						new Item(351, 19, 1),
					}, "crafting_table"){ UniqueId = 1133 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1),
					},
					new List<Item>
					{
						new Item(218, 1, 1),
						new Item(351, 19, 1),
					}, "crafting_table"){ UniqueId = 1134 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1),
					},
					new List<Item>
					{
						new Item(218, 14, 1),
						new Item(351, 19, 1),
					}, "crafting_table"){ UniqueId = 1121 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1),
					},
					new List<Item>
					{
						new Item(218, 13, 1),
						new Item(351, 19, 1),
					}, "crafting_table"){ UniqueId = 1122 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1),
					},
					new List<Item>
					{
						new Item(218, 12, 1),
						new Item(351, 19, 1),
					}, "crafting_table"){ UniqueId = 1123 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1),
					},
					new List<Item>
					{
						new Item(218, 11, 1),
						new Item(351, 19, 1),
					}, "crafting_table"){ UniqueId = 1124 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1),
					},
					new List<Item>
					{
						new Item(218, 10, 1),
						new Item(351, 19, 1),
					}, "crafting_table"){ UniqueId = 1125 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1),
					},
					new List<Item>
					{
						new Item(218, 9, 1),
						new Item(351, 19, 1),
					}, "crafting_table"){ UniqueId = 1126 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1),
					},
					new List<Item>
					{
						new Item(218, 8, 1),
						new Item(351, 19, 1),
					}, "crafting_table"){ UniqueId = 1127 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1),
					},
					new List<Item>
					{
						new Item(218, 7, 1),
						new Item(351, 19, 1),
					}, "crafting_table"){ UniqueId = 1128 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1),
					},
					new List<Item>
					{
						new Item(218, 6, 1),
						new Item(351, 19, 1),
					}, "crafting_table"){ UniqueId = 1129 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 14, 1),
					},
					new List<Item>
					{
						new Item(218, 15, 1),
						new Item(351, 1, 1),
					}, "crafting_table"){ UniqueId = 1408 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 14, 1),
					},
					new List<Item>
					{
						new Item(218, 5, 1),
						new Item(351, 1, 1),
					}, "crafting_table"){ UniqueId = 1417 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 14, 1),
					},
					new List<Item>
					{
						new Item(218, 4, 1),
						new Item(351, 1, 1),
					}, "crafting_table"){ UniqueId = 1418 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 14, 1),
					},
					new List<Item>
					{
						new Item(218, 3, 1),
						new Item(351, 1, 1),
					}, "crafting_table"){ UniqueId = 1419 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 14, 1),
					},
					new List<Item>
					{
						new Item(218, 2, 1),
						new Item(351, 1, 1),
					}, "crafting_table"){ UniqueId = 1420 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 14, 1),
					},
					new List<Item>
					{
						new Item(218, 1, 1),
						new Item(351, 1, 1),
					}, "crafting_table"){ UniqueId = 1421 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 14, 1),
					},
					new List<Item>
					{
						new Item(218, 0, 1),
						new Item(351, 1, 1),
					}, "crafting_table"){ UniqueId = 1422 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 14, 1),
					},
					new List<Item>
					{
						new Item(218, 13, 1),
						new Item(351, 1, 1),
					}, "crafting_table"){ UniqueId = 1409 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 14, 1),
					},
					new List<Item>
					{
						new Item(218, 12, 1),
						new Item(351, 1, 1),
					}, "crafting_table"){ UniqueId = 1410 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 14, 1),
					},
					new List<Item>
					{
						new Item(218, 11, 1),
						new Item(351, 1, 1),
					}, "crafting_table"){ UniqueId = 1411 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 14, 1),
					},
					new List<Item>
					{
						new Item(218, 10, 1),
						new Item(351, 1, 1),
					}, "crafting_table"){ UniqueId = 1412 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 14, 1),
					},
					new List<Item>
					{
						new Item(218, 9, 1),
						new Item(351, 1, 1),
					}, "crafting_table"){ UniqueId = 1413 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 14, 1),
					},
					new List<Item>
					{
						new Item(218, 8, 1),
						new Item(351, 1, 1),
					}, "crafting_table"){ UniqueId = 1414 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 14, 1),
					},
					new List<Item>
					{
						new Item(218, 7, 1),
						new Item(351, 1, 1),
					}, "crafting_table"){ UniqueId = 1415 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 14, 1),
					},
					new List<Item>
					{
						new Item(218, 6, 1),
						new Item(351, 1, 1),
					}, "crafting_table"){ UniqueId = 1416 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 13, 1),
					},
					new List<Item>
					{
						new Item(218, 15, 1),
						new Item(351, 2, 1),
					}, "crafting_table"){ UniqueId = 1392 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 13, 1),
					},
					new List<Item>
					{
						new Item(218, 5, 1),
						new Item(351, 2, 1),
					}, "crafting_table"){ UniqueId = 1401 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 13, 1),
					},
					new List<Item>
					{
						new Item(218, 4, 1),
						new Item(351, 2, 1),
					}, "crafting_table"){ UniqueId = 1402 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 13, 1),
					},
					new List<Item>
					{
						new Item(218, 3, 1),
						new Item(351, 2, 1),
					}, "crafting_table"){ UniqueId = 1403 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 13, 1),
					},
					new List<Item>
					{
						new Item(218, 2, 1),
						new Item(351, 2, 1),
					}, "crafting_table"){ UniqueId = 1404 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 13, 1),
					},
					new List<Item>
					{
						new Item(218, 1, 1),
						new Item(351, 2, 1),
					}, "crafting_table"){ UniqueId = 1405 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 13, 1),
					},
					new List<Item>
					{
						new Item(218, 0, 1),
						new Item(351, 2, 1),
					}, "crafting_table"){ UniqueId = 1406 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 13, 1),
					},
					new List<Item>
					{
						new Item(218, 14, 1),
						new Item(351, 2, 1),
					}, "crafting_table"){ UniqueId = 1393 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 13, 1),
					},
					new List<Item>
					{
						new Item(218, 12, 1),
						new Item(351, 2, 1),
					}, "crafting_table"){ UniqueId = 1394 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 13, 1),
					},
					new List<Item>
					{
						new Item(218, 11, 1),
						new Item(351, 2, 1),
					}, "crafting_table"){ UniqueId = 1395 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 13, 1),
					},
					new List<Item>
					{
						new Item(218, 10, 1),
						new Item(351, 2, 1),
					}, "crafting_table"){ UniqueId = 1396 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 13, 1),
					},
					new List<Item>
					{
						new Item(218, 9, 1),
						new Item(351, 2, 1),
					}, "crafting_table"){ UniqueId = 1397 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 13, 1),
					},
					new List<Item>
					{
						new Item(218, 8, 1),
						new Item(351, 2, 1),
					}, "crafting_table"){ UniqueId = 1398 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 13, 1),
					},
					new List<Item>
					{
						new Item(218, 7, 1),
						new Item(351, 2, 1),
					}, "crafting_table"){ UniqueId = 1399 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 13, 1),
					},
					new List<Item>
					{
						new Item(218, 6, 1),
						new Item(351, 2, 1),
					}, "crafting_table"){ UniqueId = 1400 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 12, 1),
					},
					new List<Item>
					{
						new Item(218, 15, 1),
						new Item(351, 3, 1),
					}, "crafting_table"){ UniqueId = 1376 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 12, 1),
					},
					new List<Item>
					{
						new Item(218, 5, 1),
						new Item(351, 3, 1),
					}, "crafting_table"){ UniqueId = 1385 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 12, 1),
					},
					new List<Item>
					{
						new Item(218, 4, 1),
						new Item(351, 3, 1),
					}, "crafting_table"){ UniqueId = 1386 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 12, 1),
					},
					new List<Item>
					{
						new Item(218, 3, 1),
						new Item(351, 3, 1),
					}, "crafting_table"){ UniqueId = 1387 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 12, 1),
					},
					new List<Item>
					{
						new Item(218, 2, 1),
						new Item(351, 3, 1),
					}, "crafting_table"){ UniqueId = 1388 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 12, 1),
					},
					new List<Item>
					{
						new Item(218, 1, 1),
						new Item(351, 3, 1),
					}, "crafting_table"){ UniqueId = 1389 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 12, 1),
					},
					new List<Item>
					{
						new Item(218, 0, 1),
						new Item(351, 3, 1),
					}, "crafting_table"){ UniqueId = 1390 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 12, 1),
					},
					new List<Item>
					{
						new Item(218, 14, 1),
						new Item(351, 3, 1),
					}, "crafting_table"){ UniqueId = 1377 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 12, 1),
					},
					new List<Item>
					{
						new Item(218, 13, 1),
						new Item(351, 3, 1),
					}, "crafting_table"){ UniqueId = 1378 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 12, 1),
					},
					new List<Item>
					{
						new Item(218, 11, 1),
						new Item(351, 3, 1),
					}, "crafting_table"){ UniqueId = 1379 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 12, 1),
					},
					new List<Item>
					{
						new Item(218, 10, 1),
						new Item(351, 3, 1),
					}, "crafting_table"){ UniqueId = 1380 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 12, 1),
					},
					new List<Item>
					{
						new Item(218, 9, 1),
						new Item(351, 3, 1),
					}, "crafting_table"){ UniqueId = 1381 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 12, 1),
					},
					new List<Item>
					{
						new Item(218, 8, 1),
						new Item(351, 3, 1),
					}, "crafting_table"){ UniqueId = 1382 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 12, 1),
					},
					new List<Item>
					{
						new Item(218, 7, 1),
						new Item(351, 3, 1),
					}, "crafting_table"){ UniqueId = 1383 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 12, 1),
					},
					new List<Item>
					{
						new Item(218, 6, 1),
						new Item(351, 3, 1),
					}, "crafting_table"){ UniqueId = 1384 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 11, 1),
					},
					new List<Item>
					{
						new Item(218, 15, 1),
						new Item(351, 4, 1),
					}, "crafting_table"){ UniqueId = 1360 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 11, 1),
					},
					new List<Item>
					{
						new Item(218, 5, 1),
						new Item(351, 4, 1),
					}, "crafting_table"){ UniqueId = 1369 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 11, 1),
					},
					new List<Item>
					{
						new Item(218, 4, 1),
						new Item(351, 4, 1),
					}, "crafting_table"){ UniqueId = 1370 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 11, 1),
					},
					new List<Item>
					{
						new Item(218, 3, 1),
						new Item(351, 4, 1),
					}, "crafting_table"){ UniqueId = 1371 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 11, 1),
					},
					new List<Item>
					{
						new Item(218, 2, 1),
						new Item(351, 4, 1),
					}, "crafting_table"){ UniqueId = 1372 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 11, 1),
					},
					new List<Item>
					{
						new Item(218, 1, 1),
						new Item(351, 4, 1),
					}, "crafting_table"){ UniqueId = 1373 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 11, 1),
					},
					new List<Item>
					{
						new Item(218, 0, 1),
						new Item(351, 4, 1),
					}, "crafting_table"){ UniqueId = 1374 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 11, 1),
					},
					new List<Item>
					{
						new Item(218, 14, 1),
						new Item(351, 4, 1),
					}, "crafting_table"){ UniqueId = 1361 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 11, 1),
					},
					new List<Item>
					{
						new Item(218, 13, 1),
						new Item(351, 4, 1),
					}, "crafting_table"){ UniqueId = 1362 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 11, 1),
					},
					new List<Item>
					{
						new Item(218, 12, 1),
						new Item(351, 4, 1),
					}, "crafting_table"){ UniqueId = 1363 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 11, 1),
					},
					new List<Item>
					{
						new Item(218, 10, 1),
						new Item(351, 4, 1),
					}, "crafting_table"){ UniqueId = 1364 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 11, 1),
					},
					new List<Item>
					{
						new Item(218, 9, 1),
						new Item(351, 4, 1),
					}, "crafting_table"){ UniqueId = 1365 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 11, 1),
					},
					new List<Item>
					{
						new Item(218, 8, 1),
						new Item(351, 4, 1),
					}, "crafting_table"){ UniqueId = 1366 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 11, 1),
					},
					new List<Item>
					{
						new Item(218, 7, 1),
						new Item(351, 4, 1),
					}, "crafting_table"){ UniqueId = 1367 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 11, 1),
					},
					new List<Item>
					{
						new Item(218, 6, 1),
						new Item(351, 4, 1),
					}, "crafting_table"){ UniqueId = 1368 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 10, 1),
					},
					new List<Item>
					{
						new Item(218, 15, 1),
						new Item(351, 5, 1),
					}, "crafting_table"){ UniqueId = 1344 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 10, 1),
					},
					new List<Item>
					{
						new Item(218, 5, 1),
						new Item(351, 5, 1),
					}, "crafting_table"){ UniqueId = 1353 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 10, 1),
					},
					new List<Item>
					{
						new Item(218, 4, 1),
						new Item(351, 5, 1),
					}, "crafting_table"){ UniqueId = 1354 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 10, 1),
					},
					new List<Item>
					{
						new Item(218, 3, 1),
						new Item(351, 5, 1),
					}, "crafting_table"){ UniqueId = 1355 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 10, 1),
					},
					new List<Item>
					{
						new Item(218, 2, 1),
						new Item(351, 5, 1),
					}, "crafting_table"){ UniqueId = 1356 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 10, 1),
					},
					new List<Item>
					{
						new Item(218, 1, 1),
						new Item(351, 5, 1),
					}, "crafting_table"){ UniqueId = 1357 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 10, 1),
					},
					new List<Item>
					{
						new Item(218, 0, 1),
						new Item(351, 5, 1),
					}, "crafting_table"){ UniqueId = 1358 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 10, 1),
					},
					new List<Item>
					{
						new Item(218, 14, 1),
						new Item(351, 5, 1),
					}, "crafting_table"){ UniqueId = 1345 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 10, 1),
					},
					new List<Item>
					{
						new Item(218, 13, 1),
						new Item(351, 5, 1),
					}, "crafting_table"){ UniqueId = 1346 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 10, 1),
					},
					new List<Item>
					{
						new Item(218, 12, 1),
						new Item(351, 5, 1),
					}, "crafting_table"){ UniqueId = 1347 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 10, 1),
					},
					new List<Item>
					{
						new Item(218, 11, 1),
						new Item(351, 5, 1),
					}, "crafting_table"){ UniqueId = 1348 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 10, 1),
					},
					new List<Item>
					{
						new Item(218, 9, 1),
						new Item(351, 5, 1),
					}, "crafting_table"){ UniqueId = 1349 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 10, 1),
					},
					new List<Item>
					{
						new Item(218, 8, 1),
						new Item(351, 5, 1),
					}, "crafting_table"){ UniqueId = 1350 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 10, 1),
					},
					new List<Item>
					{
						new Item(218, 7, 1),
						new Item(351, 5, 1),
					}, "crafting_table"){ UniqueId = 1351 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 10, 1),
					},
					new List<Item>
					{
						new Item(218, 6, 1),
						new Item(351, 5, 1),
					}, "crafting_table"){ UniqueId = 1352 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 9, 1),
					},
					new List<Item>
					{
						new Item(218, 15, 1),
						new Item(351, 6, 1),
					}, "crafting_table"){ UniqueId = 1328 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 9, 1),
					},
					new List<Item>
					{
						new Item(218, 5, 1),
						new Item(351, 6, 1),
					}, "crafting_table"){ UniqueId = 1337 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 9, 1),
					},
					new List<Item>
					{
						new Item(218, 4, 1),
						new Item(351, 6, 1),
					}, "crafting_table"){ UniqueId = 1338 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 9, 1),
					},
					new List<Item>
					{
						new Item(218, 3, 1),
						new Item(351, 6, 1),
					}, "crafting_table"){ UniqueId = 1339 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 9, 1),
					},
					new List<Item>
					{
						new Item(218, 2, 1),
						new Item(351, 6, 1),
					}, "crafting_table"){ UniqueId = 1340 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 9, 1),
					},
					new List<Item>
					{
						new Item(218, 1, 1),
						new Item(351, 6, 1),
					}, "crafting_table"){ UniqueId = 1341 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 9, 1),
					},
					new List<Item>
					{
						new Item(218, 0, 1),
						new Item(351, 6, 1),
					}, "crafting_table"){ UniqueId = 1342 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 9, 1),
					},
					new List<Item>
					{
						new Item(218, 14, 1),
						new Item(351, 6, 1),
					}, "crafting_table"){ UniqueId = 1329 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 9, 1),
					},
					new List<Item>
					{
						new Item(218, 13, 1),
						new Item(351, 6, 1),
					}, "crafting_table"){ UniqueId = 1330 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 9, 1),
					},
					new List<Item>
					{
						new Item(218, 12, 1),
						new Item(351, 6, 1),
					}, "crafting_table"){ UniqueId = 1331 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 9, 1),
					},
					new List<Item>
					{
						new Item(218, 11, 1),
						new Item(351, 6, 1),
					}, "crafting_table"){ UniqueId = 1332 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 9, 1),
					},
					new List<Item>
					{
						new Item(218, 10, 1),
						new Item(351, 6, 1),
					}, "crafting_table"){ UniqueId = 1333 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 9, 1),
					},
					new List<Item>
					{
						new Item(218, 8, 1),
						new Item(351, 6, 1),
					}, "crafting_table"){ UniqueId = 1334 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 9, 1),
					},
					new List<Item>
					{
						new Item(218, 7, 1),
						new Item(351, 6, 1),
					}, "crafting_table"){ UniqueId = 1335 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 9, 1),
					},
					new List<Item>
					{
						new Item(218, 6, 1),
						new Item(351, 6, 1),
					}, "crafting_table"){ UniqueId = 1336 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 8, 1),
					},
					new List<Item>
					{
						new Item(218, 15, 1),
						new Item(351, 7, 1),
					}, "crafting_table"){ UniqueId = 1312 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 8, 1),
					},
					new List<Item>
					{
						new Item(218, 5, 1),
						new Item(351, 7, 1),
					}, "crafting_table"){ UniqueId = 1321 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 8, 1),
					},
					new List<Item>
					{
						new Item(218, 4, 1),
						new Item(351, 7, 1),
					}, "crafting_table"){ UniqueId = 1322 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 8, 1),
					},
					new List<Item>
					{
						new Item(218, 3, 1),
						new Item(351, 7, 1),
					}, "crafting_table"){ UniqueId = 1323 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 8, 1),
					},
					new List<Item>
					{
						new Item(218, 2, 1),
						new Item(351, 7, 1),
					}, "crafting_table"){ UniqueId = 1324 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 8, 1),
					},
					new List<Item>
					{
						new Item(218, 1, 1),
						new Item(351, 7, 1),
					}, "crafting_table"){ UniqueId = 1325 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 8, 1),
					},
					new List<Item>
					{
						new Item(218, 0, 1),
						new Item(351, 7, 1),
					}, "crafting_table"){ UniqueId = 1326 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 8, 1),
					},
					new List<Item>
					{
						new Item(218, 14, 1),
						new Item(351, 7, 1),
					}, "crafting_table"){ UniqueId = 1313 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 8, 1),
					},
					new List<Item>
					{
						new Item(218, 13, 1),
						new Item(351, 7, 1),
					}, "crafting_table"){ UniqueId = 1314 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 8, 1),
					},
					new List<Item>
					{
						new Item(218, 12, 1),
						new Item(351, 7, 1),
					}, "crafting_table"){ UniqueId = 1315 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 8, 1),
					},
					new List<Item>
					{
						new Item(218, 11, 1),
						new Item(351, 7, 1),
					}, "crafting_table"){ UniqueId = 1316 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 8, 1),
					},
					new List<Item>
					{
						new Item(218, 10, 1),
						new Item(351, 7, 1),
					}, "crafting_table"){ UniqueId = 1317 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 8, 1),
					},
					new List<Item>
					{
						new Item(218, 9, 1),
						new Item(351, 7, 1),
					}, "crafting_table"){ UniqueId = 1318 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 8, 1),
					},
					new List<Item>
					{
						new Item(218, 7, 1),
						new Item(351, 7, 1),
					}, "crafting_table"){ UniqueId = 1319 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 8, 1),
					},
					new List<Item>
					{
						new Item(218, 6, 1),
						new Item(351, 7, 1),
					}, "crafting_table"){ UniqueId = 1320 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 7, 1),
					},
					new List<Item>
					{
						new Item(218, 15, 1),
						new Item(351, 8, 1),
					}, "crafting_table"){ UniqueId = 1296 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 7, 1),
					},
					new List<Item>
					{
						new Item(218, 5, 1),
						new Item(351, 8, 1),
					}, "crafting_table"){ UniqueId = 1305 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 7, 1),
					},
					new List<Item>
					{
						new Item(218, 4, 1),
						new Item(351, 8, 1),
					}, "crafting_table"){ UniqueId = 1306 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 7, 1),
					},
					new List<Item>
					{
						new Item(218, 3, 1),
						new Item(351, 8, 1),
					}, "crafting_table"){ UniqueId = 1307 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 7, 1),
					},
					new List<Item>
					{
						new Item(218, 2, 1),
						new Item(351, 8, 1),
					}, "crafting_table"){ UniqueId = 1308 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 7, 1),
					},
					new List<Item>
					{
						new Item(218, 1, 1),
						new Item(351, 8, 1),
					}, "crafting_table"){ UniqueId = 1309 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 7, 1),
					},
					new List<Item>
					{
						new Item(218, 0, 1),
						new Item(351, 8, 1),
					}, "crafting_table"){ UniqueId = 1310 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 7, 1),
					},
					new List<Item>
					{
						new Item(218, 14, 1),
						new Item(351, 8, 1),
					}, "crafting_table"){ UniqueId = 1297 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 7, 1),
					},
					new List<Item>
					{
						new Item(218, 13, 1),
						new Item(351, 8, 1),
					}, "crafting_table"){ UniqueId = 1298 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 7, 1),
					},
					new List<Item>
					{
						new Item(218, 12, 1),
						new Item(351, 8, 1),
					}, "crafting_table"){ UniqueId = 1299 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 7, 1),
					},
					new List<Item>
					{
						new Item(218, 11, 1),
						new Item(351, 8, 1),
					}, "crafting_table"){ UniqueId = 1300 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 7, 1),
					},
					new List<Item>
					{
						new Item(218, 10, 1),
						new Item(351, 8, 1),
					}, "crafting_table"){ UniqueId = 1301 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 7, 1),
					},
					new List<Item>
					{
						new Item(218, 9, 1),
						new Item(351, 8, 1),
					}, "crafting_table"){ UniqueId = 1302 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 7, 1),
					},
					new List<Item>
					{
						new Item(218, 8, 1),
						new Item(351, 8, 1),
					}, "crafting_table"){ UniqueId = 1303 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 7, 1),
					},
					new List<Item>
					{
						new Item(218, 6, 1),
						new Item(351, 8, 1),
					}, "crafting_table"){ UniqueId = 1304 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 6, 1),
					},
					new List<Item>
					{
						new Item(218, 15, 1),
						new Item(351, 9, 1),
					}, "crafting_table"){ UniqueId = 1280 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 6, 1),
					},
					new List<Item>
					{
						new Item(218, 5, 1),
						new Item(351, 9, 1),
					}, "crafting_table"){ UniqueId = 1289 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 6, 1),
					},
					new List<Item>
					{
						new Item(218, 4, 1),
						new Item(351, 9, 1),
					}, "crafting_table"){ UniqueId = 1290 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 6, 1),
					},
					new List<Item>
					{
						new Item(218, 3, 1),
						new Item(351, 9, 1),
					}, "crafting_table"){ UniqueId = 1291 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 6, 1),
					},
					new List<Item>
					{
						new Item(218, 2, 1),
						new Item(351, 9, 1),
					}, "crafting_table"){ UniqueId = 1292 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 6, 1),
					},
					new List<Item>
					{
						new Item(218, 1, 1),
						new Item(351, 9, 1),
					}, "crafting_table"){ UniqueId = 1293 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 6, 1),
					},
					new List<Item>
					{
						new Item(218, 0, 1),
						new Item(351, 9, 1),
					}, "crafting_table"){ UniqueId = 1294 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 6, 1),
					},
					new List<Item>
					{
						new Item(218, 14, 1),
						new Item(351, 9, 1),
					}, "crafting_table"){ UniqueId = 1281 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 6, 1),
					},
					new List<Item>
					{
						new Item(218, 13, 1),
						new Item(351, 9, 1),
					}, "crafting_table"){ UniqueId = 1282 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 6, 1),
					},
					new List<Item>
					{
						new Item(218, 12, 1),
						new Item(351, 9, 1),
					}, "crafting_table"){ UniqueId = 1283 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 6, 1),
					},
					new List<Item>
					{
						new Item(218, 11, 1),
						new Item(351, 9, 1),
					}, "crafting_table"){ UniqueId = 1284 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 6, 1),
					},
					new List<Item>
					{
						new Item(218, 10, 1),
						new Item(351, 9, 1),
					}, "crafting_table"){ UniqueId = 1285 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 6, 1),
					},
					new List<Item>
					{
						new Item(218, 9, 1),
						new Item(351, 9, 1),
					}, "crafting_table"){ UniqueId = 1286 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 6, 1),
					},
					new List<Item>
					{
						new Item(218, 8, 1),
						new Item(351, 9, 1),
					}, "crafting_table"){ UniqueId = 1287 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 6, 1),
					},
					new List<Item>
					{
						new Item(218, 7, 1),
						new Item(351, 9, 1),
					}, "crafting_table"){ UniqueId = 1288 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 15, 1),
					},
					new List<Item>
					{
						new Item(205, 0, 1),
						new Item(351, 0, 1),
					}, "crafting_table"){ UniqueId = 1423 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 5, 1),
					},
					new List<Item>
					{
						new Item(205, 0, 1),
						new Item(351, 10, 1),
					}, "crafting_table"){ UniqueId = 1263 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 4, 1),
					},
					new List<Item>
					{
						new Item(205, 0, 1),
						new Item(351, 11, 1),
					}, "crafting_table"){ UniqueId = 1247 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 3, 1),
					},
					new List<Item>
					{
						new Item(205, 0, 1),
						new Item(351, 12, 1),
					}, "crafting_table"){ UniqueId = 1231 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 2, 1),
					},
					new List<Item>
					{
						new Item(205, 0, 1),
						new Item(351, 13, 1),
					}, "crafting_table"){ UniqueId = 1215 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 1, 1),
					},
					new List<Item>
					{
						new Item(205, 0, 1),
						new Item(351, 14, 1),
					}, "crafting_table"){ UniqueId = 1199 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1),
					},
					new List<Item>
					{
						new Item(205, 0, 1),
						new Item(351, 15, 1),
					}, "crafting_table"){ UniqueId = 1183 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 15, 1),
					},
					new List<Item>
					{
						new Item(205, 0, 1),
						new Item(351, 16, 1),
					}, "crafting_table"){ UniqueId = 1167 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 12, 1),
					},
					new List<Item>
					{
						new Item(205, 0, 1),
						new Item(351, 17, 1),
					}, "crafting_table"){ UniqueId = 1151 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 11, 1),
					},
					new List<Item>
					{
						new Item(205, 0, 1),
						new Item(351, 18, 1),
					}, "crafting_table"){ UniqueId = 1135 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 0, 1),
					},
					new List<Item>
					{
						new Item(205, 0, 1),
						new Item(351, 19, 1),
					}, "crafting_table"){ UniqueId = 1119 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 14, 1),
					},
					new List<Item>
					{
						new Item(205, 0, 1),
						new Item(351, 1, 1),
					}, "crafting_table"){ UniqueId = 1407 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 13, 1),
					},
					new List<Item>
					{
						new Item(205, 0, 1),
						new Item(351, 2, 1),
					}, "crafting_table"){ UniqueId = 1391 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 12, 1),
					},
					new List<Item>
					{
						new Item(205, 0, 1),
						new Item(351, 3, 1),
					}, "crafting_table"){ UniqueId = 1375 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 11, 1),
					},
					new List<Item>
					{
						new Item(205, 0, 1),
						new Item(351, 4, 1),
					}, "crafting_table"){ UniqueId = 1359 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 10, 1),
					},
					new List<Item>
					{
						new Item(205, 0, 1),
						new Item(351, 5, 1),
					}, "crafting_table"){ UniqueId = 1343 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 9, 1),
					},
					new List<Item>
					{
						new Item(205, 0, 1),
						new Item(351, 6, 1),
					}, "crafting_table"){ UniqueId = 1327 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 8, 1),
					},
					new List<Item>
					{
						new Item(205, 0, 1),
						new Item(351, 7, 1),
					}, "crafting_table"){ UniqueId = 1311 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 7, 1),
					},
					new List<Item>
					{
						new Item(205, 0, 1),
						new Item(351, 8, 1),
					}, "crafting_table"){ UniqueId = 1295 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(218, 6, 1),
					},
					new List<Item>
					{
						new Item(205, 0, 1),
						new Item(351, 9, 1),
					}, "crafting_table"){ UniqueId = 1279 },
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
					}, "crafting_table"){ UniqueId = 1840 },
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
					}, "crafting_table"){ UniqueId = 1851 },
				new ShapedRecipe(1, 2,
					new List<Item>
					{
						new Item(280, 0),
					},
					new Item[]
					{
						new Item(5, 32767),
						new Item(5, 32767),
					}, "crafting_table"){ UniqueId = 1796 },
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
					}, "crafting_table"){ UniqueId = 1828 },
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
					}, "crafting_table"){ UniqueId = 1836 },
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
					}, "crafting_table"){ UniqueId = 1835 },
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
					}, "crafting_table"){ UniqueId = 1831 },
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
					}, "crafting_table"){ UniqueId = 1837 },
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
					}, "crafting_table"){ UniqueId = 1839 },
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
					}, "crafting_table"){ UniqueId = 1830 },
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
					}, "crafting_table"){ UniqueId = 1838 },
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
					}, "crafting_table"){ UniqueId = 1843 },
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
					}, "crafting_table"){ UniqueId = 1844 },
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
					}, "crafting_table"){ UniqueId = 1846 },
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
					}, "crafting_table"){ UniqueId = 1847 },
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
					}, "crafting_table"){ UniqueId = 1842 },
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
					}, "crafting_table"){ UniqueId = 1845 },
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
					}, "crafting_table"){ UniqueId = 1841 },
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
					}, "crafting_table"){ UniqueId = 1849 },
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
					}, "crafting_table"){ UniqueId = 1848 },
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
					}, "crafting_table"){ UniqueId = 1832 },
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
					}, "crafting_table"){ UniqueId = 1833 },
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
					}, "crafting_table"){ UniqueId = 1834 },
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
					}, "crafting_table"){ UniqueId = 1829 },
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
					}, "crafting_table"){ UniqueId = 1903 },
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
					}, "crafting_table"){ UniqueId = 1904 },
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
					}, "crafting_table"){ UniqueId = 1905 },
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
					}, "crafting_table"){ UniqueId = 1906 },
				new MultiRecipe() { Id = new UUID("aecd2294-4b94-434b-8667-4499bb2c9327"), UniqueId = 1954 }, // aecd2294-4b94-434b-8667-4499bb2c9327
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
					}, "crafting_table"){ UniqueId = 1913 },
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
					}, "crafting_table"){ UniqueId = 1914 },
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
					}, "crafting_table"){ UniqueId = 1915 },
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
					}, "crafting_table"){ UniqueId = 1916 },
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
					}, "crafting_table"){ UniqueId = 1917 },
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
					}, "crafting_table"){ UniqueId = 1918 },
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
					}, "crafting_table"){ UniqueId = 1919 },
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
					}, "crafting_table"){ UniqueId = 1920 },
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
					}, "crafting_table"){ UniqueId = 1921 },
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
					}, "crafting_table"){ UniqueId = 1922 },
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
					}, "crafting_table"){ UniqueId = 1923 },
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
					}, "crafting_table"){ UniqueId = 1924 },
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
					}, "crafting_table"){ UniqueId = 1925 },
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
					}, "crafting_table"){ UniqueId = 1926 },
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
					}, "crafting_table"){ UniqueId = 1927 },
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
					}, "crafting_table"){ UniqueId = 1928 },
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
					}, "crafting_table"){ UniqueId = 1929 },
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
					}, "crafting_table"){ UniqueId = 1930 },
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
					}, "crafting_table"){ UniqueId = 1931 },
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
					}, "crafting_table"){ UniqueId = 1932 },
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
					}, "crafting_table"){ UniqueId = 1933 },
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
					}, "crafting_table"){ UniqueId = 1934 },
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
					}, "crafting_table"){ UniqueId = 1935 },
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
					}, "crafting_table"){ UniqueId = 1936 },
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
					}, "crafting_table"){ UniqueId = 1937 },
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
					}, "crafting_table"){ UniqueId = 1938 },
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
					}, "crafting_table"){ UniqueId = 1939 },
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
					}, "crafting_table"){ UniqueId = 1940 },
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
					}, "crafting_table"){ UniqueId = 1941 },
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
					}, "crafting_table"){ UniqueId = 1942 },
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
					}, "crafting_table"){ UniqueId = 1943 },
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
					}, "crafting_table"){ UniqueId = 1944 },
				new ShapedRecipe(3, 3,
					new List<Item>
					{
						new Item(262, 43),
					},
					new Item[]
					{
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(441, 42),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
						new Item(262, 0),
					}, "crafting_table"){ UniqueId = 1945 },
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
					}, "crafting_table"){ UniqueId = 1908 },
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
					}, "crafting_table"){ UniqueId = 1909 },
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
					}, "crafting_table"){ UniqueId = 1910 },
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
					}, "crafting_table"){ UniqueId = 1911 },
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
					}, "crafting_table"){ UniqueId = 1912 },
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
					}, "crafting_table"){ UniqueId = 1907 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 15, 1),
					},
					new List<Item>
					{
						new Item(351, 0, 1),
						new Item(35, 14, 1),
					}, "crafting_table"){ UniqueId = 1118 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 15, 1),
					},
					new List<Item>
					{
						new Item(351, 0, 1),
						new Item(35, 5, 1),
					}, "crafting_table"){ UniqueId = 1109 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 15, 1),
					},
					new List<Item>
					{
						new Item(351, 0, 1),
						new Item(35, 4, 1),
					}, "crafting_table"){ UniqueId = 1108 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 15, 1),
					},
					new List<Item>
					{
						new Item(351, 0, 1),
						new Item(35, 3, 1),
					}, "crafting_table"){ UniqueId = 1107 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 15, 1),
					},
					new List<Item>
					{
						new Item(351, 0, 1),
						new Item(35, 2, 1),
					}, "crafting_table"){ UniqueId = 1106 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 15, 1),
					},
					new List<Item>
					{
						new Item(351, 0, 1),
						new Item(35, 1, 1),
					}, "crafting_table"){ UniqueId = 1105 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 15, 1),
					},
					new List<Item>
					{
						new Item(351, 0, 1),
						new Item(35, 0, 1),
					}, "crafting_table"){ UniqueId = 1104 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 15, 1),
					},
					new List<Item>
					{
						new Item(351, 0, 1),
						new Item(35, 13, 1),
					}, "crafting_table"){ UniqueId = 1117 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 15, 1),
					},
					new List<Item>
					{
						new Item(351, 0, 1),
						new Item(35, 12, 1),
					}, "crafting_table"){ UniqueId = 1116 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 15, 1),
					},
					new List<Item>
					{
						new Item(351, 0, 1),
						new Item(35, 11, 1),
					}, "crafting_table"){ UniqueId = 1115 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 15, 1),
					},
					new List<Item>
					{
						new Item(351, 0, 1),
						new Item(35, 10, 1),
					}, "crafting_table"){ UniqueId = 1114 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 15, 1),
					},
					new List<Item>
					{
						new Item(351, 0, 1),
						new Item(35, 9, 1),
					}, "crafting_table"){ UniqueId = 1113 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 15, 1),
					},
					new List<Item>
					{
						new Item(351, 0, 1),
						new Item(35, 8, 1),
					}, "crafting_table"){ UniqueId = 1112 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 15, 1),
					},
					new List<Item>
					{
						new Item(351, 0, 1),
						new Item(35, 7, 1),
					}, "crafting_table"){ UniqueId = 1111 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 15, 1),
					},
					new List<Item>
					{
						new Item(351, 0, 1),
						new Item(35, 6, 1),
					}, "crafting_table"){ UniqueId = 1110 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 5, 1),
					},
					new List<Item>
					{
						new Item(351, 10, 1),
						new Item(35, 15, 1),
					}, "crafting_table"){ UniqueId = 968 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 5, 1),
					},
					new List<Item>
					{
						new Item(351, 10, 1),
						new Item(35, 14, 1),
					}, "crafting_table"){ UniqueId = 967 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 5, 1),
					},
					new List<Item>
					{
						new Item(351, 10, 1),
						new Item(35, 4, 1),
					}, "crafting_table"){ UniqueId = 958 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 5, 1),
					},
					new List<Item>
					{
						new Item(351, 10, 1),
						new Item(35, 3, 1),
					}, "crafting_table"){ UniqueId = 957 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 5, 1),
					},
					new List<Item>
					{
						new Item(351, 10, 1),
						new Item(35, 2, 1),
					}, "crafting_table"){ UniqueId = 956 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 5, 1),
					},
					new List<Item>
					{
						new Item(351, 10, 1),
						new Item(35, 1, 1),
					}, "crafting_table"){ UniqueId = 955 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 5, 1),
					},
					new List<Item>
					{
						new Item(351, 10, 1),
						new Item(35, 0, 1),
					}, "crafting_table"){ UniqueId = 954 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 5, 1),
					},
					new List<Item>
					{
						new Item(351, 10, 1),
						new Item(35, 13, 1),
					}, "crafting_table"){ UniqueId = 966 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 5, 1),
					},
					new List<Item>
					{
						new Item(351, 10, 1),
						new Item(35, 12, 1),
					}, "crafting_table"){ UniqueId = 965 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 5, 1),
					},
					new List<Item>
					{
						new Item(351, 10, 1),
						new Item(35, 11, 1),
					}, "crafting_table"){ UniqueId = 964 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 5, 1),
					},
					new List<Item>
					{
						new Item(351, 10, 1),
						new Item(35, 10, 1),
					}, "crafting_table"){ UniqueId = 963 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 5, 1),
					},
					new List<Item>
					{
						new Item(351, 10, 1),
						new Item(35, 9, 1),
					}, "crafting_table"){ UniqueId = 962 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 5, 1),
					},
					new List<Item>
					{
						new Item(351, 10, 1),
						new Item(35, 8, 1),
					}, "crafting_table"){ UniqueId = 961 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 5, 1),
					},
					new List<Item>
					{
						new Item(351, 10, 1),
						new Item(35, 7, 1),
					}, "crafting_table"){ UniqueId = 960 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 5, 1),
					},
					new List<Item>
					{
						new Item(351, 10, 1),
						new Item(35, 6, 1),
					}, "crafting_table"){ UniqueId = 959 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 4, 1),
					},
					new List<Item>
					{
						new Item(351, 11, 1),
						new Item(35, 15, 1),
					}, "crafting_table"){ UniqueId = 953 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 4, 1),
					},
					new List<Item>
					{
						new Item(351, 11, 1),
						new Item(35, 14, 1),
					}, "crafting_table"){ UniqueId = 952 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 4, 1),
					},
					new List<Item>
					{
						new Item(351, 11, 1),
						new Item(35, 5, 1),
					}, "crafting_table"){ UniqueId = 943 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 4, 1),
					},
					new List<Item>
					{
						new Item(351, 11, 1),
						new Item(35, 3, 1),
					}, "crafting_table"){ UniqueId = 942 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 4, 1),
					},
					new List<Item>
					{
						new Item(351, 11, 1),
						new Item(35, 2, 1),
					}, "crafting_table"){ UniqueId = 941 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 4, 1),
					},
					new List<Item>
					{
						new Item(351, 11, 1),
						new Item(35, 1, 1),
					}, "crafting_table"){ UniqueId = 940 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 4, 1),
					},
					new List<Item>
					{
						new Item(351, 11, 1),
						new Item(35, 0, 1),
					}, "crafting_table"){ UniqueId = 939 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 4, 1),
					},
					new List<Item>
					{
						new Item(351, 11, 1),
						new Item(35, 13, 1),
					}, "crafting_table"){ UniqueId = 951 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 4, 1),
					},
					new List<Item>
					{
						new Item(351, 11, 1),
						new Item(35, 12, 1),
					}, "crafting_table"){ UniqueId = 950 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 4, 1),
					},
					new List<Item>
					{
						new Item(351, 11, 1),
						new Item(35, 11, 1),
					}, "crafting_table"){ UniqueId = 949 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 4, 1),
					},
					new List<Item>
					{
						new Item(351, 11, 1),
						new Item(35, 10, 1),
					}, "crafting_table"){ UniqueId = 948 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 4, 1),
					},
					new List<Item>
					{
						new Item(351, 11, 1),
						new Item(35, 9, 1),
					}, "crafting_table"){ UniqueId = 947 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 4, 1),
					},
					new List<Item>
					{
						new Item(351, 11, 1),
						new Item(35, 8, 1),
					}, "crafting_table"){ UniqueId = 946 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 4, 1),
					},
					new List<Item>
					{
						new Item(351, 11, 1),
						new Item(35, 7, 1),
					}, "crafting_table"){ UniqueId = 945 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 4, 1),
					},
					new List<Item>
					{
						new Item(351, 11, 1),
						new Item(35, 6, 1),
					}, "crafting_table"){ UniqueId = 944 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 3, 1),
					},
					new List<Item>
					{
						new Item(351, 12, 1),
						new Item(35, 15, 1),
					}, "crafting_table"){ UniqueId = 938 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 3, 1),
					},
					new List<Item>
					{
						new Item(351, 12, 1),
						new Item(35, 14, 1),
					}, "crafting_table"){ UniqueId = 937 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 3, 1),
					},
					new List<Item>
					{
						new Item(351, 12, 1),
						new Item(35, 5, 1),
					}, "crafting_table"){ UniqueId = 928 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 3, 1),
					},
					new List<Item>
					{
						new Item(351, 12, 1),
						new Item(35, 4, 1),
					}, "crafting_table"){ UniqueId = 927 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 3, 1),
					},
					new List<Item>
					{
						new Item(351, 12, 1),
						new Item(35, 2, 1),
					}, "crafting_table"){ UniqueId = 926 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 3, 1),
					},
					new List<Item>
					{
						new Item(351, 12, 1),
						new Item(35, 1, 1),
					}, "crafting_table"){ UniqueId = 925 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 3, 1),
					},
					new List<Item>
					{
						new Item(351, 12, 1),
						new Item(35, 0, 1),
					}, "crafting_table"){ UniqueId = 924 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 3, 1),
					},
					new List<Item>
					{
						new Item(351, 12, 1),
						new Item(35, 13, 1),
					}, "crafting_table"){ UniqueId = 936 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 3, 1),
					},
					new List<Item>
					{
						new Item(351, 12, 1),
						new Item(35, 12, 1),
					}, "crafting_table"){ UniqueId = 935 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 3, 1),
					},
					new List<Item>
					{
						new Item(351, 12, 1),
						new Item(35, 11, 1),
					}, "crafting_table"){ UniqueId = 934 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 3, 1),
					},
					new List<Item>
					{
						new Item(351, 12, 1),
						new Item(35, 10, 1),
					}, "crafting_table"){ UniqueId = 933 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 3, 1),
					},
					new List<Item>
					{
						new Item(351, 12, 1),
						new Item(35, 9, 1),
					}, "crafting_table"){ UniqueId = 932 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 3, 1),
					},
					new List<Item>
					{
						new Item(351, 12, 1),
						new Item(35, 8, 1),
					}, "crafting_table"){ UniqueId = 931 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 3, 1),
					},
					new List<Item>
					{
						new Item(351, 12, 1),
						new Item(35, 7, 1),
					}, "crafting_table"){ UniqueId = 930 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 3, 1),
					},
					new List<Item>
					{
						new Item(351, 12, 1),
						new Item(35, 6, 1),
					}, "crafting_table"){ UniqueId = 929 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 2, 1),
					},
					new List<Item>
					{
						new Item(351, 13, 1),
						new Item(35, 15, 1),
					}, "crafting_table"){ UniqueId = 923 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 2, 1),
					},
					new List<Item>
					{
						new Item(351, 13, 1),
						new Item(35, 14, 1),
					}, "crafting_table"){ UniqueId = 922 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 2, 1),
					},
					new List<Item>
					{
						new Item(351, 13, 1),
						new Item(35, 5, 1),
					}, "crafting_table"){ UniqueId = 913 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 2, 1),
					},
					new List<Item>
					{
						new Item(351, 13, 1),
						new Item(35, 4, 1),
					}, "crafting_table"){ UniqueId = 912 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 2, 1),
					},
					new List<Item>
					{
						new Item(351, 13, 1),
						new Item(35, 3, 1),
					}, "crafting_table"){ UniqueId = 911 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 2, 1),
					},
					new List<Item>
					{
						new Item(351, 13, 1),
						new Item(35, 1, 1),
					}, "crafting_table"){ UniqueId = 910 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 2, 1),
					},
					new List<Item>
					{
						new Item(351, 13, 1),
						new Item(35, 0, 1),
					}, "crafting_table"){ UniqueId = 909 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 2, 1),
					},
					new List<Item>
					{
						new Item(351, 13, 1),
						new Item(35, 13, 1),
					}, "crafting_table"){ UniqueId = 921 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 2, 1),
					},
					new List<Item>
					{
						new Item(351, 13, 1),
						new Item(35, 12, 1),
					}, "crafting_table"){ UniqueId = 920 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 2, 1),
					},
					new List<Item>
					{
						new Item(351, 13, 1),
						new Item(35, 11, 1),
					}, "crafting_table"){ UniqueId = 919 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 2, 1),
					},
					new List<Item>
					{
						new Item(351, 13, 1),
						new Item(35, 10, 1),
					}, "crafting_table"){ UniqueId = 918 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 2, 1),
					},
					new List<Item>
					{
						new Item(351, 13, 1),
						new Item(35, 9, 1),
					}, "crafting_table"){ UniqueId = 917 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 2, 1),
					},
					new List<Item>
					{
						new Item(351, 13, 1),
						new Item(35, 8, 1),
					}, "crafting_table"){ UniqueId = 916 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 2, 1),
					},
					new List<Item>
					{
						new Item(351, 13, 1),
						new Item(35, 7, 1),
					}, "crafting_table"){ UniqueId = 915 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 2, 1),
					},
					new List<Item>
					{
						new Item(351, 13, 1),
						new Item(35, 6, 1),
					}, "crafting_table"){ UniqueId = 914 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 1, 1),
					},
					new List<Item>
					{
						new Item(351, 14, 1),
						new Item(35, 15, 1),
					}, "crafting_table"){ UniqueId = 908 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 1, 1),
					},
					new List<Item>
					{
						new Item(351, 14, 1),
						new Item(35, 14, 1),
					}, "crafting_table"){ UniqueId = 907 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 1, 1),
					},
					new List<Item>
					{
						new Item(351, 14, 1),
						new Item(35, 5, 1),
					}, "crafting_table"){ UniqueId = 898 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 1, 1),
					},
					new List<Item>
					{
						new Item(351, 14, 1),
						new Item(35, 4, 1),
					}, "crafting_table"){ UniqueId = 897 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 1, 1),
					},
					new List<Item>
					{
						new Item(351, 14, 1),
						new Item(35, 3, 1),
					}, "crafting_table"){ UniqueId = 896 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 1, 1),
					},
					new List<Item>
					{
						new Item(351, 14, 1),
						new Item(35, 2, 1),
					}, "crafting_table"){ UniqueId = 895 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 1, 1),
					},
					new List<Item>
					{
						new Item(351, 14, 1),
						new Item(35, 0, 1),
					}, "crafting_table"){ UniqueId = 894 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 1, 1),
					},
					new List<Item>
					{
						new Item(351, 14, 1),
						new Item(35, 13, 1),
					}, "crafting_table"){ UniqueId = 906 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 1, 1),
					},
					new List<Item>
					{
						new Item(351, 14, 1),
						new Item(35, 12, 1),
					}, "crafting_table"){ UniqueId = 905 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 1, 1),
					},
					new List<Item>
					{
						new Item(351, 14, 1),
						new Item(35, 11, 1),
					}, "crafting_table"){ UniqueId = 904 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 1, 1),
					},
					new List<Item>
					{
						new Item(351, 14, 1),
						new Item(35, 10, 1),
					}, "crafting_table"){ UniqueId = 903 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 1, 1),
					},
					new List<Item>
					{
						new Item(351, 14, 1),
						new Item(35, 9, 1),
					}, "crafting_table"){ UniqueId = 902 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 1, 1),
					},
					new List<Item>
					{
						new Item(351, 14, 1),
						new Item(35, 8, 1),
					}, "crafting_table"){ UniqueId = 901 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 1, 1),
					},
					new List<Item>
					{
						new Item(351, 14, 1),
						new Item(35, 7, 1),
					}, "crafting_table"){ UniqueId = 900 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 1, 1),
					},
					new List<Item>
					{
						new Item(351, 14, 1),
						new Item(35, 6, 1),
					}, "crafting_table"){ UniqueId = 899 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1),
					},
					new List<Item>
					{
						new Item(351, 15, 1),
						new Item(35, 15, 1),
					}, "crafting_table"){ UniqueId = 893 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1),
					},
					new List<Item>
					{
						new Item(351, 15, 1),
						new Item(35, 14, 1),
					}, "crafting_table"){ UniqueId = 892 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1),
					},
					new List<Item>
					{
						new Item(351, 15, 1),
						new Item(35, 5, 1),
					}, "crafting_table"){ UniqueId = 883 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1),
					},
					new List<Item>
					{
						new Item(351, 15, 1),
						new Item(35, 4, 1),
					}, "crafting_table"){ UniqueId = 882 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1),
					},
					new List<Item>
					{
						new Item(351, 15, 1),
						new Item(35, 3, 1),
					}, "crafting_table"){ UniqueId = 881 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1),
					},
					new List<Item>
					{
						new Item(351, 15, 1),
						new Item(35, 2, 1),
					}, "crafting_table"){ UniqueId = 880 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1),
					},
					new List<Item>
					{
						new Item(351, 15, 1),
						new Item(35, 1, 1),
					}, "crafting_table"){ UniqueId = 879 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1),
					},
					new List<Item>
					{
						new Item(351, 15, 1),
						new Item(35, 13, 1),
					}, "crafting_table"){ UniqueId = 891 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1),
					},
					new List<Item>
					{
						new Item(351, 15, 1),
						new Item(35, 12, 1),
					}, "crafting_table"){ UniqueId = 890 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1),
					},
					new List<Item>
					{
						new Item(351, 15, 1),
						new Item(35, 11, 1),
					}, "crafting_table"){ UniqueId = 889 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1),
					},
					new List<Item>
					{
						new Item(351, 15, 1),
						new Item(35, 10, 1),
					}, "crafting_table"){ UniqueId = 888 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1),
					},
					new List<Item>
					{
						new Item(351, 15, 1),
						new Item(35, 9, 1),
					}, "crafting_table"){ UniqueId = 887 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1),
					},
					new List<Item>
					{
						new Item(351, 15, 1),
						new Item(35, 8, 1),
					}, "crafting_table"){ UniqueId = 886 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1),
					},
					new List<Item>
					{
						new Item(351, 15, 1),
						new Item(35, 7, 1),
					}, "crafting_table"){ UniqueId = 885 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1),
					},
					new List<Item>
					{
						new Item(351, 15, 1),
						new Item(35, 6, 1),
					}, "crafting_table"){ UniqueId = 884 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 15, 1),
					},
					new List<Item>
					{
						new Item(351, 16, 1),
						new Item(35, 14, 1),
					}, "crafting_table"){ UniqueId = 878 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 15, 1),
					},
					new List<Item>
					{
						new Item(351, 16, 1),
						new Item(35, 5, 1),
					}, "crafting_table"){ UniqueId = 869 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 15, 1),
					},
					new List<Item>
					{
						new Item(351, 16, 1),
						new Item(35, 4, 1),
					}, "crafting_table"){ UniqueId = 868 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 15, 1),
					},
					new List<Item>
					{
						new Item(351, 16, 1),
						new Item(35, 3, 1),
					}, "crafting_table"){ UniqueId = 867 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 15, 1),
					},
					new List<Item>
					{
						new Item(351, 16, 1),
						new Item(35, 2, 1),
					}, "crafting_table"){ UniqueId = 866 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 15, 1),
					},
					new List<Item>
					{
						new Item(351, 16, 1),
						new Item(35, 1, 1),
					}, "crafting_table"){ UniqueId = 865 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 15, 1),
					},
					new List<Item>
					{
						new Item(351, 16, 1),
						new Item(35, 0, 1),
					}, "crafting_table"){ UniqueId = 864 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 15, 1),
					},
					new List<Item>
					{
						new Item(351, 16, 1),
						new Item(35, 13, 1),
					}, "crafting_table"){ UniqueId = 877 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 15, 1),
					},
					new List<Item>
					{
						new Item(351, 16, 1),
						new Item(35, 12, 1),
					}, "crafting_table"){ UniqueId = 876 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 15, 1),
					},
					new List<Item>
					{
						new Item(351, 16, 1),
						new Item(35, 11, 1),
					}, "crafting_table"){ UniqueId = 875 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 15, 1),
					},
					new List<Item>
					{
						new Item(351, 16, 1),
						new Item(35, 10, 1),
					}, "crafting_table"){ UniqueId = 874 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 15, 1),
					},
					new List<Item>
					{
						new Item(351, 16, 1),
						new Item(35, 9, 1),
					}, "crafting_table"){ UniqueId = 873 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 15, 1),
					},
					new List<Item>
					{
						new Item(351, 16, 1),
						new Item(35, 8, 1),
					}, "crafting_table"){ UniqueId = 872 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 15, 1),
					},
					new List<Item>
					{
						new Item(351, 16, 1),
						new Item(35, 7, 1),
					}, "crafting_table"){ UniqueId = 871 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 15, 1),
					},
					new List<Item>
					{
						new Item(351, 16, 1),
						new Item(35, 6, 1),
					}, "crafting_table"){ UniqueId = 870 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 12, 1),
					},
					new List<Item>
					{
						new Item(351, 17, 1),
						new Item(35, 15, 1),
					}, "crafting_table"){ UniqueId = 863 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 12, 1),
					},
					new List<Item>
					{
						new Item(351, 17, 1),
						new Item(35, 14, 1),
					}, "crafting_table"){ UniqueId = 862 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 12, 1),
					},
					new List<Item>
					{
						new Item(351, 17, 1),
						new Item(35, 5, 1),
					}, "crafting_table"){ UniqueId = 854 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 12, 1),
					},
					new List<Item>
					{
						new Item(351, 17, 1),
						new Item(35, 4, 1),
					}, "crafting_table"){ UniqueId = 853 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 12, 1),
					},
					new List<Item>
					{
						new Item(351, 17, 1),
						new Item(35, 3, 1),
					}, "crafting_table"){ UniqueId = 852 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 12, 1),
					},
					new List<Item>
					{
						new Item(351, 17, 1),
						new Item(35, 2, 1),
					}, "crafting_table"){ UniqueId = 851 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 12, 1),
					},
					new List<Item>
					{
						new Item(351, 17, 1),
						new Item(35, 1, 1),
					}, "crafting_table"){ UniqueId = 850 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 12, 1),
					},
					new List<Item>
					{
						new Item(351, 17, 1),
						new Item(35, 0, 1),
					}, "crafting_table"){ UniqueId = 849 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 12, 1),
					},
					new List<Item>
					{
						new Item(351, 17, 1),
						new Item(35, 13, 1),
					}, "crafting_table"){ UniqueId = 861 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 12, 1),
					},
					new List<Item>
					{
						new Item(351, 17, 1),
						new Item(35, 11, 1),
					}, "crafting_table"){ UniqueId = 860 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 12, 1),
					},
					new List<Item>
					{
						new Item(351, 17, 1),
						new Item(35, 10, 1),
					}, "crafting_table"){ UniqueId = 859 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 12, 1),
					},
					new List<Item>
					{
						new Item(351, 17, 1),
						new Item(35, 9, 1),
					}, "crafting_table"){ UniqueId = 858 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 12, 1),
					},
					new List<Item>
					{
						new Item(351, 17, 1),
						new Item(35, 8, 1),
					}, "crafting_table"){ UniqueId = 857 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 12, 1),
					},
					new List<Item>
					{
						new Item(351, 17, 1),
						new Item(35, 7, 1),
					}, "crafting_table"){ UniqueId = 856 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 12, 1),
					},
					new List<Item>
					{
						new Item(351, 17, 1),
						new Item(35, 6, 1),
					}, "crafting_table"){ UniqueId = 855 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 11, 1),
					},
					new List<Item>
					{
						new Item(351, 18, 1),
						new Item(35, 15, 1),
					}, "crafting_table"){ UniqueId = 848 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 11, 1),
					},
					new List<Item>
					{
						new Item(351, 18, 1),
						new Item(35, 14, 1),
					}, "crafting_table"){ UniqueId = 847 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 11, 1),
					},
					new List<Item>
					{
						new Item(351, 18, 1),
						new Item(35, 5, 1),
					}, "crafting_table"){ UniqueId = 839 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 11, 1),
					},
					new List<Item>
					{
						new Item(351, 18, 1),
						new Item(35, 4, 1),
					}, "crafting_table"){ UniqueId = 838 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 11, 1),
					},
					new List<Item>
					{
						new Item(351, 18, 1),
						new Item(35, 3, 1),
					}, "crafting_table"){ UniqueId = 837 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 11, 1),
					},
					new List<Item>
					{
						new Item(351, 18, 1),
						new Item(35, 2, 1),
					}, "crafting_table"){ UniqueId = 836 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 11, 1),
					},
					new List<Item>
					{
						new Item(351, 18, 1),
						new Item(35, 1, 1),
					}, "crafting_table"){ UniqueId = 835 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 11, 1),
					},
					new List<Item>
					{
						new Item(351, 18, 1),
						new Item(35, 0, 1),
					}, "crafting_table"){ UniqueId = 834 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 11, 1),
					},
					new List<Item>
					{
						new Item(351, 18, 1),
						new Item(35, 13, 1),
					}, "crafting_table"){ UniqueId = 846 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 11, 1),
					},
					new List<Item>
					{
						new Item(351, 18, 1),
						new Item(35, 12, 1),
					}, "crafting_table"){ UniqueId = 845 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 11, 1),
					},
					new List<Item>
					{
						new Item(351, 18, 1),
						new Item(35, 10, 1),
					}, "crafting_table"){ UniqueId = 844 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 11, 1),
					},
					new List<Item>
					{
						new Item(351, 18, 1),
						new Item(35, 9, 1),
					}, "crafting_table"){ UniqueId = 843 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 11, 1),
					},
					new List<Item>
					{
						new Item(351, 18, 1),
						new Item(35, 8, 1),
					}, "crafting_table"){ UniqueId = 842 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 11, 1),
					},
					new List<Item>
					{
						new Item(351, 18, 1),
						new Item(35, 7, 1),
					}, "crafting_table"){ UniqueId = 841 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 11, 1),
					},
					new List<Item>
					{
						new Item(351, 18, 1),
						new Item(35, 6, 1),
					}, "crafting_table"){ UniqueId = 840 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1),
					},
					new List<Item>
					{
						new Item(351, 19, 1),
						new Item(35, 15, 1),
					}, "crafting_table"){ UniqueId = 833 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1),
					},
					new List<Item>
					{
						new Item(351, 19, 1),
						new Item(35, 14, 1),
					}, "crafting_table"){ UniqueId = 832 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1),
					},
					new List<Item>
					{
						new Item(351, 19, 1),
						new Item(35, 5, 1),
					}, "crafting_table"){ UniqueId = 823 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1),
					},
					new List<Item>
					{
						new Item(351, 19, 1),
						new Item(35, 4, 1),
					}, "crafting_table"){ UniqueId = 822 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1),
					},
					new List<Item>
					{
						new Item(351, 19, 1),
						new Item(35, 3, 1),
					}, "crafting_table"){ UniqueId = 821 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1),
					},
					new List<Item>
					{
						new Item(351, 19, 1),
						new Item(35, 2, 1),
					}, "crafting_table"){ UniqueId = 820 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1),
					},
					new List<Item>
					{
						new Item(351, 19, 1),
						new Item(35, 1, 1),
					}, "crafting_table"){ UniqueId = 819 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1),
					},
					new List<Item>
					{
						new Item(351, 19, 1),
						new Item(35, 13, 1),
					}, "crafting_table"){ UniqueId = 831 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1),
					},
					new List<Item>
					{
						new Item(351, 19, 1),
						new Item(35, 12, 1),
					}, "crafting_table"){ UniqueId = 830 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1),
					},
					new List<Item>
					{
						new Item(351, 19, 1),
						new Item(35, 11, 1),
					}, "crafting_table"){ UniqueId = 829 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1),
					},
					new List<Item>
					{
						new Item(351, 19, 1),
						new Item(35, 10, 1),
					}, "crafting_table"){ UniqueId = 828 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1),
					},
					new List<Item>
					{
						new Item(351, 19, 1),
						new Item(35, 9, 1),
					}, "crafting_table"){ UniqueId = 827 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1),
					},
					new List<Item>
					{
						new Item(351, 19, 1),
						new Item(35, 8, 1),
					}, "crafting_table"){ UniqueId = 826 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1),
					},
					new List<Item>
					{
						new Item(351, 19, 1),
						new Item(35, 7, 1),
					}, "crafting_table"){ UniqueId = 825 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 0, 1),
					},
					new List<Item>
					{
						new Item(351, 19, 1),
						new Item(35, 6, 1),
					}, "crafting_table"){ UniqueId = 824 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 14, 1),
					},
					new List<Item>
					{
						new Item(351, 1, 1),
						new Item(35, 15, 1),
					}, "crafting_table"){ UniqueId = 1103 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 14, 1),
					},
					new List<Item>
					{
						new Item(351, 1, 1),
						new Item(35, 5, 1),
					}, "crafting_table"){ UniqueId = 1094 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 14, 1),
					},
					new List<Item>
					{
						new Item(351, 1, 1),
						new Item(35, 4, 1),
					}, "crafting_table"){ UniqueId = 1093 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 14, 1),
					},
					new List<Item>
					{
						new Item(351, 1, 1),
						new Item(35, 3, 1),
					}, "crafting_table"){ UniqueId = 1092 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 14, 1),
					},
					new List<Item>
					{
						new Item(351, 1, 1),
						new Item(35, 2, 1),
					}, "crafting_table"){ UniqueId = 1091 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 14, 1),
					},
					new List<Item>
					{
						new Item(351, 1, 1),
						new Item(35, 1, 1),
					}, "crafting_table"){ UniqueId = 1090 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 14, 1),
					},
					new List<Item>
					{
						new Item(351, 1, 1),
						new Item(35, 0, 1),
					}, "crafting_table"){ UniqueId = 1089 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 14, 1),
					},
					new List<Item>
					{
						new Item(351, 1, 1),
						new Item(35, 13, 1),
					}, "crafting_table"){ UniqueId = 1102 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 14, 1),
					},
					new List<Item>
					{
						new Item(351, 1, 1),
						new Item(35, 12, 1),
					}, "crafting_table"){ UniqueId = 1101 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 14, 1),
					},
					new List<Item>
					{
						new Item(351, 1, 1),
						new Item(35, 11, 1),
					}, "crafting_table"){ UniqueId = 1100 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 14, 1),
					},
					new List<Item>
					{
						new Item(351, 1, 1),
						new Item(35, 10, 1),
					}, "crafting_table"){ UniqueId = 1099 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 14, 1),
					},
					new List<Item>
					{
						new Item(351, 1, 1),
						new Item(35, 9, 1),
					}, "crafting_table"){ UniqueId = 1098 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 14, 1),
					},
					new List<Item>
					{
						new Item(351, 1, 1),
						new Item(35, 8, 1),
					}, "crafting_table"){ UniqueId = 1097 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 14, 1),
					},
					new List<Item>
					{
						new Item(351, 1, 1),
						new Item(35, 7, 1),
					}, "crafting_table"){ UniqueId = 1096 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 14, 1),
					},
					new List<Item>
					{
						new Item(351, 1, 1),
						new Item(35, 6, 1),
					}, "crafting_table"){ UniqueId = 1095 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 13, 1),
					},
					new List<Item>
					{
						new Item(351, 2, 1),
						new Item(35, 15, 1),
					}, "crafting_table"){ UniqueId = 1088 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 13, 1),
					},
					new List<Item>
					{
						new Item(351, 2, 1),
						new Item(35, 14, 1),
					}, "crafting_table"){ UniqueId = 1087 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 13, 1),
					},
					new List<Item>
					{
						new Item(351, 2, 1),
						new Item(35, 5, 1),
					}, "crafting_table"){ UniqueId = 1079 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 13, 1),
					},
					new List<Item>
					{
						new Item(351, 2, 1),
						new Item(35, 4, 1),
					}, "crafting_table"){ UniqueId = 1078 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 13, 1),
					},
					new List<Item>
					{
						new Item(351, 2, 1),
						new Item(35, 3, 1),
					}, "crafting_table"){ UniqueId = 1077 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 13, 1),
					},
					new List<Item>
					{
						new Item(351, 2, 1),
						new Item(35, 2, 1),
					}, "crafting_table"){ UniqueId = 1076 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 13, 1),
					},
					new List<Item>
					{
						new Item(351, 2, 1),
						new Item(35, 1, 1),
					}, "crafting_table"){ UniqueId = 1075 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 13, 1),
					},
					new List<Item>
					{
						new Item(351, 2, 1),
						new Item(35, 0, 1),
					}, "crafting_table"){ UniqueId = 1074 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 13, 1),
					},
					new List<Item>
					{
						new Item(351, 2, 1),
						new Item(35, 12, 1),
					}, "crafting_table"){ UniqueId = 1086 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 13, 1),
					},
					new List<Item>
					{
						new Item(351, 2, 1),
						new Item(35, 11, 1),
					}, "crafting_table"){ UniqueId = 1085 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 13, 1),
					},
					new List<Item>
					{
						new Item(351, 2, 1),
						new Item(35, 10, 1),
					}, "crafting_table"){ UniqueId = 1084 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 13, 1),
					},
					new List<Item>
					{
						new Item(351, 2, 1),
						new Item(35, 9, 1),
					}, "crafting_table"){ UniqueId = 1083 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 13, 1),
					},
					new List<Item>
					{
						new Item(351, 2, 1),
						new Item(35, 8, 1),
					}, "crafting_table"){ UniqueId = 1082 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 13, 1),
					},
					new List<Item>
					{
						new Item(351, 2, 1),
						new Item(35, 7, 1),
					}, "crafting_table"){ UniqueId = 1081 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 13, 1),
					},
					new List<Item>
					{
						new Item(351, 2, 1),
						new Item(35, 6, 1),
					}, "crafting_table"){ UniqueId = 1080 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 12, 1),
					},
					new List<Item>
					{
						new Item(351, 3, 1),
						new Item(35, 15, 1),
					}, "crafting_table"){ UniqueId = 1073 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 12, 1),
					},
					new List<Item>
					{
						new Item(351, 3, 1),
						new Item(35, 14, 1),
					}, "crafting_table"){ UniqueId = 1072 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 12, 1),
					},
					new List<Item>
					{
						new Item(351, 3, 1),
						new Item(35, 5, 1),
					}, "crafting_table"){ UniqueId = 1064 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 12, 1),
					},
					new List<Item>
					{
						new Item(351, 3, 1),
						new Item(35, 4, 1),
					}, "crafting_table"){ UniqueId = 1063 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 12, 1),
					},
					new List<Item>
					{
						new Item(351, 3, 1),
						new Item(35, 3, 1),
					}, "crafting_table"){ UniqueId = 1062 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 12, 1),
					},
					new List<Item>
					{
						new Item(351, 3, 1),
						new Item(35, 2, 1),
					}, "crafting_table"){ UniqueId = 1061 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 12, 1),
					},
					new List<Item>
					{
						new Item(351, 3, 1),
						new Item(35, 1, 1),
					}, "crafting_table"){ UniqueId = 1060 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 12, 1),
					},
					new List<Item>
					{
						new Item(351, 3, 1),
						new Item(35, 0, 1),
					}, "crafting_table"){ UniqueId = 1059 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 12, 1),
					},
					new List<Item>
					{
						new Item(351, 3, 1),
						new Item(35, 13, 1),
					}, "crafting_table"){ UniqueId = 1071 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 12, 1),
					},
					new List<Item>
					{
						new Item(351, 3, 1),
						new Item(35, 11, 1),
					}, "crafting_table"){ UniqueId = 1070 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 12, 1),
					},
					new List<Item>
					{
						new Item(351, 3, 1),
						new Item(35, 10, 1),
					}, "crafting_table"){ UniqueId = 1069 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 12, 1),
					},
					new List<Item>
					{
						new Item(351, 3, 1),
						new Item(35, 9, 1),
					}, "crafting_table"){ UniqueId = 1068 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 12, 1),
					},
					new List<Item>
					{
						new Item(351, 3, 1),
						new Item(35, 8, 1),
					}, "crafting_table"){ UniqueId = 1067 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 12, 1),
					},
					new List<Item>
					{
						new Item(351, 3, 1),
						new Item(35, 7, 1),
					}, "crafting_table"){ UniqueId = 1066 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 12, 1),
					},
					new List<Item>
					{
						new Item(351, 3, 1),
						new Item(35, 6, 1),
					}, "crafting_table"){ UniqueId = 1065 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 11, 1),
					},
					new List<Item>
					{
						new Item(351, 4, 1),
						new Item(35, 15, 1),
					}, "crafting_table"){ UniqueId = 1058 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 11, 1),
					},
					new List<Item>
					{
						new Item(351, 4, 1),
						new Item(35, 14, 1),
					}, "crafting_table"){ UniqueId = 1057 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 11, 1),
					},
					new List<Item>
					{
						new Item(351, 4, 1),
						new Item(35, 5, 1),
					}, "crafting_table"){ UniqueId = 1049 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 11, 1),
					},
					new List<Item>
					{
						new Item(351, 4, 1),
						new Item(35, 4, 1),
					}, "crafting_table"){ UniqueId = 1048 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 11, 1),
					},
					new List<Item>
					{
						new Item(351, 4, 1),
						new Item(35, 3, 1),
					}, "crafting_table"){ UniqueId = 1047 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 11, 1),
					},
					new List<Item>
					{
						new Item(351, 4, 1),
						new Item(35, 2, 1),
					}, "crafting_table"){ UniqueId = 1046 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 11, 1),
					},
					new List<Item>
					{
						new Item(351, 4, 1),
						new Item(35, 1, 1),
					}, "crafting_table"){ UniqueId = 1045 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 11, 1),
					},
					new List<Item>
					{
						new Item(351, 4, 1),
						new Item(35, 0, 1),
					}, "crafting_table"){ UniqueId = 1044 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 11, 1),
					},
					new List<Item>
					{
						new Item(351, 4, 1),
						new Item(35, 13, 1),
					}, "crafting_table"){ UniqueId = 1056 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 11, 1),
					},
					new List<Item>
					{
						new Item(351, 4, 1),
						new Item(35, 12, 1),
					}, "crafting_table"){ UniqueId = 1055 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 11, 1),
					},
					new List<Item>
					{
						new Item(351, 4, 1),
						new Item(35, 10, 1),
					}, "crafting_table"){ UniqueId = 1054 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 11, 1),
					},
					new List<Item>
					{
						new Item(351, 4, 1),
						new Item(35, 9, 1),
					}, "crafting_table"){ UniqueId = 1053 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 11, 1),
					},
					new List<Item>
					{
						new Item(351, 4, 1),
						new Item(35, 8, 1),
					}, "crafting_table"){ UniqueId = 1052 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 11, 1),
					},
					new List<Item>
					{
						new Item(351, 4, 1),
						new Item(35, 7, 1),
					}, "crafting_table"){ UniqueId = 1051 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 11, 1),
					},
					new List<Item>
					{
						new Item(351, 4, 1),
						new Item(35, 6, 1),
					}, "crafting_table"){ UniqueId = 1050 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 10, 1),
					},
					new List<Item>
					{
						new Item(351, 5, 1),
						new Item(35, 15, 1),
					}, "crafting_table"){ UniqueId = 1043 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 10, 1),
					},
					new List<Item>
					{
						new Item(351, 5, 1),
						new Item(35, 14, 1),
					}, "crafting_table"){ UniqueId = 1042 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 10, 1),
					},
					new List<Item>
					{
						new Item(351, 5, 1),
						new Item(35, 5, 1),
					}, "crafting_table"){ UniqueId = 1034 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 10, 1),
					},
					new List<Item>
					{
						new Item(351, 5, 1),
						new Item(35, 4, 1),
					}, "crafting_table"){ UniqueId = 1033 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 10, 1),
					},
					new List<Item>
					{
						new Item(351, 5, 1),
						new Item(35, 3, 1),
					}, "crafting_table"){ UniqueId = 1032 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 10, 1),
					},
					new List<Item>
					{
						new Item(351, 5, 1),
						new Item(35, 2, 1),
					}, "crafting_table"){ UniqueId = 1031 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 10, 1),
					},
					new List<Item>
					{
						new Item(351, 5, 1),
						new Item(35, 1, 1),
					}, "crafting_table"){ UniqueId = 1030 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 10, 1),
					},
					new List<Item>
					{
						new Item(351, 5, 1),
						new Item(35, 0, 1),
					}, "crafting_table"){ UniqueId = 1029 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 10, 1),
					},
					new List<Item>
					{
						new Item(351, 5, 1),
						new Item(35, 13, 1),
					}, "crafting_table"){ UniqueId = 1041 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 10, 1),
					},
					new List<Item>
					{
						new Item(351, 5, 1),
						new Item(35, 12, 1),
					}, "crafting_table"){ UniqueId = 1040 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 10, 1),
					},
					new List<Item>
					{
						new Item(351, 5, 1),
						new Item(35, 11, 1),
					}, "crafting_table"){ UniqueId = 1039 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 10, 1),
					},
					new List<Item>
					{
						new Item(351, 5, 1),
						new Item(35, 9, 1),
					}, "crafting_table"){ UniqueId = 1038 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 10, 1),
					},
					new List<Item>
					{
						new Item(351, 5, 1),
						new Item(35, 8, 1),
					}, "crafting_table"){ UniqueId = 1037 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 10, 1),
					},
					new List<Item>
					{
						new Item(351, 5, 1),
						new Item(35, 7, 1),
					}, "crafting_table"){ UniqueId = 1036 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 10, 1),
					},
					new List<Item>
					{
						new Item(351, 5, 1),
						new Item(35, 6, 1),
					}, "crafting_table"){ UniqueId = 1035 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 9, 1),
					},
					new List<Item>
					{
						new Item(351, 6, 1),
						new Item(35, 15, 1),
					}, "crafting_table"){ UniqueId = 1028 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 9, 1),
					},
					new List<Item>
					{
						new Item(351, 6, 1),
						new Item(35, 14, 1),
					}, "crafting_table"){ UniqueId = 1027 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 9, 1),
					},
					new List<Item>
					{
						new Item(351, 6, 1),
						new Item(35, 5, 1),
					}, "crafting_table"){ UniqueId = 1019 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 9, 1),
					},
					new List<Item>
					{
						new Item(351, 6, 1),
						new Item(35, 4, 1),
					}, "crafting_table"){ UniqueId = 1018 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 9, 1),
					},
					new List<Item>
					{
						new Item(351, 6, 1),
						new Item(35, 3, 1),
					}, "crafting_table"){ UniqueId = 1017 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 9, 1),
					},
					new List<Item>
					{
						new Item(351, 6, 1),
						new Item(35, 2, 1),
					}, "crafting_table"){ UniqueId = 1016 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 9, 1),
					},
					new List<Item>
					{
						new Item(351, 6, 1),
						new Item(35, 1, 1),
					}, "crafting_table"){ UniqueId = 1015 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 9, 1),
					},
					new List<Item>
					{
						new Item(351, 6, 1),
						new Item(35, 0, 1),
					}, "crafting_table"){ UniqueId = 1014 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 9, 1),
					},
					new List<Item>
					{
						new Item(351, 6, 1),
						new Item(35, 13, 1),
					}, "crafting_table"){ UniqueId = 1026 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 9, 1),
					},
					new List<Item>
					{
						new Item(351, 6, 1),
						new Item(35, 12, 1),
					}, "crafting_table"){ UniqueId = 1025 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 9, 1),
					},
					new List<Item>
					{
						new Item(351, 6, 1),
						new Item(35, 11, 1),
					}, "crafting_table"){ UniqueId = 1024 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 9, 1),
					},
					new List<Item>
					{
						new Item(351, 6, 1),
						new Item(35, 10, 1),
					}, "crafting_table"){ UniqueId = 1023 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 9, 1),
					},
					new List<Item>
					{
						new Item(351, 6, 1),
						new Item(35, 8, 1),
					}, "crafting_table"){ UniqueId = 1022 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 9, 1),
					},
					new List<Item>
					{
						new Item(351, 6, 1),
						new Item(35, 7, 1),
					}, "crafting_table"){ UniqueId = 1021 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 9, 1),
					},
					new List<Item>
					{
						new Item(351, 6, 1),
						new Item(35, 6, 1),
					}, "crafting_table"){ UniqueId = 1020 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 8, 1),
					},
					new List<Item>
					{
						new Item(351, 7, 1),
						new Item(35, 15, 1),
					}, "crafting_table"){ UniqueId = 1013 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 8, 1),
					},
					new List<Item>
					{
						new Item(351, 7, 1),
						new Item(35, 14, 1),
					}, "crafting_table"){ UniqueId = 1012 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 8, 1),
					},
					new List<Item>
					{
						new Item(351, 7, 1),
						new Item(35, 5, 1),
					}, "crafting_table"){ UniqueId = 1004 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 8, 1),
					},
					new List<Item>
					{
						new Item(351, 7, 1),
						new Item(35, 4, 1),
					}, "crafting_table"){ UniqueId = 1003 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 8, 1),
					},
					new List<Item>
					{
						new Item(351, 7, 1),
						new Item(35, 3, 1),
					}, "crafting_table"){ UniqueId = 1002 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 8, 1),
					},
					new List<Item>
					{
						new Item(351, 7, 1),
						new Item(35, 2, 1),
					}, "crafting_table"){ UniqueId = 1001 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 8, 1),
					},
					new List<Item>
					{
						new Item(351, 7, 1),
						new Item(35, 1, 1),
					}, "crafting_table"){ UniqueId = 1000 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 8, 1),
					},
					new List<Item>
					{
						new Item(351, 7, 1),
						new Item(35, 0, 1),
					}, "crafting_table"){ UniqueId = 999 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 8, 1),
					},
					new List<Item>
					{
						new Item(351, 7, 1),
						new Item(35, 13, 1),
					}, "crafting_table"){ UniqueId = 1011 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 8, 1),
					},
					new List<Item>
					{
						new Item(351, 7, 1),
						new Item(35, 12, 1),
					}, "crafting_table"){ UniqueId = 1010 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 8, 1),
					},
					new List<Item>
					{
						new Item(351, 7, 1),
						new Item(35, 11, 1),
					}, "crafting_table"){ UniqueId = 1009 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 8, 1),
					},
					new List<Item>
					{
						new Item(351, 7, 1),
						new Item(35, 10, 1),
					}, "crafting_table"){ UniqueId = 1008 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 8, 1),
					},
					new List<Item>
					{
						new Item(351, 7, 1),
						new Item(35, 9, 1),
					}, "crafting_table"){ UniqueId = 1007 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 8, 1),
					},
					new List<Item>
					{
						new Item(351, 7, 1),
						new Item(35, 7, 1),
					}, "crafting_table"){ UniqueId = 1006 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 8, 1),
					},
					new List<Item>
					{
						new Item(351, 7, 1),
						new Item(35, 6, 1),
					}, "crafting_table"){ UniqueId = 1005 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 7, 1),
					},
					new List<Item>
					{
						new Item(351, 8, 1),
						new Item(35, 15, 1),
					}, "crafting_table"){ UniqueId = 998 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 7, 1),
					},
					new List<Item>
					{
						new Item(351, 8, 1),
						new Item(35, 14, 1),
					}, "crafting_table"){ UniqueId = 997 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 7, 1),
					},
					new List<Item>
					{
						new Item(351, 8, 1),
						new Item(35, 5, 1),
					}, "crafting_table"){ UniqueId = 989 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 7, 1),
					},
					new List<Item>
					{
						new Item(351, 8, 1),
						new Item(35, 4, 1),
					}, "crafting_table"){ UniqueId = 988 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 7, 1),
					},
					new List<Item>
					{
						new Item(351, 8, 1),
						new Item(35, 3, 1),
					}, "crafting_table"){ UniqueId = 987 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 7, 1),
					},
					new List<Item>
					{
						new Item(351, 8, 1),
						new Item(35, 2, 1),
					}, "crafting_table"){ UniqueId = 986 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 7, 1),
					},
					new List<Item>
					{
						new Item(351, 8, 1),
						new Item(35, 1, 1),
					}, "crafting_table"){ UniqueId = 985 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 7, 1),
					},
					new List<Item>
					{
						new Item(351, 8, 1),
						new Item(35, 0, 1),
					}, "crafting_table"){ UniqueId = 984 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 7, 1),
					},
					new List<Item>
					{
						new Item(351, 8, 1),
						new Item(35, 13, 1),
					}, "crafting_table"){ UniqueId = 996 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 7, 1),
					},
					new List<Item>
					{
						new Item(351, 8, 1),
						new Item(35, 12, 1),
					}, "crafting_table"){ UniqueId = 995 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 7, 1),
					},
					new List<Item>
					{
						new Item(351, 8, 1),
						new Item(35, 11, 1),
					}, "crafting_table"){ UniqueId = 994 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 7, 1),
					},
					new List<Item>
					{
						new Item(351, 8, 1),
						new Item(35, 10, 1),
					}, "crafting_table"){ UniqueId = 993 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 7, 1),
					},
					new List<Item>
					{
						new Item(351, 8, 1),
						new Item(35, 9, 1),
					}, "crafting_table"){ UniqueId = 992 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 7, 1),
					},
					new List<Item>
					{
						new Item(351, 8, 1),
						new Item(35, 8, 1),
					}, "crafting_table"){ UniqueId = 991 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 7, 1),
					},
					new List<Item>
					{
						new Item(351, 8, 1),
						new Item(35, 6, 1),
					}, "crafting_table"){ UniqueId = 990 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 6, 1),
					},
					new List<Item>
					{
						new Item(351, 9, 1),
						new Item(35, 15, 1),
					}, "crafting_table"){ UniqueId = 983 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 6, 1),
					},
					new List<Item>
					{
						new Item(351, 9, 1),
						new Item(35, 14, 1),
					}, "crafting_table"){ UniqueId = 982 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 6, 1),
					},
					new List<Item>
					{
						new Item(351, 9, 1),
						new Item(35, 5, 1),
					}, "crafting_table"){ UniqueId = 974 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 6, 1),
					},
					new List<Item>
					{
						new Item(351, 9, 1),
						new Item(35, 4, 1),
					}, "crafting_table"){ UniqueId = 973 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 6, 1),
					},
					new List<Item>
					{
						new Item(351, 9, 1),
						new Item(35, 3, 1),
					}, "crafting_table"){ UniqueId = 972 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 6, 1),
					},
					new List<Item>
					{
						new Item(351, 9, 1),
						new Item(35, 2, 1),
					}, "crafting_table"){ UniqueId = 971 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 6, 1),
					},
					new List<Item>
					{
						new Item(351, 9, 1),
						new Item(35, 1, 1),
					}, "crafting_table"){ UniqueId = 970 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 6, 1),
					},
					new List<Item>
					{
						new Item(351, 9, 1),
						new Item(35, 0, 1),
					}, "crafting_table"){ UniqueId = 969 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 6, 1),
					},
					new List<Item>
					{
						new Item(351, 9, 1),
						new Item(35, 13, 1),
					}, "crafting_table"){ UniqueId = 981 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 6, 1),
					},
					new List<Item>
					{
						new Item(351, 9, 1),
						new Item(35, 12, 1),
					}, "crafting_table"){ UniqueId = 980 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 6, 1),
					},
					new List<Item>
					{
						new Item(351, 9, 1),
						new Item(35, 11, 1),
					}, "crafting_table"){ UniqueId = 979 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 6, 1),
					},
					new List<Item>
					{
						new Item(351, 9, 1),
						new Item(35, 10, 1),
					}, "crafting_table"){ UniqueId = 978 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 6, 1),
					},
					new List<Item>
					{
						new Item(351, 9, 1),
						new Item(35, 9, 1),
					}, "crafting_table"){ UniqueId = 977 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 6, 1),
					},
					new List<Item>
					{
						new Item(351, 9, 1),
						new Item(35, 8, 1),
					}, "crafting_table"){ UniqueId = 976 },
				new ShapelessRecipe(
					new List<Item>
					{
						new Item(35, 6, 1),
					},
					new List<Item>
					{
						new Item(351, 9, 1),
						new Item(35, 7, 1),
					}, "crafting_table"){ UniqueId = 975 },
				new SmeltingRecipe(new Item(266, -1, 1), new Item(-288, 32767), "furnace"),
				new SmeltingRecipe(new Item(266, -1, 1), new Item(-288, 32767), "blast_furnace"),
				new SmeltingRecipe(new Item(-280, -1, 1), new Item(-274, 32767), "furnace"),
				new SmeltingRecipe(new Item(752, -1, 1), new Item(-271, 32767), "furnace"),
				new SmeltingRecipe(new Item(752, -1, 1), new Item(-271, 32767), "blast_furnace"),
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
				new SmeltingRecipe(new Item(-303, -1, 1), new Item(112, 32767), "furnace"),
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
				new SmeltingRecipe(new Item(452, -1, 1), new Item(256, 32767), "furnace"),
				new SmeltingRecipe(new Item(452, -1, 1), new Item(256, 32767), "blast_furnace"),
				new SmeltingRecipe(new Item(452, -1, 1), new Item(257, 32767), "furnace"),
				new SmeltingRecipe(new Item(452, -1, 1), new Item(257, 32767), "blast_furnace"),
				new SmeltingRecipe(new Item(452, -1, 1), new Item(258, 32767), "furnace"),
				new SmeltingRecipe(new Item(452, -1, 1), new Item(258, 32767), "blast_furnace"),
				new SmeltingRecipe(new Item(452, -1, 1), new Item(267, 32767), "furnace"),
				new SmeltingRecipe(new Item(452, -1, 1), new Item(267, 32767), "blast_furnace"),
				new SmeltingRecipe(new Item(371, -1, 1), new Item(283, 32767), "furnace"),
				new SmeltingRecipe(new Item(371, -1, 1), new Item(283, 32767), "blast_furnace"),
				new SmeltingRecipe(new Item(371, -1, 1), new Item(284, 32767), "furnace"),
				new SmeltingRecipe(new Item(371, -1, 1), new Item(284, 32767), "blast_furnace"),
				new SmeltingRecipe(new Item(371, -1, 1), new Item(285, 32767), "furnace"),
				new SmeltingRecipe(new Item(371, -1, 1), new Item(285, 32767), "blast_furnace"),
				new SmeltingRecipe(new Item(371, -1, 1), new Item(286, 32767), "furnace"),
				new SmeltingRecipe(new Item(371, -1, 1), new Item(286, 32767), "blast_furnace"),
				new SmeltingRecipe(new Item(452, -1, 1), new Item(292, 32767), "furnace"),
				new SmeltingRecipe(new Item(452, -1, 1), new Item(292, 32767), "blast_furnace"),
				new SmeltingRecipe(new Item(371, -1, 1), new Item(294, 32767), "furnace"),
				new SmeltingRecipe(new Item(371, -1, 1), new Item(294, 32767), "blast_furnace"),
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
				new SmeltingRecipe(new Item(320, -1, 1), new Item(319, 32767), "soul_campfire"),
				new SmeltingRecipe(new Item(320, -1, 1), new Item(319, 32767), "campfire"),
				new SmeltingRecipe(new Item(464, -1, 1), new Item(335, 32767), "smoker"),
				new SmeltingRecipe(new Item(464, -1, 1), new Item(335, 32767), "furnace"),
				new SmeltingRecipe(new Item(464, -1, 1), new Item(335, 32767), "soul_campfire"),
				new SmeltingRecipe(new Item(464, -1, 1), new Item(335, 32767), "campfire"),
				new SmeltingRecipe(new Item(336, -1, 1), new Item(337, 32767), "furnace"),
				new SmeltingRecipe(new Item(350, -1, 1), new Item(349, 32767), "smoker"),
				new SmeltingRecipe(new Item(350, -1, 1), new Item(349, 32767), "furnace"),
				new SmeltingRecipe(new Item(350, -1, 1), new Item(349, 32767), "soul_campfire"),
				new SmeltingRecipe(new Item(350, -1, 1), new Item(349, 32767), "campfire"),
				new SmeltingRecipe(new Item(364, -1, 1), new Item(363, 32767), "smoker"),
				new SmeltingRecipe(new Item(364, -1, 1), new Item(363, 32767), "furnace"),
				new SmeltingRecipe(new Item(364, -1, 1), new Item(363, 32767), "soul_campfire"),
				new SmeltingRecipe(new Item(364, -1, 1), new Item(363, 32767), "campfire"),
				new SmeltingRecipe(new Item(366, -1, 1), new Item(365, 32767), "smoker"),
				new SmeltingRecipe(new Item(366, -1, 1), new Item(365, 32767), "furnace"),
				new SmeltingRecipe(new Item(366, -1, 1), new Item(365, 32767), "soul_campfire"),
				new SmeltingRecipe(new Item(366, -1, 1), new Item(365, 32767), "campfire"),
				new SmeltingRecipe(new Item(393, -1, 1), new Item(392, 32767), "smoker"),
				new SmeltingRecipe(new Item(393, -1, 1), new Item(392, 32767), "furnace"),
				new SmeltingRecipe(new Item(393, -1, 1), new Item(392, 32767), "soul_campfire"),
				new SmeltingRecipe(new Item(393, -1, 1), new Item(392, 32767), "campfire"),
				new SmeltingRecipe(new Item(412, -1, 1), new Item(411, 32767), "smoker"),
				new SmeltingRecipe(new Item(412, -1, 1), new Item(411, 32767), "furnace"),
				new SmeltingRecipe(new Item(412, -1, 1), new Item(411, 32767), "soul_campfire"),
				new SmeltingRecipe(new Item(412, -1, 1), new Item(411, 32767), "campfire"),
				new SmeltingRecipe(new Item(452, -1, 1), new Item(417, 32767), "furnace"),
				new SmeltingRecipe(new Item(452, -1, 1), new Item(417, 32767), "blast_furnace"),
				new SmeltingRecipe(new Item(371, -1, 1), new Item(418, 32767), "furnace"),
				new SmeltingRecipe(new Item(371, -1, 1), new Item(418, 32767), "blast_furnace"),
				new SmeltingRecipe(new Item(424, -1, 1), new Item(423, 32767), "smoker"),
				new SmeltingRecipe(new Item(424, -1, 1), new Item(423, 32767), "furnace"),
				new SmeltingRecipe(new Item(424, -1, 1), new Item(423, 32767), "soul_campfire"),
				new SmeltingRecipe(new Item(424, -1, 1), new Item(423, 32767), "campfire"),
				new SmeltingRecipe(new Item(433, -1, 1), new Item(432, 32767), "furnace"),
				new SmeltingRecipe(new Item(463, -1, 1), new Item(460, 32767), "smoker"),
				new SmeltingRecipe(new Item(463, -1, 1), new Item(460, 32767), "furnace"),
				new SmeltingRecipe(new Item(463, -1, 1), new Item(460, 32767), "soul_campfire"),
				new SmeltingRecipe(new Item(463, -1, 1), new Item(460, 32767), "campfire"),
			};
		}

		public static void Add(Recipe recipe)
		{
			Log.InfoFormat("{0}", recipe.Id);
		}
	}
}