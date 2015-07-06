using System.Net;
using MiNET.Worlds;

namespace MiNET
{
	public class PlayerFactory
	{
		public virtual Player CreatePlayer(MiNetServer server, IPEndPoint endPoint, Level level, int mtuSize)
		{
			return new Player(server, endPoint, level, mtuSize);
		}
	}
}