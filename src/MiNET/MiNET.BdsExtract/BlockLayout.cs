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

	private static readonly Dictionary<string, int> Placed = BlockMembers.All
		.ToDictionary(m => m.Name, m => m.At, StringComparer.Ordinal);

	private static readonly Dictionary<string, Source> Found = BlockMembers.All
		.ToDictionary(m => m.Name, _ => Source.Unsettled, StringComparer.Ordinal);

	/// <summary>Every member, in the order the class declares them.</summary>
	public static IReadOnlyList<BlockMember> Members => BlockMembers.All;

	/// <summary>How far into the object anything is read.</summary>
	public static int Reach => Placed.Values.Max() + 64;

	/// <summary>Where a member sits on this server.</summary>
	public static int At(string name) => Placed[name];

	/// <summary>How this member's position was arrived at on this server.</summary>
	public static Source Provenance(string name) => Found[name];

	/// <summary>Takes a member's position as measured on this server.</summary>
	public static void Measured(string name, int at)
	{
		if (!Placed.ContainsKey(name)) return;
		Placed[name] = at;
		Found[name] = Source.Measured;
	}

	/// <summary>
	///     Says the published positions are this build's own, which they are on the one build the
	///     class declaration was published for and on no other.
	/// </summary>
	public static void PublishedFor(Version build)
	{
		if (!IsReferenceBuild(build)) return;
		foreach (string name in Found.Keys.ToList()) Found[name] = Source.Published;
	}

	/// <summary>Whether this build is the one the layout is published for.</summary>
	public static bool IsReferenceBuild(Version build) => build is not null && build == BlockMembers.Build;
}
