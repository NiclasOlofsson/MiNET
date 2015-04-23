using System;
using System.Collections.Generic;
using System.Diagnostics;
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
					signEntity.Text2 = string.Format("[{0}]", game.GetPlayerCount());
					signEntity.Text3 = string.Format("{0}", game.State);
					level.SetBlockEntity(signEntity);
				}
			}, null, 1000, 1000);
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
				Games.Add(player.Level.LevelId, new Game(this)
				{
					Name = player.Level.LevelId,
					Level = player.Level,
					State = GameState.Undefined
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

	public class Game
	{
		public string Name { get; set; }
		private List<Player> Players { get; set; }
		private Dictionary<Player, int> PlayerKills { get; set; }
		private Dictionary<Player, int> PlayerDeaths { get; set; }
		public int MaxPlayers { get; set; }
		public int MinPlayers { get; set; }
		public Level Level { get; set; }
		public GameState State { get; set; }
		public Timer GameTicker { get; set; }
		public Stopwatch Time { get; set; }
		public GameManager GameManager { get; set; }

		public Game(GameManager gameManager)
		{
			GameManager = gameManager;
			Players = new List<Player>();
			PlayerKills = new Dictionary<Player, int>();
			PlayerDeaths = new Dictionary<Player, int>();
			MaxPlayers = Int32.MaxValue;
			MinPlayers = 1;
			GameTicker = new Timer(Tick, this, 0, 1000);
			Time = new Stopwatch();
			Time.Start();
		}

		public void AddPlayer(Player player)
		{
			if (Players.Contains(player)) return;

			Players.Add(player);
			PlayerDeaths.Add(player, 0);
			PlayerKills.Add(player, 0);
			player.HealthManager.PlayerKilled += HealthManagerOnPlayerKilled;
		}

		public void RemovePlayer(Player player)
		{
			if (!Players.Contains(player)) return;

			Players.Remove(player);
			PlayerDeaths.Remove(player);
			PlayerKills.Remove(player);
			player.HealthManager.PlayerKilled -= HealthManagerOnPlayerKilled;
		}

		private void HealthManagerOnPlayerKilled(object sender, HealthEventArgs eventArgs)
		{
			Player targetPlayer = eventArgs.TargetEntity as Player;
			if (targetPlayer != null)
			{
				PlayerDeaths[targetPlayer]++;
			}

			Player sourcePlayer = eventArgs.TargetEntity as Player;
			if (sourcePlayer != null)
			{
				PlayerKills[sourcePlayer]++;
			}
		}

		private void Tick(object state)
		{
			if (Players.Count < MinPlayers && State == GameState.WaitingToStart)
			{
				Time.Restart();
			}
			else if (Players.Count >= MinPlayers && State == GameState.WaitingToStart && (Players.Count >= MaxPlayers || Time.ElapsedMilliseconds >= 10000))
			{
				Time.Restart();
				State = GameState.Started;

				Level.BroadcastTextMessage(string.Format("Game {0} started.", Name));
			}
			else if (State == GameState.Started && Time.ElapsedMilliseconds >= 30000)
			{
				Time.Restart();
				State = GameState.Finshed;

				Level.BroadcastTextMessage(string.Format("Game {0} finished.", Name));

				if (Players.Count > 0)
				{
					var winner = PlayerKills.OrderBy(pair => pair.Value).First();

					Level.BroadcastTextMessage(string.Format("Player {0} won game {2} with {1} kills.", winner.Key.Username, winner.Value, Name));

					foreach (var player in Players.ToArray())
					{
						GameManager.Join(player, "Default");
					}
				}
			}
			else if (State == GameState.Finshed && Time.ElapsedMilliseconds >= 10000)
			{
				Time.Restart();
				State = GameState.WaitingToStart;

				Level.BroadcastTextMessage(string.Format("New game {0} waiting to start.", Name));
			}
		}

		public int GetPlayerCount()
		{
			return Players.Count;
		}
	}

	public enum GameMode
	{
		PvP,
		Team
	}

	public enum GameState
	{
		Undefined,
		WaitingToStart,
		Started,
		Finshed,
	}

	public class GameInfo
	{
		public string Name { get; set; }
		public Type LevelType { get; set; }
		public GameMode GameMode { get; set; }
		public string Arena { get; set; }
		public bool IsTimeLimited { get; set; }
		public bool AllowRespwan { get; set; }
	}
}