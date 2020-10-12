﻿namespace MiNET.Events.Player
{
	/// <summary>
	/// 	Dispatched whenever an <see cref="OpenPlayer"/> despawns
	/// </summary>
	public class PlayerDespawnedEvent : PlayerEvent
	{
		public PlayerDespawnedEvent(MiNET.Player.Player player) : base(player)
		{
		}
	}
}
