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
using System.Text;

namespace MiNET.BdsExtract;

/// <summary>
///     The extraction as two files, because the server has two things: blocks, and their states.
///     Everything a block is goes on the block. Its identity, its physical values, its tags, the
///     class implementing it, the properties it has with the values each can take, and what its old
///     data values mean. Splitting those across a file per question made a reader join four files
///     to answer one, and produced files like a list of properties with a count of how many blocks
///     carry each, which answers nothing anybody asked.
///     Everything a state is goes on the state: its runtime id, which block it belongs to, what it
///     is, and the light it gives off and takes away.
/// </summary>
public static class BlockDocument
{
	/// <summary>
	///     The header lines every output leads with: the block state schema version and, spelled out
	///     beside it, the release it encodes (the number is four bytes, major.minor.patch.revision).
	///     Every state carries the same stamp (frozen at 1.21.60.33 for years); if a build ever
	///     splits it, the split is written as the list it is instead of one value hiding the other.
	/// </summary>
	public static string VersionHeader(IReadOnlyList<PaletteEntry> palette)
	{
		int[] versions = palette.Select(p => p.Version).Distinct().OrderBy(v => v).ToArray();
		static string Release(int v) => $"{v >> 24 & 0xff}.{v >> 16 & 0xff}.{v >> 8 & 0xff}.{v & 0xff}";
		return versions.Length == 1
			? $"\t\"blockStateVersion\": {versions[0]},\n\t\"blockStateRelease\": \"{Release(versions[0])}\",\n"
			: $"\t\"blockStateVersions\": [{string.Join(", ", versions)}],\n\t\"blockStateReleases\": [{string.Join(", ", versions.Select(v => $"\"{Release(v)}\""))}],\n";
	}

	/// <summary>One row per block, with everything the extraction knows about it.</summary>
	public static string WriteBlocks(BedrockProcess process, string versionHeader,
		IReadOnlyList<BlockProperties> blocks,
		IReadOnlyDictionary<ulong, string> classNames,
		IReadOnlyDictionary<string, BlockStateRange> ranges,
		IReadOnlyDictionary<string, int> propertyIds,
		IReadOnlyDictionary<string, LegacyStateTable> legacy,
		IReadOnlyDictionary<string, List<BlockComponent>> components)
	{
		var window = new byte[BlockLayout.Reach];
		var scratch = new byte[256];
		var text = new StringBuilder("{\n");
		text.Append(versionHeader);
		text.Append($"\t\"layoutPublishedFor\": \"{BlockMembers.Build}\",\n");
		text.Append(Classes(BlockMembers.Block, false, BlockLayout.Has, BlockLayout.At));
		text.Append($"\t\"count\": {blocks.Count},\n");
		text.Append("\t\"blocks\": [\n");
		for (int i = 0; i < blocks.Count; i++)
		{
			var block = blocks[i];

			// Geometry comes from the component the id names, because the field read picked the
			// wrong component on 5 blocks: the visual component also carries a namespaced string
			// and sorts ahead of the geometry one. That is correcting a misread, not choosing
			// between two readings.
			// Map colour is NOT substituted the same way. The field genuinely reads zero on the 95
			// blocks that carry the component, and both are already in the output, so replacing
			// one with the other would be this file deciding which is true rather than saying what
			// each holds.
			var carried = components.GetValueOrDefault(block.Name) ?? [];
			string geometry = Component(carried, "minecraft:geometry") ?? block.Geometry;

			text.Append("\t\t{\n");
			text.Append($"\t\t\t\"name\": \"{Escape(block.Name)}\",\n");

			// Every member of the class, under the class's own name, read from where this build
			// keeps it. Not a selection: a member holding a container is a row with a null value,
			// so what nothing reads is counted rather than absent.
			foreach (BlockMemberReader.Value value in BlockMemberReader.Read(process, block.Address, window, scratch))
			{
				if (value.Name == "tags")
				{
					// The tags member is the vector this tool reads, so its own row carries them
					// rather than a null beside a second list under the same name. Two entries under
					// one key is a value lost: a reader keeps the last and never sees the first.
					text.Append("			\"tags\": [");
					for (int t = 0; t < block.Tags.Count; t++)
					{
						text.Append(t == 0 ? "" : ", ");
						text.Append($"\"{Escape(block.Tags[t])}\"");
					}
					text.Append("],\n");
					continue;
				}
				text.Append($"\t\t\t\"{value.Name}\": {value.Json ?? "null"},\n");
			}

			text.Append($"\t\t\t\"class\": \"{Escape(classNames.GetValueOrDefault(block.Vtable, "block"))}\",\n");
			text.Append($"\t\t\t\"geometry\": {Text(geometry)},\n");

			// Where the row was read. Not a fact about the block and only good while that server
			// lives, but it is what lets a follow up probe go straight to the object instead of
			// finding the name again and hoping it landed on a BlockLegacy.
			text.Append($"\t\t\t\"address\": \"0x{block.Address:X}\",\n");

			// What of the object these values represent. The size is the allocator's, not the
			// furthest field read, and the holes are the bytes inside the object that no field
			// above accounts for. They are stated rather than dropped so the gap is countable: a
			// row listing twenty values out of an object three times that size reads as complete
			// unless it says otherwise. Null holes mean the size could not be measured at all.
			text.Append($"\t\t\t\"objectSize\": {(block.ObjectSize > 0 ? block.ObjectSize.ToString() : "null")},\n");
			text.Append("\t\t\t\"unread\": ");
			if (block.Unread is null)
			{
				text.Append("null,\n");
			}
			else
			{
				text.Append('[');
				for (int h = 0; h < block.Unread.Count; h++)
				{
					text.Append(h == 0 ? "" : ", ");
					text.Append($"{{ \"at\": {block.Unread[h].At}, \"bytes\": {block.Unread[h].Bytes} }}");
				}
				text.Append("],\n");
			}

			// The carried components are not published. Their ids are per-instance registration
			// numbers, so two runs of the same build disagree on every block, and rows that change
			// with the heap are noise dressed as data. They are still read, because the geometry
			// field above is resolved through them; they are just not rows in this file.


			// The properties and their values, which is what a state of this block can be.
			var range = ranges.GetValueOrDefault(block.Name);
			text.Append($"\t\t\t\"stateCount\": {range?.States ?? 0},\n");
			text.Append("\t\t\t\"stateProperties\": {");
			if (range is not null)
			{
				for (int p = 0; p < range.Properties.Count; p++)
				{
					var property = range.Properties[p];
					text.Append(p == 0 ? "\n" : ",\n");
					text.Append($"\t\t\t\t\"{Escape(property.Name)}\": {{ ");
					if (propertyIds.TryGetValue(property.Name, out int id)) text.Append($"\"id\": {id}, ");
					text.Append($"\"type\": \"{property.Type}\", \"values\": [");
					for (int v = 0; v < property.Values.Count; v++)
					{
						text.Append(v == 0 ? "" : ", ");
						text.Append(Value(property.Values[v]));
					}
					text.Append("] }");
				}
				if (range.Properties.Count > 0) text.Append("\n\t\t\t");
			}
			text.Append("},\n");

			// What each pre-flattening data value means, as network ids into the state file. Null
			// is a data value the server has no state for.
			var table = legacy.GetValueOrDefault(block.Name);
			text.Append("\t\t\t\"byData\": [");
			text.Append(table is null ? "" : string.Join(", ", table.ByData.Select(v => v?.ToString() ?? "null")));
			text.Append("]\n");

			text.Append("\t\t}");
			text.Append(i == blocks.Count - 1 ? "\n" : ",\n");
		}
		return text.Append("\t]\n}\n").ToString();
	}

	/// <summary>One row per state, in the server's own order, which is the runtime id order.</summary>
	public static string WriteStates(BedrockProcess process, IReadOnlyList<PaletteEntry> palette, ExtractionReport report)
	{
		// Say the id scheme in the file. Without it a reader cannot tell whether networkId is a
		// hash or a repeat of the index, and both look equally reasonable.
		var stateWindow = new byte[BlockLayout.StateReach];
		var scratch = new byte[256];
		var text = new StringBuilder("{\n");
		text.Append($"\t\"networkIdsAreHashes\": {Boolean(report.NetworkIdsAreHashes)},\n");

		text.Append(VersionHeader(palette));

		// The states file states its own class table, the same way the block file does, so its own
		// output can be the next reference without a layout living anywhere else.
		text.Append($"\t\"layoutPublishedFor\": \"{BlockMembers.Build}\",\n");
		text.Append(Classes(BlockMembers.State, true, BlockLayout.StateHas, BlockLayout.StateAt));
		text.Append($"\t\"count\": {palette.Count},\n");
		text.Append("\t\"states\": [\n");
		for (int i = 0; i < palette.Count; i++)
		{
			var entry = palette[i];
			text.Append("\t\t{ ");
			text.Append($"\"index\": {entry.Index}, ");
			text.Append($"\"name\": \"{Escape(entry.Name)}\", ");
			text.Append($"\"version\": {entry.Version}, ");

			// Every member of the state class, under the class's own name, read from where this
			// build keeps it. networkId is one of them, so it is not written twice, and neither is
			// the light: the palette's own reading of it locates the field and the class member is
			// what the file states.
			foreach (BlockMemberReader.Value value in
				BlockMemberReader.ReadState(process, entry.Address, stateWindow, scratch))
			{
				text.Append($"\"{value.Name}\": {value.Json ?? "null"}, ");
			}
			text.Append("\"states\": {");
			for (int s = 0; s < entry.States.Count; s++)
			{
				var property = entry.States[s];
				text.Append(s == 0 ? " " : ", ");
				text.Append($"\"{Escape(property.Name)}\": {property.ToJson()}");
			}
			text.Append(entry.States.Count == 0 ? "}" : " }");
			text.Append(" }");
			text.Append(i == palette.Count - 1 ? "\n" : ",\n");
		}
		return text.Append("\t]\n}\n").ToString();
	}

	/// <summary>
	///     Which class the file's objects are, and every class reachable from it: each one's size
	///     and its members, at the offsets that class declares. The root's members carry where this
	///     server keeps them, because those are the only ones that can move; everything reachable
	///     from them states its own offsets, which is the whole reason a class is written down once
	///     rather than spelled into whatever holds it.
	/// </summary>
	private static string Classes(ClassLayout root, bool state, Func<string, bool> has, Func<string, int> at)
	{
		var written = new List<ClassLayout>();
		Reach(root, state, written);

		var text = new StringBuilder($"\t\"class\": \"{root.Name}\",\n\t\"classes\": {{\n");
		for (int c = 0; c < written.Count; c++)
		{
			ClassLayout held = written[c];
			text.Append($"\t\t\"{held.Name}\": {{ \"size\": {held.Size}, \"members\": [\n");
			for (int m = 0; m < held.Members.Count; m++)
			{
				BlockMember member = held.Members[m];
				int position = held == root ? has(member.Name) ? at(member.Name) : -1 : member.At;
				text.Append($"\t\t\t{{ \"name\": \"{member.Name}\", \"at\": {position}, "
						+ $"\"bytes\": {member.Bytes}, \"kind\": \"{member.Kind}\"");
				if (member.Holds is not null) text.Append($", \"holds\": \"{member.Holds}\"");
				text.Append(m == held.Members.Count - 1 ? " }\n" : " },\n");
			}

			text.Append(c == written.Count - 1 ? "\t\t] }\n" : "\t\t] },\n");
		}

		return text.Append("\t},\n").ToString();
	}

	/// <summary>Every class the root holds, and every class those hold, each written once.</summary>
	private static void Reach(ClassLayout held, bool state, List<ClassLayout> written)
	{
		if (held is null || written.Contains(held)) return;
		written.Add(held);
		foreach (BlockMember member in held.Members)
		{
			if (member.Holds is not null) Reach(BlockMembers.Held(member.Holds, state), state, written);
		}
	}

	/// <summary>The value of one component the block carries, or null when it has none.</summary>
	private static string Component(IReadOnlyList<BlockComponent> carried, string name)
	{
		return carried.FirstOrDefault(c => c.Name == name)?.Value;
	}

	private static string Value(object value)
	{
		return value switch
		{
			bool b => b ? "true" : "false",
			int i => i.ToString(CultureInfo.InvariantCulture),
			string s => $"\"{Escape(s)}\"",
			_ => "null"
		};
	}

	private static string Boolean(bool value)
	{
		return value ? "true" : "false";
	}

	private static string Text(string value)
	{
		return value is null ? "null" : $"\"{Escape(value)}\"";
	}

	private static string Escape(string value)
	{
		return value.Replace("\\", "\\\\").Replace("\"", "\\\"");
	}
}
