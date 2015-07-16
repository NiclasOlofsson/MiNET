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
using MiNET.Worlds;

namespace MiNET
{
	public class MiNetServer
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (MiNetServer));

		private const int DefaultPort = 19132;

		private IPEndPoint _endpoint;
		private UdpClient _listener;
		private ConcurrentDictionary<IPEndPoint, PlayerNetworkSession> _playerSessions = new ConcurrentDictionary<IPEndPoint, PlayerNetworkSession>();
		private Level _level;


		public bool ForwardAllPlayers { get; set; }
		public IPEndPoint ForwardTarget { get; set; }

		public MotdProvider MotdProvider { get; set; }

		public bool IsSecurityEnabled { get; private set; }
		public UserManager<User> UserManager { get; set; }
		public RoleManager<Role> RoleManager { get; set; }

		public LevelFactory LevelFactory { get; set; }
		public PlayerFactory PlayerFactory { get; set; }

		public PluginManager PluginManager { get; set; }
		public SessionManager SessionManager { get; set; }

		private Random _random = new Random();

		private static bool _isPerformanceTest = false;

		private Timer _internalPingTimer;
		private Timer _ackTimer;
		private Timer _cleanerTimer;

		private List<Level> _levels = new List<Level>();

		public ServerInfo ServerInfo { get; set; }
		public int MaxNumberOfPlayers { get; set; }
		public int MaxNumberOfConcurrentConnects { get; set; }

		public MiNetServer()
		{
		}

		public MiNetServer(IPEndPoint endpoint)
		{
			_endpoint = endpoint;
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

				MaxNumberOfPlayers = Config.GetProperty("MaxNumberOfPlayers", 100);
				MaxNumberOfConcurrentConnects = Config.GetProperty("MaxNumberOfConcurrentConnects", 4);

				if (_endpoint == null)
				{
					var ip = IPAddress.Parse(Config.GetProperty("ip", "0.0.0.0"));
					int port = Config.GetProperty("port", 19132);
					_endpoint = new IPEndPoint(ip, port);
				}

				ForwardAllPlayers = Config.GetProperty("ForwardAllPlayers", false);
				if (ForwardAllPlayers)
				{
					var ip = IPAddress.Parse(Config.GetProperty("ForwardIP", "127.0.0.1"));
					int port = Config.GetProperty("ForwardPort", 19132);
					_endpoint = new IPEndPoint(ip, port);
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

				SessionManager = SessionManager ?? new SessionManager();
				LevelFactory = LevelFactory ?? new LevelFactory();
				PlayerFactory = PlayerFactory ?? new PlayerFactory();

				_level = LevelFactory.CreateLevel("Default");
				_levels.Add(_level);

				ServerInfo = new ServerInfo(_level, _playerSessions);

				//for (int i = 1; i < 10; i++)
				//{
				//	Level level = LevelFactory.CreateLevel("" + i);
				//	_levels.Add(level);
				//}

				PluginManager.EnablePlugins(this, _levels);

				_listener = new UdpClient(_endpoint);

				if (IsRunningOnMono())
				{
					_listener.Client.ReceiveBufferSize = 1024*1024*3;
					_listener.Client.SendBufferSize = 4096;
				}
				else
				{
					//_listener.Client.ReceiveBufferSize = 1600*500;
					//_listener.Client.ReceiveBufferSize = 1024 * 1024 * 3;
					_listener.Client.ReceiveBufferSize = int.MaxValue;
					//_listener.Client.SendBufferSize = 1024 * 1024 * 8;
					_listener.Client.SendBufferSize = int.MaxValue;
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

				_ackTimer = new Timer(SendAckQueue, null, 0, 20);
				_cleanerTimer = new Timer(Update, null, 0, 10);

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

		public bool StopServer()
		{
			try
			{
				if (Config.GetProperty("WorldSave", false))
				{
					Log.Info("Saving chunks...");
					_level._worldProvider.SaveChunks();
				}

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
				if (listener.Client != null)
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
				}

				return;
			}

			if (receiveBytes.Length != 0)
			{
				listener.BeginReceive(ReceiveCallback, listener);
				ServerInfo.AvailableBytes = listener.Available;
				ServerInfo.NumberOfPacketsInPerSecond++;
				ServerInfo.TotalPacketSizeIn += receiveBytes.Length;
				//ThreadPool.QueueUserWorkItem(state => ProcessMessage(receiveBytes, senderEndpoint));
				try
				{
					ProcessMessage(receiveBytes, senderEndpoint);
				}
				catch (Exception e)
				{
					Log.Warn("Process message error", e);
				}
			}
			else
			{
				Log.Debug("Unexpected end of transmission?");
			}
		}

		private void ProcessMessage(byte[] receiveBytes, IPEndPoint senderEndpoint)
		{
			byte msgId = receiveBytes[0];

			if (msgId == 0xFE)
			{
				Log.WarnFormat("A query detected from: {0}", senderEndpoint.Address);
				HandleQuery(receiveBytes, senderEndpoint);
			}
			else if (msgId <= (byte) DefaultMessageIdTypes.ID_USER_PACKET_ENUM)
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
							serverId = 22345,
							pingId = incoming.pingId /*incoming.pingId*/,
							serverName = MotdProvider.GetMotd(ServerInfo)
						};
						var data = packet.Encode();
						TraceSend(packet);
						SendData(data, senderEndpoint, new object());
						break;
					}
					case DefaultMessageIdTypes.ID_OPEN_CONNECTION_REQUEST_1:
					{
						OpenConnectionRequest1 incoming = (OpenConnectionRequest1) message;

						_isPerformanceTest = _isPerformanceTest || incoming.raknetProtocolVersion == byte.MaxValue;

						var packet = new OpenConnectionReply1
						{
							serverGuid = 12345,
							mtuSize = incoming.mtuSize,
							serverHasSecurity = 0
						};

						var data = packet.Encode();
						TraceSend(packet);
						SendData(data, senderEndpoint, new object());
						break;
					}
					case DefaultMessageIdTypes.ID_OPEN_CONNECTION_REQUEST_2:
					{
						OpenConnectionRequest2 incoming = (OpenConnectionRequest2) message;

						Log.DebugFormat("New connection from: {0} {1}", senderEndpoint.Address, incoming.clientUdpPort);

						var packet = new OpenConnectionReply2
						{
							serverGuid = 12345,
							mtuSize = incoming.mtuSize,
							doSecurityAndHandshake = new byte[0]
						};

						PlayerNetworkSession session = null;
						lock (_playerSessions)
						{
							if (_playerSessions.ContainsKey(senderEndpoint))
							{
								PlayerNetworkSession value;
								_playerSessions.TryRemove(senderEndpoint, out value);
								value.Player.HandleDisconnectionNotification();
								Log.DebugFormat("Removed ghost");
							}

							Player player = PlayerFactory.CreatePlayer(this, senderEndpoint, _levels[_random.Next(0, _levels.Count)], incoming.mtuSize);
							session = new PlayerNetworkSession(player, senderEndpoint);
							session.LastUpdatedTime = DateTime.UtcNow;
							session.Mtuize = incoming.mtuSize;

							_playerSessions.TryAdd(senderEndpoint, session);
						}


						var data = packet.Encode();
						TraceSend(packet);
						SendData(data, senderEndpoint, session.SyncRoot);
						break;
					}
					default:
						Log.ErrorFormat("Receive unexpected packet with ID: {0} (0x{0:x2}) from {1}", msgId, senderEndpoint.Address);
						break;
				}

				if (message != null)
				{
					message.PutPool();
				}
				else
				{
					Log.ErrorFormat("Receive unexpected packet with ID: {0} (0x{0:x2}) from {1}", msgId, senderEndpoint.Address);
				}
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

					ConnectedPackage package = ConnectedPackage.CreateObject();
					package.Decode(receiveBytes);
					List<Package> messages = package.Messages;

					if (messages.Count == 1)
					{
						McpeBatch batch = messages.First() as McpeBatch;
						if (batch != null)
						{
							messages = new List<Package>();

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
								byte[] internalBuffer = destination.ToArray();
								messages.Add(PackageFactory.CreatePackage(internalBuffer[0], internalBuffer) ?? new UnknownPackage(internalBuffer[0], internalBuffer));
							}
						}
					}

					// IF reliable code below is enabled, useItem start sending doubles
					// for some unknown reason.

					//Reliability reliability = package._reliability;
					//if (reliability == Reliability.Reliable
					//	|| reliability == Reliability.ReliableSequenced
					//	|| reliability == Reliability.ReliableOrdered
					//	)
					{
						EnqueueAck(senderEndpoint, package._datagramSequenceNumber);
					}

					PlayerNetworkSession playerSession;
					if (_playerSessions.TryGetValue(senderEndpoint, out playerSession))
					{
						if (ForwardAllPlayers)
						{
							playerSession.Player.SendPackage(new McpeTransfer
							{
								endpoint = ForwardTarget
							}, true);

							return;
						}

						if (package._datagramSequenceNumber.IntValue() > 0 && playerSession.LastDatagramNumber >= package._datagramSequenceNumber.IntValue())
						{
							Log.WarnFormat("Sequence out of order {0}", package._datagramSequenceNumber.IntValue());
						}

						playerSession.LastDatagramNumber = header.datagramSequenceNumber;
						foreach (var message in messages)
						{
							if (message is SplitPartPackage)
							{
								SplitPartPackage splitMessage = message as SplitPartPackage;

								int spId = package._splitPacketId;
								int spIdx = package._splitPacketIndex;
								int spCount = package._splitPacketCount;

								if (!playerSession.Splits.ContainsKey(spId))
								{
									playerSession.Splits.Add(spId, new SplitPartPackage[spCount]);
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

									MemoryStream stream = new MemoryStream();
									for (int i = 0; i < spPackets.Length; i++)
									{
										byte[] buf = spPackets[i].Message;
										stream.Write(buf, 0, buf.Length);
									}

									byte[] buffer = stream.ToArray();
									var fullMessage = PackageFactory.CreatePackage(buffer[0], buffer) ?? new UnknownPackage(buffer[0], buffer);
									HandlePackage(fullMessage, playerSession);
								}
								continue;
							}

							message.Timer.Restart();
							HandlePackage(message, playerSession);
							message.PutPool();
						}
					}

					package.PutPool();
				}
				else if (header.isACK && header.isValid)
				{
					HandleAck(receiveBytes, senderEndpoint);
				}
				else if (header.isNAK && header.isValid)
				{
					HandleNak(receiveBytes, senderEndpoint);
				}
				else if (!header.isValid)
				{
					Log.Warn("!!!! ERROR, Invalid header !!!!!");
				}
			}
		}

		private void HandleQuery(byte[] receiveBytes, IPEndPoint senderEndpoint)
		{
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
					Log.InfoFormat("Full request: {0}", isFullStatRequest);

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

					var data = new Dictionary<string, string>
					{
						{"splitnum", "" + (char) 128},
						{"hostname", "Personal Minecraft Server"},
						{"gametype", "SMP"},
						{"game_id", "MINECRAFTPE"},
						{"version", "0.10.5 alpha"},
						{"server_engine", "MiNET v0.0.0"},
						{"plugins", "MiNET v0.0.0"},
						{"map", "world"},
						{"numplayers", _playerSessions.Count.ToString(CultureInfo.InvariantCulture)},
						{"maxplayers", "4000"},
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

		private void HandleNak(byte[] receiveBytes, IPEndPoint senderEndpoint)
		{
			PlayerNetworkSession session;
			if (!_playerSessions.TryGetValue(senderEndpoint, out session)) return;

			Nak nak = Nak.CreateObject();
			nak.Decode(receiveBytes);

			int ackSeqNo = nak.sequenceNumber.IntValue();
			int toAckSeqNo = nak.toSequenceNumber.IntValue();
			if (nak.onlyOneSequence == 1) toAckSeqNo = ackSeqNo;

			nak.PutPool();

			var queue = session.WaitingForAcksQueue;

			Log.DebugFormat("NAK from Player {0} ({5}) #{1}-{2} IsOnlyOne {3} Count={4}", session.Player.Username, ackSeqNo, toAckSeqNo, nak.onlyOneSequence, nak.count, session.Player.Rtt);

			for (int i = ackSeqNo; i <= toAckSeqNo; i++)
			{
				session.ErrorCount++;

				Datagram datagram;
				if (queue.TryRemove(i, out datagram))
				{
					// RTT = RTT * 0.875 + rtt * 0.125
					// RTTVar = RTTVar * 0.875 + abs(RTT - rtt)) * 0.125
					// RTO = RTT + 4 * RTTVar
					long rtt = datagram.Timer.ElapsedMilliseconds;
					long RTT = session.Player.Rtt;
					long RTTVar = session.Player.RttVar;

					session.Player.Rtt = (long) (RTT*0.875 + rtt*0.125);
					session.Player.RttVar = (long) (RTTVar*0.875 + Math.Abs(RTT - rtt)*0.125);
					session.Player.Rto = session.Player.Rtt + 4*session.Player.RttVar + 10; // SYNC time in the end
					SendDatagram(senderEndpoint, datagram);
					Thread.Sleep(12);

					//ThreadPool.QueueUserWorkItem(delegate(object data) { SendDatagram(senderEndpoint, (Datagram)data); }, datagram);
				}
				else
				{
					Log.DebugFormat("No datagram #{0} to resend for {1}", i, session.Player.Username);
				}
			}
		}

		private void HandleAck(byte[] receiveBytes, IPEndPoint senderEndpoint)
		{
			PlayerNetworkSession session;
			if (!_playerSessions.TryGetValue(senderEndpoint, out session)) return;

			session.LastUpdatedTime = DateTime.UtcNow;

			Ack ack = Ack.CreateObject();
			ack.Decode(receiveBytes);

			int ackSeqNo = ack.sequenceNumber.IntValue();
			int toAckSeqNo = ack.toSequenceNumber.IntValue();
			if (ack.onlyOneSequence == 1) toAckSeqNo = ackSeqNo;

			if (ack.onlyOneSequence != 1 && ack.count > 2)
			{
				Log.DebugFormat("ACK from Player {0} ({5}) #{1}-{2} IsOnlyOne {3} Count={4}", session.Player.Username, ackSeqNo, toAckSeqNo, ack.onlyOneSequence, ack.count, session.Player.Rtt);
			}

			ack.PutPool();

			var queue = session.WaitingForAcksQueue;

			for (int i = ackSeqNo; i <= toAckSeqNo; i++)
			{
				Datagram datagram;
				if (queue.TryRemove(i, out datagram))
				{
					// RTT = RTT * 0.875 + rtt * 0.125
					// RTTVar = RTTVar * 0.875 + abs(RTT - rtt)) * 0.125
					// RTO = RTT + 4 * RTTVar
					long rtt = datagram.Timer.ElapsedMilliseconds;
					long RTT = session.Player.Rtt;
					long RTTVar = session.Player.RttVar;

					session.Player.Rtt = (long) (RTT*0.875 + rtt*0.125);
					session.Player.RttVar = (long) (RTTVar*0.875 + Math.Abs(RTT - rtt)*0.125);
					session.Player.Rto = session.Player.Rtt + 4*session.Player.RttVar + 100; // SYNC time in the end

					foreach (MessagePart part in datagram.MessageParts)
					{
						part.PutPool();
					}
					datagram.PutPool();
				}
				else
				{
					Log.DebugFormat("Failed to remove ACK #{0} for {2}. Queue size={1}", i, queue.Count, session.Player.Username);
				}
			}
		}

		private void HandlePackage(Package message, PlayerNetworkSession playerSession)
		{
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
					byte[] internalBuffer = destination.ToArray();
					messages.Add(PackageFactory.CreatePackage(internalBuffer[0], internalBuffer) ?? new UnknownPackage(internalBuffer[0], internalBuffer));
				}
				foreach (var msg in messages)
				{
					HandlePackage(msg, playerSession);
				}
			}

			playerSession.Player.HandlePackage(message);
			playerSession.LastUpdatedTime = DateTime.UtcNow;

			if (typeof (DisconnectionNotification) == message.GetType())
			{
				PlayerNetworkSession value;
				_playerSessions.TryRemove(playerSession.EndPoint, out value);
			}
		}

		private void EnqueueAck(IPEndPoint senderEndpoint, Int24 sequenceNumber)
		{
			ServerInfo.NumberOfAckSent++;

			if (_playerSessions.ContainsKey(senderEndpoint))
			{
				var session = _playerSessions[senderEndpoint];
				session.PlayerAckQueue.Enqueue(sequenceNumber.IntValue());
			}
		}

		private void SendAckQueue(object state)
		{
			Parallel.ForEach(_playerSessions.Values.ToArray(), delegate(PlayerNetworkSession session)
			{
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
					SendData(data, session.EndPoint, session.SyncRoot);
				}

				acks.PutPool();
			});
		}

		private void Update(object state)
		{
			long now = DateTime.UtcNow.Ticks/TimeSpan.TicksPerMillisecond;

			Parallel.ForEach(_playerSessions.Values.ToArray(), delegate(PlayerNetworkSession session)
			{
				if (!session.Player.IsBot)
				{
					//if (session.ErrorCount > 1000 /* && session.SendDelay >= 50*/)
					//{
					//	Log.WarnFormat("Kick #{0} for too many errors.", session.Player.Username);
					//	session.Player.SendPackage(new McpeDisconnect() { message = "You've been kicked with reason: Too slow client." });
					//	HandlePackage(new DisconnectionNotification(), session);
					//	return;
					//}

					long lastUpdate = session.LastUpdatedTime.Ticks/TimeSpan.TicksPerMillisecond;
					if (lastUpdate + 10000 < now)
					{
						// Disconnect user
						session.Player.SendPackage(new McpeDisconnect() {message = "You've been kicked with reason: Inactivity."});
						HandlePackage(new DisconnectionNotification(), session);

						return;
					}
					else if (lastUpdate + 8500 < now)
					{
						session.Player.DetectLostConnection();
					}
				}
				//var queue = session.WaitingForAcksQueue;
				//foreach (var datagram in queue.Values)
				//{
				//	if (!datagram.Timer.IsRunning)
				//	{
				//		Log.ErrorFormat("Timer not running for #{0}", datagram.Header.datagramSequenceNumber);
				//		continue;
				//	}

				//	if(session.Player.Rtt == -1) continue;

				//	long elapsedTime = datagram.Timer.ElapsedMilliseconds;
				//	long rto = Math.Max(100, session.Player.Rto);
				//	if (elapsedTime >= rto * (datagram.TransmissionCount))
				//	{
				//		Datagram deleted;
				//		if (queue.TryRemove(datagram.Header.datagramSequenceNumber, out deleted))
				//		{
				//			session.ErrorCount++;

				//			if (deleted.TransmissionCount > 2)
				//			{
				//				foreach (MessagePart part in deleted.MessageParts)
				//				{
				//					part.PutPool();
				//				}
				//				deleted.PutPool();

				//				continue;
				//			}

				//			//ThreadPool.QueueUserWorkItem(delegate(object data)
				//			//{
				//			//	Log.WarnFormat("Resent #{0} Type: {2} (0x{2:x2}) for {1} ({3} > {4}) RTT {5}",
				//			//		deleted.Header.datagramSequenceNumber.IntValue(),
				//			//		session.Player.Username,
				//			//		deleted.FirstMessageId,
				//			//		elapsedTime,
				//			//		rto,
				//			//		session.Player.Rtt);
				//			//	//SendDatagram(session.EndPoint, (Datagram) data);
				//			//}, datagram);
				//		}
				//	}
				//}
			});
		}

		public void SendPackage(Player player, List<Package> messages, int mtuSize, ref int reliableMessageNumber, Reliability reliability = Reliability.Reliable)
		{
			if (messages.Count == 0) return;

			Datagram.CreateDatagrams(messages, mtuSize, ref reliableMessageNumber, player.EndPoint, SendDatagram);

			PlayerNetworkSession session;
			if (_playerSessions.TryGetValue(player.EndPoint, out session))
			{
				foreach (var message in messages)
				{
					//if (!player.IsBot) Thread.Sleep(7); // 7 is a good value
					if (!player.IsBot)
					{
						if (session.ErrorCount > 50)
						{
							if (!session.IsSlowClient)
							{
								session.IsSlowClient = true;
								//player.Level.BroadcastTextMessage("Slow client detected, slowing down speed for " + player.Username, type: MessageType.Raw);
							}

							// This is to slow down chunk-sending not to overrun old devices.
							// The timeout should be configurable and enable/disable.
							if (Math.Floor(player.Rtt/10d) > 0)
							{
								Thread.Sleep(Math.Min(Math.Max((int) Math.Floor(player.Rtt/10d), 10), 20));
							}
						}
						else
						{
							Thread.Sleep(10); // Seems to be needed regardless :-(
						}
					}

					if (message is InternalPing)
					{
						ServerInfo.Latency = (ServerInfo.Latency*9 + message.Timer.ElapsedMilliseconds)/10;
					}

					TraceSend(message);

					message.PutPool();
				}
			}
		}

		private void SendDatagram(IPEndPoint senderEndpoint, Datagram datagram)
		{
			PlayerNetworkSession session;
			if (_playerSessions.TryGetValue(senderEndpoint, out session))
			{
				if (datagram.MessageParts.Count != 0)
				{
					//if (!isResend)
					{
						datagram.Header.datagramSequenceNumber = Interlocked.Increment(ref session.DatagramSequenceNumber);
					}

					byte[] data = datagram.Encode();

					if (!session.Player.IsBot)
					{
						if (!session.WaitingForAcksQueue.TryAdd(datagram.Header.datagramSequenceNumber.IntValue(), datagram))
						{
							Log.Warn(string.Format("Datagram sequence unexpectedly existed in the ACK/NAK queue already {0}", datagram.Header.datagramSequenceNumber.IntValue()));
						}
					}

					datagram.TransmissionCount++;

					SendData(data, senderEndpoint, session.SyncRoot);

					datagram.Timer.Restart();
				}

				if (session.Player.IsBot)
				{
					foreach (MessagePart part in datagram.MessageParts)
					{
						part.PutPool();
					}

					datagram.PutPool();
				}
			}
		}


		private void SendData(byte[] data, IPEndPoint targetEndPoint, object syncRoot)
		{
			lock (syncRoot)
			{
				_listener.Send(data, data.Length, targetEndPoint); // Less thread-issues it seems
			}

			ServerInfo.NumberOfPacketsOutPerSecond++;
			ServerInfo.TotalPacketSizeOut += data.Length;
		}

		private static void TraceReceive(Package message, int refNumber = 0)
		{
			if (_isPerformanceTest || !Debugger.IsAttached || !Log.IsDebugEnabled) return;

			if (!(message is InternalPing) /*&& message.Id != (int) DefaultMessageIdTypes.ID_CONNECTED_PING && message.Id != (int) DefaultMessageIdTypes.ID_UNCONNECTED_PING*/)
			{
				Log.DebugFormat("> Receive: {0}: {1} (0x{0:x2}) #{2}", message.Id, message.GetType().Name, refNumber);
			}
		}

		private static void TraceSend(Package message)
		{
			if (_isPerformanceTest || !Debugger.IsAttached || !Log.IsDebugEnabled) return;

			if (!(message is InternalPing) /*&& message.Id != (int) DefaultMessageIdTypes.ID_CONNECTED_PONG && message.Id != (int) DefaultMessageIdTypes.ID_UNCONNECTED_PONG*/)
			{
				Log.DebugFormat("<    Send: {0}: {1} (0x{0:x2})", message.Id, message.GetType().Name);
			}
		}
	}
}