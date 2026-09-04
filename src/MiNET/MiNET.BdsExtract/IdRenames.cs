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
///     The renames that carry no version with them.
///     The versioned table ends at 1.21.60.33 while the server is far past that, and a rename like
///     chain to iron_chain is nowhere in it. It is not missing: it is held in a separate map that
///     no version gates, applied to any id it matches whatever the world was written at. The
///     published reconstruction files that same rename under a schema whose own ceiling reads
///     1.21.60.33, which is what a version-shaped model has to do with a change that has no version.
///     Each entry is a list node: links, then the old id and the new one as HashedStrings.
/// </summary>
public static class IdRenames
{
	private const int NodeNext = 0;
	private const int NodePrevious = 8;
	private const int NodeOld = 16;
	private const int NodeNew = 64;
	private const int NodeReach = 104;
	private const string Namespace = "minecraft:";

	/// <summary>
	///     A map this size is a map; a pair of ids that happen to sit together is not. The real one
	///     holds eighteen entries and the largest accidental cluster holds one, so the gap is wide
	///     and the number does not need to be exact.
	/// </summary>
	private const int MinimumEntries = 3;

	public static List<KeyValuePair<string, string>> Read(BedrockProcess process)
	{
		var word = new byte[8];
		var candidates = FindCandidates(process);

		// Two ids forty eight bytes apart is the same trap the component sweep fell into: the
		// server holds a sorted registry of every block name at exactly that stride, so adjacency
		// alone matched two thousand neighbouring pairs. What separates a real entry is that its
		// node's links lead to other entries, so the map is the largest connected group.
		var best = LargestGroup(process, candidates, word);
		return best
			.Select(node => candidates[node])
			.OrderBy(pair => pair.Key, StringComparer.Ordinal)
			.ToList();
	}

	private static Dictionary<ulong, KeyValuePair<string, string>> FindCandidates(BedrockProcess process)
	{
		var window = new byte[8 * 1024 * 1024];
		var heap = new byte[128];
		var found = new Dictionary<ulong, KeyValuePair<string, string>>();

		foreach (var region in process.Regions)
		{
			for (ulong at = region.Base; at < region.End; at += (ulong) window.Length - NodeReach)
			{
				int length = (int) Math.Min((ulong) window.Length, region.End - at);
				if (length < NodeReach || !process.TryRead(at, window, length)) continue;

				for (int i = 0; i + NodeReach <= length; i += 8)
				{
					string oldId = HashedString.ReadVerified(process, window, i + NodeOld, heap);
					if (oldId is null || !oldId.StartsWith(Namespace, StringComparison.Ordinal)) continue;

					string newId = HashedString.ReadVerified(process, window, i + NodeNew, heap);
					if (newId is null || newId == oldId) continue;
					if (!newId.StartsWith(Namespace, StringComparison.Ordinal)) continue;

					found[at + (ulong) i] = new KeyValuePair<string, string>(oldId, newId);
				}
			}
		}
		return found;
	}

	/// <summary>The biggest set of candidates that reach each other through their own links.</summary>
	private static List<ulong> LargestGroup(BedrockProcess process,
		Dictionary<ulong, KeyValuePair<string, string>> candidates, byte[] word)
	{
		var seen = new HashSet<ulong>();
		var best = new List<ulong>();

		foreach (ulong start in candidates.Keys)
		{
			if (!seen.Add(start)) continue;

			var group = new List<ulong> {start};
			var frontier = new Queue<ulong>();
			frontier.Enqueue(start);
			while (frontier.Count > 0)
			{
				ulong node = frontier.Dequeue();
				foreach (int link in (int[]) [NodeNext, NodePrevious])
				{
					ulong other = process.ReadUInt64(node + (ulong) link, word);
					if (!candidates.ContainsKey(other) || !seen.Add(other)) continue;
					group.Add(other);
					frontier.Enqueue(other);
				}
			}
			if (group.Count > best.Count) best = group;
		}
		return best.Count >= MinimumEntries ? best : [];
	}
}
