using System;
using System.Net;
using MiNET.Utils;

namespace MiNET
{
	public class PlayerFactory
	{
		public virtual Player CreatePlayer(MiNetServer server, IPEndPoint endPoint, PlayerInfo playerInfo)
		{
			var player = new Player(server, endPoint);
			player.MaxViewDistance = Config.GetProperty("MaxViewDistance", 22);
			player.MoveRenderDistance = Config.GetProperty("MoveRenderDistance", 1);
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