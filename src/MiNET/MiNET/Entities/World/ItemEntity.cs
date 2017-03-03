using System.Numerics;
using log4net;
using MiNET.Items;
using MiNET.Net;
using MiNET.Worlds;

namespace MiNET.Entities.World
{
	public class ItemEntity : Mob
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (ItemEntity));

		public Item Item { get; private set; }
		public int PickupDelay { get; set; }
		public int TimeToLive { get; set; }

		public ItemEntity(Level level, Item item) : base(64, level)
		{
			Item = item;

			Height = 0.25;
			Width = 0.25;
			Length = 0.25;

			PickupDelay = 10;
			TimeToLive = 6000;

			HealthManager.IsInvulnerable = true;

			NoAi = true;
		}

		public Item GetItemStack()
		{
			return Item;
		}

		//public override void SpawnEntity()
		//{
		//	//double f = 0.7;
		//	//float xr = (float)(random.NextDouble() * f + (1.0 - f) * 0.5);
		//	//float yr = (float)(random.NextDouble() * f + (1.0 - f) * 0.5);
		//	//float zr = (float)(random.NextDouble() * f + (1.0 - f) * 0.5);

		//	Level.AddEntity(this);

		//	IsSpawned = true;
		//}

		public override void SpawnToPlayers(Player[] players)
		{
			McpeAddItemEntity mcpeAddItemEntity = McpeAddItemEntity.CreateObject();
			mcpeAddItemEntity.entityId = EntityId;
			mcpeAddItemEntity.runtimeEntityId = EntityId;
			mcpeAddItemEntity.item = GetItemStack();
			mcpeAddItemEntity.x = KnownPosition.X;
			mcpeAddItemEntity.y = KnownPosition.Y;
			mcpeAddItemEntity.z = KnownPosition.Z;
			mcpeAddItemEntity.speedX = (float) Velocity.X;
			mcpeAddItemEntity.speedY = (float) Velocity.Y;
			mcpeAddItemEntity.speedZ = (float) Velocity.Z;
			Level.RelayBroadcast(players, mcpeAddItemEntity);

			BroadcastSetEntityData();
		}

		public override void OnTick()
		{
			if (Velocity == Vector3.Zero)
			{
				if (IsInGround(KnownPosition))
				{
					Velocity += new Vector3(0, (float)Gravity, 0);
				}
				else
				{
					bool onGround = IsOnGround(KnownPosition);
					if (!onGround) Velocity -= new Vector3(0, (float)Gravity, 0);
				}
			}

			base.OnTick();

			TimeToLive--;
			PickupDelay--;

			if (TimeToLive <= 0)
			{
				DespawnEntity();
				return;
			}

			// Motion


			if (PickupDelay > 0) return;

			var players = Level.GetSpawnedPlayers();
			foreach (var player in players)
			{
				if (KnownPosition.DistanceTo(player.KnownPosition) <= 2)
				{
					if (player.GameMode == GameMode.Survival)
					{
						{
							var takeItemEntity = McpeTakeItemEntity.CreateObject();
							takeItemEntity.entityId = EntityId;
							takeItemEntity.target = player.EntityId;
							Level.RelayBroadcast(player, takeItemEntity);
						}
						{
							var takeItemEntity = McpeTakeItemEntity.CreateObject();
							takeItemEntity.entityId = EntityId;
							takeItemEntity.target = 0;
							player.SendPackage(takeItemEntity);
						}

						DespawnEntity();
						break;
					}
				}
			}
		}
	}
}