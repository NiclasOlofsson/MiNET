using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public class Bed : Block
	{
		protected internal Bed() : base(26)
		{
		}

		public override void BreakBlock(Level level)
		{
			BlockCoordinates direction = new BlockCoordinates();
			switch (Metadata & 0x07)
			{
				case 0:
					direction = Level.East;
					break; // West
				case 1:
					direction = Level.North;
					break; // North
				case 2:
					direction = Level.West;
					break; // East
				case 3:
					direction = Level.South;
					break; // South 
			}

			// Remove bed
			if ((Metadata & 0x08) != 0x08)
			{
				direction = direction*-1;
			}

			level.SetBlock(new Air {Coordinates = Coordinates + direction});
			level.SetBlock(new Air {Coordinates = Coordinates});
		}
	}
}