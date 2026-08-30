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

using System.Text;
using System.Text.Json.Nodes;
using MiNET.BdsExtract.Binary;

namespace MiNET.BdsExtract;

/// <summary>
///     The block definitions the server keeps for its data driven blocks: the objects StartGame's
///     blockProperties list is serialized from, read at the offsets the server's own builder reads
///     them at.
///     <para>
///         BlockType is the block; a BlockDefinition is what the definition the block was built from
///         still states about it, and three things the wire carries live only here. The archetype's
///         type name, which is a string the definition holds and not the C++ class of the block. The
///         archetype's own value, whose class differs per archetype: a slab names the other half of
///         its pair, a stair names the block it is a stair of, a wall foliage carries four numbers.
///         And the permutations, each a Molang condition and the components it applies.
///     </para>
///     <para>
///         The class names come from the binary. Clang bakes the instantiated type into the cereal
///         factory and loader signatures, so the build itself states
///         SharedTypes::v1_26_20::BlockDefinition::SlabArchetype, StairArchetype, WallFoliageArchetype
///         and BlockPermutation, and the EnTT binder states VanillaBlockData with block_id at 0,
///         material at 4, translucency at 56 and the two vibration flags at 64 and 65. Every one of
///         those offsets is where the live object holds that value, which is the second witness.
///         Members no binding names carry the name the server's own serializer writes on the wire.
///     </para>
///     <para>
///         Nothing here is found by adjacency. The definitions live in a map keyed by the block's own
///         full name; a candidate is a std::string whose length agrees with its text, sitting where a
///         map node's key sits, whose value states the same block id the BlockType of that name
///         states. From one such node the list is walked around to its own start, so what the
///         container holds is the answer and what it does not hold is not.
///     </para>
/// </summary>
public static class BlockDefinitions
{
	/// <summary>Where a map node keeps its key and its value, counted from the node.</summary>
	private const int KeyInNode = 0x10;

	private const int ValueInNode = 0x30;

	/// <summary>The archetype value's box: entt's any, sixteen bytes of storage, then the type it holds and how.</summary>
	private const int BoxSize = 0x40;

	private const int TypeHashInBox = 0x20;
	private const int PolicyInBox = 0x24;

	/// <summary>The box holds the value itself rather than a pointer to it.</summary>
	private const byte Embedded = 2;

	/// <summary>One definition: where it is and what was read out of it.</summary>
	public sealed record Definition(string Name, ulong Address, JsonObject Read);

	/// <summary>
	///     What the executable states about the method tables the definition side builds its
	///     components on. A definition component carries no id, so the table is the only identity
	///     it has, and the image is what says which class that table is and whether the server
	///     puts it on the wire. Null before <see cref="Read" /> has run, and then a component
	///     travels as its table and nothing else.
	/// </summary>
	private static BinaryFacts _facts;

	/// <summary>How many kinds of component were met, and what each of them answered.</summary>
	private static readonly Dictionary<string, (int Count, bool? Networked)> _kinds = new(StringComparer.Ordinal);

