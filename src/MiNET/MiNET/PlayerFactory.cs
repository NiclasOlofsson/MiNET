using System.Net;

namespace MiNET
{
	public class PlayerFactory
	{
		public virtual Player CreatePlayer(MiNetServer server, IPEndPoint endPoint, int mtuSize)
		{
			return new Player(server, endPoint, mtuSize);
		}
	}
}