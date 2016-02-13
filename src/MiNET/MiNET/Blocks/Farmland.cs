using MiNET.Items;

namespace MiNET.Blocks
{
	public class Farmland : Block
	{
		public Farmland() : base(60)
		{
		}

		public override Item GetDrops()
		{
			return new ItemBlock(new Dirt(), 0) {Count = 1}; // Drop dirt block
		}
	}
}