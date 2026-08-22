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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2020 Niclas Olofsson.
// All Rights Reserved.

#endregion

using System.Text;

namespace MiNET.BdsExtract;

/// <summary>One state property and its value, as the server holds it.</summary>
public readonly record struct StateProperty(string Name, object Value)
{
	/// <summary>The value written the way a block state expects it: bools as bools, the rest as they are.</summary>
	public string ToJson()
	{
		return Value switch
		{
			byte b => b != 0 ? "true" : "false",
			int i => i.ToString(),
			string s => $"\"{s}\"",
			_ => "null"
		};
	}
}

/// <summary>
///     Reads what a block state actually is, the "facing east, upside down" part.
///     Every Block points at the serialized NBT for its own state, the same compound of name,
///     states and version that the network id is a hash of. That compound is an MSVC std::map, so
///     reading it is a tree walk: a head sentinel whose parent is the real root, and nodes holding
///     left, parent and right pointers, then the key, then the value.
///     The value is a tag object, and which tag it is shows in its method table pointer. Those
///     addresses move whenever the server is rebuilt, so they are never written down here. They
///     are learned on the spot from three entries whose type is already known: name is a string,
///     version is an int, states is a compound. Whatever vtable those three have is what that type
///     looks like in this particular server, and everything else is read against them.
/// </summary>
public sealed class BlockStateReader
{
	private readonly BedrockProcess _process;
	private readonly Dictionary<ulong, TagKind> _tags = new();
	// The traversal decodes only a node's links and its sentinel flag, bytes 0 through 25; key,
	// tag and payload are separate exact reads by address. Reading a node's full declared size
	// here can straddle out of the allocation into an uncommitted page and fail whole, which
	// silently drops the node and every state below it.
	private readonly byte[] _node = new byte[32];
	private readonly byte[] _text = new byte[256];

	private enum TagKind
	{
		Unknown,
		Byte,
		Int,
		String,
		Compound
	}

	/// <summary>
	///     Learns the tag types from the palette itself. It takes more than one entry: name,
	///     version and states name three of the four types, but nothing teaches what a bool looks
	///     like except a block that has one, and plenty of blocks have no state properties at all.
	///     So entries are read until all four types are known.
	/// </summary>
	public BlockStateReader(BedrockProcess process, IEnumerable<ulong> sampleBlocks)
	{
		_process = process;
		foreach (ulong block in sampleBlocks)
		{
			Learn(block);
			if (IsReady) break;
		}
	}

	/// <summary>True once all four tag types are known: byte, int, string and compound.</summary>
	public bool IsReady => _tags.Values.Distinct().Count() >= 4;

	/// <summary>The state properties of one block state, empty when the block has none.</summary>
	public List<StateProperty> Read(ulong block, out int version)
	{
		version = 0;
		var properties = new List<StateProperty>();
		ulong head = _process.ReadUInt64(block + (ulong) MemoryLayout.BlockStateNbt, _text);
		if (head < 0x10000) return properties;

		foreach (ulong node in Nodes(head))
		{
			string key = StdString(node + MemoryLayout.MapNodeKey);
			switch (key)
			{
				case "version":
					if (Value(node) is int v) version = v;
					break;
				case "states":
					if (Value(node) is ulong inner && inner >= 0x10000)
					{
						foreach (ulong entry in Nodes(inner))
						{
							string name = StdString(entry + MemoryLayout.MapNodeKey);
							object value = Value(entry);
							if (name is not null && value is not null)
							{
								properties.Add(new StateProperty(name, value));
							}
						}
					}
					break;
			}
		}

		properties.Sort((a, b) => string.CompareOrdinal(a.Name, b.Name));
		return properties;
	}

