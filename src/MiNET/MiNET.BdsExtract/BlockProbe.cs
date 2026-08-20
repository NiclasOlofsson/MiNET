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
// WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License for the
// specific language governing rights and limitations under the License.
//
// The Original Code is MiNET.
//
// The Original Developer is the Initial Developer.  The Initial Developer of
// the Original Code is Niclas Olofsson.
//
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2020 Niclas Olofsson.
// All Rights Reserved.

#endregion

using System.Diagnostics;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MiNET.BdsExtract;

/// <summary>
///     Extracts block data by asking a running Bedrock Dedicated Server, through a behaviour pack
///     this deploys into it and drives.
///     <para>
///         This is the other half of the tool. The memory side reads the palette and the compiled
///         properties out of the process: it knows the order but not what a state is. This side knows
///         exactly what each state is, having asked the game, but nothing about the order. They meet
///         on the network id, a hash of the name and states computed the same way on both sides and
///         by the client, so neither has to trust the other's ordering.
///     </para>
///     <para>
///         Every value here comes from the server itself: a behaviour pack runs inside it and reports
///         what the game says about each block, and the documentation BDS writes about itself says
///         which states travel on the wire. No third party dump is read, which is the point of the
///         exercise: the data we shipped for years came from an undocumented extraction nobody could
///         reproduce, and two of its fields turned out to be wrong.
///     </para>
///     <para>
///         Run it as <c>MiNET.BdsExtract --probe &lt;bds directory&gt; [output directory]</c>. It deploys
///         the pack, starts the server, reads its output until the sweep reports itself finished, then
///         stops it and writes the files. The sweep places a block per two ticks, so a full run is
///         about an hour for 1,477 blocks and their permutations.
///     </para>
/// </summary>
public static class BlockProbe
{
	/// <summary>The pack's own folder name under development_behavior_packs.</summary>
	private const string PackName = "MiNETBlockProbe";

	/// <summary>
	///     The sweep prints one line per block and one per permutation, each tagged so the three
	///     passes can be told apart in a log that also carries everything else the server says.
	/// </summary>
	private const string BlockTag = "[BX] ";


	/// <summary>A run that has gone this long without a single line has stopped making progress.</summary>
	private static readonly TimeSpan Silence = TimeSpan.FromMinutes(3);

	/// <summary>
	///     How many times the server may be restarted to step over a block that crashes it. Each
	///     restart costs one permutation, so this bounds how much the sweep is allowed to lose.
	/// </summary>
	private const int MaxRestarts = 100;

