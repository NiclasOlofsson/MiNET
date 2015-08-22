using MiNET.Items;

namespace MiNET.Blocks
{
	public class GoldOre : Block
	{
		public GoldOre() : base(14)
		{
		}

		public override Item GetSmelt()
		{
			return ItemFactory.GetItem(266, 0);
		}
	}
}
