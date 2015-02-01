using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiNET.PluginSystem.Attributes
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	public class CommandAttribute : Attribute
	{
		public string Command;
		public string Permission;
		public string Usage;
		public string Description;

		public CommandAttribute(string command, string permission, string description = "N/A", string usage = "N/A")
		{
			Command = command;
			Permission = permission;
			Usage = usage;
			Description = description;
		}
	}
}
