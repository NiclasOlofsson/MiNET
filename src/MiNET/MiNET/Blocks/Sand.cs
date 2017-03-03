using System.Numerics;
using log4net;
using MiNET.Entities.World;
using MiNET.Items;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public class Sand : Block
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (Sand));
		private int _tickRate = 0;

		public Sand() : base(12)
		{
			BlastResistance = 2.5f;
			Hardness = 0.5f;
		}

		public override bool PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
		{
			Log.Debug("Scheduled block tick for sand");
			world.ScheduleBlockTick(this, _tickRate);
			return false;
		}

		public override void BlockUpdate(Level world, BlockCoordinates blockCoordinates)
		{
			world.ScheduleBlockTick(this, _tickRate);
		}

		public override void DoPhysics(Level level)
		{
			level.ScheduleBlockTick(this, _tickRate);
		}

		public override void OnTick(Level level, bool isRandom)
		{
			if (isRandom) return;

			if (!level.GetBlock(Coordinates + Level.Down).IsSolid)
			{
				level.SetBlock(new Air {Coordinates = Coordinates});
				new FallingSand(level)
				{
					KnownPosition = new PlayerLocation(Coordinates.X, Coordinates.Y, Coordinates.Z)
				}.SpawnEntity();
			}
		}

		public override Item GetSmelt()
		{
			return ItemFactory.GetItem(20, 0);
		}
	}
}