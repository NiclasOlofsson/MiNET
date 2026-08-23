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

using System.Text.Json.Nodes;

/// <summary>
///     Reads an object as the class it is, and hands back what each member holds.
///     <para>
///         Where a member sits comes from <see cref="BlockLayout" />. What is decided here is only
///         what kind of value a member holds, which is the same on every build.
///     </para>
///     <para>
///         The result is the object: a member holding another object is an object, a member holding
///         a value is that value. Nothing here writes JSON text. The caller serializes the model,
///         which is why the file cannot say something the model does not.
///     </para>
///     <para>
///         A member holding something with no class of its own comes back null, and so does one
///         whose read failed. Neither is dropped, because a member that is absent reads as a member
///         that does not exist.
///     </para>
/// </summary>
public static class BlockMemberReader
{
	/// <summary>
	///     What an address is, where the caller knows. A pointer to something this extraction
	///     already publishes is written as that thing's id rather than as an address: an address is
	///     true for one run of one server and names nothing a reader can look up.
	/// </summary>
	public delegate JsonNode Identify(ulong address);

	/// <summary>Every member of a block, read from where each was measured on this server.</summary>
	public static JsonObject Read(BedrockProcess process, ulong address, byte[] window, byte[] scratch,
		Identify identify = null)
	{
		int read = process.ReadClipped(address, window, Math.Min(window.Length, BlockLayout.Reach));
		return Read(process, address, window, scratch, read, BlockLayout.Blocks.Roots, identify);
	}

	/// <summary>Every member of a state, on the same terms.</summary>
	public static JsonObject ReadState(BedrockProcess process, ulong address, byte[] window, byte[] scratch,
		Identify identify = null)
	{
		int read = process.ReadClipped(address, window, Math.Min(window.Length, BlockLayout.StateReach));
		return Read(process, address, window, scratch, read, BlockLayout.States.Roots, identify);
	}

	private static JsonObject Read(BedrockProcess process, ulong address, byte[] window, byte[] scratch,
		int read, IReadOnlyList<MemberNode> nodes, Identify identify)
	{
		var members = new JsonObject();
		foreach (MemberNode node in nodes)
		{
			// The class table states every gap, with its offset and its length, so the object still
			// adds up there. A row repeating the same zero padding on every one of two thousand
			// objects says nothing the class has not already said.
			if (node.Member.Kind is MemberKind.Unknown or MemberKind.Padding) continue;

			members[node.Member.Name] = node.IsLeaf
				? node.At + node.Member.Bytes <= read
					? Node(process, address, window, scratch, node.At, node.Member, identify)
					: null
				: Read(process, address, window, scratch, read, node.Holds, identify);
		}

		return members;
	}

