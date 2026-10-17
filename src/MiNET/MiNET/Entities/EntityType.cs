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
using System.Linq;
using fNbt;
using MiNET.Entities.Hostile;
using MiNET.Entities.Passive;
using MiNET.Worlds;

namespace MiNET.Entities
{
	public enum EntityType
	{
		None = 0,

		DroppedItem = 64,
		ExperienceOrb = 69,

		ArmorStand = 61,
		PrimedTnt = 65,
		FallingBlock = 66,

		ThrownBottleoEnchanting = 68,
		EnderEye = 70,
		EnderCrystal = 71,
		FireworksRocket = 72,
		Trident = 73,
		ShulkerBullet = 76,
		FishingRodHook = 77,
		DragonFireball = 79,
		ShotArrow = 80,
		ThrownSnowball = 81,
		ThrownEgg = 82,
		Painting = 83,
		Minecart = 84,
		GhastFireball = 85,
		ThrownSpashPotion = 86,
		ThrownEnderPerl = 87,
		LeashKnot = 88,
		WitherSkull = 89,
		Boat = 90,
		WitherSkullDangerous = 91,
		LightningBolt = 93,
		BlazeFireball = 94,
		AreaEffectCloud = 95,
		HopperMinecart = 96,
		TntMinecart = 97,
		ChestMinecart = 98,
		CommandBlockMinecart = 100,
		LingeringPotion = 101,
		LlamaSpit = 102,
		EvocationFangs = 103,
		IceBomb = 106,
		Balloon = 107,
		WindCharge = 143,
		BreezeWindCharge = 141,
		OminousItemSpawner = 145,
		Cushion = 154,
		IceBall = 156,
		ChestBoat = 218,

		Zombie = 32,
		Creeper = 33,
		Skeleton = 34,
		Spider = 35,
		ZombiePigman = 36,
		Slime = 37,
		Enderman = 38,
		Silverfish = 39,
		CaveSpider = 40,
		Ghast = 41,
		MagmaCube = 42,
		Blaze = 43,
		ZombieVillager = 44,
		Witch = 45,
		Stray = 46,
		Husk = 47,
		WitherSkeleton = 48,
		Guardian = 49,
		ElderGuardian = 50,
		Wither = 52,
		Dragon = 53,
		Shulker = 54,
		Endermite = 55,
		Vindicator = 57,
		Phantom = 58,
		Evoker = 104,
		Vex = 105,
		Drowned = 110,
		Pillager = 114,
		Ravager = 59,
		ElderGuardianGhost = 120,
		Piglin = 123,
		Hoglin = 124,
		Zoglin = 126,
		PiglinBrute = 127,
		Warden = 131,
		Breeze = 140,
		Bogged = 144,
		Creaking = 146,
		ZombieNautilus = 150,
		Parched = 151,
		CamelHusk = 152,
		SulfurCube = 153,
		Frostbite = 155,
		ZombieVillagerV2 = 116,

		Chicken = 10,
		Cow = 11,
		Pig = 12,
		Sheep = 13,
		Wolf = 14,
		Villager = 15,
		MushroomCow = 16,
		Squid = 17,
		Rabbit = 18,
		Bat = 19,
		IronGolem = 20,
		SnowGolem = 21,
		Ocelot = 22,
		Horse = 23,
		Donkey = 24,
		Mule = 25,
		SkeletonHorse = 26,
		ZombieHorse = 27,
		PolarBear = 28,
		Llama = 29,
		Parrot = 30,
		Dolphin = 31,
		Turtle = 74,
		Cat = 75,
		Pufferfish = 108,
		Salmon = 109,
		TropicalFish = 111,
		Fish = 112,
		Panda = 113,
		VillagerV2 = 115,
		WanderingTrader = 118,
		Fox = 121,
		Bee = 122,
		Strider = 125,
		Goat = 128,
		GlowSquid = 129,
		Axolotl = 130,
		Frog = 132,
		Tadpole = 133,
		Allay = 134,
		Camel = 138,
		Sniffer = 139,
		Armadillo = 142,
		HappyGhast = 147,
		CopperGolem = 148,
		Nautilus = 149,
		TraderLlama = 157,

