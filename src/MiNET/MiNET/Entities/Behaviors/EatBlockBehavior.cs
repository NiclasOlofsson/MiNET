using System.Numerics;
using MiNET.Blocks;
using MiNET.Net;
using MiNET.Particles;
using MiNET.Utils;

namespace MiNET.Entities.Behaviors
{
	public class EatBlockBehavior : IBehavior
	{
		private int _duration;

		public EatBlockBehavior()
		{
		}

		public bool ShouldStart(Entity entity)
		{
			if (entity.Level.Random.Next(1000) != 0) return false;

			var coordinates = entity.KnownPosition;
			var direction = Vector3.Normalize(coordinates.GetHeadDirection());

			BlockCoordinates coord = new Vector3(coordinates.X + direction.X, coordinates.Y, coordinates.Z + direction.Z);

			var shouldStart = entity.Level.GetBlock(coord + BlockCoordinates.Down) is Grass || entity.Level.GetBlock(coord) is TallGrass;
			if (!shouldStart) return false;

			_duration = 40;

			return true;
		}

		public bool OnTick(Entity entity)
		{
			return false;
		}

		public bool CalculateNextMove(Entity entity)
		{
			if (_duration == 40)
			{
				entity.Velocity *= new Vector3(0, 1, 0);

				McpeEntityEvent entityEvent = McpeEntityEvent.CreateObject();
				entityEvent.entityId = entity.EntityId;
				entityEvent.eventId = 10;
				entity.Level.RelayBroadcast(entityEvent);
			}

			if (_duration-- < 0)
			{
				return true;
			}

			return false;
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
		}
	}
}