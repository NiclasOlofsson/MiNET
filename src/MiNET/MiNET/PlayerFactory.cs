using System.Net;
using MiNET.Worlds;

namespace MiNET
{
	public class PlayerFactory
	{
		public virtual Player CreatePlayer(MiNetServer server, IPEndPoint endPoint, int mtuSize)
		{
			Level level = server.LevelManager.GetLevel("Default");
			return new Player(server, endPoint, level, mtuSize);
		}
	}
}