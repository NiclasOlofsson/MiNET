﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiNET.Worlds;

namespace MiNET.Entities.Hostile
{
	public class Witch : HostileMob
	{
		public Witch(Level level) : base((int) EntityType.Witch, level)
		{
			Width = Length = 0.6;
			Height = 1.8;
			HealthManager.MaxHealth = 260;
			HealthManager.ResetHealth();
		}
	}
}
