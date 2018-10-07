#region LICENSE

// The contents of this file are subject to the Common Public Attribution
// License Version 1.0. (the "License"); you may not use this file except in
// compliance with the License. You may obtain a copy of the License at
// https://github.com/NiclasOlofsson/MiNET/blob/master/LICENSE. 
// The License is based on the Mozilla Public License Version 1.1, but Sections 14 
// and 15 have been added to cover use of software over a computer network and 
// provide for limited attribution for the Original Developer. In addition, Exhibit A has 
// been modified to be consistent with Exhibit B.
// 
// Software distributed under the License is distributed on an "AS IS" basis,
// WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License for
// the specific language governing rights and limitations under the License.
// 
// The Original Code is MiNET.
// 
// The Original Developer is the Initial Developer.  The Initial Developer of
// the Original Code is Niclas Olofsson.
// 
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2018 Niclas Olofsson. 
// All Rights Reserved.

#endregion

using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Numerics;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using fNbt;
using Jose;
using log4net;
using log4net.Config;
using MiNET.Blocks;
using MiNET.Crafting;
using MiNET.Entities;
using MiNET.Items;
using MiNET.Net;
using MiNET.Utils;
using MiNET.Worlds;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Agreement;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

[assembly: XmlConfigurator(Watch = true)]
// This will cause log4net to look for a configuration file
// called TestApp.exe.config in the application base
// directory (i.e. the directory containing TestApp.exe)
// The config file will be watched for changes.

