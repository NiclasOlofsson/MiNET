using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using MiNET.Net;
using MiNET.Utils;

namespace MiNET
{
	public class PlayerNetworkSession : INetworkHandler
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (PlayerNetworkSession));

		public object SyncRoot { get; private set; } = new object();
		public object EncodeSync { get; private set; } = new object();

		public MiNetServer Server { get; private set; }

		public string Username { get; set; }

		public IMcpeMessageHandler MessageHandler { get; set; }

		public IPEndPoint EndPoint { get; private set; }
		public short MtuSize { get; set; }

		private ConcurrentQueue<int> _playerAckQueue = new ConcurrentQueue<int>();
		private ConcurrentDictionary<int, Datagram> _waitingForAcksQueue = new ConcurrentDictionary<int, Datagram>();
		private ConcurrentDictionary<int, SplitPartPackage[]> _splits = new ConcurrentDictionary<int, SplitPartPackage[]>();
		public int DatagramSequenceNumber = -1;
		public int ReliableMessageNumber = 0;
		public int OrderingIndex = -1;
		public int ErrorCount { get; set; }

		public bool Evicted { get; set; }
		public ConnectionState State { get; set; }

		public DateTime LastUpdatedTime { get; set; }
		public bool WaitForAck { get; set; }
		public int ResendCount { get; set; }

		public long Rtt { get; set; } = 300;
		public long RttVar { get; set; }
		public long Rto { get; set; }

		public CryptoContext CryptoContext { get; set; }

		public PlayerNetworkSession(MiNetServer server, Player player, IPEndPoint endPoint, short mtuSize)
		{
			State = ConnectionState.Unconnected;

			Server = server;
			MessageHandler = player;
			EndPoint = endPoint;
			MtuSize = mtuSize;

			_cancellationToken = new CancellationTokenSource();
			_sendTicker = new Timer(SendQueue, null, 10, 10); // RakNet send tick-time
		}

		public ConcurrentDictionary<int, SplitPartPackage[]> Splits
		{
			get { return _splits; }
		}

		public ConcurrentQueue<int> PlayerAckQueue
		{
			get { return _playerAckQueue; }
		}

		public ConcurrentDictionary<int, Datagram> WaitingForAcksQueue
		{
			get { return _waitingForAcksQueue; }
		}

		public void Close()
		{

			PlayerNetworkSession session;
			if (!Server.ServerInfo.PlayerSessions.TryRemove(EndPoint, out session))
			{
				return;
			}

			if (PlayerAckQueue.Count > 0)
			{
				Thread.Sleep(50);
			}

			if (_sendTicker != null)
			{
				_sendTicker.Change(Timeout.Infinite, Timeout.Infinite);
				WaitHandle waitHandle = new AutoResetEvent(false);
				_sendTicker.Dispose(waitHandle);
				WaitHandle.WaitAll(new[] {waitHandle}, TimeSpan.FromMinutes(2));
				_sendTicker = null;
			}

			State = ConnectionState.Unconnected;
			Evicted = true;
			MessageHandler = null;

			SendQueue(null);

			_cancellationToken.Cancel();
			_waitEvent.Set();
			_mainWaitEvent.Set();
			_queue.Clear();

			var queue = WaitingForAcksQueue;
			foreach (var kvp in queue)
			{
				Datagram datagram;
				if (queue.TryRemove(kvp.Key, out datagram)) datagram.PutPool();
			}

			foreach (var kvp in Splits)
			{
				SplitPartPackage[] splitPartPackagese;
				if (Splits.TryRemove(kvp.Key, out splitPartPackagese))
				{
					if (splitPartPackagese == null) continue;

					foreach (SplitPartPackage package in splitPartPackagese)
					{
						if (package != null) package.PutPool();
					}
				}
			}

			queue.Clear();
			Splits.Clear();

			try
			{
				_processingThread = null;
				_cancellationToken.Dispose();
				_waitEvent.Close();
				_mainWaitEvent.Close();
			}
			catch (Exception e)
			{
			}

			if (Log.IsDebugEnabled) Log.Warn($"Closed network session for player {Username}");
		}

		private long _lastSequenceNumber = -1; // That's the first message with wrapper
		private AutoResetEvent _waitEvent = new AutoResetEvent(false);
		private AutoResetEvent _mainWaitEvent = new AutoResetEvent(false);
		private object _eventSync = new object();

		private ConcurrentPriorityQueue<int, Package> _queue = new ConcurrentPriorityQueue<int, Package>();
		private CancellationTokenSource _cancellationToken;
		private Thread _processingThread;

		public void AddToProcessing(Package message)
		{
			try
			{
				if (_cancellationToken.Token.IsCancellationRequested) return;

				bool forceOrder = Config.GetProperty("ForceOrderingForAll", false);

				if (!forceOrder)
				{
					if (CryptoContext == null || CryptoContext.UseEncryption == false)
					{
						_lastSequenceNumber = message.OrderingIndex;
						HandlePackage(message, this);
						return;
					}
				}

				lock (_eventSync)
				{
					if (_queue.Count == 0 && message.OrderingIndex == _lastSequenceNumber + 1)
					{
						_lastSequenceNumber = message.OrderingIndex;
						HandlePackage(message, this);
						return;
					}

					if (_processingThread == null)
					{
						_processingThread = new Thread(ProcessQueueThread) {IsBackground = true};
						_processingThread.Start();
						if (Log.IsDebugEnabled) Log.Warn($"Started processing thread for {Username}");
					}

					_queue.Enqueue(message.OrderingIndex, message);
					WaitHandle.SignalAndWait(_waitEvent, _mainWaitEvent);
				}
			}
			catch (Exception e)
			{
			}
		}

		private void ProcessQueueThread(object o)
		{
			ProcessQueue();
		}

		private Task ProcessQueue()
		{
			try
			{
				while (!_cancellationToken.IsCancellationRequested)
				{
					KeyValuePair<int, Package> pair;

					if (_queue.TryPeek(out pair))
					{
						if (pair.Key == _lastSequenceNumber + 1)
						{
							if (_queue.TryDequeue(out pair))
							{
								_lastSequenceNumber = pair.Key;

								HandlePackage(pair.Value, this);

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
			}
			catch (Exception e)
			{
			}

			//Log.Warn($"Exit receive handler task for {Player.Username}");
			return Task.CompletedTask;
		}

		internal void HandlePackage(Package message, PlayerNetworkSession playerSession)
		{
			if (message == null)
			{
				return;
			}

			if (typeof (McpeWrapper) == message.GetType())
			{
				McpeWrapper wrapper = (McpeWrapper) message;

				// Get bytes
				byte[] payload = wrapper.payload;
				if (playerSession.CryptoContext != null && playerSession.CryptoContext.UseEncryption)
				{
					payload = CryptoUtils.Decrypt(payload, playerSession.CryptoContext);
				}

				//if (Log.IsDebugEnabled)
				//	Log.Debug($"0x{payload[0]:x2}\n{Package.HexDump(payload)}");

				var msg = PackageFactory.CreatePackage(payload[0], payload, "mcpe") ?? new UnknownPackage(payload[0], payload);
				msg.DatagramSequenceNumber = wrapper.DatagramSequenceNumber;
				msg.Reliability = wrapper.Reliability;
				msg.ReliableMessageNumber = wrapper.ReliableMessageNumber;
				msg.OrderingChannel = wrapper.OrderingChannel;
				msg.OrderingIndex = wrapper.OrderingIndex;
				HandlePackage(msg, playerSession);

				message.PutPool();
				return;
			}

			if (typeof (UnknownPackage) == message.GetType())
			{
				UnknownPackage packet = (UnknownPackage) message;
				if (Log.IsDebugEnabled) Log.Warn($"Received unknown package 0x{message.Id:X2}\n{Package.HexDump(packet.Message)}");

				message.PutPool();
				return;
			}

			if (typeof (McpeBatch) == message.GetType())
			{
				Log.Debug("Handle MCPE batch message");
				McpeBatch batch = (McpeBatch) message;

				var messages = new List<Package>();

				// Get bytes
				byte[] payload = batch.payload.Array;
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

					while (destination.Position < destination.Length)
					{
						int len = reader.ReadInt32();
						byte[] internalBuffer = reader.ReadBytes(len);

						//if (Log.IsDebugEnabled)
						//	Log.Debug($"0x{internalBuffer[0]:x2}\n{Package.HexDump(internalBuffer)}");

						messages.Add(PackageFactory.CreatePackage(internalBuffer[0], internalBuffer, "mcpe") ?? new UnknownPackage(internalBuffer[0], internalBuffer));
					}

					if (destination.Length > destination.Position) throw new Exception("Have more data");
				}
				foreach (var msg in messages)
				{
					msg.DatagramSequenceNumber = batch.DatagramSequenceNumber;
					msg.Reliability = batch.Reliability;
					msg.ReliableMessageNumber = batch.ReliableMessageNumber;
					msg.OrderingChannel = batch.OrderingChannel;
					msg.OrderingIndex = batch.OrderingIndex;
					HandlePackage(msg, playerSession);
				}

				message.PutPool();
				return;
			}

			MiNetServer.TraceReceive(message, message.OrderingIndex);

			if (CryptoContext != null && CryptoContext.UseEncryption)
			{
				MiNetServer.FastThreadPool.QueueUserWorkItem(delegate()
				{
					HandlePackage(MessageHandler, message as Package);
					message.PutPool();
				});
			}
			else
			{
				HandlePackage(MessageHandler, message);
				message.PutPool();
			}
		}

		private void HandlePackage(IMcpeMessageHandler handler, Package message)
		{
			//var result = Server.PluginManager.PluginPacketHandler(message, true, Player);
			////if (result != message) message.PutPool();
			//message = result;

			if (message == null)
			{
				return;
			}

			else if (typeof (ConnectedPing) == message.GetType())
			{
				HandleConnectedPing((ConnectedPing) message);
			}

			else if (typeof (ConnectionRequest) == message.GetType())
			{
				HandleConnectionRequest((ConnectionRequest) message);
			}

			else if (typeof (NewIncomingConnection) == message.GetType())
			{
				HandleNewIncomingConnection((NewIncomingConnection) message);
			}

			else if (typeof (DisconnectionNotification) == message.GetType())
			{
				HandleDisconnectionNotification();
			}

			else if (typeof (McpeClientMagic) == message.GetType())
			{
				// Start encrypotion
				handler.HandleMcpeClientMagic((McpeClientMagic) message);
			}

			else if (typeof (McpeUpdateBlock) == message.GetType())
			{
				// DO NOT USE. Will dissapear from MCPE any release. 
				// It is a bug that it leaks these messages.
			}

			else if (typeof (McpeRemoveBlock) == message.GetType())
			{
				handler.HandleMcpeRemoveBlock((McpeRemoveBlock) message);
			}

			else if (typeof (McpeAnimate) == message.GetType())
			{
				handler.HandleMcpeAnimate((McpeAnimate) message);
			}

			else if (typeof (McpeUseItem) == message.GetType())
			{
				handler.HandleMcpeUseItem((McpeUseItem) message);
			}

			else if (typeof (McpeEntityEvent) == message.GetType())
			{
				handler.HandleMcpeEntityEvent((McpeEntityEvent) message);
			}

			else if (typeof (McpeText) == message.GetType())
			{
				handler.HandleMcpeText((McpeText) message);
			}

			else if (typeof (McpeRemoveEntity) == message.GetType())
			{
				// Do nothing right now, but should clear out the entities and stuff
				// from this players internal structure.
			}

			else if (typeof (McpeLogin) == message.GetType())
			{
				handler.HandleMcpeLogin((McpeLogin) message);
			}

			else if (typeof (McpeMovePlayer) == message.GetType())
			{
				handler.HandleMcpeMovePlayer((McpeMovePlayer) message);
			}

			else if (typeof (McpeInteract) == message.GetType())
			{
				handler.HandleMcpeInteract((McpeInteract) message);
			}

			else if (typeof (McpeRespawn) == message.GetType())
			{
				handler.HandleMcpeRespawn((McpeRespawn) message);
			}

			else if (typeof (McpeBlockEntityData) == message.GetType())
			{
				handler.HandleMcpeBlockEntityData((McpeBlockEntityData) message);
			}

			else if (typeof (McpePlayerAction) == message.GetType())
			{
				handler.HandleMcpePlayerAction((McpePlayerAction) message);
			}

			else if (typeof (McpeDropItem) == message.GetType())
			{
				handler.HandleMcpeDropItem((McpeDropItem) message);
			}

			else if (typeof (McpeContainerSetSlot) == message.GetType())
			{
				handler.HandleMcpeContainerSetSlot((McpeContainerSetSlot) message);
			}

			else if (typeof (McpeContainerClose) == message.GetType())
			{
				handler.HandleMcpeContainerClose((McpeContainerClose) message);
			}

			else if (typeof (McpeMobEquipment) == message.GetType())
			{
				handler.HandleMcpeMobEquipment((McpeMobEquipment) message);
			}

			else if (typeof (McpeMobArmorEquipment) == message.GetType())
			{
				handler.HandleMcpeMobArmorEquipment((McpeMobArmorEquipment) message);
			}

			else if (typeof (McpeCraftingEvent) == message.GetType())
			{
				handler.HandleMcpeCraftingEvent((McpeCraftingEvent) message);
			}

			else if (typeof (McpeRequestChunkRadius) == message.GetType())
			{
				handler.HandleMcpeRequestChunkRadius((McpeRequestChunkRadius) message);
			}

			else if (typeof (McpeMapInfoRequest) == message.GetType())
			{
				handler.HandleMcpeMapInfoRequest((McpeMapInfoRequest) message);
			}

			else if (typeof (McpeItemFramDropItem) == message.GetType())
			{
				handler.HandleMcpeItemFramDropItem((McpeItemFramDropItem) message);
			}

			else if (typeof (McpeItemFramDropItem) == message.GetType())
			{
				handler.HandleMcpePlayerInput((McpePlayerInput) message);
			}

			else
			{
				Log.Error($"Unhandled package: {message.GetType().Name} 0x{message.Id:X2} for user: {Username}, IP {EndPoint.Address}");
				return;
			}

			if (message.Timer.IsRunning)
			{
				long elapsedMilliseconds = message.Timer.ElapsedMilliseconds;
				if (elapsedMilliseconds > 1000)
				{
					Log.WarnFormat("Package (0x{1:x2}) handling too long {0}ms for {2}", elapsedMilliseconds, message.Id, Username);
				}
			}
			else
			{
				Log.WarnFormat("Package (0x{0:x2}) timer not started for {1}.", message.Id, Username);
			}
		}

		protected virtual void HandleConnectedPing(ConnectedPing message)
		{
			ConnectedPong package = ConnectedPong.CreateObject();
			package.NoBatch = true;
			package.ForceClear = true;
			package.sendpingtime = message.sendpingtime;
			package.sendpongtime = DateTimeOffset.UtcNow.Ticks/TimeSpan.TicksPerMillisecond;
			SendPackage(package);
		}

		protected virtual void HandleConnectionRequest(ConnectionRequest message)
		{
			Log.DebugFormat("Connection request from: {0}", EndPoint.Address);

			var response = ConnectionRequestAccepted.CreateObject();
			response.NoBatch = true;
			response.systemAddress = new IPEndPoint(IPAddress.Loopback, 19132);
			response.systemAddresses = new IPEndPoint[10];
			response.systemAddresses[0] = new IPEndPoint(IPAddress.Loopback, 19132);
			response.incomingTimestamp = message.timestamp;
			response.serverTimestamp = DateTime.UtcNow.Ticks/TimeSpan.TicksPerMillisecond;

			for (int i = 1; i < 10; i++)
			{
				response.systemAddresses[i] = new IPEndPoint(IPAddress.Any, 19132);
			}

			SendPackage(response);
		}

		protected virtual void HandleNewIncomingConnection(NewIncomingConnection message)
		{
			State = ConnectionState.Connected;
			Log.DebugFormat("New incoming connection from {0} {1}", EndPoint.Address, EndPoint.Port);
		}

		protected virtual void HandleDisconnectionNotification()
		{
			Disconnect("Client requested disconnected", false);
		}

		public virtual void Disconnect(string reason, bool sendDisconnect = true)
		{
			MessageHandler.Disconnect(reason, sendDisconnect);
		}

		public void DetectLostConnection()
		{
			DetectLostConnections ping = DetectLostConnections.CreateObject();
			ping.ForceClear = true;
			ping.NoBatch = true;
			SendPackage(ping);
		}

		// MCPE Login handling


		private Queue<Package> _sendQueueNotConcurrent = new Queue<Package>();
		private object _queueSync = new object();
		// ReSharper disable once NotAccessedField.Local
		private Timer _sendTicker;

		public void SendPackage(Package package)
		{
			if (package == null) return;

			if (State == ConnectionState.Unconnected)
			{
				package.PutPool();
				return;
			}

			bool isBatch = package is McpeBatch;

			if (!isBatch)
			{
				//var result = Server.PluginManager.PluginPacketHandler(package, false, Player);
				//if (result != package) package.PutPool();
				//package = result;

				//if (package == null) return;
			}

			lock (_queueSync)
			{
				_sendQueueNotConcurrent.Enqueue(package);
			}
		}

		private object _syncHack = new object();
		private MemoryStream memStream = new MemoryStream();

		private void SendQueue(object state)
		{
			if (!Monitor.TryEnter(_syncHack)) return;

			try
			{
				Queue<Package> queue = _sendQueueNotConcurrent;

				int messageCount = 0;

				int lenght = queue.Count;
				for (int i = 0; i < lenght; i++)
				{
					Package package = null;
					lock (_queueSync)
					{
						if (queue.Count == 0) break;
						try
						{
							package = queue.Dequeue();
						}
						catch (Exception e)
						{
						}
					}

					if (package == null) continue;

					if (State == ConnectionState.Unconnected)
					{
						package.PutPool();
						continue;
					}

					if (lenght == 1)
					{
						Server.SendPackage(this, package);
					}
					else if (package is McpeBatch)
					{
						SendBuffered(messageCount);
						messageCount = 0;
						Server.SendPackage(this, package);
						Thread.Sleep(1); // Really important to slow down speed a bit
					}
					else if (package.NoBatch)
					{
						SendBuffered(messageCount);
						messageCount = 0;
						Server.SendPackage(this, package);
					}
					//else if (!IsSpawned)
					//{
					//	SendBuffered(messageCount);
					//	messageCount = 0;
					//	Server.SendPackage(this, package);
					//}
					else
					{
						if (messageCount == 0)
						{
							memStream.Position = 0;
							memStream.SetLength(0);
						}

						byte[] bytes = package.Encode();
						if (bytes != null)
						{
							messageCount++;
							memStream.Write(BitConverter.GetBytes(Endian.SwapInt32(bytes.Length)), 0, 4);
							memStream.Write(bytes, 0, bytes.Length);
						}

						package.PutPool();
					}
				}

				if (State == ConnectionState.Unconnected)
				{
					return;
				}

				SendBuffered(messageCount);
			}
			finally
			{
				Monitor.Exit(_syncHack);
			}
		}

		private void SendBuffered(int messageCount)
		{
			if (messageCount == 0) return;

			//byte[] bufferNoComp = CompressBytes(array, CompressionLevel.NoCompression);
			//byte[] bufferOptimal = CompressBytes(array, CompressionLevel.Optimal);
			var batch = Player.CreateBatchPacket(memStream.GetBuffer(), 0, (int) memStream.Length, CompressionLevel.Fastest);
			
			memStream.Position = 0;
			memStream.SetLength(0);

			Server.SendPackage(this, batch);
		}

		public void SendDirectPackage(Package package)
		{
			Server.SendPackage(this, package);
		}
	}

	public class PlayerInfo
	{
		public CertificateData CertificateData { get; set; }
		public string Username { get; set; }
		public UUID ClientUuid { get; set; }
		public string ServerAddress { get; set; }
		public long ClientId { get; set; }
		public Skin Skin { get; set; }
	}
}