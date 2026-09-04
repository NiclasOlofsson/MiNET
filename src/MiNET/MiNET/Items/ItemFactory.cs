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
using System.Linq;
using System.Reflection;
using log4net;
using MiNET.Blocks;
using MiNET.Utils;
using Newtonsoft.Json;

namespace MiNET.Items
{
	public interface ICustomItemFactory
	{
		Item GetItem(string name, short metadata, int count);
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

		/// <summary>
		///     The item type dictionary this protocol version declares to clients. Generated from the
		///     pinned Bedrock data submodule, so there is no data file to read at startup.
		/// </summary>
		public static ItemRegistry ItemRegistry { get; } = CreateRegistry();

		private static ItemRegistry CreateRegistry()
		{
			var registry = new ItemRegistry();
			ItemRegistryData.Create(registry);
			return registry;
		}

		/// <summary>
		///     The identity a renamed one became ("minecraft:reeds" -> "minecraft:sugar_cane"), for
		///     code holding a name from an older registry. False when the name was never renamed,
		///     which includes every current name.
		/// </summary>
		public static bool TryGetCurrentName(string oldName, out string currentName)
		{
			return _itemRenames.Value.TryGetValue(oldName, out currentName);
		}

		/// <summary>
		///     The item registry network id for a registry string id, or 0 when the name is not in the
		///     item registry. No item is numbered 0, and 0 is the empty stack everywhere on the wire,
		///     so an unknown name degrades to an empty slot rather than to some other item.
		/// </summary>
		public static short GetNetworkIdByName(string name)
		{
			return ItemRegistry.GetNetworkId(name);
		}

		/// <summary>
		///     Builds the item a wire network id refers to. The id is resolved back to the registry
		///     string id and the item is built from that, so the decode path and the code path agree
		///     on identity. An id the registry doesn't know is an empty stack, not a guess.
		/// </summary>
		public static Item GetItemByNetworkId(int networkId, short metadata = 0, int count = 1)
		{
			string name = ItemRegistry.GetName((short) networkId);
			if (name == null)
			{
				Log.Warn($"Unknown item network id {networkId}, treating as empty");
				return new ItemAir();
			}

			return GetItemByName(name, metadata, count);
		}

		/// <summary>
		///     The block an item name places. Identical to the item name, except for the 17 surviving
		///     "minecraft:item.x" twins, whose block is the same name with "item." dropped.
		/// </summary>
		private static string BlockNameOf(string itemName)
		{
			return itemName.StartsWith("minecraft:item.", StringComparison.OrdinalIgnoreCase)
				? "minecraft:" + itemName.Substring("minecraft:item.".Length)
				: itemName;
		}

		private static string NormalizeItemKey(string name)
		{
			return name.ToLowerInvariant().Replace("_", "").Replace("minecraft:", "");
		}

		// Identities the registry renamed ("minecraft:reeds" -> "minecraft:sugar_cane"), old name to
		// current. Only the "simple" one-to-one renames; the "complex" section splits one old name
		// across several current ones by metadata and belongs to the world-load upgrade path, not here.
		private static readonly Lazy<Dictionary<string, string>> _itemRenames = new Lazy<Dictionary<string, string>>(() =>
			new Dictionary<string, string>(ResourceUtil.ReadResource<R16ToCurrentMap>("r16_to_current_item_map.json", typeof(Item), "Data").Simple, StringComparer.OrdinalIgnoreCase));

