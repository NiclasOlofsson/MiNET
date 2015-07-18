using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using MiNET;
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
		private Dictionary<string, Level> _worlds = new Dictionary<string, Level>();

		private Timer _scoreboardTimer;
		private Timer _gameTimer;
		private Timer _popupTimer;


		protected override void OnEnable()
		{
			_popupTimer = new Timer(DoDevelopmentPopups, null, 10000, 20000);
			//_gameTimer = new Timer(StartNewRoundCallback, null, 15000, 60000*3);
		}

		[PacketHandler, Receive]
		public Package ChatHandler(McpeText text, Player player)
		{
			if (text.message.StartsWith("/") || text.message.StartsWith(".")) return text;

			player.Level.BroadcastTextMessage((" §7" + player.Username + "§7: §r§f" + text.message), null, MessageType.Raw);
			return null;
		}

		private void DoDevelopmentPopups(object state)
		{
			foreach (var level in Context.Levels)
			{
				var players = level.GetSpawnedPlayers();
				foreach (var player in players)
				{
					player.AddPopup(new Popup()
					{
						MessageType = MessageType.Popup,
						Message = "Restarts without notice frequently",
						Duration = 20*5,
						DisplayDelay = 20*1
					});
					player.AddPopup(new Popup()
					{
						MessageType = MessageType.Tip,
						Message = "This is a development server",
						Duration = 20*4
					});
				}
			}
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

		//[Command]
		//public void Login(Player player, string password)
		//{
		//	UserManager<User> userManager = player.Server.UserManager;
		//	if (userManager != null)
		//	{
		//		if (player.Username == null) return;

		//		User user = userManager.FindByName(player.Username);

		//		if (user == null)
		//		{
		//			user = new User(player.Username);
		//			if (!userManager.Create(user, password).Succeeded) return;
		//		}

		//		if (userManager.CheckPassword(user, password))
		//		{
		//			player.SendMessage("Login successful");
		//		}
		//		else
		//		{
		//			player.SendMessage("Login failed");
		//		}
		//	}
		//}

		[Command(Command = "items")]
		public void AddItems(Player player, byte itemId, int noItems)
		{
			for (int i = 0; i < noItems; i++)
			{
				player.Level.DropItem(new BlockCoordinates(player.KnownPosition) + 10, new ItemStack(itemId, 1));
			}
		}

		[Command(Command = "gm")]
		[Authorize(Users = "gurun")]
		public void GameMode(Player player, int gameMode)
		{
			player.GameMode = (GameMode) gameMode;
			player.SendPackage(new McpeStartGame
			{
				seed = -1,
				generator = 1,
				gamemode = gameMode,
				entityId = player.EntityId,
				spawnX = player.Level.SpawnPoint.X,
				spawnY = player.Level.SpawnPoint.Y,
				spawnZ = player.Level.SpawnPoint.Z,
				x = player.KnownPosition.X,
				y = player.KnownPosition.Y,
				z = player.KnownPosition.Z
			});

			player.Level.BroadcastTextMessage(string.Format("{0} changed to game mode {1}.", player.Username, gameMode), type: MessageType.Raw);
		}


		[Command(Command = "tp")]
		public void Teleport(Player player, int x, int y, int z)
		{
			// send teleport to spawn
			player.KnownPosition = new PlayerLocation
			{
				X = x,
				Y = y,
				Z = z,
				Yaw = 91,
				Pitch = 28,
				HeadYaw = 91
			};

			player.SendMovePlayer();
			player.Level.BroadcastTextMessage(string.Format("{0} teleported to coordinates {1},{2},{3}.", player.Username, x, y, z), type: MessageType.Raw);
		}

		//[Command(Command = "tp")]
		//public void Teleport(Player player)
		//{
		//	if (!_worlds.ContainsKey("Default")) return;
		//	Teleport(player, "Default");
		//}

		//[Command(Command = "tp")]
		//public void Teleport(Player player, string world)
		//{
		//	if (player.Level.LevelId.Equals(world)) return;

		//	if (!_worlds.ContainsKey(player.Level.LevelId))
		//	{
		//		_worlds.Add(player.Level.LevelId, player.Level);
		//	}


		//	if (!_worlds.ContainsKey(world))
		//	{
		//		_worlds.Add(world, new Level(world, new FlatlandWorldProvider()));
		//	}

		//	Level level = _worlds[world];
		//	player.SpawnLevel(level);
		//	level.BroadcastTextMessage(string.Format("{0} teleported to world {1}.", player.Username, level.LevelId));
		//}

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
			player.Level.BroadcastTextMessage(string.Format("Current view distance set to {0}.", player.Level.ViewDistance), type: MessageType.Raw);
		}

		[Command(Command = "vd")]
		public void ViewDistance(Player player, int viewDistance)
		{
			player.Level.ViewDistance = viewDistance;
			player.Level.BroadcastTextMessage(string.Format("View distance changed to {0}.", player.Level.ViewDistance), type: MessageType.Raw);
		}

		[Command(Command = "twitter")]
		public void Twitter(Player player)
		{
			player.Level.BroadcastTextMessage("§6Twitter @NiclasOlofsson", type: MessageType.Raw);
			player.Level.BroadcastTextMessage("§5twitch.tv/niclasolofsson", type: MessageType.Raw);
		}

		[Command(Command = "pi")]
		public void PlayerInfo(Player player)
		{
			player.SendMessage(string.Format("Username={0}", player.Username), type: MessageType.Raw);
			player.SendMessage(string.Format("Entity ID={0}", player.EntityId), type: MessageType.Raw);
			player.SendMessage(string.Format("Client GUID={0}", player.ClientGuid), type: MessageType.Raw);
			player.SendMessage(string.Format("Client ID={0}", player.ClientId), type: MessageType.Raw);
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

			player.SendMessage(string.Format("Region X={0} Z={1}", chunkX >> 5, chunkZ >> 5), type: MessageType.Raw);
			player.SendMessage(string.Format("Local chunk X={0} Z={1}", xi, zi), type: MessageType.Raw);
			player.SendMessage(string.Format("Chunk X={0} Z={1}", chunkX, chunkZ), type: MessageType.Raw);
			player.SendMessage(string.Format("Position X={0:F1} Y={1:F1} Z={2:F1}", player.KnownPosition.X, player.KnownPosition.Y, player.KnownPosition.Z), type: MessageType.Raw);
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

		//[Command]
		//public void Hide(Player player)
		//{
		//	player.Level.HidePlayer(player, true);
		//	player.Level.BroadcastTextMessage(string.Format("Player {0} hides.", player.Username), type: MessageType.Raw);
		//}

		//[Command]
		//public void Unhide(Player player)
		//{
		//	player.Level.HidePlayer(player, false);
		//	player.Level.BroadcastTextMessage(string.Format("Player {0} unhides.", player.Username), type: MessageType.Raw);
		//}


		//private Dictionary<Player, Entity> _playerEntities = new Dictionary<Player, Entity>();

		//[Command]
		//[Authorize(Users = "gurun")]
		//public void Hide(Player player, byte id)
		//{
		//	Level level = player.Level;

		//	level.HidePlayer(player, true);

		//	Mob entity = new Mob(id, level)
		//	{
		//		KnownPosition = player.KnownPosition,
		//		//Data = -(blockId | 0 << 0x10)
		//	};
		//	entity.SpawnEntity();

		//	player.SendPackage(new McpeRemoveEntity()
		//	{
		//		entityId = entity.EntityId,
		//	});

		//	_playerEntities[player] = entity;

		//	level.BroadcastTextMessage(string.Format("Player {0} spawned as other entity.", player.Username), type: MessageType.Raw);
		//}


		[Command]
		[Authorize(Users = "gurun")]
		public void Kill(Player player, string target)
		{
			Player targetPlayer = player.Level.Players.FirstOrDefault(player1 => player1.Username == target);
			if (targetPlayer != null)
			{
				targetPlayer.HealthManager.Kill();
			}
		}

		//[PacketHandler, Receive]
		//public Package HandleIncoming(McpeMovePlayer packet, Player player)
		//{
		//	if (_playerEntities.ContainsKey(player))
		//	{
		//		var entity = _playerEntities[player];
		//		entity.KnownPosition = player.KnownPosition;
		//		var message = new McpeMoveEntity();
		//		message.entities = new EntityLocations();
		//		message.entities.Add(entity.EntityId, entity.KnownPosition);
		//		player.Level.RelayBroadcast(message);
		//	}

		//	return packet; // Process
		//}

		[Command]
		public void Kit(Player player, int kitId)
		{
			var armor = player.Inventory.Armor;
			var slots = player.Inventory.Slots;

			switch (kitId)
			{
				case 0:
					// Kit leather tier
					armor[0] = new MetadataSlot(new ItemStack(298)); // Helmet
					armor[1] = new MetadataSlot(new ItemStack(299)); // Chest
					armor[2] = new MetadataSlot(new ItemStack(300)); // Leggings
					armor[3] = new MetadataSlot(new ItemStack(301)); // Boots
					break;
				case 1:
					// Kit gold tier
					armor[0] = new MetadataSlot(new ItemStack(314)); // Helmet
					armor[1] = new MetadataSlot(new ItemStack(315)); // Chest
					armor[2] = new MetadataSlot(new ItemStack(316)); // Leggings
					armor[3] = new MetadataSlot(new ItemStack(317)); // Boots
					break;
				case 2:
					// Kit chain tier
					armor[0] = new MetadataSlot(new ItemStack(302)); // Helmet
					armor[1] = new MetadataSlot(new ItemStack(303)); // Chest
					armor[2] = new MetadataSlot(new ItemStack(304)); // Leggings
					armor[3] = new MetadataSlot(new ItemStack(305)); // Boots
					break;
				case 3:
					// Kit iron tier
					armor[0] = new MetadataSlot(new ItemStack(306)); // Helmet
					armor[1] = new MetadataSlot(new ItemStack(307)); // Chest
					armor[2] = new MetadataSlot(new ItemStack(308)); // Leggings
					armor[3] = new MetadataSlot(new ItemStack(309)); // Boots
					break;
				case 4:
					// Kit diamond tier
					armor[0] = new MetadataSlot(new ItemStack(310)); // Helmet
					armor[1] = new MetadataSlot(new ItemStack(311)); // Chest
					armor[2] = new MetadataSlot(new ItemStack(312)); // Leggings
					armor[3] = new MetadataSlot(new ItemStack(313)); // Boots
					break;
			}

			byte c = 0;
			slots[c++] = new MetadataSlot(new ItemStack(268, 1)); // Wooden Sword
			slots[c++] = new MetadataSlot(new ItemStack(283, 1)); // Golden Sword
			slots[c++] = new MetadataSlot(new ItemStack(272, 1)); // Stone Sword
			slots[c++] = new MetadataSlot(new ItemStack(267, 1)); // Iron Sword
			slots[c++] = new MetadataSlot(new ItemStack(276, 1)); // Diamond Sword

			slots[c++] = new MetadataSlot(new ItemStack(261, 1)); // Bow
			slots[c++] = new MetadataSlot(new ItemStack(262, 64)); // Arrows
			slots[c++] = new MetadataSlot(new ItemStack(344, 64)); // Eggs
			slots[c++] = new MetadataSlot(new ItemStack(332, 64)); // Snowballs

			player.SendPackage(new McpeContainerSetContent
			{
				windowId = 0,
				slotData = player.Inventory.Slots,
				hotbarData = player.Inventory.ItemHotbar
			});

			player.SendPackage(new McpeContainerSetContent
			{
				windowId = 0x78, // Armor windows ID
				slotData = player.Inventory.Armor,
				hotbarData = null
			});

			SendEquipmentForPlayer(player);
			SendArmorForPlayer(player);

			player.Level.BroadcastTextMessage(string.Format("Player {0} changed kit.", player.Username), type: MessageType.Raw);
		}

		private void SendEquipmentForPlayer(Player player)
		{
			player.Level.RelayBroadcast(new McpePlayerEquipment
			{
				entityId = player.EntityId,
				item = player.Inventory.ItemInHand.Value.Id,
				meta = player.Inventory.ItemInHand.Value.Metadata,
				slot = 0
			});
		}

		private void SendArmorForPlayer(Player player)
		{
			player.Level.RelayBroadcast(new McpePlayerArmorEquipment
			{
				entityId = player.EntityId,
				helmet = (byte) (((MetadataSlot) player.Inventory.Armor[0]).Value.Id - 256),
				chestplate = (byte) (((MetadataSlot) player.Inventory.Armor[1]).Value.Id - 256),
				leggings = (byte) (((MetadataSlot) player.Inventory.Armor[2]).Value.Id - 256),
				boots = (byte) (((MetadataSlot) player.Inventory.Armor[3]).Value.Id - 256)
			});
		}

		[Command]
		public void Fly(Player player)
		{
			player.SendPackage(new McpeAdventureSettings {flags = 0x80});
			player.Level.BroadcastTextMessage(string.Format("Player {0} enabled flying.", player.Username), type: MessageType.Raw);
		}

		[Command(Command = "e")]
		public void Effect(Player player)
		{
			Effect(player, 1, 3, 20);
		}

		[Command(Command = "e")]
		public void Effect(Player player, int effectId, int amplifier = 1, int duration = 20)
		{
			player.SendPackage(new McpeMobEffect
			{
				entityId = player.EntityId,
				eventId = 1, // Add
				effectId = (byte) effectId,
				duration = 20*duration,
				amplifier = (byte) amplifier,
				particles = 0,
			});
			player.Level.BroadcastTextMessage(string.Format("{0} added effect {1} with strenght {2}", player.Username, effectId, amplifier), type: MessageType.Raw);
		}

		[Command(Command = "s")]
		public void Stats(Player currentPlayer)
		{
			var players = Context.Levels[0].Players.ToArray();
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
					Priority = 100,
					MessageType = MessageType.Tip,
					Message = "SERVER WILL RESTART!",
					Duration = 20*10,
				});

				player.AddPopup(new Popup()
				{
					Priority = 100,
					MessageType = MessageType.Popup,
					Message = "Transfering all players!",
					Duration = 20*10,
				});

				Thread.Sleep(1500);

				IPHostEntry host = Dns.GetHostEntry("test.inpvp.net");
				Context.Server.ForwardTarget = new IPEndPoint(host.AddressList[0], 19132);
				Context.Server.ForwardAllPlayers = true;
			}
		}

		private void StartNewRoundCallback(object state)
		{
			if (_scoreboardTimer == null)
			{
				_scoreboardTimer = new Timer(ScoreboardCallback, null, 5000, 47000);

				Context.Levels[0].BroadcastTextMessage(
					"§6§l»§r§7 --------------------------- §6§l«\n"
					+ "§e GAME STARTED\n"
					+ "§6§l»§r§7 --------------------------- §6§l«", type: McpeText.TypeRaw);
			}
			else
			{
				var players = Context.Levels[0].GetSpawnedPlayers();
				if (players.Length <= 1) return;

				var winner = players.OrderByDescending(CalculatedKdRatio).FirstOrDefault();

				if (winner != null)
				{
					Context.Levels[0].BroadcastTextMessage(
						"§6§l»§r§7 --------------------------- §6§l«\n"
						+ "§e Winner!!\n"
						+ "§e       " + winner.Username + "\n", type: McpeText.TypeRaw);
				}
				foreach (var player in players)
				{
					player.Kills = 0;
					player.Deaths = 0;
				}

				Context.Levels[0].BroadcastTextMessage(
					"§e NEW ROUND STARTED\n"
					+ "§6§l»§r§7 --------------------------- §6§l«", type: McpeText.TypeRaw);
			}
		}

		private static double CalculatedKdRatio(Player player)
		{
			return player.Deaths == 0 ? player.Kills : player.Kills/((double) player.Deaths);
		}

		private void ScoreboardCallback(object state)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("§6§l»§r§7 --------------------------- §6§l«\n");
			var players = Context.Levels[0].GetSpawnedPlayers();
			if (players.Length <= 1) return;

			foreach (var player in players.OrderByDescending(CalculatedKdRatio).Take(5))
			{
				sb.AppendFormat("K/D: {3:0.00} K: {1:00} D: {2:00} {0}\n", player.Username, player.Kills, player.Deaths, CalculatedKdRatio(player));
			}
			sb.Append("§6§l»§r§7 --------------------------- §6§l«\n");
			Context.Levels[0].BroadcastTextMessage(sb.ToString(), type: McpeText.TypeRaw);
		}
	}
}