using MiNET.Blocks;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Items
{
	public class ItemFishing : Item
	{
		internal ItemFishing(int id, short metadata) : base(id, metadata)
		{
		}

		public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
		{
			//replace block with projectile path
			Block block = world.GetBlock(blockCoordinates);
			
			if (block is Water)
			{
				//perform fishing event
			}
		}
	}
}
