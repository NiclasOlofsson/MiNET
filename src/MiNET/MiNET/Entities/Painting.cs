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
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Entities
{
	public class Painting : Entity
	{
		public int FacingDirection { get; set; }
		public ItemPainting.PaintingData PaintingData { get; set; }
		public BoundingBox Bbox { get; set; }

		public Painting(Level level, ItemPainting.PaintingData data) : base(EntityType.Painting, level)
		{
			PaintingData = data;
			HealthManager = new PaintingHealthManager(this);
			CanDespawn = false;
		}

		public override BoundingBox GetBoundingBox()
		{
			return Bbox;
		}

		public override void SpawnToPlayers(Player[] players)
		{
			McpeAddPainting painting = McpeAddPainting.CreateObject();
			painting.coordinates = (BlockCoordinates) KnownPosition;
			painting.direction = FacingDirection;
			painting.entityIdSelf = EntityId;
			painting.runtimeEntityId = EntityId;
			painting.title = PaintingData.Title;

			Level.RelayBroadcast(players, painting);
		}

		public override void DoItemInteraction(Player player, Item itemInHand)
		{
			if (itemInHand is ItemPainting)
			{
				// Implement paintings cycler here
			}
		}

		private class PaintingHealthManager : HealthManager
		{
			public PaintingHealthManager(Entity entity) : base(entity)
			{
			}

			public override void TakeHit(Entity source, int damage = 1, DamageCause cause = DamageCause.Unknown)
			{
				Entity.DespawnEntity();
			}

			public override void TakeHit(Entity source, Item tool, int damage = 1, DamageCause cause = DamageCause.Unknown)
			{
				Entity.DespawnEntity();
			}

			public override void OnTick()
			{
			}
		}
	}
}