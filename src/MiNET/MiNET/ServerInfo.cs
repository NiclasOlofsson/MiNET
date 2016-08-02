using System.Collections.Concurrent;
using System.Net;
using System.Threading;
using log4net;

namespace MiNET
{
	public class ServerInfo
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(ServerInfo));

		private LevelManager _levelManager;
		public ConcurrentDictionary<IPEndPoint, PlayerNetworkSession> PlayerSessions { get; private set; }

		public int NumberOfPlayers { get; set; }

		public long NumberOfAckReceive = 0;
		public long NumberOfNakReceive = 0;

		public int NumberOfDeniedConnectionRequestsPerSecond = 0;
		public long NumberOfAckSent = 0;
		public long NumberOfFails = 0;
		public long NumberOfResends = 0;
		public long NumberOfPacketsOutPerSecond = 0;
		public long NumberOfPacketsInPerSecond = 0;
		public long TotalPacketSizeOut = 0;
		public long TotalPacketSizeIn = 0;

		public Timer ThroughPut { get; set; }
		public long Latency { get; set; }
		public int AvailableBytes = 0;

		public int MaxNumberOfPlayers { get; set; }
		public int MaxNumberOfConcurrentConnects { get; set; }
		public int ConnectionsInConnectPhase = 0;

		public ServerInfo(LevelManager levelManager, ConcurrentDictionary<IPEndPoint, PlayerNetworkSession> playerSessions)
		{
			_levelManager = levelManager;
			PlayerSessions = playerSessions;
			{
				ThroughPut = new Timer(delegate (object state)
				{
					NumberOfPlayers = PlayerSessions.Count;

					int threads;
					int portThreads;
					ThreadPool.GetAvailableThreads(out threads, out portThreads);
					double kbitPerSecondOut = Interlocked.Exchange(ref TotalPacketSizeOut, 0) * 8/1000000D;
					double kbitPerSecondIn = Interlocked.Exchange(ref TotalPacketSizeIn, 0) * 8/1000000D;
					Log.WarnFormat("{5} Pl(s) Pkt(#/s) (Out={0} In={2}) ACK/NAK/RESD/FTO(#/s) ({1}-{14})/{11}/{12}/{13} Tput(Mbit/s) ({3:F} {7:F}) Avail {8}kb Threads {9} Compl.ports {10}",
						Interlocked.Exchange(ref NumberOfPacketsOutPerSecond, 0),
						Interlocked.Exchange(ref NumberOfAckReceive, 0),
						Interlocked.Exchange(ref NumberOfPacketsInPerSecond, 0),
						kbitPerSecondOut,
						0 /*_level.LastTickProcessingTime*/,
						NumberOfPlayers,
						Latency,
						kbitPerSecondIn,
						AvailableBytes / 1000,
						threads,
						portThreads,
						Interlocked.Exchange(ref NumberOfNakReceive, 0),
						Interlocked.Exchange(ref NumberOfResends, 0),
						Interlocked.Exchange(ref NumberOfFails, 0),
						Interlocked.Exchange(ref NumberOfAckSent, 0)
						);

					//Interlocked.Exchange(ref NumberOfAckReceive, 0);
					//Interlocked.Exchange(ref NumberOfNakReceive, 0);
					//Interlocked.Exchange(ref NumberOfAckSent, 0);
					//Interlocked.Exchange(ref TotalPacketSizeOut, 0);
					//Interlocked.Exchange(ref TotalPacketSizeIn, 0);
					//Interlocked.Exchange(ref NumberOfPacketsOutPerSecond, 0);
					//Interlocked.Exchange(ref NumberOfPacketsInPerSecond, 0);
					Interlocked.Exchange(ref NumberOfDeniedConnectionRequestsPerSecond, 0);
					//Interlocked.Exchange(ref NumberOfFails, 0);
					//Interlocked.Exchange(ref NumberOfResends, 0);
				}, null, 1000, 1000);
			}
		}
	}
}