using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using log4net;
using MiNET;
using MiNET.BlockEntities;
using MiNET.Blocks;
using MiNET.Effects;
using MiNET.Items;
using MiNET.Net;
using MiNET.Plugins;
using MiNET.Plugins.Attributes;
using MiNET.Utils;
using MiNET.Worlds;

namespace TestPlugin
{
	[Plugin(PluginName = "CoreCommands", Description = "The core commands for MiNET", PluginVersion = "1.0", Author = "MiNET Team")]
	public class CoreCommands : Plugin
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (CoreCommands));

		private Dictionary<string, Level> _worlds = new Dictionary<string, Level>();

		protected override void OnEnable()
		{
			//Context.PluginManager.RegisterCommands(Version);
		}


		[Command]
		public void Version(Player player)
		{
			string productVersion = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion;
			player.SendMessage(string.Format("MiNET v{0}", productVersion), type: MessageType.Raw);
		}

		[Command]
		public void Plugins(Player player)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("Plugins: ");
			foreach (var plugin in Context.PluginManager.Plugins)
			{
				sb.AppendLine(plugin.GetType().Name);
			}

			player.SendMessage(sb.ToString(), type: MessageType.Raw);
		}

		[Command(Command = "items")]
		public void AddItems(Player player, byte itemId, int noItems)
		{
			for (int i = 0; i < noItems; i++)
			{
				player.Level.DropItem(new BlockCoordinates(player.KnownPosition) + 1, new ItemStack(itemId, 1));
			}
		}

		[Command(Command = "gm")]
		public void GameMode(Player player, int gameMode)
		{
			player.GameMode = (GameMode) gameMode;
			player.SendPackage(new McpeStartGame
			{
				seed = -1,
				generator = 1,
				gamemode = gameMode,
				entityId = player.EntityId,
				spawnX = (int) player.Level.SpawnPoint.X,
				spawnY = (int) player.Level.SpawnPoint.Y,
				spawnZ = (int) player.Level.SpawnPoint.Z,
				x = player.KnownPosition.X,
				y = player.KnownPosition.Y,
				z = player.KnownPosition.Z
			});

			player.Level.BroadcastMessage(string.Format("{0} changed to game mode {1}.", player.Username, gameMode), type: MessageType.Raw);
		}


		[Command(Command = "tp")]
		public void Teleport(Player player, int x, int y, int z)
		{
			// send teleport to spawn
			//player.SetPosition(
			//	new PlayerLocation
			//{
			//	X = x,
			//	Y = y,
			//	Z = z,
			//	Yaw = 91,
			//	Pitch = 28,
			//	HeadYaw = 91
			//}, true);

			ThreadPool.QueueUserWorkItem(delegate(object state)
			{
				player.SpawnLevel(player.Level, new PlayerLocation
				{
				X = x,
				Y = y,
				Z = z,
				Yaw = 91,
				Pitch = 28,
				HeadYaw = 91
				});
			}, null);

			//player.Level.BroadcastMessage(string.Format("{0} teleported to coordinates {1},{2},{3}.", player.Username, x, y, z), type: MessageType.Raw);
		}

		[Command(Command = "tp")]
		public void Teleport(Player player)
		{
			Teleport(player, "Default");
		}

		[Command(Command = "tp")]
		public void Teleport(Player player, string world)
		{
			if (player.Level.LevelId.Equals(world)) return;

			if (!Context.LevelManager.Levels.Contains(player.Level))
			{
				Context.LevelManager.Levels.Add(player.Level);
			}

			ThreadPool.QueueUserWorkItem(delegate(object state)
			{
				Level[] levels = state as Level[];

				if (levels != null)
				{
					Level nextLevel = levels.FirstOrDefault(l => l.LevelId != null && l.LevelId.Equals(world));

					if (nextLevel == null)
					{
						nextLevel = new Level(world, new FlatlandWorldProvider());
						nextLevel.Initialize();
						Context.LevelManager.Levels.Add(nextLevel);
					}

					player.Level.BroadcastMessage(string.Format("{0} teleported to world {1}.", player.Username, nextLevel.LevelId), type: MessageType.Raw);

					player.SpawnLevel(nextLevel);
				}
			}, Context.LevelManager.Levels.ToArray());
		}

		[Command]
		public void Clear(Player player)
		{
			for (byte slot = 0; slot < 35; slot++) player.Inventory.SetInventorySlot(slot, -1); //Empty all slots.
		}

		[Command]
		public void Clear(Player player, Player target)
		{
			Clear(target);
		}

		[Command(Command = "vd")]
		public void ViewDistance(Player player)
		{
			player.Level.BroadcastMessage(string.Format("Current view distance set to {0}.", player.Level.ViewDistance), type: MessageType.Raw);
		}

		[Command(Command = "vd")]
		public void ViewDistance(Player player, int viewDistance)
		{
			player.Level.ViewDistance = viewDistance;
			player.Level.BroadcastMessage(string.Format("View distance changed to {0}.", player.Level.ViewDistance), type: MessageType.Raw);
		}

		[Command(Command = "twitter")]
		public void Twitter(Player player)
		{
			player.Level.BroadcastMessage("§6Twitter @NiclasOlofsson", type: MessageType.Raw);
			player.Level.BroadcastMessage("§5twitch.tv/niclasolofsson", type: MessageType.Raw);
		}

		[Command(Command = "pi")]
		public void PlayerInfo(Player player)
		{
			player.SendMessage(string.Format("Username={0}", player.Username), type: MessageType.Raw);
			player.SendMessage(string.Format("Entity ID={0}", player.EntityId), type: MessageType.Raw);
			player.SendMessage(string.Format("Client GUID={0}", player.ClientGuid), type: MessageType.Raw);
			player.SendMessage(string.Format("Client ID={0}", player.ClientId), type: MessageType.Raw);
			player.SendMessage(string.Format("Client ID={0}", player.ClientUuid), type: MessageType.Raw);
		}

		[Command(Command = "pos")]
		public void Position(Player player)
		{
			BlockCoordinates position = new BlockCoordinates(player.KnownPosition);

			int chunkX = position.X >> 4;
			int chunkZ = position.Z >> 4;

			int xi = (chunkX%32);
			if (xi < 0) xi += 32;
			int zi = (chunkZ%32);
			if (zi < 0) zi += 32;

			StringBuilder sb = new StringBuilder();
			sb.AppendLine(string.Format("Region X:{0} Z:{1}", chunkX >> 5, chunkZ >> 5));
			sb.AppendLine(string.Format("Local chunk X:{0} Z:{1}", xi, zi));
			sb.AppendLine(string.Format("Chunk X:{0} Z:{1}", chunkX, chunkZ));
			sb.AppendLine(string.Format("Position X:{0:F1} Y:{1:F1} Z:{2:F1}", player.KnownPosition.X, player.KnownPosition.Y, player.KnownPosition.Z));
			sb.AppendLine(string.Format("Direction Yaw:{0:F1} HeadYap:{1:F1} Pitch:{2:F1}", player.KnownPosition.Yaw, player.KnownPosition.HeadYaw, player.KnownPosition.Pitch));
			string text = sb.ToString();

			player.SendMessage(text, type: MessageType.Raw);
			Log.Info(text);
		}

		//[Command]
		//[Authorize(Users = "gurun")]
		//public void Spawn(Player player, byte id)
		//{
		//	Level level = player.Level;

		//	Mob entity = new Mob(id, level)
		//	{
		//		KnownPosition = player.KnownPosition,
		//		//Data = -(blockId | 0 << 0x10)
		//	};
		//	entity.SpawnEntity();

		//	level.BroadcastTextMessage(string.Format("Player {0} spawned Mob #{1}.", player.Username, id), type: MessageType.Raw);
		//}

		[Command]
		public void Kit(Player player, int kitId)
		{
			var inventory = player.Inventory;

			switch (kitId)
			{
				case 0:
					// Kit leather tier
					break;
				case 1:
					// Kit gold tier
					break;
				case 2:
					// Kit chain tier
					break;
				case 3:
					// Kit iron tier
					inventory.Boots = new ItemIronBoots(0);
					inventory.Leggings = new ItemIronLeggings(0);
					inventory.Chest = new ItemIronChestplate(0);
					inventory.Helmet = new ItemIronHelmet(0);
					break;
				case 4:
					// Kit diamond tier
					inventory.Boots = new ItemDiamondBoots(0);
					inventory.Leggings = new ItemDiamondLeggings(0);
					inventory.Chest = new ItemDiamondChestplate(0);
					inventory.Helmet = new ItemDiamondHelmet(0);
					break;
			}

			byte c = 0;
			var command = new ItemCommand(41, 0, delegate(ItemCommand itemCommand, Level level, Player arg3, BlockCoordinates arg4) { Log.Info("Clicked on command"); });

			inventory.Slots[c++] = new ItemStack(command, 1); // Wooden Sword
			inventory.Slots[c++] = new ItemStack(268, 1); // Wooden Sword
			inventory.Slots[c++] = new ItemStack(283, 1); // Golden Sword
			inventory.Slots[c++] = new ItemStack(272, 1); // Stone Sword
			inventory.Slots[c++] = new ItemStack(267, 1); // Iron Sword
			inventory.Slots[c++] = new ItemStack(276, 1); // Diamond Sword

			inventory.Slots[c++] = new ItemStack(261, 1); // Bow
			inventory.Slots[c++] = new ItemStack(262, 64); // Arrows
			inventory.Slots[c++] = new ItemStack(344, 64); // Eggs
			inventory.Slots[c++] = new ItemStack(332, 64); // Snowballs

			player.SendPlayerInventory();
			SendEquipmentForPlayer(player);
			SendArmorForPlayer(player);

			player.Level.BroadcastMessage(string.Format("Player {0} changed kit.", player.Username), type: MessageType.Raw);
		}

		private void SendEquipmentForPlayer(Player player)
		{
			player.Level.RelayBroadcast(new McpePlayerEquipment
			{
				entityId = player.EntityId,
				item = new MetadataSlot(player.Inventory.GetItemInHand()),
				slot = 0
			});
		}

		private void SendArmorForPlayer(Player player)
		{
			var armorEquipment = new McpePlayerArmorEquipment();
			armorEquipment.entityId = player.EntityId;
			armorEquipment.helmet = new MetadataSlot(new ItemStack(player.Inventory.Helmet, 0));
			armorEquipment.chestplate = new MetadataSlot(new ItemStack(player.Inventory.Chest, 0));
			armorEquipment.leggings = new MetadataSlot(new ItemStack(player.Inventory.Leggings, 0));
			armorEquipment.boots = new MetadataSlot(new ItemStack(player.Inventory.Boots, 0));
			player.Level.RelayBroadcast(armorEquipment);
		}

		[Command]
		public void Fly(Player player)
		{
			player.SendPackage(new McpeAdventureSettings {flags = 0x80});
			player.Level.BroadcastMessage(string.Format("Player {0} enabled flying.", player.Username), type: MessageType.Raw);
		}

		[Command(Command = "e")]
		public void Effect(Player player, string effect, int level = 1, int duration = 20)
		{
			EffectType effectType;
			if (Enum.TryParse(effect, true, out effectType))
			{
				Effect eff = null;
				switch (effectType)
				{
					case EffectType.Speed:
						eff = new Speed();
						break;
					case EffectType.Slowness:
						eff = new Slowness();
						break;
					case EffectType.Haste:
						eff = new Haste();
						break;
					case EffectType.MiningFatigue:
						eff = new MiningFatigue();
						break;
					case EffectType.Strenght:
						eff = new Strength();
						break;
					case EffectType.InstandHealth:
						eff = new InstandHealth();
						break;
					case EffectType.InstantDamage:
						eff = new InstantDamage();
						break;
					case EffectType.JumpBoost:
						eff = new JumpBoost();
						break;
					case EffectType.Nausea:
						eff = new Nausea();
						break;
					case EffectType.Regeneration:
						eff = new Regeneration();
						break;
					case EffectType.Resistance:
						eff = new Resistance();
						break;
					case EffectType.FireResistance:
						eff = new FireResistance();
						break;
					case EffectType.WaterBreathing:
						eff = new WaterBreathing();
						break;
					case EffectType.Invisibility:
						eff = new Invisibility();
						break;
					case EffectType.Blindness:
						eff = new Blindness();
						break;
					case EffectType.NightVision:
						eff = new NightVision();
						break;
					case EffectType.Hunger:
						eff = new Hunger();
						break;
					case EffectType.Weakness:
						eff = new Weakness();
						break;
					case EffectType.Poison:
						eff = new Poison();
						break;
					case EffectType.Wither:
						eff = new Wither();
						break;
					case EffectType.HealthBoost:
						eff = new HealthBoost();
						break;
					case EffectType.Absorption:
						eff = new Absorption();
						break;
					case EffectType.Saturation:
						eff = new Saturation();
						break;
					case EffectType.Glowing:
						eff = new Glowing();
						break;
					case EffectType.Levitation:
						eff = new Levitation();
						break;
		}

				if (eff != null)
			{
					eff.Level = level;
					eff.Duration = duration;

					player.SetEffect(eff);
					player.Level.BroadcastMessage(string.Format("{0} added effect {1} with strenght {2}", player.Username, effectType, level), MessageType.Raw);
				}
			}
		}

		[Command(Command = "nd")]
		public void NoDamage(Player player)
		{
			player.HealthManager = player.HealthManager is NoDamageHealthManager ? new HealthManager(player) : new NoDamageHealthManager(player);
		}


		[Command(Command = "s")]
		public void Stats(Player currentPlayer)
		{
			var players = Context.LevelManager.Levels[0].Players.Values.ToArray();
			currentPlayer.SendMessage("Statistics:", type: McpeText.TypeRaw);
			foreach (var player in players)
			{
				currentPlayer.SendMessage(string.Format("RTT: {1:0000} User: {0}", player.Username, player.Rtt), type: McpeText.TypeRaw);
			}
		}

		[Command(Command = "r")]
		[Authorize(Users = "gurun")]
		public void DisplayRestartNotice(Player currentPlayer)
		{
			var players = currentPlayer.Level.GetSpawnedPlayers();
			foreach (var player in players)
			{
				player.AddPopup(new Popup()
				{
					Priority = 100, MessageType = MessageType.Tip, Message = "SERVER WILL RESTART!", Duration = 20*10,
				});

				player.AddPopup(new Popup()
				{
					Priority = 100, MessageType = MessageType.Popup, Message = "Transfering all players!", Duration = 20*10,
				});

				Thread.Sleep(1500);

				IPHostEntry host = Dns.GetHostEntry("test.inpvp.net");
				Context.Server.ForwardTarget = new IPEndPoint(host.AddressList[0], 19132);
				Context.Server.ForwardAllPlayers = true;
			}
		}

		private byte _invId = 0;

		[Command(Command = "oi")]
		public void OpenInventory(Player player)
		{
			BlockCoordinates coor = new BlockCoordinates(player.KnownPosition);
			Chest chest = new Chest
			{
				Coordinates = coor, Metadata = 0
			};
			player.Level.SetBlock(chest, true);

			// Then we create and set the sign block entity that has all the intersting data

			ChestBlockEntity chestBlockEntity = new ChestBlockEntity
			{
				Coordinates = coor
			};

			player.Level.SetBlockEntity(chestBlockEntity, false);

			player.OpenInventory(coor);
		}
	}
}