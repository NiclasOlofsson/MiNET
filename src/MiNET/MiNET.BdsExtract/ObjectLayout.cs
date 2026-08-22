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

/// <summary>A stretch of the object no field accounts for, so the hole can be counted.</summary>
public readonly record struct ByteRange(int At, int Bytes);

/// <summary>
///     What of a BlockLegacy is accounted for, and what is not.
///     The object is bigger than the fields read out of it, and the bytes in between were being
///     skipped without saying so. That is loss inside the bounds of a type the extraction claims to
///     read: the output looks complete because nothing in it mentions the parts that were never
///     looked at.
///     Size comes from the allocator rather than from the highest offset any read reaches, because
///     the highest offset read is exactly the number that would hide a hole past it. The block
///     header the allocator writes after the object is the only thing here that knows where the
///     object ends.
/// </summary>
public static class ObjectLayout
{
	/// <summary>
	///     Where each field sits, measured from the object start, so a hole is anything this does
	///     not cover. Offsets past the name are written as the name plus their own offset, which is
	///     how <see cref="MemoryLayout" /> states them.
	/// </summary>
	private static (int At, int Bytes, string Field)[] Fields() =>
	[
		(0, 8, "method table"),
		(MemoryLayout.SerializationId, 32, "serializationId"),
		(MemoryLayout.BlockComponents, 24, "components"),
		(MemoryLayout.BlockComponentIds, 24, "component ids"),
		(MemoryLayout.NameInsideLegacy + MemoryLayout.BlockTags, 24, "tags"),
		(MemoryLayout.NameInsideLegacy, HashedString.Size, "name"),
		(MemoryLayout.CreativeGroup, 32, "creativeGroup"),
		(MemoryLayout.NameInsideLegacy + MemoryLayout.Thickness, 4, "thickness"),
		(MemoryLayout.NameInsideLegacy + MemoryLayout.Translucency, 4, "translucency"),
		(MemoryLayout.NameInsideLegacy + MemoryLayout.UnnamedBytes, MemoryLayout.UnnamedByteCount, "unnamed bytes"),
		(MemoryLayout.NameInsideLegacy + MemoryLayout.MapColor, 16, "mapColor"),
		(MemoryLayout.NameInsideLegacy + MemoryLayout.TintMethod, 1, "tintMethod"),
		(MemoryLayout.NameInsideLegacy + MemoryLayout.LegacyId, 2, "legacyId"),
		(MemoryLayout.BlockProperties, 24, "properties"),
		(MemoryLayout.BlockStates, 24, "states"),
		(MemoryLayout.NameInsideLegacy + MemoryLayout.DefaultStatePointer, 8, "defaultState")
	];

	/// <summary>
	///     Far enough to clear the largest BlockLegacy seen, which is a derived class carrying its
	///     own fields. Stopping short would report a short object rather than an unmeasured one.
	/// </summary>
	private const int SearchLimit = 4096;

	/// <summary>
	///     The object's size, from the allocator's block header. Zero says the header was not
	///     found, which is reported as unmeasured rather than guessed at.
	/// </summary>
	public static int Measure(BedrockProcess process, ulong address, byte[] word)
	{
		for (int offset = 8; offset < SearchLimit; offset += 8)
		{
			if (process.ReadUInt64(address + (ulong) offset, word) >> 56 is 0x88 or 0x90) return offset;
		}
		return 0;
	}

	/// <summary>
	///     The stretches of the object no field above covers. A block that could not be measured
	///     gets null rather than an empty list: nothing unread and unable to tell are different
	///     answers and must not print the same.
	/// </summary>
	public static List<ByteRange> Holes(int size)
	{
		if (size <= 0) return null;

		var covered = new bool[size];
		foreach (var (at, bytes, _) in Fields())
		{
			for (int i = at; i < Math.Min(at + bytes, size); i++) covered[i] = true;
		}

		var holes = new List<ByteRange>();
		int start = -1;
		for (int i = 0; i < size; i++)
		{
			if (!covered[i])
			{
				if (start < 0) start = i;
			}
			else if (start >= 0)
			{
				holes.Add(new ByteRange(start, i - start));
				start = -1;
			}
		}
		if (start >= 0) holes.Add(new ByteRange(start, size - start));
		return holes;
	}
}