	/// <summary>
	///     Every definition the map holds, by the block it names, and what the walk counted.
	///     <para>
	///         Two passes over the server's committed memory. The first takes every place the text of
	///         a block's full name sits, found from the one prefix they all share rather than from a
	///         search per name. The second takes every std::string whose buffer is one of those texts
	///         and whose own length is that text's length, and asks whether it sits where a node's key
	///         sits: the value at the node states a block id, and it has to be the id the BlockType of
	///         that same name states. Arbitrary bytes do not satisfy both.
	///     </para>
	/// </summary>
	public static (IReadOnlyDictionary<string, Definition> Definitions, string Report) Read(
		BedrockProcess process, IReadOnlyList<BlockProperties> blocks, BinaryFacts facts = null)
	{
		_facts = facts;
		_kinds.Clear();

		// The class states how long the object is and where inside it the block id sits, so neither
		// is a number this file keeps.
		ClassLayout definition = Held("BlockDefinition");
		int size = definition.Size;
		int idAtInDefinition = At(definition, "vanillaBlockData") + At(Held("VanillaBlockData"), "blockId");
		var ids = new Dictionary<string, int>(StringComparer.Ordinal);
		var word = new byte[8];
		int idAt = BlockLayout.At("id");
		foreach (BlockProperties block in blocks)
		{
			if (process.TryRead(block.Address + (ulong) idAt, word, 2)) ids[block.Name] = BitConverter.ToUInt16(word, 0);
		}

		Dictionary<ulong, string> texts = NameTexts(process, ids.Keys);
		List<ulong> nodes = Nodes(process, texts, ids, size, idAtInDefinition);
		if (nodes.Count == 0)
		{
			return (new Dictionary<string, Definition>(),
				$"block definitions: none found from {texts.Count:N0} name text(s); the map was not reached");
		}

		// The container states its own membership. A node's first word is the link to the next, and
		// the list is circular, so a walk that comes back to where it started has been round the
		// whole of it. A candidate whose walk does not close is not in a list and is not used: the
		// walk that closes is the container, and the ones that do not are counted and reported.
		List<ulong> walked = null;
		int open = 0;
		foreach (ulong candidate in nodes)
		{
			List<ulong> ring = Ring(process, candidate, word);
			if (ring is null) { open++; continue; }
			if (walked is null || ring.Count > walked.Count) walked = ring;
		}

		if (walked is null)
		{
			return (new Dictionary<string, Definition>(),
				$"block definitions: {nodes.Count:N0} node(s) found and none of them lies in a list that closes");
		}

		var found = new Dictionary<string, Definition>(StringComparer.Ordinal);
		var window = new byte[size];
		var scratch = new byte[512];
		int disagreed = 0, unnamed = 0;
		foreach (ulong node in walked)
		{
			string name = Key(process, node, scratch);
			if (name is null || !ids.TryGetValue(name, out int id)) { unnamed++; continue; }
			ulong address = process.ReadUInt64(node + ValueInNode, word);
			if (address < 0x10000 || !process.IsMapped(address)) { unnamed++; continue; }
			if (process.ReadClipped(address, window, size) != size) { unnamed++; continue; }
			if (BitConverter.ToInt32(window, idAtInDefinition) != id) { disagreed++; continue; }
			found[name] = new Definition(name, address, One(process, address, window, scratch));
		}

		string report = $"block definitions: {found.Count:N0} of {blocks.Count:N0} blocks, found from "
						+ $"{nodes.Count:N0} node(s) whose value states the block's own id and walked to "
						+ $"{walked.Count:N0} node(s) in the list they belong to; {disagreed:N0} whose value "
						+ $"states another block's id, {unnamed:N0} the walk could not name, "
						+ $"{open:N0} found node(s) in no list that closes";
		return (found, report);
	}

	/// <summary>
	///     The list one node belongs to, or null when following its links never comes back. The
	///     links are followed, never a stride: a ring that closes is the container stating its own
	///     membership, and a chain that does not close is not one.
	/// </summary>
	private static List<ulong> Ring(BedrockProcess process, ulong start, byte[] word)
	{
		var ring = new List<ulong> { start };
		ulong at = start;
		for (var guard = 0; guard < 65536; guard++)
		{
			if (!process.TryRead(at, word, 8)) return null;
			at = BitConverter.ToUInt64(word, 0);
			if (at == start) return ring;
			if (at < 0x10000 || !process.IsMapped(at)) return null;
			ring.Add(at);
		}

		return null;
	}

