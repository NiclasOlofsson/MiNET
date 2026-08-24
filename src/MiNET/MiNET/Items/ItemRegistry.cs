#region LICENSE

// The contents of this file are subject to the Common Public Attribution
// License Version 1.0. (the "License"); you may not use this file except in
// compliance with the License. You may obtain a copy of the License at
// https://github.com/NiclasOlofsson/MiNET/blob/master/LICENSE.
// The License is based on the Mozilla Public License Version 1.1, but Sections 14
// and 15 have been added to cover use of software over a computer network and
// provide for limited attribution for the Original Developer. In addition, Exhibit A has
// been modified to be consistent with Exhibit B.
//
// Software distributed under the License is distributed on an "AS IS" basis,
// WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License for
// the specific language governing rights and limitations under the License.
//
// The Original Code is MiNET.
//
// The Original Developer is the Initial Developer.  The Initial Developer of
// the Original Code is Niclas Olofsson.
//
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2026 Niclas Olofsson.
// All Rights Reserved.

#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using fNbt;

namespace MiNET.Items
{
	/// <summary>
	///     One item identity as the item_registry packet declares it. <see cref="Name" /> is the
	///     durable identity; <see cref="NetworkId" /> is only the number this protocol version
	///     assigned it and means nothing outside a session.
	/// </summary>
	public class ItemRegistryEntry
	{
		public string Name { get; }

		public short NetworkId { get; }

		/// <summary>
		///     Whether the item's behaviour is data driven rather than hardcoded in the client. Note
		///     this is not the same question as "carries components": 75 items set the flag, 78 carry
		///     component NBT, and the two sets only partly overlap. BDS reports them independently.
		/// </summary>
		public bool ComponentBased { get; }

		/// <summary>Registry entry version as sent by vanilla BDS (0, 1 or 2).</summary>
		public int Version { get; }

		/// <summary>
		///     The component blob, already serialized as network NBT (little endian, varint) and
		///     written to the wire verbatim. Null for the items that carry no components. These are
		///     the exact bytes BDS sends, so nothing has to build an NBT tree to send the registry.
		/// </summary>
		public byte[] ComponentNbt { get; }

		public ItemRegistryEntry(string name, short networkId, bool componentBased, int version, byte[] componentNbt)
		{
			Name = name;
			NetworkId = networkId;
			ComponentBased = componentBased;
			Version = version;
			ComponentNbt = componentNbt;
		}

		public override string ToString()
		{
			return $"{Name} (network id {NetworkId})";
		}
	}

	/// <summary>
	///     The item type dictionary the server declares to the client. Content comes from
	///     <see cref="ItemRegistryData" />, which MiNET.BlockGen generates from the BDS memory
	///     extraction (MiNET.BdsExtract/Data items-runtime.json) and checks against a captured BDS
	///     item_registry frame. Entries are in name order; BDS sends its own map order, which
	///     nothing here reproduces and the client does not depend on.
	///     No item has network id 0. The wire uses 0 for "empty stack", so a name that isn't in the
	///     registry resolves to 0 and degrades to an empty slot rather than to some other item.
	/// </summary>
	public class ItemRegistry : IReadOnlyList<ItemRegistryEntry>
	{
		private readonly List<ItemRegistryEntry> _entries = new List<ItemRegistryEntry>();
		private readonly Dictionary<string, ItemRegistryEntry> _byName = new Dictionary<string, ItemRegistryEntry>(StringComparer.OrdinalIgnoreCase);
		private readonly Dictionary<short, ItemRegistryEntry> _byNetworkId = new Dictionary<short, ItemRegistryEntry>();

		/// <summary>
		///     Called by the generated registry data. The tree is built once, here, and kept as the
		///     network NBT bytes the item_registry packet writes verbatim.
		/// </summary>
		public void Add(string name, short networkId, bool componentBased, int version, NbtCompound components)
		{
			byte[] nbt = null;
			if (components != null)
			{
				// The wire carries an unnamed root holding the "components" compound.
				var root = new NbtCompound("") {components};
				nbt = new NbtFile(root) {BigEndian = false, UseVarInt = true}.SaveToBuffer(NbtCompression.None);
			}

			var entry = new ItemRegistryEntry(name, networkId, componentBased, version, nbt);

			_entries.Add(entry);
			_byName[name] = entry;
			_byNetworkId[networkId] = entry;
		}

		public int Count => _entries.Count;

		public ItemRegistryEntry this[int index] => _entries[index];

		public IEnumerator<ItemRegistryEntry> GetEnumerator()
		{
			return _entries.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public bool TryGetByName(string name, out ItemRegistryEntry entry)
		{
			return _byName.TryGetValue(Qualify(name), out entry);
		}

		public bool TryGetByNetworkId(short networkId, out ItemRegistryEntry entry)
		{
			return _byNetworkId.TryGetValue(networkId, out entry);
		}

		public bool Contains(string name)
		{
			return _byName.ContainsKey(Qualify(name));
		}

		/// <summary>
		///     The network id for a registry string id, or 0 when the name is not in the registry.
		///     0 is the empty stack everywhere on the wire, so an unknown name sends nothing rather
		///     than the wrong item.
		/// </summary>
		public short GetNetworkId(string name)
		{
			if (string.IsNullOrEmpty(name)) return 0;
			return _byName.TryGetValue(Qualify(name), out ItemRegistryEntry entry) ? entry.NetworkId : (short) 0;
		}

		/// <summary>The registry string id for a network id, or null when the id is not in the registry.</summary>
		public string GetName(short networkId)
		{
			return _byNetworkId.TryGetValue(networkId, out ItemRegistryEntry entry) ? entry.Name : null;
		}

		private static string Qualify(string name)
		{
			if (string.IsNullOrEmpty(name)) return name;
			return name.IndexOf(':') >= 0 ? name : "minecraft:" + name;
		}
	}
}
