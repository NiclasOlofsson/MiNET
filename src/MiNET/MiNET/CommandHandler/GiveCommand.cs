using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MiNET.Utils;

namespace MiNET.CommandHandler
{
	class GiveCommand : ICommandHandler
	{
		public string Command
		{
			get { return "give"; }
		}

		public string Description
		{
			get { return "Give the specified player an item."; }
		}

		public string Usage
		{
			get { return "/give <item> <amount> [player]"; }
		}

		public string Permission
		{
			get { return "MiNET.Give"; }
		}

		public bool Execute(Player player, string[] arguments)
		{
			try
			{
				if (arguments.Length == 2)
				{
					int amount = Convert.ToInt32(arguments[1]);
					int item = Convert.ToInt32(arguments[0]);
					Give(item, amount, player);
					return true;
				}
				else if (arguments.Length == 3)
				{
					int amount = Convert.ToInt32(arguments[1]);
					int item = Convert.ToInt32(arguments[0]);
					Player target = GetPlayer(arguments[2], player);
					Give(item, amount, target);
					return true;
				}
				else
				{
					return false;
				}
			}
			catch
			{
				return false;
			}
		}

		private void Give(int item, int amount, Player player)
		{
			player.Level.DropItem(new BlockCoordinates((int)player.KnownPosition.X, (int)player.KnownPosition.Y, (int)player.KnownPosition.Z), new ItemStack((short)item, (byte)amount));	
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