	public static int Run(string[] args)
	{
		// A run is an hour long and its only feedback is these lines, so they have to arrive as they
		// happen rather than when a redirected buffer decides to flush.
		Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) {AutoFlush = true});

		if (args.Length < 1)
		{
			Console.Error.WriteLine("usage: MiNET.BdsExtract --probe <bds directory> [output directory]");
			return 1;
		}

		string bdsDir = Path.GetFullPath(args[0]);
		string outDir = args.Length > 1 ? Path.GetFullPath(args[1]) : Directory.GetCurrentDirectory();

		string exe = Path.Combine(bdsDir, OperatingSystem.IsWindows() ? "bedrock_server.exe" : "bedrock_server");
		if (!File.Exists(exe))
		{
			Console.Error.WriteLine($"no bedrock server at {exe}");
			return 1;
		}

		// Written by the server itself when started once with {"generate_documentation": true} in
		// test_config.json. It is the only thing that knows which states reach the wire: the game
		// still carries pre-flattening states internally, and enumerating those produces four times
		// too many block states.
		string docgen = Path.Combine(bdsDir, "docs", "vanilladata_modules", "mojang-blocks.json");
		if (!File.Exists(docgen))
		{
			Console.Error.WriteLine($"no block documentation at {docgen}");
			Console.Error.WriteLine("Start BDS once with test_config.json {\"generate_documentation\": true} to write it.");
			return 1;
		}

		if (!Configure(bdsDir)) return 1;
		DeployPack(bdsDir, docgen);

		// One row per block state, exactly as the sweep reported it. Nothing is merged, collapsed or
		// inferred here: the file says what each state measured, and any summarising is somebody
		// else's decision to make later.
		List<JObject> states = new();

		// Some blocks kill the server on placement, small_amethyst_bud reproducibly. The pack
		// checkpoints before every placement and resumes past the casualty, so the supervisor's job
		// is simply to start it again. Without this the sweep can never reach the end.
		int attempt = 0;
		bool complete = false;
		var lost = new List<string>();
		var resume = new Progress();
		while (attempt++ <= MaxRestarts)
		{
			if (attempt > 1)
			{
				// Only the row it died on is stepped over. Crashes here are not a property of the
				// block: they arrive every twenty or so placements once the server has been worn
				// down, whatever is being placed, so discarding a whole block would throw away good
				// readings to no purpose. The pack moves its rig to keep out of that state.
				lost.Add($"#{resume.Index}");
				resume.Index++;
				Console.WriteLine($"server died on #{resume.Index - 1}, resuming past it "
				                  + $"(restart {attempt - 1} of {MaxRestarts})");
			}
			complete = Sweep(exe, bdsDir, states, resume);
			if (complete) break;
		}

		if (lost.Count > 0)
		{
			// Never silently: a block the sweep could not measure is missing data, not a tidy result.
			Console.WriteLine($"{lost.Count} readings lost to server crashes: {string.Join(", ", lost)}");
		}

		if (!complete) Console.Error.WriteLine($"gave up after {MaxRestarts} restarts; the data below is partial");
		if (states.Count == 0) return 1;

		Console.WriteLine($"swept {states.Count} block states");

		foreach (JObject row in states) Dampening(row);

		// The hash is the join key. It is computed the same way the client computes it, from the
		// name and states alone, so a row here and a row from a memory extraction of the same server
		// meet on it exactly. That is also where the palette index comes from: it is not in this
		// data and cannot be, since nothing the runtime publishes carries it.
		foreach (JObject row in states)
		{
			row["networkId"] = NetworkId((string) row["id"], row["states"] as JObject ?? new JObject());
		}

		Directory.CreateDirectory(outDir);
		string rowsPath = Path.Combine(outDir, "blockstates-runtime.json");
		File.WriteAllText(rowsPath, JsonConvert.SerializeObject(states, Formatting.Indented), new UTF8Encoding(true));
		Console.WriteLine($"blockstates-runtime.json: {states.Count} rows");

		// The palette is enumerated rather than measured, and the difference matters: connection
		// states are not placed, because forcing a wall to claim it joins something when it is
		// sealed in stone kills the server. They still exist and still travel, so they are counted
		// here from the values each block accepts, and every one gets the id the client computes.
		int written = WriteBlockStates(Path.Combine(outDir, "blockstates.json"), states, docgen);
		Console.WriteLine($"blockstates.json: {written} block states");
		return 0;
	}

	/// <summary>
	///     What the server has to be set to before the sweep will work, checked rather than assumed.
	///     A run is an hour long, so a setting that silently stops the pack from doing its job is an
	///     hour thrown away, and the failure looks like a bug in the probe rather than a configuration mistake.
	/// </summary>
	private static readonly (string Key, string Value, string Why)[] Required =
	{
		// The sweep claims a ticking area with a command, and a server that refuses commands leaves
		// every block unloaded, so nothing can be placed or read.
		("allow-cheats", "true", "the sweep adds a ticking area with a command"),
		// Script output IS the result. Without it on the console there is nothing to parse.
		("content-log-file-enabled", "true", "the sweep reports through the content log"),
		("content-log-console-output-enabled", "true", "the results are read from the console"),
		("content-log-level", "verbose", "so the pack's own lines are not filtered out"),
		// Two runs are only comparable if they read the same world. The sweep seals its own pocket,
		// so terrain does not reach the readings, but a fixed seed means a difference between runs
		// is a difference in the data rather than in where the probe happened to land.
		("level-seed", "1", "so every run reads the same world")
	};

	/// <summary>
	///     Brings server.properties to what the sweep needs, reporting every change. Nothing is
	///     changed silently: this is somebody's server directory.
	/// </summary>
	private static bool Configure(string bdsDir)
	{
		string path = Path.Combine(bdsDir, "server.properties");
		if (!File.Exists(path))
		{
			Console.Error.WriteLine($"no server.properties at {path}");
			return false;
		}

		List<string> lines = File.ReadAllLines(path).ToList();
		var changed = new List<string>();

		foreach ((string key, string value, string why) in Required)
		{
			int at = lines.FindIndex(l => !l.StartsWith('#') && l.StartsWith(key + "=", StringComparison.Ordinal));
			string current = at >= 0 ? lines[at][(key.Length + 1)..].Trim() : null;
			if (string.Equals(current, value, StringComparison.OrdinalIgnoreCase)) continue;

			if (at >= 0) lines[at] = $"{key}={value}";
			else lines.Add($"{key}={value}");
			changed.Add($"  {key}: {current ?? "unset"} -> {value}   ({why})");
		}

		if (changed.Count == 0)
		{
			Console.WriteLine("server.properties already set for the sweep");
			return true;
		}

		Console.WriteLine($"adjusting server.properties in {bdsDir}:");
		foreach (string line in changed) Console.WriteLine(line);
		File.WriteAllLines(path, lines);
		return true;
	}

	/// <summary>
	///     Copies the pack into the server and makes the world load it. A development pack is only
	///     read if the world names it, so the world's behaviour pack list gets the entry as well.
	/// </summary>
	private static void DeployPack(string bdsDir, string docgenPath)
	{
		string source = Path.Combine(AppContext.BaseDirectory, "ProbePack");
		if (!Directory.Exists(source)) throw new DirectoryNotFoundException($"probe pack missing from the build output: {source}");

		string target = Path.Combine(bdsDir, "development_behavior_packs", PackName);
		if (Directory.Exists(target)) Directory.Delete(target, true);
		foreach (string file in Directory.GetFiles(source, "*", SearchOption.AllDirectories))
		{
			string relative = Path.GetRelativePath(source, file);
			string destination = Path.Combine(target, relative);
			Directory.CreateDirectory(Path.GetDirectoryName(destination)!);
			File.Copy(file, destination, true);
		}
		// The pack cannot know which states travel on the wire, and the difference is not cosmetic:
		// asking for a legacy state alongside the one that replaced it (facing_direction with
		// minecraft:block_face, sandstone_type on a cut slab) is a combination the engine does not
		// refuse, it dies. So the wire state lists are generated into the pack from the server's own
		// documentation before it runs.
		WriteWireStates(Path.Combine(target, "scripts", "wire-states.js"), docgenPath);
		Console.WriteLine($"deployed probe pack to {target}");

		var manifest = JObject.Parse(File.ReadAllText(Path.Combine(source, "manifest.json")));
		string uuid = (string) manifest["header"]!["uuid"];
		int[] version = manifest["header"]!["version"]!.ToObject<int[]>();

		string worldName = ReadServerProperty(bdsDir, "level-name") ?? "Bedrock level";
		string listPath = Path.Combine(bdsDir, "worlds", worldName, "world_behavior_packs.json");
		Directory.CreateDirectory(Path.GetDirectoryName(listPath)!);

		var packs = File.Exists(listPath) ? JArray.Parse(File.ReadAllText(listPath)) : new JArray();
		if (!packs.Any(p => string.Equals((string) p["pack_id"], uuid, StringComparison.OrdinalIgnoreCase)))
		{
			packs.Add(new JObject {["pack_id"] = uuid, ["version"] = new JArray(version)});
			File.WriteAllText(listPath, packs.ToString(Formatting.Indented));
			Console.WriteLine($"enabled the pack in {worldName}");
		}
	}

	/// <summary>Writes the documented state list per block into the pack as a module it can import.</summary>
	private static void WriteWireStates(string path, string docgenPath)
	{
		var module = JObject.Parse(File.ReadAllText(docgenPath));
		var states = new JObject();
		foreach (JToken item in module["data_items"]!)
		{
			states[(string) item["name"]] = new JArray(item["properties"]!.Select(p => (string) p["name"]));
		}

		var text = new StringBuilder();
		text.AppendLine("// Generated by MiNET.BlockGen from the server's own block documentation.");
		text.AppendLine("// Which states travel on the wire, per block. The runtime also reports states that");
		text.AppendLine("// predate the flattening, and combining those with the ones that replaced them kills");
		text.AppendLine("// the server outright rather than raising an error.");
		text.Append("export const WIRE_STATES = ");
		text.Append(states.ToString(Formatting.None));
		text.AppendLine(";");
		File.WriteAllText(path, text.ToString());
		Console.WriteLine($"  wrote wire states for {states.Count} blocks into the pack");
	}

	private static string ReadServerProperty(string bdsDir, string key)
	{
		string path = Path.Combine(bdsDir, "server.properties");
		if (!File.Exists(path)) return null;
		foreach (string line in File.ReadLines(path))
		{
			if (line.StartsWith('#')) continue;
			int split = line.IndexOf('=');
			if (split > 0 && line[..split].Trim() == key) return line[(split + 1)..].Trim();
		}
		return null;
	}

	/// <summary>
	///     Runs the server and reads its output until the sweep says it is done. The server is stopped
	///     the way the console does it, so the world is saved rather than abandoned.
	/// </summary>
	/// <summary>How far the sweep got, so a restart can be told where to pick up.</summary>
	private sealed class Progress
	{
		public int Index;
	}

	private static bool Sweep(string exe, string bdsDir, List<JObject> states, Progress resume)
	{
		var start = new ProcessStartInfo(exe)
		{
			WorkingDirectory = bdsDir,
			RedirectStandardOutput = true,
			RedirectStandardInput = true,
			UseShellExecute = false
		};

		using Process server = Process.Start(start) ?? throw new InvalidOperationException("could not start the server");
		DateTime lastLine = DateTime.UtcNow;
		bool finished = false;
		bool told = false;

		try
		{
			while (!server.StandardOutput.EndOfStream)
			{
				string line = server.StandardOutput.ReadLine();
				if (line == null) break;

				if (DateTime.UtcNow - lastLine > Silence)
				{
					Console.Error.WriteLine($"no output for {Silence.TotalMinutes:0} minutes, giving up");
					break;
				}

				if (!told && line.Contains("Server started", StringComparison.OrdinalIgnoreCase))
				{
					// The pack waits to be told, so a fresh run and a resumed one are never confused.
					server.StandardInput.WriteLine($"scriptevent minet:begin {resume.Index}");
					told = true;
					continue;
				}

				if (Collect(line, BlockTag, out JObject row))
				{
					states.Add(row);
					Track(resume, row);
					lastLine = DateTime.UtcNow;
					if (states.Count % 250 == 0) Console.WriteLine($"  {states.Count} block states");
					continue;
				}

				// The passes announce themselves, and the last END is the signal to stop.
				int marker = line.IndexOf("[B", StringComparison.Ordinal);
				if (marker < 0) continue;
				string tail = line[marker..];
				// Anything the pack says that is not a data row, said out loud. Filtering these to a
				// few keywords is how a run that never loaded its world looked identical to one that
				// was working, twice.
				{
					Console.WriteLine($"  {tail}");
					lastLine = DateTime.UtcNow;
					if (tail.StartsWith(BlockTag + "END", StringComparison.Ordinal)) { finished = true; break; }
				}
			}
		}
		finally
		{
			try
			{
				server.StandardInput.WriteLine("stop");
				if (!server.WaitForExit(30_000)) server.Kill(true);
			}
			catch (Exception e)
			{
				Console.Error.WriteLine($"could not stop the server cleanly: {e.Message}");
			}
		}

		return finished;
	}

	private static bool Collect(string line, string tag, out JObject row)
	{
		row = null;
		int at = line.IndexOf(tag + "{", StringComparison.Ordinal);
		if (at < 0) return false;
		try
		{
			row = JObject.Parse(line[(at + tag.Length)..].Trim());
			return true;
		}
		catch (JsonException)
		{
			return false;
		}
	}

	/// <summary>
	///     Follows the sweep's own numbering. A row that has been printed is a row that survived, so
	///     the next attempt starts after the highest one seen.
	/// </summary>
	private static void Track(Progress resume, JObject row)
	{
		if (row["i"] is null) return;
		int index = (int) row["i"];
		if (index >= resume.Index) resume.Index = index;
	}

	private static void Add(Dictionary<string, List<JObject>> into, JObject row)
	{
		string id = (string) row["id"];
		if (!into.TryGetValue(id, out List<JObject> rows)) into[id] = rows = new List<JObject>();
		rows.Add(row);
	}

	/// <summary>
	///     Turns the corridor reading into a dampening value, and says so when it cannot.
	///     <para>
	///         Calibrated once against a known table, and the mapping is exact:
	///         <c>dampening = 14 - reading</c>. Light loses a level crossing any block at all, so an
	///         empty corridor reads 13, and a block that dampens by three reads 11. Every block the
	///         table calls 15 read nothing at all, 577 of them, with no exceptions.
	///     </para>
	///     <para>
	///         Three cases cannot be read and are not guessed. A block that emits light lights the far
	///         cell itself: every one of those reads exactly its own emission minus one, crying
	///         obsidian at 9 for its 10, a lit furnace at 12 for its 13. A block that did not stay
	///         where it was put, which is every liquid and anything needing support, was not there to
	///         be measured. And zero and one are the same block as far as the game is concerned, since
	///         propagation costs <c>max(1, dampening)</c> per block, so no arrangement of lamps can
	///         separate them: leaves, powder snow and water sit in that gap, and the exact number for
	///         them comes from the memory side, which reads the stored value rather than its effect.
	///     </para>
	/// </summary>
	private static void Dampening(JObject row)
	{
		const int EmptyCorridor = 13;

		if (row["dampeningRaw"] is not JValue value || value.Type != JTokenType.Integer) return;
		int raw = (int) value;

		if (row["light"] is JValue light && light.Type == JTokenType.Integer && (int) light > 0)
		{
			Unmeasurable(row, "the block emits light, which reaches the far cell itself");
			return;
		}

		if (row["stayed"] is JValue stayed && stayed.Type == JTokenType.Boolean && !(bool) stayed)
		{
			Unmeasurable(row, "the block did not stay where it was put, so this reading is of what replaced it");
			return;
		}

		if (raw > EmptyCorridor)
		{
			Unmeasurable(row, "brighter than an empty corridor, so something else lit the far cell");
			return;
		}

		if (raw == EmptyCorridor)
		{
			// Not a limitation of the rig: the game cannot tell these apart either.
			row["lightDampening"] = 0;
			row["lightDampeningNote"] = "0 or 1, which behave identically; take the exact value from the memory side";
			return;
		}

		row["lightDampening"] = EmptyCorridor + 1 - raw;
	}

	private static void Unmeasurable(JObject row, string why)
	{
		row["lightDampening"] = null;
		row["lightDampeningNote"] = why;
	}

	/// <summary>
	///     Every block state that travels, from the states the documentation lists and the values the
	///     sweep found each block accepts. This is enumeration, not measurement: it covers states the
	///     sweep deliberately never places.
	/// </summary>
	private static int WriteBlockStates(string path, List<JObject> rows, string docgenPath)
	{
		var module = JObject.Parse(File.ReadAllText(docgenPath));
		var wire = new Dictionary<string, List<string>>(StringComparer.Ordinal);
		foreach (JToken item in module["data_items"]!)
		{
			wire[(string) item["name"]] = item["properties"]!.Select(p => (string) p["name"]).ToList();
		}

		// One row per block carries the accepted values; the rest repeat them.
		var valuesByBlock = new Dictionary<string, JObject>(StringComparer.Ordinal);
		foreach (JObject row in rows)
		{
			string name = (string) row["id"];
			if (row["values"] is JObject v && !valuesByBlock.ContainsKey(name)) valuesByBlock[name] = v;
		}

		var entries = new JArray();
		var skipped = new List<string>();
		foreach ((string name, JObject values) in valuesByBlock)
		{
			if (!wire.TryGetValue(name, out List<string> stateNames)) { skipped.Add(name); continue; }

			if (stateNames.Count == 0)
			{
				entries.Add(Entry(name, new JObject()));
				continue;
			}

			var axes = new List<JArray>();
			foreach (string state in stateNames)
			{
				if (values[state] is not JArray axis || axis.Count == 0) { axes = null; break; }
				axes.Add(axis);
			}
			if (axes == null) { skipped.Add(name); continue; }

			foreach (JObject combination in Product(stateNames, axes)) entries.Add(Entry(name, combination));
		}

		File.WriteAllText(path, entries.ToString(Formatting.Indented), new UTF8Encoding(true));
		if (skipped.Count > 0)
		{
			Console.WriteLine($"  {skipped.Count} blocks absent from the documentation: {string.Join(", ", skipped.Order().Take(8))}");
		}
		return entries.Count;
	}

	private static JObject Entry(string name, JObject states)
	{
		return new JObject {["name"] = name, ["states"] = states, ["networkId"] = NetworkId(name, states)};
	}

	private static IEnumerable<JObject> Product(List<string> states, List<JArray> axes)
	{
		var indices = new int[states.Count];
		while (true)
		{
			var combination = new JObject();
			for (int i = 0; i < states.Count; i++) combination[states[i]] = axes[i][indices[i]];
			yield return combination;

			int digit = states.Count - 1;
			while (digit >= 0)
			{
				if (++indices[digit] < axes[digit].Count) break;
				indices[digit--] = 0;
			}
			if (digit < 0) yield break;
		}
	}

	/// <summary>
	///     FNV-1a 32 over the little endian NBT of {name, states}, states in lexical order. The client
	///     computes the same number from the same two things, which is why the order entries are
	///     written in never reaches the wire.
	/// </summary>
	private static uint NetworkId(string name, JObject states)
	{
		if (name == "minecraft:unknown") return unchecked((uint) -2);

		var nbt = new List<byte>();
		void String(string value)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(value);
			nbt.AddRange(BitConverter.GetBytes((ushort) bytes.Length));
			nbt.AddRange(bytes);
		}

		nbt.Add(0x0a);
		String("");
		nbt.Add(0x08);
		String("name");
		String(name);
		nbt.Add(0x0a);
		String("states");

		foreach (JProperty state in states.Properties().OrderBy(p => p.Name, StringComparer.Ordinal))
		{
			switch (state.Value.Type)
			{
				case JTokenType.Boolean:
					nbt.Add(0x01);
					String(state.Name);
					nbt.Add((byte) ((bool) state.Value ? 1 : 0));
					break;
				case JTokenType.String:
					nbt.Add(0x08);
					String(state.Name);
					String((string) state.Value);
					break;
				default:
					nbt.Add(0x03);
					String(state.Name);
					nbt.AddRange(BitConverter.GetBytes((int) state.Value));
					break;
			}
		}

		nbt.Add(0x00);
		nbt.Add(0x00);

		uint hash = 0x811c9dc5;
		foreach (byte b in nbt)
		{
			hash ^= b;
			hash *= 0x01000193;
		}
		return hash;
	}
}
