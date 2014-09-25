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


	public enum GameModes
	{
		SURVIVAL,
		CREATIVE,
		ADVENTURE,
		SPECTATOR,
	}

	[TestClass]
	public class UnitTest1
	{
		private List<string> _strings = new List<string>();

		[TestMethod]
		public void LabTest()
		{
			uint id = UInt32.Parse("13", NumberStyles.AllowHexSpecifier);
			DefaultMessageIdTypes denum = (DefaultMessageIdTypes) Enum.Parse(typeof (DefaultMessageIdTypes), id.ToString());
			Console.WriteLine("Message: 0x{0:x} {0} {1}", id, denum.ToString());

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
		private char _reliableMessageNumber;

		private void ReceiveCallback(IAsyncResult ar)
		{
			UdpClient listener = (UdpClient) ar.AsyncState;
			IPEndPoint senderEndpoint = new IPEndPoint(0, 0);
			Byte[] receiveBytes = listener.EndReceive(ar, ref senderEndpoint);

			int msgId = receiveBytes[0];

			if (msgId >= (int) DefaultMessageIdTypes.ID_CONNECTED_PING && msgId <= (int) DefaultMessageIdTypes.ID_USER_PACKET_ENUM)
			{
				DefaultMessageIdTypes msgIdType = (DefaultMessageIdTypes) msgId;
				Debug.Print("> Receive: {1} (0x{0:x2})", msgId, msgIdType);
				Debug.Print("\tData: {0}", ByteArrayToString(receiveBytes));

				switch (msgIdType)
				{
					case DefaultMessageIdTypes.ID_CONNECTED_PING:
						break;
					case DefaultMessageIdTypes.ID_UNCONNECTED_PING:
					case DefaultMessageIdTypes.ID_UNCONNECTED_PING_OPEN_CONNECTIONS:
					{
						var incoming = new IdUnconnectedPing();
						incoming.SetBuffer(receiveBytes);
						incoming.Decode();

						var packet = new IdUnconnectedPong();
						packet.serverId = 12345;
						packet.pingId = 100;
						packet.serverName = "MCCPP;Demo;MiNET - Another MC server"; // Magic!!!!
						packet.Encode();

						var data = packet.GetBytes();

						Debug.Print("< Send: {1} (0x{0:x2})", packet.Id, (DefaultMessageIdTypes) packet.Id);
						Debug.Print("\tData: {0}", ByteArrayToString2(data));

						SendRaw(listener, data, senderEndpoint);
						break;
					}
					case DefaultMessageIdTypes.ID_DETECT_LOST_CONNECTIONS:
						break;
					case DefaultMessageIdTypes.ID_OPEN_CONNECTION_REQUEST_1:
					{
						if (_state == ConnectionState.Connecting) break;
						_state = ConnectionState.Connecting;

						var incoming = new IdOpenConnectionRequest1();
						incoming.SetBuffer(receiveBytes);
						incoming.Decode();

						var packet = new IdOpenConnectionReply1();
						packet.serverGuid = 12345;
						packet.mtuSize = incoming.mtuSize;
						packet.serverHasSecurity = 0;
						packet.Encode();

						var data = packet.GetBytes();
						SendRaw(listener, data, senderEndpoint);
						break;
					}
					case DefaultMessageIdTypes.ID_OPEN_CONNECTION_REQUEST_2:
					{
						if (_state == ConnectionState.Connecting2) break;
						_state = ConnectionState.Connecting2;

						var incoming = new IdOpenConnectionRequest2();
						incoming.SetBuffer(receiveBytes);
						incoming.Decode();

						IdOpenConnectionReply2 packet = new IdOpenConnectionReply2();
						packet.serverGuid = 12345;
						packet.clientUdpPort = (short) senderEndpoint.Port;
						packet.mtuSize = incoming.mtuSize;
						packet.doSecurity = 0;
						packet.Encode();

						var data = packet.GetBytes();
						SendRaw(listener, data, senderEndpoint);
						break;
					}
				}
			}
			else
			{
				DatagramHeader header = new DatagramHeader(receiveBytes[0]);
				if (!header.isACK && !header.isNAK && header.isValid)
				{
					_state = ConnectionState.Connected;

					if (receiveBytes[0] != 0xa0)
					{
						byte[] buffer;
						{
							var package = new ConnectedPackage();
							package.SetBuffer(receiveBytes);
							package.Decode();

							Debug.Print("> Receive: {0} (0x{1:x2}) ", (DefaultMessageIdTypes) package.internalBuffer[0], package.internalBuffer[0]);
							Debug.Print("\tData: {0}", ByteArrayToString(receiveBytes));
							buffer = package.internalBuffer;

							SendAck(listener, senderEndpoint, package._sequenceNumber);
						}
						var message = PackageFactory.CreatePackage(buffer[0]);
						if (message != null)
						{
							message.SetBuffer(buffer);
							message.Decode();

							if (typeof (IdConnectedPing) == message.GetType())
							{
								var msg = (IdConnectedPing) message;

								var response = new IdConnectedPong();
								response.sendpingtime = msg.sendpingtime;
								response.sendpongtime = DateTimeOffset.UtcNow.Ticks/TimeSpan.TicksPerMillisecond;
								response.Encode();

								SendPackage(listener, senderEndpoint, response);
							}
							else if (typeof (IdConnectionRequest) == message.GetType())
							{
								var msg = (IdConnectionRequest) message;
								var response = new IdConnectionRequestAcceptedManual((short) senderEndpoint.Port, msg.timestamp);
								response.Encode();

								SendPackage(listener, senderEndpoint, response);
							}
							else if (typeof (IdMcpeLogin) == message.GetType())
							{
								{
									var msg = (IdMcpeLogin) message;
									msg.Decode();

									var response = new IdMcpeLoginStatus();
									response.status = 0;
									response.Encode();

									SendPackage(listener, senderEndpoint, response);
								}

								// Start game
								{
									var response = new IdMcpeStartGame();
									response.seed = 100;
									response.generator = 0;
									response.gamemode = 1;
									response.entityId = 1;
									//response.spawnX = 128;
									//response.spawnY = 4;
									//response.spawnZ = 128;
									response.x = 100;
									response.y = 2;
									response.z = 100;
									response.Encode();

									SendPackage(listener, senderEndpoint, response);
								}

								//{
								//	var response = new IdMcpeSetTime();
								//	response.time = 2154;
								//	response.started = 1;
								//	response.Encode();
								//	SendPackage(listener, senderEndpoint, response);
								//}
								//{
								//	var response = new IdMcpeSetSpawnPosition();
								//	response.x = 128;
								//	response.y = 128;
								//	response.z = 4;
								//	response.Encode();
								//	SendPackage(listener, senderEndpoint, response);
								//}
								//{
								//	var response = new IdMcpeSetHealth();
								//	response.health = 20;
								//	response.Encode();
								//	SendPackage(listener, senderEndpoint, response);
								//}
								//{
								//	// CHUNK!

								//	FlatlandGenerator generator = new FlatlandGenerator();
								//	Level level = new Level(generator, "world");
								//	Chunk chunk = generator.GenerateChunk(new Coordinates2D());
								//	byte[] data = ChunkHelper.CreatePacket(chunk);

								//	var response = new IdMcpeFullChunkDataPacket();
								//	response.chunkX = 0;
								//	response.chunkZ = 0;
								//	response.chunkData = data;
								//	response.Encode();
								//	SendPackage(listener, senderEndpoint, response);

								//}
							}
						}
					}
				}
				else if (header.isACK && header.isValid)
				{
				}
			}


			if (receiveBytes.Length != 0)
			{
				listener.BeginReceive(ReceiveCallback, listener);
			}
		}

		private void SendPackage(UdpClient listener, IPEndPoint senderEndpoint, Package message, Reliability reliability = Reliability.RELIABLE)
		{
			ConnectedPackage package = new ConnectedPackage();
			package.internalBuffer = message.GetBytes();
			package._reliability = reliability;
			package._reliableMessageNumber = _reliableMessageNumber++;
			package._sequenceNumber = _sequenceNumber++;
			package.Encode();
			byte[] data = package.GetBytes();

			Debug.Print("< Send: {1} (0x{0:x2})", data[0], (DefaultMessageIdTypes) message.Id);
			Debug.Print("\tData: {0}", ByteArrayToString(data));

			SendRaw(listener, data, senderEndpoint);
		}

		private void SendAck(UdpClient listener, IPEndPoint senderEndpoint, Int24 sequenceNumber)
		{
			ConnectedPackage connectedPackage;
			var ack = new Ack();
			ack.sequenceNumber = sequenceNumber;
			ack.count = 1;
			ack.onlyOneSequence = 1;
			ack.Encode();
			SendRaw(listener, ack._buffer.ToArray(), senderEndpoint);
		}


		private void SendRaw(UdpClient listener, byte[] data, IPEndPoint senderEndpoint)
		{
			listener.Send(data, data.Length, senderEndpoint);
			listener.BeginSend(data, data.Length, senderEndpoint, SendRequestCallback, listener);
		}

		private void SendRequestCallback(IAsyncResult ar)
		{
			UdpClient listener = (UdpClient) ar.AsyncState;
			listener.EndSend(ar);
		}

		public static string ByteArrayToString2(byte[] ba)
		{
			StringBuilder hex = new StringBuilder((ba.Length*2) + 100);
			hex.Append("{");
			foreach (byte b in ba)
				hex.AppendFormat("{0},", b);
			hex.Append("}");
			return hex.ToString();
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
		private readonly short _port;
		private readonly long _sessionId;

		public IdConnectionRequestAcceptedManual(short port, long sessionId)
		{
			_port = port;
			_sessionId = sessionId;
		}

		public void Encode()
		{
			Write((byte) 0x10);
			Write(new byte[] { 0x04, 0x3f, 0x57, 0xfe }); //Cookie
			Write((byte) 0xcd); //Security flags
			Write(_port);
			PutDataArray();
			Write(new byte[] { 0x00, 0x00 });
			Write(_sessionId);
			Write(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x04, 0x44, 0x0b, 0xa9 });
		}

		private void PutDataArray()
		{
			byte[] unknown1 = { 0xf5, 0xff, 0xff, 0xf5 };
			byte[] unknown2 = { 0xff, 0xff, 0xff, 0xff };

			Write(new Int24(unknown1.Length));
			Write(unknown1);

			for (int i = 0; i < 9; i++)
			{
				Write(new Int24(unknown2.Length));
				Write(unknown2);
			}
		}
	}
}
