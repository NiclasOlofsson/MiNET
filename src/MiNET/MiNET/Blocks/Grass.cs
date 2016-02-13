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
			return new ItemBlock(new Dirt(), 0) {Count = 1}; //Drop dirt block
		}
	}
}