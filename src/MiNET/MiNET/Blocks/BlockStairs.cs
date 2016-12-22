using System.Numerics;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public abstract class BlockStairs : Block
	{
		protected BlockStairs(byte id) : base(id)
		{
			FuelEfficiency = 15;
			IsTransparent = true;
		}

		// 000 001 010 011 100
		public override bool PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
		{
			if (player == null) return false;

			byte direction = player.GetDirection();

			byte upper = (byte) ((faceCoords.Y > 0.5 && face != BlockFace.Up) || face == BlockFace.Down ? 0x04 : 0x00);

			switch (direction)
			{
				case 0:
					Metadata = (byte) (0 | upper);
					break;
				case 1:
					Metadata = (byte) (2 | upper);
					break;
				case 2:
					Metadata = (byte) (1 | upper);
					break;
				case 3:
					Metadata = (byte) (3 | upper);
					break;
			}

			world.SetBlock(this);
			return true;
		}
	}
}