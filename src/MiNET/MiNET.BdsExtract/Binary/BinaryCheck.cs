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

using System;
using System.Collections.Generic;
using System.Linq;

namespace MiNET.BdsExtract.Binary;

/// <summary>
///     The reference against the code, as a second witness to the memory checks.
///     <para>
///         The memory checks say whether an offset still reads the value the reference states for
///         that object. This says whether the layout the reference declares is the layout the
///         build's own code describes: the classes it names, how big each one is, what the enum
///         tables hold, and where the reflection binder puts each networked member.
///     </para>
///     <para>
///         Nothing here stops a run and nothing is filtered. Every difference is appended to
///         <see cref="Differences" /> with a prefix and printed, and classifying one as acceptable
///         is Niclas's call.
///     </para>
/// </summary>
public sealed class BinaryCheck
{
	/// <summary>The block storage family, whose classes live inside a node rather than on their own.</summary>
	private const string BlockFamily = "BlockComponentStorage";

	/// <summary>
	///     The word a storage node keeps in front of the component it holds, which is also what the
	///     node is aligned to. A block component in the storage vector is never allocated on its
	///     own, so the size the binary states is the node's: a method table word, then the
	///     component, rounded up to the node's own pointer alignment.
	///     <para>
	///         The eight is the node's and not the class's. A state's direct data holds a pointer
	///         straight at the payload and the same classes read correctly from their first byte
	///         there, so a class that declared the word as its own base would be read two ways.
	///     </para>
	/// </summary>
	private const int NodeAlignment = 8;

	private readonly BinaryFacts _facts;

	private readonly Dictionary<string, BinaryClass> _byName = new(StringComparer.Ordinal);
	private readonly Dictionary<string, ClassLayout> _reference = new(StringComparer.Ordinal);
	private readonly Dictionary<string, BlockMembers.Source> _source = new(StringComparer.Ordinal);

	private int _sizeAgrees, _sizeDiffers, _sizeUnstated, _unnamed, _undeclaredClasses, _described, _notDescribed;
	private int _enumAgrees, _enumDiffers, _enumAdded, _enumNotFound;
	private int _memberAgrees, _memberDiffers, _memberUnstated, _undeclaredMembers, _unboundMembers;
	private int _namePairs, _nameUnmapped;

	public BinaryCheck(BinaryFacts facts)
	{
		_facts = facts;
	}

	/// <summary>Every difference, in the order the checks found them. The prefix says which check.</summary>
	public List<string> Differences { get; } = [];

	/// <summary>The one line the run prints for this check.</summary>
	public string Summary { get; private set; } = "";

	public void Run()
	{
		LoadReference();

		Classes();
		Inventory();
		Enums();
		Members();

		Summary = $"binary check: {_sizeAgrees} classes agree on size, {_sizeDiffers} disagree, "
			+ $"{_sizeUnstated} the binary does not size, {_unnamed} name no binary class; "
			+ $"{_undeclaredClasses} undeclared classes; "
			+ $"{_described} description classes translate onto their component, {_notDescribed} do not; "
			+ $"{_enumAgrees} enum values agree, {_enumDiffers} differ, {_enumAdded} added, {_enumNotFound} enums not found; "
			+ $"{_memberAgrees} member offsets agree, {_memberDiffers} disagree, {_memberUnstated} carry no width, "
			+ $"{_undeclaredMembers} bindings name no member, {_unboundMembers} declared members no binding names; "
			+ $"{_nameUnmapped} of {_namePairs} name pairs unmapped";
	}

	// ---- the reference -------------------------------------------------------------------------

	/// <summary>
	///     Every class the reference states, from whichever file states it. A class named by two
	///     files is one class, so the first file to state it wins and a second statement of a
	///     different size is a difference of its own rather than a silent overwrite.
	/// </summary>
	private void LoadReference()
	{
		foreach (BinaryClass entry in _facts.Classes) _byName.TryAdd(entry.Name, entry);

		foreach (BlockMembers.Source source in new[] { BlockMembers.Source.Blocks, BlockMembers.Source.States, BlockMembers.Source.Items })
		{
			foreach (ClassLayout held in BlockMembers.All(source))
			{
				if (_reference.TryGetValue(held.Name, out ClassLayout first))
				{
					if (first.Size != held.Size || first.Base != held.Base)
					{
						Differences.Add($"class: {held.Name} is stated twice, {_source[held.Name]} says {first.Base}+{first.Size} "
							+ $"and {source} says {held.Base}+{held.Size}");
						_sizeDiffers++;
					}
					continue;
				}

				_reference[held.Name] = held;
				_source[held.Name] = source;
			}
		}
	}

