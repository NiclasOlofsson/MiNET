using System;
using System.Net;
using MiNET.API;
using MiNET.Net;
using MiNET.PluginSystem.Attributes;

namespace TestPlugin
{
	[Plugin("Test", "A Test Plugin for MiNET", "1.0", "MiNET Team")]
	public partial class TestPlugin : MiNETPlugin
	{
		public override void OnEnable()
		{
			Console.WriteLine("Succesfully enabled test plugin :-)");
		}

		public override void OnDisable()
		{
			Console.WriteLine("Succesfully disabled test plugin :-)");
		}

		[HandlePacket(typeof(McpeMovePlayer))]
		public void HandlePacketTest(Package packet, IPEndPoint senderEndPoint)
		{
			McpeMovePlayer pack = (McpeMovePlayer)packet;
			Console.WriteLine("X: " + pack.x);	
		}

		[HandleSendPacket(typeof(McpeSetHealth))]
		public void HandleSendPacketTest(Package packet, IPEndPoint senderEndPoint)
		{
			McpeSetHealth pack = (McpeSetHealth)packet;
			Console.WriteLine("Health: " + pack.health);
		}
	}
}