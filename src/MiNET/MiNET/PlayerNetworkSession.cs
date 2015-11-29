using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using MiNET.Net;

namespace MiNET
{
	public class PlayerNetworkSession
	{
		public object SyncRoot { get; private set; }
		public object ProcessSyncRoot { get; private set; }
		public object SyncRootUpdate { get; private set; }

		public Player Player { get; set; }
		public int Mtuize { get; set; }

		public DateTime CreateTime { get; private set; }
		public IPEndPoint EndPoint { get; private set; }
		public UdpClient UdpClient { get; set; }

		private ConcurrentQueue<int> _playerAckQueue = new ConcurrentQueue<int>();
		private ConcurrentDictionary<int, Datagram> _waitingForAcksQueue = new ConcurrentDictionary<int, Datagram>();
		private ConcurrentDictionary<int, SplitPartPackage[]> _splits = new ConcurrentDictionary<int, SplitPartPackage[]>();
		public int DatagramSequenceNumber = -1;
		public int ReliableMessageNumber = 0;
		public double SendDelay { get; set; }
		public int ErrorCount { get; set; }
		public bool IsSlowClient { get; set; }
		public bool Evicted { get; set; }
		public ConnectionState State { get; set; }

		public DateTime LastUpdatedTime { get; set; }
		public int LastDatagramNumber { get; set; }

		public bool WaitForAck { get; set; }
		public int ResendCount { get; set; }

		public PlayerNetworkSession(Player player, IPEndPoint endPoint)
		{
			State = ConnectionState.Unconnected;
			SyncRoot = new object();
			SyncRootUpdate = new object();
			ProcessSyncRoot = new object();
			Player = player;
			EndPoint = endPoint;
			CreateTime = DateTime.UtcNow;
		}

		public ConcurrentDictionary<int, SplitPartPackage[]> Splits
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

		public void Clean()
		{
			var queue = WaitingForAcksQueue;
			foreach (var kvp in queue)
			{
				Datagram datagram;
				if (queue.TryRemove(kvp.Key, out datagram)) datagram.PutPool();
			}

			foreach (var kvp in Splits)
			{
				SplitPartPackage[] splitPartPackagese;
				if (Splits.TryRemove(kvp.Key, out splitPartPackagese))
				{
					if(splitPartPackagese == null) continue;

					foreach (SplitPartPackage package in splitPartPackagese)
					{
						if (package != null) package.PutPool();
					}
				}
			}

			queue.Clear();
			Splits.Clear();
		}
	}
}