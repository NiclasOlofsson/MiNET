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

using System.Text.Json.Nodes;
using MiNET.BdsExtract.Binary;

namespace MiNET.BdsExtract;

/// <summary>One component a block carries, with its value where the type is known.</summary>
public sealed class BlockComponent
{
	/// <summary>
	///     The server's own number for this component, from the array beside the pointer vector.
	///     This is the identity. Everything else here is an attempt to put a name to it, and the
	///     number is correct whether or not the name is known.
	/// </summary>
	public int Id { get; init; }

	/// <summary>The identifier, or null when the type could not be named.</summary>
	public string Name { get; init; }

	/// <summary>
	///     A component carrying no data at all, recorded as a bare id in the set beside the map
	///     rather than as an object. The storage keeps the two apart, mComponents and
	///     mStatelessComponents, so a block holding one is saying it has the component and that the
	///     component has nothing to say.
	/// </summary>
	public bool Stateless { get; init; }

	/// <summary>
	///     What the component holds: its decoded fields under their own names, and the bytes nothing
	///     decoded as unknown members in their place.
	/// </summary>
	public JsonObject Holds { get; init; }

	/// <summary>The object's measured size, which is what makes an unnamed component countable.</summary>
	public int Size { get; init; }

}

/// <summary>
///     The components a block actually carries, read from the vector on BlockType.
///     Each is filed under a number the server hands out the first time that component class is
///     asked for, so the number is a fact about this run and the class behind it is the fact about
///     the game. <see cref="BinaryFacts.LiveIds" /> reads which is which out of the server's own
///     code, and the layout of each class comes from the reference like every other layout, so
///     nothing here decides what a component is by looking at what it happens to hold.
///     A component whose class the reference does not state is still emitted, as its measured size
///     and its bytes, because a component dropped for being unrecognised makes the block look like
///     it has fewer than it does.
/// </summary>
public static class ComponentReader
{
	private const int VectorReach = 4096;
	private const int MaximumSize = 512;

	/// <summary>Where a component keeps its first value, past the method table.</summary>
	private const int FirstField = 8;

	/// <summary>
	///     What class each component id is, read out of the server's own code by the binary phase.
	///     A registration number is per run, so the number is never a name: what it stands for on
	///     THIS run is, and both travel out together.
	/// </summary>
	public static IReadOnlyDictionary<int, string> Names { get; private set; } = new Dictionary<int, string>();

	/// <summary>
	///     What the executable states about its own classes, which is where <see cref="Names" />
	///     comes from and what the node's method table is confirmed against. Null before
	///     <see cref="Read" /> has run, and then nothing is confirmed and nothing is claimed.
	/// </summary>
	private static BinaryFacts _facts;

	/// <summary>
	///     The components one object holds, read the same way whether it is a block or a state.
	///     A state's are the same kinds under the same ids, so reading them as names beside the
	///     block's decoded values would leave the state saying less about the same thing.
	/// </summary>
	public static List<BlockComponent> Carried(BedrockProcess process, ulong storage, byte[] word)
	{
		var list = new List<BlockComponent>();
		foreach ((int id, ulong at) in Instances(process, storage, word))
		{
			string name = Names.GetValueOrDefault(id);
			int size = Size(process, at, word);
			string disagreement = Confirm(process, id, name, at, word);
			list.Add(new BlockComponent
			{
				Id = id,
				Name = disagreement is null ? name : null,
				Size = size,
				Holds = disagreement is null
					? Decode(process, name, at, size, word)
					: new JsonObject { ["unread"] = disagreement }
			});
		}

		foreach (int id in Stateless(process, storage, word))
		{
			list.Add(new BlockComponent
			{
				Id = id,
				Name = Names.GetValueOrDefault(id),
				Stateless = true
			});
		}

		return list;
	}

