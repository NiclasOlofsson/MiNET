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

namespace MiNET
{
	public class MiNetServer
	{
		private const int DefaultPort = 19132;

		private IPEndPoint _endpoint;
		private UdpClient _listener;
		private Dictionary<IPEndPoint, Player> _playerEndpoints;
		private Level _level;

		public MiNetServer() : this(new IPEndPoint(IPAddress.Any, DefaultPort))
		{
		}

		public MiNetServer(int port) : this(new IPEndPoint(IPAddress.Any, port))
		{
		}

		public MiNetServer(IPEndPoint endpoint)
		{
			_endpoint = new IPEndPoint(IPAddress.Any, DefaultPort);
		}

		private Queue<UdpClient> _clients = new Queue<UdpClient>();
		private UdpClient _sender = null;

		public static bool IsRunningOnMono()
		{
			return Type.GetType("Mono.Runtime") != null;
		}

		public bool StartServer()
		{
			if (_listener != null) return false; // Already started

			try
			{
				_playerEndpoints = new Dictionary<IPEndPoint, Player>();

				_level = new Level("Default");
				_level.Initialize();

				_listener = new UdpClient(_endpoint);

				_listener.Client.ReceiveBufferSize = 1024*1024*3;

				_listener.Client.SendBufferSize = 4096;

				// SIO_UDP_CONNRESET (opcode setting: I, T==3)
				// Windows:  Controls whether UDP PORT_UNREACHABLE messages are reported.
				// - Set to TRUE to enable reporting.
				// - Set to FALSE to disable reporting.
				if (!IsRunningOnMono())
				{
					_listener.Client.ReceiveBufferSize = 1024 * 1024 * 8;
					_listener.Client.SendBufferSize = 1024 * 1024 * 8;
					_listener.DontFragment = true;

					uint IOC_IN = 0x80000000;
					uint IOC_VENDOR = 0x18000000;
					uint SIO_UDP_CONNRESET = IOC_IN | IOC_VENDOR | 12;
					_listener.Client.IOControl((int) SIO_UDP_CONNRESET, new byte[] {Convert.ToByte(false)}, null);
				}
				//
				//WARNING: We need to catch errors here to remove the code above.
				//

				/*
                 * We need to do something about this above. Has to be both compatible for Mono and Windows
                 */

				_listener.BeginReceive(ReceiveCallback, _listener);

				Console.WriteLine("Server open for business...");

				return true;
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
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

			ThreadPool.QueueUserWorkItem(CallBack(receiveBytes, senderEndpoint));
			if (receiveBytes.Length != 0)
			{
				listener.BeginReceive(ReceiveCallback, listener);
			}
			else
			{
				Debug.Write("Unexpected end of transmission?");
			}
		}

		private WaitCallback CallBack(byte[] receiveBytes, IPEndPoint senderEndpoint)
		{
			//TODO: Uggly. Looks like a refactoring accident that needs to change.
			return delegate(object state)
			{
				byte msgId = receiveBytes[0];

				if (msgId <= (byte) DefaultMessageIdTypes.ID_USER_PACKET_ENUM)
				{
					DefaultMessageIdTypes msgIdType = (DefaultMessageIdTypes) msgId;

					Package message = PackageFactory.CreatePackage(msgId, receiveBytes);

					TraceReceive(message);

					switch (msgIdType)
					{
						case DefaultMessageIdTypes.ID_UNCONNECTED_PING:
						case DefaultMessageIdTypes.ID_UNCONNECTED_PING_OPEN_CONNECTIONS:
						{
							UnconnectedPing incoming = (UnconnectedPing) message;

							//TODO: This needs to be verified with RakNet first
							//response.sendpingtime = msg.sendpingtime;
							//response.sendpongtime = DateTimeOffset.UtcNow.Ticks / TimeSpan.TicksPerMillisecond;

							var packet = new UnconnectedPong
							{
								serverId = 12345,
								pingId = 100 /*incoming.pingId*/,
								serverName = "MCCPP;Demo;MiNET - Another MC server"
							};
							var data = packet.Encode();
							TraceSend(packet);
							SendData(data, senderEndpoint);
							break;
						}
						case DefaultMessageIdTypes.ID_OPEN_CONNECTION_REQUEST_1:
						{
							OpenConnectionRequest1 incoming = (OpenConnectionRequest1) message;

							var packet = new OpenConnectionReply1
							{
								serverGuid = 12345,
								mtuSize = incoming.mtuSize,
								serverHasSecurity = 0
							};

							var data = packet.Encode();
							TraceSend(packet);
							SendData(data, senderEndpoint);
							break;
						}
						case DefaultMessageIdTypes.ID_OPEN_CONNECTION_REQUEST_2:
						{
							OpenConnectionRequest2 incoming = (OpenConnectionRequest2) message;

							var packet = new OpenConnectionReply2
							{
								serverGuid = 12345,
								mtuSize = incoming.mtuSize,
								doSecurityAndHandshake = new byte[0]
							};

							lock (_playerEndpoints)
							{
								Debug.WriteLine("Settled on MTU: {0}", incoming.mtuSize);
								_playerEndpoints.Remove(senderEndpoint);
								_playerEndpoints.Add(senderEndpoint, new Player(this, senderEndpoint, _level, incoming.mtuSize));
							}
							var data = packet.Encode();
							TraceSend(packet);
							SendData(data, senderEndpoint);
							break;
						}
					}

					message.PutPool();
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

						ConnectedPackage package = new ConnectedPackage();
						package.Decode(receiveBytes);
						var messages = package.Messages;

						foreach (var message in messages)
						{
							TraceReceive(message);
							SendAck(senderEndpoint, package._datagramSequenceNumber);
							HandlePackage(message, senderEndpoint);
							message.PutPool();
						}
					}
					else if (header.isACK && header.isValid)
					{
						Ack ack = new Ack();
						ack.Decode(receiveBytes);
						//Debug.WriteLine("ACK #{0}", ack.sequenceNumber.IntValue());
					}
					else if (header.isNAK && header.isValid)
					{
						Nak nak = new Nak();
						nak.Decode(receiveBytes);
						Console.WriteLine("!--> NAK on #{0}", nak.sequenceNumber.IntValue());
						Debug.WriteLine("!--> NAK on #{0}", nak.sequenceNumber.IntValue());
					}
					else if (!header.isValid)
					{
						Console.WriteLine("!!!! ERROR, Invalid header !!!!!");
						Debug.WriteLine("!!!! ERROR, Invalid header !!!!!");
					}
				}
			};
		}

