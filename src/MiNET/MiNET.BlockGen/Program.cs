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
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using fNbt;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MiNET.BlockGen;

/// <summary>
///     Writes MiNET's block classes from the canonical palette.
///     Inputs are the palette data file and the listing of Blocks/*.cs. Nothing else. In
///     particular it does not reference MiNET: a generator that links against the code it emits
///     can only run while its own previous output compiles, which makes an empty or broken
///     generated file unrecoverable.
///     Two outputs, with one rule between them. A block with a hand-written .cs file never gets a
///     generated class. Every block gets a generated partial carrying its states. Nothing else
///     declares state members, so the halves cannot collide and no block can end up without a
///     GetState.
/// </summary>
public static class Program
{
	public static int Main(string[] args)
	{
		string repoRoot = args.Length > 0 ? args[0] : FindRepoRoot();
		string blocksDir = Path.Combine(repoRoot, "src", "MiNET", "MiNET", "Blocks");
		string itemsDir = Path.Combine(repoRoot, "src", "MiNET", "MiNET", "Items");
		string dataDir = Path.Combine(repoRoot, "src", "MiNET", "MiNET.BlockGen", "Data");
		string extractDir = Path.Combine(repoRoot, "src", "MiNET", "MiNET.BdsExtract", "Data");
		string blockStatesPath = Path.Combine(extractDir, "block_states.json");
		string blockTypesPath = Path.Combine(extractDir, "blocks.json");
		string blockCreativePath = Path.Combine(extractDir, "creative_items.json");
		string itemRowsPath = Path.Combine(extractDir, "items-runtime.json");
		string capturesDir = Path.Combine(repoRoot, "src", "MiNET", "MiNET.BlockGen", "Captures");
		string itemRegistryCapture = Path.Combine(capturesDir, "item_registry-1.26.50.26.bin");
		string creativeCapture = Path.Combine(capturesDir, "creative_content-1.26.50.26.bin");

		if (!Directory.Exists(blocksDir))
		{
			Console.Error.WriteLine($"Blocks directory not found: {blocksDir}");
			return 1;
		}

		foreach (string required in new[] {blockStatesPath, blockTypesPath, blockCreativePath, itemRowsPath})
		{
			if (File.Exists(required)) continue;
			Console.Error.WriteLine($"extraction data file not found: {required}");
			Console.Error.WriteLine("It is committed with MiNET.BdsExtract. Run that extraction to rebuild it.");
			return 1;
		}

		foreach (string required in new[] {itemRegistryCapture, creativeCapture})
		{
			if (File.Exists(required)) continue;
			Console.Error.WriteLine($"captured frame not found: {required}");
			Console.Error.WriteLine("It is the check every generated item tree and creative stack is measured against.");
			return 1;
		}

		BlockExtract extract = ReadBlockExtract(blockStatesPath, blockTypesPath);
		Console.WriteLine($"block source: {extractDir}");
		Console.WriteLine($"              BDS {extract.PublishedFor}, block state release {extract.BlockStateRelease}, network ids are hashes: {extract.NetworkIdsAreHashes}");

		if (!VerifyNetworkHashes(extract)) return 1;

		List<BlockState> palette = extract.Palette;
		Console.WriteLine($"palette: {palette.Count} states, {palette.Select(p => p.Name).Distinct().Count()} blocks");

		HashSet<string> handWritten = ReadHandWrittenClasses(blocksDir);
		HashSet<string> handImplemented = ReadHandImplementedStateClasses(blocksDir);
		Console.WriteLine($"hand-written classes: {handWritten.Count}, of which hand-implement GetState: {handImplemented.Count}");

		var byName = palette.GroupBy(p => p.Name).OrderBy(g => g.Key).ToList();

		Dictionary<string, int> baseIndex = VerifyPaletteLayout(palette, byName);
		if (baseIndex == null) return 1;

		Dictionary<string, string> familyBases = ReadFamilyBases(blockCreativePath, blocksDir, handWritten);
		Console.WriteLine($"family bases: {familyBases.Values.Distinct().Count()} bases covering {familyBases.Count} blocks");

		int classes = WriteBlockDataClasses(Path.Combine(blocksDir, "BlockData.generated.cs"), byName, handWritten, familyBases);
		Console.WriteLine($"BlockData.generated.cs: {classes} classes");

		// Where each block's states are declared. The creative families are one answer; a base the
		// code declares for itself is the other, and both hoist the same way. Assigning a base to a
		// GENERATED class stays creative-group-only above: this only decides who owns the states.
		var blockClassNames = new HashSet<string>(byName.Select(g => CodeName(g.Key.Replace("minecraft:", ""))));
		Dictionary<string, string> declaredBases = ReadDeclaredBases(blocksDir, handWritten, blockClassNames);
		var stateOwners = new Dictionary<string, string>(familyBases, StringComparer.OrdinalIgnoreCase);
		foreach (IGrouping<string, BlockState> group in byName)
		{
			if (stateOwners.ContainsKey(group.Key)) continue;
			if (declaredBases.TryGetValue(CodeName(group.Key.Replace("minecraft:", "")), out string owner)) stateOwners[group.Key] = owner;
		}

		Console.WriteLine($"state owners: {stateOwners.Values.Distinct().Count()} bases covering {stateOwners.Count} blocks");

		int partials = WritePartialBlocks(Path.Combine(blocksDir, "PartialBlocks.cs"), byName, handImplemented, baseIndex, stateOwners, extract.Properties);
		Console.WriteLine($"PartialBlocks.cs: {partials} partials");

		int entries = WriteBlockPalette(Path.Combine(blocksDir, "BlockPaletteData.generated.cs"), palette);
		Console.WriteLine($"BlockPaletteData.generated.cs: {entries} entries");

		var blockNames = new HashSet<string>(palette.Select(p => p.Name), StringComparer.OrdinalIgnoreCase);
		HashSet<string> handWrittenItems = ReadHandWrittenClasses(itemsDir, "ItemData.generated.cs", "ItemRegistryData.generated.cs");
		Console.WriteLine($"hand-written item classes: {handWrittenItems.Count}");

		List<ItemGenerator.ItemEntry> items = ItemGenerator.Run(extractDir, itemsDir, itemRegistryCapture, blockNames, handWrittenItems);

		// The creative catalog, regenerated from the extraction's own stacks through the same
		// registry ids, so it can never drift from the item registry the way the old hand-captured
		// file did. Data, not symbols, like the biome table below.
		var networkIdByName = items.ToDictionary(i => i.Name, i => i.NetworkId, StringComparer.OrdinalIgnoreCase);
		CreativeGenerator.Run(extractDir, Path.Combine(itemsDir, "Data", "creative_groups.json"), creativeCapture, networkIdByName);

		// Not code: this one emits our own data file, because biomes are a table nobody writes
		// against by symbol. Their file stays here, ours ships.
		Console.WriteLine($"biome source: {dataDir}");
		Console.WriteLine($"              {DescribeSource(dataDir)}");
		BiomeGenerator.Run(dataDir, Path.Combine(repoRoot, "src", "MiNET", "MiNET", "Data", "biome_definitions.json.gz"));

		return 0;
	}

