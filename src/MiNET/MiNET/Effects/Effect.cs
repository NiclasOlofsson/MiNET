using MiNET.Net;
using MiNET.Worlds;

namespace MiNET.Effects
{
	public enum EffectType : byte
	{
		None = 0,
		Speed = 1,
		Slowness = 2,
		Haste = 3,
		MiningFatigue = 4,
		Strength = 5,
		InstantHealth = 6,
		InstantDamage = 7,
		JumpBoost = 8,
		Nausea = 9,
		Regeneration = 10,
		Resistance = 11,
		FireResistance = 12,
		WaterBreathing = 13,
		Invisibility = 14,
		Blindness = 15,
		NightVision = 16,
		Hunger = 17,
		Weakness = 18,
		Poison = 19,
		Wither = 20,
		HealthBoost = 21,
		Absorption = 22,
		Saturation = 23,
	}

	public class Effect
	{
		public const int MaxDuration = 0x7fffffff;

		public EffectType EffectId { get; set; }
		public int Duration { get; set; }
		public int Level { get; set; }
		public bool Particles { get; set; }

		protected Effect(EffectType id)
		{
			EffectId = id;
			Particles = true;
		}

		public virtual void SendAdd(Player player)
		{
			var message = McpeMobEffect.CreateObject();
			message.runtimeEntityId = EntityManager.EntityIdSelf;
			message.eventId = 1;
			message.effectId = (int) EffectId;
			message.duration = Duration;
			message.amplifier = Level;
			message.particles = Particles;
			player.SendPackage(message);

			player.BroadcastSetEntityData();
		}

		public virtual void SendUpdate(Player player)
		{
			var message = McpeMobEffect.CreateObject();
			message.runtimeEntityId = EntityManager.EntityIdSelf;
			message.eventId = 2;
			message.effectId = (int) EffectId;
			message.duration = Duration;
			message.amplifier = Level;
			message.particles = Particles;
			player.SendPackage(message);
		}

		public virtual void SendRemove(Player player)
		{
			var message = McpeMobEffect.CreateObject();
			message.runtimeEntityId = EntityManager.EntityIdSelf;
			message.eventId = 3;
			message.effectId = (int) EffectId;
			player.SendPackage(message);
		}

		public virtual void OnTick(Player player)
		{
			if (Duration > 0 && Duration != MaxDuration) Duration -= 1;
			if(Duration < 20) player.RemoveEffect(this); // Need 20 tick grace for some effects that fade
		}

		public override string ToString()
		{
			return $"EffectId: {EffectId}, Duration: {Duration}, Level: {Level}, Particles: {Particles}";
		}
	}
}