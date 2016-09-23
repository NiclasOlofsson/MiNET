using System;
using System.CodeDom.Compiler;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using fNbt;
using Jose;
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
		private readonly DedicatedThreadPool _threadPool;
		private short _mtuSize = 1192;
		private int _reliableMessageNumber = -1;
		private Vector3 _spawn;
		private long _entityId;
		public PlayerNetworkSession Session { get; set; }
		public int ChunkRadius { get; set; } = 5;

		public LevelInfo Level { get; } = new LevelInfo();
		private long _clientGuid = new Random().Next();
		private Timer _connectedPingTimer;
		public bool HaveServer = false;
		public PlayerLocation CurrentLocation { get; set; }

		public bool IsEmulator { get; set; }

		public UdpClient UdpClient { get; private set; }

		public string Username { get; set; }
		public int ClientId { get; set; }

		public MiNetClient(IPEndPoint endpoint, string username, DedicatedThreadPool threadPool)
		{
			Username = username;
			ClientId = new Random().Next();
			_serverEndpoint = endpoint;
			_threadPool = threadPool;
			if (_serverEndpoint != null) Log.Warn("Connecting to: " + _serverEndpoint);
			_clientEndpoint = new IPEndPoint(IPAddress.Any, 0);
		}

		private static void Main(string[] args)
		{
			Console.WriteLine("Starting client...");

			int threads;
			int iothreads;
			ThreadPool.GetMinThreads(out threads, out iothreads);

			//var client = new MiNetClient(null, "TheGrey", new DedicatedThreadPool(new DedicatedThreadPoolSettings(Environment.ProcessorCount)));
			//var client = new MiNetClient(new IPEndPoint(Dns.GetHostEntry("pe.mineplex.com").AddressList[0], 19132), "TheGrey");
			//var client = new MiNetClient(new IPEndPoint(Dns.GetHostEntry("yodamine.net").AddressList[0], 19132), "TheGrey");
			//var client = new MiNetClient(new IPEndPoint(IPAddress.Parse("147.75.192.106"), 19132), "TheGrey");
			//var client = new MiNetClient(new IPEndPoint(IPAddress.Loopback, 19132), "TheGrey");

			var client = new MiNetClient(new IPEndPoint(IPAddress.Parse("192.168.0.5"), 19132), "TheGrey", new DedicatedThreadPool(new DedicatedThreadPoolSettings(Environment.ProcessorCount)));
			//var client = new MiNetClient(new IPEndPoint(IPAddress.Loopback, 19132), "TheGrey", new DedicatedThreadPool(new DedicatedThreadPoolSettings(Environment.ProcessorCount)));

			// 157.7.202.57:29473
			//var client = new MiNetClient(new IPEndPoint(IPAddress.Parse("157.7.202.57"), 19132), "TheGrey");

			client.StartClient();
			Console.WriteLine("Server started.");

			if (client._serverEndpoint != null)
			{
				while (!client.HaveServer)
				{
					Console.WriteLine("Sending ping...");
					client.SendUnconnectedPing();
					Thread.Sleep(500);
				}
			}
			//else
			//{
			//	client.HaveServer = true;
			//	client.SendOpenConnectionRequest1();
			//	Thread.Sleep(50);
			//	client.SendOpenConnectionRequest1();
			//	Thread.Sleep(50);
			//	client.SendOpenConnectionRequest1();
			//}

			//Task.Delay(20000).ContinueWith(task =>
			//{
			//	Log.Error("Sending server command");
			//	client.SendChat("/server lobby-1");
			//});

			Console.WriteLine("<Enter> to exit!");
			Console.ReadLine();
			client.StopClient();
		}

		public void StartClient()
		{
			McpeSetSpawnPosition spawn = new McpeSetSpawnPosition();
			spawn.coordinates = new BlockCoordinates(0, 0, 0);
			var bytes = spawn.Encode();
			Log.Debug($"\n{Package.HexDump(bytes)}");

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

				uint IOC_IN = 0x80000000;
				uint IOC_VENDOR = 0x18000000;
				uint SIO_UDP_CONNRESET = IOC_IN | IOC_VENDOR | 12;
				UdpClient.Client.IOControl((int) SIO_UDP_CONNRESET, new byte[] {Convert.ToByte(false)}, null);

				////
				////WARNING: We need to catch errors here to remove the code above.
				////

				//Task.Run(ProcessQueue);

				Session = new PlayerNetworkSession(null, null, _clientEndpoint, _mtuSize);

				//UdpClient.BeginReceive(ReceiveCallback, UdpClient);
				new Thread(ProcessDatagrams) {IsBackground = true}.Start(UdpClient);

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
			//Environment.Exit(0);
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

		private void ProcessDatagrams(object state)
		{
			while (true)
			{
				UdpClient listener = (UdpClient) state;


				// Check if we already closed the server
				if (listener.Client == null) return;

				// WSAECONNRESET:
				// The virtual circuit was reset by the remote side executing a hard or abortive close. 
				// The application should close the socket; it is no longer usable. On a UDP-datagram socket 
				// this error indicates a previous send operation resulted in an ICMP Port Unreachable message.
				// Note the spocket settings on creation of the server. It makes us ignore these resets.
				IPEndPoint senderEndpoint = null;
				Byte[] receiveBytes = null;
				try
				{
					//var result = listener.ReceiveAsync().Result;
					//senderEndpoint = result.RemoteEndPoint;
					//receiveBytes = result.Buffer;
					receiveBytes = listener.Receive(ref senderEndpoint);

					if (receiveBytes.Length != 0)
					{
						_threadPool.QueueUserWorkItem(() =>
						{
							try
							{
								ProcessMessage(receiveBytes, senderEndpoint);
							}
							catch (Exception e)
							{
								Log.Warn(string.Format("Process message error from: {0}", senderEndpoint.Address), e);
							}
						});
					}
					else
					{
						Log.Error("Unexpected end of transmission?");
						return;
					}
				}
				catch (Exception e)
				{
					Log.Error("Unexpected end of transmission?", e);
					if (listener.Client != null)
					{
						continue;
					}

					return;
				}
			}
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
						Log.Warn($"MOTD {incoming.serverName}");
						if (!HaveServer)
						{
							_serverEndpoint = senderEndpoint;
							HaveServer = true;
							SendOpenConnectionRequest1();
						}

						break;
					}
					case DefaultMessageIdTypes.ID_OPEN_CONNECTION_REPLY_1:
					{
						OpenConnectionReply1 incoming = (OpenConnectionReply1) message;
						if (incoming.mtuSize != _mtuSize) Log.Warn("Error:" + incoming.mtuSize);
						Log.Warn("Server Has Security" + incoming.serverHasSecurity);
						_mtuSize = incoming.mtuSize;
						SendOpenConnectionRequest2();
						break;
					}
					case DefaultMessageIdTypes.ID_OPEN_CONNECTION_REPLY_2:
					{
						OnOpenConnectionReply2((OpenConnectionReply2) message);
						break;
					}
					case DefaultMessageIdTypes.ID_NO_FREE_INCOMING_CONNECTIONS:
					{
						OnNoFreeIncomingConnections((NoFreeIncomingConnections) message);
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
					package.Decode(receiveBytes);
					header = package._datagramHeader;
					//Log.Debug($"> Datagram #{header.datagramSequenceNumber}, {package._hasSplit}, {package._splitPacketId}, {package._reliability}, {package._reliableMessageNumber}, {package._sequencingIndex}, {package._orderingChannel}, {package._orderingIndex}");

					{
						Acks ack = Acks.CreateObject();
						ack.acks.Add(package._datagramSequenceNumber.IntValue());
						byte[] data = ack.Encode();
						ack.PutPool();
						SendData(data, senderEndpoint);
					}

					HandleConnectedPackage(package);
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

		private void HandleConnectedPackage(ConnectedPackage package)
		{
			foreach (var message in package.Messages)
			{
				if (message is SplitPartPackage)
				{
					HandleSplitMessage(Session, (SplitPartPackage) message);

					continue;
				}

				//TraceReceive(message);

				message.Timer.Restart();
				AddToProcessing(message);
			}
		}

		private long _lastSequenceNumber = -1; // That's the first message with wrapper
		private AutoResetEvent _waitEvent = new AutoResetEvent(false);
		private AutoResetEvent _mainWaitEvent = new AutoResetEvent(false);
		private object _eventSync = new object();
		private ConcurrentPriorityQueue<int, Package> _queue = new ConcurrentPriorityQueue<int, Package>();

		private Thread _processingThread = null;

		public void AddToProcessing(Package message)
		{
			if (Session.CryptoContext == null || Session.CryptoContext.UseEncryption == false || message.Reliability != Reliability.ReliableOrdered)
			{
				HandlePackage(message);
				return;
			}

			//Log.Error("DO NOT USE THIS");
			//throw new Exception("DO NOT USE THIS");

			lock (_eventSync)
			{
				if (_lastSequenceNumber < 0) _lastSequenceNumber = 1;

				if (_queue.Count == 0 && message.OrderingIndex == _lastSequenceNumber + 1)
				{
					_lastSequenceNumber = message.OrderingIndex;
					HandlePackage(message);
					return;
				}

				if (_processingThread == null)
				{
					_processingThread = new Thread(ProcessQueueThread);
					_processingThread.IsBackground = true;
					_processingThread.Start();
				}

				_queue.Enqueue(message.OrderingIndex, message);
				WaitHandle.SignalAndWait(_waitEvent, _mainWaitEvent);
			}
		}

		private void ProcessQueueThread(object o)
		{
			ProcessQueue();
		}

		private Task ProcessQueue()
		{
			while (true)
			{
				KeyValuePair<int, Package> pair;

				if (_queue.TryPeek(out pair))
				{
					if (pair.Key == _lastSequenceNumber + 1)
					{
						if (_queue.TryDequeue(out pair))
						{
							_lastSequenceNumber = pair.Key;

							HandlePackage(pair.Value);

							if (_queue.Count == 0)
							{
								WaitHandle.SignalAndWait(_mainWaitEvent, _waitEvent, TimeSpan.FromMilliseconds(50), true);
							}
						}
					}
					else if (pair.Key <= _lastSequenceNumber)
					{
						if (Log.IsDebugEnabled) Log.Warn($"{Username} - Resent. Expected {_lastSequenceNumber + 1}, but was {pair.Key}.");
						if (_queue.TryDequeue(out pair))
						{
							pair.Value.PutPool();
						}
					}
					else
					{
						if (Log.IsDebugEnabled) Log.Warn($"{Username} - Wrong sequence. Expected {_lastSequenceNumber + 1}, but was {pair.Key}.");
						WaitHandle.SignalAndWait(_mainWaitEvent, _waitEvent, TimeSpan.FromMilliseconds(50), true);
					}
				}
				else
				{
					if (_queue.Count == 0)
					{
						WaitHandle.SignalAndWait(_mainWaitEvent, _waitEvent, TimeSpan.FromMilliseconds(50), true);
					}
				}
			}

			//Log.Warn($"Exit receive handler task for {Player.Username}");
			return Task.CompletedTask;
		}


		private void OnOpenConnectionReply2(OpenConnectionReply2 message)
		{
			Log.Warn("MTU Size: " + message.mtuSize);
			Log.Warn("Client Endpoint: " + message.clientEndpoint);

			//_serverEndpoint = message.clientEndpoint;

			_mtuSize = message.mtuSize;
			Thread.Sleep(100);
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

		private void HandleSplitMessage(PlayerNetworkSession playerSession, SplitPartPackage splitMessage)
		{
			int spId = splitMessage.SplitId;
			int spIdx = splitMessage.SplitIdx;
			int spCount = splitMessage.SplitCount;

			Int24 sequenceNumber = splitMessage.DatagramSequenceNumber;
			Reliability reliability = splitMessage.Reliability;
			Int24 reliableMessageNumber = splitMessage.ReliableMessageNumber;
			Int24 orderingIndex = splitMessage.OrderingIndex;
			byte orderingChannel = splitMessage.OrderingChannel;

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
					ConnectedPackage newPackage = ConnectedPackage.CreateObject();
					newPackage._datagramSequenceNumber = sequenceNumber;
					newPackage._reliability = reliability;
					newPackage._reliableMessageNumber = reliableMessageNumber;
					newPackage._orderingIndex = orderingIndex;
					newPackage._orderingChannel = (byte) orderingChannel;
					newPackage._hasSplit = false;

					Package fullMessage = PackageFactory.CreatePackage(buffer[0], buffer, "raknet") ?? new UnknownPackage(buffer[0], buffer);
					fullMessage.DatagramSequenceNumber = sequenceNumber;
					fullMessage.Reliability = reliability;
					fullMessage.ReliableMessageNumber = reliableMessageNumber;
					fullMessage.OrderingIndex = orderingIndex;
					fullMessage.OrderingChannel = orderingChannel;

					newPackage.Messages = new List<Package>();
					newPackage.Messages.Add(fullMessage);

					Log.Debug($"Assembled split package {newPackage._reliability} message #{newPackage._reliableMessageNumber}, Chan: #{newPackage._orderingChannel}, OrdIdx: #{newPackage._orderingIndex}");
					HandleConnectedPackage(newPackage);
					newPackage.PutPool();
				}
				catch (Exception e)
				{
					Log.Error("Error during split message parsing", e);
					if (Log.IsDebugEnabled)
						Log.Debug($"0x{buffer[0]:x2}\n{Package.HexDump(buffer)}");
				}
			}
		}

		public int PlayerStatus { get; set; }


		private void HandlePackage(Package message)
		{
			TraceReceive(message);

			if (typeof (McpeWrapper) == message.GetType())
			{
				OnWrapper((McpeWrapper) message);

				return;
			}

			if (typeof (McpeBatch) == message.GetType())
			{
				OnBatch(message);
				return;
			}

			//Log.Debug($"Handle package 0x{message.Id:X2} {message.GetType().Name}");

			if (typeof (McpeDisconnect) == message.GetType())
			{
				McpeDisconnect msg = (McpeDisconnect) message;
				Log.InfoFormat("Disconnect {1}: {0}", msg.message, Username);
				//SendDisconnectionNotification();
				StopClient();
				return;
			}
			else if (typeof (McpeServerExchange) == message.GetType())
			{
				OnMcpeServerExchange((McpeServerExchange) message);

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

			else if (typeof (McpeSetTime) == message.GetType())
			{
				OnMcpeSetTime((McpeSetTime) message);

				return;
			}

			else if (typeof (McpeStartGame) == message.GetType())
			{
				OnMcpeStartGame((McpeStartGame) message);

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

			else if (typeof (McpeAddPlayer) == message.GetType())
			{
				OnMcpeAddPlayer(message);

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
			else if (typeof (McpePlayerList) == message.GetType())
			{
				OnMcpePlayerList((McpePlayerList) message);

				return;
			}
			else if (typeof (McpeMovePlayer) == message.GetType())
			{
				OnMcpeMovePlayer((McpeMovePlayer) message);
				return;
			}
			else if (typeof (McpeSetEntityData) == message.GetType())
			{
				OnMcpeSetEntityData(message);
				return;
			}

			else if (typeof (McpeUpdateBlock) == message.GetType())
			{
				OnMcpeUpdateBlock((McpeUpdateBlock) message);
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

			else if (typeof (McpeAvailableCommands) == message.GetType())
			{
				OnMcpeAvailableCommands((McpeAvailableCommands) message);
			}

			else if (typeof (UnknownPackage) == message.GetType())
			{
				UnknownPackage packet = (UnknownPackage) message;
				if (Log.IsDebugEnabled) Log.Warn($"Unknown package 0x{message.Id:X2}\n{Package.HexDump(packet.Bytes)}");
			}


			else
			{
				if (Log.IsDebugEnabled) Log.Warn($"Unhandled package 0x{message.Id:X2} {message.GetType().Name}\n{Package.HexDump(message.Bytes)}");
			}
		}

		private void OnMcpeAvailableCommands(McpeAvailableCommands message)
		{
			//{
			//	dynamic json = JObject.Parse(message.commands);

			//	if (Log.IsDebugEnabled) Log.Debug($"Command JSON:\n{json}");
			//}
			//{
			//	dynamic json = JObject.Parse(message.unknown);

			//	if (Log.IsDebugEnabled) Log.Debug($"Command (unknown) JSON:\n{json}");
			//}
		}

		private void OnMcpeSetTime(McpeSetTime message)
		{
			Log.Debug($@"
McpeSetTime:
	started: {message.started}	
	time: {message.time}	
");
		}

		private void OnNoFreeIncomingConnections(NoFreeIncomingConnections message)
		{
			Log.Error("No free connections from server ");
			StopClient();
		}

		private void OnMcpePlayerList(McpePlayerList message)
		{
			foreach (var playerRecord in message.records)
			{
				Log.Debug($"Added player: {playerRecord.DisplayName}");
			}

			//Log.Debug($"\n{Package.HexDump(message.Bytes)}");
		}

		public void SendLogin(string username)
		{
			//McpeLogin loginPacket = new McpeLogin
			//{
			//	protocolVersion = 81,
			//	payloadLenght = 5617,
			//	payload = Convert.FromBase64String("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")
			//};
			JWT.JsonMapper = new NewtonsoftMapper();

			CngKey clientKey = CryptoUtils.GenerateClientKey();
			byte[] data = CryptoUtils.CompressJwtBytes(CryptoUtils.EncodeJwt(Username, clientKey), CryptoUtils.EncodeSkinJwt(clientKey), CompressionLevel.Fastest);

			McpeLogin loginPacket = new McpeLogin
			{
				protocolVersion = 90,
				edition = 0,
				payload = data
			};

			//var encodedLoginPacket = loginPacket.Encode();
			//McpeBatch batch = Player.CreateBatchPacket(encodedLoginPacket, 0, encodedLoginPacket.Length, CompressionLevel.Fastest, true);
			//batch.Encode();

			Session.CryptoContext = new CryptoContext()
			{
				ClientKey = clientKey,
				UseEncryption = false,
			};

			SendPackage(loginPacket);
			//SendPackage(batch);
		}

		private void OnMcpeServerExchange(McpeServerExchange message)
		{
			string serverKey = message.serverPublicKey;
			byte[] randomKeyToken = message.token;

			// Initiate encryption

			InitiateEncryption(serverKey, randomKeyToken);
		}

		private void InitiateEncryption(string serverKey, byte[] randomKeyToken)
		{
			{
				ECDiffieHellmanPublicKey publicKey = CryptoUtils.CreateEcDiffieHellmanPublicKey(serverKey);
				Log.Debug("ServerKey (b64):\n" + serverKey);
				Log.Debug($"Cert:\n{publicKey.ToXmlString()}");

				Log.Debug($"RANDOM TOKEN (raw):\n{randomKeyToken}");

				if (randomKeyToken.Length != 0)
				{
					Log.Error("Lenght of random bytes: " + randomKeyToken.Length);
				}

				// Create shared shared secret
				ECDiffieHellmanCng ecKey = new ECDiffieHellmanCng(Session.CryptoContext.ClientKey);
				ecKey.HashAlgorithm = CngAlgorithm.Sha256;
				ecKey.KeyDerivationFunction = ECDiffieHellmanKeyDerivationFunction.Hash;
				ecKey.SecretPrepend = randomKeyToken; // Server token

				byte[] secret = ecKey.DeriveKeyMaterial(publicKey);

				//Log.Debug($"SECRET KEY (b64):\n{Convert.ToBase64String(secret)}");
				Log.Debug($"SECRET KEY (raw):\n{Encoding.UTF8.GetString(secret)}");

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
						UseEncryption = true,
					};
				}

				Thread.Sleep(1250);
				McpeClientMagic magic = new McpeClientMagic();
				byte[] encodedMagic = magic.Encode();
				McpeBatch batch = BatchUtils.CreateBatchPacket(encodedMagic, 0, encodedMagic.Length, CompressionLevel.Fastest, true);
				batch.Encode();
				SendPackage(batch);
			}
		}

		public AutoResetEvent FirstEncryptedPacketWaitHandle = new AutoResetEvent(false);

		public AutoResetEvent FirstPacketWaitHandle = new AutoResetEvent(false);

		private void OnWrapper(McpeWrapper message)
		{
			FirstPacketWaitHandle.Set();

			// Get bytes
			byte[] payload = message.payload;
			if (Session.CryptoContext != null && Session.CryptoContext.UseEncryption)
			{
				FirstEncryptedPacketWaitHandle.Set();
				payload = CryptoUtils.Decrypt(payload, Session.CryptoContext);
			}

			//if (Log.IsDebugEnabled)
			//Log.Debug($"0x{payload[0]:x2}\n{Package.HexDump(payload)}");

			try
			{
				Package newMessage = PackageFactory.CreatePackage(payload[0], payload, "mcpe") ?? new UnknownPackage(payload[0], payload);
				//TraceReceive(newMessage);

				if (_processingThread == null)
				{
					HandlePackage(newMessage);
				}
				else
				{
					_threadPool.QueueUserWorkItem(() => HandlePackage(newMessage));
				}
			}
			catch (Exception e)
			{
				Log.Error("Wrapper", e);
			}

			//Task.Run(() => { HandlePackage(newMessage); });
		}

		private void OnMcpeAdventureSettings(McpeAdventureSettings message)
		{
			Log.Debug($@"
Adventure settings 
	flags: 0x{message.flags:X2} - {Convert.ToString(message.flags, 2)}
	flags: 0x{message.userPermission:X2} - {Convert.ToString(message.userPermission, 2)}
");
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

		public AutoResetEvent PlayerStatusChangedWaitHandle = new AutoResetEvent(false);

		private void OnMcpePlayerStatus(McpePlayerStatus message)
		{
			if (Log.IsDebugEnabled) Log.Debug($"Player status={message.status}");
			PlayerStatus = message.status;

			if (PlayerStatus == 3 && IsEmulator)
			{
				PlayerStatusChangedWaitHandle.Set();

				SendMcpeMovePlayer();
			}
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
			McpeContainerSetContent msg = (McpeContainerSetContent) message;
			Log.Debug($"Set container content on Window ID: 0x{msg.windowId:x2}, Count: {msg.slotData.Count}");

			if (IsEmulator) return;

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

		private void OnMcpeUpdateBlock(McpeUpdateBlock message)
		{
			Log.Debug($"Blocks ID={message.blockId}, Metadata={message.blockMetaAndPriority}");
		}

		private void OnMcpeMovePlayer(McpeMovePlayer message)
		{
			Log.DebugFormat("McpeMovePlayer Entity ID: {0}", message.entityId);

			if (message.entityId != _entityId) return;

			CurrentLocation = new PlayerLocation(message.x, message.y, message.z);
			SendMcpeMovePlayer();
		}

		private static void OnMcpeSetEntityData(Package message)
		{
			McpeSetEntityData msg = (McpeSetEntityData) message;
			Log.DebugFormat("McpeSetEntityData Entity ID: {0}, Metadata: {1}", msg.entityId, msg.metadata);
			//Log.Debug($"Package 0x{message.Id:X2}\n{Package.HexDump(message.Bytes)}");
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
			Log.DebugFormat("Metadata: {0}", msg.metadata);
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
			if (Log.IsDebugEnabled)
			{
				foreach (var attribute in msg.attributes)
				{
					Log.Debug($"Entity attribute {attribute}");
				}
			}
			Log.DebugFormat("Links count: {0}", msg.links);
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
			Log.DebugFormat("X: {0}", message.coordinates.X);
			Log.DebugFormat("Y: {0}", message.coordinates.Y);
			Log.DebugFormat("Z: {0}", message.coordinates.Z);
			Log.DebugFormat("NBT: {0}", message.namedtag.NbtFile);
		}

		private static void OnMcpeTileEvent(Package message)
		{
			McpeBlockEvent msg = (McpeBlockEvent) message;
			Log.DebugFormat("Coord: {0}", msg.coordinates);
			Log.DebugFormat("Case 1: {0}", msg.case1);
			Log.DebugFormat("Case 2: {0}", msg.case2);
		}

		private void OnMcpeStartGame(McpeStartGame message)
		{
			_entityId = message.entityId;
			_spawn = new Vector3(message.x, message.y, message.z);

			Log.Debug($@"
StartGame:
	entityId: {message.entityId}	
	runtimeEntityId: {message.runtimeEntityId}	
	seed: {message.seed}	
	dimension: {message.dimension}	
	generator: {message.generator}	
	gamemode: {message.gamemode}	
	difficulty: {message.difficulty}	
	isLoadedInCreative: {message.isLoadedInCreative}	
	dayCycleStopTime: {message.dayCycleStopTime}	
	eduMode: {message.eduMode}	
	rainLevel: {message.rainLevel}	
	lightnigLevel: {message.lightnigLevel}	
	enableCommands: {message.enableCommands}	
	worldName: {message.worldName}	
");

			Level.LevelName = "Default";
			Level.Version = 19133;
			Level.GameType = message.gamemode;

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
			_spawn = new Vector3(msg.coordinates.X, msg.coordinates.Y, msg.coordinates.Z);
			Level.SpawnX = (int) _spawn.X;
			Level.SpawnY = (int) _spawn.Y;
			Level.SpawnZ = (int) _spawn.Z;
			Log.Info($"Spawn position: {msg.coordinates}");
			Log.Debug($"\n{Package.HexDump(message.Bytes)}");
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
				Log.Debug($"Chunk X={msg.chunkX}, Z={msg.chunkZ}, size={msg.chunkData.Length}, Count={++_numberOfChunks}");

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
				byte[] internalBuffer = null;
				do
				{
					try
					{
						var len = VarInt.ReadUInt32(destination);
						internalBuffer = new byte[len];
						destination.Read(internalBuffer, 0, (int) len);

						if (internalBuffer[0] == 0x8e) throw new Exception("Wrong code, didn't expect a 0x8E in a batched packet");

						var package = PackageFactory.CreatePackage(internalBuffer[0], internalBuffer, "mcpe") ?? new UnknownPackage(internalBuffer[0], internalBuffer);
						messages.Add(package);

						//if (Log.IsDebugEnabled) Log.Debug($"Batch: {package.GetType().Name} 0x{package.Id:x2}");
						//if (!(package is McpeFullChunkData)) Log.Debug($"Batch: {package.GetType().Name} 0x{package.Id:x2} \n{Package.HexDump(internalBuffer)}");
					}
					catch (Exception e)
					{
						if (internalBuffer != null)
							Log.Error($"Batch error while reading:\n{Package.HexDump(internalBuffer)}");
						Log.Error("Batch processing", e);
						//throw;
					}
				} while (destination.Position < destination.Length);
			}

			//Log.Error($"Batch had {messages.Count} packets.");
			if (messages.Count == 0) Log.Error($"Batch had 0 packets.");

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
			Thread.Sleep(50);

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
				//|| message is McpeAddEntity
				//|| message is McpeCraftingData
				//|| message is McpeContainerSetContent
				//|| message is McpeMobArmorEquipment
				//|| message is McpeClientboundMapItemData
				//|| message is McpeMovePlayer
				//|| message is McpeSetEntityMotion
				//|| message is McpeBatch
				//|| message is McpeFullChunkData
				//|| message is McpeWrapper
			    || message is ConnectedPing) return;

			//var stringWriter = new StringWriter();
			//ObjectDumper.Write(message, 1, stringWriter);

			Log.DebugFormat("> Receive: {0}: {1} (0x{0:x2})", message.Id, message.GetType().Name);
			//Log.Debug($"Package 0x{message.Id:X2}\n{Package.HexDump(message.Bytes)}");

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
				pingId = Stopwatch.GetTimestamp() /*incoming.pingId*/,
			};

			var data = packet.Encode();
			TraceSend(packet);
			//SendData(data);
			if (_serverEndpoint != null)
			{
				SendData(data, _serverEndpoint);
			}
			else
			{
				SendData(data, new IPEndPoint(IPAddress.Broadcast, 19132));
			}
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
				sendpongtime = sendpingtime + 200
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

			// 1446 - 1464
			// 1087 1447
			byte[] data2 = new byte[_mtuSize - data.Length + 18];
			Buffer.BlockCopy(data, 0, data2, 0, data.Length);

			SendData(data2);
		}

		public void SendOpenConnectionRequest2()
		{
			//_clientGuid = new Random().Next() + new Random().Next();
			var packet = new OpenConnectionRequest2()
			{
				remoteBindingAddress = _serverEndpoint,
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
				timestamp = DateTime.UtcNow.Ticks /*/TimeSpan.TicksPerMillisecond*/,
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