using MiNET.Worlds;
using MiNET.Utils;
using MiNET.Blocks;

namespace MiNET.Items
{
    class ItemIronHoe : Item
    {
        public ItemIronHoe(short metadata)
            : base(292, metadata)
        {
            ItemMaterial = ItemMaterial.Iron;
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
