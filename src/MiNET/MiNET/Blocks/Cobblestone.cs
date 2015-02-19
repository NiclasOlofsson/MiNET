using MiNET.Items;

namespace MiNET.Blocks
{
	public class Cobblestone : Block
	{
		internal Cobblestone() : base(4)
		{
		}

		public override Item GetSmelt()
		{
			return ItemFactory.GetItem(1, 0);
		}
	}
}
