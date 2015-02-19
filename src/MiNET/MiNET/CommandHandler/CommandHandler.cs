using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using log4net;
using MiNET.PluginSystem.Attributes;
using MiNET.Worlds;

namespace MiNET.CommandHandler
{
	internal class CommandHandler
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (MiNetServer));

		public void ConsoleCMDHandler(MiNetServer server, Level level)
		{
			Player console = new Player(server, new IPEndPoint((long) 0, 1), level, 0);
			console.Permissions.SetGroup(UserGroup.Operator);
			console.Console = true;
			console.IsConnected = false;
			while (true)
			{
				try
				{
					string cmd = Console.ReadLine();
					HandleCommand(cmd, console, true);
				}
				catch (Exception ex)
				{
					//Ignore it, just because we are a console 'player':P
				}
			}
		}

		public void HandleCommand(string message, Player player, bool console = false)
		{
			string _command = message.Split(' ')[0];
			message = message.Replace(_command + " ", "");
			string[] _splittedCommand = message.Split(' ');
			_command = _command.Replace("/", "");

			foreach (var command in Commands)
			{
				if (command.Command == _command)
				{
					if (!player.Permissions.HasPermission(command.Permission) && !player.Permissions.HasPermission("*") && !player.Permissions.IsInGroup(UserGroup.Operator))
					{
						if (!console) player.SendMessage("You are not permitted to use this command!");
						return;
					}

					if (!command.Execute(player, _splittedCommand))
					{
						if (!console) player.SendMessage(command.Usage);
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

					if (!player.Permissions.HasPermission(atrib.Permission) && !player.Permissions.HasPermission("*") && !player.Permissions.IsInGroup(UserGroup.Operator))
					{
						if (!console) player.SendMessage("You are not permitted to use this command!");
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
				new Task(() => method.Invoke(obj, new object[] {player, args})).Start();
			}
		}

		public ICommandHandler[] Commands = new ICommandHandler[] {new TestCommand(), new HelpCommand(), new BoomCommand(), new OpCommand(), new DeOpCommand(), new GiveCommand(),  };
		public static Dictionary<Attribute, MethodInfo> PluginCommands = new Dictionary<Attribute, MethodInfo>();
	}
}