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

		Player = 63,

		Npc = 51,
		Agent = 56,
		Camera = 62,
		Chalkboard = 78,

		Herobrine = 666
	}

	public static class EntityHelpers
	{
		public static TStore Store<TStore>(this Entity entity) where TStore : new()
		{
			return (TStore) entity.PluginStore.GetOrAdd(typeof(TStore), type => new TStore());
		}

		public static Entity CreateEntity(this short entityTypeId, Level world)
		{
			EntityType entityType = (EntityType) entityTypeId;
			return entityType.Create(world);
		}

		public static string ToStringId(this EntityType type)
		{
			switch (type)
			{
				case EntityType.Npc:
					return "minecraft:npc";
				case EntityType.Player:
					return "minecraft:player";
				case EntityType.WitherSkeleton:
					return "minecraft:wither_skeleton";
				case EntityType.Husk:
					return "minecraft:husk";
				case EntityType.Stray:
					return "minecraft:stray";
				case EntityType.Witch:
					return "minecraft:witch";
				case EntityType.ZombieVillager:
					return "minecraft:zombie_villager";
				case EntityType.Blaze:
					return "minecraft:blaze";
				case EntityType.MagmaCube:
					return "minecraft:magma_cube";
				case EntityType.Ghast:
					return "minecraft:ghast";
				case EntityType.CaveSpider:
					return "minecraft:cave_spider";
				case EntityType.Silverfish:
					return "minecraft:silverfish";
				case EntityType.Enderman:
					return "minecraft:enderman";
				case EntityType.Slime:
					return "minecraft:slime";
				case EntityType.ZombiePigman:
					return "minecraft:zombie_pigman";
				case EntityType.Spider:
					return "minecraft:spider";
				case EntityType.Skeleton:
					return "minecraft:skeleton";
				case EntityType.Creeper:
					return "minecraft:creeper";
				case EntityType.Zombie:
					return "minecraft:zombie";
				case EntityType.SkeletonHorse:
					return "minecraft:skeleton_horse";
				case EntityType.Mule:
					return "minecraft:mule";
				case EntityType.Donkey:
					return "minecraft:donkey";
				case EntityType.Dolphin:
					return "minecraft:dolphin";
				case EntityType.TropicalFish:
					return "minecraft:tropicalfish";
				case EntityType.Wolf:
					return "minecraft:wolf";
				case EntityType.Squid:
					return "minecraft:squid";
				case EntityType.Drowned:
					return "minecraft:drowned";
				case EntityType.Sheep:
					return "minecraft:sheep";
				case EntityType.MushroomCow:
					return "minecraft:mooshroom";
				case EntityType.Panda:
					return "minecraft:panda";
				case EntityType.Salmon:
					return "minecraft:salmon";
				case EntityType.Pig:
					return "minecraft:pig";
				case EntityType.Villager:
					return "minecraft:villager";
				case EntityType.Fish:
					return "minecraft:cod";
				case EntityType.Pufferfish:
					return "minecraft:pufferfish";
				case EntityType.Cow:
					return "minecraft:cow";
				case EntityType.Chicken:
					return "minecraft:chicken";
				case EntityType.Balloon:
					return "minecraft:balloon";
				case EntityType.Llama:
					return "minecraft:llama";
				case EntityType.IronGolem:
					return "minecraft:iron_golem";
				case EntityType.Rabbit:
					return "minecraft:rabbit";
				case EntityType.SnowGolem:
					return "minecraft:snow_golem";
				case EntityType.Bat:
					return "minecraft:bat";
				case EntityType.Ocelot:
					return "minecraft:ocelot";
				case EntityType.Horse:
					return "minecraft:horse";
				case EntityType.Cat:
					return "minecraft:cat";
				case EntityType.PolarBear:
					return "minecraft:polar_bear";
				case EntityType.ZombieHorse:
					return "minecraft:zombie_horse";
				case EntityType.Turtle:
					return "minecraft:turtle";
				case EntityType.Parrot:
					return "minecraft:parrot";
				case EntityType.Guardian:
					return "minecraft:guardian";
				case EntityType.ElderGuardian:
					return "minecraft:elder_guardian";
				case EntityType.Vindicator:
					return "minecraft:vindicator";
				case EntityType.Wither:
					return "minecraft:wither";
				case EntityType.Dragon:
					return "minecraft:ender_dragon";
				case EntityType.Shulker:
					return "minecraft:shulker";
				case EntityType.Endermite:
					return "minecraft:endermite";
				case EntityType.Minecart:
					return "minecraft:minecart";
				case EntityType.HopperMinecart:
					return "minecraft:hopper_minecart";
				case EntityType.TntMinecart:
					return "minecraft:tnt_minecart";
				case EntityType.ChestMinecart:
					return "minecraft:chest_minecart";
				case EntityType.CommandBlockMinecart:
					return "minecraft:command_block_minecart";
				case EntityType.ArmorStand:
					return "minecraft:armor_stand";
				case EntityType.DroppedItem:
					return "minecraft:item";
				case EntityType.PrimedTnt:
					return "minecraft:tnt";
				case EntityType.FallingBlock:
					return "minecraft:falling_block";
				case EntityType.ThrownBottleoEnchanting:
					return "minecraft:xp_bottle";
				case EntityType.ExperienceOrb:
					return "minecraft:xp_orb";
				case EntityType.EnderEye:
					return "minecraft:eye_of_ender_signal";
				case EntityType.EnderCrystal:
					return "minecraft:ender_crystal";
				case EntityType.ShulkerBullet:
					return "minecraft:shulker_bullet";
				case EntityType.FishingRodHook:
					return "minecraft:fishing_hook";
				case EntityType.DragonFireball:
					return "minecraft:dragon_fireball";
				case EntityType.ShotArrow:
					return "minecraft:arrow";
				case EntityType.ThrownSnowball:
					return "minecraft:snowball";
				case EntityType.ThrownEgg:
					return "minecraft:egg";
				case EntityType.Painting:
					return "minecraft:painting";
				case EntityType.Trident:
					return "minecraft:thrown_trident";
				case EntityType.GhastFireball:
					return "minecraft:fireball";
				case EntityType.ThrownSpashPotion:
					return "minecraft:splash_potion";
				case EntityType.ThrownEnderPerl:
					return "minecraft:ender_pearl";
				case EntityType.LeashKnot:
					return "minecraft:leash_knot";
				case EntityType.WitherSkull:
					return "minecraft:wither_skull";
				case EntityType.WitherSkullDangerous:
					return "minecraft:wither_skull_dangerous";
				case EntityType.Boat:
					return "minecraft:boat";
				case EntityType.LightningBolt:
					return "minecraft:lightning_bolt";
				case EntityType.BlazeFireball:
					return "minecraft:small_fireball";
				case EntityType.LlamaSpit:
					return "minecraft:llama_spit";
				case EntityType.AreaEffectCloud:
					return "minecraft:area_effect_cloud";
				case EntityType.LingeringPotion:
					return "minecraft:lingering_potion";
				case EntityType.FireworksRocket:
					return "minecraft:fireworks_rocket";
				case EntityType.EvocationFangs:
					return "minecraft:evocation_fang";
				case EntityType.Evoker:
					return "minecraft:evocation_illager";
				case EntityType.Vex:
					return "minecraft:vex";
				case EntityType.Agent:
					return "minecraft:agent";
				case EntityType.IceBomb:
					return "minecraft:ice_bomb";
				case EntityType.Phantom:
					return "minecraft:phantom";
				case EntityType.Camera:
					return "minecraft:tripod_camera";
				default:
					return ":";
			}
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