	// A block's identity in the palette: its name, its legacy id if it still has one, and the
	// set of states for one permutation.
	private sealed record BlockState(string Name, int Id, int Version, List<(string Name, object Value)> States);

	/// <summary>
	///     The palette is not an arbitrary list, and the generated GetRuntimeId arithmetic depends
	///     on that. Two properties, both asserted here rather than assumed, because a future
	///     Bedrock drop that breaks either must fail the build instead of emitting wrong ids:
	///     each block owns one contiguous run, and inside it the states are a full cross product
	///     enumerated as a mixed-radix counter.
	///     The block order itself is the names sorted by unsigned FNV-1 64 (note: FNV-1, not the
	///     FNV-1a used for the permutation network hash), which is checked here too. It is not
	///     needed to emit ids, but a collision would leave two blocks' relative order undefined.
	/// </summary>
	private static Dictionary<string, int> VerifyPaletteLayout(List<BlockState> palette, List<IGrouping<string, BlockState>> byName)
	{
		var baseIndex = new Dictionary<string, int>();
		var lastIndex = new Dictionary<string, int>();
		var counts = new Dictionary<string, int>();
		for (int i = 0; i < palette.Count; i++)
		{
			string name = palette[i].Name;
			if (!baseIndex.ContainsKey(name)) baseIndex[name] = i;
			lastIndex[name] = i;
			counts[name] = counts.GetValueOrDefault(name) + 1;
		}

		var errors = new List<string>();

		foreach (string name in baseIndex.Keys)
		{
			if (lastIndex[name] - baseIndex[name] + 1 != counts[name])
			{
				errors.Add($"{name}: run is not contiguous ({counts[name]} states between index {baseIndex[name]} and {lastIndex[name]})");
			}
		}

		foreach (IGrouping<string, BlockState> group in byName)
		{
			List<BlockState> run = group.ToList();
			if (StateStrides(run, out _, out _) == null) errors.Add($"{group.Key}: states are not a positional encoding");
		}

		var byHash = new Dictionary<ulong, string>();
		foreach (string name in baseIndex.Keys)
		{
			ulong hash = Fnv1_64(name);
			if (byHash.TryGetValue(hash, out string other)) errors.Add($"FNV-1 64 collision: {name} and {other}");
			else byHash[hash] = name;
		}

		List<string> expectedOrder = baseIndex.Keys.OrderBy(Fnv1_64).ToList();
		List<string> actualOrder = baseIndex.OrderBy(kv => kv.Value).Select(kv => kv.Key).ToList();
		if (!expectedOrder.SequenceEqual(actualOrder))
		{
			Console.WriteLine("note: block order is no longer the unsigned FNV-1 64 name sort (ids are still emitted from the data)");
		}

		if (errors.Count > 0)
		{
			Console.Error.WriteLine($"palette layout assertions failed ({errors.Count}):");
			foreach (string e in errors.Take(20)) Console.Error.WriteLine($"  {e}");
			return null;
		}

		Console.WriteLine($"palette layout: {baseIndex.Count} contiguous runs, all positional, name order is the FNV-1 64 sort");
		return baseIndex;
	}

	/// <summary>
	///     Per state, the values it takes in run order and the offset at which it advances by one.
	///     The stride is read off the data rather than assumed, because the states are listed
	///     alphabetically in a palette entry and that is not the digit order: 236 of 1356 blocks
	///     differ (minecraft:fence_gate lists in_wall_bit first but its stride is 8).
	///     Returns null when the run is not a positional encoding at all.
	/// </summary>
	private static List<object>[] StateStrides(List<BlockState> run, out int[] strides, out string[] stateNames)
	{
		strides = null;
		stateNames = null;

		BlockState first = run[0];
		int n = first.States.Count;
		if (n == 0) return run.Count == 1 ? Array.Empty<List<object>>() : null;

		string[] names = first.States.Select(s => s.Name).ToArray();
		stateNames = names;
		var domains = new List<object>[n];
		strides = new int[n];

		for (int i = 0; i < n; i++)
		{
			int digit = i;
			if (run.Any(p => p.States.Count != n || p.States[digit].Name != names[digit])) return null;

			var values = new List<object>();
			foreach (BlockState p in run)
			{
				if (!values.Contains(p.States[i].Value)) values.Add(p.States[i].Value);
			}
			domains[i] = values;

			int stride = run.FindIndex(p => !Equals(p.States[i].Value, first.States[i].Value));
			strides[i] = stride < 0 ? run.Count : stride;
		}

		int product = 1;
		foreach (List<object> d in domains) product *= d.Count;
		if (product != run.Count) return null;

		for (int off = 0; off < run.Count; off++)
		{
			int predicted = 0;
			for (int i = 0; i < n; i++) predicted += domains[i].IndexOf(run[off].States[i].Value) * strides[i];
			if (predicted != off) return null;
		}

		return domains;
	}

	/// <summary>
	///     The block's palette index in closed form, replacing a lookup that allocated a state
	///     container, a list and one boxed object per state, hashed the name, and could allocate
	///     two HashSets inside BlockStateContainer.Equals - twice per block placed.
	///     Every digit is range-checked and returns -1 for a value outside its domain. That is not
	///     decoration: an unresolvable state has to stay loud, because SubChunk.SetBlock turns -1
	///     into a refusal to write, and bare arithmetic would hand back a plausible wrong id.
	/// </summary>
	private static void WriteRuntimeId(StringBuilder sb, List<BlockState> run, int baseId, HashSet<string> bits)
	{
		List<object>[] domains = StateStrides(run, out int[] strides, out string[] stateNames);

		sb.AppendLine();
		if (domains == null || domains.Length == 0)
		{
			sb.AppendLine($"\t\tpublic override int GetRuntimeId() => {baseId};");
			return;
		}

		sb.AppendLine("\t\tpublic override int GetRuntimeId()");
		sb.AppendLine("\t\t{");

		var terms = new List<string>();
		for (int i = 0; i < domains.Length; i++)
		{
			string prop = CodeName(stateNames[i].Replace("minecraft:", ""));
			string digit = $"d{i}";
			bool needsCheck = true;

			if (bits.Contains(stateNames[i]))
			{
				int whenTrue = domains[i].FindIndex(v => Convert.ToInt64(v) == 1);
				int whenFalse = domains[i].FindIndex(v => Convert.ToInt64(v) == 0);
				sb.AppendLine($"\t\t\tint {digit} = {prop} ? {whenTrue} : {whenFalse};");
				needsCheck = false;
			}
			else if (domains[i][0] is byte or int)
			{
				List<long> values = domains[i].Select(Convert.ToInt64).ToList();
				bool ascendingRun = values.Select((v, k) => v == values[0] + k).All(x => x);
				if (ascendingRun)
				{
					long min = values[0], max = values[^1];
					sb.AppendLine($"\t\t\tif ({prop} < {min} || {prop} > {max}) return -1;");
					sb.AppendLine($"\t\t\tint {digit} = {prop}{(min == 0 ? "" : $" - {min}")};");
					needsCheck = false;
				}
				else
				{
					sb.AppendLine($"\t\t\tint {digit} = {prop} switch");
					sb.AppendLine("\t\t\t{");
					for (int k = 0; k < values.Count; k++) sb.AppendLine($"\t\t\t\t{values[k]} => {k},");
					sb.AppendLine("\t\t\t\t_ => -1");
					sb.AppendLine("\t\t\t};");
				}
			}
			else
			{
				sb.AppendLine($"\t\t\tint {digit} = {prop} switch");
				sb.AppendLine("\t\t\t{");
				for (int k = 0; k < domains[i].Count; k++) sb.AppendLine($"\t\t\t\t\"{domains[i][k]}\" => {k},");
				sb.AppendLine("\t\t\t\t_ => -1");
				sb.AppendLine("\t\t\t};");
			}

			if (needsCheck) sb.AppendLine($"\t\t\tif ({digit} < 0) return -1;");
			terms.Add(strides[i] == 1 ? digit : $"{digit} * {strides[i]}");
		}

		sb.AppendLine();
		sb.AppendLine($"\t\t\treturn {baseId} + {string.Join(" + ", terms)};");
		sb.AppendLine("\t\t} // method");
	}

