namespace MiNET.Events.Player
{
	/// <summary>
	/// 	The base class for any <see cref="OpenPlayer"/> event
	/// </summary>
	public class PlayerEvent : Event
	{
		/// <summary>
		/// 	The player that the event occured for.
		/// </summary>
		public MiNET.Player.Player Player { get; }
		
		
		public PlayerEvent(MiNET.Player.Player player)
		{
			Player = player;
		}
	}
}
