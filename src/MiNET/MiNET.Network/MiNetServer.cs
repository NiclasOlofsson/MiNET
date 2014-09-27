using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Craft.Net.Anvil;
using Craft.Net.Common;
using Craft.Net.TerrainGeneration;
using Timer = System.Timers.Timer;

namespace MiNET.Network
{
	internal enum MessageHeader
	{
		ÏsValue,
		IsAck,
		IsNack,
		IsPacketPair,
		IsContinuousSend,
		NeedsBAndAs,
	}


	public enum GameModes
	{
		SURVIVAL,
		CREATIVE,
		ADVENTURE,
		SPECTATOR,
	}


	public class MiNetServer
	{
		private ConnectionState _state = ConnectionState.Waiting;
		private int _sequenceNumber;
		private char _reliableMessageNumber;
		private IPEndPoint _endpoint;
		private UdpClient _listener;
		public const int DefaultPort = 19132;
		private StandardGenerator _generator;

		private MiNetServer()
		{
		}

		public MiNetServer(int port)
			: this(new IPEndPoint(IPAddress.Any, port))
		{
		}

		public MiNetServer(IPEndPoint endpoint = null)
		{
			_endpoint = endpoint ?? new IPEndPoint(IPAddress.Any, DefaultPort);
		}

		public bool StartServer()
		{
			if (_listener != null) return false; // Already started

			try
			{
				int playerX = 50;
				int playerZ = 50;

				_generator = new StandardGenerator();
				_generator.Seed = 1000;
				_generator.Initialize(null);

				_chunkCache = new List<Chunk2>();
				_chunkCache.AddRange(GenerateChunks(playerX, playerZ));

				_listener = new UdpClient(_endpoint);

				// SIO_UDP_CONNRESET (opcode setting: I, T==3)
				// Windows:  Controls whether UDP PORT_UNREACHABLE messages are reported.
				// - Set to TRUE to enable reporting.
				// - Set to FALSE to disable reporting.

				uint IOC_IN = 0x80000000;
				uint IOC_VENDOR = 0x18000000;
				uint SIO_UDP_CONNRESET = IOC_IN | IOC_VENDOR | 12;
				_listener.Client.IOControl((int) SIO_UDP_CONNRESET, new byte[] { Convert.ToByte(false) }, null);

				// We need to catch errors here to remove the code above.
				_listener.BeginReceive(ReceiveCallback, _listener);

				Timer sendTimer = new Timer(30);
				sendTimer.AutoReset = true;
				sendTimer.Elapsed += sendTimer_Elapsed;
				sendTimer.Start();

				Console.WriteLine("Server open for business...");
				return true;
			}
			catch (Exception e)
			{
				Debug.Write(e);
				StopServer();
			}

			return false;
		}


		public bool StopServer()
		{
			try
			{
				if (_listener == null) return true; // Already stopped. It's ok.

				_listener.Close();
				_listener = null;

				return true;
			}
			catch (Exception e)
			{
				Debug.Write(e);
			}

			return true;
		}

