using System;
using System.Collections.Concurrent;
using System.Net;
using MiNET.Net;

namespace MiNET
{
	public class PlayerNetworkSession
	{
		public Player Player { get; set; }
		public IPEndPoint EndPoint { get; set; }
		private ConcurrentQueue<int> _playerAckQueue = new ConcurrentQueue<int>();
		private ConcurrentDictionary<int, Datagram> _waitingForAcksQueue = new ConcurrentDictionary<int, Datagram>();

		public ConcurrentQueue<int> PlayerAckQueue
		{
			get { return _playerAckQueue; }
		}

		public ConcurrentDictionary<int, Datagram> WaitingForAcksQueue
		{
			get { return _waitingForAcksQueue; }
		}

		public DateTime LastUpdatedTime { get; set; }

		public PlayerNetworkSession(Player player, IPEndPoint endPoint)
		{
			Player = player;
			EndPoint = endPoint;
		}
	}
}