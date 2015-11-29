using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using log4net;
using log4net.Config;
using MiNET.Crafting;
using MiNET.Items;
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
		private IPEndPoint _serverEndpoint;
		private short _mtuSize = 1447;
		private int _reliableMessageNumber = -1;
		private Vector3 _spawn;
		private long _entityId;
		public PlayerNetworkSession Session { get; set; }

		private LevelInfo _level = new LevelInfo();
		private int _clientGuid;
		private Timer _connectedPingTimer;
		public bool HaveServer = false;
		public PlayerLocation CurrentLocation { get; set; }

		public UdpClient UdpClient { get; private set; }

		public string Username { get; set; }
		public int ClientId { get; set; }

		public MiNetClient(IPEndPoint endpoint, string username)
		{
			Username = username;
			ClientId = new Random().Next();
			_serverEndpoint = endpoint;
			_clientEndpoint = new IPEndPoint(IPAddress.Any, 0);
		}

		private static void Main(string[] args)
		{
			Console.WriteLine("Starting client...");

			var client = new MiNetClient(new IPEndPoint(Dns.GetHostEntry("test.bladestorm.net").AddressList[0], 19132), "TheGrey");
			//var client = new MiNetClient(new IPEndPoint(IPAddress.Parse("83.249.65.92"), 19132), "TheGrey");
			//var client = new MiNetClient(new IPEndPoint(IPAddress.Loopback, 19132), "TheGrey");

			client.StartClient();
			Console.WriteLine("Server started.");


			//while (!client.HaveServer)
			//{
			//	Thread.Sleep(500);
			//	Console.WriteLine("Sending ping...");
			//	client.SendUnconnectedPing();
			//}

			Console.WriteLine("<Enter> to connect!");
			Console.ReadLine();
			client.HaveServer = true;
			client.SendOpenConnectionRequest1();

			Console.WriteLine("<Enter> to exit!");
			Console.ReadLine();
			client.StopClient();
		}

		public void StartClient()
		{
			if (UdpClient != null) return;

			try
			{
				Log.Info("Initializing...");

				UdpClient = new UdpClient(_clientEndpoint)
				{
					Client =
					{
						ReceiveBufferSize = int.MaxValue,
						SendBufferSize = int.MaxValue
					},
					DontFragment = false
				};

				// SIO_UDP_CONNRESET (opcode setting: I, T==3)
				// Windows:  Controls whether UDP PORT_UNREACHABLE messages are reported.
				// - Set to TRUE to enable reporting.
				// - Set to FALSE to disable reporting.

				//uint IOC_IN = 0x80000000;
				//uint IOC_VENDOR = 0x18000000;
				//uint SIO_UDP_CONNRESET = IOC_IN | IOC_VENDOR | 12;
				//UdpClient.Client.IOControl((int) SIO_UDP_CONNRESET, new byte[] {Convert.ToByte(false)}, null);

				////
				////WARNING: We need to catch errors here to remove the code above.
				////

				Session = new PlayerNetworkSession(null, _clientEndpoint);

				UdpClient.BeginReceive(ReceiveCallback, UdpClient);
				_clientEndpoint = (IPEndPoint) UdpClient.Client.LocalEndPoint;

				Log.InfoFormat("Server open for business for {0}", Username);

				return;
			}
			catch (Exception e)
			{
				Log.Error("Main loop", e);
				StopClient();
			}
		}

		/// <summary>
		///     Stops the server.
		/// </summary>
		/// <returns></returns>
		public bool StopClient()
		{
			try
			{
				if (UdpClient == null) return true; // Already stopped. It's ok.

				UdpClient.Close();
				UdpClient = null;

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
				try
				{
					ProcessMessage(receiveBytes, senderEndpoint);
				}
				catch (Exception e)
				{
					Log.Error("Processing", e);
				}
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
						HaveServer = true;
						SendOpenConnectionRequest1();

						break;
					}
					case DefaultMessageIdTypes.ID_OPEN_CONNECTION_REPLY_1:
					{
						OpenConnectionReply1 incoming = (OpenConnectionReply1) message;
						_mtuSize = incoming.mtuSize;
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

					//ConnectedPackage package = ConnectedPackage.CreateObject();
					ConnectedPackage package = new ConnectedPackage();
					package.Decode(receiveBytes);
					header = package._datagramHeader;
					//Log.Debug(">\tReceive Datagram #" + package._datagramSequenceNumber.IntValue());
					Log.Debug($"> Datagram #{header.datagramSequenceNumber}, {header.isPacketPair}, {header.isContinuousSend}, {package._orderingChannel}, {package._orderingIndex}");

					var messages = package.Messages;

					//Reliability reliability = package._reliability;
					//if (reliability == Reliability.Reliable
					//	|| reliability == Reliability.ReliableSequenced
					//	|| reliability == Reliability.ReliableOrdered
					//	)
					{
						if (header.datagramSequenceNumber == 1000)
						{
							Log.Error("Datagram 1000 ignored");
						}
						else
						{
							// Send ACK
							Acks ack = new Acks();
							ack.acks.Add(package._datagramSequenceNumber.IntValue());
							byte[] data = ack.Encode();
							//Log.Info("Send ACK #" + package._datagramSequenceNumber.IntValue());
							SendData(data, senderEndpoint);
						}

					}

					//if (LoginSent) return; //HACK

					foreach (var message in messages)
					{
						if (message is SplitPartPackage)
						{
							lock (Session.SyncRoot)
							{
								HandleSplitMessage(Session, package, (SplitPartPackage) message);
							}

							continue;
						}

						message.Timer.Restart();
						HandlePackage(message);
						message.PutPool();
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
			//Log.Info("Ack");
		}

		private void HandleNak(byte[] receiveBytes, IPEndPoint senderEndpoint)
		{
			Log.Warn("!! WHAT THE FUK NAK NAK NAK");
		}

		private void HandleSplitMessage(PlayerNetworkSession playerSession, ConnectedPackage package, SplitPartPackage splitMessage)
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
					stream.Write(buf, 0, buf.Length);
					splitPartPackage.PutPool();
				}

				byte[] buffer = stream.ToArray();

				Package fullMessage = PackageFactory.CreatePackage(buffer[0], buffer) ?? new UnknownPackage(buffer[0], buffer);
				fullMessage.DatagramSequenceNumber = package._datagramSequenceNumber;
				fullMessage.OrderingChannel = package._orderingChannel;
				fullMessage.OrderingIndex = package._orderingIndex;
				HandlePackage(fullMessage);
				fullMessage.PutPool();
			}
		}

		private void HandlePackage(Package message)
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
					destination.Position = 0;
					NbtBinaryReader reader = new NbtBinaryReader(destination, true);
					do
					{
						int len = reader.ReadInt32();
						byte[] internalBuffer = reader.ReadBytes(len);
						messages.Add(PackageFactory.CreatePackage(internalBuffer[0], internalBuffer) ?? new UnknownPackage(internalBuffer[0], internalBuffer));
					} while (destination.Position < destination.Length);
				}
				foreach (var msg in messages)
				{
					msg.DatagramSequenceNumber = batch.DatagramSequenceNumber;
					msg.OrderingChannel = batch.OrderingChannel;
					msg.OrderingIndex = batch.OrderingIndex;
					HandlePackage(msg);
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
				//SendDisconnectionNotification();
				StopClient();
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
				McpeFullChunkData msg = (McpeFullChunkData)message;
				ChunkColumn chunk = ClientUtils.DecocedChunkColumn(msg.chunkData);
				if (chunk != null)
				{
					Log.DebugFormat("Chunk X={0}, Z={1}", chunk.x, chunk.z);

					//ClientUtils.SaveChunkToAnvil(chunk);
				}
				return;
			}

			if (typeof (ConnectionRequestAccepted) == message.GetType())
			{
				Thread.Sleep(50);
				SendNewIncomingConnection();
				//_connectedPingTimer = new Timer(state => SendConnectedPing(), null, 1000, 1000);
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
				Log.DebugFormat("Velocity Z: {0}", msg.metadata);
				//Log.DebugFormat("Links count: {0}", msg.links);

				return;
			}
			if (typeof(McpeAddPlayer) == message.GetType())
			{
				McpeAddPlayer msg = (McpeAddPlayer)message;
				Log.DebugFormat("Entity ID: {0}", msg.entityId);
				Log.DebugFormat("X: {0}", msg.x);
				Log.DebugFormat("Y: {0}", msg.y);
				Log.DebugFormat("Z: {0}", msg.z);
				Log.DebugFormat("Yaw: {0}", msg.yaw);
				Log.DebugFormat("Pitch: {0}", msg.pitch);
				Log.DebugFormat("Velocity X: {0}", msg.speedX);
				Log.DebugFormat("Velocity Y: {0}", msg.speedY);
				Log.DebugFormat("Velocity Z: {0}", msg.speedZ);
				Log.DebugFormat("Metadata: {0}", msg.metadata.ToString());
				//Log.DebugFormat("Links count: {0}", msg.links);

				return;
			}
			if (typeof (McpeSetEntityData) == message.GetType())
			{
				McpeSetEntityData msg = (McpeSetEntityData) message;
				Log.DebugFormat("Entity ID: {0}", msg.entityId);
				MetadataDictionary metadata = msg.metadata;
				if(metadata.Contains(17))
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

			if (typeof(McpeMobEffect) == message.GetType())
			{
				McpeMobEffect msg = (McpeMobEffect)message;
				Log.DebugFormat("operation: {0}", msg.eventId);
				Log.DebugFormat("entity id: {0}", msg.entityId);
				Log.DebugFormat("effectId: {0}", msg.effectId);
				Log.DebugFormat("amplifier: {0}", msg.amplifier);
				Log.DebugFormat("duration: {0}", msg.duration);
				Log.DebugFormat("particles: {0}", msg.particles);
				return;
			}

			if (typeof(McpeSpawnExperienceOrb) == message.GetType())
			{
				McpeSpawnExperienceOrb msg = (McpeSpawnExperienceOrb)message;
				Log.DebugFormat("Event ID: {0}", msg.entityId);
				Log.DebugFormat("X: {0}", msg.x);
				Log.DebugFormat("Y: {0}", msg.y);
				Log.DebugFormat("Z: {0}", msg.z);
				Log.DebugFormat("count: {0}", msg.count);
				return;
			}

			if (typeof (McpeContainerSetContent) == message.GetType())
			{
				McpeContainerSetContent msg = (McpeContainerSetContent) message;
				Log.DebugFormat("Window ID: 0x{0:x2}, Count: {1}", msg.windowId, msg.slotData.Count);
				var slots = msg.slotData.GetValues();

				foreach (var entry in slots)
				{
					MetadataSlot slot = (MetadataSlot) entry;
					//Log.DebugFormat(" - Id: {0}, Metadata: {1}, Count: {2}", slot.Value.Item.Id, slot.Value.Item.Metadata, slot.Value.Count);
				}
				return;
			}

			if (typeof (McpeCraftingData) == message.GetType())
			{
				string fileName = Path.GetTempPath() + "Recipes_" + Guid.NewGuid() + ".txt";
				Log.Info("Writing recipes to filename: " + fileName);
				FileStream file = File.OpenWrite(fileName);

				McpeCraftingData msg = (McpeCraftingData) message;
				IndentedTextWriter writer = new IndentedTextWriter(new StreamWriter(file));

				writer.WriteLine("static RecipeManager()");
				writer.WriteLine("{");
				writer.Indent++;
				writer.WriteLine("Recipes = new Recipes");
				writer.WriteLine("{");
				writer.Indent++;

				foreach (Recipe recipe in msg.recipes)
				{
					ShapelessRecipe shapelessRecipe = recipe as ShapelessRecipe;
					if (shapelessRecipe != null)
					{
						writer.WriteLine(string.Format("new ShapelessRecipe(new ItemStack(ItemFactory.GetItem({0}, {1}), {2}),", shapelessRecipe.Result.Id, shapelessRecipe.Result.Metadata, shapelessRecipe.Result.Count));
						writer.Indent++;
						writer.WriteLine("new List<ItemStack>");
						writer.WriteLine("{");
						writer.Indent++;
						foreach (var itemStack in shapelessRecipe.Input)
						{
							writer.WriteLine(string.Format("new ItemStack(ItemFactory.GetItem({0}, {1}), {2}),", itemStack.Id, itemStack.Metadata, itemStack.Count));
						}
						writer.Indent--;
						writer.WriteLine("}),");
						writer.Indent--;

						continue;
					}

					ShapedRecipe shapedRecipe = recipe as ShapedRecipe;
					if (shapedRecipe != null)
					{
						writer.WriteLine(string.Format("new ShapedRecipe({0}, {1}, new ItemStack(ItemFactory.GetItem({2}, {3}), {4}),", shapedRecipe.Width, shapedRecipe.Height, shapedRecipe.Result.Id, shapedRecipe.Result.Metadata, shapedRecipe.Result.Count));
						writer.Indent++;
						writer.WriteLine("new Item[]");
						writer.WriteLine("{");
						writer.Indent++;
						foreach (Item item in shapedRecipe.Input)
						{
							writer.WriteLine(string.Format("ItemFactory.GetItem({0}, {1}),", item.Id, item.Metadata));
						}
						writer.Indent--;
						writer.WriteLine("}),");
						writer.Indent--;

						continue;
					}
				}

				writer.WriteLine("};");
				writer.Indent--;
				writer.WriteLine("}");
				writer.Indent--;

				writer.Flush();

				file.Close();
				//Environment.Exit(0);
				return;
			}
		}

		public void SendPackage(Package message, short mtuSize, ref int reliableMessageNumber)
		{
			if (message == null) return;

			TraceSend(message);

			foreach (var datagram in Datagram.CreateDatagrams(message, mtuSize, Session))
			{
				SendDatagram(Session, datagram);
			}
		}

		private void SendDatagram(PlayerNetworkSession session, Datagram datagram)
		{
			if (datagram.MessageParts.Count != 0)
			{
				datagram.Header.datagramSequenceNumber = Interlocked.Increment(ref Session.DatagramSequenceNumber);
				byte[] data = datagram.Encode();

				datagram.Timer.Restart();
				SendData(data, _serverEndpoint);
			}
		}


		private void SendData(byte[] data)
		{
			SendData(data, _serverEndpoint);
		}


		private void SendData(byte[] data, IPEndPoint targetEndpoint)
		{
			if (UdpClient == null) return;
			try
			{
				UdpClient.SendAsync(data, data.Length, targetEndpoint);
			}
			catch (Exception e)
			{
				Log.Debug("Send exception", e);
			}
		}

		private void TraceReceive(Package message)
		{
			if (message is McpeMoveEntity 
				|| message is McpeMovePlayer 
				|| message is McpeSetEntityMotion
				|| message is McpeBatch 
				|| message is McpeFullChunkData 
				|| message is ConnectedPing) return;

			var stringWriter = new StringWriter();
			ObjectDumper.Write(message, 1, stringWriter);

			//Log.DebugFormat("> Receive: {0}: {1} (0x{0:x2})", message.Id, message.GetType().Name);
			Log.DebugFormat("> Receive: {0} (0x{0:x2}) {1}:\n{2} ", message.Id, message.GetType().Name, stringWriter.ToString());
		}

		private void TraceSend(Package message)
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
			//SendData(data);
			SendData(data, new IPEndPoint(IPAddress.Broadcast, 19132));
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
				raknetProtocolVersion = 7, // Indicate to the server that this is a performance tests. Disables logging.
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
			_clientGuid = new Random().Next();
			var packet = new OpenConnectionRequest2()
			{
				clientendpoint = _clientEndpoint,
				mtuSize = _mtuSize,
				clientGuid = _clientGuid,
			};

			var data = packet.Encode();

			TraceSend(packet);

			SendData(data);
		}

		public void SendConnectionRequest()
		{
			var packet = new ConnectionRequest()
			{
				clientGuid = _clientGuid,
				timestamp = DateTime.UtcNow.Ticks,
				doSecurity = 0,
			};

			SendPackage(packet);
		}

		private void SendPackage(Package package)
		{
			SendPackage(package, _mtuSize, ref _reliableMessageNumber);
			package.PutPool();
		}

		public void SendNewIncomingConnection()
		{
			Random rand = new Random();
			var packet = new NewIncomingConnection
			{
				doSecurity = 163,
				session = rand.Next(),
				session2 = rand.Next(),
				cookie = rand.Next(),
				port = (short) _clientEndpoint.Port
			};

			SendPackage(packet);
		}

		public void SendLogin(string username)
		{
			Skin skin = new Skin {Slim = false, Texture = Encoding.Default.GetBytes(new string('Z', 8192))};
			//Skin skin = new Skin { Slim = false, Texture = Encoding.Default.GetBytes(new string('Z', 16384)) };
			var packet = new McpeLogin()
			{
				username = username,
				protocol = 38,
				protocol2 = 38,
				clientId = ClientId,
				clientUuid = new UUID(Guid.NewGuid().ToByteArray()),
				serverAddress = _serverEndpoint.Address + ":" + _serverEndpoint.Port,
				clientSecret = "iwmvi45hm85oncyo58",
				//clientSecret = Encoding.ASCII.GetString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes("" + ClientId + _serverEndpoint.Address + _serverEndpoint.Port))),
				skin = skin,
			};

			byte[] buffer = Player.CompressBytes(packet.Encode(), CompressionLevel.Fastest, true);

			McpeBatch batch = new McpeBatch();
			batch.payloadSize = buffer.Length;
			batch.payload = buffer;
			batch.Encode();

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
	}
}