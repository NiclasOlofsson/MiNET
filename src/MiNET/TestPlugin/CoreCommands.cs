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

			player.SpawnLevel(_worlds[world]);
		}
	}
}