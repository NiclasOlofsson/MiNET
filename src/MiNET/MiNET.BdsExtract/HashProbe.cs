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
using System.Text.Json;

/// <summary>
///     Looks for the reflection system's own identifiers in memory, using the names the server
///     itself publishes.
///     <para>
///         The component registry identifies a type and each of its fields by a number rather than
///         by text: the reflection library hashes the name and keeps the hash. The names are not a
///         secret though, the server writes them out as JSON schemas when asked to document itself,
///         so hashing those gives the exact numbers to look for.
///     </para>
///     <para>
///         One number turning up somewhere proves nothing: a 32 bit value appears in four gigabytes
///         of memory by chance. Several DIFFERENT known names turning up within a few hundred bytes
///         of each other does not happen by chance, so clusters are what this reports and single
///         hits are only counted.
///     </para>
/// </summary>
public static class HashProbe
{
	/// <summary>How near two hits must be to be called the same cluster.</summary>
	private const ulong ClusterReach = 1024;

	/// <summary>How many different names a cluster must carry before it is worth reporting.</summary>
	private const int ClusterFloor = 3;

	public static int Run(string[] args)
	{
		string pathFilter = null;
		string schemas = null;
		string output = Path.Combine(Program.DefaultOutputDirectory(), "hash-probe.json");
		for (int i = 0; i < args.Length; i++)
		{
			if (args[i] == "--server" && i + 1 < args.Length) pathFilter = args[++i];
			else if (args[i] == "--schemas" && i + 1 < args.Length) schemas = args[++i];
			else if (args[i] == "--out" && i + 1 < args.Length) output = args[++i];
		}

		if (schemas is null || !Directory.Exists(schemas))
		{
			Console.Error.WriteLine("usage: MiNET.BdsExtract --hashes --schemas <docs/json_schemas/server/item> [--server <path fragment>]");
			return 2;
		}

		Dictionary<string, string> names = ReadNames(schemas);
		Console.WriteLine($"names the schemas publish: {names.Count:N0}");

		var byHash = new Dictionary<uint, string>();
		foreach ((string name, string where) in names)
		{
			uint hash = Fnv1a32(name);
			// A collision would make one hit stand for two names, so it is said rather than hidden.
			if (byHash.TryGetValue(hash, out string other) && other != name)
			{
				Console.WriteLine($"  two names hash alike: {name} and {other}");
				continue;
			}
			byHash[hash] = name;
		}
		Console.WriteLine($"distinct hashes to look for: {byHash.Count:N0}");

		using var process = BedrockProcess.Attach(pathFilter);
		Console.WriteLine($"reading pid {process.Id}");

		List<(ulong At, uint Hash)> hits = Scan(process, byHash.Keys.ToHashSet());
		Console.WriteLine($"hits: {hits.Count:N0} over {hits.Select(h => h.Hash).Distinct().Count():N0} of the names");

		// Clusters, which is the part that means anything.
		hits.Sort((a, b) => a.At.CompareTo(b.At));
		var clusters = new List<List<(ulong At, uint Hash)>>();
		var current = new List<(ulong At, uint Hash)>();
		foreach ((ulong at, uint hash) in hits)
		{
			if (current.Count > 0 && at - current[^1].At > ClusterReach)
			{
				clusters.Add(current);
				current = [];
			}
			current.Add((at, hash));
		}
		if (current.Count > 0) clusters.Add(current);

		List<List<(ulong At, uint Hash)>> worth = clusters
			.Where(c => c.Select(h => h.Hash).Distinct().Count() >= ClusterFloor)
			.OrderByDescending(c => c.Select(h => h.Hash).Distinct().Count())
			.ToList();

		Console.WriteLine($"clusters carrying {ClusterFloor} or more different names: {worth.Count:N0}");
		var text = new StringBuilder("{\n");
		text.Append($"\t\"namesPublished\": {names.Count},\n");
		text.Append($"\t\"hashesSought\": {byHash.Count},\n");
		text.Append($"\t\"hits\": {hits.Count},\n");
		text.Append($"\t\"namesSeen\": {hits.Select(h => h.Hash).Distinct().Count()},\n");
		text.Append("\t\"clusters\": [\n");

		for (int i = 0; i < worth.Count; i++)
		{
			List<(ulong At, uint Hash)> cluster = worth[i];
			List<string> carried = cluster.Select(h => byHash[h.Hash]).Distinct().ToList();
			ulong span = cluster[^1].At - cluster[0].At;
			// How far apart consecutive hits sit. A table of entries keyed by these numbers has one
			// distance repeating; a scattering has none, and that is the difference between a
			// structure and a coincidence of neighbours.
			var steps = new Dictionary<ulong, int>();
			for (int k = 1; k < cluster.Count; k++) steps[cluster[k].At - cluster[k - 1].At] = steps.GetValueOrDefault(cluster[k].At - cluster[k - 1].At) + 1;
			KeyValuePair<ulong, int> common = steps.Count == 0 ? default : steps.OrderByDescending(s => s.Value).First();

			if (i < 8)
			{
				Console.WriteLine($"  0x{cluster[0].At:X} .. +{span}: {cluster.Count} hits, {carried.Count} names, "
								+ $"commonest step {common.Key} on {common.Value} of {cluster.Count - 1}");
				Console.WriteLine($"      {string.Join(", ", carried.Take(8))}{(carried.Count > 8 ? ", ..." : "")}");
			}
			text.Append($"\t\t{{ \"at\": \"0x{cluster[0].At:X}\", \"span\": {span}, \"names\": ["
					+ string.Join(", ", carried.Select(n => $"\"{n}\"")) + "] }");
			text.Append(i == worth.Count - 1 ? "\n" : ",\n");
		}

		text.Append("\t]\n}\n");

		// One record, word by word. A table of a hundred bytes an entry holds more than the number
		// it is keyed by, and what the rest of it is decides whether the field layouts can be read
		// from here or only their names.
		if (worth.Count > 0)
		{
			Describe(process, worth[0], byHash);
			List<(ulong At, uint Hash)> best = worth.OrderByDescending(c => c.Count).First();
			Owners(process, best[0].At, best.Count > 1 ? (int) (best[1].At - best[0].At) : 8);
		}
		if (Path.GetDirectoryName(output) is { Length: > 0 } directory) Directory.CreateDirectory(directory);
		File.WriteAllText(output, text.ToString(), new UTF8Encoding(false));
		Console.WriteLine($"written {output}");
		return worth.Count > 0 ? 0 : 1;
	}

