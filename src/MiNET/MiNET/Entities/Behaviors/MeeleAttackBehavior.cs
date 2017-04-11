using System;
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

		private readonly Mob _entity;
		private double _speedMultiplier;
		private readonly double _followRange;

		private int _cooldown;
		private List<Tile> _currentPath;
		private Vector3 _lastPlayerPos;

		public MeeleAttackBehavior(Mob entity, double speedMultiplier, double followRange = 16)
		{
			_entity = entity;
			_speedMultiplier = speedMultiplier;
			_followRange = followRange;
		}

		public bool ShouldStart()
		{
			if (_entity.Target == null) return false;
			if (_entity.Target.HealthManager.IsDead) return false;

			//var currentPath = new PathFinder().FindPath(_entity, _entity.Target, _followRange);
			//if (currentPath.Count == 0)
			//{
			//	return false;
			//}

			_lastPlayerPos = _entity.Target.KnownPosition;

			return true;
		}

		public bool CanContinue()
		{
			if (_entity.Target == null) return false;
			if (_entity.Target.HealthManager.IsDead) return false;

			if (_entity.DistanceTo(_entity.Target) > _followRange*_followRange || Math.Abs(_entity.KnownPosition.Y - _entity.Target.KnownPosition.Y) > _entity.Height + 1)
			{
				_entity.SetTarget(null);
				return false;
			}

			return true;
		}

		public void OnTick()
		{
			if (_cooldown-- > 0) return;
			_cooldown = 0;

			Mob entity = _entity;
			Entity target = _entity.Target;

			if (target == null) return;

			var distanceToPlayer = entity.KnownPosition.DistanceTo(target.KnownPosition);

			var haveNoPath = (_currentPath == null || _currentPath.Count == 0);
			if (haveNoPath || Vector3.Distance(_lastPlayerPos, target.KnownPosition) > 0.01)
			{
				Log.Debug($"Search new solution. Have no path={haveNoPath}");
				var pathFinder = new PathFinder();
				_currentPath = pathFinder.FindPath(entity, target, distanceToPlayer + 1);
				if (_currentPath.Count == 0)
				{
					Log.Debug($"Found no solution, trying a search at full distance");
					_currentPath = pathFinder.FindPath(entity, target, _followRange);
				}
			}

			_lastPlayerPos = target.KnownPosition;

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
				var damage = !(_entity is Wolf) ? 0 : _entity.IsTamed ? 4 : 2;
				target.HealthManager.TakeHit(_entity, damage, DamageCause.EntityAttack);
				_cooldown = 10;
			}
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

		public void OnEnd()
		{
			if (_entity.Target == null) return;
			if (_entity.Target.HealthManager.IsDead) _entity.SetTarget(null);
			_entity.Velocity = Vector3.Zero;
			_entity.KnownPosition.Pitch = 0;
		}
	}
}