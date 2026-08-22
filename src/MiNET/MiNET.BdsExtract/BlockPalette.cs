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

/// <summary>One entry of the block palette. Its position in the list is the palette index.</summary>
public sealed class PaletteEntry
{
	public int Index { get; init; }
	public string Name { get; init; }
	public ulong NameHash { get; init; }
	public uint NetworkId { get; init; }
	public int LegacyId { get; init; }
	public int LightEmission { get; init; }
	public int LightDampening { get; init; }

	/// <summary>What this state is, sorted by property name. Empty for a block with one state.</summary>
	public IReadOnlyList<StateProperty> States { get; init; } = [];

	/// <summary>The block state version stamped into the state's own NBT.</summary>
	public int Version { get; init; }
}

/// <summary>
///     Reads the block palette out of a running server, in the server's own order.
///     The order is the point, so it must not be reconstructed. The server keeps the palette in a
///     vector, and a vector in memory is three pointers: where it begins, where it ends, and how
///     much room it has. Finding that header settles where the palette starts, because that is
///     what "begins" means. Nothing is sorted and no threshold decides where slot zero is.
///     The header is recognised by what it points at: every entry between begin and end has to be
///     one of the block-state objects found beforehand. No other vector in the process satisfies
///     that, and its length gives the entry count directly.
/// </summary>
public static class BlockPalette
{
	/// <summary>A candidate std::vector, and how many entries it holds.</summary>
	public readonly record struct VectorHeader(ulong Begin, ulong End, ulong Capacity, int Count);

	private const int MinimumPaletteSize = 4096;
	private const int MaximumPaletteSize = 200_000;
	private const int SpanSamples = 32;

	/// <summary>
	///     Every block-state object in the process. Each points back at the block it is a state of,
	///     and they are laid out identically, so the one shape they share identifies them.
	/// </summary>
	public static HashSet<ulong> FindStateObjects(BedrockProcess process, IEnumerable<BlockProperties> blocks)
	{
		var owners = blocks.Select(b => b.Address).ToHashSet();
		var pointers = FindPointersTo(process, owners);

		// Objects of one class share a first field, the pointer to their method table. Take the
		// value that most candidates agree on and keep only those, which drops coincidences.
		var word = new byte[8];
		var tally = new Dictionary<ulong, int>();
		foreach (ulong site in pointers)
		{
			ulong start = site - (ulong) MemoryLayout.BlockLegacyPointer;
			ulong table = process.ReadUInt64(start, word);
			if (table > 0x10000) tally[table] = tally.GetValueOrDefault(table) + 1;
		}
		var accepted = tally.Where(t => t.Value >= 8).Select(t => t.Key).ToHashSet();

		var states = new HashSet<ulong>();
		foreach (ulong site in pointers)
		{
			ulong start = site - (ulong) MemoryLayout.BlockLegacyPointer;
			if (accepted.Contains(process.ReadUInt64(start, word))) states.Add(start);
		}
		return states;
	}

	/// <summary>Vectors whose whole contents are block-state objects. The palette is the largest.</summary>
	public static List<VectorHeader> FindVectors(BedrockProcess process, HashSet<ulong> states)
	{
		var headers = new List<VectorHeader>();
		var window = new byte[8 * 1024 * 1024];
		var word = new byte[8];

		foreach (var region in process.Regions)
		{
			for (ulong at = region.Base; at < region.End; at += (ulong) window.Length - 24)
			{
				int length = (int) Math.Min((ulong) window.Length, region.End - at);
				if (length < 24 || !process.TryRead(at, window, length)) continue;

				for (int i = 0; i + 24 <= length; i += 8)
				{
					ulong begin = BitConverter.ToUInt64(window, i);
					ulong end = BitConverter.ToUInt64(window, i + 8);
					ulong capacity = BitConverter.ToUInt64(window, i + 16);

					if (begin < 0x10000 || end <= begin || capacity < end) continue;
					if ((end - begin) % 8 != 0) continue;
					long count = (long) ((end - begin) / 8);
					if (count < MinimumPaletteSize || count > MaximumPaletteSize) continue;
					if (!process.IsMapped(begin) || !process.IsMapped(end - 8)) continue;

					if (SpanIsAllStates(process, begin, count, states, word))
					{
						headers.Add(new VectorHeader(begin, end, capacity, (int) count));
					}
				}
			}
		}
		return headers;
	}

