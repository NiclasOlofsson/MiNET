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

using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

/// <summary>
///     The creative inventory, as the server holds it: every stack the creative menu offers, in the
///     server's own order, under the groups it puts them in.
///     <para>
///         This is the one place an item stack is published rather than an item. An item is the kind
///         of thing; a stack is a kind of thing with a count, a damage value, a block state and, for
///         the enchanted books, the tipped arrows, the potions and the trimmed armour, a compound of
///         user data that is the whole difference between one entry and the next. Nothing else the
///         server ships states those compounds.
///     </para>
///     <para>
///         The registry is found by the loop it closes rather than by a signature. CreativeItemRegistry
///         keeps its entries in a vector, and every entry points back at the registry and states its
///         own place in that vector. So a candidate is three words that read as a vector whose extent
///         divides by the entry size, and it is accepted only when every entry in it points back at
///         the object the reference says the vector belongs to and numbers itself by where it sits.
///         Arbitrary bytes do not do that thousands of times in a row.
///     </para>
/// </summary>
public static class CreativeItems
{
	private const BlockMembers.Source Which = BlockMembers.Source.Creative;

	/// <summary>MSVC lays a vector out as the first element, the end, then the end of the allocation.</summary>
	private const int VectorBegin = 0;
	private const int VectorEnd = 8;
	private const int VectorCapacity = 16;
	private const int VectorSize = 24;

	/// <summary>
	///     How few entries a candidate may hold and still be considered. The creative list runs to
	///     thousands; this is only here to keep a two element vector of something else out.
	/// </summary>
	private const int LeastEntries = 128;

	private const int MostEntries = 200_000;

	private static ClassLayout Class(string held) => BlockMembers.Held(held, Which);

	private static int At(string held, string member)
	{
		BlockMember found = Class(held).Members.FirstOrDefault(m => m.Name == member);
		if (found.Name is null) throw new InvalidOperationException($"the reference states no {member} in {held}");
		return found.At;
	}

	private static int Size(string held) => Class(held).Size;

