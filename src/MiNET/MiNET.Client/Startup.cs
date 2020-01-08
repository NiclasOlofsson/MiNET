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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2018 Niclas Olofsson. 
// All Rights Reserved.

#endregion

using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using log4net.Config;
using MiNET.Utils;

namespace MiNET.Client
{
	public class Startup
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(Startup));

		private const string MiNET = "\r\n __   __  ___   __    _  _______  _______ \r\n|  |_|  ||   | |  |  | ||       ||       |\r\n|       ||   | |   |_| ||    ___||_     _|\r\n|       ||   | |       ||   |___   |   |  \r\n|       ||   | |  _    ||    ___|  |   |  \r\n| ||_|| ||   | | | |   ||   |___   |   |  \r\n|_|   |_||___| |_|  |__||_______|  |___|  \r\n";

		static void Main(string[] args)
		{
			var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
			XmlConfigurator.Configure(logRepository, new FileInfo(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "log4net.xml")));

			Log.Info(MiNET);
			Console.WriteLine(MiNET);
			Console.WriteLine("Starting client...");

			var client = new MiNetClient(new IPEndPoint(IPAddress.Parse("192.168.0.4"), 19162), "TheGrey", new DedicatedThreadPool(new DedicatedThreadPoolSettings(Environment.ProcessorCount)));
			//var client = new MiNetClient(new IPEndPoint(IPAddress.Parse("213.89.103.206"), 19132), "TheGrey", new DedicatedThreadPool(new DedicatedThreadPoolSettings(Environment.ProcessorCount)));
			//var client = new MiNetClient(new IPEndPoint(Dns.GetHostEntry("yodamine.com").AddressList[0], 19132), "TheGrey", new DedicatedThreadPool(new DedicatedThreadPoolSettings(Environment.ProcessorCount)));
			//var client = new MiNetClient(new IPEndPoint(IPAddress.Loopback, 19132), "TheGrey", new DedicatedThreadPool(new DedicatedThreadPoolSettings(Environment.ProcessorCount)));

			client.StartClient();
			Log.Warn("Client listening for connecting on: " + client.ClientEndpoint);
			Console.WriteLine("Started!");

			//client.SendOpenConnectionRequest1();

			Console.WriteLine("Looking for server...");
			if (client.ServerEndpoint != null)
			{
				while (!client.HaveServer)
				{
					Console.WriteLine("... still looking ...");
					client.SendUnconnectedPing();
					Thread.Sleep(500);
				}
			}

			Console.WriteLine($"... YEAH! FOUND SERVER! It's at {client.ServerEndpoint?.Address}, port {client.ServerEndpoint?.Port}");

			Action<Task, PlayerLocation> doMoveTo = BotHelpers.DoMoveTo(client);
			Action<Task, string> doSendCommand = BotHelpers.DoSendCommand(client);

			Task.Run(BotHelpers.DoWaitForSpawn(client))
				.ContinueWith(t => doSendCommand(t, $"/me says \"I spawned at {client.CurrentLocation}\""))
				//.ContinueWith(t => BotHelpers.DoMobEquipment(client)(t, new ItemBlock(new Cobblestone(), 0) {Count = 64}, 0))
				//.ContinueWith(t => BotHelpers.DoMoveTo(client)(t, new PlayerLocation(client.CurrentLocation.ToVector3() - new Vector3(0, 1, 0), 180, 180, 180)))
				//.ContinueWith(t => doMoveTo(t, new PlayerLocation(40, 5.62f, -20, 180, 180, 180)))
				.ContinueWith(t => doMoveTo(t, new PlayerLocation(0, 5.62, 0, 180 + 45, 180 + 45, 180)))
				//.ContinueWith(t => doMoveTo(t, new PlayerLocation(0, 5.62, 0, 180 + 45, 180 + 45, 180)))
				//.ContinueWith(t => doMoveTo(t, new PlayerLocation(22, 5.62, 40, 180 + 45, 180 + 45, 180)))
				//.ContinueWith(t => doMoveTo(t, new PlayerLocation(50, 5.62f, 17, 180, 180, 180)))
				.ContinueWith(t => doSendCommand(t, "/me says \"Hi guys! It is I!!\""))
				//.ContinueWith(t => Task.Delay(500).Wait())
				//.ContinueWith(t => doSendCommand(t, "/summon sheep"))
				//.ContinueWith(t => Task.Delay(500).Wait())
				//.ContinueWith(t => doSendCommand(t, "/kill @e[type=sheep]"))
				.ContinueWith(t => Task.Delay(5000).Wait())
				//.ContinueWith(t =>
				//{
				//	Random rnd = new Random();
				//	while (true)
				//	{
				//		doMoveTo(t, new PlayerLocation(rnd.Next(10, 40), 5.62f, rnd.Next(-50, -10), 180, 180, 180));
				//		//doMoveTo(t, new PlayerLocation(50, 5.62f, 17, 180, 180, 180));
				//		doMoveTo(t, new PlayerLocation(rnd.Next(40, 50), 5.62f, rnd.Next(0, 20), 180, 180, 180));
				//	}
				//})
				;

			//string fileName = Path.GetTempPath() + "MobSpawns_" + Guid.NewGuid() + ".txt";
			//FileStream file = File.OpenWrite(fileName);
			//Log.Info($"Writing mob spawns to file:\n{fileName}");
			//_mobWriter = new IndentedTextWriter(new StreamWriter(file));
			//Task.Run(BotHelpers.DoWaitForSpawn(client))
			//	.ContinueWith(task =>
			//	{
			//		foreach (EntityType entityType in Enum.GetValues(typeof(EntityType)))
			//		{
			//			if (entityType == EntityType.Wither) continue;
			//			if (entityType == EntityType.Dragon) continue;
			//			if (entityType == EntityType.Slime) continue;

			//			string entityName = entityType.ToString();
			//			entityName = Regex.Replace(entityName, "([A-Z])", "_$1").TrimStart('_').ToLower();
			//			{
			//				string command = $"/summon {entityName}";
			//				McpeCommandRequest request = new McpeCommandRequest();
			//				request.command = command;
			//				request.unknownUuid = new UUID(Guid.NewGuid().ToString());
			//				client.SendPackage(request);
			//			}

			//			Task.Delay(500).Wait();

			//			{
			//				McpeCommandRequest request = new McpeCommandRequest();
			//				request.command = $"/kill @e[type={entityName}]";
			//				request.unknownUuid = new UUID(Guid.NewGuid().ToString());
			//				client.SendPackage(request);
			//			}
			//		}

			//		{
			//			McpeCommandRequest request = new McpeCommandRequest();
			//			request.command = $"/kill @e[type=!player]";
			//			request.unknownUuid = new UUID(Guid.NewGuid().ToString());
			//			client.SendPackage(request);
			//		}

			//	});

			Console.WriteLine("<Enter> to exit!");
			Console.ReadLine();
			client.SendDisconnectionNotification();
			Thread.Sleep(2000);
			client.StopClient();
		}
	}
}