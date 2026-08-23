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

using System.Globalization;
using System.Text.Json.Nodes;

namespace MiNET.BdsExtract;

/// <summary>One component a block carries, with its value where the type is known.</summary>
public sealed class BlockComponent
{
	/// <summary>
	///     The server's own number for this component, from the array beside the pointer vector.
	///     This is the identity. Everything else here is an attempt to put a name to it, and the
	///     number is correct whether or not the name is known.
	/// </summary>
	public int Id { get; init; }

	/// <summary>The identifier, or null when the type could not be named.</summary>
	public string Name { get; init; }

	/// <summary>
	///     A component carrying no data at all, recorded as a bare id in the set beside the map
	///     rather than as an object. The storage keeps the two apart, mComponents and
	///     mStatelessComponents, so a block holding one is saying it has the component and that the
	///     component has nothing to say.
	/// </summary>
	public bool Stateless { get; init; }

	/// <summary>
	///     What the component holds: its decoded fields under their own names, and the bytes nothing
	///     decoded as unknown members in their place.
	/// </summary>
	public JsonObject Holds { get; init; }

	/// <summary>The object's measured size, which is what makes an unnamed component countable.</summary>
	public int Size { get; init; }

}

/// <summary>
///     The components a block actually carries, read from the vector on BlockLegacy.
///     The schema says a block may have forty nine components. Vanilla instantiates thirteen types,
///     so most of that list is modding surface no block here holds, and the census is what tells one
///     from the other.
///     Types are found the same way block classes are, by grouping instances on their method table,
///     and then named by what the group holds rather than by its address. An address moves every
///     build; a component carrying a namespaced geometry name, or a loot table path, or four floats
///     that read as a colour, carries that whatever it is compiled to.
///     A group that cannot be named is still emitted, with its size and the blocks holding it,
///     because a component dropped for being unrecognised makes the block look like it has fewer
///     than it does.
/// </summary>
public static class ComponentReader
{
	private const int VectorReach = 4096;
	private const int MaximumSize = 512;
	private const string Namespace = "minecraft:";
	private const string LootPrefix = "loot_tables/";
	private const string LootDefault = "normal";

	/// <summary>A model reference, which the visual component uses and geometry does not.</summary>
	private const string ModelPrefix = "minecraft:geometry.";

	/// <summary>Where a component keeps its first value, past the method table.</summary>
	private const int FirstField = 8;

	/// <summary>Geometry states its name one field further in than every other string component.</summary>
	private const int GeometryText = 16;

	/// <summary>
	///     What each component id is, worked out from every instance carrying it. A registration
	///     number is per instance and two runs of one build disagree on every one of them, so the
	///     number is never published: the name it stands for is.
	/// </summary>
	public static IReadOnlyDictionary<int, string> Names { get; private set; } = new Dictionary<int, string>();

	/// <summary>
	///     The components one object holds, read the same way whether it is a block or a state.
	///     A state's are the same kinds under the same ids, so reading them as names beside the
	///     block's decoded values would leave the state saying less about the same thing.
	/// </summary>
	public static List<BlockComponent> Carried(BedrockProcess process, ulong storage, byte[] word)
	{
		var list = new List<BlockComponent>();
		foreach ((int id, ulong at) in Instances(process, storage, word))
		{
			string name = Names.GetValueOrDefault(id);
			int size = Size(process, at, word);
			list.Add(new BlockComponent
			{
				Id = id,
				Name = name,
				Size = size,
				Holds = Decode(process, name, at, size, word)
			});
		}

		foreach (int id in Stateless(process, storage, word))
		{
			list.Add(new BlockComponent
			{
				Id = id,
				Name = Names.GetValueOrDefault(id) ?? KnownIds.GetValueOrDefault(id),
				Stateless = true
			});
		}

		return list;
	}

