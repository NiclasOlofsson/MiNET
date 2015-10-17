using System.Collections.Generic;
using System.Linq;
using MiNET.Items;
using MiNET.Net;
using MiNET.Utils;

namespace MiNET
{
	public class PlayerInventory
	{
		public const int HotbarSize = 9;
		public const int InventorySize = HotbarSize + 27;
		public Player Player { get; private set; }

		public List<ItemStack> Slots { get; private set; }
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

			Slots = Enumerable.Repeat(new ItemStack(), InventorySize).ToList();
			//int c = -1;
			//Slots[++c] = new ItemStack(new ItemIronSword(0), 1);
			//Slots[++c] = new ItemStack(new ItemBow(0), 1);
			//Slots[++c] = new ItemStack(new ItemSnowball(0), 64);
			//Slots[++c] = new ItemStack(new ItemEgg(0), 64);
			//Slots[++c] = new ItemStack(262, 32);
			//Slots[++c] = new ItemStack(new ItemBucket(10), 1);
			//Slots[++c] = new ItemStack(new ItemBlock(new DiamondOre(), 0), 64);
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

			Boots = new Item(0, 0);
			Leggings = new Item(0, 0);
			Chest = new Item(0, 0);
			Helmet = new Item(0, 0);

			//Boots = new ItemDiamondBoots(0);
			//Leggings = new ItemDiamondLeggings(0);
			//Chest = new ItemDiamondChestplate(0);
			//Helmet = new ItemDiamondHelmet(0);
		}

		public virtual ItemStack GetItemInHand()
		{
			var index = ItemHotbar[InHandSlot];
			if (index == -1 || index >= Slots.Count) return new ItemStack();

			return Slots[index];
		}

		[Wired]
		public void SetInventorySlot(byte slot, short itemId, byte amount = 1, short metadata = 0)
		{
			Slots[slot] = new ItemStack(itemId, amount, metadata);

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
				metadata[i] = new MetadataInt(ItemHotbar[i] + HotbarSize);
			}

