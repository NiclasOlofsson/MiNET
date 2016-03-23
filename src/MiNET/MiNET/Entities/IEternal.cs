using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiNET.Entities
{
	interface IEternal
	{
		void Praise(Player player);

		int Minions { get; }
	}
}
