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

/// <summary>How a member is written, so it can be read the way it was stored.</summary>
public enum MemberKind
{
	Bool,

	/// <summary>One bit of a packed byte. The member states which bit.</summary>
	Bit,

	Enum8,
	Enum32,

	/// <summary>Eight bits unsigned, held as a number rather than as a name or a set of flags.</summary>
	UInt8,

	UInt16,
	Int16,
	Int32,

	/// <summary>Thirty two bits unsigned. A hash with its top bit set is not a negative number.</summary>
	UInt32,
	UInt64,
	Bits64,
	Float,
	Text,
	Hashed,
	Colour,
	Range,
	Version,
	Box,

	/// <summary>Three floats: a position or an offset, not a box.</summary>
	Vector3,

	Container,

	/// <summary>
	///     A pointer to a component. The object it names is a ComponentInstance, a method table and
	///     then the component, so the class this states is read eight bytes past the pointer.
	/// </summary>
	Component,

	/// <summary>
	///     An ItemDescriptor: its own bytes at offset 8 hold a pointer to an object whose
	///     std::string name sits at that object's own offset 8. Read as the name, or null when the
	///     pointer is unset (no item named) or nothing at the far end reads back as a string.
	/// </summary>
	Descriptor,

	/// <summary>A byte of flags over an enum: the members it names are the bits that are set.</summary>
	Flags8,

	/// <summary>
	///     Bytes the compiler inserted so the next member lands on its own alignment. Not data and
	///     not undecoded: a gap smaller than the following member's alignment, ending where that
	///     member has to start, can be nothing else.
	/// </summary>
	Padding,

	/// <summary>
	///     Bytes of the class that no member accounts for and alignment does not explain. These are
	///     the countable holes: something is there and nothing declares it.
	/// </summary>
	Unknown
}

/// <summary>
///     One member of a class: where it starts inside that class, how wide the value is, what it is
///     called, and, when it holds another object, which class that is. The offset is stated by the
///     reference and is relative to the class that declares it, never to whatever holds that class.
/// </summary>
public readonly record struct BlockMember(int At, int Bytes, MemberKind Kind, string Name, string Holds = null,
	int Bit = 0, bool Comparable = true, string Enum = null, string Elements = null,
	string Entries = null);

/// <summary>One class: how big it is and what it holds, in the order it declares them.</summary>
public sealed record ClassLayout(string Name, int Size, IReadOnlyList<BlockMember> Members);

/// <summary>
///     One member as it actually occurs: the member itself, what it holds if it holds a class, and
///     where it sits in the object that holds all of it.
///     <para>
///         The number is how the check pairs an occurrence with the value the reference states for
///         it. The path is for a person reading a report. Neither is what the data is: the data is
///         this tree.
///     </para>
/// </summary>
public sealed class MemberNode
{
	public int Index { get; init; }
	public BlockMember Member { get; init; }
	public string Path { get; init; }

	/// <summary>
	///     Where this occurrence starts in the object, which is its own class's offset added to the
	///     offset of everything holding it. Stated by the reference, not measured.
	/// </summary>
	public int At { get; init; }

	public IReadOnlyList<MemberNode> Holds { get; init; } = [];
	public bool IsLeaf => Holds.Count == 0;
}

/// <summary>A class as a tree of its members, with every occurrence numbered once.</summary>
public sealed class ClassTree
{
	/// <summary>The class's own members, each holding whatever it holds.</summary>
	public IReadOnlyList<MemberNode> Roots { get; init; } = [];

	/// <summary>Every value in the tree, for a caller that wants them all without walking.</summary>
	public IReadOnlyList<MemberNode> Leaves { get; init; } = [];
}

/// <summary>
///     Every class the reference states, read out of it rather than written here.
///     <para>
///         Each class carries its size and its members, and each member its name, its offset inside
///         that class, its width and its kind. A member holding another class names it, and that
///         class states its own members at its own offsets, so a class that moves takes what is
///         inside it along without a single number changing.
///     </para>
///     <para>
///         The names are the class's own with the m dropped, so nothing here is a name this tool
///         invented. Members holding something with no class of their own are listed too, with their
///         offset and size and no value, so the part of the object nothing reads stays countable.
///     </para>
/// </summary>
public static class BlockMembers
{
	/// <summary>Which extraction a class came out of, because each states its own.</summary>
	public enum Source
	{
		Blocks,
		States,
		Items,
		Creative
	}

