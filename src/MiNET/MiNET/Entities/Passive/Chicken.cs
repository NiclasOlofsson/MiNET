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
using System.Numerics;
using MiNET.Entities.Behaviors;
using MiNET.Items;
using MiNET.Worlds;

namespace MiNET.Entities.Passive
{
	public class Chicken : PassiveMob
	{
		private int _timeUntilLayEgg = 0;

		public Chicken(Level level, Random rnd = null) : base(EntityType.Chicken, level)
		{
			Width = Length = 0.4;
			Height = 0.7;
			HealthManager.MaxHealth = 40;
			HealthManager.ResetHealth();
			Speed = 0.25f;

			IsWalker = true;
			CanClimb = true;
			//IsInWater = true;
			HasCollision = true;
			IsAffectedByGravity = true;

			var random = rnd ?? new Random();
			_timeUntilLayEgg = 6000 + random.Next(6000);

			Behaviors.Add(new PanicBehavior(this, 60, Speed, 1.4));
			Behaviors.Add(new TemptedBehavior(this, typeof(ItemWheatSeeds), 10, 1.0)); //TODO: Add other seeds
			Behaviors.Add(new WanderBehavior(this, 1.0));
			Behaviors.Add(new LookAtPlayerBehavior(this));
			Behaviors.Add(new RandomLookaroundBehavior(this));
		}

		public override void OnTick(Entity[] entities)
		{
			base.OnTick(entities);

			if (!IsOnGround && Velocity.Y < 0.0D)
			{
				Velocity *= new Vector3(1, 0.6f, 1);
			}

			if (_timeUntilLayEgg-- <= 0)
			{
				Level.DropItem(KnownPosition, new ItemEgg());
				_timeUntilLayEgg = 6000 + Level.Random.Next(6000);
			}
		}

		public override Item[] GetDrops()
		{
			Random random = new Random();
			return new[]
			{
				ItemFactory.GetItem(365),
				ItemFactory.GetItem(288, 0, random.Next(1, 3)),
			};
		}
	}
}