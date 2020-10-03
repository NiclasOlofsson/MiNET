using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using log4net;
using MiNET.Plugins;
using MiNET.Plugins.Attributes;
using MiNET.Plugins.Commands;
using MiNET.Worlds;
using Newtonsoft.Json.Linq;

namespace MiNET.Commands
{
    public class CommandManager
    {
	    private static readonly ILog Log = LogManager.GetLogger(typeof(CommandManager));
	    
	//	private MiNET.Plugins.PluginManager PluginManager { get; }
	//	private readonly Dictionary<MethodInfo, CommandAttribute> _pluginCommands = new Dictionary<MethodInfo, CommandAttribute>();
		//public CommandManager(MiNET.Plugins.PluginManager pluginManager)
		//{
		//	PluginManager = pluginManager;
		//}
		
		private readonly Dictionary<MethodInfo, CommandData> _pluginCommands = new Dictionary<MethodInfo, CommandData>();
		private PluginManager PluginManager { get; }
        private ConcurrentDictionary<Type, CommandPermissionChecker> _permissionCheckers = new ConcurrentDictionary<Type, CommandPermissionChecker>();
        private bool HasPermissionChecker { get; set; } = false;
        public bool HasExternalPermissionChecker { get; set; } = false;
        public CommandManager(PluginManager pluginManager)
		{
			PluginManager = pluginManager;

			HasExternalPermissionChecker = false;
		}

        internal void Init()
        {
	        if (!_permissionCheckers.ContainsKey(typeof(StringPermissionAttribute)))
	        {
		        RegisterPermissionChecker(typeof(StringPermissionAttribute), new DefaultPermissionChecker());
	        }
        }

        public void RegisterPermissionChecker<TType>(CommandPermissionChecker<TType> permissionChecker)
	        where TType : CommandPermissionAttribute
        {
	        _permissionCheckers[typeof(TType)] = permissionChecker;
	        
	        HasPermissionChecker = true;
	        HasExternalPermissionChecker = true;
        }

        public void RegisterPermissionChecker(Type type, CommandPermissionChecker permissionChecker)
        {
            _permissionCheckers[type] = permissionChecker;

            HasPermissionChecker = true;
            HasExternalPermissionChecker = true;
        }

	    public void LoadCommands(object instance)
	    {
			//PluginManager.LoadCommands(instance);
			
			var type = instance.GetType();
			var methods = type.GetMethods();
			foreach (MethodInfo method in methods)
			{
				CommandAttribute commandAttribute = Attribute.GetCustomAttribute(method, typeof(CommandAttribute), false) as CommandAttribute;
				if (commandAttribute == null) continue;

				if (string.IsNullOrEmpty(commandAttribute.Name))
				{
					commandAttribute.Name = method.Name;
				}

				DescriptionAttribute descriptionAttribute = Attribute.GetCustomAttribute(method, typeof(DescriptionAttribute), false) as DescriptionAttribute;
				if (descriptionAttribute != null) commandAttribute.Description = descriptionAttribute.Description;

                CommandPermissionAttribute permissionAttribute = Attribute.GetCustomAttribute(method, typeof(CommandPermissionAttribute), true) as CommandPermissionAttribute;

                // Overwrite custom permissions if command permission is supplied
                if(!string.IsNullOrEmpty(commandAttribute.Permission))
                {
                    permissionAttribute = new StringPermissionAttribute(commandAttribute.Permission);
                }

				try
				{
					_pluginCommands.Add(method, new CommandData(commandAttribute, instance, permissionAttribute));
				}
				catch (ArgumentException e)
				{
					Log.Debug($"Command already exist {method.Name}, {method}", e);
				}
			}
	    }

	    public void UnloadCommands(object instance)
	    {
		    var instanceType = instance.GetType();
		    
		    var methods = _pluginCommands.Keys.Where(info => info.DeclaringType == instanceType).ToArray();
		    foreach (var method in methods)
		    {
			    _pluginCommands.Remove(method);
		    }
	    }

	    internal void UnloadCommands(Plugin plugin)
	    {
		    foreach (var command in _pluginCommands.Where(x =>
			    x.Value.Instance.GetType().Assembly == plugin.GetType().Assembly).ToArray())
		    {
			    _pluginCommands.Remove(command.Key);
		    }
	    }

	    internal CommandSet GenerateCommandSet(MiNET.Player player)
	    {
		    return GenerateCommandSet(_pluginCommands
			    .Where(x => x.Value.PermissionAttribute == null || 
                    (_permissionCheckers[x.Value.PermissionAttribute.GetType()] != null && _permissionCheckers[x.Value.PermissionAttribute.GetType()].HasPermission(x.Value.PermissionAttribute, player))).Select(x => x.Key).ToArray());
	    }

