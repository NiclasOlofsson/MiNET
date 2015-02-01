using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Net.Common;

namespace MiNET.CommandHandler
{
	class BoomCommand : ICommandHandler
	{
		public string Command
		{
			get { return "boom"; }
		}

		public string Description
		{
			get { return "Creates an explosion from specified argument."; }
		}

		public string Usage
		{
			get { return "/boom <Radius>"; }
		}

		public bool RequireOperator
		{
			get { return false; }
		}

		public bool Execute(Player player, string[] arguments)
		{
			if (arguments.Length > 0)
			{
				new Explosion(player.Level,
					new Coordinates3D((int) player.KnownPosition.X, (int) player.KnownPosition.Y, (int) player.KnownPosition.Z),
					(float)Convert.ToDouble(arguments[0])).Explode();
				return true;
			}
			return false;
		}
	}
}
