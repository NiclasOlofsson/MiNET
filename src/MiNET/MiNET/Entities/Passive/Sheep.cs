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
using MiNET.Entities.Behaviors;
using MiNET.Items;
using MiNET.Utils;
using MiNET.Utils.Metadata;
using MiNET.Worlds;

namespace MiNET.Entities.Passive
{
	public class Sheep : PassiveMob, IAgeable
	{
		private byte _color = 0;

		public Sheep(Level level, Random rnd = null) : base(EntityType.Sheep, level)
		{
			Width = Length = 0.9;
			Height = 1.3;
			HealthManager.MaxHealth = 80;
			HealthManager.ResetHealth();
			Speed = 0.23f;

			Random random = rnd ?? new Random();
			var d = random.NextDouble();
			_color = (byte) (d < 0.81 ? 0 : d < 0.86 ? 8 : d < 0.91 ? 7 : d < 0.96 ? 15 : d < 0.99 ? 12 : 6);

			Behaviors.Add(new PanicBehaviorNew(this, 60, 1.25));
			Behaviors.Add(new TemptedBehavior(this, typeof(ItemWheat), 10, 1.1));
			Behaviors.Add(new EatBlockBehavior(this));
			Behaviors.Add(new WanderBehavior(this, 1.0));
			Behaviors.Add(new LookAtPlayerBehavior(this));
			Behaviors.Add(new RandomLookaroundBehavior(this));
		}

		public override MetadataDictionary GetMetadata()
		{
			var metadata = base.GetMetadata();
			metadata[3] = new MetadataByte(_color);
			return metadata;
		}

		public override Item[] GetDrops()
		{
			Random random = new Random();
			return new[]
			{
				ItemFactory.GetItem(35, 0, 1),
				ItemFactory.GetItem(423, 0, random.Next(1, 3)),
			};
		}
	}
}