	/// <summary>FNV-1 (multiply then xor), not FNV-1a. This is what orders the block names.</summary>
	private static ulong Fnv1_64(string value)
	{
		ulong hash = 0xcbf29ce484222325;
		foreach (byte b in Encoding.UTF8.GetBytes(value))
		{
			hash = unchecked(hash * 0x100000001b3) ^ b;
		}

		return hash;
	}

	/// <summary>
	///     Emits the block palette as compiled code instead of a data file parsed at startup.
	///     The palette is just an ordered list of name plus states, and the order is its meaning:
	///     a block's network id is its position. All of that is known when this runs, so there is
	///     nothing for the server to work out at runtime, and nothing to keep a second copy of the
	///     source data around for.
	///     Split into parts because a C# method body is capped at 64KB of IL, the same reason
	///     RecipeData is written this way.
	/// </summary>
	private static int WriteBlockPalette(string path, List<BlockState> palette)
	{
		const int PerPart = 1000;
		int parts = (palette.Count + PerPart - 1) / PerPart;

		var sb = new StringBuilder();
		WriteHeader(sb, "MiNET.BdsExtract/Data block_states.json + blocks.json");
		sb.AppendLine("using System.Collections.Generic;");
		sb.AppendLine("using MiNET.Utils;");
		sb.AppendLine();
		sb.AppendLine("namespace MiNET.Blocks");
		sb.AppendLine("{");
		// Every entry carries the same schema stamp, so it is one constant rather than a field on
		// 16913 objects. A state written to disk without it is treated as predating every upgrade
		// schema, and Bedrock runs it through the whole rename and remap chain on load.
		int[] versions = palette.Select(p => p.Version).Distinct().ToArray();
		if (versions.Length != 1)
		{
			throw new InvalidDataException($"expected one block state version, found {versions.Length}: {string.Join(", ", versions)}");
		}

		int version = versions[0];
		sb.AppendLine("\tpublic static partial class BlockPaletteData");
		sb.AppendLine("\t{");
		sb.AppendLine("\t\t/// <summary>");
		sb.AppendLine("\t\t///     Block state schema version, as published with the palette. Stamp this on every");
		sb.AppendLine("\t\t///     block state written to disk: without it Bedrock treats the state as predating");
		sb.AppendLine("\t\t///     every upgrade schema and rewrites it on load.");
		sb.AppendLine("\t\t/// </summary>");
		sb.AppendLine($"\t\tpublic const int BlockStateVersion = {version}; // {version >> 24 & 0xff}.{version >> 16 & 0xff}.{version >> 8 & 0xff}.{version & 0xff}");
		sb.AppendLine();
		sb.AppendLine("\t\t/// <summary>Fills the palette in canonical order. Index is the network id.</summary>");
		sb.AppendLine("\t\tpublic static void Create(BlockPalette palette)");
		sb.AppendLine("\t\t{");
		for (int part = 1; part <= parts; part++) sb.AppendLine($"\t\t\tCreatePalette_Part{part}(palette);");
		sb.AppendLine("\t\t}");

		for (int part = 1; part <= parts; part++)
		{
			sb.AppendLine();
			sb.AppendLine($"\t\tprivate static void CreatePalette_Part{part}(BlockPalette palette)");
			sb.AppendLine("\t\t{");

			int from = (part - 1) * PerPart;
			int to = Math.Min(from + PerPart, palette.Count);
			for (int i = from; i < to; i++)
			{
				BlockState state = palette[i];
				var states = state.States.Select(s => s.Value switch
				{
					byte b => $"new BlockStateByte {{Name = \"{s.Name}\", Value = {b}}}",
					int n => $"new BlockStateInt {{Name = \"{s.Name}\", Value = {n}}}",
					_ => $"new BlockStateString {{Name = \"{s.Name}\", Value = \"{s.Value}\"}}"
				});

				string statePart = state.States.Count == 0 ? "" : $", States = {{{string.Join(", ", states)}}}";
				sb.AppendLine($"\t\t\tpalette.Add(new BlockStateContainer {{RuntimeId = {i}, Id = {state.Id}, Name = \"{state.Name}\"{statePart}}});");
			}

			sb.AppendLine("\t\t}");
		}

		sb.AppendLine("\t}");
		sb.AppendLine("}");
		File.WriteAllText(path, sb.ToString(), new UTF8Encoding(true));
		return palette.Count;
	}

	/// <summary>
	///     What the generated code was actually built from. The data is a pinned submodule, so the
	///     commit is the answer, and printing it means a regeneration says on the tin which palette
	///     produced it rather than leaving it to be inferred later.
	/// </summary>
	private static string DescribeSource(string dataDir)
	{
		try
		{
			// Quoted: the format contains spaces, and unquoted git reads each word as its own
			// argument and fails.
			var info = new System.Diagnostics.ProcessStartInfo("git", "log -1 --date=short \"--format=%h  %ad  %s\"")
			{
				WorkingDirectory = dataDir,
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				UseShellExecute = false
			};
			using var process = System.Diagnostics.Process.Start(info);
			string line = process?.StandardOutput.ReadToEnd().Trim();
			process?.WaitForExit();
			return string.IsNullOrEmpty(line) ? "unknown revision" : line;
		}
		catch (Exception e)
		{
			return $"unknown revision ({e.GetType().Name})";
		}
	}

	// Everything the block half of the generator reads: the palette in canonical order, the
	// per state physical properties keyed by block name, and the network hash each state carries,
	// which is what proves the palette was read the way Bedrock writes it.
	private sealed record BlockExtract(
		List<BlockState> Palette,
		Dictionary<string, List<BlockProperties>> Properties,
		List<uint> NetworkHashes,
		int BlockStateVersion,
		string BlockStateRelease,
		string PublishedFor,
		bool NetworkIdsAreHashes);

