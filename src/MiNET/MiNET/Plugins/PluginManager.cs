#region LICENSE

// The contents of this file are subject to the Common Public Attribution
// License Version 1.0. (the "License"); you may not use this file except in
// compliance with the License. You may obtain a copy of the License at
// https://github.com/NiclasOlofsson/MiNET/blob/master/LICENSE.
// The License is based on the Mozilla Public License Version 1.1, but Sections 14
// and 15 have been added to cover use of software over a computer network and
// provide for limited attribution for the Original Developer. In addition, Exhibit A has
// been modified to be consistent with Exhibit B.
// 
// Software distributed under the License is distributed on an "AS IS" basis,
// WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License for
// the specific language governing rights and limitations under the License.
// 
// The Original Code is MiNET.
// 
// The Original Developer is the Initial Developer.  The Initial Developer of
// the Original Code is Niclas Olofsson.
// 
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2020 Niclas Olofsson.
// All Rights Reserved.

#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using log4net;
using MiNET.Net;
using MiNET.Plugins.Attributes;
using MiNET.Utils;
using MiNET.Worlds;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

[assembly: InternalsVisibleTo("MiNETTests")]
[assembly: InternalsVisibleTo("MiNET.Benchmarks")]
namespace MiNET.Plugins
{
	public class PluginManager
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(MiNetServer));

		private readonly List<object> _plugins = new List<object>();
		private readonly Dictionary<MethodInfo, PacketHandlerAttribute> _packetHandlerDictionary = new Dictionary<MethodInfo, PacketHandlerAttribute>();
		private readonly Dictionary<MethodInfo, PacketHandlerAttribute> _packetSendHandlerDictionary = new Dictionary<MethodInfo, PacketHandlerAttribute>();
		private readonly Dictionary<MethodInfo, CommandAttribute> _pluginCommands = new Dictionary<MethodInfo, CommandAttribute>();

		/// <summary>
		///     Routes in registration order, which is also match order: the first template that fits
		///     wins, so a plugin loaded earlier keeps its path. Read on signaling threads and written
		///     on load, hence the copy-on-write list rather than a lock on every request.
		/// </summary>
		private volatile List<HttpRoute> _httpRoutes = new List<HttpRoute>();

		public List<object> Plugins
		{
			get { return _plugins; }
		}

		public CommandSet Commands { get; set; } = new CommandSet();

		private string _currentPath = null;

		public PluginManager()
		{
			
		}

		internal void LoadPlugins()
		{
			if (Config.GetProperty("PluginDisabled", false)) return;

			// Default it is the directory we are executing, and below.
			string pluginDirectoryPaths = Path.GetDirectoryName(new Uri(Assembly.GetEntryAssembly().Location).LocalPath);
			pluginDirectoryPaths = Config.GetProperty("PluginDirectory", pluginDirectoryPaths);
			//HACK: Make it possible to define multiple PATH;PATH;PATH

			foreach (string dirPath in pluginDirectoryPaths.Split(new char[] {';'}, StringSplitOptions.RemoveEmptyEntries))
			{
				if (dirPath == null) continue;

				string pluginDirectory = Path.GetFullPath(dirPath);

				Log.Debug($"Looking for plugin assemblies in directory {pluginDirectory}");

				if (!Directory.Exists(pluginDirectory)) continue;

				_currentPath = pluginDirectory;

				AppDomain currentDomain = AppDomain.CurrentDomain;
				currentDomain.AssemblyResolve += MyResolveEventHandler;

				List<string> pluginPaths = new List<string>();

				pluginPaths.AddRange(Directory.GetFiles(pluginDirectory, "*.dll", SearchOption.AllDirectories));
				pluginPaths.AddRange(Directory.GetFiles(pluginDirectory, "*.exe", SearchOption.AllDirectories));
				pluginPaths.ForEach(path => Log.Debug($"Looking for plugins in assembly {path}"));

				foreach (string pluginPath in pluginPaths)
				{
					try
					{
						Assembly newAssembly = Assembly.LoadFile(pluginPath);

						try
						{
							Type[] types = newAssembly.GetExportedTypes();
							foreach (Type type in types)
							{
								try
								{
									// If no PluginAttribute and does not implement IPlugin interface, not a valid plugin
									if (!type.IsDefined(typeof(PluginAttribute), true) && !typeof(IPlugin).IsAssignableFrom(type)) continue;

									// If plugin is already loaded don't load it again
									if (_plugins.Any(l => l.GetType().AssemblyQualifiedName == type.AssemblyQualifiedName))
									{
										Log.Error($"Tried to load duplicate plugin: {type}");
										continue;
									}

									if (type.IsDefined(typeof(PluginAttribute), true))
									{
										PluginAttribute pluginAttribute = Attribute.GetCustomAttribute(type, typeof(PluginAttribute), true) as PluginAttribute;
										if (pluginAttribute != null)
										{
											if (!Config.GetProperty(pluginAttribute.PluginName + ".Enabled", true)) continue;
										}
									}
									var ctor = type.GetConstructor(Type.EmptyTypes);
									if (ctor != null)
									{
										var plugin = ctor.Invoke(null);
										LoadPlugin(plugin, type);
									}
								}
								catch (Exception ex)
								{
									Log.WarnFormat("Failed loading plugin type {0} as a plugin.", type);
									Log.Debug("Plugin loader caught exception, but is moving on.", ex);
								}
							}
						}
						catch (Exception e)
						{
							Log.WarnFormat("Failed loading exported types for assembly {0} as a plugin.", newAssembly.FullName);
							Log.Debug("Plugin loader caught exception, but is moving on.", e);
						}
					}
					catch (Exception e)
					{
						Log.Warn($"Failed loading assembly at path \"{pluginPath}\"");
						Log.Debug("Plugin loader caught exception, but is moving on.", e);
					}
				}
			}

			DebugPrintCommands();
		}

		public void LoadPlugin(object plugin)
		{
			Type type = plugin.GetType();

			if (_plugins.Any(l => l.GetType().AssemblyQualifiedName == type.AssemblyQualifiedName))
			{
				Log.Error($"Tried to load duplicate plugin: {type}");
				return;
			}

			if (type.IsDefined(typeof(PluginAttribute), true))
			{
				PluginAttribute pluginAttribute = Attribute.GetCustomAttribute(type, typeof(PluginAttribute), true) as PluginAttribute;
				if (pluginAttribute != null)
				{
					if (!Config.GetProperty(pluginAttribute.PluginName + ".Enabled", true))
						return;
				}
			}

			LoadPlugin(plugin, type);
		}

		private void LoadPlugin(object plugin, Type type)
		{
			_plugins.Add(plugin);
			LoadCommands(type);
			Commands = GenerateCommandSet(_pluginCommands.Keys.ToArray());
			LoadPacketHandlers(type);
			LoadHttpHandlers(type);
			Log.Debug($"Loaded plugin {type}");
		}

		public event ResolveEventHandler AssemblyResolve;

		private Assembly MyResolveEventHandler(object sender, ResolveEventArgs args)
		{
			var assembly = AssemblyResolve?.Invoke(sender, args);

			if (assembly != null) return assembly;
			if (_currentPath == null) return null;

			if (TryLoadAssembly(_currentPath ?? "", args.Name, out assembly))
			{
				return assembly;
			}
			
			Log.Warn($"Could not resolve assembly: {args.Name}");

			return null;
		}

		private bool TryLoadAssembly(string path, string input, out Assembly assembly)
		{
			try
			{
				AssemblyName name = new AssemblyName(input);
				string dllPath = Path.Combine(path, $"{name.Name}.dll");
				string exePath = Path.Combine(path, $"{name.Name}.exe");

				if (File.Exists(dllPath))
				{
					assembly = Assembly.LoadFile(dllPath);

					return true;
				}
				else if (File.Exists(exePath))
				{
					assembly = Assembly.LoadFile(exePath);

					return true;
				}
			}
			catch (Exception ex)
			{
				Log.Warn($"Could not load assembly: {input}", ex);
			}

			assembly = null;
			return false;
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

		/// <summary>One <c>[HttpHandler]</c>, with its template pre-split into segments.</summary>
		private class HttpRoute
		{
			public string Method { get; init; }
			public string[] Template { get; init; }
			public MethodInfo Handler { get; init; }

			public bool TryMatch(string method, string path, out Dictionary<string, string> values)
			{
				values = null;
				if (!string.Equals(method, Method, StringComparison.OrdinalIgnoreCase)) return false;

				string[] segments = SplitPath(path);
				if (segments.Length != Template.Length) return false;

				for (int i = 0; i < Template.Length; i++)
				{
					string expected = Template[i];

					if (expected.Length > 1 && expected[0] == '{' && expected[^1] == '}')
					{
						values ??= new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
						values[expected[1..^1]] = Uri.UnescapeDataString(segments[i]);
						continue;
					}

					if (!string.Equals(expected, segments[i], StringComparison.OrdinalIgnoreCase)) return false;
				}

				values ??= new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
				return true;
			}
		}

		private static string[] SplitPath(string path) => (path ?? "").Split('/', StringSplitOptions.RemoveEmptyEntries);

		/// <summary>
		///     Registers every <c>[HttpHandler]</c> on the type. A method whose shape is wrong is
		///     reported and skipped rather than left to fail on the first request that reaches it,
		///     because that request comes from outside and its author is not watching this log.
		/// </summary>
		public void LoadHttpHandlers(Type type)
		{
			var routes = new List<HttpRoute>(_httpRoutes);

			foreach (MethodInfo method in type.GetMethods())
			{
				foreach (HttpHandlerAttribute attribute in method.GetCustomAttributes<HttpHandlerAttribute>(false))
				{
					ParameterInfo[] parameters = method.GetParameters();
					if (method.ReturnType != typeof(HttpResponse) || parameters.Length != 1 || parameters[0].ParameterType != typeof(HttpRequest))
					{
						Log.Warn($"Ignoring [HttpHandler] on {type.Name}.{method.Name}: it must be HttpResponse {method.Name}(HttpRequest request)");
						continue;
					}

					var route = new HttpRoute
					{
						Method = attribute.Method,
						Template = SplitPath(attribute.Path),
						Handler = method
					};

					routes.Add(route);
					Log.Info($"Plugin route {attribute.Method.ToUpperInvariant()} {attribute.Path} -> {type.Name}.{method.Name}");
				}
			}

			_httpRoutes = routes;
		}

		/// <summary>
		///     Runs the first plugin route matching the request, or returns null when none does so the
		///     caller can answer 404 itself. Runs on the connection's own thread, never the tick.
		/// </summary>
		public HttpResponse HandleHttpRequest(HttpRequest request)
		{
			foreach (HttpRoute route in _httpRoutes)
			{
				if (!route.TryMatch(request.Method, request.Path, out Dictionary<string, string> values)) continue;

				object instance = _plugins.FirstOrDefault(plugin => route.Handler.DeclaringType != null && route.Handler.DeclaringType.IsInstanceOfType(plugin));
				if (instance == null && !route.Handler.IsStatic)
				{
					Log.Warn($"Plugin route {request.Method} {request.Path} has no live instance of {route.Handler.DeclaringType?.Name}");
					return HttpResponse.Empty(500);
				}

				request.RouteValues = values;

				try
				{
					return (HttpResponse) route.Handler.Invoke(route.Handler.IsStatic ? null : instance, new object[] {request}) ?? HttpResponse.Empty(204);
				}
				catch (Exception e)
				{
					// The handler is a plugin's, so its failure is its own. It must not reach the
					// accept loop, where it would drop a connection the client cannot retry.
					Log.Error($"Plugin route {request.Method} {request.Path} threw", e.InnerException ?? e);
					return HttpResponse.Empty(500);
				}
			}

			return null;
		}

		public void LoadCommands(Type type)
		{
			var methods = type.GetMethods();
			foreach (MethodInfo method in methods)
			{
				CommandAttribute commandAttribute = Attribute.GetCustomAttribute(method, typeof(CommandAttribute), false) as CommandAttribute;
				if (commandAttribute == null) continue;

				if (string.IsNullOrEmpty(commandAttribute.Name))
				{
					commandAttribute.Name = method.Name;
				}

				commandAttribute.Description = GetDescription(method, commandAttribute);

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

		/// <summary>
		///     A command is documented either on its [Command] or as its own [Description], and both
		///     are read here so the two callers cannot answer differently. Empty when undocumented,
		///     never a placeholder: GetUsage omits the description only when it is empty, so anything
		///     else is printed to the player as if it were documentation.
		/// </summary>
		private static string GetDescription(MethodInfo method, CommandAttribute commandAttribute)
		{
			if (!string.IsNullOrEmpty(commandAttribute.Description)) return commandAttribute.Description;

			var descriptionAttribute = Attribute.GetCustomAttribute(method, typeof(DescriptionAttribute), false) as DescriptionAttribute;

			return descriptionAttribute?.Description ?? "";
		}

		public static CommandSet GenerateCommandSet(MethodInfo[] methods)
		{
			CommandSet commands = new CommandSet();

			foreach (MethodInfo method in methods)
			{
				CommandAttribute commandAttribute = Attribute.GetCustomAttribute(method, typeof(CommandAttribute), false) as CommandAttribute;
				if (commandAttribute == null) continue;

				AuthorizeAttribute authorizeAttribute = Attribute.GetCustomAttribute(method, typeof(AuthorizeAttribute), false) as AuthorizeAttribute ?? new AuthorizeAttribute();

				if (string.IsNullOrEmpty(commandAttribute.Name))
				{
					commandAttribute.Name = method.Name;
				}

				string description = GetDescription(method, commandAttribute);

				var overload = new Overload
				{
					Description = description,
					Method = method,
					Input = new Input(),
				};

				string commandName = commandAttribute.Name.ToLowerInvariant();
				var split = commandName.Split(' ');
				Parameter subCommmandParam = null;
				if (split.Length > 1)
				{
					subCommmandParam = new Parameter();
					subCommmandParam.Name = "subcommand";
					subCommmandParam.Type = "stringenum";
					subCommmandParam.EnumType = "SubCommand" + commandName.Replace(" ", "-");
					subCommmandParam.EnumValues = new[] {split[1]};
					commandName = split[0];
				}
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
								CommandPermission = authorizeAttribute.Permission,
								ErrorMessage = authorizeAttribute.ErrorMessage,
								Aliases = commandAttribute.Aliases ?? new string[0],
								Description = description,
								Overloads = new Dictionary<string, Overload>
								{
									{"default", overload},
								}
							},
						}
					});
				}


				List<Parameter> inputParams = new List<Parameter>();
				if (subCommmandParam != null)
				{
					inputParams.Add(subCommmandParam);
				}

				var parameters = method.GetParameters();
				bool isFirstParam = true;
				foreach (ParameterInfo parameter in parameters)
				{
					if (isFirstParam && typeof(Player).IsAssignableFrom(parameter.ParameterType)) continue;
					isFirstParam = false;

					var param = new Parameter();
					param.Name = ToCamelCase(parameter.Name);
					param.Type = GetParameterType(parameter);
					param.Optional = parameter.IsOptional;

					if (param.Type.Equals("bool"))
					{
						param.Type = "stringenum";
						param.EnumType = "bool";
						param.EnumValues = new string[] {"false", "true"};
					}
					else if (param.Type.Equals("softenum"))
					{
						param.EnumType = "string";
					}
					else if (param.Type.Equals("stringenum"))
					{
						if (parameter.ParameterType.IsEnum)
						{
							param.EnumValues = parameter.ParameterType.GetEnumNames().Select(s => s.ToLowerInvariant()).ToArray();

							string typeName = parameter.ParameterType.Name;
							typeName = typeName.Replace("Enum", "");
							param.EnumType = typeName;
						}
						else
						{
							if (parameter.ParameterType == typeof(ItemTypeEnum))
							{
								param.EnumValues = new string[] { };
								param.EnumType = "Item";
							}
							else if (parameter.ParameterType == typeof(BlockTypeEnum))
							{
								param.EnumValues = new string[] { };
								param.EnumType = "Block";
							}
							else if (parameter.ParameterType == typeof(EntityTypeEnum))
							{
								param.EnumValues = new string[] { };
								param.EnumType = "EntityType";
							}
							else if (parameter.ParameterType == typeof(CommandNameEnum))
							{
								param.EnumValues = new string[] { };
								param.EnumType = "CommandName";
							}
							else if (parameter.ParameterType == typeof(EnchantEnum))
							{
								param.EnumValues = new string[] { };
								param.EnumType = "Enchant";
							}
							else if (parameter.ParameterType == typeof(EffectEnum))
							{
								param.EnumValues = new string[] { };
								param.EnumType = "Effect";
							}
							else if (parameter.ParameterType == typeof(DimensionEnum))
							{
								param.EnumValues = new string[] { };
								param.EnumType = "Dimension";
							}
							else if (parameter.ParameterType == typeof(FeatureEnum))
							{
								param.EnumValues = new string[] { };
								param.EnumType = "Feature";
							}
							else
							{
								param.EnumValues = null;

								string typeName = parameter.ParameterType.Name;
								typeName = typeName.Replace("Enum", "");
								param.EnumType = typeName;
							}
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
			}

			return commands;
		}

		public static string ToCamelCase(string s)
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

			if (parameter.PropertyType == typeof(int))
				value = "int";
			else if (parameter.PropertyType == typeof(short))
				value = "int";
			else if (parameter.PropertyType == typeof(byte))
				value = "int";
			else if (parameter.PropertyType == typeof(bool))
				value = "bool";
			else if (parameter.PropertyType == typeof(string))
				value = "string";
			else if (parameter.PropertyType == typeof(string[]))
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

			if (parameter.ParameterType == typeof(int))
				value = "int";
			else if (parameter.ParameterType == typeof(short))
				value = "int";
			else if (parameter.ParameterType == typeof(byte))
				value = "int";
			else if (parameter.ParameterType == typeof(float))
				value = "float";
			else if (parameter.ParameterType == typeof(double))
				value = "float";
			else if (parameter.ParameterType == typeof(bool))
				value = "bool";
			else if (parameter.ParameterType == typeof(string))
				value = "string";
			else if (parameter.ParameterType == typeof(string[]))
				value = "rawtext";
			else if (parameter.ParameterType == typeof(Target))
				value = "target";
			else if (parameter.ParameterType == typeof(BlockPos))
				value = "blockpos";
			else if (parameter.ParameterType == typeof(EntityPos))
				value = "entitypos";
			else if (parameter.ParameterType == typeof(RelValue))
				value = "value";
			else if (parameter.ParameterType == typeof(BlockStates))
				value = "blockstates";
			else if (parameter.ParameterType.IsEnum)
				value = "stringenum";
			else if (parameter.ParameterType.BaseType == typeof(EnumBase))
				value = "stringenum";
			else if (parameter.ParameterType.BaseType == typeof(SoftEnumBase))
				value = "softenum";
			else if (typeof(IParameterSerializer).IsAssignableFrom(parameter.ParameterType))
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

			// Routes go with it. Left behind, the next request matching one would look for an
			// instance that is no longer there and answer 500 forever.
			_httpRoutes = _httpRoutes.Where(route => route.Handler.DeclaringType != instance.GetType()).ToList();

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
					PacketHandlerAttribute packetHandlerAttribute = Attribute.GetCustomAttribute(method, typeof(PacketHandlerAttribute), false) as PacketHandlerAttribute;
					if (packetHandlerAttribute != null)
					{
						ParameterInfo[] parameters = method.GetParameters();
						if (parameters.Length < 1) continue;
						if (!typeof(Packet).IsAssignableFrom(parameters[0].ParameterType)) continue;
						if (packetHandlerAttribute.PacketType == null) packetHandlerAttribute.PacketType = parameters[0].ParameterType;

						if (Attribute.GetCustomAttribute(method, typeof(SendAttribute), false) != null)
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

		public object HandleCommand(Player player, string cmdline)
		{
			var split = Regex.Split(cmdline, "(?<=^[^\"]*(?:\"[^\"]*\"[^\"]*)*) (?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)").Select(s => s.Trim('"')).ToArray();
			string commandName = split[0].Trim('/');
			string[] arguments = split.Skip(1).ToArray();

			Command command = null;
			command = GetCommand(commandName);

			//if (arguments.Length > 0 && command == null)
			//{
			//	commandName = commandName + " " + arguments[0];
			//	arguments = arguments.Skip(1).ToArray();
			//	command = GetCommand(commandName);
			//}

			if (command == null)
			{
				Log.Warn($"Found no command {commandName}");
				return null;
			}

			foreach (var overload in command.Versions.First().Overloads.Values.OrderByDescending(o => o.Input.Parameters?.Length ?? 0))
			{
				var args = arguments;

				if (args.Length > 0 && overload.Input.Parameters?.FirstOrDefault(p => p.Name.Equals("subcommand")) != null)
				{
					string subCommand = args[0];
					if (overload.Input.Parameters.FirstOrDefault(p => p.Name.Equals("subcommand") && p.EnumValues[0] == subCommand) == null) continue;
					args = args.Skip(1).ToArray();
				}

				var requiredPermission = (CommandPermission) command.Versions.First().CommandPermission;
				if (player.CommandPermission < requiredPermission)
				{
					Log.Debug($"Insufficient permissions. Require {requiredPermission} but player had {player.CommandPermission}");
					return string.Format(command.Versions.First().ErrorMessage, player.CommandPermission.ToString().ToLowerInvariant(), requiredPermission.ToString().ToLowerInvariant());
				}

				MethodInfo method = overload.Method;

				if (ExecuteCommand(method, player, args, out object retVal))
				{
					return retVal;
				}

				Log.Debug("No command executed");
			}

			return null;
		}

		private Command GetCommand(string commandName)
		{
			Command command;
			if (Commands.ContainsKey(commandName))
			{
				command = Commands[commandName];
			}
			else
			{
				command = Commands.Values.FirstOrDefault(cmd => cmd.Versions.Any(version => version.Aliases != null && version.Aliases.Any(s => s == commandName)));
			}
			return command;
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

				if (command == null)
				{
					Log.Warn($"Found no command handler for {commandName}");
					return null;
				}

				Overload overload = command.Versions.First().Overloads[commandOverload];

				var requiredPermission = (CommandPermission) command.Versions.First().CommandPermission;
				if (player.CommandPermission < requiredPermission)
				{
					Log.Debug($"Insufficient permissions. Require {requiredPermission} but player had {player.CommandPermission}");
					return null;
				}

				MethodInfo method = overload.Method;

				List<string> strings = new List<string>();
				if (commandInputJson != null)
				{
					foreach (ParameterInfo parameter in method.GetParameters())
					{
						if (typeof(Player).IsAssignableFrom(parameter.ParameterType)) continue;

						if (HasProperty(commandInputJson, parameter.Name))
						{
							Log.Debug($"Parameter: {commandInputJson[ToCamelCase(parameter.Name)].ToString()}");
							strings.Add(commandInputJson[ToCamelCase(parameter.Name)].ToString());
						}
					}
				}

				ExecuteCommand(method, player, strings.ToArray(), out object retVal);
				return retVal;
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
			return Attribute.IsDefined(param, typeof(ParamArrayAttribute));
		}

		internal bool ExecuteCommand([NotNull] MethodInfo method, [NotNull] Player player, [NotNull] string[] args, out object result)
		{
			Log.Info($"Execute command {method}, {string.Join(',', args)}");

			result = null;

			ParameterInfo[] parameters = method.GetParameters();

			bool hasPlayerParameter = parameters.Length > 0 && typeof(Player).IsAssignableFrom(parameters[0].ParameterType);

			var objectArgs = new object[parameters.Length];

			try
			{
				int argIdx = 0;
				for (int objArgIdx = 0; objArgIdx < parameters.Length; objArgIdx++)
				{
					ParameterInfo parameter = parameters[objArgIdx];
					if (objArgIdx == 0 && hasPlayerParameter)
					{
						if (typeof(Player).IsAssignableFrom(parameter.ParameterType))
						{
							objectArgs[objArgIdx] = player;
							continue;
						}
						Log.WarnFormat("Command method {0} missing Player as first argument.", method.Name);
						return false;
					}

					if (parameter.IsOptional && argIdx >= args.Length)
					{
						objectArgs[objArgIdx] = parameter.DefaultValue;
						continue;
					}

					if (argIdx >= args.Length) return false;

					if (typeof(IParameterSerializer).IsAssignableFrom(parameter.ParameterType))
					{
						ConstructorInfo ctor = parameter.ParameterType.GetConstructor(Type.EmptyTypes);
						var defaultValue = ctor?.Invoke(null) as IParameterSerializer;
						defaultValue?.Deserialize(player, args[argIdx++]);

						objectArgs[objArgIdx] = defaultValue;

						continue;
					}

					if (parameter.ParameterType.BaseType == typeof(EnumBase))
					{
						var ctor = parameter.ParameterType.GetConstructor(Type.EmptyTypes);
						var instance = (EnumBase) ctor.Invoke(null);
						instance.Value = args[argIdx++];
						objectArgs[objArgIdx] = instance;
						continue;
					}

					if (parameter.ParameterType == typeof(Target))
					{
						Target target = FillTargets(player, player.Level, args[argIdx++]);
						objectArgs[objArgIdx] = target;
						continue;
					}

					if (parameter.ParameterType == typeof(BlockStates))
					{
						objectArgs[objArgIdx] = BlockStates.Parse(args[argIdx++]);
						continue;
					}

					if (parameter.ParameterType == typeof(BlockPos))
					{
						if (args.Length < argIdx + 3) return false;

						objectArgs[objArgIdx] = BlockPos.Parse(args[argIdx++], args[argIdx++], args[argIdx++]);
						continue;
					}

					if (parameter.ParameterType == typeof(EntityPos))
					{
						if (args.Length < argIdx + 3) return false;

						objectArgs[objArgIdx] = EntityPos.Parse(args[argIdx++], args[argIdx++], args[argIdx++]);
						continue;
					}

					if (parameter.ParameterType == typeof(RelValue))
					{
						objectArgs[objArgIdx] = RelValue.Parse(args[argIdx++]);
						continue;
					}

					if (parameter.ParameterType == typeof(string))
					{
						objectArgs[objArgIdx] = args[argIdx++];
						continue;
					}
					if (parameter.ParameterType == typeof(byte))
					{
						if (!byte.TryParse(args[argIdx++], out byte value)) return false;
						objectArgs[objArgIdx] = value;
						continue;
					}
					if (parameter.ParameterType == typeof(short))
					{
						if (!short.TryParse(args[argIdx++], out short value)) return false;
						objectArgs[objArgIdx] = value;
						continue;
					}
					if (parameter.ParameterType == typeof(int))
					{
						if (!int.TryParse(args[argIdx++], out int value)) return false;
						objectArgs[objArgIdx] = value;
						continue;
					}
					if (parameter.ParameterType == typeof(bool))
					{
						if (!bool.TryParse(args[argIdx++], out bool value)) return false;
						objectArgs[objArgIdx] = value;
						continue;
					}
					if (parameter.ParameterType == typeof(float))
					{
						if (!float.TryParse(args[argIdx++], NumberStyles.Float | NumberStyles.AllowThousands, NumberFormatInfo.InvariantInfo, out float value)) return false;
						objectArgs[objArgIdx] = value;
						continue;
					}
					if (parameter.ParameterType == typeof(double))
					{
						if (!double.TryParse(args[argIdx++], NumberStyles.Float | NumberStyles.AllowThousands, NumberFormatInfo.InvariantInfo, out double value)) return false;
						objectArgs[objArgIdx] = value;
						continue;
					}
					if (parameter.ParameterType.IsEnum)
					{
						string val = args[argIdx++];
						var value = Enum.Parse(parameter.ParameterType, val, true) as Enum;
						if (value == null)
						{
							Log.Error($"Could not convert to valid enum value: {val}");
							continue;
						}

						objectArgs[objArgIdx] = value;
						continue;
					}

					if (IsParams(parameter) && parameter.ParameterType == typeof(string[]))
					{
						var strings = new List<string>();
						for (int j = argIdx++; j < args.Length; j++)
						{
							strings.Add(args[j]);
						}
						objectArgs[objArgIdx] = strings.ToArray();
						continue;
					}

					return false;
				}
			}
			catch (Exception e)
			{
				if (Log.IsDebugEnabled) Log.Error("Trying to execute command overload", e);

				return false;
			}

			try
			{
				if (method.DeclaringType == null) return false;

				object pluginInstance = _plugins.FirstOrDefault(plugin => method.DeclaringType.IsInstanceOfType(plugin));
				if (pluginInstance == null) return false;

				var filter = pluginInstance as ICommandFilter;
				filter?.OnCommandExecuting(player);

				if (method.IsStatic)
				{
					result = method.Invoke(null, objectArgs);
				}
				else
				{
					Plugin.CurrentPlayer = player; // Setting thread local for call
					result = method.Invoke(pluginInstance, objectArgs);
					Plugin.CurrentPlayer = null; // Done with thread local, we using pool to make sure it's reset.
				}

				filter?.OnCommandExecuted();

				return true;
			}
			catch (Exception e)
			{
				Log.Error($"Error while executing command {method}", e);
			}

			return false;
		}

		/// <summary>
		///     Resolves a parsed selector against the world. The parse itself belongs to
		///     <see cref="Target.Parse" />, next to the syntax it reads; this is the half that needs
		///     to see the level and so cannot live on the type.
		/// </summary>
		public Target FillTargets(Player commander, Level level, string source)
		{
			Target target = Target.Parse(source);

			if (target.Selector == "closestPlayer" && target.Rules == null)
			{
				target.Players = new[] {commander};
			}
			else if (target.Selector == "closestPlayer" && target.Rules != null)
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

		/// <summary>
		///     Whether any plugin registered a receive-side [PacketHandler] that would run for
		///     <paramref name="packetType" />. The direct-dispatch path consults this: a plugin
		///     interceptor is reflection-invoked code the IL verification can never see, so any
		///     packet type with one keeps the dispatch queue regardless of the handler's own label.
		/// </summary>
		internal bool HasReceivePacketHandler(Type packetType)
		{
			Dictionary<MethodInfo, PacketHandlerAttribute> handlers = _packetHandlerDictionary;
			if (handlers == null) return false;

			foreach (PacketHandlerAttribute attribute in handlers.Values)
			{
				if (attribute?.PacketType == null) continue;
				if (attribute.PacketType.IsAssignableFrom(packetType)) return true;
			}

			return false;
		}

		internal Packet PluginPacketHandler(Packet message, bool isReceiveHandler, Player player)
		{
			if (message == null) return null;

			Packet currentPacket = message;
			Packet returnPacket = currentPacket;

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

				// REFCT: live reflection on a per-packet path. Every packet, for every registered
				// handler, this does a GetParameters() (a fresh ParameterInfo[] each call), a LINQ scan
				// of the plugin list with a capturing closure to find the instance, an object[] for the
				// arguments, and a MethodInfo.Invoke. All of it is fixed at registration time: the
				// instance, the arity and the parameter shape cannot change per packet. Resolve them
				// once into the handler record and store a delegate, and this becomes a call. It also
				// scales with plugin count today, since the instance scan runs per handler per packet.
				foreach (var handler in packetHandlers)
				{
					if (handler.Value == null) continue;
					if (handler.Key == null) continue;

					PacketHandlerAttribute atrib = handler.Value;
					if (atrib.PacketType == null) continue;

					if (!atrib.PacketType.IsInstanceOfType(currentPacket) && atrib.PacketType != currentPacket.GetType())
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
						method.Invoke(null, new object[] {currentPacket, player});
					}
					else
					{
						object pluginInstance = _plugins.FirstOrDefault(plugin => plugin.GetType() == method.DeclaringType);
						if (pluginInstance == null) continue;

						if (method.ReturnType == typeof(void))
						{
							ParameterInfo[] parameters = method.GetParameters();
							if (parameters.Length == 1)
							{
								method.Invoke(pluginInstance, new object[] {currentPacket});
							}
							else if (parameters.Length == 2 && typeof(Player).IsAssignableFrom(parameters[1].ParameterType))
							{
								method.Invoke(pluginInstance, new object[] {currentPacket, player});
							}
						}
						else
						{
							ParameterInfo[] parameters = method.GetParameters();
							if (parameters.Length == 1)
							{
								returnPacket = method.Invoke(pluginInstance, new object[] {currentPacket}) as Packet;
							}
							else if (parameters.Length == 2 && typeof(Player).IsAssignableFrom(parameters[1].ParameterType))
							{
								returnPacket = method.Invoke(pluginInstance, new object[] {currentPacket, player}) as Packet;
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