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

using System.Globalization;
using System.Text;
using fNbt;
using Newtonsoft.Json.Linq;

namespace MiNET.BlockGen;

/// <summary>
///     Writes MiNET/Items/ItemRegistryData.generated.cs and Items/ItemData.generated.cs from the
///     BDS memory extraction (MiNET.BdsExtract/Data/items-runtime.json).
///     The registry entry carries the component tree as fNbt construction code, so the tree is
///     readable and reviewable in the generated file rather than a base64 blob. The runtime
///     serializes it once at startup and hands the bytes to the item_registry packet.
///     Nothing is written until every entry and every tree has been measured against the captured
///     BDS frame. Where the extraction cannot yet read a value, the leaf is taken from that frame
///     through a NAMED gap in <see cref="Gaps" />: every gap is counted, printed on each run, and
///     disappears the moment the extraction supplies the value.
/// </summary>
public static class ItemGenerator
{
	/// <summary>
	///     One item registry identity: the durable string id, this protocol version's network id,
	///     the version and component-based flag BDS reports, and the component tree for the items
	///     that carry one.
	/// </summary>
	public sealed record ItemEntry(string Name, short NetworkId, bool ComponentBased, int Version, NbtCompound Tree);

	/// <summary>
	///     A value the extraction cannot read yet, named by its path under "components". The frame
	///     supplies it, the run prints it, and the entry is deleted when the extraction lands the
	///     field. A path with no trailing slash covers the whole subtree beneath it.
	/// </summary>
	private static readonly (string Path, string Reason)[] Gaps =
	{
		("minecraft:kinetic_weapon", "settings is one raw blob; the component class has no layout"),
		("minecraft:piercing_weapon", "reach and creative_reach are read as pointers, hitbox_margin disagrees with them"),
		("minecraft:repairable/repair_items", "repairItems is a vector the walk does not follow"),
		("minecraft:use_modifiers/start_using", "no member holds it, and the schema default (if_first) is not what the frame carries"),
		("minecraft:storage_item/allowed_items", "the component holds no member for it"),
		("minecraft:storage_item/banned_items", "the component holds no member for it"),
		("minecraft:projectile/projectile_entity", "actorIdentifier is raw bytes"),
		("minecraft:block_placer/alignedPlacement", "the component reads only its block"),
		("minecraft:block_placer/canUseBlockAsIcon", "the component reads only its block"),
		("minecraft:block_placer/replaceBlockItem", "the component reads only its block"),
		("minecraft:block_placer/use_on", "the component reads only its block"),
		("minecraft:publisher_on_use_on", "declared, no address; the built map does not carry it"),
		("minecraft:food/effects", "effects is a vector the walk does not follow"),
		("minecraft:food/remove_effects", "removeEffects is a vector the walk does not follow"),
		("minecraft:seed/crop_result", "result is a pointer to the crop item"),
		("minecraft:seed/plant_at", "targetLandBlocks is a vector the walk does not follow"),
		("item_properties/minecraft:icon/textures/bundle_open_back", "the icon component's texture map is one raw blob"),
		("item_properties/minecraft:icon/textures/bundle_open_front", "the icon component's texture map is one raw blob")
	};

	/// <summary>
	///     The sound event names by id, as the extraction publishes them. A component that holds a
	///     sound holds its id; the wire carries the name.
	/// </summary>
	private static JObject _soundNames;

