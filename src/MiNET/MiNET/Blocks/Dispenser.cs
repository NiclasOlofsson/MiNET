using System.Numerics;
using MiNET.Items;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public class Dispenser : Block
	{
		public Dispenser() : base(23)
		{
			BlastResistance = 17.5f;
			Hardness = 3.5f;
		}

		public override Item[] GetDrops(Item tool)
		{
			// TODO: Needs Dispenser TileEntity.
			return base.GetDrops(tool);
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
								Metadata = 4;
								break;
							case 1: //West 
								Metadata = 2;
								break;
							case 2: //North
								Metadata = 5;
								break;
							case 3: //East
								Metadata = 3;
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
								Metadata = 4;
								break;
							case 1: //West 
								Metadata = 2;
								break;
							case 2: //North
								Metadata = 5;
								break;
							case 3: //East
								Metadata = 3;
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
