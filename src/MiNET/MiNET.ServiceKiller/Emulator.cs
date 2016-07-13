using System;
using System.Diagnostics;
using System.Net;
using System.Threading;
using log4net;
using log4net.Config;
using MiNET.Client;
using MiNET.Utils;

[assembly: XmlConfigurator(Watch = true)]

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


		//private const int TimeBetweenSpawns = 280;
		private const int TimeBetweenSpawns = 350;
		private static readonly TimeSpan DurationOfConnection = TimeSpan.FromSeconds(60);
		private const int NumberOfBots = 2000;
		private const int RanSleepMin = 150;
		private const int RanSleepMax = 450;
		private const int RequestChunkRadius = 5;

		private static bool _running = true;

		public bool Running
		{
			get { return _running; }
			set { _running = value; }
		}

		private static void Main(string[] args)
		{
			try
			{
				AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
				Console.WriteLine("Press <Enter> to start emulation...");
				Console.ReadLine();

				int threads;
				int iothreads;
				ThreadPool.GetMinThreads(out threads, out iothreads);

				DedicatedThreadPool threadPool = new DedicatedThreadPool(new DedicatedThreadPoolSettings(threads));

				ThreadPool.SetMinThreads(32000, 4000);
				ThreadPool.SetMaxThreads(32000, 4000);

				Emulator emulator = new Emulator {Running = true};
				Stopwatch watch = new Stopwatch();
				watch.Start();

				long start = DateTime.UtcNow.Ticks;

				//IPEndPoint endPoint = new IPEndPoint(Dns.GetHostEntry("yodamine.com").AddressList[0], 19132);
				IPEndPoint endPoint = new IPEndPoint(IPAddress.Loopback, 19132);

				for (int j = 0; j < NumberOfBots; j++)
				{
					string playerName = $"TheGrey{j + 1:D3}";

					ClientEmulator client = new ClientEmulator(threadPool, emulator, DurationOfConnection,
						playerName, (int) (DateTime.UtcNow.Ticks - start), endPoint,
						RanSleepMin, RanSleepMax, RequestChunkRadius);

					new Thread(o => { client.EmulateClient(); }) {IsBackground = true}.Start();
					//ThreadPool.QueueUserWorkItem(delegate { client.EmulateClient(); });

					Thread.Sleep(TimeBetweenSpawns);
				}

				Console.WriteLine("Press <enter> to stop all clients.");
				Console.ReadLine();
				emulator.Running = false;
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
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

		private static readonly ILog Log = LogManager.GetLogger(typeof (ClientEmulator));
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
				int threads;
				int iothreads;
				ThreadPool.GetAvailableThreads(out threads, out iothreads);

				Console.WriteLine("Client {0} connecting... Worker: {1}, IOC: {2}", Name, threads, iothreads);

				var client = new MiNetClient(EndPoint, Name, _threadPool);
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

				if (client.UdpClient != null) Console.WriteLine("\t\t\t\t\t\tClient {0} connected, emulating...", Name);

				Stopwatch watch = new Stopwatch();
				watch.Start();

				Thread.Sleep(3000);

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

				for (int i = 0; /*i < 10 && */Emulator.Running && watch.Elapsed < TimeToRun; i++)
				{
					if (client.UdpClient == null) break;

					float y = Random.Next(7, 10) + /*24*/ 55;
					float length = Random.Next(5, 20);

					double angle = 0.0;
					const double angleStepsize = 0.05;
					float heightStepsize = (float) (Random.NextDouble()/5);

					while (angle < 2*Math.PI && Emulator.Running)
					{
						if (client.UdpClient == null) break;

						float x = (float) (length*Math.Cos(angle));
						float z = (float) (length*Math.Sin(angle));
						y += heightStepsize;

						x += client.Level.SpawnX;
						z += client.Level.SpawnZ;

						client.CurrentLocation = new PlayerLocation(x, y, z);
						client.SendMcpeMovePlayer();
						Thread.Sleep(Random.Next(RanMin, RanMax));
						angle += angleStepsize;
					}
				}

				if (client.UdpClient != null)
				{
					//client.SendChat("Shadow gov agent BREXITING!");
					client.SendDisconnectionNotification();
				}

				client.StopClient();
				Console.WriteLine($"{watch.ElapsedMilliseconds} Client stopped. {client.UdpClient == null}, {Emulator.Running}");
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
		}
	}
}