	private static readonly string[] Files =
		["blocks.json", "block_states.json", "items-runtime.json", "creative_items.json"];
	private static readonly Dictionary<string, ClassLayout>[] Classes = new Dictionary<string, ClassLayout>[4];
	private static readonly string[] Roots = new string[4];
	private static readonly ClassTree[] Trees = new ClassTree[4];
	private static Version _build;

	/// <summary>The build the reference was read on.</summary>
	public static Version Build
	{
		get
		{
			Load(Source.Blocks);
			return _build;
		}
	}

	/// <summary>The class an object of that kind is, as the tree it is. Built once.</summary>
	public static ClassTree Tree(Source source)
	{
		Load(source);
		return Trees[(int) source] ??= Grow(Root(source), source);
	}

	/// <summary>The class an object of that kind is.</summary>
	public static ClassLayout Root(Source source)
	{
		Load(source);
		return Class(source, Roots[(int) source]);
	}

	/// <summary>The class a member holds, from the same file that member came from.</summary>
	public static ClassLayout Held(string name, Source source) => Class(source, name);

	/// <summary>The class if this file declares it, and null if it does not. No exception either way.</summary>
	public static ClassLayout Declared(string name, Source source)
	{
		if (name is null) return null;
		Load(source);
		return Classes[(int) source].GetValueOrDefault(name);
	}

	/// <summary>
	///     The class a component of this name is, or null where none is declared. A component is an
	///     object like any other: the reference states its members and the run reads them, instead
	///     of a decode written per name in the source.
	/// </summary>
	/// <summary>A class by name, from whichever file declares it, or null if none does.</summary>
	public static ClassLayout Any(string name)
	{
		if (name is null) return null;
		foreach (Source source in new[] { Source.Blocks, Source.States, Source.Items })
		{
			Load(source);
			if (Classes[(int) source].TryGetValue(name, out ClassLayout held)) return held;
		}

		return null;
	}

	public static ClassLayout Component(string name)
	{
		if (name is null) return null;
		foreach (Source source in new[] { Source.Blocks, Source.States, Source.Items }) Load(source);
		return _componentClasses.TryGetValue(name, out string held) ? Any(held) : null;
	}

	private static readonly Dictionary<string, string> _componentClasses = new(StringComparer.Ordinal);

	private static ClassLayout Class(Source source, string name)
	{
		Load(source);
		if (name is null) return null;
		return Classes[(int) source].TryGetValue(name, out ClassLayout held)
			? held
			: throw new InvalidOperationException($"the reference names a class {name} and then does not state it");
	}

	/// <summary>
	///     A class as the tree it is: every member in the order it is declared, and inside a member
	///     that holds another class, that class's members. Nothing is flattened. The leaf list beside
	///     it is the same nodes, for a caller that wants to visit every value without walking.
	/// </summary>
	private static ClassTree Grow(ClassLayout held, Source source)
	{
		var all = new List<MemberNode>();
		IReadOnlyList<MemberNode> roots = Branch(held, source, null, 0, all);
		return new ClassTree { Roots = roots, Leaves = all.Where(n => n.IsLeaf).ToList() };
	}

	private static IReadOnlyList<MemberNode> Branch(ClassLayout held, Source source, string path, int at,
		List<MemberNode> all)
	{
		var nodes = new List<MemberNode>(held.Members.Count);
		foreach (BlockMember member in held.Members)
		{
			string name = path is null ? member.Name : $"{path}.{member.Name}";

			// The slot is taken before the members inside are built, so a member is numbered
			// before whatever it holds and the flat list reads in declaration order.
			int index = all.Count;
			all.Add(null);
			var node = new MemberNode
			{
				Index = index,
				Member = member,
				Path = name,
				At = at + member.At,
				// A component names the class behind a pointer, not one sitting inside the object, so
				// it stays a leaf and the reader follows the pointer. Nesting it here would read the
				// class at the pointer's own offset, which is the pointer itself.
				Holds = member.Holds is null || member.Kind == MemberKind.Component
					? []
					: Branch(Held(member.Holds, source), source, name, at + member.At, all)
			};

			all[index] = node;
			nodes.Add(node);
		}

		return nodes;
	}

	private static void Load(Source source)
	{
		int which = (int) source;
		if (Classes[which] is not null) return;
		(Classes[which], Roots[which], Version build) = LoadClasses(Files[which]);
		_build ??= build;
	}

