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

			if (id > 255)
			{
				//If item is greater than 255 it's item
				//If item is less than 256 it's a block
				switch(id)
				{
					case 256:
						item = new ItemIronShovel(metadata);
						break;
					case 257:
						item = new ItemIronPickaxe(metadata); //Added to list - 10/29/2015
						break;
					case 258:
						item = new ItemIronAxe(metadata);
						break;
					case 259:
						item = new ItemFlintAndSteel(metadata);
						break;
					case 260:
						item = new ItemApple();
						break;
					case 261:
						item = new ItemBow(metadata);
						break;
					case 262:
						item = new ItemArrow(); //Added to list - 10/29/2015
						break;
					case 263:
						item = new ItemCoal(metadata);
						break;
					case 264:
						item = new ItemDiamond(); //Added to list - 10/29/2015
						break;
					case 265:
						item = new ItemIronIngot(); //Added to list - 10/29/2015
						break;
					case 266:
						item = new ItemGoldIngot(); //Added to list - 10/29/2015
						break;
					case 267:
						item = new ItemIronSword(metadata);
						break;
					case 268:
						item = new ItemWoodenSword(metadata);
						break;
					case 269:
						item = new ItemWoodenShovel(metadata);
						break;
					case 270:
						item = new ItemWoodenPickaxe(metadata);
						break;
					case 271:
						item = new ItemWoodenAxe(metadata);
						break;
					case 272:
						item = new ItemStoneSword(metadata);
						break;
					case 273:
						item = new ItemStoneShovel(metadata);
						break;
					case 274:
						item = new ItemStonePickaxe(metadata); //Added to list - 10/29/2015
						break;
					case 275:
						item = new ItemStoneAxe(metadata);
						break;
					case 276:
						item = new ItemDiamondSword(metadata);
						break;
					case 277:
						item = new ItemDiamondShovel(metadata);
						break;
					case 278:
						item = new ItemDiamondPickaxe(metadata); //Added to list - 10/29/2015
						break;
					case 279:
						item = new ItemDiamondAxe(metadata);
						break;
					case 280:
						item = new ItemStick(metadata);
						break;
					case 281:
						item = new ItemBowl(); //Added to list - 10/29/2015
						break;
					case 282:
						item = new ItemMushroomStew();  //Added to list - 10/29/2015
						break;
					case 283:
						item = new ItemGoldSword(metadata);
						break;
					case 284:
						item = new ItemGoldShovel(metadata);
						break;
					case 285:
						item = new ItemGoldPickaxe(metadata); //Added to list - 10/29/2015
						break;
					case 286:
						item = new ItemGoldAxe(metadata);
						break;
					case 287:
						item = new ItemString(); //Added to list - 10/29/2015
						break;
					case 288:
						item = new ItemFeather(); //Added to list - 10/29/2015
						break;
					case 289:
						item = new ItemGunpowder(); //Added to list - 10/29/2015
						break;
					case 290:
						item = new ItemWoodenHoe(metadata);
						break;
					case 291:
						item = new ItemStoneHoe(metadata);
						break;
					case 292:
						item = new ItemIronHoe(metadata);
						break;
					case 293:
						item = new ItemDiamondHoe(metadata);
						break;
					case 294:
						item = new ItemGoldHoe(metadata);
						break;
					case 295:
						item = new ItemSeeds(); //Added to list - 10/29/2015
						break;
					case 296:
						item = new ItemWheat(); //Added to list - 10/29/2015
						break;
					case 297:
						item = new ItemBread();
						break;
					case 298:
						item = new ItemLeatherCap(metadata); //Added to list - 10/29/2015
						break;
					case 299:
						item = new ItemLeatherTunic(metadata); //Added to list - 10/29/2015
						break;
					case 300:
						item = new ItemLeatherPants(metadata); //Added to list - 10/29/2015
						break;
					case 301:
						item = new ItemLeatherBoots(metadata); //Added to list - 10/29/2015
						break;
					case 302:
						item = new ItemChainHelmet(metadata); //Added to list - 10/29/2015
						break;
					case 303:
						item = new ItemChainChestplate(metadata); //Added to list - 10/29/2015
						break;
					case 304:
						item = new ItemChainLeggings(metadata); //Added to list - 10/29/2015
						break;
					case 305:
						item = new ItemChainBoots(metadata); //Added to list - 10/29/2015
						break;
					case 306:
						item = new ItemIronHelmet(metadata);
						break;
					case 307:
						item = new ItemIronChestplate(metadata);
						break;
					case 308:
						item = new ItemIronLeggings(metadata);
						break;
					case 309:
						item = new ItemIronBoots(metadata);
						break;
					case 310:
						item = new ItemDiamondHelmet(metadata);
						break;
					case 311:
						item = new ItemDiamondChestplate(metadata);
						break;
					case 312:
						item = new ItemDiamondLeggings(metadata);
						break;
					case 313:
						item = new ItemDiamondBoots(metadata);
						break;
					case 314:
						item = new ItemGoldenHelmet(metadata); //Added to list - 10/29/2015
						break;
					case 315:
						item = new ItemGoldenChestplate(metadata); //Added to list - 10/29/2015
						break;
					case 316:
						item = new ItemGoldenLeggings(metadata); //Added to list - 10/29/2015
						break;
					case 317:
						item = new ItemGoldenBoots(metadata); //Added to list - 10/29/2015
						break;
					case 318:
						item = new ItemFlint(); //Added to list - 10/29/2015
						break;
					case 319:
						item = new ItemRawPorkchop(metadata);
						break;
					case 320:
						item = new ItemCookedPorkshop();
						break;
					case 321:
						item = new ItemPainting(); //Added to list - 10/29/2015
						break;
					case 322:
						item = new ItemGoldenApple(metadata); //Added metadata parameter - 10/29/2015
						break;
					case 323:
						item = new ItemSign(metadata);
						break;
					case 324:
						item = new ItemDoor(metadata);
						break;
					case 325:
						item = new ItemBucket(metadata);
						break;
					case 328:
						item = new ItemMinecart(metadata); //Added to list - 10/29/2015
						break;
					case 329:
						item = new ItemSaddle(metadata); //Added to list - 10/29/2015
						break;
					case 330:
						item = new ItemIronDoor(metadata); //Added to list - 10/29/2015
						break;
					case 331:
						item = new ItemRedstone(metadata); //Added to list - 10/29/2015
						break;
					case 332:
						item = new ItemSnowball(metadata);
						break;
					case 333:
						item = new ItemBoat(); //Added to list - 10/29/2015
						break;
					case 334:
						item = new ItemLeather(); //Added to list - 10/29/2015
						break;
					case 336:
						item = new ItemBrick(); //Added to list - 10/29/2015
						break;
					case 337:
						item = new ItemClay(); //Added to list - 10/29/2015
						break;
					case 338:
						item = new ItemSugarCane(); //Added to list - 10/29/2015
						break;
					case 339:
						item = new ItemPaper(); //Added to list - 10/29/2015
						break;
					case 340:
						item = new ItemBook(); //Added to list - 10/29/2015
						break;
					case 341:
						item = new ItemSlimeball(); //Added to list - 10/29/2015
						break;
					case 344:
						item = new ItemEgg(metadata);
						break;
					case 345:
						item = new ItemCompass(); //Added to list - 10/29/2015
						break;
					case 346:
						item = new ItemFishingRod(metadata); //Added to list - 10/29/2015
						break;
					case 347:
						item = new ItemClock(); //Added to list - 10/29/2015
						break;
					case 348:
						item = new ItemGlowstoneDust(); //Added to list - 10/29/2015
						break;
					case 349:
						item = new ItemRawFish(); //Added to list - 10/29/2015
						break;
					case 350:
						item = new ItemCookedFish(); //Added to list - 10/29/2015
						break;
					case 351:
						item = new ItemDye(metadata); //Added to list - 10/29/2015
						break;
					case 352:
						item = new ItemBone(); //Added to list - 10/29/2015
						break;
					case 353:
						item = new ItemSugar(); //Added to list - 10/29/2015
						break;
					case 354:
						item = new ItemCake(); //Added to list - 10/29/2015
						break;
					case 355:
						item = new ItemBed(metadata);
						break;
					case 357:
						item = new ItemCookie();
						break;
					case 359:
						item = new ItemShears(metadata); //Added to list - 10/29/2015
						break;
					case 360:
						item = new ItemMelonSlice();
						break;
					case 361:
						item = new ItemPumpkinSeeds(); //Added to list - 10/29/2015
						break;
					case 362:
						item = new ItemMelonSeeds(); //Added to list - 10/29/2015
						break;
					case 363:
						item = new ItemRawBeef();
						break;
					case 364:
						item = new ItemSteak();
						break;
					case 365:
						item = new ItemRawChicken();
						break;
					case 366:
						item = new ItemCoockedChicken();
						break;
					case 367:
						item = new ItemRottenFlesh(); //Added to list - 10/29/2015
						break;
					case 368:
						item = new ItemBlazeRod(); //Added to list - 10/29/2015
						break;
					case 369:
						item = new ItemGhastTear(); //Added to list - 10/29/2015
						break;
					case 370:
						item = new ItemGoldNugget(); //Added to list - 10/29/2015
						break;
					case 371:
						item = new ItemNetherWart(); //Added to list - 10/29/2015
						break;
					case 372:
						item = new ItemPotion(metadata); //Added to list - 10/29/2015
						break;
					case 373:
						item = new ItemSpiderEye(); //Added to list - 10/29/2015
						break;
					case 374:
						item = new ItemFermentedSpiderEye(); //Added to list - 10/29/2015
						break;
					case 375:
						item = new ItemBlazePowder(); //Added to list - 10/29/2015
						break;
					case 376:
						item = new ItemMagmaCream(); //Added to list - 10/29/2015
						break;
					case 377:
						item = new ItemBrewingStand(); //Added to list - 10/29/2015
						break;
					case 378:
						item = new ItemSlimeball(); //Added to list - 10/29/2015
						break;
					case 383:
						item = new ItemSpawnEgg(metadata);
						break;
					case 384:
						item = new ItemBottleOfEnchanting(); //Added to list - 10/29/2015
						break;
					case 388:
						item = new ItemEmerald(); //Added to list - 10/29/2015
						break;
					case 390:
						item = new ItemFlowerPot(); //Added to list - 10/29/2015
						break;
					case 391:
						item = new ItemCarrot();
						break;
					case 392:
						item = new ItemPotato();
						break;
					case 393:
						item = new ItemBakedPotato();
						break;
					case 394:
						item = new ItemPoisonousPotato(); //Added to list - 10/29/2015
						break;
					case 396:
						item = new ItemGoldenCarrot(); //Added to list - 10/29/2015
						break;
					case 397:
						item = new ItemMobHead(metadata); //Added to list - 10/29/2015
						break;
					case 400:
						item = new ItemPumpkinPie();
						break;
					case 403:
						item = new ItemEnchantedBook(metadata); //Added to list - 10/29/2015
						break;
					case 405:
						item = new ItemNetherBrick(); //Added to list - 10/29/2015
						break;
					case 406:
						item = new ItemNetherQuartz(); //Added to list - 10/29/2015
						break;
					case 414:
						item = new ItemRabbitsFoot(); //Added to list - 10/29/2015
						break;
					case 438:
						item = new ItemSplashPotion(metadata); //Added to list - 10/29/2015
						break;
					case 457:
						item = new ItemBeetroot(); //Added to list - 10/29/2015
						break;
					case 458:
						item = new ItemBeetrootSeeds(); //Added to list - 10/29/2015
						break;
					case 459:
						item = new ItemBeetrootSoup(); //Added to list - 10/29/2015
						break;
					default:
						item = new Item(id, metadata);
						break;
				}
				return item;
			}
			else
			{
				//If id is less than 256 - It's a block
				//Lookup item from BlockFactory
			
				Block block = BlockFactory.GetBlockById((byte) id);
				if (CustomBlockItemFactory == null)
				{
					item = new ItemBlock(block, metadata);
				}
				else
				{
					item = CustomBlockItemFactory.GetBlockItem(block, metadata);
				}
				
				return item;
			}
		}
	}
}
