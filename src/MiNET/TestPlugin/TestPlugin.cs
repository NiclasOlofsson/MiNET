 // ReSharper disable UnusedMember.Global

namespace TestPlugin
{
	public class Shit
	{
	}

	//[Plugin]
	//public class SimplePlugin : Plugin
	//{
	//	[PacketHandler, Receive]
	//	public Package HandleIncoming(McpeMovePlayer packet)
	//	{
	//		return packet; // Process
	//	}

	//	[PacketHandler, Send]
	//	public Package HandleOutgoing(McpeMovePlayer packet)
	//	{
	//		return packet; // Send
	//	}
	//}

	//[Plugin]
	//public class StartupPlugin : Plugin, IStartup
	//{
	//	private static readonly ILog Log = LogManager.GetLogger(typeof (StartupPlugin));

	//	/// <summary>
	//	/// Startup class for MiNET. Example sets the user and role managers and stores 
	//	/// for the application.
	//	/// </summary>
	//	/// <param name="server"></param>
	//	public void Configure(MiNetServer server)
	//	{
	//		server.UserManager = new UserManager<User>(new DefaultUserStore());
	//		server.RoleManager = new RoleManager<Role>(new DefaultRoleStore());
	//		//server.UserManager.PasswordHasher = new CustomPasswordHasher();
	//		Log.Info("Executed startup successfully. Replaced identity managment.");
	//	}
	//}

	//public class CustomPasswordHasher : IPasswordHasher
	//{
	//	public string HashPassword(string password)
	//	{
	//		return password; //return password as is
	//	}

	//	public PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string providedPassword)
	//	{
	//		if (hashedPassword.Equals(providedPassword))
	//		{
	//			return PasswordVerificationResult.Success;
	//		}
	//		return PasswordVerificationResult.Failed;
	//	}
	//}


	//[Plugin(PluginName = "Test", Description = "A Test Plugin for MiNET", PluginVersion = "1.0", Author = "MiNET Team")]
	//public class TestPlugin : Plugin
	//{
	//	private static readonly ILog Log = LogManager.GetLogger(typeof (TestPlugin));

	//	protected override void OnEnable()
	//	{
	//		Log.Debug("Succesfully enabled test plugin.");
	//	}

	//	public override void OnDisable()
	//	{
	//		Log.Debug("Succesfully disabled test plugin.");
	//	}

	//	public Package MinimalHandler(McpeMovePlayer packet, Player source)
	//	{
	//		return packet; // Handled
	//	}

	//	[PacketHandler]
	//	public Package HandlePacketTest(McpeMovePlayer packet, Player source)
	//	{
	//		Log.DebugFormat("Player (x,y,z): {0},{1},{2}", packet.x, packet.y, packet.z);
	//		return packet; // Handled
	//	}

	//	[PacketHandler]
	//	public Package HandlePacketNoAbort(McpeMovePlayer packet, Player source)
	//	{
	//		Log.DebugFormat("Player (x,y,z): {0},{1},{2}", packet.x, packet.y, packet.z);
	//		return packet;
	//	}

	//	[PacketHandler(PacketType = typeof (McpeSetHealth))]
	//	[Receive]
	//	public Package HandleSetHealthPacket(McpeSetHealth packet, Player source)
	//	{
	//		return packet; // Send
	//	}

	//	[PacketHandler]
	//	[Send]
	//	public Package HandleSetHealthPacketNoSpec(McpeSetHealth packet, Player source)
	//	{
	//		return packet; // Send
	//	}

	//	[PacketHandler]
	//	[Send]
	//	public Package HandleSendMove(McpeMovePlayer packet, Player source)
	//	{
	//		return packet;
	//	}

	//	[Send]
	//	public Package HandleSetHealthPacketSimple(McpeSetHealth packet, Player source)
	//	{
	//		return packet; // Send
	//	}

	//	[PacketHandler(PacketType = typeof (McpeSetHealth))]
	//	[Send]
	//	public void HandleSendPacketTest(Package packet, Player source)
	//	{
	//		var healthPacket = packet as McpeSetHealth;
	//		if (healthPacket != null)
	//		{
	//			Log.DebugFormat("Username: {0}, Health: {1}", source.Username, healthPacket.health);
	//		}
	//	}

	//	//[Command(Command = "Boom", Permission = "", Description = "", Usage = "")]

	//	[Command]
	//	[Description("The smallest command implementation possible.")]
	//	public void Min()
	//	{
	//		Log.Info("Minimal command executed.");
	//	}

	//	[Command]
	//	[Authorize(Roles = "ADMIN, OP")]
	//	[Description("A command with autorization check. Allows user gurun, administrators and operators access")]
	//	public void Secured()
	//	{
	//	}

	//	[Command]
	//	[Description("Generic command with args.")]
	//	public void Generic(Player player, string[] args)
	//	{
	//		Log.Info("Generic command executed.");
	//	}

	//	[Command]
	//	[Description("Creates an explosion with the specified radius.")]
	//	public void Boom(Player player, int radius = 10)
	//	{
	//		new Explosion(
	//			player.Level,
	//			new BlockCoordinates((int) player.KnownPosition.X, (int) player.KnownPosition.Y, (int) player.KnownPosition.Z),
	//			radius).Explode();
	//	}

	//	[Command]
	//	public void DeOp(Player player, string username)
	//	{
	//		Log.Info("Deop: " + username);
	//	}

	//	[Command]
	//	public void SetHome(Player player, string username)
	//	{
	//		Log.Info("Set Home: " + username);
	//	}

	//	[Command]
	//	public void SetHome(Player player, string username, float x, float y, float z)
	//	{
	//		Log.Info(string.Format("Set {0} home:  {1},{2},{3}", username, x, y, z));
	//	}
	//}
}