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
using log4net;
using MiNET.Blocks;
using MiNET.Items;
using MiNET.Net;
using MiNET.Utils;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Entities.World
{
	public class ItemEntity : Mob
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(ItemEntity));

		public Item Item { get; set; }
		public int PickupDelay { get; set; }
		public int TimeToLive { get; set; }

		public ItemEntity(Level level, Item item) : base(EntityType.DroppedItem, level)
		{
			Item = item;

			Height = 0.25;
			Width = 0.25;
			Length = 0.25;

			Gravity = 0.04;
			Drag = 0.02;

			PickupDelay = 10;
			TimeToLive = 6000;

			HealthManager.IsInvulnerable = true;

			//NoAi = true;
			//HasCollision = false;
			//IsAffectedByGravity = false;

			NoAi = false;
			HasCollision = false;
			IsAffectedByGravity = false;
		}

		public Item GetItemStack()
		{
			return Item;
		}

		public override void SpawnToPlayers(Player[] players)
		{
			McpeAddItemEntity mcpeAddItemEntity = McpeAddItemEntity.CreateObject();
			mcpeAddItemEntity.entityIdSelf = EntityId;
			mcpeAddItemEntity.runtimeEntityId = EntityId;
			mcpeAddItemEntity.item = GetItemStack();
			mcpeAddItemEntity.x = KnownPosition.X;
			mcpeAddItemEntity.y = KnownPosition.Y;
			mcpeAddItemEntity.z = KnownPosition.Z;
			mcpeAddItemEntity.speedX = Velocity.X;
			mcpeAddItemEntity.speedY = Velocity.Y;
			mcpeAddItemEntity.speedZ = Velocity.Z;
			mcpeAddItemEntity.metadata = GetMetadata();
			LastSentPosition = (PlayerLocation) KnownPosition.Clone();
			Level.RelayBroadcast(players, mcpeAddItemEntity);

			BroadcastSetEntityData();
		}

		public override void OnTick(Entity[] entities)
		{
			if (Velocity == Vector3.Zero)
			{
				// Object was resting and now someone removed the block on which it was resting
				// or someone places a block over it.
				if (IsMobInGround(KnownPosition))
				{
					Velocity += new Vector3(0, (float) Gravity, 0);
				}
				else
				{
					bool onGround = IsMobOnGround(KnownPosition);
					if (!onGround) Velocity -= new Vector3(0, (float) Gravity, 0);
				}
			}

			if (Velocity.Length() > 0.01)
			{
				bool onGroundBefore = IsMobOnGround(KnownPosition);

				if (IsMobInGround(KnownPosition))
				{
					Velocity += new Vector3(0, (float) Gravity, 0);
					KnownPosition.X += Velocity.X;
					KnownPosition.Y += Velocity.Y;
					KnownPosition.Z += Velocity.Z;
					BroadcastMove();
					BroadcastMotion();
					return;
				}

				Vector3 adjustedVelocity = GetAdjustedLengthFromCollision(Velocity);

				KnownPosition.X += adjustedVelocity.X;
				KnownPosition.Y += adjustedVelocity.Y;
				KnownPosition.Z += adjustedVelocity.Z;

				BroadcastMove();
				BroadcastMotion();

				bool adjustAngle = adjustedVelocity != Velocity;
				if (adjustAngle)
				{
					CheckBlockAhead();
				}

				bool onGround = IsMobOnGround(KnownPosition);

				if (!onGroundBefore && onGround)
				{
					float ff = 0.6f * 0.98f;
					Velocity *= new Vector3(ff, 0, ff);
				}
				else
				{
					Velocity *= (float) (1.0 - Drag);

					if (!onGround)
					{
						Velocity -= new Vector3(0, (float) Gravity, 0);
					}
					else
					{
						float ff = 0.6f * 0.98f;
						Velocity *= new Vector3(ff, 0, ff);
					}
				}
			}
			else if (Velocity != Vector3.Zero)
			{
				KnownPosition.X += (float) Velocity.X;
				KnownPosition.Y += (float) Velocity.Y;
				KnownPosition.Z += (float) Velocity.Z;

				Velocity = Vector3.Zero;
				LastUpdatedTime = DateTime.UtcNow;
				NoAi = true;
				BroadcastMove(true);
				BroadcastMotion(true);
			}

			TimeToLive--;
			PickupDelay--;

			if (TimeToLive <= 0)
			{
				DespawnEntity();
				return;
			}

			// Motion


			if (PickupDelay > 0) return;

			var bbox = GetBoundingBox();

			var players = Level.GetSpawnedPlayers();
			foreach (var player in players)
			{
				if (player.GameMode != GameMode.Spectator && bbox.Intersects(player.GetBoundingBox() + 1))
				{
					if (player.PickUpItem(this))
					{
						{
							var takeItemEntity = McpeTakeItemEntity.CreateObject();
							takeItemEntity.runtimeEntityId = EntityId;
							takeItemEntity.target = player.EntityId;
							Level.RelayBroadcast(player, takeItemEntity);
						}
						{
							var takeItemEntity = McpeTakeItemEntity.CreateObject();
							takeItemEntity.runtimeEntityId = EntityId;
							takeItemEntity.target = EntityManager.EntityIdSelf;
							player.SendPacket(takeItemEntity);
						}

						DespawnEntity();

						if (Item.Count > 0)
						{
							Level.DropItem(KnownPosition, Item);
						}

						break;
					}
				}
			}
		}

		private Vector3 GetAdjustedLengthFromCollision(Vector3 velocity)
		{
			var length = Length / 2;
			var direction = Vector3.Normalize(Velocity * 1.00000101f);
			var position = KnownPosition.ToVector3();
			int count = (int) (Math.Ceiling(Velocity.Length() / length) + 2);
			for (int i = 0; i < count; i++)
			{
				var distVec = direction * (float) length * i;
				BlockCoordinates blockPos = position + distVec;
				Block block = Level.GetBlock(blockPos);
				if (block.IsSolid)
				{
					Ray ray = new Ray(position, direction);
					var distance = ray.Intersects(block.GetBoundingBox());
					if (distance.HasValue)
					{
						float dist = (float) ((float) distance - (Length / 4));
						return ray.Direction * new Vector3(dist);
					}
				}
			}

			return velocity;
		}

		private void AdjustForCollision()
		{
			var length = Length / 2;
			var direction = Vector3.Normalize(Velocity * 1.00000101f);
			var position = KnownPosition.ToVector3();
			int count = (int) (Math.Ceiling(Velocity.Length() / length) + 2);
			for (int i = 0; i < count; i++)
			{
				var distVec = direction * (float) length * i;
				BlockCoordinates blockPos = position + distVec;
				Block block = Level.GetBlock(blockPos);
				if (block.IsSolid)
				{
					var yaw = (Math.Atan2(direction.X, direction.Z) * 180.0D / Math.PI) + 180;
					//Log.Warn($"Will hit block {block} at angle of {yaw}");

					Ray ray = new Ray(position, direction);
					if (ray.Intersects(block.GetBoundingBox()).HasValue)
					{
						int face = IntersectSides(block.GetBoundingBox(), ray);

						//Log.Warn($"Hit block {block} at angle of {yaw} on face {face}");
						if (face == -1) continue;
						switch (face)
						{
							case 0:
								Velocity *= new Vector3(1, 1, 0);
								break;
							case 1:
								Velocity *= new Vector3(0, 1, 1);
								break;
							case 2:
								Velocity *= new Vector3(1, 1, 0);
								break;
							case 3:
								Velocity *= new Vector3(0, 1, 1);
								break;
							case 4: // Under
								Velocity *= new Vector3(1, 0, 1);
								break;
							//case 5:
							//	float ff = 0.6f * 0.98f;
							//	Velocity *= new Vector3(ff, 0.0f, ff);
							//	break;
						}
						return;
					}
					else
					{
						//Log.Warn($"Hit block {block} at angle of {yaw} had no intersection (strange)");
						Velocity *= new Vector3(0, 0, 0);
					}
				}
			}
		}
	}
}