	/// <summary>
	///     Reads the BDS memory extraction: block_states.json is the palette, one row per state in
	///     canonical order, and blocks.json is one row per block type. Between them they carry
	///     everything the palette NBT and the properties dump used to hold, read out of the running
	///     server rather than republished by a third party.
	///     The row's own networkId is its list index, which is asserted rather than assumed: the
	///     index is the runtime id every generated GetRuntimeId returns.
	///     Three values live on the block type instead of the state: the legacy numeric id, the
	///     translucency and whether the block requires the correct tool to drop anything.
	/// </summary>
	private static BlockExtract ReadBlockExtract(string statesPath, string typesPath)
	{
		var statesFile = JsonConvert.DeserializeObject<BlockStatesJson>(File.ReadAllText(statesPath));
		var typesFile = JsonConvert.DeserializeObject<BlockTypesJson>(File.ReadAllText(typesPath));

		var types = typesFile.Blocks.ToDictionary(b => b.NameInfo.FullName, StringComparer.Ordinal);

		var palette = new List<BlockState>(statesFile.States.Count);
		var properties = new Dictionary<string, List<BlockProperties>>(StringComparer.Ordinal);
		var hashes = new List<uint>(statesFile.States.Count);

		for (int i = 0; i < statesFile.States.Count; i++)
		{
			BlockStateJson row = statesFile.States[i];
			BlockSerializationIdJson id = row.SerializationId;

			if (row.NetworkId != i) throw new InvalidDataException($"{row.Name} at index {i} carries networkId {row.NetworkId}");
			if (id.Version != statesFile.BlockStateVersion) throw new InvalidDataException($"{row.Name} at index {i} carries version {id.Version}, not {statesFile.BlockStateVersion}");
			if (!types.TryGetValue(id.Name, out BlockTypeJson type)) throw new InvalidDataException($"{id.Name} at index {i} has no block type in {Path.GetFileName(typesPath)}");

			var states = new List<(string, object)>();
			foreach (KeyValuePair<string, JToken> state in id.States)
			{
				// The three state kinds Bedrock serializes: a bit as an NBT byte, a number as an
				// NBT int, an enum as a string. The kind decides the tag, and the tag decides the
				// hash, so a wrong reading here cannot survive VerifyNetworkHashes.
				object value = state.Value.Type switch
				{
					JTokenType.Boolean => (byte) (state.Value.Value<bool>() ? 1 : 0),
					JTokenType.Integer => state.Value.Value<int>(),
					JTokenType.String => state.Value.Value<string>(),
					_ => throw new InvalidDataException($"{id.Name}.{state.Key} holds {state.Value.Type}, which is not a block state kind")
				};
				states.Add((state.Key, value));
			}

			palette.Add(new BlockState(id.Name, type.Id, id.Version, states));
			hashes.Add(row.NetworkHash);

			if (!properties.TryGetValue(id.Name, out List<BlockProperties> rows)) properties[id.Name] = rows = new List<BlockProperties>();
			BlockDirectDataJson direct = row.DirectData;
			rows.Add(new BlockProperties
			{
				Name = id.Name,
				IsSolid = row.CachedComponentData.IsSolid,
				Hardness = direct.DestroySpeed,
				ExplosionResistance = direct.ExplosionResistance,
				Friction = direct.Friction,
				Translucency = type.Translucency,
				LightEmission = direct.LightEmission,
				LightDampening = direct.Light,
				BurnOdds = direct.BurnOdds,
				FlameOdds = direct.FlameOdds,
				RequiresCorrectToolForDrops = type.RequiresCorrectToolForDrops,
				CanContainLiquidSource = direct.WaterDetectionRule.CanContainLiquid
			});
		}

		return new BlockExtract(palette, properties, hashes, statesFile.BlockStateVersion, statesFile.BlockStateRelease,
			statesFile.LayoutPublishedFor, statesFile.NetworkIdsAreHashes);
	}

	/// <summary>
	///     Re-serializes every palette entry the way Bedrock hashes it and requires the result to
	///     equal the hash the extraction read out of the block itself.
	///     This is the whole proof that the palette was read correctly. The hash covers the name,
	///     every state name, every state value and the NBT tag each value was written as, so a
	///     dropped state, a reordered pair or a bit read as a number all fail it. A recipe that
	///     agrees on every row is proven; one that agrees on some is not, so a single mismatch
	///     stops the run before anything is written.
	/// </summary>
	private static bool VerifyNetworkHashes(BlockExtract extract)
	{
		var failures = new List<string>();
		for (int i = 0; i < extract.Palette.Count; i++)
		{
			BlockState state = extract.Palette[i];
			uint computed = ComputeNetworkHash(state);
			if (computed == extract.NetworkHashes[i]) continue;
			failures.Add($"  index {i} {state.Name}: computed {computed}, extraction read {extract.NetworkHashes[i]}");
		}

		if (failures.Count > 0)
		{
			Console.Error.WriteLine($"network hash proof failed on {failures.Count} of {extract.Palette.Count} states:");
			foreach (string f in failures.Take(20)) Console.Error.WriteLine(f);
			return false;
		}

		Console.WriteLine($"network hash proof: {extract.Palette.Count}/{extract.Palette.Count} states re-serialize to their own serializationIdHashForNetwork");
		return true;
	}

	/// <summary>
	///     FNV-1a 32 over the little-endian, non-varint NBT of {name, states}, states sorted
	///     alphabetically. The same recipe as MiNET.Blocks.BlockFactory.ComputeNetworkHash, written
	///     out again here because the generator does not reference the code it emits.
	/// </summary>
	private static uint ComputeNetworkHash(BlockState state)
	{
		var states = new NbtCompound("states");
		foreach ((string name, object value) in state.States.OrderBy(s => s.Name, StringComparer.Ordinal))
		{
			switch (value)
			{
				case byte b:
					states.Add(new NbtByte(name, b));
					break;
				case int n:
					states.Add(new NbtInt(name, n));
					break;
				case string s:
					states.Add(new NbtString(name, s));
					break;
			}
		}

		var root = new NbtCompound("")
		{
			new NbtString("name", state.Name),
			states
		};

		byte[] bytes = new NbtFile(root) {BigEndian = false, UseVarInt = false}.SaveToBuffer(NbtCompression.None);

		uint hash = 0x811c9dc5;
		foreach (byte b in bytes)
		{
			hash ^= b;
			hash *= 0x01000193;
		}

		return hash;
	}

	private sealed class BlockStatesJson
	{
		[JsonProperty("networkIdsAreHashes")] public bool NetworkIdsAreHashes { get; set; }
		[JsonProperty("blockStateVersion")] public int BlockStateVersion { get; set; }
		[JsonProperty("blockStateRelease")] public string BlockStateRelease { get; set; }
		[JsonProperty("layoutPublishedFor")] public string LayoutPublishedFor { get; set; }
		[JsonProperty("states")] public List<BlockStateJson> States { get; set; }
	}

