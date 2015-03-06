using System;

namespace MiNET.Plugins.Attributes
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
	public class CommandAttribute : Attribute
	{
		public string Command;
		public string Permission;
		public string Usage;
		public string Description;

		public CommandAttribute(string command = null, string permission = null, string description = null, string usage = null)
		{
			Command = command;
			Permission = permission;
			Usage = usage;
			Description = description;
		}
	}
}