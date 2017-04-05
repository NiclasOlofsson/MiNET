using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using AStarNavigator;
using log4net;
using MiNET.Entities.Passive;
using MiNET.Utils;

namespace MiNET.Entities.Behaviors
{
	public class MeeleAttackBehavior : IBehavior
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (MeeleAttackBehavior));

		private readonly Wolf _wolf;
		private double _speedMultiplier;

		public MeeleAttackBehavior(Wolf wolf, double speedMultiplier)
		{
			_wolf = wolf;
			_speedMultiplier = speedMultiplier;
		}

		public bool ShouldStart()
		{
			if (_wolf.Target == null) return false;
			if (_wolf.Target.HealthManager.IsDead) return false;

			var currentPath = new PathFinder().FindPath(_wolf, _wolf.Target, 16);
			if (currentPath.Count == 0) return false;

			_lastPlayerPos = _wolf.Target.KnownPosition;

			return true;
		}

		public bool CanContinue()
		{
			return ShouldStart();
		}

		private List<Tile> _currentPath = null;

		private Vector3 _lastPlayerPos;

		public void OnTick()
		{
			Wolf entity = _wolf;
			Entity target = _wolf.Target;

			if (target == null) return;

			var distanceToPlayer = entity.KnownPosition.DistanceTo(target.KnownPosition);

			if (_currentPath == null || _currentPath.Count == 0 || Vector3.Distance(_lastPlayerPos, target.KnownPosition) > 0.01)
			{
				Log.Debug($"Search new solution");
				var pathFinder = new PathFinder();
				_currentPath = pathFinder.FindPath(entity, target, distanceToPlayer + 1);
				if (_currentPath.Count == 0)
				{
					_currentPath = pathFinder.FindPath(entity, target, 16);
				}
			}

			if (_currentPath.Count > 0)
			{
				Tile next;
				if (GetNextTile(out next))
				{
					entity.Controller.RotateTowards(new Vector3((float) next.X + 0.5f, entity.KnownPosition.Y, (float) next.Y + 0.5f));
					entity.Controller.MoveForward(_speedMultiplier);
				}
			}
			else
			{
				Log.Debug($"Found no path solution");
				entity.Velocity = Vector3.Zero;
				_currentPath = null;
			}

			entity.Controller.LookAt(target, true);

			if ((entity.GetBoundingBox() + 0.3f).Intersects(target.GetBoundingBox()))
			{
				target.HealthManager.TakeHit(_wolf, _wolf.IsTamed ? 4 : 2, DamageCause.EntityAttack);
			}
		}

		private bool GetNextTile(out Tile next)
		{
			next = new Tile();
			if (_currentPath.Count == 0) return false;

			next = _currentPath.First();

			BlockCoordinates currPos = (BlockCoordinates) _wolf.KnownPosition;
			if ((int) next.X == currPos.X && (int) next.Y == currPos.Z)
			{
				_currentPath.Remove(next);

				if (!GetNextTile(out next)) return false;
			}

			return true;
		}

		public void OnEnd()
		{
			if (_wolf.Target == null) return;
			if (_wolf.Target.HealthManager.IsDead) _wolf.SetTarget(null);
			_wolf.Velocity = Vector3.Zero;
			_wolf.KnownPosition.Pitch = 0;
		}
	}
}