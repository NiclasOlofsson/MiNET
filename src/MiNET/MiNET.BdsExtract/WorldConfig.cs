namespace MiNET.BdsExtract;

using System.Text;

/// <summary>
///     What the server being read is configured as, taken from the server itself.
///     <para>
///         This exists because of what an item extraction quietly loses without it. The item
///         registry a running server holds is not the registry it documents: experimental content is
///         registered only when the world it is serving has the toggle on. With the toggle off, 50
///         items are simply absent, and worse, the ids of everything registered after them are two
///         lower, so a whole file of plausible looking numbers is wrong in a way nothing in the file
///         reveals. Both were measured, not supposed: with the toggle on the extract matched the
///         server's own documentation 1607 of 1607, with none absent and no id off by anything.
///     </para>
///     <para>
///         So the configuration is read before the extraction runs, checked, and written out beside
///         the data. A capture whose configuration is not recorded cannot be compared with another.
///     </para>
/// </summary>
public static class WorldConfig
{
	public sealed record Config(string ServerPath, string World, string WorldPath,
		IReadOnlyList<string> Experiments, IReadOnlyList<string> AllToggles, string NetworkIdsAreHashes);

	/// <summary>
	///     Bookkeeping rather than content. These two say a world has been near an experiment, not
	///     that any feature is on, so a world carrying only these has nothing enabled.
	/// </summary>
	private static readonly HashSet<string> Bookkeeping = new(StringComparer.Ordinal)
	{
		"experiments_ever_used", "saved_with_toggled_experiments"
	};

	/// <summary>
	///     Reads the configuration of the server this process is running, by following what the
	///     server itself holds: the folder its executable sits in, the level name in its
	///     server.properties, and the experiments in that world's level.dat.
	/// </summary>
	public static Config Read(string executablePath)
	{
		string root = Path.GetDirectoryName(executablePath) ?? ".";
		string properties = Path.Combine(root, "server.properties");
		string world = null;
		string hashes = null;
		if (File.Exists(properties))
		{
			foreach (string line in File.ReadAllLines(properties))
			{
				if (line.StartsWith("level-name=", StringComparison.Ordinal)) world = line["level-name=".Length..].Trim();
				if (line.StartsWith("block-network-ids-are-hashes=", StringComparison.Ordinal)) hashes = line["block-network-ids-are-hashes=".Length..].Trim();
			}
		}

		string worldPath = world is null ? null : Path.Combine(root, "worlds", world);
		var on = new List<string>();
		var all = new List<string>();
		string level = worldPath is null ? null : Path.Combine(worldPath, "level.dat");
		if (level is not null && File.Exists(level)) ReadExperiments(level, on, all);
		return new Config(root, world, worldPath, on, all, hashes);
	}

	/// <summary>
	///     The canonical configuration this project carries: the server.properties and the
	///     experimental flatworld every extraction runs against, so two runs are comparable because
	///     they ran the same config, not because somebody remembered to set it up the same way.
	/// </summary>
	public static string AssetsDirectory()
	{
		var directory = new DirectoryInfo(AppContext.BaseDirectory);
		while (directory is not null)
		{
			if (directory.GetFiles("MiNET.BdsExtract.csproj").Length > 0)
			{
				return Path.Combine(directory.FullName, "Assets");
			}
			directory = directory.Parent;
		}
		return "Assets";
	}

	/// <summary>The experiments the asset world carries, which is what a run's world must have on.</summary>
	public static List<string> AssetExperiments()
	{
		var on = new List<string>();
		var all = new List<string>();
		string level = Path.Combine(AssetsDirectory(), "flatworld", "level.dat");
		if (File.Exists(level)) ReadExperiments(level, on, all);
		return on;
	}

