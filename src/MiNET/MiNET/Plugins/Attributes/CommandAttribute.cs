using System;

namespace MiNET.Plugins.Attributes
{
	[AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
	public class CommandAttribute : Attribute
	{
		public string Command;
		public string Permission;
		public string Usage;
		public string Description;
	}
}