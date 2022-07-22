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
            new Item(5, 0, 1){ RuntimeId=6071, NetworkId=1, ExtraData = null }, /*minecraft:planks*/
            new Item(5, 0, 1){ RuntimeId=6072, NetworkId=2, ExtraData = null }, /*minecraft:planks*/
            new Item(5, 0, 1){ RuntimeId=6073, NetworkId=3, ExtraData = null }, /*minecraft:planks*/
            new Item(5, 0, 1){ RuntimeId=6074, NetworkId=4, ExtraData = null }, /*minecraft:planks*/
            new Item(5, 0, 1){ RuntimeId=6075, NetworkId=5, ExtraData = null }, /*minecraft:planks*/
            new Item(5, 0, 1){ RuntimeId=6076, NetworkId=6, ExtraData = null }, /*minecraft:planks*/
            new Item(-486, 0, 1){ RuntimeId=947, NetworkId=7, ExtraData = null }, /**/
            new Item(-242, 0, 1){ RuntimeId=4850, NetworkId=8, ExtraData = null }, /*minecraft:crimson_planks*/
            new Item(-243, 0, 1){ RuntimeId=920, NetworkId=9, ExtraData = null }, /*minecraft:warped_planks*/
            new Item(139, 0, 1){ RuntimeId=1182, NetworkId=10, ExtraData = null }, /*minecraft:cobblestone_wall*/
            new Item(139, 0, 1){ RuntimeId=1183, NetworkId=11, ExtraData = null }, /*minecraft:cobblestone_wall*/
            new Item(139, 0, 1){ RuntimeId=1184, NetworkId=12, ExtraData = null }, /*minecraft:cobblestone_wall*/
            new Item(139, 0, 1){ RuntimeId=1185, NetworkId=13, ExtraData = null }, /*minecraft:cobblestone_wall*/
            new Item(139, 0, 1){ RuntimeId=1186, NetworkId=14, ExtraData = null }, /*minecraft:cobblestone_wall*/
            new Item(139, 0, 1){ RuntimeId=1187, NetworkId=15, ExtraData = null }, /*minecraft:cobblestone_wall*/
            new Item(139, 0, 1){ RuntimeId=1194, NetworkId=16, ExtraData = null }, /*minecraft:cobblestone_wall*/
            new Item(139, 0, 1){ RuntimeId=1189, NetworkId=17, ExtraData = null }, /*minecraft:cobblestone_wall*/
            new Item(139, 0, 1){ RuntimeId=1190, NetworkId=18, ExtraData = null }, /*minecraft:cobblestone_wall*/
            new Item(139, 0, 1){ RuntimeId=1188, NetworkId=19, ExtraData = null }, /*minecraft:cobblestone_wall*/
            new Item(139, 0, 1){ RuntimeId=1191, NetworkId=20, ExtraData = null }, /*minecraft:cobblestone_wall*/
            new Item(139, 0, 1){ RuntimeId=1195, NetworkId=21, ExtraData = null }, /*minecraft:cobblestone_wall*/
            new Item(139, 0, 1){ RuntimeId=1192, NetworkId=22, ExtraData = null }, /*minecraft:cobblestone_wall*/
            new Item(139, 0, 1){ RuntimeId=1193, NetworkId=23, ExtraData = null }, /*minecraft:cobblestone_wall*/
            new Item(-277, 0, 1){ RuntimeId=3930, NetworkId=24, ExtraData = null }, /*minecraft:blackstone_wall*/
            new Item(-297, 0, 1){ RuntimeId=6724, NetworkId=25, ExtraData = null }, /*minecraft:polished_blackstone_wall*/
            new Item(-278, 0, 1){ RuntimeId=971, NetworkId=26, ExtraData = null }, /*minecraft:polished_blackstone_brick_wall*/
            new Item(-382, 0, 1){ RuntimeId=8082, NetworkId=27, ExtraData = null }, /**/
            new Item(-390, 0, 1){ RuntimeId=5071, NetworkId=28, ExtraData = null }, /**/
            new Item(-386, 0, 1){ RuntimeId=7817, NetworkId=29, ExtraData = null }, /**/
            new Item(-394, 0, 1){ RuntimeId=429, NetworkId=30, ExtraData = null }, /**/
            new Item(-481, 0, 1){ RuntimeId=730, NetworkId=31, ExtraData = null }, /**/
            new Item(85, 0, 1){ RuntimeId=7364, NetworkId=32, ExtraData = null }, /*minecraft:fence*/
            new Item(85, 0, 1){ RuntimeId=7365, NetworkId=33, ExtraData = null }, /*minecraft:fence*/
            new Item(85, 0, 1){ RuntimeId=7366, NetworkId=34, ExtraData = null }, /*minecraft:fence*/
            new Item(85, 0, 1){ RuntimeId=7367, NetworkId=35, ExtraData = null }, /*minecraft:fence*/
            new Item(85, 0, 1){ RuntimeId=7368, NetworkId=36, ExtraData = null }, /*minecraft:fence*/
            new Item(85, 0, 1){ RuntimeId=7369, NetworkId=37, ExtraData = null }, /*minecraft:fence*/
            new Item(-491, 0, 1){ RuntimeId=6633, NetworkId=38, ExtraData = null }, /**/
            new Item(113, 0, 1){ RuntimeId=4290, NetworkId=39, ExtraData = null }, /*minecraft:nether_brick_fence*/
            new Item(-256, 0, 1){ RuntimeId=7996, NetworkId=40, ExtraData = null }, /*minecraft:crimson_fence*/
            new Item(-257, 0, 1){ RuntimeId=5853, NetworkId=41, ExtraData = null }, /*minecraft:warped_fence*/
            new Item(107, 0, 1){ RuntimeId=76, NetworkId=42, ExtraData = null }, /*minecraft:fence_gate*/
            new Item(183, 0, 1){ RuntimeId=6584, NetworkId=43, ExtraData = null }, /*minecraft:spruce_fence_gate*/
            new Item(184, 0, 1){ RuntimeId=3777, NetworkId=44, ExtraData = null }, /*minecraft:birch_fence_gate*/
            new Item(185, 0, 1){ RuntimeId=5365, NetworkId=45, ExtraData = null }, /*minecraft:jungle_fence_gate*/
            new Item(187, 0, 1){ RuntimeId=7586, NetworkId=46, ExtraData = null }, /*minecraft:acacia_fence_gate*/
            new Item(186, 0, 1){ RuntimeId=4173, NetworkId=47, ExtraData = null }, /*minecraft:dark_oak_fence_gate*/
            new Item(-492, 0, 1){ RuntimeId=4625, NetworkId=48, ExtraData = null }, /**/
            new Item(-258, 0, 1){ RuntimeId=4661, NetworkId=49, ExtraData = null }, /*minecraft:crimson_fence_gate*/
            new Item(-259, 0, 1){ RuntimeId=5399, NetworkId=50, ExtraData = null }, /*minecraft:warped_fence_gate*/
            new Item(-180, 0, 1){ RuntimeId=633, NetworkId=51, ExtraData = null }, /*minecraft:normal_stone_stairs*/
            new Item(67, 0, 1){ RuntimeId=3708, NetworkId=52, ExtraData = null }, /*minecraft:stone_stairs*/
            new Item(-179, 0, 1){ RuntimeId=4092, NetworkId=53, ExtraData = null }, /*minecraft:mossy_cobblestone_stairs*/
            new Item(53, 0, 1){ RuntimeId=273, NetworkId=54, ExtraData = null }, /*minecraft:oak_stairs*/
            new Item(134, 0, 1){ RuntimeId=128, NetworkId=55, ExtraData = null }, /*minecraft:spruce_stairs*/
            new Item(135, 0, 1){ RuntimeId=7003, NetworkId=56, ExtraData = null }, /*minecraft:birch_stairs*/
            new Item(136, 0, 1){ RuntimeId=6967, NetworkId=57, ExtraData = null }, /*minecraft:jungle_stairs*/
            new Item(163, 0, 1){ RuntimeId=6200, NetworkId=58, ExtraData = null }, /*minecraft:acacia_stairs*/
            new Item(164, 0, 1){ RuntimeId=5063, NetworkId=59, ExtraData = null }, /*minecraft:dark_oak_stairs*/
            new Item(-488, 0, 1){ RuntimeId=4595, NetworkId=60, ExtraData = null }, /**/
            new Item(109, 0, 1){ RuntimeId=931, NetworkId=61, ExtraData = null }, /*minecraft:stone_brick_stairs*/
            new Item(-175, 0, 1){ RuntimeId=5883, NetworkId=62, ExtraData = null }, /*minecraft:mossy_stone_brick_stairs*/
            new Item(128, 0, 1){ RuntimeId=3587, NetworkId=63, ExtraData = null }, /*minecraft:sandstone_stairs*/
            new Item(-177, 0, 1){ RuntimeId=3627, NetworkId=64, ExtraData = null }, /*minecraft:smooth_sandstone_stairs*/
            new Item(180, 0, 1){ RuntimeId=5350, NetworkId=65, ExtraData = null }, /*minecraft:red_sandstone_stairs*/
            new Item(-176, 0, 1){ RuntimeId=5546, NetworkId=66, ExtraData = null }, /*minecraft:smooth_red_sandstone_stairs*/
            new Item(-169, 0, 1){ RuntimeId=3537, NetworkId=67, ExtraData = null }, /*minecraft:granite_stairs*/
            new Item(-172, 0, 1){ RuntimeId=4150, NetworkId=68, ExtraData = null }, /*minecraft:polished_granite_stairs*/
            new Item(-170, 0, 1){ RuntimeId=4391, NetworkId=69, ExtraData = null }, /*minecraft:diorite_stairs*/
            new Item(-173, 0, 1){ RuntimeId=6714, NetworkId=70, ExtraData = null }, /*minecraft:polished_diorite_stairs*/
            new Item(-171, 0, 1){ RuntimeId=5308, NetworkId=71, ExtraData = null }, /*minecraft:andesite_stairs*/
            new Item(-174, 0, 1){ RuntimeId=7028, NetworkId=72, ExtraData = null }, /*minecraft:polished_andesite_stairs*/
            new Item(108, 0, 1){ RuntimeId=6530, NetworkId=73, ExtraData = null }, /*minecraft:brick_stairs*/
            new Item(114, 0, 1){ RuntimeId=106, NetworkId=74, ExtraData = null }, /*minecraft:nether_brick_stairs*/
            new Item(-184, 0, 1){ RuntimeId=6602, NetworkId=75, ExtraData = null }, /*minecraft:red_nether_brick_stairs*/
            new Item(-178, 0, 1){ RuntimeId=6382, NetworkId=76, ExtraData = null }, /*minecraft:end_brick_stairs*/
            new Item(156, 0, 1){ RuntimeId=4767, NetworkId=77, ExtraData = null }, /*minecraft:quartz_stairs*/
            new Item(-185, 0, 1){ RuntimeId=7700, NetworkId=78, ExtraData = null }, /*minecraft:smooth_quartz_stairs*/
            new Item(203, 0, 1){ RuntimeId=7755, NetworkId=79, ExtraData = null }, /*minecraft:purpur_stairs*/
            new Item(-2, 0, 1){ RuntimeId=7263, NetworkId=80, ExtraData = null }, /*minecraft:prismarine_stairs*/
            new Item(-3, 0, 1){ RuntimeId=7430, NetworkId=81, ExtraData = null }, /*minecraft:dark_prismarine_stairs*/
            new Item(-4, 0, 1){ RuntimeId=206, NetworkId=82, ExtraData = null }, /*minecraft:prismarine_bricks_stairs*/
            new Item(-254, 0, 1){ RuntimeId=6280, NetworkId=83, ExtraData = null }, /*minecraft:crimson_stairs*/
            new Item(-255, 0, 1){ RuntimeId=3718, NetworkId=84, ExtraData = null }, /*minecraft:warped_stairs*/
            new Item(-276, 0, 1){ RuntimeId=7019, NetworkId=85, ExtraData = null }, /*minecraft:blackstone_stairs*/
            new Item(-292, 0, 1){ RuntimeId=4297, NetworkId=86, ExtraData = null }, /*minecraft:polished_blackstone_stairs*/
            new Item(-275, 0, 1){ RuntimeId=4477, NetworkId=87, ExtraData = null }, /*minecraft:polished_blackstone_brick_stairs*/
            new Item(-354, 0, 1){ RuntimeId=4604, NetworkId=88, ExtraData = null }, /**/
            new Item(-355, 0, 1){ RuntimeId=4587, NetworkId=89, ExtraData = null }, /**/
            new Item(-356, 0, 1){ RuntimeId=4305, NetworkId=90, ExtraData = null }, /**/
            new Item(-357, 0, 1){ RuntimeId=351, NetworkId=91, ExtraData = null }, /**/
            new Item(-358, 0, 1){ RuntimeId=393, NetworkId=92, ExtraData = null }, /**/
            new Item(-359, 0, 1){ RuntimeId=3902, NetworkId=93, ExtraData = null }, /**/
            new Item(-360, 0, 1){ RuntimeId=6167, NetworkId=94, ExtraData = null }, /**/
            new Item(-448, 0, 1){ RuntimeId=5840, NetworkId=95, ExtraData = null }, /**/
            new Item(-381, 0, 1){ RuntimeId=147, NetworkId=96, ExtraData = null }, /**/
            new Item(-389, 0, 1){ RuntimeId=4653, NetworkId=97, ExtraData = null }, /**/
            new Item(-385, 0, 1){ RuntimeId=294, NetworkId=98, ExtraData = null }, /**/
            new Item(-393, 0, 1){ RuntimeId=7422, NetworkId=99, ExtraData = null }, /**/
            new Item(-480, 0, 1){ RuntimeId=5522, NetworkId=100, ExtraData = null }, /**/
            new Item(324, 0, 1){ RuntimeId=0, NetworkId=101, ExtraData = null }, /*minecraft:wooden_door*/
            new Item(427, 0, 1){ RuntimeId=0, NetworkId=102, ExtraData = null }, /*minecraft:spruce_door*/
            new Item(428, 0, 1){ RuntimeId=0, NetworkId=103, ExtraData = null }, /*minecraft:birch_door*/
            new Item(429, 0, 1){ RuntimeId=0, NetworkId=104, ExtraData = null }, /*minecraft:jungle_door*/
            new Item(430, 0, 1){ RuntimeId=0, NetworkId=105, ExtraData = null }, /*minecraft:acacia_door*/
            new Item(431, 0, 1){ RuntimeId=0, NetworkId=106, ExtraData = null }, /*minecraft:dark_oak_door*/
            new Item(633, 0, 1){ RuntimeId=0, NetworkId=107, ExtraData = null }, /**/
            new Item(330, 0, 1){ RuntimeId=0, NetworkId=108, ExtraData = null }, /*minecraft:iron_door*/
            new Item(757, 0, 1){ RuntimeId=0, NetworkId=109, ExtraData = null }, /*minecraft:warped_fungus_on_a_stick*/
            new Item(758, 0, 1){ RuntimeId=0, NetworkId=110, ExtraData = null }, /*minecraft:chain*/
            new Item(96, 0, 1){ RuntimeId=229, NetworkId=111, ExtraData = null }, /*minecraft:trapdoor*/
            new Item(-149, 0, 1){ RuntimeId=6552, NetworkId=112, ExtraData = null }, /*minecraft:spruce_trapdoor*/
            new Item(-146, 0, 1){ RuntimeId=6650, NetworkId=113, ExtraData = null }, /*minecraft:birch_trapdoor*/
            new Item(-148, 0, 1){ RuntimeId=5381, NetworkId=114, ExtraData = null }, /*minecraft:jungle_trapdoor*/
            new Item(-145, 0, 1){ RuntimeId=5589, NetworkId=115, ExtraData = null }, /*minecraft:acacia_trapdoor*/
            new Item(-147, 0, 1){ RuntimeId=7502, NetworkId=116, ExtraData = null }, /*minecraft:dark_oak_trapdoor*/
            new Item(-496, 0, 1){ RuntimeId=4485, NetworkId=117, ExtraData = null }, /**/
            new Item(167, 0, 1){ RuntimeId=321, NetworkId=118, ExtraData = null }, /*minecraft:iron_trapdoor*/
            new Item(-246, 0, 1){ RuntimeId=4333, NetworkId=119, ExtraData = null }, /*minecraft:crimson_trapdoor*/
            new Item(-247, 0, 1){ RuntimeId=4733, NetworkId=120, ExtraData = null }, /*minecraft:warped_trapdoor*/
            new Item(101, 0, 1){ RuntimeId=4801, NetworkId=121, ExtraData = null }, /*minecraft:iron_bars*/
            new Item(20, 0, 1){ RuntimeId=6164, NetworkId=122, ExtraData = null }, /*minecraft:glass*/
            new Item(241, 0, 1){ RuntimeId=1133, NetworkId=123, ExtraData = null }, /*minecraft:stained_glass*/
            new Item(241, 0, 1){ RuntimeId=1141, NetworkId=124, ExtraData = null }, /*minecraft:stained_glass*/
            new Item(241, 0, 1){ RuntimeId=1140, NetworkId=125, ExtraData = null }, /*minecraft:stained_glass*/
            new Item(241, 0, 1){ RuntimeId=1148, NetworkId=126, ExtraData = null }, /*minecraft:stained_glass*/
            new Item(241, 0, 1){ RuntimeId=1145, NetworkId=127, ExtraData = null }, /*minecraft:stained_glass*/
            new Item(241, 0, 1){ RuntimeId=1147, NetworkId=128, ExtraData = null }, /*minecraft:stained_glass*/
            new Item(241, 0, 1){ RuntimeId=1134, NetworkId=129, ExtraData = null }, /*minecraft:stained_glass*/
            new Item(241, 0, 1){ RuntimeId=1137, NetworkId=130, ExtraData = null }, /*minecraft:stained_glass*/
            new Item(241, 0, 1){ RuntimeId=1138, NetworkId=131, ExtraData = null }, /*minecraft:stained_glass*/
            new Item(241, 0, 1){ RuntimeId=1146, NetworkId=132, ExtraData = null }, /*minecraft:stained_glass*/
            new Item(241, 0, 1){ RuntimeId=1142, NetworkId=133, ExtraData = null }, /*minecraft:stained_glass*/
            new Item(241, 0, 1){ RuntimeId=1136, NetworkId=134, ExtraData = null }, /*minecraft:stained_glass*/
            new Item(241, 0, 1){ RuntimeId=1144, NetworkId=135, ExtraData = null }, /*minecraft:stained_glass*/
            new Item(241, 0, 1){ RuntimeId=1143, NetworkId=136, ExtraData = null }, /*minecraft:stained_glass*/
            new Item(241, 0, 1){ RuntimeId=1135, NetworkId=137, ExtraData = null }, /*minecraft:stained_glass*/
            new Item(241, 0, 1){ RuntimeId=1139, NetworkId=138, ExtraData = null }, /*minecraft:stained_glass*/
            new Item(-334, 0, 1){ RuntimeId=5975, NetworkId=139, ExtraData = null }, /**/
            new Item(102, 0, 1){ RuntimeId=5233, NetworkId=140, ExtraData = null }, /*minecraft:glass_pane*/
            new Item(160, 0, 1){ RuntimeId=4852, NetworkId=141, ExtraData = null }, /*minecraft:stained_glass_pane*/
            new Item(160, 0, 1){ RuntimeId=4860, NetworkId=142, ExtraData = null }, /*minecraft:stained_glass_pane*/
            new Item(160, 0, 1){ RuntimeId=4859, NetworkId=143, ExtraData = null }, /*minecraft:stained_glass_pane*/
            new Item(160, 0, 1){ RuntimeId=4867, NetworkId=144, ExtraData = null }, /*minecraft:stained_glass_pane*/
            new Item(160, 0, 1){ RuntimeId=4864, NetworkId=145, ExtraData = null }, /*minecraft:stained_glass_pane*/
            new Item(160, 0, 1){ RuntimeId=4866, NetworkId=146, ExtraData = null }, /*minecraft:stained_glass_pane*/
            new Item(160, 0, 1){ RuntimeId=4853, NetworkId=147, ExtraData = null }, /*minecraft:stained_glass_pane*/
            new Item(160, 0, 1){ RuntimeId=4856, NetworkId=148, ExtraData = null }, /*minecraft:stained_glass_pane*/
            new Item(160, 0, 1){ RuntimeId=4857, NetworkId=149, ExtraData = null }, /*minecraft:stained_glass_pane*/
            new Item(160, 0, 1){ RuntimeId=4865, NetworkId=150, ExtraData = null }, /*minecraft:stained_glass_pane*/
            new Item(160, 0, 1){ RuntimeId=4861, NetworkId=151, ExtraData = null }, /*minecraft:stained_glass_pane*/
            new Item(160, 0, 1){ RuntimeId=4855, NetworkId=152, ExtraData = null }, /*minecraft:stained_glass_pane*/
            new Item(160, 0, 1){ RuntimeId=4863, NetworkId=153, ExtraData = null }, /*minecraft:stained_glass_pane*/
            new Item(160, 0, 1){ RuntimeId=4862, NetworkId=154, ExtraData = null }, /*minecraft:stained_glass_pane*/
            new Item(160, 0, 1){ RuntimeId=4854, NetworkId=155, ExtraData = null }, /*minecraft:stained_glass_pane*/
            new Item(160, 0, 1){ RuntimeId=4858, NetworkId=156, ExtraData = null }, /*minecraft:stained_glass_pane*/
            new Item(65, 0, 1){ RuntimeId=8262, NetworkId=157, ExtraData = null }, /*minecraft:ladder*/
            new Item(-165, 0, 1){ RuntimeId=3571, NetworkId=158, ExtraData = null }, /*minecraft:scaffolding*/
            new Item(44, 0, 1){ RuntimeId=4270, NetworkId=159, ExtraData = null }, /*minecraft:stone_slab*/
            new Item(-166, 0, 1){ RuntimeId=5822, NetworkId=160, ExtraData = null }, /*minecraft:stone_slab4*/
            new Item(44, 0, 1){ RuntimeId=4273, NetworkId=161, ExtraData = null }, /*minecraft:stone_slab*/
            new Item(182, 0, 1){ RuntimeId=5793, NetworkId=162, ExtraData = null }, /*minecraft:stone_slab2*/
            new Item(158, 0, 1){ RuntimeId=5270, NetworkId=163, ExtraData = null }, /*minecraft:wooden_slab*/
            new Item(158, 0, 1){ RuntimeId=5271, NetworkId=164, ExtraData = null }, /*minecraft:wooden_slab*/
            new Item(158, 0, 1){ RuntimeId=5272, NetworkId=165, ExtraData = null }, /*minecraft:wooden_slab*/
            new Item(158, 0, 1){ RuntimeId=5273, NetworkId=166, ExtraData = null }, /*minecraft:wooden_slab*/
            new Item(158, 0, 1){ RuntimeId=5274, NetworkId=167, ExtraData = null }, /*minecraft:wooden_slab*/
            new Item(158, 0, 1){ RuntimeId=5275, NetworkId=168, ExtraData = null }, /*minecraft:wooden_slab*/
            new Item(-489, 0, 1){ RuntimeId=1149, NetworkId=169, ExtraData = null }, /**/
            new Item(44, 0, 1){ RuntimeId=4275, NetworkId=170, ExtraData = null }, /*minecraft:stone_slab*/
            new Item(-166, 0, 1){ RuntimeId=5820, NetworkId=171, ExtraData = null }, /*minecraft:stone_slab4*/
            new Item(44, 0, 1){ RuntimeId=4271, NetworkId=172, ExtraData = null }, /*minecraft:stone_slab*/
            new Item(-166, 0, 1){ RuntimeId=5823, NetworkId=173, ExtraData = null }, /*minecraft:stone_slab4*/
            new Item(182, 0, 1){ RuntimeId=5794, NetworkId=174, ExtraData = null }, /*minecraft:stone_slab2*/
            new Item(182, 0, 1){ RuntimeId=5788, NetworkId=175, ExtraData = null }, /*minecraft:stone_slab2*/
            new Item(-166, 0, 1){ RuntimeId=5824, NetworkId=176, ExtraData = null }, /*minecraft:stone_slab4*/
            new Item(-162, 0, 1){ RuntimeId=5805, NetworkId=177, ExtraData = null }, /*minecraft:stone_slab3*/
            new Item(-162, 0, 1){ RuntimeId=5810, NetworkId=178, ExtraData = null }, /*minecraft:stone_slab3*/
            new Item(-162, 0, 1){ RuntimeId=5811, NetworkId=179, ExtraData = null }, /*minecraft:stone_slab3*/
            new Item(-162, 0, 1){ RuntimeId=5808, NetworkId=180, ExtraData = null }, /*minecraft:stone_slab3*/
            new Item(-162, 0, 1){ RuntimeId=5809, NetworkId=181, ExtraData = null }, /*minecraft:stone_slab3*/
            new Item(-162, 0, 1){ RuntimeId=5807, NetworkId=182, ExtraData = null }, /*minecraft:stone_slab3*/
            new Item(-162, 0, 1){ RuntimeId=5806, NetworkId=183, ExtraData = null }, /*minecraft:stone_slab3*/
            new Item(44, 0, 1){ RuntimeId=4274, NetworkId=184, ExtraData = null }, /*minecraft:stone_slab*/
            new Item(44, 0, 1){ RuntimeId=4277, NetworkId=185, ExtraData = null }, /*minecraft:stone_slab*/
            new Item(182, 0, 1){ RuntimeId=5795, NetworkId=186, ExtraData = null }, /*minecraft:stone_slab2*/
            new Item(-162, 0, 1){ RuntimeId=5804, NetworkId=187, ExtraData = null }, /*minecraft:stone_slab3*/
            new Item(44, 0, 1){ RuntimeId=4276, NetworkId=188, ExtraData = null }, /*minecraft:stone_slab*/
            new Item(-166, 0, 1){ RuntimeId=5821, NetworkId=189, ExtraData = null }, /*minecraft:stone_slab4*/
            new Item(182, 0, 1){ RuntimeId=5789, NetworkId=190, ExtraData = null }, /*minecraft:stone_slab2*/
            new Item(182, 0, 1){ RuntimeId=5790, NetworkId=191, ExtraData = null }, /*minecraft:stone_slab2*/
            new Item(182, 0, 1){ RuntimeId=5791, NetworkId=192, ExtraData = null }, /*minecraft:stone_slab2*/
            new Item(182, 0, 1){ RuntimeId=5792, NetworkId=193, ExtraData = null }, /*minecraft:stone_slab2*/
            new Item(-264, 0, 1){ RuntimeId=5900, NetworkId=194, ExtraData = null }, /*minecraft:crimson_slab*/
            new Item(-265, 0, 1){ RuntimeId=6484, NetworkId=195, ExtraData = null }, /*minecraft:warped_slab*/
            new Item(-282, 0, 1){ RuntimeId=910, NetworkId=196, ExtraData = null }, /*minecraft:blackstone_slab*/
            new Item(-293, 0, 1){ RuntimeId=6018, NetworkId=197, ExtraData = null }, /*minecraft:polished_blackstone_slab*/
            new Item(-284, 0, 1){ RuntimeId=4192, NetworkId=198, ExtraData = null }, /*minecraft:polished_blackstone_brick_slab*/
            new Item(-361, 0, 1){ RuntimeId=5235, NetworkId=199, ExtraData = null }, /**/
            new Item(-362, 0, 1){ RuntimeId=6600, NetworkId=200, ExtraData = null }, /**/
            new Item(-363, 0, 1){ RuntimeId=6053, NetworkId=201, ExtraData = null }, /**/
            new Item(-364, 0, 1){ RuntimeId=5282, NetworkId=202, ExtraData = null }, /**/
            new Item(-365, 0, 1){ RuntimeId=7815, NetworkId=203, ExtraData = null }, /**/
            new Item(-366, 0, 1){ RuntimeId=249, NetworkId=204, ExtraData = null }, /**/
            new Item(-367, 0, 1){ RuntimeId=6545, NetworkId=205, ExtraData = null }, /**/
            new Item(-449, 0, 1){ RuntimeId=708, NetworkId=206, ExtraData = null }, /**/
            new Item(-380, 0, 1){ RuntimeId=7310, NetworkId=207, ExtraData = null }, /**/
            new Item(-384, 0, 1){ RuntimeId=288, NetworkId=208, ExtraData = null }, /**/
            new Item(-388, 0, 1){ RuntimeId=4291, NetworkId=209, ExtraData = null }, /**/
            new Item(-392, 0, 1){ RuntimeId=3716, NetworkId=210, ExtraData = null }, /**/
            new Item(-478, 0, 1){ RuntimeId=3910, NetworkId=211, ExtraData = null }, /**/
            new Item(45, 0, 1){ RuntimeId=4765, NetworkId=212, ExtraData = null }, /*minecraft:brick_block*/
            new Item(-302, 0, 1){ RuntimeId=7249, NetworkId=213, ExtraData = null }, /*minecraft:chiseled_nether_bricks*/
            new Item(-303, 0, 1){ RuntimeId=4552, NetworkId=214, ExtraData = null }, /*minecraft:cracked_nether_bricks*/
            new Item(-304, 0, 1){ RuntimeId=6351, NetworkId=215, ExtraData = null }, /*minecraft:quartz_bricks*/
            new Item(98, 0, 1){ RuntimeId=6547, NetworkId=216, ExtraData = null }, /*minecraft:stonebrick*/
            new Item(98, 0, 1){ RuntimeId=6548, NetworkId=217, ExtraData = null }, /*minecraft:stonebrick*/
            new Item(98, 0, 1){ RuntimeId=6549, NetworkId=218, ExtraData = null }, /*minecraft:stonebrick*/
            new Item(98, 0, 1){ RuntimeId=6550, NetworkId=219, ExtraData = null }, /*minecraft:stonebrick*/
            new Item(206, 0, 1){ RuntimeId=281, NetworkId=220, ExtraData = null }, /*minecraft:end_bricks*/
            new Item(168, 0, 1){ RuntimeId=6087, NetworkId=221, ExtraData = null }, /*minecraft:prismarine*/
            new Item(-274, 0, 1){ RuntimeId=4680, NetworkId=222, ExtraData = null }, /*minecraft:polished_blackstone_bricks*/
            new Item(-280, 0, 1){ RuntimeId=7214, NetworkId=223, ExtraData = null }, /*minecraft:cracked_polished_blackstone_bricks*/
            new Item(-281, 0, 1){ RuntimeId=4586, NetworkId=224, ExtraData = null }, /*minecraft:gilded_blackstone*/
            new Item(-279, 0, 1){ RuntimeId=5062, NetworkId=225, ExtraData = null }, /*minecraft:chiseled_polished_blackstone*/
            new Item(-387, 0, 1){ RuntimeId=4581, NetworkId=226, ExtraData = null }, /**/
            new Item(-409, 0, 1){ RuntimeId=4160, NetworkId=227, ExtraData = null }, /**/
            new Item(-391, 0, 1){ RuntimeId=5464, NetworkId=228, ExtraData = null }, /**/
            new Item(-410, 0, 1){ RuntimeId=5364, NetworkId=229, ExtraData = null }, /**/
            new Item(-395, 0, 1){ RuntimeId=5234, NetworkId=230, ExtraData = null }, /**/
            new Item(4, 0, 1){ RuntimeId=3615, NetworkId=231, ExtraData = null }, /*minecraft:cobblestone*/
            new Item(48, 0, 1){ RuntimeId=252, NetworkId=232, ExtraData = null }, /*minecraft:mossy_cobblestone*/
            new Item(-379, 0, 1){ RuntimeId=6670, NetworkId=233, ExtraData = null }, /**/
            new Item(-183, 0, 1){ RuntimeId=4582, NetworkId=234, ExtraData = null }, /*minecraft:smooth_stone*/
            new Item(24, 0, 1){ RuntimeId=3653, NetworkId=235, ExtraData = null }, /*minecraft:sandstone*/
            new Item(24, 0, 1){ RuntimeId=3654, NetworkId=236, ExtraData = null }, /*minecraft:sandstone*/
            new Item(24, 0, 1){ RuntimeId=3655, NetworkId=237, ExtraData = null }, /*minecraft:sandstone*/
            new Item(24, 0, 1){ RuntimeId=3656, NetworkId=238, ExtraData = null }, /*minecraft:sandstone*/
            new Item(179, 0, 1){ RuntimeId=6580, NetworkId=239, ExtraData = null }, /*minecraft:red_sandstone*/
            new Item(179, 0, 1){ RuntimeId=6581, NetworkId=240, ExtraData = null }, /*minecraft:red_sandstone*/
            new Item(179, 0, 1){ RuntimeId=6582, NetworkId=241, ExtraData = null }, /*minecraft:red_sandstone*/
            new Item(179, 0, 1){ RuntimeId=6583, NetworkId=242, ExtraData = null }, /*minecraft:red_sandstone*/
            new Item(173, 0, 1){ RuntimeId=5398, NetworkId=243, ExtraData = null }, /*minecraft:coal_block*/
            new Item(-139, 0, 1){ RuntimeId=7979, NetworkId=244, ExtraData = null }, /*minecraft:dried_kelp_block*/
            new Item(41, 0, 1){ RuntimeId=291, NetworkId=245, ExtraData = null }, /*minecraft:gold_block*/
            new Item(42, 0, 1){ RuntimeId=8261, NetworkId=246, ExtraData = null }, /*minecraft:iron_block*/
            new Item(-340, 0, 1){ RuntimeId=4651, NetworkId=247, ExtraData = null }, /**/
            new Item(-341, 0, 1){ RuntimeId=593, NetworkId=248, ExtraData = null }, /**/
            new Item(-342, 0, 1){ RuntimeId=8246, NetworkId=249, ExtraData = null }, /**/
            new Item(-343, 0, 1){ RuntimeId=3553, NetworkId=250, ExtraData = null }, /**/
            new Item(-344, 0, 1){ RuntimeId=7734, NetworkId=251, ExtraData = null }, /**/
            new Item(-345, 0, 1){ RuntimeId=694, NetworkId=252, ExtraData = null }, /**/
            new Item(-346, 0, 1){ RuntimeId=707, NetworkId=253, ExtraData = null }, /**/
            new Item(-446, 0, 1){ RuntimeId=7542, NetworkId=254, ExtraData = null }, /**/
            new Item(-347, 0, 1){ RuntimeId=4689, NetworkId=255, ExtraData = null }, /**/
            new Item(-348, 0, 1){ RuntimeId=6166, NetworkId=256, ExtraData = null }, /**/
            new Item(-349, 0, 1){ RuntimeId=7197, NetworkId=257, ExtraData = null }, /**/
            new Item(-350, 0, 1){ RuntimeId=5478, NetworkId=258, ExtraData = null }, /**/
            new Item(-351, 0, 1){ RuntimeId=7293, NetworkId=259, ExtraData = null }, /**/
            new Item(-352, 0, 1){ RuntimeId=3809, NetworkId=260, ExtraData = null }, /**/
            new Item(-353, 0, 1){ RuntimeId=4851, NetworkId=261, ExtraData = null }, /**/
            new Item(-447, 0, 1){ RuntimeId=214, NetworkId=262, ExtraData = null }, /**/
            new Item(133, 0, 1){ RuntimeId=1159, NetworkId=263, ExtraData = null }, /*minecraft:emerald_block*/
            new Item(57, 0, 1){ RuntimeId=272, NetworkId=264, ExtraData = null }, /*minecraft:diamond_block*/
            new Item(22, 0, 1){ RuntimeId=4286, NetworkId=265, ExtraData = null }, /*minecraft:lapis_block*/
            new Item(-451, 0, 1){ RuntimeId=8260, NetworkId=266, ExtraData = null }, /**/
            new Item(-452, 0, 1){ RuntimeId=5269, NetworkId=267, ExtraData = null }, /**/
            new Item(-453, 0, 1){ RuntimeId=361, NetworkId=268, ExtraData = null }, /**/
            new Item(155, 0, 1){ RuntimeId=3696, NetworkId=269, ExtraData = null }, /*minecraft:quartz_block*/
            new Item(155, 0, 1){ RuntimeId=3698, NetworkId=270, ExtraData = null }, /*minecraft:quartz_block*/
            new Item(155, 0, 1){ RuntimeId=3697, NetworkId=271, ExtraData = null }, /*minecraft:quartz_block*/
            new Item(155, 0, 1){ RuntimeId=3699, NetworkId=272, ExtraData = null }, /*minecraft:quartz_block*/
            new Item(168, 0, 1){ RuntimeId=6085, NetworkId=273, ExtraData = null }, /*minecraft:prismarine*/
            new Item(168, 0, 1){ RuntimeId=6086, NetworkId=274, ExtraData = null }, /*minecraft:prismarine*/
            new Item(165, 0, 1){ RuntimeId=4233, NetworkId=275, ExtraData = null }, /*minecraft:slime*/
            new Item(-220, 0, 1){ RuntimeId=892, NetworkId=276, ExtraData = null }, /*minecraft:honey_block*/
            new Item(-221, 0, 1){ RuntimeId=4476, NetworkId=277, ExtraData = null }, /*minecraft:honeycomb_block*/
            new Item(170, 0, 1){ RuntimeId=695, NetworkId=278, ExtraData = null }, /*minecraft:hay_block*/
            new Item(216, 0, 1){ RuntimeId=4234, NetworkId=279, ExtraData = null }, /*minecraft:bone_block*/
            new Item(112, 0, 1){ RuntimeId=7272, NetworkId=280, ExtraData = null }, /*minecraft:nether_brick*/
            new Item(215, 0, 1){ RuntimeId=146, NetworkId=281, ExtraData = null }, /*minecraft:red_nether_brick*/
            new Item(-270, 0, 1){ RuntimeId=3775, NetworkId=282, ExtraData = null }, /*minecraft:netherite_block*/
            new Item(-222, 0, 1){ RuntimeId=8259, NetworkId=283, ExtraData = null }, /*minecraft:lodestone*/
            new Item(35, 0, 1){ RuntimeId=3458, NetworkId=284, ExtraData = null }, /*minecraft:wool*/
            new Item(35, 0, 1){ RuntimeId=3466, NetworkId=285, ExtraData = null }, /*minecraft:wool*/
            new Item(35, 0, 1){ RuntimeId=3465, NetworkId=286, ExtraData = null }, /*minecraft:wool*/
            new Item(35, 0, 1){ RuntimeId=3473, NetworkId=287, ExtraData = null }, /*minecraft:wool*/
            new Item(35, 0, 1){ RuntimeId=3470, NetworkId=288, ExtraData = null }, /*minecraft:wool*/
            new Item(35, 0, 1){ RuntimeId=3472, NetworkId=289, ExtraData = null }, /*minecraft:wool*/
            new Item(35, 0, 1){ RuntimeId=3459, NetworkId=290, ExtraData = null }, /*minecraft:wool*/
            new Item(35, 0, 1){ RuntimeId=3462, NetworkId=291, ExtraData = null }, /*minecraft:wool*/
            new Item(35, 0, 1){ RuntimeId=3463, NetworkId=292, ExtraData = null }, /*minecraft:wool*/
            new Item(35, 0, 1){ RuntimeId=3471, NetworkId=293, ExtraData = null }, /*minecraft:wool*/
            new Item(35, 0, 1){ RuntimeId=3467, NetworkId=294, ExtraData = null }, /*minecraft:wool*/
            new Item(35, 0, 1){ RuntimeId=3461, NetworkId=295, ExtraData = null }, /*minecraft:wool*/
            new Item(35, 0, 1){ RuntimeId=3469, NetworkId=296, ExtraData = null }, /*minecraft:wool*/
            new Item(35, 0, 1){ RuntimeId=3468, NetworkId=297, ExtraData = null }, /*minecraft:wool*/
            new Item(35, 0, 1){ RuntimeId=3460, NetworkId=298, ExtraData = null }, /*minecraft:wool*/
            new Item(35, 0, 1){ RuntimeId=3464, NetworkId=299, ExtraData = null }, /*minecraft:wool*/
            new Item(171, 0, 1){ RuntimeId=949, NetworkId=300, ExtraData = null }, /*minecraft:carpet*/
            new Item(171, 0, 1){ RuntimeId=957, NetworkId=301, ExtraData = null }, /*minecraft:carpet*/
            new Item(171, 0, 1){ RuntimeId=956, NetworkId=302, ExtraData = null }, /*minecraft:carpet*/
            new Item(171, 0, 1){ RuntimeId=964, NetworkId=303, ExtraData = null }, /*minecraft:carpet*/
            new Item(171, 0, 1){ RuntimeId=961, NetworkId=304, ExtraData = null }, /*minecraft:carpet*/
            new Item(171, 0, 1){ RuntimeId=963, NetworkId=305, ExtraData = null }, /*minecraft:carpet*/
            new Item(171, 0, 1){ RuntimeId=950, NetworkId=306, ExtraData = null }, /*minecraft:carpet*/
            new Item(171, 0, 1){ RuntimeId=953, NetworkId=307, ExtraData = null }, /*minecraft:carpet*/
            new Item(171, 0, 1){ RuntimeId=954, NetworkId=308, ExtraData = null }, /*minecraft:carpet*/
            new Item(171, 0, 1){ RuntimeId=962, NetworkId=309, ExtraData = null }, /*minecraft:carpet*/
            new Item(171, 0, 1){ RuntimeId=958, NetworkId=310, ExtraData = null }, /*minecraft:carpet*/
            new Item(171, 0, 1){ RuntimeId=952, NetworkId=311, ExtraData = null }, /*minecraft:carpet*/
            new Item(171, 0, 1){ RuntimeId=960, NetworkId=312, ExtraData = null }, /*minecraft:carpet*/
            new Item(171, 0, 1){ RuntimeId=959, NetworkId=313, ExtraData = null }, /*minecraft:carpet*/
            new Item(171, 0, 1){ RuntimeId=951, NetworkId=314, ExtraData = null }, /*minecraft:carpet*/
            new Item(171, 0, 1){ RuntimeId=955, NetworkId=315, ExtraData = null }, /*minecraft:carpet*/
            new Item(237, 0, 1){ RuntimeId=6264, NetworkId=316, ExtraData = null }, /*minecraft:concretePowder*/
            new Item(237, 0, 1){ RuntimeId=6272, NetworkId=317, ExtraData = null }, /*minecraft:concretePowder*/
            new Item(237, 0, 1){ RuntimeId=6271, NetworkId=318, ExtraData = null }, /*minecraft:concretePowder*/
            new Item(237, 0, 1){ RuntimeId=6279, NetworkId=319, ExtraData = null }, /*minecraft:concretePowder*/
            new Item(237, 0, 1){ RuntimeId=6276, NetworkId=320, ExtraData = null }, /*minecraft:concretePowder*/
            new Item(237, 0, 1){ RuntimeId=6278, NetworkId=321, ExtraData = null }, /*minecraft:concretePowder*/
            new Item(237, 0, 1){ RuntimeId=6265, NetworkId=322, ExtraData = null }, /*minecraft:concretePowder*/
            new Item(237, 0, 1){ RuntimeId=6268, NetworkId=323, ExtraData = null }, /*minecraft:concretePowder*/
            new Item(237, 0, 1){ RuntimeId=6269, NetworkId=324, ExtraData = null }, /*minecraft:concretePowder*/
            new Item(237, 0, 1){ RuntimeId=6277, NetworkId=325, ExtraData = null }, /*minecraft:concretePowder*/
            new Item(237, 0, 1){ RuntimeId=6273, NetworkId=326, ExtraData = null }, /*minecraft:concretePowder*/
            new Item(237, 0, 1){ RuntimeId=6267, NetworkId=327, ExtraData = null }, /*minecraft:concretePowder*/
            new Item(237, 0, 1){ RuntimeId=6275, NetworkId=328, ExtraData = null }, /*minecraft:concretePowder*/
            new Item(237, 0, 1){ RuntimeId=6274, NetworkId=329, ExtraData = null }, /*minecraft:concretePowder*/
            new Item(237, 0, 1){ RuntimeId=6266, NetworkId=330, ExtraData = null }, /*minecraft:concretePowder*/
            new Item(237, 0, 1){ RuntimeId=6270, NetworkId=331, ExtraData = null }, /*minecraft:concretePowder*/
            new Item(236, 0, 1){ RuntimeId=660, NetworkId=332, ExtraData = null }, /*minecraft:concrete*/
            new Item(236, 0, 1){ RuntimeId=668, NetworkId=333, ExtraData = null }, /*minecraft:concrete*/
            new Item(236, 0, 1){ RuntimeId=667, NetworkId=334, ExtraData = null }, /*minecraft:concrete*/
            new Item(236, 0, 1){ RuntimeId=675, NetworkId=335, ExtraData = null }, /*minecraft:concrete*/
            new Item(236, 0, 1){ RuntimeId=672, NetworkId=336, ExtraData = null }, /*minecraft:concrete*/
            new Item(236, 0, 1){ RuntimeId=674, NetworkId=337, ExtraData = null }, /*minecraft:concrete*/
            new Item(236, 0, 1){ RuntimeId=661, NetworkId=338, ExtraData = null }, /*minecraft:concrete*/
            new Item(236, 0, 1){ RuntimeId=664, NetworkId=339, ExtraData = null }, /*minecraft:concrete*/
            new Item(236, 0, 1){ RuntimeId=665, NetworkId=340, ExtraData = null }, /*minecraft:concrete*/
            new Item(236, 0, 1){ RuntimeId=673, NetworkId=341, ExtraData = null }, /*minecraft:concrete*/
            new Item(236, 0, 1){ RuntimeId=669, NetworkId=342, ExtraData = null }, /*minecraft:concrete*/
            new Item(236, 0, 1){ RuntimeId=663, NetworkId=343, ExtraData = null }, /*minecraft:concrete*/
            new Item(236, 0, 1){ RuntimeId=671, NetworkId=344, ExtraData = null }, /*minecraft:concrete*/
            new Item(236, 0, 1){ RuntimeId=670, NetworkId=345, ExtraData = null }, /*minecraft:concrete*/
            new Item(236, 0, 1){ RuntimeId=662, NetworkId=346, ExtraData = null }, /*minecraft:concrete*/
            new Item(236, 0, 1){ RuntimeId=666, NetworkId=347, ExtraData = null }, /*minecraft:concrete*/
            new Item(82, 0, 1){ RuntimeId=7124, NetworkId=348, ExtraData = null }, /*minecraft:clay*/
            new Item(172, 0, 1){ RuntimeId=641, NetworkId=349, ExtraData = null }, /*minecraft:hardened_clay*/
            new Item(159, 0, 1){ RuntimeId=6176, NetworkId=350, ExtraData = null }, /*minecraft:stained_hardened_clay*/
            new Item(159, 0, 1){ RuntimeId=6184, NetworkId=351, ExtraData = null }, /*minecraft:stained_hardened_clay*/
            new Item(159, 0, 1){ RuntimeId=6183, NetworkId=352, ExtraData = null }, /*minecraft:stained_hardened_clay*/
            new Item(159, 0, 1){ RuntimeId=6191, NetworkId=353, ExtraData = null }, /*minecraft:stained_hardened_clay*/
            new Item(159, 0, 1){ RuntimeId=6188, NetworkId=354, ExtraData = null }, /*minecraft:stained_hardened_clay*/
            new Item(159, 0, 1){ RuntimeId=6190, NetworkId=355, ExtraData = null }, /*minecraft:stained_hardened_clay*/
            new Item(159, 0, 1){ RuntimeId=6177, NetworkId=356, ExtraData = null }, /*minecraft:stained_hardened_clay*/
            new Item(159, 0, 1){ RuntimeId=6180, NetworkId=357, ExtraData = null }, /*minecraft:stained_hardened_clay*/
            new Item(159, 0, 1){ RuntimeId=6181, NetworkId=358, ExtraData = null }, /*minecraft:stained_hardened_clay*/
            new Item(159, 0, 1){ RuntimeId=6189, NetworkId=359, ExtraData = null }, /*minecraft:stained_hardened_clay*/
            new Item(159, 0, 1){ RuntimeId=6185, NetworkId=360, ExtraData = null }, /*minecraft:stained_hardened_clay*/
            new Item(159, 0, 1){ RuntimeId=6179, NetworkId=361, ExtraData = null }, /*minecraft:stained_hardened_clay*/
            new Item(159, 0, 1){ RuntimeId=6187, NetworkId=362, ExtraData = null }, /*minecraft:stained_hardened_clay*/
            new Item(159, 0, 1){ RuntimeId=6186, NetworkId=363, ExtraData = null }, /*minecraft:stained_hardened_clay*/
            new Item(159, 0, 1){ RuntimeId=6178, NetworkId=364, ExtraData = null }, /*minecraft:stained_hardened_clay*/
            new Item(159, 0, 1){ RuntimeId=6182, NetworkId=365, ExtraData = null }, /*minecraft:stained_hardened_clay*/
            new Item(220, 0, 1){ RuntimeId=5573, NetworkId=366, ExtraData = null }, /*minecraft:white_glazed_terracotta*/
            new Item(228, 0, 1){ RuntimeId=3531, NetworkId=367, ExtraData = null }, /*minecraft:silver_glazed_terracotta*/
            new Item(227, 0, 1){ RuntimeId=8253, NetworkId=368, ExtraData = null }, /*minecraft:gray_glazed_terracotta*/
            new Item(235, 0, 1){ RuntimeId=5834, NetworkId=369, ExtraData = null }, /*minecraft:black_glazed_terracotta*/
            new Item(232, 0, 1){ RuntimeId=3547, NetworkId=370, ExtraData = null }, /*minecraft:brown_glazed_terracotta*/
            new Item(234, 0, 1){ RuntimeId=4167, NetworkId=371, ExtraData = null }, /*minecraft:red_glazed_terracotta*/
            new Item(221, 0, 1){ RuntimeId=1151, NetworkId=372, ExtraData = null }, /*minecraft:orange_glazed_terracotta*/
            new Item(224, 0, 1){ RuntimeId=913, NetworkId=373, ExtraData = null }, /*minecraft:yellow_glazed_terracotta*/
            new Item(225, 0, 1){ RuntimeId=223, NetworkId=374, ExtraData = null }, /*minecraft:lime_glazed_terracotta*/
            new Item(233, 0, 1){ RuntimeId=6610, NetworkId=375, ExtraData = null }, /*minecraft:green_glazed_terracotta*/
            new Item(229, 0, 1){ RuntimeId=5358, NetworkId=376, ExtraData = null }, /*minecraft:cyan_glazed_terracotta*/
            new Item(223, 0, 1){ RuntimeId=5471, NetworkId=377, ExtraData = null }, /*minecraft:light_blue_glazed_terracotta*/
            new Item(231, 0, 1){ RuntimeId=5465, NetworkId=378, ExtraData = null }, /*minecraft:blue_glazed_terracotta*/
            new Item(219, 0, 1){ RuntimeId=7011, NetworkId=379, ExtraData = null }, /*minecraft:purple_glazed_terracotta*/
            new Item(222, 0, 1){ RuntimeId=965, NetworkId=380, ExtraData = null }, /*minecraft:magenta_glazed_terracotta*/
            new Item(226, 0, 1){ RuntimeId=6539, NetworkId=381, ExtraData = null }, /*minecraft:pink_glazed_terracotta*/
            new Item(201, 0, 1){ RuntimeId=7714, NetworkId=382, ExtraData = null }, /*minecraft:purpur_block*/
            new Item(201, 0, 1){ RuntimeId=7716, NetworkId=383, ExtraData = null }, /*minecraft:purpur_block*/
            new Item(-477, 0, 1){ RuntimeId=283, NetworkId=384, ExtraData = null }, /**/
            new Item(-475, 0, 1){ RuntimeId=6889, NetworkId=385, ExtraData = null }, /**/
            new Item(214, 0, 1){ RuntimeId=4293, NetworkId=386, ExtraData = null }, /*minecraft:nether_wart_block*/
            new Item(-227, 0, 1){ RuntimeId=5905, NetworkId=387, ExtraData = null }, /*minecraft:warped_wart_block*/
            new Item(-230, 0, 1){ RuntimeId=5061, NetworkId=388, ExtraData = null }, /*minecraft:shroomlight*/
            new Item(-232, 0, 1){ RuntimeId=4189, NetworkId=389, ExtraData = null }, /*minecraft:crimson_nylium*/
            new Item(-233, 0, 1){ RuntimeId=6349, NetworkId=390, ExtraData = null }, /*minecraft:warped_nylium*/
            new Item(-234, 0, 1){ RuntimeId=4349, NetworkId=391, ExtraData = null }, /*minecraft:basalt*/
            new Item(-235, 0, 1){ RuntimeId=24, NetworkId=392, ExtraData = null }, /*minecraft:polished_basalt*/
            new Item(-377, 0, 1){ RuntimeId=1157, NetworkId=393, ExtraData = null }, /**/
            new Item(-236, 0, 1){ RuntimeId=5830, NetworkId=394, ExtraData = null }, /*minecraft:soul_soil*/
            new Item(3, 0, 1){ RuntimeId=5751, NetworkId=395, ExtraData = null }, /*minecraft:dirt*/
            new Item(3, 0, 1){ RuntimeId=5752, NetworkId=396, ExtraData = null }, /*minecraft:dirt*/
            new Item(60, 0, 1){ RuntimeId=3912, NetworkId=397, ExtraData = null }, /*minecraft:farmland*/
            new Item(2, 0, 1){ RuntimeId=6975, NetworkId=398, ExtraData = null }, /*minecraft:grass*/
            new Item(198, 0, 1){ RuntimeId=8081, NetworkId=399, ExtraData = null }, /*minecraft:grass_path*/
            new Item(243, 0, 1){ RuntimeId=4650, NetworkId=400, ExtraData = null }, /*minecraft:podzol*/
            new Item(110, 0, 1){ RuntimeId=3683, NetworkId=401, ExtraData = null }, /*minecraft:mycelium*/
            new Item(-473, 0, 1){ RuntimeId=6684, NetworkId=402, ExtraData = null }, /**/
            new Item(1, 0, 1){ RuntimeId=653, NetworkId=403, ExtraData = null }, /*minecraft:stone*/
            new Item(15, 0, 1){ RuntimeId=4690, NetworkId=404, ExtraData = null }, /*minecraft:iron_ore*/
            new Item(14, 0, 1){ RuntimeId=912, NetworkId=405, ExtraData = null }, /*minecraft:gold_ore*/
            new Item(56, 0, 1){ RuntimeId=4361, NetworkId=406, ExtraData = null }, /*minecraft:diamond_ore*/
            new Item(21, 0, 1){ RuntimeId=7699, NetworkId=407, ExtraData = null }, /*minecraft:lapis_ore*/
            new Item(73, 0, 1){ RuntimeId=4289, NetworkId=408, ExtraData = null }, /*minecraft:redstone_ore*/
            new Item(16, 0, 1){ RuntimeId=4287, NetworkId=409, ExtraData = null }, /*minecraft:coal_ore*/
            new Item(-311, 0, 1){ RuntimeId=3554, NetworkId=410, ExtraData = null }, /**/
            new Item(129, 0, 1){ RuntimeId=7347, NetworkId=411, ExtraData = null }, /*minecraft:emerald_ore*/
            new Item(153, 0, 1){ RuntimeId=4501, NetworkId=412, ExtraData = null }, /*minecraft:quartz_ore*/
            new Item(-288, 0, 1){ RuntimeId=27, NetworkId=413, ExtraData = null }, /*minecraft:nether_gold_ore*/
            new Item(-271, 0, 1){ RuntimeId=6107, NetworkId=414, ExtraData = null }, /*minecraft:ancient_debris*/
            new Item(-401, 0, 1){ RuntimeId=7273, NetworkId=415, ExtraData = null }, /**/
            new Item(-402, 0, 1){ RuntimeId=6106, NetworkId=416, ExtraData = null }, /**/
            new Item(-405, 0, 1){ RuntimeId=8038, NetworkId=417, ExtraData = null }, /**/
            new Item(-400, 0, 1){ RuntimeId=7262, NetworkId=418, ExtraData = null }, /**/
            new Item(-403, 0, 1){ RuntimeId=6616, NetworkId=419, ExtraData = null }, /**/
            new Item(-407, 0, 1){ RuntimeId=6350, NetworkId=420, ExtraData = null }, /**/
            new Item(-406, 0, 1){ RuntimeId=7196, NetworkId=421, ExtraData = null }, /**/
            new Item(-408, 0, 1){ RuntimeId=105, NetworkId=422, ExtraData = null }, /**/
            new Item(13, 0, 1){ RuntimeId=8287, NetworkId=423, ExtraData = null }, /*minecraft:gravel*/
            new Item(1, 0, 1){ RuntimeId=654, NetworkId=424, ExtraData = null }, /*minecraft:stone*/
            new Item(1, 0, 1){ RuntimeId=656, NetworkId=425, ExtraData = null }, /*minecraft:stone*/
            new Item(1, 0, 1){ RuntimeId=658, NetworkId=426, ExtraData = null }, /*minecraft:stone*/
            new Item(-273, 0, 1){ RuntimeId=7585, NetworkId=427, ExtraData = null }, /*minecraft:blackstone*/
            new Item(-378, 0, 1){ RuntimeId=253, NetworkId=428, ExtraData = null }, /**/
            new Item(1, 0, 1){ RuntimeId=655, NetworkId=429, ExtraData = null }, /*minecraft:stone*/
            new Item(1, 0, 1){ RuntimeId=657, NetworkId=430, ExtraData = null }, /*minecraft:stone*/
            new Item(1, 0, 1){ RuntimeId=659, NetworkId=431, ExtraData = null }, /*minecraft:stone*/
            new Item(-291, 0, 1){ RuntimeId=3682, NetworkId=432, ExtraData = null }, /*minecraft:polished_blackstone*/
            new Item(-383, 0, 1){ RuntimeId=7754, NetworkId=433, ExtraData = null }, /**/
            new Item(12, 0, 1){ RuntimeId=4195, NetworkId=434, ExtraData = null }, /*minecraft:sand*/
            new Item(12, 0, 1){ RuntimeId=4196, NetworkId=435, ExtraData = null }, /*minecraft:sand*/
            new Item(81, 0, 1){ RuntimeId=6986, NetworkId=436, ExtraData = null }, /*minecraft:cactus*/
            new Item(17, 0, 1){ RuntimeId=6672, NetworkId=437, ExtraData = null }, /*minecraft:log*/
            new Item(-10, 0, 1){ RuntimeId=7543, NetworkId=438, ExtraData = null }, /*minecraft:stripped_oak_log*/
            new Item(17, 0, 1){ RuntimeId=6673, NetworkId=439, ExtraData = null }, /*minecraft:log*/
            new Item(-5, 0, 1){ RuntimeId=6288, NetworkId=440, ExtraData = null }, /*minecraft:stripped_spruce_log*/
            new Item(17, 0, 1){ RuntimeId=6674, NetworkId=441, ExtraData = null }, /*minecraft:log*/
            new Item(-6, 0, 1){ RuntimeId=5972, NetworkId=442, ExtraData = null }, /*minecraft:stripped_birch_log*/
            new Item(17, 0, 1){ RuntimeId=6675, NetworkId=443, ExtraData = null }, /*minecraft:log*/
            new Item(-7, 0, 1){ RuntimeId=642, NetworkId=444, ExtraData = null }, /*minecraft:stripped_jungle_log*/
            new Item(162, 0, 1){ RuntimeId=3830, NetworkId=445, ExtraData = null }, /*minecraft:log2*/
            new Item(-8, 0, 1){ RuntimeId=5848, NetworkId=446, ExtraData = null }, /*minecraft:stripped_acacia_log*/
            new Item(162, 0, 1){ RuntimeId=3831, NetworkId=447, ExtraData = null }, /*minecraft:log2*/
            new Item(-9, 0, 1){ RuntimeId=216, NetworkId=448, ExtraData = null }, /*minecraft:stripped_dark_oak_log*/
            new Item(-484, 0, 1){ RuntimeId=348, NetworkId=449, ExtraData = null }, /**/
            new Item(-485, 0, 1){ RuntimeId=8284, NetworkId=450, ExtraData = null }, /**/
            new Item(-225, 0, 1){ RuntimeId=5897, NetworkId=451, ExtraData = null }, /*minecraft:crimson_stem*/
            new Item(-240, 0, 1){ RuntimeId=6948, NetworkId=452, ExtraData = null }, /*minecraft:stripped_crimson_stem*/
            new Item(-226, 0, 1){ RuntimeId=6486, NetworkId=453, ExtraData = null }, /*minecraft:warped_stem*/
            new Item(-241, 0, 1){ RuntimeId=7400, NetworkId=454, ExtraData = null }, /*minecraft:stripped_warped_stem*/
            new Item(-212, 0, 1){ RuntimeId=3474, NetworkId=455, ExtraData = null }, /*minecraft:wood*/
            new Item(-212, 0, 1){ RuntimeId=3480, NetworkId=456, ExtraData = null }, /*minecraft:wood*/
            new Item(-212, 0, 1){ RuntimeId=3475, NetworkId=457, ExtraData = null }, /*minecraft:wood*/
            new Item(-212, 0, 1){ RuntimeId=3481, NetworkId=458, ExtraData = null }, /*minecraft:wood*/
            new Item(-212, 0, 1){ RuntimeId=3476, NetworkId=459, ExtraData = null }, /*minecraft:wood*/
            new Item(-212, 0, 1){ RuntimeId=3482, NetworkId=460, ExtraData = null }, /*minecraft:wood*/
            new Item(-212, 0, 1){ RuntimeId=3477, NetworkId=461, ExtraData = null }, /*minecraft:wood*/
            new Item(-212, 0, 1){ RuntimeId=3483, NetworkId=462, ExtraData = null }, /*minecraft:wood*/
            new Item(-212, 0, 1){ RuntimeId=3478, NetworkId=463, ExtraData = null }, /*minecraft:wood*/
            new Item(-212, 0, 1){ RuntimeId=3484, NetworkId=464, ExtraData = null }, /*minecraft:wood*/
            new Item(-212, 0, 1){ RuntimeId=3479, NetworkId=465, ExtraData = null }, /*minecraft:wood*/
            new Item(-212, 0, 1){ RuntimeId=3485, NetworkId=466, ExtraData = null }, /*minecraft:wood*/
            new Item(-497, 0, 1){ RuntimeId=4161, NetworkId=467, ExtraData = null }, /**/
            new Item(-498, 0, 1){ RuntimeId=4229, NetworkId=468, ExtraData = null }, /**/
            new Item(-299, 0, 1){ RuntimeId=4294, NetworkId=469, ExtraData = null }, /*minecraft:crimson_hyphae*/
            new Item(-300, 0, 1){ RuntimeId=6499, NetworkId=470, ExtraData = null }, /*minecraft:stripped_crimson_hyphae*/
            new Item(-298, 0, 1){ RuntimeId=5902, NetworkId=471, ExtraData = null }, /*minecraft:warped_hyphae*/
            new Item(-301, 0, 1){ RuntimeId=5579, NetworkId=472, ExtraData = null }, /*minecraft:stripped_warped_hyphae*/
            new Item(18, 0, 1){ RuntimeId=6090, NetworkId=473, ExtraData = null }, /*minecraft:leaves*/
            new Item(18, 0, 1){ RuntimeId=6091, NetworkId=474, ExtraData = null }, /*minecraft:leaves*/
            new Item(18, 0, 1){ RuntimeId=6092, NetworkId=475, ExtraData = null }, /*minecraft:leaves*/
            new Item(18, 0, 1){ RuntimeId=6093, NetworkId=476, ExtraData = null }, /*minecraft:leaves*/
            new Item(161, 0, 1){ RuntimeId=4353, NetworkId=477, ExtraData = null }, /*minecraft:leaves2*/
            new Item(161, 0, 1){ RuntimeId=4354, NetworkId=478, ExtraData = null }, /*minecraft:leaves2*/
            new Item(-472, 0, 1){ RuntimeId=6666, NetworkId=479, ExtraData = null }, /**/
            new Item(-324, 0, 1){ RuntimeId=7710, NetworkId=480, ExtraData = null }, /**/
            new Item(-325, 0, 1){ RuntimeId=6339, NetworkId=481, ExtraData = null }, /**/
            new Item(6, 0, 1){ RuntimeId=712, NetworkId=482, ExtraData = null }, /*minecraft:sapling*/
            new Item(6, 0, 1){ RuntimeId=713, NetworkId=483, ExtraData = null }, /*minecraft:sapling*/
            new Item(6, 0, 1){ RuntimeId=714, NetworkId=484, ExtraData = null }, /*minecraft:sapling*/
            new Item(6, 0, 1){ RuntimeId=715, NetworkId=485, ExtraData = null }, /*minecraft:sapling*/
            new Item(6, 0, 1){ RuntimeId=716, NetworkId=486, ExtraData = null }, /*minecraft:sapling*/
            new Item(6, 0, 1){ RuntimeId=717, NetworkId=487, ExtraData = null }, /*minecraft:sapling*/
            new Item(-474, 0, 1){ RuntimeId=6976, NetworkId=488, ExtraData = null }, /**/
            new Item(-218, 0, 1){ RuntimeId=5754, NetworkId=489, ExtraData = null }, /*minecraft:bee_nest*/
            new Item(295, 0, 1){ RuntimeId=0, NetworkId=490, ExtraData = null }, /*minecraft:wheat_seeds*/
            new Item(361, 0, 1){ RuntimeId=0, NetworkId=491, ExtraData = null }, /*minecraft:pumpkin_seeds*/
            new Item(362, 0, 1){ RuntimeId=0, NetworkId=492, ExtraData = null }, /*minecraft:melon_seeds*/
            new Item(458, 0, 1){ RuntimeId=0, NetworkId=493, ExtraData = null }, /*minecraft:beetroot_seeds*/
            new Item(296, 0, 1){ RuntimeId=0, NetworkId=494, ExtraData = null }, /*minecraft:wheat*/
            new Item(457, 0, 1){ RuntimeId=0, NetworkId=495, ExtraData = null }, /*minecraft:beetroot*/
            new Item(392, 0, 1){ RuntimeId=0, NetworkId=496, ExtraData = null }, /*minecraft:potato*/
            new Item(394, 0, 1){ RuntimeId=0, NetworkId=497, ExtraData = null }, /*minecraft:poisonous_potato*/
            new Item(391, 0, 1){ RuntimeId=0, NetworkId=498, ExtraData = null }, /*minecraft:carrot*/
            new Item(396, 0, 1){ RuntimeId=0, NetworkId=499, ExtraData = null }, /*minecraft:golden_carrot*/
            new Item(260, 0, 1){ RuntimeId=0, NetworkId=500, ExtraData = null }, /*minecraft:apple*/
            new Item(322, 0, 1){ RuntimeId=0, NetworkId=501, ExtraData = null }, /*minecraft:golden_apple*/
            new Item(466, 0, 1){ RuntimeId=0, NetworkId=502, ExtraData = null }, /*minecraft:enchanted_golden_apple*/
            new Item(103, 0, 1){ RuntimeId=392, NetworkId=503, ExtraData = null }, /*minecraft:melon_block*/
            new Item(360, 0, 1){ RuntimeId=0, NetworkId=504, ExtraData = null }, /*minecraft:melon_slice*/
            new Item(382, 0, 1){ RuntimeId=0, NetworkId=505, ExtraData = null }, /*minecraft:glistering_melon_slice*/
            new Item(477, 0, 1){ RuntimeId=0, NetworkId=506, ExtraData = null }, /*minecraft:sweet_berries*/
            new Item(654, 0, 1){ RuntimeId=0, NetworkId=507, ExtraData = null }, /**/
            new Item(86, 0, 1){ RuntimeId=4577, NetworkId=508, ExtraData = null }, /*minecraft:pumpkin*/
            new Item(-155, 0, 1){ RuntimeId=7378, NetworkId=509, ExtraData = null }, /*minecraft:carved_pumpkin*/
            new Item(91, 0, 1){ RuntimeId=6685, NetworkId=510, ExtraData = null }, /*minecraft:lit_pumpkin*/
            new Item(737, 0, 1){ RuntimeId=0, NetworkId=511, ExtraData = null }, /*minecraft:honey_bottle*/
            new Item(31, 0, 1){ RuntimeId=929, NetworkId=512, ExtraData = null }, /*minecraft:tallgrass*/
            new Item(175, 0, 1){ RuntimeId=5455, NetworkId=513, ExtraData = null }, /*minecraft:double_plant*/
            new Item(31, 0, 1){ RuntimeId=928, NetworkId=514, ExtraData = null }, /*minecraft:tallgrass*/
            new Item(175, 0, 1){ RuntimeId=5454, NetworkId=515, ExtraData = null }, /*minecraft:double_plant*/
            new Item(621, 0, 1){ RuntimeId=0, NetworkId=516, ExtraData = null }, /**/
            new Item(-131, 0, 1){ RuntimeId=6492, NetworkId=517, ExtraData = null }, /*minecraft:coral*/
            new Item(-131, 0, 1){ RuntimeId=6490, NetworkId=518, ExtraData = null }, /*minecraft:coral*/
            new Item(-131, 0, 1){ RuntimeId=6491, NetworkId=519, ExtraData = null }, /*minecraft:coral*/
            new Item(-131, 0, 1){ RuntimeId=6489, NetworkId=520, ExtraData = null }, /*minecraft:coral*/
            new Item(-131, 0, 1){ RuntimeId=6493, NetworkId=521, ExtraData = null }, /*minecraft:coral*/
            new Item(-131, 0, 1){ RuntimeId=6497, NetworkId=522, ExtraData = null }, /*minecraft:coral*/
            new Item(-131, 0, 1){ RuntimeId=6495, NetworkId=523, ExtraData = null }, /*minecraft:coral*/
            new Item(-131, 0, 1){ RuntimeId=6496, NetworkId=524, ExtraData = null }, /*minecraft:coral*/
            new Item(-131, 0, 1){ RuntimeId=6494, NetworkId=525, ExtraData = null }, /*minecraft:coral*/
            new Item(-131, 0, 1){ RuntimeId=6498, NetworkId=526, ExtraData = null }, /*minecraft:coral*/
            new Item(-133, 0, 1){ RuntimeId=4616, NetworkId=527, ExtraData = null }, /*minecraft:coral_fan*/
            new Item(-133, 0, 1){ RuntimeId=4614, NetworkId=528, ExtraData = null }, /*minecraft:coral_fan*/
            new Item(-133, 0, 1){ RuntimeId=4615, NetworkId=529, ExtraData = null }, /*minecraft:coral_fan*/
            new Item(-133, 0, 1){ RuntimeId=4613, NetworkId=530, ExtraData = null }, /*minecraft:coral_fan*/
            new Item(-133, 0, 1){ RuntimeId=4617, NetworkId=531, ExtraData = null }, /*minecraft:coral_fan*/
            new Item(-134, 0, 1){ RuntimeId=69, NetworkId=532, ExtraData = null }, /*minecraft:coral_fan_dead*/
            new Item(-134, 0, 1){ RuntimeId=67, NetworkId=533, ExtraData = null }, /*minecraft:coral_fan_dead*/
            new Item(-134, 0, 1){ RuntimeId=68, NetworkId=534, ExtraData = null }, /*minecraft:coral_fan_dead*/
            new Item(-134, 0, 1){ RuntimeId=66, NetworkId=535, ExtraData = null }, /*minecraft:coral_fan_dead*/
            new Item(-134, 0, 1){ RuntimeId=70, NetworkId=536, ExtraData = null }, /*minecraft:coral_fan_dead*/
            new Item(335, 0, 1){ RuntimeId=0, NetworkId=537, ExtraData = null }, /*minecraft:kelp*/
            new Item(-130, 0, 1){ RuntimeId=246, NetworkId=538, ExtraData = null }, /*minecraft:seagrass*/
            new Item(-223, 0, 1){ RuntimeId=7573, NetworkId=539, ExtraData = null }, /*minecraft:crimson_roots*/
            new Item(-224, 0, 1){ RuntimeId=4362, NetworkId=540, ExtraData = null }, /*minecraft:warped_roots*/
            new Item(37, 0, 1){ RuntimeId=302, NetworkId=541, ExtraData = null }, /*minecraft:yellow_flower*/
            new Item(38, 0, 1){ RuntimeId=3616, NetworkId=542, ExtraData = null }, /*minecraft:red_flower*/
            new Item(38, 0, 1){ RuntimeId=3617, NetworkId=543, ExtraData = null }, /*minecraft:red_flower*/
            new Item(38, 0, 1){ RuntimeId=3618, NetworkId=544, ExtraData = null }, /*minecraft:red_flower*/
            new Item(38, 0, 1){ RuntimeId=3619, NetworkId=545, ExtraData = null }, /*minecraft:red_flower*/
            new Item(38, 0, 1){ RuntimeId=3620, NetworkId=546, ExtraData = null }, /*minecraft:red_flower*/
            new Item(38, 0, 1){ RuntimeId=3621, NetworkId=547, ExtraData = null }, /*minecraft:red_flower*/
            new Item(38, 0, 1){ RuntimeId=3622, NetworkId=548, ExtraData = null }, /*minecraft:red_flower*/
            new Item(38, 0, 1){ RuntimeId=3623, NetworkId=549, ExtraData = null }, /*minecraft:red_flower*/
            new Item(38, 0, 1){ RuntimeId=3624, NetworkId=550, ExtraData = null }, /*minecraft:red_flower*/
            new Item(38, 0, 1){ RuntimeId=3625, NetworkId=551, ExtraData = null }, /*minecraft:red_flower*/
            new Item(38, 0, 1){ RuntimeId=3626, NetworkId=552, ExtraData = null }, /*minecraft:red_flower*/
            new Item(175, 0, 1){ RuntimeId=5452, NetworkId=553, ExtraData = null }, /*minecraft:double_plant*/
            new Item(175, 0, 1){ RuntimeId=5453, NetworkId=554, ExtraData = null }, /*minecraft:double_plant*/
            new Item(175, 0, 1){ RuntimeId=5456, NetworkId=555, ExtraData = null }, /*minecraft:double_plant*/
            new Item(175, 0, 1){ RuntimeId=5457, NetworkId=556, ExtraData = null }, /*minecraft:double_plant*/
            new Item(-216, 0, 1){ RuntimeId=6165, NetworkId=557, ExtraData = null }, /*minecraft:wither_rose*/
            new Item(351, 19, 1){ RuntimeId=0, NetworkId=558, ExtraData = null }, /*minecraft:dye*/
            new Item(351, 7, 1){ RuntimeId=0, NetworkId=559, ExtraData = null }, /*minecraft:dye*/
            new Item(351, 8, 1){ RuntimeId=0, NetworkId=560, ExtraData = null }, /*minecraft:dye*/
            new Item(351, 16, 1){ RuntimeId=0, NetworkId=561, ExtraData = null }, /*minecraft:dye*/
            new Item(351, 17, 1){ RuntimeId=0, NetworkId=562, ExtraData = null }, /*minecraft:dye*/
            new Item(351, 1, 1){ RuntimeId=0, NetworkId=563, ExtraData = null }, /*minecraft:dye*/
            new Item(351, 14, 1){ RuntimeId=0, NetworkId=564, ExtraData = null }, /*minecraft:dye*/
            new Item(351, 11, 1){ RuntimeId=0, NetworkId=565, ExtraData = null }, /*minecraft:dye*/
            new Item(351, 10, 1){ RuntimeId=0, NetworkId=566, ExtraData = null }, /*minecraft:dye*/
            new Item(351, 2, 1){ RuntimeId=0, NetworkId=567, ExtraData = null }, /*minecraft:dye*/
            new Item(351, 6, 1){ RuntimeId=0, NetworkId=568, ExtraData = null }, /*minecraft:dye*/
            new Item(351, 12, 1){ RuntimeId=0, NetworkId=569, ExtraData = null }, /*minecraft:dye*/
            new Item(351, 18, 1){ RuntimeId=0, NetworkId=570, ExtraData = null }, /*minecraft:dye*/
            new Item(351, 5, 1){ RuntimeId=0, NetworkId=571, ExtraData = null }, /*minecraft:dye*/
            new Item(351, 13, 1){ RuntimeId=0, NetworkId=572, ExtraData = null }, /*minecraft:dye*/
            new Item(351, 9, 1){ RuntimeId=0, NetworkId=573, ExtraData = null }, /*minecraft:dye*/
            new Item(351, 0, 1){ RuntimeId=0, NetworkId=574, ExtraData = null }, /*minecraft:dye*/
            new Item(503, 0, 1){ RuntimeId=0, NetworkId=575, ExtraData = null }, /*minecraft:music_disc_chirp*/
            new Item(351, 3, 1){ RuntimeId=0, NetworkId=576, ExtraData = null }, /*minecraft:dye*/
            new Item(351, 4, 1){ RuntimeId=0, NetworkId=577, ExtraData = null }, /*minecraft:dye*/
            new Item(351, 15, 1){ RuntimeId=0, NetworkId=578, ExtraData = null }, /*minecraft:dye*/
            new Item(106, 0, 1){ RuntimeId=894, NetworkId=579, ExtraData = null }, /*minecraft:vine*/
            new Item(-231, 0, 1){ RuntimeId=5479, NetworkId=580, ExtraData = null }, /*minecraft:weeping_vines*/
            new Item(-287, 0, 1){ RuntimeId=5691, NetworkId=581, ExtraData = null }, /*minecraft:twisting_vines*/
            new Item(111, 0, 1){ RuntimeId=1158, NetworkId=582, ExtraData = null }, /*minecraft:waterlily*/
            new Item(32, 0, 1){ RuntimeId=4677, NetworkId=583, ExtraData = null }, /*minecraft:deadbush*/
            new Item(-163, 0, 1){ RuntimeId=3684, NetworkId=584, ExtraData = null }, /*minecraft:bamboo*/
            new Item(80, 0, 1){ RuntimeId=4194, NetworkId=585, ExtraData = null }, /*minecraft:snow*/
            new Item(79, 0, 1){ RuntimeId=6689, NetworkId=586, ExtraData = null }, /*minecraft:ice*/
            new Item(174, 0, 1){ RuntimeId=282, NetworkId=587, ExtraData = null }, /*minecraft:packed_ice*/
            new Item(-11, 0, 1){ RuntimeId=7027, NetworkId=588, ExtraData = null }, /*minecraft:blue_ice*/
            new Item(78, 0, 1){ RuntimeId=155, NetworkId=589, ExtraData = null }, /*minecraft:snow_layer*/
            new Item(-308, 0, 1){ RuntimeId=7416, NetworkId=590, ExtraData = null }, /**/
            new Item(-317, 0, 1){ RuntimeId=893, NetworkId=591, ExtraData = null }, /**/
            new Item(-335, 0, 1){ RuntimeId=286, NetworkId=592, ExtraData = null }, /**/
            new Item(-320, 0, 1){ RuntimeId=6538, NetworkId=593, ExtraData = null }, /**/
            new Item(-318, 0, 1){ RuntimeId=5397, NetworkId=594, ExtraData = null }, /**/
            new Item(-319, 0, 1){ RuntimeId=205, NetworkId=595, ExtraData = null }, /**/
            new Item(-482, 0, 1){ RuntimeId=6175, NetworkId=596, ExtraData = null }, /**/
            new Item(-483, 0, 1){ RuntimeId=345, NetworkId=597, ExtraData = null }, /**/
            new Item(-323, 0, 1){ RuntimeId=5980, NetworkId=598, ExtraData = null }, /**/
            new Item(-336, 0, 1){ RuntimeId=4320, NetworkId=599, ExtraData = null }, /**/
            new Item(-321, 0, 1){ RuntimeId=7312, NetworkId=600, ExtraData = null }, /**/
            new Item(-337, 0, 1){ RuntimeId=6888, NetworkId=601, ExtraData = null }, /**/
            new Item(-338, 0, 1){ RuntimeId=5477, NetworkId=602, ExtraData = null }, /**/
            new Item(-411, 0, 1){ RuntimeId=5684, NetworkId=603, ExtraData = null }, /**/
            new Item(-327, 0, 1){ RuntimeId=290, NetworkId=604, ExtraData = null }, /**/
            new Item(-328, 0, 1){ RuntimeId=7002, NetworkId=605, ExtraData = null }, /**/
            new Item(-329, 0, 1){ RuntimeId=7810, NetworkId=606, ExtraData = null }, /**/
            new Item(-330, 0, 1){ RuntimeId=4728, NetworkId=607, ExtraData = null }, /**/
            new Item(-331, 0, 1){ RuntimeId=4376, NetworkId=608, ExtraData = null }, /**/
            new Item(-332, 0, 1){ RuntimeId=304, NetworkId=609, ExtraData = null }, /**/
            new Item(-333, 0, 1){ RuntimeId=347, NetworkId=610, ExtraData = null }, /**/
            new Item(-326, 0, 1){ RuntimeId=215, NetworkId=611, ExtraData = null }, /**/
            new Item(365, 0, 1){ RuntimeId=0, NetworkId=612, ExtraData = null }, /*minecraft:chicken*/
            new Item(319, 0, 1){ RuntimeId=0, NetworkId=613, ExtraData = null }, /*minecraft:porkchop*/
            new Item(363, 0, 1){ RuntimeId=0, NetworkId=614, ExtraData = null }, /*minecraft:beef*/
            new Item(423, 0, 1){ RuntimeId=0, NetworkId=615, ExtraData = null }, /*minecraft:mutton*/
            new Item(411, 0, 1){ RuntimeId=0, NetworkId=616, ExtraData = null }, /*minecraft:rabbit*/
            new Item(349, 0, 1){ RuntimeId=0, NetworkId=617, ExtraData = null }, /*minecraft:cod*/
            new Item(460, 0, 1){ RuntimeId=0, NetworkId=618, ExtraData = null }, /*minecraft:salmon*/
            new Item(461, 0, 1){ RuntimeId=0, NetworkId=619, ExtraData = null }, /*minecraft:tropical_fish*/
            new Item(462, 0, 1){ RuntimeId=0, NetworkId=620, ExtraData = null }, /*minecraft:pufferfish*/
            new Item(39, 0, 1){ RuntimeId=3546, NetworkId=621, ExtraData = null }, /*minecraft:brown_mushroom*/
            new Item(40, 0, 1){ RuntimeId=4585, NetworkId=622, ExtraData = null }, /*minecraft:red_mushroom*/
            new Item(-228, 0, 1){ RuntimeId=7753, NetworkId=623, ExtraData = null }, /*minecraft:crimson_fungus*/
            new Item(-229, 0, 1){ RuntimeId=287, NetworkId=624, ExtraData = null }, /*minecraft:warped_fungus*/
            new Item(99, 0, 1){ RuntimeId=7362, NetworkId=625, ExtraData = null }, /*minecraft:brown_mushroom_block*/
            new Item(100, 0, 1){ RuntimeId=3611, NetworkId=626, ExtraData = null }, /*minecraft:red_mushroom_block*/
            new Item(99, 0, 1){ RuntimeId=7363, NetworkId=627, ExtraData = null }, /*minecraft:brown_mushroom_block*/
            new Item(99, 0, 1){ RuntimeId=7348, NetworkId=628, ExtraData = null }, /*minecraft:brown_mushroom_block*/
            new Item(344, 0, 1){ RuntimeId=0, NetworkId=629, ExtraData = null }, /*minecraft:egg*/
            new Item(338, 0, 1){ RuntimeId=0, NetworkId=630, ExtraData = null }, /*minecraft:item.reeds*/
            new Item(353, 0, 1){ RuntimeId=0, NetworkId=631, ExtraData = null }, /*minecraft:sugar*/
            new Item(367, 0, 1){ RuntimeId=0, NetworkId=632, ExtraData = null }, /*minecraft:rotten_flesh*/
            new Item(352, 0, 1){ RuntimeId=0, NetworkId=633, ExtraData = null }, /*minecraft:bone*/
            new Item(30, 0, 1){ RuntimeId=6713, NetworkId=634, ExtraData = null }, /*minecraft:web*/
            new Item(375, 0, 1){ RuntimeId=0, NetworkId=635, ExtraData = null }, /*minecraft:spider_eye*/
            new Item(52, 0, 1){ RuntimeId=401, NetworkId=636, ExtraData = null }, /*minecraft:mob_spawner*/
            new Item(97, 0, 1){ RuntimeId=4144, NetworkId=637, ExtraData = null }, /*minecraft:monster_egg*/
            new Item(97, 0, 1){ RuntimeId=4145, NetworkId=638, ExtraData = null }, /*minecraft:monster_egg*/
            new Item(97, 0, 1){ RuntimeId=4146, NetworkId=639, ExtraData = null }, /*minecraft:monster_egg*/
            new Item(97, 0, 1){ RuntimeId=4147, NetworkId=640, ExtraData = null }, /*minecraft:monster_egg*/
            new Item(97, 0, 1){ RuntimeId=4148, NetworkId=641, ExtraData = null }, /*minecraft:monster_egg*/
            new Item(97, 0, 1){ RuntimeId=4149, NetworkId=642, ExtraData = null }, /*minecraft:monster_egg*/
            new Item(-454, 0, 1){ RuntimeId=4641, NetworkId=643, ExtraData = null }, /**/
            new Item(122, 0, 1){ RuntimeId=7271, NetworkId=644, ExtraData = null }, /*minecraft:dragon_egg*/
            new Item(-159, 0, 1){ RuntimeId=7997, NetworkId=645, ExtraData = null }, /*minecraft:turtle_egg*/
            new Item(-468, 0, 1){ RuntimeId=4399, NetworkId=646, ExtraData = null }, /**/
            new Item(-469, 0, 1){ RuntimeId=6435, NetworkId=647, ExtraData = null }, /**/
            new Item(-470, 0, 1){ RuntimeId=6481, NetworkId=648, ExtraData = null }, /**/
            new Item(-471, 0, 1){ RuntimeId=3510, NetworkId=649, ExtraData = null }, /**/
            new Item(383, 10, 1){ RuntimeId=0, NetworkId=650, ExtraData = null }, /*minecraft:spawn_egg*/
            new Item(383, 122, 1){ RuntimeId=0, NetworkId=651, ExtraData = null }, /*minecraft:spawn_egg*/
            new Item(383, 11, 1){ RuntimeId=0, NetworkId=652, ExtraData = null }, /*minecraft:spawn_egg*/
            new Item(383, 12, 1){ RuntimeId=0, NetworkId=653, ExtraData = null }, /*minecraft:spawn_egg*/
            new Item(383, 13, 1){ RuntimeId=0, NetworkId=654, ExtraData = null }, /*minecraft:spawn_egg*/
            new Item(383, 14, 1){ RuntimeId=0, NetworkId=655, ExtraData = null }, /*minecraft:spawn_egg*/
            new Item(383, 28, 1){ RuntimeId=0, NetworkId=656, ExtraData = null }, /*minecraft:spawn_egg*/
            new Item(383, 22, 1){ RuntimeId=0, NetworkId=657, ExtraData = null }, /*minecraft:spawn_egg*/
            new Item(383, 75, 1){ RuntimeId=0, NetworkId=658, ExtraData = null }, /*minecraft:spawn_egg*/
            new Item(383, 16, 1){ RuntimeId=0, NetworkId=659, ExtraData = null }, /*minecraft:spawn_egg*/
            new Item(383, 19, 1){ RuntimeId=0, NetworkId=660, ExtraData = null }, /*minecraft:spawn_egg*/
            new Item(383, 30, 1){ RuntimeId=0, NetworkId=661, ExtraData = null }, /*minecraft:spawn_egg*/
            new Item(383, 18, 1){ RuntimeId=0, NetworkId=662, ExtraData = null }, /*minecraft:spawn_egg*/
            new Item(383, 29, 1){ RuntimeId=0, NetworkId=663, ExtraData = null }, /*minecraft:spawn_egg*/
            new Item(383, 23, 1){ RuntimeId=0, NetworkId=664, ExtraData = null }, /*minecraft:spawn_egg*/
            new Item(383, 24, 1){ RuntimeId=0, NetworkId=665, ExtraData = null }, /*minecraft:spawn_egg*/
            new Item(383, 25, 1){ RuntimeId=0, NetworkId=666, ExtraData = null }, /*minecraft:spawn_egg*/
            new Item(383, 26, 1){ RuntimeId=0, NetworkId=667, ExtraData = null }, /*minecraft:spawn_egg*/
            new Item(383, 27, 1){ RuntimeId=0, NetworkId=668, ExtraData = null }, /*minecraft:spawn_egg*/
            new Item(383, 111, 1){ RuntimeId=0, NetworkId=669, ExtraData = null }, /*minecraft:spawn_egg*/
            new Item(383, 112, 1){ RuntimeId=0, NetworkId=670, ExtraData = null }, /*minecraft:spawn_egg*/
            new Item(383, 108, 1){ RuntimeId=0, NetworkId=671, ExtraData = null }, /*minecraft:spawn_egg*/
            new Item(383, 109, 1){ RuntimeId=0, NetworkId=672, ExtraData = null }, /*minecraft:spawn_egg*/
            new Item(383, 31, 1){ RuntimeId=0, NetworkId=673, ExtraData = null }, /*minecraft:spawn_egg*/
            new Item(383, 74, 1){ RuntimeId=0, NetworkId=674, ExtraData = null }, /*minecraft:spawn_egg*/
            new Item(383, 113, 1){ RuntimeId=0, NetworkId=675, ExtraData = null }, /*minecraft:spawn_egg*/
            new Item(383, 121, 1){ RuntimeId=0, NetworkId=676, ExtraData = null }, /*minecraft:spawn_egg*/
            new Item(383, 33, 1){ RuntimeId=0, NetworkId=677, ExtraData = null }, /*minecraft:spawn_egg*/
            new Item(383, 38, 1){ RuntimeId=0, NetworkId=678, ExtraData = null }, /*minecraft:spawn_egg*/
            new Item(383, 39, 1){ RuntimeId=0, NetworkId=679, ExtraData = null }, /*minecraft:spawn_egg*/
            new Item(383, 34, 1){ RuntimeId=0, NetworkId=680, ExtraData = null }, /*minecraft:spawn_egg*/
            new Item(383, 48, 1){ RuntimeId=0, NetworkId=681, ExtraData = null }, /*minecraft:spawn_egg*/
            new Item(383, 46, 1){ RuntimeId=0, NetworkId=682, ExtraData = null }, /*minecraft:spawn_egg*/
            new Item(383, 37, 1){ RuntimeId=0, NetworkId=683, ExtraData = null }, /*minecraft:spawn_egg*/
            new Item(383, 35, 1){ RuntimeId=0, NetworkId=684, ExtraData = null }, /*minecraft:spawn_egg*/
            new Item(383, 32, 1){ RuntimeId=0, NetworkId=685, ExtraData = null }, /*minecraft:spawn_egg*/
            new Item(383, 36, 1){ RuntimeId=0, NetworkId=686, ExtraData = null }, /*minecraft:spawn_egg*/
            new Item(383, 47, 1){ RuntimeId=0, NetworkId=687, ExtraData = null }, /*minecraft:spawn_egg*/
            new Item(383, 110, 1){ RuntimeId=0, NetworkId=688, ExtraData = null }, /*minecraft:spawn_egg*/
            new Item(383, 17, 1){ RuntimeId=0, NetworkId=689, ExtraData = null }, /*minecraft:spawn_egg*/
            new Item(383, 129, 1){ RuntimeId=0, NetworkId=690, ExtraData = null }, /*minecraft:spawn_egg*/
            new Item(383, 40, 1){ RuntimeId=0, NetworkId=691, ExtraData = null }, /*minecraft:spawn_egg*/
            new Item(383, 45, 1){ RuntimeId=0, NetworkId=692, ExtraData = null }, /*minecraft:spawn_egg*/
            new Item(383, 49, 1){ RuntimeId=0, NetworkId=693, ExtraData = null }, /*minecraft:spawn_egg*/
            new Item(383, 50, 1){ RuntimeId=0, NetworkId=694, ExtraData = null }, /*minecraft:spawn_egg*/
            new Item(383, 55, 1){ RuntimeId=0, NetworkId=695, ExtraData = null }, /*minecraft:spawn_egg*/
            new Item(383, 42, 1){ RuntimeId=0, NetworkId=696, ExtraData = null }, /*minecraft:spawn_egg*/
            new Item(383, 125, 1){ RuntimeId=0, NetworkId=697, ExtraData = null }, /*minecraft:spawn_egg*/
            new Item(383, 124, 1){ RuntimeId=0, NetworkId=698, ExtraData = null }, /*minecraft:spawn_egg*/
            new Item(383, 123, 1){ RuntimeId=0, NetworkId=699, ExtraData = null }, /*minecraft:spawn_egg*/
            new Item(383, 126, 1){ RuntimeId=0, NetworkId=700, ExtraData = null }, /*minecraft:spawn_egg*/
            new Item(383, 127, 1){ RuntimeId=0, NetworkId=701, ExtraData = null }, /*minecraft:spawn_egg*/
            new Item(383, 128, 1){ RuntimeId=0, NetworkId=702, ExtraData = null }, /*minecraft:spawn_egg*/
            new Item(500, 0, 1){ RuntimeId=0, NetworkId=703, ExtraData = null }, /*minecraft:music_disc_13*/
            new Item(632, 0, 1){ RuntimeId=0, NetworkId=704, ExtraData = null }, /**/
            new Item(631, 0, 1){ RuntimeId=0, NetworkId=705, ExtraData = null }, /**/
            new Item(383, 0, 1){ RuntimeId=0, NetworkId=706, ExtraData = null }, /*minecraft:spawn_egg*/
            new Item(426, 0, 1){ RuntimeId=0, NetworkId=707, ExtraData = null }, /*minecraft:end_crystal*/
            new Item(648, 0, 1){ RuntimeId=0, NetworkId=708, ExtraData = null }, /**/
            new Item(383, 41, 1){ RuntimeId=0, NetworkId=709, ExtraData = null }, /*minecraft:spawn_egg*/
            new Item(383, 43, 1){ RuntimeId=0, NetworkId=710, ExtraData = null }, /*minecraft:spawn_egg*/
            new Item(383, 54, 1){ RuntimeId=0, NetworkId=711, ExtraData = null }, /*minecraft:spawn_egg*/
            new Item(383, 57, 1){ RuntimeId=0, NetworkId=712, ExtraData = null }, /*minecraft:spawn_egg*/
            new Item(383, 104, 1){ RuntimeId=0, NetworkId=713, ExtraData = null }, /*minecraft:spawn_egg*/
            new Item(383, 105, 1){ RuntimeId=0, NetworkId=714, ExtraData = null }, /*minecraft:spawn_egg*/
            new Item(383, 115, 1){ RuntimeId=0, NetworkId=715, ExtraData = null }, /*minecraft:spawn_egg*/
            new Item(383, 118, 1){ RuntimeId=0, NetworkId=716, ExtraData = null }, /*minecraft:spawn_egg*/
            new Item(383, 116, 1){ RuntimeId=0, NetworkId=717, ExtraData = null }, /*minecraft:spawn_egg*/
            new Item(383, 58, 1){ RuntimeId=0, NetworkId=718, ExtraData = null }, /*minecraft:spawn_egg*/
            new Item(383, 114, 1){ RuntimeId=0, NetworkId=719, ExtraData = null }, /*minecraft:spawn_egg*/
            new Item(383, 59, 1){ RuntimeId=0, NetworkId=720, ExtraData = null }, /*minecraft:spawn_egg*/
            new Item(49, 0, 1){ RuntimeId=428, NetworkId=721, ExtraData = null }, /*minecraft:obsidian*/
            new Item(-289, 0, 1){ RuntimeId=6722, NetworkId=722, ExtraData = null }, /*minecraft:crying_obsidian*/
            new Item(7, 0, 1){ RuntimeId=7017, NetworkId=723, ExtraData = null }, /*minecraft:bedrock*/
            new Item(88, 0, 1){ RuntimeId=5831, NetworkId=724, ExtraData = null }, /*minecraft:soul_sand*/
            new Item(87, 0, 1){ RuntimeId=7037, NetworkId=725, ExtraData = null }, /*minecraft:netherrack*/
            new Item(213, 0, 1){ RuntimeId=8009, NetworkId=726, ExtraData = null }, /*minecraft:magma*/
            new Item(372, 0, 1){ RuntimeId=0, NetworkId=727, ExtraData = null }, /*minecraft:nether_wart*/
            new Item(121, 0, 1){ RuntimeId=3836, NetworkId=728, ExtraData = null }, /*minecraft:end_stone*/
            new Item(200, 0, 1){ RuntimeId=4530, NetworkId=729, ExtraData = null }, /*minecraft:chorus_flower*/
            new Item(240, 0, 1){ RuntimeId=5505, NetworkId=730, ExtraData = null }, /*minecraft:chorus_plant*/
            new Item(432, 0, 1){ RuntimeId=0, NetworkId=731, ExtraData = null }, /*minecraft:chorus_fruit*/
            new Item(433, 0, 1){ RuntimeId=0, NetworkId=732, ExtraData = null }, /*minecraft:popped_chorus_fruit*/
            new Item(19, 0, 1){ RuntimeId=629, NetworkId=733, ExtraData = null }, /*minecraft:sponge*/
            new Item(19, 0, 1){ RuntimeId=630, NetworkId=734, ExtraData = null }, /*minecraft:sponge*/
            new Item(-132, 0, 1){ RuntimeId=5237, NetworkId=735, ExtraData = null }, /*minecraft:coral_block*/
            new Item(-132, 0, 1){ RuntimeId=5238, NetworkId=736, ExtraData = null }, /*minecraft:coral_block*/
            new Item(-132, 0, 1){ RuntimeId=5239, NetworkId=737, ExtraData = null }, /*minecraft:coral_block*/
            new Item(-132, 0, 1){ RuntimeId=5240, NetworkId=738, ExtraData = null }, /*minecraft:coral_block*/
            new Item(-132, 0, 1){ RuntimeId=5241, NetworkId=739, ExtraData = null }, /*minecraft:coral_block*/
            new Item(-132, 0, 1){ RuntimeId=5242, NetworkId=740, ExtraData = null }, /*minecraft:coral_block*/
            new Item(-132, 0, 1){ RuntimeId=5243, NetworkId=741, ExtraData = null }, /*minecraft:coral_block*/
            new Item(-132, 0, 1){ RuntimeId=5244, NetworkId=742, ExtraData = null }, /*minecraft:coral_block*/
            new Item(-132, 0, 1){ RuntimeId=5245, NetworkId=743, ExtraData = null }, /*minecraft:coral_block*/
            new Item(-132, 0, 1){ RuntimeId=5246, NetworkId=744, ExtraData = null }, /*minecraft:coral_block*/
            new Item(-458, 0, 1){ RuntimeId=7036, NetworkId=745, ExtraData = null }, /**/
            new Item(-459, 0, 1){ RuntimeId=7132, NetworkId=746, ExtraData = null }, /**/
            new Item(-460, 0, 1){ RuntimeId=3613, NetworkId=747, ExtraData = null }, /**/
            new Item(-461, 0, 1){ RuntimeId=219, NetworkId=748, ExtraData = null }, /**/
            new Item(-307, 0, 1){ RuntimeId=4389, NetworkId=749, ExtraData = null }, /**/
            new Item(-466, 0, 1){ RuntimeId=5832, NetworkId=750, ExtraData = null }, /**/
            new Item(298, 0, 1){ RuntimeId=0, NetworkId=751, ExtraData = null }, /*minecraft:leather_helmet*/
            new Item(302, 0, 1){ RuntimeId=0, NetworkId=752, ExtraData = null }, /*minecraft:chainmail_helmet*/
            new Item(306, 0, 1){ RuntimeId=0, NetworkId=753, ExtraData = null }, /*minecraft:iron_helmet*/
            new Item(314, 0, 1){ RuntimeId=0, NetworkId=754, ExtraData = null }, /*minecraft:golden_helmet*/
            new Item(310, 0, 1){ RuntimeId=0, NetworkId=755, ExtraData = null }, /*minecraft:diamond_helmet*/
            new Item(750, 0, 1){ RuntimeId=0, NetworkId=756, ExtraData = null }, /*minecraft:netherite_leggings*/
            new Item(299, 0, 1){ RuntimeId=0, NetworkId=757, ExtraData = null }, /*minecraft:leather_chestplate*/
            new Item(303, 0, 1){ RuntimeId=0, NetworkId=758, ExtraData = null }, /*minecraft:chainmail_chestplate*/
            new Item(307, 0, 1){ RuntimeId=0, NetworkId=759, ExtraData = null }, /*minecraft:iron_chestplate*/
            new Item(315, 0, 1){ RuntimeId=0, NetworkId=760, ExtraData = null }, /*minecraft:golden_chestplate*/
            new Item(311, 0, 1){ RuntimeId=0, NetworkId=761, ExtraData = null }, /*minecraft:diamond_chestplate*/
            new Item(751, 0, 1){ RuntimeId=0, NetworkId=762, ExtraData = null }, /*minecraft:netherite_boots*/
            new Item(300, 0, 1){ RuntimeId=0, NetworkId=763, ExtraData = null }, /*minecraft:leather_leggings*/
            new Item(304, 0, 1){ RuntimeId=0, NetworkId=764, ExtraData = null }, /*minecraft:chainmail_leggings*/
            new Item(308, 0, 1){ RuntimeId=0, NetworkId=765, ExtraData = null }, /*minecraft:iron_leggings*/
            new Item(316, 0, 1){ RuntimeId=0, NetworkId=766, ExtraData = null }, /*minecraft:golden_leggings*/
            new Item(312, 0, 1){ RuntimeId=0, NetworkId=767, ExtraData = null }, /*minecraft:diamond_leggings*/
            new Item(752, 0, 1){ RuntimeId=0, NetworkId=768, ExtraData = null }, /*minecraft:netherite_scrap*/
            new Item(301, 0, 1){ RuntimeId=0, NetworkId=769, ExtraData = null }, /*minecraft:leather_boots*/
            new Item(305, 0, 1){ RuntimeId=0, NetworkId=770, ExtraData = null }, /*minecraft:chainmail_boots*/
            new Item(309, 0, 1){ RuntimeId=0, NetworkId=771, ExtraData = null }, /*minecraft:iron_boots*/
            new Item(317, 0, 1){ RuntimeId=0, NetworkId=772, ExtraData = null }, /*minecraft:golden_boots*/
            new Item(313, 0, 1){ RuntimeId=0, NetworkId=773, ExtraData = null }, /*minecraft:diamond_boots*/
            new Item(753, 0, 1){ RuntimeId=0, NetworkId=774, ExtraData = null }, /*minecraft:crimson_sign*/
            new Item(268, 0, 1){ RuntimeId=0, NetworkId=775, ExtraData = null }, /*minecraft:wooden_sword*/
            new Item(272, 0, 1){ RuntimeId=0, NetworkId=776, ExtraData = null }, /*minecraft:stone_sword*/
            new Item(267, 0, 1){ RuntimeId=0, NetworkId=777, ExtraData = null }, /*minecraft:iron_sword*/
            new Item(283, 0, 1){ RuntimeId=0, NetworkId=778, ExtraData = null }, /*minecraft:golden_sword*/
            new Item(276, 0, 1){ RuntimeId=0, NetworkId=779, ExtraData = null }, /*minecraft:diamond_sword*/
            new Item(745, 0, 1){ RuntimeId=0, NetworkId=780, ExtraData = null }, /*minecraft:netherite_pickaxe*/
            new Item(271, 0, 1){ RuntimeId=0, NetworkId=781, ExtraData = null }, /*minecraft:wooden_axe*/
            new Item(275, 0, 1){ RuntimeId=0, NetworkId=782, ExtraData = null }, /*minecraft:stone_axe*/
            new Item(258, 0, 1){ RuntimeId=0, NetworkId=783, ExtraData = null }, /*minecraft:iron_axe*/
            new Item(286, 0, 1){ RuntimeId=0, NetworkId=784, ExtraData = null }, /*minecraft:golden_axe*/
            new Item(279, 0, 1){ RuntimeId=0, NetworkId=785, ExtraData = null }, /*minecraft:diamond_axe*/
            new Item(748, 0, 1){ RuntimeId=0, NetworkId=786, ExtraData = null }, /*minecraft:netherite_helmet*/
            new Item(270, 0, 1){ RuntimeId=0, NetworkId=787, ExtraData = null }, /*minecraft:wooden_pickaxe*/
            new Item(274, 0, 1){ RuntimeId=0, NetworkId=788, ExtraData = null }, /*minecraft:stone_pickaxe*/
            new Item(257, 0, 1){ RuntimeId=0, NetworkId=789, ExtraData = null }, /*minecraft:iron_pickaxe*/
            new Item(285, 0, 1){ RuntimeId=0, NetworkId=790, ExtraData = null }, /*minecraft:golden_pickaxe*/
            new Item(278, 0, 1){ RuntimeId=0, NetworkId=791, ExtraData = null }, /*minecraft:diamond_pickaxe*/
            new Item(747, 0, 1){ RuntimeId=0, NetworkId=792, ExtraData = null }, /*minecraft:netherite_hoe*/
            new Item(269, 0, 1){ RuntimeId=0, NetworkId=793, ExtraData = null }, /*minecraft:wooden_shovel*/
            new Item(273, 0, 1){ RuntimeId=0, NetworkId=794, ExtraData = null }, /*minecraft:stone_shovel*/
            new Item(256, 0, 1){ RuntimeId=0, NetworkId=795, ExtraData = null }, /*minecraft:iron_shovel*/
            new Item(284, 0, 1){ RuntimeId=0, NetworkId=796, ExtraData = null }, /*minecraft:golden_shovel*/
            new Item(277, 0, 1){ RuntimeId=0, NetworkId=797, ExtraData = null }, /*minecraft:diamond_shovel*/
            new Item(746, 0, 1){ RuntimeId=0, NetworkId=798, ExtraData = null }, /*minecraft:netherite_axe*/
            new Item(290, 0, 1){ RuntimeId=0, NetworkId=799, ExtraData = null }, /*minecraft:wooden_hoe*/
            new Item(291, 0, 1){ RuntimeId=0, NetworkId=800, ExtraData = null }, /*minecraft:stone_hoe*/
            new Item(292, 0, 1){ RuntimeId=0, NetworkId=801, ExtraData = null }, /*minecraft:iron_hoe*/
            new Item(294, 0, 1){ RuntimeId=0, NetworkId=802, ExtraData = null }, /*minecraft:golden_hoe*/
            new Item(293, 0, 1){ RuntimeId=0, NetworkId=803, ExtraData = null }, /*minecraft:diamond_hoe*/
            new Item(749, 0, 1){ RuntimeId=0, NetworkId=804, ExtraData = null }, /*minecraft:netherite_chestplate*/
            new Item(261, 0, 1){ RuntimeId=0, NetworkId=805, ExtraData = null }, /*minecraft:bow*/
            new Item(471, 0, 1){ RuntimeId=0, NetworkId=806, ExtraData = null }, /*minecraft:crossbow*/
            new Item(262, 0, 1){ RuntimeId=0, NetworkId=807, ExtraData = null }, /*minecraft:arrow*/
            new Item(262, 6, 1){ RuntimeId=0, NetworkId=808, ExtraData = null }, /*minecraft:arrow*/
            new Item(262, 7, 1){ RuntimeId=0, NetworkId=809, ExtraData = null }, /*minecraft:arrow*/
            new Item(262, 8, 1){ RuntimeId=0, NetworkId=810, ExtraData = null }, /*minecraft:arrow*/
            new Item(262, 9, 1){ RuntimeId=0, NetworkId=811, ExtraData = null }, /*minecraft:arrow*/
            new Item(262, 10, 1){ RuntimeId=0, NetworkId=812, ExtraData = null }, /*minecraft:arrow*/
            new Item(262, 11, 1){ RuntimeId=0, NetworkId=813, ExtraData = null }, /*minecraft:arrow*/
            new Item(262, 12, 1){ RuntimeId=0, NetworkId=814, ExtraData = null }, /*minecraft:arrow*/
            new Item(262, 13, 1){ RuntimeId=0, NetworkId=815, ExtraData = null }, /*minecraft:arrow*/
            new Item(262, 14, 1){ RuntimeId=0, NetworkId=816, ExtraData = null }, /*minecraft:arrow*/
            new Item(262, 15, 1){ RuntimeId=0, NetworkId=817, ExtraData = null }, /*minecraft:arrow*/
            new Item(262, 16, 1){ RuntimeId=0, NetworkId=818, ExtraData = null }, /*minecraft:arrow*/
            new Item(262, 17, 1){ RuntimeId=0, NetworkId=819, ExtraData = null }, /*minecraft:arrow*/
            new Item(262, 18, 1){ RuntimeId=0, NetworkId=820, ExtraData = null }, /*minecraft:arrow*/
            new Item(262, 19, 1){ RuntimeId=0, NetworkId=821, ExtraData = null }, /*minecraft:arrow*/
            new Item(262, 20, 1){ RuntimeId=0, NetworkId=822, ExtraData = null }, /*minecraft:arrow*/
            new Item(262, 21, 1){ RuntimeId=0, NetworkId=823, ExtraData = null }, /*minecraft:arrow*/
            new Item(262, 22, 1){ RuntimeId=0, NetworkId=824, ExtraData = null }, /*minecraft:arrow*/
            new Item(262, 23, 1){ RuntimeId=0, NetworkId=825, ExtraData = null }, /*minecraft:arrow*/
            new Item(262, 24, 1){ RuntimeId=0, NetworkId=826, ExtraData = null }, /*minecraft:arrow*/
            new Item(262, 25, 1){ RuntimeId=0, NetworkId=827, ExtraData = null }, /*minecraft:arrow*/
            new Item(262, 26, 1){ RuntimeId=0, NetworkId=828, ExtraData = null }, /*minecraft:arrow*/
            new Item(262, 27, 1){ RuntimeId=0, NetworkId=829, ExtraData = null }, /*minecraft:arrow*/
            new Item(262, 28, 1){ RuntimeId=0, NetworkId=830, ExtraData = null }, /*minecraft:arrow*/
            new Item(262, 29, 1){ RuntimeId=0, NetworkId=831, ExtraData = null }, /*minecraft:arrow*/
            new Item(262, 30, 1){ RuntimeId=0, NetworkId=832, ExtraData = null }, /*minecraft:arrow*/
            new Item(262, 31, 1){ RuntimeId=0, NetworkId=833, ExtraData = null }, /*minecraft:arrow*/
            new Item(262, 32, 1){ RuntimeId=0, NetworkId=834, ExtraData = null }, /*minecraft:arrow*/
            new Item(262, 33, 1){ RuntimeId=0, NetworkId=835, ExtraData = null }, /*minecraft:arrow*/
            new Item(262, 34, 1){ RuntimeId=0, NetworkId=836, ExtraData = null }, /*minecraft:arrow*/
            new Item(262, 35, 1){ RuntimeId=0, NetworkId=837, ExtraData = null }, /*minecraft:arrow*/
            new Item(262, 36, 1){ RuntimeId=0, NetworkId=838, ExtraData = null }, /*minecraft:arrow*/
            new Item(262, 37, 1){ RuntimeId=0, NetworkId=839, ExtraData = null }, /*minecraft:arrow*/
            new Item(262, 38, 1){ RuntimeId=0, NetworkId=840, ExtraData = null }, /*minecraft:arrow*/
            new Item(262, 39, 1){ RuntimeId=0, NetworkId=841, ExtraData = null }, /*minecraft:arrow*/
            new Item(262, 40, 1){ RuntimeId=0, NetworkId=842, ExtraData = null }, /*minecraft:arrow*/
            new Item(262, 41, 1){ RuntimeId=0, NetworkId=843, ExtraData = null }, /*minecraft:arrow*/
            new Item(262, 42, 1){ RuntimeId=0, NetworkId=844, ExtraData = null }, /*minecraft:arrow*/
            new Item(262, 43, 1){ RuntimeId=0, NetworkId=845, ExtraData = null }, /*minecraft:arrow*/
            new Item(513, 0, 1){ RuntimeId=0, NetworkId=846, ExtraData = null }, /*minecraft:shield*/
            new Item(366, 0, 1){ RuntimeId=0, NetworkId=847, ExtraData = null }, /*minecraft:cooked_chicken*/
            new Item(320, 0, 1){ RuntimeId=0, NetworkId=848, ExtraData = null }, /*minecraft:cooked_porkchop*/
            new Item(364, 0, 1){ RuntimeId=0, NetworkId=849, ExtraData = null }, /*minecraft:cooked_beef*/
            new Item(424, 0, 1){ RuntimeId=0, NetworkId=850, ExtraData = null }, /*minecraft:cooked_mutton*/
            new Item(412, 0, 1){ RuntimeId=0, NetworkId=851, ExtraData = null }, /*minecraft:cooked_rabbit*/
            new Item(350, 0, 1){ RuntimeId=0, NetworkId=852, ExtraData = null }, /*minecraft:cooked_cod*/
            new Item(463, 0, 1){ RuntimeId=0, NetworkId=853, ExtraData = null }, /*minecraft:cooked_salmon*/
            new Item(297, 0, 1){ RuntimeId=0, NetworkId=854, ExtraData = null }, /*minecraft:bread*/
            new Item(282, 0, 1){ RuntimeId=0, NetworkId=855, ExtraData = null }, /*minecraft:mushroom_stew*/
            new Item(459, 0, 1){ RuntimeId=0, NetworkId=856, ExtraData = null }, /*minecraft:beetroot_soup*/
            new Item(413, 0, 1){ RuntimeId=0, NetworkId=857, ExtraData = null }, /*minecraft:rabbit_stew*/
            new Item(393, 0, 1){ RuntimeId=0, NetworkId=858, ExtraData = null }, /*minecraft:baked_potato*/
            new Item(357, 0, 1){ RuntimeId=0, NetworkId=859, ExtraData = null }, /*minecraft:cookie*/
            new Item(400, 0, 1){ RuntimeId=0, NetworkId=860, ExtraData = null }, /*minecraft:pumpkin_pie*/
            new Item(354, 0, 1){ RuntimeId=0, NetworkId=861, ExtraData = null }, /*minecraft:cake*/
            new Item(464, 0, 1){ RuntimeId=0, NetworkId=862, ExtraData = null }, /*minecraft:dried_kelp*/
            new Item(346, 0, 1){ RuntimeId=0, NetworkId=863, ExtraData = null }, /*minecraft:fishing_rod*/
            new Item(398, 0, 1){ RuntimeId=0, NetworkId=864, ExtraData = null }, /*minecraft:carrot_on_a_stick*/
            new Item(759, 0, 1){ RuntimeId=0, NetworkId=865, ExtraData = null }, /*minecraft:music_disc_pigstep*/
            new Item(332, 0, 1){ RuntimeId=0, NetworkId=866, ExtraData = null }, /*minecraft:snowball*/
            new Item(359, 0, 1){ RuntimeId=0, NetworkId=867, ExtraData = null }, /*minecraft:shears*/
            new Item(259, 0, 1){ RuntimeId=0, NetworkId=868, ExtraData = null }, /*minecraft:flint_and_steel*/
            new Item(420, 0, 1){ RuntimeId=0, NetworkId=869, ExtraData = null }, /*minecraft:lead*/
            new Item(347, 0, 1){ RuntimeId=0, NetworkId=870, ExtraData = null }, /*minecraft:clock*/
            new Item(345, 0, 1){ RuntimeId=0, NetworkId=871, ExtraData = null }, /*minecraft:compass*/
            new Item(646, 0, 1){ RuntimeId=0, NetworkId=872, ExtraData = null }, /**/
            new Item(434, 0, 1){ RuntimeId=0, NetworkId=873, ExtraData = null }, /*minecraft:banner_pattern*/
            new Item(434, 1, 1){ RuntimeId=0, NetworkId=874, ExtraData = null }, /*minecraft:banner_pattern*/
            new Item(434, 2, 1){ RuntimeId=0, NetworkId=875, ExtraData = null }, /*minecraft:banner_pattern*/
            new Item(434, 3, 1){ RuntimeId=0, NetworkId=876, ExtraData = null }, /*minecraft:banner_pattern*/
            new Item(434, 4, 1){ RuntimeId=0, NetworkId=877, ExtraData = null }, /*minecraft:banner_pattern*/
            new Item(434, 5, 1){ RuntimeId=0, NetworkId=878, ExtraData = null }, /*minecraft:banner_pattern*/
            new Item(434, 6, 1){ RuntimeId=0, NetworkId=879, ExtraData = null }, /*minecraft:banner_pattern*/
            new Item(434, 7, 1){ RuntimeId=0, NetworkId=880, ExtraData = null }, /*minecraft:banner_pattern*/
            new Item(395, 0, 1){ RuntimeId=0, NetworkId=881, ExtraData = null }, /*minecraft:empty_map*/
            new Item(395, 2, 1){ RuntimeId=0, NetworkId=882, ExtraData = null }, /*minecraft:empty_map*/
            new Item(329, 0, 1){ RuntimeId=0, NetworkId=883, ExtraData = null }, /*minecraft:saddle*/
            new Item(0, 0, 0){ RuntimeId=0, NetworkId=884, ExtraData = null }, /*minecraft:air*/
            new Item(416, 0, 1){ RuntimeId=0, NetworkId=885, ExtraData = null }, /*minecraft:leather_horse_armor*/
            new Item(417, 0, 1){ RuntimeId=0, NetworkId=886, ExtraData = null }, /*minecraft:iron_horse_armor*/
            new Item(417, 0, 1){ RuntimeId=0, NetworkId=887, ExtraData = null }, /*minecraft:golden_horse_armor*/
            new Item(419, 0, 1){ RuntimeId=0, NetworkId=888, ExtraData = null }, /*minecraft:diamond_horse_armor*/
            new Item(455, 0, 1){ RuntimeId=0, NetworkId=889, ExtraData = null }, /*minecraft:trident*/
            new Item(469, 0, 1){ RuntimeId=0, NetworkId=890, ExtraData = null }, /*minecraft:turtle_helmet*/
            new Item(444, 0, 1){ RuntimeId=0, NetworkId=891, ExtraData = null }, /*minecraft:elytra*/
            new Item(450, 0, 1){ RuntimeId=0, NetworkId=892, ExtraData = null }, /*minecraft:totem_of_undying*/
            new Item(374, 0, 1){ RuntimeId=0, NetworkId=893, ExtraData = null }, /*minecraft:glass_bottle*/
            new Item(384, 0, 1){ RuntimeId=0, NetworkId=894, ExtraData = null }, /*minecraft:experience_bottle*/
            new Item(373, 0, 1){ RuntimeId=0, NetworkId=895, ExtraData = null }, /*minecraft:potion*/
            new Item(373, 1, 1){ RuntimeId=0, NetworkId=896, ExtraData = null }, /*minecraft:potion*/
            new Item(373, 2, 1){ RuntimeId=0, NetworkId=897, ExtraData = null }, /*minecraft:potion*/
            new Item(373, 3, 1){ RuntimeId=0, NetworkId=898, ExtraData = null }, /*minecraft:potion*/
            new Item(373, 4, 1){ RuntimeId=0, NetworkId=899, ExtraData = null }, /*minecraft:potion*/
            new Item(373, 5, 1){ RuntimeId=0, NetworkId=900, ExtraData = null }, /*minecraft:potion*/
            new Item(373, 6, 1){ RuntimeId=0, NetworkId=901, ExtraData = null }, /*minecraft:potion*/
            new Item(373, 7, 1){ RuntimeId=0, NetworkId=902, ExtraData = null }, /*minecraft:potion*/
            new Item(373, 8, 1){ RuntimeId=0, NetworkId=903, ExtraData = null }, /*minecraft:potion*/
            new Item(373, 9, 1){ RuntimeId=0, NetworkId=904, ExtraData = null }, /*minecraft:potion*/
            new Item(373, 10, 1){ RuntimeId=0, NetworkId=905, ExtraData = null }, /*minecraft:potion*/
            new Item(373, 11, 1){ RuntimeId=0, NetworkId=906, ExtraData = null }, /*minecraft:potion*/
            new Item(373, 12, 1){ RuntimeId=0, NetworkId=907, ExtraData = null }, /*minecraft:potion*/
            new Item(373, 13, 1){ RuntimeId=0, NetworkId=908, ExtraData = null }, /*minecraft:potion*/
            new Item(373, 14, 1){ RuntimeId=0, NetworkId=909, ExtraData = null }, /*minecraft:potion*/
            new Item(373, 15, 1){ RuntimeId=0, NetworkId=910, ExtraData = null }, /*minecraft:potion*/
            new Item(373, 16, 1){ RuntimeId=0, NetworkId=911, ExtraData = null }, /*minecraft:potion*/
            new Item(373, 17, 1){ RuntimeId=0, NetworkId=912, ExtraData = null }, /*minecraft:potion*/
            new Item(373, 18, 1){ RuntimeId=0, NetworkId=913, ExtraData = null }, /*minecraft:potion*/
            new Item(373, 19, 1){ RuntimeId=0, NetworkId=914, ExtraData = null }, /*minecraft:potion*/
            new Item(373, 20, 1){ RuntimeId=0, NetworkId=915, ExtraData = null }, /*minecraft:potion*/
            new Item(373, 21, 1){ RuntimeId=0, NetworkId=916, ExtraData = null }, /*minecraft:potion*/
            new Item(373, 22, 1){ RuntimeId=0, NetworkId=917, ExtraData = null }, /*minecraft:potion*/
            new Item(373, 23, 1){ RuntimeId=0, NetworkId=918, ExtraData = null }, /*minecraft:potion*/
            new Item(373, 24, 1){ RuntimeId=0, NetworkId=919, ExtraData = null }, /*minecraft:potion*/
            new Item(373, 25, 1){ RuntimeId=0, NetworkId=920, ExtraData = null }, /*minecraft:potion*/
            new Item(373, 26, 1){ RuntimeId=0, NetworkId=921, ExtraData = null }, /*minecraft:potion*/
            new Item(373, 27, 1){ RuntimeId=0, NetworkId=922, ExtraData = null }, /*minecraft:potion*/
            new Item(373, 28, 1){ RuntimeId=0, NetworkId=923, ExtraData = null }, /*minecraft:potion*/
            new Item(373, 29, 1){ RuntimeId=0, NetworkId=924, ExtraData = null }, /*minecraft:potion*/
            new Item(373, 30, 1){ RuntimeId=0, NetworkId=925, ExtraData = null }, /*minecraft:potion*/
            new Item(373, 31, 1){ RuntimeId=0, NetworkId=926, ExtraData = null }, /*minecraft:potion*/
            new Item(373, 32, 1){ RuntimeId=0, NetworkId=927, ExtraData = null }, /*minecraft:potion*/
            new Item(373, 33, 1){ RuntimeId=0, NetworkId=928, ExtraData = null }, /*minecraft:potion*/
            new Item(373, 34, 1){ RuntimeId=0, NetworkId=929, ExtraData = null }, /*minecraft:potion*/
            new Item(373, 35, 1){ RuntimeId=0, NetworkId=930, ExtraData = null }, /*minecraft:potion*/
            new Item(373, 36, 1){ RuntimeId=0, NetworkId=931, ExtraData = null }, /*minecraft:potion*/
            new Item(373, 37, 1){ RuntimeId=0, NetworkId=932, ExtraData = null }, /*minecraft:potion*/
            new Item(373, 38, 1){ RuntimeId=0, NetworkId=933, ExtraData = null }, /*minecraft:potion*/
            new Item(373, 39, 1){ RuntimeId=0, NetworkId=934, ExtraData = null }, /*minecraft:potion*/
            new Item(373, 40, 1){ RuntimeId=0, NetworkId=935, ExtraData = null }, /*minecraft:potion*/
            new Item(373, 41, 1){ RuntimeId=0, NetworkId=936, ExtraData = null }, /*minecraft:potion*/
            new Item(373, 42, 1){ RuntimeId=0, NetworkId=937, ExtraData = null }, /*minecraft:potion*/
            new Item(438, 0, 1){ RuntimeId=0, NetworkId=938, ExtraData = null }, /*minecraft:splash_potion*/
            new Item(438, 1, 1){ RuntimeId=0, NetworkId=939, ExtraData = null }, /*minecraft:splash_potion*/
            new Item(438, 2, 1){ RuntimeId=0, NetworkId=940, ExtraData = null }, /*minecraft:splash_potion*/
            new Item(438, 3, 1){ RuntimeId=0, NetworkId=941, ExtraData = null }, /*minecraft:splash_potion*/
            new Item(438, 4, 1){ RuntimeId=0, NetworkId=942, ExtraData = null }, /*minecraft:splash_potion*/
            new Item(438, 5, 1){ RuntimeId=0, NetworkId=943, ExtraData = null }, /*minecraft:splash_potion*/
            new Item(438, 6, 1){ RuntimeId=0, NetworkId=944, ExtraData = null }, /*minecraft:splash_potion*/
            new Item(438, 7, 1){ RuntimeId=0, NetworkId=945, ExtraData = null }, /*minecraft:splash_potion*/
            new Item(438, 8, 1){ RuntimeId=0, NetworkId=946, ExtraData = null }, /*minecraft:splash_potion*/
            new Item(438, 9, 1){ RuntimeId=0, NetworkId=947, ExtraData = null }, /*minecraft:splash_potion*/
            new Item(438, 10, 1){ RuntimeId=0, NetworkId=948, ExtraData = null }, /*minecraft:splash_potion*/
            new Item(438, 11, 1){ RuntimeId=0, NetworkId=949, ExtraData = null }, /*minecraft:splash_potion*/
            new Item(438, 12, 1){ RuntimeId=0, NetworkId=950, ExtraData = null }, /*minecraft:splash_potion*/
            new Item(438, 13, 1){ RuntimeId=0, NetworkId=951, ExtraData = null }, /*minecraft:splash_potion*/
            new Item(438, 14, 1){ RuntimeId=0, NetworkId=952, ExtraData = null }, /*minecraft:splash_potion*/
            new Item(438, 15, 1){ RuntimeId=0, NetworkId=953, ExtraData = null }, /*minecraft:splash_potion*/
            new Item(438, 16, 1){ RuntimeId=0, NetworkId=954, ExtraData = null }, /*minecraft:splash_potion*/
            new Item(438, 17, 1){ RuntimeId=0, NetworkId=955, ExtraData = null }, /*minecraft:splash_potion*/
            new Item(438, 18, 1){ RuntimeId=0, NetworkId=956, ExtraData = null }, /*minecraft:splash_potion*/
            new Item(438, 19, 1){ RuntimeId=0, NetworkId=957, ExtraData = null }, /*minecraft:splash_potion*/
            new Item(438, 20, 1){ RuntimeId=0, NetworkId=958, ExtraData = null }, /*minecraft:splash_potion*/
            new Item(438, 21, 1){ RuntimeId=0, NetworkId=959, ExtraData = null }, /*minecraft:splash_potion*/
            new Item(438, 22, 1){ RuntimeId=0, NetworkId=960, ExtraData = null }, /*minecraft:splash_potion*/
            new Item(438, 23, 1){ RuntimeId=0, NetworkId=961, ExtraData = null }, /*minecraft:splash_potion*/
            new Item(438, 24, 1){ RuntimeId=0, NetworkId=962, ExtraData = null }, /*minecraft:splash_potion*/
            new Item(438, 25, 1){ RuntimeId=0, NetworkId=963, ExtraData = null }, /*minecraft:splash_potion*/
            new Item(438, 26, 1){ RuntimeId=0, NetworkId=964, ExtraData = null }, /*minecraft:splash_potion*/
            new Item(438, 27, 1){ RuntimeId=0, NetworkId=965, ExtraData = null }, /*minecraft:splash_potion*/
            new Item(438, 28, 1){ RuntimeId=0, NetworkId=966, ExtraData = null }, /*minecraft:splash_potion*/
            new Item(438, 29, 1){ RuntimeId=0, NetworkId=967, ExtraData = null }, /*minecraft:splash_potion*/
            new Item(438, 30, 1){ RuntimeId=0, NetworkId=968, ExtraData = null }, /*minecraft:splash_potion*/
            new Item(438, 31, 1){ RuntimeId=0, NetworkId=969, ExtraData = null }, /*minecraft:splash_potion*/
            new Item(438, 32, 1){ RuntimeId=0, NetworkId=970, ExtraData = null }, /*minecraft:splash_potion*/
            new Item(438, 33, 1){ RuntimeId=0, NetworkId=971, ExtraData = null }, /*minecraft:splash_potion*/
            new Item(438, 34, 1){ RuntimeId=0, NetworkId=972, ExtraData = null }, /*minecraft:splash_potion*/
            new Item(438, 35, 1){ RuntimeId=0, NetworkId=973, ExtraData = null }, /*minecraft:splash_potion*/
            new Item(438, 36, 1){ RuntimeId=0, NetworkId=974, ExtraData = null }, /*minecraft:splash_potion*/
            new Item(438, 37, 1){ RuntimeId=0, NetworkId=975, ExtraData = null }, /*minecraft:splash_potion*/
            new Item(438, 38, 1){ RuntimeId=0, NetworkId=976, ExtraData = null }, /*minecraft:splash_potion*/
            new Item(438, 39, 1){ RuntimeId=0, NetworkId=977, ExtraData = null }, /*minecraft:splash_potion*/
            new Item(438, 40, 1){ RuntimeId=0, NetworkId=978, ExtraData = null }, /*minecraft:splash_potion*/
            new Item(438, 41, 1){ RuntimeId=0, NetworkId=979, ExtraData = null }, /*minecraft:splash_potion*/
            new Item(438, 42, 1){ RuntimeId=0, NetworkId=980, ExtraData = null }, /*minecraft:splash_potion*/
            new Item(441, 0, 1){ RuntimeId=0, NetworkId=981, ExtraData = null }, /*minecraft:lingering_potion*/
            new Item(441, 1, 1){ RuntimeId=0, NetworkId=982, ExtraData = null }, /*minecraft:lingering_potion*/
            new Item(441, 2, 1){ RuntimeId=0, NetworkId=983, ExtraData = null }, /*minecraft:lingering_potion*/
            new Item(441, 3, 1){ RuntimeId=0, NetworkId=984, ExtraData = null }, /*minecraft:lingering_potion*/
            new Item(441, 4, 1){ RuntimeId=0, NetworkId=985, ExtraData = null }, /*minecraft:lingering_potion*/
            new Item(441, 5, 1){ RuntimeId=0, NetworkId=986, ExtraData = null }, /*minecraft:lingering_potion*/
            new Item(441, 6, 1){ RuntimeId=0, NetworkId=987, ExtraData = null }, /*minecraft:lingering_potion*/
            new Item(441, 7, 1){ RuntimeId=0, NetworkId=988, ExtraData = null }, /*minecraft:lingering_potion*/
            new Item(441, 8, 1){ RuntimeId=0, NetworkId=989, ExtraData = null }, /*minecraft:lingering_potion*/
            new Item(441, 9, 1){ RuntimeId=0, NetworkId=990, ExtraData = null }, /*minecraft:lingering_potion*/
            new Item(441, 10, 1){ RuntimeId=0, NetworkId=991, ExtraData = null }, /*minecraft:lingering_potion*/
            new Item(441, 11, 1){ RuntimeId=0, NetworkId=992, ExtraData = null }, /*minecraft:lingering_potion*/
            new Item(441, 12, 1){ RuntimeId=0, NetworkId=993, ExtraData = null }, /*minecraft:lingering_potion*/
            new Item(441, 13, 1){ RuntimeId=0, NetworkId=994, ExtraData = null }, /*minecraft:lingering_potion*/
            new Item(441, 14, 1){ RuntimeId=0, NetworkId=995, ExtraData = null }, /*minecraft:lingering_potion*/
            new Item(441, 15, 1){ RuntimeId=0, NetworkId=996, ExtraData = null }, /*minecraft:lingering_potion*/
            new Item(441, 16, 1){ RuntimeId=0, NetworkId=997, ExtraData = null }, /*minecraft:lingering_potion*/
            new Item(441, 17, 1){ RuntimeId=0, NetworkId=998, ExtraData = null }, /*minecraft:lingering_potion*/
            new Item(441, 18, 1){ RuntimeId=0, NetworkId=999, ExtraData = null }, /*minecraft:lingering_potion*/
            new Item(441, 19, 1){ RuntimeId=0, NetworkId=1000, ExtraData = null }, /*minecraft:lingering_potion*/
            new Item(441, 20, 1){ RuntimeId=0, NetworkId=1001, ExtraData = null }, /*minecraft:lingering_potion*/
            new Item(441, 21, 1){ RuntimeId=0, NetworkId=1002, ExtraData = null }, /*minecraft:lingering_potion*/
            new Item(441, 22, 1){ RuntimeId=0, NetworkId=1003, ExtraData = null }, /*minecraft:lingering_potion*/
            new Item(441, 23, 1){ RuntimeId=0, NetworkId=1004, ExtraData = null }, /*minecraft:lingering_potion*/
            new Item(441, 24, 1){ RuntimeId=0, NetworkId=1005, ExtraData = null }, /*minecraft:lingering_potion*/
            new Item(441, 25, 1){ RuntimeId=0, NetworkId=1006, ExtraData = null }, /*minecraft:lingering_potion*/
            new Item(441, 26, 1){ RuntimeId=0, NetworkId=1007, ExtraData = null }, /*minecraft:lingering_potion*/
            new Item(441, 27, 1){ RuntimeId=0, NetworkId=1008, ExtraData = null }, /*minecraft:lingering_potion*/
            new Item(441, 28, 1){ RuntimeId=0, NetworkId=1009, ExtraData = null }, /*minecraft:lingering_potion*/
            new Item(441, 29, 1){ RuntimeId=0, NetworkId=1010, ExtraData = null }, /*minecraft:lingering_potion*/
            new Item(441, 30, 1){ RuntimeId=0, NetworkId=1011, ExtraData = null }, /*minecraft:lingering_potion*/
            new Item(441, 31, 1){ RuntimeId=0, NetworkId=1012, ExtraData = null }, /*minecraft:lingering_potion*/
            new Item(441, 32, 1){ RuntimeId=0, NetworkId=1013, ExtraData = null }, /*minecraft:lingering_potion*/
            new Item(441, 33, 1){ RuntimeId=0, NetworkId=1014, ExtraData = null }, /*minecraft:lingering_potion*/
            new Item(441, 34, 1){ RuntimeId=0, NetworkId=1015, ExtraData = null }, /*minecraft:lingering_potion*/
            new Item(441, 35, 1){ RuntimeId=0, NetworkId=1016, ExtraData = null }, /*minecraft:lingering_potion*/
            new Item(441, 36, 1){ RuntimeId=0, NetworkId=1017, ExtraData = null }, /*minecraft:lingering_potion*/
            new Item(441, 37, 1){ RuntimeId=0, NetworkId=1018, ExtraData = null }, /*minecraft:lingering_potion*/
            new Item(441, 38, 1){ RuntimeId=0, NetworkId=1019, ExtraData = null }, /*minecraft:lingering_potion*/
            new Item(441, 39, 1){ RuntimeId=0, NetworkId=1020, ExtraData = null }, /*minecraft:lingering_potion*/
            new Item(441, 40, 1){ RuntimeId=0, NetworkId=1021, ExtraData = null }, /*minecraft:lingering_potion*/
            new Item(441, 41, 1){ RuntimeId=0, NetworkId=1022, ExtraData = null }, /*minecraft:lingering_potion*/
            new Item(441, 42, 1){ RuntimeId=0, NetworkId=1023, ExtraData = null }, /*minecraft:lingering_potion*/
            new Item(333, 0, 1){ RuntimeId=0, NetworkId=1024, ExtraData = null }, /*minecraft:boat*/
            new Item(280, 0, 1){ RuntimeId=0, NetworkId=1025, ExtraData = null }, /*minecraft:stick*/
            new Item(355, 0, 1){ RuntimeId=0, NetworkId=1026, ExtraData = null }, /*minecraft:bed*/
            new Item(355, 8, 1){ RuntimeId=0, NetworkId=1027, ExtraData = null }, /*minecraft:bed*/
            new Item(355, 7, 1){ RuntimeId=0, NetworkId=1028, ExtraData = null }, /*minecraft:bed*/
            new Item(355, 15, 1){ RuntimeId=0, NetworkId=1029, ExtraData = null }, /*minecraft:bed*/
            new Item(355, 12, 1){ RuntimeId=0, NetworkId=1030, ExtraData = null }, /*minecraft:bed*/
            new Item(355, 14, 1){ RuntimeId=0, NetworkId=1031, ExtraData = null }, /*minecraft:bed*/
            new Item(355, 1, 1){ RuntimeId=0, NetworkId=1032, ExtraData = null }, /*minecraft:bed*/
            new Item(355, 4, 1){ RuntimeId=0, NetworkId=1033, ExtraData = null }, /*minecraft:bed*/
            new Item(355, 5, 1){ RuntimeId=0, NetworkId=1034, ExtraData = null }, /*minecraft:bed*/
            new Item(355, 13, 1){ RuntimeId=0, NetworkId=1035, ExtraData = null }, /*minecraft:bed*/
            new Item(355, 9, 1){ RuntimeId=0, NetworkId=1036, ExtraData = null }, /*minecraft:bed*/
            new Item(355, 3, 1){ RuntimeId=0, NetworkId=1037, ExtraData = null }, /*minecraft:bed*/
            new Item(355, 11, 1){ RuntimeId=0, NetworkId=1038, ExtraData = null }, /*minecraft:bed*/
            new Item(355, 10, 1){ RuntimeId=0, NetworkId=1039, ExtraData = null }, /*minecraft:bed*/
            new Item(355, 2, 1){ RuntimeId=0, NetworkId=1040, ExtraData = null }, /*minecraft:bed*/
            new Item(355, 6, 1){ RuntimeId=0, NetworkId=1041, ExtraData = null }, /*minecraft:bed*/
            new Item(50, 0, 1){ RuntimeId=724, NetworkId=1042, ExtraData = null }, /*minecraft:torch*/
            new Item(-268, 0, 1){ RuntimeId=4644, NetworkId=1043, ExtraData = null }, /*minecraft:soul_torch*/
            new Item(-156, 0, 1){ RuntimeId=5855, NetworkId=1044, ExtraData = null }, /*minecraft:sea_pickle*/
            new Item(-208, 0, 1){ RuntimeId=7074, NetworkId=1045, ExtraData = null }, /*minecraft:lantern*/
            new Item(-269, 0, 1){ RuntimeId=5749, NetworkId=1046, ExtraData = null }, /*minecraft:soul_lantern*/
            new Item(-412, 0, 1){ RuntimeId=7403, NetworkId=1047, ExtraData = null }, /**/
            new Item(-413, 0, 1){ RuntimeId=5300, NetworkId=1048, ExtraData = null }, /**/
            new Item(-414, 0, 1){ RuntimeId=362, NetworkId=1049, ExtraData = null }, /**/
            new Item(-415, 0, 1){ RuntimeId=418, NetworkId=1050, ExtraData = null }, /**/
            new Item(-416, 0, 1){ RuntimeId=4569, NetworkId=1051, ExtraData = null }, /**/
            new Item(-417, 0, 1){ RuntimeId=6192, NetworkId=1052, ExtraData = null }, /**/
            new Item(-418, 0, 1){ RuntimeId=6368, NetworkId=1053, ExtraData = null }, /**/
            new Item(-419, 0, 1){ RuntimeId=7370, NetworkId=1054, ExtraData = null }, /**/
            new Item(-420, 0, 1){ RuntimeId=939, NetworkId=1055, ExtraData = null }, /**/
            new Item(-421, 0, 1){ RuntimeId=6224, NetworkId=1056, ExtraData = null }, /**/
            new Item(-422, 0, 1){ RuntimeId=7726, NetworkId=1057, ExtraData = null }, /**/
            new Item(-423, 0, 1){ RuntimeId=7038, NetworkId=1058, ExtraData = null }, /**/
            new Item(-424, 0, 1){ RuntimeId=0, NetworkId=1059, ExtraData = null }, /**/
            new Item(-425, 0, 1){ RuntimeId=5875, NetworkId=1060, ExtraData = null }, /**/
            new Item(-426, 0, 1){ RuntimeId=686, NetworkId=1061, ExtraData = null }, /**/
            new Item(-427, 0, 1){ RuntimeId=4681, NetworkId=1062, ExtraData = null }, /**/
            new Item(-428, 0, 1){ RuntimeId=171, NetworkId=1063, ExtraData = null }, /**/
            new Item(58, 0, 1){ RuntimeId=5854, NetworkId=1064, ExtraData = null }, /*minecraft:crafting_table*/
            new Item(-200, 0, 1){ RuntimeId=8288, NetworkId=1065, ExtraData = null }, /*minecraft:cartography_table*/
            new Item(-201, 0, 1){ RuntimeId=5833, NetworkId=1066, ExtraData = null }, /*minecraft:fletching_table*/
            new Item(-202, 0, 1){ RuntimeId=3726, NetworkId=1067, ExtraData = null }, /*minecraft:smithing_table*/
            new Item(-219, 0, 1){ RuntimeId=6108, NetworkId=1068, ExtraData = null }, /*minecraft:beehive*/
            new Item(734, 0, 1){ RuntimeId=0, NetworkId=1069, ExtraData = null }, /*minecraft:suspicious_stew*/
            new Item(622, 0, 1){ RuntimeId=0, NetworkId=1070, ExtraData = null }, /**/
            new Item(61, 0, 1){ RuntimeId=7802, NetworkId=1071, ExtraData = null }, /*minecraft:furnace*/
            new Item(-196, 0, 1){ RuntimeId=7567, NetworkId=1072, ExtraData = null }, /*minecraft:blast_furnace*/
            new Item(-198, 0, 1){ RuntimeId=647, NetworkId=1073, ExtraData = null }, /*minecraft:smoker*/
            new Item(-272, 0, 1){ RuntimeId=681, NetworkId=1074, ExtraData = null }, /*minecraft:respawn_anchor*/
            new Item(379, 0, 1){ RuntimeId=0, NetworkId=1075, ExtraData = null }, /*minecraft:brewing_stand*/
            new Item(145, 0, 1){ RuntimeId=6634, NetworkId=1076, ExtraData = null }, /*minecraft:anvil*/
            new Item(145, 0, 1){ RuntimeId=6638, NetworkId=1077, ExtraData = null }, /*minecraft:anvil*/
            new Item(145, 0, 1){ RuntimeId=6642, NetworkId=1078, ExtraData = null }, /*minecraft:anvil*/
            new Item(-195, 0, 1){ RuntimeId=8039, NetworkId=1079, ExtraData = null }, /*minecraft:grindstone*/
            new Item(116, 0, 1){ RuntimeId=6723, NetworkId=1080, ExtraData = null }, /*minecraft:enchanting_table*/
            new Item(47, 0, 1){ RuntimeId=6671, NetworkId=1081, ExtraData = null }, /*minecraft:bookshelf*/
            new Item(-194, 0, 1){ RuntimeId=6940, NetworkId=1082, ExtraData = null }, /*minecraft:lectern*/
            new Item(380, 0, 1){ RuntimeId=0, NetworkId=1083, ExtraData = null }, /*minecraft:cauldron*/
            new Item(-213, 0, 1){ RuntimeId=5415, NetworkId=1084, ExtraData = null }, /*minecraft:composter*/
            new Item(54, 0, 1){ RuntimeId=7115, NetworkId=1085, ExtraData = null }, /*minecraft:chest*/
            new Item(146, 0, 1){ RuntimeId=5583, NetworkId=1086, ExtraData = null }, /*minecraft:trapped_chest*/
            new Item(130, 0, 1){ RuntimeId=4369, NetworkId=1087, ExtraData = null }, /*minecraft:ender_chest*/
            new Item(-203, 0, 1){ RuntimeId=4518, NetworkId=1088, ExtraData = null }, /*minecraft:barrel*/
            new Item(205, 0, 1){ RuntimeId=3681, NetworkId=1089, ExtraData = null }, /*minecraft:undyed_shulker_box*/
            new Item(218, 0, 1){ RuntimeId=5316, NetworkId=1090, ExtraData = null }, /*minecraft:shulker_box*/
            new Item(218, 0, 1){ RuntimeId=5324, NetworkId=1091, ExtraData = null }, /*minecraft:shulker_box*/
            new Item(218, 0, 1){ RuntimeId=5323, NetworkId=1092, ExtraData = null }, /*minecraft:shulker_box*/
            new Item(218, 0, 1){ RuntimeId=5331, NetworkId=1093, ExtraData = null }, /*minecraft:shulker_box*/
            new Item(218, 0, 1){ RuntimeId=5328, NetworkId=1094, ExtraData = null }, /*minecraft:shulker_box*/
            new Item(218, 0, 1){ RuntimeId=5330, NetworkId=1095, ExtraData = null }, /*minecraft:shulker_box*/
            new Item(218, 0, 1){ RuntimeId=5317, NetworkId=1096, ExtraData = null }, /*minecraft:shulker_box*/
            new Item(218, 0, 1){ RuntimeId=5320, NetworkId=1097, ExtraData = null }, /*minecraft:shulker_box*/
            new Item(218, 0, 1){ RuntimeId=5321, NetworkId=1098, ExtraData = null }, /*minecraft:shulker_box*/
            new Item(218, 0, 1){ RuntimeId=5329, NetworkId=1099, ExtraData = null }, /*minecraft:shulker_box*/
            new Item(218, 0, 1){ RuntimeId=5325, NetworkId=1100, ExtraData = null }, /*minecraft:shulker_box*/
            new Item(218, 0, 1){ RuntimeId=5319, NetworkId=1101, ExtraData = null }, /*minecraft:shulker_box*/
            new Item(218, 0, 1){ RuntimeId=5327, NetworkId=1102, ExtraData = null }, /*minecraft:shulker_box*/
            new Item(218, 0, 1){ RuntimeId=5326, NetworkId=1103, ExtraData = null }, /*minecraft:shulker_box*/
            new Item(218, 0, 1){ RuntimeId=5318, NetworkId=1104, ExtraData = null }, /*minecraft:shulker_box*/
            new Item(218, 0, 1){ RuntimeId=5322, NetworkId=1105, ExtraData = null }, /*minecraft:shulker_box*/
            new Item(425, 0, 1){ RuntimeId=0, NetworkId=1106, ExtraData = null }, /*minecraft:armor_stand*/
            new Item(25, 0, 1){ RuntimeId=346, NetworkId=1107, ExtraData = null }, /*minecraft:noteblock*/
            new Item(84, 0, 1){ RuntimeId=4874, NetworkId=1108, ExtraData = null }, /*minecraft:jukebox*/
            new Item(500, 0, 1){ RuntimeId=0, NetworkId=1109, ExtraData = null }, /*minecraft:music_disc_13*/
            new Item(501, 0, 1){ RuntimeId=0, NetworkId=1110, ExtraData = null }, /*minecraft:music_disc_cat*/
            new Item(502, 0, 1){ RuntimeId=0, NetworkId=1111, ExtraData = null }, /*minecraft:music_disc_blocks*/
            new Item(503, 0, 1){ RuntimeId=0, NetworkId=1112, ExtraData = null }, /*minecraft:music_disc_chirp*/
            new Item(504, 0, 1){ RuntimeId=0, NetworkId=1113, ExtraData = null }, /*minecraft:music_disc_far*/
            new Item(505, 0, 1){ RuntimeId=0, NetworkId=1114, ExtraData = null }, /*minecraft:music_disc_mall*/
            new Item(506, 0, 1){ RuntimeId=0, NetworkId=1115, ExtraData = null }, /*minecraft:music_disc_mellohi*/
            new Item(507, 0, 1){ RuntimeId=0, NetworkId=1116, ExtraData = null }, /*minecraft:music_disc_stal*/
            new Item(508, 0, 1){ RuntimeId=0, NetworkId=1117, ExtraData = null }, /*minecraft:music_disc_strad*/
            new Item(509, 0, 1){ RuntimeId=0, NetworkId=1118, ExtraData = null }, /*minecraft:music_disc_ward*/
            new Item(510, 0, 1){ RuntimeId=0, NetworkId=1119, ExtraData = null }, /*minecraft:music_disc_11*/
            new Item(511, 0, 1){ RuntimeId=0, NetworkId=1120, ExtraData = null }, /*minecraft:music_disc_wait*/
            new Item(351, 0, 1){ RuntimeId=0, NetworkId=1121, ExtraData = null }, /*minecraft:dye*/
            new Item(636, 0, 1){ RuntimeId=0, NetworkId=1122, ExtraData = null }, /**/
            new Item(801, 0, 1){ RuntimeId=0, NetworkId=1123, ExtraData = null }, /*minecraft:soul_campfire*/
            new Item(637, 0, 1){ RuntimeId=0, NetworkId=1124, ExtraData = null }, /**/
            new Item(348, 0, 1){ RuntimeId=0, NetworkId=1125, ExtraData = null }, /*minecraft:glowstone_dust*/
            new Item(89, 0, 1){ RuntimeId=3885, NetworkId=1126, ExtraData = null }, /*minecraft:glowstone*/
            new Item(123, 0, 1){ RuntimeId=251, NetworkId=1127, ExtraData = null }, /*minecraft:redstone_lamp*/
            new Item(169, 0, 1){ RuntimeId=7546, NetworkId=1128, ExtraData = null }, /*minecraft:seaLantern*/
            new Item(323, 0, 1){ RuntimeId=0, NetworkId=1129, ExtraData = null }, /*minecraft:oak_sign*/
            new Item(472, 0, 1){ RuntimeId=0, NetworkId=1130, ExtraData = null }, /*minecraft:spruce_sign*/
            new Item(473, 0, 1){ RuntimeId=0, NetworkId=1131, ExtraData = null }, /*minecraft:birch_sign*/
            new Item(474, 0, 1){ RuntimeId=0, NetworkId=1132, ExtraData = null }, /*minecraft:jungle_sign*/
            new Item(475, 0, 1){ RuntimeId=0, NetworkId=1133, ExtraData = null }, /*minecraft:acacia_sign*/
            new Item(476, 0, 1){ RuntimeId=0, NetworkId=1134, ExtraData = null }, /*minecraft:dark_oak_sign*/
            new Item(634, 0, 1){ RuntimeId=0, NetworkId=1135, ExtraData = null }, /**/
            new Item(755, 0, 1){ RuntimeId=0, NetworkId=1136, ExtraData = null }, /*minecraft:crimson_door*/
            new Item(756, 0, 1){ RuntimeId=0, NetworkId=1137, ExtraData = null }, /*minecraft:warped_door*/
            new Item(321, 0, 1){ RuntimeId=0, NetworkId=1138, ExtraData = null }, /*minecraft:painting*/
            new Item(389, 0, 1){ RuntimeId=0, NetworkId=1139, ExtraData = null }, /*minecraft:frame*/
            new Item(623, 0, 1){ RuntimeId=0, NetworkId=1140, ExtraData = null }, /**/
            new Item(498, 0, 1){ RuntimeId=0, NetworkId=1141, ExtraData = null }, /*minecraft:camera*/
            new Item(390, 0, 1){ RuntimeId=0, NetworkId=1142, ExtraData = null }, /*minecraft:flower_pot*/
            new Item(281, 0, 1){ RuntimeId=0, NetworkId=1143, ExtraData = null }, /*minecraft:bowl*/
            new Item(325, 0, 1){ RuntimeId=0, NetworkId=1144, ExtraData = null }, /*minecraft:bucket*/
            new Item(325, 1, 1){ RuntimeId=0, NetworkId=1145, ExtraData = null }, /*minecraft:bucket*/
            new Item(325, 8, 1){ RuntimeId=0, NetworkId=1146, ExtraData = null }, /*minecraft:bucket*/
            new Item(325, 10, 1){ RuntimeId=0, NetworkId=1147, ExtraData = null }, /*minecraft:bucket*/
            new Item(325, 2, 1){ RuntimeId=0, NetworkId=1148, ExtraData = null }, /*minecraft:bucket*/
            new Item(325, 3, 1){ RuntimeId=0, NetworkId=1149, ExtraData = null }, /*minecraft:bucket*/
            new Item(325, 4, 1){ RuntimeId=0, NetworkId=1150, ExtraData = null }, /*minecraft:bucket*/
            new Item(325, 5, 1){ RuntimeId=0, NetworkId=1151, ExtraData = null }, /*minecraft:bucket*/
            new Item(325, 11, 1){ RuntimeId=0, NetworkId=1152, ExtraData = null }, /*minecraft:bucket*/
            new Item(325, 12, 1){ RuntimeId=0, NetworkId=1153, ExtraData = null }, /*minecraft:bucket*/
            new Item(630, 0, 1){ RuntimeId=0, NetworkId=1154, ExtraData = null }, /**/
            new Item(397, 3, 1){ RuntimeId=0, NetworkId=1155, ExtraData = null }, /*minecraft:skull*/
            new Item(397, 2, 1){ RuntimeId=0, NetworkId=1156, ExtraData = null }, /*minecraft:skull*/
            new Item(397, 4, 1){ RuntimeId=0, NetworkId=1157, ExtraData = null }, /*minecraft:skull*/
            new Item(397, 5, 1){ RuntimeId=0, NetworkId=1158, ExtraData = null }, /*minecraft:skull*/
            new Item(397, 0, 1){ RuntimeId=0, NetworkId=1159, ExtraData = null }, /*minecraft:skull*/
            new Item(397, 1, 1){ RuntimeId=0, NetworkId=1160, ExtraData = null }, /*minecraft:skull*/
            new Item(138, 0, 1){ RuntimeId=145, NetworkId=1161, ExtraData = null }, /*minecraft:beacon*/
            new Item(-206, 0, 1){ RuntimeId=6908, NetworkId=1162, ExtraData = null }, /*minecraft:bell*/
            new Item(-157, 0, 1){ RuntimeId=4232, NetworkId=1163, ExtraData = null }, /*minecraft:conduit*/
            new Item(-197, 0, 1){ RuntimeId=7574, NetworkId=1164, ExtraData = null }, /*minecraft:stonecutter_block*/
            new Item(120, 0, 1){ RuntimeId=6077, NetworkId=1165, ExtraData = null }, /*minecraft:end_portal_frame*/
            new Item(263, 0, 1){ RuntimeId=0, NetworkId=1166, ExtraData = null }, /*minecraft:coal*/
            new Item(263, 1, 1){ RuntimeId=0, NetworkId=1167, ExtraData = null }, /*minecraft:coal*/
            new Item(264, 0, 1){ RuntimeId=0, NetworkId=1168, ExtraData = null }, /*minecraft:diamond*/
            new Item(452, 0, 1){ RuntimeId=0, NetworkId=1169, ExtraData = null }, /*minecraft:iron_nugget*/
            new Item(505, 0, 1){ RuntimeId=0, NetworkId=1170, ExtraData = null }, /*minecraft:music_disc_mall*/
            new Item(506, 0, 1){ RuntimeId=0, NetworkId=1171, ExtraData = null }, /*minecraft:music_disc_mellohi*/
            new Item(507, 0, 1){ RuntimeId=0, NetworkId=1172, ExtraData = null }, /*minecraft:music_disc_stal*/
            new Item(504, 0, 1){ RuntimeId=0, NetworkId=1173, ExtraData = null }, /*minecraft:music_disc_far*/
            new Item(265, 0, 1){ RuntimeId=0, NetworkId=1174, ExtraData = null }, /*minecraft:iron_ingot*/
            new Item(754, 0, 1){ RuntimeId=0, NetworkId=1175, ExtraData = null }, /*minecraft:warped_sign*/
            new Item(744, 0, 1){ RuntimeId=0, NetworkId=1176, ExtraData = null }, /*minecraft:netherite_shovel*/
            new Item(371, 0, 1){ RuntimeId=0, NetworkId=1177, ExtraData = null }, /*minecraft:gold_nugget*/
            new Item(266, 0, 1){ RuntimeId=0, NetworkId=1178, ExtraData = null }, /*minecraft:gold_ingot*/
            new Item(388, 0, 1){ RuntimeId=0, NetworkId=1179, ExtraData = null }, /*minecraft:emerald*/
            new Item(406, 0, 1){ RuntimeId=0, NetworkId=1180, ExtraData = null }, /*minecraft:quartz*/
            new Item(337, 0, 1){ RuntimeId=0, NetworkId=1181, ExtraData = null }, /*minecraft:clay_ball*/
            new Item(336, 0, 1){ RuntimeId=0, NetworkId=1182, ExtraData = null }, /*minecraft:brick*/
            new Item(405, 0, 1){ RuntimeId=0, NetworkId=1183, ExtraData = null }, /*minecraft:netherbrick*/
            new Item(409, 0, 1){ RuntimeId=0, NetworkId=1184, ExtraData = null }, /*minecraft:prismarine_shard*/
            new Item(624, 0, 1){ RuntimeId=0, NetworkId=1185, ExtraData = null }, /**/
            new Item(422, 0, 1){ RuntimeId=0, NetworkId=1186, ExtraData = null }, /*minecraft:prismarine_crystals*/
            new Item(465, 0, 1){ RuntimeId=0, NetworkId=1187, ExtraData = null }, /*minecraft:nautilus_shell*/
            new Item(467, 0, 1){ RuntimeId=0, NetworkId=1188, ExtraData = null }, /*minecraft:heart_of_the_sea*/
            new Item(468, 0, 1){ RuntimeId=0, NetworkId=1189, ExtraData = null }, /*minecraft:scute*/
            new Item(470, 0, 1){ RuntimeId=0, NetworkId=1190, ExtraData = null }, /*minecraft:phantom_membrane*/
            new Item(287, 0, 1){ RuntimeId=0, NetworkId=1191, ExtraData = null }, /*minecraft:string*/
            new Item(288, 0, 1){ RuntimeId=0, NetworkId=1192, ExtraData = null }, /*minecraft:feather*/
            new Item(318, 0, 1){ RuntimeId=0, NetworkId=1193, ExtraData = null }, /*minecraft:flint*/
            new Item(289, 0, 1){ RuntimeId=0, NetworkId=1194, ExtraData = null }, /*minecraft:gunpowder*/
            new Item(334, 0, 1){ RuntimeId=0, NetworkId=1195, ExtraData = null }, /*minecraft:leather*/
            new Item(415, 0, 1){ RuntimeId=0, NetworkId=1196, ExtraData = null }, /*minecraft:rabbit_hide*/
            new Item(414, 0, 1){ RuntimeId=0, NetworkId=1197, ExtraData = null }, /*minecraft:rabbit_foot*/
            new Item(385, 0, 1){ RuntimeId=0, NetworkId=1198, ExtraData = null }, /*minecraft:fire_charge*/
            new Item(369, 0, 1){ RuntimeId=0, NetworkId=1199, ExtraData = null }, /*minecraft:blaze_rod*/
            new Item(377, 0, 1){ RuntimeId=0, NetworkId=1200, ExtraData = null }, /*minecraft:blaze_powder*/
            new Item(378, 0, 1){ RuntimeId=0, NetworkId=1201, ExtraData = null }, /*minecraft:magma_cream*/
            new Item(376, 0, 1){ RuntimeId=0, NetworkId=1202, ExtraData = null }, /*minecraft:fermented_spider_eye*/
            new Item(647, 0, 1){ RuntimeId=0, NetworkId=1203, ExtraData = null }, /**/
            new Item(437, 0, 1){ RuntimeId=0, NetworkId=1204, ExtraData = null }, /*minecraft:dragon_breath*/
            new Item(445, 0, 1){ RuntimeId=0, NetworkId=1205, ExtraData = null }, /*minecraft:shulker_shell*/
            new Item(370, 0, 1){ RuntimeId=0, NetworkId=1206, ExtraData = null }, /*minecraft:ghast_tear*/
            new Item(341, 0, 1){ RuntimeId=0, NetworkId=1207, ExtraData = null }, /*minecraft:slime_ball*/
            new Item(368, 0, 1){ RuntimeId=0, NetworkId=1208, ExtraData = null }, /*minecraft:ender_pearl*/
            new Item(381, 0, 1){ RuntimeId=0, NetworkId=1209, ExtraData = null }, /*minecraft:ender_eye*/
            new Item(399, 0, 1){ RuntimeId=0, NetworkId=1210, ExtraData = null }, /*minecraft:nether_star*/
            new Item(208, 0, 1){ RuntimeId=5891, NetworkId=1211, ExtraData = null }, /*minecraft:end_rod*/
            new Item(-312, 0, 1){ RuntimeId=1176, NetworkId=1212, ExtraData = null }, /**/
            new Item(653, 0, 1){ RuntimeId=0, NetworkId=1213, ExtraData = null }, /**/
            new Item(339, 0, 1){ RuntimeId=0, NetworkId=1214, ExtraData = null }, /*minecraft:paper*/
            new Item(340, 0, 1){ RuntimeId=0, NetworkId=1215, ExtraData = null }, /*minecraft:book*/
            new Item(386, 0, 1){ RuntimeId=0, NetworkId=1216, ExtraData = null }, /*minecraft:writable_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1217, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 0), new NbtShort("lvl", 1) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1218, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 0), new NbtShort("lvl", 2) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1219, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 0), new NbtShort("lvl", 3) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1220, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 0), new NbtShort("lvl", 4) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1221, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 1), new NbtShort("lvl", 1) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1222, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 1), new NbtShort("lvl", 2) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1223, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 1), new NbtShort("lvl", 3) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1224, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 1), new NbtShort("lvl", 4) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1225, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 2), new NbtShort("lvl", 1) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1226, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 2), new NbtShort("lvl", 2) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1227, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 2), new NbtShort("lvl", 3) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1228, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 2), new NbtShort("lvl", 4) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1229, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 3), new NbtShort("lvl", 1) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1230, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 3), new NbtShort("lvl", 2) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1231, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 3), new NbtShort("lvl", 3) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1232, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 3), new NbtShort("lvl", 4) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1233, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 4), new NbtShort("lvl", 1) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1234, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 4), new NbtShort("lvl", 2) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1235, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 4), new NbtShort("lvl", 3) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1236, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 4), new NbtShort("lvl", 4) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1237, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 5), new NbtShort("lvl", 1) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1238, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 5), new NbtShort("lvl", 2) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1239, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 5), new NbtShort("lvl", 3) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1240, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 6), new NbtShort("lvl", 1) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1241, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 6), new NbtShort("lvl", 2) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1242, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 6), new NbtShort("lvl", 3) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1243, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 7), new NbtShort("lvl", 1) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1244, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 7), new NbtShort("lvl", 2) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1245, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 7), new NbtShort("lvl", 3) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1246, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 8), new NbtShort("lvl", 1) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1247, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 9), new NbtShort("lvl", 1) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1248, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 9), new NbtShort("lvl", 2) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1249, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 9), new NbtShort("lvl", 3) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1250, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 9), new NbtShort("lvl", 4) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1251, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 9), new NbtShort("lvl", 5) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1252, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 10), new NbtShort("lvl", 1) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1253, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 10), new NbtShort("lvl", 2) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1254, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 10), new NbtShort("lvl", 3) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1255, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 10), new NbtShort("lvl", 4) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1256, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 10), new NbtShort("lvl", 5) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1257, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 11), new NbtShort("lvl", 1) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1258, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 11), new NbtShort("lvl", 2) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1259, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 11), new NbtShort("lvl", 3) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1260, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 11), new NbtShort("lvl", 4) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1261, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 11), new NbtShort("lvl", 5) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1262, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 12), new NbtShort("lvl", 1) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1263, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 12), new NbtShort("lvl", 2) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1264, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 13), new NbtShort("lvl", 1) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1265, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 13), new NbtShort("lvl", 2) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1266, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 14), new NbtShort("lvl", 1) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1267, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 14), new NbtShort("lvl", 2) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1268, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 14), new NbtShort("lvl", 3) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1269, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 15), new NbtShort("lvl", 1) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1270, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 15), new NbtShort("lvl", 2) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1271, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 15), new NbtShort("lvl", 3) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1272, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 15), new NbtShort("lvl", 4) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1273, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 15), new NbtShort("lvl", 5) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1274, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 16), new NbtShort("lvl", 1) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1275, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 17), new NbtShort("lvl", 1) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1276, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 17), new NbtShort("lvl", 2) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1277, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 17), new NbtShort("lvl", 3) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1278, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 18), new NbtShort("lvl", 1) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1279, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 18), new NbtShort("lvl", 2) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1280, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 18), new NbtShort("lvl", 3) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1281, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 19), new NbtShort("lvl", 1) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1282, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 19), new NbtShort("lvl", 2) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1283, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 19), new NbtShort("lvl", 3) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1284, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 19), new NbtShort("lvl", 4) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1285, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 19), new NbtShort("lvl", 5) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1286, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 20), new NbtShort("lvl", 1) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1287, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 20), new NbtShort("lvl", 2) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1288, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 21), new NbtShort("lvl", 1) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1289, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 22), new NbtShort("lvl", 1) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1290, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 23), new NbtShort("lvl", 1) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1291, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 23), new NbtShort("lvl", 2) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1292, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 23), new NbtShort("lvl", 3) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1293, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 24), new NbtShort("lvl", 1) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1294, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 24), new NbtShort("lvl", 2) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1295, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 24), new NbtShort("lvl", 3) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1296, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 25), new NbtShort("lvl", 1) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1297, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 25), new NbtShort("lvl", 2) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1298, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 26), new NbtShort("lvl", 1) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1299, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 27), new NbtShort("lvl", 1) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1300, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 28), new NbtShort("lvl", 1) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1301, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 29), new NbtShort("lvl", 1) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1302, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 29), new NbtShort("lvl", 2) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1303, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 29), new NbtShort("lvl", 3) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1304, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 29), new NbtShort("lvl", 4) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1305, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 29), new NbtShort("lvl", 5) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1306, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 30), new NbtShort("lvl", 1) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1307, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 30), new NbtShort("lvl", 2) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1308, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 30), new NbtShort("lvl", 3) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1309, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 31), new NbtShort("lvl", 1) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1310, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 31), new NbtShort("lvl", 2) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1311, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 31), new NbtShort("lvl", 3) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1312, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 32), new NbtShort("lvl", 1) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1313, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 33), new NbtShort("lvl", 1) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1314, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 34), new NbtShort("lvl", 1) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1315, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 34), new NbtShort("lvl", 2) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1316, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 34), new NbtShort("lvl", 3) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1317, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 34), new NbtShort("lvl", 4) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1318, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 35), new NbtShort("lvl", 1) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1319, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 35), new NbtShort("lvl", 2) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1320, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 35), new NbtShort("lvl", 3) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1321, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 36), new NbtShort("lvl", 1) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1322, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 36), new NbtShort("lvl", 2) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1323, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 36), new NbtShort("lvl", 3) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1324, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 37), new NbtShort("lvl", 1) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1325, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 37), new NbtShort("lvl", 2) } } } }, /*minecraft:enchanted_book*/
            new Item(403, 0, 1){ RuntimeId=0, NetworkId=1326, ExtraData = new NbtCompound { new NbtList("ench", (NbtTagType)10) { new NbtCompound { new NbtShort("id", 37), new NbtShort("lvl", 3) } } } }, /*minecraft:enchanted_book*/
            new Item(333, 0, 1){ RuntimeId=0, NetworkId=1327, ExtraData = null }, /*minecraft:boat*/
            new Item(333, 1, 1){ RuntimeId=0, NetworkId=1328, ExtraData = null }, /*minecraft:boat*/
            new Item(333, 2, 1){ RuntimeId=0, NetworkId=1329, ExtraData = null }, /*minecraft:boat*/
            new Item(333, 3, 1){ RuntimeId=0, NetworkId=1330, ExtraData = null }, /*minecraft:boat*/
            new Item(333, 4, 1){ RuntimeId=0, NetworkId=1331, ExtraData = null }, /*minecraft:boat*/
            new Item(333, 5, 1){ RuntimeId=0, NetworkId=1332, ExtraData = null }, /*minecraft:boat*/
            new Item(635, 0, 1){ RuntimeId=0, NetworkId=1333, ExtraData = null }, /**/
            new Item(638, 0, 1){ RuntimeId=0, NetworkId=1334, ExtraData = null }, /**/
            new Item(641, 0, 1){ RuntimeId=0, NetworkId=1335, ExtraData = null }, /**/
            new Item(639, 0, 1){ RuntimeId=0, NetworkId=1336, ExtraData = null }, /**/
            new Item(640, 0, 1){ RuntimeId=0, NetworkId=1337, ExtraData = null }, /**/
            new Item(642, 0, 1){ RuntimeId=0, NetworkId=1338, ExtraData = null }, /**/
            new Item(643, 0, 1){ RuntimeId=0, NetworkId=1339, ExtraData = null }, /**/
            new Item(644, 0, 1){ RuntimeId=0, NetworkId=1340, ExtraData = null }, /**/
            new Item(66, 0, 1){ RuntimeId=3920, NetworkId=1341, ExtraData = null }, /*minecraft:rail*/
            new Item(27, 0, 1){ RuntimeId=5332, NetworkId=1342, ExtraData = null }, /*minecraft:golden_rail*/
            new Item(28, 0, 1){ RuntimeId=4132, NetworkId=1343, ExtraData = null }, /*minecraft:detector_rail*/
            new Item(126, 0, 1){ RuntimeId=309, NetworkId=1344, ExtraData = null }, /*minecraft:activator_rail*/
            new Item(328, 0, 1){ RuntimeId=0, NetworkId=1345, ExtraData = null }, /*minecraft:minecart*/
            new Item(342, 0, 1){ RuntimeId=0, NetworkId=1346, ExtraData = null }, /*minecraft:chest_minecart*/
            new Item(408, 0, 1){ RuntimeId=0, NetworkId=1347, ExtraData = null }, /*minecraft:hopper_minecart*/
            new Item(407, 0, 1){ RuntimeId=0, NetworkId=1348, ExtraData = null }, /*minecraft:tnt_minecart*/
            new Item(331, 0, 1){ RuntimeId=0, NetworkId=1349, ExtraData = null }, /*minecraft:redstone*/
            new Item(152, 0, 1){ RuntimeId=3776, NetworkId=1350, ExtraData = null }, /*minecraft:redstone_block*/
            new Item(76, 0, 1){ RuntimeId=3525, NetworkId=1351, ExtraData = null }, /*minecraft:redstone_torch*/
            new Item(69, 0, 1){ RuntimeId=6514, NetworkId=1352, ExtraData = null }, /*minecraft:lever*/
            new Item(143, 0, 1){ RuntimeId=6391, NetworkId=1353, ExtraData = null }, /*minecraft:wooden_button*/
            new Item(-144, 0, 1){ RuntimeId=4321, NetworkId=1354, ExtraData = null }, /*minecraft:spruce_button*/
            new Item(-141, 0, 1){ RuntimeId=7766, NetworkId=1355, ExtraData = null }, /*minecraft:birch_button*/
            new Item(-143, 0, 1){ RuntimeId=116, NetworkId=1356, ExtraData = null }, /*minecraft:jungle_button*/
            new Item(-140, 0, 1){ RuntimeId=7231, NetworkId=1357, ExtraData = null }, /*minecraft:acacia_button*/
            new Item(-142, 0, 1){ RuntimeId=93, NetworkId=1358, ExtraData = null }, /*minecraft:dark_oak_button*/
            new Item(-487, 0, 1){ RuntimeId=7062, NetworkId=1359, ExtraData = null }, /**/
            new Item(77, 0, 1){ RuntimeId=596, NetworkId=1360, ExtraData = null }, /*minecraft:stone_button*/
            new Item(-260, 0, 1){ RuntimeId=4432, NetworkId=1361, ExtraData = null }, /*minecraft:crimson_button*/
            new Item(-261, 0, 1){ RuntimeId=7250, NetworkId=1362, ExtraData = null }, /*minecraft:warped_button*/
            new Item(-296, 0, 1){ RuntimeId=7790, NetworkId=1363, ExtraData = null }, /*minecraft:polished_blackstone_button*/
            new Item(131, 0, 1){ RuntimeId=5914, NetworkId=1364, ExtraData = null }, /*minecraft:tripwire_hook*/
            new Item(72, 0, 1){ RuntimeId=8063, NetworkId=1365, ExtraData = null }, /*minecraft:wooden_pressure_plate*/
            new Item(-154, 0, 1){ RuntimeId=3759, NetworkId=1366, ExtraData = null }, /*minecraft:spruce_pressure_plate*/
            new Item(-151, 0, 1){ RuntimeId=3555, NetworkId=1367, ExtraData = null }, /*minecraft:birch_pressure_plate*/
            new Item(-153, 0, 1){ RuntimeId=3635, NetworkId=1368, ExtraData = null }, /*minecraft:jungle_pressure_plate*/
            new Item(-150, 0, 1){ RuntimeId=5247, NetworkId=1369, ExtraData = null }, /*minecraft:acacia_pressure_plate*/
            new Item(-152, 0, 1){ RuntimeId=5956, NetworkId=1370, ExtraData = null }, /*minecraft:dark_oak_pressure_plate*/
            new Item(-490, 0, 1){ RuntimeId=3869, NetworkId=1371, ExtraData = null }, /**/
            new Item(-262, 0, 1){ RuntimeId=8268, NetworkId=1372, ExtraData = null }, /*minecraft:crimson_pressure_plate*/
            new Item(-263, 0, 1){ RuntimeId=256, NetworkId=1373, ExtraData = null }, /*minecraft:warped_pressure_plate*/
            new Item(70, 0, 1){ RuntimeId=3886, NetworkId=1374, ExtraData = null }, /*minecraft:stone_pressure_plate*/
            new Item(147, 0, 1){ RuntimeId=3665, NetworkId=1375, ExtraData = null }, /*minecraft:light_weighted_pressure_plate*/
            new Item(148, 0, 1){ RuntimeId=1160, NetworkId=1376, ExtraData = null }, /*minecraft:heavy_weighted_pressure_plate*/
            new Item(-295, 0, 1){ RuntimeId=6232, NetworkId=1377, ExtraData = null }, /*minecraft:polished_blackstone_pressure_plate*/
            new Item(251, 0, 1){ RuntimeId=3513, NetworkId=1378, ExtraData = null }, /*minecraft:observer*/
            new Item(151, 0, 1){ RuntimeId=4197, NetworkId=1379, ExtraData = null }, /*minecraft:daylight_detector*/
            new Item(356, 0, 1){ RuntimeId=0, NetworkId=1380, ExtraData = null }, /*minecraft:repeater*/
            new Item(404, 0, 1){ RuntimeId=0, NetworkId=1381, ExtraData = null }, /*minecraft:comparator*/
            new Item(410, 0, 1){ RuntimeId=0, NetworkId=1382, ExtraData = null }, /*minecraft:hopper*/
            new Item(125, 0, 1){ RuntimeId=7385, NetworkId=1383, ExtraData = null }, /*minecraft:dropper*/
            new Item(23, 0, 1){ RuntimeId=8013, NetworkId=1384, ExtraData = null }, /*minecraft:dispenser*/
            new Item(33, 0, 1){ RuntimeId=922, NetworkId=1385, ExtraData = null }, /*minecraft:piston*/
            new Item(29, 0, 1){ RuntimeId=4364, NetworkId=1386, ExtraData = null }, /*minecraft:sticky_piston*/
            new Item(46, 0, 1){ RuntimeId=6707, NetworkId=1387, ExtraData = null }, /*minecraft:tnt*/
            new Item(421, 0, 1){ RuntimeId=0, NetworkId=1388, ExtraData = null }, /*minecraft:name_tag*/
            new Item(-204, 0, 1){ RuntimeId=3826, NetworkId=1389, ExtraData = null }, /*minecraft:loom*/
            new Item(446, 0, 1){ RuntimeId=0, NetworkId=1390, ExtraData = null }, /*minecraft:banner*/
            new Item(446, 8, 1){ RuntimeId=0, NetworkId=1391, ExtraData = null }, /*minecraft:banner*/
            new Item(446, 7, 1){ RuntimeId=0, NetworkId=1392, ExtraData = null }, /*minecraft:banner*/
            new Item(446, 15, 1){ RuntimeId=0, NetworkId=1393, ExtraData = null }, /*minecraft:banner*/
            new Item(446, 12, 1){ RuntimeId=0, NetworkId=1394, ExtraData = null }, /*minecraft:banner*/
            new Item(446, 14, 1){ RuntimeId=0, NetworkId=1395, ExtraData = null }, /*minecraft:banner*/
            new Item(446, 1, 1){ RuntimeId=0, NetworkId=1396, ExtraData = null }, /*minecraft:banner*/
            new Item(446, 4, 1){ RuntimeId=0, NetworkId=1397, ExtraData = null }, /*minecraft:banner*/
            new Item(446, 5, 1){ RuntimeId=0, NetworkId=1398, ExtraData = null }, /*minecraft:banner*/
            new Item(446, 13, 1){ RuntimeId=0, NetworkId=1399, ExtraData = null }, /*minecraft:banner*/
            new Item(446, 9, 1){ RuntimeId=0, NetworkId=1400, ExtraData = null }, /*minecraft:banner*/
            new Item(446, 3, 1){ RuntimeId=0, NetworkId=1401, ExtraData = null }, /*minecraft:banner*/
            new Item(446, 11, 1){ RuntimeId=0, NetworkId=1402, ExtraData = null }, /*minecraft:banner*/
            new Item(446, 10, 1){ RuntimeId=0, NetworkId=1403, ExtraData = null }, /*minecraft:banner*/
            new Item(446, 2, 1){ RuntimeId=0, NetworkId=1404, ExtraData = null }, /*minecraft:banner*/
            new Item(446, 6, 1){ RuntimeId=0, NetworkId=1405, ExtraData = null }, /*minecraft:banner*/
            new Item(446, 15, 1){ RuntimeId=0, NetworkId=1406, ExtraData = new NbtCompound { new NbtInt("Type", 1) } }, /*minecraft:banner*/
            new Item(434, 0, 1){ RuntimeId=0, NetworkId=1407, ExtraData = null }, /*minecraft:banner_pattern*/
            new Item(434, 1, 1){ RuntimeId=0, NetworkId=1408, ExtraData = null }, /*minecraft:banner_pattern*/
            new Item(434, 2, 1){ RuntimeId=0, NetworkId=1409, ExtraData = null }, /*minecraft:banner_pattern*/
            new Item(434, 3, 1){ RuntimeId=0, NetworkId=1410, ExtraData = null }, /*minecraft:banner_pattern*/
            new Item(434, 4, 1){ RuntimeId=0, NetworkId=1411, ExtraData = null }, /*minecraft:banner_pattern*/
            new Item(434, 5, 1){ RuntimeId=0, NetworkId=1412, ExtraData = null }, /*minecraft:banner_pattern*/
            new Item(434, 6, 1){ RuntimeId=0, NetworkId=1413, ExtraData = null }, /*minecraft:banner_pattern*/
            new Item(720, 0, 1){ RuntimeId=0, NetworkId=1414, ExtraData = null }, /*minecraft:campfire*/
            new Item(401, 0, 1){ RuntimeId=0, NetworkId=1415, ExtraData = new NbtCompound { new NbtCompound("Fireworks") { new NbtList("Explosions", (NbtTagType)0), new NbtByte("Flight", 1) } } }, /*minecraft:firework_rocket*/
            new Item(401, 0, 1){ RuntimeId=0, NetworkId=1416, ExtraData = new NbtCompound { new NbtCompound("Fireworks") { new NbtList("Explosions", (NbtTagType)10) { new NbtCompound { new NbtByteArray("FireworkColor", new byte[1]{0}), new NbtByteArray("FireworkFade", new byte[0]{}), new NbtByte("FireworkFlicker", 0), new NbtByte("FireworkTrail", 0), new NbtByte("FireworkType", 0) } }, new NbtByte("Flight", 1) } } }, /*minecraft:firework_rocket*/
            new Item(401, 0, 1){ RuntimeId=0, NetworkId=1417, ExtraData = new NbtCompound { new NbtCompound("Fireworks") { new NbtList("Explosions", (NbtTagType)10) { new NbtCompound { new NbtByteArray("FireworkColor", new byte[1]{8}), new NbtByteArray("FireworkFade", new byte[0]{}), new NbtByte("FireworkFlicker", 0), new NbtByte("FireworkTrail", 0), new NbtByte("FireworkType", 0) } }, new NbtByte("Flight", 1) } } }, /*minecraft:firework_rocket*/
            new Item(401, 0, 1){ RuntimeId=0, NetworkId=1418, ExtraData = new NbtCompound { new NbtCompound("Fireworks") { new NbtList("Explosions", (NbtTagType)10) { new NbtCompound { new NbtByteArray("FireworkColor", new byte[1]{7}), new NbtByteArray("FireworkFade", new byte[0]{}), new NbtByte("FireworkFlicker", 0), new NbtByte("FireworkTrail", 0), new NbtByte("FireworkType", 0) } }, new NbtByte("Flight", 1) } } }, /*minecraft:firework_rocket*/
            new Item(401, 0, 1){ RuntimeId=0, NetworkId=1419, ExtraData = new NbtCompound { new NbtCompound("Fireworks") { new NbtList("Explosions", (NbtTagType)10) { new NbtCompound { new NbtByteArray("FireworkColor", new byte[1]{15}), new NbtByteArray("FireworkFade", new byte[0]{}), new NbtByte("FireworkFlicker", 0), new NbtByte("FireworkTrail", 0), new NbtByte("FireworkType", 0) } }, new NbtByte("Flight", 1) } } }, /*minecraft:firework_rocket*/
            new Item(401, 0, 1){ RuntimeId=0, NetworkId=1420, ExtraData = new NbtCompound { new NbtCompound("Fireworks") { new NbtList("Explosions", (NbtTagType)10) { new NbtCompound { new NbtByteArray("FireworkColor", new byte[1]{12}), new NbtByteArray("FireworkFade", new byte[0]{}), new NbtByte("FireworkFlicker", 0), new NbtByte("FireworkTrail", 0), new NbtByte("FireworkType", 0) } }, new NbtByte("Flight", 1) } } }, /*minecraft:firework_rocket*/
            new Item(401, 0, 1){ RuntimeId=0, NetworkId=1421, ExtraData = new NbtCompound { new NbtCompound("Fireworks") { new NbtList("Explosions", (NbtTagType)10) { new NbtCompound { new NbtByteArray("FireworkColor", new byte[1]{14}), new NbtByteArray("FireworkFade", new byte[0]{}), new NbtByte("FireworkFlicker", 0), new NbtByte("FireworkTrail", 0), new NbtByte("FireworkType", 0) } }, new NbtByte("Flight", 1) } } }, /*minecraft:firework_rocket*/
            new Item(401, 0, 1){ RuntimeId=0, NetworkId=1422, ExtraData = new NbtCompound { new NbtCompound("Fireworks") { new NbtList("Explosions", (NbtTagType)10) { new NbtCompound { new NbtByteArray("FireworkColor", new byte[1]{1}), new NbtByteArray("FireworkFade", new byte[0]{}), new NbtByte("FireworkFlicker", 0), new NbtByte("FireworkTrail", 0), new NbtByte("FireworkType", 0) } }, new NbtByte("Flight", 1) } } }, /*minecraft:firework_rocket*/
            new Item(401, 0, 1){ RuntimeId=0, NetworkId=1423, ExtraData = new NbtCompound { new NbtCompound("Fireworks") { new NbtList("Explosions", (NbtTagType)10) { new NbtCompound { new NbtByteArray("FireworkColor", new byte[1]{4}), new NbtByteArray("FireworkFade", new byte[0]{}), new NbtByte("FireworkFlicker", 0), new NbtByte("FireworkTrail", 0), new NbtByte("FireworkType", 0) } }, new NbtByte("Flight", 1) } } }, /*minecraft:firework_rocket*/
            new Item(401, 0, 1){ RuntimeId=0, NetworkId=1424, ExtraData = new NbtCompound { new NbtCompound("Fireworks") { new NbtList("Explosions", (NbtTagType)10) { new NbtCompound { new NbtByteArray("FireworkColor", new byte[1]{5}), new NbtByteArray("FireworkFade", new byte[0]{}), new NbtByte("FireworkFlicker", 0), new NbtByte("FireworkTrail", 0), new NbtByte("FireworkType", 0) } }, new NbtByte("Flight", 1) } } }, /*minecraft:firework_rocket*/
            new Item(401, 0, 1){ RuntimeId=0, NetworkId=1425, ExtraData = new NbtCompound { new NbtCompound("Fireworks") { new NbtList("Explosions", (NbtTagType)10) { new NbtCompound { new NbtByteArray("FireworkColor", new byte[1]{13}), new NbtByteArray("FireworkFade", new byte[0]{}), new NbtByte("FireworkFlicker", 0), new NbtByte("FireworkTrail", 0), new NbtByte("FireworkType", 0) } }, new NbtByte("Flight", 1) } } }, /*minecraft:firework_rocket*/
            new Item(401, 0, 1){ RuntimeId=0, NetworkId=1426, ExtraData = new NbtCompound { new NbtCompound("Fireworks") { new NbtList("Explosions", (NbtTagType)10) { new NbtCompound { new NbtByteArray("FireworkColor", new byte[1]{9}), new NbtByteArray("FireworkFade", new byte[0]{}), new NbtByte("FireworkFlicker", 0), new NbtByte("FireworkTrail", 0), new NbtByte("FireworkType", 0) } }, new NbtByte("Flight", 1) } } }, /*minecraft:firework_rocket*/
            new Item(401, 0, 1){ RuntimeId=0, NetworkId=1427, ExtraData = new NbtCompound { new NbtCompound("Fireworks") { new NbtList("Explosions", (NbtTagType)10) { new NbtCompound { new NbtByteArray("FireworkColor", new byte[1]{3}), new NbtByteArray("FireworkFade", new byte[0]{}), new NbtByte("FireworkFlicker", 0), new NbtByte("FireworkTrail", 0), new NbtByte("FireworkType", 0) } }, new NbtByte("Flight", 1) } } }, /*minecraft:firework_rocket*/
            new Item(401, 0, 1){ RuntimeId=0, NetworkId=1428, ExtraData = new NbtCompound { new NbtCompound("Fireworks") { new NbtList("Explosions", (NbtTagType)10) { new NbtCompound { new NbtByteArray("FireworkColor", new byte[1]{11}), new NbtByteArray("FireworkFade", new byte[0]{}), new NbtByte("FireworkFlicker", 0), new NbtByte("FireworkTrail", 0), new NbtByte("FireworkType", 0) } }, new NbtByte("Flight", 1) } } }, /*minecraft:firework_rocket*/
            new Item(401, 0, 1){ RuntimeId=0, NetworkId=1429, ExtraData = new NbtCompound { new NbtCompound("Fireworks") { new NbtList("Explosions", (NbtTagType)10) { new NbtCompound { new NbtByteArray("FireworkColor", new byte[1]{10}), new NbtByteArray("FireworkFade", new byte[0]{}), new NbtByte("FireworkFlicker", 0), new NbtByte("FireworkTrail", 0), new NbtByte("FireworkType", 0) } }, new NbtByte("Flight", 1) } } }, /*minecraft:firework_rocket*/
            new Item(401, 0, 1){ RuntimeId=0, NetworkId=1430, ExtraData = new NbtCompound { new NbtCompound("Fireworks") { new NbtList("Explosions", (NbtTagType)10) { new NbtCompound { new NbtByteArray("FireworkColor", new byte[1]{2}), new NbtByteArray("FireworkFade", new byte[0]{}), new NbtByte("FireworkFlicker", 0), new NbtByte("FireworkTrail", 0), new NbtByte("FireworkType", 0) } }, new NbtByte("Flight", 1) } } }, /*minecraft:firework_rocket*/
            new Item(401, 0, 1){ RuntimeId=0, NetworkId=1431, ExtraData = new NbtCompound { new NbtCompound("Fireworks") { new NbtList("Explosions", (NbtTagType)10) { new NbtCompound { new NbtByteArray("FireworkColor", new byte[1]{6}), new NbtByteArray("FireworkFade", new byte[0]{}), new NbtByte("FireworkFlicker", 0), new NbtByte("FireworkTrail", 0), new NbtByte("FireworkType", 0) } }, new NbtByte("Flight", 1) } } }, /*minecraft:firework_rocket*/
            new Item(402, 0, 1){ RuntimeId=0, NetworkId=1432, ExtraData = new NbtCompound { new NbtCompound("FireworksItem") { new NbtByteArray("FireworkColor", new byte[1]{0}), new NbtByteArray("FireworkFade", new byte[0]{}), new NbtByte("FireworkFlicker", 0), new NbtByte("FireworkTrail", 0), new NbtByte("FireworkType", 0) }, new NbtInt("customColor", -14869215) } }, /*minecraft:firework_star*/
            new Item(402, 8, 1){ RuntimeId=0, NetworkId=1433, ExtraData = new NbtCompound { new NbtCompound("FireworksItem") { new NbtByteArray("FireworkColor", new byte[1]{8}), new NbtByteArray("FireworkFade", new byte[0]{}), new NbtByte("FireworkFlicker", 0), new NbtByte("FireworkTrail", 0), new NbtByte("FireworkType", 0) }, new NbtInt("customColor", -12103854) } }, /*minecraft:firework_star*/
            new Item(402, 7, 1){ RuntimeId=0, NetworkId=1434, ExtraData = new NbtCompound { new NbtCompound("FireworksItem") { new NbtByteArray("FireworkColor", new byte[1]{7}), new NbtByteArray("FireworkFade", new byte[0]{}), new NbtByte("FireworkFlicker", 0), new NbtByte("FireworkTrail", 0), new NbtByte("FireworkType", 0) }, new NbtInt("customColor", -6447721) } }, /*minecraft:firework_star*/
            new Item(402, 15, 1){ RuntimeId=0, NetworkId=1435, ExtraData = new NbtCompound { new NbtCompound("FireworksItem") { new NbtByteArray("FireworkColor", new byte[1]{15}), new NbtByteArray("FireworkFade", new byte[0]{}), new NbtByte("FireworkFlicker", 0), new NbtByte("FireworkTrail", 0), new NbtByte("FireworkType", 0) }, new NbtInt("customColor", -986896) } }, /*minecraft:firework_star*/
            new Item(402, 12, 1){ RuntimeId=0, NetworkId=1436, ExtraData = new NbtCompound { new NbtCompound("FireworksItem") { new NbtByteArray("FireworkColor", new byte[1]{12}), new NbtByteArray("FireworkFade", new byte[0]{}), new NbtByte("FireworkFlicker", 0), new NbtByte("FireworkTrail", 0), new NbtByte("FireworkType", 0) }, new NbtInt("customColor", -12930086) } }, /*minecraft:firework_star*/
            new Item(402, 14, 1){ RuntimeId=0, NetworkId=1437, ExtraData = new NbtCompound { new NbtCompound("FireworksItem") { new NbtByteArray("FireworkColor", new byte[1]{14}), new NbtByteArray("FireworkFade", new byte[0]{}), new NbtByte("FireworkFlicker", 0), new NbtByte("FireworkTrail", 0), new NbtByte("FireworkType", 0) }, new NbtInt("customColor", -425955) } }, /*minecraft:firework_star*/
            new Item(402, 1, 1){ RuntimeId=0, NetworkId=1438, ExtraData = new NbtCompound { new NbtCompound("FireworksItem") { new NbtByteArray("FireworkColor", new byte[1]{1}), new NbtByteArray("FireworkFade", new byte[0]{}), new NbtByte("FireworkFlicker", 0), new NbtByte("FireworkTrail", 0), new NbtByte("FireworkType", 0) }, new NbtInt("customColor", -5231066) } }, /*minecraft:firework_star*/
            new Item(402, 4, 1){ RuntimeId=0, NetworkId=1439, ExtraData = new NbtCompound { new NbtCompound("FireworksItem") { new NbtByteArray("FireworkColor", new byte[1]{4}), new NbtByteArray("FireworkFade", new byte[0]{}), new NbtByte("FireworkFlicker", 0), new NbtByte("FireworkTrail", 0), new NbtByte("FireworkType", 0) }, new NbtInt("customColor", -12827478) } }, /*minecraft:firework_star*/
            new Item(402, 5, 1){ RuntimeId=0, NetworkId=1440, ExtraData = new NbtCompound { new NbtCompound("FireworksItem") { new NbtByteArray("FireworkColor", new byte[1]{5}), new NbtByteArray("FireworkFade", new byte[0]{}), new NbtByte("FireworkFlicker", 0), new NbtByte("FireworkTrail", 0), new NbtByte("FireworkType", 0) }, new NbtInt("customColor", -7785800) } }, /*minecraft:firework_star*/
            new Item(402, 13, 1){ RuntimeId=0, NetworkId=1441, ExtraData = new NbtCompound { new NbtCompound("FireworksItem") { new NbtByteArray("FireworkColor", new byte[1]{13}), new NbtByteArray("FireworkFade", new byte[0]{}), new NbtByte("FireworkFlicker", 0), new NbtByte("FireworkTrail", 0), new NbtByte("FireworkType", 0) }, new NbtInt("customColor", -3715395) } }, /*minecraft:firework_star*/
            new Item(402, 9, 1){ RuntimeId=0, NetworkId=1442, ExtraData = new NbtCompound { new NbtCompound("FireworksItem") { new NbtByteArray("FireworkColor", new byte[1]{9}), new NbtByteArray("FireworkFade", new byte[0]{}), new NbtByte("FireworkFlicker", 0), new NbtByte("FireworkTrail", 0), new NbtByte("FireworkType", 0) }, new NbtInt("customColor", -816214) } }, /*minecraft:firework_star*/
            new Item(402, 3, 1){ RuntimeId=0, NetworkId=1443, ExtraData = new NbtCompound { new NbtCompound("FireworksItem") { new NbtByteArray("FireworkColor", new byte[1]{3}), new NbtByteArray("FireworkFade", new byte[0]{}), new NbtByte("FireworkFlicker", 0), new NbtByte("FireworkTrail", 0), new NbtByte("FireworkType", 0) }, new NbtInt("customColor", -8170446) } }, /*minecraft:firework_star*/
            new Item(402, 11, 1){ RuntimeId=0, NetworkId=1444, ExtraData = new NbtCompound { new NbtCompound("FireworksItem") { new NbtByteArray("FireworkColor", new byte[1]{11}), new NbtByteArray("FireworkFade", new byte[0]{}), new NbtByte("FireworkFlicker", 0), new NbtByte("FireworkTrail", 0), new NbtByte("FireworkType", 0) }, new NbtInt("customColor", -75715) } }, /*minecraft:firework_star*/
            new Item(402, 10, 1){ RuntimeId=0, NetworkId=1445, ExtraData = new NbtCompound { new NbtCompound("FireworksItem") { new NbtByteArray("FireworkColor", new byte[1]{10}), new NbtByteArray("FireworkFade", new byte[0]{}), new NbtByte("FireworkFlicker", 0), new NbtByte("FireworkTrail", 0), new NbtByte("FireworkType", 0) }, new NbtInt("customColor", -8337633) } }, /*minecraft:firework_star*/
            new Item(402, 2, 1){ RuntimeId=0, NetworkId=1446, ExtraData = new NbtCompound { new NbtCompound("FireworksItem") { new NbtByteArray("FireworkColor", new byte[1]{2}), new NbtByteArray("FireworkFade", new byte[0]{}), new NbtByte("FireworkFlicker", 0), new NbtByte("FireworkTrail", 0), new NbtByte("FireworkType", 0) }, new NbtInt("customColor", -10585066) } }, /*minecraft:firework_star*/
            new Item(402, 6, 1){ RuntimeId=0, NetworkId=1447, ExtraData = new NbtCompound { new NbtCompound("FireworksItem") { new NbtByteArray("FireworkColor", new byte[1]{6}), new NbtByteArray("FireworkFade", new byte[0]{}), new NbtByte("FireworkFlicker", 0), new NbtByte("FireworkTrail", 0), new NbtByte("FireworkType", 0) }, new NbtInt("customColor", -15295332) } }, /*minecraft:firework_star*/
            new Item(760, 0, 1){ RuntimeId=0, NetworkId=1448, ExtraData = null }, /*minecraft:nether_sprouts*/
            new Item(-239, 0, 1){ RuntimeId=6390, NetworkId=1449, ExtraData = null }, /*minecraft:target*/
            new Item(743, 0, 1){ RuntimeId=0, NetworkId=1450, ExtraData = null }, /*minecraft:netherite_sword*/
        };
	}
}