		private void ReceiveCallback(IAsyncResult ar)
		{
			try
			{
				UdpClient listener = (UdpClient) ar.AsyncState;

				// WSAECONNRESET:
				// The virtual circuit was reset by the remote side executing a hard or abortive close. 
				// The application should close the socket; it is no longer usable. On a UDP-datagram socket 
				// this error indicates a previous send operation resulted in an ICMP Port Unreachable message.
				// Note the spocket settings on creation of the server. It makes us ignore these resets.
				IPEndPoint senderEndpoint = new IPEndPoint(0, 0);
				Byte[] receiveBytes = listener.EndReceive(ar, ref senderEndpoint);

				int msgId = receiveBytes[0];

				if (msgId >= (int) DefaultMessageIdTypes.ID_CONNECTED_PING && msgId <= (int) DefaultMessageIdTypes.ID_USER_PACKET_ENUM)
				{
					DefaultMessageIdTypes msgIdType = (DefaultMessageIdTypes) msgId;
					if (msgIdType != DefaultMessageIdTypes.ID_CONNECTED_PING && msgIdType != DefaultMessageIdTypes.ID_UNCONNECTED_PING)
					{
						Debug.Print("> Receive: {1} (0x{0:x2})", msgId, msgIdType);
						Debug.Print("\tData: {0}", ByteArrayToString(receiveBytes));
					}
					switch (msgIdType)
					{
						case DefaultMessageIdTypes.ID_CONNECTED_PING:
							break;
						case DefaultMessageIdTypes.ID_UNCONNECTED_PING:
						case DefaultMessageIdTypes.ID_UNCONNECTED_PING_OPEN_CONNECTIONS:
						{
							var incoming = new IdUnconnectedPing();
							incoming.SetBuffer(receiveBytes);
							incoming.Decode();

							var packet = new IdUnconnectedPong();
							packet.serverId = 12345;
							packet.pingId = 100;
							packet.serverName = "MCCPP;Demo;MiNET - Another MC server"; // Magic!!!!
							packet.Encode();

							var data = packet.GetBytes();

							//Debug.Print("< Send: {1} (0x{0:x2})", packet.Id, (DefaultMessageIdTypes) packet.Id);
							//Debug.Print("\tData: {0}", ByteArrayToString2(data));

							SendRaw(listener, data, senderEndpoint);
							break;
						}
						case DefaultMessageIdTypes.ID_DETECT_LOST_CONNECTIONS:
							break;
						case DefaultMessageIdTypes.ID_OPEN_CONNECTION_REQUEST_1:
						{
							if (_state == ConnectionState.Connecting) break;
							_state = ConnectionState.Connecting;

							var incoming = new IdOpenConnectionRequest1();
							incoming.SetBuffer(receiveBytes);
							incoming.Decode();

							var packet = new IdOpenConnectionReply1();
							packet.serverGuid = 12345;
							packet.mtuSize = incoming.mtuSize;
							packet.serverHasSecurity = 0;
							packet.Encode();

							var data = packet.GetBytes();
							SendRaw(listener, data, senderEndpoint);
							break;
						}
						case DefaultMessageIdTypes.ID_OPEN_CONNECTION_REQUEST_2:
						{
							if (_state == ConnectionState.Connecting2) break;
							_state = ConnectionState.Connecting2;

							var incoming = new IdOpenConnectionRequest2();
							incoming.SetBuffer(receiveBytes);
							incoming.Decode();

							IdOpenConnectionReply2 packet = new IdOpenConnectionReply2();
							packet.serverGuid = 12345;
							packet.clientUdpPort = (short) senderEndpoint.Port;
							packet.mtuSize = incoming.mtuSize;
							packet.doSecurity = 0;
							packet.Encode();

							var data = packet.GetBytes();
							SendRaw(listener, data, senderEndpoint);
							break;
						}
					}
				}
				else
				{
					DatagramHeader header = new DatagramHeader(receiveBytes[0]);
					if (!header.isACK && !header.isNAK && header.isValid)
					{
						_state = ConnectionState.Connected;

						if (receiveBytes[0] != 0xa0)
						{
							byte[] buffer;
							{
								var package = new ConnectedPackage();
								package.SetBuffer(receiveBytes);
								package.Decode();

								buffer = package.internalBuffer;

								SendAck(listener, senderEndpoint, package._sequenceNumber);
							}
							var message = PackageFactory.CreatePackage(buffer[0]);
							if (message == null)
							{
								Debug.Print("> Receive Unkown: {0} (0x{1:x2}) ", (DefaultMessageIdTypes) buffer[0], buffer[0]);
								Debug.Print("\tData: {0}", ByteArrayToString(receiveBytes));
							}
							else
							{
								if (message.Id != (decimal) DefaultMessageIdTypes.ID_CONNECTED_PING && message.Id != (decimal) DefaultMessageIdTypes.ID_UNCONNECTED_PING)
								{
									Debug.Print("> Receive: {0} (0x{1:x2}) ", (DefaultMessageIdTypes) message.Id, message.Id);
									Debug.Print("\tData: Length={1} {0}", ByteArrayToString(receiveBytes), buffer.Length);
								}

								message.SetBuffer(buffer);
								message.Decode();

								if (typeof (IdConnectedPing) == message.GetType())
								{
									var msg = (IdConnectedPing) message;

									var response = new IdConnectedPong();
									response.sendpingtime = msg.sendpingtime;
									response.sendpongtime = DateTimeOffset.UtcNow.Ticks/TimeSpan.TicksPerMillisecond;
									response.Encode();

									SendPackage(listener, senderEndpoint, response);
								}
								else if (typeof (IdConnectionRequest) == message.GetType())
								{
									var msg = (IdConnectionRequest) message;
									var response = new ConnectionRequestAcceptedManual((short) senderEndpoint.Port, msg.timestamp);
									response.Encode();

									SendPackage(listener, senderEndpoint, response);
								}
								else if (typeof (IdMcpeLogin) == message.GetType())
								{
									{
										var msg = (IdMcpeLogin) message;
										msg.Decode();

										var response = new IdMcpeLoginStatus();
										response.status = 0;
										response.Encode();

										SendPackage(listener, senderEndpoint, response);
									}

									int playerX = 50;
									int playerZ = 50;
									int playerY = 5;
									int playerSpawnY = 120;

									// Start game
									{
										//const SURVIVAL = 0;
										//const CREATIVE = 1;
										//const ADVENTURE = 2;
										//const SPECTATOR = 3;

										var response = new IdMcpeStartGame();
										response.seed = 1406827239;
										response.generator = 1; //0 old, 1 infinite, 2 flat
										response.gamemode = 1;
										response.entityId = 0; // Always 0 for player
										response.spawnX = playerX;
										response.spawnY = playerSpawnY;
										response.spawnZ = playerZ;
										response.x = playerX;
										response.y = playerY;
										response.z = playerZ;
										response.Encode();

										SendPackage(listener, senderEndpoint, response);
									}

									{
										// started == true ? 0x80 : 0x00);
										var response = new IdMcpeSetTime();
										response.time = 6000;
										response.started = 0x80;
										response.Encode();
										SendPackage(listener, senderEndpoint, response);
									}
									{
										var response = new IdMcpeSetSpawnPosition();
										response.x = playerX;
										response.z = playerZ;
										response.y = (byte) playerSpawnY;
										response.Encode();
										SendPackage(listener, senderEndpoint, response);
									}
									{
										var response = new IdMcpeSetHealth();
										response.health = 20;
										response.Encode();
										SendPackage(listener, senderEndpoint, response);
									}
									{
										//	// CHUNK!

										//var generator = new FlatGenerator();
										//var chunks = generator.GenerateFlatWorld(16, 16);
										var chunks = GenerateChunks(playerX, playerZ);
										int count = 0;
										foreach (var chunk in chunks)
										{
											//if (count >= 96) break; // too big for the client to deal with

											{
												byte[] data = chunk.GetBytes();

												var response = new IdMcpeFullChunkData();
												response.chunkData = data;
												response.Encode();
												SendPackage(listener, senderEndpoint, response);
												Thread.Yield();
											}
											if (count == 56)
											{
												{
													//send time again
													var response = new IdMcpeSetTime();
													response.time = 6000;
													response.started = 0x80;
													response.Encode();
													SendPackage(listener, senderEndpoint, response);
												}

												{
													// Teleport user (MovePlayerPacket) teleport=1
													//yaw: 91
													//pitch: 28
													//bodyYaw: 91

													var response = new IdMcpeMovePlayer();
													response.x = playerX;
													response.y = playerY;
													response.z = playerZ;
													response.yaw = 91;
													response.pitch = 28;
													response.bodyYaw = 91;
													response.teleport = 0x80; // true
													response.Encode();
													SendPackage(listener, senderEndpoint, response);
												}
												{
													//$flags = 0;
													//if($this->isAdventure())
													//	$flags |= 0x01; //Do not allow placing/breaking blocks, adventure mode
													//if($nametags !== false){
													//	$flags |= 0x20; //Show Nametags
													//}

													// Adventure settings (AdventureSettingsPacket)
													var response = new IdMcpeAdventureSettings();
													response.flags = 0x20;
													response.Encode();
													SendPackage(listener, senderEndpoint, response);
												}

												// Settings (ContainerSetContentPacket)
												{
													//$this->inventory->sendContents($this);
													var response = new IdMcpeContainerSetContent();
													response.windowId = 0;
													response.slotCount = 0;
													response.slotData = new byte[0];
													response.hotbarCount = 0;
													response.hotbarData = new byte[0];
													response.Encode();
													SendPackage(listener, senderEndpoint, response);
												}

												{
													//$this->inventory->sendArmorContents($this);
													var response = new IdMcpeContainerSetContent();
													response.windowId = 0x78; // Armor window id constant
													response.slotCount = 0;
													response.slotData = new byte[0];
													response.hotbarCount = 0;
													response.hotbarData = new byte[0];
													response.Encode();
													SendPackage(listener, senderEndpoint, response);
												}
												{
													// Player joined!
													var response = new IdMcpeMessage();
													response.source = "";
													response.message = "Player joined the game!";
													response.Encode();
													SendPackage(listener, senderEndpoint, response);
												}
											}

											count++;
										}
									}
								}
							}
						}
					}
					else if (header.isACK && header.isValid)
					{
						Debug.WriteLine("....... ACK .....");
					}
					else if (header.isNAK && header.isValid)
					{
						Debug.WriteLine("!!!! WARNING, NAK !!!!!");
					}
				}


				if (receiveBytes.Length != 0)
				{
					listener.BeginReceive(ReceiveCallback, listener);
				}
			}
			catch (Exception e)
			{
				Debug.Write(e);
			}
		}

