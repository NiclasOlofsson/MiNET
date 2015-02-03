using MiNET;
using MiNET.Entities;
using MiNET.PluginSystem.Attributes;

namespace TestPlugin
{
	public partial class TestPlugin
	{
		[Command("tnt", "MiNET.tnt", "Spawn primed TNT at your location.", "/tnt")]
		public void tntCMD(Player source, string[] arguments)
		{
			new PrimedTnt(source.Level) {KnownPosition = source.KnownPosition, Fuse = 20}.SpawnEntity();
		}

		[Command("nuke", "MiNET.nuke", "Nuke your world <3", "/nuke")]
		public void nukeCMD(Player source, string[] arguments)
		{
			for (int x = -10; x <= 10; x += 5)
			{
				for (int z = -10; z <= 10; z += 5)
				{
					PlayerPosition3D tntLoc = new PlayerPosition3D(source.KnownPosition.GetCoordinates3D().X + x,
						source.KnownPosition.GetCoordinates3D().Y + 10, source.KnownPosition.GetCoordinates3D().Z + z);
					new PrimedTnt(source.Level) {KnownPosition = tntLoc, Fuse = 25}.SpawnEntity();
				}
			}
		}
	}
}
