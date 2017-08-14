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

		protected override bool CanPlace(Level world, BlockCoordinates blockCoordinates, BlockCoordinates targetCoordinates, BlockFace face)
		{
			if (base.CanPlace(world, blockCoordinates, face))
			{
				Block under = world.GetBlock(Coordinates + BlockCoordinates.Down);
				return under is Grass || under is Dirt;
			}

			return false;
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