using System.Numerics;
using log4net;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public class Torch : Block
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (Torch));

		public Torch() : base(50)
		{
			IsTransparent = true;
			IsSolid = false;
			LightLevel = 14;
		}

		//protected override bool CanPlace(Level world, BlockCoordinates blockCoordinates, BlockFace face)
		//{
		//	Log.Debug("1");

		//	Block block = world.GetBlock(blockCoordinates);
		//	if (block is Farmland
		//	    || block is Ice
		//		/*|| block is Glowstone || block is Leaves  */
		//	    || block is Tnt
		//	    || block is BlockStairs
		//	    || block is StoneSlab
		//	    || block is WoodSlab) return false;
		//	Log.Debug("2");

		//	//TODO: More checks here, but PE blocks it pretty good right now
		//	if (block is Glass && face == BlockFace.Up) return true;

		//	Log.Debug($"3 {block.Id} {!block.IsTransparent}");

		//	return !block.IsTransparent;
		//}

		public override bool PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
		{
			if (face == BlockFace.Down) return true;

			switch (face)
			{
				case BlockFace.Up:
					Metadata = 5;
					break;
				case BlockFace.East:
					Metadata = 4;
					break;
				case BlockFace.West:
					Metadata = 3;
					break;
				case BlockFace.North:
					Metadata = 2;
					break;
				case BlockFace.South:
					Metadata = 1;
					break;
			}

			//world.SetBlock(this);
			//return true;
			return false;
		}
	}
}