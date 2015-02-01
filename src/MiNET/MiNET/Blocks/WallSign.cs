using Craft.Net.Common;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public class WallSign : Block
	{
		internal WallSign() : base(68)
		{
		}

		public override bool Interact(Level world, Player player, Coordinates3D blockCoordinates, BlockFace face)
		{
			return true;
		}
	}
}