	public static List<ItemEntry> Run(string extractDir, string itemsDir, string capturesPath, HashSet<string> blockNames, HashSet<string> handWrittenItems)
	{
		string rowsPath = Path.Combine(extractDir, "items-runtime.json");
		var extract = JObject.Parse(File.ReadAllText(rowsPath));
		var rows = ((JArray) extract["items"]).Cast<JObject>().ToList();
		_soundNames = (JObject) extract["enums"]["LevelSoundEvent"];
		Console.WriteLine($"item source: {rowsPath}");
		Console.WriteLine($"             BDS {extract["source"]?["server"]}, {rows.Count} items");

		List<Captures.FrameItem> frame = Captures.ReadItemRegistry(capturesPath);
		var frameByName = frame.ToDictionary(f => f.Name, StringComparer.Ordinal);
		Console.WriteLine($"item_registry frame: {frame.Count} entries, {frame.Count(f => f.Tree != null)} with a component tree");

		// The extraction lists items alphabetically; BDS sends them in its own map order, which
		// nothing here can reproduce. Entries are emitted alphabetically and checked by name.
		var names = rows.Select(r => (string) r["name"]).ToList();
		if (!names.SequenceEqual(names.OrderBy(n => n, StringComparer.Ordinal)))
		{
			Console.WriteLine("note: the extraction's rows are no longer in name order; entries are emitted in row order");
		}

		var used = new Dictionary<string, int>(StringComparer.Ordinal);
		var failures = new List<string>();
		var entries = new List<ItemEntry>(rows.Count);

		foreach (JObject row in rows)
		{
			string name = (string) row["name"];
			if (!frameByName.TryGetValue(name, out Captures.FrameItem frameItem))
			{
				failures.Add($"{name}: not in the frame");
				continue;
			}

			short id = (short) row["id"];
			bool componentBased = (bool) row["componentBased"];
			int version = (int) row["version"];

			if (id != frameItem.Id) failures.Add($"{name}: network id {id}, frame has {frameItem.Id}");
			if (componentBased != frameItem.ComponentBased) failures.Add($"{name}: component_based {componentBased}, frame has {frameItem.ComponentBased}");
			if (version != frameItem.Version) failures.Add($"{name}: version {version}, frame has {frameItem.Version}");

			NbtCompound tree = BuildTree(row, componentBased);
			if (tree != null && frameItem.Tree == null) failures.Add($"{name}: generated a tree, the frame carries none");
			else if (tree == null && frameItem.Tree != null) failures.Add($"{name}: generated no tree, the frame carries one");
			else if (tree != null) Reconcile(name, tree.Get<NbtCompound>("components"), frameItem.Tree.Get<NbtCompound>("components"), "", used, failures);

			entries.Add(new ItemEntry(name, id, componentBased, version, tree == null ? null : Sorted(tree)));
		}

		foreach (string missing in frameByName.Keys.Where(n => rows.All(r => (string) r["name"] != n)))
		{
			failures.Add($"{missing}: in the frame, not in the extraction");
		}

		// The proof: every generated tree re-serialized must be the frame's exact bytes.
		foreach (ItemEntry entry in entries)
		{
			if (entry.Tree == null) continue;
			byte[] bytes = Serialize(entry.Tree);
			byte[] expected = frameByName[entry.Name].TreeBytes;
			if (bytes.SequenceEqual(expected)) continue;

			int at = 0;
			while (at < bytes.Length && at < expected.Length && bytes[at] == expected[at]) at++;
			failures.Add($"{entry.Name}: tree serializes to {bytes.Length} bytes against the frame's {expected.Length}, first differing at {at}: " +
				$"{Convert.ToHexString(bytes[at..Math.Min(bytes.Length, at + 16)])} against {Convert.ToHexString(expected[at..Math.Min(expected.Length, at + 16)])}");
		}

		ReportGaps(used);

		if (failures.Count > 0)
		{
			Console.Error.WriteLine($"item registry proof failed on {failures.Count} points:");
			foreach (string failure in failures.Take(60)) Console.Error.WriteLine($"  {failure}");
			throw new InvalidDataException("the generated item registry does not equal the captured frame");
		}

		Console.WriteLine($"item registry proof: {entries.Count}/{frame.Count} entries and {entries.Count(e => e.Tree != null)} trees equal the frame byte for byte");

		WriteItemRegistry(Path.Combine(itemsDir, "ItemRegistryData.generated.cs"), entries);
		Console.WriteLine($"ItemRegistryData.generated.cs: {entries.Count} entries");

		int classes = WriteItemDataClasses(Path.Combine(itemsDir, "ItemData.generated.cs"), entries, blockNames, handWrittenItems);
		Console.WriteLine($"ItemData.generated.cs: {classes} classes");

		return entries;
	}

	private static void ReportGaps(Dictionary<string, int> used)
	{
		if (used.Count == 0)
		{
			Console.WriteLine("component trees: every leaf came from the extraction");
			return;
		}

		Console.WriteLine($"component trees: {used.Values.Sum()} leaves on {used.Count} paths come from the capture, because the extraction cannot read them yet:");
		foreach ((string path, int count) in used.OrderBy(p => p.Key, StringComparer.Ordinal))
		{
			string reason = Gaps.First(g => g.Path == path).Reason;
			Console.WriteLine($"  {path} x{count}: {reason}");
		}

		foreach ((string path, string _) in Gaps)
		{
			if (!used.ContainsKey(path)) Console.WriteLine($"  note: gap {path} is no longer needed, the extraction supplies it");
		}
	}

