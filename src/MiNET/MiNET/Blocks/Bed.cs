using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public class Bed : Block
	{
		public Bed() : base(26)
		{
			BlastResistance = 1;
			Hardness = 0.2f;
			IsTransparent = true;
			//IsFlammable = true; // It can catch fire from lava, but not other means.
		}

		public override void BreakBlock(Level level, bool silent = false)
		{
			BlockCoordinates direction = new BlockCoordinates();
			switch (Metadata & 0x07)
			{
				case 0:
					direction = Level.East;
					break; // West
				case 1:
					direction = Level.South;
					break; // South
				case 2:
					direction = Level.West;
					break; // East
				case 3:
					direction = Level.North;
					break; // North 
			}

			// Remove bed
			if ((Metadata & 0x08) != 0x08)
			{
				direction = direction*-1;
			}

			level.SetAir(Coordinates + direction);
		    level.SetAir(Coordinates);
		}
	}
}