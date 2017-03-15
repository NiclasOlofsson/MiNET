using System;
using System.Collections.Generic;
using MiNET.Entities;
using MiNET.Entities.Hostile;
using MiNET.Entities.Passive;
using MiNET.Items;
using MiNET.Plugins.Attributes;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Plugins.Commands
{
	public class VanillaCommands
	{
		private readonly PluginManager _pluginManager;

		public VanillaCommands(PluginManager pluginManager)
		{
			_pluginManager = pluginManager;
		}

		public class SimpleResponse
		{
			public string Body { get; set; }
		}

		[Command(Name = "op")]
		public SimpleResponse MakeOperator(Player commander, Target player)
		{
			string body = player.Selector;

			if (player.Players != null)
			{
				List<string> names = new List<string>();
				foreach (var p in player.Players)
				{
					names.Add(p.Username);
				}
				body = string.Join(", ", names);
			}
			else if (player.Entities != null)
			{
				List<string> names = new List<string>();
				foreach (var p in player.Entities)
				{
					names.Add(p.NameTag ?? p.EntityId + "");
				}
				body = string.Join(", ", names);
			}

			return new SimpleResponse() {Body = $"Oped: {body}"};
		}

		[Command]
		public SimpleResponse SetBlock(Player commander, BlockPos position, BlockTypeEnum tileName, int tileData = 0)
		{
			return new SimpleResponse {Body = $"Set block complete. {position.XRelative} {tileName.Value}"};
		}

		[Command]
		public SimpleResponse Give(Player commander, Target player, ItemTypeEnum itemName, int amount = 1, int data = 0)
		{
			string body = player.Selector;

			if (player.Players != null)
			{
				List<string> names = new List<string>();
				foreach (var p in player.Players)
				{
					names.Add(p.Username);

					Item item = ItemFactory.GetItem(ItemFactory.GetItemIdByName(itemName.Value), (short) data, (byte) amount);

					var inventory = p.Inventory.SetFirstEmptySlot(item, true, false);
				}
				body = string.Join(", ", names);
			}


			return new SimpleResponse {Body = $"Gave {body} {amount} of {itemName.Value}."};
		}

		[Command]
		public void Summon(Player player, EntityTypeEnum entityType, BlockPos spawnPos = null)
		{
			EntityType petType;
			try
			{
				petType = (EntityType) Enum.Parse(typeof (EntityType), entityType.Value, true);
			}
			catch (ArgumentException e)
			{
				return;
			}

			if (!Enum.IsDefined(typeof (EntityType), petType))
			{
				player.SendMessage("No entity found");
				return;
			}

			var coordinates = player.KnownPosition.GetCoordinates3D();
			if (spawnPos != null)
			{
				if (spawnPos.XRelative)
					coordinates.X += spawnPos.X;
				else
					coordinates.X = spawnPos.X;

				if (spawnPos.YRelative)
					coordinates.Y += spawnPos.Y;
				else
					coordinates.Y = spawnPos.Y;

				if (spawnPos.ZRelative)
					coordinates.Z += spawnPos.Z;
				else
					coordinates.Z = spawnPos.Z;
			}

			var world = player.Level;

			Mob mob = null;

			EntityType type = (EntityType) (int) petType;
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
					mob = new Wolf(world) {Owner = player};
					break;
				case EntityType.Villager:
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
				case EntityType.Stray:
					mob = new Stray(world);
					break;
				case EntityType.Husk:
					mob = new Husk(world);
					break;
				case EntityType.WitherSkeleton:
					mob = new WitherSkeleton(world);
					break;
				case EntityType.Guardian:
					mob = new Guardian(world);
					break;
				case EntityType.ElderGuardian:
					mob = new ElderGuardian(world);
					break;
				case EntityType.Horse:
					mob = new Horse(world);
					break;
				case EntityType.PolarBear:
					mob = new PolarBear(world);
					break;
				case EntityType.Shulker:
					mob = new Shulker(world);
					break;
				case EntityType.Dragon:
					mob = new Dragon(world);
					break;
				case EntityType.SkeletonHorse:
					mob = new SkeletonHorse(world);
					break;
				case EntityType.Wither:
					mob = new Mob(EntityType.Wither, world);
					break;
				case EntityType.Npc:
					mob = new PlayerMob("test", world);
					break;
			}

			if (mob == null) return;

			mob.KnownPosition = new PlayerLocation(coordinates.X, coordinates.Y, coordinates.Z);
			mob.SpawnEntity();
		}

		[Command]
		public SimpleResponse Xp(Player commander, int levels, Target player)
		{
			string body = player.Selector;

			if (player.Players != null)
			{
				List<string> names = new List<string>();
				foreach (var p in player.Players)
				{
					names.Add(p.Username);
					p.ExperienceLevel += levels;
					p.SendUpdateAttributes();
				}

				body = string.Join(", ", names);
			}

			return new SimpleResponse {Body = $"Gave {body} {levels} levels of XP"};
		}

		[Command]
		public SimpleResponse Difficulty(Player commander, Difficulty difficulty)
		{
			Level level = commander.Level;
			level.Difficulty = difficulty;
			foreach (var player in level.GetSpawnedPlayers())
			{
				player.SendSetDificulty();
			}

			return new SimpleResponse { Body = $"{commander.Username} set difficulty to {difficulty}" };
		}

	}
}