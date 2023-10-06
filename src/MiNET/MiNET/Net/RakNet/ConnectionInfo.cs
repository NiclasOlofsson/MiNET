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
// The Original Code is MiNET.
// 
// The Original Developer is the Initial Developer.  The Initial Developer of
// the Original Code is Niclas Olofsson.
// 
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2020 Niclas Olofsson.
// All Rights Reserved.

#endregion

using System;
using System.Collections.Concurrent;
using System.Net;
using System.Threading;
using log4net;
using MiNET.Utils;

namespace MiNET.Net.RakNet
{
	public class ConnectionInfo
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(ConnectionInfo));

		public ConcurrentDictionary<IPEndPoint, RakSession> RakSessions { get; set; }

		// Special property for use with ServiceKiller.
		// Will disable reliability handling after login.
		public bool IsEmulator { get; set; }
		public bool DisableAck { get; set; }

		public int NumberOfPlayers { get; set; }

		public long NumberOfAckReceive = 0;
		public long NumberOfNakReceive = 0;

		public int NumberOfDeniedConnectionRequestsPerSecond = 0;
		public long NumberOfAckSent = 0;
		public long NumberOfFails = 0;
		public long NumberOfResends = 0;
		public long NumberOfPacketsOutPerSecond = 0;
		public long NumberOfPacketsInPerSecond = 0;
		public long TotalPacketSizeOutPerSecond = 0;
		public long TotalPacketSizeInPerSecond = 0;

		public Timer ThroughPut { get; set; }
		public long Latency { get; set; }

		public int MaxNumberOfPlayers { get; set; }
		public int MaxNumberOfConcurrentConnects { get; set; }
		public int ConnectionsInConnectPhase = 0;

		private long _avgSizePerPacketIn;
		private long _avgSizePerPacketOut;

		public ConnectionInfo(ConcurrentDictionary<IPEndPoint, RakSession> rakSessions)
		{
			RakSessions = rakSessions;

			if (!Log.IsInfoEnabled) return;

			//CreateCounters();

			//PerformanceCounter ctrNumberOfPacketsOutPerSecond = new PerformanceCounter("MiNET", nameof(NumberOfPacketsOutPerSecond), "MiNET", false);
			//PerformanceCounter ctrNumberOfPacketsInPerSecond = new PerformanceCounter("MiNET", nameof(NumberOfPacketsInPerSecond), "MiNET", false);
			//PerformanceCounter ctrNumberOfAckSent = new PerformanceCounter("MiNET", nameof(NumberOfAckSent), "MiNET", false);
			//PerformanceCounter ctrNumberOfAckReceive = new PerformanceCounter("MiNET", nameof(NumberOfAckReceive), "MiNET", false);
			//PerformanceCounter ctrNumberOfNakReceive = new PerformanceCounter("MiNET", nameof(NumberOfNakReceive), "MiNET", false);
			//PerformanceCounter ctrNumberOfResends = new PerformanceCounter("MiNET", nameof(NumberOfResends), "MiNET", false);
			//PerformanceCounter ctrNumberOfFails = new PerformanceCounter("MiNET", nameof(NumberOfFails), "MiNET", false);

			{

				ThroughPut = new Timer(state =>
				{
					NumberOfPlayers = RakSessions.Count;

					//ctrNumberOfPacketsOutPerSecond.IncrementBy(NumberOfPacketsOutPerSecond);
					//ctrNumberOfPacketsInPerSecond.IncrementBy(NumberOfPacketsInPerSecond);
					//ctrNumberOfAckReceive.IncrementBy(NumberOfAckReceive);
					//ctrNumberOfAckSent.IncrementBy(NumberOfAckSent);
					//ctrNumberOfNakReceive.IncrementBy(NumberOfNakReceive);
					//ctrNumberOfResends.IncrementBy(NumberOfResends);
					//ctrNumberOfFails.IncrementBy(NumberOfFails);


					Interlocked.Exchange(ref NumberOfDeniedConnectionRequestsPerSecond, 0);

					long packetSizeOut = Interlocked.Exchange(ref TotalPacketSizeOutPerSecond, 0);
					long packetSizeIn = Interlocked.Exchange(ref TotalPacketSizeInPerSecond, 0);

					double mbpsPerSecondOut = packetSizeOut * 8 / 1_000_000D;
					double mbpsPerSecondIn = packetSizeIn * 8 / 1_000_000D;

					long numberOfPacketsOutPerSecond = Interlocked.Exchange(ref NumberOfPacketsOutPerSecond, 0);
					long numberOfPacketsInPerSecond = Interlocked.Exchange(ref NumberOfPacketsInPerSecond, 0);


					_avgSizePerPacketIn = _avgSizePerPacketIn <= 0 ? packetSizeIn * 10 : (long) ((_avgSizePerPacketIn * 9) + (numberOfPacketsInPerSecond == 0 ? 0 : packetSizeIn / (double) numberOfPacketsInPerSecond));
					_avgSizePerPacketOut = _avgSizePerPacketOut <= 0 ? packetSizeOut * 10 : (long) ((_avgSizePerPacketOut * 9) + (numberOfPacketsOutPerSecond == 0 ? 0 : packetSizeOut / (double) numberOfPacketsOutPerSecond));
					_avgSizePerPacketIn /= 10; // running avg of 100 prev values
					_avgSizePerPacketOut /= 10; // running avg of 100 prev values

					long numberOfAckIn = Interlocked.Exchange(ref NumberOfAckReceive, 0);
					long numberOfAckOut = Interlocked.Exchange(ref NumberOfAckSent, 0);
					long numberOfNakIn = Interlocked.Exchange(ref NumberOfNakReceive, 0);
					long numberOfResend = Interlocked.Exchange(ref NumberOfResends, 0);
					long numberOfFailed = Interlocked.Exchange(ref NumberOfFails, 0);

					var message =
						$"Players {NumberOfPlayers}, " +
						$"Pkt in/out(#/s) {numberOfPacketsInPerSecond}/{numberOfPacketsOutPerSecond}, " +
						$"ACK(in-out)/NAK/RSND/FTO(#/s) ({numberOfAckIn}-{numberOfAckOut})/{numberOfNakIn}/{numberOfResend}/{numberOfFailed}, " +
						$"THR in/out(Mbps) {mbpsPerSecondIn:F}/{mbpsPerSecondOut:F}, " +
						$"PktSz Total in/out(B/s){packetSizeIn}/{packetSizeOut}, " +
						$"PktSz Avg(100s) in/out(B){_avgSizePerPacketIn}/{_avgSizePerPacketOut}";

					if (Config.GetProperty("ServerInfoInTitle", false))
					{
						Console.Title = message;
					}
					else
					{
						Log.Info(message);
					}
				}, null, 1000, 1000);
			}
		}

		internal void Stop()
		{
			ThroughPut?.Change(Timeout.Infinite, Timeout.Infinite);
			ThroughPut?.Dispose();
			ThroughPut = null;
		}

		protected virtual void CreateCounters()
		{
			//if (PerformanceCounterCategory.Exists("MiNET"))
			//{
			//	PerformanceCounterCategory.Delete("MiNET");
			//}

			//if (!PerformanceCounterCategory.Exists("MiNET"))
			//{
			//	CounterCreationDataCollection ccds = new CounterCreationDataCollection
			//	{
			//		new CounterCreationData(nameof(NumberOfPacketsOutPerSecond), "", PerformanceCounterType.RateOfCountsPerSecond32),
			//		new CounterCreationData(nameof(NumberOfPacketsInPerSecond), "", PerformanceCounterType.RateOfCountsPerSecond32),
			//		new CounterCreationData(nameof(NumberOfAckReceive), "", PerformanceCounterType.RateOfCountsPerSecond32),
			//		new CounterCreationData(nameof(NumberOfAckSent), "", PerformanceCounterType.RateOfCountsPerSecond32),
			//		new CounterCreationData(nameof(NumberOfNakReceive), "", PerformanceCounterType.RateOfCountsPerSecond32),
			//		new CounterCreationData(nameof(NumberOfResends), "", PerformanceCounterType.RateOfCountsPerSecond32),
			//		new CounterCreationData(nameof(NumberOfFails), "", PerformanceCounterType.RateOfCountsPerSecond32),
			//	};

			//	PerformanceCounterCategory.Create("MiNET", "MiNET Performance Counters", PerformanceCounterCategoryType.MultiInstance, ccds);
			//}
		}
	}
}