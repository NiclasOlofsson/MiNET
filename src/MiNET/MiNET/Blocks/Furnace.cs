using System.Numerics;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public class Furnace : Block
	{
		public Furnace() : base(61)
		{
		}

		protected Furnace(byte id) : base(id)
		{
			BlastResistance = 17.5f;
			Hardness = 3.5f;
		}

		public override bool PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
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

		public override bool Interact(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoord)
		{
			player.OpenInventory(blockCoordinates);

			return true;
		}
	}
}