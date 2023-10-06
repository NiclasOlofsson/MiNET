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
using MiNET.Net;
using MiNET.Utils;
using MiNET.Utils.Metadata;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Entities.World
{
	public class PrimedTnt : Entity
	{
		public byte Fuse { get; set; }
		public bool Fire { get; set; }

		public PrimedTnt(Level level) : base(EntityType.PrimedTnt, level)
		{
			IsIgnited = true;
			NoAi = false;
			HasCollision = true;

			Gravity = 0.04;
			Drag = 0.02;
		}

		public override MetadataDictionary GetMetadata()
		{
			return new MetadataDictionary
			{
				[(int) MetadataFlags.EntityFlags] = new MetadataLong(GetDataValue()),
				[(int) MetadataFlags.DataFuseLength] = new MetadataInt(Fuse)
			};
		}

		public override void SpawnEntity()
		{
			Fire = false; // Why false?

			base.SpawnEntity();
		}

		public override void OnTick(Entity[] entities)
		{
			Fuse--;

			if (Fuse == 0)
			{
				DespawnEntity();
				Explode();
			}
			else
			{
				PositionCheck();

				if (KnownPosition.Y > -1 && _checkPosition)
				{
					Velocity -= new Vector3(0, (float) Gravity, 0);
					Velocity *= (float) (1.0f - Drag);
				}

				var entityData = McpeSetEntityData.CreateObject();
				entityData.runtimeEntityId = EntityId;
				entityData.metadata = GetMetadata();
				Level.RelayBroadcast(entityData);
			}
		}

		private bool _checkPosition = true;

		private void PositionCheck()
		{
			if (Velocity.Y < -0.1)
			{
				int distance = (int) Math.Ceiling(Velocity.Length());
				BlockCoordinates check = new BlockCoordinates(KnownPosition);
				for (int i = 0; i < distance; i++)
				{
					if (Level.GetBlock(check).IsSolid)
					{
						_checkPosition = false;
						KnownPosition = check.BlockUp();
						return;
					}
					check = check.BlockDown();
				}
			}
			KnownPosition.X += (float) Velocity.X;
			KnownPosition.Y += (float) Velocity.Y;
			KnownPosition.Z += (float) Velocity.Z;
		}

		private void Explode()
		{
			// Litteral "fire and forget"
			new Explosion(Level,
					new BlockCoordinates((int) Math.Floor(KnownPosition.X), (int) Math.Floor(KnownPosition.Y), (int) Math.Floor(KnownPosition.Z)), 4, Fire)
				.Explode();
		}
	}
}