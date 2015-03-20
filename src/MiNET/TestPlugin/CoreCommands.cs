using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using Microsoft.AspNet.Identity;
using MiNET;
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
				BodyYaw = 91
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

	}
}