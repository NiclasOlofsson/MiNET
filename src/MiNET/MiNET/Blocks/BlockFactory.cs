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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2019 Niclas Olofsson.
// All Rights Reserved.

#endregion

using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using log4net;
using MiNET.Utils;
using Newtonsoft.Json;
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
		public static BlockPallet BlockPallet { get; set; } = new BlockPallet();

		public static int[] LegacyToRuntimeId = new int[65536];

		//public static Dictionary<(int, int), int> BlockStates = new Dictionary<(int, int), int>();

		static BlockFactory()
		{
			for (int i = 0; i < byte.MaxValue * 2; i++)
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

			using (var stream = assembly.GetManifestResourceStream(typeof(Block).Namespace + ".blockstates.json"))
			using (var reader = new StreamReader(stream))
			{
				BlockPallet = BlockPallet.FromJson(reader.ReadToEnd());
			}
			int palletSize = BlockPallet.Count;
			for (int i = 0; i < palletSize; i++)
			{
				if (BlockPallet[i].Data > 15) continue; // TODO: figure out why pallet contains blocks with meta more than 15
				LegacyToRuntimeId[(BlockPallet[i].Id << 4) | (byte) BlockPallet[i].Data] = i;
			}
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
			else if (blockId == 34) block = new PistonArmCollision();
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
			else if (blockId == 247) block = new Netherreactor();
			else if (blockId == 251) block = new Observer();

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
			else if (blockId == 416) block = new Barrier();
			else if (blockId == 417) block = new StoneSlab3();
			else if (blockId == 418) block = new Bamboo();
			else if (blockId == 419) block = new BambooSapling();
			else if (blockId == 420) block = new Scaffolding();
			else if (blockId == 421) block = new StoneSlab4();
			else if (blockId == 422) block = new DoubleStoneSlab3();
			else if (blockId == 423) block = new DoubleStoneSlab4();
			else if (blockId == 436) block = new SpruceStandingSign();
			else if (blockId == 437) block = new SpruceWallSign();
			else if (blockId == 438) block = new SmoothStone();
			else if (blockId == 441) block = new BirchStandingSign();
			else if (blockId == 442) block = new BirchWallSign();
			else if (blockId == 443) block = new JungleStandingSign();
			else if (blockId == 444) block = new JungleWallSign();
			else if (blockId == 445) block = new AcaciaStandingSign();
			else if (blockId == 446) block = new AcaciaWallSign();
			else if (blockId == 447) block = new DarkoakStandingSign();
			else if (blockId == 448) block = new DarkoakWallSign();
			else if (blockId == 450) block = new Grindstone();
			else if (blockId == 451) block = new BlastFurnace();
			else if (blockId == 453) block = new Smoker();
			else if (blockId == 454) block = new LitSmoker();
			else if (blockId == 469) block = new LitBlastFurnace();
			else if (blockId == 455) block = new CartographyTable();
			else if (blockId == 456) block = new FletchingTable();
			else if (blockId == 457) block = new SmithingTable();
			else if (blockId == 458) block = new Barrel();
			else if (blockId == 461) block = new Bell();
			else if (blockId == 463) block = new Lantern();
			else if (blockId == 465) block = new LavaCauldron();

			else if (blockId == 449) block = new Lectern();
			else if (blockId == 452) block = new StonecutterBlock();
			else if (blockId == 459) block = new Loom();
			else if (blockId == 462) block = new SweetBerryBush();
			else if (blockId == 464) block = new Campfire();
			else if (blockId == 466) block = new Jigsaw();
			else if (blockId == 467) block = new Wood();
			else if (blockId == 468) block = new Composter();

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

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint GetRuntimeId(int blockId, byte metadata)
		{
			int idx = TryGetRuntimeId(blockId, metadata);
			if (idx != -1)
			{
				return (uint) idx;
			}

			//block found with bad metadata, try getting with zero
			int idx2 = TryGetRuntimeId(blockId, 0);
			if (idx2 != -1)
			{
				return (uint) idx2;
			}

			return (uint) TryGetRuntimeId(248, 0); //legacy id for info_update block (for unknown block)
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int TryGetRuntimeId(int blockId, byte metadata)
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