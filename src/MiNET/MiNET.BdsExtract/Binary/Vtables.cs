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
using System.Collections.Generic;
using System.Linq;
using Iced.Intel;

namespace MiNET.BdsExtract.Binary;

/// <summary>
///     One class's method table as the image states it.
/// </summary>
/// <param name="VtableRva">The table's RVA, or 0 when the construction path did not resolve one.</param>
/// <param name="Witnesses">How many of the class's own functions store this table into the first word of a new object.</param>
/// <param name="Candidates">Every table the class's functions stored that way, so a disagreement is countable.</param>
/// <param name="Size">
///     The size the deleting destructor hands the deallocator, or 0 when it states none.
///     <paramref name="SizeCandidates" /> holds every size it hands out, in the order it hands them.
/// </param>
/// <param name="Networked">Slot 3's verdict, or null when its body is neither stub; <paramref name="NetworkedBytes" /> then holds what was there.</param>
/// <param name="SharedWith">The other classes whose construction path stores this same table.</param>
public sealed record ClassVtable(
	string Name,
	string Family,
	uint VtableRva,
	int Witnesses,
	IReadOnlyList<uint> Candidates,
	int Size,
	IReadOnlyList<int> SizeCandidates,
	bool? Networked,
	string NetworkedBytes,
	IReadOnlyList<string> SharedWith);

/// <summary>
///     The method table, the class size and the networked verdict, all read from the image rather
///     than from a live object.
///
///     The construction path is what names the table. A heap-allocated component is built by the
///     same three steps every time: the allocator is called with the class's size, the class's
///     table is loaded with "lea r64, [rip+disp]" into read-only data, and that register is stored
///     to the first word of the new object. The store to offset zero is what separates the table
///     from everything else the constructor loads: FoodItemComponent's constructor loads a second
///     table right after and stores it to +0x10, which is a second base class, not this class's
///     identity.
///
///     From the table the rest follows the way the live reader already does it. Slot 0 is the
///     deleting destructor, which hands the class's own size to the deallocator, and slot 3 is
///     isNetworkComponent, whose body is a three byte stub: B0 01 C3 returns one, 31 C0 C3 returns
///     zero, and anything else is handed back as bytes because a body this does not recognise is
///     not evidence either way.
///
///     Where two classes construct with the same table, the linker folded their bodies; both keep
///     the table and each lists the other, because the table alone can no longer tell them apart.
/// </summary>
/// <summary>
///     One method table a block definition builds its components on, and what the build states
///     about it.
///     <para>
///         A definition's components are not the block's. The block keeps a BlockComponentStorage
///         node per component and files it under a runtime id; a definition keeps a vector of
///         Block*Description objects, each a heap object whose first word is its own table. There
///         is no id, so the table is the only identity, and this is what the image says about one.
///     </para>
///     <para>
///         Slot 7 is the description's networked verdict, a three byte stub the same way slot 3 is
///         the component's: B0 01 C3 returns one, 31 C0 C3 returns zero. That verdict is the rule
///         the server applies to its own tree, so a component whose table says zero is not on the
///         wire and a permutation holding none but those is not sent at all.
///     </para>
///     <para>
///         <paramref name="Installs" /> is every component class the table's own twelve slots name
///         through a signature literal, which is the description saying which component it builds.
///         <paramref name="States" /> is every tag literal those same slots reference, which is the
///         description's own reader naming the fields it fills. <paramref name="Names" /> is every
///         class name those literals qualify a function with, which is the build naming the class
///         outright. All three are the build talking; which of them identifies a class is the
///         reference's call.
///     </para>
///     <para>
///         <paramref name="Constructs" /> is the fourth witness and the only one most of these
///         tables have. entt bakes <c>[Len = N, Type = X]</c> into the signature of the any vtable
///         and the any deleter it instantiates for X, and those two functions take the address of
///         X's own method table, so a function carrying that literal and this table is the build
///         saying which class the table belongs to. Five tables the reference already identifies
///         through <paramref name="Installs" /> or <paramref name="States" /> agree with it.
///     </para>
/// </summary>
public sealed record BlockDescriptionTable(uint Rva, bool? Networked, string NetworkedBytes,
	IReadOnlyList<string> Installs, IReadOnlyList<string> States, IReadOnlyList<string> Names,
	IReadOnlyList<string> Constructs);

