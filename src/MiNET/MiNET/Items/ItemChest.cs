using MiNET.BlockEntities;
using MiNET.Blocks;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Items
{
	public class ItemChest : Item
	{
		public ItemChest(short metadata) : base(54, metadata)
		{
		}

		public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
		{
			var coor = GetNewCoordinatesFromFace(blockCoordinates, face);
			Chest chest = new Chest
			{
				Coordinates = coor,
				Metadata = (byte) Metadata
			};

			if (!chest.CanPlace(world, face)) return;

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