	// ---------------------------------------------------------------- tree building

	/// <summary>
	///     Which items carry a tree, read off the extraction rather than off the frame: a
	///     component-based item whose component map holds at least one entry that could be read, or
	///     a hardcoded item carrying one of the three legacy pointer components. That rule picks the
	///     same 31 and 47 items the frame does.
	/// </summary>
	private static NbtCompound BuildTree(JObject row, bool componentBased)
	{
		var components = new NbtCompound("components");

		if (componentBased)
		{
			var readable = Readable(row);
			if (readable.Count == 0) return null;

			components.Add(ItemProperties(row, readable));
			components.Add(Tags("item_tags", row["tags"] as JArray));
			foreach ((string kind, JObject value) in readable)
			{
				NbtTag tag = Component(kind, value, row);
				if (tag != null) components.Add(tag);
			}
		}
		else
		{
			JObject food = row["foodComponent"] as JObject;
			JObject seed = row["seedComponent"] as JObject;
			JObject camera = row["cameraComponent"] as JObject;
			if (food == null && seed == null && camera == null) return null;

			if (row["block"]?.Type == JTokenType.String) components.Add(new NbtString("minecraft:block", (string) row["block"]));
			if (camera != null) components.Add(Camera(camera));
			if (food != null) components.Add(LegacyFood(food));
			if ((bool) row["isGlint"]) components.Add(new NbtByte("minecraft:foil", 1));
			int maxStackSize = (int) row["maxStackSize"];
			if (maxStackSize != 64) components.Add(new NbtInt("minecraft:max_stack_size", maxStackSize));
			if (seed != null) components.Add(Seed(seed));
			if ((bool) row["isStackedByData"]) components.Add(new NbtByte("minecraft:stacked_by_data", 1));
			int useDuration = (int) row["useDuration"];
			if (useDuration != 0) components.Add(new NbtInt("minecraft:use_duration", useDuration));
		}

		return new NbtCompound("") {components};
	}

	/// <summary>
	///     The component entries the extraction actually read. An entry that is {"unread": ...} was
	///     declared but never reached, so it is absent rather than empty. minecraft:icon is dropped
	///     here because it is not a component on the wire: the icon rides inside item_properties.
	/// </summary>
	private static List<(string Kind, JObject Value)> Readable(JObject row)
	{
		var result = new List<(string, JObject)>();
		if (row["components"] is not JObject components) return result;

		foreach (JProperty property in components.Properties())
		{
			if (property.Value is not JObject value) continue;
			if (value["unread"] != null) continue;
			if (property.Name == "minecraft:icon") continue;
			result.Add((property.Name, value));
		}

		return result;
	}

	/// <summary>
	///     The Item class fields the wire calls item_properties. This is the one hand-kept list:
	///     every name, type and source field is fixed, and the frame diff is what says it is right.
	/// </summary>
	private static NbtCompound ItemProperties(JObject row, List<(string Kind, JObject Value)> readable)
	{
		JObject enchantable = readable.FirstOrDefault(r => r.Kind == "minecraft:enchantable").Value;
		string slot = enchantable != null ? (string) enchantable["enchantSlot"] : "none";

		var properties = new NbtCompound("item_properties")
		{
			new NbtByte("allow_off_hand", Bit(row["allowOffhand"])),
			new NbtByte("can_destroy_in_creative", Bit(row["canDestroyInCreative"])),
			new NbtInt("creative_category", (int) row["creativeCategory"]),
			new NbtString("creative_group", (string) row["creativeGroup"] ?? ""),
			new NbtInt("damage", (int) row["damage"]),
			new NbtString("enchantable_slot", slot),
			new NbtInt("enchantable_value", (int) row["enchantableValue"]),
			new NbtByte("foil", Bit(row["isGlint"])),
			new NbtInt("frame_count", (int) row["frameCount"]),
			new NbtByte("hand_equipped", Bit(row["handEquipped"])),
			new NbtByte("hidden_in_commands", (byte) (int) row["hiddenInCommands"]),
			new NbtByte("liquid_clipped", Bit(row["liquidClipped"])),
			new NbtInt("max_stack_size", (int) row["maxStackSize"]),
			new NbtFloat("mining_speed", (float) row["miningSpeed"]),
			new NbtByte("should_despawn", Bit(row["shouldDespawn"])),
			new NbtByte("stacked_by_data", Bit(row["isStackedByData"])),
			new NbtInt("use_animation", (int) row["useAnimation"]),
			new NbtInt("use_duration", (int) row["useDuration"])
		};

		// The icon is a texture map keyed by role; the row names the default one, and an item
		// whose icon name is empty carries no icon at all.
		string icon = (string) row["iconName"];
		if (!string.IsNullOrEmpty(icon))
		{
			properties.Add(new NbtCompound("minecraft:icon") {new NbtCompound("textures") {new NbtString("default", icon)}});
		}

		return properties;
	}

