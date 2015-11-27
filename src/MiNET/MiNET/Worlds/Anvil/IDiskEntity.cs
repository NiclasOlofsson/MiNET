using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using fNbt;
using fNbt.Serialization;

namespace Craft.Net.Anvil
{
    public interface IDiskEntity : INbtSerializable
    {
        string Id { get; }
    }
}
