using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;

namespace MiNET.Network
{
	public class Player
	{
		private readonly MiNetServer _server;
		private readonly IPEndPoint _endpoint;
		private Dictionary<string, ChunkColumn> _chunksUsed;
		private Level _level;
		private List<Player> _entities;

		public DateTime LastUpdatedTime { get; private set; }
		public PlayerPosition3D KnownPosition { get; private set; }
		public bool IsSpawned { get; private set; }
		public string Username { get; private set; }

		public Player(MiNetServer server, IPEndPoint endpoint, Level level)
		{
			_server = server;
			_endpoint = endpoint;
			_level = level;
			_chunksUsed = new Dictionary<string, ChunkColumn>();
			_entities = new List<Player>();
			AddEntity(this); // Make sure we are entity with ID == 0;
			IsSpawned = false;
			KnownPosition = new PlayerPosition3D
			{
				X = _level.SpawnPoint.X,
				Y = _level.SpawnPoint.Y,
				Z = _level.SpawnPoint.Z,
				Yaw = 91,
				Pitch = 28,
				BodyYaw = 91
			};
		}


		public bool HandlePackage(Package message)
		{
			if (typeof (DisconnectionNotification) == message.GetType())
			{
				_level.RemovePlayer(this);
				return true;
			}

			if (typeof (McpeRemovePlayer) == message.GetType())
			{
				// Do nothing right now, but should clear out the entities and stuff
				// from this players internal structure.
			}

			if (typeof (McpeLogin) == message.GetType())
			{
				var msg = (McpeLogin) message;
				Username = msg.username;
				SendPackage(new McpeLoginStatus { status = 0 });

				// Start game
				SendStartGame();
				SendSetTime();
				SendSetSpawnPosition();
				SendSetHealth();
				SendChunksForKnownPosition();
				LastUpdatedTime = DateTime.Now;

				return true;
			}

			if (typeof (McpeMovePlayer) == message.GetType())
			{
				var moveMessage = (McpeMovePlayer) message;

				KnownPosition = new PlayerPosition3D(moveMessage.x, moveMessage.y, moveMessage.z) { Pitch = moveMessage.pitch, Yaw = moveMessage.yaw };
				LastUpdatedTime = DateTime.Now;

				var chunks = _level.GenerateChunks((int) KnownPosition.X, (int) KnownPosition.Z, _chunksUsed);

				foreach (var chunk in chunks)
				{
					{
						byte[] data = chunk.GetBytes();

						var response = new McpeFullChunkData { chunkData = data };
						SendPackage(response);
					}
				}

				return true;
			}

			return false;
		}

		private void SendStartGame()
		{
			SendPackage(new McpeStartGame
			{
				seed = 1406827239,
				generator = 1,
				gamemode = 1,
				entityId = GetEntityId(this),
				spawnX = (int) KnownPosition.X,
				spawnY = (int) KnownPosition.Y,
				spawnZ = (int) KnownPosition.Z,
				x = KnownPosition.X,
				y = KnownPosition.Y,
				z = KnownPosition.Z
			});
		}

		private void SendSetSpawnPosition()
		{
			SendPackage(new McpeSetSpawnPosition
			{
				x = (int) KnownPosition.X,
				y = (byte) KnownPosition.Y,
				z = (int) KnownPosition.Z
			});
		}

		private void SendChunksForKnownPosition()
		{
			List<ChunkColumn> chunks = _level.GenerateChunks((int) KnownPosition.X, (int) KnownPosition.Z, _chunksUsed);

			int count = 0;
			foreach (var chunk in chunks)
			{
				SendPackage(new McpeFullChunkData { chunkData = chunk.GetBytes() });
				Thread.Yield();

				if (count == 56 & !IsSpawned)
				{
					InitializePlayer();

					IsSpawned = true;
					_level.AddPlayer(this);
				}

				count++;
			}
		}

		private void SendSetHealth()
		{
			SendPackage(new McpeSetHealth { health = 20 });
		}

		public void SendSetTime()
		{
			// started == true ? 0x80 : 0x00);
			SendPackage(new McpeSetTime { time = _level.CurrentWorldTime, started = (byte) (_level.WorldTimeStarted ? 0x80 : 0x00) });
		}

		private void InitializePlayer()
		{
			//send time again
			SendSetTime();

			// Teleport user (MovePlayerPacket) teleport=1
			SendPackage(new McpeMovePlayer
			{
				entityId = GetEntityId(this),
				x = KnownPosition.X,
				y = KnownPosition.Y,
				z = KnownPosition.Z,
				yaw = KnownPosition.Yaw,
				pitch = KnownPosition.Pitch,
				bodyYaw = KnownPosition.BodyYaw,
				teleport = 0x80
			});

			// Adventure settings (AdventureSettingsPacket)
			//$flags = 0;
			//if($this->isAdventure())
			//	$flags |= 0x01; //Do not allow placing/breaking blocks, adventure mode
			//if($nametags !== false){
			//	$flags |= 0x20; //Show Nametags
			//}
			SendPackage(new McpeAdventureSettings { flags = 0x20 });

			// Settings (ContainerSetContentPacket)
			//$this->inventory->sendContents($this);
			SendPackage(new McpeContainerSetContent
			{
				windowId = 0,
				slotCount = 0,
				slotData = new byte[0],
				hotbarCount = 0,
				hotbarData = new byte[0]
			});

			//$this->inventory->sendArmorContents($this);
			SendPackage(new McpeContainerSetContent
			{
				windowId = 0x78,
				slotCount = 0,
				slotData = new byte[0],
				hotbarCount = 0,
				hotbarData = new byte[0]
			});
		}

		public void SendPackage(Package package)
		{
			_server.SendPackage(_endpoint, package);
		}

		public void SendAddPlayer(Player player)
		{
			if (player == this) return;

			SendPackage(new McpeAddPlayer
			{
				clientId = 0,
				username = player.Username,
				entityId = GetEntityId(player),
				x = player.KnownPosition.X,
				y = player.KnownPosition.Y,
				z = player.KnownPosition.Z,
				yaw = (byte) player.KnownPosition.Yaw,
				pitch = (byte) player.KnownPosition.Pitch,
				metadata = new byte[0]
			});
		}

		public void SendRemovePlayer(Player player)
		{
			if (player == this) return;

			SendPackage(new McpeRemovePlayer
			{
				clientId = 0,
				entityId = GetEntityId(player)
			});
		}

		public void SendMovementForPlayer(Player player)
		{
			if (player == this) return;

			var knownPosition = player.KnownPosition;

			SendPackage(new McpeMovePlayer
			{
				entityId = GetEntityId(player),
				x = knownPosition.X,
				y = knownPosition.Y,
				z = knownPosition.Z,
				yaw = knownPosition.Yaw,
				pitch = knownPosition.Pitch,
				bodyYaw = knownPosition.BodyYaw,
				teleport = 0
			});
		}

		private void AddEntity(Player player)
		{
			int entityId = _entities.IndexOf(player);
			if (entityId != -1)
			{
				// Allready exist				
				if (entityId != 0 && player == this)
				{
					// If this is the actual player, it should always be a 0
					_entities.Remove(player);
					_entities.Insert(0, player);
				}
			}
			else
			{
				_entities.Add(player);
			}
		}

		private int GetEntityId(Player player)
		{
			int entityId = _entities.IndexOf(player);
			if (entityId == -1)
			{
				AddEntity(player);
				entityId = _entities.IndexOf(player);
			}

			return entityId;
		}
	}
}
