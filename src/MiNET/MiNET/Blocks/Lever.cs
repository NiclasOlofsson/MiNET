using System.Numerics;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public class Lever : Block
	{
		public Lever() : base(69)
		{
			IsTransparent = true;
			IsSolid = false;
			BlastResistance = 2.5f;
			Hardness = 0.5f;
		}

		public override void BlockUpdate(Level level, BlockCoordinates blockCoordinates)
		{
			var currentFace = (Metadata & 0x7);

			if (((currentFace == 5 || currentFace == 6)
				 && Coordinates + BlockCoordinates.Down == blockCoordinates)

			    || ((currentFace == 7 || currentFace == 0)
					&& Coordinates + BlockCoordinates.Up == blockCoordinates)

			    || (currentFace == 2
			        && Coordinates + BlockCoordinates.East == blockCoordinates)

			    || (currentFace == 4
			        && Coordinates + BlockCoordinates.South == blockCoordinates)

			    || (currentFace == 1
			        && Coordinates + BlockCoordinates.West == blockCoordinates)

			    || (currentFace == 3
			        && Coordinates + BlockCoordinates.North == blockCoordinates))
			{
				level.BreakBlock(this);
			}
		}

		public override bool PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
		{
			var playerRotation = player.GetDirection();

			if (face == BlockFace.Up)
			{
				switch (playerRotation)
				{
					case 0: //South
						Metadata = 6;
						break;
					case 1: //West 
						Metadata = 5;
						break;
					case 2: //North
						Metadata = 6;
						break;
					case 3: //East
						Metadata = 5;
						break;
				}
			}
			else if (face == BlockFace.Down)
			{
				switch (playerRotation)
				{
					case 0: //South
						Metadata = 0;
						break;
					case 1: //West 
						Metadata = 7;
						break;
					case 2: //North
						Metadata = 0;
						break;
					case 3: //East
						Metadata = 7;
						break;
				}
			}
			else if (face == BlockFace.East)
			{
				Metadata = 4;
			}
			else if (face == BlockFace.South)
			{
				Metadata = 1;
			}
			else if (face == BlockFace.North)
			{
				Metadata = 2;
			}
			else if (face == BlockFace.West)
			{
				Metadata = 3;
			}

			world.SetBlock(this);
			return true;
		}

		public override bool Interact(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoord)
		{
			Metadata = (byte)(Metadata ^ (0x8));
			world.SetBlock(this);

			return true;
		}
	}
}