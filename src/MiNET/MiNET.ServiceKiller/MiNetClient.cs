using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Sockets;
using MiNET.Net;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.ServiceKiller
{
	public class MiNetClient
	{
		private IPEndPoint _endpoint;
		public UdpClient _listener;
		private Level _level;

		private IPEndPoint _serverEndpoint;
		private short _mtuSize = 1447;
		private decimal _lastSequenceNumber;
		private McpeMovePlayer _movePlayerPacket;

		public MiNetClient(IPEndPoint endpoint = null)
		{
			_endpoint = new IPEndPoint(IPAddress.Any, 0);
			_serverEndpoint = endpoint ?? new IPEndPoint(IPAddress.Loopback, 19132);
			_movePlayerPacket = new McpeMovePlayer();
		}

		public bool StartClient()
		{
			try
			{
				_listener = new UdpClient(_endpoint);
				_listener.Client.DontFragment = false;
				//_listener.Client.ReceiveBufferSize = 1024*1024;
				//_listener.Client.SendBufferSize = 4096;

				// SIO_UDP_CONNRESET (opcode setting: I, T==3)
				// Windows:  Controls whether UDP PORT_UNREACHABLE messages are reported.
				// - Set to TRUE to enable reporting.
				// - Set to FALSE to disable reporting.

				//uint IOC_IN = 0x80000000;
				//uint IOC_VENDOR = 0x18000000;
				//uint SIO_UDP_CONNRESET = IOC_IN | IOC_VENDOR | 12;
				//_listener.Client.IOControl((int) SIO_UDP_CONNRESET, new byte[] {Convert.ToByte(false)}, null);

				// We need to catch errors here to remove the code above.
				//_listener.BeginReceive(ReceiveCallback, _listener);

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

		public int SendPackage(Package message, short mtuSize, int sequenceNumber, int reliableMessageNumber, Reliability reliability = Reliability.Reliable)
		{
			byte[] encodedMessage = message.Encode();
			//int count = (int) Math.Ceiling(encodedMessage.Length/((double) mtuSize - 60));
			int count = 1;
			int index = 0;
			short splitId = (short) (sequenceNumber%65536);
			//foreach (var bytes in ArraySplit(encodedMessage, mtuSize - 60))
			//{
			ConnectedPackage package = new ConnectedPackage
			{
				Buffer = encodedMessage,
				_reliability = reliability,
				_reliableMessageNumber = reliableMessageNumber++,
				_datagramSequenceNumber = sequenceNumber++,
				_hasSplit = count > 1,
				_splitPacketCount = count,
				_splitPacketId = splitId,
				_splitPacketIndex = index++
			};

			byte[] data = package.Encode();

			SendData(data);
			//}

			message.PutPool();

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
		}

		public void SendUnconnectedPing()
		{
			var packet = new UnconnectedPing
			{
				pingId = 100 /*incoming.pingId*/,
			};

			var data = packet.Encode();
			SendData(data);
		}

		public void SendOpenConnectionRequest1()
		{
			var packet = new OpenConnectionRequest1()
			{
				raknetProtocolVersion = byte.MaxValue, // Indicate to the server that this is a performance tests. Disables logging.
				mtuSize = _mtuSize
			};

			var data = packet.Encode();
			SendData(data);
		}

		public void SendOpenConnectionRequest2()
		{
			var packet = new OpenConnectionRequest2()
			{
				systemadress = new byte[4],
				mtuSize = _mtuSize
			};

			var data = packet.Encode();
			SendData(data);
		}

		public void SendConnectionRequest()
		{
			var packet = new ConnectionRequest()
			{
			};

			SendPackage(packet, _mtuSize, 0, 0);
		}

		public void SendLogin(string username)
		{
			var packet = new McpeLogin()
			{
				clientId = 12345,
				username = username,
				protocol = 27,
				slim = 0,
				skin = new string('Z', 8192)
			};

			McpeBatch batch = new McpeBatch();
			byte[] buffer = CompressBytes(packet.Encode());

			batch.payloadSize = buffer.Length;
			batch.payload = buffer;

			SendPackage(batch, _mtuSize, 0, 0);
		}

		public byte[] CompressBytes(byte[] input)
		{
			MemoryStream stream = new MemoryStream();
			stream.WriteByte(0x78);
			stream.WriteByte(0x01);
			int checksum;
			using (var compressStream = new ZLibStream(stream, CompressionLevel.Optimal, true))
			{
				NbtBinaryWriter writer = new NbtBinaryWriter(compressStream, true);
				writer.Write(input);

				writer.Flush();

				checksum = compressStream.Checksum;
				writer.Close();
			}

			byte[] checksumBytes = BitConverter.GetBytes(checksum);
			if (BitConverter.IsLittleEndian)
			{
				// Adler32 checksum is big-endian
				Array.Reverse(checksumBytes);
			}
			stream.Write(checksumBytes, 0, checksumBytes.Length);

			var bytes = stream.ToArray();
			stream.Close();

			return bytes;
		}


		public void SendDisconnectionNotification()
		{
			var packet = new DisconnectionNotification()
			{
			};

			SendPackage(packet, _mtuSize, 0, 0);
		}

		public void SendMcpeMovePlayer(float x, float y, float z)
		{
			//var movePlayerPacket = McpeMovePlayer.AddReference();
			_movePlayerPacket.Reset();
			_movePlayerPacket.x = x;
			_movePlayerPacket.y = y;
			_movePlayerPacket.z = z;
			_movePlayerPacket.yaw = 91;
			_movePlayerPacket.pitch = 28;
			_movePlayerPacket.headYaw = 91;

			SendPackage(_movePlayerPacket, _mtuSize, 0, 0);
		}
	}
}