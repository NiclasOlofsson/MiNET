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

using MiNET.Entities.Behaviors;
using MiNET.Items;
using MiNET.Worlds;

namespace MiNET.Entities.Passive
{
	public class Pig : PassiveMob, IAgeable
	{
		public Pig(Level level) : base(EntityType.Pig, level)
		{
			Width = Length = 0.9;
			Height = 0.9;
			HealthManager.MaxHealth = 100;
			HealthManager.ResetHealth();
			Speed = 0.25;

			Behaviors.Add(new PanicBehavior(this, 60, Speed, 1.25));
			Behaviors.Add(new TemptedBehavior(this, typeof(ItemCarrot), 10, 1.2));
			//Behaviors.Add(new TemptedBehavior(this, typeof(ItemCarrotStick), 10, 1.2));
			Behaviors.Add(new WanderBehavior(this, 1.0));
			Behaviors.Add(new LookAtPlayerBehavior(this));
			Behaviors.Add(new RandomLookaroundBehavior(this));
		}

		public override Item[] GetDrops()
		{
			return new[]
			{
				ItemFactory.GetItem(319, 0, 2)
			};
		}
	}
}