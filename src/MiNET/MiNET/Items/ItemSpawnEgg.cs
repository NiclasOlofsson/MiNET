using log4net;
using MiNET.Entities;
using MiNET.Entities.Hostile;
using MiNET.Entities.Passive;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Items
{
	public class ItemSpawnEgg : Item
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (ItemSpawnEgg));

		public ItemSpawnEgg(short metadata) : base(383, metadata)
		{
			MaxStackSize = 1;
		}

		public override void UseItem(Level world, Player player, BlockCoordinates blockCoordinates, BlockFace face, Vector3 faceCoords)
		{
			var coordinates = GetNewCoordinatesFromFace(blockCoordinates, face);

			Mob mob = null;

			EntityType type = (EntityType) Metadata;
			switch (type)
			{
				case EntityType.Chicken:
					mob = new Chicken(world);
					break;
				case EntityType.Cow:
					mob = new Cow(world);
					break;
				case EntityType.Pig:
					mob = new Pig(world);
					break;
				case EntityType.Sheep:
					mob = new Sheep(world);
					break;
				case EntityType.Wolf:
					mob = new Wolf(world);
					break;
				case EntityType.Npc:
					mob = new Villager(world);
					break;
				case EntityType.MushroomCow:
					mob = new MushroomCow(world);
					break;
				case EntityType.Squid:
					mob = new Squid(world);
					break;
				case EntityType.Rabbit:
					mob = new Rabbit(world);
					break;
				case EntityType.Bat:
					mob = new Bat(world);
					break;
				case EntityType.IronGolem:
					mob = new IronGolem(world);
					break;
				case EntityType.SnowGolem:
					mob = new SnowGolem(world);
					break;
				case EntityType.Ocelot:
					mob = new Ocelot(world);
					break;
				case EntityType.Zombie:
					mob = new Zombie(world);
					break;
				case EntityType.Creeper:
					mob = new Creeper(world);
					break;
				case EntityType.Skeleton:
					mob = new Skeleton(world);
					break;
				case EntityType.Spider:
					mob = new Spider(world);
					break;
				case EntityType.ZombiePigman:
					mob = new ZombiePigman(world);
					break;
				case EntityType.Slime:
					mob = new Slime(world);
					break;
				case EntityType.Enderman:
					mob = new Enderman(world);
					break;
				case EntityType.Silverfish:
					mob = new Silverfish(world);
					break;
				case EntityType.CaveSpider:
					mob = new CaveSpider(world);
					break;
				case EntityType.Ghast:
					mob = new Ghast(world);
					break;
				case EntityType.MagmaCube:
					mob = new MagmaCube(world);
					break;
				case EntityType.Blaze:
					mob = new Blaze(world);
					break;
				case EntityType.ZombieVillager:
					mob = new ZombieVillager(world);
					break;
				case EntityType.Witch:
					mob = new Witch(world);
					break;
			}

			if (mob == null) return;

			mob.KnownPosition = new PlayerLocation(coordinates.X, coordinates.Y, coordinates.Z);
			mob.SpawnEntity();

			Log.WarnFormat("Player {0} spawned Mob #{1}.", player.Username, Metadata);
		}
	}
}