using log4net;
using MiNET.Entities;
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

			MobTypes type = (MobTypes) Metadata;
			switch (type)
			{
				case MobTypes.Chicken:
					mob = new Chicken(world);
					break;
				case MobTypes.Cow:
					mob = new Cow(world);
					break;
				case MobTypes.Pig:
					mob = new Pig(world);
					break;
				case MobTypes.Sheep:
					mob = new Sheep(world);
					break;
				case MobTypes.Wolf:
					mob = new Wolf(world);
					break;
				case MobTypes.Npc:
					mob = new Villager(world);
					break;
				case MobTypes.MushroomCow:
					mob = new MushroomCow(world);
					break;
				case MobTypes.Squid:
					mob = new Squid(world);
					break;
				case MobTypes.Rabbit:
					mob = new Rabbit(world);
					break;
				case MobTypes.Bat:
					mob = new Bat(world);
					break;
				case MobTypes.IronGolem:
					mob = new IronGolem(world);
					break;
				case MobTypes.Snowman:
					mob = new Snowman(world);
					break;
				case MobTypes.Ocelot:
					mob = new Ocelot(world);
					break;
				case MobTypes.Zombie:
					mob = new Zombie(world);
					break;
				case MobTypes.Creeper:
					mob = new Creeper(world);
					break;
				case MobTypes.Skeleton:
					mob = new Skeleton(world);
					break;
				case MobTypes.Spider:
					mob = new Spider(world);
					break;
				case MobTypes.ZombiePigman:
					mob = new ZombiePigman(world);
					break;
				case MobTypes.Slime:
					mob = new Slime(world);
					break;
				case MobTypes.Enderman:
					mob = new Enderman(world);
					break;
				case MobTypes.Silverfish:
					mob = new Silverfish(world);
					break;
				case MobTypes.CaveSpider:
					mob = new CaveSpider(world);
					break;
				case MobTypes.Ghast:
					mob = new Ghast(world);
					break;
				case MobTypes.MagmaCube:
					mob = new MagmaCube(world);
					break;
				case MobTypes.Blaze:
					mob = new Blaze(world);
					break;
				case MobTypes.ZombieVillager:
					mob = new ZombieVillager(world);
					break;
				case MobTypes.Witch:
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