		// Typed classes for plain (non-block) items, keyed by the class name minus its "Item"
		// prefix, normalized the same way a lookup name is normalized. Discovered by reflection so
		// newly generated classes need no hand-maintained registration.
		//
		// Also carries the registry's renames (r16_to_current_item_map.json "simple" section):
		// when the registry renamed an identity (e.g. "minecraft:melon" -> "minecraft:melon_slice"),
		// the class written for the old name is aliased under the new, current name too. The old
		// name stays the key that resolves - the alias only adds the current name as a second way
		// in; it never replaces or renumbers anything.
		private static readonly Lazy<Dictionary<string, Type>> _typedItemTypeByClassName = new Lazy<Dictionary<string, Type>>(() =>
		{
			var map = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);
			foreach (Type t in typeof(Item).Assembly.GetTypes())
			{
				if (t == typeof(Item) || t == typeof(ItemBlock) || !typeof(Item).IsAssignableFrom(t) || t.IsAbstract) continue;
				if (t.GetConstructor(Type.EmptyTypes) == null) continue;
				if (!t.Name.StartsWith("Item", StringComparison.Ordinal)) continue;
				map.TryAdd(NormalizeItemKey(t.Name.Substring(4)), t);
			}

			var r16 = ResourceUtil.ReadResource<R16ToCurrentMap>("r16_to_current_item_map.json", typeof(Item), "Data");
			foreach (KeyValuePair<string, string> rename in r16.Simple)
			{
				string oldKey = NormalizeItemKey(rename.Key);
				string newKey = NormalizeItemKey(rename.Value);
				if (map.TryGetValue(oldKey, out Type existing)) map.TryAdd(newKey, existing);
			}

			return map;
		});

