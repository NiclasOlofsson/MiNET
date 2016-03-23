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

		protected Vechicle(byte id, Level level) : base(id, level)
		{
			
		}

	}
}
