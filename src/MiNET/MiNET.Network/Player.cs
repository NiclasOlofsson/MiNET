using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using Craft.Net.Anvil;
using Craft.Net.Common;
using Craft.Net.TerrainGeneration;

namespace MiNET.Network
{
	public class Player
	{
		private readonly MiNetServer _server;
		private readonly IPEndPoint _endpoint;
		private Dictionary<string, Chunk2> _chunksUsed;

		public Player(MiNetServer server, IPEndPoint endpoint)
		{
			_server = server;
			_endpoint = endpoint;
			_chunksUsed = new Dictionary<string, Chunk2>();
		}

		public void HandlePackage(Package message)
		{
			if (typeof (IdMcpeLogin) == message.GetType())
			{
				{
					var msg = (IdMcpeLogin) message;
					msg.Decode();

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
					var chunks = GenerateChunks(playerX, playerZ);

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
								// Player joined!
								var response = new IdMcpeMessage();
								response.source = "";
								response.message = "Player joined the game!";
								response.Encode();
								SendPackage(response);
							}
						}

						count++;
					}
				}
			}
			else if (typeof (IdMcpeMovePlayer) == message.GetType())
			{
				var moveMessage = (IdMcpeMovePlayer) message;
				var chunks = GenerateChunks((int) moveMessage.x, (int) moveMessage.z);
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

		private void SendPackage(Package package)
		{
			_server.SendPackage(_endpoint, package);
		}

		public List<Chunk2> GenerateChunks(int playerX, int playerZ)
		{
			Dictionary<string, double> newOrder = new Dictionary<string, double>();
			int viewDistanace = 90;
			double radiusSquared = viewDistanace/Math.PI;
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
			if (newOrder.Count > viewDistanace)
			{
				int count = 0;
				loadQueue = new Dictionary<string, double>();
				foreach (var key in sortedKeys)
				{
					loadQueue[key] = newOrder[key];
					if (++count > viewDistanace) break;
				}
			}
			else
			{
				loadQueue = newOrder;
			}

			List<Chunk2> chunks = new List<Chunk2>();
			foreach (var pair in loadQueue)
			{
				if (_chunksUsed.ContainsKey(pair.Key)) continue;

				Chunk2 chunk = CraftNetGenerateChunkForIndex(pair.Key);
//				Chunk2 chunk = FlatlandGenerateChunkForIndex(pair.Key);
				chunks.Add(chunk);
				_chunksUsed.Add(pair.Key, chunk);
			}

			return chunks;
		}

		private Chunk2 FlatlandGenerateChunkForIndex(string index)
		{
			int x = Int32.Parse(index.Split(new[] { ':' })[0]);
			int z = Int32.Parse(index.Split(new[] { ':' })[1]);

			FlatGenerator generator = new FlatGenerator();
			Chunk2 chunk = new Chunk2();
			chunk.x = x;
			chunk.z = z;
			generator.PopulateChunk(chunk);
			return chunk;
		}

		private Chunk2 CraftNetGenerateChunkForIndex(string index)
		{
			var generator = new StandardGenerator();
			generator.Seed = 1000;
			generator.Initialize(null);

			int x = Int32.Parse(index.Split(new[] { ':' })[0]);
			int z = Int32.Parse(index.Split(new[] { ':' })[1]);

			var firstOrDefault = _server.ChunkCache.FirstOrDefault(chunk2 => chunk2 != null && chunk2.x == x && chunk2.z == z);
			if (firstOrDefault != null)
			{
				return firstOrDefault;
			}

			Chunk2 chunk;
			chunk = new Chunk2 { x = x, z = z };

			Chunk anvilChunk = generator.GenerateChunk(new Coordinates2D(x, z));

			chunk.biomeId = anvilChunk.Biomes;
			for (int i = 0; i < chunk.biomeId.Length; i++)
			{
				if (chunk.biomeId[i] > 22) chunk.biomeId[i] = 0;
			}
			if (chunk.biomeId.Length > 256) throw new Exception();

			for (int xi = 0; xi < 16; xi++)
			{
				for (int zi = 0; zi < 16; zi++)
				{
					for (int yi = 0; yi < 128; yi++)
					{
						chunk.SetBlock(xi, yi, zi, (byte) anvilChunk.GetBlockId(new Coordinates3D(xi, yi + 45, zi)));
						chunk.SetBlocklight(xi, yi, zi, anvilChunk.GetBlockLight(new Coordinates3D(xi, yi + 45, zi)));
						chunk.SetMetadata(xi, yi, zi, anvilChunk.GetMetadata(new Coordinates3D(xi, yi + 45, zi)));
						chunk.SetSkylight(xi, yi, zi, anvilChunk.GetSkyLight(new Coordinates3D(xi, yi + 45, zi)));
					}
				}
			}

			for (int i = 0; i < chunk.skylight.Length; i++)
				chunk.skylight[i] = 0xff;

			for (int i = 0; i < chunk.biomeColor.Length; i++)
				chunk.biomeColor[i] = 8761930;

			return chunk;
		}

		private string GetChunkHash(double chunkX, double chunkZ)
		{
			return string.Format("{0}:{1}", chunkX, chunkZ);
		}
	}
}
