using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiNET;
using MiNET.PluginSystem.Attributes;

namespace TestPlugin
{
	public partial class TestPlugin
	{
		[Command("testplugin", "MiNET.debug", "A plugin test command", "/testplugin")]
		public void TestCMD(Player source, string[] arguments)
		{
			source.SendMessage("Execution of TestPLUGIN :D");
		}

		[Command("testplugin2", "MiNET.debug", "A second plugin test command", "/testplugin2")]
		public void TestCMD2(Player source, string[] arguments)
		{
			source.SendMessage("Execution of TestPLUGIN2 :D");
		}

		[Command("tnt", "MiNET.tnt", "A second plugin test command", "/testplugin2")]
		public void tntCMD(Player source, string[] arguments)
		{
			new Explosion().SpawnTNT(source.KnownPosition.GetCoordinates3D(), source.Level);
		}
	}
}
