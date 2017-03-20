using System.Numerics;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public class EnchantingTable : Block
	{
		public EnchantingTable() : base(116)
		{
			FuelEfficiency = 15;
			IsTransparent = true;
			BlastResistance = 6000;
			Hardness = 5;
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