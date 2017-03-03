using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET
{
    public class LevelManager
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof (LevelManager));

        public List<Level> Levels { get; set; } = new List<Level>();

        public EntityManager EntityManager { get; set; } = new EntityManager();

        public LevelManager()
        {
        }

        public virtual Level GetLevel(Player player, string name)
        {
            Level level = Levels.FirstOrDefault(l => l.LevelId.Equals(name, StringComparison.InvariantCultureIgnoreCase));
            if (level == null)
            {
                GameMode gameMode = Config.GetProperty("GameMode", GameMode.Survival);
                Difficulty difficulty = Config.GetProperty("Difficulty", Difficulty.Normal);
                int viewDistance = Config.GetProperty("ViewDistance", 11);
                bool enableBlockTicking = Config.GetProperty("EnableBlockTicking", false);

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
                        worldProvider = new AnvilWorldProvider() {MissingChunkProvider = new FlatlandWorldProvider()};
                        break;
                    default:
                        worldProvider = new FlatlandWorldProvider();
                        break;
                }

                level = new Level(name, worldProvider, EntityManager, gameMode, difficulty, viewDistance) {EnableBlockTicking = enableBlockTicking};
                level.Initialize();

				if(Config.GetProperty("CalculateLights", false))
				{
					{
						AnvilWorldProvider wp = level._worldProvider as AnvilWorldProvider;
						if (wp != null)
						{
							//wp.PruneAir();
							//wp.MakeAirChunksAroundWorldToCompensateForBadRendering();

							SkyLightCalculations.Calculate(level);

							Stopwatch sw = new Stopwatch();

							int count = wp.LightSources.Count;
							sw.Restart();
							RecalculateLight(level, wp);

							var chunkCount = wp._chunkCache.Where(chunk => chunk.Value != null).ToArray().Length;
							Log.Debug($"Recalc light for {chunkCount} chunks, {chunkCount*16*16*256} blocks and {count} light sources. Time {sw.ElapsedMilliseconds}ms");
						}
					}

					{
						FlatlandWorldProvider wp = level._worldProvider as FlatlandWorldProvider;
						if (wp != null)
						{
							SkyLightCalculations.Calculate(level);
						}
					}
					
				}
				Levels.Add(level);

                OnLevelCreated(new LevelEventArgs(null, level));
            }

            return level;
        }

        public void RecalculateLight(Level level, AnvilWorldProvider wp)
        {
            while (wp.LightSources.Count > 0)
            {
                var block = wp.LightSources.Dequeue();
                block = level.GetBlock(block.Coordinates);
                BlockLightCalculations.Calculate(level, block);
            }
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
        private static readonly ILog Log = LogManager.GetLogger(typeof (SpreadLevelManager));

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

            var level = new Level(name, worldProvider, EntityManager, gameMode, difficulty, viewDistance);
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