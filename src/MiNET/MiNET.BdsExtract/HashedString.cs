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
///     Bedrock stores many strings as a HashedString: the hash of the text, immediately followed
///     by the text itself. That pairing is what makes the whole extraction possible.
///     Searching memory for it is self-checking. A candidate is only accepted when the text that
///     follows actually hashes to the number in front of it, and arbitrary bytes do not satisfy
///     that. So the search cannot invent a block or misread a name. It can only fail to find one,
///     which is a visible shortfall rather than a silent corruption.
/// </summary>
public static class HashedString
{
	/// <summary>Layout of a HashedString, relative to its own start.</summary>
	public const int HashOffset = 0;
	public const int TextOffset = 8;
	public const int LengthOffset = 24;
	public const int CapacityOffset = 32;
	public const int Size = 40;

	/// <summary>The shortest name worth considering, "minecraft:" plus one character.</summary>
	private const int MinimumNameLength = 11;
	private const int MaximumNameLength = 64;

	/// <summary>Fowler-Noll-Vo 1, 64 bit. Multiply then exclusive-or, which is FNV-1 and not FNV-1a.</summary>
	public static ulong Fnv1(string text)
	{
		ulong hash = 0xcbf29ce484222325;
		foreach (char c in text)
		{
			hash *= 0x100000001b3;
			hash ^= (byte) c;
		}
		return hash;
	}

	/// <summary>
	///     Reads a HashedString and returns its text only when the text hashes to the stored value.
	///     <paramref name="buffer" /> holds memory already read from the target and
	///     <paramref name="at" /> is where the HashedString starts inside it.
	/// </summary>
	public static string ReadVerified(BedrockProcess process, byte[] buffer, int at, byte[] heapScratch)
	{
		ulong hash = BitConverter.ToUInt64(buffer, at + HashOffset);
		if (hash == 0) return null;

		ulong length = BitConverter.ToUInt64(buffer, at + LengthOffset);
		ulong capacity = BitConverter.ToUInt64(buffer, at + CapacityOffset);
		if (length < MinimumNameLength || length > MaximumNameLength) return null;

		// A std::string grows in sixteen byte steps and keeps short text inline. Anything else
		// is not a string header.
		if (capacity < length || (capacity + 1) % 16 != 0) return null;

		string text = capacity == 15
			? Encoding.ASCII.GetString(buffer, at + TextOffset, (int) length)
			: ReadFromHeap(process, BitConverter.ToUInt64(buffer, at + TextOffset), (int) length, heapScratch);

		if (text is null) return null;
		return Fnv1(text) == hash ? text : null;
	}

	private static string ReadFromHeap(BedrockProcess process, ulong pointer, int length, byte[] scratch)
	{
		if (pointer < 0x10000 || length > scratch.Length || !process.IsMapped(pointer)) return null;
		if (!process.TryRead(pointer, scratch, length)) return null;
		for (int i = 0; i < length; i++)
		{
			if (scratch[i] is < 0x20 or > 0x7E) return null;
		}
		return Encoding.ASCII.GetString(scratch, 0, length);
	}
}
