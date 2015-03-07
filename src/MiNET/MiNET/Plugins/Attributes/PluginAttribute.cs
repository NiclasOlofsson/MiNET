using System;

namespace MiNET.Plugins.Attributes
{
	[AttributeUsage(AttributeTargets.Class)]
	public class PluginAttribute : Attribute
	{
		public string PluginName;
		public string Description;
		public string PluginVersion;
		public string Author;

		public PluginAttribute(string pluginname = null, string description = null, string pluginversion = null, string author = null)
		{
			PluginName = pluginname;
			Description = description;
			PluginVersion = pluginversion;
			Author = author;
		}
	}
}