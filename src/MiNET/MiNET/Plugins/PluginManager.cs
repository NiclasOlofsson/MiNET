using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using log4net;
using MiNET.CommandHandler;
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

		internal void LoadPlugins()
		{
			string pluginDirectory = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
			pluginDirectory = ConfigParser.GetProperty("PluginDirectory", pluginDirectory);
			if (pluginDirectory != null)
			{
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

				CommandManager.PluginCommands.Add(commandAttribute, method);
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

		internal void EnablePlugins(Level level)
		{
			foreach (IPlugin miNetPlugin in _plugins)
			{
				try
				{
					miNetPlugin.OnEnable(new PluginContext(this, new List<Level>()));
				}
				catch (Exception ex)
				{
					Log.Warn("On enable plugin", ex);
				}
			}
		}

		internal void DisablePlugins()
		{
			foreach (IPlugin miNetPlugin in _plugins)
			{
				try
				{
					miNetPlugin.OnDisable();
				}
				catch (Exception ex)
				{
					Log.Warn("On disable plugin", ex);
				}
			}
		}

		internal Package PluginSendPacketHandler(Package message)
		{
			try
			{
				foreach (var handler in _packetSendHandlerDictionary)
				{
					PacketHandlerAttribute atrib = handler.Value;
					if (atrib.PacketType == null) continue;
					if (atrib.PacketType == message.GetType())
					{
						MethodInfo method = handler.Key;
						if (method == null) return message;
						if (method.IsStatic)
						{
							method.Invoke(null, new object[] {message, this});
						}
						else
						{
							object obj = Activator.CreateInstance(method.DeclaringType);
							Package result = method.Invoke(obj, new object[] {message, this}) as Package;
							return result;
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

			return message;
		}

		internal Package PluginPacketHandler(Package message)
		{
			Package currentPackage = message;
			try
			{
				foreach (var handler in _packetHandlerDictionary)
				{
					PacketHandlerAttribute atrib = handler.Value;
					if (atrib.PacketType == null) continue;
					if (atrib.PacketType != currentPackage.GetType()) continue;

					MethodInfo method = handler.Key;
					if (method == null) continue;
					if (method.IsStatic)
					{
						method.Invoke(null, new object[] {currentPackage, this});
					}
					else
					{
						//object obj = Activator.CreateInstance(method.DeclaringType);
						object pluginInstance = _plugins.FirstOrDefault(plugin => plugin.GetType() == method.DeclaringType);
						if (pluginInstance == null) continue;

						if (method.ReturnType == typeof (void))
						{
							method.Invoke(pluginInstance, new object[] {currentPackage, this});
						}
						else
						{
							currentPackage = method.Invoke(pluginInstance, new object[] {currentPackage, this}) as Package;
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