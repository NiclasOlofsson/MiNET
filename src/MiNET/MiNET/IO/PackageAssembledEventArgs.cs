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
    public class PackageAssembledEventArgs : EventArgs
    {
        public PlayerNetworkSession Session { get; set; }
        public Package Package { get; set; }

        public PackageAssembledEventArgs(PlayerNetworkSession session, Package request)
        {
            this.Session = session;
            this.Package = request;
        }
    }
}
