using System;

namespace MiNET.Plugins.Attributes
{
	public class PacketHandlerAttribute : Attribute
	{
		public Type PacketType { get; internal set; }

		public PacketHandlerAttribute(Type packetType = null)
		{
			PacketType = packetType;
		}
	}

	public class ReceiveAttribute : Attribute
	{
	}

	public class SendAttribute : Attribute
	{
	}
}