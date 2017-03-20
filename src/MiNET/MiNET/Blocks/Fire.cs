using MiNET.Items;

namespace MiNET.Blocks
{
	public class Fire : Block
	{
		public Fire() : base(51)
		{
			IsReplacible = true;
			IsTransparent = true;
			LightLevel = 15;
			IsSolid = false;
		}

		public override Item[] GetDrops(Item tool)
		{
			return new Item[0];
		}
	}
}