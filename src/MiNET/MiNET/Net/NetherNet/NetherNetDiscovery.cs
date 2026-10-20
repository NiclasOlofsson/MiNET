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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2026 Niclas Olofsson.
// All Rights Reserved.

#endregion

using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using MiNET.Utils;

namespace MiNET.Net.NetherNet
{
	/// <summary>
	///     RakNet's unconnected ping/pong, outliving RakNet itself: clients up to 1.26.70.20 discovered
	///     and listed servers through this exchange, and status tools still may, so a NetherNet-only
	///     server still answers it on the gameplay UDP port. The 1.26.70.21 client discovers LAN
	///     servers with <see cref="NetherNetLanDiscovery" /> instead. Both halves live
	///     here, the server-side answerer the mux calls for non-STUN datagrams and the client-side
	///     ping a NetherNet client uses to read a server's MOTD, so the whole legacy format is one
	///     file that attaches (or does not) at one seam. It is stateless offline-message handling:
	///     no session, no admission against the mux's first-contact budget, nothing to tear down.
	/// </summary>
	public class NetherNetDiscovery
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(NetherNetDiscovery));

		private const byte UnconnectedPingId = 0x01;
		private const byte UnconnectedPingOpenConnectionsId = 0x02;
		private const byte UnconnectedPongId = 0x1c;

		// The RakNet offline-message magic, at offset 9 in a ping (after id and time) and offset 17
		// in a pong (after id, time and server id). Its presence is what separates these messages
		// from coincidental first bytes.
		private static readonly byte[] OfflineMessageDataId = {0x00, 0xff, 0xff, 0x00, 0xfe, 0xfe, 0xfe, 0xfe, 0xfd, 0xfd, 0xfd, 0xfd, 0x12, 0x34, 0x56, 0x78};

		private readonly MotdProvider _motdProvider;
		private readonly ConnectionInfo _connectionInfo;
		private readonly Func<int> _playerCount;

		/// <param name="playerCount">
		///     Live player count for the pong, supplied by the transport that actually holds the
		///     sessions: the RakNet-era counter inside <paramref name="connectionInfo" /> only sees
		///     RakNet sessions and reads zero on a NetherNet-only server.
		/// </param>
		public NetherNetDiscovery(MotdProvider motdProvider, ConnectionInfo connectionInfo, Func<int> playerCount)
		{
			_motdProvider = motdProvider;
			_connectionInfo = connectionInfo;
			_playerCount = playerCount;
		}

		/// <summary>
		///     The mux's offline responder: an unconnected ping comes back as a pong built from the
		///     MOTD provider, anything else comes back null and falls through to the caller's drop
		///     accounting.
		/// </summary>
		public byte[] HandleOffline(ReadOnlySpan<byte> datagram, IPEndPoint from)
		{
			if (!LooksLikeUnconnectedPing(datagram)) return null;

			UnconnectedPing ping;
			try
			{
				ping = (UnconnectedPing) PacketFactory.Create(datagram[0], datagram.ToArray(), "raknet");
			}
			catch (Exception)
			{
				return null;
			}

			if (ping == null) return null;

			Log.Trace($"RakNet unconnected ping 0x{datagram[0]:x2} from {from}, ping id {ping.pingId}, client guid {ping.guid}");

			_connectionInfo.NumberOfPlayers = _playerCount();

			var pong = UnconnectedPong.CreateObject();
			pong.serverId = _motdProvider.GetServerId(from);
			pong.pingId = ping.pingId;
			pong.serverName = _motdProvider.GetMotd(_connectionInfo, from);
			byte[] reply = pong.Encode();
			pong.PutPool();
			ping.PutPool();

			return reply;
		}

		public static bool LooksLikeUnconnectedPing(ReadOnlySpan<byte> datagram)
		{
			if (datagram.Length < 33) return false;
			if (datagram[0] != UnconnectedPingId && datagram[0] != UnconnectedPingOpenConnectionsId) return false;
			return datagram.Slice(9, OfflineMessageDataId.Length).SequenceEqual(OfflineMessageDataId);
		}

		/// <summary>A fresh unconnected ping, ready to send. The ping id doubles as the send timestamp.</summary>
		public static byte[] BuildPing()
		{
			var ping = UnconnectedPing.CreateObject();
			ping.pingId = DateTime.UtcNow.Ticks / TimeSpan.TicksPerMillisecond;
			ping.guid = Random.Shared.NextInt64();
			byte[] data = ping.Encode();
			ping.PutPool();
			return data;
		}

		public static bool TryParsePong(ReadOnlySpan<byte> datagram, out string motd)
		{
			motd = null;
			if (datagram.Length < 35 || datagram[0] != UnconnectedPongId) return false;
			if (!datagram.Slice(17, OfflineMessageDataId.Length).SequenceEqual(OfflineMessageDataId)) return false;

			try
			{
				var pong = (UnconnectedPong) PacketFactory.Create(UnconnectedPongId, datagram.ToArray(), "raknet");
				if (pong == null) return false;
				motd = pong.serverName;
				pong.PutPool();
				return motd != null;
			}
			catch (Exception)
			{
				return false;
			}
		}

		/// <summary>
		///     The client half: pings a server's UDP port and returns the raw MOTD string, or null
		///     when nothing answers in time. One socket, one exchange, nothing retained.
		/// </summary>
		public static async Task<string> PingAsync(string host, int port, TimeSpan timeout, CancellationToken cancellationToken = default)
		{
			using var socket = new UdpClient();
			using var timeoutSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
			timeoutSource.CancelAfter(timeout);

			try
			{
				byte[] ping = BuildPing();
				await socket.SendAsync(ping, host, port, timeoutSource.Token);

				while (true)
				{
					UdpReceiveResult result = await socket.ReceiveAsync(timeoutSource.Token);
					if (TryParsePong(result.Buffer, out string motd)) return motd;
				}
			}
			catch (OperationCanceledException)
			{
				return null;
			}
			catch (SocketException e)
			{
				Log.Debug($"Discovery ping to {host}:{port} failed: {e.Message}");
				return null;
			}
		}
	}
}