using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MiNET.Network
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


	public class MiNetServer
	{
		private ConnectionState _state = ConnectionState.Waiting;
		private int _sequenceNumber;
		private char _reliableMessageNumber;
		private IPEndPoint _endpoint;
		private UdpClient _listener;
		public const int DefaultPort = 19132;

		private MiNetServer()
		{
		}

		public MiNetServer(int port)
			: this(new IPEndPoint(IPAddress.Any, port))
		{
		}

		public MiNetServer(IPEndPoint endpoint = null)
		{
			_endpoint = endpoint ?? new IPEndPoint(IPAddress.Any, DefaultPort);
		}

		public bool StartServer()
		{
			if (_listener != null) return false; // Already started

			try
			{
				_listener = new UdpClient(_endpoint);

				// SIO_UDP_CONNRESET (opcode setting: I, T==3)
				// Windows:  Controls whether UDP PORT_UNREACHABLE messages are reported.
				// - Set to TRUE to enable reporting.
				// - Set to FALSE to disable reporting.

				uint IOC_IN = 0x80000000;
				uint IOC_VENDOR = 0x18000000;
				uint SIO_UDP_CONNRESET = IOC_IN | IOC_VENDOR | 12;
				_listener.Client.IOControl((int) SIO_UDP_CONNRESET, new byte[] { Convert.ToByte(false) }, null);

				// We need to catch errors here to remove the code above.
				_listener.BeginReceive(ReceiveCallback, _listener);

				//while (true) Thread.Yield();

				return true;
			}
			catch (Exception e)
			{
				Debug.Write(e);
				StopServer();
			}

			return false;
		}

		public bool StopServer()
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

			// WSAECONNRESET:
			// The virtual circuit was reset by the remote side executing a hard or abortive close. 
			// The application should close the socket; it is no longer usable. On a UDP-datagram socket 
			// this error indicates a previous send operation resulted in an ICMP Port Unreachable message.
			// Note the spocket settings on creation of the server. It makes us ignore these resets.
			IPEndPoint senderEndpoint = new IPEndPoint(0, 0);
			Byte[] receiveBytes = listener.EndReceive(ar, ref senderEndpoint);

			int msgId = receiveBytes[0];

			if (msgId >= (int) DefaultMessageIdTypes.ID_CONNECTED_PING && msgId <= (int) DefaultMessageIdTypes.ID_USER_PACKET_ENUM)
			{
				DefaultMessageIdTypes msgIdType = (DefaultMessageIdTypes) msgId;
				if (msgIdType != DefaultMessageIdTypes.ID_CONNECTED_PING && msgIdType != DefaultMessageIdTypes.ID_UNCONNECTED_PING)
				{
					Debug.Print("> Receive: {1} (0x{0:x2})", msgId, msgIdType);
					Debug.Print("\tData: {0}", ByteArrayToString(receiveBytes));
				}
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

						//Debug.Print("< Send: {1} (0x{0:x2})", packet.Id, (DefaultMessageIdTypes) packet.Id);
						//Debug.Print("\tData: {0}", ByteArrayToString2(data));

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

							buffer = package.internalBuffer;

							SendAck(listener, senderEndpoint, package._sequenceNumber);
						}
						var message = PackageFactory.CreatePackage(buffer[0]);
						if (message == null)
						{
							Debug.Print("> Receive Unkown: {0} (0x{1:x2}) ", (DefaultMessageIdTypes) buffer[0], buffer[0]);
							Debug.Print("\tData: {0}", ByteArrayToString(receiveBytes));
						}
						else
						{
							if (message.Id != (decimal) DefaultMessageIdTypes.ID_CONNECTED_PING && message.Id != (decimal) DefaultMessageIdTypes.ID_UNCONNECTED_PING)
							{
								Debug.Print("> Receive: {0} (0x{1:x2}) ", (DefaultMessageIdTypes) message.Id, message.Id);
								Debug.Print("\tData: {0}", ByteArrayToString(receiveBytes));
							}

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
								var response = new ConnectionRequestAcceptedManual((short) senderEndpoint.Port, msg.timestamp);
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
									response.seed = 1406827239;
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

								{
									var response = new IdMcpeSetTime();
									response.time = 6000;
									response.started = 1;
									response.Encode();
									SendPackage(listener, senderEndpoint, response);
								}
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
								{
									//	// CHUNK!

									//Level level = new Level(generator, "world");
									//FlatlandGenerator generator = new FlatlandGenerator();
									//Chunk chunk = generator.GenerateChunk(new Coordinates2D());
									//byte[] data = ChunkHelper.CreatePacket(chunk);

									//var response = new IdMcpeFullChunkDataPacket();
									//response.chunkX = 0;
									//response.chunkZ = 0;
									//response.chunkData = data;
									//response.Encode();
									//	SendPackage(listener, senderEndpoint, response);
								}
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

			if (message.Id != (decimal) DefaultMessageIdTypes.ID_CONNECTED_PONG && message.Id != (decimal) DefaultMessageIdTypes.ID_UNCONNECTED_PONG)
			{
				Debug.Print("< Send: {1} (0x{0:x2})", data[0], (DefaultMessageIdTypes) message.Id);
				Debug.Print("\tData: {0}", ByteArrayToString(data));
			}
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


		public static string ByteArrayToString(byte[] ba)
		{
			StringBuilder hex = new StringBuilder((ba.Length*2) + 100);
			hex.Append("{");
			foreach (byte b in ba)
				hex.AppendFormat("0x{0:x2},", b);
			hex.Append("}");
			return hex.ToString();
		}


		private enum ConnectionState
		{
			Waiting,
			Connecting,
			Connecting2,
			Connected,
		}
	}
}
