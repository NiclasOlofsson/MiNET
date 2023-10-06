using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Xml.Linq;
using fNbt;
using log4net;
using MiNET.Blocks;
using MiNET.Utils;
using Newtonsoft.Json.Linq;

namespace MiNET.Worlds.Anvil
{
	public class AnvilPaletteConverter
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(AnvilPaletteConverter));

		private static readonly AnvilToBedrockStateMapper _mapper = new AnvilToBedrockStateMapper();

		private static readonly Dictionary<string, string> _anvilBedrockBiomesMap = new Dictionary<string, string>
		{
			{ "old_growth_pine_taiga", "mega_taiga" },
			{ "old_growth_spruce_taiga", "redwood_taiga_mutated" },
			{ "stony_shore", "stone_beach" },
			{ "snowy_beach", "cold_beach" },
		};

		private static readonly HashSet<string> _seaBlocks = new HashSet<string>
		{
			"minecraft:seagrass",
			"minecraft:tall_seagrass",
			"minecraft:bubble_column",
			"minecraft:kelp"
		};

		private static readonly string[] _woodList = new[]
		{
			"oak",
			"spruce",
			"birch",
			"jungle",
			"acacia",
			"dark_oak",
			"mangrove",
			"cherry",
			"bamboo",
			"crimson",
			"warped"
		};

		private static readonly string[] _colorsList = new[]
		{
			"white",
			"orange",
			"magenta",
			"light_blue",
			"yellow",
			"lime",
			"pink",
			"gray",
			"light_gray",
			"silver",
			"cyan",
			"purple",
			"blue",
			"brown",
			"green",
			"red",
			"black"
		};

		private static readonly string[] _infestedStoneList = new[]
		{
			"stone",
			"cobblestone",
			"stone_brick",
			"mossy_stone_brick",
			"cracked_stone_brick",
			"chiseled_stone_brick"
		};

		private static readonly string[] _slabMaterialsList = new[]
		{
			"bamboo_mosaic",
			"stone",
			"granite",
			"polished_granite",
			"diorite",
			"polished_diorite",
			"andesite",
			"polished_andesite",
			"cobblestone",
			"mossy_cobblestone",
			"stone_brick",
			"mossy_stone_brick",
			"brick",
			"end_brick",
			"nether_brick",
			"red_nether_brick",
			"sandstone",
			"smooth_sandstone",
			"red_sandstone",
			"smooth_red_sandstone",
			"quartz",
			"smooth_quartz",
			"purpur",
			"prismarine",
			"prismarine_bricks",
			"dark_prismarine",
			"blackstone",
			"polished_blackstone",
			"polished_blackstone_brick",
			"cut_copper",
			"exposed_cut_copper",
			"weathered_cut_copper",
			"oxidized_cut_copper",
			"waxed_cut_copper",
			"waxed_exposed_cut_copper",
			"waxed_weathered_cut_copper",
			"waxed_oxidized_cut_copper",
			"cobbled_deepslate",
			"polished_deepslate",
			"deepslate_brick",
			"deepslate_tile",
			"mud_brick"
		}
			.Concat(_woodList)
			.ToArray();

		static AnvilPaletteConverter()
		{
			var faceDirectionSkipMap = new BlockStateMapper(
				new SkipPropertyStateMapper("down"),
				new SkipPropertyStateMapper("up"),
				new SkipPropertyStateMapper("north"),
				new SkipPropertyStateMapper("south"),
				new SkipPropertyStateMapper("west"),
				new SkipPropertyStateMapper("east"));
			var upperBlockBitMap = new PropertyStateMapper("half", "upper_block_bit",
					new PropertyValueStateMapper("upper", "true"),
					new PropertyValueStateMapper("lower", "false"));
			var upsideDownBitMap = new PropertyStateMapper("half", "upside_down_bit",
					new PropertyValueStateMapper("top", "1"),
					new PropertyValueStateMapper("bottom", "0"));
			var facingDirectionMap = new PropertyStateMapper("facing", "facing_direction",
					new PropertyValueStateMapper("down", "0"),
					new PropertyValueStateMapper("up", "1"),
					new PropertyValueStateMapper("north", "2"),
					new PropertyValueStateMapper("south", "3"),
					new PropertyValueStateMapper("west", "4"),
					new PropertyValueStateMapper("east", "5"));
			var multiFaceDirectonMap = new BlockStateMapper(
				(name, properties) =>
				{
					var faceDirection = 0;

					faceDirection |= properties["down"].StringValue == "true" ? 1 << 0 : 0;
					faceDirection |= properties["up"].StringValue == "true" ? 1 << 1 : 0;
					faceDirection |= properties["south"].StringValue == "true" ? 1 << 2 : 0;
					faceDirection |= properties["west"].StringValue == "true" ? 1 << 3 : 0;
					faceDirection |= properties["north"].StringValue == "true" ? 1 << 4 : 0;
					faceDirection |= properties["east"].StringValue == "true" ? 1 << 5 : 0;

					properties.Clear();
					properties.Add(new NbtString("multi_face_direction_bits", faceDirection.ToString()));
				});
			var litMap = new BlockStateMapper(
				(name, properties) =>
				{
					var litName = properties["lit"].StringValue == "true" ? name.Replace("minecraft:", "minecraft:lit_") : name;
					properties.Remove("lit");

					return litName;
				});

			_mapper.AddDefault(new BlockStateMapper(
				new PropertyStateMapper("facing", "direction",
					new PropertyValueStateMapper("north", "0"),
					new PropertyValueStateMapper("east", "1"),
					new PropertyValueStateMapper("south", "2"),
					new PropertyValueStateMapper("west", "3"))));
			_mapper.AddDefault(new BlockStateMapper(
				new PropertyStateMapper("axis", "pillar_axis")));
			_mapper.AddDefault(new BlockStateMapper(
				new SkipPropertyStateMapper("waterlogged"),
				new SkipPropertyStateMapper("snowy"),
				new SkipPropertyStateMapper("powered")));


			_mapper.Add(new BlockStateMapper("minecraft:grass_block", "minecraft:grass"));
			_mapper.Add(new BlockStateMapper("minecraft:magma_block", "minecraft:magma"));
			_mapper.Add(new BlockStateMapper("minecraft:cobweb", "minecraft:web"));
			_mapper.Add(new BlockStateMapper("minecraft:cave_air", "minecraft:air"));
			_mapper.Add(new BlockStateMapper("minecraft:void_air", "minecraft:air"));
			_mapper.Add(new BlockStateMapper("minecraft:spawner", "minecraft:mob_spawner"));
			_mapper.Add(new BlockStateMapper("minecraft:dirt_path", "minecraft:grass_path"));
			_mapper.Add(new BlockStateMapper("minecraft:rooted_dirt", "minecraft:dirt_with_roots"));
			_mapper.Add(new BlockStateMapper("minecraft:dandelion", "minecraft:yellow_flower"));
			_mapper.Add(new BlockStateMapper("minecraft:snow_block", "minecraft:snow"));
			_mapper.Add(new BlockStateMapper("minecraft:sugar_cane", "minecraft:reeds"));
			_mapper.Add(new BlockStateMapper("minecraft:terracotta", "minecraft:hardened_clay"));
			_mapper.Add(new BlockStateMapper("minecraft:jack_o_lantern", "minecraft:lit_pumpkin"));
			_mapper.Add(new BlockStateMapper("minecraft:bricks", "minecraft:brick_block"));
			_mapper.Add(new BlockStateMapper("minecraft:dead_bush", "minecraft:deadbush"));

			_mapper.Add(new BlockStateMapper("minecraft:bubble_column",
				new PropertyStateMapper("drag", "drag_down")));

			_mapper.Add(new BlockStateMapper("minecraft:sculk_shrieker",
				new PropertyStateMapper("shrieking", "active")));

			_mapper.Add(new BlockStateMapper("minecraft:composter",
				new PropertyStateMapper("level", "composter_fill_level")));

			_mapper.Add(new BlockStateMapper("minecraft:coarse_dirt", "minecraft:dirt",
				(_, properties) => properties.Add(new NbtString("dirt_type", "coarse"))));

			_mapper.Add(new BlockStateMapper("minecraft:snow", "minecraft:snow_layer",
				new PropertyStateMapper("layers",
					(_, _, property) => new NbtString("height", (int.Parse(property.Value) - 1).ToString()))));

			_mapper.Add(new BlockStateMapper("minecraft:sweet_berry_bush",
				new PropertyStateMapper("age",
					(_, _, property) => new NbtString("growth", (int.Parse(property.Value) * 2 + 1).ToString()))));

			_mapper.Add(new BlockStateMapper("minecraft:small_dripleaf", "minecraft:small_dripleaf_block", upperBlockBitMap));

			var suspiciousMap = new BlockStateMapper(
				new PropertyStateMapper("dusted", "brushed_progress"));
			_mapper.Add("minecraft:suspicious_sand", suspiciousMap);
			_mapper.Add("minecraft:suspicious_gravel", suspiciousMap);

			var tallGrassMap = new BlockStateMapper("minecraft:tallgrass",
				(name, properties) => properties.Add(new NbtString("tall_grass_type", name.Replace("minecraft:", "").Replace("grass", "tall"))));
			_mapper.Add("minecraft:grass", tallGrassMap);
			_mapper.Add("minecraft:fern", tallGrassMap);


			_mapper.Add(new BlockStateMapper("minecraft:farmland",
				new PropertyStateMapper("moisture", "moisturized_amount")));

			#region Facing

			_mapper.Add("minecraft:glow_lichen", multiFaceDirectonMap);
			_mapper.Add("minecraft:sculk_vein", multiFaceDirectonMap);


			_mapper.Add(new BlockStateMapper("minecraft:small_amethyst_bud", facingDirectionMap));
			_mapper.Add(new BlockStateMapper("minecraft:medium_amethyst_bud", facingDirectionMap));
			_mapper.Add(new BlockStateMapper("minecraft:large_amethyst_bud", facingDirectionMap));
			_mapper.Add(new BlockStateMapper("minecraft:amethyst_cluster", facingDirectionMap));
			_mapper.Add(new BlockStateMapper("minecraft:ladder", facingDirectionMap));

			foreach (var color in _colorsList)
				_mapper.Add(new BlockStateMapper($"minecraft:{color}_glazed_terracotta", facingDirectionMap));

			_mapper.Add(new BlockStateMapper("minecraft:chest",
				facingDirectionMap,
				new SkipPropertyStateMapper("type")));


			_mapper.Add("minecraft:fire", faceDirectionSkipMap);
			_mapper.Add("minecraft:glass_pane", faceDirectionSkipMap);
			_mapper.Add("minecraft:nether_brick_fence", faceDirectionSkipMap);
			foreach (var wood in _woodList)
				_mapper.Add($"minecraft:{wood}_fence", faceDirectionSkipMap);


			_mapper.Add("minecraft:deepslate_redstone_ore", litMap);
			_mapper.Add("minecraft:redstone_ore", litMap);


			var bigDripleafMap = new BlockStateMapper("minecraft:big_dripleaf",
				new AdditionalPropertyStateMapper("big_dripleaf_head", (name, _) => name == "minecraft:big_dripleaf" ? "true" : "false"),
				new PropertyStateMapper("tilt", "big_dripleaf_tilt",
					new PropertyValueStateMapper("partial", "partial_tilt"),
					new PropertyValueStateMapper("full", "full_tilt")));

			_mapper.Add(bigDripleafMap);
			_mapper.Add("minecraft:big_dripleaf_stem", bigDripleafMap);

			#endregion

			#region Colored

			foreach (var color in _colorsList)
				_mapper.Add(new BlockStateMapper($"minecraft:{color}_terracotta", "minecraft:stained_hardened_clay",
				(name, properties) =>
				{
					properties.Clear();
					properties.Add(new NbtString("color", color));
				}));

			foreach (var color in _colorsList)
				_mapper.Add(new BlockStateMapper($"minecraft:{color}_stained_glass_pane", "minecraft:hard_stained_glass_pane",
				(name, properties) =>
				{
					properties.Clear();
					properties.Add(new NbtString("color", color));
				}));

			#endregion

			#region Liquid

			var liquidMap = new BlockStateMapper(
				new PropertyStateMapper("level", "liquid_depth"),
				new SkipPropertyStateMapper("falling"));

			_mapper.Add("minecraft:water", liquidMap);
			_mapper.Add("minecraft:lava", liquidMap);
			_mapper.Add("minecraft:flowing_water", liquidMap);
			_mapper.Add("minecraft:flowing_lava", liquidMap);

			#endregion

			#region Wood and Leaves

			var leavesMap = new BlockStateMapper(
				(name, properties) =>
				{
					var leavesType = name.Replace("minecraft:", "").Replace("_leaves", "");
					switch (leavesType)
					{
						case "oak":
						case "spruce":
						case "birch":
						case "jungle":
							properties.Add(new NbtString("old_leaf_type", leavesType));
							return "minecraft:leaves";
						case "acacia":
						case "dark_oak":
							properties.Add(new NbtString("new_leaf_type", leavesType));
							return "minecraft:leaves2";
					}

					return name;
				},
				new PropertyStateMapper("persistent", "persistent_bit"),
				new SkipPropertyStateMapper("distance"));

			_mapper.Add("minecraft:azalea_leaves", leavesMap);
			var floweringAzeleaLeavesMap = leavesMap.Clone();
			floweringAzeleaLeavesMap.BedrockName = "minecraft:azalea_leaves_flowered";
			_mapper.Add("minecraft:flowering_azalea_leaves", floweringAzeleaLeavesMap);
			foreach (var wood in _woodList)
				_mapper.Add($"minecraft:{wood}_leaves", leavesMap);


			var planksMap = new BlockStateMapper(
				(name, properties) =>
				{
					var planksType = name.Replace("minecraft:", "").Replace("_planks", "");
					switch (planksType)
					{
						case "oak":
						case "spruce":
						case "birch":
						case "jungle":
						case "acacia":
						case "dark_oak":
							properties.Add(new NbtString("wood_type", planksType));
							return "minecraft:planks";
					}

					return name;
				},
				new PropertyStateMapper("persistent", "persistent_bit"),
				new SkipPropertyStateMapper("distance"));

			foreach (var wood in _woodList)
				_mapper.Add($"minecraft:{wood}_planks", planksMap);

			#endregion

			#region Doors and Trapdoors

			var trapdoorMap = new BlockStateMapper(
				upsideDownBitMap,
				new PropertyStateMapper("facing", "direction",
					new PropertyValueStateMapper("east", "0"),
					new PropertyValueStateMapper("west", "1"),
					new PropertyValueStateMapper("south", "2"),
					new PropertyValueStateMapper("north", "3")),
				new PropertyStateMapper("open", "open_bit"));

			_mapper.Add($"minecraft:iron_trapdoor", trapdoorMap);
			foreach (var wood in _woodList)
				_mapper.Add($"minecraft:{wood}_trapdoor", trapdoorMap);

			var doorFacingDirectionMap = new PropertyStateMapper("facing", "direction",
					new PropertyValueStateMapper("east", "0"),
					new PropertyValueStateMapper("south", "1"),
					new PropertyValueStateMapper("west", "2"),
					new PropertyValueStateMapper("north", "3"));

			var doorMap = new BlockStateMapper(
				upperBlockBitMap,
				doorFacingDirectionMap,
				new PropertyStateMapper("open", "open_bit"),
				new PropertyStateMapper("hinge",
					(name, properties, _) => new NbtString("door_hinge_bit", (int.Parse(properties["direction"]?.StringValue ?? doorFacingDirectionMap.Resolve(name, properties, properties["facing"] as NbtString).Value) % 2).ToString())));

			_mapper.Add($"minecraft:iron_door", doorMap);
			foreach (var wood in _woodList)
				_mapper.Add($"minecraft:{wood}_door", doorMap);

			#endregion

			#region Stone

			var stoneMap = new BlockStateMapper((name, properties) =>
			{
				var stoneType = name.Replace("minecraft:", "");
				if (stoneType.StartsWith("polished_"))
					stoneType = stoneType.Replace("polished_", "") + "_smooth";

				properties.Add(new NbtString("stone_type", stoneType));
				return "minecraft:stone";
			});

			_mapper.Add("minecraft:granite", stoneMap);
			_mapper.Add("minecraft:polished_granite", stoneMap);
			_mapper.Add("minecraft:diorite", stoneMap);
			_mapper.Add("minecraft:polished_diorite", stoneMap);
			_mapper.Add("minecraft:andesite", stoneMap);
			_mapper.Add("minecraft:polished_andesite", stoneMap);
			_mapper.Add("minecraft:stone", stoneMap);

			#endregion

			#region Torch

			var tourchMap = new BlockStateMapper(
				(name, properties) =>
				{
					if (name.Contains("wall_"))
						return name.Replace("wall_", "");

					properties["facing"] = new NbtString("facing", "top");
					return name;
				},
				new PropertyStateMapper("facing", "torch_facing_direction",
					new PropertyValueStateMapper("west", "east"),
					new PropertyValueStateMapper("east", "west"),
					new PropertyValueStateMapper("north", "south"),
					new PropertyValueStateMapper("south", "north")));

			_mapper.Add("minecraft:torch", tourchMap);
			_mapper.Add("minecraft:wall_torch", tourchMap);
			_mapper.Add("minecraft:soul_torch", tourchMap);
			_mapper.Add("minecraft:soul_wall_torch", tourchMap);

			#endregion

			#region Stone bricks

			var stonebrickMap = new BlockStateMapper("minecraft:stonebrick",
				(name, properties) =>
				{
					var stonebrickType = name.Replace("minecraft:", "").Replace("_stone_bricks", "");
					if (stonebrickType == "stone_bricks")
						stonebrickType = "default";

					properties.Add(new NbtString("stone_brick_type", stonebrickType));
				});

			_mapper.Add("minecraft:stone_bricks", stonebrickMap);
			_mapper.Add("minecraft:cracked_stone_bricks", stonebrickMap);
			_mapper.Add("minecraft:mossy_stone_bricks", stonebrickMap);
			_mapper.Add("minecraft:chiseled_stone_bricks", stonebrickMap);

			#endregion

			#region Campfire

			var campFireMap = new BlockStateMapper(
				new PropertyStateMapper("lit", "extinguished",
					new PropertyValueStateMapper("true", "0"),
					new PropertyValueStateMapper("false", "1")),
				new SkipPropertyStateMapper("signal_fire"));

			_mapper.Add("minecraft:campfire", campFireMap);
			_mapper.Add("minecraft:soul_campfire", campFireMap);

			#endregion

			#region Flowers

			var flowerPlantMap = new BlockStateMapper("minecraft:red_flower",
				(name, properties) =>
				{
					var flowerName = name.Replace("minecraft:", "");
					flowerName = flowerName switch
					{
						"blue_orchid" => "orchid",
						"azure_bluet" => "houstonia",
						"red_tulip" => "tulip_red",
						"orange_tulip" => "tulip_orange",
						"white_tulip" => "tulip_white",
						"pink_tulip" => "tulip_pink",
						"oxeye_daisy" => "oxeye",
						_ => flowerName
					};

					properties.Add(new NbtString("flower_type", flowerName));
				},
				upperBlockBitMap);

			_mapper.Add("minecraft:poppy", flowerPlantMap);
			_mapper.Add("minecraft:blue_orchid", flowerPlantMap);
			_mapper.Add("minecraft:allium", flowerPlantMap);
			_mapper.Add("minecraft:azure_bluet", flowerPlantMap);
			_mapper.Add("minecraft:red_tulip", flowerPlantMap);
			_mapper.Add("minecraft:orange_tulip", flowerPlantMap);
			_mapper.Add("minecraft:white_tulip", flowerPlantMap);
			_mapper.Add("minecraft:pink_tulip", flowerPlantMap);
			_mapper.Add("minecraft:oxeye_daisy", flowerPlantMap);
			_mapper.Add("minecraft:cornflower", flowerPlantMap);
			_mapper.Add("minecraft:lily_of_the_valley", flowerPlantMap);

			#endregion

			#region Growth

			var growthMap = new BlockStateMapper(
				facingDirectionMap,
				new PropertyStateMapper("age", "growth"));

			_mapper.Add("minecraft:wheat", growthMap);
			_mapper.Add("minecraft:beetroot", growthMap);
			_mapper.Add("minecraft:carrots", growthMap);
			_mapper.Add("minecraft:potatoes", growthMap);
			_mapper.Add("minecraft:melon_stem", growthMap);
			_mapper.Add("minecraft:attached_melon_stem", growthMap);
			_mapper.Add("minecraft:pumpkin_stem", growthMap);
			_mapper.Add("minecraft:attached_pumpkin_stem", growthMap);
			_mapper.Add("minecraft:nether_wart", growthMap);

			#endregion


			// minecraft:cave_vines
			var caveVinesMap = new BlockStateMapper("minecraft:cave_vines",
				(name, properties) =>
				{
					var barries = properties["barries"]?.StringValue;

					if (barries != null)
					{
						properties.Remove("barries");

						return barries == "true"
						? name == "minecraft:cave_vines" ? "minecraft:cave_vines_head_with_berries" : "minecraft:cave_vines_body_with_berries"
						: name;
					}

					return name;
				},
				new PropertyStateMapper("age", "growing_plant_age"),
				new SkipPropertyStateMapper("berries"));

			_mapper.Add("minecraft:cave_vines", caveVinesMap);
			_mapper.Add("minecraft:cave_vines_plant", caveVinesMap);


			// minecraft:vine
			_mapper.Add(new BlockStateMapper("minecraft:vine",
				(name, properties) =>
				{
					var faceDirection = 0;

					faceDirection |= properties["south"].StringValue == "true" ? 1 << 0 : 0;
					faceDirection |= properties["west"].StringValue == "true" ? 1 << 1 : 0;
					faceDirection |= properties["north"].StringValue == "true" ? 1 << 2 : 0;
					faceDirection |= properties["east"].StringValue == "true" ? 1 << 3 : 0;

					properties.Clear();
					properties.Add(new NbtString("vine_direction_bits", faceDirection.ToString()));

					return name;
				}));


			// minecraft:kelp
			var kelpMap = new BlockStateMapper("minecraft:kelp",
				(name, properties) =>
				{
					if (properties["age"] != null)
						properties["age"] = new NbtString("age", Math.Min(15, int.Parse(properties["age"].StringValue)).ToString());
				},
				new PropertyStateMapper("age", "kelp_age"));

			_mapper.Add("minecraft:kelp", kelpMap);
			_mapper.Add("minecraft:kelp_plant", kelpMap);


			// minecraft:double_plant
			var doublePlantMap = new BlockStateMapper("minecraft:double_plant",
				(name, properties) =>
				{
					var flowerName = name.Replace("minecraft:", "");
					flowerName = flowerName switch
					{
						"tall_grass" => "grass",
						"large_fern" => "fern",
						"lilac" => "syringa",
						"rose_bush" => "rose",
						"peony" => "paeonia",
						_ => flowerName
					};

					properties.Add(new NbtString("double_plant_type", flowerName));
				},
				upperBlockBitMap);

			_mapper.Add("minecraft:tall_grass", doublePlantMap);
			_mapper.Add("minecraft:large_fern", doublePlantMap);
			_mapper.Add("minecraft:sunflower", doublePlantMap);
			_mapper.Add("minecraft:lilac", doublePlantMap);
			_mapper.Add("minecraft:rose_bush", doublePlantMap);
			_mapper.Add("minecraft:peony", doublePlantMap);


			// minecraft:seagrass
			var seagrassMap = new BlockStateMapper("minecraft:seagrass",
				(name, properties) =>
				{
					var grassType = "default";
					if (name == "minecraft:tall_seagrass")
						grassType = properties["half"].StringValue == "upper" ? "double_top" : "double_bot";

					properties.Add(new NbtString("sea_grass_type", grassType));
				},
				new SkipPropertyStateMapper("half"));

			_mapper.Add("minecraft:seagrass", seagrassMap);
			_mapper.Add("minecraft:tall_seagrass", seagrassMap);


			// minecraft:bell
			_mapper.Add(new BlockStateMapper("minecraft:bell",
				new PropertyStateMapper("attachment",
					new PropertyValueStateMapper("floor", "standing"),
					new PropertyValueStateMapper("ceiling", "hanging"),
					new PropertyValueStateMapper("single_wall", "side"),
					new PropertyValueStateMapper("double_wall", "multiple")),
				new PropertyStateMapper("powered", "toggle_bit")));


			// minecraft:rail
			_mapper.Add(new BlockStateMapper("minecraft:rail",
				new PropertyStateMapper("shape", "rail_direction",
					new PropertyValueStateMapper("north_south", "0"),
					new PropertyValueStateMapper("east_west", "1"),
					new PropertyValueStateMapper("ascending_east", "2"),
					new PropertyValueStateMapper("ascending_west", "3"),
					new PropertyValueStateMapper("ascending_north", "4"),
					new PropertyValueStateMapper("ascending_south", "5"),
					new PropertyValueStateMapper("south_east", "6"),
					new PropertyValueStateMapper("south_west", "7"),
					new PropertyValueStateMapper("north_west", "8"),
					new PropertyValueStateMapper("north_east", "9"))));


			// minecraft:sculk_sensor
			_mapper.Add(new BlockStateMapper("minecraft:sculk_sensor",
				new PropertyStateMapper("sculk_sensor_phase", "powered_bit",
					new PropertyValueStateMapper("inactive", "0"),
					new PropertyValueStateMapper("cooldown", "0"),
					new PropertyValueStateMapper("active", "1")),
				new SkipPropertyStateMapper("power")));


			// minecraft:infested_*
			foreach (var stone in _infestedStoneList)
				_mapper.Add(new BlockStateMapper($"minecraft:infested_{stone.Replace("brick", "bricks")}", "minecraft:monster_egg",
					(name, properties) =>
					{
						properties.Clear();
						properties.Add(new NbtString("monster_egg_stone_type", stone));
					}));

			// minecraft:*_stairs
			foreach (var material in _slabMaterialsList)
			{
				var bedrockName = material;
				if (material == "stone")
					bedrockName = "normal_stone";
				else if (material == "cobblestone")
					bedrockName = "stone";

				_mapper.Add(new BlockStateMapper($"minecraft:{material}_stairs", $"minecraft:{bedrockName}_stairs",
					upsideDownBitMap,
					new PropertyStateMapper("facing", "weirdo_direction",
						new PropertyValueStateMapper("east", "0"),
						new PropertyValueStateMapper("west", "1"),
						new PropertyValueStateMapper("south", "2"),
						new PropertyValueStateMapper("north", "3")),
					new SkipPropertyStateMapper("shape")));
			}


			// minecraft:*_wall
			var wallConnectionMap = new PropertyValueStateMapper("low", "short");

			foreach (var material in _slabMaterialsList)
			{
				var bedrockName = material switch
				{
					"blackstone" => "blackstone",
					"polished_blackstone" => "polished_blackstone",
					"polished_blackstone_brick" => "polished_blackstone_brick",
					"cobbled_deepslate" => "cobbled_deepslate",
					"polished_deepslate" => "polished_deepslate",
					"deepslate_brick" => "deepslate_brick",
					"deepslate_tile" => "deepslate_tile",
					"mud_brick" => "mud_brick",
					_ => "cobblestone"
				};

				_mapper.Add(new BlockStateMapper($"minecraft:{material}_wall", $"minecraft:{bedrockName}_wall",
					(name, properties) =>
					{
						if (material != bedrockName)
							properties.Add(new NbtString("wall_block_type", material));
					},
					new PropertyStateMapper("up", "wall_post_bit"),
					new PropertyStateMapper("east", "wall_connection_type_east", wallConnectionMap),
					new PropertyStateMapper("north", "wall_connection_type_north", wallConnectionMap),
					new PropertyStateMapper("south", "wall_connection_type_south", wallConnectionMap),
					new PropertyStateMapper("west", "wall_connection_type_west", wallConnectionMap)));
			}
		}

		public static int GetRuntimeIdByPalette(SubChunk chunk, NbtCompound palette)
		{
			var name = palette["Name"].StringValue;
			var properties = (NbtCompound) (palette["Properties"] as NbtCompound)?.Clone() ?? new NbtCompound();

			try
			{
				name = _mapper.Resolve(name, properties);

				var block = BlockFactory.GetBlockById(name);

				if (block == null)
				{
					Log.Warn($"Can't find block [{name}] with props1 [{palette["Properties"]}], props2 [{properties}]");
					return new InfoUpdate().GetRuntimeId();
				}

				var state = block.GetState();

				var result = FillProperties(state, properties);
				if (!result)
				{
					Log.Warn($"Can't find block [{name}] with props1 [{palette["Properties"]}], props2 [{properties}]");
					return new InfoUpdate().GetRuntimeId();
				}

				if (!BlockFactory.BlockStates.TryGetValue(state, out var blockstate))
				{
					Log.Warn($"Did not find block state for {block}, {state}");
					return new InfoUpdate().GetRuntimeId();
				}

				return blockstate.RuntimeId;
			}
			catch (Exception e)
			{
				Log.Warn($"Can't find block [{name}] with props1 [{palette["Properties"]}], props2 [{properties}]");
				Log.Error(e);
				return new InfoUpdate().GetRuntimeId();
			}
		}

		public static bool IsSeaBlock(string name)
		{
			return _seaBlocks.Contains(name);
		}

		public static Biome GetBiomeByName(string name)
		{
			var biomeName = name.Split(':').Last();

			var biome = BiomeUtils.GetBiome(_anvilBedrockBiomesMap.GetValueOrDefault(biomeName, biomeName));
			if (biome == null)
			{
				Log.Warn($"Missing biome [{name}]");
				return BiomeUtils.GetBiome(1);
			}

			return biome;
		}

		private static bool FillProperties(BlockStateContainer blockStateContainer, NbtCompound propertiesTag)
		{
			foreach (var prop in propertiesTag)
			{
				var state = blockStateContainer.States.FirstOrDefault(state => state.Name == prop.Name);

				if (state == null)
					return false;

				var value = prop.StringValue;
				switch (state)
				{
					case BlockStateString blockStateString:
						blockStateString.Value = value;
						break;
					case BlockStateByte blockStateByte:
						blockStateByte.Value = value switch
						{
							"false" => 0,
							"true" => 1,
							_ => byte.Parse(value)
						};
						break;
					case BlockStateInt blockStateByte:
						blockStateByte.Value = int.Parse(value);
						break;
				}
			}

			return true;
		}

		public class AnvilToBedrockStateMapper
		{
			private readonly Dictionary<string, BlockStateMapper> _map = new Dictionary<string, BlockStateMapper>();
			private readonly List<BlockStateMapper> _defaultMap = new List<BlockStateMapper>();

			public void Add(BlockStateMapper map)
			{
				Add(map.AnvilName, map);
			}

			public void Add(string name, BlockStateMapper map)
			{
				_map.Add(name, map);
			}

			public void AddDefault(BlockStateMapper map)
			{
				_defaultMap.Add(map);
			}

			public string Resolve(string anvilName, NbtCompound properties)
			{
				if (_map.TryGetValue(anvilName, out var map))
					anvilName = map.Resolve(anvilName, properties);

				foreach (var defMap in _defaultMap)
					defMap.ResolveDefault(anvilName, properties);

				return anvilName;
			}
		}

		public class BlockStateMapper
		{
			public string AnvilName { get; set; }
			public string BedrockName { get; set; }

			public Dictionary<string, PropertyStateMapper> PropertiesMap { get; } = new Dictionary<string, PropertyStateMapper>();
			public List<AdditionalPropertyStateMapper> AdditionalProperties { get; } = new List<AdditionalPropertyStateMapper>();
			public Dictionary<string, SkipPropertyStateMapper> SkipProperties { get; } = new Dictionary<string, SkipPropertyStateMapper>();

			private readonly Func<string, NbtCompound, string> _func;

			public BlockStateMapper(Func<string, NbtCompound, string> func)
			{
				_func = func;
			}

			public BlockStateMapper(Action<string, NbtCompound> func, params IPropertyStateMapper[] propertiesMap)
				: this(null, null, func, propertiesMap)
			{

			}

			public BlockStateMapper(Func<string, NbtCompound, string> func, params IPropertyStateMapper[] propertiesMap)
				: this(null, null, func, propertiesMap)
			{

			}

			public BlockStateMapper(string anvilName, Action<string, NbtCompound> func)
				: this(anvilName, anvilName, func)
			{

			}

			public BlockStateMapper(string anvilName, Func<string, NbtCompound, string> func)
				: this(anvilName, anvilName, func)
			{

			}

			public BlockStateMapper(string anvilName, Action<string, NbtCompound> func, params IPropertyStateMapper[] propertiesMap)
				: this(anvilName, anvilName, func, propertiesMap)
			{

			}

			public BlockStateMapper(string anvilName, Func<string, NbtCompound, string> func, params IPropertyStateMapper[] propertiesMap)
				: this(anvilName, anvilName, func, propertiesMap)
			{

			}

			public BlockStateMapper(params IPropertyStateMapper[] propertiesMap)
				: this(anvilName: null, propertiesMap)
			{

			}

			public BlockStateMapper(string anvilName, params IPropertyStateMapper[] propertiesMap)
				: this(anvilName, anvilName, null, propertiesMap)
			{

			}

			public BlockStateMapper(string anvilName, string bedrockName, params IPropertyStateMapper[] propertiesMap)
				: this(anvilName, bedrockName, null, propertiesMap)
			{

			}

			public BlockStateMapper(string anvilName, string bedrockName, Action<string, NbtCompound> func, params IPropertyStateMapper[] propertiesMap)
				: this(anvilName, bedrockName, (name, properties) =>
				{
					func(name, properties);
					return name;
				}, propertiesMap)
			{
			}

			public BlockStateMapper(string anvilName, string bedrockName, Func<string, NbtCompound, string> func, params IPropertyStateMapper[] propertiesMap)
			{
				AnvilName = anvilName;
				BedrockName = bedrockName;

				_func = func;

				foreach (var map in propertiesMap)
					if (map is PropertyStateMapper propertyStateMapper)
						PropertiesMap.Add(propertyStateMapper.AnvilName ?? propertyStateMapper.GetHashCode().ToString(), propertyStateMapper);
					else if (map is AdditionalPropertyStateMapper additionalPropertyStateMapper)
						AdditionalProperties.Add(additionalPropertyStateMapper);
					else if (map is SkipPropertyStateMapper skipPropertyStateMapper)
						SkipProperties.Add(skipPropertyStateMapper.Name ?? skipPropertyStateMapper.GetHashCode().ToString(), skipPropertyStateMapper);
			}

			public string Resolve(string anvilName, NbtCompound properties)
			{
				if (_func != null)
					anvilName = _func(anvilName, properties);

				foreach (NbtString prop in properties.ToArray())
					if (SkipProperties.TryGetValue(prop.Name, out var skipMap) && skipMap.Resolve(anvilName, properties, prop))
						properties.Remove(prop.Name);
					else if (PropertiesMap.TryGetValue(prop.Name, out var propMap))
					{
						properties.Remove(prop.Name);
						properties.Add(propMap.Resolve(anvilName, properties, prop));
					}

				foreach (var prop in AdditionalProperties)
					properties[prop.Name] = prop.Resolve(anvilName, properties);

				return BedrockName ?? anvilName;
			}

			public string ResolveDefault(string anvilName, NbtCompound properties)
			{
				if (_func != null)
					return _func(anvilName, properties);

				foreach (NbtString prop in properties.ToArray())
				{
					var skipMap = SkipProperties.Values.FirstOrDefault(map => map.Name == prop.Name || map.Name == null);
					if (skipMap != null && skipMap.Resolve(anvilName, properties, prop))
						properties.Remove(prop.Name);
					else
					{
						var propMap = PropertiesMap.Values.FirstOrDefault(map => map.AnvilName == prop.Name || map.AnvilName == null);

						if (propMap != null)
						{
							properties.Remove(prop.Name);
							properties.Add(propMap.Resolve(anvilName, properties, prop));
						}
					}
				}

				foreach (var prop in AdditionalProperties)
					properties[prop.Name] = prop.Resolve(anvilName, properties);

				return BedrockName ?? anvilName;
			}

			public BlockStateMapper Clone()
			{
				var propertiesMap = new List<IPropertyStateMapper>();

				foreach (var property in PropertiesMap.Values)
					propertiesMap.Add(property.Clone());

				foreach (var property in AdditionalProperties)
					propertiesMap.Add(property.Clone());

				foreach (var property in SkipProperties.Values)
					propertiesMap.Add(property.Clone());

				return new BlockStateMapper(AnvilName, BedrockName, (Func<string, NbtCompound, string>) _func?.Clone(), propertiesMap.ToArray());
			}
		}

		public interface IPropertyStateMapper { }

		public class PropertyStateMapper : IPropertyStateMapper
		{
			public string AnvilName { get; set; }
			public string BedrockName { get; set; }

			public Dictionary<string, PropertyValueStateMapper> ValuesMap { get; } = new Dictionary<string, PropertyValueStateMapper>();

			private readonly Func<string, NbtCompound, NbtString, NbtString> _func;

			public PropertyStateMapper(params PropertyValueStateMapper[] propertiesNameMap)
				: this(anvilName: null, bedrockName: null, propertiesNameMap)
			{

			}

			public PropertyStateMapper(Func<string, NbtCompound, NbtString, NbtString> func)
				: this(anvilName: null, func)
			{

			}

			public PropertyStateMapper(string anvilName, params PropertyValueStateMapper[] propertiesNameMap)
				: this(anvilName, bedrockName: null, propertiesNameMap)
			{

			}

			public PropertyStateMapper(string anvilName, string bedrockName, params PropertyValueStateMapper[] propertiesNameMap)
			{
				AnvilName = anvilName;
				BedrockName = bedrockName;

				foreach (var map in propertiesNameMap)
					ValuesMap.Add(map.AnvilName, map);
			}

			public PropertyStateMapper(string anvilName, Func<string, NbtCompound, NbtString, NbtString> func, params PropertyValueStateMapper[] propertiesNameMap)
			{
				AnvilName = anvilName;
				_func = func;

				foreach (var map in propertiesNameMap)
					ValuesMap.Add(map.AnvilName, map);
			}

			public NbtString Resolve(string anvilName, NbtCompound properties, NbtString property)
			{
				return _func?.Invoke(anvilName, properties, property)
					?? new NbtString(BedrockName ?? property.Name, ValuesMap.GetValueOrDefault(property.StringValue)?.Resolve(anvilName, properties) ?? property.StringValue);
			}

			public PropertyStateMapper Clone()
			{
				return new PropertyStateMapper(
					AnvilName,
					(Func<string, NbtCompound, NbtString, NbtString>) _func?.Clone(),
					ValuesMap.Values.Select(v => v.Clone()).ToArray())
				{
					BedrockName = BedrockName
				};
			}
		}

		public class AdditionalPropertyStateMapper : IPropertyStateMapper
		{
			public string Name { get; set; }
			public string Value { get; set; }

			private readonly Func<string, NbtCompound, string> _func;

			public AdditionalPropertyStateMapper(string name, string value)
			{
				Name = name;
				Value = value;
			}

			public AdditionalPropertyStateMapper(string name, Func<string, NbtCompound, string> func)
			{
				Name = name;
				_func = func;
			}

			public NbtString Resolve(string anvilName, NbtCompound properties)
			{
				return new NbtString(Name, _func?.Invoke(anvilName, properties) ?? Value);
			}

			public AdditionalPropertyStateMapper Clone()
			{
				return new AdditionalPropertyStateMapper(
					Name,
					(Func<string, NbtCompound, string>) _func?.Clone())
				{
					Value = Value
				};
			}
		}

		public class SkipPropertyStateMapper : IPropertyStateMapper
		{
			public string Name { get; set; }

			private readonly Func<string, NbtCompound, NbtString, bool> _func;

			public SkipPropertyStateMapper(string name)
			{
				Name = name;
			}

			public SkipPropertyStateMapper(string name, Func<string, NbtCompound, NbtString, bool> func)
			{
				Name = name;
				_func = func;
			}

			public bool Resolve(string anvilName, NbtCompound properties, NbtString value)
			{
				return _func?.Invoke(anvilName, properties, value) ?? true;
			}

			public SkipPropertyStateMapper Clone()
			{
				return new SkipPropertyStateMapper(Name, (Func<string, NbtCompound, NbtString, bool>) _func?.Clone());
			}
		}

		public class PropertyValueStateMapper
		{
			public string AnvilName { get; set; }
			public string BedrockName { get; set; }

			private readonly Func<string, NbtCompound, string, string> _func;

			public PropertyValueStateMapper(string anvilName, string bedrockName)
			{
				AnvilName = anvilName;
				BedrockName = bedrockName;
			}

			public PropertyValueStateMapper(string anvilName, Func<string, NbtCompound, string, string> func)
			{
				AnvilName = anvilName;
				_func = func;
			}

			public string Resolve(string anvilName, NbtCompound properties)
			{
				return _func?.Invoke(anvilName, properties, AnvilName) ?? BedrockName;
			}

			public PropertyValueStateMapper Clone()
			{
				return new PropertyValueStateMapper(
					AnvilName,
					(Func<string, NbtCompound, string, string>) _func?.Clone())
				{
					BedrockName = BedrockName
				};
			}
		}
	}
}