		private List<Chunk2> GenerateChunks(int playerX, int playerZ)
		{
			Dictionary<string, double> newOrder = new Dictionary<string, double>();
			int viewDistanace = 90;
			double radiusSquared = viewDistanace/Math.PI;
			double radius = Math.Ceiling(Math.Sqrt(radiusSquared));
			var centerX = playerX >> 4;
			var centerZ = playerZ >> 4;
			Queue<Chunk> chunkQueue = new Queue<Chunk>();
			for (double x = -radius; x <= radius; ++x)
			{
				for (double z = -radius; z <= radius; ++z)
				{
					var distance = (x*x) + (z*z);
					if (distance > radiusSquared)
					{
						continue;
					}
					var chunkX = x + centerX;
					var chunkZ = z + centerZ;
					string index = GetChunkHash(chunkX, chunkZ);

					newOrder[index] = distance;
				}
			}


			// Should be member..
			Dictionary<string, double> loadQueue = new Dictionary<string, double>();

			var sortedKeys = newOrder.Keys.ToList();
			sortedKeys.Sort();
			if (newOrder.Count > viewDistanace)
			{
				int count = 0;
				loadQueue = new Dictionary<string, double>();
				foreach (var key in sortedKeys)
				{
					loadQueue[key] = newOrder[key];
					if (++count > viewDistanace) break;
				}
			}
			else
			{
				loadQueue = newOrder;
			}

			List<Chunk2> chunks = new List<Chunk2>();
			foreach (var pair in loadQueue)
			{
//				Chunk2 chunk = CraftNetGenerateChunkForIndex(pair.Key);
				Chunk2 chunk = FlatlandGenerateChunkForIndex(pair.Key);
				if (chunk != null) chunks.Add(chunk);
			}

			return chunks;
		}


