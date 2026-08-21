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
///     Reads a running Bedrock Dedicated Server and writes what it finds as JSON, into the Data
///     folder of this project unless told otherwise. Three outputs, answering different questions.
///     block_palette.json is the palette: every block state the server knows, in the server's own
///     order. Position in that list is the palette index, which is the runtime id when the server
///     is not using hashed ids. Each entry also carries its network id, which is the runtime id
///     when it is, what state it is, and the light it gives off and takes away. Nothing about the
///     order is computed here, it is the order the server holds.
///     block_properties.json is per block, not per state: hardness, friction, blast resistance and
///     the rest, the things every state of a block shares.
///     block_upgrade_rules.json is what the server does to blocks that no longer exist when it
///     loads an old world. Nothing else publishes those rules; every copy in circulation was
///     reconstructed from the outside by people who could not read them. It holds the records
///     that read as rules: every slot in their tables resolves and they name a block or its
///     states.
///     block_upgrade_raw.json is every record the same search matched, rules and otherwise. Most
///     of it is not upgrade data, hash buckets and allocation regions where the occasional slot
///     happens to look like a tag, and it is kept because the line between the two is drawn here
///     rather than by the server. A line drawn wrongly should be visible, not silent.
/// </summary>
public static class Program
{
	public static int Main(string[] args)
	{
		// Reconnaissance for the item side: what is named in the process that the block palette does
		// not claim. It reports rather than writes, because offsets are worth having only once they
		// have been fitted against items whose values are known.
		if (args.Length > 0 && args[0] == "--items") return ItemRegistry.Run(args[1..]);

		string outputDirectory = DefaultOutputDirectory();
		string pathFilter = null;
		bool upgradesOnly = false;

		for (int i = 0; i < args.Length; i++)
		{
			switch (args[i])
			{
				case "--out" when i + 1 < args.Length:
					outputDirectory = args[++i];
					break;
				case "--server" when i + 1 < args.Length:
					pathFilter = args[++i];
					break;
				// The palette read is the slow half, so it can be skipped while the upgrade side
				// is what is being worked on.
				case "--upgrades":
					upgradesOnly = true;
					break;
				default:
					Console.Error.WriteLine("usage: MiNET.BdsExtract [--out <directory>] [--server <path fragment>]");
							Console.Error.WriteLine();
					Console.Error.WriteLine("  --out     where to write the json files, default the Data folder in this project");
					Console.Error.WriteLine("  --upgrades  read only the upgrade table, skipping the palette");
					Console.Error.WriteLine("  --server  which server to read when several are running, matched on");
					Console.Error.WriteLine(@"            executable path, for example --server bds\probe");
					return 2;
			}
		}

		try
		{
			return Run(outputDirectory, pathFilter, upgradesOnly);
		}
		catch (Exception e)
		{
			Console.Error.WriteLine($"error: {e.Message}");
			return 1;
		}
	}

	/// <summary>
	///     Where the extraction lands unless told otherwise: the Data folder beside this project's
	///     source, so a run updates the copy that is committed rather than dropping files wherever
	///     it happened to be started from.
	///     The data is the point of the tool. Reading it needs a Windows machine, a matching
	///     server build and the patience to run one, so committing the result is what lets everyone
	///     else use it without any of that.
	/// </summary>
	internal static string DefaultOutputDirectory()
	{
		var directory = new DirectoryInfo(AppContext.BaseDirectory);
		while (directory is not null)
		{
			if (directory.GetFiles("MiNET.BdsExtract.csproj").Length > 0)
			{
				return Path.Combine(directory.FullName, "Data");
			}
			directory = directory.Parent;
		}
		// Running from somewhere that is not a build of this project, so stay where we are.
		return "Data";
	}

