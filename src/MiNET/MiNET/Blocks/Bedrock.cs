using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiNET.Blocks
{
    class Bedrock : Block
    {
        public Bedrock() : base(7)
        {
            Durability = 60000;
        }
    }
}
