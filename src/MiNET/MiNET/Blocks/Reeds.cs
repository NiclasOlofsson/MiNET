using MiNET.Items;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public class Reeds : Block
	{
		public Reeds() : base(83)
		{
			IsSolid = false;
			IsTransparent = true;
		}

		public override void BlockUpdate(Level level, BlockCoordinates blockCoordinates)
		{
			if (Coordinates + BlockCoordinates.Down == blockCoordinates)
			{
				level.BreakBlock(this, null);
			}
		}

		public override Item[] GetDrops(Item tool)
		{
			return new[] {ItemFactory.GetItem(338, 0, 1)};
		}
	}
}