		Player = 63,

		Npc = 51,
		Agent = 56,
		Camera = 62,
		Chalkboard = 78,

		Herobrine = 666
	}

	/// <summary>
	///     What the client is told about an entity type: its identifier, whether it has a spawn egg
	///     and whether /summon accepts it. The runtime id is the <see cref="EntityType" /> value
	///     itself, except where vanilla disagrees (only the player does, at 257).
	/// </summary>
	public record EntityIdentity(string Id, bool HasSpawnEgg, bool Summonable, string Bid = "", int? Rid = null)
	{
		public int GetRid(EntityType type)
		{
			return Rid ?? (int) type;
		}
	}

	public static class EntityHelpers
	{
		public static readonly Dictionary<EntityType, EntityIdentity> LegacyEntityTypeIdConverter = new Dictionary<EntityType, EntityIdentity>
		{
			// No spawn egg exists for the npc; it is command/editor-only, and vanilla reports it so.
			{ EntityType.Npc, new EntityIdentity("minecraft:npc", HasSpawnEgg: false, Summonable: true) },
			{ EntityType.Player, new EntityIdentity("minecraft:player", HasSpawnEgg: false, Summonable: false, Bid: "minecraft:", Rid: 257) },
			{ EntityType.WitherSkeleton, new EntityIdentity("minecraft:wither_skeleton", HasSpawnEgg: true, Summonable: true) },
			{ EntityType.Husk, new EntityIdentity("minecraft:husk", HasSpawnEgg: true, Summonable: true) },
			{ EntityType.Stray, new EntityIdentity("minecraft:stray", HasSpawnEgg: true, Summonable: true) },
			{ EntityType.Witch, new EntityIdentity("minecraft:witch", HasSpawnEgg: true, Summonable: true) },
			{ EntityType.ZombieVillager, new EntityIdentity("minecraft:zombie_villager", HasSpawnEgg: false, Summonable: true) },
			{ EntityType.Blaze, new EntityIdentity("minecraft:blaze", HasSpawnEgg: true, Summonable: true) },
			{ EntityType.MagmaCube, new EntityIdentity("minecraft:magma_cube", HasSpawnEgg: true, Summonable: true) },
			{ EntityType.Ghast, new EntityIdentity("minecraft:ghast", HasSpawnEgg: true, Summonable: true) },
			{ EntityType.CaveSpider, new EntityIdentity("minecraft:cave_spider", HasSpawnEgg: true, Summonable: true) },
			{ EntityType.Silverfish, new EntityIdentity("minecraft:silverfish", HasSpawnEgg: true, Summonable: true) },
			{ EntityType.Enderman, new EntityIdentity("minecraft:enderman", HasSpawnEgg: true, Summonable: true) },
			{ EntityType.Slime, new EntityIdentity("minecraft:slime", HasSpawnEgg: true, Summonable: true) },
			{ EntityType.ZombiePigman, new EntityIdentity("minecraft:zombie_pigman", HasSpawnEgg: true, Summonable: true) },
			{ EntityType.Spider, new EntityIdentity("minecraft:spider", HasSpawnEgg: true, Summonable: true) },
			{ EntityType.Skeleton, new EntityIdentity("minecraft:skeleton", HasSpawnEgg: true, Summonable: true) },
			{ EntityType.Creeper, new EntityIdentity("minecraft:creeper", HasSpawnEgg: true, Summonable: true) },
			{ EntityType.Zombie, new EntityIdentity("minecraft:zombie", HasSpawnEgg: true, Summonable: true) },
			{ EntityType.SkeletonHorse, new EntityIdentity("minecraft:skeleton_horse", HasSpawnEgg: true, Summonable: true) },
			{ EntityType.Mule, new EntityIdentity("minecraft:mule", HasSpawnEgg: true, Summonable: true) },
			{ EntityType.Donkey, new EntityIdentity("minecraft:donkey", HasSpawnEgg: true, Summonable: true) },
			{ EntityType.Dolphin, new EntityIdentity("minecraft:dolphin", HasSpawnEgg: true, Summonable: true) },
			{ EntityType.TropicalFish, new EntityIdentity("minecraft:tropicalfish", HasSpawnEgg: true, Summonable: true) },
			{ EntityType.Wolf, new EntityIdentity("minecraft:wolf", HasSpawnEgg: true, Summonable: true) },
			{ EntityType.Squid, new EntityIdentity("minecraft:squid", HasSpawnEgg: true, Summonable: true) },
			{ EntityType.Drowned, new EntityIdentity("minecraft:drowned", HasSpawnEgg: true, Summonable: true) },
			{ EntityType.Sheep, new EntityIdentity("minecraft:sheep", HasSpawnEgg: true, Summonable: true) },
			{ EntityType.MushroomCow, new EntityIdentity("minecraft:mooshroom", HasSpawnEgg: true, Summonable: true) },
			{ EntityType.Panda, new EntityIdentity("minecraft:panda", HasSpawnEgg: true, Summonable: true) },
			{ EntityType.Salmon, new EntityIdentity("minecraft:salmon", HasSpawnEgg: true, Summonable: true) },
			{ EntityType.Pig, new EntityIdentity("minecraft:pig", HasSpawnEgg: true, Summonable: true) },
			{ EntityType.Villager, new EntityIdentity("minecraft:villager", HasSpawnEgg: false, Summonable: true) },
			{ EntityType.Fish, new EntityIdentity("minecraft:cod", HasSpawnEgg: true, Summonable: true) },
			{ EntityType.Pufferfish, new EntityIdentity("minecraft:pufferfish", HasSpawnEgg: true, Summonable: true) },
			{ EntityType.Cow, new EntityIdentity("minecraft:cow", HasSpawnEgg: true, Summonable: true) },
			{ EntityType.Chicken, new EntityIdentity("minecraft:chicken", HasSpawnEgg: true, Summonable: true) },
			{ EntityType.Balloon, new EntityIdentity("minecraft:balloon", HasSpawnEgg: false, Summonable: false) },
			{ EntityType.Llama, new EntityIdentity("minecraft:llama", HasSpawnEgg: true, Summonable: true) },
			{ EntityType.IronGolem, new EntityIdentity("minecraft:iron_golem", HasSpawnEgg: true, Summonable: true) },
			{ EntityType.Rabbit, new EntityIdentity("minecraft:rabbit", HasSpawnEgg: true, Summonable: true) },
			{ EntityType.SnowGolem, new EntityIdentity("minecraft:snow_golem", HasSpawnEgg: true, Summonable: true) },
			{ EntityType.Bat, new EntityIdentity("minecraft:bat", HasSpawnEgg: true, Summonable: true) },
			{ EntityType.Ocelot, new EntityIdentity("minecraft:ocelot", HasSpawnEgg: true, Summonable: true) },
			{ EntityType.Horse, new EntityIdentity("minecraft:horse", HasSpawnEgg: true, Summonable: true) },
			{ EntityType.Cat, new EntityIdentity("minecraft:cat", HasSpawnEgg: true, Summonable: true) },
			{ EntityType.PolarBear, new EntityIdentity("minecraft:polar_bear", HasSpawnEgg: true, Summonable: true) },
			{ EntityType.ZombieHorse, new EntityIdentity("minecraft:zombie_horse", HasSpawnEgg: true, Summonable: true) },
			{ EntityType.Turtle, new EntityIdentity("minecraft:turtle", HasSpawnEgg: true, Summonable: true) },
			{ EntityType.Parrot, new EntityIdentity("minecraft:parrot", HasSpawnEgg: true, Summonable: true) },
			{ EntityType.Guardian, new EntityIdentity("minecraft:guardian", HasSpawnEgg: true, Summonable: true) },
			{ EntityType.ElderGuardian, new EntityIdentity("minecraft:elder_guardian", HasSpawnEgg: true, Summonable: true) },
			{ EntityType.Vindicator, new EntityIdentity("minecraft:vindicator", HasSpawnEgg: true, Summonable: true) },
			{ EntityType.Wither, new EntityIdentity("minecraft:wither", HasSpawnEgg: true, Summonable: true) },
			{ EntityType.Dragon, new EntityIdentity("minecraft:ender_dragon", HasSpawnEgg: true, Summonable: true) },
			{ EntityType.Shulker, new EntityIdentity("minecraft:shulker", HasSpawnEgg: true, Summonable: true) },
			{ EntityType.Endermite, new EntityIdentity("minecraft:endermite", HasSpawnEgg: true, Summonable: true) },
			{ EntityType.Minecart, new EntityIdentity("minecraft:minecart", HasSpawnEgg: false, Summonable: true) },
			{ EntityType.HopperMinecart, new EntityIdentity("minecraft:hopper_minecart", HasSpawnEgg: false, Summonable: true) },
			{ EntityType.TntMinecart, new EntityIdentity("minecraft:tnt_minecart", HasSpawnEgg: false, Summonable: true) },
			{ EntityType.ChestMinecart, new EntityIdentity("minecraft:chest_minecart", HasSpawnEgg: false, Summonable: true) },
			{ EntityType.CommandBlockMinecart, new EntityIdentity("minecraft:command_block_minecart", HasSpawnEgg: false, Summonable: true) },
			{ EntityType.ArmorStand, new EntityIdentity("minecraft:armor_stand", HasSpawnEgg: false, Summonable: true) },
			{ EntityType.DroppedItem, new EntityIdentity("minecraft:item", HasSpawnEgg: false, Summonable: false) },
			{ EntityType.PrimedTnt, new EntityIdentity("minecraft:tnt", HasSpawnEgg: false, Summonable: true) },
			{ EntityType.FallingBlock, new EntityIdentity("minecraft:falling_block", HasSpawnEgg: false, Summonable: false) },
			{ EntityType.ThrownBottleoEnchanting, new EntityIdentity("minecraft:xp_bottle", HasSpawnEgg: false, Summonable: true) },
			{ EntityType.ExperienceOrb, new EntityIdentity("minecraft:xp_orb", HasSpawnEgg: false, Summonable: true) },
			{ EntityType.EnderEye, new EntityIdentity("minecraft:eye_of_ender_signal", HasSpawnEgg: false, Summonable: false) },
			{ EntityType.EnderCrystal, new EntityIdentity("minecraft:ender_crystal", HasSpawnEgg: false, Summonable: true) },
			{ EntityType.ShulkerBullet, new EntityIdentity("minecraft:shulker_bullet", HasSpawnEgg: false, Summonable: false) },
			{ EntityType.FishingRodHook, new EntityIdentity("minecraft:fishing_hook", HasSpawnEgg: false, Summonable: false) },
			{ EntityType.DragonFireball, new EntityIdentity("minecraft:dragon_fireball", HasSpawnEgg: false, Summonable: false) },
			{ EntityType.ShotArrow, new EntityIdentity("minecraft:arrow", HasSpawnEgg: false, Summonable: true) },
			{ EntityType.ThrownSnowball, new EntityIdentity("minecraft:snowball", HasSpawnEgg: false, Summonable: true) },
			{ EntityType.ThrownEgg, new EntityIdentity("minecraft:egg", HasSpawnEgg: false, Summonable: true) },
			{ EntityType.Painting, new EntityIdentity("minecraft:painting", HasSpawnEgg: false, Summonable: false) },
			{ EntityType.Trident, new EntityIdentity("minecraft:thrown_trident", HasSpawnEgg: false, Summonable: false) },
			{ EntityType.GhastFireball, new EntityIdentity("minecraft:fireball", HasSpawnEgg: false, Summonable: false) },
			{ EntityType.ThrownSpashPotion, new EntityIdentity("minecraft:splash_potion", HasSpawnEgg: false, Summonable: true) },
			{ EntityType.ThrownEnderPerl, new EntityIdentity("minecraft:ender_pearl", HasSpawnEgg: false, Summonable: false) },
			{ EntityType.LeashKnot, new EntityIdentity("minecraft:leash_knot", HasSpawnEgg: false, Summonable: true) },
			{ EntityType.WitherSkull, new EntityIdentity("minecraft:wither_skull", HasSpawnEgg: false, Summonable: false) },
			{ EntityType.WitherSkullDangerous, new EntityIdentity("minecraft:wither_skull_dangerous", HasSpawnEgg: false, Summonable: false) },
			{ EntityType.Boat, new EntityIdentity("minecraft:boat", HasSpawnEgg: false, Summonable: true) },
			{ EntityType.LightningBolt, new EntityIdentity("minecraft:lightning_bolt", HasSpawnEgg: false, Summonable: true) },
			{ EntityType.BlazeFireball, new EntityIdentity("minecraft:small_fireball", HasSpawnEgg: false, Summonable: false) },
			{ EntityType.LlamaSpit, new EntityIdentity("minecraft:llama_spit", HasSpawnEgg: false, Summonable: false) },
			{ EntityType.AreaEffectCloud, new EntityIdentity("minecraft:area_effect_cloud", HasSpawnEgg: false, Summonable: false) },
			{ EntityType.LingeringPotion, new EntityIdentity("minecraft:lingering_potion", HasSpawnEgg: false, Summonable: false) },
			{ EntityType.FireworksRocket, new EntityIdentity("minecraft:fireworks_rocket", HasSpawnEgg: false, Summonable: true) },
			{ EntityType.EvocationFangs, new EntityIdentity("minecraft:evocation_fang", HasSpawnEgg: false, Summonable: true) },
			{ EntityType.Evoker, new EntityIdentity("minecraft:evocation_illager", HasSpawnEgg: true, Summonable: true) },
			{ EntityType.Vex, new EntityIdentity("minecraft:vex", HasSpawnEgg: true, Summonable: true) },
			{ EntityType.Agent, new EntityIdentity("minecraft:agent", HasSpawnEgg: false, Summonable: false) },
			{ EntityType.IceBomb, new EntityIdentity("minecraft:ice_bomb", HasSpawnEgg: false, Summonable: false) },
			{ EntityType.Phantom, new EntityIdentity("minecraft:phantom", HasSpawnEgg: true, Summonable: true) },
			{ EntityType.Camera, new EntityIdentity("minecraft:tripod_camera", HasSpawnEgg: false, Summonable: false) },
			{ EntityType.Pillager, new EntityIdentity("minecraft:pillager", HasSpawnEgg: true, Summonable: true) },
			{ EntityType.Ravager, new EntityIdentity("minecraft:ravager", HasSpawnEgg: true, Summonable: true) },
			{ EntityType.ElderGuardianGhost, new EntityIdentity("minecraft:elder_guardian_ghost", HasSpawnEgg: false, Summonable: true) },
			{ EntityType.VillagerV2, new EntityIdentity("minecraft:villager_v2", HasSpawnEgg: true, Summonable: false) },
			{ EntityType.ZombieVillagerV2, new EntityIdentity("minecraft:zombie_villager_v2", HasSpawnEgg: true, Summonable: false) },
			{ EntityType.WanderingTrader, new EntityIdentity("minecraft:wandering_trader", HasSpawnEgg: true, Summonable: true) },
			{ EntityType.TraderLlama, new EntityIdentity("minecraft:trader_llama", HasSpawnEgg: true, Summonable: true) },
			{ EntityType.Fox, new EntityIdentity("minecraft:fox", HasSpawnEgg: true, Summonable: true) },
			{ EntityType.Bee, new EntityIdentity("minecraft:bee", HasSpawnEgg: true, Summonable: true) },
			{ EntityType.Piglin, new EntityIdentity("minecraft:piglin", HasSpawnEgg: true, Summonable: true) },
			{ EntityType.PiglinBrute, new EntityIdentity("minecraft:piglin_brute", HasSpawnEgg: true, Summonable: true) },
			{ EntityType.Hoglin, new EntityIdentity("minecraft:hoglin", HasSpawnEgg: true, Summonable: true) },
			{ EntityType.Zoglin, new EntityIdentity("minecraft:zoglin", HasSpawnEgg: true, Summonable: true) },
			{ EntityType.Strider, new EntityIdentity("minecraft:strider", HasSpawnEgg: true, Summonable: true) },
			{ EntityType.Goat, new EntityIdentity("minecraft:goat", HasSpawnEgg: true, Summonable: true) },
			{ EntityType.GlowSquid, new EntityIdentity("minecraft:glow_squid", HasSpawnEgg: true, Summonable: true) },
			{ EntityType.Axolotl, new EntityIdentity("minecraft:axolotl", HasSpawnEgg: true, Summonable: true) },
			{ EntityType.Warden, new EntityIdentity("minecraft:warden", HasSpawnEgg: true, Summonable: true) },
			{ EntityType.Frog, new EntityIdentity("minecraft:frog", HasSpawnEgg: true, Summonable: true) },
			{ EntityType.Tadpole, new EntityIdentity("minecraft:tadpole", HasSpawnEgg: true, Summonable: true) },
			{ EntityType.Allay, new EntityIdentity("minecraft:allay", HasSpawnEgg: true, Summonable: true) },
			{ EntityType.Camel, new EntityIdentity("minecraft:camel", HasSpawnEgg: true, Summonable: true) },
			{ EntityType.Sniffer, new EntityIdentity("minecraft:sniffer", HasSpawnEgg: true, Summonable: true) },
			{ EntityType.Breeze, new EntityIdentity("minecraft:breeze", HasSpawnEgg: true, Summonable: true) },
			{ EntityType.BreezeWindCharge, new EntityIdentity("minecraft:breeze_wind_charge_projectile", HasSpawnEgg: false, Summonable: false) },
			{ EntityType.WindCharge, new EntityIdentity("minecraft:wind_charge_projectile", HasSpawnEgg: false, Summonable: true) },
			{ EntityType.Armadillo, new EntityIdentity("minecraft:armadillo", HasSpawnEgg: true, Summonable: true) },
			{ EntityType.Bogged, new EntityIdentity("minecraft:bogged", HasSpawnEgg: true, Summonable: true) },
			{ EntityType.OminousItemSpawner, new EntityIdentity("minecraft:ominous_item_spawner", HasSpawnEgg: false, Summonable: false) },
			{ EntityType.Creaking, new EntityIdentity("minecraft:creaking", HasSpawnEgg: true, Summonable: true) },
			{ EntityType.HappyGhast, new EntityIdentity("minecraft:happy_ghast", HasSpawnEgg: true, Summonable: true) },
			{ EntityType.CopperGolem, new EntityIdentity("minecraft:copper_golem", HasSpawnEgg: true, Summonable: true) },
			{ EntityType.Nautilus, new EntityIdentity("minecraft:nautilus", HasSpawnEgg: true, Summonable: true) },
			{ EntityType.ZombieNautilus, new EntityIdentity("minecraft:zombie_nautilus", HasSpawnEgg: true, Summonable: true) },
			{ EntityType.Parched, new EntityIdentity("minecraft:parched", HasSpawnEgg: true, Summonable: true) },
			{ EntityType.CamelHusk, new EntityIdentity("minecraft:camel_husk", HasSpawnEgg: true, Summonable: true) },
			{ EntityType.SulfurCube, new EntityIdentity("minecraft:sulfur_cube", HasSpawnEgg: true, Summonable: true) },
			{ EntityType.Frostbite, new EntityIdentity("minecraft:frostbite", HasSpawnEgg: true, Summonable: true) },
			{ EntityType.Cushion, new EntityIdentity("minecraft:cushion", HasSpawnEgg: false, Summonable: true) },
			{ EntityType.IceBall, new EntityIdentity("minecraft:ice_ball", HasSpawnEgg: false, Summonable: true) },
			{ EntityType.ChestBoat, new EntityIdentity("minecraft:chest_boat", HasSpawnEgg: false, Summonable: true) },
		};