	/// <summary>
	///     Every name the item schemas publish: the component's own name, which is the schema's
	///     title, and the name of each field it declares.
	/// </summary>
	private static Dictionary<string, string> ReadNames(string directory)
	{
		var names = new Dictionary<string, string>(StringComparer.Ordinal);
		foreach (string file in Directory.GetFiles(directory, "*.json", SearchOption.AllDirectories))
		{
			JsonDocument document;
			try
			{
				document = JsonDocument.Parse(File.ReadAllText(file));
			}
			catch (JsonException)
			{
				Console.WriteLine($"  unreadable: {Path.GetFileName(file)}");
				continue;
			}

			using (document)
			{
				Collect(document.RootElement, Path.GetFileName(file), names);
			}
		}
		return names;
	}

	/// <summary>
	///     Titles and property names, wherever they sit. The schemas nest, and a field's own fields
	///     are named the same way further in, so the whole document is walked rather than its first
	///     level only.
	/// </summary>
	private static void Collect(JsonElement element, string file, Dictionary<string, string> names)
	{
		if (element.ValueKind == JsonValueKind.Object)
		{
			foreach (JsonProperty property in element.EnumerateObject())
			{
				if (property.Name == "title" && property.Value.ValueKind == JsonValueKind.String)
				{
					string title = property.Value.GetString();
					if (!string.IsNullOrEmpty(title)) names[title] = file;
				}

				if (property.Name == "properties" && property.Value.ValueKind == JsonValueKind.Object)
				{
					foreach (JsonProperty field in property.Value.EnumerateObject()) names[field.Name] = file;
				}

				Collect(property.Value, file, names);
			}
		}
		else if (element.ValueKind == JsonValueKind.Array)
		{
			foreach (JsonElement item in element.EnumerateArray()) Collect(item, file, names);
		}
	}

