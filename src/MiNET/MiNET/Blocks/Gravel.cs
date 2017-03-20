using System;
using System.Numerics;
using MiNET.Entities.World;
using MiNET.Items;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Blocks
{
	public class Gravel : Block
	{
		private int _tickRate = 1;

		public Gravel() : base(13)
		{
			BlastResistance = 3;
			Hardness = 0.6f;
		}

		public override bool PlaceBlock(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
		{
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
				level.SetAir(Coordinates);

				var bbox = GetBoundingBox();
				var d = (bbox.Max - bbox.Min)/2;

				new FallingBlock(level, this)
				{
					KnownPosition = new PlayerLocation(Coordinates.X + d.X, Coordinates.Y - 0.01f, Coordinates.Z + d.Z)
				}.SpawnEntity();
			}
		}


		public override Item[] GetDrops(Item tool)
		{
			var rnd = new Random((int) DateTime.UtcNow.Ticks);
			if (rnd.NextDouble() <= 0.1)
			{
				return new[] {ItemFactory.GetItem(318)};
			}

			return base.GetDrops(tool);
		}
	}
}