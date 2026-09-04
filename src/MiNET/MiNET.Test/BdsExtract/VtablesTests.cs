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
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiNET.BdsExtract.Binary;
using Newtonsoft.Json.Linq;

namespace MiNET.Test.BdsExtract
{
	[TestClass]
	public class VtablesTests
	{
		/// <summary>
		///     The owner whose function constructs the item component. cereal::internal::TypeSchema
		///     &lt;X&gt;::doLoad allocates the component and stores its table into the object's first
		///     word; the other owners that name the same class build the schema machinery around it
		///     instead, so feeding all of them in makes every class ambiguous.
		/// </summary>
		private static List<InventoryClass> ItemComponents(IReadOnlyList<InventoryClass> classes)
		{
			return classes.Where(c => c.Name.EndsWith("ItemComponent") && c.Family == $"cereal::internal::TypeSchema<{c.Name}>").ToList();
		}

		[TestMethod]
		public void Vtables_resolve_and_sizes_match_the_live_reads()
		{
			var image = PeImage.Open(PeImageTests.ExePath());
			var code = new CodeIndex(image);
			IReadOnlyList<InventoryClass> classes = SignatureStrings.Find(image);
			IReadOnlyList<ClassVtable> vtables = Vtables.Find(code, image, ItemComponents(classes));

			ClassVtable food = vtables.Single(v => v.Name == "FoodItemComponent");
			Assert.AreEqual(96, food.Size, "measured live: every food object is 96 bytes");
			Assert.IsTrue(food.Networked == true, $"slot 3 read as {food.NetworkedBytes}");
			Assert.AreEqual(1, food.Witnesses);

			Assert.IsTrue(vtables.Count(v => v.VtableRva != 0) >= 46, $"{vtables.Count(v => v.VtableRva != 0)} of {vtables.Count} item component classes resolved a vtable");
			foreach (ClassVtable vtable in vtables.OrderBy(v => v.Name, StringComparer.Ordinal))
			{
				Console.WriteLine($"{vtable.Name,-52} 0x{vtable.VtableRva:x7} witnesses {vtable.Witnesses} size {vtable.Size,4} sizes [{string.Join(",", vtable.SizeCandidates)}] networked {(vtable.Networked?.ToString() ?? "null/" + vtable.NetworkedBytes)} candidates [{string.Join(",", vtable.Candidates.Select(c => "0x" + c.ToString("x")))}]");
			}
		}

		/// <summary>
		///     A destructor that frees heap members before itself hands their sizes out first. The
		///     object's own free is the one behind the deleting-flag test, so that is the size named,
		///     whatever position it sits at; without such a test nothing is guessed.
		/// </summary>
		[TestMethod]
		public void The_own_size_is_the_flagged_free_not_the_first()
		{
			static byte[] Free(int size) => [0xBA, (byte) size, 0, 0, 0, 0xE8, 0, 0, 0, 0];

			// The 1.26.60.21 shape, clang-cl: `mov edi, edx` parks the flag, a list walk frees two
			// 120-byte entries behind a `test rbx, rbx` null check, then `test edi, edi; jz; mov edx,
			// 72; mov rcx, rsi; call` frees the object. The null check must not name the entry free.
			byte[] prologue = [0x56, 0x57, 0x48, 0x83, 0xEC, 0x28, 0x89, 0xD7, 0x48, 0x89, 0xCE];
			byte[] nullCheck = [0x48, 0x85, 0xDB, 0x74, 0x26];
			byte[] own = [0x85, 0xFF, 0x74, 0x0D, 0xBA, 72, 0, 0, 0, 0x48, 0x89, 0xF1, 0xE8, 0, 0, 0, 0];
			(List<int> sizes, int index) = Vtables.DestructorSizes([.. prologue, .. nullCheck, .. Free(120), .. Free(120), .. own, .. Free(120)]);
			CollectionAssert.AreEqual(new[] {120, 120, 72, 120}, sizes);
			Assert.AreEqual(2, index);
			Assert.AreEqual(72, Vtables.OwnSize((sizes, index)));

			// MSVC's shape: `mov ebx, edx` then `test bl, 1`; and the flag left in edx, `test dl, 1`.
			Assert.AreEqual(1, Vtables.DestructorSizes([0x89, 0xD3, .. Free(120), 0xF6, 0xC3, 0x01, 0x74, 0x05, .. Free(40)]).Own);
			Assert.AreEqual(0, Vtables.DestructorSizes([0x48, 0x89, 0xCE, 0xF6, 0xC2, 0x01, 0x74, 0x05, .. Free(40)]).Own);

			// A flag parked in r14d (`mov r14d, edx`, REX.B) is tested as `test r14d, r14d` (REX.RB).
			Assert.AreEqual(1, Vtables.DestructorSizes([0x41, 0x89, 0xD6, .. Free(120), 0x45, 0x85, 0xF6, 0x74, 0x05, .. Free(56)]).Own);

			// No flag test at all: a lone candidate is the class, candidates that all agree state
			// their one value, candidates that differ are unstated.
			Assert.AreEqual(40, Vtables.OwnSize(Vtables.DestructorSizes(Free(40))));
			Assert.AreEqual(80, Vtables.OwnSize(Vtables.DestructorSizes([0x89, 0xD7, .. Free(80), .. Free(80)])));
			Assert.AreEqual(0, Vtables.OwnSize(Vtables.DestructorSizes([0x89, 0xD7, .. Free(120), .. Free(72)])));
		}

