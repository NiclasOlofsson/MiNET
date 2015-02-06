using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using MiNET.Net;
using MiNET.PluginSystem;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET
{
	public class PlayerNetworkSession
	{
		public Player Player { get; set; }
		public IPEndPoint EndPoint { get; set; }
		private ConcurrentQueue<Ack> _playerAckQueue = new ConcurrentQueue<Ack>();
		private ConcurrentQueue<Package> _playerWaitingForAcksQueue = new ConcurrentQueue<Package>();

		public ConcurrentQueue<Ack> PlayerAckQueue
		{
			get { return _playerAckQueue; }
		}

		public ConcurrentQueue<Package> PlayerWaitingForAcksQueue
		{
			get { return _playerWaitingForAcksQueue; }
		}

		public PlayerNetworkSession(Player player, IPEndPoint endPoint)
		{
			Player = player;
			EndPoint = endPoint;
		}
	}

	public class MiNetServer
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (MiNetServer));

		private const int DefaultPort = 19132;

		private IPEndPoint _endpoint;
		private UdpClient _listener;
		private Dictionary<IPEndPoint, PlayerNetworkSession> _playerSessions = new Dictionary<IPEndPoint, PlayerNetworkSession>();
		private Level _level;
		private Level _level2;
		private PluginLoader _pluginLoader;
		private Timer _internalPingTimer;
		private Random _random = new Random();
		private string _motd = string.Empty;

		/// <summary>
		///     Initializes a new instance of the <see cref="MiNetServer" /> class.
		/// </summary>
		public MiNetServer() : this(new IPEndPoint(IPAddress.Any, DefaultPort))
		{
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="MiNetServer" /> class.
		/// </summary>
		/// <param name="port">The port.</param>
		public MiNetServer(int port) : this(new IPEndPoint(IPAddress.Any, port))
		{
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="MiNetServer" /> class.
		/// </summary>
		/// <param name="endpoint">The endpoint.</param>
		public MiNetServer(IPEndPoint endpoint)
		{
			_endpoint = new IPEndPoint(IPAddress.Any, DefaultPort);
		}

		/// <summary>
		///     Determines whether is running on mono.
		/// </summary>
		/// <returns></returns>
		public static bool IsRunningOnMono()
		{
			return Type.GetType("Mono.Runtime") != null;
		}


		private List<Level> _levels = new List<Level>();

		/// <summary>
		///     Starts the server.
		/// </summary>
		/// <returns></returns>
		public bool StartServer()
		{
			if (_listener != null) return false; // Already started

			try
			{
				Log.Info("Loading settings...");
				_motd = ConfigParser.GetProperty("motd", "MiNET - Another MC server");
				Log.Info("Loading plugins...");
				_pluginLoader = new PluginLoader();
				_pluginLoader.LoadPlugins();
				_pluginLoader.EnablePlugins();
				Log.Info("Plugins loaded!");

				_level = new Level("Default");
				_level.Initialize();
				_levels.Add(_level);
				//for (int i = 1; i < 4; i++)
				//{
				//	Level level = new Level("" + i);
				//	level.Initialize();
				//	_levels.Add(level);
				//}

				_listener = new UdpClient(_endpoint);

				if (IsRunningOnMono())
				{
					_listener.Client.ReceiveBufferSize = 1024*1024*3;
					_listener.Client.SendBufferSize = 4096;
				}
				else
				{
					_listener.Client.ReceiveBufferSize = int.MaxValue;
					_listener.Client.SendBufferSize = 1024*1024*8;
					//_listener.DontFragment = true;

					// SIO_UDP_CONNRESET (opcode setting: I, T==3)
					// Windows:  Controls whether UDP PORT_UNREACHABLE messages are reported.
					// - Set to TRUE to enable reporting.
					// - Set to FALSE to disable reporting.

					uint IOC_IN = 0x80000000;
					uint IOC_VENDOR = 0x18000000;
					uint SIO_UDP_CONNRESET = IOC_IN | IOC_VENDOR | 12;
					_listener.Client.IOControl((int) SIO_UDP_CONNRESET, new byte[] {Convert.ToByte(false)}, null);

					//
					//WARNING: We need to catch errors here to remove the code above.
					//
				}

				_listener.BeginReceive(ReceiveCallback, _listener);

				// Measure latency through system
				_internalPingTimer = new Timer(delegate(object state)
				{
					IPEndPoint playerEndpoint = _playerSessions.Keys.FirstOrDefault();
					if (playerEndpoint != null)
					{
						var ping = new InternalPing();
						ping.Timer.Start();
						HandlePackage(ping, playerEndpoint);
					}
				}, null, 1000, 1000);

				Log.Info("Server open for business...");

				return true;
			}
			catch (Exception e)
			{
				Log.Error(e);
				StopServer();
			}

			return false;
		}

		/// <summary>
		///     Stops the server.
		/// </summary>
		/// <returns></returns>
		public bool StopServer()
		{
			try
			{
				if (ConfigParser.GetProperty("save_pe", true))
				{
					Log.Info("Saving chunks...");
					_level._worldProvider.SaveChunks();
				}
				if (ConfigParser.GetProperty("save_playerdata", true))
				{
					Log.Info("Saving player data...");
					foreach (Player a in _level.Players)
					{
						a.SavePlayerData();
					}
				}
				Log.Info("Disabling plugins...");
				_pluginLoader.DisablePlugins();

				Log.Info("Shutting down...");
				if (_listener == null) return true; // Already stopped. It's ok.

				_listener.Close();
				_listener = null;

				return true;
			}
			catch (Exception e)
			{
				Log.Error(e);
			}

			return false;
		}

		/// <summary>
		///     Handles the callback.
		/// </summary>
		/// <param name="ar">The results</param>
		private void ReceiveCallback(IAsyncResult ar)
		{
			UdpClient listener = (UdpClient) ar.AsyncState;

			// WSAECONNRESET:
			// The virtual circuit was reset by the remote side executing a hard or abortive close. 
			// The application should close the socket; it is no longer usable. On a UDP-datagram socket 
			// this error indicates a previous send operation resulted in an ICMP Port Unreachable message.
			// Note the spocket settings on creation of the server. It makes us ignore these resets.
			IPEndPoint senderEndpoint = new IPEndPoint(0, 0);
			Byte[] receiveBytes = null;
			try
			{
				receiveBytes = listener.EndReceive(ar, ref senderEndpoint);
			}
			catch (Exception e)
			{
				Log.Warn(e);
				try
				{
					listener.BeginReceive(ReceiveCallback, listener);
				}
				catch (ObjectDisposedException dex)
				{
					// Log and move on. Should probably free up the player and remove them here.
					Log.Warn(dex);
				}

				return;
			}

			if (receiveBytes.Length != 0)
			{
				listener.BeginReceive(ReceiveCallback, listener);
				_availableBytes = listener.Available;
				_numberOfPacketsInPerSecond++;
				_totalPacketSizeIn += receiveBytes.Length;
				ThreadPool.QueueUserWorkItem(state => ProcessMessage(receiveBytes, senderEndpoint));
				//ProcessMessage(receiveBytes, senderEndpoint);
			}
			else
			{
				Log.Debug("Unexpected end of transmission?");
			}
		}

		/// <summary>
		///     Processes a message.
		/// </summary>
		/// <param name="receiveBytes">The received bytes.</param>
		/// <param name="senderEndpoint">The sender's endpoint.</param>
		/// <exception cref="System.Exception">Receive ERROR, NAK in wrong place</exception>
		private void ProcessMessage(byte[] receiveBytes, IPEndPoint senderEndpoint)
		{
			//Thread.CurrentThread.IsBackground = false;
			//Thread.CurrentThread.Priority = ThreadPriority.Highest;

			byte msgId = receiveBytes[0];

			if (msgId <= (byte) DefaultMessageIdTypes.ID_USER_PACKET_ENUM)
			{
				DefaultMessageIdTypes msgIdType = (DefaultMessageIdTypes) msgId;

				Package message = PackageFactory.CreatePackage(msgId, receiveBytes);

				TraceReceive(message);

				switch (msgIdType)
				{
					case DefaultMessageIdTypes.ID_UNCONNECTED_PING:
					case DefaultMessageIdTypes.ID_UNCONNECTED_PING_OPEN_CONNECTIONS:
					{
						UnconnectedPing incoming = (UnconnectedPing) message;

						//TODO: This needs to be verified with RakNet first
						//response.sendpingtime = msg.sendpingtime;
						//response.sendpongtime = DateTimeOffset.UtcNow.Ticks / TimeSpan.TicksPerMillisecond;

						var packet = new UnconnectedPong
						{
							serverId = 12345,
							pingId = 100 /*incoming.pingId*/,
							serverName = "MCCPP;Demo;MiNET - Another MC server"
						};
						var data = packet.Encode();
						TraceSend(packet);
						SendData(data, senderEndpoint);
						break;
					}
					case DefaultMessageIdTypes.ID_OPEN_CONNECTION_REQUEST_1:
					{
						OpenConnectionRequest1 incoming = (OpenConnectionRequest1) message;

						_performanceTest = _performanceTest || incoming.raknetProtocolVersion == byte.MaxValue;

						var packet = new OpenConnectionReply1
						{
							serverGuid = 12345,
							mtuSize = incoming.mtuSize,
							serverHasSecurity = 0
						};

						var data = packet.Encode();
						TraceSend(packet);
						SendData(data, senderEndpoint);
						break;
					}
					case DefaultMessageIdTypes.ID_OPEN_CONNECTION_REQUEST_2:
					{
						OpenConnectionRequest2 incoming = (OpenConnectionRequest2) message;

						var packet = new OpenConnectionReply2
						{
							serverGuid = 12345,
							mtuSize = incoming.mtuSize,
							doSecurityAndHandshake = new byte[0]
						};

						lock (_playerSessions)
						{
							if (_playerSessions.ContainsKey(senderEndpoint))
							{
								Log.Info("Removed ghost");
								_playerSessions[senderEndpoint].Player.HandleDisconnectionNotification();
								_playerSessions.Remove(senderEndpoint);
							}

							PlayerNetworkSession session =
								new PlayerNetworkSession(new Player(this, senderEndpoint, _levels[_random.Next(0, _levels.Count)], incoming.mtuSize), senderEndpoint);
							_playerSessions.Add(senderEndpoint, session);
						}
						var data = packet.Encode();
						TraceSend(packet);
						SendData(data, senderEndpoint);
						break;
					}
				}

				message.PutPool();
			}
			else
			{
				DatagramHeader header = new DatagramHeader(receiveBytes[0]);
				if (!header.isACK && !header.isNAK && header.isValid)
				{
					if (receiveBytes[0] == 0xa0)
					{
						throw new Exception("Receive ERROR, NAK in wrong place");
					}

					ConnectedPackage package = new ConnectedPackage();
					package.Decode(receiveBytes);
					var messages = package.Messages;

					foreach (var message in messages)
					{
						TraceReceive(message);
						EnqueueAck(senderEndpoint, package._datagramSequenceNumber);
						HandlePackage(message, senderEndpoint);
						message.PutPool();
					}
				}
				else if (header.isACK && header.isValid)
				{
					Ack ack = new Ack();
					ack.Decode(receiveBytes);
				}
				else if (header.isNAK && header.isValid)
				{
					Nak nak = new Nak();
					nak.Decode(receiveBytes);
					Log.WarnFormat("!--> NAK on #{0}", nak.sequenceNumber.IntValue());
					_level.BroadcastTextMessage(string.Format("NAK #{0}", nak.sequenceNumber.IntValue()));
				}
				else if (!header.isValid)
				{
					Log.Warn("!!!! ERROR, Invalid header !!!!!");
				}
			}
		}

		/// <summary>
		///     Handles the specified package.
		/// </summary>
		/// <param name="message">The package.</param>
		/// <param name="senderEndpoint">The sender's endpoint.</param>
		private void HandlePackage(Package message, IPEndPoint senderEndpoint)
		{
			if (typeof (UnknownPackage) == message.GetType())
			{
				return;
			}

			if (_playerSessions.ContainsKey(senderEndpoint))
			{
				_playerSessions[senderEndpoint].Player.HandlePackage(message);
			}

			if (typeof (DisconnectionNotification) == message.GetType())
			{
				_playerSessions.Remove(senderEndpoint);
			}
		}

		private void EnqueueAck(IPEndPoint senderEndpoint, Int24 sequenceNumber)
		{
			var ack = Ack.CreateObject();
			ack.sequenceNumber = sequenceNumber;
			ack.count = 1;
			ack.onlyOneSequence = 1;

			_numberOfAckSent++;

			if (_playerSessions.ContainsKey(senderEndpoint))
			{
				var session = _playerSessions[senderEndpoint];
				session.PlayerAckQueue.Enqueue(ack);
				//SendAckQueue(null);
			}

			//var data = ack.Encode();
			//SendData(data, senderEndpoint);

			if (_ackTimer == null)
			{
				_ackTimer = new Timer(SendAckQueue, null, 0, 10);
			}
		}

		private Timer _ackTimer;

		private void SendAckQueue(object state)
		{
			Parallel.ForEach(_playerSessions.Values.ToList(), delegate(PlayerNetworkSession session)
			{
				var queue = session.PlayerAckQueue;
				int lenght = queue.Count;

				if (lenght == 0) return;

				Acks acks = new Acks();
				for (int i = 0; i < lenght; i++)
				{
					Ack ack;
					if (!session.PlayerAckQueue.TryDequeue(out ack)) break;

					acks.acks.Add(ack.sequenceNumber.IntValue());
					ack.PutPool();
				}

				if (acks.acks.Count > 0)
				{
					byte[] data = acks.Encode();
					SendData(data, session.EndPoint);
				}
			});
		}

		public void SendPackage(IPEndPoint senderEndpoint, List<Package> messages, short mtuSize, ref int datagramSequenceNumber, ref int reliableMessageNumber, Reliability reliability = Reliability.RELIABLE)
		{
			if (messages.Count == 0) return;

			Datagram.CreateDatagrams(messages, mtuSize, ref datagramSequenceNumber, ref reliableMessageNumber, senderEndpoint, GetValue);

			foreach (var message in messages)
			{
				if (message is InternalPing)
				{
					_latency = message.Timer.ElapsedMilliseconds;
				}

				TraceSend(message);
				message.PutPool();
			}
		}

		private void GetValue(IPEndPoint senderEndpoint, Datagram datagram)
		{
			Task<int> task = null;
			if (datagram.MessageParts.Count != 0)
			{
				byte[] data = datagram.Encode();

				SendData(data, senderEndpoint);

				foreach (MessagePart part in datagram.MessageParts)
				{
					part.Buffer = null;
					part.PutPool();
				}
			}

			datagram.PutPool();
		}

		private long _numberOfAckSent = 0;
		private long _numberOfPacketsOutPerSecond = 0;
		private long _numberOfPacketsInPerSecond = 0;
		private long _totalPacketSizeOut = 0;
		private long _totalPacketSizeIn = 0;
		private Timer _throughPut = null;
		private long _latency = -1;
		private int _availableBytes;

		/// <summary>
		///     Sends the data.
		/// </summary>
		/// <param name="data">The data.</param>
		/// <param name="targetEndpoint">The target endpoint.</param>
		private void SendData(byte[] data, IPEndPoint targetEndpoint)
		{
			if (_throughPut == null)
			{
				_throughPut = new Timer(delegate(object state)
				{
					int threads;
					int portThreads;
					ThreadPool.GetAvailableThreads(out threads, out portThreads);
					double kbitPerSecondOut = _totalPacketSizeOut*8/1000000D;
					double kbitPerSecondIn = _totalPacketSizeIn*8/1000000D;
					Log.InfoFormat("TT {4:00}ms Ly {6:00}ms {5} Pl(s) Pkt(#/s) ({0} {2}) ACKs {1}/s Tput(Mbit/s) ({3:F} {7:F}) Avail {8}kb Threads {9} Compl.ports {10}",
						_numberOfPacketsOutPerSecond, _numberOfAckSent, _numberOfPacketsInPerSecond, kbitPerSecondOut, _level.LastTickProcessingTime,
						_level.Players.Count, _latency, kbitPerSecondIn, _availableBytes/1000, threads, portThreads);

					_numberOfAckSent = 0;
					_totalPacketSizeOut = 0;
					_totalPacketSizeIn = 0;
					_numberOfPacketsOutPerSecond = 0;
					_numberOfPacketsInPerSecond = 0;
				}, null, 1000, 1000);
			}

			//_listener.SendAsync(data, data.Length, targetEndpoint);
			//_listener.BeginSend(data, data.Length, targetEndpoint, null, null);

			//lock (_listener)
			{
				_listener.SendAsync(data, data.Length, targetEndpoint).Wait();
			}

			_numberOfPacketsOutPerSecond++;
			_totalPacketSizeOut += data.Length;
		}

		// ReSharper disable once UnusedMember.Global

		/// <summary>
		///     Converts a byte[] to string.
		/// </summary>
		/// <param name="ba">The data to convert.</param>
		/// <returns></returns>
		public static string ByteArrayToString(byte[] ba)
		{
			StringBuilder hex = new StringBuilder((ba.Length*2) + 100);
			hex.Append("{");
			foreach (byte b in ba)
				hex.AppendFormat("0x{0:x2},", b);
			hex.Append("}");
			return hex.ToString();
		}


		private static bool _performanceTest = false;

		private static void TraceReceive(Package message)
		{
			if (_performanceTest || !Debugger.IsAttached || !Log.IsDebugEnabled) return;

			if (!(message is InternalPing) && message.Id != (int) DefaultMessageIdTypes.ID_CONNECTED_PING && message.Id != (int) DefaultMessageIdTypes.ID_UNCONNECTED_PING)
			{
				Log.DebugFormat("> Receive: {0}: {1} (0x{0:x2})", message.Id, message.GetType().Name);
			}
		}

		private static void TraceSend(Package message)
		{
			if (_performanceTest || !Debugger.IsAttached || !Log.IsDebugEnabled) return;

			if (!(message is InternalPing) && message.Id != (int) DefaultMessageIdTypes.ID_CONNECTED_PONG && message.Id != (int) DefaultMessageIdTypes.ID_UNCONNECTED_PONG)
			{
				Log.DebugFormat("<    Send: {0}: {1} (0x{0:x2})", message.Id, message.GetType().Name);
			}
		}
	}
}