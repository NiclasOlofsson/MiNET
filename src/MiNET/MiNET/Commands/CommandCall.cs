using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiNET.Commands
{
    public class CommandCall
    {
        public string Label { get; }

        public Player Sender { get; }

        public List<String> Args { get; }

        public CommandCall(string label, Player sender, List<String> args)
        {
            Label = label;
            Sender = sender;
            Args = args;
        }
    }
}
