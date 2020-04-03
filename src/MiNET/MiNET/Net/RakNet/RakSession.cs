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
using System.Buffers;
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
using MiNET.Utils;

namespace MiNET.Net.RakNet
{
	public class RakSession : INetworkHandler
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(RakSession));

		private readonly IPacketSender _packetSender;

		private long _lastOrderingIndex = -1; // That's the first message with wrapper
		private AutoResetEvent _packetQueuedWaitEvent = new AutoResetEvent(false);
		private AutoResetEvent _packetHandledWaitEvent = new AutoResetEvent(false);
		private object _eventSync = new object();

		private readonly ConcurrentPriorityQueue<int, Packet> _orderingBufferQueue = new ConcurrentPriorityQueue<int, Packet>();
		private CancellationTokenSource _cancellationToken;
		private Thread _processingThread;


		public object EncryptionSyncRoot { get; } = new object();

		public ServerInfo ServerInfo { get; }

		public ICustomMessageHandler CustomMessageHandler { get; set; }

		public string Username { get; set; }
		public IPEndPoint EndPoint { get; private set; }
		public short MtuSize { get; set; }
		public long NetworkIdentifier { get; set; }

		public int DatagramSequenceNumber = -1;
		public int ReliableMessageNumber = -1;
		public int SplitPartId = 0;
		public int OrderingIndex = -1;
		public int ErrorCount { get; set; }

		public bool Evicted { get; set; }

		public ConnectionState State { get; set; } = ConnectionState.Unconnected;

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

		public long InactivityTimeout { get; }
		public int ResendThreshold { get; }

		public ConcurrentDictionary<int, SplitPartPacket[]> Splits { get; } = new ConcurrentDictionary<int, SplitPartPacket[]>();
		public ConcurrentQueue<int> OutgoingAckQueue { get; } = new ConcurrentQueue<int>();
		public ConcurrentDictionary<int, Datagram> WaitingForAckQueue { get; } = new ConcurrentDictionary<int, Datagram>();


		public RakSession(ServerInfo serverInfo, IPacketSender packetSender, IPEndPoint endPoint, short mtuSize, ICustomMessageHandler messageHandler = null)
		{
			_packetSender = packetSender;
			ServerInfo = serverInfo;
			CustomMessageHandler = messageHandler ?? new DefaultMessageHandler();
			EndPoint = endPoint;
			MtuSize = mtuSize;

			InactivityTimeout = Config.GetProperty("InactivityTimeout", 8500);
			ResendThreshold = Config.GetProperty("ResendThreshold", 10);

			_cancellationToken = new CancellationTokenSource();
			//_tickerHighPrecisionTimer = new HighPrecisionTimer(10, SendTick, true);
		}

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


		public void HandleDatagram(Datagram datagram)
		{
			foreach (Packet packet in datagram.Messages)
			{
				Packet message = packet;
				if (message is SplitPartPacket splitPartPacket)
				{
					message = HandleSplitMessage(splitPartPacket);
					if (message == null) continue;
				}

				message.Timer.Restart();
				HandleRakMessage(message);
			}
		}

		private void HandleRakMessage(Packet message)
		{
			if (message == null) return;

			// This is not completely finished. Ordering and sequence streams (32 unique channels/streams each)
			// needs to work by their channel index. Right now, it's only one channel per reliability type.
			// According to Dylan order and sequence streams can run on the same channel, but documentation
			// says it can not. So I'll go with documentation until proven wrong.

			switch (message.ReliabilityHeader.Reliability)
			{
				case Reliability.ReliableOrdered:
				case Reliability.ReliableOrderedWithAckReceipt:
					AddToOrderedChannel(message);
					break;
				case Reliability.UnreliableSequenced:
				case Reliability.ReliableSequenced:
					AddToSequencedChannel(message);
					break;
				case Reliability.Unreliable:
				case Reliability.UnreliableWithAckReceipt:
				case Reliability.Reliable:
				case Reliability.ReliableWithAckReceipt:
					HandlePacket(message);
					break;
				case Reliability.Undefined:
					Log.Error($"Receive packet with undefined reliability");
					break;
				default:
					Log.Warn($"Receive packet with unexpected reliability={message.ReliabilityHeader.Reliability}");
					break;
			}
		}

		private Packet HandleSplitMessage(SplitPartPacket splitPart)
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
				if (splitPartList[spIdx] != null) return null;

				splitPartList[spIdx] = splitPart;

				foreach (SplitPartPacket spp in splitPartList)
				{
					if (spp != null) continue;

					haveAllParts = false;
					break;
				}
			}

			if (!haveAllParts) return null;

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

				return fullMessage;
			}
			catch (Exception e)
			{
				Log.Error("Error during split message parsing", e);
				if (Log.IsDebugEnabled) Log.Debug($"0x{buffer.Span[0]:x2}\n{Packet.HexDump(buffer)}");
				Disconnect("Bad packet received from client.", false);
			}

			return null;
		}

		public void AddToSequencedChannel(Packet message)
		{
			AddToOrderedChannel(message);
		}

		public void AddToOrderedChannel(Packet message)
		{
			try
			{
				if (_cancellationToken.Token.IsCancellationRequested) return;

				//if (CryptoContext == null || CryptoContext.UseEncryption == false)
				//{
				//	_lastOrderingIndex = message.ReliabilityHeader.OrderingIndex;
				//	HandlePacket(message);
				//	return;
				//}

				lock (_eventSync)
				{
					//Log.Debug($"Received packet {message.Id} with ordering index={message.ReliabilityHeader.OrderingIndex}. Current index={_lastOrderingIndex}");

					if (_orderingBufferQueue.Count == 0 && message.ReliabilityHeader.OrderingIndex == _lastOrderingIndex + 1)
					{
						if (_processingThread != null)
						{
							// Remove the thread again? But need to deal with cancellation token, so not entirely easy.
							// Needs refactoring of the processing thread first.
						}
						_lastOrderingIndex = message.ReliabilityHeader.OrderingIndex;
						HandlePacket(message);
						return;
					}

					if (_processingThread == null)
					{
						_processingThread = new Thread(ProcessQueueThread) {IsBackground = true, Name = $"Ordering Thread [{Username}]"};
						_processingThread.Start();
						if (Log.IsDebugEnabled) Log.Warn($"Started processing thread for {Username}");
					}

					_orderingBufferQueue.Enqueue(message.ReliabilityHeader.OrderingIndex, message);
					if (message.ReliabilityHeader.OrderingIndex == _lastOrderingIndex + 1)
					{
						WaitHandle.SignalAndWait(_packetQueuedWaitEvent, _packetHandledWaitEvent);
					}
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
					if (_orderingBufferQueue.TryPeek(out KeyValuePair<int, Packet> pair))
					{
						if (pair.Key == _lastOrderingIndex + 1)
						{
							if (_orderingBufferQueue.TryDequeue(out pair))
							{
								_lastOrderingIndex = pair.Key;

								//Log.Debug($"Handling packet ordering index={pair.Value.ReliabilityHeader.OrderingIndex}. Current index={_lastOrderingIndex}");
								HandlePacket(pair.Value);

								if (_orderingBufferQueue.Count == 0)
								{
									WaitHandle.SignalAndWait(_packetHandledWaitEvent, _packetQueuedWaitEvent, TimeSpan.FromMilliseconds(50), true);
								}
							}
						}
						else if (pair.Key <= _lastOrderingIndex)
						{
							if (Log.IsDebugEnabled) Log.Warn($"{Username} - Resent. Expected {_lastOrderingIndex + 1}, but was {pair.Key}.");
							if (_orderingBufferQueue.TryDequeue(out pair))
							{
								pair.Value.PutPool();
							}
						}
						else
						{
							if (Log.IsDebugEnabled) Log.Warn($"{Username} - Wrong sequence. Expected {_lastOrderingIndex + 1}, but was {pair.Key}.");
							WaitHandle.SignalAndWait(_packetHandledWaitEvent, _packetQueuedWaitEvent, TimeSpan.FromMilliseconds(50), true);
						}
					}
					else
					{
						if (_orderingBufferQueue.Count == 0)
						{
							WaitHandle.SignalAndWait(_packetHandledWaitEvent, _packetQueuedWaitEvent, TimeSpan.FromMilliseconds(50), true);
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

		private void HandlePacket(Packet message)
		{
			if (message == null) return;

			try
			{
				RakProcessor.TraceReceive(message);

				if (message.Id < (int) DefaultMessageIdTypes.ID_USER_PACKET_ENUM)
				{
					// Standard RakNet online message handlers
					switch (message)
					{
						case ConnectedPing connectedPing:
							HandleConnectedPing(connectedPing);
							break;
						case DetectLostConnections _:
							break;
						case ConnectionRequest connectionRequest:
							HandleConnectionRequest(connectionRequest);
							break;
						case ConnectionRequestAccepted connectionRequestAccepted:
							HandleConnectionRequestAccepted(connectionRequestAccepted);
							break;
						case NewIncomingConnection newIncomingConnection:
							HandleNewIncomingConnection(newIncomingConnection);
							break;
						case DisconnectionNotification _:
							HandleDisconnectionNotification();
							break;
						default:
							Log.Error($"Unhandled packet: {message.GetType().Name} 0x{message.Id:X2} for user: {Username}, IP {EndPoint.Address}");
							if (Log.IsDebugEnabled) Log.Warn($"Unknown packet 0x{message.Id:X2}\n{Packet.HexDump(message.Bytes)}");
							break;
					}
				}
				else
				{
					CustomMessageHandler.HandlePacket(message);
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
			catch (Exception e)
			{
				Log.Error("Packet handling", e);
				throw;
			}
			finally
			{
				message?.PutPool();
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

		private void HandleConnectionRequestAccepted(ConnectionRequestAccepted message)
		{
			SendNewIncomingConnection();

			State = ConnectionState.Connected;

			//SendLogin(Username);
		}

		public void SendNewIncomingConnection()
		{
			var packet = NewIncomingConnection.CreateObject();
			packet.clientendpoint = EndPoint;
			packet.systemAddresses = new IPEndPoint[20];
			for (int i = 0; i < 20; i++)
			{
				packet.systemAddresses[i] = new IPEndPoint(IPAddress.Any, 0);
			}

			SendPacket(packet);
		}


		protected virtual void HandleDisconnectionNotification()
		{
			Disconnect("Client requested disconnected", false);
		}

		public virtual void Disconnect(string reason, bool sendDisconnect = true)
		{
			CustomMessageHandler?.Disconnect(reason, sendDisconnect);
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
				Log.Warn($"Ignoring packet, because session is not connected");
				packet.PutPool();
				return;
			}

			RakProcessor.TraceSend(packet);

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
			}
		}

		private int _tickCounter;

		public async Task SendTickAsync()
		{
			try
			{
				if (_tickCounter++ >= 5)
				{
					await Task.WhenAll(SendAckQueueAsync(), UpdateAsync(), SendQueueAsync());
					_tickCounter = 0;
				}
				else
				{
					await Task.WhenAll(SendAckQueueAsync(), SendQueueAsync());
				}
			}
			catch (Exception e)
			{
				Log.Warn(e);
			}
		}


		//private object _updateSync = new object();
		private SemaphoreSlim _updateSync = new SemaphoreSlim(1, 1);
		private Stopwatch _forceQuitTimer = new Stopwatch();

		private async Task UpdateAsync()
		{
			if (Evicted) return;

			if (MiNetServer.FastThreadPool == null) return;

			if (!await _updateSync.WaitAsync(0)) return;

			try
			{
				_forceQuitTimer.Restart();

				if (Evicted) return;

				long now = DateTime.UtcNow.Ticks / TimeSpan.TicksPerMillisecond;

				long lastUpdate = LastUpdatedTime.Ticks / TimeSpan.TicksPerMillisecond;

				if (lastUpdate + InactivityTimeout + 3000 < now)
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


				if (State != ConnectionState.Connected && CustomMessageHandler != null && lastUpdate + 3000 < now)
				{
					MiNetServer.FastThreadPool.QueueUserWorkItem(() => { Disconnect("You've been kicked with reason: Lost connection."); });

					return;
				}

				if (CustomMessageHandler == null) return;

				if (!WaitForAck && (ResendCount > ResendThreshold || lastUpdate + InactivityTimeout < now))
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

							Interlocked.Increment(ref ServerInfo.NumberOfResends);
							await SendDatagramAsync(this, datagram);

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

		private async Task SendAckQueueAsync()
		{
			RakSession session = this;
			var queue = session.OutgoingAckQueue;
			int queueCount = queue.Count;

			if (queueCount == 0) return;

			var acks = Acks.CreateObject();
			for (int i = 0; i < queueCount; i++)
			{
				if (!queue.TryDequeue(out int ack)) break;

				Interlocked.Increment(ref ServerInfo.NumberOfAckSent);
				acks.acks.Add(ack);
			}

			if (acks.acks.Count > 0)
			{
				byte[] data = acks.Encode();
				await _packetSender.SendDataAsync(data, session.EndPoint);
			}

			acks.PutPool();
		}

		private SemaphoreSlim _syncHack = new SemaphoreSlim(1, 1);

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
							await SendPacketAsync(packet);
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
						await SendPacketAsync(packet);
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

			await SendPacketAsync(batch);
		}

		public void SendDirectPacket(Packet packet)
		{
			SendPacketAsync(packet).Wait();
		}

		public IPEndPoint GetClientEndPoint()
		{
			return EndPoint;
		}

		public long GetNetworkNetworkIdentifier()
		{
			return NetworkIdentifier;
		}

		public async Task SendPacketAsync(Packet message)
		{
			foreach (Datagram datagram in Datagram.CreateDatagrams(message, MtuSize, this))
			{
				await SendDatagramAsync(this, datagram);
			}

			message.PutPool();
		}

		public async Task SendDatagramAsync(RakSession session, Datagram datagram)
		{
			if (datagram.MessageParts.Count == 0)
			{
				Log.Warn($"Failed to send #{datagram.Header.DatagramSequenceNumber.IntValue()}");
				datagram.PutPool();
				return;
			}

			if (datagram.TransmissionCount > 10)
			{
				if (Log.IsDebugEnabled) Log.Warn($"Retransmission count exceeded. No more resend of #{datagram.Header.DatagramSequenceNumber.IntValue()} Type: {datagram.FirstMessageId} (0x{datagram.FirstMessageId:x2}) for {session.Username}");

				datagram.PutPool();

				Interlocked.Increment(ref ServerInfo.NumberOfFails);
				//TODO: Disconnect! Because of encryption, this connection can't be used after this point
				return;
			}

			datagram.Header.DatagramSequenceNumber = Interlocked.Increment(ref session.DatagramSequenceNumber);
			datagram.TransmissionCount++;
			datagram.RetransmitImmediate = false;

			byte[] buffer = ArrayPool<byte>.Shared.Rent(1600);
			int length = (int) datagram.GetEncoded(ref buffer);

			datagram.Timer.Restart();

			if (!session.WaitingForAckQueue.TryAdd(datagram.Header.DatagramSequenceNumber.IntValue(), datagram))
			{
				Log.Warn($"Datagram sequence unexpectedly existed in the ACK/NAK queue already {datagram.Header.DatagramSequenceNumber.IntValue()}");
				datagram.PutPool();
			}

			//lock (session.SyncRoot)
			{
				await _packetSender.SendDataAsync(buffer, length, session.EndPoint);
				ArrayPool<byte>.Shared.Return(buffer);
			}
		}

		public void Close()
		{
			if (!ServerInfo.RakSessions.TryRemove(EndPoint, out RakSession session))
			{
				return;
			}

			if (OutgoingAckQueue.Count > 0)
			{
				Thread.Sleep(50);
			}

			State = ConnectionState.Unconnected;
			Evicted = true;
			CustomMessageHandler = null;

			// Send with high priority, bypass queue
			SendDirectPacket(new DisconnectionNotification());

			SendQueueAsync().Wait();

			_cancellationToken.Cancel();
			_packetQueuedWaitEvent.Set();
			_packetHandledWaitEvent.Set();
			_orderingBufferQueue.Clear();

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
				_packetQueuedWaitEvent.Close();
				_packetHandledWaitEvent.Close();
			}
			catch
			{
				// ignored
			}

			if (Log.IsDebugEnabled) Log.Warn($"Closed network session for player {Username}");
		}
	}
}