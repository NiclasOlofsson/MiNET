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
using System.Linq;
using System.Numerics;
using MiNET.Worlds;

namespace MiNET.Entities.Behaviors
{
	public class FindAttackableTargetBehavior : BehaviorBase, ITargetingBehavior
	{
		protected readonly Mob _entity;
		private readonly double _targetDistance;
		private int _targetUnseenTicks = 0;

		public FindAttackableTargetBehavior(Mob entity, double targetDistance = 16)
		{
			_entity = entity;
			_targetDistance = targetDistance;
		}

		public override bool ShouldStart()
		{
			if (_entity.Level.Random.Next(10) != 0)
			{
				return false;
			}

			var player = _entity.Level.Players
				.OrderBy(p => Vector3.Distance(_entity.KnownPosition, p.Value.KnownPosition))
				.FirstOrDefault(p =>
					p.Value.IsSpawned
					&& !p.Value.HealthManager.IsDead
					&& p.Value.GameMode != GameMode.Creative
					&& p.Value.GameMode != GameMode.Spectator
					&& _entity.DistanceTo(p.Value) < GetTargetDistance(p.Value)).Value;

			if (player == null)
			{
				_entity.SetTarget(null);
				return false;
			}

			_entity.SetTarget(player);

			return true;
		}

		private double GetTargetDistance(Player player)
		{
			double distance = _targetDistance;

			if (player.IsSneaking)
			{
				distance *= 0.8;
			}

			return distance;
		}

		public override void OnStart()
		{
			_targetUnseenTicks = 0;
		}

		public override bool CanContinue()
		{
			var target = _entity.Target;

			if (target == null) return false;

			if (target.HealthManager.IsDead) return false;


			if (!(target is Player))
			{
				if (_entity.DistanceTo(target) > _targetDistance)
				{
					return false;
				}
			}
			else
			{
				if (_entity.DistanceTo(target) > GetTargetDistance((Player) target))
				{
					return false;
				}

				if (_entity.CanSee(target))
				{
					_targetUnseenTicks = 0;
				}
				else if (_targetUnseenTicks++ > 60)
				{
					return false;
				}

				_entity.SetTarget(target); // This makes sense when we to attacked by targeting
			}

			return true;
		}

		public override void OnTick(Entity[] entities)
		{
		}

		public override void OnEnd()
		{
			_entity.SetTarget(null);
		}
	}

	public class FindAttackableEntityTargetBehavior<TEntity> : BehaviorBase, ITargetingBehavior where TEntity : Entity
	{
		private readonly Mob _entity;
		private readonly double _targetDistance;
		private readonly int _attackChance;
		private int _targetUnseenTicks = 0;

		public FindAttackableEntityTargetBehavior(Mob entity, double targetDistance = 16, int attackChance = 10)
		{
			_entity = entity;
			_targetDistance = targetDistance;
			_attackChance = attackChance;
		}

		public override bool ShouldStart()
		{
			if (_entity.Level.Random.Next(_attackChance) != 0)
			{
				return false;
			}

			var target = _entity.Level.Entities
				.OrderBy(p => Vector3.Distance(_entity.KnownPosition, p.Value.KnownPosition))
				.FirstOrDefault(p =>
					p.Value != _entity
					&& p.Value is TEntity
					&& !p.Value.HealthManager.IsDead
					&& _entity.DistanceTo(p.Value) < _targetDistance).Value as TEntity;

			if (target == null)
			{
				_entity.SetTarget(null);
				return false;
			}

			_entity.SetTarget(target);

			return true;
		}

		public override void OnStart()
		{
			_targetUnseenTicks = 0;
		}

		public override bool CanContinue()
		{
			// Give the poor entity a chance to survive
			// Also makes it let go of unreachable targets
			if (_entity.Level.Random.Next(_attackChance) == 0)
			{
				return false;
			}

			var target = _entity.Target;

			if (target == null)
				return false;

			if (target.HealthManager.IsDead)
				return false;


			if (_entity.DistanceTo(target) > _targetDistance)
			{
				return false;
			}

			if (_entity.CanSee(target))
			{
				_targetUnseenTicks = 0;
			}
			else if (_targetUnseenTicks++ > 60)
			{
				return false;
			}

			_entity.SetTarget(target); // This makes sense when we to attacked by targeting

			return true;
		}

		public override void OnTick(Entity[] entities)
		{
		}

		public override void OnEnd()
		{
			_entity.SetTarget(null);
		}
	}

}