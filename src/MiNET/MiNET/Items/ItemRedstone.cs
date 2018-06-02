using System.Numerics;
using MiNET.Blocks;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Items
{
	public class ItemRedstone : Item
	{
		public ItemRedstone() : base(331)
		{
		}

        public override void PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
        {
            RedstoneWire redstone = new RedstoneWire() { Coordinates = GetNewCoordinatesFromFace(blockCoordinates, face) };
            if (redstone.CanPlace(world, player, blockCoordinates, face))
                redstone.PlaceBlock(world, player, blockCoordinates, face, faceCoords);
        }
    }
}