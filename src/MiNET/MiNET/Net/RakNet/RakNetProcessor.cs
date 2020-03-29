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
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using log4net;
using MiNET.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MiNET.Net.RakNet
{
	public class RakNetProcessor
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(RakNetProcessor));

		internal static void HandleOfflineRakMessage(MiNetServer server, ReadOnlyMemory<byte> receiveBytes, IPEndPoint senderEndpoint, byte msgId, ServerInfo serverInfo)
		{
			var messageType = (DefaultMessageIdTypes) msgId;

			// Increase fast, decrease slow on 1s ticks.
			if (serverInfo.NumberOfPlayers < serverInfo.PlayerSessions.Count) serverInfo.NumberOfPlayers = serverInfo.PlayerSessions.Count;

			// Shortcut to reply fast, and no parsing
			if (messageType == DefaultMessageIdTypes.ID_OPEN_CONNECTION_REQUEST_1)
			{
				if (!server.GreylistManager.AcceptConnection(senderEndpoint.Address))
				{
					var noFree = NoFreeIncomingConnections.CreateObject();
					var bytes = noFree.Encode();

					TraceSend(noFree);

					noFree.PutPool();

					server.SendData(bytes, senderEndpoint);
					Interlocked.Increment(ref serverInfo.NumberOfDeniedConnectionRequestsPerSecond);
					return;
				}
			}

			Packet message = null;
			try
			{
				try
				{
					message = PacketFactory.Create(msgId, receiveBytes, "raknet");
				}
				catch (Exception)
				{
					message = null;
				}

				if (message == null)
				{
					server.GreylistManager.Blacklist(senderEndpoint.Address);
					Log.ErrorFormat("Receive bad packet with ID: {0} (0x{0:x2}) {2} from {1}", msgId, senderEndpoint.Address, (DefaultMessageIdTypes) msgId);

					return;
				}

				TraceReceive(message);

				switch (messageType)
				{
					case DefaultMessageIdTypes.ID_UNCONNECTED_PING:
					case DefaultMessageIdTypes.ID_UNCONNECTED_PING_OPEN_CONNECTIONS:
					{
						HandleRakNetMessage(server, senderEndpoint, (UnconnectedPing) message);
						break;
					}
					case DefaultMessageIdTypes.ID_OPEN_CONNECTION_REQUEST_1:
					{
						HandleRakNetMessage(server, senderEndpoint, (OpenConnectionRequest1) message);
						break;
					}
					case DefaultMessageIdTypes.ID_OPEN_CONNECTION_REQUEST_2:
					{
						HandleRakNetMessage(server, senderEndpoint, (OpenConnectionRequest2) message);
						break;
					}
					default:
						server.GreylistManager.Blacklist(senderEndpoint.Address);
						if (Log.IsInfoEnabled)
						{
							Log.ErrorFormat("Receive unexpected packet with ID: {0} (0x{0:x2}) {2} from {1}", msgId, senderEndpoint.Address, (DefaultMessageIdTypes) msgId);
						}
						break;
				}
			}
			finally
			{
				if (message != null) message.PutPool();
			}
		}

		public static void HandleRakNetMessage(MiNetServer server, IPEndPoint senderEndpoint, UnconnectedPing incoming)
		{
			//TODO: This needs to be verified with RakNet first
			//response.sendpingtime = msg.sendpingtime;
			//response.sendpongtime = DateTimeOffset.UtcNow.Ticks / TimeSpan.TicksPerMillisecond;

			if (server.IsEdu)
			{
				var packet = UnconnectedPong.CreateObject();
				packet.serverId = server.MotdProvider.ServerId;
				packet.pingId = incoming.pingId;
				packet.serverName = server.MotdProvider.GetMotd(server.ServerInfo, senderEndpoint, true);
				var data = packet.Encode();

				TraceSend(packet);

				packet.PutPool();

				server.SendData(data, senderEndpoint);
			}

			{
				Log.Debug($"Ping from: {senderEndpoint.Address.ToString()}:{senderEndpoint.Port}");

				var packet = UnconnectedPong.CreateObject();
				packet.serverId = server.MotdProvider.ServerId;
				packet.pingId = incoming.pingId;
				packet.serverName = server.MotdProvider.GetMotd(server.ServerInfo, senderEndpoint);
				var data = packet.Encode();

				TraceSend(packet);

				packet.PutPool();

				server.SendData(data, senderEndpoint);
			}

			return;
		}

		public static void HandleRakNetMessage(MiNetServer server, IPEndPoint senderEndpoint, OpenConnectionRequest1 incoming)
		{
			lock (server._rakNetSessions)
			{
				// Already connecting, then this is just a duplicate
				if (server._connectionAttemps.TryGetValue(senderEndpoint, out DateTime created))
				{
					if (DateTime.UtcNow < created + TimeSpan.FromSeconds(3)) return;

					server._connectionAttemps.TryRemove(senderEndpoint, out _);
				}

				if (!server._connectionAttemps.TryAdd(senderEndpoint, DateTime.UtcNow)) return;
			}

			if (Log.IsDebugEnabled) Log.Warn($"New connection from {senderEndpoint.Address} {senderEndpoint.Port}, MTU={incoming.mtuSize}, RakNet version={incoming.raknetProtocolVersion}");

			var packet = OpenConnectionReply1.CreateObject();
			packet.serverGuid = server.MotdProvider.ServerId;
			packet.mtuSize = incoming.mtuSize;
			packet.serverHasSecurity = 0;
			byte[] data = packet.Encode();

			TraceSend(packet);

			packet.PutPool();

			server.SendData(data, senderEndpoint);
		}

		public static void HandleRakNetMessage(MiNetServer server, IPEndPoint senderEndpoint, OpenConnectionRequest2 incoming)
		{
			RakSession session;
			lock (server._rakNetSessions)
			{
				if (!server._connectionAttemps.TryRemove(senderEndpoint, out _))
				{
					Log.WarnFormat("Unexpected connection request packet from {0}. Probably a resend.", senderEndpoint.Address);
					return;
				}

				if (server._rakNetSessions.TryGetValue(senderEndpoint, out session))
				{
					// Already connecting, then this is just a duplicate
					if (session.State == ConnectionState.Connecting /* && DateTime.UtcNow < session.LastUpdatedTime + TimeSpan.FromSeconds(2)*/)
					{
						return;
					}

					Log.InfoFormat("Unexpected session from {0}. Removing old session and disconnecting old player.", senderEndpoint.Address);

					session.Disconnect("Reconnecting.", false);

					server._rakNetSessions.TryRemove(senderEndpoint, out _);
				}

				session = new RakSession(server, null, senderEndpoint, incoming.mtuSize)
				{
					State = ConnectionState.Connecting,
					LastUpdatedTime = DateTime.UtcNow,
					MtuSize = incoming.mtuSize,
					NetworkIdentifier = incoming.clientGuid
				};

				server._rakNetSessions.TryAdd(senderEndpoint, session);
			}

			session.MessageHandler = new LoginMessageHandler(session);

			var reply = OpenConnectionReply2.CreateObject();
			reply.serverGuid = server.MotdProvider.ServerId;
			reply.clientEndpoint = senderEndpoint;
			reply.mtuSize = incoming.mtuSize;
			reply.doSecurityAndHandshake = new byte[1];
			byte[] data = reply.Encode();

			TraceSend(reply);

			reply.PutPool();

			server.SendData(data, senderEndpoint);
		}

		internal static void TraceReceive(Packet message)
		{
			if (!Log.IsDebugEnabled) return;

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
					Log.Debug($"> Receive: {message.Id} (0x{message.Id:x2}): {message.GetType().Name}");
				}
				else if (verbosity == 1 || verbosity == 3)
				{
					var jsonSerializerSettings = new JsonSerializerSettings
					{
						PreserveReferencesHandling = PreserveReferencesHandling.Arrays,
						TypeNameHandling = TypeNameHandling.Auto,
						Formatting = Formatting.Indented,
					};

					jsonSerializerSettings.Converters.Add(new StringEnumConverter());
					jsonSerializerSettings.Converters.Add(new NbtIntConverter());
					jsonSerializerSettings.Converters.Add(new NbtStringConverter());
					jsonSerializerSettings.Converters.Add(new IPAddressConverter());
					jsonSerializerSettings.Converters.Add(new IPEndPointConverter());

					string result = JsonConvert.SerializeObject(message, jsonSerializerSettings);
					Log.Debug($"> Receive: {message.Id} (0x{message.Id:x2}): {message.GetType().Name}\n{result}");
				}
				else if (verbosity == 2 || verbosity == 3)
				{
					Log.Debug($"> Receive: {message.Id} (0x{message.Id:x2}): {message.GetType().Name}\n{Packet.HexDump(message.Bytes)}");
				}
			}
			catch (Exception e)
			{
				Log.Error("Error when printing trace", e);
			}
		}

		internal static void TraceSend(Packet message)
		{
			if (!Log.IsDebugEnabled) return;

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
					Log.Debug($"<    Send: {message.Id} (0x{message.Id:x2}): {message.GetType().Name}");
				}
				else if (verbosity == 1 || verbosity == 3)
				{
					var jsonSerializerSettings = new JsonSerializerSettings
					{
						PreserveReferencesHandling = PreserveReferencesHandling.Arrays,
						TypeNameHandling = TypeNameHandling.Auto,
						Formatting = Formatting.Indented,
					};
					jsonSerializerSettings.Converters.Add(new NbtIntConverter());
					jsonSerializerSettings.Converters.Add(new NbtStringConverter());
					jsonSerializerSettings.Converters.Add(new IPAddressConverter());
					jsonSerializerSettings.Converters.Add(new IPEndPointConverter());

					string result = JsonConvert.SerializeObject(message, jsonSerializerSettings);
					Log.Debug($"<    Send: {message.Id} (0x{message.Id:x2}): {message.GetType().Name}\n{result}");
				}
				else if (verbosity == 2 || verbosity == 3)
				{
					Log.Debug($"<    Send: {message.Id} (0x{message.Id:x2}): {message.GetType().Name}\n{Packet.HexDump(message.Bytes)}");
				}
			}
			catch (Exception e)
			{
				Log.Error("Error when printing trace", e);
			}
		}
	}
}