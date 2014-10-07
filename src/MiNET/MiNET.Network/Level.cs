using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Timers;
using Craft.Net.Common;
using Craft.Net.TerrainGeneration;
using Timer = System.Timers.Timer;

namespace MiNET.Network
{
	public enum GameMode
	{
		/// <summary>
		///     Players fight against the enviornment, mobs, and players
		///     with limited resources.
		/// </summary>
		Survival = 0,

		/// <summary>
		///     Players are given unlimited resources, flying, and
		///     invulnerability.
		/// </summary>
		Creative = 1,

		/// <summary>
		///     Similar to survival, with the exception that players may
		///     not place or remove blocks.
		/// </summary>
		Adventure = 2,

		/// <summary>
		///     Similar to creative, with the exception that players may
		///     not place or remove blocks.
		/// </summary>
		Spectator = 3
	}


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
		public GameMode GameMode { get; private set; }
		public int CurrentWorldTime { get; private set; }
		public bool WorldTimeStarted { get; private set; }

		public Level(string levelId, IWorldProvider worldProvider = null)
		{
			SpawnPoint = new Coordinates3D(50, 10, 50);
			Players = new List<Player>();
			LevelId = levelId;
			GameMode = GameMode.Creative;
			_worldProvider = worldProvider;
		}

		public void Initialize()
		{
			CurrentWorldTime = 6000;
			WorldTimeStarted = true;

			var loadIt = new FlatlandGenerator();

			if (_worldProvider == null) _worldProvider = new FlatlandWorldProvider();
//			if (_worldProvider == null) _worldProvider = new CraftNetAnvilWorldProvider();
			_worldProvider.Initialize();

			SpawnPoint = _worldProvider.GetSpawnPoint();

			if (_worldProvider.IsCaching)
			{
				BackgroundWorker worker = new BackgroundWorker();
				worker.RunWorkerCompleted += (sender, args) => Debug.WriteLine("Chunk-caching completed");
				worker.DoWork += delegate
				{
					// Pre-cache chunks for spawn coordinates
					foreach (var chunk in GenerateChunks(SpawnPoint.X, SpawnPoint.Z, new Dictionary<string, ChunkColumn>()))
					{
					}
				};
				worker.RunWorkerAsync();
			}


			if (_levelTicker != null)
			{
				_levelTicker.Stop();
			}

			_levelTicker = new Timer(50); // MC worlds tick-time
			_levelTicker.Elapsed += LevelTickerTicked;
			_levelTicker.Start();
		}

		public void AddPlayer(Player player)
		{
			Players.Add(player);
			foreach (var targetPlayer in Players.ToArray())
			{
				if (targetPlayer.IsSpawned)
					targetPlayer.SendAddForPlayer(player);
			}

			foreach (var targetPlayer in Players.ToArray())
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
			foreach (var targetPlayer in Players.ToArray())
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

			foreach (var player in Players.ToArray())
			{
				// Should probaby encode first...
				player.SendPackage((Package) response.Clone());
			}
		}


		private object _tickSync = new object();

