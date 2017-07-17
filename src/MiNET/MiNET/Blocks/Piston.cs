using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public class Piston : Block
	{
		public Piston() : this(33)
		{
		}

		public Piston(byte id) : base(id)
		{
			BlastResistance = 2.5f;
			Hardness = 0.5f;
		}

		public override bool PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
		{
			switch (face)
			{
				case BlockFace.Down:
					if (player.KnownPosition.Pitch < -65)
					{
						Metadata = 0x0;
					}
					else
					{
						var playerRotation = player.GetDirection();
						switch (playerRotation)
						{
							case 0: //South
								Metadata = 5;
								break;
							case 1: //West 
								Metadata = 3;
								break;
							case 2: //North
								Metadata = 4;
								break;
							case 3: //East
								Metadata = 2;
								break;
						}
					}
					break;
				case BlockFace.Up:
					if (player.KnownPosition.Pitch > 65)
					{
						Metadata = 0x1;
					}
					else
					{
						var playerRotation = player.GetDirection();
						switch (playerRotation)
						{
							case 0: //South
								Metadata = 5;
								break;
							case 1: //West 
								Metadata = 3;
								break;
							case 2: //North
								Metadata = 4;
								break;
							case 3: //East
								Metadata = 2;
								break;
						}
					}
					break;
				case BlockFace.East:
					Metadata = 3;
					break;
				case BlockFace.West:
					Metadata = 2;
					break;
				case BlockFace.North:
					Metadata = 5;
					break;
				case BlockFace.South:
					Metadata = 4;
					break;
				case BlockFace.None:

					break;
			}

			world.SetBlock(this);
			return true;
		}
	}
}
