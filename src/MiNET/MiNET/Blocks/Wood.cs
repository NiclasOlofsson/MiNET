using MiNET.Items;
using MiNET.Utils;

namespace MiNET.Blocks
{
	public class Wood : Block
	{
		public Wood() : base(17)
		{
			FuelEfficiency = 15;
		}

		public override ItemStack GetDrops()
		{
			return new ItemStack(Id, 1, (short) (Metadata & 0x03));
		}

		public override Item GetSmelt()
		{
			return ItemFactory.GetItem(263, 1);
		}
	}
}
