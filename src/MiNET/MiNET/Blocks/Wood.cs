using MiNET.Items;

namespace MiNET.Blocks
{
	public class Wood : Block
	{
		internal Wood() : base(17)
		{
			FuelEfficiency = 15;
		}

		public override Item GetSmelt()
		{
			return ItemFactory.GetItem(263, 1);
		}
	}
}
