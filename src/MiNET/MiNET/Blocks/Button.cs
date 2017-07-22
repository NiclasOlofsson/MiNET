using System.Numerics;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public abstract class Button : Block
	{
		public int TickRate { get; set; }

		protected Button(byte id) : base(id)
		{
			IsSolid = false;
			IsTransparent = true;
			BlastResistance = 2.5f;
			Hardness = 0.5f;
		}

		public override bool PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
		{
			switch (face)
			{
				case BlockFace.Down:
					Metadata = 0;
					break;
				case BlockFace.Up:
					Metadata = 1;
					break;
				case BlockFace.East:
					Metadata = 2;
					break;
				case BlockFace.West:
					Metadata = 3;
					break;
				case BlockFace.North:
					Metadata = 4;
					break;
				case BlockFace.South:
					Metadata = 5;
					break;
				case BlockFace.None:

					break;
			}

			world.SetBlock(this);
			return true;
		}

		public override bool Interact(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoord)
		{
			Metadata = (byte) (Metadata | (0x8));
			world.SetBlock(this);
			world.ScheduleBlockTick(this, TickRate);
			return true;
		}

		public override void OnTick(Level level, bool isRandom)
		{
			if (isRandom) return;

			Metadata = (byte) (Metadata & (0x7));
			level.SetBlock(this);
		}
	}
}