	/// <summary>
	///     What the values of one enum are called, where the reference states them. A number that
	///     names something is written with the name beside it, because the number is what was read
	///     and the name is what it means, and dropping either one loses half the fact.
	/// </summary>
	public static string Value(string enumeration, long value)
	{
		Load(Source.Blocks);
		return _enums.TryGetValue(enumeration ?? "", out var named)
			&& named.TryGetValue(value, out string name) ? name : null;
	}

	private static readonly Dictionary<string, Dictionary<long, string>> _enums = new(StringComparer.Ordinal);

	/// <summary>
	///     An enum whose names the server holds as data rather than the reference: read from the
	///     process by the run and then stated like any other, so a member declared over it names its
	///     numbers and the output file carries the table.
	///     <para>
	///         Refused when the reference already states that enum, because two sources for one fact
	///         is how one of them silently wins. The caller says which happened.
	///     </para>
	/// </summary>
	public static bool Discovered(string enumeration, IReadOnlyDictionary<long, string> values)
	{
		foreach (Source source in new[] { Source.Blocks, Source.States, Source.Items }) Load(source);
		if (_enums.ContainsKey(enumeration)) return false;

		_enums[enumeration] = new Dictionary<long, string>(values);
		return true;
	}

	/// <summary>
	///     What the reference carries besides its classes: the names behind each enum, and which
	///     class each component is. A run writes these back out, because a file that cannot become
	///     the next reference is not the reference format, and losing them silently is how nine flag
	///     bits and every enum name went missing from a refreshed reference.
	/// </summary>
	public static IReadOnlyDictionary<string, Dictionary<long, string>> Enums
	{
		get
		{
			foreach (Source source in new[] { Source.Blocks, Source.States, Source.Items }) Load(source);
			return _enums;
		}
	}

	public static IReadOnlyDictionary<string, string> ComponentClasses
	{
		get
		{
			foreach (Source source in new[] { Source.Blocks, Source.States, Source.Items }) Load(source);
			return _componentClasses;
		}
	}

	private static (Dictionary<string, ClassLayout>, string, Version) LoadClasses(string file)
	{
		var classes = new Dictionary<string, ClassLayout>(StringComparer.Ordinal);
		string path = Path.Combine(WorldConfig.AssetsDirectory(), "reference", file);
		if (!File.Exists(path))
		{
			Console.Error.WriteLine($"no reference at {path}; there is no class to read anything as");
			return (classes, null, null);
		}

		using JsonDocument document = JsonDocument.Parse(File.ReadAllText(path));
		JsonElement root = document.RootElement;

		Version build = null;
		if (root.TryGetProperty("layoutPublishedFor", out JsonElement stated)
			&& Version.TryParse(stated.GetString() ?? "", out Version parsed))
		{
			build = parsed;
		}

		if (root.TryGetProperty("enums", out JsonElement enums))
		{
			foreach (JsonProperty held in enums.EnumerateObject())
			{
				var values = new Dictionary<long, string>();
				foreach (JsonProperty entry in held.Value.EnumerateObject())
				{
					if (long.TryParse(entry.Name, out long number)) values[number] = entry.Value.GetString();
				}

				_enums[held.Name] = values;
			}
		}

		// Which class each component is, where the reference says. Only the block file carries this;
		// the others have no components of their own to name.
		if (root.TryGetProperty("componentClasses", out JsonElement components))
		{
			foreach (JsonProperty held in components.EnumerateObject())
			{
				_componentClasses[held.Name] = held.Value.GetString();
			}
		}

		string name = root.TryGetProperty("class", out JsonElement which) ? which.GetString() : null;
		if (!root.TryGetProperty("classes", out JsonElement stateClasses)) return (classes, name, build);

		foreach (JsonProperty held in stateClasses.EnumerateObject())
		{
			int size = held.Value.TryGetProperty("size", out JsonElement bytes) ? bytes.GetInt32() : 0;
			var members = new List<BlockMember>();
			if (held.Value.TryGetProperty("members", out JsonElement stated2))
			{
				foreach (JsonElement member in stated2.EnumerateArray())
				{
					if (!member.TryGetProperty("name", out JsonElement memberName)) continue;
					if (!member.TryGetProperty("at", out JsonElement at)) continue;
					if (!member.TryGetProperty("bytes", out JsonElement memberBytes)) continue;
					if (!member.TryGetProperty("kind", out JsonElement kind)) continue;
					if (!Enum.TryParse(kind.GetString(), out MemberKind parsedKind)) continue;

					// A gap is worked out from the members around it, so it is never read back as one
					// of them. Taking it from the file freezes it: version widened from one byte to
					// four and the gap that used to follow it stayed where it was, overlapping it.
					if (parsedKind is MemberKind.Unknown or MemberKind.Padding) continue;
					string holds = member.TryGetProperty("holds", out JsonElement inner) ? inner.GetString() : null;
					int bit = member.TryGetProperty("bit", out JsonElement index) ? index.GetInt32() : 0;
			string enumeration = member.TryGetProperty("enum", out JsonElement named) ? named.GetString() : null;
			string elements = member.TryGetProperty("elements", out JsonElement each) ? each.GetString() : null;
			string entries = member.TryGetProperty("entries", out JsonElement pair) ? pair.GetString() : null;
					bool comparable = !member.TryGetProperty("comparable", out JsonElement across)
						|| across.ValueKind != JsonValueKind.False;
					members.Add(new BlockMember(at.GetInt32(), memberBytes.GetInt32(), parsedKind,
						memberName.GetString(), holds, bit, comparable, enumeration, elements, entries));
				}
			}

			classes[held.Name] = new ClassLayout(held.Name, size, Tile(members, size));
		}

		return (classes, name, build);
	}

