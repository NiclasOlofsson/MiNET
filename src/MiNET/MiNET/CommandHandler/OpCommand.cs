using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace MiNET.CommandHandler
{
	class OpCommand : ICommandHandler
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(MiNetServer));
		public string Command
		{
			get { return "op"; }
		}

		public string Description
		{
			get { return "Gives a player OP permissions."; }
		}

		public string Usage
		{
			get { return "/op <player>"; }
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
				target.Permissions.SetGroup(UserGroup.Operator);

				if (!player.Console)
				{
					player.SendMessage("Player is now an Operator.");
					target.SendMessage("[SERVER] " + player.Username + " just gave you operator rights!");
				}
				else
				{
					Log.Info("Player is now an Operator.");
					target.SendMessage("[SERVER] " + player.Username + " just gave you operator rights!");
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