	/// <summary>
	///     The creative registry, the groups and every stack in it, written beside the other
	///     extractions. Returns zero when every check held.
	/// </summary>
	public static int Run(BedrockProcess process, IReadOnlyList<PaletteEntry> palette)
	{
		var states = new Palette(palette);

		string output = Path.Combine(Program.DefaultOutputDirectory(), "creative_items.json");
		Console.WriteLine("creative inventory:");

		int entrySize = Size("CreativeItemEntry");
		int groupSize = Size("CreativeGroupInfo");
		int itemsAt = At("CreativeItemRegistry", "creativeItems");
		int groupsAt = At("CreativeItemRegistry", "creativeGroups");

		List<Found> candidates = Search(process, entrySize, itemsAt);
		Console.WriteLine($"  registries closing the loop: {candidates.Count}");
		foreach (Found candidate in candidates)
		{
			Console.WriteLine($"    0x{candidate.Registry:X} with {candidate.Count:N0} entries, "
							+ $"method table 0x{candidate.MethodTable:X}");
		}

		if (candidates.Count == 0)
		{
			Console.Error.WriteLine("  no object holds a vector of entries that point back at it and number themselves, "
								+ $"so the creative registry was not found at entry size {entrySize} "
								+ $"and creativeItems at +{itemsAt}");
			return 1;
		}

		// More than one is not a choice to make quietly. The largest is taken, the rest are named
		// above, and the file says how many there were.
		Found registry = candidates.OrderByDescending(c => c.Count).First();

		var nbt = new CompoundTagReader(process);
		var word = new byte[8];

		// Read once and throw it away, then read again for the file. A list holds pointers to tags
		// and a tag states no type of its own, so a list is only readable once some entry elsewhere
		// has named that class. Which entry that is depends on where it sits in the list, and the
		// first pass removes the order from the answer.
		int userDataAt = At("ItemInstance", "userData");
		int instanceAt = At("CreativeItemEntry", "itemInstance");
		for (int i = 0; i < registry.Count; i++)
		{
			ulong data = process.ReadUInt64(
				registry.Begin + (ulong) (i * entrySize + instanceAt + userDataAt), word);
			if (data >= 0x10000) nbt.Read(data);
		}

		Console.WriteLine($"  tag classes named on the first pass: {nbt.Tables.Count} of 12, "
						+ $"from {nbt.Entries:N0} entries");
		nbt.Reset();

		// The groups first, so an entry can name the group it belongs to rather than only its index.
		var groups = new List<Group>();
		if (Vector(process, registry.Registry + (ulong) groupsAt, groupSize, word) is { } held)
		{
			for (int i = 0; i < held.Count; i++)
			{
				groups.Add(ReadGroup(process, held.Begin + (ulong) (i * groupSize), nbt, word, states));
			}
		}

		Console.WriteLine($"  groups: {groups.Count:N0}");

		var entries = new List<Entry>(registry.Count);
		for (int i = 0; i < registry.Count; i++)
		{
			entries.Add(ReadEntry(process, registry.Begin + (ulong) (i * entrySize), i, groups, nbt, word, states));
		}

		int named = entries.Count(e => e.Item is not null);
		int withData = entries.Count(e => e.UserData is not null);
		int pickupTimes = entries.Count(e => e.PickupTimeSet);
		Console.WriteLine($"  entries: {entries.Count:N0}, {named:N0} naming an item, {withData:N0} carrying user data");
		Console.WriteLine($"  tags read: {nbt.Entries:N0}, {nbt.Disagreed:N0} whose class and type number disagree, "
						+ $"{nbt.UntypedElements:N0} list elements of a class no variant named, "
						+ $"{nbt.Miscounted:N0} compounds whose node count is not the count they state");

		Console.WriteLine($"  block states: {states.Named:N0} stacks naming one, {states.Verified:N0} whose network id "
						+ $"indexes the palette row they were read from, {states.Unnamed:N0} at an address the palette "
						+ $"does not hold");
		foreach (string failure in states.Failures) Console.WriteLine($"    {failure}");

		var document = new JsonObject
		{
			["source"] = ItemRegistry.Source?.DeepClone(),
			["registry"] = new JsonObject
			{
				["entries"] = registry.Count,
				["groups"] = groups.Count,
				["otherCandidates"] = candidates.Count - 1
			},
			["checks"] = new JsonObject
			{
				["entriesNamingAnItem"] = named,
				["entriesCarryingUserData"] = withData,
				["tagEntriesRead"] = nbt.Entries,
				["tagClassAndTypeDisagreed"] = nbt.Disagreed,
				["listElementsOfAnUnnamedClass"] = nbt.UntypedElements,
				["compoundsMiscounted"] = nbt.Miscounted,
				["tagClassesNamed"] = new JsonArray(nbt.Tables.Keys.OrderBy(k => k)
					.Select(k => (JsonNode) k).ToArray()),
				["entriesWhosePickupTimeIsSet"] = pickupTimes,
				["stacksNamingABlockState"] = states.Named,
				["blockStatesIndexingTheirOwnPaletteRow"] = states.Verified,
				["blockStatesAtNoPaletteAddress"] = states.Unnamed,
				["blockStateFailures"] = new JsonArray(states.Failures.Select(f => (JsonNode) f).ToArray())
			}
		};

		// The layout this run read with, carried through so the file can become the next reference.
		foreach ((string key, JsonNode node) in Reference())
		{
			document[key] = node;
		}

		document["groups"] = new JsonArray(groups.Select(g => (JsonNode) g.Stated).ToArray());
		document["items"] = new JsonArray(entries.Select(e => (JsonNode) e.Stated).ToArray());

		Directory.CreateDirectory(Program.DefaultOutputDirectory());
		File.WriteAllText(output, BlockDocument.Serialize(document), new UTF8Encoding(false));
		Console.WriteLine($"written {output}");

		bool sound = nbt.Disagreed == 0 && nbt.Miscounted == 0 && named == entries.Count
					&& states.Unnamed == 0 && states.Verified == states.Named;
		if (!sound)
		{
			Console.WriteLine("  a check did not hold. The file is written, marked with the counts, and this exits non zero.");
		}

		return sound ? 0 : 1;
	}

