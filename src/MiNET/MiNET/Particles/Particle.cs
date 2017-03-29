using System.Numerics;
using MiNET.Net;
using MiNET.Worlds;

namespace MiNET.Particles
{
	public enum ParticleType
	{
		Bubble = 1,
		Critical,
		BlockForceField,
		Smoke,
		Explode,
		WhiteSmoke,
		Flame,
		Lava,
		LargeSmoke,
		Redstone,
		RisingRedDust,
		ItemBreak,
		SnowballPoof,
		LargeExplode,
		HugeExplode,
		MobFlame,
		Heart,
		Terrain,
		TownAura,
		Portal,
		WaterSplash,
		WaterWake,
		DripWater,
		DripLava,
		Dust,
		MobSpell,
		MobSpellAmbient,
		MobSpellInstantaneous,
		Ink,
		Slime,
		RainSplash,
		VillagerAngry,
		VillagerHappy,
		EnchantmentTable,
		TrackingEmitter,
		Note,
		WitchSpell,
		Carrot,
		Unknown39,
		EndRod,
		DragonsBreath
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

		public Particle(int id, Level level)
		{
			Id = id;
			Level = level;
		}

		public virtual void Spawn()
		{
			McpeLevelEvent particleEvent = McpeLevelEvent.CreateObject();
			particleEvent.eventId = (short) (0x4000 | Id);
			particleEvent.position = Position;
			particleEvent.data = Data;
			Level.RelayBroadcast(particleEvent);
		}

		public virtual void Spawn(Player[] players)
		{
			McpeLevelEvent particleEvent = McpeLevelEvent.CreateObject();
			particleEvent.eventId = (short) (0x4000 | Id);
			particleEvent.position = Position;
			particleEvent.data = Data;
			Level.RelayBroadcast(players, particleEvent);
		}
	}
}