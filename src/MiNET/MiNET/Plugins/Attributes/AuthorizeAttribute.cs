using System;
using MiNET.Config;
using MiNET.Config.Contracts;
using MiNET.Net;
using MiNET.Utils;

namespace MiNET.Plugins.Attributes
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
	public class AuthorizeAttribute : Attribute
	{
		private static readonly IServerConfiguration ServerConfig = ConfigurationProvider.MiNetConfiguration.Server;

		public int Permission { get; set; } = (int) CommandPermission.Normal;
		public string ErrorMessage { get; set; } = ServerConfig.AuthorizeErrorMessage;
	}
}