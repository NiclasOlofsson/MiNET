using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiNET.API;

namespace TestPlugin
{
    public class Main : MiNETPlugin
    {
        public override string PluginName { get { return "Test Plugin"; } }
        public override string PluginDescription { get { return "A test plugin for MiNET"; } }
        public override string Author { get { return "The MiNET team"; } }
        public override string PluginVersion { get { return "1.0"; } }
        public override void OnEnable()
        {
            Console.WriteLine("MiNET Test plugin is enabled with lots of happyness.");
        }
        public override void OnDisable()
        {
            Console.WriteLine("MiNET Test plugin was disabled with lots of crying.");
        }
    }
}
