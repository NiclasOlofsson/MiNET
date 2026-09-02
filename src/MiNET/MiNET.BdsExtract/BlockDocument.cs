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

namespace MiNET.BdsExtract;

using System.Globalization;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;

/// <summary>
///     The extraction as two files, because the server has two things: blocks, and their states.
///     Everything a block is goes on the block. Its identity, its physical values, its tags, the
///     class implementing it, the properties it has with the values each can take, and what its old
///     data values mean. Splitting those across a file per question made a reader join four files
///     to answer one, and produced files like a list of properties with a count of how many blocks
///     carry each, which answers nothing anybody asked.
///     Everything a state is goes on the state: its runtime id, which block it belongs to, what it
///     is, and the light it gives off and takes away.
///     Both are built as the model and handed to the serializer. Nothing here writes JSON text: a
///     file that is printed rather than serialized can say something the model does not, and every
///     way of getting that wrong (a key written twice, a value rendered one way and compared
///     another, a shape that drifts from the reference) was reachable while it did.
/// </summary>
public static class BlockDocument
{
	/// <summary>How every file this tool writes is written: the model, indented with tabs.</summary>
	private static readonly JsonSerializerOptions Format = new()
	{
		WriteIndented = true,
		IndentCharacter = '\t',
		IndentSize = 1,
		Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
	};

	/// <summary>The model as the file it becomes.</summary>
	public static string Serialize(JsonNode document)
	{
		return document.ToJsonString(Format) + "\n";
	}

	/// <summary>
	///     The header every output leads with: the block state schema version and, spelled out
	///     beside it, the release it encodes (the number is four bytes, major.minor.patch.revision).
	///     Every state carries the same stamp (frozen at 1.21.60.33 for years); if a build ever
	///     splits it, the split is written as the list it is instead of one value hiding the other.
	/// </summary>
	public static void Version(JsonObject document, IReadOnlyList<PaletteEntry> palette)
	{
		int[] versions = palette.Select(p => p.Version).Distinct().OrderBy(v => v).ToArray();
		static string Release(int v) => $"{v >> 24 & 0xff}.{v >> 16 & 0xff}.{v >> 8 & 0xff}.{v & 0xff}";

		if (versions.Length == 1)
		{
			document["blockStateVersion"] = versions[0];
			document["blockStateRelease"] = Release(versions[0]);
			return;
		}

		document["blockStateVersions"] = new JsonArray(versions.Select(v => (JsonNode) v).ToArray());
		document["blockStateReleases"] = new JsonArray(versions.Select(v => (JsonNode) Release(v)).ToArray());
	}

