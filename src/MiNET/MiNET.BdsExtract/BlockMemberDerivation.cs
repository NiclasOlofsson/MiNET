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

using System.Text.Json;
using System.Text.Json.Nodes;

/// <summary>
///     Says whether the layout the reference states still reads the server in front of it.
///     <para>
///         Every member is read where the reference puts it, and counted against the value the
///         reference states for that object. An object is matched between the two by what it is: a
///         block by its full name, a state by its block's name and its property values.
///     </para>
///     <para>
///         At or above the floor the offset still reads the field and the run goes on. Below the
///         floor it is said out loud. Below half the offset is reading something else, and the run
///         stops rather than write an output that looks right.
///     </para>
///     <para>
///         Nothing here searches and nothing here moves. When a member breaks, its offset is
///         corrected in the reference by hand and the run is repeated until the new layout reads.
///     </para>
/// </summary>
public static class BlockMemberDerivation
{
	/// <summary>How a member came out of the check.</summary>
	public enum Verdict
	{
		/// <summary>Right for at least the floor of the objects, so the offset still reads it.</summary>
		Holds,

		/// <summary>Right for most but not the floor. The offset is probably still the field.</summary>
		Slipped,

		/// <summary>Right for less than half. The offset is reading something else.</summary>
		Broken,

		/// <summary>The reference states no value for it, so there is nothing to check it against.</summary>
		Unchecked
	}

	/// <summary>What the check found for one member.</summary>
	public readonly record struct Found(string Name, int At, int Held, int Of, Verdict Verdict)
	{
		public double Share => Of == 0 ? 0 : (double) Held / Of;
	}

	/// <summary>One object to check against: what it is, and where it is.</summary>
	public readonly record struct Subject(string Key, ulong Address);

	/// <summary>Every block, keyed by its full name, which is what a block is.</summary>
	public static List<Found> CheckBlocks(BedrockProcess process, IReadOnlyList<BlockProperties> blocks, int reach)
	{
		return Check(process, BlockLayout.Blocks,
			blocks.Select(b => new Subject(b.Name, b.Address)).ToList(),
			Load("blocks.json", "blocks", BlockLayout.Blocks, b => b.TryGetProperty("name", out JsonElement name)
				&& name.ValueKind == JsonValueKind.String ? name.GetString() : null),
			reach);
	}

	/// <summary>
	///     Every state, keyed by its block's name and its property values. Never by its place in the
	///     palette: the order is the server's own and two builds do not have to agree on it, so an
	///     index would compare one build's state against another build's neighbour.
	/// </summary>
	public static List<Found> CheckStates(BedrockProcess process, IReadOnlyList<PaletteEntry> palette, int reach)
	{
		return Check(process, BlockLayout.States,
			palette.Select(e => new Subject(StateSentinels.Key(e.Name, e.States), e.Address)).ToList(),
			Load("block_states.json", "states", BlockLayout.States, StateSentinels.KeyOf),
			reach);
	}

	/// <summary>
	///     Every item, keyed by its full name. An item's window starts before its name rather than at
	///     the object, so the window it is read through is handed over already read.
	/// </summary>
	public static List<Found> CheckItems(BedrockProcess process, IReadOnlyCollection<ItemRegistry.Item> items)
	{
		ClassTree tree = ItemLayout.Items;
		Dictionary<string, Dictionary<int, JsonNode>> known =
			Load("items-runtime.json", "items", tree, i => i.TryGetProperty("name", out JsonElement name)
				&& name.ValueKind == JsonValueKind.String ? name.GetString() : null);

		var windows = new List<(byte[] Window, ulong Address, Dictionary<int, JsonNode> Wanted)>();
		foreach (ItemRegistry.Item item in items)
		{
			if (!item.IsItem) continue;
			if (!known.TryGetValue(item.Name, out Dictionary<int, JsonNode> wanted)) continue;
			windows.Add((item.Window, item.Address, wanted));
		}

		return Score(process, tree, windows, ItemRegistry.NameInsideItem - ItemRegistry.Before);
	}

	/// <summary>
	///     Reads every member where the reference says it is and counts how many objects still hold
	///     what the reference says they hold. Nothing is searched for and nothing is moved: the
	///     reference states the layout, and this says whether that layout still reads this server.
	/// </summary>
	private static List<Found> Check(BedrockProcess process, ClassTree tree,
		IReadOnlyList<Subject> subjects, Dictionary<string, Dictionary<int, JsonNode>> known, int reach)
	{
		if (known.Count == 0) return [];

		// Every object both the reference and this server have, read once.
		var windows = new List<(byte[] Window, ulong Address, Dictionary<int, JsonNode> Wanted)>();
		foreach (Subject subject in subjects)
		{
			if (!known.TryGetValue(subject.Key, out Dictionary<int, JsonNode> wanted)) continue;
			var window = new byte[reach];
			if (process.ReadClipped(subject.Address, window, reach) < reach) continue;
			windows.Add((window, subject.Address, wanted));
		}

		return Score(process, tree, windows, 0);
	}

