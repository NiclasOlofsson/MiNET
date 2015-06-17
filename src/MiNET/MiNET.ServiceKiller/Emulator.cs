using System;
using System.Net;
using System.Threading;

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
			//ThreadPool.SetMaxThreads(2000, 2000);

			int[] counter = {0};
			Random random = new Random();
			for (int i = 0; i < 1000; i++)
			{
				counter[0]++;
				string playerName = string.Format("Player {0}", (i + 1));
				//string playerName = "Player " + Guid.NewGuid();
				ThreadPool.QueueUserWorkItem(emulator.EmulateClient, playerName);
				Thread.Sleep(random.Next(33, 333));
			}

			Console.WriteLine("Clients done. Press <enter> to exit.");

			Console.ReadLine();
			_running = false;
			Console.ReadLine();
		}

		private void EmulateClient(object state)
		{
			try
			{
				string username = (string) state;
				Console.WriteLine("Client {0} connecting...", username);

				var client = new MiNetClient(new IPEndPoint(IPAddress.Loopback, 19132));
				//var client = new MiNetClient(new IPEndPoint(IPAddress.Parse("65.52.75.30"), 19132));

				client.StartClient();

				client.SendUnconnectedPing();
				Thread.Sleep(100); // Let the server process
				//Thread.Yield();

				client.SendOpenConnectionRequest1();
				Thread.Sleep(100); // Let the server process
				//Thread.Yield();

				client.SendOpenConnectionRequest2();
				Thread.Sleep(100); // Let the server process
				//Thread.Yield();

				client.SendConnectionRequest();
				Thread.Sleep(100); // Let the server process
				//Thread.Yield();

				client.SendLogin(username);
				Thread.Sleep(100); // Let the server process
				//Thread.Yield();

				//Console.WriteLine("\t\tClient {0} connected, sleeping 10s...", username);

				Thread.Sleep(20000);

				Console.WriteLine("\t\t\t\t\t\tClient {0} moving...", username);

				Random random = new Random();
				for (int i = 0; i < 100; i++)
				{
					float y = random.Next(7, 10);
					float x, z;
					float length = random.Next(5, 20);
					double angle = 0.0;
					const double angleStepsize = 0.05;
					float heightStepsize = (float) (random.NextDouble()/5);

					while (angle < 2*Math.PI && _running)
					{
						x = (float) (length*Math.Cos(angle));
						z = (float) (length*Math.Sin(angle));
						y += heightStepsize;

						client.SendMcpeMovePlayer(x + 50, y, z + 50);
						Thread.Sleep(random.Next(50, 500));
						angle += angleStepsize;
					}
				}

				client.SendDisconnectionNotification();
				Thread.Sleep(100); // Let the server process
				//Thread.Yield();

				client.StopClient();

				Console.WriteLine("Clients {0} disconnected.", username);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
		}
	}
}