	/// <summary>
	///     What one definition holds. The class states the layout and the reader reads it; what is
	///     added here is the three places the class table cannot state on its own: the archetype's
	///     value, whose class the archetype's own name decides, the components, which are objects of
	///     classes this extraction has no layout for, and the permutations, each of which carries
	///     both.
	/// </summary>
	private static JsonObject One(BedrockProcess process, ulong address, byte[] window, byte[] scratch)
	{
		ClassLayout layout = Held("BlockDefinition");
		JsonObject read = BlockMemberReader.Held(process, address, window, layout, 0, scratch);

		if (read["vanillaBlockData"] is JsonObject data)
		{
			var names = new List<string>();
			foreach (JsonNode node in data["blockArchetypeNames"] as JsonArray ?? [])
			{
				names.Add(node?["value"]?.GetValue<string>());
			}

			data["blockArchetype"] = Archetypes(process,
				address + (ulong) (At(layout, "vanillaBlockData") + At(Held("VanillaBlockData"), "blockArchetype")),
				names, scratch);
		}

		read["components"] = Carried(process, address + (ulong) At(layout, "components"), scratch);
		read["permutations"] = Permutations(process, address + (ulong) At(layout, "permutations"), scratch);
		return read;
	}

	/// <summary>Where a member sits, as the class states it. Nothing here carries an offset of its own.</summary>
	private static int At(ClassLayout held, string member) =>
		held.Members.First(m => m.Name == member).At;

	private static ClassLayout Held(string name) => BlockMembers.Held(name, BlockMembers.Source.Blocks);

	/// <summary>
	///     The class that holds an archetype's value, named the way the build names it. Every
	///     archetype the server registers has a class whose name is the registered name without its
	///     "_block" tail, run together, with "Archetype" behind it: the binary states all 27 of them
	///     in its cereal factory and loader signatures, so this is reading the build's own naming
	///     rather than a table this tool keeps. A name no class is declared for reads as no class.
	/// </summary>
	private static ClassLayout ArchetypeClass(string name)
	{
		if (string.IsNullOrEmpty(name)) return null;
		string stem = name.EndsWith("_block", StringComparison.Ordinal) ? name[..^"_block".Length] : name;
		var built = new StringBuilder();
		foreach (string part in stem.Split('_', StringSplitOptions.RemoveEmptyEntries))
		{
			built.Append(char.ToUpperInvariant(part[0])).Append(part[1..]);
		}

		return BlockMembers.Any(built.Append("Archetype").ToString());
	}

	/// <summary>
	///     The archetype values, one per name the definition states. Each is an entt any: sixteen
	///     bytes that either are the value or point at it, and behind them the type id of what it
	///     holds and which of the two that is. The class comes from the name beside it, so a value is
	///     only read as a class when the definition itself said that is what it is; anything else
	///     travels as the bytes with the type id that names it, because a box this run cannot name is
	///     a value read and not understood rather than a value to leave out.
	/// </summary>
	private static JsonArray Archetypes(BedrockProcess process, ulong at, IReadOnlyList<string> names, byte[] scratch)
	{
		var list = new JsonArray();
		var header = new byte[24];
		if (!process.TryRead(at, header, header.Length)) return list;

		ulong begin = BitConverter.ToUInt64(header, 0);
		ulong end = BitConverter.ToUInt64(header, 8);
		if (begin < 0x10000 || end < begin || end - begin > 4096) return list;

		var box = new byte[BoxSize];
		var word = new byte[8];
		for (ulong e = begin; e + BoxSize <= end; e += BoxSize)
		{
			if (!process.TryRead(e, box, BoxSize)) break;
			int index = (int) ((e - begin) / BoxSize);
			string name = index < names.Count ? names[index] : null;
			var stated = new JsonObject
			{
				["name"] = name,
				["typeHash"] = $"0x{BitConverter.ToUInt32(box, TypeHashInBox):X8}"
			};

			ClassLayout held = ArchetypeClass(name);
			byte policy = box[PolicyInBox];
			ulong value = policy == Embedded ? e : BitConverter.ToUInt64(box, 0);
			if (held is null || value < 0x10000)
			{
				stated["unread"] = held is null
					? "no class is declared for this archetype"
					: "the box states no value";
				list.Add(stated);
				continue;
			}

			var body = new byte[held.Size];
			if (!process.TryRead(value, body, body.Length))
			{
				stated["unread"] = held.Size;
				list.Add(stated);
				continue;
			}

			stated["class"] = held.Name;
			foreach ((string member, JsonNode read) in BlockMemberReader.Held(process, value, body, held, 0, scratch))
			{
				stated[member] = read?.DeepClone();
			}

			list.Add(stated);
		}

		// The names the definition states and the values it holds are one list read twice; a name
		// with no value beside it is stated rather than dropped.
		for (int extra = list.Count; extra < names.Count; extra++)
		{
			list.Add(new JsonObject { ["name"] = names[extra], ["unread"] = "the definition holds no value for this name" });
		}

		return list;
	}

