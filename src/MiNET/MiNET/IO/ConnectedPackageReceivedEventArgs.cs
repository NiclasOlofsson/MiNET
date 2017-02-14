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
    public class ConnectedPackageReceivedEventArgs : EventArgs
    {
        public PlayerNetworkSession Session { get; set; }
        public ConnectedPackage Package { get; set; }

        public ConnectedPackageReceivedEventArgs(PlayerNetworkSession session, ConnectedPackage package)
        {
            this.Session = session;
            this.Package = package;
        }
    }
}
