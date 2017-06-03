using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management.Instrumentation;
using System.Numerics;
using System.Text.RegularExpressions;
using fNbt;
using log4net;
using log4net.Appender;
using MiNET.BlockEntities;
using MiNET.Blocks;
using MiNET.Items;
using MiNET.Net;
using MiNET.Utils;

namespace MiNET.Worlds
{
	public class Mapper : Tuple<int, Func<int, byte, byte>>
	{
		public Mapper(int blockId, Func<int, byte, byte> dataMapper)
			: base(blockId, dataMapper)
		{
		}
	}

	public class NoDataMapper : Mapper
	{
		public NoDataMapper(int blockId) : base(blockId, (bi, i1) => i1)
		{
		}
	}

	public class AnvilWorldProvider : IWorldProvider, ICachingWorldProvider, ICloneable
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (AnvilWorldProvider));

		public static readonly Dictionary<int, Tuple<int, Func<int, byte, byte>>> Convert;

	    public static readonly Dictionary<string, int> StringIdConvert;

		public IWorldProvider MissingChunkProvider { get; set; }

		public LevelInfo LevelInfo { get; private set; }

		public ConcurrentDictionary<ChunkCoordinates, ChunkColumn> _chunkCache = new ConcurrentDictionary<ChunkCoordinates, ChunkColumn>();

		public string BasePath { get; private set; }

		public bool IsCaching { get; private set; }

		public byte WaterOffsetY { get; set; }

		static AnvilWorldProvider()
		{
			var air = new Mapper(0, (i, b) => 0);

			Convert = new Dictionary<int, Tuple<int, Func<int, byte, byte>>>
			{
				//{23, air}, // minecraft:dispenser	=> Air
				//{29, air}, // minecraft:sticky_piston	=> Air
				//{33, air}, // minecraft:piston		=> Air
				//{34, air}, // minecraft:piston_head		=> Air
				{36, new NoDataMapper(250)}, // minecraft:piston_extension		=> MovingBlock
				{43, new Mapper(43, (i, b) => (byte) (b == 6 ? 7 : b == 6 ? 7 : b))}, // Fence		=> Fence
				{44, new Mapper(44, (i, b) => (byte) (b == 6 ? 7 : b == 6 ? 7 : b == 14 ? 15 : b == 15 ? 14 : b))}, // Fence		=> Fence
				{84, air}, // minecraft:jukebox		=> Air
				{85, new Mapper(85, (i, b) => 0)}, // Fence		=> Fence
				//{90, air}, // Nether Portal	=> Air
				//{93, air}, // minecraft:unpowered_repeater	=> Air
				//{94, air}, // minecraft:powered_repeater	=> Air
				{95, new NoDataMapper(241)}, // minecraft:stained_glass	=> Stained Glass
				{96, new Mapper(96, (i, b) => (byte) (((b & 0x04) << 1) | ((b & 0x08) >> 1) | (3 - (b & 0x03))))}, // Trapdoor Fix
				{167, new Mapper(167, (i, b) => (byte) (((b & 0x04) << 1) | ((b & 0x08) >> 1) | (3 - (b & 0x03)))) }, //Fix iron_trapdoor
				//{113, new NoDataMapper(85)}, // Nether Fence		=> Fence
				//{118, air}, // minecraft:cauldron		=> Air
				//{119, air}, // minecraft:end_portal		=> Air
				//{122, air}, // Dragon Egg		=> Air
				//{123, new NoDataMapper(122)}, // Redstone Lamp O	=> Glowstone
				//{124, new NoDataMapper(123)}, // Redstone Lamp O	=> Glowstone
				{125, new NoDataMapper(157)}, // minecraft:double_wooden_slab	=> minecraft:double_wooden_slab
				{126, new NoDataMapper(158)}, // minecraft:wooden_slab		=> minecraft:wooden_slab
				//{130, new NoDataMapper(54)}, // Ender Chest		=> Chest
				{137, air}, // Command Block	=> Air
				//{138, air}, // Beacon		=> Air
				{
					143, new Mapper(143, delegate(int i, byte b)
					{
						switch (b & 0x7f)
						{
							case 0:
								return (byte) BlockFace.Down; // 0
							case 1:
								return (byte) BlockFace.South; // 5
							case 2:
								return (byte) BlockFace.North; // 4
							case 3:
								return (byte) BlockFace.West; // 3
							case 4:
								return (byte) BlockFace.East; // 2
							case 5:
								return (byte) BlockFace.Up; // 1
						}

						return 0;
					})
				}, // Trapdoor Fix
				{
					77, new Mapper(77, delegate(int i, byte b)
					{
						switch (b & 0x7f)
						{
							case 0:
								return (byte) BlockFace.Down;
							case 1:
								return (byte) BlockFace.South;
							case 2:
								return (byte) BlockFace.North;
							case 3:
								return (byte) BlockFace.West;
							case 4:
								return (byte) BlockFace.East;
							case 5:
								return (byte) BlockFace.Up;
						}

						return 0;
					})
				}, // Trapdoor Fix

				//{149, air}, // minecraft:unpowered_comparator		=> Air
				//{150, air}, // minecraft:powered_comparator		=> Air

				//{154, air}, // minecraft:hopper		=> Air
				{157, new NoDataMapper(126)}, // minecraft:activator_rail	=> minecraft:activator_rail
				{158, new NoDataMapper(125)}, // minecraft:dropper		=> Air
				//{160, new NoDataMapper(160)}, // minecraft:stained_glass_pane	=> Glass Pane
				//{165, air}, // Slime Block		=> Air
				{166, new NoDataMapper(95)}, // minecraft:barrier		=> (Invisible Bedrock)
				//{168, air}, // minecraft:prismarine		=> Air
				//{169, new NoDataMapper(89)}, // minecraft:sea_lantern		=> Glowstone
				{176, air}, // minecraft:standing_banner		=> Air
				{177, air}, // minecraft:wall_banner		=> Air
				// 179-182 Need mapping (Red Sandstone)
				{183, new NoDataMapper(183)}, // Spruce Gate		=> Gate
				{184, new NoDataMapper(184)}, // Birch Gate		=> Gate
				{185, new NoDataMapper(185)}, // Jungle Gate		=> Gate
				{186, new NoDataMapper(186)}, // Dark Oak Gate	=> Gate
				{187, new NoDataMapper(187)}, // Acacia Gate		=> Gate
				{188, new Mapper(85, (i, b) => 1)}, // Spruce Fence		=> Fence
				{189, new Mapper(85, (i, b) => 2)}, // Birch Fence		=> Fence
				{190, new Mapper(85, (i, b) => 3)}, // Jungle Fence		=> Fence
				{191, new Mapper(85, (i, b) => 5)}, // Dark Oak Fence	=> Fence
				{192, new Mapper(85, (i, b) => 4)}, // Acacia Fence		=> Fence
				{198, new NoDataMapper(208)}, // minecraft:end_rod	=> EndRod
				{199, new NoDataMapper(140)}, // minecraft:chorus_plant => ChorusPlant
				{202, new Mapper(201, (i, b) => 2) }, // minecraft:purpur_pillar => PurpurBlock:2 (idk why)
				{205, new Mapper(182, (i, b) => 1) }, // minecraft:purpur_slab => RedSandstoneSlab:1 (idk why)
				{207, new NoDataMapper(244)}, // minecraft:beetroot_block => beetroot
				{208, new NoDataMapper(198)}, // minecraft:grass_path => grass_path
				{209, new NoDataMapper(209)}, // minecraft:end_gateway => EndGateway
				{212, new NoDataMapper(174)}, // Frosted Ice => Packed Ice
				{218, new NoDataMapper(251)} // minecraft:observer => Observer
			};

		    StringIdConvert = new Dictionary<string, int>()
		    {
                {"minecraft:air",0},
                {"minecraft:stone",1},
                {"minecraft:grass",2},
                {"minecraft:dirt",3},
                {"minecraft:cobblestone",4},
                {"minecraft:planks",5},
                {"minecraft:sapling",6},
                {"minecraft:bedrock",7},
                {"minecraft:flowing_water",8},
                {"minecraft:water",9},
                {"minecraft:flowing_lava",10},
                {"minecraft:lava",11},
                {"minecraft:sand",12},
                {"minecraft:gravel",13},
                {"minecraft:gold_ore",14},
                {"minecraft:iron_ore",15},
                {"minecraft:coal_ore",16},
                {"minecraft:log",17},
                {"minecraft:leaves",18},
                {"minecraft:sponge",19},
                {"minecraft:glass",20},
                {"minecraft:lapis_ore",21},
                {"minecraft:lapis_block",22},
                {"minecraft:dispenser",23},
                {"minecraft:sandstone",24},
                {"minecraft:noteblock",25},
                {"minecraft:bed",26},
                {"minecraft:golden_rail",27},
                {"minecraft:detector_rail",28},
                {"minecraft:sticky_piston",29},
                {"minecraft:web",30},
                {"minecraft:tallgrass",31},
                {"minecraft:deadbush",32},
                {"minecraft:piston",33},
                {"minecraft:piston_head",34},
                {"minecraft:wool",35},
                {"minecraft:yellow_flower",37},
                {"minecraft:red_flower",38},
                {"minecraft:brown_mushroom",39},
                {"minecraft:red_mushroom",40},
                {"minecraft:gold_block",41},
                {"minecraft:iron_block",42},
                {"minecraft:double_stone_slab",43},
                {"minecraft:stone_slab",44},
                {"minecraft:brick_block",45},
                {"minecraft:tnt",46},
                {"minecraft:bookshelf",47},
                {"minecraft:mossy_cobblestone",48},
                {"minecraft:obsidian",49},
                {"minecraft:torch",50},
                {"minecraft:fire",51},
                {"minecraft:mob_spawner",52},
                {"minecraft:oak_stairs",53},
                {"minecraft:chest",54},
                {"minecraft:redstone_wire",55},
                {"minecraft:diamond_ore",56},
                {"minecraft:diamond_block",57},
                {"minecraft:crafting_table",58},
                {"minecraft:wheat",59},
                {"minecraft:farmland",60},
                {"minecraft:furnace",61},
                {"minecraft:lit_furnace",62},
                {"minecraft:standing_sign",63},
                {"minecraft:wooden_door",64},
                {"minecraft:ladder",65},
                {"minecraft:rail",66},
                {"minecraft:stone_stairs",67},
                {"minecraft:wall_sign",68},
                {"minecraft:lever",69},
                {"minecraft:stone_pressure_plate",70},
                {"minecraft:iron_door",71},
                {"minecraft:wooden_pressure_plate",72},
                {"minecraft:redstone_ore",73},
                {"minecraft:lit_redstone_ore",74},
                {"minecraft:unlit_redstone_torch",75},
                {"minecraft:redstone_torch",76},
                {"minecraft:stone_button",77},
                {"minecraft:snow_layer",78},
                {"minecraft:ice",79},
                {"minecraft:snow",80},
                {"minecraft:cactus",81},
                {"minecraft:clay",82},
                {"minecraft:reeds",83},
                {"minecraft:jukebox",84},
                {"minecraft:fence",85},
                {"minecraft:pumpkin",86},
                {"minecraft:netherrack",87},
                {"minecraft:soul_sand",88},
                {"minecraft:glowstone",89},
                {"minecraft:portal",90},
                {"minecraft:lit_pumpkin",91},
                {"minecraft:cake",92},
                {"minecraft:unpowered_repeater",93},
                {"minecraft:powered_repeater",94},
                {"minecraft:stained_glass",95},
                {"minecraft:trapdoor",96},
                {"minecraft:monster_egg",97},
                {"minecraft:stonebrick",98},
                {"minecraft:brown_mushroom_block",99},
                {"minecraft:red_mushroom_block",100},
                {"minecraft:iron_bars",101},
                {"minecraft:glass_pane",102},
                {"minecraft:melon_block",103},
                {"minecraft:pumpkin_stem",104},
                {"minecraft:melon_stem",105},
                {"minecraft:vine",106},
                {"minecraft:fence_gate",107},
                {"minecraft:brick_stairs",108},
                {"minecraft:stone_brick_stairs",109},
                {"minecraft:mycelium",110},
                {"minecraft:waterlily",111},
                {"minecraft:nether_brick",112},
                {"minecraft:nether_brick_fence",113},
                {"minecraft:nether_brick_stairs",114},
                {"minecraft:nether_wart",115},
                {"minecraft:enchanting_table",116},
                {"minecraft:brewing_stand",117},
                {"minecraft:cauldron",118},
                {"minecraft:end_portal",119},
                {"minecraft:end_portal_frame",120},
                {"minecraft:end_stone",121},
                {"minecraft:dragon_egg",122},
                {"minecraft:redstone_lamp",123},
                {"minecraft:lit_redstone_lamp",124},
                {"minecraft:double_wooden_slab",125},
                {"minecraft:wooden_slab",126},
                {"minecraft:cocoa",127},
                {"minecraft:sandstone_stairs",128},
                {"minecraft:emerald_ore",129},
                {"minecraft:ender_chest",130},
                {"minecraft:tripwire_hook",131},
                {"minecraft:emerald_block",133},
                {"minecraft:spruce_stairs",134},
                {"minecraft:birch_stairs",135},
                {"minecraft:jungle_stairs",136},
                {"minecraft:command_block",137},
                {"minecraft:beacon",138},
                {"minecraft:cobblestone_wall",139},
                {"minecraft:flower_pot",140},
                {"minecraft:carrots",141},
                {"minecraft:potatoes",142},
                {"minecraft:wooden_button",143},
                {"minecraft:skull",144},
                {"minecraft:anvil",145},
                {"minecraft:trapped_chest",146},
                {"minecraft:light_weighted_pressure_plate",147},
                {"minecraft:heavy_weighted_pressure_plate",148},
                {"minecraft:unpowered_comparator",149},
                {"minecraft:powered_comparator",150},
                {"minecraft:daylight_detector",151},
                {"minecraft:redstone_block",152},
                {"minecraft:quartz_ore",153},
                {"minecraft:hopper",154},
                {"minecraft:quartz_block",155},
                {"minecraft:quartz_stairs",156},
                {"minecraft:activator_rail",157},
                {"minecraft:dropper",158},
                {"minecraft:stained_hardened_clay",159},
                {"minecraft:stained_glass_pane",160},
                {"minecraft:leaves2",161},
                {"minecraft:log2",162},
                {"minecraft:acacia_stairs",163},
                {"minecraft:dark_oak_stairs",164},
                {"minecraft:slime",165},
                {"minecraft:barrier",166},
                {"minecraft:iron_trapdoor",167},
                {"minecraft:prismarine",168},
                {"minecraft:sea_lantern",169},
                {"minecraft:hay_block",170},
                {"minecraft:carpet",171},
                {"minecraft:hardened_clay",172},
                {"minecraft:coal_block",173},
                {"minecraft:packed_ice",174},
                {"minecraft:double_plant",175},
                {"minecraft:standing_banner",176},
                {"minecraft:wall_banner",177},
                {"minecraft:daylight_detector_inverted",178},
                {"minecraft:red_sandstone",179},
                {"minecraft:red_sandstone_stairs",180},
                {"minecraft:double_stone_slab2",181},
                {"minecraft:stone_slab2",182},
                {"minecraft:spruce_fence_gate",183},
                {"minecraft:birch_fence_gate",184},
                {"minecraft:jungle_fence_gate",185},
                {"minecraft:dark_oak_fence_gate",186},
                {"minecraft:acacia_fence_gate",187},
                {"minecraft:spruce_fence",188},
                {"minecraft:birch_fence",189},
                {"minecraft:jungle_fence",190},
                {"minecraft:dark_oak_fence",191},
                {"minecraft:acacia_fence",192},
                {"minecraft:spruce_door",193},
                {"minecraft:birch_door",194},
                {"minecraft:jungle_door",195},
                {"minecraft:acacia_door",196},
                {"minecraft:dark_oak_door",197},
                {"minecraft:end_rod",198},
                {"minecraft:chorus_plant",199},
                {"minecraft:chorus_flower",200},
                {"minecraft:purpur_block",201},
                {"minecraft:purpur_pillar",202},
                {"minecraft:purpur_stairs",203},
                {"minecraft:purpur_double_slab",204},
                {"minecraft:purpur_slab",205},
                {"minecraft:end_bricks",206},
                {"minecraft:beetroots",207},
                {"minecraft:grass_path",208},
                {"minecraft:end_gateway",209},
                {"minecraft:repeating_command_block",210},
                {"minecraft:chain_command_block",211},
                {"minecraft:frosted_ice",212},
                {"minecraft:magma",213},
                {"minecraft:nether_wart_block",214},
                {"minecraft:red_nether_brick",215},
                {"minecraft:bone_block",216},
                {"minecraft:structure_void",217},
                {"minecraft:structure_block",255},
                {"minecraft:iron_shovel",256},
                {"minecraft:iron_pickaxe",257},
                {"minecraft:iron_axe",258},
                {"minecraft:flint_and_steel",259},
                {"minecraft:apple",260},
                {"minecraft:bow",261},
                {"minecraft:arrow",262},
                {"minecraft:coal",263},
                {"minecraft:diamond",264},
                {"minecraft:iron_ingot",265},
                {"minecraft:gold_ingot",266},
                {"minecraft:iron_sword",267},
                {"minecraft:wooden_sword",268},
                {"minecraft:wooden_shovel",269},
                {"minecraft:wooden_pickaxe",270},
                {"minecraft:wooden_axe",271},
                {"minecraft:stone_sword",272},
                {"minecraft:stone_shovel",273},
                {"minecraft:stone_pickaxe",274},
                {"minecraft:stone_axe",275},
                {"minecraft:diamond_sword",276},
                {"minecraft:diamond_shovel",277},
                {"minecraft:diamond_pickaxe",278},
                {"minecraft:diamond_axe",279},
                {"minecraft:stick",280},
                {"minecraft:bowl",281},
                {"minecraft:mushroom_stew",282},
                {"minecraft:golden_sword",283},
                {"minecraft:golden_shovel",284},
                {"minecraft:golden_pickaxe",285},
                {"minecraft:golden_axe",286},
                {"minecraft:string",287},
                {"minecraft:feather",288},
                {"minecraft:gunpowder",289},
                {"minecraft:wooden_hoe",290},
                {"minecraft:stone_hoe",291},
                {"minecraft:iron_hoe",292},
                {"minecraft:diamond_hoe",293},
                {"minecraft:golden_hoe",294},
                {"minecraft:wheat_seeds",295},
                {"minecraft:bread",297},
                {"minecraft:leather_helmet",298},
                {"minecraft:leather_chestplate",299},
                {"minecraft:leather_leggings",300},
                {"minecraft:leather_boots",301},
                {"minecraft:chainmail_helmet",302},
                {"minecraft:chainmail_chestplate",303},
                {"minecraft:chainmail_leggings",304},
                {"minecraft:chainmail_boots",305},
                {"minecraft:iron_helmet",306},
                {"minecraft:iron_chestplate",307},
                {"minecraft:iron_leggings",308},
                {"minecraft:iron_boots",309},
                {"minecraft:diamond_helmet",310},
                {"minecraft:diamond_chestplate",311},
                {"minecraft:diamond_leggings",312},
                {"minecraft:diamond_boots",313},
                {"minecraft:golden_helmet",314},
                {"minecraft:golden_chestplate",315},
                {"minecraft:golden_leggings",316},
                {"minecraft:golden_boots",317},
                {"minecraft:flint",318},
                {"minecraft:porkchop",319},
                {"minecraft:cooked_porkchop",320},
                {"minecraft:painting",321},
                {"minecraft:golden_apple",322},
                {"minecraft:sign",323},
                {"minecraft:bucket",325},
                {"minecraft:water_bucket",326},
                {"minecraft:lava_bucket",327},
                {"minecraft:minecart",328},
                {"minecraft:saddle",329},
                {"minecraft:redstone",331},
                {"minecraft:snowball",332},
                {"minecraft:boat",333},
                {"minecraft:leather",334},
                {"minecraft:milk_bucket",335},
                {"minecraft:brick",336},
                {"minecraft:clay_ball",337},
                {"minecraft:paper",339},
                {"minecraft:book",340},
                {"minecraft:slime_ball",341},
                {"minecraft:chest_minecart",342},
                {"minecraft:furnace_minecart",343},
                {"minecraft:egg",344},
                {"minecraft:compass",345},
                {"minecraft:fishing_rod",346},
                {"minecraft:clock",347},
                {"minecraft:glowstone_dust",348},
                {"minecraft:fish",349},
                {"minecraft:cooked_fish",350},
                {"minecraft:dye",351},
                {"minecraft:bone",352},
                {"minecraft:sugar",353},
                {"minecraft:repeater",356},
                {"minecraft:cookie",357},
                {"minecraft:filled_map",358},
                {"minecraft:shears",359},
                {"minecraft:melon",360},
                {"minecraft:pumpkin_seeds",361},
                {"minecraft:melon_seeds",362},
                {"minecraft:beef",363},
                {"minecraft:cooked_beef",364},
                {"minecraft:chicken",365},
                {"minecraft:cooked_chicken",366},
                {"minecraft:rotten_flesh",367},
                {"minecraft:ender_pearl",368},
                {"minecraft:blaze_rod",369},
                {"minecraft:ghast_tear",370},
                {"minecraft:gold_nugget",371},
                {"minecraft:potion",373},
                {"minecraft:glass_bottle",374},
                {"minecraft:spider_eye",375},
                {"minecraft:fermented_spider_eye",376},
                {"minecraft:blaze_powder",377},
                {"minecraft:magma_cream",378},
                {"minecraft:ender_eye",381},
                {"minecraft:speckled_melon",382},
                {"minecraft:spawn_egg",383},
                {"minecraft:experience_bottle",384},
                {"minecraft:fire_charge",385},
                {"minecraft:writable_book",386},
                {"minecraft:written_book",387},
                {"minecraft:emerald",388},
                {"minecraft:item_frame",389},
                {"minecraft:carrot",391},
                {"minecraft:potato",392},
                {"minecraft:baked_potato",393},
                {"minecraft:poisonous_potato",394},
                {"minecraft:map",395},
                {"minecraft:golden_carrot",396},
                {"minecraft:carrot_on_a_stick",398},
                {"minecraft:nether_star",399},
                {"minecraft:pumpkin_pie",400},
                {"minecraft:fireworks",401},
                {"minecraft:firework_charge",402},
                {"minecraft:enchanted_book",403},
                {"minecraft:comparator",404},
                {"minecraft:netherbrick",405},
                {"minecraft:quartz",406},
                {"minecraft:tnt_minecart",407},
                {"minecraft:hopper_minecart",408},
                {"minecraft:prismarine_shard",409},
                {"minecraft:prismarine_crystals",410},
                {"minecraft:rabbit",411},
                {"minecraft:cooked_rabbit",412},
                {"minecraft:rabbit_stew",413},
                {"minecraft:rabbit_foot",414},
                {"minecraft:rabbit_hide",415},
                {"minecraft:armor_stand",416},
                {"minecraft:iron_horse_armor",417},
                {"minecraft:golden_horse_armor",418},
                {"minecraft:diamond_horse_armor",419},
                {"minecraft:lead",420},
                {"minecraft:name_tag",421},
                {"minecraft:command_block_minecart",422},
                {"minecraft:mutton",423},
                {"minecraft:cooked_mutton",424},
                {"minecraft:banner",425},
                {"minecraft:chorus_fruit",432},
                {"minecraft:popped_chorus_fruit",433},
                {"minecraft:beetroot",434},
                {"minecraft:beetroot_seeds",435},
                {"minecraft:beetroot_soup",436},
                {"minecraft:dragon_breath",437},
                {"minecraft:splash_potion",438},
                {"minecraft:spectral_arrow",439},
                {"minecraft:tipped_arrow",440},
                {"minecraft:lingering_potion",441},
                {"minecraft:shield",442},
                {"minecraft:elytra",443},
                {"minecraft:spruce_boat",444},
                {"minecraft:birch_boat",445},
                {"minecraft:jungle_boat",446},
                {"minecraft:acacia_boat",447},
                {"minecraft:dark_oak_boat",448},
                {"minecraft:record_13",2256},
                {"minecraft:record_cat",2257},
                {"minecraft:record_blocks",2258},
                {"minecraft:record_chirp",2259},
                {"minecraft:record_far",2260},
                {"minecraft:record_mall",2261},
                {"minecraft:record_mellohi",2262},
                {"minecraft:record_stal",2263},
                {"minecraft:record_strad",2264},
                {"minecraft:record_ward",2265},
                {"minecraft:record_11",2266},
                {"minecraft:record_wait",2267}

            };

		}

		public AnvilWorldProvider()
		{
			IsCaching = true;
			//_flatland = new FlatlandWorldProvider();
		}

		public AnvilWorldProvider(string basePath) : this()
		{
			BasePath = basePath;
		}

		protected AnvilWorldProvider(string basePath, LevelInfo levelInfo, byte waterOffsetY, ConcurrentDictionary<ChunkCoordinates, ChunkColumn> chunkCache)
		{
			IsCaching = true;
			BasePath = basePath;
			LevelInfo = levelInfo;
			WaterOffsetY = waterOffsetY;
			_chunkCache = chunkCache;
			_isInitialized = true;
			//_flatland = new FlatlandWorldProvider();
		}

		private bool _isInitialized = false;
		private object _initializeSync = new object();

		public void Initialize()
		{
			if (_isInitialized) return; // Quick exit

			lock (_initializeSync)
			{
				if (_isInitialized) return;

				BasePath = BasePath ?? Config.GetProperty("PCWorldFolder", "World").Trim();

				NbtFile file = new NbtFile();
				file.LoadFromFile(Path.Combine(BasePath, "level.dat"));
				NbtTag dataTag = file.RootTag["Data"];
				LevelInfo = new LevelInfo(dataTag);

				WaterOffsetY = WaterOffsetY == 0 ? (byte) Config.GetProperty("PCWaterOffset", 0) : WaterOffsetY;

				_isInitialized = true;
			}
		}

		private int Noop(int blockId, int data)
		{
			return 0;
		}

		public ChunkColumn[] GetCachedChunks()
		{
			return _chunkCache.Values.Where(column => column != null).ToArray();
		}

		public void ClearCachedChunks()
		{
			_chunkCache.Clear();
		}

		public ChunkColumn GenerateChunkColumn(ChunkCoordinates chunkCoordinates)
		{
			// Warning: The following code MAY execute the GetChunk 2 times for the same coordinate
			// if called in rapid succession. However, for the scenario of the provider, this is highly unlikely.
			return _chunkCache.GetOrAdd(chunkCoordinates, coordinates => GetChunk(coordinates, BasePath, MissingChunkProvider, WaterOffsetY));
		}

		public Queue<Block> LightSources { get; set; } = new Queue<Block>();

		public ChunkColumn GetChunk(ChunkCoordinates coordinates, string basePath, IWorldProvider generator, int yoffset)
		{
			int width = 32;
			int depth = 32;

			int rx = coordinates.X >> 5;
			int rz = coordinates.Z >> 5;

			Log.Debug($"Generating chunk @{coordinates}");

			string filePath = Path.Combine(basePath, string.Format(@"region{2}r.{0}.{1}.mca", rx, rz, Path.DirectorySeparatorChar));

			if (!File.Exists(filePath))
			{
				var chunkColumn = generator?.GenerateChunkColumn(coordinates);
				if (chunkColumn != null)
				{
					chunkColumn.NeedSave = true;
				}

				return chunkColumn;
				//return new ChunkColumn
				//{
				//	x = coordinates.X,
				//	z = coordinates.Z,
				//};
			}

			using (var regionFile = File.OpenRead(filePath))
			{
				byte[] buffer = new byte[8192];

				regionFile.Read(buffer, 0, 8192);

				int xi = (coordinates.X%width);
				if (xi < 0) xi += 32;
				int zi = (coordinates.Z%depth);
				if (zi < 0) zi += 32;
				int tableOffset = (xi + zi*width)*4;

				regionFile.Seek(tableOffset, SeekOrigin.Begin);

				byte[] offsetBuffer = new byte[4];
				regionFile.Read(offsetBuffer, 0, 3);
				Array.Reverse(offsetBuffer);
				int offset = BitConverter.ToInt32(offsetBuffer, 0) << 4;

				byte[] bytes = BitConverter.GetBytes(offset >> 4);
				Array.Reverse(bytes);
				if (offset != 0 && offsetBuffer[0] != bytes[0] && offsetBuffer[1] != bytes[1] && offsetBuffer[2] != bytes[2])
				{
					throw new Exception($"Not the same buffer\n{Package.HexDump(offsetBuffer)}\n{Package.HexDump(bytes)}");
				}

				int length = regionFile.ReadByte();

				if (offset == 0 || length == 0)
				{
					var chunkColumn = generator?.GenerateChunkColumn(coordinates);
					if (chunkColumn != null)
					{
						chunkColumn.NeedSave = true;
					}

					return chunkColumn;
					//return new ChunkColumn
					//{
					//	x = coordinates.X,
					//	z = coordinates.Z,
					//};
				}

				regionFile.Seek(offset, SeekOrigin.Begin);
				byte[] waste = new byte[4];
				regionFile.Read(waste, 0, 4);
				int compressionMode = regionFile.ReadByte();

				if (compressionMode != 0x02) throw new Exception($"CX={coordinates.X}, CZ={coordinates.Z}, NBT wrong compression. Expected 0x02, got 0x{compressionMode :X2}. " +
				                                                 $"Offset={offset}, length={length}\n{Package.HexDump(waste)}");

				var nbt = new NbtFile();
				nbt.LoadFromStream(regionFile, NbtCompression.ZLib);

				NbtTag dataTag = nbt.RootTag["Level"];

				NbtList sections = dataTag["Sections"] as NbtList;

				ChunkColumn chunk = new ChunkColumn
				{
					x = coordinates.X,
					z = coordinates.Z,
					biomeId = dataTag["Biomes"].ByteArrayValue,
					isAllAir = true
				};

				if (chunk.biomeId.Length > 256) throw new Exception();

				// This will turn into a full chunk column
				foreach (NbtTag sectionTag in sections)
				{
					ReadSection(yoffset, sectionTag, chunk);
				}

				NbtList entities = dataTag["Entities"] as NbtList;
				NbtList blockEntities = dataTag["TileEntities"] as NbtList;
				if (blockEntities != null)
				{
					foreach (var nbtTag in blockEntities)
					{
						var blockEntityTag = (NbtCompound) nbtTag.Clone();
						string entityId = blockEntityTag["id"].StringValue;
						int x = blockEntityTag["x"].IntValue;
						int y = blockEntityTag["y"].IntValue - yoffset;
						int z = blockEntityTag["z"].IntValue;
						blockEntityTag["y"] = new NbtInt("y", y);

						if (entityId.StartsWith("minecraft:"))
						{
							var id = entityId.Split(':')[1];

							entityId = id.First().ToString().ToUpper() + id.Substring(1);

							blockEntityTag["id"] = new NbtString("id", entityId);
						}

						BlockEntity blockEntity = BlockEntityFactory.GetBlockEntityById(entityId);

						if (blockEntity != null)
						{
							blockEntityTag.Name = string.Empty;

							if (blockEntity is Sign)
							{
								// Remove the JSON stuff and get the text out of extra data.
								// TAG_String("Text2"): "{"extra":["10c a loaf!"],"text":""}"
								CleanSignText(blockEntityTag, "Text1");
								CleanSignText(blockEntityTag, "Text2");
								CleanSignText(blockEntityTag, "Text3");
								CleanSignText(blockEntityTag, "Text4");
							}
							else if (blockEntity is ChestBlockEntity)
							{
								NbtList items = (NbtList) blockEntityTag["Items"];

								if(items != null)
								{
									//for (byte i = 0; i < items.Count; i++)
									//{
									//	NbtCompound item = (NbtCompound) items[i];

									//	item.Add(new NbtShort("OriginalDamage", item["Damage"].ShortValue));

									//	byte metadata = (byte) (item["Damage"].ShortValue & 0xff);
									//	item.Remove("Damage");
									//	item.Add(new NbtByte("Damage", metadata));
									//}
								}
							}
                            else if (blockEntity is FlowerPotBlockEntity)
							{
							    string itemStringId = blockEntityTag["Item"].StringValue;

                                int itemId = StringIdConvert.ContainsKey(itemStringId) ? StringIdConvert[itemStringId] : 0;
							    int itemMeta = blockEntityTag["Data"].IntValue;

                                Func<int, byte, byte> dataConverter = (i, b) => b; // Default no-op converter
                                if (Convert.ContainsKey(itemId))
                                {
                                    dataConverter = Convert[itemId].Item2;
                                    itemId = Convert[itemId].Item1;
                                    itemMeta = dataConverter(itemId, (byte) itemMeta);
                                }

							    blockEntityTag.Remove("Item");
                                blockEntityTag.Remove("Data");

                                blockEntityTag["item"] = new NbtShort("item", (short) itemId);
                                blockEntityTag["mData"] = new NbtInt("mData", (short) itemMeta);
                            }

							chunk.SetBlockEntity(new BlockCoordinates(x, y, z), blockEntityTag);
						}
						else
						{
							if(Log.IsDebugEnabled)
								Log.Debug($"Loaded unknown block entity: {blockEntityTag}");
						}
					}
				}

				//NbtList tileTicks = dataTag["TileTicks"] as NbtList;

				chunk.isDirty = false;
				return chunk;
			}
		}

		private void ReadSection(int yoffset, NbtTag sectionTag, ChunkColumn chunk)
		{
			int sy = sectionTag["Y"].ByteValue*16;
			byte[] blocks = sectionTag["Blocks"].ByteArrayValue;
			byte[] data = sectionTag["Data"].ByteArrayValue;
			NbtTag addTag = sectionTag["Add"];
			byte[] adddata = new byte[2048];
			if (addTag != null) adddata = addTag.ByteArrayValue;
			byte[] blockLight = sectionTag["BlockLight"].ByteArrayValue;
			byte[] skyLight = sectionTag["SkyLight"].ByteArrayValue;

			for (int x = 0; x < 16; x++)
			{
				for (int z = 0; z < 16; z++)
				{
					for (int y = 0; y < 16; y++)
					{
						int yi = sy + y - yoffset;
						if (yi < 0 || yi >= 256) continue;

						int anvilIndex = y*16*16 + z*16 + x;
						int blockId = blocks[anvilIndex] + (Nibble4(adddata, anvilIndex) << 8);

						// Anvil to PE friendly converstion

						Func<int, byte, byte> dataConverter = (i, b) => b; // Default no-op converter
						if (Convert.ContainsKey(blockId))
						{
							dataConverter = Convert[blockId].Item2;
							blockId = Convert[blockId].Item1;
						}
						else
						{
							if (BlockFactory.GetBlockById((byte) blockId).GetType() == typeof (Block))
							{
								Log.Warn($"No block implemented for block ID={blockId}, Meta={data}");
								//blockId = 57;
							}
						}

						chunk.isAllAir = chunk.isAllAir && blockId == 0;
						if (blockId > 255)
						{
							Log.Warn($"Failed mapping for block ID={blockId}, Meta={data}");
							blockId = 41;
						}

						if (yi == 0 && (blockId == 8 || blockId == 9)) blockId = 7;

						chunk.SetBlock(x, yi, z, (byte) blockId);
						byte metadata = Nibble4(data, anvilIndex);
						metadata = dataConverter(blockId, metadata);

						chunk.SetMetadata(x, yi, z, metadata);
						chunk.SetBlocklight(x, yi, z, Nibble4(blockLight, anvilIndex));
						chunk.SetSkyLight(x, yi, z, Nibble4(skyLight, anvilIndex));

						if(blockId == 0) continue;

						if (blockId == 3 && metadata == 1)
						{
							// Dirt Course => (Grass Path)
							chunk.SetBlock(x, yi, z, 198);
							chunk.SetMetadata(x, yi, z, 0);
							blockId = 198;
						}
						else if (blockId == 3 && metadata == 2)
						{
							// Dirt Podzol => (Podzol)
							chunk.SetBlock(x, yi, z, 243);
							chunk.SetMetadata(x, yi, z, 0);
							blockId = 243;
						}

						if (BlockFactory.LuminousBlocks.ContainsKey(blockId))
						{
							var block = BlockFactory.GetBlockById(chunk.GetBlock(x, yi, z));
							block.Coordinates = new BlockCoordinates(x + (16 * chunk.x), yi, z + (16 * chunk.z));
							LightSources.Enqueue(block);
						}
					}
				}
			}
		}

		private static readonly Regex _signCleanRegex = new Regex(@"^{(?:""extra"":\[{""text"":""(?'extra'.*)""}\],)?""text"":""(?'text'.*)""}$", RegexOptions.Compiled);

		private static void CleanSignText(NbtCompound blockEntityTag, string tagName)
		{
			var text = blockEntityTag[tagName].StringValue;
			var replace = _signCleanRegex.Replace(text, match => Regex.Unescape(match.Groups["extra"].Success && !string.IsNullOrWhiteSpace(match.Groups["extra"].Value) ? match.Groups["extra"].Value : match.Groups["text"].Value));
			blockEntityTag[tagName] = new NbtString(tagName, replace);
		}

		private static byte Nibble4(byte[] arr, int index)
		{
			return (byte) (index%2 == 0 ? arr[index/2] & 0x0F : (arr[index/2] >> 4) & 0x0F);
		}

		private static void SetNibble4(byte[] arr, int index, byte value)
		{
			if (index%2 == 0)
			{
				arr[index/2] = (byte) ((value & 0x0F) | arr[index/2]);
			}
			else
			{
				arr[index/2] = (byte) (((value << 4) & 0xF0) | arr[index/2]);
			}
		}

		public Vector3 GetSpawnPoint()
		{
			var spawnPoint = new Vector3(LevelInfo.SpawnX, LevelInfo.SpawnY + 2 /* + WaterOffsetY*/, LevelInfo.SpawnZ);

			if (spawnPoint.Y > 256) spawnPoint.Y = 255;

			return spawnPoint;
		}

		public long GetTime()
		{
			return 6000;
			//return LevelInfo.Time;
		}

		public string GetName()
		{
			return LevelInfo.LevelName;
		}

		public void SaveLevelInfo(LevelInfo level)
		{
			if (!Directory.Exists(BasePath))
				Directory.CreateDirectory(BasePath);
			else
				return;

			if (LevelInfo.SpawnY <= 0) LevelInfo.SpawnY = 256;

			NbtFile file = new NbtFile();
			NbtTag dataTag = file.RootTag["Data"] = new NbtCompound("Data");
			level.SaveToNbt(dataTag);
			file.SaveToFile(Path.Combine(BasePath, "level.dat"), NbtCompression.ZLib);
		}

		public int SaveChunks()
		{
			int count = 0;
			try
			{
				lock (_chunkCache)
				{
					SaveLevelInfo(new LevelInfo());

					foreach (var chunkColumn in _chunkCache)
					{
						if (chunkColumn.Value != null && chunkColumn.Value.NeedSave)
						{
							SaveChunk(chunkColumn.Value, BasePath, WaterOffsetY);
							count++;
						}
					}
				}
			}
			catch (Exception e)
			{
				Log.Error("saving chunks", e);
			}

			return count;
		}

		public static void SaveChunk(ChunkColumn chunk, string basePath, int yoffset)
		{
			// WARNING: This method does not consider growing size of the chunks. Needs refactoring to find
			// free sectors and clear up old ones. It works fine as long as no dynamic data is written
			// like block entity data (signs etc).

			Log.Debug($"Save chunk X={chunk.x}, Z={chunk.z} to {basePath}");

			chunk.NeedSave = false;

			var coordinates = new ChunkCoordinates(chunk.x, chunk.z);

			int width = 32;
			int depth = 32;

			int rx = coordinates.X >> 5;
			int rz = coordinates.Z >> 5;

			string filePath = Path.Combine(basePath, string.Format(@"region{2}r.{0}.{1}.mca", rx, rz, Path.DirectorySeparatorChar));

			if (!File.Exists(filePath))
			{
				// Make sure directory exist
				Directory.CreateDirectory(Path.Combine(basePath, "region"));

				// Create empty region file
				using (var regionFile = File.Open(filePath, FileMode.CreateNew))
				{
					byte[] buffer = new byte[8192];
					regionFile.Write(buffer, 0, buffer.Length);
				}

				return;
			}

			using (var regionFile = File.Open(filePath, FileMode.Open))
			{
				byte[] buffer = new byte[8192];
				regionFile.Read(buffer, 0, buffer.Length);

				int xi = (coordinates.X%width);
				if (xi < 0) xi += 32;
				int zi = (coordinates.Z%depth);
				if (zi < 0) zi += 32;
				int tableOffset = (xi + zi*width)*4;

				regionFile.Seek(tableOffset, SeekOrigin.Begin);

				byte[] offsetBuffer = new byte[4];
				regionFile.Read(offsetBuffer, 0, 3);
				Array.Reverse(offsetBuffer);
				int offset = BitConverter.ToInt32(offsetBuffer, 0) << 4;
				int length = regionFile.ReadByte();

				// Seriaize NBT to get lenght
				NbtFile nbt = CreateNbtFromChunkColumn(chunk, yoffset);
				byte[] nbtBuf = nbt.SaveToBuffer(NbtCompression.ZLib);
				int nbtLength = nbtBuf.Length;
				// Don't write yet, just use the lenght

				if (offset == 0 || length == 0 || nbtLength < length)
				{
					if(length != 0) Log.Debug("Creating new sectors for this chunk even tho it existed");

					regionFile.Seek(0, SeekOrigin.End);
					offset = (int) ((int) regionFile.Position & 0xfffffff0);

					regionFile.Seek(tableOffset, SeekOrigin.Begin);

					byte[] bytes = BitConverter.GetBytes(offset >> 4);
					Array.Reverse(bytes);
					regionFile.Write(bytes, 0, 3);
					regionFile.WriteByte(1);
				}

				byte[] lenghtBytes = BitConverter.GetBytes(nbtLength + 1);
				Array.Reverse(lenghtBytes);

				regionFile.Seek(offset, SeekOrigin.Begin);
				regionFile.Write(lenghtBytes, 0, 4); // Lenght
				regionFile.WriteByte(0x02); // Compression mode

				regionFile.Write(nbtBuf, 0, nbtBuf.Length);

				int reminder;
				Math.DivRem(nbtLength + 4, 4096, out reminder);

				byte[] padding = new byte[4096 - reminder];
				if (padding.Length > 0) regionFile.Write(padding, 0, padding.Length);
			}
		}

		public static NbtFile CreateNbtFromChunkColumn(ChunkColumn chunk, int yoffset)
		{
			var nbt = new NbtFile();

			NbtCompound levelTag = new NbtCompound("Level");
			nbt.RootTag.Add(levelTag);

			levelTag.Add(new NbtInt("xPos", chunk.x));
			levelTag.Add(new NbtInt("zPos", chunk.z));
			levelTag.Add(new NbtByteArray("Biomes", chunk.biomeId));

			NbtList sectionsTag = new NbtList("Sections");
			levelTag.Add(sectionsTag);

			for (int i = 0; i < 8; i++)
			{
				NbtCompound sectionTag = new NbtCompound();
				sectionsTag.Add(sectionTag);
				sectionTag.Add(new NbtByte("Y", (byte) i));
				int sy = i*16;

				byte[] blocks = new byte[4096];
				byte[] data = new byte[2048];
				byte[] blockLight = new byte[2048];
				byte[] skyLight = new byte[2048];

				for (int x = 0; x < 16; x++)
				{
					for (int z = 0; z < 16; z++)
					{
						for (int y = 0; y < 16; y++)
						{
							int yi = sy + y;
							if (yi < 0 || yi >= 256) continue; // ?

							int anvilIndex = (y + yoffset)*16*16 + z*16 + x;
							byte blockId = chunk.GetBlock(x, yi, z);

							// PE to Anvil friendly converstion
							if (blockId == 5) blockId = 125;
							else if (blockId == 158) blockId = 126;
							else if (blockId == 50) blockId = 75;
							else if (blockId == 50) blockId = 76;
							else if (blockId == 89) blockId = 123;
							else if (blockId == 89) blockId = 124;
							else if (blockId == 73) blockId = 152;

							blocks[anvilIndex] = blockId;
							SetNibble4(data, anvilIndex, chunk.GetMetadata(x, yi, z));
							SetNibble4(blockLight, anvilIndex, chunk.GetBlocklight(x, yi, z));
							SetNibble4(skyLight, anvilIndex, chunk.GetSkylight(x, yi, z));
						}
					}
				}

				sectionTag.Add(new NbtByteArray("Blocks", blocks));
				sectionTag.Add(new NbtByteArray("Data", data));
				sectionTag.Add(new NbtByteArray("BlockLight", blockLight));
				sectionTag.Add(new NbtByteArray("SkyLight", skyLight));
			}

			// TODO: Save entities
			NbtList entitiesTag = new NbtList("Entities", NbtTagType.Compound);
			levelTag.Add(entitiesTag);

			NbtList blockEntitiesTag = new NbtList("TileEntities", NbtTagType.Compound);
			levelTag.Add(blockEntitiesTag);
			foreach (NbtCompound blockEntityNbt in chunk.BlockEntities.Values)
			{
				NbtCompound nbtClone = (NbtCompound) blockEntityNbt.Clone();
				nbtClone.Name = null;
				blockEntitiesTag.Add(nbtClone);
			}

			levelTag.Add(new NbtList("TileTicks", NbtTagType.Compound));

			return nbt;
		}

		public int NumberOfCachedChunks()
		{
			return _chunkCache.Count;
		}

		public object Clone()
		{
			ConcurrentDictionary<ChunkCoordinates, ChunkColumn> chunkCache = new ConcurrentDictionary<ChunkCoordinates, ChunkColumn>();
			foreach (KeyValuePair<ChunkCoordinates, ChunkColumn> valuePair in _chunkCache)
			{
				chunkCache.TryAdd(valuePair.Key, (ChunkColumn) valuePair.Value?.Clone());
			}

			AnvilWorldProvider provider = new AnvilWorldProvider(BasePath, (LevelInfo) LevelInfo.Clone(), WaterOffsetY, chunkCache);
			return provider;
		}

		public int PruneAir()
		{
			int prunedChunks = 0;
			Stopwatch sw = new Stopwatch();
			sw.Start();

			foreach (KeyValuePair<ChunkCoordinates, ChunkColumn> valuePair in _chunkCache)
			{
				ChunkCoordinates chunkCoordinates = valuePair.Key;
				ChunkColumn chunkColumn = valuePair.Value;

				if (chunkColumn != null && chunkColumn.isAllAir)
				{
					bool surroundingIsAir = true;

					for (int startX = chunkCoordinates.X - 1; startX <= chunkCoordinates.X + 1; startX++)
					{
						for (int startZ = chunkCoordinates.Z - 1; startZ <= chunkCoordinates.Z + 1; startZ++)
						{
							ChunkCoordinates surroundingChunkCoordinates = new ChunkCoordinates(startX, startZ);

							if (!surroundingChunkCoordinates.Equals(chunkCoordinates))
							{
								ChunkColumn surroundingChunkColumn;

								_chunkCache.TryGetValue(surroundingChunkCoordinates, out surroundingChunkColumn);

								if (surroundingChunkColumn != null && !surroundingChunkColumn.isAllAir)
								{
									surroundingIsAir = false;
									break;
								}
							}
						}
					}

					if (surroundingIsAir)
					{
						_chunkCache[chunkCoordinates] = null;
						prunedChunks++;
					}
				}
			}

			sw.Stop();
			Log.Info("Pruned " + prunedChunks + " in " + sw.ElapsedMilliseconds + "ms");
			return prunedChunks;
		}

		public int MakeAirChunksAroundWorldToCompensateForBadRendering()
		{
			int createdChunks = 0;
			Stopwatch sw = new Stopwatch();
			sw.Start();

			foreach (KeyValuePair<ChunkCoordinates, ChunkColumn> valuePair in _chunkCache)
			{
				ChunkCoordinates chunkCoordinates = valuePair.Key;
				ChunkColumn chunkColumn = valuePair.Value;

				if (chunkColumn != null && !chunkColumn.isAllAir)
				{
					for (int startX = chunkCoordinates.X - 1; startX <= chunkCoordinates.X + 1; startX++)
					{
						for (int startZ = chunkCoordinates.Z - 1; startZ <= chunkCoordinates.Z + 1; startZ++)
						{
							ChunkCoordinates surroundingChunkCoordinates = new ChunkCoordinates(startX, startZ);

							if (surroundingChunkCoordinates.Equals(chunkCoordinates)) continue;

							ChunkColumn surroundingChunkColumn;

							_chunkCache.TryGetValue(surroundingChunkCoordinates, out surroundingChunkColumn);

							if (surroundingChunkColumn == null)
							{
								ChunkColumn airColumn = new ChunkColumn
								{
									x = startX,
									z = startZ,
									isAllAir = true
								};

								airColumn.GetBatch();

								_chunkCache[surroundingChunkCoordinates] = airColumn;
								createdChunks++;
							}
						}
					}
				}
			}

			sw.Stop();
			Log.Info("Created " + createdChunks + " air chunks in " + sw.ElapsedMilliseconds + "ms");
			return createdChunks;
		}
	}
}