public static class Vtables
{
	/// <summary>How far past the LEA the store to the first word may sit before the pair stops counting as one construction.</summary>
	private const int StoreWindow = 6;

	/// <summary>The window the deleting destructor is read through, the same 1024 bytes the live reader uses.</summary>
	private const int DestructorWindow = 1024;

	public static IReadOnlyList<ClassVtable> Find(CodeIndex code, PeImage image, IReadOnlyList<InventoryClass> classes)
	{
		PeSection rdata = image.Sections.First(s => s.Name == ".rdata");

		var found = new List<ClassVtable>();
		foreach (InventoryClass entry in classes)
		{
			var functions = new HashSet<uint>();
			foreach (uint stringRva in entry.SignatureRvas)
			{
				foreach (uint site in code.ReferencesTo(stringRva))
				{
					if (code.TryFunctionOf(site, out PeFunction function)) functions.Add(function.Begin);
				}
			}

			var votes = new Dictionary<uint, int>();
			foreach (uint begin in functions)
			{
				if (!code.TryFunctionOf(begin, out PeFunction function)) continue;
				foreach (uint table in ConstructedTables(code, function, rdata)) votes[table] = votes.GetValueOrDefault(table) + 1;
			}

			List<uint> candidates = votes.Keys.OrderBy(t => t).ToList();
			if (votes.Count != 1)
			{
				found.Add(new ClassVtable(entry.Name, entry.Family, 0, 0, candidates, 0, Array.Empty<int>(), null, null, Array.Empty<string>()));
				continue;
			}

			uint vtable = candidates[0];
			List<int> sizes = ClassSizeCandidates(image, vtable);
			(bool? networked, string bytes) = Networks(image, vtable);
			found.Add(new ClassVtable(entry.Name, entry.Family, vtable, votes[vtable], candidates,
				sizes.Count == 0 ? 0 : sizes[0], sizes, networked, bytes, Array.Empty<string>()));
		}

		// The folded tables, filled in once every class has one.
		var byTable = found.Where(v => v.VtableRva != 0).GroupBy(v => v.VtableRva).ToDictionary(g => g.Key, g => g.Select(v => v.Name).ToList());
		return found
			.Select(v => v.VtableRva != 0 && byTable[v.VtableRva].Count > 1
				? v with { SharedWith = byTable[v.VtableRva].Where(n => n != v.Name).OrderBy(n => n, StringComparer.Ordinal).ToList() }
				: v)
			.ToList();
	}

	/// <summary>How many slots a block definition component description's method table has of its own.</summary>
	private const int DescriptionSlots = 12;

	/// <summary>Which slot of that table holds the networked verdict.</summary>
	private const int DescriptionNetworkedSlot = 7;

	/// <summary>How far back a class name may sit inside the literal that carries it.</summary>
	private const int LiteralReach = 512;

