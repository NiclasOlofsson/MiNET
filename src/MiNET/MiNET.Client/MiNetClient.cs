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
// The Original Code is Niclas Olofsson.
// 
// The Original Developer is the Initial Developer.  The Initial Developer of
// the Original Code is Niclas Olofsson.
// 
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2017 Niclas Olofsson. 
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
using Newtonsoft.Json.Linq;

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
		public long EntityId { get; private set; }
		public long NetworkEntityId { get; private set; }
		public PlayerNetworkSession Session { get; set; }
		private Thread _mainProcessingThread;
		public int ChunkRadius { get; set; } = 5;

		public LevelInfo Level { get; } = new LevelInfo();

		//private long _clientGuid = new Random().Next();
		private long _clientGuid = 1111111;

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

			//var client = new MiNetClient(null, "TheGrey", new DedicatedThreadPool(new DedicatedThreadPoolSettings(Environment.ProcessorCount)));
			//var client = new MiNetClient(new IPEndPoint(IPAddress.Parse("192.168.0.5"), 19132), "TheGrey", new DedicatedThreadPool(new DedicatedThreadPoolSettings(Environment.ProcessorCount)));
			//var client = new MiNetClient(new IPEndPoint(IPAddress.Parse("192.168.0.3"), 19132), "TheGrey", new DedicatedThreadPool(new DedicatedThreadPoolSettings(Environment.ProcessorCount)));
			var client = new MiNetClient(new IPEndPoint(IPAddress.Parse("173.208.195.250"), 19132), "TheGrey", new DedicatedThreadPool(new DedicatedThreadPoolSettings(Environment.ProcessorCount)));
			//var client = new MiNetClient(new IPEndPoint(Dns.GetHostEntry("true-games.org").AddressList[0], 2222), "TheGrey", new DedicatedThreadPool(new DedicatedThreadPoolSettings(Environment.ProcessorCount)));
			//var client = new MiNetClient(new IPEndPoint(Dns.GetHostEntry("yodamine.net").AddressList[0], 19132), "TheGrey", new DedicatedThreadPool(new DedicatedThreadPoolSettings(Environment.ProcessorCount)));
			//var client = new MiNetClient(new IPEndPoint(IPAddress.Loopback, 19132), "TheGrey", new DedicatedThreadPool(new DedicatedThreadPoolSettings(Environment.ProcessorCount)));

			//var client = new MiNetClient(new IPEndPoint(IPAddress.Parse("54.229.52.56"), 27212), "TheGrey", new DedicatedThreadPool(new DedicatedThreadPoolSettings(Environment.ProcessorCount)));

			// 157.7.202.57:29473
			//var client = new MiNetClient(new IPEndPoint(IPAddress.Parse("157.7.202.57"), 19132), "TheGrey");

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

			Action<Task, Item, BlockCoordinates> doUseItem = BotHelpers.DoUseItem(client);
			Action<Task, int, Item, int> doSetSlot = BotHelpers.DoContainerSetSlot(client);
			Action<Task, PlayerLocation> doMoveTo = BotHelpers.DoMoveTo(client);
			Action<Task, string> doSendCommand = BotHelpers.DoSendCommand(client);

			//.ContinueWith(t => doMovePlayer(t, client.CurrentLocation + new Vector3(0, 1.62f, 0)))
			//.ContinueWith(t => doMoveTo(t, client.CurrentLocation + new Vector3(10, 1.62f, 10)))

			Task.Run(BotHelpers.DoWaitForSpawn(client))
				.ContinueWith(t => BotHelpers.DoMobEquipment(client)(t, new ItemBlock(new Cobblestone(), 0) {Count = 64}, 0))
				//.ContinueWith(t => BotHelpers.DoMoveTo(client)(t, new PlayerLocation(client.CurrentLocation.ToVector3() - new Vector3(0, 1, 0), 180, 180, 180)))
				//.ContinueWith(t => doMoveTo(t, new PlayerLocation(40, 5.62f, -20, 180, 180, 180)))
				.ContinueWith(t => doMoveTo(t, new PlayerLocation(0, 5.62, 0, 180 + 45, 180 + 45, 180)))
				//.ContinueWith(t => doMoveTo(t, new PlayerLocation(22, 5.62, 40, 180 + 45, 180 + 45, 180)))
				//.ContinueWith(t => doMoveTo(t, new PlayerLocation(50, 5.62f, 17, 180, 180, 180)))
				.ContinueWith(t => doSendCommand(t, "/test"))
				.ContinueWith(t => doUseItem(t, new ItemBlock(new Stone(), 0) {Count = 1}, new BlockCoordinates(22, 4, 42)))
				.ContinueWith(t => Task.Delay(5000).Wait())
				.ContinueWith(t => doSetSlot(t, 2, new ItemIronSword() {Count = 1}, 0))
				.ContinueWith(t => doSetSlot(t, 2, ItemFactory.GetItem(351, 4, 64), 1))
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
				_mainProcessingThread?.Abort();
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
			else if (typeof (McpeServerToClientHandshake) == message.GetType())
			{
				OnMcpeServerToClientHandshake((McpeServerToClientHandshake) message);

				return;
			}
			else if (typeof (ConnectedPing) == message.GetType())
			{
				ConnectedPing msg = (ConnectedPing) message;
				SendConnectedPong(msg.sendpingtime);
				return;
			}

			else if (typeof (McpeResourcePacksInfo) == message.GetType())
			{
				OnMcpeResourcePacksInfo((McpeResourcePacksInfo) message);
				return;
			}

			else if (typeof (McpeResourcePackStack) == message.GetType())
			{
				OnMcpeResourcePackStack((McpeResourcePackStack) message);
				return;
			}

			else if (typeof (McpeResourcePackDataInfo) == message.GetType())
			{
				OnMcpeResourcePackDataInfo((McpeResourcePackDataInfo) message);
				return;
			}

			else if (typeof (McpeResourcePackChunkData) == message.GetType())
			{
				OnMcpeResourcePackChunkData((McpeResourcePackChunkData) message);
				return;
			}

			else if (typeof (McpeFullChunkData) == message.GetType())
			{
				OnFullChunkData((McpeFullChunkData) message);
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

			else if (typeof (McpeGameRulesChanged) == message.GetType())
			{
				OnMcpeGameRulesChanged((McpeGameRulesChanged) message);

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
			else if (typeof (McpeRespawn) == message.GetType())
			{
				OnMcpeRespawn((McpeRespawn) message);

				return;
			}

			else if (typeof (McpeBlockEvent) == message.GetType())
			{
				OnMcpeBlockEvent(message);
				return;
			}

			else if (typeof (McpeBlockEntityData) == message.GetType())
			{
				OnMcpeBlockEntityData((McpeBlockEntityData) message);
				return;
			}

			else if (typeof (McpeAddPlayer) == message.GetType())
			{
				OnMcpeAddPlayer(message);

				return;
			}
			else if (typeof (McpeAddEntity) == message.GetType())
			{
				OnMcpeAddEntity((McpeAddEntity) message);

				return;
			}
			else if (typeof (McpeRemoveEntity) == message.GetType())
			{
				OnMcpeRemoveEntity((McpeRemoveEntity) message);

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

			else if (typeof (McpeMobArmorEquipment) == message.GetType())
			{
				OnMcpeMobArmorEquipment((McpeMobArmorEquipment) message);

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

			else if (typeof (McpeContainerOpen) == message.GetType())
			{
				OnMcpeContainerOpen((McpeContainerOpen) message);
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
				OnMcpeMoveEntity((McpeMoveEntity) message);
				return;
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

			else if (typeof (McpePlayStatus) == message.GetType())
			{
				OnMcpePlayStatus((McpePlayStatus) message);
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

			else if (typeof (McpeMapInfoRequest) == message.GetType())
			{
				OnMcpeMapInfoRequest((McpeMapInfoRequest) message);
			}

			else if (typeof (McpeClientboundMapItemData) == message.GetType())
			{
				OnMcpeClientboundMapItemData((McpeClientboundMapItemData) message);
			}

			else if (typeof (McpeInteract) == message.GetType())
			{
				OnMcpeInteract((McpeInteract) message);
			}

			else if (typeof (McpeLevelSoundEvent) == message.GetType())
			{
				OnMcpeLevelSoundEvent((McpeLevelSoundEvent) message);
			}

			else if (typeof (McpeAvailableCommands) == message.GetType())
			{
				OnMcpeAvailableCommands((McpeAvailableCommands) message);
			}
			else if (typeof (McpeCommandStep) == message.GetType())
			{
				OnMcpeCommandStep((McpeCommandStep) message);
			}
			else if (typeof (McpeChangeDimension) == message.GetType())
			{
				OnMcpeChangeDimension((McpeChangeDimension) message);
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

		private void OnMcpeChangeDimension(McpeChangeDimension message)
		{
			Thread.Sleep(3000);
			McpePlayerAction action = McpePlayerAction.CreateObject();
			action.runtimeEntityId = EntityId;
			action.actionId = (int) PlayerAction.DimensionChange;
			SendPackage(action);
		}

		private void OnMcpeClientboundMapItemData(McpeClientboundMapItemData message)
		{
		}

		private void OnMcpeMapInfoRequest(McpeMapInfoRequest message)
		{
		}

		private void OnMcpeMobArmorEquipment(McpeMobArmorEquipment message)
		{
		}

		private void OnMcpeMoveEntity(McpeMoveEntity message)
		{
			//if (Entities.ContainsKey(message.entityId))
			//{
			//    Entity entity = Entities[message.entityId];

			//    Log.Debug($"Entity move: Id={message.entityId}, Type={entity.EntityTypeId} - 0x{entity.EntityTypeId:x2}");
			//}
		}


		private void OnMcpeRespawn(McpeRespawn message)
		{
			CurrentLocation = new PlayerLocation(message.x, message.y, message.z);
		}

		private void OnMcpeLevelSoundEvent(McpeLevelSoundEvent message)
		{
			Log.Debug($"SoundId: {message.soundId}, Position: {message.position}, ExtraData: {message.extraData}, Pitch: {message.pitch}, unknown1: {message.unknown1}, disableRelativeVolume: {message.disableRelativeVolume}");
		}

		private void OnMcpeGameRulesChanged(McpeGameRulesChanged message)
		{
			var rules = message.rules;
			foreach (var rule in rules)
			{
				Log.Debug($"Rule: {rule}");
			}
		}

		private void OnMcpeResourcePacksInfo(McpeResourcePacksInfo message)
		{
			Log.Warn($"HEX: \n{Package.HexDump(message.Bytes)}");

			var sb = new StringBuilder();
			sb.AppendLine();

			sb.AppendLine("Resource packs:");
			foreach (ResourcePackInfo info in message.resourcepackinfos)
			{
				sb.AppendLine($"ID={info.PackIdVersion.Id}, Version={info.PackIdVersion.Version}, Unknown={info.Size}");
			}

			sb.AppendLine("Behavior packs:");
			foreach (ResourcePackInfo info in message.behahaviorpackinfos)
			{
				sb.AppendLine($"ID={info.PackIdVersion.Id}, Version={info.PackIdVersion.Version}");
			}

			Log.Debug(sb.ToString());

			//McpeResourcePackClientResponse response = new McpeResourcePackClientResponse();
			//response.responseStatus = 3;
			//SendPackage(response);

			if (message.resourcepackinfos.Count != 0)
			{
				ResourcePackIds resourcePackIds = new ResourcePackIds();

				foreach (var packInfo in message.resourcepackinfos)
				{
					resourcePackIds.Add(packInfo.PackIdVersion.Id);
				}

				McpeResourcePackClientResponse response = new McpeResourcePackClientResponse();
				response.responseStatus = 2;
				response.resourcepackids = resourcePackIds;
				SendPackage(response);
			}
			else
			{
				McpeResourcePackClientResponse response = new McpeResourcePackClientResponse();
				response.responseStatus = 3;
				SendPackage(response);
			}
		}

		private void OnMcpeResourcePackStack(McpeResourcePackStack message)
		{
			Log.Debug($"HEX: \n{Package.HexDump(message.Bytes)}");

			var sb = new StringBuilder();
			sb.AppendLine();

			sb.AppendLine("Resource pack stacks:");
			foreach (var info in message.resourcepackidversions)
			{
				sb.AppendLine($"ID={info.Id}, Version={info.Version}");
			}

			sb.AppendLine("Behavior pack stacks:");
			foreach (var info in message.behaviorpackidversions)
			{
				sb.AppendLine($"ID={info.Id}, Version={info.Version}");
			}

			Log.Debug(sb.ToString());

			//if (message.resourcepackidversions.Count != 0)
			//{
			//	McpeResourcePackClientResponse response = new McpeResourcePackClientResponse();
			//	response.responseStatus = 2;
			//	response.resourcepackidversions = message.resourcepackidversions;
			//	SendPackage(response);
			//}
			//else
			{
				McpeResourcePackClientResponse response = new McpeResourcePackClientResponse();
				response.responseStatus = 4;
				SendPackage(response);
			}
		}

		private Dictionary<string, uint> resourcePackDataInfos = new Dictionary<string, uint>();

		private void OnMcpeResourcePackDataInfo(McpeResourcePackDataInfo message)
		{
			var packageId = message.packageId;
			McpeResourcePackChunkRequest request = new McpeResourcePackChunkRequest();
			request.packageId = packageId;
			request.chunkIndex = 0;
			SendPackage(request);
			resourcePackDataInfos.Add(message.packageId, message.chunkCount);
		}

		private void OnMcpeResourcePackChunkData(McpeResourcePackChunkData message)
		{
			string fileName = Path.GetTempPath() + "ResourcePackChunkData_" + message.packageId + ".zip";
			Log.Warn("Writing ResourcePackChunkData part " + message.chunkIndex.ToString() + " to filename: " + fileName);

			FileStream file = File.OpenWrite(fileName);
			file.Seek((long) message.progress, SeekOrigin.Begin);

			file.Write(message.payload, 0, message.payload.Length);
			file.Close();

			Log.Debug($"packageId={message.packageId}");
			Log.Debug($"unknown1={message.chunkIndex}");
			Log.Debug($"unknown3={message.progress}");
			Log.Debug($"Reported Lenght={message.length}");
			Log.Debug($"Actual Lenght={message.payload.Length}");

			if (message.chunkIndex + 1 < resourcePackDataInfos[message.packageId])
			{
				var packageId = message.packageId;
				McpeResourcePackChunkRequest request = new McpeResourcePackChunkRequest();
				request.packageId = packageId;
				request.chunkIndex = message.chunkIndex + 1;
				SendPackage(request);
			}
			else
			{
				resourcePackDataInfos.Remove(message.packageId);
			}

			if (resourcePackDataInfos.Count == 0)
			{
				McpeResourcePackClientResponse response = new McpeResourcePackClientResponse();
				response.responseStatus = 3;
				SendPackage(response);
			}
		}

		private void OnMcpeContainerOpen(McpeContainerOpen message)
		{
			var stringWriter = new StringWriter();
			ObjectDumper.Write(message, 1, stringWriter);

			Log.Debug($"Handled chest for {EntityId} 0x{message.Id:x2} {message.GetType().Name}:\n{stringWriter} ");
		}

		private void OnMcpeAvailableCommands(McpeAvailableCommands message)
		{
			{
				dynamic json = JObject.Parse(message.commands);

				//if (Log.IsDebugEnabled) Log.Debug($"Command JSON:\n{json}");
				string fileName = Path.GetTempPath() + "AvailableCommands_" + Guid.NewGuid() + ".json";
				Log.Info($"Writing commands to filename: {fileName}");
				File.WriteAllText(fileName, message.commands);
			}
			{
				dynamic json = JObject.Parse(message.unknown);

				//if (Log.IsDebugEnabled) Log.Debug($"Command (unknown) JSON:\n{json}");
			}
		}

		private void OnMcpeCommandStep(McpeCommandStep message)
		{
			var jsonSerializerSettings = new JsonSerializerSettings
			{
				PreserveReferencesHandling = PreserveReferencesHandling.None,
				Formatting = Formatting.Indented,
			};

			var commanJson = JsonConvert.DeserializeObject(message.commandOutputJson);
			Log.Debug($"CommandJson\n{JsonConvert.SerializeObject(commanJson, jsonSerializerSettings)}");
		}

		private void OnMcpeSetTime(McpeSetTime message)
		{
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
				Log.Warn($"{playerRecord.GetType()} Player: {playerRecord.DisplayName}, {playerRecord.EntityId}, {playerRecord.ClientUuid}");
			}

			//Log.Debug($"\n{Package.HexDump(message.Bytes)}");
		}

		public void SendLogin(string username)
		{
			JWT.JsonMapper = new NewtonsoftMapper();

			CngKey clientKey = CryptoUtils.GenerateClientKey();
			byte[] data = CryptoUtils.CompressJwtBytes(CryptoUtils.EncodeJwt(Username, clientKey, IsEmulator), CryptoUtils.EncodeSkinJwt(clientKey), CompressionLevel.Fastest);

			McpeLogin loginPacket = new McpeLogin
			{
				protocolVersion = Config.GetProperty("EnableEdu", false) ? 111 : 113,
				edition = (byte) (Config.GetProperty("EnableEdu", false) ? 1 : 0),
				payload = data
			};

			Session.CryptoContext = new CryptoContext()
			{
				ClientKey = clientKey,
				UseEncryption = false,
			};

			SendPackage(loginPacket);
			//SendPackage(batch);
		}

		private void OnMcpeServerToClientHandshake(McpeServerToClientHandshake message)
		{
			string serverKey = message.serverPublicKey;
			byte[] randomKeyToken = message.token;

			// Initiate encryption

			InitiateEncryption(serverKey, randomKeyToken);
		}

		private void InitiateEncryption(string serverKey, byte[] randomKeyToken)
		{
			try
			{
				ECDiffieHellmanPublicKey publicKey = CryptoUtils.CreateEcDiffieHellmanPublicKey(serverKey);
				Log.Debug("ServerKey (b64):\n" + serverKey);
				//Log.Debug($"Cert:\n{publicKey.ToXmlString()}");

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
				McpeClientToServerHandshake magic = new McpeClientToServerHandshake();
				//byte[] encodedMagic = magic.Encode();
				//McpeBatch batch = BatchUtils.CreateBatchPacket(encodedMagic, 0, encodedMagic.Length, CompressionLevel.Fastest, true);
				//batch.Encode();
				SendPackage(magic);
			}
			catch (Exception e)
			{
				Log.Error("Initiate encryption", e);
			}
		}

		public AutoResetEvent FirstEncryptedPacketWaitHandle = new AutoResetEvent(false);

		public AutoResetEvent FirstPacketWaitHandle = new AutoResetEvent(false);

		public UserPermission UserPermission { get; set; }

		private void OnMcpeAdventureSettings(McpeAdventureSettings message)
		{
			Log.Debug($@"
Adventure settings 
	flags: 0x{message.flags:X2} - {Convert.ToString(message.flags, 2)}
	flags: 0x{message.userPermission:X2} - {Convert.ToString(message.userPermission, 2)}
");
			UserPermission = (UserPermission) message.userPermission;
		}

		private void OnMcpeInteract(McpeInteract message)
		{
			Log.Debug($"Interact: EID={message.targetRuntimeEntityId}, Action ID={message.actionId}");
		}

		private void OnMcpeAnimate(McpeAnimate message)
		{
			Log.Debug($"Animate: EID={message.runtimeEntityId}, Action ID={message.actionId}");
		}

		private void OnMcpeHurtArmor(McpeHurtArmor message)
		{
			Log.Debug($"Hurt Armor: Health={message.health}");
		}

		public AutoResetEvent PlayerStatusChangedWaitHandle = new AutoResetEvent(false);
		public bool HasSpawned { get; set; }

		private void OnMcpePlayStatus(McpePlayStatus message)
		{
			if (Log.IsDebugEnabled) Log.Debug($"Player status={message.status}");
			PlayerStatus = message.status;

			if (PlayerStatus == 3)
			{
				HasSpawned = true;
				if (IsEmulator)
				{
					PlayerStatusChangedWaitHandle.Set();

					SendMcpeMovePlayer();
				}
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
			if (Log.IsDebugEnabled) Log.Debug($"PlayerEquipment: Entity ID: {message.runtimeEntityId}, Selected Slot: {message.selectedSlot}, Slot: {message.slot}, Item ID: {message.item.Id}");
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
						eq.runtimeEntityId = EntityId;
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
					eq.runtimeEntityId = EntityId;
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
					eq.runtimeEntityId = EntityId;
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
			if (Log.IsDebugEnabled) Log.Debug($"Set inventory slot on window 0x{message.windowId:X2} with slot: {message.slot} HOTBAR: {message.hotbarslot} Item ID: {itemStack.Id} Item Count: {itemStack.Count} Meta: {itemStack.Metadata}: DatagramSequenceNumber: {message.DatagramSequenceNumber}, ReliableMessageNumber: {message.ReliableMessageNumber}, OrderingIndex: {message.OrderingIndex}");
		}

		private void OnMcpeContainerSetContent(Package message)
		{
			McpeContainerSetContent msg = (McpeContainerSetContent) message;
			Log.Debug($"Set container content on Window ID: 0x{msg.windowId:x2}, {msg.hotbarData.Count}, Count: {msg.slotData.Count}");

			if (IsEmulator) return;

			ItemStacks slots = msg.slotData;

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
					Log.Error($"{msg.windowId} Hotbar slot: {i} val: {val.Value}");
					i++;
				}
			}
			else if (msg.windowId == 0x7b)
			{
				var hotbar = msg.hotbarData.GetValues();
				int i = 0;
				foreach (MetadataEntry entry in hotbar)
				{
					MetadataInt val = (MetadataInt) entry;
					Log.Error($"{msg.windowId} Hotbar slot: {i} val: {val.Value}");
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
		}

		private void OnMcpeMobEffect(Package message)
		{
		}

		private void OnMcpeLevelEvent(Package message)
		{
			McpeLevelEvent msg = (McpeLevelEvent) message;
			int data = msg.data;
			if (msg.eventId == 2001)
			{
				int blockId = data & 0xff;
				int metadata = data >> 12;
				Log.Debug($"BlockID={blockId}, Metadata={metadata}");
			}
		}

		private void OnMcpeUpdateBlock(McpeUpdateBlock message)
		{
			Log.Debug($"Block Coordinates={message.coordinates}, Block ID={message.blockId}, Metadata={message.blockMetaAndPriority & 0xf}");
		}

		private void OnMcpeMovePlayer(McpeMovePlayer message)
		{
			if (message.runtimeEntityId != EntityId) return;

			CurrentLocation = new PlayerLocation(message.x, message.y, message.z);
			Level.SpawnX = (int) message.x;
			Level.SpawnY = (int) message.y;
			Level.SpawnZ = (int) message.z;
			SendMcpeMovePlayer();
		}

		private static void OnMcpeSetEntityData(Package message)
		{
			McpeSetEntityData msg = (McpeSetEntityData) message;
			Log.DebugFormat("McpeSetEntityData Entity ID: {0}, Metadata: {1}", msg.runtimeEntityId, MetadataToCode(msg.metadata));
			//Log.Debug($"Package 0x{message.Id:X2}\n{Package.HexDump(message.Bytes)}");
		}

		public static string MetadataToCode(MetadataDictionary metadata)
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
			foreach (var val in Enum.GetValues(typeof (Entity.DataFlags)))
			{
				if (bits[(int) val]) flags.Add((Entity.DataFlags) val);
			}

			StringBuilder sb = new StringBuilder();
			return sb.Append(string.Join(", ", flags)).ToString();
		}

		private void OnMcpeAddPlayer(Package message)
		{
			if (IsEmulator) return;

			McpeAddPlayer msg = (McpeAddPlayer) message;
			Log.DebugFormat("McpeAddPlayer Entity ID: {0}", msg.entityIdSelf);
			Log.DebugFormat("McpeAddPlayer Runtime Entity ID: {0}", msg.runtimeEntityId);
			Log.DebugFormat("X: {0}", msg.x);
			Log.DebugFormat("Y: {0}", msg.y);
			Log.DebugFormat("Z: {0}", msg.z);
			Log.DebugFormat("Yaw: {0}", msg.yaw);
			Log.DebugFormat("Pitch: {0}", msg.pitch);
			Log.DebugFormat("Velocity X: {0}", msg.speedX);
			Log.DebugFormat("Velocity Y: {0}", msg.speedY);
			Log.DebugFormat("Velocity Z: {0}", msg.speedZ);
			Log.DebugFormat("Metadata: {0}", MetadataToCode(msg.metadata));
			//Log.DebugFormat("Links count: {0}", msg.links);
		}

		public ConcurrentDictionary<long, Entity> Entities { get; private set; } = new ConcurrentDictionary<long, Entity>();

		private void OnMcpeAddEntity(McpeAddEntity message)
		{
			if (IsEmulator) return;

			if (!Entities.ContainsKey(message.entityIdSelf))
			{
				Entity entity = new Entity((int) message.entityType, null);
				entity.EntityId = message.runtimeEntityId;
				entity.KnownPosition = new PlayerLocation(message.x, message.y, message.z, message.yaw, message.yaw, message.pitch);
				entity.Velocity = new Vector3(message.speedX, message.speedY, message.speedZ);
				Entities.TryAdd(entity.EntityId, entity);
			}

			byte[] typeBytes = BitConverter.GetBytes(message.entityType);

			Log.DebugFormat("McpeAddEntity Entity ID: {0}", message.entityIdSelf);
			Log.DebugFormat("McpeAddEntity Runtime Entity ID: {0}", message.runtimeEntityId);
			Log.DebugFormat("Entity Type: {0} - 0x{0:x2}", message.entityType);
			Log.DebugFormat("Entity Family: {0} - 0x{0:x2}", typeBytes[1]);
			Log.DebugFormat("Entity Type ID: {0} - 0x{0:x2} {1}", typeBytes[0], (EntityType) typeBytes[0]);
			Log.DebugFormat("X: {0}", message.x);
			Log.DebugFormat("Y: {0}", message.y);
			Log.DebugFormat("Z: {0}", message.z);
			Log.DebugFormat("Yaw: {0}", message.yaw);
			Log.DebugFormat("Pitch: {0}", message.pitch);
			Log.DebugFormat("Velocity X: {0}", message.speedX);
			Log.DebugFormat("Velocity Y: {0}", message.speedY);
			Log.DebugFormat("Velocity Z: {0}", message.speedZ);
			Log.DebugFormat("Metadata: {0}", MetadataToCode(message.metadata));

			if (message.metadata.Contains(0))
			{
				long? value = ((MetadataLong) message.metadata[0])?.Value;
				if (value != null)
				{
					long dataValue = (long) value;
					Log.Debug($"Bit-array datavalue: dec={dataValue} hex=0x{dataValue:x2}, bin={Convert.ToString(dataValue, 2)}b ");
				}
			}
			if (Log.IsDebugEnabled)
			{
				foreach (var attribute in message.attributes)
				{
					Log.Debug($"Entity attribute {attribute}");
				}
			}
			Log.DebugFormat("Links count: {0}", message.links);
		}

		private void OnMcpeRemoveEntity(McpeRemoveEntity message)
		{
			Entity value;
			Entities.TryRemove(message.entityIdSelf, out value);
		}

		private void OnMcpeAddItemEntity(Package message)
		{
			if (IsEmulator) return;

			//McpeAddItemEntity msg = (McpeAddItemEntity) message;
			//Log.DebugFormat("McpeAddEntity Entity ID: {0}", msg.entityId);
			//Log.DebugFormat("X: {0}", msg.x);
			//Log.DebugFormat("Y: {0}", msg.y);
			//Log.DebugFormat("Z: {0}", msg.z);
			//Log.DebugFormat("Velocity X: {0}", msg.speedX);
			//Log.DebugFormat("Velocity Y: {0}", msg.speedY);
			//Log.DebugFormat("Velocity Z: {0}", msg.speedZ);
			//Log.Info($"Item {msg.item}");
		}

		private static void OnMcpeBlockEntityData(McpeBlockEntityData message)
		{
			Log.DebugFormat("X: {0}", message.coordinates.X);
			Log.DebugFormat("Y: {0}", message.coordinates.Y);
			Log.DebugFormat("Z: {0}", message.coordinates.Z);
			Log.DebugFormat("NBT:\n{0}", message.namedtag.NbtFile.RootTag);
		}

		private static void OnMcpeBlockEvent(Package message)
		{
			McpeBlockEvent msg = (McpeBlockEvent) message;
			Log.DebugFormat("Coord: {0}", msg.coordinates);
			Log.DebugFormat("Case 1: {0}", msg.case1);
			Log.DebugFormat("Case 2: {0}", msg.case2);
		}

		private void OnMcpeStartGame(McpeStartGame message)
		{
			EntityId = message.runtimeEntityId;
			NetworkEntityId = message.entityIdSelf;
			_spawn = new Vector3(message.x, message.y, message.z);
			CurrentLocation = new PlayerLocation(_spawn);
			Log.Debug($@"
StartGame:
	entityId: {message.entityIdSelf}	
	runtimeEntityId: {message.runtimeEntityId}	
	spawn: {message.spawn}	
	unknown1: {message.unknown1}	
	dimension: {message.dimension}	
	generator: {message.generator}	
	gamemode: {message.gamemode}	
	difficulty: {message.difficulty}	
	hasAchievementsDisabled: {message.hasAchievementsDisabled}	
	dayCycleStopTime: {message.dayCycleStopTime}	
	eduMode: {message.eduMode}	
	rainLevel: {message.rainLevel}	
	lightnigLevel: {message.lightnigLevel}	
	enableCommands: {message.enableCommands}	
	isTexturepacksRequired: {message.isTexturepacksRequired}	
	secret: {message.levelId}	
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
			if (Log.IsDebugEnabled) Log.Debug($"\n{Package.HexDump(message.Bytes)}");
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

		private void OnFullChunkData(McpeFullChunkData msg)
		{
			if (IsEmulator) return;

			if (_chunks.TryAdd(new Tuple<int, int>(msg.chunkX, msg.chunkZ), true))
			{
				//Log.Debug($"Chunk X={msg.chunkX}, Z={msg.chunkZ}, size={msg.chunkData.Length}, Count={++_numberOfChunks}");

				try
				{
					ChunkColumn chunk = ClientUtils.DecocedChunkColumn(msg.chunkData);
					if (chunk != null)
					{
						chunk.x = msg.chunkX;
						chunk.z = msg.chunkZ;
						Log.DebugFormat("Chunk X={0}, Z={1}", chunk.x, chunk.z);
						foreach (KeyValuePair<BlockCoordinates, NbtCompound> blockEntity in chunk.BlockEntities)
						{
							Log.Debug($"Blockentity: {blockEntity.Value}");
						}

						//ClientUtils.SaveChunkToAnvil(chunk);
					}
				}
				catch (Exception e)
				{
					Log.Error("Reading chunk", e);
				}
			}
		}

		private void OnBatch(Package message)
		{
			FirstPacketWaitHandle.Set();

			McpeWrapper batch = (McpeWrapper) message;

			var messages = new List<Package>();


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
					byte[] internalBuffer = null;
					do
					{
						try
						{
							var len = VarInt.ReadUInt32(destination);
							internalBuffer = new byte[len];
							destination.Read(internalBuffer, 0, (int) len);

							if (internalBuffer[0] == 0x8e) throw new Exception("Wrong code, didn't expect a 0x8E in a batched packet");

							var package = PackageFactory.CreatePackage(internalBuffer[0], internalBuffer, "mcpe") ??
							              new UnknownPackage(internalBuffer[0], internalBuffer);
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
					PreserveReferencesHandling = PreserveReferencesHandling.Arrays,

					Formatting = Formatting.Indented,
				};
				jsonSerializerSettings.Converters.Add(new NbtIntConverter());
				jsonSerializerSettings.Converters.Add(new NbtStringConverter());

				string result = JsonConvert.SerializeObject(message, jsonSerializerSettings);
				Log.Debug($"> Receive: {message.Id} (0x{message.Id:x2}): {message.GetType().Name}\n{result}");
			}
			else if (verbosity == 2)
			{
				Log.Debug($"> Receive: {message.Id} (0x{message.Id:x2}): {message.GetType().Name}\n{Package.HexDump(message.Bytes)}");
			}
		}

		private void TraceSend(Package message)
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

			SendPackage(packet);
		}

		public void SendPackage(Package package)
		{
			SendPackage(package, _mtuSize, ref _reliableMessageNumber);
			package.PutPool();
		}

		public void SendNewIncomingConnection()
		{
			Random rand = new Random();
			var packet = NewIncomingConnection.CreateObject();
			packet.clientendpoint = _serverEndpoint;
			packet.systemAddresses = new IPEndPoint[20];
			for (int i = 0; i < 20; i++)
			{
				packet.systemAddresses[i] = new IPEndPoint(IPAddress.Any, 0);
			}

			SendPackage(packet);
		}

		public void SendChat(string text)
		{
			var packet = McpeText.CreateObject();
			packet.type = (byte) MessageType.Chat;
			packet.source = Username;
			packet.message = text;

			SendPackage(packet);
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

			SendPackage(movePlayerPacket);
		}


		public void SendDisconnectionNotification()
		{
			Session.CryptoContext = null;
			SendPackage(new DisconnectionNotification());
		}
	}
}