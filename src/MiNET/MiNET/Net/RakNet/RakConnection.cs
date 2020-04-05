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
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using MiNET.Utils;

namespace MiNET.Net.RakNet
{
	public class RakConnection : IPacketSender
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(RakConnection));

		private UdpClient _listener;
		private IPEndPoint _endpoint;
		private DedicatedThreadPool _receiveThreadPool;
		private Thread _receiveThread;
		private HighPrecisionTimer _tickerHighPrecisionTimer;
		private ConcurrentDictionary<IPEndPoint, RakSession> _rakSessions = new ConcurrentDictionary<IPEndPoint, RakSession>();
		private readonly GreyListManager _greyListManager;
		private readonly MotdProvider _motdProvider;
		public readonly RakProcessor _rakProcessor;
		public ServerInfo ServerInfo { get; }

		public bool FoundServer => _rakProcessor.HaveServer;
		public bool AutoConnect
		{
			get => _rakProcessor.AutoConnect;
			set => _rakProcessor.AutoConnect = value;
		}

		// This is only used in client scenarios. Will contain
		// information regarding a located server.
		public IPEndPoint RemoteEndpoint { get; set; }
		public string RemoteServerName { get; set; }

		public Func<RakSession, ICustomMessageHandler> CustomMessageHandlerFactory { get; set; }

		public RakConnection(GreyListManager greyListManager, MotdProvider motdProvider, DedicatedThreadPool threadPool = null) : this(null, greyListManager, motdProvider, threadPool)
		{
		}

		public RakConnection(IPEndPoint endpoint, GreyListManager greyListManager, MotdProvider motdProvider, DedicatedThreadPool threadPool = null)
		{
			_endpoint = endpoint ?? new IPEndPoint(IPAddress.Any, 0);

			_greyListManager = greyListManager;
			_motdProvider = motdProvider;

			_receiveThreadPool = threadPool ?? new DedicatedThreadPool(new DedicatedThreadPoolSettings(100, "Datagram Rcv Thread"));

			ServerInfo = new ServerInfo(_rakSessions);

			_rakProcessor = new RakProcessor(this, this, _greyListManager, _motdProvider, ServerInfo);
		}

		public void Start()
		{
			if (_listener != null) return;

			_listener = CreateListener(_endpoint);
			_receiveThread = new Thread(ReceiveDatagram) {IsBackground = true};
			_receiveThread.Start(_listener);

			_tickerHighPrecisionTimer = new HighPrecisionTimer(10, SendTick, true);
		}

		public void Stop()
		{
			try
			{
				Log.Info("Shutting down...");

				foreach (var rakSession in _rakSessions)
				{
					rakSession.Value.Close();
				}

				var timer = _tickerHighPrecisionTimer;
				_tickerHighPrecisionTimer = null;
				timer?.Dispose();

				var listener = _listener;
				if (listener == null) return;

				_listener = null;
				listener.Close();
			}
			catch (Exception e)
			{
				Log.Error(e);
			}
		}


		private static UdpClient CreateListener(IPEndPoint endpoint)
		{
			var listener = new UdpClient();

			//_listener.Client.ReceiveBufferSize = 1600*40000;
			listener.Client.ReceiveBufferSize = int.MaxValue;
			//_listener.Client.SendBufferSize = 1600*40000;
			listener.Client.SendBufferSize = int.MaxValue;
			listener.DontFragment = false;
			listener.EnableBroadcast = true;

			if (Environment.OSVersion.Platform != PlatformID.Unix && Environment.OSVersion.Platform != PlatformID.MacOSX)
			{
				// SIO_UDP_CONNRESET (opcode setting: I, T==3)
				// Windows:  Controls whether UDP PORT_UNREACHABLE messages are reported.
				// - Set to TRUE to enable reporting.
				// - Set to FALSE to disable reporting.

				uint IOC_IN = 0x80000000;
				uint IOC_VENDOR = 0x18000000;
				uint SIO_UDP_CONNRESET = IOC_IN | IOC_VENDOR | 12;
				listener.Client.IOControl((int) SIO_UDP_CONNRESET, new byte[] {Convert.ToByte(false)}, null);

				//
				//WARNING: We need to catch errors here to remove the code above.
				//
			}

			listener.Client.Bind(endpoint);
			return listener;
		}


		private void ReceiveDatagram(object state)
		{
			var listener = (UdpClient) state;

			while (true)
			{
				// Check if we already closed the server
				if (listener?.Client == null) return;

				// WSAECONNRESET:
				// The virtual circuit was reset by the remote side executing a hard or abortive close. 
				// The application should close the socket; it is no longer usable. On a UDP-datagram socket 
				// this error indicates a previous send operation resulted in an ICMP Port Unreachable message.
				// Note the spocket settings on creation of the server. It makes us ignore these resets.
				IPEndPoint senderEndpoint = null;
				try
				{
					ReadOnlyMemory<byte> receiveBytes = listener.Receive(ref senderEndpoint);
					//UdpReceiveResult result = listener.ReceiveAsync().Result;
					//senderEndpoint = result.RemoteEndPoint;
					//byte[] receiveBytes = result.Buffer;

					Interlocked.Increment(ref ServerInfo.NumberOfPacketsInPerSecond);
					Interlocked.Add(ref ServerInfo.TotalPacketSizeInPerSecond, receiveBytes.Length);

					if (receiveBytes.Length != 0)
					{
						_receiveThreadPool.QueueUserWorkItem(() =>
						{
							try
							{
								if (_greyListManager != null)
								{
									if (!_greyListManager.IsWhitelisted(senderEndpoint.Address) && _greyListManager.IsBlacklisted(senderEndpoint.Address)) return;
									if (_greyListManager.IsGreylisted(senderEndpoint.Address)) return;
								}

								ProcessMessage(receiveBytes, senderEndpoint);
							}
							catch (Exception e)
							{
								Log.Warn($"Process message error from: {senderEndpoint.Address}", e);
							}
						});
					}
					else
					{
						Log.Warn("Unexpected end of transmission?");
						continue;
					}
				}
				catch (SocketException e)
				{
					if (e.ErrorCode != 10004) Log.Error("Unexpected end of receive", e);

					if (listener.Client != null) continue;

					return;
				}
			}
		}

		private void ProcessMessage(ReadOnlyMemory<byte> receivedBytes, IPEndPoint clientEndpoint)
		{
			var header = new DatagramHeader(receivedBytes.Span[0]);

			if (!header.IsValid)
			{
				// We parse as an offline message. This is not actually correct, but works.

				byte messageId = receivedBytes.Span[0];

				if (messageId <= (byte) DefaultMessageIdTypes.ID_USER_PACKET_ENUM)
				{
					_rakProcessor.HandleOfflineRakMessage(receivedBytes, clientEndpoint);
				}
				else
				{
					Log.Warn($"Receive invalid message, but not a RakNet message. Message ID={messageId}. Ignoring.");
				}

				return;
			}

			if (!_rakSessions.TryGetValue(clientEndpoint, out RakSession rakSession))
			{
				//Log.DebugFormat("Receive MCPE message 0x{1:x2} without session {0}", senderEndpoint.Address, msgId);
				//if (!_badPacketBans.ContainsKey(senderEndpoint.Address))
				//{
				//	_badPacketBans.Add(senderEndpoint.Address, true);
				//}
				return;
			}

			if (rakSession.CustomMessageHandler == null)
			{
				Log.ErrorFormat("Receive online message without message handler for IP={0}. Session removed.", clientEndpoint.Address);
				//_rakSessions.TryRemove(clientEndpoint, out rakSession);
				//if (!_badPacketBans.ContainsKey(senderEndpoint.Address))
				//{
				//	_badPacketBans.Add(senderEndpoint.Address, true);
				//}
				//return;
			}

			if (rakSession.Evicted) return;

			rakSession.LastUpdatedTime = DateTime.UtcNow;

			if (header.IsAck)
			{
				if (ServerInfo.IsEmulator) return;

				rakSession.HandleAck(receivedBytes, ServerInfo);
				return;
			}

			if (header.IsNak)
			{
				if (ServerInfo.IsEmulator) return;

				rakSession.HandleNak(receivedBytes, ServerInfo);
				return;
			}

			if (ServerInfo.IsEmulator)
			{
				if (_tickerHighPrecisionTimer != null)
				{
					var timer = _tickerHighPrecisionTimer;
					_tickerHighPrecisionTimer = null;
					timer?.Dispose();
				}

				var datagramSequenceNumber = new Int24(receivedBytes.Span.Slice(1, 3));

				var acks = Acks.CreateObject();
				acks.acks.Add(datagramSequenceNumber);
				byte[] data = acks.Encode();
				SendData(data, clientEndpoint);
				return;
			}

			var datagram = Datagram.CreateObject();
			try
			{
				datagram.Decode(receivedBytes);
			}
			catch (Exception e)
			{
				rakSession.Disconnect("Bad packet received from client.");

				Log.Warn($"Bad packet {receivedBytes.Span[0]}\n{Packet.HexDump(receivedBytes)}", e);

				_greyListManager.Blacklist(clientEndpoint.Address);

				return;
			}

			rakSession.OutgoingAckQueue.Enqueue(datagram.Header.DatagramSequenceNumber);

			rakSession.HandleDatagram(datagram);
			datagram.PutPool();
		}

		private async void SendTick(object obj)
		{
			//var watch = Stopwatch.StartNew();

			var tasks = new List<Task>();
			foreach (KeyValuePair<IPEndPoint, RakSession> session in _rakSessions)
			{
				tasks.Add(session.Value.SendTickAsync());
			}
			await Task.WhenAll(tasks);

			//long duration = watch.ElapsedMilliseconds;
			//if (duration > 10) Log.Warn($"Ticker thread exceeded max time. Took {watch.ElapsedMilliseconds}ms for {_playerSessions.Count} sessions.");
		}

		public void SendData(byte[] data, IPEndPoint targetEndPoint)
		{
			try
			{
				_listener.Send(data, data.Length, targetEndPoint); // Less thread-issues it seems

				Interlocked.Increment(ref ServerInfo.NumberOfPacketsOutPerSecond);
				Interlocked.Add(ref ServerInfo.TotalPacketSizeOutPerSecond, data.Length);
			}
			catch (ObjectDisposedException e)
			{
			}
			catch (Exception e)
			{
				//if (_listener == null || _listener.Client != null) Log.Error(string.Format("Send data lenght: {0}", data.Length), e);
			}
		}

		public async Task SendDataAsync(byte[] data, IPEndPoint targetEndPoint)
		{
			await SendDataAsync(data, data.Length, targetEndPoint);
		}

		public async Task SendDataAsync(byte[] data, int length, IPEndPoint targetEndPoint)
		{
			try
			{
				await _listener.SendAsync(data, length, targetEndPoint);
				//_listener.Send(data, length, targetEndPoint);

				Interlocked.Increment(ref ServerInfo.NumberOfPacketsOutPerSecond);
				Interlocked.Add(ref ServerInfo.TotalPacketSizeOutPerSecond, length);
			}
			catch (ObjectDisposedException e)
			{
				Log.Warn(e);
			}
			catch (Exception e)
			{
				Log.Warn(e);
				//if (_listener == null || _listener.Client != null) Log.Error(string.Format("Send data lenght: {0}", data.Length), e);
			}
		}
	}
}