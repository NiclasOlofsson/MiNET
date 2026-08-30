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
using MiNET.Blocks;
using Newtonsoft.Json.Linq;

namespace MiNET.BlockGen;

/// <summary>
///     Builds the StartGame block definitions from the extraction's own <c>definition</c> rows and
///     proves them against the captured BDS frame before anything is written.
///     The frame is the CHECK and never the source: every value here comes out of
///     MiNET.BdsExtract/Data blocks.json. A value the extraction states as a number that the wire
///     states as a name is looked up in a table this file names and sources; a number no table
///     covers is a failure, not a number written where a name belongs.
/// </summary>
public static class DefinitionGenerator
{
	/// <summary>
	///     The build's own CreativeItemCategory registration (function 0x3B6ED00, named by the
	///     literal at 0xBDE31FA) registers construction, nature, equipment, items, none in that
	///     order, which puts nature at 2 and none at 5. Both are confirmed on the wire. Value 0 is
	///     registered nowhere and is the extraction's own All.
	/// </summary>
	private static readonly Dictionary<int, string> Category = new()
	{
		[0] = "all", [1] = "construction", [2] = "nature", [3] = "equipment", [4] = "items", [5] = "none"
	};

	/// <summary>
	///     The only render layer the wire settles: 11 on both carriers, spelled
	///     alpha_test_single_sided. The block definition schema names eight render methods and this
	///     value is outside that range, so nothing else in the vocabulary is mapped.
	/// </summary>
	private static readonly Dictionary<int, string> RenderMethod = new() {[11] = "alpha_test_single_sided"};

	/// <summary>The connection type the two carriers hold. One value, confirmed twice.</summary>
	private static readonly Dictionary<int, string> ConnectionType = new() {[0] = "none"};

	private static readonly string[] LiquidReaction = ["broken", "popped", "blocking", "no_reaction"];

	private static readonly string[] LiquidType = ["water"];

	/// <summary>
	///     The block's own components, used only for the wire components no definition class is
	///     declared for yet.
	/// </summary>
	private static readonly Dictionary<string, string> FromBlock = new()
	{
		["BlockLiquidDetectionComponent"] = "minecraft:liquid_detection",
		["BlockRedstoneComponent"] = "minecraft:redstone_conductivity",
		["BlockSupportComponent"] = "minecraft:support",
		["BlockPrecipitationInteractionsComponent"] = "minecraft:precipitation_interactions",
		["BlockReplaceableComponent"] = "minecraft:replaceable"
	};

	/// <summary>
	///     Every block in blocks.json that carries a definition row, in the file's order. A row that
	///     cannot be mapped is reported by name and reason, and the whole build fails; no row is
	///     dropped and no value is guessed.
	/// </summary>
	public static Dictionary<string, BlockDefinition> BuildAll(string blockTypesPath)
	{
		var extract = JObject.Parse(File.ReadAllText(blockTypesPath));
		var definitions = new Dictionary<string, BlockDefinition>(StringComparer.Ordinal);
		var failures = new List<string>();

		foreach (JObject row in ((JArray) extract["blocks"]).Cast<JObject>())
		{
			if (row["definition"] is not JObject) continue;

			string name = (string) row["name"];
			try
			{
				definitions.Add(name, Build(row));
			}
			catch (Exception e)
			{
				failures.Add($"{name}: {e.Message}");
			}
		}

		if (failures.Count == 0) return definitions;

		Console.Error.WriteLine($"block definitions could not be built for {failures.Count} block(s):");
		foreach (string failure in failures) Console.Error.WriteLine($"  {failure}");
		return null;
	}

