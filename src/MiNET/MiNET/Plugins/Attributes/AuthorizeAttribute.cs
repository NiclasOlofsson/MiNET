using System;
using MiNET.Net;

namespace MiNET.Plugins.Attributes
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
	public class AuthorizeAttribute : Attribute
	{
		public CommandPermission Permission { get; set; } = CommandPermission.Normal;
	}
}