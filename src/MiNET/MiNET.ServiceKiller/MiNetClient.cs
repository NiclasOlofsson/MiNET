using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using MiNET.Net;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.ServiceKiller
{
	public class MiNetClient
	{
		private IPEndPoint _endpoint;
		private UdpClient _listener;
		private Level _level;

		private IPEndPoint _serverEndpoint;
		private short _mtuSize = 1500;

		public MiNetClient(int port) : this(new IPEndPoint(IPAddress.Any, port))
		{
		}

		public MiNetClient(IPEndPoint endpoint = null)
		{
			_endpoint = endpoint ?? new IPEndPoint(IPAddress.Any, 0);
			_serverEndpoint = new IPEndPoint(IPAddress.Loopback, 19132);
		}

		public bool StartClient()
		{
			if (_listener != null) return false; // Already started

			try
			{
				_listener = new UdpClient(_endpoint);
				_listener.Client.ReceiveBufferSize = 1024*1024;
				_listener.Client.SendBufferSize = 4096;

				// SIO_UDP_CONNRESET (opcode setting: I, T==3)
				// Windows:  Controls whether UDP PORT_UNREACHABLE messages are reported.
				// - Set to TRUE to enable reporting.
				// - Set to FALSE to disable reporting.

				uint IOC_IN = 0x80000000;
				uint IOC_VENDOR = 0x18000000;
				uint SIO_UDP_CONNRESET = IOC_IN | IOC_VENDOR | 12;
				_listener.Client.IOControl((int) SIO_UDP_CONNRESET, new byte[] {Convert.ToByte(false)}, null);

				// We need to catch errors here to remove the code above.
//				_listener.BeginReceive(ReceiveCallback, _listener);

				Console.WriteLine("Client open for business...");

				return true;
			}
			catch (Exception e)
			{
				Debug.Write(e);
				StopClient();
			}

			return false;
		}


		public bool StopClient()
		{
			try
			{
				if (_listener == null) return true; // Already stopped. It's ok.

				_listener.Close();
				_listener = null;

				return true;
			}
			catch (Exception e)
			{
				Debug.Write(e);
			}

			return true;
		}

		private void ReceiveCallback(IAsyncResult ar)
		{
			UdpClient listener = (UdpClient) ar.AsyncState;

			if (!listener.Client.IsBound || !listener.Client.Connected) return;

			// WSAECONNRESET:
			// The virtual circuit was reset by the remote side executing a hard or abortive close. 
			// The application should close the socket; it is no longer usable. On a UDP-datagram socket 
			// this error indicates a previous send operation resulted in an ICMP Port Unreachable message.
			// Note the spocket settings on creation of the server. It makes us ignore these resets.
			IPEndPoint senderEndpoint = new IPEndPoint(0, 0);
			Byte[] receiveBytes = null;
			try
			{
				receiveBytes = listener.EndReceive(ar, ref senderEndpoint);
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				listener.BeginReceive(ReceiveCallback, listener);

				return;
			}

			byte msgId = receiveBytes[0];

			if (msgId <= (byte) DefaultMessageIdTypes.ID_USER_PACKET_ENUM)
			{
				DefaultMessageIdTypes msgIdType = (DefaultMessageIdTypes) msgId;

				Package message = PackageFactory.CreatePackage(msgId, receiveBytes);

				TraceReceive(msgIdType, msgId, receiveBytes, receiveBytes.Length, message);

				switch (msgIdType)
				{
					case DefaultMessageIdTypes.ID_UNCONNECTED_PING:
					case DefaultMessageIdTypes.ID_UNCONNECTED_PING_OPEN_CONNECTIONS:
					case DefaultMessageIdTypes.ID_OPEN_CONNECTION_REQUEST_1:
					case DefaultMessageIdTypes.ID_OPEN_CONNECTION_REQUEST_2:
						break;
				}
			}
			else
			{
				DatagramHeader header = new DatagramHeader(receiveBytes[0]);
				if (!header.isACK && !header.isNAK && header.isValid)
				{
					if (receiveBytes[0] == 0xa0)
					{
						throw new Exception("Receive ERROR, NAK in wrong place");
					}

					var package = new ConnectedPackage();
					package.Decode(receiveBytes);
					var messages = package.Messages;

					foreach (var message in messages)
					{
						TraceReceive((DefaultMessageIdTypes) message.Id, message.Id, receiveBytes, package.MessageLength, message, message is UnknownPackage);
						SendAck(senderEndpoint, package._sequenceNumber);
						HandlePackage(message, senderEndpoint);
					}
				}
				else if (header.isACK && header.isValid)
				{
					//Ack ack = new Ack();
					//ack.Decode(receiveBytes);
					//Debug.WriteLine("ACK #{0}", ack.nakSequencePackets.FirstOrDefault());
				}
				else if (header.isNAK && header.isValid)
				{
					Nak nak = new Nak();
					nak.Decode(receiveBytes);
					Debug.WriteLine("!--> NAK on #{0}", nak.sequenceNumber.IntValue());
				}
				else if (!header.isValid)
				{
					Debug.WriteLine("!!!! ERROR, Invalid header !!!!!");
				}
			}


			if (receiveBytes.Length != 0)
			{
				listener.BeginReceive(ReceiveCallback, listener);
			}
			else
			{
				Debug.Write("Unexpected end of transmission?");
			}
		}

		private void HandlePackage(Package message, IPEndPoint senderEndpoint)
		{
			if (typeof (UnknownPackage) == message.GetType())
			{
				return;
			}

			//if (_serverEndpoint.ContainsKey(senderEndpoint))
			//{
			//	_serverEndpoint[senderEndpoint].HandlePackage(message);
			//}

			//if (typeof (DisconnectionNotification) == message.GetType())
			//{
			//	_serverEndpoint.Remove(senderEndpoint);
			//}
		}

		public int SendPackage(Package message, short mtuSize, int sequenceNumber, int reliableMessageNumber, Reliability reliability = Reliability.RELIABLE)
		{
			byte[] encodedMessage = message.Encode();
			int count = (int) Math.Ceiling(encodedMessage.Length/((double) mtuSize - 60));
			int index = 0;
			short splitId = (short) (sequenceNumber%65536);
			foreach (var bytes in ArraySplit(encodedMessage, mtuSize - 60))
			{
				ConnectedPackage package = new ConnectedPackage
				{
					Buffer = bytes,
					_reliability = reliability,
					_reliableMessageNumber = reliableMessageNumber++,
					_sequenceNumber = sequenceNumber++,
					_hasSplit = count > 1,
					_splitPacketCount = count,
					_splitPacketId = splitId,
					_splitPacketIndex = index++
				};

				byte[] data = package.Encode();

				TraceSend(message, data, package);

				SendData(data);
			}

			return count;
		}

		public IEnumerable<byte[]> ArraySplit(byte[] bArray, int intBufforLengt)
		{
			int bArrayLenght = bArray.Length;
			byte[] bReturn = null;

			int i = 0;
			for (; bArrayLenght > (i + 1)*intBufforLengt; i++)
			{
				bReturn = new byte[intBufforLengt];
				Array.Copy(bArray, i*intBufforLengt, bReturn, 0, intBufforLengt);
				yield return bReturn;
			}

			int intBufforLeft = bArrayLenght - i*intBufforLengt;
			if (intBufforLeft > 0)
			{
				bReturn = new byte[intBufforLeft];
				Array.Copy(bArray, i*intBufforLengt, bReturn, 0, intBufforLeft);
				yield return bReturn;
			}
		}

		private void SendAck(IPEndPoint senderEndpoint, Int24 sequenceNumber)
		{
			var ack = new Ack
			{
				sequenceNumber = sequenceNumber,
				count = 1,
				onlyOneSequence = 1
			};

			var data = ack.Encode();

			SendData(data);
		}

		public void SendData(byte[] data)
		{
			_listener.Send(data, data.Length, _serverEndpoint);
			Thread.Yield();
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

		private static void TraceReceive(DefaultMessageIdTypes msgIdType, int msgId, byte[] receiveBytes, int length, Package package, bool isUnknown = false)
		{
			Debug.Print("C<S Receive {2}: {1} (0x{0:x2} {3})", msgId, msgIdType, isUnknown ? "Unknown" : "", package.GetType().Name);
		}

		private static void TraceSend(Package message, byte[] data)
		{
			Debug.Print("C>S Send: {0:x2} {1} (0x{2:x2})", data[0], (DefaultMessageIdTypes) message.Id, message.Id);
			//Debug.Print("\tData: Length={1} {0}", ByteArrayToString(data), data.Length);
		}

		private static void TraceSend(Package message, byte[] data, ConnectedPackage package)
		{
			Debug.Print("C>S Send: {0:x2} {1} (0x{2:x2} {4}) SeqNo: {3}", data[0], (DefaultMessageIdTypes) message.Id, message.Id, package._sequenceNumber.IntValue(), package.GetType().Name);
			//Debug.Print("\tData: Length={1} {0}", ByteArrayToString(data), package.MessageLength);
		}

		public void SendUnconnectedPing()
		{
			var packet = new UnconnectedPing
			{
				pingId = 100 /*incoming.pingId*/,
			};

			var data = packet.Encode();
			TraceSend(packet, data);
			SendData(data);
		}

		public void SendOpenConnectionRequest1()
		{
			var packet = new OpenConnectionRequest1()
			{
				mtuSize = _mtuSize
			};

			var data = packet.Encode();
			TraceSend(packet, data);
			SendData(data);
		}

		public void SendOpenConnectionRequest2()
		{
			var packet = new OpenConnectionRequest2()
			{
				cookie = new byte[4],
				mtuSize = _mtuSize
			};

			var data = packet.Encode();
			TraceSend(packet, data);
			SendData(data);
		}

		public void SendConnectionRequest()
		{
			var packet = new ConnectionRequest()
			{
			};

			SendPackage(packet, _mtuSize, 0, 0);
		}

		public void SendMcpeLogin(string username)
		{
			var packet = new McpeLogin()
			{
				username = username,
				logindata = "nothing"
			};

			SendPackage(packet, _mtuSize, 0, 0);
		}


		public void SendDisconnectionNotification()
		{
			var packet = new DisconnectionNotification()
			{
			};

			SendPackage(packet, _mtuSize, 0, 0);
		}

		public void SendMcpeMovePlayer(int x, int y, int z)
		{
			var position = new PlayerPosition3D
			{
				X = x,
				Y = y,
				Z = z,
				Yaw = 91,
				Pitch = 28,
				BodyYaw = 91
			};

			var packet = new McpeMovePlayer()
			{
				entityId = 0,
				x = position.X,
				y = position.Y,
				z = position.Z,
				yaw = position.Yaw,
				pitch = position.Pitch,
				bodyYaw = position.BodyYaw,
			};

			SendPackage(packet, _mtuSize, 0, 0);
		}
	}
}