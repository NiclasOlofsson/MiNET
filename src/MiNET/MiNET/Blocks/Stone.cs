using MiNET.Items;

namespace MiNET.Blocks
{
	public class Stone : Block
	{
		public Stone() : base(1)
		{
			BlastResistance = 30;
			Hardness = 1.5f;
            IsConductive = true;
        }

		public override Item[] GetDrops(Item tool)
		{
			if (tool.ItemType != ItemType.PickAxe) return new Item[0];

			return new[] {new ItemBlock(new Cobblestone(), 0) {Count = 1}}; // Drop cobblestone
		}
	}
}