using System;

namespace MiNET.Plugins.Attributes
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
	public class AuthorizeAttribute : Attribute
	{
		public UserPermission Permission { get; set; } = UserPermission.Any;
	}
}