	/// <summary>
	///     Every method table a block definition builds components on, found where a table is
	///     found rather than searched for: a constructor loads it with a LEA, so the index of
	///     rip-relative targets already holds every one of them. A candidate is kept when all
	///     twelve of its slots are code and the seventh is one of the two networked stubs, which
	///     is a shape arbitrary read-only data does not have.
	///     <para>
	///         What each table then states is read from its own slots and nothing else: the
	///         component classes their signature literals name, and the tag literals they
	///         reference. A literal is only a name when the byte after it is the terminator; the
	///         compiler merges short strings into the tails of longer ones, and a hit inside one
	///         of those is that longer string.
	///     </para>
	/// </summary>
	public static IReadOnlyList<BlockDescriptionTable> Descriptions(CodeIndex code, PeImage image,
		IReadOnlyList<InventoryClass> classes, IReadOnlyList<string> tags, IReadOnlyList<string> names)
	{
		var installs = new Dictionary<uint, SortedSet<string>>();
		foreach (InventoryClass entry in classes.Where(c => c.Family == "BlockComponentStorage"))
		{
			foreach (uint literal in entry.SignatureRvas)
			{
				foreach (uint site in code.ReferencesTo(literal))
				{
					if (!code.TryFunctionOf(site, out PeFunction function)) continue;
					Add(installs, function.Begin, entry.Name);
				}
			}
		}

		var stated = new Dictionary<uint, SortedSet<string>>();
		var literals = new DataLiterals(image, tags);
		foreach (string tag in tags)
		{
			foreach (uint rva in literals.Occurrences(tag))
			{
				if (!image.TryFileOffset(rva + (uint) tag.Length, out _)) continue;
				if (image.Bytes(rva + (uint) tag.Length, 1)[0] != 0) continue;
				foreach (uint site in code.ReferencesTo(rva))
				{
					if (!code.TryFunctionOf(site, out PeFunction function)) continue;
					Add(stated, function.Begin, tag);
				}
			}
		}

		// A class the build names outright. Clang bakes the qualified function name into the
		// assertion paths, so "BlockGeometryDescription::initializeComponent" is a literal and the
		// two colons behind the name are what says it is a qualifier rather than a longer word.
		var qualified = new Dictionary<uint, SortedSet<string>>();
		var classLiterals = new DataLiterals(image, names);
		foreach (string name in names)
		{
			foreach (uint rva in classLiterals.Occurrences(name))
			{
				if (!image.TryFileOffset(rva + (uint) name.Length + 1, out _)) continue;
				ReadOnlySpan<byte> after = image.Bytes(rva + (uint) name.Length, 2);
				if (after[0] != (byte) ':' || after[1] != (byte) ':') continue;

				// The code refers to the literal, not to the middle of it. A class name sits
				// inside a whole function signature, so the address that is referenced is the
				// byte after the previous terminator, and looking the interior up finds nothing.
				uint start = rva;
				while (start > 0 && rva - start < LiteralReach)
				{
					if (!image.TryFileOffset(start - 1, out _)) break;
					if (image.Bytes(start - 1, 1)[0] == 0) break;
					start--;
				}

				foreach (uint site in code.ReferencesTo(start))
				{
					if (!code.TryFunctionOf(site, out PeFunction function)) continue;
					Add(qualified, function.Begin, name);
				}
			}
		}

		// The class the entt any names. Its vtable and its deleter are instantiated per type and
		// carry "[Len = N, Type = X]" in their own signature, and both take the address of X's
		// method table, so a function holding that literal and a table names the table's class.
		var anyNamed = new Dictionary<uint, SortedSet<string>>();
		var anyNeedles = names.Select(AnyNeedle).ToList();
		var anyLiterals = new DataLiterals(image, anyNeedles);
		for (var i = 0; i < names.Count; i++)
		{
			foreach (uint rva in anyLiterals.Occurrences(anyNeedles[i]))
			{
				// The code refers to the whole signature, whose first byte is the one after the
				// previous terminator, so the interior address the needle landed on finds nothing.
				uint start = rva;
				while (start > 0 && rva - start < LiteralReach)
				{
					if (!image.TryFileOffset(start - 1, out _)) break;
					if (image.Bytes(start - 1, 1)[0] == 0) break;
					start--;
				}

				foreach (uint site in code.ReferencesTo(start))
				{
					if (!code.TryFunctionOf(site, out PeFunction function)) continue;
					Add(anyNamed, function.Begin, names[i]);
				}
			}
		}

		PeSection text = image.Text;
		var found = new List<BlockDescriptionTable>();
		foreach (uint target in code.ReferenceTargets.Distinct().OrderBy(t => t))
		{
			// Every slot but the last has to be code. The last one is not: a table's twelfth
			// entry is nought on most of these classes and the next table's first function on
			// the rest, so requiring it too threw away sixteen of the twenty tables the
			// definitions actually use.
			var slots = new List<uint>();
			var solid = 0;
			for (var slot = 0; slot < DescriptionSlots; slot++)
			{
				if (!TryRead(image, target + (uint) (slot * 8), 8, out ReadOnlySpan<byte> word)) break;
				ulong entry = BitConverter.ToUInt64(word);
				if (entry < image.ImageBase) continue;
				uint rva = (uint) (entry - image.ImageBase);
				if (rva < text.VirtualAddress || rva >= text.VirtualAddress + text.VirtualSize) continue;
				if (slot < DescriptionSlots - 1) solid++;
				slots.Add(rva);
			}
			if (solid != DescriptionSlots - 1) continue;

			(bool? networked, string bytes) = Networks(image, target, DescriptionNetworkedSlot);
			if (networked is null) continue;

			var builds = new SortedSet<string>(StringComparer.Ordinal);
			var tagged = new SortedSet<string>(StringComparer.Ordinal);
			var named = new SortedSet<string>(StringComparer.Ordinal);
			foreach (uint slot in slots)
			{
				uint key = code.TryFunctionOf(slot, out PeFunction function) ? function.Begin : slot;
				if (installs.TryGetValue(key, out SortedSet<string> one)) builds.UnionWith(one);
				if (stated.TryGetValue(key, out SortedSet<string> two)) tagged.UnionWith(two);
				if (qualified.TryGetValue(key, out SortedSet<string> three)) named.UnionWith(three);
			}

			var constructs = new SortedSet<string>(StringComparer.Ordinal);
			foreach (uint site in code.ReferencesTo(target))
			{
				if (!code.TryFunctionOf(site, out PeFunction function)) continue;
				if (anyNamed.TryGetValue(function.Begin, out SortedSet<string> four)) constructs.UnionWith(four);
			}

			found.Add(new BlockDescriptionTable(target, networked, bytes, builds.ToList(), tagged.ToList(),
				named.ToList(), constructs.ToList()));
		}

		return found;
	}

