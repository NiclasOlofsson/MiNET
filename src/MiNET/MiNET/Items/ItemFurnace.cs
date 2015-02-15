using MiNET.BlockEntities;
using MiNET.Blocks;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Items
{
	public class ItemFurnace : Item
	{
		internal ItemFurnace(short metadata) : base(61, metadata)
		{
		}


		public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face)
		{
			var coor = GetNewCoordinatesFromFace(blockCoordinates, face);
			Furnace furnace = new Furnace
			{
				Coordinates = coor,
				Metadata = (byte) Metadata
			};

			if (!furnace.CanPlace(world)) return;

			furnace.PlaceBlock(world, player, coor, face);

			// Then we create and set the sign block entity that has all the intersting data

			FurnaceBlockEntity furnaceBlockEntity = new FurnaceBlockEntity
			{
				Coordinates = coor
			};

			world.SetBlockEntity(furnaceBlockEntity);
		}
	}
}