using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiNET.Net;

namespace MiNET.PluginSystem.Attributes
{
	public class HandlePacketAttribute : Attribute
	{
		public Type Packet;

		public HandlePacketAttribute(Type packet)
		{
			Packet = packet;
		}
	}
}
