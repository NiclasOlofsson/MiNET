using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using log4net;

namespace MiNET.Ftl.Service
{
	internal class MiNetFtlServer
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (MiNetFtlServer));

		private UdpClient _listener;

		public IPEndPoint Endpoint { get; set; }
		private ConcurrentDictionary<IPEndPoint, UdpClient> _endpointMappings = new ConcurrentDictionary<IPEndPoint, UdpClient>();
		private IPEndPoint _serverTargetEndpoint;
		private IPEndPoint _serverListenerEndPoint;


		public void StartServer()
		{
			//var client = new MiNetClient(new IPEndPoint(Dns.GetHostEntry("pe.mineplex.com").AddressList[0], 19132), "TheGrey");
			_serverTargetEndpoint = new IPEndPoint(Dns.GetHostEntry("yodamine.com").AddressList[0], 19135);
			//_serverTargetEndpoint = new IPEndPoint(IPAddress.Parse("147.75.194.126"), 19132);
			_serverListenerEndPoint = Endpoint = new IPEndPoint(IPAddress.Any, 19134);

			_listener = new UdpClient(Endpoint);
			_listener.Client.ReceiveBufferSize = int.MaxValue;
			_listener.Client.SendBufferSize = int.MaxValue;
			_listener.DontFragment = false;
			_listener.EnableBroadcast = false;

			// SIO_UDP_CONNRESET (opcode setting: I, T==3)
			// Windows:  Controls whether UDP PORT_UNREACHABLE messages are reported.
			// - Set to TRUE to enable reporting.
			// - Set to FALSE to disable reporting.

			uint IOC_IN = 0x80000000;
			uint IOC_VENDOR = 0x18000000;
			uint SIO_UDP_CONNRESET = IOC_IN | IOC_VENDOR | 12;

			_listener.Client.IOControl((int) SIO_UDP_CONNRESET, new byte[] {Convert.ToByte(false)}, null);
			_listener.BeginReceive(ReceiveCallback, _listener);
		}


		public void StopServer()
		{
			try
			{
				Log.Info("Shutting down...");
				if (_listener == null) return;

				_listener.Close();
				_listener = null;
			}
			catch (Exception e)
			{
				Log.Error("Shutdown", e);
			}
		}


		private void ReceiveCallback(IAsyncResult ar)
		{
			UdpClient listener = (UdpClient) ar.AsyncState;

			// Check if we already closed the server
			if (listener.Client == null) return;

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
				if (listener.Client != null)
				{
					try
					{
						listener.BeginReceive(ReceiveCallback, listener);
					}
					catch (ObjectDisposedException dex)
					{
					}
				}

				return;
			}

			if (receiveBytes.Length != 0)
			{
				listener.BeginReceive(ReceiveCallback, listener);
				try
				{
					ProcessMessage(receiveBytes, senderEndpoint);
				}
				catch (Exception e)
				{
					Log.Warn(string.Format("Process message error from: {0}", senderEndpoint.Address), e);
				}
			}
			else
			{
				Log.Error("Unexpected end of transmission?");
			}
		}

		private void ProcessMessage(byte[] receiveBytes, IPEndPoint senderEndpoint)
		{
			UdpClient client;
			if (!_endpointMappings.TryGetValue(senderEndpoint, out client))
			{
				client = new UdpClient(new IPEndPoint(IPAddress.Any, 0));
				client.Client.ReceiveBufferSize = int.MaxValue;
				client.Client.SendBufferSize = int.MaxValue;
				client.DontFragment = false;
				client.EnableBroadcast = false;
				uint IOC_IN = 0x80000000;
				uint IOC_VENDOR = 0x18000000;
				uint SIO_UDP_CONNRESET = IOC_IN | IOC_VENDOR | 12;
				client.Client.IOControl((int) SIO_UDP_CONNRESET, new byte[] {Convert.ToByte(false)}, null);

				//client.Connect(senderEndpoint);
				client.BeginReceive(ClientRequestCallback, client);
				_endpointMappings.TryAdd(senderEndpoint, client);
			}

			client.Send(receiveBytes, receiveBytes.Length, _serverTargetEndpoint);
			Log.Info($"Forwarded message from c {senderEndpoint} -> s {_serverTargetEndpoint} 0x{receiveBytes[0]:x2}");
		}

		private void ClientRequestCallback(IAsyncResult ar)
		{
			UdpClient client = (UdpClient) ar.AsyncState;

			IPEndPoint endpoint = _endpointMappings.FirstOrDefault(pair => pair.Value == client).Key;

			// Check if we already closed the server
			if (client.Client == null) return;

			// WSAECONNRESET:
			// The virtual circuit was reset by the remote side executing a hard or abortive close. 
			// The application should close the socket; it is no longer usable. On a UDP-datagram socket 
			// this error indicates a previous send operation resulted in an ICMP Port Unreachable message.
			// Note the spocket settings on creation of the server. It makes us ignore these resets.
			IPEndPoint senderEndpoint = new IPEndPoint(0, 0);
			Byte[] receiveBytes = null;
			try
			{
				receiveBytes = client.EndReceive(ar, ref senderEndpoint);
			}
			catch (Exception e)
			{
				if (client.Client != null)
				{
					try
					{
						client.BeginReceive(ClientRequestCallback, client);
					}
					catch (ObjectDisposedException dex)
					{
					}
				}

				return;
			}

			if (receiveBytes.Length != 0)
			{
				client.BeginReceive(ClientRequestCallback, client);
				try
				{
					_listener.Send(receiveBytes, receiveBytes.Length, endpoint);
					Log.Info($"Forwarded message from s {_serverListenerEndPoint} -> c {endpoint} 0x{receiveBytes[0]:x2}");
				}
				catch (Exception e)
				{
					Log.Warn($"Process message error from: {senderEndpoint.Address}", e);
				}
			}
			else
			{
				Log.Error("Unexpected end of transmission?");
			}
		}
	}
}