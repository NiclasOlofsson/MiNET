using System;
using System.Net;
using MiNET.Config;
using MiNET.Config.Contracts;
using MiNET.Utils;

namespace MiNET
{
	public class PlayerFactory
	{
		private static readonly IPlayerConfiguration PlayerConfig = ConfigurationProvider.Configuration.Player;
		public virtual Player CreatePlayer(MiNetServer server, IPEndPoint endPoint, PlayerInfo playerInfo)
		{
			var player = new Player(server, endPoint);
			player.MaxViewDistance = PlayerConfig.MaxViewDistance;
			player.MoveRenderDistance = PlayerConfig.MoveRenderDistance;
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