	public static Dictionary<string, List<BlockComponent>> Read(BedrockProcess process,
		IReadOnlyList<BlockProperties> blocks,
		IReadOnlyList<PaletteEntry> palette)
	{
		var word = new byte[8];
		var facts = Facts(process, palette);
		var byBlock = new Dictionary<string, List<(int Id, ulong At)>>(StringComparer.Ordinal);
		var byId = new Dictionary<int, List<(string Block, ulong At)>>();

		foreach (var block in blocks)
		{
			var held = Instances(process, block.Address + (ulong) BlockLayout.At("components"), word);
			byBlock[block.Name] = held;
			foreach (var (id, at) in held)
			{
				if (!byId.TryGetValue(id, out var list)) byId[id] = list = [];
				list.Add((block.Name, at));
			}
		}

		// One name per id, decided from every instance carrying it. Grouping on the method table
		// instead put two components in one bucket, because one class serves several of them: the
		// id separates what the class does not.
		var named = new Dictionary<int, string>();
		var sized = new Dictionary<int, int>();
		foreach (var (id, instances) in byId)
		{
			sized[id] = Size(process, instances[0].At, word);
			named[id] = Identify(process, instances, facts, word);
		}

		ApplyKnownIds(named);

		// The same ids answer on a state, because both come out of one registry, so the names
		// worked out here are what lets a state say what it carries rather than a bare number.
		Names = named;

		var result = new Dictionary<string, List<BlockComponent>>(StringComparer.Ordinal);
		foreach (var block in blocks)
		{
			var list = new List<BlockComponent>();
			foreach (var (id, at) in byBlock[block.Name])
			{
				string name = named.GetValueOrDefault(id);
				int size = sized.GetValueOrDefault(id);
				list.Add(new BlockComponent
				{
					Id = id,
					Name = name,
					Size = size,
					Holds = Decode(process, name, at, size, word)
				});
			}
			// The ones with no data are components too, so they go in the same list saying what they
			// are: an id, its name where the run knows it, and nothing else, because there is
			// nothing else in the server either.
			foreach (int id in Stateless(process, block.Address + (ulong) BlockLayout.At("components"), word))
			{
				// A dataless component is never in the map, so the naming above never sees its id and
				// the table is the only thing that can name it.
				string bare = named.GetValueOrDefault(id) ?? KnownIds.GetValueOrDefault(id);
				list.Add(new BlockComponent { Id = id, Name = bare, Stateless = true });
			}

			result[block.Name] = list;
		}
		return result;
	}

	/// <summary>
	///     The components on one block, each with the id the server files it under.
	///     Two vectors run in step: pointers at <see cref="MemoryLayout.BlockComponents" /> and one
	///     sixteen bit id each at <see cref="MemoryLayout.BlockComponentIds" />. They are required
	///     to be the same length, which is what says they are the same list, and a block where they
	///     disagree is skipped rather than paired up by position and hoped for.
	/// </summary>
	/// <summary>
	///     The ids of the components a block holds that carry no data. They sit in a flat_set of
	///     their own beside the map of the ones that do, and reading only the map drops them: 45
	///     blocks hold 50 of them, and a block saying it is replaceable by holding a bare id is
	///     saying it just as much as one holding an object.
	/// </summary>
	internal static List<int> Stateless(BedrockProcess process, ulong storage, byte[] word)
	{
		var ids = new List<int>();
		ulong begin = process.ReadUInt64(storage + StatelessSet, word);
		ulong end = process.ReadUInt64(storage + StatelessSet + 8, word);
		if (begin < 0x10000 || end <= begin || end - begin > VectorReach || (end - begin) % 2 != 0) return ids;

		var raw = new byte[end - begin];
		if (!process.TryRead(begin, raw, raw.Length)) return ids;
		for (int i = 0; i + 2 <= raw.Length; i += 2) ids.Add(BitConverter.ToInt16(raw, i));
		return ids;
	}

	/// <summary>Where the set of dataless component ids sits inside the storage.</summary>
	private const int StatelessSet = 48;