		private Chunk2 FlatlandGenerateChunkForIndex(string index)
		{
			int x = Int32.Parse(index.Split(new[] { ':' })[0]);
			int z = Int32.Parse(index.Split(new[] { ':' })[1]);

			FlatGenerator generator = new FlatGenerator();
			Chunk2 chunk = new Chunk2();
			chunk.x = x;
			chunk.z = z;
			generator.PopulateChunk(chunk);
			return chunk;
		}

		private Chunk2 CraftNetGenerateChunkForIndex(string index)
		{
			int x = Int32.Parse(index.Split(new[] { ':' })[0]);
			int z = Int32.Parse(index.Split(new[] { ':' })[1]);

			var firstOrDefault = _chunkCache.FirstOrDefault(chunk2 => chunk2 != null && chunk2.x == x && chunk2.z == z);
			if (firstOrDefault != null)
			{
				return firstOrDefault;
			}

			Chunk2 chunk;
			chunk = new Chunk2 { x = x, z = z };

			Chunk anvilChunk = _generator.GenerateChunk(new Coordinates2D(x, z));

			chunk.biomeId = anvilChunk.Biomes;
			for (int i = 0; i < chunk.biomeId.Length; i++)
			{
				if (chunk.biomeId[i] > 22) chunk.biomeId[i] = 0;
			}
			if (chunk.biomeId.Length > 256) throw new Exception();

			for (int xi = 0; xi < 16; xi++)
			{
				for (int zi = 0; zi < 16; zi++)
				{
					for (int yi = 0; yi < 128; yi++)
					{
						chunk.SetBlock(xi, yi, zi, (byte) anvilChunk.GetBlockId(new Coordinates3D(xi, yi + 45, zi)));
						chunk.SetBlocklight(xi, yi, zi, anvilChunk.GetBlockLight(new Coordinates3D(xi, yi + 45, zi)));
						chunk.SetMetadata(xi, yi, zi, anvilChunk.GetMetadata(new Coordinates3D(xi, yi + 45, zi)));
						chunk.SetSkylight(xi, yi, zi, anvilChunk.GetSkyLight(new Coordinates3D(xi, yi + 45, zi)));
					}
				}
			}

			for (int i = 0; i < chunk.skylight.Length; i++)
				chunk.skylight[i] = 0xff;

			for (int i = 0; i < chunk.biomeColor.Length; i++)
				chunk.biomeColor[i] = 8761930;

			return chunk;
		}

