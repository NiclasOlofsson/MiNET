using System;

namespace MiNET.Plugins.Attributes
{
	[AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
	public class PacketHandlerAttribute : Attribute
	{
		public Type PacketType { get; set; }
	}

	[AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
	public class ReceiveAttribute : Attribute
	{
	}

	[AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
	public class SendAttribute : Attribute
	{
	}
}