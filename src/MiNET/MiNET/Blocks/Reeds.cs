using MiNET.Items;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public partial class Reeds
	{
		public Reeds() : base()
		{
			IsSolid = false;
			IsTransparent = true;
		}

		protected override bool CanPlace(Level world, Player player, BlockCoordinates blockCoordinates, BlockCoordinates targetCoordinates, BlockFace face)
		{
			return CanGrowOn(world, blockCoordinates) && world.GetBlock(blockCoordinates).IsReplaceable;
		}

		public override void DoPhysics(Level level)
		{
			level.ScheduleBlockTick(this, 1);
		}

		public override void OnTick(Level level, bool isRandom)
		{
			if (!CanGrowOn(level, Coordinates))
			{
				level.BreakBlock(this);
			}
		}

		public override Item GetItem(Level world, bool blockItem = false)
		{
			return blockItem ? base.GetItem(world, blockItem) : new ItemSugarCane();
		}

		private bool CanGrowOn(Level world, BlockCoordinates blockCoordinates)
		{
			var currentBlock = world.GetBlock(blockCoordinates);

			if (currentBlock is Stationary 
				|| currentBlock is Flowing)
			{
				return false;
			}

			var targetCoordinates = blockCoordinates.BlockDown();
			var targetBlock = world.GetBlock(targetCoordinates);

			if (targetBlock is Reeds) return true;

			if (targetBlock is not Sand
				&& targetBlock is not Dirt
				&& targetBlock is not DirtWithRoots
				&& targetBlock is not Mycelium
				&& targetBlock is not Grass
				&& targetBlock is not Podzol)
			{
				return false;
			}

			foreach (var aroundCoords in targetCoordinates.Get2dAroundCoordinates())
			{
				var aroundBlock = world.GetBlock(aroundCoords);
				if (aroundBlock is Water || aroundBlock is FlowingWater)
				{
					return true;
				}
			}

			return false;
		}
	}
}