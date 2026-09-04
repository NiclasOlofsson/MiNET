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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace MiNET.BdsExtract.Binary;

/// <summary>
///     One class as the image states it, in the family that names it.
///     <para>
///         The two families answer different questions with the same words, so the names differ
///         with them. An item component is constructed by its own schema loader, so
///         <see cref="Vtable" /> is the class's own table and <see cref="Size" /> the size its
///         deleting destructor hands the deallocator. A block component is constructed inside a
///         storage node that holds it, so what the construction path stores is the node's table and
///         the node's size, not the component's: they are named nodeVtable and nodeSize for that
///         reason, and eighteen block classes fold onto one node table because a node holding eight
///         bytes or less is the same node whatever it holds.
///     </para>
/// </summary>
public sealed record BinaryClass(
	string Name,
	string Family,
	uint TypeIdSlot,
	int SlotWitnesses,
	IReadOnlyList<uint> SlotCandidates,
	uint Vtable,
	int VtableWitnesses,
	IReadOnlyList<uint> VtableCandidates,
	int Size,
	IReadOnlyList<int> SizeCandidates,
	bool? Networked,
	string NetworkedBytes,
	IReadOnlyList<string> SharedVtableWith);

/// <summary>
///     What bedrock_server.exe states about itself, read from the file on disk before anything is
///     read out of a running process: the component classes each storage family names, where each
///     one's type id will be written, its method table, its size and its networked verdict, the
///     value-to-name table of every enum the reference states, the member accesses the serializers
///     perform, and the reflection bindings the EnTT binder registers.
///     <para>
///         Every position here is a relative virtual address of the image on disk. A live phase adds
///         the process's image base; nothing in this file is a process address.
///     </para>
///     <para>
///         Nothing is dropped. A class whose slot or table did not resolve is written with its
///         candidates, an enum whose table was rejected is written with the reason, and both are
///         counted again under <c>unresolved</c> so the holes are countable from the file alone.
///     </para>
/// </summary>
public sealed class BinaryFacts
{
	/// <summary>
	///     The item component family: the schema loader that allocates the class. Several owners name
	///     the same class, and only this one's construction path stores the class's own table into
	///     the object it just allocated; the rest build the schema machinery around it and make every
	///     class ambiguous. The loader is instantiated for far more than components, so the family is
	///     the loader AND a name ending in ItemComponent, and the classes left out are counted in the
	///     families block rather than passed over in silence.
	/// </summary>
	private const string ItemFamilyFormat = "cereal::internal::TypeSchema<{0}>";

	private const string ItemSuffix = "ItemComponent";

	/// <summary>The block component family: one storage template names every block component class this build has.</summary>
	private const string BlockFamily = "BlockComponentStorage";

	private BinaryFacts()
	{
	}

	public string ExePath { get; private init; }
	public string Sha256 { get; private init; }
	public string Build { get; private init; }
	public ulong ImageBase { get; private init; }
	public int TextBytes { get; private init; }
	public int FunctionRanges { get; private init; }
	public int ChainedRanges { get; private init; }
	public long IndexMilliseconds { get; private init; }
	public long ElapsedMilliseconds { get; private set; }

	public IReadOnlyList<BinaryClass> Classes { get; private init; }

	/// <summary>Per family: how many classes the signature strings name, and how many of them the class table holds.</summary>
	public IReadOnlyList<(string Family, string Filter, int Named, int Taken)> Families { get; private init; }
	public IReadOnlyList<EnumTable> Enums { get; private init; }
	public IReadOnlyList<MemberAccess> Members { get; private init; }
	public IReadOnlyList<Binding> Bindings { get; private init; }

	/// <summary>
	///     Every method table a block definition builds its components on: its networked verdict,
	///     the component classes its slots name, and the tag literals they reference. A definition
	///     component carries no id, so the table is its only identity and this is what the image
	///     says about each one.
	/// </summary>
	public IReadOnlyList<BlockDescriptionTable> BlockDescriptions { get; private init; }

	/// <summary>Everything the run could not settle, each with the reason it could not. A count here is a hole, not a failure to report one.</summary>
	public IReadOnlyList<(string What, string Why)> Unresolved { get; private init; }

