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
	public class Egg : Entity
	{
		public Player Player { get; set; }

		public Egg(Player player, Level level) : base(82, level)
		{
			Player = player;
			Width = 0.25;
			Length = 0.25;
			Height = 0.25;

			Gravity = 0.03;
			Drag = 0.01;
		}


		public override void OnTick()
		{
			if (KnownPosition.Y <= 0 || Velocity.Distance <= 0)
			{
				DespawnEntity();
				return;
			}

			Velocity *= (1.0 - Drag);
			Velocity -= new Vector3(0, Gravity, 0);

			KnownPosition.X += (float) Velocity.X;
			KnownPosition.Y += (float) Velocity.Y;
			KnownPosition.Z += (float) Velocity.Z;

			var bbox = GetBoundingBox();
			bbox.Min -= 0.3;
			bbox.Max += 0.3;

			Player playerHitted = CheckEntityCollide(bbox);

			bool collided;
			if (playerHitted != null)
			{
				playerHitted.HealthManager.TakeHit(this, 1, DamageCause.Projectile);
				playerHitted.BroadcastEntityEvent();
				collided = true;
			}
			else
			{
				collided = CheckBlockCollide(KnownPosition);
			}

			if (collided)
			{
				Velocity = Vector3.Zero;
			}

			BroadcastMoveAndMotion();
		}

		private Player CheckEntityCollide(BoundingBox bbox)
		{
			Player[] players = Level.GetSpawnedPlayers();
			foreach (var player in players)
			{
				if(player == Player) continue;

				if (player.GetBoundingBox().Intersects(bbox))
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

						BoundingBox blockbox = block.GetBoundingBox();
						if (blockbox.Intersects(GetBoundingBox()))
						{
							//if (!blockbox.Contains(KnownPosition.ToVector3())) continue;

							var midPoint = blockbox.Min + 0.5;
							blocks.Add((pos - Velocity).DistanceTo(midPoint), block);
						}
					}
				}
			}

			if (blocks.Count == 0) return false;

			var firstBlock = blocks.OrderBy(pair => pair.Key).First().Value;
			var substBlock = new Stone {Coordinates = firstBlock.Coordinates};
			Level.SetBlock(substBlock);

			Velocity = new Vector3();
			return true;
		}

		private void BroadcastMoveAndMotion()
		{
			McpeSetEntityMotion motions = McpeSetEntityMotion.CreateObject();
			motions.entities = new EntityMotions {{EntityId, Velocity}};
			//new Task(() => Level.RelayBroadcast(motions)).Start();

			McpeMoveEntity moveEntity = McpeMoveEntity.CreateObject();
			moveEntity.entities = new EntityLocations {{EntityId, KnownPosition}};
			new Task(() => Level.RelayBroadcast(moveEntity)).Start();
		}
	}
}