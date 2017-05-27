using System.Numerics;
using MiNET.Blocks;
using MiNET.Net;
using MiNET.Particles;
using MiNET.Utils;

namespace MiNET.Entities.Behaviors
{
	public class EatBlockBehavior : IBehavior
	{
		private readonly Mob _entity;
		private int _duration;

		public EatBlockBehavior(Mob entity)
		{
			this._entity = entity;
		}

		public bool ShouldStart()
		{
			if (_entity.Level.Random.Next(1000) != 0) return false;

			var coordinates = _entity.KnownPosition;
			var direction = Vector3.Normalize(coordinates.GetHeadDirection());

			BlockCoordinates coord = new Vector3(coordinates.X + direction.X, coordinates.Y, coordinates.Z + direction.Z);

			var shouldStart = _entity.Level.GetBlock(coord + BlockCoordinates.Down) is Grass || _entity.Level.GetBlock(coord) is TallGrass;
			if (!shouldStart) return false;

			_duration = 40;

			_entity.Velocity *= new Vector3(0, 1, 0);

			McpeEntityEvent entityEvent = McpeEntityEvent.CreateObject();
			entityEvent.runtimeEntityId = _entity.EntityId;
			entityEvent.eventId = 10;
			_entity.Level.RelayBroadcast(entityEvent);

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
		}
	}
}