	private static int Run(string outputDirectory, string pathFilter, bool upgradesOnly)
	{
		using var server = BedrockProcess.Attach(pathFilter);
		Console.WriteLine($"reading pid {server.Id}");
		Console.WriteLine($"  {server.ExecutablePath}");

		if (upgradesOnly)
		{
			var only = BlockUpgradeReader.Read(server);
			Console.WriteLine($"upgrade records: {only.Count:N0}, {only.Count(x => x.IsRule):N0} read as rules");
			Directory.CreateDirectory(outputDirectory);
			string onlyPath = Path.Combine(outputDirectory, "block_upgrade_rules.json");
			File.WriteAllText(onlyPath, WriteUpgrades(only, IdRenames.Read(server)), new UTF8Encoding(false));
			Console.WriteLine($"written {onlyPath}");
			return 0;
		}

		var blocks = BlockRegistry.Read(server);
		Console.WriteLine($"blocks found: {blocks.Count:N0}");
		if (blocks.Count == 0)
		{
			Console.Error.WriteLine("no blocks found; the field offsets probably do not match this server build");
			return 1;
		}

		var states = BlockPalette.FindStateObjects(server, blocks);
		var vectors = BlockPalette.FindVectors(server, states);
		Console.WriteLine($"block states: {states.Count:N0}, candidate palettes: {vectors.Count}");
		if (vectors.Count == 0)
		{
			Console.Error.WriteLine("no palette found; the field offsets probably do not match this server build");
			return 1;
		}

		// The palette is the largest such vector. The smaller ones are chunk storage, which holds
		// the same kind of pointer but only as many as fit in a sixteen cubed section.
		var header = vectors.OrderByDescending(v => v.Count).First();
		var palette = BlockPalette.Read(server, header);
		Console.WriteLine($"palette read from 0x{header.Begin:X}");

		// The palette decides what a block is. The memory sweep also turns up features, biomes and
		// other named things that are not blocks, and there is no need to guess which: anything the
		// palette never refers to is not a block.
		var inPalette = palette.Select(e => e.Name).ToHashSet(StringComparer.Ordinal);
		int swept = blocks.Count;
		blocks = blocks.Where(b => inPalette.Contains(b.Name)).ToList();
		Console.WriteLine($"blocks in the palette: {blocks.Count:N0} of {swept:N0} named objects swept");

		var report = ExtractionReport.Check(palette);
		report.WriteTo(Console.Out);

		// A block whose read failed is not written. The row that would go out otherwise is not a
		// weaker measurement, it is an invented one: zero hardness, zero friction and a liquid
		// reaction that is not in the enum, in the same shape as the blocks that read correctly and
		// with nothing to mark it. This tool's whole claim is that it can fail to find a block but
		// cannot invent one, and writing that row is the one way it breaks that claim.
		var incomplete = blocks.Where(b => !BlockRegistry.IsComplete(b)).ToList();
		blocks = blocks.Where(BlockRegistry.IsComplete).ToList();
		if (incomplete.Count > 0)
		{
			Console.Error.WriteLine($"  {incomplete.Count} block(s) did not read and are NOT in the output:");
			foreach (var block in incomplete) Console.Error.WriteLine($"      {block.Name}");
		}

		// The offsets, proved against this build before anything downstream trusts them. This runs
		// ahead of the upgrade table because a failure here invalidates every read, so there is no
		// point spending the rest of the extraction on it.
		Console.WriteLine("layout guard:");
		if (!LayoutGuard.Report(LayoutGuard.Check(server, blocks, palette), Console.Out)) return 2;

		var schemas = BlockUpgradeReader.Read(server);
		var rules = schemas.Where(x => x.IsRule).ToList();
		Console.WriteLine($"upgrade records: {schemas.Count:N0} found, {rules.Count:N0} read as rules, "
						+ $"{schemas.Sum(x => x.EntryCount):N0} entries");

		// The names the palette uses, plus every key the records mention. An old property name is
		// one the palette has usually stopped using, so both halves are needed to find the renames.
		var propertyNames = palette
			.SelectMany(e => e.States.Select(s => s.Name))
			.Concat(schemas.SelectMany(x => x.Tables).SelectMany(t => t).Select(e => e.Key))
			.ToHashSet(StringComparer.Ordinal);
		var renames = BlockUpgradeReader.ReadRenamedProperties(server, propertyNames);
		Console.WriteLine($"string pairs on a property name: {renames.Count:N0} from {propertyNames.Count:N0} known names");

		var renamedIds = IdRenames.Read(server);
		Console.WriteLine($"unversioned id renames: {renamedIds.Count:N0}");

		var components = ComponentNames.Read(server);
		Console.WriteLine($"component names: {components.Count:N0} across "
						+ $"{components.Select(c => c.Owner).Distinct().Count()} owners");

		var stateProperties = StateProperties.Read(server, blocks);
		Console.WriteLine($"state properties: {stateProperties.Count:N0} registered, "
						+ $"ids {stateProperties.Min(p => p.Id)} to {stateProperties.Max(p => p.Id)}");

		var legacyStates = LegacyStates.Read(server, blocks);
		Console.WriteLine($"legacy data tables: {legacyStates.Count:N0} blocks, "
						+ $"{legacyStates.Sum(t => t.ByData.Count):N0} data values, "
						+ $"{legacyStates.Sum(t => t.ByData.Count - t.Used):N0} of them never legal");

		var classNames = BlockClasses.Name(blocks);
		Console.WriteLine($"block classes: {classNames.Count:N0} distinct, "
						+ $"largest {blocks.GroupBy(b => b.Vtable).Max(g => g.Count()):N0} blocks");

		var held = ComponentReader.Read(server, blocks);
		int identified = held.Values.Sum(c => c.Count(x => x.Name is not null));
		int carried = held.Values.Sum(c => c.Count);
		Console.WriteLine($"components carried: {carried:N0} instances, {identified:N0} identified, "
						+ $"{held.Values.SelectMany(c => c).Where(x => x.Name is not null).Select(x => x.Name).Distinct().Count()} types named");

		var ranges = BlockStateRanges.Read(palette);
		int dependent = ranges.Count(r => !r.Independent);
		Console.WriteLine($"state ranges: {ranges.Count:N0} blocks, "
						+ $"{ranges.Sum(r => r.Properties.Count):N0} properties, "
						+ $"{dependent:N0} whose states are fewer than their combinations");

		Directory.CreateDirectory(outputDirectory);
		string blockPath = Path.Combine(outputDirectory, "blocks.json");
		string statePath = Path.Combine(outputDirectory, "block_states.json");
		string upgradePath = Path.Combine(outputDirectory, "block_upgrade_rules.json");
		string componentPath = Path.Combine(outputDirectory, "component_names.json");

		File.WriteAllText(blockPath, BlockDocument.WriteBlocks(blocks, classNames,
			ranges.ToDictionary(r => r.Name, StringComparer.Ordinal),
			stateProperties.ToDictionary(p => p.Name, p => p.Id, StringComparer.Ordinal),
			legacyStates.ToDictionary(t => t.Name, StringComparer.Ordinal), held), new UTF8Encoding(false));
		File.WriteAllText(statePath, BlockDocument.WriteStates(palette, report), new UTF8Encoding(false));
		File.WriteAllText(upgradePath, WriteUpgrades(rules, renamedIds), new UTF8Encoding(false));
		File.WriteAllText(componentPath, WriteComponents(components), new UTF8Encoding(false));
		Console.WriteLine($"written {blockPath}");
		Console.WriteLine($"written {statePath}");
		Console.WriteLine($"written {upgradePath}");
		Console.WriteLine($"written {componentPath}");

		return report.Passed && incomplete.Count == 0 ? 0 : 1;
	}

