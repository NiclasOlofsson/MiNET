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
			if (typeof (McpeLogin) == message.GetType())
			{
				{
					var msg = (McpeLogin) message;

					Username = msg.username;

					var response = new McpeLoginStatus { status = 0 };
					SendPackage(response);
				}

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
			var response = new McpeStartGame();
			response.seed = 1406827239;
			response.generator = 1; //0 old, 1 infinite, 2 flat
			response.gamemode = 1;
			response.entityId = GetEntityId(this); // Always 0 for player
			response.spawnX = (int) KnownPosition.X;
			response.spawnY = (int) KnownPosition.Y;
			response.spawnZ = (int) KnownPosition.Z;
			response.x = KnownPosition.X;
			response.y = KnownPosition.Y;
			response.z = KnownPosition.Z;

			SendPackage(response);
		}

		private void SendSetSpawnPosition()
		{
			{
				var response = new McpeSetSpawnPosition();
				response.x = (int) KnownPosition.X;
				response.y = (byte) KnownPosition.Y;
				response.z = (int) KnownPosition.Z;

				SendPackage(response);
			}
		}

		private void SendChunksForKnownPosition()
		{
			{
				var chunks = _level.GenerateChunks((int) KnownPosition.X, (int) KnownPosition.Z, _chunksUsed);

				int count = 0;
				foreach (var chunk in chunks)
				{
					//if (count >= 96) break; // too big for the client to deal with

					{
						byte[] data = chunk.GetBytes();

						var response = new McpeFullChunkData();
						response.chunkData = data;

						SendPackage(response);
						Thread.Yield();
					}
					if (count == 56)
					{
						InitializePlayer();

						IsSpawned = true;
						_level.AddPlayer(this);
					}

					count++;
				}
			}
		}

		private void SendSetHealth()
		{
			{
				var response = new McpeSetHealth();
				response.health = 20;

				SendPackage(response);
			}
		}

		private void SendSetTime()
		{
			{
				// started == true ? 0x80 : 0x00);
				var response = new McpeSetTime();
				response.time = 6000;
				response.started = 0x80;

				SendPackage(response);
			}
		}

		private void InitializePlayer()
		{
			{
				//send time again
				var response = new McpeSetTime();
				response.time = 6000;
				response.started = 0x80;

				SendPackage(response);
			}

			{
				// Teleport user (MovePlayerPacket) teleport=1
				//yaw: 91
				//pitch: 28
				//bodyYaw: 91

				var response = new McpeMovePlayer();
				response.entityId = GetEntityId(this);
				response.x = KnownPosition.X;
				response.y = KnownPosition.Y;
				response.z = KnownPosition.Z;
				response.yaw = KnownPosition.Yaw;
				response.pitch = KnownPosition.Pitch;
				response.bodyYaw = KnownPosition.BodyYaw;
				response.teleport = 0x80; // true

				SendPackage(response);
			}
			{
				//$flags = 0;
				//if($this->isAdventure())
				//	$flags |= 0x01; //Do not allow placing/breaking blocks, adventure mode
				//if($nametags !== false){
				//	$flags |= 0x20; //Show Nametags
				//}

				// Adventure settings (AdventureSettingsPacket)
				var response = new McpeAdventureSettings();
				response.flags = 0x20;

				SendPackage(response);
			}

			// Settings (ContainerSetContentPacket)
			{
				//$this->inventory->sendContents($this);
				var response = new McpeContainerSetContent();
				response.windowId = 0;
				response.slotCount = 0;
				response.slotData = new byte[0];
				response.hotbarCount = 0;
				response.hotbarData = new byte[0];

				SendPackage(response);
			}

			{
				//$this->inventory->sendArmorContents($this);
				var response = new McpeContainerSetContent();
				response.windowId = 0x78; // Armor window id constant
				response.slotCount = 0;
				response.slotData = new byte[0];
				response.hotbarCount = 0;
				response.hotbarData = new byte[0];

				SendPackage(response);
			}
		}

		public void SendPackage(Package package)
		{
			_server.SendPackage(_endpoint, package);
		}

		public void SendAddPlayer(Player player)
		{
			if (player == this) return;

			var response = new McpeAddPlayer();
			response.clientId = 0;
			response.username = player.Username;
			response.entityId = GetEntityId(player);
			response.x = player.KnownPosition.X;
			response.y = player.KnownPosition.Y;
			response.z = player.KnownPosition.Z;
			response.yaw = (byte) player.KnownPosition.Yaw;
			response.pitch = (byte) player.KnownPosition.Pitch;
			response.metadata = new byte[0];

			SendPackage(response);
		}

		public void SendMovementForPlayer(Player player)
		{
			if (player == this) return;

			var knownPosition = player.KnownPosition;
			var package = new McpeMovePlayer
			{
				entityId = GetEntityId(player),
				x = knownPosition.X,
				y = knownPosition.Y,
				z = knownPosition.Z,
				yaw = knownPosition.Yaw,
				pitch = knownPosition.Pitch,
				bodyYaw = knownPosition.BodyYaw,
				teleport = 0
			};

			SendPackage(package);
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
