using MiNET.Items;

namespace MiNET.Blocks
{
	public class Cobblestone : Block
	{
		public Cobblestone() : base(4)
		{
			BlastResistance = 30;
			Hardness = 2;
		}

		public override Item GetSmelt()
		{
			return ItemFactory.GetItem(1, 0);
		}
	}
}