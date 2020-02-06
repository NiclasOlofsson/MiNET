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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using Microsoft.IO;
using MiNET.Net;
using MiNET.Plugins;
using MiNET.Utils;
using MiNET.Worlds;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MiNET
{
	public class MiNetServer
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(MiNetServer));

		public const string MiNET = "\r\n __   __  ___   __    _  _______  _______ \r\n|  |_|  ||   | |  |  | ||       ||       |\r\n|       ||   | |   |_| ||    ___||_     _|\r\n|       ||   | |       ||   |___   |   |  \r\n|       ||   | |  _    ||    ___|  |   |  \r\n| ||_|| ||   | | | |   ||   |___   |   |  \r\n|_|   |_||___| |_|  |__||_______|  |___|  \r\n";

		private const int DefaultPort = 19132;

		public IPEndPoint Endpoint { get; private set; }
		private UdpClient _listener;
		private ConcurrentDictionary<IPEndPoint, PlayerNetworkSession> _playerSessions = new ConcurrentDictionary<IPEndPoint, PlayerNetworkSession>();

		public MotdProvider MotdProvider { get; set; }

		public static RecyclableMemoryStreamManager MemoryStreamManager { get; set; } = new RecyclableMemoryStreamManager();

		public IServerManager ServerManager { get; set; }
		public LevelManager LevelManager { get; set; }
		public PlayerFactory PlayerFactory { get; set; }
		public GreylistManager GreylistManager { get; set; }

		public bool IsEdu { get; set; } = Config.GetProperty("EnableEdu", false);
		public EduTokenManager EduTokenManager { get; set; }

		public PluginManager PluginManager { get; set; }
		public SessionManager SessionManager { get; set; }

		private Timer _internalPingTimer;
		private Timer _cleanerTimer;

		public int InacvitityTimeout { get; private set; }
		public int ResendThreshold { get; private set; }

		public ServerInfo ServerInfo { get; set; }

		public ServerRole ServerRole { get; set; } = ServerRole.Full;

		public bool ForceOrderingForAll { get; set; }

		internal static DedicatedThreadPool FastThreadPool { get; set; }
		internal static DedicatedThreadPool LevelThreadPool { get; set; }

		public MiNetServer()
		{
			ServerRole = Config.GetProperty("ServerRole", ServerRole.Full);
			InacvitityTimeout = Config.GetProperty("InactivityTimeout", 8500);
			ResendThreshold = Config.GetProperty("ResendThreshold", 10);
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
			_receiveThreadPool = new DedicatedThreadPool(new DedicatedThreadPoolSettings(Environment.ProcessorCount));
		}

		public MiNetServer(IPEndPoint endpoint) : base()
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

				ServerManager = ServerManager ?? new DefaultServerManager(this);

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
					//LevelManager = LevelManager ?? new SpreadLevelManager(1);
					PlayerFactory = PlayerFactory ?? new PlayerFactory();

					PluginManager.EnablePlugins(this, LevelManager);

					// Cache - remove
					LevelManager.GetLevel(null, Dimension.Overworld.ToString());
				}

				GreylistManager = GreylistManager ?? new GreylistManager(this);
				MotdProvider = MotdProvider ?? new MotdProvider();

				if (ServerRole == ServerRole.Full || ServerRole == ServerRole.Proxy)
				{
					_listener = CreateListener();

					new Thread(ProcessDatagrams) {IsBackground = true}.Start(_listener);
				}

				ServerInfo = new ServerInfo(LevelManager, _playerSessions)
				{
					MaxNumberOfPlayers = Config.GetProperty("MaxNumberOfPlayers", 10)
				};
				ServerInfo.MaxNumberOfConcurrentConnects = Config.GetProperty("MaxNumberOfConcurrentConnects", ServerInfo.MaxNumberOfPlayers);

				_tickerHighPrecisionTimer = new HighPrecisionTimer(10, SendTick, true);

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

		private void SendTick(object obj)
		{
			foreach (var session in _playerSessions.Values) session.SendTick(null);
			//Parallel.ForEach(_playerSessions.Values, (session, state) => { session.SendTick(null); });
		}

		private UdpClient CreateListener()
		{
			var listener = new UdpClient();

			//_listener.Client.ReceiveBufferSize = 1600*40000;
			listener.Client.ReceiveBufferSize = int.MaxValue;
			//_listener.Client.SendBufferSize = 1600*40000;
			listener.Client.SendBufferSize = int.MaxValue;
			listener.DontFragment = false;
			listener.EnableBroadcast = true;

			if (Environment.OSVersion.Platform != PlatformID.Unix && Environment.OSVersion.Platform != PlatformID.MacOSX)
			{
				// SIO_UDP_CONNRESET (opcode setting: I, T==3)
				// Windows:  Controls whether UDP PORT_UNREACHABLE messages are reported.
				// - Set to TRUE to enable reporting.
				// - Set to FALSE to disable reporting.

				uint IOC_IN = 0x80000000;
				uint IOC_VENDOR = 0x18000000;
				uint SIO_UDP_CONNRESET = IOC_IN | IOC_VENDOR | 12;
				listener.Client.IOControl((int) SIO_UDP_CONNRESET, new byte[] {Convert.ToByte(false)}, null);

				//
				//WARNING: We need to catch errors here to remove the code above.
				//
			}

			//_cleanerTimer = new Timer(Update, null, 10, Timeout.Infinite);

			listener.Client.Bind(Endpoint);
			return listener;
		}

		public bool StopServer()
		{
			try
			{
				Log.Info("Disabling plugins...");
				PluginManager?.DisablePlugins();

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

		private void ProcessDatagrams(object state)
		{
			UdpClient listener = (UdpClient) state;

			while (true)
			{
				// Check if we already closed the server
				if (listener.Client == null) return;

				// WSAECONNRESET:
				// The virtual circuit was reset by the remote side executing a hard or abortive close. 
				// The application should close the socket; it is no longer usable. On a UDP-datagram socket 
				// this error indicates a previous send operation resulted in an ICMP Port Unreachable message.
				// Note the spocket settings on creation of the server. It makes us ignore these resets.
				IPEndPoint senderEndpoint = null;
				try
				{
					//var result = listener.ReceiveAsync().Result;
					//senderEndpoint = result.RemoteEndPoint;
					//receiveBytes = result.Buffer;
					Byte[] receiveBytes = listener.Receive(ref senderEndpoint);

					Interlocked.Exchange(ref ServerInfo.AvailableBytes, listener.Available);
					Interlocked.Increment(ref ServerInfo.NumberOfPacketsInPerSecond);
					Interlocked.Add(ref ServerInfo.TotalPacketSizeIn, receiveBytes.Length);

					if (receiveBytes.Length != 0)
					{
						_receiveThreadPool.QueueUserWorkItem(() =>
						{
							try
							{
								if (!GreylistManager.IsWhitelisted(senderEndpoint.Address) && GreylistManager.IsBlacklisted(senderEndpoint.Address)) return;
								if (GreylistManager.IsGreylisted(senderEndpoint.Address)) return;
								ProcessMessage(receiveBytes, senderEndpoint);
							}
							catch (Exception e)
							{
								Log.Warn($"Process message error from: {senderEndpoint.Address}", e);
							}
						});
					}
					else
					{
						Log.Warn("Unexpected end of transmission?");
						continue;
					}
				}
				catch (Exception e)
				{
					Log.Error("Unexpected end of transmission?", e);
					if (listener.Client != null)
					{
						continue;
					}

					return;
				}
			}
		}

		private void ProcessMessage(byte[] receiveBytes, IPEndPoint senderEndpoint)
		{
			byte msgId = receiveBytes[0];

			if (msgId == 0xFE)
			{
				Log.InfoFormat("A query detected from: {0}", senderEndpoint.Address);
				HandleQuery(receiveBytes, senderEndpoint);
			}
			else if (msgId <= (byte) DefaultMessageIdTypes.ID_USER_PACKET_ENUM)
			{
				HandleRakNetMessage(receiveBytes, senderEndpoint, msgId);
			}
			else
			{
				PlayerNetworkSession playerSession;
				if (!_playerSessions.TryGetValue(senderEndpoint, out playerSession))
				{
					//Log.DebugFormat("Receive MCPE message 0x{1:x2} without session {0}", senderEndpoint.Address, msgId);
					//if (!_badPacketBans.ContainsKey(senderEndpoint.Address))
					//{
					//	_badPacketBans.Add(senderEndpoint.Address, true);
					//}
					return;
				}

				if (playerSession.MessageHandler == null)
				{
					Log.ErrorFormat("Receive MCPE message 0x{1:x2} without message handler {0}. Session removed.", senderEndpoint.Address, msgId);
					_playerSessions.TryRemove(senderEndpoint, out playerSession);
					//if (!_badPacketBans.ContainsKey(senderEndpoint.Address))
					//{
					//	_badPacketBans.Add(senderEndpoint.Address, true);
					//}
					return;
				}

				if (playerSession.Evicted) return;

				playerSession.LastUpdatedTime = DateTime.UtcNow;

				DatagramHeader header = new DatagramHeader(receiveBytes[0]);
				if (!header.isACK && !header.isNAK && header.isValid)
				{
					if (receiveBytes[0] == 0xa0)
					{
						throw new Exception("Receive ERROR, NAK in wrong place");
					}

					ConnectedPacket packet = ConnectedPacket.CreateObject();
					try
					{
						packet.Decode(receiveBytes);
					}
					catch (Exception e)
					{
						playerSession.Disconnect("Bad packet received from client.");

						Log.Warn($"Bad packet {receiveBytes[0]}\n{Packet.HexDump(receiveBytes)}", e);

						GreylistManager.Blacklist(senderEndpoint.Address);

						return;
					}


					// IF reliable code below is enabled, useItem start sending doubles
					// for some unknown reason.

					//Reliability reliability = packet._reliability;
					//if (reliability == Reliability.Reliable
					//	|| reliability == Reliability.ReliableSequenced
					//	|| reliability == Reliability.ReliableOrdered
					//	)
					{
						EnqueueAck(playerSession, packet._datagramSequenceNumber);
						//if (Log.IsDebugEnabled) Log.Debug("ACK on #" + packet._datagramSequenceNumber.IntValue());
					}

					HandleConnectedPacket(playerSession, packet);
					packet.PutPool();
				}
				else if (header.isACK && header.isValid)
				{
					HandleAck(playerSession, receiveBytes);
				}
				else if (header.isNAK && header.isValid)
				{
					HandleNak(playerSession, receiveBytes);
				}
				else if (!header.isValid)
				{
					Log.Warn("!!!! ERROR, Invalid header !!!!!");
				}
			}
		}

		private ConcurrentDictionary<IPEndPoint, DateTime> _connectionAttemps = new ConcurrentDictionary<IPEndPoint, DateTime>();
		private DedicatedThreadPool _receiveThreadPool;
		private HighPrecisionTimer _tickerHighPrecisionTimer;

		private void HandleRakNetMessage(byte[] receiveBytes, IPEndPoint senderEndpoint, byte msgId)
		{
			var msgIdType = (DefaultMessageIdTypes) msgId;

			// Increase fast, decrease slow on 1s ticks.
			if (ServerInfo.NumberOfPlayers < ServerInfo.PlayerSessions.Count) ServerInfo.NumberOfPlayers = ServerInfo.PlayerSessions.Count;

			// Shortcut to reply fast, and no parsing
			if (msgIdType == DefaultMessageIdTypes.ID_OPEN_CONNECTION_REQUEST_1)
			{
				if (!GreylistManager.AcceptConnection(senderEndpoint.Address))
				{
					var noFree = NoFreeIncomingConnections.CreateObject();
					var bytes = noFree.Encode();

					TraceSend(noFree);
					
					noFree.PutPool();

					SendData(bytes, senderEndpoint);
					Interlocked.Increment(ref ServerInfo.NumberOfDeniedConnectionRequestsPerSecond);
					return;
				}
			}

			Packet message = null;
			try
			{
				try
				{
					message = PacketFactory.Create(msgId, receiveBytes, "raknet");
				}
				catch (Exception)
				{
					message = null;
				}

				if (message == null)
				{
					GreylistManager.Blacklist(senderEndpoint.Address);
					Log.ErrorFormat("Receive bad packet with ID: {0} (0x{0:x2}) {2} from {1}", msgId, senderEndpoint.Address, (DefaultMessageIdTypes) msgId);

					return;
				}

				TraceReceive(message);

				switch (msgIdType)
				{
					case DefaultMessageIdTypes.ID_UNCONNECTED_PING:
					case DefaultMessageIdTypes.ID_UNCONNECTED_PING_OPEN_CONNECTIONS:
					{
						HandleRakNetMessage(senderEndpoint, (UnconnectedPing) message);
						break;
					}
					case DefaultMessageIdTypes.ID_OPEN_CONNECTION_REQUEST_1:
					{
						HandleRakNetMessage(senderEndpoint, (OpenConnectionRequest1) message);
						break;
					}
					case DefaultMessageIdTypes.ID_OPEN_CONNECTION_REQUEST_2:
					{
						HandleRakNetMessage(senderEndpoint, (OpenConnectionRequest2) message);
						break;
					}
					default:
						GreylistManager.Blacklist(senderEndpoint.Address);
						if (Log.IsInfoEnabled)
						{
							Log.ErrorFormat("Receive unexpected packet with ID: {0} (0x{0:x2}) {2} from {1}", msgId, senderEndpoint.Address, (DefaultMessageIdTypes) msgId);
						}
						break;
				}
			}
			finally
			{
				if (message != null) message.PutPool();
			}
		}

		private void HandleRakNetMessage(IPEndPoint senderEndpoint, UnconnectedPing incoming)
		{
			//TODO: This needs to be verified with RakNet first
			//response.sendpingtime = msg.sendpingtime;
			//response.sendpongtime = DateTimeOffset.UtcNow.Ticks / TimeSpan.TicksPerMillisecond;

			if (IsEdu)
			{
				var packet = UnconnectedPong.CreateObject();
				packet.serverId = MotdProvider.ServerId;
				packet.pingId = incoming.pingId;
				packet.serverName = MotdProvider.GetMotd(ServerInfo, senderEndpoint, true);
				var data = packet.Encode();
				
				TraceSend(packet);

				packet.PutPool();

				SendData(data, senderEndpoint);
			}

			{
				Log.Debug($"Ping from: {senderEndpoint.Address.ToString()}:{senderEndpoint.Port}");

				var packet = UnconnectedPong.CreateObject();
				packet.serverId = MotdProvider.ServerId;
				packet.pingId = incoming.pingId;
				packet.serverName = MotdProvider.GetMotd(ServerInfo, senderEndpoint);
				var data = packet.Encode();

				TraceSend(packet);

				packet.PutPool();

				SendData(data, senderEndpoint);
			}

			return;
		}

		private void HandleRakNetMessage(IPEndPoint senderEndpoint, OpenConnectionRequest1 incoming)
		{
			lock (_playerSessions)
			{
				// Already connecting, then this is just a duplicate
				if (_connectionAttemps.ContainsKey(senderEndpoint))
				{
					DateTime created;
					_connectionAttemps.TryGetValue(senderEndpoint, out created);

					if (DateTime.UtcNow < created + TimeSpan.FromSeconds(3))
					{
						return;
					}

					_connectionAttemps.TryRemove(senderEndpoint, out created);
				}

				if (!_connectionAttemps.TryAdd(senderEndpoint, DateTime.UtcNow)) return;
			}

			if (Log.IsDebugEnabled)
				Log.WarnFormat("New connection from: {0} {1}, MTU: {2}, Ver: {3}", senderEndpoint.Address, senderEndpoint.Port, incoming.mtuSize, incoming.raknetProtocolVersion);

			var packet = OpenConnectionReply1.CreateObject();
			packet.serverGuid = MotdProvider.ServerId;
			packet.mtuSize = incoming.mtuSize;
			packet.serverHasSecurity = 0;
			var data = packet.Encode();

			TraceSend(packet);

			packet.PutPool();

			SendData(data, senderEndpoint);
		}

		private void HandleRakNetMessage(IPEndPoint senderEndpoint, OpenConnectionRequest2 incoming)
		{
			PlayerNetworkSession session;
			lock (_playerSessions)
			{
				DateTime trash;
				if (!_connectionAttemps.TryRemove(senderEndpoint, out trash))
				{
					Log.WarnFormat("Unexpected connection request packet from {0}. Probably a resend.", senderEndpoint.Address);
					return;
				}

				if (_playerSessions.TryGetValue(senderEndpoint, out session))
				{
					// Already connecting, then this is just a duplicate
					if (session.State == ConnectionState.Connecting /* && DateTime.UtcNow < session.LastUpdatedTime + TimeSpan.FromSeconds(2)*/)
					{
						return;
					}

					Log.InfoFormat("Unexpected session from {0}. Removing old session and disconnecting old player.", senderEndpoint.Address);

					session.Disconnect("Reconnecting.", false);

					_playerSessions.TryRemove(senderEndpoint, out session);
				}

				session = new PlayerNetworkSession(this, null, senderEndpoint, incoming.mtuSize)
				{
					State = ConnectionState.Connecting,
					LastUpdatedTime = DateTime.UtcNow,
					MtuSize = incoming.mtuSize,
					NetworkIdentifier = incoming.clientGuid
				};

				_playerSessions.TryAdd(senderEndpoint, session);
			}

			//Player player = PlayerFactory.CreatePlayer(this, senderEndpoint);
			//player.ClientGuid = incoming.clientGuid;
			//player.NetworkHandler = session;
			//session.Player = player;
			session.MessageHandler = new LoginMessageHandler(session);

			var reply = OpenConnectionReply2.CreateObject();
			reply.serverGuid = MotdProvider.ServerId;
			reply.clientEndpoint = senderEndpoint;
			reply.mtuSize = incoming.mtuSize;
			reply.doSecurityAndHandshake = new byte[1];
			var data = reply.Encode();

			TraceSend(reply);
			
			reply.PutPool();


			SendData(data, senderEndpoint);
		}

		private void HandleConnectedPacket(PlayerNetworkSession playerSession, ConnectedPacket packet)
		{
			foreach (var message in packet.Messages)
			{
				if (message is SplitPartPacket)
				{
					HandleSplitMessage(playerSession, (SplitPartPacket) message);
					continue;
				}

				message.Timer.Restart();
				HandlePacket(message, playerSession);
			}
		}

		private void HandleSplitMessage(PlayerNetworkSession playerSession, SplitPartPacket splitMessage)
		{
			int spId = splitMessage.SplitId;
			int spIdx = splitMessage.SplitIdx;
			int spCount = splitMessage.SplitCount;

			Int24 sequenceNumber = splitMessage.DatagramSequenceNumber;
			Reliability reliability = splitMessage.Reliability;
			Int24 reliableMessageNumber = splitMessage.ReliableMessageNumber;
			Int24 orderingIndex = splitMessage.OrderingIndex;
			byte orderingChannel = splitMessage.OrderingChannel;

			SplitPartPacket[] spPackets;
			bool haveEmpty = false;

			// Need sync for this part since they come very fast, and very close in time. 
			// If no synk, will often detect complete message two times (or more).
			lock (playerSession.Splits)
			{
				if (!playerSession.Splits.ContainsKey(spId))
				{
					playerSession.Splits.TryAdd(spId, new SplitPartPacket[spCount]);
				}

				spPackets = playerSession.Splits[spId];
				if (spPackets[spIdx] != null)
				{
					Log.Debug("Already had splitpart (resent). Ignore this part.");
					return;
				}
				spPackets[spIdx] = splitMessage;

				for (int i = 0; i < spPackets.Length; i++)
				{
					haveEmpty = haveEmpty || spPackets[i] == null;
				}
			}

			if (!haveEmpty)
			{
				Log.DebugFormat("Got all {0} split packets for split ID: {1}", spCount, spId);

				SplitPartPacket[] waste;
				playerSession.Splits.TryRemove(spId, out waste);

				using (MemoryStream stream = MemoryStreamManager.GetStream())
				{
					for (int i = 0; i < spPackets.Length; i++)
					{
						SplitPartPacket splitPartPacket = spPackets[i];
						byte[] buf = splitPartPacket.Message;
						if (buf == null)
						{
							Log.Error("Expected bytes in splitpart, but got none");
							continue;
						}

						stream.Write(buf, 0, buf.Length);
						splitPartPacket.PutPool();
					}

					byte[] buffer = stream.ToArray();
					try
					{
						ConnectedPacket newPacket = ConnectedPacket.CreateObject();
						newPacket._datagramSequenceNumber = sequenceNumber;
						newPacket._reliability = reliability;
						newPacket._reliableMessageNumber = reliableMessageNumber;
						newPacket._orderingIndex = orderingIndex;
						newPacket._orderingChannel = (byte) orderingChannel;
						newPacket._hasSplit = false;

						Packet fullMessage = PacketFactory.Create(buffer[0], buffer, "raknet") ??
											new UnknownPacket(buffer[0], buffer);
						fullMessage.DatagramSequenceNumber = sequenceNumber;
						fullMessage.Reliability = reliability;
						fullMessage.ReliableMessageNumber = reliableMessageNumber;
						fullMessage.OrderingIndex = orderingIndex;
						fullMessage.OrderingChannel = orderingChannel;

						newPacket.Messages = new List<Packet>();
						newPacket.Messages.Add(fullMessage);

						Log.Debug(
							$"Assembled split packet {newPacket._reliability} message #{newPacket._reliableMessageNumber}, Chan: #{newPacket._orderingChannel}, OrdIdx: #{newPacket._orderingIndex}");
						HandleConnectedPacket(playerSession, newPacket);
						newPacket.PutPool();
					}
					catch (Exception e)
					{
						Log.Error("Error during split message parsing", e);
						if (Log.IsDebugEnabled)
							Log.Debug($"0x{buffer[0]:x2}\n{Packet.HexDump(buffer)}");
						playerSession.Disconnect("Bad packet received from client.", false);
					}
				}
			}
		}

		private void HandleQuery(byte[] receiveBytes, IPEndPoint senderEndpoint)
		{
			if (!Config.GetProperty("EnableQuery", false)) return;

			if (receiveBytes[0] != 0xFE || receiveBytes[1] != 0xFD) return;

			byte packetId = receiveBytes[2];
			switch (packetId)
			{
				case 0x09:
				{
					byte[] buffer = new byte[17];
					// ID
					buffer[0] = 0x09;

					// Sequence number
					buffer[1] = receiveBytes[3];
					buffer[2] = receiveBytes[4];
					buffer[3] = receiveBytes[5];
					buffer[4] = receiveBytes[6];

					// Textual representation of int32 (token) with null terminator
					string str = new Random().Next().ToString(CultureInfo.InvariantCulture) + "\x00";
					Buffer.BlockCopy(str.ToCharArray(), 0, buffer, 5, 11);

					_listener.Send(buffer, buffer.Length, senderEndpoint);
					break;
				}
				case 0x00:
				{
					using (var stream = MemoryStreamManager.GetStream())
					{
						bool isFullStatRequest = receiveBytes.Length == 15;
						if (Log.IsInfoEnabled) Log.InfoFormat("Full request: {0}", isFullStatRequest);

						// ID
						stream.WriteByte(0x00);

						// Sequence number
						stream.WriteByte(receiveBytes[3]);
						stream.WriteByte(receiveBytes[4]);
						stream.WriteByte(receiveBytes[5]);
						stream.WriteByte(receiveBytes[6]);

						//{
						//	string str = "splitnum\0";
						//	byte[] bytes = Encoding.ASCII.GetBytes(str.ToCharArray());
						//	stream.Write(bytes, 0, bytes.Length);
						//}

						MotdProvider.GetMotd(ServerInfo, senderEndpoint); // Force update the player counts :-)

						var data = new Dictionary<string, string>
						{
							{"splitnum", "" + (char) 128},
							{"hostname", Config.GetProperty("motd", "MiNET: MCPE Server")},
							{"gametype", "SMP"},
							{"game_id", "MINECRAFTPE"},
							{"version", McpeProtocolInfo.GameVersion},
							{"server_engine", "MiNET v1.0.0"},
							{"plugins", "MiNET v1.0.0"},
							{"map", "world"},
							{"numplayers", MotdProvider.NumberOfPlayers.ToString()},
							{"maxplayers", MotdProvider.MaxNumberOfPlayers.ToString()},
							{"whitelist", "off"},
							//{"hostip", "192.168.0.1"},
							//{"hostport", "19132"}
						};

						foreach (KeyValuePair<string, string> valuePair in data)
						{
							string key = valuePair.Key + "\x00" + valuePair.Value + "\x00";
							byte[] bytes = Encoding.ASCII.GetBytes(key.ToCharArray());
							stream.Write(bytes, 0, bytes.Length);
						}

						{
							string str = "\x00\x01player_\x00\x00";
							byte[] bytes = Encoding.ASCII.GetBytes(str.ToCharArray());
							stream.Write(bytes, 0, bytes.Length);
						}

						// End the stream with 0 byte
						stream.WriteByte(0);
						var buffer = stream.ToArray();
						_listener.Send(buffer, buffer.Length, senderEndpoint);
					}
					break;
				}
				default:
					return;
			}
		}

		private void HandleNak(PlayerNetworkSession session, byte[] receiveBytes)
		{
			if (session == null) return;

			Nak nak = Nak.CreateObject();
			nak.Reset();
			nak.Decode(receiveBytes);

			var queue = session.WaitingForAcksQueue;

			foreach (Tuple<int, int> range in nak.ranges)
			{
				Interlocked.Increment(ref ServerInfo.NumberOfNakReceive);

				int start = range.Item1;
				int end = range.Item2;

				for (int i = start; i <= end; i++)
				{
					if (queue.TryGetValue(i, out var datagram))
					{
						CalculateRto(session, datagram);

						datagram.RetransmitImmediate = true;
					}
					else
					{
						if (Log.IsDebugEnabled)
							Log.WarnFormat("NAK, no datagram #{0} for {1}", i, session.Username);
					}
				}
			}

			nak.PutPool();
		}

		private void HandleAck(PlayerNetworkSession session, byte[] receiveBytes)
		{
			if (session == null) return;

			//Ack ack = Ack.CreateObject();
			Ack ack = new Ack();
			//ack.Reset();
			ack.Decode(receiveBytes);

			var queue = session.WaitingForAcksQueue;

			foreach (Tuple<int, int> range in ack.ranges)
			{
				Interlocked.Increment(ref ServerInfo.NumberOfAckReceive);

				int start = range.Item1;
				int end = range.Item2;
				for (int i = start; i <= end; i++)
				{
					if (queue.TryRemove(i, out var datagram))
					{
						//if (Log.IsDebugEnabled)
						//	Log.DebugFormat("ACK, on datagram #{0} for {2}. Queue size={1}", i, queue.Count, player.Username);

						CalculateRto(session, datagram);

						datagram.PutPool();
					}
					else
					{
						if (Log.IsDebugEnabled)
							Log.WarnFormat("ACK, Failed to remove datagram #{0} for {2}. Queue size={1}", i, queue.Count, session.Username);
					}
				}
			}

			//ack.PutPool();

			session.ResendCount = 0;
			session.WaitForAck = false;
		}

		private static void CalculateRto(PlayerNetworkSession session, Datagram datagram)
		{
			// RTT = RTT * 0.875 + rtt * 0.125
			// RTTVar = RTTVar * 0.875 + abs(RTT - rtt)) * 0.125
			// RTO = RTT + 4 * RTTVar
			long rtt = datagram.Timer.ElapsedMilliseconds;
			long RTT = session.Rtt;
			long RTTVar = session.RttVar;

			session.Rtt = (long) (RTT * 0.875 + rtt * 0.125);
			session.RttVar = (long) (RTTVar * 0.875 + Math.Abs(RTT - rtt) * 0.125);
			session.Rto = session.Rtt + 4 * session.RttVar + 100; // SYNC time in the end
		}

		internal void HandlePacket(Packet message, PlayerNetworkSession playerSession)
		{
			if (message == null)
			{
				return;
			}

			if (message.Reliability == Reliability.ReliableOrdered)
			{
				if (ForceOrderingForAll == false && (playerSession.CryptoContext == null || playerSession.CryptoContext.UseEncryption == false))
				{
					playerSession.AddToProcessing(message);
				}
				else
				{
					FastThreadPool.QueueUserWorkItem(() => playerSession.AddToProcessing(message));
				}

				return;
			}

			playerSession.HandlePacket(message, playerSession);
		}

		private void EnqueueAck(PlayerNetworkSession session, int sequenceNumber)
		{
			session.PlayerAckQueue.Enqueue(sequenceNumber);
			session.SignalTick();
		}

		public void SendPacket(PlayerNetworkSession session, Packet message)
		{
			foreach (var datagram in Datagram.CreateDatagrams(message, session.MtuSize, session))
			{
				SendDatagram(session, datagram);
			}

			message.PutPool();
		}

		internal void SendDatagram(PlayerNetworkSession session, Datagram datagram)
		{
			if (datagram.MessageParts.Count == 0)
			{
				datagram.PutPool();
				Log.WarnFormat("Failed to resend #{0}", datagram.Header.datagramSequenceNumber.IntValue());
				return;
			}

			if (datagram.TransmissionCount > 10)
			{
				if (Log.IsDebugEnabled)
					Log.WarnFormat("Retransmission count exceeded. No more resend of #{0} Type: {2} (0x{2:x2}) for {1}",
						datagram.Header.datagramSequenceNumber.IntValue(),
						session.Username,
						datagram.FirstMessageId);

				datagram.PutPool();

				Interlocked.Increment(ref ServerInfo.NumberOfFails);
				return;
			}

			datagram.Header.datagramSequenceNumber = Interlocked.Increment(ref session.DatagramSequenceNumber);
			datagram.TransmissionCount++;
			datagram.RetransmitImmediate = false;

			//byte[] data = datagram.Encode();
			byte[] data;
			var lenght = (int) datagram.GetEncoded(out data);

			datagram.Timer.Restart();

			if (!session.WaitingForAcksQueue.TryAdd(datagram.Header.datagramSequenceNumber.IntValue(), datagram))
			{
				Log.Warn(string.Format("Datagram sequence unexpectedly existed in the ACK/NAK queue already {0}", datagram.Header.datagramSequenceNumber.IntValue()));
			}

			lock (session.SyncRoot)
			{
				SendData(data, lenght, session.EndPoint);
			}
		}

		internal void SendData(byte[] data, int lenght, IPEndPoint targetEndPoint)
		{
			try
			{
				_listener.Send(data, lenght, targetEndPoint); // Less thread-issues it seems

				Interlocked.Increment(ref ServerInfo.NumberOfPacketsOutPerSecond);
				Interlocked.Add(ref ServerInfo.TotalPacketSizeOut, lenght);
			}
			catch (ObjectDisposedException e)
			{
				Log.Warn(e);
			}
			catch (Exception e)
			{
				Log.Warn(e);
				//if (_listener == null || _listener.Client != null) Log.Error(string.Format("Send data lenght: {0}", data.Length), e);
			}
		}


		internal void SendData(byte[] data, IPEndPoint targetEndPoint)
		{
			try
			{
				_listener.Send(data, data.Length, targetEndPoint); // Less thread-issues it seems

				Interlocked.Increment(ref ServerInfo.NumberOfPacketsOutPerSecond);
				Interlocked.Add(ref ServerInfo.TotalPacketSizeOut, data.Length);
			}
			catch (ObjectDisposedException e)
			{
			}
			catch (Exception e)
			{
				//if (_listener == null || _listener.Client != null) Log.Error(string.Format("Send data lenght: {0}", data.Length), e);
			}
		}

		internal static void TraceReceive(Packet message)
		{
			if (!Log.IsDebugEnabled) return;

			try
			{
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
				else if (verbosity == 1 || verbosity == 3)
				{
					var jsonSerializerSettings = new JsonSerializerSettings
					{
						PreserveReferencesHandling = PreserveReferencesHandling.Arrays,
						TypeNameHandling = TypeNameHandling.Auto,
						Formatting = Formatting.Indented,
					};

					jsonSerializerSettings.Converters.Add(new StringEnumConverter());
					jsonSerializerSettings.Converters.Add(new NbtIntConverter());
					jsonSerializerSettings.Converters.Add(new NbtStringConverter());
					jsonSerializerSettings.Converters.Add(new IPAddressConverter());
					jsonSerializerSettings.Converters.Add(new IPEndPointConverter());

					string result = JsonConvert.SerializeObject(message, jsonSerializerSettings);
					Log.Debug($"> Receive: {message.Id} (0x{message.Id:x2}): {message.GetType().Name}\n{result}");
				}
				else if (verbosity == 2 || verbosity == 3)
				{
					Log.Debug($"> Receive: {message.Id} (0x{message.Id:x2}): {message.GetType().Name}\n{Packet.HexDump(message.Bytes)}");
				}
			}
			catch (Exception e)
			{
				Log.Error("Error when printing trace", e);
			}
		}

		//public static void TraceSend(Packet message)
		//{
		//	if (!Log.IsDebugEnabled) return;
		//	if (message is McpeWrapper) return;
		//	if (message is UnconnectedPong) return;
		//	if (message is McpeMovePlayer) return;
		//	//if (message is McpeSetEntityMotion) return;
		//	//if (message is McpeMoveEntity) return;
		//	if (message is McpeSetEntityData) return;
		//	if (message is McpeUpdateBlock) return;
		//	if (message is McpeText) return;
		//	if (message is McpeLevelEvent) return;
		//	//if (!Debugger.IsAttached) return;

		//	Log.DebugFormat("<    Send: {0}: {1} (0x{0:x2})", message.Id, message.GetType().Name);
		//}

		internal static void TraceSend(Packet message)
		{
			if (!Log.IsDebugEnabled) return;

			try
			{
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
					Log.Debug($"<    Send: {message.Id} (0x{message.Id:x2}): {message.GetType().Name}");
				}
				else if (verbosity == 1 || verbosity == 3)
				{
					var jsonSerializerSettings = new JsonSerializerSettings
					{
						PreserveReferencesHandling = PreserveReferencesHandling.Arrays,
						TypeNameHandling = TypeNameHandling.Auto,
						Formatting = Formatting.Indented,
					};
					jsonSerializerSettings.Converters.Add(new NbtIntConverter());
					jsonSerializerSettings.Converters.Add(new NbtStringConverter());
					jsonSerializerSettings.Converters.Add(new IPAddressConverter());
					jsonSerializerSettings.Converters.Add(new IPEndPointConverter());

					string result = JsonConvert.SerializeObject(message, jsonSerializerSettings);
					Log.Debug($"<    Send: {message.Id} (0x{message.Id:x2}): {message.GetType().Name}\n{result}");
				}
				else if (verbosity == 2 || verbosity == 3)
				{
					Log.Debug($"<    Send: {message.Id} (0x{message.Id:x2}): {message.GetType().Name}\n{Packet.HexDump(message.Bytes)}");
				}
			}
			catch (Exception e)
			{
				Log.Error("Error when printing trace", e);
			}
		}
	}

	public enum ServerRole
	{
		Node,
		Proxy,
		Full,
	}
}
