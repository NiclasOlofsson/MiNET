using System.Numerics;
using MiNET.Blocks;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Items
{
	public class ItemCarrot : FoodItem
	{
		public ItemCarrot() : base(391, 0, 3, 4.8)
		{
		}

		public override void UseItem(Level world, Player player, BlockCoordinates targetCoordinates, BlockFace face, Vector3 faceCoords)
		{
			Block targetBlock = world.GetBlock(targetCoordinates);
			Block carrots = new Carrots();
			carrots.Coordinates = targetBlock.IsReplacible ? targetCoordinates : GetNewCoordinatesFromFace(targetCoordinates, face);

			carrots.Metadata = (byte) Metadata;

			if (!carrots.CanPlace(world, targetCoordinates, face)) return;

			if (carrots.PlaceBlock(world, player, targetCoordinates, face, faceCoords)) return; // Handled

			world.SetBlock(carrots);
		}
	}
}