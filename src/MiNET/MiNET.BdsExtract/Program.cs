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
using System.Text.Json.Nodes;

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
		// One job: read the server and write the files. The only thing worth saying on the command
		// line is which server, because several run at once when every build is being checked, and
		// the one action that is not a read, bringing a server folder to the canonical config.
		if (args.Length > 0 && args[0] == "--prepare")
		{
			if (args.Length != 2)
			{
				Console.Error.WriteLine("usage: MiNET.BdsExtract --prepare <server folder>");
				return 2;
			}
			return WorldConfig.Prepare(args[1]);
		}

		string outputDirectory = DefaultOutputDirectory();
		string pathFilter = null;

		for (int i = 0; i < args.Length; i++)
		{
			if (args[i] == "--server" && i + 1 < args.Length)
			{
				pathFilter = args[++i];
				continue;
			}

			Console.Error.WriteLine("usage: MiNET.BdsExtract [--server <path fragment>]");
			Console.Error.WriteLine();
			Console.Error.WriteLine("  --server   which server to read when several are running, matched on");
			Console.Error.WriteLine(@"             executable path, for example --server server-1.26.20.5");
			Console.Error.WriteLine("  --prepare <server folder>  write the canonical config from Assets into a server folder");
			return 2;
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
	/// <summary>
	///     How many blocks have to close the round trip before its answer is used. A handful of
	///     unrelated pointers can close it once or twice by chance; a dozen cannot.
	/// </summary>
	private const int MinimumRoundTrips = 8;

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

	private static int Run(string outputDirectory, string pathFilter)
	{
		using var server = BedrockProcess.Attach(pathFilter);
		Console.WriteLine($"reading pid {server.Id}");

		Console.WriteLine(server.BuildVersion is null
			? "  build      not stated by the executable and not named by its folder"
			: $"  build      {server.BuildVersion}");
		Console.WriteLine($"  {server.ExecutablePath}");

		// Before anything is read, not after. The palette a server holds is filtered by its
		// world's experiments and positioned by the id scheme, and neither mistake is visible
		// in the output.
		if (!WorldConfig.Check(WorldConfig.Read(server.ExecutablePath), true)) return 1;
		Console.WriteLine();

		// The block layout is measured from this server before anything is swept. A block and its
		// default state point at each other, and where those two pointers sit moves between builds:
		// 224/352/104 on the versions we target, 200/448/96 on 1.26.3.1. Reading with the wrong
		// ones finds no blocks at all, so they are taken from the server rather than compiled in.
		(ulong lead, int stateAt, int backAt, int roundTrips) =
			RegistryDiscovery.DeriveBlockLayout(server, RegistryDiscovery.NamedThings(server));
		if (roundTrips >= MinimumRoundTrips)
		{
			MemoryLayout.UseMeasured((int) lead, stateAt, backAt);
			Console.WriteLine($"block layout measured on {roundTrips} round trips: name at +{lead}, "
							+ $"state pointer at name+{stateAt}, back pointer at state+{backAt}");
		}
		else
		{
			Console.WriteLine($"block layout not measurable ({roundTrips} round trips), keeping "
							+ $"name at +{MemoryLayout.NameInsideLegacy}, state pointer at name+{MemoryLayout.DefaultStatePointer}, "
							+ $"back pointer at state+{MemoryLayout.BlockLegacyPointer}");
		}

		var blocks = BlockRegistry.Read(server);
		Console.WriteLine($"blocks found: {blocks.Count:N0}");


		// Where a block keeps its own states, measured from the blocks just found: every state in
		// that table names the block it belongs to, so the position where they all do is the table.
		(int nbtAt, int nbtAgreed) = RegistryDiscovery.DeriveStateNbt(server, blocks);
		if (nbtAt >= 0)
		{
			MemoryLayout.UseMeasuredField("stateNbt", nbtAt);
			Console.WriteLine($"  the state's own description is at state+{nbtAt}, on {nbtAgreed:N0} states carrying a name key");
		}
		else
		{
			Console.WriteLine("  no state carries a map with a name key, so what a state IS cannot be read");
		}

		(int propertiesAt, int propertiesAgreed) = RegistryDiscovery.DerivePropertyTable(server, blocks);
		if (propertiesAt >= 0)
		{
			MemoryLayout.UseMeasuredField("properties", propertiesAt);
			Console.WriteLine($"  properties measured at +{propertiesAt}, on {propertiesAgreed:N0} blocks carrying one");
		}
		else
		{
			Console.WriteLine("  no position holds a property table, so no block's properties can be read");
		}

		(int componentsAt, int componentIdsAt, int componentsAgreed) = RegistryDiscovery.DeriveComponents(server, blocks);
		if (componentsAt >= 0)
		{
			MemoryLayout.UseMeasuredField("components", componentsAt);
			MemoryLayout.UseMeasuredField("componentIds", componentIdsAt);
			Console.WriteLine($"  components measured at +{componentsAt} with their ids at +{componentIdsAt}, "
							+ $"on {componentsAgreed:N0} blocks whose two vectors hold the same count");
		}
		else
		{
			Console.WriteLine("  no two vectors run in step, so components cannot be read");
		}

		(int tagsAt, int tagStride, int tagsAgreed) = RegistryDiscovery.DeriveTags(server, blocks);
		if (tagsAt >= 0)
		{
			MemoryLayout.UseMeasuredField("tags", tagsAt);
			MemoryLayout.UseMeasuredField("tagStride", tagStride);
			Console.WriteLine($"  tags measured at name+{tagsAt}, {tagStride} bytes a tag, "
							+ $"on {tagsAgreed:N0} blocks whose every record names itself");
		}
		else
		{
			Console.WriteLine("  no position holds a run of tags, so no block's tags can be read");
		}

		// Read again, now that where a block keeps its properties, components and tags is measured
		// rather than assumed: the first pass could not fill them from positions it had not found.
		blocks = BlockRegistry.Read(server);

		(int statesAt, int statesAgreed) = RegistryDiscovery.DeriveStateTable(server, blocks);
		if (statesAt >= 0)
		{
			MemoryLayout.UseMeasuredField("states", statesAt);
			Console.WriteLine($"  states measured at +{statesAt}, on {statesAgreed:N0} blocks whose states name them back");
		}
		if (blocks.Count == 0)
		{
			Console.Error.WriteLine("no blocks found; the field offsets probably do not match this server build");
			return 1;
		}

		// Every value is looked for on this server, on every build without exception. Nothing is
		// published and nothing is taken from the reference but the values themselves.
		if (!Report("block members", BlockMemberDerivation.CheckBlocks(server, blocks, LargestBlock(server, blocks))))
		{
			return 2;
		}

		// Only now can a block's own values be read, because only now is there anywhere to read
		// them from. The sweep answers what a block is and where it is; what it holds comes after
		// the layout has been measured against it.
		BlockRegistry.ReadValues(server, blocks);

		BlockPalette.VectorHeader? found = BlockPalette.FindPalette(server, blocks);
		if (found is null)
		{
			Console.Error.WriteLine("no container holds every block's default state, so the palette was not found");
			return 1;
		}
		BlockPalette.VectorHeader header = found.Value;
		Console.WriteLine($"  the palette holds {header.Count:N0} states");

		// Where a state keeps its own light, from the states the reference already knows. The block
		// carries light of its own and the two disagree wherever light depends on state, so this is
		// measured against what each state is rather than against what its block says.
		(List<(ulong, int)> emission, List<(ulong, int)> dampening) = BlockPalette.KnownLight(server, header);
		foreach ((string field, List<(ulong Address, int Value)> wanted) in
			new[] {("lightEmission", emission), ("lightDampening", dampening)})
		{
			(int at, int agreed, int population) = RegistryDiscovery.DeriveStateByte(server, wanted);
			if (at >= 0)
			{
				MemoryLayout.UseMeasuredField(field, at);
				Console.WriteLine($"  {field} measured at state+{at}, on {agreed:N0} of {population:N0} known states");
			}
			else
			{
				Console.WriteLine($"  no position holds {field}, keeping state+"
								+ (field == "lightEmission" ? MemoryLayout.StateLightEmission : MemoryLayout.StateLightDampening));
			}
		}

		// Where a state keeps its network id, from the palette itself: with hashed ids off it is
		// the state's own place in the list, so the position holding that for every state is the
		// field. Nothing answers when the server numbers states by hash instead, and the count
		// below says which of the two happened.
		(int idAt, int idAgreed) = BlockPalette.DeriveNetworkId(server, header);
		if (idAt >= 0 && idAgreed == header.Count)
		{
			MemoryLayout.UseMeasuredField("networkId", idAt);
			Console.WriteLine($"  the network id is at state+{idAt}, holding its own place in the palette on all {idAgreed:N0} states");
		}
		else
		{
			Console.WriteLine($"  no position holds every state's place in the palette: the best is state+{idAt} "
							+ $"on {idAgreed:N0} of {header.Count:N0}");
		}

		var palette = BlockPalette.Read(server, header);
		Console.WriteLine($"palette read from 0x{header.Begin:X}");

		// The state class gets the same treatment as the block class, and on the same terms: every
		// value looked for on this server, none of them taken from anywhere else.
		int stateSize = StateClassSize(server, palette);
		if (stateSize == 0)
		{
			Console.Error.WriteLine("the state class does not record its own size, so no state value can be measured");
			return 1;
		}

		Console.WriteLine($"  the state class records itself as {stateSize} bytes");
		if (!Report("state members", BlockMemberDerivation.CheckStates(server, palette, stateSize))) return 2;

		// The palette decides what a block is. The memory sweep also turns up features, biomes and
		// other named things that are not blocks, and there is no need to guess which: anything the
		// palette never refers to is not a block.
		var inPalette = palette.Select(e => e.Name).ToHashSet(StringComparer.Ordinal);
		int swept = blocks.Count;
		blocks = blocks.Where(b => inPalette.Contains(b.Name)).ToList();
		Console.WriteLine($"blocks in the palette: {blocks.Count:N0} of {swept:N0} named objects swept");

		var report = ExtractionReport.Check(palette);
		report.WriteTo(Console.Out);


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

		// Read only to resolve each block's geometry, which comes from the component the id names.
		// Nothing about the components themselves is reported or published: their ids are
		// per-instance registration numbers that differ between runs of the same build, so a count
		// of how many were "identified" describes this heap rather than the game.
		var held = ComponentReader.Read(server, blocks, palette);

		var ranges = BlockStateRanges.Read(palette);
		int dependent = ranges.Count(r => !r.Independent);
		Console.WriteLine($"state ranges: {ranges.Count:N0} blocks, "
						+ $"{ranges.Sum(r => r.Properties.Count):N0} properties, "
						+ $"{dependent:N0} whose states are fewer than their combinations");

		Directory.CreateDirectory(outputDirectory);
		string blockPath = Path.Combine(outputDirectory, "blocks.json");
		string statePath = Path.Combine(outputDirectory, "block_states.json");
		string upgradePath = Path.Combine(outputDirectory, "block_upgrade_rules.json");

		// Every member of the block class, each under the class's own name, read from where the
		// published layout says it is. This is the whole object rather than the values this tool
		// happened to name, so what nothing reads is a member saying so instead of an absence.
		File.WriteAllText(blockPath, BlockDocument.Serialize(BlockDocument.Blocks(server, palette, blocks, classNames,
			ranges.ToDictionary(r => r.Name, StringComparer.Ordinal),
			stateProperties.ToDictionary(p => p.Name, p => p.Id, StringComparer.Ordinal),
			legacyStates.ToDictionary(t => t.Name, StringComparer.Ordinal), held)), new UTF8Encoding(false));
		File.WriteAllText(statePath, BlockDocument.Serialize(BlockDocument.States(server, palette, blocks, report)), new UTF8Encoding(false));
		File.WriteAllText(upgradePath, BlockDocument.Serialize(Upgrades(palette, rules, renamedIds)), new UTF8Encoding(false));
		Console.WriteLine($"written {blockPath}");
		Console.WriteLine($"written {statePath}");
		Console.WriteLine($"written {upgradePath}");

		// The items, from the same process, in the same run. One command, one set of files, and
		// the run fails if either half does.
		Console.WriteLine();
		int items = ItemRegistry.Run(server);
		return report.Passed && items == 0 ? 0 : 1;
	}

	/// <summary>
	///     How far the member search looks, which is as far as the largest block object goes. The
	///     classes state their own sizes, so this is measured rather than chosen.
	/// </summary>
	/// <summary>
	///     Whether the layout the reference states still reads this server, member by member. A
	///     member that has slipped is said out loud and the run goes on; one that is broken is not
	///     the field any more, so there is no point reading anything past it.
	/// </summary>
	internal static bool Report(string what, IReadOnlyList<BlockMemberDerivation.Found> checks)
	{
		int holds = checks.Count(f => f.Verdict == BlockMemberDerivation.Verdict.Holds);
		int slipped = checks.Count(f => f.Verdict == BlockMemberDerivation.Verdict.Slipped);
		int broken = checks.Count(f => f.Verdict == BlockMemberDerivation.Verdict.Broken);
		int unchecked_ = checks.Count(f => f.Verdict == BlockMemberDerivation.Verdict.Unchecked);
		Console.WriteLine($"{what} against {BlockMembers.Build}: {holds} hold, {slipped} slipped, "
						+ $"{broken} broken, {unchecked_} with nothing to check them against");

		foreach (BlockMemberDerivation.Found f in checks)
		{
			switch (f.Verdict)
			{
				case BlockMemberDerivation.Verdict.Holds:
					break;
				case BlockMemberDerivation.Verdict.Unchecked:
					Console.WriteLine($"  {f.Name,-52} at +{f.At}, the reference states no value");
					break;
				case BlockMemberDerivation.Verdict.Slipped:
					Console.WriteLine($"  {f.Name,-52} at +{f.At}, SLIPPED to {f.Share:P1}, "
									+ $"{f.Held:N0} of {f.Of:N0}");
					break;
				default:
					Console.Error.WriteLine($"  {f.Name,-52} at +{f.At}, BROKEN at {f.Share:P1}, "
									+ $"{f.Held:N0} of {f.Of:N0}");
					break;
			}
		}

		if (broken == 0) return true;

		Console.Error.WriteLine();
		Console.Error.WriteLine($"{broken} member(s) of the {what.Split(' ')[0]} class read something else on this");
		Console.Error.WriteLine("build. Correct their offsets in the reference and run again. Nothing past");
		Console.Error.WriteLine("this point would be worth reading.");
		return false;
	}

	/// <summary>
	///     How big a state is, from the class itself: the size is baked into the deleting destructor
	///     the method table points at. Every state in the palette is the same class, so a palette
	///     holding two method tables is a reason to stop rather than to pick one.
	///     <para>
	///         A destructor destroys its members before itself, so it hands out their sizes first:
	///         this one offers 112, 296, 64, 56 and only the second is the class. Which one it is
	///         comes from what this run has already measured inside a state, not from its place in
	///         that list. A class cannot be smaller than a field read out of it, so the answer is the
	///         smallest size that still covers the furthest one.
	///     </para>
	/// </summary>
	private static int StateClassSize(BedrockProcess process, IReadOnlyList<PaletteEntry> palette)
	{
		var word = new byte[8];
		var tables = new HashSet<ulong>();
		foreach (PaletteEntry entry in palette) tables.Add(process.ReadUInt64(entry.Address, word));
		if (tables.Count != 1)
		{
			Console.Error.WriteLine($"the palette holds {tables.Count} method tables, so the states are not one class");
			return 0;
		}

		int measured = Math.Max(MemoryLayout.StateLightEmission + 1, MemoryLayout.StateLightDampening + 1);
		List<int> sizes = ItemRegistry.ClassSizeCandidates(process, tables.First());
		int size = sizes.Where(s => s >= measured).DefaultIfEmpty(0).Min();
		Console.WriteLine($"  the state destructor hands out {string.Join(", ", sizes)}; "
						+ $"a state is read as far as +{measured}, so the class is {size}");
		return size;
	}

	private static int LargestBlock(BedrockProcess process, IReadOnlyList<BlockProperties> blocks)
	{
		var word = new byte[8];
		int largest = 0;
		foreach (BlockProperties block in blocks)
		{
			largest = Math.Max(largest, ObjectLayout.Measure(process, block.Address, word));
		}
		return largest;
	}

	/// <summary>
	///     The upgrade rules as the steps they are, in the order the server runs them.
	/// </summary>
	private static JsonObject Upgrades(IReadOnlyList<PaletteEntry> palette,
		IReadOnlyList<UpgradeSchema> schemas,
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

		var document = new JsonObject();
		BlockDocument.Version(document, palette);
		document["steps"] = steps.Count;

		var renames = new JsonObject();
		foreach (var rename in renamedIds) renames[rename.Key] = rename.Value;
		document["renameIds"] = renames;

		var upgrade = new JsonArray();
		foreach (var (version, rule, parent) in steps)
		{
			var step = new JsonObject
			{
				["version"] = $"{(version >> 24) & 0xFF}.{(version >> 16) & 0xFF}."
							+ $"{(version >> 8) & 0xFF}.{version & 0xFF}"
			};

			// A child names the block its parent names, so the step stands on its own.
			string subject = rule.Subject ?? parent?.Subject;
			bool pattern = rule.Subject is not null ? rule.SubjectIsPattern : parent?.SubjectIsPattern ?? false;
			if (subject is not null) step[pattern ? "blockPattern" : "block"] = subject;

			step["match"] = Side(rule, 0);
			step["result"] = Side(rule, 1);

			// The bare property names, written once when both sides list the same ones, which is
			// almost always: a rule names the properties it touches, not two different sets.
			JsonArray matchFields = Fields(rule, 0);
			JsonArray resultFields = Fields(rule, 1);
			if (JsonNode.DeepEquals(matchFields, resultFields))
			{
				if (matchFields.Count > 0) step["fields"] = matchFields;
			}
			else
			{
				if (matchFields.Count > 0) step["matchFields"] = matchFields;
				if (resultFields.Count > 0) step["resultFields"] = resultFields;
			}

			if (rule.Produces is not null)
			{
				step["produces"] = rule.Produces;
				step["producesFrom"] = rule.ProducesFromCode ? "code" : "data";
			}

			// The old value is the subscript, so the list is written as one and position says which
			// old value each entry is for.
			if (rule.RemapValues.Count > 0)
			{
				step["remaps"] = new JsonArray(rule.RemapValues
					.Select(row => (JsonNode) new JsonArray(row.Select(v => (JsonNode) v).ToArray()))
					.ToArray());
			}

			// A nested rule says whose it is, by that rule's place in this list rather than by its
			// address. Flattening otherwise loses the one thing the nesting carried: that these
			// sixteen steps are one flattening of one block rather than sixteen unrelated ones.
			// The address itself is not written: it is different on every run of the same server,
			// so a file that carries it differs every time even when nothing about the game did.
			if (parent is not null) step["of"] = steps.FindIndex(s => ReferenceEquals(s.Rule, parent));
			upgrade.Add(step);
		}

		document["upgrade"] = upgrade;
		return document;
	}

	/// <summary>
	///     The entries of one side that constrain a value, as a map of property to value.
	///     A side holds two different kinds of entry and they were being written into one object,
	///     which put the same property in twice: once carrying its value and once bare. JSON keeps
	///     the last of a repeated key, so the bare one silently overwrote the real one and every
	///     rule that constrained a property lost exactly the thing it constrained. They are two
	///     facts and they are written as two now.
	/// </summary>
	private static JsonObject Side(UpgradeSchema rule, int which)
	{
		var side = new JsonObject();
		if (rule.Tables.Count <= which) return side;

		foreach (var entry in rule.Tables[which])
		{
			if (entry.Key is null || entry.Kind is UpgradeValueKind.None) continue;
			side[entry.Key] = Value(entry);
		}

		return side;
	}

	/// <summary>
	///     The entries of one side that name a property without constraining it, in the server's
	///     order. "states" is dropped: it is the compound the others sit in, not a property.
	/// </summary>
	private static JsonArray Fields(UpgradeSchema rule, int which)
	{
		var names = new List<string>();
		if (rule.Tables.Count > which)
		{
			foreach (var entry in rule.Tables[which])
			{
				if (entry.Key is null or "states" || entry.Kind is not UpgradeValueKind.None) continue;
				if (!names.Contains(entry.Key)) names.Add(entry.Key);
			}
		}

		return new JsonArray(names.Select(n => (JsonNode) n).ToArray());
	}

	/// <summary>
	///     One entry's value, as the shape the server holds rather than as a string or a null. A
	///     number stays a number, a pattern says it is one, and an entry with no value at all is
	///     null, which means what it says: the rule names the property and constrains nothing about
	///     its value.
	/// </summary>
	private static JsonNode Value(UpgradeEntry entry)
	{
		switch (entry.Kind)
		{
			case UpgradeValueKind.Number:
				return JsonValue.Create(entry.Number);

			case UpgradeValueKind.Text:
				return entry.Text;

			case UpgradeValueKind.Pattern:
				return new JsonObject { ["pattern"] = entry.Text };

			case UpgradeValueKind.Table:
				var table = new JsonObject();
				foreach (var row in entry.Table)
				{
					table[row.Key.ToString(CultureInfo.InvariantCulture)] = row.Value;
				}
				return table;

			// A size with no shape behind it. Said out loud, because an entry nobody can read is
			// not the same fact as an entry holding nothing, and the two read alike as null.
			case UpgradeValueKind.Unknown:
				return new JsonObject { ["unread"] = entry.Size };

			default:
				return null;
		}
	}

	private static string Escape(string value)
	{
		return value.Replace("\\", "\\\\").Replace("\"", "\\\"");
	}

	private static string Boolean(bool value)
	{
		return value ? "true" : "false";
	}
}
