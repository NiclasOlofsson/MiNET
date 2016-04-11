using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiNET.Worlds;

namespace MiNET.Entities.Vehicles
{
	public abstract class Vehicle : Entity
	{

		protected Vehicle(EntityType type, Level level) : base((int)type, level)
		{
			
		}

	}
}
