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
///     Where each value sits in the object, from its start. The position is the one the reference
///     states, added up through whatever holds it, and nothing else ever sets one.
/// </summary>
public static class BlockLayout
{
	/// <summary>The block class, as the tree it is.</summary>
	public static ClassTree Blocks => BlockMembers.Tree(BlockMembers.Source.Blocks);

	/// <summary>The state class, as the tree it is.</summary>
	public static ClassTree States => BlockMembers.Tree(BlockMembers.Source.States);

	/// <summary>How far into a block anything is read, which is as far as the furthest value reaches.</summary>
	public static int Reach => Blocks.Leaves.Max(n => n.At + n.Member.Bytes) + 64;

	/// <summary>How far into a state anything is read.</summary>
	public static int StateReach => States.Leaves.Max(n => n.At + n.Member.Bytes) + 64;

	/// <summary>
	///     A member of the block class itself, by name, for the readers that want one thing rather
	///     than the whole tree. Only the class's own members: anything deeper belongs to whatever
	///     holds it and is reached through that.
	/// </summary>
	public static MemberNode Member(string name) =>
		Blocks.Roots.FirstOrDefault(n => n.Member.Name == name)
		?? throw new InvalidOperationException($"the block class has no member called {name}");

	public static int At(string name) => Member(name).At;

	/// <summary>
	///     A value in the state, by the path it sits at: the member's own name under the names of
	///     whatever holds it, so <c>directData.destroySpeed</c> is the destroy speed inside the
	///     direct data. The position is the reference's, added up through the containers.
	/// </summary>
	public static int StateAt(string path) =>
		States.Leaves.FirstOrDefault(n => n.Path == path)?.At
		?? throw new InvalidOperationException($"the state class has no value at {path}");
}
