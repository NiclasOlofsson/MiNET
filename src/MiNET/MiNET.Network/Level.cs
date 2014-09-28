using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Craft.Net.Anvil;
using Craft.Net.Common;

namespace MiNET.Network
{
	public class Level
	{
		private IWorldProvider _worldProvider;
		private int _viewDistanace = 96;
		private Timer _levelTicker;

		public Coordinates3D SpawnPoint { get; private set; }
		public List<Player> Players { get; private set; }
		public string LevelId { get; private set; }

		//const SURVIVAL = 0;
		//const CREATIVE = 1;
		//const ADVENTURE = 2;
		//const SPECTATOR = 3;
		public int GameMode { get; private set; }

		public Level(string levelId, IWorldProvider worldProvider = null)
		{
			SpawnPoint = new Coordinates3D(50, 10, 50);
			Players = new List<Player>();
			LevelId = levelId;
			GameMode = 1; // Creative for now
			_worldProvider = worldProvider;
		}

		public void Initialize()
		{
			if (_worldProvider == null) _worldProvider = new FlatlandWorldProvider();

			if (_worldProvider.IsCaching)
			{
				// Pre-cache chunks for spawn coordinates
				GenerateChunks(SpawnPoint.X, SpawnPoint.Y, new Dictionary<string, ChunkColumn>());
			}
			SpawnPoint = _worldProvider.GetSpawnPoint();


			if (_levelTicker != null)
			{
				_levelTicker.Stop();
			}

			_levelTicker = new Timer(200); // MC worlds tick-time
			_levelTicker.Elapsed += LevelTickerTicked;
			_levelTicker.Start();
		}

		public void AddPlayer(Player player)
		{
			Players.Add(player);
			foreach (var targetPlayer in Players)
			{
				if (targetPlayer.IsSpawned)
					targetPlayer.SendAddPlayer(player);
			}

			foreach (var targetPlayer in Players)
			{
				// Add all existing users to new player
				if (targetPlayer.IsSpawned)
					player.SendAddPlayer(targetPlayer);
			}

			BroadcastTextMessage(string.Format("Player {0} joined the game!", player.Username));
		}

		private void BroadcastTextMessage(string text)
		{
			var response = new McpeMessage
			{
				source = "",
				message = text
			};

			foreach (var player in Players)
			{
				// Should probaby encode first...
				player.SendPackage((Package) response.Clone());
			}
		}


		private void LevelTickerTicked(object sender, ElapsedEventArgs e)
		{
			// broadcast events to all players

			// Movement
			foreach (var player in Players)
			{
				if (player.IsSpawned && (DateTime.Now.Ticks - player.LastUpdatedTime.Ticks) <= 200*TimeSpan.TicksPerMillisecond)
				{
					// Has been updated since last world-tick
					BroadCastMovement(player);
				}
			}

			// New players

			// Player stuff
			// - armor
			// - held things
			// - etc..

			// Entity updates
		}

		private void BroadCastMovement(Player player)
		{
			foreach (var targetPlayer in Players)
			{
				targetPlayer.SendMovementForPlayer(player);
			}
		}

		public List<ChunkColumn> GenerateChunks(int playerX, int playerZ, Dictionary<string, ChunkColumn> chunksUsed)
		{
			Dictionary<string, double> newOrder = new Dictionary<string, double>();
			_viewDistanace = 90;
			double radiusSquared = _viewDistanace/Math.PI;
			double radius = Math.Ceiling(Math.Sqrt(radiusSquared));
			var centerX = playerX >> 4;
			var centerZ = playerZ >> 4;
			Queue<Chunk> chunkQueue = new Queue<Chunk>();
			for (double x = -radius; x <= radius; ++x)
			{
				for (double z = -radius; z <= radius; ++z)
				{
					var distance = (x*x) + (z*z);
					if (distance > radiusSquared)
					{
						continue;
					}
					var chunkX = x + centerX;
					var chunkZ = z + centerZ;
					string index = GetChunkHash(chunkX, chunkZ);

					newOrder[index] = distance;
				}
			}

			// Should be member..
			Dictionary<string, double> loadQueue = new Dictionary<string, double>();

			var sortedKeys = newOrder.Keys.ToList();
			sortedKeys.Sort();
			if (newOrder.Count > _viewDistanace)
			{
				int count = 0;
				loadQueue = new Dictionary<string, double>();
				foreach (var key in sortedKeys)
				{
					loadQueue[key] = newOrder[key];
					if (++count > _viewDistanace) break;
				}
			}
			else
			{
				loadQueue = newOrder;
			}

			List<ChunkColumn> chunks = new List<ChunkColumn>();
			foreach (var pair in loadQueue)
			{
				if (chunksUsed.ContainsKey(pair.Key)) continue;

				int x = Int32.Parse(pair.Key.Split(new[] { ':' })[0]);
				int z = Int32.Parse(pair.Key.Split(new[] { ':' })[1]);

				//ChunkColumn chunk = CraftNetGenerateChunkForIndex(x, z);
				ChunkColumn chunk = _worldProvider.GenerateChunkColumn(new Coordinates2D(x, z));
				chunks.Add(chunk);
				chunksUsed.Add(pair.Key, chunk);
			}

			return chunks;
		}

		private string GetChunkHash(double chunkX, double chunkZ)
		{
			return string.Format("{0}:{1}", chunkX, chunkZ);
		}

		public void RemovePlayer(Player player)
		{
			Players.Remove(player);
			foreach (var targetPlayer in Players)
			{
				if (targetPlayer.IsSpawned)
					targetPlayer.SendRemovePlayer(player);
			}

			BroadcastTextMessage(string.Format("Player {0} left the game!", player.Username));
		}
	}
}
