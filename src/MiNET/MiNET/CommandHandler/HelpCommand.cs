using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiNET.PluginSystem.Attributes;

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

		public string Permission { get { return "MiNET.help"; } }

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

			foreach (var i in CommandHandler.PluginCommands)
			{
				CommandAttribute cmd = (CommandAttribute) i.Key;
				if (cmd.Command == command)
				{
					return cmd.Command + ": " + cmd.Description + "\nUsage: " + cmd.Usage;
				}
			}

			return "Command not found!";
		}
	}
}