	/// <summary>
	///     What one entry of a table holds, said in terms of what each word reads as rather than
	///     what it is hoped to be. The hash the entry is keyed by is known, so its place inside the
	///     entry is known too, and everything else is described from its own contents.
	/// </summary>
	private static void Describe(BedrockProcess process, List<(ulong At, uint Hash)> cluster, Dictionary<uint, string> byHash)
	{
		if (cluster.Count < 2) return;
		ulong stride = cluster[1].At - cluster[0].At;
		var body = new byte[(int) stride];
		var scratch = new byte[256];

		Console.WriteLine();
		Console.WriteLine($"one entry of the table at 0x{cluster[0].At:X}, {stride} bytes each:");
		for (int entry = 0; entry < 2 && entry < cluster.Count; entry++)
		{
			ulong start = cluster[entry].At;
			Console.WriteLine($"  entry keyed by {byHash[cluster[entry].Hash]} (the hash sits at the start of what is shown)");
			if (process.ReadClipped(start, body, body.Length) < body.Length) continue;

			for (int offset = 0; offset + 8 <= body.Length; offset += 8)
			{
				ulong value = BitConverter.ToUInt64(body, offset);
				uint low = BitConverter.ToUInt32(body, offset);
				string what;
				if (byHash.TryGetValue(low, out string named)) what = $"the hash of {named}";
				else if (value > process.ModuleBase && value - process.ModuleBase < 0x10000000) what = $"into the image at +0x{value - process.ModuleBase:X}";
				else if (value > 0x10000 && process.IsMapped(value)) what = "a pointer into the heap";
				else if (value < 0x10000) what = $"the number {value}";
				else what = "not resolved";

				string text = ItemRegistry.ReadStdString(process, body, offset, scratch);
				if (text is { Length: > 0 }) what += $", or the text \"{text}\"";

				// A pointer may lead to the name rather than hold it. One step is followed and
				// anything readable at the far end is reported, because a name reachable from the
				// record is what would let an unknown hash be named without a word list.
				if (value > 0x10000 && process.IsMapped(value))
				{
					var beyond = new byte[64];
					if (process.ReadClipped(value, beyond, beyond.Length) >= 16)
					{
						string inline = Readable(beyond, 0);
						string held = ItemRegistry.ReadStdString(process, beyond, 0, scratch);
						if (inline is not null) what += $" -> the text \"{inline}\"";
						else if (held is { Length: > 0 }) what += $" -> holding the text \"{held}\"";
					}
				}

				Console.WriteLine($"    +{offset,3}: {what}");
			}
		}
	}

	/// <summary>Plain text lying at an address, when what is there is text at all.</summary>
	private static string Readable(byte[] buffer, int at)
	{
		int length = 0;
		while (at + length < buffer.Length && buffer[at + length] is >= 0x20 and < 0x7F) length++;
		return length >= 4 && at + length < buffer.Length && buffer[at + length] == 0
			? Encoding.ASCII.GetString(buffer, at, length)
			: null;
	}

