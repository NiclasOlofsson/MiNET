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
		private readonly Mob _entity;
		private int _duration;
		private int _timeLeft;

		public HorseEatBlockBehavior(Mob entity, int duration)
		{
			this._entity = entity;
			_duration = Math.Max(40, duration);
			_timeLeft = _duration;
		}

		public bool ShouldStart()
		{
			if (!(_entity is Horse)) return false;

			if (_entity.Level.Random.Next(1000) != 0) return false;

			var coordinates = _entity.KnownPosition;
			var direction = Vector3.Normalize(coordinates.GetHeadDirection());

			BlockCoordinates coord = new Vector3(coordinates.X + direction.X, coordinates.Y, coordinates.Z + direction.Z);

			var shouldStart = _entity.Level.GetBlock(coord + BlockCoordinates.Down) is Grass || _entity.Level.GetBlock(coord) is TallGrass;
			if (!shouldStart) return false;

			_duration = 40;

			_entity.Velocity *= new Vector3(0, 1, 0);
			SetEating((Horse) _entity, true);

			return true;
		}

		public bool CanContinue()
		{
			return _duration-- > 0;
		}

		public void OnTick()
		{
		}

		public void OnEnd()
		{
			var coordinates = _entity.KnownPosition;
			var direction = Vector3.Normalize(coordinates.GetHeadDirection());

			BlockCoordinates coord = new Vector3(coordinates.X + direction.X, coordinates.Y, coordinates.Z + direction.Z);

			Block broken = null;
			if (_entity.Level.GetBlock(coord) is TallGrass)
			{
				broken = _entity.Level.GetBlock(coord);
				_entity.Level.SetAir(coord);
			}
			else
			{
				coord += BlockCoordinates.Down;
				broken = _entity.Level.GetBlock(coord);
				_entity.Level.SetBlock(new Dirt {Coordinates = coord});
			}
			DestroyBlockParticle particle = new DestroyBlockParticle(_entity.Level, broken);
			particle.Spawn();
			SetEating((Horse) _entity, false);
		}

		private void SetEating(Horse horse, bool isEating)
		{
			horse.IsEating = isEating;
			horse.BroadcastSetEntityData();
		}
	}
}