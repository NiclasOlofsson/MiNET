﻿namespace MiNET.Events.Player
{
	/// <summary>
	/// 	Dispatched when an <see cref="OpenPlayer"/> leaves the server
	/// </summary>
	public class PlayerQuitEvent : PlayerEvent
	{
		public PlayerQuitEvent(MiNET.Player.Player player) : base(player)
		{
		}
	}
}
