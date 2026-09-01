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
using MiNET.Blocks;
using MiNET.Utils;

namespace MiNET.Benchmarks
{
	/// <summary>
	///     Reads one stored palette entry straight into the container the resolver takes, without
	///     building a tag tree. The entry is a compound holding a name, a states compound and a
	///     version, written little endian with fixed-width lengths, which is how a stored world writes
	///     it; anything else in the compound is walked past by size.
	///     <para>
	///         This exists to be measured against the fNbt path, not to replace it yet.
	///     </para>
	/// </summary>
	public static class SpanPaletteReader
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
		///     Walks one entry and reports nothing but where it ended: the structure work with every
		///     string and object left unmade. What a lookup keyed on the raw bytes would pay.
		/// </summary>
		public static void Skip(ReadOnlySpan<byte> span, ref int position)
		{
			byte rootType = span[position++];
			if (rootType != TagCompound) throw new FormatException($"Palette entry starts with tag {rootType}, not a compound.");

			SkipString(span, ref position);
			SkipValue(span, ref position, TagCompound);
		}

		/// <summary>
		///     Walks one entry and makes only the strings: the name and each state's name and string
		///     value. No state objects, no list, no container. Isolates what transcoding costs from
		///     what object construction costs.
		/// </summary>
		public static int ReadStringsOnly(ReadOnlySpan<byte> span, ref int position)
		{
			int chars = 0;

			byte rootType = span[position++];
			if (rootType != TagCompound) throw new FormatException($"Palette entry starts with tag {rootType}, not a compound.");
			SkipString(span, ref position);

			while (true)
			{
				byte type = span[position++];
				if (type == TagEnd) break;

				ReadOnlySpan<byte> name = ReadName(span, ref position);

				if (type == TagString && name.SequenceEqual("name"u8))
				{
					chars += ReadString(span, ref position).Length;
					continue;
				}

				if (type == TagCompound && name.SequenceEqual("states"u8))
				{
					while (true)
					{
						byte stateType = span[position++];
						if (stateType == TagEnd) break;

						chars += ReadString(span, ref position).Length;

						if (stateType == TagString) chars += ReadString(span, ref position).Length;
						else SkipValue(span, ref position, stateType);
					}

					continue;
				}

				SkipValue(span, ref position, type);
			}

			return chars;
		}

		/// <summary>
		///     Walks one entry into a container that is handed in rather than made, taking its state
		///     objects from a pool. Nothing is allocated once both are warm, which is what the resolve
		///     lookup needs and no more.
		/// </summary>
		public static void ReadInto(ReadOnlySpan<byte> span, ref int position, BlockStateContainer scratch, StatePool pool)
		{
			scratch.States.Clear();
			pool.Reset();

			byte rootType = span[position++];
			if (rootType != TagCompound) throw new FormatException($"Palette entry starts with tag {rootType}, not a compound.");
			SkipString(span, ref position);

			while (true)
			{
				byte type = span[position++];
				if (type == TagEnd) return;

				ReadOnlySpan<byte> name = ReadName(span, ref position);

				if (type == TagString && name.SequenceEqual("name"u8))
				{
					scratch.Name = ReadString(span, ref position);
					continue;
				}

				if (type == TagCompound && name.SequenceEqual("states"u8))
				{
					while (true)
					{
						byte stateType = span[position++];
						if (stateType == TagEnd) break;

						string stateName = ReadString(span, ref position);

						switch (stateType)
						{
							case TagByte:
								scratch.States.Add(pool.Byte(stateName, span[position++]));
								break;
							case TagInt:
								scratch.States.Add(pool.Int(stateName, BinaryPrimitives.ReadInt32LittleEndian(span[position..])));
								position += 4;
								break;
							case TagString:
								scratch.States.Add(pool.String(stateName, ReadString(span, ref position)));
								break;
							default:
								SkipValue(span, ref position, stateType);
								break;
						}
					}

					continue;
				}

				SkipValue(span, ref position, type);
			}
		}

		public static BlockStateContainer Read(ReadOnlySpan<byte> span, ref int position)
		{
			var container = new BlockStateContainer();

			byte rootType = span[position++];
			if (rootType != TagCompound) throw new FormatException($"Palette entry starts with tag {rootType}, not a compound.");
			SkipString(span, ref position); // the root's own name, empty in a stored entry

			while (true)
			{
				byte type = span[position++];
				if (type == TagEnd) break;

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

				SkipValue(span, ref position, type);
			}

			return container;
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
						SkipValue(span, ref position, type);
						break;
				}
			}
		}

		/// <summary>The name bytes themselves, so a known key is matched without allocating a string.</summary>
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

		private static void SkipValue(ReadOnlySpan<byte> span, ref int position, byte type)
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
					for (int i = 0; i < count; i++) SkipValue(span, ref position, itemType);
					break;
				}
				case TagCompound:
				{
					while (true)
					{
						byte childType = span[position++];
						if (childType == TagEnd) return;

						SkipString(span, ref position);
						SkipValue(span, ref position, childType);
					}
				}
				default:
					throw new FormatException($"Unknown tag type {type} in a palette entry.");
			}
		}
	}
}