	public static Dictionary<string, List<BlockComponent>> Read(BedrockProcess process,
		IReadOnlyList<BlockProperties> blocks, BinaryFacts facts)
	{
		var word = new byte[8];

		// Which class each id is, from the server rather than from a table in here. The number is a
		// per-run registration, so a list of them written down last week describes a heap that no
		// longer exists: 7 was redstone conductivity in one such table and is the redstone component
		// on this run, and 26 was leashable and is the connection component.
		_facts = facts;
		Names = facts.LiveIds;

		var result = new Dictionary<string, List<BlockComponent>>(StringComparer.Ordinal);
		var sized = new Dictionary<int, int>();
		foreach (var block in blocks)
		{
			var list = new List<BlockComponent>();
			foreach (var (id, at) in Instances(process,
						block.Address + (ulong) BlockLayout.At("components"), word))
			{
				// The class states its own size, and every instance of one class is one size, so it
				// is measured once per id rather than once per block.
				if (!sized.TryGetValue(id, out int size)) sized[id] = size = Size(process, at, word);
				string name = Names.GetValueOrDefault(id);
				string disagreement = Confirm(process, id, name, at, word);
				list.Add(new BlockComponent
				{
					Id = id,
					Name = disagreement is null ? name : null,
					Size = size,
					Holds = disagreement is null
						? Decode(process, name, at, size, word)
						: new JsonObject { ["unread"] = disagreement }
				});
			}
			// The ones with no data are components too, so they go in the same list saying what they
			// are: an id, its class, and nothing else, because there is nothing else in the server
			// either.
			foreach (int id in Stateless(process, block.Address + (ulong) BlockLayout.At("components"), word))
			{
				list.Add(new BlockComponent { Id = id, Name = Names.GetValueOrDefault(id), Stateless = true });
			}

			result[block.Name] = list;
		}
		return result;
	}

	/// <summary>
	///     Whether the object an id points at is the class that id names, or the reason it is not.
	///     <para>
	///         The id and the method table are two independent statements about the same object.
	///         The id comes from a static the binary phase located from the class's own signature
	///         literal; the leading word of the object is the storage node's table, which the same
	///         phase read from that class's construction path. A block component is built inside a
	///         node that holds it, so the table is the node's rather than the component's, and a
	///         node holding eight bytes or less is the same node whatever it holds: eighteen classes
	///         share one table on this build, and any of them is a confirmation.
	///     </para>
	///     <para>
	///         Where the two disagree the entry is left unread saying which said what. Reading it
	///         under the id's name would be publishing one statement and dropping the other.
	///     </para>
	///     <para>
	///         A class whose node table the binary phase did not settle is a different thing
	///         entirely: there is one statement and nothing to hold it against, so the entry is read
	///         as its id names and the missing confirmation is counted and named. Calling that a
	///         disagreement would throw away every value the id does account for.
	///     </para>
	/// </summary>
	private static string Confirm(BedrockProcess process, int id, string name, ulong at, byte[] word)
	{
		if (_facts is null) return null;
		if (name is null)
		{
			Tally("(no class for the id)").Unnamed++;
			return null;
		}

		NodeTally tally = Tally(name);
		if (!_facts.NodeTable(name, out ulong expected))
		{
			tally.Unconfirmed++;
			tally.Note ??= $"{name}: the binary settles no node table for it, so the id stands on its own";
			return null;
		}

		ulong table = process.ReadUInt64(at, word);
		IReadOnlyList<string> names = _facts.ClassesAt(table);
		if (table == expected || names.Contains(name, StringComparer.Ordinal))
		{
			tally.Agrees++;
			return null;
		}

		tally.Differs++;
		string states = names.Count == 0
			? $"no class in the binary (0x{table:X})"
			: string.Join(", ", names);
		string reason = $"id {id} names {name}, the node's method table names {states}";
		tally.Note ??= reason;
		return reason;
	}

