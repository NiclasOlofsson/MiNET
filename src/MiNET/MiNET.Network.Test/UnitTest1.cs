using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MiNET.Network.Test
{
	internal enum MessageHeader
	{
		ÏsValue,
		IsAck,
		IsNack,
		IsPacketPair,
		IsContinuousSend,
		NeedsBAndAs,
	}

	[TestClass]
	public class UnitTest1
	{
		private List<string> _strings = new List<string>();

		[TestMethod]
		public void LabTest()
		{

			uint id = UInt32.Parse("0300", NumberStyles.AllowHexSpecifier);
			DefaultMessageIdTypes denum = (DefaultMessageIdTypes) Enum.Parse(typeof (DefaultMessageIdTypes), id.ToString());
			Console.WriteLine("Message: 0x{0:x} {0} {1}", id/8, denum.ToString());

			//Assert.AreEqual(DefaultMessageIdTypes.ID_CONNECTION_REQUEST_ACCEPTED, (DefaultMessageIdTypes)denum);
			Assert.AreEqual(15.ToString("x2"), ((int) DefaultMessageIdTypes.ID_CONNECTION_REQUEST_ACCEPTED).ToString("x2"));
			//Assert.AreEqual(DefaultMessageIdTypes.ID_CONNECTION_REQUEST_ACCEPTED, ((int) DefaultMessageIdTypes.ID_CONNECTION_REQUEST_ACCEPTED).ToString("x2"));
		}

		private enum ConnectionState
		{
			Waiting,
			Connecting,
			Connecting2,
			Connected,
		}

		[TestMethod]
		public void TestMethod1()
		{
			IPEndPoint ip = new IPEndPoint(IPAddress.Any, 19132);
			UdpClient listener = new UdpClient(ip);

			listener.BeginReceive(ReceiveCallback, listener);

			while (true)
			{
				Thread.Yield();
			}
		}


		private ConnectionState _state = ConnectionState.Waiting;
		private int _sequenceNumber;

		private void ReceiveCallback(IAsyncResult ar)
		{
			UdpClient listener = (UdpClient) ar.AsyncState;
			IPEndPoint senderEndpoint = new IPEndPoint(0, 0);

			Byte[] receiveBytes = listener.EndReceive(ar, ref senderEndpoint);
			int msgId = receiveBytes[0];

			if (msgId >= (int) DefaultMessageIdTypes.ID_CONNECTED_PING && msgId <= (int) DefaultMessageIdTypes.ID_USER_PACKET_ENUM)
			{
			}

			if (msgId >= (int) DefaultMessageIdTypes.ID_CONNECTED_PING && msgId <= (int) DefaultMessageIdTypes.ID_USER_PACKET_ENUM)
			{
				Debug.Print("Receive standard packet: 0x{0:x2} {0}", msgId);
				Debug.Print("\tPacket data: {0}", ByteArrayToString(receiveBytes));

				DefaultMessageIdTypes msgIdType = (DefaultMessageIdTypes) msgId;
				switch (msgIdType)
				{
					case DefaultMessageIdTypes.ID_CONNECTED_PING:
						break;
					case DefaultMessageIdTypes.ID_UNCONNECTED_PING:
					{
						var incoming = new IdUnconnectedPing();
						incoming._buffer.Write(receiveBytes, 0, receiveBytes.Length);
						incoming.Decode();

						var packet = new IdUnconnectedPong();
						packet.serverId = 1;
						packet.pingId = incoming.pingId;
						packet.Encode();
						//packet._buffer.WriteByte(2);
						packet.Write(Encoding.UTF8.GetBytes("HO"));

						var data = packet._buffer.ToArray();
						SendData(listener, data, senderEndpoint);
					}
						break;
					case DefaultMessageIdTypes.ID_UNCONNECTED_PING_OPEN_CONNECTIONS:
						break;
					case DefaultMessageIdTypes.ID_CONNECTED_PONG:
						break;
					case DefaultMessageIdTypes.ID_DETECT_LOST_CONNECTIONS:
						break;
					case DefaultMessageIdTypes.ID_OPEN_CONNECTION_REQUEST_1:
					{
						if (_state == ConnectionState.Connecting) break;
						_state = ConnectionState.Connecting;

						var packet = new IdOpenConnectionReply1();
						packet.serverGuid = 1;
						packet.mtuSize = 1500;
						packet.serverHasSecurity = 0;
						packet.Encode();

						var data = packet._buffer.ToArray();
						SendData(listener, data, senderEndpoint);
					}
						break;
					case DefaultMessageIdTypes.ID_OPEN_CONNECTION_REPLY_1:
						break;
					case DefaultMessageIdTypes.ID_OPEN_CONNECTION_REQUEST_2:
					{
						if (_state == ConnectionState.Connecting2) break;
						_state = ConnectionState.Connecting2;

						IdOpenConnectionReply2 packet = new IdOpenConnectionReply2();
						packet.serverGuid = 0;
						packet.mtuSize = 1500;
						packet.doSecurity = 0;
						packet.Encode();

						var data = packet._buffer.ToArray();
						SendData(listener, data, senderEndpoint);
					}

						break;
					case DefaultMessageIdTypes.ID_OPEN_CONNECTION_REPLY_2:
						break;
					case DefaultMessageIdTypes.ID_CONNECTION_REQUEST:
						break;
					case DefaultMessageIdTypes.ID_REMOTE_SYSTEM_REQUIRES_PUBLIC_KEY:
						break;
					case DefaultMessageIdTypes.ID_OUR_SYSTEM_REQUIRES_SECURITY:
						break;
					case DefaultMessageIdTypes.ID_PUBLIC_KEY_MISMATCH:
						break;
					case DefaultMessageIdTypes.ID_OUT_OF_BAND_INTERNAL:
						break;
					case DefaultMessageIdTypes.ID_SND_RECEIPT_ACKED:
						break;
					case DefaultMessageIdTypes.ID_SND_RECEIPT_LOSS:
						break;
					case DefaultMessageIdTypes.ID_CONNECTION_REQUEST_ACCEPTED:
						break;
					case DefaultMessageIdTypes.ID_CONNECTION_ATTEMPT_FAILED:
						break;
					case DefaultMessageIdTypes.ID_ALREADY_CONNECTED:
						break;
					case DefaultMessageIdTypes.ID_NEW_INCOMING_CONNECTION:
						break;
					case DefaultMessageIdTypes.ID_NO_FREE_INCOMING_CONNECTIONS:
						break;
					case DefaultMessageIdTypes.ID_DISCONNECTION_NOTIFICATION:
						break;
					case DefaultMessageIdTypes.ID_CONNECTION_LOST:
						break;
					case DefaultMessageIdTypes.ID_CONNECTION_BANNED:
						break;
					case DefaultMessageIdTypes.ID_INVALID_PASSWORD:
						break;
					case DefaultMessageIdTypes.ID_INCOMPATIBLE_PROTOCOL_VERSION:
						break;
					case DefaultMessageIdTypes.ID_IP_RECENTLY_CONNECTED:
						break;
					case DefaultMessageIdTypes.ID_TIMESTAMP:
						break;
					case DefaultMessageIdTypes.ID_UNCONNECTED_PONG:
						break;
					case DefaultMessageIdTypes.ID_ADVERTISE_SYSTEM:
						break;
					case DefaultMessageIdTypes.ID_DOWNLOAD_PROGRESS:
						break;
					case DefaultMessageIdTypes.ID_REMOTE_DISCONNECTION_NOTIFICATION:
						break;
					case DefaultMessageIdTypes.ID_REMOTE_CONNECTION_LOST:
						break;
					case DefaultMessageIdTypes.ID_REMOTE_NEW_INCOMING_CONNECTION:
						break;
					case DefaultMessageIdTypes.ID_FILE_LIST_TRANSFER_HEADER:
						break;
					case DefaultMessageIdTypes.ID_FILE_LIST_TRANSFER_FILE:
						break;
					case DefaultMessageIdTypes.ID_FILE_LIST_REFERENCE_PUSH_ACK:
						break;
					case DefaultMessageIdTypes.ID_DDT_DOWNLOAD_REQUEST:
						break;
					case DefaultMessageIdTypes.ID_TRANSPORT_STRING:
						break;
					case DefaultMessageIdTypes.ID_REPLICA_MANAGER_CONSTRUCTION:
						break;
					case DefaultMessageIdTypes.ID_REPLICA_MANAGER_SCOPE_CHANGE:
						break;
					case DefaultMessageIdTypes.ID_REPLICA_MANAGER_SERIALIZE:
						break;
					case DefaultMessageIdTypes.ID_REPLICA_MANAGER_DOWNLOAD_STARTED:
						break;
					case DefaultMessageIdTypes.ID_REPLICA_MANAGER_DOWNLOAD_COMPLETE:
						break;
					case DefaultMessageIdTypes.ID_REPLICA_MANAGER_3_SERIALIZE_CONSTRUCTION_EXISTING:
						break;
					case DefaultMessageIdTypes.ID_REPLICA_MANAGER_3_LOCAL_CONSTRUCTION_REJECTED:
						break;
					case DefaultMessageIdTypes.ID_REPLICA_MANAGER_3_LOCAL_CONSTRUCTION_ACCEPTED:
						break;
					case DefaultMessageIdTypes.ID_RAKVOICE_OPEN_CHANNEL_REQUEST:
						break;
					case DefaultMessageIdTypes.ID_RAKVOICE_OPEN_CHANNEL_REPLY:
						break;
					case DefaultMessageIdTypes.ID_RAKVOICE_CLOSE_CHANNEL:
						break;
					case DefaultMessageIdTypes.ID_RAKVOICE_DATA:
						break;
					case DefaultMessageIdTypes.ID_AUTOPATCHER_GET_CHANGELIST_SINCE_DATE:
						break;
					case DefaultMessageIdTypes.ID_AUTOPATCHER_CREATION_LIST:
						break;
					case DefaultMessageIdTypes.ID_AUTOPATCHER_DELETION_LIST:
						break;
					case DefaultMessageIdTypes.ID_AUTOPATCHER_GET_PATCH:
						break;
					case DefaultMessageIdTypes.ID_AUTOPATCHER_PATCH_LIST:
						break;
					case DefaultMessageIdTypes.ID_AUTOPATCHER_REPOSITORY_FATAL_ERROR:
						break;
					case DefaultMessageIdTypes.ID_AUTOPATCHER_FINISHED_INTERNAL:
						break;
					case DefaultMessageIdTypes.ID_AUTOPATCHER_FINISHED:
						break;
					case DefaultMessageIdTypes.ID_AUTOPATCHER_RESTART_APPLICATION:
						break;
					case DefaultMessageIdTypes.ID_NAT_PUNCHTHROUGH_REQUEST:
						break;
					case DefaultMessageIdTypes.ID_NAT_CONNECT_AT_TIME:
						break;
					case DefaultMessageIdTypes.ID_NAT_GET_MOST_RECENT_PORT:
						break;
					case DefaultMessageIdTypes.ID_NAT_CLIENT_READY:
						break;
					case DefaultMessageIdTypes.ID_NAT_TARGET_NOT_CONNECTED:
						break;
					case DefaultMessageIdTypes.ID_NAT_TARGET_UNRESPONSIVE:
						break;
					case DefaultMessageIdTypes.ID_NAT_CONNECTION_TO_TARGET_LOST:
						break;
					case DefaultMessageIdTypes.ID_NAT_ALREADY_IN_PROGRESS:
						break;
					case DefaultMessageIdTypes.ID_NAT_PUNCHTHROUGH_FAILED:
						break;
					case DefaultMessageIdTypes.ID_NAT_PUNCHTHROUGH_SUCCEEDED:
						break;
					case DefaultMessageIdTypes.ID_READY_EVENT_SET:
						break;
					case DefaultMessageIdTypes.ID_READY_EVENT_UNSET:
						break;
					case DefaultMessageIdTypes.ID_READY_EVENT_ALL_SET:
						break;
					case DefaultMessageIdTypes.ID_READY_EVENT_QUERY:
						break;
					case DefaultMessageIdTypes.ID_LOBBY_GENERAL:
						break;
					case DefaultMessageIdTypes.ID_RPC_REMOTE_ERROR:
						break;
					case DefaultMessageIdTypes.ID_RPC_PLUGIN:
						break;
					case DefaultMessageIdTypes.ID_FILE_LIST_REFERENCE_PUSH:
						break;
					case DefaultMessageIdTypes.ID_READY_EVENT_FORCE_ALL_SET:
						break;
					case DefaultMessageIdTypes.ID_ROOMS_EXECUTE_FUNC:
						break;
					case DefaultMessageIdTypes.ID_ROOMS_LOGON_STATUS:
						break;
					case DefaultMessageIdTypes.ID_ROOMS_HANDLE_CHANGE:
						break;
					case DefaultMessageIdTypes.ID_LOBBY2_SEND_MESSAGE:
						break;
					case DefaultMessageIdTypes.ID_LOBBY2_SERVER_ERROR:
						break;
					case DefaultMessageIdTypes.ID_FCM2_NEW_HOST:
						break;
					case DefaultMessageIdTypes.ID_FCM2_REQUEST_FCMGUID:
						break;
					case DefaultMessageIdTypes.ID_FCM2_RESPOND_CONNECTION_COUNT:
						break;
					case DefaultMessageIdTypes.ID_FCM2_INFORM_FCMGUID:
						break;
					case DefaultMessageIdTypes.ID_UDP_PROXY_GENERAL:
						break;
					case DefaultMessageIdTypes.ID_SQLite3_EXEC:
						break;
					case DefaultMessageIdTypes.ID_SQLite3_UNKNOWN_DB:
						break;
					case DefaultMessageIdTypes.ID_SQLLITE_LOGGER:
						break;
					case DefaultMessageIdTypes.ID_NAT_TYPE_DETECTION_REQUEST:
						break;
					case DefaultMessageIdTypes.ID_NAT_TYPE_DETECTION_RESULT:
						break;
					case DefaultMessageIdTypes.ID_ROUTER_2_INTERNAL:
						break;
					case DefaultMessageIdTypes.ID_ROUTER_2_FORWARDING_NO_PATH:
						break;
					case DefaultMessageIdTypes.ID_ROUTER_2_FORWARDING_ESTABLISHED:
						break;
					case DefaultMessageIdTypes.ID_ROUTER_2_REROUTED:
						break;
					case DefaultMessageIdTypes.ID_TEAM_BALANCER_INTERNAL:
						break;
					case DefaultMessageIdTypes.ID_TEAM_BALANCER_REQUESTED_TEAM_CHANGE_PENDING:
						break;
					case DefaultMessageIdTypes.ID_TEAM_BALANCER_TEAMS_LOCKED:
						break;
					case DefaultMessageIdTypes.ID_TEAM_BALANCER_TEAM_ASSIGNED:
						break;
					case DefaultMessageIdTypes.ID_LIGHTSPEED_INTEGRATION:
						break;
					case DefaultMessageIdTypes.ID_XBOX_LOBBY:
						break;
					case DefaultMessageIdTypes.ID_TWO_WAY_AUTHENTICATION_INCOMING_CHALLENGE_SUCCESS:
						break;
					case DefaultMessageIdTypes.ID_TWO_WAY_AUTHENTICATION_OUTGOING_CHALLENGE_SUCCESS:
						break;
					case DefaultMessageIdTypes.ID_TWO_WAY_AUTHENTICATION_INCOMING_CHALLENGE_FAILURE:
						break;
					case DefaultMessageIdTypes.ID_TWO_WAY_AUTHENTICATION_OUTGOING_CHALLENGE_FAILURE:
						break;
					case DefaultMessageIdTypes.ID_TWO_WAY_AUTHENTICATION_OUTGOING_CHALLENGE_TIMEOUT:
						break;
					case DefaultMessageIdTypes.ID_TWO_WAY_AUTHENTICATION_NEGOTIATION:
						break;
					case DefaultMessageIdTypes.ID_CLOUD_POST_REQUEST:
						break;
					case DefaultMessageIdTypes.ID_CLOUD_RELEASE_REQUEST:
						break;
					case DefaultMessageIdTypes.ID_CLOUD_GET_REQUEST:
						break;
					case DefaultMessageIdTypes.ID_CLOUD_GET_RESPONSE:
						break;
					case DefaultMessageIdTypes.ID_CLOUD_UNSUBSCRIBE_REQUEST:
						break;
					case DefaultMessageIdTypes.ID_CLOUD_SERVER_TO_SERVER_COMMAND:
						break;
					case DefaultMessageIdTypes.ID_CLOUD_SUBSCRIPTION_NOTIFICATION:
						break;
					case DefaultMessageIdTypes.ID_RESERVED_1:
						break;
					case DefaultMessageIdTypes.ID_RESERVED_2:
						break;
					case DefaultMessageIdTypes.ID_RESERVED_3:
						break;
					case DefaultMessageIdTypes.ID_RESERVED_4:
						break;
					case DefaultMessageIdTypes.ID_RESERVED_5:
						break;
					case DefaultMessageIdTypes.ID_RESERVED_6:
						break;
					case DefaultMessageIdTypes.ID_RESERVED_7:
						break;
					case DefaultMessageIdTypes.ID_RESERVED_8:
						break;
					case DefaultMessageIdTypes.ID_RESERVED_9:
						break;
					case DefaultMessageIdTypes.ID_USER_PACKET_ENUM:
						break;
				}
			}
			else
			{
				Debug.Print("Receive custom packet: 0x{0:x2} {0}", msgId);
				Debug.Print("\tPacket data: {0}", ByteArrayToString(receiveBytes));
				DatagramHeader header = new DatagramHeader(receiveBytes[0]);
				if (!header.isACK && !header.isNAK && header.isValid)
				{
					//if (_state != ConnectionState.Connected)
					{
						_state = ConnectionState.Connected;

						if (receiveBytes[0] != 0xa0)
						{
							var connectedPackage = new ConnectedPackage();
							connectedPackage._buffer.Write(receiveBytes, 0, receiveBytes.Length);
							connectedPackage.Decode();
							// Send ACK

							var internalPackage = PackageFactory.CreatePackage(connectedPackage.receiveBuffer[0]);
							if (internalPackage != null)
							{
								internalPackage.Write(connectedPackage.receiveBuffer);
								internalPackage.Decode();

								// Ok, we got it no problem, send ACK back
								var ack = new Ack();
								ack.sequenceNumber = connectedPackage.sequenceNumber;
								ack.count = IPAddress.HostToNetworkOrder((short) 1);
								ack.onlyOneSequence = 1;
								ack.Encode();
								SendData(listener, ack._buffer.ToArray(), senderEndpoint);
							}
							else
							{
								// Send NAK
								var nak = new Nak();
								nak.sequenceNumber = connectedPackage.sequenceNumber;
								nak.count = IPAddress.HostToNetworkOrder((short) 1);
								nak.onlyOneSequence = 1;
								nak.Encode();
								SendData(listener, nak._buffer.ToArray(), senderEndpoint);
							}

							if (internalPackage != null && typeof (IdConnectionRequest) == internalPackage.GetType())
							{
								var connectionRequest = (IdConnectionRequest) internalPackage;
								var intern = new IdConnectionRequestAcceptedManual();
								byte[] result = intern.Encode((short) senderEndpoint.Port, connectionRequest.timestamp);

								connectedPackage.sendBuffer = result;
								connectedPackage.sequenceNumber = _sequenceNumber++;
								connectedPackage.Encode();
								var data = connectedPackage._buffer.ToArray();
								SendData(listener, data, senderEndpoint);
							}
						}
					}
				}
				else if (header.isACK && header.isValid)
				{
					//var connectedPackage = new ConnectedPackage();
					//connectedPackage._buffer.Write(receiveBytes, 0, receiveBytes.Length);
					//connectedPackage.Decode();

					//connectedPackage.Encode();
					//var data = connectedPackage._buffer.ToArray();
					//SendData(listener, data, senderEndpoint);
				}
			}


			if (receiveBytes.Length != 0)
			{
				listener.BeginReceive(ReceiveCallback, listener);
			}
		}

		private static void SendData(UdpClient listener, byte[] data, IPEndPoint senderEndpoint)
		{
			Debug.Print("Send packet: 0x{0:x2} {0}", data[0]);
			Debug.Print("\tPacket data: {0}", ByteArrayToString(data));
			listener.Send(data, data.Length, senderEndpoint);
		}

		public static string ByteArrayToString(byte[] ba)
		{
			StringBuilder hex = new StringBuilder((ba.Length*2) + 100);
			hex.Append("{");
			foreach (byte b in ba)
				hex.AppendFormat("0x{0:x2},", b);
			hex.Append("}");
			return hex.ToString();
		}
	}

	internal class IdConnectionRequestAcceptedManual : Package
	{
		public byte[] Encode(short port, long sessionID)
		{
			Write((byte) 0x10);
			Write(new byte[] { 0x04, 0x3f, 0x57, 0xfe }); //Cookie
			Write((byte) 0xcd); //Security flags
			Write(IPAddress.HostToNetworkOrder(port));
			PutDataArray();
			Write(new byte[] { 0x00, 0x00 });
			Write(sessionID);
			Write(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x04, 0x44, 0x0b, 0xa9 });

			return _buffer.ToArray();
		}

		public void encode()
		{
		}

		private void PutDataArray()
		{
			byte[] unknown1 = new byte[] { (byte) 0xf5, (byte) 0xff, (byte) 0xff, (byte) 0xf5 };
			byte[] unknown2 = new byte[] { (byte) 0xff, (byte) 0xff, (byte) 0xff, (byte) 0xff };

			Write((Int24) unknown1.Length);
			Write(unknown1);

			for (int i = 0; i < 9; i++)
			{
				Write((Int24) unknown2.Length);
				Write(unknown2);
			}
		}
	}
}
