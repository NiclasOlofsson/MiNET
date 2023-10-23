using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using fNbt;
using log4net;
using MiNET.BlockEntities;
using MiNET.Blocks;
using MiNET.Entities.Hostile;
using MiNET.Utils;

namespace MiNET.Worlds.Anvil
{
	public class AnvilPaletteConverter
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(AnvilPaletteConverter));

		private const string AnvilIncompatibleBitName = "anvil_incompatible_bit";

		private static readonly AnvilToBedrockStateMapper _mapper = new AnvilToBedrockStateMapper();

		private static readonly Dictionary<string, string> _anvilBedrockBiomesMap = new Dictionary<string, string>
		{
			{ "old_growth_pine_taiga", "mega_taiga" },
			{ "old_growth_spruce_taiga", "redwood_taiga_mutated" },
			{ "stony_shore", "stone_beach" },
			{ "snowy_beach", "cold_beach" },
			{ "snowy_taiga", "cold_taiga" },
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

		private static readonly string[] _skullsList = new[]
		{
			"skeleton_skull",
			"wither_skeleton_skull",
			"zombie_head",
			"player_head",
			"creeper_head",
			"dragon_head",
			"piglin_head",
			"skeleton_wall_skull",
			"wither_skeleton_wall_skull",
			"zombie_wall_head",
			"player_wall_head",
			"creeper_wall_head",
			"dragon_wall_head",
			"piglin_wall_head"
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
			"smooth_stone",
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

		private static readonly string[] _pottedPlantsList = new[]
		{
			"dandelion",
			"poppy",
			"blue_orchid",
			"allium",
			"azure_bluet",
			"red_tulip",
			"orange_tulip",
			"white_tulip",
			"pink_tulip",
			"oxeye_daisy",
			"cornflower",
			"lily_of_the_valley",
			"wither_rose",
			"oak_sapling",
			"spruce_sapling",
			"birch_sapling",
			"jungle_sapling",
			"acacia_sapling",
			"dark_oak_sapling",
			"red_mushroom",
			"brown_mushroom",
			"fern",
			"dead_bush",
			"cactus",
			"bamboo",
			"azalea_bush",
			"flowering_azalea_bush",
			"crimson_fungus",
			"warped_fungus",
			"crimson_roots",
			"warped_roots",
			"mangrove_propagule"
		};

		static AnvilPaletteConverter()
		{
			var poweredSkipMap = 
				new SkipPropertyStateMapper("powered");
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
			var directionMap = new PropertyStateMapper("facing", "direction",
					new PropertyValueStateMapper("south", "0"),
					new PropertyValueStateMapper("west", "1"),
					new PropertyValueStateMapper("north", "2"),
					new PropertyValueStateMapper("east", "3"));
			var directionMap2 = new PropertyStateMapper("facing", "direction",
					new PropertyValueStateMapper("north", "0"),
					new PropertyValueStateMapper("east", "1"),
					new PropertyValueStateMapper("south", "2"),
					new PropertyValueStateMapper("west", "3"));

			var multiFaceDirectonMap = new BlockStateMapper(
				context =>
				{
					var faceDirection = 0;

					faceDirection |= context.Properties["down"].StringValue == "true" ? 1 << 0 : 0;
					faceDirection |= context.Properties["up"].StringValue == "true" ? 1 << 1 : 0;
					faceDirection |= context.Properties["south"].StringValue == "true" ? 1 << 2 : 0;
					faceDirection |= context.Properties["west"].StringValue == "true" ? 1 << 3 : 0;
					faceDirection |= context.Properties["north"].StringValue == "true" ? 1 << 4 : 0;
					faceDirection |= context.Properties["east"].StringValue == "true" ? 1 << 5 : 0;

					context.Properties.Clear();
					context.Properties.Add(new NbtString("multi_face_direction_bits", faceDirection.ToString()));
				});
			var litMap = new BlockStateMapper(
				context =>
				{
					var litName = context.Properties["lit"].StringValue == "true" ? context.AnvilName.Replace("minecraft:", "minecraft:lit_") : context.AnvilName;
					context.Properties.Remove("lit");

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
				poweredSkipMap));


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
			_mapper.Add(new BlockStateMapper("minecraft:bricks", "minecraft:brick_block"));
			_mapper.Add(new BlockStateMapper("minecraft:dead_bush", "minecraft:deadbush"));
			_mapper.Add(new BlockStateMapper("minecraft:terracotta", "minecraft:hardened_clay"));
			_mapper.Add(new BlockStateMapper("minecraft:lily_pad", "minecraft:waterlily"));
			_mapper.Add(new BlockStateMapper("minecraft:nether_bricks", "minecraft:nether_brick"));
			_mapper.Add(new BlockStateMapper("minecraft:red_nether_bricks", "minecraft:red_nether_brick"));
			_mapper.Add(new BlockStateMapper("minecraft:slime_block", "minecraft:slime"));
			_mapper.Add(new BlockStateMapper("minecraft:melon", "minecraft:melon_block"));


			_mapper.Add(new BlockStateMapper("minecraft:note_block", "minecraft:noteblock",
				new SkipPropertyStateMapper("instrument"),
				new SkipPropertyStateMapper("note"),
				poweredSkipMap));

			_mapper.Add(new BlockStateMapper("minecraft:jukebox",
				new SkipPropertyStateMapper("has_record")));

			_mapper.Add(new BlockStateMapper("minecraft:bubble_column",
				new PropertyStateMapper("drag", "drag_down")));

			_mapper.Add(new BlockStateMapper("minecraft:tnt",
				new PropertyStateMapper("unstable", "explode_bit")));

			_mapper.Add(new BlockStateMapper("minecraft:jack_o_lantern", "minecraft:lit_pumpkin", directionMap));

			_mapper.Add(new BlockStateMapper("minecraft:sculk_shrieker",
				new PropertyStateMapper("shrieking", "active")));

			_mapper.Add(new BlockStateMapper("minecraft:composter",
				new PropertyStateMapper("level", "composter_fill_level")));

			_mapper.Add(new BlockStateMapper("minecraft:coarse_dirt", "minecraft:dirt",
				context => context.Properties.Add(new NbtString("dirt_type", "coarse"))));

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
				context => context.Properties.Add(new NbtString("tall_grass_type", context.AnvilName.Replace("minecraft:", "").Replace("grass", "tall"))));
			_mapper.Add("minecraft:grass", tallGrassMap);
			_mapper.Add("minecraft:fern", tallGrassMap);


			_mapper.Add(new BlockStateMapper("minecraft:farmland",
				new PropertyStateMapper("moisture", "moisturized_amount")));

			_mapper.Add(new BlockStateMapper("minecraft:red_sand", "minecraft:sand",
				new AdditionalPropertyStateMapper("sand_type", "red")));

			#region Facing

			_mapper.Add("minecraft:glow_lichen", multiFaceDirectonMap);
			_mapper.Add("minecraft:sculk_vein", multiFaceDirectonMap);


			_mapper.Add(new BlockStateMapper("minecraft:small_amethyst_bud", facingDirectionMap));
			_mapper.Add(new BlockStateMapper("minecraft:medium_amethyst_bud", facingDirectionMap));
			_mapper.Add(new BlockStateMapper("minecraft:large_amethyst_bud", facingDirectionMap));
			_mapper.Add(new BlockStateMapper("minecraft:amethyst_cluster", facingDirectionMap));
			_mapper.Add(new BlockStateMapper("minecraft:ladder", facingDirectionMap));
			_mapper.Add(new BlockStateMapper("minecraft:end_rod", facingDirectionMap));
			_mapper.Add(new BlockStateMapper("minecraft:dropper",
				facingDirectionMap, 
				new BitPropertyStateMapper("triggered")));
			_mapper.Add(new BlockStateMapper("minecraft:observer",
				facingDirectionMap,
				new BitPropertyStateMapper("powered")));
			_mapper.Add(new BlockStateMapper("minecraft:hopper",
				facingDirectionMap,
				new PropertyStateMapper("enabled", "toggle_bit",
					new PropertyValueStateMapper("true", "false"),
					new PropertyValueStateMapper("false", "true"))));

			_mapper.Add(new BlockStateMapper("minecraft:ender_chest", facingDirectionMap));
			_mapper.Add(new BlockStateMapper("minecraft:chest",
				facingDirectionMap,
				new SkipPropertyStateMapper("type")));
			_mapper.Add(new BlockStateMapper("minecraft:trapped_chest",
				facingDirectionMap,
				new SkipPropertyStateMapper("type")));

			var furnaceMap = litMap.Clone();
			furnaceMap.PropertiesMap.Add(facingDirectionMap.AnvilName, facingDirectionMap);
			_mapper.Add("minecraft:furnace", furnaceMap);


			_mapper.Add("minecraft:fire", faceDirectionSkipMap);
			_mapper.Add("minecraft:iron_bars", faceDirectionSkipMap);
			_mapper.Add("minecraft:glass_pane", faceDirectionSkipMap);
			_mapper.Add("minecraft:nether_brick_fence", faceDirectionSkipMap);
			foreach (var wood in _woodList)
				_mapper.Add($"minecraft:{wood}_fence", faceDirectionSkipMap);


			_mapper.Add("minecraft:deepslate_redstone_ore", litMap);
			_mapper.Add("minecraft:redstone_ore", litMap);
			_mapper.Add("minecraft:redstone_lamp", litMap);


			var bigDripleafMap = new BlockStateMapper("minecraft:big_dripleaf",
				new AdditionalPropertyStateMapper("big_dripleaf_head", (name, _) => name == "minecraft:big_dripleaf" ? "true" : "false"),
				new PropertyStateMapper("tilt", "big_dripleaf_tilt",
					new PropertyValueStateMapper("partial", "partial_tilt"),
					new PropertyValueStateMapper("full", "full_tilt")));

			_mapper.Add(bigDripleafMap);
			_mapper.Add("minecraft:big_dripleaf_stem", bigDripleafMap);

			var fenceGateMap = new BlockStateMapper(
				directionMap,
				new BitPropertyStateMapper("in_wall"),
				new BitPropertyStateMapper("open"));

			var oakFenceGateMap = fenceGateMap.Clone();
			oakFenceGateMap.BedrockName = "minecraft:fence_gate";
			_mapper.Add($"minecraft:oak_fence_gate", oakFenceGateMap);
			foreach (var wood in _woodList)
				_mapper.TryAdd($"minecraft:{wood}_fence_gate", fenceGateMap);

			#endregion

			#region Colored

			foreach (var color in _colorsList)
				_mapper.Add(new BlockStateMapper($"minecraft:{color}_terracotta", "minecraft:stained_hardened_clay",
					context =>
					{
						context.Properties.Clear();
						context.Properties.Add(new NbtString("color", color.Replace("light_gray", "silver")));
					}));

			_mapper.Add(new BlockStateMapper($"minecraft:light_gray_glazed_terracotta", "minecraft:silver_glazed_terracotta", facingDirectionMap));
			foreach (var color in _colorsList)
				_mapper.TryAdd(new BlockStateMapper($"minecraft:{color}_glazed_terracotta", facingDirectionMap));

			foreach (var color in _colorsList)
				_mapper.Add(new BlockStateMapper($"minecraft:{color}_stained_glass_pane", "minecraft:stained_glass_pane",
					context =>
					{
						context.Properties.Clear();
						context.Properties.Add(new NbtString("color", color.Replace("light_gray", "silver")));
					}));

			foreach (var color in _colorsList)
				_mapper.Add(new BlockStateMapper($"minecraft:{color}_stained_glass", "minecraft:stained_glass",
					context =>
					{
						context.Properties.Clear();
						context.Properties.Add(new NbtString("color", color.Replace("light_gray", "silver")));
					}));

			foreach (var color in _colorsList)
				_mapper.Add(new BlockStateMapper($"minecraft:{color}_carpet", "minecraft:carpet",
					context =>
					{
						context.Properties.Clear();
						context.Properties.Add(new NbtString("color", color.Replace("light_gray", "silver")));
					}));

			_mapper.Add(new BlockStateMapper($"minecraft:shulker_box", "minecraft:undyed_shulker_box",
					context => context.Properties.Clear()));
			foreach (var color in _colorsList)
				_mapper.Add(new BlockStateMapper($"minecraft:{color}_shulker_box", "minecraft:shulker_box",
					context =>
					{
						context.Properties.Clear();
						context.Properties.Add(new NbtString("color", color.Replace("light_gray", "silver")));
					}));

			for (var i = 0; i < _colorsList.Length; i++)
			{
				var color = _colorsList[i];
				var blockEntity = new BedBlockEntity() { Color = (byte) i };

				_mapper.Add(new BlockStateMapper($"minecraft:{color}_bed", "minecraft:bed",
					context => context.BlockEntityTemplate = blockEntity,
					new PropertyStateMapper("part", "head_piece_bit",
						new PropertyValueStateMapper("foot", "false"),
						new PropertyValueStateMapper("head", "true")),
					directionMap,
					new BitPropertyStateMapper("occupied")));
			}

			for (var i = 0; i < _colorsList.Length; i++)
			{
				var color = _colorsList[i];
				var blockEntity = new BannerBlockEntity() { Base = 15 - i };

				var banerMap = new BlockStateMapper(context =>
				{
					var name = context.AnvilName.Replace("minecraft:", "");

					context.BlockEntityTemplate = blockEntity;

					return context.AnvilName.Contains("_wall_banner") ? "minecraft:wall_banner" : "minecraft:standing_banner";
				},
					facingDirectionMap,
					new PropertyStateMapper("rotation", "ground_sign_direction"));

				_mapper.Add($"minecraft:{color}_banner", banerMap);
				_mapper.Add($"minecraft:{color}_wall_banner", banerMap);
			}

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
				context =>
				{
					var leavesType = context.AnvilName.Replace("minecraft:", "").Replace("_leaves", "");
					switch (leavesType)
					{
						case "oak":
						case "spruce":
						case "birch":
						case "jungle":
							context.Properties.Add(new NbtString("old_leaf_type", leavesType));
							return "minecraft:leaves";
						case "acacia":
						case "dark_oak":
							context.Properties.Add(new NbtString("new_leaf_type", leavesType));
							return "minecraft:leaves2";
					}

					return context.AnvilName;
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
				context =>
				{
					var planksType = context.AnvilName.Replace("minecraft:", "").Replace("_planks", "");
					switch (planksType)
					{
						case "oak":
						case "spruce":
						case "birch":
						case "jungle":
						case "acacia":
						case "dark_oak":
							context.Properties.Add(new NbtString("wood_type", planksType));
							return "minecraft:planks";
					}

					return context.AnvilName;
				},
				new PropertyStateMapper("persistent", "persistent_bit"),
				new SkipPropertyStateMapper("distance"));

			foreach (var wood in _woodList)
				_mapper.Add($"minecraft:{wood}_planks", planksMap);


			var woodMap = new BlockStateMapper(
				context =>
				{
					var woodType = context.AnvilName.Replace("minecraft:", "").Replace("stripped_", "").Replace("_wood", "");
					switch (woodType)
					{
						case "oak":
						case "spruce":
						case "birch":
						case "jungle":
						case "acacia":
						case "dark_oak":
							context.Properties.Add(new NbtString("wood_type", woodType));
							if (context.AnvilName.Contains("stripped_"))
							{
								context.Properties.Add(new NbtString("stripped_bit", "true"));
							}
							return "minecraft:wood";
					}

					return context.AnvilName;
				},
				new PropertyStateMapper("persistent", "persistent_bit"),
				new SkipPropertyStateMapper("distance"));

			foreach (var wood in _woodList)
			{
				_mapper.Add($"minecraft:{wood}_wood", woodMap);
				_mapper.Add($"minecraft:stripped_{wood}_wood", woodMap);
			}

			#endregion

			#region Sings

			var singMap = new BlockStateMapper(context =>
				{
					var name = context.AnvilName.Replace("minecraft:", "");
					if (name.Contains("_hanging") && !name.Contains("_wall"))
					{
						context.Properties.Add(new NbtString("hanging", "true"));
					}

					if (name.Replace("_sign", "").Split('_').Length == 1)
					{
						name = name.Replace("_sign", "_standing_sign");
					}

					if (!name.Contains("_hanging"))
					{
						name = name.Replace("oak_", "");
					}

					return $"minecraft:{name.Replace("_wall_hanging", "_hanging")}";
				},
				facingDirectionMap,
				new PropertyStateMapper("rotation", "ground_sign_direction"));

			foreach (var wood in _woodList)
			{
				_mapper.Add($"minecraft:{wood}_sign", singMap);
				_mapper.Add($"minecraft:{wood}_wall_sign", singMap);
				_mapper.Add($"minecraft:{wood}_hanging_sign", singMap);
				_mapper.Add($"minecraft:{wood}_wall_hanging_sign", singMap);
			}

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

			var oakTrapdoorMap = trapdoorMap.Clone();
			oakTrapdoorMap.BedrockName = "minecraft:trapdoor";
			_mapper.Add($"minecraft:oak_trapdoor", oakTrapdoorMap);
			_mapper.Add($"minecraft:iron_trapdoor", trapdoorMap);
			foreach (var wood in _woodList)
				_mapper.TryAdd($"minecraft:{wood}_trapdoor", trapdoorMap);

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

			var oakDoorMap = doorMap.Clone();
			oakDoorMap.BedrockName = "minecraft:wooden_door";
			_mapper.Add($"minecraft:oak_door", oakDoorMap);
			_mapper.Add($"minecraft:iron_door", doorMap);
			foreach (var wood in _woodList)
				_mapper.TryAdd($"minecraft:{wood}_door", doorMap);

			var buttonMap = new BlockStateMapper(
				context =>
				{
					var face = context.Properties["face"].StringValue;
					var facing = context.Properties["facing"].StringValue;

					var direction = (face, facing) switch
					{
						("ceiling", _) => "0",
						("floor", _) => "1",
						("wall", "north") => "2",
						("wall", "south") => "3",
						("wall", "west") => "4",
						("wall", "east") => "5",
						_ => "0"
					};

					context.Properties.Add(new NbtString("facing_direction", direction));
				},
				new PropertyStateMapper("powered", "button_pressed_bit"),
				new SkipPropertyStateMapper("face"),
				new SkipPropertyStateMapper("facing"));

			_mapper.TryAdd($"minecraft:stone_button", buttonMap);
			_mapper.TryAdd($"minecraft:polished_blackstone_button", buttonMap);
			var oakButtonMap = buttonMap.Clone();
			oakButtonMap.BedrockName = "minecraft:wooden_button";
			_mapper.TryAdd($"minecraft:oak_button", oakButtonMap);
			foreach (var wood in _woodList)
				_mapper.TryAdd($"minecraft:{wood}_button", buttonMap);

			#endregion

			#region Stone

			var stoneMap = new BlockStateMapper(context =>
			{
				var stoneType = context.AnvilName.Replace("minecraft:", "");
				if (stoneType.StartsWith("polished_"))
					stoneType = stoneType.Replace("polished_", "") + "_smooth";

				context.Properties.Add(new NbtString("stone_type", stoneType));
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
				context =>
				{
					if (context.AnvilName.Contains("wall_"))
					{
						context.AnvilName = context.AnvilName.Replace("wall_", "");
					}
					else
					{
						context.Properties["facing"] = new NbtString("facing", "top");
					}

					return context.Properties["lit"]?.StringValue == "false" ? $"minecraft:unlit_{context.AnvilName.Replace("minecraft:", "")}" : context.AnvilName;
				},
				new PropertyStateMapper("facing", "torch_facing_direction",
					new PropertyValueStateMapper("west", "east"),
					new PropertyValueStateMapper("east", "west"),
					new PropertyValueStateMapper("north", "south"),
					new PropertyValueStateMapper("south", "north")),
				new SkipPropertyStateMapper("lit"));

			_mapper.Add("minecraft:torch", tourchMap);
			_mapper.Add("minecraft:wall_torch", tourchMap);
			_mapper.Add("minecraft:soul_torch", tourchMap);
			_mapper.Add("minecraft:soul_wall_torch", tourchMap);
			_mapper.Add("minecraft:redstone_torch", tourchMap);
			_mapper.Add("minecraft:redstone_wall_torch", tourchMap);

			#endregion

			#region Pressure plates

			var pressurePlateMap = new BlockStateMapper(
				new PropertyStateMapper("powered", "redstone_signal",
					new PropertyValueStateMapper("false", "0"),
					new PropertyValueStateMapper("true", "1")));

			var woodenPressurePlateMap = pressurePlateMap.Clone();
			woodenPressurePlateMap.BedrockName = "minecraft:wooden_pressure_plate";

			_mapper.Add($"minecraft:oak_pressure_plate", woodenPressurePlateMap);
			_mapper.Add($"minecraft:stone_pressure_plate", pressurePlateMap);
			_mapper.Add($"minecraft:polished_blackstone_pressure_plate", pressurePlateMap);
			foreach (var wood in _woodList)
			{
				_mapper.TryAdd($"minecraft:{wood}_pressure_plate", pressurePlateMap);
			}

			var weightedPressurePlateMap = new BlockStateMapper(
				new PropertyStateMapper("power", "redstone_signal"));

			_mapper.Add($"minecraft:light_weighted_pressure_plate", weightedPressurePlateMap);
			_mapper.Add($"minecraft:heavy_weighted_pressure_plate", weightedPressurePlateMap);

			#endregion

			#region Stone bricks

			var stonebrickMap = new BlockStateMapper("minecraft:stonebrick",
				context =>
				{
					var stonebrickType = context.AnvilName.Replace("minecraft:", "").Replace("_stone_bricks", "");
					if (stonebrickType == "stone_bricks")
						stonebrickType = "default";

					context.Properties.Add(new NbtString("stone_brick_type", stonebrickType));
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
				context =>
				{
					var flowerName = context.AnvilName.Replace("minecraft:", "");
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

					context.Properties.Add(new NbtString("flower_type", flowerName));
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

			#region Flower pot

			var flowerPotMap = new BlockStateMapper(
				context =>
				{
					var plantType = context.AnvilName.Replace("minecraft:", "").Replace("potted_", "");
					plantType = plantType switch
					{
						"blue_orchid" => "orchid",
						"azure_bluet" => "houstonia",
						"red_tulip" => "tulip_red",
						"orange_tulip" => "tulip_orange",
						"white_tulip" => "tulip_white",
						"pink_tulip" => "tulip_pink",
						"oxeye_daisy" => "oxeye",
						_ => plantType
					};

					var block = BlockFactory.GetBlockById($"minecraft:{plantType}") ?? new RedFlower() { FlowerType = plantType };

					if (block.GetRuntimeId() >= 0)
					{
						context.Properties.Add(new NbtString("update_bit", "true"));
					}
					else
					{
						block = null;
						Log.Warn($"Can't find plant block for flower pot [{plantType}]");
					}

					context.BlockEntityTemplate = new FlowerPotBlockEntity()
					{
						PlantBlock = block
					};

					return "minecraft:flower_pot";
				});

			foreach (var plant in _pottedPlantsList)
			{
				_mapper.Add($"minecraft:potted_{plant}", flowerPotMap);
			}

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

			#region Rails

			var railDirection = new PropertyStateMapper("shape", "rail_direction",
					new PropertyValueStateMapper("north_south", "0"),
					new PropertyValueStateMapper("east_west", "1"),
					new PropertyValueStateMapper("ascending_east", "2"),
					new PropertyValueStateMapper("ascending_west", "3"),
					new PropertyValueStateMapper("ascending_north", "4"),
					new PropertyValueStateMapper("ascending_south", "5"),
					new PropertyValueStateMapper("south_east", "6"),
					new PropertyValueStateMapper("south_west", "7"),
					new PropertyValueStateMapper("north_west", "8"),
					new PropertyValueStateMapper("north_east", "9"));

			_mapper.Add(new BlockStateMapper("minecraft:rail", railDirection));
			_mapper.Add(new BlockStateMapper("minecraft:activator_rail", 
				railDirection,
				new PropertyStateMapper("powered", "rail_data_bit")));

			#endregion

			#region Mushroom blocks

			var mashroomBlockMap = new BlockStateMapper(
				context =>
				{
					var nameOnly = context.AnvilName.Replace("minecraft:", "");
					var down = context.Properties["down"].StringValue == "true" ? 1 : 0;
					var up = context.Properties["up"].StringValue == "true" ? 1 : 0;
					var south = context.Properties["south"].StringValue == "true" ? 1 : 0;
					var west = context.Properties["west"].StringValue == "true" ? 1 : 0;
					var north = context.Properties["north"].StringValue == "true" ? 1 : 0;
					var east = context.Properties["east"].StringValue == "true" ? 1 : 0;

					var faceDirection = nameOnly switch
					{
						"mushroom_stem" => (down, up, south, west, north, east) switch
						{
							(0, 0, 1, 1, 1, 1) => 10,
							(1, 1, 1, 1, 1, 1) => 15,
							_ => 0
						},

						_ when nameOnly.Contains("mushroom_block") => (down, up, south, west, north, east) switch
						{
							(0, 1, 0, 1, 1, 0) => 1,
							(0, 1, 0, 0, 1, 0) => 2,
							(0, 1, 0, 0, 1, 1) => 3,
							(0, 1, 0, 1, 0, 0) => 4,
							(0, 1, 0, 0, 0, 0) => 5,
							(0, 1, 0, 0, 0, 1) => 6,
							(0, 1, 1, 1, 0, 0) => 7,
							(0, 1, 1, 0, 0, 0) => 8,
							(0, 1, 1, 0, 0, 1) => 9,
							(1, 1, 1, 1, 1, 1) => 14,
							_ => 0
						},

						_ => 0
					};

					context.Properties.Clear();
					context.Properties.Add(new NbtString("huge_mushroom_bits", faceDirection.ToString()));

					if (nameOnly == "mushroom_stem")
					{
						context.Properties.Add(new NbtByte(AnvilIncompatibleBitName, 1));
						return "miencraft:brown_mushroom_block";
					}

					return context.AnvilName;
				});

			_mapper.Add("minecraft:brown_mushroom_block", mashroomBlockMap);
			_mapper.Add("minecraft:red_mushroom_block", mashroomBlockMap);
			_mapper.Add("minecraft:mushroom_stem", mashroomBlockMap);

			#endregion

			#region Slabs

			var slabMapFunc = (string doubleSlabName, NbtCompound properties) =>
			{
				bool doubleSlab = false;

				var type = properties["type"].StringValue;
				if (type == "top")
				{
					properties.Add(new NbtString("top_slot_bit", "true"));
				}
				else if (type == "double")
				{
					doubleSlab = true;
				}

				var slabName = doubleSlabName.Replace("double_", "");
				var slabType = slabName.Replace("minecraft:", "").Replace("_slab", "");

				var woodType = slabType switch
				{
					"oak" => slabType,
					"spruce" => slabType,
					"birch" => slabType,
					"jungle" => slabType,
					"acacia" => slabType,
					"dark_oak" => slabType,
					_ => null
				};

				if (woodType != null)
				{
					properties.Add(new NbtString("wood_type", woodType));
					return doubleSlab ? "minecraft:double_wooden_slab" : "minecraft:wooden_slab";
				}

				var stoneSlabType = slabType switch
				{
					"smooth_stone" => slabType,
					"sandstone" => slabType,
					"wood" => slabType,
					"cobblestone" => slabType,
					"brick" => slabType,
					"stone_brick" => slabType,
					"quartz" => slabType,
					"nether_brick" => slabType,
					_ => null
				};

				if (stoneSlabType != null)
				{
					properties.Add(new NbtString("stone_slab_type", stoneSlabType));
					return doubleSlab ? "minecraft:double_stone_block_slab" : "minecraft:stone_block_slab";
				}

				var stoneSlabType2 = slabType switch
				{
					"red_sandstone" => slabType,
					"purpur" => slabType,
					"prismarine_rough" => slabType,
					"prismarine_dark" => slabType,
					"prismarine_brick" => slabType,
					"mossy_cobblestone" => slabType,
					"smooth_sandstone" => slabType,
					"red_nether_brick" => slabType,
					_ => null
				};

				if (stoneSlabType2 != null)
				{
					properties.Add(new NbtString("stone_slab_type_2", stoneSlabType2));
					return doubleSlab ? "minecraft:double_stone_block_slab2" : "minecraft:stone_block_slab2";
				}

				var stoneSlabType3 = slabType switch
				{
					"end_stone_brick" => slabType,
					"smooth_red_sandstone" => slabType,
					"polished_andesite" => slabType,
					"andesite" => slabType,
					"diorite" => slabType,
					"polished_diorite" => slabType,
					"granite" => slabType,
					"polished_granite" => slabType,
					_ => null
				};

				if (stoneSlabType3 != null)
				{
					properties.Add(new NbtString("stone_slab_type_3", stoneSlabType3));
					return doubleSlab ? "minecraft:double_stone_block_slab3" : "minecraft:stone_block_slab3";
				}

				var stoneSlabType4 = slabType switch
				{
					"mossy_stone_brick" => slabType,
					"smooth_quartz" => slabType,
					"stone" => slabType,
					"cut_sandstone" => slabType,
					"cut_red_sandstone" => slabType,
					_ => null
				};

				if (stoneSlabType4 != null)
				{
					properties.Add(new NbtString("stone_slab_type_4", stoneSlabType4));
					return doubleSlab ? "minecraft:double_stone_block_slab4" : "minecraft:stone_block_slab4";
				}

				return doubleSlab ? doubleSlabName : slabName;
			};


			var slabToDoubleMap = new Dictionary<string, string>();
			foreach (var id in BlockFactory.Ids)
			{
				if (id.Contains("_slab") && id.Contains("double_"))
				{
					slabToDoubleMap.Add(id.Replace("double_", ""), id);
				}
			}

			foreach (var material in _slabMaterialsList)
			{
				var slabName = $"minecraft:{material}_slab";
				_mapper.Add(slabName, new BlockStateMapper(
					context => slabMapFunc(slabToDoubleMap.GetValueOrDefault(slabName, slabName), context.Properties),
					new SkipPropertyStateMapper("type")));
			}

			#endregion

			#region Cauldron

			var cauldronLevelMap = new PropertyStateMapper("level", (_, _, property) =>
			{
				var level = int.Parse(property.Value) * 2;
				return new NbtString("fill_level", level.ToString());
			});

			_mapper.Add(new BlockStateMapper("minecraft:lava_cauldron",
				cauldronLevelMap,
				new AdditionalPropertyStateMapper("cauldron_liquid", "lava")));

			_mapper.Add(new BlockStateMapper("minecraft:powder_snow_cauldron", "minecraft:cauldron",
				cauldronLevelMap,
				new AdditionalPropertyStateMapper("cauldron_liquid", "powder_snow")));

			_mapper.Add(new BlockStateMapper("minecraft:water_cauldron", "minecraft:cauldron",
				cauldronLevelMap,
				new AdditionalPropertyStateMapper("cauldron_liquid", "water")));

			#endregion

			#region Quartz block

			var quartzBlockMap = new BlockStateMapper(context =>
				{
					var chiselType = context.AnvilName.Replace("minecraft:", "") switch
					{
						"chiseled_quartz_block" => "chiseled",
						"quartz_pillar" => "lines",
						"smooth_quartz" => "smooth",
						_ => "default"
					};

					context.Properties.Add(new NbtString("chisel_type", chiselType));

					if (!context.Properties.Contains("axis"))
					{
						context.Properties.Add(new NbtString("axis", "x"));
					}

					return "minecraft:quartz_block";
				});

			_mapper.Add("minecraft:quartz_block", quartzBlockMap);
			_mapper.Add("minecraft:chiseled_quartz_block", quartzBlockMap);
			_mapper.Add("minecraft:quartz_pillar", quartzBlockMap);
			_mapper.Add("minecraft:smooth_quartz", quartzBlockMap);

			#endregion

			#region Cakes

			var bitesMap = new PropertyStateMapper("bites", "bite_counter");

			_mapper.Add(new BlockStateMapper("minecraft:cake", bitesMap));
			_mapper.Add(new BlockStateMapper("minecraft:candle_cake", bitesMap));
			foreach (var color in _colorsList)
			{
				_mapper.Add(new BlockStateMapper($"minecraft:{color}_candle_cake", bitesMap));
			}

			#endregion

			#region Skull

			var skullMap = new BlockStateMapper(context =>
			{
				var rotation = byte.Parse(context.Properties["rotation"]?.StringValue ?? "0");
				if (!context.AnvilName.Contains("_wall"))
				{
					context.Properties["facing"] = new NbtString("facing", "up");
				} 

				var skullType = context.AnvilName.Replace("minecraft:", "").Replace("_head", "").Replace("_wall", "");

				var skullTypeBit = skullType switch
				{
					"skeleton" => 0,
					"wither_skeleton" => 1,
					"zombie" => 2,
					"player" => 3,
					"creeper" => 4,
					"dragon" => 5,
					_ => 0
				};

				context.BlockEntityTemplate = new SkullBlockEntity()
				{
					Rotation = rotation,
					SkullType = (byte) skullTypeBit
				};

				return "minecraft:skull";
			},
			facingDirectionMap,
			poweredSkipMap,
			new SkipPropertyStateMapper("rotation"));

			foreach (var skull in _skullsList)
			{
				_mapper.Add($"minecraft:{skull}", skullMap);
			}

			#endregion

			#region Anvil

			var anvilMap = new BlockStateMapper("minecraft:anvil",
				context =>
				{
					var name = context.AnvilName.Replace("minecraft:", "");

					var damage = name switch
					{
						"chipped_anvil" => "slightly_damaged",
						"damaged_anvil" => "very_damaged",
						_ => "undamaged"
					};

					context.Properties.Add(new NbtString("damage", damage));
				},
				new PropertyStateMapper("facing", "direction",
					new PropertyValueStateMapper("west", "0"),
					new PropertyValueStateMapper("north", "1"),
					new PropertyValueStateMapper("east", "2"),
					new PropertyValueStateMapper("south", "3")));

			_mapper.Add("minecraft:anvil", anvilMap);
			_mapper.Add("minecraft:chipped_anvil", anvilMap);
			_mapper.Add("minecraft:damaged_anvil", anvilMap);

			#endregion

			#region Pistons

			var pistonFacingDirectionMap = new PropertyStateMapper("facing", "facing_direction",
					new PropertyValueStateMapper("down", "0"),
					new PropertyValueStateMapper("up", "1"),
					new PropertyValueStateMapper("south", "2"),
					new PropertyValueStateMapper("north", "3"),
					new PropertyValueStateMapper("east", "4"),
					new PropertyValueStateMapper("west", "5"));

			var pistonMap = new BlockStateMapper(
				pistonFacingDirectionMap,
				new SkipPropertyStateMapper("extended"));

			_mapper.Add("minecraft:piston", pistonMap);
			_mapper.Add("minecraft:sticky_piston", pistonMap);

			_mapper.Add("minecraft:piston_head", new BlockStateMapper(
				context =>
				{
					return context.Properties["type"].StringValue == "sticky" ? "minecraft:sticky_piston_arm_collision" : "minecraft:piston_arm_collision";
				},
				pistonFacingDirectionMap,
				new SkipPropertyStateMapper("short"),
				new SkipPropertyStateMapper("type")));

			_mapper.Add(new BlockStateMapper("minecraft:moving_piston", "minecraft:moving_block",
				new SkipPropertyStateMapper("facing"),
				new SkipPropertyStateMapper("type")));

			#endregion


			// minecraft:cave_vines
			var caveVinesMap = new BlockStateMapper("minecraft:cave_vines",
				context =>
				{
					var barries = context.Properties["barries"]?.StringValue;

					if (barries != null)
					{
						context.Properties.Remove("barries");

						return barries == "true"
						? context.AnvilName == "minecraft:cave_vines" ? "minecraft:cave_vines_head_with_berries" : "minecraft:cave_vines_body_with_berries"
						: context.AnvilName;
					}

					return context.AnvilName;
				},
				new PropertyStateMapper("age", "growing_plant_age"),
				new SkipPropertyStateMapper("berries"));

			_mapper.Add("minecraft:cave_vines", caveVinesMap);
			_mapper.Add("minecraft:cave_vines_plant", caveVinesMap);


			// minecraft:vine
			_mapper.Add(new BlockStateMapper("minecraft:vine",
				context =>
				{
					var faceDirection = 0;

					faceDirection |= context.Properties["south"].StringValue == "true" ? 1 << 0 : 0;
					faceDirection |= context.Properties["west"].StringValue == "true" ? 1 << 1 : 0;
					faceDirection |= context.Properties["north"].StringValue == "true" ? 1 << 2 : 0;
					faceDirection |= context.Properties["east"].StringValue == "true" ? 1 << 3 : 0;

					context.Properties.Clear();
					context.Properties.Add(new NbtString("vine_direction_bits", faceDirection.ToString()));

					return context.AnvilName;
				}));


			// minecraft:kelp
			var kelpMap = new BlockStateMapper("minecraft:kelp",
				context =>
				{
					if (context.Properties["age"] != null)
						context.Properties["age"] = new NbtString("age", Math.Min(15, int.Parse(context.Properties["age"].StringValue)).ToString());
				},
				new PropertyStateMapper("age", "kelp_age"));

			_mapper.Add("minecraft:kelp", kelpMap);
			_mapper.Add("minecraft:kelp_plant", kelpMap);


			// minecraft:double_plant
			var doublePlantMap = new BlockStateMapper("minecraft:double_plant",
				context =>
				{
					var plantName = context.AnvilName.Replace("minecraft:", "");
					plantName = plantName switch
					{
						"tall_grass" => "grass",
						"large_fern" => "fern",
						"lilac" => "syringa",
						"rose_bush" => "rose",
						"peony" => "paeonia",
						_ => plantName
					};

					context.Properties.Add(new NbtString("double_plant_type", plantName));
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
				context =>
				{
					var grassType = "default";
					if (context.AnvilName == "minecraft:tall_seagrass")
						grassType = context.Properties["half"].StringValue == "upper" ? "double_top" : "double_bot";

					context.Properties.Add(new NbtString("sea_grass_type", grassType));
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
					context =>
					{
						context.Properties.Clear();
						context.Properties.Add(new NbtString("monster_egg_stone_type", stone));
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
					context =>
					{
						if (material != bedrockName)
							context.Properties.Add(new NbtString("wall_block_type", material));
					},
					new PropertyStateMapper("up", "wall_post_bit"),
					new PropertyStateMapper("east", "wall_connection_type_east", wallConnectionMap),
					new PropertyStateMapper("north", "wall_connection_type_north", wallConnectionMap),
					new PropertyStateMapper("south", "wall_connection_type_south", wallConnectionMap),
					new PropertyStateMapper("west", "wall_connection_type_west", wallConnectionMap)));
			}

			// minecraft:lever
			_mapper.Add(new BlockStateMapper("minecraft:lever",
				context =>
				{
					var face = context.Properties["face"].StringValue;
					var facing = context.Properties["facing"].StringValue;

					var direction = (face, facing) switch
					{
						("ceiling", "east" or "west") => "down_east_west",
						("wall", _) => facing,
						("floor", "north" or "south") => "up_north_south",
						("floor", "east" or "west") => "up_east_west",
						("ceiling", "north" or "south") => "down_north_south",
						_ => "down_east_west"
					};

					context.Properties.Add(new NbtString("lever_direction", direction));
				},
				new PropertyStateMapper("powered", "open_bit"),
				new SkipPropertyStateMapper("face"),
				new SkipPropertyStateMapper("facing")));

			//minecraft:redstone_wire
			var redstoneWireMap = faceDirectionSkipMap.Clone();
			redstoneWireMap.PropertiesMap.Add("power", new PropertyStateMapper("power", "redstone_signal"));
			_mapper.Add("minecraft:redstone_wire", redstoneWireMap);

			//minecraft:repeater
			_mapper.Add("minecraft:repeater", new BlockStateMapper(
				context =>
				{
					return context.Properties["powered"].StringValue == "true" ? "minecraft:powered_repeater" : "minecraft:unpowered_repeater";
				},
				new PropertyStateMapper("delay", 
					(name, properties, property) => new NbtString("repeater_delay", (int.Parse(property.Value) - 1).ToString())),
				directionMap2,
				new SkipPropertyStateMapper("locked"),
				poweredSkipMap));

			//minecraft:comparator
			_mapper.Add("minecraft:comparator", new BlockStateMapper(
				context =>
				{
					return context.Properties["powered"].StringValue == "true" ? "minecraft:powered_comparator" : "minecraft:unpowered_comparator";
				},
				new PropertyStateMapper("mode", "output_subtract_bit",
					new PropertyValueStateMapper("compare", "false"),
					new PropertyValueStateMapper("subtract", "true")),
				new PropertyStateMapper("powered", "output_lit_bit"),
				directionMap2,
				new SkipPropertyStateMapper("locked")));

			//minecraft:tripwire_hook
			_mapper.Add(new BlockStateMapper("minecraft:tripwire_hook",
				new BitPropertyStateMapper("attached"),
				new BitPropertyStateMapper("powered"),
				directionMap));
		}

		public static int GetRuntimeIdByPalette(NbtCompound palette, out BlockEntity blockEntity)
		{
			var name = palette["Name"].StringValue;
			var properties = (NbtCompound) (palette["Properties"] as NbtCompound)?.Clone() ?? new NbtCompound();

			blockEntity = null;
			var context = new BlockStateMapperContext(name, properties);

			try
			{
				name = _mapper.Resolve(context);

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

				blockEntity = context.BlockEntityTemplate;
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
				// workaround for incompatible mapping from anvil
				if (prop.Name == AnvilIncompatibleBitName)
				{
					blockStateContainer.States.Add(new BlockStateByte() { Name = prop.Name, Value = 1 });
					continue;
				}

				var state = blockStateContainer.States.FirstOrDefault(state => state.Name == prop.Name);

				if (state == null) return false;

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

			public bool TryAdd(BlockStateMapper map)
			{
				return TryAdd(map.AnvilName, map);
			}

			public bool TryAdd(string name, BlockStateMapper map)
			{
				return _map.TryAdd(name, map);
			}

			public void AddDefault(BlockStateMapper map)
			{
				_defaultMap.Add(map);
			}

			public string Resolve(BlockStateMapperContext context)
			{
				if (_map.TryGetValue(context.AnvilName, out var map))
				{
					context.AnvilName = map.Resolve(context);
				}

				foreach (var defMap in _defaultMap)
				{
					defMap.ResolveDefault(context);
				}

				return context.AnvilName;
			}
		}

		public class BlockStateMapperContext
		{
			public string AnvilName { get; set; }
			public string BedrockName { get; set; }
			public NbtCompound Properties { get; set; }
			public BlockEntity BlockEntityTemplate { get; set; }

			public BlockStateMapperContext(string anvilName, NbtCompound properties)
			{
				AnvilName = anvilName;
				Properties = properties;
			}
		}

		public class BlockStateMapper
		{
			public string AnvilName { get; set; }
			public string BedrockName { get; set; }

			public Dictionary<string, PropertyStateMapper> PropertiesMap { get; } = new Dictionary<string, PropertyStateMapper>();
			public List<AdditionalPropertyStateMapper> AdditionalProperties { get; } = new List<AdditionalPropertyStateMapper>();
			public Dictionary<string, SkipPropertyStateMapper> SkipProperties { get; } = new Dictionary<string, SkipPropertyStateMapper>();

			private readonly Func<BlockStateMapperContext, string> _func;

			public BlockStateMapper(Func<BlockStateMapperContext, string> func)
			{
				_func = func;
			}

			public BlockStateMapper(Action<BlockStateMapperContext> func, params IPropertyStateMapper[] propertiesMap)
				: this(null, null, func, propertiesMap)
			{

			}

			public BlockStateMapper(Func<BlockStateMapperContext, string> func, params IPropertyStateMapper[] propertiesMap)
				: this(null, null, func, propertiesMap)
			{

			}

			public BlockStateMapper(string anvilName, Action<BlockStateMapperContext> func)
				: this(anvilName, anvilName, func)
			{

			}

			public BlockStateMapper(string anvilName, Func<BlockStateMapperContext, string> func)
				: this(anvilName, anvilName, func)
			{

			}

			public BlockStateMapper(string anvilName, Action<BlockStateMapperContext> func, params IPropertyStateMapper[] propertiesMap)
				: this(anvilName, anvilName, func, propertiesMap)
			{

			}

			public BlockStateMapper(string anvilName, Func<BlockStateMapperContext, string> func, params IPropertyStateMapper[] propertiesMap)
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

			public BlockStateMapper(string anvilName, string bedrockName, Action<BlockStateMapperContext> func, params IPropertyStateMapper[] propertiesMap)
				: this(anvilName, bedrockName, context =>
				{
					func(context);
					return context.AnvilName;
				}, propertiesMap)
			{
			}

			public BlockStateMapper(string anvilName, string bedrockName, Func<BlockStateMapperContext, string> func, params IPropertyStateMapper[] propertiesMap)
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

			public string Resolve(BlockStateMapperContext context)
			{
				if (_func != null) context.AnvilName = _func(context);

				foreach (NbtString prop in context.Properties.ToArray())
				{
					if (SkipProperties.TryGetValue(prop.Name, out var skipMap) && skipMap.Resolve(context.AnvilName, context.Properties, prop))
					{
						context.Properties.Remove(prop.Name);
					}
					else if (PropertiesMap.TryGetValue(prop.Name, out var propMap))
					{
						context.Properties.Remove(prop.Name);
						context.Properties.Add(propMap.Resolve(context.AnvilName, context.Properties, prop));
					}
				}

				foreach (var prop in AdditionalProperties)
				{
					context.Properties[prop.Name] = prop.Resolve(context.AnvilName, context.Properties);
				}

				return BedrockName ?? context.AnvilName;
			}

			public string ResolveDefault(BlockStateMapperContext context)
			{
				if (_func != null) return _func(context);

				foreach (NbtString prop in context.Properties.ToArray())
				{
					var skipMap = SkipProperties.Values.FirstOrDefault(map => map.Name == prop.Name || map.Name == null);
					if (skipMap != null && skipMap.Resolve(context.AnvilName, context.Properties, prop))
					{
						context.Properties.Remove(prop.Name);
					}
					else
					{
						var propMap = PropertiesMap.Values.FirstOrDefault(map => map.AnvilName == prop.Name || map.AnvilName == null);

						if (propMap != null)
						{
							context.Properties.Remove(prop.Name);
							context.Properties.Add(propMap.Resolve(context.AnvilName, context.Properties, prop));
						}
					}
				}

				foreach (var prop in AdditionalProperties)
				{
					context.Properties[prop.Name] = prop.Resolve(context.AnvilName, context.Properties);
				}

				return BedrockName ?? context.AnvilName;
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

				return new BlockStateMapper(AnvilName, BedrockName, (Func<BlockStateMapperContext, string>) _func?.Clone(), propertiesMap.ToArray());
			}
		}

		public interface IPropertyStateMapper { }

		public class BitPropertyStateMapper : PropertyStateMapper
		{
			public BitPropertyStateMapper(string anvilName)
				: base(anvilName, $"{anvilName}_bit",
					  new PropertyValueStateMapper("false", "0"),
					  new PropertyValueStateMapper("true", "1"))
			{

			}
		}

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
