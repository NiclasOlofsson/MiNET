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
			return new Item(3); // Drop dirt block
		}
	}
}