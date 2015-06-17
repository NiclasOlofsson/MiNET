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
		private static string Motd = string.Empty;

		private IPEndPoint _endpoint;
		private UdpClient _listener;
		private ConcurrentDictionary<IPEndPoint, PlayerNetworkSession> _playerSessions = new ConcurrentDictionary<IPEndPoint, PlayerNetworkSession>();
		private Level _level;
		private PluginManager _pluginManager;
		// ReSharper disable once NotAccessedField.Local
		private Timer _internalPingTimer;
		private Random _random = new Random();

		// ReSharper disable once NotAccessedField.Local
		private Timer _ackTimer;
		// ReSharper disable once NotAccessedField.Local
		private Timer _cleanerTimer;

		private static bool _isPerformanceTest = false;
		public UserManager<User> UserManager { get; set; }
		public RoleManager<Role> RoleManager { get; set; }

		public LevelFactory LevelFactory { get; set; }

		// Performance measures
		private long _numberOfAckSent = 0;
		private long _numberOfPacketsOutPerSecond = 0;
		private long _numberOfPacketsInPerSecond = 0;
		private long _totalPacketSizeOut = 0;
		private long _totalPacketSizeIn = 0;
		private Timer _throughPut = null;
		private long _latency = -1;
		private int _availableBytes;


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
			_endpoint = endpoint;
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
				Log.Info("Initializing...");

				Log.Info("Loading settings...");
				Motd = Config.GetProperty("motd", "Minecraft: PE Server");

				Log.Info("Loading plugins...");
				_pluginManager = new PluginManager();
				_pluginManager.LoadPlugins();
				Log.Info("Plugins loaded!");

				// Bootstrap server
				_pluginManager.ExecuteStartup(this);

				if (Config.GetProperty("EnableSecurity", false))
				{
					// http://www.asp.net/identity/overview/extensibility/overview-of-custom-storage-providers-for-aspnet-identity
					UserManager = UserManager ?? new UserManager<User>(new DefaultUserStore());
					RoleManager = RoleManager ?? new RoleManager<Role>(new DefaultRoleStore());
				}

				LevelFactory = LevelFactory ?? new LevelFactory();

				_level = LevelFactory.CreateLevel("Default");
				_levels.Add(_level);

				//for (int i = 1; i < 60; i++)
				//{
				//	Level level = LevelFactory.CreateLevel("" + i);
				//	_levels.Add(level);
				//}

				_pluginManager.EnablePlugins(_levels);

				_listener = new UdpClient(_endpoint);

				if (IsRunningOnMono())
				{
					_listener.Client.ReceiveBufferSize = 1024*1024*3;
					_listener.Client.SendBufferSize = 4096;
				}
				else
				{
					_listener.Client.ReceiveBufferSize = int.MaxValue;
					//_listener.Client.SendBufferSize = 1024*1024*8;
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

				//Task.Run(() =>
				//{
				//	while (true)
				//	{
				//		var result = _listener.ReceiveAsync();
				//		byte[] receiveBytes = result.Result.Buffer;
				//		_availableBytes = _listener.Available;
				//		_numberOfPacketsInPerSecond++;
				//		_totalPacketSizeIn += receiveBytes.Length;
				//		ThreadPool.QueueUserWorkItem(state => ProcessMessage(receiveBytes, result.Result.RemoteEndPoint));
				//	}
				//});

				_ackTimer = new Timer(SendAckQueue, null, 0, 20);
				_cleanerTimer = new Timer(Update, null, 0, 10);

				_listener.BeginReceive(ReceiveCallback, _listener);

				// Measure latency through system
				_internalPingTimer = new Timer(delegate(object state)
				{
					var playerSession = _playerSessions.Values.FirstOrDefault();
					if (playerSession != null)
					{
						var ping = new InternalPing();
						ping.Timer.Start();
						HandlePackage(ping, playerSession);
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
				if (Config.GetProperty("WorldSave", false))
				{
					Log.Info("Saving chunks...");
					_level._worldProvider.SaveChunks();
				}

				Log.Info("Disabling plugins...");
				_pluginManager.DisablePlugins();

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
				_availableBytes = listener.Available;
				_numberOfPacketsInPerSecond++;
				_totalPacketSizeIn += receiveBytes.Length;
				//ThreadPool.QueueUserWorkItem(state => ProcessMessage(receiveBytes, senderEndpoint));
				ProcessMessage(receiveBytes, senderEndpoint);
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
			byte msgId = receiveBytes[0];

			if (msgId == 0xFE)
			{
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
							//serverName = "MCCPP;MiNET - Another MC server"
							serverName = string.Format("MCPE;{0};27;0.11.1;0;20", Motd)
						};
						var data = packet.Encode();
						TraceSend(packet);
						SendData(data, senderEndpoint);
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
								PlayerNetworkSession value;
								_playerSessions.TryRemove(senderEndpoint, out value);
							}

							PlayerNetworkSession session =
								new PlayerNetworkSession(new Player(this, senderEndpoint, _levels[_random.Next(0, _levels.Count)], _pluginManager, incoming.mtuSize), senderEndpoint);
							session.LastUpdatedTime = DateTime.UtcNow;

							_playerSessions.TryAdd(senderEndpoint, session);
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
					//{
					EnqueueAck(senderEndpoint, package._datagramSequenceNumber);
					//}

					if (_playerSessions.ContainsKey(senderEndpoint))
					{
						PlayerNetworkSession playerSession = _playerSessions[senderEndpoint];


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
									playerSession.Splits[spId] = new SplitPartPackage[spCount];
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

			ack.PutPool();

			var queue = session.WaitingForAcksQueue;

			for (int i = ackSeqNo; i <= toAckSeqNo; i++)
			{
				Datagram datagram;
				if (queue.TryRemove(i, out datagram))
				{
					foreach (MessagePart part in datagram.MessageParts)
					{
						part.PutPool();
					}
					datagram.PutPool();
					Log.DebugFormat("Remove ACK #{0}. Queue size={1}", i, queue.Count);
				}
				else
				{
					Log.DebugFormat("Failed to remove ACK #{0}. Queue size={1}", i, queue.Count);
				}
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

			for (int i = ackSeqNo; i <= toAckSeqNo; i++)
			{
				Log.DebugFormat("NAK from Player {1} #{0}", i, session.Player.Username);

				Datagram datagram;
				if (queue.TryGetValue(i, out datagram))
				{
					SendDatagram(senderEndpoint, datagram, true);
					Log.DebugFormat("Resent #{0}", i);
				}
				else
				{
					Log.WarnFormat("No datagram #{0} to resend", i);
				}
			}
		}

		/// <summary>
		///     Handles the specified package.
		/// </summary>
		/// <param name="message">The package.</param>
		/// <param name="senderEndpoint">The sender's endpoint.</param>
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
			_numberOfAckSent++;

			if (_playerSessions.ContainsKey(senderEndpoint))
			{
				var session = _playerSessions[senderEndpoint];
				session.PlayerAckQueue.Enqueue(sequenceNumber.IntValue());
			}
		}

		private void Update(object state)
		{
			if (_isPerformanceTest) return;

			Parallel.ForEach(_playerSessions.Values.ToArray(), delegate(PlayerNetworkSession session)
			{
				long lastUpdate = session.LastUpdatedTime.Ticks/TimeSpan.TicksPerMillisecond;
				long now = DateTime.UtcNow.Ticks/TimeSpan.TicksPerMillisecond;
				if (lastUpdate + 10000 < now)
				{
					// Disconnect user
					HandlePackage(new DisconnectionNotification(), session);

					return;
				}
				else if (lastUpdate + 8500 < now)
				{
					session.Player.DetectLostConnection();
				}

				var queue = session.WaitingForAcksQueue;

				foreach (var datagram in queue.Values)
				{
					if (!datagram.Timer.IsRunning)
					{
						Log.DebugFormat("Timer not running for #{0}", datagram.Header.datagramSequenceNumber);
					}

					if (datagram.Timer.ElapsedMilliseconds > 5000)
					{
						Datagram deleted;
						if (queue.TryRemove(datagram.Header.datagramSequenceNumber, out deleted))
						{
							Log.DebugFormat("Cleaned #{0}", deleted.Header.datagramSequenceNumber.IntValue());
							foreach (MessagePart part in deleted.MessageParts)
							{
								part.PutPool();
							}
							deleted.PutPool();
						}
					}
				}
			});
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
					SendData(data, session.EndPoint);
				}

				acks.PutPool();
			});
		}

		public void SendPackage(IPEndPoint senderEndpoint, List<Package> messages, short mtuSize, ref int datagramSequenceNumber, ref int reliableMessageNumber, Reliability reliability = Reliability.Reliable)
		{
			if (messages.Count == 0) return;

			Datagram.CreateDatagrams(messages, mtuSize, ref datagramSequenceNumber, ref reliableMessageNumber, senderEndpoint, SendDatagram);

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

		private void SendDatagram(IPEndPoint senderEndpoint, Datagram datagram)
		{
			SendDatagram(senderEndpoint, datagram, false);
		}


		private void SendDatagram(IPEndPoint senderEndpoint, Datagram datagram, bool isResend)
		{
			if (datagram.MessageParts.Count != 0)
			{
				byte[] data = datagram.Encode();

				datagram.Timer.Restart();
				SendData(data, senderEndpoint);

				if (_playerSessions.ContainsKey(senderEndpoint) && !isResend && !_isPerformanceTest)
				{
					PlayerNetworkSession session = _playerSessions[senderEndpoint];
					session.WaitingForAcksQueue.TryAdd(datagram.Header.datagramSequenceNumber, datagram);
				}
			}

			if (_isPerformanceTest)
			{
				foreach (MessagePart part in datagram.MessageParts)
				{
					part.PutPool();
				}

				datagram.PutPool();
			}
		}


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

			//_listener.SendAsync(data, data.Length, targetEndpoint).Wait(); // Has thread pooling issues?
			_listener.Send(data, data.Length, targetEndpoint); // Less thread-issues it seems

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