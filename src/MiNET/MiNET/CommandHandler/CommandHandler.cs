using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiNET.CommandHandler
{
	class CommandHandler
	{
		public void HandleCommand(string message, Player player)
		{
			string _command = message.Split(' ')[0];
			message = message.Replace(_command + " ", "");
			string[] _splittedCommand = message.Split(' ');
			_command = _command.Replace("/", "");

			foreach (var command in Commands)
			{
				if (command.Command == _command)
				{
					if (command.RequireOperator && !player.IsOperator)
					{
						player.SendMessage("You are not permitted to use this command!");
						break;
					}

					if (!command.Execute(player, _splittedCommand))
					{
						player.SendMessage(command.Usage);
					}
					break;
				}
			}
		}

		public ICommandHandler[] Commands = new ICommandHandler[] { new TestCommand(), new HelpCommand(), new BoomCommand() };
	}
}
