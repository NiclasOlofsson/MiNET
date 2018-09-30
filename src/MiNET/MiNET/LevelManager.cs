#region LICENSE

// The contents of this file are subject to the Common Public Attribution
// License Version 1.0. (the "License"); you may not use this file except in
// compliance with the License. You may obtain a copy of the License at
// https://github.com/NiclasOlofsson/MiNET/blob/master/LICENSE. 
// The License is based on the Mozilla Public License Version 1.1, but Sections 14 
// and 15 have been added to cover use of software over a computer network and 
// provide for limited attribution for the Original Developer. In addition, Exhibit A has 
// been modified to be consistent with Exhibit B.
// 
// Software distributed under the License is distributed on an "AS IS" basis,
// WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License for
// the specific language governing rights and limitations under the License.
// 
// The Original Code is MiNET.
// 
// The Original Developer is the Initial Developer.  The Initial Developer of
// the Original Code is Niclas Olofsson.
// 
// All portions of the code written by Niclas Olofsson are Copyright (c) 2014-2018 Niclas Olofsson. 
// All Rights Reserved.

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using MiNET.Config;
using MiNET.Config.Contracts;
using MiNET.Utils;
using MiNET.Worlds;

namespace MiNET
{
	public class LevelManager
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof(LevelManager));
		protected static readonly IMiNETConfiguration Config = ConfigurationProvider.MiNetConfiguration;

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
				GameMode gameMode = Config.World.GameMode;
				Difficulty difficulty = Config.World.Difficulty;
				int viewDistance = Config.World.ViewDistance;

				IWorldProvider worldProvider = null;

				switch (Config.World.WorldProvider.ToLower().Trim())
				{
					case "cool":
						worldProvider = new CoolWorldProvider();
						break;
					case "experimental":
						worldProvider = new ExperimentalWorldProvider();
						break;
					case "anvil":
					case "flat":
					case "flatland":
					default:
						worldProvider = new AnvilWorldProvider
						{
							MissingChunkProvider = new SuperflatGenerator(Dimension.Overworld),
							ReadSkyLight = !Config.World.CalculateLights,
							ReadBlockLight = !Config.World.CalculateLights,
						};
						break;
				}

				level = new Level(this, name, worldProvider, EntityManager, gameMode, difficulty, viewDistance)
				{
					EnableBlockTicking = Config.World.EnableBlockTicking,
					EnableChunkTicking = Config.World.EnableChunkTicking,
					SaveInterval = Config.World.SaveInterval,
					UnloadInterval = Config.World.UnloadInterval,

					DrowningDamage = Config.GameRules.DrowningDamage,
					CommandblockOutput = Config.GameRules.CommandBlockOutput,
					DoTiledrops = Config.GameRules.DoTiledrops,
					DoMobloot = Config.GameRules.DoMobloot,
					KeepInventory = Config.GameRules.KeepInventory,
					DoDaylightcycle = Config.GameRules.DoDaylightcycle,
					DoMobspawning = Config.GameRules.DoMobSpawning,
					DoEntitydrops = Config.GameRules.DoEntitydrops,
					DoFiretick = Config.GameRules.DoFiretick,
					DoWeathercycle = Config.GameRules.DoWeathercycle,
					Pvp = Config.GameRules.Pvp,
					Falldamage = Config.GameRules.Falldamage,
					Firedamage = Config.GameRules.Firedamage,
					Mobgriefing = Config.GameRules.Mobgriefing,
					ShowCoordinates = Config.GameRules.ShowCoordinates,
					NaturalRegeneration = Config.GameRules.NaturalRegeneration,
					TntExplodes = Config.GameRules.TntExplodes,
					SendCommandfeedback = Config.GameRules.SendCommandfeedback,
					RandomTickSpeed = Config.GameRules.RandomTickSpeed,
				};
				level.Initialize();

				//if (_config.World.CalculateLights)
				//{
				//	{
				//		AnvilWorldProvider wp = level.WorldProvider as AnvilWorldProvider;
				//		if (wp != null)
				//		{
				//			wp.Locked = true;
				//			//			wp.PruneAir();
				//			//			wp.MakeAirChunksAroundWorldToCompensateForBadRendering();
				//			Stopwatch sw = new Stopwatch();

				//			var chunkCount = 0;
				//			sw.Restart();
				//			SkyLightCalculations.Calculate(level);
				//			sw.Stop();
				//			chunkCount = wp._chunkCache.Where(chunk => chunk.Value != null).ToArray().Length;
				//			Log.Debug($"Recalculated sky light for {chunkCount} chunks, {chunkCount * 16 * 16 * 256} blocks. Time {sw.ElapsedMilliseconds}ms");

				//			int count = wp.LightSources.Count;
				//			sw.Restart();
				//			RecalculateBlockLight(level, wp);

				//			chunkCount = wp._chunkCache.Where(chunk => chunk.Value != null).ToArray().Length;
				//			Log.Debug($"Recalculated sky and block light for {chunkCount} chunks, {chunkCount * 16 * 16 * 256} blocks and {count} light sources. Time {sw.ElapsedMilliseconds}ms. Touched {BlockLightCalculations.touches}");

				//			wp.Locked = false;
				//		}
				//	}
				//}

				Levels.Add(level);

				OnLevelCreated(new LevelCancelEventArgs(null, level));
			}

			return level;
		}

		public static void RecalculateBlockLight(Level level, AnvilWorldProvider wp)
		{
			var sources = wp.LightSources.ToArray();
			Parallel.ForEach(sources, block => { BlockLightCalculations.Calculate(level, block.Coordinates); });
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

		public virtual Level GetDimension(Level level, Dimension dimension)
		{
			if (dimension == Dimension.Overworld) throw new Exception($"Can not get level for '{dimension}' from the LevelManager");
			if (dimension == Dimension.Nether && !level.WorldProvider.HaveNether()) return null;
			if (dimension == Dimension.TheEnd && !level.WorldProvider.HaveTheEnd()) return null;

			AnvilWorldProvider overworld = level.WorldProvider as AnvilWorldProvider;
			if (overworld == null) return null;

			var worldProvider = new AnvilWorldProvider(overworld.BasePath)
			{
				ReadBlockLight = overworld.ReadBlockLight,
				ReadSkyLight = overworld.ReadSkyLight,
				Dimension = dimension,
				MissingChunkProvider = new SuperflatGenerator(dimension),
			};

			Level newLevel = new Level(level.LevelManager, level.LevelId + "_" + dimension, worldProvider, EntityManager, level.GameMode, level.Difficulty, level.ViewDistance)
			{
				OverworldLevel = level,
				Dimension = dimension,
				EnableBlockTicking = level.EnableBlockTicking,
				EnableChunkTicking = level.EnableChunkTicking,
				SaveInterval = level.SaveInterval,
				UnloadInterval = level.UnloadInterval,

				DrowningDamage = level.DrowningDamage,
				CommandblockOutput = level.CommandblockOutput,
				DoTiledrops = level.DoTiledrops,
				DoMobloot = level.DoMobloot,
				KeepInventory = level.KeepInventory,
				DoDaylightcycle = level.DoDaylightcycle,
				DoMobspawning = level.DoMobspawning,
				DoEntitydrops = level.DoEntitydrops,
				DoFiretick = level.DoFiretick,
				DoWeathercycle = level.DoWeathercycle,
				Pvp = level.Pvp,
				Falldamage = level.Falldamage,
				Firedamage = level.Firedamage,
				Mobgriefing = level.Mobgriefing,
				ShowCoordinates = level.ShowCoordinates,
				NaturalRegeneration = level.NaturalRegeneration,
				TntExplodes = level.TntExplodes,
				SendCommandfeedback = level.SendCommandfeedback,
				RandomTickSpeed = level.RandomTickSpeed,
			};

			newLevel.Initialize();

			//if (_config.World.CalculateLights)
			//{
			//	worldProvider.Locked = true;
			//	SkyLightCalculations.Calculate(newLevel);

			//	int count = worldProvider.LightSources.Count;
			//	Log.Debug($"Recalculating block light for {count} light sources.");
			//	Stopwatch sw = new Stopwatch();
			//	sw.Start();
			//	RecalculateBlockLight(newLevel, worldProvider);

			//	var chunkCount = worldProvider._chunkCache.Where(chunk => chunk.Value != null).ToArray().Length;
			//	Log.Debug($"Recalc sky and block light for {chunkCount} chunks, {chunkCount * 16 * 16 * 256} blocks and {count} light sources. Time {sw.ElapsedMilliseconds}ms");
			//	worldProvider.Locked = false;
			//}

			return newLevel;
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
			GameMode gameMode = Config.World.GameMode;
			Difficulty difficulty = Config.World.Difficulty;
			int viewDistance = Config.World.ViewDistance;

			IWorldProvider worldProvider = null;
			worldProvider = provider ?? new AnvilWorldProvider {MissingChunkProvider = new SuperflatGenerator(Dimension.Overworld)};

			var level = new Level(this, name, worldProvider, EntityManager, gameMode, difficulty, viewDistance);
			level.Initialize();

			OnLevelCreated(new LevelCancelEventArgs(null, level));

			return level;
		}

		public event EventHandler<LevelCancelEventArgs> LevelCreated;

		protected virtual void OnLevelCreated(LevelCancelEventArgs e)
		{
			LevelCreated?.Invoke(this, e);
		}
	}
}