using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MiNET.IO
{
    public class ReceivedDataEventArgs : CancelEventArgs
    {
        public IPEndPoint Sender { get; private set; }
        public byte[] Data { get; private set; }
        public ReceivedDataEventArgs(IPEndPoint sender, byte[] data)
        {
            this.Data = data;
            this.Sender = sender;
            this.Cancel = false;
        }
    }
}
