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
using fNbt;
using MiNET.Items;
using MiNET.Net;

namespace MiNET.Test.Items
{
	/// <summary>
	///     An item's identity is its registry name; the network id is only what this protocol version
	///     numbered it. Everything here guards that: if a lookup lands on a different identity than
	///     the one asked for, the server hands the client the wrong item, silently.
	/// </summary>
	[TestClass]
	public class ItemRegistryTests
	{
		/// <summary>
		///     Building an item by name must produce that name and the registry's number for it. The
		///     bug this exists for: typed item classes were constructed with the network id passed into
		///     the slot that meant the legacy id, so 113 of them encoded as an unrelated item on the
		///     wire (an acacia boat went out as a rabbit).
		/// </summary>
		[TestMethod]
		public void EveryRegistryNameResolvesToItsOwnIdentity()
		{
			var wrong = new List<string>();

			foreach (ItemRegistryEntry entry in ItemFactory.ItemRegistry)
			{
				Item item = ItemFactory.GetItemByName(entry.Name);

				if (!string.Equals(item.Name, entry.Name, StringComparison.OrdinalIgnoreCase))
				{
					wrong.Add($"{entry.Name} -> {item.Name} ({item.GetType().Name})");
				}
				else if (item.NetworkId != entry.NetworkId)
				{
					wrong.Add($"{entry.Name} network id {item.NetworkId}, registry says {entry.NetworkId}");
				}
			}

			Assert.AreEqual(0, wrong.Count, $"{wrong.Count} of {ItemFactory.ItemRegistry.Count} items resolve wrong: " + string.Join(", ", wrong.Take(10)));
		}

		/// <summary>The decode direction: a network id off the wire has to come back as the same item.</summary>
		[TestMethod]
		public void EveryNetworkIdResolvesBackToItsName()
		{
			var wrong = new List<string>();

			foreach (ItemRegistryEntry entry in ItemFactory.ItemRegistry)
			{
				Item item = ItemFactory.GetItemByNetworkId(entry.NetworkId);
				if (!string.Equals(item.Name, entry.Name, StringComparison.OrdinalIgnoreCase)) wrong.Add($"{entry.NetworkId} -> {item.Name}, expected {entry.Name}");
			}

			Assert.AreEqual(0, wrong.Count, "network ids resolving to the wrong name: " + string.Join(", ", wrong.Take(10)));
		}

		/// <summary>
		///     Two identities must never share a typed class. A class serving two names means one of
		///     them is silently answered as the other, which is how the id-space mix-up hid.
		/// </summary>
		[TestMethod]
		public void NoTypedClassServesTwoIdentities()
		{
			var byType = new Dictionary<Type, string>();
			var collisions = new List<string>();

			foreach (ItemRegistryEntry entry in ItemFactory.ItemRegistry)
			{
				Item item = ItemFactory.GetItemByName(entry.Name);
				Type type = item.GetType();

				// Block items all share ItemBlock, and anything unmodelled shares Item; they carry
				// their identity in Name rather than in the class, so they are not collisions.
				if (type == typeof(Item) || type == typeof(ItemBlock) || type == typeof(GenericFoodItem)) continue;

				if (byType.TryGetValue(type, out string first)) collisions.Add($"{type.Name}: {first} and {entry.Name}");
				else byType[type] = entry.Name;
			}

			Assert.AreEqual(0, collisions.Count, "typed classes serving more than one identity: " + string.Join(", ", collisions.Take(10)));
		}

		/// <summary>
		///     Air has a registry entry because it is a block, but an empty slot goes out as network id
		///     0, which no real item uses. Confusing the two puts a stack of air in someone's inventory.
		/// </summary>
		[TestMethod]
		public void AirIsAnEmptySlotNotAnItem()
		{
			Assert.IsTrue(new ItemAir().IsAir);
			Assert.IsTrue(ItemFactory.GetItemByName("minecraft:air").IsAir);
			Assert.IsFalse(ItemFactory.GetItemByName("minecraft:stone").IsAir);

			Assert.AreEqual(0, ItemFactory.GetNetworkIdByName("minecraft:this_item_does_not_exist"));
			Assert.IsFalse(ItemFactory.ItemRegistry.Any(e => e.NetworkId == 0), "an item numbered 0 would be indistinguishable from an empty slot");
		}

