using MiNET.Blocks;
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

			ItemInHand = ItemFactory.GetItem("bow");

			Behaviors.Add(new StrollBehavior(this, 60, Speed, 0.7));
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

		public override void OnTick()
		{
			base.OnTick();

			Block block = Level.GetBlock(KnownPosition);
			if (!(block is StationaryWater) && !(block is FlowingWater) && block.SkyLight > 7 && (Level.CurrentWorldTime > 450 && Level.CurrentWorldTime < 11615))
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