	/// <summary>One definition row as the wire carries it.</summary>
	public static BlockDefinition Build(JObject row)
	{
		var definition = (JObject) row["definition"];
		var vanilla = (JObject) definition["vanillaBlockData"];

		List<string> tags = ((JArray) definition["tags"]).Select(t => (string) t["value"]).ToList();

		// Which components the server puts on the wire is the verdict each description's own method
		// table carries at slot 7, which the extraction reads and writes as networked.
		BlockComponents components = Components(definition["components"] as JArray, row);

		var menu = (JObject) definition["menuCategory"];
		var category = new BlockMenuCategory(
			Named(Category, (int) menu["category"], "menu category"),
			(string) menu["group"],
			(bool) menu["isHiddenInCommands"]);

		var data = new VanillaBlockData(
			(int) vanilla["blockId"],
			Snake((string) row["material"]["name"]),
			Archetype(vanilla),
			(bool) vanilla["requiresCorrectToolForDrops"],
			vanilla["translucency"] is null or {Type: JTokenType.Null} ? null : (float) vanilla["translucency"],
			(bool) vanilla["canDampenVibrations"],
			(bool) vanilla["canOccludeVibrations"]);

		// A permutation holding nothing the server networks is not sent. Same verdict, same source.
		var permutations = new List<BlockPermutation>();
		foreach (JObject permutation in ((JArray) definition["permutations"]).Cast<JObject>())
		{
			var held = permutation["components"] as JArray;
			if (held == null || !held.OfType<JObject>().Any(c => c["networked"] is {} n && (bool) n)) continue;
			permutations.Add(new BlockPermutation((string) permutation["condition"], Components(held, null)));
		}

		return new BlockDefinition(tags, category, data, components, permutations, (int) definition["molangVersion"]);
	}

	// ---------------------------------------------------------------- the mapping

	/// <summary>
	///     The networked components of one definition or permutation. The block row is passed for a
	///     definition and null for a permutation, because the five components no definition class is
	///     declared for are read off the block itself and a permutation has no such fallback.
	/// </summary>
	private static BlockComponents Components(JArray described, JObject row)
	{
		var components = new BlockComponents();

		foreach (JObject component in described?.OfType<JObject>() ?? [])
		{
			if (component["networked"] is not {} networked || !(bool) networked) continue;

			switch ((string) component["class"])
			{
				case "BlockCollisionBoxDescription":
					components = components with {CollisionBox = new CollisionBox((bool) component["enabled"], Boxes(component))};
					break;
				case "BlockSelectionBoxDescription":
					components = components with {SelectionBox = new SelectionBox((bool) component["enabled"], Vector(component["origin"]), Vector(component["size"]))};
					break;
				case "BlockGeometryDescription":
					components = components with {Geometry = Geometry(component)};
					break;
				case "BlockMaterialInstancesDescription":
					components = components with {MaterialInstances = Materials(component)};
					break;
				case "BlockTransformationDescription":
					components = components with {Transformation = Transform(component)};
					break;
				case "BlockDestructibleByMiningDescription":
					components = components with {DestructibleByMining = (float) component["secondsToDestroy"]};
					break;
				case "BlockDestructionParticlesDescription":
					components = components with
					{
						DestructionParticles = new DestructionParticles((int) component["particleCount"], (string) component["texture"], Lower(component["tintMethod"]))
					};
					break;
				case "BlockConnectionRuleDescription":
					components = components with
					{
						ConnectionRule = new ConnectionRule(
							Named(ConnectionType, (int) component["acceptsConnectionsFrom"], "connection type"),
							((JArray) component["enabledDirections"]).Select(d => (string) d["value"]["name"]).ToList())
					};
					break;
			}
		}

		if (row == null) return components;

		// What no definition class is declared for yet, from the block's own component.
		foreach ((string component, string wire) in FromBlock)
		{
			JObject held = ((JArray) row["components"]).OfType<JObject>().FirstOrDefault(c => (string) c["name"] == component);
			if (held == null) continue;

			switch (wire)
			{
				case "minecraft:liquid_detection" when components.LiquidDetection == null:
				{
					var rule = (JObject) held["waterDetectionRule"];
					components = components with
					{
						LiquidDetection =
						[
							new LiquidDetectionRule(
								(bool) rule["canContainLiquid"],
								Named(LiquidType, (int) rule["liquidType"], "liquid type"),
								Named(LiquidReaction, (int) rule["onLiquidTouches"], "liquid reaction"),
								(byte) rule["stopsFlowDirections"],
								(bool) rule["clipAgainstCollider"])
						]
					};
					break;
				}
				case "minecraft:redstone_conductivity" when components.RedstoneConductivity == null:
					components = components with
					{
						RedstoneConductivity = new RedstoneConductivity((bool) held["redstoneConductor"], (bool) held["allowsWireToStepDown"])
					};
					break;
				case "minecraft:support" when components.SupportShape == null:
					components = components with {SupportShape = Lower(held["shape"])};
					break;
				case "minecraft:precipitation_interactions" when components.PrecipitationBehavior == null:
					components = components with {PrecipitationBehavior = (string) held["behavior"]["name"]};
					break;
				case "minecraft:replaceable" when components.Replaceable == null:
					components = components with {Replaceable = true};
					break;
			}
		}

		return components;
	}

