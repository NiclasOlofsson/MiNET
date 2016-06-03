using System.Numerics;
using MiNET.Net;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Particles
{
	public enum ParticleType
	{
		Bubble = 1,
		Critical = 2,
		Smoke = 3,
		Explode = 4,
		WhiteSmoke = 5,
		Flame = 6,
		Lava = 7,
		LargeSmoke = 8,
		Redstone = 9,
		ItemBreak = 10,
		SnowballPoof = 11,
		LargeExplode = 12,
		HugeExplode = 13,
		MobFlame = 14,
		Heart = 15,
		Terrain = 16,
		TownAura = 17,
		Portal = 18,
		WaterSplash = 19,
		WaterWake = 20,
		DripWater = 21,
		DripLava = 22,
		Dust = 23,
		MobSpell = 24,
		MobSpellAmbient = 25,
		MobSpellInstantaneous = 26,
		Ink = 27,
		Slime = 28,
		RainSplash = 29,
		VillagerAngry = 30,
		VillagerHappy = 31,
		EnchantmentTable = 32,
	}

	public class Particle
	{
		public int Id { get; private set; }
		protected Level Level { get; set; }
		public Vector3 Position { get; set; }
		protected int Data { get; set; }

		protected Particle(ParticleType particleType, Level level) : this((int) particleType, level)
		{
		}

		protected Particle(int id, Level level)
		{
			Id = id;
			Level = level;
		}

		public virtual void Spawn()
		{
			McpeLevelEvent particleEvent = McpeLevelEvent.CreateObject();
			particleEvent.eventId = (short) (0x4000 | Id);
			particleEvent.x = (float) Position.X;
			particleEvent.y = (float) Position.Y;
			particleEvent.z = (float) Position.Z;
			particleEvent.data = Data;
			Level.RelayBroadcast(particleEvent);
		}
	}
}