	/// <summary>One component, by the wire's own field names. The frame diff checks every leaf.</summary>
	private static NbtTag Component(string kind, JObject value, JObject row)
	{
		switch (kind)
		{
			case "minecraft:block_placer":
				return new NbtCompound(kind) {new NbtString("block", (string) value["block"])};
			case "minecraft:bundle_interaction":
				return new NbtCompound(kind) {new NbtInt("num_viewable_slots", (int) value["numViewableSlots"])};
			case "minecraft:compostable":
				return new NbtCompound(kind) {new NbtByte("composting_chance", (byte) (int) value["compostingChance"])};
			case "minecraft:cooldown":
				return new NbtCompound(kind)
				{
					new NbtString("category", (string) value["category"] ?? ""),
					new NbtFloat("duration", (float) value["duration"]),
					new NbtString("type", CooldownType(value["type"]))
				};
			case "minecraft:damage":
				return new NbtCompound(kind) {new NbtShort("value", (short) (int) value["damage"])};
			case "minecraft:display_name":
				return new NbtCompound(kind) {new NbtString("value", (string) value["descriptionId"])};
			case "minecraft:durability":
				return new NbtCompound(kind)
				{
					new NbtCompound("damage_chance")
					{
						new NbtInt("max", (int) value["damageChance"]["max"]),
						new NbtInt("min", (int) value["damageChance"]["min"])
					},
					new NbtInt("max_durability", (int) value["maxDamage"])
				};
			case "minecraft:enchantable":
				return new NbtCompound(kind)
				{
					new NbtString("slot", (string) value["enchantSlot"]),
					new NbtByte("value", (byte) (int) value["enchantValue"])
				};
			case "minecraft:food":
				return new NbtCompound(kind)
				{
					new NbtByte("can_always_eat", Bit(value["canAlwaysEat"])),
					new NbtInt("nutrition", (int) value["nutrition"]),
					new NbtFloat("saturation_modifier", (float) value["saturationModifier"]),
					// An item descriptor, empty when the food converts to nothing.
					new NbtCompound("using_converts_to")
				};
			case "minecraft:fire_resistant":
				return new NbtCompound(kind) {new NbtByte("value", Bit(value["value"]))};
			case "minecraft:fuel":
				return new NbtCompound(kind) {new NbtFloat("duration", (float) value["fuelDuration"])};
			case "minecraft:hand_equipped":
				return new NbtCompound(kind) {new NbtByte("value", Bit(value["handEquipped"]))};
			case "minecraft:kinetic_weapon":
				return new NbtCompound(kind);
			case "minecraft:max_stack_size":
				return new NbtCompound(kind) {new NbtByte("value", (byte) (int) value["maxStackSize"])};
			case "minecraft:piercing_weapon":
				return new NbtCompound(kind);
			case "minecraft:projectile":
				return new NbtCompound(kind) {new NbtFloat("minimum_critical_power", (float) value["minCriticalPower"])};
			case "minecraft:publisher_on_use_on":
				return new NbtCompound(kind);
			case "minecraft:repairable":
				return new NbtCompound(kind);
			case "minecraft:storage_item":
				return new NbtCompound(kind)
				{
					new NbtByte("allow_nested_storage_items", Bit(value["allowNestedStorageItem"])),
					new NbtInt("max_slots", (int) value["numSlots"])
				};
			case "minecraft:storage_weight_limit":
				return new NbtCompound(kind) {new NbtInt("max_weight_limit", (int) value["weightLimit"])};
			case "minecraft:storage_weight_modifier":
				return new NbtCompound(kind) {new NbtInt("weight_in_storage_item", (int) value["weightInStorageItem"])};
			case "minecraft:swing_duration":
				return new NbtCompound(kind) {new NbtFloat("value", (float) value["valueInSeconds"])};
			case "minecraft:swing_sounds":
				return new NbtCompound(kind)
				{
					new NbtString("attack_hit", SoundName(value["attackHit"])),
					new NbtString("attack_miss", SoundName(value["attackMiss"]))
				};
			case "minecraft:tags":
				return new NbtCompound(kind) {Tags("tags", value["tags"] as JArray)};
			case "minecraft:throwable":
				return new NbtCompound(kind)
				{
					new NbtByte("do_swing_animation", Bit(value["doSwing"])),
					new NbtFloat("launch_power_scale", (float) value["launchPowerScale"]),
					new NbtFloat("max_draw_duration", (float) value["drawDuration"]),
					new NbtFloat("max_launch_power", (float) value["maxLaunchPower"]),
					new NbtFloat("min_draw_duration", (float) value["minDrawDuration"]),
					new NbtByte("scale_power_by_draw_duration", Bit(value["scalePowerByDrawDuration"]))
				};
			case "minecraft:use_animation":
				return new NbtCompound(kind) {new NbtString("value", (string) row["decoded"]["useAnimation"])};
			case "minecraft:use_modifiers":
			{
				var modifiers = new NbtCompound(kind)
				{
					new NbtByte("emit_vibrations", Bit(value["emitVibrations"])),
					new NbtFloat("movement_modifier", (float) value["movementModifier"]),
					new NbtFloat("use_duration", (float) value["useDuration"])
				};

				// The sound is optional, and its own flag says whether the id beside it means
				// anything. Without the flag every item would claim the sound at id 0.
				if ((bool) value["startSoundHasValue"]) modifiers.Add(new NbtString("start_sound", SoundName(value["startSound"]["value"])));
				return modifiers;
			}
			default:
				throw new InvalidDataException($"no wire shape for component {kind}");
		}
	}