		public static TStore Store<TStore>(this Entity entity) where TStore : new()
		{
			return (TStore) entity.PluginStore.GetOrAdd(typeof(TStore), type => new TStore());
		}

		public static Entity CreateEntity(this short entityTypeId, Level world)
		{
			EntityType entityType = (EntityType) entityTypeId;
			return entityType.Create(world);
		}

		/// <summary>
		///     The registry as the client wants it, for AvailableEntityIdentifiers: field order
		///     bid, hasspawnegg, id, rid, summonable, matching what a real server sends.
		/// </summary>
		public static NbtList GenerateEntityIdentifiers()
		{
			var list = new NbtList("idlist");

			foreach (var q in LegacyEntityTypeIdConverter)
			{
				list.Add(new NbtCompound
				{
					new NbtString("bid", q.Value.Bid),
					new NbtByte("hasspawnegg", (byte) (q.Value.HasSpawnEgg ? 1 : 0)),
					new NbtString("id", q.Value.Id),
					new NbtInt("rid", q.Value.GetRid(q.Key)),
					new NbtByte("summonable", (byte) (q.Value.Summonable ? 1 : 0))
				});
			}

			return list;
		}

		public static string ToStringId(this EntityType type)
		{
			if(LegacyEntityTypeIdConverter.TryGetValue(type, out var value))
			{
				return value.Id;
			}

			return ":";
		}

