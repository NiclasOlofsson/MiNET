#region LICENSE

// The contents of this file are subject to the Common Public Attribution
// License Version 1.0. (the "License"); you may not use this file except in
// compliance with the License. You may obtain a copy of the License at
// https://github.com/NiclasOlofsson/MiNET/blob/master/LICENSE. 
// The License is based on the Mozilla Public License Version 1.1, but Sections 14 
// and 15 have been added to cover use of software over a computer network and 
// provide for limited attribution for the Original Developer. In addition, Exhibit A has 
// been modified to be consistent with Exhibit B.
// 
// Software distributed under the License is distributed on an "AS IS" basis,
// WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License for
// the specific language governing rights and limitations under the License.
// 
// The Original Code is Niclas Olofsson.
// 
// The Original Developer is the Initial Developer.  The Initial Developer of
// the Original Code is Niclas Olofsson.
// 
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2017 Niclas Olofsson. 
// All Rights Reserved.

#endregion

using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net;
using System.Threading;
using log4net;

namespace MiNET
{
	public class ServerInfo
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (ServerInfo));

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
			//CreateCounters();

			//PerformanceCounter ctrNumberOfPacketsOutPerSecond = new PerformanceCounter("MiNET", nameof(NumberOfPacketsOutPerSecond), "MiNET", false);
			//PerformanceCounter ctrNumberOfPacketsInPerSecond = new PerformanceCounter("MiNET", nameof(NumberOfPacketsInPerSecond), "MiNET", false);
			//PerformanceCounter ctrNumberOfAckSent = new PerformanceCounter("MiNET", nameof(NumberOfAckSent), "MiNET", false);
			//PerformanceCounter ctrNumberOfAckReceive = new PerformanceCounter("MiNET", nameof(NumberOfAckReceive), "MiNET", false);
			//PerformanceCounter ctrNumberOfNakReceive = new PerformanceCounter("MiNET", nameof(NumberOfNakReceive), "MiNET", false);
			//PerformanceCounter ctrNumberOfResends = new PerformanceCounter("MiNET", nameof(NumberOfResends), "MiNET", false);
			//PerformanceCounter ctrNumberOfFails = new PerformanceCounter("MiNET", nameof(NumberOfFails), "MiNET", false);


			_levelManager = levelManager;
			PlayerSessions = playerSessions;
			{
				ThroughPut = new Timer(delegate(object state)
				{
					NumberOfPlayers = PlayerSessions.Count;

					//ctrNumberOfPacketsOutPerSecond.IncrementBy(NumberOfPacketsOutPerSecond);
					//ctrNumberOfPacketsInPerSecond.IncrementBy(NumberOfPacketsInPerSecond);
					//ctrNumberOfAckReceive.IncrementBy(NumberOfAckReceive);
					//ctrNumberOfAckSent.IncrementBy(NumberOfAckSent);
					//ctrNumberOfNakReceive.IncrementBy(NumberOfNakReceive);
					//ctrNumberOfResends.IncrementBy(NumberOfResends);
					//ctrNumberOfFails.IncrementBy(NumberOfFails);

					int threads;
					int portThreads;
					ThreadPool.GetAvailableThreads(out threads, out portThreads);
					double kbitPerSecondOut = Interlocked.Exchange(ref TotalPacketSizeOut, 0)*8/1000000D;
					double kbitPerSecondIn = Interlocked.Exchange(ref TotalPacketSizeIn, 0)*8/1000000D;
					Log.InfoFormat("{5} Pl(s) Pkt(#/s) (Out={0} In={2}) ACK/NAK/RESD/FTO(#/s) ({1}-{14})/{11}/{12}/{13} Tput(Mbit/s) ({3:F} {7:F}) Avail {8}kb Threads {9} Compl.ports {10}",
						Interlocked.Exchange(ref NumberOfPacketsOutPerSecond, 0),
						Interlocked.Exchange(ref NumberOfAckReceive, 0),
						Interlocked.Exchange(ref NumberOfPacketsInPerSecond, 0),
						kbitPerSecondOut,
						0 /*_level.LastTickProcessingTime*/,
						NumberOfPlayers,
						Latency,
						kbitPerSecondIn,
						AvailableBytes/1000,
						threads,
						portThreads,
						Interlocked.Exchange(ref NumberOfNakReceive, 0),
						Interlocked.Exchange(ref NumberOfResends, 0),
						Interlocked.Exchange(ref NumberOfFails, 0),
						Interlocked.Exchange(ref NumberOfAckSent, 0)
					);

					Interlocked.Exchange(ref NumberOfDeniedConnectionRequestsPerSecond, 0);
				}, null, 1000, 1000);
			}
		}

		protected virtual void CreateCounters()
		{
			//if (PerformanceCounterCategory.Exists("MiNET"))
			//{
			//	PerformanceCounterCategory.Delete("MiNET");
			//}

			if (!PerformanceCounterCategory.Exists("MiNET"))
			{
				CounterCreationDataCollection ccds = new CounterCreationDataCollection
				{
					new CounterCreationData(nameof(NumberOfPacketsOutPerSecond), "", PerformanceCounterType.RateOfCountsPerSecond32),
					new CounterCreationData(nameof(NumberOfPacketsInPerSecond), "", PerformanceCounterType.RateOfCountsPerSecond32),
					new CounterCreationData(nameof(NumberOfAckReceive), "", PerformanceCounterType.RateOfCountsPerSecond32),
					new CounterCreationData(nameof(NumberOfAckSent), "", PerformanceCounterType.RateOfCountsPerSecond32),
					new CounterCreationData(nameof(NumberOfNakReceive), "", PerformanceCounterType.RateOfCountsPerSecond32),
					new CounterCreationData(nameof(NumberOfResends), "", PerformanceCounterType.RateOfCountsPerSecond32),
					new CounterCreationData(nameof(NumberOfFails), "", PerformanceCounterType.RateOfCountsPerSecond32),
				};

				PerformanceCounterCategory.Create("MiNET", "MiNET Performance Counters", PerformanceCounterCategoryType.MultiInstance, ccds);
			}
		}
	}
}