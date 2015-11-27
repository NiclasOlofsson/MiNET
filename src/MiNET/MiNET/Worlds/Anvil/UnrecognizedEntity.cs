using fNbt;
using fNbt.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craft.Net.Anvil
{
    /// <summary>
    /// Used to allow unrecognized entity types to remain in the world.
    /// </summary>
    internal class UnrecognizedEntity : IDiskEntity
    {
        public UnrecognizedEntity(string id)
        {
            Id = id;
        }

        public string Id { get; private set; }

        private NbtTag NbtValue { get; set; }

        public NbtTag Serialize(string tagName)
        {
            return NbtValue;
        }

        public void Deserialize(NbtTag value)
        {
            NbtValue = value;
        }
    }
}