	/// <summary>
	///     Where the image the facts were read from is loaded in the process, from
	///     <see cref="BedrockProcess.ModuleBase" />, which is the main module's base address: the
	///     executable's own load address and no library's. Nought until <see cref="ResolveLive" />
	///     has run, and it is the only thing here that turns an RVA into a process address.
	/// </summary>
	public ulong ProcessImageBase { get; private set; }

	/// <summary>
	///     What class each block component id stands for ON THIS RUN. A type id is not in the image:
	///     Bedrock writes one into the class's own static the first time something asks for that
	///     component, so the map is a fact about one server's lifetime and the class behind it is
	///     the fact about the game.
	/// </summary>
	public IReadOnlyDictionary<int, string> LiveIds { get; private set; } = new Dictionary<int, string>();

	/// <summary>
	///     Every method table these classes name, by the address it loaded at: the class's own table
	///     for an item component, the storage node's table for a block component. Where the linker
	///     folded several classes onto one table the name is all of them, because the table can no
	///     longer tell them apart and saying one would be picking.
	/// </summary>
	public IReadOnlyDictionary<ulong, string> VtableNames { get; private set; } = new Dictionary<ulong, string>();

	/// <summary>What <see cref="ResolveLive" /> has to say, for the caller to print. One line, one fact.</summary>
	public IReadOnlyList<string> LiveReport { get; private set; } = Array.Empty<string>();

	private Dictionary<ulong, List<string>> _vtableClasses = new();

	/// <summary>
	///     The classes whose method table loaded at this address: one, or every class the linker
	///     folded onto it. Empty where no class in the inventory names that table.
	/// </summary>
	public IReadOnlyList<string> ClassesAt(ulong vtable)
	{
		return _vtableClasses.TryGetValue(vtable, out List<string> names) ? names : Array.Empty<string>();
	}

	/// <summary>
	///     Where this block component class's storage node keeps its method table in the process,
	///     or false when the image settles none for it. False is not a disagreement: it is one
	///     statement missing, and the caller has nothing to hold anything against.
	/// </summary>
	public bool NodeTable(string name, out ulong vtable)
	{
		vtable = 0;
		if (ProcessImageBase == 0) return false;

		BinaryClass entry = Classes.FirstOrDefault(c => c.Family == BlockFamily && c.Name == name);
		if (entry is null || entry.Vtable == 0) return false;

		vtable = ProcessImageBase + entry.Vtable;
		return true;
	}

	/// <summary>
	///     What the image states about the method table a definition component was built on, or
	///     false when it states nothing about that address. The address is a process address, so
	///     the module's load address comes off it first and the answer is about the image.
	/// </summary>
	public bool TryDescription(ulong processTable, out BlockDescriptionTable table)
	{
		table = null;
		if (ProcessImageBase == 0 || processTable < ProcessImageBase) return false;

		var rva = (uint) (processTable - ProcessImageBase);
		table = BlockDescriptions?.FirstOrDefault(t => t.Rva == rva);
		return table is not null;
	}

	/// <summary>
	///     Every item component class the binary names by this reference class name: the bare name
	///     the runtime class carries, and every namespaced spelling of it, which is how the
	///     versioned schema structs are written (SharedTypes::v1_21_10::DamageAbsorptionItemComponent
	///     is the same class name under the version that introduced it). All of them are handed back
	///     rather than one being chosen, because which of them an object leads with is the question
	///     rather than the answer.
	/// </summary>
	public IReadOnlyList<BinaryClass> ItemClasses(string referenceName)
	{
		return Classes
			.Where(c => c.Family != BlockFamily)
			.Where(c => c.Name == referenceName || c.Name.EndsWith("::" + referenceName, StringComparison.Ordinal))
			.OrderBy(c => c.Name, StringComparer.Ordinal)
			.ToList();
	}

