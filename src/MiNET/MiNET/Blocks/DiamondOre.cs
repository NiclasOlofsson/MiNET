using MiNET.Items;
using MiNET.Utils;

namespace MiNET.Blocks
{
	public class DiamondOre : Block
	{
		public DiamondOre() : base(56)
		{
		}

		public override ItemStack GetDrops()
		{
			return new ItemStack(264, 1);
		}

		public override Item GetSmelt()
		{
			return ItemFactory.GetItem(264, 0);
		}
	}
}