	/// <summary>
	///     The members in order with every gap between them named and sized, so the list accounts
	///     for the whole class. A class whose members are read top to bottom must add up to its own
	///     size; bytes that nothing declares are the ones that quietly stop adding up, so they are
	///     declared as what they are: unknown, this long, here.
	///     Bits are why this counts coverage rather than summing sizes. Several of them share one
	///     byte by design, so the same byte is claimed more than once and a sum would say the class
	///     is bigger than it is.
	/// </summary>
	/// <summary>
	///     What a member has to sit on. Taken from the kind, because that is what the size and the
	///     type give: a byte anywhere, a short on two, a float or an int on four, and anything built
	///     from pointers on eight.
	/// </summary>
	private static int Alignment(BlockMember member)
	{
		switch (member.Kind)
		{
			case MemberKind.Bool:
			case MemberKind.Bit:
			case MemberKind.Enum8: return 1;
			case MemberKind.UInt16:
			case MemberKind.Int16: return 2;
			case MemberKind.Int32:
			case MemberKind.UInt32:
			case MemberKind.Enum32:
			case MemberKind.Float:
			case MemberKind.Colour:
			case MemberKind.Range:
			case MemberKind.Box:
			case MemberKind.Vector3: return 4;
			default: return 8;
		}
	}

	private static int Alignment(IEnumerable<BlockMember> members)
	{
		return members.Select(Alignment).DefaultIfEmpty(1).Max();
	}

	private static List<BlockMember> Tile(List<BlockMember> members, int size)
	{
		if (members.Count == 0) return members;

		var covered = new bool[Math.Max(size, members.Max(m => m.At + m.Bytes))];
		foreach (BlockMember member in members)
		{
			for (int i = member.At; i < member.At + member.Bytes && i < covered.Length; i++) covered[i] = true;
		}

		var gaps = new List<BlockMember>();
		int unknown = 0, padding = 0;
		for (int at = 0; at < size; at++)
		{
			if (covered[at]) continue;
			int end = at;
			while (end < size && !covered[end]) end++;
			// Alignment or a hole, decided by the rule rather than by eye: the gap is padding when
			// it is narrower than the alignment of the member that follows it and that member sits
			// on that alignment. A trailing gap is padding when the class's own size is rounded to
			// its widest member's alignment.
			BlockMember next = members.Where(m => m.At >= end).OrderBy(m => m.At).FirstOrDefault();
			int need = next.Name is null ? Alignment(members) : Alignment(next);
			bool fits = end - at < need && (next.Name is null ? size % need == 0 : next.At % need == 0);
			gaps.Add(fits
				? new BlockMember(at, end - at, MemberKind.Padding, $"padding{++padding}", null, 0, false)
				: new BlockMember(at, end - at, MemberKind.Unknown, $"unknown{++unknown}", null, 0, false));
			at = end;
		}

		if (gaps.Count == 0) return members;

		// In order, because the point is that the list walks the object.
		return members.Concat(gaps).OrderBy(m => m.At).ThenBy(m => m.Bit).ToList();
	}
}