	/// <summary>
	///     Reads the two things that only exist while the server runs: the id each block component
	///     class was handed, and where each class's method table sits in this process.
	///     <para>
	///         A slot holding nought is not a registration. It is the loader's zero, shared by every
	///         class nothing has asked for, so those classes are named as never asked for rather
	///         than published under an id they do not have.
	///     </para>
	///     <para>
	///         Two classes reading the same id means this is not reading what it thinks it is, so
	///         neither is published and both are named.
	///     </para>
	/// </summary>
	public void ResolveLive(BedrockProcess process)
	{
		var report = new List<string>();
		ProcessImageBase = process.ModuleBase;
		if (ProcessImageBase == 0)
		{
			report.Add("live ids: the server's image is not mapped, so no type id slot and no method table has an address");
			LiveReport = report;
			return;
		}

		var slot = new byte[2];
		var claims = new Dictionary<int, List<string>>();
		var silent = new List<string>();
		var unread = new List<string>();
		var slotless = new List<string>();
		int block = 0;
		foreach (BinaryClass entry in Classes.Where(c => c.Family == BlockFamily).OrderBy(c => c.Name, StringComparer.Ordinal))
		{
			block++;
			if (entry.TypeIdSlot == 0 || entry.SlotWitnesses == 0)
			{
				slotless.Add(entry.Name);
				continue;
			}
			if (!process.TryRead(ProcessImageBase + entry.TypeIdSlot, slot, slot.Length))
			{
				unread.Add(entry.Name);
				continue;
			}

			int id = BitConverter.ToUInt16(slot, 0);
			if (id == 0)
			{
				silent.Add(entry.Name);
				continue;
			}

			if (!claims.TryGetValue(id, out List<string> holders)) claims[id] = holders = new List<string>();
			holders.Add(entry.Name);
		}

		var ids = new Dictionary<int, string>();
		var shared = new List<string>();
		foreach ((int id, List<string> holders) in claims.OrderBy(c => c.Key))
		{
			if (holders.Count == 1) ids[id] = holders[0];
			else shared.Add($"id {id} is claimed by {string.Join(", ", holders)}, so none of them is read");
		}
		LiveIds = ids;

		var byTable = new Dictionary<ulong, List<string>>();
		foreach (BinaryClass entry in Classes.Where(c => c.Vtable != 0).OrderBy(c => c.Name, StringComparer.Ordinal))
		{
			ulong at = ProcessImageBase + entry.Vtable;
			if (!byTable.TryGetValue(at, out List<string> names)) byTable[at] = names = new List<string>();
			names.Add(entry.Name);
		}
		_vtableClasses = byTable;
		VtableNames = byTable.ToDictionary(t => t.Key, t => string.Join(", ", t.Value));

		report.Add($"live ids: {block:N0} block component classes in the binary, {LiveIds.Count:N0} registered on this run, "
			+ $"{silent.Count:N0} never asked for"
			+ (slotless.Count > 0 ? $", {slotless.Count:N0} with no slot in the binary" : "")
			+ (unread.Count > 0 ? $", {unread.Count:N0} whose slot did not read back" : "")
			+ (shared.Count > 0 ? $", {shared.Count:N0} number(s) claimed twice" : ""));
		if (slotless.Count > 0) report.Add($"  no type id slot in the binary: {string.Join(", ", slotless)}");
		if (unread.Count > 0) report.Add($"  slot did not read back: {string.Join(", ", unread)}");
		if (silent.Count > 0) report.Add($"  never asked for: {string.Join(", ", silent)}");
		foreach (string line in shared) report.Add($"  {line}");
		report.Add($"method tables: {byTable.Count:N0} addresses for {Classes.Count(c => c.Vtable != 0):N0} classes, "
			+ $"{byTable.Count(t => t.Value.Count > 1):N0} of them shared by more than one class");
		LiveReport = report;
	}

	/// <summary>
	///     The dictionary the entt meta type ids are resolved against. Clang bakes
	///     <c>entt::type_name&lt;T&gt;</c> into the image as its own signature literal, so every type
	///     the binder ever names is already in the signature inventory, nested enums included
	///     (<c>UseModifiersItemComponent::StartUsing</c> is a literal in this build). The C++
	///     primitives are added because a primitive has no template instantiation to leave one.
	/// </summary>
	public static IReadOnlyList<string> TypeNames(PeImage image)
	{
		var names = new List<string>
		{
			"bool", "char", "signed char", "unsigned char", "short", "unsigned short",
			"int", "unsigned int", "long", "unsigned long", "long long", "unsigned long long",
			"float", "double", "void", "std::string", "std::basic_string<char>", "std::string_view"
		};
		foreach (InventoryClass entry in SignatureStrings.Find(image))
		{
			names.Add(entry.Name);
			if (entry.Args != null) names.Add(entry.Name + entry.Args);
		}
		return names.Distinct(StringComparer.Ordinal).ToList();
	}

