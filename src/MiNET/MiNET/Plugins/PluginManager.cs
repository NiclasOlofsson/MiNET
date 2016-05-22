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

namespace MiNET.Plugins
{
    public class PluginManager
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MiNetServer));

        private readonly List<object> _plugins = new List<object>();
        private readonly Dictionary<MethodInfo, CommandAttribute> _pluginCommands = new Dictionary<MethodInfo, CommandAttribute>();

        private readonly IDictionary<object, ISet<IPackageEventHandler>> _registrantHandlerMapping = new Dictionary<object, ISet<IPackageEventHandler>>();
        private readonly IDictionary<Type, ISet<IPackageEventHandler>> _packageReceiveHandlers = new Dictionary<Type, ISet<IPackageEventHandler>>();
        private readonly IDictionary<Type, ISet<IPackageEventHandler>> _packageSendHandlers = new Dictionary<Type, ISet<IPackageEventHandler>>();
        private readonly IPackageEventHandlerGenerator _packageEventHandlerGenerator = new EmittedPackageHandlerGenerator();

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

            foreach (string dirPath in pluginDirectoryPaths.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (dirPath == null) continue;

                string pluginDirectory = Path.GetFullPath(dirPath);

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
                            if (!type.IsDefined(typeof(PluginAttribute), true) && !typeof(IPlugin).IsAssignableFrom(type)) continue;
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
                                _plugins.Add(plugin);
                                LoadCommands(type);
                                CreatePacketHandlers(plugin);
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
        }

        private void LoadCommands(Type type)
        {
            var methods = type.GetMethods();
            foreach (MethodInfo method in methods)
            {
                CommandAttribute commandAttribute = Attribute.GetCustomAttribute(method, typeof(CommandAttribute), false) as CommandAttribute;
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
                bool isFirstParam = true;
                foreach (var parameter in parameters)
                {
                    if (isFirstParam && parameter.ParameterType == typeof(Player))
                    {
                        continue;
                    }
                    isFirstParam = false;

                    sb.AppendFormat("<{0}> ", parameter.Name);
                }
                commandAttribute.Usage = sb.ToString().Trim();

                DescriptionAttribute descriptionAttribute = Attribute.GetCustomAttribute(method, typeof(DescriptionAttribute), false) as DescriptionAttribute;
                if (descriptionAttribute != null) commandAttribute.Description = descriptionAttribute.Description;

                _pluginCommands.Add(method, commandAttribute);
                Log.Debug($"Loaded command {commandAttribute.Usage}");
            }
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
        }

        public void LoadPacketHandlers(object instance)
        {
            if (!_plugins.Contains(instance)) _plugins.Add(instance);
            CreatePacketHandlers(instance);
        }

        private void CreatePacketHandlers(object instance)
        {
            Type type = instance.GetType();
            foreach (MethodInfo method in type.GetMethods())
            {
                PacketHandlerAttribute packetHandlerAttribute = Attribute.GetCustomAttribute(method, typeof(PacketHandlerAttribute), false) as PacketHandlerAttribute;
                if (packetHandlerAttribute != null)
                {
                    ParameterInfo[] parameters = method.GetParameters();
                    if (parameters.Length < 1 || parameters.Length > 2)
                    {
                        continue;
                    }

                    if (!typeof(Package).IsAssignableFrom(parameters[0].ParameterType) || (parameters.Length == 2 && typeof(Player) != parameters[1].ParameterType))
                    {
                        continue;
                    }

                    // TODO: Figure out potential alternative for tracking registration of static methods.
                    if (method.IsStatic && parameters.Length != 2)
                    {
                        continue;
                    }

                    if (packetHandlerAttribute.PacketType == null)
                    {
                        packetHandlerAttribute.PacketType = parameters[0].ParameterType;
                    }
                    else
                    {
                        if (packetHandlerAttribute.PacketType != parameters[0].ParameterType)
                        {
                            // TODO: Is it more appropriate to throw an error, or log a warning here?
                            continue;
                        }
                    }

                    IDictionary<Type, ISet<IPackageEventHandler>> target = _packageReceiveHandlers;
                    if (method.GetCustomAttribute<SendAttribute>(false) != null)
                    {
                        target = _packageSendHandlers;
                    }

                    Type package = packetHandlerAttribute.PacketType;
                    ISet<IPackageEventHandler> handlers;
                    if (!target.TryGetValue(package, out handlers))
                    {
                        handlers = new HashSet<IPackageEventHandler>();
                        target.Add(package, handlers);
                    }

                    try
                    {
                        IPackageEventHandler handler = _packageEventHandlerGenerator.Generate(instance, method, package);
                        if (!handlers.Add(handler))
                        {
                            Log.WarnFormat("Failed to register handler for {0}#{1}, already registered?", type.FullName, method.Name);
                        }

                        if (!_registrantHandlerMapping.TryGetValue(instance, out handlers))
                        {
                            handlers = new HashSet<IPackageEventHandler>();
                            _registrantHandlerMapping.Add(instance, handlers);
                        }
                        handlers.Add(handler);
                    }
                    catch (Exception exception)
                    {
                        Log.ErrorFormat("Failed to generate handler for {0}#{1} using {2}: {3}", type.FullName, method.Name, _packageEventHandlerGenerator.GetType().FullName, exception);
                    }
                }
            }
        }

        public void UnloadPacketHandlers(object instance)
        {
            ISet<IPackageEventHandler> handlers;
            if (_registrantHandlerMapping.TryGetValue(instance, out handlers))
            {
                _registrantHandlerMapping.Remove(instance);

                foreach (IPackageEventHandler handler in handlers)
                {
                    foreach (var set in _packageSendHandlers.Values)
                    {
                        set.Remove(handler);
                    }

                    foreach (var set in _packageReceiveHandlers.Values)
                    {
                        set.Remove(handler);
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
                finally
                {
                    UnloadPacketHandlers(enablingPlugin);
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

                string[] arguments = message.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                List<CommandAttribute> foundCommands = new List<CommandAttribute>();
                foreach (var handlerEntry in _pluginCommands)
                {
                    CommandAttribute commandAttribute = handlerEntry.Value;
                    if (!commandText.Equals(commandAttribute.Command, StringComparison.InvariantCultureIgnoreCase)) continue;

                    MethodInfo method = handlerEntry.Key;
                    if (method == null) return;

                    foundCommands.Add(commandAttribute);

                    var authorizationAttributes = method.GetCustomAttributes<AuthorizeAttribute>(true);
                    foreach (AuthorizeAttribute authorizationAttribute in authorizationAttributes)
                    {
                        if (userManager == null)
                        {
                            player.SendMessage($"UserManager not found. You are not permitted to use this command!");
                            return;
                        }

                        User user = userManager.FindByName(player.Username);
                        if (user == null)
                        {
                            player.SendMessage($"No registered user '{player.Username}' found. You are not permitted to use this command!");
                            return;
                        }

                        var userIdentity = userManager.CreateIdentity(user, "none");
                        if (!authorizationAttribute.OnAuthorization(new GenericPrincipal(userIdentity, new string[0])))
                        {
                            player.SendMessage("You are not permitted to use this command!");
                            return;
                        }
                    }

                    if (ExecuteCommand(method, player, arguments)) return;
                }

                foreach (var commandAttribute in foundCommands)
                {
                    player.SendMessage($"Usage: {commandAttribute.Usage}");
                }
            }
            catch (Exception ex)
            {
                Log.Warn(ex);
            }
        }

        private static bool IsParams(ParameterInfo param)
        {
            return Attribute.IsDefined(param, typeof(ParamArrayAttribute));
        }

        private bool ExecuteCommand(MethodInfo method, Player player, string[] args)
        {
            Log.Info($"Execute command {method}");

            var parameters = method.GetParameters();

            int addLenght = 0;
            if (parameters.Length > 0 && parameters[0].ParameterType == typeof(Player))
            {
                addLenght = 1;
            }

            if (IsParams(parameters.Last()))
            {
                // method params ex: int int params int[] 
                // input ex:           1  1  1 1 1 
                // so arguments in must be at least the lenght of method arguments
                if (parameters.Length - 1 > args.Length + addLenght) return false;
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
                    if (parameter.ParameterType == typeof(Player))
                    {
                        objectArgs[k] = player;
                        continue;
                    }
                    Log.WarnFormat("Command method {0} missing Player as first argument.", method.Name);
                    return false;
                }

                if (parameter.ParameterType == typeof(string))
                {
                    objectArgs[k] = args[i];
                    continue;
                }
                if (parameter.ParameterType == typeof(byte))
                {
                    byte value;
                    if (!byte.TryParse(args[i], out value)) return false;
                    objectArgs[k] = value;
                    continue;
                }
                if (parameter.ParameterType == typeof(short))
                {
                    short value;
                    if (!short.TryParse(args[i], out value)) return false;
                    objectArgs[k] = value;
                    continue;
                }
                if (parameter.ParameterType == typeof(int))
                {
                    int value;
                    if (!int.TryParse(args[i], out value)) return false;
                    objectArgs[k] = value;
                    continue;
                }
                if (parameter.ParameterType == typeof(bool))
                {
                    bool value;
                    if (!bool.TryParse(args[i], out value)) return false;
                    objectArgs[k] = value;
                    continue;
                }
                if (parameter.ParameterType == typeof(float))
                {
                    float value;
                    if (!float.TryParse(args[i], out value)) return false;
                    objectArgs[k] = value;
                    continue;
                }
                if (parameter.ParameterType == typeof(double))
                {
                    double value;
                    if (!double.TryParse(args[i], out value)) return false;
                    objectArgs[k] = value;
                    continue;
                }

                if (IsParams(parameter) && parameter.ParameterType == typeof(string[]))
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
            if (message == null) return null;

            Package result = message;
            try
            {
                IDictionary<Type, ISet<IPackageEventHandler>> packetHandlers = _packageReceiveHandlers;
                if (!isReceiveHandler)
                {
                    packetHandlers = _packageSendHandlers;
                }

                ISet<IPackageEventHandler> handlers;
                if (!packetHandlers.TryGetValue(message.GetType(), out handlers))
                {
                    return message;
                }

                foreach (var handler in handlers)
                {
                    result = handler.Handle(message, player);
                }
            }
            catch (Exception ex)
            {
                //For now we will just ignore this, not to big of a deal.
                //Will have to think a bit more about this later on.
                Log.Warn("Plugin Error: ", ex);
                Log.Warn("Plugin Error: ", ex.InnerException);
            }
            return result;
        }
    }
}