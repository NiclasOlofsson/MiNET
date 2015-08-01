using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using log4net;
using log4net.Config;
using MiNET.Net;
using MiNET.Utils;
using MiNET.Worlds;

[assembly: XmlConfigurator(Watch = true)]
// This will cause log4net to look for a configuration file
// called TestApp.exe.config in the application base
// directory (i.e. the directory containing TestApp.exe)
// The config file will be watched for changes.

namespace MiNET.Client
{
	public class MiNetClient
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (MiNetServer));

		private IPEndPoint _clientEndpoint;
		private IPEndPoint _serverTargetEndpoint;
		private short _mtuSize = 1447;
		private int _reliableMessageNumber = 1;
		private Vector3 _spawn;
		private long _entityId;
		public PlayerNetworkSession Session { get; set; }


		private LevelInfo _level = new LevelInfo();
		public PlayerLocation CurrentLocation { get; set; }

		public UdpClient Listener { get; private set; }

		public string Username { get; set; }
		public int ClientId { get; set; }

		public MiNetClient(IPEndPoint targetEndpoint, IPEndPoint clientEndpoint = null)
		{
			_serverTargetEndpoint = targetEndpoint;
			_clientEndpoint = clientEndpoint ?? new IPEndPoint(IPAddress.Any, 19132);
		}

		public static bool IsRunningOnMono()
		{
			return Type.GetType("Mono.Runtime") != null;
		}

		public bool StartServer()
		{
			if (Listener != null) return false; // Already started

			try
			{
				Log.Info("Initializing...");

				Listener = new UdpClient(_clientEndpoint);

				if (IsRunningOnMono())
				{
					Listener.Client.ReceiveBufferSize = 1024*1024*3;
					Listener.Client.SendBufferSize = 4096;
				}
				else
				{
					Listener.Client.ReceiveBufferSize = int.MaxValue;
					Listener.Client.SendBufferSize = int.MaxValue;
					Listener.DontFragment = false;

					// SIO_UDP_CONNRESET (opcode setting: I, T==3)
					// Windows:  Controls whether UDP PORT_UNREACHABLE messages are reported.
					// - Set to TRUE to enable reporting.
					// - Set to FALSE to disable reporting.

					uint IOC_IN = 0x80000000;
					uint IOC_VENDOR = 0x18000000;
					uint SIO_UDP_CONNRESET = IOC_IN | IOC_VENDOR | 12;
					Listener.Client.IOControl((int) SIO_UDP_CONNRESET, new byte[] {Convert.ToByte(false)}, null);

					////
					////WARNING: We need to catch errors here to remove the code above.
					////
				}

				Session = new PlayerNetworkSession(null, _clientEndpoint);

				Listener.BeginReceive(ReceiveCallback, Listener);

				Log.InfoFormat("Server open for business for {0}", Username);

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
				if (Listener == null) return true; // Already stopped. It's ok.

				Listener.Close();
				Listener = null;

				Log.InfoFormat("Client closed for business {0}", Username);

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

			if (listener.Client == null) return;

			// WSAECONNRESET:
			// The virtual circuit was reset by the remote side executing a hard or abortive close. 
			// The application should close the socket; it is no longer usable. On a UDP-datagram socket 
			// this error indicates a previous send operation resulted in an ICMP Port Unreachable message.
			// Note the spocket settings on creation of the server. It makes us ignore these resets.
			IPEndPoint senderEndpoint = new IPEndPoint(0, 0);
			Byte[] receiveBytes;
			try
			{
				receiveBytes = listener.EndReceive(ar, ref senderEndpoint);
			}
			catch (Exception e)
			{
				if (listener.Client == null) return;
				Log.Debug(e);
				try
				{
					listener.BeginReceive(ReceiveCallback, listener);
				}
				catch (ObjectDisposedException dex)
				{
					// Log and move on. Should probably free up the player and remove them here.
					Log.Debug(dex);
				}

				return;
			}

			if (receiveBytes.Length != 0)
			{
				if (listener.Client == null) return;
				listener.BeginReceive(ReceiveCallback, listener);

				if (listener.Client == null) return;
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

			if (msgId <= (byte) DefaultMessageIdTypes.ID_USER_PACKET_ENUM)
			{
				DefaultMessageIdTypes msgIdType = (DefaultMessageIdTypes) msgId;

				Package message = PackageFactory.CreatePackage(msgId, receiveBytes);

				if (message == null) return;

				TraceReceive(message);

				switch (msgIdType)
				{
					case DefaultMessageIdTypes.ID_UNCONNECTED_PONG:
					{
						UnconnectedPong incoming = (UnconnectedPong) message;
						SendOpenConnectionRequest1();

						break;
					}
					case DefaultMessageIdTypes.ID_OPEN_CONNECTION_REPLY_1:
					{
						OpenConnectionReply1 incoming = (OpenConnectionReply1) message;
						//if (incoming.mtuSize < _mtuSize) throw new Exception("Error:" + incoming.mtuSize);
						SendOpenConnectionRequest2();
						break;
					}
					case DefaultMessageIdTypes.ID_OPEN_CONNECTION_REPLY_2:
					{
						OpenConnectionReply2 incoming = (OpenConnectionReply2) message;
						//_mtuSize = incoming.mtuSize;
						SendConnectionRequest();
						break;
					}
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
					Log.Debug(">\tReceive Datagram #" + package._datagramSequenceNumber.IntValue());

					var messages = package.Messages;


					//Reliability reliability = package._reliability;
					//if (reliability == Reliability.Reliable
					//	|| reliability == Reliability.ReliableSequenced
					//	|| reliability == Reliability.ReliableOrdered
					//	)
					{
						// Send ACK
						Acks ack = new Acks();
						ack.acks.Add(package._datagramSequenceNumber.IntValue());
						byte[] data = ack.Encode();
						SendData(data, senderEndpoint);
					}

					if (LoginSent) return; //HACK

					foreach (var message in messages)
					{
						if (message is SplitPartPackage)
						{
							var splits = Session.Splits;
							lock (splits)
							{
								SplitPartPackage splitMessage = message as SplitPartPackage;

								int spId = package._splitPacketId;
								int spIdx = package._splitPacketIndex;
								int spCount = package._splitPacketCount;

								Log.DebugFormat("Got split package {2} (of {0}) for split ID: {1}", spCount, spId, spIdx);

								if (!splits.ContainsKey(spId))
								{
									splits.Add(spId, new SplitPartPackage[spCount]);
								}
								else
								{
									Log.DebugFormat("Resent split package {2} (of {0}) for split ID: {1}", spCount, spId, spIdx);
								}

								SplitPartPackage[] spPackets = splits[spId];
								if (spIdx < 0 || spIdx >= spPackets.Length)
								{
									Log.DebugFormat("Unexpeted split package {2} (of {0}) for split ID: {1}", spCount, spId, spIdx);
									return;
								}
								spPackets[spIdx] = splitMessage;

								bool haveEmpty = false;
								for (int i = 0; i < spPackets.Length; i++)
								{
									haveEmpty = haveEmpty || spPackets[i] == null;
								}

								if (!haveEmpty)
								{
									//Log.WarnFormat("Got all {0} split packages for split ID: {1}", spCount, spId);

									MemoryStream stream = new MemoryStream();
									for (int i = 0; i < spPackets.Length; i++)
									{
										byte[] buf = spPackets[i].Message;
										stream.Write(buf, 0, buf.Length);
									}

									try
									{
										byte[] buffer = stream.ToArray();
										var fullMessage = PackageFactory.CreatePackage(buffer[0], buffer) ?? new UnknownPackage(buffer[0], buffer);
										Log.Debug("Processing split-message");
										HandlePackage(fullMessage, senderEndpoint);
										fullMessage.PutPool();
									}
									catch (Exception e)
									{
										Log.Debug("When processing split-message", e);
									}
								}

								message.PutPool();
								return;
							}
						}

						{
							message.Timer.Restart();
							HandlePackage(message, senderEndpoint);
							message.PutPool();
						}
					}

					//package.PutPool();
				}
				else if (header.isPacketPair)
				{
					Log.Warn("header.isPacketPair");
				}
				else if (header.isACK && header.isValid)
				{
					HandleAck(receiveBytes, senderEndpoint);
				}
				else if (header.isNAK && header.isValid)
				{
					Nak nak = new Nak();
					nak.Decode(receiveBytes);

					Log.Warn("!!!! NAK !!!!!" + nak.sequenceNumber.IntValue());
					HandleNak(receiveBytes, senderEndpoint);
				}
				else if (!header.isValid)
				{
					Log.Warn("!!!! ERROR, Invalid header !!!!!");
				}
				else
				{
					Log.Warn("!! WHAT THE F");
				}
			}
		}

		private void HandleAck(byte[] receiveBytes, IPEndPoint senderEndpoint)
		{
		}

		private void HandleNak(byte[] receiveBytes, IPEndPoint senderEndpoint)
		{
			Log.Warn("!! WHAT THE F NAK NAK NAK");
		}

		/// <summary>
		///     Handles the specified package.
		/// </summary>
		/// <param name="message">The package.</param>
		/// <param name="senderEndpoint">The sender's endpoint.</param>
		private void HandlePackage(Package message, IPEndPoint senderEndpoint)
		{
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
					HandlePackage(msg, senderEndpoint);
					msg.PutPool();
				}
				return;
			}

			TraceReceive(message);

			if (typeof (UnknownPackage) == message.GetType())
			{
				return;
			}

			if (typeof (McpeDisconnect) == message.GetType())
			{
				McpeDisconnect msg = (McpeDisconnect) message;
				Log.InfoFormat("Disconnect {1}: {0}", msg.message, Username);
				SendDisconnectionNotification();
				StopServer();
				return;
			}

			if (typeof (ConnectedPing) == message.GetType())
			{
				ConnectedPing msg = (ConnectedPing) message;
				SendConnectedPong(msg.sendpingtime);
				return;
			}

			if (typeof (McpeFullChunkData) == message.GetType())
			{
				//McpeFullChunkData msg = (McpeFullChunkData) message;
				//ChunkColumn chunk = ClientUtils.DecocedChunkColumn(msg.chunkData);
				//if (chunk != null)
				//{
				//	Log.DebugFormat("Chunk X={0}", chunk.x);
				//	Log.DebugFormat("Chunk Z={0}", chunk.z);

				//	//ClientUtils.SaveChunkToAnvil(chunk);
				//}
				return;
			}

			if (typeof (ConnectionRequestAccepted) == message.GetType())
			{
				Thread.Sleep(150);
				SendNewIncomingConnection();
				var t1 = new Timer(state => SendConnectedPing(), null, 2000, 5000);
				Thread.Sleep(50);
				SendLogin(Username);
				return;
			}

			if (typeof (McpeSetSpawnPosition) == message.GetType())
			{
				McpeSetSpawnPosition msg = (McpeSetSpawnPosition) message;
				_spawn = new Vector3(msg.x, msg.y, msg.z);
				_level.SpawnX = (int) _spawn.X;
				_level.SpawnY = (int) _spawn.Y;
				_level.SpawnZ = (int) _spawn.Z;

				return;
			}

			if (typeof (McpeStartGame) == message.GetType())
			{
				McpeStartGame msg = (McpeStartGame) message;
				_entityId = msg.entityId;
				_spawn = new Vector3(msg.x, msg.y, msg.z);
				_level.LevelName = "Default";
				_level.Version = 19133;
				_level.GameType = msg.gamemode;

				//ClientUtils.SaveLevel(_level);

				return;
			}

			if (typeof (McpeTileEvent) == message.GetType())
			{
				McpeTileEvent msg = (McpeTileEvent) message;
				Log.DebugFormat("X: {0}", msg.x);
				Log.DebugFormat("Y: {0}", msg.y);
				Log.DebugFormat("Z: {0}", msg.z);
				Log.DebugFormat("Case 1: {0}", msg.case1);
				Log.DebugFormat("Case 2: {0}", msg.case2);
				return;
			}
			if (typeof (McpeAddEntity) == message.GetType())
			{
				McpeAddEntity msg = (McpeAddEntity) message;
				Log.DebugFormat("Entity ID: {0}", msg.entityId);
				Log.DebugFormat("Entity Type: {0}", msg.entityType);
				Log.DebugFormat("X: {0}", msg.x);
				Log.DebugFormat("Y: {0}", msg.y);
				Log.DebugFormat("Z: {0}", msg.z);
				Log.DebugFormat("Yaw: {0}", msg.yaw);
				Log.DebugFormat("Pitch: {0}", msg.pitch);
				Log.DebugFormat("Velocity X: {0}", msg.speedX);
				Log.DebugFormat("Velocity Y: {0}", msg.speedY);
				Log.DebugFormat("Velocity Z: {0}", msg.speedZ);
				Log.DebugFormat("Metadata: {0}", msg.metadata.ToString());
				Log.DebugFormat("Links count: {0}", msg.links);

				return;
			}
			if (typeof (McpeSetEntityData) == message.GetType())
			{
				McpeSetEntityData msg = (McpeSetEntityData) message;
				Log.DebugFormat("Entity ID: {0}", msg.entityId);
				MetadataDictionary metadata = msg.metadata;
				Log.DebugFormat("Metadata: {0}", metadata.ToString());
				return;
			}

			if (typeof (McpeMovePlayer) == message.GetType())
			{
				//McpeMovePlayer msg = (McpeMovePlayer) message;
				//Log.DebugFormat("Entity ID: {0}", msg.entityId);

				//CurrentLocation = new PlayerLocation(msg.x, msg.y + 10, msg.z);
				//SendMcpeMovePlayer();
				return;
			}

			if (typeof (McpeUpdateBlock) == message.GetType())
			{
				McpeUpdateBlock msg = (McpeUpdateBlock) message;
				Log.DebugFormat("No of Blocks: {0}", msg.blocks.Count);
				return;
			}

			if (typeof (McpeLevelEvent) == message.GetType())
			{
				McpeLevelEvent msg = (McpeLevelEvent) message;
				Log.DebugFormat("Event ID: {0}", msg.eventId);
				Log.DebugFormat("X: {0}", msg.x);
				Log.DebugFormat("Y: {0}", msg.y);
				Log.DebugFormat("Z: {0}", msg.z);
				Log.DebugFormat("Data: {0}", msg.data);
				return;
			}
		}

		public void SendPackage(List<Package> messages, short mtuSize, ref int reliableMessageNumber)
		{
			if (messages.Count == 0) return;

			foreach (var message in messages)
			{
				TraceSend(message);
			}

			Datagram.CreateDatagrams(messages, mtuSize, ref reliableMessageNumber, Session, SendDatagram);
		}

		private void SendDatagram(PlayerNetworkSession session, Datagram datagram)
		{
			if (datagram.MessageParts.Count != 0)
			{
				datagram.Header.datagramSequenceNumber = Interlocked.Increment(ref Session.DatagramSequenceNumber);
				byte[] data = datagram.Encode();

				datagram.Timer.Restart();
				SendData(data, _serverTargetEndpoint);
			}
		}


		private void SendData(byte[] data)
		{
			SendData(data, _serverTargetEndpoint);
		}


		private void SendData(byte[] data, IPEndPoint targetEndpoint)
		{
			if (Listener == null) return;
			try
			{
				Listener.Send(data, data.Length, targetEndpoint);
			}
			catch (Exception e)
			{
				Log.Debug("Send exception", e);
			}
		}

		private void TraceReceive(Package message)
		{
			//Log.InfoFormat("> Receive: {0}: {1} (0x{0:x2})", message.Id, message.GetType().Name);
		}

		private void TraceSend(Package message)
		{
			//Log.InfoFormat("<    Send: {0}: {1} (0x{0:x2})", message.Id, message.GetType().Name);
		}

		public void SendUnconnectedPing()
		{
			var packet = new UnconnectedPing
			{
				pingId = DateTime.UtcNow.Ticks /*incoming.pingId*/,
			};

			var data = packet.Encode();
			TraceSend(packet);
			SendData(data);
		}

		public void SendConnectedPing()
		{
			var packet = new ConnectedPing()
			{
				sendpingtime = DateTime.UtcNow.Ticks
			};

			SendPackage(packet);
		}

		public void SendConnectedPong(long sendpingtime)
		{
			var packet = new ConnectedPong
			{
				sendpingtime = sendpingtime,
				sendpongtime = sendpingtime + 10
			};

			SendPackage(packet);
		}

		public void SendOpenConnectionRequest1()
		{
			var packet = new OpenConnectionRequest1()
			{
				raknetProtocolVersion = 5, // Indicate to the server that this is a performance tests. Disables logging.
				mtuSize = _mtuSize
			};

			byte[] data = packet.Encode();

			TraceSend(packet);

			// 1087 1447
			byte[] data2 = new byte[_mtuSize - data.Length];
			Buffer.BlockCopy(data, 0, data2, 0, data.Length);

			SendData(data2);
		}

		public void SendOpenConnectionRequest2()
		{
			var packet = new OpenConnectionRequest2()
			{
				systemadress = new byte[4],
				mtuSize = _mtuSize,
				clientGuid = DateTime.UtcNow.Ticks,
				//clientUdpPort = (short) _listener.Client.
			};

			var data = packet.Encode();

			TraceSend(packet);

			SendData(data);
		}

		public void SendConnectionRequest()
		{
			var packet = new ConnectionRequest()
			{
			};

			SendPackage(packet);
		}

		private void SendPackage(Package package)
		{
			SendPackage(new List<Package> {package}, _mtuSize, ref _reliableMessageNumber);
			package.PutPool();
		}

		public void SendNewIncomingConnection()
		{
			var packet = new NewIncomingConnection
			{
			};

			SendPackage(packet);
		}

		public void SendLogin(string username)
		{
			Skin skin = new Skin {Slim = false, Texture = Encoding.Default.GetBytes(new string('Z', 8192))};
			var packet = new McpeLogin()
			{
				clientId = ClientId,
				username = username,
				protocol = 27,
				skin = skin
			};

			McpeBatch batch = new McpeBatch();
			byte[] buffer = Player.CompressBytes(packet.Encode(), CompressionLevel.Fastest);

			batch.payloadSize = buffer.Length;
			batch.payload = buffer;

			SendPackage(batch);
			LoginSent = true;
		}

		public bool LoginSent { get; set; }

		public void SendChat(string text)
		{
			var packet = new McpeText()
			{
				source = Username,
				message = text
			};

			SendPackage(packet);
		}

		public void SendMcpeMovePlayer()
		{
			//var movePlayerPacket = McpeMovePlayer.AddReference();
			McpeMovePlayer movePlayerPacket = McpeMovePlayer.CreateObject();
			movePlayerPacket.entityId = 0;
			movePlayerPacket.x = CurrentLocation.X;
			movePlayerPacket.y = CurrentLocation.Y;
			movePlayerPacket.z = CurrentLocation.Z;
			movePlayerPacket.yaw = 91;
			movePlayerPacket.pitch = 28;
			movePlayerPacket.headYaw = 91;

			SendPackage(movePlayerPacket);

			//SendChat("Movin " + CurrentLocation);
		}


		public void SendDisconnectionNotification()
		{
			SendPackage(new DisconnectionNotification());
		}

		public static void Connect()
		{
			//var client = new MiNetRealClient(new IPEndPoint(IPAddress.Loopback, 19132));
			var client = new MiNetClient(new IPEndPoint(IPAddress.Parse("192.168.0.2"), 19132));

			client.StartServer();
			Console.WriteLine("Server started.");

			Thread.Sleep(2000);

			Console.WriteLine("Sending ping...");

			client.SendUnconnectedPing();
			//client.SendUnconnectedPing();
			//client.SendUnconnectedPing();

			Console.WriteLine("<Enter> to exit!");
			Console.ReadLine();
			client.StopServer();
		}

		private static void Main(string[] args)
		{
			Console.WriteLine("Starting client...");

			Connect();

			Console.ReadLine();

			Console.WriteLine("Close client...");
		}
	}
}