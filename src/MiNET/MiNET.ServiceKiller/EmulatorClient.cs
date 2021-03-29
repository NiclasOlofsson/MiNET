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
using System.Net;
using System.Threading;
using log4net;
using MiNET.Client;
using MiNET.Utils;
using MiNET.Utils.IO;
using MiNET.Utils.Vectors;

#pragma warning disable 1591

namespace MiNET.ServiceKiller
{
	public class EmulatorClient
	{
		private readonly DedicatedThreadPool _threadPool;

		public int RanMin { get; set; }
		public int RanMax { get; set; }
		public int ChunkRadius { get; set; }

		private static readonly ILog Log = LogManager.GetLogger(typeof(EmulatorClient));
		public IPEndPoint EndPoint { get; }

		public Emulator Emulator { get; private set; }
		public string Name { get; set; }
		public int ClientId { get; set; }
		public Random Random { get; set; } = new Random();
		public TimeSpan TimeToRun { get; set; }

		public EmulatorClient(DedicatedThreadPool threadPool, Emulator emulator, TimeSpan timeToRun, string name, int clientId, IPEndPoint endPoint, int ranMin = 150, int ranMax = 450, int chunkRadius = 8)
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
				client.ChunkRadius = ChunkRadius;
				client.IsEmulator = true;
				client.UseBlobCache = true;
				client.ClientId = ClientId;

				client.StartClient();
				client.Connection.ConnectionInfo.DisableAck = true;
				Console.WriteLine("Client started.");

				client.SendOpenConnectionRequest1();

				//client.FirstPacketWaitHandle.WaitOne();
				//client.FirstEncryptedPacketWaitHandle.WaitOne();
				client.PlayerStatusChangedWaitHandle.WaitOne();
				client.Connection.ConnectionInfo.IsEmulator = true;
				Emulator.ConcurrentSpawnWaitHandle.Set();
				Console.CursorLeft = Console.CursorLeft = Console.BufferWidth - $"Client {Name} connected, emulating...".Length;
				Console.WriteLine($"Client {Name} connected, emulating...");

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
					if (!client.IsConnected) break;

					float y = client.LevelInfo.SpawnX + Random.Next(7, 10);
					float length = Random.Next(5, 20);

					double angle = 0.0;
					const double angleStepsize = 0.05;
					float heightStepSize = (float) (Random.NextDouble() / 5);

					while (angle < 2 * Math.PI && Emulator.Running)
					{
						if (!client.IsConnected) break;

						float x = (float) (length * Math.Cos(angle));
						float z = (float) (length * Math.Sin(angle));
						y += heightStepSize;

						x += client.LevelInfo.SpawnX;
						z += client.LevelInfo.SpawnZ;

						client.CurrentLocation = new PlayerLocation(x, y, z, (float) angle.ToDegrees(), (float) angle.ToDegrees());
						//client.SendMcpeMovePlayer();
						client.SendCurrentPlayerPositionAsync().Wait();

						int timeout = RanMin == RanMax ? RanMin : Random.Next(RanMin, RanMax);
						if (timeout > 0) Thread.Sleep(timeout);

						angle += angleStepsize;
					}
				}

				if (client.IsConnected)
				{
					client.SendChat("Shadow gov agent BREXITING!");
					client.SendDisconnectionNotification();
				}

				client.StopClient();
				Console.WriteLine($"{runningTime.ElapsedMilliseconds} Client stopped. {client.IsConnected}, {Emulator.Running}");
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
		}
	}
}