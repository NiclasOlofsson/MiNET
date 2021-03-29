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
using System.Net;
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

		public AutoResetEvent ConcurrentSpawnWaitHandle = new AutoResetEvent(false);

		public bool Running { get; set; } = true;

		/// <summary>
		/// </summary>
		/// <param name="numberOfBots">The number of bots to spawn.</param>
		/// <param name="durationOfConnection">How long (in seconds) should each individual bots stay connected.</param>
		/// <param name="concurrentSpawn">Should the emulator spawn bots in parallel.</param>
		/// <param name="batchSize">If parallel spawn, how many in each batch.</param>
		/// <param name="chunkRadius">The chunk radius the bots will request. Server may override.</param>
		/// <param name="processorAffinity">Processor affinity mask represented as an integer.</param>
		private static void Main(int numberOfBots = 500, int durationOfConnection = 900, bool concurrentSpawn = true, int batchSize = 5, int chunkRadius = 5, int processorAffinity = 0)
		{
			NumberOfBots = numberOfBots;
			DurationOfConnection = TimeSpan.FromSeconds(durationOfConnection);
			ConcurrentSpawn = concurrentSpawn;
			ConcurrentBatchSize = batchSize;
			RequestChunkRadius = chunkRadius;

			var currentProcess = Process.GetCurrentProcess();
			currentProcess.ProcessorAffinity = processorAffinity <= 0 ? currentProcess.ProcessorAffinity : (IntPtr) processorAffinity;

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
				Console.WriteLine("Press <Enter> to start emulation...");
				Console.ReadLine();

				var threadPool = new DedicatedThreadPool(new DedicatedThreadPoolSettings(numberOfBots, "Shared_Thread"));

				var emulator = new Emulator {Running = true};
				long start = DateTime.UtcNow.Ticks;

				//IPEndPoint endPoint = new IPEndPoint(Dns.GetHostEntry("yodamine.com").AddressList[0], 19132);
				var endPoint = new IPEndPoint(IPAddress.Loopback, 19132);

				Task.Run(() =>
				{
					var sw = Stopwatch.StartNew();
					for (int j = 0; j < NumberOfBots; j++)
					{
						string playerName = $"TheGrey{j + 1:D3}";

						var client = new EmulatorClient(threadPool,
							emulator,
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

				Console.WriteLine("Press <enter> to stop all clients.");
				Console.ReadLine();

				emulator.Running = false;

				Console.WriteLine("Stopping all clients, press <enter> to exit.");
				Console.ReadLine();
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