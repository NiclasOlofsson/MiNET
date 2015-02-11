using Craft.Net.Common;
using MiNET.Net;
using MiNET.Utils;
using MiNET.Worlds;
using ItemStack = MiNET.Utils.ItemStack;
using MetadataSlot = MiNET.Utils.MetadataSlot;

namespace MiNET.Blocks
{
	public class Chest : Block
	{
		public Chest() : base(54)
		{
		}

		public override bool PlaceBlock(Level world, Player player, Coordinates3D blockCoordinates, BlockFace face)
		{
			byte direction = player.GetDirection();

			switch (direction)
			{
				case 1:
					Metadata = 2;
					break; // West
				case 2:
					Metadata = 5;
					break; // North
				case 3:
					Metadata = 3;
					break; // East
				case 0:
					Metadata = 4;
					break; // South 
			}

			world.SetBlock(this);

			return true;
		}


		public override bool Interact(Level world, Player player, Coordinates3D blockCoordinates, BlockFace face)
		{
			player.OpenInventory(blockCoordinates);

			return true;
		}
	}
}