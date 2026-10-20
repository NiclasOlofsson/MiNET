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
using System.Buffers.Binary;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using MiNET.Utils;

namespace MiNET.Net.NetherNet
{
	/// <summary>
	///     LAN visibility over NetherNet. The client broadcasts a discovery request on UDP 7551 every
	///     two seconds and lists whoever answers with a response carrying the server data. BDS 1.26.70
	///     binds nothing here even with enable-lan-visibility on, so the format follows go-nethernet's
	///     discovery package and the client's own requests.
	///     <para>
	///         Every datagram is an HMAC-SHA256 of the plaintext followed by the plaintext under
	///         AES-256-ECB, both keyed with SHA-256 of the application id 0xdeadbeef as a little-endian
	///         uint64. The plaintext is a uint16 length that counts itself, the packet id, the sender's
	///         network id and eight bytes of padding, then the body.
	///     </para>
	/// </summary>
	public class NetherNetLanDiscovery : IDisposable
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(NetherNetLanDiscovery));

		public const int Port = 7551;

		private const ushort RequestPacketId = 0;
		private const ushort ResponsePacketId = 1;
		private const ushort MessagePacketId = 2;

		private const int MacLength = 32;
		private const int HeaderLength = 2 + 2 + 8 + 8;
		private const byte ServerDataVersion = 7;

		// The connection type vanilla advertises for NetherNet signaled over the LAN itself.
		private const int ConnectionTypeLanSignaling = 4;

		// SIO_UDP_CONNRESET: stop an ICMP port-unreachable from a vanished requester aborting the socket.
		private const int SioUdpConnReset = -1744830452;

		private static readonly byte[] Key = SHA256.HashData(BitConverter.GetBytes(0xdeadbeefUL));

		private readonly MotdProvider _motdProvider;
		private readonly ConnectionInfo _connectionInfo;
		private readonly Func<int> _playerCount;
		private readonly bool _acceptsSelfSignedAuth;
		private readonly string _nonce = Convert.ToHexStringLower(RandomNumberGenerator.GetBytes(16));
		private readonly Aes _aes = Aes.Create();

		private Socket _socket;
		private CancellationTokenSource _cancellation;

		/// <summary>The id this server signs its discovery packets with, and the one a LAN join addresses.</summary>
		public ulong NetworkId { get; }

		public NetherNetLanDiscovery(MotdProvider motdProvider, ConnectionInfo connectionInfo, Func<int> playerCount)
		{
			_motdProvider = motdProvider;
			_connectionInfo = connectionInfo;
			_playerCount = playerCount;
			_acceptsSelfSignedAuth = !Config.GetProperty("ForceXBLAuthentication", false);
			_aes.Key = Key;
			NetworkId = (ulong) motdProvider.ServerId;
		}

		/// <summary>
		///     Binds 7551 shared, because a client on the same machine holds the port too, and starts
		///     answering. A bind the OS refuses costs LAN visibility, never the server.
		/// </summary>
		public void Start()
		{
			try
			{
				_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp) {EnableBroadcast = true};
				_socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
				_socket.Bind(new IPEndPoint(IPAddress.Any, Port));
				if (OperatingSystem.IsWindows()) _socket.IOControl(SioUdpConnReset, new byte[] {0}, null);
			}
			catch (SocketException e)
			{
				Log.Warn($"NetherNet LAN discovery could not bind UDP {Port}, the server will not show in LAN lists: {e.Message}");
				_socket?.Dispose();
				_socket = null;
				return;
			}

			_cancellation = new CancellationTokenSource();
			Task.Run(() => ReceiveLoop(_cancellation.Token));
			Log.Info($"NetherNet LAN discovery answering on UDP {Port} as network id {NetworkId}");
		}

		private async Task ReceiveLoop(CancellationToken token)
		{
			byte[] buffer = new byte[2048];
			EndPoint any = new IPEndPoint(IPAddress.Any, 0);

			while (!token.IsCancellationRequested)
			{
				SocketReceiveFromResult result;
				try
				{
					result = await _socket.ReceiveFromAsync(buffer, SocketFlags.None, any, token);
				}
				catch (OperationCanceledException)
				{
					return;
				}
				catch (ObjectDisposedException)
				{
					return;
				}
				catch (SocketException e)
				{
					Log.Debug($"NetherNet LAN discovery receive failed: {e.SocketErrorCode}");
					continue;
				}

				try
				{
					Handle(buffer.AsSpan(0, result.ReceivedBytes), (IPEndPoint) result.RemoteEndPoint);
				}
				catch (Exception e)
				{
					Log.Debug($"NetherNet LAN discovery dropped a datagram from {result.RemoteEndPoint}: {e.Message}");
				}
			}
		}

		private void Handle(ReadOnlySpan<byte> datagram, IPEndPoint from)
		{
			if (datagram.Length < MacLength + 16) return;

			byte[] plain = _aes.DecryptEcb(datagram[MacLength..], PaddingMode.PKCS7);
			if (!CryptographicOperations.FixedTimeEquals(HMACSHA256.HashData(Key, plain), datagram[..MacLength])) return;
			if (plain.Length < HeaderLength) return;

			ushort packetId = BinaryPrimitives.ReadUInt16LittleEndian(plain.AsSpan(2));
			ulong senderId = BinaryPrimitives.ReadUInt64LittleEndian(plain.AsSpan(4));
			if (senderId == NetworkId) return;

			switch (packetId)
			{
				case RequestPacketId:
					SendResponse(from);
					break;
				case MessagePacketId:
					ulong recipientId = plain.Length >= HeaderLength + 8 ? BinaryPrimitives.ReadUInt64LittleEndian(plain.AsSpan(HeaderLength)) : 0;
					string data = plain.Length > HeaderLength + 12 ? Encoding.UTF8.GetString(plain, HeaderLength + 12, plain.Length - HeaderLength - 12) : "";
					Log.Debug($"NetherNet LAN message from {senderId} at {from} to {recipientId}: {data}");
					break;
			}
		}

		private void SendResponse(IPEndPoint requester)
		{
			byte[] hex = Encoding.ASCII.GetBytes(Convert.ToHexStringLower(BuildServerData()));
			byte[] body = new byte[4 + hex.Length];
			BinaryPrimitives.WriteUInt32LittleEndian(body, (uint) hex.Length);
			hex.CopyTo(body, 4);

			// A requester on this machine shares the port with this socket, and Windows hands a
			// unicast to only one of the sockets bound there. A broadcast reaches all of them.
			IPEndPoint target = MotdProvider.IsThisMachine(requester.Address) || IPAddress.IsLoopback(requester.Address)
				? new IPEndPoint(IPAddress.Broadcast, Port)
				: requester;

			_socket.SendTo(Seal(ResponsePacketId, body), target);
		}

		private byte[] BuildServerData()
		{
			// The world card's title is the level name and its subtitle is "<server name>'s world",
			// so the MOTD goes in the level name and the second line is what the subtitle names.
			var stream = new MemoryStream();
			stream.WriteByte(ServerDataVersion);
			WriteString(stream, _motdProvider.SecondLine);
			VarInt.WriteSInt32(stream, McpeProtocolInfo.ProtocolVersion);
			WriteString(stream, McpeProtocolInfo.GameVersion);
			WriteString(stream, _motdProvider.Motd);
			VarInt.WriteSInt32(stream, _playerCount());
			VarInt.WriteSInt32(stream, _connectionInfo.MaxNumberOfPlayers);
			VarInt.WriteSInt32(stream, _motdProvider.GameType);
			stream.WriteByte((byte) (_motdProvider.IsInEditorMode ? 1 : 0));
			stream.WriteByte(0); // hardcore
			stream.WriteByte(1); // accepts Xbox Live authentication
			stream.WriteByte((byte) (_acceptsSelfSignedAuth ? 1 : 0));
			WriteString(stream, _nonce);
			VarInt.WriteSInt32(stream, ConnectionTypeLanSignaling);
			return stream.ToArray();
		}

		private static void WriteString(Stream stream, string value)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(value ?? "");
			VarInt.WriteUInt32(stream, (uint) bytes.Length);
			stream.Write(bytes);
		}

		/// <summary>Header, body, length, then the MAC over the plaintext ahead of the ciphertext.</summary>
		private byte[] Seal(ushort packetId, byte[] body)
		{
			byte[] plain = new byte[HeaderLength + body.Length];
			BinaryPrimitives.WriteUInt16LittleEndian(plain, (ushort) plain.Length);
			BinaryPrimitives.WriteUInt16LittleEndian(plain.AsSpan(2), packetId);
			BinaryPrimitives.WriteUInt64LittleEndian(plain.AsSpan(4), NetworkId);
			body.CopyTo(plain, HeaderLength);

			byte[] cipher = _aes.EncryptEcb(plain, PaddingMode.PKCS7);
			byte[] datagram = new byte[MacLength + cipher.Length];
			HMACSHA256.HashData(Key, plain).CopyTo(datagram, 0);
			cipher.CopyTo(datagram, MacLength);
			return datagram;
		}

		public void Dispose()
		{
			_cancellation?.Cancel();
			_socket?.Dispose();
			_socket = null;
			_aes.Dispose();
		}
	}
}