	private sealed class BlockStateJson
	{
		[JsonProperty("name")] public string Name { get; set; }
		[JsonProperty("networkId")] public int NetworkId { get; set; }
		[JsonProperty("serializationId")] public BlockSerializationIdJson SerializationId { get; set; }
		[JsonProperty("serializationIdHashForNetwork")] public uint NetworkHash { get; set; }
		[JsonProperty("cachedComponentData")] public BlockCachedComponentDataJson CachedComponentData { get; set; }
		[JsonProperty("directData")] public BlockDirectDataJson DirectData { get; set; }
	}

	private sealed class BlockSerializationIdJson
	{
		[JsonProperty("name")] public string Name { get; set; }
		[JsonProperty("version")] public int Version { get; set; }

		// A JObject, because the states keep the order they are written in and each value carries
		// its own kind. A dictionary would keep neither.
		[JsonProperty("states")] public JObject States { get; set; }
	}

	private sealed class BlockCachedComponentDataJson
	{
		[JsonProperty("isSolid")] public bool IsSolid { get; set; }
	}

	private sealed class BlockDirectDataJson
	{
		[JsonProperty("destroySpeed")] public float DestroySpeed { get; set; }
		[JsonProperty("explosionResistance")] public float ExplosionResistance { get; set; }
		[JsonProperty("friction")] public float Friction { get; set; }
		[JsonProperty("lightEmission")] public int LightEmission { get; set; }
		[JsonProperty("light")] public int Light { get; set; }
		[JsonProperty("burnOdds")] public int BurnOdds { get; set; }
		[JsonProperty("flameOdds")] public int FlameOdds { get; set; }
		[JsonProperty("waterDetectionRule")] public BlockWaterDetectionJson WaterDetectionRule { get; set; }
	}

	private sealed class BlockWaterDetectionJson
	{
		[JsonProperty("canContainLiquid")] public bool CanContainLiquid { get; set; }
	}

	private sealed class BlockTypesJson
	{
		[JsonProperty("blocks")] public List<BlockTypeJson> Blocks { get; set; }
	}

	private sealed class BlockTypeJson
	{
		[JsonProperty("nameInfo")] public BlockNameInfoJson NameInfo { get; set; }
		[JsonProperty("id")] public int Id { get; set; }
		[JsonProperty("creativeGroup")] public string CreativeGroup { get; set; }
		[JsonProperty("translucency")] public float Translucency { get; set; }
		[JsonProperty("requiresCorrectToolForDrops")] public bool RequiresCorrectToolForDrops { get; set; }
	}

	private sealed class BlockNameInfoJson
	{
		[JsonProperty("fullName")] public string FullName { get; set; }
	}

	/// <summary>
	///     Class names declared by a hand-written file in Blocks/. Read off disk rather than by
	///     reflection: the previously generated classes are indistinguishable from hand-written
	///     ones once compiled, so asking the type system makes the generator skip everything.
	/// </summary>
	/// <summary>
	///     Maps each block to the base class its family shares, from the creative inventory grouping.
	///     Nothing on the wire says oak_stairs is a stair: the palette carries a name and states and
	///     no notion of a family. The creative groups are the one place Mojang publishes it, and they
	///     get it right where naming does not, separating fence from fence gate rather than lumping
	///     everything ending in _fence together.
	///     <para>
	///         The base name is derived, not configured: itemGroup.name.leaves becomes LeavesBase. A
	///         family only gets a base when someone has written that file, so adding shared behaviour
	///         is one new file and no wiring, and a family with nothing to share stays on Block.
	///     </para>
	/// </summary>
	private static Dictionary<string, string> ReadFamilyBases(string creativePath, string blocksDir, HashSet<string> handWritten)
	{
		var creative = JsonConvert.DeserializeObject<CreativeFamiliesJson>(File.ReadAllText(creativePath));
		var bases = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
		var usable = new Dictionary<string, bool>();

		foreach (CreativeFamilyItemJson item in creative.Items)
		{
			string group = item.Group;
			if (string.IsNullOrEmpty(group) || !group.StartsWith("itemGroup.name.")) continue;

			string baseName = CodeName(group.Substring("itemGroup.name.".Length)) + "Base";
			if (!handWritten.Contains(baseName)) continue;

			// Only a base the generated class can actually call, which now means one that takes
			// nothing: either no declared constructor at all, or an explicitly parameterless one.
			// Several of the older bases still want an argument (SlabBase needs its double-slab
			// name, others take a byte) that is not derivable from the palette, and those stay
			// opt-in through a hand-written class until they are converted.
			if (!usable.TryGetValue(baseName, out bool ok))
			{
				string file = Path.Combine(blocksDir, baseName + ".cs");
				if (File.Exists(file))
				{
					string text = File.ReadAllText(file);
					ok = Regex.IsMatch(text, @"\b" + baseName + @"\s*\(\s*\)")
						|| !Regex.IsMatch(text, @"\b" + baseName + @"\s*\([^)]");
				}

				usable[baseName] = ok;
			}

			if (!ok) continue;

			bases[item.Item] = baseName;
		}

		return bases;
	}

	private sealed class CreativeFamiliesJson
	{
		[JsonProperty("items")] public List<CreativeFamilyItemJson> Items { get; set; }
	}

	private sealed class CreativeFamilyItemJson
	{
		[JsonProperty("item")] public string Item { get; set; }
		[JsonProperty("group")] public string Group { get; set; }
	}

	private static HashSet<string> ReadHandWrittenClasses(string blocksDir, params string[] generatedFiles)
	{
		var generated = new HashSet<string>(generatedFiles.Length > 0 ? generatedFiles : new[] {"PartialBlocks.cs", "BlockData.generated.cs"});
		var names = new HashSet<string>();
		foreach (string path in Directory.GetFiles(blocksDir, "*.cs"))
		{
			if (generated.Contains(Path.GetFileName(path))) continue;
			foreach (Match m in Regex.Matches(File.ReadAllText(path), @"public\s+(?:abstract\s+)?(?:partial\s+)?class\s+(\w+)"))
			{
				names.Add(m.Groups[1].Value);
			}
		}

		return names;
	}

	/// <summary>
	///     Which base each hand-written block class declares, read from the source. The creative
	///     groups name the families Mojang publishes, but the code has bases they do not: liquids,
	///     furnaces, redstone torches. Their members say so themselves, in the file, and that is the
	///     only place it is written down.
	///     <para>
	///         Only a base that is not itself a block counts, so a concrete class that happens to be
	///         someone's base (RedstoneOre under LitRedstoneOre) keeps its own states rather than
	///         having them hoisted out from under it.
	///     </para>
	/// </summary>
	/// <summary>Line comments only, enough to keep a commented-out class from being read as one.</summary>
	private static string StripLineComments(string text)
	{
		return Regex.Replace(text, @"//.*?$", "", RegexOptions.Multiline);
	}

