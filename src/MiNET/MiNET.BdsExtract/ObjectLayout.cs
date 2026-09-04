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
	private static (int At, int Bytes, string Field)[] Fields()
	{
		// Every member of the class, where this build keeps it, plus the method table the object
		// opens with. A hole is whatever that does not cover.
		return BlockLayout.Blocks.Leaves
			.Select(n => (n.At, n.Member.Bytes, n.Path))
			.Append((0, 8, "method table"))
			.ToArray();
	}

	/// <summary>
	///     Far enough to clear the largest BlockLegacy seen, which is a derived class carrying its
	///     own fields. Stopping short would report a short object rather than an unmeasured one.
	/// </summary>
	private const int SearchLimit = 4096;

	/// <summary>
	///     How many bytes the object at that address is, from its own class.
	///     <para>
	///         A polymorphic class writes its size into its deleting destructor, which the method
	///         table's first slot points at, so the number is the one the compiler wrote and not a
	///         guess. It is looked up once per class and answers for every object of it.
	///     </para>
	///     <para>
	///         Zero means the class states no size, which is a read that failed rather than an object
	///         of length zero, and callers say so instead of reading a length they made up.
	///     </para>
	/// </summary>
	public static int Measure(BedrockProcess process, ulong address, byte[] word)
	{
		ulong vtable = process.ReadUInt64(address, word);
		return vtable < 0x10000 ? 0 : ItemRegistry.ClassSize(process, vtable);
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
