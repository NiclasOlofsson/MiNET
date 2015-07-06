using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace MiNET
{
	public class SessionManager
	{
		private List<Session> _sessions = new List<Session>();

		public virtual Session FindSession(IPEndPoint endPoint, int clientId, string username)
		{
			return _sessions.FirstOrDefault(session => session.EndPoint.Address.Equals(endPoint.Address)
			                                           && session.ClientId == clientId
			                                           && session.Username.Equals(username, StringComparison.InvariantCultureIgnoreCase));
		}

		public virtual Session CreateSession(IPEndPoint endPoint, int clientId, string username)
		{
			Session session = new Session(endPoint, clientId, username);
			_sessions.Add(session);
			return session;
		}
	}

	public class Session : Dictionary<string, object>
	{
		public IPEndPoint EndPoint { get; set; }
		public int ClientId { get; set; }
		public string Username { get; set; }

		public Session(IPEndPoint endPoint, int clientId, string username)
		{
			EndPoint = endPoint;
			ClientId = clientId;
			Username = username;
		}
	}
}