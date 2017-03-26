using System;
using System.Numerics;
using MiNET.Blocks;
using MiNET.Entities.Passive;
using MiNET.Particles;
using MiNET.Utils;

namespace MiNET.Entities.Behaviors
{
	public class HorseEatBlockBehavior : IBehavior
	{
		private int _duration;
		private int _timeLeft;

		public HorseEatBlockBehavior(int duration)
		{
			_duration = Math.Max(40, duration);
			_timeLeft = _duration;
		}

		public bool ShouldStart(Entity entity)
		{
			if (!(entity is Horse)) return false;

			if (entity.Level.Random.Next(1000) != 0) return false;

			var coordinates = entity.KnownPosition;
			var direction = Vector3.Normalize(coordinates.GetHeadDirection());

			BlockCoordinates coord = new Vector3(coordinates.X + direction.X, coordinates.Y, coordinates.Z + direction.Z);

			var shouldStart = entity.Level.GetBlock(coord + BlockCoordinates.Down) is Grass || entity.Level.GetBlock(coord) is TallGrass;
			if (!shouldStart) return false;

			_duration = 40;

			entity.Velocity *= new Vector3(0, 1, 0);
			SetEating((Horse) entity, true);

			return true;
		}

		public bool OnTick(Entity entity)
		{
			return false;
		}

		public bool CalculateNextMove(Entity entity)
		{
			return _duration-- < 0;
		}

		public void OnEnd(Entity entity)
		{
			var coordinates = entity.KnownPosition;
			var direction = Vector3.Normalize(coordinates.GetHeadDirection());

			BlockCoordinates coord = new Vector3(coordinates.X + direction.X, coordinates.Y, coordinates.Z + direction.Z);

			Block broken = null;
			if (entity.Level.GetBlock(coord) is TallGrass)
			{
				broken = entity.Level.GetBlock(coord);
				entity.Level.SetAir(coord);
			}
			else
			{
				coord += BlockCoordinates.Down;
				broken = entity.Level.GetBlock(coord);
				entity.Level.SetBlock(new Dirt {Coordinates = coord});
			}
			DestroyBlockParticle particle = new DestroyBlockParticle(entity.Level, broken);
			particle.Spawn();
			SetEating((Horse) entity, false);
		}

		private void SetEating(Horse horse, bool isEating)
		{
			horse.IsEating = isEating;
			horse.BroadcastSetEntityData();
		}
	}
}