	/// <summary>
	///     The count, member by member. The lead is how far into each window the object starts, which
	///     is nothing when the window is the object and something when it begins before it.
	/// </summary>
	private static List<Found> Score(BedrockProcess process, ClassTree tree,
		List<(byte[] Window, ulong Address, Dictionary<int, JsonNode> Wanted)> windows, int lead)
	{
		var results = new List<Found>();
		var scratch = new byte[256];
		foreach (MemberNode leaf in tree.Leaves)
		{
			// A member holding something with no class of its own has no value to read, so there is
			// nothing to check whatever the file states beside its name. Nor has a member whose
			// value is a fact about the build rather than about the object: a state's place in the
			// palette is a different number on every release by construction, and what has to be
			// true of it, that it is unique and that the sequence is contiguous, is checked where
			// the palette itself is checked.
			if (leaf.Member.Kind == MemberKind.Container || !leaf.Member.Comparable)
			{
				results.Add(new Found(leaf.Path, leaf.At, 0, 0, Verdict.Unchecked));
				continue;
			}

			int position = leaf.At - lead;
			int held = 0, of = 0;
			foreach ((byte[] window, ulong address, Dictionary<int, JsonNode> values) in windows)
			{
				if (!values.TryGetValue(leaf.Index, out JsonNode stated)) continue;
				of++;
				if (position < 0 || position + leaf.Member.Bytes > window.Length) continue;
				if (JsonNode.DeepEquals(BlockMemberReader.Node(process, address, window, scratch, position, leaf.Member), stated)) held++;
			}

			Verdict verdict = of == 0
				? Verdict.Unchecked
				: held >= Sentinels.Floor * of
					? Verdict.Holds
					: held >= Sentinels.Abort * of
						? Verdict.Slipped
						: Verdict.Broken;
			results.Add(new Found(leaf.Path, leaf.At, held, of, verdict));
		}

		return results;
	}

	/// <summary>
	///     Every object the reference knows, by what it is, with each member's value written exactly
	///     as the file writes it. An object two keys cannot separate is used by neither, because a
	///     value that could have come from either proves nothing about where it was read.
	/// </summary>
	private static Dictionary<string, Dictionary<int, JsonNode>> Load(string file, string array,
		ClassTree tree, Func<JsonElement, string> keyOf)
	{
		string path = Path.Combine(WorldConfig.AssetsDirectory(), "reference", file);
		var known = new Dictionary<string, Dictionary<int, JsonNode>>(StringComparer.Ordinal);
		if (!File.Exists(path))
		{
			Console.Error.WriteLine($"no reference at {path}; no member position can be measured");
			return known;
		}

		using JsonDocument document = JsonDocument.Parse(File.ReadAllText(path));
		if (!document.RootElement.TryGetProperty(array, out JsonElement objects)) return known;

		var seen = new HashSet<string>(StringComparer.Ordinal);
		foreach (JsonElement held in objects.EnumerateArray())
		{
			string key = keyOf(held);
			if (key is null) continue;
			if (!seen.Add(key))
			{
				known.Remove(key);
				continue;
			}

			var values = new Dictionary<int, JsonNode>();
			Gather(tree.Roots, held, values);
			if (values.Count > 0) known[key] = values;
		}

		return known;
	}

	/// <summary>
	///     What the reference states for one object, walked the same way the class is: a member is
	///     looked for by its own name inside whatever holds it, and a member that holds a class is
	///     descended into. The reference is not flattened and no path is built. A member the file
	///     states nothing for is simply absent, and the search then has nothing to match it on.
	/// </summary>
	private static void Gather(IReadOnlyList<MemberNode> nodes, JsonElement held, Dictionary<int, JsonNode> values)
	{
		if (held.ValueKind != JsonValueKind.Object) return;
		foreach (MemberNode node in nodes)
		{
			if (!held.TryGetProperty(node.Member.Name, out JsonElement stated)) continue;
			if (stated.ValueKind == JsonValueKind.Null) continue;
			if (!node.IsLeaf)
			{
				Gather(node.Holds, stated, values);
				continue;
			}

			// A leaf holds one value. A file stating an object or a list under its name is stating
			// something else, and comparing a number against it would count every object as wrong
			// rather than as unstated. The exceptions are the leaves that ARE written as one: a
			// version, a range, a box.
			if (stated.ValueKind == JsonValueKind.Object
				&& node.Member.Kind is not (MemberKind.Version or MemberKind.Range)) continue;
			if (stated.ValueKind == JsonValueKind.Array
				&& node.Member.Kind is not (MemberKind.Box or MemberKind.Bits64)) continue;
			values[node.Index] = JsonNode.Parse(stated.GetRawText());
		}
	}
}
