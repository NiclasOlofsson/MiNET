﻿namespace MiNET.Events.Player
{
	/// <summary>
	/// 	Dispatched when an <see cref="OpenPlayer"/> spawns in a world
	/// </summary>
	public class PlayerSpawnedEvent : PlayerEvent
	{
		public PlayerSpawnedEvent(MiNET.Player player) : base(player)
		{
		}
	}
}
