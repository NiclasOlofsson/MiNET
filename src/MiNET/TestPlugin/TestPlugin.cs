#region LICENSE

// The contents of this file are subject to the Common Public Attribution
// License Version 1.0. (the "License"); you may not use this file except in
// compliance with the License. You may obtain a copy of the License at
// https://github.com/NiclasOlofsson/MiNET/blob/master/LICENSE. 
// The License is based on the Mozilla Public License Version 1.1, but Sections 14 
// and 15 have been added to cover use of software over a computer network and 
// provide for limited attribution for the Original Developer. In addition, Exhibit A has 
// been modified to be consistent with Exhibit B.
// 
// Software distributed under the License is distributed on an "AS IS" basis,
// WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License for
// the specific language governing rights and limitations under the License.
// 
// The Original Code is MiNET.
// 
// The Original Developer is the Initial Developer.  The Initial Developer of
// the Original Code is Niclas Olofsson.
// 
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2018 Niclas Olofsson. 
// All Rights Reserved.

#endregion

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