	private static List<Aabb> Boxes(JObject component)
	{
		return ((JArray) component["boxes"]).Cast<JObject>()
			.Select(b => new Aabb(
				new Vec3((float) b["minX"], (float) b["minY"], (float) b["minZ"]),
				new Vec3((float) b["maxX"], (float) b["maxY"], (float) b["maxZ"])))
			.ToList();
	}

	private static Vec3 Vector(JToken values)
	{
		var array = (JArray) values;
		return new Vec3((float) array[0], (float) array[1], (float) array[2]);
	}

	/// <summary>
	///     The geometry description. uvsLocked is a two-alternative value: alternative 0 is the bool
	///     the wire writes as uv_lock, alternative 1 is the set of bone names the wire writes into
	///     bone_visibility. No carrier here holds alternative 1, and nothing states its shape, so it
	///     is a failure rather than a guess.
	/// </summary>
	private static BlockGeometry Geometry(JObject component)
	{
		var locked = (JObject) component["uvsLocked"];
		if (locked == null || (int) locked["which"] != 0) throw new InvalidDataException("geometry uvsLocked holds bone names, whose wire shape nothing states");

		return new BlockGeometry(
			(string) component["identifier"] ?? "",
			(string) component["culling"]?["value"] ?? "",
			(string) component["cullingLayer"] ?? "",
			(string) component["cullingShape"] ?? "",
			(bool) component["needsLegacyTopRotation"],
			(bool) component["useBlockTypeLightAbsorption"],
			(bool) component["ignoreGeometryForIsSolid"],
			(bool) component["isV1Fullblock"],
			(bool) locked["value"]);
	}

	private static MaterialInstances Materials(JObject component)
	{
		if (((JArray) component["mappings"]).Count > 0) throw new InvalidDataException("material instances carry mappings, whose wire shape nothing states");

		var materials = new Dictionary<string, MaterialInstance>(StringComparer.Ordinal);
		foreach (JObject material in ((JArray) component["materials"]).Cast<JObject>())
		{
			var instance = (JObject) material["instance"];
			materials.Add((string) material["name"], new MaterialInstance(
				(float) instance["ambientOcclusion"],
				// The five render flags the server packs into one byte; the wire carries the packed byte.
				(byte) ((int) instance["packedBools"] & 0x1f),
				Named(RenderMethod, (int) instance["renderLayer"], "render layer"),
				(string) instance["textureName"],
				Lower(instance["tintMethod"])));
		}

		return new MaterialInstances(materials);
	}

	private static Transformation Transform(JObject component)
	{
		return new Transformation(
			new Vec3((float) component["TX"], (float) component["TY"], (float) component["TZ"]),
			new Vec3I((int) component["RX"], (int) component["RY"], (int) component["RZ"]),
			new Vec3((float) component["RXP"], (float) component["RYP"], (float) component["RZP"]),
			new Vec3((float) component["SX"], (float) component["SY"], (float) component["SZ"]),
			new Vec3((float) component["SXP"], (float) component["SYP"], (float) component["SZP"]),
			(bool) component["hasJsonVersionBeforeValidation"]);
	}

