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
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using Microsoft.IO;
using MiNET.Net;
using MiNET.Net.RakNet;
using MiNET.Plugins;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET
{
	public class MiNetServer
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(MiNetServer));

		public const string MiNET = "\r\n __   __  ___   __    _  _______  _______ \r\n|  |_|  ||   | |  |  | ||       ||       |\r\n|       ||   | |   |_| ||    ___||_     _|\r\n|       ||   | |       ||   |___   |   |  \r\n|       ||   | |  _    ||    ___|  |   |  \r\n| ||_|| ||   | | | |   ||   |___   |   |  \r\n|_|   |_||___| |_|  |__||_______|  |___|  \r\n";

		private const int DefaultPort = 19132;

		public IPEndPoint Endpoint { get; private set; }
		private UdpClient _listener;
		internal ConcurrentDictionary<IPEndPoint, RakSession> _rakNetSessions = new ConcurrentDictionary<IPEndPoint, RakSession>();

		public MotdProvider MotdProvider { get; set; }

		public static RecyclableMemoryStreamManager MemoryStreamManager { get; set; } = new RecyclableMemoryStreamManager();

		public IServerManager ServerManager { get; set; }
		public LevelManager LevelManager { get; set; }
		public PlayerFactory PlayerFactory { get; set; }
		public GreylistManager GreylistManager { get; set; }

		public bool IsEdu { get; set; } = Config.GetProperty("EnableEdu", false);
		public EduTokenManager EduTokenManager { get; set; }

		public PluginManager PluginManager { get; set; }
		public SessionManager SessionManager { get; set; }

		private Timer _internalPingTimer;
		private Timer _cleanerTimer;

		public int InacvitityTimeout { get; private set; }
		public int ResendThreshold { get; private set; }

		public ServerInfo ServerInfo { get; set; }

		public ServerRole ServerRole { get; set; } = ServerRole.Full;

		internal static DedicatedThreadPool FastThreadPool { get; set; }
		internal static DedicatedThreadPool LevelThreadPool { get; set; }

		public MiNetServer()
		{
			ServerRole = Config.GetProperty("ServerRole", ServerRole.Full);
			InacvitityTimeout = Config.GetProperty("InactivityTimeout", 8500);
			ResendThreshold = Config.GetProperty("ResendThreshold", 10);

			int confMinWorkerThreads = Config.GetProperty("MinWorkerThreads", -1);
			int confMinCompletionPortThreads = Config.GetProperty("MinCompletionPortThreads", -1);

			int threads;
			int iothreads;
			ThreadPool.GetMinThreads(out threads, out iothreads);

			//if (confMinWorkerThreads != -1) threads = confMinWorkerThreads;
			//else threads *= 4;

			//if (confMinCompletionPortThreads != -1) iothreads = confMinCompletionPortThreads;
			//else iothreads *= 4;

			//ThreadPool.SetMinThreads(threads, iothreads);
			FastThreadPool = new DedicatedThreadPool(new DedicatedThreadPoolSettings(Environment.ProcessorCount));
			LevelThreadPool = new DedicatedThreadPool(new DedicatedThreadPoolSettings(Environment.ProcessorCount));
			_receiveThreadPool = new DedicatedThreadPool(new DedicatedThreadPoolSettings(Environment.ProcessorCount));
		}

		public MiNetServer(IPEndPoint endpoint) : base()
		{
			Endpoint = endpoint;
		}

		public static void DisplayTimerProperties()
		{
			Console.WriteLine($"Are you blessed with HW accelerated vectors? {(Vector.IsHardwareAccelerated ? "Yep!" : "Nope, sorry :-(")}");

			// Display the timer frequency and resolution.
			if (Stopwatch.IsHighResolution)
			{
				Console.WriteLine("Operations timed using the system's high-resolution performance counter.");
			}
			else
			{
				Console.WriteLine("Operations timed using the DateTime class.");
			}

			long frequency = Stopwatch.Frequency;
			Console.WriteLine("  Timer frequency in ticks per second = {0}",
				frequency);
			long nanosecPerTick = (1000L * 1000L * 1000L) / frequency;
			Console.WriteLine("  Timer is accurate within {0} nanoseconds",
				nanosecPerTick);
		}

		public bool StartServer()
		{
			DisplayTimerProperties();

			if (_listener != null) return false; // Already started

			try
			{
				Log.Info("Initializing...");

				if (ServerRole == ServerRole.Full || ServerRole == ServerRole.Proxy)
				{
					if (IsEdu) EduTokenManager = new EduTokenManager();

					if (Endpoint == null)
					{
						var ip = IPAddress.Parse(Config.GetProperty("ip", "0.0.0.0"));
						int port = Config.GetProperty("port", DefaultPort);
						Endpoint = new IPEndPoint(ip, port);
					}
				}

				ServerManager ??= new DefaultServerManager(this);

				if (ServerRole == ServerRole.Full || ServerRole == ServerRole.Node)
				{
					Log.Info("Loading plugins...");
					PluginManager = new PluginManager();
					PluginManager.LoadPlugins();
					Log.Info("Plugins loaded!");

					// Bootstrap server
					PluginManager.ExecuteStartup(this);

					GreylistManager ??= new GreylistManager(this);
					SessionManager ??= new SessionManager();
					LevelManager ??= new LevelManager();
					//LevelManager ??= new SpreadLevelManager(75);
					PlayerFactory ??= new PlayerFactory();

					PluginManager.EnablePlugins(this, LevelManager);

					// Cache - remove
					LevelManager.GetLevel(null, Dimension.Overworld.ToString());
				}

				GreylistManager ??= new GreylistManager(this);
				MotdProvider ??= new MotdProvider();

				if (ServerRole == ServerRole.Full || ServerRole == ServerRole.Proxy)
				{
					_listener = CreateListener();

					new Thread(Receive) {IsBackground = true}.Start(_listener);
				}

				ServerInfo = new ServerInfo(LevelManager, _rakNetSessions) {MaxNumberOfPlayers = Config.GetProperty("MaxNumberOfPlayers", 10)};
				ServerInfo.MaxNumberOfConcurrentConnects = Config.GetProperty("MaxNumberOfConcurrentConnects", ServerInfo.MaxNumberOfPlayers);

				_tickerHighPrecisionTimer = new HighPrecisionTimer(10, SendTick, true);

				Log.Info("Server open for business on port " + Endpoint?.Port + " ...");

				return true;
			}
			catch (Exception e)
			{
				Log.Error("Error during startup!", e);
				StopServer();
			}

			return false;
		}

		private async void SendTick(object obj)
		{
			//var watch = Stopwatch.StartNew();
			var tasks = new List<Task>();
			foreach (var session in _rakNetSessions)
			{
				tasks.Add(session.Value.SendTickAsync());
			}
			await Task.WhenAll(tasks);

			//long duration = watch.ElapsedMilliseconds;
			//if (duration > 10) Log.Warn($"Ticker thread exceeded max time. Took {watch.ElapsedMilliseconds}ms for {_playerSessions.Count} sessions.");
		}

		private UdpClient CreateListener()
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

			//_cleanerTimer = new Timer(Update, null, 10, Timeout.Infinite);

			listener.Client.Bind(Endpoint);
			return listener;
		}

		public bool StopServer()
		{
			try
			{
				Log.Info("Disabling plugins...");
				PluginManager?.DisablePlugins();

				Log.Info("Shutting down...");
				if (_listener == null) return true; // Already stopped. It's ok.

				_listener.Close();
				_listener = null;

				return true;
			}
			catch (Exception e)
			{
				Log.Error(e);
			}

			return false;
		}

		private void Receive(object state)
		{
			var listener = (UdpClient) state;

			while (true)
			{
				// Check if we already closed the server
				if (listener.Client == null) return;

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
								if (!GreylistManager.IsWhitelisted(senderEndpoint.Address) && GreylistManager.IsBlacklisted(senderEndpoint.Address)) return;
								if (GreylistManager.IsGreylisted(senderEndpoint.Address)) return;

								ProcessMessage(this, receiveBytes, senderEndpoint);
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

		private static void ProcessMessage(MiNetServer server, ReadOnlyMemory<byte> receivedBytes, IPEndPoint clientEndpoint)
		{
			var header = new DatagramHeader(receivedBytes.Span[0]);

			if (!header.IsValid)
			{
				// We parse as an offline message. This is not actually correct, but works.

				byte messageId = receivedBytes.Span[0];

				if (messageId <= (byte) DefaultMessageIdTypes.ID_USER_PACKET_ENUM)
				{
					RakNetProcessor.HandleOfflineRakMessage(server, receivedBytes, clientEndpoint, messageId, server.ServerInfo);
				}
				else
				{
					Log.Warn($"Receive invalid message, but not a RakNet message. Message ID={messageId}. Ignoring.");
				}

				return;
			}

			if (!server._rakNetSessions.TryGetValue(clientEndpoint, out RakSession rakNetSession))
			{
				//Log.DebugFormat("Receive MCPE message 0x{1:x2} without session {0}", senderEndpoint.Address, msgId);
				//if (!_badPacketBans.ContainsKey(senderEndpoint.Address))
				//{
				//	_badPacketBans.Add(senderEndpoint.Address, true);
				//}
				return;
			}

			if (rakNetSession.MessageHandler == null)
			{
				Log.ErrorFormat("Receive online message without message handler for IP={0}. Session removed.", clientEndpoint.Address);
				server._rakNetSessions.TryRemove(clientEndpoint, out rakNetSession);
				//if (!_badPacketBans.ContainsKey(senderEndpoint.Address))
				//{
				//	_badPacketBans.Add(senderEndpoint.Address, true);
				//}
				return;
			}

			if (rakNetSession.Evicted) return;

			rakNetSession.LastUpdatedTime = DateTime.UtcNow;

			if (header.IsAck)
			{
				rakNetSession.HandleAck(receivedBytes, server.ServerInfo);
				return;
			}

			if (header.IsNak)
			{
				rakNetSession.HandleNak(receivedBytes, server.ServerInfo);
				return;
			}

			var datagram = ConnectedPacket.CreateObject();
			try
			{
				datagram.Decode(receivedBytes);
			}
			catch (Exception e)
			{
				rakNetSession.Disconnect("Bad packet received from client.");

				Log.Warn($"Bad packet {receivedBytes.Span[0]}\n{Packet.HexDump(receivedBytes)}", e);

				server.GreylistManager.Blacklist(clientEndpoint.Address);

				return;
			}

			// IF reliable code below is enabled, useItem start sending doubles
			// for some unknown reason.

			//Reliability reliability = packet._reliability;
			//if (reliability == Reliability.Reliable
			//	|| reliability == Reliability.ReliableSequenced
			//	|| reliability == Reliability.ReliableOrdered
			//	)
			{
				server.EnqueueAck(rakNetSession, datagram.Header.DatagramSequenceNumber);
			}

			rakNetSession.HandleDatagram(datagram);
			datagram.PutPool();
		}

		public List<Packet> ParseDatagram(Span<byte> buffer)
		{

			return null;
		}

		internal ConcurrentDictionary<IPEndPoint, DateTime> _connectionAttemps = new ConcurrentDictionary<IPEndPoint, DateTime>();
		private DedicatedThreadPool _receiveThreadPool;
		private HighPrecisionTimer _tickerHighPrecisionTimer;

		private void EnqueueAck(RakSession session, int sequenceNumber)
		{
			session.OutgoingAckQueue.Enqueue(sequenceNumber);
			session.SignalTick();
		}

		public async Task SendPacketAsync(RakSession session, Packet message)
		{
			foreach (Datagram datagram in Datagram.CreateDatagrams(message, session.MtuSize, session))
			{
				await SendDatagramAsync(session, datagram);
			}

			message.PutPool();
		}

		internal async Task SendDatagramAsync(RakSession session, Datagram datagram)
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
				await SendDataAsync(buffer, length, session.EndPoint);
				ArrayPool<byte>.Shared.Return(buffer);
			}
		}

		private async Task SendDataAsync(byte[] data, int length, IPEndPoint targetEndPoint)
		{
			try
			{
				await _listener.SendAsync(data, length, targetEndPoint); // Less thread-issues it seems

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


		internal void SendData(byte[] data, IPEndPoint targetEndPoint)
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

		internal async Task SendDataAsync(byte[] data, IPEndPoint targetEndPoint)
		{
			try
			{
				await _listener.SendAsync(data, data.Length, targetEndPoint); // Less thread-issues it seems

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
	}

	public enum ServerRole
	{
		Node,
		Proxy,
		Full,
	}
}