	/// <summary>The class list and the enums, straight out of the reference, so the output states its own layout.</summary>
	private static IEnumerable<(string, JsonNode)> Reference()
	{
		string path = Path.Combine(WorldConfig.AssetsDirectory(), "reference", "creative_items.json");
		if (!File.Exists(path)) yield break;

		JsonNode stated = JsonNode.Parse(File.ReadAllText(path));
		foreach (string key in new[] { "class", "classes", "enums" })
		{
			if (stated?[key] is { } held) yield return (key, held.DeepClone());
		}
	}

	private readonly record struct Found(ulong Registry, ulong Begin, int Count, ulong MethodTable);

	private readonly record struct Span(ulong Begin, int Count);

	private sealed class Group
	{
		public uint Index;
		public string Name;
		public JsonObject Stated;
	}

	/// <summary>
	///     The block palette this run read, by the address each state sits at.
	///     <para>
	///         A stack holds a Block, and a Block is one state of a block, not the block. The palette
	///         is that same object read once, so a stack's pointer is answered by the entry at its
	///         address rather than by a second reading of the state.
	///     </para>
	///     <para>
	///         The network id read there is checked against the palette it came from: the row it
	///         indexes has to be the row the pointer named, name and every state property. An id that
	///         indexes a different state is the reading being wrong, and it is counted and said.
	///     </para>
	/// </summary>
	private sealed class Palette
	{
		private readonly IReadOnlyList<PaletteEntry> _rows;
		private readonly Dictionary<ulong, PaletteEntry> _byAddress;

		public Palette(IReadOnlyList<PaletteEntry> rows)
		{
			_rows = rows;
			_byAddress = new Dictionary<ulong, PaletteEntry>();
			foreach (PaletteEntry row in rows) _byAddress.TryAdd(row.Address, row);
		}

		/// <summary>Stacks holding a block, so a state was named.</summary>
		public int Named { get; private set; }

		/// <summary>Of those, the ones whose network id indexes the row they were read from.</summary>
		public int Verified { get; private set; }

		/// <summary>Stacks holding a block the palette has no entry for, so nothing was read.</summary>
		public int Unnamed { get; private set; }

		public List<string> Failures { get; } = [];

		public JsonNode State(ulong block)
		{
			if (block < 0x10000) return null;
			if (!_byAddress.TryGetValue(block, out PaletteEntry entry))
			{
				Unnamed++;
				Failures.Add($"0x{block:X} is a block the palette holds no state for, so no state was read");
				return BlockMemberReader.Pointer(new JsonObject { ["unread"] = "the palette holds no state at this address" }, block);
			}

			Named++;
			var states = new JsonObject();
			foreach (StateProperty property in entry.States) states[property.Name] = property.ToNode();

			// The id has to index the state it was read from, checked against the palette rather than
			// believed. Anything else and the number on the wire names a different block.
			PaletteEntry indexed = entry.NetworkId < (uint) _rows.Count ? _rows[(int) entry.NetworkId] : null;
			if (indexed is not null && indexed.Name == entry.Name && Same(indexed.States, entry.States))
			{
				Verified++;
			}
			else
			{
				Failures.Add($"{entry.Name} at 0x{block:X} states network id {entry.NetworkId}, which is "
							+ (indexed is null
								? $"past the {_rows.Count} rows of the palette"
								: $"{indexed.Name} {Describe(indexed.States)} and not {entry.Name} {Describe(entry.States)}"));
			}

			return new JsonObject
			{
				["name"] = entry.Name,
				["states"] = states,
				["networkId"] = entry.NetworkId
			};
		}

		private static bool Same(IReadOnlyList<StateProperty> left, IReadOnlyList<StateProperty> right)
		{
			if (left.Count != right.Count) return false;
			for (int i = 0; i < left.Count; i++)
			{
				if (left[i].Name != right[i].Name || left[i].ToJson() != right[i].ToJson()) return false;
			}

			return true;
		}

