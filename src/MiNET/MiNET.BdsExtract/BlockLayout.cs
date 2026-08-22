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

/// <summary>
///     Where each member of a block sits on the server being read, measured from the object's start.
///     <para>
///         On 1.26.20.5 the positions are the published ones in <see cref="BlockMembers" />, computed
///         from the class declaration rather than found by searching. That build is the reference:
///         it is the only one whose layout is stated rather than inferred.
///     </para>
///     <para>
///         On any other build the positions start as that build's and are replaced by what is
///         measured against the reference. A member the measurement does not settle keeps the
///         published position and is reported as unsettled, so a value read from a position nothing
///         confirmed is never mistaken for one that was.
///     </para>
/// </summary>
public static class BlockLayout
{
	/// <summary>How a member's position on this server was arrived at.</summary>
	public enum Source
	{
		/// <summary>The class declaration states it, on the build it is published for.</summary>
		Published,

		/// <summary>Measured on this server against the reference.</summary>
		Measured,

		/// <summary>Nothing settled it here, so the published position stands unconfirmed.</summary>
		Unsettled
	}

	/// <summary>
	///     Where each member sits on the server being read. Empty until something says so, which on
	///     the reference build is the class declaration and on every other build is a measurement.
	/// </summary>
	private static readonly Dictionary<string, int> Placed = new(StringComparer.Ordinal);

	private static readonly Dictionary<string, Source> Found = new(StringComparer.Ordinal);

	/// <summary>
	///     Every member of the block class, in the order it declares them. Only these are ever
	///     placed: a member inside one of them sits where its own class says, and moves only when
	///     the thing holding it does.
	/// </summary>
	public static IReadOnlyList<BlockMember> Members => BlockMembers.Block.Members;

	/// <summary>How far into the object anything is read, which is as far as the furthest member.</summary>
	public static int Reach => Placed.Count == 0
		? throw new InvalidOperationException("no member of the block class has been placed on this server")
		: Placed.Values.Max() + 64;

	/// <summary>
	///     Where a member sits on this server. A member nothing has placed is refused rather than
	///     answered with where it sat on another build: reading one field at the wrong offset is
	///     invisible in the output, and a build that moved one moved several.
	/// </summary>
	public static int At(string name) => Placed.TryGetValue(name, out int at)
		? at
		: throw new InvalidOperationException($"the block class member {name} was never placed on this server");

	/// <summary>Whether a member has been placed at all.</summary>
	public static bool Has(string name) => Placed.ContainsKey(name);

	/// <summary>How this member's position was arrived at on this server.</summary>
	public static Source Provenance(string name) => Found.TryGetValue(name, out Source source) ? source : Source.Unsettled;

	/// <summary>Takes a member's position as measured on this server.</summary>
	public static void Measured(string name, int at)
	{
		Placed[name] = at;
		Found[name] = Source.Measured;
	}

	/// <summary>
	///     Takes the published positions as this build's own, which they are on the one build the
	///     class declaration was published for and on no other. Nothing is placed until this or a
	///     measurement says so.
	/// </summary>
	public static void PublishedFor(Version build)
	{
		if (!IsReferenceBuild(build)) return;
		foreach (BlockMember member in BlockMembers.Block.Members)
		{
			Placed[member.Name] = member.At;
			Found[member.Name] = Source.Published;
		}

		foreach (BlockMember member in BlockMembers.State.Members) StatePlaced[member.Name] = member.At;
	}

	/// <summary>Where each member of the state class sits, on the same terms as the block's.</summary>
	private static readonly Dictionary<string, int> StatePlaced = new(StringComparer.Ordinal);

	/// <summary>Every member of the state class, in the order it declares them.</summary>
	public static IReadOnlyList<BlockMember> StateMembers => BlockMembers.State.Members;

	/// <summary>Where a state member sits on this server, or a refusal.</summary>
	public static int StateAt(string name) => StatePlaced.TryGetValue(name, out int at)
		? at
		: throw new InvalidOperationException($"the state class member {name} was never placed on this server");

	public static bool StateHas(string name) => StatePlaced.ContainsKey(name);

	public static void StateMeasured(string name, int at) => StatePlaced[name] = at;

	/// <summary>How far into a state anything is read.</summary>
	public static int StateReach => StatePlaced.Count == 0
		? throw new InvalidOperationException("no member of the state class has been placed on this server")
		: StatePlaced.Values.Max() + 64;

	/// <summary>Whether this build is the one the layout is published for.</summary>
	public static bool IsReferenceBuild(Version build) => build is not null && build == BlockMembers.Build;
}
