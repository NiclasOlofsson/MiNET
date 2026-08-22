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

using System.Globalization;
using System.Text.Json;

/// <summary>
///     Finds where an item keeps each of its values on this build, the same way the block side does:
///     every item the reference knows is scored against every position in the object, and the
///     position that gives the right value for at least nine tenths of them is where the field is.
///     <para>
///         Positions are stated from the name, because the name is the one place in the object that
///         proves itself: a HashedString hashes to its own text. Everything else is measured from it,
///         and a build that moved a field is read where it now sits rather than where it used to.
///     </para>
/// </summary>
public static class ItemDerivation
{
	/// <summary>How a value is written, so a position can be tried the way the field is read.</summary>
	private enum Kind
	{
		Byte,
		Flag,
		Int16,
		UInt16,
		UInt32,
		Float,
		Text,
		Hex
	}

	/// <summary>
	///     The fields the item file states a value for, with where in the JSON that value sits and
	///     how it is written. A field the file does not state cannot be measured and keeps the
	///     position compiled into <see cref="ItemLayout" />.
	/// </summary>
	private static readonly (string Field, string Path, Kind Kind)[] Fields =
	[
		("version", "version", Kind.Byte),
		("iconFrameCount", "iconFrameCount", Kind.Byte),
		("animatesInToolbar", "animatesInToolbar", Kind.Flag),
		("mirroredArt", "mirroredArt", Kind.Flag),
		("useAnimation", "useAnimation.value", Kind.Byte),
		("maxStackSize", "maxStackSize", Kind.Byte),
		("id", "id", Kind.Int16),
		("translationKey", "translationKey", Kind.Text),
		("textureAtlas", "textureAtlas", Kind.Text),
		("icon", "icon", Kind.Text),
		("icon2", "icon2", Kind.Text),
		("creativeGroup", "creative.group", Kind.Text),
		("maxDurability", "maxDurability", Kind.UInt16),
		("flags", "flags.raw", Kind.Hex),
		("useDuration", "useDuration", Kind.UInt32),
		("creativeCategory", "creative.category", Kind.Byte),
		("hiddenInCommands", "hiddenInCommands", Kind.Byte),
		("rarity", "rarity.value", Kind.Byte),
		("mineBlockType", "mineBlockType.value", Kind.Byte),
		("furnaceFuel", "furnace.fuelDuration", Kind.Float),
		("smeltingExperience", "furnace.smeltingExperience", Kind.Float)
	];

	/// <summary>
	///     Measures every field it can and applies what it measures, reporting each either way. The
	///     population is the items this server and the reference both have.
	/// </summary>
	public static void Measure(BedrockProcess process, IReadOnlyCollection<ItemRegistry.Item> items)
	{
		Dictionary<string, Dictionary<string, string>> known = Load();
		if (known.Count == 0) return;

		var population = items.Where(i => i.IsItem && known.ContainsKey(i.Name)).ToList();
		Console.WriteLine($"item field positions, measured over {population.Count:N0} items the reference also has:");

		foreach ((string field, string path, Kind kind) in Fields)
		{
			var wanted = new List<(ItemRegistry.Item Item, string Value)>();
			foreach (ItemRegistry.Item item in population)
			{
				if (known[item.Name].TryGetValue(path, out string value)) wanted.Add((item, value));
			}

			if (wanted.Count == 0)
			{
				Console.WriteLine($"  {field,-20} the reference states it for no item this server has");
				continue;
			}

			var scored = new List<(int At, int Held)>();
			foreach (int at in Positions(kind))
			{
				int held = wanted.Count(w => Holds(process, w.Item, at, kind, w.Value));
				if (held >= Sentinels.Floor * wanted.Count) scored.Add((at, held));
			}

			if (scored.Count == 0)
			{
				Console.WriteLine($"  {field,-20} no position holds it for {Sentinels.Floor:P0} of {wanted.Count:N0} items");
				continue;
			}

			int most = scored.Max(s => s.Held);
			List<(int At, int Held)> best = scored.Where(s => s.Held == most).ToList();
			if (best.Count > 1)
			{
				Console.WriteLine($"  {field,-20} {best.Count} positions hold it: {string.Join(", ", best.Select(b => b.At))}");
				continue;
			}

			ItemLayout.UseMeasured(field, best[0].At);
			Console.WriteLine($"  {field,-20} measured at name{best[0].At:+#;-#;+0}, on {best[0].Held:N0} of {wanted.Count:N0} items");
		}

		Console.WriteLine();
	}

