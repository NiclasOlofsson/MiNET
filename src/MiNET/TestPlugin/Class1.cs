using System;
using MiNET.API;

namespace TestPlugin
{
    [Plugin(PluginName = "Test plugin", Description = "Nice plugin for MiNET", Author = "The MiNET Team", PluginVersion = "1.0 Alpha")]
    public class Main : MiNETPlugin
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
}
