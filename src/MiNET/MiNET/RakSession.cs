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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2020 Niclas Olofsson.
// All Rights Reserved.

#endregion

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using MiNET.Net;
using MiNET.Net.RakNet;
using MiNET.Utils;
using MiNET.Utils.Skins;

namespace MiNET
{
	public class RakSession : INetworkHandler
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(RakSession));

		public object SyncRoot { get; private set; } = new object();
		public object EncodeSync { get; private set; } = new object();

		public MiNetServer Server { get; private set; }

		public IMcpeMessageHandler MessageHandler { get; set; }

		public string Username { get; set; }
		public CryptoContext CryptoContext { get; set; }
		public IPEndPoint EndPoint { get; private set; }
		public short MtuSize { get; set; }
		public long NetworkIdentifier { get; set; }

		public int DatagramSequenceNumber = -1;
		public int ReliableMessageNumber = -1;
		public int SplitPartId = 0;
		public int OrderingIndex = -1;
		public int ErrorCount { get; set; }

		public bool Evicted { get; set; }

		public ConnectionState State { get; set; }

		public DateTime LastUpdatedTime { get; set; }
		public bool WaitForAck { get; set; }
		public int ResendCount { get; set; }

		/// <summary>
		/// </summary>
		public long Syn { get; set; } = 300;

		/// <summary>
		///     Round Trip Time.
		///     <code>RTT = RTT * 0.875 + rtt * 0.125</code>
		/// </summary>
		public long Rtt { get; set; } = 300;

		/// <summary>
		///     Round Trip Time Variance.
		///     <code>RTTVar = RTTVar * 0.875 + abs(RTT - rtt)) * 0.125</code>
		/// </summary>
		public long RttVar { get; set; }

		/// <summary>
		///     Retransmission Time Out.
		///     <code>RTO = RTT + 4 * RTTVar</code>
		/// </summary>
		public long Rto { get; set; }

		public ConcurrentDictionary<int, SplitPartPacket[]> Splits { get; } = new ConcurrentDictionary<int, SplitPartPacket[]>();
		public ConcurrentQueue<int> OutgoingAckQueue { get; } = new ConcurrentQueue<int>();
		public ConcurrentDictionary<int, Datagram> WaitingForAckQueue { get; } = new ConcurrentDictionary<int, Datagram>();

		public RakSession(MiNetServer server, Player player, IPEndPoint endPoint, short mtuSize)
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
				_tickerHighPrecisionTimer?.Dispose();

				return;
			}

			if (!Server.ServerInfo.PlayerSessions.TryRemove(EndPoint, out RakSession session))
			{
				return;
			}

			if (OutgoingAckQueue.Count > 0)
			{
				Thread.Sleep(50);
			}

			_tickerHighPrecisionTimer?.Dispose();

			State = ConnectionState.Unconnected;
			Evicted = true;
			MessageHandler = null;

			SendQueueAsync().Wait();

			_cancellationToken.Cancel();
			_waitEvent.Set();
			_mainWaitEvent.Set();
			_queue.Clear();

			var queue = WaitingForAckQueue;
			foreach (var kvp in queue)
			{
				if (queue.TryRemove(kvp.Key, out Datagram datagram)) datagram.PutPool();
			}

			foreach (var kvp in Splits)
			{
				if (Splits.TryRemove(kvp.Key, out SplitPartPacket[] splitPartPackets))
				{
					if (splitPartPackets == null) continue;

					foreach (SplitPartPacket packet in splitPartPackets)
					{
						packet?.PutPool();
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
			catch
			{
				// ignored
			}

			if (Log.IsDebugEnabled) Log.Warn($"Closed network session for player {Username}");
		}

		private long _lastOrderingIndex = -1; // That's the first message with wrapper
		private AutoResetEvent _waitEvent = new AutoResetEvent(false);
		private AutoResetEvent _mainWaitEvent = new AutoResetEvent(false);
		private object _eventSync = new object();

		private ConcurrentPriorityQueue<int, Packet> _queue = new ConcurrentPriorityQueue<int, Packet>();
		private CancellationTokenSource _cancellationToken;
		private Thread _processingThread;

		internal void HandleAck(ReadOnlyMemory<byte> receiveBytes, ServerInfo serverInfo)
		{
			var ack = new Ack();
			ack.Decode(receiveBytes);

			var queue = WaitingForAckQueue;

			foreach ((int start, int end) range in ack.ranges)
			{
				Interlocked.Increment(ref serverInfo.NumberOfAckReceive);

				for (int i = range.start; i <= range.end; i++)
				{
					if (queue.TryRemove(i, out Datagram datagram))
					{
						CalculateRto(datagram);

						datagram.PutPool();
					}
					else
					{
						if (Log.IsDebugEnabled) Log.Warn($"ACK, Failed to remove datagram #{i} for {Username}. Queue size={queue.Count}");
					}
				}
			}

			ResendCount = 0;
			WaitForAck = false;
		}

		internal void HandleNak(ReadOnlyMemory<byte> receiveBytes, ServerInfo serverInfo)
		{
			var nak = Nak.CreateObject();
			nak.Reset();
			nak.Decode(receiveBytes);

			var queue = WaitingForAckQueue;

			foreach (Tuple<int, int> range in nak.ranges)
			{
				Interlocked.Increment(ref serverInfo.NumberOfNakReceive);

				int start = range.Item1;
				int end = range.Item2;

				for (int i = start; i <= end; i++)
				{
					if (queue.TryGetValue(i, out var datagram))
					{
						CalculateRto(datagram);

						datagram.RetransmitImmediate = true;
					}
					else
					{
						if (Log.IsDebugEnabled)
							Log.WarnFormat("NAK, no datagram #{0} for {1}", i, Username);
					}
				}
			}

			nak.PutPool();
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void CalculateRto(Datagram datagram)
		{
			// RTT = RTT * 0.875 + rtt * 0.125
			// RTTVar = RTTVar * 0.875 + abs(RTT - rtt)) * 0.125
			// RTO = RTT + 4 * RTTVar
			long rtt = datagram.Timer.ElapsedMilliseconds;
			long RTT = Rtt;
			long RTTVar = RttVar;

			Rtt = (long) (RTT * 0.875 + rtt * 0.125);
			RttVar = (long) (RTTVar * 0.875 + Math.Abs(RTT - rtt) * 0.125);
			Rto = Rtt + 4 * RttVar + 100; // SYNC time in the end
		}


		public void HandleDatagram(ConnectedPacket packet)
		{
			foreach (Packet message in packet.Messages)
			{
				if (message is SplitPartPacket splitPartPacket)
				{
					HandleSplitMessage(splitPartPacket);
					continue;
				}

				message.Timer.Restart();
				HandleRakMessage(message);
			}
		}

		private void HandleRakMessage(Packet message)
		{
			if (message == null) return;

			if (message.ReliabilityHeader.Reliability == Reliability.ReliableOrdered)
			{
				// Adds to a "ordered channel" that doesn't really exist, but oh well.
				//if (CryptoContext?.UseEncryption == true)
				//{
				//	MiNetServer.FastThreadPool.QueueUserWorkItem(() => AddToProcessing(message));
				//}
				//else
				{
					AddToProcessing(message);
				}

				return;
			}

			Log.Warn($"Receive packet with unexpected reliability={message.ReliabilityHeader.Reliability}");

			HandlePacket(message);
		}


		private void HandleSplitMessage(SplitPartPacket splitPart)
		{
			int spId = splitPart.ReliabilityHeader.PartId;
			int spIdx = splitPart.ReliabilityHeader.PartIndex;
			int spCount = splitPart.ReliabilityHeader.PartCount;

			SplitPartPacket[] splitPartList = Splits.GetOrAdd(spId, new SplitPartPacket[spCount]);
			bool haveAllParts = true;
			// Need sync for this part since they come very fast, and very close in time. 
			// If no sync, will often detect complete message two times (or more).

			lock (splitPartList)
			{
				// Already had part (resent). Then ignore. 
				if (splitPartList[spIdx] != null) return;

				splitPartList[spIdx] = splitPart;

				foreach (SplitPartPacket spp in splitPartList)
				{
					if (spp != null) continue;

					haveAllParts = false;
					break;
				}
			}

			if (!haveAllParts) return;

			Log.Debug($"Got all {spCount} split packets for split ID: {spId}");

			Splits.TryRemove(spId, out SplitPartPacket[] _);

			int contiguousLength = 0;
			foreach (SplitPartPacket spp in splitPartList)
			{
				contiguousLength += spp.Message.Length;
			}

			var buffer = new Memory<byte>(new byte[contiguousLength]);

			Reliability headerReliability = splitPart.ReliabilityHeader.Reliability;
			var headerReliableMessageNumber = splitPart.ReliabilityHeader.ReliableMessageNumber;
			var headerOrderingChannel = splitPart.ReliabilityHeader.OrderingChannel;
			var headerOrderingIndex = splitPart.ReliabilityHeader.OrderingIndex;

			int position = 0;
			foreach (SplitPartPacket spp in splitPartList)
			{
				spp.Message.CopyTo(buffer.Slice(position));
				position += spp.Message.Length;
				spp.PutPool();
			}

			try
			{
				Packet fullMessage = PacketFactory.Create(buffer.Span[0], buffer, "raknet") ??
									new UnknownPacket(buffer.Span[0], buffer.ToArray());

				fullMessage.ReliabilityHeader = new ReliabilityHeader()
				{
					Reliability = headerReliability,
					ReliableMessageNumber = headerReliableMessageNumber,
					OrderingChannel = headerOrderingChannel,
					OrderingIndex = headerOrderingIndex,
				};

				Log.Debug($"Assembled split packet {fullMessage.ReliabilityHeader.Reliability} message #{fullMessage.ReliabilityHeader.ReliableMessageNumber}, OrdIdx: #{fullMessage.ReliabilityHeader.OrderingIndex}");

				fullMessage.Timer.Restart();
				HandleRakMessage(fullMessage);
			}
			catch (Exception e)
			{
				Log.Error("Error during split message parsing", e);
				if (Log.IsDebugEnabled) Log.Debug($"0x{buffer.Span[0]:x2}\n{Packet.HexDump(buffer)}");
				Disconnect("Bad packet received from client.", false);
			}
		}

		public void AddToProcessing(Packet message)
		{
			Log.Debug($"Received packet with ordering index={message.ReliabilityHeader.OrderingIndex}");
			try
			{
				if (_cancellationToken.Token.IsCancellationRequested) return;

				if (CryptoContext == null || CryptoContext.UseEncryption == false)
				{
					_lastOrderingIndex = message.ReliabilityHeader.OrderingIndex;
					HandlePacket(message);
					return;
				}

				lock (_eventSync)
				{
					if (_queue.Count == 0 && message.ReliabilityHeader.OrderingIndex == _lastOrderingIndex + 1)
					{
						if (_processingThread != null)
						{
							// Remove the thread again. But need to deal with cancellation token, so not entirely easy.
							// Needs refactoring of the processing thread first.
						}
						_lastOrderingIndex = message.ReliabilityHeader.OrderingIndex;
						HandlePacket(message);
						return;
					}

					if (_processingThread == null)
					{
						_processingThread = new Thread(ProcessQueueThread) {IsBackground = true};
						_processingThread.Start();
						if (Log.IsDebugEnabled) Log.Warn($"Started processing thread for {Username}");
					}

					_queue.Enqueue(message.ReliabilityHeader.OrderingIndex, message);
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
						if (pair.Key == _lastOrderingIndex + 1)
						{
							if (_queue.TryDequeue(out pair))
							{
								_lastOrderingIndex = pair.Key;

								HandlePacket(pair.Value);

								if (_queue.Count == 0)
								{
									WaitHandle.SignalAndWait(_mainWaitEvent, _waitEvent, TimeSpan.FromMilliseconds(50), true);
								}
							}
						}
						else if (pair.Key <= _lastOrderingIndex)
						{
							if (Log.IsDebugEnabled) Log.Warn($"{Username} - Resent. Expected {_lastOrderingIndex + 1}, but was {pair.Key}.");
							if (_queue.TryDequeue(out pair))
							{
								pair.Value.PutPool();
							}
						}
						else
						{
							if (Log.IsDebugEnabled) Log.Warn($"{Username} - Wrong sequence. Expected {_lastOrderingIndex + 1}, but was {pair.Key}.");
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

		internal void HandlePacket(Packet message)
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
					var batch = (McpeWrapper) message;
					var messages = new List<Packet>();

					// Get bytes to process
					ReadOnlyMemory<byte> payload = batch.payload;

					// Decrypt bytes

					if (CryptoContext != null && CryptoContext.UseEncryption)
					{
						// This call copies the entire buffer, but what can we do? It is kind of compensated by not
						// creating a new buffer when parsing the packet (only a mem-slice)
						payload = CryptoUtils.Decrypt(payload, CryptoContext);
					}

					// Decompress bytes

					var stream = new MemoryStreamReader(payload.Slice(0, payload.Length - 4)); // slice away adler
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
						while (!stream.Eof)
						{
							int len = (int) VarInt.ReadUInt32(deflateStream);
							Memory<byte> internalBuffer = new byte[len];
							deflateStream.Read(internalBuffer.Span);
							int id = internalBuffer.Span[0];
							internalBuffer = internalBuffer.Slice(id > 127 ? 2 : 1); //TODO: This is stupid. Get rid of the id slicing

							//if (Log.IsDebugEnabled)
							//	Log.Debug($"0x{internalBuffer[0]:x2}\n{Packet.HexDump(internalBuffer)}");

							try
							{
								messages.Add(PacketFactory.Create((byte) id, internalBuffer, "mcpe") ??
											new UnknownPacket((byte) id, internalBuffer));
							}
							catch (Exception)
							{
								if (Log.IsDebugEnabled) Log.Warn($"Error parsing packet 0x{message.Id:X2}\n{Packet.HexDump(internalBuffer)}");
								throw;
							}
						}

						if (stream.Length > stream.Position) throw new Exception("Have more data");
					}

					foreach (Packet msg in messages)
					{
						// Temp fix for performance, take 1.
						var interact = msg as McpeInteract;
						if (interact?.actionId == 4 && interact.targetRuntimeEntityId == 0) continue;

						msg.ReliabilityHeader = new ReliabilityHeader()
						{
							Reliability = batch.ReliabilityHeader.Reliability,
							ReliableMessageNumber = batch.ReliabilityHeader.ReliableMessageNumber,
							OrderingChannel = batch.ReliabilityHeader.OrderingChannel,
							OrderingIndex = batch.ReliabilityHeader.OrderingIndex,
						};

						HandlePacket(msg);
					}

					message.PutPool();
					return;
				}

				RakNetProcessor.TraceReceive(message);

				// This needs rework. Some packets (transaction) really require the correct order to be maintained 
				// in order to process correctly. I was thinking perhaps marking some packets as "order important" and
				// check that flag here. But for now, I'm disabling the threading here. Not even sure why it is here.

				HandlePacket(MessageHandler, message);
				message.PutPool();
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
			var packet = ConnectedPong.CreateObject();
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

			RakNetProcessor.TraceSend(packet);

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

		private int _tickCounter;

		public async Task SendTickAsync()
		{
			try
			{
				Task sendAckQueueAsync = SendAckQueueAsync();
				Task sendQueueAsync = SendQueueAsync();

				if (_tickCounter++ >= 5)
				{
					Task updateAsync = UpdateAsync();
					await Task.WhenAll(sendAckQueueAsync, sendQueueAsync, updateAsync);
					_tickCounter = 0;
				}
				else
				{
					await Task.WhenAll(sendAckQueueAsync, sendQueueAsync);
				}
			}
			catch (Exception e)
			{
				Log.Error(e);
			}
		}


		//private object _updateSync = new object();
		private SemaphoreSlim _updateSync = new SemaphoreSlim(1, 1);
		private Stopwatch _forceQuitTimer = new Stopwatch();

		private async Task UpdateAsync()
		{
			if (Server == null) return; // MiNET Client

			if (Evicted) return;

			if (MiNetServer.FastThreadPool == null) return;

			if (!(await _updateSync.WaitAsync(0)))
				return;

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

				if (WaitingForAckQueue.Count == 0) return;

				if (WaitForAck) return;

				if (Rto == 0) return;

				long rto = Math.Max(100, Rto);
				var queue = WaitingForAckQueue;

				foreach (KeyValuePair<int, Datagram> datagramPair in queue)
				{
					if (Evicted) return;

					Datagram datagram = datagramPair.Value;

					if (!datagram.Timer.IsRunning)
					{
						Log.Error($"Timer not running for #{datagram.Header.DatagramSequenceNumber}");
						datagram.Timer.Restart();
						continue;
					}

					if (Rtt == -1) return;

					long elapsedTime = datagram.Timer.ElapsedMilliseconds;
					long datagramTimeout = rto * (datagram.TransmissionCount + ResendCount + 1);
					datagramTimeout = Math.Min(datagramTimeout, 3000);
					datagramTimeout = Math.Max(datagramTimeout, 100);

					if (datagram.RetransmitImmediate || elapsedTime >= datagramTimeout)
					{
						if (!Evicted && WaitingForAckQueue.TryRemove(datagram.Header.DatagramSequenceNumber, out _))
						{
							ErrorCount++;
							ResendCount++;


							if (Log.IsDebugEnabled) Log.Warn($"{(datagram.RetransmitImmediate ? "NAK RSND" : "TIMEOUT")}, Resent #{datagram.Header.DatagramSequenceNumber.IntValue()} Type: {datagram.FirstMessageId} (0x{datagram.FirstMessageId:x2}) for {Username} ({elapsedTime} > {datagramTimeout}) RTO {Rto}");

							Interlocked.Increment(ref Server.ServerInfo.NumberOfResends);
							await Server.SendDatagramAsync(this, datagram);

							//var resendTasks = new List<Task>();
							//resendTasks.Add(Server.SendDatagramAsync(this, datagram));
							//await Task.WhenAll(resendTasks);
						}
					}
				}
			}
			finally
			{
				if (_forceQuitTimer.ElapsedMilliseconds > 100)
				{
					Log.Warn($"Update took unexpected long time={_forceQuitTimer.ElapsedMilliseconds}, Count={WaitingForAckQueue.Count}");
				}

				_updateSync.Release();
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


		private async Task SendAckQueueAsync()
		{
			RakSession session = this;
			var queue = session.OutgoingAckQueue;
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
				await Server.SendDataAsync(data, session.EndPoint);
			}

			acks.PutPool();
		}

		private SemaphoreSlim _syncHack = new SemaphoreSlim(1, 1);
		private HighPrecisionTimer _tickerHighPrecisionTimer;

		[SuppressMessage("ReSharper", "InconsistentlySynchronizedField")]
		public async Task SendQueueAsync()
		{
			if (_sendQueueNotConcurrent.Count == 0) return;

			if (!(await _syncHack.WaitAsync(0))) return;

			try
			{
				using (var memStream = new MemoryStream())
				{
					Queue<Packet> queue = _sendQueueNotConcurrent;
					var sendLast = new List<Packet>();

					int messageCount = 0;

					int length = queue.Count;
					for (int i = 0; i < length; i++)
					{
						Packet packet = null;
						lock (_queueSync)
						{
							if (queue.Count == 0) break;
							try
							{
								packet = queue.Dequeue();
							}
							catch (Exception)
							{
							}
						}

						if (packet == null) continue;

						if (State == ConnectionState.Unconnected)
						{
							packet.PutPool();
							continue;
						}

						if (length == 1)
						{
							await Server.SendPacketAsync(this, packet);
							continue;
						}

						if (packet is McpeWrapper)
						{
							sendLast.Add(packet);
							continue;
						}

						if (packet.NoBatch)
						{
							sendLast.Add(packet);
							continue;
						}

						if (messageCount == 0)
						{
							memStream.Position = 0;
							memStream.SetLength(0);
						}

						byte[] bytes = packet.Encode();
						if (bytes != null)
						{
							BatchUtils.WriteLength(memStream, bytes.Length);
							memStream.Write(bytes, 0, bytes.Length);
							messageCount++;
						}

						packet.PutPool();
					}
					foreach (Packet packet in sendLast)
					{
						await Server.SendPacketAsync(this, packet);
					}

					if (State == ConnectionState.Unconnected)
					{
						return;
					}

					await SendBufferedAsync(messageCount, memStream);
				}
			}
			catch (Exception e)
			{
				Log.Error(e);
			}
			finally
			{
				_syncHack.Release();
			}
		}

		private async Task SendBufferedAsync(int messageCount, MemoryStream memStream)
		{
			if (messageCount == 0) return;

			McpeWrapper batch = BatchUtils.CreateBatchPacket(new Memory<byte>(memStream.GetBuffer(), 0, (int) memStream.Length), CompressionLevel.Fastest, false);
			batch.Encode();
			memStream.Position = 0;
			memStream.SetLength(0);

			await Server.SendPacketAsync(this, batch);
		}

		public void SendDirectPacket(Packet packet)
		{
			Server.SendPacketAsync(this, packet).Wait();
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