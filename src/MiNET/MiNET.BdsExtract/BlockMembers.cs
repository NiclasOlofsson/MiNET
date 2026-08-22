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
///     One member of a class: where it starts inside that class, how wide it is, and what it is
///     called. A member holding another object names the class it holds, and its own position is
///     the only thing that can move: what is inside it is that class's business.
/// </summary>
public readonly record struct BlockMember(int At, int Bytes, MemberKind Kind, string Name, string Holds = null);

/// <summary>One class: how big it is and what it holds, in the order it declares them.</summary>
public sealed record ClassLayout(string Name, int Size, IReadOnlyList<BlockMember> Members);

/// <summary>
///     Every member of the block class, read out of the reference in Assets rather than written
///     here.
///     <para>
///         The reference is an extraction of one build read at the offsets that build's own class
///         declaration states, and it carries the member list it was read with: each member's name,
///         position, width and kind, the class size, and which build it is for. Holding a second
///         copy of that in source would be a number this tool asserts, and the first thing to go
///         stale.
///     </para>
///     <para>
///         The names are the class's own with the m dropped, so nothing here is a name this tool
///         invented. Members holding a container are listed too, with their position and size and no
///         value, so the part of the object nothing reads stays countable rather than disappearing.
///     </para>
/// </summary>
public static class BlockMembers
{
	/// <summary>Every class the block file states, by name, and which of them a block is.</summary>
	private static Dictionary<string, ClassLayout> _blockClasses;
	private static string _blockRoot;
	private static Version _build;

	/// <summary>Every class the state file states, by name, and which of them a state is.</summary>
	private static Dictionary<string, ClassLayout> _stateClasses;
	private static string _stateRoot;

	/// <summary>The class a block is.</summary>
	public static ClassLayout Block
	{
		get
		{
			LoadBlocks();
			return Class(_blockClasses, _blockRoot);
		}
	}

	/// <summary>The class a state is.</summary>
	public static ClassLayout State
	{
		get
		{
			LoadStates();
			return Class(_stateClasses, _stateRoot);
		}
	}

	/// <summary>The build the reference was read on, and the only one its positions are stated for.</summary>
	public static Version Build
	{
		get
		{
			LoadBlocks();
			return _build;
		}
	}

	/// <summary>The class a member holds, from the same file that member came from.</summary>
	public static ClassLayout Held(string name, bool state)
	{
		if (state) LoadStates();
		else LoadBlocks();
		return Class(state ? _stateClasses : _blockClasses, name);
	}

	private static ClassLayout Class(Dictionary<string, ClassLayout> classes, string name)
	{
		if (name is null) return null;
		return classes.TryGetValue(name, out ClassLayout held)
			? held
			: throw new InvalidOperationException($"the reference names a class {name} and then does not state it");
	}

	private static void LoadStates()
	{
		if (_stateClasses is not null) return;
		(_stateClasses, _stateRoot, _) = LoadClasses("block_states.json");
	}

	private static void LoadBlocks()
	{
		if (_blockClasses is not null) return;
		(_blockClasses, _blockRoot, _build) = LoadClasses("blocks.json");
	}

	/// <summary>
	///     The class table a reference states: every class it knows with its size and its members,
	///     and which of them the file's objects are.
	/// </summary>
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
