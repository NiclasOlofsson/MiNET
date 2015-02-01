using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiNET.CommandHandler
{
	class HelpCommand : ICommandHandler
	{
		public string Command
		{
			get { return "help"; }
		}

		public string Description
		{
			get { return "Get information about a specific command."; }
		}

		public string Usage
		{
			get { return "/help <COMMAND>"; }
		}

		public bool RequireOperator
		{
			get { return false; }
		}

		public bool Execute(Player player, string[] arguments)
		{
			if (arguments.Length >= 1)
			{
				player.SendMessage(getCommandHelp(arguments[0]));
				return true;
			}
			return false;
		}

		private string getCommandHelp(string command, bool usage = false)
		{
			foreach (ICommandHandler i in new CommandHandler().Commands)
			{
				if (i.Command == command)
				{
					if (!usage)
					{
						return i.Command + ": " + i.Description + "\nUsage: " + i.Usage;
					}
					break;
				}
			}
			return "Command not found!";
		}
	}
}
