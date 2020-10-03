using System.Reflection;
using MiNET.Plugins;

namespace MiNET.Events.Plugins
{
    /// <summary>
    ///     Gets dispatched when a plugin was loaded & enabled
    /// </summary>
    public class PluginEnabledEvent : Event
    {
        /// <summary>
        ///     The assembly the plugin was loaded from
        /// </summary>
        public Assembly PluginAssembly { get; set; }
        
        /// <summary>
        ///     The plugin instance that was enabled
        /// </summary>
        public Plugin PluginInstance { get; set; }
        
        /// <summary>
        ///     
        /// </summary>
        /// <param name="pluginAssembly"></param>
        /// <param name="pluginInstance"></param>
        public PluginEnabledEvent(Assembly pluginAssembly, Plugin pluginInstance)
        {
            PluginAssembly = pluginAssembly;
            PluginInstance = pluginInstance;
        }
    }
}