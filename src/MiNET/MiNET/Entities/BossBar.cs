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
using MiNET.Items;
using MiNET.Net;
using MiNET.Worlds;

namespace MiNET.Entities
{
	public class BossBar : Entity
	{
		public class NoDamageHealthManager : HealthManager
		{
			public NoDamageHealthManager(Entity entity) : base(entity)
			{
			}

			public override void TakeHit(Entity source, Item tool, int damage = 1, DamageCause cause = DamageCause.Unknown)
			{
			}

			public override void TakeHit(Entity source, int damage = 1, DamageCause cause = DamageCause.Unknown)
			{
			}

			public override void OnTick()
			{
			}
		}

		public bool IsVisible { get; set; } = true;
		public bool Animate { get; set; } = false;
		public int Progress { get; set; } = 100;
		public int MaxProgress { get; set; } = 100;

		public BossBar(Level level) : base(EntityType.Slime, level)
		{
			Width = 0;
			Length = 0;
			Height = 0;

			HideNameTag = true;
			IsAlwaysShowName = false;
			IsInvisible = true;
			IsSilent = true;
			HealthManager = new NoDamageHealthManager(this);

			KnownPosition = level.SpawnPoint;
		}

		[Wired]
		public virtual void SetNameTag(string nameTag)
		{
			NameTag = nameTag;

			var bossEvent = McpeBossEvent.CreateObject();
			bossEvent.bossEntityId = EntityId;
			bossEvent.title = NameTag;
			bossEvent.eventType = 5;
			Level?.RelayBroadcast(bossEvent);
		}

		[Wired]
		public virtual void SetProgress(int progress = Int32.MinValue, int maxProgress = Int32.MinValue)
		{
			if (progress != Int32.MinValue) Progress = progress;
			if (maxProgress != Int32.MinValue) MaxProgress = maxProgress;

			var bossEvent = McpeBossEvent.CreateObject();
			bossEvent.bossEntityId = EntityId;
			bossEvent.healthPercent = (float) Progress / MaxProgress;
			bossEvent.eventType = 4;
			Level?.RelayBroadcast(bossEvent);
		}

		public override void SpawnToPlayers(Player[] players)
		{
			base.SpawnToPlayers(players);

			var bossEvent = McpeBossEvent.CreateObject();
			bossEvent.bossEntityId = EntityId;
			bossEvent.eventType = (uint) (IsVisible ? 0 : 2);
			bossEvent.title = NameTag;
			bossEvent.healthPercent = (float)Progress / MaxProgress;
			Level?.RelayBroadcast(players, bossEvent);
		}

		public override void DespawnFromPlayers(Player[] players)
		{
			base.DespawnFromPlayers(players);

			var bossEvent = McpeBossEvent.CreateObject();
			bossEvent.bossEntityId = EntityId;
			bossEvent.eventType = 2;
			Level?.RelayBroadcast(players, bossEvent);
		}

		public override void OnTick(Entity[] entities)
		{
			base.OnTick(entities);

			if (!Animate) return;

			if (Level.TickTime % 2 == 0)
			{
				if (Progress > MaxProgress) Progress = 0;
				SetProgress();
				Progress++;
			}
		}
	}
}