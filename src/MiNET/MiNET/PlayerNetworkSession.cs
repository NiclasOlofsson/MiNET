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
		private ConcurrentQueue<Datagram> _playerWaitingForAcksQueue = new ConcurrentQueue<Datagram>();

		public ConcurrentQueue<int> PlayerAckQueue
		{
			get { return _playerAckQueue; }
		}

		public ConcurrentQueue<Datagram> PlayerWaitingForAcksQueue
		{
			get { return _playerWaitingForAcksQueue; }
		}

		public DateTime LastUpdatedTime { get; set; }

		public PlayerNetworkSession(Player player, IPEndPoint endPoint)
		{
			Player = player;
			EndPoint = endPoint;
		}
	}
}