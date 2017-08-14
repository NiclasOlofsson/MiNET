using System.Numerics;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public class IronDoor : Block
	{
		public IronDoor() : base(71)
		{
			IsTransparent = true;
			BlastResistance = 25;
			Hardness = 5;
		}

		protected override bool CanPlace(Level world, BlockCoordinates blockCoordinates, BlockCoordinates targetCoordinates, BlockFace face)
		{
			return world.GetBlock(blockCoordinates).IsReplacible && world.GetBlock(blockCoordinates + Level.Up).IsReplacible;
		}

		public override void BreakBlock(Level level, bool silent = false)
		{
			// Remove door
			if ((Metadata & 0x08) == 0x08) // Is Upper?
			{
				level.SetAir(Coordinates + Level.Down);
			}
			else
			{
				level.SetAir(Coordinates + Level.Up);
			}

			level.SetAir(Coordinates);
		}

		public override bool Interact(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoord)
		{
			Block block = this;
			// Remove door
			if ((Metadata & 0x08) == 0x08) // Is Upper?
			{
				block = world.GetBlock(GetNewCoordinatesFromFace(blockCoordinates, BlockFace.Down));
			}

			block.Metadata ^= 0x04;
			world.SetBlock(block);

			return true;
		}
	}
}