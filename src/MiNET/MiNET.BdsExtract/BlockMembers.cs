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

/// <summary>One member of the block class: where it starts, how wide it is, and what it is called.</summary>
public readonly record struct BlockMember(int At, int Bytes, MemberKind Kind, string Name);

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
	private static BlockMember[] _all;
	private static int _classSize;
	private static Version _build;

	/// <summary>Every member, in the order the class declares them.</summary>
	public static BlockMember[] All
	{
		get
		{
			Load();
			return _all;
		}
	}

	/// <summary>The class, before whatever a derived block class adds after it.</summary>
	public static int ClassSize
	{
		get
		{
			Load();
			return _classSize;
		}
	}

	/// <summary>The build the reference was read on, and the only one its positions are stated for.</summary>
	public static Version Build
	{
		get
		{
			Load();
			return _build;
		}
	}

	private static void Load()
	{
		if (_all is not null) return;
		_all = [];

		string path = Path.Combine(WorldConfig.AssetsDirectory(), "reference", "blocks.json");
		if (!File.Exists(path))
		{
			Console.Error.WriteLine($"no reference at {path}; the block class has no member list, so nothing can be read from it");
			return;
		}

		using JsonDocument document = JsonDocument.Parse(File.ReadAllText(path));
		JsonElement root = document.RootElement;

		if (root.TryGetProperty("blockClassSize", out JsonElement size)) _classSize = size.GetInt32();
		if (root.TryGetProperty("layoutPublishedFor", out JsonElement build)
			&& Version.TryParse(build.GetString() ?? "", out Version parsed))
		{
			_build = parsed;
		}

		if (!root.TryGetProperty("members", out JsonElement members)) return;

		var all = new List<BlockMember>();
		foreach (JsonElement member in members.EnumerateArray())
		{
			if (!member.TryGetProperty("name", out JsonElement name)) continue;
			if (!member.TryGetProperty("at", out JsonElement at)) continue;
			if (!member.TryGetProperty("bytes", out JsonElement bytes)) continue;
			if (!member.TryGetProperty("kind", out JsonElement kind)) continue;
			if (!Enum.TryParse(kind.GetString(), out MemberKind parsedKind)) continue;
			all.Add(new BlockMember(at.GetInt32(), bytes.GetInt32(), parsedKind, name.GetString()));
		}

		_all = all.ToArray();
	}
}
