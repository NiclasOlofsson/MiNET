using MiNET.Items;

namespace MiNET.Blocks
{
	public class Grass : Block
	{
		public Grass() : base(2)
		{
		}

		public override Item GetDrops()
		{
			return new Item(3); //Drop dirt block
		}
	}
}