	/// <summary>
	///     The components a definition or a permutation carries. Each is a heap object whose first
	///     word is its own method table, and the table is the whole of its identity: there is no id
	///     beside it the way the block side has one.
	///     <para>
	///         What the image states about that table travels with every component: whether the
	///         server networks it, which is the rule its own builder applies, and the class it is
	///         where the reference declares one. A table the reference names no class for is still
	///         emitted, as the table and the size of what was not read, because a component left
	///         out makes the definition look like it holds fewer than it does.
	///     </para>
	/// </summary>
	private static JsonArray Carried(BedrockProcess process, ulong at, byte[] scratch)
	{
		var list = new JsonArray();
		var header = new byte[24];
		if (!process.TryRead(at, header, header.Length)) return list;

		ulong begin = BitConverter.ToUInt64(header, 0);
		ulong end = BitConverter.ToUInt64(header, 8);
		if (begin < 0x10000 || end < begin || end - begin > 65536) return list;

		var word = new byte[8];
		for (ulong e = begin; e + 16 <= end; e += 16)
		{
			ulong component = process.ReadUInt64(e, word);
			if (component < 0x10000) { list.Add(null); continue; }

			ulong table = process.ReadUInt64(component, word);
			var read = new JsonObject { ["methodTable"] = $"0x{table:X}" };
			BlockDescriptionTable stated = null;
			if (_facts is not null && _facts.TryDescription(table, out BlockDescriptionTable found)) stated = found;

			if (stated is null)
			{
				read["unread"] = "the binary states nothing about this component's method table";
				Met("(no table in the binary)", null);
				list.Add(read);
				continue;
			}

			read["networked"] = stated.Networked;
			(ClassLayout held, string why) = Describes(stated);
			if (held is null)
			{
				read["unread"] = why;
				// Keyed by the table as well as the reason, because two tables can fail to be
				// identified for the same reason and answer differently about the wire: one line
				// for both said 326 objects were networked when 227 of them are not.
				Met($"0x{stated.Rva:X}: {why}", stated.Networked);
				list.Add(read);
				continue;
			}

			read["class"] = held.Name;
			Met(held.Name, stated.Networked);
			var body = new byte[held.Size];
			if (process.ReadClipped(component, body, held.Size) != held.Size)
			{
				read["unread"] = held.Size;
				list.Add(read);
				continue;
			}

			foreach ((string member, JsonNode value) in BlockMemberReader.Held(process, component, body, held, 0, scratch))
			{
				read[member] = value?.DeepClone();
			}

			list.Add(read);
		}

		return list;
	}

