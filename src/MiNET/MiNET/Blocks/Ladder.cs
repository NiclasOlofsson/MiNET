using System.Numerics;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public class Ladder : Block
	{
		public Ladder() : base(65)
		{
			IsTransparent = true;
			BlastResistance = 2;
			Hardness = 0.4f;
		}

		protected override bool CanPlace(Level world, BlockCoordinates blockCoordinates, BlockFace face)
		{
			return !world.GetBlock(blockCoordinates).IsTransparent && face != BlockFace.Down && face != BlockFace.Up;
		}

		public override bool PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
		{
			Metadata = (byte) face;
			return base.PlaceBlock(world, player, blockCoordinates, face, faceCoords);
		}
	}
}