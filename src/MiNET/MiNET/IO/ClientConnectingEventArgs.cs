using MiNET.Net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MiNET.IO
{
    public class ClientConnectingEventArgs : CancelEventArgs
    {
        public OpenConnectionRequest2 Request { get; private set; }
        public IPEndPoint Sender { get; private set; }
        public ClientConnectingEventArgs(OpenConnectionRequest2 request, IPEndPoint senderEndpoint)
        {
            this.Request = request;
            this.Sender = senderEndpoint;
            this.Cancel = false;
        }
    }
}