	/// <summary>
	///     The archetype the block places as. The type name belongs to the record that carries it;
	///     the row's own name picks which record, so an archetype nothing declares fails here rather
	///     than reaching the wire as a shape nobody read.
	/// </summary>
	private static BlockArchetype Archetype(JObject vanilla)
	{
		var archetypes = (JArray) vanilla["blockArchetype"];
		if (archetypes.Count != 1) throw new InvalidDataException($"{archetypes.Count} archetypes, expected one");

		var archetype = (JObject) archetypes[0];
		string name = (string) archetype["name"];
		string declared = (string) ((JArray) vanilla["blockArchetypeNames"])[0]["value"];
		if (name != declared) throw new InvalidDataException($"archetype is '{name}' but the name list says '{declared}'");

		BlockArchetype built = name switch
		{
			"stair_block" => new StairArchetype((string) archetype["base_block"]),
			"slab_block" => new SlabArchetype((string) archetype["double_slab_block"], (bool) archetype["is_double"]),
			"bush_block" => new BushArchetype(),
			"wall_foliage_block" => new WallFoliageArchetype(
				(int) archetype["size_count"], (bool) archetype["is_bonemealable"],
				(float) archetype["bounciness"], (float) archetype["fall_damage_multiplier"]),
			_ => throw new InvalidDataException($"no archetype record is declared for '{name}'")
		};

		// Every member the row holds has to land in the record, or the wire loses it silently.
		var carried = new HashSet<string>(archetype.Properties().Select(p => p.Name), StringComparer.Ordinal);
		carried.ExceptWith(["name", "typeHash", "class", "unread"]);
		string[] mapped = name switch
		{
			"stair_block" => ["base_block"],
			"slab_block" => ["double_slab_block", "is_double"],
			"bush_block" => [],
			_ => ["size_count", "is_bonemealable", "bounciness", "fall_damage_multiplier"]
		};
		carried.ExceptWith(mapped);
		if (carried.Count > 0) throw new InvalidDataException($"archetype '{name}' carries {string.Join(", ", carried)}, which no record member holds");

		return built;
	}

	private static string Lower(JToken enumValue)
	{
		return ((string) enumValue["name"] ?? "").ToLowerInvariant();
	}

	private static string Named(Dictionary<int, string> table, int value, string what)
	{
		if (table.TryGetValue(value, out string name)) return name;
		throw new InvalidDataException($"{what} {value} has no name in the table");
	}

	private static string Named(string[] table, int value, string what)
	{
		if (value >= 0 && value < table.Length) return table[value];
		throw new InvalidDataException($"{what} {value} has no name in the table");
	}

	/// <summary>SolidPlant to solid_plant, which is how the wire spells a material.</summary>
	private static string Snake(string name)
	{
		var sb = new StringBuilder();
		for (int i = 0; i < name.Length; i++)
		{
			if (i > 0 && char.IsUpper(name[i])) sb.Append('_');
			sb.Append(char.ToLowerInvariant(name[i]));
		}

		return sb.ToString();
	}

	// ---------------------------------------------------------------- the proof

	/// <summary>
	///     Every model serialized by MiNET's own writer has to equal the frame's entry byte for
	///     byte, and the set of names has to be the frame's set. A difference is printed as the
	///     block, the leaf path and both values, and nothing is written.
	/// </summary>
	public static bool Prove(IReadOnlyDictionary<string, BlockDefinition> definitions, StartGameCapture capture)
	{
		var failures = new List<string>();

		foreach (string name in capture.BlockOrder)
		{
			if (!definitions.ContainsKey(name)) failures.Add($"{name}: the frame declares it, the extraction has no definition row for it");
		}

		foreach (string name in definitions.Keys)
		{
			if (!capture.BlockProperties.ContainsKey(name)) failures.Add($"{name}: the extraction has a definition row, the frame does not declare it");
		}

		foreach ((string name, BlockDefinition definition) in definitions)
		{
			if (!capture.BlockProperties.TryGetValue(name, out byte[] expected)) continue;

			NbtCompound tree = BlockDefinitionWriter.Write(definition);
			byte[] actual = BlockDefinitionWriter.ToNetworkBytes(tree);
			if (actual.SequenceEqual(expected)) continue;

			int before = failures.Count;
			Diff(name, tree, Parse(expected), "", failures);

			// Byte-equal trees are what is being proved, so a tree that reads the same and still
			// serializes differently is reported rather than passed.
			if (failures.Count == before)
			{
				failures.Add($"{name}: serializes to {actual.Length} bytes against the frame's {expected.Length} with no leaf difference");
			}
		}

		if (failures.Count == 0) return true;

		Console.Error.WriteLine($"block definitions proof failed on {failures.Count} points:");
		foreach (string failure in failures.Take(60)) Console.Error.WriteLine($"  {failure}");
		return false;
	}