		private void LevelTickerTicked(object sender, ElapsedEventArgs e)
		{
			if (!Monitor.TryEnter(_tickSync))
			{
				_levelTicker.Interval += 5;
				Debug.WriteLine("Increased tick-time to {0}", _levelTicker.Interval);
				return;
			}
			else
			{
				try
				{
					CurrentWorldTime += 1;
					if (CurrentWorldTime > 24000) CurrentWorldTime = 0;

					// broadcast events to all players

					// Movement
					foreach (var player in Players.ToArray())
					{
						// Check if player has been updated since last world-tick
						if (player.IsSpawned && (DateTime.Now.Ticks - player.LastUpdatedTime.Ticks) <= _levelTicker.Interval*TimeSpan.TicksPerMillisecond)
						{
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
					if (CurrentWorldTime%20 == 0)
					{
						foreach (var player in Players.ToArray())
						{
							if (player.IsSpawned)
							{
								player.SendSetTime();
							}
						}
					}


//					if (CurrentWorldTime%2 == 0)
//					{
//						foreach (var player in Players.ToArray())
//						{
//							if (player.IsSpawned)
//							{
//								int centerX = (int) player.KnownPosition.X/16;
//								int centerZ = (int) player.KnownPosition.Z/16;

//								ChunkColumn chunk = _worldProvider.GenerateChunkColumn(new Coordinates2D(centerX, centerZ));
//								player.SendPackage(new McpeFullChunkData { chunkData = chunk.GetBytes() });
////								player.SendChunksForKnownPosition(true);
//							}
//						}
//					}

//					if (CurrentWorldTime%1 == 0)
//					{
//						foreach (var player in Players.ToArray())
//						{
//							if (player.IsSpawned)
//							{
//								player.SendPackage(new McpeUpdateBlock
//								{
//									x = (int) player.KnownPosition.X,
//									y = (byte) 127,
//									z = (int) player.KnownPosition.Z,
//									block = 7,
//									meta = 0
//								});
//							}
//						}
//					}


					if (Math.Abs(_levelTicker.Interval - 50) >= 5)
					{
						_levelTicker.Interval -= 5;
						Debug.WriteLine("Decreased tick-time to {0}", _levelTicker.Interval);
					}
				}
				finally
				{
					Monitor.Exit(_tickSync);
				}
			}
		}

		private void BroadCastMovement(Player player)
		{
			foreach (var targetPlayer in Players.ToArray())
			{
				targetPlayer.SendMovementForPlayer(player);
			}
		}

		public IEnumerable<ChunkColumn> GenerateChunks(int playerX, int playerZ, Dictionary<string, ChunkColumn> chunksUsed)
		{
			lock (chunksUsed)
			{
				Dictionary<string, double> newOrders = new Dictionary<string, double>();
				double radiusSquared = _viewDistance/Math.PI;
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

				if (newOrders.Count > _viewDistance)
				{
					foreach (var pair in newOrders.OrderByDescending(pair => pair.Value))
					{
						if (newOrders.Count <= _viewDistance) break;
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

				if (chunksUsed.Count != 96) Debug.WriteLine("Too many chunks used: {0}", chunksUsed.Count);
			}
		}

		private string GetChunkHash(double chunkX, double chunkZ)
		{
			return string.Format("{0}:{1}", chunkX, chunkZ);
		}


		public void RelayBroadcast(Player source, Package message)
		{
			foreach (var player in Players.ToArray())
			{
				if (player == source) continue;

				player.SendPackage((Package) message.Clone());
			}
		}

		public void RelayBroadcast(Player source, McpeAnimate message)
		{
			foreach (var player in Players.ToArray())
			{
				if (player == source) continue;

				var send = message.Clone<McpeAnimate>();
				send.entityId = player.GetEntityId(source);
				player.SendPackage(send);
			}
		}

		public void RelayBroadcast(Player source, McpePlayerArmorEquipment message)
		{
			foreach (var player in Players.ToArray())
			{
				if (player == source) continue;

				var send = message.Clone<McpePlayerArmorEquipment>();
				send.entityId = player.GetEntityId(source);
				player.SendPackage(send);
			}
		}

		public void RelayBroadcast(Player source, McpePlayerEquipment message)
		{
			foreach (var player in Players.ToArray())
			{
				if (player == source) continue;

				var send = message.Clone<McpePlayerEquipment>();
				send.entityId = player.GetEntityId(source);
				player.SendPackage(send);
			}
		}

		public int GetBlockId(Coordinates3D blockCoordinates)
		{
			ChunkColumn chunk = _worldProvider.GenerateChunkColumn(new Coordinates2D(blockCoordinates.X/16, blockCoordinates.Z/16));
			return chunk.GetBlock(blockCoordinates.X & 0x0f, blockCoordinates.Y & 0x7f, blockCoordinates.Z & 0x0f);
		}

		public void SetBlockId(Coordinates3D blockCoordinates, byte bid)
		{
			ChunkColumn chunk = _worldProvider.GenerateChunkColumn(new Coordinates2D(blockCoordinates.X/16, blockCoordinates.Z/16));
			chunk.SetBlock(blockCoordinates.X & 0x0f, blockCoordinates.Y & 0x7f, blockCoordinates.Z & 0x0f, bid);
		}
	}
}
