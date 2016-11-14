using System;
using System.Numerics;
using MiNET.BlockEntities;
using MiNET.Blocks;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Items
{
    public class ItemEnderChest : ItemBlock
    {
        public ItemEnderChest() : base(130, 0)
        {
        }

        public override Item GetSmelt()
        {
            return null;
        }

        public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
        {
            if (player.GameMode != GameMode.Creative)
            {
                Item itemStackInHand = player.Inventory.GetItemInHand();
                itemStackInHand.Count--;

                if (itemStackInHand.Count <= 0)
                {
                    // set empty
                    player.Inventory.Slots[player.Inventory.Slots.IndexOf(itemStackInHand)] = new ItemAir();
                }
            }

            var coor = GetNewCoordinatesFromFace(blockCoordinates, face);
            EnderChest chest = new EnderChest
            {
                Coordinates = coor,
            };

            if (!chest.CanPlace(world, face)) return;

            chest.PlaceBlock(world, player, coor, face, faceCoords);

            EnderChestBlockEntity chestBlockEntity = new EnderChestBlockEntity
            {
                Coordinates = coor
            };

            world.SetBlockEntity(chestBlockEntity);
        }
    }
}
