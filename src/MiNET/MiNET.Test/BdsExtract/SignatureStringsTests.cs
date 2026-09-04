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
using MiNET.BdsExtract.Binary;

namespace MiNET.Test.BdsExtract
{
	[TestClass]
	public class SignatureStringsTests
	{
		[TestMethod]
		public void Every_block_component_class_is_named_by_a_signature_string()
		{
			var image = PeImage.Open(PeImageTests.ExePath());
			IReadOnlyList<InventoryClass> classes = SignatureStrings.Find(image);
			var blocks = classes.Where(c => c.Family == "BlockComponentStorage").Select(c => c.Name).ToHashSet();
			Assert.IsTrue(blocks.Contains("BlockChestObstructionComponent"));
			Assert.IsTrue(blocks.Contains("BlockDestructibleByMiningComponent"));
			Assert.IsTrue(blocks.Count >= 58, $"{blocks.Count} block component classes; the 1.26.50.26 scan found 58");
			Assert.IsTrue(classes.All(c => c.SignatureRvas.Count >= 1));

			foreach (IGrouping<string, InventoryClass> family in classes.GroupBy(c => c.Family).OrderByDescending(g => g.Select(c => c.Name).Distinct().Count()))
			{
				Console.WriteLine($"{family.Select(c => c.Name).Distinct().Count(),6}  {family.Key}");
			}
		}

		/// <summary>
		///     The item components have no storage template of their own on this build. They are
		///     named through cereal and entt, whose parameter is spelled Type rather than T, and the
		///     owner that carries each class's construction is cereal::internal::TypeSchema&lt;X&gt;.
		///     Five of the classes the reference declares are named by no literal at all, so the
		///     inventory is a floor for the item side, not the whole of it.
		/// </summary>
		[TestMethod]
		public void The_item_components_are_named_by_the_cereal_type_schema_owner()
		{
			var image = PeImage.Open(PeImageTests.ExePath());
			IReadOnlyList<InventoryClass> classes = SignatureStrings.Find(image);

			foreach (string name in new[] { "FoodItemComponent", "UseModifiersItemComponent" })
			{
				var owners = classes.Where(c => c.Name == name).Select(c => c.Family).ToList();
				Assert.IsTrue(owners.Contains($"cereal::internal::TypeSchema<{name}>"), $"{name} owners: {string.Join(", ", owners)}");
				Assert.IsTrue(owners.Contains("entt::internal"), $"{name} owners: {string.Join(", ", owners)}");
			}

			var schema = classes.Where(c => c.Family == $"cereal::internal::TypeSchema<{c.Name}>" && c.Name.EndsWith("ItemComponent")).ToList();
			Assert.IsTrue(schema.Count >= 22, $"{schema.Count} item component classes under cereal::internal::TypeSchema<X>");
			foreach (InventoryClass entry in schema.OrderBy(c => c.Name, StringComparer.Ordinal)) Console.WriteLine($"{entry.Name}: {entry.SignatureRvas.Count} literals");
		}
	}
}
