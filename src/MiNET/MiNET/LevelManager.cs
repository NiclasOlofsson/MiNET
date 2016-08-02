using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using log4net;
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
				int viewDistance = Config.GetProperty("ViewDistance", 11);

				IWorldProvider worldProvider = null;

				switch (Config.GetProperty("WorldProvider", "flat").ToLower().Trim())
				{
					case "flat":
					case "flatland":
						worldProvider = new FlatlandWorldProvider();
						break;
					case "cool":
						worldProvider = new CoolWorldProvider();
						break;
					case "experimental":
						worldProvider = new ExperimentalWorldProvider();
						break;
					case "anvil":
						worldProvider = new AnvilWorldProvider();
						break;
					default:
						worldProvider = new FlatlandWorldProvider();
						break;
				}

				level = new Level(name, worldProvider, gameMode, difficulty, viewDistance);
				level.Initialize();
				Levels.Add(level);

				OnLevelCreated(new LevelEventArgs(null, level));

			}

			return level;
		}

		public void RemoveLevel(Level level)
		{
			if (Levels.Contains(level))
			{
				Levels.Remove(level);
			}

			level.Close();
		}

		public event EventHandler<LevelEventArgs> LevelCreated;

		protected virtual void OnLevelCreated(LevelEventArgs e)
		{
			LevelCreated?.Invoke(this, e);
		}
	}

	public class SpreadLevelManager : LevelManager
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(SpreadLevelManager));

		private readonly int _numberOfLevels;

		public SpreadLevelManager(int numberOfLevels)
		{
			Log.Warn($"Creating and caching {numberOfLevels} levels");

			//Level template = CreateLevel("Default", null);

			_numberOfLevels = numberOfLevels;
			Levels = new List<Level>();
			Parallel.For(0, numberOfLevels, i =>
			{
				var name = "Default" + i;
				//Levels.Add(CreateLevel(name, template._worldProvider));
				Levels.Add(CreateLevel(name, null));
				Log.Warn($"Created level {name}");
			});

			Log.Warn("DONE Creating and caching worlds");
		}

		public override Level GetLevel(Player player, string name)
		{
			Random rand = new Random();

			return Levels[rand.Next(0, _numberOfLevels)];
		}

		public virtual Level CreateLevel(string name, IWorldProvider provider)
		{
			GameMode gameMode = Config.GetProperty("GameMode", GameMode.Survival);
			Difficulty difficulty = Config.GetProperty("Difficulty", Difficulty.Peaceful);
			int viewDistance = Config.GetProperty("ViewDistance", 11);

			IWorldProvider worldProvider = null;
			worldProvider = provider ?? new FlatlandWorldProvider();

			var level = new Level(name, worldProvider, gameMode, difficulty, viewDistance);
			level.Initialize();

			OnLevelCreated(new LevelEventArgs(null, level));

			return level;
		}

		public event EventHandler<LevelEventArgs> LevelCreated;

		protected virtual void OnLevelCreated(LevelEventArgs e)
		{
			LevelCreated?.Invoke(this, e);
		}
	}

}