	/// <summary>
	///     What one position holds, as the value it is. A member that holds several numbers is an
	///     object or a list of them rather than one string standing for both, so a reader gets at
	///     the parts without parsing them back out.
	/// </summary>
	internal static JsonNode Node(BedrockProcess process, ulong address, byte[] window, byte[] scratch,
		int at, BlockMember member, Identify identify = null)
	{
		switch (member.Kind)
		{
			case MemberKind.Bool:
				return JsonValue.Create(window[at] != 0);
			case MemberKind.Bit:
				return JsonValue.Create(((window[at] >> member.Bit) & 1) != 0);
			case MemberKind.Enum8:
				return Enumerated(window[at], member);

			// A set over an enum: the names of the bits that are set. A bit with no name goes out
			// as its own index, so a value the table does not know is still stated.
			case MemberKind.Flags8:
			{
				var set = new JsonArray();
				for (int b = 0; b < 8; b++)
				{
					if ((window[at] >> b & 1) == 0) continue;
					set.Add(BlockMembers.Value(member.Enum, b) ?? (JsonNode) b);
				}

				return set;
			}
			case MemberKind.UInt16:
				return JsonValue.Create((long) BitConverter.ToUInt16(window, at));
			case MemberKind.Int16:
				return JsonValue.Create((long) BitConverter.ToInt16(window, at));
			case MemberKind.Enum32:
				return Enumerated(BitConverter.ToInt32(window, at), member);
			case MemberKind.Int32:
				return JsonValue.Create((long) BitConverter.ToInt32(window, at));
			case MemberKind.UInt32:
				return JsonValue.Create((long) BitConverter.ToUInt32(window, at));
			case MemberKind.UInt64:
				return JsonValue.Create(BitConverter.ToUInt64(window, at));
			// The bits that are set, by their own index. A flag word means the traits it carries and
			// the list says them: 0x200 is bit 9, the flag exactly the fourteen button blocks hold.
			// The value reads back from the list, so nothing is lost by not also printing it as a
			// number every reader has to parse before it means anything.
			case MemberKind.Bits64:
			{
				ulong bits = BitConverter.ToUInt64(window, at);
				var set = new JsonArray();
				for (int b = 0; b < 64; b++)
				{
					if ((bits >> b & 1) == 0) continue;
					set.Add(BlockMembers.Value(member.Enum, b) ?? (JsonNode) b);
				}

				return set;
			}
			case MemberKind.Float:
				return Sentinels.Number(member.Name, BitConverter.ToSingle(window, at));
			case MemberKind.Text:
			{
				string text = ItemRegistry.StdString(process, window, at, scratch);
				return text is null ? null : JsonValue.Create(text);
			}
			case MemberKind.Hashed:
			{
				// The hash is what proves the text, so the length is only a cheap pre-filter and it
				// must not be the thing that rejects a real value. The default minimum is eleven,
				// "minecraft:" plus a character, which is right for a namespaced name and wrong for
				// a bare one: copper_axe is ten, and 446 items lost their bare name to that gate.
				string text = HashedString.ReadVerified(process, window, at, scratch, 1, 96);
				return text is null ? null : JsonValue.Create(text);
			}
			case MemberKind.Colour:
			{
				var channels = new float[4];
				for (int c = 0; c < 4; c++) channels[c] = BitConverter.ToSingle(window, at + (c * 4));
				return JsonValue.Create(Hex(channels));
			}
			case MemberKind.Range:
				return new JsonObject
				{
					["min"] = JsonValue.Create((long) BitConverter.ToInt32(window, at)),
					["max"] = JsonValue.Create((long) BitConverter.ToInt32(window, at + 4))
				};
			case MemberKind.Version:
			{
				// A BaseGameVersion: a semantic version of three numbers and two flags, then two
				// packed pointers to text, then a flag of its own.
				return new JsonObject
				{
					["version"] = JsonValue.Create($"{BitConverter.ToUInt16(window, at)}."
												+ $"{BitConverter.ToUInt16(window, at + 2)}."
												+ $"{BitConverter.ToUInt16(window, at + 4)}"),
					["parsed"] = JsonValue.Create(window[at + 6] != 0),
					["anyVersion"] = JsonValue.Create(window[at + 7] != 0),
					["neverCompatible"] = JsonValue.Create(window[at + 24] != 0)
				};
			}
			case MemberKind.Vector3:
			{
				var axes = new JsonArray();
				for (int c = 0; c < 3; c++)
				{
					axes.Add(Sentinels.Number(member.Name, BitConverter.ToSingle(window, at + (c * 4))));
				}

				return axes;
			}
			case MemberKind.Box:
			{
				var corners = new JsonArray();
				for (int c = 0; c < 6; c++)
				{
					corners.Add(Sentinels.Number(member.Name, BitConverter.ToSingle(window, at + (c * 4))));
				}

				return corners;
			}
			// A container with no class of its own. Its bytes go out as they are, under a name that
			// says they were not decoded, because null is a different fact: null says the member
			// holds nothing, and these mostly hold something. Eleven of the block class's sixteen
			// such members point at real objects on every block, and while they all wrote null the
			// four that are genuinely empty were indistinguishable from them.
			// A pointer to a component. The state holds these already past the method table, which
			// sits eight bytes behind what the pointer names, so the class starts at the pointer
			// itself. Lectern's collision box reads its own AABB there and nothing else does.
			case MemberKind.Component:
			{
				// Null means one thing only: the object holds no component here. A reference that
				// names no class, and a read that failed, are different facts and say so, because a
				// reader cannot tell three causes apart from one null.
				ulong pointer = BitConverter.ToUInt64(window, at);
				if (pointer < 0x10000) return null;

				if (BlockMembers.Any(member.Holds) is not { } held)
				{
					return new JsonObject
					{
						["pointer"] = $"0x{pointer:X}",
						["undeclared"] = member.Holds
					};
				}

				var body = new byte[Math.Max(held.Size, 16)];
				if (!process.TryRead(pointer, body, body.Length))
				{
					return new JsonObject { ["pointer"] = $"0x{pointer:X}", ["unreadable"] = held.Size };
				}

				return Held(process, pointer, body, held, 0, scratch);
			}

			case MemberKind.Container:
				return Container(process, window, at, member, identify);

			// Bytes nothing declares. They travel out as themselves so the member list adds up to
			// the class and the hole is countable rather than absent.
			case MemberKind.Unknown:
				return new JsonObject
				{
					["unknown"] = member.Bytes,
					["at"] = member.At,
					["raw"] = Convert.ToHexString(window, at, member.Bytes)
				};

			default:
				return null;
		}
	}

