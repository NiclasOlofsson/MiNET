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
		new ShapedRecipe(3, 3, new ItemStack(ItemFactory.GetItem(270, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(5, -1),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(5, -1),
				ItemFactory.GetItem(280, 0),
				ItemFactory.GetItem(280, 0),
				ItemFactory.GetItem(5, -1),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(0, 0),
			}),
		new ShapedRecipe(1, 3, new ItemStack(ItemFactory.GetItem(269, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(5, -1),
				ItemFactory.GetItem(280, 0),
				ItemFactory.GetItem(280, 0),
			}),
		new ShapedRecipe(2, 3, new ItemStack(ItemFactory.GetItem(271, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(5, -1),
				ItemFactory.GetItem(280, 0),
				ItemFactory.GetItem(5, -1),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(5, -1),
				ItemFactory.GetItem(280, 0),
			}),
		new ShapedRecipe(2, 3, new ItemStack(ItemFactory.GetItem(290, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(5, -1),
				ItemFactory.GetItem(280, 0),
				ItemFactory.GetItem(5, -1),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(280, 0),
			}),
		new ShapedRecipe(3, 3, new ItemStack(ItemFactory.GetItem(274, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(4, -1),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(4, -1),
				ItemFactory.GetItem(280, 0),
				ItemFactory.GetItem(280, 0),
				ItemFactory.GetItem(4, -1),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(0, 0),
			}),
		new ShapedRecipe(1, 3, new ItemStack(ItemFactory.GetItem(273, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(4, -1),
				ItemFactory.GetItem(280, 0),
				ItemFactory.GetItem(280, 0),
			}),
		new ShapedRecipe(2, 3, new ItemStack(ItemFactory.GetItem(275, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(4, -1),
				ItemFactory.GetItem(280, 0),
				ItemFactory.GetItem(4, -1),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(4, -1),
				ItemFactory.GetItem(280, 0),
			}),
		new ShapedRecipe(2, 3, new ItemStack(ItemFactory.GetItem(291, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(4, -1),
				ItemFactory.GetItem(280, 0),
				ItemFactory.GetItem(4, -1),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(280, 0),
			}),
		new ShapedRecipe(3, 3, new ItemStack(ItemFactory.GetItem(257, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(265, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(265, 0),
				ItemFactory.GetItem(280, 0),
				ItemFactory.GetItem(280, 0),
				ItemFactory.GetItem(265, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(0, 0),
			}),
		new ShapedRecipe(1, 3, new ItemStack(ItemFactory.GetItem(256, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(265, 0),
				ItemFactory.GetItem(280, 0),
				ItemFactory.GetItem(280, 0),
			}),
		new ShapedRecipe(2, 3, new ItemStack(ItemFactory.GetItem(258, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(265, 0),
				ItemFactory.GetItem(280, 0),
				ItemFactory.GetItem(265, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(265, 0),
				ItemFactory.GetItem(280, 0),
			}),
		new ShapedRecipe(2, 3, new ItemStack(ItemFactory.GetItem(292, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(265, 0),
				ItemFactory.GetItem(280, 0),
				ItemFactory.GetItem(265, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(280, 0),
			}),
		new ShapedRecipe(3, 3, new ItemStack(ItemFactory.GetItem(278, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(264, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(264, 0),
				ItemFactory.GetItem(280, 0),
				ItemFactory.GetItem(280, 0),
				ItemFactory.GetItem(264, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(0, 0),
			}),
		new ShapedRecipe(1, 3, new ItemStack(ItemFactory.GetItem(277, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(264, 0),
				ItemFactory.GetItem(280, 0),
				ItemFactory.GetItem(280, 0),
			}),
		new ShapedRecipe(2, 3, new ItemStack(ItemFactory.GetItem(279, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(264, 0),
				ItemFactory.GetItem(280, 0),
				ItemFactory.GetItem(264, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(264, 0),
				ItemFactory.GetItem(280, 0),
			}),
		new ShapedRecipe(2, 3, new ItemStack(ItemFactory.GetItem(293, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(264, 0),
				ItemFactory.GetItem(280, 0),
				ItemFactory.GetItem(264, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(280, 0),
			}),
		new ShapedRecipe(3, 3, new ItemStack(ItemFactory.GetItem(285, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(266, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(266, 0),
				ItemFactory.GetItem(280, 0),
				ItemFactory.GetItem(280, 0),
				ItemFactory.GetItem(266, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(0, 0),
			}),
		new ShapedRecipe(1, 3, new ItemStack(ItemFactory.GetItem(284, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(266, 0),
				ItemFactory.GetItem(280, 0),
				ItemFactory.GetItem(280, 0),
			}),
		new ShapedRecipe(2, 3, new ItemStack(ItemFactory.GetItem(286, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(266, 0),
				ItemFactory.GetItem(280, 0),
				ItemFactory.GetItem(266, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(266, 0),
				ItemFactory.GetItem(280, 0),
			}),
		new ShapedRecipe(2, 3, new ItemStack(ItemFactory.GetItem(294, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(266, 0),
				ItemFactory.GetItem(280, 0),
				ItemFactory.GetItem(266, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(280, 0),
			}),
		new ShapedRecipe(2, 2, new ItemStack(ItemFactory.GetItem(359, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(265, 0),
				ItemFactory.GetItem(265, 0),
				ItemFactory.GetItem(0, 0),
			}),
		new ShapedRecipe(1, 3, new ItemStack(ItemFactory.GetItem(268, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(5, -1),
				ItemFactory.GetItem(5, -1),
				ItemFactory.GetItem(280, 0),
			}),
		new ShapedRecipe(1, 3, new ItemStack(ItemFactory.GetItem(272, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(4, -1),
				ItemFactory.GetItem(4, -1),
				ItemFactory.GetItem(280, 0),
			}),
		new ShapedRecipe(1, 3, new ItemStack(ItemFactory.GetItem(267, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(265, 0),
				ItemFactory.GetItem(265, 0),
				ItemFactory.GetItem(280, 0),
			}),
		new ShapedRecipe(1, 3, new ItemStack(ItemFactory.GetItem(276, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(264, 0),
				ItemFactory.GetItem(264, 0),
				ItemFactory.GetItem(280, 0),
			}),
		new ShapedRecipe(1, 3, new ItemStack(ItemFactory.GetItem(283, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(266, 0),
				ItemFactory.GetItem(266, 0),
				ItemFactory.GetItem(280, 0),
			}),
		new ShapedRecipe(3, 3, new ItemStack(ItemFactory.GetItem(261, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(280, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(280, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(280, 0),
				ItemFactory.GetItem(287, 0),
				ItemFactory.GetItem(287, 0),
				ItemFactory.GetItem(287, 0),
			}),
		new ShapedRecipe(1, 3, new ItemStack(ItemFactory.GetItem(262, 0), 4),
			new Item[]
			{
				ItemFactory.GetItem(318, 0),
				ItemFactory.GetItem(280, 0),
				ItemFactory.GetItem(288, 0),
			}),
		new ShapedRecipe(3, 3, new ItemStack(ItemFactory.GetItem(41, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(266, 0),
				ItemFactory.GetItem(266, 0),
				ItemFactory.GetItem(266, 0),
				ItemFactory.GetItem(266, 0),
				ItemFactory.GetItem(266, 0),
				ItemFactory.GetItem(266, 0),
				ItemFactory.GetItem(266, 0),
				ItemFactory.GetItem(266, 0),
				ItemFactory.GetItem(266, 0),
			}),
		new ShapedRecipe(1, 1, new ItemStack(ItemFactory.GetItem(266, 0), 9),
			new Item[]
			{
				ItemFactory.GetItem(41, -1),
			}),
		new ShapedRecipe(3, 3, new ItemStack(ItemFactory.GetItem(42, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(265, 0),
				ItemFactory.GetItem(265, 0),
				ItemFactory.GetItem(265, 0),
				ItemFactory.GetItem(265, 0),
				ItemFactory.GetItem(265, 0),
				ItemFactory.GetItem(265, 0),
				ItemFactory.GetItem(265, 0),
				ItemFactory.GetItem(265, 0),
				ItemFactory.GetItem(265, 0),
			}),
		new ShapedRecipe(1, 1, new ItemStack(ItemFactory.GetItem(265, 0), 9),
			new Item[]
			{
				ItemFactory.GetItem(42, -1),
			}),
		new ShapedRecipe(3, 3, new ItemStack(ItemFactory.GetItem(57, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(264, 0),
				ItemFactory.GetItem(264, 0),
				ItemFactory.GetItem(264, 0),
				ItemFactory.GetItem(264, 0),
				ItemFactory.GetItem(264, 0),
				ItemFactory.GetItem(264, 0),
				ItemFactory.GetItem(264, 0),
				ItemFactory.GetItem(264, 0),
				ItemFactory.GetItem(264, 0),
			}),
		new ShapedRecipe(1, 1, new ItemStack(ItemFactory.GetItem(264, 0), 9),
			new Item[]
			{
				ItemFactory.GetItem(57, -1),
			}),
		new ShapedRecipe(3, 3, new ItemStack(ItemFactory.GetItem(133, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(388, 0),
				ItemFactory.GetItem(388, 0),
				ItemFactory.GetItem(388, 0),
				ItemFactory.GetItem(388, 0),
				ItemFactory.GetItem(388, 0),
				ItemFactory.GetItem(388, 0),
				ItemFactory.GetItem(388, 0),
				ItemFactory.GetItem(388, 0),
				ItemFactory.GetItem(388, 0),
			}),
		new ShapedRecipe(1, 1, new ItemStack(ItemFactory.GetItem(388, 0), 9),
			new Item[]
			{
				ItemFactory.GetItem(133, -1),
			}),
		new ShapedRecipe(3, 3, new ItemStack(ItemFactory.GetItem(152, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(331, 0),
				ItemFactory.GetItem(331, 0),
				ItemFactory.GetItem(331, 0),
				ItemFactory.GetItem(331, 0),
				ItemFactory.GetItem(331, 0),
				ItemFactory.GetItem(331, 0),
				ItemFactory.GetItem(331, 0),
				ItemFactory.GetItem(331, 0),
				ItemFactory.GetItem(331, 0),
			}),
		new ShapedRecipe(1, 1, new ItemStack(ItemFactory.GetItem(331, 0), 9),
			new Item[]
			{
				ItemFactory.GetItem(152, -1),
			}),
		new ShapedRecipe(3, 3, new ItemStack(ItemFactory.GetItem(22, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(351, 4),
				ItemFactory.GetItem(351, 4),
				ItemFactory.GetItem(351, 4),
				ItemFactory.GetItem(351, 4),
				ItemFactory.GetItem(351, 4),
				ItemFactory.GetItem(351, 4),
				ItemFactory.GetItem(351, 4),
				ItemFactory.GetItem(351, 4),
				ItemFactory.GetItem(351, 4),
			}),
		new ShapedRecipe(1, 1, new ItemStack(ItemFactory.GetItem(351, 4), 9),
			new Item[]
			{
				ItemFactory.GetItem(22, -1),
			}),
		new ShapedRecipe(3, 3, new ItemStack(ItemFactory.GetItem(170, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(296, 0),
				ItemFactory.GetItem(296, 0),
				ItemFactory.GetItem(296, 0),
				ItemFactory.GetItem(296, 0),
				ItemFactory.GetItem(296, 0),
				ItemFactory.GetItem(296, 0),
				ItemFactory.GetItem(296, 0),
				ItemFactory.GetItem(296, 0),
				ItemFactory.GetItem(296, 0),
			}),
		new ShapedRecipe(1, 1, new ItemStack(ItemFactory.GetItem(296, 0), 9),
			new Item[]
			{
				ItemFactory.GetItem(170, -1),
			}),
		new ShapedRecipe(3, 3, new ItemStack(ItemFactory.GetItem(173, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(263, 0),
				ItemFactory.GetItem(263, 0),
				ItemFactory.GetItem(263, 0),
				ItemFactory.GetItem(263, 0),
				ItemFactory.GetItem(263, 0),
				ItemFactory.GetItem(263, 0),
				ItemFactory.GetItem(263, 0),
				ItemFactory.GetItem(263, 0),
				ItemFactory.GetItem(263, 0),
			}),
		new ShapedRecipe(1, 1, new ItemStack(ItemFactory.GetItem(263, 0), 9),
			new Item[]
			{
				ItemFactory.GetItem(173, -1),
			}),
		new ShapedRecipe(3, 3, new ItemStack(ItemFactory.GetItem(266, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(371, 0),
				ItemFactory.GetItem(371, 0),
				ItemFactory.GetItem(371, 0),
				ItemFactory.GetItem(371, 0),
				ItemFactory.GetItem(371, 0),
				ItemFactory.GetItem(371, 0),
				ItemFactory.GetItem(371, 0),
				ItemFactory.GetItem(371, 0),
				ItemFactory.GetItem(371, 0),
			}),
		new ShapedRecipe(1, 1, new ItemStack(ItemFactory.GetItem(371, 0), 9),
			new Item[]
			{
				ItemFactory.GetItem(266, 0),
			}),
		new ShapedRecipe(2, 2, new ItemStack(ItemFactory.GetItem(282, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(39, -1),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(40, -1),
				ItemFactory.GetItem(281, 0),
			}),
		new ShapelessRecipe(new ItemStack(ItemFactory.GetItem(282, 0), 1),
			new List<ItemStack>
			{
				new ItemStack(ItemFactory.GetItem(39, -1), 1),
				new ItemStack(ItemFactory.GetItem(40, -1), 1),
				new ItemStack(ItemFactory.GetItem(281, 0), 1),
			}),
		new ShapedRecipe(3, 3, new ItemStack(ItemFactory.GetItem(459, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(457, 0),
				ItemFactory.GetItem(457, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(457, 0),
				ItemFactory.GetItem(457, 0),
				ItemFactory.GetItem(281, 0),
				ItemFactory.GetItem(457, 0),
				ItemFactory.GetItem(457, 0),
				ItemFactory.GetItem(0, 0),
			}),
		new ShapedRecipe(3, 1, new ItemStack(ItemFactory.GetItem(357, 0), 8),
			new Item[]
			{
				ItemFactory.GetItem(296, 0),
				ItemFactory.GetItem(351, 3),
				ItemFactory.GetItem(296, 0),
			}),
		new ShapedRecipe(3, 3, new ItemStack(ItemFactory.GetItem(103, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(360, 0),
				ItemFactory.GetItem(360, 0),
				ItemFactory.GetItem(360, 0),
				ItemFactory.GetItem(360, 0),
				ItemFactory.GetItem(360, 0),
				ItemFactory.GetItem(360, 0),
				ItemFactory.GetItem(360, 0),
				ItemFactory.GetItem(360, 0),
				ItemFactory.GetItem(360, 0),
			}),
		new ShapedRecipe(1, 1, new ItemStack(ItemFactory.GetItem(362, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(360, 0),
			}),
		new ShapedRecipe(1, 1, new ItemStack(ItemFactory.GetItem(361, 0), 4),
			new Item[]
			{
				ItemFactory.GetItem(86, -1),
			}),
		new ShapedRecipe(2, 2, new ItemStack(ItemFactory.GetItem(400, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(86, -1),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(353, 0),
				ItemFactory.GetItem(344, 0),
			}),
		new ShapelessRecipe(new ItemStack(ItemFactory.GetItem(400, 0), 1),
			new List<ItemStack>
			{
				new ItemStack(ItemFactory.GetItem(86, -1), 1),
				new ItemStack(ItemFactory.GetItem(353, 0), 1),
				new ItemStack(ItemFactory.GetItem(344, 0), 1),
			}),
		new ShapedRecipe(2, 2, new ItemStack(ItemFactory.GetItem(376, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(375, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(353, 0),
				ItemFactory.GetItem(39, -1),
			}),
		new ShapedRecipe(3, 3, new ItemStack(ItemFactory.GetItem(54, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(5, -1),
				ItemFactory.GetItem(5, -1),
				ItemFactory.GetItem(5, -1),
				ItemFactory.GetItem(5, -1),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(5, -1),
				ItemFactory.GetItem(5, -1),
				ItemFactory.GetItem(5, -1),
				ItemFactory.GetItem(5, -1),
			}),
		new ShapedRecipe(2, 1, new ItemStack(ItemFactory.GetItem(146, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(131, -1),
				ItemFactory.GetItem(54, -1),
			}),
		new ShapedRecipe(3, 3, new ItemStack(ItemFactory.GetItem(61, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(4, -1),
				ItemFactory.GetItem(4, -1),
				ItemFactory.GetItem(4, -1),
				ItemFactory.GetItem(4, -1),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(4, -1),
				ItemFactory.GetItem(4, -1),
				ItemFactory.GetItem(4, -1),
				ItemFactory.GetItem(4, -1),
			}),
		new ShapedRecipe(2, 2, new ItemStack(ItemFactory.GetItem(58, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(5, -1),
				ItemFactory.GetItem(5, -1),
				ItemFactory.GetItem(5, -1),
				ItemFactory.GetItem(5, -1),
			}),
		new ShapedRecipe(2, 2, new ItemStack(ItemFactory.GetItem(24, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(12, 0),
				ItemFactory.GetItem(12, 0),
				ItemFactory.GetItem(12, 0),
				ItemFactory.GetItem(12, 0),
			}),
		new ShapedRecipe(2, 2, new ItemStack(ItemFactory.GetItem(24, 2), 4),
			new Item[]
			{
				ItemFactory.GetItem(24, -1),
				ItemFactory.GetItem(24, -1),
				ItemFactory.GetItem(24, -1),
				ItemFactory.GetItem(24, -1),
			}),
		new ShapedRecipe(1, 2, new ItemStack(ItemFactory.GetItem(24, 1), 1),
			new Item[]
			{
				ItemFactory.GetItem(44, 1),
				ItemFactory.GetItem(44, 1),
			}),
		new ShapedRecipe(2, 2, new ItemStack(ItemFactory.GetItem(98, 0), 4),
			new Item[]
			{
				ItemFactory.GetItem(1, 0),
				ItemFactory.GetItem(1, 0),
				ItemFactory.GetItem(1, 0),
				ItemFactory.GetItem(1, 0),
			}),
		new ShapedRecipe(2, 1, new ItemStack(ItemFactory.GetItem(98, 1), 1),
			new Item[]
			{
				ItemFactory.GetItem(98, -1),
				ItemFactory.GetItem(106, -1),
			}),
		new ShapelessRecipe(new ItemStack(ItemFactory.GetItem(98, 1), 1),
			new List<ItemStack>
			{
				new ItemStack(ItemFactory.GetItem(98, -1), 1),
				new ItemStack(ItemFactory.GetItem(106, -1), 1),
			}),
		new ShapedRecipe(1, 2, new ItemStack(ItemFactory.GetItem(98, 3), 1),
			new Item[]
			{
				ItemFactory.GetItem(44, 5),
				ItemFactory.GetItem(44, 5),
			}),
		new ShapedRecipe(2, 1, new ItemStack(ItemFactory.GetItem(48, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(4, -1),
				ItemFactory.GetItem(106, -1),
			}),
		new ShapelessRecipe(new ItemStack(ItemFactory.GetItem(48, 0), 1),
			new List<ItemStack>
			{
				new ItemStack(ItemFactory.GetItem(4, -1), 1),
				new ItemStack(ItemFactory.GetItem(106, -1), 1),
			}),
		new ShapedRecipe(3, 2, new ItemStack(ItemFactory.GetItem(101, 0), 16),
			new Item[]
			{
				ItemFactory.GetItem(265, 0),
				ItemFactory.GetItem(265, 0),
				ItemFactory.GetItem(265, 0),
				ItemFactory.GetItem(265, 0),
				ItemFactory.GetItem(265, 0),
				ItemFactory.GetItem(265, 0),
			}),
		new ShapedRecipe(3, 2, new ItemStack(ItemFactory.GetItem(102, 0), 16),
			new Item[]
			{
				ItemFactory.GetItem(20, -1),
				ItemFactory.GetItem(20, -1),
				ItemFactory.GetItem(20, -1),
				ItemFactory.GetItem(20, -1),
				ItemFactory.GetItem(20, -1),
				ItemFactory.GetItem(20, -1),
			}),
		new ShapedRecipe(2, 2, new ItemStack(ItemFactory.GetItem(112, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(405, 0),
				ItemFactory.GetItem(405, 0),
				ItemFactory.GetItem(405, 0),
				ItemFactory.GetItem(405, 0),
			}),
		new ShapedRecipe(2, 2, new ItemStack(ItemFactory.GetItem(155, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(406, 0),
				ItemFactory.GetItem(406, 0),
				ItemFactory.GetItem(406, 0),
				ItemFactory.GetItem(406, 0),
			}),
		new ShapedRecipe(1, 2, new ItemStack(ItemFactory.GetItem(155, 1), 1),
			new Item[]
			{
				ItemFactory.GetItem(44, 6),
				ItemFactory.GetItem(44, 6),
			}),
		new ShapedRecipe(1, 2, new ItemStack(ItemFactory.GetItem(155, 2), 2),
			new Item[]
			{
				ItemFactory.GetItem(155, 0),
				ItemFactory.GetItem(155, 0),
			}),
		new ShapedRecipe(2, 2, new ItemStack(ItemFactory.GetItem(1, 3), 2),
			new Item[]
			{
				ItemFactory.GetItem(4, -1),
				ItemFactory.GetItem(406, 0),
				ItemFactory.GetItem(406, 0),
				ItemFactory.GetItem(4, -1),
			}),
		new ShapedRecipe(2, 1, new ItemStack(ItemFactory.GetItem(1, 1), 1),
			new Item[]
			{
				ItemFactory.GetItem(1, 3),
				ItemFactory.GetItem(406, 0),
			}),
		new ShapelessRecipe(new ItemStack(ItemFactory.GetItem(1, 1), 1),
			new List<ItemStack>
			{
				new ItemStack(ItemFactory.GetItem(1, 3), 1),
				new ItemStack(ItemFactory.GetItem(406, 0), 1),
			}),
		new ShapedRecipe(2, 1, new ItemStack(ItemFactory.GetItem(1, 5), 2),
			new Item[]
			{
				ItemFactory.GetItem(1, 3),
				ItemFactory.GetItem(4, -1),
			}),
		new ShapelessRecipe(new ItemStack(ItemFactory.GetItem(1, 5), 2),
			new List<ItemStack>
			{
				new ItemStack(ItemFactory.GetItem(1, 3), 1),
				new ItemStack(ItemFactory.GetItem(4, -1), 1),
			}),
		new ShapedRecipe(2, 2, new ItemStack(ItemFactory.GetItem(1, 4), 4),
			new Item[]
			{
				ItemFactory.GetItem(1, 3),
				ItemFactory.GetItem(1, 3),
				ItemFactory.GetItem(1, 3),
				ItemFactory.GetItem(1, 3),
			}),
		new ShapedRecipe(2, 2, new ItemStack(ItemFactory.GetItem(1, 2), 4),
			new Item[]
			{
				ItemFactory.GetItem(1, 1),
				ItemFactory.GetItem(1, 1),
				ItemFactory.GetItem(1, 1),
				ItemFactory.GetItem(1, 1),
			}),
		new ShapedRecipe(2, 2, new ItemStack(ItemFactory.GetItem(1, 6), 4),
			new Item[]
			{
				ItemFactory.GetItem(1, 5),
				ItemFactory.GetItem(1, 5),
				ItemFactory.GetItem(1, 5),
				ItemFactory.GetItem(1, 5),
			}),
		new ShapedRecipe(3, 2, new ItemStack(ItemFactory.GetItem(298, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(334, 0),
				ItemFactory.GetItem(334, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(334, 0),
				ItemFactory.GetItem(334, 0),
				ItemFactory.GetItem(334, 0),
			}),
		new ShapedRecipe(3, 3, new ItemStack(ItemFactory.GetItem(299, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(334, 0),
				ItemFactory.GetItem(334, 0),
				ItemFactory.GetItem(334, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(334, 0),
				ItemFactory.GetItem(334, 0),
				ItemFactory.GetItem(334, 0),
				ItemFactory.GetItem(334, 0),
				ItemFactory.GetItem(334, 0),
			}),
		new ShapedRecipe(3, 3, new ItemStack(ItemFactory.GetItem(300, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(334, 0),
				ItemFactory.GetItem(334, 0),
				ItemFactory.GetItem(334, 0),
				ItemFactory.GetItem(334, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(334, 0),
				ItemFactory.GetItem(334, 0),
				ItemFactory.GetItem(334, 0),
			}),
		new ShapedRecipe(3, 2, new ItemStack(ItemFactory.GetItem(301, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(334, 0),
				ItemFactory.GetItem(334, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(334, 0),
				ItemFactory.GetItem(334, 0),
			}),
		new ShapedRecipe(3, 2, new ItemStack(ItemFactory.GetItem(306, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(265, 0),
				ItemFactory.GetItem(265, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(265, 0),
				ItemFactory.GetItem(265, 0),
				ItemFactory.GetItem(265, 0),
			}),
		new ShapedRecipe(3, 3, new ItemStack(ItemFactory.GetItem(307, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(265, 0),
				ItemFactory.GetItem(265, 0),
				ItemFactory.GetItem(265, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(265, 0),
				ItemFactory.GetItem(265, 0),
				ItemFactory.GetItem(265, 0),
				ItemFactory.GetItem(265, 0),
				ItemFactory.GetItem(265, 0),
			}),
		new ShapedRecipe(3, 3, new ItemStack(ItemFactory.GetItem(308, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(265, 0),
				ItemFactory.GetItem(265, 0),
				ItemFactory.GetItem(265, 0),
				ItemFactory.GetItem(265, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(265, 0),
				ItemFactory.GetItem(265, 0),
				ItemFactory.GetItem(265, 0),
			}),
		new ShapedRecipe(3, 2, new ItemStack(ItemFactory.GetItem(309, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(265, 0),
				ItemFactory.GetItem(265, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(265, 0),
				ItemFactory.GetItem(265, 0),
			}),
		new ShapedRecipe(3, 2, new ItemStack(ItemFactory.GetItem(310, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(264, 0),
				ItemFactory.GetItem(264, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(264, 0),
				ItemFactory.GetItem(264, 0),
				ItemFactory.GetItem(264, 0),
			}),
		new ShapedRecipe(3, 3, new ItemStack(ItemFactory.GetItem(311, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(264, 0),
				ItemFactory.GetItem(264, 0),
				ItemFactory.GetItem(264, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(264, 0),
				ItemFactory.GetItem(264, 0),
				ItemFactory.GetItem(264, 0),
				ItemFactory.GetItem(264, 0),
				ItemFactory.GetItem(264, 0),
			}),
		new ShapedRecipe(3, 3, new ItemStack(ItemFactory.GetItem(312, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(264, 0),
				ItemFactory.GetItem(264, 0),
				ItemFactory.GetItem(264, 0),
				ItemFactory.GetItem(264, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(264, 0),
				ItemFactory.GetItem(264, 0),
				ItemFactory.GetItem(264, 0),
			}),
		new ShapedRecipe(3, 2, new ItemStack(ItemFactory.GetItem(313, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(264, 0),
				ItemFactory.GetItem(264, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(264, 0),
				ItemFactory.GetItem(264, 0),
			}),
		new ShapedRecipe(3, 2, new ItemStack(ItemFactory.GetItem(314, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(266, 0),
				ItemFactory.GetItem(266, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(266, 0),
				ItemFactory.GetItem(266, 0),
				ItemFactory.GetItem(266, 0),
			}),
		new ShapedRecipe(3, 3, new ItemStack(ItemFactory.GetItem(315, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(266, 0),
				ItemFactory.GetItem(266, 0),
				ItemFactory.GetItem(266, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(266, 0),
				ItemFactory.GetItem(266, 0),
				ItemFactory.GetItem(266, 0),
				ItemFactory.GetItem(266, 0),
				ItemFactory.GetItem(266, 0),
			}),
		new ShapedRecipe(3, 3, new ItemStack(ItemFactory.GetItem(316, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(266, 0),
				ItemFactory.GetItem(266, 0),
				ItemFactory.GetItem(266, 0),
				ItemFactory.GetItem(266, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(266, 0),
				ItemFactory.GetItem(266, 0),
				ItemFactory.GetItem(266, 0),
			}),
		new ShapedRecipe(3, 2, new ItemStack(ItemFactory.GetItem(317, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(266, 0),
				ItemFactory.GetItem(266, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(266, 0),
				ItemFactory.GetItem(266, 0),
			}),
		new ShapedRecipe(2, 1, new ItemStack(ItemFactory.GetItem(35, 15), 1),
			new Item[]
			{
				ItemFactory.GetItem(351, 0),
				ItemFactory.GetItem(35, 0),
			}),
		new ShapedRecipe(2, 1, new ItemStack(ItemFactory.GetItem(35, 14), 1),
			new Item[]
			{
				ItemFactory.GetItem(351, 1),
				ItemFactory.GetItem(35, 0),
			}),
		new ShapedRecipe(2, 1, new ItemStack(ItemFactory.GetItem(35, 13), 1),
			new Item[]
			{
				ItemFactory.GetItem(351, 2),
				ItemFactory.GetItem(35, 0),
			}),
		new ShapedRecipe(2, 1, new ItemStack(ItemFactory.GetItem(35, 12), 1),
			new Item[]
			{
				ItemFactory.GetItem(351, 3),
				ItemFactory.GetItem(35, 0),
			}),
		new ShapedRecipe(2, 1, new ItemStack(ItemFactory.GetItem(35, 11), 1),
			new Item[]
			{
				ItemFactory.GetItem(351, 4),
				ItemFactory.GetItem(35, 0),
			}),
		new ShapedRecipe(2, 1, new ItemStack(ItemFactory.GetItem(35, 10), 1),
			new Item[]
			{
				ItemFactory.GetItem(351, 5),
				ItemFactory.GetItem(35, 0),
			}),
		new ShapedRecipe(2, 1, new ItemStack(ItemFactory.GetItem(35, 9), 1),
			new Item[]
			{
				ItemFactory.GetItem(351, 6),
				ItemFactory.GetItem(35, 0),
			}),
		new ShapedRecipe(2, 1, new ItemStack(ItemFactory.GetItem(35, 8), 1),
			new Item[]
			{
				ItemFactory.GetItem(351, 7),
				ItemFactory.GetItem(35, 0),
			}),
		new ShapedRecipe(2, 1, new ItemStack(ItemFactory.GetItem(35, 7), 1),
			new Item[]
			{
				ItemFactory.GetItem(351, 8),
				ItemFactory.GetItem(35, 0),
			}),
		new ShapedRecipe(2, 1, new ItemStack(ItemFactory.GetItem(35, 6), 1),
			new Item[]
			{
				ItemFactory.GetItem(351, 9),
				ItemFactory.GetItem(35, 0),
			}),
		new ShapedRecipe(2, 1, new ItemStack(ItemFactory.GetItem(35, 5), 1),
			new Item[]
			{
				ItemFactory.GetItem(351, 10),
				ItemFactory.GetItem(35, 0),
			}),
		new ShapedRecipe(2, 1, new ItemStack(ItemFactory.GetItem(35, 4), 1),
			new Item[]
			{
				ItemFactory.GetItem(351, 11),
				ItemFactory.GetItem(35, 0),
			}),
		new ShapedRecipe(2, 1, new ItemStack(ItemFactory.GetItem(35, 3), 1),
			new Item[]
			{
				ItemFactory.GetItem(351, 12),
				ItemFactory.GetItem(35, 0),
			}),
		new ShapedRecipe(2, 1, new ItemStack(ItemFactory.GetItem(35, 2), 1),
			new Item[]
			{
				ItemFactory.GetItem(351, 13),
				ItemFactory.GetItem(35, 0),
			}),
		new ShapedRecipe(2, 1, new ItemStack(ItemFactory.GetItem(35, 1), 1),
			new Item[]
			{
				ItemFactory.GetItem(351, 14),
				ItemFactory.GetItem(35, 0),
			}),
		new ShapedRecipe(2, 1, new ItemStack(ItemFactory.GetItem(35, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(351, 15),
				ItemFactory.GetItem(35, 0),
			}),
		new ShapedRecipe(1, 1, new ItemStack(ItemFactory.GetItem(351, 11), 1),
			new Item[]
			{
				ItemFactory.GetItem(37, 0),
			}),
		new ShapedRecipe(1, 1, new ItemStack(ItemFactory.GetItem(351, 1), 2),
			new Item[]
			{
				ItemFactory.GetItem(244, 0),
			}),
		new ShapedRecipe(1, 1, new ItemStack(ItemFactory.GetItem(351, 1), 1),
			new Item[]
			{
				ItemFactory.GetItem(38, 0),
			}),
		new ShapedRecipe(1, 1, new ItemStack(ItemFactory.GetItem(351, 15), 3),
			new Item[]
			{
				ItemFactory.GetItem(352, 0),
			}),
		new ShapedRecipe(2, 1, new ItemStack(ItemFactory.GetItem(351, 9), 2),
			new Item[]
			{
				ItemFactory.GetItem(351, 1),
				ItemFactory.GetItem(351, 15),
			}),
		new ShapelessRecipe(new ItemStack(ItemFactory.GetItem(351, 9), 2),
			new List<ItemStack>
			{
				new ItemStack(ItemFactory.GetItem(351, 1), 1),
				new ItemStack(ItemFactory.GetItem(351, 15), 1),
			}),
		new ShapedRecipe(2, 1, new ItemStack(ItemFactory.GetItem(351, 14), 2),
			new Item[]
			{
				ItemFactory.GetItem(351, 1),
				ItemFactory.GetItem(351, 11),
			}),
		new ShapelessRecipe(new ItemStack(ItemFactory.GetItem(351, 14), 2),
			new List<ItemStack>
			{
				new ItemStack(ItemFactory.GetItem(351, 1), 1),
				new ItemStack(ItemFactory.GetItem(351, 11), 1),
			}),
		new ShapedRecipe(2, 1, new ItemStack(ItemFactory.GetItem(351, 10), 2),
			new Item[]
			{
				ItemFactory.GetItem(351, 2),
				ItemFactory.GetItem(351, 15),
			}),
		new ShapelessRecipe(new ItemStack(ItemFactory.GetItem(351, 10), 2),
			new List<ItemStack>
			{
				new ItemStack(ItemFactory.GetItem(351, 2), 1),
				new ItemStack(ItemFactory.GetItem(351, 15), 1),
			}),
		new ShapedRecipe(2, 1, new ItemStack(ItemFactory.GetItem(351, 8), 2),
			new Item[]
			{
				ItemFactory.GetItem(351, 0),
				ItemFactory.GetItem(351, 15),
			}),
		new ShapelessRecipe(new ItemStack(ItemFactory.GetItem(351, 8), 2),
			new List<ItemStack>
			{
				new ItemStack(ItemFactory.GetItem(351, 0), 1),
				new ItemStack(ItemFactory.GetItem(351, 15), 1),
			}),
		new ShapedRecipe(2, 1, new ItemStack(ItemFactory.GetItem(351, 7), 2),
			new Item[]
			{
				ItemFactory.GetItem(351, 8),
				ItemFactory.GetItem(351, 15),
			}),
		new ShapelessRecipe(new ItemStack(ItemFactory.GetItem(351, 7), 2),
			new List<ItemStack>
			{
				new ItemStack(ItemFactory.GetItem(351, 8), 1),
				new ItemStack(ItemFactory.GetItem(351, 15), 1),
			}),
		new ShapedRecipe(2, 2, new ItemStack(ItemFactory.GetItem(351, 7), 3),
			new Item[]
			{
				ItemFactory.GetItem(351, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(351, 15),
				ItemFactory.GetItem(351, 15),
			}),
		new ShapelessRecipe(new ItemStack(ItemFactory.GetItem(351, 7), 3),
			new List<ItemStack>
			{
				new ItemStack(ItemFactory.GetItem(351, 0), 1),
				new ItemStack(ItemFactory.GetItem(351, 15), 1),
				new ItemStack(ItemFactory.GetItem(351, 15), 1),
			}),
		new ShapedRecipe(2, 2, new ItemStack(ItemFactory.GetItem(351, 3), 2),
			new Item[]
			{
				ItemFactory.GetItem(351, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(351, 1),
				ItemFactory.GetItem(351, 11),
			}),
		new ShapelessRecipe(new ItemStack(ItemFactory.GetItem(351, 3), 2),
			new List<ItemStack>
			{
				new ItemStack(ItemFactory.GetItem(351, 0), 1),
				new ItemStack(ItemFactory.GetItem(351, 1), 1),
				new ItemStack(ItemFactory.GetItem(351, 11), 1),
			}),
		new ShapedRecipe(2, 1, new ItemStack(ItemFactory.GetItem(351, 12), 2),
			new Item[]
			{
				ItemFactory.GetItem(351, 4),
				ItemFactory.GetItem(351, 15),
			}),
		new ShapelessRecipe(new ItemStack(ItemFactory.GetItem(351, 12), 2),
			new List<ItemStack>
			{
				new ItemStack(ItemFactory.GetItem(351, 4), 1),
				new ItemStack(ItemFactory.GetItem(351, 15), 1),
			}),
		new ShapedRecipe(2, 1, new ItemStack(ItemFactory.GetItem(351, 6), 2),
			new Item[]
			{
				ItemFactory.GetItem(351, 4),
				ItemFactory.GetItem(351, 2),
			}),
		new ShapelessRecipe(new ItemStack(ItemFactory.GetItem(351, 6), 2),
			new List<ItemStack>
			{
				new ItemStack(ItemFactory.GetItem(351, 4), 1),
				new ItemStack(ItemFactory.GetItem(351, 2), 1),
			}),
		new ShapedRecipe(2, 1, new ItemStack(ItemFactory.GetItem(351, 5), 2),
			new Item[]
			{
				ItemFactory.GetItem(351, 4),
				ItemFactory.GetItem(351, 1),
			}),
		new ShapelessRecipe(new ItemStack(ItemFactory.GetItem(351, 5), 2),
			new List<ItemStack>
			{
				new ItemStack(ItemFactory.GetItem(351, 4), 1),
				new ItemStack(ItemFactory.GetItem(351, 1), 1),
			}),
		new ShapedRecipe(2, 1, new ItemStack(ItemFactory.GetItem(351, 13), 2),
			new Item[]
			{
				ItemFactory.GetItem(351, 5),
				ItemFactory.GetItem(351, 9),
			}),
		new ShapelessRecipe(new ItemStack(ItemFactory.GetItem(351, 13), 2),
			new List<ItemStack>
			{
				new ItemStack(ItemFactory.GetItem(351, 5), 1),
				new ItemStack(ItemFactory.GetItem(351, 9), 1),
			}),
		new ShapedRecipe(2, 2, new ItemStack(ItemFactory.GetItem(351, 13), 3),
			new Item[]
			{
				ItemFactory.GetItem(351, 4),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(351, 1),
				ItemFactory.GetItem(351, 9),
			}),
		new ShapelessRecipe(new ItemStack(ItemFactory.GetItem(351, 13), 3),
			new List<ItemStack>
			{
				new ItemStack(ItemFactory.GetItem(351, 4), 1),
				new ItemStack(ItemFactory.GetItem(351, 1), 1),
				new ItemStack(ItemFactory.GetItem(351, 9), 1),
			}),
		new ShapedRecipe(2, 2, new ItemStack(ItemFactory.GetItem(351, 13), 4),
			new Item[]
			{
				ItemFactory.GetItem(351, 4),
				ItemFactory.GetItem(351, 1),
				ItemFactory.GetItem(351, 15),
				ItemFactory.GetItem(351, 1),
			}),
		new ShapelessRecipe(new ItemStack(ItemFactory.GetItem(351, 13), 4),
			new List<ItemStack>
			{
				new ItemStack(ItemFactory.GetItem(351, 4), 1),
				new ItemStack(ItemFactory.GetItem(351, 15), 1),
				new ItemStack(ItemFactory.GetItem(351, 1), 1),
				new ItemStack(ItemFactory.GetItem(351, 1), 1),
			}),
		new ShapedRecipe(1, 1, new ItemStack(ItemFactory.GetItem(351, 12), 1),
			new Item[]
			{
				ItemFactory.GetItem(38, 1),
			}),
		new ShapedRecipe(1, 1, new ItemStack(ItemFactory.GetItem(351, 13), 1),
			new Item[]
			{
				ItemFactory.GetItem(38, 2),
			}),
		new ShapedRecipe(1, 1, new ItemStack(ItemFactory.GetItem(351, 7), 1),
			new Item[]
			{
				ItemFactory.GetItem(38, 3),
			}),
		new ShapedRecipe(1, 1, new ItemStack(ItemFactory.GetItem(351, 1), 1),
			new Item[]
			{
				ItemFactory.GetItem(38, 4),
			}),
		new ShapedRecipe(1, 1, new ItemStack(ItemFactory.GetItem(351, 14), 1),
			new Item[]
			{
				ItemFactory.GetItem(38, 5),
			}),
		new ShapedRecipe(1, 1, new ItemStack(ItemFactory.GetItem(351, 7), 1),
			new Item[]
			{
				ItemFactory.GetItem(38, 6),
			}),
		new ShapedRecipe(1, 1, new ItemStack(ItemFactory.GetItem(351, 9), 1),
			new Item[]
			{
				ItemFactory.GetItem(38, 7),
			}),
		new ShapedRecipe(1, 1, new ItemStack(ItemFactory.GetItem(351, 7), 1),
			new Item[]
			{
				ItemFactory.GetItem(38, 8),
			}),
		new ShapedRecipe(1, 1, new ItemStack(ItemFactory.GetItem(351, 11), 2),
			new Item[]
			{
				ItemFactory.GetItem(175, 0),
			}),
		new ShapedRecipe(1, 1, new ItemStack(ItemFactory.GetItem(351, 13), 2),
			new Item[]
			{
				ItemFactory.GetItem(175, 1),
			}),
		new ShapedRecipe(1, 1, new ItemStack(ItemFactory.GetItem(351, 1), 2),
			new Item[]
			{
				ItemFactory.GetItem(175, 4),
			}),
		new ShapedRecipe(1, 1, new ItemStack(ItemFactory.GetItem(351, 9), 2),
			new Item[]
			{
				ItemFactory.GetItem(175, 5),
			}),
		new ShapedRecipe(3, 1, new ItemStack(ItemFactory.GetItem(339, 0), 3),
			new Item[]
			{
				ItemFactory.GetItem(338, 0),
				ItemFactory.GetItem(338, 0),
				ItemFactory.GetItem(338, 0),
			}),
		new ShapedRecipe(2, 2, new ItemStack(ItemFactory.GetItem(340, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(339, 0),
				ItemFactory.GetItem(339, 0),
				ItemFactory.GetItem(339, 0),
				ItemFactory.GetItem(334, 0),
			}),
		new ShapelessRecipe(new ItemStack(ItemFactory.GetItem(340, 0), 1),
			new List<ItemStack>
			{
				new ItemStack(ItemFactory.GetItem(339, 0), 1),
				new ItemStack(ItemFactory.GetItem(339, 0), 1),
				new ItemStack(ItemFactory.GetItem(339, 0), 1),
				new ItemStack(ItemFactory.GetItem(334, 0), 1),
			}),
		new ShapedRecipe(3, 2, new ItemStack(ItemFactory.GetItem(139, 0), 6),
			new Item[]
			{
				ItemFactory.GetItem(4, -1),
				ItemFactory.GetItem(4, -1),
				ItemFactory.GetItem(4, -1),
				ItemFactory.GetItem(4, -1),
				ItemFactory.GetItem(4, -1),
				ItemFactory.GetItem(4, -1),
			}),
		new ShapedRecipe(3, 2, new ItemStack(ItemFactory.GetItem(139, 1), 6),
			new Item[]
			{
				ItemFactory.GetItem(48, -1),
				ItemFactory.GetItem(48, -1),
				ItemFactory.GetItem(48, -1),
				ItemFactory.GetItem(48, -1),
				ItemFactory.GetItem(48, -1),
				ItemFactory.GetItem(48, -1),
			}),
		new ShapedRecipe(3, 2, new ItemStack(ItemFactory.GetItem(113, 0), 6),
			new Item[]
			{
				ItemFactory.GetItem(112, -1),
				ItemFactory.GetItem(112, -1),
				ItemFactory.GetItem(112, -1),
				ItemFactory.GetItem(112, -1),
				ItemFactory.GetItem(112, -1),
				ItemFactory.GetItem(112, -1),
			}),
		new ShapedRecipe(3, 2, new ItemStack(ItemFactory.GetItem(85, 0), 3),
			new Item[]
			{
				ItemFactory.GetItem(5, 0),
				ItemFactory.GetItem(5, 0),
				ItemFactory.GetItem(280, 0),
				ItemFactory.GetItem(280, 0),
				ItemFactory.GetItem(5, 0),
				ItemFactory.GetItem(5, 0),
			}),
		new ShapedRecipe(3, 2, new ItemStack(ItemFactory.GetItem(85, 1), 3),
			new Item[]
			{
				ItemFactory.GetItem(5, 1),
				ItemFactory.GetItem(5, 1),
				ItemFactory.GetItem(280, 0),
				ItemFactory.GetItem(280, 0),
				ItemFactory.GetItem(5, 1),
				ItemFactory.GetItem(5, 1),
			}),
		new ShapedRecipe(3, 2, new ItemStack(ItemFactory.GetItem(85, 2), 3),
			new Item[]
			{
				ItemFactory.GetItem(5, 2),
				ItemFactory.GetItem(5, 2),
				ItemFactory.GetItem(280, 0),
				ItemFactory.GetItem(280, 0),
				ItemFactory.GetItem(5, 2),
				ItemFactory.GetItem(5, 2),
			}),
		new ShapedRecipe(3, 2, new ItemStack(ItemFactory.GetItem(85, 3), 3),
			new Item[]
			{
				ItemFactory.GetItem(5, 3),
				ItemFactory.GetItem(5, 3),
				ItemFactory.GetItem(280, 0),
				ItemFactory.GetItem(280, 0),
				ItemFactory.GetItem(5, 3),
				ItemFactory.GetItem(5, 3),
			}),
		new ShapedRecipe(3, 2, new ItemStack(ItemFactory.GetItem(85, 4), 3),
			new Item[]
			{
				ItemFactory.GetItem(5, 4),
				ItemFactory.GetItem(5, 4),
				ItemFactory.GetItem(280, 0),
				ItemFactory.GetItem(280, 0),
				ItemFactory.GetItem(5, 4),
				ItemFactory.GetItem(5, 4),
			}),
		new ShapedRecipe(3, 2, new ItemStack(ItemFactory.GetItem(85, 5), 3),
			new Item[]
			{
				ItemFactory.GetItem(5, 5),
				ItemFactory.GetItem(5, 5),
				ItemFactory.GetItem(280, 0),
				ItemFactory.GetItem(280, 0),
				ItemFactory.GetItem(5, 5),
				ItemFactory.GetItem(5, 5),
			}),
		new ShapedRecipe(3, 2, new ItemStack(ItemFactory.GetItem(107, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(280, 0),
				ItemFactory.GetItem(280, 0),
				ItemFactory.GetItem(5, 0),
				ItemFactory.GetItem(5, 0),
				ItemFactory.GetItem(280, 0),
				ItemFactory.GetItem(280, 0),
			}),
		new ShapedRecipe(3, 2, new ItemStack(ItemFactory.GetItem(183, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(280, 0),
				ItemFactory.GetItem(280, 0),
				ItemFactory.GetItem(5, 1),
				ItemFactory.GetItem(5, 1),
				ItemFactory.GetItem(280, 0),
				ItemFactory.GetItem(280, 0),
			}),
		new ShapedRecipe(3, 2, new ItemStack(ItemFactory.GetItem(184, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(280, 0),
				ItemFactory.GetItem(280, 0),
				ItemFactory.GetItem(5, 2),
				ItemFactory.GetItem(5, 2),
				ItemFactory.GetItem(280, 0),
				ItemFactory.GetItem(280, 0),
			}),
		new ShapedRecipe(3, 2, new ItemStack(ItemFactory.GetItem(185, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(280, 0),
				ItemFactory.GetItem(280, 0),
				ItemFactory.GetItem(5, 3),
				ItemFactory.GetItem(5, 3),
				ItemFactory.GetItem(280, 0),
				ItemFactory.GetItem(280, 0),
			}),
		new ShapedRecipe(3, 2, new ItemStack(ItemFactory.GetItem(187, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(280, 0),
				ItemFactory.GetItem(280, 0),
				ItemFactory.GetItem(5, 4),
				ItemFactory.GetItem(5, 4),
				ItemFactory.GetItem(280, 0),
				ItemFactory.GetItem(280, 0),
			}),
		new ShapedRecipe(3, 2, new ItemStack(ItemFactory.GetItem(186, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(280, 0),
				ItemFactory.GetItem(280, 0),
				ItemFactory.GetItem(5, 5),
				ItemFactory.GetItem(5, 5),
				ItemFactory.GetItem(280, 0),
				ItemFactory.GetItem(280, 0),
			}),
		new ShapedRecipe(3, 3, new ItemStack(ItemFactory.GetItem(145, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(42, -1),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(265, 0),
				ItemFactory.GetItem(42, -1),
				ItemFactory.GetItem(265, 0),
				ItemFactory.GetItem(265, 0),
				ItemFactory.GetItem(42, -1),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(265, 0),
			}),
		new ShapedRecipe(3, 3, new ItemStack(ItemFactory.GetItem(47, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(5, -1),
				ItemFactory.GetItem(340, 0),
				ItemFactory.GetItem(5, -1),
				ItemFactory.GetItem(5, -1),
				ItemFactory.GetItem(340, 0),
				ItemFactory.GetItem(5, -1),
				ItemFactory.GetItem(5, -1),
				ItemFactory.GetItem(340, 0),
				ItemFactory.GetItem(5, -1),
			}),
		new ShapedRecipe(2, 2, new ItemStack(ItemFactory.GetItem(80, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(332, 0),
				ItemFactory.GetItem(332, 0),
				ItemFactory.GetItem(332, 0),
				ItemFactory.GetItem(332, 0),
			}),
		new ShapedRecipe(3, 1, new ItemStack(ItemFactory.GetItem(78, 0), 6),
			new Item[]
			{
				ItemFactory.GetItem(80, -1),
				ItemFactory.GetItem(80, -1),
				ItemFactory.GetItem(80, -1),
			}),
		new ShapedRecipe(2, 2, new ItemStack(ItemFactory.GetItem(82, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(337, 0),
				ItemFactory.GetItem(337, 0),
				ItemFactory.GetItem(337, 0),
				ItemFactory.GetItem(337, 0),
			}),
		new ShapedRecipe(2, 2, new ItemStack(ItemFactory.GetItem(45, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(336, 0),
				ItemFactory.GetItem(336, 0),
				ItemFactory.GetItem(336, 0),
				ItemFactory.GetItem(336, 0),
			}),
		new ShapedRecipe(2, 2, new ItemStack(ItemFactory.GetItem(89, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(348, 0),
				ItemFactory.GetItem(348, 0),
				ItemFactory.GetItem(348, 0),
				ItemFactory.GetItem(348, 0),
			}),
		new ShapedRecipe(2, 2, new ItemStack(ItemFactory.GetItem(35, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(287, 0),
				ItemFactory.GetItem(287, 0),
				ItemFactory.GetItem(287, 0),
				ItemFactory.GetItem(287, 0),
			}),
		new ShapedRecipe(3, 3, new ItemStack(ItemFactory.GetItem(46, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(289, 0),
				ItemFactory.GetItem(12, -1),
				ItemFactory.GetItem(289, 0),
				ItemFactory.GetItem(12, -1),
				ItemFactory.GetItem(289, 0),
				ItemFactory.GetItem(12, -1),
				ItemFactory.GetItem(289, 0),
				ItemFactory.GetItem(12, -1),
				ItemFactory.GetItem(289, 0),
			}),
		new ShapedRecipe(3, 1, new ItemStack(ItemFactory.GetItem(44, 3), 6),
			new Item[]
			{
				ItemFactory.GetItem(4, -1),
				ItemFactory.GetItem(4, -1),
				ItemFactory.GetItem(4, -1),
			}),
		new ShapedRecipe(3, 1, new ItemStack(ItemFactory.GetItem(44, 0), 6),
			new Item[]
			{
				ItemFactory.GetItem(1, -1),
				ItemFactory.GetItem(1, -1),
				ItemFactory.GetItem(1, -1),
			}),
		new ShapedRecipe(3, 1, new ItemStack(ItemFactory.GetItem(44, 1), 6),
			new Item[]
			{
				ItemFactory.GetItem(24, -1),
				ItemFactory.GetItem(24, -1),
				ItemFactory.GetItem(24, -1),
			}),
		new ShapedRecipe(3, 1, new ItemStack(ItemFactory.GetItem(158, 0), 6),
			new Item[]
			{
				ItemFactory.GetItem(5, 0),
				ItemFactory.GetItem(5, 0),
				ItemFactory.GetItem(5, 0),
			}),
		new ShapedRecipe(3, 1, new ItemStack(ItemFactory.GetItem(158, 1), 6),
			new Item[]
			{
				ItemFactory.GetItem(5, 1),
				ItemFactory.GetItem(5, 1),
				ItemFactory.GetItem(5, 1),
			}),
		new ShapedRecipe(3, 1, new ItemStack(ItemFactory.GetItem(158, 2), 6),
			new Item[]
			{
				ItemFactory.GetItem(5, 2),
				ItemFactory.GetItem(5, 2),
				ItemFactory.GetItem(5, 2),
			}),
		new ShapedRecipe(3, 1, new ItemStack(ItemFactory.GetItem(158, 3), 6),
			new Item[]
			{
				ItemFactory.GetItem(5, 3),
				ItemFactory.GetItem(5, 3),
				ItemFactory.GetItem(5, 3),
			}),
		new ShapedRecipe(3, 1, new ItemStack(ItemFactory.GetItem(158, 4), 6),
			new Item[]
			{
				ItemFactory.GetItem(5, 4),
				ItemFactory.GetItem(5, 4),
				ItemFactory.GetItem(5, 4),
			}),
		new ShapedRecipe(3, 1, new ItemStack(ItemFactory.GetItem(158, 5), 6),
			new Item[]
			{
				ItemFactory.GetItem(5, 5),
				ItemFactory.GetItem(5, 5),
				ItemFactory.GetItem(5, 5),
			}),
		new ShapedRecipe(3, 1, new ItemStack(ItemFactory.GetItem(44, 4), 6),
			new Item[]
			{
				ItemFactory.GetItem(45, -1),
				ItemFactory.GetItem(45, -1),
				ItemFactory.GetItem(45, -1),
			}),
		new ShapedRecipe(3, 1, new ItemStack(ItemFactory.GetItem(44, 5), 6),
			new Item[]
			{
				ItemFactory.GetItem(98, -1),
				ItemFactory.GetItem(98, -1),
				ItemFactory.GetItem(98, -1),
			}),
		new ShapedRecipe(3, 1, new ItemStack(ItemFactory.GetItem(44, 7), 6),
			new Item[]
			{
				ItemFactory.GetItem(112, -1),
				ItemFactory.GetItem(112, -1),
				ItemFactory.GetItem(112, -1),
			}),
		new ShapedRecipe(3, 1, new ItemStack(ItemFactory.GetItem(44, 6), 6),
			new Item[]
			{
				ItemFactory.GetItem(155, -1),
				ItemFactory.GetItem(155, -1),
				ItemFactory.GetItem(155, -1),
			}),
		new ShapedRecipe(3, 3, new ItemStack(ItemFactory.GetItem(65, 0), 3),
			new Item[]
			{
				ItemFactory.GetItem(280, 0),
				ItemFactory.GetItem(280, 0),
				ItemFactory.GetItem(280, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(280, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(280, 0),
				ItemFactory.GetItem(280, 0),
				ItemFactory.GetItem(280, 0),
			}),
		new ShapedRecipe(2, 3, new ItemStack(ItemFactory.GetItem(324, 0), 3),
			new Item[]
			{
				ItemFactory.GetItem(5, 0),
				ItemFactory.GetItem(5, 0),
				ItemFactory.GetItem(5, 0),
				ItemFactory.GetItem(5, 0),
				ItemFactory.GetItem(5, 0),
				ItemFactory.GetItem(5, 0),
			}),
		new ShapedRecipe(2, 3, new ItemStack(ItemFactory.GetItem(427, 0), 3),
			new Item[]
			{
				ItemFactory.GetItem(5, 1),
				ItemFactory.GetItem(5, 1),
				ItemFactory.GetItem(5, 1),
				ItemFactory.GetItem(5, 1),
				ItemFactory.GetItem(5, 1),
				ItemFactory.GetItem(5, 1),
			}),
		new ShapedRecipe(2, 3, new ItemStack(ItemFactory.GetItem(428, 0), 3),
			new Item[]
			{
				ItemFactory.GetItem(5, 2),
				ItemFactory.GetItem(5, 2),
				ItemFactory.GetItem(5, 2),
				ItemFactory.GetItem(5, 2),
				ItemFactory.GetItem(5, 2),
				ItemFactory.GetItem(5, 2),
			}),
		new ShapedRecipe(2, 3, new ItemStack(ItemFactory.GetItem(429, 0), 3),
			new Item[]
			{
				ItemFactory.GetItem(5, 3),
				ItemFactory.GetItem(5, 3),
				ItemFactory.GetItem(5, 3),
				ItemFactory.GetItem(5, 3),
				ItemFactory.GetItem(5, 3),
				ItemFactory.GetItem(5, 3),
			}),
		new ShapedRecipe(2, 3, new ItemStack(ItemFactory.GetItem(430, 0), 3),
			new Item[]
			{
				ItemFactory.GetItem(5, 4),
				ItemFactory.GetItem(5, 4),
				ItemFactory.GetItem(5, 4),
				ItemFactory.GetItem(5, 4),
				ItemFactory.GetItem(5, 4),
				ItemFactory.GetItem(5, 4),
			}),
		new ShapedRecipe(2, 3, new ItemStack(ItemFactory.GetItem(431, 0), 3),
			new Item[]
			{
				ItemFactory.GetItem(5, 5),
				ItemFactory.GetItem(5, 5),
				ItemFactory.GetItem(5, 5),
				ItemFactory.GetItem(5, 5),
				ItemFactory.GetItem(5, 5),
				ItemFactory.GetItem(5, 5),
			}),
		new ShapedRecipe(3, 2, new ItemStack(ItemFactory.GetItem(96, 0), 2),
			new Item[]
			{
				ItemFactory.GetItem(5, -1),
				ItemFactory.GetItem(5, -1),
				ItemFactory.GetItem(5, -1),
				ItemFactory.GetItem(5, -1),
				ItemFactory.GetItem(5, -1),
				ItemFactory.GetItem(5, -1),
			}),
		new ShapedRecipe(2, 2, new ItemStack(ItemFactory.GetItem(167, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(265, 0),
				ItemFactory.GetItem(265, 0),
				ItemFactory.GetItem(265, 0),
				ItemFactory.GetItem(265, 0),
			}),
		new ShapedRecipe(1, 3, new ItemStack(ItemFactory.GetItem(131, 0), 2),
			new Item[]
			{
				ItemFactory.GetItem(265, 0),
				ItemFactory.GetItem(280, 0),
				ItemFactory.GetItem(5, -1),
			}),
		new ShapedRecipe(3, 3, new ItemStack(ItemFactory.GetItem(25, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(5, -1),
				ItemFactory.GetItem(5, -1),
				ItemFactory.GetItem(5, -1),
				ItemFactory.GetItem(5, -1),
				ItemFactory.GetItem(331, 0),
				ItemFactory.GetItem(5, -1),
				ItemFactory.GetItem(5, -1),
				ItemFactory.GetItem(5, -1),
				ItemFactory.GetItem(5, -1),
			}),
		new ShapedRecipe(2, 3, new ItemStack(ItemFactory.GetItem(330, 0), 3),
			new Item[]
			{
				ItemFactory.GetItem(265, 0),
				ItemFactory.GetItem(265, 0),
				ItemFactory.GetItem(265, 0),
				ItemFactory.GetItem(265, 0),
				ItemFactory.GetItem(265, 0),
				ItemFactory.GetItem(265, 0),
			}),
		new ShapedRecipe(3, 3, new ItemStack(ItemFactory.GetItem(323, 0), 3),
			new Item[]
			{
				ItemFactory.GetItem(5, -1),
				ItemFactory.GetItem(5, -1),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(5, -1),
				ItemFactory.GetItem(5, -1),
				ItemFactory.GetItem(280, 0),
				ItemFactory.GetItem(5, -1),
				ItemFactory.GetItem(5, -1),
				ItemFactory.GetItem(0, 0),
			}),
		new ShapedRecipe(3, 3, new ItemStack(ItemFactory.GetItem(325, 0), 3),
			new Item[]
			{
				ItemFactory.GetItem(325, 1),
				ItemFactory.GetItem(353, 0),
				ItemFactory.GetItem(296, 0),
				ItemFactory.GetItem(325, 1),
				ItemFactory.GetItem(344, 0),
				ItemFactory.GetItem(296, 0),
				ItemFactory.GetItem(325, 1),
				ItemFactory.GetItem(353, 0),
				ItemFactory.GetItem(296, 0),
			}),
		new ShapedRecipe(1, 1, new ItemStack(ItemFactory.GetItem(353, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(338, 0),
			}),
		new ShapedRecipe(3, 3, new ItemStack(ItemFactory.GetItem(396, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(371, 0),
				ItemFactory.GetItem(371, 0),
				ItemFactory.GetItem(371, 0),
				ItemFactory.GetItem(371, 0),
				ItemFactory.GetItem(391, 0),
				ItemFactory.GetItem(371, 0),
				ItemFactory.GetItem(371, 0),
				ItemFactory.GetItem(371, 0),
				ItemFactory.GetItem(371, 0),
			}),
		new ShapedRecipe(3, 3, new ItemStack(ItemFactory.GetItem(382, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(371, 0),
				ItemFactory.GetItem(371, 0),
				ItemFactory.GetItem(371, 0),
				ItemFactory.GetItem(371, 0),
				ItemFactory.GetItem(360, 0),
				ItemFactory.GetItem(371, 0),
				ItemFactory.GetItem(371, 0),
				ItemFactory.GetItem(371, 0),
				ItemFactory.GetItem(371, 0),
			}),
		new ShapedRecipe(1, 1, new ItemStack(ItemFactory.GetItem(377, 0), 2),
			new Item[]
			{
				ItemFactory.GetItem(369, 0),
			}),
		new ShapedRecipe(2, 1, new ItemStack(ItemFactory.GetItem(378, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(377, 0),
				ItemFactory.GetItem(341, 0),
			}),
		new ShapelessRecipe(new ItemStack(ItemFactory.GetItem(378, 0), 1),
			new List<ItemStack>
			{
				new ItemStack(ItemFactory.GetItem(377, 0), 1),
				new ItemStack(ItemFactory.GetItem(341, 0), 1),
			}),
		new ShapedRecipe(2, 2, new ItemStack(ItemFactory.GetItem(376, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(39, -1),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(353, 0),
				ItemFactory.GetItem(375, 0),
			}),
		new ShapelessRecipe(new ItemStack(ItemFactory.GetItem(376, 0), 1),
			new List<ItemStack>
			{
				new ItemStack(ItemFactory.GetItem(39, -1), 1),
				new ItemStack(ItemFactory.GetItem(353, 0), 1),
				new ItemStack(ItemFactory.GetItem(375, 0), 1),
			}),
		new ShapedRecipe(1, 1, new ItemStack(ItemFactory.GetItem(5, 0), 4),
			new Item[]
			{
				ItemFactory.GetItem(17, 0),
			}),
		new ShapedRecipe(1, 1, new ItemStack(ItemFactory.GetItem(5, 1), 4),
			new Item[]
			{
				ItemFactory.GetItem(17, 1),
			}),
		new ShapedRecipe(1, 1, new ItemStack(ItemFactory.GetItem(5, 2), 4),
			new Item[]
			{
				ItemFactory.GetItem(17, 2),
			}),
		new ShapedRecipe(1, 1, new ItemStack(ItemFactory.GetItem(5, 3), 4),
			new Item[]
			{
				ItemFactory.GetItem(17, 3),
			}),
		new ShapedRecipe(1, 1, new ItemStack(ItemFactory.GetItem(5, 4), 4),
			new Item[]
			{
				ItemFactory.GetItem(162, 0),
			}),
		new ShapedRecipe(1, 1, new ItemStack(ItemFactory.GetItem(5, 5), 4),
			new Item[]
			{
				ItemFactory.GetItem(162, 1),
			}),
		new ShapedRecipe(1, 2, new ItemStack(ItemFactory.GetItem(280, 0), 4),
			new Item[]
			{
				ItemFactory.GetItem(5, -1),
				ItemFactory.GetItem(5, -1),
			}),
		new ShapedRecipe(1, 2, new ItemStack(ItemFactory.GetItem(50, 0), 4),
			new Item[]
			{
				ItemFactory.GetItem(263, 0),
				ItemFactory.GetItem(280, 0),
			}),
		new ShapedRecipe(1, 2, new ItemStack(ItemFactory.GetItem(50, 0), 4),
			new Item[]
			{
				ItemFactory.GetItem(263, 1),
				ItemFactory.GetItem(280, 0),
			}),
		new ShapedRecipe(3, 2, new ItemStack(ItemFactory.GetItem(281, 0), 4),
			new Item[]
			{
				ItemFactory.GetItem(5, -1),
				ItemFactory.GetItem(5, -1),
				ItemFactory.GetItem(5, -1),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(0, 0),
			}),
		new ShapedRecipe(3, 3, new ItemStack(ItemFactory.GetItem(413, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(391, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(412, 0),
				ItemFactory.GetItem(393, 0),
				ItemFactory.GetItem(281, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(39, -1),
				ItemFactory.GetItem(0, 0),
			}),
		new ShapedRecipe(3, 3, new ItemStack(ItemFactory.GetItem(413, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(391, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(412, 0),
				ItemFactory.GetItem(393, 0),
				ItemFactory.GetItem(281, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(40, -1),
				ItemFactory.GetItem(0, 0),
			}),
		new ShapedRecipe(3, 2, new ItemStack(ItemFactory.GetItem(374, 0), 3),
			new Item[]
			{
				ItemFactory.GetItem(20, -1),
				ItemFactory.GetItem(20, -1),
				ItemFactory.GetItem(20, -1),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(0, 0),
			}),
		new ShapedRecipe(3, 2, new ItemStack(ItemFactory.GetItem(390, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(336, 0),
				ItemFactory.GetItem(336, 0),
				ItemFactory.GetItem(336, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(0, 0),
			}),
		new ShapedRecipe(3, 3, new ItemStack(ItemFactory.GetItem(66, 0), 16),
			new Item[]
			{
				ItemFactory.GetItem(265, 0),
				ItemFactory.GetItem(265, 0),
				ItemFactory.GetItem(265, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(280, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(265, 0),
				ItemFactory.GetItem(265, 0),
				ItemFactory.GetItem(265, 0),
			}),
		new ShapedRecipe(3, 3, new ItemStack(ItemFactory.GetItem(27, 0), 6),
			new Item[]
			{
				ItemFactory.GetItem(266, 0),
				ItemFactory.GetItem(266, 0),
				ItemFactory.GetItem(266, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(280, 0),
				ItemFactory.GetItem(331, 0),
				ItemFactory.GetItem(266, 0),
				ItemFactory.GetItem(266, 0),
				ItemFactory.GetItem(266, 0),
			}),
		new ShapedRecipe(3, 3, new ItemStack(ItemFactory.GetItem(28, 0), 6),
			new Item[]
			{
				ItemFactory.GetItem(265, 0),
				ItemFactory.GetItem(265, 0),
				ItemFactory.GetItem(265, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(70, -1),
				ItemFactory.GetItem(331, 0),
				ItemFactory.GetItem(265, 0),
				ItemFactory.GetItem(265, 0),
				ItemFactory.GetItem(265, 0),
			}),
		new ShapedRecipe(3, 3, new ItemStack(ItemFactory.GetItem(126, 0), 6),
			new Item[]
			{
				ItemFactory.GetItem(265, 0),
				ItemFactory.GetItem(265, 0),
				ItemFactory.GetItem(265, 0),
				ItemFactory.GetItem(280, 0),
				ItemFactory.GetItem(76, -1),
				ItemFactory.GetItem(280, 0),
				ItemFactory.GetItem(265, 0),
				ItemFactory.GetItem(265, 0),
				ItemFactory.GetItem(265, 0),
			}),
		new ShapedRecipe(3, 2, new ItemStack(ItemFactory.GetItem(328, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(265, 0),
				ItemFactory.GetItem(265, 0),
				ItemFactory.GetItem(265, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(265, 0),
				ItemFactory.GetItem(265, 0),
			}),
		new ShapedRecipe(3, 2, new ItemStack(ItemFactory.GetItem(379, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(4, -1),
				ItemFactory.GetItem(369, 0),
				ItemFactory.GetItem(4, -1),
				ItemFactory.GetItem(4, -1),
			}),
		new ShapedRecipe(1, 2, new ItemStack(ItemFactory.GetItem(91, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(86, -1),
				ItemFactory.GetItem(50, -1),
			}),
		new ShapedRecipe(3, 2, new ItemStack(ItemFactory.GetItem(333, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(5, 0),
				ItemFactory.GetItem(5, 0),
				ItemFactory.GetItem(5, 0),
				ItemFactory.GetItem(269, 0),
				ItemFactory.GetItem(5, 0),
				ItemFactory.GetItem(5, 0),
			}),
		new ShapedRecipe(3, 2, new ItemStack(ItemFactory.GetItem(333, 1), 1),
			new Item[]
			{
				ItemFactory.GetItem(5, 1),
				ItemFactory.GetItem(5, 1),
				ItemFactory.GetItem(5, 1),
				ItemFactory.GetItem(269, 0),
				ItemFactory.GetItem(5, 1),
				ItemFactory.GetItem(5, 1),
			}),
		new ShapedRecipe(3, 2, new ItemStack(ItemFactory.GetItem(333, 2), 1),
			new Item[]
			{
				ItemFactory.GetItem(5, 2),
				ItemFactory.GetItem(5, 2),
				ItemFactory.GetItem(5, 2),
				ItemFactory.GetItem(269, 0),
				ItemFactory.GetItem(5, 2),
				ItemFactory.GetItem(5, 2),
			}),
		new ShapedRecipe(3, 2, new ItemStack(ItemFactory.GetItem(333, 3), 1),
			new Item[]
			{
				ItemFactory.GetItem(5, 3),
				ItemFactory.GetItem(5, 3),
				ItemFactory.GetItem(5, 3),
				ItemFactory.GetItem(269, 0),
				ItemFactory.GetItem(5, 3),
				ItemFactory.GetItem(5, 3),
			}),
		new ShapedRecipe(3, 2, new ItemStack(ItemFactory.GetItem(333, 4), 1),
			new Item[]
			{
				ItemFactory.GetItem(5, 4),
				ItemFactory.GetItem(5, 4),
				ItemFactory.GetItem(5, 4),
				ItemFactory.GetItem(269, 0),
				ItemFactory.GetItem(5, 4),
				ItemFactory.GetItem(5, 4),
			}),
		new ShapedRecipe(3, 2, new ItemStack(ItemFactory.GetItem(333, 5), 1),
			new Item[]
			{
				ItemFactory.GetItem(5, 5),
				ItemFactory.GetItem(5, 5),
				ItemFactory.GetItem(5, 5),
				ItemFactory.GetItem(269, 0),
				ItemFactory.GetItem(5, 5),
				ItemFactory.GetItem(5, 5),
			}),
		new ShapedRecipe(3, 2, new ItemStack(ItemFactory.GetItem(325, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(265, 0),
				ItemFactory.GetItem(265, 0),
				ItemFactory.GetItem(265, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(0, 0),
			}),
		new ShapedRecipe(2, 2, new ItemStack(ItemFactory.GetItem(259, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(265, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(318, 0),
			}),
		new ShapelessRecipe(new ItemStack(ItemFactory.GetItem(259, 0), 1),
			new List<ItemStack>
			{
				new ItemStack(ItemFactory.GetItem(265, 0), 1),
				new ItemStack(ItemFactory.GetItem(318, 0), 1),
			}),
		new ShapedRecipe(3, 1, new ItemStack(ItemFactory.GetItem(297, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(296, 0),
				ItemFactory.GetItem(296, 0),
				ItemFactory.GetItem(296, 0),
			}),
		new ShapedRecipe(3, 3, new ItemStack(ItemFactory.GetItem(53, 0), 4),
			new Item[]
			{
				ItemFactory.GetItem(5, 0),
				ItemFactory.GetItem(5, 0),
				ItemFactory.GetItem(5, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(5, 0),
				ItemFactory.GetItem(5, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(5, 0),
			}),
		new ShapedRecipe(3, 3, new ItemStack(ItemFactory.GetItem(134, 0), 4),
			new Item[]
			{
				ItemFactory.GetItem(5, 1),
				ItemFactory.GetItem(5, 1),
				ItemFactory.GetItem(5, 1),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(5, 1),
				ItemFactory.GetItem(5, 1),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(5, 1),
			}),
		new ShapedRecipe(3, 3, new ItemStack(ItemFactory.GetItem(135, 0), 4),
			new Item[]
			{
				ItemFactory.GetItem(5, 2),
				ItemFactory.GetItem(5, 2),
				ItemFactory.GetItem(5, 2),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(5, 2),
				ItemFactory.GetItem(5, 2),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(5, 2),
			}),
		new ShapedRecipe(3, 3, new ItemStack(ItemFactory.GetItem(136, 0), 4),
			new Item[]
			{
				ItemFactory.GetItem(5, 3),
				ItemFactory.GetItem(5, 3),
				ItemFactory.GetItem(5, 3),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(5, 3),
				ItemFactory.GetItem(5, 3),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(5, 3),
			}),
		new ShapedRecipe(3, 3, new ItemStack(ItemFactory.GetItem(163, 0), 4),
			new Item[]
			{
				ItemFactory.GetItem(5, 4),
				ItemFactory.GetItem(5, 4),
				ItemFactory.GetItem(5, 4),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(5, 4),
				ItemFactory.GetItem(5, 4),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(5, 4),
			}),
		new ShapedRecipe(3, 3, new ItemStack(ItemFactory.GetItem(164, 0), 4),
			new Item[]
			{
				ItemFactory.GetItem(5, 5),
				ItemFactory.GetItem(5, 5),
				ItemFactory.GetItem(5, 5),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(5, 5),
				ItemFactory.GetItem(5, 5),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(5, 5),
			}),
		new ShapedRecipe(3, 3, new ItemStack(ItemFactory.GetItem(67, 0), 4),
			new Item[]
			{
				ItemFactory.GetItem(4, -1),
				ItemFactory.GetItem(4, -1),
				ItemFactory.GetItem(4, -1),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(4, -1),
				ItemFactory.GetItem(4, -1),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(4, -1),
			}),
		new ShapedRecipe(3, 3, new ItemStack(ItemFactory.GetItem(108, 0), 4),
			new Item[]
			{
				ItemFactory.GetItem(45, -1),
				ItemFactory.GetItem(45, -1),
				ItemFactory.GetItem(45, -1),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(45, -1),
				ItemFactory.GetItem(45, -1),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(45, -1),
			}),
		new ShapedRecipe(3, 3, new ItemStack(ItemFactory.GetItem(128, 0), 4),
			new Item[]
			{
				ItemFactory.GetItem(24, -1),
				ItemFactory.GetItem(24, -1),
				ItemFactory.GetItem(24, -1),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(24, -1),
				ItemFactory.GetItem(24, -1),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(24, -1),
			}),
		new ShapedRecipe(3, 3, new ItemStack(ItemFactory.GetItem(109, 0), 4),
			new Item[]
			{
				ItemFactory.GetItem(98, -1),
				ItemFactory.GetItem(98, -1),
				ItemFactory.GetItem(98, -1),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(98, -1),
				ItemFactory.GetItem(98, -1),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(98, -1),
			}),
		new ShapedRecipe(3, 3, new ItemStack(ItemFactory.GetItem(156, 0), 4),
			new Item[]
			{
				ItemFactory.GetItem(155, -1),
				ItemFactory.GetItem(155, -1),
				ItemFactory.GetItem(155, -1),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(155, -1),
				ItemFactory.GetItem(155, -1),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(155, -1),
			}),
		new ShapedRecipe(3, 3, new ItemStack(ItemFactory.GetItem(114, 0), 4),
			new Item[]
			{
				ItemFactory.GetItem(112, -1),
				ItemFactory.GetItem(112, -1),
				ItemFactory.GetItem(112, -1),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(112, -1),
				ItemFactory.GetItem(112, -1),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(112, -1),
			}),
		new ShapedRecipe(3, 3, new ItemStack(ItemFactory.GetItem(346, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(280, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(280, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(280, 0),
				ItemFactory.GetItem(287, 0),
				ItemFactory.GetItem(287, 0),
			}),
		new ShapedRecipe(3, 3, new ItemStack(ItemFactory.GetItem(321, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(280, 0),
				ItemFactory.GetItem(280, 0),
				ItemFactory.GetItem(280, 0),
				ItemFactory.GetItem(280, 0),
				ItemFactory.GetItem(35, -1),
				ItemFactory.GetItem(280, 0),
				ItemFactory.GetItem(280, 0),
				ItemFactory.GetItem(280, 0),
				ItemFactory.GetItem(280, 0),
			}),
		new ShapedRecipe(3, 3, new ItemStack(ItemFactory.GetItem(322, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(266, 0),
				ItemFactory.GetItem(266, 0),
				ItemFactory.GetItem(266, 0),
				ItemFactory.GetItem(266, 0),
				ItemFactory.GetItem(260, 0),
				ItemFactory.GetItem(266, 0),
				ItemFactory.GetItem(266, 0),
				ItemFactory.GetItem(266, 0),
				ItemFactory.GetItem(266, 0),
			}),
		new ShapedRecipe(3, 3, new ItemStack(ItemFactory.GetItem(466, 1), 1),
			new Item[]
			{
				ItemFactory.GetItem(41, -1),
				ItemFactory.GetItem(41, -1),
				ItemFactory.GetItem(41, -1),
				ItemFactory.GetItem(41, -1),
				ItemFactory.GetItem(260, 0),
				ItemFactory.GetItem(41, -1),
				ItemFactory.GetItem(41, -1),
				ItemFactory.GetItem(41, -1),
				ItemFactory.GetItem(41, -1),
			}),
		new ShapedRecipe(1, 2, new ItemStack(ItemFactory.GetItem(69, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(280, 0),
				ItemFactory.GetItem(4, -1),
			}),
		new ShapedRecipe(1, 2, new ItemStack(ItemFactory.GetItem(76, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(331, 0),
				ItemFactory.GetItem(280, 0),
			}),
		new ShapedRecipe(3, 3, new ItemStack(ItemFactory.GetItem(151, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(20, -1),
				ItemFactory.GetItem(406, 0),
				ItemFactory.GetItem(158, -1),
				ItemFactory.GetItem(20, -1),
				ItemFactory.GetItem(406, 0),
				ItemFactory.GetItem(158, -1),
				ItemFactory.GetItem(20, -1),
				ItemFactory.GetItem(406, 0),
				ItemFactory.GetItem(158, -1),
			}),
		new ShapedRecipe(3, 3, new ItemStack(ItemFactory.GetItem(123, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(331, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(331, 0),
				ItemFactory.GetItem(89, -1),
				ItemFactory.GetItem(331, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(331, 0),
				ItemFactory.GetItem(0, 0),
			}),
		new ShapedRecipe(3, 3, new ItemStack(ItemFactory.GetItem(347, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(266, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(266, 0),
				ItemFactory.GetItem(331, 0),
				ItemFactory.GetItem(266, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(266, 0),
				ItemFactory.GetItem(0, 0),
			}),
		new ShapedRecipe(3, 3, new ItemStack(ItemFactory.GetItem(345, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(265, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(265, 0),
				ItemFactory.GetItem(331, 0),
				ItemFactory.GetItem(265, 0),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(265, 0),
				ItemFactory.GetItem(0, 0),
			}),
		new ShapedRecipe(1, 1, new ItemStack(ItemFactory.GetItem(77, 5), 1),
			new Item[]
			{
				ItemFactory.GetItem(1, 0),
			}),
		new ShapedRecipe(1, 1, new ItemStack(ItemFactory.GetItem(143, 5), 1),
			new Item[]
			{
				ItemFactory.GetItem(5, -1),
			}),
		new ShapedRecipe(2, 1, new ItemStack(ItemFactory.GetItem(70, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(1, 0),
				ItemFactory.GetItem(1, 0),
			}),
		new ShapedRecipe(2, 1, new ItemStack(ItemFactory.GetItem(72, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(5, -1),
				ItemFactory.GetItem(5, -1),
			}),
		new ShapedRecipe(2, 1, new ItemStack(ItemFactory.GetItem(147, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(266, 0),
				ItemFactory.GetItem(266, 0),
			}),
		new ShapedRecipe(2, 1, new ItemStack(ItemFactory.GetItem(148, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(265, 0),
				ItemFactory.GetItem(265, 0),
			}),
		new ShapedRecipe(3, 2, new ItemStack(ItemFactory.GetItem(355, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(35, -1),
				ItemFactory.GetItem(35, -1),
				ItemFactory.GetItem(5, -1),
				ItemFactory.GetItem(35, -1),
				ItemFactory.GetItem(5, -1),
				ItemFactory.GetItem(5, -1),
			}),
		new ShapedRecipe(3, 3, new ItemStack(ItemFactory.GetItem(116, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(264, 0),
				ItemFactory.GetItem(49, -1),
				ItemFactory.GetItem(340, 0),
				ItemFactory.GetItem(49, -1),
				ItemFactory.GetItem(49, -1),
				ItemFactory.GetItem(0, 0),
				ItemFactory.GetItem(264, 0),
				ItemFactory.GetItem(49, -1),
			}),
		new ShapedRecipe(2, 2, new ItemStack(ItemFactory.GetItem(334, 0), 1),
			new Item[]
			{
				ItemFactory.GetItem(415, 0),
				ItemFactory.GetItem(415, 0),
				ItemFactory.GetItem(415, 0),
				ItemFactory.GetItem(415, 0),
			}),
		new ShapedRecipe(2, 1, new ItemStack(ItemFactory.GetItem(171, 0), 3),
			new Item[]
			{
				ItemFactory.GetItem(35, 0),
				ItemFactory.GetItem(35, 0),
			}),
		new ShapedRecipe(3, 3, new ItemStack(ItemFactory.GetItem(159, 15), 8),
			new Item[]
			{
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(351, 0),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
			}),
		new ShapedRecipe(2, 1, new ItemStack(ItemFactory.GetItem(171, 1), 3),
			new Item[]
			{
				ItemFactory.GetItem(35, 1),
				ItemFactory.GetItem(35, 1),
			}),
		new ShapedRecipe(3, 3, new ItemStack(ItemFactory.GetItem(159, 14), 8),
			new Item[]
			{
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(351, 1),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
			}),
		new ShapedRecipe(2, 1, new ItemStack(ItemFactory.GetItem(171, 2), 3),
			new Item[]
			{
				ItemFactory.GetItem(35, 2),
				ItemFactory.GetItem(35, 2),
			}),
		new ShapedRecipe(3, 3, new ItemStack(ItemFactory.GetItem(159, 13), 8),
			new Item[]
			{
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(351, 2),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
			}),
		new ShapedRecipe(2, 1, new ItemStack(ItemFactory.GetItem(171, 3), 3),
			new Item[]
			{
				ItemFactory.GetItem(35, 3),
				ItemFactory.GetItem(35, 3),
			}),
		new ShapedRecipe(3, 3, new ItemStack(ItemFactory.GetItem(159, 12), 8),
			new Item[]
			{
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(351, 3),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
			}),
		new ShapedRecipe(2, 1, new ItemStack(ItemFactory.GetItem(171, 4), 3),
			new Item[]
			{
				ItemFactory.GetItem(35, 4),
				ItemFactory.GetItem(35, 4),
			}),
		new ShapedRecipe(3, 3, new ItemStack(ItemFactory.GetItem(159, 11), 8),
			new Item[]
			{
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(351, 4),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
			}),
		new ShapedRecipe(2, 1, new ItemStack(ItemFactory.GetItem(171, 5), 3),
			new Item[]
			{
				ItemFactory.GetItem(35, 5),
				ItemFactory.GetItem(35, 5),
			}),
		new ShapedRecipe(3, 3, new ItemStack(ItemFactory.GetItem(159, 10), 8),
			new Item[]
			{
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(351, 5),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
			}),
		new ShapedRecipe(2, 1, new ItemStack(ItemFactory.GetItem(171, 6), 3),
			new Item[]
			{
				ItemFactory.GetItem(35, 6),
				ItemFactory.GetItem(35, 6),
			}),
		new ShapedRecipe(3, 3, new ItemStack(ItemFactory.GetItem(159, 9), 8),
			new Item[]
			{
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(351, 6),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
			}),
		new ShapedRecipe(2, 1, new ItemStack(ItemFactory.GetItem(171, 7), 3),
			new Item[]
			{
				ItemFactory.GetItem(35, 7),
				ItemFactory.GetItem(35, 7),
			}),
		new ShapedRecipe(3, 3, new ItemStack(ItemFactory.GetItem(159, 8), 8),
			new Item[]
			{
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(351, 7),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
			}),
		new ShapedRecipe(2, 1, new ItemStack(ItemFactory.GetItem(171, 8), 3),
			new Item[]
			{
				ItemFactory.GetItem(35, 8),
				ItemFactory.GetItem(35, 8),
			}),
		new ShapedRecipe(3, 3, new ItemStack(ItemFactory.GetItem(159, 7), 8),
			new Item[]
			{
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(351, 8),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
			}),
		new ShapedRecipe(2, 1, new ItemStack(ItemFactory.GetItem(171, 9), 3),
			new Item[]
			{
				ItemFactory.GetItem(35, 9),
				ItemFactory.GetItem(35, 9),
			}),
		new ShapedRecipe(3, 3, new ItemStack(ItemFactory.GetItem(159, 6), 8),
			new Item[]
			{
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(351, 9),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
			}),
		new ShapedRecipe(2, 1, new ItemStack(ItemFactory.GetItem(171, 10), 3),
			new Item[]
			{
				ItemFactory.GetItem(35, 10),
				ItemFactory.GetItem(35, 10),
			}),
		new ShapedRecipe(3, 3, new ItemStack(ItemFactory.GetItem(159, 5), 8),
			new Item[]
			{
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(351, 10),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
			}),
		new ShapedRecipe(2, 1, new ItemStack(ItemFactory.GetItem(171, 11), 3),
			new Item[]
			{
				ItemFactory.GetItem(35, 11),
				ItemFactory.GetItem(35, 11),
			}),
		new ShapedRecipe(3, 3, new ItemStack(ItemFactory.GetItem(159, 4), 8),
			new Item[]
			{
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(351, 11),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
			}),
		new ShapedRecipe(2, 1, new ItemStack(ItemFactory.GetItem(171, 12), 3),
			new Item[]
			{
				ItemFactory.GetItem(35, 12),
				ItemFactory.GetItem(35, 12),
			}),
		new ShapedRecipe(3, 3, new ItemStack(ItemFactory.GetItem(159, 3), 8),
			new Item[]
			{
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(351, 12),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
			}),
		new ShapedRecipe(2, 1, new ItemStack(ItemFactory.GetItem(171, 13), 3),
			new Item[]
			{
				ItemFactory.GetItem(35, 13),
				ItemFactory.GetItem(35, 13),
			}),
		new ShapedRecipe(3, 3, new ItemStack(ItemFactory.GetItem(159, 2), 8),
			new Item[]
			{
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(351, 13),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
			}),
		new ShapedRecipe(2, 1, new ItemStack(ItemFactory.GetItem(171, 14), 3),
			new Item[]
			{
				ItemFactory.GetItem(35, 14),
				ItemFactory.GetItem(35, 14),
			}),
		new ShapedRecipe(3, 3, new ItemStack(ItemFactory.GetItem(159, 1), 8),
			new Item[]
			{
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(351, 14),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
			}),
		new ShapedRecipe(2, 1, new ItemStack(ItemFactory.GetItem(171, 15), 3),
			new Item[]
			{
				ItemFactory.GetItem(35, 15),
				ItemFactory.GetItem(35, 15),
			}),
		new ShapedRecipe(3, 3, new ItemStack(ItemFactory.GetItem(159, 0), 8),
			new Item[]
			{
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(351, 15),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
				ItemFactory.GetItem(172, -1),
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

		public SmeltingRecipe()
		{
		}

		public SmeltingRecipe(ItemStack result, Item input) : this()
		{
			Result = result;
			Input = input;
		}

	}
}