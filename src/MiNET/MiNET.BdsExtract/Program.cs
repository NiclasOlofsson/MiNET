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
///     reconstructed from the outside by people who could not read them.
/// </summary>
public static class Program
{
	public static int Main(string[] args)
	{
		// The probe half runs a server rather than reading one, so it takes the rest of the command
		// line and returns: none of the memory-side options apply to it.
		if (args.Length > 0 && args[0] == "--probe") return BlockProbe.Run(args[1..]);

		string outputDirectory = DefaultOutputDirectory();
		string pathFilter = null;

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
				default:
					Console.Error.WriteLine("usage: MiNET.BdsExtract [--out <directory>] [--server <path fragment>]");
					Console.Error.WriteLine("       MiNET.BdsExtract --probe <bds directory> [output directory]");
					Console.Error.WriteLine();
					Console.Error.WriteLine("  --out     where to write the json files, default the Data folder in this project");
					Console.Error.WriteLine("  --server  which server to read when several are running, matched on");
					Console.Error.WriteLine(@"            executable path, for example --server bds\probe");
					return 2;
			}
		}

		try
		{
			return Run(outputDirectory, pathFilter);
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
	private static string DefaultOutputDirectory()
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

	private static int Run(string outputDirectory, string pathFilter)
	{
		using var server = BedrockProcess.Attach(pathFilter);
		Console.WriteLine($"reading pid {server.Id}");
		Console.WriteLine($"  {server.ExecutablePath}");

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

		var upgrades = BlockUpgradeReader.Read(server);
		Console.WriteLine($"upgrade rules: {upgrades.Count:N0} blocks, "
						+ $"{upgrades.Sum(u => u.RenamedProperties.Count):N0} property renames");

		Directory.CreateDirectory(outputDirectory);
		string palettePath = Path.Combine(outputDirectory, "block_palette.json");
		string propertiesPath = Path.Combine(outputDirectory, "block_properties.json");
		string upgradePath = Path.Combine(outputDirectory, "block_upgrade_rules.json");
		File.WriteAllText(palettePath, WritePalette(palette, report), new UTF8Encoding(false));
		File.WriteAllText(propertiesPath, WriteProperties(blocks), new UTF8Encoding(false));
		File.WriteAllText(upgradePath, WriteUpgrades(upgrades), new UTF8Encoding(false));
		Console.WriteLine($"written {palettePath}");
		Console.WriteLine($"written {propertiesPath}");
		Console.WriteLine($"written {upgradePath}");

		return report.Passed ? 0 : 1;
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

	private static string WriteUpgrades(IReadOnlyList<BlockUpgradeRule> rules)
	{
		// A rule names the properties it touches. What those properties turn into is not read
		// yet, so the file says which are renamed and leaves the rest listed rather than
		// implying it knows their new values.
		var text = new StringBuilder("[\n");
		for (int i = 0; i < rules.Count; i++)
		{
			var rule = rules[i];
			text.Append("\t{\n");
			text.Append($"\t\t\"block\": \"{rule.Block}\",\n");
			text.Append($"\t\t\"properties\": [{string.Join(", ", rule.Properties.Select(p => $"\"{p}\""))}],\n");
			text.Append("\t\t\"renamedProperties\": {");
			text.Append(string.Join(",", rule.RenamedProperties.Select(r => $" \"{r.Key}\": \"{r.Value}\"")));
			text.Append(rule.RenamedProperties.Count == 0 ? "}\n" : " }\n");
			text.Append("\t}");
			text.Append(i == rules.Count - 1 ? "\n" : ",\n");
		}
		return text.Append("]\n").ToString();
	}

	private static string WriteProperties(IReadOnlyList<BlockProperties> blocks)
	{
		var text = new StringBuilder("[\n");
		for (int i = 0; i < blocks.Count; i++)
		{
			var block = blocks[i];
			text.Append("\t{\n");
			text.Append($"\t\t\"name\": \"{block.Name}\",\n");
			text.Append($"\t\t\"nameHash\": \"0x{block.NameHash:X16}\",\n");
			text.Append($"\t\t\"legacyId\": {block.LegacyId},\n");
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
			text.Append($"\t\t\"mapColor\": \"{block.MapColor}\"\n");
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
