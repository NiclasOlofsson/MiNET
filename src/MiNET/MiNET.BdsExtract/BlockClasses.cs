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

/// <summary>
///     Names the C++ class behind each block, so the grouping can be used rather than just read.
///     A binary with no RTTI keeps no class names, and the only identity a class has is the address
///     of its method table. That address is real but it moves every build, so it is no use to
///     anything downstream. The members are stable, though: the class holding a hundred and
///     fifty four blocks whose names all end in slab is the slab class whatever its address is this
///     time, and naming it from its members gives the grouping a handle that survives a rebuild.
///     The name is derived, not read. It says what the class contains, which is the useful thing
///     for generating code from it, and it is not a claim about what Mojang calls it.
/// </summary>
public static class BlockClasses
{
	private const string Namespace = "minecraft:";

	/// <summary>
	///     A token has to be shared by this much of the class to name it. Below that the members
	///     have nothing in common and a name taken from them would describe a minority.
	/// </summary>
	private const double Majority = 0.6;

	public static Dictionary<ulong, string> Name(IReadOnlyList<BlockProperties> blocks)
	{
		var members = new Dictionary<ulong, List<string>>();
		foreach (var block in blocks)
		{
			if (block.Vtable < 0x10000) continue;
			if (!members.TryGetValue(block.Vtable, out var list)) members[block.Vtable] = list = [];
			list.Add(block.Name.StartsWith(Namespace, StringComparison.Ordinal)
				? block.Name[Namespace.Length..]
				: block.Name);
		}

		// Biggest first, so the class that most deserves a plain name gets it and the smaller ones
		// take the numbered variants.
		var named = new Dictionary<ulong, string>();
		var used = new Dictionary<string, int>(StringComparer.Ordinal);
		foreach (var (vtable, list) in members.OrderByDescending(m => m.Value.Count)
					.ThenBy(m => m.Key))
		{
			string name = Describe(list);
			if (used.TryGetValue(name, out int seen))
			{
				used[name] = seen + 1;
				name = $"{name}_{seen + 1}";
			}
			else
			{
				used[name] = 1;
			}
			named[vtable] = name;
		}
		return named;
	}

	/// <summary>
	///     What a group of blocks has in common, as a name.
	///     One member names itself. Otherwise the last word most of them share does it: every
	///     acacia_slab and andesite_slab is a slab. When they share nothing, as the class holding
	///     every plain solid block does, there is nothing honest to call it but a block.
	/// </summary>
	private static string Describe(List<string> members)
	{
		if (members.Count == 1) return members[0];

		// Both ends. A family is named by its tail often enough that suffixes alone get most of
		// them, but element_0 to element_118 share a head and nothing else, and so does every
		// stripped_ log; those came out as block_2 and block_4 until prefixes were counted too.
		var shared = new Dictionary<string, int>(StringComparer.Ordinal);
		foreach (string member in members)
		{
			// Once per member, whichever end it matched. A family often contains the plain name it
			// is built from, and "candle" is both the suffix of every candle and the whole of one
			// of them, so counting both ends put it above the class's own size.
			var parts = new HashSet<string>(StringComparer.Ordinal);
			var words = member.Split('_');
			for (int start = 0; start < words.Length; start++) parts.Add(string.Join('_', words[start..]));
			for (int end = 1; end <= words.Length; end++) parts.Add(string.Join('_', words[..end]));
			foreach (string part in parts) shared[part] = shared.GetValueOrDefault(part) + 1;
		}

		int enough = (int) Math.Ceiling(members.Count * Majority);
		string best = null;
		foreach (var (part, count) in shared)
		{
			if (count < enough) continue;
			// The longest part that still covers the class, so log beats nothing and candle_cake
			// beats cake.
			if (best is null || part.Length > best.Length) best = part;
		}
		return best ?? "block";
	}
}
