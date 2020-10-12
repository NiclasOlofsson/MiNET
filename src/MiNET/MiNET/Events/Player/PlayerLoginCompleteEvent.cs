using System;

namespace MiNET.Events.Player
{
	/// <summary>
	/// 	Dispatched when an <see cref="OpenPlayer"/> completes the login cycle
	/// </summary>
	public class PlayerLoginCompleteEvent : PlayerEvent
	{
		/// <summary>
		/// 	The time the player completed the login
		/// </summary>
		public DateTime CompletionTime { get; }
		public PlayerLoginCompleteEvent(MiNET.Player.Player player, DateTime loginCompleteTime) : base(player)
		{
			CompletionTime = loginCompleteTime;
		}
	}
}
