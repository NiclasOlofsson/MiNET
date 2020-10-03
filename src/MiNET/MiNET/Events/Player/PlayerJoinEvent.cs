﻿namespace MiNET.Events.Player
{
	/// <summary>
	/// 	Dispatched when an <see cref="OpenPlayer"/> joins the server
	/// </summary>
	public class PlayerJoinEvent : PlayerEvent
	{
		public PlayerJoinEvent(MiNET.Player player) : base(player)
		{
		}
	}
}
