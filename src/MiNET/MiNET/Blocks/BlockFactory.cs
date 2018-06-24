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

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using log4net;
using Newtonsoft.Json.Linq;

namespace MiNET.Blocks
{
	public interface ICustomBlockFactory
	{
		Block GetBlockById(int blockId);
	}

	public static class BlockFactory
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(BlockFactory));

		public static ICustomBlockFactory CustomBlockFactory { get; set; }

		public static readonly byte[] TransparentBlocks = new byte[500];
		public static readonly byte[] LuminousBlocks = new byte[500];
		public static Dictionary<string, int> NameToId { get; private set; }
		public static Blockstates Blockstates { get; set; } = new Blockstates();

		private static int[] LegacyToRuntimeId = new int[65536];

		//public static Dictionary<(int, int), int> BlockStates = new Dictionary<(int, int), int>();

		static BlockFactory()
		{
			for (int i = 0; i < byte.MaxValue*2; i++)
			{
				var block = GetBlockById(i);
				if (block != null)
				{
					if (block.IsTransparent)
					{
						TransparentBlocks[block.Id] = 1;
					}
					if (block.LightLevel > 0)
					{
						LuminousBlocks[block.Id] = (byte) block.LightLevel;
					}
				}
			}

			NameToId = BuildNameToId();

			for (int i = 0; i < LegacyToRuntimeId.Length; ++i)
			{
				LegacyToRuntimeId[i] = -1;
			}

			var assembly = Assembly.GetAssembly(typeof(Block));
			using (Stream stream = assembly.GetManifestResourceStream(typeof(Block).Namespace + ".blockstates.json"))
			using (StreamReader reader = new StreamReader(stream))
			{
				dynamic result = JArray.Parse(reader.ReadToEnd());

				int runtimeId = 0;
				foreach (var obj in result)
				{
					LegacyToRuntimeId[((int) obj.id << 4) | (int) obj.data] = (int)runtimeId;
					Blockstates.Add(runtimeId, new Blockstate(){Id = (int)obj.id, Data = (short)obj.data, Name = (string)obj.name, RuntimeId = runtimeId});

					runtimeId++;
				}
			}
		}

		private static Dictionary<string, int> BuildNameToId()
		{
			var nameToId = new Dictionary<string, int>();
			for (byte idx = 0; idx < byte.MaxValue; idx++)
			{
				Block block = GetBlockById(idx);
				string name = block.GetType().Name.ToLowerInvariant();

				if (name.Equals("block"))
				{
					//if (Log.IsDebugEnabled)
					//	Log.Debug($"Missing implementation for block ID={idx}");
					continue;
				}

				nameToId.Add(name, idx);
			}

			return nameToId;
		}

		public static int GetBlockIdByName(string blockName)
		{
			blockName = blockName.ToLowerInvariant();
			blockName = blockName.Replace("_", "");

			if (NameToId.ContainsKey(blockName))
			{
				return NameToId[blockName];
			}

			return 0;
		}

		public static Block GetBlockByName(string blockName)
		{
			if (string.IsNullOrEmpty(blockName)) return null;

			if (blockName.StartsWith("minecraft:")) blockName = blockName.Substring(10);
			blockName = blockName.ToLowerInvariant();
			blockName = blockName.Replace("_", "");

			if (NameToId.ContainsKey(blockName))
			{
				return GetBlockById(NameToId[blockName]);
			}

			return null;
		}

		public static Block GetBlockById(int blockId)
		{
			Block block = null;

			if (CustomBlockFactory != null)
			{
				block = CustomBlockFactory.GetBlockById(blockId);
			}

			if (block != null) return block;

			if (blockId == 0) block = new Air();
			else if (blockId == 1) block = new Stone();
			else if (blockId == 2) block = new Grass();
			else if (blockId == 3) block = new Dirt();
			else if (blockId == 4) block = new Cobblestone();
			else if (blockId == 5) block = new Planks();
			else if (blockId == 6) block = new Sapling();
			else if (blockId == 7) block = new Bedrock();
			else if (blockId == 8) block = new FlowingWater();
			else if (blockId == 9) block = new Water();
			else if (blockId == 10) block = new FlowingLava();
			else if (blockId == 11) block = new Lava();
			else if (blockId == 12) block = new Sand();
			else if (blockId == 13) block = new Gravel();
			else if (blockId == 14) block = new GoldOre();
			else if (blockId == 15) block = new IronOre();
			else if (blockId == 16) block = new CoalOre();
			else if (blockId == 17) block = new Log();
			else if (blockId == 18) block = new Leaves();
			else if (blockId == 19) block = new Sponge();
			else if (blockId == 20) block = new Glass();
			else if (blockId == 21) block = new LapisOre();
			else if (blockId == 22) block = new LapisBlock();
			else if (blockId == 23) block = new Dispenser();
			else if (blockId == 24) block = new Sandstone();
			else if (blockId == 25) block = new NoteBlock();
			else if (blockId == 26) block = new Bed();
			else if (blockId == 27) block = new GoldenRail();
			else if (blockId == 28) block = new DetectorRail();
			else if (blockId == 29) block = new StickyPiston();
			else if (blockId == 30) block = new Web();
			else if (blockId == 31) block = new TallGrass();
			else if (blockId == 32) block = new DeadBush();
			else if (blockId == 33) block = new Piston();
			else if (blockId == 34) block = new PistonHead();
			else if (blockId == 35) block = new Wool();
			else if (blockId == 37) block = new YellowFlower();
			else if (blockId == 38) block = new RedFlower();
			else if (blockId == 39) block = new BrownMushroom();
			else if (blockId == 40) block = new RedMushroom();
			else if (blockId == 41) block = new GoldBlock();
			else if (blockId == 42) block = new IronBlock();
			else if (blockId == 43) block = new DoubleStoneSlab();
			else if (blockId == 44) block = new StoneSlab();
			else if (blockId == 45) block = new BrickBlock();
			else if (blockId == 46) block = new Tnt();
			else if (blockId == 47) block = new Bookshelf();
			else if (blockId == 48) block = new MossyCobblestone();
			else if (blockId == 49) block = new Obsidian();
			else if (blockId == 50) block = new Torch();
			else if (blockId == 51) block = new Fire();
			else if (blockId == 52) block = new MobSpawner();
			else if (blockId == 53) block = new OakStairs();
			else if (blockId == 54) block = new Chest();

			else if (blockId == 55) block = new RedstoneWire();
			else if (blockId == 56) block = new DiamondOre();
			else if (blockId == 57) block = new DiamondBlock();
			else if (blockId == 58) block = new CraftingTable();
			else if (blockId == 59) block = new Wheat();
			else if (blockId == 60) block = new Farmland();
			else if (blockId == 61) block = new Furnace();
			else if (blockId == 62) block = new LitFurnace();
			else if (blockId == 63) block = new StandingSign();
			else if (blockId == 64) block = new WoodenDoor();
			else if (blockId == 65) block = new Ladder();
			else if (blockId == 66) block = new Rail();
			else if (blockId == 67) block = new StoneStairs();
			else if (blockId == 68) block = new WallSign();
			else if (blockId == 69) block = new Lever();
			else if (blockId == 70) block = new StonePressurePlate();
			else if (blockId == 71) block = new IronDoor();
			else if (blockId == 72) block = new WoodenPressurePlate();
			else if (blockId == 73) block = new RedstoneOre();
			else if (blockId == 74) block = new LitRedstoneOre();
			else if (blockId == 75) block = new UnlitRedstoneTorch();
			else if (blockId == 76) block = new RedstoneTorch();
			else if (blockId == 77) block = new StoneButton();
			else if (blockId == 78) block = new SnowLayer();
			else if (blockId == 79) block = new Ice();
			else if (blockId == 80) block = new Snow();
			else if (blockId == 81) block = new Cactus();
			else if (blockId == 82) block = new Clay();
			else if (blockId == 83) block = new Reeds();
			else if (blockId == 83) block = new Jukebox();
			else if (blockId == 85) block = new Fence();
			else if (blockId == 86) block = new Pumpkin();
			else if (blockId == 87) block = new Netherrack();
			else if (blockId == 88) block = new SoulSand();
			else if (blockId == 89) block = new Glowstone();
			else if (blockId == 90) block = new Portal();
			else if (blockId == 91) block = new LitPumpkin();
			else if (blockId == 92) block = new Cake();
			else if (blockId == 93) block = new UnpoweredRepeater();
			else if (blockId == 94) block = new PoweredRepeater();
			else if (blockId == 95) block = new InvisibleBedrock();
			else if (blockId == 96) block = new Trapdoor();
			else if (blockId == 97) block = new MonsterEgg();
			else if (blockId == 98) block = new StoneBrick();
			else if (blockId == 99) block = new BrownMushroomBlock();
			else if (blockId == 100) block = new RedMushroomBlock();
			else if (blockId == 101) block = new IronBars();
			else if (blockId == 102) block = new GlassPane();
			else if (blockId == 103) block = new MelonBlock();
			else if (blockId == 104) block = new PumpkinStem();
			else if (blockId == 105) block = new MelonStem();
			else if (blockId == 106) block = new Vine();
			else if (blockId == 107) block = new FenceGate();
			else if (blockId == 108) block = new BrickStairs();
			else if (blockId == 109) block = new StoneBrickStairs();

			else if (blockId == 110) block = new Mycelium();
			else if (blockId == 111) block = new Waterlily();
			else if (blockId == 112) block = new NetherBrick();
			else if (blockId == 113) block = new NetherBrickFence();
			else if (blockId == 114) block = new NetherBrickStairs();
			else if (blockId == 115) block = new NetherWart();
			else if (blockId == 116) block = new EnchantingTable();
			else if (blockId == 117) block = new BrewingStand();
			else if (blockId == 118) block = new Cauldron();
			else if (blockId == 119) block = new EndPortal();
			else if (blockId == 120) block = new EndPortalFrame();
			else if (blockId == 121) block = new EndStone();
			else if (blockId == 122) block = new DragonEgg();
			else if (blockId == 123) block = new RedstoneLamp();
			else if (blockId == 124) block = new LitRedstoneLamp();
			else if (blockId == 125) block = new Dropper();
			else if (blockId == 126) block = new ActivatorRail();
			else if (blockId == 127) block = new Cocoa();
			else if (blockId == 128) block = new SandStoneStairs();
			else if (blockId == 129) block = new EmeraldOre();
			else if (blockId == 130) block = new EnderChest();
			else if (blockId == 131) block = new TripwireHook();
			else if (blockId == 132) block = new Tripwire();
			else if (blockId == 133) block = new EmeraldBlock();
			else if (blockId == 134) block = new SpruceStairs();
			else if (blockId == 135) block = new BirchStairs();
			else if (blockId == 136) block = new JungleStairs();
			else if (blockId == 138) block = new Beacon();
			else if (blockId == 139) block = new CobblestoneWall();
			else if (blockId == 140) block = new FlowerPot();
			else if (blockId == 141) block = new Carrots();
			else if (blockId == 142) block = new Potatoes();
			else if (blockId == 143) block = new WoodenButton();
			else if (blockId == 144) block = new Skull();
			else if (blockId == 145) block = new Anvil();
			else if (blockId == 146) block = new TrappedChest();
			else if (blockId == 147) block = new LightWeightedPressurePlate();
			else if (blockId == 148) block = new HeavyWeightedPressurePlate();
			else if (blockId == 149) block = new UnpoweredComparator();
			else if (blockId == 150) block = new PoweredComparator();
			else if (blockId == 151) block = new DaylightDetector();
			else if (blockId == 152) block = new RedstoneBlock();
			else if (blockId == 153) block = new QuartzOre();
			else if (blockId == 154) block = new Hopper();
			else if (blockId == 155) block = new QuartzBlock();
			else if (blockId == 156) block = new QuartzStairs();
			else if (blockId == 157) block = new DoubleWoodenSlab();
			else if (blockId == 158) block = new WoodenSlab();
			else if (blockId == 159) block = new StainedHardenedClay();
			else if (blockId == 160) block = new StainedGlassPane();
			else if (blockId == 161) block = new Leaves2();
			else if (blockId == 162) block = new Log2();
			else if (blockId == 163) block = new AcaciaStairs();
			else if (blockId == 164) block = new DarkOakStairs();

			else if (blockId == 165) block = new Slime();
			else if (blockId == 167) block = new IronTrapdoor();
			else if (blockId == 168) block = new Prismarine();
			else if (blockId == 169) block = new SeaLantern();
			else if (blockId == 170) block = new HayBlock();
			else if (blockId == 171) block = new Carpet();
			else if (blockId == 172) block = new HardenedClay();
			else if (blockId == 173) block = new CoalBlock();
			else if (blockId == 174) block = new PackedIce();
			else if (blockId == 175) block = new DoublePlant();
			else if (blockId == 176) block = new StandingBanner();
			else if (blockId == 177) block = new WallBanner();
			else if (blockId == 178) block = new DaylightDetectorInverted();
			else if (blockId == 179) block = new RedSandstone();
			else if (blockId == 180) block = new RedSandstoneStairs();
			else if (blockId == 181) block = new DoubleStoneSlab2();
			else if (blockId == 182) block = new StoneSlab2();
			else if (blockId == 183) block = new SpruceFenceGate();
			else if (blockId == 184) block = new BirchFenceGate();
			else if (blockId == 185) block = new JungleFenceGate();
			else if (blockId == 186) block = new DarkOakFenceGate();
			else if (blockId == 187) block = new AcaciaFenceGate();
			else if (blockId == 193) block = new SpruceDoor();
			else if (blockId == 194) block = new BirchDoor();
			else if (blockId == 195) block = new JungleDoor();
			else if (blockId == 196) block = new AcaciaDoor();
			else if (blockId == 197) block = new DarkOakDoor();
			else if (blockId == 198) block = new GrassPath();
			else if (blockId == 199) block = new Frame();
			else if (blockId == 200) block = new ChorusFlower();
			else if (blockId == 201) block = new PurpurBlock();
			else if (blockId == 203) block = new PurpurStairs();
			else if (blockId == 206) block = new EndBricks();
			else if (blockId == 207) block = new FrostedIce();
			else if (blockId == 208) block = new EndRod();
			else if (blockId == 209) block = new EndGateway();
			else if (blockId == 219) block = new PurpleGlazedTerracotta();
			else if (blockId == 220) block = new WhiteGlazedTerracotta();
			else if (blockId == 221) block = new OrangeGlazedTerracotta();
			else if (blockId == 222) block = new MagentaGlazedTerracotta();
			else if (blockId == 223) block = new LightBlueGlazedTerracotta();
			else if (blockId == 224) block = new YellowGlazedTerracotta();
			else if (blockId == 225) block = new LimeGlazedTerracotta();
			else if (blockId == 226) block = new PinkGlazedTerracotta();
			else if (blockId == 227) block = new GrayGlazedTerracotta();
			else if (blockId == 228) block = new SilverGlazedTerracotta();
			else if (blockId == 229) block = new CyanGlazedTerracotta();
			else if (blockId == 231) block = new BlueGlazedTerracotta();
			else if (blockId == 232) block = new BrownGlazedTerracotta();
			else if (blockId == 233) block = new GreenGlazedTerracotta();
			else if (blockId == 234) block = new RedGlazedTerracotta();
			else if (blockId == 235) block = new BlackGlazedTerracotta();
			else if (blockId == 236) block = new Concrete();
			else if (blockId == 237) block = new ConcretePowder();
			else if (blockId == 240) block = new ChorusPlant();
			else if (blockId == 241) block = new StainedGlass();
			else if (blockId == 243) block = new Podzol();
			else if (blockId == 244) block = new Beetroot();
			else if (blockId == 245) block = new Stonecutter();
			else if (blockId == 246) block = new GlowingObsidian();
			else if (blockId == 247) block = new NetherReactorCore();
			else if (blockId == 251) block = new Observer();
			else
			{
				//				Log.DebugFormat(@"
				//	// Add this missing block to the BlockFactory
				//	else if (blockId == {1}) block = new {0}();
				//	
				//	public class {0} : Block
				//	{{
				//		internal {0}() : base({1})
				//		{{
				//		}}
				//	}}
				//", "Missing", blockId);
				block = new Block(blockId);
			}

			return block;
		}

		public static uint GetRuntimeId(int blockId, byte metadata)
		{
			int idx = TryGetRuntimeId(blockId, metadata);
			if (idx != -1)
			{
				return (uint) idx;
			}

			//block found with bad metadata, try getting with zero
			idx = TryGetRuntimeId(blockId, 0);
			if (idx != -1)
			{
				return (uint) idx;
			}

			Log.Warn($"Trying to get runtime ID failed for {blockId} and data {metadata}");
			return (uint) TryGetRuntimeId(248, 0); //legacy id for info_update block (for unknown block)
		}

		private static int TryGetRuntimeId(int blockId, byte metadata)
		{
			return LegacyToRuntimeId[(blockId << 4) | metadata];
		}
	}

	//public class BlockStateUtils
	//{
	//	public static Dictionary<(int, int), int> BlockStates = new Dictionary<(int, int), int>();

	//	static BlockStateUtils()
	//	{
	//		var assembly = Assembly.GetAssembly(typeof(Block));
	//		using (Stream stream = assembly.GetManifestResourceStream(typeof(Block).Namespace + ".blockstates.json"))
	//		using (StreamReader reader = new StreamReader(stream))
	//		{
	//			dynamic result = JArray.Parse(reader.ReadToEnd());

	//			foreach (var obj in result)
	//			{
	//				int bid = (int) obj.id;
	//				int data = (int) obj.data;
	//				short hash = (short)(bid << 4 | (data & 0x0f));
	//				BlockStates.Add((bid, data), hash);
	//			}
	//		}
	//	}
	//}

}