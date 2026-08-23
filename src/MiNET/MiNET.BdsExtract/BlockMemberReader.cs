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
///     Reads an object as the class it is, and hands back what each member holds.
///     <para>
///         Where a member sits comes from <see cref="BlockLayout" />. What is decided here is only
///         how a value of that kind is written, which is the same on every build.
///     </para>
///     <para>
///         A member holding something with no class of its own comes back with no value, and so does
///         one whose read failed. Neither is dropped, because a row that is absent reads as a row
///         that does not exist.
///     </para>
/// </summary>
public static class BlockMemberReader
{
	/// <summary>
	///     One member as it was read: its own name, and either the value it holds or, when it holds
	///     another object, that object's members. It comes out shaped the way the class is, because
	///     that is what the data is.
	/// </summary>
	public sealed record Value(string Name, string Json, IReadOnlyList<Value> Holds);

	/// <summary>Every member of a block, read from where each was measured on this server.</summary>
	public static List<Value> Read(BedrockProcess process, ulong address, byte[] window, byte[] scratch)
	{
		int read = process.ReadClipped(address, window, Math.Min(window.Length, BlockLayout.Reach));
		return Read(process, address, window, scratch, read, BlockLayout.Blocks.Roots);
	}

	/// <summary>Every member of a state, on the same terms.</summary>
	public static List<Value> ReadState(BedrockProcess process, ulong address, byte[] window, byte[] scratch)
	{
		int read = process.ReadClipped(address, window, Math.Min(window.Length, BlockLayout.StateReach));
		return Read(process, address, window, scratch, read, BlockLayout.States.Roots);
	}

	private static List<Value> Read(BedrockProcess process, ulong address, byte[] window, byte[] scratch,
		int read, IReadOnlyList<MemberNode> nodes)
	{
		var values = new List<Value>(nodes.Count);
		foreach (MemberNode node in nodes)
		{
			if (!node.IsLeaf)
			{
				values.Add(new Value(node.Member.Name, null,
					Read(process, address, window, scratch, read, node.Holds)));
				continue;
			}

			string json = node.At + node.Member.Bytes <= read
				? Text(process, address, window, scratch, node.At, node.Member)
				: null;

			values.Add(new Value(node.Member.Name, json, []));
		}

		return values;
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
			case MemberKind.Int16:
				return BitConverter.ToInt16(window, at).ToString(CultureInfo.InvariantCulture);
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