	internal static List<(int Id, ulong At)> Instances(BedrockProcess process, ulong storage, byte[] word)
	{
		var held = new List<(int, ulong)>();

		// The keys are first and the values second, which is what the flat_map declares. The ids
		// are two bytes each and the pointers eight, and requiring both counts to agree is what
		// says the two vectors are the same list.
		ulong idBegin = process.ReadUInt64(storage, word);
		ulong idEnd = process.ReadUInt64(storage + 8, word);
		ulong begin = process.ReadUInt64(storage + 24, word);
		ulong end = process.ReadUInt64(storage + 32, word);
		if (begin < 0x10000 || end <= begin || (end - begin) % 8 != 0) return held;
		if (end - begin > VectorReach || !process.IsMapped(begin)) return held;

		int count = (int) ((end - begin) / 8);
		if (idBegin < 0x10000 || idEnd - idBegin != (ulong) (count * 2)) return held;
		if (!process.IsMapped(idBegin)) return held;

		var ids = new byte[count * 2];
		if (!process.TryRead(idBegin, ids, ids.Length)) return held;

		for (int i = 0; i < count; i++)
		{
			ulong component = process.ReadUInt64(begin + (ulong) (i * 8), word);
			if (component < 0x10000 || !process.IsMapped(component)) continue;
			held.Add((BitConverter.ToUInt16(ids, i * 2), component));
		}
		return held;
	}

	/// <summary>
	///     What a group of instances is, from what they hold. Each test is something the other
	///     twelve types fail, and a test that only most of a group passes names nothing: a
	///     component type holds one kind of value in every instance or it is not one type.
	/// </summary>
	/// <summary>
	///     What a block's own state says about the values four of its components carry. The state
	///     holds these compiled out into fields, and the component holds the same numbers, so the
	///     state is the left hand side that names the component.
	/// </summary>
	private readonly record struct StateFacts(float Hardness, float Resistance, int FlameOdds,
		int BurnOdds, bool CanContainLiquid, int OnLiquidTouches);

	/// <summary>
	///     One reading per block, taken from the first state the palette lists for it. Hardness,
	///     blast resistance and the two odds are properties of the block rather than of one state,
	///     so any state of the block states them; a state whose values cannot be read is left out
	///     rather than entered as zero, because a zero that was never read would match a component
	///     holding zero and name it wrongly.
	/// </summary>
	private static Dictionary<string, StateFacts> Facts(BedrockProcess process,
		IReadOnlyList<PaletteEntry> palette)
	{
		int destroySpeed = BlockLayout.StateAt("directData.destroySpeed");
		int resistance = BlockLayout.StateAt("directData.explosionResistance");
		int flameOdds = BlockLayout.StateAt("directData.flameOdds");
		int liquid = BlockLayout.StateAt("directData.waterDetectionRule.canContainLiquid");
		int touches = BlockLayout.StateAt("directData.waterDetectionRule.onLiquidTouches");

		var facts = new Dictionary<string, StateFacts>(StringComparer.Ordinal);
		var word = new byte[8];
		foreach (PaletteEntry state in palette)
		{
			if (state.Name is null || facts.ContainsKey(state.Name)) continue;

			float[] speed = Floats(process, state.Address + (ulong) destroySpeed, 1, word);
			float[] blast = Floats(process, state.Address + (ulong) resistance, 1, word);
			int[] odds = Shorts(process, state.Address + (ulong) flameOdds, 2, word);
			byte[] holds = Bytes(process, state.Address + (ulong) liquid, 1, word);
			byte[] reaction = Bytes(process, state.Address + (ulong) touches, 1, word);
			if (speed is null || blast is null || odds is null || holds is null || reaction is null) continue;

			facts[state.Name] = new StateFacts(speed[0], blast[0], odds[0], odds[1],
				holds[0] != 0, reaction[0]);
		}

		return facts;
	}