		private void HandlePackage(Package message, IPEndPoint senderEndpoint)
		{
			if (typeof (UnknownPackage) == message.GetType())
			{
				return;
			}

			if (_playerEndpoints.ContainsKey(senderEndpoint))
			{
				_playerEndpoints[senderEndpoint].HandlePackage(message);
			}

			if (typeof (DisconnectionNotification) == message.GetType())
			{
				_playerEndpoints.Remove(senderEndpoint);
			}
		}

		private int _dropCountPerSecond = 0;

		public ObjectPool<MessagePart> _messagePartPool = new ObjectPool<MessagePart>(() => new MessagePart());
		public ObjectPool<Datagram> _datagramPool = new ObjectPool<Datagram>(() => new Datagram(0));

		public void SendPackage(IPEndPoint senderEndpoint, List<Package> messages, short mtuSize, ref int datagramSequenceNumber, ref int reliableMessageNumber, Reliability reliability = Reliability.RELIABLE)
		{
			if (messages.Count == 0) return;

			var datagrams = Datagram.CreateDatagrams(messages, mtuSize, ref datagramSequenceNumber, ref reliableMessageNumber, _messagePartPool, _datagramPool);
			foreach (var datagram in datagrams)
			{
				byte[] data = datagram.Encode();

				SendData(data, senderEndpoint);

				foreach (var part in datagram.MessageParts)
				{
					if (part.MessagePartPool != null)
					{
						part.Buffer = null;
						part.Reset();
						part.MessagePartPool.PutObject(part);
					}
				}

				if (datagram.Pool != null)
				{
					datagram.Reset();
					datagram.Pool.PutObject(datagram);
				}
			}

			foreach (var message in messages)
			{
				message.PutPool();
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

			_numberOfAckSent++;

			_listener.Send(data, data.Length, senderEndpoint);
		}

		private long _numberOfAckSent = 0;
		private long _numberOfPacketsSentPerSecond = 0;
		private long _totalPacketSize = 0;
		private Timer _throughPut = null;

		private void SendData(byte[] data, IPEndPoint targetEndpoint)
		{
			if (_throughPut == null)
			{
				_throughPut = new Timer(delegate(object state)
				{
					double kbytesPerSecond = _totalPacketSize*8/1000000D;
					long avaragePacketSize = _totalPacketSize/(_numberOfPacketsSentPerSecond + 1);
					Console.WriteLine("TT {5}ms {6} player(s) Pkt {0}/s ACKs {1}/s AvSize: {2}b Thoughput: {3:F}Mbit/s",
						_numberOfPacketsSentPerSecond, _numberOfAckSent, avaragePacketSize, kbytesPerSecond,
						_dropCountPerSecond, _level.lastTickProcessingTime, _level.Players.Count);

					_dropCountPerSecond = 0;
					_numberOfAckSent = 0;
					_totalPacketSize = 0;
					_numberOfPacketsSentPerSecond = 0;
				}, null, 1000, 1000);
			}

			//_listener.SendAsync(data, data.Length, targetEndpoint);
			_listener.Send(data, data.Length, targetEndpoint);
			//_listener.BeginSend(data, data.Length, targetEndpoint, null, null);

			_numberOfPacketsSentPerSecond++;
			_totalPacketSize += data.Length;
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

		private static void TraceReceive(Package message)
		{
			if (message.Id != (int) DefaultMessageIdTypes.ID_CONNECTED_PING && message.Id != (int) DefaultMessageIdTypes.ID_UNCONNECTED_PING)
			{
				Debug.Print("> Receive: {0}: {1} (0x{0:x2})", message.Id, message.GetType().Name);
			}
		}

		private static void TraceSend(Package message)
		{
			if (message.Id != (int) DefaultMessageIdTypes.ID_CONNECTED_PONG && message.Id != (int) DefaultMessageIdTypes.ID_UNCONNECTED_PONG)
			{
				Debug.Print("<    Send: {0}: {1} (0x{0:x2})", message.Id, message.GetType().Name);
				//Debug.Print("\tData: Length={1} {0}", ByteArrayToString(data), data.Length);
			}
		}

		private static void TraceSend(Datagram datagram, byte[] data)
		{
			//Debug.Print("< Send: Data: Length={1} {0}", ByteArrayToString(data), data.Length);
		}
	}
}