using System.Net;
using MiNET.Net;

namespace MiNET
{
	public interface INetworkHandler
	{
		void Close();

		void SendPackage(Package package);
		void SendDirectPackage(Package package);
		IPEndPoint GetClientEndPoint();
		long GetNetworkNetworkIdentifier();
	}
}