	/// <summary>The class in the binary a reference name stands for, by its own name or through a namespace.</summary>
	private BinaryClass Binary(string name)
	{
		if (_byName.TryGetValue(name, out BinaryClass exact)) return exact;
		List<BinaryClass> qualified = _facts.Classes
			.Where(c => c.Name.EndsWith("::" + name, StringComparison.Ordinal))
			.OrderByDescending(c => c.Size)
			.ToList();
		return qualified.FirstOrDefault();
	}

	// ---- 1: the classes ------------------------------------------------------------------------

	/// <summary>
	///     Every component class the reference declares, against the size the binary states for it.
	///     <para>
	///         The two families are sized differently and the rule follows what the size IS. An item
	///         component is allocated on its own, so the class's base plus its own size is the size
	///         its deleting destructor hands the deallocator. A block component sits inside a
	///         storage node, so what the binary states is the node: the base's method table word
	///         plus the component rounded up to the node's alignment.
	///     </para>
	/// </summary>
	private void Classes()
	{
		foreach (ClassLayout held in _reference.Values.OrderBy(c => c.Name, StringComparer.Ordinal))
		{
			BinaryClass entry = BlockComponent(held.Name);
			bool node = entry is not null;
			entry ??= ItemComponent(held);
			if (entry is null) continue;

			if (entry.Size == 0)
			{
				Differences.Add($"class: {held.Name} matches {entry.Name} in the binary, which states no size "
					+ $"({(entry.Vtable == 0 ? "no method table resolved" : "the deleting destructor hands out no size")}); "
					+ $"the reference declares {held.Base}+{held.Size}");
				_sizeUnstated++;
				continue;
			}

			// A block component's class does not declare the node's word: the state's own direct
			// data hands out a pointer straight at the payload and reads the same class from its
			// first byte, so the eight belongs to the node and is added here rather than by the
			// class. Where a class does declare a base, that base is what precedes it.
			int prefix = node ? held.Base > 0 ? held.Base : NodeAlignment : held.Base;
			int declared = node ? prefix + Rounded(held.Size) : prefix + held.Size;
			if (declared == entry.Size)
			{
				_sizeAgrees++;
			}
			else
			{
				string rule = node
					? $"{prefix}+{held.Size} rounded to {NodeAlignment} = {declared}"
					: $"{prefix}+{held.Size} = {declared}";
				Differences.Add($"size: {held.Name} declares {rule}, the binary states {entry.Size} "
					+ $"({(node ? "nodeSize" : "size")} of {entry.Name}), {Math.Abs(entry.Size - declared)} bytes "
					+ $"{(entry.Size > declared ? "past the declaration" : "short of it")}");
				_sizeDiffers++;
			}

			// The reference states no networked flag per class, so there is nothing to compare the
			// binary's verdict against. Said once, in the summary, rather than per class.
		}

		foreach (ClassLayout held in _reference.Values.OrderBy(c => c.Name, StringComparer.Ordinal))
		{
			if (held.Base == 0 || _source[held.Name] != BlockMembers.Source.Items) continue;
			if (Binary(held.Name) is not null) continue;

			Differences.Add($"class: {held.Name} declares a base of {held.Base} and names no class in the binary's item family");
			_unnamed++;
		}
	}

	/// <summary>
	///     The block component class a reference name stands for, or null when the block family
	///     does not name it. The reference's own componentClasses map carries the alias where the
	///     two spellings differ, so BlockCollisionBoxComponent finds BlockShapeComponentData.
	/// </summary>
	private BinaryClass BlockComponent(string referenceName)
	{
		foreach (BinaryClass entry in _facts.Classes.Where(c => c.Family == BlockFamily))
		{
			if (entry.Name == referenceName) return entry;
			if (BlockMembers.ComponentClasses.TryGetValue(entry.Name, out string mapped) && mapped == referenceName) return entry;
		}

		return null;
	}