		public static EntityType ToEntityType(string type)
		{
			return LegacyEntityTypeIdConverter.FirstOrDefault(l => l.Value.Id == type).Key;
		}

		public static Entity Create(this EntityType entityType, Level world)
		{
			Entity entity = null;

			switch (entityType)
			{
				case EntityType.None:
					return null;
				case EntityType.Chicken:
					entity = new Chicken(world);
					break;
				case EntityType.Cow:
					entity = new Cow(world);
					break;
				case EntityType.Pig:
					entity = new Pig(world);
					break;
				case EntityType.Sheep:
					entity = new Sheep(world);
					break;
				case EntityType.Wolf:
					entity = new Wolf(world);
					break;
				case EntityType.Villager:
					entity = new Villager(world);
					break;
				case EntityType.MushroomCow:
					entity = new MushroomCow(world);
					break;
				case EntityType.Squid:
					entity = new Squid(world);
					break;
				case EntityType.Rabbit:
					entity = new Rabbit(world);
					break;
				case EntityType.Bat:
					entity = new Bat(world);
					break;
				case EntityType.IronGolem:
					entity = new IronGolem(world);
					break;
				case EntityType.SnowGolem:
					entity = new SnowGolem(world);
					break;
				case EntityType.Ocelot:
					entity = new Ocelot(world);
					break;
				case EntityType.Zombie:
					entity = new Zombie(world);
					break;
				case EntityType.Creeper:
					entity = new Creeper(world);
					break;
				case EntityType.Skeleton:
					entity = new Skeleton(world);
					break;
				case EntityType.Spider:
					entity = new Spider(world);
					break;
				case EntityType.ZombiePigman:
					entity = new ZombiePigman(world);
					break;
				case EntityType.Slime:
					entity = new Slime(world);
					break;
				case EntityType.Enderman:
					entity = new Enderman(world);
					break;
				case EntityType.Silverfish:
					entity = new Silverfish(world);
					break;
				case EntityType.CaveSpider:
					entity = new CaveSpider(world);
					break;
				case EntityType.Ghast:
					entity = new Ghast(world);
					break;
				case EntityType.MagmaCube:
					entity = new MagmaCube(world);
					break;
				case EntityType.Blaze:
					entity = new Blaze(world);
					break;
				case EntityType.ZombieVillager:
					entity = new ZombieVillager(world);
					break;
				case EntityType.Witch:
					entity = new Witch(world);
					break;
				case EntityType.Stray:
					entity = new Stray(world);
					break;
				case EntityType.Husk:
					entity = new Husk(world);
					break;
				case EntityType.WitherSkeleton:
					entity = new WitherSkeleton(world);
					break;
				case EntityType.Guardian:
					entity = new Guardian(world);
					break;
				case EntityType.ElderGuardian:
					entity = new ElderGuardian(world);
					break;
				case EntityType.Horse:
					var random = new Random();
					entity = new Horse(world, random.NextDouble() < 0.10, random);
					break;
				case EntityType.PolarBear:
					entity = new PolarBear(world);
					break;
				case EntityType.Shulker:
					entity = new Shulker(world);
					break;
				case EntityType.Dragon:
					entity = new Dragon(world);
					break;
				case EntityType.SkeletonHorse:
					entity = new SkeletonHorse(world);
					break;
				case EntityType.Wither:
					entity = new Wither(world);
					break;
				case EntityType.Evoker:
					entity = new Evoker(world);
					break;
				case EntityType.Vindicator:
					entity = new Vindicator(world);
					break;
				case EntityType.Vex:
					entity = new Vex(world);
					break;
				case EntityType.Ravager:
					entity = new Ravager(world);
					break;
				case EntityType.ElderGuardianGhost:
					entity = new ElderGuardianGhost(world);
					break;
				case EntityType.Piglin:
					entity = new Piglin(world);
					break;
				case EntityType.PiglinBrute:
					entity = new PiglinBrute(world);
					break;
				case EntityType.Hoglin:
					entity = new Hoglin(world);
					break;
				case EntityType.Zoglin:
					entity = new Zoglin(world);
					break;
				case EntityType.Warden:
					entity = new Warden(world);
					break;
				case EntityType.Breeze:
					entity = new Breeze(world);
					break;
				case EntityType.Bogged:
					entity = new Bogged(world);
					break;
				case EntityType.Creaking:
					entity = new Creaking(world);
					break;
				case EntityType.ZombieVillagerV2:
					entity = new ZombieVillagerV2(world);
					break;
				case EntityType.ZombieNautilus:
					entity = new ZombieNautilus(world);
					break;
				case EntityType.Parched:
					entity = new Parched(world);
					break;
				case EntityType.CamelHusk:
					entity = new CamelHusk(world);
					break;
				case EntityType.SulfurCube:
					entity = new SulfurCube(world);
					break;
				case EntityType.VillagerV2:
					entity = new VillagerV2(world);
					break;
				case EntityType.WanderingTrader:
					entity = new WanderingTrader(world);
					break;
				case EntityType.TraderLlama:
					entity = new TraderLlama(world);
					break;
				case EntityType.Fox:
					entity = new Fox(world);
					break;
				case EntityType.Bee:
					entity = new Bee(world);
					break;
				case EntityType.Strider:
					entity = new Strider(world);
					break;
				case EntityType.Goat:
					entity = new Goat(world);
					break;
				case EntityType.GlowSquid:
					entity = new GlowSquid(world);
					break;
				case EntityType.Axolotl:
					entity = new Axolotl(world);
					break;
				case EntityType.Frog:
					entity = new Frog(world);
					break;
				case EntityType.Tadpole:
					entity = new Tadpole(world);
					break;
				case EntityType.Allay:
					entity = new Allay(world);
					break;
				case EntityType.Camel:
					entity = new Camel(world);
					break;
				case EntityType.Sniffer:
					entity = new Sniffer(world);
					break;
				case EntityType.Armadillo:
					entity = new Armadillo(world);
					break;
				case EntityType.HappyGhast:
					entity = new HappyGhast(world);
					break;
				case EntityType.CopperGolem:
					entity = new CopperGolem(world);
					break;
				case EntityType.Nautilus:
					entity = new Nautilus(world);
					break;
				case EntityType.Npc:
					entity = new PlayerMob("test", world);
					break;
				default:
					return null;
			}

			return entity;
		}
	}
}