	/// <summary>
	///     Which declared class a method table is, or the reason it is none of them. The reference
	///     states, per class, the component class the table's slots have to name and the tag
	///     literals they have to reference; the image states what each table actually does name.
	///     One class satisfying it is the answer, and more than one is a reference that does not
	///     tell them apart, which is said rather than picked between.
	/// </summary>
	private static (ClassLayout Held, string Why) Describes(BlockDescriptionTable table)
	{
		var matched = new List<ClassLayout>();
		foreach (ClassLayout held in BlockMembers.All(BlockMembers.Source.Blocks))
		{
			bool named = table.Names.Contains(held.Name, StringComparer.Ordinal)
						|| table.Constructs.Contains(held.Name, StringComparer.Ordinal);
			if (held.Installs is null && held.States is null && !named) continue;
			if (held.Installs is not null && !table.Installs.Contains(held.Installs, StringComparer.Ordinal)) continue;
			if (held.States is not null && held.States.Any(t => !table.States.Contains(t, StringComparer.Ordinal))) continue;
			matched.Add(held);
		}

		if (matched.Count == 1) return (matched[0], null);
		if (matched.Count > 1)
		{
			return (null, $"the reference declares {matched.Count} classes for this table: "
						+ string.Join(", ", matched.Select(m => m.Name)));
		}

		string builds = table.Installs.Count == 0 ? "no component class" : string.Join(", ", table.Installs);
		string tags = table.States.Count == 0 ? "no tag" : string.Join(", ", table.States);
		string own = table.Names.Count == 0 ? "no class of its own" : string.Join(", ", table.Names);
		string any = table.Constructs.Count == 0 ? "no class in an entt any" : string.Join(", ", table.Constructs);
		return (null, $"no class is declared for a table that names {builds}, states {tags}, "
					+ $"is called {own} and is constructed as {any}");
	}

	private static void Met(string kind, bool? networked)
	{
		_kinds.TryGetValue(kind, out (int Count, bool? Networked) held);
		_kinds[kind] = (held.Count + 1, networked);
	}

	/// <summary>
	///     What the run met on the definition side, one line per kind of component: how many
	///     objects, whether the build networks them, and the class they were read as. A kind that
	///     no class was declared for says so here rather than only inside the rows.
	/// </summary>
	public static IEnumerable<string> Kinds()
	{
		foreach ((string kind, (int count, bool? networked)) in _kinds.OrderBy(k => k.Key, StringComparer.Ordinal))
		{
			string verdict = networked switch
			{
				true => "networked",
				false => "not networked",
				_ => "no networked verdict"
			};
			yield return $"  {kind,-58} {count,6:N0} object(s), {verdict}";
		}
	}

	/// <summary>
	///     The permutations, in the order the definition holds them. The condition is the Molang the
	///     wire carries, read back from the compiled expression the way every other expression in
	///     this extraction is; the components are the ones the permutation applies.
	/// </summary>
	private static JsonArray Permutations(BedrockProcess process, ulong at, byte[] scratch)
	{
		var list = new JsonArray();
		ClassLayout layout = Held("BlockPermutation");
		var header = new byte[24];
		if (!process.TryRead(at, header, header.Length)) return list;

		ulong begin = BitConverter.ToUInt64(header, 0);
		ulong end = BitConverter.ToUInt64(header, 8);
		if (begin < 0x10000 || end < begin || end - begin > 1 << 20) return list;
		if ((end - begin) % (ulong) layout.Size != 0) return list;

		var body = new byte[layout.Size];
		for (ulong e = begin; e + (ulong) layout.Size <= end; e += (ulong) layout.Size)
		{
			if (!process.TryRead(e, body, body.Length)) break;
			JsonObject read = BlockMemberReader.Held(process, e, body, layout, 0, scratch);
			read["components"] = Carried(process, e + (ulong) At(layout, "components"), scratch);
			list.Add(read);
		}

		return list;
	}

	/// <summary>The key of a map node, or null when what sits there is not one of the names.</summary>
	private static string Key(BedrockProcess process, ulong node, byte[] scratch)
	{
		var head = new byte[32];
		if (!process.TryRead(node + KeyInNode, head, head.Length)) return null;

		ulong size = BitConverter.ToUInt64(head, 16);
		ulong capacity = BitConverter.ToUInt64(head, 24);
		if (size == 0 || size > capacity || size > 256) return null;
		if (capacity < 16) return Encoding.UTF8.GetString(head, 0, (int) size);

		ulong text = BitConverter.ToUInt64(head, 0);
		if (text < 0x10000) return null;
		return process.ReadClipped(text, scratch, (int) size) == (int) size
			? Encoding.UTF8.GetString(scratch, 0, (int) size)
			: null;
	}