	/// <summary>
	///     The wire names to look for, gathered from the two schema trees that state them: Mojang's
	///     item component schemas committed under MiNET.BlockGen, and the block schemas the target
	///     server writes into its own docs folder. Every property name in them is taken, at every
	///     depth, because which of them is a member is what the binding answers rather than what
	///     this decides. A tree that is not there is reported as missing, never quietly skipped.
	/// </summary>
	public static IReadOnlyList<string> WireNames(string exePath, List<(string What, string Why)> unresolved)
	{
		var names = new HashSet<string>(StringComparer.Ordinal);
		foreach (string root in SchemaRoots(exePath, unresolved))
		{
			foreach (string file in Directory.GetFiles(root, "*.json", SearchOption.AllDirectories))
			{
				try
				{
					using JsonDocument document = JsonDocument.Parse(File.ReadAllText(file));
					CollectNames(document.RootElement, names);
				}
				catch (JsonException e)
				{
					unresolved.Add(($"schema {Path.GetFileName(file)}", $"does not parse as JSON: {e.Message}"));
				}
			}
		}
		return names.Where(n => n.Length >= 2).OrderBy(n => n, StringComparer.Ordinal).ToList();
	}

	/// <summary>
	///     Reads the image and answers every family. The enum seeds are the reference's own tables,
	///     which are both what finds the function and the criterion a candidate has to reproduce;
	///     the wire names are the schemas'.
	/// </summary>
	public static BinaryFacts Read(string exePath, IReadOnlyList<string> wireNames, IReadOnlyList<EnumSeed> enumSeeds, string build,
		IReadOnlyList<string> descriptionTags = null, IReadOnlyList<string> descriptionNames = null)
	{
		var clock = Stopwatch.StartNew();
		var unresolved = new List<(string What, string Why)>();

		var image = PeImage.Open(exePath);
		var indexClock = Stopwatch.StartNew();
		var code = new CodeIndex(image);
		indexClock.Stop();

		IReadOnlyList<InventoryClass> inventory = SignatureStrings.Find(image);
		List<InventoryClass> blocks = inventory.Where(c => c.Family == BlockFamily).ToList();
		List<InventoryClass> schemas = inventory.Where(c => c.Family == string.Format(ItemFamilyFormat, c.Name)).ToList();
		List<InventoryClass> items = schemas.Where(c => c.Name.EndsWith(ItemSuffix, StringComparison.Ordinal)).ToList();

		var families = new List<(string Family, string Filter, int Named, int Taken)>
		{
			(BlockFamily, "every class the storage template names", blocks.Count, blocks.Count),
			("cereal::internal::TypeSchema<T>", $"the loader's own instantiations whose name ends in {ItemSuffix}", schemas.Count, items.Count)
		};

		// The type id is a block-side fact. Bedrock hands a block component type a 16-bit id the
		// first time something asks for it and keeps it in a writable static beside the templated
		// accessor; the item side resolves its components through the schema loader and has no such
		// static, so looking for one there would report 113 holes that are not holes.
		var classes = new List<BinaryClass>();
		classes.AddRange(Family(code, image, blocks, true, unresolved));
		classes.AddRange(Family(code, image, items, false, unresolved));

		IReadOnlyList<EnumTable> enums = enumSeeds.Count == 0
			? Array.Empty<EnumTable>()
			: EnumTables.Find(code, image, enumSeeds);
		foreach (EnumTable table in enums.Where(t => !t.Accepted))
		{
			unresolved.Add(($"enum {table.Name}", table.Evidence));
		}

		IReadOnlyList<MemberAccess> members = MemberAccesses.Find(code, image, wireNames);
		IReadOnlyList<Binding> bindings = Binary.Bindings.Find(code, image, wireNames, TypeNames(image));
		foreach (Binding binding in bindings.Where(b => b.Owner == null))
		{
			unresolved.Add(($"binding {binding.WireName} at 0x{binding.SiteRva:x}", $"no owner: {binding.Evidence}"));
		}
		foreach (Binding binding in bindings.Where(b => b.TypeName == null))
		{
			unresolved.Add(($"binding {binding.WireName} at 0x{binding.SiteRva:x}", $"type id 0x{binding.TypeHash:x8} reproduces no name in the signature inventory"));
		}

		// The definition side's method tables. The tags are the reference's, because they are what
		// tells one description class from another where the image names no class in the table's
		// own slots, and a list this phase kept would be a list that goes stale on its own.
		IReadOnlyList<BlockDescriptionTable> descriptions = Vtables.Descriptions(code, image, inventory,
			descriptionTags ?? Array.Empty<string>(), descriptionNames ?? Array.Empty<string>());

		var bound = bindings.Select(b => b.WireName).ToHashSet(StringComparer.Ordinal);
		var accessed = members.Select(m => m.WireName).ToHashSet(StringComparer.Ordinal);
		foreach (string name in wireNames.Where(n => !bound.Contains(n) && !accessed.Contains(n)))
		{
			unresolved.Add(($"wire name {name}", "no registration and no member access reference the literal"));
		}

		var facts = new BinaryFacts
		{
			ExePath = exePath,
			Sha256 = Digest(exePath),
			Build = build,
			ImageBase = image.ImageBase,
			TextBytes = image.Text.RawSize,
			FunctionRanges = image.Functions.Count,
			ChainedRanges = code.ChainedRanges,
			IndexMilliseconds = indexClock.ElapsedMilliseconds,
			Classes = classes,
			Families = families,
			Enums = enums,
			Members = members,
			Bindings = bindings,
			BlockDescriptions = descriptions,
			Unresolved = unresolved
		};
		clock.Stop();
		facts.ElapsedMilliseconds = clock.ElapsedMilliseconds;
		return facts;
	}