	/// <summary>
	///     Brings a server folder to the canonical configuration: the asset server.properties, and a
	///     fresh copy of the asset world. The world is replaced rather than merged, because a world a
	///     newer server has already upgraded in place is not the asset any more, and a conformance
	///     run on top of it measures the leftovers instead of the config.
	/// </summary>
	public static int Prepare(string serverDirectory)
	{
		if (!File.Exists(Path.Combine(serverDirectory, "bedrock_server.exe")))
		{
			Console.Error.WriteLine($"not a server folder (no bedrock_server.exe): {serverDirectory}");
			return 1;
		}

		string assets = AssetsDirectory();
		string properties = Path.Combine(assets, "server.properties");
		string world = Path.Combine(assets, "flatworld");
		if (!File.Exists(properties) || !Directory.Exists(world))
		{
			Console.Error.WriteLine($"assets missing under {assets}; expected server.properties and flatworld");
			return 1;
		}

		File.Copy(properties, Path.Combine(serverDirectory, "server.properties"), true);
		Console.WriteLine($"wrote {Path.Combine(serverDirectory, "server.properties")}");

		// A leftover documentation config makes the server generate its docs and quit right after
		// "Server started", and an extraction that attaches then reads a dying process: the guards
		// refuse it, but the run is wasted. Part of the canonical config is that this file is gone.
		string testConfig = Path.Combine(serverDirectory, "test_config.json");
		if (File.Exists(testConfig))
		{
			File.Delete(testConfig);
			Console.WriteLine($"removed {testConfig} (documentation mode: the server would quit after startup)");
		}

		string target = Path.Combine(serverDirectory, "worlds", "flatworld");
		if (Directory.Exists(target))
		{
			Directory.Delete(target, true);
			Console.WriteLine($"removed {target} (a world an earlier run upgraded is not the asset)");
		}
		CopyTree(world, target);
		Console.WriteLine($"copied the asset world to {target}");
		return 0;
	}

	private static void CopyTree(string from, string to)
	{
		Directory.CreateDirectory(to);
		foreach (string file in Directory.GetFiles(from)) File.Copy(file, Path.Combine(to, Path.GetFileName(file)));
		foreach (string sub in Directory.GetDirectories(from)) CopyTree(sub, Path.Combine(to, Path.GetFileName(sub)));
	}

	/// <summary>
	///     Refuses to extract from a server that is not running the canonical configuration, because
	///     what comes out is wrong in ways the output does not show. A world without the asset's
	///     experiments loses registrations AND shifts every id after them; hashed network ids make
	///     every palette position incomparable to anything this project holds. Says what it found,
	///     either way, so the configuration is on the record next to the run.
	/// </summary>
	public static bool Check(Config config, bool require)
	{
		Console.WriteLine($"server   {config.ServerPath}");
		Console.WriteLine($"world    {config.World ?? "(no level-name in server.properties)"}");
		Console.WriteLine($"network ids  block-network-ids-are-hashes={config.NetworkIdsAreHashes ?? "(not stated)"}");
		if (config.AllToggles.Count == 0)
		{
			Console.WriteLine("experiments  none recorded in level.dat");
		}
		else
		{
			Console.WriteLine($"experiments  on: {(config.Experiments.Count == 0 ? "(none)" : string.Join(", ", config.Experiments))}");
			IEnumerable<string> off = config.AllToggles.Except(config.Experiments).Except(Bookkeeping);
			if (off.Any()) Console.WriteLine($"             off: {string.Join(", ", off)}");
		}

		if (!require) return true;

		var reasons = new List<string>();
		if (config.NetworkIdsAreHashes != "false")
		{
			reasons.Add("block-network-ids-are-hashes is not false, so palette positions are not runtime");
			reasons.Add("  ids and nothing extracted is comparable to the data this project holds.");
		}

		List<string> expected = AssetExperiments();
		if (expected.Count == 0)
		{
			reasons.Add($"the asset world under {AssetsDirectory()} is missing or carries no experiments,");
			reasons.Add("  so there is nothing to hold this run's world against.");
		}
		else
		{
			// An asset experiment this build does not even list cannot filter its registry: 1.26.30
			// drops y_2026_drop_3 from the level.dat outright because nothing in that build knows the
			// toggle. That is said out loud and tolerated. A toggle the build KNOWS but has off is
			// the silent-loss case, and that refuses.
			List<string> unknown = expected.Except(config.AllToggles).ToList();
			if (unknown.Count > 0)
			{
				Console.WriteLine($"             not known to this build (tolerated): {string.Join(", ", unknown)}");
			}

			List<string> missing = expected.Intersect(config.AllToggles).Except(config.Experiments).ToList();
			if (missing.Count > 0)
			{
				reasons.Add($"the world has experiments off that the asset world has on: {string.Join(", ", missing)}.");
				reasons.Add("  The registries a server holds are filtered by its world's toggles: content is");
				reasons.Add("  absent AND every id after it is short by the number missing, which is not");
				reasons.Add("  visible in the output.");
			}
		}

		if (reasons.Count == 0) return true;
		Console.WriteLine();
		Console.WriteLine("REFUSING to extract: this server is not running the canonical configuration.");
		foreach (string reason in reasons) Console.WriteLine($"  {reason}");
		Console.WriteLine($"  Run --prepare {config.ServerPath} and start the server again,");
		Console.WriteLine("  or pass --any-world to extract anyway.");
		return false;
	}