	private static Dictionary<string, string> ReadDeclaredBases(string blocksDir, HashSet<string> handWritten, HashSet<string> blockClassNames)
	{
		var declared = new Dictionary<string, string>();
		foreach (string path in Directory.GetFiles(blocksDir, "*.cs"))
		{
			if (Path.GetFileName(path) is "PartialBlocks.cs" or "BlockData.generated.cs" or "BlockPaletteData.generated.cs") continue;

			string text = StripLineComments(File.ReadAllText(path));
			foreach (Match m in Regex.Matches(text, @"public\s+(?:(?:abstract|sealed|static|partial)\s+)*class\s+(\w+)\s*:\s*(\w+)"))
			{
				string baseName = m.Groups[2].Value;
				if (baseName == "Block" || !handWritten.Contains(baseName) || blockClassNames.Contains(baseName)) continue;
				declared[m.Groups[1].Value] = baseName;
			}
		}

		return declared;
	}

	/// <summary>
	///     Classes that write their own GetState by hand. A generated partial would duplicate it,
	///     so those blocks are the one case where the states are not generated. Detected from the
	///     file text, since the compiled type cannot tell a hand-written override from a
	///     previously generated one.
	/// </summary>
	private static HashSet<string> ReadHandImplementedStateClasses(string blocksDir)
	{
		var generated = new HashSet<string> {"PartialBlocks.cs", "BlockData.generated.cs", "LegacyPartialBlocks.cs"};
		var names = new HashSet<string>();
		foreach (string path in Directory.GetFiles(blocksDir, "*.cs"))
		{
			if (generated.Contains(Path.GetFileName(path))) continue;
			string text = File.ReadAllText(path);
			if (!text.Contains("BlockStateContainer GetState()")) continue;
			foreach (Match m in Regex.Matches(text, @"public\s+(?:abstract\s+)?(?:partial\s+)?class\s+(\w+)"))
			{
				names.Add(m.Groups[1].Value);
			}
		}

		return names;
	}

	private static int WriteBlockDataClasses(string path, List<IGrouping<string, BlockState>> byName, HashSet<string> handWritten, Dictionary<string, string> familyBases)
	{
		var sb = new StringBuilder();
		WriteHeader(sb, "MiNET.BdsExtract/Data block_states.json + blocks.json");
		sb.AppendLine("namespace MiNET.Blocks");
		sb.AppendLine("{");

		int count = 0;
		foreach (IGrouping<string, BlockState> group in byName)
		{
			string className = CodeName(group.Key.Replace("minecraft:", ""));
			if (handWritten.Contains(className)) continue;

			count++;
			sb.AppendLine();

			// The family base when the block belongs to one and someone has written it, Block otherwise.
			string familyBase = familyBases.TryGetValue(group.Key, out string found) ? found : "Block";

			sb.AppendLine($"\tpublic partial class {className} : {familyBase} // {group.Key}");
			sb.AppendLine("\t{");
			sb.AppendLine($"\t\tpublic {className}()");
			sb.AppendLine("\t\t{");
			sb.AppendLine("\t\t\tIsGenerated = true;");
			sb.AppendLine("\t\t}");
			sb.AppendLine("\t} // class");
		}

		sb.AppendLine("}");
		File.WriteAllText(path, sb.ToString(), new UTF8Encoding(true));
		return count;
	}

	/// <summary>
	///     The physical properties of a block, from CloudburstMC block_properties.json. The file is
	///     per block STATE and in palette order, so a block's rows pair positionally with its
	///     permutations. A property whose value is the same across all of them is emitted as a
	///     constant; one that differs is emitted as a switch over the states it depends on, which is
	///     how a lit candle reports 3, 6, 9 or 12 instead of the unlit 0 that leads its palette run.
	///     Emitted get-only, because these describe the block rather than record anything: a block
	///     needing a different value overrides the property.
	/// </summary>
	private static void WriteBlockProperties(StringBuilder sb, IGrouping<string, BlockState> group, Dictionary<string, List<BlockProperties>> properties)
	{
		if (!properties.TryGetValue(group.Key, out List<BlockProperties> rows) || rows.Count == 0) return;

		List<BlockState> permutations = group.ToList();

		WriteProperty(sb, "float", "Hardness", group.Key, rows, permutations, p => Literal(p.Hardness));
		WriteProperty(sb, "float", "BlastResistance", group.Key, rows, permutations, p => Literal(p.ExplosionResistance));
		WriteProperty(sb, "float", "FrictionFactor", group.Key, rows, permutations, p => Literal(p.Friction));
		WriteProperty(sb, "int", "LightLevel", group.Key, rows, permutations, p => p.LightEmission.ToString(CultureInfo.InvariantCulture));
		WriteProperty(sb, "int", "LightDampening", group.Key, rows, permutations, p => p.LightDampening.ToString(CultureInfo.InvariantCulture));
		WriteProperty(sb, "float", "Translucency", group.Key, rows, permutations, p => Literal(p.Translucency));
		WriteProperty(sb, "int", "BurnOdds", group.Key, rows, permutations, p => p.BurnOdds.ToString(CultureInfo.InvariantCulture));
		WriteProperty(sb, "int", "FlameOdds", group.Key, rows, permutations, p => p.FlameOdds.ToString(CultureInfo.InvariantCulture));
		WriteProperty(sb, "bool", "IsSolid", group.Key, rows, permutations, p => p.IsSolid ? "true" : "false");
		WriteProperty(sb, "bool", "RequiresCorrectToolForDrops", group.Key, rows, permutations, p => p.RequiresCorrectToolForDrops ? "true" : "false");
		WriteProperty(sb, "bool", "CanContainLiquidSource", group.Key, rows, permutations, p => p.CanContainLiquidSource ? "true" : "false");
		sb.AppendLine();
	}

	/// <summary>
	///     One property: a constant when every state agrees, otherwise a switch over the smallest set
	///     of states that decides it. A block whose rows do not pair with its permutations cannot be
	///     resolved per state, so a varying value there is an error rather than a silent first-row
	///     guess, which is the bug this replaces.
	/// </summary>
	private static void WriteProperty(StringBuilder sb, string type, string name, string blockName,
		List<BlockProperties> rows, List<BlockState> permutations, Func<BlockProperties, string> format)
	{
		List<string> values = rows.Select(format).ToList();
		string first = values[0];
		if (values.All(v => v == first))
		{
			sb.AppendLine($"\t\tpublic override {type} {name} => {first};");
			return;
		}

		if (rows.Count != permutations.Count)
		{
			throw new InvalidDataException(
				$"{blockName}.{name} differs between states, but it has {rows.Count} property rows against " +
				$"{permutations.Count} palette permutations, so no row can be attributed to a state.");
		}

		List<string> deciding = FindDecidingStates(permutations, values)
			?? throw new InvalidDataException($"{blockName}.{name} differs between states but no combination of its states decides it.");

		// The commonest value carries the default arm, so the switch lists only the exceptions.
		string fallback = values.GroupBy(v => v).OrderByDescending(g => g.Count()).First().Key;
		var arms = new List<string>();
		var seen = new HashSet<string>(StringComparer.Ordinal);
		for (int i = 0; i < permutations.Count; i++)
		{
			if (values[i] == fallback) continue;
			string pattern = StatePattern(permutations, deciding, i);
			if (seen.Add(pattern)) arms.Add($"\t\t\t{pattern} => {values[i]},");
		}

		string subject = deciding.Count == 1
			? StateProperty(deciding[0])
			: $"({string.Join(", ", deciding.Select(StateProperty))})";

		sb.AppendLine($"\t\tpublic override {type} {name} => {subject} switch");
		sb.AppendLine("\t\t{");
		foreach (string arm in arms) sb.AppendLine(arm);
		sb.AppendLine($"\t\t\t_ => {fallback}");
		sb.AppendLine("\t\t};");
	}