	private static string WritePalette(IReadOnlyList<PaletteEntry> palette, ExtractionReport report)
	{
		// State the id scheme in the file itself. Without it a reader cannot tell whether
		// networkId is a hash or a repeat of index, and both look equally reasonable.
		var text = new StringBuilder("{\n");
		text.Append($"\t\"networkIdsAreHashes\": {Boolean(report.NetworkIdsAreHashes)},\n");
		text.Append($"\t\"count\": {palette.Count},\n");
		text.Append("\t\"palette\": [\n");
		for (int i = 0; i < palette.Count; i++)
		{
			var entry = palette[i];
			// One entry per line. Seventeen thousand entries broken across eight lines each is
			// not more readable than this, it is just longer.
			text.Append("\t\t{ ");
			text.Append($"\"index\": {entry.Index}, ");
			text.Append($"\"name\": \"{entry.Name}\", ");
			text.Append($"\"nameHash\": \"0x{entry.NameHash:X16}\", ");
			text.Append($"\"networkId\": {entry.NetworkId}, ");
			text.Append($"\"legacyId\": {entry.LegacyId}, ");
			text.Append($"\"lightEmission\": {entry.LightEmission}, ");
			text.Append($"\"lightDampening\": {entry.LightDampening}, ");
			text.Append($"\"version\": {entry.Version}, ");
			text.Append("\"states\": {");
			for (int s = 0; s < entry.States.Count; s++)
			{
				var property = entry.States[s];
				text.Append(s == 0 ? " " : ", ");
				text.Append($"\"{property.Name}\": {property.ToJson()}");
			}
			text.Append(entry.States.Count == 0 ? "}" : " }");
			text.Append(" }");
			text.Append(i == palette.Count - 1 ? "\n" : ",\n");
		}
		return text.Append("\t]\n}\n").ToString();
	}

