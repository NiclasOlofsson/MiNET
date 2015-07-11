using System.Collections.Concurrent;
using System.Collections.Generic;

namespace MiNET
{
	public class SessionManager
	{
		private ConcurrentDictionary<int, Session> _sessions = new ConcurrentDictionary<int, Session>();

		public virtual Session FindSession(Player player)
		{
			Session session;
			_sessions.TryGetValue(player.ClientId, out session);

			return session;
		}

		public virtual Session CreateSession(Player player)
		{
			_sessions.TryAdd(player.ClientId, new Session(player));

			return FindSession(player);
		}
	}

	public class Session : Dictionary<string, object>
	{
		public Player Player { get; private set; }

		public Session(Player player)
		{
			Player = player;
		}
	}
}