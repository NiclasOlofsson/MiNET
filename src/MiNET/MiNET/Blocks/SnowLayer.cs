using MiNET.Items;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public class SnowLayer : Block
	{
		public SnowLayer() : base(78)
		{
			IsTransparent = true;
			BlastResistance = 0.5f;
			Hardness = 0.1f;
		}

		public override Item GetDrops()
		{
			// One per layer, plus one.
			return ItemFactory.GetItem(332, 0, (byte)(Metadata + 2));
		}
	}
}