	/// <summary>
	///     What values each block's properties take, with both counts kept.
	///     "states" is how many the palette holds and "combinations" is the product of the ranges.
	///     They are equal for most blocks and are written whether or not they agree, because a block
	///     that does not use every combination is telling us something and correcting one number
	///     against the other would erase it.
	/// </summary>
	private static string WriteRanges(IReadOnlyList<BlockStateRange> ranges)
	{
		var text = new StringBuilder("{\n");
		text.Append($"\t\"count\": {ranges.Count},\n");
		text.Append($"\t\"blocks\": [\n");
		for (int i = 0; i < ranges.Count; i++)
		{
			var block = ranges[i];
			text.Append("\t\t{ ");
			text.Append($"\"name\": \"{Escape(block.Name)}\", ");
			text.Append($"\"states\": {block.States}, ");
			text.Append($"\"combinations\": {block.Product}, ");
			text.Append("\"properties\": {");
			for (int p = 0; p < block.Properties.Count; p++)
			{
				var property = block.Properties[p];
				text.Append(p == 0 ? " " : ", ");
				text.Append($"\"{Escape(property.Name)}\": {{ \"type\": \"{property.Type}\", \"values\": [");
				for (int v = 0; v < property.Values.Count; v++)
				{
					text.Append(v == 0 ? "" : ", ");
					text.Append(RangeValue(property.Values[v]));
				}
				text.Append("] }");
			}
			text.Append(block.Properties.Count == 0 ? "}" : " }");
			text.Append(" }");
			text.Append(i == ranges.Count - 1 ? "\n" : ",\n");
		}
		return text.Append("\t]\n}\n").ToString();
	}

	/// <summary>
	///     The component vocabulary, grouped by what owns each component.
	///     Names only, which is all that is missing elsewhere: the values are the compiled fields in
	///     block_properties.json and the shape of each component is in the JSON schemas BDS writes
	///     about itself. An alias listed twice has two live names and both are kept.
	/// </summary>
	private static string WriteComponents(IReadOnlyList<ComponentName> components)
	{
		var text = new StringBuilder("{\n");
		var owners = components.GroupBy(c => c.Owner).OrderBy(g => g.Key, StringComparer.Ordinal).ToList();
		for (int o = 0; o < owners.Count; o++)
		{
			var rows = owners[o].ToList();
			text.Append($"\t\"{Escape(owners[o].Key)}\": [\n");
			for (int i = 0; i < rows.Count; i++)
			{
				text.Append($"\t\t{{ \"name\": \"{Escape(rows[i].Name)}\", ");
				text.Append($"\"field\": \"{Escape(rows[i].Alias)}\" }}");
				text.Append(i == rows.Count - 1 ? "\n" : ",\n");
			}
			text.Append("\t]");
			text.Append(o == owners.Count - 1 ? "\n" : ",\n");
		}
		return text.Append("}\n").ToString();
	}

	private static string RangeValue(object value)
	{
		return value switch
		{
			bool b => b ? "true" : "false",
			int i => i.ToString(CultureInfo.InvariantCulture),
			string s => $"\"{Escape(s)}\"",
			_ => "null"
		};
	}

	private static string WriteRenames(IReadOnlyDictionary<string, string> renames)
	{
		var text = new StringBuilder("{\n");
		var ordered = renames.OrderBy(r => r.Key, StringComparer.Ordinal).ToList();
		for (int i = 0; i < ordered.Count; i++)
		{
			text.Append($"\t\"{Escape(ordered[i].Key)}\": \"{Escape(ordered[i].Value)}\"");
			text.Append(i == ordered.Count - 1 ? "\n" : ",\n");
		}
		return text.Append("}\n").ToString();
	}

