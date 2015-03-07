using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using log4net;
using MiNET.Net;
using MiNET.Plugins.Attributes;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Plugins
{
	public class PluginManager
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (MiNetServer));

		private readonly List<IPlugin> _plugins = new List<IPlugin>();
		private readonly Dictionary<MethodInfo, PacketHandlerAttribute> _packetHandlerDictionary = new Dictionary<MethodInfo, PacketHandlerAttribute>();
		private readonly Dictionary<MethodInfo, PacketHandlerAttribute> _packetSendHandlerDictionary = new Dictionary<MethodInfo, PacketHandlerAttribute>();
		private readonly Dictionary<MethodInfo, CommandAttribute> _pluginCommands = new Dictionary<MethodInfo, CommandAttribute>();

		internal void LoadPlugins()
		{
			string pluginDirectory = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
			pluginDirectory = ConfigParser.GetProperty("PluginDirectory", pluginDirectory);
			if (pluginDirectory != null)
			{
				pluginDirectory = Path.GetFullPath(pluginDirectory);

				foreach (string pluginPath in Directory.GetFiles(pluginDirectory, "*.dll", SearchOption.AllDirectories))
				{
					Assembly newAssembly = Assembly.LoadFile(pluginPath);
					Type[] types = newAssembly.GetExportedTypes();
					foreach (Type type in types)
					{
						try
						{
							if (!type.IsDefined(typeof (PluginAttribute), true)) continue;

							var ctor = type.GetConstructor(new Type[] {});
							if (ctor != null)
							{
								var plugin = ctor.Invoke(new object[] {}) as IPlugin;
								_plugins.Add(plugin);
								LoadCommands(type);
								LoadPacketHandlers(type);
							}
						}
						catch (Exception ex)
						{
							Log.Warn("Plugin loader caught exception, but is moving on.", ex);
						}
					}
				}
			}
		}

		private void LoadCommands(Type type)
		{
			var methods = type.GetMethods();
			foreach (MethodInfo method in methods)
			{
				CommandAttribute commandAttribute = Attribute.GetCustomAttribute(method, typeof (CommandAttribute), false) as CommandAttribute;
				if (commandAttribute == null) continue;

				if (string.IsNullOrEmpty(commandAttribute.Command))
				{
					commandAttribute.Command = method.Name;
				}

				StringBuilder sb = new StringBuilder();
				sb.Append("/");
				sb.Append(commandAttribute.Command);
				var parameters = method.GetParameters();
				if (parameters.Length > 0) sb.Append(" ");
				foreach (var parameter in parameters)
				{
					sb.AppendFormat("<{0}> ", parameter.Name);
				}
				commandAttribute.Usage = sb.ToString().Trim();

				DescriptionAttribute descriptionAttribute = Attribute.GetCustomAttribute(method, typeof (DescriptionAttribute), false) as DescriptionAttribute;
				if (descriptionAttribute != null) commandAttribute.Description = descriptionAttribute.Description;

				_pluginCommands.Add(method, commandAttribute);
			}
		}

		private void LoadPacketHandlers(Type type)
		{
			var methods = type.GetMethods();
			foreach (MethodInfo method in methods)
			{
				{
					PacketHandlerAttribute packetHandlerAttribute = Attribute.GetCustomAttribute(method, typeof (PacketHandlerAttribute), false) as PacketHandlerAttribute;
					if (packetHandlerAttribute != null)
					{
						ParameterInfo[] parameters = method.GetParameters();
						if (parameters.Length < 1) continue;
						if (!parameters[0].ParameterType.IsSubclassOf(typeof (Package))) continue;
						if (packetHandlerAttribute.PacketType == null) packetHandlerAttribute.PacketType = parameters[0].ParameterType;

						if (Attribute.GetCustomAttribute(method, typeof (SendAttribute), false) != null)
						{
							_packetSendHandlerDictionary.Add(method, packetHandlerAttribute);
						}
						else
						{
							_packetHandlerDictionary.Add(method, packetHandlerAttribute);
						}
					}
				}
			}
		}

		internal void EnablePlugins(List<Level> levels)
		{
			foreach (IPlugin plugin in _plugins)
			{
				try
				{
					plugin.OnEnable(new PluginContext(this, levels));
				}
				catch (Exception ex)
				{
					Log.Warn("On enable plugin", ex);
				}
			}
		}

		internal void DisablePlugins()
		{
			foreach (IPlugin plugin in _plugins)
			{
				try
				{
					plugin.OnDisable();
				}
				catch (Exception ex)
				{
					Log.Warn("On disable plugin", ex);
				}
			}
		}

		public void HandleCommand(string message, Player player, bool console = false)
		{
			try
			{
				string commandText = message.Split(' ')[0];
				message = message.Replace(commandText, "").Trim();
				commandText = commandText.Replace("/", "").Replace(".", "");

				string[] arguments = message.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);

				foreach (var handlerEntry in _pluginCommands)
				{
					CommandAttribute commandAttribute = handlerEntry.Value;
					if (!commandText.Equals(commandAttribute.Command, StringComparison.InvariantCultureIgnoreCase)) continue;

					if (!player.Permissions.HasPermission(commandAttribute.Permission) && !player.Permissions.HasPermission("*") && !player.Permissions.IsInGroup(UserGroup.Operator))
					{
						if (!console) player.SendMessage("You are not permitted to use this command!");
						return;
					}

					MethodInfo method = handlerEntry.Key;
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

		internal Package PluginPacketHandler(Package message, bool isReceiveHandler, Player player)
		{
			Package currentPackage = message;

			try
			{
				Dictionary<MethodInfo, PacketHandlerAttribute> packetHandlers;
				if (isReceiveHandler)
				{
					packetHandlers = _packetHandlerDictionary;
				}
				else
				{
					packetHandlers = _packetSendHandlerDictionary;
				}

				foreach (var handler in packetHandlers)
				{
					PacketHandlerAttribute atrib = handler.Value;
					if (atrib.PacketType == null) continue;
					if (atrib.PacketType != currentPackage.GetType()) continue;

					MethodInfo method = handler.Key;
					if (method == null) continue;
					if (method.IsStatic)
					{
						method.Invoke(null, new object[] {currentPackage, player});
					}
					else
					{
						object pluginInstance = _plugins.FirstOrDefault(plugin => plugin.GetType() == method.DeclaringType);
						if (pluginInstance == null) continue;

						if (method.ReturnType == typeof (void))
						{
							ParameterInfo[] parameters = method.GetParameters();
							if (parameters.Length == 1)
							{
								method.Invoke(pluginInstance, new object[] {currentPackage});
							}
							else if (parameters.Length == 2 && parameters[1].ParameterType == typeof (Player))
							{
								method.Invoke(pluginInstance, new object[] {currentPackage, player});
							}
						}
						else
						{
							ParameterInfo[] parameters = method.GetParameters();
							if (parameters.Length == 1)
							{
								currentPackage = method.Invoke(pluginInstance, new object[] {currentPackage}) as Package;
							}
							else if (parameters.Length == 2 && parameters[1].ParameterType == typeof (Player))
							{
								currentPackage = method.Invoke(pluginInstance, new object[] {currentPackage, player}) as Package;
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				//For now we will just ignore this, not to big of a deal.
				//Will have to think a bit more about this later on.
				Log.Warn("Plugin Error: " + ex);
			}

			return currentPackage;
		}
	}
}