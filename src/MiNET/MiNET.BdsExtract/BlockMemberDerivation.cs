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
///     Finds where each member of a class sits on a build the class layout is not published for, by
///     scoring every position in the object against the build it is published for.
///     <para>
///         The reference is an extraction of 1.26.20.5 read at the offsets its own class declaration
///         states. An object is matched between the two builds by what it is: a block by its full
///         name, a state by its block's name and its property values. A position holds a member when
///         it gives that member's reference value for at least the floor of the objects both builds
///         have.
///     </para>
///     <para>
///         A member the game changed on a few objects still locates, because the floor is not one. A
///         member nothing settles is placed nowhere and reported unsettled, rather than quietly read
///         from a position nothing confirmed.
///     </para>
///     <para>
///         The block class and the state class run the same search. What separates them is the
///         member list, the reference file, what an object is keyed by, and where a settled position
///         is recorded, all of which the caller states.
///     </para>
/// </summary>
public static class BlockMemberDerivation
{
	/// <summary>What one member's search found. A member nothing settled sits at -1, which is nowhere.</summary>
	public readonly record struct Found(string Name, int At, int Held, int Of, bool Settled);

	/// <summary>One object to score: what it is, and where it is.</summary>
	public readonly record struct Subject(string Key, ulong Address);

	/// <summary>One value inside a member: what to call it, where it sits, and how it reads.</summary>
	private readonly record struct Leaf(string Path, int At, BlockMember Member);

	/// <summary>
	///     Every value a member holds, by the path the reference states it under. A member that is
	///     another object contributes everything inside it, at the offsets that object's own class
	///     declares. That is the whole point of doing it this way: the members of one object have a
	///     single degree of freedom between them, so they are searched for together, once, and the
	///     search cannot put two of them in places that are not the distance apart they must be.
	/// </summary>
	private static List<Leaf> Leaves(BlockMember member, bool state, string path = null, int at = 0)
	{
		path = path is null ? member.Name : $"{path}.{member.Name}";
		if (member.Kind != MemberKind.Container) return [new Leaf(path, at, member)];
		if (member.Holds is null) return [];

		var leaves = new List<Leaf>();
		foreach (BlockMember inner in BlockMembers.Held(member.Holds, state).Members)
		{
			leaves.AddRange(Leaves(inner, state, path, at + inner.At));
		}

		return leaves;
	}

	/// <summary>Every block, keyed by its full name, which is what a block is.</summary>
	public static List<Found> MeasureBlocks(BedrockProcess process, IReadOnlyList<BlockProperties> blocks, int reach)
	{
		return Measure(process, BlockLayout.Members, false,
			blocks.Select(b => new Subject(b.Name, b.Address)).ToList(),
			Load("blocks.json", "blocks", b => b.TryGetProperty("name", out JsonElement name)
				&& name.ValueKind == JsonValueKind.String ? name.GetString() : null),
			reach, BlockLayout.Measured);
	}

	/// <summary>
	///     Every state, keyed by its block's name and its property values. Never by its place in the
	///     palette: the order is the server's own and two builds do not have to agree on it, so an
	///     index would compare one build's state against another build's neighbour.
	/// </summary>
	public static List<Found> MeasureStates(BedrockProcess process, IReadOnlyList<PaletteEntry> palette, int reach)
	{
		return Measure(process, BlockLayout.StateMembers, true,
			palette.Select(e => new Subject(StateSentinels.Key(e.Name, e.States), e.Address)).ToList(),
			Load("block_states.json", "states", StateSentinels.KeyOf),
			reach, BlockLayout.StateMeasured);
	}

