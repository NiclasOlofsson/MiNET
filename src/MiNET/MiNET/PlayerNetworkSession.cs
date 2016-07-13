using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using MiNET.Net;
using MiNET.Utils;

namespace MiNET
{
	public class PlayerNetworkSession
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (PlayerNetworkSession));

		public object SyncRoot { get; private set; } = new object();
		public object ProcessSyncRoot { get; private set; } = new object();
		public object SyncRootUpdate { get; private set; } = new object();
		public object EncodeSync { get; private set; } = new object();

		public Player Player { get; set; }
		public int Mtuize { get; set; }

		public DateTime CreateTime { get; private set; }
		public IPEndPoint EndPoint { get; private set; }
		public short MtuSize { get; set; }
		public UdpClient UdpClient { get; set; }

		private ConcurrentQueue<int> _playerAckQueue = new ConcurrentQueue<int>();
		private ConcurrentDictionary<int, Datagram> _waitingForAcksQueue = new ConcurrentDictionary<int, Datagram>();
		private ConcurrentDictionary<int, SplitPartPackage[]> _splits = new ConcurrentDictionary<int, SplitPartPackage[]>();
		public int DatagramSequenceNumber = -1;
		public int ReliableMessageNumber = 0;
		public int OrderingIndex = -1;
		public double SendDelay { get; set; }
		public int ErrorCount { get; set; }
		public bool IsSlowClient { get; set; }
		public bool Evicted { get; set; }
		public ConnectionState State { get; set; }

		public DateTime LastUpdatedTime { get; set; }
		public bool WaitForAck { get; set; }
		public int ResendCount { get; set; }


		public CryptoContext CryptoContext { get; set; }

		public PlayerNetworkSession(Player player, IPEndPoint endPoint, short mtuSize)
		{
			State = ConnectionState.Unconnected;
			Player = player;
			EndPoint = endPoint;
			MtuSize = mtuSize;
			CreateTime = DateTime.UtcNow;
			_cancellationToken = new CancellationTokenSource();
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

		public void Clean()
		{
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
						if (Log.IsDebugEnabled) Log.Warn($"Started processing thread for {Player.Username}");
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
							if (Log.IsDebugEnabled) Log.Warn($"{Player.Username} - Resent. Expected {_lastSequenceNumber + 1}, but was {pair.Key}.");
							if (_queue.TryDequeue(out pair))
							{
								pair.Value.PutPool();
							}
						}
						else
						{
							if (Log.IsDebugEnabled) Log.Warn($"{Player.Username} - Wrong sequence. Expected {_lastSequenceNumber + 1}, but was {pair.Key}.");
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
					playerSession.Player?.HandlePackage(message as Package);
					message.PutPool();
				});
			}
			else
			{
				playerSession.Player?.HandlePackage(message);
				message.PutPool();
			}
		}
	}
}