using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using Microsoft.AspNet.Identity;
using MiNET;
using MiNET.Entities;
using MiNET.Net;
using MiNET.Plugins;
using MiNET.Plugins.Attributes;
using MiNET.Security;
using MiNET.Utils;
using MiNET.Worlds;

namespace TestPlugin
{
	[Plugin(PluginName = "CoreCommands", Description = "The core commands for MiNET", PluginVersion = "1.0", Author = "MiNET Team")]
	public class CoreCommands : Plugin
	{
		private Dictionary<string, Level> _worlds = new Dictionary<string, Level>();

		[Command]
		public void Version(Player player)
		{
			string productVersion = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion;
			player.SendMessage(string.Format("MiNET v{0}", productVersion));
		}

		[Command]
		public void Plugins(Player player)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("Plugins: ");
			foreach (var plugin in Context.PluginManager.Plugins)
			{
				sb.Append(plugin.GetType().Name);
				sb.Append(" ");
			}

			player.SendMessage(sb.ToString());
		}

		[Command]
		public void Login(Player player, string password)
		{
			UserManager<User> userManager = player.Server.UserManager;
			if (userManager != null)
			{
				if (player.Username == null) return;

				User user = userManager.FindByName(player.Username);

				if (user == null)
				{
					user = new User(player.Username);
					if (!userManager.Create(user, password).Succeeded) return;
				}

				if (userManager.CheckPassword(user, password))
				{
					player.SendMessage("Login successful");
				}
			}
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
			player.Level.BroadcastTextMessage(string.Format("{0} teleported to coordinates {1},{2},{3}.", player.Username, x, y, z));
		}

		[Command(Command = "tp")]
		public void Teleport(Player player)
		{
			if (!_worlds.ContainsKey("Default")) return;
			Teleport(player, "Default");
		}

		[Command(Command = "tp")]
		public void Teleport(Player player, string world)
		{
			if (player.Level.LevelId.Equals(world)) return;

			if (!_worlds.ContainsKey(player.Level.LevelId))
			{
				_worlds.Add(player.Level.LevelId, player.Level);
			}


			if (!_worlds.ContainsKey(world))
			{
				_worlds.Add(world, new Level(world, new FlatlandWorldProvider()));
			}

			Level level = _worlds[world];
			player.SpawnLevel(level);
			level.BroadcastTextMessage(string.Format("{0} teleported to world {1}.", player.Username, level.LevelId));
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
			player.Level.BroadcastTextMessage(string.Format("Current view distance set to {0}.", player.Level.ViewDistance));
		}

		[Command(Command = "vd")]
		public void ViewDistance(Player player, int viewDistance)
		{
			player.Level.ViewDistance = viewDistance;
			player.Level.BroadcastTextMessage(string.Format("View distance changed to {0}.", player.Level.ViewDistance));
		}

		[Command(Command = "pi")]
		public void PlayerInfo(Player player)
		{
			player.SendMessage(string.Format("Username={0}", player.Username));
			player.SendMessage(string.Format("Entity ID={0}", player.EntityId));
			player.SendMessage(string.Format("Client GUID={0}", player.ClientGuid));
			player.SendMessage(string.Format("Client ID={0}", player.ClientId));
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

			player.SendMessage(string.Format("Region X={0} Z={1}", chunkX >> 5, chunkZ >> 5));
			player.SendMessage(string.Format("Local chunk X={0} Z={1}", xi, zi));
			player.SendMessage(string.Format("Chunk X={0} Z={1}", chunkX, chunkZ));
			player.SendMessage(string.Format("Position X={0:F1} Y={1:F1} Z={2:F1}", player.KnownPosition.X, player.KnownPosition.Y, player.KnownPosition.Z));
		}

		[Command]
		public void Spawn(Player player, byte id)
		{
			Level level = player.Level;

			Mob entity = new Mob(id, level)
			{
				KnownPosition = player.KnownPosition,
				//Data = -(blockId | 0 << 0x10)
			};
			entity.SpawnEntity();

			level.BroadcastTextMessage(string.Format("Player {0} spawned Mob #{1}.", player.Username, id));
		}

		[Command]
		public void Hide(Player player)
		{
			player.Level.HidePlayer(player, true);
			player.Level.BroadcastTextMessage(string.Format("Player {0} hides.", player.Username));
		}

		[Command]
		public void Unhide(Player player)
		{
			player.Level.HidePlayer(player, false);
			player.Level.BroadcastTextMessage(string.Format("Player {0} unhides.", player.Username));
		}


		private Dictionary<Player, Entity> _playerEntities = new Dictionary<Player, Entity>();

		[Command]
		public void Hide(Player player, byte id)
		{
			Level level = player.Level;

			level.HidePlayer(player, true);

			Mob entity = new Mob(id, level)
			{
				KnownPosition = player.KnownPosition,
				//Data = -(blockId | 0 << 0x10)
			};
			entity.SpawnEntity();

			player.SendPackage(new McpeRemoveEntity()
			{
				entityId = entity.EntityId,
			});

			_playerEntities[player] = entity;

			level.BroadcastTextMessage(string.Format("Player {0} spawned as other entity.", player.Username));
		}

		[PacketHandler, Receive]
		public Package HandleIncoming(McpeMovePlayer packet, Player player)
		{
			if (_playerEntities.ContainsKey(player))
			{
				var entity = _playerEntities[player];
				entity.KnownPosition = player.KnownPosition;
				var message = new McpeMoveEntity();
				message.entities = new EntityLocations();
				message.entities.Add(entity.EntityId, entity.KnownPosition);
				player.Level.RelayBroadcast(message);
			}

			return packet; // Process
		}

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

			slots[0] = new MetadataSlot(new ItemStack(268, 1)); // Wooden Sword
			slots[1] = new MetadataSlot(new ItemStack(283, 1)); // Golden Sword
			slots[2] = new MetadataSlot(new ItemStack(272, 1)); // Stone Sword
			slots[3] = new MetadataSlot(new ItemStack(267, 1)); // Iron Sword
			slots[3] = new MetadataSlot(new ItemStack(276, 1)); // Diamond Sword

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

			player.Level.BroadcastTextMessage(string.Format("Player {0} changed kit.", player.Username));
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
	}
}