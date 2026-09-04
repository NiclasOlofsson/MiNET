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

using System.Text;

namespace MiNET.BdsExtract;

/// <summary>
///     One component: the name it goes by in a pack, the field the server calls it internally, and
///     what kind of thing owns it.
/// </summary>
public readonly record struct ComponentName(string Owner, string Alias, string Name);

/// <summary>
///     The component names, read out of the descriptors the server builds for them.
///     A component's DATA is not here and does not need to be: the values a block's components
///     carry are the compiled fields <see cref="BlockRegistry" /> already reads, friction and
///     destroy time and light emission, and the shape of each component is in the JSON schemas BDS
///     writes about itself. What is nowhere else is the vocabulary, which name means which field.
///     A descriptor is three fields in a row: the internal alias as a std::string, the namespaced
///     name thirty two bytes on, then a pointer and a length to the owner's name. Two adjacent
///     strings on their own match a great deal of hash table that is not this, so the owner is what
///     separates them, and it checks itself: the length stored beside the pointer has to equal the
///     length of the NUL terminated text it points at, which arbitrary bytes do not manage.
///     An alias appearing twice is not a fault. One internal field can feed two public names, and
///     both are reported: block_light_filter is both minecraft:block_light_filter and
///     minecraft:block_light_absorption, which is a rename in progress with both ends live.
/// </summary>
public static class ComponentNames
{
	private const int NameOffset = 32;        // the namespaced name, one std::string on
	private const int OwnerOffset = 64;       // then a pointer and a length
	private const int StringSize = 32;
	private const int RecordReach = 96;
	private const int MaximumText = 128;
	private const int MinimumOwner = 3;
	private const int MaximumOwner = 64;
	private const string Namespace = "minecraft:";

	public static List<ComponentName> Read(BedrockProcess process)
	{
		var window = new byte[8 * 1024 * 1024];
		var scratch = new byte[MaximumText];
		var found = new HashSet<ComponentName>();

		foreach (var region in process.Regions)
		{
			for (ulong at = region.Base; at < region.End; at += (ulong) window.Length - RecordReach)
			{
				int length = (int) Math.Min((ulong) window.Length, region.End - at);
				if (length < RecordReach || !process.TryRead(at, window, length)) continue;

				for (int i = 0; i + RecordReach <= length; i += 8)
				{
					string name = StdString(process, window, i + NameOffset, scratch);
					if (name is null || !name.StartsWith(Namespace, StringComparison.Ordinal)) continue;

					string owner = Owner(process, window, i + OwnerOffset, scratch);
					if (owner is null || !owner.StartsWith(Namespace, StringComparison.Ordinal)) continue;

					string alias = StdString(process, window, i, scratch);
					if (alias is not null) found.Add(new ComponentName(owner, alias, name));
				}
			}
		}
		return found
			.OrderBy(c => c.Owner, StringComparer.Ordinal)
			.ThenBy(c => c.Alias, StringComparer.Ordinal)
			.ThenBy(c => c.Name, StringComparer.Ordinal)
			.ToList();
	}

	/// <summary>
	///     A pointer and the length of what it points at, which is how the owner is stored.
	///     Reading the length rather than scanning to the terminator is what makes this a check
	///     instead of a guess: the text has to end exactly where the length says it does.
	/// </summary>
	private static string Owner(BedrockProcess process, byte[] window, int at, byte[] scratch)
	{
		ulong pointer = BitConverter.ToUInt64(window, at);
		ulong length = BitConverter.ToUInt64(window, at + 8);
		if (pointer < 0x10000 || length is < MinimumOwner or > MaximumOwner) return null;
		if (!process.IsMapped(pointer)) return null;

		int span = (int) length + 1;
		if (!process.TryRead(pointer, scratch, span) || scratch[length] != 0) return null;
		return Printable(scratch, (int) length) ? Encoding.ASCII.GetString(scratch, 0, (int) length) : null;
	}

	/// <summary>An MSVC std::string that is already inside the window being swept.</summary>
	private static string StdString(BedrockProcess process, byte[] window, int at, byte[] scratch)
	{
		ulong length = BitConverter.ToUInt64(window, at + 16);
		ulong capacity = BitConverter.ToUInt64(window, at + 24);
		if (length == 0 || length > capacity || length > MaximumText) return null;

		if (capacity == 15)
		{
			return Printable(window, at, (int) length)
				? Encoding.ASCII.GetString(window, at, (int) length) : null;
		}
		if (capacity > 4096 || (capacity + 1) % 16 != 0) return null;

		ulong pointer = BitConverter.ToUInt64(window, at);
		if (pointer < 0x10000 || !process.IsMapped(pointer)) return null;
		if (!process.TryRead(pointer, scratch, (int) length)) return null;
		return Printable(scratch, 0, (int) length) ? Encoding.ASCII.GetString(scratch, 0, (int) length) : null;
	}

	private static bool Printable(byte[] buffer, int length)
	{
		return Printable(buffer, 0, length);
	}

	private static bool Printable(byte[] buffer, int at, int length)
	{
		if (at < 0 || at + length > buffer.Length) return false;
		for (int i = at; i < at + length; i++)
		{
			if (buffer[i] is < 0x20 or > 0x7E) return false;
		}
		return true;
	}

	/// <summary>The size of a std::string, so the caller can reason about the record's reach.</summary>
	public const int Stride = StringSize;
}
