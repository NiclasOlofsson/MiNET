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

        private static int[] LegacyToRuntimeId = new int[65536];

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
<<<<<<< HEAD
            BuildRuntimeIdTable();
=======

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
>>>>>>> 86f35b43910890e118cedd4a207ba5d5e79c1298
		}

		private static Dictionary<string, int> BuildNameToId()
		{
			var nameToId = new Dictionary<string, int>();
			for (int idx = 0; idx < 1000; idx++)
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
			else if (blockId == 84) block = new Jukebox();
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
			else if (blockId == 205) block = new UndyedShulkerBox();
			else if (blockId == 206) block = new EndBricks();
			else if (blockId == 207) block = new FrostedIce();
			else if (blockId == 208) block = new EndRod();
			else if (blockId == 209) block = new EndGateway();
			else if (blockId == 210) block = new Allow();
			else if (blockId == 211) block = new Deny();
			else if (blockId == 212) block = new Border();
			else if (blockId == 218) block = new ShulkerBox();
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
			else if (blockId == 230) block = new Chalkboard();
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

			else if (blockId == 34) block = new Pistonarmcollision();
			else if (blockId == 36) block = new Element0();
			else if (blockId == 137) block = new CommandBlock();
			else if (blockId == 166) block = new GlowStick();
			else if (blockId == 188) block = new RepeatingCommandBlock();
			else if (blockId == 189) block = new ChainCommandBlock();
			else if (blockId == 190) block = new HardGlassPane();
			else if (blockId == 191) block = new HardStainedGlassPane();
			else if (blockId == 192) block = new ChemicalHeat();
			else if (blockId == 202) block = new ColoredTorchRg();
			else if (blockId == 204) block = new ColoredTorchBp();
			else if (blockId == 213) block = new Magma();
			else if (blockId == 214) block = new NetherWartBlock();
			else if (blockId == 215) block = new RedNetherBrick();
			else if (blockId == 216) block = new BoneBlock();
			else if (blockId == 238) block = new ChemistryTable();
			else if (blockId == 239) block = new UnderwaterTorch();
			else if (blockId == 247) block = new Netherreactor();
			else if (blockId == 248) block = new InfoUpdate();
			else if (blockId == 249) block = new InfoUpdate2();
			else if (blockId == 250) block = new Movingblock();
			else if (blockId == 252) block = new StructureBlock();
			else if (blockId == 253) block = new HardGlass();
			else if (blockId == 254) block = new HardStainedGlass();
			else if (blockId == 255) block = new Reserved6();
			else if (blockId == 260) block = new StrippedSpruceLog();
			else if (blockId == 261) block = new StrippedBirchLog();
			else if (blockId == 262) block = new StrippedJungleLog();
			else if (blockId == 263) block = new StrippedAcaciaLog();
			else if (blockId == 264) block = new StrippedDarkOakLog();
			else if (blockId == 265) block = new StrippedOakLog();
			else if (blockId == 266) block = new BlueIce();
			else if (blockId == 267) block = new Element1();
			else if (blockId == 268) block = new Element2();
			else if (blockId == 269) block = new Element3();
			else if (blockId == 270) block = new Element4();
			else if (blockId == 271) block = new Element5();
			else if (blockId == 272) block = new Element6();
			else if (blockId == 273) block = new Element7();
			else if (blockId == 274) block = new Element8();
			else if (blockId == 275) block = new Element9();
			else if (blockId == 276) block = new Element10();
			else if (blockId == 277) block = new Element11();
			else if (blockId == 278) block = new Element12();
			else if (blockId == 279) block = new Element13();
			else if (blockId == 280) block = new Element14();
			else if (blockId == 281) block = new Element15();
			else if (blockId == 282) block = new Element16();
			else if (blockId == 283) block = new Element17();
			else if (blockId == 284) block = new Element18();
			else if (blockId == 285) block = new Element19();
			else if (blockId == 286) block = new Element20();
			else if (blockId == 287) block = new Element21();
			else if (blockId == 288) block = new Element22();
			else if (blockId == 289) block = new Element23();
			else if (blockId == 290) block = new Element24();
			else if (blockId == 291) block = new Element25();
			else if (blockId == 292) block = new Element26();
			else if (blockId == 293) block = new Element27();
			else if (blockId == 294) block = new Element28();
			else if (blockId == 295) block = new Element29();
			else if (blockId == 296) block = new Element30();
			else if (blockId == 297) block = new Element31();
			else if (blockId == 298) block = new Element32();
			else if (blockId == 299) block = new Element33();
			else if (blockId == 300) block = new Element34();
			else if (blockId == 301) block = new Element35();
			else if (blockId == 302) block = new Element36();
			else if (blockId == 303) block = new Element37();
			else if (blockId == 304) block = new Element38();
			else if (blockId == 305) block = new Element39();
			else if (blockId == 306) block = new Element40();
			else if (blockId == 307) block = new Element41();
			else if (blockId == 308) block = new Element42();
			else if (blockId == 309) block = new Element43();
			else if (blockId == 310) block = new Element44();
			else if (blockId == 311) block = new Element45();
			else if (blockId == 312) block = new Element46();
			else if (blockId == 313) block = new Element47();
			else if (blockId == 314) block = new Element48();
			else if (blockId == 315) block = new Element49();
			else if (blockId == 316) block = new Element50();
			else if (blockId == 317) block = new Element51();
			else if (blockId == 318) block = new Element52();
			else if (blockId == 319) block = new Element53();
			else if (blockId == 320) block = new Element54();
			else if (blockId == 321) block = new Element55();
			else if (blockId == 322) block = new Element56();
			else if (blockId == 323) block = new Element57();
			else if (blockId == 324) block = new Element58();
			else if (blockId == 325) block = new Element59();
			else if (blockId == 326) block = new Element60();
			else if (blockId == 327) block = new Element61();
			else if (blockId == 328) block = new Element62();
			else if (blockId == 329) block = new Element63();
			else if (blockId == 330) block = new Element64();
			else if (blockId == 331) block = new Element65();
			else if (blockId == 332) block = new Element66();
			else if (blockId == 333) block = new Element67();
			else if (blockId == 334) block = new Element68();
			else if (blockId == 335) block = new Element69();
			else if (blockId == 336) block = new Element70();
			else if (blockId == 337) block = new Element71();
			else if (blockId == 338) block = new Element72();
			else if (blockId == 339) block = new Element73();
			else if (blockId == 340) block = new Element74();
			else if (blockId == 341) block = new Element75();
			else if (blockId == 342) block = new Element76();
			else if (blockId == 343) block = new Element77();
			else if (blockId == 344) block = new Element78();
			else if (blockId == 345) block = new Element79();
			else if (blockId == 346) block = new Element80();
			else if (blockId == 347) block = new Element81();
			else if (blockId == 348) block = new Element82();
			else if (blockId == 349) block = new Element83();
			else if (blockId == 350) block = new Element84();
			else if (blockId == 351) block = new Element85();
			else if (blockId == 352) block = new Element86();
			else if (blockId == 353) block = new Element87();
			else if (blockId == 354) block = new Element88();
			else if (blockId == 355) block = new Element89();
			else if (blockId == 356) block = new Element90();
			else if (blockId == 357) block = new Element91();
			else if (blockId == 358) block = new Element92();
			else if (blockId == 359) block = new Element93();
			else if (blockId == 360) block = new Element94();
			else if (blockId == 361) block = new Element95();
			else if (blockId == 362) block = new Element96();
			else if (blockId == 363) block = new Element97();
			else if (blockId == 364) block = new Element98();
			else if (blockId == 365) block = new Element99();
			else if (blockId == 366) block = new Element100();
			else if (blockId == 367) block = new Element101();
			else if (blockId == 368) block = new Element102();
			else if (blockId == 369) block = new Element103();
			else if (blockId == 370) block = new Element104();
			else if (blockId == 371) block = new Element105();
			else if (blockId == 372) block = new Element106();
			else if (blockId == 373) block = new Element107();
			else if (blockId == 374) block = new Element108();
			else if (blockId == 375) block = new Element109();
			else if (blockId == 376) block = new Element110();
			else if (blockId == 377) block = new Element111();
			else if (blockId == 378) block = new Element112();
			else if (blockId == 379) block = new Element113();
			else if (blockId == 380) block = new Element114();
			else if (blockId == 381) block = new Element115();
			else if (blockId == 382) block = new Element116();
			else if (blockId == 383) block = new Element117();
			else if (blockId == 384) block = new Element118();
			else if (blockId == 385) block = new Seagrass();
			else if (blockId == 386) block = new Coral();
			else if (blockId == 387) block = new CoralBlock();
			else if (blockId == 388) block = new CoralFan();
			else if (blockId == 389) block = new CoralFanDead();
			else if (blockId == 390) block = new CoralFanHang();
			else if (blockId == 391) block = new CoralFanHang2();
			else if (blockId == 392) block = new CoralFanHang3();
			else if (blockId == 393) block = new Kelp();
			else if (blockId == 394) block = new DriedKelpBlock();
			else if (blockId == 395) block = new AcaciaButton();
			else if (blockId == 396) block = new BirchButton();
			else if (blockId == 397) block = new DarkOakButton();
			else if (blockId == 398) block = new JungleButton();
			else if (blockId == 399) block = new SpruceButton();
			else if (blockId == 400) block = new AcaciaTrapdoor();
			else if (blockId == 401) block = new BirchTrapdoor();
			else if (blockId == 402) block = new DarkOakTrapdoor();
			else if (blockId == 403) block = new JungleTrapdoor();
			else if (blockId == 404) block = new SpruceTrapdoor();
			else if (blockId == 405) block = new AcaciaPressurePlate();
			else if (blockId == 406) block = new BirchPressurePlate();
			else if (blockId == 407) block = new DarkOakPressurePlate();
			else if (blockId == 408) block = new JunglePressurePlate();
			else if (blockId == 409) block = new SprucePressurePlate();
			else if (blockId == 410) block = new CarvedPumpkin();
			else if (blockId == 411) block = new SeaPickle();
			else if (blockId == 412) block = new Conduit();
			else if (blockId == 414) block = new TurtleEgg();
			else if (blockId == 415) block = new BubbleColumn();


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

<<<<<<< HEAD
        private static void BuildRuntimeIdTable()
        {
            for (int i = 0; i < LegacyToRuntimeId.Length; ++i)
            {
                LegacyToRuntimeId[i] = -1;
            }

            LegacyToRuntimeId[0] = 0; //minecraft:air:0
            LegacyToRuntimeId[16] = 1; //minecraft:stone:0
            LegacyToRuntimeId[17] = 2; //minecraft:stone:1
            LegacyToRuntimeId[18] = 3; //minecraft:stone:2
            LegacyToRuntimeId[19] = 4; //minecraft:stone:3
            LegacyToRuntimeId[20] = 5; //minecraft:stone:4
            LegacyToRuntimeId[21] = 6; //minecraft:stone:5
            LegacyToRuntimeId[22] = 7; //minecraft:stone:6
            LegacyToRuntimeId[23] = 8; //minecraft:stone:7
            LegacyToRuntimeId[32] = 9; //minecraft:grass:0
            LegacyToRuntimeId[33] = 10; //minecraft:grass:1
            LegacyToRuntimeId[34] = 11; //minecraft:grass:2
            LegacyToRuntimeId[35] = 12; //minecraft:grass:3
            LegacyToRuntimeId[36] = 13; //minecraft:grass:4
            LegacyToRuntimeId[37] = 14; //minecraft:grass:5
            LegacyToRuntimeId[38] = 15; //minecraft:grass:6
            LegacyToRuntimeId[39] = 16; //minecraft:grass:7
            LegacyToRuntimeId[40] = 17; //minecraft:grass:8
            LegacyToRuntimeId[41] = 18; //minecraft:grass:9
            LegacyToRuntimeId[42] = 19; //minecraft:grass:10
            LegacyToRuntimeId[43] = 20; //minecraft:grass:11
            LegacyToRuntimeId[44] = 21; //minecraft:grass:12
            LegacyToRuntimeId[45] = 22; //minecraft:grass:13
            LegacyToRuntimeId[46] = 23; //minecraft:grass:14
            LegacyToRuntimeId[47] = 24; //minecraft:grass:15
            LegacyToRuntimeId[48] = 25; //minecraft:dirt:0
            LegacyToRuntimeId[49] = 26; //minecraft:dirt:1
            LegacyToRuntimeId[64] = 27; //minecraft:cobblestone:0
            LegacyToRuntimeId[80] = 28; //minecraft:planks:0
            LegacyToRuntimeId[81] = 29; //minecraft:planks:1
            LegacyToRuntimeId[82] = 30; //minecraft:planks:2
            LegacyToRuntimeId[83] = 31; //minecraft:planks:3
            LegacyToRuntimeId[84] = 32; //minecraft:planks:4
            LegacyToRuntimeId[85] = 33; //minecraft:planks:5
            LegacyToRuntimeId[86] = 34; //minecraft:planks:6
            LegacyToRuntimeId[87] = 35; //minecraft:planks:7
            LegacyToRuntimeId[96] = 36; //minecraft:sapling:0
            LegacyToRuntimeId[97] = 37; //minecraft:sapling:1
            LegacyToRuntimeId[98] = 38; //minecraft:sapling:2
            LegacyToRuntimeId[99] = 39; //minecraft:sapling:3
            LegacyToRuntimeId[100] = 40; //minecraft:sapling:4
            LegacyToRuntimeId[101] = 41; //minecraft:sapling:5
            LegacyToRuntimeId[102] = 42; //minecraft:sapling:6
            LegacyToRuntimeId[103] = 43; //minecraft:sapling:7
            LegacyToRuntimeId[104] = 44; //minecraft:sapling:8
            LegacyToRuntimeId[105] = 45; //minecraft:sapling:9
            LegacyToRuntimeId[106] = 46; //minecraft:sapling:10
            LegacyToRuntimeId[107] = 47; //minecraft:sapling:11
            LegacyToRuntimeId[108] = 48; //minecraft:sapling:12
            LegacyToRuntimeId[109] = 49; //minecraft:sapling:13
            LegacyToRuntimeId[110] = 50; //minecraft:sapling:14
            LegacyToRuntimeId[111] = 51; //minecraft:sapling:15
            LegacyToRuntimeId[112] = 52; //minecraft:bedrock:0
            LegacyToRuntimeId[113] = 53; //minecraft:bedrock:1
            LegacyToRuntimeId[128] = 54; //minecraft:flowing_water:0
            LegacyToRuntimeId[129] = 55; //minecraft:flowing_water:1
            LegacyToRuntimeId[130] = 56; //minecraft:flowing_water:2
            LegacyToRuntimeId[131] = 57; //minecraft:flowing_water:3
            LegacyToRuntimeId[132] = 58; //minecraft:flowing_water:4
            LegacyToRuntimeId[133] = 59; //minecraft:flowing_water:5
            LegacyToRuntimeId[134] = 60; //minecraft:flowing_water:6
            LegacyToRuntimeId[135] = 61; //minecraft:flowing_water:7
            LegacyToRuntimeId[136] = 62; //minecraft:flowing_water:8
            LegacyToRuntimeId[137] = 63; //minecraft:flowing_water:9
            LegacyToRuntimeId[138] = 64; //minecraft:flowing_water:10
            LegacyToRuntimeId[139] = 65; //minecraft:flowing_water:11
            LegacyToRuntimeId[140] = 66; //minecraft:flowing_water:12
            LegacyToRuntimeId[141] = 67; //minecraft:flowing_water:13
            LegacyToRuntimeId[142] = 68; //minecraft:flowing_water:14
            LegacyToRuntimeId[143] = 69; //minecraft:flowing_water:15
            LegacyToRuntimeId[144] = 70; //minecraft:water:0
            LegacyToRuntimeId[145] = 71; //minecraft:water:1
            LegacyToRuntimeId[146] = 72; //minecraft:water:2
            LegacyToRuntimeId[147] = 73; //minecraft:water:3
            LegacyToRuntimeId[148] = 74; //minecraft:water:4
            LegacyToRuntimeId[149] = 75; //minecraft:water:5
            LegacyToRuntimeId[150] = 76; //minecraft:water:6
            LegacyToRuntimeId[151] = 77; //minecraft:water:7
            LegacyToRuntimeId[152] = 78; //minecraft:water:8
            LegacyToRuntimeId[153] = 79; //minecraft:water:9
            LegacyToRuntimeId[154] = 80; //minecraft:water:10
            LegacyToRuntimeId[155] = 81; //minecraft:water:11
            LegacyToRuntimeId[156] = 82; //minecraft:water:12
            LegacyToRuntimeId[157] = 83; //minecraft:water:13
            LegacyToRuntimeId[158] = 84; //minecraft:water:14
            LegacyToRuntimeId[159] = 85; //minecraft:water:15
            LegacyToRuntimeId[160] = 86; //minecraft:flowing_lava:0
            LegacyToRuntimeId[161] = 87; //minecraft:flowing_lava:1
            LegacyToRuntimeId[162] = 88; //minecraft:flowing_lava:2
            LegacyToRuntimeId[163] = 89; //minecraft:flowing_lava:3
            LegacyToRuntimeId[164] = 90; //minecraft:flowing_lava:4
            LegacyToRuntimeId[165] = 91; //minecraft:flowing_lava:5
            LegacyToRuntimeId[166] = 92; //minecraft:flowing_lava:6
            LegacyToRuntimeId[167] = 93; //minecraft:flowing_lava:7
            LegacyToRuntimeId[168] = 94; //minecraft:flowing_lava:8
            LegacyToRuntimeId[169] = 95; //minecraft:flowing_lava:9
            LegacyToRuntimeId[170] = 96; //minecraft:flowing_lava:10
            LegacyToRuntimeId[171] = 97; //minecraft:flowing_lava:11
            LegacyToRuntimeId[172] = 98; //minecraft:flowing_lava:12
            LegacyToRuntimeId[173] = 99; //minecraft:flowing_lava:13
            LegacyToRuntimeId[174] = 100; //minecraft:flowing_lava:14
            LegacyToRuntimeId[175] = 101; //minecraft:flowing_lava:15
            LegacyToRuntimeId[176] = 102; //minecraft:lava:0
            LegacyToRuntimeId[177] = 103; //minecraft:lava:1
            LegacyToRuntimeId[178] = 104; //minecraft:lava:2
            LegacyToRuntimeId[179] = 105; //minecraft:lava:3
            LegacyToRuntimeId[180] = 106; //minecraft:lava:4
            LegacyToRuntimeId[181] = 107; //minecraft:lava:5
            LegacyToRuntimeId[182] = 108; //minecraft:lava:6
            LegacyToRuntimeId[183] = 109; //minecraft:lava:7
            LegacyToRuntimeId[184] = 110; //minecraft:lava:8
            LegacyToRuntimeId[185] = 111; //minecraft:lava:9
            LegacyToRuntimeId[186] = 112; //minecraft:lava:10
            LegacyToRuntimeId[187] = 113; //minecraft:lava:11
            LegacyToRuntimeId[188] = 114; //minecraft:lava:12
            LegacyToRuntimeId[189] = 115; //minecraft:lava:13
            LegacyToRuntimeId[190] = 116; //minecraft:lava:14
            LegacyToRuntimeId[191] = 117; //minecraft:lava:15
            LegacyToRuntimeId[192] = 118; //minecraft:sand:0
            LegacyToRuntimeId[193] = 119; //minecraft:sand:1
            LegacyToRuntimeId[208] = 120; //minecraft:gravel:0
            LegacyToRuntimeId[224] = 121; //minecraft:gold_ore:0
            LegacyToRuntimeId[240] = 122; //minecraft:iron_ore:0
            LegacyToRuntimeId[256] = 123; //minecraft:coal_ore:0
            LegacyToRuntimeId[272] = 124; //minecraft:log:0
            LegacyToRuntimeId[273] = 125; //minecraft:log:1
            LegacyToRuntimeId[274] = 126; //minecraft:log:2
            LegacyToRuntimeId[275] = 127; //minecraft:log:3
            LegacyToRuntimeId[276] = 128; //minecraft:log:4
            LegacyToRuntimeId[277] = 129; //minecraft:log:5
            LegacyToRuntimeId[278] = 130; //minecraft:log:6
            LegacyToRuntimeId[279] = 131; //minecraft:log:7
            LegacyToRuntimeId[280] = 132; //minecraft:log:8
            LegacyToRuntimeId[281] = 133; //minecraft:log:9
            LegacyToRuntimeId[282] = 134; //minecraft:log:10
            LegacyToRuntimeId[283] = 135; //minecraft:log:11
            LegacyToRuntimeId[284] = 136; //minecraft:log:12
            LegacyToRuntimeId[285] = 137; //minecraft:log:13
            LegacyToRuntimeId[286] = 138; //minecraft:log:14
            LegacyToRuntimeId[287] = 139; //minecraft:log:15
            LegacyToRuntimeId[288] = 140; //minecraft:leaves:0
            LegacyToRuntimeId[289] = 141; //minecraft:leaves:1
            LegacyToRuntimeId[290] = 142; //minecraft:leaves:2
            LegacyToRuntimeId[291] = 143; //minecraft:leaves:3
            LegacyToRuntimeId[292] = 144; //minecraft:leaves:4
            LegacyToRuntimeId[293] = 145; //minecraft:leaves:5
            LegacyToRuntimeId[294] = 146; //minecraft:leaves:6
            LegacyToRuntimeId[295] = 147; //minecraft:leaves:7
            LegacyToRuntimeId[296] = 148; //minecraft:leaves:8
            LegacyToRuntimeId[297] = 149; //minecraft:leaves:9
            LegacyToRuntimeId[298] = 150; //minecraft:leaves:10
            LegacyToRuntimeId[299] = 151; //minecraft:leaves:11
            LegacyToRuntimeId[300] = 152; //minecraft:leaves:12
            LegacyToRuntimeId[301] = 153; //minecraft:leaves:13
            LegacyToRuntimeId[302] = 154; //minecraft:leaves:14
            LegacyToRuntimeId[303] = 155; //minecraft:leaves:15
            LegacyToRuntimeId[304] = 156; //minecraft:sponge:0
            LegacyToRuntimeId[305] = 157; //minecraft:sponge:1
            LegacyToRuntimeId[320] = 158; //minecraft:glass:0
            LegacyToRuntimeId[336] = 159; //minecraft:lapis_ore:0
            LegacyToRuntimeId[352] = 160; //minecraft:lapis_block:0
            LegacyToRuntimeId[368] = 161; //minecraft:dispenser:0
            LegacyToRuntimeId[369] = 162; //minecraft:dispenser:1
            LegacyToRuntimeId[370] = 163; //minecraft:dispenser:2
            LegacyToRuntimeId[371] = 164; //minecraft:dispenser:3
            LegacyToRuntimeId[372] = 165; //minecraft:dispenser:4
            LegacyToRuntimeId[373] = 166; //minecraft:dispenser:5
            LegacyToRuntimeId[374] = 167; //minecraft:dispenser:6
            LegacyToRuntimeId[375] = 168; //minecraft:dispenser:7
            LegacyToRuntimeId[376] = 169; //minecraft:dispenser:8
            LegacyToRuntimeId[377] = 170; //minecraft:dispenser:9
            LegacyToRuntimeId[378] = 171; //minecraft:dispenser:10
            LegacyToRuntimeId[379] = 172; //minecraft:dispenser:11
            LegacyToRuntimeId[380] = 173; //minecraft:dispenser:12
            LegacyToRuntimeId[381] = 174; //minecraft:dispenser:13
            LegacyToRuntimeId[382] = 175; //minecraft:dispenser:14
            LegacyToRuntimeId[383] = 176; //minecraft:dispenser:15
            LegacyToRuntimeId[384] = 177; //minecraft:sandstone:0
            LegacyToRuntimeId[385] = 178; //minecraft:sandstone:1
            LegacyToRuntimeId[386] = 179; //minecraft:sandstone:2
            LegacyToRuntimeId[387] = 180; //minecraft:sandstone:3
            LegacyToRuntimeId[400] = 181; //minecraft:noteblock:0
            LegacyToRuntimeId[416] = 182; //minecraft:bed:0
            LegacyToRuntimeId[417] = 183; //minecraft:bed:1
            LegacyToRuntimeId[418] = 184; //minecraft:bed:2
            LegacyToRuntimeId[419] = 185; //minecraft:bed:3
            LegacyToRuntimeId[420] = 186; //minecraft:bed:4
            LegacyToRuntimeId[421] = 187; //minecraft:bed:5
            LegacyToRuntimeId[422] = 188; //minecraft:bed:6
            LegacyToRuntimeId[423] = 189; //minecraft:bed:7
            LegacyToRuntimeId[424] = 190; //minecraft:bed:8
            LegacyToRuntimeId[425] = 191; //minecraft:bed:9
            LegacyToRuntimeId[426] = 192; //minecraft:bed:10
            LegacyToRuntimeId[427] = 193; //minecraft:bed:11
            LegacyToRuntimeId[428] = 194; //minecraft:bed:12
            LegacyToRuntimeId[429] = 195; //minecraft:bed:13
            LegacyToRuntimeId[430] = 196; //minecraft:bed:14
            LegacyToRuntimeId[431] = 197; //minecraft:bed:15
            LegacyToRuntimeId[432] = 198; //minecraft:golden_rail:0
            LegacyToRuntimeId[433] = 199; //minecraft:golden_rail:1
            LegacyToRuntimeId[434] = 200; //minecraft:golden_rail:2
            LegacyToRuntimeId[435] = 201; //minecraft:golden_rail:3
            LegacyToRuntimeId[436] = 202; //minecraft:golden_rail:4
            LegacyToRuntimeId[437] = 203; //minecraft:golden_rail:5
            LegacyToRuntimeId[438] = 204; //minecraft:golden_rail:6
            LegacyToRuntimeId[439] = 205; //minecraft:golden_rail:7
            LegacyToRuntimeId[440] = 206; //minecraft:golden_rail:8
            LegacyToRuntimeId[441] = 207; //minecraft:golden_rail:9
            LegacyToRuntimeId[442] = 208; //minecraft:golden_rail:10
            LegacyToRuntimeId[443] = 209; //minecraft:golden_rail:11
            LegacyToRuntimeId[444] = 210; //minecraft:golden_rail:12
            LegacyToRuntimeId[445] = 211; //minecraft:golden_rail:13
            LegacyToRuntimeId[446] = 212; //minecraft:golden_rail:14
            LegacyToRuntimeId[447] = 213; //minecraft:golden_rail:15
            LegacyToRuntimeId[448] = 214; //minecraft:detector_rail:0
            LegacyToRuntimeId[449] = 215; //minecraft:detector_rail:1
            LegacyToRuntimeId[450] = 216; //minecraft:detector_rail:2
            LegacyToRuntimeId[451] = 217; //minecraft:detector_rail:3
            LegacyToRuntimeId[452] = 218; //minecraft:detector_rail:4
            LegacyToRuntimeId[453] = 219; //minecraft:detector_rail:5
            LegacyToRuntimeId[454] = 220; //minecraft:detector_rail:6
            LegacyToRuntimeId[455] = 221; //minecraft:detector_rail:7
            LegacyToRuntimeId[456] = 222; //minecraft:detector_rail:8
            LegacyToRuntimeId[457] = 223; //minecraft:detector_rail:9
            LegacyToRuntimeId[458] = 224; //minecraft:detector_rail:10
            LegacyToRuntimeId[459] = 225; //minecraft:detector_rail:11
            LegacyToRuntimeId[460] = 226; //minecraft:detector_rail:12
            LegacyToRuntimeId[461] = 227; //minecraft:detector_rail:13
            LegacyToRuntimeId[462] = 228; //minecraft:detector_rail:14
            LegacyToRuntimeId[463] = 229; //minecraft:detector_rail:15
            LegacyToRuntimeId[464] = 230; //minecraft:sticky_piston:0
            LegacyToRuntimeId[465] = 231; //minecraft:sticky_piston:1
            LegacyToRuntimeId[466] = 232; //minecraft:sticky_piston:2
            LegacyToRuntimeId[467] = 233; //minecraft:sticky_piston:3
            LegacyToRuntimeId[468] = 234; //minecraft:sticky_piston:4
            LegacyToRuntimeId[469] = 235; //minecraft:sticky_piston:5
            LegacyToRuntimeId[470] = 236; //minecraft:sticky_piston:6
            LegacyToRuntimeId[471] = 237; //minecraft:sticky_piston:7
            LegacyToRuntimeId[480] = 238; //minecraft:web:0
            LegacyToRuntimeId[496] = 239; //minecraft:tallgrass:0
            LegacyToRuntimeId[497] = 240; //minecraft:tallgrass:1
            LegacyToRuntimeId[498] = 241; //minecraft:tallgrass:2
            LegacyToRuntimeId[499] = 242; //minecraft:tallgrass:3
            LegacyToRuntimeId[512] = 243; //minecraft:deadbush:0
            LegacyToRuntimeId[528] = 244; //minecraft:piston:0
            LegacyToRuntimeId[529] = 245; //minecraft:piston:1
            LegacyToRuntimeId[530] = 246; //minecraft:piston:2
            LegacyToRuntimeId[531] = 247; //minecraft:piston:3
            LegacyToRuntimeId[532] = 248; //minecraft:piston:4
            LegacyToRuntimeId[533] = 249; //minecraft:piston:5
            LegacyToRuntimeId[534] = 250; //minecraft:piston:6
            LegacyToRuntimeId[535] = 251; //minecraft:piston:7
            LegacyToRuntimeId[544] = 252; //minecraft:pistonArmCollision:0
            LegacyToRuntimeId[545] = 253; //minecraft:pistonArmCollision:1
            LegacyToRuntimeId[546] = 254; //minecraft:pistonArmCollision:2
            LegacyToRuntimeId[547] = 255; //minecraft:pistonArmCollision:3
            LegacyToRuntimeId[548] = 256; //minecraft:pistonArmCollision:4
            LegacyToRuntimeId[549] = 257; //minecraft:pistonArmCollision:5
            LegacyToRuntimeId[550] = 258; //minecraft:pistonArmCollision:6
            LegacyToRuntimeId[551] = 259; //minecraft:pistonArmCollision:7
            LegacyToRuntimeId[560] = 260; //minecraft:wool:0
            LegacyToRuntimeId[561] = 261; //minecraft:wool:1
            LegacyToRuntimeId[562] = 262; //minecraft:wool:2
            LegacyToRuntimeId[563] = 263; //minecraft:wool:3
            LegacyToRuntimeId[564] = 264; //minecraft:wool:4
            LegacyToRuntimeId[565] = 265; //minecraft:wool:5
            LegacyToRuntimeId[566] = 266; //minecraft:wool:6
            LegacyToRuntimeId[567] = 267; //minecraft:wool:7
            LegacyToRuntimeId[568] = 268; //minecraft:wool:8
            LegacyToRuntimeId[569] = 269; //minecraft:wool:9
            LegacyToRuntimeId[570] = 270; //minecraft:wool:10
            LegacyToRuntimeId[571] = 271; //minecraft:wool:11
            LegacyToRuntimeId[572] = 272; //minecraft:wool:12
            LegacyToRuntimeId[573] = 273; //minecraft:wool:13
            LegacyToRuntimeId[574] = 274; //minecraft:wool:14
            LegacyToRuntimeId[575] = 275; //minecraft:wool:15
            LegacyToRuntimeId[592] = 277; //minecraft:yellow_flower:0
            LegacyToRuntimeId[593] = 278; //minecraft:yellow_flower:1
            LegacyToRuntimeId[594] = 279; //minecraft:yellow_flower:2
            LegacyToRuntimeId[595] = 280; //minecraft:yellow_flower:3
            LegacyToRuntimeId[596] = 281; //minecraft:yellow_flower:4
            LegacyToRuntimeId[597] = 282; //minecraft:yellow_flower:5
            LegacyToRuntimeId[598] = 283; //minecraft:yellow_flower:6
            LegacyToRuntimeId[599] = 284; //minecraft:yellow_flower:7
            LegacyToRuntimeId[600] = 285; //minecraft:yellow_flower:8
            LegacyToRuntimeId[601] = 286; //minecraft:yellow_flower:9
            LegacyToRuntimeId[602] = 287; //minecraft:yellow_flower:10
            LegacyToRuntimeId[603] = 288; //minecraft:yellow_flower:11
            LegacyToRuntimeId[604] = 289; //minecraft:yellow_flower:12
            LegacyToRuntimeId[605] = 290; //minecraft:yellow_flower:13
            LegacyToRuntimeId[606] = 291; //minecraft:yellow_flower:14
            LegacyToRuntimeId[607] = 292; //minecraft:yellow_flower:15
            LegacyToRuntimeId[608] = 293; //minecraft:red_flower:0
            LegacyToRuntimeId[609] = 294; //minecraft:red_flower:1
            LegacyToRuntimeId[610] = 295; //minecraft:red_flower:2
            LegacyToRuntimeId[611] = 296; //minecraft:red_flower:3
            LegacyToRuntimeId[612] = 297; //minecraft:red_flower:4
            LegacyToRuntimeId[613] = 298; //minecraft:red_flower:5
            LegacyToRuntimeId[614] = 299; //minecraft:red_flower:6
            LegacyToRuntimeId[615] = 300; //minecraft:red_flower:7
            LegacyToRuntimeId[616] = 301; //minecraft:red_flower:8
            LegacyToRuntimeId[617] = 302; //minecraft:red_flower:9
            LegacyToRuntimeId[618] = 303; //minecraft:red_flower:10
            LegacyToRuntimeId[619] = 304; //minecraft:red_flower:11
            LegacyToRuntimeId[620] = 305; //minecraft:red_flower:12
            LegacyToRuntimeId[621] = 306; //minecraft:red_flower:13
            LegacyToRuntimeId[622] = 307; //minecraft:red_flower:14
            LegacyToRuntimeId[623] = 308; //minecraft:red_flower:15
            LegacyToRuntimeId[624] = 309; //minecraft:brown_mushroom:0
            LegacyToRuntimeId[640] = 310; //minecraft:red_mushroom:0
            LegacyToRuntimeId[656] = 311; //minecraft:gold_block:0
            LegacyToRuntimeId[672] = 312; //minecraft:iron_block:0
            LegacyToRuntimeId[688] = 313; //minecraft:double_stone_slab:0
            LegacyToRuntimeId[689] = 314; //minecraft:double_stone_slab:1
            LegacyToRuntimeId[690] = 315; //minecraft:double_stone_slab:2
            LegacyToRuntimeId[691] = 316; //minecraft:double_stone_slab:3
            LegacyToRuntimeId[692] = 317; //minecraft:double_stone_slab:4
            LegacyToRuntimeId[693] = 318; //minecraft:double_stone_slab:5
            LegacyToRuntimeId[694] = 319; //minecraft:double_stone_slab:6
            LegacyToRuntimeId[695] = 320; //minecraft:double_stone_slab:7
            LegacyToRuntimeId[696] = 321; //minecraft:double_stone_slab:8
            LegacyToRuntimeId[697] = 322; //minecraft:double_stone_slab:9
            LegacyToRuntimeId[698] = 323; //minecraft:double_stone_slab:10
            LegacyToRuntimeId[699] = 324; //minecraft:double_stone_slab:11
            LegacyToRuntimeId[700] = 325; //minecraft:double_stone_slab:12
            LegacyToRuntimeId[701] = 326; //minecraft:double_stone_slab:13
            LegacyToRuntimeId[702] = 327; //minecraft:double_stone_slab:14
            LegacyToRuntimeId[703] = 328; //minecraft:double_stone_slab:15
            LegacyToRuntimeId[704] = 329; //minecraft:stone_slab:0
            LegacyToRuntimeId[705] = 330; //minecraft:stone_slab:1
            LegacyToRuntimeId[706] = 331; //minecraft:stone_slab:2
            LegacyToRuntimeId[707] = 332; //minecraft:stone_slab:3
            LegacyToRuntimeId[708] = 333; //minecraft:stone_slab:4
            LegacyToRuntimeId[709] = 334; //minecraft:stone_slab:5
            LegacyToRuntimeId[710] = 335; //minecraft:stone_slab:6
            LegacyToRuntimeId[711] = 336; //minecraft:stone_slab:7
            LegacyToRuntimeId[712] = 337; //minecraft:stone_slab:8
            LegacyToRuntimeId[713] = 338; //minecraft:stone_slab:9
            LegacyToRuntimeId[714] = 339; //minecraft:stone_slab:10
            LegacyToRuntimeId[715] = 340; //minecraft:stone_slab:11
            LegacyToRuntimeId[716] = 341; //minecraft:stone_slab:12
            LegacyToRuntimeId[717] = 342; //minecraft:stone_slab:13
            LegacyToRuntimeId[718] = 343; //minecraft:stone_slab:14
            LegacyToRuntimeId[719] = 344; //minecraft:stone_slab:15
            LegacyToRuntimeId[720] = 345; //minecraft:brick_block:0
            LegacyToRuntimeId[736] = 346; //minecraft:tnt:0
            LegacyToRuntimeId[737] = 347; //minecraft:tnt:1
            LegacyToRuntimeId[752] = 348; //minecraft:bookshelf:0
            LegacyToRuntimeId[768] = 349; //minecraft:mossy_cobblestone:0
            LegacyToRuntimeId[784] = 350; //minecraft:obsidian:0
            LegacyToRuntimeId[800] = 351; //minecraft:torch:0
            LegacyToRuntimeId[801] = 352; //minecraft:torch:1
            LegacyToRuntimeId[802] = 353; //minecraft:torch:2
            LegacyToRuntimeId[803] = 354; //minecraft:torch:3
            LegacyToRuntimeId[804] = 355; //minecraft:torch:4
            LegacyToRuntimeId[805] = 356; //minecraft:torch:5
            LegacyToRuntimeId[806] = 357; //minecraft:torch:6
            LegacyToRuntimeId[807] = 358; //minecraft:torch:7
            LegacyToRuntimeId[816] = 359; //minecraft:fire:0
            LegacyToRuntimeId[817] = 360; //minecraft:fire:1
            LegacyToRuntimeId[818] = 361; //minecraft:fire:2
            LegacyToRuntimeId[819] = 362; //minecraft:fire:3
            LegacyToRuntimeId[820] = 363; //minecraft:fire:4
            LegacyToRuntimeId[821] = 364; //minecraft:fire:5
            LegacyToRuntimeId[822] = 365; //minecraft:fire:6
            LegacyToRuntimeId[823] = 366; //minecraft:fire:7
            LegacyToRuntimeId[824] = 367; //minecraft:fire:8
            LegacyToRuntimeId[825] = 368; //minecraft:fire:9
            LegacyToRuntimeId[826] = 369; //minecraft:fire:10
            LegacyToRuntimeId[827] = 370; //minecraft:fire:11
            LegacyToRuntimeId[828] = 371; //minecraft:fire:12
            LegacyToRuntimeId[829] = 372; //minecraft:fire:13
            LegacyToRuntimeId[830] = 373; //minecraft:fire:14
            LegacyToRuntimeId[831] = 374; //minecraft:fire:15
            LegacyToRuntimeId[832] = 375; //minecraft:mob_spawner:0
            LegacyToRuntimeId[848] = 376; //minecraft:oak_stairs:0
            LegacyToRuntimeId[849] = 377; //minecraft:oak_stairs:1
            LegacyToRuntimeId[850] = 378; //minecraft:oak_stairs:2
            LegacyToRuntimeId[851] = 379; //minecraft:oak_stairs:3
            LegacyToRuntimeId[852] = 380; //minecraft:oak_stairs:4
            LegacyToRuntimeId[853] = 381; //minecraft:oak_stairs:5
            LegacyToRuntimeId[854] = 382; //minecraft:oak_stairs:6
            LegacyToRuntimeId[855] = 383; //minecraft:oak_stairs:7
            LegacyToRuntimeId[864] = 384; //minecraft:chest:0
            LegacyToRuntimeId[865] = 385; //minecraft:chest:1
            LegacyToRuntimeId[866] = 386; //minecraft:chest:2
            LegacyToRuntimeId[867] = 387; //minecraft:chest:3
            LegacyToRuntimeId[868] = 388; //minecraft:chest:4
            LegacyToRuntimeId[869] = 389; //minecraft:chest:5
            LegacyToRuntimeId[870] = 390; //minecraft:chest:6
            LegacyToRuntimeId[871] = 391; //minecraft:chest:7
            LegacyToRuntimeId[880] = 392; //minecraft:redstone_wire:0
            LegacyToRuntimeId[881] = 393; //minecraft:redstone_wire:1
            LegacyToRuntimeId[882] = 394; //minecraft:redstone_wire:2
            LegacyToRuntimeId[883] = 395; //minecraft:redstone_wire:3
            LegacyToRuntimeId[884] = 396; //minecraft:redstone_wire:4
            LegacyToRuntimeId[885] = 397; //minecraft:redstone_wire:5
            LegacyToRuntimeId[886] = 398; //minecraft:redstone_wire:6
            LegacyToRuntimeId[887] = 399; //minecraft:redstone_wire:7
            LegacyToRuntimeId[888] = 400; //minecraft:redstone_wire:8
            LegacyToRuntimeId[889] = 401; //minecraft:redstone_wire:9
            LegacyToRuntimeId[890] = 402; //minecraft:redstone_wire:10
            LegacyToRuntimeId[891] = 403; //minecraft:redstone_wire:11
            LegacyToRuntimeId[892] = 404; //minecraft:redstone_wire:12
            LegacyToRuntimeId[893] = 405; //minecraft:redstone_wire:13
            LegacyToRuntimeId[894] = 406; //minecraft:redstone_wire:14
            LegacyToRuntimeId[895] = 407; //minecraft:redstone_wire:15
            LegacyToRuntimeId[896] = 408; //minecraft:diamond_ore:0
            LegacyToRuntimeId[912] = 409; //minecraft:diamond_block:0
            LegacyToRuntimeId[928] = 410; //minecraft:crafting_table:0
            LegacyToRuntimeId[944] = 411; //minecraft:wheat:0
            LegacyToRuntimeId[945] = 412; //minecraft:wheat:1
            LegacyToRuntimeId[946] = 413; //minecraft:wheat:2
            LegacyToRuntimeId[947] = 414; //minecraft:wheat:3
            LegacyToRuntimeId[948] = 415; //minecraft:wheat:4
            LegacyToRuntimeId[949] = 416; //minecraft:wheat:5
            LegacyToRuntimeId[950] = 417; //minecraft:wheat:6
            LegacyToRuntimeId[951] = 418; //minecraft:wheat:7
            LegacyToRuntimeId[960] = 419; //minecraft:farmland:0
            LegacyToRuntimeId[961] = 420; //minecraft:farmland:1
            LegacyToRuntimeId[962] = 421; //minecraft:farmland:2
            LegacyToRuntimeId[963] = 422; //minecraft:farmland:3
            LegacyToRuntimeId[964] = 423; //minecraft:farmland:4
            LegacyToRuntimeId[965] = 424; //minecraft:farmland:5
            LegacyToRuntimeId[966] = 425; //minecraft:farmland:6
            LegacyToRuntimeId[967] = 426; //minecraft:farmland:7
            LegacyToRuntimeId[976] = 427; //minecraft:furnace:0
            LegacyToRuntimeId[977] = 428; //minecraft:furnace:1
            LegacyToRuntimeId[978] = 429; //minecraft:furnace:2
            LegacyToRuntimeId[979] = 430; //minecraft:furnace:3
            LegacyToRuntimeId[980] = 431; //minecraft:furnace:4
            LegacyToRuntimeId[981] = 432; //minecraft:furnace:5
            LegacyToRuntimeId[982] = 433; //minecraft:furnace:6
            LegacyToRuntimeId[983] = 434; //minecraft:furnace:7
            LegacyToRuntimeId[992] = 435; //minecraft:lit_furnace:0
            LegacyToRuntimeId[993] = 436; //minecraft:lit_furnace:1
            LegacyToRuntimeId[994] = 437; //minecraft:lit_furnace:2
            LegacyToRuntimeId[995] = 438; //minecraft:lit_furnace:3
            LegacyToRuntimeId[996] = 439; //minecraft:lit_furnace:4
            LegacyToRuntimeId[997] = 440; //minecraft:lit_furnace:5
            LegacyToRuntimeId[998] = 441; //minecraft:lit_furnace:6
            LegacyToRuntimeId[999] = 442; //minecraft:lit_furnace:7
            LegacyToRuntimeId[1008] = 443; //minecraft:standing_sign:0
            LegacyToRuntimeId[1009] = 444; //minecraft:standing_sign:1
            LegacyToRuntimeId[1010] = 445; //minecraft:standing_sign:2
            LegacyToRuntimeId[1011] = 446; //minecraft:standing_sign:3
            LegacyToRuntimeId[1012] = 447; //minecraft:standing_sign:4
            LegacyToRuntimeId[1013] = 448; //minecraft:standing_sign:5
            LegacyToRuntimeId[1014] = 449; //minecraft:standing_sign:6
            LegacyToRuntimeId[1015] = 450; //minecraft:standing_sign:7
            LegacyToRuntimeId[1016] = 451; //minecraft:standing_sign:8
            LegacyToRuntimeId[1017] = 452; //minecraft:standing_sign:9
            LegacyToRuntimeId[1018] = 453; //minecraft:standing_sign:10
            LegacyToRuntimeId[1019] = 454; //minecraft:standing_sign:11
            LegacyToRuntimeId[1020] = 455; //minecraft:standing_sign:12
            LegacyToRuntimeId[1021] = 456; //minecraft:standing_sign:13
            LegacyToRuntimeId[1022] = 457; //minecraft:standing_sign:14
            LegacyToRuntimeId[1023] = 458; //minecraft:standing_sign:15
            LegacyToRuntimeId[1024] = 459; //minecraft:wooden_door:0
            LegacyToRuntimeId[1025] = 460; //minecraft:wooden_door:1
            LegacyToRuntimeId[1026] = 461; //minecraft:wooden_door:2
            LegacyToRuntimeId[1027] = 462; //minecraft:wooden_door:3
            LegacyToRuntimeId[1028] = 463; //minecraft:wooden_door:4
            LegacyToRuntimeId[1029] = 464; //minecraft:wooden_door:5
            LegacyToRuntimeId[1030] = 465; //minecraft:wooden_door:6
            LegacyToRuntimeId[1031] = 466; //minecraft:wooden_door:7
            LegacyToRuntimeId[1032] = 467; //minecraft:wooden_door:8
            LegacyToRuntimeId[1033] = 468; //minecraft:wooden_door:9
            LegacyToRuntimeId[1034] = 469; //minecraft:wooden_door:10
            LegacyToRuntimeId[1035] = 470; //minecraft:wooden_door:11
            LegacyToRuntimeId[1036] = 471; //minecraft:wooden_door:12
            LegacyToRuntimeId[1037] = 472; //minecraft:wooden_door:13
            LegacyToRuntimeId[1038] = 473; //minecraft:wooden_door:14
            LegacyToRuntimeId[1039] = 474; //minecraft:wooden_door:15
            LegacyToRuntimeId[1040] = 475; //minecraft:ladder:0
            LegacyToRuntimeId[1041] = 476; //minecraft:ladder:1
            LegacyToRuntimeId[1042] = 477; //minecraft:ladder:2
            LegacyToRuntimeId[1043] = 478; //minecraft:ladder:3
            LegacyToRuntimeId[1044] = 479; //minecraft:ladder:4
            LegacyToRuntimeId[1045] = 480; //minecraft:ladder:5
            LegacyToRuntimeId[1046] = 481; //minecraft:ladder:6
            LegacyToRuntimeId[1047] = 482; //minecraft:ladder:7
            LegacyToRuntimeId[1056] = 483; //minecraft:rail:0
            LegacyToRuntimeId[1057] = 484; //minecraft:rail:1
            LegacyToRuntimeId[1058] = 485; //minecraft:rail:2
            LegacyToRuntimeId[1059] = 486; //minecraft:rail:3
            LegacyToRuntimeId[1060] = 487; //minecraft:rail:4
            LegacyToRuntimeId[1061] = 488; //minecraft:rail:5
            LegacyToRuntimeId[1062] = 489; //minecraft:rail:6
            LegacyToRuntimeId[1063] = 490; //minecraft:rail:7
            LegacyToRuntimeId[1064] = 491; //minecraft:rail:8
            LegacyToRuntimeId[1065] = 492; //minecraft:rail:9
            LegacyToRuntimeId[1066] = 493; //minecraft:rail:10
            LegacyToRuntimeId[1067] = 494; //minecraft:rail:11
            LegacyToRuntimeId[1068] = 495; //minecraft:rail:12
            LegacyToRuntimeId[1069] = 496; //minecraft:rail:13
            LegacyToRuntimeId[1070] = 497; //minecraft:rail:14
            LegacyToRuntimeId[1071] = 498; //minecraft:rail:15
            LegacyToRuntimeId[1072] = 499; //minecraft:stone_stairs:0
            LegacyToRuntimeId[1073] = 500; //minecraft:stone_stairs:1
            LegacyToRuntimeId[1074] = 501; //minecraft:stone_stairs:2
            LegacyToRuntimeId[1075] = 502; //minecraft:stone_stairs:3
            LegacyToRuntimeId[1076] = 503; //minecraft:stone_stairs:4
            LegacyToRuntimeId[1077] = 504; //minecraft:stone_stairs:5
            LegacyToRuntimeId[1078] = 505; //minecraft:stone_stairs:6
            LegacyToRuntimeId[1079] = 506; //minecraft:stone_stairs:7
            LegacyToRuntimeId[1088] = 507; //minecraft:wall_sign:0
            LegacyToRuntimeId[1089] = 508; //minecraft:wall_sign:1
            LegacyToRuntimeId[1090] = 509; //minecraft:wall_sign:2
            LegacyToRuntimeId[1091] = 510; //minecraft:wall_sign:3
            LegacyToRuntimeId[1092] = 511; //minecraft:wall_sign:4
            LegacyToRuntimeId[1093] = 512; //minecraft:wall_sign:5
            LegacyToRuntimeId[1094] = 513; //minecraft:wall_sign:6
            LegacyToRuntimeId[1095] = 514; //minecraft:wall_sign:7
            LegacyToRuntimeId[1096] = 515; //minecraft:wall_sign:8
            LegacyToRuntimeId[1097] = 516; //minecraft:wall_sign:9
            LegacyToRuntimeId[1098] = 517; //minecraft:wall_sign:10
            LegacyToRuntimeId[1099] = 518; //minecraft:wall_sign:11
            LegacyToRuntimeId[1100] = 519; //minecraft:wall_sign:12
            LegacyToRuntimeId[1101] = 520; //minecraft:wall_sign:13
            LegacyToRuntimeId[1102] = 521; //minecraft:wall_sign:14
            LegacyToRuntimeId[1103] = 522; //minecraft:wall_sign:15
            LegacyToRuntimeId[1104] = 523; //minecraft:lever:0
            LegacyToRuntimeId[1105] = 524; //minecraft:lever:1
            LegacyToRuntimeId[1106] = 525; //minecraft:lever:2
            LegacyToRuntimeId[1107] = 526; //minecraft:lever:3
            LegacyToRuntimeId[1108] = 527; //minecraft:lever:4
            LegacyToRuntimeId[1109] = 528; //minecraft:lever:5
            LegacyToRuntimeId[1110] = 529; //minecraft:lever:6
            LegacyToRuntimeId[1111] = 530; //minecraft:lever:7
            LegacyToRuntimeId[1112] = 531; //minecraft:lever:8
            LegacyToRuntimeId[1113] = 532; //minecraft:lever:9
            LegacyToRuntimeId[1114] = 533; //minecraft:lever:10
            LegacyToRuntimeId[1115] = 534; //minecraft:lever:11
            LegacyToRuntimeId[1116] = 535; //minecraft:lever:12
            LegacyToRuntimeId[1117] = 536; //minecraft:lever:13
            LegacyToRuntimeId[1118] = 537; //minecraft:lever:14
            LegacyToRuntimeId[1119] = 538; //minecraft:lever:15
            LegacyToRuntimeId[1120] = 539; //minecraft:stone_pressure_plate:0
            LegacyToRuntimeId[1121] = 540; //minecraft:stone_pressure_plate:1
            LegacyToRuntimeId[1122] = 541; //minecraft:stone_pressure_plate:2
            LegacyToRuntimeId[1123] = 542; //minecraft:stone_pressure_plate:3
            LegacyToRuntimeId[1124] = 543; //minecraft:stone_pressure_plate:4
            LegacyToRuntimeId[1125] = 544; //minecraft:stone_pressure_plate:5
            LegacyToRuntimeId[1126] = 545; //minecraft:stone_pressure_plate:6
            LegacyToRuntimeId[1127] = 546; //minecraft:stone_pressure_plate:7
            LegacyToRuntimeId[1128] = 547; //minecraft:stone_pressure_plate:8
            LegacyToRuntimeId[1129] = 548; //minecraft:stone_pressure_plate:9
            LegacyToRuntimeId[1130] = 549; //minecraft:stone_pressure_plate:10
            LegacyToRuntimeId[1131] = 550; //minecraft:stone_pressure_plate:11
            LegacyToRuntimeId[1132] = 551; //minecraft:stone_pressure_plate:12
            LegacyToRuntimeId[1133] = 552; //minecraft:stone_pressure_plate:13
            LegacyToRuntimeId[1134] = 553; //minecraft:stone_pressure_plate:14
            LegacyToRuntimeId[1135] = 554; //minecraft:stone_pressure_plate:15
            LegacyToRuntimeId[1136] = 555; //minecraft:iron_door:0
            LegacyToRuntimeId[1137] = 556; //minecraft:iron_door:1
            LegacyToRuntimeId[1138] = 557; //minecraft:iron_door:2
            LegacyToRuntimeId[1139] = 558; //minecraft:iron_door:3
            LegacyToRuntimeId[1140] = 559; //minecraft:iron_door:4
            LegacyToRuntimeId[1141] = 560; //minecraft:iron_door:5
            LegacyToRuntimeId[1142] = 561; //minecraft:iron_door:6
            LegacyToRuntimeId[1143] = 562; //minecraft:iron_door:7
            LegacyToRuntimeId[1144] = 563; //minecraft:iron_door:8
            LegacyToRuntimeId[1145] = 564; //minecraft:iron_door:9
            LegacyToRuntimeId[1146] = 565; //minecraft:iron_door:10
            LegacyToRuntimeId[1147] = 566; //minecraft:iron_door:11
            LegacyToRuntimeId[1148] = 567; //minecraft:iron_door:12
            LegacyToRuntimeId[1149] = 568; //minecraft:iron_door:13
            LegacyToRuntimeId[1150] = 569; //minecraft:iron_door:14
            LegacyToRuntimeId[1151] = 570; //minecraft:iron_door:15
            LegacyToRuntimeId[1152] = 571; //minecraft:wooden_pressure_plate:0
            LegacyToRuntimeId[1153] = 572; //minecraft:wooden_pressure_plate:1
            LegacyToRuntimeId[1154] = 573; //minecraft:wooden_pressure_plate:2
            LegacyToRuntimeId[1155] = 574; //minecraft:wooden_pressure_plate:3
            LegacyToRuntimeId[1156] = 575; //minecraft:wooden_pressure_plate:4
            LegacyToRuntimeId[1157] = 576; //minecraft:wooden_pressure_plate:5
            LegacyToRuntimeId[1158] = 577; //minecraft:wooden_pressure_plate:6
            LegacyToRuntimeId[1159] = 578; //minecraft:wooden_pressure_plate:7
            LegacyToRuntimeId[1160] = 579; //minecraft:wooden_pressure_plate:8
            LegacyToRuntimeId[1161] = 580; //minecraft:wooden_pressure_plate:9
            LegacyToRuntimeId[1162] = 581; //minecraft:wooden_pressure_plate:10
            LegacyToRuntimeId[1163] = 582; //minecraft:wooden_pressure_plate:11
            LegacyToRuntimeId[1164] = 583; //minecraft:wooden_pressure_plate:12
            LegacyToRuntimeId[1165] = 584; //minecraft:wooden_pressure_plate:13
            LegacyToRuntimeId[1166] = 585; //minecraft:wooden_pressure_plate:14
            LegacyToRuntimeId[1167] = 586; //minecraft:wooden_pressure_plate:15
            LegacyToRuntimeId[1168] = 587; //minecraft:redstone_ore:0
            LegacyToRuntimeId[1184] = 588; //minecraft:lit_redstone_ore:0
            LegacyToRuntimeId[1200] = 589; //minecraft:unlit_redstone_torch:0
            LegacyToRuntimeId[1201] = 590; //minecraft:unlit_redstone_torch:1
            LegacyToRuntimeId[1202] = 591; //minecraft:unlit_redstone_torch:2
            LegacyToRuntimeId[1203] = 592; //minecraft:unlit_redstone_torch:3
            LegacyToRuntimeId[1204] = 593; //minecraft:unlit_redstone_torch:4
            LegacyToRuntimeId[1205] = 594; //minecraft:unlit_redstone_torch:5
            LegacyToRuntimeId[1206] = 595; //minecraft:unlit_redstone_torch:6
            LegacyToRuntimeId[1207] = 596; //minecraft:unlit_redstone_torch:7
            LegacyToRuntimeId[1216] = 597; //minecraft:redstone_torch:0
            LegacyToRuntimeId[1217] = 598; //minecraft:redstone_torch:1
            LegacyToRuntimeId[1218] = 599; //minecraft:redstone_torch:2
            LegacyToRuntimeId[1219] = 600; //minecraft:redstone_torch:3
            LegacyToRuntimeId[1220] = 601; //minecraft:redstone_torch:4
            LegacyToRuntimeId[1221] = 602; //minecraft:redstone_torch:5
            LegacyToRuntimeId[1222] = 603; //minecraft:redstone_torch:6
            LegacyToRuntimeId[1223] = 604; //minecraft:redstone_torch:7
            LegacyToRuntimeId[1232] = 605; //minecraft:stone_button:0
            LegacyToRuntimeId[1233] = 606; //minecraft:stone_button:1
            LegacyToRuntimeId[1234] = 607; //minecraft:stone_button:2
            LegacyToRuntimeId[1235] = 608; //minecraft:stone_button:3
            LegacyToRuntimeId[1236] = 609; //minecraft:stone_button:4
            LegacyToRuntimeId[1237] = 610; //minecraft:stone_button:5
            LegacyToRuntimeId[1238] = 611; //minecraft:stone_button:6
            LegacyToRuntimeId[1239] = 612; //minecraft:stone_button:7
            LegacyToRuntimeId[1240] = 613; //minecraft:stone_button:8
            LegacyToRuntimeId[1241] = 614; //minecraft:stone_button:9
            LegacyToRuntimeId[1242] = 615; //minecraft:stone_button:10
            LegacyToRuntimeId[1243] = 616; //minecraft:stone_button:11
            LegacyToRuntimeId[1244] = 617; //minecraft:stone_button:12
            LegacyToRuntimeId[1245] = 618; //minecraft:stone_button:13
            LegacyToRuntimeId[1246] = 619; //minecraft:stone_button:14
            LegacyToRuntimeId[1247] = 620; //minecraft:stone_button:15
            LegacyToRuntimeId[1248] = 621; //minecraft:snow_layer:0
            LegacyToRuntimeId[1249] = 622; //minecraft:snow_layer:1
            LegacyToRuntimeId[1250] = 623; //minecraft:snow_layer:2
            LegacyToRuntimeId[1251] = 624; //minecraft:snow_layer:3
            LegacyToRuntimeId[1252] = 625; //minecraft:snow_layer:4
            LegacyToRuntimeId[1253] = 626; //minecraft:snow_layer:5
            LegacyToRuntimeId[1254] = 627; //minecraft:snow_layer:6
            LegacyToRuntimeId[1255] = 628; //minecraft:snow_layer:7
            LegacyToRuntimeId[1256] = 629; //minecraft:snow_layer:8
            LegacyToRuntimeId[1257] = 630; //minecraft:snow_layer:9
            LegacyToRuntimeId[1258] = 631; //minecraft:snow_layer:10
            LegacyToRuntimeId[1259] = 632; //minecraft:snow_layer:11
            LegacyToRuntimeId[1260] = 633; //minecraft:snow_layer:12
            LegacyToRuntimeId[1261] = 634; //minecraft:snow_layer:13
            LegacyToRuntimeId[1262] = 635; //minecraft:snow_layer:14
            LegacyToRuntimeId[1263] = 636; //minecraft:snow_layer:15
            LegacyToRuntimeId[1264] = 637; //minecraft:ice:0
            LegacyToRuntimeId[1280] = 638; //minecraft:snow:0
            LegacyToRuntimeId[1296] = 639; //minecraft:cactus:0
            LegacyToRuntimeId[1297] = 640; //minecraft:cactus:1
            LegacyToRuntimeId[1298] = 641; //minecraft:cactus:2
            LegacyToRuntimeId[1299] = 642; //minecraft:cactus:3
            LegacyToRuntimeId[1300] = 643; //minecraft:cactus:4
            LegacyToRuntimeId[1301] = 644; //minecraft:cactus:5
            LegacyToRuntimeId[1302] = 645; //minecraft:cactus:6
            LegacyToRuntimeId[1303] = 646; //minecraft:cactus:7
            LegacyToRuntimeId[1304] = 647; //minecraft:cactus:8
            LegacyToRuntimeId[1305] = 648; //minecraft:cactus:9
            LegacyToRuntimeId[1306] = 649; //minecraft:cactus:10
            LegacyToRuntimeId[1307] = 650; //minecraft:cactus:11
            LegacyToRuntimeId[1308] = 651; //minecraft:cactus:12
            LegacyToRuntimeId[1309] = 652; //minecraft:cactus:13
            LegacyToRuntimeId[1310] = 653; //minecraft:cactus:14
            LegacyToRuntimeId[1311] = 654; //minecraft:cactus:15
            LegacyToRuntimeId[1312] = 655; //minecraft:clay:0
            LegacyToRuntimeId[1328] = 656; //minecraft:reeds:0
            LegacyToRuntimeId[1329] = 657; //minecraft:reeds:1
            LegacyToRuntimeId[1330] = 658; //minecraft:reeds:2
            LegacyToRuntimeId[1331] = 659; //minecraft:reeds:3
            LegacyToRuntimeId[1332] = 660; //minecraft:reeds:4
            LegacyToRuntimeId[1333] = 661; //minecraft:reeds:5
            LegacyToRuntimeId[1334] = 662; //minecraft:reeds:6
            LegacyToRuntimeId[1335] = 663; //minecraft:reeds:7
            LegacyToRuntimeId[1336] = 664; //minecraft:reeds:8
            LegacyToRuntimeId[1337] = 665; //minecraft:reeds:9
            LegacyToRuntimeId[1338] = 666; //minecraft:reeds:10
            LegacyToRuntimeId[1339] = 667; //minecraft:reeds:11
            LegacyToRuntimeId[1340] = 668; //minecraft:reeds:12
            LegacyToRuntimeId[1341] = 669; //minecraft:reeds:13
            LegacyToRuntimeId[1342] = 670; //minecraft:reeds:14
            LegacyToRuntimeId[1343] = 671; //minecraft:reeds:15
            LegacyToRuntimeId[1344] = 672; //minecraft:jukebox:0
            LegacyToRuntimeId[1360] = 673; //minecraft:fence:0
            LegacyToRuntimeId[1361] = 674; //minecraft:fence:1
            LegacyToRuntimeId[1362] = 675; //minecraft:fence:2
            LegacyToRuntimeId[1363] = 676; //minecraft:fence:3
            LegacyToRuntimeId[1364] = 677; //minecraft:fence:4
            LegacyToRuntimeId[1365] = 678; //minecraft:fence:5
            LegacyToRuntimeId[1366] = 679; //minecraft:fence:6
            LegacyToRuntimeId[1367] = 680; //minecraft:fence:7
            LegacyToRuntimeId[1376] = 681; //minecraft:pumpkin:0
            LegacyToRuntimeId[1377] = 682; //minecraft:pumpkin:1
            LegacyToRuntimeId[1378] = 683; //minecraft:pumpkin:2
            LegacyToRuntimeId[1379] = 684; //minecraft:pumpkin:3
            LegacyToRuntimeId[1392] = 685; //minecraft:netherrack:0
            LegacyToRuntimeId[1408] = 686; //minecraft:soul_sand:0
            LegacyToRuntimeId[1424] = 687; //minecraft:glowstone:0
            LegacyToRuntimeId[1440] = 688; //minecraft:portal:0
            LegacyToRuntimeId[1441] = 689; //minecraft:portal:1
            LegacyToRuntimeId[1442] = 690; //minecraft:portal:2
            LegacyToRuntimeId[1443] = 691; //minecraft:portal:3
            LegacyToRuntimeId[1456] = 692; //minecraft:lit_pumpkin:0
            LegacyToRuntimeId[1457] = 693; //minecraft:lit_pumpkin:1
            LegacyToRuntimeId[1458] = 694; //minecraft:lit_pumpkin:2
            LegacyToRuntimeId[1459] = 695; //minecraft:lit_pumpkin:3
            LegacyToRuntimeId[1472] = 696; //minecraft:cake:0
            LegacyToRuntimeId[1473] = 697; //minecraft:cake:1
            LegacyToRuntimeId[1474] = 698; //minecraft:cake:2
            LegacyToRuntimeId[1475] = 699; //minecraft:cake:3
            LegacyToRuntimeId[1476] = 700; //minecraft:cake:4
            LegacyToRuntimeId[1477] = 701; //minecraft:cake:5
            LegacyToRuntimeId[1478] = 702; //minecraft:cake:6
            LegacyToRuntimeId[1479] = 703; //minecraft:cake:7
            LegacyToRuntimeId[1488] = 704; //minecraft:unpowered_repeater:0
            LegacyToRuntimeId[1489] = 705; //minecraft:unpowered_repeater:1
            LegacyToRuntimeId[1490] = 706; //minecraft:unpowered_repeater:2
            LegacyToRuntimeId[1491] = 707; //minecraft:unpowered_repeater:3
            LegacyToRuntimeId[1492] = 708; //minecraft:unpowered_repeater:4
            LegacyToRuntimeId[1493] = 709; //minecraft:unpowered_repeater:5
            LegacyToRuntimeId[1494] = 710; //minecraft:unpowered_repeater:6
            LegacyToRuntimeId[1495] = 711; //minecraft:unpowered_repeater:7
            LegacyToRuntimeId[1496] = 712; //minecraft:unpowered_repeater:8
            LegacyToRuntimeId[1497] = 713; //minecraft:unpowered_repeater:9
            LegacyToRuntimeId[1498] = 714; //minecraft:unpowered_repeater:10
            LegacyToRuntimeId[1499] = 715; //minecraft:unpowered_repeater:11
            LegacyToRuntimeId[1500] = 716; //minecraft:unpowered_repeater:12
            LegacyToRuntimeId[1501] = 717; //minecraft:unpowered_repeater:13
            LegacyToRuntimeId[1502] = 718; //minecraft:unpowered_repeater:14
            LegacyToRuntimeId[1503] = 719; //minecraft:unpowered_repeater:15
            LegacyToRuntimeId[1504] = 720; //minecraft:powered_repeater:0
            LegacyToRuntimeId[1505] = 721; //minecraft:powered_repeater:1
            LegacyToRuntimeId[1506] = 722; //minecraft:powered_repeater:2
            LegacyToRuntimeId[1507] = 723; //minecraft:powered_repeater:3
            LegacyToRuntimeId[1508] = 724; //minecraft:powered_repeater:4
            LegacyToRuntimeId[1509] = 725; //minecraft:powered_repeater:5
            LegacyToRuntimeId[1510] = 726; //minecraft:powered_repeater:6
            LegacyToRuntimeId[1511] = 727; //minecraft:powered_repeater:7
            LegacyToRuntimeId[1512] = 728; //minecraft:powered_repeater:8
            LegacyToRuntimeId[1513] = 729; //minecraft:powered_repeater:9
            LegacyToRuntimeId[1514] = 730; //minecraft:powered_repeater:10
            LegacyToRuntimeId[1515] = 731; //minecraft:powered_repeater:11
            LegacyToRuntimeId[1516] = 732; //minecraft:powered_repeater:12
            LegacyToRuntimeId[1517] = 733; //minecraft:powered_repeater:13
            LegacyToRuntimeId[1518] = 734; //minecraft:powered_repeater:14
            LegacyToRuntimeId[1519] = 735; //minecraft:powered_repeater:15
            LegacyToRuntimeId[1520] = 736; //minecraft:invisibleBedrock:0
            LegacyToRuntimeId[1536] = 737; //minecraft:trapdoor:0
            LegacyToRuntimeId[1537] = 738; //minecraft:trapdoor:1
            LegacyToRuntimeId[1538] = 739; //minecraft:trapdoor:2
            LegacyToRuntimeId[1539] = 740; //minecraft:trapdoor:3
            LegacyToRuntimeId[1540] = 741; //minecraft:trapdoor:4
            LegacyToRuntimeId[1541] = 742; //minecraft:trapdoor:5
            LegacyToRuntimeId[1542] = 743; //minecraft:trapdoor:6
            LegacyToRuntimeId[1543] = 744; //minecraft:trapdoor:7
            LegacyToRuntimeId[1544] = 745; //minecraft:trapdoor:8
            LegacyToRuntimeId[1545] = 746; //minecraft:trapdoor:9
            LegacyToRuntimeId[1546] = 747; //minecraft:trapdoor:10
            LegacyToRuntimeId[1547] = 748; //minecraft:trapdoor:11
            LegacyToRuntimeId[1548] = 749; //minecraft:trapdoor:12
            LegacyToRuntimeId[1549] = 750; //minecraft:trapdoor:13
            LegacyToRuntimeId[1550] = 751; //minecraft:trapdoor:14
            LegacyToRuntimeId[1551] = 752; //minecraft:trapdoor:15
            LegacyToRuntimeId[1552] = 753; //minecraft:monster_egg:0
            LegacyToRuntimeId[1553] = 754; //minecraft:monster_egg:1
            LegacyToRuntimeId[1554] = 755; //minecraft:monster_egg:2
            LegacyToRuntimeId[1555] = 756; //minecraft:monster_egg:3
            LegacyToRuntimeId[1556] = 757; //minecraft:monster_egg:4
            LegacyToRuntimeId[1557] = 758; //minecraft:monster_egg:5
            LegacyToRuntimeId[1558] = 759; //minecraft:monster_egg:6
            LegacyToRuntimeId[1559] = 760; //minecraft:monster_egg:7
            LegacyToRuntimeId[1568] = 761; //minecraft:stonebrick:0
            LegacyToRuntimeId[1569] = 762; //minecraft:stonebrick:1
            LegacyToRuntimeId[1570] = 763; //minecraft:stonebrick:2
            LegacyToRuntimeId[1571] = 764; //minecraft:stonebrick:3
            LegacyToRuntimeId[1572] = 765; //minecraft:stonebrick:4
            LegacyToRuntimeId[1573] = 766; //minecraft:stonebrick:5
            LegacyToRuntimeId[1574] = 767; //minecraft:stonebrick:6
            LegacyToRuntimeId[1575] = 768; //minecraft:stonebrick:7
            LegacyToRuntimeId[1584] = 769; //minecraft:brown_mushroom_block:0
            LegacyToRuntimeId[1585] = 770; //minecraft:brown_mushroom_block:1
            LegacyToRuntimeId[1586] = 771; //minecraft:brown_mushroom_block:2
            LegacyToRuntimeId[1587] = 772; //minecraft:brown_mushroom_block:3
            LegacyToRuntimeId[1588] = 773; //minecraft:brown_mushroom_block:4
            LegacyToRuntimeId[1589] = 774; //minecraft:brown_mushroom_block:5
            LegacyToRuntimeId[1590] = 775; //minecraft:brown_mushroom_block:6
            LegacyToRuntimeId[1591] = 776; //minecraft:brown_mushroom_block:7
            LegacyToRuntimeId[1592] = 777; //minecraft:brown_mushroom_block:8
            LegacyToRuntimeId[1593] = 778; //minecraft:brown_mushroom_block:9
            LegacyToRuntimeId[1594] = 779; //minecraft:brown_mushroom_block:10
            LegacyToRuntimeId[1595] = 780; //minecraft:brown_mushroom_block:11
            LegacyToRuntimeId[1596] = 781; //minecraft:brown_mushroom_block:12
            LegacyToRuntimeId[1597] = 782; //minecraft:brown_mushroom_block:13
            LegacyToRuntimeId[1598] = 783; //minecraft:brown_mushroom_block:14
            LegacyToRuntimeId[1599] = 784; //minecraft:brown_mushroom_block:15
            LegacyToRuntimeId[1600] = 785; //minecraft:red_mushroom_block:0
            LegacyToRuntimeId[1601] = 786; //minecraft:red_mushroom_block:1
            LegacyToRuntimeId[1602] = 787; //minecraft:red_mushroom_block:2
            LegacyToRuntimeId[1603] = 788; //minecraft:red_mushroom_block:3
            LegacyToRuntimeId[1604] = 789; //minecraft:red_mushroom_block:4
            LegacyToRuntimeId[1605] = 790; //minecraft:red_mushroom_block:5
            LegacyToRuntimeId[1606] = 791; //minecraft:red_mushroom_block:6
            LegacyToRuntimeId[1607] = 792; //minecraft:red_mushroom_block:7
            LegacyToRuntimeId[1608] = 793; //minecraft:red_mushroom_block:8
            LegacyToRuntimeId[1609] = 794; //minecraft:red_mushroom_block:9
            LegacyToRuntimeId[1610] = 795; //minecraft:red_mushroom_block:10
            LegacyToRuntimeId[1611] = 796; //minecraft:red_mushroom_block:11
            LegacyToRuntimeId[1612] = 797; //minecraft:red_mushroom_block:12
            LegacyToRuntimeId[1613] = 798; //minecraft:red_mushroom_block:13
            LegacyToRuntimeId[1614] = 799; //minecraft:red_mushroom_block:14
            LegacyToRuntimeId[1615] = 800; //minecraft:red_mushroom_block:15
            LegacyToRuntimeId[1616] = 801; //minecraft:iron_bars:0
            LegacyToRuntimeId[1632] = 802; //minecraft:glass_pane:0
            LegacyToRuntimeId[1648] = 803; //minecraft:melon_block:0
            LegacyToRuntimeId[1664] = 804; //minecraft:pumpkin_stem:0
            LegacyToRuntimeId[1665] = 805; //minecraft:pumpkin_stem:1
            LegacyToRuntimeId[1666] = 806; //minecraft:pumpkin_stem:2
            LegacyToRuntimeId[1667] = 807; //minecraft:pumpkin_stem:3
            LegacyToRuntimeId[1668] = 808; //minecraft:pumpkin_stem:4
            LegacyToRuntimeId[1669] = 809; //minecraft:pumpkin_stem:5
            LegacyToRuntimeId[1670] = 810; //minecraft:pumpkin_stem:6
            LegacyToRuntimeId[1671] = 811; //minecraft:pumpkin_stem:7
            LegacyToRuntimeId[1680] = 812; //minecraft:melon_stem:0
            LegacyToRuntimeId[1681] = 813; //minecraft:melon_stem:1
            LegacyToRuntimeId[1682] = 814; //minecraft:melon_stem:2
            LegacyToRuntimeId[1683] = 815; //minecraft:melon_stem:3
            LegacyToRuntimeId[1684] = 816; //minecraft:melon_stem:4
            LegacyToRuntimeId[1685] = 817; //minecraft:melon_stem:5
            LegacyToRuntimeId[1686] = 818; //minecraft:melon_stem:6
            LegacyToRuntimeId[1687] = 819; //minecraft:melon_stem:7
            LegacyToRuntimeId[1696] = 820; //minecraft:vine:0
            LegacyToRuntimeId[1697] = 821; //minecraft:vine:1
            LegacyToRuntimeId[1698] = 822; //minecraft:vine:2
            LegacyToRuntimeId[1699] = 823; //minecraft:vine:3
            LegacyToRuntimeId[1700] = 824; //minecraft:vine:4
            LegacyToRuntimeId[1701] = 825; //minecraft:vine:5
            LegacyToRuntimeId[1702] = 826; //minecraft:vine:6
            LegacyToRuntimeId[1703] = 827; //minecraft:vine:7
            LegacyToRuntimeId[1704] = 828; //minecraft:vine:8
            LegacyToRuntimeId[1705] = 829; //minecraft:vine:9
            LegacyToRuntimeId[1706] = 830; //minecraft:vine:10
            LegacyToRuntimeId[1707] = 831; //minecraft:vine:11
            LegacyToRuntimeId[1708] = 832; //minecraft:vine:12
            LegacyToRuntimeId[1709] = 833; //minecraft:vine:13
            LegacyToRuntimeId[1710] = 834; //minecraft:vine:14
            LegacyToRuntimeId[1711] = 835; //minecraft:vine:15
            LegacyToRuntimeId[1712] = 836; //minecraft:fence_gate:0
            LegacyToRuntimeId[1713] = 837; //minecraft:fence_gate:1
            LegacyToRuntimeId[1714] = 838; //minecraft:fence_gate:2
            LegacyToRuntimeId[1715] = 839; //minecraft:fence_gate:3
            LegacyToRuntimeId[1716] = 840; //minecraft:fence_gate:4
            LegacyToRuntimeId[1717] = 841; //minecraft:fence_gate:5
            LegacyToRuntimeId[1718] = 842; //minecraft:fence_gate:6
            LegacyToRuntimeId[1719] = 843; //minecraft:fence_gate:7
            LegacyToRuntimeId[1720] = 844; //minecraft:fence_gate:8
            LegacyToRuntimeId[1721] = 845; //minecraft:fence_gate:9
            LegacyToRuntimeId[1722] = 846; //minecraft:fence_gate:10
            LegacyToRuntimeId[1723] = 847; //minecraft:fence_gate:11
            LegacyToRuntimeId[1724] = 848; //minecraft:fence_gate:12
            LegacyToRuntimeId[1725] = 849; //minecraft:fence_gate:13
            LegacyToRuntimeId[1726] = 850; //minecraft:fence_gate:14
            LegacyToRuntimeId[1727] = 851; //minecraft:fence_gate:15
            LegacyToRuntimeId[1728] = 852; //minecraft:brick_stairs:0
            LegacyToRuntimeId[1729] = 853; //minecraft:brick_stairs:1
            LegacyToRuntimeId[1730] = 854; //minecraft:brick_stairs:2
            LegacyToRuntimeId[1731] = 855; //minecraft:brick_stairs:3
            LegacyToRuntimeId[1732] = 856; //minecraft:brick_stairs:4
            LegacyToRuntimeId[1733] = 857; //minecraft:brick_stairs:5
            LegacyToRuntimeId[1734] = 858; //minecraft:brick_stairs:6
            LegacyToRuntimeId[1735] = 859; //minecraft:brick_stairs:7
            LegacyToRuntimeId[1744] = 860; //minecraft:stone_brick_stairs:0
            LegacyToRuntimeId[1745] = 861; //minecraft:stone_brick_stairs:1
            LegacyToRuntimeId[1746] = 862; //minecraft:stone_brick_stairs:2
            LegacyToRuntimeId[1747] = 863; //minecraft:stone_brick_stairs:3
            LegacyToRuntimeId[1748] = 864; //minecraft:stone_brick_stairs:4
            LegacyToRuntimeId[1749] = 865; //minecraft:stone_brick_stairs:5
            LegacyToRuntimeId[1750] = 866; //minecraft:stone_brick_stairs:6
            LegacyToRuntimeId[1751] = 867; //minecraft:stone_brick_stairs:7
            LegacyToRuntimeId[1760] = 868; //minecraft:mycelium:0
            LegacyToRuntimeId[1776] = 869; //minecraft:waterlily:0
            LegacyToRuntimeId[1792] = 870; //minecraft:nether_brick:0
            LegacyToRuntimeId[1808] = 871; //minecraft:nether_brick_fence:0
            LegacyToRuntimeId[1809] = 872; //minecraft:nether_brick_fence:1
            LegacyToRuntimeId[1810] = 873; //minecraft:nether_brick_fence:2
            LegacyToRuntimeId[1811] = 874; //minecraft:nether_brick_fence:3
            LegacyToRuntimeId[1812] = 875; //minecraft:nether_brick_fence:4
            LegacyToRuntimeId[1813] = 876; //minecraft:nether_brick_fence:5
            LegacyToRuntimeId[1814] = 877; //minecraft:nether_brick_fence:6
            LegacyToRuntimeId[1815] = 878; //minecraft:nether_brick_fence:7
            LegacyToRuntimeId[1824] = 879; //minecraft:nether_brick_stairs:0
            LegacyToRuntimeId[1825] = 880; //minecraft:nether_brick_stairs:1
            LegacyToRuntimeId[1826] = 881; //minecraft:nether_brick_stairs:2
            LegacyToRuntimeId[1827] = 882; //minecraft:nether_brick_stairs:3
            LegacyToRuntimeId[1828] = 883; //minecraft:nether_brick_stairs:4
            LegacyToRuntimeId[1829] = 884; //minecraft:nether_brick_stairs:5
            LegacyToRuntimeId[1830] = 885; //minecraft:nether_brick_stairs:6
            LegacyToRuntimeId[1831] = 886; //minecraft:nether_brick_stairs:7
            LegacyToRuntimeId[1840] = 887; //minecraft:nether_wart:0
            LegacyToRuntimeId[1841] = 888; //minecraft:nether_wart:1
            LegacyToRuntimeId[1842] = 889; //minecraft:nether_wart:2
            LegacyToRuntimeId[1843] = 890; //minecraft:nether_wart:3
            LegacyToRuntimeId[1856] = 891; //minecraft:enchanting_table:0
            LegacyToRuntimeId[1872] = 892; //minecraft:brewing_stand:0
            LegacyToRuntimeId[1873] = 893; //minecraft:brewing_stand:1
            LegacyToRuntimeId[1874] = 894; //minecraft:brewing_stand:2
            LegacyToRuntimeId[1875] = 895; //minecraft:brewing_stand:3
            LegacyToRuntimeId[1876] = 896; //minecraft:brewing_stand:4
            LegacyToRuntimeId[1877] = 897; //minecraft:brewing_stand:5
            LegacyToRuntimeId[1878] = 898; //minecraft:brewing_stand:6
            LegacyToRuntimeId[1879] = 899; //minecraft:brewing_stand:7
            LegacyToRuntimeId[1888] = 900; //minecraft:cauldron:0
            LegacyToRuntimeId[1889] = 901; //minecraft:cauldron:1
            LegacyToRuntimeId[1890] = 902; //minecraft:cauldron:2
            LegacyToRuntimeId[1891] = 903; //minecraft:cauldron:3
            LegacyToRuntimeId[1892] = 904; //minecraft:cauldron:4
            LegacyToRuntimeId[1893] = 905; //minecraft:cauldron:5
            LegacyToRuntimeId[1894] = 906; //minecraft:cauldron:6
            LegacyToRuntimeId[1895] = 907; //minecraft:cauldron:7
            LegacyToRuntimeId[1904] = 908; //minecraft:end_portal:0
            LegacyToRuntimeId[1920] = 909; //minecraft:end_portal_frame:0
            LegacyToRuntimeId[1921] = 910; //minecraft:end_portal_frame:1
            LegacyToRuntimeId[1922] = 911; //minecraft:end_portal_frame:2
            LegacyToRuntimeId[1923] = 912; //minecraft:end_portal_frame:3
            LegacyToRuntimeId[1924] = 913; //minecraft:end_portal_frame:4
            LegacyToRuntimeId[1925] = 914; //minecraft:end_portal_frame:5
            LegacyToRuntimeId[1926] = 915; //minecraft:end_portal_frame:6
            LegacyToRuntimeId[1927] = 916; //minecraft:end_portal_frame:7
            LegacyToRuntimeId[1936] = 917; //minecraft:end_stone:0
            LegacyToRuntimeId[1952] = 918; //minecraft:dragon_egg:0
            LegacyToRuntimeId[1968] = 919; //minecraft:redstone_lamp:0
            LegacyToRuntimeId[1984] = 920; //minecraft:lit_redstone_lamp:0
            LegacyToRuntimeId[2000] = 921; //minecraft:dropper:0
            LegacyToRuntimeId[2001] = 922; //minecraft:dropper:1
            LegacyToRuntimeId[2002] = 923; //minecraft:dropper:2
            LegacyToRuntimeId[2003] = 924; //minecraft:dropper:3
            LegacyToRuntimeId[2004] = 925; //minecraft:dropper:4
            LegacyToRuntimeId[2005] = 926; //minecraft:dropper:5
            LegacyToRuntimeId[2006] = 927; //minecraft:dropper:6
            LegacyToRuntimeId[2007] = 928; //minecraft:dropper:7
            LegacyToRuntimeId[2008] = 929; //minecraft:dropper:8
            LegacyToRuntimeId[2009] = 930; //minecraft:dropper:9
            LegacyToRuntimeId[2010] = 931; //minecraft:dropper:10
            LegacyToRuntimeId[2011] = 932; //minecraft:dropper:11
            LegacyToRuntimeId[2012] = 933; //minecraft:dropper:12
            LegacyToRuntimeId[2013] = 934; //minecraft:dropper:13
            LegacyToRuntimeId[2014] = 935; //minecraft:dropper:14
            LegacyToRuntimeId[2015] = 936; //minecraft:dropper:15
            LegacyToRuntimeId[2016] = 937; //minecraft:activator_rail:0
            LegacyToRuntimeId[2017] = 938; //minecraft:activator_rail:1
            LegacyToRuntimeId[2018] = 939; //minecraft:activator_rail:2
            LegacyToRuntimeId[2019] = 940; //minecraft:activator_rail:3
            LegacyToRuntimeId[2020] = 941; //minecraft:activator_rail:4
            LegacyToRuntimeId[2021] = 942; //minecraft:activator_rail:5
            LegacyToRuntimeId[2022] = 943; //minecraft:activator_rail:6
            LegacyToRuntimeId[2023] = 944; //minecraft:activator_rail:7
            LegacyToRuntimeId[2024] = 945; //minecraft:activator_rail:8
            LegacyToRuntimeId[2025] = 946; //minecraft:activator_rail:9
            LegacyToRuntimeId[2026] = 947; //minecraft:activator_rail:10
            LegacyToRuntimeId[2027] = 948; //minecraft:activator_rail:11
            LegacyToRuntimeId[2028] = 949; //minecraft:activator_rail:12
            LegacyToRuntimeId[2029] = 950; //minecraft:activator_rail:13
            LegacyToRuntimeId[2030] = 951; //minecraft:activator_rail:14
            LegacyToRuntimeId[2031] = 952; //minecraft:activator_rail:15
            LegacyToRuntimeId[2032] = 953; //minecraft:cocoa:0
            LegacyToRuntimeId[2033] = 954; //minecraft:cocoa:1
            LegacyToRuntimeId[2034] = 955; //minecraft:cocoa:2
            LegacyToRuntimeId[2035] = 956; //minecraft:cocoa:3
            LegacyToRuntimeId[2036] = 957; //minecraft:cocoa:4
            LegacyToRuntimeId[2037] = 958; //minecraft:cocoa:5
            LegacyToRuntimeId[2038] = 959; //minecraft:cocoa:6
            LegacyToRuntimeId[2039] = 960; //minecraft:cocoa:7
            LegacyToRuntimeId[2040] = 961; //minecraft:cocoa:8
            LegacyToRuntimeId[2041] = 962; //minecraft:cocoa:9
            LegacyToRuntimeId[2042] = 963; //minecraft:cocoa:10
            LegacyToRuntimeId[2043] = 964; //minecraft:cocoa:11
            LegacyToRuntimeId[2044] = 965; //minecraft:cocoa:12
            LegacyToRuntimeId[2045] = 966; //minecraft:cocoa:13
            LegacyToRuntimeId[2046] = 967; //minecraft:cocoa:14
            LegacyToRuntimeId[2047] = 968; //minecraft:cocoa:15
            LegacyToRuntimeId[2048] = 969; //minecraft:sandstone_stairs:0
            LegacyToRuntimeId[2049] = 970; //minecraft:sandstone_stairs:1
            LegacyToRuntimeId[2050] = 971; //minecraft:sandstone_stairs:2
            LegacyToRuntimeId[2051] = 972; //minecraft:sandstone_stairs:3
            LegacyToRuntimeId[2052] = 973; //minecraft:sandstone_stairs:4
            LegacyToRuntimeId[2053] = 974; //minecraft:sandstone_stairs:5
            LegacyToRuntimeId[2054] = 975; //minecraft:sandstone_stairs:6
            LegacyToRuntimeId[2055] = 976; //minecraft:sandstone_stairs:7
            LegacyToRuntimeId[2064] = 977; //minecraft:emerald_ore:0
            LegacyToRuntimeId[2080] = 978; //minecraft:ender_chest:0
            LegacyToRuntimeId[2081] = 979; //minecraft:ender_chest:1
            LegacyToRuntimeId[2082] = 980; //minecraft:ender_chest:2
            LegacyToRuntimeId[2083] = 981; //minecraft:ender_chest:3
            LegacyToRuntimeId[2084] = 982; //minecraft:ender_chest:4
            LegacyToRuntimeId[2085] = 983; //minecraft:ender_chest:5
            LegacyToRuntimeId[2086] = 984; //minecraft:ender_chest:6
            LegacyToRuntimeId[2087] = 985; //minecraft:ender_chest:7
            LegacyToRuntimeId[2096] = 986; //minecraft:tripwire_hook:0
            LegacyToRuntimeId[2097] = 987; //minecraft:tripwire_hook:1
            LegacyToRuntimeId[2098] = 988; //minecraft:tripwire_hook:2
            LegacyToRuntimeId[2099] = 989; //minecraft:tripwire_hook:3
            LegacyToRuntimeId[2100] = 990; //minecraft:tripwire_hook:4
            LegacyToRuntimeId[2101] = 991; //minecraft:tripwire_hook:5
            LegacyToRuntimeId[2102] = 992; //minecraft:tripwire_hook:6
            LegacyToRuntimeId[2103] = 993; //minecraft:tripwire_hook:7
            LegacyToRuntimeId[2104] = 994; //minecraft:tripwire_hook:8
            LegacyToRuntimeId[2105] = 995; //minecraft:tripwire_hook:9
            LegacyToRuntimeId[2106] = 996; //minecraft:tripwire_hook:10
            LegacyToRuntimeId[2107] = 997; //minecraft:tripwire_hook:11
            LegacyToRuntimeId[2108] = 998; //minecraft:tripwire_hook:12
            LegacyToRuntimeId[2109] = 999; //minecraft:tripwire_hook:13
            LegacyToRuntimeId[2110] = 1000; //minecraft:tripwire_hook:14
            LegacyToRuntimeId[2111] = 1001; //minecraft:tripwire_hook:15
            LegacyToRuntimeId[2112] = 1002; //minecraft:tripWire:0
            LegacyToRuntimeId[2113] = 1003; //minecraft:tripWire:1
            LegacyToRuntimeId[2114] = 1004; //minecraft:tripWire:2
            LegacyToRuntimeId[2115] = 1005; //minecraft:tripWire:3
            LegacyToRuntimeId[2116] = 1006; //minecraft:tripWire:4
            LegacyToRuntimeId[2117] = 1007; //minecraft:tripWire:5
            LegacyToRuntimeId[2118] = 1008; //minecraft:tripWire:6
            LegacyToRuntimeId[2119] = 1009; //minecraft:tripWire:7
            LegacyToRuntimeId[2120] = 1010; //minecraft:tripWire:8
            LegacyToRuntimeId[2121] = 1011; //minecraft:tripWire:9
            LegacyToRuntimeId[2122] = 1012; //minecraft:tripWire:10
            LegacyToRuntimeId[2123] = 1013; //minecraft:tripWire:11
            LegacyToRuntimeId[2124] = 1014; //minecraft:tripWire:12
            LegacyToRuntimeId[2125] = 1015; //minecraft:tripWire:13
            LegacyToRuntimeId[2126] = 1016; //minecraft:tripWire:14
            LegacyToRuntimeId[2127] = 1017; //minecraft:tripWire:15
            LegacyToRuntimeId[2128] = 1018; //minecraft:emerald_block:0
            LegacyToRuntimeId[2144] = 1019; //minecraft:spruce_stairs:0
            LegacyToRuntimeId[2145] = 1020; //minecraft:spruce_stairs:1
            LegacyToRuntimeId[2146] = 1021; //minecraft:spruce_stairs:2
            LegacyToRuntimeId[2147] = 1022; //minecraft:spruce_stairs:3
            LegacyToRuntimeId[2148] = 1023; //minecraft:spruce_stairs:4
            LegacyToRuntimeId[2149] = 1024; //minecraft:spruce_stairs:5
            LegacyToRuntimeId[2150] = 1025; //minecraft:spruce_stairs:6
            LegacyToRuntimeId[2151] = 1026; //minecraft:spruce_stairs:7
            LegacyToRuntimeId[2160] = 1027; //minecraft:birch_stairs:0
            LegacyToRuntimeId[2161] = 1028; //minecraft:birch_stairs:1
            LegacyToRuntimeId[2162] = 1029; //minecraft:birch_stairs:2
            LegacyToRuntimeId[2163] = 1030; //minecraft:birch_stairs:3
            LegacyToRuntimeId[2164] = 1031; //minecraft:birch_stairs:4
            LegacyToRuntimeId[2165] = 1032; //minecraft:birch_stairs:5
            LegacyToRuntimeId[2166] = 1033; //minecraft:birch_stairs:6
            LegacyToRuntimeId[2167] = 1034; //minecraft:birch_stairs:7
            LegacyToRuntimeId[2176] = 1035; //minecraft:jungle_stairs:0
            LegacyToRuntimeId[2177] = 1036; //minecraft:jungle_stairs:1
            LegacyToRuntimeId[2178] = 1037; //minecraft:jungle_stairs:2
            LegacyToRuntimeId[2179] = 1038; //minecraft:jungle_stairs:3
            LegacyToRuntimeId[2180] = 1039; //minecraft:jungle_stairs:4
            LegacyToRuntimeId[2181] = 1040; //minecraft:jungle_stairs:5
            LegacyToRuntimeId[2182] = 1041; //minecraft:jungle_stairs:6
            LegacyToRuntimeId[2183] = 1042; //minecraft:jungle_stairs:7
            LegacyToRuntimeId[2192] = 1043; //minecraft:command_block:0
            LegacyToRuntimeId[2193] = 1044; //minecraft:command_block:1
            LegacyToRuntimeId[2194] = 1045; //minecraft:command_block:2
            LegacyToRuntimeId[2195] = 1046; //minecraft:command_block:3
            LegacyToRuntimeId[2196] = 1047; //minecraft:command_block:4
            LegacyToRuntimeId[2197] = 1048; //minecraft:command_block:5
            LegacyToRuntimeId[2198] = 1049; //minecraft:command_block:6
            LegacyToRuntimeId[2199] = 1050; //minecraft:command_block:7
            LegacyToRuntimeId[2200] = 1051; //minecraft:command_block:8
            LegacyToRuntimeId[2201] = 1052; //minecraft:command_block:9
            LegacyToRuntimeId[2202] = 1053; //minecraft:command_block:10
            LegacyToRuntimeId[2203] = 1054; //minecraft:command_block:11
            LegacyToRuntimeId[2204] = 1055; //minecraft:command_block:12
            LegacyToRuntimeId[2205] = 1056; //minecraft:command_block:13
            LegacyToRuntimeId[2206] = 1057; //minecraft:command_block:14
            LegacyToRuntimeId[2207] = 1058; //minecraft:command_block:15
            LegacyToRuntimeId[2208] = 1059; //minecraft:beacon:0
            LegacyToRuntimeId[2224] = 1060; //minecraft:cobblestone_wall:0
            LegacyToRuntimeId[2225] = 1061; //minecraft:cobblestone_wall:1
            LegacyToRuntimeId[2240] = 1062; //minecraft:flower_pot:0
            LegacyToRuntimeId[2241] = 1063; //minecraft:flower_pot:1
            LegacyToRuntimeId[2256] = 1064; //minecraft:carrots:0
            LegacyToRuntimeId[2257] = 1065; //minecraft:carrots:1
            LegacyToRuntimeId[2258] = 1066; //minecraft:carrots:2
            LegacyToRuntimeId[2259] = 1067; //minecraft:carrots:3
            LegacyToRuntimeId[2260] = 1068; //minecraft:carrots:4
            LegacyToRuntimeId[2261] = 1069; //minecraft:carrots:5
            LegacyToRuntimeId[2262] = 1070; //minecraft:carrots:6
            LegacyToRuntimeId[2263] = 1071; //minecraft:carrots:7
            LegacyToRuntimeId[2272] = 1072; //minecraft:potatoes:0
            LegacyToRuntimeId[2273] = 1073; //minecraft:potatoes:1
            LegacyToRuntimeId[2274] = 1074; //minecraft:potatoes:2
            LegacyToRuntimeId[2275] = 1075; //minecraft:potatoes:3
            LegacyToRuntimeId[2276] = 1076; //minecraft:potatoes:4
            LegacyToRuntimeId[2277] = 1077; //minecraft:potatoes:5
            LegacyToRuntimeId[2278] = 1078; //minecraft:potatoes:6
            LegacyToRuntimeId[2279] = 1079; //minecraft:potatoes:7
            LegacyToRuntimeId[2288] = 1080; //minecraft:wooden_button:0
            LegacyToRuntimeId[2289] = 1081; //minecraft:wooden_button:1
            LegacyToRuntimeId[2290] = 1082; //minecraft:wooden_button:2
            LegacyToRuntimeId[2291] = 1083; //minecraft:wooden_button:3
            LegacyToRuntimeId[2292] = 1084; //minecraft:wooden_button:4
            LegacyToRuntimeId[2293] = 1085; //minecraft:wooden_button:5
            LegacyToRuntimeId[2294] = 1086; //minecraft:wooden_button:6
            LegacyToRuntimeId[2295] = 1087; //minecraft:wooden_button:7
            LegacyToRuntimeId[2296] = 1088; //minecraft:wooden_button:8
            LegacyToRuntimeId[2297] = 1089; //minecraft:wooden_button:9
            LegacyToRuntimeId[2298] = 1090; //minecraft:wooden_button:10
            LegacyToRuntimeId[2299] = 1091; //minecraft:wooden_button:11
            LegacyToRuntimeId[2300] = 1092; //minecraft:wooden_button:12
            LegacyToRuntimeId[2301] = 1093; //minecraft:wooden_button:13
            LegacyToRuntimeId[2302] = 1094; //minecraft:wooden_button:14
            LegacyToRuntimeId[2303] = 1095; //minecraft:wooden_button:15
            LegacyToRuntimeId[2304] = 1096; //minecraft:skull:0
            LegacyToRuntimeId[2305] = 1097; //minecraft:skull:1
            LegacyToRuntimeId[2306] = 1098; //minecraft:skull:2
            LegacyToRuntimeId[2307] = 1099; //minecraft:skull:3
            LegacyToRuntimeId[2308] = 1100; //minecraft:skull:4
            LegacyToRuntimeId[2309] = 1101; //minecraft:skull:5
            LegacyToRuntimeId[2310] = 1102; //minecraft:skull:6
            LegacyToRuntimeId[2311] = 1103; //minecraft:skull:7
            LegacyToRuntimeId[2312] = 1104; //minecraft:skull:8
            LegacyToRuntimeId[2313] = 1105; //minecraft:skull:9
            LegacyToRuntimeId[2314] = 1106; //minecraft:skull:10
            LegacyToRuntimeId[2315] = 1107; //minecraft:skull:11
            LegacyToRuntimeId[2316] = 1108; //minecraft:skull:12
            LegacyToRuntimeId[2317] = 1109; //minecraft:skull:13
            LegacyToRuntimeId[2318] = 1110; //minecraft:skull:14
            LegacyToRuntimeId[2319] = 1111; //minecraft:skull:15
            LegacyToRuntimeId[2320] = 1112; //minecraft:anvil:0
            LegacyToRuntimeId[2321] = 1113; //minecraft:anvil:1
            LegacyToRuntimeId[2322] = 1114; //minecraft:anvil:2
            LegacyToRuntimeId[2323] = 1115; //minecraft:anvil:3
            LegacyToRuntimeId[2324] = 1116; //minecraft:anvil:4
            LegacyToRuntimeId[2325] = 1117; //minecraft:anvil:5
            LegacyToRuntimeId[2326] = 1118; //minecraft:anvil:6
            LegacyToRuntimeId[2327] = 1119; //minecraft:anvil:7
            LegacyToRuntimeId[2328] = 1120; //minecraft:anvil:8
            LegacyToRuntimeId[2329] = 1121; //minecraft:anvil:9
            LegacyToRuntimeId[2330] = 1122; //minecraft:anvil:10
            LegacyToRuntimeId[2331] = 1123; //minecraft:anvil:11
            LegacyToRuntimeId[2332] = 1124; //minecraft:anvil:12
            LegacyToRuntimeId[2333] = 1125; //minecraft:anvil:13
            LegacyToRuntimeId[2334] = 1126; //minecraft:anvil:14
            LegacyToRuntimeId[2335] = 1127; //minecraft:anvil:15
            LegacyToRuntimeId[2336] = 1128; //minecraft:trapped_chest:0
            LegacyToRuntimeId[2337] = 1129; //minecraft:trapped_chest:1
            LegacyToRuntimeId[2338] = 1130; //minecraft:trapped_chest:2
            LegacyToRuntimeId[2339] = 1131; //minecraft:trapped_chest:3
            LegacyToRuntimeId[2340] = 1132; //minecraft:trapped_chest:4
            LegacyToRuntimeId[2341] = 1133; //minecraft:trapped_chest:5
            LegacyToRuntimeId[2342] = 1134; //minecraft:trapped_chest:6
            LegacyToRuntimeId[2343] = 1135; //minecraft:trapped_chest:7
            LegacyToRuntimeId[2352] = 1136; //minecraft:light_weighted_pressure_plate:0
            LegacyToRuntimeId[2353] = 1137; //minecraft:light_weighted_pressure_plate:1
            LegacyToRuntimeId[2354] = 1138; //minecraft:light_weighted_pressure_plate:2
            LegacyToRuntimeId[2355] = 1139; //minecraft:light_weighted_pressure_plate:3
            LegacyToRuntimeId[2356] = 1140; //minecraft:light_weighted_pressure_plate:4
            LegacyToRuntimeId[2357] = 1141; //minecraft:light_weighted_pressure_plate:5
            LegacyToRuntimeId[2358] = 1142; //minecraft:light_weighted_pressure_plate:6
            LegacyToRuntimeId[2359] = 1143; //minecraft:light_weighted_pressure_plate:7
            LegacyToRuntimeId[2360] = 1144; //minecraft:light_weighted_pressure_plate:8
            LegacyToRuntimeId[2361] = 1145; //minecraft:light_weighted_pressure_plate:9
            LegacyToRuntimeId[2362] = 1146; //minecraft:light_weighted_pressure_plate:10
            LegacyToRuntimeId[2363] = 1147; //minecraft:light_weighted_pressure_plate:11
            LegacyToRuntimeId[2364] = 1148; //minecraft:light_weighted_pressure_plate:12
            LegacyToRuntimeId[2365] = 1149; //minecraft:light_weighted_pressure_plate:13
            LegacyToRuntimeId[2366] = 1150; //minecraft:light_weighted_pressure_plate:14
            LegacyToRuntimeId[2367] = 1151; //minecraft:light_weighted_pressure_plate:15
            LegacyToRuntimeId[2368] = 1152; //minecraft:heavy_weighted_pressure_plate:0
            LegacyToRuntimeId[2369] = 1153; //minecraft:heavy_weighted_pressure_plate:1
            LegacyToRuntimeId[2370] = 1154; //minecraft:heavy_weighted_pressure_plate:2
            LegacyToRuntimeId[2371] = 1155; //minecraft:heavy_weighted_pressure_plate:3
            LegacyToRuntimeId[2372] = 1156; //minecraft:heavy_weighted_pressure_plate:4
            LegacyToRuntimeId[2373] = 1157; //minecraft:heavy_weighted_pressure_plate:5
            LegacyToRuntimeId[2374] = 1158; //minecraft:heavy_weighted_pressure_plate:6
            LegacyToRuntimeId[2375] = 1159; //minecraft:heavy_weighted_pressure_plate:7
            LegacyToRuntimeId[2376] = 1160; //minecraft:heavy_weighted_pressure_plate:8
            LegacyToRuntimeId[2377] = 1161; //minecraft:heavy_weighted_pressure_plate:9
            LegacyToRuntimeId[2378] = 1162; //minecraft:heavy_weighted_pressure_plate:10
            LegacyToRuntimeId[2379] = 1163; //minecraft:heavy_weighted_pressure_plate:11
            LegacyToRuntimeId[2380] = 1164; //minecraft:heavy_weighted_pressure_plate:12
            LegacyToRuntimeId[2381] = 1165; //minecraft:heavy_weighted_pressure_plate:13
            LegacyToRuntimeId[2382] = 1166; //minecraft:heavy_weighted_pressure_plate:14
            LegacyToRuntimeId[2383] = 1167; //minecraft:heavy_weighted_pressure_plate:15
            LegacyToRuntimeId[2384] = 1168; //minecraft:unpowered_comparator:0
            LegacyToRuntimeId[2385] = 1169; //minecraft:unpowered_comparator:1
            LegacyToRuntimeId[2386] = 1170; //minecraft:unpowered_comparator:2
            LegacyToRuntimeId[2387] = 1171; //minecraft:unpowered_comparator:3
            LegacyToRuntimeId[2388] = 1172; //minecraft:unpowered_comparator:4
            LegacyToRuntimeId[2389] = 1173; //minecraft:unpowered_comparator:5
            LegacyToRuntimeId[2390] = 1174; //minecraft:unpowered_comparator:6
            LegacyToRuntimeId[2391] = 1175; //minecraft:unpowered_comparator:7
            LegacyToRuntimeId[2392] = 1176; //minecraft:unpowered_comparator:8
            LegacyToRuntimeId[2393] = 1177; //minecraft:unpowered_comparator:9
            LegacyToRuntimeId[2394] = 1178; //minecraft:unpowered_comparator:10
            LegacyToRuntimeId[2395] = 1179; //minecraft:unpowered_comparator:11
            LegacyToRuntimeId[2396] = 1180; //minecraft:unpowered_comparator:12
            LegacyToRuntimeId[2397] = 1181; //minecraft:unpowered_comparator:13
            LegacyToRuntimeId[2398] = 1182; //minecraft:unpowered_comparator:14
            LegacyToRuntimeId[2399] = 1183; //minecraft:unpowered_comparator:15
            LegacyToRuntimeId[2400] = 1184; //minecraft:powered_comparator:0
            LegacyToRuntimeId[2401] = 1185; //minecraft:powered_comparator:1
            LegacyToRuntimeId[2402] = 1186; //minecraft:powered_comparator:2
            LegacyToRuntimeId[2403] = 1187; //minecraft:powered_comparator:3
            LegacyToRuntimeId[2404] = 1188; //minecraft:powered_comparator:4
            LegacyToRuntimeId[2405] = 1189; //minecraft:powered_comparator:5
            LegacyToRuntimeId[2406] = 1190; //minecraft:powered_comparator:6
            LegacyToRuntimeId[2407] = 1191; //minecraft:powered_comparator:7
            LegacyToRuntimeId[2408] = 1192; //minecraft:powered_comparator:8
            LegacyToRuntimeId[2409] = 1193; //minecraft:powered_comparator:9
            LegacyToRuntimeId[2410] = 1194; //minecraft:powered_comparator:10
            LegacyToRuntimeId[2411] = 1195; //minecraft:powered_comparator:11
            LegacyToRuntimeId[2412] = 1196; //minecraft:powered_comparator:12
            LegacyToRuntimeId[2413] = 1197; //minecraft:powered_comparator:13
            LegacyToRuntimeId[2414] = 1198; //minecraft:powered_comparator:14
            LegacyToRuntimeId[2415] = 1199; //minecraft:powered_comparator:15
            LegacyToRuntimeId[2416] = 1200; //minecraft:daylight_detector:0
            LegacyToRuntimeId[2417] = 1201; //minecraft:daylight_detector:1
            LegacyToRuntimeId[2418] = 1202; //minecraft:daylight_detector:2
            LegacyToRuntimeId[2419] = 1203; //minecraft:daylight_detector:3
            LegacyToRuntimeId[2420] = 1204; //minecraft:daylight_detector:4
            LegacyToRuntimeId[2421] = 1205; //minecraft:daylight_detector:5
            LegacyToRuntimeId[2422] = 1206; //minecraft:daylight_detector:6
            LegacyToRuntimeId[2423] = 1207; //minecraft:daylight_detector:7
            LegacyToRuntimeId[2424] = 1208; //minecraft:daylight_detector:8
            LegacyToRuntimeId[2425] = 1209; //minecraft:daylight_detector:9
            LegacyToRuntimeId[2426] = 1210; //minecraft:daylight_detector:10
            LegacyToRuntimeId[2427] = 1211; //minecraft:daylight_detector:11
            LegacyToRuntimeId[2428] = 1212; //minecraft:daylight_detector:12
            LegacyToRuntimeId[2429] = 1213; //minecraft:daylight_detector:13
            LegacyToRuntimeId[2430] = 1214; //minecraft:daylight_detector:14
            LegacyToRuntimeId[2431] = 1215; //minecraft:daylight_detector:15
            LegacyToRuntimeId[2432] = 1216; //minecraft:redstone_block:0
            LegacyToRuntimeId[2448] = 1217; //minecraft:quartz_ore:0
            LegacyToRuntimeId[2464] = 1218; //minecraft:hopper:0
            LegacyToRuntimeId[2465] = 1219; //minecraft:hopper:1
            LegacyToRuntimeId[2466] = 1220; //minecraft:hopper:2
            LegacyToRuntimeId[2467] = 1221; //minecraft:hopper:3
            LegacyToRuntimeId[2468] = 1222; //minecraft:hopper:4
            LegacyToRuntimeId[2469] = 1223; //minecraft:hopper:5
            LegacyToRuntimeId[2470] = 1224; //minecraft:hopper:6
            LegacyToRuntimeId[2471] = 1225; //minecraft:hopper:7
            LegacyToRuntimeId[2472] = 1226; //minecraft:hopper:8
            LegacyToRuntimeId[2473] = 1227; //minecraft:hopper:9
            LegacyToRuntimeId[2474] = 1228; //minecraft:hopper:10
            LegacyToRuntimeId[2475] = 1229; //minecraft:hopper:11
            LegacyToRuntimeId[2476] = 1230; //minecraft:hopper:12
            LegacyToRuntimeId[2477] = 1231; //minecraft:hopper:13
            LegacyToRuntimeId[2478] = 1232; //minecraft:hopper:14
            LegacyToRuntimeId[2479] = 1233; //minecraft:hopper:15
            LegacyToRuntimeId[2480] = 1234; //minecraft:quartz_block:0
            LegacyToRuntimeId[2481] = 1235; //minecraft:quartz_block:1
            LegacyToRuntimeId[2482] = 1236; //minecraft:quartz_block:2
            LegacyToRuntimeId[2483] = 1237; //minecraft:quartz_block:3
            LegacyToRuntimeId[2484] = 1238; //minecraft:quartz_block:4
            LegacyToRuntimeId[2485] = 1239; //minecraft:quartz_block:5
            LegacyToRuntimeId[2486] = 1240; //minecraft:quartz_block:6
            LegacyToRuntimeId[2487] = 1241; //minecraft:quartz_block:7
            LegacyToRuntimeId[2488] = 1242; //minecraft:quartz_block:8
            LegacyToRuntimeId[2489] = 1243; //minecraft:quartz_block:9
            LegacyToRuntimeId[2490] = 1244; //minecraft:quartz_block:10
            LegacyToRuntimeId[2491] = 1245; //minecraft:quartz_block:11
            LegacyToRuntimeId[2492] = 1246; //minecraft:quartz_block:12
            LegacyToRuntimeId[2493] = 1247; //minecraft:quartz_block:13
            LegacyToRuntimeId[2494] = 1248; //minecraft:quartz_block:14
            LegacyToRuntimeId[2495] = 1249; //minecraft:quartz_block:15
            LegacyToRuntimeId[2496] = 1250; //minecraft:quartz_stairs:0
            LegacyToRuntimeId[2497] = 1251; //minecraft:quartz_stairs:1
            LegacyToRuntimeId[2498] = 1252; //minecraft:quartz_stairs:2
            LegacyToRuntimeId[2499] = 1253; //minecraft:quartz_stairs:3
            LegacyToRuntimeId[2500] = 1254; //minecraft:quartz_stairs:4
            LegacyToRuntimeId[2501] = 1255; //minecraft:quartz_stairs:5
            LegacyToRuntimeId[2502] = 1256; //minecraft:quartz_stairs:6
            LegacyToRuntimeId[2503] = 1257; //minecraft:quartz_stairs:7
            LegacyToRuntimeId[2512] = 1258; //minecraft:double_wooden_slab:0
            LegacyToRuntimeId[2513] = 1259; //minecraft:double_wooden_slab:1
            LegacyToRuntimeId[2514] = 1260; //minecraft:double_wooden_slab:2
            LegacyToRuntimeId[2515] = 1261; //minecraft:double_wooden_slab:3
            LegacyToRuntimeId[2516] = 1262; //minecraft:double_wooden_slab:4
            LegacyToRuntimeId[2517] = 1263; //minecraft:double_wooden_slab:5
            LegacyToRuntimeId[2518] = 1264; //minecraft:double_wooden_slab:6
            LegacyToRuntimeId[2519] = 1265; //minecraft:double_wooden_slab:7
            LegacyToRuntimeId[2520] = 1266; //minecraft:double_wooden_slab:8
            LegacyToRuntimeId[2521] = 1267; //minecraft:double_wooden_slab:9
            LegacyToRuntimeId[2522] = 1268; //minecraft:double_wooden_slab:10
            LegacyToRuntimeId[2523] = 1269; //minecraft:double_wooden_slab:11
            LegacyToRuntimeId[2524] = 1270; //minecraft:double_wooden_slab:12
            LegacyToRuntimeId[2525] = 1271; //minecraft:double_wooden_slab:13
            LegacyToRuntimeId[2526] = 1272; //minecraft:double_wooden_slab:14
            LegacyToRuntimeId[2527] = 1273; //minecraft:double_wooden_slab:15
            LegacyToRuntimeId[2528] = 1274; //minecraft:wooden_slab:0
            LegacyToRuntimeId[2529] = 1275; //minecraft:wooden_slab:1
            LegacyToRuntimeId[2530] = 1276; //minecraft:wooden_slab:2
            LegacyToRuntimeId[2531] = 1277; //minecraft:wooden_slab:3
            LegacyToRuntimeId[2532] = 1278; //minecraft:wooden_slab:4
            LegacyToRuntimeId[2533] = 1279; //minecraft:wooden_slab:5
            LegacyToRuntimeId[2534] = 1280; //minecraft:wooden_slab:6
            LegacyToRuntimeId[2535] = 1281; //minecraft:wooden_slab:7
            LegacyToRuntimeId[2536] = 1282; //minecraft:wooden_slab:8
            LegacyToRuntimeId[2537] = 1283; //minecraft:wooden_slab:9
            LegacyToRuntimeId[2538] = 1284; //minecraft:wooden_slab:10
            LegacyToRuntimeId[2539] = 1285; //minecraft:wooden_slab:11
            LegacyToRuntimeId[2540] = 1286; //minecraft:wooden_slab:12
            LegacyToRuntimeId[2541] = 1287; //minecraft:wooden_slab:13
            LegacyToRuntimeId[2542] = 1288; //minecraft:wooden_slab:14
            LegacyToRuntimeId[2543] = 1289; //minecraft:wooden_slab:15
            LegacyToRuntimeId[2544] = 1290; //minecraft:stained_hardened_clay:0
            LegacyToRuntimeId[2545] = 1291; //minecraft:stained_hardened_clay:1
            LegacyToRuntimeId[2546] = 1292; //minecraft:stained_hardened_clay:2
            LegacyToRuntimeId[2547] = 1293; //minecraft:stained_hardened_clay:3
            LegacyToRuntimeId[2548] = 1294; //minecraft:stained_hardened_clay:4
            LegacyToRuntimeId[2549] = 1295; //minecraft:stained_hardened_clay:5
            LegacyToRuntimeId[2550] = 1296; //minecraft:stained_hardened_clay:6
            LegacyToRuntimeId[2551] = 1297; //minecraft:stained_hardened_clay:7
            LegacyToRuntimeId[2552] = 1298; //minecraft:stained_hardened_clay:8
            LegacyToRuntimeId[2553] = 1299; //minecraft:stained_hardened_clay:9
            LegacyToRuntimeId[2554] = 1300; //minecraft:stained_hardened_clay:10
            LegacyToRuntimeId[2555] = 1301; //minecraft:stained_hardened_clay:11
            LegacyToRuntimeId[2556] = 1302; //minecraft:stained_hardened_clay:12
            LegacyToRuntimeId[2557] = 1303; //minecraft:stained_hardened_clay:13
            LegacyToRuntimeId[2558] = 1304; //minecraft:stained_hardened_clay:14
            LegacyToRuntimeId[2559] = 1305; //minecraft:stained_hardened_clay:15
            LegacyToRuntimeId[2560] = 1306; //minecraft:stained_glass_pane:0
            LegacyToRuntimeId[2561] = 1307; //minecraft:stained_glass_pane:1
            LegacyToRuntimeId[2562] = 1308; //minecraft:stained_glass_pane:2
            LegacyToRuntimeId[2563] = 1309; //minecraft:stained_glass_pane:3
            LegacyToRuntimeId[2564] = 1310; //minecraft:stained_glass_pane:4
            LegacyToRuntimeId[2565] = 1311; //minecraft:stained_glass_pane:5
            LegacyToRuntimeId[2566] = 1312; //minecraft:stained_glass_pane:6
            LegacyToRuntimeId[2567] = 1313; //minecraft:stained_glass_pane:7
            LegacyToRuntimeId[2568] = 1314; //minecraft:stained_glass_pane:8
            LegacyToRuntimeId[2569] = 1315; //minecraft:stained_glass_pane:9
            LegacyToRuntimeId[2570] = 1316; //minecraft:stained_glass_pane:10
            LegacyToRuntimeId[2571] = 1317; //minecraft:stained_glass_pane:11
            LegacyToRuntimeId[2572] = 1318; //minecraft:stained_glass_pane:12
            LegacyToRuntimeId[2573] = 1319; //minecraft:stained_glass_pane:13
            LegacyToRuntimeId[2574] = 1320; //minecraft:stained_glass_pane:14
            LegacyToRuntimeId[2575] = 1321; //minecraft:stained_glass_pane:15
            LegacyToRuntimeId[2576] = 1322; //minecraft:leaves2:0
            LegacyToRuntimeId[2577] = 1323; //minecraft:leaves2:1
            LegacyToRuntimeId[2578] = 1324; //minecraft:leaves2:2
            LegacyToRuntimeId[2579] = 1325; //minecraft:leaves2:3
            LegacyToRuntimeId[2580] = 1326; //minecraft:leaves2:4
            LegacyToRuntimeId[2581] = 1327; //minecraft:leaves2:5
            LegacyToRuntimeId[2582] = 1328; //minecraft:leaves2:6
            LegacyToRuntimeId[2583] = 1329; //minecraft:leaves2:7
            LegacyToRuntimeId[2584] = 1330; //minecraft:leaves2:8
            LegacyToRuntimeId[2585] = 1331; //minecraft:leaves2:9
            LegacyToRuntimeId[2586] = 1332; //minecraft:leaves2:10
            LegacyToRuntimeId[2587] = 1333; //minecraft:leaves2:11
            LegacyToRuntimeId[2588] = 1334; //minecraft:leaves2:12
            LegacyToRuntimeId[2589] = 1335; //minecraft:leaves2:13
            LegacyToRuntimeId[2590] = 1336; //minecraft:leaves2:14
            LegacyToRuntimeId[2591] = 1337; //minecraft:leaves2:15
            LegacyToRuntimeId[2592] = 1338; //minecraft:log2:0
            LegacyToRuntimeId[2593] = 1339; //minecraft:log2:1
            LegacyToRuntimeId[2594] = 1340; //minecraft:log2:2
            LegacyToRuntimeId[2595] = 1341; //minecraft:log2:3
            LegacyToRuntimeId[2596] = 1342; //minecraft:log2:4
            LegacyToRuntimeId[2597] = 1343; //minecraft:log2:5
            LegacyToRuntimeId[2598] = 1344; //minecraft:log2:6
            LegacyToRuntimeId[2599] = 1345; //minecraft:log2:7
            LegacyToRuntimeId[2600] = 1346; //minecraft:log2:8
            LegacyToRuntimeId[2601] = 1347; //minecraft:log2:9
            LegacyToRuntimeId[2602] = 1348; //minecraft:log2:10
            LegacyToRuntimeId[2603] = 1349; //minecraft:log2:11
            LegacyToRuntimeId[2604] = 1350; //minecraft:log2:12
            LegacyToRuntimeId[2605] = 1351; //minecraft:log2:13
            LegacyToRuntimeId[2606] = 1352; //minecraft:log2:14
            LegacyToRuntimeId[2607] = 1353; //minecraft:log2:15
            LegacyToRuntimeId[2608] = 1354; //minecraft:acacia_stairs:0
            LegacyToRuntimeId[2609] = 1355; //minecraft:acacia_stairs:1
            LegacyToRuntimeId[2610] = 1356; //minecraft:acacia_stairs:2
            LegacyToRuntimeId[2611] = 1357; //minecraft:acacia_stairs:3
            LegacyToRuntimeId[2612] = 1358; //minecraft:acacia_stairs:4
            LegacyToRuntimeId[2613] = 1359; //minecraft:acacia_stairs:5
            LegacyToRuntimeId[2614] = 1360; //minecraft:acacia_stairs:6
            LegacyToRuntimeId[2615] = 1361; //minecraft:acacia_stairs:7
            LegacyToRuntimeId[2624] = 1362; //minecraft:dark_oak_stairs:0
            LegacyToRuntimeId[2625] = 1363; //minecraft:dark_oak_stairs:1
            LegacyToRuntimeId[2626] = 1364; //minecraft:dark_oak_stairs:2
            LegacyToRuntimeId[2627] = 1365; //minecraft:dark_oak_stairs:3
            LegacyToRuntimeId[2628] = 1366; //minecraft:dark_oak_stairs:4
            LegacyToRuntimeId[2629] = 1367; //minecraft:dark_oak_stairs:5
            LegacyToRuntimeId[2630] = 1368; //minecraft:dark_oak_stairs:6
            LegacyToRuntimeId[2631] = 1369; //minecraft:dark_oak_stairs:7
            LegacyToRuntimeId[2640] = 1370; //minecraft:slime:0
            LegacyToRuntimeId[2672] = 1372; //minecraft:iron_trapdoor:0
            LegacyToRuntimeId[2673] = 1373; //minecraft:iron_trapdoor:1
            LegacyToRuntimeId[2674] = 1374; //minecraft:iron_trapdoor:2
            LegacyToRuntimeId[2675] = 1375; //minecraft:iron_trapdoor:3
            LegacyToRuntimeId[2676] = 1376; //minecraft:iron_trapdoor:4
            LegacyToRuntimeId[2677] = 1377; //minecraft:iron_trapdoor:5
            LegacyToRuntimeId[2678] = 1378; //minecraft:iron_trapdoor:6
            LegacyToRuntimeId[2679] = 1379; //minecraft:iron_trapdoor:7
            LegacyToRuntimeId[2680] = 1380; //minecraft:iron_trapdoor:8
            LegacyToRuntimeId[2681] = 1381; //minecraft:iron_trapdoor:9
            LegacyToRuntimeId[2682] = 1382; //minecraft:iron_trapdoor:10
            LegacyToRuntimeId[2683] = 1383; //minecraft:iron_trapdoor:11
            LegacyToRuntimeId[2684] = 1384; //minecraft:iron_trapdoor:12
            LegacyToRuntimeId[2685] = 1385; //minecraft:iron_trapdoor:13
            LegacyToRuntimeId[2686] = 1386; //minecraft:iron_trapdoor:14
            LegacyToRuntimeId[2687] = 1387; //minecraft:iron_trapdoor:15
            LegacyToRuntimeId[2688] = 1388; //minecraft:prismarine:0
            LegacyToRuntimeId[2689] = 1389; //minecraft:prismarine:1
            LegacyToRuntimeId[2690] = 1390; //minecraft:prismarine:2
            LegacyToRuntimeId[2691] = 1391; //minecraft:prismarine:3
            LegacyToRuntimeId[2704] = 1392; //minecraft:seaLantern:0
            LegacyToRuntimeId[2720] = 1393; //minecraft:hay_block:0
            LegacyToRuntimeId[2721] = 1394; //minecraft:hay_block:1
            LegacyToRuntimeId[2722] = 1395; //minecraft:hay_block:2
            LegacyToRuntimeId[2723] = 1396; //minecraft:hay_block:3
            LegacyToRuntimeId[2724] = 1397; //minecraft:hay_block:4
            LegacyToRuntimeId[2725] = 1398; //minecraft:hay_block:5
            LegacyToRuntimeId[2726] = 1399; //minecraft:hay_block:6
            LegacyToRuntimeId[2727] = 1400; //minecraft:hay_block:7
            LegacyToRuntimeId[2728] = 1401; //minecraft:hay_block:8
            LegacyToRuntimeId[2729] = 1402; //minecraft:hay_block:9
            LegacyToRuntimeId[2730] = 1403; //minecraft:hay_block:10
            LegacyToRuntimeId[2731] = 1404; //minecraft:hay_block:11
            LegacyToRuntimeId[2732] = 1405; //minecraft:hay_block:12
            LegacyToRuntimeId[2733] = 1406; //minecraft:hay_block:13
            LegacyToRuntimeId[2734] = 1407; //minecraft:hay_block:14
            LegacyToRuntimeId[2735] = 1408; //minecraft:hay_block:15
            LegacyToRuntimeId[2736] = 1409; //minecraft:carpet:0
            LegacyToRuntimeId[2737] = 1410; //minecraft:carpet:1
            LegacyToRuntimeId[2738] = 1411; //minecraft:carpet:2
            LegacyToRuntimeId[2739] = 1412; //minecraft:carpet:3
            LegacyToRuntimeId[2740] = 1413; //minecraft:carpet:4
            LegacyToRuntimeId[2741] = 1414; //minecraft:carpet:5
            LegacyToRuntimeId[2742] = 1415; //minecraft:carpet:6
            LegacyToRuntimeId[2743] = 1416; //minecraft:carpet:7
            LegacyToRuntimeId[2744] = 1417; //minecraft:carpet:8
            LegacyToRuntimeId[2745] = 1418; //minecraft:carpet:9
            LegacyToRuntimeId[2746] = 1419; //minecraft:carpet:10
            LegacyToRuntimeId[2747] = 1420; //minecraft:carpet:11
            LegacyToRuntimeId[2748] = 1421; //minecraft:carpet:12
            LegacyToRuntimeId[2749] = 1422; //minecraft:carpet:13
            LegacyToRuntimeId[2750] = 1423; //minecraft:carpet:14
            LegacyToRuntimeId[2751] = 1424; //minecraft:carpet:15
            LegacyToRuntimeId[2752] = 1425; //minecraft:hardened_clay:0
            LegacyToRuntimeId[2768] = 1426; //minecraft:coal_block:0
            LegacyToRuntimeId[2784] = 1427; //minecraft:packed_ice:0
            LegacyToRuntimeId[2800] = 1428; //minecraft:double_plant:0
            LegacyToRuntimeId[2801] = 1429; //minecraft:double_plant:1
            LegacyToRuntimeId[2802] = 1430; //minecraft:double_plant:2
            LegacyToRuntimeId[2803] = 1431; //minecraft:double_plant:3
            LegacyToRuntimeId[2804] = 1432; //minecraft:double_plant:4
            LegacyToRuntimeId[2805] = 1433; //minecraft:double_plant:5
            LegacyToRuntimeId[2806] = 1434; //minecraft:double_plant:6
            LegacyToRuntimeId[2807] = 1435; //minecraft:double_plant:7
            LegacyToRuntimeId[2808] = 1436; //minecraft:double_plant:8
            LegacyToRuntimeId[2809] = 1437; //minecraft:double_plant:9
            LegacyToRuntimeId[2810] = 1438; //minecraft:double_plant:10
            LegacyToRuntimeId[2811] = 1439; //minecraft:double_plant:11
            LegacyToRuntimeId[2812] = 1440; //minecraft:double_plant:12
            LegacyToRuntimeId[2813] = 1441; //minecraft:double_plant:13
            LegacyToRuntimeId[2814] = 1442; //minecraft:double_plant:14
            LegacyToRuntimeId[2815] = 1443; //minecraft:double_plant:15
            LegacyToRuntimeId[2816] = 1444; //minecraft:standing_banner:0
            LegacyToRuntimeId[2817] = 1445; //minecraft:standing_banner:1
            LegacyToRuntimeId[2818] = 1446; //minecraft:standing_banner:2
            LegacyToRuntimeId[2819] = 1447; //minecraft:standing_banner:3
            LegacyToRuntimeId[2820] = 1448; //minecraft:standing_banner:4
            LegacyToRuntimeId[2821] = 1449; //minecraft:standing_banner:5
            LegacyToRuntimeId[2822] = 1450; //minecraft:standing_banner:6
            LegacyToRuntimeId[2823] = 1451; //minecraft:standing_banner:7
            LegacyToRuntimeId[2824] = 1452; //minecraft:standing_banner:8
            LegacyToRuntimeId[2825] = 1453; //minecraft:standing_banner:9
            LegacyToRuntimeId[2826] = 1454; //minecraft:standing_banner:10
            LegacyToRuntimeId[2827] = 1455; //minecraft:standing_banner:11
            LegacyToRuntimeId[2828] = 1456; //minecraft:standing_banner:12
            LegacyToRuntimeId[2829] = 1457; //minecraft:standing_banner:13
            LegacyToRuntimeId[2830] = 1458; //minecraft:standing_banner:14
            LegacyToRuntimeId[2831] = 1459; //minecraft:standing_banner:15
            LegacyToRuntimeId[2832] = 1460; //minecraft:wall_banner:0
            LegacyToRuntimeId[2833] = 1461; //minecraft:wall_banner:1
            LegacyToRuntimeId[2834] = 1462; //minecraft:wall_banner:2
            LegacyToRuntimeId[2835] = 1463; //minecraft:wall_banner:3
            LegacyToRuntimeId[2836] = 1464; //minecraft:wall_banner:4
            LegacyToRuntimeId[2837] = 1465; //minecraft:wall_banner:5
            LegacyToRuntimeId[2838] = 1466; //minecraft:wall_banner:6
            LegacyToRuntimeId[2839] = 1467; //minecraft:wall_banner:7
            LegacyToRuntimeId[2848] = 1468; //minecraft:daylight_detector_inverted:0
            LegacyToRuntimeId[2849] = 1469; //minecraft:daylight_detector_inverted:1
            LegacyToRuntimeId[2850] = 1470; //minecraft:daylight_detector_inverted:2
            LegacyToRuntimeId[2851] = 1471; //minecraft:daylight_detector_inverted:3
            LegacyToRuntimeId[2852] = 1472; //minecraft:daylight_detector_inverted:4
            LegacyToRuntimeId[2853] = 1473; //minecraft:daylight_detector_inverted:5
            LegacyToRuntimeId[2854] = 1474; //minecraft:daylight_detector_inverted:6
            LegacyToRuntimeId[2855] = 1475; //minecraft:daylight_detector_inverted:7
            LegacyToRuntimeId[2856] = 1476; //minecraft:daylight_detector_inverted:8
            LegacyToRuntimeId[2857] = 1477; //minecraft:daylight_detector_inverted:9
            LegacyToRuntimeId[2858] = 1478; //minecraft:daylight_detector_inverted:10
            LegacyToRuntimeId[2859] = 1479; //minecraft:daylight_detector_inverted:11
            LegacyToRuntimeId[2860] = 1480; //minecraft:daylight_detector_inverted:12
            LegacyToRuntimeId[2861] = 1481; //minecraft:daylight_detector_inverted:13
            LegacyToRuntimeId[2862] = 1482; //minecraft:daylight_detector_inverted:14
            LegacyToRuntimeId[2863] = 1483; //minecraft:daylight_detector_inverted:15
            LegacyToRuntimeId[2864] = 1484; //minecraft:red_sandstone:0
            LegacyToRuntimeId[2865] = 1485; //minecraft:red_sandstone:1
            LegacyToRuntimeId[2866] = 1486; //minecraft:red_sandstone:2
            LegacyToRuntimeId[2867] = 1487; //minecraft:red_sandstone:3
            LegacyToRuntimeId[2880] = 1488; //minecraft:red_sandstone_stairs:0
            LegacyToRuntimeId[2881] = 1489; //minecraft:red_sandstone_stairs:1
            LegacyToRuntimeId[2882] = 1490; //minecraft:red_sandstone_stairs:2
            LegacyToRuntimeId[2883] = 1491; //minecraft:red_sandstone_stairs:3
            LegacyToRuntimeId[2884] = 1492; //minecraft:red_sandstone_stairs:4
            LegacyToRuntimeId[2885] = 1493; //minecraft:red_sandstone_stairs:5
            LegacyToRuntimeId[2886] = 1494; //minecraft:red_sandstone_stairs:6
            LegacyToRuntimeId[2887] = 1495; //minecraft:red_sandstone_stairs:7
            LegacyToRuntimeId[2896] = 1496; //minecraft:double_stone_slab2:0
            LegacyToRuntimeId[2897] = 1497; //minecraft:double_stone_slab2:1
            LegacyToRuntimeId[2898] = 1498; //minecraft:double_stone_slab2:2
            LegacyToRuntimeId[2899] = 1499; //minecraft:double_stone_slab2:3
            LegacyToRuntimeId[2900] = 1500; //minecraft:double_stone_slab2:4
            LegacyToRuntimeId[2901] = 1501; //minecraft:double_stone_slab2:5
            LegacyToRuntimeId[2902] = 1502; //minecraft:double_stone_slab2:6
            LegacyToRuntimeId[2903] = 1503; //minecraft:double_stone_slab2:7
            LegacyToRuntimeId[2904] = 1504; //minecraft:double_stone_slab2:8
            LegacyToRuntimeId[2905] = 1505; //minecraft:double_stone_slab2:9
            LegacyToRuntimeId[2906] = 1506; //minecraft:double_stone_slab2:10
            LegacyToRuntimeId[2907] = 1507; //minecraft:double_stone_slab2:11
            LegacyToRuntimeId[2908] = 1508; //minecraft:double_stone_slab2:12
            LegacyToRuntimeId[2909] = 1509; //minecraft:double_stone_slab2:13
            LegacyToRuntimeId[2910] = 1510; //minecraft:double_stone_slab2:14
            LegacyToRuntimeId[2911] = 1511; //minecraft:double_stone_slab2:15
            LegacyToRuntimeId[2912] = 1512; //minecraft:stone_slab2:0
            LegacyToRuntimeId[2913] = 1513; //minecraft:stone_slab2:1
            LegacyToRuntimeId[2914] = 1514; //minecraft:stone_slab2:2
            LegacyToRuntimeId[2915] = 1515; //minecraft:stone_slab2:3
            LegacyToRuntimeId[2916] = 1516; //minecraft:stone_slab2:4
            LegacyToRuntimeId[2917] = 1517; //minecraft:stone_slab2:5
            LegacyToRuntimeId[2918] = 1518; //minecraft:stone_slab2:6
            LegacyToRuntimeId[2919] = 1519; //minecraft:stone_slab2:7
            LegacyToRuntimeId[2920] = 1520; //minecraft:stone_slab2:8
            LegacyToRuntimeId[2921] = 1521; //minecraft:stone_slab2:9
            LegacyToRuntimeId[2922] = 1522; //minecraft:stone_slab2:10
            LegacyToRuntimeId[2923] = 1523; //minecraft:stone_slab2:11
            LegacyToRuntimeId[2924] = 1524; //minecraft:stone_slab2:12
            LegacyToRuntimeId[2925] = 1525; //minecraft:stone_slab2:13
            LegacyToRuntimeId[2926] = 1526; //minecraft:stone_slab2:14
            LegacyToRuntimeId[2927] = 1527; //minecraft:stone_slab2:15
            LegacyToRuntimeId[2928] = 1528; //minecraft:spruce_fence_gate:0
            LegacyToRuntimeId[2929] = 1529; //minecraft:spruce_fence_gate:1
            LegacyToRuntimeId[2930] = 1530; //minecraft:spruce_fence_gate:2
            LegacyToRuntimeId[2931] = 1531; //minecraft:spruce_fence_gate:3
            LegacyToRuntimeId[2932] = 1532; //minecraft:spruce_fence_gate:4
            LegacyToRuntimeId[2933] = 1533; //minecraft:spruce_fence_gate:5
            LegacyToRuntimeId[2934] = 1534; //minecraft:spruce_fence_gate:6
            LegacyToRuntimeId[2935] = 1535; //minecraft:spruce_fence_gate:7
            LegacyToRuntimeId[2936] = 1536; //minecraft:spruce_fence_gate:8
            LegacyToRuntimeId[2937] = 1537; //minecraft:spruce_fence_gate:9
            LegacyToRuntimeId[2938] = 1538; //minecraft:spruce_fence_gate:10
            LegacyToRuntimeId[2939] = 1539; //minecraft:spruce_fence_gate:11
            LegacyToRuntimeId[2940] = 1540; //minecraft:spruce_fence_gate:12
            LegacyToRuntimeId[2941] = 1541; //minecraft:spruce_fence_gate:13
            LegacyToRuntimeId[2942] = 1542; //minecraft:spruce_fence_gate:14
            LegacyToRuntimeId[2943] = 1543; //minecraft:spruce_fence_gate:15
            LegacyToRuntimeId[2944] = 1544; //minecraft:birch_fence_gate:0
            LegacyToRuntimeId[2945] = 1545; //minecraft:birch_fence_gate:1
            LegacyToRuntimeId[2946] = 1546; //minecraft:birch_fence_gate:2
            LegacyToRuntimeId[2947] = 1547; //minecraft:birch_fence_gate:3
            LegacyToRuntimeId[2948] = 1548; //minecraft:birch_fence_gate:4
            LegacyToRuntimeId[2949] = 1549; //minecraft:birch_fence_gate:5
            LegacyToRuntimeId[2950] = 1550; //minecraft:birch_fence_gate:6
            LegacyToRuntimeId[2951] = 1551; //minecraft:birch_fence_gate:7
            LegacyToRuntimeId[2952] = 1552; //minecraft:birch_fence_gate:8
            LegacyToRuntimeId[2953] = 1553; //minecraft:birch_fence_gate:9
            LegacyToRuntimeId[2954] = 1554; //minecraft:birch_fence_gate:10
            LegacyToRuntimeId[2955] = 1555; //minecraft:birch_fence_gate:11
            LegacyToRuntimeId[2956] = 1556; //minecraft:birch_fence_gate:12
            LegacyToRuntimeId[2957] = 1557; //minecraft:birch_fence_gate:13
            LegacyToRuntimeId[2958] = 1558; //minecraft:birch_fence_gate:14
            LegacyToRuntimeId[2959] = 1559; //minecraft:birch_fence_gate:15
            LegacyToRuntimeId[2960] = 1560; //minecraft:jungle_fence_gate:0
            LegacyToRuntimeId[2961] = 1561; //minecraft:jungle_fence_gate:1
            LegacyToRuntimeId[2962] = 1562; //minecraft:jungle_fence_gate:2
            LegacyToRuntimeId[2963] = 1563; //minecraft:jungle_fence_gate:3
            LegacyToRuntimeId[2964] = 1564; //minecraft:jungle_fence_gate:4
            LegacyToRuntimeId[2965] = 1565; //minecraft:jungle_fence_gate:5
            LegacyToRuntimeId[2966] = 1566; //minecraft:jungle_fence_gate:6
            LegacyToRuntimeId[2967] = 1567; //minecraft:jungle_fence_gate:7
            LegacyToRuntimeId[2968] = 1568; //minecraft:jungle_fence_gate:8
            LegacyToRuntimeId[2969] = 1569; //minecraft:jungle_fence_gate:9
            LegacyToRuntimeId[2970] = 1570; //minecraft:jungle_fence_gate:10
            LegacyToRuntimeId[2971] = 1571; //minecraft:jungle_fence_gate:11
            LegacyToRuntimeId[2972] = 1572; //minecraft:jungle_fence_gate:12
            LegacyToRuntimeId[2973] = 1573; //minecraft:jungle_fence_gate:13
            LegacyToRuntimeId[2974] = 1574; //minecraft:jungle_fence_gate:14
            LegacyToRuntimeId[2975] = 1575; //minecraft:jungle_fence_gate:15
            LegacyToRuntimeId[2976] = 1576; //minecraft:dark_oak_fence_gate:0
            LegacyToRuntimeId[2977] = 1577; //minecraft:dark_oak_fence_gate:1
            LegacyToRuntimeId[2978] = 1578; //minecraft:dark_oak_fence_gate:2
            LegacyToRuntimeId[2979] = 1579; //minecraft:dark_oak_fence_gate:3
            LegacyToRuntimeId[2980] = 1580; //minecraft:dark_oak_fence_gate:4
            LegacyToRuntimeId[2981] = 1581; //minecraft:dark_oak_fence_gate:5
            LegacyToRuntimeId[2982] = 1582; //minecraft:dark_oak_fence_gate:6
            LegacyToRuntimeId[2983] = 1583; //minecraft:dark_oak_fence_gate:7
            LegacyToRuntimeId[2984] = 1584; //minecraft:dark_oak_fence_gate:8
            LegacyToRuntimeId[2985] = 1585; //minecraft:dark_oak_fence_gate:9
            LegacyToRuntimeId[2986] = 1586; //minecraft:dark_oak_fence_gate:10
            LegacyToRuntimeId[2987] = 1587; //minecraft:dark_oak_fence_gate:11
            LegacyToRuntimeId[2988] = 1588; //minecraft:dark_oak_fence_gate:12
            LegacyToRuntimeId[2989] = 1589; //minecraft:dark_oak_fence_gate:13
            LegacyToRuntimeId[2990] = 1590; //minecraft:dark_oak_fence_gate:14
            LegacyToRuntimeId[2991] = 1591; //minecraft:dark_oak_fence_gate:15
            LegacyToRuntimeId[2992] = 1592; //minecraft:acacia_fence_gate:0
            LegacyToRuntimeId[2993] = 1593; //minecraft:acacia_fence_gate:1
            LegacyToRuntimeId[2994] = 1594; //minecraft:acacia_fence_gate:2
            LegacyToRuntimeId[2995] = 1595; //minecraft:acacia_fence_gate:3
            LegacyToRuntimeId[2996] = 1596; //minecraft:acacia_fence_gate:4
            LegacyToRuntimeId[2997] = 1597; //minecraft:acacia_fence_gate:5
            LegacyToRuntimeId[2998] = 1598; //minecraft:acacia_fence_gate:6
            LegacyToRuntimeId[2999] = 1599; //minecraft:acacia_fence_gate:7
            LegacyToRuntimeId[3000] = 1600; //minecraft:acacia_fence_gate:8
            LegacyToRuntimeId[3001] = 1601; //minecraft:acacia_fence_gate:9
            LegacyToRuntimeId[3002] = 1602; //minecraft:acacia_fence_gate:10
            LegacyToRuntimeId[3003] = 1603; //minecraft:acacia_fence_gate:11
            LegacyToRuntimeId[3004] = 1604; //minecraft:acacia_fence_gate:12
            LegacyToRuntimeId[3005] = 1605; //minecraft:acacia_fence_gate:13
            LegacyToRuntimeId[3006] = 1606; //minecraft:acacia_fence_gate:14
            LegacyToRuntimeId[3007] = 1607; //minecraft:acacia_fence_gate:15
            LegacyToRuntimeId[3008] = 1608; //minecraft:repeating_command_block:0
            LegacyToRuntimeId[3009] = 1609; //minecraft:repeating_command_block:1
            LegacyToRuntimeId[3010] = 1610; //minecraft:repeating_command_block:2
            LegacyToRuntimeId[3011] = 1611; //minecraft:repeating_command_block:3
            LegacyToRuntimeId[3012] = 1612; //minecraft:repeating_command_block:4
            LegacyToRuntimeId[3013] = 1613; //minecraft:repeating_command_block:5
            LegacyToRuntimeId[3014] = 1614; //minecraft:repeating_command_block:6
            LegacyToRuntimeId[3015] = 1615; //minecraft:repeating_command_block:7
            LegacyToRuntimeId[3016] = 1616; //minecraft:repeating_command_block:8
            LegacyToRuntimeId[3017] = 1617; //minecraft:repeating_command_block:9
            LegacyToRuntimeId[3018] = 1618; //minecraft:repeating_command_block:10
            LegacyToRuntimeId[3019] = 1619; //minecraft:repeating_command_block:11
            LegacyToRuntimeId[3020] = 1620; //minecraft:repeating_command_block:12
            LegacyToRuntimeId[3021] = 1621; //minecraft:repeating_command_block:13
            LegacyToRuntimeId[3022] = 1622; //minecraft:repeating_command_block:14
            LegacyToRuntimeId[3023] = 1623; //minecraft:repeating_command_block:15
            LegacyToRuntimeId[3024] = 1624; //minecraft:chain_command_block:0
            LegacyToRuntimeId[3025] = 1625; //minecraft:chain_command_block:1
            LegacyToRuntimeId[3026] = 1626; //minecraft:chain_command_block:2
            LegacyToRuntimeId[3027] = 1627; //minecraft:chain_command_block:3
            LegacyToRuntimeId[3028] = 1628; //minecraft:chain_command_block:4
            LegacyToRuntimeId[3029] = 1629; //minecraft:chain_command_block:5
            LegacyToRuntimeId[3030] = 1630; //minecraft:chain_command_block:6
            LegacyToRuntimeId[3031] = 1631; //minecraft:chain_command_block:7
            LegacyToRuntimeId[3032] = 1632; //minecraft:chain_command_block:8
            LegacyToRuntimeId[3033] = 1633; //minecraft:chain_command_block:9
            LegacyToRuntimeId[3034] = 1634; //minecraft:chain_command_block:10
            LegacyToRuntimeId[3035] = 1635; //minecraft:chain_command_block:11
            LegacyToRuntimeId[3036] = 1636; //minecraft:chain_command_block:12
            LegacyToRuntimeId[3037] = 1637; //minecraft:chain_command_block:13
            LegacyToRuntimeId[3038] = 1638; //minecraft:chain_command_block:14
            LegacyToRuntimeId[3039] = 1639; //minecraft:chain_command_block:15
            LegacyToRuntimeId[3088] = 1643; //minecraft:spruce_door:0
            LegacyToRuntimeId[3089] = 1644; //minecraft:spruce_door:1
            LegacyToRuntimeId[3090] = 1645; //minecraft:spruce_door:2
            LegacyToRuntimeId[3091] = 1646; //minecraft:spruce_door:3
            LegacyToRuntimeId[3092] = 1647; //minecraft:spruce_door:4
            LegacyToRuntimeId[3093] = 1648; //minecraft:spruce_door:5
            LegacyToRuntimeId[3094] = 1649; //minecraft:spruce_door:6
            LegacyToRuntimeId[3095] = 1650; //minecraft:spruce_door:7
            LegacyToRuntimeId[3096] = 1651; //minecraft:spruce_door:8
            LegacyToRuntimeId[3097] = 1652; //minecraft:spruce_door:9
            LegacyToRuntimeId[3098] = 1653; //minecraft:spruce_door:10
            LegacyToRuntimeId[3099] = 1654; //minecraft:spruce_door:11
            LegacyToRuntimeId[3100] = 1655; //minecraft:spruce_door:12
            LegacyToRuntimeId[3101] = 1656; //minecraft:spruce_door:13
            LegacyToRuntimeId[3102] = 1657; //minecraft:spruce_door:14
            LegacyToRuntimeId[3103] = 1658; //minecraft:spruce_door:15
            LegacyToRuntimeId[3104] = 1659; //minecraft:birch_door:0
            LegacyToRuntimeId[3105] = 1660; //minecraft:birch_door:1
            LegacyToRuntimeId[3106] = 1661; //minecraft:birch_door:2
            LegacyToRuntimeId[3107] = 1662; //minecraft:birch_door:3
            LegacyToRuntimeId[3108] = 1663; //minecraft:birch_door:4
            LegacyToRuntimeId[3109] = 1664; //minecraft:birch_door:5
            LegacyToRuntimeId[3110] = 1665; //minecraft:birch_door:6
            LegacyToRuntimeId[3111] = 1666; //minecraft:birch_door:7
            LegacyToRuntimeId[3112] = 1667; //minecraft:birch_door:8
            LegacyToRuntimeId[3113] = 1668; //minecraft:birch_door:9
            LegacyToRuntimeId[3114] = 1669; //minecraft:birch_door:10
            LegacyToRuntimeId[3115] = 1670; //minecraft:birch_door:11
            LegacyToRuntimeId[3116] = 1671; //minecraft:birch_door:12
            LegacyToRuntimeId[3117] = 1672; //minecraft:birch_door:13
            LegacyToRuntimeId[3118] = 1673; //minecraft:birch_door:14
            LegacyToRuntimeId[3119] = 1674; //minecraft:birch_door:15
            LegacyToRuntimeId[3120] = 1675; //minecraft:jungle_door:0
            LegacyToRuntimeId[3121] = 1676; //minecraft:jungle_door:1
            LegacyToRuntimeId[3122] = 1677; //minecraft:jungle_door:2
            LegacyToRuntimeId[3123] = 1678; //minecraft:jungle_door:3
            LegacyToRuntimeId[3124] = 1679; //minecraft:jungle_door:4
            LegacyToRuntimeId[3125] = 1680; //minecraft:jungle_door:5
            LegacyToRuntimeId[3126] = 1681; //minecraft:jungle_door:6
            LegacyToRuntimeId[3127] = 1682; //minecraft:jungle_door:7
            LegacyToRuntimeId[3128] = 1683; //minecraft:jungle_door:8
            LegacyToRuntimeId[3129] = 1684; //minecraft:jungle_door:9
            LegacyToRuntimeId[3130] = 1685; //minecraft:jungle_door:10
            LegacyToRuntimeId[3131] = 1686; //minecraft:jungle_door:11
            LegacyToRuntimeId[3132] = 1687; //minecraft:jungle_door:12
            LegacyToRuntimeId[3133] = 1688; //minecraft:jungle_door:13
            LegacyToRuntimeId[3134] = 1689; //minecraft:jungle_door:14
            LegacyToRuntimeId[3135] = 1690; //minecraft:jungle_door:15
            LegacyToRuntimeId[3136] = 1691; //minecraft:acacia_door:0
            LegacyToRuntimeId[3137] = 1692; //minecraft:acacia_door:1
            LegacyToRuntimeId[3138] = 1693; //minecraft:acacia_door:2
            LegacyToRuntimeId[3139] = 1694; //minecraft:acacia_door:3
            LegacyToRuntimeId[3140] = 1695; //minecraft:acacia_door:4
            LegacyToRuntimeId[3141] = 1696; //minecraft:acacia_door:5
            LegacyToRuntimeId[3142] = 1697; //minecraft:acacia_door:6
            LegacyToRuntimeId[3143] = 1698; //minecraft:acacia_door:7
            LegacyToRuntimeId[3144] = 1699; //minecraft:acacia_door:8
            LegacyToRuntimeId[3145] = 1700; //minecraft:acacia_door:9
            LegacyToRuntimeId[3146] = 1701; //minecraft:acacia_door:10
            LegacyToRuntimeId[3147] = 1702; //minecraft:acacia_door:11
            LegacyToRuntimeId[3148] = 1703; //minecraft:acacia_door:12
            LegacyToRuntimeId[3149] = 1704; //minecraft:acacia_door:13
            LegacyToRuntimeId[3150] = 1705; //minecraft:acacia_door:14
            LegacyToRuntimeId[3151] = 1706; //minecraft:acacia_door:15
            LegacyToRuntimeId[3152] = 1707; //minecraft:dark_oak_door:0
            LegacyToRuntimeId[3153] = 1708; //minecraft:dark_oak_door:1
            LegacyToRuntimeId[3154] = 1709; //minecraft:dark_oak_door:2
            LegacyToRuntimeId[3155] = 1710; //minecraft:dark_oak_door:3
            LegacyToRuntimeId[3156] = 1711; //minecraft:dark_oak_door:4
            LegacyToRuntimeId[3157] = 1712; //minecraft:dark_oak_door:5
            LegacyToRuntimeId[3158] = 1713; //minecraft:dark_oak_door:6
            LegacyToRuntimeId[3159] = 1714; //minecraft:dark_oak_door:7
            LegacyToRuntimeId[3160] = 1715; //minecraft:dark_oak_door:8
            LegacyToRuntimeId[3161] = 1716; //minecraft:dark_oak_door:9
            LegacyToRuntimeId[3162] = 1717; //minecraft:dark_oak_door:10
            LegacyToRuntimeId[3163] = 1718; //minecraft:dark_oak_door:11
            LegacyToRuntimeId[3164] = 1719; //minecraft:dark_oak_door:12
            LegacyToRuntimeId[3165] = 1720; //minecraft:dark_oak_door:13
            LegacyToRuntimeId[3166] = 1721; //minecraft:dark_oak_door:14
            LegacyToRuntimeId[3167] = 1722; //minecraft:dark_oak_door:15
            LegacyToRuntimeId[3168] = 1723; //minecraft:grass_path:0
            LegacyToRuntimeId[3184] = 1724; //minecraft:frame:0
            LegacyToRuntimeId[3185] = 1725; //minecraft:frame:1
            LegacyToRuntimeId[3186] = 1726; //minecraft:frame:2
            LegacyToRuntimeId[3187] = 1727; //minecraft:frame:3
            LegacyToRuntimeId[3200] = 1728; //minecraft:chorus_flower:0
            LegacyToRuntimeId[3201] = 1729; //minecraft:chorus_flower:1
            LegacyToRuntimeId[3202] = 1730; //minecraft:chorus_flower:2
            LegacyToRuntimeId[3203] = 1731; //minecraft:chorus_flower:3
            LegacyToRuntimeId[3204] = 1732; //minecraft:chorus_flower:4
            LegacyToRuntimeId[3205] = 1733; //minecraft:chorus_flower:5
            LegacyToRuntimeId[3206] = 1734; //minecraft:chorus_flower:6
            LegacyToRuntimeId[3207] = 1735; //minecraft:chorus_flower:7
            LegacyToRuntimeId[3216] = 1736; //minecraft:purpur_block:0
            LegacyToRuntimeId[3217] = 1737; //minecraft:purpur_block:1
            LegacyToRuntimeId[3218] = 1738; //minecraft:purpur_block:2
            LegacyToRuntimeId[3219] = 1739; //minecraft:purpur_block:3
            LegacyToRuntimeId[3220] = 1740; //minecraft:purpur_block:4
            LegacyToRuntimeId[3221] = 1741; //minecraft:purpur_block:5
            LegacyToRuntimeId[3222] = 1742; //minecraft:purpur_block:6
            LegacyToRuntimeId[3223] = 1743; //minecraft:purpur_block:7
            LegacyToRuntimeId[3224] = 1744; //minecraft:purpur_block:8
            LegacyToRuntimeId[3225] = 1745; //minecraft:purpur_block:9
            LegacyToRuntimeId[3226] = 1746; //minecraft:purpur_block:10
            LegacyToRuntimeId[3227] = 1747; //minecraft:purpur_block:11
            LegacyToRuntimeId[3228] = 1748; //minecraft:purpur_block:12
            LegacyToRuntimeId[3229] = 1749; //minecraft:purpur_block:13
            LegacyToRuntimeId[3230] = 1750; //minecraft:purpur_block:14
            LegacyToRuntimeId[3231] = 1751; //minecraft:purpur_block:15
            LegacyToRuntimeId[3248] = 1753; //minecraft:purpur_stairs:0
            LegacyToRuntimeId[3249] = 1754; //minecraft:purpur_stairs:1
            LegacyToRuntimeId[3250] = 1755; //minecraft:purpur_stairs:2
            LegacyToRuntimeId[3251] = 1756; //minecraft:purpur_stairs:3
            LegacyToRuntimeId[3252] = 1757; //minecraft:purpur_stairs:4
            LegacyToRuntimeId[3253] = 1758; //minecraft:purpur_stairs:5
            LegacyToRuntimeId[3254] = 1759; //minecraft:purpur_stairs:6
            LegacyToRuntimeId[3255] = 1760; //minecraft:purpur_stairs:7
            LegacyToRuntimeId[3280] = 1762; //minecraft:undyed_shulker_box:0
            LegacyToRuntimeId[3296] = 1763; //minecraft:end_bricks:0
            LegacyToRuntimeId[3312] = 1764; //minecraft:frosted_ice:0
            LegacyToRuntimeId[3313] = 1765; //minecraft:frosted_ice:1
            LegacyToRuntimeId[3314] = 1766; //minecraft:frosted_ice:2
            LegacyToRuntimeId[3315] = 1767; //minecraft:frosted_ice:3
            LegacyToRuntimeId[3328] = 1768; //minecraft:end_rod:0
            LegacyToRuntimeId[3329] = 1769; //minecraft:end_rod:1
            LegacyToRuntimeId[3330] = 1770; //minecraft:end_rod:2
            LegacyToRuntimeId[3331] = 1771; //minecraft:end_rod:3
            LegacyToRuntimeId[3332] = 1772; //minecraft:end_rod:4
            LegacyToRuntimeId[3333] = 1773; //minecraft:end_rod:5
            LegacyToRuntimeId[3334] = 1774; //minecraft:end_rod:6
            LegacyToRuntimeId[3335] = 1775; //minecraft:end_rod:7
            LegacyToRuntimeId[3344] = 1776; //minecraft:end_gateway:0
            LegacyToRuntimeId[3408] = 1780; //minecraft:magma:0
            LegacyToRuntimeId[3424] = 1781; //minecraft:nether_wart_block:0
            LegacyToRuntimeId[3440] = 1782; //minecraft:red_nether_brick:0
            LegacyToRuntimeId[3456] = 1783; //minecraft:bone_block:0
            LegacyToRuntimeId[3457] = 1784; //minecraft:bone_block:1
            LegacyToRuntimeId[3458] = 1785; //minecraft:bone_block:2
            LegacyToRuntimeId[3459] = 1786; //minecraft:bone_block:3
            LegacyToRuntimeId[3460] = 1787; //minecraft:bone_block:4
            LegacyToRuntimeId[3461] = 1788; //minecraft:bone_block:5
            LegacyToRuntimeId[3462] = 1789; //minecraft:bone_block:6
            LegacyToRuntimeId[3463] = 1790; //minecraft:bone_block:7
            LegacyToRuntimeId[3464] = 1791; //minecraft:bone_block:8
            LegacyToRuntimeId[3465] = 1792; //minecraft:bone_block:9
            LegacyToRuntimeId[3466] = 1793; //minecraft:bone_block:10
            LegacyToRuntimeId[3467] = 1794; //minecraft:bone_block:11
            LegacyToRuntimeId[3468] = 1795; //minecraft:bone_block:12
            LegacyToRuntimeId[3469] = 1796; //minecraft:bone_block:13
            LegacyToRuntimeId[3470] = 1797; //minecraft:bone_block:14
            LegacyToRuntimeId[3471] = 1798; //minecraft:bone_block:15
            LegacyToRuntimeId[3488] = 1800; //minecraft:shulker_box:0
            LegacyToRuntimeId[3489] = 1801; //minecraft:shulker_box:1
            LegacyToRuntimeId[3490] = 1802; //minecraft:shulker_box:2
            LegacyToRuntimeId[3491] = 1803; //minecraft:shulker_box:3
            LegacyToRuntimeId[3492] = 1804; //minecraft:shulker_box:4
            LegacyToRuntimeId[3493] = 1805; //minecraft:shulker_box:5
            LegacyToRuntimeId[3494] = 1806; //minecraft:shulker_box:6
            LegacyToRuntimeId[3495] = 1807; //minecraft:shulker_box:7
            LegacyToRuntimeId[3496] = 1808; //minecraft:shulker_box:8
            LegacyToRuntimeId[3497] = 1809; //minecraft:shulker_box:9
            LegacyToRuntimeId[3498] = 1810; //minecraft:shulker_box:10
            LegacyToRuntimeId[3499] = 1811; //minecraft:shulker_box:11
            LegacyToRuntimeId[3500] = 1812; //minecraft:shulker_box:12
            LegacyToRuntimeId[3501] = 1813; //minecraft:shulker_box:13
            LegacyToRuntimeId[3502] = 1814; //minecraft:shulker_box:14
            LegacyToRuntimeId[3503] = 1815; //minecraft:shulker_box:15
            LegacyToRuntimeId[3504] = 1816; //minecraft:purple_glazed_terracotta:0
            LegacyToRuntimeId[3505] = 1817; //minecraft:purple_glazed_terracotta:1
            LegacyToRuntimeId[3506] = 1818; //minecraft:purple_glazed_terracotta:2
            LegacyToRuntimeId[3507] = 1819; //minecraft:purple_glazed_terracotta:3
            LegacyToRuntimeId[3508] = 1820; //minecraft:purple_glazed_terracotta:4
            LegacyToRuntimeId[3509] = 1821; //minecraft:purple_glazed_terracotta:5
            LegacyToRuntimeId[3510] = 1822; //minecraft:purple_glazed_terracotta:6
            LegacyToRuntimeId[3511] = 1823; //minecraft:purple_glazed_terracotta:7
            LegacyToRuntimeId[3520] = 1824; //minecraft:white_glazed_terracotta:0
            LegacyToRuntimeId[3521] = 1825; //minecraft:white_glazed_terracotta:1
            LegacyToRuntimeId[3522] = 1826; //minecraft:white_glazed_terracotta:2
            LegacyToRuntimeId[3523] = 1827; //minecraft:white_glazed_terracotta:3
            LegacyToRuntimeId[3524] = 1828; //minecraft:white_glazed_terracotta:4
            LegacyToRuntimeId[3525] = 1829; //minecraft:white_glazed_terracotta:5
            LegacyToRuntimeId[3526] = 1830; //minecraft:white_glazed_terracotta:6
            LegacyToRuntimeId[3527] = 1831; //minecraft:white_glazed_terracotta:7
            LegacyToRuntimeId[3536] = 1832; //minecraft:orange_glazed_terracotta:0
            LegacyToRuntimeId[3537] = 1833; //minecraft:orange_glazed_terracotta:1
            LegacyToRuntimeId[3538] = 1834; //minecraft:orange_glazed_terracotta:2
            LegacyToRuntimeId[3539] = 1835; //minecraft:orange_glazed_terracotta:3
            LegacyToRuntimeId[3540] = 1836; //minecraft:orange_glazed_terracotta:4
            LegacyToRuntimeId[3541] = 1837; //minecraft:orange_glazed_terracotta:5
            LegacyToRuntimeId[3542] = 1838; //minecraft:orange_glazed_terracotta:6
            LegacyToRuntimeId[3543] = 1839; //minecraft:orange_glazed_terracotta:7
            LegacyToRuntimeId[3552] = 1840; //minecraft:magenta_glazed_terracotta:0
            LegacyToRuntimeId[3553] = 1841; //minecraft:magenta_glazed_terracotta:1
            LegacyToRuntimeId[3554] = 1842; //minecraft:magenta_glazed_terracotta:2
            LegacyToRuntimeId[3555] = 1843; //minecraft:magenta_glazed_terracotta:3
            LegacyToRuntimeId[3556] = 1844; //minecraft:magenta_glazed_terracotta:4
            LegacyToRuntimeId[3557] = 1845; //minecraft:magenta_glazed_terracotta:5
            LegacyToRuntimeId[3558] = 1846; //minecraft:magenta_glazed_terracotta:6
            LegacyToRuntimeId[3559] = 1847; //minecraft:magenta_glazed_terracotta:7
            LegacyToRuntimeId[3568] = 1848; //minecraft:light_blue_glazed_terracotta:0
            LegacyToRuntimeId[3569] = 1849; //minecraft:light_blue_glazed_terracotta:1
            LegacyToRuntimeId[3570] = 1850; //minecraft:light_blue_glazed_terracotta:2
            LegacyToRuntimeId[3571] = 1851; //minecraft:light_blue_glazed_terracotta:3
            LegacyToRuntimeId[3572] = 1852; //minecraft:light_blue_glazed_terracotta:4
            LegacyToRuntimeId[3573] = 1853; //minecraft:light_blue_glazed_terracotta:5
            LegacyToRuntimeId[3574] = 1854; //minecraft:light_blue_glazed_terracotta:6
            LegacyToRuntimeId[3575] = 1855; //minecraft:light_blue_glazed_terracotta:7
            LegacyToRuntimeId[3584] = 1856; //minecraft:yellow_glazed_terracotta:0
            LegacyToRuntimeId[3585] = 1857; //minecraft:yellow_glazed_terracotta:1
            LegacyToRuntimeId[3586] = 1858; //minecraft:yellow_glazed_terracotta:2
            LegacyToRuntimeId[3587] = 1859; //minecraft:yellow_glazed_terracotta:3
            LegacyToRuntimeId[3588] = 1860; //minecraft:yellow_glazed_terracotta:4
            LegacyToRuntimeId[3589] = 1861; //minecraft:yellow_glazed_terracotta:5
            LegacyToRuntimeId[3590] = 1862; //minecraft:yellow_glazed_terracotta:6
            LegacyToRuntimeId[3591] = 1863; //minecraft:yellow_glazed_terracotta:7
            LegacyToRuntimeId[3600] = 1864; //minecraft:lime_glazed_terracotta:0
            LegacyToRuntimeId[3601] = 1865; //minecraft:lime_glazed_terracotta:1
            LegacyToRuntimeId[3602] = 1866; //minecraft:lime_glazed_terracotta:2
            LegacyToRuntimeId[3603] = 1867; //minecraft:lime_glazed_terracotta:3
            LegacyToRuntimeId[3604] = 1868; //minecraft:lime_glazed_terracotta:4
            LegacyToRuntimeId[3605] = 1869; //minecraft:lime_glazed_terracotta:5
            LegacyToRuntimeId[3606] = 1870; //minecraft:lime_glazed_terracotta:6
            LegacyToRuntimeId[3607] = 1871; //minecraft:lime_glazed_terracotta:7
            LegacyToRuntimeId[3616] = 1872; //minecraft:pink_glazed_terracotta:0
            LegacyToRuntimeId[3617] = 1873; //minecraft:pink_glazed_terracotta:1
            LegacyToRuntimeId[3618] = 1874; //minecraft:pink_glazed_terracotta:2
            LegacyToRuntimeId[3619] = 1875; //minecraft:pink_glazed_terracotta:3
            LegacyToRuntimeId[3620] = 1876; //minecraft:pink_glazed_terracotta:4
            LegacyToRuntimeId[3621] = 1877; //minecraft:pink_glazed_terracotta:5
            LegacyToRuntimeId[3622] = 1878; //minecraft:pink_glazed_terracotta:6
            LegacyToRuntimeId[3623] = 1879; //minecraft:pink_glazed_terracotta:7
            LegacyToRuntimeId[3632] = 1880; //minecraft:gray_glazed_terracotta:0
            LegacyToRuntimeId[3633] = 1881; //minecraft:gray_glazed_terracotta:1
            LegacyToRuntimeId[3634] = 1882; //minecraft:gray_glazed_terracotta:2
            LegacyToRuntimeId[3635] = 1883; //minecraft:gray_glazed_terracotta:3
            LegacyToRuntimeId[3636] = 1884; //minecraft:gray_glazed_terracotta:4
            LegacyToRuntimeId[3637] = 1885; //minecraft:gray_glazed_terracotta:5
            LegacyToRuntimeId[3638] = 1886; //minecraft:gray_glazed_terracotta:6
            LegacyToRuntimeId[3639] = 1887; //minecraft:gray_glazed_terracotta:7
            LegacyToRuntimeId[3648] = 1888; //minecraft:silver_glazed_terracotta:0
            LegacyToRuntimeId[3649] = 1889; //minecraft:silver_glazed_terracotta:1
            LegacyToRuntimeId[3650] = 1890; //minecraft:silver_glazed_terracotta:2
            LegacyToRuntimeId[3651] = 1891; //minecraft:silver_glazed_terracotta:3
            LegacyToRuntimeId[3652] = 1892; //minecraft:silver_glazed_terracotta:4
            LegacyToRuntimeId[3653] = 1893; //minecraft:silver_glazed_terracotta:5
            LegacyToRuntimeId[3654] = 1894; //minecraft:silver_glazed_terracotta:6
            LegacyToRuntimeId[3655] = 1895; //minecraft:silver_glazed_terracotta:7
            LegacyToRuntimeId[3664] = 1896; //minecraft:cyan_glazed_terracotta:0
            LegacyToRuntimeId[3665] = 1897; //minecraft:cyan_glazed_terracotta:1
            LegacyToRuntimeId[3666] = 1898; //minecraft:cyan_glazed_terracotta:2
            LegacyToRuntimeId[3667] = 1899; //minecraft:cyan_glazed_terracotta:3
            LegacyToRuntimeId[3668] = 1900; //minecraft:cyan_glazed_terracotta:4
            LegacyToRuntimeId[3669] = 1901; //minecraft:cyan_glazed_terracotta:5
            LegacyToRuntimeId[3670] = 1902; //minecraft:cyan_glazed_terracotta:6
            LegacyToRuntimeId[3671] = 1903; //minecraft:cyan_glazed_terracotta:7
            LegacyToRuntimeId[3696] = 1905; //minecraft:blue_glazed_terracotta:0
            LegacyToRuntimeId[3697] = 1906; //minecraft:blue_glazed_terracotta:1
            LegacyToRuntimeId[3698] = 1907; //minecraft:blue_glazed_terracotta:2
            LegacyToRuntimeId[3699] = 1908; //minecraft:blue_glazed_terracotta:3
            LegacyToRuntimeId[3700] = 1909; //minecraft:blue_glazed_terracotta:4
            LegacyToRuntimeId[3701] = 1910; //minecraft:blue_glazed_terracotta:5
            LegacyToRuntimeId[3702] = 1911; //minecraft:blue_glazed_terracotta:6
            LegacyToRuntimeId[3703] = 1912; //minecraft:blue_glazed_terracotta:7
            LegacyToRuntimeId[3712] = 1913; //minecraft:brown_glazed_terracotta:0
            LegacyToRuntimeId[3713] = 1914; //minecraft:brown_glazed_terracotta:1
            LegacyToRuntimeId[3714] = 1915; //minecraft:brown_glazed_terracotta:2
            LegacyToRuntimeId[3715] = 1916; //minecraft:brown_glazed_terracotta:3
            LegacyToRuntimeId[3716] = 1917; //minecraft:brown_glazed_terracotta:4
            LegacyToRuntimeId[3717] = 1918; //minecraft:brown_glazed_terracotta:5
            LegacyToRuntimeId[3718] = 1919; //minecraft:brown_glazed_terracotta:6
            LegacyToRuntimeId[3719] = 1920; //minecraft:brown_glazed_terracotta:7
            LegacyToRuntimeId[3728] = 1921; //minecraft:green_glazed_terracotta:0
            LegacyToRuntimeId[3729] = 1922; //minecraft:green_glazed_terracotta:1
            LegacyToRuntimeId[3730] = 1923; //minecraft:green_glazed_terracotta:2
            LegacyToRuntimeId[3731] = 1924; //minecraft:green_glazed_terracotta:3
            LegacyToRuntimeId[3732] = 1925; //minecraft:green_glazed_terracotta:4
            LegacyToRuntimeId[3733] = 1926; //minecraft:green_glazed_terracotta:5
            LegacyToRuntimeId[3734] = 1927; //minecraft:green_glazed_terracotta:6
            LegacyToRuntimeId[3735] = 1928; //minecraft:green_glazed_terracotta:7
            LegacyToRuntimeId[3744] = 1929; //minecraft:red_glazed_terracotta:0
            LegacyToRuntimeId[3745] = 1930; //minecraft:red_glazed_terracotta:1
            LegacyToRuntimeId[3746] = 1931; //minecraft:red_glazed_terracotta:2
            LegacyToRuntimeId[3747] = 1932; //minecraft:red_glazed_terracotta:3
            LegacyToRuntimeId[3748] = 1933; //minecraft:red_glazed_terracotta:4
            LegacyToRuntimeId[3749] = 1934; //minecraft:red_glazed_terracotta:5
            LegacyToRuntimeId[3750] = 1935; //minecraft:red_glazed_terracotta:6
            LegacyToRuntimeId[3751] = 1936; //minecraft:red_glazed_terracotta:7
            LegacyToRuntimeId[3760] = 1937; //minecraft:black_glazed_terracotta:0
            LegacyToRuntimeId[3761] = 1938; //minecraft:black_glazed_terracotta:1
            LegacyToRuntimeId[3762] = 1939; //minecraft:black_glazed_terracotta:2
            LegacyToRuntimeId[3763] = 1940; //minecraft:black_glazed_terracotta:3
            LegacyToRuntimeId[3764] = 1941; //minecraft:black_glazed_terracotta:4
            LegacyToRuntimeId[3765] = 1942; //minecraft:black_glazed_terracotta:5
            LegacyToRuntimeId[3766] = 1943; //minecraft:black_glazed_terracotta:6
            LegacyToRuntimeId[3767] = 1944; //minecraft:black_glazed_terracotta:7
            LegacyToRuntimeId[3776] = 1945; //minecraft:concrete:0
            LegacyToRuntimeId[3777] = 1946; //minecraft:concrete:1
            LegacyToRuntimeId[3778] = 1947; //minecraft:concrete:2
            LegacyToRuntimeId[3779] = 1948; //minecraft:concrete:3
            LegacyToRuntimeId[3780] = 1949; //minecraft:concrete:4
            LegacyToRuntimeId[3781] = 1950; //minecraft:concrete:5
            LegacyToRuntimeId[3782] = 1951; //minecraft:concrete:6
            LegacyToRuntimeId[3783] = 1952; //minecraft:concrete:7
            LegacyToRuntimeId[3784] = 1953; //minecraft:concrete:8
            LegacyToRuntimeId[3785] = 1954; //minecraft:concrete:9
            LegacyToRuntimeId[3786] = 1955; //minecraft:concrete:10
            LegacyToRuntimeId[3787] = 1956; //minecraft:concrete:11
            LegacyToRuntimeId[3788] = 1957; //minecraft:concrete:12
            LegacyToRuntimeId[3789] = 1958; //minecraft:concrete:13
            LegacyToRuntimeId[3790] = 1959; //minecraft:concrete:14
            LegacyToRuntimeId[3791] = 1960; //minecraft:concrete:15
            LegacyToRuntimeId[3792] = 1961; //minecraft:concretePowder:0
            LegacyToRuntimeId[3793] = 1962; //minecraft:concretePowder:1
            LegacyToRuntimeId[3794] = 1963; //minecraft:concretePowder:2
            LegacyToRuntimeId[3795] = 1964; //minecraft:concretePowder:3
            LegacyToRuntimeId[3796] = 1965; //minecraft:concretePowder:4
            LegacyToRuntimeId[3797] = 1966; //minecraft:concretePowder:5
            LegacyToRuntimeId[3798] = 1967; //minecraft:concretePowder:6
            LegacyToRuntimeId[3799] = 1968; //minecraft:concretePowder:7
            LegacyToRuntimeId[3800] = 1969; //minecraft:concretePowder:8
            LegacyToRuntimeId[3801] = 1970; //minecraft:concretePowder:9
            LegacyToRuntimeId[3802] = 1971; //minecraft:concretePowder:10
            LegacyToRuntimeId[3803] = 1972; //minecraft:concretePowder:11
            LegacyToRuntimeId[3804] = 1973; //minecraft:concretePowder:12
            LegacyToRuntimeId[3805] = 1974; //minecraft:concretePowder:13
            LegacyToRuntimeId[3806] = 1975; //minecraft:concretePowder:14
            LegacyToRuntimeId[3807] = 1976; //minecraft:concretePowder:15
            LegacyToRuntimeId[3840] = 1979; //minecraft:chorus_plant:0
            LegacyToRuntimeId[3856] = 1980; //minecraft:stained_glass:0
            LegacyToRuntimeId[3857] = 1981; //minecraft:stained_glass:1
            LegacyToRuntimeId[3858] = 1982; //minecraft:stained_glass:2
            LegacyToRuntimeId[3859] = 1983; //minecraft:stained_glass:3
            LegacyToRuntimeId[3860] = 1984; //minecraft:stained_glass:4
            LegacyToRuntimeId[3861] = 1985; //minecraft:stained_glass:5
            LegacyToRuntimeId[3862] = 1986; //minecraft:stained_glass:6
            LegacyToRuntimeId[3863] = 1987; //minecraft:stained_glass:7
            LegacyToRuntimeId[3864] = 1988; //minecraft:stained_glass:8
            LegacyToRuntimeId[3865] = 1989; //minecraft:stained_glass:9
            LegacyToRuntimeId[3866] = 1990; //minecraft:stained_glass:10
            LegacyToRuntimeId[3867] = 1991; //minecraft:stained_glass:11
            LegacyToRuntimeId[3868] = 1992; //minecraft:stained_glass:12
            LegacyToRuntimeId[3869] = 1993; //minecraft:stained_glass:13
            LegacyToRuntimeId[3870] = 1994; //minecraft:stained_glass:14
            LegacyToRuntimeId[3871] = 1995; //minecraft:stained_glass:15
            LegacyToRuntimeId[3888] = 1997; //minecraft:podzol:0
            LegacyToRuntimeId[3904] = 1998; //minecraft:beetroot:0
            LegacyToRuntimeId[3905] = 1999; //minecraft:beetroot:1
            LegacyToRuntimeId[3906] = 2000; //minecraft:beetroot:2
            LegacyToRuntimeId[3907] = 2001; //minecraft:beetroot:3
            LegacyToRuntimeId[3908] = 2002; //minecraft:beetroot:4
            LegacyToRuntimeId[3909] = 2003; //minecraft:beetroot:5
            LegacyToRuntimeId[3910] = 2004; //minecraft:beetroot:6
            LegacyToRuntimeId[3911] = 2005; //minecraft:beetroot:7
            LegacyToRuntimeId[3920] = 2006; //minecraft:stonecutter:0
            LegacyToRuntimeId[3936] = 2007; //minecraft:glowingobsidian:0
            LegacyToRuntimeId[3952] = 2008; //minecraft:netherreactor:0
            LegacyToRuntimeId[3968] = 2009; //minecraft:info_update:0
            LegacyToRuntimeId[3984] = 2010; //minecraft:info_update2:0
            LegacyToRuntimeId[4000] = 2011; //minecraft:movingBlock:0
            LegacyToRuntimeId[4016] = 2012; //minecraft:observer:0
            LegacyToRuntimeId[4017] = 2013; //minecraft:observer:1
            LegacyToRuntimeId[4018] = 2014; //minecraft:observer:2
            LegacyToRuntimeId[4019] = 2015; //minecraft:observer:3
            LegacyToRuntimeId[4020] = 2016; //minecraft:observer:4
            LegacyToRuntimeId[4021] = 2017; //minecraft:observer:5
            LegacyToRuntimeId[4022] = 2018; //minecraft:observer:6
            LegacyToRuntimeId[4023] = 2019; //minecraft:observer:7
            LegacyToRuntimeId[4024] = 2020; //minecraft:observer:8
            LegacyToRuntimeId[4025] = 2021; //minecraft:observer:9
            LegacyToRuntimeId[4026] = 2022; //minecraft:observer:10
            LegacyToRuntimeId[4027] = 2023; //minecraft:observer:11
            LegacyToRuntimeId[4028] = 2024; //minecraft:observer:12
            LegacyToRuntimeId[4029] = 2025; //minecraft:observer:13
            LegacyToRuntimeId[4030] = 2026; //minecraft:observer:14
            LegacyToRuntimeId[4031] = 2027; //minecraft:observer:15
            LegacyToRuntimeId[4032] = 2028; //minecraft:structure_block:0
            LegacyToRuntimeId[4033] = 2029; //minecraft:structure_block:1
            LegacyToRuntimeId[4034] = 2030; //minecraft:structure_block:2
            LegacyToRuntimeId[4035] = 2031; //minecraft:structure_block:3
            LegacyToRuntimeId[4036] = 2032; //minecraft:structure_block:4
            LegacyToRuntimeId[4037] = 2033; //minecraft:structure_block:5
            LegacyToRuntimeId[4038] = 2034; //minecraft:structure_block:6
            LegacyToRuntimeId[4039] = 2035; //minecraft:structure_block:7
            LegacyToRuntimeId[4080] = 2038; //minecraft:reserved6:0
            LegacyToRuntimeId[4112] = 2040; //minecraft:prismarine_stairs:0
            LegacyToRuntimeId[4113] = 2041; //minecraft:prismarine_stairs:1
            LegacyToRuntimeId[4114] = 2042; //minecraft:prismarine_stairs:2
            LegacyToRuntimeId[4115] = 2043; //minecraft:prismarine_stairs:3
            LegacyToRuntimeId[4116] = 2044; //minecraft:prismarine_stairs:4
            LegacyToRuntimeId[4117] = 2045; //minecraft:prismarine_stairs:5
            LegacyToRuntimeId[4118] = 2046; //minecraft:prismarine_stairs:6
            LegacyToRuntimeId[4119] = 2047; //minecraft:prismarine_stairs:7
            LegacyToRuntimeId[4128] = 2048; //minecraft:dark_prismarine_stairs:0
            LegacyToRuntimeId[4129] = 2049; //minecraft:dark_prismarine_stairs:1
            LegacyToRuntimeId[4130] = 2050; //minecraft:dark_prismarine_stairs:2
            LegacyToRuntimeId[4131] = 2051; //minecraft:dark_prismarine_stairs:3
            LegacyToRuntimeId[4132] = 2052; //minecraft:dark_prismarine_stairs:4
            LegacyToRuntimeId[4133] = 2053; //minecraft:dark_prismarine_stairs:5
            LegacyToRuntimeId[4134] = 2054; //minecraft:dark_prismarine_stairs:6
            LegacyToRuntimeId[4135] = 2055; //minecraft:dark_prismarine_stairs:7
            LegacyToRuntimeId[4144] = 2056; //minecraft:prismarine_bricks_stairs:0
            LegacyToRuntimeId[4145] = 2057; //minecraft:prismarine_bricks_stairs:1
            LegacyToRuntimeId[4146] = 2058; //minecraft:prismarine_bricks_stairs:2
            LegacyToRuntimeId[4147] = 2059; //minecraft:prismarine_bricks_stairs:3
            LegacyToRuntimeId[4148] = 2060; //minecraft:prismarine_bricks_stairs:4
            LegacyToRuntimeId[4149] = 2061; //minecraft:prismarine_bricks_stairs:5
            LegacyToRuntimeId[4150] = 2062; //minecraft:prismarine_bricks_stairs:6
            LegacyToRuntimeId[4151] = 2063; //minecraft:prismarine_bricks_stairs:7
            LegacyToRuntimeId[4160] = 2064; //minecraft:stripped_spruce_log:0
            LegacyToRuntimeId[4161] = 2065; //minecraft:stripped_spruce_log:1
            LegacyToRuntimeId[4162] = 2066; //minecraft:stripped_spruce_log:2
            LegacyToRuntimeId[4163] = 2067; //minecraft:stripped_spruce_log:3
            LegacyToRuntimeId[4176] = 2068; //minecraft:stripped_birch_log:0
            LegacyToRuntimeId[4177] = 2069; //minecraft:stripped_birch_log:1
            LegacyToRuntimeId[4178] = 2070; //minecraft:stripped_birch_log:2
            LegacyToRuntimeId[4179] = 2071; //minecraft:stripped_birch_log:3
            LegacyToRuntimeId[4192] = 2072; //minecraft:stripped_jungle_log:0
            LegacyToRuntimeId[4193] = 2073; //minecraft:stripped_jungle_log:1
            LegacyToRuntimeId[4194] = 2074; //minecraft:stripped_jungle_log:2
            LegacyToRuntimeId[4195] = 2075; //minecraft:stripped_jungle_log:3
            LegacyToRuntimeId[4208] = 2076; //minecraft:stripped_acacia_log:0
            LegacyToRuntimeId[4209] = 2077; //minecraft:stripped_acacia_log:1
            LegacyToRuntimeId[4210] = 2078; //minecraft:stripped_acacia_log:2
            LegacyToRuntimeId[4211] = 2079; //minecraft:stripped_acacia_log:3
            LegacyToRuntimeId[4224] = 2080; //minecraft:stripped_dark_oak_log:0
            LegacyToRuntimeId[4225] = 2081; //minecraft:stripped_dark_oak_log:1
            LegacyToRuntimeId[4226] = 2082; //minecraft:stripped_dark_oak_log:2
            LegacyToRuntimeId[4227] = 2083; //minecraft:stripped_dark_oak_log:3
            LegacyToRuntimeId[4240] = 2084; //minecraft:stripped_oak_log:0
            LegacyToRuntimeId[4241] = 2085; //minecraft:stripped_oak_log:1
            LegacyToRuntimeId[4242] = 2086; //minecraft:stripped_oak_log:2
            LegacyToRuntimeId[4243] = 2087; //minecraft:stripped_oak_log:3
        }

        public static uint GetRuntimeId(byte blockId, byte metadata)
        {
            int idx = TryGetRuntimeId(blockId, metadata);
            if (idx != -1) return (uint)idx;

            //block found with bad metadata, try getting with zero
            idx = TryGetRuntimeId(blockId, 0);
            if (idx != -1) return (uint)idx;

            return (uint)TryGetRuntimeId(248, 0); //legacy id for info_update block (for unknown block)
        }

        private static int TryGetRuntimeId(byte blockId, byte metadata)
        {
            return LegacyToRuntimeId[(blockId << 4) | metadata];
        }

    }
=======
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

>>>>>>> 86f35b43910890e118cedd4a207ba5d5e79c1298
}