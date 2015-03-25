using System;
using System.Threading.Tasks;
using MiNET.Net;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Entities
{
	public class Arrow : Entity
	{
		public Arrow(Level level) : base(80, level)
		{
			Width = 0.5;
			Length = 0.5;
			Height = 0.5;

			Gravity = 0.05;
			Drag = 0.01;
		}

		public override void OnTick()
		{
			if (KnownPosition.Y <= 0)
			{
				DespawnEntity();
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
					DespawnEntity();
					break;
				}
			}

			BroadcastMotion();
		}

		private void BroadcastMotion()
		{
			var motions = McpeSetEntityMotion.CreateObject();
			motions.entities = new EntityMotions();
			motions.entities.Add(EntityId, Velocity);
			new Task(() => Level.RelayBroadcast(motions)).Start();

			var moveEntity = McpeMoveEntity.CreateObject();
			moveEntity.entities = new EntityLocations();
			moveEntity.entities.Add(EntityId, KnownPosition);
			moveEntity.Encode();

			new Task(() => Level.RelayBroadcast(moveEntity)).Start();
		}
	}
}