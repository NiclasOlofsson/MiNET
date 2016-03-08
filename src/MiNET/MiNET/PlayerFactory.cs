using System;
using System.Net;

namespace MiNET
{
	public class PlayerFactory
	{
		public virtual Player CreatePlayer(MiNetServer server, IPEndPoint endPoint)
		{
			var player = new Player(server, endPoint);
			OnPlayerCreated(new PlayerEventArgs(player));
			return player;
		}

		public event EventHandler<PlayerEventArgs> PlayerCreated;

		protected virtual void OnPlayerCreated(PlayerEventArgs e)
		{
			PlayerCreated?.Invoke(this, e);
		}
	}
}