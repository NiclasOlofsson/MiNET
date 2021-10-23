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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2020 Niclas Olofsson.
// All Rights Reserved.

#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using fNbt;
using log4net;
using MiNET.Items;
using MiNET.Net;
using MiNET.Utils;
using Newtonsoft.Json;

namespace MiNET.Blocks
{
	public interface ICustomBlockFactory
	{
		Block GetBlockById(int blockId);
	}

	public class R12ToCurrentBlockMapEntry
	{
		public string StringId { get; set; }
		public short Meta { get; set; }
		public BlockStateContainer State { get; set; }

		public R12ToCurrentBlockMapEntry(string id, short meta, BlockStateContainer state)
		{
			StringId = id;
			Meta = meta;
			State = state;
		}
	}
	
	public static class BlockFactory
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(BlockFactory));

		public static ICustomBlockFactory CustomBlockFactory { get; set; }

		public static readonly byte[] TransparentBlocks = new byte[600];
		public static readonly byte[] LuminousBlocks = new byte[600];
		public static Dictionary<string, int> NameToId { get; private set; }
		public static BlockPalette BlockPalette { get; set; } = null;
		public static HashSet<BlockStateContainer> BlockStates { get; set; } = null;

		public static int[] LegacyToRuntimeId = new int[65536];

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

			lock (lockObj)
			{
				Dictionary<string, int> idMapping = new Dictionary<string, int>(ResourceUtil.ReadResource<Dictionary<string, int>>("block_id_map.json", typeof(Block), "Data"), StringComparer.OrdinalIgnoreCase);

				int runtimeId = 0;
				BlockPalette = new BlockPalette();
				
				using (var stream = assembly.GetManifestResourceStream(typeof(Block).Namespace + ".Data.canonical_block_states.nbt"))
				{
					do
					{
						var compound = Packet.ReadNbtCompound(stream, true);
						var container = GetBlockStateContainer(compound);
						
						container.RuntimeId = runtimeId++;
						BlockPalette.Add(container);
					} while (stream.Position < stream.Length);
				}

				List<R12ToCurrentBlockMapEntry> legacyStateMap = new List<R12ToCurrentBlockMapEntry>();
				using (var stream = assembly.GetManifestResourceStream(typeof(Block).Namespace + ".Data.r12_to_current_block_map.bin"))
				{
					while (stream.Position < stream.Length)
					{
						var length = VarInt.ReadUInt32(stream);
						byte[] bytes = new byte[length];
						stream.Read(bytes, 0, bytes.Length);

						string stringId = Encoding.UTF8.GetString(bytes);

						bytes = new byte[2];
						stream.Read(bytes, 0, bytes.Length);
						var meta = BitConverter.ToInt16(bytes);

						var compound = Packet.ReadNbtCompound(stream, true);

						legacyStateMap.Add(new R12ToCurrentBlockMapEntry(stringId, meta, GetBlockStateContainer(compound)));
					}
				}
				
				Dictionary<string, List<int>> idToStatesMap = new Dictionary<string, List<int>>(StringComparer.OrdinalIgnoreCase);

				for (var index = 0; index < BlockPalette.Count; index++)
				{
					var state = BlockPalette[index];
					List<int> candidates;

					if (!idToStatesMap.TryGetValue(state.Name, out candidates))
						candidates = new List<int>();

					candidates.Add(index);

					idToStatesMap[state.Name] = candidates;
				}

				foreach (var pair in legacyStateMap)
				{
					if (!idMapping.TryGetValue(pair.StringId, out int id))
						continue;

					var data = pair.Meta;

					if (data > 15)
					{
						continue;
					}

					var mappedState = pair.State;
					var mappedName = pair.State.Name;

					if (!idToStatesMap.TryGetValue(mappedName, out var matching))
					{
						continue;
					}

					foreach (var match in matching)
					{
						var networkState = BlockPalette[match];

						var thisStates = new HashSet<IBlockState>(mappedState.States);
						var otherStates = new HashSet<IBlockState>(networkState.States);

						otherStates.IntersectWith(thisStates);

						if (otherStates.Count == thisStates.Count)
						{
							BlockPalette[match].Id = id;
							BlockPalette[match].Data = data;

							BlockPalette[match].ItemInstance = new ItemPickInstance()
							{
								Id = (short) id,
								Metadata = data,
								WantNbt = false
							};

							LegacyToRuntimeId[(id << 4) | (byte) data] = match;

							break;
						}
					}
				}

				foreach(var record in BlockPalette)
				{
					var states = new List<NbtTag>();
					foreach (IBlockState state in record.States)
					{
						NbtTag stateTag = null;
						switch (state)
						{
							case BlockStateByte blockStateByte:
								stateTag = new NbtByte(state.Name, blockStateByte.Value);
								break;
							case BlockStateInt blockStateInt:
								stateTag = new NbtInt(state.Name, blockStateInt.Value);
								break;
							case BlockStateString blockStateString:
								stateTag = new NbtString(state.Name, blockStateString.Value);
								break;
							default:
								throw new ArgumentOutOfRangeException(nameof(state));
						}
						states.Add(stateTag);
					}

					var nbt = new NbtFile()
					{
						BigEndian = false,
						UseVarInt = true,
						RootTag = new NbtCompound("states", states)
					};

					byte[] nbtBinary = nbt.SaveToBuffer(NbtCompression.None);

					record.StatesCacheNbt = nbtBinary;
				}
			}
			
			BlockStates = new HashSet<BlockStateContainer>(BlockPalette);
		}
		
		private static BlockStateContainer GetBlockStateContainer(NbtTag tag)
		{
			var record = new BlockStateContainer();

			string name = tag["name"].StringValue;
			record.Name = name;
			record.States = GetBlockStates(tag);

			return record;
		}

		private static List<IBlockState> GetBlockStates(NbtTag tag)
		{
			var result = new List<IBlockState>();

			var states = tag["states"];
			if (states != null && states is NbtCompound compound)
			{
				foreach (var stateEntry in compound)
				{
					switch (stateEntry)
					{
						case NbtInt nbtInt:
							result.Add(new BlockStateInt()
							{
								Name = nbtInt.Name,
								Value = nbtInt.Value
							});
							break;
						case NbtByte nbtByte:
							result.Add(new BlockStateByte()
							{
								Name = nbtByte.Name,
								Value = nbtByte.Value
							});
							break;
						case NbtString nbtString:
							result.Add(new BlockStateString()
							{
								Name = nbtString.Name,
								Value = nbtString.Value
							});
							break;
					}
				}
			}

			return result;
		}

		private static object lockObj = new object();

		private static Dictionary<string, int> BuildNameToId()
		{
			//TODO: Refactor to use the Item.Name in hashed set instead.

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
			blockName = blockName.ToLowerInvariant().Replace("_", "").Replace("minecraft:", "");

			if (NameToId.ContainsKey(blockName))
			{
				return NameToId[blockName];
			}

			return 0;
		}

		public static Block GetBlockByName(string blockName)
		{
			if (string.IsNullOrEmpty(blockName)) return null;

			blockName = blockName.ToLowerInvariant().Replace("_", "").Replace("minecraft:", "");

			if (NameToId.ContainsKey(blockName))
			{
				return GetBlockById(NameToId[blockName]);
			}

			return null;
		}

		public static Block GetBlockById(int blockId, byte metadata)
		{
			int runtimeId = (int) GetRuntimeId(blockId, metadata);
			if (runtimeId < 0 || runtimeId >= BlockPalette.Count) return null;
			BlockStateContainer blockState = BlockPalette[runtimeId];
			Block block = GetBlockById(blockState.Id);
			block.SetState(blockState.States);
			return block;
		}

		public static Block GetBlockById(int blockId)
		{
			Block block = null;

			if (CustomBlockFactory != null) block = CustomBlockFactory.GetBlockById(blockId);

			if (block != null) return block;

			block = blockId switch
			{
				0 => new Air(),
				1 => new Stone(),
				2 => new Grass(),
				3 => new Dirt(),
				4 => new Cobblestone(),
				5 => new Planks(),
				6 => new Sapling(),
				7 => new Bedrock(),
				8 => new FlowingWater(),
				9 => new Water(),
				10 => new FlowingLava(),
				11 => new Lava(),
				12 => new Sand(),
				13 => new Gravel(),
				14 => new GoldOre(),
				15 => new IronOre(),
				16 => new CoalOre(),
				17 => new Log(),
				18 => new Leaves(),
				19 => new Sponge(),
				20 => new Glass(),
				21 => new LapisOre(),
				22 => new LapisBlock(),
				23 => new Dispenser(),
				24 => new Sandstone(),
				25 => new Noteblock(),
				26 => new Bed(),
				27 => new GoldenRail(),
				28 => new DetectorRail(),
				29 => new StickyPiston(),
				30 => new Web(),
				31 => new Tallgrass(),
				32 => new Deadbush(),
				33 => new Piston(),
				34 => new PistonArmCollision(),
				35 => new Wool(),
				36 => new Element0(),
				37 => new YellowFlower(),
				38 => new RedFlower(),
				39 => new BrownMushroom(),
				40 => new RedMushroom(),
				41 => new GoldBlock(),
				42 => new IronBlock(),
				43 => new DoubleStoneSlab(),
				44 => new StoneSlab(),
				45 => new BrickBlock(),
				46 => new Tnt(),
				47 => new Bookshelf(),
				48 => new MossyCobblestone(),
				49 => new Obsidian(),
				50 => new Torch(),
				51 => new Fire(),
				52 => new MobSpawner(),
				53 => new OakStairs(),
				54 => new Chest(),
				55 => new RedstoneWire(),
				56 => new DiamondOre(),
				57 => new DiamondBlock(),
				58 => new CraftingTable(),
				59 => new Wheat(),
				60 => new Farmland(),
				61 => new Furnace(),
				62 => new LitFurnace(),
				63 => new StandingSign(),
				64 => new WoodenDoor(),
				65 => new Ladder(),
				66 => new Rail(),
				67 => new StoneStairs(),
				68 => new WallSign(),
				69 => new Lever(),
				70 => new StonePressurePlate(),
				71 => new IronDoor(),
				72 => new WoodenPressurePlate(),
				73 => new RedstoneOre(),
				74 => new LitRedstoneOre(),
				75 => new UnlitRedstoneTorch(),
				76 => new RedstoneTorch(),
				77 => new StoneButton(),
				78 => new SnowLayer(),
				79 => new Ice(),
				80 => new Snow(),
				81 => new Cactus(),
				82 => new Clay(),
				83 => new Reeds(),
				84 => new Jukebox(),
				85 => new Fence(),
				86 => new Pumpkin(),
				87 => new Netherrack(),
				88 => new SoulSand(),
				89 => new Glowstone(),
				90 => new Portal(),
				91 => new LitPumpkin(),
				92 => new Cake(),
				93 => new UnpoweredRepeater(),
				94 => new PoweredRepeater(),
				95 => new InvisibleBedrock(),
				96 => new Trapdoor(),
				97 => new MonsterEgg(),
				98 => new Stonebrick(),
				99 => new BrownMushroomBlock(),
				100 => new RedMushroomBlock(),
				101 => new IronBars(),
				102 => new GlassPane(),
				103 => new MelonBlock(),
				104 => new PumpkinStem(),
				105 => new MelonStem(),
				106 => new Vine(),
				107 => new FenceGate(),
				108 => new BrickStairs(),
				109 => new StoneBrickStairs(),
				110 => new Mycelium(),
				111 => new Waterlily(),
				112 => new NetherBrick(),
				113 => new NetherBrickFence(),
				114 => new NetherBrickStairs(),
				115 => new NetherWart(),
				116 => new EnchantingTable(),
				117 => new BrewingStand(),
				118 => new Cauldron(),
				119 => new EndPortal(),
				120 => new EndPortalFrame(),
				121 => new EndStone(),
				122 => new DragonEgg(),
				123 => new RedstoneLamp(),
				124 => new LitRedstoneLamp(),
				125 => new Dropper(),
				126 => new ActivatorRail(),
				127 => new Cocoa(),
				128 => new SandstoneStairs(),
				129 => new EmeraldOre(),
				130 => new EnderChest(),
				131 => new TripwireHook(),
				132 => new TripWire(),
				133 => new EmeraldBlock(),
				134 => new SpruceStairs(),
				135 => new BirchStairs(),
				136 => new JungleStairs(),
				137 => new CommandBlock(),
				138 => new Beacon(),
				139 => new CobblestoneWall(),
				140 => new FlowerPot(),
				141 => new Carrots(),
				142 => new Potatoes(),
				143 => new WoodenButton(),
				144 => new Skull(),
				145 => new Anvil(),
				146 => new TrappedChest(),
				147 => new LightWeightedPressurePlate(),
				148 => new HeavyWeightedPressurePlate(),
				149 => new UnpoweredComparator(),
				150 => new PoweredComparator(),
				151 => new DaylightDetector(),
				152 => new RedstoneBlock(),
				153 => new QuartzOre(),
				154 => new Hopper(),
				155 => new QuartzBlock(),
				156 => new QuartzStairs(),
				157 => new DoubleWoodenSlab(),
				158 => new WoodenSlab(),
				159 => new StainedHardenedClay(),
				160 => new StainedGlassPane(),
				161 => new Leaves2(),
				162 => new Log2(),
				163 => new AcaciaStairs(),
				164 => new DarkOakStairs(),
				165 => new Slime(),
				167 => new IronTrapdoor(),
				168 => new Prismarine(),
				169 => new SeaLantern(),
				170 => new HayBlock(),
				171 => new Carpet(),
				172 => new HardenedClay(),
				173 => new CoalBlock(),
				174 => new PackedIce(),
				175 => new DoublePlant(),
				176 => new StandingBanner(),
				177 => new WallBanner(),
				178 => new DaylightDetectorInverted(),
				179 => new RedSandstone(),
				180 => new RedSandstoneStairs(),
				181 => new DoubleStoneSlab2(),
				182 => new StoneSlab2(),
				183 => new SpruceFenceGate(),
				184 => new BirchFenceGate(),
				185 => new JungleFenceGate(),
				186 => new DarkOakFenceGate(),
				187 => new AcaciaFenceGate(),
				188 => new RepeatingCommandBlock(),
				189 => new ChainCommandBlock(),
				190 => new HardGlassPane(),
				191 => new HardStainedGlassPane(),
				192 => new ChemicalHeat(),
				193 => new SpruceDoor(),
				194 => new BirchDoor(),
				195 => new JungleDoor(),
				196 => new AcaciaDoor(),
				197 => new DarkOakDoor(),
				198 => new GrassPath(),
				199 => new Frame(),
				200 => new ChorusFlower(),
				201 => new PurpurBlock(),
				202 => new ColoredTorchRg(),
				203 => new PurpurStairs(),
				204 => new ColoredTorchBp(),
				205 => new UndyedShulkerBox(),
				206 => new EndBricks(),
				207 => new FrostedIce(),
				208 => new EndRod(),
				209 => new EndGateway(),
				210 => new Allow(),
				211 => new Deny(),
				212 => new BorderBlock(),
				213 => new Magma(),
				214 => new NetherWartBlock(),
				215 => new RedNetherBrick(),
				216 => new BoneBlock(),
				217 => new StructureVoid(),
				218 => new ShulkerBox(),
				219 => new PurpleGlazedTerracotta(),
				220 => new WhiteGlazedTerracotta(),
				221 => new OrangeGlazedTerracotta(),
				222 => new MagentaGlazedTerracotta(),
				223 => new LightBlueGlazedTerracotta(),
				224 => new YellowGlazedTerracotta(),
				225 => new LimeGlazedTerracotta(),
				226 => new PinkGlazedTerracotta(),
				227 => new GrayGlazedTerracotta(),
				228 => new SilverGlazedTerracotta(),
				229 => new CyanGlazedTerracotta(),
				230 => new Chalkboard(),
				231 => new BlueGlazedTerracotta(),
				232 => new BrownGlazedTerracotta(),
				233 => new GreenGlazedTerracotta(),
				234 => new RedGlazedTerracotta(),
				235 => new BlackGlazedTerracotta(),
				236 => new Concrete(),
				237 => new ConcretePowder(),
				238 => new ChemistryTable(),
				239 => new UnderwaterTorch(),
				240 => new ChorusPlant(),
				241 => new StainedGlass(),
				242 => new Camera(),
				243 => new Podzol(),
				244 => new Beetroot(),
				245 => new Stonecutter(),
				246 => new Glowingobsidian(),
				247 => new Netherreactor(),
				248 => new InfoUpdate(),
				249 => new InfoUpdate2(),
				250 => new MovingBlock(),
				251 => new Observer(),
				252 => new StructureBlock(),
				253 => new HardGlass(),
				254 => new HardStainedGlass(),
				255 => new Reserved6(),
				257 => new PrismarineStairs(),
				258 => new DarkPrismarineStairs(),
				259 => new PrismarineBricksStairs(),
				260 => new StrippedSpruceLog(),
				261 => new StrippedBirchLog(),
				262 => new StrippedJungleLog(),
				263 => new StrippedAcaciaLog(),
				264 => new StrippedDarkOakLog(),
				265 => new StrippedOakLog(),
				266 => new BlueIce(),
				267 => new Element1(),
				268 => new Element2(),
				269 => new Element3(),
				270 => new Element4(),
				271 => new Element5(),
				272 => new Element6(),
				273 => new Element7(),
				274 => new Element8(),
				275 => new Element9(),
				276 => new Element10(),
				277 => new Element11(),
				278 => new Element12(),
				279 => new Element13(),
				280 => new Element14(),
				281 => new Element15(),
				282 => new Element16(),
				283 => new Element17(),
				284 => new Element18(),
				285 => new Element19(),
				286 => new Element20(),
				287 => new Element21(),
				288 => new Element22(),
				289 => new Element23(),
				290 => new Element24(),
				291 => new Element25(),
				292 => new Element26(),
				293 => new Element27(),
				294 => new Element28(),
				295 => new Element29(),
				296 => new Element30(),
				297 => new Element31(),
				298 => new Element32(),
				299 => new Element33(),
				300 => new Element34(),
				301 => new Element35(),
				302 => new Element36(),
				303 => new Element37(),
				304 => new Element38(),
				305 => new Element39(),
				306 => new Element40(),
				307 => new Element41(),
				308 => new Element42(),
				309 => new Element43(),
				310 => new Element44(),
				311 => new Element45(),
				312 => new Element46(),
				313 => new Element47(),
				314 => new Element48(),
				315 => new Element49(),
				316 => new Element50(),
				317 => new Element51(),
				318 => new Element52(),
				319 => new Element53(),
				320 => new Element54(),
				321 => new Element55(),
				322 => new Element56(),
				323 => new Element57(),
				324 => new Element58(),
				325 => new Element59(),
				326 => new Element60(),
				327 => new Element61(),
				328 => new Element62(),
				329 => new Element63(),
				330 => new Element64(),
				331 => new Element65(),
				332 => new Element66(),
				333 => new Element67(),
				334 => new Element68(),
				335 => new Element69(),
				336 => new Element70(),
				337 => new Element71(),
				338 => new Element72(),
				339 => new Element73(),
				340 => new Element74(),
				341 => new Element75(),
				342 => new Element76(),
				343 => new Element77(),
				344 => new Element78(),
				345 => new Element79(),
				346 => new Element80(),
				347 => new Element81(),
				348 => new Element82(),
				349 => new Element83(),
				350 => new Element84(),
				351 => new Element85(),
				352 => new Element86(),
				353 => new Element87(),
				354 => new Element88(),
				355 => new Element89(),
				356 => new Element90(),
				357 => new Element91(),
				358 => new Element92(),
				359 => new Element93(),
				360 => new Element94(),
				361 => new Element95(),
				362 => new Element96(),
				363 => new Element97(),
				364 => new Element98(),
				365 => new Element99(),
				366 => new Element100(),
				367 => new Element101(),
				368 => new Element102(),
				369 => new Element103(),
				370 => new Element104(),
				371 => new Element105(),
				372 => new Element106(),
				373 => new Element107(),
				374 => new Element108(),
				375 => new Element109(),
				376 => new Element110(),
				377 => new Element111(),
				378 => new Element112(),
				379 => new Element113(),
				380 => new Element114(),
				381 => new Element115(),
				382 => new Element116(),
				383 => new Element117(),
				384 => new Element118(),
				385 => new Seagrass(),
				386 => new Coral(),
				387 => new CoralBlock(),
				388 => new CoralFan(),
				389 => new CoralFanDead(),
				390 => new CoralFanHang(),
				391 => new CoralFanHang2(),
				392 => new CoralFanHang3(),
				393 => new Kelp(),
				394 => new DriedKelpBlock(),
				395 => new AcaciaButton(),
				396 => new BirchButton(),
				397 => new DarkOakButton(),
				398 => new JungleButton(),
				399 => new SpruceButton(),
				400 => new AcaciaTrapdoor(),
				401 => new BirchTrapdoor(),
				402 => new DarkOakTrapdoor(),
				403 => new JungleTrapdoor(),
				404 => new SpruceTrapdoor(),
				405 => new AcaciaPressurePlate(),
				406 => new BirchPressurePlate(),
				407 => new DarkOakPressurePlate(),
				408 => new JunglePressurePlate(),
				409 => new SprucePressurePlate(),
				410 => new CarvedPumpkin(),
				411 => new SeaPickle(),
				412 => new Conduit(),
				414 => new TurtleEgg(),
				415 => new BubbleColumn(),
				416 => new Barrier(),
				417 => new StoneSlab3(),
				418 => new Bamboo(),
				419 => new BambooSapling(),
				420 => new Scaffolding(),
				421 => new StoneSlab4(),
				422 => new DoubleStoneSlab3(),
				423 => new DoubleStoneSlab4(),
				424 => new GraniteStairs(),
				425 => new DioriteStairs(),
				426 => new AndesiteStairs(),
				427 => new PolishedGraniteStairs(),
				428 => new PolishedDioriteStairs(),
				429 => new PolishedAndesiteStairs(),
				430 => new MossyStoneBrickStairs(),
				431 => new SmoothRedSandstoneStairs(),
				432 => new SmoothSandstoneStairs(),
				433 => new EndBrickStairs(),
				434 => new MossyCobblestoneStairs(),
				435 => new NormalStoneStairs(),
				436 => new SpruceStandingSign(),
				437 => new SpruceWallSign(),
				438 => new SmoothStone(),
				439 => new RedNetherBrickStairs(),
				440 => new SmoothQuartzStairs(),
				441 => new BirchStandingSign(),
				442 => new BirchWallSign(),
				443 => new JungleStandingSign(),
				444 => new JungleWallSign(),
				445 => new AcaciaStandingSign(),
				446 => new AcaciaWallSign(),
				447 => new DarkoakStandingSign(),
				448 => new DarkoakWallSign(),
				449 => new Lectern(),
				450 => new Grindstone(),
				451 => new BlastFurnace(),
				452 => new StonecutterBlock(),
				453 => new Smoker(),
				454 => new LitSmoker(),
				455 => new CartographyTable(),
				456 => new FletchingTable(),
				457 => new SmithingTable(),
				458 => new Barrel(),
				459 => new Loom(),
				461 => new Bell(),
				462 => new SweetBerryBush(),
				463 => new Lantern(),
				464 => new Campfire(),
				465 => new LavaCauldron(),
				466 => new Jigsaw(),
				467 => new Wood(),
				468 => new Composter(),
				469 => new LitBlastFurnace(),
				470 => new LightBlock(),
				471 => new WitherRose(),
				472 => new StickyPistonArmCollision(),
				473 => new BeeNest(),
				474 => new Beehive(),
				475 => new HoneyBlock(),
				476 => new HoneycombBlock(),
				477 => new Lodestone(),
				478 => new CrimsonRoots(),
				479 => new WarpedRoots(),
				480 => new CrimsonStem(),
				481 => new WarpedStem(),
				482 => new WarpedWartBlock(),
				483 => new CrimsonFungus(),
				484 => new WarpedFungus(),
				485 => new Shroomlight(),
				486 => new WeepingVines(),
				487 => new CrimsonNylium(),
				488 => new WarpedNylium(),
				489 => new Basalt(),
				490 => new PolishedBasalt(),
				491 => new SoulSoil(),
				492 => new SoulFire(),
				493 => new NetherSprouts(),
				494 => new Target(),
				495 => new StrippedCrimsonStem(),
				496 => new StrippedWarpedStem(),
				497 => new CrimsonPlanks(),
				498 => new WarpedPlanks(),
				499 => new CrimsonDoor(),
				500 => new WarpedDoor(),
				501 => new CrimsonTrapdoor(),
				502 => new WarpedTrapdoor(),
				505 => new CrimsonStandingSign(),
				506 => new WarpedStandingSign(),
				507 => new CrimsonWallSign(),
				508 => new WarpedWallSign(),
				509 => new CrimsonStairs(),
				510 => new WarpedStairs(),
				511 => new CrimsonFence(),
				512 => new WarpedFence(),
				513 => new CrimsonFenceGate(),
				514 => new WarpedFenceGate(),
				515 => new CrimsonButton(),
				516 => new WarpedButton(),
				517 => new CrimsonPressurePlate(),
				518 => new WarpedPressurePlate(),
				519 => new CrimsonSlab(),
				520 => new WarpedSlab(),
				521 => new CrimsonDoubleSlab(),
				522 => new WarpedDoubleSlab(),
				523 => new SoulTorch(),
				524 => new SoulLantern(),
				525 => new NetheriteBlock(),
				526 => new AncientDebris(),
				527 => new RespawnAnchor(),
				528 => new Blackstone(),
				529 => new PolishedBlackstoneBricks(),
				530 => new PolishedBlackstoneBrickStairs(),
				531 => new BlackstoneStairs(),
				532 => new BlackstoneWall(),
				533 => new PolishedBlackstoneBrickWall(),
				534 => new ChiseledPolishedBlackstone(),
				535 => new CrackedPolishedBlackstoneBricks(),
				536 => new GildedBlackstone(),
				537 => new BlackstoneSlab(),
				538 => new BlackstoneDoubleSlab(),
				539 => new PolishedBlackstoneBrickSlab(),
				540 => new PolishedBlackstoneBrickDoubleSlab(),
				541 => new Chain(),
				542 => new TwistingVines(),
				543 => new NetherGoldOre(),
				544 => new CryingObsidian(),
				545 => new SoulCampfire(),
				546 => new PolishedBlackstone(),
				547 => new PolishedBlackstoneStairs(),
				548 => new PolishedBlackstoneSlab(),
				549 => new PolishedBlackstoneDoubleSlab(),
				550 => new PolishedBlackstonePressurePlate(),
				551 => new PolishedBlackstoneButton(),
				552 => new PolishedBlackstoneWall(),
				553 => new WarpedHyphae(),
				554 => new CrimsonHyphae(),
				555 => new StrippedCrimsonHyphae(),
				556 => new StrippedWarpedHyphae(),
				557 => new ChiseledNetherBricks(),
				558 => new CrackedNetherBricks(),
				559 => new QuartzBricks(),
				_ => new Block(blockId)
			};

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
		private static int TryGetRuntimeId(int blockId, byte metadata)
		{
			return LegacyToRuntimeId[(blockId << 4) | metadata];
		}
	}
}