using MiNET.Items;

namespace MiNET.Blocks
{
	public class IronOre : Block
	{
		public IronOre() : base(15)
		{
		}

		public override Item GetSmelt()
		{
			return ItemFactory.GetItem(265, 0);
		}
	}
}
