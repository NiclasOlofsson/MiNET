using System;
using MiNET.Blocks;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Entities
{
	public class Mob : Entity
	{
		public Mob(int entityTypeId, Level level) : base(entityTypeId, level)
		{
			Width = Length = 0.6;
			Height = 1.80;
		}

		public Mob(MobTypes mobTypes, Level level) : this((int) mobTypes, level)
		{
		}

		public override void OnTick()
		{
			base.OnTick();

			if (Velocity.Distance > 0)
			{
				PlayerLocation oldPosition = (PlayerLocation) KnownPosition.Clone();
				bool onGroundBefore = IsOnGround(KnownPosition);

				KnownPosition.X += (float) Velocity.X;
				KnownPosition.Y += (float) Velocity.Y;
				KnownPosition.Z += (float) Velocity.Z;

				bool onGround = IsOnGround(KnownPosition);
				if (!onGroundBefore && onGround)
				{
					KnownPosition.Y = (float) Math.Floor(oldPosition.Y);
					Velocity = Vector3.Zero;
				}
				else
				{
					Velocity *= (1.0 - Drag);
					if (!onGround)
					{
						Velocity -= new Vector3(0, Gravity, 0);
					}
				}
				LastUpdatedTime = DateTime.UtcNow;
			}
			else if (Velocity != Vector3.Zero)
			{
				Velocity = Vector3.Zero;
				LastUpdatedTime = DateTime.UtcNow;
			}
		}

		private bool IsOnGround(PlayerLocation position)
		{
			PlayerLocation pos = (PlayerLocation) position.Clone();
			pos.Y -= 0.1f;
			Block block = Level.GetBlock(new BlockCoordinates(pos));

			return block.Id != 0; // Should probably test for solid
		}
	}

	public class Chicken : Mob
	{
		public Chicken(Level level) : base(MobTypes.Chicken, level)
		{
			Width = Length = 0.4;
			Height = 0.7;
		}
	}

	public class Cow : Mob
	{
		public Cow(Level level) : base(MobTypes.Cow, level)
		{
			Width = Length = 0.9;
			Height = 1.4;
		}
	}

	public class Pig : Mob
	{
		public Pig(Level level) : base(MobTypes.Pig, level)
		{
			Width = Length = 0.9;
			Height = 0.9;
		}
	}

	public class Sheep : Mob
	{
		public Sheep(Level level) : base(MobTypes.Sheep, level)
		{
			Width = Length = 0.9;
			Height = 1.3;
		}
	}

	public class Wolf : Mob
	{
		public Wolf(Level level) : base(MobTypes.Wolf, level)
		{
			Width = Length = 0.6;
			Height = 0.8;
		}
	}

	public class Villager : Mob
	{
		public Villager(Level level) : base(MobTypes.Villager, level)
		{
			Width = Length = 0.6;
			Height = 1.8;
		}
	}

	public class MushroomCow : Mob
	{
		public MushroomCow(Level level) : base(MobTypes.MushroomCow, level)
		{
			Width = Length = 0.9;
			Height = 1.4;
		}
	}

	public class Squid : Mob
	{
		public Squid(Level level) : base(MobTypes.Squid, level)
		{
			Width = Length = 0.95;
			Height = 0.95;
		}
	}

	public class Rabbit : Mob
	{
		public Rabbit(Level level) : base(MobTypes.Rabbit, level)
		{
			Width = Length = 0.6;
			Height = 0.7;
		}
	}

	public class Bat : Mob
	{
		public Bat(Level level) : base(MobTypes.Bat, level)
		{
			Width = Length = 0.5;
			Height = 0.9;
		}
	}

	public class IronGolem : Mob
	{
		public IronGolem(Level level) : base(MobTypes.IronGolem, level)
		{
			Width = Length = 1.4;
			Height = 2.9;
		}
	}

	public class Snowman : Mob
	{
		public Snowman(Level level) : base(MobTypes.Snowman, level)
		{
			Width = Length = 0.7;
			Height = 1.9;
		}
	}

	public class Ocelot : Mob
	{
		public Ocelot(Level level) : base(MobTypes.Ocelot, level)
		{
			Width = Length = 0.6;
			Height = 0.8;
		}
	}

	public class Zombie : Mob
	{
		public Zombie(Level level) : base(MobTypes.Zombie, level)
		{
			Width = Length = 0.6;
			Height = 1.8;
		}
	}

	public class Creeper : Mob
	{
		public Creeper(Level level) : base(MobTypes.Creeper, level)
		{
			Width = Length = 0.6;
			Height = 1.8;
		}
	}

	public class Skeleton : Mob
	{
		public Skeleton(Level level) : base(MobTypes.Skeleton, level)
		{
			Width = Length = 0.6;
			Height = 1.95;
		}
	}

	public class Spider : Mob
	{
		public Spider(Level level) : base(MobTypes.Spider, level)
		{
			Width = Length = 1.4;
			Height = 0.9;
		}
	}

	public class ZombiePigman : Mob
	{
		public ZombiePigman(Level level) : base(MobTypes.ZombiePigman, level)
		{
			Width = Length = 0.6;
			Height = 1.8;
		}
	}

	public class Slime : Mob
	{
		private int _size = 1;

		public int Size
		{
			get { return _size; }
			set
			{
				_size = value;
				Width = Height = Length = _size*0.51000005;
			}
		}

		public Slime(Level level, int size = 1) : base(MobTypes.Slime, level)
		{
			Size = size;
		}

		public override MetadataDictionary GetMetadata()
		{
			var metadata = base.GetMetadata();
			metadata[16] = new MetadataByte((byte) Size);

			return metadata;
		}
	}

	public class Enderman : Mob
	{
		public Enderman(Level level) : base(MobTypes.Enderman, level)
		{
			Width = Length = 0.6;
			Height = 2.9;
		}
	}

	public class Silverfish : Mob
	{
		public Silverfish(Level level) : base(MobTypes.Silverfish, level)
		{
			Width = Length = 0.4;
			Height = 0.3;
		}
	}

	public class CaveSpider : Mob
	{
		public CaveSpider(Level level) : base(MobTypes.CaveSpider, level)
		{
			Width = Length = 0.7;
			Height = 0.5;
		}
	}

	public class Ghast : Mob
	{
		public Ghast(Level level) : base(MobTypes.Ghast, level)
		{
			Width = Length = 4.0;
			Height = 4.0;
		}
	}

	public class MagmaCube : Mob
	{
		public MagmaCube(Level level) : base(MobTypes.MagmaCube, level)
		{
			Width = Length = 0.51000005;
			Height = 0.51000005;
		}
	}

	public class Blaze : Mob
	{
		public Blaze(Level level) : base(MobTypes.Blaze, level)
		{
			Width = Length = 0.6;
			Height = 1.8;
		}
	}

	public class ZombieVillager : Mob
	{
		public ZombieVillager(Level level) : base(MobTypes.ZombieVillager, level)
		{
			Width = Length = 0.6;
			Height = 1.8;
		}
	}

	public class Witch : Mob
	{
		public Witch(Level level) : base(MobTypes.Witch, level)
		{
			Width = Length = 0.6;
			Height = 1.8;
		}
	}

	public class Herobrine : Mob
	{
		public Herobrine(Level level) : base(666, level)
		{
			Width = Length = 0.6;
			Height = 1.8;
		}
	}
}