	private void Learn(ulong block)
	{
		ulong head = _process.ReadUInt64(block + (ulong) MemoryLayout.BlockStateNbt, _text);
		if (head < 0x10000) return;

		foreach (ulong node in Nodes(head))
		{
			ulong vtable = _process.ReadUInt64(node + MemoryLayout.MapNodeVtable, _text);
			if (vtable < 0x10000) continue;

			switch (StdString(node + MemoryLayout.MapNodeKey))
			{
				case "name":
					_tags[vtable] = TagKind.String;
					break;
				case "version":
					_tags[vtable] = TagKind.Int;
					break;
				case "states":
					_tags[vtable] = TagKind.Compound;
					break;
			}
		}

		// Bools are the one type the three known keys cannot teach, because none of them is one.
		// Any vtable inside a states compound that is not already known is that type, and there
		// is only ever one such vtable.
		foreach (ulong node in Nodes(head))
		{
			if (StdString(node + MemoryLayout.MapNodeKey) != "states") continue;
			if (Value(node) is not ulong inner || inner < 0x10000) continue;
			foreach (ulong entry in Nodes(inner))
			{
				ulong vtable = _process.ReadUInt64(entry + MemoryLayout.MapNodeVtable, _text);
				if (vtable >= 0x10000 && !_tags.ContainsKey(vtable)) _tags[vtable] = TagKind.Byte;
			}
		}
	}

	private object Value(ulong node)
	{
		ulong vtable = _process.ReadUInt64(node + MemoryLayout.MapNodeVtable, _text);
		if (!_tags.TryGetValue(vtable, out var kind)) return null;

		ulong payload = node + MemoryLayout.MapNodePayload;
		switch (kind)
		{
			case TagKind.Byte:
				return _process.TryRead(payload, _text, 1) ? _text[0] : null;
			case TagKind.Int:
				return _process.TryRead(payload, _text, 4) ? BitConverter.ToInt32(_text, 0) : (object) null;
			case TagKind.String:
				return StdString(payload);
			case TagKind.Compound:
				return _process.ReadUInt64(payload, _text);
			default:
				return null;
		}
	}

	/// <summary>
	///     Every node of the map. The head sentinel is not a node and marks itself in the byte after
	///     its colour, which is what stops the walk running off the top of the tree.
	/// </summary>
	private List<ulong> Nodes(ulong head)
	{
		var found = new List<ulong>();
		var seen = new HashSet<ulong>();
		var pending = new Stack<ulong>();
		pending.Push(_process.ReadUInt64(head + MemoryLayout.MapNodeParent, _text));

		// A malformed tree would otherwise loop forever, and no block has anything like this many.
		// The seen set keeps a malformed one from reporting a node twice on its way to the guard.
		for (int guard = 0; pending.Count > 0 && guard < 8192; guard++)
		{
			ulong node = pending.Pop();
			if (node < 0x10000 || node == head || !seen.Add(node)) continue;
			if (!_process.TryRead(node, _node, _node.Length)) continue;
			if (_node[MemoryLayout.MapNodeFlags + 1] == 1) continue;

			found.Add(node);
			pending.Push(BitConverter.ToUInt64(_node, MemoryLayout.MapNodeLeft));
			pending.Push(BitConverter.ToUInt64(_node, MemoryLayout.MapNodeRight));
		}
		return found;
	}

	/// <summary>An MSVC std::string: sixteen bytes of union, then the length, then the capacity.</summary>
	private string StdString(ulong at)
	{
		var header = new byte[32];
		if (!_process.TryRead(at, header, header.Length)) return null;

		ulong length = BitConverter.ToUInt64(header, 16);
		ulong capacity = BitConverter.ToUInt64(header, 24);
		if (length == 0 || length > capacity || length >= (ulong) _text.Length) return null;

		if (capacity < 16) return Encoding.ASCII.GetString(header, 0, (int) length);

		ulong pointer = BitConverter.ToUInt64(header, 0);
		if (pointer < 0x10000 || !_process.TryRead(pointer, _text, (int) length)) return null;
		return Encoding.ASCII.GetString(_text, 0, (int) length);
	}
}