	/// <summary>
	///     The whole migration as one list, in the order the server applies it, one step per line.
	///     Nothing is nested and nothing lives in a second file. A flattening used to be a rule with
	///     children hanging off it, so reading what happened to wool meant descending into the rule
	///     and carrying the parent's block down with you; here each child is a step of its own that
	///     names its block and its condition outright. The renames that carry no version are the
	///     same kind of thing and sit at the top, where they belong: they are applied first and to
	///     everything.
	///     Every step has the same four parts, so the file reads down the page: when it applies,
	///     what it applies to, what has to match, and what comes out.
	/// </summary>
	private static string WriteUpgrades(IReadOnlyList<UpgradeSchema> schemas,
		IReadOnlyList<KeyValuePair<string, string>> renamedIds)
	{
		// A block state carries a version stamp and the server runs every rule stamped above it in
		// order, so sorting by the stamp IS the order of application: major, minor and patch name
		// the release and the last byte counts the rules within it from zero.
		var steps = new List<(uint Version, UpgradeSchema Rule, UpgradeSchema Parent)>();
		foreach (var rule in schemas.OrderBy(x => x.Version))
		{
			steps.Add((rule.Version, rule, null));
			foreach (var child in rule.Nested) steps.Add((rule.Version, child, rule));
		}

		var text = new StringBuilder("{\n");
		text.Append($"\t\"steps\": {steps.Count},\n");
		text.Append("\t\"renameIds\": {");
		for (int i = 0; i < renamedIds.Count; i++)
		{
			text.Append(i == 0 ? "\n" : ",\n");
			text.Append($"\t\t\"{Escape(renamedIds[i].Key)}\": \"{Escape(renamedIds[i].Value)}\"");
		}
		text.Append(renamedIds.Count == 0 ? "},\n" : "\n\t},\n");

		text.Append("\t\"upgrade\": [\n");
		for (int i = 0; i < steps.Count; i++)
		{
			var (version, rule, parent) = steps[i];
			text.Append("\t\t{ ");
			text.Append($"\"version\": \"{(version >> 24) & 0xFF}.{(version >> 16) & 0xFF}."
						+ $"{(version >> 8) & 0xFF}.{version & 0xFF}\", ");

			// A child names the block its parent names, so the step stands on its own.
			string subject = rule.Subject ?? parent?.Subject;
			bool pattern = rule.Subject is not null ? rule.SubjectIsPattern : parent?.SubjectIsPattern ?? false;
			text.Append(subject is null ? ""
				: pattern ? $"\"blockPattern\": \"{Escape(subject)}\", "
				: $"\"block\": \"{Escape(subject)}\", ");

			text.Append($"\"match\": {Side(rule, 0)}, ");
			text.Append($"\"result\": {Side(rule, 1)}");

			// The bare property names, written once when both sides list the same ones, which is
			// almost always: a rule names the properties it touches, not two different sets.
			string matchFields = Fields(rule, 0);
			string resultFields = Fields(rule, 1);
			if (matchFields == resultFields)
			{
				if (matchFields != "[]") text.Append($", \"fields\": {matchFields}");
			}
			else
			{
				if (matchFields != "[]") text.Append($", \"matchFields\": {matchFields}");
				if (resultFields != "[]") text.Append($", \"resultFields\": {resultFields}");
			}
			if (rule.Produces is not null)
			{
				text.Append($", \"produces\": \"{Escape(rule.Produces)}\"");
				text.Append($", \"producesFrom\": \"{(rule.ProducesFromCode ? "code" : "data")}\"");
			}

			// The old value is the subscript, so the array is written as one and position says
			// which old value each entry is for.
			if (rule.RemapValues.Count > 0)
			{
				text.Append(", \"remaps\": [");
				for (int k = 0; k < rule.RemapValues.Count; k++)
				{
					text.Append(k == 0 ? "" : ", ");
					text.Append($"[{string.Join(", ",
						rule.RemapValues[k].Select(v => $"\"{Escape(v)}\""))}]");
				}
				text.Append("]");
			}
			// A nested rule says whose it is. Flattening the list to read top down otherwise loses
			// the one thing the nesting carried: that these sixteen steps are one flattening of one
			// block rather than sixteen unrelated ones.
			if (parent is not null) text.Append($", \"of\": \"0x{parent.Address:X}\"");
			text.Append($", \"address\": \"0x{rule.Address:X}\"");
			text.Append(" }");
			text.Append(i == steps.Count - 1 ? "\n" : ",\n");
		}
		return text.Append("\t]\n}\n").ToString();
	}

	/// <summary>
	///     A rule without its revision, so a nested rule and the rule holding it print the same.
	///     "produces" says what the block becomes, and where that was read: a flattening captures
	///     it beside the rule, a rename has it only in the code the rule points at, and calling
	///     both the same thing would hide that one of them came out of an instruction.
	/// </summary>
	private static string Body(UpgradeSchema rule)
	{
		var text = new StringBuilder();
		// A rule that selects its block by pattern says so, because most of those patterns spell
		// a whole block id and would otherwise read as one that was named.
		text.Append(rule.Subject is null ? ""
			: rule.SubjectIsPattern ? $"\"blockPattern\": \"{Escape(rule.Subject)}\", "
			: $"\"block\": \"{Escape(rule.Subject)}\", ");
		text.Append($"\"address\": \"0x{rule.Address:X}\", ");
		text.Append($"\"old\": {Side(rule, 0)}, ");
		text.Append($"\"new\": {Side(rule, 1)}");
		if (rule.Produces is not null)
		{
			text.Append($", \"produces\": \"{Escape(rule.Produces)}\"");
			text.Append($", \"producesFrom\": \"{(rule.ProducesFromCode ? "code" : "data")}\"");
		}
		return text.ToString();
	}

	/// <summary>
	///     The entries of one side that constrain a value, as a map of property to value.
	///     A side holds two different kinds of entry and they were being written into one object,
	///     which put the same property in twice: once carrying its value and once bare. JSON keeps
	///     the last of a repeated key, so the bare one silently overwrote the real one and every
	///     rule that constrained a property lost exactly the thing it constrained. They are two
	///     facts and they are written as two now.
	///     The nesting under "states" is gone with it. It was presentation, and in a file meant to
	///     be read top down a step that is one line beats a step that has to be descended into.
	/// </summary>
	private static string Side(UpgradeSchema rule, int which)
	{
		if (rule.Tables.Count <= which) return "{}";

		var text = new StringBuilder("{");
		bool first = true;
		foreach (var entry in rule.Tables[which])
		{
			if (entry.Key is null || entry.Kind is UpgradeValueKind.None) continue;
			text.Append(first ? " " : ", ");
			text.Append($"\"{Escape(entry.Key)}\": {Value(entry)}");
			first = false;
		}
		return text.Append(first ? "}" : " }").ToString();
	}

	/// <summary>
	///     The entries of one side that name a property without constraining it, in the server's
	///     order. "states" is dropped: it is the compound the others sit in, not a property.
	/// </summary>
	private static string Fields(UpgradeSchema rule, int which)
	{
		if (rule.Tables.Count <= which) return "[]";

		var names = new List<string>();
		foreach (var entry in rule.Tables[which])
		{
			if (entry.Key is null or "states" || entry.Kind is not UpgradeValueKind.None) continue;
			if (!names.Contains(entry.Key)) names.Add(entry.Key);
		}
		return names.Count == 0 ? "[]" : $"[{string.Join(", ", names.Select(n => $"\"{Escape(n)}\""))}]";
	}
	/// <summary>
	///     One entry's value, written as the shape the server holds rather than as a string or a
	///     null. A number stays a number, a pattern says it is one, and an entry with no value at
	///     all is null, which now means what it says: the rule names the property and constrains
	///     nothing about its value.
	/// </summary>
	private static string Value(UpgradeEntry entry)
	{
		switch (entry.Kind)
		{
			case UpgradeValueKind.Number:
				return entry.Number.ToString(CultureInfo.InvariantCulture);

			case UpgradeValueKind.Text:
				return $"\"{Escape(entry.Text)}\"";

			case UpgradeValueKind.Pattern:
				return entry.Text is null
					? "{ \"pattern\": null }"
					: $"{{ \"pattern\": \"{Escape(entry.Text)}\" }}";

			case UpgradeValueKind.Table:
				var text = new StringBuilder("{");
				foreach (var row in entry.Table)
				{
					text.Append(text.Length == 1 ? " " : ", ");
					text.Append($"\"{row.Key}\": \"{Escape(row.Value)}\"");
				}
				return text.Append(" }").ToString();

			// A size with no shape behind it. Said out loud, because an entry nobody can read is
			// not the same fact as an entry holding nothing, and the two read alike as null.
			case UpgradeValueKind.Unknown:
				return $"{{ \"unread\": {entry.Size} }}";

			default:
				return "null";
		}
	}

	private static string Escape(string value)
	{
		return value.Replace("\\", "\\\\").Replace("\"", "\\\"");
	}

	/// <summary>A string, or null when the server does not hold one. Absent is a value.</summary>
	private static string Text(string value)
	{
		return value is null ? "null" : $"\"{Escape(value)}\"";
	}
	private static string WriteProperties(IReadOnlyList<BlockProperties> blocks,
		IReadOnlyDictionary<ulong, string> classNames)
	{
		var text = new StringBuilder("[\n");
		for (int i = 0; i < blocks.Count; i++)
		{
			var block = blocks[i];
			text.Append("\t{\n");
			text.Append($"\t\t\"name\": \"{block.Name}\",\n");
			text.Append($"\t\t\"nameHash\": \"0x{block.NameHash:X16}\",\n");
			text.Append($"\t\t\"legacyId\": {block.LegacyId},\n");
			// The pre-flattening name, and the creative tab, which not every block has.
			text.Append($"\t\t\"serializationId\": {Text(block.SerializationId)},\n");
			text.Append($"\t\t\"creativeGroup\": {Text(block.CreativeGroup)},\n");
			text.Append($"\t\t\"hardness\": {Number(block.Hardness)},\n");
			text.Append($"\t\t\"explosionResistance\": {Number(block.ExplosionResistance)},\n");
			text.Append($"\t\t\"friction\": {Number(block.Friction)},\n");
			text.Append($"\t\t\"thickness\": {Number(block.Thickness)},\n");
			text.Append($"\t\t\"translucency\": {Number(block.Translucency)},\n");
			text.Append($"\t\t\"burnOdds\": {block.BurnOdds},\n");
			text.Append($"\t\t\"flameOdds\": {block.FlameOdds},\n");
			text.Append($"\t\t\"isSolid\": {Boolean(block.IsSolid)},\n");
			text.Append($"\t\t\"canContainLiquidSource\": {Boolean(block.CanContainLiquidSource)},\n");
			text.Append($"\t\t\"liquidReactionOnTouch\": \"{block.LiquidReactionOnTouch}\",\n");
			text.Append($"\t\t\"tintMethod\": \"{block.TintMethod}\",\n");
			text.Append($"\t\t\"mapColor\": \"{block.MapColor}\",\n");
			// The implementing class: the method table is the only type identity this binary
			// carries, and the name is derived from what the class holds so it survives a rebuild.
			text.Append($"\t\t\"class\": \"{Escape(classNames.GetValueOrDefault(block.Vtable, "block"))}\",\n");
			text.Append($"\t\t\"classPointer\": \"0x{block.Vtable:X}\",\n");
			// Where this was read. Only meaningful while that server lives, and the thing that
			// makes a follow up probe exact instead of re-finding the name and hoping.
			text.Append($"\t\t\"address\": \"0x{block.Address:X}\",\n");
			text.Append("\t\t\"tags\": [");
			for (int t = 0; t < block.Tags.Count; t++)
			{
				text.Append(t == 0 ? "" : ", ");
				text.Append($"\"{Escape(block.Tags[t])}\"");
			}
			text.Append("]\n");
			text.Append("\t}");
			text.Append(i == blocks.Count - 1 ? "\n" : ",\n");
		}
		return text.Append("]\n").ToString();
	}

	private static string Number(float value)
	{
		return float.IsFinite(value) ? value.ToString("0.######", CultureInfo.InvariantCulture) : "null";
	}

	private static string Boolean(bool value)
	{
		return value ? "true" : "false";
	}
}
