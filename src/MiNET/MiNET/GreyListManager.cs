using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using log4net;

namespace MiNET
{
	public class GreylistManager
	{
		private readonly MiNetServer _server;
		private static readonly ILog Log = LogManager.GetLogger(typeof (GreylistManager));

		private HashSet<IPAddress> _blacklist = new HashSet<IPAddress>();
		private ConcurrentDictionary<IPAddress, DateTime> _greylist = new ConcurrentDictionary<IPAddress, DateTime>();

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
			lock (_blacklist)
			{
				return _blacklist.Contains(senderAddress);
			}
		}

		public virtual void Blacklist(IPAddress senderAddress)
		{
			lock (_blacklist)
			{
				_blacklist.Add(senderAddress);
			}
		}

		public virtual bool AcceptConnection(IPAddress senderAddress)
		{
			if (IsWhitelisted(senderAddress)) return true;

			ServerInfo serverInfo = _server.ServerInfo;

			if (serverInfo.NumberOfPlayers >= serverInfo.MaxNumberOfPlayers || serverInfo.ConnectionsInConnectPhase >= serverInfo.MaxNumberOfConcurrentConnects)
			{
				if (Log.IsInfoEnabled)
					Log.InfoFormat("Rejected connection (server full) from: {0}", senderAddress);

				return false;
			}

			return true;
		}

		public virtual bool IsGreylisted(IPAddress address)
		{
			//if (_greylist.ContainsKey(address))
			//{
			//	if (_greylist[address] > DateTime.UtcNow)
			//	{
			//		return true;
			//	}

			//	DateTime waste;
			//	_greylist.TryRemove(address, out waste);
			//}

			return false;
		}

		public void Greylist(IPAddress address, int time)
		{
			//var dateTime = DateTime.UtcNow.AddMilliseconds(time);
			//Thread.Sleep(1);
			//_greylist.TryAdd(address, dateTime);
		}
	}
}