using MiNET.Worlds;

namespace MiNET.Blocks
{
	public class Stationary : Block
	{
		internal Stationary(byte id) : base(id)
		{
			IsSolid = false;
			IsBuildable = false;
		}

		public override void DoPhysics(Level level)
		{
			if (level.GetBlock(Coordinates).Id == Id)
			{
				SetToFlowing(level);
			}
		}

		private void SetToFlowing(Level world)
		{
			Block flowingBlock = BlockFactory.GetBlockById((byte) (Id - 1));
			flowingBlock.Metadata = Metadata;
			flowingBlock.Coordinates = Coordinates;
			world.SetBlock(flowingBlock, applyPhysics: false);
			world.ScheduleBlockTick(flowingBlock, 5);
		}
	}
}