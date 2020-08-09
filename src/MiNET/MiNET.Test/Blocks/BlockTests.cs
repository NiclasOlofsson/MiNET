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
			int runtimeId = 3313;

			BlockStateContainer blocStateFromPick = BlockFactory.BlockPalette[runtimeId];
			var block = BlockFactory.GetBlockById(blocStateFromPick.Id) as CobblestoneWall;
			Assert.IsNotNull(block);
			block.SetState(blocStateFromPick.States);

			Item item = block.GetItem();
			Assert.AreEqual("minecraft:cobblestone_wall", item.Name);
			Assert.AreEqual(139, item.Id);
			Assert.AreEqual(12, item.Metadata);
		}
	}
}