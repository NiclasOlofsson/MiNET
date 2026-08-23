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

	/// <summary>
	///     Where a block keeps its name, its default state and the state's way back, measured from
	///     the objects rather than searched for in a window.
	///     <para>
	///         A name is found the way every name is found, by hashing to its own text. From there
	///         the object start is walked back to: the first word behind the name that is a method
	///         table whose class is big enough to contain the name at that distance. That word is
	///         the start, the class states the size, and both are facts rather than candidates.
	///     </para>
	///     <para>
	///         The state pointer is then looked for across the whole object, because the class said
	///         how long it is. This used to read a fixed 512 bytes past the name and vote on
	///         triples, which is two invented numbers deciding every offset downstream.
	///     </para>
	/// </summary>
	public static (ulong Lead, int StateAt, int BackAt, int Agreed) DeriveBlockLayout(
		BedrockProcess process, List<(ulong At, string Name)> names, int sample = 400)
	{
		var word = new byte[8];
		var counts = new Dictionary<(ulong, int, int), int>();

		// Spread across the whole set rather than taken from the front: names lie in memory grouped
		// by what made them, so the first few hundred are all one kind of thing.
		int step = Math.Max(1, names.Count / sample);
		for (int index = 0; index < names.Count; index += step)
		{
			ulong name = names[index].At;
			(ulong start, int size) = ObjectAround(process, name, word);
			if (size == 0) continue;

			var whole = new byte[size];
			if (process.ReadClipped(start, whole, size) < size) continue;

			ulong lead = name - start;
			for (int stateAt = 0; stateAt + 8 <= size; stateAt += 8)
			{
				ulong state = BitConverter.ToUInt64(whole, stateAt);
				if (state < 0x10000 || !process.IsMapped(state)) continue;

				(ulong stateStart, int stateSize) = (state, ObjectLayout.Measure(process, state, word));
				if (stateSize == 0) continue;

				var inner = new byte[stateSize];
				if (process.ReadClipped(stateStart, inner, stateSize) < stateSize) continue;

				for (int backAt = 0; backAt + 8 <= stateSize; backAt += 8)
				{
					if (BitConverter.ToUInt64(inner, backAt) != start) continue;
					counts[(lead, stateAt - (int) lead, backAt)] =
						counts.GetValueOrDefault((lead, stateAt - (int) lead, backAt)) + 1;
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
	///     The object a name sits inside: its start and its size.
	///     <para>
	///         Walked back word by word to the first method table whose class states a size large
	///         enough to reach the name. A polymorphic object opens with that pointer and the size
	///         is the one the compiler wrote into the destructor, so neither is a guess. Zero size
	///         means no object was found behind the name, which is a name that is not in one.
	///     </para>
	/// </summary>
	private static (ulong Start, int Size) ObjectAround(BedrockProcess process, ulong name, byte[] word)
	{
		for (ulong back = 0; back <= MaximumLead; back += 8)
		{
			if (back > name) break;
			ulong start = name - back;
			int size = ObjectLayout.Measure(process, start, word);
			if (size > 0 && size >= (int) back + HashedString.Size) return (start, size);
		}

		return (0, 0);
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

	/// <summary>A vector, as its own three words say it: where it begins, where it ends, its room.</summary>
	private sealed record Vector(ulong At, ulong Begin, ulong End)
	{
		public int Count => (int) ((End - Begin) / 8);
	}

	private sealed record SlotRun(ulong Begin, int Count);

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
