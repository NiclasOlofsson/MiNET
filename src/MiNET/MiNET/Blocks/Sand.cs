using MiNET.Items;

namespace MiNET.Blocks
{
	public class Sand : Block
	{
		public Sand() : base(12)
		{
			BlastResistance = 2.5f;
			Hardness = 0.5f;
		}

		public override Item GetSmelt()
		{
			return ItemFactory.GetItem(20, 0);
		}
	}
}
