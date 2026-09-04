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
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Reflection;
using System.Text.RegularExpressions;
using log4net;
using Microsoft.IO;
using MiNET.Crafting;
using MiNET.Items;
using MiNET.Net;
using MiNET.Net.NetherNet;
using MiNET.Plugins;
using MiNET.Utils;
using MiNET.Utils.Cryptography;
using MiNET.Utils.Diagnostics;
using MiNET.Utils.IO;
using MiNET.Worlds;

namespace MiNET
{
	public class MiNetServer
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(MiNetServer));

		public const string MiNET = "\r\n __   __  ___   __    _  _______  _______ \r\n|  |_|  ||   | |  |  | ||       ||       |\r\n|       ||   | |   |_| ||    ___||_     _|\r\n|       ||   | |       ||   |___   |   |  \r\n|       ||   | |  _    ||    ___|  |   |  \r\n| ||_|| ||   | | | |   ||   |___   |   |  \r\n|_|   |_||___| |_|  |__||_______|  |___|  \r\n";

		private const int DefaultPort = 19132;

		public IPEndPoint Endpoint { get; private set; }
		private NetherNetListener _netherNetListener;
		private AcmeCertificateManager _acmeCertificateManager;

		public MotdProvider MotdProvider { get; set; }

		// Wrapper payloads lease these buffers for as long as the wrapper is in flight, so blocks
		// are sized for typical batches and the free pools are capped: an unbounded default pool
		// retains its high-water mark forever.
		public static RecyclableMemoryStreamManager MemoryStreamManager { get; set; } = new RecyclableMemoryStreamManager(new RecyclableMemoryStreamManager.Options
		{
			BlockSize = 16 * 1024,
			LargeBufferMultiple = 256 * 1024,
			MaximumBufferSize = 32 * 1024 * 1024,
			MaximumSmallPoolFreeBytes = 64 * 1024 * 1024,
			MaximumLargePoolFreeBytes = 128 * 1024 * 1024,
		});

		public IServerManager ServerManager { get; set; }
		public LevelManager LevelManager { get; set; }
		public PlayerFactory PlayerFactory { get; set; }

		public bool IsEdu { get; set; } = Config.GetProperty("EnableEdu", false);
		public EduTokenManager EduTokenManager { get; set; }

		public PluginManager PluginManager { get; set; }
		public SessionManager SessionManager { get; set; }

		public ConnectionInfo ConnectionInfo { get; set; }

		/// <summary>
		///     Whether new connections are answered. Clearing it leaves established sessions running
		///     and the socket open, but a client that tries to join gets no reply at all, so it
		///     concludes the server is down. Closing this before moving players off is what stops a
		///     reconnect racing the shutdown.
		/// </summary>
		public bool AcceptConnections
		{
			get => _netherNetListener?.AcceptConnections ?? false;
			set
			{
				if (_netherNetListener != null) _netherNetListener.AcceptConnections = value;
			}
		}

		/// <summary>Live transport sessions right now, read straight off the listener (ConnectionInfo's count refreshes on a timer and can lag by a second).</summary>
		public int LiveSessionCount => _netherNetListener?.Sessions.Count ?? 0;

		/// <summary>The transport, for a plugin that needs to open its own signaling ports. Null until the server starts.</summary>
		public NetherNetListener NetherNetListener => _netherNetListener;

		/// <summary>Raised once the server is listening and <see cref="NetherNetListener" /> exists.</summary>
		public event EventHandler ServerStarted;

		public ServerRole ServerRole { get; set; }

		internal static DedicatedThreadPool FastThreadPool { get; set; }

		static MiNetServer()
		{
			
		}
		
		public MiNetServer()
		{
			ServerRole = Config.GetProperty("ServerRole", ServerRole.Full);
			FastThreadPool?.Dispose();
			FastThreadPool = new DedicatedThreadPool(new DedicatedThreadPoolSettings(Config.GetProperty("FastThreads", 100), "Fast_Thread"));
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

			if (_netherNetListener != null) return false; // Already started

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

				if (ServerRole == ServerRole.Full || ServerRole == ServerRole.Node)
				{
					// This stuff needs to be in an extension to connection
					// somehow ...

					Log.Info("Loading plugins...");
					PluginManager = new PluginManager();
					PluginManager.LoadPlugins();
					Log.Info("Plugins loaded!");

					// Bootstrap server
					PluginManager.ExecuteStartup(this);

					SessionManager ??= new SessionManager();
					LevelManager ??= new LevelManager();
					//LevelManager ??= new SpreadLevelManager(50);
					PlayerFactory ??= new PlayerFactory();

					PluginManager.EnablePlugins(this, LevelManager);

					// Load the recipe registry here, after plugins have had their say about it, so the
					// first player to join doesn't pay for it on the login thread (resolving thousands of
					// recipes takes about a second).
					Log.Info($"Loaded {RecipeManager.Recipes.Count} recipes");

					// Label every handler method now that the closed world is final (plugins loaded):
					// verified handlers may dispatch without the queue hop, everything else keeps the
					// queue, and the warnings this prints are the cleanup worklist. Runs once, ~200ms.
					var handlerAssemblies = AppDomain.CurrentDomain.GetAssemblies()
						.Where(a => !a.IsDynamic && !string.IsNullOrEmpty(a.Location))
						.Where(a =>
						{
							string name = a.GetName().Name ?? "";
							return !name.StartsWith("System") && !name.StartsWith("Microsoft") && name != "mscorlib" && name != "netstandard";
						})
						.ToList();
					var activeHandlerTypes = handlerAssemblies
						.SelectMany(a =>
						{
							try { return a.GetTypes(); }
							catch (ReflectionTypeLoadException e) { return e.Types.Where(t => t != null); }
						})
						.Where(t => typeof(Player).IsAssignableFrom(t) || typeof(LoginMessageHandler).IsAssignableFrom(t))
						.ToArray();
					HandlerVerification.ScanAndReport(handlerAssemblies, activeHandlerTypes);

					// Cache - remove
					LevelManager.GetLevel(null, Dimension.Overworld.ToString());
				}

				MotdProvider ??= new MotdProvider();
				if (Endpoint != null)
				{
					MotdProvider.PortV4 = Endpoint.Port;

					// Both the same, because one socket serves both families and nothing is bound on
					// port + 1. Advertising a port we do not listen on is what the BDS convention
					// would have us do, and the client appears to list a row per address it is
					// offered.
					MotdProvider.PortV6 = Endpoint.Port;
				}

				if (ServerRole == ServerRole.Full || ServerRole == ServerRole.Proxy)
				{
					_netherNetListener = new NetherNetListener(Endpoint);
					_netherNetListener.CustomMessageHandlerFactory = session => new BedrockMessageHandler(session, ServerManager, PluginManager);

					// Plugins serve the server port for anything NetherNet does not claim itself.
					_netherNetListener.RequestHandler = PluginManager.HandleHttpRequest;

					NetherNetListener listener = _netherNetListener;
					ConnectionInfo = new ConnectionInfo(() => listener.Sessions.Count);

					// The 1.26.50 client's server-list ping is GET /v1/join over the signaling TCP
					// port; the JSON body is the MOTD now.
					MotdProvider motdProvider = MotdProvider;
					_netherNetListener.JoinStatusProvider = () => motdProvider.GetJoinStatus(ConnectionInfo, listener.Sessions.Count);

					// The same live count the console line reads, handed to the meter so
					// transport.sessions.active is the denominator for every per-session rate a
					// collector computes. The two queue depths walk the same table; both run on the
					// collector's scrape thread, once an interval, never on a transport thread.
					TransportMetrics.SessionCountProvider = () => listener.Sessions.Count;
					TransportMetrics.SendQueueDepthProvider = () =>
					{
						long depth = 0;
						foreach (NetherNetSession session in listener.Sessions.Values) depth += session.SendQueueDepth;
						return depth;
					};
					TransportMetrics.DispatchQueueDepthProvider = () =>
					{
						long depth = 0;
						foreach (NetherNetSession session in listener.Sessions.Values) depth += session.DispatchQueueDepth;
						return depth;
					};
					ConnectionInfo.MaxNumberOfPlayers = Config.GetProperty("MaxNumberOfPlayers", 10);
					ConnectionInfo.MaxNumberOfConcurrentConnects = Config.GetProperty("MaxNumberOfConcurrentConnects", ConnectionInfo.MaxNumberOfPlayers);

					// The mux answers RakNet's legacy unconnected ping on the gameplay UDP port so
					// the server still shows in the client's server tab; Mojang shipped no NetherNet
					// replacement for it. EnableDiscovery=false turns the responder off entirely.
					if (Config.GetProperty("EnableDiscovery", true))
					{
						_netherNetListener.Discovery = new NetherNetDiscovery(MotdProvider, ConnectionInfo, () => listener.Sessions.Count);
					}

					_netherNetListener.Start();

					// TLS for the signaling port, default OFF: SignalingTls.Enabled gates the whole
					// ACME machinery, so a stock install never dials a certificate authority
					// whatever else its config carries. Enabled, the manager issues and renews a
					// Let's Encrypt certificate for SignalingDomain through the listener's own
					// challenge route, and the listener answers a client's TLS offer with it
					// instead of refusing into the plaintext fallback. Started after the listener,
					// because the manager's first act is to dial its own responder.
					if (Config.GetProperty("SignalingTls.Enabled", false))
					{
						string signalingDomain = Config.GetProperty("SignalingDomain", null);
						if (string.IsNullOrWhiteSpace(signalingDomain))
						{
							Log.Warn("SignalingTls.Enabled is set but SignalingDomain is empty; signaling TLS stays off");
						}
						else
						{
							_acmeCertificateManager = new AcmeCertificateManager(
								signalingDomain.Trim(),
								Config.GetProperty("SignalingCertificateDirectory", "certificates"),
								Config.GetProperty("AcmeContactEmail", null),
								Config.GetProperty("AcmeStaging", false));
							_netherNetListener.TlsCertificateProvider = _acmeCertificateManager.GetCertificateContext;
							_netherNetListener.AcmeChallengeHandler = _acmeCertificateManager.GetChallengeResponse;
							_acmeCertificateManager.Start();
						}
					}
				}

				Log.Info("Server open for business on port " + Endpoint?.Port + " ...");

				// After the transport exists, which is the whole point: plugins are enabled long
				// before this, and LevelCreated fires earlier still, so anything needing the
				// listener had nowhere to hook until here.
				ServerStarted?.Invoke(this, EventArgs.Empty);

				return true;
			}
			catch (Exception e)
			{
				Log.Error("Error during startup!", e);
				_netherNetListener?.Stop();
			}

			return false;
		}

		public void StopServer()
		{
			Log.Info($"Stopping...");
			LevelManager.Close();
			
			Log.Info("Disabling plugins...");
			PluginManager?.DisablePlugins();
			
			_acmeCertificateManager?.Stop();
			_netherNetListener?.Stop();
			ConnectionInfo?.Stop();

			var fastThreadPool = FastThreadPool;
			fastThreadPool?.Dispose();
			
			Log.Info($"Waiting for threads to exit...");

			// Bounded, not infinite: a pool worker that never exits (an observed, unexplained
			// hang) would otherwise wedge the process forever AFTER the level is already saved,
			// turning every restart into a manual kill. Ten seconds is grace, not correctness;
			// everything that matters has already been flushed above.
			fastThreadPool?.WaitForThreadsExit(TimeSpan.FromSeconds(10));
		}
	}

	public enum ServerRole
	{
		Node,
		Proxy,
		Full,
	}
}