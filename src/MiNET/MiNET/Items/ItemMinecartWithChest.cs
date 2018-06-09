using MiNET.Entities.Vehicles;
using MiNET.Utils;
using MiNET.Worlds;
using System.Numerics;

namespace MiNET.Items
{
    internal class ItemMinecartWithChest : Item
    {
        public ItemMinecartWithChest() : base(342, 0, 1)
        {
            MaxStackSize = 1;
        }

        public override void PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
        {
            if (world.GetBlock(blockCoordinates, null).Id == 66)
                new ChestMinecart(world, new PlayerLocation(blockCoordinates) + new PlayerLocation(0.5, 0, 0.5)).SpawnEntity();
            else
                base.PlaceBlock(world, player, blockCoordinates, face, faceCoords);
        }
    }
}