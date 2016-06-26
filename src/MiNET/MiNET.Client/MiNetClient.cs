using System;
using System.CodeDom.Compiler;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
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

			//var client = new MiNetClient(new IPEndPoint(Dns.GetHostEntry("test.bladestorm.net").AddressList[0], 19132), "TheGrey");
			//var client = new MiNetClient(new IPEndPoint(IPAddress.Parse("192.168.0.3"), 19134), "TheGrey");
			//var client = new MiNetClient(new IPEndPoint(IPAddress.Parse("147.75.192.106"), 19132), "TheGrey");
			var client = new MiNetClient(new IPEndPoint(IPAddress.Loopback, 19134), "TheGrey");

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

				Package message = PackageFactory.CreatePackage(msgId, receiveBytes, "raknet");

				if (message == null) return;

				TraceReceive(message);

				switch (msgIdType)
				{
					case DefaultMessageIdTypes.ID_UNCONNECTED_PONG:
					{
						UnconnectedPong incoming = (UnconnectedPong) message;
						if (!HaveServer)
						{
							HaveServer = true;
							SendOpenConnectionRequest1();
						}

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
						HandleOpenConnectionReply2((OpenConnectionReply2) message);
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

		private void HandleOpenConnectionReply2(OpenConnectionReply2 message)
		{
			SendConnectionRequest();
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

				Package fullMessage = PackageFactory.CreatePackage(id, buffer, "raknet") ?? new UnknownPackage(id, buffer);
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


		public AutoResetEvent FirstPacketWaitHandle = new AutoResetEvent(false);

		private void HandlePackage(Package message)
		{
			TraceReceive(message);

			//Log.Warn($"Package 0x{message.Id:X2} {message.GetType().Name}");

			if (typeof (McpeWrapper) == message.GetType())
			{
				OnWrapper((McpeWrapper) message);

				FirstPacketWaitHandle.Set();

				return;
			}

			if (typeof (McpeServerExchange) == message.GetType())
			{
				OnMcpeServerExchange((McpeServerExchange) message);

				return;
			}

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

			else if (typeof (McpeBlockEvent) == message.GetType())
			{
				OnMcpeTileEvent(message);
				return;
			}

			else if (typeof (McpeBlockEntityData) == message.GetType())
			{
				OnMcpeTileEntityData((McpeBlockEntityData) message);
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

			else if (typeof (McpeUpdateBlock) == message.GetType())
			{
				OnMcpeUpdateBlock(message);
				return;
			}

			else if (typeof (McpeMovePlayer) == message.GetType())
			{
				OnMcpeMovePlayer((McpeMovePlayer) message);
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

			else if (typeof (McpeMobEquipment) == message.GetType())
			{
				OnMcpePlayerEquipment((McpeMobEquipment) message);
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
				if (Log.IsDebugEnabled) Log.Warn($"Unknown package 0x{message.Id:X2}\n{Package.HexDump(packet.Message)}");
			}


			else
			{
				if (Log.IsDebugEnabled) Log.Warn($"Unhandled package 0x{message.Id:X2} {message.GetType().Name}\n{Package.HexDump(message.Bytes)}");
			}
		}

		public void SendLogin(string username)
		{
			//{
			//	"exp": 1464983845,
			//	"extraData": {
			//		"displayName": "gurunx",
			//		"identity": "af6f7c5e-fcea-3e43-bf3a-e005e400e578"
			//	},
			//	"identityPublicKey": "MHYwEAYHKoZIzj0CAQYFK4EEACIDYgAE7nnZpCfxmCrSwDdBv7eBXXMtKhroxOriEr3hmMOJAuw/ZpQXj1K5GGtHS4CpFNttd1JYAKYoJxYgaykpie0EyAv3qiK6utIH2qnOAt3VNrQYXfIZJS/VRe3Il8Pgu9CB",
			//	"nbf": 1464983844
			//}

			//ECDiffieHellmanCng ecKey = new ECDiffieHellmanCng(384);
			//ecKey.HashAlgorithm = CngAlgorithm.Sha256;
			//ecKey.KeyDerivationFunction = ECDiffieHellmanKeyDerivationFunction.Hash;

			//string publicKey = Convert.ToBase64String(ecKey.PublicKey.GetDerEncoded());

			//JWT.Encode("payload", ecKey, JwsAlgorithm.ES384);

			//Skin skin = new Skin { Slim = false, Texture = Encoding.Default.GetBytes(new string('Z', 8192)) };
			//skin.SkinType = "Standard_Custom";

			////Skin skin = new Skin { Slim = false, Texture = Encoding.Default.GetBytes(new string('Z', 16384)) };
			//{
			//	var packet = new McpeLogin()
			//	{
			//		username = username,
			//		protocol = 45,
			//		protocol2 = 45,
			//		clientId = ClientId,
			//		clientUuid = new UUID(Guid.NewGuid().ToByteArray()),
			//		serverAddress = _serverEndpoint.Address + ":" + _serverEndpoint.Port,
			//		//clientSecret = "iwmvi45hm85oncyo58",
			//		clientSecret = Encoding.ASCII.GetString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes("" + ClientId + _serverEndpoint.Address + _serverEndpoint.Port))),
			//		skin = skin,
			//	};

			//	SendPackage(packet);
			//}

			McpeLogin loginPacket = new McpeLogin
			{
				protocolVersion = 81,
				payloadLenght = 8697,
				payload = Convert.FromBase64String("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")
			};

			McpeBatch batch = new McpeBatch
			{
				payloadSize = Player.CompressBytes(loginPacket.Encode(), CompressionLevel.Fastest, true).Length,
				payload = Player.CompressBytes(loginPacket.Encode(), CompressionLevel.Fastest, true)
			};

			//McpeWrapper wrapper = new McpeWrapper
			//{
			//	payload = batch.Encode()
			//};

			Session.CryptoContext = new CryptoContext();

			SendPackage(batch);
			LoginSent = true;
		}

		private void OnMcpeServerExchange(McpeServerExchange message)
		{
			var serverKey = message.serverPublicKey;
			var randomKeyToken = message.randomKeyToken;

			// Initiate encryption

			InitiateEncryption(serverKey, randomKeyToken);
		}

		private void InitiateEncryption(string serverKey, string randomKeyToken)
		{
			{
				string certString = serverKey;

				ECDiffieHellmanPublicKey publicKey = CryptoUtils.CreateEcDiffieHellmanPublicKey(certString);
				Log.Debug($"Cert:\n{publicKey.ToXmlString()}");

				// Create shared shared secret
				ECDiffieHellmanCng ecKey = new ECDiffieHellmanCng(384);
				ecKey.HashAlgorithm = CngAlgorithm.Sha256;
				ecKey.KeyDerivationFunction = ECDiffieHellmanKeyDerivationFunction.Hash;
				ecKey.SecretPrepend = Encoding.UTF8.GetBytes(randomKeyToken); // Server token

				byte[] secret = ecKey.DeriveKeyMaterial(publicKey);

				Log.Debug($"SECRET KEY (b64):\n{Convert.ToBase64String(secret)}");

				{
					RijndaelManaged rijAlg = new RijndaelManaged
					{
						BlockSize = 128,
						Padding = PaddingMode.None,
						Mode = CipherMode.CFB,
						FeedbackSize = 8,
						Key = secret,
						IV = secret.Take(16).ToArray(),
					};

					// Create a decrytor to perform the stream transform.
					ICryptoTransform decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);
					MemoryStream inputStream = new MemoryStream();
					CryptoStream cryptoStreamIn = new CryptoStream(inputStream, decryptor, CryptoStreamMode.Read);

					ICryptoTransform encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);
					MemoryStream outputStream = new MemoryStream();
					CryptoStream cryptoStreamOut = new CryptoStream(outputStream, encryptor, CryptoStreamMode.Write);

					Session.CryptoContext = new CryptoContext
					{
						Algorithm = rijAlg,
						Decryptor = decryptor,
						Encryptor = encryptor,
						InputStream = inputStream,
						OutputStream = outputStream,
						CryptoStreamIn = cryptoStreamIn,
						CryptoStreamOut = cryptoStreamOut,
					};
				}

				McpeClientMagic magic = new McpeClientMagic();
				SendPackage(magic);
			}
		}

		private void OnWrapper(McpeWrapper message)
		{
			// Get bytes
			byte[] payload = message.payload;
			//if (playerSession.CryptoContext != null && Config.GetProperty("UseEncryption", true))
			//{
			//	payload = CryptoUtils.Decrypt(payload, playerSession.CryptoContext);
			//}

			//if (Log.IsDebugEnabled)
			//	Log.Debug($"0x{payload[0]:x2}\n{Package.HexDump(payload)}");

			Package newMessage = PackageFactory.CreatePackage(payload[0], payload, "mcpe") ?? new UnknownPackage(payload[0], payload);
			HandlePackage(newMessage);
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

		private void OnMcpePlayerEquipment(McpeMobEquipment message)
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
						McpeMobEquipment eq = McpeMobEquipment.CreateObject();
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
					McpeMobEquipment eq = McpeMobEquipment.CreateObject();
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
					McpeMobEquipment eq = McpeMobEquipment.CreateObject();
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

		private void OnMcpeMovePlayer(McpeMovePlayer message)
		{
			//Log.DebugFormat("McpeMovePlayer Entity ID: {0}", msg.entityId);

			if (message.entityId != _entityId) return;

			CurrentLocation = new PlayerLocation(message.x, message.y + 1, message.z);
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

		private static void OnMcpeTileEntityData(McpeBlockEntityData message)
		{
			Log.DebugFormat("X: {0}", message.x);
			Log.DebugFormat("Y: {0}", message.y);
			Log.DebugFormat("Z: {0}", message.z);
			Log.DebugFormat("NBT: {0}", message.namedtag.NbtFile);
		}

		private static void OnMcpeTileEvent(Package message)
		{
			McpeBlockEvent msg = (McpeBlockEvent) message;
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

					var package = PackageFactory.CreatePackage(internalBuffer[0], internalBuffer, "mcpe") ?? new UnknownPackage(internalBuffer[0], internalBuffer);
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
			    || message is McpeMobArmorEquipment
			    || message is McpeClientboundMapItemData
			    || message is McpeMovePlayer
			    || message is McpeSetEntityMotion
			    || message is McpeBatch
			    || message is McpeFullChunkData
			    || message is McpeWrapper
			    || message is ConnectedPing) return;

			var stringWriter = new StringWriter();
			ObjectDumper.Write(message, 1, stringWriter);

			Log.DebugFormat("> Receive: {0}: {1} (0x{0:x2})", message.Id, message.GetType().Name);
			//Log.DebugFormat("> Receive: {0} (0x{0:x2}) {1}:\n{2} ", message.Id, message.GetType().Name, stringWriter.ToString());
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
				raknetProtocolVersion = 8,
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
			movePlayerPacket.entityId = _entityId;
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
			Session.CryptoContext = null;
			SendPackage(new DisconnectionNotification());
		}
	}
}