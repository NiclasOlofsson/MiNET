using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace MiNET.CommandHandler
{
	class DeOpCommand : ICommandHandler
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(MiNetServer));
		public string Command
		{
			get { return "deop"; }
		}

		public string Description
		{
			get { return "Removes player OP permissions."; }
		}

		public string Usage
		{
			get { return "/deop <player>"; }
		}

		public string Permission
		{
			get { return "MiNET.OP"; }
		}

		public bool Execute(Player player, string[] arguments)
		{
			if (arguments.Length >= 1)
			{
				Player target = GetPlayer(arguments[0], player);
				if (!target.Console)
				{
					target.Permissions.SetGroup(UserGroup.User);

					if (!player.Console)
					{
						player.SendMessage("Player is now now a user.");
						target.SendMessage("[SERVER] " + player.Username + " just removed your operator rights!");
					}
					else
					{
						Log.Info("Player is now a user.");
						target.SendMessage("[SERVER] " + player.Username + " just removed your operator rights!");
					}
				}

				return true;
			}
			else
			{
				if (!player.Console)
				{
					player.SendMessage(Usage);
				}
				else
				{
					Log.Info(Usage);
				}
			}
			return false;
		}

		private Player GetPlayer(string name, Player source)
		{
			foreach (var user in source.Level.Players)
			{
				if (user.Username.Contains(name))
				{
					return user;
				}
			}
			return source;
		}
	}
}
