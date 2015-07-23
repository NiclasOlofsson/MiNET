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
			Console.WriteLine("Press <Enter> to start emulation...");
			Console.ReadLine();

			ThreadPool.SetMinThreads(1000, 1000);

			Emulator emulator = new Emulator {Running = true};
			Random random = new Random();

			{
				Stopwatch watch = new Stopwatch();
				watch.Start();
				int i = 0;
				while (watch.ElapsedMilliseconds < 300*1000)
				{
					if (i > 0 && i%20 == 0) Thread.Sleep(10000);

					string playerName = string.Format("Player-{0}", (i + 1));
					//string playerName = "Player " + Guid.NewGuid();
					ClientEmulator client = new ClientEmulator(emulator, random.Next(10, 30)*1000, playerName, i)
					{
						Random = random
					};
					ThreadPool.QueueUserWorkItem(delegate { client.EmulateClient(); });

					Thread.Sleep(500);

					i++;
				}
			}

			Console.WriteLine("Press <enter> to stop all clients.");

			Console.ReadLine();
			emulator.Running = false;

			Console.WriteLine("Emulation complete. Press <enter> to exit.");
			Console.ReadLine();
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
				var client = new MiNetClient(new IPEndPoint(IPAddress.Loopback, 19132), new IPEndPoint(IPAddress.Loopback, 0));

				client.Username = Name;
				client.ClientId = (int) DateTime.UtcNow.Ticks;

				client.StartServer();
				Console.WriteLine("Client started.");

				Thread.Sleep(2000);

				client.SendUnconnectedPing();

				Thread.Sleep(1000);

				client.LoginSent = true;

				Stopwatch watch = new Stopwatch();
				watch.Start();
				if (client.Listener != null) Console.WriteLine("\t\t\t\t\t\tClient {0} moving...", Name);

				for (int i = 0; i < 10 && Emulator.Running && watch.ElapsedMilliseconds < TimeToRun; i++)
				{
					if (client.Listener == null) break;

					float y = Random.Next(7, 10) + /*24*/ 30;
					float length = Random.Next(5, 20);

					double angle = 0.0;
					const double angleStepsize = 0.05;
					float heightStepsize = (float) (Random.NextDouble()/5);

					while (angle < 2*Math.PI && Emulator.Running)
					{
						if (client.Listener == null) break;

						float x = (float) (length*Math.Cos(angle));
						float z = (float) (length*Math.Sin(angle));
						y += heightStepsize;

						x += -421;
						z += -1633;

						client.CurrentLocation = new PlayerLocation(x, y, z);
						client.SendMcpeMovePlayer();
						Thread.Sleep(Random.Next(150, 450));
						angle += angleStepsize;
					}
				}

				if (client.Listener != null) client.SendDisconnectionNotification();

				client.StopServer();
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
		}
	}
}