using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using Microsoft.AspNet.Identity;
using MiNET.Net;
using MiNET.Plugins;
using MiNET.Security;
using MiNET.Utils;

namespace MiNET
{
	public class MiNetServer
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (MiNetServer));

		private const int DefaultPort = 19132;

		public IPEndPoint Endpoint { get; private set; }
		private UdpClient _listener;
		private ConcurrentDictionary<IPEndPoint, PlayerNetworkSession> _playerSessions = new ConcurrentDictionary<IPEndPoint, PlayerNetworkSession>();

		public bool ForwardAllPlayers { get; set; }
		public IPEndPoint ForwardTarget { get; set; }

		public MotdProvider MotdProvider { get; set; }

		public bool IsSecurityEnabled { get; private set; }
		public UserManager<User> UserManager { get; set; }
		public RoleManager<Role> RoleManager { get; set; }

		public LevelManager LevelManager { get; set; }
		public PlayerFactory PlayerFactory { get; set; }
		public GreylistManager GreylistManager { get; set; }

		public PluginManager PluginManager { get; set; }
		public SessionManager SessionManager { get; set; }

		private Timer _internalPingTimer;
		private Timer _ackTimer;
		private Timer _cleanerTimer;

		public int InacvitityTimeout { get; private set; }

		public ServerInfo ServerInfo { get; set; }

		public MiNetServer()
		{
		}

		public MiNetServer(IPEndPoint endpoint)
		{
			Endpoint = endpoint;
		}

		public static bool IsRunningOnMono()
		{
			return Type.GetType("Mono.Runtime") != null;
		}

		public bool StartServer()
		{
			if (_listener != null) return false; // Already started

			try
			{
				Log.Info("Initializing...");

				InacvitityTimeout = Config.GetProperty("InacvitityTimeout", 8500);

				if (Endpoint == null)
				{
					var ip = IPAddress.Parse(Config.GetProperty("ip", "0.0.0.0"));
					int port = Config.GetProperty("port", 19132);
					Endpoint = new IPEndPoint(ip, port);
				}

				ForwardAllPlayers = Config.GetProperty("ForwardAllPlayers", false);
				if (ForwardAllPlayers)
				{
					var ip = IPAddress.Parse(Config.GetProperty("ForwardIP", "127.0.0.1"));
					int port = Config.GetProperty("ForwardPort", 19132);
					ForwardTarget = new IPEndPoint(ip, port);
				}

				Log.Info("Loading plugins...");
				PluginManager = new PluginManager();
				PluginManager.LoadPlugins();
				Log.Info("Plugins loaded!");

				// Bootstrap server
				PluginManager.ExecuteStartup(this);

				MotdProvider = MotdProvider ?? new MotdProvider();

				IsSecurityEnabled = Config.GetProperty("EnableSecurity", false);
				if (IsSecurityEnabled)
				{
					// http://www.asp.net/identity/overview/extensibility/overview-of-custom-storage-providers-for-aspnet-identity
					UserManager = UserManager ?? new UserManager<User>(new DefaultUserStore());
					RoleManager = RoleManager ?? new RoleManager<Role>(new DefaultRoleStore());
				}

				GreylistManager = GreylistManager ?? new GreylistManager(this);
				SessionManager = SessionManager ?? new SessionManager();
				LevelManager = LevelManager ?? new LevelManager();
				PlayerFactory = PlayerFactory ?? new PlayerFactory();

				// Cache - remove
				LevelManager.GetLevel(null, "Default");

				ServerInfo = new ServerInfo(LevelManager, _playerSessions);
				ServerInfo.MaxNumberOfPlayers = Config.GetProperty("MaxNumberOfPlayers", 1000);
				ServerInfo.MaxNumberOfConcurrentConnects = Config.GetProperty("MaxNumberOfConcurrentConnects", ServerInfo.MaxNumberOfPlayers);

				//for (int i = 1; i < 10; i++)
				//{
				//	Level level = LevelFactory.CreateLevel("" + i);
				//	_levels.Add(level);
				//}

				PluginManager.EnablePlugins(this, LevelManager);

				_listener = new UdpClient(Endpoint);

				if (IsRunningOnMono())
				{
					_listener.Client.ReceiveBufferSize = 1024*1024*3;
					_listener.Client.SendBufferSize = 4096;
				}
				else
				{
					//_listener.Client.ReceiveBufferSize = 1600*40000;
					_listener.Client.ReceiveBufferSize = int.MaxValue;
					//_listener.Client.SendBufferSize = 1600*40000;
					_listener.Client.SendBufferSize = int.MaxValue;
					_listener.DontFragment = false;
					_listener.EnableBroadcast = false;

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

				_ackTimer = new Timer(SendAckQueue, null, 0, 50);
				_cleanerTimer = new Timer(Update, null, 10, Timeout.Infinite);

				_listener.BeginReceive(ReceiveCallback, _listener);

				// Measure latency through system
				//_internalPingTimer = new Timer(delegate(object state)
				//{
				//	var playerSession = _playerSessions.Values.FirstOrDefault();
				//	if (playerSession != null)
				//	{
				//		var ping = new InternalPing();
				//		ping.Timer.Start();
				//		HandlePackage(ping, playerSession);
				//	}
				//}, null, 1000, 1000);

				Log.Info("Server open for business on port " + Endpoint.Port + " ...");

				return true;
			}
			catch (Exception e)
			{
				Log.Error("Error during startup!", e);
				StopServer();
			}

			return false;
		}

		public bool StopServer()
		{
			try
			{
				Log.Info("Disabling plugins...");
				PluginManager.DisablePlugins();

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

		private void ReceiveCallback(IAsyncResult ar)
		{
			UdpClient listener = (UdpClient) ar.AsyncState;

			// Check if we already closed the server
			if (listener.Client == null) return;

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
				Log.Error("Unexpected end of transmission?", e);
				if (listener.Client != null)
				{
					try
					{
						listener.BeginReceive(ReceiveCallback, listener);
					}
					catch (ObjectDisposedException dex)
					{
						Log.Error("Unexpected end of transmission?", dex);
					}
				}

				return;
			}

			if (receiveBytes.Length != 0)
			{
				listener.BeginReceive(ReceiveCallback, listener);
				ServerInfo.AvailableBytes = listener.Available;
				ServerInfo.NumberOfPacketsInPerSecond++;
				ServerInfo.TotalPacketSizeIn += receiveBytes.Length;
				try
				{
					if (!GreylistManager.IsWhitelisted(senderEndpoint.Address) && GreylistManager.IsBlacklisted(senderEndpoint.Address)) return;
					if (GreylistManager.IsGreylisted(senderEndpoint.Address)) return;
					ProcessMessage(receiveBytes, senderEndpoint);
				}
				catch (Exception e)
				{
					Log.Warn(string.Format("Process message error from: {0}", senderEndpoint.Address), e);
				}
			}
			else
			{
				Log.Error("Unexpected end of transmission?");
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

				Player player = playerSession.Player;

				if (player == null)
				{
					Log.ErrorFormat("Receive MCPE message 0x{1:x2} without player {0}. Session removed.", senderEndpoint.Address, msgId);
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

					ConnectedPackage package = ConnectedPackage.CreateObject();
					try
					{
						package.Decode(receiveBytes);
					}
					catch (Exception e)
					{
						player.Disconnect("Bad package received from client.");
						//if (Log.IsDebugEnabled)
						{
							Log.Warn("Bad packet " + receiveBytes[0], e);
						}

						GreylistManager.Blacklist(senderEndpoint.Address);

						return;
					}


					// IF reliable code below is enabled, useItem start sending doubles
					// for some unknown reason.

					//Reliability reliability = package._reliability;
					//if (reliability == Reliability.Reliable
					//	|| reliability == Reliability.ReliableSequenced
					//	|| reliability == Reliability.ReliableOrdered
					//	)
					{
						EnqueueAck(playerSession, package._datagramSequenceNumber);
					}


					DelayedProcessing(playerSession, package);
					package.PutPool();
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

		private void HandleRakNetMessage(byte[] receiveBytes, IPEndPoint senderEndpoint, byte msgId)
		{
			DefaultMessageIdTypes msgIdType = (DefaultMessageIdTypes) msgId;

			// Increase fast, decrease slow on 1s ticks.
			if (ServerInfo.NumberOfPlayers < ServerInfo.PlayerSessions.Count) ServerInfo.NumberOfPlayers = ServerInfo.PlayerSessions.Count;

			// Shortcut to reply fast, and no parsing
			if (msgIdType == DefaultMessageIdTypes.ID_OPEN_CONNECTION_REQUEST_1)
			{
				if (!GreylistManager.AcceptConnection(senderEndpoint.Address))
				{
					var noFree = NoFreeIncomingConnections.CreateObject();
					var bytes = noFree.Encode();
					noFree.PutPool();

					TraceSend(noFree);

					SendData(bytes, senderEndpoint);
					ServerInfo.NumberOfDeniedConnectionRequestsPerSecond++;
					return;
				}
			}

			Package message = null;
			try
			{
				try
				{
					message = PackageFactory.CreatePackage(msgId, receiveBytes);
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
						Log.ErrorFormat("Receive unexpected packet with ID: {0} (0x{0:x2}) {2} from {1}", msgId, senderEndpoint.Address, (DefaultMessageIdTypes) msgId);
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

			var packet = UnconnectedPong.CreateObject();
			packet.serverId = 22345;
			packet.pingId = incoming.pingId;
			packet.serverName = MotdProvider.GetMotd(ServerInfo);
			var data = packet.Encode();
			packet.PutPool();

			TraceSend(packet);

			SendData(data, senderEndpoint);
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
				Log.WarnFormat("New connection from: {0} {1}, MTU: {2}", senderEndpoint.Address, senderEndpoint.Port, incoming.mtuSize);

			var packet = OpenConnectionReply1.CreateObject();
			packet.serverGuid = 12345;
			packet.mtuSize = incoming.mtuSize;
			packet.serverHasSecurity = 0;
			var data = packet.Encode();
			packet.PutPool();

			TraceSend(packet);

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

					Player oldPlayer = session.Player;
					if (oldPlayer != null)
					{
						oldPlayer.Disconnect("Reconnecting.", false);
					}

					_playerSessions.TryRemove(session.EndPoint, out session);
				}

				session = new PlayerNetworkSession(null, senderEndpoint)
				{
					State = ConnectionState.Connecting,
					LastUpdatedTime = DateTime.UtcNow,
					Mtuize = incoming.mtuSize
				};

				_playerSessions.TryAdd(senderEndpoint, session);
			}

			Player player = PlayerFactory.CreatePlayer(this, senderEndpoint, incoming.mtuSize);
			player.ClientGuid = incoming.clientGuid;
			player.NetworkSession = session;
			session.Player = player;

			var reply = OpenConnectionReply2.CreateObject();
			reply.serverGuid = 12345;
			reply.clientendpoint = senderEndpoint;
			reply.mtuSize = incoming.mtuSize;
			reply.doSecurityAndHandshake = new byte[1];
			var data = reply.Encode();
			reply.PutPool();

			TraceSend(reply);

			SendData(data, senderEndpoint);
		}

		private void DelayedProcessing(PlayerNetworkSession playerSession, ConnectedPackage package)
		{
			Player player = playerSession.Player;

			if (ForwardAllPlayers)
			{
				player.SendPackage(new McpeTransfer
				{
					endpoint = ForwardTarget
				}, true);

				return;
			}

			List<Package> messages = package.Messages;
			foreach (var message in messages)
			{
				//message.DatagramSequenceNumber = package._datagramSequenceNumber;
				//message.ReliableMessageNumber = package._reliableMessageNumber;
				//message.OrderingChannel = package._orderingChannel;
				//message.OrderingIndex = package._orderingIndex;

				if (message is SplitPartPackage)
				{
					HandleSplitMessage(playerSession, package, (SplitPartPackage) message, player);

					continue;
				}

				message.Timer.Restart();
				HandlePackage(message, playerSession);
				message.PutPool(); // Handled in HandlePacket now()
			}
		}

		private void HandleSplitMessage(PlayerNetworkSession playerSession, ConnectedPackage package, SplitPartPackage splitMessage, Player player)
		{
			int spId = package._splitPacketId;
			int spIdx = package._splitPacketIndex;
			int spCount = package._splitPacketCount;

			if (!playerSession.Splits.ContainsKey(spId))
			{
				playerSession.Splits.TryAdd(spId, new SplitPartPackage[spCount]);
			}

			SplitPartPackage[] spPackets = playerSession.Splits[spId];
			spPackets[spIdx] = splitMessage;

			bool haveEmpty = false;
			for (int i = 0; i < spPackets.Length; i++)
			{
				haveEmpty = haveEmpty || spPackets[i] == null;
			}

			if (!haveEmpty)
			{
				Log.DebugFormat("Got all {0} split packages for split ID: {1}", spCount, spId);

				SplitPartPackage[] waste;
				playerSession.Splits.TryRemove(spId, out waste);

				MemoryStream stream = new MemoryStream();
				for (int i = 0; i < spPackets.Length; i++)
				{
					SplitPartPackage splitPartPackage = spPackets[i];
					byte[] buf = splitPartPackage.Message;
					if (buf == null)
					{
						Log.Error("Expected bytes in splitpart, but got none");
						continue;
					}

					stream.Write(buf, 0, buf.Length);
					splitPartPackage.PutPool();
				}

				byte[] buffer = stream.ToArray();
				try
				{
					Package fullMessage = PackageFactory.CreatePackage(buffer[0], buffer) ?? new UnknownPackage(buffer[0], buffer);
					fullMessage.DatagramSequenceNumber = package._datagramSequenceNumber;
					fullMessage.ReliableMessageNumber = package._reliableMessageNumber;
					fullMessage.OrderingChannel = package._orderingChannel;
					fullMessage.OrderingIndex = package._orderingIndex;
					HandlePackage(fullMessage, playerSession);
					fullMessage.PutPool();
				}
				catch (Exception e)
				{
					player.Disconnect("Bad package received from client.");
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
					var stream = new MemoryStream();

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

					MotdProvider.GetMotd(ServerInfo); // Force update the player counts :-)

					var data = new Dictionary<string, string>
					{
						{"splitnum", "" + (char) 128},
						{"hostname", "Minecraft PE Server"},
						{"gametype", "SMP"},
						{"game_id", "MINECRAFTPE"},
						{"version", "0.13.0"},
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
					break;
				}
				default:
					return;
			}
		}

		private void HandleNak(PlayerNetworkSession session, byte[] receiveBytes)
		{
			if (session == null) return;

			Player player = session.Player;
			if (player == null) return;

			Nak nak = Nak.CreateObject();
			nak.Reset();
			nak.Decode(receiveBytes);

			var queue = session.WaitingForAcksQueue;

			foreach (Tuple<int, int> range in nak.ranges)
			{
				ServerInfo.NumberOfNakReceive++;

				int start = range.Item1;
				int end = range.Item2;

				for (int i = start; i <= end; i++)
				{
					session.ErrorCount++;

					// HACK: Just to make sure we aren't getting unessecary load on the queue during heavy buffering.
					if (ServerInfo.AvailableBytes > 1000) continue;

					Datagram datagram;
					if (queue.TryGetValue(i, out datagram))
					{
						// RTT = RTT * 0.875 + rtt * 0.125
						// RTTVar = RTTVar * 0.875 + abs(RTT - rtt)) * 0.125
						// RTO = RTT + 4 * RTTVar
						long rtt = datagram.Timer.ElapsedMilliseconds;
						long RTT = player.Rtt;
						long RTTVar = player.RttVar;

						player.Rtt = (long) (RTT*0.875 + rtt*0.125);
						player.RttVar = (long) (RTTVar*0.875 + Math.Abs(RTT - rtt)*0.125);
						player.Rto = player.Rtt + 4*player.RttVar + 100; // SYNC time in the end

						//if (Log.IsDebugEnabled)
						//	Log.ErrorFormat("NAK, resending datagram #{0} for {1}, MTU: {2}", i, player.Username, session.Mtuize);

						//SendDatagram(session, datagram, false);
					}
					else
					{
						if (Log.IsDebugEnabled)
							Log.WarnFormat("NAK, no datagram #{0} to resend for {1}", i, player.Username);
					}
				}
			}

			nak.PutPool();
		}

		private void HandleAck(PlayerNetworkSession session, byte[] receiveBytes)
		{
			if (session == null) return;

			Player player = session.Player;
			if (player == null) return;

			Ack ack = Ack.CreateObject();
			ack.Reset();
			ack.Decode(receiveBytes);

			var queue = session.WaitingForAcksQueue;

			foreach (Tuple<int, int> range in ack.ranges)
			{
				ServerInfo.NumberOfAckReceive++;

				int start = range.Item1;
				int end = range.Item2;
				for (int i = start; i <= end; i++)
				{
					Datagram datagram;
					if (queue.TryRemove(i, out datagram))
					{
						//if (Log.IsDebugEnabled)
						//	Log.DebugFormat("ACK, on datagram #{0} for {2}. Queue size={1}", i, queue.Count, player.Username);

						// RTT = RTT * 0.875 + rtt * 0.125
						// RTTVar = RTTVar * 0.875 + abs(RTT - rtt)) * 0.125
						// RTO = RTT + 4 * RTTVar
						long rtt = datagram.Timer.ElapsedMilliseconds;
						long RTT = player.Rtt;
						long RTTVar = player.RttVar;

						player.Rtt = (long) (RTT*0.875 + rtt*0.125);
						player.RttVar = (long) (RTTVar*0.875 + Math.Abs(RTT - rtt)*0.125);
						player.Rto = player.Rtt + 4*player.RttVar + 100; // SYNC time in the end

						datagram.PutPool();
					}
					else
					{
						if (Log.IsDebugEnabled)
							Log.WarnFormat("ACK, Failed to remove datagram #{0} for {2}. Queue size={1}", i, queue.Count, player.Username);
					}
				}
			}

			ack.PutPool();

			session.ResendCount = 0;
			session.WaitForAck = false;
		}

		internal void HandlePackage(Package message, PlayerNetworkSession playerSession)
		{
			if (message == null) return;

			TraceReceive(message);

			if (typeof (UnknownPackage) == message.GetType())
			{
				return;
			}

			if (typeof (McpeBatch) == message.GetType())
			{
				McpeBatch batch = (McpeBatch) message;

				var messages = new List<Package>();

				// Get bytes
				byte[] payload = batch.payload;
				// Decompress bytes

				MemoryStream stream = new MemoryStream(payload);
				if (stream.ReadByte() != 0x78)
				{
					throw new InvalidDataException("Incorrect ZLib header. Expected 0x78 0x9C");
				}
				stream.ReadByte();
				using (var defStream2 = new DeflateStream(stream, CompressionMode.Decompress, false))
				{
					// Get actual package out of bytes
					MemoryStream destination = new MemoryStream();
					defStream2.CopyTo(destination);
					destination.Position = 0;
					NbtBinaryReader reader = new NbtBinaryReader(destination, true);
					int len = reader.ReadInt32();
					byte[] internalBuffer = reader.ReadBytes(len);

					//byte[] internalBuffer = destination.ToArray();
					messages.Add(PackageFactory.CreatePackage(internalBuffer[0], internalBuffer) ?? new UnknownPackage(internalBuffer[0], internalBuffer));
					if (destination.Length > destination.Position) throw new Exception("Have more data");
				}
				foreach (var msg in messages)
				{
					msg.DatagramSequenceNumber = batch.DatagramSequenceNumber;
					msg.OrderingChannel = batch.OrderingChannel;
					msg.OrderingIndex = batch.OrderingIndex;
					HandlePackage(msg, playerSession);
					msg.PutPool();
				}

				return;
			}

			Player player = playerSession.Player;
			if (player != null) player.HandlePackage(message);
		}

		private void EnqueueAck(PlayerNetworkSession session, int sequenceNumber)
		{
			ServerInfo.NumberOfAckSent++;
			session.PlayerAckQueue.Enqueue(sequenceNumber);
		}

		private void SendAckQueue(object state)
		{
			var sessions = _playerSessions.Values.ToArray();

			foreach (var s in sessions)
			{
				ThreadPool.QueueUserWorkItem(delegate(object o)
				{
					PlayerNetworkSession session = (PlayerNetworkSession) o;
					var queue = session.PlayerAckQueue;
					int lenght = queue.Count;

					if (lenght == 0) return;

					Acks acks = Acks.CreateObject();
					for (int i = 0; i < lenght; i++)
					{
						int ack;
						if (!session.PlayerAckQueue.TryDequeue(out ack)) break;

						acks.acks.Add(ack);
					}

					if (acks.acks.Count > 0)
					{
						byte[] data = acks.Encode();
						SendData(data, session.EndPoint);
					}

					acks.PutPool();
				}, s);
			}
		}

		private object _updateGlobalLock = new object();

		private Stopwatch _forceQuitTimer = new Stopwatch();

		private void Update(object state)
		{
			if (!Monitor.TryEnter(_updateGlobalLock)) return;
			_forceQuitTimer.Restart();

			try
			{
				long now = DateTime.UtcNow.Ticks/TimeSpan.TicksPerMillisecond;

				Parallel.ForEach(_playerSessions, delegate(KeyValuePair<IPEndPoint, PlayerNetworkSession> pair)
				{
					PlayerNetworkSession session = pair.Value;

					if (session == null) return;
					if (session.Evicted) return;

					Player player = session.Player;

					long lastUpdate = session.LastUpdatedTime.Ticks/TimeSpan.TicksPerMillisecond;
					bool serverHasNoLag = ServerInfo.AvailableBytes < 1000;

					if (serverHasNoLag && lastUpdate + InacvitityTimeout + 3000 < now)
					{
						session.Evicted = true;
						// Disconnect user
						ThreadPool.QueueUserWorkItem(delegate(object o)
						{
							PlayerNetworkSession s = o as PlayerNetworkSession;
							if (s != null)
							{
								Player p = s.Player;
								if (p != null)
								{
									p.Disconnect("You've been kicked with reason: Network timeout.");
								}
								else
								{
									if (ServerInfo.PlayerSessions.TryRemove(session.EndPoint, out session))
									{
										session.Player = null;
										session.State = ConnectionState.Unconnected;
										session.Evicted = true;
										session.Clean();
									}
								}
							}
						}, session);

						return;
					}


					if (serverHasNoLag && session.State != ConnectionState.Connected && player != null && lastUpdate + 3000 < now)
					{
						ThreadPool.QueueUserWorkItem(delegate(object o)
						{
							PlayerNetworkSession s = o as PlayerNetworkSession;
							if (s != null)
							{
								Player p = s.Player;
								if (p != null) p.Disconnect("You've been kicked with reason: Lost connection.");
							}
						}, session);

						return;
					}

					if (player == null) return;

					if (serverHasNoLag && lastUpdate + InacvitityTimeout < now && !session.WaitForAck)
					{
						player.DetectLostConnection();
						session.WaitForAck = true;
					}

					if (player.Rto == 0) return;

					long rto = Math.Max(100, player.Rto);
					var queue = session.WaitingForAcksQueue;

					foreach (KeyValuePair<int, Datagram> datagramPair in queue)
					{
						// We don't do too much processing in each step, becasue one bad queue will hold the others.
						//if (_forceQuitTimer.ElapsedMilliseconds > 100)
						//{
						//	Log.WarnFormat("Update aborted early");
						//	return;
						//}

						var datagram = datagramPair.Value;

						if (!datagram.Timer.IsRunning)
						{
							Log.ErrorFormat("Timer not running for #{0}", datagram.Header.datagramSequenceNumber);
							datagram.Timer.Restart();
							continue;
						}

						if (player.Rtt == -1) return;

						//if (session.WaitForAck) return;

						long elapsedTime = datagram.Timer.ElapsedMilliseconds;
						long datagramTimout = rto*(datagram.TransmissionCount + session.ResendCount + 1);

						//if(elapsedTime > 5000)
						//{
						//	Datagram deleted;
						//	queue.TryRemove(datagram.Header.datagramSequenceNumber, out deleted);
						//}
						//else 
						if (serverHasNoLag && elapsedTime >= datagramTimout)
						{
							//if (session.WaitForAck) return;

							//session.WaitForAck = session.ResendCount++ > 3;

							Datagram deleted;
							if (queue.TryRemove(datagram.Header.datagramSequenceNumber, out deleted))
							{
								session.ErrorCount++;

								if (deleted.TransmissionCount > 3)
								{
									if (Log.IsDebugEnabled)
										Log.WarnFormat("TIMEOUT, Retransmission count remove from ACK queue #{0} Type: {2} (0x{2:x2}) for {1} ({3} > {4}) RTO {5}",
											deleted.Header.datagramSequenceNumber.IntValue(),
											player.Username,
											deleted.FirstMessageId,
											elapsedTime,
											datagramTimout,
											rto);

									deleted.PutPool();

									//session.WaitForAck = true;

									continue;
								}

								if (!session.Evicted)
								{
									ThreadPool.QueueUserWorkItem(delegate(object data)
									{
										if (Log.IsDebugEnabled)
											Log.DebugFormat("TIMEOUT, Resent #{0} Type: {2} (0x{2:x2}) for {1} ({3} > {4}) RTO {5}",
												deleted.Header.datagramSequenceNumber.IntValue(),
												player.Username,
												deleted.FirstMessageId,
												elapsedTime,
												datagramTimout,
												player.Rto);
										SendDatagram(session, (Datagram) data);
									}, datagram);
								}
							}
						}
					}
				});
			}
			finally
			{
				if (_forceQuitTimer.ElapsedMilliseconds > 100)
				{
					Log.WarnFormat("Update took unexpected long time: {0}", _forceQuitTimer.ElapsedMilliseconds);
				}

				Monitor.Exit(_updateGlobalLock);
				_cleanerTimer.Change(10, Timeout.Infinite);
			}
		}

		public void SendPackage(Player player, Package message, int mtuSize, Reliability reliability = Reliability.Reliable)
		{
			if (message == null) return;

			PlayerNetworkSession session;
			if (_playerSessions.TryGetValue(player.EndPoint, out session))
			{
				foreach (var datagram in Datagram.CreateDatagrams(message, mtuSize, session))
				{
					SendDatagram(session, datagram);
				}

				TraceSend(message);

				message.PutPool();

				//Thread.Sleep(1); // Really important to slow down speed a bit
			}
		}

		private void SendDatagram(PlayerNetworkSession session, Datagram datagram)
		{
			if (datagram.MessageParts.Count == 0)
			{
				datagram.PutPool();
				Log.WarnFormat("Failed to resend #{0}", datagram.Header.datagramSequenceNumber.IntValue());
				return;
			}

			datagram.Header.datagramSequenceNumber = Interlocked.Increment(ref session.DatagramSequenceNumber);

			datagram.TransmissionCount++;

			byte[] data = datagram.Encode();

			datagram.Timer.Restart();

			if (!session.WaitingForAcksQueue.TryAdd(datagram.Header.datagramSequenceNumber.IntValue(), datagram))
			{
				Log.Warn(string.Format("Datagram sequence unexpectedly existed in the ACK/NAK queue already {0}", datagram.Header.datagramSequenceNumber.IntValue()));
			}

			lock (session.SyncRoot)
			{
				SendData(data, session.EndPoint);
			}
		}


		private void SendData(byte[] data, IPEndPoint targetEndPoint)
		{
			try
			{
				_listener.Send(data, data.Length, targetEndPoint); // Less thread-issues it seems

				ServerInfo.NumberOfPacketsOutPerSecond++;
				ServerInfo.TotalPacketSizeOut += data.Length;
			}
			catch (ObjectDisposedException e)
			{
			}
			catch (Exception e)
			{
				//if (_listener == null || _listener.Client != null) Log.Error(string.Format("Send data lenght: {0}", data.Length), e);
			}
		}

		private static void TraceReceive(Package message, int refNumber = 0)
		{
			if (!Log.IsDebugEnabled) return;
			if (!Debugger.IsAttached) return;

			if (!(message is InternalPing) /*&& message.Id != (int) DefaultMessageIdTypes.ID_CONNECTED_PING && message.Id != (int) DefaultMessageIdTypes.ID_UNCONNECTED_PING*/)
			{
				Log.DebugFormat("> Receive: {0}: {1} (0x{0:x2}) #{2}", message.Id, message.GetType().Name, refNumber);
			}
		}

		private static void TraceSend(Package message)
		{
			if (!Log.IsDebugEnabled) return;
			if (!Debugger.IsAttached) return;

			if (!(message is InternalPing) /*&& message.Id != (int) DefaultMessageIdTypes.ID_CONNECTED_PONG && message.Id != (int) DefaultMessageIdTypes.ID_UNCONNECTED_PONG*/)
			{
				Log.DebugFormat("<    Send: {0}: {1} (0x{0:x2})", message.Id, message.GetType().Name);
			}
		}
	}
}