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

/// <summary>
///     Finds the registries themselves, rather than the objects they hold.
///     <para>
///         The sweeps elsewhere find items and rules by what they look like, which leaves two
///         questions they cannot answer: whether the population is complete, and which of several
///         copies of a name is the one the server is using. Both are answered by the container: a
///         registry states its own membership, so what it holds is the answer and what it does not
///         hold is not.
///     </para>
///     <para>
///         Nothing here is identified by adjacency. The walk starts from members already proven and
///         goes backwards through pointers: an object is only accepted when it closes over the whole
///         member set, and the arrangement that accepts it has to be one arbitrary bytes cannot
///         satisfy. A registry vector is a run of pointers each landing on a counter that points
///         back at a proven item; a vector header is three words that agree with that run's extent.
///     </para>
///     <para>
///         The shapes come from the server's own class layouts. SharedCounter is a pointer and two
///         counts; SharedPtr and WeakPtr are the counter pointer and nothing else, which is why
///         scanning memory for an item's address finds counters rather than the registry. ItemRegistry
///         keeps its items in a vector of SharedPtr followed by a map keyed by id and a map keyed by
///         name, so one object anchoring all three is the convergence that identifies it.
///     </para>
/// </summary>
public static class RegistryDiscovery
{
	/// <summary>SharedCounter: the object, then the strong and weak counts as two 32 bit numbers.</summary>
	private const int CounterShare = 8;
	private const int CounterWeak = 12;

	/// <summary>
	///     A count no live registry reaches, so a word pair that merely looks like counts is refused.
	///     Emitted rather than assumed: candidates rejected here are reported with their values.
	/// </summary>
	private const int CountCeiling = 1_000_000;

	/// <summary>
	///     MSVC lays an unordered_map out as the load factor, the node list, the bucket vector and
	///     two mask words. Only the first two are needed: the list is the membership.
	/// </summary>
	private const int MapListHead = 8;
	private const int MapSize = 16;

	/// <summary>A list node is its two links, then the value.</summary>
	private const int NodeValue = 16;

	/// <summary>
	///     How far in front of its name an object may start. Wide enough to cover any item class
	///     these builds have; what the distance actually is comes from the counters agreeing on it.
	/// </summary>
	private const ulong MaximumLead = 1024;

	public static int Run(string[] args)
	{
		string pathFilter = null;
		string output = Path.Combine(Program.DefaultOutputDirectory(), "registry-discovery.json");
		for (int i = 0; i < args.Length; i++)
		{
			if (args[i] == "--server" && i + 1 < args.Length) pathFilter = args[++i];
			else if (args[i] == "--out" && i + 1 < args.Length) output = args[++i];
		}

		using var process = BedrockProcess.Attach(pathFilter);
		Console.WriteLine($"reading pid {process.Id}");
		Console.WriteLine($"  {process.ExecutablePath}");
		Console.WriteLine();

		List<ItemRegistry.Item> swept = ItemRegistry.Read(process);
		Console.WriteLine($"objects with a verified name: {swept.Count:N0}");

		var report = new StringBuilder("{\n");
		report.Append($"\t\"server\": \"{process.ExecutablePath.Replace("\\", "\\\\")}\",\n");
		report.Append($"\t\"objectsSwept\": {swept.Count},\n");

		int found = FindItemRegistry(process, swept, report);
		found += FindBlockRegistry(process, report);
		found += FindUpgradeContext(process, report);

		report.Append("\t\"end\": true\n}\n");
		if (Path.GetDirectoryName(output) is { Length: > 0 } directory) Directory.CreateDirectory(directory);
		File.WriteAllText(output, report.ToString(), new UTF8Encoding(false));
		Console.WriteLine($"written {output}");
		return found;
	}

	/// <summary>
	///     The item registry, from the items outward: every counter that points at a swept object,
	///     the run of counter pointers that is the registry's own vector, the three words that head
	///     that vector, and the object those words sit in.
	/// </summary>
	private static int FindItemRegistry(BedrockProcess process, List<ItemRegistry.Item> swept, StringBuilder report)
	{
		// Every swept object, not only the ones that passed the item checks. Which copies are live
		// is the question this answers, so pre-filtering the candidates would answer it in advance.
		//
		// The name is the only part proven on every build: it hashes to the hash beside it. How far
		// the object starts in front of its name is a layout detail that moves between versions, so
		// it is derived here instead of assumed. Every address within reach in front of a verified
		// name is offered, and the counters decide which distance is the real one by all agreeing
		// on it. A wrong distance cannot be held by a thousand counters.
		var leads = new Dictionary<ulong, ulong>();   // candidate object start -> its name
		foreach (ItemRegistry.Item item in swept)
		{
			for (ulong lead = 0; lead <= MaximumLead; lead += 8)
			{
				if (item.NameAddress > lead) leads.TryAdd(item.NameAddress - lead, item.NameAddress);
			}
		}

		Console.WriteLine($"scanning for counters holding an object in front of one of {swept.Count:N0} names");
		Dictionary<ulong, List<ulong>> holders = Locate(process, leads.Keys, 64);

		var word = new byte[16];
		var counters = new Dictionary<ulong, ulong>();  // counter -> object
		var byLead = new Dictionary<ulong, int>();      // name distance -> counters agreeing on it
		int rejected = 0;
		foreach ((ulong start, List<ulong> places) in holders)
		{
			foreach (ulong place in places)
			{
				// The counter's own shape decides, not where the pointer was found: the object
				// first, then a strong count that a registered item cannot have at zero, then a
				// weak count. Arbitrary bytes holding this address do not also hold two counts.
				if (!process.TryRead(place, word, word.Length)) continue;
				int share = BitConverter.ToInt32(word, CounterShare);
				int weak = BitConverter.ToInt32(word, CounterWeak);
				if (share is < 1 or > CountCeiling || weak is < 0 or > CountCeiling)
				{
					rejected++;
					continue;
				}
				counters[place] = start;
				ulong lead = leads[start] - start;
				byLead[lead] = byLead.GetValueOrDefault(lead) + 1;
			}
		}

		// Which distance is this build's own is settled by the registry, not by the vote. The
		// distances are tried in order of how many counters agree, and the first that leads to an
		// object anchoring the two maps is the answer: on 1.19 the leading distance loses to the
		// runner up by a tenth, and the runner up is the one items are actually at.
		List<KeyValuePair<ulong, int>> ranked = byLead.OrderByDescending(p => p.Value).ToList();
		Console.WriteLine($"counters: {counters.Count:N0} over {counters.Values.Distinct().Count():N0} objects, "
						+ $"{rejected:N0} holders rejected for their counts");
		Console.WriteLine($"name distances the counters hold: {string.Join(", ", ranked.Take(4).Select(r => $"{r.Key} by {r.Value:N0}"))}");
		report.Append($"\t\"counters\": {counters.Count},\n");
		report.Append($"\t\"holdersRejected\": {rejected},\n");
		report.Append($"\t\"nameDistances\": [{string.Join(", ", ranked.Take(8).Select(r => $"{{ \"lead\": {r.Key}, \"counters\": {r.Value} }}"))}],\n");

		foreach ((ulong lead, int agreeing) in ranked)
		{
			// Only the counters at this distance. A counter at another one points at something
			// that is not an item object, whatever else it is.
			var selected = new Dictionary<ulong, ulong>();
			foreach ((ulong counter, ulong start) in counters)
			{
				if (leads[start] - start == lead) selected[counter] = start;
			}
			if (selected.Count < 8) continue;

			Console.WriteLine($"trying {lead} bytes ({agreeing:N0} counters): scanning for runs of counter pointers");
			Dictionary<ulong, List<ulong>> slots = Locate(process, selected.Keys, 16);
			List<SlotRun> runs = Runs(slots.SelectMany(s => s.Value));
			if (runs.Count == 0)
			{
				Console.WriteLine("  no run of counter pointers at this distance");
				continue;
			}

			Console.WriteLine($"  runs: {runs.Count} (largest {runs.Max(r => r.Count):N0} slots)");
			foreach (SlotRun run in runs.OrderByDescending(r => r.Count))
			{
				ulong last = run.Begin + (ulong) (run.Count - 1) * 8;
				foreach (Vector vector in Headers(process, run.Begin, last, run.Count))
				{
					Console.WriteLine($"  header 0x{vector.At:X} declares {vector.Count:N0} slots"
									+ $" (0x{vector.Begin:X}..0x{vector.End:X})");
					if (!Anchors(process, vector, selected, swept, lead, report)) continue;
					report.Append($"\t\"nameInsideObject\": {lead},\n");
					Console.WriteLine($"  the name sits {lead} bytes into an item object on this build");
					return 0;
				}
			}
		}