			return metadata;
		}

		public MetadataSlots GetSlots()
		{
			var slotData = new MetadataSlots();
			for (byte i = 0; i < Slots.Count; i++)
			{
				if (Slots[i].Count == 0) Slots[i] = new ItemStack();
				slotData[i] = new MetadataSlot(Slots[i]);
			}

			return slotData;
		}

		public MetadataSlots GetArmor()
		{
			var slotData = new MetadataSlots();
			slotData[0] = new MetadataSlot(new ItemStack((short) Helmet.Id, 1, Helmet.Metadata));
			slotData[1] = new MetadataSlot(new ItemStack((short) Chest.Id, 1, Helmet.Metadata));
			slotData[2] = new MetadataSlot(new ItemStack((short) Leggings.Id, 1, Helmet.Metadata));
			slotData[3] = new MetadataSlot(new ItemStack((short) Boots.Id, 1, Helmet.Metadata));
			return slotData;
		}

		public void SetFirstEmptySlot(short itemId, byte amount = 1, short metadata = 0)
		{
			for (byte s = 0; s < Slots.Count; s++)
			{
				var b = (MetadataSlot) Slots[s];
				if (b.Value.Id == itemId && b.Value.Metadata == metadata && b.Value.Count < 64)
				{
					SetInventorySlot(s, itemId, (byte) (b.Value.Count + amount), metadata);
					break;
				}
				else if (b.Value == null || b.Value.Id == 0 || b.Value.Id == -1)
				{
					SetInventorySlot(s, itemId, amount, metadata);
					break;
				}
			}
		}

		public void SetHeldItemSlot(int slot)
		{
			McpePlayerEquipment order = McpePlayerEquipment.CreateObject();
			order.entityId = 0;
			order.selectedSlot = (byte) slot;
			Player.SendPackage(order);

			McpePlayerEquipment broadcast = McpePlayerEquipment.CreateObject();
			broadcast.entityId = Player.EntityId;
			broadcast.selectedSlot = (byte) slot;
			Player.Level.RelayBroadcast(broadcast);
		}

		/// <summary>
		///     Empty the specified slot
		/// </summary>
		/// <param name="slot">The slot to empty.</param>
		public void ClearInventorySlot(byte slot)
		{
			SetInventorySlot(slot, 0, 0);
		}

		public bool HasItem(MetadataSlot item)
		{
			for (byte i = 0; i < Slots.Count; i++)
			{
				if (((MetadataSlot) Slots[i]).Value.Id == item.Value.Id && ((MetadataSlot) Slots[i]).Value.Metadata == item.Value.Metadata)
				{
					return true;
				}
			}
			return false;
		}
	}

	public static class InventoryUtils
	{
		public static void Clear(this PlayerInventory inv)
		{
			for (byte i = 0; i < inv.Slots.Count; ++i)
			{
				if (inv.Slots[i] == null || inv.Slots[i].Id != 0) inv.Slots[i] = new ItemStack();
			}

			if (inv.Helmet.Id != 0) inv.Helmet = new Item(0, 0);
			if (inv.Chest.Id != 0) inv.Chest = new Item(0, 0);
			if (inv.Leggings.Id != 0) inv.Leggings = new Item(0, 0);
			if (inv.Boots.Id != 0) inv.Boots = new Item(0, 0);

			inv.Player.SendPlayerInventory();
		}

		public static List<ItemStack> CreativeInventoryItems = new List<ItemStack>()
		{
			new ItemStack(Material.Cobblestone, 1, 0),
			new ItemStack(Material.StoneBrick, 1, 0),
			new ItemStack(Material.StoneBrick, 1, 1),
			new ItemStack(Material.StoneBrick, 1, 2),
			new ItemStack(Material.StoneBrick, 1, 3),
			new ItemStack(Material.MossStone, 1, 0),
			new ItemStack(Material.WoodPlank, 1, 0),
			new ItemStack(Material.WoodPlank, 1, 1),
			new ItemStack(Material.WoodPlank, 1, 2),
			new ItemStack(Material.WoodPlank, 1, 3),
			new ItemStack(Material.WoodPlank, 1, 4),
			new ItemStack(Material.WoodPlank, 1, 5),
			new ItemStack(Material.Bricks, 1, 0),
			new ItemStack(Material.Stone, 1, 0),
			new ItemStack(Material.Stone, 1, 1),
			new ItemStack(Material.Stone, 1, 2),
			new ItemStack(Material.Stone, 1, 3),
			new ItemStack(Material.Stone, 1, 4),
			new ItemStack(Material.Stone, 1, 5),
			new ItemStack(Material.Stone, 1, 6),
			new ItemStack(Material.Dirt, 1, 0),
			new ItemStack(Material.Podzol, 1, 0),
			new ItemStack(Material.Grass, 1, 0),
			new ItemStack(Material.Mycelium, 1, 0),
			new ItemStack(Material.ClayBlock, 1, 0),
			new ItemStack(Material.HardenedClay, 1, 0),
			new ItemStack(Material.StainedClay, 1, 0),
			new ItemStack(Material.StainedClay, 1, 1),
			new ItemStack(Material.StainedClay, 1, 2),
			new ItemStack(Material.StainedClay, 1, 3),
			new ItemStack(Material.StainedClay, 1, 4),
			new ItemStack(Material.StainedClay, 1, 5),
			new ItemStack(Material.StainedClay, 1, 6),
			new ItemStack(Material.StainedClay, 1, 7),
			new ItemStack(Material.StainedClay, 1, 8),
			new ItemStack(Material.StainedClay, 1, 9),
			new ItemStack(Material.StainedClay, 1, 10),
			new ItemStack(Material.StainedClay, 1, 11),
			new ItemStack(Material.StainedClay, 1, 12),
			new ItemStack(Material.StainedClay, 1, 13),
			new ItemStack(Material.StainedClay, 1, 14),
			new ItemStack(Material.StainedClay, 1, 15),
			new ItemStack(Material.Sandstone, 1, 0),
			new ItemStack(Material.Sandstone, 1, 1),
			new ItemStack(Material.Sandstone, 1, 2),
			new ItemStack(Material.Sand, 1, 0),
			new ItemStack(Material.Sand, 1, 1),
			new ItemStack(Material.Gravel, 1, 0),
			new ItemStack(Material.NetherBrick, 1, 0),
			new ItemStack(Material.Netherrack, 1, 0),
			new ItemStack(Material.Bedrock, 1, 0),
			new ItemStack(Material.CobbleStairs, 1, 0),
			new ItemStack(Material.OakWoodStairs, 1, 0),
			new ItemStack(Material.SpruceWoodStairs, 1, 0),
			new ItemStack(Material.BirchWoodStairs, 1, 0),
			new ItemStack(Material.JungleWoodStairs, 1, 0),
			new ItemStack(Material.AcaciaWoodStairs, 1, 0),
			new ItemStack(Material.DarkOakWoodStairs, 1, 0),
			new ItemStack(Material.BrickStairs, 1, 0),
			new ItemStack(Material.SandstoneStairs, 1, 0),
			new ItemStack(Material.StoneBrickStairs, 1, 0),
			new ItemStack(Material.NetherBrickStairs, 1, 0),
			new ItemStack(Material.QuartzStairs, 1, 0),
			new ItemStack(Material.StoneSlab, 1, 0),
			new ItemStack(Material.StoneSlab, 1, 1),
			new ItemStack(Material.StoneSlab, 1, 2),
			new ItemStack(Material.StoneSlab, 1, 3),
			new ItemStack(Material.StoneSlab, 1, 4),
			new ItemStack(Material.StoneSlab, 1, 5),
			new ItemStack(Material.StoneSlab, 1, 6),
			new ItemStack(Material.WoodSlab, 1, 0),
			new ItemStack(Material.WoodSlab, 1, 1),
			new ItemStack(Material.WoodSlab, 1, 2),
			new ItemStack(Material.WoodSlab, 1, 3),
			new ItemStack(Material.WoodSlab, 1, 4),
			new ItemStack(Material.WoodSlab, 1, 5),
			new ItemStack(Material.QuartzBlock, 1, 0),
			new ItemStack(Material.QuartzBlock, 1, 1),
			new ItemStack(Material.QuartzBlock, 1, 2),
			new ItemStack(Material.CoalOre, 1, 0),
			new ItemStack(Material.IronOre, 1, 0),
			new ItemStack(Material.GoldOre, 1, 0),
			new ItemStack(Material.DiamondOre, 1, 0),
			new ItemStack(Material.LapisOre, 1, 0),
			new ItemStack(Material.RedstoneOre, 1, 0),
			new ItemStack(Material.EmeraldOre, 1, 0),
			new ItemStack(Material.Obsidian, 1, 0),
			new ItemStack(Material.Ice, 1, 0),
			new ItemStack(Material.Snow, 1, 0),
			new ItemStack(Material.EndStone, 1, 0),
			new ItemStack(Material.CobbleWall, 1, 0),
			new ItemStack(Material.CobbleWall, 1, 1),
			new ItemStack(Material.LapisBlock, 1, 0),
			new ItemStack(Material.CoalBlock, 1, 0),
			new ItemStack(Material.EmeraldBlock, 1, 0),
			new ItemStack(Material.RedstoneBlock, 1, 0),
			new ItemStack(Material.SnowLayer, 1, 0),
			new ItemStack(Material.Glass, 1, 0),
			new ItemStack(Material.Glowstone, 1, 0),
			new ItemStack(Material.Vines, 1, 0),
			new ItemStack(Material.Ladder, 1, 0),
			new ItemStack(Material.Sponge, 1, 0),
			new ItemStack(Material.GlassPane, 1, 0),
			new ItemStack(Material.WoodDoorItem, 1, 0),
			new ItemStack(Material.Trapdoor, 1, 0),
			new ItemStack(Material.Fence, 1, 0),
			new ItemStack(Material.Fence, 1, 0),
			new ItemStack(Material.Fence, 1, 1),
			new ItemStack(Material.Fence, 1, 2),
			new ItemStack(Material.Fence, 1, 3),
			new ItemStack(Material.Fence, 1, 4),
			new ItemStack(Material.Fence, 1, 5),
			new ItemStack(Material.FenceGate, 1, 0),
			new ItemStack(Material.BirchFenceGate, 1, 0),
			new ItemStack(Material.SpruceFenceGate, 1, 0),
			new ItemStack(Material.DarkOakFenceGate, 1, 0),
			new ItemStack(Material.JungleFenceGate, 1, 0),
			new ItemStack(Material.AcaciaFenceGate, 1, 0),
			new ItemStack(Material.IronBars, 1, 0),
			new ItemStack(Material.Bed, 1, 0),
			new ItemStack(Material.Bookshelf, 1, 0),
			new ItemStack(Material.Painting, 1, 0),
			new ItemStack(Material.CraftingTable, 1, 0),
			new ItemStack(Material.StoneCutter, 1, 0),
			new ItemStack(Material.Chest, 1, 0),
			new ItemStack(Material.Furnace, 1, 0),
			new ItemStack(Material.EndPortal, 1, 0),
			new ItemStack(Material.Dandelion, 1, 0),
			new ItemStack(Material.Flower, 1, 0),
			new ItemStack(Material.Flower, 1, 1),
			new ItemStack(Material.Flower, 1, 2),
			new ItemStack(Material.Flower, 1, 3),
			new ItemStack(Material.Flower, 1, 4),
			new ItemStack(Material.Flower, 1, 5),
			new ItemStack(Material.Flower, 1, 6),
			new ItemStack(Material.Flower, 1, 7),
			new ItemStack(Material.Flower, 1, 8),
			new ItemStack(Material.BrownMushroomBlock, 1, 0),
			new ItemStack(Material.RedMushroomBlock, 1, 0),
			new ItemStack(Material.Cactus, 1, 0),
			new ItemStack(Material.MelonBlock, 1, 0),
			new ItemStack(Material.Pumpkin, 1, 0),
			new ItemStack(Material.PumpkinLantern, 1, 0),
			new ItemStack(Material.Cobweb, 1, 0),
			new ItemStack(Material.Cobweb, 1, 0),
			new ItemStack(Material.HayBale, 1, 0),
			new ItemStack(Material.TallGrass, 1, 1),
			new ItemStack(Material.TallGrass, 1, 2),
			new ItemStack(Material.DeadBush, 1, 0),
			new ItemStack(Material.Sapling, 1, 0),
			new ItemStack(Material.Sapling, 1, 1),
			new ItemStack(Material.Sapling, 1, 2),
			new ItemStack(Material.Sapling, 1, 3),
			new ItemStack(Material.Sapling, 1, 4),
			new ItemStack(Material.Sapling, 1, 5),
			new ItemStack(Material.Leaves, 1, 0),
			new ItemStack(Material.Leaves, 1, 1),
			new ItemStack(Material.Leaves, 1, 2),
			new ItemStack(Material.Leaves, 1, 3),
			new ItemStack(Material.AcaciaLeaves, 1, 0),
			new ItemStack(Material.AcaciaLeaves, 1, 1),
			new ItemStack(Material.CakeItem, 1, 0),
			new ItemStack(Material.SignItem, 1, 0),
			new ItemStack(Material.MonsterSpawner, 1, 0),
			new ItemStack(Material.Wool, 1, 0),
			new ItemStack(Material.Wool, 1, 1),
			new ItemStack(Material.Wool, 1, 2),
			new ItemStack(Material.Wool, 1, 3),
			new ItemStack(Material.Wool, 1, 4),
			new ItemStack(Material.Wool, 1, 5),
			new ItemStack(Material.Wool, 1, 6),
			new ItemStack(Material.Wool, 1, 7),
			new ItemStack(Material.Wool, 1, 8),
			new ItemStack(Material.Wool, 1, 9),
			new ItemStack(Material.Wool, 1, 10),
			new ItemStack(Material.Wool, 1, 11),
			new ItemStack(Material.Wool, 1, 12),
			new ItemStack(Material.Wool, 1, 13),
			new ItemStack(Material.Wool, 1, 14),
			new ItemStack(Material.Wool, 1, 15),
			new ItemStack(Material.Wool, 1, 16),
			new ItemStack(Material.Carpet, 1, 0),
			new ItemStack(Material.Carpet, 1, 1),
			new ItemStack(Material.Carpet, 1, 2),
			new ItemStack(Material.Carpet, 1, 3),
			new ItemStack(Material.Carpet, 1, 4),
			new ItemStack(Material.Carpet, 1, 5),
			new ItemStack(Material.Carpet, 1, 6),
			new ItemStack(Material.Carpet, 1, 7),
			new ItemStack(Material.Carpet, 1, 8),
			new ItemStack(Material.Carpet, 1, 9),
			new ItemStack(Material.Carpet, 1, 10),
			new ItemStack(Material.Carpet, 1, 11),
			new ItemStack(Material.Carpet, 1, 12),
			new ItemStack(Material.Carpet, 1, 13),
			new ItemStack(Material.Carpet, 1, 14),
			new ItemStack(Material.Carpet, 1, 15),
			new ItemStack(Material.Carpet, 1, 16),
			new ItemStack(Material.Torch, 1, 0),
			new ItemStack(Material.Bucket, 1, 0),
			new ItemStack(Material.Bucket, 1, 1),
			new ItemStack(Material.Bucket, 1, 8),
			new ItemStack(Material.Bucket, 1, 10),
			new ItemStack(Material.TNT, 1, 0),
			new ItemStack(Material.IronHoe, 1, 0),
			new ItemStack(Material.IronShovel, 1, 0),
			new ItemStack(Material.IronSword, 1, 0),
			new ItemStack(Material.Bow, 1, 0),
			new ItemStack(Material.Shears, 1, 0),
			new ItemStack(Material.FlintAndSteel, 1, 0),
			new ItemStack(Material.Clock, 1, 0),
			new ItemStack(Material.Compass, 1, 0),
			new ItemStack(Material.Minecart, 1, 0),
			new ItemStack(Material.Snowball, 1, 0),
			new ItemStack(Material.SugarCane, 1, 0),
			new ItemStack(Material.Wheat, 1, 0),
			new ItemStack(Material.SeedsItem, 1, 0),
			new ItemStack(Material.MelonSeeds, 1, 0),
			new ItemStack(Material.PumpkinSeeds, 1, 0),
			new ItemStack(Material.CarrotItem, 1, 0),
			new ItemStack(Material.BeetrootSeeds, 1, 0),
			new ItemStack(Material.Egg, 1, 0),
			new ItemStack(Material.RawFish, 1, 0),
			new ItemStack(Material.RawFish, 1, 1),
			new ItemStack(Material.RawFish, 1, 2),
			new ItemStack(Material.RawFish, 1, 3),
			new ItemStack(Material.CookedFish, 1, 0),
			new ItemStack(Material.CookedFish, 1, 1),
			new ItemStack(Material.Dye, 1, 0),
			new ItemStack(Material.Dye, 1, 1),
			new ItemStack(Material.Dye, 1, 2),
			new ItemStack(Material.Dye, 1, 3),
			new ItemStack(Material.Dye, 1, 4),
			new ItemStack(Material.Dye, 1, 5),
			new ItemStack(Material.Dye, 1, 6),
			new ItemStack(Material.Dye, 1, 7),
			new ItemStack(Material.Dye, 1, 8),
			new ItemStack(Material.Dye, 1, 9),
			new ItemStack(Material.Dye, 1, 10),
			new ItemStack(Material.Dye, 1, 11),
			new ItemStack(Material.Dye, 1, 12),
			new ItemStack(Material.Dye, 1, 13),
			new ItemStack(Material.Dye, 1, 14),
			new ItemStack(Material.Dye, 1, 15),
		};
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