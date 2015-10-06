using System.Collections.Generic;
using System.Net;
using log4net;

namespace MiNET
{
	public class GreylistManager
	{
		private readonly MiNetServer _server;
		private static readonly ILog Log = LogManager.GetLogger(typeof (GreylistManager));

		private IDictionary<IPAddress, bool> _blacklist = new Dictionary<IPAddress, bool>();

		public GreylistManager(MiNetServer server)
		{
			_server = server;
		}

		public virtual bool IsWhitelisted(IPAddress senderAddress)
		{
			return false;
		}

		public virtual bool IsBlacklisted(IPAddress senderAddress)
		{
			return _blacklist.ContainsKey(senderAddress);
		}

		public virtual void Blacklist(IPAddress senderAddress)
		{
			if (!_blacklist.ContainsKey(senderAddress))
			{
				_blacklist.Add(senderAddress, true);
			}
		}

		public virtual bool AcceptConnection(IPEndPoint senderEndpoint)
		{
			ServerInfo serverInfo = _server.ServerInfo;

			if (serverInfo.NumberOfPlayers >= serverInfo.MaxNumberOfPlayers || serverInfo.ConnectionsInConnectPhase >= serverInfo.MaxNumberOfConcurrentConnects)
			{
				if (Log.IsInfoEnabled)
					Log.InfoFormat("Rejected connection (server full) from: {0} {1}", senderEndpoint.Address, senderEndpoint.Port);

				return false;
			}

			return true;
		}
	}
}