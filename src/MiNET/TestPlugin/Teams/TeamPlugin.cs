using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using MiNET;
using MiNET.BlockEntities;
using MiNET.Blocks;
using MiNET.Plugins;
using MiNET.Plugins.Attributes;
using MiNET.Utils;
using MiNET.Worlds;

namespace TestPlugin.Teams
{
	[Plugin(PluginName = "Team", Description = "A simple game engine with teams", PluginVersion = "1.0", Author = "MiNET Team")]
	public class TeamPlugin : IStartup, IPlugin
	{
		private MiNetServer _server;
		private PluginContext _context;

		public void Configure(MiNetServer server)
		{
			_server = server;
			BlockFactory.CustomBlockFactory = new PluginBlockFactory(new GameManager());
		}


		public void OnEnable(PluginContext context)
		{
			_context = context;
		}

		public void OnDisable()
		{
		}
	}

	public class GameManager
	{
		private Timer _timer;
		public Dictionary<string, Game> Games { get; set; }
		public Dictionary<Sign, Level> Signs { get; set; }

		public GameManager()
		{
			Games = new Dictionary<string, Game>();
			Signs = new Dictionary<Sign, Level>();

			_timer = new Timer(delegate
			{
				foreach (var pair in Signs)
				{
					Sign signEntity = pair.Key;
					Level level = pair.Value;

					Game game = Games[signEntity.Text1];
					signEntity.Text2 = string.Format("[{0}]", GetPlayerCount(game));
					level.SetBlockEntity(signEntity);
				}
			}, null, 1000, 1000);
		}

		public int GetPlayerCount(Game game)
		{
			return game.Players.Count;
		}

		public void Register(Sign signEntity, Level level)
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
				Games.Add(player.Level.LevelId, new Game
				{
					Name = player.Level.LevelId,
					Level = player.Level
				});
			}

			if (!Games.ContainsKey(world))
			{
				Level gameLevel;

				if (world.Equals("1"))
				{
					gameLevel = new Level(world, new AnvilWorldProvider(@"D:\Development\Worlds\MiniGames\Castle Wars"));
				}
				else if (world.Equals("2"))
				{
					gameLevel = new Level(world, new AnvilWorldProvider(@"D:\Development\Worlds\MiniGames\King of the hill"));
				}
				else
				{
					gameLevel = new Level(world, new FlatlandWorldProvider());
				}

				gameLevel.Initialize();
				var spawn = gameLevel.SpawnPoint;
				gameLevel.SpawnPoint = new BlockCoordinates
				{
					X = spawn.X,
					Y = spawn.Y + 20,
					Z = spawn.Z,
				};

				Games.Add(world, new Game {Name = world, Level = gameLevel});
			}

			Game currentGame = Games[player.Level.LevelId];
			if (currentGame.Players.Contains(player)) currentGame.Players.Remove(player);

			Game game = Games[world];
			game.Players.Add(player);
			player.SpawnLevel(game.Level);
			game.Level.BroadcastTextMessage(string.Format("{0} joined game <{1}>.", player.Username, game.Name));

			return game;
		}
	}

	public class Game
	{
		public string Name { get; set; }
		public List<Player> Players { get; set; }
		public Level Level { get; set; }

		public Game()
		{
			Players = new List<Player>();
		}
	}

	public enum GameMode
	{
		PvP,
		Team
	}

	public class GameInfo
	{
		public string Name { get; set; }
		public Type LevelType { get; set; }
		public GameMode GameMode { get; set; }
	}
}