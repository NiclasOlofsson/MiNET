using System.Numerics;
using MiNET.BlockEntities;
using MiNET.Blocks;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Items
{
	public class ItemChest : ItemBlock
	{
		public ItemChest() : base(54, 0)
		{
		}

		public override Item GetSmelt()
		{
			return null;
		}

		public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
		{
			if (player.GameMode != GameMode.Creative)
			{
				Item itemStackInHand = player.Inventory.GetItemInHand();
				itemStackInHand.Count--;

				if (itemStackInHand.Count <= 0)
				{
					// set empty
					player.Inventory.Slots[player.Inventory.Slots.IndexOf(itemStackInHand)] = new ItemAir();
				}
			}

			var coor = GetNewCoordinatesFromFace(blockCoordinates, face);
			Chest chest = new Chest
			{
				Coordinates = coor,
			};

			if (!chest.CanPlace(world, blockCoordinates, face)) return;

			chest.PlaceBlock(world, player, coor, face, faceCoords);

			// Then we create and set the sign block entity that has all the intersting data

			ChestBlockEntity chestBlockEntity = new ChestBlockEntity
			{
				Coordinates = coor
			};

			world.SetBlockEntity(chestBlockEntity);
		}
	}
}