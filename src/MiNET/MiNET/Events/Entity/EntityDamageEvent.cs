﻿using MiNET.Entities;

namespace MiNET.Events.Entity
{
	/// <summary>
	/// 	Dispatched when an Entity gets damaged
	/// </summary>
	public class EntityDamageEvent : EntityEvent
	{
		public DamageCause Cause { get; }
		public int PreviousHealth { get; }
		public int NewHealth { get; }
		public MiNET.Entities.Entity Attacker { get; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="entity">The entity that has been damaged</param>
		/// <param name="source">The source of the of the damage</param>
		/// <param name="cause">The cause of the damage</param>
		/// <param name="previousHealth">The entity's previous health value</param>
		/// <param name="newHealth">The entity's new health value</param>
		public EntityDamageEvent(MiNET.Entities.Entity entity, MiNET.Entities.Entity source, DamageCause cause, int previousHealth, int newHealth) : base(entity)
		{
			Cause = cause;
			PreviousHealth = previousHealth;
			NewHealth = newHealth;
			Attacker = source;
		}
	}
}
