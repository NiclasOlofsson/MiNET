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
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2018 Niclas Olofsson. 
// All Rights Reserved.

#endregion

using System;
using System.Diagnostics;
using System.Numerics;
using AStarNavigator;
using log4net;

namespace MiNET.Entities.Behaviors
{
	public class MeleeAttackBehavior : BehaviorBase
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(MeleeAttackBehavior));

		protected readonly Mob _entity;
		private double _speedMultiplier;
		private readonly double _followRange;

		private int _attackCooldown;
		private int _delay;
		private Path _currentPath;
		private Vector3 _lastPlayerPos;

		public MeleeAttackBehavior(Mob entity, double speedMultiplier, double followRange)
		{
			_entity = entity;
			_speedMultiplier = speedMultiplier;
			_followRange = followRange;
		}

		public override bool ShouldStart()
		{
			var target = _entity.Target;
			if (target == null) return false;

			var pathfinder = new Pathfinder();
			_currentPath = pathfinder.FindPath(_entity, target, _followRange);

			if (!_currentPath.HavePath()) return false;

			_lastPlayerPos = target.KnownPosition;

			return true;
		}

		public override void OnStart()
		{
			_delay = 0;
		}

		public override bool CanContinue()
		{
			return _entity.Target != null;
		}

		public override void OnTick(Entity[] entities)
		{
			Entity target = _entity.Target;
			if (target == null) return;

			double distanceToPlayer = _entity.DistanceTo(target);

			--_delay;

			float deltaDistance = Vector3.Distance(_lastPlayerPos, target.KnownPosition);

			bool canSee = _entity.CanSee(target);

			if (canSee || _delay <= 0 || deltaDistance > 1 || _entity.Level.Random.NextDouble() < 0.05)
			{
				var pathfinder = new Pathfinder();
				Stopwatch sw = Stopwatch.StartNew();
				_currentPath = pathfinder.FindPath(_entity, target, _followRange);
				if (Log.IsDebugEnabled)
				{
					sw.Stop();
					if (sw.ElapsedMilliseconds > 5) Log.Warn($"A* search for {_entity.GetType()} on a distance of {_followRange}. Spent {sw.ElapsedMilliseconds}ms and lenght of path is {_currentPath.Current.Count}");
					// DEBUG
					//_currentPath.PrintPath(_entity.Level);
				}

				_lastPlayerPos = target.KnownPosition;

				_delay = 4 + _entity.Level.Random.Next(7);

				if (distanceToPlayer > 32)
				{
					_delay += 10;
				}
				else if (distanceToPlayer > 16)
				{
					_delay += 5;
				}

				if (_currentPath.NoPath())
				{
					_delay += 15;
				}
			}

			// Movement
			if (_currentPath.HavePath())
			{
				if (_currentPath.GetNextTile(_entity, out Tile next /*, true*/))
				{
					_entity.Controller.RotateTowards(new Vector3(next.X + 0.5f, _entity.KnownPosition.Y, next.Y + 0.5f));
					_entity.Controller.MoveForward(_speedMultiplier, entities);
				} // else something is really wrong
			}
			else
			{
				_entity.Velocity = Vector3.Zero;
			}

			_entity.Controller.LookAt(target);

			_attackCooldown = Math.Max(_attackCooldown - 1, 0);
			if (_attackCooldown <= 0 && distanceToPlayer < GetAttackReach())
			{
				var damage = _entity.AttackDamage;
				target.HealthManager.TakeHit(_entity, damage, DamageCause.EntityAttack);
				_attackCooldown = 20;
			}
		}

		private double GetAttackReach()
		{
			return _entity.Width * 2.0F + _entity.Target.Width;
		}

		public override void OnEnd()
		{
			_entity.Velocity = Vector3.Zero;
			_entity.KnownPosition.Pitch = 0;
			_attackCooldown = 0;
			_delay = 0;
			_currentPath = null;
		}
	}
}