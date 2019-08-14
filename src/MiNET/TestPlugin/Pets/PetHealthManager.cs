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

using System.Numerics;
using MiNET;
using MiNET.Entities;
using MiNET.Particles;

namespace TestPlugin.Pets
{
	public class PetHealthManager : HealthManager
	{
		public PetHealthManager(Entity entity) : base(entity)
		{
		}

		public override void TakeHit(Entity source, int damage = 1, DamageCause cause = DamageCause.Unknown)
		{
			if (!(source is Player)) return;

			// Pets must die in void or they get stuck forever :-(
			if (cause == DamageCause.Void)
			{
				base.TakeHit(source, damage, cause);
				return; // Love denied!
			}

			int size = Entity.Level.Random.Next(0, 3); // The size of the hearts

			Pet pet = Entity as Pet;
			if (pet != null)
			{
				if (pet.AttackTarget != null) return;

				// He is still angry, better not pet him right now.
				if (!pet.IsInRage && IsOnFire && pet.Level.Random.Next(10) == 0)
				{
					pet.AttackTarget = (Player) source;
					pet.RageTick = 20 * 3;
					return;
				}

				// Only owner do petting with my pet!
				if (pet.Owner == source)
				{
					// Don't trust animals!
					if (pet.Level.Random.Next(500) == 0)
					{
						pet.AttackTarget = (Player) source;
						pet.RageTick = 20 * 2;
						return;
					}

					LegacyParticle particle = new HeartParticle(pet.Level, size);
					particle.Position = Entity.KnownPosition.ToVector3() + new Vector3(0, (float) (Entity.Height + 0.85d), 0);
					particle.Spawn();
				}
				else
				{
					// HAHA Steal IT!
					if (pet.Level.Random.Next(50) == 0)
					{
						pet.Owner = (Player) source;
						pet.AttackTarget = null;
						pet.RageTick = 0;
						return;
					}

					// Don't trust animals!
					if (pet.Level.Random.Next(30) == 0)
					{
						pet.AttackTarget = (Player) source;
						pet.RageTick = 20 * 3;
						return;
					}
				}
			}
		}

		public override void OnTick()
		{
			Health = 200;
			base.OnTick();
		}
	}
}