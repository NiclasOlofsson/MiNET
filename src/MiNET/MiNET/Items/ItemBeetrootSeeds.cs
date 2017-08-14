using System.Numerics;
using MiNET.Blocks;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Items
{
	public class ItemBeetrootSeeds : Item
	{
		public ItemBeetrootSeeds() : base(458)
		{
		}

		public override void UseItem(Level world, Player player, BlockCoordinates targetCoordinates, BlockFace face, Vector3 faceCoords)
		{
			Block targetBlock = world.GetBlock(targetCoordinates);
			Block beetroot = new Beetroot();
			beetroot.Coordinates = targetBlock.IsReplacible ? targetCoordinates : GetNewCoordinatesFromFace(targetCoordinates, face);

			beetroot.Metadata = (byte) Metadata;

			if (!beetroot.CanPlace(world, targetCoordinates, face)) return;

			if (beetroot.PlaceBlock(world, player, targetCoordinates, face, faceCoords)) return; // Handled

			world.SetBlock(beetroot);
		}
	}
}