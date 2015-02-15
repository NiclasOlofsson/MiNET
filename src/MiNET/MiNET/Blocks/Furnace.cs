using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public class Furnace : Block
	{
		internal Furnace() : base(61)
		{
		}

		protected Furnace(byte id) : base(id)
		{
		}

		public override bool PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face)
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

		public override bool Interact(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face)
		{
			player.OpenInventory(blockCoordinates);

			return true;
		}
	}

	public class LitFurnace : Furnace
	{
		public LitFurnace() : base(62)
		{
		}
	}
}