using MiNET.Net;

namespace MiNET.Effects
{
	public enum EffectType : byte
	{
		None = 0,
		Speed,
		Slowness,
		Haste,
		MiningFatigue,
		Strenght,
		InstandHealth,
		InstantDamage,
		JumpBoost,
		Nausea,
		Regeneration,
		Resistance,
		FireResistance,
		WaterBreathing,
		Invisibility,
		Blindness,
		NightVision,
		Hunger,
		Weakness,
		Poison,
		Wither,
		HealthBoost,
		Absorption,
		Saturation,
		Glowing,
		Levitation
	}

	public class Effect
	{
		public const int MaxDuration = 0x7fffffff/20;

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
			message.entityId = 0;
			message.eventId = 1;
			message.effectId = (byte) EffectId;
			message.duration = 20*Duration;
			message.amplifier = (byte) Level;
			message.particles = (byte) (Particles ? 1 : 0);
			player.SendPackage(message);
		}

		public virtual void SendUpdate(Player player)
		{
			var message = McpeMobEffect.CreateObject();
			message.entityId = 0;
			message.eventId = 2;
			message.effectId = (byte) EffectId;
			message.duration = 20*Duration;
			message.amplifier = (byte) Level;
			message.particles = (byte) (Particles ? 1 : 0);
			player.SendPackage(message);
		}

		public virtual void SendRemove(Player player)
		{
			var message = McpeMobEffect.CreateObject();
			message.entityId = 0;
			message.eventId = 3;
			message.effectId = (byte) EffectId;
			player.SendPackage(message);
		}
	}
}