	private static NbtCompound Parse(byte[] bytes)
	{
		var file = new NbtFile {BigEndian = false, UseVarInt = true, AllowAlternativeRootTag = false};
		using var stream = new MemoryStream(bytes, false);
		file.LoadFromStream(stream, NbtCompression.None);
		return (NbtCompound) file.RootTag;
	}

	private static void Diff(string block, NbtTag ours, NbtTag theirs, string path, List<string> failures)
	{
		if (ours.TagType != theirs.TagType)
		{
			failures.Add($"{block} {path}: generated {Describe(ours)}, the frame has {Describe(theirs)}");
			return;
		}

		switch (ours)
		{
			case NbtCompound generated when theirs is NbtCompound frame:
			{
				foreach (NbtTag child in frame)
				{
					NbtTag mine = generated.Get<NbtTag>(child.Name);
					if (mine == null) failures.Add($"{block} {path}/{child.Name}: generated nothing, the frame has {Describe(child)}");
					else Diff(block, mine, child, $"{path}/{child.Name}", failures);
				}

				foreach (NbtTag child in generated)
				{
					if (frame.Get<NbtTag>(child.Name) == null) failures.Add($"{block} {path}/{child.Name}: generated {Describe(child)}, the frame has no such field");
				}

				break;
			}
			case NbtList generated when theirs is NbtList frame:
			{
				if (generated.Count != frame.Count || generated.ListType != frame.ListType)
				{
					failures.Add($"{block} {path}: generated {Describe(generated)}, the frame has {Describe(frame)}");
					break;
				}

				for (int i = 0; i < generated.Count; i++) Diff(block, generated[i], frame[i], $"{path}[{i}]", failures);
				break;
			}
			default:
			{
				if (ours.StringValue != theirs.StringValue) failures.Add($"{block} {path}: generated {ours.StringValue}, the frame has {theirs.StringValue}");
				break;
			}
		}
	}

	private static string Describe(NbtTag tag)
	{
		return tag switch
		{
			null => "nothing",
			NbtCompound compound => $"a compound of {compound.Count}",
			NbtList list => $"a list of {list.Count} {list.ListType}",
			_ => $"{tag.TagType}({tag.StringValue})"
		};
	}

	// ---------------------------------------------------------------- emit

	/// <summary>The override one block's class carries, as C# initializer text.</summary>
	public static string Emit(BlockDefinition definition, string indent)
	{
		var sb = new StringBuilder();
		sb.AppendLine($"{indent}public override BlockDefinition CreateDefinition() => new(");

		var arguments = new List<string>
		{
			$"Tags: [{string.Join(", ", definition.Tags.Select(Text))}]",
			$"MenuCategory: new({Text(definition.MenuCategory.Category)}, {Text(definition.MenuCategory.Group)}, {Bool(definition.MenuCategory.HiddenInCommands)})",
			$"VanillaBlockData: new({definition.VanillaBlockData.BlockId}, {Text(definition.VanillaBlockData.Material)}, {Archetype(definition.VanillaBlockData.Archetype)}, " +
			$"{Bool(definition.VanillaBlockData.RequiresCorrectToolForDrops)}, {(definition.VanillaBlockData.Translucency.HasValue ? Float(definition.VanillaBlockData.Translucency.Value) : "null")}, " +
			$"{Bool(definition.VanillaBlockData.CanDampenVibrations)}, {Bool(definition.VanillaBlockData.CanOccludeVibrations)})",
			ComponentsArgument("Components", definition.Components, indent + "\t"),
			Permutations(definition.Permutations, indent + "\t")
		};

		if (definition.MolangVersion != 13) arguments.Add($"MolangVersion: {definition.MolangVersion}");

		for (int i = 0; i < arguments.Count; i++)
		{
			sb.AppendLine($"{indent}\t{arguments[i]}{(i == arguments.Count - 1 ? ");" : ",")}");
		}

