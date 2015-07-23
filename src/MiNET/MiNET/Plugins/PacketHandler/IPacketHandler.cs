using MiNET.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiNET.Plugins
{
    public interface IPacketHandler
    {
        Package OnReceive(Package package, Player player);

        Package OnSend(Package package, Player player);

        List<Type> GetPackageTypes();
    }
}