		Console.Error.WriteLine("no object anchors a run of counters together with an id map and a name map");
		report.Append("\t\"itemRegistry\": null,\n");
		return 1;
	}

	/// <summary>
	///     Whether the object holding this vector header is the item registry, which is decided by
	///     what else it holds. The registry keeps the vector, then a map keyed by id, then a map
	///     keyed by name, so the test is that all three describe the same population. The distance
	///     between them is measured here rather than assumed: the maps are looked for at every
	///     offset behind the vector, and the offsets that hold are reported.
	/// </summary>
	private static bool Anchors(BedrockProcess process, Vector vector,
		Dictionary<ulong, ulong> counters, List<ItemRegistry.Item> swept, ulong lead, StringBuilder report)
	{
		// What the vector holds, which is the population every other claim is measured against.
		var word = new byte[8];
		var held = new HashSet<ulong>();
		for (int i = 0; i < vector.Count; i++) held.Add(process.ReadUInt64(vector.Begin + (ulong) i * 8, word));

		// The maps behind it, and for each one whether its entries lead back to the very pointers
		// the vector holds. Counting entries is not enough and was not right: the registry's map by
		// id legitimately holds more entries than the vector has members. What cannot happen by
		// accident is a list of nodes each carrying, at one fixed place, a pointer out of this
		// exact set.
		var maps = new List<(int Offset, long Size, int Nodes, int Matched, int ValueAt)>();
		for (int offset = 24; offset <= 512; offset += 8)
		{
			long size = MapCount(process, vector.At + (ulong) offset);
			if (size <= 0) continue;
			(int nodes, int matched, int valueAt) = MapPointsInto(process, vector.At + (ulong) offset, held);
			maps.Add((offset, size, nodes, matched, valueAt));
		}

		List<(int Offset, long Size, int Nodes, int Matched, int ValueAt)> matching =
			maps.Where(m => m.Nodes > 0 && m.Matched == m.Nodes).ToList();
		if (matching.Count < 2)
		{
			Console.WriteLine($"    maps behind it: {(maps.Count == 0 ? "none" : string.Join(", ", maps.Select(m => $"+{m.Offset} of {m.Size} ({m.Matched}/{m.Nodes} lead into the vector)")))}"
							+ "; needed two whose every entry does");
			return false;
		}

		Console.WriteLine($"  header 0x{vector.At:X} anchors {matching.Count} maps whose every entry leads into the vector, "
						+ $"at {string.Join(", ", matching.Select(m => $"+{m.Offset} ({m.Nodes} entries, pointer at node+{m.ValueAt})"))}");

		// What each map is keyed BY, read rather than inferred from the value's distance. One of
		// these has to be a table of numbers and the other a table of names, which is what makes
		// them an id lookup and a name lookup rather than two maps of something else.
		var keyRows = new List<string>();
		foreach ((int offset, long _, int _, int _, int valueAt) in matching)
		{
			string described = DescribeKeys(process, vector.At + (ulong) offset, valueAt);
			Console.WriteLine($"    +{offset} keyed by {described}");
			keyRows.Add($"{{ \"at\": {offset}, \"keys\": \"{described}\" }}");
		}

		report.Append("\t\"itemRegistry\": {\n");
		report.Append($"\t\t\"vectorHeader\": \"0x{vector.At:X}\",\n");
		report.Append($"\t\t\"items\": {vector.Count},\n");
		report.Append($"\t\t\"mapsIntoTheVector\": [{string.Join(", ", matching.Select(m => m.Offset))}],\n");
		report.Append($"\t\t\"mapKeys\": [{string.Join(", ", keyRows)}],\n");
		report.Append($"\t\t\"allMaps\": [{string.Join(", ", maps.Select(m => $"{{ \"at\": {m.Offset}, \"entries\": {m.Size}, \"leadIntoVector\": {m.Matched}, \"pointerAtNode\": {m.ValueAt} }}"))}],\n");

		// What the vector holds, read through it rather than swept: every slot, its counter, and the
		// object that counter points at. A slot whose counter was not among the proven ones is a
		// member the sweep never found, which is exactly what this walk exists to surface.
		// Compared at the distance this build actually uses, not the one the item reader compiles
		// in. Holding the sweep to a foreign build's constant made every live member read as a
		// member the sweep had missed.
		var sweptObjects = new HashSet<ulong>(swept.Where(s => s.NameAddress > lead).Select(s => s.NameAddress - lead));
		var members = new List<(ulong Slot, ulong Counter, ulong Item, bool Swept)>();
		for (int i = 0; i < vector.Count; i++)
		{
			ulong slot = vector.Begin + (ulong) i * 8;
			ulong counter = process.ReadUInt64(slot, word);
			ulong item = counters.TryGetValue(counter, out ulong known) ? known : process.ReadUInt64(counter, word);
			members.Add((slot, counter, item, sweptObjects.Contains(item)));
		}

		// A member the sweep never reached is the whole reason for asking the container. Each is
		// named where its name still reads, so it can be chased rather than counted.
		List<(ulong Slot, ulong Counter, ulong Item, bool Swept)> missed = members.Where(m => !m.Swept).ToList();
		Console.WriteLine($"  members: {members.Count:N0}, of which {missed.Count:N0} were not found by the sweep");
		var heap = new byte[256];
		var probe = new byte[HashedString.Size];
		var missedRows = new List<string>();
		foreach ((ulong _, ulong counter, ulong item, bool _) in missed)
		{
			string name = lead > 0 && process.TryRead(item + lead, probe, probe.Length)
				? HashedString.ReadVerified(process, probe, 0, heap)
				: null;
			ulong vtable = process.ReadUInt64(item, word);
			Console.WriteLine($"    0x{item:X} counter 0x{counter:X} class +0x{(vtable > process.ModuleBase ? vtable - process.ModuleBase : 0):X} {name ?? "(name does not read)"}");
			missedRows.Add($"{{ \"item\": \"0x{item:X}\", \"counter\": \"0x{counter:X}\", "
						+ $"\"classRva\": {(vtable > process.ModuleBase ? vtable - process.ModuleBase : 0)}, "
						+ $"\"name\": {(name is null ? "null" : $"\"{name}\"")} }}");
		}

		report.Append($"\t\t\"membersNotSwept\": {missed.Count},\n");
		report.Append($"\t\t\"membersNotSweptDetail\": [{string.Join(", ", missedRows)}],\n");

		// Each member with the class it belongs to, stated as the offset into the server's own
		// image. That offset is what a symbol file names, so a build that ships symbols can be
		// asked whether these objects are the classes this claims they are.
		var classes = new Dictionary<ulong, int>();
		foreach ((ulong _, ulong _, ulong item, bool _) in members)
		{
			ulong vtable = process.ReadUInt64(item, word);
			if (vtable <= process.ModuleBase) continue;
			classes[vtable - process.ModuleBase] = classes.GetValueOrDefault(vtable - process.ModuleBase) + 1;
		}

		// Who points at the object this vector sits in. Where the object starts is not assumed: every
		// address the vector could be a member of is offered, and whatever the process points at
		// answers. A pointer from inside the server's own image is a static, and its offset into the
		// image is what a symbol file names.
		var starts = new List<ulong>();
		for (ulong behind = 0; behind <= 512; behind += 8)
		{
			if (vector.At > behind) starts.Add(vector.At - behind);
		}

		Dictionary<ulong, List<ulong>> pointing = Locate(process, starts, 64);
		var owners = new List<string>();
		foreach ((ulong start, List<ulong> places) in pointing.OrderBy(p => vector.At - p.Key))
		{
			foreach (ulong place in places.Where(p => p > process.ModuleBase && p - process.ModuleBase < 0x8000000))
			{
				Console.WriteLine($"    object at header-{vector.At - start} is pointed at from image+0x{place - process.ModuleBase:X}");
				owners.Add($"{{ \"behindHeader\": {vector.At - start}, \"staticAt\": {place - process.ModuleBase} }}");
			}
		}

		report.Append($"\t\t\"moduleBase\": \"0x{process.ModuleBase:X}\",\n");
		report.Append($"\t\t\"staticsPointingAtTheObject\": [{string.Join(", ", owners)}],\n");
		report.Append($"\t\t\"classesByImageOffset\": [{string.Join(", ", classes.OrderByDescending(c => c.Value)
			.Select(c => $"{{ \"rva\": {c.Key}, \"members\": {c.Value} }}"))}],\n");
		report.Append($"\t\t\"members\": [{string.Join(", ", members.Take(4096).Select(m => $"\"0x{m.Item:X}\""))}],\n");
		FindReflectionContext(process, vector, report);
		report.Append("\t\t\"end\": true\n");
		report.Append("\t},\n");
		return true;
	}

	/// <summary>
	///     The block registry, which is a map from name to block rather than a list, so it is found
	///     by its entries rather than by a run of pointers.
	///     <para>
	///         An entry is worth more than a slot in an array: it holds the block's name as well as
	///         the pointer to it, so a candidate entry can be asked whether the name it is filed
	///         under is the name the block itself carries. Two structures, written for different
	///         purposes, agreeing on the text for a thousand blocks is not something a wrong offset
	///         produces. That check settles the distance from a block's name to its start at the
	///         same time, which is the number the sweep elsewhere has to be told.
	///     </para>
	/// </summary>
	private static int FindBlockRegistry(BedrockProcess process, StringBuilder report)
	{
		Console.WriteLine();
		List<(ulong At, string Name)> names = NamedThings(process);
		Console.WriteLine($"namespaced names that hash to their own text: {names.Count:N0}");

		(ulong blockLead, int stateAt, int backAt, int roundTrips) = DeriveBlockLayout(process, names);
		if (roundTrips > 0)
		{
			Console.WriteLine($"a block and its state point at each other on {roundTrips:N0} of the names tried:");
			Console.WriteLine($"  the name sits {blockLead} bytes into a block, the state pointer is at name+{stateAt}, "
							+ $"and the state points back from its own +{backAt}");
			report.Append($"\t\"blockNameInside\": {blockLead},\n");
			report.Append($"\t\"blockStatePointerAt\": {stateAt},\n");
			report.Append($"\t\"blockStateBackPointerAt\": {backAt},\n");
			report.Append($"\t\"blockLayoutAgreed\": {roundTrips},\n");
		}
		else
		{
			Console.WriteLine("no block and state point at each other, so this build's block layout is not readable");
			report.Append("\t\"blockNameInside\": null,\n");
		}
		if (names.Count == 0)
		{
			report.Append("\t\"blockRegistry\": null,\n");
			return 1;
		}

		// Every address a named thing could start at, as with items: the distance is what is being
		// worked out, so all of them are offered and the entries decide.
		// A candidate start can be produced by more than one name, since names lie close together
		// in memory, so every name that could account for it is kept. Keeping only the first made
		// almost every entry fail a check it should have passed.
		var leads = new Dictionary<ulong, List<(ulong Name, string Text)>>();
		foreach ((ulong at, string name) in names)
		{
			for (ulong lead = 0; lead <= MaximumLead; lead += 8)
			{
				if (at <= lead) continue;
				if (!leads.TryGetValue(at - lead, out List<(ulong, string)> here)) leads[at - lead] = here = [];
				here.Add((at, name));
			}
		}

		Console.WriteLine("scanning for counters holding a named object");
		var word = new byte[16];
		var counters = new Dictionary<ulong, ulong>();   // counter -> the object it holds
		foreach ((ulong start, List<ulong> places) in Locate(process, leads.Keys, 32))
		{
			foreach (ulong place in places)
			{
				if (!process.TryRead(place, word, word.Length)) continue;
				int share = BitConverter.ToInt32(word, CounterShare);
				int weak = BitConverter.ToInt32(word, CounterWeak);
				if (share is < 1 or > CountCeiling || weak is < 0 or > CountCeiling) continue;
				counters[place] = start;
			}
		}
		Console.WriteLine($"counters: {counters.Count:N0} over {counters.Values.Distinct().Count():N0} objects");

		// What distances the counters vote for, before any map is looked at. If a kind of object is
		// not held by a counter at all it cannot be found this way, and this is where that shows.
		var votes = new Dictionary<ulong, int>();
		foreach ((ulong _, ulong start) in counters)
		{
			foreach ((ulong nameAt, string _) in leads[start]) votes[nameAt - start] = votes.GetValueOrDefault(nameAt - start) + 1;
		}
		Console.WriteLine($"distances the counters vote for: {string.Join(", ", votes.OrderByDescending(v => v.Value).Take(8)
			.Select(v => $"{v.Key} by {v.Value:N0}"))}");

		if (counters.Count == 0)
		{
			report.Append("\t\"blockRegistry\": null,\n");
			return 1;
		}

		// Where those counters are held. A holder inside a map entry sits a fixed way past the
		// entry's start, and the entry's key is a name: the pairing that matters is that the key
		// reads as the same text the object itself carries.
		Console.WriteLine("scanning for map entries holding those counters");
		Dictionary<ulong, List<ulong>> held = Locate(process, counters.Keys, 8);

		var node = new byte[80];
		var heap = new byte[256];
		// Two node shapes are possible and which one a registry uses is not knowable in advance: a
		// sorted map keeps its links, a colour and a marker before the key, a hashed one keeps two
		// links and then the key. Both are tried and the entries say which is in use.
		int[] keyPlaces = [16, 32];
		var agreement = new Dictionary<(ulong Lead, int Key, int Value), int>();
		var examples = new Dictionary<(ulong Lead, int Key, int Value), ulong>();

		// What actually holds a block's counter, shown rather than assumed. The distance the
		// counters agree on next after the items' is taken as the blocks', and a couple of their
		// holders are described word by word so the shape of whatever keeps them can be read off.
		if (Environment.GetEnvironmentVariable("BDSEXTRACT_SHOW_HOLDERS") is not null)
		{
			// Driven from blocks the sweep already proves on this build, so the question is about
			// what holds them rather than about which distance the blocks are at.
			List<BlockProperties> proven = BlockRegistry.Read(process);
			var known = new HashSet<ulong>(proven.Select(b => b.Address));
			int withCounter = counters.Values.Distinct().Count(known.Contains);
			Console.WriteLine($"blocks the sweep proves: {proven.Count:N0}, of which {withCounter:N0} are held by a counter");
			var around = new byte[192];
			var probe = new byte[256];
			int shown = 0;
			foreach ((ulong counter, ulong start) in counters)
			{
				if (!known.Contains(start) || counter == start) continue;
				if (!held.TryGetValue(counter, out List<ulong> where) || where.Count == 0) continue;
				foreach (ulong place in where.Take(2))
				{
					Console.WriteLine($"  a block at 0x{start:X}, counter 0x{counter:X}, held at 0x{place:X}");
					if (place < 96 || !process.TryRead(place - 96, around, around.Length)) continue;
					for (int offset = 0; offset + 8 <= around.Length; offset += 8)
					{
						ulong value = BitConverter.ToUInt64(around, offset);
						string tag = value == counter ? "   <- the counter" : "";
						string text = offset + HashedString.Size <= around.Length
									&& HashedString.ReadVerified(process, around, offset, probe) is { } n
							? $"   a name, \"{n}\"" : "";
						if (value == 0 && text.Length == 0 && tag.Length == 0) continue;
						Console.WriteLine($"     {offset - 96,+4}: 0x{value:X}{tag}{text}");
					}
				}
				if (++shown >= 2) break;
			}
		}
		foreach ((ulong counter, List<ulong> places) in held)
		{
			ulong start = counters[counter];
			foreach (ulong place in places)
			{
				// The entry begins some way in front of where its value sits. Every distance is
				// tried, and the key has to be a name this very object carries for one to count.
				for (int value = 40; value <= 104; value += 8)
				{
					if (place < (ulong) value) continue;
					ulong entry = place - (ulong) value;
					if (!process.TryRead(entry, node, node.Length)) continue;

					foreach (int keyPlace in keyPlaces)
					{
						if (keyPlace + HashedString.Size > value) continue;
						if (HashedString.ReadVerified(process, node, keyPlace, heap) is not { } key) continue;

						foreach ((ulong nameAt, string text) in leads[start])
						{
							if (text != key) continue;
							ulong lead = nameAt - start;
							agreement[(lead, keyPlace, value)] = agreement.GetValueOrDefault((lead, keyPlace, value)) + 1;
							examples.TryAdd((lead, keyPlace, value), entry);
						}
					}
				}
			}
		}

		if (agreement.Count == 0)
		{
			Console.Error.WriteLine("no map entry files a named object under its own name");
			report.Append("\t\"blockRegistry\": null,\n");
			return 1;
		}

		List<KeyValuePair<(ulong Lead, int Key, int Value), int>> ranked = agreement.OrderByDescending(a => a.Value).ToList();
		Console.WriteLine("entries filing an object under its own name:");
		foreach (KeyValuePair<(ulong Lead, int Key, int Value), int> row in ranked.Take(6))
		{
			Console.WriteLine($"    {row.Value,6} entries: name at object+{row.Key.Lead}, key at entry+{row.Key.Key}, pointer at entry+{row.Key.Value}");
		}

		((ulong settled, int keyAt, int pointerAt), int agreed) = (ranked[0].Key, ranked[0].Value);
		Console.WriteLine($"a block's name sits {settled} bytes into it, and its entry keeps the pointer at +{pointerAt}, agreed by {agreed:N0} entries");

		// The whole map, walked from one entry: up to the head the tree hangs from, then down over
		// every node. What the map holds is the registry's membership, which is the answer a sweep
		// can only approximate.
		List<(string Name, ulong Block)> members = WalkTree(process, examples[(settled, keyAt, pointerAt)], keyAt, pointerAt);
		Console.WriteLine($"the registry holds {members.Count:N0} blocks");

		report.Append("\t\"blockRegistry\": {\n");
		report.Append($"\t\t\"nameInsideBlock\": {settled},\n");
		report.Append($"\t\t\"pointerInsideEntry\": {pointerAt},\n");
		report.Append($"\t\t\"entriesAgreeing\": {agreed},\n");
		report.Append($"\t\t\"blocks\": {members.Count},\n");
		report.Append($"\t\t\"names\": [{string.Join(", ", members.Take(4096).Select(m => $"\"{m.Name}\""))}]\n");
		report.Append("\t},\n");
		return members.Count > 0 ? 0 : 1;
	}

	/// <summary>Where a map entry keeps its key, past the links and the colour.</summary>
	private const int MapEntryKey = 32;

	/// <summary>
	///     The three numbers the block sweep has to be told, measured instead of assumed: how far a
	///     block's name sits inside it, where the block keeps the pointer to its default state, and
	///     where that state keeps the pointer back.
	///     <para>
	///         Nothing is searched for blindly. A block and its default state point at each other,
	///         and a loop that closes is not something arbitrary bytes do: follow a pointer out of a
	///         verified name, follow one back, and see whether it lands just in front of that same
	///         name. Every combination of the two offsets is tried on a sample of names, and the one
	///         that closes the loop for hundreds of them is the build's own layout.
	///     </para>
	/// </summary>
	public static (ulong Lead, int StateAt, int BackAt, int Agreed) DeriveBlockLayout(
		BedrockProcess process, List<(ulong At, string Name)> names, int sample = 400)
	{
		var ahead = new byte[512];
		var state = new byte[320];
		var counts = new Dictionary<(ulong, int, int), int>();

		// Spread across the whole set rather than taken from the front: names lie in memory grouped
		// by what made them, so the first few hundred are all one kind of thing and say nothing
		// about the rest.
		int step = Math.Max(1, names.Count / sample);
		for (int index = 0; index < names.Count; index += step)
		{
			ulong at = names[index].At;
			if (process.ReadClipped(at, ahead, ahead.Length) < ahead.Length) continue;
			for (int stateAt = 0; stateAt + 8 <= ahead.Length; stateAt += 8)
			{
				ulong stateAddress = BitConverter.ToUInt64(ahead, stateAt);
				if (stateAddress < 0x10000 || !process.IsMapped(stateAddress)) continue;
				if (process.ReadClipped(stateAddress, state, state.Length) < state.Length) continue;

				for (int backAt = 0; backAt + 8 <= state.Length; backAt += 8)
				{
					ulong back = BitConverter.ToUInt64(state, backAt);
					if (back >= at || at - back > MaximumLead) continue;
					counts[(at - back, stateAt, backAt)] = counts.GetValueOrDefault((at - back, stateAt, backAt)) + 1;
				}
			}
		}

		if (counts.Count == 0) return (0, 0, 0, 0);
		List<KeyValuePair<(ulong Lead, int StateAt, int BackAt), int>> ranked =
			counts.OrderByDescending(c => c.Value).ToList();
		foreach (KeyValuePair<(ulong Lead, int StateAt, int BackAt), int> row in ranked.Take(5))
		{
			Console.WriteLine($"    {row.Value,5} round trips: name at object+{row.Key.Lead}, "
							+ $"state pointer at name+{row.Key.StateAt}, back pointer at state+{row.Key.BackAt}");
		}

		KeyValuePair<(ulong Lead, int StateAt, int BackAt), int> best = ranked[0];
		return (best.Key.Lead, best.Key.StateAt, best.Key.BackAt, best.Value);
	}

	/// <summary>
	///     Every entry of the tree one node belongs to. The way up is the parent link, which ends at
	///     the head the tree hangs from; the head marks itself, so the climb knows where to stop
	///     without being told how deep it started.
	/// </summary>
	private static List<(string Name, ulong Block)> WalkTree(BedrockProcess process, ulong from, int keyAt, int pointerAt)
	{
		var word = new byte[8];
		var node = new byte[80];
		var heap = new byte[256];

		ulong head = from;
		for (int climb = 0; climb < 64; climb++)
		{
			ulong parent = process.ReadUInt64(head + 8, word);
			if (parent < 0x10000 || !process.IsMapped(parent)) break;
			if (!process.TryRead(parent, node, node.Length)) break;
			if (node[25] != 0) { head = parent; break; }
			head = parent;
		}

		var members = new List<(string, ulong)>();
		var seen = new HashSet<ulong>();
		var pending = new Stack<ulong>();
		pending.Push(process.ReadUInt64(head + 8, word));
		while (pending.Count > 0 && members.Count < 100_000)
		{
			ulong at = pending.Pop();
			if (at < 0x10000 || at == head || !seen.Add(at) || !process.IsMapped(at)) continue;
			if (!process.TryRead(at, node, node.Length)) continue;
			if (HashedString.ReadVerified(process, node, keyAt, heap) is { } name)
			{
				ulong counter = BitConverter.ToUInt64(node, pointerAt);
				ulong block = counter >= 0x10000 ? process.ReadUInt64(counter, word) : 0;
				members.Add((name, block));
			}
			pending.Push(BitConverter.ToUInt64(node, 0));
			pending.Push(BitConverter.ToUInt64(node, 16));
		}

		members.Sort((a, b) => string.CompareOrdinal(a.Item1, b.Item1));
		return members;
	}

	/// <summary>
	///     Every namespaced name in the process that hashes to its own text. This is the one thing
	///     that can be found without knowing any layout, which is why everything else starts here.
	/// </summary>
	internal static List<(ulong At, string Name)> NamedThings(BedrockProcess process)
	{
		var heap = new byte[256];
		var found = new List<(ulong, string)>();
		var scan = new byte[8 * 1024 * 1024];

		foreach (BedrockProcess.Region region in process.Regions)
		{
			for (ulong at = region.Base; at < region.End;)
			{
				int length = (int) Math.Min((ulong) scan.Length, region.End - at);
				if (length >= HashedString.Size && process.TryRead(at, scan, length))
				{
					for (int i = 0; i + HashedString.Size <= length; i += 8)
					{
						string name = HashedString.ReadVerified(process, scan, i, heap);
						if (name is null || !name.Contains(':')) continue;
						found.Add((at + (ulong) i, name));
					}
				}
				at += (ulong) length;
			}
		}
		return found;
	}

	/// <summary>
	///     Where a block keeps the table of its own states, measured rather than assumed.
	///     <para>
	///         The table is a vector of pointers, and every state it holds points back at the block
	///         holding it. So a candidate position is tried by reading a vector there and asking
	///         each of its entries whose block it belongs to: the position where a thousand blocks
	///         all get their own states back is the table, and no other position answers that.
	///         Empty slots are legitimate, the table having a place for every value four bits can
	///         carry, so only the filled ones are asked and a table with none of them fails.
	///     </para>
	/// </summary>
	public static (int At, int Agreed) DeriveStateTable(BedrockProcess process, IReadOnlyList<BlockProperties> blocks, int reach = 1024)
	{
		var word = new byte[8];
		var counts = new Dictionary<int, int>();

		foreach (BlockProperties block in blocks)
		{
			for (int at = 0; at + 16 <= reach; at += 8)
			{
				ulong begin = process.ReadUInt64(block.Address + (ulong) at, word);
				ulong end = process.ReadUInt64(block.Address + (ulong) (at + 8), word);
				if (begin < 0x10000 || end <= begin || (end - begin) % 8 != 0) continue;
				if (end - begin > 1 << 16 || !process.IsMapped(begin)) continue;

				bool wrong = false;
				int filled = 0;
				for (ulong slot = begin; slot < end && !wrong; slot += 8)
				{
					ulong state = process.ReadUInt64(slot, word);
					if (state < 0x10000 || !process.IsMapped(state)) continue;
					if (process.ReadUInt64(state + (ulong) MemoryLayout.BlockLegacyPointer, word) == block.Address) filled++;
					else wrong = true;
				}

				if (!wrong && filled > 0) counts[at] = counts.GetValueOrDefault(at) + 1;
			}
		}

		if (counts.Count == 0) return (-1, 0);
		KeyValuePair<int, int> best = counts.OrderByDescending(c => c.Value).First();
		return (best.Key, best.Value);
	}

	/// <summary>
	///     Where a state keeps the map describing what it is, measured rather than assumed.
	///     <para>
	///         The map is keyed by text, and one of its keys is "name". So a candidate position is
	///         tried by reading a map there and looking for that key: the position where a thousand
	///         states all have it is the map, and a wrong position has no key at all.
	///     </para>
	/// </summary>
	public static (int At, int Agreed) DeriveStateNbt(BedrockProcess process, IReadOnlyList<BlockProperties> blocks, int reach = 512)
	{
		var word = new byte[8];
		var node = new byte[MemoryLayout.MapNodeSize];
		var text = new byte[256];
		var counts = new Dictionary<int, int>();

		foreach (BlockProperties block in blocks)
		{
			ulong state = process.ReadUInt64(
				block.Address + (ulong) (MemoryLayout.NameInsideLegacy + MemoryLayout.DefaultStatePointer), word);
			if (state < 0x10000 || !process.IsMapped(state)) continue;

			for (int at = 0; at + 8 <= reach; at += 8)
			{
				ulong map = process.ReadUInt64(state + (ulong) at, word);
				if (map < 0x10000 || !process.IsMapped(map)) continue;

				ulong root = process.ReadUInt64(map + (ulong) MemoryLayout.MapNodeParent, word);
				if (root < 0x10000 || !process.IsMapped(root)) continue;

				var seen = new HashSet<ulong>();
				var pending = new Stack<ulong>();
				pending.Push(root);
				bool named = false;
				while (pending.Count > 0 && seen.Count < 64 && !named)
				{
					ulong entry = pending.Pop();
					if (entry < 0x10000 || entry == map || !seen.Add(entry) || !process.IsMapped(entry)) continue;
					if (!process.TryRead(entry, node, node.Length)) continue;
					if (node[MemoryLayout.MapNodeFlags + 1] == 0
						&& StdString(process, entry + (ulong) MemoryLayout.MapNodeKey, text) == "name")
					{
						named = true;
					}
					pending.Push(BitConverter.ToUInt64(node, MemoryLayout.MapNodeLeft));
					pending.Push(BitConverter.ToUInt64(node, MemoryLayout.MapNodeRight));
				}

				if (named) counts[at] = counts.GetValueOrDefault(at) + 1;
			}
		}

		if (counts.Count == 0) return (-1, 0);
		KeyValuePair<int, int> best = counts.OrderByDescending(c => c.Value).First();
		return (best.Key, best.Value);
	}

	/// <summary>An MSVC std::string at an address: sixteen bytes of union, then length and capacity.</summary>
	private static string StdString(BedrockProcess process, ulong at, byte[] scratch)
	{
		var header = new byte[32];
		if (!process.TryRead(at, header, header.Length)) return null;
		ulong length = BitConverter.ToUInt64(header, 16);
		ulong capacity = BitConverter.ToUInt64(header, 24);
		if (length == 0 || length > capacity || length >= (ulong) scratch.Length) return null;
		if (capacity < 16) return Encoding.ASCII.GetString(header, 0, (int) length);
		ulong pointer = BitConverter.ToUInt64(header, 0);
		if (pointer < 0x10000 || !process.TryRead(pointer, scratch, (int) length)) return null;
		return Encoding.ASCII.GetString(scratch, 0, (int) length);
	}

	/// <summary>
	///     The block upgrade context, from the rules outward. The rules the sweep proves are held by
	///     one vector of owning pointers, and the object in front of that vector is the context that
	///     owns them. What the vector holds is the rule list; what the sweep found and the vector
	///     does not hold is not a rule, whatever it looked like.
	/// </summary>
	private static int FindUpgradeContext(BedrockProcess process, StringBuilder report)
	{
		Console.WriteLine();
		List<UpgradeSchema> schemas = BlockUpgradeReader.Read(process);
		var rules = schemas.Where(s => s.IsRule && s.Address > 0x10000).Select(s => s.Address).ToHashSet();
		Console.WriteLine($"records swept: {schemas.Count:N0}, of which {rules.Count:N0} read as rules");
		report.Append($"\t\"upgradeRecordsSwept\": {schemas.Count},\n");
		report.Append($"\t\"upgradeRecordsAsRules\": {rules.Count},\n");

		if (rules.Count == 0)
		{
			report.Append("\t\"blockUpgradeContext\": null,\n");
			return 1;
		}

		Console.WriteLine("scanning for runs of pointers to proven rules");
		Dictionary<ulong, List<ulong>> holders = Locate(process, rules, 16);
		List<SlotRun> runs = Runs(holders.SelectMany(h => h.Value));
		if (runs.Count == 0)
		{
			Console.Error.WriteLine("no run of rule pointers, so nothing holds the rules as a list");
			report.Append("\t\"blockUpgradeContext\": null,\n");
			return 1;
		}

		Console.WriteLine($"runs of rule pointers: {runs.Count} (largest {runs.Max(r => r.Count):N0} slots)");
		var word = new byte[8];
		foreach (SlotRun run in runs.OrderByDescending(r => r.Count))
		{
			ulong last = run.Begin + (ulong) (run.Count - 1) * 8;
			foreach (Vector vector in Headers(process, run.Begin, last, run.Count))
			{
				// Every slot has to hold something the process actually has, and the rules already
				// proven have to be among them. What the first word of an updater is stays a
				// reading rather than a requirement: it is reported as an image offset, which a
				// build shipping symbols can name, instead of being assumed to be a method table.
				int mapped = 0, proven = 0;
				var members = new List<ulong>();
				var firstWords = new Dictionary<long, int>();
				for (int i = 0; i < vector.Count; i++)
				{
					ulong updater = process.ReadUInt64(vector.Begin + (ulong) i * 8, word);
					members.Add(updater);
					if (updater > 0x10000 && process.IsMapped(updater)) mapped++;
					if (rules.Contains(updater)) proven++;

					ulong head = process.ReadUInt64(updater, word);
					long rva = head > process.ModuleBase ? (long) (head - process.ModuleBase) : -1;
					firstWords[rva] = firstWords.GetValueOrDefault(rva) + 1;
				}
				int objects = mapped;

				// The context keeps the schema version in front of the vector, one byte of it.
				ulong context = vector.At - 8;
				byte version = process.TryRead(context, word, 1) ? word[0] : (byte) 0;

				Console.WriteLine($"  header 0x{vector.At:X} declares {vector.Count:N0} slots, {mapped:N0} are mapped, "
								+ $"{proven:N0} are proven rules, byte in front is {version}");
				Console.WriteLine($"    first word of each: {string.Join(", ", firstWords.OrderByDescending(f => f.Value).Take(4)
					.Select(f => f.Key < 0 ? $"outside the image on {f.Value}" : $"image+0x{f.Key:X} on {f.Value}"))}");
				if (mapped != vector.Count || proven < 8) continue;

				report.Append("\t\"blockUpgradeContext\": {\n");
				report.Append($"\t\t\"context\": \"0x{context:X}\",\n");
				report.Append($"\t\t\"updaterVersion\": {version},\n");
				report.Append($"\t\t\"moduleBase\": \"0x{process.ModuleBase:X}\",\n");
				report.Append($"\t\t\"firstWordByImageOffset\": [{string.Join(", ", firstWords.OrderByDescending(f => f.Value)
					.Select(f => $"{{ \"rva\": {f.Key}, \"members\": {f.Value} }}"))}],\n");
				report.Append($"\t\t\"updaters\": {vector.Count},\n");
				report.Append($"\t\t\"provenBySweep\": {proven},\n");
				report.Append($"\t\t\"sweptNotHeld\": {rules.Count(r => !members.Contains(r))},\n");

				// Who points at the context. A pointer to it from inside the server's own image is
				// a static variable, and its offset into the image is what a symbol file names, so
				// a build shipping symbols can say whether this is the object it calls the block
				// updater's context.
				List<ulong> pointers = Locate(process, [context], 64).GetValueOrDefault(context) ?? [];
				List<long> statics = pointers.Where(p => p > process.ModuleBase && p - process.ModuleBase < 0x8000000)
					.Select(p => (long) (p - process.ModuleBase)).ToList();
				Console.WriteLine($"    pointed at from {pointers.Count} places, {statics.Count} inside the image"
								+ (statics.Count > 0 ? $" at {string.Join(", ", statics.Select(s => $"image+0x{s:X}"))}" : ""));
				report.Append($"\t\t\"pointedAtFrom\": {pointers.Count},\n");
				report.Append($"\t\t\"staticsByImageOffset\": [{string.Join(", ", statics)}],\n");
				report.Append($"\t\t\"members\": [{string.Join(", ", members.Select(m => $"\"0x{m:X}\""))}]\n");
				report.Append("\t},\n");
				Console.WriteLine($"  the context holds {vector.Count:N0} updaters; the sweep proved {proven:N0} of them "
								+ $"and found {rules.Count(r => !members.Contains(r)):N0} rule-shaped records it does not hold");
				return 0;
			}
		}

		Console.Error.WriteLine("no vector of rule pointers is headed by an object");
		report.Append("\t\"blockUpgradeContext\": null,\n");
		return 1;
	}

	/// <summary>
	///     Whether a map's entries lead into a given set of pointers, and at what distance into a
	///     node that pointer sits. The distance is not assumed: every position in the node is tried
	///     across every entry, and the one the whole list agrees on is the value's place. A map of
	///     something else has no position where its entries all point into this set.
	/// </summary>
	private static (int Nodes, int Matched, int ValueAt) MapPointsInto(BedrockProcess process, ulong map, HashSet<ulong> pointers)
	{
		var word = new byte[8];
		var node = new byte[128];
		var hits = new Dictionary<int, int>();
		ulong head = process.ReadUInt64(map + MapListHead, word);
		if (head < 0x10000 || !process.IsMapped(head)) return (0, 0, -1);

		int nodes = 0;
		ulong at = process.ReadUInt64(head, word);
		while (at >= 0x10000 && at != head && nodes < 1_000_000 && process.IsMapped(at))
		{
			nodes++;
			int have = process.ReadClipped(at, node, node.Length);
			for (int offset = NodeValue; offset + 8 <= have; offset += 8)
			{
				if (pointers.Contains(BitConverter.ToUInt64(node, offset))) hits[offset] = hits.GetValueOrDefault(offset) + 1;
			}
			at = process.ReadUInt64(at, word);
		}

		if (hits.Count == 0) return (nodes, 0, -1);
		KeyValuePair<int, int> best = hits.OrderByDescending(h => h.Value).First();
		return (nodes, best.Value, best.Key);
	}

	/// <summary>
	///     The reflection context the registry keeps, which is where component types are registered
	///     by name. The registry holds it as its first member, so it is reached from the object
	///     already proven rather than searched for: every word in front of the item vector is tried,
	///     and a candidate is only accepted when it holds a list of names that read as text.
	///     <para>
	///         The list is a vector of tuples whose first member is a string. How wide a tuple is
	///         depends on types this does not model, so the width is derived: the one that divides
	///         the vector and puts a readable name at every element is the width it has.
	///     </para>
	/// </summary>
	private static void FindReflectionContext(BedrockProcess process, Vector vector, StringBuilder report)
	{
		var word = new byte[8];
		var body = new byte[256];
		var element = new byte[64];
		var scratch = new byte[256];

		for (ulong at = vector.At >= 512 ? vector.At - 512 : 0; at < vector.At; at += 8)
		{
			ulong candidate = process.ReadUInt64(at, word);
			if (candidate < 0x10000 || !process.IsMapped(candidate)) continue;
			if (process.ReadClipped(candidate, body, body.Length) < 128) continue;

			for (int offset = 0; offset + 24 <= body.Length; offset += 8)
			{
				ulong begin = BitConverter.ToUInt64(body, offset);
				ulong end = BitConverter.ToUInt64(body, offset + 8);
				ulong room = BitConverter.ToUInt64(body, offset + 16);
				if (begin < 0x10000 || end <= begin || room < end || end - begin > 1 << 20) continue;
				if (!process.IsMapped(begin)) continue;

				for (int stride = 32; stride <= 128; stride += 8)
				{
					if ((end - begin) % (ulong) stride != 0) continue;
					int count = (int) ((end - begin) / (ulong) stride);
					if (count is < 8 or > 100_000) continue;
					if (!process.TryRead(begin, element, Math.Min(element.Length, stride))) continue;

					// Where the name sits inside an entry is derived too. A tuple is not laid out
					// in the order it is declared, so the name is wherever it reads, and the
					// position that reads on EVERY entry is the one it is at.
					for (int inside = 0; inside + 32 <= stride && inside + 32 <= element.Length; inside += 8)
					{
						if (ItemRegistry.ReadStdString(process, element, inside, scratch) is null or { Length: 0 }) continue;

						var names = new List<string>();
						bool whole = true;
						for (int i = 0; i < count && whole; i++)
						{
							if (!process.TryRead(begin + (ulong) (i * stride), element, Math.Min(element.Length, stride))) { whole = false; break; }
							string name = ItemRegistry.ReadStdString(process, element, inside, scratch);
							if (name is null or { Length: 0 }) whole = false;
							else names.Add(name);
						}
						if (!whole || names.Count < 8) continue;

						Console.WriteLine($"    reflection context at 0x{candidate:X} (held {vector.At - at} bytes in front of the item vector)");
						Console.WriteLine($"      names it knows: {names.Count:N0}, entries {stride} bytes apart, name at entry+{inside}");
						Console.WriteLine($"      such as {string.Join(", ", names.Take(8))}");
						report.Append($"\t\t\"reflectionContext\": \"0x{candidate:X}\",\n");
						report.Append($"\t\t\"reflectionContextHeldAt\": {vector.At - at},\n");
						report.Append($"\t\t\"knownNameCount\": {names.Count},\n");
						report.Append($"\t\t\"knownNameStride\": {stride},\n");
						report.Append($"\t\t\"knownNameInsideEntry\": {inside},\n");
						report.Append($"\t\t\"knownNames\": [{string.Join(", ", names.Select(n => $"\"{n.Replace("\\", "\\\\").Replace("\"", "\\\"")}\""))}],\n");
						return;
					}
				}
			}
		}

		// Nothing accepted, so say what was there instead of only that it failed. Each pointer in
		// front of the vector, with the class it belongs to and how big that class says it is, is
		// what a reader needs to see to know whether the context is absent or merely unrecognised.
		Console.WriteLine("    no reflection context found; the pointers in front of the item vector are:");
		var seen = new List<string>();
		for (ulong at = vector.At >= 512 ? vector.At - 512 : 0; at < vector.At; at += 8)
		{
			ulong candidate = process.ReadUInt64(at, word);
			if (candidate < 0x10000 || !process.IsMapped(candidate)) continue;
			ulong head = process.ReadUInt64(candidate, word);
			int size = head > process.ModuleBase ? ItemRegistry.ClassSize(process, head) : 0;
			string what = head > process.ModuleBase
				? $"object of a class at image+0x{head - process.ModuleBase:X}, {(size > 0 ? size + " bytes" : "size unstated")}"
				: "not an object with a method table";
			Console.WriteLine($"      -{vector.At - at,3}: 0x{candidate:X} {what}");
			seen.Add($"{{ \"behind\": {vector.At - at}, \"at\": \"0x{candidate:X}\", \"what\": \"{what}\" }}");
		}

		report.Append("\t\t\"reflectionContext\": null,\n");
		report.Append($"\t\t\"pointersInFrontOfTheVector\": [{string.Join(", ", seen)}],\n");
	}

	/// <summary>
	///     What a map is keyed by, said in terms of what its keys actually read as. The key lies
	///     between a node's links and its value, so its size is known once the value's place is,
	///     and a key wide enough to be a hashed name is only called one when the text hashes to the
	///     hash beside it on every entry.
	/// </summary>
	private static string DescribeKeys(BedrockProcess process, ulong map, int valueAt)
	{
		var word = new byte[8];
		var node = new byte[128];
		var heap = new byte[256];
		int size = valueAt - NodeValue;
		if (size <= 0) return $"nothing readable, the value sits at node+{valueAt}";

		ulong head = process.ReadUInt64(map + MapListHead, word);
		int nodes = 0, verified = 0;
		long low = long.MaxValue, high = long.MinValue;
		var names = new List<string>();
		var numbers = new HashSet<long>();

		ulong at = process.ReadUInt64(head, word);
		while (at >= 0x10000 && at != head && nodes < 1_000_000 && process.IsMapped(at))
		{
			nodes++;
			int have = process.ReadClipped(at, node, node.Length);
			if (have >= NodeValue + size)
			{
				if (size >= HashedString.Size)
				{
					string name = HashedString.ReadVerified(process, node, NodeValue, heap, 1, 128);
					if (name is not null)
					{
						verified++;
						if (names.Count < 3) names.Add(name);
					}
				}
				else
				{
					long value = BitConverter.ToInt32(node, NodeValue);
					numbers.Add(value);
					low = Math.Min(low, value);
					high = Math.Max(high, value);
				}
			}
			at = process.ReadUInt64(at, word);
		}

		if (size >= HashedString.Size)
		{
			return $"names, {verified} of {nodes} hashing to their own text"
				+ (names.Count > 0 ? $", such as {string.Join(", ", names)}" : "");
		}

		return numbers.Count == 0
			? $"{size} bytes that were not read"
			: $"numbers, {numbers.Count} distinct over {nodes} entries, from {low} to {high}";
	}

	/// <summary>
	///     How many entries a map holds, or -1 when this is not one. The count is not taken on
	///     trust: the node list is walked and has to close back on its own head after exactly that
	///     many nodes, which arbitrary bytes do not do.
	/// </summary>
	private static long MapCount(BedrockProcess process, ulong map)
	{
		var word = new byte[8];
		long stated = (long) process.ReadUInt64(map + MapSize, word);
		if (stated is <= 0 or > 1_000_000) return -1;

		ulong head = process.ReadUInt64(map + MapListHead, word);
		if (head < 0x10000 || !process.IsMapped(head)) return -1;

		long walked = 0;
		ulong node = process.ReadUInt64(head, word);
		while (node >= 0x10000 && node != head && walked <= stated && process.IsMapped(node))
		{
			walked++;
			node = process.ReadUInt64(node, word);
		}

		return node == head && walked == stated ? stated : -1;
	}

	/// <summary>A vector, as its own three words say it: where it begins, where it ends, its room.</summary>
	private sealed record Vector(ulong At, ulong Begin, ulong End)
	{
		public int Count => (int) ((End - Begin) / 8);
	}

	/// <summary>
	///     Where a vector containing this run is headed from: three words saying where it begins,
	///     where it ends, and how far its room reaches, arranged so that the run lies inside it.
	///     <para>
	///         Containing, rather than matching exactly. The run is only the slots whose contents
	///         were recognised, and one member at either edge that a sweep did not reach would leave
	///         a vector that really holds it looking like the wrong vector. What the header itself
	///         declares is taken as the extent from here on, which is the whole point of asking the
	///         container instead of the population.
	///     </para>
	/// </summary>
	private static List<Vector> Headers(BedrockProcess process, ulong first, ulong last, int identified)
	{
		// A vector's end is its size, not its room, so one holding this run is this run's length
		// give or take the few members that were not recognised. Without that bound any pair of
		// words spanning the heap contains the run and calls itself a vector.
		int most = identified + 64;
		var headers = new List<Vector>();
		var scan = new byte[8 * 1024 * 1024];
		foreach (BedrockProcess.Region region in process.Regions)
		{
			for (ulong at = region.Base; at < region.End;)
			{
				int length = (int) Math.Min((ulong) scan.Length, region.End - at);
				if (length >= 24 && process.TryRead(at, scan, length))
				{
					for (int k = 0; k + 24 <= length; k += 8)
					{
						ulong begin = BitConverter.ToUInt64(scan, k);
						if (begin > first || (first - begin) % 8 != 0) continue;
						ulong end = BitConverter.ToUInt64(scan, k + 8);
						if (end <= last || (end - begin) % 8 != 0) continue;
						if ((end - begin) / 8 > (ulong) most) continue;
						if (BitConverter.ToUInt64(scan, k + 16) < end) continue;
						if (!process.IsMapped(begin)) continue;
						headers.Add(new Vector(at + (ulong) k, begin, end));
					}
				}
				at += (ulong) length;
			}
		}

		// The tightest first: a vector that merely spans the run from far away describes something
		// larger that happens to contain it, and the one that fits is the one being looked for.
		return headers.OrderBy(h => h.Count).ToList();
	}

	private sealed record SlotRun(ulong Begin, int Count);

	/// <summary>
	///     The places that hold these addresses, gathered into runs of consecutive slots. A vector
	///     of pointers is such a run; a scattering of single hits is not one and stays out.
	/// </summary>
	private static List<SlotRun> Runs(IEnumerable<ulong> locations)
	{
		List<ulong> sorted = locations.Distinct().OrderBy(x => x).ToList();
		var runs = new List<SlotRun>();
		int i = 0;
		while (i < sorted.Count)
		{
			// A gap of a slot or two is a member whose contents were not recognised, not the end of
			// the array. Bridging them keeps one unrecognised member from splitting a registry in
			// half; the extent that counts is the one the vector's own header declares anyway.
			int j = i;
			while (j + 1 < sorted.Count && sorted[j + 1] - sorted[j] <= 32 && (sorted[j + 1] - sorted[j]) % 8 == 0) j++;
			int count = j - i + 1;
			if (count >= 8) runs.Add(new SlotRun(sorted[i], count));
			i = j + 1;
		}
		return runs;
	}

	/// <summary>
	///     Every place in the process holding one of these addresses. One pass over memory, because
	///     the answer is a property of the whole address space rather than of any region.
	/// </summary>
	internal static Dictionary<ulong, List<ulong>> Locate(BedrockProcess process, IReadOnlyCollection<ulong> targets, int cap)
	{
		var wanted = new HashSet<ulong>(targets);
		ulong low = targets.Min(), high = targets.Max();
		var found = new Dictionary<ulong, List<ulong>>();
		var scan = new byte[8 * 1024 * 1024];

		foreach (BedrockProcess.Region region in process.Regions)
		{
			for (ulong at = region.Base; at < region.End;)
			{
				int length = (int) Math.Min((ulong) scan.Length, region.End - at);
				if (length >= 8 && process.TryRead(at, scan, length))
				{
					for (int k = 0; k + 8 <= length; k += 8)
					{
						ulong value = BitConverter.ToUInt64(scan, k);
						if (value < low || value > high || !wanted.Contains(value)) continue;
						if (!found.TryGetValue(value, out List<ulong> places)) found[value] = places = [];
						if (places.Count < cap) places.Add(at + (ulong) k);
					}
				}
				at += (ulong) length;
			}
		}
		return found;
	}

	/// <summary>
	///     Where a block keeps its property table, measured rather than assumed. The table is
	///     sixteen buckets of pointers, and the nodes behind them each open with a HashedString
	///     that hashes to its own text. A wrong position produces buckets that lead to nothing that
	///     verifies, so the position where the most blocks yield a property is the table.
	/// </summary>
	public static (int At, int Agreed) DerivePropertyTable(BedrockProcess process,
		IReadOnlyList<BlockProperties> blocks, int reach = 1024)
	{
		const int buckets = 16;
		const int nodeName = 16;
		var word = new byte[8];
		var header = new byte[HashedString.Size];
		var heap = new byte[256];
		var counts = new Dictionary<int, int>();

		foreach (BlockProperties block in blocks)
		{
			for (int at = 0; at + 16 <= reach; at += 8)
			{
				ulong begin = process.ReadUInt64(block.Address + (ulong) at, word);
				ulong end = process.ReadUInt64(block.Address + (ulong) (at + 8), word);
				if (begin < 0x10000 || end <= begin || (end - begin) / 8 != buckets) continue;
				if (!process.IsMapped(begin)) continue;

				bool named = false;
				for (int i = 0; i < buckets && !named; i++)
				{
					ulong node = process.ReadUInt64(begin + (ulong) (i * 8), word);
					if (node < 0x10000 || !process.IsMapped(node)) continue;
					if (!process.TryRead(node + nodeName, header, header.Length)) continue;
					named = HashedString.ReadVerified(process, header, 0, heap,
						MemoryLayout.MinimumTagLength, MemoryLayout.MaximumTagLength) is not null;
				}

				if (named) counts[at] = counts.GetValueOrDefault(at) + 1;
			}
		}

		if (counts.Count == 0) return (-1, 0);
		KeyValuePair<int, int> best = counts.OrderByDescending(c => c.Value).First();
		return (best.Key, best.Value);
	}

	/// <summary>
	///     Where a block keeps its components and their ids. Two vectors that run in step: pointers
	///     to the components, and one sixteen bit id each. Requiring the two to hold the same count
	///     is what separates them from every other vector in the object.
	/// </summary>
	public static (int At, int IdsAt, int Agreed) DeriveComponents(BedrockProcess process,
		IReadOnlyList<BlockProperties> blocks, int reach = 1024)
	{
		var word = new byte[8];
		var counts = new Dictionary<(int At, int IdsAt), int>();

		foreach (BlockProperties block in blocks)
		{
			foreach ((int at, long count) in Vectors(process, block.Address, reach, 8, word))
			{
				if (count is 0 or > 64) continue;
				foreach ((int idsAt, long ids) in Vectors(process, block.Address, reach, 2, word))
				{
					if (idsAt == at || ids != count) continue;
					counts[(at, idsAt)] = counts.GetValueOrDefault((at, idsAt)) + 1;
				}
			}
		}

		if (counts.Count == 0) return (-1, -1, 0);
		KeyValuePair<(int At, int IdsAt), int> best = counts.OrderByDescending(c => c.Value).First();
		return (best.Key.At, best.Key.IdsAt, best.Value);
	}

	/// <summary>
	///     Where a block keeps its tags, and how wide one tag record is. A tag opens with a
	///     HashedString, so a candidate is a vector whose span divides by the stride and whose every
	///     record hashes to its own text.
	/// </summary>
	public static (int At, int Stride, int Agreed) DeriveTags(BedrockProcess process,
		IReadOnlyList<BlockProperties> blocks, int reach = 512)
	{
		int[] strides = [16, 24, 32, 40, 48, 56, 64];
		var word = new byte[8];
		var element = new byte[HashedString.Size];
		var heap = new byte[MemoryLayout.MaximumTagLength];
		var counts = new Dictionary<(int At, int Stride), int>();

		foreach (BlockProperties block in blocks)
		{
			ulong name = block.Address + (ulong) MemoryLayout.NameInsideLegacy;
			for (int at = 0; at + 16 <= reach; at += 8)
			{
				ulong begin = process.ReadUInt64(name + (ulong) at, word);
				ulong end = process.ReadUInt64(name + (ulong) (at + 8), word);
				if (begin < 0x10000 || end <= begin || !process.IsMapped(begin)) continue;
				ulong span = end - begin;

				foreach (int stride in strides)
				{
					if (span % (ulong) stride != 0 || span / (ulong) stride > 64) continue;

					bool all = true;
					for (ulong record = begin; record < end && all; record += (ulong) stride)
					{
						all = process.TryRead(record, element, element.Length)
							&& HashedString.ReadVerified(process, element, 0, heap,
								MemoryLayout.MinimumTagLength, MemoryLayout.MaximumTagLength) is not null;
					}

					if (all) counts[(at, stride)] = counts.GetValueOrDefault((at, stride)) + 1;
				}
			}
		}

		if (counts.Count == 0) return (-1, -1, 0);
		// The widest stride that still divides every span, because a record twice the width also
		// divides and would read every second tag.
		int most = counts.Values.Max();
		KeyValuePair<(int At, int Stride), int> best = counts
			.Where(c => c.Value == most).OrderBy(c => c.Key.Stride).First();
		return (best.Key.At, best.Key.Stride, best.Value);
	}

	/// <summary>Every position in the object holding a vector of fixed width elements, with its count.</summary>
	private static IEnumerable<(int At, long Count)> Vectors(BedrockProcess process, ulong address,
		int reach, int width, byte[] word)
	{
		for (int at = 0; at + 16 <= reach; at += 8)
		{
			ulong begin = process.ReadUInt64(address + (ulong) at, word);
			ulong end = process.ReadUInt64(address + (ulong) (at + 8), word);
			if (begin < 0x10000 || end < begin || !process.IsMapped(begin)) continue;
			ulong span = end - begin;
			if (span % (ulong) width != 0 || span > 1 << 16) continue;
			yield return (at, (long) (span / (ulong) width));
		}
	}

	/// <summary>
	///     Where a state keeps a byte whose value is known for every state, measured by trying every
	///     position over the whole palette. The floor is the same nine tenths the block fields use,
	///     so a build where the game changed the value on a handful of states still locates it.
	/// </summary>
	public static (int At, int Agreed, int Population) DeriveStateByte(BedrockProcess process,
		IReadOnlyList<(ulong Address, int Value)> wanted, int reach = 384)
	{
		if (wanted.Count == 0) return (-1, 0, 0);

		var window = new byte[reach];
		var held = new int[reach];
		int population = 0;

		foreach ((ulong address, int value) in wanted)
		{
			if (process.ReadClipped(address, window, reach) < reach) continue;
			population++;
			for (int at = 0; at < reach; at++)
			{
				if (window[at] == value) held[at]++;
			}
		}

		if (population == 0) return (-1, 0, 0);
		int best = -1;
		for (int at = 0; at < reach; at++)
		{
			if (held[at] >= Sentinels.Floor * population && (best < 0 || held[at] > held[best])) best = at;
		}
		return best < 0 ? (-1, 0, population) : (best, held[best], population);
	}
}
