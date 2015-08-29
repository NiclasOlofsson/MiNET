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

		public void SendAdd(Player player)
		{
			{
				var message = McpeMobEffect.CreateObject();
				message.entityId = 0;
				message.eventId = 1;
				message.effectId = (byte) EffectId;
				message.duration = 20*Duration;
				message.amplifier = (byte) Level;
				message.particles = (byte) (Particles ? 1 : 0);
				player.SendPackage(message, true);
			}

			//{
			//	var message = McpeMobEffect.CreateObject();
			//	message.entityId = player.EntityId;
			//	message.eventId = 1;
			//	message.effectId = (byte) EffectId;
			//	message.duration = 20*Duration;
			//	message.amplifier = (byte) Level;
			//	message.particles = (byte) (Particles ? 1 : 0);
			//	player.Level.RelayBroadcast(player, message);
			//}
		}

		public void SendUpdate(Player player)
		{
			{
				var message = McpeMobEffect.CreateObject();
				message.entityId = 0;
				message.eventId = 2;
				message.effectId = (byte) EffectId;
				message.duration = 20*Duration;
				message.amplifier = (byte) Level;
				message.particles = (byte) (Particles ? 1 : 0);
				player.SendPackage(message, true);
			}

			{
				var message = McpeMobEffect.CreateObject();
				message.entityId = player.EntityId;
				message.eventId = 2;
				message.effectId = (byte) EffectId;
				message.duration = 20*Duration;
				message.amplifier = (byte) Level;
				message.particles = (byte) (Particles ? 1 : 0);
				player.Level.RelayBroadcast(player, message, true);
			}
		}

		public void SendRemove(Player player)
		{
			{
				var message = McpeMobEffect.CreateObject();
				message.entityId = 0;
				message.eventId = 3;
				message.effectId = (byte) EffectId;
				player.SendPackage(message, true);
			}

			{
				var message = McpeMobEffect.CreateObject();
				message.entityId = player.EntityId;
				message.eventId = 3;
				message.effectId = (byte) EffectId;
				player.Level.RelayBroadcast(player, message, true);
			}
		}
	}

	public class Speed : Effect
	{
		public Speed() : base(EffectType.Speed)
		{
		}
	}

	public class Slowness : Effect
	{
		public Slowness() : base(EffectType.Slowness)
		{
		}
	}

	public class Haste : Effect
	{
		public Haste() : base(EffectType.Haste)
		{
		}
	}


	public class MiningFatigue : Effect
	{
		public MiningFatigue() : base(EffectType.MiningFatigue)
		{
		}
	}

	public class Strength : Effect
	{
		public Strength() : base(EffectType.Strenght)
		{
		}
	}

	public class InstandHealth : Effect
	{
		public InstandHealth() : base(EffectType.InstandHealth)
		{
		}
	}

	public class InstantDamage : Effect
	{
		public InstantDamage() : base(EffectType.InstantDamage)
		{
		}
	}

	public class JumpBoost : Effect
	{
		public JumpBoost() : base(EffectType.JumpBoost)
		{
		}
	}

	public class Nausea : Effect
	{
		public Nausea() : base(EffectType.Nausea)
		{
		}
	}


	public class Regeneration : Effect
	{
		public Regeneration() : base(EffectType.Regeneration)
		{
		}
	}

	public class Resistance : Effect
	{
		public Resistance() : base(EffectType.Resistance)
		{
		}
	}

	public class FireResistance : Effect
	{
		public FireResistance() : base(EffectType.FireResistance)
		{
		}
	}

	public class WaterBreathing : Effect
	{
		public WaterBreathing() : base(EffectType.WaterBreathing)
		{
		}
	}

	public class Invisibility : Effect
	{
		public Invisibility() : base(EffectType.Invisibility)
		{
		}
	}

	public class Blindness : Effect
	{
		public Blindness() : base(EffectType.Blindness)
		{
		}
	}

	public class NightVision : Effect
	{
		public NightVision() : base(EffectType.NightVision)
		{
		}
	}

	public class Hunger : Effect
	{
		public Hunger() : base(EffectType.Hunger)
		{
		}
	}

	public class Weakness : Effect
	{
		public Weakness() : base(EffectType.Weakness)
		{
		}
	}

	public class Poison : Effect
	{
		public Poison() : base(EffectType.Poison)
		{
		}
	}

	public class Wither : Effect
	{
		public Wither() : base(EffectType.Wither)
		{
		}
	}

	public class HealthBoost : Effect
	{
		public HealthBoost() : base(EffectType.HealthBoost)
		{
		}
	}

	public class Absorption : Effect
	{
		public Absorption() : base(EffectType.Absorption)
		{
		}
	}

	public class Saturation : Effect
	{
		public Saturation() : base(EffectType.Saturation)
		{
		}
	}

	public class Glowing : Effect
	{
		public Glowing() : base(EffectType.Glowing)
		{
		}
	}

	public class Levitation : Effect
	{
		public Levitation() : base(EffectType.Levitation)
		{
		}
	}
}