	/// <summary>The legacy food pointer component, the shape the hardcoded food items still send.</summary>
	private static NbtCompound LegacyFood(JObject food)
	{
		return new NbtCompound("minecraft:food")
		{
			new NbtByte("can_always_eat", Bit(food["canAlwaysEat"])),
			new NbtInt("cooldown_time", (int) food["cooldownDuration"]),
			new NbtString("cooldown_type", (string) food["cooldownCategory"] ?? ""),
			new NbtInt("nutrition", (int) food["nutrition"]),
			new NbtInt("on_use_action", (int) food["onUseAction"]),
			new NbtList("on_use_range", NbtTagType.Float)
			{
				new NbtFloat((float) food["onUseRange"][0]),
				new NbtFloat((float) food["onUseRange"][1]),
				new NbtFloat((float) food["onUseRange"][2])
			},
			new NbtFloat("saturation_modifier", (float) food["saturationModifier"]),
			new NbtString("using_converts_to", (string) food["usingConvertsTo"] ?? "")
		};
	}

	private static NbtCompound Seed(JObject seed)
	{
		return new NbtCompound("minecraft:seed")
		{
			new NbtByte("plant_at_any_solid_surface", Bit(seed["plantAtAnyVisibleSolidSurface"])),
			new NbtString("plant_at_face", ((string) seed["faceToPlantAt"]["name"]).ToLowerInvariant())
		};
	}

	private static NbtCompound Camera(JObject camera)
	{
		return new NbtCompound("minecraft:camera")
		{
			new NbtFloat("black_bars_duration", (float) camera["blackBarsDuration"]),
			new NbtFloat("black_bars_screen_ratio", (float) camera["blackBarsScreenRatio"]),
			new NbtFloat("picture_duration", (float) camera["pictureDuration"]),
			new NbtFloat("shutter_duration", (float) camera["shutterDuration"]),
			new NbtFloat("shutter_screen_ratio", (float) camera["shutterScreenRatio"]),
			new NbtFloat("slide_away_duration", (float) camera["slideAwayDuration"])
		};
	}

	private static NbtList Tags(string name, JArray tags)
	{
		if (tags == null || tags.Count == 0) return new NbtList(name, NbtTagType.End);

		var list = new NbtList(name, NbtTagType.String);
		foreach (JToken tag in tags) list.Add(new NbtString((string) tag));
		return list;
	}

