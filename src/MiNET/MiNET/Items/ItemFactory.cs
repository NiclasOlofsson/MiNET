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
using MiNET.Blocks;

namespace MiNET.Items
{
	public interface ICustomItemFactory
	{
		Item GetItem(short id, short metadata, int count);
	}

	public interface ICustomBlockItemFactory
	{
		ItemBlock GetBlockItem(Block block, short metadata, int count);
	}

	public class ItemFactory
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(ItemFactory));

		public static ICustomItemFactory CustomItemFactory { get; set; }
		public static ICustomBlockItemFactory CustomBlockItemFactory { get; set; }

		public static Dictionary<string, short> NameToId { get; private set; }
		public static Itemstates Itemstates { get; set; } = new Itemstates();

		static ItemFactory()
		{
			NameToId = BuildNameToId();

			var assembly = Assembly.GetAssembly(typeof(Item));
			using (var stream = assembly.GetManifestResourceStream(typeof(Item).Namespace + ".itemstates.json"))
			using (var reader = new StreamReader(stream))
			{
				Itemstates = Itemstates.FromJson(reader.ReadToEnd());
			}
		}

		private static Dictionary<string, short> BuildNameToId()
		{
			var nameToId = new Dictionary<string, short>();
			for (short idx = -600; idx < 800; idx++)
			{
				Item item = GetItem(idx);
				string name = item.GetType().Name.ToLowerInvariant();

				if (name.Equals("item"))
				{
					//if (Log.IsDebugEnabled)
					//	Log.Debug($"Missing implementation for item ID={idx}");
					continue;
				}

				if (name.Equals("itemblock"))
				{
					ItemBlock itemBlock = item as ItemBlock;
					if (itemBlock != null)
					{
						Block block = itemBlock.Block;
						name = block?.GetType().Name.ToLowerInvariant();
						if (name == null || name.Equals("block"))
						{
							continue;
						}
					}
				}
				else
				{
					name = name.Substring(4);
				}

				try
				{
					nameToId.Remove(name); // This is in case a block was added that have item that should be used.
					nameToId.Add(name, idx);
				}
				catch (Exception e)
				{
					Log.Error($"Tried to add duplicate item for {name} {idx}");
				}
			}

			return nameToId;
		}

		public static short GetItemIdByName(string itemName)
		{
			itemName = itemName.ToLowerInvariant();
			itemName = itemName.Replace("_", "");

			if (NameToId.ContainsKey(itemName))
			{
				return NameToId[itemName];
			}

			return (short) BlockFactory.GetBlockIdByName(itemName);
		}

		public static Item GetItem(string name, short metadata = 0, int count = 1)
		{
			return GetItem(GetItemIdByName(name), metadata, count);
		}

		public static Item GetItem(short id, short metadata = 0, int count = 1)
		{
			Item item = null;

			if (CustomItemFactory != null)
			{
				item = CustomItemFactory.GetItem(id, metadata, count);
			}

			if (item != null) return item;

			if (id == 0) item = new ItemAir();
			else if (id == 256) item = new ItemIronShovel();
			else if (id == 257) item = new ItemIronPickaxe();
			else if (id == 258) item = new ItemIronAxe();
			else if (id == 259) item = new ItemFlintAndSteel();
			else if (id == 260) item = new ItemApple();
			else if (id == 261) item = new ItemBow();
			else if (id == 262) item = new ItemArrow();
			else if (id == 263) item = new ItemCoal();
			else if (id == 264) item = new ItemDiamond();
			else if (id == 265) item = new ItemIronIngot();
			else if (id == 266) item = new ItemGoldIngot();
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
			else if (id == 281) item = new ItemBowl();
			else if (id == 282) item = new ItemMushroomStew();
			else if (id == 283) item = new ItemGoldenSword();
			else if (id == 284) item = new ItemGoldenShovel();
			else if (id == 285) item = new ItemGoldenPickaxe();
			else if (id == 286) item = new ItemGoldenAxe();
			else if (id == 287) item = new ItemString();
			else if (id == 288) item = new ItemFeather();
			else if (id == 289) item = new ItemGunpowder();
			else if (id == 290) item = new ItemWoodenHoe();
			else if (id == 291) item = new ItemStoneHoe();
			else if (id == 292) item = new ItemIronHoe();
			else if (id == 293) item = new ItemDiamondHoe();
			else if (id == 294) item = new ItemGoldenHoe();
			else if (id == 295) item = new ItemWheatSeeds();
			else if (id == 296) item = new ItemWheat();
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
			else if (id == 318) item = new ItemFlint();
			else if (id == 319) item = new ItemRawPorkchop();
			else if (id == 320) item = new ItemCookedPorkshop();
			else if (id == 321) item = new ItemPainting();
			else if (id == 322) item = new ItemGoldenApple();
			else if (id == 323) item = new ItemSign();
			else if (id == 324) item = new ItemWoodenDoor();
			else if (id == 325) item = new ItemBucket(metadata);
			else if (id == 328) item = new ItemMinecart();
			else if (id == 329) item = new ItemSaddle();
			else if (id == 330) item = new ItemIronDoor();
			else if (id == 331) item = new ItemRedstone();
			else if (id == 332) item = new ItemSnowball();
			else if (id == 333) item = new ItemBoat(metadata);
			else if (id == 334) item = new ItemLeather();
			else if (id == 335) item = new ItemKelp();
			else if (id == 336) item = new ItemBrick();
			else if (id == 337) item = new ItemClayBall();
			else if (id == 338) item = new ItemReeds();
			else if (id == 339) item = new ItemPaper();
			else if (id == 340) item = new ItemBook();
			else if (id == 341) item = new ItemSlimeBall();
			else if (id == 342) item = new ItemChestMinecart();
			else if (id == 344) item = new ItemEgg();
			else if (id == 345) item = new ItemCompass();
			else if (id == 346) item = new ItemFishingRod();
			else if (id == 347) item = new ItemClock();
			else if (id == 348) item = new ItemGlowstoneDust();
			else if (id == 349) item = new ItemFish();
			else if (id == 350) item = new ItemCookedFish();
			else if (id == 351) item = new ItemDye();
			else if (id == 352) item = new ItemBone();
			else if (id == 353) item = new ItemSugar();
			else if (id == 354) item = new ItemCake();
			else if (id == 355) item = new ItemBed();
			else if (id == 356) item = new ItemRepeater();
			else if (id == 357) item = new ItemCookie();
			else if (id == 358) item = new ItemMap();
			else if (id == 359) item = new ItemShears();
			else if (id == 360) item = new ItemMelonSlice();
			else if (id == 361) item = new ItemPumpkinSeeds();
			else if (id == 362) item = new ItemMelonSeeds();
			else if (id == 363) item = new ItemBeef();
			else if (id == 364) item = new ItemCookedBeef();
			else if (id == 365) item = new ItemRawChicken();
			else if (id == 366) item = new ItemCookedChicken();
			else if (id == 367) item = new ItemRottenFlesh();
			else if (id == 368) item = new ItemEnderPearl();
			else if (id == 369) item = new ItemBlazeRod();
			else if (id == 370) item = new ItemGhastTear();
			else if (id == 371) item = new ItemGoldNugget();
			else if (id == 372) item = new ItemNetherWart();
			else if (id == 373) item = new ItemPotion(metadata);
			else if (id == 374) item = new ItemGlassBottle();
			else if (id == 375) item = new ItemSpiderEye();
			else if (id == 376) item = new ItemFermentedSpiderEye();
			else if (id == 377) item = new ItemBlazePowder();
			else if (id == 378) item = new ItemMagmaCream();
			else if (id == 379) item = new ItemBrewingStand();
			else if (id == 380) item = new ItemCauldron();
			else if (id == 381) item = new ItemEnderEye();
			else if (id == 382) item = new ItemSpeckledMelon();
			else if (id == 383) item = new ItemMonsterEgg(metadata);
			else if (id == 389) item = new ItemFrame();
			else if (id == 384) item = new ItemExperienceBottle();
			else if (id == 385) item = new ItemFireball();
			else if (id == 386) item = new ItemWritableBook();
			else if (id == 387) item = new ItemWrittenBook();
			else if (id == 388) item = new ItemEmerald();
			else if (id == 390) item = new ItemFlowerPot();
			else if (id == 391) item = new ItemCarrot();
			else if (id == 392) item = new ItemPotato();
			else if (id == 393) item = new ItemBakedPotato();
			else if (id == 394) item = new ItemPoisonousPotato();
			else if (id == 395) item = new ItemEmptyMap();
			else if (id == 396) item = new ItemGoldenCarrot();
			else if (id == 397) item = new ItemMobHead(metadata);
			else if (id == 398) item = new ItemCarrotonastick();
			else if (id == 399) item = new ItemNetherstar();
			else if (id == 400) item = new ItemPumpkinPie();
			else if (id == 401) item = new ItemFireworks();
			else if (id == 402) item = new ItemFireworkscharge();
			else if (id == 403) item = new ItemEnchantedBook();
			else if (id == 404) item = new ItemComparator();
			else if (id == 405) item = new ItemNetherbrick();
			else if (id == 406) item = new ItemQuartz();
			else if (id == 407) item = new ItemTntMinecart();
			else if (id == 408) item = new ItemHopperMinecart();
			else if (id == 409) item = new ItemPrismarineShard();
			else if (id == 410) item = new ItemHopper();
			else if (id == 411) item = new ItemRabbit();
			else if (id == 412) item = new ItemCookedRabbit();
			else if (id == 413) item = new ItemRabbitStew();
			else if (id == 414) item = new ItemRabbitFoot();
			else if (id == 415) item = new ItemRabbitHide();
			else if (id == 416) item = new ItemHorseArmorLeather();
			else if (id == 417) item = new ItemHorseArmorIron();
			else if (id == 418) item = new ItemHorseArmorGold();
			else if (id == 419) item = new ItemHorseArmorDiamond();
			else if (id == 420) item = new ItemLead();
			else if (id == 421) item = new ItemNameTag();
			else if (id == 422) item = new ItemPrismarineCrystals();
			else if (id == 423) item = new ItemMuttonRaw();
			else if (id == 424) item = new ItemMuttonCooked();
			else if (id == 425) item = new ItemArmorStand();
			else if (id == 426) item = new ItemEndCrystal();
			else if (id == 427) item = new ItemSpruceDoor();
			else if (id == 428) item = new ItemBirchDoor();
			else if (id == 429) item = new ItemJungleDoor();
			else if (id == 430) item = new ItemAcaciaDoor();
			else if (id == 431) item = new ItemDarkOakDoor();
			else if (id == 432) item = new ItemChorusFruit();
			else if (id == 433) item = new ItemChorusFruitPopped();
			else if (id == 434) item = new ItemBannerPattern();
			else if (id == 437) item = new ItemDragonBreath();
			else if (id == 438) item = new ItemSplashPotion();
			else if (id == 441) item = new ItemLingeringPotion();
			else if (id == 442) item = new ItemSparkler();
			else if (id == 443) item = new ItemCommandBlockMinecart();
			else if (id == 444) item = new ItemElytra();
			else if (id == 445) item = new ItemShulkerShell();
			else if (id == 446) item = new ItemBanner();
			else if (id == 447) item = new ItemMedicine();
			else if (id == 448) item = new ItemBalloon();
			else if (id == 449) item = new ItemRapidFertilizer();
			else if (id == 450) item = new ItemTotem();
			else if (id == 451) item = new ItemBleach();
			else if (id == 452) item = new ItemIronNugget();
			else if (id == 453) item = new ItemIceBomb();
			else if (id == 454 && metadata == 0) item = new ItemSlate();
			else if (id == 454 && metadata == 1) item = new ItemPoster();
			else if (id == 454 && metadata == 2) item = new ItemBoard();
			else if (id == 455) item = new ItemTrident();
			else if (id == 457) item = new ItemBeetroot();
			else if (id == 458) item = new ItemBeetrootSeeds();
			else if (id == 459) item = new ItemBeetrootSoup();
			else if (id == 460) item = new ItemSalmon();
			else if (id == 461) item = new ItemClownfish();
			else if (id == 462) item = new ItemPufferfish();
			else if (id == 463) item = new ItemCookedSalmon();
			else if (id == 464) item = new ItemDriedKelp();
			else if (id == 465) item = new ItemNautilusShell();
			else if (id == 466) item = new ItemAppleenchanted();
			else if (id == 467) item = new ItemHeartOfTheSea();
			else if (id == 468) item = new ItemTurtleShellPiece();
			else if (id == 469) item = new ItemTurtleHelmet();
			else if (id == 470) item = new ItemPhantomMembrane();
			else if (id == 471) item = new ItemCrossbow();
			else if (id == 472) item = new ItemSpruceSign();
			else if (id == 473) item = new ItemBirchSign();
			else if (id == 474) item = new ItemJungleSign();
			else if (id == 475) item = new ItemAcaciaSign();
			else if (id == 476) item = new ItemDarkoakSign();
			else if (id == 477) item = new ItemSweetBerries();
			else if (id == 498) item = new ItemCamera(metadata);
			else if (id == 499) item = new ItemCompound();
			else if (id == 500) item = new ItemRecord13();
			else if (id == 501) item = new ItemRecordCat();
			else if (id == 502) item = new ItemRecordBlocks();
			else if (id == 503) item = new ItemRecordChirp();
			else if (id == 504) item = new ItemRecordFar();
			else if (id == 505) item = new ItemRecordMall();
			else if (id == 506) item = new ItemRecordMellohi();
			else if (id == 507) item = new ItemRecordStal();
			else if (id == 508) item = new ItemRecordStrad();
			else if (id == 509) item = new ItemRecordWard();
			else if (id == 510) item = new ItemRecord11();
			else if (id == 511) item = new ItemRecordWait();
			else if (id == 513) item = new ItemShield();
			else if (id == 720) item = new ItemCampfire();
			else if (id == 734) item = new ItemSuspiciousStew();
			else if (id == 736) item = new ItemHoneycomb();
			else if (id == 737) item = new ItemHoneyBottle();
			else if (id == 741) item = new ItemLodestonecompass();
			else if (id == 742) item = new ItemNetheriteIngot();
			else if (id == 743) item = new ItemNetheriteSword();
			else if (id == 744) item = new ItemNetheriteShovel();
			else if (id == 745) item = new ItemNetheritePickaxe();
			else if (id == 746) item = new ItemNetheriteAxe();
			else if (id == 747) item = new ItemNetheriteHoe();
			else if (id == 748) item = new ItemNetheriteHelmet();
			else if (id == 749) item = new ItemNetheriteChestplate();
			else if (id == 750) item = new ItemNetheriteLeggings();
			else if (id == 751) item = new ItemNetheriteBoots();
			else if (id == 752) item = new ItemNetheriteScrap();
			else if (id == 753) item = new ItemCrimsonSign();
			else if (id == 754) item = new ItemWarpedSign();
			else if (id == 755) item = new ItemCrimsonDoor();
			else if (id == 756) item = new ItemWarpedDoor();
			else if (id == 757) item = new ItemWarpedFungusOnAStick();
			else if (id == 758) item = new ItemChain();
			else if (id == 759) item = new ItemRecordPigstep();
			else if (id == 760) item = new ItemNetherSprouts();
			else if (id == 801) item = new ItemSoulCampfire();

			else if (id <= 255)
			{
				int blockId = id;
				if (blockId < 0) blockId = (short) (Math.Abs(id) + 255); // hehe
				Block block = BlockFactory.GetBlockById(blockId);
				var runtimeId = BlockFactory.GetRuntimeId(blockId, (byte) metadata);
				if (runtimeId < BlockFactory.BlockPalette.Count)
				{
					var blockState = BlockFactory.BlockPalette[(int) runtimeId];
					block.SetState(blockState);
				}

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
			item.Count = (byte) count;

			return item;
		}
	}

	public class ItemRabbit : Item { public ItemRabbit() : base(411) {} }
	public class ItemMushroomStew : Item { public ItemMushroomStew() : base(282) {} }
	public class ItemRecordWard : Item { public ItemRecordWard() : base(509) {} }
	public class ItemAppleenchanted : Item { public ItemAppleenchanted() : base(466) {} }
	public class ItemFish : Item { public ItemFish() : base(349) {} }
	public class ItemSalmon : Item { public ItemSalmon() : base(460) {} }
	public class ItemClownfish : Item { public ItemClownfish() : base(461) {} }
	public class ItemPufferfish : Item { public ItemPufferfish() : base(462) {} }
	public class ItemCookedFish : Item { public ItemCookedFish() : base(350) {} }
	public class ItemCookedSalmon : Item { public ItemCookedSalmon() : base(463) {} }
	public class ItemSparkler : Item { public ItemSparkler() : base(442) {} }
	public class ItemDriedKelp : Item { public ItemDriedKelp() : base(464) {} }
	public class ItemNautilusShell : Item { public ItemNautilusShell() : base(465) {} }
	public class ItemComparator : Item { public ItemComparator() : base(404) {} }
	public class ItemRottenFlesh : Item { public ItemRottenFlesh() : base(367) {} }
	public class ItemRabbitFoot : Item { public ItemRabbitFoot() : base(414) {} }
	public class ItemLingeringPotion : Item { public ItemLingeringPotion() : base(441) {} }
	public class ItemCampfire : Item { public ItemCampfire() : base(720) {} }
	public class ItemRecordFar : Item { public ItemRecordFar() : base(504) {} }
	public class ItemSpiderEye : Item { public ItemSpiderEye() : base(375) {} }
	public class ItemPoisonousPotato : Item { public ItemPoisonousPotato() : base(394) {} }
	public class ItemBeetrootSoup : Item { public ItemBeetrootSoup() : base(459) {} }
	public class ItemSweetBerries : Item { public ItemSweetBerries() : base(477) {} }
	public class ItemCookedRabbit : Item { public ItemCookedRabbit() : base(412) {} }
	public class ItemRabbitStew : Item { public ItemRabbitStew() : base(413) {} }
	public class ItemPumpkinSeeds : Item { public ItemPumpkinSeeds() : base(361) {} }
	public class ItemCommandBlockMinecart : Item { public ItemCommandBlockMinecart() : base(443) {} }
	public class ItemMelonSeeds : Item { public ItemMelonSeeds() : base(362) {} }
	public class ItemNetherWart : Item { public ItemNetherWart() : base(372) {} }
	public class ItemRecordStrad : Item { public ItemRecordStrad() : base(508) {} }
	public class ItemBowl : Item { public ItemBowl() : base(281) {} }
	public class ItemString : Item { public ItemString() : base(287) {} }
	public class ItemFeather : Item { public ItemFeather() : base(288) {} }
	public class ItemGunpowder : Item { public ItemGunpowder() : base(289) {} }
	public class ItemRecordMellohi : Item { public ItemRecordMellohi() : base(506) {} }
	public class ItemEnderEye : Item { public ItemEnderEye() : base(381) {} }
	public class ItemShield : Item { public ItemShield() : base(513) {} }
	public class ItemFlint : Item { public ItemFlint() : base(318) {} }
	public class ItemHeartOfTheSea : Item { public ItemHeartOfTheSea() : base(467) {} }
	public class ItemMinecart : Item { public ItemMinecart() : base(328) {} }
	public class ItemWrittenBook : Item { public ItemWrittenBook() : base(387) {} }
	public class ItemLeather : Item { public ItemLeather() : base(334) {} }
	public class ItemKelp : Item { public ItemKelp() : base(335) {} }
	public class ItemBrick : Item { public ItemBrick() : base(336) {} }
	public class ItemClayBall : Item { public ItemClayBall() : base(337) {} }
	public class ItemCarrotonastick : Item { public ItemCarrotonastick() : base(398) {} }
	public class ItemReeds : Item { public ItemReeds() : base(338) {} }
	public class ItemPaper : Item { public ItemPaper() : base(339) {} }
	public class ItemTrident : Item { public ItemTrident() : base(455) {} }
	public class ItemSlimeBall : Item { public ItemSlimeBall() : base(341) {} }
	public class ItemChestMinecart : Item { public ItemChestMinecart() : base(342) {} }
	public class ItemFishingRod : Item { public ItemFishingRod() : base(346) {} }
	public class ItemClock : Item { public ItemClock() : base(347) {} }
	public class ItemGlowstoneDust : Item { public ItemGlowstoneDust() : base(348) {} }
	public class ItemNameTag : Item { public ItemNameTag() : base(421) {} }
	public class ItemCake : Item { public ItemCake() : base(354) {} }
	public class ItemRepeater : Item { public ItemRepeater() : base(356) {} }
	public class ItemEnderPearl : Item { public ItemEnderPearl() : base(368) {} }
	public class ItemGhastTear : Item { public ItemGhastTear() : base(370) {} }
	public class ItemGlassBottle : Item { public ItemGlassBottle() : base(374) {} }
	public class ItemFermentedSpiderEye : Item { public ItemFermentedSpiderEye() : base(376) {} }
	public class ItemMagmaCream : Item { public ItemMagmaCream() : base(378) {} }
	public class ItemBrewingStand : Item { public ItemBrewingStand() : base(379) {} }
	public class ItemRapidFertilizer : Item { public ItemRapidFertilizer() : base(449) {} }
	public class ItemSpeckledMelon : Item { public ItemSpeckledMelon() : base(382) {} }
	public class ItemExperienceBottle : Item { public ItemExperienceBottle() : base(384) {} }
	public class ItemFireball : Item { public ItemFireball() : base(385) {} }
	public class ItemWritableBook : Item { public ItemWritableBook() : base(386) {} }
	public class ItemEmerald : Item { public ItemEmerald() : base(388) {} }
	public class ItemRecordPigstep : Item { public ItemRecordPigstep() : base(759) {} }
	public class ItemFlowerPot : Item { public ItemFlowerPot() : base(390) {} }
	public class ItemNetherstar : Item { public ItemNetherstar() : base(399) {} }
	public class ItemHopperMinecart : Item { public ItemHopperMinecart() : base(408) {} }
	public class ItemFireworkscharge : Item { public ItemFireworkscharge() : base(402) {} }
	public class ItemNetherbrick : Item { public ItemNetherbrick() : base(405) {} }
	public class ItemQuartz : Item { public ItemQuartz() : base(406) {} }
	public class ItemTntMinecart : Item { public ItemTntMinecart() : base(407) {} }
	public class ItemHopper : Item { public ItemHopper() : base(410) {} }
	public class ItemDragonBreath : Item { public ItemDragonBreath() : base(437) {} }
	public class ItemRabbitHide : Item { public ItemRabbitHide() : base(415) {} }
	public class ItemRecord13 : Item { public ItemRecord13() : base(500) {} }
	public class ItemRecordCat : Item { public ItemRecordCat() : base(501) {} }
	public class ItemRecordBlocks : Item { public ItemRecordBlocks() : base(502) {} }
	public class ItemRecordChirp : Item { public ItemRecordChirp() : base(503) {} }
	public class ItemRecordMall : Item { public ItemRecordMall() : base(505) {} }
	public class ItemRecordStal : Item { public ItemRecordStal() : base(507) {} }
	public class ItemRecord11 : Item { public ItemRecord11() : base(510) {} }
	public class ItemRecordWait : Item { public ItemRecordWait() : base(511) {} }
	public class ItemLead : Item { public ItemLead() : base(420) {} }
	public class ItemPrismarineCrystals : Item { public ItemPrismarineCrystals() : base(422) {} }
	public class ItemArmorStand : Item { public ItemArmorStand() : base(425) {} }
	public class ItemPhantomMembrane : Item { public ItemPhantomMembrane() : base(470) {} }
	public class ItemChorusFruit : Item { public ItemChorusFruit() : base(432) {} }
	public class ItemSuspiciousStew : Item { public ItemSuspiciousStew() : base(734) {} }
	public class ItemChorusFruitPopped : Item { public ItemChorusFruitPopped() : base(433) {} }
	public class ItemSplashPotion : Item { public ItemSplashPotion() : base(438) {} }
	public class ItemPrismarineShard : Item { public ItemPrismarineShard() : base(409) {} }
	public class ItemShulkerShell : Item { public ItemShulkerShell() : base(445) {} }
	public class ItemTotem : Item { public ItemTotem() : base(450) {} }
	public class ItemTurtleShellPiece : Item { public ItemTurtleShellPiece() : base(468) {} }
	public class ItemCrossbow : Item { public ItemCrossbow() : base(471) {} }
	public class ItemBalloon : Item { public ItemBalloon() : base(448) {} }
	public class ItemBannerPattern : Item { public ItemBannerPattern() : base(434) {} }
	public class ItemHoneycomb : Item { public ItemHoneycomb() : base(736) {} }
	public class ItemHoneyBottle : Item { public ItemHoneyBottle() : base(737) {} }
	public class ItemCompound : Item { public ItemCompound() : base(499) {} }
	public class ItemIceBomb : Item { public ItemIceBomb() : base(453) {} }
	public class ItemBleach : Item { public ItemBleach() : base(451) {} }
	public class ItemMedicine : Item { public ItemMedicine() : base(447) {} }
	public class ItemLodestonecompass : Item { public ItemLodestonecompass() : base(741) {} }
	public class ItemNetheriteIngot : Item { public ItemNetheriteIngot() : base(742) {} }
	public class ItemNetheriteScrap : Item { public ItemNetheriteScrap() : base(752) {} }
	public class ItemChain : Item { public ItemChain() : base(758) {} }
	public class ItemWarpedFungusOnAStick : Item { public ItemWarpedFungusOnAStick() : base(757) {} }
	public class ItemNetherSprouts : Item { public ItemNetherSprouts() : base(760) {} }
	public class ItemSoulCampfire : Item { public ItemSoulCampfire() : base(801) {} }
	public class ItemEndCrystal : Item { public ItemEndCrystal() : base(426) {} }

}