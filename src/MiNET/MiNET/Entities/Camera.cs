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

using MiNET.Items;
using MiNET.Net;
using MiNET.Utils;
using MiNET.Utils.Metadata;
using MiNET.Worlds;

namespace MiNET.Entities
{
	public class Camera : Entity
	{
		private long _countdown;

		public string InteractionLabel { get; set; } = "action.interact.takepicture";


		public Camera(Level level) : base(EntityType.Camera, level)
		{
			Width = Length = 0.75f;
			Height = 1.8f;

			HasCollision = true;
			IsAffectedByGravity = true;

			HealthManager.IsInvulnerable = true;
		}

		public override MetadataDictionary GetMetadata()
		{
			var metadata = base.GetMetadata();
			metadata[40] = new MetadataString(InteractionLabel);
			return metadata;
		}

		public override void DoInteraction(int actionId, Player player)
		{
			McpeCamera camera = McpeCamera.CreateObject();
			camera.unknown1 = EntityId;
			camera.unknown2 = player.EntityId;
			player.Level.RelayBroadcast(camera);

			_countdown = Age + 100;
			InteractionLabel = "";
			BroadcastSetEntityData();
		}

		public override void OnTick(Entity[] entities)
		{
			base.OnTick(entities);

			if (!IsSpawned) return;

			if (_countdown > 0 && Age > _countdown)
			{
				DespawnEntity();
			}
		}


		public override Item[] GetDrops()
		{
			return new[]
			{
				ItemFactory.GetItem(498)
			};
		}
	}
}