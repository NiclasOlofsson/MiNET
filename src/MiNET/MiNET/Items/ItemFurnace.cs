using System.Numerics;
using MiNET.BlockEntities;
using MiNET.Blocks;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Items
{
	public class ItemFurnace : ItemBlock
	{
		public ItemFurnace() : base(61, 0)
		{
		}

		public override Item GetSmelt()
		{
			return null;
		}

		public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
		{
			var coor = GetNewCoordinatesFromFace(blockCoordinates, face);
			Furnace furnace = new Furnace
			{
				Coordinates = coor,
			};

			if (!furnace.CanPlace(world, blockCoordinates, face)) return;

			furnace.PlaceBlock(world, player, coor, face, faceCoords);

			// Then we create and set the sign block entity that has all the intersting data

			FurnaceBlockEntity furnaceBlockEntity = new FurnaceBlockEntity
			{
				Coordinates = coor
			};

			world.SetBlockEntity(furnaceBlockEntity);
		}
	}
}