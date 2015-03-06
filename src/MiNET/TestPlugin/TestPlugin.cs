using System.ComponentModel;
using log4net;
using MiNET;
using MiNET.Net;
using MiNET.Plugins;
using MiNET.Plugins.Attributes;
using MiNET.Utils;

namespace TestPlugin
{
	[Plugin("CoreCommands", "The core commands for MiNET", "1.0", "MiNET Team")]
	internal class CoreCommands : Plugin
	{
	}

	[Plugin("Test", "A Test Plugin for MiNET", "1.0", "MiNET Team")]
	public class TestPlugin : Plugin
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (TestPlugin));

		protected override void OnEnable()
		{
			Log.Debug("Succesfully enabled test plugin.");
		}

		public override void OnDisable()
		{
			Log.Debug("Succesfully disabled test plugin.");
		}

		[PacketHandler]
		public Package HandlePacketTest(McpeMovePlayer packet, Player source)
		{
			Log.DebugFormat("Player (x,y,z): {0},{1},{2}", packet.x, packet.y, packet.z);
			return packet; // Handled
		}

		[PacketHandler]
		public Package HandlePacketNoAbort(McpeMovePlayer packet, Player source)
		{
			Log.DebugFormat("Player (x,y,z): {0},{1},{2}", packet.x, packet.y, packet.z);
			return packet;
		}

		[PacketHandler(typeof (McpeSetHealth))]
		[Send]
		public Package HandleSetHealthPacket(McpeSetHealth packet, Player source)
		{
			return packet; // Send
		}

		[PacketHandler]
		[Send]
		public Package HandleSetHealthPacketNoSpec(McpeSetHealth packet, Player source)
		{
			return packet; // Send
		}

		[Send]
		public Package HandleSetHealthPacketSimple(McpeSetHealth packet, Player source)
		{
			return packet; // Send
		}

		[PacketHandler(typeof (McpeSetHealth))]
		[Send]
		public void HandleSendPacketTest(Package packet, Player source)
		{
			var healthPacket = packet as McpeSetHealth;
			if (healthPacket != null)
			{
				Log.DebugFormat("Username: {0}, Health: {1}", source.Username, healthPacket.health);
			}
		}

		//[Command(Command = "Boom", Permission = "", Description = "", Usage = "")]

		[Command]
		[Description("The smallest command implementation possible.")]
		public void Min()
		{
			Log.Info("Minimal command executed.");
		}

		[Command]
		[Description("Generic command with args.")]
		public void Generic(Player player, string[] args)
		{
			Log.Info("Generic command executed.");
		}

		[Command]
		[Description("Creates an explosion with the specified radius.")]
		public void Boom(Player player, int radius = 10)
		{
			new Explosion(
				player.Level,
				new BlockCoordinates((int) player.KnownPosition.X, (int) player.KnownPosition.Y, (int) player.KnownPosition.Z),
				radius).Explode();
		}

		[Command]
		public void DeOp(Player player, string username)
		{
			Log.Info("Deop: " + username);
		}

		[Command]
		public void SetHome(Player player, string username)
		{
			Log.Info("Set Home: " + username);
		}

		[Command]
		public void SetHome(Player player, string username, float x, float y, float z)
		{
			Log.Info(string.Format("Set {0} home:  {1},{2},{3}", username, x, y, z));
		}
	}
}