		// Name-first resolution: registry string ids are the durable identity in modern Bedrock.
		// Block-items resolve name -> block name (identity, or the exceptions map) -> palette
		// default state -> ItemBlock; plain items resolve to a typed class when the registry
		// name (or a rename of it) has one, else a generic Item carrying the name and its
		// registry network id. Legacy short ids are not involved.
		public static Item GetItemByName(string name, short metadata = 0, int count = 1)
		{
			if (string.IsNullOrEmpty(name)) return new ItemAir();
			if (name.IndexOf(':') < 0) name = "minecraft:" + name;

			Item custom = CustomItemFactory?.GetItem(name, metadata, count);
			if (custom != null) return custom;

			Block block = BlockFactory.GetBlockByName(BlockNameOf(name));

			Item item;
			if (block != null)
			{
				item = CustomBlockItemFactory == null
					? new ItemBlock(block, metadata)
					: CustomBlockItemFactory.GetBlockItem(block, metadata, count);

				// A block's name is normally an item name too, but not always: the sugar cane block
				// is "minecraft:reeds" and its item was renamed to "minecraft:sugar_cane". Land the
				// item on a real registry identity, or it has no network id and goes out as empty.
				if (!ItemRegistry.Contains(name) && _itemRenames.Value.TryGetValue(name, out string renamed)) name = renamed;
			}
			else if (_typedItemTypeByClassName.Value.TryGetValue(NormalizeItemKey(name), out Type itemType))
			{
				item = (Item) Activator.CreateInstance(itemType);
			}
			else
			{
				item = new Item(name, metadata);
			}

			item.Metadata = metadata;
			item.Count = (byte) count;
			item.Name = name;

			if (ItemRegistry.TryGetByName(name, out ItemRegistryEntry entry)) item.NetworkId = entry.NetworkId;

			return item;
		}

	}

	public class ItemRabbit : Item { public ItemRabbit() : base("minecraft:rabbit") {} }
	public class ItemMushroomStew : Item { public ItemMushroomStew() : base("minecraft:mushroom_stew") {} }
	public class ItemMusicDiscWard : Item { public ItemMusicDiscWard() : base("minecraft:music_disc_ward") {} }
	public class ItemEnchantedApple : Item { public ItemEnchantedApple() : base("minecraft:enchanted_golden_apple") {} }
	public class ItemCod : Item { public ItemCod() : base("minecraft:cod") {} }
	public class ItemSalmon : Item { public ItemSalmon() : base("minecraft:salmon") {} }
	public class ItemTropicalFish : Item { public ItemTropicalFish() : base("minecraft:tropical_fish") {} }
	public class ItemPufferfish : Item { public ItemPufferfish() : base("minecraft:pufferfish") {} }
	public class ItemCookedCod : Item { public ItemCookedCod() : base("minecraft:cooked_cod") {} }
	public class ItemCookedSalmon : Item { public ItemCookedSalmon() : base("minecraft:cooked_salmon") {} }
	public class ItemSparkler : Item { public ItemSparkler() : base("minecraft:sparkler") {} }
	public class ItemDriedKelp : Item { public ItemDriedKelp() : base("minecraft:dried_kelp") {} }
	public class ItemNautilusShell : Item { public ItemNautilusShell() : base("minecraft:nautilus_shell") {} }
	public class ItemComparator : Item { public ItemComparator() : base("minecraft:comparator") {} }
	public class ItemRottenFlesh : Item { public ItemRottenFlesh() : base("minecraft:rotten_flesh") {} }
	public class ItemRabbitFoot : Item { public ItemRabbitFoot() : base("minecraft:rabbit_foot") {} }
	public class ItemLingeringPotion : Item { public ItemLingeringPotion() : base("minecraft:lingering_potion") {} }
	public class ItemCampfire : Item { public ItemCampfire() : base("minecraft:campfire") {} }
	public class ItemMusicDiscFar : Item { public ItemMusicDiscFar() : base("minecraft:music_disc_far") {} }
	public class ItemSpiderEye : Item { public ItemSpiderEye() : base("minecraft:spider_eye") {} }
	public class ItemPoisonousPotato : Item { public ItemPoisonousPotato() : base("minecraft:poisonous_potato") {} }
	public class ItemBeetrootSoup : Item { public ItemBeetrootSoup() : base("minecraft:beetroot_soup") {} }
	public class ItemSweetBerries : Item { public ItemSweetBerries() : base("minecraft:sweet_berries") {} }
	public class ItemCookedRabbit : Item { public ItemCookedRabbit() : base("minecraft:cooked_rabbit") {} }
	public class ItemRabbitStew : Item { public ItemRabbitStew() : base("minecraft:rabbit_stew") {} }
	public class ItemPumpkinSeeds : Item { public ItemPumpkinSeeds() : base("minecraft:pumpkin_seeds") {} }
	public class ItemCommandBlockMinecart : Item { public ItemCommandBlockMinecart() : base("minecraft:command_block_minecart") {} }
	public class ItemMelonSeeds : Item { public ItemMelonSeeds() : base("minecraft:melon_seeds") {} }
	public class ItemNetherWart : Item { public ItemNetherWart() : base("minecraft:nether_wart") {} }
	public class ItemMusicDiscStrad : Item { public ItemMusicDiscStrad() : base("minecraft:music_disc_strad") {} }
	public class ItemBowl : Item { public ItemBowl() : base("minecraft:bowl") {} }
	public class ItemString : Item { public ItemString() : base("minecraft:string") {} }
	public class ItemFeather : Item { public ItemFeather() : base("minecraft:feather") {} }
	public class ItemGunpowder : Item { public ItemGunpowder() : base("minecraft:gunpowder") {} }
	public class ItemMusicDiscMellohi : Item { public ItemMusicDiscMellohi() : base("minecraft:music_disc_mellohi") {} }
	public class ItemEnderEye : Item { public ItemEnderEye() : base("minecraft:ender_eye") {} }
	public class ItemShield : Item { public ItemShield() : base("minecraft:shield") {} }
	public class ItemFlint : Item { public ItemFlint() : base("minecraft:flint") {} }
	public class ItemHeartOfTheSea : Item { public ItemHeartOfTheSea() : base("minecraft:heart_of_the_sea") {} }
	public class ItemMinecart : Item { public ItemMinecart() : base("minecraft:minecart") {} }
	public class ItemWrittenBook : Item { public ItemWrittenBook() : base("minecraft:written_book") {} }
	public class ItemLeather : Item { public ItemLeather() : base("minecraft:leather") {} }
	public class ItemKelp : Item { public ItemKelp() : base("minecraft:kelp") {} }
	public class ItemBrick : Item { public ItemBrick() : base("minecraft:brick") {} }
	public class ItemClayBall : Item { public ItemClayBall() : base("minecraft:clay_ball") {} }
	public class ItemCarrotOnAStick : Item { public ItemCarrotOnAStick() : base("minecraft:carrot_on_a_stick") {} }
	public class ItemSugarCane : Item { public ItemSugarCane() : base("minecraft:sugar_cane") {} }
	public class ItemPaper : Item { public ItemPaper() : base("minecraft:paper") {} }
	public class ItemTrident : Item { public ItemTrident() : base("minecraft:trident") {} }
	public class ItemSlimeBall : Item { public ItemSlimeBall() : base("minecraft:slime_ball") {} }
	public class ItemChestMinecart : Item { public ItemChestMinecart() : base("minecraft:chest_minecart") {} }
	public class ItemFishingRod : Item { public ItemFishingRod() : base("minecraft:fishing_rod") {} }
	public class ItemClock : Item { public ItemClock() : base("minecraft:clock") {} }
	public class ItemGlowstoneDust : Item { public ItemGlowstoneDust() : base("minecraft:glowstone_dust") {} }
	public class ItemNameTag : Item { public ItemNameTag() : base("minecraft:name_tag") {} }
	public class ItemCake : Item { public ItemCake() : base("minecraft:cake") {} }
	public class ItemRepeater : Item { public ItemRepeater() : base("minecraft:repeater") {} }
	public class ItemEnderPearl : Item { public ItemEnderPearl() : base("minecraft:ender_pearl") {} }
	public class ItemGhastTear : Item { public ItemGhastTear() : base("minecraft:ghast_tear") {} }
	public class ItemGlassBottle : Item { public ItemGlassBottle() : base("minecraft:glass_bottle") {} }
	public class ItemFermentedSpiderEye : Item { public ItemFermentedSpiderEye() : base("minecraft:fermented_spider_eye") {} }
	public class ItemMagmaCream : Item { public ItemMagmaCream() : base("minecraft:magma_cream") {} }
	public class ItemBrewingStand : Item { public ItemBrewingStand() : base("minecraft:brewing_stand") {} }
	public class ItemRapidFertilizer : Item { public ItemRapidFertilizer() : base("minecraft:rapid_fertilizer") {} } // what is this?
	public class ItemGlisteningMelonSlice : Item { public ItemGlisteningMelonSlice() : base("minecraft:glistering_melon_slice") {} }
	public class ItemExperienceBottle : Item { public ItemExperienceBottle() : base("minecraft:experience_bottle") {} }
	public class ItemFireCharge : Item { public ItemFireCharge() : base("minecraft:fire_charge") {} }
	public class ItemWritableBook : Item { public ItemWritableBook() : base("minecraft:writable_book") {} }
	public class ItemEmerald : Item { public ItemEmerald() : base("minecraft:emerald") {} }
	public class ItemMusicDiscPigstep : Item { public ItemMusicDiscPigstep() : base("minecraft:music_disc_pigstep") {} }
	public class ItemFlowerPot : Item { public ItemFlowerPot() : base("minecraft:flower_pot") {} }
	public class ItemNetherStar : Item { public ItemNetherStar() : base("minecraft:nether_star") {} }
	public class ItemHopperMinecart : Item { public ItemHopperMinecart() : base("minecraft:hopper_minecart") {} }
	public class ItemFireworkStar : Item { public ItemFireworkStar() : base("minecraft:firework_star") {} }
	public class ItemNetherbrick : Item { public ItemNetherbrick() : base("minecraft:netherbrick") {} }
	public class ItemQuartz : Item { public ItemQuartz() : base("minecraft:quartz") {} }
	public class ItemTntMinecart : Item { public ItemTntMinecart() : base("minecraft:tnt_minecart") {} }
	public class ItemHopper : Item { public ItemHopper() : base("minecraft:hopper") {} }
	public class ItemDragonBreath : Item { public ItemDragonBreath() : base("minecraft:dragon_breath") {} }
	public class ItemRabbitHide : Item { public ItemRabbitHide() : base("minecraft:rabbit_hide") {} }
	public class ItemMusicDisc13 : Item { public ItemMusicDisc13() : base("minecraft:music_disc_13") {} }
	public class ItemMusicDiscCat : Item { public ItemMusicDiscCat() : base("minecraft:music_disc_cat") {} }
	public class ItemMusicDiscBlocks : Item { public ItemMusicDiscBlocks() : base("minecraft:music_disc_blocks") {} }
	public class ItemMusicDiscChirp : Item { public ItemMusicDiscChirp() : base("minecraft:music_disc_chirp") {} }
	public class ItemMusicDiscMall : Item { public ItemMusicDiscMall() : base("minecraft:music_disc_mall") {} }
	public class ItemMusicDiscStal : Item { public ItemMusicDiscStal() : base("minecraft:music_disc_stal") {} }
	public class ItemMusicDisc11 : Item { public ItemMusicDisc11() : base("minecraft:music_disc_11") {} }
	public class ItemMusicDiscWait : Item { public ItemMusicDiscWait() : base("minecraft:music_disc_wait") {} }
	public class ItemLead : Item { public ItemLead() : base("minecraft:lead") {} }
	public class ItemPrismarineCrystals : Item { public ItemPrismarineCrystals() : base("minecraft:prismarine_crystals") {} }
	public class ItemArmorStand : Item { public ItemArmorStand() : base("minecraft:armor_stand") {} }
	public class ItemPhantomMembrane : Item { public ItemPhantomMembrane() : base("minecraft:phantom_membrane") {} }
	public class ItemChorusFruit : Item { public ItemChorusFruit() : base("minecraft:chorus_fruit") {} }
	public class ItemSuspiciousStew : Item { public ItemSuspiciousStew() : base("minecraft:suspicious_stew") {} }
	public class ItemPoppedChorusFruit : Item { public ItemPoppedChorusFruit() : base("minecraft:popped_chorus_fruit") {} }
	public class ItemSplashPotion : Item { public ItemSplashPotion() : base("minecraft:splash_potion") {} }
	public class ItemPrismarineShard : Item { public ItemPrismarineShard() : base("minecraft:prismarine_shard") {} }
	public class ItemShulkerShell : Item { public ItemShulkerShell() : base("minecraft:shulker_shell") {} }
	public class ItemTotemOfUndying : Item { public ItemTotemOfUndying() : base("minecraft:totem_of_undying") {} }
	public class ItemTurtleScute : Item { public ItemTurtleScute() : base("minecraft:turtle_scute") {} }
	public class ItemCrossbow : Item { public ItemCrossbow() : base("minecraft:crossbow") {} }
	public class ItemBalloon : Item { public ItemBalloon() : base("minecraft:balloon") {} }
	public class ItemBannerPattern : Item { public ItemBannerPattern() : base("minecraft:banner_pattern") {} }
	public class ItemHoneycomb : Item { public ItemHoneycomb() : base("minecraft:honeycomb") {} }
	public class ItemHoneyBottle : Item { public ItemHoneyBottle() : base("minecraft:honey_bottle") {} }
	public class ItemCompound : Item { public ItemCompound() : base("minecraft:compound") {} }
	public class ItemIceBomb : Item { public ItemIceBomb() : base("minecraft:ice_bomb") {} }
	public class ItemBleach : Item { public ItemBleach() : base("minecraft:bleach") {} } // A Trump item?
	public class ItemMedicine : Item { public ItemMedicine() : base("minecraft:medicine") {} } // Corona?
	public class ItemLodestoneCompass : Item { public ItemLodestoneCompass() : base("minecraft:lodestone_compass") {} }
	public class ItemNetheriteIngot : Item { public ItemNetheriteIngot() : base("minecraft:netherite_ingot") {} }
	public class ItemNetheriteScrap : Item { public ItemNetheriteScrap() : base("minecraft:netherite_scrap") {} }
	public class ItemIronChain : Item { public ItemIronChain() : base("minecraft:iron_chain") {} }
	public class ItemWarpedFungusOnAStick : Item { public ItemWarpedFungusOnAStick() : base("minecraft:warped_fungus_on_a_stick") {} }
	public class ItemNetherSprouts : Item { public ItemNetherSprouts() : base("minecraft:nether_sprouts") {} }
	public class ItemSoulCampfire : Item { public ItemSoulCampfire() : base("minecraft:soul_campfire") {} }
	public class ItemEndCrystal : Item { public ItemEndCrystal() : base("minecraft:end_crystal") {} }

}