﻿using MiNET.Blocks;
using MiNET.Entities.Behaviors;
using MiNET.Items;
using MiNET.Net;
using MiNET.Worlds;

namespace MiNET.Entities.Hostile
{
	public class Skeleton : HostileMob
	{
		public short Boots { get; set; }
		public short Leggings { get; set; }
		public short Chest { get; set; }
		public short Helmet { get; set; }

		public Item ItemInHand { get; set; }

		public Skeleton(Level level) : base((int) EntityType.Skeleton, level)
		{
			Width = Length = 0.6;
			Height = 1.95;
			NoAi = true;

			AttackDamage = 4;

			ItemInHand = ItemFactory.GetItem("bow");

			Behaviors.Add(new WanderBehavior(this, Speed, 1.0));
			Behaviors.Add(new LookAtPlayerBehavior(this, 8.0));
			Behaviors.Add(new RandomLookaroundBehavior(this));
		}

		public override void SpawnToPlayers(Player[] players)
		{
			base.SpawnToPlayers(players);
			SendArmor();
			SendEquipment();
		}


		protected virtual void SendEquipment()
		{
			McpeMobEquipment message = McpeMobEquipment.CreateObject();
			message.runtimeEntityId = EntityId;
			message.item = ItemInHand;
			message.slot = 0;
			Level.RelayBroadcast(message);
		}

		protected virtual void SendArmor()
		{
			McpeMobArmorEquipment armorEquipment = McpeMobArmorEquipment.CreateObject();
			armorEquipment.runtimeEntityId = EntityId;
			armorEquipment.helmet = ItemFactory.GetItem(Helmet);
			armorEquipment.chestplate = ItemFactory.GetItem(Chest);
			armorEquipment.leggings = ItemFactory.GetItem(Leggings);
			armorEquipment.boots = ItemFactory.GetItem(Boots);
			Level.RelayBroadcast(armorEquipment);
		}

		public override void OnTick(Entity[] entities)
		{
			base.OnTick(entities);

			Block block = Level.GetBlock(KnownPosition);
			if (!(block is StationaryWater) && !(block is FlowingWater) && block.SkyLight > 7 && (Level.CurrentWorldTime < 12566 || Level.CurrentWorldTime > 23450))
			{
				if (!HealthManager.IsOnFire) HealthManager.Ignite(80);
			}
			else
			{
				if (HealthManager.IsOnFire) HealthManager.Ignite(80); // last kick in the butt
			}
		}
	}
}