#region LICENSE

// The contents of this file are subject to the Common Public Attribution
// License Version 1.0. (the "License"); you may not use this file except in
// compliance with the License. You may obtain a copy of the License at
// https://github.com/NiclasOlofsson/MiNET/blob/master/LICENSE. 
// The License is based on the Mozilla Public License Version 1.1, but Sections 14 
// and 15 have been added to cover use of software over a computer network and 
// provide for limited attribution for the Original Developer. In addition, Exhibit A has 
// been modified to be consistent with Exhibit B.
// 
// Software distributed under the License is distributed on an "AS IS" basis,
// WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License for
// the specific language governing rights and limitations under the License.
// 
// The Original Code is MiNET.
// 
// The Original Developer is the Initial Developer.  The Initial Developer of
// the Original Code is Niclas Olofsson.
// 
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2017 Niclas Olofsson. 
// All Rights Reserved.

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using AStarNavigator;
using log4net;
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
			_attackCooldown = 0;

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
			_attackCooldown = Math.Max(_attackCooldown - 1, 0);

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
					//Log.Debug($"Found no solution. Trying a search at full follow range ({_followRange})");
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
				//Log.Debug($"Found no path solution");
				entity.Velocity = Vector3.Zero;
				_currentPath = null;
				_delay += 15;
			}

			entity.Controller.LookAt(target, true);

			if (_attackCooldown <= 0 && distanceToPlayer < GetAttackReach())
			{
				var damage = _entity.AttackDamage;
				target.HealthManager.TakeHit(_entity, damage, DamageCause.EntityAttack);
				_attackCooldown = 20;
			}
		}

		private double GetAttackReach()
		{
			return _entity.Width*2.0F + _entity.Target.Width;
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
			_attackCooldown = 0;
		}
	}
}