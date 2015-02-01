using System;
using MiNET;
using MiNET.API;
using MiNET.PluginSystem.Attributes;

namespace TestPlugin
{
    [Plugin(PluginName = "Test plugin", Description = "Nice plugin for MiNET", Author = "The MiNET Team", PluginVersion = "1.0 Alpha")]
    public partial class Main : MiNETPlugin
    {
        public override void OnEnable()
        {
            Console.WriteLine("Succesfully enabled test plugin :-)");
        }
        public override void OnDisable()
        {
            Console.WriteLine("Succesfully disabled test plugin :-)");
        }
    }

	public partial class Main
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
	}
}