		/// <summary>
		///     The sugar cane block is "minecraft:reeds" and has no item of that name; the item was
		///     renamed to "minecraft:sugar_cane". Without following the rename it resolves to nothing
		///     and the stack goes out empty.
		/// </summary>
		[TestMethod]
		public void RenamedBlockItemLandsOnTheCurrentIdentity()
		{
			Item item = ItemFactory.GetItemByName("minecraft:reeds");

			Assert.AreEqual("minecraft:sugar_cane", item.Name);
			Assert.AreNotEqual(0, item.NetworkId);
		}


		/// <summary>
		///     Block entities store items as NBT, and six of them used to disagree about the shape:
		///     four wrote "id" as a legacy short while two wrote the modern "Name". Reading has to
		///     accept both, because an old world still holds the legacy form, and writing has to
		///     always produce the modern one so a world converts itself the first time it is saved.
		/// </summary>
		[TestMethod]
		public void ItemNbtReadsBothShapesAndWritesTheModernOne()
		{
			Item apple = ItemFactory.GetItemByName("minecraft:apple", 0, 7);

			NbtCompound written = ItemNbt.Write(apple, "Item");
			Assert.AreEqual("Item", written.Name);
			Assert.AreEqual("minecraft:apple", written["Name"].StringValue);
			Assert.AreEqual(7, written["Count"].ByteValue);
			Assert.IsNull(written["id"], "the legacy short must never be written again");

			Item roundTripped = ItemNbt.Read(written);
			Assert.AreEqual(apple.Name, roundTripped.Name);
			Assert.AreEqual(apple.Count, roundTripped.Count);
			Assert.AreEqual(apple.NetworkId, roundTripped.NetworkId);

			// The legacy shape an old save still holds: "id" as a pre-flattening short.
			var legacy = new NbtCompound("Item")
			{
				new NbtShort("id", 260),
				new NbtShort("Damage", 0),
				new NbtByte("Count", 3)
			};

			Item upgraded = ItemNbt.Read(legacy);
			Assert.AreEqual("minecraft:apple", upgraded.Name, "legacy id 260 is the apple");
			Assert.AreEqual(3, upgraded.Count);

			// Writing it back drops the legacy form for good.
			Assert.IsNull(ItemNbt.Write(upgraded)["id"]);

			Assert.IsTrue(ItemNbt.Read(null).IsAir);
			Assert.IsTrue(ItemNbt.Read(new NbtCompound("Item")).IsAir);
		}


		/// <summary>
		///     An empty slot goes on the wire as network id 0. Air is a real registry entry (-158),
		///     so resolving an empty stack by name yields a genuine item id and the client is told
		///     the slot holds air rather than nothing. Every writer has to ask IsAir first.
		/// </summary>
		[TestMethod]
		public void EmptyStacksEncodeAsNothingNotAsAir()
		{
			Assert.AreNotEqual(0, ItemFactory.GetNetworkIdByName("minecraft:air"),
				"if air were 0 this whole class of bug would be impossible; it is not, so the guard matters");

			foreach (Item empty in new[] {new ItemAir(), ItemFactory.GetItemByName("minecraft:air")})
			{
				Assert.IsTrue(empty.IsAir);

				var packet = McpeMobEquipment.CreateObject();
				packet.item = empty;
				byte[] bytes = packet.Encode();
				packet.PutPool();

				var read = McpeMobEquipment.CreateObject();
				read.Decode(bytes);
				Assert.IsTrue(read.item.IsAir, "an empty hand decoded as a real item");
				read.PutPool();
			}
		}

		/// <summary>
		///     A pre-flattening (id, meta) pair out of an old save has to land on a modern identity.
		///     The metadata-split case is the interesting one: the old id alone is ambiguous and the
		///     metadata is what picks the identity, after which it no longer means anything.
		/// </summary>
		[TestMethod]
		public void LegacyIdsUpgradeToModernIdentities()
		{
			// Plain item id, metadata is durability and survives.
			Assert.AreEqual("minecraft:apple", LegacyItemUpgrader.Upgrade(260).Name);

			// Renamed identity.
			Assert.AreEqual("minecraft:enchanted_golden_apple", LegacyItemUpgrader.Upgrade(466).Name);

			// Metadata split: one old id, several modern names.
			Item pattern = LegacyItemUpgrader.Upgrade(434, 3);
			Assert.AreEqual("minecraft:mojang_banner_pattern", pattern.Name);
			Assert.AreEqual(0, pattern.Metadata, "the metadata was consumed by the split and must not survive it");

			// Below 256 the number is a block id, not an item id.
			Assert.AreEqual("minecraft:stone", LegacyItemUpgrader.Upgrade(1).Name);

			Assert.IsTrue(LegacyItemUpgrader.Upgrade(0).IsAir);
		}
	}
}
