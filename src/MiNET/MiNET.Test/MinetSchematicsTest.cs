using fNbt;
using NUnit.Framework;

namespace MiNET
{
	[TestFixture]
	public class MinetSchematicsTest
	{
		[Test, Ignore("")]
		public void ReadSchematicsTest()
		{
			NbtFile file = new NbtFile(@"D:\Downloads\schematics\medieval-castle.schematic");
			NbtCompound schematic = file.RootTag;
			Assert.AreEqual("Schematic", schematic.Name);
			Assert.AreEqual(110, schematic["Width"].ShortValue);
			Assert.AreEqual(94, schematic["Length"].ShortValue);
			Assert.AreEqual(62, schematic["Height"].ShortValue);
			Assert.AreEqual("Alpha", schematic["Materials"].StringValue);
			byte[] blocks = schematic["Blocks"].ByteArrayValue;
			Assert.AreEqual(641080, blocks.Length);
			byte[] blockData = schematic["Data"].ByteArrayValue;
			Assert.AreEqual(641080, blockData.Length);

			NbtList blockEntities = schematic["TileEntities"] as NbtList;
			if (blockEntities != null)
			{
				Assert.AreEqual(30, blockEntities.Count);
				foreach (var blockEntity in blockEntities)
				{
				}
			}
		}
	}
}