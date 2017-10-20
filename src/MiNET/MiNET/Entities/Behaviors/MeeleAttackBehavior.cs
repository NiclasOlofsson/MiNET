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

		private int _attackCooldown;
		private int _delay;
		private List<Tile> _currentPath;
		private Vector3 _lastPlayerPos;
		private PathFinder _pathFinder = new PathFinder();

		public MeeleAttackBehavior(Mob entity, double speedMultiplier, double followRange)
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

			//if (_entity.DistanceTo(_entity.Target) > _followRange*_followRange || Math.Abs(_entity.KnownPosition.Y - _entity.Target.KnownPosition.Y) > _entity.Height + 1)
			//{
			//	_entity.SetTarget(null);
			//	return false;
			//}

			if (_entity.DistanceTo(_entity.Target) > _followRange)
			{
				_entity.SetTarget(null);
				return false;
			}

			return true;
		}

		public void OnTick(Entity[] entities)
		{
			--_delay;
			--_attackCooldown;

			if (_attackCooldown > 0) return;

			_attackCooldown = 0;

			Mob entity = _entity;
			Entity target = _entity.Target;

			if (target == null) return;

			double distanceToPlayer = entity.KnownPosition.DistanceTo(target.KnownPosition);

			bool haveNoPath = (_currentPath == null || _currentPath.Count == 0);
			if (haveNoPath && _delay > 0) return;

			if (haveNoPath || Vector3.Distance(_lastPlayerPos, target.KnownPosition) > 0.5)
			{
				//Log.Debug($"Search new solution to player (distance={distanceToPlayer + 1}). Have path={!haveNoPath}");
				_currentPath = _pathFinder.FindPath(entity, target, distanceToPlayer + 1);
				if (_currentPath.Count == 0)
				{
					Log.Debug($"Found no solution. Trying a search at full follow range ({_followRange})");
					_currentPath = _pathFinder.FindPath(entity, target, _followRange);
				}
			}

			_lastPlayerPos = target.KnownPosition;

			_delay = 4 + entity.Level.Random.Next(7);

			if (_currentPath.Count > 0)
			{
				Tile next;
				if (GetNextTile(out next))
				{
					entity.Controller.RotateTowards(new Vector3((float) next.X + 0.5f, entity.KnownPosition.Y, (float) next.Y + 0.5f));
					entity.Controller.MoveForward(_speedMultiplier, entities);
				}

				if (distanceToPlayer > 32)
				{
					_delay += 10;
				}
				else if (distanceToPlayer > 16)
				{
					_delay += 5;
				}
			}
			else
			{
				Log.Debug($"Found no path solution");
				entity.Velocity = Vector3.Zero;
				_currentPath = null;
				_delay += 15;
			}

			entity.Controller.LookAt(target, true);

			if ((entity.GetBoundingBox() + 0.3f).Intersects(target.GetBoundingBox()))
			{
				var damage = !(_entity is Wolf) ? 0 : _entity.IsTamed ? 4 : 2;
				target.HealthManager.TakeHit(_entity, damage, DamageCause.EntityAttack);
				_attackCooldown = 20;
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