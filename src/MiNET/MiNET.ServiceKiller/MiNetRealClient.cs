// Configure log4net using the .config file

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using log4net;
using log4net.Config;
using MiNET.Net;
using MiNET.Utils;

[assembly: XmlConfigurator(Watch = true)]
// This will cause log4net to look for a configuration file
// called TestApp.exe.config in the application base
// directory (i.e. the directory containing TestApp.exe)
// The config file will be watched for changes.

namespace MiNET.ServiceKiller
{
	namespace MiNET
	{
		public class MiNetRealClient
		{
			private static readonly ILog Log = LogManager.GetLogger(typeof (MiNetServer));

			private IPEndPoint _endpoint;
			private IPEndPoint _serverEndpoint;
			private UdpClient _listener;
			private short _mtuSize = 1447;
			public int _reliableMessageNumber;
			public int _datagramSequenceNumber;
			private Vector3 _spawn;
			private int _entityId;

			public MiNetRealClient(IPEndPoint endpoint)
			{
				_serverEndpoint = endpoint ?? new IPEndPoint(IPAddress.Loopback, 19132);
				_endpoint = new IPEndPoint(IPAddress.Any, 0);
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
					//new Task(() => ProcessMessage(receiveBytes, senderEndpoint)).Start();
					ProcessMessage(receiveBytes, senderEndpoint);
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

							//var packet = new UnconnectedPong
							//{
							//	serverId = 12345,
							//	pingId = 100 /*incoming.pingId*/,
							//	serverName = "MCCPP;Demo;MiNET - Another MC server"
							//};
							//var data = packet.Encode();
							//TraceSend(packet);
							//SendData(data, senderEndpoint);
							break;
						}
						case DefaultMessageIdTypes.ID_OPEN_CONNECTION_REQUEST_1:
						{
							OpenConnectionRequest1 incoming = (OpenConnectionRequest1) message;

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
						var messages = package.Messages;

						//Log.Debug("Received package: #" + package._datagramSequenceNumber.IntValue());

						Reliability reliability = package._reliability;
						//Log.InfoFormat("Reliability: {0}", reliability);

						if (reliability == Reliability.Reliable
						    || reliability == Reliability.ReliableSequenced
						    || reliability == Reliability.ReliableOrdered
							)
						{
							// Send ACK
							Acks ack = new Acks();
							ack.acks.Add(package._datagramSequenceNumber.IntValue());
							byte[] data = ack.Encode();
							//Log.Debug("Send ACK on #" + package._datagramSequenceNumber.IntValue());
							SendData(data, senderEndpoint);
						}

						foreach (var message in messages)
						{
							message.Timer.Restart();
							TraceReceive(message);
							HandlePackage(message, senderEndpoint);
							//message.PutPool();
						}

						//package.PutPool();
					}
					else if (header.isPacketPair)
					{
						Log.Warn("header.isPacketPair");
					}
					else if (header.isACK && header.isValid)
					{
						Log.Debug("ACK");
						//Acks ack = new Acks();
						//ack.acks.Add(header.datagramSequenceNumber.IntValue());
						//byte[] data = ack.Encode();
						////Log.Debug("Send ACK on #" + header.datagramSequenceNumber.IntValue());
						//SendData(data, senderEndpoint);
						HandleAck(receiveBytes, senderEndpoint);
					}
					else if (header.isNAK && header.isValid)
					{
						Log.Warn("!!!! NAK !!!!!");
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

				if (typeof (McpeSetSpawnPosition) == message.GetType())
				{
					McpeSetSpawnPosition msg = (McpeSetSpawnPosition) message;
					_spawn = new Vector3(msg.x, msg.y, msg.z);
					return;
				}

				if (typeof (McpeStartGame) == message.GetType())
				{
					McpeStartGame msg = (McpeStartGame) message;
					_entityId = msg.entityId;
					_spawn = new Vector3(msg.x, msg.y, msg.z);
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
					Log.DebugFormat("DID: {0}", msg.did);
					Log.DebugFormat("X: {0}", msg.x);
					Log.DebugFormat("Y: {0}", msg.y);
					Log.DebugFormat("Z: {0}", msg.z);
					Log.DebugFormat("Velocity X: {0}", msg.velocityX);
					Log.DebugFormat("Velocity Y: {0}", msg.velocityY);
					Log.DebugFormat("Velocity Z: {0}", msg.velocityZ);

					return;
				}
				if (typeof (McpeSetEntityData) == message.GetType())
				{
					McpeSetEntityData msg = (McpeSetEntityData) message;
					Log.DebugFormat("Entity ID: {0}", msg.entityId);
					MetadataDictionary metadata = GetMetadata(msg.namedtag);
					Log.DebugFormat("NameTag: {0}", metadata.ToString());

					return;
				}

				if (typeof (McpeMovePlayer) == message.GetType())
				{
					McpeMovePlayer msg = (McpeMovePlayer) message;
					Log.DebugFormat("Entity ID: {0}", msg.entityId);

					return;
				}
			}

