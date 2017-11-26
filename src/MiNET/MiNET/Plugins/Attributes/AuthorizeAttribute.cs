using System;
using MiNET.Net;
using MiNET.Utils;

namespace MiNET.Plugins.Attributes
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
	public class AuthorizeAttribute : Attribute
	{
		public int Permission { get; set; } = (int) CommandPermission.Normal;
		public string ErrorMessage { get; set; } = Config.GetProperty("Authorize.ErrorMessage", "§cInsufficient permissions. Requires {1}, but player had {0}");
	}
}