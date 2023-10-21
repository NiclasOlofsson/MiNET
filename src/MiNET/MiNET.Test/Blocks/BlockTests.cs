using Microsoft.VisualStudio.TestTools.UnitTesting;
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
			int runtimeId = 917;

			BlockStateContainer blocStateFromPick = BlockFactory.BlockPalette[runtimeId];
			var block = BlockFactory.GetBlockById(blocStateFromPick.Id);
			Assert.IsNotNull(block);
			block.SetState(blocStateFromPick.States);

			Item item = block.GetItem(null);

			Assert.AreEqual(blocStateFromPick.Id, item.Id);
			Assert.AreEqual(0, item.Metadata);
		}

		[TestMethod()]
		public void GetDoorItemFromBlockTest()
		{
			var block = BlockFactory.GetBlockById(BlockFactory.GetIdByType<DarkOakDoor>());

			ItemBlock item = block.GetItem(null) as ItemBlock;
			Assert.IsNotNull(item, "Found no item");
			Assert.IsInstanceOfType(item, typeof(ItemDarkOakDoor));

			Assert.IsNotNull(item.Block);
			Assert.IsInstanceOfType(item.Block, typeof(DarkOakDoor));
			Assert.AreEqual("minecraft:dark_oak_door", item.Id);
			Assert.AreEqual("minecraft:dark_oak_door", block.Id);
			Assert.AreEqual(0, item.Metadata);
		}

		[TestMethod()]
		public void GetRuntimeIdFromBlockStateTest()
		{
			var block = new DoublePlant();
			block.DoublePlantType = "grass";
			block.UpperBlockBit = true;

			Assert.AreNotEqual(-1, block.GetRuntimeId());
		}
	}
}