	public void Write(string path)
	{
		var document = new JsonObject
		{
			["build"] = Build,
			["imageBase"] = $"0x{ImageBase:X}",
			// How long the read took is not a fact about the build. It goes to the log line the run
			// prints, so two runs of the same binary write the same file byte for byte and a
			// difference between them is a difference in what was read.
			["exe"] = new JsonObject
			{
				["path"] = ExePath,
				["sha256"] = Sha256,
				["textBytes"] = TextBytes,
				["functionRanges"] = FunctionRanges,
				["chainedRanges"] = ChainedRanges
			}
		};

		var families = new JsonArray();
		foreach ((string family, string filter, int named, int taken) in Families)
		{
			families.Add(new JsonObject
			{
				["family"] = family,
				["filter"] = filter,
				["namedBySignatureStrings"] = named,
				["inTheClassTable"] = taken
			});
		}
		document["families"] = families;

		var classes = new JsonArray();
		foreach (BinaryClass entry in Classes)
		{
			bool node = entry.Family == BlockFamily;
			var row = new JsonObject
			{
				["name"] = entry.Name,
				["family"] = entry.Family,
				[node ? "nodeVtable" : "vtable"] = entry.Vtable == 0 ? null : $"0x{entry.Vtable:X}",
				["vtableWitnesses"] = entry.VtableWitnesses,
				[node ? "nodeSize" : "size"] = entry.Size,
				["networked"] = entry.Networked,
				["sharedVtableWith"] = Strings(entry.SharedVtableWith)
			};
			if (node)
			{
				row["typeIdSlot"] = entry.TypeIdSlot == 0 ? null : $"0x{entry.TypeIdSlot:X}";
				row["slotWitnesses"] = entry.SlotWitnesses;
				if (entry.SlotCandidates.Count > 1 || entry.TypeIdSlot == 0) row["slotCandidates"] = Addresses(entry.SlotCandidates);
			}
			if (entry.NetworkedBytes != null) row["networkedBytes"] = entry.NetworkedBytes;
			if (entry.VtableCandidates.Count > 1 || entry.Vtable == 0) row["vtableCandidates"] = Addresses(entry.VtableCandidates);
			if (entry.SizeCandidates.Count > 1) row["sizeCandidates"] = new JsonArray(entry.SizeCandidates.Select(s => (JsonNode) s).ToArray());
			classes.Add(row);
		}
		document["classes"] = classes;

		var enums = new JsonObject();
		foreach (EnumTable table in Enums.OrderBy(t => t.Name, StringComparer.Ordinal))
		{
			var values = new JsonObject();
			foreach (KeyValuePair<int, string> value in table.Values.OrderBy(v => v.Key)) values[value.Key.ToString()] = value.Value;
			enums[table.Name] = new JsonObject
			{
				["accepted"] = table.Accepted,
				["function"] = table.FunctionRva == 0 ? null : $"0x{table.FunctionRva:X}",
				["evidence"] = table.Evidence,
				["values"] = values
			};
		}
		document["enums"] = enums;

		var bindings = new JsonArray();
		foreach (Binding binding in Bindings)
		{
			bindings.Add(new JsonObject
			{
				["wireName"] = binding.WireName,
				["owner"] = binding.Owner,
				["offset"] = binding.Offset,
				["typeHash"] = $"0x{binding.TypeHash:X8}",
				["typeName"] = binding.TypeName,
				["width"] = binding.Width,
				["site"] = $"0x{binding.SiteRva:X}",
				["evidence"] = binding.Evidence
			});
		}
		document["bindings"] = bindings;

		var members = new JsonArray();
		foreach (MemberAccess access in Members)
		{
			members.Add(new JsonObject
			{
				["wireName"] = access.WireName,
				["function"] = $"0x{access.FunctionRva:X}",
				["instruction"] = $"0x{access.InstructionRva:X}",
				["offset"] = access.Offset,
				["width"] = access.Width,
				["operand"] = access.Operand,
				["store"] = access.IsStore,
				["register"] = access.Register
			});
		}
		document["members"] = members;

		// Only the tables that state something travel. A candidate whose slots name no class and
		// reference no tag is a table this phase reached and could not tell apart, and a file of
		// nine hundred of those says nothing that the count in the summary does not.
		var descriptions = new JsonArray();
		foreach (BlockDescriptionTable table in BlockDescriptions.Where(t => t.Installs.Count > 0 || t.States.Count > 0 || t.Names.Count > 0 || t.Constructs.Count > 0))
		{
			descriptions.Add(new JsonObject
			{
				["vtable"] = $"0x{table.Rva:X}",
				["networked"] = table.Networked,
				["networkedBytes"] = table.NetworkedBytes,
				["installs"] = new JsonArray(table.Installs.Select(n => (JsonNode) n).ToArray()),
				["states"] = new JsonArray(table.States.Select(n => (JsonNode) n).ToArray()),
				["names"] = new JsonArray(table.Names.Select(n => (JsonNode) n).ToArray()),
				["constructs"] = new JsonArray(table.Constructs.Select(n => (JsonNode) n).ToArray())
			});
		}
		document["blockDescriptions"] = descriptions;

		var unresolved = new JsonArray();
		foreach ((string what, string why) in Unresolved)
		{
			unresolved.Add(new JsonObject { ["what"] = what, ["why"] = why });
		}
		document["unresolved"] = unresolved;

		File.WriteAllText(path, BlockDocument.Serialize(document), new UTF8Encoding(false));
	}

