using MiNET.Items;

namespace MiNET.Blocks
{
	public class LitFurnace : Furnace
	{
		public LitFurnace() : base(62)
		{
			LightLevel = 13;
		}

		public override Item GetDrops()
		{
			return new ItemBlock(new Furnace(), 0);
		}
	}
}