		private static string Describe(IReadOnlyList<StateProperty> states) =>
			"[" + string.Join(", ", states.Select(s => $"{s.Name}={s.ToJson()}")) + "]";
	}

	private sealed class Entry
	{
		/// <summary>
		///     Whether the stack carries a pickup time. The value itself is a reading of this run's
		///     clock rather than anything about the stack, so it is counted and not written per row:
		///     a number that differs on every run of the same server tells a reader nothing and
		///     turns every extraction into a whole-file change.
		/// </summary>
		public bool PickupTimeSet;

		public string Item;
		public JsonObject UserData;
		public JsonObject Stated;
	}

	/// <summary>
	///     A vector at an address, when the three words read as one whose extent divides by the size
	///     of what it holds. Null otherwise, so a caller never walks a run it cannot measure.
	/// </summary>
	private static Span? Vector(BedrockProcess process, ulong at, int stride, byte[] word)
	{
		var header = new byte[VectorSize];
		if (!process.TryRead(at, header, header.Length)) return null;

		ulong begin = BitConverter.ToUInt64(header, VectorBegin);
		ulong end = BitConverter.ToUInt64(header, VectorEnd);
		ulong capacity = BitConverter.ToUInt64(header, VectorCapacity);
		if (begin < 0x10000 || end < begin || capacity < end) return null;
		if ((end - begin) % (ulong) stride != 0 || (capacity - begin) % (ulong) stride != 0) return null;

		ulong count = (end - begin) / (ulong) stride;
		return count > MostEntries ? null : new Span(begin, (int) count);
	}

	/// <summary>
	///     Every object in the process that holds a vector of creative entries. The sweep is over
	///     three word candidates and the acceptance is the loop those entries close, which is why
	///     nothing here needs to know what the registry's method table is.
	/// </summary>
	private static List<Found> Search(BedrockProcess process, int entrySize, int itemsAt)
	{
		var found = new List<Found>();
		var word = new byte[8];
		var seen = new HashSet<ulong>();

		foreach (BedrockProcess.Region region in process.Regions)
		{
			var scan = new byte[(int) Math.Min(8 * 1024 * 1024, (long) region.Size)];
			for (ulong at = region.Base; at < region.End; at += (ulong) scan.Length - VectorSize)
			{
				int length = (int) Math.Min((ulong) scan.Length, region.End - at);
				if (length < VectorSize || !process.TryRead(at, scan, length)) continue;

				for (int i = 0; i + VectorSize <= length; i += 8)
				{
					ulong begin = BitConverter.ToUInt64(scan, i + VectorBegin);
					ulong end = BitConverter.ToUInt64(scan, i + VectorEnd);
					ulong capacity = BitConverter.ToUInt64(scan, i + VectorCapacity);
					if (begin < 0x10000 || end <= begin || capacity < end) continue;

					ulong bytes = end - begin;
					if (bytes % (ulong) entrySize != 0 || (capacity - begin) % (ulong) entrySize != 0) continue;

					ulong count = bytes / (ulong) entrySize;
					if (count < LeastEntries || count > MostEntries) continue;

					ulong registry = at + (ulong) i - (ulong) itemsAt;
					if (!seen.Add(registry)) continue;
					if (Closes(process, registry, begin, (int) count, entrySize, word, out ulong table))
					{
						found.Add(new Found(registry, begin, (int) count, table));
					}
				}
			}
		}

		return found;
	}

	/// <summary>
	///     Whether every entry of a candidate points back at the object the vector would belong to
	///     and states its own place in the run. All of them, not a sample: one entry agreeing is a
	///     coincidence and a thousand is the registry.
	/// </summary>
	private static bool Closes(BedrockProcess process, ulong registry, ulong begin, int count, int entrySize,
		byte[] word, out ulong table)
	{
		table = 0;
		if (registry < 0x10000 || !process.IsMapped(registry)) return false;

		int registryAt = At("CreativeItemEntry", "registry");
		int indexAt = At("CreativeItemEntry", "index");
		var body = new byte[entrySize];

		for (int i = 0; i < count; i++)
		{
			if (!process.TryRead(begin + (ulong) (i * entrySize), body, body.Length)) return false;
			if (BitConverter.ToUInt64(body, registryAt) != registry) return false;
			if (BitConverter.ToUInt32(body, indexAt) != (uint) i) return false;

			ulong own = BitConverter.ToUInt64(body, 0);
			if (i == 0) table = own;
			else if (own != table) return false;
		}

		return count > 0;
	}