	/// <summary>
	///     Every position a value of that width can sit at, from the object's start to the end of the
	///     part every item has. Aligned to its own width, because the compiler aligns it there.
	/// </summary>
	private static IEnumerable<int> Positions(Kind kind)
	{
		int width = kind switch
		{
			Kind.Int16 or Kind.UInt16 => 2,
			Kind.UInt32 or Kind.Float => 4,
			Kind.Text => 8,
			_ => 1
		};

		int first = ItemLayout.MethodTable;
		for (int at = first; at + Span(kind) <= Reach; at++)
		{
			if (((at - first) % width) == 0) yield return at;
		}
	}

	/// <summary>How far past the name every item reaches, which is the part they all have in common.</summary>
	private const int Reach = 240;

	private static int Span(Kind kind) => kind switch
	{
		Kind.Int16 or Kind.UInt16 => 2,
		Kind.UInt32 or Kind.Float => 4,
		Kind.Text => 32,
		_ => 1
	};

	/// <summary>Whether the position holds the value, read the way that kind of value is written.</summary>
	private static bool Holds(BedrockProcess process, ItemRegistry.Item item, int at, Kind kind, string value)
	{
		if (!item.Covers(at, Span(kind))) return false;

		switch (kind)
		{
			case Kind.Byte:
				return int.TryParse(value, out int b) && b is >= 0 and < 256 && item.Byte(at) == b;
			case Kind.Flag:
				return (item.Byte(at) != 0) == (value == "true");
			case Kind.Hex:
				return value.StartsWith("0x", StringComparison.Ordinal)
					&& int.TryParse(value[2..], NumberStyles.HexNumber, CultureInfo.InvariantCulture, out int hex)
					&& item.Byte(at) == hex;
			case Kind.Int16:
				return int.TryParse(value, out int i) && i is >= short.MinValue and <= short.MaxValue && item.I16(at) == i;
			case Kind.UInt16:
				return int.TryParse(value, out int u) && u is >= 0 and <= ushort.MaxValue && item.U16(at) == u;
			case Kind.UInt32:
				return uint.TryParse(value, out uint w) && item.U32(at) == w;
			case Kind.Float:
				return float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out float f)
					&& item.F32(at) == f;
			case Kind.Text:
				return ItemRegistry.StdString(process, item.Window, item.At(at), new byte[256]) == value;
			default:
				return false;
		}
	}

	/// <summary>Every item the reference knows, by name, with each value as the file writes it.</summary>
	private static Dictionary<string, Dictionary<string, string>> Load()
	{
		string path = Path.Combine(WorldConfig.AssetsDirectory(), "reference", "items-runtime.json");
		var known = new Dictionary<string, Dictionary<string, string>>(StringComparer.Ordinal);
		if (!File.Exists(path))
		{
			Console.Error.WriteLine($"no reference at {path}; no item field position can be measured");
			return known;
		}

		using JsonDocument document = JsonDocument.Parse(File.ReadAllText(path));
		if (!document.RootElement.TryGetProperty("items", out JsonElement items)) return known;

		foreach (JsonElement item in items.EnumerateArray())
		{
			if (!item.TryGetProperty("name", out JsonElement name)) continue;
			var values = new Dictionary<string, string>(StringComparer.Ordinal);
			foreach ((string _, string jsonPath, Kind _) in Fields)
			{
				JsonElement held = item;
				bool found = true;
				foreach (string step in jsonPath.Split('.'))
				{
					if (held.ValueKind != JsonValueKind.Object || !held.TryGetProperty(step, out held))
					{
						found = false;
						break;
					}
				}

				if (!found) continue;
				// Taken as the file writes it. Re-emitting a number through a formatter here is what
				// destroyed values on the block side.
				string text = held.ValueKind switch
				{
					JsonValueKind.String => held.GetString(),
					JsonValueKind.True => "true",
					JsonValueKind.False => "false",
					JsonValueKind.Number => held.GetRawText(),
					_ => null
				};
				if (text is not null) values[jsonPath] = text;
			}

			if (values.Count > 0) known[name.GetString() ?? ""] = values;
		}

		return known;
	}
}
