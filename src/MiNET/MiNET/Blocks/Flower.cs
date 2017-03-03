using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public class Flower : Block
	{
		public Flower() : base(38)
		{
			IsSolid = false;
			IsTransparent = true;
		}

		public override void BlockUpdate(Level level, BlockCoordinates blockCoordinates)
		{
			if (Coordinates + BlockCoordinates.Down == blockCoordinates)
			{
				level.SetAir(Coordinates);
				UpdateBlocks(level);
			}
		}
	}
}