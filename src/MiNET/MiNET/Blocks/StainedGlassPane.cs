using MiNET.Items;

namespace MiNET.Blocks
{
	public class StainedGlassPane : Block
	{
		public StainedGlassPane() : base(160)
		{
			IsTransparent = true; // I should hope so at least
			BlastResistance = 1.5f;
		}

		public override Item[] GetDrops(Item tool)
		{
			return new Item[0]; // No drops
		}
	}
}