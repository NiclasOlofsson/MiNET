using System;
using System.Collections.Generic;
using System.Linq;
using MiNET.Worlds;

namespace MiNET
{
	public class LevelManager
	{
		public List<Level> Levels { get; set; }

		public LevelManager()
		{
			Levels = new List<Level>();
		}

		public virtual Level GetLevel(Player player, string name)
		{
			Level level = Levels.FirstOrDefault(l => l.LevelId.Equals(name, StringComparison.InvariantCultureIgnoreCase));
			if (level == null)
			{
				level = new Level(name);
				level.Initialize();
				Levels.Add(level);
			}

			return level;
		}
	}
}