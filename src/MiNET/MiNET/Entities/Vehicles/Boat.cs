using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiNET.Items;
using MiNET.Worlds;

namespace MiNET.Entities.Vehicles
{
	public class Boat : Vechicle
	{
		public Boat(Level level) : base((int) EntityType.Boat, level)
		{
			
		}

		public override Item[] GetDrops()
		{
			return new[]
			{
				ItemFactory.GetItem(333)
			};
		}
	}
}
