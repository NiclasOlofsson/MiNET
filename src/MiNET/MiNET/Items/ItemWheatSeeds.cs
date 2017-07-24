using System.Numerics;
using MiNET.Blocks;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Items
{
	public class ItemWheatSeeds : Item
	{
		public ItemWheatSeeds() : base(295)
		{
		}

		public override void UseItem(Level world, Player player, BlockCoordinates targetCoordinates, BlockFace face, Vector3 faceCoords)
		{
			Block targetBlock = world.GetBlock(targetCoordinates);
			Block wheat = new Wheat();
			wheat.Coordinates = targetBlock.IsReplacible ? targetCoordinates : GetNewCoordinatesFromFace(targetCoordinates, face);

			wheat.Metadata = (byte) Metadata;

			if (!wheat.CanPlace(world, targetCoordinates, face)) return;

			if (wheat.PlaceBlock(world, player, targetCoordinates, face, faceCoords)) return; // Handled

			world.SetBlock(wheat);
		}
	}
}