using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Sockets;
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

		private IPEndPoint _endpoint;
		private IPEndPoint _serverEndpoint;
		private UdpClient _listener;
		private short _mtuSize = 1447;
		private int _reliableMessageNumber;
		private int _datagramSequenceNumber;
		private Vector3 _spawn;
		private long _entityId;

		public MiNetClient(IPEndPoint endpoint)
		{
			_serverEndpoint = endpoint;
			_endpoint = new IPEndPoint(IPAddress.Any, 19132);
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

				_listener = new UdpClient(_endpoint);

				if (IsRunningOnMono())
				{
					_listener.Client.ReceiveBufferSize = 1024*1024*3;
					_listener.Client.SendBufferSize = 4096;
				}
				else
				{
					_listener.Client.ReceiveBufferSize = int.MaxValue;
					////_listener.Client.SendBufferSize = 1024*1024*8;
					_listener.Client.SendBufferSize = int.MaxValue;
					_listener.DontFragment = true;

					// SIO_UDP_CONNRESET (opcode setting: I, T==3)
					// Windows:  Controls whether UDP PORT_UNREACHABLE messages are reported.
					// - Set to TRUE to enable reporting.
					// - Set to FALSE to disable reporting.

					uint IOC_IN = 0x80000000;
					uint IOC_VENDOR = 0x18000000;
					uint SIO_UDP_CONNRESET = IOC_IN | IOC_VENDOR | 12;
					_listener.Client.IOControl((int) SIO_UDP_CONNRESET, new byte[] {Convert.ToByte(false)}, null);

					////
					////WARNING: We need to catch errors here to remove the code above.
					////
				}

				_listener.BeginReceive(ReceiveCallback, _listener);

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
				if (listener.Client == null) return;
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
				//new Task(() => ProcessMessage(receiveBytes, senderEndpoint)).Start();
				ProcessMessage(receiveBytes, senderEndpoint);
				if (listener.Client == null) return;
				listener.BeginReceive(ReceiveCallback, listener);
			}
			else
			{
				Log.Error("Unexpected end of transmission?");
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
			//_serverEndpoint = senderEndpoint;
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
						Thread.Sleep(50);
						UnconnectedPong incoming = (UnconnectedPong) message;
						SendOpenConnectionRequest1();

						break;
					}
					case DefaultMessageIdTypes.ID_OPEN_CONNECTION_REPLY_1:
					{
						Thread.Sleep(50);
						OpenConnectionReply1 incoming = (OpenConnectionReply1) message;
						SendOpenConnectionRequest2();
						break;
					}
					case DefaultMessageIdTypes.ID_OPEN_CONNECTION_REPLY_2:
					{
						OpenConnectionReply2 incoming = (OpenConnectionReply2) message;
						Thread.Sleep(50);
						//_mtuSize = incoming.mtuSize;
						SendConnectionRequest();
						break;
					}
					case DefaultMessageIdTypes.ID_CONNECTION_REQUEST_ACCEPTED:
					{
						Thread.Sleep(50);
						SendNewIncomingConnection();
						var t1 = new Timer(state => SendConnectedPing(), null, 0, 5000);
						Thread.Sleep(50);
						SendLogin("Client12");
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

					//Log.Debug("Received package: #" + package._datagramSequenceNumber.IntValue());

					Reliability reliability = package._reliability;
					//Log.InfoFormat("Reliability: {0}", reliability);

					//if (reliability == Reliability.Reliable
					//	|| reliability == Reliability.ReliableSequenced
					//	|| reliability == Reliability.ReliableOrdered
					//	)
					{
						// Send ACK
						Acks ack = new Acks();
						ack.acks.Add(package._datagramSequenceNumber.IntValue());
						byte[] data = ack.Encode();
						//Log.Debug("<\tSend ACK on #" + package._datagramSequenceNumber.IntValue());
						SendData(data, senderEndpoint);
					}

					foreach (var message in messages)
					{
						if (message is SplitPartPackage)
						{
							SplitPartPackage splitMessage = message as SplitPartPackage;

							int spId = package._splitPacketId;
							int spIdx = package._splitPacketIndex;
							int spCount = package._splitPacketCount;

							Log.DebugFormat("Got split package {2} (of {0}) for split ID: {1}", spCount, spId, spIdx);

							if (!_splits.ContainsKey(spId))
							{
								_splits[spId] = new SplitPartPackage[spCount];
							}

							SplitPartPackage[] spPackets = _splits[spId];
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
									Log.Warn("When processing split-message", e);
								}
							}

							message.PutPool();
							return;
						}

						{
							message.Timer.Restart();
							HandlePackage(message, senderEndpoint);
							//message.PutPool();
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
			Ack ack = Ack.CreateObject();
			ack.Decode(receiveBytes);
			int ackSeqNo = ack.sequenceNumber.IntValue();
			Log.Debug("ACK #" + ackSeqNo);
		}

		private void HandleNak(byte[] receiveBytes, IPEndPoint senderEndpoint)
		{
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
				Log.Debug(msg.message);
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
				McpeFullChunkData msg = (McpeFullChunkData) message;
				ChunkColumn chunk = ClientUtils.DecocedChunkColumn(msg.chunkData);
				if (chunk != null)
				{
					Log.DebugFormat("Chunk X={0}", chunk.x);
					Log.DebugFormat("Chunk Z={0}", chunk.z);

					ClientUtils.SaveChunkToAnvil(chunk);
				}
				return;
			}

			if (typeof (ConnectionRequestAccepted) == message.GetType())
			{
				Thread.Sleep(50);
				SendNewIncomingConnection();
				var t1 = new Timer(state => SendConnectedPing(), null, 2000, 5000);
				Thread.Sleep(50);
				SendLogin("Client12");
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

				ClientUtils.SaveLevel(_level);

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
				Log.DebugFormat("Velocity X: {0}", msg.speedX);
				Log.DebugFormat("Velocity Y: {0}", msg.speedY);
				Log.DebugFormat("Velocity Z: {0}", msg.speedZ);
				Log.DebugFormat("Metadata: {0}", msg.metadata.ToString());

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
				McpeMovePlayer msg = (McpeMovePlayer) message;
				Log.DebugFormat("Entity ID: {0}", msg.entityId);

				_currentLocation = new PlayerLocation(msg.x, msg.y + 10, msg.z);
				SendMcpeMovePlayer();
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

		public void SendPackage(IPEndPoint senderEndpoint, List<Package> messages, short mtuSize, ref int datagramSequenceNumber, ref int reliableMessageNumber, Reliability reliability = Reliability.Reliable)
		{
			if (messages.Count == 0) return;

			foreach (var message in messages)
			{
				TraceSend(message);
			}

			Datagram.CreateDatagrams(messages, mtuSize, ref datagramSequenceNumber, ref reliableMessageNumber, senderEndpoint, SendDatagram);
		}

		private void SendDatagram(IPEndPoint senderEndpoint, Datagram datagram)
		{
			SendDatagram(senderEndpoint, datagram, false);
		}


		private void SendDatagram(IPEndPoint senderEndpoint, Datagram datagram, bool isResend)
		{
			if (datagram.MessageParts.Count != 0)
			{
				Log.Debug("<\tSend Datagram #" + datagram.Header.datagramSequenceNumber.IntValue());
				byte[] data = datagram.Encode();

				datagram.Timer.Restart();
				SendData(data, senderEndpoint);
			}
		}


		private void SendData(byte[] data)
		{
			SendData(data, _serverEndpoint);
		}


		private void SendData(byte[] data, IPEndPoint targetEndpoint)
		{
			if (_listener == null) return;
			_listener.Send(data, data.Length, targetEndpoint);
		}

		private static void TraceReceive(Package message)
		{
			Log.DebugFormat("> Receive: {0}: {1} (0x{0:x2})", message.Id, message.GetType().Name);
		}

		private static void TraceSend(Package message)
		{
			Log.DebugFormat("<    Send: {0}: {1} (0x{0:x2})", message.Id, message.GetType().Name);
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
			SendPackage(_serverEndpoint, new List<Package>() {package}, _mtuSize, ref _datagramSequenceNumber, ref _reliableMessageNumber);
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
			var packet = new McpeLogin()
			{
				clientId = 12345,
				username = username,
				protocol = 27,
				slim = 0,
				skin = new string('Z', 8192)
			};

			McpeBatch batch = new McpeBatch();
			byte[] buffer = CompressBytes(packet.Encode());

			batch.payloadSize = buffer.Length;
			batch.payload = buffer;

			SendPackage(batch);
		}

		public byte[] CompressBytes(byte[] input)
		{
			MemoryStream stream = new MemoryStream();
			stream.WriteByte(0x78);
			stream.WriteByte(0x01);
			int checksum;
			using (var compressStream = new ZLibStream(stream, CompressionLevel.Optimal, true))
			{
				NbtBinaryWriter writer = new NbtBinaryWriter(compressStream, true);
				writer.Write(input);

				writer.Flush();

				checksum = compressStream.Checksum;
				writer.Close();
			}

			byte[] checksumBytes = BitConverter.GetBytes(checksum);
			if (BitConverter.IsLittleEndian)
			{
				// Adler32 checksum is big-endian
				Array.Reverse(checksumBytes);
			}
			stream.Write(checksumBytes, 0, checksumBytes.Length);

			var bytes = stream.ToArray();
			stream.Close();

			return bytes;
		}

		public void SendChat(string text)
		{
			var packet = new McpeText()
			{
				source = "Nicke",
				message = text
			};

			SendPackage(packet);
		}

		private Random random = new Random();
		private Dictionary<int, SplitPartPackage[]> _splits = new Dictionary<int, SplitPartPackage[]>();
		private LevelInfo _level = new LevelInfo();
		private PlayerLocation _currentLocation;


		public void SendMcpeMovePlayer()
		{
			//var movePlayerPacket = McpeMovePlayer.AddReference();
			McpeMovePlayer movePlayerPacket = new McpeMovePlayer();
			movePlayerPacket.entityId = _entityId;
			movePlayerPacket.x = _currentLocation.X;
			movePlayerPacket.y = _currentLocation.Y;
			movePlayerPacket.z = _currentLocation.Z;
			movePlayerPacket.yaw = 91;
			movePlayerPacket.pitch = 28;
			movePlayerPacket.headYaw = 91;

			SendPackage(movePlayerPacket);

			//SendChat("Testing");
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