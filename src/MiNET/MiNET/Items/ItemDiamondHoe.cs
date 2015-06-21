using MiNET.Worlds;
using MiNET.Utils;
using MiNET.Blocks;

namespace MiNET.Items
{
    class ItemDiamondHoe : Item
    {
        public ItemDiamondHoe(short metadata)
            : base(293, metadata)
        {
            ItemMaterial = ItemMaterial.Diamond;
            ItemType = ItemType.Hoe;
        }

        public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
        {
            Block tile = world.GetBlock(blockCoordinates);
            if (tile.Id == 2 || tile.Id == 3)
            {
                FarmLand farm = new FarmLand();
                farm.Coordinates = blockCoordinates;
                farm.Metadata = 0;
                world.SetBlock(farm);
            }
        }
    }
}