		/// <summary>
		///     The one block component on this image whose destructor frees members first: its
		///     materials are a list of 120-byte entries, and its own node is 72.
		/// </summary>
		[TestMethod]
		public void Material_instances_size_is_its_own_node_not_an_entry()
		{
			var image = PeImage.Open(PeImageTests.ExePath());
			var code = new CodeIndex(image);
			List<InventoryClass> blocks = SignatureStrings.Find(image).Where(c => c.Family == "BlockComponentStorage").ToList();
			IReadOnlyList<ClassVtable> vtables = Vtables.Find(code, image, blocks);

			ClassVtable materials = vtables.Single(v => v.Name == "BlockMaterialInstancesComponent");
			Console.WriteLine($"candidates [{string.Join(", ", materials.SizeCandidates)}], own {materials.Size}");
			Assert.AreEqual(72, materials.Size);
			Assert.IsTrue(materials.SizeCandidates.Contains(120), "the entry frees are still listed, in order");
		}

		/// <summary>
		///     The block component storage folds identical wrappers, so a table can name more than
		///     one class and every class in the fold has to say so. Seventeen classes whose data is
		///     eight bytes or less share one table on 1.26.50.26.
		/// </summary>
		[TestMethod]
		public void Folded_tables_list_every_class_that_shares_them()
		{
			var image = PeImage.Open(PeImageTests.ExePath());
			var code = new CodeIndex(image);
			List<InventoryClass> blocks = SignatureStrings.Find(image).Where(c => c.Family == "BlockComponentStorage").ToList();
			IReadOnlyList<ClassVtable> vtables = Vtables.Find(code, image, blocks);

			ClassVtable obstruction = vtables.Single(v => v.Name == "BlockChestObstructionComponent");
			Assert.AreNotEqual(0u, obstruction.VtableRva);
			Assert.IsTrue(obstruction.SharedWith.Contains("BlockContainerComponent"), $"shared with [{string.Join(", ", obstruction.SharedWith)}]");

			foreach (IGrouping<uint, ClassVtable> group in vtables.Where(v => v.VtableRva != 0).GroupBy(v => v.VtableRva).Where(g => g.Count() > 1).OrderBy(g => g.Key))
			{
				Console.WriteLine($"0x{group.Key:x7}: {string.Join(", ", group.Select(v => v.Name).OrderBy(n => n, StringComparer.Ordinal))}");
			}
			foreach (ClassVtable vtable in vtables.Where(v => v.VtableRva == 0))
			{
				Console.WriteLine($"unresolved: {vtable.Name} candidates [{string.Join(", ", vtable.Candidates.Select(c => "0x" + c.ToString("x")))}]");
			}
		}

