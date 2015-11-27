using Craft.Net.Common;
using fNbt.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Anvil
{
    public abstract class TileEntity
    {
        [NbtIgnore]
        public Coordinates3D Coordinates
        {
            get
            {
                return new Coordinates3D(X, Y, Z);
            }
        }

        [TagName("id")]
        public abstract string Id { get; }
        [TagName("x")]
        public int X { get; set; }
        [TagName("y")]
        public int Y { get; set; }
        [TagName("z")]
        public int Z { get; set; }
    }
}
