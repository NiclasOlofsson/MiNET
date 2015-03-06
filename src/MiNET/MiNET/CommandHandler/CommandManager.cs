using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using log4net;
using MiNET.Plugins.Attributes;
using MiNET.Worlds;

namespace MiNET.CommandHandler
{
	internal class CommandManager
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (MiNetServer));

		public void ConsoleCMDHandler(MiNetServer server, Level level)
		{
			//Player console = new Player(server, new IPEndPoint((long) 0, 1), level,  0);
			//console.Permissions.SetGroup(UserGroup.Operator);
			//console.Console = true;
			//console.IsConnected = false;
			//while (true)
			//{
			//	try
			//	{
			//		string cmd = Console.ReadLine();
			//		HandleCommand(cmd, console, true);
			//	}
			//	catch (Exception ex)
			//	{
			//		//Ignore it, just because we are a console 'player':P
			//	}
			//}
		}

		public void HandleCommand(string message, Player player, bool console = false)
		{
			try
			{
				string commandText = message.Split(' ')[0];
				message = message.Replace(commandText, "").Trim();
				commandText = commandText.Replace("/", "").Replace(".", "");

				string[] arguments = message.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);

				foreach (var command in Commands)
				{
					if (!commandText.Equals(command.Command, StringComparison.InvariantCultureIgnoreCase)) continue;

					if (!player.Permissions.HasPermission(command.Permission) && !player.Permissions.HasPermission("*") && !player.Permissions.IsInGroup(UserGroup.Operator))
					{
						if (!console) player.SendMessage("You are not permitted to use this command!");
						return;
					}

					if (!command.Execute(player, arguments))
					{
						if (!console) player.SendMessage(command.Usage);
					}

					return;
				}

				foreach (var handlerEntry in PluginCommands)
				{
					CommandAttribute commandAttribute = handlerEntry.Key;
					if (!commandText.Equals(commandAttribute.Command, StringComparison.InvariantCultureIgnoreCase)) continue;

					if (!player.Permissions.HasPermission(commandAttribute.Permission) && !player.Permissions.HasPermission("*") && !player.Permissions.IsInGroup(UserGroup.Operator))
					{
						if (!console) player.SendMessage("You are not permitted to use this command!");
						return;
					}

					MethodInfo method = handlerEntry.Value;
					if (method == null) return;

					if (ExecutePluginCommand(method, player, arguments)) return;
				}
			}
			catch (Exception ex)
			{
				Log.Warn(ex);
			}
		}

		private bool ExecutePluginCommand(MethodInfo method, Player player, string[] args)
		{
			var parameters = method.GetParameters();

			int addLenght = 0;
			if (parameters.Length > 0 && parameters[0].ParameterType == typeof (Player))
			{
				addLenght = 1;
			}

			if (parameters.Length != args.Length + addLenght) return false;

			object[] objectArgs = new object[parameters.Length];

			for (int k = 0; k < parameters.Length; k++)
			{
				var parameter = parameters[k];
				int i = k - addLenght;
				if (k == 0 && addLenght == 1)
				{
					if (parameter.ParameterType == typeof (Player))
					{
						objectArgs[k] = player;
						continue;
					}
					Log.WarnFormat("Command method {0} missing Player as first argument.", method.Name);
					return false;
				}

				if (parameter.ParameterType == typeof (string))
				{
					objectArgs[k] = args[i];
					continue;
				}
				if (parameter.ParameterType == typeof (short))
				{
					short value;
					if (!short.TryParse(args[i], out value)) return false;
					objectArgs[k] = value;
					continue;
				}
				if (parameter.ParameterType == typeof (int))
				{
					int value;
					if (!int.TryParse(args[i], out value)) return false;
					objectArgs[k] = value;
					continue;
				}
				if (parameter.ParameterType == typeof (bool))
				{
					bool value;
					if (!bool.TryParse(args[i], out value)) return false;
					objectArgs[k] = value;
					continue;
				}
				if (parameter.ParameterType == typeof (float))
				{
					float value;
					if (!float.TryParse(args[i], out value)) return false;
					objectArgs[k] = value;
					continue;
				}
				if (parameter.ParameterType == typeof (double))
				{
					double value;
					if (!double.TryParse(args[i], out value)) return false;
					objectArgs[k] = value;
					continue;
				}

				return false;
			}

			if (method.IsStatic)
			{
				method.Invoke(null, objectArgs);
			}
			else
			{
				if (method.DeclaringType == null) return false;

				object obj = Activator.CreateInstance(method.DeclaringType);
				method.Invoke(obj, objectArgs);
			}

			return true;
		}

		public ICommandHandler[] Commands =
		{
			new TestCommand(),
			new HelpCommand(),
			//new BoomCommand(),
			new OpCommand(),
			new DeOpCommand(),
			new GiveCommand(),
		};

		public static Dictionary<CommandAttribute, MethodInfo> PluginCommands = new Dictionary<CommandAttribute, MethodInfo>();
	}
}