using System;
using System.Net;
using System.Threading;
using MiNET.Client;
using MiNET.Utils;

namespace MiNET.ServiceKiller
{
	internal class Emulator
	{
		private static bool _running = true;

		private static void Main(string[] args)
		{
			Console.WriteLine("Clients sleeping...");
			Thread.Sleep(1000); // Let the server start first

			Console.ReadLine();

			var emulator = new Emulator();
			ThreadPool.SetMinThreads(1000, 1000);
			//ThreadPool.SetMinThreads(1000, 1000);
			//ThreadPool.SetMaxThreads(2000, 2000);

			int[] counter = {0};
			Random random = new Random();
			for (int i = 0; i < 1000; i++)
			{
				if (i > 0 && i%100 == 0) Thread.Sleep(1000);

				counter[0]++;
				string playerName = string.Format("Player-{0}", (i + 1));
				//string playerName = "Player " + Guid.NewGuid();
				ThreadPool.QueueUserWorkItem(emulator.EmulateClient, playerName);
				Thread.Sleep(1);
				//Thread.Sleep(random.Next(250, 551));
				//Thread.Sleep(random.Next(150, 1100));
			}

			Console.WriteLine("Clients done. Press <enter> to exit.");

			Console.ReadLine();
			_running = false;
			Console.ReadLine();
		}

		private void EmulateClient(object state)
		{
			Random random = new Random();

			try
			{
				string username = (string) state;
				Console.WriteLine("Client {0} connecting...", username);

				IPHostEntry Host = Dns.GetHostEntry("play.inpvp.net");


				//var client = new MiNetClient(new IPEndPoint(Host.AddressList[0], random.Next(2) == 0 ? 19152 : 19153));
				//var client = new MiNetClient(new IPEndPoint(Host.AddressList[0], 19152));
				var client = new MiNetClient(new IPEndPoint(IPAddress.Loopback, 19132), new IPEndPoint(IPAddress.Loopback, 0));
				client.Username = username;
				//var client = new MiNetClient(new IPEndPoint(IPAddress.Parse("65.52.75.30"), 19132));

				client.StartServer();
				Console.WriteLine("Client started.");

				Thread.Sleep(2000);

				client.SendUnconnectedPing();

				//client.StartServer();

				//client.SendUnconnectedPing();
				//Thread.Sleep(100); // Let the server process
				////Thread.Yield();

				//client.SendOpenConnectionRequest1();
				//Thread.Sleep(100); // Let the server process
				////Thread.Yield();

				//client.SendOpenConnectionRequest2();
				//Thread.Sleep(100); // Let the server process
				////Thread.Yield();

				//client.SendConnectionRequest();
				//Thread.Sleep(100); // Let the server process
				////Thread.Yield();

				//client.SendLogin(username);
				////Thread.Sleep(1000); // Let the server process
				////Thread.Yield();

				//Console.WriteLine("\t\tClient {0} connected, sleeping 10s...", username);

				Thread.Sleep(7000);

				if (client.Listener != null) Console.WriteLine("\t\t\t\t\t\tClient {0} moving...", username);

				for (int i = 0; i < 100; i++)
				{
					if (client.Listener == null) break;

					float y = random.Next(7, 10) + /*24*/ 30;
					float x, z;
					float length = random.Next(5, 20);

					double angle = 0.0;
					const double angleStepsize = 0.05;
					float heightStepsize = (float) (random.NextDouble()/5);

					while (angle < 2*Math.PI && _running)
					{
						if (client.Listener == null) break;

						x = (float) (length*Math.Cos(angle));
						z = (float) (length*Math.Sin(angle));
						y += heightStepsize;

						x += -421;
						z += -1633;

						client.CurrentLocation = new PlayerLocation(x, y, z);
						client.SendMcpeMovePlayer();
						Thread.Sleep(random.Next(150, 450));
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