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
using System.Numerics;
using System.Reflection;
using log4net;
using Microsoft.IO;
using MiNET.Commands;
using MiNET.Events;
using MiNET.Net;
using MiNET.Net.RakNet;
using MiNET.Plugins;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET
{
	public class MiNetServer
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(MiNetServer));

		public const string MiNET = "\r\n __   __  ___   __    _  _______  _______ \r\n|  |_|  ||   | |  |  | ||       ||       |\r\n|       ||   | |   |_| ||    ___||_     _|\r\n|       ||   | |       ||   |___   |   |  \r\n|       ||   | |  _    ||    ___|  |   |  \r\n| ||_|| ||   | | | |   ||   |___   |   |  \r\n|_|   |_||___| |_|  |__||_______|  |___|  \r\n";

		private const int DefaultPort = 19132;

		public IPEndPoint Endpoint { get; private set; }
		private RakConnection _listener;

		public MotdProvider MotdProvider { get; set; }

		public static RecyclableMemoryStreamManager MemoryStreamManager { get; set; } = new RecyclableMemoryStreamManager();

		public IServerManager ServerManager { get; set; }
		public LevelManager LevelManager { get; set; }
		public PlayerFactory PlayerFactory { get; set; }
		public GreyListManager GreyListManager { get; set; }

		public bool IsEdu { get; set; } = Config.GetProperty("EnableEdu", false);
		public EduTokenManager EduTokenManager { get; set; }

		public CommandManager  CommandManager  { get; private set; }
		public EventDispatcher EventDispatcher { get; set; }
		public PluginManager   PluginManager   { get; set; }
		public SessionManager  SessionManager  { get; set; }

		public ConnectionInfo ConnectionInfo { get; set; }

		public ServerRole ServerRole { get; set; }

		internal static DedicatedThreadPool FastThreadPool { get; set; } = new DedicatedThreadPool(new DedicatedThreadPoolSettings(100, "Fast_Thread"));

		public MiNetServer()
		{
			ServerRole = Config.GetProperty("ServerRole", ServerRole.Full);
		}

		public MiNetServer(IPEndPoint endpoint) : this()
		{
			Endpoint = endpoint;
		}

		public static void DisplayTimerProperties()
		{
			Console.WriteLine($"Are you blessed with HW accelerated vectors? {(Vector.IsHardwareAccelerated ? "Yep!" : "Nope, sorry :-(")}");

			// Display the timer frequency and resolution.
			if (Stopwatch.IsHighResolution)
			{
				Console.WriteLine("Operations timed using the system's high-resolution performance counter.");
			}
			else
			{
				Console.WriteLine("Operations timed using the DateTime class.");
			}

			long frequency = Stopwatch.Frequency;
			Console.WriteLine("  Timer frequency in ticks per second = {0}",
				frequency);
			long nanosecPerTick = (1000L * 1000L * 1000L) / frequency;
			Console.WriteLine("  Timer is accurate within {0} nanoseconds",
				nanosecPerTick);
		}

		public bool StartServer()
		{
			DisplayTimerProperties();

			if (_listener != null) return false; // Already started

			try
			{
				Log.Info("Initializing...");

				if (ServerRole == ServerRole.Full || ServerRole == ServerRole.Proxy)
				{
					if (IsEdu) EduTokenManager = new EduTokenManager();

					if (Endpoint == null)
					{
						var ip = IPAddress.Parse(Config.GetProperty("ip", "0.0.0.0"));
						int port = Config.GetProperty("port", DefaultPort);
						Endpoint = new IPEndPoint(ip, port);
					}
				}

				ServerManager ??= new DefaultServerManager(this);
				EventDispatcher ??= new EventDispatcher(this);
				CommandManager ??= new CommandManager(PluginManager);
				
				if (ServerRole == ServerRole.Full || ServerRole == ServerRole.Node)
				{
					// This stuff needs to be in an extension to connection
					// somehow ...

					Log.Info("Loading plugins...");
					
					string pluginDirectoryPaths =
						Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
					pluginDirectoryPaths = Config.GetProperty("PluginDirectory", pluginDirectoryPaths);
					
					PluginManager = new PluginManager(this);
					PluginManager.DiscoverPlugins(pluginDirectoryPaths.Split(new char[] {';'},
						StringSplitOptions.RemoveEmptyEntries));
					
					Log.Info("Plugins loaded!");

					// Bootstrap server
					PluginManager.ExecuteStartup(this);

					SessionManager ??= new SessionManager();
					LevelManager ??= new LevelManager();
					//LevelManager ??= new SpreadLevelManager(50);
					PlayerFactory ??= new PlayerFactory();

					//PluginManager.EnablePlugins(this, LevelManager);

					PluginManager.EnablePlugins();
					
					// Cache - remove
					LevelManager.GetLevel(null, Dimension.Overworld.ToString());
				}

				GreyListManager ??= new GreyListManager();
				MotdProvider ??= new MotdProvider();

				if (ServerRole == ServerRole.Full || ServerRole == ServerRole.Proxy)
				{
					_listener = new RakConnection(Endpoint, GreyListManager, MotdProvider);
					//_listener.ServerInfo.DisableAck = true;
					_listener.CustomMessageHandlerFactory = session => new BedrockMessageHandler(session, ServerManager, PluginManager);

					//TODO: This is bad design, need to refactor this later.
					GreyListManager.ConnectionInfo = _listener.ConnectionInfo;
					ConnectionInfo = _listener.ConnectionInfo;
					ConnectionInfo.MaxNumberOfPlayers = Config.GetProperty("MaxNumberOfPlayers", 10);
					ConnectionInfo.MaxNumberOfConcurrentConnects = Config.GetProperty("MaxNumberOfConcurrentConnects", ConnectionInfo.MaxNumberOfPlayers);

					_listener.Start();
				}

				Log.Info("Server open for business on port " + Endpoint?.Port + " ...");

				return true;
			}
			catch (Exception e)
			{
				Log.Error("Error during startup!", e);
				_listener.Stop();
			}

			return false;
		}

		public void StopServer()
		{
			foreach (Level level in LevelManager.Levels)
			{
				try
				{
					level.Close();
				}
				catch (Exception e)
				{
					Log.Warn($"Error while stopping server", e);
				}
			}

			Log.Info("Disabling plugins...");
			PluginManager?.UnloadAll();
			_listener?.Stop();
		}
	}

	public enum ServerRole
	{
		Node,
		Proxy,
		Full,
	}
}