	/// <summary>
	///     A container with no class of its own, said as what it structurally is rather than as
	///     null. Null is a different fact: it says the member holds nothing, and most of these hold
	///     something. Eleven of the block class's sixteen such members point at a real object on
	///     every block, and while they all wrote null the four that are genuinely empty could not be
	///     told apart from them.
	///     The size states the shape. Eight bytes is a pointer. Twenty four is an MSVC vector, three
	///     pointers of begin, end and capacity, and it is only called one when those three hold in
	///     order, so a member that merely happens to be that wide is not dressed up as a list.
	///     Anything else, and anything that fails its own check, goes out as the bytes it is.
	/// </summary>
	private static JsonNode Container(BedrockProcess process, byte[] window, int at, BlockMember member,
		Identify identify)
	{
		// No "unread" and no offset. The eight bytes of a pointer ARE read, so calling the member
		// unread is untrue: what nothing followed is the object at the other end. The offset and the
		// size are the class's to state, and it states them once instead of on every row.
		var stated = new JsonObject();

		if (member.Bytes == 8)
		{
			// A pointer holding nothing is null, said the same way everywhere. Wrapping that in an
			// object with an offset and a null inside it states the same fact in a second shape,
			// and two shapes for one fact is what a reader has to write code to tell apart.
			ulong pointer = BitConverter.ToUInt64(window, at);
			if (pointer == 0) return null;
			if (identify?.Invoke(pointer) is { } named) return named;
			stated["pointer"] = $"0x{pointer:X}";
			return stated;
		}

		// An unordered_map: a load factor, then the list its entries live in, then the buckets.
		// Only the list is walked, because the buckets index it and hold nothing of their own.
		if (member.Bytes == 64 && member.Entries is not null && BlockMembers.Any(member.Entries) is { } shape)
		{
			var word = new byte[8];
			ulong head = BitConverter.ToUInt64(window, at + 8);
			long count = (long) BitConverter.ToUInt64(window, at + 16);
			var pairs = new JsonArray();
			if (head >= 0x10000 && count > 0 && count <= 256)
			{
				var node = new byte[shape.Size];
				ulong walk = process.ReadUInt64(head, word);
				for (long i = 0; i < count && walk >= 0x10000 && walk != head; i++)
				{
					if (!process.TryRead(walk, node, node.Length)) break;
					pairs.Add(Held(process, walk, node, shape, 0, new byte[256]));
					walk = BitConverter.ToUInt64(node, 0);
				}
			}

			return pairs;
		}

		if (member.Bytes == 24)
		{
			ulong begin = BitConverter.ToUInt64(window, at);
			ulong end = BitConverter.ToUInt64(window, at + 8);
			ulong capacity = BitConverter.ToUInt64(window, at + 16);

			// A vector of pointers to things that are published elsewhere is the list of their ids.
			// Every element has to resolve or be empty, so a vector that merely divides by eight is
			// not dressed up as a list of ids: arbitrary bytes do not all land on known objects.
			// An empty slot stays in place as null rather than being skipped, because position is
			// what the table means. A block's permutations are the full product of its properties'
			// bit widths, so acacia_button holds twelve states and four empty slots for the four
			// combinations that are not legal, and dropping them would move every state after them.
			if (identify is not null && end > begin && (end - begin) % 8 == 0 && end - begin <= 8 * 4096)
			{
				var elements = new byte[end - begin];
				if (process.TryRead(begin, elements, elements.Length))
				{
					var ids = new JsonArray();
					int found = 0;
					for (int e = 0; e + 8 <= elements.Length; e += 8)
					{
						ulong element = BitConverter.ToUInt64(elements, e);
						if (element == 0) { ids.Add(null); continue; }
						if (identify(element) is not { } one) { ids = null; break; }
						ids.Add(one);
						found++;
					}

					if (ids is not null && found > 0) return ids;
				}
			}

			// An empty vector is an empty list. Saying that with three null pointers and a zero
			// length describes the container rather than what it holds, which is nothing.
			if (begin == end && capacity >= end) return new JsonArray();

			// A vector that says what it holds is read as those elements. The count has to divide
			// exactly, so a wrong element size yields nothing rather than fragments.
			if (member.Elements is not null && end > begin && BlockMembers.Any(member.Elements) is { } each
				&& each.Size > 0 && (end - begin) % (ulong) each.Size == 0 && end - begin <= 4096)
			{
				var body = new byte[end - begin];
				if (process.TryRead(begin, body, body.Length))
				{
					var list = new JsonArray();
					for (int e = 0; e + each.Size <= body.Length; e += each.Size)
					{
						list.Add(Held(process, begin, body, each, e, new byte[256]));
					}

					return list;
				}
			}

			if (end > begin && capacity >= end)
			{
				stated["vector"] = new JsonObject
				{
					["begin"] = $"0x{begin:X}",
					["end"] = $"0x{end:X}",
					["bytes"] = (long) (end - begin)
				};
				return stated;
			}
		}

		// A shared pointer is the object and its control block, so two pointers wide. Stated as the
		// pointer it is rather than as sixteen bytes of hex.
		if (member.Bytes == 16)
		{
			// The object it points at is the fact. A shared pointer holding nothing is null, whatever
			// its control block happens to contain.
			ulong held = BitConverter.ToUInt64(window, at);
			if (held == 0) return null;
			stated["pointer"] = $"0x{held:X}";
			return stated;
		}

		stated["raw"] = Convert.ToHexString(window, at, member.Bytes);
		return stated;
	}


