﻿namespace MiNET.Events.Entity
{
	/// <summary>
	/// 	Dispatched when an entity got killed.
	/// </summary>
	public class EntityKilledEvent : EntityEvent
	{
		/// <summary>
		/// 	
		/// </summary>
		/// <param name="entity">The entity that got killed.</param>
		public EntityKilledEvent(MiNET.Entities.Entity entity) : base(entity)
		{

		}
	}
}
