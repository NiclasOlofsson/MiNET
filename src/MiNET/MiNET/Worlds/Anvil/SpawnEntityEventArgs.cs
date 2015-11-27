using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Anvil
{
    public class SpawnEntityEventArgs : EventArgs
    {
        public object Entity { get; set; }

        public SpawnEntityEventArgs(object entity)
        {
            Entity = entity;
        }
    }
}