	private static List<Found> Measure(BedrockProcess process, IReadOnlyList<BlockMember> members, bool state,
		IReadOnlyList<Subject> subjects, Dictionary<string, Dictionary<string, string>> known,
		int reach, Action<string, int> place)
	{
		var results = new List<Found>();
		if (known.Count == 0) return results;

		// Every object both builds have, read once. The search runs over these windows rather than
		// over the process, so a member costs one pass instead of one read a position.
		var windows = new List<(byte[] Window, ulong Address, Dictionary<string, string> Wanted)>();
		foreach (Subject subject in subjects)
		{
			if (!known.TryGetValue(subject.Key, out Dictionary<string, string> wanted)) continue;
			var window = new byte[reach];
			if (process.ReadClipped(subject.Address, window, reach) < reach) continue;
			windows.Add((window, subject.Address, wanted));
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

		foreach (BlockMember member in members)
		{
			List<Leaf> leaves = Leaves(member, state);
			if (leaves.Count == 0)
			{
				results.Add(new Found(member.Name, -1, 0, 0, false));
				continue;
			}

			var wanted = windows.Where(w => leaves.Any(l => w.Wanted.ContainsKey(l.Path))).ToList();
			if (wanted.Count == 0)
			{
				results.Add(new Found(member.Name, -1, 0, 0, false));
				continue;
			}

			(int Best, int Most, int Of, int Ties) score = Score(process, wanted, member, leaves, scratch, reach, null);
			if (score.Most == score.Of && score.Ties == 1)
			{
				place(member.Name, score.Best);
				reserved.Add((score.Best, score.Best + member.Bytes, member.Name));
				results.Add(new Found(member.Name, score.Best, score.Most, score.Of, true));
			}
			else
			{
				pending.Add(member);
			}
		}

		foreach (BlockMember member in pending)
		{
			List<Leaf> leaves = Leaves(member, state);
			var wanted = windows.Where(w => leaves.Any(l => w.Wanted.ContainsKey(l.Path))).ToList();
			(int Best, int Most, int Of, int Ties) score = Score(process, wanted, member, leaves, scratch, reach, reserved);
			bool settled = score.Best >= 0 && score.Most >= Sentinels.Floor * score.Of && score.Ties == 1;
			if (settled)
			{
				place(member.Name, score.Best);
				reserved.Add((score.Best, score.Best + member.Bytes, member.Name));
			}
			results.Add(new Found(member.Name, settled ? score.Best : -1, score.Most, score.Of, settled));
		}

		return results;
	}

	/// <summary>
	///     The position that holds the member for the most objects, how many that is, and how many
	///     positions tie with it. Positions inside an already reserved member are not considered.
	/// </summary>
	private static (int Best, int Most, int Of, int Ties) Score(BedrockProcess process,
		List<(byte[] Window, ulong Address, Dictionary<string, string> Wanted)> wanted,
		BlockMember member, List<Leaf> leaves, byte[] scratch, int reach,
		List<(int From, int To, string Name)> reserved)
	{
		// How many values are being asked for in total: one object's worth is every value inside
		// the member that the reference states for that object. A member holding one value is the
		// same arithmetic with a count of one.
		int of = 0;
		foreach ((byte[] _, ulong _, Dictionary<string, string> values) in wanted)
		{
			of += leaves.Count(l => values.ContainsKey(l.Path));
		}

		int width = Width(member.Kind);
		int best = -1, most = 0, ties = 0;
		for (int at = 0; at + member.Bytes <= reach; at += width)
		{
			if (reserved is not null && reserved.Any(r => at < r.To && at + member.Bytes > r.From)) continue;

			int held = 0;
			foreach ((byte[] window, ulong address, Dictionary<string, string> values) in wanted)
			{
				foreach (Leaf leaf in leaves)
				{
					if (!values.TryGetValue(leaf.Path, out string stated)) continue;
					if (BlockMemberReader.Text(process, address, window, scratch, at + leaf.At, leaf.Member) == stated) held++;
				}
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

		return (best, most, of, ties);
	}

	/// <summary>The step a member of that kind is searched on, which is its own alignment.</summary>
	private static int Width(MemberKind kind) => kind switch
	{
		MemberKind.Bool or MemberKind.Enum8 => 1,
		MemberKind.UInt16 => 2,
		MemberKind.Enum32 or MemberKind.Int32 or MemberKind.Float or MemberKind.Colour or MemberKind.Box => 4,
		_ => 8
	};

	/// <summary>
	///     Every object the reference knows, by what it is, with each member's value written exactly
	///     as the file writes it. An object two keys cannot separate is used by neither, because a
	///     value that could have come from either proves nothing about where it was read.
	/// </summary>
	private static Dictionary<string, Dictionary<string, string>> Load(string file, string array,
		Func<JsonElement, string> keyOf)
	{
		string path = Path.Combine(WorldConfig.AssetsDirectory(), "reference", file);
		var known = new Dictionary<string, Dictionary<string, string>>(StringComparer.Ordinal);
		if (!File.Exists(path))
		{
			Console.Error.WriteLine($"no reference at {path}; no member position can be measured");
			return known;
		}

		using JsonDocument document = JsonDocument.Parse(File.ReadAllText(path));
		if (!document.RootElement.TryGetProperty(array, out JsonElement objects)) return known;

		var seen = new HashSet<string>(StringComparer.Ordinal);
		foreach (JsonElement held in objects.EnumerateArray())
		{
			string key = keyOf(held);
			if (key is null) continue;
			if (!seen.Add(key))
			{
				known.Remove(key);
				continue;
			}

			var values = new Dictionary<string, string>(StringComparer.Ordinal);
			Flatten(held, null, values);
			if (values.Count > 0) known[key] = values;
		}

		return known;
	}

	/// <summary>
	///     Every value the reference states for one object, under the path it sits at. An object
	///     inside an object contributes its own values under its own name, which is the same path
	///     the search builds when it walks the class, so the two meet without either being told
	///     about the other.
	/// </summary>
	private static void Flatten(JsonElement held, string path, Dictionary<string, string> values)
	{
		foreach (JsonProperty member in held.EnumerateObject())
		{
			string name = path is null ? member.Name : $"{path}.{member.Name}";
			if (member.Value.ValueKind == JsonValueKind.Object) Flatten(member.Value, name, values);
			else if (member.Value.ValueKind != JsonValueKind.Null) values[name] = member.Value.GetRawText();
		}
	}
}