	/// <summary>How the objects under one class's id answered the class's own node table.</summary>
	private sealed class NodeTally
	{
		public int Agrees;
		public int Differs;
		public int Unconfirmed;
		public int Unnamed;
		public string Note;
	}

	private static readonly Dictionary<string, NodeTally> _nodeTables = new(StringComparer.Ordinal);

	private static NodeTally Tally(string kind)
	{
		if (!_nodeTables.TryGetValue(kind, out NodeTally tally)) _nodeTables[kind] = tally = new NodeTally();
		return tally;
	}

	/// <summary>
	///     What every block component object said about its own class, per kind. It is printed after
	///     the blocks AND the states have been read, because both carry the same kinds and a count
	///     taken halfway through would be a count of half of them.
	/// </summary>
	public static void ReportNodeTables()
	{
		if (_nodeTables.Count == 0) return;

		int agrees = _nodeTables.Values.Sum(t => t.Agrees);
		int differs = _nodeTables.Values.Sum(t => t.Differs);
		int unconfirmed = _nodeTables.Values.Sum(t => t.Unconfirmed);
		int unnamed = _nodeTables.Values.Sum(t => t.Unnamed);
		Console.WriteLine($"block component tables: {agrees + differs + unconfirmed + unnamed:N0} objects across "
			+ $"{_nodeTables.Count(t => t.Value.Unnamed == 0)} kinds, {agrees:N0} whose node table names the class "
			+ $"their id names, {differs:N0} that disagree, {unconfirmed:N0} whose class the binary settles no table for, "
			+ $"{unnamed:N0} whose id names no class");
		foreach ((string kind, NodeTally tally) in _nodeTables.OrderBy(t => t.Key, StringComparer.Ordinal))
		{
			Console.WriteLine($"  {kind,-52} {tally.Agrees,6:N0} agree, {tally.Differs,4:N0} disagree"
				+ (tally.Unconfirmed > 0 ? $", {tally.Unconfirmed:N0} with no table to confirm against" : "")
				+ (tally.Unnamed > 0 ? $", {tally.Unnamed:N0} with no class for the id" : ""));
			if (tally.Note is not null) Console.WriteLine($"      {tally.Note}");
		}
	}

	/// <summary>
	///     The components on one block, each with the id the server files it under.
	///     Two vectors run in step: pointers at <see cref="MemoryLayout.BlockComponents" /> and one
	///     sixteen bit id each at <see cref="MemoryLayout.BlockComponentIds" />. They are required
	///     to be the same length, which is what says they are the same list, and a block where they
	///     disagree is skipped rather than paired up by position and hoped for.
	/// </summary>
	/// <summary>
	///     The ids of the components a block holds that carry no data. They sit in a flat_set of
	///     their own beside the map of the ones that do, and reading only the map drops them: 45
	///     blocks hold 50 of them, and a block saying it is replaceable by holding a bare id is
	///     saying it just as much as one holding an object.
	/// </summary>
	internal static List<int> Stateless(BedrockProcess process, ulong storage, byte[] word)
	{
		var ids = new List<int>();
		ulong begin = process.ReadUInt64(storage + StatelessSet, word);
		ulong end = process.ReadUInt64(storage + StatelessSet + 8, word);
		if (begin < 0x10000 || end <= begin || end - begin > VectorReach || (end - begin) % 2 != 0) return ids;

		var raw = new byte[end - begin];
		if (!process.TryRead(begin, raw, raw.Length)) return ids;
		for (int i = 0; i + 2 <= raw.Length; i += 2) ids.Add(BitConverter.ToInt16(raw, i));
		return ids;
	}

	/// <summary>Where the set of dataless component ids sits inside the storage.</summary>
	private const int StatelessSet = 48;

