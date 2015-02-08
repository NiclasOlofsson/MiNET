using Craft.Net.Common;
using MiNET.BlockEntities;
using MiNET.Blocks;
using MiNET.Worlds;

namespace MiNET.Items
{
	public class ItemChest : Item
	{
		public ItemChest() : base(54)
		{
		}

		public override void UseItem(Level world, Player player, Coordinates3D blockCoordinates, BlockFace face)
		{
			var coor = GetNewCoordinatesFromFace(blockCoordinates, face);
			Chest chest = new Chest
			{
				Coordinates = coor,
				Metadata = (byte) Metadata
			};

			if (!chest.CanPlace(world)) return;

			chest.PlaceBlock(world, player, coor, face);

			// Then we create and set the sign block entity that has all the intersting data

			ChestBlockEntity chestBlockEntity = new ChestBlockEntity
			{
				Coordinates = coor
			};

			world.SetBlockEntity(chestBlockEntity);
		}
	}
}