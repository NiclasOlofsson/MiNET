using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiNET.PluginSystem.Attributes
{
	[AttributeUsage(AttributeTargets.Class)]
	public class PluginAttribute : Attribute
	{
		public PluginAttribute(string pluginname, string description = "", string pluginversion = "", string author = "")
		{
			PluginName = pluginname;
			Description = description;
			PluginVersion = pluginversion;
			Author = author;

			if (!Directory.Exists(PluginName)) Directory.CreateDirectory(PluginName);
		}

		public string PluginName;
		public string Description;
		public string PluginVersion;
		public string Author;
	}
}
