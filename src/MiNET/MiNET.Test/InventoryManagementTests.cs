using System;
using System.Collections.Generic;
using System.Net;
using MiNET.Blocks;
using MiNET.Items;
using MiNET.Worlds;
using Moq;
using NUnit.Framework;

namespace MiNET
{
	[TestFixture]
	public class InventoryManagementTests
	{
		[Test]
		public void CreateInventoryForPlayerTest()
		{
			var server = new Mock<MiNetServer>();
			var worldProvider = new Mock<IWorldProvider>();
			var level = new Mock<Level>("level", worldProvider.Object);

			Player player = new Player(server.Object, new IPEndPoint(IPAddress.Loopback, 0), level.Object, 1500);
			Player playerOther = new Player(server.Object, new IPEndPoint(IPAddress.Loopback, 0), level.Object, 1500);

			// Create an inventory manager for all players, all entities
			InventoryManager inventoryManager = new InventoryManager();
			Assert.IsNotNull(inventoryManager);

			// Create an invnetory for a specific entity (player)
			Inventory inventory = inventoryManager.GetInventory(player);
			Assert.IsNotNull(inventory);

			// Create another inventory for another specific entity (player)
			Inventory inventoryOther = inventoryManager.GetInventory(playerOther);
			Assert.IsNotNull(inventoryOther);

			Assert.AreNotSame(inventory, inventoryOther);
			Assert.AreNotEqual(inventory.GetItem(0).Id, inventoryOther.GetItem(0).Id); // BUG: Fixme!

			// Add Item(s) to the inventory
			ItemBlock chest = new ItemBlock(BlockFactory.GetBlockById(54));
			ItemBlock door = new ItemBlock(BlockFactory.GetBlockById(64));
			Assert.IsNotNull(inventory);
			inventory.SetItem(0, chest);
			inventory.SetItem(1, door);
			inventory.SetItem(1, door); // should work

			// Get the items back
			ItemBlock item = inventory.GetItem(0);
			Assert.IsNotNull(item);
			Assert.AreEqual(chest.Id, item.Id);

			item = inventory.GetItem(1);
			Assert.IsNotNull(item);
			Assert.AreEqual(door.Id, item.Id);

			// Limit the size
			for (byte i = 0; i < 35; i++)
			{
				inventory.SetItem(i, new ItemBlock(BlockFactory.GetBlockById(i)));
			}

			try
			{
				inventory.SetItem(36, new ItemBlock(BlockFactory.GetBlockById(35)));
				Assert.Fail("Max limit exceeded");
			}
			catch (IndexOutOfRangeException e)
			{
			}


			// Getting the inventory again, should give back the same content of the
			// iventory, but not necesarily the same instance (depends on the impl)
			Inventory inventorySecond = inventoryManager.GetInventory(player);
			Assert.IsNotNull(inventoryOther);
			//Assert.AreNotSame(inventory, inventorySecond); // May be the same

			Assert.AreEqual(inventory.GetItem(0).Id, inventorySecond.GetItem(0).Id); // BUG: Fixme!
		}
	}

	public class Inventory
	{
		private const int MaxNumberOfSlots = 36;
		private Dictionary<int, ItemBlock> _items = new Dictionary<int, ItemBlock>();

		public void SetItem(int slot, ItemBlock item)
		{
			if (slot >= MaxNumberOfSlots) 
				throw new IndexOutOfRangeException(string.Format("Maximum allowed slots exceeded. Max: {0}", MaxNumberOfSlots));

			_items[slot] = item;
		}

		public ItemBlock GetItem(int slot)
		{
			return _items[slot];
		}
	}

	public class InventoryManager
	{
		private Dictionary<Player, Inventory> _inventories;

		public InventoryManager()
		{
			_inventories = new Dictionary<Player, Inventory>();
		}

		public Inventory GetInventory(Player player)
		{
			Inventory inventory;
			if (_inventories.TryGetValue(player, out inventory))
			{
				return inventory;
			}

			inventory = new Inventory();
			_inventories[player] = inventory;

			return inventory;
		}
	}
}