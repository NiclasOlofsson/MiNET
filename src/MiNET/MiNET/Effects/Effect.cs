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

using System.Drawing;
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
		Levitation = 24,

		/// <summary>Bedrock only: poison that does kill.</summary>
		FatalPoison = 25,

		ConduitPower = 26,
		SlowFalling = 27,
		BadOmen = 28,

		/// <summary>Java calls this Hero of the Village; Bedrock's own name for it is village_hero.</summary>
		VillageHero = 29,

		Darkness = 30,
		TrialOmen = 31,
		RaidOmen = 32,
		WindCharged = 33,
		Weaving = 34,
		Oozing = 35,
		Infested = 36,
		BreathOfTheNautilus = 37,
		Freezing = 38,
	}

	public class Effect
	{
		/// <summary>
		///     An effect that never expires. The client renders this as an infinity symbol; any
		///     other value it counts down itself from whatever we send.
		/// </summary>
		public const int InfiniteDuration = -1;

		/// <summary>Kept for callers; prefer <see cref="InfiniteDuration" />, which is what it means.</summary>
		public const int MaxDuration = InfiniteDuration;

		public EffectType EffectId { get; set; }
		public int Duration { get; set; }
		public int Level { get; set; }
		public bool Particles { get; set; }
		public Color ParticleColor { get; set; } = Color.Black;

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
			message.tick = 0;
			player.SendPacket(message);

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
			message.tick = 0;
			player.SendPacket(message);
		}

		public virtual void SendRemove(Player player)
		{
			var message = McpeMobEffect.CreateObject();
			message.runtimeEntityId = EntityManager.EntityIdSelf;
			message.eventId = 3;
			message.effectId = (int) EffectId;
			message.tick = 0;
			player.SendPacket(message);
		}

		public bool IsInfinite => Duration == InfiniteDuration;

		public virtual void OnTick(Player player)
		{
			if (IsInfinite) return;

			if (Duration > 0) Duration -= 1;
			if (Duration < 20) player.RemoveEffect(this); // Need 20 tick grace for some effects that fade
		}

		public override string ToString()
		{
			return $"EffectId: {EffectId}, Duration: {Duration}, Level: {Level}, Particles: {Particles}";
		}
	}
}