	/// <summary>One row per block, with everything the extraction knows about it.</summary>
	public static JsonObject Blocks(BedrockProcess process, IReadOnlyList<PaletteEntry> palette,
		IReadOnlyList<BlockProperties> blocks,
		IReadOnlyDictionary<ulong, string> classNames,
		IReadOnlyDictionary<string, BlockStateRange> ranges,
		IReadOnlyDictionary<string, int> propertyIds,
		IReadOnlyDictionary<string, LegacyStateTable> legacy,
		IReadOnlyDictionary<string, List<BlockComponent>> components,
		IReadOnlyDictionary<string, BlockDefinitions.Definition> definitions)
	{
		var window = new byte[BlockLayout.Reach];
		var scratch = new byte[256];

		// What every address a block holds actually is. A pointer to something this extraction
		// already knows is written as that thing rather than as an address, because an address is
		// true for one run of one server and names nothing a reader can look up.
		// The value, not a node: a node belongs to one parent, and 534 blocks share one material.
		var known = new Dictionary<ulong, object>();
		var materials = new Dictionary<ulong, byte>();

		// Its states, by the index the state file publishes them under.
		foreach (PaletteEntry state in palette) known.TryAdd(state.Address, state.Index);

		// Another block, by its name. The derived class a block is keeps one: a stair holds the
		// block it is a stair of, which is the base_block the wire's block archetype carries and
		// which no member of BlockType states.
		foreach (BlockProperties held in blocks) known.TryAdd(held.Address, held.Name);

		// Its class. The method table keeps its address, because that is what was read, and carries
		// the name the class table gives it where there is one.
		var vtables = new Dictionary<ulong, string>(classNames);

		// Its material, by the type the entry states. The materials are a registry: 25 entries of
		// 16 bytes sharing one array, each leading with its own type, and those types run 0 to 24
		// with no repeats, which is what says the byte is the entry's identity rather than its
		// position. 1,356 blocks point into those 25.
		int materialAt = BlockLayout.At("material");
		var word = new byte[8];
		foreach (BlockProperties block in blocks)
		{
			ulong entry = process.ReadUInt64(block.Address + (ulong) materialAt, word);
			if (entry < 0x10000 || known.ContainsKey(entry)) continue;
			// Both halves: the value is what was read, the name is what it means. A name alone
			// throws the byte away, and a byte alone says nothing.
			if (process.TryRead(entry, word, 1)) materials[entry] = word[0];
		}

		BlockMemberReader.Identify identify = a =>
		{
			if (vtables.TryGetValue(a, out string named))
			{
				return BlockMemberReader.Pointer(new JsonObject { ["class"] = named }, a);
			}

			if (materials.TryGetValue(a, out byte type))
			{
				return new JsonObject
				{
					["value"] = (int) type,
					["name"] = type < Materials.Length ? Materials[type] : null
				};
			}

			return Scalar(known.GetValueOrDefault(a));
		};

		var document = new JsonObject();
		Version(document, palette);
		document["layoutPublishedFor"] = BlockMembers.Build.ToString();
		Classes(document, BlockMembers.Root(BlockMembers.Source.Blocks), BlockMembers.Source.Blocks);
		document["count"] = blocks.Count;

		var rows = new JsonArray();
		foreach (BlockProperties block in blocks)
		{
			// Geometry comes from the component the id names, because the field read picked the
			// wrong component on 5 blocks: the visual component also carries a namespaced string
			// and sorts ahead of the geometry one. That is correcting a misread, not choosing
			// between two readings.
			// Map colour is NOT substituted the same way. The field genuinely reads zero on the 95
			// blocks that carry the component, and both are already in the output, so replacing
			// one with the other would be this file deciding which is true rather than saying what
			// each holds.
			var carried = components.GetValueOrDefault(block.Name) ?? [];
			string geometry = carried.FirstOrDefault(c => c.Name == "BlockGeometryComponent")?.Holds?
				["geometryName"]?.GetValue<string>() ?? block.Geometry;

			// The row is the object: every member of the class, in the class's own order, and
			// nothing put in front of them. What this tool works out about the block follows after,
			// so a reader can tell what was read from what was derived by where it sits.
			var row = new JsonObject();

			// Every member of the class, under the class's own name, shaped the way the class is:
			// a member holding another object is an object. Not a selection: a member nothing read
			// is a member holding null, so what nothing reads is counted rather than absent.
			// The tags member is the vector this tool reads, so its own member carries them rather
			// than sitting null beside a second list under the same name.
			JsonObject members = BlockMemberReader.Read(process, block.Address, window, scratch, identify);
			members["tags"] = new JsonArray(block.Tags.Select(t => (JsonNode) t).ToArray());

			// The components the block holds, in the member that holds them. The id is not published
			// on its own: it is a per-instance registration number and two runs of one build disagree
			// on every one, so a component this run could name travels under that name and one it
			// could not travels as its bytes, which is a value read and kept rather than a number
			// that means nothing tomorrow.
			members["components"] = new JsonArray(carried.Select(c => (JsonNode) Carried(c)).ToArray());

			// The properties the block declares, from the map the member holds. Not the same fact as
			// the stateProperties below it: this is what the block says it has, and that is what its
			// states turned out to be, and on some blocks the declaration is the wider of the two.
			members["states"] = BlockStateDefinitions.Read(process, block.Address + (ulong) BlockLayout.At("states"));
			members["stateNameMap"] = BlockStateDefinitions.Names(process,
				block.Address + (ulong) BlockLayout.At("stateNameMap"));

			Fold(members, row);

			row["name"] = block.Name;
			row["geometry"] = geometry;

			// The definition the server still keeps for this block, where it keeps one. A block the
			// map does not hold has no definition, which is a value: 1,379 of these are compiled
			// blocks the data driven path never built, and the wire sends block properties for none
			// of them.
			row["definition"] = definitions.GetValueOrDefault(block.Name)?.Read;

			// What of the object these values represent. The size is the allocator's, not the
			// furthest field read, and the holes are the bytes inside the object that no member
			// above accounts for. They are stated rather than dropped so the gap is countable: a
			// row listing twenty values out of an object three times that size reads as complete
			// unless it says otherwise. A null hole list means the size could not be measured.
			row["objectSize"] = block.ObjectSize > 0 ? block.ObjectSize : null;
			row["unread"] = block.Unread is null
				? null
				: new JsonArray(block.Unread
					.Select(hole => (JsonNode) Hole(process, block.Address, hole, identify))
					.ToArray());

			// The carried components are not published. Their ids are per-instance registration
			// numbers, so two runs of the same build disagree on every block, and rows that change
			// with the heap are noise dressed as data. They are still read, because the geometry
			// above is resolved through them; they are just not part of this file.

			// The properties and their values, which is what a state of this block can be.
			BlockStateRange range = ranges.GetValueOrDefault(block.Name);
			row["stateCount"] = range?.States ?? 0;

			var properties = new JsonObject();
			foreach (var property in range?.Properties ?? [])
			{
				var stated = new JsonObject();
				if (propertyIds.TryGetValue(property.Name, out int id)) stated["id"] = id;
				stated["type"] = property.Type.ToString();
				stated["values"] = new JsonArray(property.Values.Select(Scalar).ToArray());
				properties[property.Name] = stated;
			}

			row["stateProperties"] = properties;

			// What each pre-flattening data value means, as network ids into the state file. Null
			// is a data value the server has no state for.
			LegacyStateTable table = legacy.GetValueOrDefault(block.Name);
			row["byData"] = new JsonArray((table?.ByData ?? [])
				.Select(v => v is null ? null : (JsonNode) v.Value).ToArray());

			rows.Add(row);
		}

		document["blocks"] = rows;
		return document;
	}