		return sb.ToString();
	}

	private static string Archetype(BlockArchetype archetype)
	{
		return archetype switch
		{
			StairArchetype stair => $"new StairArchetype({Text(stair.BaseBlock)})",
			SlabArchetype slab => $"new SlabArchetype({Text(slab.DoubleSlabBlock)}, {Bool(slab.IsDouble)})",
			BushArchetype => "new BushArchetype()",
			WallFoliageArchetype foliage =>
				$"new WallFoliageArchetype({foliage.SizeCount}, {Bool(foliage.IsBonemealable)}, {Float(foliage.Bounciness)}, {Float(foliage.FallDamageMultiplier)})",
			_ => throw new InvalidDataException($"no emitter for archetype {archetype.Type}")
		};
	}

	private static string ComponentsArgument(string name, BlockComponents components, string indent)
	{
		List<string> members = ComponentMembers(components);
		if (members.Count == 0) return $"{name}: new()";

		string single = $"{name}: new({string.Join(", ", members)})";
		if (single.Length + indent.Length <= 160) return single;

		return $"{name}: new({Environment.NewLine}{indent}\t{string.Join($",{Environment.NewLine}{indent}\t", members)})";
	}

	private static List<string> ComponentMembers(BlockComponents c)
	{
		var members = new List<string>();
		if (c.DestructibleByMining.HasValue) members.Add($"DestructibleByMining: {Float(c.DestructibleByMining.Value)}");
		if (c.DestructionParticles != null)
		{
			members.Add($"DestructionParticles: new({c.DestructionParticles.ParticleCount}, {Text(c.DestructionParticles.Texture)}, {Text(c.DestructionParticles.TintMethod)})");
		}

		if (c.LiquidDetection != null)
		{
			IEnumerable<string> rules = c.LiquidDetection.Select(r =>
				$"new({Bool(r.CanContainLiquid)}, {Text(r.LiquidType)}, {Text(r.OnLiquidTouches)}, {r.StopsLiquidFromDirection}, {Bool(r.UseLiquidClipping)})");
			members.Add($"LiquidDetection: [{string.Join(", ", rules)}]");
		}

		if (c.RedstoneConductivity != null)
		{
			members.Add($"RedstoneConductivity: new({Bool(c.RedstoneConductivity.RedstoneConductor)}, {Bool(c.RedstoneConductivity.AllowsWireToStepDown)})");
		}

		if (c.SupportShape != null) members.Add($"SupportShape: {Text(c.SupportShape)}");
		if (c.CollisionBox != null)
		{
			IEnumerable<string> boxes = c.CollisionBox.Boxes.Select(b => $"new({Vector(b.Min)}, {Vector(b.Max)})");
			members.Add($"CollisionBox: new({Bool(c.CollisionBox.Enabled)}, [{string.Join(", ", boxes)}])");
		}

		if (c.SelectionBox != null) members.Add($"SelectionBox: new({Bool(c.SelectionBox.Enabled)}, {Vector(c.SelectionBox.Origin)}, {Vector(c.SelectionBox.Size)})");
		if (c.Geometry != null)
		{
			members.Add($"Geometry: new({Text(c.Geometry.Identifier)}, {Text(c.Geometry.Culling)}, {Text(c.Geometry.CullingLayer)}, {Text(c.Geometry.CullingShape)}, " +
				$"{Bool(c.Geometry.NeedsLegacyTopRotation)}, {Bool(c.Geometry.UseBlockTypeLightAbsorption)}, {Bool(c.Geometry.IgnoreGeometryForIsSolid)}, " +
				$"{Bool(c.Geometry.IsV1Fullblock)}, {Bool(c.Geometry.UvLock)})");
		}

		if (c.MaterialInstances != null)
		{
			IEnumerable<string> faces = c.MaterialInstances.Materials.Select(m =>
				$"[{Text(m.Key)}] = new({Float(m.Value.AmbientOcclusion)}, {m.Value.PackedBools}, {Text(m.Value.RenderMethod)}, {Text(m.Value.Texture)}, {Text(m.Value.TintMethod)})");
			members.Add($"MaterialInstances: new(new Dictionary<string, MaterialInstance> {{{string.Join(", ", faces)}}})");
		}

		if (c.Transformation != null)
		{
			members.Add($"Transformation: new({Vector(c.Transformation.Translation)}, {Vector(c.Transformation.Rotation)}, {Vector(c.Transformation.RotationPivot)}, " +
				$"{Vector(c.Transformation.Scale)}, {Vector(c.Transformation.ScalePivot)}, {Bool(c.Transformation.HasJsonVersionBeforeValidation)})");
		}

		if (c.ConnectionRule != null)
		{
			members.Add($"ConnectionRule: new({Text(c.ConnectionRule.AcceptsConnectionsFrom)}, [{string.Join(", ", c.ConnectionRule.EnabledDirections.Select(Text))}])");
		}

		if (c.PrecipitationBehavior != null) members.Add($"PrecipitationBehavior: {Text(c.PrecipitationBehavior)}");
		if (c.Replaceable.HasValue) members.Add($"Replaceable: {Bool(c.Replaceable.Value)}");
		return members;
	}

	private static string Permutations(IReadOnlyList<BlockPermutation> permutations, string indent)
	{
		if (permutations.Count == 0) return "Permutations: []";

		var sb = new StringBuilder();
		sb.Append("Permutations:").AppendLine().Append(indent).Append('[');
		for (int i = 0; i < permutations.Count; i++)
		{
			sb.AppendLine().Append(indent).Append('\t');
			sb.Append($"new({Text(permutations[i].Condition)}, new({string.Join(", ", ComponentMembers(permutations[i].Components))}))");
			if (i < permutations.Count - 1) sb.Append(',');
		}

		sb.AppendLine().Append(indent).Append(']');
		return sb.ToString();
	}

	private static string Vector(Vec3 value) => $"new({Float(value.X)}, {Float(value.Y)}, {Float(value.Z)})";

	private static string Vector(Vec3I value) => $"new({value.X}, {value.Y}, {value.Z})";

	private static string Bool(bool value) => value ? "true" : "false";

	/// <summary>
	///     A float literal that reads back as the same float. The proof runs on the model, so a
	///     literal that loses a bit would pass it and still put the wrong bytes on the wire; the
	///     round trip is checked here, where the literal is made.
	/// </summary>
	private static string Float(float value)
	{
		string literal = value.ToString("R", CultureInfo.InvariantCulture);
		if (!float.TryParse(literal, NumberStyles.Float, CultureInfo.InvariantCulture, out float read) || read.Equals(value) == false)
		{
			throw new InvalidDataException($"the float {value} has no literal that reads back as itself");
		}

		return literal + "f";
	}

	private static string Text(string value)
	{
		return "\"" + value.Replace("\\", "\\\\").Replace("\"", "\\\"") + "\"";
	}

	// ---------------------------------------------------------------- experiments

	/// <summary>
	///     The experiments the server declares in StartGame, from the extraction's own record of the
	///     world it read: <c>source.experiments</c> in creative_items.json is the set that world had
	///     ON, which is why the extraction trusts its registry. The frame is the check, and a
	///     difference is reported rather than resolved.
	/// </summary>
	public static void ReportExperiments(string creativeItemsPath, StartGameCapture capture)
	{
		var extract = JObject.Parse(File.ReadAllText(creativeItemsPath));
		List<string> experiments = ((JArray) extract["source"]["experiments"]).Select(e => (string) e).ToList();

		var frame = capture.Experiments.Select(e => e.Name).ToList();
		foreach (string missing in frame.Where(f => !experiments.Contains(f)))
		{
			Console.WriteLine($"experiments: the frame declares {missing}, the extraction's world does not have it on");
		}

		foreach (string extra in experiments.Where(e => !frame.Contains(e)))
		{
			Console.WriteLine($"experiments: the extraction's world has {extra} on, the frame does not declare it");
		}

		foreach ((string name, bool enabled) in capture.Experiments)
		{
			if (!enabled) Console.WriteLine($"experiments: the frame declares {name} off, and the extraction only records the ones that are on");
		}

	}
}
