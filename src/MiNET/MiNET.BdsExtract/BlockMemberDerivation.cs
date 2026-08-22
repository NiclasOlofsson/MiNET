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

using System.Text.Json;

/// <summary>
///     Finds where each member of the block class sits on a build the class layout is not published
///     for, by scoring every position in the object against the build it is published for.
///     <para>
///         The reference is Assets/reference-block-members.json, an extraction of 1.26.20.5 read at
///         the offsets its own class declaration states. A block is matched between the two builds by
///         its full name, and a position holds a member when it gives that member's reference value
///         for at least the floor of the blocks both builds have.
///     </para>
///     <para>
///         A member the game changed on a few blocks still locates, because the floor is not one. A
///         member nothing settles keeps its published position and is reported unsettled rather than
///         quietly read from a place nothing confirmed.
///     </para>
/// </summary>
public static class BlockMemberDerivation
{
	/// <summary>What one member's search found.</summary>
	public readonly record struct Found(string Name, int At, int Held, int Of, bool Settled);

	public static List<Found> Measure(BedrockProcess process, IReadOnlyList<BlockProperties> blocks, int reach)
	{
		Dictionary<string, Dictionary<string, string>> known = Load();
		var results = new List<Found>();
		if (known.Count == 0) return results;

		// Every block both builds have, read once. The search runs over these windows rather than
		// over the process, so a member costs one pass instead of one read a position.
		var windows = new List<(byte[] Window, ulong Address, Dictionary<string, string> Wanted)>();
		foreach (BlockProperties block in blocks)
		{
			if (!known.TryGetValue(block.Name, out Dictionary<string, string> wanted)) continue;
			var window = new byte[reach];
			if (process.ReadClipped(block.Address, window, reach) < reach) continue;
			windows.Add((window, block.Address, wanted));
		}

		var scratch = new byte[256];

		// Two passes. The first keeps only what is right for every block at exactly one position:
		// a name, a colour, a version, values distinctive enough that nothing else in the object
		// reads the same. Those are reserved, byte for byte.
		// The second looks for everything else and will not consider a position inside a reserved
		// one, because a bool carries a single bit and matches half the object by luck. Without the
		// reservation, solid, isOpaqueFullBlock and lightBlock all settled on the same byte, and one
		// byte is not three members.
		var reserved = new List<(int From, int To, string Name)>();
		var pending = new List<BlockMember>();

		foreach (BlockMember member in BlockLayout.Members)
		{
			if (member.Kind == MemberKind.Container || member.Name == "nameInfo") continue;

			var wanted = windows.Where(w => w.Wanted.ContainsKey(member.Name)).ToList();
			if (wanted.Count == 0)
			{
				results.Add(new Found(member.Name, BlockLayout.At(member.Name), 0, 0, false));
				continue;
			}

			(int Best, int Most, int Ties) score = Score(process, wanted, member, scratch, reach, null);
			if (score.Most == wanted.Count && score.Ties == 1)
			{
				BlockLayout.Measured(member.Name, score.Best);
				reserved.Add((score.Best, score.Best + member.Bytes, member.Name));
				results.Add(new Found(member.Name, score.Best, score.Most, wanted.Count, true));
			}
			else
			{
				pending.Add(member);
			}
		}

		foreach (BlockMember member in pending)
		{
			var wanted = windows.Where(w => w.Wanted.ContainsKey(member.Name)).ToList();
			(int Best, int Most, int Ties) score = Score(process, wanted, member, scratch, reach, reserved);
			bool settled = score.Best >= 0 && score.Most >= Sentinels.Floor * wanted.Count && score.Ties == 1;
			if (settled)
			{
				BlockLayout.Measured(member.Name, score.Best);
				reserved.Add((score.Best, score.Best + member.Bytes, member.Name));
			}
			results.Add(new Found(member.Name, settled ? score.Best : BlockLayout.At(member.Name),
				score.Most, wanted.Count, settled));
		}

		return results;
	}

	/// <summary>
	///     The position that holds the member for the most blocks, how many that is, and how many
	///     positions tie with it. Positions inside an already reserved member are not considered.
	/// </summary>
	private static (int Best, int Most, int Ties) Score(BedrockProcess process,
		List<(byte[] Window, ulong Address, Dictionary<string, string> Wanted)> wanted,
		BlockMember member, byte[] scratch, int reach, List<(int From, int To, string Name)> reserved)
	{
		int width = Width(member.Kind);
		int best = -1, most = 0, ties = 0;
		for (int at = 0; at + member.Bytes <= reach; at += width)
		{
			if (reserved is not null && reserved.Any(r => at < r.To && at + member.Bytes > r.From)) continue;

			int held = 0;
			foreach ((byte[] window, ulong address, Dictionary<string, string> values) in wanted)
			{
				if (Reads(process, window, address, at, member, scratch) == values[member.Name]) held++;
			}

			if (held > most)
			{
				most = held;
				best = at;
				ties = 1;
			}
			else if (held == most && held > 0)
			{
				ties++;
			}
		}

		return (best, most, ties);
	}

	/// <summary>What that position holds, written the way the reference writes it.</summary>
	private static string Reads(BedrockProcess process, byte[] window, ulong address, int at,
		BlockMember member, byte[] scratch)
	{
		return BlockMemberReader.Text(process, address, window, scratch, at, member);
	}

	/// <summary>The step a member of that kind is searched on, which is its own alignment.</summary>
	private static int Width(MemberKind kind) => kind switch
	{
		MemberKind.Bool or MemberKind.Enum8 => 1,
		MemberKind.UInt16 => 2,
		MemberKind.Enum32 or MemberKind.Int32 or MemberKind.Float or MemberKind.Colour or MemberKind.Box => 4,
		_ => 8
	};

	/// <summary>Every block the reference knows, by name, with each member's value as it writes it.</summary>
	private static Dictionary<string, Dictionary<string, string>> Load()
	{
		string path = Path.Combine(WorldConfig.AssetsDirectory(), "reference", "blocks.json");
		var known = new Dictionary<string, Dictionary<string, string>>(StringComparer.Ordinal);
		if (!File.Exists(path))
		{
			Console.Error.WriteLine($"no reference at {path}; no member position can be measured");
			return known;
		}

		using JsonDocument document = JsonDocument.Parse(File.ReadAllText(path));
		if (!document.RootElement.TryGetProperty("blocks", out JsonElement blocks)) return known;

		foreach (JsonElement block in blocks.EnumerateArray())
		{
			if (!block.TryGetProperty("name", out JsonElement name)) continue;
			if (name.ValueKind != JsonValueKind.String) continue;

			var values = new Dictionary<string, string>(StringComparer.Ordinal);
			foreach (JsonProperty held in block.EnumerateObject())
			{
				if (held.Value.ValueKind == JsonValueKind.Null) continue;
				values[held.Name] = held.Value.GetRawText();
			}

			if (values.Count > 0) known[name.GetString()] = values;
		}

		return known;
	}
}
