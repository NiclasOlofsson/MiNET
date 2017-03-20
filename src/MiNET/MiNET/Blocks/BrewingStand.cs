using MiNET.Items;

namespace MiNET.Blocks
{
	public class BrewingStand : Block
	{
		public BrewingStand() : base(117)
		{
			IsTransparent = true;
			LightLevel = 1;
			BlastResistance = 2.5f;
			Hardness = 0.5f;
		}

		public override Item[] GetDrops(Item tool)
		{
			return new Item[] { ItemFactory.GetItem(379, 0, 1) };
		}
	}
}