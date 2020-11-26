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
			// Picked block minecraft:cobblestone_wall from blockstate 3313
			int runtimeId = 917;

			BlockStateContainer blocStateFromPick = BlockFactory.BlockPalette[runtimeId];
			var block = BlockFactory.GetBlockById(blocStateFromPick.Id) as Chain;
			Assert.IsNotNull(block);
			block.SetState(blocStateFromPick.States);

			Item item = block.GetItem();
			Assert.AreEqual("minecraft:chain", item.Name);
			Assert.AreEqual(541, item.Id);
			Assert.AreEqual(1, item.Metadata);
		}
	}
}