	/// <summary>One row per state, in the server's own order, which is the runtime id order.</summary>
	public static JsonObject States(BedrockProcess process, IReadOnlyList<PaletteEntry> palette,
		IReadOnlyList<BlockProperties> blocks, ExtractionReport report)
	{
		var stateWindow = new byte[BlockLayout.StateReach];
		var scratch = new byte[256];
		var word = new byte[8];

		// A state points back at the block it belongs to, which the block file publishes under its
		// name, so that is what the pointer is written as.
		var known = new Dictionary<ulong, object>();
		foreach (BlockProperties block in blocks) known.TryAdd(block.Address, block.Name);

		// Every state is one class, so its method table is one address. Named the same way a block
		// names its own, from the class the reference states rather than from anything invented.
		var word2 = new byte[8];
		string root = BlockMembers.Root(BlockMembers.Source.States).Name;
		ulong stateTable = palette.Count > 0 ? process.ReadUInt64(palette[0].Address, word2) : 0;

		BlockMemberReader.Identify owner = a => a == stateTable && stateTable != 0
			? BlockMemberReader.Pointer(new JsonObject { ["class"] = root }, a)
			: Scalar(known.GetValueOrDefault(a));

		// Say the id scheme in the file. Without it a reader cannot tell whether networkId is a
		// hash or a repeat of the index, and both look equally reasonable.
		var document = new JsonObject { ["networkIdsAreHashes"] = report.NetworkIdsAreHashes };
		Version(document, palette);

		// The states file states its own class table, the same way the block file does, so its own
		// output can be the next reference without a layout living anywhere else.
		document["layoutPublishedFor"] = BlockMembers.Build.ToString();
		Classes(document, BlockMembers.Root(BlockMembers.Source.States), BlockMembers.Source.States);
		document["count"] = palette.Count;

		int tagsAt = BlockLayout.States.Roots.First(n => n.Member.Name == "tags").At;
		int componentsAt = BlockLayout.States.Roots.First(n => n.Member.Name == "components").At;

		var rows = new JsonArray();
		foreach (PaletteEntry entry in palette)
		{
			var row = new JsonObject();

			// Every member of the state class, under the class's own name, read from where this
			// build keeps it. networkId is one of them, so it is not written twice, and neither is
			// the light: the palette's own reading of it locates the field and the class member is
			// what the file states.
			// A state holds its own components and its own tags, in the same two classes a block
			// holds them in, so the same two readers answer for them. The members carrying them
			// take the value rather than sitting null beside a second list under another name.
			JsonObject members = BlockMemberReader.ReadState(process, entry.Address, stateWindow, scratch, owner);

			ulong tags = entry.Address + (ulong) tagsAt;
			members["tags"] = new JsonArray(BlockRegistry
				.Tags(process, process.ReadUInt64(tags, word), process.ReadUInt64(tags + 8, word))
				.Select(t => (JsonNode) t).ToArray());

			// Under the names the block side worked out for the ids, never the ids: a registration
			// number is per instance and two runs of one build disagree on every one. A component
			// nothing named is still counted, as the number it is, so the hole stays visible.
			members["components"] = new JsonArray(ComponentReader
				.Carried(process, entry.Address + (ulong) componentsAt, word)
				.Select(c => (JsonNode) Carried(c)).ToArray());

			// The compound the network id is a hash of: a name, its state properties and a version,
			// which is the three entries the tag's own count states. It is what this row's name,
			// version and states were read from, so the member that holds it carries it.
			var serialized = new JsonObject();
			foreach (StateProperty property in entry.States) serialized[property.Name] = property.ToNode();
			members["serializationId"] = new JsonObject
			{
				["name"] = entry.Name,
				["states"] = serialized,
				["version"] = entry.Version
			};

			Fold(members, row);

			row["index"] = entry.Index;
			row["name"] = entry.Name;
			row["version"] = entry.Version;

			var states = new JsonObject();
			foreach (StateProperty property in entry.States) states[property.Name] = property.ToNode();
			row["states"] = states;

			rows.Add(row);
		}

		document["states"] = rows;
		return document;
	}

