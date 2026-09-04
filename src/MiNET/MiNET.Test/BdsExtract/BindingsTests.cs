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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiNET.BdsExtract.Binary;

namespace MiNET.Test.BdsExtract
{
	[TestClass]
	public class BindingsTests
	{
		private static readonly string[] SelfProof =
		{
			"use_duration", "emit_vibrations", "start_using", "seconds_to_destroy",
			"nutrition", "saturation_modifier", "can_always_eat", "value", "slot",
			"autoSucceedOnClient", "max_stack_size", "creative_category"
		};

		/// <summary>
		///     The offsets are the member's place in the whole object, which is what the getter adds:
		///     the reference states the same member as a base plus a place inside the derived part,
		///     so FoodItemComponent's nutrition at 0 with base 24 is +24 here.
		/// </summary>
		[TestMethod]
		public void The_registration_states_each_member_offset_type_and_width()
		{
			var image = PeImage.Open(PeImageTests.ExePath());
			var code = new CodeIndex(image);
			IReadOnlyList<Binding> bindings = Bindings.Find(code, image, SelfProof, BinaryFacts.TypeNames(image));

			foreach (Binding b in bindings.Where(b => SelfProof.Contains(b.WireName)))
			{
				Console.WriteLine($"{b.WireName,-20} {b.Owner ?? "(no owner)"} +{b.Offset} {b.TypeName ?? $"0x{b.TypeHash:x8}"} width {b.Width}");
			}

			Expect(bindings, "use_duration", "UseModifiersItemComponent", 16, "float", 4);
			Expect(bindings, "emit_vibrations", "UseModifiersItemComponent", 20, "bool", 1);
			Expect(bindings, "start_using", "UseModifiersItemComponent", 80, "UseModifiersItemComponent::StartUsing", 1);
			Expect(bindings, "seconds_to_destroy", "BlockDestructibleByMiningDescription", 16, "float", 4);
			Expect(bindings, "nutrition", "FoodItemComponent", 24, "int", 4);
			Expect(bindings, "saturation_modifier", "FoodItemComponent", 28, "float", 4);
			Expect(bindings, "can_always_eat", "FoodItemComponent", 48, "bool", 1);
			Expect(bindings, "value", "EnchantableItemComponent", 16, "unsigned char", 1);

			// The one member of the self-proof whose width nothing in the binding measures: a
			// std::string is built by a call rather than written by one store, so the width is 0 and
			// the reason travels with it instead of a number nothing stated.
			Expect(bindings, "slot", "EnchantableItemComponent", 24, "std::basic_string<char>", 0);
		}

		/// <summary>
		///     Three names the plan expected to find and the binary does not bind, each for its own
		///     reason. autoSucceedOnClient is the class's own member name, which only the hand
		///     written buildNetworkTag and initializeFromNetwork use; max_stack_size and
		///     creative_category are wire names no registration in this build carries. A name with
		///     no binding comes back with no rows, never with a row the tool made up.
		/// </summary>
		[TestMethod]
		public void A_name_the_binder_never_registers_yields_no_binding()
		{
			var image = PeImage.Open(PeImageTests.ExePath());
			var code = new CodeIndex(image);
			var names = new[] { "autoSucceedOnClient", "max_stack_size", "creative_category" };
			IReadOnlyList<Binding> bindings = Bindings.Find(code, image, names, BinaryFacts.TypeNames(image));

			Assert.AreEqual(0, bindings.Count(b => b.WireName == "autoSucceedOnClient"), "autoSucceedOnClient is bound after all");
			Assert.AreEqual(0, bindings.Count(b => b.WireName == "creative_category"), "creative_category is bound after all");

			// max_stack_size is bound, but on a mob goal rather than on MaxStackSizeItemComponent.
			Assert.IsFalse(bindings.Any(b => b.WireName == "max_stack_size" && b.Owner != null && b.Owner.Contains("MaxStackSize")),
				"MaxStackSizeItemComponent binds max_stack_size after all");
			foreach (Binding b in bindings) Console.WriteLine($"{b.WireName} {b.Owner ?? "(no owner)"} +{b.Offset} {b.TypeName}");
		}

		/// <summary>
		///     Every id the getters state has to reproduce a name the dictionary was given. An id
		///     that reproduces none is not a defect in itself, but it is the count that says how much
		///     of the build the dictionary covers, so it is asserted rather than watched.
		/// </summary>
		[TestMethod]
		public void Every_type_id_in_the_self_proof_reproduces_a_name()
		{
			var image = PeImage.Open(PeImageTests.ExePath());
			var code = new CodeIndex(image);
			IReadOnlyList<Binding> bindings = Bindings.Find(code, image, SelfProof, BinaryFacts.TypeNames(image));

			List<Binding> unresolved = bindings.Where(b => b.TypeName == null).ToList();
			foreach (Binding b in unresolved) Console.WriteLine($"unresolved: {b.WireName} 0x{b.TypeHash:x8} at 0x{b.SiteRva:x}");
			Assert.AreEqual(0, unresolved.Count, $"{unresolved.Count} of {bindings.Count} bindings name a type id no name reproduces");
		}

		/// <summary>FNV-1a 32 over the type's own name is what entt::type_hash computes, and the ids in the code are that and nothing else.</summary>
		[TestMethod]
		public void The_type_ids_are_FNV1a32_of_the_type_name()
		{
			Assert.AreEqual(0xa6c45d85, Bindings.TypeHash("float"));
			Assert.AreEqual(0xc894953d, Bindings.TypeHash("bool"));
			Assert.AreEqual(0x95e97e5eu, Bindings.TypeHash("int"));
			Assert.AreEqual(0x48b5725fu, Bindings.TypeHash("void"));
			Assert.AreEqual(0x7d773434u, Bindings.TypeHash("UseModifiersItemComponent"));
			Assert.AreEqual(0xcddccf90, Bindings.TypeHash("UseModifiersItemComponent::StartUsing"));
		}

		private static void Expect(IReadOnlyList<Binding> bindings, string wireName, string owner, int offset, string typeName, int width)
		{
			List<Binding> matching = bindings.Where(b => b.WireName == wireName && b.Owner == owner).ToList();
			Assert.AreEqual(1, matching.Count, $"{wireName} on {owner}: {matching.Count} bindings, one expected");
			Binding binding = matching[0];
			Assert.AreEqual(offset, binding.Offset, $"{wireName} offset; {binding.Evidence}");
			Assert.AreEqual(typeName, binding.TypeName, $"{wireName} type; {binding.Evidence}");
			Assert.AreEqual(width, binding.Width, $"{wireName} width; {binding.Evidence}");
		}
	}
}