	/// <summary>One creative group: which tab it is under, what it is called, and the stack it shows.</summary>
	private static Group ReadGroup(BedrockProcess process, ulong at, CompoundTagReader nbt, byte[] word,
		Palette palette)
	{
		var body = new byte[Size("CreativeGroupInfo")];
		var scratch = new byte[256];
		if (!process.TryRead(at, body, body.Length)) return new Group { Stated = Unread("CreativeGroupInfo") };

		// One byte, and what follows it is not part of it. The bytes between the category and the
		// registry pointer are the alignment the compiler left there, and they hold whatever the
		// allocation held before, because the class copies itself member by member and a copy
		// leaves padding alone. Read as four they turn Construction into 7340033. The class table
		// states that gap with its offset and its length, so it is not repeated on every row.
		int category = body[At("CreativeGroupInfo", "category")];

		uint index = BitConverter.ToUInt32(body, At("CreativeGroupInfo", "index"));
		string name = HashedString.ReadVerified(process, body, At("CreativeGroupInfo", "name"), scratch, 1, 96);

		Span? members = Vector(process, at + (ulong) At("CreativeGroupInfo", "itemIndexes"), 4, word);
		var indexes = new JsonArray();
		if (members is { } run)
		{
			var numbers = new byte[run.Count * 4];
			if (process.TryRead(run.Begin, numbers, numbers.Length))
			{
				for (int i = 0; i < run.Count; i++) indexes.Add(BitConverter.ToUInt32(numbers, i * 4));
			}
		}

		var stated = new JsonObject
		{
			["index"] = index,
			["name"] = name,
			["category"] = BlockMembers.Value("CreativeItemCategory", category) ?? category.ToString(),
			["categoryValue"] = category,
			["icon"] = Instance(process, at + (ulong) At("CreativeGroupInfo", "icon"), nbt, word, palette),
			["items"] = indexes
		};

		return new Group { Index = index, Name = name, Stated = stated };
	}

	/// <summary>One entry of the creative list: the stack, its place, and the group it sits in.</summary>
	private static Entry ReadEntry(BedrockProcess process, ulong at, int position, List<Group> groups,
		CompoundTagReader nbt, byte[] word, Palette palette)
	{
		var body = new byte[Size("CreativeItemEntry")];
		if (!process.TryRead(at, body, body.Length))
		{
			return new Entry { Stated = Unread("CreativeItemEntry") };
		}

		uint groupIndex = BitConverter.ToUInt32(body, At("CreativeItemEntry", "groupIndex"));
		uint netId = BitConverter.ToUInt32(body, At("CreativeItemEntry", "creativeNetId"));
		JsonObject instance = Instance(process, at + (ulong) At("CreativeItemEntry", "itemInstance"), nbt, word,
			palette);

		var stated = new JsonObject
		{
			["index"] = position,
			["creativeNetId"] = netId,
			["groupIndex"] = groupIndex,
			["group"] = groups.FirstOrDefault(g => g.Index == groupIndex)?.Name
		};

		foreach (KeyValuePair<string, JsonNode> member in instance.ToList())
		{
			instance.Remove(member.Key);
			stated[member.Key] = member.Value;
		}

		return new Entry
		{
			PickupTimeSet = BitConverter.ToUInt64(body,
				At("CreativeItemEntry", "itemInstance") + At("ItemInstance", "pickupTime")) != 0,
			Item = stated["item"]?.GetValue<string>(),
			UserData = stated["userData"] as JsonObject,
			Stated = stated
		};
	}

