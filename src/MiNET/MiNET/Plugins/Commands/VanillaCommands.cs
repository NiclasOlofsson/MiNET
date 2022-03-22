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
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading;
using log4net;
using MiNET.Entities;
using MiNET.Entities.Hostile;
using MiNET.Entities.Passive;
using MiNET.Entities.Vehicles;
using MiNET.Items;
using MiNET.Net;
using MiNET.Plugins.Attributes;
using MiNET.Utils;
using MiNET.Utils.Vectors;
using MiNET.Worlds;

namespace MiNET.Plugins.Commands
{
	public class VanillaCommands
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(VanillaCommands));

		public VanillaCommands()
		{
		}

		[Command(Name = "op", Description = "Make player an operator")]
		[Authorize(Permission = 4)]
		public string MakeOperator(Player commander, Target target)
		{
			string body = target.Selector;

			if (target.Players != null)
			{
				List<string> names = new List<string>();
				foreach (var p in target.Players)
				{
					names.Add(p.Username);
					p.ActionPermissions = ActionPermissions.Operator;
					p.CommandPermission = 4;
					p.PermissionLevel = PermissionLevel.Operator;
					p.SendAdventureSettings();
				}
				body = string.Join(", ", names);
			}
			else if (target.Entities != null)
			{
				List<string> names = new List<string>();
				foreach (var p in target.Entities)
				{
					names.Add(p.NameTag ?? p.EntityId + "");
				}
				body = string.Join(", ", names);
			}

			return $"Oped: {body}";
		}

		[Command]
		public void Worldbuilder(Player commander)
		{
			commander.IsWorldBuilder = !commander.IsWorldBuilder;
			commander.SendAdventureSettings();
		}

		[Command]
		public string SetBlock(Player commander, BlockPos position, BlockTypeEnum tileName, int tileData = 0)
		{
			return $"Set block complete. {position.XRelative} {tileName.Value}";
		}

		[Command]
		public string Give(Player commander, Target player, ItemTypeEnum itemName, int amount = 1, int data = 0)
		{
			string body = player.Selector;

			if (player.Players != null)
			{
				List<string> names = new List<string>();
				foreach (var p in player.Players)
				{
					names.Add(p.Username);

					Item item = ItemFactory.GetItem(ItemFactory.GetItemIdByName(itemName.Value), (short) data, (byte) amount);

					if (item.Count > item.MaxStackSize) return $"The number you have entered ({amount}) is too big. It must be at most {item.MaxStackSize}";

					p.Inventory.SetFirstEmptySlot(item, true);
				}
				body = string.Join(", ", names);
			}


			return $"Gave {body} {amount} of {itemName.Value}.";
		}

		[Command]
		public void Summon(Player player, EntityTypeEnum entityType, bool noAi = true, BlockPos spawnPos = null)
		{
			EntityType petType;
			try
			{
				petType = (EntityType) Enum.Parse(typeof(EntityType), entityType.Value, true);
			}
			catch (ArgumentException)
			{
				return;
			}

			if (!Enum.IsDefined(typeof(EntityType), petType))
			{
				player.SendMessage("No entity found");
				return;
			}

			var coordinates = player.KnownPosition;
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
			Entity entity = null;

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
					var random = new Random();
					mob = new Horse(world, random.NextDouble() < 0.10, random);
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
					mob = new Wither(world);
					break;
				case EntityType.Evoker:
					mob = new Evoker(world);
					break;
				case EntityType.Vindicator:
					mob = new Vindicator(world);
					break;
				case EntityType.Vex:
					mob = new Vex(world);
					break;
				case EntityType.Npc:
					if (Config.GetProperty("EnableEdu", false))
					{
						mob = new Npc(world);
					}
					else
					{
						mob = new PlayerMob("test", world);
					}
					break;
				case EntityType.Boat:
					entity = new Boat(world);
					break;
			}

			if (mob != null)
			{
				mob.NoAi = noAi;
				var direction = Vector3.Normalize(player.KnownPosition.GetHeadDirection()) * 1.5f;
				mob.KnownPosition = new PlayerLocation(coordinates.X + direction.X, coordinates.Y, coordinates.Z + direction.Z, coordinates.HeadYaw, coordinates.Yaw);
				mob.SpawnEntity();
			}
			else if (entity != null)
			{
				entity.NoAi = noAi;
				var direction = Vector3.Normalize(player.KnownPosition.GetHeadDirection()) * 1.5f;
				entity.KnownPosition = new PlayerLocation(coordinates.X + direction.X, coordinates.Y, coordinates.Z + direction.Z, coordinates.HeadYaw, coordinates.Yaw);
				entity.SpawnEntity();
			}
		}

		[Command]
		public string Xp(Player commander, int experience, Target player)
		{
			string body = player.Selector;

			if (player.Players != null)
			{
				List<string> names = new List<string>();
				foreach (var p in player.Players)
				{
					names.Add(p.Username);
					p.ExperienceManager.AddExperience(experience);
				}

				body = string.Join(", ", names);
			}

			return $"Gave {body} {experience} experience points.";
		}

		[Command]
		public string Difficulty(Player commander, Difficulty difficulty)
		{
			Level level = commander.Level;
			level.Difficulty = difficulty;
			foreach (var player in level.GetAllPlayers())
			{
				player.SendSetDificulty();
			}

			return $"{commander.Username} set difficulty to {difficulty}";
		}

		[Command(Name = "time set", Description = "Changes or queries the world's game time")]
		public string TimeSet(Player commander, int time = 5000)
		{
			Level level = commander.Level;
			level.WorldTime = time;

			McpeSetTime message = McpeSetTime.CreateObject();
			message.time = (int) level.WorldTime;
			level.RelayBroadcast(message);

			return $"{commander.Username} sets time to {time}";
		}

		public enum DayNight
		{
			Day = 1000,
			Night = 13000
		}

		[Command(Name = "time set")]
		public string TimeSet(Player commander, DayNight time)
		{
			Level level = commander.Level;
			level.WorldTime = (int) time;

			McpeSetTime message = McpeSetTime.CreateObject();
			message.time = (int) level.WorldTime;
			level.RelayBroadcast(message);

			return $"{commander.Username} sets time to {time}";
		}

		[Command(Name = "tp", Aliases = new[] {"teleport"}, Description = "Teleports self to given position.")]
		public string Teleport(Player commander, BlockPos destination, int yrot = 90, int xrot = 0)
		{
			var coordinates = commander.KnownPosition;
			if (destination != null)
			{
				if (destination.XRelative)
					coordinates.X += destination.X;
				else
					coordinates.X = destination.X;

				if (destination.YRelative)
					coordinates.Y += destination.Y;
				else
					coordinates.Y = destination.Y;

				if (destination.ZRelative)
					coordinates.Z += destination.Z;
				else
					coordinates.Z = destination.Z;
			}

			ThreadPool.QueueUserWorkItem(delegate
			{
				commander.Teleport(new PlayerLocation
				{
					X = coordinates.X,
					Y = coordinates.Y,
					Z = coordinates.Z,
					Yaw = yrot,
					Pitch = xrot,
					HeadYaw = yrot
				});
			}, null);

			return $"{commander.Username} teleported to coordinates {coordinates.X},{coordinates.Y},{coordinates.Z}.";
		}

		[Command(Name = "tp", Aliases = new[] {"teleport"}, Description = "Teleports player to given coordinates.")]
		public string Teleport(Player commander, Target victim, BlockPos destination, int yrot = 90, int xrot = 0)
		{
			string body = victim.Selector;

			if (victim.Players != null)
			{
				List<string> names = new List<string>();
				foreach (var p in victim.Players)
				{
					names.Add(p.Username);

					ThreadPool.QueueUserWorkItem(delegate
					{
						var coordinates = p.KnownPosition;
						if (destination != null)
						{
							if (destination.XRelative)
								coordinates.X += destination.X;
							else
								coordinates.X = destination.X;

							if (destination.YRelative)
								coordinates.Y += destination.Y;
							else
								coordinates.Y = destination.Y;

							if (destination.ZRelative)
								coordinates.Z += destination.Z;
							else
								coordinates.Z = destination.Z;
						}

						p.Teleport(new PlayerLocation
						{
							X = coordinates.X,
							Y = coordinates.Y,
							Z = coordinates.Z,
							Yaw = yrot,
							Pitch = xrot,
							HeadYaw = yrot
						});
					}, null);
				}
				body = string.Join(", ", names);
			}

			return $"{body} teleported to new coordinates.";
		}


		[Command(Name = "tp", Aliases = new[] {"teleport"}, Description = "Teleports player to other player.")]
		public string Teleport(Player commander, Target victim, Target target)
		{
			string body = victim.Selector;

			if (target.Players == null || target.Players.Length != 1) return "Found not target for teleport";

			Player targetPlayer = target.Players.First();

			if (victim.Players != null)
			{
				List<string> names = new List<string>();
				foreach (var p in victim.Players)
				{
					names.Add(p.Username);

					ThreadPool.QueueUserWorkItem(delegate
					{
						var coordinates = targetPlayer.KnownPosition;
						p.Teleport(new PlayerLocation
						{
							X = coordinates.X,
							Y = coordinates.Y,
							Z = coordinates.Z,
							Yaw = coordinates.Yaw,
							Pitch = coordinates.Pitch,
							HeadYaw = coordinates.HeadYaw
						});
					}, null);
				}
				body = string.Join(", ", names);
			}


			return $"Teleported {body} to {targetPlayer.Username}.";
		}

		[Command(Name = "tp", Aliases = new[] {"teleport"}, Description = "Teleports self to other player.")]
		public string Teleport(Player commander, Target target)
		{
			if (target.Players == null || target.Players.Length != 1) return "Found not target for teleport";

			Player targetPlayer = target.Players.First();

			var coordinates = targetPlayer.KnownPosition;

			ThreadPool.QueueUserWorkItem(delegate
			{
				commander.Teleport(new PlayerLocation
				{
					X = coordinates.X,
					Y = coordinates.Y,
					Z = coordinates.Z,
					Yaw = coordinates.Yaw,
					Pitch = coordinates.Pitch,
					HeadYaw = coordinates.HeadYaw
				});
			}, null);


			return $"Teleported to {targetPlayer.Username}.";
		}

		[Command]
		public void Enchant(Player commander, Target target, EnchantmentTypeEnum enchantmentTypeName, int level = 1)
		{
			Player targetPlayer = target.Players.First();
			Item item = targetPlayer.Inventory.GetItemInHand();
			if (item is ItemAir) return;

			EnchantingType enchanting;
			if (!Enum.TryParse(enchantmentTypeName.Value.Replace("_", ""), true, out enchanting)) return;

			var enchanings = item.GetEnchantings();
			enchanings.RemoveAll(ench => ench.Id == enchanting);
			enchanings.Add(new Enchanting
			{
				Id = enchanting,
				Level = (short) level
			});
			item.SetEnchantings(enchanings);
			targetPlayer.Inventory.SendSetSlot(targetPlayer.Inventory.InHandSlot);
		}

		[Command]
		public string GameMode(Player commander, GameMode gameMode, Target target = null)
		{
			Player targetPlayer = commander;
			if (target != null) targetPlayer = target.Players.First();

			switch (gameMode)
			{
				case Worlds.GameMode.Spectator:
					targetPlayer.SetSpectator(true);
					break;
				default:
					targetPlayer.SetGameMode(gameMode);
					break;
			}

			commander.Level.BroadcastMessage($"{targetPlayer.Username} changed to game mode {gameMode}.", type: MessageType.Raw);

			return $"Set {targetPlayer.Username} game mode to {gameMode}.";
		}

		[Command]
		public string GameRule(Player player, GameRulesEnum rule)
		{
			return $"{rule.ToString().ToLower()}={player.Level.GetGameRule(rule).ToString().ToLower()}.";
		}

		[Command]
		public string GameRule(Player player, GameRulesEnum rule, bool value)
		{
			player.Level.SetGameRule(rule, value);
			player.Level.BroadcastGameRules();
			return $"{player.Username} set {rule.ToString().ToLower()} to {value.ToString().ToLower()}.";
		}

		[Command]
		public string Daylock(Player player, bool value)
		{
			Level level = player.Level;
			level.SetGameRule(GameRulesEnum.DoDaylightcycle, !value);
			level.BroadcastGameRules();

			level.WorldTime = 5000;

			McpeSetTime message = McpeSetTime.CreateObject();
			message.time = (int) level.WorldTime;
			level.RelayBroadcast(message);

			return $"{player.Username} set day to 5000 and locked time.";
		}

		[Command]
		public void Fill(Player commander, BlockPos from, BlockPos to, BlockTypeEnum tileName, int tileData = 0)
		{
		}
	}
}