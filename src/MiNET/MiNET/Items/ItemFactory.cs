using MiNET.Blocks;

namespace MiNET.Items
{
	public interface ICustomItemFactory
	{
		Item GetItem(int id, short metadata);
	}

	public interface ICustomBlockItemFactory
	{
		ItemBlock GetBlockItem(Block block, short metadata);
	}

	public class ItemFactory
	{
		public static ICustomItemFactory CustomItemFactory { get; set; }
		public static ICustomBlockItemFactory CustomBlockItemFactory { get; set; }

		public static Item GetItem(int id)
		{
			return GetItem(id, 0);
		}

		public static Item GetItem(int id, short metadata)
		{
			Item item = null;

			if (CustomItemFactory != null)
			{
				item = CustomItemFactory.GetItem(id, metadata);
			}

			if (item != null) return item;

            if (id == 54) item = new ItemChest(metadata);
            else if (id == 44) item = new ItemSlab(id, metadata);
            else if (id == 61) item = new ItemFurnace(metadata);
            else if (id == 63) item = new ItemSign(metadata);
            else if (id == 68) item = new ItemSign(metadata);
            else if (id == 116) item = new ItemEnchantingTable(metadata);
            else if (id == 158) item = new ItemSlab(id, metadata);
            else if (id == 256) item = new ItemIronShovel(metadata);
            else if (id == 258) item = new ItemIronAxe(metadata);
            else if (id == 259) item = new ItemFlintAndSteel(metadata);
            else if (id == 260) item = new ItemApple();
            else if (id == 261) item = new ItemBow(metadata);
            else if (id == 263) item = new ItemCoal(metadata);
            else if (id == 267) item = new ItemIronSword(metadata);
            else if (id == 268) item = new ItemWoodenSword(metadata);
            else if (id == 269) item = new ItemWoodenShovel(metadata);
            else if (id == 270) item = new ItemWoodenPickaxe(metadata);
            else if (id == 271) item = new ItemWoodenAxe(metadata);
            else if (id == 272) item = new ItemStoneSword(metadata);
            else if (id == 273) item = new ItemStoneShovel(metadata);
            else if (id == 275) item = new ItemStoneAxe(metadata);
            else if (id == 276) item = new ItemDiamondSword(metadata);
            else if (id == 277) item = new ItemDiamondShovel(metadata);
            else if (id == 279) item = new ItemDiamondAxe(metadata);
            else if (id == 280) item = new ItemStick(metadata);
            else if (id == 283) item = new ItemGoldSword(metadata);
            else if (id == 284) item = new ItemGoldShovel(metadata);
            else if (id == 286) item = new ItemGoldAxe(metadata);
            else if (id == 290) item = new ItemWoodenHoe(metadata);
            else if (id == 291) item = new ItemStoneHoe(metadata);
            else if (id == 292) item = new ItemIronHoe(metadata);
            else if (id == 293) item = new ItemDiamondHoe(metadata);
            else if (id == 294) item = new ItemGoldHoe(metadata);
            else if (id == 297) item = new ItemBread();
            else if (id == 298) item = new ItemLeatherHelmet(metadata);
            else if (id == 299) item = new ItemLeatherChestplate(metadata);
            else if (id == 300) item = new ItemLeatherLeggings(metadata);
            else if (id == 301) item = new ItemLeatherBoots(metadata);
            else if (id == 302) item = new ItemChainmailHelmet(metadata);
            else if (id == 303) item = new ItemChainmailChestplate(metadata);
            else if (id == 304) item = new ItemChainmailLeggings(metadata);
            else if (id == 305) item = new ItemChainmailBoots(metadata);
            else if (id == 309) item = new ItemIronBoots(metadata);
            else if (id == 308) item = new ItemIronLeggings(metadata);
            else if (id == 307) item = new ItemIronChestplate(metadata);
            else if (id == 306) item = new ItemIronHelmet(metadata);
            else if (id == 310) item = new ItemDiamondHelmet(metadata);
            else if (id == 311) item = new ItemDiamondChestplate(metadata);
            else if (id == 312) item = new ItemDiamondLeggings(metadata);
            else if (id == 313) item = new ItemDiamondBoots(metadata);
            else if (id == 314) item = new ItemGoldHelmet(metadata);
            else if (id == 315) item = new ItemGoldChestplate(metadata);
            else if (id == 316) item = new ItemGoldLeggings(metadata);
            else if (id == 317) item = new ItemGoldBoots(metadata);
            else if (id == 319) item = new ItemRawPorkchop(metadata);
            else if (id == 320) item = new ItemCookedPorkshop();
            else if (id == 322) item = new ItemGoldenApple();
            else if (id == 323) item = new ItemSign(metadata);
            else if (id == 324) item = new ItemDoor(metadata);
            else if (id == 325) item = new ItemBucket(metadata);
            else if (id == 332) item = new ItemSnowball(metadata);
            else if (id == 344) item = new ItemEgg(metadata);
            else if (id == 351) item = new ItemDye(metadata);
            else if (id == 355) item = new ItemBed(metadata);
            else if (id == 357) item = new ItemCookie();
            else if (id == 360) item = new ItemMelonSlice();
            else if (id == 363) item = new ItemRawBeef();
            else if (id == 364) item = new ItemSteak();
            else if (id == 365) item = new ItemRawChicken();
            else if (id == 366) item = new ItemCoockedChicken();
            else if (id == 383) item = new ItemSpawnEgg(metadata);
            else if (id == 391) item = new ItemCarrot();
            else if (id == 392) item = new ItemPotato();
            else if (id == 393) item = new ItemBakedPotato();
            else if (id == 397) item = new ItemMobHead(metadata);
            else if (id == 400) item = new ItemPumpkinPie();
            else if (id <= 255)
            {
                Block block = BlockFactory.GetBlockById((byte)id);
                if (CustomBlockItemFactory == null)
                {
                    item = new ItemBlock(block, metadata);
                }
                else
                {
                    item = CustomBlockItemFactory.GetBlockItem(block, metadata);
                }
            }
            else item = new Item(id, metadata);

			return item;
		}
	}
}