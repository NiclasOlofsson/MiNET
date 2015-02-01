using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using log4net;
using System.Linq.Expressions;
using MiNET.PluginSystem.Attributes;

namespace MiNET.CommandHandler
{
	class CommandHandler
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(MiNetServer));
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
					if (!player.Permissions.HasPermission(command.Permission) && !player.Permissions.HasPermission("*"))
					{
						player.SendMessage("You are not permitted to use this command!");
						return;
					}

					if (!command.Execute(player, _splittedCommand))
					{
						player.SendMessage(command.Usage);
					}
					break;
				}
			}

			foreach (var cmd in PluginCommands)
			{
				try
				{
					CommandAttribute atrib = (CommandAttribute) cmd.Key;
					if (atrib.Command != _command) continue;

					if (!player.Permissions.HasPermission(atrib.Permission) && !player.Permissions.HasPermission("*"))
					{
						player.SendMessage("You are not permitted to use this command!");
						return;
					}

					var method = cmd.Value;
					if (method == null) return;
					ExecutePluginCommand(method, player, _splittedCommand);
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex);
				}
			}
		}

		private void ExecutePluginCommand(MethodInfo method, Player player, string[] args)
		{
			if (method.IsStatic)
			{
				new Task(() => method.Invoke(null, new object[] {player, args})).Start();
			}
			else
			{
				object obj = Activator.CreateInstance(method.DeclaringType);
				new Task(() => method.Invoke(obj, new object[] { player, args })).Start();
			}
		}

		public ICommandHandler[] Commands = new ICommandHandler[] { new TestCommand(), new HelpCommand(), new BoomCommand() };
		public static Dictionary<Attribute, MethodInfo> PluginCommands = new Dictionary<Attribute, MethodInfo>(); 
	}
}
