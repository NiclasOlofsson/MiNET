using MiNET.Worlds;
using MiNET.Utils;
using MiNET.Blocks;

namespace MiNET.Items
{
    class ItemStoneHoe : Item
    {
        public ItemStoneHoe(short metadata)
            : base(291, metadata)
        {
            ItemMaterial = ItemMaterial.Stone;
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
