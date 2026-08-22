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

using System.Text;

namespace MiNET.BdsExtract;

/// <summary>One state property as the server registers it, with the number it goes by.</summary>
public sealed class RegisteredProperty
{
	/// <summary>The server's own number for the property, the same on every block that has it.</summary>
	public int Id { get; init; }

	public string Name { get; init; }

	/// <summary>How many blocks carry it, which is the difference between axis and bite_counter.</summary>
	public int Blocks { get; init; }
}

/// <summary>
///     The block state property registry, read from the table every block keeps of its own
///     properties.
///     BlockLegacy holds a sixteen bucket hash of the properties that block has, and each entry
///     carries the property's name as a HashedString and a number. That number is the same wherever
///     the property appears: across all 1,429 blocks not one property is numbered two ways, which
///     is what makes it an id rather than a position.
///     Reading it is also a check on the palette. The property SETS this table gives match the ones
///     derived from the palette's states on all 1,429 blocks, and the names agree exactly, 113 of
///     them. Two structures with nothing in common, built for different purposes, agreeing block
///     for block is worth more than either on its own.
/// </summary>
public static class StateProperties
{
	private const int Buckets = 16;
	private const int NodeName = 16;     // a HashedString: the hash, then the text
	private const int NodeId = 64;
	private const int MaximumId = 1024;

	public static List<RegisteredProperty> Read(BedrockProcess process, IReadOnlyList<BlockProperties> blocks)
	{
		var word = new byte[8];
		var heap = new byte[256];
		var byName = new Dictionary<string, (int Id, int Count)>(StringComparer.Ordinal);

		foreach (var block in blocks)
		{
			foreach (ulong node in Nodes(process, block.Address, word))
			{
				var header = new byte[HashedString.Size];
				if (!process.TryRead(node + NodeName, header, header.Length)) continue;

				string name = HashedString.ReadVerified(process, header, 0, heap,
					MemoryLayout.MinimumTagLength, MemoryLayout.MaximumTagLength);
				if (name is null) continue;

				ulong id = process.ReadUInt64(node + NodeId, word);
				if (id > MaximumId) continue;

				// A property numbered two ways would mean this is a position, not an id. It never
				// happens, and if it starts happening the count is what will show it.
				if (byName.TryGetValue(name, out var seen))
				{
					if (seen.Id == (int) id) byName[name] = (seen.Id, seen.Count + 1);
					continue;
				}
				byName[name] = ((int) id, 1);
			}
		}

		return byName
			.Select(p => new RegisteredProperty {Id = p.Value.Id, Name = p.Key, Blocks = p.Value.Count})
			.OrderBy(p => p.Id)
			.ToList();
	}

	/// <summary>Which properties one block has, by the id the registry gives them.</summary>
	public static List<int> Of(BedrockProcess process, BlockProperties block, byte[] word)
	{
		var ids = new List<int>();
		foreach (ulong node in Nodes(process, block.Address, word))
		{
			ulong id = process.ReadUInt64(node + NodeId, word);
			if (id <= MaximumId && !ids.Contains((int) id)) ids.Add((int) id);
		}
		ids.Sort();
		return ids;
	}

	/// <summary>
	///     Which properties one block has, by name. The same walk as <see cref="Of" />, kept apart
	///     because the guard compares names against the palette and the ids mean nothing to it.
	/// </summary>
	public static List<string> NamesOf(BedrockProcess process, BlockProperties block, byte[] word)
	{
		var heap = new byte[256];
		var header = new byte[HashedString.Size];
		var names = new List<string>();
		foreach (ulong node in Nodes(process, block.Address, word))
		{
			if (!process.TryRead(node + NodeName, header, header.Length)) continue;
			string name = HashedString.ReadVerified(process, header, 0, heap,
				MemoryLayout.MinimumTagLength, MemoryLayout.MaximumTagLength);
			if (name is not null && !names.Contains(name)) names.Add(name);
		}
		return names;
	}

	/// <summary>
	///     The distinct entries of a block's property table. Sixteen buckets pointing into a linked
	///     chain of nodes, and an empty bucket points at the table's own head. A bucket holding two
	///     properties keeps the second behind the first node's link words, so the walk follows every
	///     node's own links as well as the bucket heads. Only a node whose name verifies as a
	///     HashedString is yielded, which no arbitrary byte run can pass; the head sentinel fails it
	///     and yields nothing, while its links still lead to real nodes. Found on 1.26.50, where
	///     trip_wire is the first block to land two properties in one bucket and a heads-only walk
	///     read 7 of its 8, silently dropping minecraft:connection_east.
	/// </summary>
	private static IEnumerable<ulong> Nodes(BedrockProcess process, ulong legacy, byte[] word)
	{
		ulong begin = process.ReadUInt64(legacy + (ulong) MemoryLayout.BlockProperties, word);
		ulong end = process.ReadUInt64(legacy + (ulong) (MemoryLayout.BlockProperties + 8), word);
		if (begin < 0x10000 || end <= begin || (end - begin) / 8 != Buckets) yield break;
		if (!process.IsMapped(begin)) yield break;

		var heap = new byte[256];
		var header = new byte[HashedString.Size];
		var seen = new HashSet<ulong>();
		var pending = new Queue<ulong>();
		for (int i = 0; i < Buckets; i++)
		{
			pending.Enqueue(process.ReadUInt64(begin + (ulong) (i * 8), word));
		}

		// The visit cap is a runaway backstop only: a table cannot hold more nodes than the
		// registry holds properties, and the seen set already stops every cycle.
		while (pending.Count > 0 && seen.Count <= MaximumId)
		{
			ulong node = pending.Dequeue();
			if (node < 0x10000 || !seen.Add(node) || !process.IsMapped(node)) continue;

			// The first two words of a node are its links. Followed unconditionally: a word that
			// is not a link fails the mapped or verified test on the far side and goes no further.
			pending.Enqueue(process.ReadUInt64(node, word));
			pending.Enqueue(process.ReadUInt64(node + 8, word));

			if (!process.TryRead(node + NodeName, header, header.Length)) continue;
			if (HashedString.ReadVerified(process, header, 0, heap,
				MemoryLayout.MinimumTagLength, MemoryLayout.MaximumTagLength) is null) continue;

			yield return node;
		}
	}

	/// <summary>The registry, smallest id first, with how many blocks carry each property.</summary>
	public static string Write(IReadOnlyList<RegisteredProperty> properties)
	{
		var text = new StringBuilder("{\n");
		text.Append($"\t\"count\": {properties.Count},\n");
		text.Append("\t\"properties\": [\n");
		for (int i = 0; i < properties.Count; i++)
		{
			var property = properties[i];
			text.Append($"\t\t{{ \"id\": {property.Id}, \"name\": \"{property.Name}\", ");
			text.Append($"\"blocks\": {property.Blocks} }}");
			text.Append(i == properties.Count - 1 ? "\n" : ",\n");
		}
		return text.Append("\t]\n}\n").ToString();
	}
}
