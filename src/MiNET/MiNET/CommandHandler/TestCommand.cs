using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Common;

namespace MiNET.CommandHandler
{
	class TestCommand : ICommandHandler
	{

		public string Command
		{
			get { return "test"; }
		}

		public string Description
		{
			get { return "A MiNET Test command."; }
		}

		public string Usage { get { return "/test"; }}

		public bool RequireOperator
		{
			get { return false; }
		}

		public bool Execute(Player player, string[] arguments)
		{
			player.Level.BroadcastTextMessage("Hello World", player);
			return true;
		}
	}
}
