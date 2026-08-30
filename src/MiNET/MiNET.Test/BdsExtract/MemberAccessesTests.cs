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
using System.Linq;
using Iced.Intel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiNET.BdsExtract.Binary;

namespace MiNET.Test.BdsExtract
{
	[TestClass]
	public class MemberAccessesTests
	{
		[TestMethod]
		public void autoSucceedOnClient_is_a_byte_at_0x40()
		{
			var image = PeImage.Open(PeImageTests.ExePath());
			var code = new CodeIndex(image);
			var accesses = MemberAccesses.Find(code, image, new[] { "autoSucceedOnClient" });
			MemberAccess store = accesses.Single(a => a.WireName == "autoSucceedOnClient" && a.IsStore);
			Assert.AreEqual(0x40, store.Offset);
			Assert.AreEqual(1, store.Width);
			Assert.AreEqual("RSI", store.Register);
			foreach (MemberAccess a in accesses) Console.WriteLine($"fn=0x{a.FunctionRva:x} at=0x{a.InstructionRva:x} +{a.Offset} w={a.Width} {a.Operand} store={a.IsStore} reg={a.Register}");
		}

		/// <summary>
		///     The second anchor: the class's own serializer, reached through its vtable rather than
		///     through a name literal. Slot 4 is buildNetworkTag and slot 5 is initializeFromNetwork,
		///     and both take the object in RCX: the hidden return slot follows `this` rather than
		///     preceding it, which the code states outright, buildNetworkTag writing the tag it
		///     built through the second argument and reading the member off the first.
		///     OnUseOnItemComponent's vtable is at this RVA on 1.26.50.26 (process address
		///     0x7ff71c459ce0 less the module base 0x7ff7108b0000).
		/// </summary>
		[TestMethod]
		public void The_vtable_serializer_reaches_the_same_member_without_the_name()
		{
			var image = PeImage.Open(PeImageTests.ExePath());
			var code = new CodeIndex(image);
			var accesses = MemberAccesses.FindByVtableSerializer(code, image, 0xbba9ce0, 4, Register.RCX, "OnUseOnItemComponent");
			Assert.IsTrue(accesses.Count > 0, "the serializer touched nothing");
			Assert.IsTrue(accesses.Any(a => a.Offset == 0x40 && a.Width == 1), $"no byte at +0x40 among {accesses.Count} accesses");
			foreach (MemberAccess a in accesses.OrderBy(a => a.Offset)) Console.WriteLine($"fn=0x{a.FunctionRva:x} at=0x{a.InstructionRva:x} +{a.Offset} w={a.Width} {a.Operand} store={a.IsStore} reg={a.Register}");
		}

		/// <summary>
		///     The third anchor: a constructor, read from its own RVA with the object in RCX. Item's
		///     constructor writes the fields the class declares, so the offsets it touches are the
		///     ones the reference states for Item.
		/// </summary>
		[TestMethod]
		public void The_Item_constructor_writes_the_offsets_the_reference_declares()
		{
			var image = PeImage.Open(PeImageTests.ExePath());
			var code = new CodeIndex(image);
			var accesses = MemberAccesses.FindByConstructor(code, image, 0x1cf2510, Register.RCX, "Item");
			Assert.IsTrue(accesses.Count > 20, $"only {accesses.Count} accesses in the constructor");
			Assert.IsTrue(accesses.Any(a => a.Offset == 170 && a.Width == 2 && a.IsStore), "no 2-byte store at +170, where the reference puts id");
			Console.WriteLine($"{accesses.Count} accesses, {accesses.Select(a => a.Offset).Distinct().Count()} distinct offsets");
		}
	}
}