using MiNET.Blocks;

namespace MiNET.Items
{
	public interface ICustomItemFactory
	{
		Item GetItem(short id, short metadata, byte count);
	}

	public interface ICustomBlockItemFactory
	{
		ItemBlock GetBlockItem(Block block, short metadata, byte count);
	}

	public class ItemFactory
	{
		public static ICustomItemFactory CustomItemFactory { get; set; }
		public static ICustomBlockItemFactory CustomBlockItemFactory { get; set; }

		public static Item GetItem(short id, short metadata = 0, byte count = 1)
		{
			Item item = null;

			if (CustomItemFactory != null)
			{
				item = CustomItemFactory.GetItem(id, metadata, count);
			}

			if (item != null) return item;

			if (id == 0) item = new ItemAir();
			else if (id == 54) item = new ItemChest();
			else if (id == 44) item = new ItemSlab(id, metadata);
			else if (id == 61) item = new ItemFurnace();
			else if (id == 63) item = new ItemSign();
			else if (id == 68) item = new ItemSign();
			else if (id == 116) item = new ItemEnchantingTable();
			else if (id == 130) item = new ItemEnderChest();
			else if (id == 158) item = new ItemSlab(id, metadata);
			else if (id == 199) item = new ItemItemFrame();
			else if (id == 256) item = new ItemIronShovel();
			else if (id == 257) item = new ItemIronPickaxe();
			else if (id == 258) item = new ItemIronAxe();
			else if (id == 259) item = new ItemFlintAndSteel();
			else if (id == 260) item = new ItemApple();
			else if (id == 261) item = new ItemBow();
			else if (id == 262) item = new ItemArrow();
			else if (id == 263) item = new ItemCoal();
			else if (id == 267) item = new ItemIronSword();
			else if (id == 268) item = new ItemWoodenSword();
			else if (id == 269) item = new ItemWoodenShovel();
			else if (id == 270) item = new ItemWoodenPickaxe();
			else if (id == 271) item = new ItemWoodenAxe();
			else if (id == 272) item = new ItemStoneSword();
			else if (id == 273) item = new ItemStoneShovel();
			else if (id == 274) item = new ItemStonePickaxe();
			else if (id == 275) item = new ItemStoneAxe();
			else if (id == 276) item = new ItemDiamondSword();
			else if (id == 277) item = new ItemDiamondShovel();
			else if (id == 278) item = new ItemDiamondPickaxe();
			else if (id == 279) item = new ItemDiamondAxe();
			else if (id == 280) item = new ItemStick();
			else if (id == 283) item = new ItemGoldSword();
			else if (id == 284) item = new ItemGoldShovel();
			else if (id == 285) item = new ItemGoldPickaxe();
			else if (id == 286) item = new ItemGoldAxe();
			else if (id == 290) item = new ItemWoodenHoe();
			else if (id == 291) item = new ItemStoneHoe();
			else if (id == 292) item = new ItemIronHoe();
			else if (id == 293) item = new ItemDiamondHoe();
			else if (id == 294) item = new ItemGoldHoe();
			else if (id == 297) item = new ItemBread();
			else if (id == 298) item = new ItemLeatherHelmet();
			else if (id == 299) item = new ItemLeatherChestplate();
			else if (id == 300) item = new ItemLeatherLeggings();
			else if (id == 301) item = new ItemLeatherBoots();
			else if (id == 302) item = new ItemChainmailHelmet();
			else if (id == 303) item = new ItemChainmailChestplate();
			else if (id == 304) item = new ItemChainmailLeggings();
			else if (id == 305) item = new ItemChainmailBoots();
			else if (id == 309) item = new ItemIronBoots();
			else if (id == 308) item = new ItemIronLeggings();
			else if (id == 307) item = new ItemIronChestplate();
			else if (id == 306) item = new ItemIronHelmet();
			else if (id == 310) item = new ItemDiamondHelmet();
			else if (id == 311) item = new ItemDiamondChestplate();
			else if (id == 312) item = new ItemDiamondLeggings();
			else if (id == 313) item = new ItemDiamondBoots();
			else if (id == 314) item = new ItemGoldHelmet();
			else if (id == 315) item = new ItemGoldChestplate();
			else if (id == 316) item = new ItemGoldLeggings();
			else if (id == 317) item = new ItemGoldBoots();
			else if (id == 319) item = new ItemRawPorkchop();
			else if (id == 320) item = new ItemCookedPorkshop();
			else if (id == 322) item = new ItemGoldenApple();
			else if (id == 323) item = new ItemSign();
			else if (id == 324) item = new ItemDoor();
			else if (id == 325) item = new ItemBucket(metadata);
			else if (id == 331) item = new ItemRedstone();
			else if (id == 332) item = new ItemSnowball();
			else if (id == 344) item = new ItemEgg();
			else if (id == 345) item = new ItemCompass();
			else if (id == 355) item = new ItemBed();
			else if (id == 357) item = new ItemCookie();
			else if (id == 358) item = new ItemMap();
			else if (id == 360) item = new ItemMelonSlice();
			else if (id == 363) item = new ItemRawBeef();
			else if (id == 364) item = new ItemSteak();
			else if (id == 365) item = new ItemRawChicken();
			else if (id == 366) item = new ItemCoockedChicken();
			else if (id == 373) item = new ItemPotion(metadata);
			else if (id == 383) item = new ItemSpawnEgg(metadata);
			else if (id == 391) item = new ItemCarrot();
			else if (id == 392) item = new ItemPotato();
			else if (id == 393) item = new ItemBakedPotato();
			else if (id == 395) item = new ItemEmptyMap();
			else if (id == 397) item = new ItemMobHead(metadata);
			else if (id == 400) item = new ItemPumpkinPie();
			else if (id <= 255)
			{
				Block block = BlockFactory.GetBlockById((byte) id);
				if (CustomBlockItemFactory == null)
				{
					item = new ItemBlock(block, metadata);
				}
				else
				{
					item = CustomBlockItemFactory.GetBlockItem(block, metadata, count);
				}
			}
			else item = new Item(id, metadata, count);

			// This might now be a good idea if the constructor changes these
			// properties for custom items.
			item.Metadata = metadata;
			item.Count = count;

			return item;
		}
	}
}