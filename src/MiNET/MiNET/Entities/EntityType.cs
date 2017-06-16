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
// The Original Code is Niclas Olofsson.
// 
// The Original Developer is the Initial Developer.  The Initial Developer of
// the Original Code is Niclas Olofsson.
// 
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2017 Niclas Olofsson. 
// All Rights Reserved.

#endregion

namespace MiNET.Entities
{
	public enum EntityType
	{
		None = 0,

		DroppedItem = 64,
		ExperienceOrb = 69,

		PrimedTnt = 65,
		FallingBlock = 66,

		ThrownBottleoEnchanting = 68,
		EnderEye = 70,
		EnderCrystal = 71,
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
		Evoker = 104,
		Vex = 105,

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

		Player = 63,

		Npc = 51,
		Agent = 56,
		Camera = 62,
		Chalkboard = 78
	}
}