	/// <summary>
	///     The smallest group of states whose values decide this property, preferring fewer states so
	///     a candle switches on lit and candles rather than on every state it happens to carry.
	///     Null when no group does, which means the data disagrees with itself.
	/// </summary>
	private static List<string> FindDecidingStates(List<BlockState> permutations, List<string> values)
	{
		List<string> candidates = permutations[0].States
			.Select(s => s.Name)
			.Where(n => permutations.Select(p => StateValue(p, n)).Distinct().Count() > 1)
			.ToList();
		if (candidates.Count == 0 || candidates.Count > 12) return null;

		for (int size = 1; size <= candidates.Count; size++)
		{
			for (int mask = 0; mask < 1 << candidates.Count; mask++)
			{
				if (BitOperations.PopCount((uint) mask) != size) continue;

				List<string> subset = candidates.Where((_, i) => (mask & (1 << i)) != 0).ToList();
				var byKey = new Dictionary<string, string>(StringComparer.Ordinal);
				bool consistent = true;
				for (int i = 0; i < permutations.Count && consistent; i++)
				{
					string key = string.Join("", subset.Select(n => StateValue(permutations[i], n)));
					if (byKey.TryGetValue(key, out string existing)) consistent = existing == values[i];
					else byKey[key] = values[i];
				}
				if (consistent) return subset;
			}
		}
		return null;
	}

	private static string StateValue(BlockState permutation, string stateName)
	{
		object value = permutation.States.First(s => s.Name == stateName).Value;
		return Convert.ToString(value, CultureInfo.InvariantCulture);
	}

	private static string StateProperty(string stateName)
	{
		return CodeName(stateName.Replace("minecraft:", ""));
	}

	/// <summary>
	///     The case pattern for one permutation, matching how the state itself is declared: a byte
	///     that only ever holds 0 or 1 is a bool property, so it has to be matched as true or false.
	/// </summary>
	private static string StatePattern(List<BlockState> permutations, List<string> deciding, int index)
	{
		var parts = new List<string>();
		foreach (string stateName in deciding)
		{
			object value = permutations[index].States.First(s => s.Name == stateName).Value;
			List<object> all = permutations.Select(p => p.States.First(s => s.Name == stateName).Value).Distinct().ToList();
			parts.Add(value switch
			{
				byte b when IsBitState(all) => b == 1 ? "true" : "false",
				byte b => b.ToString(CultureInfo.InvariantCulture),
				int n => n.ToString(CultureInfo.InvariantCulture),
				_ => $"\"{value}\""
			});
		}
		return deciding.Count == 1 ? parts[0] : $"({string.Join(", ", parts)})";
	}

	/// <summary>Matches the rule the state declaration uses: a 0/1 byte state becomes a bool.</summary>
	private static bool IsBitState(List<object> values)
	{
		if (values.Count == 0 || values[0] is not byte) return false;
		List<byte> bytes = values.Cast<byte>().ToList();
		return bytes.Count <= 2 && bytes.Min() == 0 && bytes.Max() <= 1;
	}

	private static string Literal(float value)
	{
		return value.ToString("0.0###########", CultureInfo.InvariantCulture) + "f";
	}

	/// <summary>
	///     The physical properties of one block state, as the extraction reads them off the block:
	///     the ones on the block itself from its directData and cachedComponentData, the two that
	///     live on the block type from there. A block's rows are in palette order, so they pair
	///     positionally with its permutations, which is what lets a per-state value be generated as
	///     a switch. Only lightEmission (51 blocks, the candles) and lightDampening (cauldron)
	///     differ between a block's own states; the rest are constant and collapse back to one
	///     value.
	/// </summary>
	private class BlockProperties
	{
		public string Name { get; set; }
		public bool IsSolid { get; set; }
		public float Hardness { get; set; }
		public float ExplosionResistance { get; set; }
		public float Friction { get; set; }
		public float Translucency { get; set; }
		public int LightEmission { get; set; }
		public int LightDampening { get; set; }
		public int BurnOdds { get; set; }
		public int FlameOdds { get; set; }
		public bool RequiresCorrectToolForDrops { get; set; }
		public bool CanContainLiquidSource { get; set; }
	}