	/// <summary>
	///     One ItemInstance, every member of it. The item is a weak pointer, which is the counter
	///     rather than the item, so the object is one hop past it and its name verifies against its
	///     own hash there or is not read at all.
	///     <para>
	///         The block is a Block, which is one state of a block rather than the block, so it is
	///         written as that state: the name, what the state is, and the network id the wire carries
	///         for it. A stack can hold a state that is not its block's default, and a name alone
	///         throws that away.
	///     </para>
	/// </summary>
	private static JsonObject Instance(BedrockProcess process, ulong at, CompoundTagReader nbt, byte[] word,
		Palette palette)
	{
		var body = new byte[Size("ItemInstance")];
		var scratch = new byte[256];
		if (!process.TryRead(at, body, body.Length)) return Unread("ItemInstance");

		ulong counter = BitConverter.ToUInt64(body, At("ItemInstance", "item"));
		ulong item = counter >= 0x10000 ? process.ReadUInt64(counter, word) : 0;
		ulong userData = BitConverter.ToUInt64(body, At("ItemInstance", "userData"));
		ulong block = BitConverter.ToUInt64(body, At("ItemInstance", "block"));
		ulong charged = BitConverter.ToUInt64(body, At("ItemInstance", "chargedItem"));

		var stated = new JsonObject
		{
			["item"] = ItemName(process, item, scratch),
			["count"] = body[At("ItemInstance", "count")],
			["auxValue"] = BitConverter.ToInt16(body, At("ItemInstance", "auxValue")),
			["block"] = palette.State(block),
			["valid"] = body[At("ItemInstance", "valid")] != 0,
			["showPickUp"] = body[At("ItemInstance", "showPickUp")] != 0,
			["wasPickedUp"] = body[At("ItemInstance", "wasPickedUp")] != 0,
			["blockingTick"] = BitConverter.ToUInt64(body, At("ItemInstance", "blockingTick")),
			["canPlaceOn"] = Blocks(process, at + (ulong) At("ItemInstance", "canPlaceOn"), word, scratch),
			["canPlaceOnHash"] = BitConverter.ToUInt64(body, At("ItemInstance", "canPlaceOnHash")),
			["canDestroy"] = Blocks(process, at + (ulong) At("ItemInstance", "canDestroy"), word, scratch),
			["canDestroyHash"] = BitConverter.ToUInt64(body, At("ItemInstance", "canDestroyHash")),
			["userData"] = userData >= 0x10000 ? nbt.Read(userData) : null,
			["chargedItem"] = charged >= 0x10000 ? Instance(process, charged, nbt, word, palette) : null
		};

		return stated;
	}

	/// <summary>The blocks a vector of BlockType pointers names, each verified by its own hash.</summary>
	private static JsonArray Blocks(BedrockProcess process, ulong at, byte[] word, byte[] scratch)
	{
		var named = new JsonArray();
		Span? held = Vector(process, at, 8, word);
		if (held is not { } run || run.Count == 0) return named;

		for (int i = 0; i < run.Count; i++)
		{
			ulong legacy = process.ReadUInt64(run.Begin + (ulong) (i * 8), word);
			named.Add(Hashed(process, legacy + (ulong) MemoryLayout.NameInsideLegacy, scratch)
					?? $"unreadBlock");
		}

		return named;
	}

	/// <summary>The item an object is, by the name it carries where the item sweep measured one.</summary>
	private static string ItemName(BedrockProcess process, ulong item, byte[] scratch) =>
		item < 0x10000 ? null : Hashed(process, item + (ulong) ItemRegistry.NameInsideItem, scratch);

	private static string Hashed(BedrockProcess process, ulong at, byte[] scratch)
	{
		if (at < 0x10000 || !process.IsMapped(at)) return null;
		var body = new byte[HashedString.Size];
		return process.TryRead(at, body, body.Length)
			? HashedString.ReadVerified(process, body, 0, scratch, 1, 96)
			: null;
	}

	/// <summary>An object that could not be read, said so rather than left out.</summary>
	private static JsonObject Unread(string held) => new()
	{
		["unread"] = held,
		["bytes"] = Size(held)
	};
}
