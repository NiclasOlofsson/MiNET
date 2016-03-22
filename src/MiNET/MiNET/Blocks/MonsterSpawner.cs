using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiNET.Items;

namespace MiNET.Blocks
{
	public class MonsterSpawner : Block
	{
		public MonsterSpawner() : base(52)
		{
			IsTransparent = true; // Doesn't block light
			LightLevel = 1;
			BlastResistance = 25;
			Hardness = 5;
		}

		public override Item GetDrops()
		{
			return null;
		}
	}
}
