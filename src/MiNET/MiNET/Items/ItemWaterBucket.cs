using System.Numerics;
using MiNET.Blocks;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Items
{
	public partial class ItemWaterBucket
	{
		public ItemWaterBucket() : base()
		{
			MaxStackSize = 1;
		}

		public override bool PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
		{
			if (ItemFactory.GetItem<Water>().PlaceBlock(world, player, blockCoordinates, face, faceCoords))
			{
				player.Inventory.AddItem(new ItemBucket(), true);
				return true;
			}

			return false;
		}
	}
}
