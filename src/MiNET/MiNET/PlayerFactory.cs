using System.Net;

namespace MiNET
{
	public class PlayerFactory
	{
		public virtual Player CreatePlayer(MiNetServer server, IPEndPoint endPoint)
		{
			return new Player(server, endPoint);
		}
	}
}