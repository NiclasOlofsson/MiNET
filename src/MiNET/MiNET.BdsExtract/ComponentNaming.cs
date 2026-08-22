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

using System.Text;

/// <summary>
///     Names the components a block carries, by class rather than by the number the server happens
///     to have registered them under this run.
///     <para>
///         The number is useless as an identity: it is handed out at startup, so the same component
///         is 33 in one process and 38 in the next, and every list keyed by it disagrees with
///         itself between runs. A component's CLASS does not move. Its method table lives in the
///         server image, so the distance from the image's start to that table is the same in every
///         process of the same build, and that distance is what identifies the type here.
///     </para>
///     <para>
///         The name comes from the registry the reflection system keeps: a table of records, each
///         keyed by the hash of a component's name and carrying pointers into the image. The names
///         are not in memory as text, but the server publishes them as schemas, so hashing those
///         gives the number each record is keyed by. Where a record points at the same method table
///         an instance uses, the record's name is that instance's name, and nothing was guessed.
///     </para>
/// </summary>
public static class ComponentNaming
{
	/// <summary>Where a record keeps its pointers into the image, relative to the hash it is keyed by.</summary>
	private const int RecordPointersFrom = 8;
	private const int RecordPointersTo = 104;

	/// <summary>How much of one component object to show when looking at a single block's list.</summary>
	private const int MaximumDump = 264;