	/// <summary>
	///     Which class the file's objects are, and every class reachable from it: each one's size and
	///     its members, with each member's offset inside the class that declares it. This is the
	///     layout the run read with, written back out, so this file is the next run's reference.
	/// </summary>
	internal static void Classes(JsonObject document, ClassLayout root, BlockMembers.Source source)
	{
		// Every class the reference declares, in the order it declares them. Not a walk from the
		// root: what a class table has to contain is what the run read with, and a class is read
		// with whether or not a member points at it. Five item components are decoded from the
		// name the server registers them under rather than through a member, so a walk reached
		// none of them and a run's own output could not become the next reference. Reachability
		// also decided the order, which is why AABB and BlockAABBComponentData swapped places
		// against a reference nothing had touched.
		IReadOnlyList<ClassLayout> written = BlockMembers.All(source);

		var classes = new JsonObject();
		foreach (ClassLayout held in written)
		{
			var members = new JsonArray();
			foreach (BlockMember member in held.Members)
			{
				var stated = new JsonObject
				{
					["name"] = member.Name,
					["at"] = member.At,
					["bytes"] = member.Bytes,
					["kind"] = member.Kind.ToString()
				};
				// Everything the member states, or the file cannot be the next reference. Dropping
				// "bit" gave nine flags all reading bit zero; dropping "enum" turned every trait
				// name back into a number. Both got past the check, because the check compares the
				// file to itself and both sides were written by the same lossy writer.
				if (member.Kind == MemberKind.Bit) stated["bit"] = member.Bit;
				if (!member.Comparable) stated["comparable"] = false;
				if (!member.Emit) stated["emit"] = false;
				if (member.Holds is not null) stated["holds"] = member.Holds;
				if (member.Enum is not null) stated["enum"] = member.Enum;
				if (member.Elements is not null) stated["elements"] = member.Elements;
				if (member.Entries is not null) stated["entries"] = member.Entries;
				if (member.Gate != 0) stated["gate"] = member.Gate;
				members.Add(stated);
			}

			var stated2 = new JsonObject { ["size"] = held.Size };
			// Where the class's own first member sits inside the object. Only stated where it is
			// not nought, so a class that is the whole object says nothing extra, and a component
			// class carries the sixteen or twenty four bytes of base it does not declare.
			if (held.Base != 0) stated2["base"] = held.Base;
			// How a class no id names is picked out of the build. Dropping either would leave the
			// next reference unable to tell one definition component description from another.
			if (held.Installs is not null) stated2["installs"] = held.Installs;
			if (held.States is not null)
			{
				stated2["states"] = new JsonArray(held.States.Select(t => (JsonNode) t).ToArray());
			}

			stated2["members"] = members;
			classes[held.Name] = stated2;
		}

		document["class"] = root.Name;
		document["classes"] = classes;

		// Everything else the reference states, so this file can become the next one. A refreshed
		// reference that dropped these came back with every enum unnamed and no component classes.
		var named = new JsonObject();
		foreach ((string name, Dictionary<long, string> values) in BlockMembers.Enums)
		{
			var entries = new JsonObject();
			foreach ((long value, string text) in values) entries[value.ToString(CultureInfo.InvariantCulture)] = text;
			named[name] = entries;
		}

		if (named.Count > 0) document["enums"] = named;

		var components = new JsonObject();
		foreach ((string component, string held) in BlockMembers.ComponentClasses)
		{
			if (classes.ContainsKey(held)) components[component] = held;
		}

		if (components.Count > 0) document["componentClasses"] = components;
	}

