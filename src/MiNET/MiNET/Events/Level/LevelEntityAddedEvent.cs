﻿namespace MiNET.Events.Level
{
	/// <summary>
	/// 	Dispatched when an Entity gets added to a <see cref="OpenLevel"/>
	/// </summary>
	public class LevelEntityAddedEvent : LevelEvent
	{
		/// <summary>
		/// 	The entity that got added
		/// </summary>
		public MiNET.Entities.Entity Entity { get; }
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="world">The level the entity was added to</param>
		/// <param name="entity">The entity that has been added</param>
		public LevelEntityAddedEvent(Worlds.Level world, MiNET.Entities.Entity entity) : base(world)
		{
			Entity = entity;
		}
	}
}
