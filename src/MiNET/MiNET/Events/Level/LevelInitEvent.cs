﻿namespace MiNET.Events.Level
{
	/// <summary>
	/// 	Dispatched when an <see cref="OpenLevel"/> gets initiated.
	/// </summary>
	public class LevelInitEvent : LevelEvent
	{
		/// <summary>
		/// 	
		/// </summary>
		/// <param name="world">The level that got initiated</param>
		public LevelInitEvent(Worlds.Level world) : base(world)
		{ 
		}
	}
}
