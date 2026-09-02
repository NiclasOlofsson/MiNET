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

	/// <summary>
	///     Whether the per-process view travels out with the rest of the object, for the whole run:
	///     padding bytes and heap addresses. Off, a padding member is read and counted but not
	///     written, and an address is written as what it led to rather than as a number; on
	///     (--debug), padding comes out as its bytes on every row of every class and every address
	///     is written. An address is true for one start of one server and moves on the next, so
	///     with them in the rows a diff between two runs is mostly addresses that changed with
	///     nothing behind them changed.
	/// </summary>
	internal static bool Debug;

	/// <summary>
	///     An address on a row: written under --debug, otherwise <paramref name="otherwise" /> where
	///     the row would say nothing else about the pointer, and nothing where a key beside it
	///     already says what the pointer led to.
	/// </summary>
	internal static JsonObject Pointer(JsonObject stated, ulong address, string otherwise = null)
	{
		if (Debug) stated["pointer"] = $"0x{address:X}";
		else if (otherwise is not null) stated["pointer"] = otherwise;
		return stated;
	}

	/// <summary>What every instance of one padding or unknown member held, so the claim can be settled.</summary>
	private sealed class PaddingSpan
	{
		public string Class;
		public string Name;
		public int At;
		public int Bytes;
		public int Zero;
		public int NotZero;

		/// <summary>The distinct non zero readings, which is what a person needs to rule on it.</summary>
		public readonly SortedSet<string> Values = new(StringComparer.Ordinal);

		/// <summary>How many instances held each distinct reading, so a value that is the same on
		/// every carrier is told apart from one that differs on every one.</summary>
		public readonly Dictionary<string, int> Carriers = new(StringComparer.Ordinal);
	}

	private static readonly Dictionary<string, PaddingSpan> Paddings = new(StringComparer.Ordinal);
	private static readonly Dictionary<string, PaddingSpan> Unknowns = new(StringComparer.Ordinal);

	/// <summary>
	///     Counts one reading of one padding member. Zero on every instance is what padding looks
	///     like; anything else is a member nobody has named yet, and only the count can tell them
	///     apart.
	/// </summary>
	private static void Padding(BlockMember member, string raw) => Span(Paddings, member, raw);

	/// <summary>
	///     Counts one reading of one unknown member. The bytes of a hole are the only thing that
	///     can say what is in it, and a row states one instance: the tally states every one, so a
	///     span that holds the same value on every carrier is told apart from one that does not.
	/// </summary>
	internal static void Unknown(BlockMember member, string raw) => Span(Unknowns, member, raw);

	/// <summary>
	///     Counts one reading of one gap out of a window, where the caller has the bytes but is not
	///     writing them. A gap past what the read reached is counted as unread rather than as zero.
	/// </summary>
	internal static void Tally(BlockMember member, byte[] window, int at, int read)
	{
		Dictionary<string, PaddingSpan> into = member.Kind == MemberKind.Unknown ? Unknowns : Paddings;
		Span(into, member, at >= 0 && at + member.Bytes <= Math.Min(read, window.Length)
			? Convert.ToHexString(window, at, member.Bytes)
			: "unread");
	}

	private static void Span(Dictionary<string, PaddingSpan> into, BlockMember member, string raw)
	{
		string key = $"{member.Class ?? "?"}.{member.Name}@{member.At}";
		if (!into.TryGetValue(key, out PaddingSpan span))
		{
			into[key] = span = new PaddingSpan
			{
				Class = member.Class ?? "?", Name = member.Name, At = member.At, Bytes = member.Bytes
			};
		}

		span.Carriers[raw] = span.Carriers.GetValueOrDefault(raw) + 1;

		bool zero = true;
		foreach (char c in raw)
		{
			if (c == '0') continue;
			zero = false;
			break;
		}

		if (zero)
		{
			span.Zero++;
			return;
		}

		span.NotZero++;
		if (span.Values.Count < 8) span.Values.Add(raw);
	}

	/// <summary>
	///     Every padding member of every class, with how many instances held nothing and how many
	///     held something. A span that is non zero on any instance is not padding: it stays
	///     declared and its bytes stay in the rows, and the ruling on what it is belongs to a
	///     person, not to this.
	/// </summary>
	public static void ReportPadding()
	{
		Report("padding spans", Paddings, "NOT PADDING");
		Report("unknown spans", Unknowns, "holds");
	}

	private static void Report(string what, Dictionary<string, PaddingSpan> from, string carrying)
	{
		if (from.Count == 0) return;

		List<PaddingSpan> spans = from.Values
			.OrderBy(p => p.Class, StringComparer.Ordinal)
			.ThenBy(p => p.At)
			.ToList();
		Console.WriteLine($"{what}: {spans.Count} declared across "
			+ $"{spans.Select(p => p.Class).Distinct(StringComparer.Ordinal).Count()} classes, "
			+ $"{spans.Sum(p => p.Zero):N0} readings all zero, {spans.Sum(p => p.NotZero):N0} not, "
			+ $"{spans.Count(p => p.NotZero > 0)} span(s) carrying something on at least one instance");
		foreach (PaddingSpan span in spans)
		{
			string line = $"  {span.Class}.{span.Name,-10} at +{span.At,-4} {span.Bytes,3} bytes, "
				+ $"{span.Zero:N0} zero / {span.NotZero:N0} not";
			if (span.NotZero > 0) line += $"; {carrying}, distinct: {string.Join(", ", span.Values)}";
			Console.WriteLine(line);

			// How the readings divide. One value on every carrier is a constant the class holds;
			// as many values as carriers is either a per instance field or somebody else's slack,
			// and the only way to tell them apart is to see the split.
			List<KeyValuePair<string, int>> top = span.Carriers.OrderByDescending(v => v.Value)
				.ThenBy(v => v.Key, StringComparer.Ordinal).ToList();
			Console.WriteLine($"      {span.Carriers.Count:N0} distinct reading(s) over "
				+ $"{span.Zero + span.NotZero:N0} carrier(s): "
				+ string.Join(", ", top.Take(6).Select(v => $"{v.Key} x{v.Value:N0}"))
				+ (top.Count > 6 ? $", and {top.Count - 6:N0} more" : ""));
		}
	}

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
			// adds up there. Padding stays out of the rows unless the run was asked to show it, and
			// then it is the bytes, because seeing what a padding span holds is the only reason to
			// show one.
			if (!node.Member.Emit) continue;
			if (node.Member.Kind is MemberKind.Unknown or MemberKind.Padding)
			{
				// Counted here whether or not it is written. A gap that no path tallies is a gap
				// nobody can rule on, and the rows are not where that ruling is made.
				Tally(node.Member, window, node.At, read);
				if (node.Member.Kind is MemberKind.Unknown || !Debug) continue;
			}

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
		// An optional that holds nothing has never had its value written, so the bytes there belong
		// to whoever had the allocation before. Reading them as the member is how 888 blocks came
		// out playing a note they do not play, and the flag beside them said so the whole time.
		// A variant states its discriminator in the same place, and there nought is an alternative
		// rather than an absent value, so the gate is not read as one.
		if (member.Gate != 0 && member.Kind != MemberKind.Variant
			&& at + member.Gate < window.Length && window[at + member.Gate] == 0) return null;

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
			case MemberKind.UInt8:
				return JsonValue.Create((long) window[at]);
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
			// A Bedrock::StaticOptimizedString. The characters sit at the low forty eight bits of
			// the word; bit fifty five says the length is the eight bytes before them rather than
			// the seven bits under that flag, which is the same packing the compiled Molang
			// expression keeps its source text in and the same bit the class's own destructor tests
			// before it frees. The text is the value; the address is a fact about this heap.
			case MemberKind.Packed:
			{
				ulong packed = BitConverter.ToUInt64(window, at);
				ulong text = packed & 0x0000FFFFFFFFFFFFul;
				if (text < 0x10000) return null;

				ulong length;
				if ((packed & (1ul << 55)) != 0)
				{
					var size = new byte[8];
					if (!process.TryRead(text - 8, size, size.Length)) return null;
					length = BitConverter.ToUInt64(size, 0);
				}
				else
				{
					length = (packed >> 48) & 0x7f;
				}

				if (length == 0) return JsonValue.Create(string.Empty);
				if (length > (ulong) scratch.Length) return null;
				return process.ReadClipped(text, scratch, (int) length) == (int) length
					? JsonValue.Create(System.Text.Encoding.UTF8.GetString(scratch, 0, (int) length))
					: null;
			}

			// A std::variant: the storage at the member's own offset and the alternative it holds
			// in the byte the member states as its gate, which is where MSVC puts it. The first
			// alternative is read as the class the member names, or as a bool where it names none;
			// any other alternative is stated as its number and the storage nobody read, because a
			// number alone would say the variant is empty and it is not.
			case MemberKind.Variant:
			{
				int which = at + member.Gate < window.Length ? window[at + member.Gate] : -1;
				if (which != 0) return new JsonObject { ["which"] = which, ["unread"] = member.Gate };
				if (member.Holds is null) return new JsonObject { ["which"] = which, ["value"] = window[at] != 0 };
				if (BlockMembers.Any(member.Holds) is not { } first)
				{
					return new JsonObject { ["which"] = which, ["undeclared"] = member.Holds };
				}

				return new JsonObject { ["which"] = which, ["value"] = Held(process, address, window, first, at, scratch) };
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
					return Pointer(new JsonObject { ["undeclared"] = member.Holds }, pointer);
				}

				var body = new byte[Math.Max(held.Size, 16)];
				if (!process.TryRead(pointer, body, body.Length))
				{
					return Pointer(new JsonObject { ["unreadable"] = held.Size }, pointer);
				}

				return Held(process, pointer, body, held, 0, scratch);
			}

			// A shared pointer to a declared class. Only the object matters: the control block
			// behind it is the allocator's business, and a pointer holding nothing is null.
			case MemberKind.Shared:
			{
				ulong pointer = BitConverter.ToUInt64(window, at);
				if (pointer < 0x10000) return null;
				if (BlockMembers.Any(member.Holds) is not { } shared)
				{
					return new JsonObject { ["undeclared"] = member.Holds };
				}

				var held2 = new byte[shared.Size];
				if (!process.TryRead(pointer, held2, held2.Length))
				{
					return new JsonObject { ["unread"] = shared.Size };
				}

				return Held(process, pointer, held2, shared, 0, scratch);
			}

			case MemberKind.Container:
				return Container(process, window, at, member, identify);

			// An ItemDescriptor. Its own pointer sits eight bytes in, following the same shape
			// DescriptorNames already walks for a vector of these: the pointer names the descriptor
			// object, and which kind of descriptor it is decides where the text sits. One names an
			// item and keeps a std::string eight bytes in; one names a tag and keeps a Molang
			// expression there instead, through a shared pointer, whose source text is the query
			// the wire sends. The item name is tried first and only a verified string is taken, so
			// a tag descriptor cannot come out as an item with a nonsense name.
			case MemberKind.Descriptor:
			{
				ulong pointer = BitConverter.ToUInt64(window, at + 8);
				if (pointer < 0x10000) return null;
				var target = new byte[64];
				if (!process.TryRead(pointer, target, target.Length)) return null;

				string name = ItemRegistry.StdString(process, target, 8, scratch);
				if (name is { Length: > 0 }) return new JsonObject { ["name"] = name };

				ulong held = BitConverter.ToUInt64(target, 8);
				ulong expression = held >= 0x10000 ? process.ReadUInt64(held, scratch) : 0;
				string query = MolangSource(process, expression, scratch);
				return query is null ? null : new JsonObject { ["tags"] = query };
			}

			// A Molang ExpressionNode: the variant's payload first and which alternative it holds
			// behind it. Nought is the compiled expression, and the text it was parsed from is what
			// the wire carries. One is a constant the parser folded, which is a number and travels
			// out as one.
			case MemberKind.Expression:
			{
				ulong payload = BitConverter.ToUInt64(window, at);
				// Which alternative is one byte, which is how the server's own reader takes it. The
				// seven behind it are whatever the allocation held before: a block permutation's
				// condition reads "sion" in them and came out null while they were part of the
				// number.
				long which = window[at + 8];
				if (which == 1) return Sentinels.Number(member.Name, BitConverter.ToSingle(window, at));
				if (which != 0 || payload < 0x10000) return null;

				string source = MolangSource(process, payload, scratch);
				return source is null
					? Pointer(new JsonObject { ["unread"] = "the expression states no source text" }, payload)
					: JsonValue.Create(source);
			}

			// Bytes the compiler inserted so the next member lands on its alignment. Every reading is
			// tallied, because "padding" is a claim about content and nothing but the content can
			// settle it: a stretch that is non zero on any instance is carrying something, and the
			// run says so. The bytes themselves stay out of the rows unless the run was asked to
			// show them; the class table states the gap once.
			case MemberKind.Padding:
			{
				string raw = Convert.ToHexString(window, at, member.Bytes);
				Padding(member, raw);
				return Debug ? new JsonObject { ["raw"] = raw } : null;
			}

			// Bytes nothing declares. They travel out as themselves so the member list adds up to
			// the class and the hole is countable rather than absent.
			case MemberKind.Unknown:
			{
				string raw = Convert.ToHexString(window, at, member.Bytes);
				Unknown(member, raw);
				return new JsonObject
				{
					["unknown"] = member.Bytes,
					["at"] = member.At,
					["raw"] = raw
				};
			}

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
			return Pointer(stated, pointer, "unfollowed");
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

			// A vector of a four byte enum. The member names the enum, so the elements are read as
			// that enum and not as a byte count: leaving them behind the pointer would have said
			// eighty eight bytes and nothing about the twenty two values inside them. The count has
			// to divide exactly, so a member that merely happens to be a vector yields nothing.
			if (member.Enum is not null && end > begin && capacity >= end
				&& (end - begin) % 4 == 0 && end - begin <= 4096)
			{
				var values = new byte[end - begin];
				if (process.TryRead(begin, values, values.Length))
				{
					var named = new JsonArray();
					for (int e = 0; e + 4 <= values.Length; e += 4)
					{
						named.Add(Enumerated(BitConverter.ToInt32(values, e), member));
					}

					return named;
				}
			}

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
				var vector = new JsonObject();
				if (Debug)
				{
					vector["begin"] = $"0x{begin:X}";
					vector["end"] = $"0x{end:X}";
				}
				vector["bytes"] = (long) (end - begin);
				stated["vector"] = vector;
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
			return Pointer(stated, held, "unfollowed");
		}

		stated["raw"] = Convert.ToHexString(window, at, member.Bytes);
		return stated;
	}

	/// <summary>Where a compiled Molang expression keeps the text it was parsed from.</summary>
	private const int MolangSourceInExpression = 0x18;

	/// <summary>
	///     The source text a compiled Molang expression holds, read the way the server's own
	///     getExpressionString reads it. The value at that position is a pointer with the length
	///     packed into the two bytes above it: bit fifty five says the text is long enough to carry
	///     a size of its own, which sits in the eight bytes before the characters, and below that
	///     the length is the seven bits under that flag.
	///     The read proves itself. The characters have to be printable and end in a nul exactly
	///     where the stated length puts it, which arbitrary bytes do not.
	/// </summary>
	private static string MolangSource(BedrockProcess process, ulong expression, byte[] scratch)
	{
		if (expression < 0x10000) return null;

		var word = new byte[8];
		if (!process.TryRead(expression + MolangSourceInExpression, word, word.Length)) return null;

		ulong packed = BitConverter.ToUInt64(word, 0);
		ulong text = packed & 0x0000FFFFFFFFFFFFul;
		if (text < 0x10000) return null;

		ulong length;
		if ((packed & (1ul << 55)) != 0)
		{
			if (!process.TryRead(text - 8, word, word.Length)) return null;
			length = BitConverter.ToUInt64(word, 0);
		}
		else
		{
			length = (packed >> 48) & 0x7f;
		}

		if (length == 0 || length + 1 > (ulong) scratch.Length) return null;
		if (process.ReadClipped(text, scratch, (int) length + 1) != (int) length + 1) return null;
		if (scratch[length] != 0) return null;

		for (var i = 0; i < (int) length; i++)
		{
			if (scratch[i] is < 0x20 or > 0x7e) return null;
		}

		return System.Text.Encoding.ASCII.GetString(scratch, 0, (int) length);
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
			if (at + member.Bytes > window.Length) continue;

			if (member.Kind == MemberKind.Unknown)
			{
				Tally(member, window, at, window.Length);
				continue;
			}

			// Read and checked, and refused a place in the file. Every one of these is an address,
			// which is true for one run of one server and churns the output on every other.
			if (!member.Emit) continue;

			JsonNode value = member.Holds is not null && member.Kind == MemberKind.Container
				? Held(process, address, window, BlockMembers.Any(member.Holds), at, scratch)
				: Node(process, address, window, scratch, at, member);

			// Padding is read and tallied above, and stays out of the file unless the run was asked
			// to show it, in which case it is the bytes. Never a key with nothing in it.
			if (member.Kind == MemberKind.Padding && !Debug) continue;

			fields[member.Name] = value;
		}

		return fields;
	}

	private static string Hex(float[] rgba)
	{
		static int Channel(float value) => Math.Clamp((int) Math.Round(value * 255f), 0, 255);
		return $"#{Channel(rgba[0]):X2}{Channel(rgba[1]):X2}{Channel(rgba[2]):X2}{Channel(rgba[3]):X2}";
	}
}