	    private CommandSet GenerateCommandSet(MethodInfo[] methods)
		{
			CommandSet commands = new CommandSet();

			foreach (MethodInfo method in methods)
			{
				CommandAttribute commandAttribute = Attribute.GetCustomAttribute(method, typeof(CommandAttribute), false) as CommandAttribute;
				if (commandAttribute == null) continue;

                // TODO: Do we need the authorize attribute with the new permission system?
				AuthorizeAttribute authorizeAttribute = Attribute.GetCustomAttribute(method, typeof(AuthorizeAttribute), false) as AuthorizeAttribute ?? new AuthorizeAttribute();

				if (string.IsNullOrEmpty(commandAttribute.Name))
				{
					commandAttribute.Name = method.Name;
				}

				var overload = new Overload
				{
					Description = commandAttribute.Description ?? "Bullshit",
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
							new MiNET.Plugins.Commands.Version()
							{
								Permission = authorizeAttribute.Permission.ToString().ToLowerInvariant(),
								CommandPermission = authorizeAttribute.Permission,
								ErrorMessage = authorizeAttribute.ErrorMessage,
								Aliases = commandAttribute.Aliases ?? new string[0],
								Description = commandAttribute.Description ?? "",
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


				List<Parameter> inputParams = new List<Parameter>();
				if (subCommmandParam != null)
				{
					inputParams.Add(subCommmandParam);
				}

				var parameters = method.GetParameters();
				bool isFirstParam = true;
				foreach (var parameter in parameters)
				{
					if (isFirstParam && typeof(MiNET.Player).IsAssignableFrom(parameter.ParameterType))
					{
						continue;
					}
					isFirstParam = false;
					
					if (PluginManager.Services.TryResolve(parameter.ParameterType, out _))
					{
						//This is a depencency injected param
						continue;
					}

					Parameter param = new Parameter();
					param.Name = ToCamelCase(parameter.Name);
					param.Type = GetParameterType(parameter);
					param.Optional = parameter.IsOptional;
					if (param.Type.Equals("bool"))
					{
						param.Type = "stringenum";
						param.EnumType = "bool";
						param.EnumValues = new string[] {"false", "true"};
					}
					else if (param.Type.Equals("stringenum"))
					{
						if (parameter.ParameterType.IsEnum)
						{
							param.EnumValues = parameter.ParameterType.GetEnumNames().Select(s => s.ToLowerInvariant()).ToArray();

							string typeName = parameter.ParameterType.Name;
							typeName = typeName.Replace("Enum", "");
							typeName = typeName.ToLowerInvariant()[0] + typeName.Substring(1);
							param.EnumType = typeName;
						}
						else
						{
							param.EnumValues = null;

							string typeName = parameter.ParameterType.Name;
							typeName = typeName.Replace("Enum", "");
							typeName = typeName.ToLowerInvariant()[0] + typeName.Substring(1);
							param.EnumType = typeName;

							if (parameter.ParameterType == typeof(ItemTypeEnum))
							{
								param.EnumValues = new string[] { };
								param.EnumType = "Item";
							}
							if (parameter.ParameterType == typeof(BlockTypeEnum))
							{
								param.EnumValues = new string[] { };
								param.EnumType = "Block";
							}
							if (parameter.ParameterType == typeof(EntityTypeEnum))
							{
								param.EnumValues = new string[] { };
								param.EnumType = "EntityType";
							}
							if (parameter.ParameterType == typeof(CommandNameEnum))
							{
								param.EnumValues = new string[] { };
								param.EnumType = "CommandName";
							}
							if (parameter.ParameterType == typeof(EnchantEnum))
							{
								param.EnumValues = new string[] {"enchant_test"};
								param.EnumType = "Enchant";
							}
							if (parameter.ParameterType == typeof(EffectEnum))
							{
								param.EnumValues = new string[] {"effect_test"};
								param.EnumType = "Effect";
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
			else if (parameter.ParameterType.IsEnum)
				value = "stringenum";
			else if (parameter.ParameterType.BaseType == typeof(EnumBase))
				value = "stringenum";
			else if (typeof(IParameterSerializer).IsAssignableFrom(parameter.ParameterType))
				// Custom serialization
				value = "string";
			else
				Log.Warn("No parameter type mapping for type: " + parameter.ParameterType.ToString());

			return value;
		}
		
		#region Command Handling
		
		internal object HandleCommand(MiNET.Player player, string cmdline)
		{
			var split = Regex.Split(cmdline, "(?<=^[^\"]*(?:\"[^\"]*\"[^\"]*)*) (?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)").Select(s => s.Trim('"')).ToArray();
			string commandName = split[0].Trim('/');
			string[] arguments = split.Skip(1).ToArray();

			Command command = null;
			command = GetCommand(player.Commands, commandName);

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

                // TODO: Do we need the authorize attribute with the new permission system?
                int requiredPermission = command.Versions.First().CommandPermission;
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

				Log.Debug("No result from execution");
			}

			return null;
		}

		private Command GetCommand(CommandSet commands, string commandName)
		{
			Command command;
			if (commands.ContainsKey(commandName))
			{
				command = commands[commandName];
			}
			else
			{
				command = commands.Values.FirstOrDefault(cmd => cmd.Versions.Any(version => version.Aliases != null && version.Aliases.Any(s => s == commandName)));
			}
			return command;
		}

		internal object HandleCommand(MiNET.Player player, string commandName, string commandOverload, dynamic commandInputJson)
		{
			Log.Debug($"HandleCommand {commandName}");
			var commands = player.Commands;
			try
			{
				Command command = null;
				if (commands.ContainsKey(commandName))
				{
					command = commands[commandName];
				}
				else
				{
					command = commands.Values.FirstOrDefault(cmd => cmd.Versions.Any(version => version.Aliases != null && version.Aliases.Any(s => s == commandName)));
				}

				if (command == null)
				{
					Log.Warn($"Found no command handler for {commandName}");
					return null;
				}

				Overload overload = command.Versions.First().Overloads[commandOverload];

				int requiredPermission = command.Versions.First().CommandPermission;
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
						if (typeof(MiNET.Player).IsAssignableFrom(parameter.ParameterType)) continue;

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

		private bool ExecuteCommand(MethodInfo method, MiNET.Player player, string[] args, out object result)
		{
			Log.Info($"Execute command {method}");

			result = null;

			var parameters = method.GetParameters();

			int addLenght = 0;
			if (parameters.Length > 0 && typeof(MiNET.Player).IsAssignableFrom(parameters[0].ParameterType))
			{
				addLenght = 1;
			}

			object[] objectArgs = new object[parameters.Length];

			try
			{
				int i = 0;
				for (int k = 0; k < parameters.Length; k++)
				{
					var parameter = parameters[k];
					if (k == 0 && addLenght == 1)
					{
						if (typeof(MiNET.Player).IsAssignableFrom(parameter.ParameterType))
						{
							objectArgs[k] = player;
							continue;
						}
						Log.WarnFormat("Command method {0} missing Player as first argument.", method.Name);
						return false;
					}

					if (PluginManager.Services.TryResolve(parameter.ParameterType, out var param))
					{
						objectArgs[k] = param;
						continue;
					}

					if (parameter.IsOptional && args.Length <= i)
					{
						objectArgs[k] = parameter.DefaultValue;
						continue;
					}

					if (args.Length < k) return false;

					if (typeof(IParameterSerializer).IsAssignableFrom(parameter.ParameterType))
					{
						var ctor = parameter.ParameterType.GetConstructor(Type.EmptyTypes);
						IParameterSerializer defaultValue = ctor.Invoke(null) as IParameterSerializer;
						defaultValue?.Deserialize(player, args[i++]);

						objectArgs[k] = defaultValue;

						continue;
					}

					if (parameter.ParameterType.BaseType == typeof(EnumBase))
					{
						var ctor = parameter.ParameterType.GetConstructor(Type.EmptyTypes);
						EnumBase instance = (EnumBase) ctor.Invoke(null);
						instance.Value = args[i++];
						objectArgs[k] = instance;
						continue;
					}

					if (parameter.ParameterType == typeof(Target))
					{
						var target = FillTargets(player, player.Level, args[i++]);
						objectArgs[k] = target;
						continue;
					}

					if (parameter.ParameterType == typeof(BlockPos))
					{
						if (args.Length < i + 3) return false;

						BlockPos blockPos = new BlockPos();

						string val = args[i++];
						if (val.StartsWith("~"))
						{
							val = val.Substring(1);
							blockPos.XRelative = true;
						}

						int.TryParse(val, out var x);
						blockPos.X = x;

						val = args[i++];
						if (val.StartsWith("~"))
						{
							val = val.Substring(1);
							blockPos.YRelative = true;
						}

						int.TryParse(val, out var y);
						blockPos.Y = y;

						val = args[i++];
						if (val.StartsWith("~"))
						{
							val = val.Substring(1);
							blockPos.ZRelative = true;
						}

						int.TryParse(val, out var z);
						blockPos.Z = z;

						objectArgs[k] = blockPos;
						continue;
					}

					if (parameter.ParameterType == typeof(string))
					{
						objectArgs[k] = args[i++];
						continue;
					}
					if (parameter.ParameterType == typeof(byte))
					{
						byte value;
						if (!byte.TryParse(args[i++], out value)) return false;
						objectArgs[k] = value;
						continue;
					}
					if (parameter.ParameterType == typeof(short))
					{
						short value;
						if (!short.TryParse(args[i++], out value)) return false;
						objectArgs[k] = value;
						continue;
					}
					if (parameter.ParameterType == typeof(int))
					{
						int value;
						if (!int.TryParse(args[i++], out value)) return false;
						objectArgs[k] = value;
						continue;
					}
					if (parameter.ParameterType == typeof(bool))
					{
						bool value;
						if (!bool.TryParse(args[i++], out value)) return false;
						objectArgs[k] = value;
						continue;
					}
					if (parameter.ParameterType == typeof(float))
					{
						float value;
						if (!float.TryParse(args[i++], out value)) return false;
						objectArgs[k] = value;
						continue;
					}
					if (parameter.ParameterType == typeof(double))
					{
						double value;
						if (!double.TryParse(args[i++], out value)) return false;
						objectArgs[k] = value;
						continue;
					}
					if (parameter.ParameterType.IsEnum)
					{
						string val = args[i++];
						Enum value = Enum.Parse(parameter.ParameterType, val, true) as Enum;
						if (value == null)
						{
							Log.Error($"Could not convert to valid enum value: {val}");
							continue;
						}

						objectArgs[k] = value;
						continue;
					}

					if (IsParams(parameter) && parameter.ParameterType == typeof(string[]))
					{
						List<string> strings = new List<string>();
						for (int j = i++; j < args.Length; j++)
						{
							strings.Add(args[j]);
						}
						objectArgs[k] = strings.ToArray();
						continue;
					}

					return false;
				}
			}
			catch (Exception e)
			{
			//	if (Log.IsDebugEnabled)
				{
					Log.Error("Trying to execute command overload", e);
				}

				return false;
			}

			try
			{
				if (method.IsStatic)
				{
					result = method.Invoke(null, objectArgs);
				}
				else
				{
					if (_pluginCommands.TryGetValue(method, out CommandData data))
					{
						object instance = data.Instance;
						
						Plugin.CurrentPlayer = player; // Setting thread local for call
						result = method.Invoke(instance, objectArgs);
						Plugin.CurrentPlayer = null;
					}
					else
					{
						Log.Warn($"Could not find instance of command's declaringtype!");
						return false;
					}
				}

				return true;
			}
			catch (Exception e)
			{
				Log.Error($"Error while executing command {method}", e);
			}

			return false;
		}

		public Target FillTargets(MiNET.Player commander, Level level, string source)
		{
			Target target = ParseTarget(source);

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
				MiNET.Player[] players = level.GetAllPlayers();
				target.Players = new[] {players[new Random().Next(players.Length)]};
			}


			return target;
		}

		public static Target ParseTarget(string source)
		{
			Target target = new Target();
			if (!source.StartsWith("@"))
			{
				target.Selector = "closestPlayer";
				target.Rules = new[]
				{
					new Target.Rule()
					{
						Name = "name",
						Value = source
					}
				};
			}
			else
			{
				var matches = Regex.Matches(source, @"^(?<selector>@[aeprs])(\[((?<args>(c|dx|dy|dz|l|lm|m|name|r|rm|rx|rxm|rym|type|x|y|z)=.*?)(,*?))*\])*$");
				var selector = matches[0].Groups["selector"].Captures[0].Value;
				switch (selector)
				{
					case "@a":
						selector = "allPlayers";
						break;
					case "@e":
						selector = "allEntities";
						break;
					case "@p":
						selector = "closestPlayer";
						break;
					case "@r":
						selector = "randomPlayer";
						break;
					case "@s":
						selector = "yourself";
						break;
				}
				target.Selector = selector;
				List<Target.Rule> rules = new List<Target.Rule>();
				foreach (Capture arg in matches[0].Groups["args"].Captures)
				{
					string[] split = arg.Value.Split('=');
					string name = split[0];
					string value = split[1];

					Target.Rule rule = new Target.Rule();
					rule.Name = name;
					if (value.StartsWith("!"))
					{
						rule.Inverted = true;
						rule.Value = value.Substring(1);
					}
					else
					{
						rule.Value = value;
					}

					rules.Add(rule);
				}


				if (rules.Count != 0) target.Rules = rules.ToArray();
			}

			return target;
		}
		
		#endregion

		public string GetUsage(Command command)
		{
			throw new NotImplementedException();
		}

		public string GetUsage(Command command, bool b, string aqua)
		{
			throw new NotImplementedException();
		}
	}
}