			public MetadataDictionary GetMetadata(byte[] buffer)
			{
				var stream = new MemoryStream(buffer);
				//stream.Write(buffer, 0, buffer.Length);
				//stream.Position = 0;
				var writer = new BinaryReader(stream);
				return MetadataDictionary.FromStream(writer);
			}

			public void SendPackage(IPEndPoint senderEndpoint, List<Package> messages, short mtuSize, ref int datagramSequenceNumber, ref int reliableMessageNumber, Reliability reliability = Reliability.Reliable)
			{
				if (messages.Count == 0) return;

				Datagram.CreateDatagrams(messages, mtuSize, ref datagramSequenceNumber, ref reliableMessageNumber, senderEndpoint, SendDatagram);

				foreach (var message in messages)
				{
					TraceSend(message);

					//message.PutPool();
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
				}

				//foreach (MessagePart part in datagram.MessageParts)
				//{
				//	part.PutPool();
				//}

				//datagram.PutPool();
			}


			private void SendData(byte[] data)
			{
				SendData(data, _serverEndpoint);
			}


			private void SendData(byte[] data, IPEndPoint targetEndpoint)
			{
				//_listener.SendAsync(data, data.Length, targetEndpoint); // Has thread pooling issues?
				_listener.Send(data, data.Length, targetEndpoint); // Less thread-issues it seems
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
					pingId = 100 /*incoming.pingId*/,
				};

				var data = packet.Encode();
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

			public void SendOpenConnectionRequest1()
			{
				var packet = new OpenConnectionRequest1()
				{
					raknetProtocolVersion = 5, // Indicate to the server that this is a performance tests. Disables logging.
					mtuSize = _mtuSize
				};

				byte[] data = packet.Encode();

				byte[] data2 = new byte[_mtuSize - data.Length];
				Buffer.BlockCopy(data, 0, data2, 0, data.Length);

				SendData(data2);
			}

			public void SendOpenConnectionRequest2()
			{
				var packet = new OpenConnectionRequest2()
				{
					cookie = new byte[4],
					mtuSize = _mtuSize,
					//clientUdpPort = (short) _listener.Client.
				};

				var data = packet.Encode();
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

			public void SendMcpeLogin(string username)
			{
				var packet = new McpeLogin()
				{
					clientId = 12345,
					username = username,
					logindata = "nothing",
					protocol = 20,
				};

				SendPackage(packet);
			}

			public void SendChat(string text)
			{
				var packet = new McpeMessage
				{
					source = "Nicke",
					message = text
				};

				SendPackage(packet);
			}

			private Random random = new Random();

			public void SendMcpeMovePlayer()
			{
				//var movePlayerPacket = McpeMovePlayer.AddReference();
				McpeMovePlayer movePlayerPacket = new McpeMovePlayer();
				movePlayerPacket.entityId = _entityId;
				movePlayerPacket.x = (float) _spawn.X + random.Next(0, 2);
				movePlayerPacket.y = (float) _spawn.Y + random.Next(0, 2);
				movePlayerPacket.z = (float) _spawn.Z + random.Next(0, 2);
				movePlayerPacket.yaw = 91;
				movePlayerPacket.pitch = 28;
				movePlayerPacket.bodyYaw = 91;

				SendPackage(movePlayerPacket);

				//SendChat("Testing");
			}


			public static void Connect()
			{
				//var client = new MiNetRealClient(new IPEndPoint(IPAddress.Loopback, 19132));
				var client = new MiNetRealClient(new IPEndPoint(IPAddress.Parse("192.168.0.10"), 19132));

				client.StartServer();
				Console.WriteLine("Server started.");
				Thread.Sleep(1000);

				client.SendUnconnectedPing();
				Console.WriteLine("SendUnconnectedPing");
				Thread.Sleep(1000);

				client.SendOpenConnectionRequest1();
				Console.WriteLine("SendOpenConnectionRequest1");
				Thread.Sleep(10);

				client.SendOpenConnectionRequest2();
				Console.WriteLine("SendOpenConnectionRequest2");
				Thread.Sleep(10);

				client.SendConnectionRequest();
				Console.WriteLine("SendConnectionRequest");
				Thread.Sleep(50);

				var t1 = new Timer(state => client.SendConnectedPing(), null, 0, 5000);

				client.SendNewIncomingConnection();
				client.SendMcpeLogin("Client12");
				Console.WriteLine("SendMcpeLogin");

				//var t2 = new Timer(state => client.SendMcpeMovePlayer(), null, 0, 1509);

				client.SendChat("Testing 1");
				Thread.Sleep(2000);

				client.SendChat("Testing 2");

				Console.WriteLine("<Enter> to exit!");
				Console.ReadLine();
				client.StopServer();
			}

			private static void Main(string[] args)
			{
				ThreadPool.SetMinThreads(1000, 1000);

				Console.WriteLine("Starting client...");

				Connect();

				Console.ReadLine();

				Console.WriteLine("Close client...");
			}
		}
	}
}