	/// <summary>The cooldown kind is a one byte enum; the extraction publishes its own name table.</summary>
	private static string CooldownType(JToken type)
	{
		string raw = (string) type["raw"];
		return raw switch
		{
			"00" => "use",
			"01" => "attack",
			_ => throw new InvalidDataException($"unknown ItemCooldownType {raw}")
		};
	}

	private static byte Bit(JToken value) => value != null && (bool) value ? (byte) 1 : (byte) 0;

	private static string SoundName(JToken id)
	{
		string key = ((int) id).ToString(CultureInfo.InvariantCulture);
		JToken name = _soundNames[key];
		if (name == null) throw new InvalidDataException($"sound event {key} is not in the extraction's LevelSoundEvent table");
		return (string) name;
	}

	// ---------------------------------------------------------------- the proof

	/// <summary>
	///     Measures one generated tree against the frame and fills the leaves the extraction cannot
	///     read yet, each through a named gap. Anything that differs without a gap is a defect and
	///     is reported with both values.
	/// </summary>
	private static void Reconcile(string item, NbtCompound generated, NbtCompound frame, string path, Dictionary<string, int> used, List<string> failures)
	{
		foreach (NbtTag expected in frame)
		{
			string child = path.Length == 0 ? expected.Name : path + "/" + expected.Name;
			NbtTag actual = generated.Get<NbtTag>(expected.Name);

			if (actual is NbtCompound actualCompound && expected is NbtCompound expectedCompound)
			{
				Reconcile(item, actualCompound, expectedCompound, child, used, failures);
				continue;
			}

			if (actual != null && Same(actual, expected)) continue;

			string gap = GapFor(child);
			if (gap == null)
			{
				failures.Add($"{item} {child}: generated {Describe(actual)}, frame has {Describe(expected)}");
				continue;
			}

			used[gap] = used.GetValueOrDefault(gap) + 1;
			if (actual != null) generated.Remove(actual);
			generated.Add((NbtTag) expected.Clone());
		}

		foreach (NbtTag extra in generated.ToList())
		{
			if (frame.Get<NbtTag>(extra.Name) != null) continue;
			string child = path.Length == 0 ? extra.Name : path + "/" + extra.Name;
			failures.Add($"{item} {child}: generated {Describe(extra)}, the frame has no such field");
		}
	}

	/// <summary>The gap that covers this path, or the one covering a subtree it sits inside.</summary>
	private static string GapFor(string path)
	{
		foreach ((string gap, string _) in Gaps)
		{
			if (path == gap || path.StartsWith(gap + "/", StringComparison.Ordinal)) return gap;
		}

		return null;
	}

	private static bool Same(NbtTag a, NbtTag b)
	{
		if (a.TagType != b.TagType) return false;
		return Serialize(new NbtCompound("") {(NbtTag) a.Clone()}).SequenceEqual(Serialize(new NbtCompound("") {(NbtTag) b.Clone()}));
	}

	private static string Describe(NbtTag tag)
	{
		if (tag == null) return "nothing";
		return tag switch
		{
			NbtCompound compound => $"a compound of {compound.Count}",
			NbtList list => $"a list of {list.Count} {list.ListType}",
			_ => $"{tag.TagType}({tag.StringValue})"
		};
	}

	/// <summary>
	///     Every compound BDS writes is in name order, so the generated one is rebuilt in that order
	///     before it is emitted. Sorting is what lets a component be built in whatever order its
	///     fields are read and still land on the frame's bytes.
	///     It rebuilds rather than reorders in place: a compound's children live in a dictionary, and
	///     removing them all and adding them back reuses the freed slots, so the order that comes
	///     back out is not the order they went in.
	/// </summary>
	private static NbtCompound Sorted(NbtCompound compound)
	{
		var result = new NbtCompound(compound.Name);
		foreach (NbtTag child in compound.OrderBy(c => c.Name, StringComparer.Ordinal)) result.Add(SortedTag(child));
		return result;
	}

	private static NbtTag SortedTag(NbtTag tag)
	{
		switch (tag)
		{
			case NbtCompound compound:
				return Sorted(compound);
			case NbtList list:
			{
				var result = new NbtList(list.Name, list.ListType);
				foreach (NbtTag element in list) result.Add(SortedTag(element));
				return result;
			}
			default:
				return (NbtTag) tag.Clone();
		}
	}

	private static byte[] Serialize(NbtCompound tree)
	{
		var root = (NbtCompound) tree.Clone();
		root.Name = "";
		return new NbtFile(root) {BigEndian = false, UseVarInt = true}.SaveToBuffer(NbtCompression.None);
	}