	/// <summary>
	///     One stretch of a block object that no member of BlockType covers, as its bytes and as
	///     whatever this extraction can already say about them.
	///     <para>
	///         All of it is the derived class's own tail: BlockType is 888 bytes and every hole
	///         starts there. No layout exists for any of those tails, so the bytes go out as bytes;
	///         what is added is the same thing every other pointer in this file gets, which is that
	///         an address this extraction already publishes travels as the thing it names. That is
	///         where a stair states its base block: the first word of StairBlock's tail points at
	///         the BlockType of the block it is a stair of, which is the base_block of the wire's
	///         block archetype and is stated nowhere else in the object.
	///     </para>
	/// </summary>
	private static JsonObject Hole(BedrockProcess process, ulong address, ByteRange hole,
		BlockMemberReader.Identify identify)
	{
		var stated = new JsonObject { ["at"] = hole.At, ["bytes"] = hole.Bytes };

		var body = new byte[hole.Bytes];
		if (!process.TryRead(address + (ulong) hole.At, body, body.Length)) return stated;
		stated["raw"] = Convert.ToHexString(body);

		// Word by word, in place, so a reader can tell which eight bytes named the thing. A word
		// that names nothing is null; every hole is a multiple of eight on every block here, and
		// one that is not simply has no words to state.
		var words = new JsonArray();
		bool any = false;
		for (int at = 0; at + 8 <= body.Length; at += 8)
		{
			JsonNode named = identify(BitConverter.ToUInt64(body, at));
			any |= named is not null;
			words.Add(named);
		}

		if (any) stated["words"] = words;
		return stated;
	}

	/// <summary>
	///     Moves one object's members into another, keeping the order they were read in. A node
	///     belongs to one parent, so the members are detached as they go rather than copied: a copy
	///     would be a second reading of the same thing, and only one of two can be right.
	/// </summary>
	private static void Fold(JsonObject from, JsonObject into)
	{
		foreach (string name in from.Select(member => member.Key).ToArray())
		{
			JsonNode value = from[name];
			from.Remove(name);
			into[name] = value;
		}
	}

	/// <summary>
	///     One component a block carries: everything read about it. The id is the server's own
	///     registration number, which differs between runs of one build, so it is a fact about this
	///     heap rather than about the game; it goes out anyway, because deciding which of the things
	///     that were read a reader is allowed to see is not this tool's call.
	/// </summary>
	private static JsonObject Carried(BlockComponent component)
	{
		var stated = new JsonObject
		{
			["id"] = component.Id,
			["name"] = component.Name
		};

		// A component with no data says so, rather than reading as one whose object failed to read.
		if (component.Stateless)
		{
			stated["stateless"] = true;
			return stated;
		}

		Fold(component.Holds, stated);
		return stated;
	}

	/// <summary>
	///     What each material is, by the name the class gives it. From the generated MaterialType,
	///     whose values run 0 to 24 with Any and Size as sentinels past them, which is exactly the
	///     25 entries the registry holds. A type this table has no name for travels as its number.
	/// </summary>
	private static readonly string[] Materials =
	[
		"Air", "Dirt", "Wood", "Metal", "Grate", "Water", "Lava", "Leaves", "Plant", "SolidPlant",
		"Fire", "Glass", "Explosive", "Ice", "PowderSnow", "Cactus", "Portal", "StoneDecoration",
		"Bubble", "Barrier", "DecorationSolid", "ClientRequestPlaceholder", "StructureVoid", "Solid",
		"NonSolid"
	];

	/// <summary>A value the server holds as one of a few kinds, as the kind it is.</summary>
	internal static JsonNode Scalar(object value)
	{
		return value switch
		{
			bool b => JsonValue.Create(b),
			byte b => JsonValue.Create((long) b),
			int i => JsonValue.Create((long) i),
			long l => JsonValue.Create(l),
			string s => JsonValue.Create(s),
			_ => null
		};
	}
}
