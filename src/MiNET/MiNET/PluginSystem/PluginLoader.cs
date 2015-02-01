using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MiNET.API;
using MiNET.PluginSystem.Attributes;

namespace MiNET.PluginSystem
{
    class PluginLoader
    {
        private List<IMiNETPlugin> Plugins = new List<IMiNETPlugin>(); 
        public void LoadPlugins()
        {
            if (!Directory.Exists("Plugins"))
                Directory.CreateDirectory("Plugins");

            string pluginsFolder = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Plugins");
            foreach (string pluginPath in Directory.GetFiles(pluginsFolder, "*.dll", SearchOption.TopDirectoryOnly))
            {
                Assembly newAssembly = Assembly.LoadFile(pluginPath);
                Type[] types = newAssembly.GetExportedTypes();
                foreach (Type type in types)
                {
					new Task(() => GetCommandHandlers(type)).Start();
	                if (!type.IsDefined(typeof (PluginAttribute), true)) continue;
	                var ctor = type.GetConstructor(new Type[] {});
	                if (ctor != null)
	                {
		                var plugin = ctor.Invoke(new object[] {}) as IMiNETPlugin;
		                Plugins.Add(plugin);
	                }
                }
            }
        }

	    private void GetCommandHandlers(Type type)
	    {
			var methods = type.GetMethods();
			foreach (MethodInfo method in methods)
			{
				var cmd = Attribute.GetCustomAttribute(method,
					typeof(CommandAttribute), false) as CommandAttribute;
				if (cmd == null)
					continue;
				CommandHandler.CommandHandler.PluginCommands.Add(cmd, method);
			}
	    }

        public void EnablePlugins()
        {
            foreach (IMiNETPlugin miNetPlugin in Plugins)
            {
                miNetPlugin.OnEnable();
            }
        }

        public void DisablePlugins()
        {
            foreach (IMiNETPlugin miNetPlugin in Plugins)
            {
                miNetPlugin.OnDisable();
            }
        }

        private static string GetAttribute(Type t, PluginAttributes atribute)
        {
            PluginAttribute data =
                (PluginAttribute)Attribute.GetCustomAttribute(t, typeof(PluginAttribute));

            if (data != null)
            {
                switch (atribute)
                {
                    case PluginAttributes.Author:
						return data.Author;
                    case PluginAttributes.Description:
		                return data.Description;
					case PluginAttributes.Name:
		                return data.PluginName;
					case PluginAttributes.Version:
		                return data.PluginVersion;
                }
            }
	        return "N/A";
        }

        private enum PluginAttributes
        {
            Author,
            Description,
            Version,
            Name
        }
    }
}
