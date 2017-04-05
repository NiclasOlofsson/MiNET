using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using AStarNavigator;
using log4net;
using MiNET.Entities.Passive;
using MiNET.Utils;

namespace MiNET.Entities.Behaviors
{
	public class FollowOwnerBehavior : IBehavior
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (TemptedBehavior));

		private readonly Wolf _entity;
		private readonly double _lookDistance;
		private readonly double _speedMultiplier;

		public FollowOwnerBehavior(Wolf entity, double lookDistance, double speedMultiplier)
		{
			_entity = entity;
			_lookDistance = lookDistance;
			_speedMultiplier = speedMultiplier;
		}

		public bool ShouldStart()
		{
			if (!_entity.IsTamed) return false;
			if (_entity.Owner == null) return false;

			return true;
		}

		public bool CanContinue()
		{
			return ShouldStart();
		}

		private List<Tile> _currentPath = null;

		public void OnTick()
		{
			if (_entity.Owner == null) return;
			Player player = (Player) _entity.Owner;

			var distanceToPlayer = _entity.KnownPosition.DistanceTo(player.KnownPosition);

			if (distanceToPlayer < 1.75)
			{
				// if within 6m stop following (walking)
				_entity.Velocity = Vector3.Zero;
				_entity.Controller.LookAt(player);
				return;
			}

			if (_currentPath == null || _currentPath.Count == 0)
			{
				Log.Debug($"Search new solution");
				var pathFinder = new PathFinder();
				_currentPath = pathFinder.FindPath(_entity, player, distanceToPlayer + 1);
				if (_currentPath.Count == 0)
				{
					_currentPath = pathFinder.FindPath(_entity, player, _lookDistance);
				}
			}

			if (_currentPath.Count > 0)
			{
				Tile next;
				if (!GetNextTile(out next)) return;

				_entity.Controller.RotateTowards(new Vector3((float) next.X + 0.5f, _entity.KnownPosition.Y, (float) next.Y + 0.5f));

				if (distanceToPlayer < 1.75)
				{
					// if within 6m stop following (walking)
					_entity.Velocity = Vector3.Zero;
					_currentPath = null;
				}
				else
				{
					// else find path to player

					var m = 2 - distanceToPlayer;
					if (m <= 0)
					{
						m = 1;
					}
					else
					{
						m = m/2.0;
					}
					//double m = 1;
					_entity.Controller.MoveForward(_speedMultiplier*m);
				}
			}
			else
			{
				Log.Debug($"Found no path solution");
				_entity.Velocity = Vector3.Zero;
				_currentPath = null;
			}

			_entity.Controller.LookAt(player, true);
		}

		private bool GetNextTile(out Tile next)
		{
			next = new Tile();
			if (_currentPath.Count == 0) return false;

			next = _currentPath.First();

			BlockCoordinates currPos = (BlockCoordinates) _entity.KnownPosition;
			if ((int) next.X == currPos.X && (int) next.Y == currPos.Z)
			{
				_currentPath.Remove(next);

				if (!GetNextTile(out next)) return false;
			}

			return true;
		}

		public double Distance(Player player, Tile tile)
		{
			Vector2 pos1 = new Vector2(player.KnownPosition.X, player.KnownPosition.Z);
			Vector2 pos2 = new Vector2((float) tile.X, (float) tile.Y);
			return (pos1 - pos2).Length();
		}

		public void OnEnd()
		{
			_entity.Velocity = Vector3.Zero;
			_entity.KnownPosition.Pitch = 0;
		}
	}
}