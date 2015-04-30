using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using MiNET;
using MiNET.Worlds;

namespace TestPlugin.Teams
{
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
}