	/// <summary>The signature entt bakes into the any it instantiates for a class of this name.</summary>
	private static string AnyNeedle(string name) => "Type = " + name + "]";

	private static void Add(Dictionary<uint, SortedSet<string>> to, uint key, string name)
	{
		if (!to.TryGetValue(key, out SortedSet<string> set)) to[key] = set = new SortedSet<string>(StringComparer.Ordinal);
		set.Add(name);
	}

	/// <summary>
	///     Every read-only-data address this function loads with a LEA and stores, within a few
	///     instructions, to the first word of the register it was loaded beside. The store is
	///     "mov [reg], leaReg" with no displacement, which is the object's first word and nothing
	///     else.
	/// </summary>
	private static IEnumerable<uint> ConstructedTables(CodeIndex code, PeFunction function, PeSection rdata)
	{
		var pending = new List<(Register Register, uint Target, int At)>();
		var tables = new List<uint>();
		int index = 0;
		foreach (Instruction ins in code.Decode(function))
		{
			index++;
			pending.RemoveAll(p => index - p.At > StoreWindow);

			if (ins.Mnemonic == Mnemonic.Lea && ins.IsIPRelativeMemoryOperand && ins.Op0Kind == OpKind.Register)
			{
				uint target = (uint) (ins.IPRelativeMemoryAddress - code.ImageBase);
				if (target >= rdata.VirtualAddress && target < rdata.VirtualAddress + rdata.VirtualSize)
				{
					pending.RemoveAll(p => p.Register == ins.Op0Register);
					pending.Add((ins.Op0Register, target, index));
				}
				continue;
			}

			if (ins.Mnemonic != Mnemonic.Mov || ins.Op0Kind != OpKind.Memory || ins.Op1Kind != OpKind.Register) continue;
			if (ins.MemorySize != MemorySize.UInt64 && ins.MemorySize != MemorySize.Int64) continue;
			if (ins.MemoryDisplacement64 != 0 || ins.MemoryIndex != Register.None || ins.MemoryBase == Register.None) continue;
			if (ins.IsIPRelativeMemoryOperand) continue;

			foreach ((Register register, uint target, int _) in pending)
			{
				if (register != ins.Op1Register) continue;
				if (!tables.Contains(target)) tables.Add(target);
			}
			// The register has been consumed by its store; a later store of the same register is
			// the same table, not a second one.
			pending.RemoveAll(p => p.Register == ins.Op1Register);
		}
		return tables;
	}

