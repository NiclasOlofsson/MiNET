using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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
		private Dictionary<int, SplitPartPackage[]> _splits = new Dictionary<int, SplitPartPackage[]>();

		public Dictionary<int, SplitPartPackage[]> Splits
		{
			get { return _splits; }
		}

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