using System;
using System.Numerics;
using log4net;
using MiNET.Entities.Behaviors;
using MiNET.Items;
using MiNET.Particles;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Entities.Passive
{
	public class Wolf : PassiveMob
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (Wolf));

		public byte CollarColor { get; set; }
		public Entity Owner { get; set; }

		public Wolf(Level level) : base(EntityType.Wolf, level)
		{
			Width = Length = 0.6;
			Height = 0.8;
			IsAngry = false;
			CollarColor = 14;
			HealthManager.MaxHealth = 80;
			HealthManager.ResetHealth();
			Speed = 0.3;

			Behaviors.Add(new SittingBehavior(this));
			Behaviors.Add(new JumpAttackBehavior(this, 1.0));
			Behaviors.Add(new MeeleAttackBehavior(this, 1.0));
			Behaviors.Add(new OwnerHurtByTargetBehavior(this));
			Behaviors.Add(new OwnerHurtTargetBehavior(this));
			Behaviors.Add(new HurtByTargetBehavior(this));
			Behaviors.Add(new FollowOwnerBehavior(this, 20, 1.0));
			Behaviors.Add(new StrollBehavior(this, 60, Speed, 0.7));
			Behaviors.Add(new LookAtPlayerBehavior(this, 8.0));
			Behaviors.Add(new RandomLookaroundBehavior(this));
		}

		public override void DoInteraction(byte actionId, Player player)
		{
			if (IsTamed)
			{
				if (Owner == player)
				{
					IsSitting = !IsSitting;
					BroadcastSetEntityData();
				}
				else
				{
					// Hmm?
				}
			}
			else
			{
				if (player.Inventory.GetItemInHand() is ItemBone)
				{
					Log.Debug($"Wolf taming attempt by {player.Username}");

					player.Inventory.RemoveItems(new ItemBone().Id, 1);

					var random = new Random();
					if (random.Next(3) == 0)
					{
						Owner = player;
						IsTamed = true;
						IsSitting = true;
						IsAngry = false;
						BroadcastSetEntityData();

						for (int i = 0; i < 7; ++i)
						{
							Particle particle = new HeartParticle(Level, random.Next(3));
							particle.Position = KnownPosition + new Vector3(0, (float) (Height + 0.85d), 0);
							particle.Spawn();
						}


						Log.Debug($"Wolf is now tamed by {player.Username}");
					}
					else
					{
						for (int i = 0; i < 7; ++i)
						{
							Particle particle = new SmokeParticle(Level);
							particle.Position = KnownPosition + new Vector3(0, (float) (Height + 0.85d), 0);
							particle.Spawn();
						}
					}
				}
			}
		}

		public override MetadataDictionary GetMetadata()
		{
			MetadataDictionary metadata = base.GetMetadata();
			metadata[1] = new MetadataInt(12);
			metadata[2] = new MetadataInt(0);
			metadata[3] = new MetadataByte(CollarColor);
			//metadata[4] = new MetadataString("Testing");
			if (Owner != null)
			{
				metadata[5] = new MetadataLong(Owner.EntityId);
			}
			metadata[7] = new MetadataShort(300);
			metadata[8] = new MetadataInt(0);
			metadata[9] = new MetadataByte(0);
			metadata[38] = new MetadataLong(0);
			metadata[39] = new MetadataFloat(1.0f);
			metadata[44] = new MetadataShort(300);
			metadata[45] = new MetadataInt(0);
			metadata[46] = new MetadataByte(0);
			metadata[47] = new MetadataInt(0);
			metadata[53] = new MetadataFloat(0.6f);
			metadata[54] = new MetadataFloat(0.8f);
			metadata[56] = new MetadataVector3(0, 0, 0);
			metadata[57] = new MetadataByte(0);
			metadata[58] = new MetadataFloat(0f);
			metadata[59] = new MetadataFloat(0f);

			return metadata;
		}
	}
}