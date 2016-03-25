using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiNET.Worlds;

namespace MiNET.Entities.Vehicles
{
	public abstract class Vechicle : Entity
	{

		protected Vechicle(EntityType type, Level level) : base((int)type, level)
		{
			
		}

	}
}