	private static string Identify(BedrockProcess process,
		List<(string Block, ulong At)> instances,
		IReadOnlyDictionary<string, StateFacts> facts,
		byte[] word)
	{
		var sample = instances.Take(SampleSize).ToList();

		// A test on shape or on a matching number proves nothing about a handful of instances: a
		// single float is symmetric about zero, or equal to that one block's hardness, far too
		// easily. Running a pack of one-block-per-component through this named five different ids
		// "random_offset" at once. Only tests that read a distinctive STRING are allowed below the
		// bar, because a component holding "loot_tables/blocks/x.json" is not a coincidence at any
		// size.
		bool weighty = instances.Count >= MinimumEvidence;

		// Geometry names itself. The visual component also carries a namespaced string here, so
		// the model references it uses are excluded rather than folded in: minecraft:unit_cube is
		// a shape, minecraft:geometry.cross is a model, and they are not the same component.
		if (sample.All(i => StdString(process, i.At + GeometryText, word) is { } shape
							&& shape.StartsWith(Namespace, StringComparison.Ordinal)
							&& !shape.StartsWith(ModelPrefix, StringComparison.Ordinal)))
		{
			return "minecraft:geometry";
		}

		// Loot names a table in the packs. The id holding the literal "normal" on every block is a
		// different component and is deliberately not matched here: one string test covering both
		// put 1,429 instances under the wrong name.
		if (sample.All(i => StdString(process, i.At + FirstField, word)?.StartsWith(LootPrefix,
				StringComparison.Ordinal) == true))
		{
			return "minecraft:loot";
		}

		// Break particles, whose texture is optional: most blocks take their own, and the ones that
		// override name a texture outright. lever_particle and straw_bed_particle say what the
		// field is; grass_bottom and big_oak_leaves say it is the break texture rather than the
		// block's. The rule is that no instance holds anything but a plain texture name, and at
		// least one holds one, which a component with no string at all cannot pass.
		// The texture is OPTIONAL, and that is the test. A component where every instance carries
		// the same literal is a different component that merely also holds a plain word, which is
		// how the id holding "normal" everywhere ended up under this name once already.
		var textures = sample
			.Select(i => StdString(process, i.At + FirstField, word))
			.ToList();
		if (weighty && textures.Any(t => t is not null) && textures.Any(t => t is null)
			&& textures.All(t => t is null || (t.Length > 2 && !t.Contains(':') && !t.Contains('/'))))
		{
			return "minecraft:destruction_particles";
		}

		// Four floats reading as a colour, the fourth being opaque alpha.
		if (weighty && sample.All(i => Floats(process, i.At + FirstField, 4, word) is { } rgba
							&& rgba.All(v => v is >= 0f and <= 1f) && Math.Abs(rgba[3] - 1f) < 1e-6))
		{
			return "minecraft:map_color";
		}

		// A symmetric range about zero, which is what an offset jitter is.
		if (weighty && sample.All(i => Floats(process, i.At + FirstField, 2, word) is { } range
							&& range[0] < 0f && Math.Abs(range[0] + range[1]) < 1e-6))
		{
			return "minecraft:random_offset";
		}

		// One float that is this block's hardness, on every block in the group. Hardness takes
		// dozens of values across the registry, so agreeing everywhere is not a coincidence.
		if (weighty && Every(sample, facts, (i, state) =>
				Floats(process, i.At + FirstField, 1, word) is { } one
				&& Math.Abs(one[0] - state.Hardness) < 1e-4f))
		{
			return "minecraft:destructible_by_mining";
		}

		// Liquid detection, which is where both liquid answers come from: the component holds a
		// detection rule laid out exactly as the state's own, a flag first and the reaction two
		// bytes in. Every block that can hold a liquid source carries this component, so the
		// state's pair is a copy of what this says and both halves are checked against it.
		if (weighty && Every(sample, facts, (i, state) =>
				Bytes(process, i.At + FirstField, 3, word) is { } raw
				&& raw[0] <= 1 && (raw[0] != 0) == state.CanContainLiquid
				&& raw[2] == state.OnLiquidTouches))
		{
			return "minecraft:liquid_detection";
		}

		// Catching fire: two sixteen bit odds, matching the pair the state carries. The blocks that
		// hold this component with both odds at zero are a block saying outright that it does not
		// burn, and they agree too.
		if (weighty && Every(sample, facts, (i, state) =>
				Shorts(process, i.At + FlammableOdds, 2, word) is { } odds
				&& odds[0] == state.FlameOdds && odds[1] == state.BurnOdds))
		{
			return "minecraft:flammable";
		}

		// Blast resistance, stated five times over. Only instances on blocks with a non zero
		// resistance are asked, because at zero the test proves nothing; but the id already says
		// these are all one component, so agreeing on the ones that CAN be checked names the whole
		// group, zero-resistance blocks included.
		var checkable = sample
			.Where(i => facts.TryGetValue(i.Block, out StateFacts s) && s.Resistance > 0f)
			.ToList();
		if (weighty && checkable.Count > 0
			&& checkable.All(i => Floats(process, i.At + FirstField, 1, word) is { } one
								&& Math.Abs(one[0] - facts[i.Block].Resistance * ResistanceScale) < 1e-3f))
		{
			return "minecraft:destructible_by_explosion";
		}

		return null;
	}