		private string GetChunkHash(double chunkX, double chunkZ)
		{
			return string.Format("{0}:{1}", chunkX, chunkZ);
		}

		private void SendPackage(UdpClient listener, IPEndPoint senderEndpoint, Package message, Reliability reliability = Reliability.RELIABLE)
		{
			ConnectedPackage package = new ConnectedPackage();
			var messageData = package.internalBuffer = message.GetBytes();
			package._reliability = reliability;
			package._reliableMessageNumber = _reliableMessageNumber++;
			package._sequenceNumber = _sequenceNumber++;
			package.Encode();
			byte[] data = package.GetBytes();

			if (message.Id != (decimal) DefaultMessageIdTypes.ID_CONNECTED_PONG && message.Id != (decimal) DefaultMessageIdTypes.ID_UNCONNECTED_PONG)
			{
				Debug.Print("< Send: {0:x2} {1} (0x{2:x2})", data[0], (DefaultMessageIdTypes) message.Id, message.Id);
				Debug.Print("\tData: Length={1} {0}", ByteArrayToString(data), messageData.Length);
			}
			SendRaw(listener, data, senderEndpoint);
		}

		private void SendAck(UdpClient listener, IPEndPoint senderEndpoint, Int24 sequenceNumber)
		{
			ConnectedPackage connectedPackage;
			var ack = new Ack();
			ack.sequenceNumber = sequenceNumber;
			ack.count = 1;
			ack.onlyOneSequence = 1;
			ack.Encode();
			RealRaw(listener, ack._buffer.ToArray(), senderEndpoint);
		}

		private Queue<Tuple<IPEndPoint, byte[]>> sendQueue = new Queue<Tuple<IPEndPoint, byte[]>>();
		private List<Chunk2> _chunkCache;

		private void sendTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			lock (sendQueue)
			{
				if (sendQueue.Count == 0) return;

				var item = sendQueue.Dequeue();
				RealRaw(_listener, item.Item2, item.Item1);
			}
		}

		private void SendRaw(UdpClient listener, byte[] data, IPEndPoint senderEndpoint)
		{
			lock (sendQueue)
			{
				sendQueue.Enqueue(new Tuple<IPEndPoint, byte[]>(senderEndpoint, data));
			}
		}

		private void RealRaw(UdpClient listener, byte[] data, IPEndPoint senderEndpoint)
		{
			listener.Send(data, data.Length, senderEndpoint);
			listener.BeginSend(data, data.Length, senderEndpoint, SendRequestCallback, listener);
		}

		private void SendRequestCallback(IAsyncResult ar)
		{
			UdpClient listener = (UdpClient) ar.AsyncState;
			listener.EndSend(ar);
		}


		public static string ByteArrayToString(byte[] ba)
		{
			StringBuilder hex = new StringBuilder((ba.Length*2) + 100);
			hex.Append("{");
			foreach (byte b in ba)
				hex.AppendFormat("0x{0:x2},", b);
			hex.Append("}");
			return hex.ToString();
		}


		private enum ConnectionState
		{
			Waiting,
			Connecting,
			Connecting2,
			Connected,
		}
	}
}
