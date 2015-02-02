using System;
using MiNET;
using MiNET.API;
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
    }
}
