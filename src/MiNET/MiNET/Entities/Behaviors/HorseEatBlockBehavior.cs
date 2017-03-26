using System;
using System.Numerics;
using MiNET.Blocks;
using MiNET.Entities.Passive;
using MiNET.Net;
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
			Horse horse = entity as Horse;
			if (horse == null) return false;

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
			Horse horse = entity as Horse;
			if (horse == null) return true;

			var coordinates = entity.KnownPosition;
			var direction = Vector3.Normalize(coordinates.GetHeadDirection());

			BlockCoordinates coord = new Vector3(coordinates.X + direction.X, coordinates.Y, coordinates.Z + direction.Z);
			coord += BlockCoordinates.Down;

			if (_timeLeft == _duration)
			{
				entity.Velocity *= new Vector3(0, 1, 0);
				SetEating(horse, true);
			}

			if (_timeLeft-- <= 0)
			{
				McpeLevelEvent levelEvent = McpeLevelEvent.CreateObject();
				levelEvent.position = coord;
				levelEvent.eventId = 2001;
				levelEvent.data = 2;
				entity.Level.RelayBroadcast(levelEvent);

				entity.Level.SetBlock(new Dirt() {Coordinates = coord});
				SetEating(horse, false);

				_timeLeft = _duration;
				return true;
			}

			return false;
		}

		public bool CalculateNextMove(Entity entity)
		{
			return false;
		}

		private void SetEating(Horse horse, bool isEating)
		{
			horse.IsEating = isEating;
			horse.BroadcastSetEntityData();
		}

		public void OnEnd(Entity entity)
		{
			SetEating((Horse) entity, false);
			_timeLeft = _duration;
		}

	}
}