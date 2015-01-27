using System;
using System.Collections.Generic;
using System.Diagnostics;
using Craft.Net.Common;
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
            if (world.GetBlock(blockCoordinates).Id != 46)
            {
                var fire = new Block(51)
                {
                    Coordinates = GetNewCoordinatesFromFace(blockCoordinates, BlockFace.PositiveY)
                };
                world.SetBlock(fire);
            }
            else
            {
                new Explosion(world, blockCoordinates, 4).Explode();
            }
        }
    }
}

