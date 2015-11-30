using System;
using System.Diagnostics;
using System.Net;
using System.Threading;
using MiNET.Client;
using MiNET.Utils;

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
			Random random = new Random();

			//{
			//	Stopwatch watch = new Stopwatch();
			//	watch.Start();
			//	int i = 0;
			//	while (watch.ElapsedMilliseconds < 300*1000)
			//	{
			//		if (i > 0 && i%10 == 0) Thread.Sleep(10000);

			//		string playerName = string.Format("Player-{0}", (i + 1));
			//		//string playerName = "Player " + Guid.NewGuid();
			//		ClientEmulator client = new ClientEmulator(emulator, random.Next(60, 120)*1000, playerName, i)
			//		{
			//			Random = random
			//		};
			//		ThreadPool.QueueUserWorkItem(delegate { client.EmulateClient(); });

			//		Thread.Sleep(500);

			//		i++;
			//	}
			//}

			{
				Stopwatch watch = new Stopwatch();
				watch.Start();
				for (int j = 0; j < 100; j++)
				{
					string playerName = string.Format("Player-{0}", (j + 1));
					ClientEmulator client = new ClientEmulator(emulator, 300*1000, playerName, j)
					{
						Random = random
					};
					ThreadPool.QueueUserWorkItem(delegate { client.EmulateClient(); });

					Thread.Sleep(500);
				}
			}

			Thread.Sleep(300000);

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
		public Emulator Emulator { get; private set; }
		public string Name { get; set; }
		public int ClientId { get; set; }
		public Random Random { get; set; }
		public int TimeToRun { get; set; }

		public ClientEmulator(Emulator emulator, int timeToRun, string name, int clientId)
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
				//var client = new MiNetClient(new IPEndPoint(IPAddress.Parse("188.165.235.161"), 19132), new IPEndPoint(IPAddress.Any, 0));
				var client = new MiNetClient(new IPEndPoint(IPAddress.Loopback, 19132), Name);
				client.ClientId = ClientId;

				client.StartClient();
				Console.WriteLine("Client started.");

				Thread.Sleep(3000);

				client.SendUnconnectedPing();

				Thread.Sleep(2000);

				//client.LoginSent = true;

				Stopwatch watch = new Stopwatch();
				watch.Start();
				if (client.UdpClient != null) Console.WriteLine("\t\t\t\t\t\tClient {0} moving...", Name);

				for (int i = 0; i < 10 && Emulator.Running && watch.ElapsedMilliseconds < TimeToRun; i++)
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

						x += -2562;
						z += 743;

						client.CurrentLocation = new PlayerLocation(x, y, z);
						client.SendMcpeMovePlayer();
						Thread.Sleep(Random.Next(150, 450));
						angle += angleStepsize;
					}
				}

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