using System;
using MiNET.Net;
using MiNET.Utils;

namespace MiNET.Plugins.Attributes
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
	public class AuthorizeAttribute : Attribute
	{
		public CommandPermission Permission { get; set; } = CommandPermission.Normal;
		public string ErrorMessage { get; set; } = Config.GetProperty("Authorize.ErrorMessage", "§cInsufficient permissions. Requires {1}, but player had {0}");
	}
}