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
		IReadOnlyList<string> Experiments, IReadOnlyList<string> AllToggles);

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
		if (File.Exists(properties))
		{
			foreach (string line in File.ReadAllLines(properties))
			{
				if (line.StartsWith("level-name=", StringComparison.Ordinal)) world = line["level-name=".Length..].Trim();
			}
		}

		string worldPath = world is null ? null : Path.Combine(root, "worlds", world);
		var on = new List<string>();
		var all = new List<string>();
		string level = worldPath is null ? null : Path.Combine(worldPath, "level.dat");
		if (level is not null && File.Exists(level)) ReadExperiments(level, on, all);
		return new Config(root, world, worldPath, on, all);
	}

	/// <summary>
	///     Refuses to extract from a server whose world has no experiment enabled, because what comes
	///     out is short and silently misnumbered rather than merely incomplete. Says what it found,
	///     either way, so the configuration is on the record next to the run.
	/// </summary>
	public static bool Check(Config config, bool require)
	{
		Console.WriteLine($"server   {config.ServerPath}");
		Console.WriteLine($"world    {config.World ?? "(no level-name in server.properties)"}");
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

		if (config.Experiments.Count > 0 || !require) return true;
		Console.WriteLine();
		Console.WriteLine("REFUSING to extract: this world has no experiment enabled.");
		Console.WriteLine("  The registry a server holds is filtered by its world's toggles. Without them the");
		Console.WriteLine("  experimental items are absent AND every id registered after them is short by the");
		Console.WriteLine("  number missing, which is not visible in the output.");
		Console.WriteLine($"  Enable them in {config.WorldPath}\\level.dat, or point level-name at a world that has");
		Console.WriteLine("  them, and start the server again. Pass --any-world to extract anyway.");
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
