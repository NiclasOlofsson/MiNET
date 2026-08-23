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
	Enum8,
	Enum32,
	UInt16,
	Int16,
	Int32,
	UInt64,
	Bits64,
	Float,
	Text,
	Hashed,
	Colour,
	Range,
	Version,
	Box,
	Container
}

/// <summary>
///     One member of a class: where it starts inside that class, how wide the value is, what it is
///     called, and, when it holds another object, which class that is. The offset is stated by the
///     reference and is relative to the class that declares it, never to whatever holds that class.
/// </summary>
public readonly record struct BlockMember(int At, int Bytes, MemberKind Kind, string Name, string Holds = null);

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
		Items
	}

	private static readonly string[] Files = ["blocks.json", "block_states.json", "items-runtime.json"];
	private static readonly Dictionary<string, ClassLayout>[] Classes = new Dictionary<string, ClassLayout>[3];
	private static readonly string[] Roots = new string[3];
	private static readonly ClassTree[] Trees = new ClassTree[3];
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
				Holds = member.Holds is null
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
					string holds = member.TryGetProperty("holds", out JsonElement inner) ? inner.GetString() : null;
					members.Add(new BlockMember(at.GetInt32(), memberBytes.GetInt32(), parsedKind,
						memberName.GetString(), holds));
				}
			}

			classes[held.Name] = new ClassLayout(held.Name, size, members);
		}

		return (classes, name, build);
	}
}
