using System;

namespace MiNET.Plugins.Attributes
{
	[AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
	public class CommandAttribute : Attribute
	{
		public string Name;
		public string Overload;
		public string[] Aliases;
		public string Permission;
		public string Description;
		public string[] OutputFormatStrings;
	}
}