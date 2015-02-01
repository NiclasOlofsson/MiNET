using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiNET.CommandHandler
{
	interface ICommandHandler
	{
		string Command { get; }
		string Description { get; }
		string Usage { get; }
		string Permission { get; }
		bool Execute(Player player, string[] arguments);

	}
}
