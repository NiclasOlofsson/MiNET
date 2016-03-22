using MiNET.Items;

namespace MiNET.Blocks
{
	public class StoneBrick : Block
	{
		public StoneBrick() : base(98)
		{
			BlastResistance = 30;
			Hardness = 1.5f;
		}

		public override Item GetSmelt()
		{
			return ItemFactory.GetItem(1, 2);
		}
	}
}