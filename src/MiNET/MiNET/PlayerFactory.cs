using System;
using System.Net;
using MiNET.Utils;

namespace MiNET
{
	public class PlayerFactory
	{
		public virtual Player CreatePlayer(MiNetServer server, IPEndPoint endPoint)
		{
			var player = new Player(server, endPoint);
			player.MaxViewDistance = Config.GetProperty("MaxViewDistance", 22);
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