	/// <summary>The one line a run prints about this phase.</summary>
	public string Summary()
	{
		int stated = BlockDescriptions?.Count(t => t.Installs.Count > 0 || t.States.Count > 0 || t.Names.Count > 0 || t.Constructs.Count > 0) ?? 0;
		return $"binary facts: {Classes.Count} classes, {Enums.Count(e => e.Accepted)} of {Enums.Count} enums, "
			+ $"{Members.Count} member accesses, {Bindings.Count} bindings, "
			+ $"{BlockDescriptions?.Count ?? 0} definition component tables of which {stated} state a class or a tag, "
			+ $"{Unresolved.Count} unresolved "
			+ $"({ElapsedMilliseconds / 1000.0:F1}s, index {IndexMilliseconds / 1000.0:F1}s)";
	}

	// ---- the families ------------------------------------------------------------------------

	private static IEnumerable<BinaryClass> Family(CodeIndex code, PeImage image, IReadOnlyList<InventoryClass> classes, bool hasTypeIds, List<(string What, string Why)> unresolved)
	{
		if (classes.Count == 0) yield break;

		IReadOnlyList<TypeIdSlot> slots = hasTypeIds ? TypeIdSlots.Find(image, code, classes) : Array.Empty<TypeIdSlot>();
		IReadOnlyList<ClassVtable> vtables = Vtables.Find(code, image, classes);
		var slotByName = slots.ToDictionary(s => s.Name, s => s, StringComparer.Ordinal);
		var vtableByName = vtables.ToDictionary(v => v.Name, v => v, StringComparer.Ordinal);

		foreach (InventoryClass entry in classes)
		{
			TypeIdSlot slot = slotByName.GetValueOrDefault(entry.Name);
			ClassVtable vtable = vtableByName.GetValueOrDefault(entry.Name);

			if (hasTypeIds && (slot == null || slot.Witnesses == 0))
			{
				unresolved.Add(($"{entry.Family} {entry.Name} type id slot",
					slot == null || slot.Candidates.Count == 0
						? "no writable static is read by any function carrying the class's signature literal"
						: $"{slot.Candidates.Count} candidate statics: {string.Join(", ", slot.Candidates.Select(c => $"0x{c:X}"))}"));
			}
			if (vtable == null || vtable.VtableRva == 0)
			{
				unresolved.Add(($"{entry.Family} {entry.Name} vtable",
					vtable == null || vtable.Candidates.Count == 0
						? "no construction path stores a read-only table into the object's first word"
						: $"{vtable.Candidates.Count} candidate tables: {string.Join(", ", vtable.Candidates.Select(c => $"0x{c:X}"))}"));
			}
			else if (vtable.Size == 0)
			{
				unresolved.Add(($"{entry.Family} {entry.Name} size", "the deleting destructor hands no size to a deallocator"));
			}

			yield return new BinaryClass(
				entry.Name,
				entry.Family,
				slot?.SlotRva ?? 0,
				slot?.Witnesses ?? 0,
				slot?.Candidates ?? Array.Empty<uint>(),
				vtable?.VtableRva ?? 0,
				vtable?.Witnesses ?? 0,
				vtable?.Candidates ?? Array.Empty<uint>(),
				vtable?.Size ?? 0,
				vtable?.SizeCandidates ?? Array.Empty<int>(),
				vtable?.Networked,
				vtable?.NetworkedBytes,
				vtable?.SharedWith ?? Array.Empty<string>());
		}
	}

