using System.Collections.Concurrent;
using System.Net;
using MiNET.Net;

namespace MiNET
{
	public class PlayerNetworkSession
	{
		public Player Player { get; set; }
		public IPEndPoint EndPoint { get; set; }
		private ConcurrentQueue<Ack> _playerAckQueue = new ConcurrentQueue<Ack>();
		private ConcurrentQueue<Package> _playerWaitingForAcksQueue = new ConcurrentQueue<Package>();

		public ConcurrentQueue<Ack> PlayerAckQueue
		{
			get { return _playerAckQueue; }
		}

		public ConcurrentQueue<Package> PlayerWaitingForAcksQueue
		{
			get { return _playerWaitingForAcksQueue; }
		}

		public PlayerNetworkSession(Player player, IPEndPoint endPoint)
		{
			Player = player;
			EndPoint = endPoint;
		}
	}
}