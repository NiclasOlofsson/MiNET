using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Threading;
using MiNET;
using MiNET.BlockEntities;
using MiNET.Utils;
using MiNET.Worlds;
using TestPlugin.Teams.Model;
using GameMode = TestPlugin.Teams.Model.GameMode;

namespace TestPlugin.Teams
{
	public class GameManager
	{
		private Timer _timer;
		public Dictionary<string, Game> Games { get; set; }
		public Dictionary<string, GameSettings> GameTemplates { get; set; }
		public Dictionary<Sign, Level> Signs { get; set; }

		public GameManager()
		{
			Games = new Dictionary<string, Game>();
			GameTemplates = new Dictionary<string, GameSettings>
			{
				{
					"1", new GameSettings
					{
						Name = "1",
						LevelType = typeof (Level),
						IsTimeLimited = true,
						TimeLimit = 30000L,
						AllowRespwan = true,
						ArenaWorldPath = @"D:\Development\Worlds\MiniGames\Castle Wars",
						GameMode = GameMode.TvT,
						MaxPlayers = Int32.MaxValue,
						MinPlayers = 1,
						SpawnLocations = new List<SpawnCoordinates>
						{
							new SpawnCoordinates(0, 0, 0, 0),
							new SpawnCoordinates(0, 0, 0, 0),
							new SpawnCoordinates(0, 0, 0, 0),
						}
					}
				},
				{
					"2", new GameSettings
					{
						Name = "2",
						LevelType = typeof (Level),
						IsTimeLimited = true,
						TimeLimit = 30000L,
						AllowRespwan = true,
						ArenaWorldPath = @"D:\Development\Worlds\MiniGames\King of the hill",
						GameMode = GameMode.TvT,
						MaxPlayers = Int32.MaxValue,
						MinPlayers = 1,
					}
				},
				{
					"Castle Wars", new GameSettings
					{
						Name = "Castle Wars",
						LevelType = typeof (Level),
						IsTimeLimited = true,
						TimeLimit = 30000L,
						AllowRespwan = true,
						ArenaWorldPath = @"D:\Development\Worlds\MiniGames\Castle Wars",
						GameMode = GameMode.TvT,
						MaxPlayers = Int32.MaxValue,
						MinPlayers = 1,
					}
				},
				{
					"Blockade", new GameSettings
					{
						Name = "Blockade",
						LevelType = typeof (Level),
						IsTimeLimited = true,
						TimeLimit = 30000L,
						AllowRespwan = true,
						ArenaWorldPath = @"D:\Development\Worlds\MiniGames\King of the hill",
						GameMode = GameMode.TvT,
						MaxPlayers = Int32.MaxValue,
						MinPlayers = 1,
					}
				}
			};


			string pluginDirectory = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);

			using (FileStream stream = new FileStream(Path.Combine(pluginDirectory, "game-config.json"), FileMode.Create))
			{
				DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof (List<GameSettings>));
				ser.WriteObject(stream, GameTemplates.Values.ToList());
			}

			//using (FileStream stream = new FileStream(Path.Combine(pluginDirectory, "game-config.json"), FileMode.Open))
			//{
			//	DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof (List<GameSettings>));
			//	List<GameSettings> readConfig = (List<GameSettings>) ser.ReadObject(stream);
			//	GameTemplates = readConfig.ToDictionary(settings => settings.Name);
			//}

			Signs = new Dictionary<Sign, Level>();

			_timer = new Timer(delegate
			{
				foreach (var pair in Signs)
				{
					Sign signEntity = pair.Key;
					Level level = pair.Value;

					Game game = Games[signEntity.Text1];
					signEntity.Text2 = string.Format("[{0}]", game.GetPlayerCount());
					signEntity.Text3 = string.Format("{0}", game.State);
					level.SetBlockEntity(signEntity);
				}
			}, null, 1000, 1000);
		}

		public void RegisterSign(Sign signEntity, Level level)
		{
			if (Signs.FirstOrDefault(pair => pair.Key.Coordinates == signEntity.Coordinates && pair.Value == level).Key == null)
			{
				Signs[signEntity] = level;
			}
		}

		public Game Join(Player player, string world)
		{
			if (!Games.ContainsKey(player.Level.LevelId))
			{
				Games.Add(player.Level.LevelId, new Game(this)
				{
					Name = player.Level.LevelId,
					Level = player.Level,
					State = GameState.Undefined
				});
			}

			if (!Games.ContainsKey(world))
			{
				if (!GameTemplates.ContainsKey(world)) return null;

				GameSettings settings = GameTemplates[world];

				Level gameLevel = new Level(settings.Name, new AnvilWorldProvider(settings.ArenaWorldPath));
				gameLevel.Initialize();

				var spawn = gameLevel.SpawnPoint;
				if (!world.Equals("Default"))
				{
					gameLevel.SpawnPoint = new BlockCoordinates
					{
						X = spawn.X,
						Y = spawn.Y + 20,
						Z = spawn.Z,
					};
				}

				Games.Add(world, new Game(this) {Name = world, Level = gameLevel, State = GameState.WaitingToStart});
			}

			Game currentGame = Games[player.Level.LevelId];
			Game game = Games[world];

			if (game.State == GameState.Started || game.State == GameState.Finshed)
			{
				return currentGame;
			}

			currentGame.RemovePlayer(player);

			game.AddPlayer(player);
			player.SpawnLevel(game.Level);
			game.Level.BroadcastTextMessage(string.Format("{0} joined game <{1}>.", player.Username, game.Name));

			return game;
		}
	}
}