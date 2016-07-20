using MiNET.Net;

namespace MiNET
{
	public interface INetworkHandler
	{
		void SendPackage(Package package);
		void SendDirectPackage(Package package);
	}
}