namespace MiNET.Client
{
	public class MiNetClient
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(MiNetClient));

		private IPEndPoint _clientEndpoint;
		private IPEndPoint _serverEndpoint;
		private readonly DedicatedThreadPool _threadPool;
		private short _mtuSize = 1192;
		private int _reliableMessageNumber = -1;
		public Vector3 _spawn;
		public long EntityId { get; set; }
		public long NetworkEntityId { get; set; }
		public PlayerNetworkSession Session { get; set; }
		private Thread _mainProcessingThread;
		public int ChunkRadius { get; set; } = 5;

		public LevelInfo Level { get; } = new LevelInfo();

		//private long _clientGuid = new Random().Next();
		private long _clientGuid = 1111111 + new Random().Next();
		//private long _clientGuid = 1111111 + DateTimeOffset.Now.ToUnixTimeMilliseconds();

		public bool HaveServer = false;
		public PlayerLocation CurrentLocation { get; set; }

		public bool IsEmulator { get; set; }

		public UdpClient UdpClient { get; private set; }

		public string Username { get; set; }
		public int ClientId { get; set; }


		public IMcpeClientMessageHandler MessageHandler { get; set; }

		public MiNetClient(IPEndPoint endpoint, string username, DedicatedThreadPool threadPool)
		{
			Random random = new Random();
			_clientGuid = 1111111 + random.Next() + random.Next();

			Username = username;
			ClientId = new Random().Next();
			_serverEndpoint = endpoint;
			_threadPool = threadPool;
			if (_serverEndpoint != null) Log.Warn("Connecting to: " + _serverEndpoint);
			_clientEndpoint = new IPEndPoint(IPAddress.Any, 0);
			MessageHandler = new BedrockTraceHandler(this);
		}

		private static void Main(string[] args)
		{
			var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
			XmlConfigurator.Configure(logRepository, new FileInfo("log4net.xml"));

			Console.WriteLine("Starting client...");

			int threads;
			int iothreads;
			ThreadPool.GetMinThreads(out threads, out iothreads);

			var client = new MiNetClient(new IPEndPoint(Dns.GetHostEntry("yodamine.com").AddressList[0], 19132), "TheGrey", new DedicatedThreadPool(new DedicatedThreadPoolSettings(Environment.ProcessorCount)));
			//var client = new MiNetClient(new IPEndPoint(IPAddress.Loopback, 19132), "TheGrey", new DedicatedThreadPool(new DedicatedThreadPoolSettings(Environment.ProcessorCount)));

			client.StartClient();
			Log.Warn("Client listening for connecting on: " + client._clientEndpoint);
			Console.WriteLine("Server started.");

			//client.SendOpenConnectionRequest1();

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


			//{
			//	McpeAnimate message = new McpeAnimate();
			//	message.actionId = 2;
			//	message.entityId = client._entityId;
			//	client.SendPackage(message);
			//}
			//{
			//	McpeUseItem message = new McpeUseItem();
			//	message.blockcoordinates = new BlockCoordinates(53, 4, 17);
			//	message.face = 4;
			//	message.facecoordinates = new Vector3(0.1f, 0.1f, 0.1f);
			//	message.playerposition = client.CurrentLocation.ToVector3();
			//	message.item = new ItemBlock(new Stone(), 0) {Count = 1};
			//	client.SendPackage(message);
			//}

			Action<Task, PlayerLocation> doMoveTo = BotHelpers.DoMoveTo(client);
			Action<Task, string> doSendCommand = BotHelpers.DoSendCommand(client);

			//.ContinueWith(t => doMovePlayer(t, client.CurrentLocation + new Vector3(0, 1.62f, 0)))
			//.ContinueWith(t => doMoveTo(t, client.CurrentLocation + new Vector3(10, 1.62f, 10)))

			Task.Run(BotHelpers.DoWaitForSpawn(client))
				//.ContinueWith(t => BotHelpers.DoMobEquipment(client)(t, new ItemBlock(new Cobblestone(), 0) {Count = 64}, 0))
				//.ContinueWith(t => BotHelpers.DoMoveTo(client)(t, new PlayerLocation(client.CurrentLocation.ToVector3() - new Vector3(0, 1, 0), 180, 180, 180)))
				//.ContinueWith(t => doMoveTo(t, new PlayerLocation(40, 5.62f, -20, 180, 180, 180)))
				.ContinueWith(t => doMoveTo(t, new PlayerLocation(24, 5.62, -13, 180 + 45, 180 + 45, 180)))
				//.ContinueWith(t => doMoveTo(t, new PlayerLocation(0, 5.62, 0, 180 + 45, 180 + 45, 180)))
				//.ContinueWith(t => doMoveTo(t, new PlayerLocation(22, 5.62, 40, 180 + 45, 180 + 45, 180)))
				//.ContinueWith(t => doMoveTo(t, new PlayerLocation(50, 5.62f, 17, 180, 180, 180)))
				.ContinueWith(t => doSendCommand(t, "/me says \"Hi guys! It is I!!\""))
				//.ContinueWith(t => Task.Delay(500).Wait())
				//.ContinueWith(t => doSendCommand(t, "/summon sheep"))
				//.ContinueWith(t => Task.Delay(500).Wait())
				//.ContinueWith(t => doSendCommand(t, "/kill @e[type=sheep]"))
				.ContinueWith(t => Task.Delay(5000).Wait())
				//.ContinueWith(t =>
				//{
				//	Random rnd = new Random();
				//	while (true)
				//	{
				//		doMoveTo(t, new PlayerLocation(rnd.Next(10, 40), 5.62f, rnd.Next(-50, -10), 180, 180, 180));
				//		//doMoveTo(t, new PlayerLocation(50, 5.62f, 17, 180, 180, 180));
				//		doMoveTo(t, new PlayerLocation(rnd.Next(40, 50), 5.62f, rnd.Next(0, 20), 180, 180, 180));
				//	}
				//})
				;

			//string fileName = Path.GetTempPath() + "MobSpawns_" + Guid.NewGuid() + ".txt";
			//FileStream file = File.OpenWrite(fileName);
			//Log.Info($"Writing mob spawns to file:\n{fileName}");
			//_mobWriter = new IndentedTextWriter(new StreamWriter(file));
			//Task.Run(BotHelpers.DoWaitForSpawn(client))
			//	.ContinueWith(task =>
			//	{
			//		foreach (EntityType entityType in Enum.GetValues(typeof(EntityType)))
			//		{
			//			if (entityType == EntityType.Wither) continue;
			//			if (entityType == EntityType.Dragon) continue;
			//			if (entityType == EntityType.Slime) continue;

			//			string entityName = entityType.ToString();
			//			entityName = Regex.Replace(entityName, "([A-Z])", "_$1").TrimStart('_').ToLower();
			//			{
			//				string command = $"/summon {entityName}";
			//				McpeCommandRequest request = new McpeCommandRequest();
			//				request.command = command;
			//				request.unknownUuid = new UUID(Guid.NewGuid().ToString());
			//				client.SendPackage(request);
			//			}

			//			Task.Delay(500).Wait();

			//			{
			//				McpeCommandRequest request = new McpeCommandRequest();
			//				request.command = $"/kill @e[type={entityName}]";
			//				request.unknownUuid = new UUID(Guid.NewGuid().ToString());
			//				client.SendPackage(request);
			//			}
			//		}

			//		{
			//			McpeCommandRequest request = new McpeCommandRequest();
			//			request.command = $"/kill @e[type=!player]";
			//			request.unknownUuid = new UUID(Guid.NewGuid().ToString());
			//			client.SendPackage(request);
			//		}

			//	});

			Console.WriteLine("<Enter> to exit!");
			Console.ReadLine();
			client.SendDisconnectionNotification();
			Thread.Sleep(2000);
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

				if (Environment.OSVersion.Platform != PlatformID.Unix && Environment.OSVersion.Platform != PlatformID.MacOSX)
				{
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
				}

				//Task.Run(ProcessQueue);

				Session = new PlayerNetworkSession(null, null, _clientEndpoint, _mtuSize);

				//UdpClient.BeginReceive(ReceiveCallback, UdpClient);
				_mainProcessingThread = new Thread(ProcessDatagrams) {IsBackground = true};
				_mainProcessingThread.Start(UdpClient);

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
				Session?.Close();
				//_mainProcessingThread?.Abort();
				_mainProcessingThread = null;

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

				Packet message = PacketFactory.Create(msgId, receiveBytes, "raknet");

				if (message == null) return;

				TraceReceive(message);

				switch (msgIdType)
				{
					case DefaultMessageIdTypes.ID_UNCONNECTED_PONG:
					{
						UnconnectedPong incoming = (UnconnectedPong) message;
						Log.Warn($"MOTD: {incoming.serverName}");
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
						if (incoming.mtuSize != _mtuSize) Log.Warn("Error, mtu differ from what we sent:" + incoming.mtuSize);
						Log.Warn($"Server with ID {incoming.serverGuid} security={incoming.serverHasSecurity}");
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

					ConnectedPacket packet = ConnectedPacket.CreateObject();
					packet.Decode(receiveBytes);
					header = packet._datagramHeader;
					//Log.Debug($"> Datagram #{header.datagramSequenceNumber}, {package._hasSplit}, {package._splitPacketId}, {package._reliability}, {package._reliableMessageNumber}, {package._sequencingIndex}, {package._orderingChannel}, {package._orderingIndex}");

					{
						Acks ack = Acks.CreateObject();
						ack.acks.Add(packet._datagramSequenceNumber.IntValue());
						byte[] data = ack.Encode();
						ack.PutPool();
						SendData(data, senderEndpoint);
					}

					HandleConnectedPacket(packet);
					packet.PutPool();
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

		private void HandleConnectedPacket(ConnectedPacket packet)
		{
			foreach (var message in packet.Messages)
			{
				if (message is SplitPartPacket)
				{
					HandleSplitMessage(Session, (SplitPartPacket) message);

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
		private ConcurrentPriorityQueue<int, Packet> _queue = new ConcurrentPriorityQueue<int, Packet>();

		private Thread _processingThread = null;

		public void AddToProcessing(Packet message)
		{
			if (message.Reliability != Reliability.ReliableOrdered)
			{
				HandlePacket(message);
				return;
			}
			//Log.Error("DO NOT USE THIS");
			//throw new Exception("DO NOT USE THIS");

			lock (_eventSync)
			{
				if (_queue.Count == 0 && message.OrderingIndex == _lastSequenceNumber + 1)
				{
					HandlePacket(message);
					_lastSequenceNumber = message.OrderingIndex;
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
				KeyValuePair<int, Packet> pair;

				if (_queue.TryPeek(out pair))
				{
					if (pair.Key == _lastSequenceNumber + 1)
					{
						if (_queue.TryDequeue(out pair))
						{
							HandlePacket(pair.Value);

							_lastSequenceNumber = pair.Key;

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

		private void HandleSplitMessage(PlayerNetworkSession playerSession, SplitPartPacket splitMessage)
		{
			int spId = splitMessage.SplitId;
			int spIdx = splitMessage.SplitIdx;
			int spCount = splitMessage.SplitCount;

			Int24 sequenceNumber = splitMessage.DatagramSequenceNumber;
			Reliability reliability = splitMessage.Reliability;
			Int24 reliableMessageNumber = splitMessage.ReliableMessageNumber;
			Int24 orderingIndex = splitMessage.OrderingIndex;
			byte orderingChannel = splitMessage.OrderingChannel;

			SplitPartPacket[] spPackets;
			bool haveEmpty = false;

			// Need sync for this part since they come very fast, and very close in time. 
			// If no synk, will often detect complete message two times (or more).
			lock (playerSession.Splits)
			{
				if (!playerSession.Splits.ContainsKey(spId))
				{
					playerSession.Splits.TryAdd(spId, new SplitPartPacket[spCount]);
				}

				spPackets = playerSession.Splits[spId];
				if (spPackets[spIdx] != null)
				{
					Log.Debug("Already had splitpart (resent). Ignore this part.");
					return;
				}
				spPackets[spIdx] = splitMessage;

				for (int i = 0; i < spPackets.Length; i++)
				{
					haveEmpty = haveEmpty || spPackets[i] == null;
				}
			}

			if (!haveEmpty)
			{
				Log.DebugFormat("Got all {0} split packets for split ID: {1}", spCount, spId);

				SplitPartPacket[] waste;
				playerSession.Splits.TryRemove(spId, out waste);

				using (MemoryStream stream = MiNetServer.MemoryStreamManager.GetStream())
				{
					for (int i = 0; i < spPackets.Length; i++)
					{
						SplitPartPacket splitPartPacket = spPackets[i];
						byte[] buf = splitPartPacket.Message;
						if (buf == null)
						{
							Log.Error("Expected bytes in splitpart, but got none");
							continue;
						}

						stream.Write(buf, 0, buf.Length);
						splitPartPacket.PutPool();
					}

					byte[] buffer = stream.ToArray();
					try
					{
						ConnectedPacket newPacket = ConnectedPacket.CreateObject();
						newPacket._datagramSequenceNumber = sequenceNumber;
						newPacket._reliability = reliability;
						newPacket._reliableMessageNumber = reliableMessageNumber;
						newPacket._orderingIndex = orderingIndex;
						newPacket._orderingChannel = (byte) orderingChannel;
						newPacket._hasSplit = false;

						Packet fullMessage = PacketFactory.Create(buffer[0], buffer, "raknet") ?? new UnknownPacket(buffer[0], buffer);
						fullMessage.DatagramSequenceNumber = sequenceNumber;
						fullMessage.Reliability = reliability;
						fullMessage.ReliableMessageNumber = reliableMessageNumber;
						fullMessage.OrderingIndex = orderingIndex;
						fullMessage.OrderingChannel = orderingChannel;

						newPacket.Messages = new List<Packet>();
						newPacket.Messages.Add(fullMessage);

						Log.Debug($"Assembled split packet {newPacket._reliability} message #{newPacket._reliableMessageNumber}, Chan: #{newPacket._orderingChannel}, OrdIdx: #{newPacket._orderingIndex}");
						HandleConnectedPacket(newPacket);
						newPacket.PutPool();
					}
					catch (Exception e)
					{
						Log.Error("Error during split message parsing", e);
						if (Log.IsDebugEnabled)
							Log.Debug($"0x{buffer[0]:x2}\n{Packet.HexDump(buffer)}");
						playerSession.Disconnect("Bad packet received from client.", false);
					}
				}
			}
		}

		public int PlayerStatus { get; set; }


		private void HandlePacket(Packet message)
		{
			TraceReceive(message);

			if (typeof(McpeWrapper) == message.GetType())
			{
				HandleBatch((McpeWrapper) message);
				return;
			}

			//Log.Debug($"Handle packet 0x{message.Id:X2} {message.GetType().Name}");

			if (typeof(McpeDisconnect) == message.GetType())
			{
				MessageHandler.HandleMcpeDisconnect((McpeDisconnect) message);
			}
			else if (typeof(McpeServerToClientHandshake) == message.GetType())
			{
				MessageHandler.HandleMcpeServerToClientHandshake((McpeServerToClientHandshake) message);
			}
			else if (typeof(ConnectedPing) == message.GetType())
			{
				ConnectedPing msg = (ConnectedPing) message;
				SendConnectedPong(msg.sendpingtime);
			}
			else if (typeof(McpeResourcePacksInfo) == message.GetType())
			{
				MessageHandler.HandleMcpeResourcePacksInfo((McpeResourcePacksInfo) message);
			}
			else if (typeof(McpeResourcePackStack) == message.GetType())
			{
				MessageHandler.HandleMcpeResourcePackStack((McpeResourcePackStack) message);
			}

			else if (typeof(McpeResourcePackDataInfo) == message.GetType())
			{
				MessageHandler.HandleMcpeResourcePackDataInfo((McpeResourcePackDataInfo) message);
			}
			else if (typeof(McpeResourcePackChunkData) == message.GetType())
			{
				MessageHandler.HandleMcpeResourcePackChunkData((McpeResourcePackChunkData) message);
			}
			else if (typeof(McpeFullChunkData) == message.GetType())
			{
				MessageHandler.HandleMcpeFullChunkData((McpeFullChunkData) message);
			}
			else if (typeof(ConnectionRequestAccepted) == message.GetType())
			{
				OnConnectionRequestAccepted();
			}
			else if (typeof(McpeSetSpawnPosition) == message.GetType())
			{
				MessageHandler.HandleMcpeSetSpawnPosition((McpeSetSpawnPosition) message);
			}
			else if (typeof(McpeGameRulesChanged) == message.GetType())
			{
				MessageHandler.HandleMcpeGameRulesChanged((McpeGameRulesChanged) message);
			}
			else if (typeof(McpeSetTime) == message.GetType())
			{
				MessageHandler.HandleMcpeSetTime((McpeSetTime) message);
			}
			else if (typeof(McpeStartGame) == message.GetType())
			{
				MessageHandler.HandleMcpeStartGame((McpeStartGame) message);
			}
			else if (typeof(McpeRespawn) == message.GetType())
			{
				MessageHandler.HandleMcpeRespawn((McpeRespawn) message);
			}
			else if (typeof(McpeBlockEvent) == message.GetType())
			{
				MessageHandler.HandleMcpeBlockEvent((McpeBlockEvent) message);
			}
			else if (typeof(McpeBlockEntityData) == message.GetType())
			{
				MessageHandler.HandleMcpeBlockEntityData((McpeBlockEntityData) message);
			}
			else if (typeof(McpeAddPlayer) == message.GetType())
			{
				MessageHandler.HandleMcpeAddPlayer((McpeAddPlayer) message);
			}
			else if (typeof(McpeAddEntity) == message.GetType())
			{
				MessageHandler.HandleMcpeAddEntity((McpeAddEntity) message);
			}
			else if (typeof(McpeRemoveEntity) == message.GetType())
			{
				MessageHandler.HandleMcpeRemoveEntity((McpeRemoveEntity) message);
			}
			else if (typeof(McpeAddItemEntity) == message.GetType())
			{
				MessageHandler.HandleMcpeAddItemEntity((McpeAddItemEntity) message);
			}
			else if (typeof(McpePlayerList) == message.GetType())
			{
				MessageHandler.HandleMcpePlayerList((McpePlayerList) message);
			}
			else if (typeof(McpeMovePlayer) == message.GetType())
			{
				MessageHandler.HandleMcpeMovePlayer((McpeMovePlayer) message);
			}

			else if (typeof(McpeMobArmorEquipment) == message.GetType())
			{
				MessageHandler.HandleMcpeMobArmorEquipment((McpeMobArmorEquipment) message);
			}
			else if (typeof(McpeSetEntityData) == message.GetType())
			{
				MessageHandler.HandleMcpeSetEntityData((McpeSetEntityData) message);
			}
			else if (typeof(McpeLevelEvent) == message.GetType())
			{
				MessageHandler.HandleMcpeLevelEvent((McpeLevelEvent) message);
			}
			else if (typeof(McpeMobEffect) == message.GetType())
			{
				MessageHandler.HandleMcpeMobEffect((McpeMobEffect) message);
			}
			else if (typeof(McpeSpawnExperienceOrb) == message.GetType())
			{
				MessageHandler.HandleMcpeSpawnExperienceOrb((McpeSpawnExperienceOrb) message);
			}
			else if (typeof(McpeMobEquipment) == message.GetType())
			{
				MessageHandler.HandleMcpeMobEquipment((McpeMobEquipment) message);
			}
			else if (typeof(McpeContainerOpen) == message.GetType())
			{
				MessageHandler.HandleMcpeContainerOpen((McpeContainerOpen) message);
			}
			else if (typeof(McpeContainerSetData) == message.GetType())
			{
				MessageHandler.HandleMcpeContainerSetData((McpeContainerSetData) message);
			}
			else if (typeof(McpeInventoryContent) == message.GetType())
			{
				MessageHandler.HandleMcpeInventoryContent((McpeInventoryContent) message);
			}
			else if (typeof(McpeInventoryTransaction) == message.GetType())
			{
				MessageHandler.HandleMcpeInventoryTransaction((McpeInventoryTransaction) message);
			}
			else if (typeof(McpePlayerHotbar) == message.GetType())
			{
				MessageHandler.HandleMcpePlayerHotbar((McpePlayerHotbar) message);
			}
			else if (typeof(McpeCraftingData) == message.GetType())
			{
				MessageHandler.HandleMcpeCraftingData((McpeCraftingData) message);
			}
			else if (typeof(McpeAdventureSettings) == message.GetType())
			{
				MessageHandler.HandleMcpeAdventureSettings((McpeAdventureSettings) message);
			}
			else if (typeof(McpeMoveEntity) == message.GetType())
			{
				MessageHandler.HandleMcpeMoveEntity((McpeMoveEntity) message);
			}
			else if (typeof(McpeMoveEntityDelta) == message.GetType())
			{
				MessageHandler.HandleMcpeMoveEntityDelta((McpeMoveEntityDelta) message);
			}
			else if (typeof(McpeSetEntityMotion) == message.GetType())
			{
				MessageHandler.HandleMcpeSetEntityMotion((McpeSetEntityMotion) message);
			}
			else if (typeof(McpeEntityEvent) == message.GetType())
			{
				MessageHandler.HandleMcpeEntityEvent((McpeEntityEvent) message);
			}
			else if (typeof(McpeUpdateAttributes) == message.GetType())
			{
				MessageHandler.HandleMcpeUpdateAttributes((McpeUpdateAttributes) message);
			}
			else if (typeof(McpeText) == message.GetType())
			{
				MessageHandler.HandleMcpeText((McpeText) message);
			}
			else if (typeof(McpePlayStatus) == message.GetType())
			{
				MessageHandler.HandleMcpePlayStatus((McpePlayStatus) message);
			}
			else if (typeof(McpeClientboundMapItemData) == message.GetType())
			{
				MessageHandler.HandleMcpeClientboundMapItemData((McpeClientboundMapItemData) message);
			}
			else if (typeof(McpeHurtArmor) == message.GetType())
			{
				MessageHandler.HandleMcpeHurtArmor((McpeHurtArmor) message);
			}
			else if (typeof(McpeAnimate) == message.GetType())
			{
				MessageHandler.HandleMcpeAnimate((McpeAnimate) message);
			}
			else if (typeof(McpeMapInfoRequest) == message.GetType())
			{
				MessageHandler.HandleMcpeMapInfoRequest((McpeMapInfoRequest) message);
			}
			else if (typeof(McpeClientboundMapItemData) == message.GetType())
			{
				MessageHandler.HandleMcpeClientboundMapItemData((McpeClientboundMapItemData) message);
			}
			else if (typeof(McpeInteract) == message.GetType())
			{
				MessageHandler.HandleMcpeInteract((McpeInteract) message);
			}
			else if (typeof(McpeAvailableCommands) == message.GetType())
			{
				MessageHandler.HandleMcpeAvailableCommands((McpeAvailableCommands) message);
			}
			else if (typeof(McpeChangeDimension) == message.GetType())
			{
				MessageHandler.HandleMcpeChangeDimension((McpeChangeDimension) message);
			}
			else if (typeof(McpeUpdateEquipment) == message.GetType())
			{
				MessageHandler.HandleMcpeUpdateEquipment((McpeUpdateEquipment) message);
			}
			else if (typeof(UnknownPacket) == message.GetType())
			{
				UnknownPacket packet = (UnknownPacket) message;
				if (Log.IsDebugEnabled) Log.Warn($"Unknown packet 0x{message.Id:X2}\n{Packet.HexDump(packet.Message)}");
			}
			else
			{
				if (Log.IsDebugEnabled) Log.Warn($"Unhandled packet 0x{message.Id:X2} {message.GetType().Name}\n{Packet.HexDump(message.Bytes)}");
			}
		}

		private void OnNoFreeIncomingConnections(NoFreeIncomingConnections message)
		{
			Log.Error("No free connections from server ");
			StopClient();
		}

		public void SendLogin(string username)
		{
			JWT.JsonMapper = new NewtonsoftMapper();

			var clientKey = CryptoUtils.GenerateClientKey();
			byte[] data = CryptoUtils.CompressJwtBytes(CryptoUtils.EncodeJwt(username, clientKey, IsEmulator), CryptoUtils.EncodeSkinJwt(clientKey, username), CompressionLevel.Fastest);

			McpeLogin loginPacket = new McpeLogin
			{
				protocolVersion = Config.GetProperty("EnableEdu", false) ? 111 : McpeProtocolInfo.ProtocolVersion,
				payload = data
			};

			Session.CryptoContext = new CryptoContext()
			{
				ClientKey = clientKey,
				UseEncryption = false,
			};

			SendPacket(loginPacket);
		}

		public void InitiateEncryption(byte[] serverKey, byte[] randomKeyToken)
		{
			try
			{
				ECPublicKeyParameters remotePublicKey = (ECPublicKeyParameters)
					PublicKeyFactory.CreateKey(serverKey);

				//ECDiffieHellmanPublicKey publicKey = CryptoUtils.FromDerEncoded(serverKey);
				//Log.Debug("ServerKey (b64):\n" + serverKey);
				//Log.Debug($"Cert:\n{publicKey.ToXmlString()}");

				Log.Debug($"RANDOM TOKEN (raw):\n\n{Encoding.UTF8.GetString(randomKeyToken)}");

				//if (randomKeyToken.Length != 0)
				//{
				//	Log.Error("Lenght of random bytes: " + randomKeyToken.Length);
				//}

				ECDHBasicAgreement agreement = new ECDHBasicAgreement();
				agreement.Init(Session.CryptoContext.ClientKey.Private);
				byte[] secret;
				using (var sha = SHA256.Create())
				{
					secret = sha.ComputeHash(randomKeyToken.Concat(agreement.CalculateAgreement(remotePublicKey).ToByteArrayUnsigned()).ToArray());
				}

				Log.Debug($"SECRET KEY (raw):\n{Encoding.UTF8.GetString(secret)}");

				// Create a decrytor to perform the stream transform.
				IBufferedCipher decryptor = CipherUtilities.GetCipher("AES/CFB8/NoPadding");
				decryptor.Init(false, new ParametersWithIV(new KeyParameter(secret), secret.Take(16).ToArray()));

				IBufferedCipher encryptor = CipherUtilities.GetCipher("AES/CFB8/NoPadding");
				encryptor.Init(true, new ParametersWithIV(new KeyParameter(secret), secret.Take(16).ToArray()));

				Session.CryptoContext = new CryptoContext
				{
					Decryptor = decryptor,
					Encryptor = encryptor,
					UseEncryption = true,
					Key = secret
				};

				Thread.Sleep(1250);
				McpeClientToServerHandshake magic = new McpeClientToServerHandshake();
				SendPacket(magic);
			}
			catch (Exception e)
			{
				Log.Error("Initiate encryption", e);
			}
		}

		public AutoResetEvent FirstEncryptedPacketWaitHandle = new AutoResetEvent(false);

		public AutoResetEvent FirstPacketWaitHandle = new AutoResetEvent(false);

		public CommandPermission UserPermission { get; set; }

		public AutoResetEvent PlayerStatusChangedWaitHandle = new AutoResetEvent(false);

		public bool HasSpawned { get; set; }

		public ShapedRecipe _recipeToSend = null;

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
					for (uint i = 0; i < recipe.Input.Length; i++)
					{
						slotData.Add(recipe.Input[i]);

						McpeInventorySlot sendSlot = McpeInventorySlot.CreateObject();
						sendSlot.inventoryId = 0;
						sendSlot.slot = i;
						sendSlot.item = recipe.Input[i];
						SendPacket(sendSlot);

						//McpeContainerSetSlot setSlot = McpeContainerSetSlot.CreateObject();
						//setSlot.item = recipe.Input[i];
						//setSlot.windowId = 0;
						//setSlot.slot = (short) (i);
						//SendPackage(setSlot);
						//Log.Error("Set set slot");
					}
					crafting.input = slotData;

					{
						McpeMobEquipment eq = McpeMobEquipment.CreateObject();
						eq.runtimeEntityId = EntityId;
						eq.slot = 9;
						eq.selectedSlot = 0;
						eq.item = recipe.Input[0];
						SendPacket(eq);
						Log.Error("Set eq slot");
					}
				}
				{
					ItemStacks slotData = new ItemStacks {recipe.Result};
					crafting.result = slotData;
				}

				SendPacket(crafting);
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
					//McpeContainerSetSlot setSlot = McpeContainerSetSlot.CreateObject();
					//setSlot.item = new ItemBlock(new Block(17), 0) {Count = 1};
					//setSlot.windowId = 0;
					//setSlot.slot = 0;
					//SendPackage(setSlot);
				}
				{
					McpeMobEquipment eq = McpeMobEquipment.CreateObject();
					eq.runtimeEntityId = EntityId;
					eq.slot = 9;
					eq.selectedSlot = 0;
					eq.item = new ItemBlock(new Block(17), 0) {Count = 1};
					SendPacket(eq);
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

				SendPacket(crafting);

				//{
				//	McpeContainerSetSlot setSlot = McpeContainerSetSlot.CreateObject();
				//	setSlot.item = new MetadataSlot(new ItemStack(new ItemBlock(new Block(5), 0), 4));
				//	setSlot.windowId = 0;
				//	setSlot.slot = 0;
				//	SendPackage(setSlot);
				//}

				{
					McpeMobEquipment eq = McpeMobEquipment.CreateObject();
					eq.runtimeEntityId = EntityId;
					eq.slot = 10;
					eq.selectedSlot = 1;
					eq.item = new ItemBlock(new Block(5), 0) {Count = 1};
					SendPacket(eq);
				}
			}
		}

		public void WriteInventoryToFile(string fileName, ItemStacks slots)
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
					Log.Debug("Extradata: \n" + extraData);
					if (extraData.Contains("ench"))
					{
						NbtList ench = (NbtList) extraData["ench"];

						NbtCompound enchComp = (NbtCompound) ench[0];
						var id = enchComp["id"].ShortValue;
						var lvl = enchComp["lvl"].ShortValue;
						writer.WriteLine($"new Item({slot.Id}, {slot.Metadata}, {slot.Count}){{ExtraData = new NbtCompound {{new NbtList(\"ench\") {{new NbtCompound {{new NbtShort(\"id\", {id}), new NbtShort(\"lvl\", {lvl}) }} }} }} }},");
					}
					else if (extraData.Contains("Fireworks"))
					{
						NbtCompound fireworks = (NbtCompound) extraData["Fireworks"];
						NbtList explosions = (NbtList) fireworks["Explosions"];
						byte flight = fireworks["Flight"].ByteValue;
						if (explosions.Count > 0)
						{
							NbtCompound compound = (NbtCompound) explosions[0];
							byte[] fireworkColor = compound["FireworkColor"].ByteArrayValue;
							byte[] fireworkFade = compound["FireworkFade"].ByteArrayValue;
							byte fireworkFlicker = compound["FireworkFlicker"].ByteValue;
							byte fireworkTrail = compound["FireworkTrail"].ByteValue;
							byte fireworkType = compound["FireworkType"].ByteValue;

							writer.WriteLine($"new Item({slot.Id}, {slot.Metadata}, {slot.Count}){{ExtraData = new NbtCompound {{ new NbtCompound(\"Fireworks\") {{ new NbtList(\"Explosions\") {{ new NbtCompound {{ new NbtByteArray(\"FireworkColor\", new byte[]{{{fireworkColor[0]}}}), new NbtByteArray(\"FireworkFade\", new byte[0]), new NbtByte(\"FireworkFlicker\", {fireworkFlicker}), new NbtByte(\"FireworkTrail\", {fireworkTrail}), new NbtByte(\"FireworkType\", {fireworkType})  }} }}, new NbtByte(\"Flight\", {flight}) }} }} }},");
						}
						else
						{
							writer.WriteLine($"new Item({slot.Id}, {slot.Metadata}, {slot.Count}){{ExtraData = new NbtCompound {{new NbtCompound(\"Fireworks\") {{new NbtList(\"Explosions\", NbtTagType.Compound), new NbtByte(\"Flight\", {flight}) }} }} }},");
						}
					}
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

		public string MetadataToCode(MetadataDictionary metadata)
		{
			StringBuilder sb = new StringBuilder();

			sb.AppendLine();
			sb.AppendLine("MetadataDictionary metadata = new MetadataDictionary();");

			foreach (var kvp in metadata._entries)
			{
				int idx = kvp.Key;
				MetadataEntry entry = kvp.Value;

				sb.Append($"metadata[{idx}] = new ");
				switch (entry.Identifier)
				{
					case 0:
					{
						var e = (MetadataByte) entry;
						sb.Append($"{e.GetType().Name}({e.Value});");
						break;
					}
					case 1:
					{
						var e = (MetadataShort) entry;
						sb.Append($"{e.GetType().Name}({e.Value});");
						break;
					}
					case 2:
					{
						var e = (MetadataInt) entry;
						sb.Append($"{e.GetType().Name}({e.Value});");
						break;
					}
					case 3:
					{
						var e = (MetadataFloat) entry;
						sb.Append($"{e.GetType().Name}({e.Value.ToString(NumberFormatInfo.InvariantInfo)}f);");
						break;
					}
					case 4:
					{
						var e = (MetadataString) entry;
						sb.Append($"{e.GetType().Name}(\"{e.Value}\");");
						break;
					}
					case 5:
					{
						var e = (MetadataSlot) entry;
						sb.Append($"{e.GetType().Name}({e.Value});");
						break;
					}
					case 6:
					{
						var e = (MetadataIntCoordinates) entry;
						sb.Append($"{e.GetType().Name}({e.Value});");
						break;
					}
					case 7:
					{
						var e = (MetadataLong) entry;
						sb.Append($"{e.GetType().Name}({e.Value});");
						if (idx == 0)
						{
							sb.Append($" // {Convert.ToString((long) e.Value, 2)}; {FlagsToString(e.Value)}");
						}
						break;
					}
					case 8:
					{
						var e = (MetadataVector3) entry;
						sb.Append($"{e.GetType().Name}({e.Value});");
						break;
					}
				}
				sb.AppendLine();
			}

			return sb.ToString();
		}

		private static string FlagsToString(long input)
		{
			BitArray bits = new BitArray(BitConverter.GetBytes(input));

			byte[] bytes = new byte[8];
			bits.CopyTo(bytes, 0);

			List<Entity.DataFlags> flags = new List<Entity.DataFlags>();
			foreach (var val in Enum.GetValues(typeof(Entity.DataFlags)))
			{
				if (bits[(int) val]) flags.Add((Entity.DataFlags) val);
			}

			StringBuilder sb = new StringBuilder();
			sb.Append(string.Join(", ", flags));
			sb.Append("; ");
			for (var i = 0; i < bits.Count; i++)
			{
				if (bits[i]) sb.Append($"{i}, ");
			}

			return sb.ToString();
		}

		public ConcurrentDictionary<long, Entity> Entities { get; private set; } = new ConcurrentDictionary<long, Entity>();

		public string CodeName(string name, bool firstUpper = false)
		{
			name = name.ToLowerInvariant();

			bool upperCase = firstUpper;

			var result = string.Empty;
			for (int i = 0; i < name.Length; i++)
			{
				if (name[i] == ' ' || name[i] == '_')
				{
					upperCase = true;
				}
				else
				{
					if ((i == 0 && firstUpper) || upperCase)
					{
						result += name[i].ToString().ToUpperInvariant();
						upperCase = false;
					}
					else
					{
						result += name[i];
					}
				}
			}

			result = result.Replace(@"[]", "s");
			return result;
		}

		public virtual void OnConnectionRequestAccepted()
		{
			Thread.Sleep(50);
			SendNewIncomingConnection();
			//_connectedPingTimer = new Timer(state => SendConnectedPing(), null, 1000, 1000);
			Thread.Sleep(50);
			SendLogin(Username);
		}

		private int _numberOfChunks = 0;

		public ConcurrentDictionary<Tuple<int, int>, bool> _chunks = new ConcurrentDictionary<Tuple<int, int>, bool>();
		public IndentedTextWriter _mobWriter;

		public virtual void HandleBatch(McpeWrapper batch)
		{
			FirstPacketWaitHandle.Set();

			var messages = new List<Packet>();


			// Get bytes
			byte[] payload = batch.payload;

			if (Session.CryptoContext != null && Session.CryptoContext.UseEncryption)
			{
				FirstEncryptedPacketWaitHandle.Set();
				payload = CryptoUtils.Decrypt(payload, Session.CryptoContext);
			}

			MemoryStream stream = new MemoryStream(payload);
			if (stream.ReadByte() != 0x78)
			{
				throw new InvalidDataException("Incorrect ZLib header. Expected 0x78 0x9C");
			}
			stream.ReadByte();
			using (var defStream2 = new DeflateStream(stream, CompressionMode.Decompress, false))
			{
				// Get actual package out of bytes
				using (MemoryStream destination = MiNetServer.MemoryStreamManager.GetStream())
				{
					defStream2.CopyTo(destination);
					destination.Position = 0;
					do
					{
						byte[] internalBuffer = null;
						try
						{
							int len = (int) VarInt.ReadUInt32(destination);
							long pos = destination.Position;
							int id = (int) VarInt.ReadUInt32(destination);
							len = (int) (len - (destination.Position - pos)); // calculate len of buffer after varint
							internalBuffer = new byte[len];
							destination.Read(internalBuffer, 0, len);

							if (id == 0x8e) throw new Exception("Wrong code, didn't expect a 0x8E in a batched packet");

							var packet = PacketFactory.Create((byte) id, internalBuffer, "mcpe") ??
										new UnknownPacket((byte) id, internalBuffer);
							messages.Add(packet);

							//if (Log.IsDebugEnabled) Log.Debug($"Batch: {packet.GetType().Name} 0x{packet.Id:x2}");
							if (packet is UnknownPacket) Log.Error($"Batch: {packet.GetType().Name} 0x{packet.Id:x2}");
							//if (!(package is McpeFullChunkData)) Log.Debug($"Batch: {package.GetType().Name} 0x{package.Id:x2} \n{Package.HexDump(internalBuffer)}");
						}
						catch (Exception e)
						{
							if (internalBuffer != null)
								Log.Error($"Batch error while reading:\n{Packet.HexDump(internalBuffer)}");
							Log.Error("Batch processing", e);
						}
					} while (destination.Position < destination.Length);
				}
			}

			//Log.Error($"Batch had {messages.Count} packets.");
			if (messages.Count == 0) Log.Error($"Batch had 0 packets.");

			foreach (var msg in messages)
			{
				msg.DatagramSequenceNumber = batch.DatagramSequenceNumber;
				msg.OrderingChannel = batch.OrderingChannel;
				msg.OrderingIndex = batch.OrderingIndex;
				HandlePacket(msg);
				msg.PutPool();
			}
		}

		public void SendPacket(Packet message, short mtuSize, ref int reliableMessageNumber)
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

		private void TraceReceive(Packet message)
		{
			if (!Log.IsDebugEnabled) return;

			string typeName = message.GetType().Name;

			string includePattern = Config.GetProperty("TracePackets.Include", ".*");
			string excludePattern = Config.GetProperty("TracePackets.Exclude", null);
			int verbosity = Config.GetProperty("TracePackets.Verbosity", 0);
			verbosity = Config.GetProperty($"TracePackets.Verbosity.{typeName}", verbosity);

			if (!Regex.IsMatch(typeName, includePattern))
			{
				return;
			}

			if (!string.IsNullOrWhiteSpace(excludePattern) && Regex.IsMatch(typeName, excludePattern))
			{
				return;
			}

			if (verbosity == 0)
			{
				Log.Debug($"> Receive: {message.Id} (0x{message.Id:x2}): {message.GetType().Name}");
			}
			else if (verbosity == 1)
			{
				var jsonSerializerSettings = new JsonSerializerSettings
				{
					PreserveReferencesHandling = PreserveReferencesHandling.All,

					Formatting = Formatting.Indented,
				};
				jsonSerializerSettings.Converters.Add(new NbtIntConverter());
				jsonSerializerSettings.Converters.Add(new NbtStringConverter());

				string result = JsonConvert.SerializeObject(message, jsonSerializerSettings);
				Log.Debug($"> Receive: {message.Id} (0x{message.Id:x2}): {message.GetType().Name}\n{result}");
			}
			else if (verbosity == 2)
			{
				Log.Debug($"> Receive: {message.Id} (0x{message.Id:x2}): {message.GetType().Name}\n{Packet.HexDump(message.Bytes)}");
			}
		}

		private void TraceSend(Packet message)
		{
			if (!Log.IsDebugEnabled) return;

			string typeName = message.GetType().Name;

			string includePattern = Config.GetProperty("TracePackets.Include", ".*");
			string excludePattern = Config.GetProperty("TracePackets.Exclude", "");
			int verbosity = Config.GetProperty("TracePackets.Verbosity", 0);
			verbosity = Config.GetProperty($"TracePackets.Verbosity.{typeName}", verbosity);


			if (!Regex.IsMatch(typeName, includePattern))
			{
				return;
			}
			if (!string.IsNullOrWhiteSpace(excludePattern) && Regex.IsMatch(typeName, excludePattern))
			{
				return;
			}

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

			SendPacket(packet);
		}

		public void SendConnectedPong(long sendpingtime)
		{
			var packet = new ConnectedPong
			{
				sendpingtime = sendpingtime,
				sendpongtime = sendpingtime + 200
			};

			SendPacket(packet);
		}

		public void SendOpenConnectionRequest1()
		{
			var packet = new OpenConnectionRequest1()
			{
				raknetProtocolVersion = 9,
				mtuSize = _mtuSize
			};

			byte[] data = packet.Encode();

			TraceSend(packet);

			// 1446 - 1464
			// 1087 1447
			byte[] data2 = new byte[_mtuSize - data.Length - 10];
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

			SendPacket(packet);
		}

		public void SendPacket(Packet packet)
		{
			SendPacket(packet, _mtuSize, ref _reliableMessageNumber);
			packet.PutPool();
		}

		public void SendNewIncomingConnection()
		{
			var packet = NewIncomingConnection.CreateObject();
			packet.clientendpoint = _serverEndpoint;
			packet.systemAddresses = new IPEndPoint[20];
			for (int i = 0; i < 20; i++)
			{
				packet.systemAddresses[i] = new IPEndPoint(IPAddress.Any, 0);
			}

			SendPacket(packet);
		}

		public void SendChat(string text)
		{
			var packet = McpeText.CreateObject();
			packet.type = (byte) MessageType.Chat;
			packet.source = Username;
			packet.message = text;

			SendPacket(packet);
		}

		public void SendMcpeMovePlayer()
		{
			//SendChat($"Moving Entity ID: {EntityId}  to {CurrentLocation}");

			McpeMovePlayer movePlayerPacket = McpeMovePlayer.CreateObject();
			movePlayerPacket.runtimeEntityId = EntityId;
			movePlayerPacket.x = CurrentLocation.X;
			movePlayerPacket.y = CurrentLocation.Y;
			movePlayerPacket.z = CurrentLocation.Z;
			movePlayerPacket.yaw = 91;
			movePlayerPacket.pitch = 28;
			movePlayerPacket.headYaw = 91;

			SendPacket(movePlayerPacket);
		}


		public void SendDisconnectionNotification()
		{
			Session.CryptoContext = null;
			SendPacket(new DisconnectionNotification());
		}
	}
}