	// ---- the schema trees --------------------------------------------------------------------

	private static IEnumerable<string> SchemaRoots(string exePath, List<(string What, string Why)> unresolved)
	{
		string server = Path.GetDirectoryName(exePath);
		string blocks = server == null ? null : Path.Combine(server, "docs", "json_schemas", "server", "block");
		if (blocks != null && Directory.Exists(blocks)) yield return blocks;
		else unresolved.Add(("block schemas", $"{blocks ?? "(no server folder)"} does not exist, so no block wire name was looked for"));

		string items = RepoDirectory(Path.Combine("src", "MiNET", "MiNET.BlockGen", "Schemas", "item"));
		if (items != null) yield return items;
		else unresolved.Add(("item schemas", "MiNET.BlockGen/Schemas/item is not in this checkout, so no item wire name was looked for"));
	}

	/// <summary>A folder inside the checkout this build came from, or null when the run is not inside one.</summary>
	private static string RepoDirectory(string relative)
	{
		var directory = new DirectoryInfo(AppContext.BaseDirectory);
		while (directory is not null)
		{
			string candidate = Path.Combine(directory.FullName, relative);
			if (Directory.Exists(candidate)) return candidate;
			directory = directory.Parent;
		}
		return null;
	}

	private static void CollectNames(JsonElement element, HashSet<string> names)
	{
		if (element.ValueKind == JsonValueKind.Object)
		{
			foreach (JsonProperty property in element.EnumerateObject())
			{
				names.Add(property.Name);
				CollectNames(property.Value, names);
			}
			return;
		}
		if (element.ValueKind == JsonValueKind.Array)
		{
			foreach (JsonElement item in element.EnumerateArray()) CollectNames(item, names);
		}
	}

	/// <summary>The executable's SHA-256, which is what a saved live scan has to carry to say which image it was read on.</summary>
	public static string Digest(string path)
	{
		using FileStream stream = File.OpenRead(path);
		return Convert.ToHexString(SHA256.HashData(stream)).ToLowerInvariant();
	}

	private static JsonArray Addresses(IEnumerable<uint> values)
	{
		return new JsonArray(values.Select(v => (JsonNode) $"0x{v:X}").ToArray());
	}

	private static JsonArray Strings(IEnumerable<string> values)
	{
		return new JsonArray(values.Select(v => (JsonNode) v).ToArray());
	}
}