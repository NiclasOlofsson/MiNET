using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public abstract class Button : Block
	{
		public int TickRate { get; set; }

		protected Button(byte id) : base(id)
		{
		}

		public override bool PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
		{
			Metadata = (byte) face;

			world.SetBlock(this);
			return true;
		}

		public override bool Interact(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face)
		{
			Metadata = (byte) (Metadata | (0x8));
			world.SetBlock(this);
			world.ScheduleBlockTick(this, TickRate);
			return true;
		}

		public override void OnTick(Level level)
		{
			Metadata = (byte) (Metadata & (0x7));
			level.SetBlock(this);
		}
	}
}