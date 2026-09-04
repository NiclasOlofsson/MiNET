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
using System.Text;
using MiNET.Utils;

namespace MiNET.Worlds
{
	/// <summary>
	///     Reads one stored palette entry straight into the container the palette is keyed by,
	///     without building a tag tree. A section's entries are read, looked up and thrown away, so
	///     everything fNbt builds on the way is garbage the moment the runtime id is known: measured
	///     at 623ns and 1.4KB per entry through fNbt against 128ns and 361 bytes through this.
	///     <para>
	///         Reads the stored form only: little endian, fixed-width lengths, states in a compound
	///         beside the name. Anything else in the compound is stepped over by size, and a shape
	///         this cannot read throws, which the caller answers by parsing the same bytes with fNbt.
	///     </para>
	/// </summary>
	public static class PaletteEntryReader
	{
		private const byte TagEnd = 0;
		private const byte TagByte = 1;
		private const byte TagShort = 2;
		private const byte TagInt = 3;
		private const byte TagLong = 4;
		private const byte TagFloat = 5;
		private const byte TagDouble = 6;
		private const byte TagByteArray = 7;
		private const byte TagString = 8;
		private const byte TagList = 9;
		private const byte TagCompound = 10;
		private const byte TagIntArray = 11;
		private const byte TagLongArray = 12;

		/// <summary>
		///     Fills <paramref name="container" /> from the entry at <paramref name="position" /> and
		///     leaves the position on the byte after it. The container is handed in rather than made so
		///     a caller reading a whole palette can reuse one.
		/// </summary>
		public static void Read(ReadOnlySpan<byte> span, ref int position, BlockStateContainer container)
		{
			container.Name = null;
			container.States.Clear();

			byte rootType = span[position++];
			if (rootType != TagCompound) throw new FormatException($"Palette entry starts with tag {rootType}, not a compound.");

			SkipString(span, ref position); // the root's own name, empty in a stored entry

			while (true)
			{
				byte type = span[position++];
				if (type == TagEnd) return;

				ReadOnlySpan<byte> name = ReadName(span, ref position);

				if (type == TagString && name.SequenceEqual("name"u8))
				{
					container.Name = ReadString(span, ref position);
					continue;
				}

				if (type == TagCompound && name.SequenceEqual("states"u8))
				{
					ReadStates(span, ref position, container);
					continue;
				}

				// "version" lands here, and anything else a version we have not met writes beside it.
				// Neither takes part in identity, so both are stepped over.
				Skip(span, ref position, type);
			}
		}

		private static void ReadStates(ReadOnlySpan<byte> span, ref int position, BlockStateContainer container)
		{
			while (true)
			{
				byte type = span[position++];
				if (type == TagEnd) return;

				string name = ReadString(span, ref position);

				switch (type)
				{
					case TagByte:
						container.States.Add(new BlockStateByte {Name = name, Value = span[position++]});
						break;
					case TagInt:
						container.States.Add(new BlockStateInt {Name = name, Value = BinaryPrimitives.ReadInt32LittleEndian(span[position..])});
						position += 4;
						break;
					case TagString:
						container.States.Add(new BlockStateString {Name = name, Value = ReadString(span, ref position)});
						break;
					default:
						// The palette holds these three types and nothing else, so a fourth is a
						// shape we do not know. Stepping over it silently would resolve the entry to
						// the wrong block; throwing sends the whole entry to fNbt instead.
						throw new FormatException($"Palette state {name} has tag type {type}, which is not a block state type.");
				}
			}
		}

		/// <summary>The name bytes themselves, so a known key is recognised without allocating.</summary>
		private static ReadOnlySpan<byte> ReadName(ReadOnlySpan<byte> span, ref int position)
		{
			int length = BinaryPrimitives.ReadUInt16LittleEndian(span[position..]);
			position += 2;

			ReadOnlySpan<byte> name = span.Slice(position, length);
			position += length;

			return name;
		}

		private static string ReadString(ReadOnlySpan<byte> span, ref int position)
		{
			int length = BinaryPrimitives.ReadUInt16LittleEndian(span[position..]);
			position += 2;

			string value = Encoding.UTF8.GetString(span.Slice(position, length));
			position += length;

			return value;
		}

		private static void SkipString(ReadOnlySpan<byte> span, ref int position)
		{
			int length = BinaryPrimitives.ReadUInt16LittleEndian(span[position..]);
			position += 2 + length;
		}

		private static void Skip(ReadOnlySpan<byte> span, ref int position, byte type)
		{
			switch (type)
			{
				case TagByte:
					position += 1;
					break;
				case TagShort:
					position += 2;
					break;
				case TagInt:
				case TagFloat:
					position += 4;
					break;
				case TagLong:
				case TagDouble:
					position += 8;
					break;
				case TagString:
					SkipString(span, ref position);
					break;
				case TagByteArray:
					position += 4 + BinaryPrimitives.ReadInt32LittleEndian(span[position..]);
					break;
				case TagIntArray:
					position += 4 + BinaryPrimitives.ReadInt32LittleEndian(span[position..]) * 4;
					break;
				case TagLongArray:
					position += 4 + BinaryPrimitives.ReadInt32LittleEndian(span[position..]) * 8;
					break;
				case TagList:
				{
					byte itemType = span[position++];
					int count = BinaryPrimitives.ReadInt32LittleEndian(span[position..]);
					position += 4;
					for (int i = 0; i < count; i++) Skip(span, ref position, itemType);
					break;
				}
				case TagCompound:
				{
					while (true)
					{
						byte childType = span[position++];
						if (childType == TagEnd) return;

						SkipString(span, ref position);
						Skip(span, ref position, childType);
					}
				}
				default:
					throw new FormatException($"Unknown tag type {type} in a palette entry.");
			}
		}
	}
}
