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

using MiNET.Net;
using MiNET.Worlds;

namespace MiNET.Particles
{
	public enum ParticleType
	{
		Bubble = 1,
		BubbleManual = 2,
		Critical = 3,
		BlockForceField = 4,
		Smoke = 5,
		Explode = 6,
		Evaporation = 7,
		Flame = 8,
		CandleFlame = 9,
		Lava = 10,
		LargeSmoke = 11,
		Redstone = 12,
		RisingRedDust = 13,
		ItemBreak = 14,
		SnowballPoof = 15,
		HugeExplode = 16,
		HugeExplodeSeed = 17,
		MobFlame = 18,
		Heart = 19,
		Terrain = 20,
		SuspendedTown = 21, 
		TownAura = 21,
		Portal = 22,
		//23 same as 22
		Splash = 24, 
		WaterSplash = 24,
		WaterSplashManual = 25,
		WaterWake = 26,
		DripWater = 27,
		DripLava = 28,
		DripHoney = 29,
		StalactiteDripWater = 30,
		StalactiteDripLava = 31,
		FallingDust = 32, Dust = 32,
		MobSpell = 33,
		MobSpellAmbient = 34,
		MobSpellInstantaneous = 35,
		Ink = 36,
		Slime = 37,
		RainSplash = 38,
		VillagerAngry = 39,
		VillagerHappy = 40,
		EnchantmentTable = 41,
		TrackingEmitter = 42,
		Note = 43,
		WitchSpell = 44,
		Carrot = 45,
		MobAppearance = 46,
		EndRod = 47,
		DragonsBreath = 48,
		Spit = 49,
		Totem = 50,
		Food = 51,
		FireworksStarter = 52,
		FireworksSpark = 53,
		FireworksOverlay = 54,
		BalloonGas = 55,
		ColoredFlame = 56,
		Sparkler = 57,
		Conduit = 58,
		BubbleColumnUp = 59,
		BubbleColumnDown = 60,
		Sneeze = 61,
		ShulkerBullet = 62,
		Bleach = 63,
		DragonDestroyBlock = 64,
		MyceliumDust = 65,
		FallingRedDust = 66,
		CampfireSmoke = 67,
		TallCampfireSmoke = 68,
		DragonBreathFire = 69,
		DragonBreathTrail = 70,
		BlueFlame = 71,
		Soul = 72,
		ObsidianTear = 73,
		PortalReverse = 74,
		Snowflake = 75,
		VibrationSignal = 76,
		SculkSensorRedstone = 77,
		SporeBlossomShower = 78,
		SporeBlossomAmbient = 79,
		Wax = 80,
		Electric_spark = 81,
}

	public class LegacyParticle : Particle
	{
		public int Id { get; private set; }
		protected int Data { get; set; }

		public LegacyParticle(ParticleType particle, Level level): this((int)particle, level)
		{
		}

		public LegacyParticle(int id, Level level) : base(level)
		{
			Id = id;
			Level = level;
		}

		public override void Spawn(Player[] players)
		{
			var particleEvent = McpeLevelEvent.CreateObject();
			particleEvent.eventId = (short) (0x4000 | Id);
			particleEvent.position = Position;
			particleEvent.data = Data;
			Level.RelayBroadcast(players, particleEvent);
		}
	}
}