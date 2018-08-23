
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MiNET.Net;
namespace MiNET.Utils
{

    public class ScoreboardIdentityPackets : List<ScoreboardIdentityPacket> { }

    public class ScoreboardIdentityPacket
    {
        public long scoreboardId { get; set; }
        public UUID uuid { get; set; }

        
    }
}