	/// <summary>
	///     Who holds this table, and who holds them. A table is reached from something, and on a
	///     build that ships symbols the far end of that chain has a name, which is how a structure
	///     found by shape alone gets told what it is called.
	/// </summary>
	private static void Owners(BedrockProcess process, ulong table, int stride)
	{
		Console.WriteLine();
		Console.WriteLine($"who points at the table around 0x{table:X}:");

		// Where a record starts is not known: only that the hashes inside them are one stride
		// apart. So every address within a stride in front of the first hash is offered, and
		// whichever one the process points at is the array's beginning.
		var starts = new List<ulong>();
		for (ulong behind = 0; behind < (ulong) Math.Max(stride, 8); behind += 8)
		{
			if (table > behind) starts.Add(table - behind);
		}

		Dictionary<ulong, List<ulong>> found = RegistryDiscovery.Locate(process, starts, 64);
		var holders = new List<ulong>();
		foreach ((ulong start, List<ulong> places) in found)
		{
			Console.WriteLine($"  its first record begins {table - start} bytes in front of the hash, pointed at from {places.Count}");
			holders.AddRange(places);
		}

		if (holders.Count == 0)
		{
			Console.WriteLine("  nothing points at any address within a record of it");
			return;
		}

		var word = new byte[8];
		var second = new List<ulong>();
		foreach (ulong holder in holders)
		{
			bool inImage = holder > process.ModuleBase && holder - process.ModuleBase < 0x10000000;
			Console.WriteLine($"  0x{holder:X}{(inImage ? $" = image+0x{holder - process.ModuleBase:X}" : " (heap)")}"
							+ $", the word after it is 0x{process.ReadUInt64(holder + 8, word):X}");
			if (!inImage)
			{
				// The holder is a vector header inside some object, so the object itself is what a
				// static would name. Every address the header could sit inside is offered.
				for (ulong behind = 0; behind <= 256; behind += 8)
				{
					if (holder > behind) second.Add(holder - behind);
				}
			}
		}

		if (second.Count == 0) return;
		Console.WriteLine("  and who points at the objects those sit in:");
		Dictionary<ulong, List<ulong>> above = RegistryDiscovery.Locate(process, second, 32);
		int shown = 0;
		foreach ((ulong start, List<ulong> places) in above)
		{
			foreach (ulong place in places.Where(p => p > process.ModuleBase && p - process.ModuleBase < 0x10000000))
			{
				Console.WriteLine($"    image+0x{place - process.ModuleBase:X} points at 0x{start:X}");
				if (++shown >= 16) return;
			}
		}
		if (shown == 0) Console.WriteLine("    nothing in the image points at any of them");
	}

	/// <summary>The names a schema tree publishes, by the number the reflection system keys them on.</summary>
	public static Dictionary<uint, string> NamesByHash(string schemas)
	{
		var byHash = new Dictionary<uint, string>();
		foreach ((string name, string _) in ReadNames(schemas))
		{
			uint hash = Fnv1a32(name);
			if (!byHash.TryGetValue(hash, out string other) || other == name) byHash[hash] = name;
		}
		return byHash;
	}

	/// <summary>Every place one of these numbers sits, for a caller that wants the places rather than a report.</summary>
	public static List<(ulong At, uint Hash)> Hits(BedrockProcess process, HashSet<uint> wanted) => Scan(process, wanted);

	/// <summary>Every place a sought hash sits, read four bytes at a time.</summary>
	private static List<(ulong At, uint Hash)> Scan(BedrockProcess process, HashSet<uint> wanted)
	{
		var hits = new List<(ulong, uint)>();
		var scan = new byte[8 * 1024 * 1024];
		foreach (BedrockProcess.Region region in process.Regions)
		{
			for (ulong at = region.Base; at < region.End;)
			{
				int length = (int) Math.Min((ulong) scan.Length, region.End - at);
				if (length >= 4 && process.TryRead(at, scan, length))
				{
					for (int k = 0; k + 4 <= length; k += 4)
					{
						uint value = BitConverter.ToUInt32(scan, k);
						if (wanted.Contains(value)) hits.Add((at + (ulong) k, value));
					}
				}
				at += (ulong) length;
			}
		}
		return hits;
	}

	/// <summary>
	///     FNV-1a over 32 bits, which is what the reflection library hashes a name with. Note the
	///     order: xor first, then multiply, the opposite of the FNV-1 that orders block names.
	/// </summary>
	public static uint Fnv1a32(string value)
	{
		uint hash = 2166136261;
		foreach (byte b in Encoding.UTF8.GetBytes(value))
		{
			hash = unchecked((hash ^ b) * 16777619);
		}
		return hash;
	}
}
