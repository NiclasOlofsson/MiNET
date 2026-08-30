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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiNET.BdsExtract;
using MiNET.BdsExtract.Binary;

namespace MiNET.Test.BdsExtract
{
	[TestClass]
	public class BinaryCheckTests
	{
		/// <summary>
		///     The facts, read once for the whole class. Reading the image costs the same twenty
		///     seconds every time and every test here asks the same question of the same build.
		/// </summary>
		private static BinaryFacts _facts;

		private static BinaryCheck Checked()
		{
			string exe = PeImageTests.ExePath();
			if (BlockMembers.All(BlockMembers.Source.Items).Count == 0)
			{
				Assert.Inconclusive("the reference under MiNET.BdsExtract/Assets/reference is not reachable from here");
			}

			var seeds = new List<EnumSeed>();
			foreach (KeyValuePair<string, Dictionary<long, string>> stated in BlockMembers.Enums)
			{
				seeds.Add(new EnumSeed(stated.Key, stated.Value.ToDictionary(v => (int) v.Key, v => v.Value)));
			}

			_facts ??= BinaryFacts.Read(exe, BinaryFacts.WireNames(exe, new List<(string, string)>()), seeds, null);

			var check = new BinaryCheck(_facts);
			check.Run();
			Console.WriteLine(check.Summary);
			foreach (string difference in check.Differences) Console.WriteLine(difference);
			return check;
		}

		/// <summary>
		///     An item component is allocated on its own, so the size its deleting destructor hands
		///     the deallocator is the whole class and the reference's base plus its own size has to
		///     equal it. A class declared short reads the members after the hole under each other's
		///     names, which is how FoodItemComponent came out with two members swapped.
		/// </summary>
		[TestMethod]
		public void Every_declared_item_component_is_the_size_its_destructor_states()
		{
			BinaryCheck check = Checked();
			List<string> sizes = check.Differences
				.Where(d => d.StartsWith("size:", StringComparison.Ordinal))
				.Where(d => !d.Contains("nodeSize", StringComparison.Ordinal))
				.ToList();
			Assert.AreEqual(0, sizes.Count, string.Join("\n", sizes));
		}

		/// <summary>
		///     The binder states a member's place and its type, so a declared member the binder also
		///     names has to be that wide. A width the reference gets wrong is not visible in a round
		///     trip: max_stack_size read two bytes wide gave the black bundle a stack of 8,193.
		/// </summary>
		[TestMethod]
		public void Every_declared_member_a_primitive_binding_names_is_that_wide()
		{
			BinaryCheck check = Checked();
			List<string> widths = check.Differences
				.Where(d => d.StartsWith("member:", StringComparison.Ordinal))
				.Where(d => d.Contains(", a primitive)", StringComparison.Ordinal))
				.ToList();
			Assert.AreEqual(0, widths.Count, string.Join("\n", widths));
		}

		/// <summary>
		///     The check has to be able to fail. Every family it reports on has to have found
		///     something to compare, or a green run means the comparison never happened.
		/// </summary>
		[TestMethod]
		public void The_check_compares_all_four_families()
		{
			BinaryCheck check = Checked();
			Assert.IsTrue(check.Summary.Length > 0, "no summary line");
			Assert.IsTrue(check.Differences.Any(d => d.StartsWith("undeclared:", StringComparison.Ordinal)),
				"the binary names classes and members the reference does not declare; none was reported");
			Assert.IsTrue(check.Differences.Any(d => d.StartsWith("enum:", StringComparison.Ordinal)),
				"13 of the 16 seeded enums do not resolve in the binary; none was reported");
		}
	}
}
