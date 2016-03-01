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
		private static bool _running = true;

		public bool Running
		{
			get { return _running; }
			set { _running = value; }
		}

		private static void Main(string[] args)
		{
			AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
			Console.WriteLine("Press <Enter> to start emulation...");
			Console.ReadLine();

			ThreadPool.SetMinThreads(1000, 1000);

			Emulator emulator = new Emulator {Running = true};
			Stopwatch watch = new Stopwatch();
			watch.Start();

			long start = DateTime.UtcNow.Ticks;

			for (int j = 0; j < 200; j++)
			{
				string playerName = $"{Guid.NewGuid()}";
				//string playerName = $"TheGrey{j + 1:D3}";
				ClientEmulator client = new ClientEmulator(emulator, 12*60*1000, playerName, (int) (DateTime.UtcNow.Ticks-start));
				ThreadPool.QueueUserWorkItem(delegate { client.EmulateClient(); });

				Thread.Sleep(150);
			}

			Console.WriteLine("Press <enter> to stop all clients.");
			Console.ReadLine();
			emulator.Running = false;

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
		private static readonly ILog Log = LogManager.GetLogger(typeof (ClientEmulator));

		public Emulator Emulator { get; private set; }
		public string Name { get; set; }
		public int ClientId { get; set; }
		public Random Random { get; set; } = new Random();
		public long TimeToRun { get; set; }

		public ClientEmulator(Emulator emulator, long timeToRun, string name, int clientId)
		{
			Emulator = emulator;
			TimeToRun = timeToRun;
			Name = name;
			ClientId = clientId;
		}

		public void EmulateClient()
		{
			try
			{
				Console.WriteLine("Client {0} connecting...", Name);

				//var client = new MiNetClient(new IPEndPoint(Dns.GetHostEntry("play.lbsg.net").AddressList[0], 19132), new IPEndPoint(IPAddress.Any, 0));
				//var client = new MiNetClient(new IPEndPoint(Dns.GetHostEntry("test.inpvp.net").AddressList[0], 19132), new IPEndPoint(IPAddress.Any, 0));
				//var client = new MiNetClient(new IPEndPoint(IPAddress.Parse("46.7.101.176"), 19132), Name);
				var client = new MiNetClient(new IPEndPoint(IPAddress.Loopback, 19134), Name);
				client.IsEmulator = true;
				client.ClientId = ClientId;

				client.StartClient();
				Console.WriteLine("Client started.");

				client.HaveServer = true;
				client.SendOpenConnectionRequest1();

				Thread.Sleep(2000);

				client.LoginSent = true;

				Stopwatch watch = new Stopwatch();
				watch.Start();
				if (client.UdpClient != null) Console.WriteLine("\t\t\t\t\t\tClient {0} moving...", Name);

				for (int i = 0; /*i < 10 && */Emulator.Running && watch.ElapsedMilliseconds < TimeToRun; i++)
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
						Thread.Sleep(50);
						//Thread.Sleep(Random.Next(150, 450));
						angle += angleStepsize;
					}
				}

				Console.WriteLine($"{watch.ElapsedMilliseconds} Client stopping. {client.UdpClient == null}, {Emulator.Running}");

				if (client.UdpClient != null) client.SendDisconnectionNotification();

				client.StopClient();
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
		}
	}
}