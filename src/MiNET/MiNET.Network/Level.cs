using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Timers;
using Craft.Net.Common;
using Craft.Net.TerrainGeneration;

namespace MiNET.Network
{
	public class Level
	{
		private IWorldProvider _worldProvider;
		private int _viewDistance = 96;
		private Timer _levelTicker;

		public Coordinates3D SpawnPoint { get; private set; }
		public List<Player> Players { get; private set; } //TODO: Need to protect this, not threadsafe
		public string LevelId { get; private set; }

		//const SURVIVAL = 0;
		//const CREATIVE = 1;
		//const ADVENTURE = 2;
		//const SPECTATOR = 3;
		public int GameMode { get; private set; }
		public int CurrentWorldTime { get; private set; }
		public bool WorldTimeStarted { get; private set; }

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
			CurrentWorldTime = 6000;
			WorldTimeStarted = true;

			var loadIt = new FlatlandGenerator();

//			if (_worldProvider == null) _worldProvider = new FlatlandWorldProvider();
			if (_worldProvider == null) _worldProvider = new CraftNetAnvilWorldProvider();
			_worldProvider.Initialize();

			SpawnPoint = _worldProvider.GetSpawnPoint();

			if (_worldProvider.IsCaching)
			{
				BackgroundWorker worker = new BackgroundWorker();
				worker.RunWorkerCompleted += (sender, args) => Debug.WriteLine("Chunk-caching completed");
				worker.DoWork += delegate
				{
					// Pre-cache chunks for spawn coordinates
					foreach (var chunk in GenerateChunks(SpawnPoint.X, SpawnPoint.Z, new Dictionary<string, ChunkColumn>(), 200))
					{
					}
				};
				worker.RunWorkerAsync();
			}


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
					targetPlayer.SendAddForPlayer(player);
			}

			foreach (var targetPlayer in Players)
			{
				// Add all existing users to new player
				if (targetPlayer.IsSpawned)
					player.SendAddForPlayer(targetPlayer);
			}

			BroadcastTextMessage(string.Format("Player {0} joined the game!", player.Username), true);
		}

		public void RemovePlayer(Player player)
		{
			Players.Remove(player);
			foreach (var targetPlayer in Players)
			{
				if (targetPlayer.IsSpawned)
					targetPlayer.SendRemovePlayer(player);
			}

			BroadcastTextMessage(string.Format("Player {0} left the game!", player.Username), true);
		}

		public void BroadcastTextMessage(string text, bool isSystemMessage = false)
		{
			var response = new McpeMessage
			{
				source = "",
				message = (isSystemMessage ? "MiNET says - " : "") + text
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

			// Set time
			foreach (var player in Players)
			{
				if (player.IsSpawned)
				{
					player.SendSetTime();
				}
			}
		}

		private void BroadCastMovement(Player player)
		{
			foreach (var targetPlayer in Players)
			{
				targetPlayer.SendMovementForPlayer(player);
			}
		}

		private object syncObject = new object();

		public IEnumerable<ChunkColumn> GenerateChunks(int playerX, int playerZ, Dictionary<string, ChunkColumn> chunksUsed)
		{
			return GenerateChunks(playerX, playerZ, chunksUsed, _viewDistance);
		}

		public IEnumerable<ChunkColumn> GenerateChunks(int playerX, int playerZ, Dictionary<string, ChunkColumn> chunksUsed, int viewDistance)
		{
			lock (chunksUsed)
			{
				Dictionary<string, double> newOrders = new Dictionary<string, double>();
				double radiusSquared = viewDistance/Math.PI;
				double radius = Math.Ceiling(Math.Sqrt(radiusSquared));
				var centerX = playerX/16;
				var centerZ = playerZ/16;

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
						newOrders[index] = distance;
					}
				}

				if (newOrders.Count > viewDistance)
				{
					foreach (var pair in newOrders.OrderByDescending(pair => pair.Value))
					{
						if (newOrders.Count <= viewDistance) break;
						newOrders.Remove(pair.Key);
					}
				}


				foreach (var chunkKey in chunksUsed.Keys.ToArray())
				{
					if (!newOrders.ContainsKey(chunkKey))
					{
						chunksUsed.Remove(chunkKey);
					}
				}

				Stopwatch stopwatch = new Stopwatch();
				long avarageLoadTime = -1;
				foreach (var pair in newOrders.OrderBy(pair => pair.Value))
				{
					if (chunksUsed.ContainsKey(pair.Key)) continue;

					stopwatch.Restart();

					int x = Int32.Parse(pair.Key.Split(new[] { ':' })[0]);
					int z = Int32.Parse(pair.Key.Split(new[] { ':' })[1]);

					ChunkColumn chunk = _worldProvider.GenerateChunkColumn(new Coordinates2D(x, z));
					chunksUsed.Add(pair.Key, chunk);

					long elapsed = stopwatch.ElapsedMilliseconds;
					if (avarageLoadTime == -1) avarageLoadTime = elapsed;
					else avarageLoadTime = (avarageLoadTime + elapsed)/2;
					Debug.WriteLine("Chunk generated in: {0} ms (Avarage: {1} ms)", elapsed, avarageLoadTime);

					yield return chunk;
				}

				if(chunksUsed.Count != 96) Debug.WriteLine("Too many chunks used: {0}", chunksUsed.Count);
	
			}
		}

		private string GetChunkHash(double chunkX, double chunkZ)
		{
			return string.Format("{0}:{1}", chunkX, chunkZ);
		}


		public void RelayBroadcast(Package message)
		{
			foreach (var player in Players)
			{
				player.SendPackage((Package) message.Clone());
			}
		}

		public void RelayBroadcast(Player source, McpeAnimate message)
		{
			foreach (var player in Players)
			{
				var send = message.Clone<McpeAnimate>();
				send.entityId = player.GetEntityId(source);
				player.SendPackage(send);
			}
		}

		public void RelayBroadcast(Player source, McpePlayerArmorEquipment message)
		{
			foreach (var player in Players)
			{
				var send = message.Clone<McpePlayerArmorEquipment>();
				send.entityId = player.GetEntityId(source);
				player.SendPackage(send);
			}
		}

		public void RelayBroadcast(Player source, McpePlayerEquipment message)
		{
			foreach (var player in Players)
			{
				var send = message.Clone<McpePlayerEquipment>();
				send.entityId = player.GetEntityId(source);
				player.SendPackage(send);
			}
		}
	}
}
