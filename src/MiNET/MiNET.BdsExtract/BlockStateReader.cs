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
using System.Text.Json.Nodes;

namespace MiNET.BdsExtract;

/// <summary>One state property and its value, as the server holds it.</summary>
public readonly record struct StateProperty(string Name, object Value)
{
	/// <summary>The value as the kind it is: a byte is the bool the server means by it.</summary>
	public JsonNode ToNode()
	{
		return Value switch
		{
			byte b => JsonValue.Create(b != 0),
			int i => JsonValue.Create((long) i),
			string s => JsonValue.Create(s),
			_ => null
		};
	}

	/// <summary>The same value as the text a state key is built from.</summary>
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

/// <summary>
///     The state properties a block declares, read from the map the block holds rather than worked
///     out from the states that exist. BlockType keeps them as
///     <c>std::map&lt;uint64, BlockStateInstance&gt;</c>: two words, the head node and the count.
///     A node is the tree's three links and its colour, then the pair, so the property id sits at
///     +32 and the instance at +40.
///     The instance's numbers were placed by the only thing that can settle them, which is that
///     they have to agree with each other: acacia_button reads 1 bit ending at bit 3 with mask 8,
///     and facing_direction 3 bits ending at bit 2 with mask 7, and in both the mask is exactly the
///     bits the width and the end bit describe. The declaration order in the published header does
///     not match, and memory wins over a header.
///     The instance points at the BlockState, which carries the property's own id and name. That
///     name is a HashedString, so it hashes to its own text or it is not read at all.
/// </summary>
public static class BlockStateDefinitions
{
	private const int Key = 32;
	private const int Instance = 40;
	private const int NodeSize = 72;

	// Inside the instance.
	private const int VariationCount = 0;
	private const int NumBits = 4;
	private const int EndBit = 8;
	private const int Mask = 12;
	private const int State = 16;

	// Inside the BlockState the instance points at.
	private const int StateId = 8;
	private const int StateVariations = 16;
	private const int StateName = 24;

	/// <summary>
	///     The block's property name to id index, read from the unordered_map it holds. That map is
	///     a load factor, then the list its entries actually live in, then the bucket vector, so
	///     only the list is walked: buckets are an index into it and hold nothing of their own.
	///     A node is next, previous, then the pair, so the name sits at +16 as a HashedString and
	///     the id at +64. The name verifies against its own hash or it is not read.
	/// </summary>
	public static JsonObject Names(BedrockProcess process, ulong at)
	{
		var word = new byte[8];
		var index = new JsonObject();
		ulong head = process.ReadUInt64(at + ListHead, word);
		long count = (long) process.ReadUInt64(at + ListHead + 8, word);
		if (head < 0x10000 || count <= 0 || count > 64) return index;

		var node = new byte[72];
		var scratch = new byte[256];
		ulong walk = process.ReadUInt64(head, word);
		for (long i = 0; i < count && walk >= 0x10000 && walk != head; i++)
		{
			if (!process.TryRead(walk, node, node.Length)) break;
			string name = HashedString.ReadVerified(process, node, EntryName, scratch);
			if (name is not null) index[name] = (long) BitConverter.ToUInt64(node, EntryId);
			walk = BitConverter.ToUInt64(node, 0);
		}

		return index;
	}

	/// <summary>Where the entries live inside the map, and where each node keeps its pair.</summary>
	private const int ListHead = 8;
	private const int EntryName = 16;
	private const int EntryId = 64;

	public static JsonObject Read(BedrockProcess process, ulong at)
	{
		var word = new byte[8];
		var declared = new JsonObject();
		ulong head = process.ReadUInt64(at, word);
		long count = (long) process.ReadUInt64(at + 8, word);
		if (head < 0x10000 || count <= 0 || count > 64) return declared;

		Walk(process, process.ReadUInt64(head + 8, word), declared, 0);
		return declared;
	}

	private static void Walk(BedrockProcess process, ulong node, JsonObject declared, int depth)
	{
		if (node < 0x10000 || depth > 32) return;

		var body = new byte[NodeSize];
		if (!process.TryRead(node, body, body.Length)) return;

		// The sentinel marks itself, which is what stops the walk running off the top of the tree.
		if (body[25] != 0) return;

		Walk(process, BitConverter.ToUInt64(body, 0), declared, depth + 1);

		var scratch = new byte[256];
		ulong state = BitConverter.ToUInt64(body, Instance + State);
		var head = new byte[64];
		string name = state >= 0x10000 && process.TryRead(state, head, head.Length)
			? HashedString.ReadVerified(process, head, StateName, scratch)
			: null;

		var stated = new JsonObject
		{
			["id"] = (long) BitConverter.ToUInt64(body, Key),
			["values"] = BitConverter.ToUInt32(body, Instance + VariationCount),
			["bits"] = BitConverter.ToUInt32(body, Instance + NumBits),
			["endBit"] = BitConverter.ToUInt32(body, Instance + EndBit),
			["mask"] = BitConverter.ToUInt32(body, Instance + Mask)
		};

		declared[name ?? $"unnamed{BitConverter.ToUInt64(body, Key)}"] = stated;

		Walk(process, BitConverter.ToUInt64(body, 16), declared, depth + 1);
	}
}

/// <summary>
///     A CompoundTag out of the process, whole, as the tags it holds.
///     <para>
///         The map walk is the same one <see cref="BlockStateReader" /> does, because it is the same
///         container: a head sentinel whose parent is the root, nodes of three links and a colour,
///         then the key, then the value. What is different is the typing. A block state holds four
///         kinds of tag under three known keys, so the reader there learns each type from the key it
///         sits under. An item's user data holds anything, so nothing can be learned from a key.
///     </para>
///     <para>
///         The type comes from the value itself. A map entry holds a CompoundTagVariant, which is a
///         std::variant over the twelve tag classes, and a variant states which of them it holds in
///         a byte after the storage. That byte is the type, directly, for every entry.
///     </para>
///     <para>
///         It is also checked rather than believed. Every tag of one class shares one method table,
///         so the byte and the table have to agree one to one across the whole population: a class
///         appearing under two type numbers, or a type number appearing with two classes, means the
///         byte is not the type and the run says so. The same table is then what types the elements
///         of a list, which hold pointers to tags and no variant of their own.
///     </para>
/// </summary>
public sealed class CompoundTagReader
{
	private const int VectorBegin = 0;
	private const int VectorEnd = 8;

	private readonly BedrockProcess _process;
	private readonly byte[] _node = new byte[32];
	private readonly byte[] _word = new byte[8];
	private readonly byte[] _text = new byte[4096];

	/// <summary>Which type number each method table belongs to, learned from the variants.</summary>
	private readonly Dictionary<ulong, int> _typeOf = new();
	private readonly Dictionary<int, ulong> _tableOf = new();

	/// <summary>How the reference names the twelve tag classes, by the type number each is.</summary>
	private static readonly string[] TagClasses =
	[
		"EndTag", "ByteTag", "ShortTag", "IntTag", "Int64Tag", "FloatTag",
		"DoubleTag", "ByteArrayTag", "StringTag", "ListTag", "CompoundTag", "IntArrayTag"
	];

	public CompoundTagReader(BedrockProcess process)
	{
		_process = process;
	}

	/// <summary>Entries whose method table and type byte name two different classes.</summary>
	public int Disagreed { get; private set; }

	/// <summary>Entries read, across every compound this reader has been asked for.</summary>
	public int Entries { get; private set; }

	/// <summary>List elements whose class no variant has ever named, so their type is unread.</summary>
	public int UntypedElements { get; private set; }

	/// <summary>Compounds whose walk found a different number of nodes than the map states.</summary>
	public int Miscounted { get; private set; }

	/// <summary>What each class number was seen as, for the report.</summary>
	public IReadOnlyDictionary<int, ulong> Tables => _tableOf;

	/// <summary>
	///     Clears the counts and keeps what has been learned, so a first pass over the whole
	///     population can name the classes and the pass that writes the file counts only itself.
	///     Without it the first compound read is judged on a table nothing has filled in yet: every
	///     enchanted book came out with its list unread, and the class it wanted was named half a
	///     thousand entries later by an item further down the list.
	/// </summary>
	public void Reset()
	{
		Disagreed = 0;
		Entries = 0;
		UntypedElements = 0;
		Miscounted = 0;
	}

	/// <summary>Where a tag class keeps its value, which is past its method table.</summary>
	private static int Payload(string held) =>
		BlockMembers.Held(held, BlockMembers.Source.Creative).Members.First(m => m.Name == "data").At;

	/// <summary>Where a compound keeps its map.</summary>
	private static int TagsAt =>
		BlockMembers.Held("CompoundTag", BlockMembers.Source.Creative).Members.First(m => m.Name == "tags").At;

	/// <summary>Where a variant states which tag it holds.</summary>
	private static int WhichAt =>
		BlockMembers.Held("CompoundTagVariant", BlockMembers.Source.Creative).Members.First(m => m.Name == "which").At;

	/// <summary>
	///     The compound at an address, as an object of its keys. Null where the address holds no
	///     readable map; an empty object is a compound that really is empty.
	/// </summary>
	public JsonObject Read(ulong compound, int depth = 0)
	{
		if (compound < 0x10000 || depth > 16 || !_process.IsMapped(compound)) return null;

		ulong head = _process.ReadUInt64(compound + (ulong) TagsAt, _word);
		ulong stated = _process.ReadUInt64(compound + (ulong) TagsAt + 8, _word);
		if (head < 0x10000) return null;

		List<ulong> nodes = Nodes(head);
		if ((ulong) nodes.Count != stated) Miscounted++;

		var held = new JsonObject();
		foreach (ulong node in nodes)
		{
			string key = StdString(node + MemoryLayout.MapNodeKey) ?? "unreadKey";
			ulong table = _process.ReadUInt64(node + MemoryLayout.MapNodeVtable, _word);
			int which = -1;
			if (_process.TryRead(node + (ulong) (MemoryLayout.MapNodeVtable + WhichAt), _word, 1)) which = _word[0];

			Entries++;
			if (which is < 0 or >= 12)
			{
				held[key] = new JsonObject
				{
					["type"] = "unread",
					["reason"] = "the variant states a class number no tag has",
					["stated"] = which
				};
				continue;
			}

			Remember(table, which);
			held[key] = Decode(which, node + MemoryLayout.MapNodeVtable, depth);
		}

		return held;
	}

	/// <summary>
	///     Ties a method table to a class number, and counts it when the two disagree with what was
	///     already seen. Nothing is corrected: the first pairing stands and the mismatch is reported.
	/// </summary>
	private void Remember(ulong table, int which)
	{
		if (table < 0x10000) return;
		if (_typeOf.TryGetValue(table, out int already))
		{
			if (already != which) Disagreed++;
			return;
		}

		if (_tableOf.TryGetValue(which, out ulong other) && other != table)
		{
			Disagreed++;
			return;
		}

		_typeOf[table] = which;
		_tableOf[which] = table;
	}

	/// <summary>One tag, at the address its own object starts at, as the value it holds.</summary>
	private JsonNode Decode(int which, ulong tag, int depth)
	{
		string held = TagClasses[which];

		// Two of the twelve classes state no member called data: the end tag holds nothing at all,
		// and a compound holds a map, which is read from its own offset by the walk below. Asking
		// either of them where its value sits is an error rather than a missing number.
		ulong payload = which is 0 or 10 ? tag : tag + (ulong) Payload(held);

		JsonNode value = which switch
		{
			0 => null,
			1 => Bytes(payload, 1) is { } b ? JsonValue.Create((long) b[0]) : null,
			2 => Bytes(payload, 2) is { } s ? JsonValue.Create((long) BitConverter.ToInt16(s, 0)) : null,
			3 => Bytes(payload, 4) is { } i ? JsonValue.Create((long) BitConverter.ToInt32(i, 0)) : null,
			4 => Bytes(payload, 8) is { } l ? JsonValue.Create(BitConverter.ToInt64(l, 0)) : null,
			5 => Bytes(payload, 4) is { } f ? Sentinels.Number("nbt", BitConverter.ToSingle(f, 0)) : null,
			6 => Bytes(payload, 8) is { } d ? JsonValue.Create(BitConverter.ToDouble(d, 0)) : null,
			7 => Numbers(payload, 1),
			8 => StdString(payload) is { } t ? JsonValue.Create(t) : null,
			9 => Elements(payload, depth),
			10 => Read(tag, depth + 1),
			11 => Numbers(payload, 4),
			_ => null
		};

		return new JsonObject { ["type"] = held[..^3], ["value"] = value };
	}

	/// <summary>
	///     The tags a list holds. A list is a vector of pointers to tags, and a tag carries no type
	///     of its own, so each element is typed by the method table the variants already named. An
	///     element of a class no variant has named is emitted saying so, with its table, rather than
	///     guessed at or dropped.
	/// </summary>
	private JsonArray Elements(ulong vector, int depth)
	{
		var held = new JsonArray();
		ulong begin = _process.ReadUInt64(vector + VectorBegin, _word);
		ulong end = _process.ReadUInt64(vector + VectorEnd, _word);
		if (begin < 0x10000 || end < begin || end - begin > 1 << 20 || (end - begin) % 8 != 0) return held;

		for (ulong at = begin; at < end; at += 8)
		{
			ulong tag = _process.ReadUInt64(at, _word);
			if (tag < 0x10000) continue;

			ulong table = _process.ReadUInt64(tag, _word);
			if (!_typeOf.TryGetValue(table, out int which))
			{
				UntypedElements++;
				held.Add(new JsonObject
				{
					["type"] = "unread",
					["reason"] = "no variant has named this class, so what the element is was never read"
				});
				continue;
			}

			held.Add(Decode(which, tag, depth + 1));
		}

		return held;
	}

	/// <summary>A vector of fixed width numbers, which is what the two array tags hold.</summary>
	private JsonArray Numbers(ulong vector, int width)
	{
		var held = new JsonArray();
		ulong begin = _process.ReadUInt64(vector + VectorBegin, _word);
		ulong end = _process.ReadUInt64(vector + VectorEnd, _word);
		if (begin < 0x10000 || end < begin || end - begin > 1 << 20 || (end - begin) % (ulong) width != 0) return held;

		var body = new byte[end - begin];
		if (body.Length == 0) return held;
		if (!_process.TryRead(begin, body, body.Length)) return held;

		for (int at = 0; at + width <= body.Length; at += width)
		{
			held.Add(width == 1 ? body[at] : BitConverter.ToInt32(body, at));
		}

		return held;
	}

	private byte[] Bytes(ulong at, int length)
	{
		var body = new byte[length];
		return _process.TryRead(at, body, length) ? body : null;
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
		pending.Push(_process.ReadUInt64(head + MemoryLayout.MapNodeParent, _word));

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
	public string StdString(ulong at)
	{
		var header = new byte[32];
		if (at < 0x10000 || !_process.TryRead(at, header, header.Length)) return null;

		ulong length = BitConverter.ToUInt64(header, 16);
		ulong capacity = BitConverter.ToUInt64(header, 24);
		if (length > capacity || length >= (ulong) _text.Length) return null;
		if (length == 0) return "";

		if (capacity < 16) return Encoding.ASCII.GetString(header, 0, (int) length);

		ulong pointer = BitConverter.ToUInt64(header, 0);
		if (pointer < 0x10000 || !_process.TryRead(pointer, _text, (int) length)) return null;
		return Encoding.ASCII.GetString(_text, 0, (int) length);
	}
}
