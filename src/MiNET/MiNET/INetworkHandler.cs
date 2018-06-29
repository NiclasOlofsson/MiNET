using System.Net;
using MiNET.Net;

namespace MiNET
{
	public interface INetworkHandler
	{
		void Close();

		void SendPacket(Packet packet);
		void SendDirectPacket(Packet packet);
		IPEndPoint GetClientEndPoint();
		long GetNetworkNetworkIdentifier();
	}
}