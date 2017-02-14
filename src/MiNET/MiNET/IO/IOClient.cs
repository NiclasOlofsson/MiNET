using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using log4net;
using MiNET.Utils;
using MiNET.Net;

namespace MiNET.IO
{
    internal class IOClient
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MiNetServer));

        UdpClient _udpClient;

        [Obsolete("Should be set in constructor to ensure always exists")]
        public ServerInfo ServerInfo { get; set; }

        private DedicatedThreadPool _receiveThreadPool;

        public IOClient(IPEndPoint endpoint)
        {
            _receiveThreadPool = new DedicatedThreadPool(new DedicatedThreadPoolSettings(Environment.ProcessorCount));

            this._udpClient = CreateListener(endpoint);

            new Thread(ProcessDatagrams) { IsBackground = true }.Start();
        }

        protected virtual UdpClient CreateListener(IPEndPoint endpoint)
        {
            var listener = new UdpClient(endpoint);
            ConfigureListener(listener);
            return listener;
        }

        protected virtual void ConfigureListener(UdpClient listener)
        {
            //_listener.Client.ReceiveBufferSize = 1600*40000;
            listener.Client.ReceiveBufferSize = int.MaxValue;
            //_listener.Client.SendBufferSize = 1600*40000;
            listener.Client.SendBufferSize = int.MaxValue;
            listener.DontFragment = false;
            listener.EnableBroadcast = false;

            // SIO_UDP_CONNRESET (opcode setting: I, T==3)
            // Windows:  Controls whether UDP PORT_UNREACHABLE messages are reported.
            // - Set to TRUE to enable reporting.
            // - Set to FALSE to disable reporting.

            uint IOC_IN = 0x80000000;
            uint IOC_VENDOR = 0x18000000;
            uint SIO_UDP_CONNRESET = IOC_IN | IOC_VENDOR | 12;
            listener.Client.IOControl((int)SIO_UDP_CONNRESET, new byte[] { Convert.ToByte(false) }, null);

            //
            //WARNING: We need to catch errors here to remove the code above.
            //
        }

        internal void Send(Package packet, IPEndPoint senderEndpoint)
        {
            var data = packet.Encode();
            packet.PutPool();
            TraceSend(packet);

            this.Send(data, senderEndpoint);
        }

        public void Send(byte[] data, IPEndPoint target)
        {
            try
            {
                this.SendRaw(data, target);

                Interlocked.Increment(ref ServerInfo.NumberOfPacketsOutPerSecond);
                Interlocked.Add(ref ServerInfo.TotalPacketSizeOut, data.Length);
            }
            catch (ObjectDisposedException e)
            {
            }
            catch (Exception e)
            {
                //if (this.Udp == null || this.Udp.Client != null) Log.Error(string.Format("Send data lenght: {0}", data.Length), e);
            }
        }

        public void SendRaw(byte[] data, IPEndPoint target)
        {
            this._udpClient.Send(data, data.Length, target);
        }

        private static void TraceSend(Package message)
        {
            if (!Log.IsDebugEnabled) return;
            //if (!Debugger.IsAttached) return;

            Log.DebugFormat("<    Send: {0}: {1} (0x{0:x2})", message.Id, message.GetType().Name);
        }


        public void Close()
        {
            if (this.IsClosed)
                return;

#warning Check if this is really a thread-safe way to stop processing
            this._udpClient.Close();
            this._udpClient = null;
        }

        public bool IsClosed
        {
            get
            {
                return this._udpClient?.Client == null;
            }
        }

        private void ProcessDatagrams(/*object state*/)
        {
            while (true)
            {
                if (this.IsClosed)
                    return;

                try
                {
                    if (!processDatagram())
                        return;
                }
                catch (Exception e)
                {
                    Log.Error("Unexpected end of transmission?", e);
                    if (this.IsClosed)
                        return;
                }

            }
        }

        private bool processDatagram()
        {
            //var result = listener.ReceiveAsync().Result;
            //senderEndpoint = result.RemoteEndPoint;
            //receiveBytes = result.Buffer;
            IPEndPoint senderEndpoint = null;
            var receiveBytes = this._udpClient.Receive(ref senderEndpoint);

            Interlocked.Exchange(ref ServerInfo.AvailableBytes, this._udpClient.Available);
            Interlocked.Increment(ref ServerInfo.NumberOfPacketsInPerSecond);
            Interlocked.Add(ref ServerInfo.TotalPacketSizeIn, receiveBytes.Length);

            if (receiveBytes.Length == 0)
            {
                Log.Error("Unexpected end of transmission?");
                return false;
            }

            _receiveThreadPool.QueueUserWorkItem(() => processDatagramAsync(receiveBytes, senderEndpoint));

            return true;
        }

        private void processDatagramAsync(byte[] receiveBytes, IPEndPoint senderEndpoint)
        {
            try
            {
                if (!OnReceived(new ReceivedDataEventArgs(senderEndpoint, receiveBytes)))
                    return;
            }
            catch (Exception e)
            {
                Log.Warn(string.Format("Process message error from: {0}", senderEndpoint.Address), e);
            }
        }

        public event EventHandler<ReceivedDataEventArgs> ReceivedData;
        protected bool OnReceived(ReceivedDataEventArgs e)
        {
            ReceivedData(this, e);
            return e.Cancel;
        }
    }

    internal class IOClient_Mono : IOClient
    {
        public IOClient_Mono(IPEndPoint endpoint)
            : base(endpoint)
        {
        }

        protected override void ConfigureListener(UdpClient listener)
        {
            //base.ConfigureListener(listener);
            listener.Client.ReceiveBufferSize = 1024 * 1024 * 3;
            listener.Client.SendBufferSize = 4096;
        }
    }
}
