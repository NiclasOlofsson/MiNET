using MiNET.Items;

namespace MiNET.Blocks
{
	public class Sand : Block
	{
		internal Sand() : base(12)
		{
		}

		public override Item GetSmelt()
		{
			return ItemFactory.GetItem(20, 0);
		}
	}
}