	/// <summary>
	///     A test every sampled instance must pass against its own block's state. An instance whose
	///     block has no state reading fails it, because an unconfirmed instance is not a passing one.
	/// </summary>
	private static bool Every(List<(string Block, ulong At)> sample,
		IReadOnlyDictionary<string, StateFacts> facts,
		Func<(string Block, ulong At), StateFacts, bool> test) =>
		sample.All(i => facts.TryGetValue(i.Block, out StateFacts state) && test(i, state));

	/// <summary>
	///     Components named by experiment rather than by reading their contents, keyed by the id
	///     they had when the experiment ran.
	///     They were found by giving a behaviour pack one block per component and seeing which id
	///     appeared. That is the only way to name a component whose payload says nothing about what
	///     it is: a byte holding 1 is a byte holding 1 whether it means movable or replaceable.
	///     An id is NOT a stable name for a component. Registering a pack moved loot from 28 to 31
	///     on the very server this was measured against, so the table is applied only while the
	///     anchors below still sit where they did, and dropped whole if any has moved.
	/// </summary>
	private static readonly Dictionary<int, string> KnownIds = new()
	{
		[5] = "minecraft:precipitation_interactions",
		[6] = "minecraft:connection_rule",
		[7] = "minecraft:redstone_conductivity",
		[8] = "minecraft:movable",
		[13] = "minecraft:collision_box",
		[14] = "minecraft:selection_box",
		[24] = "minecraft:support",
		[26] = "minecraft:leashable",

		// Named from the class, not from a pack. The Linux build keeps Itanium RTTI, and every
		// ComponentInstance<T> typeinfo in it names its component class; these four were tied to an
		// id by the size their own destructor states plus the blocks that carry them. Ambient sound
		// is 80 bytes on the nine blocks that have one, item visual 24 on doors and trapdoors and
		// shelves, flower pottable on exactly the 38 blocks a pot takes, and redstone producer on
		// redstone_block alone, whose first byte is 15, full signal.
		// Named by identity, not by inference: on all four blocks that carry it, the
		// blockTransformationComponent slot in the state's direct data points at the same object
		// this id names in the list. The class is declared, so the entry decodes.
		[28] = "BlockTransformationComponent",

		[17] = "BlockAmbientSoundComponent",
		[18] = "BlockItemVisualComponent",
		[22] = "BlockFlowerPottableComponent",
		[27] = "BlockRedstoneProducerComponent",

		// No data at all, so they are in the set beside the map rather than in it, and the classes
		// they match declare no members either.
		[4] = "BlockReplaceableComponent",
		[16] = "BlockDesertAmbientSoundComponent"
	};

	/// <summary>
	///     Ids this run works out on its own, and where the table expects them. One disagreement
	///     means the enumeration has moved and the table describes an ordering that no longer
	///     exists, so none of it is applied.
	/// </summary>
	private static readonly Dictionary<int, string> Anchors = new()
	{
		[1] = "minecraft:destructible_by_explosion",
		[2] = "minecraft:destructible_by_mining",
		[3] = "minecraft:liquid_detection",
		[11] = "minecraft:destruction_particles",
		[12] = "minecraft:map_color",
		[15] = "minecraft:flammable",
		[20] = "minecraft:geometry"
	};

	private static void ApplyKnownIds(Dictionary<int, string> named)
	{
		foreach (var (id, expected) in Anchors)
		{
			if (named.GetValueOrDefault(id) != expected) return;
		}

		// Including ids the block census never saw. A component carried only by states, or only in
		// the dataless set, is not in that census at all, and requiring it to be there left
		// BlockTransformationComponent anonymous on every state that holds it.
		foreach (var (id, name) in KnownIds)
		{
			if (!named.TryGetValue(id, out string already) || already is null) named[id] = name;
		}
	}

