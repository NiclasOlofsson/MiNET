using MiNET.Blocks;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Items
{
    public class ItemDye : Item
    {
        public ItemDye(short metadata) : base(351, metadata)
        {
        }

        public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
        {
            if (player.Inventory.GetItemInHand().Metadata == 15)
            {
                Block block = world.GetBlock(blockCoordinates);
                if (block is Grass && face == BlockFace.Up)
                {
                    Block tallgrass = new TallGrass
                    {
                        Coordinates = blockCoordinates,
                        Metadata = 0
                    };
                    world.SetBlock(tallgrass);
                }
            }
        }
    }
}