	/// <summary>
	///     Every size the deleting destructor passes to a call, in the order it passes them. This is
	///     <see cref="ItemRegistry.ClassSizeCandidates" /> reading the image instead of the process:
	///     the table's first slot is a code address, the address is an RVA once the image base comes
	///     off, and a slot that only jumps is a thunk whose target is the real body. A destructor
	///     that destroys members first hands their sizes to their own deletes before its own, so a
	///     class whose first one is not its own is visible here rather than answered wrong.
	/// </summary>
	public static List<int> ClassSizeCandidates(PeImage image, uint vtableRva)
	{
		var sizes = new List<int>();
		if (!TryRead(image, vtableRva, 8, out ReadOnlySpan<byte> slot)) return sizes;
		ulong destructor = BitConverter.ToUInt64(slot);
		if (destructor < image.ImageBase) return sizes;
		uint at = (uint) (destructor - image.ImageBase);

		if (!TryRead(image, at, DestructorWindow, out ReadOnlySpan<byte> code)) return sizes;

		if (code[0] == 0xE9)
		{
			long target = (long) at + 5 + BitConverter.ToInt32(code.Slice(1, 4));
			if (target < 0 || !TryRead(image, (uint) target, DestructorWindow, out code)) return sizes;
		}

		for (int i = 0; i + 5 <= code.Length; i++)
		{
			if (code[i] != 0xBA) continue;
			int value = BitConverter.ToInt32(code.Slice(i + 1, 4));
			if (value is < 16 or > 8192 || value % 8 != 0) continue;
			for (int j = i + 5; j < Math.Min(i + 21, code.Length); j++)
			{
				if (code[j] != 0xE8) continue;
				sizes.Add(value);
				break;
			}
		}

		return sizes;
	}

	/// <summary>
	///     Whether the class networks this component, read out of method table slot 3 the way the
	///     live reader reads it.
	/// </summary>
	private static (bool? Networked, string Bytes) Networks(PeImage image, uint vtableRva, int at = 3)
	{
		if (!TryRead(image, vtableRva + (uint) (at * 8), 8, out ReadOnlySpan<byte> slot)) return (null, null);
		ulong body = BitConverter.ToUInt64(slot);
		if (body < image.ImageBase) return (null, null);
		if (!TryRead(image, (uint) (body - image.ImageBase), 3, out ReadOnlySpan<byte> code)) return (null, null);

		string bytes = Convert.ToHexString(code);
		return bytes switch
		{
			"B001C3" => (true, null),
			"31C0C3" => (false, null),
			_ => (null, bytes)
		};
	}

	private static bool TryRead(PeImage image, uint rva, int length, out ReadOnlySpan<byte> bytes)
	{
		bytes = default;
		if (!image.TryFileOffset(rva, out _) || !image.TryFileOffset(rva + (uint) length - 1, out _)) return false;
		bytes = image.Bytes(rva, length);
		return true;
	}
}
