using System.Collections.Concurrent;
using System.Collections.Generic;
using MiNET.Net;

namespace MiNET
{
	public class SessionManager
	{
		private ConcurrentDictionary<UUID, Session> _sessions = new ConcurrentDictionary<UUID, Session>();

		public virtual Session FindSession(Player player)
		{
			Session session;
			_sessions.TryGetValue(player.ClientUuid, out session);

			return session;
		}

		public virtual Session CreateSession(Player player)
		{
			_sessions.TryAdd(player.ClientUuid, new Session(player));

			return FindSession(player);
		}

		public virtual void SaveSession(Session session)
		{
		}

		public virtual void RemoveSession(Session session)
		{
			if (session.Player == null) return;
			if (session.Player.ClientUuid == null) return;

			_sessions.TryRemove(session.Player.ClientUuid, out session);
		}
	}

	public class Session : Dictionary<string, object>
	{
		public Player Player { get; set; }

		public Session(Player player)
		{
			Player = player;
		}
	}
}