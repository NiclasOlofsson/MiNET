using System.Numerics;
using MiNET.Blocks;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Items
{
	public partial class ItemLavaBucket
	{
		public ItemLavaBucket() : base()
		{
			MaxStackSize = 1;
			FuelEfficiency = 1000;
		}

		public override bool PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
		{
			if (ItemFactory.GetItem<Lava>().PlaceBlock(world, player, blockCoordinates, face, faceCoords))
			{
				player.Inventory.AddItem(new ItemBucket(), true);
				return true;
			}

			return false;
		}
	}
}
