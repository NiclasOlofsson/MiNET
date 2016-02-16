using System;
using System.Collections.Generic;
using System.Linq;
using MiNET.Utils;
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
				GameMode gameMode = Config.GetProperty("GameMode", GameMode.Survival);
				Difficulty difficulty = Config.GetProperty("Difficulty", Difficulty.Peaceful);
				int viewDistance = Config.GetProperty("ViewDistance", 250);

				IWorldProvider _worldProvider = null;

				switch (Config.GetProperty("WorldProvider", "flat").ToLower().Trim())
				{
					case "flat":
					case "flatland":
						_worldProvider = new FlatlandWorldProvider();
						break;
					case "cool":
						_worldProvider = new CoolWorldProvider();
						break;
					case "experimental":
						_worldProvider = new ExperimentalWorldProvider();
						break;
					case "anvil":
						_worldProvider = new AnvilWorldProvider();
						break;
					default:
						_worldProvider = new FlatlandWorldProvider();
						break;
				}

				level = new Level(name, _worldProvider, gameMode, difficulty, viewDistance);
				level.Initialize();
				Levels.Add(level);
			}

			return level;
		}

		public void RemoveLevel(Level level)
		{
			if(Levels.Contains(level))
			{
				Levels.Remove(level);
			}

			level.Close();
		}


	}
}