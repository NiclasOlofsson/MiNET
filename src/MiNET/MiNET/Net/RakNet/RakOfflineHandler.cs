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
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using log4net;
using MiNET.Utils;
using MiNET.Utils.Nbt;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MiNET.Net.RakNet
{
	public sealed class RakOfflineHandler
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(RakOfflineHandler));

		public const int UdpHeaderSize = 28;

		public static short MtuSize { get; set; } = 1500;
		public static short MaxMtuSize { get; } = 1500;

		private readonly IPacketSender _sender;
		private readonly RakConnection _connection;
		private readonly MotdProvider _motdProvider;
		private readonly ConnectionInfo _connectionInfo;
		private readonly GreyListManager _greyListManager;
		private readonly ConcurrentDictionary<IPEndPoint, DateTime> _connectionAttempts = new ConcurrentDictionary<IPEndPoint, DateTime>();

		public long ClientGuid { get; }

		// RakNet found a remote server using Ping.
		public bool HaveServer { get; set; }

		// Tell RakNet to automatically connect to any found server.
		public bool AutoConnect { get; set; } = true;

		internal RakOfflineHandler(RakConnection connection, IPacketSender sender, GreyListManager greyListManager, MotdProvider motdProvider, ConnectionInfo connectionInfo)
		{
			_sender = sender;
			_connection = connection;
			_motdProvider = motdProvider;
			_connectionInfo = connectionInfo;
			_greyListManager = greyListManager;

			byte[] buffer = new byte[8];
			new Random().NextBytes(buffer);
			ClientGuid = BitConverter.ToInt64(buffer, 0);
		}

		internal void HandleOfflineRakMessage(ReadOnlyMemory<byte> receiveBytes, IPEndPoint senderEndpoint)
		{
			byte messageId = receiveBytes.Span[0];
			var messageType = (DefaultMessageIdTypes) messageId;

			// Increase fast, decrease slow on 1s ticks.
			if (_connectionInfo.NumberOfPlayers < _connectionInfo.RakSessions.Count) _connectionInfo.NumberOfPlayers = _connectionInfo.RakSessions.Count;

			// Shortcut to reply fast, and no parsing
			if (messageType == DefaultMessageIdTypes.ID_OPEN_CONNECTION_REQUEST_1)
			{
				if (!_greyListManager.AcceptConnection(senderEndpoint.Address))
				{
					var noFree = NoFreeIncomingConnections.CreateObject();
					var bytes = noFree.Encode();

					TraceSend(noFree);

					noFree.PutPool();

					_sender.SendData(bytes, senderEndpoint);
					Interlocked.Increment(ref _connectionInfo.NumberOfDeniedConnectionRequestsPerSecond);
					return;
				}
			}

			Packet message = null;
			try
			{
				try
				{
					message = PacketFactory.Create(messageId, receiveBytes, "raknet");
				}
				catch (Exception)
				{
					message = null;
				}

				if (message == null)
				{
					_greyListManager.Blacklist(senderEndpoint.Address);
					Log.Error($"Receive bad packet with ID: {messageId} (0x{messageId:x2}) {messageType} from {senderEndpoint.Address}");

					return;
				}

				TraceReceive(Log, message);

				switch (messageType)
				{
					case DefaultMessageIdTypes.ID_NO_FREE_INCOMING_CONNECTIONS:
						// Stop this client connection
						_connection.Stop();
						break;
					case DefaultMessageIdTypes.ID_UNCONNECTED_PING:
					case DefaultMessageIdTypes.ID_UNCONNECTED_PING_OPEN_CONNECTIONS:
						HandleRakNetMessage(senderEndpoint, (UnconnectedPing) message);
						break;
					case DefaultMessageIdTypes.ID_UNCONNECTED_PONG:
						HandleRakNetMessage(senderEndpoint, (UnconnectedPong) message);
						break;
					case DefaultMessageIdTypes.ID_OPEN_CONNECTION_REQUEST_1:
						HandleRakNetMessage(senderEndpoint, (OpenConnectionRequest1) message);
						break;
					case DefaultMessageIdTypes.ID_OPEN_CONNECTION_REPLY_1:
						HandleRakNetMessage(senderEndpoint, (OpenConnectionReply1) message);
						break;
					case DefaultMessageIdTypes.ID_OPEN_CONNECTION_REQUEST_2:
						HandleRakNetMessage(senderEndpoint, (OpenConnectionRequest2) message);
						break;
					case DefaultMessageIdTypes.ID_OPEN_CONNECTION_REPLY_2:
						HandleRakNetMessage(senderEndpoint, (OpenConnectionReply2) message);
						break;
					default:
						_greyListManager.Blacklist(senderEndpoint.Address);
						if (Log.IsInfoEnabled) Log.Error($"Receive unexpected packet with ID: {messageId} (0x{messageId:x2}) {messageType} from {senderEndpoint.Address}");
						break;
				}
			}
			finally
			{
				message?.PutPool();
			}
		}

		private void HandleRakNetMessage(IPEndPoint senderEndpoint, UnconnectedPing message)
		{
			//TODO: This needs to be verified with RakNet first
			//response.sendpingtime = msg.sendpingtime;
			//response.sendpongtime = DateTimeOffset.UtcNow.Ticks / TimeSpan.TicksPerMillisecond;

			//if (sender.IsEdu)
			//{
			//	var packet = UnconnectedPong.CreateObject();
			//	packet.serverId = sender.MotdProvider.ServerId;
			//	packet.pingId = incoming.pingId;
			//	packet.serverName = sender.MotdProvider.GetMotd(sender.ServerInfo, senderEndpoint, true);
			//	var data = packet.Encode();

			//	TraceSend(packet);

			//	packet.PutPool();

			//	sender.SendData(data, senderEndpoint);
			//}

			{
				Log.Debug($"Ping from: {senderEndpoint.Address}:{senderEndpoint.Port}");

				var packet = UnconnectedPong.CreateObject();
				packet.serverId = _motdProvider.ServerId;
				packet.pingId = message.pingId;
				packet.serverName = _motdProvider.GetMotd(_connectionInfo, senderEndpoint);
				byte[] data = packet.Encode();

				TraceSend(packet);

				packet.PutPool();

				_sender.SendData(data, senderEndpoint);
			}
		}

		public void HandleRakNetMessage(IPEndPoint senderEndpoint, UnconnectedPong message)
		{
			Log.Debug($"Found server at {senderEndpoint}");
			Log.Debug($"MOTD: {message.serverName}");

			if (!HaveServer)
			{
				string[] motdParts = message.serverName.Split(';');
				if (motdParts.Length >= 11)
				{
					senderEndpoint.Port = int.Parse(motdParts[10]);
				}

				if (AutoConnect)
				{
					Log.Debug($"Connecting to {senderEndpoint}");
					HaveServer = true;
					SendOpenConnectionRequest1(senderEndpoint, MtuSize);
				}
				else
				{
					Log.Debug($"Connect to server using actual endpoint={senderEndpoint}");
					_connection.RemoteEndpoint = senderEndpoint;
					_connection.RemoteServerName = message.serverName;
					HaveServer = true;
				}
			}
		}

		public void SendOpenConnectionRequest1(IPEndPoint targetEndPoint, short mtuSize)
		{
			MtuSize = mtuSize; // This is what we will use from connections this point forward

			var packet = OpenConnectionRequest1.CreateObject();
			packet.raknetProtocolVersion = 10;
			packet.mtuSize = mtuSize;

			byte[] data = packet.Encode();

			TraceSend(packet);

			Log.Debug($"Sending MTU size={mtuSize}, data length={data.Length}");
			_sender.SendData(data, targetEndPoint);
		}

		private void HandleRakNetMessage(IPEndPoint senderEndpoint, OpenConnectionRequest1 message)
		{
			ConcurrentDictionary<IPEndPoint, RakSession> sessions = _connectionInfo.RakSessions;
			ConcurrentDictionary<IPEndPoint, DateTime> connectionAttempts = _connectionAttempts;

			if (Log.IsDebugEnabled) Log.Warn($"New connection from {senderEndpoint.Address} {senderEndpoint.Port}, MTU={message.mtuSize}, RakNet version={message.raknetProtocolVersion}");

			lock (sessions)
			{
				// Already connecting, then this is just a duplicate
				if (connectionAttempts.TryGetValue(senderEndpoint, out DateTime created))
				{
					if (DateTime.UtcNow < created + TimeSpan.FromSeconds(3)) return;

					connectionAttempts.TryRemove(senderEndpoint, out _);
				}

				if (!connectionAttempts.TryAdd(senderEndpoint, DateTime.UtcNow)) return;
			}

			if (message.mtuSize > MaxMtuSize) return;

			var packet = OpenConnectionReply1.CreateObject();
			packet.serverGuid = _motdProvider.ServerId;
			packet.mtuSize = message.mtuSize;
			packet.serverHasSecurity = 0;
			byte[] data = packet.Encode();

			TraceSend(packet);

			packet.PutPool();

			_sender.SendData(data, senderEndpoint);
		}

		private void HandleRakNetMessage(IPEndPoint senderEndpoint, OpenConnectionReply1 message)
		{
			if (message.mtuSize != MtuSize)
			{
				Log.Debug($"Error, mtu differ from what we sent. Received {message.mtuSize} bytes");
				//return;
			}

			Log.Debug($"Server with ID {message.serverGuid} security={message.serverHasSecurity}, mtu agreed on {message.mtuSize}");

			SendOpenConnectionRequest2(senderEndpoint, message.mtuSize);
		}

		private void SendOpenConnectionRequest2(IPEndPoint targetEndPoint, short mtuSize)
		{
			var packet = OpenConnectionRequest2.CreateObject();
			packet.remoteBindingAddress = targetEndPoint;
			packet.mtuSize = mtuSize;
			packet.clientGuid = ClientGuid;

			byte[] data = packet.Encode();

			TraceSend(packet);

			_sender.SendData(data, targetEndPoint);
		}

		private void HandleRakNetMessage(IPEndPoint senderEndpoint, OpenConnectionRequest2 incoming)
		{
			ConcurrentDictionary<IPEndPoint, RakSession> sessions = _connectionInfo.RakSessions;
			ConcurrentDictionary<IPEndPoint, DateTime> connectionAttempts = _connectionAttempts;

			lock (sessions)
			{
				if (!connectionAttempts.ContainsKey(senderEndpoint))
				{
					Log.Warn($"Unexpected connection request packet from {senderEndpoint}. Probably a resend.");
					return;
				}

				if (sessions.TryGetValue(senderEndpoint, out _))
				{
					Log.Warn($"Trying to create session where session already exist. Please wait for timeout on {senderEndpoint}. Ignoring this request.");
					return;
				}

				var session = new RakSession(_connectionInfo, _sender, senderEndpoint, incoming.mtuSize)
				{
					State = ConnectionState.Connecting,
					LastUpdatedTime = DateTime.UtcNow,
					MtuSize = incoming.mtuSize,
					NetworkIdentifier = incoming.clientGuid,
				};

				session.CustomMessageHandler = _connection.CustomMessageHandlerFactory?.Invoke(session);

				sessions.TryAdd(senderEndpoint, session);
			}

			var reply = OpenConnectionReply2.CreateObject();
			reply.serverGuid = _motdProvider.ServerId;
			reply.clientEndpoint = senderEndpoint;
			reply.mtuSize = incoming.mtuSize;
			reply.doSecurityAndHandshake = new byte[1];
			byte[] data = reply.Encode();

			TraceSend(reply);

			reply.PutPool();

			_sender.SendData(data, senderEndpoint);
		}

		private void HandleRakNetMessage(IPEndPoint senderEndpoint, OpenConnectionReply2 message)
		{
			Log.Debug("MTU Size: " + message.mtuSize);
			Log.Debug("Client Endpoint: " + message.clientEndpoint);

			HaveServer = true;

			SendConnectionRequest(senderEndpoint, message.mtuSize);
		}

		private void SendConnectionRequest(IPEndPoint targetEndPoint, short mtuSize)
		{
			ConcurrentDictionary<IPEndPoint, RakSession> sessions = _connectionInfo.RakSessions;

			RakSession session;
			lock (sessions)
			{
				if (sessions.ContainsKey(targetEndPoint))
				{
					Log.Debug($"Session already exist, ignoring");
					return;
				}

				session = new RakSession(_connectionInfo, _sender, targetEndPoint, mtuSize)
				{
					State = ConnectionState.Connecting,
					LastUpdatedTime = DateTime.UtcNow,
					NetworkIdentifier = ClientGuid,
				};

				session.CustomMessageHandler = _connection.CustomMessageHandlerFactory?.Invoke(session);

				if (!sessions.TryAdd(targetEndPoint, session))
				{
					Log.Debug($"Session already exist, ignoring");
					return;
				}
			}

			var packet = ConnectionRequest.CreateObject();
			packet.clientGuid = ClientGuid;
			packet.timestamp = DateTime.UtcNow.Ticks;
			packet.doSecurity = 0;

			session.SendPacket(packet);
		}

		internal static void TraceReceive(ILog log, Packet message)
		{
			if (!Log.IsTraceEnabled()) return;

			try
			{
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
					log.Trace($"> Receive: {message.Id} (0x{message.Id:x2}): {message.GetType().Name}");
				}
				else if (verbosity == 1 || verbosity == 3)
				{
					var jsonSerializerSettings = new JsonSerializerSettings
					{
						PreserveReferencesHandling = PreserveReferencesHandling.Arrays,
						TypeNameHandling = TypeNameHandling.Auto,
						Formatting = Formatting.Indented,
						ReferenceLoopHandling = ReferenceLoopHandling.Ignore
					};

					jsonSerializerSettings.Converters.Add(new StringEnumConverter());
					jsonSerializerSettings.Converters.Add(new NbtIntConverter());
					jsonSerializerSettings.Converters.Add(new NbtStringConverter());
					jsonSerializerSettings.Converters.Add(new IPAddressConverter());
					jsonSerializerSettings.Converters.Add(new IPEndPointConverter());

					string result = JsonConvert.SerializeObject(message, jsonSerializerSettings);
					log.Trace($"> Receive: {message.Id} (0x{message.Id:x2}): {message.GetType().Name}\n{result}");
				}
				else if (verbosity == 2 || verbosity == 3)
				{
					log.Verbose($"> Receive: {message.Id} (0x{message.Id:x2}): {message.GetType().Name}\n{Packet.HexDump(message.Bytes)}");
				}
			}
			catch (Exception e)
			{
				log.Error("Error when printing trace", e);
			}
		}

		internal static void TraceSend(Packet message)
		{
			if (!Log.IsTraceEnabled()) return;

			try
			{
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
					Log.Trace($"<    Send: {message.Id} (0x{message.Id:x2}): {message.GetType().Name}");
				}
				else if (verbosity == 1 || verbosity == 3)
				{
					var jsonSerializerSettings = new JsonSerializerSettings
					{
						PreserveReferencesHandling = PreserveReferencesHandling.Arrays,
						TypeNameHandling = TypeNameHandling.Auto,
						Formatting = Formatting.Indented,
						DefaultValueHandling = DefaultValueHandling.Include,
						ReferenceLoopHandling = ReferenceLoopHandling.Ignore
					};

					jsonSerializerSettings.Converters.Add(new StringEnumConverter());
					jsonSerializerSettings.Converters.Add(new NbtIntConverter());
					jsonSerializerSettings.Converters.Add(new NbtStringConverter());
					jsonSerializerSettings.Converters.Add(new IPAddressConverter());
					jsonSerializerSettings.Converters.Add(new IPEndPointConverter());

					string result = JsonConvert.SerializeObject(message, jsonSerializerSettings);
					Log.Trace($"<    Send: {message.Id} (0x{message.Id:x2}): {message.GetType().Name}\n{result}");
				}
				else if (verbosity == 2 || verbosity == 3)
				{
					Log.Verbose($"<    Send: {message.Id} (0x{message.Id:x2}): {message.GetType().Name}\n{Packet.HexDump(message.Bytes)}");
				}
			}
			catch (Exception e)
			{
				Log.Error("Error when printing trace", e);
			}
		}
	}
}