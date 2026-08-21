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

/// <summary>One block's states, in the order the server indexes them by the old data value.</summary>
public sealed class LegacyStateTable
{
	public string Name { get; init; }
	public int LegacyId { get; init; }

	/// <summary>
	///     The network id of the state each data value means, or null where the server has no state
	///     for it. A null is a fact: acacia_button reserves three bits for a facing that has six
	///     values, so data 6 and 7 were never legal, and the table says so rather than leaving the
	///     reader to guess how many of the sixteen slots count.
	/// </summary>
	public IReadOnlyList<uint?> ByData { get; init; } = [];

	public int Used => ByData.Count(v => v is not null);
}

/// <summary>
///     What a block's pre flattening data value meant, read from the server rather than shipped.
///     Every BlockLegacy holds a vector of its states indexed by the old data value, which is the
///     mapping a world written before the flattening needs: block id 17 data 1 is a spruce log.
///     That mapping is currently carried around as a binary blob nobody can read, copied between
///     projects; this is the same thing from the server that actually uses it, in a form that can
///     be checked.
///     The vector is not the palette's list. It is the full product of each property's bit width,
///     so it holds slots for combinations that are not legal states, and those slots are null.
///     Counting them is what tells a block with 12 states and 16 data values from one where the two
///     agree, and 250 of the 1,429 blocks are the former.
/// </summary>
public static class LegacyStates
{
	public static List<LegacyStateTable> Read(BedrockProcess process, IReadOnlyList<BlockProperties> blocks)
	{
		var word = new byte[8];
		var tables = new List<LegacyStateTable>();

		foreach (var block in blocks)
		{
			ulong begin = process.ReadUInt64(block.Address + MemoryLayout.BlockStates, word);
			ulong end = process.ReadUInt64(block.Address + MemoryLayout.BlockStates + 8, word);
			if (begin < 0x10000 || end <= begin || (end - begin) % 8 != 0) continue;

			int count = (int) ((end - begin) / 8);
			if (count > MaximumStates || !process.IsMapped(begin)) continue;

			var byData = new List<uint?>(count);
			for (int i = 0; i < count; i++)
			{
				ulong state = process.ReadUInt64(begin + (ulong) (i * 8), word);

				// An empty slot is a data value the server has no state for, and it is written as
				// one. Only a slot that belongs to this block counts: the back pointer is what
				// says so, and it is the same check the palette read uses.
				if (state < 0x10000 || !process.IsMapped(state)
					|| process.ReadUInt64(state + MemoryLayout.BlockLegacyPointer, word) != block.Address)
				{
					byData.Add(null);
					continue;
				}
				byData.Add(process.TryRead(state + MemoryLayout.BlockNetworkId, word, 4)
					? BitConverter.ToUInt32(word, 0)
					: null);
			}

			tables.Add(new LegacyStateTable
			{
				Name = block.Name,
				LegacyId = block.LegacyId,
				ByData = byData
			});
		}
		return tables;
	}

	/// <summary>
	///     What each old data value means now, one line per block. The numbers are network ids so
	///     they join to block_palette.json, and null is a data value with no state behind it.
	/// </summary>
	public static string Write(IReadOnlyList<LegacyStateTable> tables)
	{
		var text = new StringBuilder("{\n");
		text.Append($"\t\"count\": {tables.Count},\n");
		text.Append("\t\"blocks\": [\n");
		for (int i = 0; i < tables.Count; i++)
		{
			var table = tables[i];
			text.Append("\t\t{ ");
			text.Append($"\"name\": \"{table.Name}\", ");
			text.Append($"\"legacyId\": {table.LegacyId}, ");
			text.Append($"\"used\": {table.Used}, ");
			text.Append($"\"byData\": [{string.Join(", ", table.ByData.Select(v => v?.ToString() ?? "null"))}]");
			text.Append(" }");
			text.Append(i == tables.Count - 1 ? "\n" : ",\n");
		}
		return text.Append("\t]\n}\n").ToString();
	}

	/// <summary>
	///     A data value was four bits before the flattening, and the widest table here is a hanging
	///     sign at five hundred and twelve. Anything longer is not this vector.
	/// </summary>
	private const int MaximumStates = 4096;
}
