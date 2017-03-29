using MiNET.Items;

namespace MiNET.Blocks
{
	public class EnderChest : Block
	{
		public EnderChest() : base(130)
		{
			IsTransparent = true;
			LightLevel = 7;
			BlastResistance = 3000;
			Hardness = 22.5f;
		}

		public override Item[] GetDrops(Item tool)
		{
			return new Item[] {ItemFactory.GetItem(49, 0, 8)}; // 8 Obsidian
		}
	}
}