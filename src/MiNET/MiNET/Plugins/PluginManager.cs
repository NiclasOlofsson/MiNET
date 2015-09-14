using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using log4net;
using Microsoft.AspNet.Identity;
using MiNET.Net;
using MiNET.Plugins.Attributes;
using MiNET.Security;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Plugins
{
	public class PluginManager
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (MiNetServer));

		private readonly List<object> _plugins = new List<object>();
		private readonly Dictionary<MethodInfo, PacketHandlerAttribute> _packetHandlerDictionary = new Dictionary<MethodInfo, PacketHandlerAttribute>();
		private readonly Dictionary<MethodInfo, PacketHandlerAttribute> _packetSendHandlerDictionary = new Dictionary<MethodInfo, PacketHandlerAttribute>();
		private readonly Dictionary<MethodInfo, CommandAttribute> _pluginCommands = new Dictionary<MethodInfo, CommandAttribute>();

		public List<object> Plugins
		{
			get { return _plugins; }
		}

		public List<CommandAttribute> PluginCommands
		{
			get { return _pluginCommands.Values.ToList(); }
		}

		private string _currentPath = null;

		internal void LoadPlugins()
		{
			if (Config.GetProperty("PluginDisabled", false)) return;

			// Default it is the directory we are executing, and below.
			string pluginDirectoryPaths = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
			pluginDirectoryPaths = Config.GetProperty("PluginDirectory", pluginDirectoryPaths);
			//HACK: Make it possible to define multiple PATH;PATH;PATH

			foreach (string dirPath in pluginDirectoryPaths.Split(new char[] {';'}, StringSplitOptions.RemoveEmptyEntries))
			{
				if (dirPath == null) continue;

				string pluginDirectory = Path.GetFullPath(dirPath);

				_currentPath = pluginDirectory;

				AppDomain currentDomain = AppDomain.CurrentDomain;
				currentDomain.AssemblyResolve += MyResolveEventHandler;

				foreach (string pluginPath in Directory.GetFiles(pluginDirectory, "*.dll", SearchOption.AllDirectories))
				{
					Assembly newAssembly = Assembly.LoadFile(pluginPath);

					Type[] types = newAssembly.GetExportedTypes();
					foreach (Type type in types)
					{
						try
						{
							// If no PluginAttribute and does not implement IPlugin interface, not a valid plugin
							if (!type.IsDefined(typeof (PluginAttribute), true) && !typeof (IPlugin).IsAssignableFrom(type)) continue;
							if (type.IsDefined(typeof (PluginAttribute), true))
							{
								PluginAttribute pluginAttribute = Attribute.GetCustomAttribute(type, typeof (PluginAttribute), true) as PluginAttribute;
								if (pluginAttribute != null)
								{
									if (!Config.GetProperty(pluginAttribute.PluginName + ".Enabled", true)) continue;
								}
							}
							var ctor = type.GetConstructor(Type.EmptyTypes);
							if (ctor != null)
							{
								var plugin = ctor.Invoke(null);
								_plugins.Add(plugin);
								LoadCommands(type);
								LoadPacketHandlers(type);
							}
						}
						catch (Exception ex)
						{
							Log.WarnFormat("Failed loading plugin type {0} as a plugin.", type);
							Log.Debug("Plugin loader caught exception, but is moving on.", ex);
						}
					}
				}
			}
		}

		private Assembly MyResolveEventHandler(object sender, ResolveEventArgs args)
		{
			if (_currentPath == null) return null;

			try
			{
				AssemblyName name = new AssemblyName(args.Name);
				string assemblyPath = _currentPath + "\\" + name.Name + ".dll";
				return Assembly.LoadFile(assemblyPath);
			}
			catch (Exception)
			{
				return Assembly.LoadFile(args.Name + ".dll");
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

		internal void ExecuteStartup(MiNetServer server)
		{
			foreach (object plugin in _plugins)
			{
				IStartup startupClass = plugin as IStartup;
				if (startupClass == null) continue;

				try
				{
					startupClass.Configure(server);
				}
				catch (Exception ex)
				{
					Log.Warn("Execute Startup class failed", ex);
				}
			}
		}

		internal void EnablePlugins(MiNetServer server, LevelManager levelManager)
		{
			foreach (object plugin in _plugins)
			{
				IPlugin enablingPlugin = plugin as IPlugin;
				if (enablingPlugin == null) continue;

				try
				{
					enablingPlugin.OnEnable(new PluginContext(server, this, levelManager));
				}
				catch (Exception ex)
				{
					Log.Warn("On enable plugin", ex);
				}
			}
		}

		internal void DisablePlugins()
		{
			foreach (object plugin in _plugins)
			{
				IPlugin enablingPlugin = plugin as IPlugin;
				if (enablingPlugin == null) continue;

				try
				{
					enablingPlugin.OnDisable();
				}
				catch (Exception ex)
				{
					Log.Warn("On disable plugin", ex);
				}
			}
		}

		public void HandleCommand(UserManager<User> userManager, string message, Player player)
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

					MethodInfo method = handlerEntry.Key;
					if (method == null) return;

					var authorizationAttributes = method.GetCustomAttributes<AuthorizeAttribute>(true);
					foreach (AuthorizeAttribute authorizationAttribute in authorizationAttributes)
					{
						User user = userManager.FindByName(player.Username);
						var userIdentity = userManager.CreateIdentity(user, "none");
						if (!authorizationAttribute.OnAuthorization(new GenericPrincipal(userIdentity, new string[0])))
						{
							player.SendMessage("You are not permitted to use this command!");
							return;
						}
					}

					if (ExecuteCommand(method, player, arguments)) return;
				}
			}
			catch (Exception ex)
			{
				Log.Warn(ex);
			}
		}

		private static bool IsParams(ParameterInfo param)
		{
			return Attribute.IsDefined(param, typeof (ParamArrayAttribute));
		}

		private bool ExecuteCommand(MethodInfo method, Player player, string[] args)
		{
			var parameters = method.GetParameters();

			int addLenght = 0;
			if (parameters.Length > 0 && parameters[0].ParameterType == typeof (Player))
			{
				addLenght = 1;
			}

			if (IsParams(parameters.Last()))
			{
				// method params ex: int int params int[] 
				// input ex:           1  1  1 1 1 
				// so arguments in must be at least the lenght of method arguments
				if (parameters.Length > args.Length + addLenght) return false;
			}
			else
			{
				if (parameters.Length != args.Length + addLenght) return false;
			}

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
				if (parameter.ParameterType == typeof (byte))
				{
					byte value;
					if (!byte.TryParse(args[i], out value)) return false;
					objectArgs[k] = value;
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

				if (IsParams(parameter) && parameter.ParameterType == typeof (string[]))
				{
					List<string> strings = new List<string>();
					for (int j = i; j < args.Length; j++)
					{
						strings.Add(args[j]);
					}
					objectArgs[k] = strings.ToArray();
					continue;
				}

				return false;
			}

			object pluginInstance = _plugins.FirstOrDefault(plugin => plugin.GetType() == method.DeclaringType);
			if (pluginInstance == null) return false;

			if (method.IsStatic)
			{
				method.Invoke(null, objectArgs);
			}
			else
			{
				if (method.DeclaringType == null) return false;

				method.Invoke(pluginInstance, objectArgs);
			}

			return true;
		}

		internal Package PluginPacketHandler(Package message, bool isReceiveHandler, Player player)
		{
			Package currentPackage = message;
			Package returnPacket = currentPackage;

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

				if (packetHandlers == null) return message;

				foreach (var handler in packetHandlers)
				{
					if (handler.Value == null) continue;
					if (handler.Key == null) continue;

					PacketHandlerAttribute atrib = handler.Value;
					if (atrib.PacketType == null) continue;
					if (currentPackage != null && atrib.PacketType != currentPackage.GetType()) continue;

					MethodInfo method = handler.Key;
					if (method == null) continue;
					if (method.IsStatic)
					{
						//TODO: Move below and set pluginInstance = null instead
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
								returnPacket = method.Invoke(pluginInstance, new object[] {currentPackage}) as Package;
							}
							else if (parameters.Length == 2 && parameters[1].ParameterType == typeof (Player))
							{
								returnPacket = method.Invoke(pluginInstance, new object[] { currentPackage, player }) as Package;
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				//For now we will just ignore this, not to big of a deal.
				//Will have to think a bit more about this later on.
				Log.Warn("Plugin Error: ", ex);
				Log.Warn("Plugin Error: ", ex.InnerException);
			}

			return returnPacket;
		}
	}
}