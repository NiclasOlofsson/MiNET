using System;
using System.CodeDom.Compiler;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Sockets;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using fNbt;
using log4net;
using log4net.Config;
using MiNET.Blocks;
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
		private static readonly ILog Log = LogManager.GetLogger(typeof (MiNetClient));

		private IPEndPoint _clientEndpoint;
		private IPEndPoint _serverEndpoint;
		private short _mtuSize = 1447;
		private int _reliableMessageNumber = -1;
		private Vector3 _spawn;
		private long _entityId;
		public PlayerNetworkSession Session { get; set; }
		public int ChunkRadius { get; set; } = 5;

		public LevelInfo Level { get; } = new LevelInfo();
		private int _clientGuid;
		private Timer _connectedPingTimer;
		public bool HaveServer = false;
		public PlayerLocation CurrentLocation { get; set; }

		public bool IsEmulator { get; set; }

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
			//var client = new MiNetClient(new IPEndPoint(IPAddress.Parse("192.168.0.3"), 19132), "TheGrey");
			//var client = new MiNetClient(new IPEndPoint(IPAddress.Parse("147.75.192.106"), 19132), "TheGrey");
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

				Session = new PlayerNetworkSession(null, _clientEndpoint, 1300);

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
				Log.Error("Recieve processing", e);

				try
				{
					listener.BeginReceive(ReceiveCallback, listener);
				}
				catch (ObjectDisposedException dex)
				{
					// Log and move on. Should probably free up the player and remove them here.
					Log.Error("Recieve processing", dex);
				}

				return;
			}

			if (receiveBytes.Length != 0)
			{
				if (listener.Client == null) return;
				try
				{
					listener.BeginReceive(ReceiveCallback, listener);

					if (listener.Client == null) return;
					ProcessMessage(receiveBytes, senderEndpoint);
				}
				catch (Exception e)
				{
					Log.Error("Processing", e);
				}
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

					if (IsEmulator && PlayerStatus == 3)
					{
						int datagramId = new Int24(new[] {receiveBytes[1], receiveBytes[2], receiveBytes[3]});

						//Acks ack = Acks.CreateObject();
						Acks ack = new Acks();
						ack.acks.Add(datagramId);
						byte[] data = ack.Encode();
						ack.PutPool();
						SendData(data, senderEndpoint);

						return;
					}

					ConnectedPackage package = ConnectedPackage.CreateObject();
					//var package = new ConnectedPackage();
					package.Decode(receiveBytes);
					header = package._datagramHeader;
					//Log.Debug($"> Datagram #{header.datagramSequenceNumber}, {package._hasSplit}, {package._splitPacketId}, {package._reliability}, {package._reliableMessageNumber}, {package._sequencingIndex}, {package._orderingChannel}, {package._orderingIndex}");

					var messages = package.Messages;

					//Reliability reliability = package._reliability;
					//if (reliability == Reliability.Reliable
					//	|| reliability == Reliability.ReliableSequenced
					//	|| reliability == Reliability.ReliableOrdered
					//	)
					{
						Acks ack = Acks.CreateObject();
						ack.acks.Add(package._datagramSequenceNumber.IntValue());
						byte[] data = ack.Encode();
						ack.PutPool();
						SendData(data, senderEndpoint);
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
					package.PutPool();
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
			int spId = splitMessage.SplitId;
			int spIdx = splitMessage.SplitIdx;
			int spCount = splitMessage.SplitCount;

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

				MemoryStream stream = MiNetServer.MemoryStreamManager.GetStream();
				for (int i = 0; i < spPackets.Length; i++)
				{
					SplitPartPackage splitPartPackage = spPackets[i];
					byte[] buf = splitPartPackage.Message;
					stream.Write(buf, 0, buf.Length);
					splitPartPackage.PutPool();
				}

				byte[] buffer = stream.ToArray();

				byte id = buffer[0];
				if (id == 0x8e)
				{
					id = buffer[1];
				}

				Package fullMessage = PackageFactory.CreatePackage(id, buffer) ?? new UnknownPackage(id, buffer);
				fullMessage.DatagramSequenceNumber = package._datagramSequenceNumber;
				fullMessage.OrderingChannel = package._orderingChannel;
				fullMessage.OrderingIndex = package._orderingIndex;

				if (Log.IsDebugEnabled) Log.Debug($"Split: {fullMessage.GetType().Name} 0x{fullMessage.Id:x2}");
				;
				//if (!(fullMessage is McpeBatch)) Log.Debug($"Split: {fullMessage.GetType().Name} 0x{fullMessage.Id:x2} \n{Package.HexDump(buffer)}");

				HandlePackage(fullMessage);
				fullMessage.PutPool();
			}
		}

		public int PlayerStatus { get; set; }


		private void HandlePackage(Package message)
		{
			TraceReceive(message);

			//Log.Warn($"Package 0x{message.Id:X2} {message.GetType().Name}");

			if (typeof (McpeBatch) == message.GetType())
			{
				OnBatch(message);

				return;
			}

			else if (typeof (McpeDisconnect) == message.GetType())
			{
				McpeDisconnect msg = (McpeDisconnect) message;
				Log.InfoFormat("Disconnect {1}: {0}", msg.message, Username);
				//SendDisconnectionNotification();
				StopClient();
				return;
			}

			else if (typeof (ConnectedPing) == message.GetType())
			{
				ConnectedPing msg = (ConnectedPing) message;
				SendConnectedPong(msg.sendpingtime);
				return;
			}

			else if (typeof (McpeFullChunkData) == message.GetType())
			{
				OnFullChunkData(message);
				return;
			}

			else if (typeof (ConnectionRequestAccepted) == message.GetType())
			{
				OnConnectionRequestAccepted();
				return;
			}

			else if (typeof (McpeSetSpawnPosition) == message.GetType())
			{
				OnMcpeSetSpawnPosition(message);

				return;
			}

			else if (typeof (McpeStartGame) == message.GetType())
			{
				OnMcpeStartGame(message);

				return;
			}

			else if (typeof (McpeTileEvent) == message.GetType())
			{
				OnMcpeTileEvent(message);
				return;
			}

			else if (typeof (McpeTileEntityData) == message.GetType())
			{
				OnMcpeTileEntityData((McpeTileEntityData) message);
				return;
			}

			else if (typeof (McpeAddEntity) == message.GetType())
			{
				OnMcpeAddEntity(message);

				return;
			}
			else if (typeof (McpeAddItemEntity) == message.GetType())
			{
				OnMcpeAddItemEntity(message);

				return;
			}
			else if (typeof (McpeAddPlayer) == message.GetType())
			{
				OnMcpeAddPlayer(message);

				return;
			}
			else if (typeof (McpeSetEntityData) == message.GetType())
			{
				OnMcpeSetEntityData(message);
				return;
			}

			else if (typeof (McpeMovePlayer) == message.GetType())
			{
				//OnMcpeMovePlayer(message);
				return;
			}

			else if (typeof (McpeUpdateBlock) == message.GetType())
			{
				OnMcpeUpdateBlock(message);
				return;
			}

			else if (typeof (McpeLevelEvent) == message.GetType())
			{
				OnMcpeLevelEvent(message);
				return;
			}

			else if (typeof (McpeMobEffect) == message.GetType())
			{
				OnMcpeMobEffect(message);
				return;
			}

			else if (typeof (McpeSpawnExperienceOrb) == message.GetType())
			{
				OnMcpeSpawnExperienceOrb(message);
				return;
			}

			else if (typeof (McpePlayerEquipment) == message.GetType())
			{
				OnMcpePlayerEquipment((McpePlayerEquipment) message);
				return;
			}

			else if (typeof (McpeContainerSetContent) == message.GetType())
			{
				OnMcpeContainerSetContent(message);
				return;
			}

			else if (typeof (McpeContainerSetSlot) == message.GetType())
			{
				OnMcpeContainerSetSlot(message);
				return;
			}

			else if (typeof (McpeContainerSetData) == message.GetType())
			{
				OnMcpeContainerSetData(message);
				return;
			}

			else if (typeof (McpeCraftingData) == message.GetType())
			{
				OnMcpeCraftingData(message);
				return;
			}

			else if (typeof (McpeAdventureSettings) == message.GetType())
			{
				OnMcpeAdventureSettings((McpeAdventureSettings) message);
				return;
			}


			else if (typeof (McpeMoveEntity) == message.GetType())
			{
			}

			else if (typeof (McpeSetEntityMotion) == message.GetType())
			{
			}

			else if (typeof (McpeEntityEvent) == message.GetType())
			{
			}

			else if (typeof (McpeUpdateAttributes) == message.GetType())
			{
				OnMcpeUpdateAttributes((McpeUpdateAttributes) message);
			}

			else if (typeof (McpeText) == message.GetType())
			{
				OnMcpeText((McpeText) message);
			}

			else if (typeof (McpePlayerStatus) == message.GetType())
			{
				OnMcpePlayerStatus((McpePlayerStatus) message);
			}

			else if (typeof (McpeClientboundMapItemData) == message.GetType())
			{
				McpeClientboundMapItemData packet = (McpeClientboundMapItemData) message;
			}

			else if (typeof (McpeHurtArmor) == message.GetType())
			{
				OnMcpeHurtArmor((McpeHurtArmor) message);
			}

			else if (typeof (McpeAnimate) == message.GetType())
			{
				OnMcpeAnimate((McpeAnimate) message);
			}

			else if (typeof (McpeInteract) == message.GetType())
			{
				OnMcpeInteract((McpeInteract) message);
			}

			else if (typeof (UnknownPackage) == message.GetType())
			{
				UnknownPackage packet = (UnknownPackage) message;
				Log.Warn($"Unknown package 0x{message.Id:X2}\n{Package.HexDump(packet.Message)}");
			}


			else
			{
				Log.Warn($"Unhandled package 0x{message.Id:X2} {message.GetType().Name}");
			}
		}

		private void OnMcpeAdventureSettings(McpeAdventureSettings message)
		{
			Log.Info($"Adventure settings flags: 0x{message.flags:X2}");
		}

		private void OnMcpeInteract(McpeInteract message)
		{
			Log.Debug($"Interact: EID={message.targetEntityId}, Action ID={message.actionId}");
		}

		private void OnMcpeAnimate(McpeAnimate message)
		{
			Log.Debug($"Animate: EID={message.entityId}, Action ID={message.actionId}");
		}

		private void OnMcpeHurtArmor(McpeHurtArmor message)
		{
			Log.Debug($"Hurt Armor: Health={message.health}");
		}

		private void OnMcpePlayerStatus(McpePlayerStatus message)
		{
			if (Log.IsDebugEnabled) Log.Debug($"Player status={message.status}");
			PlayerStatus = message.status;
		}

		private void OnMcpeUpdateAttributes(McpeUpdateAttributes message)
		{
			if (Log.IsDebugEnabled)
				foreach (var playerAttribute in message.attributes)
				{
					Log.Debug($"Attribute {playerAttribute}");
				}
		}

		private void OnMcpeText(McpeText message)
		{
			if (Log.IsDebugEnabled) Log.Debug($"Text: {message.message}");

			if (message.message.Equals(".do"))
			{
				SendCraftingEvent();
			}
		}

		private void OnMcpePlayerEquipment(McpePlayerEquipment message)
		{
			if (Log.IsDebugEnabled) Log.Debug($"PlayerEquipment: Entity ID: {message.entityId}, Selected Slot: {message.selectedSlot}, Slot: {message.slot}, Item ID: {message.item.Id}");
		}

		private ShapedRecipe _recipeToSend = null;

		public void SendCraftingEvent2()
		{
			var recipe = _recipeToSend;

			if (recipe != null)
			{
				Log.Error("Sending crafting event: " + recipe.Id);

				McpeCraftingEvent crafting = McpeCraftingEvent.CreateObject();
				crafting.windowId = 0;
				crafting.recipeType = 1;
				crafting.recipeId = recipe.Id;

				{
					ItemStacks slotData = new ItemStacks();
					for (int i = 0; i < recipe.Input.Length; i++)
					{
						slotData.Add(recipe.Input[i]);

						McpeContainerSetSlot setSlot = McpeContainerSetSlot.CreateObject();
						setSlot.item = recipe.Input[i];
						setSlot.windowId = 0;
						setSlot.slot = (short) (i);
						SendPackage(setSlot);
						Log.Error("Set set slot");
					}
					crafting.input = slotData;

					{
						McpePlayerEquipment eq = McpePlayerEquipment.CreateObject();
						eq.entityId = _entityId;
						eq.slot = 9;
						eq.selectedSlot = 0;
						eq.item = recipe.Input[0];
						SendPackage(eq);
						Log.Error("Set eq slot");
					}
				}
				{
					ItemStacks slotData = new ItemStacks {recipe.Result};
					crafting.result = slotData;
				}

				SendPackage(crafting);
			}


			//{
			//	McpeContainerSetSlot setSlot = McpeContainerSetSlot.CreateObject();
			//	setSlot.item = new MetadataSlot(new ItemStack(new ItemDiamondAxe(0), 1));
			//	setSlot.windowId = 0;
			//	setSlot.slot = 0;
			//	SendPackage(setSlot);
			//}
			//{
			//	McpePlayerEquipment eq = McpePlayerEquipment.CreateObject();
			//	eq.entityId = _entityId;
			//	eq.slot = 9;
			//	eq.selectedSlot = 0;
			//	eq.item = new MetadataSlot(new ItemStack(new ItemDiamondAxe(0), 1));
			//	SendPackage(eq);
			//}
		}

		public void SendCraftingEvent()
		{
			var recipe = _recipeToSend;

			if (recipe != null)
			{
				{
					McpeContainerSetSlot setSlot = McpeContainerSetSlot.CreateObject();
					setSlot.item = new ItemBlock(new Block(17), 0) {Count = 1};
					setSlot.windowId = 0;
					setSlot.slot = 0;
					SendPackage(setSlot);
				}
				{
					McpePlayerEquipment eq = McpePlayerEquipment.CreateObject();
					eq.entityId = _entityId;
					eq.slot = 9;
					eq.selectedSlot = 0;
					eq.item = new ItemBlock(new Block(17), 0) {Count = 1};
					SendPackage(eq);
				}

				Log.Error("Sending crafting event: " + recipe.Id);

				McpeCraftingEvent crafting = McpeCraftingEvent.CreateObject();
				crafting.windowId = 0;
				crafting.recipeType = 1;
				crafting.recipeId = recipe.Id;

				{
					ItemStacks slotData = new ItemStacks {new ItemBlock(new Block(17), 0) {Count = 1}};
					crafting.input = slotData;
				}
				{
					ItemStacks slotData = new ItemStacks {new ItemBlock(new Block(5), 0) {Count = 1}};
					crafting.result = slotData;
				}

				SendPackage(crafting);

				//{
				//	McpeContainerSetSlot setSlot = McpeContainerSetSlot.CreateObject();
				//	setSlot.item = new MetadataSlot(new ItemStack(new ItemBlock(new Block(5), 0), 4));
				//	setSlot.windowId = 0;
				//	setSlot.slot = 0;
				//	SendPackage(setSlot);
				//}

				{
					McpePlayerEquipment eq = McpePlayerEquipment.CreateObject();
					eq.entityId = _entityId;
					eq.slot = 10;
					eq.selectedSlot = 1;
					eq.item = new ItemBlock(new Block(5), 0) {Count = 1};
					SendPackage(eq);
				}
			}
		}


		private void OnMcpeCraftingData(Package message)
		{
			if (IsEmulator) return;

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
					writer.WriteLine($"new ShapelessRecipe(new Item({shapelessRecipe.Result.Id}, {shapelessRecipe.Result.Metadata}, {shapelessRecipe.Result.Count}),");
					writer.Indent++;
					writer.WriteLine("new List<Item>");
					writer.WriteLine("{");
					writer.Indent++;
					foreach (var itemStack in shapelessRecipe.Input)
					{
						writer.WriteLine($"new Item({itemStack.Id}, {itemStack.Metadata}, {itemStack.Count}),");
					}
					writer.Indent--;
					writer.WriteLine("}),");
					writer.Indent--;

					continue;
				}

				ShapedRecipe shapedRecipe = recipe as ShapedRecipe;
				if (shapedRecipe != null && _recipeToSend == null)
				{
					if (shapedRecipe.Result.Id == 5 && shapedRecipe.Result.Count == 4 && shapedRecipe.Result.Metadata == 0)
					{
						Log.Error("Setting recipe! " + shapedRecipe.Id);
						_recipeToSend = shapedRecipe;
					}
				}
				if (shapedRecipe != null)
				{
					writer.WriteLine($"new ShapedRecipe({shapedRecipe.Width}, {shapedRecipe.Height}, new Item({shapedRecipe.Result.Id}, {shapedRecipe.Result.Metadata}, {shapedRecipe.Result.Count}),");
					writer.Indent++;
					writer.WriteLine("new Item[]");
					writer.WriteLine("{");
					writer.Indent++;
					foreach (Item item in shapedRecipe.Input)
					{
						writer.WriteLine($"new Item({item.Id}, {item.Metadata}),");
					}
					writer.Indent--;
					writer.WriteLine("}),");
					writer.Indent--;

					continue;
				}
				SmeltingRecipe smeltingRecipe = recipe as SmeltingRecipe;
				if (smeltingRecipe != null)
				{
					writer.WriteLine($"new SmeltingRecipe(new Item({smeltingRecipe.Result.Id}, {smeltingRecipe.Result.Metadata}, {smeltingRecipe.Result.Count}), new Item({smeltingRecipe.Input.Id}, {smeltingRecipe.Input.Metadata})),");
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
		}

		private void OnMcpeContainerSetData(Package msg)
		{
			McpeContainerSetData message = (McpeContainerSetData) msg;
			if (Log.IsDebugEnabled) Log.Debug($"Set container data window 0x{message.windowId:X2} with property ID: {message.property} value: {message.value}");
		}

		private void OnMcpeContainerSetSlot(Package msg)
		{
			McpeContainerSetSlot message = (McpeContainerSetSlot) msg;
			Item itemStack = message.item;
			if (Log.IsDebugEnabled) Log.Debug($"Set inventory slot on window 0x{message.windowId:X2} with slot: {message.slot} HOTBAR: {message.unknown} Item ID: {itemStack.Id} Item Count: {itemStack.Count} Meta: {itemStack.Metadata}: DatagramSequenceNumber: {message.DatagramSequenceNumber}, ReliableMessageNumber: {message.ReliableMessageNumber}, OrderingIndex: {message.OrderingIndex}");
		}

		private void OnMcpeContainerSetContent(Package message)
		{
			if (IsEmulator) return;

			McpeContainerSetContent msg = (McpeContainerSetContent) message;
			Log.Error($"Set container content on Window ID: 0x{msg.windowId:x2}, Count: {msg.slotData.Count}");
			var slots = msg.slotData;

			if (msg.windowId == 0x79)
			{
				string fileName = Path.GetTempPath() + "Inventory_0x79_" + Guid.NewGuid() + ".txt";
				WriteInventoryToFile(fileName, slots);
			}
			else if (msg.windowId == 0x00)
			{
				string fileName = Path.GetTempPath() + "Inventory_0x00_" + Guid.NewGuid() + ".txt";
				WriteInventoryToFile(fileName, slots);
				var hotbar = msg.hotbarData.GetValues();
				int i = 0;
				foreach (MetadataEntry entry in hotbar)
				{
					MetadataInt val = (MetadataInt) entry;
					Log.Error($"Hotbar slot: {i} val: {val.Value}");
					i++;
				}
			}
		}

		private void WriteInventoryToFile(string fileName, ItemStacks slots)
		{
			Log.Info($"Writing inventory to filename: {fileName}");
			FileStream file = File.OpenWrite(fileName);

			IndentedTextWriter writer = new IndentedTextWriter(new StreamWriter(file));

			writer.WriteLine("// GENERATED CODE. DON'T EDIT BY HAND");
			writer.Indent++;
			writer.Indent++;
			writer.WriteLine("public static List<Item> CreativeInventoryItems = new List<Item>()");
			writer.WriteLine("{");
			writer.Indent++;

			foreach (var entry in slots)
			{
				var slot = entry;
				NbtCompound extraData = slot.ExtraData;
				if (extraData == null)
				{
					writer.WriteLine($"new Item({slot.Id}, {slot.Metadata}, {slot.Count}),");
				}
				else
				{
					NbtList ench = (NbtList) extraData["ench"];
					NbtCompound enchComp = (NbtCompound) ench[0];
					var id = enchComp["id"].ShortValue;
					var lvl = enchComp["lvl"].ShortValue;
					writer.WriteLine($"new Item({slot.Id}, {slot.Metadata}, {slot.Count}){{ExtraData = new NbtCompound {{new NbtList(\"ench\") {{new NbtCompound {{new NbtShort(\"id\", {id}), new NbtShort(\"lvl\", {lvl}) }} }} }} }},");
				}
			}

			// Template
			new ItemAir {ExtraData = new NbtCompound {new NbtList("ench") {new NbtCompound {new NbtShort("id", 0), new NbtShort("lvl", 0)}}}};
			//var compound = new NbtCompound(string.Empty) { new NbtList("ench", new NbtCompound()) {new NbtShort("id", 0),new NbtShort("lvl", 0),}, };

			writer.Indent--;
			writer.WriteLine("};");
			writer.Indent--;
			writer.Indent--;

			writer.Flush();
			file.Close();
		}

		private void OnMcpeSpawnExperienceOrb(Package message)
		{
			McpeSpawnExperienceOrb msg = (McpeSpawnExperienceOrb) message;
			Log.DebugFormat("Event ID: {0}", msg.entityId);
			Log.DebugFormat("X: {0}", msg.x);
			Log.DebugFormat("Y: {0}", msg.y);
			Log.DebugFormat("Z: {0}", msg.z);
			Log.DebugFormat("count: {0}", msg.count);
		}

		private void OnMcpeMobEffect(Package message)
		{
			McpeMobEffect msg = (McpeMobEffect) message;
			Log.DebugFormat("operation: {0}", msg.eventId);
			Log.DebugFormat("entity id: {0}", msg.entityId);
			Log.DebugFormat("effectId: {0}", msg.effectId);
			Log.DebugFormat("amplifier: {0}", msg.amplifier);
			Log.DebugFormat("duration: {0}", msg.duration);
			Log.DebugFormat("particles: {0}", msg.particles);
		}

		private void OnMcpeLevelEvent(Package message)
		{
			McpeLevelEvent msg = (McpeLevelEvent) message;
			Log.DebugFormat("Event ID: {0}", msg.eventId);
			Log.DebugFormat("X: {0}", msg.x);
			Log.DebugFormat("Y: {0}", msg.y);
			Log.DebugFormat("Z: {0}", msg.z);
			Log.DebugFormat("Data: {0}", msg.data);
		}

		private void OnMcpeUpdateBlock(Package message)
		{
			McpeUpdateBlock msg = (McpeUpdateBlock) message;
			Log.DebugFormat("No of Blocks: {0}", msg.blocks.Count);
			foreach (Block block in msg.blocks)
			{
				Log.Debug($"Blocks ID={block.Id}, Metadata={block.Metadata}");
			}
		}

		private void OnMcpeMovePlayer(Package message)
		{
			McpeMovePlayer msg = (McpeMovePlayer) message;
			//Log.DebugFormat("McpeMovePlayer Entity ID: {0}", msg.entityId);

			CurrentLocation = new PlayerLocation(msg.x, msg.y + 10, msg.z);
			SendMcpeMovePlayer();
		}

		private static void OnMcpeSetEntityData(Package message)
		{
			McpeSetEntityData msg = (McpeSetEntityData) message;
			Log.DebugFormat("McpeSetEntityData Entity ID: {0}, Metadata: {1}", msg.entityId, msg.metadata);
		}

		private void OnMcpeAddPlayer(Package message)
		{
			if (IsEmulator) return;

			McpeAddPlayer msg = (McpeAddPlayer) message;
			Log.DebugFormat("McpeAddPlayer Entity ID: {0}", msg.entityId);
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
		}

		private void OnMcpeAddEntity(Package message)
		{
			if (IsEmulator) return;

			McpeAddEntity msg = (McpeAddEntity) message;
			Log.DebugFormat("McpeAddEntity Entity ID: {0}", msg.entityId);
			Log.DebugFormat("Entity Type: {0}", msg.entityType);
			Log.DebugFormat("X: {0}", msg.x);
			Log.DebugFormat("Y: {0}", msg.y);
			Log.DebugFormat("Z: {0}", msg.z);
			Log.DebugFormat("Yaw: {0}", msg.yaw);
			Log.DebugFormat("Pitch: {0}", msg.pitch);
			Log.DebugFormat("Velocity X: {0}", msg.speedX);
			Log.DebugFormat("Velocity Y: {0}", msg.speedY);
			Log.DebugFormat("Velocity Z: {0}", msg.speedZ);
			Log.DebugFormat("Metadata: {0}", msg.metadata);
			//Log.DebugFormat("Links count: {0}", msg.links);
		}

		private void OnMcpeAddItemEntity(Package message)
		{
			if (IsEmulator) return;

			McpeAddItemEntity msg = (McpeAddItemEntity) message;
			Log.DebugFormat("McpeAddEntity Entity ID: {0}", msg.entityId);
			Log.DebugFormat("X: {0}", msg.x);
			Log.DebugFormat("Y: {0}", msg.y);
			Log.DebugFormat("Z: {0}", msg.z);
			Log.DebugFormat("Velocity X: {0}", msg.speedX);
			Log.DebugFormat("Velocity Y: {0}", msg.speedY);
			Log.DebugFormat("Velocity Z: {0}", msg.speedZ);
			Log.Info($"Item {msg.item}");
		}

		private static void OnMcpeTileEntityData(McpeTileEntityData message)
		{
			Log.DebugFormat("X: {0}", message.x);
			Log.DebugFormat("Y: {0}", message.y);
			Log.DebugFormat("Z: {0}", message.z);
			Log.DebugFormat("NBT: {0}", message.namedtag.NbtFile);
		}

		private static void OnMcpeTileEvent(Package message)
		{
			McpeTileEvent msg = (McpeTileEvent) message;
			Log.DebugFormat("X: {0}", msg.x);
			Log.DebugFormat("Y: {0}", msg.y);
			Log.DebugFormat("Z: {0}", msg.z);
			Log.DebugFormat("Case 1: {0}", msg.case1);
			Log.DebugFormat("Case 2: {0}", msg.case2);
		}

		private void OnMcpeStartGame(Package message)
		{
			McpeStartGame msg = (McpeStartGame) message;
			_entityId = msg.entityId;
			_spawn = new Vector3(msg.x, msg.y, msg.z);

			Level.LevelName = "Default";
			Level.Version = 19133;
			Level.GameType = msg.gamemode;

			//ClientUtils.SaveLevel(_level);

			{
				var packet = McpeRequestChunkRadius.CreateObject();
				ChunkRadius = 5;
				packet.chunkRadius = ChunkRadius;

				SendPackage(packet);
			}
		}

		private void OnMcpeSetSpawnPosition(Package message)
		{
			McpeSetSpawnPosition msg = (McpeSetSpawnPosition) message;
			_spawn = new Vector3(msg.x, msg.y, msg.z);
			Level.SpawnX = (int) _spawn.X;
			Level.SpawnY = (int) _spawn.Y;
			Level.SpawnZ = (int) _spawn.Z;
			Log.Info($"Spawn position: {msg.x}, {msg.y}, {msg.z}");
		}

		private void OnConnectionRequestAccepted()
		{
			Thread.Sleep(50);
			SendNewIncomingConnection();
			//_connectedPingTimer = new Timer(state => SendConnectedPing(), null, 1000, 1000);
			Thread.Sleep(50);
			SendLogin(Username);
		}

		private int _numberOfChunks = 0;

		private ConcurrentDictionary<Tuple<int, int>, bool> _chunks = new ConcurrentDictionary<Tuple<int, int>, bool>();

		private void OnFullChunkData(Package message)
		{
			if (IsEmulator) return;

			McpeFullChunkData msg = (McpeFullChunkData) message;
			if (_chunks.TryAdd(new Tuple<int, int>(msg.chunkX, msg.chunkZ), true))
			{
				Log.Debug($"Chunk X={msg.chunkX}, Z={msg.chunkZ}, size={msg.chunkDataLength}, actualSize={msg.chunkData.Length}, Count={++_numberOfChunks}");

				ChunkColumn chunk = ClientUtils.DecocedChunkColumn(msg.chunkData);
				if (chunk != null)
				{
					chunk.x = msg.chunkX;
					chunk.z = msg.chunkZ;
					Log.DebugFormat("Chunk X={0}, Z={1}", chunk.x, chunk.z);

					//ClientUtils.SaveChunkToAnvil(chunk);
				}
			}
		}

		private void OnBatch(Package message)
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
				MemoryStream destination = MiNetServer.MemoryStreamManager.GetStream();
				defStream2.CopyTo(destination);
				destination.Position = 0;
				NbtBinaryReader reader = new NbtBinaryReader(destination, true);
				do
				{
					int len = reader.ReadInt32();
					byte[] internalBuffer = reader.ReadBytes(len);

					if (internalBuffer[0] == 0x8e) throw new Exception("Wrong code, didn't expect a 0x8E in a batched packet");

					var package = PackageFactory.CreatePackage(internalBuffer[0], internalBuffer) ?? new UnknownPackage(internalBuffer[0], internalBuffer);
					messages.Add(package);

					//if (Log.IsDebugEnabled) Log.Debug($"Batch: {package.GetType().Name} 0x{package.Id:x2}");
					//if (!(package is McpeFullChunkData)) Log.Debug($"Batch: {package.GetType().Name} 0x{package.Id:x2} \n{Package.HexDump(internalBuffer)}");
				} while (destination.Position < destination.Length);
			}
			//Log.Error($"Batch had {messages.Count} packets.");
			foreach (var msg in messages)
			{
				msg.DatagramSequenceNumber = batch.DatagramSequenceNumber;
				msg.OrderingChannel = batch.OrderingChannel;
				msg.OrderingIndex = batch.OrderingIndex;
				HandlePackage(msg);
				msg.PutPool();
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
				datagram.PutPool();

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
				UdpClient.Send(data, data.Length, targetEndpoint);
			}
			catch (Exception e)
			{
				Log.Debug("Send exception", e);
			}
		}

		private void TraceReceive(Package message)
		{
			if (!Log.IsDebugEnabled) return;

			if (message is McpeMoveEntity
			    || message is McpeAddEntity
			    || message is McpeCraftingData
			    || message is McpeContainerSetContent
			    || message is McpePlayerArmorEquipment
			    || message is McpeClientboundMapItemData
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
			if (!Log.IsDebugEnabled) return;

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
			var packet = NewIncomingConnection.CreateObject();
			packet.clientendpoint = _clientEndpoint;
			packet.systemAddresses = new IPEndPoint[10];
			for (int i = 0; i < 10; i++)
			{
				packet.systemAddresses[i] = new IPEndPoint(IPAddress.Any, 0);
			}

			SendPackage(packet);
		}

		public void SendLogin(string username)
		{
			Skin skin = new Skin {Slim = false, Texture = Encoding.Default.GetBytes(new string('Z', 8192))};
			skin.SkinType = "Standard_Custom";
			//Skin skin = new Skin { Slim = false, Texture = Encoding.Default.GetBytes(new string('Z', 16384)) };
			{
				var packet = McpeLogin.CreateObject();
				packet.username = username;
				packet.protocol = 81;
				packet.protocol2 = 81;
				packet.clientId = ClientId;
				packet.clientUuid = new UUID(Guid.NewGuid().ToByteArray());
				packet.serverAddress = _serverEndpoint.Address + ":" + _serverEndpoint.Port;
				packet.clientSecret = Encoding.ASCII.GetString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes("" + ClientId + _serverEndpoint.Address + _serverEndpoint.Port)));
				packet.skin = skin;

				SendPackage(packet);
			}

			//byte[] buffer = Player.CompressBytes(packet.Encode(), CompressionLevel.Fastest, true);

			//McpeBatch batch = McpeBatch.CreateObject();
			//batch.payloadSize = buffer.Length;
			//batch.payload = buffer;
			//batch.Encode();

			//SendPackage(batch);
			LoginSent = true;
		}

		public bool LoginSent { get; set; }

		public void SendChat(string text)
		{
			var packet = McpeText.CreateObject();
			packet.source = Username;
			packet.message = text;

			SendPackage(packet);
		}

		public void SendMcpeMovePlayer()
		{
			McpeMovePlayer movePlayerPacket = McpeMovePlayer.CreateObject();
			//McpeMovePlayer movePlayerPacket = new McpeMovePlayer();
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