	/// <summary>
	///     Every place in the server where the text of one of the given names sits. Searched by the
	///     namespace every one of them starts with rather than name by name: one pass over the
	///     server's memory instead of one per block, and what is kept is only a hit whose whole text
	///     is a name that was asked for.
	/// </summary>
	private static Dictionary<ulong, string> NameTexts(BedrockProcess process, IEnumerable<string> names)
	{
		var wanted = new HashSet<string>(names, StringComparer.Ordinal);
		int longest = wanted.Count == 0 ? 0 : wanted.Max(n => n.Length);
		byte[] prefix = Encoding.ASCII.GetBytes("minecraft:");
		var texts = new Dictionary<ulong, string>();
		var buffer = new byte[1 << 20];

		foreach (BedrockProcess.Region region in process.Regions)
		{
			for (ulong at = region.Base; at < region.End;)
			{
				int want = (int) Math.Min((ulong) buffer.Length, region.End - at);
				int read = process.ReadClipped(at, buffer, want);
				if (read <= 0) { at += 4096; continue; }

				var bytes = new ReadOnlySpan<byte>(buffer, 0, read);
				for (var i = 0; i + prefix.Length <= read;)
				{
					int hit = bytes.Slice(i, read - i).IndexOf(prefix);
					if (hit < 0) break;
					int start = i + hit;
					i = start + 1;

					int length = 0;
					while (start + length < read && length <= longest && buffer[start + length] != 0) length++;
					if (length == 0 || length > longest) continue;
					string text = Encoding.ASCII.GetString(buffer, start, length);
					if (wanted.Contains(text)) texts[at + (ulong) start] = text;
				}

				at += (ulong) read;
			}
		}

		return texts;
	}

	/// <summary>
	///     Every node whose key is one of those texts and whose value states that block's own id. The
	///     key is a std::string, so its own length has to be the text's length; the value is the
	///     definition, and the id it states has to be the one the BlockType of that name states.
	/// </summary>
	private static List<ulong> Nodes(BedrockProcess process, IReadOnlyDictionary<ulong, string> texts,
		IReadOnlyDictionary<string, int> ids, int size, int idAtInDefinition)
	{
		var found = new List<ulong>();
		var seen = new HashSet<ulong>();
		var buffer = new byte[1 << 20];
		var window = new byte[size];
		var word = new byte[8];

		foreach (BedrockProcess.Region region in process.Regions)
		{
			for (ulong at = region.Base; at < region.End;)
			{
				int want = (int) Math.Min((ulong) buffer.Length, region.End - at);
				int read = process.ReadClipped(at, buffer, want);
				if (read <= 0) { at += 4096; continue; }

				for (var i = 0; i + 32 <= read; i += 8)
				{
					ulong pointer = BitConverter.ToUInt64(buffer, i);
					if (!texts.TryGetValue(pointer, out string name)) continue;
					ulong length = BitConverter.ToUInt64(buffer, i + 16);
					ulong capacity = BitConverter.ToUInt64(buffer, i + 24);
					if (length != (ulong) name.Length || capacity < length) continue;

					ulong node = at + (ulong) i - KeyInNode;
					ulong address = process.ReadUInt64(node + ValueInNode, word);
					if (address < 0x10000 || !process.IsMapped(address)) continue;
					if (process.ReadClipped(address, window, size) != size) continue;
					if (!ids.TryGetValue(name, out int id) || BitConverter.ToInt32(window, idAtInDefinition) != id) continue;
					if (seen.Add(node)) found.Add(node);
				}

				at += (ulong) read;
			}
		}

		return found;
	}
}
