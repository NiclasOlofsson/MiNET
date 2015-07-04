using System.Net;

namespace MiNET.Net
{
	public class ConnectionRequestAcceptedManual : Package<ConnectionRequestAcceptedManual>
	{
		private readonly short _port;
		private readonly long _sessionId;

		public ConnectionRequestAcceptedManual(short port, long sessionId)
		{
			Id = 0x10;
			_port = port;
			_sessionId = sessionId;
		}

		public ConnectionRequestAcceptedManual()
		{
			Id = 0x10;
		}

		protected override void EncodePackage()
		{
			Write(Id);
			Write(new IPEndPoint(IPAddress.Loopback, 19132));
			Write((short) 0);
			for (int i = 0; i < 10; i++)
			{
				if(i == 0)
				{
					Write(new IPEndPoint(IPAddress.Loopback, 19132));
					continue;
				}
				Write(new IPEndPoint(IPAddress.Any, 19132));
			}
			Write((long) 0);
			Write((long) 0);
		}
	}
}