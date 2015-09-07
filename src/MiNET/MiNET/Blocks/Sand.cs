using MiNET.Items;

namespace MiNET.Blocks
{
	public class Sand : Block
	{
		public Sand() : base(12)
		{
		}

		public override Item GetSmelt()
		{
			return ItemFactory.GetItem(20, 0);
		}
	}
}
