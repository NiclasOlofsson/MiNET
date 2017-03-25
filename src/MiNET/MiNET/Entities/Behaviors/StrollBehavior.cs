using System.Numerics;
using log4net;
using MiNET.Blocks;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Entities.Behaviors
{
	public class PanicBehavior : StrollBehavior
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (PanicBehavior));

		public PanicBehavior(int duration, double speed, double speedMultiplier) : base(duration, speed, speedMultiplier)
		{
		}

		public override bool ShouldStart(Entity entity)
		{
			return entity.HealthManager.LastDamageSource != null;
		}
	}

	public class StrollBehavior : IBehavior
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (StrollBehavior));

		private int _duration;
		private double _speed;
		private double _speedMultiplier;
		private int _timeLeft;

		public StrollBehavior(int duration, double speed, double speedMultiplier)
		{
			_duration = duration;
			_speed = speed;
			_speedMultiplier = speedMultiplier;
			_timeLeft = duration;
		}

		public virtual bool ShouldStart(Entity entity)
		{
			return entity.Level.Random.Next(120) == 0;
		}

		public virtual bool OnTick(Entity entity)
		{
			return false;
		}

		public virtual bool CalculateNextMove(Entity entity)
		{
			if (_timeLeft-- <= 0)
			{
				_timeLeft = _duration;
				return true;
			}

			float speedFactor = (float) (_speed*_speedMultiplier);
			var level = entity.Level;
			var coordinates = entity.KnownPosition;
			var direction = Vector3.Normalize(coordinates.GetHeadDirection()*new Vector3(1, 0, 1));

			var blockDown = level.GetBlock(coordinates + BlockCoordinates.Down);
			if (entity.Velocity.Y < 0 && blockDown is Air)
			{
				return false;
			}

			BlockCoordinates coord = (BlockCoordinates) (coordinates + (direction*speedFactor) + (direction*(float) entity.Length/2));
			BlockCoordinates coordUp = coord + BlockCoordinates.Up;

			var players = level.GetSpawnedPlayers();
			bool entityCollide = false;
			var boundingBox = entity.GetBoundingBox().OffsetBy((direction*speedFactor) + (direction*(float) entity.Length/2));
			foreach (var player in players)
			{
				if (player.GetBoundingBox().Intersects(boundingBox))
				{
					entityCollide = true;
					break;
				}
			}

			if (!entityCollide)
			{
				var entities = level.GetEntites();
				foreach (var ent in entities)
				{
					if (ent == entity) continue;

					if (ent.GetBoundingBox().Intersects(boundingBox))
					{
						entityCollide = true;
						break;
					}
				}
			}

			var block = level.GetBlock(coord);
			var blockUp = level.GetBlock(coordUp);

			var colliding = block.IsSolid || blockUp.IsSolid;
			if (!colliding && !entityCollide)
			{
				var velocity = direction*speedFactor;
				Log.Debug($"Moving sheep: {velocity}");
				if (entity.Velocity.Length() < velocity.Length())
				{
					entity.Velocity += velocity - entity.Velocity;
				}
			}
			else
			{
				if (!entityCollide && !blockUp.IsSolid && level.Random.Next(4) != 0)
				{
					Log.Debug($"Block ahead: {block}, jumping");
					entity.Velocity = new Vector3(0, 0.42f, 0);
				}
				else
				{
					Log.Debug($"Block ahead: {block}, turning");
					int rot = level.Random.Next(2) == 0 ? level.Random.Next(45, 180) : level.Random.Next(-180, -45);
					entity.KnownPosition.HeadYaw += rot;
					entity.KnownPosition.Yaw += rot;
					entity.Velocity *= new Vector3(0, 1, 0);
				}
			}

			return false;
		}

		private bool AreaIsClear(Level level, BoundingBox bbox)
		{
			BlockCoordinates min = bbox.Min;
			BlockCoordinates max = bbox.Max;
			for (int x = min.X; x < max.X; x++)
			{
				for (int y = min.Y; y < max.Y; y++)
				{
					for (int z = min.Z; z < max.Z; z++)
					{
						if (!level.IsAir(new BlockCoordinates(x, y, z))) return false;
					}
				}
			}

			return true;
		}
	}
}