		/// <summary>
		///     The same tables a previous run read out of live item objects, RVA for RVA. The scan's
		///     addresses are process addresses of a relocated module, so its own moduleBase comes off
		///     first. Every difference is printed; which of them matters is not this test's call.
		///     <para>
		///         Addresses move with every build, so the scan is only a witness for the image it
		///         was read on: it has to carry that image's SHA-256 as exeSha256, and an exe with
		///         another digest is not a disagreement, it is a different question. A scan without
		///         the digest cannot say which build it belongs to and is not measured against any.
		///     </para>
		/// </summary>
		[TestMethod]
		public void The_vtables_agree_with_the_live_scan()
		{
			string path = TypeIdSlotsTests.RepoFile(Path.Combine("temp_auto", "widths", "vtables.json"));
			if (path == null) Assert.Inconclusive("temp_auto/widths/vtables.json is not in this checkout");

			JObject scan = JObject.Parse(File.ReadAllText(path));
			string scanned = (string) scan["exeSha256"];
			if (string.IsNullOrEmpty(scanned)) Assert.Inconclusive("the scan carries no exeSha256, so nothing says which build its addresses belong to");
			string digest = BinaryFacts.Digest(PeImageTests.ExePath());
			if (!string.Equals(scanned, digest, StringComparison.OrdinalIgnoreCase)) Assert.Inconclusive($"the scan was read on an image with digest {scanned}; MINET_BDS_EXE has {digest}");

			var image = PeImage.Open(PeImageTests.ExePath());
			var code = new CodeIndex(image);
			IReadOnlyList<ClassVtable> vtables = Vtables.Find(code, image, ItemComponents(SignatureStrings.Find(image)));

			ulong moduleBase = Convert.ToUInt64((string) scan["moduleBase"], 16);
			var live = new Dictionary<string, uint>();
			foreach (JProperty property in ((JObject) scan["classes"]).Properties())
			{
				live[property.Name] = (uint) (Convert.ToUInt64((string) property.Value[0][0], 16) - moduleBase);
			}

			int agree = 0;
			var differences = new List<string>();
			foreach (KeyValuePair<string, uint> entry in live.OrderBy(e => e.Key, StringComparer.Ordinal))
			{
				ClassVtable vtable = vtables.FirstOrDefault(v => v.Name == entry.Key);
				if (vtable == null) differences.Add($"{entry.Key}: no cereal::internal::TypeSchema<{entry.Key}> literal in the image, the scan read 0x{entry.Value:x}");
				else if (vtable.VtableRva == 0) differences.Add($"{entry.Key}: unresolved from the binary, candidates [{string.Join(", ", vtable.Candidates.Select(c => "0x" + c.ToString("x")))}], the scan read 0x{entry.Value:x}");
				else if (vtable.VtableRva != entry.Value) differences.Add($"{entry.Key}: binary 0x{vtable.VtableRva:x}, scan 0x{entry.Value:x}");
				else agree++;
			}
			foreach (string difference in differences) Console.WriteLine(difference);
			Console.WriteLine($"{agree} agree, {differences.Count} differ of {live.Count}");
			Assert.IsTrue(agree >= 21, $"only {agree} of {live.Count} tables agree with the live scan");
		}

		/// <summary>
		///     The size the deleting destructor states against the size the promoted reference
		///     states, class by class. The reference gives a component class its base and its own
		///     size; the two added are the whole object the destructor deallocates.
		/// </summary>
		[TestMethod]
		public void The_class_sizes_agree_with_the_reference()
		{
			var image = PeImage.Open(PeImageTests.ExePath());
			var code = new CodeIndex(image);
			IReadOnlyList<ClassVtable> vtables = Vtables.Find(code, image, ItemComponents(SignatureStrings.Find(image)));

			string reference = TypeIdSlotsTests.RepoFile(Path.Combine("src", "MiNET", "MiNET.BdsExtract", "Assets", "reference", "items-runtime.json"));
			if (reference == null) Assert.Inconclusive("Assets/reference/items-runtime.json is not in this checkout");

			int agree = 0;
			var differences = new List<string>();
			var unresolved = new List<string>();
			foreach (JProperty property in ((JObject) JObject.Parse(File.ReadAllText(reference))["classes"]).Properties())
			{
				int size = (int) property.Value["size"];
				int declaredBase = property.Value["base"] == null ? 0 : (int) property.Value["base"];
				ClassVtable vtable = vtables.FirstOrDefault(v => v.Name == property.Name && v.VtableRva != 0);
				if (vtable == null) { unresolved.Add(property.Name); continue; }
				if (vtable.Size == size + declaredBase) agree++;
				else differences.Add($"{property.Name}: reference base {declaredBase} + size {size} = {size + declaredBase}, binary {vtable.Size}, candidates [{string.Join(",", vtable.SizeCandidates)}]");
			}
			foreach (string difference in differences) Console.WriteLine(difference);
			Console.WriteLine($"{agree} agree, {differences.Count} differ, {unresolved.Count} with no binary vtable: {string.Join(", ", unresolved)}");
			Assert.IsTrue(agree >= 22, $"only {agree} reference class sizes agree with the destructor");
		}
	}
}
