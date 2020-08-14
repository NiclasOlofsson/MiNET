using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiNET.Blocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MiNET.Items;
using MiNET.Utils;

namespace MiNET.Blocks.Tests
{
	[TestClass()]
	public class BlockTests
	{
		[TestMethod()]
		public void GetItemFromBlockStateTest()
		{
			// Picked block minecraft:chain from blockstate 917
			int runtimeId = 917;

			BlockStateContainer blocStateFromPick = BlockFactory.BlockPalette[runtimeId];
			var block = BlockFactory.GetBlockById(blocStateFromPick.Id) as Chain;
			Assert.IsNotNull(block);
			block.SetState(blocStateFromPick.States);

			Item item = block.GetItem();

			Assert.AreEqual("minecraft:chain", item.Name);
			Assert.AreEqual(758, item.Id);
			Assert.AreEqual(0, item.Metadata);
		}

		[TestMethod()]
		public void GetDoorItemFromBlockStateTest()
		{
			// Picked block minecraft:dark_oak_door from blockstate 4003. Expected block to be in slot 9
			// Picked block minecraft:dark_oak_door from blockstate 3991. Expected block to be in slot 9
			int runtimeId = 3991;

			BlockStateContainer blocStateFromPick = BlockFactory.BlockPalette[runtimeId];
			var block = BlockFactory.GetBlockById(blocStateFromPick.Id) as DarkOakDoor;
			Assert.IsNotNull(block);
			block.SetState(blocStateFromPick.States);

			ItemBlock item = block.GetItem() as ItemBlock;
			Assert.IsNotNull(item, "Found no item");
			Assert.IsNotNull(item.Block);
			Assert.AreEqual("minecraft:dark_oak_door", item.Name);
			Assert.AreEqual(431, item.Id);
			Assert.AreEqual(0, item.Metadata);
		}

		[TestMethod()]
		public void GetRuntimeIdFromBlockStateTest()
		{
			var block = new DoublePlant();
			block.DoublePlantType = "grass";
			block.UpperBlockBit = true;

			Assert.AreEqual(0, block.GetRuntimeId());
		}
	}
}