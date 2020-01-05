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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using MiNET.Net;
using MiNET.Utils;
using MiNET.Utils.Skins;

namespace MiNET
{
	public class PlayerNetworkSession : INetworkHandler
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(PlayerNetworkSession));

		public object SyncRoot { get; private set; } = new object();
		public object EncodeSync { get; private set; } = new object();

		public MiNetServer Server { get; private set; }

		public string Username { get; set; }

		public IMcpeMessageHandler MessageHandler { get; set; }

		public IPEndPoint EndPoint { get; private set; }
		public short MtuSize { get; set; }
		public long NetworkIdentifier { get; set; }

		public int DatagramSequenceNumber = -1;
		public int ReliableMessageNumber = -1;
		public int SplitPartId = 0;
		public int OrderingIndex = -1;
		public int ErrorCount { get; set; }

		public bool Evicted { get; set; }

		public Random Random { get; } = new Random();

		public ConnectionState State { get; set; }

		public DateTime LastUpdatedTime { get; set; }
		public bool WaitForAck { get; set; }
		public int ResendCount { get; set; }

		public long Rtt { get; set; } = 300;
		public long RttVar { get; set; }
		public long Rto { get; set; }

		public ConcurrentDictionary<int, SplitPartPacket[]> Splits { get; } = new ConcurrentDictionary<int, SplitPartPacket[]>();
		public ConcurrentQueue<int> PlayerAckQueue { get; } = new ConcurrentQueue<int>();
		public ConcurrentDictionary<int, Datagram> WaitingForAcksQueue { get; } = new ConcurrentDictionary<int, Datagram>();

		public CryptoContext CryptoContext { get; set; }

		public PlayerNetworkSession(MiNetServer server, Player player, IPEndPoint endPoint, short mtuSize)
		{
			State = ConnectionState.Unconnected;

			Server = server;
			MessageHandler = player;
			EndPoint = endPoint;
			MtuSize = mtuSize;

			_cancellationToken = new CancellationTokenSource();
			//_tickerHighPrecisionTimer = new HighPrecisionTimer(10, SendTick, true);
		}

		public void Close()
		{
			if (Server == null) // EMULATOR
			{
				if (_tickerHighPrecisionTimer != null)
				{
					_tickerHighPrecisionTimer.Dispose();
				}

				return;
			}

			PlayerNetworkSession session;
			if (!Server.ServerInfo.PlayerSessions.TryRemove(EndPoint, out session))
			{
				return;
			}

			if (PlayerAckQueue.Count > 0)
			{
				Thread.Sleep(50);
			}

			if (_tickerHighPrecisionTimer != null)
			{
				_tickerHighPrecisionTimer.Dispose();
			}

			State = ConnectionState.Unconnected;
			Evicted = true;
			MessageHandler = null;

			SendQueue();

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
				SplitPartPacket[] splitPartPackets;
				if (Splits.TryRemove(kvp.Key, out splitPartPackets))
				{
					if (splitPartPackets == null) continue;

					foreach (SplitPartPacket packet in splitPartPackets)
					{
						if (packet != null) packet.PutPool();
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

		private ConcurrentPriorityQueue<int, Packet> _queue = new ConcurrentPriorityQueue<int, Packet>();
		private CancellationTokenSource _cancellationToken;
		private Thread _processingThread;

		public void AddToProcessing(Packet message)
		{
			try
			{
				if (_cancellationToken.Token.IsCancellationRequested) return;

				bool forceOrder = Server.ForceOrderingForAll;

				if (!forceOrder)
				{
					if (CryptoContext == null || CryptoContext.UseEncryption == false)
					{
						_lastSequenceNumber = message.OrderingIndex;
						HandlePacket(message, this);
						return;
					}
				}

				lock (_eventSync)
				{
					if (_queue.Count == 0 && message.OrderingIndex == _lastSequenceNumber + 1)
					{
						_lastSequenceNumber = message.OrderingIndex;
						HandlePacket(message, this);
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
					KeyValuePair<int, Packet> pair;

					if (_queue.TryPeek(out pair))
					{
						if (pair.Key == _lastSequenceNumber + 1)
						{
							if (_queue.TryDequeue(out pair))
							{
								_lastSequenceNumber = pair.Key;

								HandlePacket(pair.Value, this);

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
				Log.Error($"Exit receive handler task for player", e);
			}

			return Task.CompletedTask;
		}

		internal void HandlePacket(Packet message, PlayerNetworkSession playerSession)
		{
			//SignalTick();

			try
			{
				if (message == null)
				{
					return;
				}

				if (typeof(UnknownPacket) == message.GetType())
				{
					UnknownPacket packet = (UnknownPacket) message;
					if (Log.IsDebugEnabled) Log.Warn($"Received unknown packet 0x{message.Id:X2}\n{Packet.HexDump(packet.Message)}");

					message.PutPool();
					return;
				}

				if (typeof(McpeWrapper) == message.GetType())
				{
					McpeWrapper batch = (McpeWrapper) message;
					var messages = new List<Packet>();

					// Get bytes
					byte[] payload = batch.payload;
					if (playerSession.CryptoContext != null && playerSession.CryptoContext.UseEncryption)
					{
						payload = CryptoUtils.Decrypt(payload, playerSession.CryptoContext);
					}


					// Decompress bytes

					MemoryStream stream = new MemoryStream(payload);
					if (stream.ReadByte() != 0x78)
					{
						if (Log.IsDebugEnabled) Log.Error($"Incorrect ZLib header. Expected 0x78 0x9C 0x{message.Id:X2}\n{Packet.HexDump(batch.payload)}");
						if (Log.IsDebugEnabled) Log.Error($"Incorrect ZLib header. Decrypted 0x{message.Id:X2}\n{Packet.HexDump(payload)}");
						throw new InvalidDataException("Incorrect ZLib header. Expected 0x78 0x9C");
					}
					stream.ReadByte();
					using (var deflateStream = new DeflateStream(stream, CompressionMode.Decompress, false))
					{
						// Get actual packet out of bytes
						using (MemoryStream destination = MiNetServer.MemoryStreamManager.GetStream())
						{
							deflateStream.CopyTo(destination);
							destination.Position = 0;

							while (destination.Position < destination.Length)
							{
								int len = (int) VarInt.ReadUInt32(destination);
								long pos = destination.Position;
								int id = (int) VarInt.ReadUInt32(destination);
								len = (int) (len - (destination.Position - pos)); // calculate len of buffer after varint
								byte[] internalBuffer = new byte[len];
								destination.Read(internalBuffer, 0, len);

								//if (Log.IsDebugEnabled)
								//	Log.Debug($"0x{internalBuffer[0]:x2}\n{Packet.HexDump(internalBuffer)}");

								try
								{
									messages.Add(PacketFactory.Create((byte) id, internalBuffer, "mcpe") ??
												new UnknownPacket((byte) id, internalBuffer));
								}
								catch (Exception e)
								{
									if (Log.IsDebugEnabled) Log.Warn($"Error parsing packet 0x{message.Id:X2}\n{Packet.HexDump(internalBuffer)}");

									throw;
								}
							}

							if (destination.Length > destination.Position) throw new Exception("Have more data");
						}
					}
					foreach (var msg in messages)
					{
						// Temp fix for performance, take 1.
						var interact = msg as McpeInteract;
						if (interact?.actionId == 4 && interact.targetRuntimeEntityId == 0) continue;

						msg.DatagramSequenceNumber = batch.DatagramSequenceNumber;
						msg.Reliability = batch.Reliability;
						msg.ReliableMessageNumber = batch.ReliableMessageNumber;
						msg.OrderingChannel = batch.OrderingChannel;
						msg.OrderingIndex = batch.OrderingIndex;
						HandlePacket(msg, playerSession);
					}

					message.PutPool();
					return;
				}

				MiNetServer.TraceReceive(message);

				// This needs rework. Some packets (transaction) really require the correct order to be maintained 
				// in order to process correctly. I was thinking perhaps marking some packets as "order important" and
				// check that flag here. But for now, I'm disabling the threading here. Not even sure why it is here.

				//if (CryptoContext != null && CryptoContext.UseEncryption)
				//{
				//	MiNetServer.FastThreadPool.QueueUserWorkItem(() =>
				//	{
				//		HandlePacket(MessageHandler, message as Packet);
				//		message.PutPool();
				//	});
				//}
				//else
				{
					HandlePacket(MessageHandler, message);
					message.PutPool();
				}
			}
			catch (Exception e)
			{
				Log.Error("Packet handling", e);
				throw;
			}
		}

		private void HandlePacket(IMcpeMessageHandler handler, Packet message)
		{
			if (handler is Player)
			{
				var result = Server.PluginManager.PluginPacketHandler(message, true, (Player) handler);
				if (result != message) message.PutPool();
				message = result;
			}

			if (message == null)
			{
				return;
			}

			else if (typeof(ConnectedPing) == message.GetType())
			{
				HandleConnectedPing((ConnectedPing) message);
			}

			else if (typeof(ConnectionRequest) == message.GetType())
			{
				HandleConnectionRequest((ConnectionRequest) message);
			}

			else if (typeof(NewIncomingConnection) == message.GetType())
			{
				HandleNewIncomingConnection((NewIncomingConnection) message);
			}

			else if (typeof(DisconnectionNotification) == message.GetType())
			{
				HandleDisconnectionNotification();
			}

			else if (typeof(McpeClientToServerHandshake) == message.GetType())
			{
				// Start encryption
				handler.HandleMcpeClientToServerHandshake((McpeClientToServerHandshake) message);
			}

			else if (typeof(McpeResourcePackClientResponse) == message.GetType())
			{
				handler.HandleMcpeResourcePackClientResponse((McpeResourcePackClientResponse) message);
			}

			else if (typeof(McpeResourcePackChunkRequest) == message.GetType())
			{
				handler.HandleMcpeResourcePackChunkRequest((McpeResourcePackChunkRequest) message);
			}

			else if (typeof(McpeSetLocalPlayerAsInitializedPacket) == message.GetType())
			{
				handler.HandleMcpeSetLocalPlayerAsInitializedPacket((McpeSetLocalPlayerAsInitializedPacket) message);
			}

			else if (typeof(McpeScriptCustomEventPacket) == message.GetType())
			{
				handler.HandleMcpeScriptCustomEventPacket((McpeScriptCustomEventPacket) message);
			}

			else if (typeof(McpeUpdateBlock) == message.GetType())
			{
				// DO NOT USE. Will dissapear from MCPE any release. 
				// It is a bug that it leaks these messages.
			}

			else if (typeof(McpeLevelSoundEvent) == message.GetType())
			{
				handler.HandleMcpeLevelSoundEvent((McpeLevelSoundEvent) message);
			}

			else if (typeof(McpeClientCacheStatus) == message.GetType())
			{
				handler.HandleMcpeClientCacheStatus((McpeClientCacheStatus) message);
			}

			else if (typeof(McpeAnimate) == message.GetType())
			{
				handler.HandleMcpeAnimate((McpeAnimate) message);
			}

			else if (typeof(McpeEntityFall) == message.GetType())
			{
				handler.HandleMcpeEntityFall((McpeEntityFall) message);
			}

			else if (typeof(McpeEntityEvent) == message.GetType())
			{
				handler.HandleMcpeEntityEvent((McpeEntityEvent) message);
			}

			else if (typeof(McpeText) == message.GetType())
			{
				handler.HandleMcpeText((McpeText) message);
			}

			else if (typeof(McpeRemoveEntity) == message.GetType())
			{
				// Do nothing right now, but should clear out the entities and stuff
				// from this players internal structure.
			}

			else if (typeof(McpeLogin) == message.GetType())
			{
				handler.HandleMcpeLogin((McpeLogin) message);
			}

			else if (typeof(McpeMovePlayer) == message.GetType())
			{
				handler.HandleMcpeMovePlayer((McpeMovePlayer) message);
			}

			else if (typeof(McpeInteract) == message.GetType())
			{
				handler.HandleMcpeInteract((McpeInteract) message);
			}

			else if (typeof(McpeRespawn) == message.GetType())
			{
				handler.HandleMcpeRespawn((McpeRespawn) message);
			}

			else if (typeof(McpeBlockEntityData) == message.GetType())
			{
				handler.HandleMcpeBlockEntityData((McpeBlockEntityData) message);
			}

			else if (typeof(McpeAdventureSettings) == message.GetType())
			{
				handler.HandleMcpeAdventureSettings((McpeAdventureSettings) message);
			}

			else if (typeof(McpePlayerAction) == message.GetType())
			{
				handler.HandleMcpePlayerAction((McpePlayerAction) message);
			}

			else if (typeof(McpeContainerClose) == message.GetType())
			{
				handler.HandleMcpeContainerClose((McpeContainerClose) message);
			}

			else if (typeof(McpeMobEquipment) == message.GetType())
			{
				handler.HandleMcpeMobEquipment((McpeMobEquipment) message);
			}

			else if (typeof(McpeMobArmorEquipment) == message.GetType())
			{
				handler.HandleMcpeMobArmorEquipment((McpeMobArmorEquipment) message);
			}

			else if (typeof(McpeCraftingEvent) == message.GetType())
			{
				handler.HandleMcpeCraftingEvent((McpeCraftingEvent) message);
			}

			else if (typeof(McpeInventoryTransaction) == message.GetType())
			{
				handler.HandleMcpeInventoryTransaction((McpeInventoryTransaction) message);
			}

			else if (typeof(McpeServerSettingsRequest) == message.GetType())
			{
				handler.HandleMcpeServerSettingsRequest((McpeServerSettingsRequest) message);
			}

			else if (typeof(McpeSetPlayerGameType) == message.GetType())
			{
				handler.HandleMcpeSetPlayerGameType((McpeSetPlayerGameType) message);
			}

			else if (typeof(McpePlayerHotbar) == message.GetType())
			{
				handler.HandleMcpePlayerHotbar((McpePlayerHotbar) message);
			}

			else if (typeof(McpeInventoryContent) == message.GetType())
			{
				handler.HandleMcpeInventoryContent((McpeInventoryContent) message);
			}

			else if (typeof(McpeRequestChunkRadius) == message.GetType())
			{
				handler.HandleMcpeRequestChunkRadius((McpeRequestChunkRadius) message);
			}

			else if (typeof(McpeMapInfoRequest) == message.GetType())
			{
				handler.HandleMcpeMapInfoRequest((McpeMapInfoRequest) message);
			}

			else if (typeof(McpeItemFrameDropItem) == message.GetType())
			{
				handler.HandleMcpeItemFrameDropItem((McpeItemFrameDropItem) message);
			}

			else if (typeof(McpePlayerInput) == message.GetType())
			{
				handler.HandleMcpePlayerInput((McpePlayerInput) message);
			}

			else if (typeof(McpeRiderJump) == message.GetType())
			{
				handler.HandleMcpeRiderJump((McpeRiderJump) message);
			}

			else if (typeof(McpeCommandRequest) == message.GetType())
			{
				handler.HandleMcpeCommandRequest((McpeCommandRequest) message);
			}

			else if (typeof(McpeBlockPickRequest) == message.GetType())
			{
				handler.HandleMcpeBlockPickRequest((McpeBlockPickRequest) message);
			}

			else if (typeof(McpeEntityPickRequest) == message.GetType())
			{
				handler.HandleMcpeEntityPickRequest((McpeEntityPickRequest) message);
			}

			else if (typeof(McpeModalFormResponse) == message.GetType())
			{
				handler.HandleMcpeModalFormResponse((McpeModalFormResponse) message);
			}
			else if (typeof(McpeCommandBlockUpdate) == message.GetType())
			{
				handler.HandleMcpeCommandBlockUpdate((McpeCommandBlockUpdate) message);
			}

			else if (typeof(McpeEntityPickRequest) == message.GetType())
			{
				handler.HandleMcpeEntityPickRequest((McpeEntityPickRequest) message);
			}

			else if (typeof(McpeMoveEntity) == message.GetType())
			{
				handler.HandleMcpeMoveEntity((McpeMoveEntity) message);
			}

			else if (typeof(McpeSetEntityMotion) == message.GetType())
			{
				handler.HandleMcpeSetEntityMotion((McpeSetEntityMotion) message);
			}

			else if (typeof(McpePhotoTransfer) == message.GetType())
			{
				handler.HandleMcpePhotoTransfer((McpePhotoTransfer) message);
			}

			else if (typeof(McpeSetEntityData) == message.GetType())
			{
				handler.HandleMcpeSetEntityData((McpeSetEntityData) message);
			}

			else if (typeof(McpeTickSync) == message.GetType())
			{
				handler.HandleMcpeTickSync((McpeTickSync) message);
			}

			else if (typeof(McpeNpcRequest) == message.GetType())
			{
				handler.HandleMcpeNpcRequest((McpeNpcRequest) message);
			}

			else if (typeof(McpeNetworkStackLatencyPacket) == message.GetType())
			{
				handler.HandleMcpeNetworkStackLatencyPacket((McpeNetworkStackLatencyPacket) message);
			}

			else
			{
				Log.Error($"Unhandled packet: {message.GetType().Name} 0x{message.Id:X2} for user: {Username}, IP {EndPoint.Address}");
				if (Log.IsDebugEnabled) Log.Warn($"Unknown packet 0x{message.Id:X2}\n{Packet.HexDump(message.Bytes)}");
				return;
			}

			if (message.Timer.IsRunning)
			{
				long elapsedMilliseconds = message.Timer.ElapsedMilliseconds;
				if (elapsedMilliseconds > 1000)
				{
					Log.WarnFormat("Packet (0x{1:x2}) handling too long {0}ms for {2}", elapsedMilliseconds, message.Id, Username);
				}
			}
			else
			{
				Log.WarnFormat("Packet (0x{0:x2}) timer not started for {1}.", message.Id, Username);
			}
		}

		protected virtual void HandleConnectedPing(ConnectedPing message)
		{
			ConnectedPong packet = ConnectedPong.CreateObject();
			packet.NoBatch = true;
			packet.ForceClear = true;
			packet.sendpingtime = message.sendpingtime;
			packet.sendpongtime = DateTimeOffset.UtcNow.Ticks / TimeSpan.TicksPerMillisecond;
			SendPacket(packet);
		}

		protected virtual void HandleConnectionRequest(ConnectionRequest message)
		{
			Log.DebugFormat("Connection request from: {0}", EndPoint.Address);

			var response = ConnectionRequestAccepted.CreateObject();
			response.NoBatch = true;
			response.systemAddress = new IPEndPoint(IPAddress.Loopback, 19132);
			response.systemAddresses = new IPEndPoint[20];
			response.systemAddresses[0] = new IPEndPoint(IPAddress.Loopback, 19132);
			response.incomingTimestamp = message.timestamp;
			response.serverTimestamp = DateTime.UtcNow.Ticks / TimeSpan.TicksPerMillisecond;

			for (int i = 1; i < 20; i++)
			{
				response.systemAddresses[i] = new IPEndPoint(IPAddress.Any, 19132);
			}

			SendPacket(response);
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
			MessageHandler?.Disconnect(reason, sendDisconnect);
			Close();
		}

		public void DetectLostConnection()
		{
			DetectLostConnections ping = DetectLostConnections.CreateObject();
			ping.ForceClear = true;
			ping.NoBatch = true;
			SendPacket(ping);
		}

		// MCPE Login handling


		private Queue<Packet> _sendQueueNotConcurrent = new Queue<Packet>();
		private object _queueSync = new object();

		public void SendPacket(Packet packet)
		{
			if (packet == null) return;

			if (State == ConnectionState.Unconnected)
			{
				packet.PutPool();
				return;
			}

			MiNetServer.TraceSend(packet);

			bool isBatch = packet is McpeWrapper;

			if (!isBatch)
			{
				//var result = Server.PluginManager.PluginPacketHandler(packet, false, Player);
				//if (result != packet) packet.PutPool();
				//packet = result;

				//if (packet == null) return;
			}

			lock (_queueSync)
			{
				_sendQueueNotConcurrent.Enqueue(packet);
				SignalTick();
			}
		}

		private int i = 0;

		public void SendTick(object state)
		{
			try
			{
				SendAckQueue();
				SendQueue();
				if (i++ >= 5)
				{
					Update();
					i = 0;
				}
			}
			catch (Exception e)
			{
				Log.Error(e);
			}
		}


		private object _updateSync = new object();
		private Stopwatch _forceQuitTimer = new Stopwatch();

		private void Update()
		{
			if (Server == null) return; // MiNET Client

			if (Evicted) return;

			if (MiNetServer.FastThreadPool == null) return;

			if (!Monitor.TryEnter(_updateSync)) return;

			try
			{
				_forceQuitTimer.Restart();

				if (Evicted) return;

				long now = DateTime.UtcNow.Ticks / TimeSpan.TicksPerMillisecond;

				long lastUpdate = LastUpdatedTime.Ticks / TimeSpan.TicksPerMillisecond;

				if (lastUpdate + Server.InacvitityTimeout + 3000 < now)
				{
					Evicted = true;
					// Disconnect user
					MiNetServer.FastThreadPool.QueueUserWorkItem(() =>
					{
						Disconnect("You've been kicked with reason: Network timeout.");
						Close();
					});

					return;
				}


				if (State != ConnectionState.Connected && MessageHandler != null && lastUpdate + 3000 < now)
				{
					MiNetServer.FastThreadPool.QueueUserWorkItem(() => { Disconnect("You've been kicked with reason: Lost connection."); });

					return;
				}

				if (MessageHandler == null) return;

				if (!WaitForAck && (ResendCount > Server.ResendThreshold || lastUpdate + Server.InacvitityTimeout < now))
				{
					//TODO: Seems to have lost code here. This should actually count the resends too.
					// Spam is a bit too much. The Russians have trouble with bad connections.
					DetectLostConnection();
					WaitForAck = true;
				}

				if (WaitingForAcksQueue.Count == 0) return;

				if (WaitForAck) return;

				if (Rto == 0) return;

				long rto = Math.Max(100, Rto);
				var queue = WaitingForAcksQueue;

				foreach (KeyValuePair<int, Datagram> datagramPair in queue)
				{
					if (Evicted) return;

					var datagram = datagramPair.Value;

					if (!datagram.Timer.IsRunning)
					{
						Log.ErrorFormat("Timer not running for #{0}", datagram.Header.datagramSequenceNumber);
						datagram.Timer.Restart();
						continue;
					}

					if (Rtt == -1) return;

					long elapsedTime = datagram.Timer.ElapsedMilliseconds;
					long datagramTimout = rto * (datagram.TransmissionCount + ResendCount + 1);
					datagramTimout = Math.Min(datagramTimout, 3000);
					datagramTimout = Math.Max(datagramTimout, 100);

					if (datagram.RetransmitImmediate || elapsedTime >= datagramTimout)
					{
						if (!Evicted && WaitingForAcksQueue.TryRemove(datagram.Header.datagramSequenceNumber, out var deleted))
						{
							ErrorCount++;
							ResendCount++;

							MiNetServer.FastThreadPool.QueueUserWorkItem(() =>
							{
								var dtgram = deleted;
								if (Log.IsDebugEnabled)
								{
									Log.Warn($"{(deleted.RetransmitImmediate ? "NAK RSND" : "TIMEOUT")}, Resent #{deleted.Header.datagramSequenceNumber.IntValue()} Type: {deleted.FirstMessageId} (0x{deleted.FirstMessageId:x2}) for {Username} ({elapsedTime} > {datagramTimout}) RTO {Rto}");
								}
								Server.SendDatagram(this, (Datagram) dtgram);
								Interlocked.Increment(ref Server.ServerInfo.NumberOfResends);
							});
						}
					}
				}
			}
			finally
			{
				if (_forceQuitTimer.ElapsedMilliseconds > 100)
				{
					Log.Warn($"Update took unexpected long time={_forceQuitTimer.ElapsedMilliseconds}, Count={WaitingForAcksQueue.Count}");
				}

				Monitor.Exit(_updateSync);
			}
		}

		public void SignalTick()
		{
			try
			{
				_tickerHighPrecisionTimer?.AutoReset?.Set();
			}
			catch (ObjectDisposedException)
			{
			}
		}


		private void SendAckQueue()
		{
			PlayerNetworkSession session = this;
			var queue = session.PlayerAckQueue;
			int lenght = queue.Count;

			if (lenght == 0) return;

			Acks acks = Acks.CreateObject();
			for (int i = 0; i < lenght; i++)
			{
				if (!queue.TryDequeue(out var ack)) break;

				Interlocked.Increment(ref Server.ServerInfo.NumberOfAckSent);
				acks.acks.Add(ack);
			}

			if (acks.acks.Count > 0)
			{
				byte[] data = acks.Encode();
				Server.SendData(data, session.EndPoint);
			}

			acks.PutPool();
		}

		private object _syncHack = new object();
		private HighPrecisionTimer _tickerHighPrecisionTimer;

		public void SendQueue()
		{
			if (_sendQueueNotConcurrent.Count == 0) return;

			if (!Monitor.TryEnter(_syncHack)) return;

			try
			{
				using (MemoryStream memStream = new MemoryStream())
				{
					Queue<Packet> queue = _sendQueueNotConcurrent;

					int messageCount = 0;

					int lenght = queue.Count;
					for (int i = 0; i < lenght; i++)
					{
						Packet packet = null;
						lock (_queueSync)
						{
							if (queue.Count == 0) break;
							try
							{
								packet = queue.Dequeue();
							}
							catch (Exception e)
							{
							}
						}

						if (packet == null) continue;

						if (State == ConnectionState.Unconnected)
						{
							packet.PutPool();
							continue;
						}

						if (lenght == 1)
						{
							Server.SendPacket(this, packet);
						}
						else if (packet is McpeWrapper)
						{
							SendBuffered(messageCount, memStream);
							messageCount = 0;
							Server.SendPacket(this, packet);
							// The following is necessary if no throttling is done on chunk sending,
							// but having it creates a lot of packet lag when using SpawnLevel. You can see it
							// as players standing still, but having running particles.
							//
							//Thread.Sleep(1); // Really important to slow down speed a bit
						}
						else if (packet.NoBatch)
						{
							SendBuffered(messageCount, memStream);
							messageCount = 0;
							Server.SendPacket(this, packet);
						}
						else
						{
							if (messageCount == 0)
							{
								memStream.Position = 0;
								memStream.SetLength(0);
							}

							byte[] bytes = packet.Encode();
							if (bytes != null)
							{
								messageCount++;
								BatchUtils.WriteLength(memStream, bytes.Length);
								//memStream.Write(BitConverter.GetBytes(Endian.SwapInt32(bytes.Length)), 0, 4);
								memStream.Write(bytes, 0, bytes.Length);
							}

							packet.PutPool();
						}
					}

					if (State == ConnectionState.Unconnected)
					{
						return;
					}

					SendBuffered(messageCount, memStream);
				}
			}
			finally
			{
				Monitor.Exit(_syncHack);
			}
		}

		private void SendBuffered(int messageCount, MemoryStream memStream)
		{
			if (messageCount == 0) return;

			var batch = BatchUtils.CreateBatchPacket(new Memory<byte>(memStream.GetBuffer(), 0, (int) memStream.Length), CompressionLevel.Fastest, false);
			batch.Encode();
			memStream.Position = 0;
			memStream.SetLength(0);

			Server.SendPacket(this, batch);
		}

		public void SendDirectPacket(Packet packet)
		{
			Server.SendPacket(this, packet);
		}

		public IPEndPoint GetClientEndPoint()
		{
			return EndPoint;
		}

		public long GetNetworkNetworkIdentifier()
		{
			return NetworkIdentifier;
		}
	}

	public class PlayerInfo
	{
		public int ADRole { get; set; }
		public CertificateData CertificateData { get; set; }
		public string Username { get; set; }
		public UUID ClientUuid { get; set; }
		public string ServerAddress { get; set; }
		public long ClientId { get; set; }
		public Skin Skin { get; set; }
		public int CurrentInputMode { get; set; }
		public int DefaultInputMode { get; set; }
		public string DeviceModel { get; set; }
		public string GameVersion { get; set; }
		public int DeviceOS { get; set; }
		public string DeviceId { get; set; }
		public int GuiScale { get; set; }
		public int UIProfile { get; set; }
		public int Edition { get; set; }
		public int ProtocolVersion { get; set; }
		public string LanguageCode { get; set; }
		public string PlatformChatId { get; set; }
		public string ThirdPartyName { get; set; }
		public string TenantId { get; set; }
	}
}