	public static int Run(string[] args)
	{
		string pathFilter = null;
		string schemas = null;
		string output = Path.Combine(Program.DefaultOutputDirectory(), "component-naming.json");
		for (int i = 0; i < args.Length; i++)
		{
			if (args[i] == "--server" && i + 1 < args.Length) pathFilter = args[++i];
			else if (args[i] == "--schemas" && i + 1 < args.Length) schemas = args[++i];
			else if (args[i] == "--out" && i + 1 < args.Length) output = args[++i];
		}

		if (schemas is null || !Directory.Exists(schemas))
		{
			Console.Error.WriteLine("usage: MiNET.BdsExtract --name-components --schemas <docs/json_schemas/server/block> [--server <fragment>]");
			return 2;
		}

		using var process = BedrockProcess.Attach(pathFilter);
		Console.WriteLine($"reading pid {process.Id}");
		if (Membership(process, output) is { } code) return code;

		// What the blocks actually carry, gathered by the class each instance belongs to.
		var word = new byte[8];
		var blocks = BlockRegistry.Read(process);
		var byClass = new Dictionary<long, ClassUse>();
		int instances = 0;
		foreach (BlockProperties block in blocks)
		{
			foreach ((int id, ulong at) in ComponentReader.Instances(process, block.Address, word))
			{
				ulong vtable = process.ReadUInt64(at, word);
				if (vtable <= process.ModuleBase) continue;
				long rva = (long) (vtable - process.ModuleBase);
				if (!byClass.TryGetValue(rva, out ClassUse use)) byClass[rva] = use = new ClassUse();
				use.Instances++;
				use.Ids.Add(id);
				use.Example ??= block.Name;
				instances++;
			}
		}

		Console.WriteLine($"blocks: {blocks.Count:N0}, component instances: {instances:N0}, distinct classes: {byClass.Count:N0}");

		// The registry's records, found by the hashes of the names the server publishes.
		Dictionary<uint, string> byHash = HashProbe.NamesByHash(schemas);
		Console.WriteLine($"names the schemas publish: {byHash.Count:N0}");

		var records = new Dictionary<long, string>();   // an image offset a record points at -> its name
		var pointed = new byte[RecordPointersTo];
		int found = 0;
		foreach ((ulong at, uint hash) in HashProbe.Hits(process, byHash.Keys.ToHashSet()))
		{
			if (!process.TryRead(at, pointed, pointed.Length)) continue;
			found++;
			for (int offset = RecordPointersFrom; offset + 8 <= RecordPointersTo; offset += 8)
			{
				ulong value = BitConverter.ToUInt64(pointed, offset);
				if (value <= process.ModuleBase || value - process.ModuleBase > 0x10000000) continue;
				records[(long) (value - process.ModuleBase)] = byHash[hash];
			}
		}

		Console.WriteLine($"records read: {found:N0}, image offsets they point at: {records.Count:N0}");

		// Where the number a component goes by is written down beside its name. The number is handed
		// out at startup, so it is not in the image and not in the schemas; it can only be read from
		// this process. Every place a name's hash appears is examined for a small number near it,
		// and the position that gives EVERY name a different number is the one that holds the
		// binding. A position that gives two names the same number is not it, whatever it looks
		// like, so nothing is accepted on one example.
		var wanted = byHash.Where(p => p.Value.StartsWith("minecraft:", StringComparison.Ordinal))
			.ToDictionary(p => p.Key, p => p.Value);
		var seenIds = byClass.Values.SelectMany(u => u.Ids).ToHashSet();

		var candidates = new Dictionary<int, Dictionary<string, HashSet<int>>>();
		var around = new byte[80];
		foreach ((ulong at, uint hash) in HashProbe.Hits(process, wanted.Keys.ToHashSet()))
		{
			if (at < 32 || !process.TryRead(at - 32, around, around.Length)) continue;
			for (int offset = 0; offset + 4 <= around.Length; offset += 4)
			{
				if (offset == 32) continue;                       // the hash itself
				int value = BitConverter.ToInt32(around, offset);
				if (!seenIds.Contains(value)) continue;           // only numbers blocks actually use
				int relative = offset - 32;
				if (!candidates.TryGetValue(relative, out Dictionary<string, HashSet<int>> map))
					candidates[relative] = map = new Dictionary<string, HashSet<int>>(StringComparer.Ordinal);
				if (!map.TryGetValue(wanted[hash], out HashSet<int> values)) map[wanted[hash]] = values = [];
				values.Add(value);
			}
		}

		// The other way a name could reach its number: the record points into the image, and the
		// place it points at holds the number. The reflection library hands a type its number the
		// first time the type is used and keeps it in a variable of its own, so a pointer to that
		// variable is a pointer to the answer.
		Console.WriteLine("looking for a record pointer that leads to the number a name goes by:");
		var throughPointer = new Dictionary<int, Dictionary<string, HashSet<int>>>();
		var cell = new byte[4];
		foreach ((ulong at, uint hash) in HashProbe.Hits(process, wanted.Keys.ToHashSet()))
		{
			if (!process.TryRead(at, pointed, pointed.Length)) continue;
			for (int offset = 8; offset + 8 <= RecordPointersTo; offset += 8)
			{
				ulong value = BitConverter.ToUInt64(pointed, offset);
				if (value <= process.ModuleBase || value - process.ModuleBase > 0x10000000) continue;
				if (!process.TryRead(value, cell, cell.Length)) continue;
				int number = BitConverter.ToInt32(cell, 0);
				if (!seenIds.Contains(number)) continue;
				if (!throughPointer.TryGetValue(offset, out Dictionary<string, HashSet<int>> map))
					throughPointer[offset] = map = new Dictionary<string, HashSet<int>>(StringComparer.Ordinal);
				if (!map.TryGetValue(wanted[hash], out HashSet<int> numbers)) map[wanted[hash]] = numbers = [];
				numbers.Add(number);
			}
		}

		foreach ((int offset, Dictionary<string, HashSet<int>> map) in throughPointer.OrderByDescending(t => t.Value.Count).Take(6))
		{
			int single = map.Count(m => m.Value.Count == 1);
			int distinct = map.Where(m => m.Value.Count == 1).Select(m => m.Value.First()).Distinct().Count();
			Console.WriteLine($"    the pointer at +{offset}: {map.Count} names, {single} with one number, {distinct} of those different");
			if (single >= 8 && single == distinct)
			{
				foreach ((string name, HashSet<int> numbers) in map.Where(m => m.Value.Count == 1).OrderBy(m => m.Value.First()))
				{
					Console.WriteLine($"        {numbers.First(),3}  {name}");
				}
			}
		}

		Console.WriteLine("looking for the position that binds a name to the number it goes by:");
		foreach ((int relative, Dictionary<string, HashSet<int>> map) in candidates.OrderByDescending(c => c.Value.Count).Take(6))
		{
			int single = map.Count(m => m.Value.Count == 1);
			int distinct = map.Where(m => m.Value.Count == 1).Select(m => m.Value.First()).Distinct().Count();
			Console.WriteLine($"    {relative,+4} from the hash: {map.Count} names, {single} with one number, {distinct} of those different");
			if (single >= 8 && single == distinct)
			{
				Console.WriteLine("      names and their numbers:");
				foreach ((string name, HashSet<int> values) in map.Where(m => m.Value.Count == 1).OrderBy(m => m.Value.First()))
				{
					Console.WriteLine($"        {values.First(),3}  {name}");
				}
			}
		}

		var named = new List<string>();
		var unnamed = new List<string>();
		foreach ((long rva, ClassUse use) in byClass.OrderByDescending(c => c.Value.Instances))
		{
			string name = records.GetValueOrDefault(rva);
			string ids = string.Join(", ", use.Ids.OrderBy(i => i));
			string row = $"{{ \"classRva\": {rva}, \"instances\": {use.Instances}, \"ids\": [{ids}], "
					+ $"\"example\": \"{use.Example}\", \"name\": {(name is null ? "null" : $"\"{name}\"")} }}";
			(name is null ? unnamed : named).Add(row);
			Console.WriteLine($"  {use.Instances,6} instances  ids [{ids}]  {name ?? $"UNNAMED at image+0x{rva:X}"}  (e.g. {use.Example})");
		}

		Console.WriteLine();
		Console.WriteLine($"classes named by the registry: {named.Count:N0}, still unnamed: {unnamed.Count:N0}");

		var text = new StringBuilder("{\n");
		text.Append($"\t\"componentInstances\": {instances},\n");
		text.Append($"\t\"distinctClasses\": {byClass.Count},\n");
		text.Append($"\t\"named\": [{string.Join(", ", named)}],\n");
		text.Append($"\t\"unnamed\": [{string.Join(", ", unnamed)}]\n}}\n");
		if (Path.GetDirectoryName(output) is { Length: > 0 } directory) Directory.CreateDirectory(directory);
		File.WriteAllText(output, text.ToString(), new UTF8Encoding(false));
		Console.WriteLine($"written {output}");
		return unnamed.Count == 0 ? 0 : 1;
	}

	/// <summary>
	///     What each component is, said by which blocks carry it.
	///     <para>
	///         Most components hold nothing. A marker's whole content is its presence, so no reading
	///         of its bytes can name it and no comparison against a block's known values can either.
	///         What separates one marker from another is the set of blocks that have it, and that
	///         set is as good as a name: the component on exactly one block, and that block the
	///         crafting table, is the crafting table component.
	///     </para>
	///     <para>
	///         The number a component goes by is not written out. It is handed out at startup and
	///         differs between runs, so it identifies nothing outside this process and is used here
	///         only to group the instances while they are being looked at.
	///     </para>
	/// </summary>
	private static int? Membership(BedrockProcess process, string output)
	{
		var word = new byte[8];
		var blocks = BlockRegistry.Read(process);
		var carriers = new Dictionary<int, List<string>>();
		foreach (BlockProperties block in blocks)
		{
			foreach ((int id, ulong _) in ComponentReader.Instances(process, block.Address, word))
			{
				if (!carriers.TryGetValue(id, out List<string> names)) carriers[id] = names = [];
				names.Add(block.Name);
			}
		}

		Console.WriteLine($"blocks: {blocks.Count:N0}, components in use this run: {carriers.Count}");
		Console.WriteLine();

		// One block's components in full: what number each goes by, what class it belongs to, how
		// big that class is, and every byte of it. This is what decides whether an object can say
		// what it is on its own.
		BlockProperties sample = blocks.FirstOrDefault(b => b.Name == "minecraft:acacia_fence");
		if (sample is not null)
		{
			Console.WriteLine($"{sample.Name}, component by component:");
			var payload = new byte[MaximumDump];
			foreach ((int id, ulong at) in ComponentReader.Instances(process, sample.Address, word))
			{
				ulong vtable = process.ReadUInt64(at, word);
				int size = ItemRegistry.ClassSize(process, vtable);
				int have = process.ReadClipped(at, payload, Math.Min(size > 0 ? size : 16, payload.Length));
				string bytes = have > 8 ? Convert.ToHexString(payload, 8, have - 8) : "(nothing past the method table)";
				Console.WriteLine($"  id {id,3}  class image+0x{(vtable > process.ModuleBase ? vtable - process.ModuleBase : 0):X}"
								+ $"  {(size > 0 ? size + " bytes" : "size unstated")}  {bytes}");
			}
			Console.WriteLine();
		}

		var rows = new List<string>();
		foreach ((int id, List<string> names) in carriers.OrderBy(c => c.Value.Count))
		{
			string shared = Shared(names);
			Console.WriteLine($"  {names.Count,5} blocks  {(shared is null ? "" : $"[{shared}] ")}"
							+ $"{string.Join(", ", names.Take(6))}{(names.Count > 6 ? ", ..." : "")}");
			rows.Add($"{{ \"blocks\": {names.Count}, \"shared\": {(shared is null ? "null" : $"\"{shared}\"")}, "
					+ $"\"carriers\": [{string.Join(", ", names.Take(2048).Select(n => $"\"{n}\""))}] }}");
		}

		var text = new StringBuilder("{\n");
		text.Append($"\t\"blocks\": {blocks.Count},\n");
		text.Append($"\t\"componentsInUse\": {carriers.Count},\n");
		text.Append($"\t\"byMembership\": [{string.Join(",\n\t\t", rows)}]\n}}\n");
		if (Path.GetDirectoryName(output) is { Length: > 0 } directory) Directory.CreateDirectory(directory);
		File.WriteAllText(output, text.ToString(), new UTF8Encoding(false));
		Console.WriteLine();
		Console.WriteLine($"written {output}");
		return 0;
	}

	/// <summary>
	///     A word every carrier's name contains, when there is one. A set of blocks that all say
	///     "fence" is a fence component whatever it is called, and that is a good deal more than a
	///     number nobody can reproduce.
	/// </summary>
	private static string Shared(List<string> names)
	{
		if (names.Count < 2) return null;
		string[] first = names[0].Replace("minecraft:", "").Split('_');
		var common = new List<string>();
		foreach (string part in first)
		{
			if (part.Length >= 3 && names.All(n => n.Contains(part, StringComparison.Ordinal))) common.Add(part);
		}
		return common.Count > 0 ? string.Join(" ", common) : null;
	}

	private sealed class ClassUse
	{
		public int Instances;
		public SortedSet<int> Ids { get; } = [];
		public string Example;
	}
}
