using System;
using MiNET.Blocks;

namespace MiNET.Items
{
    public class ItemFlintAndSteel : Item
    {
        public ItemFlintAndSteel() : base(259)
        {
        }

        public override void UseItem(MiNET.Worlds.Level world, Player player, Craft.Net.Common.Coordinates3D blockCoordinates, BlockFace face)
        {
            player.Level.BroadcastTextMessage("Face: " + face);
            if (face == BlockFace.PositiveY)
            {
                Block fire = new Block(51);
                fire.Coordinates = GetNewCoordinatesFromFace(blockCoordinates, BlockFace.PositiveY);
                world.SetBlock(fire);
            }
            else
            {
                if (world.GetBlock(blockCoordinates).Id == 46)
                {
                    //We need to handle activating TNT here
                }
            }
        }
    }
}

