using MiNET.Items;

namespace MiNET.Blocks
{
	public class StoneBrick : Block
	{
		internal StoneBrick()
			: base(98)
		{
		}

		public override Item GetSmelt()
		{
			return ItemFactory.GetItem(1, 2);
		}
	}
}
