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
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using MiNET.Utils;
using MiNET.Utils.Collections;

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
		private Thread _orderedQueueProcessingThread;


		public object EncryptionSyncRoot { get; } = new object();

		public ConnectionInfo ConnectionInfo { get; }

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


		public RakSession(ConnectionInfo connectionInfo, IPacketSender packetSender, IPEndPoint endPoint, short mtuSize, ICustomMessageHandler messageHandler = null)
		{
			Log.Debug($"Create session for {endPoint}");

			_packetSender = packetSender;
			ConnectionInfo = connectionInfo;
			CustomMessageHandler = messageHandler ?? new DefaultMessageHandler();
			EndPoint = endPoint;
			MtuSize = mtuSize;

			InactivityTimeout = Config.GetProperty("InactivityTimeout", 8500);
			ResendThreshold = Config.GetProperty("ResendThreshold", 10);

			_cancellationToken = new CancellationTokenSource();
			//_tickerHighPrecisionTimer = new HighPrecisionTimer(10, SendTick, true);
		}

		/// <summary>
		///     Main receive entry to this layer. Will receive and handle messages
		///     on RakNet message level. May come from either UDP or TCP, matters not.
		/// </summary>
		/// <param name="message"></param>
		internal void HandleRakMessage(Packet message)
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

		public void AddToSequencedChannel(Packet message)
		{
			AddToOrderedChannel(message);
		}

		public void AddToOrderedChannel(Packet message)
		{
			try
			{
				if (_cancellationToken.Token.IsCancellationRequested) return;

				if (message.ReliabilityHeader.OrderingIndex <= _lastOrderingIndex) return;

				lock (_eventSync)
				{
					//Log.Debug($"Received packet {message.Id} with ordering index={message.ReliabilityHeader.OrderingIndex}. Current index={_lastOrderingIndex}");

					//if (_orderingBufferQueue.Count == 0 && message.ReliabilityHeader.OrderingIndex == _lastOrderingIndex + 1)
					//{
					//	if (_orderedQueueProcessingThread != null)
					//	{
					//		// Remove the thread again? But need to deal with cancellation token, so not entirely easy.
					//		// Needs refactoring of the processing thread first.
					//	}
					//	_lastOrderingIndex = message.ReliabilityHeader.OrderingIndex;
					//	HandlePacket(message);
					//	return;
					//}

					if (_orderedQueueProcessingThread == null)
					{
						_orderedQueueProcessingThread = new Thread(ProcessOrderedQueue)
						{
							IsBackground = true,
							Name = $"Ordering Thread [{EndPoint}]"
						};
						_orderedQueueProcessingThread.Start();
						if (Log.IsDebugEnabled) Log.Warn($"Started processing thread for {Username}");
					}

					_orderingBufferQueue.Enqueue(message.ReliabilityHeader.OrderingIndex, message);
					if (message.ReliabilityHeader.OrderingIndex == _lastOrderingIndex + 1)
					{
						WaitHandle.SignalAndWait(_packetQueuedWaitEvent, _packetHandledWaitEvent);
					}
				}
			}
			catch (Exception)
			{
			}
		}

		private void ProcessOrderedQueue()
		{
			try
			{
				while (!_cancellationToken.IsCancellationRequested && !ConnectionInfo.IsEmulator)
				{
					if (_orderingBufferQueue.TryPeek(out KeyValuePair<int, Packet> pair))
					{
						if (pair.Key == _lastOrderingIndex + 1)
						{
							if (_orderingBufferQueue.TryDequeue(out pair))
							{
								//Log.Debug($"Handling packet ordering index={pair.Value.ReliabilityHeader.OrderingIndex}. Current index={_lastOrderingIndex}");

								_lastOrderingIndex = pair.Key;
								HandlePacket(pair.Value);

								if (_orderingBufferQueue.Count == 0)
								{
									WaitHandle.SignalAndWait(_packetHandledWaitEvent, _packetQueuedWaitEvent, 500, true);
								}
							}
						}
						else if (pair.Key <= _lastOrderingIndex)
						{
							if (Log.IsDebugEnabled) Log.Debug($"{Username} - Resent. Expected {_lastOrderingIndex + 1}, but was {pair.Key}.");
							if (_orderingBufferQueue.TryDequeue(out pair))
							{
								pair.Value.PutPool();
							}
						}
						else
						{
							if (Log.IsDebugEnabled) Log.Debug($"{Username} - Wrong sequence. Expected {_lastOrderingIndex + 1}, but was {pair.Key}.");
							WaitHandle.SignalAndWait(_packetHandledWaitEvent, _packetQueuedWaitEvent, 500, true);
						}
					}
					else
					{
						if (_orderingBufferQueue.Count == 0)
						{
							WaitHandle.SignalAndWait(_packetHandledWaitEvent, _packetQueuedWaitEvent, 500, true);
						}
					}
				}
			}
			catch (ObjectDisposedException)
			{
				// Ignore. Comes from the reset events being waited on while being disposed. Not a problem.
			}
			catch (Exception e)
			{
				Log.Error($"Exit receive handler task for player", e);
			}
		}

		private void HandlePacket(Packet message)
		{
			if (message == null) return;

			try
			{
				RakOfflineHandler.TraceReceive(Log, message);

				if (message.Id < (int) DefaultMessageIdTypes.ID_USER_PACKET_ENUM)
				{
					// Standard RakNet online message handlers
					switch (message)
					{
						case ConnectedPing connectedPing:
							HandleConnectedPing(connectedPing);
							break;
						case ConnectedPong connectedPong:
							HandleConnectedPong(connectedPong);
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
					try
					{
						CustomMessageHandler.HandlePacket(message);
					}
					catch (Exception e)
					{
						// ignore
						Log.Warn($"Custom message handler error", e);
					}
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

		private void HandleConnectedPong(ConnectedPong connectedPong)
		{
			// Ignore
		}

		protected virtual void HandleConnectedPing(ConnectedPing message)
		{
			var packet = ConnectedPong.CreateObject();
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
			Log.Debug($"New incoming connection from {EndPoint.Address} {EndPoint.Port}");

			State = ConnectionState.Connected;
		}

		private void HandleConnectionRequestAccepted(ConnectionRequestAccepted message)
		{
			SendNewIncomingConnection();

			State = ConnectionState.Connected;

			CustomMessageHandler?.Connected();
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
			var ping = DetectLostConnections.CreateObject();
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
				if (Log.IsDebugEnabled) Log.Debug($"Ignoring send of packet {packet.GetType().Name} because session is not connected");
				packet.PutPool();
				return;
			}

			RakOfflineHandler.TraceSend(packet);

			lock (_queueSync)
			{
				_sendQueueNotConcurrent.Enqueue(packet);
			}
		}

		private int _tickCounter;

		public async Task SendTickAsync(RakConnection connection)
		{
			try
			{
				if (_tickCounter++ >= 5)
				{
					await Task.WhenAll(SendAckQueueAsync(), UpdateAsync(), SendQueueAsync(), connection.UpdateAsync(this));
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

		private async Task UpdateAsync()
		{
			if (Evicted) return;

			if (MiNetServer.FastThreadPool == null) return;

			if (!await _updateSync.WaitAsync(0)) return;

			try
			{
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
			}
			finally
			{
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

				Interlocked.Increment(ref ConnectionInfo.NumberOfAckSent);
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
		public async Task SendQueueAsync(int millisecondsWait = 0)
		{
			if (_sendQueueNotConcurrent.Count == 0) return;

			// Extremely important that this will not allow more than one thread at a time.
			// This methods handle ordering and potential encryption, hence order matters.
			if (!(await _syncHack.WaitAsync(millisecondsWait))) return;

			try
			{
				var sendList = new List<Packet>();
				Queue<Packet> queue = _sendQueueNotConcurrent;
				int length = queue.Count;
				for (int i = 0; i < length; i++)
				{
					Packet packet;
					lock (_queueSync)
					{
						if (queue.Count == 0) break;

						if (!queue.TryDequeue(out packet)) break;
					}

					if (packet == null) continue;

					if (State == ConnectionState.Unconnected)
					{
						packet.PutPool();
						continue;
					}

					sendList.Add(packet);
				}

				if (sendList.Count == 0) return;

				List<Packet> prepareSend = CustomMessageHandler.PrepareSend(sendList);
				var preppedSendList = new List<Packet>();
				foreach (Packet packet in prepareSend)
				{
					Packet message = packet;

					if (CustomMessageHandler != null) message = CustomMessageHandler.HandleOrderedSend(message);

					Reliability reliability = message.ReliabilityHeader.Reliability;
					if (reliability == Reliability.Undefined) reliability = Reliability.Reliable; // Questionable practice

					if (reliability == Reliability.ReliableOrdered) message.ReliabilityHeader.OrderingIndex = Interlocked.Increment(ref OrderingIndex);

					preppedSendList.Add(message);
					//await _packetSender.SendPacketAsync(this, message);
				}

				await _packetSender.SendPacketAsync(this, preppedSendList);
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

		public void SendDirectPacket(Packet packet)
		{
			if (packet.ReliabilityHeader.Reliability == Reliability.ReliableOrdered)
				throw new Exception($"Can't send direct messages with ordering. The offending packet was {packet.GetType().Name}");

			if (packet.ReliabilityHeader.Reliability == Reliability.Undefined)
				packet.ReliabilityHeader.Reliability = Reliability.Reliable; // Questionable practice

			_packetSender.SendPacketAsync(this, packet).Wait();
		}

		public IPEndPoint GetClientEndPoint()
		{
			return EndPoint;
		}

		public long GetNetworkNetworkIdentifier()
		{
			return NetworkIdentifier;
		}

		public void Close()
		{
			if (!ConnectionInfo.RakSessions.TryRemove(EndPoint, out _))
			{
				return;
			}

			State = ConnectionState.Unconnected;
			Evicted = true;
			CustomMessageHandler = null;

			// Send with high priority, bypass queue
			SendDirectPacket(DisconnectionNotification.CreateObject());

			SendQueueAsync(500).Wait();

			_cancellationToken.Cancel();
			_packetQueuedWaitEvent.Set();
			_packetHandledWaitEvent.Set();
			_orderingBufferQueue.Clear();

			_packetSender.Close(this);

			try
			{
				_orderedQueueProcessingThread = null;
				_cancellationToken.Dispose();
				_packetQueuedWaitEvent.Close();
				_packetHandledWaitEvent.Close();
			}
			catch
			{
				// ignored
			}

			if (Log.IsDebugEnabled) Log.Info($"Closed network session for player {Username}");
		}
	}
}