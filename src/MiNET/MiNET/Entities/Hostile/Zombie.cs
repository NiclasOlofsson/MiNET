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

using MiNET.Blocks;
using MiNET.Entities.Behaviors;
using MiNET.Utils;
using MiNET.Utils.Metadata;
using MiNET.Worlds;

namespace MiNET.Entities.Hostile
{
	public class Zombie : HostileMob, IAgeable
	{
		public override double Height
		{
			get => IsBaby ? base.Height * 0.5f : base.Height;
			set => base.Height = value;
		}

		public override double Speed
		{
			get => IsBaby ? base.Speed * 1.5f : base.Speed;
			set => base.Speed = value;
		}

		public Zombie(Level level) : base(EntityType.Zombie, level)
		{
			Width = Length = 0.6;
			Height = 1.95;
			NoAi = true;
			Speed = 0.23;

			AttackDamage = 3;

			TargetBehaviors.Add(new HurtByTargetBehavior(this));
			TargetBehaviors.Add(new FindAttackableTargetBehavior(this, 35));

			Behaviors.Add(new MeleeAttackBehavior(this, 1.0, 35));
			Behaviors.Add(new WanderBehavior(this, 1.0));
			Behaviors.Add(new LookAtPlayerBehavior(this, 8.0));
			Behaviors.Add(new RandomLookaroundBehavior(this));
		}

		public override MetadataDictionary GetMetadata()
		{
			var metadata = base.GetMetadata();
			metadata[(int) MetadataFlags.Scale] = new MetadataFloat(IsBaby ? 0.5f : 1.0);
			return metadata;
		}

		public override void OnTick(Entity[] entities)
		{
			base.OnTick(entities);

			Block block = Level.GetBlock(KnownPosition);
			if (!(block is Water) && !(block is FlowingWater) && block.SkyLight > 7 && (Level.CurrentWorldCycleTime < 12566 || Level.CurrentWorldCycleTime > 23450))
			{
				HealthManager.Ignite(160);
			}
		}
	}
}