	/// <summary>
	///     The experiments compound out of a Bedrock level.dat, which is little endian NBT behind an
	///     eight byte header of a storage version and a byte length.
	///     Walked by the tag structure rather than searched for by name. A search would find the same
	///     text in a chunk of unrelated bytes and report a toggle that is not there, and the whole
	///     point of this is to be trusted about a configuration nothing else records.
	/// </summary>
	private static void ReadExperiments(string path, List<string> on, List<string> all)
	{
		byte[] data = File.ReadAllBytes(path);
		if (data.Length < 8) return;
		int length = BitConverter.ToInt32(data, 4);
		var reader = new Nbt(data, 8, Math.Min(data.Length, 8 + length));

		byte type = reader.Byte();
		reader.String();
		if (type != 10) return;
		while (true)
		{
			byte child = reader.Byte();
			if (child == 0) break;
			string name = reader.String();
			if (child == 10 && name == "experiments")
			{
				while (true)
				{
					byte toggle = reader.Byte();
					if (toggle == 0) break;
					string key = reader.String();
					object value = reader.Payload(toggle);
					all.Add(key);
					if (!Bookkeeping.Contains(key) && toggle == 1 && (byte) value != 0) on.Add(key);
				}
				return;
			}
			reader.Payload(child);
		}
	}

	private sealed class Nbt(byte[] data, int at, int end)
	{
		public byte Byte() => at < end ? data[at++] : (byte) 0;

		private short Int16() { short v = BitConverter.ToInt16(data, at); at += 2; return v; }
		private int Int32() { int v = BitConverter.ToInt32(data, at); at += 4; return v; }

		public string String()
		{
			int size = Int16();
			string text = Encoding.UTF8.GetString(data, at, size);
			at += size;
			return text;
		}

		public object Payload(byte type)
		{
			switch (type)
			{
				case 1: return Byte();
				case 2: return Int16();
				case 3: return Int32();
				case 4: at += 8; return null;
				case 5: at += 4; return null;
				case 6: at += 8; return null;
				case 7: at += Int32(); return null;
				case 8: return String();
				case 9:
				{
					byte element = Byte();
					int count = Int32();
					for (int i = 0; i < count; i++) Payload(element);
					return null;
				}
				case 10:
				{
					while (true)
					{
						byte child = Byte();
						if (child == 0) return null;
						String();
						Payload(child);
					}
				}
				case 11: at += Int32() * 4; return null;
				case 12: at += Int32() * 8; return null;
				default: throw new InvalidDataException($"nbt tag {type} at {at}");
			}
		}
	}
}