	/// <summary>The item component class a reference class stands for. A base is what makes it one.</summary>
	private BinaryClass ItemComponent(ClassLayout held)
	{
		if (held.Base == 0) return null;
		if (_source[held.Name] != BlockMembers.Source.Items) return null;
		return Binary(held.Name);
	}

	private static int Rounded(int size) => (size + NodeAlignment - 1) / NodeAlignment * NodeAlignment;

	// ---- 2: the inventory ----------------------------------------------------------------------

	/// <summary>Every class the binary names that the reference does not declare.</summary>
	private void Inventory()
	{
		foreach (BinaryClass entry in _facts.Classes)
		{
			string declared = BlockMembers.ComponentClasses.GetValueOrDefault(entry.Name) ?? entry.Name;
			if (_reference.ContainsKey(declared)) continue;

			// A namespaced spelling of a class the reference declares under its bare name is the
			// same class, and the size check above already paired them.
			int separator = entry.Name.LastIndexOf("::", StringComparison.Ordinal);
			if (separator > 0 && _reference.ContainsKey(entry.Name[(separator + 2)..])) continue;

			bool node = entry.Family == BlockFamily;
			Differences.Add($"undeclared: {(node ? "block" : "item")} class {entry.Name}, "
				+ $"{(entry.Size == 0 ? "no size stated" : $"{(node ? "nodeSize" : "size")} {entry.Size}")}, "
				+ $"networked {entry.Networked?.ToString() ?? "unstated"}");
			_undeclaredClasses++;
		}
	}

	// ---- 3: the enums --------------------------------------------------------------------------

	/// <summary>
	///     Every enum table the reference states, against the table the code registers. A table the
	///     binary phase rejected is reported with what it did find, because a name the search never
	///     located is a hole in the search, not a hole in the game.
	/// </summary>
	private void Enums()
	{
		var stated = BlockMembers.Enums;
		foreach (EnumTable table in _facts.Enums.OrderBy(t => t.Name, StringComparer.Ordinal))
		{
			if (!stated.TryGetValue(table.Name, out Dictionary<long, string> reference))
			{
				Differences.Add($"enum: {table.Name} is in the binary facts and the reference states no table for it");
				_enumNotFound++;
				continue;
			}

			if (!table.Accepted)
			{
				Differences.Add($"enum: {table.Name} not found in the binary; the reference states {reference.Count} values "
					+ $"and the search is anchored on their names, which the reference spells as class names "
					+ $"while the binary registers wire spellings: {table.Evidence}");
				_enumNotFound++;
				continue;
			}

			foreach ((int value, string name) in table.Values.OrderBy(v => v.Key))
			{
				if (!reference.TryGetValue(value, out string was))
				{
					Differences.Add($"enum: {table.Name} {value} is {name} in the binary and the reference states no name for it");
					_enumAdded++;
					continue;
				}

				if (was == name)
				{
					_enumAgrees++;
					continue;
				}

				Differences.Add($"enum: {table.Name} {value} is {name} in the binary and {was} in the reference");
				_enumDiffers++;
			}

			foreach ((long value, string name) in reference.OrderBy(v => v.Key))
			{
				if (table.Values.ContainsKey((int) value)) continue;
				Differences.Add($"enum: {table.Name} {value} is {name} in the reference and the binary's table stops short of it");
				_enumDiffers++;
			}
		}
	}

	// ---- 4: the members ------------------------------------------------------------------------

