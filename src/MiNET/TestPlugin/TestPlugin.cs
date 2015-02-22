using System;
using System.Net;
using MiNET;
using MiNET.API;
using MiNET.Net;
using MiNET.PluginSystem.Attributes;
using MiNET.Worlds;

namespace TestPlugin
{
	[Plugin("Test", "A Test Plugin for MiNET", "1.0", "MiNET Team")]
	public partial class TestPlugin : MiNETPlugin
	{
		public override void OnEnable(Level level)
		{
			Console.WriteLine("[TestPlugin] Succesfully enabled test plugin :-)");
		}

		public override void OnDisable()
		{
			Console.WriteLine("[TestPlugin] Succesfully disabled test plugin :-)");
		}

		[HandlePacket(typeof(McpeMovePlayer))]
		public void HandlePacketTest(Package packet, Player source)
		{
			McpeMovePlayer pack = (McpeMovePlayer)packet;
			Console.WriteLine("[TestPlugin] X: " + pack.x);	
		}

		[HandleSendPacket(typeof(McpeSetHealth))]
		public void HandleSendPacketTest(Package packet, Player source)
		{
			McpeSetHealth pack = (McpeSetHealth)packet;
			Console.WriteLine("[TestPlugin] Health: " + pack.health);
			Console.WriteLine("[TestPlugin] Username: " + source.Username);
		}
	}
}