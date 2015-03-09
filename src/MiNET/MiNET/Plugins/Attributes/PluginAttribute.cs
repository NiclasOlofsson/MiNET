using System;

namespace MiNET.Plugins.Attributes
{
	[AttributeUsage(AttributeTargets.Class, Inherited = true)]
	public class PluginAttribute : Attribute
	{
		public string PluginName;
		public string Description;
		public string PluginVersion;
		public string Author;
	}
}