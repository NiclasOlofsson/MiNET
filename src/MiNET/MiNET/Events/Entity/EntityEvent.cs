﻿namespace MiNET.Events.Entity
{
	/// <summary>
	/// 	The base class for any Entity related events
	/// </summary>
	public class EntityEvent : Event
	{
		public MiNET.Entities.Entity Entity { get; }
		public EntityEvent(MiNET.Entities.Entity entity)
		{
			Entity = entity;
		}
	}
}