	/// <summary>
	///     Where a state keeps its network id, measured rather than assumed.
	///     <para>
	///         The palette is a list, so a state's place in it is known before anything is read, and
	///         with hashed ids off the network id is that place. The position holding each state's
	///         own index, for every state in the list, is the field. A position holding anything
	///         else fails on the second state, and one holding a constant fails on the first.
	///     </para>
	///     <para>
	///         With hashed ids on there is no index to match and no position answers, which is not a
	///         failure to find the field but the server saying it is not numbering states this way.
	///         The two cases are told apart by the count, never inferred from a value that looks
	///         wrong.
	///     </para>
	/// </summary>
	public static (int At, int Agreed) DeriveNetworkId(BedrockProcess process, VectorHeader header, int reach = 512)
	{
		var slots = new byte[header.Count * 8];
		if (!process.TryRead(header.Begin, slots, slots.Length)) return (-1, 0);

		// One state, one value. The state at place 222 must hold 222 somewhere, so every position
		// holding 222 in that state is a candidate and everything else is out. Then the candidates
		// are put to the whole list: the one that holds every other state's own place too is the
		// field. One state proposes, the population decides.
		const int probe = 222;
		var state = new byte[reach];
		ulong at222 = BitConverter.ToUInt64(slots, probe * 8);
		if (header.Count <= probe || at222 < 0x10000
			|| process.ReadClipped(at222, state, state.Length) < state.Length)
		{
			return (-1, 0);
		}

		var candidates = new List<int>();
		for (int at = 0; at + 4 <= reach; at += 4)
		{
			if (BitConverter.ToUInt32(state, at) == probe) candidates.Add(at);
		}

		if (candidates.Count == 0) return (-1, 0);

		var counts = new Dictionary<int, int>();
		foreach (int at in candidates) counts[at] = 0;
		for (int i = 0; i <= probe; i++)
		{
			ulong address = BitConverter.ToUInt64(slots, i * 8);
			if (address < 0x10000 || process.ReadClipped(address, state, state.Length) < state.Length) continue;
			foreach (int at in candidates)
			{
				if (BitConverter.ToUInt32(state, at) == (uint) i) counts[at]++;
			}
		}

		// The states that do not hold their own place, named with what they hold instead. A position
		// that is right for fifteen thousand states and wrong for thirteen is telling something
		// about those thirteen, and they cannot be seen unless they are listed.
		KeyValuePair<int, int> best = counts.OrderByDescending(c => c.Value).First();
		var missed = new List<string>();
		int unread = 0, agreed = 0;
		for (int i = 0; i < header.Count; i++)
		{
			ulong address = BitConverter.ToUInt64(slots, i * 8);
			if (address < 0x10000 || process.ReadClipped(address, state, best.Key + 4) < best.Key + 4)
			{
				unread++;
				continue;
			}
			uint held = BitConverter.ToUInt32(state, best.Key);
			if (held == (uint) i) agreed++;
			else if (missed.Count < 20) missed.Add($"slot {i} holds {held}");
		}

		Console.WriteLine($"  at state+{best.Key} over the whole palette: {missed.Count:N0} hold something else, {unread:N0} could not be read"
						+ (missed.Count > 0 ? $" ({string.Join(", ", missed)})" : ""));

		Console.WriteLine($"  state {probe} holds {probe} at {candidates.Count} position(s): "
						+ string.Join(", ", candidates.Select(c => $"state+{c} on {counts[c]:N0} of the first {probe + 1}")));

		// The whole palette's agreement, not the probe's. One state proposes and the population
		// decides, so the count that goes back is the one the population produced: returning the
		// proposal's count instead capped it at 223 and made the caller reject a position that had
		// just been verified against every state in the list.
		return (best.Key, agreed);
	}

	/// <summary>
	///     Each palette entry paired with what the reference says its light is, found by what the
	///     state IS rather than by where it sits: the block's name with its property values. Only
	///     the states the reference also knows come back, so a build with states it does not have
	///     contributes nothing rather than a wrong answer.
	/// </summary>
	public static (List<(ulong Address, int Value)> Emission, List<(ulong Address, int Value)> Dampening)
		KnownLight(BedrockProcess process, VectorHeader header)
	{
		var emission = new List<(ulong, int)>();
		var dampening = new List<(ulong, int)>();
		var slots = new byte[header.Count * 8];
		if (!process.TryRead(header.Begin, slots, slots.Length)) return (emission, dampening);

		var word = new byte[8];
		var heap = new byte[256];
		var legacy = new byte[MemoryLayout.NameInsideLegacy + MemoryLayout.LegacyReach];
		var reader = new BlockStateReader(process, Addresses(slots, header.Count));
		Dictionary<string, (int Emission, int Dampening)> known = StateSentinels.Known;

		for (int i = 0; i < header.Count; i++)
		{
			ulong address = BitConverter.ToUInt64(slots, i * 8);
			if (!process.IsMapped(address)) continue;

			ulong owner = process.ReadUInt64(address + (ulong) MemoryLayout.BlockLegacyPointer, word);
			if (!process.IsMapped(owner) || !process.TryRead(owner, legacy, legacy.Length)) continue;

			string name = HashedString.ReadVerified(process, legacy, MemoryLayout.NameInsideLegacy, heap);
			if (name is null) continue;

			List<StateProperty> properties = reader.Read(address, out _);
			if (!known.TryGetValue(StateSentinels.Key(name, properties), out (int Emission, int Dampening) light)) continue;

			emission.Add((address, light.Emission));
			dampening.Add((address, light.Dampening));
		}

		return (emission, dampening);
	}