	/// <summary>Blast resistance is stated five times over in the explosion component.</summary>
	private const float ResistanceScale = 5f;

	/// <summary>
	///     What one component holds, read as the class it is. The reference states that class the
	///     same way it states the block's, so a component is not decoded by a switch on its name
	///     here: the layout lives beside every other layout and a new build is a reference edit.
	///     The instance is a method table followed by the component, so the class sits eight bytes
	///     in and the table is stated separately. Bytes no member of the class covers become
	///     unknown members in their place, so the instance adds up to its measured size.
	/// </summary>
	private static JsonObject Decode(BedrockProcess process, string name, ulong at, int size, byte[] word)
	{
		// The method table is not emitted. It is how the size and the class are found, which makes
		// it a fact about this heap rather than about the block, and it is the same pointer on
		// twelve different components anyway.
		var fields = new JsonObject();
		var covered = new List<(int At, int Bytes)> { (0, FirstField) };

		ClassLayout held = BlockMembers.Component(name);
		if (held is null)
		{
			Leftovers(process, at, size, covered, fields, false);
			return fields;
		}

		int span = Math.Min(size > 0 ? size : MaximumSize, MaximumSize);
		var window = new byte[span];
		int read = process.ReadClipped(at, window, span);
		Members(process, at, window, read, held, FirstField, fields, covered);

		// The class states how big it is, so the tiling stops there. What the allocator handed out
		// past the end of the object is slack, and calling that unknown says the class has fields
		// nobody decoded when it does not: a component of a method table and one float is twelve
		// bytes whatever size block it was given.
		// Nothing else. The class declares the object, so every byte a member does not cover is one
		// the compiler inserted for alignment, and alignment is not data about the block.
		return fields;
	}

	/// <summary>
	///     One class's members at a position, and the members of anything they hold. Every one is
	///     recorded as covered whether or not its read succeeded, because the bytes belong to that
	///     member either way and counting them twice would hide a hole somewhere else.
	/// </summary>
	private static void Members(BedrockProcess process, ulong address, byte[] window, int read,
		ClassLayout held, int lead, JsonObject fields, List<(int At, int Bytes)> covered)
	{
		var scratch = new byte[256];
		foreach (BlockMember member in held.Members)
		{
			int at = lead + member.At;
			covered.Add((at, member.Bytes));

			// The class tiler names every gap so a hole in an object nobody has fully described
			// stays countable. A component is fully described, so its gaps are the compiler's
			// alignment and nothing else, and emitting them is noise.
			if (member.Kind == MemberKind.Unknown) continue;
			if (at + member.Bytes > read) { fields[member.Name] = null; continue; }

			if (member.Holds is not null)
			{
				var inner = new JsonObject();
				Members(process, address, window, read, BlockMembers.Held(member.Holds, BlockMembers.Source.Blocks),
					at, inner, covered);
				fields[member.Name] = inner;
				continue;
			}

			fields[member.Name] = BlockMemberReader.Node(process, address, window, scratch, at, member);
		}
	}

	/// <summary>
	///     The bytes of a component that no member of its class covers, as members in their place.
	///     Without them the instance does not add up to its measured size, and a reader cannot tell
	///     a component that was read whole from one where three fields were picked out of forty.
	/// </summary>
	private static void Leftovers(BedrockProcess process, ulong at, int size,
		List<(int At, int Bytes)> covered, JsonObject fields, bool declared)
	{
		if (size <= 0 || size > MaximumSize) return;

		var claimed = new bool[size];
		foreach ((int from, int length) in covered)
		{
			for (int i = from; i < from + length && i < size; i++) claimed[i] = true;
		}

		var body = new byte[size];
		if (!process.TryRead(at, body, size)) return;

		int unknown = 0;
		for (int i = 0; i < size; i++)
		{
			if (claimed[i]) continue;
			int end = i;
			while (end < size && !claimed[end]) end++;
			// Padding where a class declares the object, because then every byte a member does not
			// cover is one the compiler inserted for alignment. Unknown only where nothing declares
			// the object at all, which is the honest word for bytes nobody has accounted for.
			fields[$"{(declared ? "padding" : "unknown")}{++unknown}"] = new JsonObject
			{
				[declared ? "padding" : "unknown"] = end - i,
				["at"] = i,
				["raw"] = Convert.ToHexString(body, i, end - i)
			};
			i = end;
		}
	}

