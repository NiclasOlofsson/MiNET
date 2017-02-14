using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using log4net;
using Microsoft.AspNet.Identity;
using Microsoft.IO;
using MiNET.Net;
using MiNET.Plugins;
using MiNET.Security;
using MiNET.Utils;
using Newtonsoft.Json;

namespace MiNET
{
	public class MiNetServer
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (MiNetServer));

		private const int DefaultPort = 19132;

		public IPEndPoint Endpoint { get; private set; }
        private IO.IOClient _io;
        private IO.QueryHandler _queryHandler;
        private IO.ClientConnectionHandler _clientConnectionHandler;
        private IO.DatagramProcessor _datagramProcessor;
        private IO.PackageAssembler _packageAssembler;

        private ConcurrentDictionary<IPEndPoint, PlayerNetworkSession> _playerSessions = new ConcurrentDictionary<IPEndPoint, PlayerNetworkSession>();

		public MotdProvider MotdProvider { get; set; }

		public bool IsSecurityEnabled { get; private set; }
		public UserManager<User> UserManager { get; set; }
		public RoleManager<Role> RoleManager { get; set; }

		public static RecyclableMemoryStreamManager MemoryStreamManager { get; set; } = new RecyclableMemoryStreamManager();

		public IServerManager ServerManager { get; set; }
		public LevelManager LevelManager { get; set; }
		public PlayerFactory PlayerFactory { get; set; }
		public GreylistManager GreylistManager { get; set; }

		public PluginManager PluginManager { get; set; }
		public SessionManager SessionManager { get; set; }

		private Timer _internalPingTimer;
		private Timer _cleanerTimer;

		public int InacvitityTimeout { get; private set; }

		public ServerInfo ServerInfo { get; set; }

		public ServerRole ServerRole { get; set; } = ServerRole.Full;

		public bool ForceOrderingForAll { get; set; }

		internal static DedicatedThreadPool FastThreadPool { get; set; }
		internal static DedicatedThreadPool LevelThreadPool { get; set; }

		public MiNetServer()
		{
			ServerRole = Config.GetProperty("ServerRole", ServerRole.Full);
			InacvitityTimeout = Config.GetProperty("InactivityTimeout", 8500);
			ForceOrderingForAll = Config.GetProperty("ForceOrderingForAll", false);

			int confMinWorkerThreads = Config.GetProperty("MinWorkerThreads", -1);
			int confMinCompletionPortThreads = Config.GetProperty("MinCompletionPortThreads", -1);

            int threads;
			int iothreads;
			ThreadPool.GetMinThreads(out threads, out iothreads);

			//if (confMinWorkerThreads != -1) threads = confMinWorkerThreads;
			//else threads *= 4;

			//if (confMinCompletionPortThreads != -1) iothreads = confMinCompletionPortThreads;
			//else iothreads *= 4;

			//ThreadPool.SetMinThreads(threads, iothreads);
			FastThreadPool = new DedicatedThreadPool(new DedicatedThreadPoolSettings(Environment.ProcessorCount));
			LevelThreadPool = new DedicatedThreadPool(new DedicatedThreadPoolSettings(Environment.ProcessorCount));
		}

		public MiNetServer(IPEndPoint endpoint) : base()
		{
			Endpoint = endpoint;
		}

		public static bool IsRunningOnMono()
		{
			return Type.GetType("Mono.Runtime") != null;
		}

		public static void DisplayTimerProperties()
		{
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
			long nanosecPerTick = (1000L*1000L*1000L)/frequency;
			Console.WriteLine("  Timer is accurate within {0} nanoseconds",
				nanosecPerTick);
		}

		public bool StartServer()
		{
			DisplayTimerProperties();

			if (_io != null) return false; // Already started

			try
			{
				Log.Info("Initializing...");

				if (ServerRole == ServerRole.Full || ServerRole == ServerRole.Proxy)
				{
					if (Endpoint == null)
					{
						var ip = IPAddress.Parse(Config.GetProperty("ip", "0.0.0.0"));
						int port = Config.GetProperty("port", 19132);
						Endpoint = new IPEndPoint(ip, port);
					}
				}

				ServerManager = ServerManager ?? new DefualtServerManager(this);

				if (ServerRole == ServerRole.Full || ServerRole == ServerRole.Node)
				{
					Log.Info("Loading plugins...");
					PluginManager = new PluginManager();
					PluginManager.LoadPlugins();
					Log.Info("Plugins loaded!");

					// Bootstrap server
					PluginManager.ExecuteStartup(this);

					GreylistManager = GreylistManager ?? new GreylistManager(this);
					SessionManager = SessionManager ?? new SessionManager();
					LevelManager = LevelManager ?? new LevelManager();
					PlayerFactory = PlayerFactory ?? new PlayerFactory();

					PluginManager.EnablePlugins(this, LevelManager);

					// Cache - remove
					LevelManager.GetLevel(null, "Default");
				}

				GreylistManager = GreylistManager ?? new GreylistManager(this);
				MotdProvider = MotdProvider ?? new MotdProvider();

                ServerInfo = new ServerInfo(LevelManager, _playerSessions)
                {
                    MaxNumberOfPlayers = Config.GetProperty("MaxNumberOfPlayers", 1000)
                };
                ServerInfo.MaxNumberOfConcurrentConnects = Config.GetProperty("MaxNumberOfConcurrentConnects", ServerInfo.MaxNumberOfPlayers);

                if (ServerRole == ServerRole.Full || ServerRole == ServerRole.Proxy)
                {
                    createIoAndHandlers();
                }

                Log.Info("Server open for business on port " + Endpoint?.Port + " ...");

				return true;
			}
			catch (Exception e)
			{
				Log.Error("Error during startup!", e);
				StopServer();
			}

			return false;
		}

        private void createIoAndHandlers()
        {
            _io = IsRunningOnMono()
                ? new IO.IOClient_Mono(this.Endpoint)
                : new IO.IOClient(this.Endpoint);

            _io.ServerInfo = ServerInfo;
            _io.ReceivedData += this.OnReceivedData;

            //_cleanerTimer = new Timer(Update, null, 10, Timeout.Infinite);

            _queryHandler = Config.GetProperty("EnableQuery", false)
                ? new IO.QueryHandler(this._io, this.MotdProvider)
                : new IO.QueryHandler_NoOp();

            _clientConnectionHandler = new IO.ClientConnectionHandler(this._io, this.MotdProvider, this.GreylistManager);
            _clientConnectionHandler.ClientConnecting += OnClientConnecting;

            _datagramProcessor = new IO.DatagramProcessor(this._io, this._playerSessions, this.GreylistManager);
            _datagramProcessor.ConnectedPackageReceived += OnConnectedPackageReceived;

            _packageAssembler = new IO.PackageAssembler();
            _packageAssembler.PackageAssembled += OnPackageAssembled;

        }

        private void OnReceivedData(object sender, IO.ReceivedDataEventArgs e)
        {
            if (!GreylistManager.IsWhitelisted(e.Sender.Address) && GreylistManager.IsBlacklisted(e.Sender.Address))
                e.Cancel = true;
            else if (GreylistManager.IsGreylisted(e.Sender.Address))
                e.Cancel = true;
            else
            {
                byte msgId = e.Data[0];
                if (msgId == 0xFE)
                {
                    Log.InfoFormat("A query detected from: {0}", e.Sender.Address);
                    _queryHandler.Process(e.Sender, msgId, e.Data);
                }
                else if (msgId <= (byte)DefaultMessageIdTypes.ID_USER_PACKET_ENUM)
                {
                    _clientConnectionHandler.Process(e.Sender, msgId, e.Data);
                }
                else
                {
                    _datagramProcessor.Process(e.Sender, msgId, e.Data);
                }
            }
        }

        private void OnClientConnecting(object sender, IO.ClientConnectingEventArgs e)
        {
            PlayerNetworkSession session;

            if (_playerSessions.TryGetValue(e.Sender, out session))
            {
                // Already connecting, then this is just a duplicate
                if (session.State == ConnectionState.Connecting /* && DateTime.UtcNow < session.LastUpdatedTime + TimeSpan.FromSeconds(2)*/)
                {
                    e.Cancel = true;
                    return;
                }

                Log.InfoFormat("Unexpected session from {0}. Removing old session and disconnecting old player.", e.Sender.Address);

                session.Disconnect("Reconnecting.", false);

                _playerSessions.TryRemove(e.Sender, out session);
            }

            session = new PlayerNetworkSession(this, null, e.Sender, e.Request.mtuSize)
            {
                State = ConnectionState.Connecting,
                LastUpdatedTime = DateTime.UtcNow,
                MtuSize = e.Request.mtuSize
            };

            _playerSessions.TryAdd(e.Sender, session);

            //Player player = PlayerFactory.CreatePlayer(this, senderEndpoint);
            //player.ClientGuid = e.Request.clientGuid;
            //player.NetworkHandler = session;
            //session.Player = player;
            session.MessageHandler = new LoginMessageHandler(session);
        }

        private void OnConnectedPackageReceived(object sender, IO.ConnectedPackageReceivedEventArgs e)
        {
            _packageAssembler.Process(e.Session, e.Package);
        }

        private void OnPackageAssembled(object sender, IO.PackageAssembledEventArgs e)
        {
            if (e.Package.Reliability != Reliability.ReliableOrdered)
            {
                e.Session.HandlePackage(e.Package, e.Session);
            }
            else
            {
                if (ForceOrderingForAll == false && (e.Session.CryptoContext == null || e.Session.CryptoContext.UseEncryption == false))
                {
                    e.Session.AddToProcessing(e.Package);
                }
                else
                {
                    FastThreadPool.QueueUserWorkItem(() => e.Session.AddToProcessing(e.Package));
                }
            }
        }

        public bool StopServer()
		{
			try
			{
				Log.Info("Disabling plugins...");
				PluginManager.DisablePlugins();

				Log.Info("Shutting down...");
				if (_io == null) return true; // Already stopped. It's ok.

                _io.Close();
				return true;
			}
			catch (Exception e)
			{
				Log.Error(e);
			}

			return false;
		}

        internal void SendDatagram(PlayerNetworkSession session, Datagram datagram)
        {
            this._datagramProcessor.Send(session, datagram);
        }

		public void SendPackage(PlayerNetworkSession session, Package message)
		{
            var datagrams = Datagram.CreateDatagrams(message, session.MtuSize, session);
            this._datagramProcessor.Send(session, datagrams);

			message.PutPool();
		}



		internal void SendData(byte[] data, IPEndPoint targetEndPoint)
		{
            this._io.Send(data, targetEndPoint);
		}

		internal static void TraceReceive(Package message)
		{
			if (!Log.IsDebugEnabled) return;

			string typeName = message.GetType().Name;

			string includePattern = Config.GetProperty("TracePackets.Include", ".*");
			string excludePattern = Config.GetProperty("TracePackets.Exclude", null);
			int verbosity = Config.GetProperty("TracePackets.Verbosity", 0);
			verbosity = Config.GetProperty($"TracePackets.Verbosity.{typeName}", verbosity);

			if (!Regex.IsMatch(typeName, includePattern))
			{
				return;
			}

			if (!string.IsNullOrWhiteSpace(excludePattern) && Regex.IsMatch(typeName, excludePattern))
			{
				return;
			}

			if (verbosity == 0)
			{
				Log.Debug($"> Receive: {message.Id} (0x{message.Id:x2}): {message.GetType().Name}");
			}
			else if (verbosity == 1)
			{
				var jsonSerializerSettings = new JsonSerializerSettings
				{
					PreserveReferencesHandling = PreserveReferencesHandling.None,
					Formatting = Formatting.Indented,
				};
				string result = JsonConvert.SerializeObject(message, jsonSerializerSettings);
				Log.Debug($"> Receive: {message.Id} (0x{message.Id:x2}): {message.GetType().Name}\n{result}");
			}
			else if (verbosity == 2)
			{
				Log.Debug($"> Receive: {message.Id} (0x{message.Id:x2}): {message.GetType().Name}\n{Package.HexDump(message.Bytes)}");
			}
		}

		public static void TraceSend(Package message)
		{
			if (!Log.IsDebugEnabled) return;
			//if (!Debugger.IsAttached) return;

			Log.DebugFormat("<    Send: {0}: {1} (0x{0:x2})", message.Id, message.GetType().Name);
		}
	}

	public enum ServerRole
	{
		Node,
		Proxy,
		Full,
	}
}