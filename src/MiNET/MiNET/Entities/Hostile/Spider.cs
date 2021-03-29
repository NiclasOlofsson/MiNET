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
using MiNET.Entities.Behaviors;
using MiNET.Items;
using MiNET.Utils;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Entities.Hostile
{
	public class Spider : HostileMob
	{
		public Spider(Level level) : base(EntityType.Spider, level)
		{
			Width = Length = 1.4;
			Height = 0.9;
			NoAi = true;
			Speed = 0.3;

			CanClimb = true;

			HealthManager.MaxHealth = 160;
			HealthManager.ResetHealth();

			AttackDamage = 3;

			TargetBehaviors.Add(new HurtByTargetBehavior(this));
			TargetBehaviors.Add(new SpiderTargetBehavior(this));

			Behaviors.Add(new SpiderAttackBehavior(this, 1.0, 16));
			Behaviors.Add(new WanderBehavior(this, 0.8));
			Behaviors.Add(new LookAtPlayerBehavior(this, 8.0));
			Behaviors.Add(new RandomLookaroundBehavior(this));
		}

		public override Item[] GetDrops()
		{
			var rnd = new Random();
			Item[] result = new Item[0];

			var count = rnd.Next(2);
			if (count > 0)
			{
				result = result.Concat(new[]
				{
					ItemFactory.GetItem(287, 0, (byte) count)
				}).ToArray();
			}

			if (rnd.NextDouble() < 0.33)
			{
				result = result.Concat(new[]
				{
					ItemFactory.GetItem(375),
				}).ToArray();
			}

			return result;
		}

		public class SpiderAttackBehavior : MeleeAttackBehavior
		{
			public SpiderAttackBehavior(Mob entity, double speedMultiplier, double followRange) : base(entity, speedMultiplier, followRange)
			{
			}

			public override bool CanContinue()
			{
				BlockCoordinates coord = (BlockCoordinates) _entity.KnownPosition;
				if ((_entity.Level.GetSkyLight(coord) >= 11 || _entity.Level.GetBlockLight(coord) >= 11) && _entity.Level.Random.Next(100) == 0)
				{
					_entity.SetTarget(null);
					return false;
				}

				return base.CanContinue();
			}
		}

		public class SpiderTargetBehavior : FindAttackableTargetBehavior
		{
			public SpiderTargetBehavior(Mob entity, double targetDistance = 16) : base(entity, targetDistance)
			{
			}

			public override bool ShouldStart()
			{
				BlockCoordinates coord = (BlockCoordinates) _entity.KnownPosition;
				if (_entity.Level.GetSkyLight(coord) >= 11 || _entity.Level.GetBlockLight(coord) >= 11) return false;

				return base.ShouldStart();
			}
		}
	}
}