using System;
using System.ComponentModel;
using System.Threading;

namespace MiNET.ServiceKiller
{
	internal class Emulator
	{
		private static void Main(string[] args)
		{
			Console.WriteLine("Clients sleeping...");
			Thread.Sleep(1000); // Let the server start first

			var emulator = new Emulator();

			int[] counter = {0};
			Random random = new Random();
			for (int i = 0; i < 1*2; i++)
			{
				counter[0]++;
				var worker = new BackgroundWorker();
				worker.DoWork += (sender, eventArgs) => emulator.EmulateClient("Player " + i);
				worker.RunWorkerCompleted += (sender, eventArgs) => counter[0]--;
				worker.RunWorkerAsync();
				Thread.Sleep(random.Next(200, 1000));
			}

			while (counter[0] > 0)
			{
				Thread.Yield();
			}

			Console.WriteLine("Clients done. Press <enter> to exit.");

			Console.ReadLine();
		}

		private void EmulateClient(string username)
		{
			Console.WriteLine("Clients {0} connecting...", username);

			var client = new MiNetClient();
			client.StartClient();

			client.SendUnconnectedPing();
			Thread.Sleep(100); // Let the server process
			Thread.Yield();

			client.SendOpenConnectionRequest1();
			Thread.Sleep(100); // Let the server process
			Thread.Yield();

			client.SendOpenConnectionRequest2();
			Thread.Sleep(100); // Let the server process
			Thread.Yield();

			client.SendConnectionRequest();
			Thread.Sleep(100); // Let the server process
			Thread.Yield();

			client.SendMcpeLogin(username);
			Thread.Sleep(100); // Let the server process
			Thread.Yield();


			Random random = new Random();
			for (int i = 0; i < 60; i++)
			{
				int y = random.Next(4, 8);
				int x, z;
				int length = random.Next(5, 20);
				double angle = 0.0;
				double angleStepsize = 0.05;

				while (angle < 2*Math.PI)
				{
					x = (int) (length*Math.Cos(angle));
					z = (int) (length*Math.Sin(angle));

					client.SendMcpeMovePlayer(x + 50, y, z + 50);
					Thread.Yield();
					Thread.Sleep(50);
					angle += angleStepsize;
				}
			}


			client.SendDisconnectionNotification();
			Thread.Sleep(100); // Let the server process
			Thread.Yield();

			client.StopClient();

			Console.WriteLine("Clients {0} disconnected.", username);
		}
	}
}