	private static int WritePartialBlocks(string path, List<IGrouping<string, BlockState>> byName, HashSet<string> handImplemented, Dictionary<string, int> baseIndex,
		Dictionary<string, string> familyBases, Dictionary<string, List<BlockProperties>> properties)
	{
		// State declarations for each family base, collected once and written at the end.
		var baseStates = new Dictionary<string, StringBuilder>();

		// A base may only declare a state every one of its members has. Families are mostly uniform,
		// but not always: mangrove_propagule sits in the sapling family and adds hanging and
		// propagule_stage on top of the shared age_bit, so those two stay on the variant.
		var sharedStateNames = new Dictionary<string, HashSet<string>>();
		foreach (IGrouping<string, BlockState> group in byName)
		{
			if (!familyBases.TryGetValue(group.Key, out string owner)) continue;

			var names = new HashSet<string>(group.First().States.Select(s => s.Name));
			if (sharedStateNames.TryGetValue(owner, out HashSet<string> common)) common.IntersectWith(names);
			else sharedStateNames[owner] = names;
		}

		Console.WriteLine($"block properties: {properties.Count} blocks");

		var sb = new StringBuilder();
		WriteHeader(sb, "MiNET.BdsExtract/Data block_states.json + blocks.json");
		sb.AppendLine("using System;");
		sb.AppendLine("using System.Collections.Generic;");
		sb.AppendLine("using MiNET.Utils;");
		sb.AppendLine();
		sb.AppendLine("namespace MiNET.Blocks");
		sb.AppendLine("{");

		int count = 0;
		foreach (IGrouping<string, BlockState> group in byName)
		{
			string className = CodeName(group.Key.Replace("minecraft:", ""));
			if (handImplemented.Contains(className)) continue;
			count++;

			BlockState first = group.First();

			// Every distinct value a state takes across this block's permutations, so a bit stays
			// a bool and a range keeps its real bounds.
			var valuesByState = new Dictionary<string, List<object>>();
			foreach (BlockState permutation in group)
			foreach ((string stateName, object value) in permutation.States)
			{
				if (!valuesByState.TryGetValue(stateName, out List<object> values)) valuesByState[stateName] = values = new List<object>();
				if (!values.Contains(value)) values.Add(value);
			}

			var bits = new HashSet<string>();
			sb.AppendLine();
			sb.AppendLine($"\tpublic partial class {className} // {group.Key}");
			sb.AppendLine("\t{");
			sb.AppendLine($"\t\tpublic override string Name => \"{group.Key}\";");
			sb.AppendLine();
			WriteBlockProperties(sb, group, properties);

			// A family shares one state signature, so its states are declared once on the base
			// rather than repeated on every member. Emitted into baseStates here and written out
			// after the loop; the variant still gets SetState and GetState, which need its own
			// name and id.
			familyBases.TryGetValue(group.Key, out string ownerBase);
			StringBuilder baseTarget = null;
			var claimed = new HashSet<string>();
			if (ownerBase != null)
			{
				if (!baseStates.TryGetValue(ownerBase, out baseTarget)) baseStates[ownerBase] = baseTarget = new StringBuilder();
				claimed = baseStates[ownerBase].Length == 0 ? sharedStateNames[ownerBase] : new HashSet<string>();
			}

			foreach ((string stateName, object defaultValue) in first.States)
			{
				// Shared states go on the base, once. Anything this block adds beyond the family
				// stays here, and members after the first emit neither, since the base has them.
				StringBuilder stateTarget = ownerBase == null || !sharedStateNames[ownerBase].Contains(stateName)
					? sb
					: claimed.Contains(stateName) ? baseTarget : new StringBuilder();

				string prop = CodeName(stateName.Replace("minecraft:", ""));
				List<object> values = valuesByState[stateName];
				switch (defaultValue)
				{
					case byte:
					{
						List<byte> bytes = values.Cast<byte>().OrderBy(v => v).ToList();
						if (bytes.Count <= 2 && bytes.Min() == 0 && bytes.Max() <= 1)
						{
							bits.Add(stateName);
							stateTarget.AppendLine($"\t\t[StateBit] public bool {prop} {{ get; set; }} = {((byte) defaultValue == 1 ? "true" : "false")};");
						}
						else
						{
							stateTarget.AppendLine($"\t\t[StateRange({bytes.Min()}, {bytes.Max()})] public byte {prop} {{ get; set; }} = {(byte) defaultValue};");
						}
						break;
					}
					case int:
					{
						List<int> ints = values.Cast<int>().OrderBy(v => v).ToList();
						stateTarget.AppendLine($"\t\t[StateRange({ints.Min()}, {ints.Max()})] public int {prop} {{ get; set; }} = {(int) defaultValue};");
						break;
					}
					case string:
					{
						string enumValues = string.Join(",", values.Cast<string>().Select(v => $"\"{v}\""));
						stateTarget.AppendLine($"\t\t[StateEnum({enumValues})] public string {prop} {{ get; set; }} = \"{(string) defaultValue}\";");
						break;
					}
				}
			}

			sb.AppendLine();
			sb.AppendLine("\t\tpublic override void SetState(List<IBlockState> states)");
			sb.AppendLine("\t\t{");
			sb.AppendLine("\t\t\tforeach (var state in states)");
			sb.AppendLine("\t\t\t{");
			sb.AppendLine("\t\t\t\tswitch (state)");
			sb.AppendLine("\t\t\t\t{");
			foreach ((string stateName, object value) in first.States)
			{
				string prop = CodeName(stateName.Replace("minecraft:", ""));
				string type = StateTypeName(value);
				string assign = bits.Contains(stateName) ? "Convert.ToBoolean(s.Value)" : "s.Value";
				sb.AppendLine($"\t\t\t\t\tcase {type} s when s.Name == \"{stateName}\":");
				sb.AppendLine($"\t\t\t\t\t\t{prop} = {assign};");
				sb.AppendLine("\t\t\t\t\t\tbreak;");
			}
			sb.AppendLine("\t\t\t\t} // switch");
			sb.AppendLine("\t\t\t} // foreach");
			sb.AppendLine("\t\t} // method");

			sb.AppendLine();
			sb.AppendLine("\t\tpublic override BlockStateContainer GetState()");
			sb.AppendLine("\t\t{");
			sb.AppendLine("\t\t\tvar record = new BlockStateContainer();");
			sb.AppendLine($"\t\t\trecord.Name = \"{group.Key}\";");
			sb.AppendLine($"\t\t\trecord.Id = {first.Id};");
			foreach ((string stateName, object value) in first.States)
			{
				string prop = CodeName(stateName.Replace("minecraft:", ""));
				string type = StateTypeName(value);
				string expr = bits.Contains(stateName) ? $"Convert.ToByte({prop})" : prop;
				sb.AppendLine($"\t\t\trecord.States.Add(new {type} {{Name = \"{stateName}\", Value = {expr}}});");
			}
			sb.AppendLine("\t\t\treturn record;");
			sb.AppendLine("\t\t} // method");

			WriteRuntimeId(sb, group.ToList(), baseIndex[group.Key], bits);

			sb.AppendLine("\t} // class");
		}

		// The family bases last, each carrying the states its members share. Declared here rather
		// than on every member so a base can use them as typed properties instead of digging them
		// out of the state container by name, and so 138 slabs declare their states once.
		foreach ((string baseName, StringBuilder states) in baseStates.OrderBy(p => p.Key, StringComparer.Ordinal))
		{
			if (states.Length == 0) continue;

			sb.AppendLine();
			sb.AppendLine($"\tpublic abstract partial class {baseName}");
			sb.AppendLine("\t{");
			sb.Append(states);
			sb.AppendLine("\t} // class");
		}

		sb.AppendLine("}");
		File.WriteAllText(path, sb.ToString(), new UTF8Encoding(true));
		return count;
	}

	private static string StateTypeName(object value) => value switch
	{
		byte => "BlockStateByte",
		int => "BlockStateInt",
		_ => "BlockStateString"
	};

	private static void WriteHeader(StringBuilder sb, string source)
	{
		sb.AppendLine($"// GENERATED by MiNET.BlockGen from {source}.");
		sb.AppendLine("// Do not hand-edit. Run the tool again after updating the source data.");
		sb.AppendLine();
	}

	private static string CodeName(string name)
	{
		var sb = new StringBuilder();
		bool upper = true;
		foreach (char c in name)
		{
			if (c == '_' || c == '.' || c == ':')
			{
				upper = true;
				continue;
			}
			sb.Append(upper ? char.ToUpperInvariant(c) : c);
			upper = false;
		}

		return sb.ToString();
	}

	private static string FindRepoRoot()
	{
		var dir = new DirectoryInfo(AppContext.BaseDirectory);
		while (dir != null && !Directory.Exists(Path.Combine(dir.FullName, ".git"))) dir = dir.Parent;
		return dir?.FullName ?? Directory.GetCurrentDirectory();
	}
}
