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
			for (short idx = -500; idx < 500; idx++)
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
			else if (id == 283) item = new ItemGoldenSword();
			else if (id == 284) item = new ItemGoldenShovel();
			else if (id == 285) item = new ItemGoldenPickaxe();
			else if (id == 286) item = new ItemGoldenAxe();
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
			else if (id == 319) item = new ItemRawPorkchop();
			else if (id == 320) item = new ItemCookedPorkshop();
			else if (id == 321) item = new ItemPainting();
			else if (id == 322) item = new ItemGoldenApple();
			else if (id == 323) item = new ItemSign();
			else if (id == 324) item = new ItemWoodenDoor();
			else if (id == 325) item = new ItemBucket(metadata);
			else if (id == 329) item = new ItemSaddle();
			else if (id == 331) item = new ItemRedstone();
			else if (id == 332) item = new ItemSnowball();
			else if (id == 333) item = new ItemBoat(metadata);
			else if (id == 340) item = new ItemBook();
			else if (id == 344) item = new ItemEgg();
			else if (id == 345) item = new ItemCompass();
			else if (id == 351) item = new ItemDye();
			else if (id == 352) item = new ItemBone();
			else if (id == 352) item = new ItemSugar();
			else if (id == 355) item = new ItemBed();
			else if (id == 357) item = new ItemCookie();
			else if (id == 358) item = new ItemMap();
			else if (id == 359) item = new ItemShears();
			else if (id == 360) item = new ItemMelonSlice();
			else if (id == 363) item = new ItemBeef();
			else if (id == 364) item = new ItemCookedBeef();
			else if (id == 365) item = new ItemRawChicken();
			else if (id == 366) item = new ItemCookedChicken();
			else if (id == 369) item = new ItemBlazeRod();
			else if (id == 371) item = new ItemGoldNugget();
			else if (id == 373) item = new ItemPotion(metadata);
			else if (id == 377) item = new ItemBlazePowder();
			else if (id == 380) item = new ItemCauldron();
			else if (id == 383) item = new ItemMonsterEgg(metadata);
			else if (id == 389) item = new ItemFrame();
			else if (id == 391) item = new ItemCarrot();
			else if (id == 392) item = new ItemPotato();
			else if (id == 393) item = new ItemBakedPotato();
			else if (id == 395) item = new ItemEmptyMap();
			else if (id == 395) item = new ItemGoldenCarrot();
			else if (id == 397) item = new ItemMobHead(metadata);
			else if (id == 400) item = new ItemPumpkinPie();
			else if (id == 401) item = new ItemFireworks();
			else if (id == 403) item = new ItemEnchantedBook();
			else if (id == 416) item = new ItemHorseArmorLeather();
			else if (id == 417) item = new ItemHorseArmorIron();
			else if (id == 418) item = new ItemHorseArmorGold();
			else if (id == 419) item = new ItemHorseArmorDiamond();
			else if (id == 423) item = new ItemMuttonRaw();
			else if (id == 424) item = new ItemMuttonCooked();
			else if (id == 427) item = new ItemSpruceDoor();
			else if (id == 428) item = new ItemBirchDoor();
			else if (id == 429) item = new ItemJungleDoor();
			else if (id == 430) item = new ItemAcaciaDoor();
			else if (id == 431) item = new ItemDarkOakDoor();
			else if (id == 444) item = new ItemElytra();
			else if (id == 446) item = new ItemBanner();
			else if (id == 452) item = new ItemIronNugget();
			else if (id == 454 && metadata == 0) item = new ItemSlate();
			else if (id == 454 && metadata == 1) item = new ItemPoster();
			else if (id == 454 && metadata == 2) item = new ItemBoard();
			else if (id == 458) item = new ItemBeetrootSeeds();
			else if (id == 498) item = new ItemCamera(metadata);
			else if (id <= 255)
			{
				int blockId = id;
				if (blockId < 0) blockId = (short) (Math.Abs(id) + 255); // hehe
				Block block = BlockFactory.GetBlockById(blockId);
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
}