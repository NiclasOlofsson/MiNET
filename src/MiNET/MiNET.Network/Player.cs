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
		private string _username;
		private Level _level;

		public Player(MiNetServer server, IPEndPoint endpoint, Level level)
		{
			_server = server;
			_endpoint = endpoint;
			_level = level;
			_chunksUsed = new Dictionary<string, ChunkColumn>();
		}

		public void HandlePackage(Package message)
		{
			if (typeof (IdMcpeLogin) == message.GetType())
			{
				{
					var msg = (IdMcpeLogin) message;
					msg.Decode();

					_username = msg.username;

					var response = new IdMcpeLoginStatus();
					response.status = 0;
					response.Encode();

					SendPackage(response);
				}

				int playerX = 50;
				int playerZ = 50;
				int playerY = 5;
				int playerSpawnY = 120;

				// Start game
				{
					//const SURVIVAL = 0;
					//const CREATIVE = 1;
					//const ADVENTURE = 2;
					//const SPECTATOR = 3;

					var response = new IdMcpeStartGame();
					response.seed = 1406827239;
					response.generator = 1; //0 old, 1 infinite, 2 flat
					response.gamemode = 1;
					response.entityId = 0; // Always 0 for player
					response.spawnX = playerX;
					response.spawnY = playerSpawnY;
					response.spawnZ = playerZ;
					response.x = playerX;
					response.y = playerY;
					response.z = playerZ;
					response.Encode();

					SendPackage(response);
				}

				{
					// started == true ? 0x80 : 0x00);
					var response = new IdMcpeSetTime();
					response.time = 6000;
					response.started = 0x80;
					response.Encode();
					SendPackage(response);
				}
				{
					var response = new IdMcpeSetSpawnPosition();
					response.x = playerX;
					response.z = playerZ;
					response.y = (byte) playerSpawnY;
					response.Encode();
					SendPackage(response);
				}
				{
					var response = new IdMcpeSetHealth();
					response.health = 20;
					response.Encode();
					SendPackage(response);
				}
				{
					//	// CHUNK!

					//var generator = new FlatGenerator();
					//var chunks = generator.GenerateFlatWorld(16, 16);
					var chunks = _level.GenerateChunks(playerX, playerZ, _chunksUsed);

					int count = 0;
					foreach (var chunk in chunks)
					{
						//if (count >= 96) break; // too big for the client to deal with

						{
							byte[] data = chunk.GetBytes();

							var response = new IdMcpeFullChunkData();
							response.chunkData = data;
							response.Encode();
							SendPackage(response);
							Thread.Yield();
						}
						if (count == 56)
						{
							{
								//send time again
								var response = new IdMcpeSetTime();
								response.time = 6000;
								response.started = 0x80;
								response.Encode();
								SendPackage(response);
							}

							{
								// Teleport user (MovePlayerPacket) teleport=1
								//yaw: 91
								//pitch: 28
								//bodyYaw: 91

								var response = new IdMcpeMovePlayer();
								response.x = playerX;
								response.y = playerY;
								response.z = playerZ;
								response.yaw = 91;
								response.pitch = 28;
								response.bodyYaw = 91;
								response.teleport = 0x80; // true
								response.Encode();
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
								var response = new IdMcpeAdventureSettings();
								response.flags = 0x20;
								response.Encode();
								SendPackage(response);
							}

							// Settings (ContainerSetContentPacket)
							{
								//$this->inventory->sendContents($this);
								var response = new IdMcpeContainerSetContent();
								response.windowId = 0;
								response.slotCount = 0;
								response.slotData = new byte[0];
								response.hotbarCount = 0;
								response.hotbarData = new byte[0];
								response.Encode();
								SendPackage(response);
							}

							{
								//$this->inventory->sendArmorContents($this);
								var response = new IdMcpeContainerSetContent();
								response.windowId = 0x78; // Armor window id constant
								response.slotCount = 0;
								response.slotData = new byte[0];
								response.hotbarCount = 0;
								response.hotbarData = new byte[0];
								response.Encode();
								SendPackage(response);
							}
							{
								var response = new IdMcpeAddPlayer();
								response.clientId = 0;
								response.username = _username;
								response.entityId = 0;
								response.x = playerX;
								response.y = playerY;
								response.z = playerZ;
								response.yaw = 91;
								response.pitch = 28;
								response.metadata = new byte[0];
								response.Encode();
								BroadcastPackage(response);
							}
							{
								// Player joined!
								var response = new IdMcpeMessage();
								response.source = "";
								response.message = string.Format("Player {0} joined the game!", _username);
								response.Encode();
								BroadcastPackage(response, true);
							}
						}

						count++;
					}
				}
			}
			else if (typeof (IdMcpeMovePlayer) == message.GetType())
			{
				var moveMessage = (IdMcpeMovePlayer) message;
				var chunks = _level.GenerateChunks((int) moveMessage.x, (int) moveMessage.z, _chunksUsed);
				int count = 0;
				foreach (var chunk in chunks)
				{
					//if (count >= 96) break; // too big for the client to deal with

					{
						byte[] data = chunk.GetBytes();

						var response = new IdMcpeFullChunkData();
						response.chunkData = data;
						response.Encode();
						SendPackage(response);
						Thread.Yield();
					}
				}
			}
		}

		private void BroadcastPackage(Package package, bool toSelf = false)
		{
			_server.BroadcastPackage(this, package, toSelf: toSelf);
		}

		private void SendPackage(Package package)
		{
			_server.SendPackage(_endpoint, package);
		}
	}
}
