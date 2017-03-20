using System.Numerics;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public class Chest : Block
	{
		public Chest() : this(54)
		{
		}

		public Chest(byte id) : base(id)
		{
			FuelEfficiency = 15;
			IsTransparent = true;
			BlastResistance = 12.5f;
			Hardness = 2.5f;
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