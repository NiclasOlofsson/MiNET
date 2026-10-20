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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using log4net.Config;
using MiNET.Utils;
using MiNET.Utils.IO;

#pragma warning disable 1591

namespace MiNET.ServiceKiller
{
	public class Emulator
	{
		//private const int TimeBetweenSpawns = 500;
		//private const int DurationOfConnection = 60 * 1000;
		//private const int NumberOfBots = 2000;
		//private const int RanSleepMin = 150;
		//private const int RanSleepMax = 450;
		//private const int RequestChunkRadius = 8;


		private static int TimeBetweenSpawns = 0;
		private static TimeSpan DurationOfConnection = TimeSpan.FromMinutes(15);
		private static int NumberOfBots = 100;
		private static int RanSleepMin = 40;
		private static int RanSleepMax = 100;
		private static int RequestChunkRadius = 5;
		private static bool ConcurrentSpawn = true;
		private static int ConcurrentBatchSize = 5;
		private static string Walker = "helix";
		private static int WalkBounds = 800;

		public AutoResetEvent ConcurrentSpawnWaitHandle = new AutoResetEvent(false);

		public bool Running { get; set; } = true;

		/// <summary>The fleet's one pacing clock; every spawned bot's walk registers here. See <see cref="ServiceKiller.WalkClock" /> for why this replaced a sleeping thread per bot.</summary>
		public WalkClock WalkClock { get; } = new WalkClock();

		private static int NameOffset = 0;

		/// <summary>One fresh path per bot from the run's --walker choice; paths carry per-bot state.</summary>
		public static IWalkPath CreateWalkPath()
		{
			return string.Equals(Walker, "waypoints", StringComparison.OrdinalIgnoreCase) ? new WaypointPath(WalkBounds) : new HelixPath();
		}

		/// <summary>
		/// </summary>
		/// <param name="numberOfBots">The number of bots to spawn.</param>
		/// <param name="durationOfConnection">How long (in seconds) should each individual bots stay connected.</param>
		/// <param name="concurrentSpawn">Should the emulator spawn bots in parallel.</param>
		/// <param name="batchSize">If parallel spawn, how many in each batch.</param>
		/// <param name="chunkRadius">The chunk radius the bots will request. Server may override.</param>
		/// <param name="processorAffinity">Processor affinity mask represented as an integer.</param>
		/// <param name="transport">Transport the bots connect over: nethernet (raknet is gone).</param>
		/// <param name="nameOffset">Offset for bot numbering, so parallel emulator processes get distinct names.</param>
		/// <param name="auto">Run unattended: start without a prompt, exit when every bot's duration has elapsed. For scripted/detached runs, where stdin is not a console.</param>
		/// <param name="sendIntervalMin">Lower bound (ms) of each bot's fixed movement-send cadence; with max, the per-bot interval is drawn once from this range. Higher = fewer packets = less CPU on both ends.</param>
		/// <param name="sendIntervalMax">Upper bound (ms) of the cadence range.</param>
		/// <param name="port">Signaling port to aim at. 19132 is the server port; a parking or side server may be elsewhere.</param>
		/// <param name="walker">
		///     Path shape for every bot: helix (the historical default, whole fleet corkscrewing in
		///     one dense cluster at spawn) or waypoints (wander the map inside --walk-bounds, with
		///     shared hubs where routes cross). Runs are only comparable to runs with the same walker.
		/// </param>
		/// <param name="walkBounds">waypoints only: radius in blocks of the wander disc around the level spawn.</param>
		/// <param name="host">
		///     Server to connect to. Loopback keeps the bots on the same machine, which is fast but
		///     makes every send also pay the receiving side's delivery on the same box, so the CPU
		///     numbers it produces are not the server's alone. Naming the public host instead
		///     (yodamine.com) sends the traffic out to the router and back, a real network path,
		///     which is what a measurement run wants.
		/// </param>
		private static void Main(int numberOfBots = 500, int durationOfConnection = 900, bool concurrentSpawn = true, int batchSize = 5, int chunkRadius = 5, int processorAffinity = 0, string transport = "nethernet", int nameOffset = 0, bool auto = false, int sendIntervalMin = 40, int sendIntervalMax = 100, string host = "127.0.0.1", int port = 19132, string walker = "helix", int walkBounds = 800)
		{
			NumberOfBots = numberOfBots;
			DurationOfConnection = TimeSpan.FromSeconds(durationOfConnection);
			ConcurrentSpawn = concurrentSpawn;
			ConcurrentBatchSize = batchSize;
			RequestChunkRadius = chunkRadius;
			NameOffset = nameOffset;
			RanSleepMin = sendIntervalMin;
			RanSleepMax = sendIntervalMax;
			Walker = walker;
			WalkBounds = walkBounds;

			if (!string.Equals(transport, "nethernet", StringComparison.OrdinalIgnoreCase))
			{
				Console.WriteLine($"Unknown transport '{transport}': nethernet is the only transport (RakNet was removed).");
				return;
			}

			if (!string.Equals(walker, "helix", StringComparison.OrdinalIgnoreCase) && !string.Equals(walker, "waypoints", StringComparison.OrdinalIgnoreCase))
			{
				Console.WriteLine($"Unknown walker '{walker}': expected helix or waypoints.");
				return;
			}

			// Processor affinity exists only on Windows and Linux. macOS throws PlatformNotSupportedException.
			if (OperatingSystem.IsWindows() || OperatingSystem.IsLinux())
			{
				var currentProcess = Process.GetCurrentProcess();
				currentProcess.ProcessorAffinity = processorAffinity <= 0 ? currentProcess.ProcessorAffinity : (IntPtr) processorAffinity;
			}

			//var listener = new GcFinalizersEventListener();

			var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
			XmlConfigurator.Configure(logRepository, new FileInfo(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "log4net.xml")));

