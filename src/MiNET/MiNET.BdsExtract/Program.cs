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
///     Reads a running Bedrock Dedicated Server and writes what it finds as JSON.
///     Two outputs, and they answer different questions.
///     block_palette.json is the palette: every block state the server knows, in the server's own
///     order. Position in that list is the palette index, which is the runtime id when the server
///     is not using hashed ids. Each entry also carries its network id, which is the runtime id
///     when it is. Nothing about the order is computed here, it is the order the server holds.
///     block_properties.json is per block, not per state: hardness, friction, light and the rest.
///     Neither file describes what a state actually is, the "facing east, upside down" part. That
///     is deliberate. Every entry carries a network id, and that id is a hash of the state, so a
///     separate program that knows the states can hash them and join on that number. Nothing about
///     ordering has to pass between the two sides, so neither can corrupt the other.
/// </summary>
public static class Program
{
	public static int Main(string[] args)
	{
		// The probe half runs a server rather than reading one, so it takes the rest of the command
		// line and returns: none of the memory-side options apply to it.
		if (args.Length > 0 && args[0] == "--probe") return BlockProbe.Run(args[1..]);

		string outputDirectory = ".";
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
					Console.Error.WriteLine("  --out     where to write the two json files, default the working directory");
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

		Directory.CreateDirectory(outputDirectory);
		string palettePath = Path.Combine(outputDirectory, "block_palette.json");
		string propertiesPath = Path.Combine(outputDirectory, "block_properties.json");
		File.WriteAllText(palettePath, WritePalette(palette, report), new UTF8Encoding(false));
		File.WriteAllText(propertiesPath, WriteProperties(blocks), new UTF8Encoding(false));
		Console.WriteLine($"written {palettePath}");
		Console.WriteLine($"written {propertiesPath}");

		return report.Passed ? 0 : 1;
	}

	private static string WritePalette(IReadOnlyList<PaletteEntry> palette, ExtractionReport report)
	{
		// State the id scheme in the file itself. Without it a reader cannot tell whether
		// networkId is a hash or a repeat of index, and both look equally reasonable.
		var text = new StringBuilder("{\n");
		text.Append($"  \"networkIdsAreHashes\": {Boolean(report.NetworkIdsAreHashes)},\n");
		text.Append($"  \"count\": {palette.Count},\n");
		text.Append("  \"palette\": [\n");
		for (int i = 0; i < palette.Count; i++)
		{
			var entry = palette[i];
			text.Append("    { ");
			text.Append($"\"index\": {entry.Index}, ");
			text.Append($"\"name\": \"{entry.Name}\", ");
			text.Append($"\"nameHash\": \"0x{entry.NameHash:X16}\", ");
			text.Append($"\"networkId\": {entry.NetworkId}, ");
			text.Append($"\"legacyId\": {entry.LegacyId}");
			text.Append(" }");
			text.Append(i == palette.Count - 1 ? "\n" : ",\n");
		}
		return text.Append("]\n").ToString();
	}

	private static string WriteProperties(IReadOnlyList<BlockProperties> blocks)
	{
		var text = new StringBuilder("[\n");
		for (int i = 0; i < blocks.Count; i++)
		{
			var block = blocks[i];
			text.Append("  {\n");
			text.Append($"    \"name\": \"{block.Name}\",\n");
			text.Append($"    \"nameHash\": \"0x{block.NameHash:X16}\",\n");
			text.Append($"    \"legacyId\": {block.LegacyId},\n");
			text.Append($"    \"hardness\": {Number(block.Hardness)},\n");
			text.Append($"    \"explosionResistance\": {Number(block.ExplosionResistance)},\n");
			text.Append($"    \"friction\": {Number(block.Friction)},\n");
			text.Append($"    \"thickness\": {Number(block.Thickness)},\n");
			text.Append($"    \"translucency\": {Number(block.Translucency)},\n");
			text.Append($"    \"lightEmission\": {block.LightEmission},\n");
			text.Append($"    \"lightDampening\": {block.LightDampening},\n");
			text.Append($"    \"burnOdds\": {block.BurnOdds},\n");
			text.Append($"    \"flameOdds\": {block.FlameOdds},\n");
			text.Append($"    \"isSolid\": {Boolean(block.IsSolid)},\n");
			text.Append($"    \"canContainLiquidSource\": {Boolean(block.CanContainLiquidSource)},\n");
			text.Append($"    \"liquidReactionOnTouch\": \"{block.LiquidReactionOnTouch}\",\n");
			text.Append($"    \"tintMethod\": \"{block.TintMethod}\",\n");
			text.Append($"    \"mapColor\": \"{block.MapColor}\"\n");
			text.Append("  }");
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