	// ---------------------------------------------------------------- emit

	/// <summary>
	///     Emits the item registry as compiled code. An item's identity is its string id; the
	///     network id is only what this protocol version numbered it, so it is generated data rather
	///     than something the server works out. The component tree is emitted as fNbt construction
	///     code so the wire field names are readable in the file, and the runtime serializes it once.
	///     Split into parts for the 64KB IL method body cap, as with the block palette.
	/// </summary>
	private static void WriteItemRegistry(string path, List<ItemEntry> items)
	{
		const int PerPart = 200;
		int parts = (items.Count + PerPart - 1) / PerPart;

		var sb = new StringBuilder();
		sb.AppendLine("// GENERATED by MiNET.BlockGen from MiNET.BdsExtract/Data items-runtime.json.");
		sb.AppendLine("// Do not hand-edit. Run the tool again after updating the source data.");
		sb.AppendLine("// Every entry and every component tree is checked against the captured BDS item_registry");
		sb.AppendLine("// frame under MiNET.BlockGen/Captures before this file is written.");
		sb.AppendLine();
		sb.AppendLine("using fNbt;");
		sb.AppendLine();
		sb.AppendLine("namespace MiNET.Items");
		sb.AppendLine("{");
		sb.AppendLine("\tpublic static partial class ItemRegistryData");
		sb.AppendLine("\t{");
		sb.AppendLine("\t\t/// <summary>Fills the registry. Entry order is the extraction's, which is by name.</summary>");
		sb.AppendLine("\t\tpublic static void Create(ItemRegistry registry)");
		sb.AppendLine("\t\t{");
		for (int part = 1; part <= parts; part++) sb.AppendLine($"\t\t\tCreateItems_Part{part}(registry);");
		sb.AppendLine("\t\t}");

		for (int part = 1; part <= parts; part++)
		{
			sb.AppendLine();
			sb.AppendLine($"\t\tprivate static void CreateItems_Part{part}(ItemRegistry registry)");
			sb.AppendLine("\t\t{");

			int from = (part - 1) * PerPart;
			int to = Math.Min(from + PerPart, items.Count);
			for (int i = from; i < to; i++)
			{
				ItemEntry item = items[i];
				string componentBased = item.ComponentBased ? "true" : "false";
				sb.Append($"\t\t\tregistry.Add(\"{item.Name}\", {item.NetworkId}, {componentBased}, {item.Version},");
				if (item.Tree == null) sb.AppendLine(" null);");
				else
				{
					sb.AppendLine();
					WriteTag(sb, item.Tree.Get<NbtTag>("components"), 4);
					sb.AppendLine(");");
				}
			}

			sb.AppendLine("\t\t}");
		}

		sb.AppendLine("\t}");
		sb.AppendLine("}");
		File.WriteAllText(path, sb.ToString(), new UTF8Encoding(true));
	}

	/// <summary>One tag as fNbt construction code, named unless it is a list element.</summary>
	private static void WriteTag(StringBuilder sb, NbtTag tag, int indent, bool named = true)
	{
		string pad = new string('\t', indent);
		string name = named && tag.Name != null ? $"\"{tag.Name}\"" : null;

		switch (tag)
		{
			case NbtCompound compound:
			{
				sb.AppendLine($"{pad}new NbtCompound({name})");
				sb.AppendLine($"{pad}{{");
				var children = compound.ToList();
				for (int i = 0; i < children.Count; i++)
				{
					WriteTag(sb, children[i], indent + 1);
					sb.AppendLine(i == children.Count - 1 ? "" : ",");
				}

				sb.Append($"{pad}}}");
				break;
			}
			case NbtList list:
			{
				string arguments = name == null ? $"NbtTagType.{list.ListType}" : $"{name}, NbtTagType.{list.ListType}";
				sb.AppendLine($"{pad}new NbtList({arguments})");
				sb.AppendLine($"{pad}{{");
				var elements = list.ToList();
				for (int i = 0; i < elements.Count; i++)
				{
					WriteTag(sb, elements[i], indent + 1, named: false);
					sb.AppendLine(i == elements.Count - 1 ? "" : ",");
				}

				sb.Append($"{pad}}}");
				break;
			}
			default:
			{
				string arguments = Value(tag);
				if (name != null) arguments = $"{name}, {arguments}";
				sb.Append($"{pad}new {tag.GetType().Name}({arguments})");
				break;
			}
		}
	}