	internal static List<(int Id, ulong At)> Instances(BedrockProcess process, ulong storage, byte[] word)
	{
		var held = new List<(int, ulong)>();

		// The keys are first and the values second, which is what the flat_map declares. The ids
		// are two bytes each and the pointers eight, and requiring both counts to agree is what
		// says the two vectors are the same list.
		ulong idBegin = process.ReadUInt64(storage, word);
		ulong idEnd = process.ReadUInt64(storage + 8, word);
		ulong begin = process.ReadUInt64(storage + 24, word);
		ulong end = process.ReadUInt64(storage + 32, word);
		if (begin < 0x10000 || end <= begin || (end - begin) % 8 != 0) return held;
		if (end - begin > VectorReach || !process.IsMapped(begin)) return held;

		int count = (int) ((end - begin) / 8);
		if (idBegin < 0x10000 || idEnd - idBegin != (ulong) (count * 2)) return held;
		if (!process.IsMapped(idBegin)) return held;

		var ids = new byte[count * 2];
		if (!process.TryRead(idBegin, ids, ids.Length)) return held;

		for (int i = 0; i < count; i++)
		{
			ulong component = process.ReadUInt64(begin + (ulong) (i * 8), word);
			if (component < 0x10000 || !process.IsMapped(component)) continue;
			held.Add((BitConverter.ToUInt16(ids, i * 2), component));
		}
		return held;
	}

	/// <summary>
	///     What one component holds, read as the class it is. The reference states that class the
	///     same way it states the block's, so a component is not decoded by a switch on its name
	///     here: the layout lives beside every other layout and a new build is a reference edit.
	///     The instance is a method table followed by the component, so the class sits eight bytes
	///     in and the table is stated separately. Bytes no member of the class covers become
	///     unknown members in their place, so the instance adds up to its measured size.
	/// </summary>
	private static JsonObject Decode(BedrockProcess process, string name, ulong at, int size, byte[] word)
	{
		// The method table is not emitted. It is how the size and the class are found, which makes
		// it a fact about this heap rather than about the block, and it is the same pointer on
		// twelve different components anyway.
		var fields = new JsonObject();
		var covered = new List<(int At, int Bytes)> { (0, FirstField) };

		// A class the reference states under the component's own class name, or under the pack name
		// it used to be looked up by. Either way the layout comes from the file.
		ClassLayout held = BlockMembers.Component(name) ?? BlockMembers.Any(name);
		if (held is null)
		{
			Leftovers(process, at, size, covered, fields, false);
			return fields;
		}

		int span = Math.Min(size > 0 ? size : MaximumSize, MaximumSize);
		var window = new byte[span];
		int read = process.ReadClipped(at, window, span);
		Members(process, at, window, read, held, FirstField, fields, covered);

		// What the class the server compiled has past the members the reference declares. The size
		// comes from the object's own deleting destructor, so it is the class rather than the block
		// the allocator handed out, and a stretch of it that no member covers is a hole in the
		// layout rather than slack: the geometry component measures 264 bytes and 60 are declared.
		// A stretch narrower than the object's own pointer alignment cannot be a member, so that one
		// is the compiler rounding the class up and is not called unknown.
		int tail = size - FirstField - held.Size;
		if (size > 0 && size <= MaximumSize && tail >= 8)
		{
			// After whatever gaps the class itself declares, because two entries under one key is a
			// value lost and nothing in the file says it happened.
			int number = 1;
			while (fields.ContainsKey($"unknown{number}")) number++;

			var body = new byte[tail];
			bool got = process.TryRead(at + (ulong) (FirstField + held.Size), body, tail);
			string raw = got ? Convert.ToHexString(body) : null;
			fields[$"unknown{number}"] = new JsonObject
			{
				["unknown"] = tail,
				["at"] = held.Size,
				["raw"] = raw
			};

			// Counted like every other hole, so the class the reference states short is countable in
			// one place rather than only in the rows that happen to carry it.
			BlockMemberReader.Unknown(new BlockMember(held.Size, tail, MemberKind.Unknown,
				$"unknown{number}", Class: name ?? held.Name), raw ?? "unread");
		}

		return fields;
	}

