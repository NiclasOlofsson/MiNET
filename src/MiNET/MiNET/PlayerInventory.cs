using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using MiNET.Items;
using MiNET.Net;
using MiNET.Utils;

namespace MiNET
{
	public class PlayerInventory
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (PlayerInventory));

		public const int HotbarSize = 9;
		public const int InventorySize = HotbarSize + 36;
		public Player Player { get; private set; }

		public List<Item> Slots { get; private set; }
		public int[] ItemHotbar { get; private set; }
		public int InHandSlot { get; set; }


		// Armour
		public Item Boots { get; set; }
		public Item Leggings { get; set; }
		public Item Chest { get; set; }
		public Item Helmet { get; set; }

		public PlayerInventory(Player player)
		{
			Player = player;

			Slots = Enumerable.Repeat((Item) new ItemAir(), InventorySize).ToList();
			//Slots = Enumerable.Repeat(new ItemStack(new ItemIronSword(0), 1), InventorySize).ToList();
			//Slots[Slots.Count-10] = new ItemStack(new ItemDiamondAxe(0), 1);
			//Slots[Slots.Count-9] = new ItemStack(new ItemDiamondAxe(0), 1);
			//int c = -1;
			//Slots[++c] = new ItemStack(new ItemEnchantingTable(0), 3);
			//Slots[++c] = new ItemStack(new ItemIronSword(0), 1);
			//Slots[++c] = new ItemStack(new ItemDiamondSword(0), 1);
			//Slots[++c] = new ItemStack(new Item(17, 0), 14);
			//Slots[++c] = new ItemStack(new ItemBow(0), 1);
			//Slots[++c] = new ItemStack(new ItemSnowball(0), 64);
			//Slots[++c] = new ItemStack(new ItemEgg(0), 64);
			//Slots[++c] = new ItemStack(262, 32);
			//Slots[++c] = new ItemStack(new ItemBucket(10), 1);
			//Slots[++c] = new ItemStack(new ItemChest(0), 1);
			////Slots[++c] = new ItemStack(new ItemBlock(new DiamondOre(), 0), 64);
			//Slots[++c] = new ItemStack(new Item(351, 4), 30);
			//Slots[++c] = new ItemStack(new ItemBlock(new GoldBlock(), 0), 64);
			//Slots[++c] = new ItemStack(new ItemBlock(new GoldBlock(), 0), 64);
			//Slots[++c] = new ItemStack(new ItemBlock(new GoldBlock(), 0), 64);
			//Slots[++c] = new ItemStack(new ItemBlock(new GoldBlock(), 0), 64);
			//Slots[++c] = new ItemStack(new ItemBlock(new GoldBlock(), 0), 64);
			//Slots[++c] = new ItemStack(new ItemBlock(new GoldBlock(), 0), 64);
			//Slots[++c] = new ItemStack(new ItemBlock(new GoldBlock(), 0), 64);
			//Slots[++c] = new ItemStack(new ItemBlock(new GoldBlock(), 0), 64);
			//Slots[++c] = new ItemStack(new ItemBlock(new GoldBlock(), 0), 64);
			//Slots[++c] = new ItemStack(new ItemBlock(new GoldBlock(), 0), 64);
			//Slots[++c] = new ItemStack(new ItemBlock(new GoldBlock(), 0), 64);
			//Slots[++c] = new ItemStack(new ItemBlock(new GoldBlock(), 0), 64);
			//Slots[++c] = new ItemStack(new ItemBlock(new GoldBlock(), 0), 64);
			//Slots[++c] = new ItemStack(new ItemBlock(new GoldBlock(), 0), 64);
			//Slots[++c] = new ItemStack(new ItemBlock(new GoldBlock(), 0), 64);
			//Slots[++c] = new ItemStack(new ItemBlock(new CoalBlock(), 0), 64);

			//Slots = new List<ItemStack>();
			//for (int i = 0; i < 100; i++)
			//{
			//	Slots.Add(new ItemStack(ItemFactory.GetItem(i, 0), 1));
			//}

			ItemHotbar = new int[HotbarSize];
			InHandSlot = 0;

			for (byte i = 0; i < ItemHotbar.Length; i++)
			{
				ItemHotbar[i] = i;
			}

			Boots = new ItemAir();
			Leggings = new ItemAir();
			Chest = new ItemAir();
			Helmet = new ItemAir();

			//Boots = new ItemDiamondBoots(0);
			//Leggings = new ItemDiamondLeggings(0);
			//Chest = new ItemDiamondChestplate(0);
			//Helmet = new ItemDiamondHelmet(0);
		}

		public virtual Item GetItemInHand()
		{
			var index = ItemHotbar[InHandSlot];
			if (index == -1 || index >= Slots.Count) return new ItemAir();

			return Slots[index] ?? new ItemAir();
		}

		public virtual int GetItemInHandSlot()
		{
			return ItemHotbar[InHandSlot];
		}

		[Wired]
		[Obsolete("Use method with item instead.")]
		public void SetInventorySlot(int slot, short itemId, byte amount = 1, short metadata = 0)
		{
			SetInventorySlot(slot, ItemFactory.GetItem(itemId, metadata, amount));
		}

		[Wired]
		public void SetInventorySlot(int slot, Item item)
		{
			Slots[slot] = item;

			var containerSetContent = McpeContainerSetContent.CreateObject();
			containerSetContent.windowId = 0;
			containerSetContent.slotData = GetSlots();
			containerSetContent.hotbarData = GetHotbar();
			Player.SendPackage(containerSetContent);
		}

		public MetadataInts GetHotbar()
		{
			MetadataInts metadata = new MetadataInts();
			for (byte i = 0; i < ItemHotbar.Length; i++)
			{
				if (ItemHotbar[i] == -1)
				{
					metadata[i] = new MetadataInt(-1);
				}
				else
				{
					metadata[i] = new MetadataInt(ItemHotbar[i] + HotbarSize);
				}
			}

			return metadata;
		}

		public ItemStacks GetSlots()
		{
			ItemStacks slotData = new ItemStacks();
			for (int i = 0; i < Slots.Count; i++)
			{
				if (Slots[i].Count == 0) Slots[i] = new ItemAir();
				slotData.Add(Slots[i]);
			}

			return slotData;
		}

		public ItemStacks GetArmor()
		{
			return new ItemStacks
			{
				Helmet ?? new ItemAir(),
				Chest ?? new ItemAir(),
				Leggings ?? new ItemAir(),
				Boots ?? new ItemAir(),
			};
		}

		public bool SetFirstEmptySlot(Item item, bool update, bool reverseOrder)
		{
			if (reverseOrder)
			{
				for (int si = Slots.Count; si > 0; si--)
				{
					if (FirstEmptySlot(item, update, si - 1)) return true;
				}
			}
			else
			{
				for (int si = 0; si < Slots.Count; si++)
				{
					if (FirstEmptySlot(item, update, si)) return true;
				}
			}

			return false;
		}

		private bool FirstEmptySlot(Item item, bool update, int si)
		{
			Item existingItem = Slots[si];

			if (existingItem.Id == item.Id && existingItem.Metadata == item.Metadata && existingItem.Count + item.Count <= item.MaxStackSize)
			{
				Slots[si].Count += item.Count;
				//if (update) Player.SendPlayerInventory();
				if (update) SendSetSlot(si);
				return true;
			}
			else if (existingItem is ItemAir || existingItem.Id == -1)
			{
				Slots[si] = item;
				//if (update) Player.SendPlayerInventory();
				if (update) SendSetSlot(si);
				return true;
			}

			return false;
		}

		public void SetHeldItemSlot(int slot, bool sendToPlayer = true)
		{
			InHandSlot = slot;

			if (sendToPlayer)
			{
				McpePlayerEquipment order = McpePlayerEquipment.CreateObject();
				order.entityId = 0;
				order.item = GetItemInHand();
				order.selectedSlot = (byte) slot;
				Player.SendPackage(order);
			}

			McpePlayerEquipment broadcast = McpePlayerEquipment.CreateObject();
			broadcast.entityId = Player.EntityId;
			broadcast.item = GetItemInHand();
			broadcast.selectedSlot = (byte) slot;
			Player.Level?.RelayBroadcast(broadcast);
		}

		/// <summary>
		///     Empty the specified slot
		/// </summary>
		/// <param name="slot">The slot to empty.</param>
		public void ClearInventorySlot(byte slot)
		{
			SetInventorySlot(slot, 0, 0);
		}

		public bool HasItem(Item item)
		{
			for (byte i = 0; i < Slots.Count; i++)
			{
				if ((Slots[i]).Id == item.Id && (Slots[i]).Metadata == item.Metadata)
				{
					return true;
				}
			}
			return false;
		}

		public void RemoveItems(short id, byte count)
		{
			for (byte i = 0; i < Slots.Count; i++)
			{
				var slot = Slots[i];
				if (slot.Id == id)
				{
					slot.Count--;
					if (slot.Count == 0)
					{
						Slots[i] = new ItemAir();
					}

					SendSetSlot(i);
					return;
				}
			}
		}

		public void SendSetSlot(int slot)
		{
			if (slot < HotbarSize && (ItemHotbar[slot] == -1 || ItemHotbar[slot] == slot))
			{
				ItemHotbar[slot] = slot /* + HotbarSize*/;
				Player.SendPlayerInventory();
			}
			else
			{
				McpeContainerSetSlot sendSlot = new McpeContainerSetSlot();
				sendSlot.NoBatch = true;
				sendSlot.slot = (short) slot;
				sendSlot.item = Slots[slot];
				Player.SendPackage(sendSlot);
			}
		}
	}

	public class Material
	{
		public const byte Air = 0;
		public const byte Stone = 1;
		public const byte Grass = 2;
		public const byte Dirt = 3;
		public const byte Cobblestone = 4;
		public const byte WoodPlank = 5;
		public const byte Sapling = 6;
		public const byte Bedrock = 7;
		public const byte Water = 8;
		public const byte WaterStill = 9;
		public const byte Lava = 10;
		public const byte LavaStill = 11;
		public const byte Sand = 12;
		public const byte Gravel = 13;
		public const byte GoldOre = 14;
		public const byte IronOre = 15;
		public const byte CoalOre = 16;
		public const byte Wood = 17;
		public const byte Leaves = 18;
		public const byte Sponge = 19;
		public const byte Glass = 20;
		public const byte LapisOre = 21;
		public const byte LapisBlock = 22;

		public const byte Sandstone = 24;

		public const byte Bed = 26;
		public const byte PoweredRail = 27;


		public const byte Cobweb = 30;
		public const byte TallGrass = 31;
		public const byte DeadBush = 32;


		public const byte Wool = 35;

		public const byte Dandelion = 37;
		public const byte Flower = 38;
		public const byte BrownMushroom = 39;
		public const byte RedMushroom = 40;
		public const byte GoldBlock = 41;
		public const byte IronBlock = 42;
		public const byte DoubleStoneSlab = 43;
		public const byte StoneSlab = 44;
		public const byte Bricks = 45;
		public const byte TNT = 46;
		public const byte Bookshelf = 47;
		public const byte MossStone = 48;
		public const byte Obsidian = 49;
		public const byte Torch = 50;
		public const byte Fire = 51;
		public const byte MonsterSpawner = 52;
		public const byte OakWoodStairs = 53;
		public const byte Chest = 54;

		public const byte DiamondOre = 56;
		public const byte DiamondBlock = 57;
		public const byte CraftingTable = 58;
		public const byte SeedsBlock = 59;
		public const byte Farmland = 60;
		public const byte Furnace = 61;
		public const byte FurnaceBurning = 62;
		public const byte SignPost = 63;
		public const byte WoodDoorBlock = 64;
		public const byte Ladder = 65;
		public const byte Rail = 66;
		public const byte CobbleStairs = 67;
		public const byte SignWall = 68;


		public const byte IronDoorBlock = 71;

		public const byte RedstoneOre = 73;
		public const byte RedstoneOreGlowing = 74;


		public const byte SnowLayer = 78;
		public const byte Ice = 79;
		public const byte Snow = 80;
		public const byte Cactus = 81;
		public const byte ClayBlock = 82;
		public const byte SugarCane = 83;

		public const byte Fence = 85;
		public const byte Pumpkin = 86;
		public const byte Netherrack = 87;

		public const byte Glowstone = 89;

		public const byte PumpkinLantern = 91;


		public const byte Barrier = 95;
		public const byte Trapdoor = 96;
		public const byte MonsterEgg = 97;
		public const byte StoneBrick = 98;
		public const byte BrownMushroomBlock = 99;
		public const byte RedMushroomBlock = 100;
		public const byte IronBars = 101;
		public const byte GlassPane = 102;
		public const byte MelonBlock = 103;
		public const byte PumpkinStem = 104;
		public const byte MelonStem = 105;
		public const byte Vines = 106;
		public const byte FenceGate = 107;
		public const byte BrickStairs = 108;
		public const byte StoneBrickStairs = 109;
		public const byte Mycelium = 110;
		public const byte LilyPad = 111;
		public const byte NetherBrick = 112;

		public const byte NetherBrickStairs = 114;


		public const byte EndPortal = 120;
		public const byte EndStone = 121;


		public const byte Cocoa = 127;
		public const byte SandstoneStairs = 128;
		public const byte EmeraldOre = 129;


		public const byte EmeraldBlock = 133;
		public const byte SpruceWoodStairs = 134;
		public const byte BirchWoodStairs = 135;
		public const byte JungleWoodStairs = 136;


		public const byte CobbleWall = 139;

		public const byte CarrotsBlock = 141;
		public const byte PotatoBlock = 142;


		public const byte RedstoneBlock = 152;


		public const byte QuartzBlock = 155;
		public const byte QuartzStairs = 156;
		public const byte WoodDoubleSlab = 157;
		public const byte WoodSlab = 158;
		public const byte StainedClay = 159;

		public const byte AcaciaLeaves = 161;
		public const byte AcaciaWood = 162;
		public const byte AcaciaWoodStairs = 163;
		public const byte DarkOakWoodStairs = 164;


		public const byte HayBale = 170;
		public const byte Carpet = 171;
		public const byte HardenedClay = 172;
		public const byte CoalBlock = 173;
		public const byte IcePacked = 174;
		public const byte Sunflower = 175;


		public const byte SpruceFenceGate = 183;
		public const byte BirchFenceGate = 184;
		public const byte JungleFenceGate = 185;
		public const byte DarkOakFenceGate = 186;
		public const byte AcaciaFenceGate = 187;


		public const byte GrassPath = 198;
		// ...
		public const byte Podzol = 243;
		public const byte BeetrootBlock = 244;
		public const byte StoneCutter = 245;
		public const byte ObsidianGlowing = 246;
		public const byte NetherCoreReactor = 247;


		public const int IronShovel = 256;
		public const int IronPickaxe = 257;
		public const int IronAxe = 258;
		public const int FlintAndSteel = 259;
		public const int Apple = 260;
		public const int Bow = 261;
		public const int Arrow = 262;
		public const int Coal = 263;
		public const int Diamond = 264;
		public const int IronIngot = 265;
		public const int GoldIngot = 266;
		public const int IronSword = 267;
		public const int WoodSword = 268;
		public const int WoodShovel = 269;
		public const int WoodPickaxe = 270;
		public const int WoodAxe = 271;
		public const int StoneSword = 272;
		public const int StoneShovel = 273;
		public const int StonePickaxe = 274;
		public const int StoneAxe = 275;
		public const int DiamondSword = 276;
		public const int DiamondShovel = 277;
		public const int DiamondPickaxe = 278;
		public const int DiamondAxe = 279;
		public const int Stick = 280;
		public const int Bowl = 281;
		public const int MushroomStew = 282;
		public const int GoldSword = 283;
		public const int GoldShovel = 284;
		public const int GoldPickaxe = 285;
		public const int GoldAxe = 286;
		public const int String = 287;
		public const int Feather = 288;
		public const int Gunpowder = 289;
		public const int WoodHoe = 290;
		public const int StoneHoe = 291;
		public const int IronHoe = 292;
		public const int DiamondHoe = 293;
		public const int GoldHoe = 294;
		public const int SeedsItem = 295;
		public const int Wheat = 296;
		public const int Bread = 297;
		public const int LeatherHelmet = 298;
		public const int LeatherChestplate = 299;
		public const int LeatherLeggings = 300;
		public const int LeatherBoots = 301;
		public const int ChainHelmet = 302;
		public const int ChainChestplate = 303;
		public const int ChainLeggings = 304;
		public const int ChainBoots = 305;
		public const int IronHelmet = 306;
		public const int IronChestplate = 307;
		public const int IronLeggings = 308;
		public const int IronBoots = 309;
		public const int DiamondHelmet = 310;
		public const int DiamondChestplate = 311;
		public const int DiamondLeggings = 312;
		public const int DiamondBoots = 313;
		public const int GoldHelmet = 314;
		public const int GoldChestplate = 315;
		public const int GoldLeggings = 316;
		public const int GoldBoots = 317;
		public const int Flint = 318;
		public const int RawPorkchop = 319;
		public const int CookedPorkchop = 320;
		public const int Painting = 321;
		public const int SignItem = 323;
		public const int WoodDoorItem = 324;
		public const int Bucket = 325;
		public const int Minecart = 328;
		public const int Saddle = 329;
		public const int IronDoorItem = 330;
		public const int RedstonePowder = 331;
		public const int Snowball = 332;
		public const int Boat = 333;
		public const int Leather = 334;
		public const int Brick = 336;
		public const int ClayItem = 337;
		public const int SugarCaneItem = 338;
		public const int Paper = 339;
		public const int Book = 340;
		public const int Slimeball = 341;
		public const int Egg = 344;
		public const int Compass = 345;
		public const int FishingRod = 346;
		public const int Clock = 347;
		public const int GlowstoneDust = 348;
		public const int RawFish = 349;
		public const int CookedFish = 350;
		public const int Dye = 351;
		public const int Bone = 352;
		public const int Sugar = 353;
		public const int CakeItem = 354;
		public const int BedItem = 355;
		public const int Cookie = 357;
		public const int Shears = 359;
		public const int MelonItem = 360;
		public const int PumpkinSeeds = 361;
		public const int MelonSeeds = 362;
		public const int RawBeef = 363;
		public const int Steak = 364;
		public const int RawChicken = 365;
		public const int CookedChicken = 366;
		public const int RottenFlesh = 367;
		public const int MagmaCream = 378;
		public const int SpawnEgg = 383;
		public const int Emerald = 388;
		public const int CarrotItem = 391;
		public const int PotatoItem = 392;
		public const int BakedPotato = 393;
		public const int PumpkinPie = 400;
		public const int NetherBrickitem = 405;
		public const int NetherQuartz = 406;
		public const int BeetrootItem = 457;
		public const int BeetrootSeeds = 458;
		public const int BeetrootSoup = 459;
	}
}