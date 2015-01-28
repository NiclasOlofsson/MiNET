using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MiNET.API;

namespace MiNET.PluginSystem
{
    class PluginLoader
    {
        private List<IMiNETPlugin> Plugins = new List<IMiNETPlugin>(); 
        public void LoadPlugins()
        {
            if (Directory.Exists("Plugins"))
                Directory.CreateDirectory("Plugins");

            string pluginsFolder = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Plugins");
            foreach (string pluginPath in Directory.GetFiles(pluginsFolder, "*.dll", SearchOption.TopDirectoryOnly))
            {
                Assembly newAssembly = Assembly.LoadFile(pluginPath);
                Type[] types = newAssembly.GetExportedTypes();
                foreach (var type in types)
                {
                    //If Type is a class and implements IPlugin interface  
                    if (type.IsClass && (type.GetInterface(typeof(IMiNETPlugin).FullName) != null))
                    {
                        var ctor = type.GetConstructor(new Type[] { });
                        if (ctor != null)
                        {
                            var plugin = ctor.Invoke(new object[] { }) as IMiNETPlugin;
                            Plugins.Add(plugin);
                        }
                    }
                }
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
    }
}
