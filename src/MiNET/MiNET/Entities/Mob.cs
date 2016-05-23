using System;
using MiNET.Blocks;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET.Entities
{
	public class Mob : Entity
	{
		public Mob(int entityTypeId, Level level) : base(entityTypeId, level)
		{
			Width = Length = 0.6;
			Height = 1.80;
		}

		public Mob(EntityType mobTypes, Level level) : this((int) mobTypes, level)
		{
		}

	}
}