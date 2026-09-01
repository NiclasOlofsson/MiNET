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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2026 Niclas Olofsson.
// All Rights Reserved.

#endregion

using System;
using System.Buffers.Binary;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MiNET.Utils
{
	/// <summary>
	///     The 64-bit hash block identity is compared on. Multiply-fold over the raw bytes: one
	///     64x64 multiply per eight bytes, which is a single instruction on x64, so a block name
	///     costs a handful of them.
	///     <para>
	///         Deterministic by construction. Nothing here is seeded by the process the way
	///         <see cref="string.GetHashCode()" /> is, so the same block yields the same number on
	///         every run and every machine.
	///     </para>
	///     <para>
	///         This is NOT the block network hash. That one is FNV-1a 32 over a canonical NBT
	///         document, it goes on the wire, and it must match what the client computes. This one
	///         never leaves memory.
	///     </para>
	/// </summary>
	public static class StateHash
	{
		private const ulong P1 = 0xa0761d6478bd642f;
		private const ulong P2 = 0xe7037ed1a0b428db;
		private const ulong P3 = 0x8ebc6af09c88c6e3;

		/// <summary>
		///     Folds two values into one. The high and low halves of the 128-bit product both depend
		///     on every input bit, so xoring them mixes far better than a shift-and-add chain.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ulong Mix(ulong a, ulong b)
		{
			ulong high = Math.BigMul(a, b, out ulong low);

			return high ^ low;
		}

		/// <summary>
		///     The string's own UTF-16 bytes, reinterpreted rather than converted: no encoding pass
		///     and no allocation.
		/// </summary>
		public static ulong OfString(string value)
		{
			return value == null ? 0 : OfBytes(MemoryMarshal.AsBytes(value.AsSpan()));
		}

		public static ulong OfBytes(ReadOnlySpan<byte> data)
		{
			ulong hash = P1 ^ (ulong) data.Length;

			while (data.Length >= 8)
			{
				hash = Mix(hash ^ BinaryPrimitives.ReadUInt64LittleEndian(data), P2);
				data = data[8..];
			}

			if (data.Length >= 4)
			{
				hash = Mix(hash ^ BinaryPrimitives.ReadUInt32LittleEndian(data), P3);
				data = data[4..];
			}

			for (int i = 0; i < data.Length; i++)
			{
				hash = Mix(hash ^ data[i], P3);
			}

			return Mix(hash, P1);
		}
	}
}
