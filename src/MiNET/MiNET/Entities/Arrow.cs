using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MiNET.Blocks;
using MiNET.Net;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Entities
{
	public class Arrow : Entity
	{
		public Player Player { get; set; }

		public Arrow(Player player, Level level) : base(80, level)
		{
			Player = player;
			Width = 0.5;
			Length = 0.5;
			Height = 0.0;

			Gravity = 0.05;
			Drag = 0.01;

			HealthManager.IsInvulnerable = true;
		}

		public override void OnTick()
		{
			base.OnTick();

			if (Velocity.Distance <= 0)
			{
				//var dropCoords = coords + BlockCoordinates.Up;
				//Level.DropItem(dropCoords, new ItemStack(262, 1));
				//DespawnEntity();
				return;
			}

			Velocity *= (1.0 - Drag);
			Velocity -= new Vector3(0, Gravity, 0);

			KnownPosition.X += (float) Velocity.X;
			KnownPosition.Y += (float) Velocity.Y;
			KnownPosition.Z += (float) Velocity.Z;

			var k = Math.Sqrt((Velocity.X*Velocity.X) + (Velocity.Z*Velocity.Z));
			KnownPosition.Yaw = (float) (Math.Atan2(Velocity.X, Velocity.Z)*180f/Math.PI);
			KnownPosition.Pitch = (float) (Math.Atan2(Velocity.Y, k)*180f/Math.PI);

			var bbox = GetBoundingBox();

			Player[] players = Level.GetSpawnedPlayers();
			foreach (var player in players)
			{
				if (player.GetBoundingBox().Intersects(bbox))
				{
					player.HealthManager.TakeHit(this, 1, DamageCause.Projectile);
					player.BroadcastEntityEvent();
					Velocity = new Vector3();
					break;
				}
			}

			if (CheckBlockObstructions(KnownPosition))
			{
				Velocity = new Vector3();
			}

			BroadcastMoveAndMotion();
		}

		private bool CheckBlockObstructions(PlayerLocation location)
		{
			var bbox = GetBoundingBox();
			var pos = location.ToVector3();

			var coords = new BlockCoordinates(
				(int) Math.Floor(KnownPosition.X),
				(int) Math.Floor((bbox.Max.Y + bbox.Min.Y)/2.0),
				(int) Math.Floor(KnownPosition.Z));

			Dictionary<double, Block> blocks = new Dictionary<double, Block>();

			for (int x = -1; x < 2; x++)
			{
				for (int z = -1; z < 2; z++)
				{
					for (int y = -1; y < 2; y++)
					{
						Block block = Level.GetBlock(coords.X + x, coords.Y + y, coords.Z + z);
						if (block is Air) continue;

						BoundingBox blockbox = block.GetBoundingBox();
						if (blockbox.Intersects(GetBoundingBox()))
						{
							if (block is FlowingLava || block is StationaryLava)
							{
								HealthManager.Ignite(1200);
								continue;
							}

							if (!block.IsSolid) continue;

							var midPoint = blockbox.Min + 0.5;
							blocks.Add((pos - Velocity).DistanceTo(midPoint), block);
						}
					}
				}
			}

			if (blocks.Count == 0) return false;

			var firstBlock = blocks.OrderBy(pair => pair.Key).First().Value;
			//var substBlock = new Stone {Coordinates = firstBlock.Coordinates};
			//Level.SetBlock(substBlock);

			BoundingBox boundingBox = firstBlock.GetBoundingBox();
			SetIntersectLocation(boundingBox, KnownPosition);

			return true;
		}

		public void SetIntersectLocation(BoundingBox bbox, PlayerLocation location)
		{
			Ray ray = new Ray(location.ToVector3() - Velocity, Velocity.Normalize());
			double? distance = ray.Intersects(bbox);
			if (distance != null)
			{
				double dist = (double) distance - 0.2;
				Vector3 pos = ray.Position + (ray.Direction*dist);
				KnownPosition.X = (float) pos.X;
				KnownPosition.Y = (float) pos.Y;
				KnownPosition.Z = (float) pos.Z;
			}
		}

		private void BroadcastMoveAndMotion()
		{
			McpeSetEntityMotion motions = McpeSetEntityMotion.CreateObject();
			motions.entities = new EntityMotions {{EntityId, Velocity}};
			new Task(() => Level.RelayBroadcast(motions)).Start();

			McpeMoveEntity moveEntity = McpeMoveEntity.CreateObject();
			moveEntity.entities = new EntityLocations {{EntityId, KnownPosition}};
			new Task(() => Level.RelayBroadcast(moveEntity)).Start();
		}
	}
}