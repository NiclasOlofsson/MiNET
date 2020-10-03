﻿namespace MiNET.Events.Entity
{
	/// <summary>
	/// 	Dispatched when an entity regains health
	/// </summary>
	public class EntityRegainHealthEvent : EntityEvent
	{
		public int PreviousHealth { get; }
		public int NewHealth { get; }
		
		/// <summary>
		/// 	
		/// </summary>
		/// <param name="entity">The entity who's health was regained</param>
		/// <param name="previousHealth">The entity's previous health</param>
		/// <param name="newHealth">The entity's new health</param>
		public EntityRegainHealthEvent(MiNET.Entities.Entity entity, int previousHealth, int newHealth) : base(entity)
		{
			PreviousHealth = previousHealth;
			NewHealth = newHealth;
		}
	}
}
