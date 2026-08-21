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
	/// <summary>One row per block, with everything the extraction knows about it.</summary>
	public static string WriteBlocks(IReadOnlyList<BlockProperties> blocks,
		IReadOnlyDictionary<ulong, string> classNames,
		IReadOnlyDictionary<string, BlockStateRange> ranges,
		IReadOnlyDictionary<string, int> propertyIds,
		IReadOnlyDictionary<string, LegacyStateTable> legacy,
		IReadOnlyDictionary<string, List<BlockComponent>> components)
	{
		var text = new StringBuilder("{\n");
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
			text.Append($"\t\t\t\"legacyId\": {block.LegacyId},\n");
			text.Append($"\t\t\t\"serializationId\": {Text(block.SerializationId)},\n");
			text.Append($"\t\t\t\"creativeGroup\": {Text(block.CreativeGroup)},\n");
			text.Append($"\t\t\t\"creativeCategory\": {Byte(block, MemoryLayout.CreativeCategoryByte)},\n");
			text.Append($"\t\t\t\"blockEntityType\": {Byte(block, MemoryLayout.BlockEntityTypeByte)},\n");
			text.Append($"\t\t\t\"material\": {Byte(block, MemoryLayout.MaterialByte)},\n");
			text.Append($"\t\t\t\"class\": \"{Escape(classNames.GetValueOrDefault(block.Vtable, "block"))}\",\n");
			text.Append($"\t\t\t\"geometry\": {Text(geometry)},\n");
			text.Append($"\t\t\t\"hardness\": {Number(block.Hardness)},\n");
			text.Append($"\t\t\t\"explosionResistance\": {Number(block.ExplosionResistance)},\n");
			text.Append($"\t\t\t\"friction\": {Number(block.Friction)},\n");
			text.Append($"\t\t\t\"thickness\": {Number(block.Thickness)},\n");
			text.Append($"\t\t\t\"translucency\": {Number(block.Translucency)},\n");
			text.Append($"\t\t\t\"burnOdds\": {block.BurnOdds},\n");
			text.Append($"\t\t\t\"flameOdds\": {block.FlameOdds},\n");
			text.Append($"\t\t\t\"isSolid\": {Boolean(block.IsSolid)},\n");
			text.Append($"\t\t\t\"requiresCorrectToolForDrops\": {Boolean(Flag(block,
				MemoryLayout.ToolRequiredByte, MemoryLayout.ToolRequiredBit))},\n");
			text.Append($"\t\t\t\"fallable\": {Boolean(Flag(block,
				MemoryLayout.FallableByte, MemoryLayout.FallableBit))},\n");
			text.Append($"\t\t\t\"blockLightEmission\": {Byte(block, MemoryLayout.LightEmissionByte)},\n");
			text.Append($"\t\t\t\"blockLightDampening\": {Byte(block, MemoryLayout.LightDampeningByte)},\n");
			text.Append($"\t\t\t\"canContainLiquidSource\": {Boolean(block.CanContainLiquidSource)},\n");
			text.Append($"\t\t\t\"liquidReactionOnTouch\": \"{block.LiquidReactionOnTouch}\",\n");
			text.Append($"\t\t\t\"tintMethod\": \"{block.TintMethod}\",\n");
			text.Append($"\t\t\t\"mapColor\": \"{block.MapColor}\",\n");
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

			// The eight bytes at name+128. Only one is identified, bit 1 of the fifth being the
			// solid flag, so the rest go out by offset with their values and no interpretation.
			text.Append($"\t\t\t\"unnamedBytes\": {{ \"at\": \"name+{MemoryLayout.UnnamedBytes}\", \"values\": [");
			text.Append(string.Join(", ", block.UnnamedBytes));
			text.Append("] },\n");

			// What the block actually carries, against the forty nine the schema allows. An
			// unnamed one still goes out with its size: dropping it would make the block look
			// like it holds fewer components than it does.
			var held = components.GetValueOrDefault(block.Name) ?? [];
			text.Append("\t\t\t\"components\": [");
			for (int c = 0; c < held.Count; c++)
			{
				text.Append(c == 0 ? "" : ", ");
				var component = held[c];
				// The id is always written, the name only when it is known. The id is the server's
				// own identity for the component, so a row with an id and no name still says
				// exactly which component the block carries.
				text.Append(component.Name is null
					? $"{{ \"id\": {component.Id}, \"name\": null, \"size\": {component.Size}, "
					+ $"\"bytes\": {Text(component.Bytes)} }}"
					: $"{{ \"id\": {component.Id}, \"name\": \"{Escape(component.Name)}\", "
					+ $"\"value\": {Text(component.Value)} }}");
			}
			text.Append("],\n");

			text.Append("\t\t\t\"tags\": [");
			for (int t = 0; t < block.Tags.Count; t++)
			{
				text.Append(t == 0 ? "" : ", ");
				text.Append($"\"{Escape(block.Tags[t])}\"");
			}
			text.Append("],\n");

			// The properties and their values, which is what a state of this block can be.
			var range = ranges.GetValueOrDefault(block.Name);
			text.Append($"\t\t\t\"stateCount\": {range?.States ?? 0},\n");
			text.Append("\t\t\t\"properties\": {");
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
	public static string WriteStates(IReadOnlyList<PaletteEntry> palette, ExtractionReport report)
	{
		// Say the id scheme in the file. Without it a reader cannot tell whether networkId is a
		// hash or a repeat of the index, and both look equally reasonable.
		var text = new StringBuilder("{\n");
		text.Append($"\t\"networkIdsAreHashes\": {Boolean(report.NetworkIdsAreHashes)},\n");
		text.Append($"\t\"count\": {palette.Count},\n");
		text.Append("\t\"states\": [\n");
		for (int i = 0; i < palette.Count; i++)
		{
			var entry = palette[i];
			text.Append("\t\t{ ");
			text.Append($"\"index\": {entry.Index}, ");
			text.Append($"\"name\": \"{Escape(entry.Name)}\", ");
			text.Append($"\"networkId\": {entry.NetworkId}, ");
			text.Append($"\"lightEmission\": {entry.LightEmission}, ");
			text.Append($"\"lightDampening\": {entry.LightDampening}, ");
			text.Append($"\"version\": {entry.Version}, ");
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

	/// <summary>One whole byte out of the eight, for the two that hold a value rather than flags.</summary>
	private static string Byte(BlockProperties block, int index)
	{
		return index < block.UnnamedBytes.Count
			? block.UnnamedBytes[index].ToString(CultureInfo.InvariantCulture)
			: "null";
	}

	/// <summary>One bit out of the flag bytes, for the flags that have been identified.</summary>
	private static bool Flag(BlockProperties block, int index, int bit)
	{
		return index < block.UnnamedBytes.Count && (block.UnnamedBytes[index] >> bit & 1) != 0;
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

	private static string Number(float value)
	{
		return float.IsFinite(value) ? value.ToString("0.######", CultureInfo.InvariantCulture) : "null";
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
