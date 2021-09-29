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
using fNbt;
using MiNET.Items;
using MiNET.Utils;

namespace MiNET
{
	// ReSharper disable RedundantArgumentDefaultValue
	public static class InventoryUtils
	{
		public static CreativeItemStacks GetCreativeMetadataSlots()
		{
			CreativeItemStacks slotData = new CreativeItemStacks();
			for (int i = 0; i < CreativeInventoryItems.Count; i++)
			{
				slotData.Add(CreativeInventoryItems[i]);
			}

			return slotData;
		}
		

		// GENERATED CODE. DON'T EDIT BY HAND
        public static List<Item> CreativeInventoryItems = new List<Item>()
        {
            new Item(5, 0, 1){ NetworkId=1 }, /*minecraft:planks*/
            new Item(5, 0, 1){ NetworkId=2 }, /*minecraft:planks*/
            new Item(5, 0, 1){ NetworkId=3 }, /*minecraft:planks*/
            new Item(5, 0, 1){ NetworkId=4 }, /*minecraft:planks*/
            new Item(5, 0, 1){ NetworkId=5 }, /*minecraft:planks*/
            new Item(5, 0, 1){ NetworkId=6 }, /*minecraft:planks*/
            new Item(-242, 0, 1){ NetworkId=7 }, /*minecraft:crimson_planks*/
            new Item(-243, 0, 1){ NetworkId=8 }, /*minecraft:warped_planks*/
            new Item(139, 0, 1){ NetworkId=9 }, /*minecraft:cobblestone_wall*/
            new Item(139, 0, 1){ NetworkId=10 }, /*minecraft:cobblestone_wall*/
            new Item(139, 0, 1){ NetworkId=11 }, /*minecraft:cobblestone_wall*/
            new Item(139, 0, 1){ NetworkId=12 }, /*minecraft:cobblestone_wall*/
            new Item(139, 0, 1){ NetworkId=13 }, /*minecraft:cobblestone_wall*/
            new Item(139, 0, 1){ NetworkId=14 }, /*minecraft:cobblestone_wall*/
            new Item(139, 0, 1){ NetworkId=15 }, /*minecraft:cobblestone_wall*/
            new Item(139, 0, 1){ NetworkId=16 }, /*minecraft:cobblestone_wall*/
            new Item(139, 0, 1){ NetworkId=17 }, /*minecraft:cobblestone_wall*/
            new Item(139, 0, 1){ NetworkId=18 }, /*minecraft:cobblestone_wall*/
            new Item(139, 0, 1){ NetworkId=19 }, /*minecraft:cobblestone_wall*/
            new Item(139, 0, 1){ NetworkId=20 }, /*minecraft:cobblestone_wall*/
            new Item(139, 0, 1){ NetworkId=21 }, /*minecraft:cobblestone_wall*/
            new Item(139, 0, 1){ NetworkId=22 }, /*minecraft:cobblestone_wall*/
            new Item(-277, 0, 1){ NetworkId=23 }, /*minecraft:blackstone_wall*/
            new Item(-297, 0, 1){ NetworkId=24 }, /*minecraft:polished_blackstone_wall*/
            new Item(-278, 0, 1){ NetworkId=25 }, /*minecraft:polished_blackstone_brick_wall*/
            new Item(-382, 0, 1){ NetworkId=26 }, /**/
            new Item(-390, 0, 1){ NetworkId=27 }, /**/
            new Item(-386, 0, 1){ NetworkId=28 }, /**/
            new Item(-394, 0, 1){ NetworkId=29 }, /**/
            new Item(85, 0, 1){ NetworkId=30 }, /*minecraft:fence*/
            new Item(85, 0, 1){ NetworkId=31 }, /*minecraft:fence*/
            new Item(85, 0, 1){ NetworkId=32 }, /*minecraft:fence*/
            new Item(85, 0, 1){ NetworkId=33 }, /*minecraft:fence*/
            new Item(85, 0, 1){ NetworkId=34 }, /*minecraft:fence*/
            new Item(85, 0, 1){ NetworkId=35 }, /*minecraft:fence*/
            new Item(113, 0, 1){ NetworkId=36 }, /*minecraft:nether_brick_fence*/
            new Item(-256, 0, 1){ NetworkId=37 }, /*minecraft:crimson_fence*/
            new Item(-257, 0, 1){ NetworkId=38 }, /*minecraft:warped_fence*/
            new Item(107, 0, 1){ NetworkId=39 }, /*minecraft:fence_gate*/
            new Item(183, 0, 1){ NetworkId=40 }, /*minecraft:spruce_fence_gate*/
            new Item(184, 0, 1){ NetworkId=41 }, /*minecraft:birch_fence_gate*/
            new Item(185, 0, 1){ NetworkId=42 }, /*minecraft:jungle_fence_gate*/
            new Item(187, 0, 1){ NetworkId=43 }, /*minecraft:acacia_fence_gate*/
            new Item(186, 0, 1){ NetworkId=44 }, /*minecraft:dark_oak_fence_gate*/
            new Item(-258, 0, 1){ NetworkId=45 }, /*minecraft:crimson_fence_gate*/
            new Item(-259, 0, 1){ NetworkId=46 }, /*minecraft:warped_fence_gate*/
            new Item(-180, 0, 1){ NetworkId=47 }, /*minecraft:normal_stone_stairs*/
            new Item(67, 0, 1){ NetworkId=48 }, /*minecraft:stone_stairs*/
            new Item(-179, 0, 1){ NetworkId=49 }, /*minecraft:mossy_cobblestone_stairs*/
            new Item(53, 0, 1){ NetworkId=50 }, /*minecraft:oak_stairs*/
            new Item(134, 0, 1){ NetworkId=51 }, /*minecraft:spruce_stairs*/
            new Item(135, 0, 1){ NetworkId=52 }, /*minecraft:birch_stairs*/
            new Item(136, 0, 1){ NetworkId=53 }, /*minecraft:jungle_stairs*/
            new Item(163, 0, 1){ NetworkId=54 }, /*minecraft:acacia_stairs*/
            new Item(164, 0, 1){ NetworkId=55 }, /*minecraft:dark_oak_stairs*/
            new Item(109, 0, 1){ NetworkId=56 }, /*minecraft:stone_brick_stairs*/
            new Item(-175, 0, 1){ NetworkId=57 }, /*minecraft:mossy_stone_brick_stairs*/
            new Item(128, 0, 1){ NetworkId=58 }, /*minecraft:sandstone_stairs*/
            new Item(-177, 0, 1){ NetworkId=59 }, /*minecraft:smooth_sandstone_stairs*/
            new Item(180, 0, 1){ NetworkId=60 }, /*minecraft:red_sandstone_stairs*/
            new Item(-176, 0, 1){ NetworkId=61 }, /*minecraft:smooth_red_sandstone_stairs*/
            new Item(-169, 0, 1){ NetworkId=62 }, /*minecraft:granite_stairs*/
            new Item(-172, 0, 1){ NetworkId=63 }, /*minecraft:polished_granite_stairs*/
            new Item(-170, 0, 1){ NetworkId=64 }, /*minecraft:diorite_stairs*/
            new Item(-173, 0, 1){ NetworkId=65 }, /*minecraft:polished_diorite_stairs*/
            new Item(-171, 0, 1){ NetworkId=66 }, /*minecraft:andesite_stairs*/
            new Item(-174, 0, 1){ NetworkId=67 }, /*minecraft:polished_andesite_stairs*/
            new Item(108, 0, 1){ NetworkId=68 }, /*minecraft:brick_stairs*/
            new Item(114, 0, 1){ NetworkId=69 }, /*minecraft:nether_brick_stairs*/
            new Item(-184, 0, 1){ NetworkId=70 }, /*minecraft:red_nether_brick_stairs*/
            new Item(-178, 0, 1){ NetworkId=71 }, /*minecraft:end_brick_stairs*/
            new Item(156, 0, 1){ NetworkId=72 }, /*minecraft:quartz_stairs*/
            new Item(-185, 0, 1){ NetworkId=73 }, /*minecraft:smooth_quartz_stairs*/
            new Item(203, 0, 1){ NetworkId=74 }, /*minecraft:purpur_stairs*/
            new Item(-2, 0, 1){ NetworkId=75 }, /*minecraft:prismarine_stairs*/
            new Item(-3, 0, 1){ NetworkId=76 }, /*minecraft:dark_prismarine_stairs*/
            new Item(-4, 0, 1){ NetworkId=77 }, /*minecraft:prismarine_bricks_stairs*/
            new Item(-254, 0, 1){ NetworkId=78 }, /*minecraft:crimson_stairs*/
            new Item(-255, 0, 1){ NetworkId=79 }, /*minecraft:warped_stairs*/
            new Item(-276, 0, 1){ NetworkId=80 }, /*minecraft:blackstone_stairs*/
            new Item(-292, 0, 1){ NetworkId=81 }, /*minecraft:polished_blackstone_stairs*/
            new Item(-275, 0, 1){ NetworkId=82 }, /*minecraft:polished_blackstone_brick_stairs*/
            new Item(-354, 0, 1){ NetworkId=83 }, /**/
            new Item(-355, 0, 1){ NetworkId=84 }, /**/
            new Item(-356, 0, 1){ NetworkId=85 }, /**/
            new Item(-357, 0, 1){ NetworkId=86 }, /**/
            new Item(-358, 0, 1){ NetworkId=87 }, /**/
            new Item(-359, 0, 1){ NetworkId=88 }, /**/
            new Item(-360, 0, 1){ NetworkId=89 }, /**/
            new Item(-448, 0, 1){ NetworkId=90 }, /**/
            new Item(-381, 0, 1){ NetworkId=91 }, /**/
            new Item(-389, 0, 1){ NetworkId=92 }, /**/
            new Item(-385, 0, 1){ NetworkId=93 }, /**/
            new Item(-393, 0, 1){ NetworkId=94 }, /**/
            new Item(324, 0, 1){ NetworkId=95 }, /*minecraft:wooden_door*/
            new Item(427, 0, 1){ NetworkId=96 }, /*minecraft:spruce_door*/
            new Item(428, 0, 1){ NetworkId=97 }, /*minecraft:birch_door*/
            new Item(429, 0, 1){ NetworkId=98 }, /*minecraft:jungle_door*/
            new Item(430, 0, 1){ NetworkId=99 }, /*minecraft:acacia_door*/
            new Item(431, 0, 1){ NetworkId=100 }, /*minecraft:dark_oak_door*/
            new Item(330, 0, 1){ NetworkId=101 }, /*minecraft:iron_door*/
            new Item(755, 0, 1){ NetworkId=102 }, /*minecraft:crimson_door*/
            new Item(756, 0, 1){ NetworkId=103 }, /*minecraft:warped_door*/
            new Item(96, 0, 1){ NetworkId=104 }, /*minecraft:trapdoor*/
            new Item(-149, 0, 1){ NetworkId=105 }, /*minecraft:spruce_trapdoor*/
            new Item(-146, 0, 1){ NetworkId=106 }, /*minecraft:birch_trapdoor*/
            new Item(-148, 0, 1){ NetworkId=107 }, /*minecraft:jungle_trapdoor*/
            new Item(-145, 0, 1){ NetworkId=108 }, /*minecraft:acacia_trapdoor*/
            new Item(-147, 0, 1){ NetworkId=109 }, /*minecraft:dark_oak_trapdoor*/
            new Item(167, 0, 1){ NetworkId=110 }, /*minecraft:iron_trapdoor*/
            new Item(-246, 0, 1){ NetworkId=111 }, /*minecraft:crimson_trapdoor*/
            new Item(-247, 0, 1){ NetworkId=112 }, /*minecraft:warped_trapdoor*/
            new Item(101, 0, 1){ NetworkId=113 }, /*minecraft:iron_bars*/
            new Item(20, 0, 1){ NetworkId=114 }, /*minecraft:glass*/
            new Item(241, 0, 1){ NetworkId=115 }, /*minecraft:stained_glass*/
            new Item(241, 0, 1){ NetworkId=116 }, /*minecraft:stained_glass*/
            new Item(241, 0, 1){ NetworkId=117 }, /*minecraft:stained_glass*/
            new Item(241, 0, 1){ NetworkId=118 }, /*minecraft:stained_glass*/
            new Item(241, 0, 1){ NetworkId=119 }, /*minecraft:stained_glass*/
            new Item(241, 0, 1){ NetworkId=120 }, /*minecraft:stained_glass*/
            new Item(241, 0, 1){ NetworkId=121 }, /*minecraft:stained_glass*/
            new Item(241, 0, 1){ NetworkId=122 }, /*minecraft:stained_glass*/
            new Item(241, 0, 1){ NetworkId=123 }, /*minecraft:stained_glass*/
            new Item(241, 0, 1){ NetworkId=124 }, /*minecraft:stained_glass*/
            new Item(241, 0, 1){ NetworkId=125 }, /*minecraft:stained_glass*/
            new Item(241, 0, 1){ NetworkId=126 }, /*minecraft:stained_glass*/
            new Item(241, 0, 1){ NetworkId=127 }, /*minecraft:stained_glass*/
            new Item(241, 0, 1){ NetworkId=128 }, /*minecraft:stained_glass*/
            new Item(241, 0, 1){ NetworkId=129 }, /*minecraft:stained_glass*/
            new Item(241, 0, 1){ NetworkId=130 }, /*minecraft:stained_glass*/
            new Item(-334, 0, 1){ NetworkId=131 }, /**/
            new Item(102, 0, 1){ NetworkId=132 }, /*minecraft:glass_pane*/
            new Item(160, 0, 1){ NetworkId=133 }, /*minecraft:stained_glass_pane*/
            new Item(160, 0, 1){ NetworkId=134 }, /*minecraft:stained_glass_pane*/
            new Item(160, 0, 1){ NetworkId=135 }, /*minecraft:stained_glass_pane*/
            new Item(160, 0, 1){ NetworkId=136 }, /*minecraft:stained_glass_pane*/
            new Item(160, 0, 1){ NetworkId=137 }, /*minecraft:stained_glass_pane*/
            new Item(160, 0, 1){ NetworkId=138 }, /*minecraft:stained_glass_pane*/
            new Item(160, 0, 1){ NetworkId=139 }, /*minecraft:stained_glass_pane*/
            new Item(160, 0, 1){ NetworkId=140 }, /*minecraft:stained_glass_pane*/
            new Item(160, 0, 1){ NetworkId=141 }, /*minecraft:stained_glass_pane*/
            new Item(160, 0, 1){ NetworkId=142 }, /*minecraft:stained_glass_pane*/
            new Item(160, 0, 1){ NetworkId=143 }, /*minecraft:stained_glass_pane*/
            new Item(160, 0, 1){ NetworkId=144 }, /*minecraft:stained_glass_pane*/
            new Item(160, 0, 1){ NetworkId=145 }, /*minecraft:stained_glass_pane*/
            new Item(160, 0, 1){ NetworkId=146 }, /*minecraft:stained_glass_pane*/
            new Item(160, 0, 1){ NetworkId=147 }, /*minecraft:stained_glass_pane*/
            new Item(160, 0, 1){ NetworkId=148 }, /*minecraft:stained_glass_pane*/
            new Item(65, 0, 1){ NetworkId=149 }, /*minecraft:ladder*/
            new Item(-165, 0, 1){ NetworkId=150 }, /*minecraft:scaffolding*/
            new Item(44, 0, 1){ NetworkId=151 }, /*minecraft:stone_slab*/
            new Item(-166, 0, 1){ NetworkId=152 }, /*minecraft:stone_slab4*/
            new Item(44, 0, 1){ NetworkId=153 }, /*minecraft:stone_slab*/
            new Item(182, 0, 1){ NetworkId=154 }, /*minecraft:stone_slab2*/
            new Item(158, 0, 1){ NetworkId=155 }, /*minecraft:wooden_slab*/
            new Item(158, 0, 1){ NetworkId=156 }, /*minecraft:wooden_slab*/
            new Item(158, 0, 1){ NetworkId=157 }, /*minecraft:wooden_slab*/
            new Item(158, 0, 1){ NetworkId=158 }, /*minecraft:wooden_slab*/
            new Item(158, 0, 1){ NetworkId=159 }, /*minecraft:wooden_slab*/
            new Item(158, 0, 1){ NetworkId=160 }, /*minecraft:wooden_slab*/
            new Item(44, 0, 1){ NetworkId=161 }, /*minecraft:stone_slab*/
            new Item(-166, 0, 1){ NetworkId=162 }, /*minecraft:stone_slab4*/
            new Item(44, 0, 1){ NetworkId=163 }, /*minecraft:stone_slab*/
            new Item(-166, 0, 1){ NetworkId=164 }, /*minecraft:stone_slab4*/
            new Item(182, 0, 1){ NetworkId=165 }, /*minecraft:stone_slab2*/
            new Item(182, 0, 1){ NetworkId=166 }, /*minecraft:stone_slab2*/
            new Item(-166, 0, 1){ NetworkId=167 }, /*minecraft:stone_slab4*/
            new Item(-162, 0, 1){ NetworkId=168 }, /*minecraft:stone_slab3*/
            new Item(-162, 0, 1){ NetworkId=169 }, /*minecraft:stone_slab3*/
            new Item(-162, 0, 1){ NetworkId=170 }, /*minecraft:stone_slab3*/
            new Item(-162, 0, 1){ NetworkId=171 }, /*minecraft:stone_slab3*/
            new Item(-162, 0, 1){ NetworkId=172 }, /*minecraft:stone_slab3*/
            new Item(-162, 0, 1){ NetworkId=173 }, /*minecraft:stone_slab3*/
            new Item(-162, 0, 1){ NetworkId=174 }, /*minecraft:stone_slab3*/
            new Item(44, 0, 1){ NetworkId=175 }, /*minecraft:stone_slab*/
            new Item(44, 0, 1){ NetworkId=176 }, /*minecraft:stone_slab*/
            new Item(182, 0, 1){ NetworkId=177 }, /*minecraft:stone_slab2*/
            new Item(-162, 0, 1){ NetworkId=178 }, /*minecraft:stone_slab3*/
            new Item(44, 0, 1){ NetworkId=179 }, /*minecraft:stone_slab*/
            new Item(-166, 0, 1){ NetworkId=180 }, /*minecraft:stone_slab4*/
            new Item(182, 0, 1){ NetworkId=181 }, /*minecraft:stone_slab2*/
            new Item(182, 0, 1){ NetworkId=182 }, /*minecraft:stone_slab2*/
            new Item(182, 0, 1){ NetworkId=183 }, /*minecraft:stone_slab2*/
            new Item(182, 0, 1){ NetworkId=184 }, /*minecraft:stone_slab2*/
            new Item(-264, 0, 1){ NetworkId=185 }, /*minecraft:crimson_slab*/
            new Item(-265, 0, 1){ NetworkId=186 }, /*minecraft:warped_slab*/
            new Item(-282, 0, 1){ NetworkId=187 }, /*minecraft:blackstone_slab*/
            new Item(-293, 0, 1){ NetworkId=188 }, /*minecraft:polished_blackstone_slab*/
            new Item(-284, 0, 1){ NetworkId=189 }, /*minecraft:polished_blackstone_brick_slab*/
            new Item(-361, 0, 1){ NetworkId=190 }, /**/
            new Item(-362, 0, 1){ NetworkId=191 }, /**/
            new Item(-363, 0, 1){ NetworkId=192 }, /**/
            new Item(-364, 0, 1){ NetworkId=193 }, /**/
            new Item(-365, 0, 1){ NetworkId=194 }, /**/
            new Item(-366, 0, 1){ NetworkId=195 }, /**/
            new Item(-367, 0, 1){ NetworkId=196 }, /**/
            new Item(-449, 0, 1){ NetworkId=197 }, /**/
            new Item(-380, 0, 1){ NetworkId=198 }, /**/
            new Item(-384, 0, 1){ NetworkId=199 }, /**/
            new Item(-388, 0, 1){ NetworkId=200 }, /**/
            new Item(-392, 0, 1){ NetworkId=201 }, /**/
            new Item(45, 0, 1){ NetworkId=202 }, /*minecraft:brick_block*/
            new Item(-302, 0, 1){ NetworkId=203 }, /*minecraft:chiseled_nether_bricks*/
            new Item(-303, 0, 1){ NetworkId=204 }, /*minecraft:cracked_nether_bricks*/
            new Item(-304, 0, 1){ NetworkId=205 }, /*minecraft:quartz_bricks*/
            new Item(98, 0, 1){ NetworkId=206 }, /*minecraft:stonebrick*/
            new Item(98, 0, 1){ NetworkId=207 }, /*minecraft:stonebrick*/
            new Item(98, 0, 1){ NetworkId=208 }, /*minecraft:stonebrick*/
            new Item(98, 0, 1){ NetworkId=209 }, /*minecraft:stonebrick*/
            new Item(206, 0, 1){ NetworkId=210 }, /*minecraft:end_bricks*/
            new Item(168, 0, 1){ NetworkId=211 }, /*minecraft:prismarine*/
            new Item(-274, 0, 1){ NetworkId=212 }, /*minecraft:polished_blackstone_bricks*/
            new Item(-280, 0, 1){ NetworkId=213 }, /*minecraft:cracked_polished_blackstone_bricks*/
            new Item(-281, 0, 1){ NetworkId=214 }, /*minecraft:gilded_blackstone*/
            new Item(-279, 0, 1){ NetworkId=215 }, /*minecraft:chiseled_polished_blackstone*/
            new Item(-387, 0, 1){ NetworkId=216 }, /**/
            new Item(-409, 0, 1){ NetworkId=217 }, /**/
            new Item(-391, 0, 1){ NetworkId=218 }, /**/
            new Item(-410, 0, 1){ NetworkId=219 }, /**/
            new Item(-395, 0, 1){ NetworkId=220 }, /**/
            new Item(4, 0, 1){ NetworkId=221 }, /*minecraft:cobblestone*/
            new Item(48, 0, 1){ NetworkId=222 }, /*minecraft:mossy_cobblestone*/
            new Item(-379, 0, 1){ NetworkId=223 }, /**/
            new Item(-183, 0, 1){ NetworkId=224 }, /*minecraft:smooth_stone*/
            new Item(24, 0, 1){ NetworkId=225 }, /*minecraft:sandstone*/
            new Item(24, 0, 1){ NetworkId=226 }, /*minecraft:sandstone*/
            new Item(24, 0, 1){ NetworkId=227 }, /*minecraft:sandstone*/
            new Item(24, 0, 1){ NetworkId=228 }, /*minecraft:sandstone*/
            new Item(179, 0, 1){ NetworkId=229 }, /*minecraft:red_sandstone*/
            new Item(179, 0, 1){ NetworkId=230 }, /*minecraft:red_sandstone*/
            new Item(179, 0, 1){ NetworkId=231 }, /*minecraft:red_sandstone*/
            new Item(179, 0, 1){ NetworkId=232 }, /*minecraft:red_sandstone*/
            new Item(173, 0, 1){ NetworkId=233 }, /*minecraft:coal_block*/
            new Item(-139, 0, 1){ NetworkId=234 }, /*minecraft:dried_kelp_block*/
            new Item(41, 0, 1){ NetworkId=235 }, /*minecraft:gold_block*/
            new Item(42, 0, 1){ NetworkId=236 }, /*minecraft:iron_block*/
            new Item(-340, 0, 1){ NetworkId=237 }, /**/
            new Item(-341, 0, 1){ NetworkId=238 }, /**/
            new Item(-342, 0, 1){ NetworkId=239 }, /**/
            new Item(-343, 0, 1){ NetworkId=240 }, /**/
            new Item(-344, 0, 1){ NetworkId=241 }, /**/
            new Item(-345, 0, 1){ NetworkId=242 }, /**/
            new Item(-346, 0, 1){ NetworkId=243 }, /**/
            new Item(-446, 0, 1){ NetworkId=244 }, /**/
            new Item(-347, 0, 1){ NetworkId=245 }, /**/
            new Item(-348, 0, 1){ NetworkId=246 }, /**/
            new Item(-349, 0, 1){ NetworkId=247 }, /**/
            new Item(-350, 0, 1){ NetworkId=248 }, /**/
            new Item(-351, 0, 1){ NetworkId=249 }, /**/
            new Item(-352, 0, 1){ NetworkId=250 }, /**/
            new Item(-353, 0, 1){ NetworkId=251 }, /**/
            new Item(-447, 0, 1){ NetworkId=252 }, /**/
            new Item(133, 0, 1){ NetworkId=253 }, /*minecraft:emerald_block*/
            new Item(57, 0, 1){ NetworkId=254 }, /*minecraft:diamond_block*/
            new Item(22, 0, 1){ NetworkId=255 }, /*minecraft:lapis_block*/
            new Item(-451, 0, 1){ NetworkId=256 }, /**/
            new Item(-452, 0, 1){ NetworkId=257 }, /**/
            new Item(-453, 0, 1){ NetworkId=258 }, /**/
            new Item(155, 0, 1){ NetworkId=259 }, /*minecraft:quartz_block*/
            new Item(155, 0, 1){ NetworkId=260 }, /*minecraft:quartz_block*/
            new Item(155, 0, 1){ NetworkId=261 }, /*minecraft:quartz_block*/
            new Item(155, 0, 1){ NetworkId=262 }, /*minecraft:quartz_block*/
            new Item(168, 0, 1){ NetworkId=263 }, /*minecraft:prismarine*/
            new Item(168, 0, 1){ NetworkId=264 }, /*minecraft:prismarine*/
            new Item(165, 0, 1){ NetworkId=265 }, /*minecraft:slime*/
            new Item(-220, 0, 1){ NetworkId=266 }, /*minecraft:honey_block*/
            new Item(-221, 0, 1){ NetworkId=267 }, /*minecraft:honeycomb_block*/
            new Item(170, 0, 1){ NetworkId=268 }, /*minecraft:hay_block*/
            new Item(216, 0, 1){ NetworkId=269 }, /*minecraft:bone_block*/
            new Item(112, 0, 1){ NetworkId=270 }, /*minecraft:nether_brick*/
            new Item(215, 0, 1){ NetworkId=271 }, /*minecraft:red_nether_brick*/
            new Item(-270, 0, 1){ NetworkId=272 }, /*minecraft:netherite_block*/
            new Item(-222, 0, 1){ NetworkId=273 }, /*minecraft:lodestone*/
            new Item(35, 0, 1){ NetworkId=274 }, /*minecraft:wool*/
            new Item(35, 0, 1){ NetworkId=275 }, /*minecraft:wool*/
            new Item(35, 0, 1){ NetworkId=276 }, /*minecraft:wool*/
            new Item(35, 0, 1){ NetworkId=277 }, /*minecraft:wool*/
            new Item(35, 0, 1){ NetworkId=278 }, /*minecraft:wool*/
            new Item(35, 0, 1){ NetworkId=279 }, /*minecraft:wool*/
            new Item(35, 0, 1){ NetworkId=280 }, /*minecraft:wool*/
            new Item(35, 0, 1){ NetworkId=281 }, /*minecraft:wool*/
            new Item(35, 0, 1){ NetworkId=282 }, /*minecraft:wool*/
            new Item(35, 0, 1){ NetworkId=283 }, /*minecraft:wool*/
            new Item(35, 0, 1){ NetworkId=284 }, /*minecraft:wool*/
            new Item(35, 0, 1){ NetworkId=285 }, /*minecraft:wool*/
            new Item(35, 0, 1){ NetworkId=286 }, /*minecraft:wool*/
            new Item(35, 0, 1){ NetworkId=287 }, /*minecraft:wool*/
            new Item(35, 0, 1){ NetworkId=288 }, /*minecraft:wool*/
            new Item(35, 0, 1){ NetworkId=289 }, /*minecraft:wool*/
            new Item(171, 0, 1){ NetworkId=290 }, /*minecraft:carpet*/
            new Item(171, 0, 1){ NetworkId=291 }, /*minecraft:carpet*/
            new Item(171, 0, 1){ NetworkId=292 }, /*minecraft:carpet*/
            new Item(171, 0, 1){ NetworkId=293 }, /*minecraft:carpet*/
            new Item(171, 0, 1){ NetworkId=294 }, /*minecraft:carpet*/
            new Item(171, 0, 1){ NetworkId=295 }, /*minecraft:carpet*/
            new Item(171, 0, 1){ NetworkId=296 }, /*minecraft:carpet*/
            new Item(171, 0, 1){ NetworkId=297 }, /*minecraft:carpet*/
            new Item(171, 0, 1){ NetworkId=298 }, /*minecraft:carpet*/
            new Item(171, 0, 1){ NetworkId=299 }, /*minecraft:carpet*/
            new Item(171, 0, 1){ NetworkId=300 }, /*minecraft:carpet*/
            new Item(171, 0, 1){ NetworkId=301 }, /*minecraft:carpet*/
            new Item(171, 0, 1){ NetworkId=302 }, /*minecraft:carpet*/
            new Item(171, 0, 1){ NetworkId=303 }, /*minecraft:carpet*/
            new Item(171, 0, 1){ NetworkId=304 }, /*minecraft:carpet*/
            new Item(171, 0, 1){ NetworkId=305 }, /*minecraft:carpet*/
            new Item(237, 0, 1){ NetworkId=306 }, /*minecraft:concretePowder*/
            new Item(237, 0, 1){ NetworkId=307 }, /*minecraft:concretePowder*/
            new Item(237, 0, 1){ NetworkId=308 }, /*minecraft:concretePowder*/
            new Item(237, 0, 1){ NetworkId=309 }, /*minecraft:concretePowder*/
            new Item(237, 0, 1){ NetworkId=310 }, /*minecraft:concretePowder*/
            new Item(237, 0, 1){ NetworkId=311 }, /*minecraft:concretePowder*/
            new Item(237, 0, 1){ NetworkId=312 }, /*minecraft:concretePowder*/
            new Item(237, 0, 1){ NetworkId=313 }, /*minecraft:concretePowder*/
            new Item(237, 0, 1){ NetworkId=314 }, /*minecraft:concretePowder*/
            new Item(237, 0, 1){ NetworkId=315 }, /*minecraft:concretePowder*/
            new Item(237, 0, 1){ NetworkId=316 }, /*minecraft:concretePowder*/
            new Item(237, 0, 1){ NetworkId=317 }, /*minecraft:concretePowder*/
            new Item(237, 0, 1){ NetworkId=318 }, /*minecraft:concretePowder*/
            new Item(237, 0, 1){ NetworkId=319 }, /*minecraft:concretePowder*/
            new Item(237, 0, 1){ NetworkId=320 }, /*minecraft:concretePowder*/
            new Item(237, 0, 1){ NetworkId=321 }, /*minecraft:concretePowder*/
            new Item(236, 0, 1){ NetworkId=322 }, /*minecraft:concrete*/
            new Item(236, 0, 1){ NetworkId=323 }, /*minecraft:concrete*/
            new Item(236, 0, 1){ NetworkId=324 }, /*minecraft:concrete*/
            new Item(236, 0, 1){ NetworkId=325 }, /*minecraft:concrete*/
            new Item(236, 0, 1){ NetworkId=326 }, /*minecraft:concrete*/
            new Item(236, 0, 1){ NetworkId=327 }, /*minecraft:concrete*/
            new Item(236, 0, 1){ NetworkId=328 }, /*minecraft:concrete*/
            new Item(236, 0, 1){ NetworkId=329 }, /*minecraft:concrete*/
            new Item(236, 0, 1){ NetworkId=330 }, /*minecraft:concrete*/
            new Item(236, 0, 1){ NetworkId=331 }, /*minecraft:concrete*/
            new Item(236, 0, 1){ NetworkId=332 }, /*minecraft:concrete*/
            new Item(236, 0, 1){ NetworkId=333 }, /*minecraft:concrete*/
            new Item(236, 0, 1){ NetworkId=334 }, /*minecraft:concrete*/
            new Item(236, 0, 1){ NetworkId=335 }, /*minecraft:concrete*/
            new Item(236, 0, 1){ NetworkId=336 }, /*minecraft:concrete*/
            new Item(236, 0, 1){ NetworkId=337 }, /*minecraft:concrete*/
            new Item(82, 0, 1){ NetworkId=338 }, /*minecraft:clay*/
            new Item(172, 0, 1){ NetworkId=339 }, /*minecraft:hardened_clay*/
            new Item(159, 0, 1){ NetworkId=340 }, /*minecraft:stained_hardened_clay*/
            new Item(159, 0, 1){ NetworkId=341 }, /*minecraft:stained_hardened_clay*/
            new Item(159, 0, 1){ NetworkId=342 }, /*minecraft:stained_hardened_clay*/
            new Item(159, 0, 1){ NetworkId=343 }, /*minecraft:stained_hardened_clay*/
            new Item(159, 0, 1){ NetworkId=344 }, /*minecraft:stained_hardened_clay*/
            new Item(159, 0, 1){ NetworkId=345 }, /*minecraft:stained_hardened_clay*/
            new Item(159, 0, 1){ NetworkId=346 }, /*minecraft:stained_hardened_clay*/
            new Item(159, 0, 1){ NetworkId=347 }, /*minecraft:stained_hardened_clay*/
            new Item(159, 0, 1){ NetworkId=348 }, /*minecraft:stained_hardened_clay*/
            new Item(159, 0, 1){ NetworkId=349 }, /*minecraft:stained_hardened_clay*/
            new Item(159, 0, 1){ NetworkId=350 }, /*minecraft:stained_hardened_clay*/
            new Item(159, 0, 1){ NetworkId=351 }, /*minecraft:stained_hardened_clay*/
            new Item(159, 0, 1){ NetworkId=352 }, /*minecraft:stained_hardened_clay*/
            new Item(159, 0, 1){ NetworkId=353 }, /*minecraft:stained_hardened_clay*/
            new Item(159, 0, 1){ NetworkId=354 }, /*minecraft:stained_hardened_clay*/
            new Item(159, 0, 1){ NetworkId=355 }, /*minecraft:stained_hardened_clay*/
            new Item(220, 0, 1){ NetworkId=356 }, /*minecraft:white_glazed_terracotta*/
            new Item(228, 0, 1){ NetworkId=357 }, /*minecraft:silver_glazed_terracotta*/
            new Item(227, 0, 1){ NetworkId=358 }, /*minecraft:gray_glazed_terracotta*/
            new Item(235, 0, 1){ NetworkId=359 }, /*minecraft:black_glazed_terracotta*/
            new Item(232, 0, 1){ NetworkId=360 }, /*minecraft:brown_glazed_terracotta*/
            new Item(234, 0, 1){ NetworkId=361 }, /*minecraft:red_glazed_terracotta*/
            new Item(221, 0, 1){ NetworkId=362 }, /*minecraft:orange_glazed_terracotta*/
            new Item(224, 0, 1){ NetworkId=363 }, /*minecraft:yellow_glazed_terracotta*/
            new Item(225, 0, 1){ NetworkId=364 }, /*minecraft:lime_glazed_terracotta*/
            new Item(233, 0, 1){ NetworkId=365 }, /*minecraft:green_glazed_terracotta*/
            new Item(229, 0, 1){ NetworkId=366 }, /*minecraft:cyan_glazed_terracotta*/
            new Item(223, 0, 1){ NetworkId=367 }, /*minecraft:light_blue_glazed_terracotta*/
            new Item(231, 0, 1){ NetworkId=368 }, /*minecraft:blue_glazed_terracotta*/
            new Item(219, 0, 1){ NetworkId=369 }, /*minecraft:purple_glazed_terracotta*/
            new Item(222, 0, 1){ NetworkId=370 }, /*minecraft:magenta_glazed_terracotta*/
            new Item(226, 0, 1){ NetworkId=371 }, /*minecraft:pink_glazed_terracotta*/
            new Item(201, 0, 1){ NetworkId=372 }, /*minecraft:purpur_block*/
            new Item(201, 0, 1){ NetworkId=373 }, /*minecraft:purpur_block*/
            new Item(214, 0, 1){ NetworkId=374 }, /*minecraft:nether_wart_block*/
            new Item(-227, 0, 1){ NetworkId=375 }, /*minecraft:warped_wart_block*/
            new Item(-230, 0, 1){ NetworkId=376 }, /*minecraft:shroomlight*/
            new Item(-232, 0, 1){ NetworkId=377 }, /*minecraft:crimson_nylium*/
            new Item(-233, 0, 1){ NetworkId=378 }, /*minecraft:warped_nylium*/
            new Item(-234, 0, 1){ NetworkId=379 }, /*minecraft:basalt*/
            new Item(-235, 0, 1){ NetworkId=380 }, /*minecraft:polished_basalt*/
            new Item(-377, 0, 1){ NetworkId=381 }, /**/
            new Item(-236, 0, 1){ NetworkId=382 }, /*minecraft:soul_soil*/
            new Item(3, 0, 1){ NetworkId=383 }, /*minecraft:dirt*/
            new Item(3, 0, 1){ NetworkId=384 }, /*minecraft:dirt*/
            new Item(60, 0, 1){ NetworkId=385 }, /*minecraft:farmland*/
            new Item(2, 0, 1){ NetworkId=386 }, /*minecraft:grass*/
            new Item(198, 0, 1){ NetworkId=387 }, /*minecraft:grass_path*/
            new Item(243, 0, 1){ NetworkId=388 }, /*minecraft:podzol*/
            new Item(110, 0, 1){ NetworkId=389 }, /*minecraft:mycelium*/
            new Item(1, 0, 1){ NetworkId=390 }, /*minecraft:stone*/
            new Item(15, 0, 1){ NetworkId=391 }, /*minecraft:iron_ore*/
            new Item(14, 0, 1){ NetworkId=392 }, /*minecraft:gold_ore*/
            new Item(56, 0, 1){ NetworkId=393 }, /*minecraft:diamond_ore*/
            new Item(21, 0, 1){ NetworkId=394 }, /*minecraft:lapis_ore*/
            new Item(73, 0, 1){ NetworkId=395 }, /*minecraft:redstone_ore*/
            new Item(16, 0, 1){ NetworkId=396 }, /*minecraft:coal_ore*/
            new Item(-311, 0, 1){ NetworkId=397 }, /**/
            new Item(129, 0, 1){ NetworkId=398 }, /*minecraft:emerald_ore*/
            new Item(153, 0, 1){ NetworkId=399 }, /*minecraft:quartz_ore*/
            new Item(-288, 0, 1){ NetworkId=400 }, /*minecraft:nether_gold_ore*/
            new Item(-271, 0, 1){ NetworkId=401 }, /*minecraft:ancient_debris*/
            new Item(-401, 0, 1){ NetworkId=402 }, /**/
            new Item(-402, 0, 1){ NetworkId=403 }, /**/
            new Item(-405, 0, 1){ NetworkId=404 }, /**/
            new Item(-400, 0, 1){ NetworkId=405 }, /**/
            new Item(-403, 0, 1){ NetworkId=406 }, /**/
            new Item(-407, 0, 1){ NetworkId=407 }, /**/
            new Item(-406, 0, 1){ NetworkId=408 }, /**/
            new Item(-408, 0, 1){ NetworkId=409 }, /**/
            new Item(13, 0, 1){ NetworkId=410 }, /*minecraft:gravel*/
            new Item(1, 0, 1){ NetworkId=411 }, /*minecraft:stone*/
            new Item(1, 0, 1){ NetworkId=412 }, /*minecraft:stone*/
            new Item(1, 0, 1){ NetworkId=413 }, /*minecraft:stone*/
            new Item(-273, 0, 1){ NetworkId=414 }, /*minecraft:blackstone*/
            new Item(-378, 0, 1){ NetworkId=415 }, /**/
            new Item(1, 0, 1){ NetworkId=416 }, /*minecraft:stone*/
            new Item(1, 0, 1){ NetworkId=417 }, /*minecraft:stone*/
            new Item(1, 0, 1){ NetworkId=418 }, /*minecraft:stone*/
            new Item(-291, 0, 1){ NetworkId=419 }, /*minecraft:polished_blackstone*/
            new Item(-383, 0, 1){ NetworkId=420 }, /**/
            new Item(12, 0, 1){ NetworkId=421 }, /*minecraft:sand*/
            new Item(12, 0, 1){ NetworkId=422 }, /*minecraft:sand*/
            new Item(81, 0, 1){ NetworkId=423 }, /*minecraft:cactus*/
            new Item(17, 0, 1){ NetworkId=424 }, /*minecraft:log*/
            new Item(-10, 0, 1){ NetworkId=425 }, /*minecraft:stripped_oak_log*/
            new Item(17, 0, 1){ NetworkId=426 }, /*minecraft:log*/
            new Item(-5, 0, 1){ NetworkId=427 }, /*minecraft:stripped_spruce_log*/
            new Item(17, 0, 1){ NetworkId=428 }, /*minecraft:log*/
            new Item(-6, 0, 1){ NetworkId=429 }, /*minecraft:stripped_birch_log*/
            new Item(17, 0, 1){ NetworkId=430 }, /*minecraft:log*/
            new Item(-7, 0, 1){ NetworkId=431 }, /*minecraft:stripped_jungle_log*/
            new Item(162, 0, 1){ NetworkId=432 }, /*minecraft:log2*/
            new Item(-8, 0, 1){ NetworkId=433 }, /*minecraft:stripped_acacia_log*/
            new Item(162, 0, 1){ NetworkId=434 }, /*minecraft:log2*/
            new Item(-9, 0, 1){ NetworkId=435 }, /*minecraft:stripped_dark_oak_log*/
            new Item(-225, 0, 1){ NetworkId=436 }, /*minecraft:crimson_stem*/
            new Item(-240, 0, 1){ NetworkId=437 }, /*minecraft:stripped_crimson_stem*/
            new Item(-226, 0, 1){ NetworkId=438 }, /*minecraft:warped_stem*/
            new Item(-241, 0, 1){ NetworkId=439 }, /*minecraft:stripped_warped_stem*/
            new Item(-212, 0, 1){ NetworkId=440 }, /*minecraft:wood*/
            new Item(-212, 0, 1){ NetworkId=441 }, /*minecraft:wood*/
            new Item(-212, 0, 1){ NetworkId=442 }, /*minecraft:wood*/
            new Item(-212, 0, 1){ NetworkId=443 }, /*minecraft:wood*/
            new Item(-212, 0, 1){ NetworkId=444 }, /*minecraft:wood*/
            new Item(-212, 0, 1){ NetworkId=445 }, /*minecraft:wood*/
            new Item(-212, 0, 1){ NetworkId=446 }, /*minecraft:wood*/
            new Item(-212, 0, 1){ NetworkId=447 }, /*minecraft:wood*/
            new Item(-212, 0, 1){ NetworkId=448 }, /*minecraft:wood*/
            new Item(-212, 0, 1){ NetworkId=449 }, /*minecraft:wood*/
            new Item(-212, 0, 1){ NetworkId=450 }, /*minecraft:wood*/
            new Item(-212, 0, 1){ NetworkId=451 }, /*minecraft:wood*/
            new Item(-299, 0, 1){ NetworkId=452 }, /*minecraft:crimson_hyphae*/
            new Item(-300, 0, 1){ NetworkId=453 }, /*minecraft:stripped_crimson_hyphae*/
            new Item(-298, 0, 1){ NetworkId=454 }, /*minecraft:warped_hyphae*/
            new Item(-301, 0, 1){ NetworkId=455 }, /*minecraft:stripped_warped_hyphae*/
            new Item(18, 0, 1){ NetworkId=456 }, /*minecraft:leaves*/
            new Item(18, 0, 1){ NetworkId=457 }, /*minecraft:leaves*/
            new Item(18, 0, 1){ NetworkId=458 }, /*minecraft:leaves*/
            new Item(18, 0, 1){ NetworkId=459 }, /*minecraft:leaves*/
            new Item(161, 0, 1){ NetworkId=460 }, /*minecraft:leaves2*/
            new Item(161, 0, 1){ NetworkId=461 }, /*minecraft:leaves2*/
            new Item(-324, 0, 1){ NetworkId=462 }, /**/
            new Item(-325, 0, 1){ NetworkId=463 }, /**/
            new Item(6, 0, 1){ NetworkId=464 }, /*minecraft:sapling*/
            new Item(6, 0, 1){ NetworkId=465 }, /*minecraft:sapling*/
            new Item(6, 0, 1){ NetworkId=466 }, /*minecraft:sapling*/
            new Item(6, 0, 1){ NetworkId=467 }, /*minecraft:sapling*/
            new Item(6, 0, 1){ NetworkId=468 }, /*minecraft:sapling*/
            new Item(6, 0, 1){ NetworkId=469 }, /*minecraft:sapling*/
            new Item(-218, 0, 1){ NetworkId=470 }, /*minecraft:bee_nest*/
            new Item(295, 0, 1){ NetworkId=471 }, /*minecraft:wheat_seeds*/
            new Item(361, 0, 1){ NetworkId=472 }, /*minecraft:pumpkin_seeds*/
            new Item(362, 0, 1){ NetworkId=473 }, /*minecraft:melon_seeds*/
            new Item(458, 0, 1){ NetworkId=474 }, /*minecraft:beetroot_seeds*/
            new Item(296, 0, 1){ NetworkId=475 }, /*minecraft:wheat*/
            new Item(457, 0, 1){ NetworkId=476 }, /*minecraft:beetroot*/
            new Item(392, 0, 1){ NetworkId=477 }, /*minecraft:potato*/
            new Item(394, 0, 1){ NetworkId=478 }, /*minecraft:poisonous_potato*/
            new Item(391, 0, 1){ NetworkId=479 }, /*minecraft:carrot*/
            new Item(396, 0, 1){ NetworkId=480 }, /*minecraft:golden_carrot*/
            new Item(260, 0, 1){ NetworkId=481 }, /*minecraft:apple*/
            new Item(322, 0, 1){ NetworkId=482 }, /*minecraft:golden_apple*/
            new Item(466, 0, 1){ NetworkId=483 }, /*minecraft:enchanted_golden_apple*/
            new Item(103, 0, 1){ NetworkId=484 }, /*minecraft:melon_block*/
            new Item(360, 0, 1){ NetworkId=485 }, /*minecraft:melon_slice*/
            new Item(382, 0, 1){ NetworkId=486 }, /*minecraft:glistering_melon_slice*/
            new Item(477, 0, 1){ NetworkId=487 }, /*minecraft:sweet_berries*/
            new Item(630, 0, 1){ NetworkId=488 }, /**/
            new Item(86, 0, 1){ NetworkId=489 }, /*minecraft:pumpkin*/
            new Item(-155, 0, 1){ NetworkId=490 }, /*minecraft:carved_pumpkin*/
            new Item(91, 0, 1){ NetworkId=491 }, /*minecraft:lit_pumpkin*/
            new Item(736, 0, 1){ NetworkId=492 }, /*minecraft:honeycomb*/
            new Item(31, 0, 1){ NetworkId=493 }, /*minecraft:tallgrass*/
            new Item(175, 0, 1){ NetworkId=494 }, /*minecraft:double_plant*/
            new Item(31, 0, 1){ NetworkId=495 }, /*minecraft:tallgrass*/
            new Item(175, 0, 1){ NetworkId=496 }, /*minecraft:double_plant*/
            new Item(760, 0, 1){ NetworkId=497 }, /*minecraft:nether_sprouts*/
            new Item(-131, 0, 1){ NetworkId=498 }, /*minecraft:coral*/
            new Item(-131, 0, 1){ NetworkId=499 }, /*minecraft:coral*/
            new Item(-131, 0, 1){ NetworkId=500 }, /*minecraft:coral*/
            new Item(-131, 0, 1){ NetworkId=501 }, /*minecraft:coral*/
            new Item(-131, 0, 1){ NetworkId=502 }, /*minecraft:coral*/
            new Item(-131, 0, 1){ NetworkId=503 }, /*minecraft:coral*/
            new Item(-131, 0, 1){ NetworkId=504 }, /*minecraft:coral*/
            new Item(-131, 0, 1){ NetworkId=505 }, /*minecraft:coral*/
            new Item(-131, 0, 1){ NetworkId=506 }, /*minecraft:coral*/
            new Item(-131, 0, 1){ NetworkId=507 }, /*minecraft:coral*/
            new Item(-133, 0, 1){ NetworkId=508 }, /*minecraft:coral_fan*/
            new Item(-133, 0, 1){ NetworkId=509 }, /*minecraft:coral_fan*/
            new Item(-133, 0, 1){ NetworkId=510 }, /*minecraft:coral_fan*/
            new Item(-133, 0, 1){ NetworkId=511 }, /*minecraft:coral_fan*/
            new Item(-133, 0, 1){ NetworkId=512 }, /*minecraft:coral_fan*/
            new Item(-134, 0, 1){ NetworkId=513 }, /*minecraft:coral_fan_dead*/
            new Item(-134, 0, 1){ NetworkId=514 }, /*minecraft:coral_fan_dead*/
            new Item(-134, 0, 1){ NetworkId=515 }, /*minecraft:coral_fan_dead*/
            new Item(-134, 0, 1){ NetworkId=516 }, /*minecraft:coral_fan_dead*/
            new Item(-134, 0, 1){ NetworkId=517 }, /*minecraft:coral_fan_dead*/
            new Item(335, 0, 1){ NetworkId=518 }, /*minecraft:kelp*/
            new Item(-130, 0, 1){ NetworkId=519 }, /*minecraft:seagrass*/
            new Item(-223, 0, 1){ NetworkId=520 }, /*minecraft:crimson_roots*/
            new Item(-224, 0, 1){ NetworkId=521 }, /*minecraft:warped_roots*/
            new Item(37, 0, 1){ NetworkId=522 }, /*minecraft:yellow_flower*/
            new Item(38, 0, 1){ NetworkId=523 }, /*minecraft:red_flower*/
            new Item(38, 0, 1){ NetworkId=524 }, /*minecraft:red_flower*/
            new Item(38, 0, 1){ NetworkId=525 }, /*minecraft:red_flower*/
            new Item(38, 0, 1){ NetworkId=526 }, /*minecraft:red_flower*/
            new Item(38, 0, 1){ NetworkId=527 }, /*minecraft:red_flower*/
            new Item(38, 0, 1){ NetworkId=528 }, /*minecraft:red_flower*/
            new Item(38, 0, 1){ NetworkId=529 }, /*minecraft:red_flower*/
            new Item(38, 0, 1){ NetworkId=530 }, /*minecraft:red_flower*/
            new Item(38, 0, 1){ NetworkId=531 }, /*minecraft:red_flower*/
            new Item(38, 0, 1){ NetworkId=532 }, /*minecraft:red_flower*/
            new Item(38, 0, 1){ NetworkId=533 }, /*minecraft:red_flower*/
            new Item(175, 0, 1){ NetworkId=534 }, /*minecraft:double_plant*/
            new Item(175, 0, 1){ NetworkId=535 }, /*minecraft:double_plant*/
            new Item(175, 0, 1){ NetworkId=536 }, /*minecraft:double_plant*/
            new Item(175, 0, 1){ NetworkId=537 }, /*minecraft:double_plant*/
            new Item(-216, 0, 1){ NetworkId=538 }, /*minecraft:wither_rose*/
            new Item(351, 19, 1){ NetworkId=539 }, /*minecraft:dye*/
            new Item(351, 7, 1){ NetworkId=540 }, /*minecraft:dye*/
            new Item(351, 8, 1){ NetworkId=541 }, /*minecraft:dye*/
            new Item(351, 16, 1){ NetworkId=542 }, /*minecraft:dye*/
            new Item(351, 17, 1){ NetworkId=543 }, /*minecraft:dye*/
            new Item(351, 1, 1){ NetworkId=544 }, /*minecraft:dye*/
            new Item(351, 14, 1){ NetworkId=545 }, /*minecraft:dye*/
            new Item(351, 11, 1){ NetworkId=546 }, /*minecraft:dye*/
            new Item(351, 10, 1){ NetworkId=547 }, /*minecraft:dye*/
            new Item(351, 2, 1){ NetworkId=548 }, /*minecraft:dye*/
            new Item(351, 6, 1){ NetworkId=549 }, /*minecraft:dye*/
            new Item(351, 12, 1){ NetworkId=550 }, /*minecraft:dye*/
            new Item(351, 18, 1){ NetworkId=551 }, /*minecraft:dye*/
            new Item(351, 5, 1){ NetworkId=552 }, /*minecraft:dye*/
            new Item(351, 13, 1){ NetworkId=553 }, /*minecraft:dye*/
            new Item(351, 9, 1){ NetworkId=554 }, /*minecraft:dye*/
            new Item(351, 0, 1){ NetworkId=555 }, /*minecraft:dye*/
            new Item(503, 0, 1){ NetworkId=556 }, /*minecraft:music_disc_chirp*/
            new Item(351, 3, 1){ NetworkId=557 }, /*minecraft:dye*/
            new Item(351, 4, 1){ NetworkId=558 }, /*minecraft:dye*/
            new Item(351, 15, 1){ NetworkId=559 }, /*minecraft:dye*/
            new Item(106, 0, 1){ NetworkId=560 }, /*minecraft:vine*/
            new Item(-231, 0, 1){ NetworkId=561 }, /*minecraft:weeping_vines*/
            new Item(-287, 0, 1){ NetworkId=562 }, /*minecraft:twisting_vines*/
            new Item(111, 0, 1){ NetworkId=563 }, /*minecraft:waterlily*/
            new Item(32, 0, 1){ NetworkId=564 }, /*minecraft:deadbush*/
            new Item(-163, 0, 1){ NetworkId=565 }, /*minecraft:bamboo*/
            new Item(80, 0, 1){ NetworkId=566 }, /*minecraft:snow*/
            new Item(79, 0, 1){ NetworkId=567 }, /*minecraft:ice*/
            new Item(174, 0, 1){ NetworkId=568 }, /*minecraft:packed_ice*/
            new Item(-11, 0, 1){ NetworkId=569 }, /*minecraft:blue_ice*/
            new Item(78, 0, 1){ NetworkId=570 }, /*minecraft:snow_layer*/
            new Item(-308, 0, 1){ NetworkId=571 }, /**/
            new Item(-317, 0, 1){ NetworkId=572 }, /**/
            new Item(-335, 0, 1){ NetworkId=573 }, /**/
            new Item(-320, 0, 1){ NetworkId=574 }, /**/
            new Item(-318, 0, 1){ NetworkId=575 }, /**/
            new Item(-319, 0, 1){ NetworkId=576 }, /**/
            new Item(-323, 0, 1){ NetworkId=577 }, /**/
            new Item(-336, 0, 1){ NetworkId=578 }, /**/
            new Item(-321, 0, 1){ NetworkId=579 }, /**/
            new Item(-337, 0, 1){ NetworkId=580 }, /**/
            new Item(-338, 0, 1){ NetworkId=581 }, /**/
            new Item(-411, 0, 1){ NetworkId=582 }, /**/
            new Item(-327, 0, 1){ NetworkId=583 }, /**/
            new Item(-328, 0, 1){ NetworkId=584 }, /**/
            new Item(-329, 0, 1){ NetworkId=585 }, /**/
            new Item(-330, 0, 1){ NetworkId=586 }, /**/
            new Item(-331, 0, 1){ NetworkId=587 }, /**/
            new Item(-332, 0, 1){ NetworkId=588 }, /**/
            new Item(-333, 0, 1){ NetworkId=589 }, /**/
            new Item(-326, 0, 1){ NetworkId=590 }, /**/
            new Item(365, 0, 1){ NetworkId=591 }, /*minecraft:chicken*/
            new Item(319, 0, 1){ NetworkId=592 }, /*minecraft:porkchop*/
            new Item(363, 0, 1){ NetworkId=593 }, /*minecraft:beef*/
            new Item(423, 0, 1){ NetworkId=594 }, /*minecraft:mutton*/
            new Item(411, 0, 1){ NetworkId=595 }, /*minecraft:rabbit*/
            new Item(349, 0, 1){ NetworkId=596 }, /*minecraft:cod*/
            new Item(460, 0, 1){ NetworkId=597 }, /*minecraft:salmon*/
            new Item(461, 0, 1){ NetworkId=598 }, /*minecraft:tropical_fish*/
            new Item(462, 0, 1){ NetworkId=599 }, /*minecraft:pufferfish*/
            new Item(39, 0, 1){ NetworkId=600 }, /*minecraft:brown_mushroom*/
            new Item(40, 0, 1){ NetworkId=601 }, /*minecraft:red_mushroom*/
            new Item(-228, 0, 1){ NetworkId=602 }, /*minecraft:crimson_fungus*/
            new Item(-229, 0, 1){ NetworkId=603 }, /*minecraft:warped_fungus*/
            new Item(99, 0, 1){ NetworkId=604 }, /*minecraft:brown_mushroom_block*/
            new Item(100, 0, 1){ NetworkId=605 }, /*minecraft:red_mushroom_block*/
            new Item(99, 0, 1){ NetworkId=606 }, /*minecraft:brown_mushroom_block*/
            new Item(99, 0, 1){ NetworkId=607 }, /*minecraft:brown_mushroom_block*/
            new Item(344, 0, 1){ NetworkId=608 }, /*minecraft:egg*/
            new Item(338, 0, 1){ NetworkId=609 }, /*minecraft:item.reeds*/
            new Item(353, 0, 1){ NetworkId=610 }, /*minecraft:sugar*/
            new Item(367, 0, 1){ NetworkId=611 }, /*minecraft:rotten_flesh*/
            new Item(352, 0, 1){ NetworkId=612 }, /*minecraft:bone*/
            new Item(30, 0, 1){ NetworkId=613 }, /*minecraft:web*/
            new Item(375, 0, 1){ NetworkId=614 }, /*minecraft:spider_eye*/
            new Item(52, 0, 1){ NetworkId=615 }, /*minecraft:mob_spawner*/
            new Item(97, 0, 1){ NetworkId=616 }, /*minecraft:monster_egg*/
            new Item(97, 0, 1){ NetworkId=617 }, /*minecraft:monster_egg*/
            new Item(97, 0, 1){ NetworkId=618 }, /*minecraft:monster_egg*/
            new Item(97, 0, 1){ NetworkId=619 }, /*minecraft:monster_egg*/
            new Item(97, 0, 1){ NetworkId=620 }, /*minecraft:monster_egg*/
            new Item(97, 0, 1){ NetworkId=621 }, /*minecraft:monster_egg*/
            new Item(-454, 0, 1){ NetworkId=622 }, /**/
            new Item(122, 0, 1){ NetworkId=623 }, /*minecraft:dragon_egg*/
            new Item(-159, 0, 1){ NetworkId=624 }, /*minecraft:turtle_egg*/
            new Item(383, 10, 1){ NetworkId=625 }, /*minecraft:spawn_egg*/
            new Item(383, 122, 1){ NetworkId=626 }, /*minecraft:spawn_egg*/
            new Item(383, 11, 1){ NetworkId=627 }, /*minecraft:spawn_egg*/
            new Item(383, 12, 1){ NetworkId=628 }, /*minecraft:spawn_egg*/
            new Item(383, 13, 1){ NetworkId=629 }, /*minecraft:spawn_egg*/
            new Item(383, 14, 1){ NetworkId=630 }, /*minecraft:spawn_egg*/
            new Item(383, 28, 1){ NetworkId=631 }, /*minecraft:spawn_egg*/
            new Item(383, 22, 1){ NetworkId=632 }, /*minecraft:spawn_egg*/
            new Item(383, 75, 1){ NetworkId=633 }, /*minecraft:spawn_egg*/
            new Item(383, 16, 1){ NetworkId=634 }, /*minecraft:spawn_egg*/
            new Item(383, 19, 1){ NetworkId=635 }, /*minecraft:spawn_egg*/
            new Item(383, 30, 1){ NetworkId=636 }, /*minecraft:spawn_egg*/
            new Item(383, 18, 1){ NetworkId=637 }, /*minecraft:spawn_egg*/
            new Item(383, 29, 1){ NetworkId=638 }, /*minecraft:spawn_egg*/
            new Item(383, 23, 1){ NetworkId=639 }, /*minecraft:spawn_egg*/
            new Item(383, 24, 1){ NetworkId=640 }, /*minecraft:spawn_egg*/
            new Item(383, 25, 1){ NetworkId=641 }, /*minecraft:spawn_egg*/
            new Item(383, 26, 1){ NetworkId=642 }, /*minecraft:spawn_egg*/
            new Item(383, 27, 1){ NetworkId=643 }, /*minecraft:spawn_egg*/
            new Item(383, 111, 1){ NetworkId=644 }, /*minecraft:spawn_egg*/
            new Item(383, 112, 1){ NetworkId=645 }, /*minecraft:spawn_egg*/
            new Item(383, 108, 1){ NetworkId=646 }, /*minecraft:spawn_egg*/
            new Item(383, 109, 1){ NetworkId=647 }, /*minecraft:spawn_egg*/
            new Item(383, 31, 1){ NetworkId=648 }, /*minecraft:spawn_egg*/
            new Item(383, 74, 1){ NetworkId=649 }, /*minecraft:spawn_egg*/
            new Item(383, 113, 1){ NetworkId=650 }, /*minecraft:spawn_egg*/
            new Item(383, 121, 1){ NetworkId=651 }, /*minecraft:spawn_egg*/
            new Item(383, 33, 1){ NetworkId=652 }, /*minecraft:spawn_egg*/
            new Item(383, 38, 1){ NetworkId=653 }, /*minecraft:spawn_egg*/
            new Item(383, 39, 1){ NetworkId=654 }, /*minecraft:spawn_egg*/
            new Item(383, 34, 1){ NetworkId=655 }, /*minecraft:spawn_egg*/
            new Item(383, 48, 1){ NetworkId=656 }, /*minecraft:spawn_egg*/
            new Item(383, 46, 1){ NetworkId=657 }, /*minecraft:spawn_egg*/
            new Item(383, 37, 1){ NetworkId=658 }, /*minecraft:spawn_egg*/
            new Item(383, 35, 1){ NetworkId=659 }, /*minecraft:spawn_egg*/
            new Item(383, 32, 1){ NetworkId=660 }, /*minecraft:spawn_egg*/
            new Item(383, 36, 1){ NetworkId=661 }, /*minecraft:spawn_egg*/
            new Item(383, 47, 1){ NetworkId=662 }, /*minecraft:spawn_egg*/
            new Item(383, 110, 1){ NetworkId=663 }, /*minecraft:spawn_egg*/
            new Item(383, 17, 1){ NetworkId=664 }, /*minecraft:spawn_egg*/
            new Item(502, 0, 1){ NetworkId=665 }, /*minecraft:music_disc_blocks*/
            new Item(383, 40, 1){ NetworkId=666 }, /*minecraft:spawn_egg*/
            new Item(383, 45, 1){ NetworkId=667 }, /*minecraft:spawn_egg*/
            new Item(383, 49, 1){ NetworkId=668 }, /*minecraft:spawn_egg*/
            new Item(383, 50, 1){ NetworkId=669 }, /*minecraft:spawn_egg*/
            new Item(383, 55, 1){ NetworkId=670 }, /*minecraft:spawn_egg*/
            new Item(383, 42, 1){ NetworkId=671 }, /*minecraft:spawn_egg*/
            new Item(383, 125, 1){ NetworkId=672 }, /*minecraft:spawn_egg*/
            new Item(383, 124, 1){ NetworkId=673 }, /*minecraft:spawn_egg*/
            new Item(383, 123, 1){ NetworkId=674 }, /*minecraft:spawn_egg*/
            new Item(383, 126, 1){ NetworkId=675 }, /*minecraft:spawn_egg*/
            new Item(383, 127, 1){ NetworkId=676 }, /*minecraft:spawn_egg*/
            new Item(383, 128, 1){ NetworkId=677 }, /*minecraft:spawn_egg*/
            new Item(500, 0, 1){ NetworkId=678 }, /*minecraft:music_disc_13*/
            new Item(383, 41, 1){ NetworkId=679 }, /*minecraft:spawn_egg*/
            new Item(383, 43, 1){ NetworkId=680 }, /*minecraft:spawn_egg*/
            new Item(383, 54, 1){ NetworkId=681 }, /*minecraft:spawn_egg*/
            new Item(383, 57, 1){ NetworkId=682 }, /*minecraft:spawn_egg*/
            new Item(383, 104, 1){ NetworkId=683 }, /*minecraft:spawn_egg*/
            new Item(383, 105, 1){ NetworkId=684 }, /*minecraft:spawn_egg*/
            new Item(383, 115, 1){ NetworkId=685 }, /*minecraft:spawn_egg*/
            new Item(383, 118, 1){ NetworkId=686 }, /*minecraft:spawn_egg*/
            new Item(383, 116, 1){ NetworkId=687 }, /*minecraft:spawn_egg*/
            new Item(383, 58, 1){ NetworkId=688 }, /*minecraft:spawn_egg*/
            new Item(383, 114, 1){ NetworkId=689 }, /*minecraft:spawn_egg*/
            new Item(383, 59, 1){ NetworkId=690 }, /*minecraft:spawn_egg*/
            new Item(49, 0, 1){ NetworkId=691 }, /*minecraft:obsidian*/
            new Item(-289, 0, 1){ NetworkId=692 }, /*minecraft:crying_obsidian*/
            new Item(7, 0, 1){ NetworkId=693 }, /*minecraft:bedrock*/
            new Item(88, 0, 1){ NetworkId=694 }, /*minecraft:soul_sand*/
            new Item(87, 0, 1){ NetworkId=695 }, /*minecraft:netherrack*/
            new Item(213, 0, 1){ NetworkId=696 }, /*minecraft:magma*/
            new Item(372, 0, 1){ NetworkId=697 }, /*minecraft:nether_wart*/
            new Item(121, 0, 1){ NetworkId=698 }, /*minecraft:end_stone*/
            new Item(200, 0, 1){ NetworkId=699 }, /*minecraft:chorus_flower*/
            new Item(240, 0, 1){ NetworkId=700 }, /*minecraft:chorus_plant*/
            new Item(432, 0, 1){ NetworkId=701 }, /*minecraft:chorus_fruit*/
            new Item(433, 0, 1){ NetworkId=702 }, /*minecraft:popped_chorus_fruit*/
            new Item(19, 0, 1){ NetworkId=703 }, /*minecraft:sponge*/
            new Item(19, 0, 1){ NetworkId=704 }, /*minecraft:sponge*/
            new Item(-132, 0, 1){ NetworkId=705 }, /*minecraft:coral_block*/
            new Item(-132, 0, 1){ NetworkId=706 }, /*minecraft:coral_block*/
            new Item(-132, 0, 1){ NetworkId=707 }, /*minecraft:coral_block*/
            new Item(-132, 0, 1){ NetworkId=708 }, /*minecraft:coral_block*/
            new Item(-132, 0, 1){ NetworkId=709 }, /*minecraft:coral_block*/
            new Item(-132, 0, 1){ NetworkId=710 }, /*minecraft:coral_block*/
            new Item(-132, 0, 1){ NetworkId=711 }, /*minecraft:coral_block*/
            new Item(-132, 0, 1){ NetworkId=712 }, /*minecraft:coral_block*/
            new Item(-132, 0, 1){ NetworkId=713 }, /*minecraft:coral_block*/
            new Item(-132, 0, 1){ NetworkId=714 }, /*minecraft:coral_block*/
            new Item(298, 0, 1){ NetworkId=715 }, /*minecraft:leather_helmet*/
            new Item(302, 0, 1){ NetworkId=716 }, /*minecraft:chainmail_helmet*/
            new Item(306, 0, 1){ NetworkId=717 }, /*minecraft:iron_helmet*/
            new Item(314, 0, 1){ NetworkId=718 }, /*minecraft:golden_helmet*/
            new Item(310, 0, 1){ NetworkId=719 }, /*minecraft:diamond_helmet*/
            new Item(748, 0, 1){ NetworkId=720 }, /*minecraft:netherite_helmet*/
            new Item(299, 0, 1){ NetworkId=721 }, /*minecraft:leather_chestplate*/
            new Item(303, 0, 1){ NetworkId=722 }, /*minecraft:chainmail_chestplate*/
            new Item(307, 0, 1){ NetworkId=723 }, /*minecraft:iron_chestplate*/
            new Item(315, 0, 1){ NetworkId=724 }, /*minecraft:golden_chestplate*/
            new Item(311, 0, 1){ NetworkId=725 }, /*minecraft:diamond_chestplate*/
            new Item(749, 0, 1){ NetworkId=726 }, /*minecraft:netherite_chestplate*/
            new Item(300, 0, 1){ NetworkId=727 }, /*minecraft:leather_leggings*/
            new Item(304, 0, 1){ NetworkId=728 }, /*minecraft:chainmail_leggings*/
            new Item(308, 0, 1){ NetworkId=729 }, /*minecraft:iron_leggings*/
            new Item(316, 0, 1){ NetworkId=730 }, /*minecraft:golden_leggings*/
            new Item(312, 0, 1){ NetworkId=731 }, /*minecraft:diamond_leggings*/
            new Item(750, 0, 1){ NetworkId=732 }, /*minecraft:netherite_leggings*/
            new Item(301, 0, 1){ NetworkId=733 }, /*minecraft:leather_boots*/
            new Item(305, 0, 1){ NetworkId=734 }, /*minecraft:chainmail_boots*/
            new Item(309, 0, 1){ NetworkId=735 }, /*minecraft:iron_boots*/
            new Item(317, 0, 1){ NetworkId=736 }, /*minecraft:golden_boots*/
            new Item(313, 0, 1){ NetworkId=737 }, /*minecraft:diamond_boots*/
            new Item(751, 0, 1){ NetworkId=738 }, /*minecraft:netherite_boots*/
            new Item(268, 0, 1){ NetworkId=739 }, /*minecraft:wooden_sword*/
            new Item(272, 0, 1){ NetworkId=740 }, /*minecraft:stone_sword*/
            new Item(267, 0, 1){ NetworkId=741 }, /*minecraft:iron_sword*/
            new Item(283, 0, 1){ NetworkId=742 }, /*minecraft:golden_sword*/
            new Item(276, 0, 1){ NetworkId=743 }, /*minecraft:diamond_sword*/
            new Item(743, 0, 1){ NetworkId=744 }, /*minecraft:netherite_sword*/
            new Item(271, 0, 1){ NetworkId=745 }, /*minecraft:wooden_axe*/
            new Item(275, 0, 1){ NetworkId=746 }, /*minecraft:stone_axe*/
            new Item(258, 0, 1){ NetworkId=747 }, /*minecraft:iron_axe*/
            new Item(286, 0, 1){ NetworkId=748 }, /*minecraft:golden_axe*/
            new Item(279, 0, 1){ NetworkId=749 }, /*minecraft:diamond_axe*/
            new Item(746, 0, 1){ NetworkId=750 }, /*minecraft:netherite_axe*/
            new Item(270, 0, 1){ NetworkId=751 }, /*minecraft:wooden_pickaxe*/
            new Item(274, 0, 1){ NetworkId=752 }, /*minecraft:stone_pickaxe*/
            new Item(257, 0, 1){ NetworkId=753 }, /*minecraft:iron_pickaxe*/
            new Item(285, 0, 1){ NetworkId=754 }, /*minecraft:golden_pickaxe*/
            new Item(278, 0, 1){ NetworkId=755 }, /*minecraft:diamond_pickaxe*/
            new Item(745, 0, 1){ NetworkId=756 }, /*minecraft:netherite_pickaxe*/
            new Item(269, 0, 1){ NetworkId=757 }, /*minecraft:wooden_shovel*/
            new Item(273, 0, 1){ NetworkId=758 }, /*minecraft:stone_shovel*/
            new Item(256, 0, 1){ NetworkId=759 }, /*minecraft:iron_shovel*/
            new Item(284, 0, 1){ NetworkId=760 }, /*minecraft:golden_shovel*/
            new Item(277, 0, 1){ NetworkId=761 }, /*minecraft:diamond_shovel*/
            new Item(744, 0, 1){ NetworkId=762 }, /*minecraft:netherite_shovel*/
            new Item(290, 0, 1){ NetworkId=763 }, /*minecraft:wooden_hoe*/
            new Item(291, 0, 1){ NetworkId=764 }, /*minecraft:stone_hoe*/
            new Item(292, 0, 1){ NetworkId=765 }, /*minecraft:iron_hoe*/
            new Item(294, 0, 1){ NetworkId=766 }, /*minecraft:golden_hoe*/
            new Item(293, 0, 1){ NetworkId=767 }, /*minecraft:diamond_hoe*/
            new Item(747, 0, 1){ NetworkId=768 }, /*minecraft:netherite_hoe*/
            new Item(261, 0, 1){ NetworkId=769 }, /*minecraft:bow*/
            new Item(471, 0, 1){ NetworkId=770 }, /*minecraft:crossbow*/
            new Item(262, 0, 1){ NetworkId=771 }, /*minecraft:arrow*/
            new Item(262, 6, 1){ NetworkId=772 }, /*minecraft:arrow*/
            new Item(262, 7, 1){ NetworkId=773 }, /*minecraft:arrow*/
            new Item(262, 8, 1){ NetworkId=774 }, /*minecraft:arrow*/
            new Item(262, 9, 1){ NetworkId=775 }, /*minecraft:arrow*/
            new Item(262, 10, 1){ NetworkId=776 }, /*minecraft:arrow*/
            new Item(262, 11, 1){ NetworkId=777 }, /*minecraft:arrow*/
            new Item(262, 12, 1){ NetworkId=778 }, /*minecraft:arrow*/
            new Item(262, 13, 1){ NetworkId=779 }, /*minecraft:arrow*/
            new Item(262, 14, 1){ NetworkId=780 }, /*minecraft:arrow*/
            new Item(262, 15, 1){ NetworkId=781 }, /*minecraft:arrow*/
            new Item(262, 16, 1){ NetworkId=782 }, /*minecraft:arrow*/
            new Item(262, 17, 1){ NetworkId=783 }, /*minecraft:arrow*/
            new Item(262, 18, 1){ NetworkId=784 }, /*minecraft:arrow*/
            new Item(262, 19, 1){ NetworkId=785 }, /*minecraft:arrow*/
            new Item(262, 20, 1){ NetworkId=786 }, /*minecraft:arrow*/
            new Item(262, 21, 1){ NetworkId=787 }, /*minecraft:arrow*/
            new Item(262, 22, 1){ NetworkId=788 }, /*minecraft:arrow*/
            new Item(262, 23, 1){ NetworkId=789 }, /*minecraft:arrow*/
            new Item(262, 24, 1){ NetworkId=790 }, /*minecraft:arrow*/
            new Item(262, 25, 1){ NetworkId=791 }, /*minecraft:arrow*/
            new Item(262, 26, 1){ NetworkId=792 }, /*minecraft:arrow*/
            new Item(262, 27, 1){ NetworkId=793 }, /*minecraft:arrow*/
            new Item(262, 28, 1){ NetworkId=794 }, /*minecraft:arrow*/
            new Item(262, 29, 1){ NetworkId=795 }, /*minecraft:arrow*/
            new Item(262, 30, 1){ NetworkId=796 }, /*minecraft:arrow*/
            new Item(262, 31, 1){ NetworkId=797 }, /*minecraft:arrow*/
            new Item(262, 32, 1){ NetworkId=798 }, /*minecraft:arrow*/
            new Item(262, 33, 1){ NetworkId=799 }, /*minecraft:arrow*/
            new Item(262, 34, 1){ NetworkId=800 }, /*minecraft:arrow*/
            new Item(262, 35, 1){ NetworkId=801 }, /*minecraft:arrow*/
            new Item(262, 36, 1){ NetworkId=802 }, /*minecraft:arrow*/
            new Item(262, 37, 1){ NetworkId=803 }, /*minecraft:arrow*/
            new Item(262, 38, 1){ NetworkId=804 }, /*minecraft:arrow*/
            new Item(262, 39, 1){ NetworkId=805 }, /*minecraft:arrow*/
            new Item(262, 40, 1){ NetworkId=806 }, /*minecraft:arrow*/
            new Item(262, 41, 1){ NetworkId=807 }, /*minecraft:arrow*/
            new Item(262, 42, 1){ NetworkId=808 }, /*minecraft:arrow*/
            new Item(262, 43, 1){ NetworkId=809 }, /*minecraft:arrow*/
            new Item(513, 0, 1){ NetworkId=810 }, /*minecraft:shield*/
            new Item(366, 0, 1){ NetworkId=811 }, /*minecraft:cooked_chicken*/
            new Item(320, 0, 1){ NetworkId=812 }, /*minecraft:cooked_porkchop*/
            new Item(364, 0, 1){ NetworkId=813 }, /*minecraft:cooked_beef*/
            new Item(424, 0, 1){ NetworkId=814 }, /*minecraft:cooked_mutton*/
            new Item(412, 0, 1){ NetworkId=815 }, /*minecraft:cooked_rabbit*/
            new Item(350, 0, 1){ NetworkId=816 }, /*minecraft:cooked_cod*/
            new Item(463, 0, 1){ NetworkId=817 }, /*minecraft:cooked_salmon*/
            new Item(297, 0, 1){ NetworkId=818 }, /*minecraft:bread*/
            new Item(282, 0, 1){ NetworkId=819 }, /*minecraft:mushroom_stew*/
            new Item(459, 0, 1){ NetworkId=820 }, /*minecraft:beetroot_soup*/
            new Item(413, 0, 1){ NetworkId=821 }, /*minecraft:rabbit_stew*/
            new Item(393, 0, 1){ NetworkId=822 }, /*minecraft:baked_potato*/
            new Item(357, 0, 1){ NetworkId=823 }, /*minecraft:cookie*/
            new Item(400, 0, 1){ NetworkId=824 }, /*minecraft:pumpkin_pie*/
            new Item(354, 0, 1){ NetworkId=825 }, /*minecraft:cake*/
            new Item(464, 0, 1){ NetworkId=826 }, /*minecraft:dried_kelp*/
            new Item(346, 0, 1){ NetworkId=827 }, /*minecraft:fishing_rod*/
            new Item(398, 0, 1){ NetworkId=828 }, /*minecraft:carrot_on_a_stick*/
            new Item(757, 0, 1){ NetworkId=829 }, /*minecraft:warped_fungus_on_a_stick*/
            new Item(332, 0, 1){ NetworkId=830 }, /*minecraft:snowball*/
            new Item(359, 0, 1){ NetworkId=831 }, /*minecraft:shears*/
            new Item(259, 0, 1){ NetworkId=832 }, /*minecraft:flint_and_steel*/
            new Item(420, 0, 1){ NetworkId=833 }, /*minecraft:lead*/
            new Item(347, 0, 1){ NetworkId=834 }, /*minecraft:clock*/
            new Item(345, 0, 1){ NetworkId=835 }, /*minecraft:compass*/
            new Item(395, 0, 1){ NetworkId=836 }, /*minecraft:empty_map*/
            new Item(395, 2, 1){ NetworkId=837 }, /*minecraft:empty_map*/
            new Item(329, 0, 1){ NetworkId=838 }, /*minecraft:saddle*/
            new Item(416, 0, 1){ NetworkId=839 }, /*minecraft:leather_horse_armor*/
            new Item(417, 0, 1){ NetworkId=840 }, /*minecraft:iron_horse_armor*/
            new Item(417, 0, 1){ NetworkId=841 }, /*minecraft:golden_horse_armor*/
            new Item(419, 0, 1){ NetworkId=842 }, /*minecraft:diamond_horse_armor*/
            new Item(455, 0, 1){ NetworkId=843 }, /*minecraft:trident*/
            new Item(469, 0, 1){ NetworkId=844 }, /*minecraft:turtle_helmet*/
            new Item(444, 0, 1){ NetworkId=845 }, /*minecraft:elytra*/
            new Item(450, 0, 1){ NetworkId=846 }, /*minecraft:totem_of_undying*/
            new Item(374, 0, 1){ NetworkId=847 }, /*minecraft:glass_bottle*/
            new Item(384, 0, 1){ NetworkId=848 }, /*minecraft:experience_bottle*/
            new Item(373, 0, 1){ NetworkId=849 }, /*minecraft:potion*/
            new Item(373, 1, 1){ NetworkId=850 }, /*minecraft:potion*/
            new Item(373, 2, 1){ NetworkId=851 }, /*minecraft:potion*/
            new Item(373, 3, 1){ NetworkId=852 }, /*minecraft:potion*/
            new Item(373, 4, 1){ NetworkId=853 }, /*minecraft:potion*/
            new Item(373, 5, 1){ NetworkId=854 }, /*minecraft:potion*/
            new Item(373, 6, 1){ NetworkId=855 }, /*minecraft:potion*/
            new Item(373, 7, 1){ NetworkId=856 }, /*minecraft:potion*/
            new Item(373, 8, 1){ NetworkId=857 }, /*minecraft:potion*/
            new Item(373, 9, 1){ NetworkId=858 }, /*minecraft:potion*/
            new Item(373, 10, 1){ NetworkId=859 }, /*minecraft:potion*/
            new Item(373, 11, 1){ NetworkId=860 }, /*minecraft:potion*/
            new Item(373, 12, 1){ NetworkId=861 }, /*minecraft:potion*/
            new Item(373, 13, 1){ NetworkId=862 }, /*minecraft:potion*/
            new Item(373, 14, 1){ NetworkId=863 }, /*minecraft:potion*/
            new Item(373, 15, 1){ NetworkId=864 }, /*minecraft:potion*/
            new Item(373, 16, 1){ NetworkId=865 }, /*minecraft:potion*/
            new Item(373, 17, 1){ NetworkId=866 }, /*minecraft:potion*/
            new Item(373, 18, 1){ NetworkId=867 }, /*minecraft:potion*/
            new Item(373, 19, 1){ NetworkId=868 }, /*minecraft:potion*/
            new Item(373, 20, 1){ NetworkId=869 }, /*minecraft:potion*/
            new Item(373, 21, 1){ NetworkId=870 }, /*minecraft:potion*/
            new Item(373, 22, 1){ NetworkId=871 }, /*minecraft:potion*/
            new Item(373, 23, 1){ NetworkId=872 }, /*minecraft:potion*/
            new Item(373, 24, 1){ NetworkId=873 }, /*minecraft:potion*/
            new Item(373, 25, 1){ NetworkId=874 }, /*minecraft:potion*/
            new Item(373, 26, 1){ NetworkId=875 }, /*minecraft:potion*/
            new Item(373, 27, 1){ NetworkId=876 }, /*minecraft:potion*/
            new Item(373, 28, 1){ NetworkId=877 }, /*minecraft:potion*/
            new Item(373, 29, 1){ NetworkId=878 }, /*minecraft:potion*/
            new Item(373, 30, 1){ NetworkId=879 }, /*minecraft:potion*/
            new Item(373, 31, 1){ NetworkId=880 }, /*minecraft:potion*/
            new Item(373, 32, 1){ NetworkId=881 }, /*minecraft:potion*/
            new Item(373, 33, 1){ NetworkId=882 }, /*minecraft:potion*/
            new Item(373, 34, 1){ NetworkId=883 }, /*minecraft:potion*/
            new Item(373, 35, 1){ NetworkId=884 }, /*minecraft:potion*/
            new Item(373, 36, 1){ NetworkId=885 }, /*minecraft:potion*/
            new Item(373, 37, 1){ NetworkId=886 }, /*minecraft:potion*/
            new Item(373, 38, 1){ NetworkId=887 }, /*minecraft:potion*/
            new Item(373, 39, 1){ NetworkId=888 }, /*minecraft:potion*/
            new Item(373, 40, 1){ NetworkId=889 }, /*minecraft:potion*/
            new Item(373, 41, 1){ NetworkId=890 }, /*minecraft:potion*/
            new Item(373, 42, 1){ NetworkId=891 }, /*minecraft:potion*/
            new Item(438, 0, 1){ NetworkId=892 }, /*minecraft:splash_potion*/
            new Item(438, 1, 1){ NetworkId=893 }, /*minecraft:splash_potion*/
            new Item(438, 2, 1){ NetworkId=894 }, /*minecraft:splash_potion*/
            new Item(438, 3, 1){ NetworkId=895 }, /*minecraft:splash_potion*/
            new Item(438, 4, 1){ NetworkId=896 }, /*minecraft:splash_potion*/
            new Item(438, 5, 1){ NetworkId=897 }, /*minecraft:splash_potion*/
            new Item(438, 6, 1){ NetworkId=898 }, /*minecraft:splash_potion*/
            new Item(438, 7, 1){ NetworkId=899 }, /*minecraft:splash_potion*/
            new Item(438, 8, 1){ NetworkId=900 }, /*minecraft:splash_potion*/
            new Item(438, 9, 1){ NetworkId=901 }, /*minecraft:splash_potion*/
            new Item(438, 10, 1){ NetworkId=902 }, /*minecraft:splash_potion*/
            new Item(438, 11, 1){ NetworkId=903 }, /*minecraft:splash_potion*/
            new Item(438, 12, 1){ NetworkId=904 }, /*minecraft:splash_potion*/
            new Item(438, 13, 1){ NetworkId=905 }, /*minecraft:splash_potion*/
            new Item(438, 14, 1){ NetworkId=906 }, /*minecraft:splash_potion*/
            new Item(438, 15, 1){ NetworkId=907 }, /*minecraft:splash_potion*/
            new Item(438, 16, 1){ NetworkId=908 }, /*minecraft:splash_potion*/
            new Item(438, 17, 1){ NetworkId=909 }, /*minecraft:splash_potion*/
            new Item(438, 18, 1){ NetworkId=910 }, /*minecraft:splash_potion*/
            new Item(438, 19, 1){ NetworkId=911 }, /*minecraft:splash_potion*/
            new Item(438, 20, 1){ NetworkId=912 }, /*minecraft:splash_potion*/
            new Item(438, 21, 1){ NetworkId=913 }, /*minecraft:splash_potion*/
            new Item(438, 22, 1){ NetworkId=914 }, /*minecraft:splash_potion*/
            new Item(438, 23, 1){ NetworkId=915 }, /*minecraft:splash_potion*/
            new Item(438, 24, 1){ NetworkId=916 }, /*minecraft:splash_potion*/
            new Item(438, 25, 1){ NetworkId=917 }, /*minecraft:splash_potion*/
            new Item(438, 26, 1){ NetworkId=918 }, /*minecraft:splash_potion*/
            new Item(438, 27, 1){ NetworkId=919 }, /*minecraft:splash_potion*/
            new Item(438, 28, 1){ NetworkId=920 }, /*minecraft:splash_potion*/
            new Item(438, 29, 1){ NetworkId=921 }, /*minecraft:splash_potion*/
            new Item(438, 30, 1){ NetworkId=922 }, /*minecraft:splash_potion*/
            new Item(438, 31, 1){ NetworkId=923 }, /*minecraft:splash_potion*/
            new Item(438, 32, 1){ NetworkId=924 }, /*minecraft:splash_potion*/
            new Item(438, 33, 1){ NetworkId=925 }, /*minecraft:splash_potion*/
            new Item(438, 34, 1){ NetworkId=926 }, /*minecraft:splash_potion*/
            new Item(438, 35, 1){ NetworkId=927 }, /*minecraft:splash_potion*/
            new Item(438, 36, 1){ NetworkId=928 }, /*minecraft:splash_potion*/
            new Item(438, 37, 1){ NetworkId=929 }, /*minecraft:splash_potion*/
            new Item(438, 38, 1){ NetworkId=930 }, /*minecraft:splash_potion*/
            new Item(438, 39, 1){ NetworkId=931 }, /*minecraft:splash_potion*/
            new Item(438, 40, 1){ NetworkId=932 }, /*minecraft:splash_potion*/
            new Item(438, 41, 1){ NetworkId=933 }, /*minecraft:splash_potion*/
            new Item(438, 42, 1){ NetworkId=934 }, /*minecraft:splash_potion*/
            new Item(441, 0, 1){ NetworkId=935 }, /*minecraft:lingering_potion*/
            new Item(441, 1, 1){ NetworkId=936 }, /*minecraft:lingering_potion*/
            new Item(441, 2, 1){ NetworkId=937 }, /*minecraft:lingering_potion*/
            new Item(441, 3, 1){ NetworkId=938 }, /*minecraft:lingering_potion*/
            new Item(441, 4, 1){ NetworkId=939 }, /*minecraft:lingering_potion*/
            new Item(441, 5, 1){ NetworkId=940 }, /*minecraft:lingering_potion*/
            new Item(441, 6, 1){ NetworkId=941 }, /*minecraft:lingering_potion*/
            new Item(441, 7, 1){ NetworkId=942 }, /*minecraft:lingering_potion*/
            new Item(441, 8, 1){ NetworkId=943 }, /*minecraft:lingering_potion*/
            new Item(441, 9, 1){ NetworkId=944 }, /*minecraft:lingering_potion*/
            new Item(441, 10, 1){ NetworkId=945 }, /*minecraft:lingering_potion*/
            new Item(441, 11, 1){ NetworkId=946 }, /*minecraft:lingering_potion*/
            new Item(441, 12, 1){ NetworkId=947 }, /*minecraft:lingering_potion*/
            new Item(441, 13, 1){ NetworkId=948 }, /*minecraft:lingering_potion*/
            new Item(441, 14, 1){ NetworkId=949 }, /*minecraft:lingering_potion*/
            new Item(441, 15, 1){ NetworkId=950 }, /*minecraft:lingering_potion*/
            new Item(441, 16, 1){ NetworkId=951 }, /*minecraft:lingering_potion*/
            new Item(441, 17, 1){ NetworkId=952 }, /*minecraft:lingering_potion*/
            new Item(441, 18, 1){ NetworkId=953 }, /*minecraft:lingering_potion*/
            new Item(441, 19, 1){ NetworkId=954 }, /*minecraft:lingering_potion*/
            new Item(441, 20, 1){ NetworkId=955 }, /*minecraft:lingering_potion*/
            new Item(441, 21, 1){ NetworkId=956 }, /*minecraft:lingering_potion*/
            new Item(441, 22, 1){ NetworkId=957 }, /*minecraft:lingering_potion*/
            new Item(441, 23, 1){ NetworkId=958 }, /*minecraft:lingering_potion*/
            new Item(441, 24, 1){ NetworkId=959 }, /*minecraft:lingering_potion*/
            new Item(441, 25, 1){ NetworkId=960 }, /*minecraft:lingering_potion*/
            new Item(441, 26, 1){ NetworkId=961 }, /*minecraft:lingering_potion*/
            new Item(441, 27, 1){ NetworkId=962 }, /*minecraft:lingering_potion*/
            new Item(441, 28, 1){ NetworkId=963 }, /*minecraft:lingering_potion*/
            new Item(441, 29, 1){ NetworkId=964 }, /*minecraft:lingering_potion*/
            new Item(441, 30, 1){ NetworkId=965 }, /*minecraft:lingering_potion*/
            new Item(441, 31, 1){ NetworkId=966 }, /*minecraft:lingering_potion*/
            new Item(441, 32, 1){ NetworkId=967 }, /*minecraft:lingering_potion*/
            new Item(441, 33, 1){ NetworkId=968 }, /*minecraft:lingering_potion*/
            new Item(441, 34, 1){ NetworkId=969 }, /*minecraft:lingering_potion*/
            new Item(441, 35, 1){ NetworkId=970 }, /*minecraft:lingering_potion*/
            new Item(441, 36, 1){ NetworkId=971 }, /*minecraft:lingering_potion*/
            new Item(441, 37, 1){ NetworkId=972 }, /*minecraft:lingering_potion*/
            new Item(441, 38, 1){ NetworkId=973 }, /*minecraft:lingering_potion*/
            new Item(441, 39, 1){ NetworkId=974 }, /*minecraft:lingering_potion*/
            new Item(441, 40, 1){ NetworkId=975 }, /*minecraft:lingering_potion*/
            new Item(441, 41, 1){ NetworkId=976 }, /*minecraft:lingering_potion*/
            new Item(441, 42, 1){ NetworkId=977 }, /*minecraft:lingering_potion*/
            new Item(624, 0, 1){ NetworkId=978 }, /**/
            new Item(280, 0, 1){ NetworkId=979 }, /*minecraft:stick*/
            new Item(355, 0, 1){ NetworkId=980 }, /*minecraft:bed*/
            new Item(355, 8, 1){ NetworkId=981 }, /*minecraft:bed*/
            new Item(355, 7, 1){ NetworkId=982 }, /*minecraft:bed*/
            new Item(355, 15, 1){ NetworkId=983 }, /*minecraft:bed*/
            new Item(355, 12, 1){ NetworkId=984 }, /*minecraft:bed*/
            new Item(355, 14, 1){ NetworkId=985 }, /*minecraft:bed*/
            new Item(355, 1, 1){ NetworkId=986 }, /*minecraft:bed*/
            new Item(355, 4, 1){ NetworkId=987 }, /*minecraft:bed*/
            new Item(355, 5, 1){ NetworkId=988 }, /*minecraft:bed*/
            new Item(355, 13, 1){ NetworkId=989 }, /*minecraft:bed*/
            new Item(355, 9, 1){ NetworkId=990 }, /*minecraft:bed*/
            new Item(355, 3, 1){ NetworkId=991 }, /*minecraft:bed*/
            new Item(355, 11, 1){ NetworkId=992 }, /*minecraft:bed*/
            new Item(355, 10, 1){ NetworkId=993 }, /*minecraft:bed*/
            new Item(355, 2, 1){ NetworkId=994 }, /*minecraft:bed*/
            new Item(355, 6, 1){ NetworkId=995 }, /*minecraft:bed*/
            new Item(50, 0, 1){ NetworkId=996 }, /*minecraft:torch*/
            new Item(-268, 0, 1){ NetworkId=997 }, /*minecraft:soul_torch*/
            new Item(-156, 0, 1){ NetworkId=998 }, /*minecraft:sea_pickle*/
            new Item(-208, 0, 1){ NetworkId=999 }, /*minecraft:lantern*/
            new Item(-269, 0, 1){ NetworkId=1000 }, /*minecraft:soul_lantern*/
            new Item(-412, 0, 1){ NetworkId=1001 }, /**/
            new Item(-413, 0, 1){ NetworkId=1002 }, /**/
            new Item(-414, 0, 1){ NetworkId=1003 }, /**/
            new Item(-415, 0, 1){ NetworkId=1004 }, /**/
            new Item(-416, 0, 1){ NetworkId=1005 }, /**/
            new Item(-417, 0, 1){ NetworkId=1006 }, /**/
            new Item(-418, 0, 1){ NetworkId=1007 }, /**/
            new Item(-419, 0, 1){ NetworkId=1008 }, /**/
            new Item(-420, 0, 1){ NetworkId=1009 }, /**/
            new Item(-421, 0, 1){ NetworkId=1010 }, /**/
            new Item(-422, 0, 1){ NetworkId=1011 }, /**/
            new Item(-423, 0, 1){ NetworkId=1012 }, /**/
            new Item(-424, 0, 1){ NetworkId=1013 }, /**/
            new Item(-425, 0, 1){ NetworkId=1014 }, /**/
            new Item(-426, 0, 1){ NetworkId=1015 }, /**/
            new Item(-427, 0, 1){ NetworkId=1016 }, /**/
            new Item(-428, 0, 1){ NetworkId=1017 }, /**/
            new Item(58, 0, 1){ NetworkId=1018 }, /*minecraft:crafting_table*/
            new Item(-200, 0, 1){ NetworkId=1019 }, /*minecraft:cartography_table*/
            new Item(-201, 0, 1){ NetworkId=1020 }, /*minecraft:fletching_table*/
            new Item(-202, 0, 1){ NetworkId=1021 }, /*minecraft:smithing_table*/
            new Item(-219, 0, 1){ NetworkId=1022 }, /*minecraft:beehive*/
            new Item(720, 0, 1){ NetworkId=1023 }, /*minecraft:campfire*/
            new Item(801, 0, 1){ NetworkId=1024 }, /*minecraft:soul_campfire*/
            new Item(61, 0, 1){ NetworkId=1025 }, /*minecraft:furnace*/
            new Item(-196, 0, 1){ NetworkId=1026 }, /*minecraft:blast_furnace*/
            new Item(-198, 0, 1){ NetworkId=1027 }, /*minecraft:smoker*/
            new Item(-272, 0, 1){ NetworkId=1028 }, /*minecraft:respawn_anchor*/
            new Item(379, 0, 1){ NetworkId=1029 }, /*minecraft:brewing_stand*/
            new Item(145, 0, 1){ NetworkId=1030 }, /*minecraft:anvil*/
            new Item(145, 0, 1){ NetworkId=1031 }, /*minecraft:anvil*/
            new Item(145, 0, 1){ NetworkId=1032 }, /*minecraft:anvil*/
            new Item(-195, 0, 1){ NetworkId=1033 }, /*minecraft:grindstone*/
            new Item(116, 0, 1){ NetworkId=1034 }, /*minecraft:enchanting_table*/
            new Item(47, 0, 1){ NetworkId=1035 }, /*minecraft:bookshelf*/
            new Item(-194, 0, 1){ NetworkId=1036 }, /*minecraft:lectern*/
            new Item(380, 0, 1){ NetworkId=1037 }, /*minecraft:cauldron*/
            new Item(-213, 0, 1){ NetworkId=1038 }, /*minecraft:composter*/
            new Item(54, 0, 1){ NetworkId=1039 }, /*minecraft:chest*/
            new Item(146, 0, 1){ NetworkId=1040 }, /*minecraft:trapped_chest*/
            new Item(130, 0, 1){ NetworkId=1041 }, /*minecraft:ender_chest*/
            new Item(-203, 0, 1){ NetworkId=1042 }, /*minecraft:barrel*/
            new Item(205, 0, 1){ NetworkId=1043 }, /*minecraft:undyed_shulker_box*/
            new Item(218, 0, 1){ NetworkId=1044 }, /*minecraft:shulker_box*/
            new Item(218, 0, 1){ NetworkId=1045 }, /*minecraft:shulker_box*/
            new Item(218, 0, 1){ NetworkId=1046 }, /*minecraft:shulker_box*/
            new Item(218, 0, 1){ NetworkId=1047 }, /*minecraft:shulker_box*/
            new Item(218, 0, 1){ NetworkId=1048 }, /*minecraft:shulker_box*/
            new Item(218, 0, 1){ NetworkId=1049 }, /*minecraft:shulker_box*/
            new Item(218, 0, 1){ NetworkId=1050 }, /*minecraft:shulker_box*/
            new Item(218, 0, 1){ NetworkId=1051 }, /*minecraft:shulker_box*/
            new Item(218, 0, 1){ NetworkId=1052 }, /*minecraft:shulker_box*/
            new Item(218, 0, 1){ NetworkId=1053 }, /*minecraft:shulker_box*/
            new Item(218, 0, 1){ NetworkId=1054 }, /*minecraft:shulker_box*/
            new Item(218, 0, 1){ NetworkId=1055 }, /*minecraft:shulker_box*/
            new Item(218, 0, 1){ NetworkId=1056 }, /*minecraft:shulker_box*/
            new Item(218, 0, 1){ NetworkId=1057 }, /*minecraft:shulker_box*/
            new Item(218, 0, 1){ NetworkId=1058 }, /*minecraft:shulker_box*/
            new Item(218, 0, 1){ NetworkId=1059 }, /*minecraft:shulker_box*/
            new Item(425, 0, 1){ NetworkId=1060 }, /*minecraft:armor_stand*/
            new Item(25, 0, 1){ NetworkId=1061 }, /*minecraft:noteblock*/
            new Item(84, 0, 1){ NetworkId=1062 }, /*minecraft:jukebox*/
            new Item(500, 0, 1){ NetworkId=1063 }, /*minecraft:music_disc_13*/
            new Item(501, 0, 1){ NetworkId=1064 }, /*minecraft:music_disc_cat*/
            new Item(502, 0, 1){ NetworkId=1065 }, /*minecraft:music_disc_blocks*/
            new Item(503, 0, 1){ NetworkId=1066 }, /*minecraft:music_disc_chirp*/
            new Item(504, 0, 1){ NetworkId=1067 }, /*minecraft:music_disc_far*/
            new Item(505, 0, 1){ NetworkId=1068 }, /*minecraft:music_disc_mall*/
            new Item(506, 0, 1){ NetworkId=1069 }, /*minecraft:music_disc_mellohi*/
            new Item(507, 0, 1){ NetworkId=1070 }, /*minecraft:music_disc_stal*/
            new Item(508, 0, 1){ NetworkId=1071 }, /*minecraft:music_disc_strad*/
            new Item(509, 0, 1){ NetworkId=1072 }, /*minecraft:music_disc_ward*/
            new Item(510, 0, 1){ NetworkId=1073 }, /*minecraft:music_disc_11*/
            new Item(511, 0, 1){ NetworkId=1074 }, /*minecraft:music_disc_wait*/
            new Item(759, 0, 1){ NetworkId=1075 }, /*minecraft:music_disc_pigstep*/
            new Item(348, 0, 1){ NetworkId=1076 }, /*minecraft:glowstone_dust*/
            new Item(89, 0, 1){ NetworkId=1077 }, /*minecraft:glowstone*/
            new Item(123, 0, 1){ NetworkId=1078 }, /*minecraft:redstone_lamp*/
            new Item(169, 0, 1){ NetworkId=1079 }, /*minecraft:seaLantern*/
            new Item(323, 0, 1){ NetworkId=1080 }, /*minecraft:oak_sign*/
            new Item(472, 0, 1){ NetworkId=1081 }, /*minecraft:spruce_sign*/
            new Item(473, 0, 1){ NetworkId=1082 }, /*minecraft:birch_sign*/
            new Item(474, 0, 1){ NetworkId=1083 }, /*minecraft:jungle_sign*/
            new Item(475, 0, 1){ NetworkId=1084 }, /*minecraft:acacia_sign*/
            new Item(476, 0, 1){ NetworkId=1085 }, /*minecraft:dark_oak_sign*/
            new Item(753, 0, 1){ NetworkId=1086 }, /*minecraft:crimson_sign*/
            new Item(754, 0, 1){ NetworkId=1087 }, /*minecraft:warped_sign*/
            new Item(321, 0, 1){ NetworkId=1088 }, /*minecraft:painting*/
            new Item(389, 0, 1){ NetworkId=1089 }, /*minecraft:frame*/
            new Item(621, 0, 1){ NetworkId=1090 }, /**/
            new Item(737, 0, 1){ NetworkId=1091 }, /*minecraft:honey_bottle*/
            new Item(390, 0, 1){ NetworkId=1092 }, /*minecraft:flower_pot*/
            new Item(281, 0, 1){ NetworkId=1093 }, /*minecraft:bowl*/
            new Item(325, 0, 1){ NetworkId=1094 }, /*minecraft:bucket*/
            new Item(325, 1, 1){ NetworkId=1095 }, /*minecraft:bucket*/
            new Item(325, 8, 1){ NetworkId=1096 }, /*minecraft:bucket*/
            new Item(325, 10, 1){ NetworkId=1097 }, /*minecraft:bucket*/
            new Item(325, 2, 1){ NetworkId=1098 }, /*minecraft:bucket*/
            new Item(325, 3, 1){ NetworkId=1099 }, /*minecraft:bucket*/
            new Item(325, 4, 1){ NetworkId=1100 }, /*minecraft:bucket*/
            new Item(325, 5, 1){ NetworkId=1101 }, /*minecraft:bucket*/
            new Item(368, 0, 1){ NetworkId=1102 }, /*minecraft:ender_pearl*/
            new Item(369, 0, 1){ NetworkId=1103 }, /*minecraft:blaze_rod*/
            new Item(397, 3, 1){ NetworkId=1104 }, /*minecraft:skull*/
            new Item(397, 2, 1){ NetworkId=1105 }, /*minecraft:skull*/
            new Item(397, 4, 1){ NetworkId=1106 }, /*minecraft:skull*/
            new Item(397, 5, 1){ NetworkId=1107 }, /*minecraft:skull*/
            new Item(397, 0, 1){ NetworkId=1108 }, /*minecraft:skull*/
            new Item(397, 1, 1){ NetworkId=1109 }, /*minecraft:skull*/
            new Item(138, 0, 1){ NetworkId=1110 }, /*minecraft:beacon*/
            new Item(-206, 0, 1){ NetworkId=1111 }, /*minecraft:bell*/
            new Item(-157, 0, 1){ NetworkId=1112 }, /*minecraft:conduit*/
            new Item(-197, 0, 1){ NetworkId=1113 }, /*minecraft:stonecutter_block*/
            new Item(120, 0, 1){ NetworkId=1114 }, /*minecraft:end_portal_frame*/
            new Item(263, 0, 1){ NetworkId=1115 }, /*minecraft:coal*/
            new Item(263, 1, 1){ NetworkId=1116 }, /*minecraft:coal*/
            new Item(264, 0, 1){ NetworkId=1117 }, /*minecraft:diamond*/
            new Item(452, 0, 1){ NetworkId=1118 }, /*minecraft:iron_nugget*/
            new Item(505, 0, 1){ NetworkId=1119 }, /*minecraft:music_disc_mall*/
            new Item(506, 0, 1){ NetworkId=1120 }, /*minecraft:music_disc_mellohi*/
            new Item(507, 0, 1){ NetworkId=1121 }, /*minecraft:music_disc_stal*/
            new Item(504, 0, 1){ NetworkId=1122 }, /*minecraft:music_disc_far*/
            new Item(265, 0, 1){ NetworkId=1123 }, /*minecraft:iron_ingot*/
            new Item(752, 0, 1){ NetworkId=1124 }, /*minecraft:netherite_scrap*/
            new Item(742, 0, 1){ NetworkId=1125 }, /*minecraft:netherite_ingot*/
            new Item(371, 0, 1){ NetworkId=1126 }, /*minecraft:gold_nugget*/
            new Item(266, 0, 1){ NetworkId=1127 }, /*minecraft:gold_ingot*/
            new Item(388, 0, 1){ NetworkId=1128 }, /*minecraft:emerald*/
            new Item(406, 0, 1){ NetworkId=1129 }, /*minecraft:quartz*/
            new Item(337, 0, 1){ NetworkId=1130 }, /*minecraft:clay_ball*/
            new Item(336, 0, 1){ NetworkId=1131 }, /*minecraft:brick*/
            new Item(405, 0, 1){ NetworkId=1132 }, /*minecraft:netherbrick*/
            new Item(409, 0, 1){ NetworkId=1133 }, /*minecraft:prismarine_shard*/
            new Item(623, 0, 1){ NetworkId=1134 }, /**/
            new Item(422, 0, 1){ NetworkId=1135 }, /*minecraft:prismarine_crystals*/
            new Item(465, 0, 1){ NetworkId=1136 }, /*minecraft:nautilus_shell*/
            new Item(467, 0, 1){ NetworkId=1137 }, /*minecraft:heart_of_the_sea*/
            new Item(468, 0, 1){ NetworkId=1138 }, /*minecraft:scute*/
            new Item(470, 0, 1){ NetworkId=1139 }, /*minecraft:phantom_membrane*/
            new Item(287, 0, 1){ NetworkId=1140 }, /*minecraft:string*/
            new Item(288, 0, 1){ NetworkId=1141 }, /*minecraft:feather*/
            new Item(318, 0, 1){ NetworkId=1142 }, /*minecraft:flint*/
            new Item(289, 0, 1){ NetworkId=1143 }, /*minecraft:gunpowder*/
            new Item(334, 0, 1){ NetworkId=1144 }, /*minecraft:leather*/
            new Item(415, 0, 1){ NetworkId=1145 }, /*minecraft:rabbit_hide*/
            new Item(414, 0, 1){ NetworkId=1146 }, /*minecraft:rabbit_foot*/
            new Item(385, 0, 1){ NetworkId=1147 }, /*minecraft:fire_charge*/
            new Item(369, 0, 1){ NetworkId=1148 }, /*minecraft:blaze_rod*/
            new Item(377, 0, 1){ NetworkId=1149 }, /*minecraft:blaze_powder*/
            new Item(378, 0, 1){ NetworkId=1150 }, /*minecraft:magma_cream*/
            new Item(376, 0, 1){ NetworkId=1151 }, /*minecraft:fermented_spider_eye*/
            new Item(437, 0, 1){ NetworkId=1152 }, /*minecraft:dragon_breath*/
            new Item(445, 0, 1){ NetworkId=1153 }, /*minecraft:shulker_shell*/
            new Item(370, 0, 1){ NetworkId=1154 }, /*minecraft:ghast_tear*/
            new Item(341, 0, 1){ NetworkId=1155 }, /*minecraft:slime_ball*/
            new Item(368, 0, 1){ NetworkId=1156 }, /*minecraft:ender_pearl*/
            new Item(381, 0, 1){ NetworkId=1157 }, /*minecraft:ender_eye*/
            new Item(399, 0, 1){ NetworkId=1158 }, /*minecraft:nether_star*/
            new Item(208, 0, 1){ NetworkId=1159 }, /*minecraft:end_rod*/
            new Item(-312, 0, 1){ NetworkId=1160 }, /**/
            new Item(426, 0, 1){ NetworkId=1161 }, /*minecraft:end_crystal*/
            new Item(339, 0, 1){ NetworkId=1162 }, /*minecraft:paper*/
            new Item(340, 0, 1){ NetworkId=1163 }, /*minecraft:book*/
            new Item(386, 0, 1){ NetworkId=1164 }, /*minecraft:writable_book*/
            new Item(403, 0, 1){ NetworkId=1165, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 0), new NbtShort("lvl", 1) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1166, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 0), new NbtShort("lvl", 2) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1167, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 0), new NbtShort("lvl", 3) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1168, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 0), new NbtShort("lvl", 4) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1169, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 1), new NbtShort("lvl", 1) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1170, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 1), new NbtShort("lvl", 2) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1171, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 1), new NbtShort("lvl", 3) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1172, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 1), new NbtShort("lvl", 4) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1173, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 2), new NbtShort("lvl", 1) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1174, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 2), new NbtShort("lvl", 2) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1175, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 2), new NbtShort("lvl", 3) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1176, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 2), new NbtShort("lvl", 4) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1177, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 3), new NbtShort("lvl", 1) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1178, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 3), new NbtShort("lvl", 2) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1179, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 3), new NbtShort("lvl", 3) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1180, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 3), new NbtShort("lvl", 4) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1181, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 4), new NbtShort("lvl", 1) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1182, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 4), new NbtShort("lvl", 2) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1183, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 4), new NbtShort("lvl", 3) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1184, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 4), new NbtShort("lvl", 4) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1185, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 5), new NbtShort("lvl", 1) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1186, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 5), new NbtShort("lvl", 2) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1187, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 5), new NbtShort("lvl", 3) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1188, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 6), new NbtShort("lvl", 1) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1189, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 6), new NbtShort("lvl", 2) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1190, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 6), new NbtShort("lvl", 3) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1191, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 7), new NbtShort("lvl", 1) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1192, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 7), new NbtShort("lvl", 2) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1193, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 7), new NbtShort("lvl", 3) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1194, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 8), new NbtShort("lvl", 1) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1195, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 9), new NbtShort("lvl", 1) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1196, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 9), new NbtShort("lvl", 2) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1197, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 9), new NbtShort("lvl", 3) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1198, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 9), new NbtShort("lvl", 4) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1199, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 9), new NbtShort("lvl", 5) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1200, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 10), new NbtShort("lvl", 1) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1201, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 10), new NbtShort("lvl", 2) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1202, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 10), new NbtShort("lvl", 3) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1203, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 10), new NbtShort("lvl", 4) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1204, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 10), new NbtShort("lvl", 5) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1205, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 11), new NbtShort("lvl", 1) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1206, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 11), new NbtShort("lvl", 2) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1207, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 11), new NbtShort("lvl", 3) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1208, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 11), new NbtShort("lvl", 4) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1209, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 11), new NbtShort("lvl", 5) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1210, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 12), new NbtShort("lvl", 1) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1211, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 12), new NbtShort("lvl", 2) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1212, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 13), new NbtShort("lvl", 1) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1213, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 13), new NbtShort("lvl", 2) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1214, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 14), new NbtShort("lvl", 1) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1215, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 14), new NbtShort("lvl", 2) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1216, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 14), new NbtShort("lvl", 3) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1217, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 15), new NbtShort("lvl", 1) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1218, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 15), new NbtShort("lvl", 2) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1219, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 15), new NbtShort("lvl", 3) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1220, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 15), new NbtShort("lvl", 4) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1221, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 15), new NbtShort("lvl", 5) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1222, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 16), new NbtShort("lvl", 1) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1223, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 17), new NbtShort("lvl", 1) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1224, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 17), new NbtShort("lvl", 2) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1225, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 17), new NbtShort("lvl", 3) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1226, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 18), new NbtShort("lvl", 1) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1227, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 18), new NbtShort("lvl", 2) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1228, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 18), new NbtShort("lvl", 3) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1229, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 19), new NbtShort("lvl", 1) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1230, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 19), new NbtShort("lvl", 2) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1231, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 19), new NbtShort("lvl", 3) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1232, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 19), new NbtShort("lvl", 4) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1233, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 19), new NbtShort("lvl", 5) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1234, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 20), new NbtShort("lvl", 1) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1235, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 20), new NbtShort("lvl", 2) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1236, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 21), new NbtShort("lvl", 1) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1237, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 22), new NbtShort("lvl", 1) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1238, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 23), new NbtShort("lvl", 1) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1239, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 23), new NbtShort("lvl", 2) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1240, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 23), new NbtShort("lvl", 3) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1241, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 24), new NbtShort("lvl", 1) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1242, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 24), new NbtShort("lvl", 2) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1243, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 24), new NbtShort("lvl", 3) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1244, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 25), new NbtShort("lvl", 1) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1245, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 25), new NbtShort("lvl", 2) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1246, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 26), new NbtShort("lvl", 1) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1247, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 27), new NbtShort("lvl", 1) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1248, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 28), new NbtShort("lvl", 1) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1249, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 29), new NbtShort("lvl", 1) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1250, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 29), new NbtShort("lvl", 2) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1251, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 29), new NbtShort("lvl", 3) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1252, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 29), new NbtShort("lvl", 4) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1253, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 29), new NbtShort("lvl", 5) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1254, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 30), new NbtShort("lvl", 1) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1255, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 30), new NbtShort("lvl", 2) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1256, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 30), new NbtShort("lvl", 3) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1257, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 31), new NbtShort("lvl", 1) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1258, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 31), new NbtShort("lvl", 2) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1259, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 31), new NbtShort("lvl", 3) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1260, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 32), new NbtShort("lvl", 1) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1261, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 33), new NbtShort("lvl", 1) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1262, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 34), new NbtShort("lvl", 1) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1263, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 34), new NbtShort("lvl", 2) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1264, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 34), new NbtShort("lvl", 3) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1265, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 34), new NbtShort("lvl", 4) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1266, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 35), new NbtShort("lvl", 1) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1267, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 35), new NbtShort("lvl", 2) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1268, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 35), new NbtShort("lvl", 3) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1269, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 36), new NbtShort("lvl", 1) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1270, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 36), new NbtShort("lvl", 2) } } } },/*minecraft:enchanted_book*/
            new Item(403, 0, 1){ NetworkId=1271, ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 36), new NbtShort("lvl", 3) } } } },/*minecraft:enchanted_book*/
            new Item(333, 0, 1){ NetworkId=1272 }, /*minecraft:boat*/
            new Item(333, 1, 1){ NetworkId=1273 }, /*minecraft:boat*/
            new Item(333, 2, 1){ NetworkId=1274 }, /*minecraft:boat*/
            new Item(333, 3, 1){ NetworkId=1275 }, /*minecraft:boat*/
            new Item(333, 4, 1){ NetworkId=1276 }, /*minecraft:boat*/
            new Item(333, 5, 1){ NetworkId=1277 }, /*minecraft:boat*/
            new Item(66, 0, 1){ NetworkId=1278 }, /*minecraft:rail*/
            new Item(27, 0, 1){ NetworkId=1279 }, /*minecraft:golden_rail*/
            new Item(28, 0, 1){ NetworkId=1280 }, /*minecraft:detector_rail*/
            new Item(126, 0, 1){ NetworkId=1281 }, /*minecraft:activator_rail*/
            new Item(328, 0, 1){ NetworkId=1282 }, /*minecraft:minecart*/
            new Item(342, 0, 1){ NetworkId=1283 }, /*minecraft:chest_minecart*/
            new Item(408, 0, 1){ NetworkId=1284 }, /*minecraft:hopper_minecart*/
            new Item(407, 0, 1){ NetworkId=1285 }, /*minecraft:tnt_minecart*/
            new Item(331, 0, 1){ NetworkId=1286 }, /*minecraft:redstone*/
            new Item(152, 0, 1){ NetworkId=1287 }, /*minecraft:redstone_block*/
            new Item(76, 0, 1){ NetworkId=1288 }, /*minecraft:redstone_torch*/
            new Item(69, 0, 1){ NetworkId=1289 }, /*minecraft:lever*/
            new Item(143, 0, 1){ NetworkId=1290 }, /*minecraft:wooden_button*/
            new Item(-144, 0, 1){ NetworkId=1291 }, /*minecraft:spruce_button*/
            new Item(-141, 0, 1){ NetworkId=1292 }, /*minecraft:birch_button*/
            new Item(-143, 0, 1){ NetworkId=1293 }, /*minecraft:jungle_button*/
            new Item(-140, 0, 1){ NetworkId=1294 }, /*minecraft:acacia_button*/
            new Item(-142, 0, 1){ NetworkId=1295 }, /*minecraft:dark_oak_button*/
            new Item(77, 0, 1){ NetworkId=1296 }, /*minecraft:stone_button*/
            new Item(-260, 0, 1){ NetworkId=1297 }, /*minecraft:crimson_button*/
            new Item(-261, 0, 1){ NetworkId=1298 }, /*minecraft:warped_button*/
            new Item(-296, 0, 1){ NetworkId=1299 }, /*minecraft:polished_blackstone_button*/
            new Item(131, 0, 1){ NetworkId=1300 }, /*minecraft:tripwire_hook*/
            new Item(72, 0, 1){ NetworkId=1301 }, /*minecraft:wooden_pressure_plate*/
            new Item(-154, 0, 1){ NetworkId=1302 }, /*minecraft:spruce_pressure_plate*/
            new Item(-151, 0, 1){ NetworkId=1303 }, /*minecraft:birch_pressure_plate*/
            new Item(-153, 0, 1){ NetworkId=1304 }, /*minecraft:jungle_pressure_plate*/
            new Item(-150, 0, 1){ NetworkId=1305 }, /*minecraft:acacia_pressure_plate*/
            new Item(-152, 0, 1){ NetworkId=1306 }, /*minecraft:dark_oak_pressure_plate*/
            new Item(-262, 0, 1){ NetworkId=1307 }, /*minecraft:crimson_pressure_plate*/
            new Item(-263, 0, 1){ NetworkId=1308 }, /*minecraft:warped_pressure_plate*/
            new Item(70, 0, 1){ NetworkId=1309 }, /*minecraft:stone_pressure_plate*/
            new Item(147, 0, 1){ NetworkId=1310 }, /*minecraft:light_weighted_pressure_plate*/
            new Item(148, 0, 1){ NetworkId=1311 }, /*minecraft:heavy_weighted_pressure_plate*/
            new Item(-295, 0, 1){ NetworkId=1312 }, /*minecraft:polished_blackstone_pressure_plate*/
            new Item(251, 0, 1){ NetworkId=1313 }, /*minecraft:observer*/
            new Item(151, 0, 1){ NetworkId=1314 }, /*minecraft:daylight_detector*/
            new Item(356, 0, 1){ NetworkId=1315 }, /*minecraft:repeater*/
            new Item(404, 0, 1){ NetworkId=1316 }, /*minecraft:comparator*/
            new Item(410, 0, 1){ NetworkId=1317 }, /*minecraft:hopper*/
            new Item(125, 0, 1){ NetworkId=1318 }, /*minecraft:dropper*/
            new Item(23, 0, 1){ NetworkId=1319 }, /*minecraft:dispenser*/
            new Item(33, 0, 1){ NetworkId=1320 }, /*minecraft:piston*/
            new Item(29, 0, 1){ NetworkId=1321 }, /*minecraft:sticky_piston*/
            new Item(46, 0, 1){ NetworkId=1322 }, /*minecraft:tnt*/
            new Item(421, 0, 1){ NetworkId=1323 }, /*minecraft:name_tag*/
            new Item(-204, 0, 1){ NetworkId=1324 }, /*minecraft:loom*/
            new Item(446, 0, 1){ NetworkId=1325 }, /*minecraft:banner*/
            new Item(446, 8, 1){ NetworkId=1326 }, /*minecraft:banner*/
            new Item(446, 7, 1){ NetworkId=1327 }, /*minecraft:banner*/
            new Item(446, 15, 1){ NetworkId=1328 }, /*minecraft:banner*/
            new Item(446, 12, 1){ NetworkId=1329 }, /*minecraft:banner*/
            new Item(446, 14, 1){ NetworkId=1330 }, /*minecraft:banner*/
            new Item(446, 1, 1){ NetworkId=1331 }, /*minecraft:banner*/
            new Item(446, 4, 1){ NetworkId=1332 }, /*minecraft:banner*/
            new Item(446, 5, 1){ NetworkId=1333 }, /*minecraft:banner*/
            new Item(446, 13, 1){ NetworkId=1334 }, /*minecraft:banner*/
            new Item(446, 9, 1){ NetworkId=1335 }, /*minecraft:banner*/
            new Item(446, 3, 1){ NetworkId=1336 }, /*minecraft:banner*/
            new Item(446, 11, 1){ NetworkId=1337 }, /*minecraft:banner*/
            new Item(446, 10, 1){ NetworkId=1338 }, /*minecraft:banner*/
            new Item(446, 2, 1){ NetworkId=1339 }, /*minecraft:banner*/
            new Item(446, 6, 1){ NetworkId=1340 }, /*minecraft:banner*/
            new Item(434, 0, 1){ NetworkId=1342 }, /*minecraft:banner_pattern*/
            new Item(434, 1, 1){ NetworkId=1343 }, /*minecraft:banner_pattern*/
            new Item(434, 2, 1){ NetworkId=1344 }, /*minecraft:banner_pattern*/
            new Item(434, 3, 1){ NetworkId=1345 }, /*minecraft:banner_pattern*/
            new Item(434, 4, 1){ NetworkId=1346 }, /*minecraft:banner_pattern*/
            new Item(434, 5, 1){ NetworkId=1347 }, /*minecraft:banner_pattern*/
            new Item(434, 6, 1){ NetworkId=1348 }, /*minecraft:banner_pattern*/
            new Item(401, 0, 1){ NetworkId=1349, ExtraData = new NbtCompound {new NbtCompound("Fireworks") {new NbtList("Explosions", NbtTagType.Compound), new NbtByte("Flight", 1) } } },/*minecraft:firework_rocket*/
            new Item(401, 0, 1){ NetworkId=1350, ExtraData = new NbtCompound { new NbtCompound("Fireworks") { new NbtList("Explosions") { new NbtCompound { new NbtByteArray("FireworkColor", new byte[]{0}), new NbtByteArray("FireworkFade", new byte[0]), new NbtByte("FireworkFlicker", 0), new NbtByte("FireworkTrail", 0), new NbtByte("FireworkType", 0)  } }, new NbtByte("Flight", 1) } } },/*minecraft:firework_rocket*/
            new Item(401, 0, 1){ NetworkId=1351, ExtraData = new NbtCompound { new NbtCompound("Fireworks") { new NbtList("Explosions") { new NbtCompound { new NbtByteArray("FireworkColor", new byte[]{8}), new NbtByteArray("FireworkFade", new byte[0]), new NbtByte("FireworkFlicker", 0), new NbtByte("FireworkTrail", 0), new NbtByte("FireworkType", 0)  } }, new NbtByte("Flight", 1) } } },/*minecraft:firework_rocket*/
            new Item(401, 0, 1){ NetworkId=1352, ExtraData = new NbtCompound { new NbtCompound("Fireworks") { new NbtList("Explosions") { new NbtCompound { new NbtByteArray("FireworkColor", new byte[]{7}), new NbtByteArray("FireworkFade", new byte[0]), new NbtByte("FireworkFlicker", 0), new NbtByte("FireworkTrail", 0), new NbtByte("FireworkType", 0)  } }, new NbtByte("Flight", 1) } } },/*minecraft:firework_rocket*/
            new Item(401, 0, 1){ NetworkId=1353, ExtraData = new NbtCompound { new NbtCompound("Fireworks") { new NbtList("Explosions") { new NbtCompound { new NbtByteArray("FireworkColor", new byte[]{15}), new NbtByteArray("FireworkFade", new byte[0]), new NbtByte("FireworkFlicker", 0), new NbtByte("FireworkTrail", 0), new NbtByte("FireworkType", 0)  } }, new NbtByte("Flight", 1) } } },/*minecraft:firework_rocket*/
            new Item(401, 0, 1){ NetworkId=1354, ExtraData = new NbtCompound { new NbtCompound("Fireworks") { new NbtList("Explosions") { new NbtCompound { new NbtByteArray("FireworkColor", new byte[]{12}), new NbtByteArray("FireworkFade", new byte[0]), new NbtByte("FireworkFlicker", 0), new NbtByte("FireworkTrail", 0), new NbtByte("FireworkType", 0)  } }, new NbtByte("Flight", 1) } } },/*minecraft:firework_rocket*/
            new Item(401, 0, 1){ NetworkId=1355, ExtraData = new NbtCompound { new NbtCompound("Fireworks") { new NbtList("Explosions") { new NbtCompound { new NbtByteArray("FireworkColor", new byte[]{14}), new NbtByteArray("FireworkFade", new byte[0]), new NbtByte("FireworkFlicker", 0), new NbtByte("FireworkTrail", 0), new NbtByte("FireworkType", 0)  } }, new NbtByte("Flight", 1) } } },/*minecraft:firework_rocket*/
            new Item(401, 0, 1){ NetworkId=1356, ExtraData = new NbtCompound { new NbtCompound("Fireworks") { new NbtList("Explosions") { new NbtCompound { new NbtByteArray("FireworkColor", new byte[]{1}), new NbtByteArray("FireworkFade", new byte[0]), new NbtByte("FireworkFlicker", 0), new NbtByte("FireworkTrail", 0), new NbtByte("FireworkType", 0)  } }, new NbtByte("Flight", 1) } } },/*minecraft:firework_rocket*/
            new Item(401, 0, 1){ NetworkId=1357, ExtraData = new NbtCompound { new NbtCompound("Fireworks") { new NbtList("Explosions") { new NbtCompound { new NbtByteArray("FireworkColor", new byte[]{4}), new NbtByteArray("FireworkFade", new byte[0]), new NbtByte("FireworkFlicker", 0), new NbtByte("FireworkTrail", 0), new NbtByte("FireworkType", 0)  } }, new NbtByte("Flight", 1) } } },/*minecraft:firework_rocket*/
            new Item(401, 0, 1){ NetworkId=1358, ExtraData = new NbtCompound { new NbtCompound("Fireworks") { new NbtList("Explosions") { new NbtCompound { new NbtByteArray("FireworkColor", new byte[]{5}), new NbtByteArray("FireworkFade", new byte[0]), new NbtByte("FireworkFlicker", 0), new NbtByte("FireworkTrail", 0), new NbtByte("FireworkType", 0)  } }, new NbtByte("Flight", 1) } } },/*minecraft:firework_rocket*/
            new Item(401, 0, 1){ NetworkId=1359, ExtraData = new NbtCompound { new NbtCompound("Fireworks") { new NbtList("Explosions") { new NbtCompound { new NbtByteArray("FireworkColor", new byte[]{13}), new NbtByteArray("FireworkFade", new byte[0]), new NbtByte("FireworkFlicker", 0), new NbtByte("FireworkTrail", 0), new NbtByte("FireworkType", 0)  } }, new NbtByte("Flight", 1) } } },/*minecraft:firework_rocket*/
            new Item(401, 0, 1){ NetworkId=1360, ExtraData = new NbtCompound { new NbtCompound("Fireworks") { new NbtList("Explosions") { new NbtCompound { new NbtByteArray("FireworkColor", new byte[]{9}), new NbtByteArray("FireworkFade", new byte[0]), new NbtByte("FireworkFlicker", 0), new NbtByte("FireworkTrail", 0), new NbtByte("FireworkType", 0)  } }, new NbtByte("Flight", 1) } } },/*minecraft:firework_rocket*/
            new Item(401, 0, 1){ NetworkId=1361, ExtraData = new NbtCompound { new NbtCompound("Fireworks") { new NbtList("Explosions") { new NbtCompound { new NbtByteArray("FireworkColor", new byte[]{3}), new NbtByteArray("FireworkFade", new byte[0]), new NbtByte("FireworkFlicker", 0), new NbtByte("FireworkTrail", 0), new NbtByte("FireworkType", 0)  } }, new NbtByte("Flight", 1) } } },/*minecraft:firework_rocket*/
            new Item(401, 0, 1){ NetworkId=1362, ExtraData = new NbtCompound { new NbtCompound("Fireworks") { new NbtList("Explosions") { new NbtCompound { new NbtByteArray("FireworkColor", new byte[]{11}), new NbtByteArray("FireworkFade", new byte[0]), new NbtByte("FireworkFlicker", 0), new NbtByte("FireworkTrail", 0), new NbtByte("FireworkType", 0)  } }, new NbtByte("Flight", 1) } } },/*minecraft:firework_rocket*/
            new Item(401, 0, 1){ NetworkId=1363, ExtraData = new NbtCompound { new NbtCompound("Fireworks") { new NbtList("Explosions") { new NbtCompound { new NbtByteArray("FireworkColor", new byte[]{10}), new NbtByteArray("FireworkFade", new byte[0]), new NbtByte("FireworkFlicker", 0), new NbtByte("FireworkTrail", 0), new NbtByte("FireworkType", 0)  } }, new NbtByte("Flight", 1) } } },/*minecraft:firework_rocket*/
            new Item(401, 0, 1){ NetworkId=1364, ExtraData = new NbtCompound { new NbtCompound("Fireworks") { new NbtList("Explosions") { new NbtCompound { new NbtByteArray("FireworkColor", new byte[]{2}), new NbtByteArray("FireworkFade", new byte[0]), new NbtByte("FireworkFlicker", 0), new NbtByte("FireworkTrail", 0), new NbtByte("FireworkType", 0)  } }, new NbtByte("Flight", 1) } } },/*minecraft:firework_rocket*/
            new Item(401, 0, 1){ NetworkId=1365, ExtraData = new NbtCompound { new NbtCompound("Fireworks") { new NbtList("Explosions") { new NbtCompound { new NbtByteArray("FireworkColor", new byte[]{6}), new NbtByteArray("FireworkFade", new byte[0]), new NbtByte("FireworkFlicker", 0), new NbtByte("FireworkTrail", 0), new NbtByte("FireworkType", 0)  } }, new NbtByte("Flight", 1) } } },/*minecraft:firework_rocket*/
            new Item(758, 0, 1){ NetworkId=1382 }, /*minecraft:chain*/
            new Item(-239, 0, 1){ NetworkId=1383 }, /*minecraft:target*/
            new Item(741, 0, 1){ NetworkId=1384 }, /*minecraft:lodestone_compass*/
        };
	}
}