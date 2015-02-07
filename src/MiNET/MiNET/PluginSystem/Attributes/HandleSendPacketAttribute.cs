using System;

namespace MiNET.PluginSystem.Attributes
{
	public class HandleSendPacketAttribute : Attribute
	{
		public Type Packet;

		public HandleSendPacketAttribute(Type packet)
		{
			Packet = packet;
		}
	}
}
