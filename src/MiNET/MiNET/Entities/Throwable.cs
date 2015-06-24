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
	public class Throwable : Entity
	{
		public Player Shooter { get; set; }
		public int Ttl { get; set; }
		public bool DespawnOnImpact { get; set; }

		protected Throwable(Player shooter, int entityTypeId, Level level) : base(entityTypeId, level)
		{
			Shooter = shooter;
			Ttl = 0;
			DespawnOnImpact = true;
			BroadcastMovement = false;
		}

		public override MetadataDictionary GetMetadata()
		{
			var metadata = base.GetMetadata();

			if (Shooter != null)
			{
				metadata[17] = new MetadataLong(Shooter.EntityId);
			}

			return metadata;
		}

		public override void OnTick()
		{
			//base.OnTick();

			if (KnownPosition.Y <= 0
			    || (Velocity.Distance <= 0 && DespawnOnImpact)
			    || (Velocity.Distance <= 0 && !DespawnOnImpact && Ttl <= 0))
			{
				DespawnEntity();
				return;
			}

			Ttl--;

			if (KnownPosition.Y <= 0 || Velocity.Distance <= 0) return;

			var bbox = GetBoundingBox();

			Player playerHitted = CheckEntityCollide(bbox);

			bool collided = false;
			if (playerHitted != null)
			{
				playerHitted.HealthManager.TakeHit(this, 1, DamageCause.Projectile);
				collided = true;
			}
			else
			{
				//collided = CheckBlockCollide(KnownPosition);
				if (!collided)
				{
					var velocity2 = Velocity;
					velocity2 *= (1.0d - Drag);
					velocity2 -= new Vector3(0, Gravity, 0);
					double distance = velocity2.Distance;
					velocity2 = velocity2.Normalize()/2;

					for (int i = 0; i < Math.Ceiling(distance)*2; i++)
					{
						PlayerLocation nextPos = (PlayerLocation) KnownPosition.Clone();
						nextPos.X += (float) velocity2.X*i;
						nextPos.Y += (float) velocity2.Y*i;
						nextPos.Z += (float) velocity2.Z*i;

						BlockCoordinates coord = new BlockCoordinates(nextPos);
						Block block = Level.GetBlock(coord);
						collided = block.Id != 0 && (block.GetBoundingBox()).Contains(nextPos.ToVector3());
						if (collided)
						{
							var substBlock = new Block(57) {Coordinates = block.Coordinates};
							//Level.SetBlock(substBlock);
							//KnownPosition = nextPos;
							SetIntersectLocation(block.GetBoundingBox(), KnownPosition);
							break;
						}
					}
				}
			}

			if (collided)
			{
				Velocity = Vector3.Zero;
			}
			else
			{
				KnownPosition.X += (float)Velocity.X;
				KnownPosition.Y += (float)Velocity.Y;
				KnownPosition.Z += (float)Velocity.Z;

				Velocity *= (1.0 - Drag);
				Velocity -= new Vector3(0, Gravity, 0);

				var k = Math.Sqrt((Velocity.X*Velocity.X) + (Velocity.Z*Velocity.Z));
				KnownPosition.Yaw = (float) (Math.Atan2(Velocity.X, Velocity.Z)*180f/Math.PI);
				KnownPosition.Pitch = (float) (Math.Atan2(Velocity.Y, k)*180f/Math.PI);
			}

			// For debugging of flight-path
			BroadcastMoveAndMotion();
		}

		private Player CheckEntityCollide(BoundingBox bbox)
		{
			Player[] players = Level.GetSpawnedPlayers();
			foreach (var player in players)
			{
				if (player == Shooter) continue;

				if ((player.GetBoundingBox() + 0.3).Intersects(bbox))
				{
					return player;
				}
			}

			return null;
		}

		private bool CheckBlockCollide(PlayerLocation location)
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

						BoundingBox blockbox = block.GetBoundingBox() + 0.3;
						if (blockbox.Intersects(GetBoundingBox()))
						{
							//if (!blockbox.Contains(KnownPosition.ToVector3())) continue;

							if (block is FlowingLava || block is StationaryLava)
							{
								HealthManager.Ignite(1200);
								continue;
							}

							if (!block.IsSolid) continue;

							blockbox = block.GetBoundingBox();

							var midPoint = blockbox.Min + 0.5;
							blocks.Add((pos - Velocity).DistanceTo(midPoint), block);
						}
					}
				}
			}

			if (blocks.Count == 0) return false;

			var firstBlock = blocks.OrderBy(pair => pair.Key).First().Value;

			BoundingBox boundingBox = firstBlock.GetBoundingBox();
			if (!SetIntersectLocation(boundingBox, KnownPosition))
			{
				// No real hit
				return false;
			}

			// Use to debug hits, makes visual impressions (can be used for paintball too)
			var substBlock = new Stone {Coordinates = firstBlock.Coordinates};
			Level.SetBlock(substBlock);
			// End debug block

			Velocity = Vector3.Zero;
			return true;
		}

		public bool SetIntersectLocation(BoundingBox bbox, PlayerLocation location)
		{
			Ray ray = new Ray(location.ToVector3() - Velocity, Velocity.Normalize());
			double? distance = ray.Intersects(bbox);
			if (distance != null)
			{
				double dist = (double) distance - 0.1;
				Vector3 pos = ray.Position + (ray.Direction*dist);
				KnownPosition.X = (float) pos.X;
				KnownPosition.Y = (float) pos.Y;
				KnownPosition.Z = (float) pos.Z;
				return true;
			}

			return false;
		}

		/// <summary>
		/// For debugging of flight-path and rotation.
		/// </summary>
		private void BroadcastMoveAndMotion()
		{
			if (BroadcastMovement)
			{
				McpeSetEntityMotion motions = McpeSetEntityMotion.CreateObject();
				motions.entities = new EntityMotions {{EntityId, Velocity}};
				new Task(() => Level.RelayBroadcast(motions)).Start();

				McpeMoveEntity moveEntity = McpeMoveEntity.CreateObject();
				moveEntity.entities = new EntityLocations {{EntityId, KnownPosition}};
				new Task(() => Level.RelayBroadcast(moveEntity)).Start();
			}
		}

		public bool BroadcastMovement { get; set; }
	}
}