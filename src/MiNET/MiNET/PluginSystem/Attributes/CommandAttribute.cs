using System;

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