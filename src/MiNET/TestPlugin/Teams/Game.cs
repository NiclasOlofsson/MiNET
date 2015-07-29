using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using log4net;
using MiNET;
using MiNET.Utils;
using MiNET.Worlds;
using TestPlugin.Teams.Model;
using GameMode = TestPlugin.Teams.Model.GameMode;

namespace TestPlugin.Teams
{
	public class Game
	{
		private static readonly ILog Log = LogManager.GetLogger(typeof (Game));


		public string Name { get; set; }
		private List<Player> Players { get; set; }
		private Dictionary<Player, int> PlayerKills { get; set; }
		private Dictionary<Player, int> PlayerDeaths { get; set; }
		public Level Level { get; set; }
		public GameState State { get; set; }
		public Timer GameTicker { get; set; }
		public Stopwatch Time { get; set; }
		public GameManager GameManager { get; set; }
		public GameSettings Settings { get; set; }
		public Dictionary<int, List<Player>> Teams { get; set; }

		public Game(GameManager gameManager, GameSettings settings)
		{
			GameManager = gameManager;
			Settings = settings;
			Players = new List<Player>();
			PlayerKills = new Dictionary<Player, int>();
			PlayerDeaths = new Dictionary<Player, int>();
			GameTicker = new Timer(Tick, this, 0, 1000);
			Time = new Stopwatch();
			Time.Start();
			if (Settings.GameMode == GameMode.TvT)
			{
				Teams = new Dictionary<int, List<Player>>();
				for (int i = 0; i < Settings.NumberOfTeams; i++)
				{
					Teams.Add(i, new List<Player>());
				}
			}
		}

		public BlockCoordinates AddPlayer(Player player)
		{
			BlockCoordinates spawnPoint = Level.SpawnPoint;
			if (Players.Contains(player)) return spawnPoint;

			Players.Add(player);
			PlayerDeaths.Add(player, 0);
			PlayerKills.Add(player, 0);

			if (Settings.GameMode == GameMode.TvT && Settings.NumberOfTeams > 1)
			{
				var team = Teams.OrderBy(pair => pair.Value.Count).ThenBy(pair => pair.Key).First();
				team.Value.Add(player);
				TeamSettings teamSettings = Settings.Teams.First(ts => ts.TeamId == team.Key);
				spawnPoint = teamSettings.TeamSpawn;

				Log.InfoFormat("Added {0} to Team {1}({2}), spawn: {3}", player.Username, team.Key, teamSettings.TeamName, spawnPoint);
			}

			return spawnPoint;
		}

		public void RemovePlayer(Player player)
		{
			if (!Players.Contains(player)) return;

			Players.Remove(player);
			PlayerDeaths.Remove(player);
			PlayerKills.Remove(player);
		}

		private void Tick(object state)
		{
			if (State == GameState.WaitingToStart && Players.Count < Settings.MinPlayers)
			{
				Time.Restart();
			}
			else if (State == GameState.WaitingToStart && Players.Count >= Settings.MinPlayers && (Players.Count >= Settings.MaxPlayers || Time.ElapsedMilliseconds >= 10000))
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
					var winner = PlayerKills.OrderByDescending(pair => pair.Value).First();

					Level.BroadcastTextMessage(string.Format("Player {0} won game {2} with {1} kills.", winner.Key.Username, winner.Value, Name));
					
					foreach (var player in Players.ToArray())
					{
						GameManager.Join(player, "Default");
					}
					State= GameState.Finshed;
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

		public TeamSettings GetTeamSettingsFor(Player player)
		{
			if (Teams == null) return null;

			foreach (var team in Teams)
			{
				if (team.Value.Contains(player))
				{
					return Settings.Teams.First(ts => ts.TeamId == team.Key);
				}
			}
			return null;
		}
	}
}