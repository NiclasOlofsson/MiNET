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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2026 Niclas Olofsson.
// All Rights Reserved.

#endregion

using System.Text;
using fNbt;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MiNET.BlockGen;

/// <summary>
///     Writes MiNET/Items/Data/creative_groups.json from the BDS memory extraction
///     (MiNET.BdsExtract/Data creative_items.json).
///     The catalog is name-addressed at the source, so every network id is resolved through the
///     item registry generated in the same pass and the two cannot drift apart. Stack user data is
///     carried as typed JSON NBT in the extraction's own shape rather than base64, so the file says
///     what each stack holds.
///     The output schema matches MiNET.CreativeGroupData exactly (the runtime reads it at startup,
///     the same way the biome table is a generated data file rather than symbols). When a captured
///     BDS creative_content frame is present, every group and every entry is measured against it
///     and each difference is reported; the extraction is written either way.
/// </summary>
public static class CreativeGenerator
{
	public static void Run(string extractDir, string outputPath, string capturePath, IReadOnlyDictionary<string, short> networkIdByName)
	{
		var source = JObject.Parse(File.ReadAllText(Path.Combine(extractDir, "creative_items.json")));

		short NetworkId(string name)
		{
			if (name == null) return 0;
			if (networkIdByName.TryGetValue(name, out short id)) return id;
			throw new InvalidDataException($"creative item {name} is not in the item registry");
		}

		Captures.FrameCreative frame = capturePath == null ? null : Captures.ReadCreativeContent(capturePath, NetworkId("minecraft:shield"));
		if (frame != null) Console.WriteLine($"creative_content frame: {Path.GetFileName(capturePath)}, {frame.Groups.Count} groups, {frame.Entries.Count} entries");

		var output = new CreativeGroupDataJson();
		var failures = new List<string>();
		int blockStacks = 0;

		int RuntimeId(JObject stack)
		{
			// The stack holds the block STATE it places, and that state's network id is the answer.
			// The block's own data table cannot stand in for it: the state a stack places is often
			// not the one its aux value selects (a chest faces north, a dripstone tip is not a
			// dripstone base), which is 29 of the 1,065 block stacks here.
			JToken block = stack["block"];
			if (block == null || block.Type == JTokenType.Null) return 0;
			if (block is not JObject state || state["networkId"] == null) throw new InvalidDataException($"creative stack's block {block} carries no state network id");

			blockStacks++;
			return (int) state["networkId"];
		}

		var groups = ((JArray) source["groups"]).Cast<JObject>().ToList();
		for (int i = 0; i < groups.Count; i++)
		{
			JObject group = groups[i];
			var icon = (JObject) group["icon"];
			string iconName = icon == null ? null : (string) icon["item"];

			var definition = new CreativeGroupDefJson
			{
				Category = (int) group["categoryValue"],
				Name = (string) group["name"] ?? "",
				Icon = iconName,
				IconNetworkId = NetworkId(iconName),
				IconMetadata = icon == null ? (short) 0 : (short) (int) icon["auxValue"],
				IconRuntimeId = icon == null ? 0 : RuntimeId(icon),
				IconNbt = icon?["userData"] as JObject
			};

			output.Groups.Add(definition);

			if (frame == null) continue;
			if (i >= frame.Groups.Count)
			{
				failures.Add($"group {i} {definition.Name}: the frame has only {frame.Groups.Count} groups");
				continue;
			}

			Captures.FrameGroup expected = frame.Groups[i];
			if (definition.Category != expected.Category) failures.Add($"group {i} {definition.Name}: category {definition.Category}, frame has {expected.Category}");
			if (definition.Name != expected.Name) failures.Add($"group {i}: name '{definition.Name}', frame has '{expected.Name}'");
			CheckStack($"group {i} {definition.Name} icon", definition.IconNetworkId, 1, definition.IconMetadata, definition.IconRuntimeId, definition.IconNbt, expected.Icon,
				failures);
		}

		// The Editor world the extraction runs in adds editor:map_marker_spawn_egg to the catalog.
		// A normal world does not send it, so the entry is dropped here and the drop is what makes
		// the entry list equal the frame.
		var entries = ((JArray) source["items"]).Cast<JObject>().Where(e => !((string) e["item"]).StartsWith("editor:", StringComparison.Ordinal)).ToList();
		int dropped = ((JArray) source["items"]).Count - entries.Count;

		for (int i = 0; i < entries.Count; i++)
		{
			JObject entry = entries[i];
			var definition = new CreativeEntryDefJson
			{
				GroupIndex = (int) entry["groupIndex"],
				NetworkId = NetworkId((string) entry["item"]),
				Metadata = (short) (int) entry["auxValue"],
				RuntimeId = RuntimeId(entry),
				Nbt = entry["userData"] as JObject
			};

			output.Entries.Add(definition);

			if (frame == null) continue;
			if (i >= frame.Entries.Count)
			{
				failures.Add($"entry {i} {entry["item"]}: the frame has only {frame.Entries.Count} entries");
				continue;
			}

			Captures.FrameEntry expected = frame.Entries[i];
			if (definition.GroupIndex != expected.GroupIndex) failures.Add($"entry {i} {entry["item"]}: group {definition.GroupIndex}, frame has {expected.GroupIndex}");
			if (i + 1 != expected.CreativeNetId) failures.Add($"entry {i} {entry["item"]}: creative net id {i + 1}, frame has {expected.CreativeNetId}");
			CheckStack($"entry {i} {entry["item"]}", definition.NetworkId, (int) entry["count"], definition.Metadata, definition.RuntimeId, definition.Nbt, expected.Stack,
				failures);
		}

		if (frame != null && entries.Count < frame.Entries.Count) failures.Add($"the frame has {frame.Entries.Count - entries.Count} entries the extraction does not");

		// Unused by the runtime but part of the schema; keep it truthful rather than empty.
		output.EntryGroups = output.Entries.Select(e => e.GroupIndex).ToList();

		Console.WriteLine($"creative stacks: {blockStacks} place a block, each carrying the network id of the state it places");
		if (dropped > 0) Console.WriteLine($"creative stacks: {dropped} editor: entries dropped, which a normal world does not send");

		// Every difference is printed, none is dropped; the catalog is written either way.
		if (frame == null)
		{
			Console.WriteLine($"creative catalog: {output.Groups.Count} groups and {output.Entries.Count} entries written unmeasured, no captured frame under Captures");
		}
		else if (failures.Count > 0)
		{
			Console.Error.WriteLine($"creative catalog differs from the frame on {failures.Count} points:");
			foreach (string failure in failures) Console.Error.WriteLine($"  {failure}");
		}
		else
		{
			Console.WriteLine($"creative catalog: {output.Groups.Count} groups and {output.Entries.Count} entries equal the frame");
		}

		string json = JsonConvert.SerializeObject(output, Formatting.Indented, new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore});
		File.WriteAllText(outputPath, json, new UTF8Encoding(true));
		Console.WriteLine($"creative_groups.json: {output.Groups.Count} groups, {output.Entries.Count} entries");
	}

	private static void CheckStack(string what, int networkId, int count, short metadata, int runtimeId, JObject nbt, Captures.FrameStack expected, List<string> failures)
	{
		if (networkId != expected.NetworkId) failures.Add($"{what}: network id {networkId}, frame has {expected.NetworkId}");
		if (networkId != 0 && count != expected.Count) failures.Add($"{what}: count {count}, frame has {expected.Count}");
		if (metadata != expected.Metadata) failures.Add($"{what}: metadata {metadata}, frame has {expected.Metadata}");
		if (runtimeId != expected.BlockRuntimeId) failures.Add($"{what}: block runtime id {runtimeId}, frame has {expected.BlockRuntimeId}");

		byte[] bytes = nbt == null ? null : SerializeUserData(nbt);
		if (bytes == null && expected.NbtBytes == null) return;
		if (bytes == null || expected.NbtBytes == null || !bytes.SequenceEqual(expected.NbtBytes))
		{
			failures.Add($"{what}: user data is {bytes?.Length.ToString() ?? "nothing"}, frame has {expected.NbtBytes?.Length.ToString() ?? "nothing"}");
		}
	}

	/// <summary>
	///     Item extra data is fixed little endian NBT with an unnamed root, which is the form
	///     Packet.WriteItemExtraData puts on the wire.
	/// </summary>
	private static byte[] SerializeUserData(JObject userData)
	{
		var root = TypedNbt.ReadCompound(userData, "");
		return new NbtFile(root) {BigEndian = false, UseVarInt = false}.SaveToBuffer(NbtCompression.None);
	}

	// MiNET.CreativeGroupData shape (the runtime reader).
	private sealed class CreativeGroupDataJson
	{
		public List<CreativeGroupDefJson> Groups { get; } = new();
		public List<CreativeEntryDefJson> Entries { get; } = new();
		public List<int> EntryGroups { get; set; }
	}

	private sealed class CreativeGroupDefJson
	{
		public int Category { get; set; }
		public string Name { get; set; }
		public string Icon { get; set; }
		public int IconNetworkId { get; set; }
		public short IconMetadata { get; set; }
		public int IconRuntimeId { get; set; }
		public JObject IconNbt { get; set; }
	}

	private sealed class CreativeEntryDefJson
	{
		public int GroupIndex { get; set; }
		public int NetworkId { get; set; }
		public short Metadata { get; set; }
		public int RuntimeId { get; set; }
		public JObject Nbt { get; set; }
	}
}
