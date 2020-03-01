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
using MiNET.Client;
using MiNET.Net;
using MiNET.Utils;

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


		private const int TimeBetweenSpawns = 0;
		private static readonly TimeSpan DurationOfConnection = TimeSpan.FromMinutes(15);
		private const int NumberOfBots = 500;
		private const int RanSleepMin = 40;
		private const int RanSleepMax = 100;
		private const int RequestChunkRadius = 5;
		private const bool ConcurrentSpawn = true;
		private const int ConcurrentBatchSize = 10;

		public AutoResetEvent ConcurrentSpawnWaitHandle = new AutoResetEvent(false);

		private static bool _running = true;

		public bool Running
		{
			get { return _running; }
			set { _running = value; }
		}

		private static void Main(string[] args)
		{
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

				//int threads;
				//int iothreads;

				//ThreadPool.GetMaxThreads(out threads, out iothreads);
				//ThreadPool.SetMaxThreads(threads, 4000);

				//ThreadPool.GetMinThreads(out threads, out iothreads);
				//ThreadPool.SetMinThreads(4000, 4000);

				//DedicatedThreadPool threadPool = new DedicatedThreadPool(new DedicatedThreadPoolSettings(Environment.ProcessorCount));
				var threadPool = new DedicatedThreadPool(new DedicatedThreadPoolSettings(4000));

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

						var client = new ClientEmulator(threadPool,
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
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
			finally
			{
				Console.ResetColor();
			}

			Console.WriteLine("Emulation complete. Press <enter> to exit.");
			Console.ReadLine();
		}

		private static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs unhandledExceptionEventArgs)
		{
			Console.WriteLine("ERROR." + unhandledExceptionEventArgs.ExceptionObject);
		}
	}

	public class ClientEmulator
	{
		private readonly DedicatedThreadPool _threadPool;

		public int RanMin { get; set; }
		public int RanMax { get; set; }
		public int ChunkRadius { get; set; }

		private static readonly ILog Log = LogManager.GetLogger(typeof(ClientEmulator));
		public IPEndPoint EndPoint { get; }

		public Emulator Emulator { get; private set; }
		public string Name { get; set; }
		public int ClientId { get; set; }
		public Random Random { get; set; } = new Random();
		public TimeSpan TimeToRun { get; set; }

		public ClientEmulator(DedicatedThreadPool threadPool, Emulator emulator, TimeSpan timeToRun, string name, int clientId, IPEndPoint endPoint, int ranMin = 150, int ranMax = 450, int chunkRadius = 8)
		{
			_threadPool = threadPool;
			Emulator = emulator;
			TimeToRun = timeToRun;
			Name = name;
			ClientId = clientId;
			EndPoint = endPoint;
			RanMin = ranMin;
			RanMax = ranMax;
			ChunkRadius = chunkRadius;
		}

		public void EmulateClient()
		{
			try
			{
				Console.WriteLine($"Client {Name} connecting...");

				var client = new MiNetClient(EndPoint, Name, _threadPool);
				client.MessageDispatcher = new McpeClientMessageDispatcher(new DefaultMessageHandler(client));
				client.ChunkRadius = ChunkRadius;
				client.IsEmulator = true;
				client.ClientId = ClientId;

				client.StartClient();
				Console.WriteLine("Client started.");

				client.HaveServer = true;
				client.SendOpenConnectionRequest1();

				//client.FirstPacketWaitHandle.WaitOne();
				//client.FirstEncryptedPacketWaitHandle.WaitOne();
				client.PlayerStatusChangedWaitHandle.WaitOne();
				Emulator.ConcurrentSpawnWaitHandle.Set();
				Console.CursorLeft = Console.CursorLeft = Console.BufferWidth - $"Client {Name} connected, emulating...".Length;
				if (client.UdpClient != null) Console.WriteLine($"Client {Name} connected, emulating...");

				var runningTime = Stopwatch.StartNew();

				//Thread.Sleep(3000);

				//client.SendChat(".tp test");
				//client.SendChat("/join bb");
				//client.SendChat("/join skywars");

				//Thread.Sleep(TimeToRun);

				//if (client.CurrentLocation != null)
				//{
				//	client.CurrentLocation = new PlayerLocation(client.CurrentLocation.X, -10, client.CurrentLocation.Z);
				//	client.SendMcpeMovePlayer();
				//	Thread.Sleep(3000);
				//}

				//client.SendChat("/hub");

				//Thread.Sleep(3000);

				for (int i = 0; /*i < 10 && */Emulator.Running && runningTime.Elapsed < TimeToRun; i++)
				{
					if (client.UdpClient == null) break;

					float y = client.LevelInfo.SpawnX + Random.Next(7, 10);
					float length = Random.Next(5, 20);

					double angle = 0.0;
					const double angleStepsize = 0.05;
					float heightStepSize = (float) (Random.NextDouble() / 5);

					while (angle < 2 * Math.PI && Emulator.Running)
					{
						if (client.UdpClient == null) break;

						float x = (float) (length * Math.Cos(angle));
						float z = (float) (length * Math.Sin(angle));
						y += heightStepSize;

						x += client.LevelInfo.SpawnX;
						z += client.LevelInfo.SpawnZ;

						client.CurrentLocation = new PlayerLocation(x, y, z, (float) angle.ToDegrees(), (float) angle.ToDegrees());
						client.SendMcpeMovePlayer();

						int timeout = RanMin == RanMax ? RanMin : Random.Next(RanMin, RanMax);
						if (timeout > 0) Thread.Sleep(timeout);

						angle += angleStepsize;
					}
				}

				if (client.UdpClient != null)
				{
					client.SendChat("Shadow gov agent BREXITING!");
					client.SendDisconnectionNotification();
				}

				client.StopClient();
				Console.WriteLine($"{runningTime.ElapsedMilliseconds} Client stopped. {client.UdpClient == null}, {Emulator.Running}");
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
		}
	}
}