	/// <summary>Reads the palette in order, resolving each entry through its own object.</summary>
	public static List<PaletteEntry> Read(BedrockProcess process, VectorHeader header)
	{
		var slots = new byte[header.Count * 8];
		if (!process.TryRead(header.Begin, slots, slots.Length)) return [];

		var state = new byte[MemoryLayout.BlockNetworkId + 4];
		var legacy = new byte[MemoryLayout.NameInsideLegacy + MemoryLayout.LegacyReach];
		var heap = new byte[256];
		var entries = new List<PaletteEntry>(header.Count);

		// The tag types are learned once and used for the whole palette.
		var reader = new BlockStateReader(process, Addresses(slots, header.Count));

		for (int i = 0; i < header.Count; i++)
		{
			ulong address = BitConverter.ToUInt64(slots, i * 8);
			string name = null;
			ulong hash = 0;
			uint network = 0;
			int legacyId = -1;
			int emission = 0;
			int dampening = 0;
			int version = 0;
			IReadOnlyList<StateProperty> properties = [];

			if (process.IsMapped(address) && process.TryRead(address, state, state.Length))
			{
				network = BitConverter.ToUInt32(state, MemoryLayout.BlockNetworkId);
				emission = state[MemoryLayout.StateLightEmission];
				dampening = state[MemoryLayout.StateLightDampening];
				properties = reader.Read(address, out version);
				ulong owner = BitConverter.ToUInt64(state, MemoryLayout.BlockLegacyPointer);
				if (process.IsMapped(owner) && process.TryRead(owner, legacy, legacy.Length))
				{
					// Read the name through this entry's own block, never through a shared table,
					// so one unreadable block cannot cost unrelated entries their names.
					name = HashedString.ReadVerified(process, legacy, MemoryLayout.NameInsideLegacy, heap);
					hash = BitConverter.ToUInt64(legacy, MemoryLayout.NameInsideLegacy);
					legacyId = BitConverter.ToUInt16(legacy, MemoryLayout.NameInsideLegacy + MemoryLayout.LegacyId);
				}
			}

			entries.Add(new PaletteEntry
			{
				Index = i,
				Name = name ?? "",
				NameHash = hash,
				NetworkId = network,
				LegacyId = legacyId,
				LightEmission = emission,
				LightDampening = dampening,
				States = properties,
				Version = version
			});
		}
		return entries;
	}

	private static IEnumerable<ulong> Addresses(byte[] slots, int count)
	{
		for (int i = 0; i < count; i++) yield return BitConverter.ToUInt64(slots, i * 8);
	}

	private static bool SpanIsAllStates(BedrockProcess process, ulong begin, long count, HashSet<ulong> states, byte[] word)
	{
		long step = Math.Max(1, count / SpanSamples);
		for (long k = 0; k < count; k += step)
		{
			if (!states.Contains(process.ReadUInt64(begin + (ulong) (k * 8), word))) return false;
		}
		return true;
	}

	private static List<ulong> FindPointersTo(BedrockProcess process, HashSet<ulong> targets)
	{
		var sites = new List<ulong>();
		var window = new byte[8 * 1024 * 1024];
		foreach (var region in process.Regions)
		{
			for (ulong at = region.Base; at < region.End; at += (ulong) window.Length)
			{
				int length = (int) Math.Min((ulong) window.Length, region.End - at);
				if (!process.TryRead(at, window, length)) continue;
				for (int i = 0; i + 8 <= length; i += 8)
				{
					ulong value = BitConverter.ToUInt64(window, i);
					if (value >= 0x10000 && targets.Contains(value)) sites.Add(at + (ulong) i);
				}
			}
		}
		return sites;
	}
}