	/// <summary>
	///     One class's members at a position, and the members of anything they hold. Every one is
	///     recorded as covered whether or not its read succeeded, because the bytes belong to that
	///     member either way and counting them twice would hide a hole somewhere else.
	/// </summary>
	private static void Members(BedrockProcess process, ulong address, byte[] window, int read,
		ClassLayout held, int lead, JsonObject fields, List<(int At, int Bytes)> covered)
	{
		var scratch = new byte[256];
		foreach (BlockMember member in held.Members)
		{
			int at = lead + member.At;
			covered.Add((at, member.Bytes));

			// A gap the tiler could not call alignment is a member nobody has named, and its bytes
			// are what says so: the culling component's leading two bytes hold the same occlusion
			// shape handle the finalized one does, and a class table entry saying "two bytes, here"
			// would never have shown that. Padding stays out of the rows; the class table states it
			// once and the padding span report counts every reading of it.
			if (member.Kind is MemberKind.Padding)
			{
				if (at + member.Bytes <= read) BlockMemberReader.Node(process, address, window, scratch, at, member);
				continue;
			}

			if (!member.Emit) continue;
			if (at + member.Bytes > read) { fields[member.Name] = null; continue; }

			if (member.Holds is not null)
			{
				// From whichever file declares it, the same way every other nested class is found.
				// The transformation component is stated by the state file and carried by blocks,
				// so looking only in the block file threw on a class the reference does state.
				var inner = new JsonObject();
				Members(process, address, window, read, BlockMembers.Any(member.Holds), at, inner, covered);
				fields[member.Name] = inner;
				continue;
			}

			fields[member.Name] = BlockMemberReader.Node(process, address, window, scratch, at, member);
		}
	}

	/// <summary>
	///     The bytes of a component that no member of its class covers, as members in their place.
	///     Without them the instance does not add up to its measured size, and a reader cannot tell
	///     a component that was read whole from one where three fields were picked out of forty.
	/// </summary>
	private static void Leftovers(BedrockProcess process, ulong at, int size,
		List<(int At, int Bytes)> covered, JsonObject fields, bool declared)
	{
		if (size <= 0 || size > MaximumSize) return;

		var claimed = new bool[size];
		foreach ((int from, int length) in covered)
		{
			for (int i = from; i < from + length && i < size; i++) claimed[i] = true;
		}

		var body = new byte[size];
		if (!process.TryRead(at, body, size)) return;

		int unknown = 0;
		for (int i = 0; i < size; i++)
		{
			if (claimed[i]) continue;
			int end = i;
			while (end < size && !claimed[end]) end++;
			// Padding where a class declares the object, because then every byte a member does not
			// cover is one the compiler inserted for alignment. Unknown only where nothing declares
			// the object at all, which is the honest word for bytes nobody has accounted for.
			fields[$"{(declared ? "padding" : "unknown")}{++unknown}"] = new JsonObject
			{
				[declared ? "padding" : "unknown"] = end - i,
				["at"] = i,
				["raw"] = Convert.ToHexString(body, i, end - i)
			};
			i = end;
		}
	}

	/// <summary>
	///     How big a component instance is, from the class rather than from the heap. The method
	///     table's first slot is the deleting destructor, and the compiler writes the allocation
	///     size into it as an immediate before the call to operator delete, so this is the size the
	///     type actually has.
	///     It replaced a scan that walked forward to the allocator's next block header. That number
	///     is the block handed out, not the object: a component of a method table and one float is
	///     twelve bytes and the scan reported twenty four, so twelve bytes of somebody else's slack
	///     were being read and published as part of the component.
	/// </summary>
	private static int Size(BedrockProcess process, ulong at, byte[] word)
	{
		ulong vtable = process.ReadUInt64(at, word);
		return vtable < 0x10000 ? 0 : ItemRegistry.ClassSize(process, vtable);
	}

}