			Console.ResetColor();
			Console.BackgroundColor = ConsoleColor.DarkGreen;
			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine(MiNetServer.MiNET);
			Console.BackgroundColor = ConsoleColor.Black;
			Console.ForegroundColor = ConsoleColor.White;

			try
			{
				AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
				if (!auto)
				{
					Console.WriteLine("Press <Enter> to start emulation...");
					Console.ReadLine();
				}

				var emulator = new Emulator {Running = true};
				emulator.WalkClock.Start();
				long start = DateTime.UtcNow.Ticks;

				// A name resolves to whatever the router hands back, so pointing the fleet at the
				// public host bounces every datagram out and back instead of short-circuiting in the
				// loopback adapter. Same server, real network path.
				IPAddress address = IPAddress.TryParse(host, out IPAddress parsed) ? parsed : Dns.GetHostEntry(host).AddressList.First(a => a.AddressFamily == AddressFamily.InterNetwork);
				var endPoint = new IPEndPoint(address, port);
				Console.WriteLine($"Target: {host} -> {endPoint}");

				Task.Run(() =>
				{
					var sw = Stopwatch.StartNew();
					for (int j = 0; j < NumberOfBots; j++)
					{
						string playerName = $"TheGrey{j + 1 + NameOffset:D3}";

						var client = new EmulatorClient(emulator,
							DurationOfConnection,
							playerName,
							(int) (DateTime.UtcNow.Ticks - start),
							endPoint,
							RanSleepMin,
							RanSleepMax,
							RequestChunkRadius);

						new Thread(o => { client.EmulateClient(); }) {IsBackground = true}.Start();

						if (ConcurrentSpawn)
						{
							if (j % ConcurrentBatchSize == 0 && j != 0)
							{
								for (int i = 0; i < ConcurrentBatchSize; i++)
								{
									emulator.ConcurrentSpawnWaitHandle.WaitOne(TimeSpan.FromMilliseconds(1000));
								}
							}

							continue;
						}

						emulator.ConcurrentSpawnWaitHandle.WaitOne();

						long elapsed = sw.ElapsedMilliseconds;
						if (elapsed < TimeBetweenSpawns) Thread.Sleep((int) (TimeBetweenSpawns - elapsed));

						sw.Restart();
					}
				});

				if (auto)
				{
					// Unattended: the spawn ramp plus every bot's scripted duration bounds the run;
					// the margin covers connect/teardown straggling. Killing the process early is the
					// out-of-band stop.
					Thread.Sleep(DurationOfConnection + TimeSpan.FromSeconds(60));
					emulator.Running = false;
					Thread.Sleep(TimeSpan.FromSeconds(5));

					// Transport plumbing (session dispatch threads, timers) can hold the process open
					// past Main; an unattended run must never rely on every thread being background.
					Environment.Exit(0);
				}
				else
				{
					Console.WriteLine("Press <enter> to stop all clients.");
					Console.ReadLine();

					emulator.Running = false;

					Console.WriteLine("Stopping all clients, press <enter> to exit.");
					Console.ReadLine();
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
			finally
			{
				Console.ResetColor();
			}
		}

		private static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs unhandledExceptionEventArgs)
		{
			Console.WriteLine("ERROR." + unhandledExceptionEventArgs.ExceptionObject);
		}
	}
}