	/// <summary>
	///     Every reflection binding against the member the reference declares at that place.
	///     <para>
	///         A binding states a whole-object offset, so the reference member it stands for is the
	///         one whose own offset plus its class's base lands on it. The width comes from the
	///         binding where the binder stated one and from the primitive the type names otherwise;
	///         a type that is neither leaves the width unstated, which is a third answer and is
	///         counted as one rather than being called a disagreement.
	///     </para>
	///     <para>
	///         The block side binds on the JSON description class, not on the runtime component, so
	///         a description's offsets are that class's own. They are used only where the runtime
	///         class declares a member of the same wire name, and every line says which class the
	///         offset came from.
	///     </para>
	/// </summary>
	private void Members()
	{
		var bound = new Dictionary<string, HashSet<string>>(StringComparer.Ordinal);
		Dictionary<string, int> describes = Describes();

		foreach (Binding binding in _facts.Bindings.OrderBy(b => b.Owner, StringComparer.Ordinal).ThenBy(b => b.Offset))
		{
			if (binding.Owner is null) continue;
			(ClassLayout held, string through) = Owner(binding, describes);
			if (held is null) continue;

			string via = through is null ? "" : $" [through {through}, the JSON description class, measured {describes[through]} bytes ahead of the runtime component]";
			int at = binding.Offset - (through is null ? held.Base : describes[through]);
			BlockMember member = held.Members.FirstOrDefault(m =>
				m.Kind is not (MemberKind.Padding or MemberKind.Unknown) && m.At == at);

			if (member.Name is null)
			{
				BlockMember gap = held.Members.FirstOrDefault(m =>
					m.Kind is MemberKind.Padding or MemberKind.Unknown && at >= m.At && at < m.At + m.Bytes);
				BlockMember inside = held.Members.FirstOrDefault(m =>
					m.Kind is not (MemberKind.Padding or MemberKind.Unknown) && at > m.At && at < m.At + m.Bytes);
				string where = gap.Name is not null
					? $"inside the declared {gap.Kind.ToString().ToLowerInvariant()} {gap.Name} at +{gap.At} of {gap.Bytes} bytes"
					: inside.Name is not null
						? $"inside the declared member {inside.Name} at +{inside.At} of {inside.Bytes} bytes"
						: at >= held.Size ? "past the declared end of the class" : "at a place the class declares nothing";
				Differences.Add($"undeclared: {held.Name}.{binding.WireName} at object +{binding.Offset} "
					+ $"(class +{at}) is {binding.TypeName ?? $"type 0x{binding.TypeHash:X8}"}"
					+ $"{(binding.Width > 0 ? $", {binding.Width} bytes" : "")}, {where}{via}");
				_undeclaredMembers++;
				continue;
			}

			bound.TryAdd(held.Name, new HashSet<string>(StringComparer.Ordinal));
			bound[held.Name].Add(member.Name);

			_namePairs++;
			if (Normalized(member.Name) != Normalized(binding.WireName))
			{
				Differences.Add($"unmapped: {held.Name}.{member.Name} is {binding.WireName} on the wire{via}");
				_nameUnmapped++;
			}

			int width = binding.Width > 0 ? binding.Width : Primitive(binding.TypeName);
			if (width == 0)
			{
				Differences.Add($"member: {held.Name}.{member.Name} at +{member.At} declares {member.Bytes} bytes and "
					+ $"the binding for {binding.WireName} states no width ({binding.TypeName ?? $"type 0x{binding.TypeHash:X8}"} "
					+ $"is not a primitive){via}");
				_memberUnstated++;
				continue;
			}

			if (width == member.Bytes)
			{
				_memberAgrees++;
				continue;
			}

			// Where the type is a primitive the width is the type's own and there is nothing else
			// it could be. Where it came from a store the binder's setter happened to make, the
			// store can be narrower than the member, so the line says which of the two it is.
			Differences.Add($"member: {held.Name}.{member.Name} at +{member.At} declares {member.Bytes} bytes and "
				+ $"the binding for {binding.WireName} states {width} "
				+ $"({binding.TypeName}{(Primitive(binding.TypeName) > 0 ? ", a primitive" : ", from the setter's store")}){via}");
			_memberDiffers++;
		}

		// The other way round. A class the binder registers members for still holds state it never
		// sends, so a declared member no binding names is expected rather than wrong; it is counted
		// and listed so the two halves of each class are both visible.
		foreach ((string name, HashSet<string> named) in bound.OrderBy(b => b.Key, StringComparer.Ordinal))
		{
			ClassLayout held = _reference[name];
			foreach (BlockMember member in held.Members)
			{
				if (member.Kind is MemberKind.Padding or MemberKind.Unknown) continue;
				if (named.Contains(member.Name)) continue;

				Differences.Add($"member: {held.Name}.{member.Name} at +{member.At} of {member.Bytes} bytes "
					+ $"({member.Kind.ToString().ToLowerInvariant()}) is declared and no binding names it");
				_unboundMembers++;
			}
		}
	}

	/// <summary>
	///     The class a binding belongs to, and the description class it came through where it is not
	///     the runtime one. An owner is taken at its own name; a namespaced spelling is a different
	///     class with its own layout and is never folded onto the runtime class.
	/// </summary>
	private (ClassLayout Held, string Through) Owner(Binding binding, Dictionary<string, int> describes)
	{
		if (_reference.TryGetValue(binding.Owner, out ClassLayout own)) return (own, null);
		if (!describes.ContainsKey(binding.Owner)) return (null, null);
		return (_reference[Runtime(binding.Owner)], binding.Owner);
	}

	/// <summary>The runtime component a JSON description class describes, by name.</summary>
	private static string Runtime(string description) => description[..^"Description".Length] + "Component";

	/// <summary>
	///     How far ahead of the runtime component each JSON description class puts the same member.
	///     <para>
	///         The block side binds on the description, which is the class the behaviour pack JSON
	///         loads into, not on the component the game runs. The two are different classes and
	///         nothing says their members line up, so the distance is measured rather than assumed:
	///         every binding whose wire name the runtime class also declares states one distance,
	///         and the description is usable only when every one of them states the same distance.
	///         A description whose members disagree is reported with the distances it produced and
	///         none of its bindings is read across.
	///     </para>
	/// </summary>
	private Dictionary<string, int> Describes()
	{
		var measured = new Dictionary<string, int>(StringComparer.Ordinal);
		IEnumerable<IGrouping<string, Binding>> owners = _facts.Bindings
			.Where(b => b.Owner is not null
				&& b.Owner.EndsWith("Description", StringComparison.Ordinal)
				&& !b.Owner.Contains("::", StringComparison.Ordinal)
				&& !_reference.ContainsKey(b.Owner)
				&& _reference.ContainsKey(Runtime(b.Owner)))
			.GroupBy(b => b.Owner, StringComparer.Ordinal)
			.OrderBy(g => g.Key, StringComparer.Ordinal);

		foreach (IGrouping<string, Binding> owner in owners)
		{
			ClassLayout held = _reference[Runtime(owner.Key)];
			var distances = new Dictionary<int, List<string>>();
			foreach (Binding binding in owner)
			{
				BlockMember named = held.Members.FirstOrDefault(m => Normalized(m.Name) == Normalized(binding.WireName));
				if (named.Name is null) continue;
				int distance = binding.Offset - named.At;
				if (!distances.TryGetValue(distance, out List<string> names)) distances[distance] = names = [];
				names.Add(binding.WireName);
			}

			if (distances.Count == 1)
			{
				measured[owner.Key] = distances.Keys.First();
				_described++;
				continue;
			}

			Differences.Add(distances.Count == 0
				? $"class: {owner.Key} describes {Runtime(owner.Key)} and no wire name it binds is a member the runtime class declares, "
					+ $"so nothing measures how far apart the two classes put the same member: "
					+ $"{string.Join(", ", owner.Select(b => $"{b.WireName} at +{b.Offset}"))}"
				: $"class: {owner.Key} describes {Runtime(owner.Key)} and its members are not one distance apart from the "
					+ $"runtime class's, so it is a different layout and none of its bindings is read across: "
					+ $"{string.Join("; ", distances.OrderBy(d => d.Key).Select(d => $"+{d.Key} from {string.Join(", ", d.Value)}"))}");
			_notDescribed++;
		}

		return measured;
	}

	/// <summary>A name with the case and the separators taken out, so snake and camel compare equal.</summary>
	private static string Normalized(string name)
	{
		return new string((name ?? "").Where(char.IsLetterOrDigit).Select(char.ToLowerInvariant).ToArray());
	}

	/// <summary>How wide a primitive is, or nought where the type names something else.</summary>
	private static int Primitive(string type)
	{
		return type switch
		{
			"bool" or "char" or "signed char" or "unsigned char" => 1,
			"short" or "unsigned short" => 2,
			"int" or "unsigned int" or "long" or "unsigned long" or "float" => 4,
			"long long" or "unsigned long long" or "double" => 8,
			_ => 0
		};
	}
}
