using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using log4net;
using MiNET.Net;
using MiNET.Plugins.Attributes;
using MiNET.Utils;
using MiNET.Worlds;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

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

		public CommandSet Commands { get; set; } = new CommandSet();

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

				if (!Directory.Exists(pluginDirectory)) continue;

				_currentPath = pluginDirectory;

				AppDomain currentDomain = AppDomain.CurrentDomain;
				currentDomain.AssemblyResolve += MyResolveEventHandler;

				List<string> pluginPaths = new List<string>();

				pluginPaths.AddRange(Directory.GetFiles(pluginDirectory, "*.dll", SearchOption.AllDirectories));
				pluginPaths.AddRange(Directory.GetFiles(pluginDirectory, "*.exe", SearchOption.AllDirectories));

				foreach (string pluginPath in pluginPaths)
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
								Commands = GenerateCommandSet(_pluginCommands.Keys.ToArray());
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

			DebugPrintCommands();
		}

		public event ResolveEventHandler AssemblyResolve;

		private Assembly MyResolveEventHandler(object sender, ResolveEventArgs args)
		{
			var assembly = AssemblyResolve?.Invoke(sender, args);

			if (assembly != null) return assembly;
			if (_currentPath == null) return null;

			try
			{
				AssemblyName name = new AssemblyName(args.Name);
				string assemblyPath = _currentPath + "\\" + name.Name + ".dll";
				return Assembly.LoadFile(assemblyPath);
			}
			catch (Exception)
			{
				try
				{
					AssemblyName name = new AssemblyName(args.Name);
					string assemblyPath = _currentPath + "\\" + name.Name + ".exe";
					return Assembly.LoadFile(assemblyPath);
				}
				catch (Exception)
				{
					return Assembly.LoadFile(args.Name + ".dll");
				}
			}
		}

		public void LoadCommands(object instance)
		{
			if (!_plugins.Contains(instance)) _plugins.Add(instance);
			LoadCommands(instance.GetType());
			Commands = GenerateCommandSet(_pluginCommands.Keys.ToArray());

			DebugPrintCommands();
		}

		private void DebugPrintCommands()
		{
			return;
			if (!Log.IsDebugEnabled) return;

			var settings = new JsonSerializerSettings();
			settings.NullValueHandling = NullValueHandling.Ignore;
			settings.DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate;
			settings.MissingMemberHandling = MissingMemberHandling.Error;
			settings.Formatting = Formatting.Indented;
			settings.ContractResolver = new CamelCasePropertyNamesContractResolver();

			var content = JsonConvert.SerializeObject(Commands, settings);

			Log.Debug($"Commmands\n{content}");
		}

		public void LoadCommands(Type type)
		{
			var methods = type.GetMethods();
			foreach (MethodInfo method in methods)
			{
				CommandAttribute commandAttribute = Attribute.GetCustomAttribute(method, typeof (CommandAttribute), false) as CommandAttribute;
				if (commandAttribute == null) continue;

				if (string.IsNullOrEmpty(commandAttribute.Name))
				{
					commandAttribute.Name = method.Name;
				}

				DescriptionAttribute descriptionAttribute = Attribute.GetCustomAttribute(method, typeof (DescriptionAttribute), false) as DescriptionAttribute;
				if (descriptionAttribute != null) commandAttribute.Description = descriptionAttribute.Description;

				try
				{
					_pluginCommands.Add(method, commandAttribute);
				}
				catch (ArgumentException e)
				{
					Log.Debug($"Command already exist {method.Name}, {method}", e);
				}
			}
		}

		public static CommandSet GenerateCommandSet(MethodInfo[] methods)
		{
			CommandSet commands = new CommandSet();

			foreach (MethodInfo method in methods)
			{
				CommandAttribute commandAttribute = Attribute.GetCustomAttribute(method, typeof (CommandAttribute), false) as CommandAttribute;
				if (commandAttribute == null) continue;

				AuthorizeAttribute authorizeAttribute = Attribute.GetCustomAttribute(method, typeof (AuthorizeAttribute), false) as AuthorizeAttribute ?? new AuthorizeAttribute();

				if (string.IsNullOrEmpty(commandAttribute.Name))
				{
					commandAttribute.Name = method.Name;
				}

				var overload = new Overload
				{
					Description = commandAttribute.Description??"Bullshit",
					Method = method,
					Input = new Input(),
					Output = new Output()
					{
						FormatStrings = new[]
						{
							new FormatString()
							{
								Format = "{0}"
							},
						},
						Parameters = new[]
						{
							new Parameter
							{
								Name = "result",
								Type = "string"
							},
						}
					}
				};

				string commandName = commandAttribute.Name.ToLowerInvariant();
				if (commands.ContainsKey(commandName))
				{
					Command command = commands[commandName];
					command.Versions.First().Overloads.Add(commandAttribute.Overload ?? Guid.NewGuid().ToString(), overload);
				}
				else
				{
					commands.Add(commandName, new Command
					{
						Name = commandName,
						Versions = new[]
						{
							new Version
							{
								Permission = authorizeAttribute.Permission.ToString().ToLowerInvariant(),
								Aliases = commandAttribute.Aliases,
								Description = commandAttribute.Description??"Bullshit",
								Overloads = new Dictionary<string, Overload>
								{
									{
										"default", overload
									},
								}
							},
						}
					});
				}


				var parameters = method.GetParameters();
				bool isFirstParam = true;
				List<Parameter> inputParams = new List<Parameter>();
				foreach (var parameter in parameters)
				{
					if (isFirstParam && typeof (Player).IsAssignableFrom(parameter.ParameterType))
					{
						continue;
					}
					isFirstParam = false;

					Parameter param = new Parameter();
					param.Name = ToCamelCase(parameter.Name);
					param.Type = GetParameterType(parameter);
					param.Optional = parameter.IsOptional;
					if (param.Type.Equals("stringenum"))
					{
						if (parameter.ParameterType.IsEnum)
						{
							param.EnumValues = parameter.ParameterType.GetEnumNames().Select(s => s.ToLowerInvariant()).ToArray();
						}
						else
						{
							string typeName = parameter.ParameterType.Name;
							typeName = typeName.Replace("Enum", "");
							typeName = typeName.ToLowerInvariant()[0] + typeName.Substring(1);
							param.EnumType = typeName;
						}
					}
					inputParams.Add(param);
				}

				if (inputParams.Count == 0)
				{
					overload.Input.Parameters = null;
				}
				else
				{
					overload.Input.Parameters = inputParams.ToArray();
				}

				// Output objects
				if (method.ReturnType != typeof (void))
				{
					var properties = method.ReturnType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
					List<Parameter> outputParams = new List<Parameter>();
					foreach (PropertyInfo property in properties)
					{
						if (property.Name.Equals("StatusCode")) continue;
						if (property.Name.Equals("SuccessCount")) continue;

						Parameter param = new Parameter();
						param.Name = ToCamelCase(property.Name);
						param.Type = GetPropertyType(property);
						outputParams.Add(param);
					}

					overload.Output.Parameters = outputParams.ToArray();
				}

				if (commandAttribute.OutputFormatStrings != null)
				{
					overload.Output.FormatStrings = new FormatString[commandAttribute.OutputFormatStrings.Length];
					int i = 0;
					foreach (var formatString in commandAttribute.OutputFormatStrings)
					{
						overload.Output.FormatStrings[i] = new FormatString() {Format = commandAttribute.OutputFormatStrings[i]};
						i++;
					}
				}
			}

			return commands;
		}

		private static string ToCamelCase(string s)
		{
			if (string.IsNullOrEmpty(s) || !char.IsUpper(s[0]))
				return s;
			char[] chArray = s.ToCharArray();
			for (int index = 0; index < chArray.Length && (index != 1 || char.IsUpper(chArray[index])); ++index)
			{
				bool flag = index + 1 < chArray.Length;
				if (!(index > 0 & flag) || char.IsUpper(chArray[index + 1]))
				{
					char ch = char.ToLower(chArray[index], CultureInfo.InvariantCulture);
					chArray[index] = ch;
				}
				else
					break;
			}
			return new string(chArray);
		}


		private static string GetPropertyType(PropertyInfo parameter)
		{
			string value = parameter.PropertyType.ToString();

			if (parameter.PropertyType == typeof (int))
				value = "int";
			else if (parameter.PropertyType == typeof (short))
				value = "int";
			else if (parameter.PropertyType == typeof (byte))
				value = "int";
			else if (parameter.PropertyType == typeof (bool))
				value = "bool";
			else if (parameter.PropertyType == typeof (string))
				value = "string";
			else if (parameter.PropertyType == typeof (string[]))
				value = "rawtext";
			else
			{
				Log.Warn("No property type mapping for type: " + parameter.PropertyType.ToString());
			}

			return value;
		}

		private static string GetParameterType(ParameterInfo parameter)
		{
			string value = parameter.ParameterType.ToString();

			if (parameter.ParameterType == typeof (int))
				value = "int";
			else if (parameter.ParameterType == typeof (short))
				value = "int";
			else if (parameter.ParameterType == typeof (byte))
				value = "int";
			else if (parameter.ParameterType == typeof (bool))
				value = "bool";
			else if (parameter.ParameterType == typeof (string))
				value = "string";
			else if (parameter.ParameterType == typeof (string[]))
				value = "rawtext";
			else if (parameter.ParameterType == typeof (Target))
				value = "target";
			else if (parameter.ParameterType == typeof (BlockPos))
				value = "blockpos";
			else if (parameter.ParameterType.IsEnum)
				value = "stringenum";
			else if (parameter.ParameterType.BaseType == typeof (EnumBase))
				value = "stringenum";
			else if (typeof (IParameterSerializer).IsAssignableFrom(parameter.ParameterType))
				// Custom serialization
				value = "string";
			else
				Log.Warn("No parameter type mapping for type: " + parameter.ParameterType.ToString());

			return value;
		}

		public void UnloadCommands(object instance)
		{
			if (!_plugins.Contains(instance)) return;
			_plugins.Remove(instance);

			var methods = _pluginCommands.Keys.Where(info => info.DeclaringType == instance.GetType()).ToArray();
			foreach (var method in methods)
			{
				_pluginCommands.Remove(method);
			}

			Commands = GenerateCommandSet(_pluginCommands.Keys.ToArray());
		}

		public void LoadPacketHandlers(object instance)
		{
			if (!_plugins.Contains(instance)) _plugins.Add(instance);
			LoadPacketHandlers(instance.GetType());
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
						if (!typeof (Package).IsAssignableFrom(parameters[0].ParameterType)) continue;
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

		public void UnloadPacketHandlers(object instance)
		{
			//if (!_plugins.Contains(instance)) return;
			//_plugins.Remove(instance);

			var methods = _packetHandlerDictionary.Keys.Where(info => info.DeclaringType == instance.GetType()).ToArray();
			foreach (var method in methods)
			{
				_packetHandlerDictionary.Remove(method);
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
			foreach (object plugin in _plugins.ToArray())
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

		public object HandleCommand(Player player, string commandName, string commandOverload, dynamic commandInputJson)
		{
			Log.Debug($"HandleCommand {commandName}");

			try
			{
				Command command = null;
				if (Commands.ContainsKey(commandName))
				{
					command = Commands[commandName];
				}
				else
				{
					command = Commands.Values.FirstOrDefault(cmd => cmd.Versions.Any(version => version.Aliases != null && version.Aliases.Any(s => s == commandName)));
				}

				if (command == null) return null;

				Overload overload = command.Versions.First().Overloads[commandOverload];

				UserPermission requiredPermission = (UserPermission) Enum.Parse(typeof (UserPermission), command.Versions.First().Permission, true);
				if (player.PermissionLevel < requiredPermission)
				{
					Log.Debug($"Insufficient permissions. Require {requiredPermission} but player had {player.PermissionLevel}");
					return null;
				}

				MethodInfo method = overload.Method;

				List<string> strings = new List<string>();
				if (commandInputJson != null)
				{
					foreach (ParameterInfo parameter in method.GetParameters())
					{
						if (typeof (Player).IsAssignableFrom(parameter.ParameterType)) continue;

						if (HasProperty(commandInputJson, parameter.Name))
						{
							Log.Debug($"Parameter: {commandInputJson[ToCamelCase(parameter.Name)].ToString()}");
							strings.Add(commandInputJson[ToCamelCase(parameter.Name)].ToString());
						}
					}
				}

				return ExecuteCommand(method, player, strings.ToArray());
			}
			catch (Exception e)
			{
				Log.Error("Handle JSON command", e);
			}

			return null;
		}

		public static bool HasProperty(dynamic obj, string name)
		{
			JObject tobj = obj;
			return tobj.Property(name) != null;
		}

		private static bool IsParams(ParameterInfo param)
		{
			return Attribute.IsDefined(param, typeof (ParamArrayAttribute));
		}

		private object ExecuteCommand(MethodInfo method, Player player, string[] args)
		{
			Log.Info($"Execute command {method}");

			var parameters = method.GetParameters();

			int addLenght = 0;
			if (parameters.Length > 0 && typeof (Player).IsAssignableFrom(parameters[0].ParameterType))
			{
				addLenght = 1;
			}

			object[] objectArgs = new object[parameters.Length];

			for (int k = 0; k < parameters.Length; k++)
			{
				var parameter = parameters[k];
				int i = k - addLenght;
				if (k == 0 && addLenght == 1)
				{
					if (typeof (Player).IsAssignableFrom(parameter.ParameterType))
					{
						objectArgs[k] = player;
						continue;
					}
					Log.WarnFormat("Command method {0} missing Player as first argument.", method.Name);
					return null;
				}

				if (parameter.IsOptional && args.Length <= i)
				{
					objectArgs[k] = parameter.DefaultValue;
					continue;
				}

				if (typeof (IParameterSerializer).IsAssignableFrom(parameter.ParameterType))
				{
					var ctor = parameter.ParameterType.GetConstructor(Type.EmptyTypes);
					IParameterSerializer defaultValue = ctor.Invoke(null) as IParameterSerializer;
					defaultValue?.Deserialize(player, args[i]);

					objectArgs[k] = defaultValue;

					continue;
				}

				if (parameter.ParameterType.BaseType == typeof (EnumBase))
				{
					var ctor = parameter.ParameterType.GetConstructor(Type.EmptyTypes);
					EnumBase instance = (EnumBase) ctor.Invoke(null);
					instance.Value = args[i];
					objectArgs[k] = instance;
					continue;
				}

				if (parameter.ParameterType == typeof (Target))
				{
					var target = JsonConvert.DeserializeObject<Target>(args[i]);
					target = FillTargets(player, player.Level, target);
					objectArgs[k] = target;
					continue;
				}

				if (parameter.ParameterType == typeof (BlockPos))
				{
					var blockpos = JsonConvert.DeserializeObject<BlockPos>(args[i]);
					objectArgs[k] = blockpos;
					continue;
				}

				if (parameter.ParameterType == typeof (string))
				{
					objectArgs[k] = args[i];
					continue;
				}
				if (parameter.ParameterType == typeof (byte))
				{
					byte value;
					if (!byte.TryParse(args[i], out value)) return null;
					objectArgs[k] = value;
					continue;
				}
				if (parameter.ParameterType == typeof (short))
				{
					short value;
					if (!short.TryParse(args[i], out value)) return null;
					objectArgs[k] = value;
					continue;
				}
				if (parameter.ParameterType == typeof (int))
				{
					int value;
					if (!int.TryParse(args[i], out value)) return null;
					objectArgs[k] = value;
					continue;
				}
				if (parameter.ParameterType == typeof (bool))
				{
					bool value;
					if (!bool.TryParse(args[i], out value)) return null;
					objectArgs[k] = value;
					continue;
				}
				if (parameter.ParameterType == typeof (float))
				{
					float value;
					if (!float.TryParse(args[i], out value)) return null;
					objectArgs[k] = value;
					continue;
				}
				if (parameter.ParameterType == typeof (double))
				{
					double value;
					if (!double.TryParse(args[i], out value)) return null;
					objectArgs[k] = value;
					continue;
				}
				if (parameter.ParameterType.IsEnum)
				{
					Enum value = Enum.Parse(parameter.ParameterType, args[i], true) as Enum;
					if (value == null)
					{
						Log.Error($"Could not convert to valid enum value: {args[i]}");
						continue;
					}

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

				return null;
			}

			object result = null;

			try
			{
				object pluginInstance = _plugins.FirstOrDefault(plugin => plugin.GetType() == method.DeclaringType);
				if (pluginInstance == null) return null;

				ICommandFilter filter = pluginInstance as ICommandFilter;
				if (filter != null)
				{
					filter.OnCommandExecuting(player);
				}

				if (method.IsStatic)
				{
					result = method.Invoke(null, objectArgs);
				}
				else
				{
					if (method.DeclaringType == null) return false;

					Plugin.CurrentPlayer = player; // Setting thread local for call
					result = method.Invoke(pluginInstance, objectArgs);
					Plugin.CurrentPlayer = null; // Done with thread local, we using pool to make sure it's reset.
				}

				if (filter != null)
				{
					filter.OnCommandExecuted();
				}
			}
			catch (Exception e)
			{
				Log.Error($"Error while executing command {method}", e);
			}
			return result;
		}

		private Target FillTargets(Player commander, Level level, Target target)
		{
			if (target.Selector == "nearestPlayer" && target.Rules == null)
			{
				target.Players = new[] {commander};
			}
			else if (target.Selector == "nearestPlayer" && target.Rules != null)
			{
				string username = target.Rules.First().Value;
				var players = level.GetAllPlayers().Where(p => p.Username == username);
				target.Players = players.ToArray();
			}
			else if (target.Selector == "allPlayers")
			{
				target.Players = level.GetAllPlayers();
			}
			else if (target.Selector == "allEntities")
			{
				target.Entities = level.GetEntites();
			}
			else if (target.Selector == "randomPlayer")
			{
				Player[] players = level.GetAllPlayers();
				target.Players = new[] {players[new Random().Next(players.Length)]};
			}

			return target;
		}

		internal Package PluginPacketHandler(Package message, bool isReceiveHandler, Player player)
		{
			if (message == null) return null;

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

					if (!atrib.PacketType.IsInstanceOfType(currentPackage) && atrib.PacketType != currentPackage.GetType())
					{
						//Log.Warn($"No assignable {atrib.PacketType.Name} from {currentPackage.GetType().Name}");
						continue;
					}

					//Log.Warn($"IS assignable {atrib.PacketType.Name} from {currentPackage.GetType().Name}");

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
							else if (parameters.Length == 2 && typeof (Player).IsAssignableFrom(parameters[1].ParameterType))
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
							else if (parameters.Length == 2 && typeof (Player).IsAssignableFrom(parameters[1].ParameterType))
							{
								returnPacket = method.Invoke(pluginInstance, new object[] {currentPackage, player}) as Package;
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

		public static string GetUsage(Command command, bool includeDescription = false, string prepend = "", string postpend = "")
		{
			StringBuilder sb = new StringBuilder();
			bool isFirst = true;
			foreach (var overload in command.Versions.First().Overloads.Values)
			{
				if (!isFirst) sb.Append("\n");
				isFirst = false;

				sb.Append(prepend);
				sb.Append("/");
				sb.Append(command.Name);
				sb.Append(" ");

				if (overload.Input.Parameters != null)
				{
					foreach (var parameter in overload.Input.Parameters)
					{
						sb.Append(parameter.Optional ? "[" : "<");
						sb.Append(parameter.Name);
						sb.Append(": ");
						sb.Append(parameter.Type);
						sb.Append(parameter.Optional ? "]" : ">");
						sb.Append(" ");
					}
				}
				sb.Append(ChatFormatting.Reset);
				if (includeDescription && !string.IsNullOrEmpty(overload.Description)) sb.Append($" - {overload.Description}");
				sb.Append(postpend);
			}

			return sb.ToString();
		}
	}
}