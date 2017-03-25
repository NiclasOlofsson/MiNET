using System;
using System.Numerics;
using MiNET.Blocks;
using MiNET.Net;
using MiNET.Utils;

namespace MiNET.Entities.Behaviors
{
	public class EatBlockBehavior : IBehavior
	{
		private int _duration;
		private int _timeLeft;

		public EatBlockBehavior(int duration)
		{
			_duration = Math.Max(40, duration);
			_timeLeft = _duration;
		}

		public bool ShouldStart(Entity entity)
		{
			var coordinates = entity.KnownPosition;
			var direction = Vector3.Normalize(coordinates.GetHeadDirection());

			BlockCoordinates coord = new Vector3(coordinates.X + direction.X, coordinates.Y, coordinates.Z + direction.Z);
			coord += BlockCoordinates.Down;

			if (!(entity.Level.GetBlock(coord) is Grass))
			{
				return false;
			}

			return true;
		}

		public bool OnTick(Entity entity)
		{
			return false;
		}

		public bool CalculateNextMove(Entity entity)
		{
			var coordinates = entity.KnownPosition;
			var direction = Vector3.Normalize(coordinates.GetHeadDirection());

			BlockCoordinates coord = new Vector3(coordinates.X + direction.X, coordinates.Y, coordinates.Z + direction.Z);
			coord += BlockCoordinates.Down;

			if (_timeLeft == _duration)
			{
				entity.Velocity *= new Vector3(0, 1, 0);
				McpeEntityEvent entityEvent = McpeEntityEvent.CreateObject();
				entityEvent.entityId = entity.EntityId;
				entityEvent.eventId = 10;
				entity.Level.RelayBroadcast(entityEvent);

				//entity.NoAi = false;
				//entity.BroadcastSetEntityData();
			}

			if (_timeLeft == _duration - 40)
			{
				McpeLevelEvent levelEvent = McpeLevelEvent.CreateObject();
				levelEvent.position = coord;
				levelEvent.eventId = 2001;
				levelEvent.data = 2;
				entity.Level.RelayBroadcast(levelEvent);

				entity.Level.SetBlock(new Dirt() {Coordinates = coord});

				//entity.NoAi = true;
				//entity.BroadcastSetEntityData();
				//entity.IsSheared = true;
				//entity.IsFlagAllFlying = true;
				//entity.IsElder = true;
				//entity.IsMoving = true;
				//entity.BroadcastSetEntityData();
			}

			if (_timeLeft-- <= 0)
			{
				_timeLeft = _duration;
				return true;
			}

			return false;
		}
	}
}