	private static string Value(NbtTag tag) => tag switch
	{
		NbtByte value => value.Value.ToString(CultureInfo.InvariantCulture),
		NbtShort value => value.Value.ToString(CultureInfo.InvariantCulture),
		NbtInt value => value.Value.ToString(CultureInfo.InvariantCulture),
		NbtLong value => value.Value.ToString(CultureInfo.InvariantCulture) + "L",
		NbtFloat value => value.Value.ToString("R", CultureInfo.InvariantCulture) + "f",
		NbtDouble value => value.Value.ToString("R", CultureInfo.InvariantCulture) + "d",
		NbtString value => Quote(value.Value),
		_ => throw new InvalidDataException($"no literal for {tag.TagType}")
	};

	private static string Quote(string value) => "\"" + value.Replace("\\", "\\\\").Replace("\"", "\\\"") + "\"";

	/// <summary>
	///     Writes MiNET/Items/ItemData.generated.cs: a typed Item subclass for every registry
	///     identity that doesn't already have one.
	///     Three things are skipped. Block items, because a block's own generated class covers them.
	///     Names with a hand-written class in Items/. And names that are only a rename of something
	///     already written, since ItemFactory resolves the old class under the current name too.
	///     The class carries the registry string id and nothing else. The network id is not baked in:
	///     it changes every protocol version, and an identity that carries a stale number is worse
	///     than one that carries none.
	/// </summary>
	private static int WriteItemDataClasses(string path, List<ItemEntry> items, HashSet<string> blockNames, HashSet<string> handWritten)
	{
		var sb = new StringBuilder();
		sb.AppendLine("// GENERATED by MiNET.BlockGen from MiNET.BdsExtract/Data items-runtime.json.");
		sb.AppendLine("// Do not hand-edit. Run the tool again after updating the source data.");
		sb.AppendLine();
		sb.AppendLine("namespace MiNET.Items");
		sb.AppendLine("{");

		var seen = new HashSet<string>();
		int count = 0;
		foreach (ItemEntry item in items.OrderBy(i => i.Name, StringComparer.Ordinal))
		{
			if (blockNames.Contains(BlockNameOf(item.Name))) continue;

			string className = "Item" + CodeName(item.Name.Replace("minecraft:", ""));
			if (handWritten.Contains(className)) continue;
			if (!seen.Add(className)) continue;

			string baseClass = BaseClassFor(className);
			count++;
			sb.AppendLine();
			sb.AppendLine($"\tpublic partial class {className} : {baseClass} // {item.Name}");
			sb.AppendLine("\t{");
			sb.AppendLine($"\t\tpublic {className}() : base(\"{item.Name}\") {{ }}");
			sb.AppendLine("\t} // class");
		}

		sb.AppendLine("}");
		File.WriteAllText(path, sb.ToString(), new UTF8Encoding(true));
		return count;
	}

	/// <summary>
	///     The block an item name refers to. Identical to the item name, except for the surviving
	///     "minecraft:item.x" twins, whose block simply drops the "item." prefix.
	/// </summary>
	private static string BlockNameOf(string itemName)
	{
		return itemName.StartsWith("minecraft:item.", StringComparison.Ordinal) ? "minecraft:" + itemName.Substring("minecraft:item.".Length) : itemName;
	}

	private static string BaseClassFor(string className)
	{
		if (className.EndsWith("Axe", StringComparison.Ordinal)) return "ItemAxe";
		if (className.EndsWith("Shovel", StringComparison.Ordinal)) return "ItemShovel";
		if (className.EndsWith("Pickaxe", StringComparison.Ordinal)) return "ItemPickaxe";
		if (className.EndsWith("Hoe", StringComparison.Ordinal)) return "ItemHoe";
		if (className.EndsWith("Sword", StringComparison.Ordinal)) return "ItemSword";
		if (className.EndsWith("Helmet", StringComparison.Ordinal)) return "ArmorHelmetBase";
		if (className.EndsWith("Chestplate", StringComparison.Ordinal)) return "ArmorChestplateBase";
		if (className.EndsWith("Leggings", StringComparison.Ordinal)) return "ArmorLeggingsBase";
		if (className.EndsWith("Boots", StringComparison.Ordinal)) return "ArmorBootsBase";
		return "Item";
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
}
