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

using System.Globalization;

/// <summary>
///     Reads every member of a block object and writes each one as the value it holds.
///     <para>
///         Where the members are comes from <see cref="BlockLayout" />, which is the published class
///         layout on the build it was published for and a measured position on every other. What is
///         read here is only how a value of that kind is written, which does not change between
///         builds.
///     </para>
///     <para>
///         A member holding a container is not read, and says so with its offset and size. A member
///         whose read fails says that too. Neither is dropped, because a row that is absent reads as
///         a row that does not exist.
///     </para>
/// </summary>
public static class BlockMemberReader
{
	/// <summary>One member's value, or why there is not one.</summary>
	public readonly record struct Value(string Name, int At, int Bytes, MemberKind Kind, string Json);

	/// <summary>Every member of one block, in the order the class declares them.</summary>
	public static List<Value> Read(BedrockProcess process, ulong address, byte[] window, byte[] scratch)
	{
		int read = process.ReadClipped(address, window, Math.Min(window.Length, BlockLayout.Reach));
		return Read(process, address, window, scratch, read, false, BlockLayout.Has, BlockLayout.At);
	}

	/// <summary>Every member of one state, in the order the class declares them.</summary>
	public static List<Value> ReadState(BedrockProcess process, ulong address, byte[] window, byte[] scratch)
	{
		int read = process.ReadClipped(address, window, Math.Min(window.Length, BlockLayout.StateReach));
		return Read(process, address, window, scratch, read, true, BlockLayout.StateHas, BlockLayout.StateAt);
	}

	private static List<Value> Read(BedrockProcess process, ulong address, byte[] window, byte[] scratch,
		int read, bool state, Func<string, bool> has, Func<string, int> at)
	{
		IReadOnlyList<BlockMember> members = state ? BlockLayout.StateMembers : BlockLayout.Members;
		var values = new List<Value>(members.Count);
		foreach (BlockMember member in members)
		{
			// A member nothing placed on this build sits nowhere, and says so with a position of
			// -1 and no value. Reading it from where it sat on another build is the one mistake
			// the output cannot show.
			if (!has(member.Name))
			{
				values.Add(new Value(member.Name, -1, member.Bytes, member.Kind, null));
				continue;
			}

			int position = at(member.Name);
			string json = position + member.Bytes > read
				? null
				: Held(process, address, window, scratch, position, member, state);
			values.Add(new Value(member.Name, position, member.Bytes, member.Kind, json));
		}

		return values;
	}

	/// <summary>
	///     What a member holds. A member that is another object is read as that object, from its own
	///     class, and comes out as one: its members are inside it, not spelled into its parent's
	///     names. Nothing inside is placed by searching, because nothing inside can move on its own.
	/// </summary>
	private static string Held(BedrockProcess process, ulong address, byte[] window, byte[] scratch,
		int at, BlockMember member, bool state)
	{
		if (member.Kind != MemberKind.Container || member.Holds is null)
		{
			return Text(process, address, window, scratch, at, member);
		}

		ClassLayout held = BlockMembers.Held(member.Holds, state);
		var text = new System.Text.StringBuilder("{ ");
		for (int m = 0; m < held.Members.Count; m++)
		{
			BlockMember inner = held.Members[m];
			text.Append(m == 0 ? "" : ", ");
			text.Append($"\"{inner.Name}\": ");
			text.Append(Held(process, address, window, scratch, at + inner.At, inner, state) ?? "null");
		}

		return text.Append(" }").ToString();
	}

	/// <summary>What one position holds, written the way that kind of value is written.</summary>
	internal static string Text(BedrockProcess process, ulong address, byte[] window, byte[] scratch,
		int at, BlockMember member)
	{
		switch (member.Kind)
		{
			case MemberKind.Bool:
				return window[at] != 0 ? "true" : "false";
			case MemberKind.Enum8:
				return window[at].ToString(CultureInfo.InvariantCulture);
			case MemberKind.UInt16:
				return BitConverter.ToUInt16(window, at).ToString(CultureInfo.InvariantCulture);
			case MemberKind.Enum32:
			case MemberKind.Int32:
				return BitConverter.ToInt32(window, at).ToString(CultureInfo.InvariantCulture);
			case MemberKind.UInt64:
				return BitConverter.ToUInt64(window, at).ToString(CultureInfo.InvariantCulture);
			case MemberKind.Bits64:
				return $"\"0x{BitConverter.ToUInt64(window, at):X16}\"";
			case MemberKind.Float:
				return Sentinels.Number(member.Name, BitConverter.ToSingle(window, at));
			case MemberKind.Text:
			{
				string text = ItemRegistry.StdString(process, window, at, scratch);
				return text is null ? "null" : Quote(text);
			}
			case MemberKind.Hashed:
			{
				string text = HashedString.ReadVerified(process, window, at, scratch);
				return text is null ? "null" : Quote(text);
			}
			case MemberKind.Colour:
			{
				var channels = new float[4];
				for (int c = 0; c < 4; c++) channels[c] = BitConverter.ToSingle(window, at + (c * 4));
				return Quote(Hex(channels));
			}
			case MemberKind.Range:
				return $"{{ \"min\": {BitConverter.ToInt32(window, at)}, \"max\": {BitConverter.ToInt32(window, at + 4)} }}";
			case MemberKind.Version:
			{
				// A BaseGameVersion: a semantic version of three numbers and two flags, then two
				// packed pointers to text, then a flag of its own.
				string version = $"{BitConverter.ToUInt16(window, at)}.{BitConverter.ToUInt16(window, at + 2)}.{BitConverter.ToUInt16(window, at + 4)}";
				return $"{{ \"version\": {Quote(version)}, \"parsed\": {(window[at + 6] != 0 ? "true" : "false")}, "
					+ $"\"anyVersion\": {(window[at + 7] != 0 ? "true" : "false")}, "
					+ $"\"neverCompatible\": {(window[at + 24] != 0 ? "true" : "false")} }}";
			}
			case MemberKind.Box:
			{
				var corners = new float[6];
				for (int c = 0; c < 6; c++) corners[c] = BitConverter.ToSingle(window, at + (c * 4));
				return "[" + string.Join(", ", corners.Select(v => Sentinels.Number(member.Name, v))) + "]";
			}
			default:
				return null;
		}
	}

	private static string Hex(float[] rgba)
	{
		static int Channel(float value) => Math.Clamp((int) Math.Round(value * 255f), 0, 255);
		return $"#{Channel(rgba[0]):X2}{Channel(rgba[1]):X2}{Channel(rgba[2]):X2}{Channel(rgba[3]):X2}";
	}

	private static string Quote(string text)
	{
		var quoted = new System.Text.StringBuilder("\"");
		foreach (char c in text)
		{
			if (c is '"' or '\\') quoted.Append('\\').Append(c);
			else if (c < ' ') quoted.Append($"\\u{(int) c:X4}");
			else quoted.Append(c);
		}
		return quoted.Append('"').ToString();
	}
}