	/// <summary>How many instances of a group are asked. Enough that a wrong test cannot pass.</summary>
	private const int SampleSize = 64;

	/// <summary>How many carriers a numeric or shape test needs before its agreement means anything.</summary>
	private const int MinimumEvidence = 10;

	/// <summary>Where the flammable component keeps its two odds, past a leading flag.</summary>
	private const int FlammableOdds = 10;

	private static byte[] Bytes(BedrockProcess process, ulong at, int count, byte[] word)
	{
		var raw = new byte[count];
		return process.TryRead(at, raw, count) ? raw : null;
	}

	private static int[] Shorts(BedrockProcess process, ulong at, int count, byte[] word)
	{
		var raw = new byte[count * 2];
		if (!process.TryRead(at, raw, raw.Length)) return null;
		var values = new int[count];
		for (int i = 0; i < count; i++) values[i] = BitConverter.ToUInt16(raw, i * 2);
		return values;
	}

	private static float[] Floats(BedrockProcess process, ulong at, int count, byte[] word)
	{
		var raw = new byte[count * 4];
		if (!process.TryRead(at, raw, raw.Length)) return null;
		var values = new float[count];
		for (int i = 0; i < count; i++)
		{
			values[i] = BitConverter.ToSingle(raw, i * 4);
			if (!float.IsFinite(values[i])) return null;
		}
		return values;
	}

	/// <summary>
	///     Everything the component holds after its method table, as hex.
	///     Emitted rather than interpreted: a component whose meaning is unknown still has a value,
	///     and guessing at the value's type is how a float field that is not a float ends up in the
	///     output reading as 1.8e-40.
	/// </summary>
	private static string Payload(BedrockProcess process, ulong at, int size)
	{
		int length = size - FirstField;
		if (length is <= 0 or > MaximumSize) return null;

		var body = new byte[length];
		return process.TryRead(at + FirstField, body, length) ? Convert.ToHexString(body) : null;
	}

	/// <summary>The allocator's size for the object, so an unnamed component is still countable.</summary>
	/// <summary>
	///     How big a component instance is, from the class rather than from the heap. The method
	///     table's first slot is the deleting destructor, and the compiler writes the allocation
	///     size into it as an immediate before the call to operator delete, so this is the size the
	///     type actually has.
	///     It replaced a scan that walked forward to the allocator's next block header. That number
	///     is the block handed out, not the object: a component of a method table and one float is
	///     twelve bytes and the scan reported twenty four, so twelve bytes of somebody else's slack
	///     were being read and published as part of the component.
	/// </summary>
	private static int Size(BedrockProcess process, ulong at, byte[] word)
	{
		ulong vtable = process.ReadUInt64(at, word);
		return vtable < 0x10000 ? 0 : ItemRegistry.ClassSize(process, vtable);
	}

	/// <summary>An MSVC std::string, which is short inside the object and long behind a pointer.</summary>
	private static string StdString(BedrockProcess process, ulong at, byte[] word)
	{
		var head = new byte[32];
		if (!process.TryRead(at, head, head.Length)) return null;

		ulong length = BitConverter.ToUInt64(head, 16);
		ulong capacity = BitConverter.ToUInt64(head, 24);
		if (length == 0 || length > 512 || capacity < length) return null;

		byte[] body;
		if (capacity == 15)
		{
			if (length > 15) return null;
			body = head;
		}
		else
		{
			ulong pointer = BitConverter.ToUInt64(head, 0);
			if (pointer < 0x10000 || !process.IsMapped(pointer)) return null;
			body = new byte[length];
			if (!process.TryRead(pointer, body, body.Length)) return null;
		}

		for (int i = 0; i < (int) length; i++)
		{
			if (body[i] is < 0x20 or > 0x7E) return null;
		}
		return System.Text.Encoding.ASCII.GetString(body, 0, (int) length);
	}

	private static string Text(float value)
	{
		return value.ToString("0.######", CultureInfo.InvariantCulture);
	}
}