	/// <summary>
	///     An enum value with the name the class gives it beside it. Both, because the number is
	///     what was read and the name is what it means: a name alone throws the value away and a
	///     value alone says nothing. A member with no enum stated is just its number.
	/// </summary>
	private static JsonNode Enumerated(long value, BlockMember member)
	{
		if (member.Enum is null) return JsonValue.Create(value);
		return new JsonObject
		{
			["value"] = value,
			["name"] = BlockMembers.Value(member.Enum, value)
		};
	}

	/// <summary>One class's members read at a position, and the members of whatever they hold.</summary>
	internal static JsonObject Held(BedrockProcess process, ulong address, byte[] window,
		ClassLayout held, int lead, byte[] scratch)
	{
		var fields = new JsonObject();
		foreach (BlockMember member in held.Members)
		{
			int at = lead + member.At;
			if (member.Kind == MemberKind.Unknown || at + member.Bytes > window.Length) continue;

			fields[member.Name] = member.Holds is not null && member.Kind == MemberKind.Container
				? Held(process, address, window, BlockMembers.Any(member.Holds), at, scratch)
				: Node(process, address, window, scratch, at, member);
		}

		return fields;
	}

	private static string Hex(float[] rgba)
	{
		static int Channel(float value) => Math.Clamp((int) Math.Round(value * 255f), 0, 255);
		return $"#{Channel(rgba[0]):X2}{Channel(rgba[1]):X2}{Channel(rgba[2]):X2}{Channel(rgba[3]):X2}";
	}
}
