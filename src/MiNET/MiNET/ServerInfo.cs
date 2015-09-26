using System.Collections.Concurrent;
using System.Net;
using System.Threading;
using log4net;
using MiNET.Utils;

namespace MiNET
{
	public class ServerInfo
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (ServerInfo));

		private LevelManager _levelManager;
		public ConcurrentDictionary<IPEndPoint, PlayerNetworkSession> PlayerSessions { get; private set; }

		public int NumberOfPlayers { get; set; }

		public long NumberOfAckReceive { get; set; }
		public long NumberOfNakReceive { get; set; }

		public long NumberOfAckSent { get; set; }
		public long NumberOfPacketsOutPerSecond { get; set; }
		public long NumberOfPacketsInPerSecond { get; set; }
		public long TotalPacketSizeOut { get; set; }
		public long TotalPacketSizeIn { get; set; }
		public Timer ThroughPut { get; set; }
		public long Latency { get; set; }
		public int AvailableBytes { get; set; }

		public int MaxNumberOfPlayers { get; set; }
		public int MaxNumberOfConcurrentConnects { get; set; }
		public int ConnectionsInConnectPhase = 0;

		public ServerInfo(LevelManager levelManager, ConcurrentDictionary<IPEndPoint, PlayerNetworkSession> playerSessions)
		{
			_levelManager = levelManager;
			PlayerSessions = playerSessions;
			if (Config.GetProperty("EnableStatistics", true))
			{
				ThroughPut = new Timer(delegate(object state)
				{
					NumberOfPlayers = PlayerSessions.Count;

					int threads;
					int portThreads;
					ThreadPool.GetAvailableThreads(out threads, out portThreads);
					double kbitPerSecondOut = TotalPacketSizeOut*8/1000000D;
					double kbitPerSecondIn = TotalPacketSizeIn*8/1000000D;
					Log.InfoFormat("TT {4:00}ms Ly {6:00}ms {5} Pl(s) Pkt(#/s) ({0} {2}) ACK/NAK(#/s) {1}/{11} Tput(Mbit/s) ({3:F} {7:F}) Avail {8}kb Threads {9} Compl.ports {10}",
						NumberOfPacketsOutPerSecond,
						NumberOfAckSent,
						NumberOfPacketsInPerSecond,
						kbitPerSecondOut,
						0 /*_level.LastTickProcessingTime*/,
						NumberOfPlayers,
						Latency,
						kbitPerSecondIn, AvailableBytes/1000,
						threads,
						portThreads,
						NumberOfNakReceive);

					NumberOfAckReceive = 0;
					NumberOfNakReceive = 0;

					NumberOfAckSent = 0;
					TotalPacketSizeOut = 0;
					TotalPacketSizeIn = 0;
					NumberOfPacketsOutPerSecond = 0;
					NumberOfPacketsInPerSecond = 0;
				}, null, 1000, 1000);
			}
		}
	}
}